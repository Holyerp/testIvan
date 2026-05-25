using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Items;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Items;

public class ItemServiceTests
{
    private static ItemService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new ItemService(bc, new ItemMapper());
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsItems()
    {
        var result = await CreateService().GetItemsAsync(1, 20, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task GetItemsAsync_RespectsPageSize()
    {
        var result = await CreateService().GetItemsAsync(1, 3, null, null, null, null, null);
        Assert.True(result.Items.Count <= 3);
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsTotalCount()
    {
        var result = await CreateService().GetItemsAsync(1, 3, null, null, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
    }

    [Fact]
    public async Task GetItemsAsync_InvalidPage_DefaultsToOne()
    {
        var result = await CreateService().GetItemsAsync(0, 20, null, null, null, null, null);
        Assert.Equal(1, result.Page);
    }

    [Fact]
    public async Task GetItemsAsync_InvalidPageSize_DefaultsTo20()
    {
        var result = await CreateService().GetItemsAsync(1, 9999, null, null, null, null, null);
        Assert.Equal(20, result.PageSize);
    }

    [Fact]
    public async Task GetItemsAsync_Search_NarrowsResults()
    {
        var all = await CreateService().GetItemsAsync(1, 100, null, null, null, null, null);
        var filtered = await CreateService().GetItemsAsync(1, 100, "Cement", null, null, null, null);

        Assert.NotEmpty(filtered.Items);
        Assert.True(filtered.Total < all.Total);
        Assert.All(filtered.Items, i =>
            Assert.Contains("Cement", i.Description, System.StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetItemsAsync_SearchByNumber_Matches()
    {
        var result = await CreateService().GetItemsAsync(1, 100, "ITM-001", null, null, null, null);
        Assert.Single(result.Items);
        Assert.Equal("ITM-001", result.Items[0].Number);
    }

    [Fact]
    public async Task GetItemsAsync_CategoryFilter_NarrowsResults()
    {
        var all = await CreateService().GetItemsAsync(1, 100, null, null, null, null, null);
        var filtered = await CreateService().GetItemsAsync(1, 100, null, null, null, "ALATI", null);

        Assert.NotEmpty(filtered.Items);
        Assert.True(filtered.Total < all.Total);
        Assert.All(filtered.Items, i =>
            Assert.Equal("ALATI", i.Category, System.StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetItemsAsync_LocationFilter_NarrowsResults()
    {
        var all = await CreateService().GetItemsAsync(1, 100, null, null, null, null, null);
        var filtered = await CreateService().GetItemsAsync(1, 100, null, null, null, null, "MAGACIN-2");

        Assert.NotEmpty(filtered.Items);
        Assert.True(filtered.Total < all.Total);
    }

    [Fact]
    public async Task GetItemsAsync_CategoryAndSearch_CombineWithAnd()
    {
        // "Bušilica" is in category ALATI; combining with the GRAĐEVINA category yields none.
        var filtered = await CreateService().GetItemsAsync(1, 100, "Bušilica", null, null, "GRAĐEVINA", null);
        Assert.Empty(filtered.Items);
    }

    [Fact]
    public async Task GetItemsAsync_MapsFields_IncludingIsLowStock()
    {
        // ITM-002 (Čelična šipka) is below minimum stock (30 < 100) → IsLowStock true.
        var lowResult = await CreateService().GetItemsAsync(1, 100, "ITM-002", null, null, null, null);
        var low = lowResult.Items.Single();

        Assert.Equal("itm002", low.Id);
        Assert.Equal("ITM-002", low.Number);
        Assert.False(string.IsNullOrEmpty(low.Description));
        Assert.False(string.IsNullOrEmpty(low.Category));
        Assert.False(string.IsNullOrEmpty(low.UnitOfMeasure));
        Assert.True(low.UnitCost > 0);
        Assert.True(low.IsLowStock);

        // ITM-001 (Cement) is at/above minimum stock (120 >= 50) → IsLowStock false.
        var okResult = await CreateService().GetItemsAsync(1, 100, "ITM-001", null, null, null, null);
        var ok = okResult.Items.Single();
        Assert.False(ok.IsLowStock);
    }

    [Fact]
    public async Task GetItemsAsync_SortByQuantityAsc_OrdersAscending()
    {
        var result = await CreateService().GetItemsAsync(1, 100, null, "quantity", "asc", null, null);
        var quantities = result.Items.Select(i => i.QuantityOnHand).ToList();
        var ordered = quantities.OrderBy(q => q).ToList();
        Assert.Equal(ordered, quantities);
    }

    [Fact]
    public async Task GetItemsAsync_SortByNameDesc_OrdersDescending()
    {
        var result = await CreateService().GetItemsAsync(1, 100, null, "name", "desc", null, null);
        var names = result.Items.Select(i => i.Description).ToList();
        var ordered = names.OrderByDescending(n => n, System.StringComparer.OrdinalIgnoreCase).ToList();
        Assert.Equal(ordered, names);
    }

    [Fact]
    public async Task GetItemsAsync_SortByUnitCostAsc_OrdersAscending()
    {
        var result = await CreateService().GetItemsAsync(1, 100, null, "unitCost", "asc", null, null);
        var costs = result.Items.Select(i => i.UnitCost).ToList();
        var ordered = costs.OrderBy(c => c).ToList();
        Assert.Equal(ordered, costs);
    }
}
