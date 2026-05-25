# Notifications API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

In-app notification feed surfaced by the topbar bell (T-005). Implemented by `INotificationService` (`NotificationService`), which aggregates actionable alerts from the existing list services — `ISalesService` and `IItemService` — backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. It does not query BC ad-hoc; it reuses the list services so OData filter escaping and paging stay in one place. Each notification kind is cached briefly (60s) so repeated bell fetches do not re-hit BC.

**Base path:** `/api/v1/notifications`
**Authentication:** Bearer JWT. Authorization policy `RequireDashboard` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`, `WAREHOUSE`. The endpoint is available to ALL authenticated roles; the notification list itself is role-filtered (see below).

**RBAC (role-based notification filtering):** notifications mirror the financial/warehouse module-access model.

| Notification kind | Visible to roles |
|-------------------|------------------|
| `OVERDUE_INVOICE` (financial alert) | `ADMIN`, `MANAGER`, `ACCOUNTING` |
| `LOW_STOCK` (warehouse alert) | `ADMIN`, `MANAGER`, `WAREHOUSE` |

`ADMIN` and `MANAGER` see both kinds. A `WAREHOUSE`-only caller receives only `LOW_STOCK` notifications (never financial `OVERDUE_INVOICE`); an `ACCOUNTING`-only caller receives only `OVERDUE_INVOICE` (never `LOW_STOCK`). This keeps the bell reachable for everyone while returning only what each role may act on.

---

## GET /api/v1/notifications

**Description:** Returns the actionable-alert list for the caller, role-filtered per the table above. Two notification kinds are aggregated:

- **`OVERDUE_INVOICE`** — open sales invoices (`salesInvoices`) whose `dueDate` is in the past and whose status is not `PAID`. One notification per invoice, ordered oldest-due-date first, capped at 10. Severity is `WARNING`, escalating to `CRITICAL` when 30+ days overdue.
- **`LOW_STOCK`** — items where `isLowStock` is true (`quantityOnHand < minimumStock`), ordered lowest-stock first, capped at 10. Severity `WARNING`.

**Authentication:** `RequireDashboard` (ADMIN / MANAGER / ACCOUNTING / WAREHOUSE)

**Query Parameters:** none.

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": "OVERDUE_INVOICE:inv007",
      "type": "OVERDUE_INVOICE",
      "title": "SI-007",
      "message": "Invoice SI-007 for Acme d.o.o. is 10 day(s) overdue.",
      "severity": "WARNING",
      "link": "/sales/invoices/inv007"
    },
    {
      "id": "LOW_STOCK:itm002",
      "type": "LOW_STOCK",
      "title": "Čelična šipka 12mm",
      "message": "Čelična šipka 12mm is low on stock (30 / min 100 M).",
      "severity": "WARNING",
      "link": "/items/itm002"
    }
  ]
}
```

A caller with no actionable alerts (or a role that maps to neither kind) returns an empty array:
```json
{ "success": true, "data": [] }
```

**Response fields (each notification):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | Stable per-notification id (`{TYPE}:{entityId}`). |
| `type` | string | Notification-kind discriminator (SCREAMING_SNAKE wire value, see allowed values below). |
| `title` | string | Primary label — invoice number, or item description. |
| `message` | string | Human-readable English summary. The frontend may show this directly or map `type`/`severity` to localized labels. |
| `severity` | string | Severity discriminator (SCREAMING_SNAKE wire value, see allowed values below). |
| `link` | string \| null | Frontend route the notification deep-links to. |

**Allowed `type` values** (cross-layer enum, SCREAMING_SNAKE_CASE wire format):

| Value | Trigger | Title source | Link | i18n label key |
|-------|---------|--------------|------|----------------|
| `OVERDUE_INVOICE` | Open sales invoice past `dueDate`, not `PAID` | invoice `number` | `/sales/invoices/{id}` | `notifications.type.OVERDUE_INVOICE` |
| `LOW_STOCK` | Item with `quantityOnHand < minimumStock` | item `description` | `/items/{id}` | `notifications.type.LOW_STOCK` |

**Allowed `severity` values** (cross-layer enum, SCREAMING_SNAKE_CASE wire format):

| Value | Meaning | i18n label key |
|-------|---------|----------------|
| `INFO` | Informational | `notifications.severity.INFO` |
| `WARNING` | Needs attention | `notifications.severity.WARNING` |
| `CRITICAL` | Urgent (e.g. invoice 30+ days overdue) | `notifications.severity.CRITICAL` |

The frontend maps `type` and `severity` to i18n labels (`notifications.type.<TYPE>` / `notifications.severity.<SEVERITY>`) and renders a severity-colored dot; it never displays the raw wire value.

**Error Responses:**
- `400 Bad Request` — malformed request (not applicable: the endpoint takes no parameters)
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireDashboard (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`). Note: a permitted role with no alerts for its kind is NOT forbidden — it receives an empty array.
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "error": "Notifications are unavailable", "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## Export utility (no endpoint)

T-005 also provides a **client-side export utility** — `apps/web/lib/export.ts` — for downloading list data as Excel (`.xlsx`, via `xlsx`) or PDF (via `jspdf` + `jspdf-autotable`). There is **no export API endpoint**: export runs entirely in the browser from data already loaded into a list screen. The data-shaping core (`toRowMatrix`, `extractHeaders`, `rowToCells`) is a set of pure functions; `exportToExcel` / `exportToPdf` wrap them with the library + browser download. Consumed by US-020 (analytics dashboard) and US-023 (audit log export).

---

## Consumers

- Frontend topbar notification bell: `apps/web/components/notification-bell.tsx`. Fetches this endpoint with the Bearer token, shows an unread-count badge, and opens a dropdown listing each notification (severity-colored dot, title, message) that deep-links to `link` on click. Pure helpers (`notificationSeverityClass`, `unreadCount`) live in `apps/web/lib/notifications.ts`.
- Mounted in the protected layout topbar: `apps/web/app/(protected)/layout.tsx`.
- Email service: `IEmailService` (`Application/Interfaces/IEmailService.cs`) with the dev no-op `LoggingEmailService` (`Infrastructure/Email/LoggingEmailService.cs`) is also part of T-005. It logs (recipient + subject only — never the body) and does not send. Production wires a real MailKit SMTP implementation via configuration. US-021 (password reset) depends on this interface.
