using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface IInventoryService
{
    // Inventory KPI summary, optionally narrowed by location and/or category before
    // aggregating. Cached for the BC cache window (5 min) per filter combination.
    Task<InventorySummaryDto> GetSummaryAsync(
        string? location,
        string? category,
        CancellationToken cancellationToken = default);

    // Stock-by-location breakdown grouped across every item's per-location stock.
    Task<List<InventoryLocationDto>> GetByLocationAsync(CancellationToken cancellationToken = default);

    // Items below their minimum stock, sorted by quantity on hand ascending.
    Task<List<LowStockItemDto>> GetLowStockAsync(CancellationToken cancellationToken = default);
}
