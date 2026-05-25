using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Vendors;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Vendors;

public class VendorServiceTests
{
    private static VendorService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new VendorService(bc, new VendorMapper());
    }

    [Fact]
    public async Task GetVendorsAsync_ReturnsItems()
    {
        var result = await CreateService().GetVendorsAsync(1, 20, null, null, null);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task GetVendorsAsync_RespectsPageSize()
    {
        var result = await CreateService().GetVendorsAsync(1, 3, null, null, null);
        Assert.True(result.Items.Count <= 3);
    }

    [Fact]
    public async Task GetVendorsAsync_ReturnsTotalCount()
    {
        var result = await CreateService().GetVendorsAsync(1, 3, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
    }

    [Fact]
    public async Task GetVendorsAsync_InvalidPage_DefaultsToOne()
    {
        var result = await CreateService().GetVendorsAsync(0, 20, null, null, null);
        Assert.Equal(1, result.Page);
    }

    [Fact]
    public async Task GetVendorsAsync_InvalidPageSize_DefaultsTo20()
    {
        var result = await CreateService().GetVendorsAsync(1, 9999, null, null, null);
        Assert.Equal(20, result.PageSize);
    }

    [Fact]
    public async Task GetVendorsAsync_Search_NarrowsResults()
    {
        var all = await CreateService().GetVendorsAsync(1, 100, null, null, null);
        var filtered = await CreateService().GetVendorsAsync(1, 100, "Supplier", null, null);

        Assert.NotEmpty(filtered.Items);
        Assert.True(filtered.Total < all.Total);
        Assert.All(filtered.Items, v =>
            Assert.Contains("Supplier", v.DisplayName, System.StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetVendorsAsync_SearchByNumber_Matches()
    {
        var result = await CreateService().GetVendorsAsync(1, 100, "V001", null, null);
        Assert.Single(result.Items);
        Assert.Equal("V001", result.Items[0].Number);
    }

    [Fact]
    public async Task GetVendorsAsync_MapsAllFields_IncludingCityAndPhone()
    {
        var result = await CreateService().GetVendorsAsync(1, 100, "V001", null, null);
        var vendor = result.Items.Single();

        Assert.Equal("v001", vendor.Id);
        Assert.Equal("V001", vendor.Number);
        Assert.False(string.IsNullOrEmpty(vendor.DisplayName));
        Assert.False(string.IsNullOrEmpty(vendor.City));
        Assert.False(string.IsNullOrEmpty(vendor.Phone));
        Assert.True(vendor.Balance > 0);
    }

    [Fact]
    public async Task GetVendorsAsync_SortByBalanceAsc_OrdersAscending()
    {
        var result = await CreateService().GetVendorsAsync(1, 100, null, "balance", "asc");
        var balances = result.Items.Select(v => v.Balance).ToList();
        var ordered = balances.OrderBy(b => b).ToList();
        Assert.Equal(ordered, balances);
    }

    [Fact]
    public async Task GetVendorsAsync_SortByDisplayNameDesc_OrdersDescending()
    {
        var result = await CreateService().GetVendorsAsync(1, 100, null, "displayName", "desc");
        var names = result.Items.Select(v => v.DisplayName).ToList();
        var ordered = names.OrderByDescending(n => n, System.StringComparer.OrdinalIgnoreCase).ToList();
        Assert.Equal(ordered, names);
    }
}
