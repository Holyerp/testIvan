namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Grouped universal-search result. Each group holds at most the requested limit of
/// <see cref="SearchHitDto"/> entries. Empty groups are returned (not omitted) so the
/// shape is stable for the frontend. WAREHOUSE-only callers receive all-empty groups
/// because they have no access to the financial entities (see SearchService).
/// </summary>
public class SearchResultsDto
{
    public List<SearchHitDto> Customers { get; set; } = new();
    public List<SearchHitDto> Vendors { get; set; } = new();
    public List<SearchHitDto> SalesInvoices { get; set; } = new();
    public List<SearchHitDto> PurchaseInvoices { get; set; } = new();
}

/// <summary>
/// A single cross-entity search hit. <see cref="Type"/> is a cross-layer enum whose
/// wire value is SCREAMING_SNAKE_CASE — one of CUSTOMER | VENDOR | SALES_INVOICE |
/// PURCHASE_INVOICE — which the frontend maps to a detail route and an i18n group label.
/// </summary>
public class SearchHitDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;     // CUSTOMER | VENDOR | SALES_INVOICE | PURCHASE_INVOICE
    public string Title { get; set; } = string.Empty;     // e.g. customer name / invoice number
    public string Subtitle { get; set; } = string.Empty;  // e.g. number / customer name / vendor name
}
