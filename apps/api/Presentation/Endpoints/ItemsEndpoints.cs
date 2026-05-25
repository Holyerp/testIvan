using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class ItemsEndpoints
{
    public static void MapItemsEndpoints(this WebApplication app)
    {
        // WAREHOUSE module (T-004): ADMIN / MANAGER / WAREHOUSE. ACCOUNTING → 403.
        var group = app.MapGroup("/api/v1/items")
            .WithTags("Items")
            .RequireAuthorization("RequireWarehouse");

        group.MapGet("/", GetItems);
    }

    internal static async Task<IResult> GetItems(
        IItemService items,
        int? page,
        int? pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? category,
        string? location,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await items.GetItemsAsync(
                page ?? 1, pageSize ?? 20, search, sortBy, sortDir, category, location, cancellationToken);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch items", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
