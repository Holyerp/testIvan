namespace Pinoles.Api.Application.DTOs;

public class BcSalesInvoice
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public decimal TotalAmountIncludingTax { get; set; }
    public string Status { get; set; } = string.Empty;
}
