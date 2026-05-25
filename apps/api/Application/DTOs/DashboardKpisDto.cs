namespace Pinoles.Api.Application.DTOs;

public class DashboardKpisDto
{
    public int TotalCustomers { get; set; }
    public decimal OpenInvoicesAmount { get; set; }
    public decimal OverdueInvoicesAmount { get; set; }
    public int TotalVendors { get; set; }
    public List<InvoiceTrendPoint> InvoiceTrend { get; set; } = new();
    public bool IsMockMode { get; set; }
}

public class InvoiceTrendPoint
{
    public string Month { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}
