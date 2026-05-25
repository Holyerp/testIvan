namespace Pinoles.Api.Application.DTOs;

public class SalesInvoiceDetailDto
{
    public SalesInvoiceHeaderDto Header { get; set; } = new();
    public List<SalesInvoiceLineDto> Lines { get; set; } = new();
    public SalesInvoiceTotalsDto Totals { get; set; } = new();
}

public class SalesInvoiceHeaderDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BillToAddress { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;   // ISO yyyy-MM-dd
    public string DueDate { get; set; } = string.Empty;       // ISO yyyy-MM-dd
    public string PaymentTerms { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;        // OPEN | PARTIAL | PAID (SCREAMING_SNAKE wire format)
}

public class SalesInvoiceLineDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatPercent { get; set; }
    public decimal LineTotal { get; set; }
}

public class SalesInvoiceTotalsDto
{
    public decimal Subtotal { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
}
