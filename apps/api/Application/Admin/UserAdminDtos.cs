namespace Pinoles.Api.Application.Admin;

/// <summary>
/// A user row as rendered on the admin User Management screen. Never exposes the
/// password hash. <see cref="Status"/> is the SCREAMING_SNAKE wire value derived from
/// <c>User.IsActive</c> (ACTIVE | INACTIVE).
/// </summary>
public record UserListItemDto(
    string Id,
    string Name,
    string? Email,
    string Username,
    string Role,
    string Status,
    DateTime? LastLoginAt,
    DateTime CreatedAt);

/// <summary>Create-user request body. <see cref="TempPassword"/> is hashed (bcrypt) and never stored or logged in plaintext.</summary>
public record CreateUserRequest(
    string Name,
    string? Email,
    string Username,
    string Role,
    string TempPassword);

/// <summary>Update-user request body — only role and active status are mutable here.</summary>
public record UpdateUserRequest(
    string Role,
    bool IsActive);

/// <summary>User status wire values (ACTIVE | INACTIVE).</summary>
public static class UserStatus
{
    public const string Active = "ACTIVE";
    public const string Inactive = "INACTIVE";

    public static string From(bool isActive) => isActive ? Active : Inactive;
}

/// <summary>
/// Outcome of an admin user-management operation. Mirrors the <c>LoginResult</c> pattern:
/// a success flag plus an optional SCREAMING_SNAKE error code that the endpoint maps to an
/// HTTP status. <see cref="User"/> carries the resulting row on success where relevant.
/// </summary>
public record UserAdminResult(
    bool Success,
    UserListItemDto? User = null,
    string? ErrorCode = null);
