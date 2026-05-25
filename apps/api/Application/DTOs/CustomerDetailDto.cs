namespace Pinoles.Api.Application.DTOs;

public class CustomerDetailDto
{
    public CustomerProfileDto Customer { get; set; } = new();
    public List<CustomerInvoiceDto> Invoices { get; set; } = new();
}

public class CustomerProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal BalanceDue { get; set; }
}

public class CustomerInvoiceDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;
    public decimal TotalAmountIncludingTax { get; set; }
    public string Status { get; set; } = string.Empty;
}
