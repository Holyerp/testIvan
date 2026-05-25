using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Dashboard;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Xunit;

namespace Pinoles.Api.Tests.Application.Dashboard;

public class DashboardServiceTests
{
    private static DashboardService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        var cache = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()));
        var opts = Options.Create(new BcOptions { UseMock = true, CacheSeconds = 300 });
        return new DashboardService(bc, cache, opts);
    }

    [Fact]
    public async Task GetKpisAsync_ReturnsPositiveCustomerCount()
    {
        var result = await CreateService().GetKpisAsync();
        Assert.True(result.TotalCustomers > 0);
    }

    [Fact]
    public async Task GetKpisAsync_ReturnsSixTrendPoints()
    {
        var result = await CreateService().GetKpisAsync();
        Assert.Equal(6, result.InvoiceTrend.Count);
    }

    [Fact]
    public async Task GetKpisAsync_IsMockModeTrue()
    {
        var result = await CreateService().GetKpisAsync();
        Assert.True(result.IsMockMode);
    }

    [Fact]
    public async Task GetKpisAsync_OpenInvoicesAmountPositive()
    {
        var result = await CreateService().GetKpisAsync();
        Assert.True(result.OpenInvoicesAmount > 0);
    }

    [Fact]
    public async Task GetKpisAsync_SecondCallReturnsCachedSameValues()
    {
        var service = CreateService();
        var first = await service.GetKpisAsync();
        var second = await service.GetKpisAsync();
        Assert.Equal(first.TotalCustomers, second.TotalCustomers);
        Assert.Equal(first.OpenInvoicesAmount, second.OpenInvoicesAmount);
    }
}
