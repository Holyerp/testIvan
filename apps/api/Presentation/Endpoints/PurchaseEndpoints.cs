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

        group.MapGet("/invoices/{id}", (IPurchaseService purchase, string id, CancellationToken ct) =>
            Detail(purchase, "purchaseInvoices", id, NotFoundPurchaseInvoice, "Purchase invoice not found", ct));

        group.MapGet("/posted-invoices/{id}", (IPurchaseService purchase, string id, CancellationToken ct) =>
            Detail(purchase, "purchaseInvoicesPosted", id, NotFoundPurchaseInvoice, "Purchase invoice not found", ct));
    }

    private const string NotFoundPurchaseInvoice = "NOT_FOUND_PURCHASE_INVOICE";

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
}
