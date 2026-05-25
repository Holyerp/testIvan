namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Raw BC purchase-invoice line as returned via the <c>purchaseInvoiceLines</c> $expand
/// navigation property. <see cref="LineAmount"/> is the line total excluding tax
/// (BC <c>amountExcludingTax</c> / <c>lineAmount</c>). Mirrors <see cref="BcSalesInvoiceLine"/>.
/// </summary>
public class BcPurchaseInvoiceLine
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatPercent { get; set; }
    public decimal LineAmount { get; set; }   // BC: amountExcludingTax / lineAmount
}
