using Pinoles.Api.Application.Admin;

namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Admin user-management (US-021): CRUD over the LOCAL <c>users</c> table plus password
/// reset. Every mutation writes an audit-log row. Business Central data is never touched —
/// these are portal accounts only. The <c>actor*</c> parameters identify the acting admin
/// (from the JWT) for audit attribution and the self-delete guard.
/// </summary>
public interface IUserAdminService
{
    Task<IReadOnlyList<UserListItemDto>> GetUsersAsync(CancellationToken ct = default);

    Task<UserAdminResult> CreateUserAsync(
        CreateUserRequest request,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default);

    Task<UserAdminResult> UpdateUserAsync(
        Guid id,
        UpdateUserRequest request,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default);

    Task<UserAdminResult> ResetPasswordAsync(
        Guid id,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default);

    Task<UserAdminResult> DeleteUserAsync(
        Guid id,
        Guid actorUserId,
        string actorUsername,
        CancellationToken ct = default);
}
