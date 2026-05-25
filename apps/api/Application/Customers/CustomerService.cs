using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Customers;

public class CustomerService : ICustomerService
{
    private readonly IBcHttpClient _bc;

    public CustomerService(IBcHttpClient bc)
    {
        _bc = bc;
    }

    public async Task<PagedResultDto<CustomerListItemDto>> GetCustomersAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var allowedSort = new[] { "displayName", "number" };
        var sortField = allowedSort.Contains(sortBy) ? sortBy! : "displayName";
        var dir = sortDir == "desc" ? "desc" : "asc";

        var options = new BcQueryOptions
        {
            Top = pageSize,
            Skip = (page - 1) * pageSize,
            Count = true,
            OrderBy = $"{sortField} {dir}",
        };

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Escape single quotes per OData string-literal rules to avoid filter injection.
            var safe = search.Replace("'", "''");
            options.Filter = $"contains(displayName,'{safe}') or contains(number,'{safe}')";
        }

        var result = await _bc.GetCollectionAsync<BcCustomer>("customers", options, cancellationToken);

        var items = result.Value.Select(c => new CustomerListItemDto
        {
            Id = c.Id,
            Number = c.Number,
            DisplayName = c.DisplayName,
            City = c.City,
            Balance = c.Balance,
            BalanceDue = c.BalanceDue,
        }).ToList();

        return new PagedResultDto<CustomerListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = page,
            PageSize = pageSize,
        };
    }
}
