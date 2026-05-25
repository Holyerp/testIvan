using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Purchase;

/// <summary>
/// Serves the three purchase-invoice list collections (open / posted / credit memos).
/// The <c>entitySet</c> parameter selects which BC collection to read so the three
/// endpoints share one implementation (DRY). Uses <see cref="BcListQuery"/> for
/// paging/sort/filter and <see cref="PurchaseInvoiceMapper"/> for BC -> DTO mapping.
/// Mirrors <see cref="Sales.SalesService"/> but reads vendor-bearing collections.
/// </summary>
public class PurchaseService : IPurchaseService
{
    // BC field names the BcListQuery allow-list validates against (first entry is the default).
    private static readonly string[] AllowedSortFields = { "postingDate", "dueDate", "totalAmountIncludingTax" };

    // Navigation property expanded for the detail view to pull line items in one call.
    private const string LinesExpand = "purchaseInvoiceLines";

    // BC collections whose records are credit memos. Credit memos use OPEN | POSTED
    // status semantics, so the status is re-normalized via InvoiceStatus.NormalizeCreditMemo
    // after the shared mapper runs. Everything else (paging, sort, filter) is identical.
    private static readonly HashSet<string> CreditMemoEntitySets = new(StringComparer.Ordinal)
    {
        "purchaseCreditMemos",
    };

    private readonly IBcHttpClient _bc;
    private readonly IBcMapper<BcPurchaseInvoice, PurchaseInvoiceListItemDto> _mapper;
    private readonly IBcMapper<BcPurchaseInvoice, PurchaseInvoiceDetailDto> _detailMapper;

    public PurchaseService(
        IBcHttpClient bc,
        IBcMapper<BcPurchaseInvoice, PurchaseInvoiceListItemDto> mapper,
        IBcMapper<BcPurchaseInvoice, PurchaseInvoiceDetailDto> detailMapper)
    {
        _bc = bc;
        _mapper = mapper;
        _detailMapper = detailMapper;
    }

    public async Task<PagedResultDto<PurchaseInvoiceListItemDto>> GetInvoicesAsync(
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
            term => $"contains(number,'{term}') or contains(vendorName,'{term}')");

        // Compose the additional status / date-range predicates onto whatever filter
        // BcListQuery built from the search term. Single quotes are escaped per OData rules.
        options.Filter = CombineFilters(options.Filter, status, fromDate, toDate);

        var result = await _bc.GetCollectionAsync<BcPurchaseInvoice>(entitySet, options, ct);

        var items = result.Value.Select(_mapper.Map).ToList();

        // Credit memos use OPEN | POSTED semantics; re-normalize the status so a
        // credit memo can never surface a PARTIAL/PAID invoice status.
        if (CreditMemoEntitySets.Contains(entitySet))
            foreach (var item in items)
                item.Status = InvoiceStatus.NormalizeCreditMemo(item.Status);

        var effectivePageSize = options.Top ?? BcListQuery.DefaultPageSize;
        var effectivePage = (options.Skip ?? 0) / effectivePageSize + 1;

        return new PagedResultDto<PurchaseInvoiceListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = effectivePage,
            PageSize = effectivePageSize,
        };
    }

    public async Task<PurchaseInvoiceDetailDto?> GetInvoiceByIdAsync(
        string entitySet,
        string id,
        CancellationToken ct = default)
    {
        // $expand the line items so header + lines arrive in a single BC request.
        var invoice = await _bc.GetByIdAsync<BcPurchaseInvoice>(
            entitySet, id, new BcQueryOptions { Expand = LinesExpand }, ct);

        if (invoice == null || string.IsNullOrEmpty(invoice.Id)) return null;

        return _detailMapper.Map(invoice);
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
