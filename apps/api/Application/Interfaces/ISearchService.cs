using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface ISearchService
{
    /// <summary>
    /// Run a universal cross-entity search and return grouped hits. The caller's
    /// <paramref name="roles"/> gate which groups are populated: callers without a
    /// financial role (ADMIN/MANAGER/ACCOUNTING) — i.e. WAREHOUSE-only — receive all
    /// groups empty, because the four entity types are financial data they may not see.
    /// </summary>
    Task<SearchResultsDto> SearchAsync(
        string query,
        int limit,
        IEnumerable<string> roles,
        CancellationToken ct = default);
}
