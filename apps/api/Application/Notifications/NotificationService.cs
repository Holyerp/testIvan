using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Constants;

namespace Pinoles.Api.Application.Notifications;

/// <summary>
/// Aggregates actionable in-app notifications from the existing list services (DRY:
/// reuses <see cref="ISalesService"/> + <see cref="IItemService"/> rather than re-querying
/// BC ad-hoc). Two notification kinds:
///   OVERDUE_INVOICE — open sales invoices past their due date (financial alert)
///   LOW_STOCK       — items below their minimum stock (warehouse alert)
/// Notifications are role-filtered to mirror the financial/warehouse module-access model
/// (see SearchService): financial roles see overdue invoices, warehouse-capable roles see
/// low stock, ADMIN/MANAGER see both. The result is cached briefly to spare BC.
/// </summary>
public class NotificationService : INotificationService
{
    // Per-kind cap so the bell stays a digest, not an unbounded dump.
    private const int MaxPerKind = 10;
    private const int CacheSeconds = 60;
    // Each notification kind is cached independently of the caller's role, then combined
    // per request — so the cache stays role-agnostic and shareable across users.
    private const string OverdueCacheKey = "notifications:overdue-invoices";
    private const string LowStockCacheKey = "notifications:low-stock";

    // Days overdue beyond which an invoice is escalated from WARNING to CRITICAL.
    private const int CriticalOverdueDays = 30;

    // Roles permitted to see financial (overdue-invoice) notifications — matches the
    // RequireFinancial policy. Warehouse roles get the warehouse (low-stock) set instead.
    private static readonly string[] FinancialRoles =
    {
        UserRoles.Admin, UserRoles.Manager, UserRoles.Accounting,
    };

    private static readonly string[] WarehouseRoles =
    {
        UserRoles.Admin, UserRoles.Manager, UserRoles.Warehouse,
    };

    private readonly ISalesService _sales;
    private readonly IItemService _items;
    private readonly ICacheService _cache;

    public NotificationService(ISalesService sales, IItemService items, ICacheService cache)
    {
        _sales = sales;
        _items = items;
        _cache = cache;
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync(
        IEnumerable<string> roles,
        CancellationToken ct = default)
    {
        var roleList = roles.ToList();
        var seesFinancial = roleList.Any(r => FinancialRoles.Contains(r, StringComparer.Ordinal));
        var seesWarehouse = roleList.Any(r => WarehouseRoles.Contains(r, StringComparer.Ordinal));

        var notifications = new List<NotificationDto>();

        if (seesFinancial)
            notifications.AddRange(await BuildOverdueInvoiceNotificationsAsync(ct));

        if (seesWarehouse)
            notifications.AddRange(await BuildLowStockNotificationsAsync(ct));

        return notifications;
    }

    private async Task<List<NotificationDto>> BuildOverdueInvoiceNotificationsAsync(CancellationToken ct)
    {
        var cached = await _cache.GetAsync<List<NotificationDto>>(OverdueCacheKey, ct);
        if (cached != null) return cached;

        // Pull a generous page of open invoices; BC paging caps at 50 and the cap below
        // trims to MaxPerKind. Open invoices only (PAID never appears in the open set).
        var open = await _sales.GetInvoicesAsync(
            "salesInvoices", 1, 50, null, "dueDate", "asc", null, null, null, ct);

        var today = DateTime.UtcNow.Date;

        var notifications = open.Items
            .Where(i => i.Status != "PAID")
            .Where(i => DateTime.TryParse(i.DueDate, out var due) && due.Date < today)
            .OrderBy(i => i.DueDate) // oldest due date first → most overdue surfaces first
            .Take(MaxPerKind)
            .Select(i =>
            {
                DateTime.TryParse(i.DueDate, out var due);
                var daysOverdue = (int)(today - due.Date).TotalDays;
                var severity = daysOverdue >= CriticalOverdueDays ? "CRITICAL" : "WARNING";
                return new NotificationDto
                {
                    Id = $"OVERDUE_INVOICE:{i.Id}",
                    Type = "OVERDUE_INVOICE",
                    Title = i.Number,
                    Message = $"Invoice {i.Number} for {i.CustomerName} is {daysOverdue} day(s) overdue.",
                    Severity = severity,
                    Link = $"/sales/invoices/{i.Id}",
                };
            })
            .ToList();

        await _cache.SetAsync(OverdueCacheKey, notifications, TimeSpan.FromSeconds(CacheSeconds), ct);
        return notifications;
    }

    private async Task<List<NotificationDto>> BuildLowStockNotificationsAsync(CancellationToken ct)
    {
        var cached = await _cache.GetAsync<List<NotificationDto>>(LowStockCacheKey, ct);
        if (cached != null) return cached;

        var items = await _items.GetItemsAsync(1, 50, null, null, null, null, null, ct);

        var notifications = items.Items
            .Where(i => i.IsLowStock)
            .OrderBy(i => i.QuantityOnHand) // lowest stock first
            .Take(MaxPerKind)
            .Select(i => new NotificationDto
            {
                Id = $"LOW_STOCK:{i.Id}",
                Type = "LOW_STOCK",
                Title = i.Description,
                Message = $"{i.Description} is low on stock ({i.QuantityOnHand} / min {i.MinimumStock} {i.UnitOfMeasure}).",
                Severity = "WARNING",
                Link = $"/items/{i.Id}",
            })
            .ToList();

        await _cache.SetAsync(LowStockCacheKey, notifications, TimeSpan.FromSeconds(CacheSeconds), ct);
        return notifications;
    }
}
