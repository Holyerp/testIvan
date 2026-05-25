using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Admin;
using Pinoles.Api.Application.Audit;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Persistence;
using Xunit;

namespace Pinoles.Api.Tests.Application.Admin;

public class UserAdminServiceTests
{
    /// <summary>Stub email service — records the last send so reset tests can assert it fired.</summary>
    private sealed class StubEmailService : IEmailService
    {
        public int SendCount { get; private set; }
        public string? LastTo { get; private set; }

        public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
        {
            SendCount++;
            LastTo = to;
            return Task.CompletedTask;
        }
    }

    private static PinolesDbContext CreateDbContext()
    {
        var opts = new DbContextOptionsBuilder<PinolesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new PinolesDbContext(opts);
    }

    private static UserAdminService CreateService(PinolesDbContext db, IEmailService? email = null)
    {
        var audit = new AuditWriter(db);
        return new UserAdminService(db, email ?? new StubEmailService(), audit, NullLogger<UserAdminService>.Instance);
    }

    private static async Task<User> SeedUser(
        PinolesDbContext db,
        string username,
        string role,
        bool isActive = true,
        string name = "Test User")
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Name = name,
            Email = $"{username}@pinoles.local",
            // cost 4 for tests (fast)
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!", 4),
            Role = role,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    private static CreateUserRequest NewUserRequest(
        string username = "newuser",
        string role = UserRoles.Manager,
        string password = "TempPass123!") =>
        new("New User", $"{username}@pinoles.local", username, role, password);

    [Fact]
    public async Task GetUsersAsync_ReturnsAll_WithoutPasswordHash()
    {
        await using var db = CreateDbContext();
        var admin = await SeedUser(db, "admin", UserRoles.Admin);
        await SeedUser(db, "manager", UserRoles.Manager);
        var service = CreateService(db);

        var users = await service.GetUsersAsync();

        Assert.Equal(2, users.Count);
        // DTO type carries no PasswordHash member at all — the list is hash-free by construction.
        Assert.All(users, u => Assert.False(string.IsNullOrEmpty(u.Username)));
        Assert.Contains(users, u => u.Username == "admin" && u.Status == UserStatus.Active);
        Assert.DoesNotContain(typeof(UserListItemDto).GetProperties(), p => p.Name == "PasswordHash");
        _ = admin;
    }

    [Fact]
    public async Task CreateUserAsync_HashesPassword_AndWritesAudit()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.CreateUserAsync(NewUserRequest(), actor.Id, actor.Username);

        Assert.True(result.Success);
        Assert.NotNull(result.User);

        var created = await db.Users.SingleAsync(u => u.Username == "newuser");
        Assert.NotEqual("TempPass123!", created.PasswordHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("TempPass123!", created.PasswordHash));

        Assert.True(await db.AuditLogs.AnyAsync(a => a.Action == AuditActions.AdminUserCreated && a.UserId == actor.Id));
    }

    [Fact]
    public async Task CreateUserAsync_InvalidRole_ReturnsValidationError()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.CreateUserAsync(NewUserRequest(role: "SUPERUSER"), actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("VALIDATION_INVALID_ROLE", result.ErrorCode);
    }

    [Fact]
    public async Task CreateUserAsync_DuplicateUsername_ReturnsConflict()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        await SeedUser(db, "taken", UserRoles.Manager);
        var service = CreateService(db);

        var result = await service.CreateUserAsync(NewUserRequest(username: "taken"), actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("CONFLICT_USERNAME_TAKEN", result.ErrorCode);
    }

    [Fact]
    public async Task CreateUserAsync_ShortPassword_ReturnsValidationError()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.CreateUserAsync(NewUserRequest(password: "short"), actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("VALIDATION_PASSWORD_TOO_SHORT", result.ErrorCode);
    }

    [Fact]
    public async Task UpdateUserAsync_ChangesRoleAndStatus_AndWritesAudit()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var target = await SeedUser(db, "manager", UserRoles.Manager);
        var service = CreateService(db);

        var result = await service.UpdateUserAsync(
            target.Id, new UpdateUserRequest(UserRoles.Accounting, false), actor.Id, actor.Username);

        Assert.True(result.Success);
        var updated = await db.Users.SingleAsync(u => u.Id == target.Id);
        Assert.Equal(UserRoles.Accounting, updated.Role);
        Assert.False(updated.IsActive);
        Assert.Equal(UserStatus.Inactive, result.User!.Status);
        Assert.True(await db.AuditLogs.AnyAsync(a => a.Action == AuditActions.AdminUserUpdated));
    }

    [Fact]
    public async Task UpdateUserAsync_NonExistent_ReturnsNotFound()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.UpdateUserAsync(
            Guid.NewGuid(), new UpdateUserRequest(UserRoles.Manager, true), actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("NOT_FOUND_USER", result.ErrorCode);
    }

    [Fact]
    public async Task UpdateUserAsync_DeactivateLastAdmin_IsBlocked()
    {
        await using var db = CreateDbContext();
        var admin = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.UpdateUserAsync(
            admin.Id, new UpdateUserRequest(UserRoles.Admin, false), admin.Id, admin.Username);

        Assert.False(result.Success);
        Assert.Equal("CONFLICT_LAST_ADMIN", result.ErrorCode);
        var unchanged = await db.Users.SingleAsync(u => u.Id == admin.Id);
        Assert.True(unchanged.IsActive);
    }

    [Fact]
    public async Task UpdateUserAsync_DeactivateAdmin_WhenAnotherAdminExists_Succeeds()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin1", UserRoles.Admin);
        var target = await SeedUser(db, "admin2", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.UpdateUserAsync(
            target.Id, new UpdateUserRequest(UserRoles.Admin, false), actor.Id, actor.Username);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task ResetPasswordAsync_ChangesHash_SendsEmail_RevokesTokens_WritesAudit()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var target = await SeedUser(db, "manager", UserRoles.Manager);
        var oldHash = target.PasswordHash;
        db.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "active-token",
            UserId = target.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
        var email = new StubEmailService();
        var service = CreateService(db, email);

        var result = await service.ResetPasswordAsync(target.Id, actor.Id, actor.Username);

        Assert.True(result.Success);
        var updated = await db.Users.SingleAsync(u => u.Id == target.Id);
        Assert.NotEqual(oldHash, updated.PasswordHash);
        Assert.Equal(1, email.SendCount);
        Assert.Equal(target.Email, email.LastTo);
        Assert.True(await db.RefreshTokens.Where(t => t.UserId == target.Id).AllAsync(t => t.IsRevoked));
        Assert.True(await db.AuditLogs.AnyAsync(a => a.Action == AuditActions.AdminPasswordReset));
    }

    [Fact]
    public async Task ResetPasswordAsync_NonExistent_ReturnsNotFound()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.ResetPasswordAsync(Guid.NewGuid(), actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("NOT_FOUND_USER", result.ErrorCode);
    }

    [Fact]
    public async Task DeleteUserAsync_OwnAccount_IsBlocked()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.DeleteUserAsync(actor.Id, actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("CONFLICT_CANNOT_DELETE_SELF", result.ErrorCode);
        Assert.True(await db.Users.AnyAsync(u => u.Id == actor.Id));
    }

    [Fact]
    public async Task DeleteUserAsync_LastAdmin_IsBlocked()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin1", UserRoles.Admin);
        var lastAdmin = await SeedUser(db, "admin2", UserRoles.Admin);
        // Deactivate the actor so the target is the only ACTIVE admin.
        actor.IsActive = false;
        await db.SaveChangesAsync();
        var service = CreateService(db);

        var result = await service.DeleteUserAsync(lastAdmin.Id, actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("CONFLICT_LAST_ADMIN", result.ErrorCode);
        Assert.True(await db.Users.AnyAsync(u => u.Id == lastAdmin.Id));
    }

    [Fact]
    public async Task DeleteUserAsync_NonExistent_ReturnsNotFound()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var service = CreateService(db);

        var result = await service.DeleteUserAsync(Guid.NewGuid(), actor.Id, actor.Username);

        Assert.False(result.Success);
        Assert.Equal("NOT_FOUND_USER", result.ErrorCode);
    }

    [Fact]
    public async Task DeleteUserAsync_Valid_DeletesAndWritesAudit()
    {
        await using var db = CreateDbContext();
        var actor = await SeedUser(db, "admin", UserRoles.Admin);
        var target = await SeedUser(db, "manager", UserRoles.Manager);
        var service = CreateService(db);

        var result = await service.DeleteUserAsync(target.Id, actor.Id, actor.Username);

        Assert.True(result.Success);
        Assert.False(await db.Users.AnyAsync(u => u.Id == target.Id));
        Assert.True(await db.AuditLogs.AnyAsync(
            a => a.Action == AuditActions.AdminUserDeleted && a.UserId == actor.Id));
    }
}
