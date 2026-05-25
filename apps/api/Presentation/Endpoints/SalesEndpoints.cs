using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class SalesEndpoints
{
    public static void MapSalesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/sales")
            .WithTags("Sales")
            .RequireAuthorization("RequireFinancial");

        group.MapGet("/invoices", (
            ISalesService sales, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(sales, "salesInvoices", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        group.MapGet("/posted-invoices", (
            ISalesService sales, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(sales, "salesInvoicesPosted", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        group.MapGet("/credit-memos", (
            ISalesService sales, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(sales, "salesCreditMemos", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));
    }

    private static async Task<IResult> List(
        ISalesService sales,
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
            var result = await sales.GetInvoicesAsync(
                entitySet, page ?? 1, pageSize ?? 20, search, sortBy, sortDir, status, fromDate, toDate, ct);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch sales invoices", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
