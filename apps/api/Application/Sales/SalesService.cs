using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Sales;

/// <summary>
/// Serves the three sales-invoice list collections (open / posted / credit memos).
/// The <c>entitySet</c> parameter selects which BC collection to read so the three
/// endpoints share one implementation (DRY). Uses <see cref="BcListQuery"/> for
/// paging/sort/filter and <see cref="SalesInvoiceMapper"/> for BC -> DTO mapping.
/// </summary>
public class SalesService : ISalesService
{
    // BC field names the BcListQuery allow-list validates against (first entry is the default).
    private static readonly string[] AllowedSortFields = { "postingDate", "dueDate", "totalAmountIncludingTax" };

    private readonly IBcHttpClient _bc;
    private readonly IBcMapper<BcSalesInvoice, SalesInvoiceListItemDto> _mapper;

    public SalesService(
        IBcHttpClient bc,
        IBcMapper<BcSalesInvoice, SalesInvoiceListItemDto> mapper)
    {
        _bc = bc;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<SalesInvoiceListItemDto>> GetInvoicesAsync(
        string entitySet,
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? status,
        string? fromDate,
        string? toDate,
        CancellationToken ct = default)
    {
        // Translate the UI sort key into the BC field name before allow-list validation.
        var mappedSortBy = MapSortField(sortBy);

        var options = BcListQuery.Build(
            page,
            pageSize,
            mappedSortBy,
            sortDir,
            AllowedSortFields,
            search,
            term => $"contains(number,'{term}') or contains(customerName,'{term}')");

        // Compose the additional status / date-range predicates onto whatever filter
        // BcListQuery built from the search term. Single quotes are escaped per OData rules.
        options.Filter = CombineFilters(options.Filter, status, fromDate, toDate);

        var result = await _bc.GetCollectionAsync<BcSalesInvoice>(entitySet, options, ct);

        var items = result.Value.Select(_mapper.Map).ToList();

        var effectivePageSize = options.Top ?? BcListQuery.DefaultPageSize;
        var effectivePage = (options.Skip ?? 0) / effectivePageSize + 1;

        return new PagedResultDto<SalesInvoiceListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = effectivePage,
            PageSize = effectivePageSize,
        };
    }

    private static string? MapSortField(string? uiSortBy) => uiSortBy switch
    {
        "date" => "postingDate",
        "dueDate" => "dueDate",
        "amount" => "totalAmountIncludingTax",
        // Already a BC field name (or null) — pass through; allow-list rejects anything invalid.
        _ => uiSortBy,
    };

    /// <summary>
    /// AND-combine the search filter (if any) with optional status and posting-date-range
    /// predicates. Status is normalized to a BC-style value; date bounds use OData ge/le.
    /// </summary>
    private static string? CombineFilters(string? searchFilter, string? status, string? fromDate, string? toDate)
    {
        var clauses = new List<string>();

        if (!string.IsNullOrWhiteSpace(searchFilter))
            clauses.Add($"({searchFilter})");

        if (!string.IsNullOrWhiteSpace(status))
        {
            var safeStatus = status.Replace("'", "''");
            clauses.Add($"status eq '{safeStatus}'");
        }

        if (!string.IsNullOrWhiteSpace(fromDate))
        {
            var safeFrom = fromDate.Replace("'", "''");
            clauses.Add($"postingDate ge '{safeFrom}'");
        }

        if (!string.IsNullOrWhiteSpace(toDate))
        {
            var safeTo = toDate.Replace("'", "''");
            clauses.Add($"postingDate le '{safeTo}'");
        }

        return clauses.Count > 0 ? string.Join(" and ", clauses) : null;
    }
}
