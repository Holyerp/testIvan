using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Analytics;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Xunit;

namespace Pinoles.Api.Tests.Application.Analytics;

public class AnalyticsServiceTests
{
    private static AnalyticsService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        var cache = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()));
        var opts = Options.Create(new BcOptions { UseMock = true, CacheSeconds = 300 });
        return new AnalyticsService(bc, cache, opts);
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_ReturnsPointsWithRevenueAndExpense()
    {
        var result = await CreateService().GetRevenueExpenseAsync(Granularity.Monthly, null, null);

        Assert.NotEmpty(result.Points);
        Assert.True(result.Points.Sum(p => p.Revenue) > 0);
        Assert.True(result.Points.Sum(p => p.Expense) > 0);
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_ProfitEqualsRevenueMinusExpense()
    {
        var result = await CreateService().GetRevenueExpenseAsync(Granularity.Monthly, null, null);

        Assert.All(result.Points, p => Assert.Equal(p.Revenue - p.Expense, p.Profit));
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_GranularityBucketsDiffer()
    {
        var service = CreateService();
        var monthly = await service.GetRevenueExpenseAsync(Granularity.Monthly, null, null);
        var quarterly = await service.GetRevenueExpenseAsync(Granularity.Quarterly, null, null);
        var yearly = await service.GetRevenueExpenseAsync(Granularity.Yearly, null, null);

        // Coarser granularities collapse more rows into each bucket, so the point count is
        // non-increasing as the period widens. Over a 12-month spread the monthly view has
        // strictly more buckets than the yearly view.
        Assert.True(monthly.Points.Count >= quarterly.Points.Count);
        Assert.True(quarterly.Points.Count >= yearly.Points.Count);
        Assert.True(monthly.Points.Count > yearly.Points.Count);
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_QuarterlyPeriodLabelFormat()
    {
        var result = await CreateService().GetRevenueExpenseAsync(Granularity.Quarterly, null, null);

        Assert.All(result.Points, p => Assert.Contains("-Q", p.Period));
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_InvalidGranularityFallsBackToMonthly()
    {
        var result = await CreateService().GetRevenueExpenseAsync("WEEKLY", null, null);

        Assert.Equal(Granularity.Monthly, result.Granularity);
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_ComputesNonTrivialComparison()
    {
        var result = await CreateService().GetRevenueExpenseAsync(Granularity.Monthly, null, null);

        // The mock spreads sales/purchases into both the current and the prior 12-month
        // window, so both current and prior totals are positive.
        Assert.True(result.Comparison.CurrentRevenue > 0);
        Assert.True(result.Comparison.PriorRevenue > 0);
        Assert.True(result.Comparison.CurrentExpense > 0);
        Assert.True(result.Comparison.PriorExpense > 0);
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_DeltaPercentGuardsZeroPrior()
    {
        // A 1-day range guarantees an empty prior period (delta % must be 0, not infinite).
        var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
        var result = await CreateService().GetRevenueExpenseAsync(Granularity.Monthly, today, today);

        if (result.Comparison.PriorRevenue == 0)
            Assert.Equal(0m, result.Comparison.RevenueDeltaPercent);
        if (result.Comparison.PriorExpense == 0)
            Assert.Equal(0m, result.Comparison.ExpenseDeltaPercent);
    }

    [Fact]
    public async Task GetRevenueExpenseAsync_CachesSecondCall()
    {
        var service = CreateService();
        var first = await service.GetRevenueExpenseAsync(Granularity.Monthly, null, null);
        var second = await service.GetRevenueExpenseAsync(Granularity.Monthly, null, null);

        Assert.Equal(first.Comparison.CurrentRevenue, second.Comparison.CurrentRevenue);
        Assert.Equal(first.Points.Count, second.Points.Count);
    }

    [Fact]
    public async Task GetTopCustomersAsync_OrderedDescendingByRevenue()
    {
        var result = await CreateService().GetTopCustomersAsync(10);

        Assert.NotEmpty(result);
        for (var i = 1; i < result.Count; i++)
            Assert.True(result[i - 1].Revenue >= result[i].Revenue);
        Assert.All(result, c => Assert.True(c.InvoiceCount > 0));
    }

    [Fact]
    public async Task GetTopCustomersAsync_CappedAtTop()
    {
        var result = await CreateService().GetTopCustomersAsync(2);

        Assert.True(result.Count <= 2);
    }

    [Fact]
    public async Task GetTopItemsAsync_OrderedDescendingBySalesVolume()
    {
        var result = await CreateService().GetTopItemsAsync(10);

        Assert.NotEmpty(result);
        for (var i = 1; i < result.Count; i++)
            Assert.True(result[i - 1].SalesVolume >= result[i].SalesVolume);
        Assert.All(result, i => Assert.True(i.SalesVolume > 0));
    }

    [Fact]
    public async Task GetTopItemsAsync_CappedAtTop()
    {
        var result = await CreateService().GetTopItemsAsync(3);

        Assert.True(result.Count <= 3);
    }
}
