namespace Pinoles.Api.Application.DTOs;

public class CustomerListItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal BalanceDue { get; set; }
}
