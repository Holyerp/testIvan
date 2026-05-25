namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Item list-item DTO the warehouse item list (US-017) consumes. <see cref="IsLowStock"/>
/// is computed by <see cref="Mapping.ItemMapper"/> from QuantityOnHand &lt; MinimumStock so
/// the UI can render the low-stock warning badge without re-deriving the threshold.
/// </summary>
public class ItemListItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsLowStock { get; set; }      // QuantityOnHand < MinimumStock (computed in mapper)
}
