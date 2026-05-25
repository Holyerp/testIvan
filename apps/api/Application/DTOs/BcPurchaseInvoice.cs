namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Raw BC purchase-invoice / purchase-credit-memo entity. Mirrors
/// <see cref="BcSalesInvoice"/> but carries a vendor instead of a customer.
/// </summary>
public class BcPurchaseInvoice
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public decimal TotalAmountIncludingTax { get; set; }
    public string Status { get; set; } = string.Empty;
}
