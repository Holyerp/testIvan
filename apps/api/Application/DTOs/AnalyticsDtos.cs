namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// A single bucket on the revenue-vs-expense time series (US-020). The bucket label
/// (<see cref="Period"/>) shape depends on the requested granularity: yyyy-MM (monthly),
/// yyyy-Qn (quarterly), yyyy (yearly). Profit is revenue minus expense for the bucket.
/// </summary>
public class RevenueExpensePointDto
{
    public string Period { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Expense { get; set; }
    public decimal Profit { get; set; }
}

/// <summary>
/// Revenue/expense time series plus the current-vs-prior period comparison and the
/// granularity the series was bucketed at. <see cref="Granularity"/> is the cross-layer
/// wire value: MONTHLY | QUARTERLY | YEARLY (SCREAMING_SNAKE_CASE).
/// </summary>
public class RevenueExpenseSeriesDto
{
    public List<RevenueExpensePointDto> Points { get; set; } = new();
    public PeriodComparisonDto Comparison { get; set; } = new();
    public string Granularity { get; set; } = string.Empty;
}

/// <summary>
/// Totals for the current period vs the immediately-prior equal-length period, with the
/// percentage deltas. Delta % guards against a zero prior (returns 0 rather than dividing).
/// </summary>
public class PeriodComparisonDto
{
    public decimal CurrentRevenue { get; set; }
    public decimal PriorRevenue { get; set; }
    public decimal CurrentExpense { get; set; }
    public decimal PriorExpense { get; set; }
    public decimal RevenueDeltaPercent { get; set; }
    public decimal ExpenseDeltaPercent { get; set; }
}

/// <summary>
/// A top customer by revenue (sum of sales-invoice totals). Id falls back to the customer
/// name when the name cannot be resolved to a customer record.
/// </summary>
public class TopCustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int InvoiceCount { get; set; }
}

/// <summary>
/// A top item by sales volume (units sold). SalesValue is the monetary value of those
/// sales. Sourced from the additive sales-volume basis on the items mock (US-020 note).
/// </summary>
public class TopItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal SalesVolume { get; set; }
    public decimal SalesValue { get; set; }
}
