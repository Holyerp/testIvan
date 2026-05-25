namespace Pinoles.Api.Application.DTOs;

public class PurchaseInvoiceListItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;   // ISO yyyy-MM-dd
    public string DueDate { get; set; } = string.Empty;       // ISO yyyy-MM-dd
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;        // OPEN | PARTIAL | PAID (SCREAMING_SNAKE wire format)
}
