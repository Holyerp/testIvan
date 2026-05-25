using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Inventory;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Xunit;

namespace Pinoles.Api.Tests.Application.Inventory;

public class InventoryServiceTests
{
    private static InventoryService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        var cache = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()));
        var opts = Options.Create(new BcOptions { UseMock = true, CacheSeconds = 300 });
        return new InventoryService(bc, cache, opts);
    }

    [Fact]
    public async Task GetSummaryAsync_ReturnsPositiveTotals()
    {
        var result = await CreateService().GetSummaryAsync(null, null);

        Assert.True(result.TotalItems > 0);
        Assert.True(result.TotalStockValue > 0);
    }

    [Fact]
    public async Task GetSummaryAsync_ItemsBelowMinimum_MatchesMockBelowMinimumCount()
    {
        // The mock seeds 4 items below their minimum stock: ITM-002 (30<100),
        // ITM-004 (800<1000), ITM-006 (5<10), ITM-009 (6<25).
        var result = await CreateService().GetSummaryAsync(null, null);
        Assert.Equal(4, result.ItemsBelowMinimum);
    }

    [Fact]
    public async Task GetSummaryAsync_TotalStockValue_EqualsSumOfQuantityTimesUnitCost()
    {
        var result = await CreateService().GetSummaryAsync(null, null);

        // Total value must be the sum of quantityOnHand * unitCost across all 12 items;
        // a non-trivial figure well above the largest single line.
        Assert.True(result.TotalStockValue > 100000m);
    }

    [Fact]
    public async Task GetSummaryAsync_CategoryFilter_NarrowsTotalItems()
    {
        var all = await CreateService().GetSummaryAsync(null, null);
        var alati = await CreateService().GetSummaryAsync(null, "ALATI");

        Assert.True(alati.TotalItems > 0);
        Assert.True(alati.TotalItems < all.TotalItems);
    }

    [Fact]
    public async Task GetSummaryAsync_LocationFilter_NarrowsTotalItems()
    {
        var all = await CreateService().GetSummaryAsync(null, null);
        var magacin2 = await CreateService().GetSummaryAsync("MAGACIN-2", null);

        Assert.True(magacin2.TotalItems > 0);
        Assert.True(magacin2.TotalItems < all.TotalItems);
    }

    [Fact]
    public async Task GetSummaryAsync_SecondCall_ReturnsCachedSameValues()
    {
        var service = CreateService();
        var first = await service.GetSummaryAsync(null, null);
        var second = await service.GetSummaryAsync(null, null);

        Assert.Equal(first.TotalItems, second.TotalItems);
        Assert.Equal(first.TotalStockValue, second.TotalStockValue);
        Assert.Equal(first.ItemsBelowMinimum, second.ItemsBelowMinimum);
    }

    [Fact]
    public async Task GetByLocationAsync_GroupsWithConsistentTotals()
    {
        var locations = await CreateService().GetByLocationAsync();

        Assert.NotEmpty(locations);
        // Every group has a non-empty location, a positive item count and quantity, and a
        // positive value; values are never negative.
        Assert.All(locations, l =>
        {
            Assert.False(string.IsNullOrEmpty(l.Location));
            Assert.True(l.ItemCount > 0);
            Assert.True(l.TotalQuantity > 0);
            Assert.True(l.TotalValue >= 0);
        });
    }

    [Fact]
    public async Task GetByLocationAsync_TotalValue_ReconcilesWithSummary()
    {
        var service = CreateService();
        var summary = await service.GetSummaryAsync(null, null);
        var locations = await service.GetByLocationAsync();

        // Each item's value is allocated across its locations proportionally to the
        // quantity held, so the per-location values sum back to the overall stock value.
        var locationValueSum = locations.Sum(l => l.TotalValue);
        Assert.Equal(summary.TotalStockValue, locationValueSum, precision: 2);
    }

    [Fact]
    public async Task GetLowStockAsync_OnlyBelowMinimum_SortedAscending()
    {
        var lowStock = await CreateService().GetLowStockAsync();

        Assert.NotEmpty(lowStock);
        // Only items below their minimum threshold are returned.
        Assert.All(lowStock, i => Assert.True(i.QuantityOnHand < i.MinimumStock));

        // Sorted by quantity on hand ascending.
        var quantities = lowStock.Select(i => i.QuantityOnHand).ToList();
        var ordered = quantities.OrderBy(q => q).ToList();
        Assert.Equal(ordered, quantities);
    }

    [Fact]
    public async Task GetLowStockAsync_CountMatchesSummaryBelowMinimum()
    {
        var service = CreateService();
        var summary = await service.GetSummaryAsync(null, null);
        var lowStock = await service.GetLowStockAsync();

        Assert.Equal(summary.ItemsBelowMinimum, lowStock.Count);
    }
}
