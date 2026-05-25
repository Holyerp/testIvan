using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Read-only view over the LOCAL <c>audit_logs</c> table (US-023). Returns a paginated,
/// newest-first slice with optional category / username / date-range filters. ADMIN-only —
/// enforced by the endpoint's <c>RequireAdmin</c> policy. Business Central is never touched.
/// </summary>
public interface IAuditLogService
{
    Task<PagedResultDto<AuditLogEntryDto>> GetAuditLogAsync(
        int page,
        int pageSize,
        string? category,
        string? username,
        string? fromDate,
        string? toDate,
        CancellationToken ct = default);
}
