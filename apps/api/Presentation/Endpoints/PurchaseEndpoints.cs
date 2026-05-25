using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class PurchaseEndpoints
{
    public static void MapPurchaseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/purchase")
            .WithTags("Purchase")
            .RequireAuthorization("RequireFinancial");

        group.MapGet("/invoices", (
            IPurchaseService purchase, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(purchase, "purchaseInvoices", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        group.MapGet("/posted-invoices", (
            IPurchaseService purchase, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(purchase, "purchaseInvoicesPosted", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        group.MapGet("/credit-memos", (
            IPurchaseService purchase, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(purchase, "purchaseCreditMemos", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        // Advance (proforma) purchase invoices (US-015). The list reuses the shared List
        // handler over the purchaseAdvanceInvoices collection (same PurchaseInvoiceListItemDto
        // columns, vendor side); the detail uses a dedicated handler that returns the
        // payment-tracking block. See Q-003 note in docs/api/purchase-advance.md (standard
        // BC schema pending client confirmation).
        group.MapGet("/advance-invoices", (
            IPurchaseService purchase, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(purchase, "purchaseAdvanceInvoices", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        group.MapGet("/advance-invoices/{id}", (IPurchaseService purchase, string id, CancellationToken ct) =>
            AdvanceDetail(purchase, id, ct));

        group.MapGet("/invoices/{id}", (IPurchaseService purchase, string id, CancellationToken ct) =>
            Detail(purchase, "purchaseInvoices", id, NotFoundPurchaseInvoice, "Purchase invoice not found", ct));

        group.MapGet("/posted-invoices/{id}", (IPurchaseService purchase, string id, CancellationToken ct) =>
            Detail(purchase, "purchaseInvoicesPosted", id, NotFoundPurchaseInvoice, "Purchase invoice not found", ct));
    }

    private const string NotFoundPurchaseInvoice = "NOT_FOUND_PURCHASE_INVOICE";
    private const string NotFoundPurchaseAdvanceInvoice = "NOT_FOUND_PURCHASE_ADVANCE_INVOICE";

    private static async Task<IResult> List(
        IPurchaseService purchase,
        string entitySet,
        int? page,
        int? pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? status,
        string? fromDate,
        string? toDate,
        CancellationToken ct)
    {
        try
        {
            var result = await purchase.GetInvoicesAsync(
                entitySet, page ?? 1, pageSize ?? 20, search, sortBy, sortDir, status, fromDate, toDate, ct);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch purchase invoices", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    // Shared detail handler. The not-found code + message are parameterized to mirror
    // SalesEndpoints; both purchase detail routes return NOT_FOUND_PURCHASE_INVOICE.
    private static async Task<IResult> Detail(
        IPurchaseService purchase,
        string entitySet,
        string id,
        string notFoundCode,
        string notFoundMessage,
        CancellationToken ct)
    {
        try
        {
            var detail = await purchase.GetInvoiceByIdAsync(entitySet, id, ct);
            if (detail == null)
            {
                return Results.Json(
                    new { success = false, error = notFoundMessage, code = notFoundCode },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = detail });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch purchase document", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    // Advance-invoice detail handler (US-015). Mirrors Detail but calls the
    // advance-specific service method (which adds the payment-tracking block) and
    // returns NOT_FOUND_PURCHASE_ADVANCE_INVOICE on an unknown id.
    private static async Task<IResult> AdvanceDetail(IPurchaseService purchase, string id, CancellationToken ct)
    {
        try
        {
            var detail = await purchase.GetAdvanceInvoiceByIdAsync(id, ct);
            if (detail == null)
            {
                return Results.Json(
                    new { success = false, error = "Purchase advance invoice not found", code = NotFoundPurchaseAdvanceInvoice },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = detail });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch purchase advance invoice", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
