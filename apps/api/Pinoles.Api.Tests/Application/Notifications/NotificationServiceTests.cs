using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Items;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Notifications;
using Pinoles.Api.Application.Sales;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Xunit;

namespace Pinoles.Api.Tests.Application.Notifications;

public class NotificationServiceTests
{
    private static readonly string[] AccountingRole = { UserRoles.Accounting };
    private static readonly string[] WarehouseRole = { UserRoles.Warehouse };
    private static readonly string[] AdminRole = { UserRoles.Admin };
    private static readonly string[] ManagerRole = { UserRoles.Manager };

    // Wire the real list services + MockBcHttpClient (mirrors SearchServiceTests), plus a
    // real MemoryCacheService (mirrors DashboardServiceTests).
    private static NotificationService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        var sales = new SalesService(bc, new SalesInvoiceMapper(), new SalesInvoiceDetailMapper(), new SalesAdvanceInvoiceDetailMapper());
        var items = new ItemService(bc, new ItemMapper(), new ItemDetailMapper(), new StockByLocationMapper(), new ItemLedgerEntryMapper());
        var cache = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()));
        return new NotificationService(sales, items, cache);
    }

    [Fact]
    public async Task GetNotificationsAsync_FinancialRole_ReturnsOverdueInvoiceNotifications()
    {
        var result = await CreateService().GetNotificationsAsync(AccountingRole);

        Assert.NotEmpty(result);
        Assert.All(result, n => Assert.Equal("OVERDUE_INVOICE", n.Type));
        // The mock has open invoices due in the past (inv007/inv009/inv010).
        Assert.Contains(result, n => n.Title.StartsWith("SI-"));
    }

    [Fact]
    public async Task GetNotificationsAsync_WarehouseRole_ReturnsLowStockNotifications()
    {
        var result = await CreateService().GetNotificationsAsync(WarehouseRole);

        Assert.NotEmpty(result);
        Assert.All(result, n => Assert.Equal("LOW_STOCK", n.Type));
    }

    [Fact]
    public async Task GetNotificationsAsync_WarehouseRole_DoesNotSeeOverdueInvoices()
    {
        var result = await CreateService().GetNotificationsAsync(WarehouseRole);

        Assert.DoesNotContain(result, n => n.Type == "OVERDUE_INVOICE");
    }

    [Fact]
    public async Task GetNotificationsAsync_FinancialRole_DoesNotSeeLowStock()
    {
        var result = await CreateService().GetNotificationsAsync(AccountingRole);

        Assert.DoesNotContain(result, n => n.Type == "LOW_STOCK");
    }

    [Fact]
    public async Task GetNotificationsAsync_AdminRole_SeesBothKinds()
    {
        var result = await CreateService().GetNotificationsAsync(AdminRole);

        Assert.Contains(result, n => n.Type == "OVERDUE_INVOICE");
        Assert.Contains(result, n => n.Type == "LOW_STOCK");
    }

    [Fact]
    public async Task GetNotificationsAsync_ManagerRole_SeesBothKinds()
    {
        var result = await CreateService().GetNotificationsAsync(ManagerRole);

        Assert.Contains(result, n => n.Type == "OVERDUE_INVOICE");
        Assert.Contains(result, n => n.Type == "LOW_STOCK");
    }

    [Fact]
    public async Task GetNotificationsAsync_SetsSeverityToWireValues()
    {
        var result = await CreateService().GetNotificationsAsync(AdminRole);

        Assert.NotEmpty(result);
        Assert.All(result, n => Assert.Contains(n.Severity, new[] { "INFO", "WARNING", "CRITICAL" }));
    }

    [Fact]
    public async Task GetNotificationsAsync_OverdueInvoice_PopulatesLinkToInvoiceDetail()
    {
        var result = await CreateService().GetNotificationsAsync(AccountingRole);

        var overdue = Assert.Single(result.Where(n => n.Type == "OVERDUE_INVOICE").Take(1));
        Assert.False(string.IsNullOrEmpty(overdue.Link));
        Assert.StartsWith("/sales/invoices/", overdue.Link!);
        Assert.False(string.IsNullOrEmpty(overdue.Message));
    }

    [Fact]
    public async Task GetNotificationsAsync_LowStock_PopulatesLinkToItemDetail()
    {
        var result = await CreateService().GetNotificationsAsync(WarehouseRole);

        var lowStock = Assert.Single(result.Where(n => n.Type == "LOW_STOCK").Take(1));
        Assert.False(string.IsNullOrEmpty(lowStock.Link));
        Assert.StartsWith("/items/", lowStock.Link!);
        Assert.Equal("WARNING", lowStock.Severity);
    }

    [Fact]
    public async Task GetNotificationsAsync_NoRoles_ReturnsEmpty()
    {
        var result = await CreateService().GetNotificationsAsync(System.Array.Empty<string>());

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetNotificationsAsync_CapsEachKindAtTen()
    {
        var result = await CreateService().GetNotificationsAsync(AdminRole);

        Assert.True(result.Count(n => n.Type == "OVERDUE_INVOICE") <= 10);
        Assert.True(result.Count(n => n.Type == "LOW_STOCK") <= 10);
    }
}
