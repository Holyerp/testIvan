namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Item detail payload the warehouse item detail screen (US-018) consumes. Mirrors
/// <see cref="VendorDetailDto"/>: a profile block plus related lists (stock by location +
/// the most recent item ledger entries). The ledger entries are also exposed by the
/// dedicated /{id}/ledger-entries endpoint; including a capped copy here lets the screen
/// render in a single fetch.
/// </summary>
public class ItemDetailDto
{
    public ItemProfileDto Item { get; set; } = new();
    public List<StockByLocationDto> StockByLocation { get; set; } = new();
    public List<ItemLedgerEntryDto> RecentLedgerEntries { get; set; } = new();
}

/// <summary>
/// Full item profile (extends the list-item shape with unit price). <see cref="IsLowStock"/>
/// is computed by <see cref="Mapping.ItemDetailMapper"/> from QuantityOnHand &lt; MinimumStock.
/// </summary>
public class ItemProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsLowStock { get; set; }
}

/// <summary>Stock split across a single warehouse location for an item.</summary>
public class StockByLocationDto
{
    public string Location { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal QuantityReserved { get; set; }
}

/// <summary>
/// One item ledger entry (stock movement). <see cref="EntryType"/> is a cross-layer enum
/// (SCREAMING_SNAKE_CASE wire value — see <see cref="Domain.Constants.ItemLedgerEntryType"/>).
/// </summary>
public class ItemLedgerEntryDto
{
    public string Date { get; set; } = string.Empty;        // ISO yyyy-MM-dd
    public string EntryType { get; set; } = string.Empty;   // PURCHASE | SALE | ADJUSTMENT | TRANSFER
    public decimal Quantity { get; set; }                   // signed: + inbound, - outbound
    public decimal Remaining { get; set; }                  // running balance after this entry
}
