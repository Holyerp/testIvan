using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC item entity to the item list-item DTO the warehouse UI consumes.
/// Mirrors <see cref="VendorMapper"/> (the reference list-mapper). Computes the
/// low-stock flag (QuantityOnHand &lt; MinimumStock) here so the threshold lives in one
/// place rather than being re-derived on the client.
/// </summary>
public class ItemMapper : IBcMapper<BcItem, ItemListItemDto>
{
    public ItemListItemDto Map(BcItem source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        Description = source.Description,
        Category = source.Category,
        UnitOfMeasure = source.UnitOfMeasure,
        QuantityOnHand = source.QuantityOnHand,
        MinimumStock = source.MinimumStock,
        UnitCost = source.UnitCost,
        IsLowStock = source.QuantityOnHand < source.MinimumStock,
    };
}
