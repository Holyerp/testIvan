using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;

namespace Pinoles.Api.Presentation.Endpoints;

public static class AnalyticsEndpoints
{
    public static void MapAnalyticsEndpoints(this WebApplication app)
    {
        // FINANCIAL module (US-020): ADMIN / MANAGER / ACCOUNTING. WAREHOUSE → 403.
        var group = app.MapGroup("/api/v1/analytics")
            .WithTags("Analytics")
            .RequireAuthorization("RequireFinancial");

        group.MapGet("/revenue-expense", GetRevenueExpense);
        group.MapGet("/top-customers", GetTopCustomers);
        group.MapGet("/top-items", GetTopItems);
    }

    internal static async Task<IResult> GetRevenueExpense(
        IAnalyticsService analytics,
        string? granularity,
        string? fromDate,
        string? toDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var series = await analytics.GetRevenueExpenseAsync(
                granularity ?? Granularity.Monthly, fromDate, toDate, cancellationToken);
            return Results.Ok(new { success = true, data = series });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch analytics data", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    internal static async Task<IResult> GetTopCustomers(
        IAnalyticsService analytics,
        int? top,
        string? fromDate,
        string? toDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var customers = await analytics.GetTopCustomersAsync(top ?? 10, fromDate, toDate, cancellationToken);
            return Results.Ok(new { success = true, data = customers });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch top customers", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    internal static async Task<IResult> GetTopItems(
        IAnalyticsService analytics,
        int? top,
        string? fromDate,
        string? toDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var items = await analytics.GetTopItemsAsync(top ?? 10, fromDate, toDate, cancellationToken);
            return Results.Ok(new { success = true, data = items });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch top items", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
