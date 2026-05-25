using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Management analytics aggregation (US-020). Blends the dashboard KPI-aggregation
/// pattern (fetch BC collections, aggregate in memory, 5-min cache) over sales (revenue)
/// and purchase (expense) invoices, plus top customers / top items.
/// </summary>
public interface IAnalyticsService
{
    /// <param name="granularity">MONTHLY | QUARTERLY | YEARLY — invalid values fall back to MONTHLY.</param>
    /// <param name="fromDate">Inclusive ISO yyyy-MM-dd lower bound; defaults to last 12 months.</param>
    /// <param name="toDate">Inclusive ISO yyyy-MM-dd upper bound; defaults to today.</param>
    Task<RevenueExpenseSeriesDto> GetRevenueExpenseAsync(
        string granularity, string? fromDate, string? toDate, CancellationToken ct = default);

    Task<List<TopCustomerDto>> GetTopCustomersAsync(
        int top = 10, string? fromDate = null, string? toDate = null, CancellationToken ct = default);

    Task<List<TopItemDto>> GetTopItemsAsync(
        int top = 10, string? fromDate = null, string? toDate = null, CancellationToken ct = default);
}
