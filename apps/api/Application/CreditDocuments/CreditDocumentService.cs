using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.CreditDocuments;

/// <summary>
/// Serves the unified credit-documents list + detail (US-016) over the single BC
/// <c>creditDocuments</c> collection (credit memos + debit memos + storno together).
/// Mirrors <see cref="Sales.SalesService"/> but adds a <c>type</c> filter and carries
/// the original-invoice reference. Uses <see cref="BcListQuery"/> for paging/sort/filter
/// and <see cref="CreditDocumentMapper"/> / <see cref="CreditDocumentDetailMapper"/> for
/// BC -> DTO mapping.
/// </summary>
public class CreditDocumentService : ICreditDocumentService
{
    // BC field names the BcListQuery allow-list validates against (first entry is the default).
    private static readonly string[] AllowedSortFields = { "postingDate", "number", "totalAmountIncludingTax" };

    // Single unified collection mixing all three correction document types.
    private const string EntitySet = "creditDocuments";

    // Navigation property expanded for the detail view to pull line items in one call
    // (reuses the sales-invoice line schema — DRY).
    private const string LinesExpand = "salesInvoiceLines";

    private readonly IBcHttpClient _bc;
    private readonly IBcMapper<BcCreditDocument, CreditDocumentListItemDto> _mapper;
    private readonly IBcMapper<BcCreditDocument, CreditDocumentDetailDto> _detailMapper;

    public CreditDocumentService(
        IBcHttpClient bc,
        IBcMapper<BcCreditDocument, CreditDocumentListItemDto> mapper,
        IBcMapper<BcCreditDocument, CreditDocumentDetailDto> detailMapper)
    {
        _bc = bc;
        _mapper = mapper;
        _detailMapper = detailMapper;
    }

    public async Task<PagedResultDto<CreditDocumentListItemDto>> GetCreditDocumentsAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? type,
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
            term => $"contains(number,'{term}') or contains(partyName,'{term}')");

        // Compose the additional type / date-range predicates onto whatever filter
        // BcListQuery built from the search term. Single quotes are escaped per OData rules.
        options.Filter = CombineFilters(options.Filter, type, fromDate, toDate);

        var result = await _bc.GetCollectionAsync<BcCreditDocument>(EntitySet, options, ct);

        var items = result.Value.Select(_mapper.Map).ToList();

        var effectivePageSize = options.Top ?? BcListQuery.DefaultPageSize;
        var effectivePage = (options.Skip ?? 0) / effectivePageSize + 1;

        return new PagedResultDto<CreditDocumentListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = effectivePage,
            PageSize = effectivePageSize,
        };
    }

    public async Task<CreditDocumentDetailDto?> GetCreditDocumentByIdAsync(
        string id,
        CancellationToken ct = default)
    {
        // $expand the line items so header + lines arrive in a single BC request.
        var document = await _bc.GetByIdAsync<BcCreditDocument>(
            EntitySet, id, new BcQueryOptions { Expand = LinesExpand }, ct);

        if (document == null || string.IsNullOrEmpty(document.Id)) return null;

        return _detailMapper.Map(document);
    }

    private static string? MapSortField(string? uiSortBy) => uiSortBy switch
    {
        "date" => "postingDate",
        "number" => "number",
        "amount" => "totalAmountIncludingTax",
        // Already a BC field name (or null) — pass through; allow-list rejects anything invalid.
        _ => uiSortBy,
    };

    /// <summary>
    /// AND-combine the search filter (if any) with optional document-type and
    /// posting-date-range predicates. The type is validated against the allowed wire
    /// values (CREDIT_MEMO | DEBIT_MEMO | STORNO) and silently ignored when invalid so a
    /// bad value never produces an injectable filter; date bounds use OData ge/le.
    /// </summary>
    private static string? CombineFilters(string? searchFilter, string? type, string? fromDate, string? toDate)
    {
        var clauses = new List<string>();

        if (!string.IsNullOrWhiteSpace(searchFilter))
            clauses.Add($"({searchFilter})");

        if (!string.IsNullOrWhiteSpace(type))
        {
            var normalizedType = type.Trim().ToUpperInvariant();
            // Only emit a type predicate for a known wire value (defends against injection
            // and stray query params). Unknown types match nothing meaningful, so skip.
            if (CreditDocumentType.IsValid(normalizedType))
                clauses.Add($"type eq '{normalizedType}'");
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
