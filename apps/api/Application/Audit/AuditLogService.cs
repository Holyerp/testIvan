using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Persistence;

namespace Pinoles.Api.Application.Audit;

/// <summary>
/// EF Core-backed read view over the <c>audit_logs</c> table (US-023). Filters by category
/// (mapped to an action-code prefix/substring), username (case-insensitive contains), and a
/// CreatedAt date range; orders newest-first; pages via Skip/Take with a total count.
/// </summary>
public class AuditLogService : IAuditLogService
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    // Matches the "(id=<value>...)" fragment the US-021 audit writers embed in Details.
    private static readonly Regex IdPattern = new(@"id=([^,)\s]+)", RegexOptions.Compiled);

    private readonly PinolesDbContext _db;

    public AuditLogService(PinolesDbContext db) => _db = db;

    public async Task<PagedResultDto<AuditLogEntryDto>> GetAuditLogAsync(
        int page,
        int pageSize,
        string? category,
        string? username,
        string? fromDate,
        string? toDate,
        CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        IQueryable<AuditLog> query = _db.AuditLogs.AsNoTracking();

        // Category -> action-code condition (prefix/substring). Unknown category = no filter.
        switch (category)
        {
            case AuditActionCategory.Login:
                query = query.Where(a => a.Action.StartsWith("AUTH_LOGIN"));
                break;
            case AuditActionCategory.Export:
                query = query.Where(a => a.Action.Contains("EXPORT"));
                break;
            case AuditActionCategory.View:
                query = query.Where(a => a.Action.Contains("VIEW"));
                break;
            case AuditActionCategory.Admin:
                query = query.Where(a => a.Action.StartsWith("ADMIN_"));
                break;
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            var term = username.Trim().ToLower();
            query = query.Where(a => a.Username != null && a.Username.ToLower().Contains(term));
        }

        if (TryParseDate(fromDate, out var from))
            query = query.Where(a => a.CreatedAt >= from);

        if (TryParseDate(toDate, out var to))
        {
            // Inclusive of the whole "to" day when a bare date is supplied.
            var toEnd = to.TimeOfDay == TimeSpan.Zero ? to.AddDays(1).AddTicks(-1) : to;
            query = query.Where(a => a.CreatedAt <= toEnd);
        }

        var total = await query.CountAsync(ct);

        var rows = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResultDto<AuditLogEntryDto>
        {
            Items = rows.Select(ToDto).ToList(),
            Total = total,
            Page = page,
            PageSize = pageSize,
        };
    }

    private static AuditLogEntryDto ToDto(AuditLog a)
    {
        var (entityType, entityId) = ParseEntity(a.Action, a.Details);
        return new AuditLogEntryDto
        {
            Id = a.Id.ToString(),
            Timestamp = a.CreatedAt.ToString("o"),
            Username = a.Username,
            Action = a.Action,
            Category = AuditActionCategory.Categorize(a.Action),
            EntityType = entityType,
            EntityId = entityId,
            Details = a.Details,
        };
    }

    /// <summary>
    /// Best-effort entity extraction. Entity TYPE is derived from the action code's domain
    /// segment (e.g. ADMIN_USER_CREATED -&gt; USER); entity ID is the "id=..." token in Details
    /// when present. Both are null when nothing can be inferred — kept simple by design.
    /// </summary>
    private static (string? type, string? id) ParseEntity(string action, string? details)
    {
        string? type = action switch
        {
            _ when action.Contains("USER") => "USER",
            _ when action.Contains("PASSWORD") => "USER",
            _ => null,
        };

        string? id = null;
        if (!string.IsNullOrEmpty(details))
        {
            var match = IdPattern.Match(details);
            if (match.Success)
                id = match.Groups[1].Value;
        }

        return (type, id);
    }

    private static bool TryParseDate(string? value, out DateTime result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (DateTime.TryParse(
                value,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal,
                out var parsed))
        {
            result = parsed;
            return true;
        }

        return false;
    }
}
