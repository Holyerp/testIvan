using Microsoft.Extensions.Options;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Inventory;

/// <summary>
/// Inventory stock-overview service (US-019). Blends the Dashboard KPI-aggregation
/// pattern (US-003: fetch a BC collection, compute summary numbers, 5-min cache) with
/// the warehouse item / stock-by-location data (US-017 / US-018). Reads the `items`
/// collection (and each item's stock-by-location navigation) and aggregates in memory —
/// the mock returns a manageable set, mirroring how <see cref="Dashboard.DashboardService"/>
/// aggregates over the fetched collections. First WAREHOUSE-module summary service.
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly IBcHttpClient _bc;
    private readonly ICacheService _cache;
    private readonly BcOptions _bcOptions;

    public InventoryService(IBcHttpClient bc, ICacheService cache, IOptions<BcOptions> bcOptions)
    {
        _bc = bc;
        _cache = cache;
        _bcOptions = bcOptions.Value;
    }

    public async Task<InventorySummaryDto> GetSummaryAsync(
        string? location,
        string? category,
        CancellationToken cancellationToken = default)
    {
        // Cache key varies per filter combination so a filtered summary never serves an
        // unfiltered (or differently-filtered) cached value. Mirrors DashboardService's
        // 5-min cache window (BcOptions.CacheSeconds).
        var cacheKey = $"inventory:summary:{location ?? string.Empty}:{category ?? string.Empty}";
        var cached = await _cache.GetAsync<InventorySummaryDto>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var items = await FetchItemsAsync(location, category, cancellationToken);

        var summary = new InventorySummaryDto
        {
            TotalItems = items.Count,
            TotalStockValue = items.Sum(i => i.QuantityOnHand * i.UnitCost),
            ItemsBelowMinimum = items.Count(i => i.QuantityOnHand < i.MinimumStock),
        };

        await _cache.SetAsync(
            cacheKey, summary, TimeSpan.FromSeconds(_bcOptions.CacheSeconds), cancellationToken);
        return summary;
    }

    public async Task<List<InventoryLocationDto>> GetByLocationAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await FetchItemsAsync(null, null, cancellationToken);

        // Accumulate per-location item count, total quantity, and total value. Each item's
        // value is allocated to a location proportionally to the quantity held there, so
        // the per-location values sum back to the overall stock value (US-018 reconciles
        // stock-by-location quantities with the item total).
        var accumulators = new Dictionary<string, LocationAccumulator>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            var rows = await GetStockRowsAsync(item, cancellationToken);
            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.Location)) continue;
                if (!accumulators.TryGetValue(row.Location, out var acc))
                {
                    acc = new LocationAccumulator { Location = row.Location };
                    accumulators[row.Location] = acc;
                }
                acc.ItemCount++;
                acc.TotalQuantity += row.QuantityOnHand;
                acc.TotalValue += row.QuantityOnHand * item.UnitCost;
            }
        }

        return accumulators.Values
            .OrderBy(a => a.Location, StringComparer.OrdinalIgnoreCase)
            .Select(a => new InventoryLocationDto
            {
                Location = a.Location,
                ItemCount = a.ItemCount,
                TotalQuantity = a.TotalQuantity,
                TotalValue = a.TotalValue,
            })
            .ToList();
    }

    public async Task<List<LowStockItemDto>> GetLowStockAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await FetchItemsAsync(null, null, cancellationToken);

        return items
            .Where(i => i.QuantityOnHand < i.MinimumStock)
            .OrderBy(i => i.QuantityOnHand)
            .Select(i => new LowStockItemDto
            {
                Id = i.Id,
                Number = i.Number,
                Description = i.Description,
                QuantityOnHand = i.QuantityOnHand,
                MinimumStock = i.MinimumStock,
                Location = i.Location,
            })
            .ToList();
    }

    // Fetch the warehouse items collection, optionally narrowed by location/category.
    // Filter values are escaped per OData string-literal rules to avoid filter injection,
    // matching ItemService.GetItemsAsync.
    private async Task<List<BcItem>> FetchItemsAsync(
        string? location, string? category, CancellationToken cancellationToken)
    {
        var filters = new List<string>();
        if (!string.IsNullOrWhiteSpace(category))
            filters.Add($"category eq '{category.Replace("'", "''")}'");
        if (!string.IsNullOrWhiteSpace(location))
            filters.Add($"location eq '{location.Replace("'", "''")}'");

        var options = new BcQueryOptions
        {
            Top = 1000, // single page — the warehouse catalogue is small
            Filter = filters.Count > 0 ? string.Join(" and ", filters) : null,
        };

        var result = await _bc.GetCollectionAsync<BcItem>("items", options, cancellationToken);
        return result.Value;
    }

    // Resolve an item's stock-by-location rows. The list query does not expand the
    // navigation (US-018 attaches it only via GetByIdAsync), so fetch the item by id.
    // Falls back to a single row at the item's primary location if none are present.
    private async Task<List<BcStockByLocation>> GetStockRowsAsync(
        BcItem item, CancellationToken cancellationToken)
    {
        var detail = await _bc.GetByIdAsync<BcItem>(
            "items", item.Id, new BcQueryOptions { Expand = "stockByLocation" }, cancellationToken);

        var rows = detail?.StockByLocation;
        if (rows != null && rows.Count > 0) return rows;

        return new List<BcStockByLocation>
        {
            new() { Location = item.Location, QuantityOnHand = item.QuantityOnHand },
        };
    }

    private sealed class LocationAccumulator
    {
        public string Location { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}
