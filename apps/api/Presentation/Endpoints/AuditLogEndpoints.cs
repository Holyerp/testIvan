using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

/// <summary>
/// Audit-log view endpoint (US-023). ADMIN-only (<c>RequireAdmin</c> policy); other roles get
/// 403. Reads the LOCAL audit_logs table only — BC is never touched, so the 502 path does not
/// apply. Canonical envelope: <c>{ success, data: PagedResultDto&lt;AuditLogEntryDto&gt; }</c>.
/// </summary>
public static class AuditLogEndpoints
{
    public static void MapAuditLogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/admin/audit-log")
            .WithTags("AuditLog")
            .RequireAuthorization("RequireAdmin");

        group.MapGet("/", GetAuditLog);
    }

    internal static async Task<IResult> GetAuditLog(
        IAuditLogService service,
        CancellationToken ct,
        int page = 1,
        int pageSize = 20,
        string? category = null,
        string? username = null,
        string? fromDate = null,
        string? toDate = null)
    {
        try
        {
            var result = await service.GetAuditLogAsync(
                page, pageSize, category, username, fromDate, toDate, ct);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Internal server error", code = "INTERNAL_ERROR" },
                statusCode: 500);
        }
    }
}
