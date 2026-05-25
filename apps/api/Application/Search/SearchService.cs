using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;

namespace Pinoles.Api.Application.Search;

/// <summary>
/// Universal search: aggregates the four existing list services (customers, vendors,
/// sales invoices, purchase invoices) for a single query term. Reuses each service's
/// search path rather than re-querying BC ad-hoc (DRY) so OData filter escaping and
/// paging stay in one place.
///
/// RBAC: the four entity types are financial data. WAREHOUSE has no access to them
/// (mirrors the RequireFinancial gating on the list endpoints), so a caller without a
/// financial role gets all-empty groups and BC is never queried for that caller.
/// </summary>
public class SearchService : ISearchService
{
    private const int MinLimit = 1;
    private const int MaxLimit = 20;

    // Roles permitted to see the financial entities (matches the RequireFinancial policy).
    private static readonly string[] FinancialRoles =
    {
        UserRoles.Admin, UserRoles.Manager, UserRoles.Accounting,
    };

    private readonly ICustomerService _customers;
    private readonly IVendorService _vendors;
    private readonly ISalesService _sales;
    private readonly IPurchaseService _purchase;

    public SearchService(
        ICustomerService customers,
        IVendorService vendors,
        ISalesService sales,
        IPurchaseService purchase)
    {
        _customers = customers;
        _vendors = vendors;
        _sales = sales;
        _purchase = purchase;
    }

    public async Task<SearchResultsDto> SearchAsync(
        string query,
        int limit,
        IEnumerable<string> roles,
        CancellationToken ct = default)
    {
        var results = new SearchResultsDto();

        // Below the minimum length, or for a caller that may not see financial data,
        // return empty groups without touching BC.
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2) return results;
        if (!HasFinancialAccess(roles)) return results;

        var clampedLimit = Math.Clamp(limit, MinLimit, MaxLimit);

        // Sequential awaits — the mock BC client is not designed for concurrent calls,
        // and the query count is small (four). Each list service already escapes the
        // OData filter value, so the search term is passed through unmodified.
        var customers = await _customers.GetCustomersAsync(1, clampedLimit, query, null, null, ct);
        var vendors = await _vendors.GetVendorsAsync(1, clampedLimit, query, null, null, ct);
        var salesInvoices = await _sales.GetInvoicesAsync(
            "salesInvoices", 1, clampedLimit, query, null, null, null, null, null, ct);
        var purchaseInvoices = await _purchase.GetInvoicesAsync(
            "purchaseInvoices", 1, clampedLimit, query, null, null, null, null, null, ct);

        results.Customers = customers.Items.Select(MapCustomer).ToList();
        results.Vendors = vendors.Items.Select(MapVendor).ToList();
        results.SalesInvoices = salesInvoices.Items.Select(MapSalesInvoice).ToList();
        results.PurchaseInvoices = purchaseInvoices.Items.Select(MapPurchaseInvoice).ToList();

        return results;
    }

    private static bool HasFinancialAccess(IEnumerable<string> roles) =>
        roles.Any(r => FinancialRoles.Contains(r, StringComparer.Ordinal));

    private static SearchHitDto MapCustomer(CustomerListItemDto c) => new()
    {
        Id = c.Id,
        Type = "CUSTOMER",
        Title = c.DisplayName,
        Subtitle = c.Number,
    };

    private static SearchHitDto MapVendor(VendorListItemDto v) => new()
    {
        Id = v.Id,
        Type = "VENDOR",
        Title = v.DisplayName,
        Subtitle = v.Number,
    };

    private static SearchHitDto MapSalesInvoice(SalesInvoiceListItemDto i) => new()
    {
        Id = i.Id,
        Type = "SALES_INVOICE",
        Title = i.Number,
        Subtitle = i.CustomerName,
    };

    private static SearchHitDto MapPurchaseInvoice(PurchaseInvoiceListItemDto i) => new()
    {
        Id = i.Id,
        Type = "PURCHASE_INVOICE",
        Title = i.Number,
        Subtitle = i.VendorName,
    };
}
