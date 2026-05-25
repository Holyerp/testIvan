using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Reference implementation of the BC mapper pattern: maps a raw BC customer
/// entity to the list-item DTO the UI consumes. US-006+ mappers follow this shape.
/// </summary>
public class CustomerMapper : IBcMapper<BcCustomer, CustomerListItemDto>
{
    public CustomerListItemDto Map(BcCustomer source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        DisplayName = source.DisplayName,
        City = source.City,
        Balance = source.Balance,
        BalanceDue = source.BalanceDue,
    };
}
