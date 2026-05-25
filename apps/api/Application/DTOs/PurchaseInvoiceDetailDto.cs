namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Purchase-invoice detail DTO consumed by the UI (US-010). Mirrors
/// <see cref="SalesInvoiceDetailDto"/> but carries a vendor-oriented header.
/// </summary>
public class PurchaseInvoiceDetailDto
{
    public PurchaseInvoiceHeaderDto Header { get; set; } = new();
    public List<PurchaseInvoiceLineDto> Lines { get; set; } = new();
    public PurchaseInvoiceTotalsDto Totals { get; set; } = new();
}

public class PurchaseInvoiceHeaderDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;   // ISO yyyy-MM-dd
    public string DueDate { get; set; } = string.Empty;       // ISO yyyy-MM-dd
    public string PaymentTerms { get; set; } = string.Empty;
    public string OurReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;        // OPEN | PARTIAL | PAID (SCREAMING_SNAKE wire format)
}

public class PurchaseInvoiceLineDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatPercent { get; set; }
    public decimal LineTotal { get; set; }
}

public class PurchaseInvoiceTotalsDto
{
    public decimal Subtotal { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
}
