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
}
