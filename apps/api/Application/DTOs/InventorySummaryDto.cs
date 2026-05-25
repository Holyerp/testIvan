namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Inventory KPI summary (US-019). Aggregated from the warehouse `items` collection,
/// mirroring the Dashboard KPI-aggregation pattern (US-003). Optionally narrowed by a
/// location and/or category filter before aggregating.
/// </summary>
public class InventorySummaryDto
{
    public int TotalItems { get; set; }
    public decimal TotalStockValue { get; set; }   // Σ quantityOnHand * unitCost
    public int ItemsBelowMinimum { get; set; }      // count where quantityOnHand < minimumStock
}

/// <summary>
/// Stock-by-location breakdown row (US-019): per-location item count and totals, grouped
/// across every item's stock-by-location data (US-018).
/// </summary>
public class InventoryLocationDto
{
    public string Location { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
}

/// <summary>
/// Low-stock list row (US-019): an item whose quantity on hand is below its minimum stock.
/// The low-stock list is sorted by quantity on hand ascending.
/// </summary>
public class LowStockItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumStock { get; set; }
    public string Location { get; set; } = string.Empty;
}
