using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Persistence;

namespace Pinoles.Api.Application.Admin;

/// <summary>
/// Admin user-management service. All writes target the LOCAL <c>users</c> table; every
/// mutation stages an audit-log row through <see cref="IAuditWriter"/> and commits it in the
/// same SaveChanges as the mutation. Passwords are bcrypt-hashed at cost 12 and never logged
/// or returned in plaintext.
/// </summary>
public class UserAdminService : IUserAdminService
{
    private const int BcryptCost = 12;

    private readonly PinolesDbContext _db;
    private readonly IEmailService _email;
    private readonly IAuditWriter _audit;
    private readonly ILogger<UserAdminService> _logger;

    public UserAdminService(
        PinolesDbContext db,
        IEmailService email,
        IAuditWriter audit,
        ILogger<UserAdminService> logger)
    {
        _db = db;
        _email = email;
        _audit = audit;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UserListItemDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _db.Users
            .OrderBy(u => u.Name)
            .ThenBy(u => u.Username)
            .ToListAsync(ct);

        return users.Select(ToDto).ToList();
    }

    public async Task<UserAdminResult> CreateUserAsync(
        CreateUserRequest request,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name)
            || string.IsNullOrWhiteSpace(request.Username)
            || string.IsNullOrWhiteSpace(request.TempPassword))
            return new UserAdminResult(false, ErrorCode: "VALIDATION_REQUIRED_FIELDS");

        if (request.TempPassword.Length < 8)
            return new UserAdminResult(false, ErrorCode: "VALIDATION_PASSWORD_TOO_SHORT");

        if (!UserRoles.IsValid(request.Role))
            return new UserAdminResult(false, ErrorCode: "VALIDATION_INVALID_ROLE");

        var usernameTaken = await _db.Users.AnyAsync(u => u.Username == request.Username, ct);
        if (usernameTaken)
            return new UserAdminResult(false, ErrorCode: "CONFLICT_USERNAME_TAKEN");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailTaken = await _db.Users.AnyAsync(u => u.Email == request.Email, ct);
            if (emailTaken)
                return new UserAdminResult(false, ErrorCode: "CONFLICT_EMAIL_TAKEN");
        }

        var now = DateTime.UtcNow;
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Username = request.Username.Trim(),
            Role = request.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.TempPassword, BcryptCost),
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
        };
        _db.Users.Add(user);

        _audit.Write(
            AuditActions.AdminUserCreated,
            actorUserId,
            actorUsername,
            $"Created user {user.Username} (id={user.Id}, role={user.Role})");

        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("admin.user.created actorId={ActorId} targetId={TargetId}", actorUserId, user.Id);

        return new UserAdminResult(true, ToDto(user));
    }

    public async Task<UserAdminResult> UpdateUserAsync(
        Guid id,
        UpdateUserRequest request,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default)
    {
        if (!UserRoles.IsValid(request.Role))
            return new UserAdminResult(false, ErrorCode: "VALIDATION_INVALID_ROLE");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user == null)
            return new UserAdminResult(false, ErrorCode: "NOT_FOUND_USER");

        // Guard: do not strip the last active admin (by deactivating OR by demoting it).
        var wouldRemainAdmin = request.Role == UserRoles.Admin && request.IsActive;
        if (!wouldRemainAdmin && await IsLastActiveAdmin(user, ct))
            return new UserAdminResult(false, ErrorCode: "CONFLICT_LAST_ADMIN");

        var previous = $"role={user.Role}, active={user.IsActive}";
        user.Role = request.Role;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _audit.Write(
            AuditActions.AdminUserUpdated,
            actorUserId,
            actorUsername,
            $"Updated user {user.Username} (id={user.Id}): {previous} -> role={user.Role}, active={user.IsActive}");

        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("admin.user.updated actorId={ActorId} targetId={TargetId}", actorUserId, user.Id);

        return new UserAdminResult(true, ToDto(user));
    }

    public async Task<UserAdminResult> ResetPasswordAsync(
        Guid id,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default)
    {
        var user = await _db.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user == null)
            return new UserAdminResult(false, ErrorCode: "NOT_FOUND_USER");

        var tempPassword = GenerateTempPassword();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword, BcryptCost);
        user.UpdatedAt = DateTime.UtcNow;

        // Invalidate active sessions so the old credentials can no longer refresh.
        foreach (var token in user.RefreshTokens.Where(t => !t.IsRevoked))
            token.IsRevoked = true;

        // No-op email service in dev; production wires real SMTP. NEVER log the plaintext password.
        await _email.SendAsync(
            user.Email ?? user.Username,
            "Password reset",
            $"Your Pinoles password has been reset. Temporary password: {tempPassword}",
            ct);

        _audit.Write(
            AuditActions.AdminPasswordReset,
            actorUserId,
            actorUsername,
            $"Reset password for user {user.Username} (id={user.Id})");

        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("admin.user.password_reset actorId={ActorId} targetId={TargetId}", actorUserId, user.Id);

        return new UserAdminResult(true, ToDto(user));
    }

    public async Task<UserAdminResult> DeleteUserAsync(
        Guid id,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default)
    {
        if (id == actorUserId)
            return new UserAdminResult(false, ErrorCode: "CONFLICT_CANNOT_DELETE_SELF");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user == null)
            return new UserAdminResult(false, ErrorCode: "NOT_FOUND_USER");

        if (await IsLastActiveAdmin(user, ct))
            return new UserAdminResult(false, ErrorCode: "CONFLICT_LAST_ADMIN");

        _db.Users.Remove(user);

        _audit.Write(
            AuditActions.AdminUserDeleted,
            actorUserId,
            actorUsername,
            $"Deleted user {user.Username} (id={user.Id}, role={user.Role})");

        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("admin.user.deleted actorId={ActorId} targetId={TargetId}", actorUserId, user.Id);

        return new UserAdminResult(true);
    }

    /// <summary>True when the given user is an active ADMIN and no other active admin exists.</summary>
    private async Task<bool> IsLastActiveAdmin(User user, CancellationToken ct)
    {
        if (user.Role != UserRoles.Admin || !user.IsActive)
            return false;

        var otherActiveAdmins = await _db.Users.CountAsync(
            u => u.Id != user.Id && u.Role == UserRoles.Admin && u.IsActive, ct);
        return otherActiveAdmins == 0;
    }

    private static string GenerateTempPassword()
    {
        // 18 url-safe characters from a CSPRNG — well above the 8-char minimum.
        var bytes = RandomNumberGenerator.GetBytes(18);
        return Convert.ToBase64String(bytes)
            .Replace('+', 'A')
            .Replace('/', 'B')
            .Replace('=', 'C');
    }

    private static UserListItemDto ToDto(User u) => new(
        u.Id.ToString(),
        u.Name,
        u.Email,
        u.Username,
        u.Role,
        UserStatus.From(u.IsActive),
        u.LastLoginAt,
        u.CreatedAt);
}
