# Purchase API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only purchase-invoice and purchase-credit-memo list endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. All list endpoints share one service (`IPurchaseService`) and one response shape; they differ only in the BC collection they read. Mirrors the [Sales API](sales.md) but carries a vendor (`vendorName`) instead of a customer.

**Base path:** `/api/v1/purchase`
**Authentication:** Bearer JWT. Authorization policy `RequireFinancial` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`. `WAREHOUSE` is denied (403).

---

## Shared Query Parameters

All three endpoints accept the same query parameters:

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `page` | int | `1` | 1-based page number (clamped to >= 1) |
| `pageSize` | int | `20` | Items per page (reset to 20 if < 1 or > 100) |
| `search` | string | — | Case-insensitive contains-match on invoice `number` OR `vendorName` |
| `sortBy` | string | `date` | One of `date`, `dueDate`, `amount` (anything else falls back to default) |
| `sortDir` | string | `asc` | `asc` or `desc` |
| `status` | string | — | BC status filter (e.g. `Open`, `Paid`, `Partially Paid`); omitted = all |
| `fromDate` | string | — | Lower bound on posting date (ISO `yyyy-MM-dd`) |
| `toDate` | string | — | Upper bound on posting date (ISO `yyyy-MM-dd`) |

### Status enum (response `status` field)

Wire values are `SCREAMING_SNAKE_CASE` (per `.claude/rules/enums-and-constants.md`). The mapper normalizes BC casing to these values; the frontend maps each to a localized label via i18next key `purchase.status.<VALUE>`.

| Wire value | Meaning | BC source values |
|------------|---------|------------------|
| `OPEN` | Open / unpaid | `Open` (and any unknown) |
| `PARTIAL` | Partially paid | `Partially Paid`, `partial` |
| `PAID` | Fully paid | `Paid` |

### Credit-memo status enum

Purchase credit memos (`/credit-memos`) use a distinct lifecycle — `OPEN | POSTED` — not the invoice `OPEN/PARTIAL/PAID` lifecycle. The credit-memo normalizer maps invoice-only BC statuses (`Paid`, `Partially Paid`) to `OPEN`, since they are not part of the credit-memo lifecycle.

| Wire value | Meaning | BC source values |
|------------|---------|------------------|
| `OPEN` | Draft / unposted | `Open` (and any unknown) |
| `POSTED` | Booked / posted | `Posted` |

---

## GET /api/v1/purchase/invoices

**Description:** Paginated, filtered list of open purchase invoices.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "pinv001",
        "number": "PI-001",
        "vendorName": "Supplier A d.o.o.",
        "postingDate": "2026-01-15",
        "dueDate": "2026-02-14",
        "amount": 38000.00,
        "status": "OPEN"
      }
    ],
    "total": 10,
    "page": 1,
    "pageSize": 20
  }
}
```

**Error Responses:**
- `400 Bad Request` — malformed query parameters
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/purchase/posted-invoices

**Description:** Paginated, filtered list of posted purchase invoices. Same query params, response shape, and error responses as `/invoices` (reads the `purchaseInvoicesPosted` BC collection).

---

## GET /api/v1/purchase/credit-memos

**Description:** Paginated, filtered list of purchase credit memos. Same query params, response shape, and error responses as `/invoices` (reads the `purchaseCreditMemos` BC collection). The response `status` field uses the [credit-memo status enum](#credit-memo-status-enum) (`OPEN` / `POSTED`).

---

## Consumers

- Frontend list: `apps/web/app/(protected)/purchase/invoices/page.tsx` (US-009, PurchaseInvoiceListScreen — Open / Posted / Credit Memos tabs).
- Frontend detail (US-010, forthcoming): `apps/web/app/(protected)/purchase/invoices/[id]/page.tsx`.
