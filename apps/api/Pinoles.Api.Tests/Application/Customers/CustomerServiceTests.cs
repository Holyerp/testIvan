using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Customers;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Customers;

public class CustomerServiceTests
{
    private static CustomerService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new CustomerService(bc, new CustomerMapper());
    }

    [Fact]
    public async Task GetCustomersAsync_ReturnsItems()
    {
        var result = await CreateService().GetCustomersAsync(1, 20, null, null, null);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task GetCustomersAsync_RespectsPageSize()
    {
        var result = await CreateService().GetCustomersAsync(1, 2, null, null, null);
        Assert.True(result.Items.Count <= 2);
    }

    [Fact]
    public async Task GetCustomersAsync_ReturnsTotalCount()
    {
        var result = await CreateService().GetCustomersAsync(1, 2, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
    }

    [Fact]
    public async Task GetCustomersAsync_InvalidPage_DefaultsToOne()
    {
        var result = await CreateService().GetCustomersAsync(0, 20, null, null, null);
        Assert.Equal(1, result.Page);
    }

    [Fact]
    public async Task GetCustomersAsync_InvalidPageSize_DefaultsTo20()
    {
        var result = await CreateService().GetCustomersAsync(1, 9999, null, null, null);
        Assert.Equal(20, result.PageSize);
    }

    [Fact]
    public async Task GetCustomersAsync_MapsFields()
    {
        var result = await CreateService().GetCustomersAsync(1, 20, null, null, null);
        var first = result.Items.First();
        Assert.False(string.IsNullOrEmpty(first.Number));
        Assert.False(string.IsNullOrEmpty(first.DisplayName));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_KnownId_ReturnsCustomer()
    {
        var result = await CreateService().GetCustomerByIdAsync("c001");
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result!.Customer.DisplayName));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_UnknownId_ReturnsNull()
    {
        var result = await CreateService().GetCustomerByIdAsync("does-not-exist");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_IncludesInvoicesList()
    {
        var result = await CreateService().GetCustomerByIdAsync("c001");
        Assert.NotNull(result);
        Assert.NotNull(result!.Invoices);
    }
}
