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
        group.MapGet("/{id}", GetItemById);
        group.MapGet("/{id}/ledger-entries", GetItemLedgerEntries);
    }

    private const string NotFoundItem = "NOT_FOUND_ITEM";

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

    internal static async Task<IResult> GetItemById(
        string id,
        IItemService items,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await items.GetItemByIdAsync(id, cancellationToken);
            if (detail == null)
            {
                return Results.Json(
                    new { success = false, error = "Item not found", code = NotFoundItem },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = detail });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch item", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    internal static async Task<IResult> GetItemLedgerEntries(
        string id,
        IItemService items,
        CancellationToken cancellationToken)
    {
        try
        {
            var entries = await items.GetItemLedgerEntriesAsync(id, 20, cancellationToken);
            if (entries == null)
            {
                return Results.Json(
                    new { success = false, error = "Item not found", code = NotFoundItem },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = entries });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch item ledger entries", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
