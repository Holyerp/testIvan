namespace Pinoles.Api.Domain.Constants;

/// <summary>
/// Audit-log action codes. Wire format is SCREAMING_SNAKE_CASE per the cross-layer
/// enum convention; these are the values stored in <c>AuditLog.Action</c> and surfaced
/// by the audit-log view (US-023).
/// </summary>
public static class AuditActions
{
    public const string AuthLoginSuccess = "AUTH_LOGIN_SUCCESS";
    public const string AdminUserCreated = "ADMIN_USER_CREATED";
    public const string AdminUserUpdated = "ADMIN_USER_UPDATED";
    public const string AdminUserDeleted = "ADMIN_USER_DELETED";
    public const string AdminPasswordReset = "ADMIN_PASSWORD_RESET";
}
