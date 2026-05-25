namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// In-app notification surfaced in the topbar bell (T-005). <see cref="Type"/> and
/// <see cref="Severity"/> are cross-layer enums in SCREAMING_SNAKE_CASE wire format
/// (per .claude/rules/enums-and-constants.md); the frontend maps them to i18n labels.
///   Type:     OVERDUE_INVOICE | LOW_STOCK
///   Severity: INFO | WARNING | CRITICAL
/// </summary>
public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;      // OVERDUE_INVOICE | LOW_STOCK
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;  // INFO | WARNING | CRITICAL
    public string? Link { get; set; }                     // e.g. /sales/invoices/{id} or /items/{id}
}
