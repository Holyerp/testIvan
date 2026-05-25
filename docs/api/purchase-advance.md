# Purchase Advance Invoices API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only purchase advance (proforma) invoice list + detail endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. Advance invoices track advance payments made to vendors. They live under the same `/api/v1/purchase` group and share the regular purchase-invoice list shape; the detail adds a payment-tracking block.

**Base path:** `/api/v1/purchase`
**Authentication:** Bearer JWT. Authorization policy `RequireFinancial` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`. `WAREHOUSE` is denied (403).

> **Q-003 — schema assumption (pending client confirmation).** The advance-invoice
> format is not yet confirmed by the client (standard BC schema vs. a BiH/SRB localized
> format). These endpoints are implemented against the **STANDARD BC advance-invoice
> schema** (header + line items + payment status). The shape is isolated behind
> `IBcMapper` (`PurchaseAdvanceInvoiceDetailMapper`) and the `MockBcHttpClient`
> `purchaseAdvanceInvoices` collection, so switching to a localized format later is a
> low-cost mapper + mock swap with no endpoint/contract change.

---

## Status enum (response `status` field)

Wire values are `SCREAMING_SNAKE_CASE` (per `.claude/rules/enums-and-constants.md`). The mapper normalizes BC casing to these values; the frontend maps each to a localized label via i18next key `purchase.status.<VALUE>`.

| Wire value | Meaning | BC source values |
|------------|---------|------------------|
| `OPEN` | Open / unpaid advance | `Open` (and any unknown) |
| `PARTIAL` | Partially paid advance | `Partially Paid`, `partial` |
| `PAID` | Fully paid advance | `Paid` |

---

## GET /api/v1/purchase/advance-invoices

**Description:** Paginated, filtered list of purchase advance (proforma) invoices. Reads the `purchaseAdvanceInvoices` BC collection. Same list item shape as `/api/v1/purchase/invoices`.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Query parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `page` | int | `1` | 1-based page number (clamped to >= 1) |
| `pageSize` | int | `20` | Items per page (reset to 20 if < 1 or > 100) |
| `search` | string | — | Case-insensitive contains-match on `number` OR `vendorName` |
| `sortBy` | string | `date` | One of `date`, `dueDate`, `amount` (anything else falls back to default) |
| `sortDir` | string | `asc` | `asc` or `desc` |
| `status` | string | — | BC status filter (e.g. `Open`, `Paid`, `Partially Paid`); omitted = all |
| `fromDate` | string | — | Lower bound on posting date (ISO `yyyy-MM-dd`) |
| `toDate` | string | — | Upper bound on posting date (ISO `yyyy-MM-dd`) |

OData `$filter` values are single-quote-escaped before composition (no filter injection).

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "pav001",
        "number": "PA-2026-001",
        "vendorName": "Supplier A d.o.o.",
        "postingDate": "2026-01-15",
        "dueDate": "2026-02-14",
        "amount": 50000.00,
        "status": "OPEN"
      }
    ],
    "total": 8,
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

## GET /api/v1/purchase/advance-invoices/{id}

**Description:** Purchase advance invoice detail — header, line items, computed totals, and a payment-tracking block. Loads the line items via the BC `purchaseInvoiceLines` `$expand` navigation property (standard schema).

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Path parameter:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC purchase-advance-invoice id (e.g. `pav001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "header": {
      "id": "pav001",
      "number": "PA-2026-001",
      "vendorName": "Supplier A d.o.o.",
      "postingDate": "2026-01-15",
      "dueDate": "2026-02-14",
      "paymentTerms": "30 dana",
      "ourReference": "REF-PA-2026-001",
      "status": "OPEN"
    },
    "lines": [
      {
        "description": "Sirovine i materijal",
        "quantity": 100,
        "unitPrice": 350.00,
        "vatPercent": 20,
        "lineTotal": 35000.00
      }
    ],
    "totals": {
      "subtotal": 67000.00,
      "vatAmount": 13400.00,
      "total": 80400.00
    },
    "paymentTracking": {
      "amount": 80400.00,
      "amountPaid": 0.00,
      "remaining": 80400.00
    }
  }
}
```

The `header.status` field uses the `SCREAMING_SNAKE_CASE` enum (`OPEN` / `PARTIAL` / `PAID`) documented above. Totals are computed from the lines: `subtotal = Σ lineTotal`, `vatAmount = Σ (lineTotal × vatPercent / 100)`, `total = subtotal + vatAmount`.

**Payment tracking** reports the advance-payment state. Invariant: `amount = amountPaid + remaining`. `amount` is the document total; `amountPaid` is reconciled from the payment status (`PAID` → full, `PARTIAL` → half, `OPEN` → 0) so the block is always internally consistent regardless of the amount base BC reports against.

**Error Responses:**
- `400 Bad Request` — malformed path parameter
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — advance-invoice id does not exist (`{ "success": false, "code": "NOT_FOUND_PURCHASE_ADVANCE_INVOICE" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## Consumers

- Frontend list: `apps/web/app/(protected)/purchase/advance-invoices/page.tsx` (US-015, PurchaseAdvanceInvoiceListScreen — number / vendor / date / amount / status, with an "Advance" tag distinguishing it from regular purchase invoices).
- Frontend detail: `apps/web/app/(protected)/purchase/advance-invoices/[id]/page.tsx` (US-015 — header + payment-tracking section + line items + totals; 404 → `NOT_FOUND_PURCHASE_ADVANCE_INVOICE`).
