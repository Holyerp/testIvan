using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Domain.Constants;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC item entity to the full item profile DTO the warehouse detail screen
/// (US-018) consumes. Mirrors <see cref="ItemMapper"/> but adds UnitPrice and is the
/// detail-tier projection. The low-stock flag (QuantityOnHand &lt; MinimumStock) is computed
/// here so the threshold lives in one place (DRY with the list mapper's intent).
/// </summary>
public class ItemDetailMapper : IBcMapper<BcItem, ItemProfileDto>
{
    public ItemProfileDto Map(BcItem source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        Description = source.Description,
        Category = source.Category,
        UnitOfMeasure = source.UnitOfMeasure,
        QuantityOnHand = source.QuantityOnHand,
        MinimumStock = source.MinimumStock,
        UnitCost = source.UnitCost,
        UnitPrice = source.UnitPrice,
        IsLowStock = source.QuantityOnHand < source.MinimumStock,
    };
}

/// <summary>Maps a raw BC stock-by-location row to its UI DTO (US-018).</summary>
public class StockByLocationMapper : IBcMapper<BcStockByLocation, StockByLocationDto>
{
    public StockByLocationDto Map(BcStockByLocation source) => new()
    {
        Location = source.Location,
        QuantityOnHand = source.QuantityOnHand,
        QuantityReserved = source.QuantityReserved,
    };
}

/// <summary>
/// Maps a raw BC item ledger entry to its UI DTO (US-018). Normalizes the BC entry-type
/// casing to the SCREAMING_SNAKE wire value via <see cref="ItemLedgerEntryType.Normalize"/>.
/// </summary>
public class ItemLedgerEntryMapper : IBcMapper<BcItemLedgerEntry, ItemLedgerEntryDto>
{
    public ItemLedgerEntryDto Map(BcItemLedgerEntry source) => new()
    {
        Date = source.Date,
        EntryType = ItemLedgerEntryType.Normalize(source.EntryType),
        Quantity = source.Quantity,
        Remaining = source.Remaining,
    };
}
