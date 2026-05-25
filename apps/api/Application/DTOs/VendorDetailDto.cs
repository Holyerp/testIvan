namespace Pinoles.Api.Application.DTOs;

public class VendorDetailDto
{
    public VendorProfileDto Vendor { get; set; } = new();

    // Reuse the purchase list-item DTO for the history rows (DRY) — last 20 posted
    // purchase invoices for this vendor.
    public List<PurchaseInvoiceListItemDto> Invoices { get; set; } = new();
}

public class VendorProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string VatNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string PaymentTerms { get; set; } = string.Empty;
}
