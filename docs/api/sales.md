# Sales API

**Version:** 1.1
**Last Updated:** 2026-05-25
**Status:** Active

Read-only sales-invoice and credit-memo list + detail endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. All list endpoints share one service (`ISalesService`) and one response shape, as do the detail endpoints; they differ only in the BC collection they read and (for detail) the not-found code returned.

> Sales **advance (proforma) invoices** also live under `/api/v1/sales` (`/advance-invoices`, `/advance-invoices/{id}`) but carry a payment-tracking block on the detail — they are documented separately in [`sales-advance.md`](sales-advance.md) (US-014).

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

### Credit-memo status enum

Credit memos (`/credit-memos`, `/credit-memos/{id}`, `/posted-credit-memos`) use a distinct lifecycle — `OPEN | POSTED` — not the invoice `OPEN/PARTIAL/PAID` lifecycle. The credit-memo normalizer maps invoice-only BC statuses (`Paid`, `Partially Paid`) to `OPEN`, since they are not part of the credit-memo lifecycle. The frontend maps each value to a localized label via i18next key `creditMemos.status.<VALUE>`.

| Wire value | Meaning | BC source values |
|------------|---------|------------------|
| `OPEN` | Draft / unposted | `Open` (and any unknown) |
| `POSTED` | Booked / posted | `Posted` |

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

**Description:** Paginated, filtered list of open (draft) sales credit memos. Same query params, response shape, and error responses as `/invoices` (reads the `salesCreditMemos` BC collection). The response `status` field uses the [credit-memo status enum](#credit-memo-status-enum) (`OPEN` / `POSTED`).

---

## GET /api/v1/sales/posted-credit-memos

**Description:** Paginated, filtered list of posted sales credit memos. Same query params, response shape, and error responses as `/invoices` (reads the `salesCreditMemosPosted` BC collection). The response `status` field uses the [credit-memo status enum](#credit-memo-status-enum) — posted credit memos are `POSTED`.

---

## GET /api/v1/sales/credit-memos/{id}

**Description:** Sales credit memo detail — header, line items, and computed totals. Same response shape as `/invoices/{id}` (loads line items via the BC `salesInvoiceLines` `$expand` navigation property). `header.status` uses the [credit-memo status enum](#credit-memo-status-enum) (`OPEN` / `POSTED`).

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Path parameter:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC sales-credit-memo id (e.g. `scm001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "header": {
      "id": "scm001",
      "number": "SCM-001",
      "customerName": "Acme d.o.o.",
      "billToAddress": "Acme d.o.o., Bulevar Oslobođenja 12, Beograd",
      "postingDate": "2026-03-25",
      "dueDate": "2026-04-24",
      "paymentTerms": "30 dana",
      "status": "OPEN"
    },
    "lines": [
      {
        "description": "Konsultantske usluge",
        "quantity": 10,
        "unitPrice": 5000.00,
        "vatPercent": 20,
        "lineTotal": 50000.00
      }
    ],
    "totals": {
      "subtotal": 82000.00,
      "vatAmount": 16400.00,
      "total": 98400.00
    }
  }
}
```

Totals are computed from the lines: `subtotal = Σ lineTotal`, `vatAmount = Σ (lineTotal × vatPercent / 100)`, `total = subtotal + vatAmount`.

**Error Responses:**
- `400 Bad Request` — malformed path parameter
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — credit-memo id does not exist (`{ "success": false, "code": "NOT_FOUND_SALES_CREDIT_MEMO" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/sales/invoices/{id}

**Description:** Sales invoice detail — header, line items, and computed totals. Loads the line items via the BC `salesInvoiceLines` `$expand` navigation property.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Path parameter:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC sales-invoice id (e.g. `inv001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "header": {
      "id": "inv001",
      "number": "SI-001",
      "customerName": "Acme d.o.o.",
      "billToAddress": "Acme d.o.o., Bulevar Oslobođenja 12, Beograd",
      "postingDate": "2026-01-15",
      "dueDate": "2026-02-14",
      "paymentTerms": "30 dana",
      "status": "OPEN"
    },
    "lines": [
      {
        "description": "Konsultantske usluge",
        "quantity": 10,
        "unitPrice": 5000.00,
        "vatPercent": 20,
        "lineTotal": 50000.00
      }
    ],
    "totals": {
      "subtotal": 82000.00,
      "vatAmount": 16400.00,
      "total": 98400.00
    }
  }
}
```

The `header.status` field uses the same `SCREAMING_SNAKE_CASE` enum (`OPEN` / `PARTIAL` / `PAID`) documented in [Status enum](#status-enum-response-status-field). Totals are computed from the lines: `subtotal = Σ lineTotal`, `vatAmount = Σ (lineTotal × vatPercent / 100)`, `total = subtotal + vatAmount`.

**Error Responses:**
- `400 Bad Request` — malformed path parameter
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — invoice id does not exist (`{ "success": false, "code": "NOT_FOUND_SALES_INVOICE" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/sales/posted-invoices/{id}

**Description:** Posted sales invoice detail. Same path parameter, response shape, and error responses as `/invoices/{id}` (reads the `salesInvoicesPosted` BC collection). Unknown id → `404 NOT_FOUND_SALES_INVOICE`.

---

## Consumers

- Frontend list: `apps/web/app/(protected)/sales/invoices/page.tsx` (US-006, SalesInvoiceListScreen — Open / Posted / Credit Memos tabs).
- Frontend detail: `apps/web/app/(protected)/sales/invoices/[id]/page.tsx` (US-007, SalesInvoiceDetailScreen — header + line items + totals).
- Frontend credit-memo list: `apps/web/app/(protected)/sales/credit-memos/page.tsx` (US-008, SalesCreditMemoListScreen — Open / Posted tabs over `/credit-memos` and `/posted-credit-memos`).
- Frontend credit-memo detail: `apps/web/app/(protected)/sales/credit-memos/[id]/page.tsx` (US-008 — header + line items + totals; 404 → `NOT_FOUND_SALES_CREDIT_MEMO`).
