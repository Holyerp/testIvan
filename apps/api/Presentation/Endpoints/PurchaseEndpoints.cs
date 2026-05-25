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
    }

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
}
