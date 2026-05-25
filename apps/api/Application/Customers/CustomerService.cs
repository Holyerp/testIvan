using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Customers;

public class CustomerService : ICustomerService
{
    private static readonly string[] AllowedSortFields = { "displayName", "number" };

    private readonly IBcHttpClient _bc;
    private readonly IBcMapper<BcCustomer, CustomerListItemDto> _customerMapper;

    public CustomerService(
        IBcHttpClient bc,
        IBcMapper<BcCustomer, CustomerListItemDto> customerMapper)
    {
        _bc = bc;
        _customerMapper = customerMapper;
    }

    public async Task<PagedResultDto<CustomerListItemDto>> GetCustomersAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken = default)
    {
        var options = BcListQuery.Build(
            page,
            pageSize,
            sortBy,
            sortDir,
            AllowedSortFields,
            search,
            term => $"contains(displayName,'{term}') or contains(number,'{term}')");

        var result = await _bc.GetCollectionAsync<BcCustomer>("customers", options, cancellationToken);

        var items = result.Value.Select(_customerMapper.Map).ToList();

        // Reuse the clamped values BcListQuery produced so the result echoes the
        // effective page/pageSize rather than the raw (possibly invalid) request.
        var effectivePage = (options.Skip ?? 0) / (options.Top ?? BcListQuery.DefaultPageSize) + 1;
        var effectivePageSize = options.Top ?? BcListQuery.DefaultPageSize;

        return new PagedResultDto<CustomerListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = effectivePage,
            PageSize = effectivePageSize,
        };
    }

    public async Task<CustomerDetailDto?> GetCustomerByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var customer = await _bc.GetByIdAsync<BcCustomer>("customers", id, cancellationToken: cancellationToken);
        if (customer == null || string.IsNullOrEmpty(customer.Id)) return null;

        // Escape single quotes per OData string-literal rules to avoid filter injection.
        var safeName = customer.DisplayName.Replace("'", "''");
        var invoicesResult = await _bc.GetCollectionAsync<BcSalesInvoice>(
            "salesInvoices",
            new BcQueryOptions
            {
                Filter = $"customerName eq '{safeName}'",
                OrderBy = "postingDate desc",
                Top = 10,
            },
            cancellationToken);

        return new CustomerDetailDto
        {
            Customer = new CustomerProfileDto
            {
                Id = customer.Id,
                Number = customer.Number,
                DisplayName = customer.DisplayName,
                City = customer.City,
                Balance = customer.Balance,
                BalanceDue = customer.BalanceDue,
            },
            Invoices = invoicesResult.Value.Select(i => new CustomerInvoiceDto
            {
                Id = i.Id,
                Number = i.Number,
                PostingDate = i.PostingDate,
                TotalAmountIncludingTax = i.TotalAmountIncludingTax,
                Status = i.Status,
            }).ToList(),
        };
    }
}
