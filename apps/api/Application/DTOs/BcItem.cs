namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Raw BC item (product / inventory) entity. Carries the warehouse stock fields the
/// item list (US-017) renders. Mirrors <see cref="BcVendor"/> as the closest list-shaped
/// BC entity, but belongs to the WAREHOUSE module rather than the financial one.
/// </summary>
public class BcItem
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;        // primary/default location for list view
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }

    // Detail-only navigation (US-018): populated by GetByIdAsync (mock attaches it,
    // real BC via $expand). Empty for the list query, so the list mapper ignores it.
    public List<BcStockByLocation> StockByLocation { get; set; } = new();
}

/// <summary>
/// Raw BC stock-by-location row for an item (US-018). Carried on <see cref="BcItem"/> as
/// an expand navigation. quantityOnHand summed across rows reconciles with the item total.
/// </summary>
public class BcStockByLocation
{
    public string Location { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal QuantityReserved { get; set; }
}

/// <summary>
/// Raw BC item ledger entry (stock movement, US-018). entryType uses BC casing and is
/// normalized to the SCREAMING_SNAKE wire value by the mapper
/// (<see cref="Domain.Constants.ItemLedgerEntryType"/>).
/// </summary>
public class BcItemLedgerEntry
{
    public string Date { get; set; } = string.Empty;        // ISO yyyy-MM-dd
    public string EntryType { get; set; } = string.Empty;   // raw BC casing
    public decimal Quantity { get; set; }                   // signed
    public decimal Remaining { get; set; }                  // running balance
}
