using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Common;

/// <summary>
/// Builds a <see cref="BcQueryOptions"/> for a paginated/sorted/filtered list request.
/// Centralizes page/pageSize clamping, sort allow-list enforcement, and OData filter
/// composition so every Phase 2 list service shares one implementation.
/// </summary>
public static class BcListQuery
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    /// <summary>
    /// Build query options from raw request inputs.
    /// </summary>
    /// <param name="page">Requested page (clamped to >= 1).</param>
    /// <param name="pageSize">Requested page size (reset to default if &lt; 1 or &gt; max).</param>
    /// <param name="sortBy">Requested sort field; rejected to the default if not in the allow-list.</param>
    /// <param name="sortDir">"asc" or "desc"; anything else defaults to "asc".</param>
    /// <param name="allowedSortFields">Whitelist of sortable fields; first entry is the default.</param>
    /// <param name="search">Optional search term.</param>
    /// <param name="filterBuilder">
    /// Builds the OData $filter from an already-escaped search term. Called only when
    /// <paramref name="search"/> is non-empty. The term passed in has single quotes escaped.
    /// </param>
    public static BcQueryOptions Build(
        int page,
        int pageSize,
        string? sortBy,
        string? sortDir,
        IReadOnlyList<string> allowedSortFields,
        string? search = null,
        Func<string, string>? filterBuilder = null)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > MaxPageSize ? DefaultPageSize : pageSize;

        var defaultSort = allowedSortFields.Count > 0 ? allowedSortFields[0] : null;
        var sortField = sortBy != null && allowedSortFields.Contains(sortBy) ? sortBy : defaultSort;
        var dir = sortDir == "desc" ? "desc" : "asc";

        var options = new BcQueryOptions
        {
            Top = pageSize,
            Skip = (page - 1) * pageSize,
            Count = true,
            OrderBy = sortField != null ? $"{sortField} {dir}" : null,
        };

        if (!string.IsNullOrWhiteSpace(search) && filterBuilder != null)
        {
            // Escape single quotes per OData string-literal rules to avoid filter injection.
            var safe = search.Replace("'", "''");
            options.Filter = filterBuilder(safe);
        }

        return options;
    }
}
