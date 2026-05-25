using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Build the actionable-alert list for the caller, filtered by the caller's role(s):
    /// financial roles (ADMIN / MANAGER / ACCOUNTING) see OVERDUE_INVOICE notifications;
    /// warehouse-capable roles (ADMIN / MANAGER / WAREHOUSE) see LOW_STOCK notifications.
    /// ADMIN / MANAGER see both. Aggregates overdue open sales invoices and low-stock items.
    /// </summary>
    Task<List<NotificationDto>> GetNotificationsAsync(
        IEnumerable<string> roles,
        CancellationToken ct = default);
}
