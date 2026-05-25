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

        group.MapGet("/posted-credit-memos", (
            ISalesService sales, int? page, int? pageSize, string? search,
            string? sortBy, string? sortDir, string? status, string? fromDate, string? toDate,
            CancellationToken ct) =>
            List(sales, "salesCreditMemosPosted", page, pageSize, search, sortBy, sortDir, status, fromDate, toDate, ct));

        group.MapGet("/invoices/{id}", (ISalesService sales, string id, CancellationToken ct) =>
            Detail(sales, "salesInvoices", id, NotFoundSalesInvoice, "Sales invoice not found", ct));

        group.MapGet("/posted-invoices/{id}", (ISalesService sales, string id, CancellationToken ct) =>
            Detail(sales, "salesInvoicesPosted", id, NotFoundSalesInvoice, "Sales invoice not found", ct));

        group.MapGet("/credit-memos/{id}", (ISalesService sales, string id, CancellationToken ct) =>
            Detail(sales, "salesCreditMemos", id, NotFoundSalesCreditMemo, "Sales credit memo not found", ct));
    }

    private const string NotFoundSalesInvoice = "NOT_FOUND_SALES_INVOICE";
    private const string NotFoundSalesCreditMemo = "NOT_FOUND_SALES_CREDIT_MEMO";

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

    // Shared detail handler. The not-found code + message are parameterized so credit
    // memos return NOT_FOUND_SALES_CREDIT_MEMO while invoices keep NOT_FOUND_SALES_INVOICE.
    private static async Task<IResult> Detail(
        ISalesService sales,
        string entitySet,
        string id,
        string notFoundCode,
        string notFoundMessage,
        CancellationToken ct)
    {
        try
        {
            var detail = await sales.GetInvoiceByIdAsync(entitySet, id, ct);
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
                new { success = false, error = "Failed to fetch sales document", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
