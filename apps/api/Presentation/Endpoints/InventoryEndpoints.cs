using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this WebApplication app)
    {
        // WAREHOUSE module (US-019): ADMIN / MANAGER / WAREHOUSE. ACCOUNTING → 403.
        var group = app.MapGroup("/api/v1/inventory")
            .WithTags("Inventory")
            .RequireAuthorization("RequireWarehouse");

        group.MapGet("/summary", GetSummary);
        group.MapGet("/locations", GetByLocation);
        group.MapGet("/low-stock", GetLowStock);
    }

    internal static async Task<IResult> GetSummary(
        IInventoryService inventory,
        string? location,
        string? category,
        CancellationToken cancellationToken)
    {
        try
        {
            var summary = await inventory.GetSummaryAsync(location, category, cancellationToken);
            return Results.Ok(new { success = true, data = summary });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch inventory summary", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    internal static async Task<IResult> GetByLocation(
        IInventoryService inventory,
        CancellationToken cancellationToken)
    {
        try
        {
            var locations = await inventory.GetByLocationAsync(cancellationToken);
            return Results.Ok(new { success = true, data = locations });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch stock by location", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    internal static async Task<IResult> GetLowStock(
        IInventoryService inventory,
        CancellationToken cancellationToken)
    {
        try
        {
            var items = await inventory.GetLowStockAsync(cancellationToken);
            return Results.Ok(new { success = true, data = items });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch low-stock items", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
