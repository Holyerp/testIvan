using Microsoft.Extensions.Options;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IBcHttpClient _bc;
    private readonly ICacheService _cache;
    private readonly BcOptions _bcOptions;
    private const string CacheKey = "dashboard:kpis";

    public DashboardService(IBcHttpClient bc, ICacheService cache, IOptions<BcOptions> bcOptions)
    {
        _bc = bc;
        _cache = cache;
        _bcOptions = bcOptions.Value;
    }

    public async Task<DashboardKpisDto> GetKpisAsync(CancellationToken cancellationToken = default)
    {
        var cached = await _cache.GetAsync<DashboardKpisDto>(CacheKey, cancellationToken);
        if (cached != null) return cached;

        var customersResult = await _bc.GetCollectionAsync<BcCustomer>(
            "customers", new BcQueryOptions { Count = true }, cancellationToken);
        var totalCustomers = customersResult.Count ?? customersResult.Value.Count;

        var invoicesResult = await _bc.GetCollectionAsync<BcSalesInvoice>(
            "salesInvoices", new BcQueryOptions { Count = true }, cancellationToken);

        var openInvoices = invoicesResult.Value
            .Where(i => i.Status == "Open")
            .ToList();
        var openAmount = openInvoices.Sum(i => i.TotalAmountIncludingTax);

        var overdueAmount = openInvoices
            .Where(i => DateTime.TryParse(i.PostingDate, out var d) && d < DateTime.UtcNow.AddDays(-30))
            .Sum(i => i.TotalAmountIncludingTax);

        var vendorsResult = await _bc.GetCollectionAsync<BcVendor>(
            "vendors", new BcQueryOptions { Count = true }, cancellationToken);
        var totalVendors = vendorsResult.Count ?? vendorsResult.Value.Count;

        var trend = BuildTrend(invoicesResult.Value);

        var kpis = new DashboardKpisDto
        {
            TotalCustomers = totalCustomers,
            OpenInvoicesAmount = openAmount,
            OverdueInvoicesAmount = overdueAmount,
            TotalVendors = totalVendors,
            InvoiceTrend = trend,
            IsMockMode = _bcOptions.UseMock,
        };

        await _cache.SetAsync(CacheKey, kpis, TimeSpan.FromSeconds(_bcOptions.CacheSeconds), cancellationToken);
        return kpis;
    }

    private static List<InvoiceTrendPoint> BuildTrend(List<BcSalesInvoice> invoices)
    {
        var now = DateTime.UtcNow;
        var points = new List<InvoiceTrendPoint>();
        for (int i = 5; i >= 0; i--)
        {
            var month = now.AddMonths(-i);
            var monthInvoices = invoices.Where(inv =>
                DateTime.TryParse(inv.PostingDate, out var d) &&
                d.Year == month.Year && d.Month == month.Month).ToList();
            points.Add(new InvoiceTrendPoint
            {
                Month = month.ToString("yyyy-MM"),
                Count = monthInvoices.Count,
                TotalAmount = monthInvoices.Sum(inv => inv.TotalAmountIncludingTax),
            });
        }
        return points;
    }
}
