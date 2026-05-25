# Sales API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only sales-invoice list endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. All three endpoints share one service (`ISalesService`) and one response shape; they differ only in the BC collection they read.

**Base path:** `/api/v1/sales`
**Authentication:** Bearer JWT. Authorization policy `RequireFinancial` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`. `WAREHOUSE` is denied (403).

---

## Shared Query Parameters

All three endpoints accept the same query parameters:

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `page` | int | `1` | 1-based page number (clamped to >= 1) |
| `pageSize` | int | `20` | Items per page (reset to 20 if < 1 or > 100) |
| `search` | string | — | Case-insensitive contains-match on invoice `number` OR `customerName` |
| `sortBy` | string | `date` | One of `date`, `dueDate`, `amount` (anything else falls back to default) |
| `sortDir` | string | `asc` | `asc` or `desc` |
| `status` | string | — | BC status filter (e.g. `Open`, `Paid`, `Partially Paid`); omitted = all |
| `fromDate` | string | — | Lower bound on posting date (ISO `yyyy-MM-dd`) |
| `toDate` | string | — | Upper bound on posting date (ISO `yyyy-MM-dd`) |

### Status enum (response `status` field)

Wire values are `SCREAMING_SNAKE_CASE` (per `.claude/rules/enums-and-constants.md`). The mapper normalizes BC casing to these values; the frontend maps each to a localized label via i18next key `sales.status.<VALUE>`.

| Wire value | Meaning | BC source values |
|------------|---------|------------------|
| `OPEN` | Open / unpaid | `Open` (and any unknown) |
| `PARTIAL` | Partially paid | `Partially Paid`, `partial` |
| `PAID` | Fully paid | `Paid` |

---

## GET /api/v1/sales/invoices

**Description:** Paginated, filtered list of open sales invoices.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "inv001",
        "number": "SI-001",
        "customerName": "Acme d.o.o.",
        "postingDate": "2026-01-15",
        "dueDate": "2026-02-14",
        "amount": 45000.00,
        "status": "OPEN"
      }
    ],
    "total": 12,
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

## GET /api/v1/sales/posted-invoices

**Description:** Paginated, filtered list of posted sales invoices. Same query params, response shape, and error responses as `/invoices` (reads the `salesInvoicesPosted` BC collection).

---

## GET /api/v1/sales/credit-memos

**Description:** Paginated, filtered list of sales credit memos. Same query params, response shape, and error responses as `/invoices` (reads the `salesCreditMemos` BC collection).

---

## Consumers

- Frontend: `apps/web/app/(protected)/sales/invoices/page.tsx` (US-006, SalesInvoiceListScreen — Open / Posted / Credit Memos tabs).
