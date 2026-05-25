using System.Globalization;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Analytics;

/// <summary>
/// Management analytics aggregation (US-020). Mirrors <see cref="Dashboard.DashboardService"/>:
/// fetch BC collections via <see cref="IBcHttpClient"/>, aggregate in memory, and cache the
/// (computed) revenue/expense series for <see cref="BcOptions.CacheSeconds"/> (5 min).
/// Revenue = sales-invoice totals; expense = purchase-invoice totals; both are bucketed by
/// period per the requested granularity. Top customers come from the sales collection;
/// top items from the additive sales-volume basis on the items collection.
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IBcHttpClient _bc;
    private readonly ICacheService _cache;
    private readonly BcOptions _bcOptions;

    public AnalyticsService(IBcHttpClient bc, ICacheService cache, IOptions<BcOptions> bcOptions)
    {
        _bc = bc;
        _cache = cache;
        _bcOptions = bcOptions.Value;
    }

    public async Task<RevenueExpenseSeriesDto> GetRevenueExpenseAsync(
        string granularity, string? fromDate, string? toDate, CancellationToken ct = default)
    {
        var resolvedGranularity = Granularity.Normalize(granularity);
        var (from, to) = ResolveRange(fromDate, toDate);

        // Cache key varies per granularity + range so a filtered series never serves a
        // differently-scoped cached value. Mirrors DashboardService's 5-min window.
        var cacheKey = $"analytics:revenue-expense:{resolvedGranularity}:{Iso(from)}:{Iso(to)}";
        var cached = await _cache.GetAsync<RevenueExpenseSeriesDto>(cacheKey, ct);
        if (cached != null) return cached;

        var sales = await FetchInvoiceAmountsAsync("salesInvoices", ct);
        var purchases = await FetchInvoiceAmountsAsync("purchaseInvoices", ct);

        // Current-period rows; prior period is the immediately-preceding equal-length window.
        var periodLength = to - from;
        var priorTo = from.AddDays(-1);
        var priorFrom = priorTo - periodLength;

        var currentSales = WithinRange(sales, from, to);
        var currentPurchases = WithinRange(purchases, from, to);

        var points = BuildPoints(currentSales, currentPurchases, resolvedGranularity);

        var comparison = new PeriodComparisonDto
        {
            CurrentRevenue = currentSales.Sum(d => d.Amount),
            PriorRevenue = WithinRange(sales, priorFrom, priorTo).Sum(d => d.Amount),
            CurrentExpense = currentPurchases.Sum(d => d.Amount),
            PriorExpense = WithinRange(purchases, priorFrom, priorTo).Sum(d => d.Amount),
        };
        comparison.RevenueDeltaPercent = DeltaPercent(comparison.CurrentRevenue, comparison.PriorRevenue);
        comparison.ExpenseDeltaPercent = DeltaPercent(comparison.CurrentExpense, comparison.PriorExpense);

        var series = new RevenueExpenseSeriesDto
        {
            Points = points,
            Comparison = comparison,
            Granularity = resolvedGranularity,
        };

        await _cache.SetAsync(cacheKey, series, TimeSpan.FromSeconds(_bcOptions.CacheSeconds), ct);
        return series;
    }

    public async Task<List<TopCustomerDto>> GetTopCustomersAsync(
        int top = 10, string? fromDate = null, string? toDate = null, CancellationToken ct = default)
    {
        var (from, to) = ResolveRange(fromDate, toDate);

        var invoicesResult = await _bc.GetCollectionAsync<BcSalesInvoice>(
            "salesInvoices", new BcQueryOptions(), ct);
        var customersResult = await _bc.GetCollectionAsync<BcCustomer>(
            "customers", new BcQueryOptions(), ct);

        // Resolve a customer name to its id where possible; fall back to the name as id.
        var idByName = customersResult.Value
            .GroupBy(c => c.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

        var inRange = invoicesResult.Value
            .Where(i => WithinDates(i.PostingDate, from, to));

        return inRange
            .GroupBy(i => i.CustomerName, StringComparer.OrdinalIgnoreCase)
            .Select(g => new TopCustomerDto
            {
                Id = idByName.TryGetValue(g.Key, out var id) ? id : g.Key,
                Name = g.Key,
                Revenue = g.Sum(i => i.TotalAmountIncludingTax),
                InvoiceCount = g.Count(),
            })
            .OrderByDescending(c => c.Revenue)
            .Take(Math.Max(1, top))
            .ToList();
    }

    public async Task<List<TopItemDto>> GetTopItemsAsync(
        int top = 10, string? fromDate = null, string? toDate = null, CancellationToken ct = default)
    {
        // Top items are sourced from the additive sales-volume basis carried on each item
        // (BcItem.SalesVolume / SalesValue). Invoice line items in this BC use free-text
        // descriptions that are not linked to item records, so they cannot be aggregated by
        // item; the items collection's sales-volume basis is the faithful source. The date
        // range is accepted for API symmetry but the mock basis is a rolling aggregate.
        var itemsResult = await _bc.GetCollectionAsync<BcItem>("items", new BcQueryOptions(), ct);

        return itemsResult.Value
            .Where(i => i.SalesVolume > 0)
            .Select(i => new TopItemDto
            {
                Id = i.Id,
                Number = i.Number,
                Description = i.Description,
                SalesVolume = i.SalesVolume,
                SalesValue = i.SalesValue,
            })
            .OrderByDescending(i => i.SalesVolume)
            .Take(Math.Max(1, top))
            .ToList();
    }

    // Fetch an invoice-shaped collection reduced to the (date, amount) pairs the time
    // series needs. Keeps the aggregation independent of the full BC entity shape.
    private async Task<List<DatedAmount>> FetchInvoiceAmountsAsync(
        string entitySet, CancellationToken ct)
    {
        if (entitySet == "salesInvoices")
        {
            var result = await _bc.GetCollectionAsync<BcSalesInvoice>(entitySet, new BcQueryOptions(), ct);
            return result.Value
                .Select(i => new DatedAmount(i.PostingDate, i.TotalAmountIncludingTax))
                .ToList();
        }

        var purchases = await _bc.GetCollectionAsync<BcPurchaseInvoice>(entitySet, new BcQueryOptions(), ct);
        return purchases.Value
            .Select(i => new DatedAmount(i.PostingDate, i.TotalAmountIncludingTax))
            .ToList();
    }

    // Bucket revenue + expense by the granularity-specific period key, ordered chronologically.
    private static List<RevenueExpensePointDto> BuildPoints(
        List<DatedAmount> sales, List<DatedAmount> purchases, string granularity)
    {
        var buckets = new Dictionary<string, RevenueExpensePointDto>(StringComparer.Ordinal);

        RevenueExpensePointDto Bucket(string period)
        {
            if (!buckets.TryGetValue(period, out var point))
            {
                point = new RevenueExpensePointDto { Period = period };
                buckets[period] = point;
            }
            return point;
        }

        foreach (var s in sales)
            if (TryParse(s.Date, out var d))
                Bucket(PeriodKey(d, granularity)).Revenue += s.Amount;

        foreach (var p in purchases)
            if (TryParse(p.Date, out var d))
                Bucket(PeriodKey(d, granularity)).Expense += p.Amount;

        foreach (var point in buckets.Values)
            point.Profit = point.Revenue - point.Expense;

        return buckets.Values.OrderBy(p => p.Period, StringComparer.Ordinal).ToList();
    }

    // The period label for a date at the given granularity:
    //   MONTHLY   -> yyyy-MM
    //   QUARTERLY -> yyyy-Qn   (n = 1..4)
    //   YEARLY    -> yyyy
    private static string PeriodKey(DateTime date, string granularity) => granularity switch
    {
        Granularity.Yearly => date.ToString("yyyy", CultureInfo.InvariantCulture),
        Granularity.Quarterly => $"{date.Year}-Q{(date.Month - 1) / 3 + 1}",
        _ => date.ToString("yyyy-MM", CultureInfo.InvariantCulture),
    };

    // Percentage delta of current vs prior, guarding a zero prior (returns 0 to avoid
    // dividing by zero / reporting an infinite increase). Rounded to 1 decimal place.
    private static decimal DeltaPercent(decimal current, decimal prior)
    {
        if (prior == 0) return 0m;
        return decimal.Round((current - prior) / prior * 100m, 1);
    }

    // Default the range to the last 12 months when no bounds are supplied. Invalid date
    // strings are treated as "not supplied".
    private static (DateTime From, DateTime To) ResolveRange(string? fromDate, string? toDate)
    {
        var to = TryParse(toDate, out var parsedTo) ? parsedTo : DateTime.UtcNow.Date;
        var from = TryParse(fromDate, out var parsedFrom) ? parsedFrom : to.AddMonths(-12);
        if (from > to) (from, to) = (to, from);
        return (from, to);
    }

    private static List<DatedAmount> WithinRange(List<DatedAmount> source, DateTime from, DateTime to)
        => source.Where(d => WithinDates(d.Date, from, to)).ToList();

    private static bool WithinDates(string isoDate, DateTime from, DateTime to)
        => TryParse(isoDate, out var d) && d >= from && d <= to;

    private static bool TryParse(string? value, out DateTime date)
        => DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

    private static string Iso(DateTime d) => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private readonly record struct DatedAmount(string Date, decimal Amount);
}
