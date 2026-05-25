namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Writes a single audit-log row. Centralizes audit persistence so callers (admin
/// user management, auth) do not duplicate the AuditLog construction. The row is added
/// to the current unit of work; the caller's <c>SaveChangesAsync</c> commits it.
/// </summary>
public interface IAuditWriter
{
    /// <summary>
    /// Stage an audit entry. <paramref name="actorUserId"/>/<paramref name="actorUsername"/>
    /// identify who performed the action; <paramref name="details"/> describes the target
    /// and the change (NEVER passwords or other secrets).
    /// </summary>
    void Write(
        string action,
        Guid? actorUserId,
        string? actorUsername,
        string? details = null,
        string? ipAddress = null);
}
