# Analytics API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only management analytics endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. They use `IAnalyticsService`, which blends the dashboard KPI-aggregation pattern (fetch BC collections, aggregate in memory, 5-min cache) over sales (revenue) and purchase (expense) invoices, plus top customers and top items (US-020).

**Base path:** `/api/v1/analytics`
**Authentication:** Bearer JWT. Authorization policy `RequireFinancial` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`. `WAREHOUSE` is denied (403).

**Granularity enum** (request param + response field `granularity`): `MONTHLY` | `QUARTERLY` | `YEARLY` (wire format `SCREAMING_SNAKE_CASE`). Any other / missing value falls back to `MONTHLY`.

**Date range:** `fromDate` / `toDate` are inclusive ISO `yyyy-MM-dd` bounds. When omitted, the range defaults to the last 12 months ending today. The period comparison always compares the resolved range (current) against the immediately-preceding equal-length window (prior).

---

## GET /api/v1/analytics/revenue-expense

**Description:** Revenue vs. expense time series bucketed by the requested granularity, plus a current-vs-prior period comparison.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `granularity` | string | `MONTHLY` | One of `MONTHLY` (period label `yyyy-MM`), `QUARTERLY` (`yyyy-Qn`), `YEARLY` (`yyyy`). Invalid values fall back to `MONTHLY`. |
| `fromDate` | string | last 12 months | Inclusive ISO `yyyy-MM-dd` lower bound |
| `toDate` | string | today | Inclusive ISO `yyyy-MM-dd` upper bound |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "points": [
      { "period": "2025-11", "revenue": 45000.00, "expense": 38000.00, "profit": 7000.00 },
      { "period": "2025-12", "revenue": 78000.00, "expense": 64000.00, "profit": 14000.00 }
    ],
    "comparison": {
      "currentRevenue": 723000.00,
      "priorRevenue": 204000.00,
      "currentExpense": 591500.00,
      "priorExpense": 154000.00,
      "revenueDeltaPercent": 254.4,
      "expenseDeltaPercent": 284.1
    },
    "granularity": "MONTHLY"
  }
}
```

**Response fields:**

| Field | Type | Description |
|-------|------|-------------|
| `points[].period` | string | Bucket label; shape depends on `granularity` (`yyyy-MM` / `yyyy-Qn` / `yyyy`) |
| `points[].revenue` | decimal | Sum of sales-invoice totals in the bucket (RSD) |
| `points[].expense` | decimal | Sum of purchase-invoice totals in the bucket (RSD) |
| `points[].profit` | decimal | `revenue − expense` for the bucket |
| `comparison.currentRevenue` | decimal | Total revenue in the resolved range |
| `comparison.priorRevenue` | decimal | Total revenue in the prior equal-length window |
| `comparison.currentExpense` | decimal | Total expense in the resolved range |
| `comparison.priorExpense` | decimal | Total expense in the prior equal-length window |
| `comparison.revenueDeltaPercent` | decimal | `(current − prior) / prior × 100`, rounded to 1 dp; `0` when prior is `0` |
| `comparison.expenseDeltaPercent` | decimal | As above, for expense |
| `granularity` | string | Resolved granularity enum: `MONTHLY` \| `QUARTERLY` \| `YEARLY` |

**Caching:** Result is cached for 5 minutes (`BcOptions.CacheSeconds`), keyed by granularity + resolved range.

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial, e.g. `WAREHOUSE` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/analytics/top-customers

**Description:** Top customers by revenue (sum of sales-invoice totals) within the resolved date range.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `top` | int | `10` | Maximum number of customers to return (clamped to >= 1) |
| `fromDate` | string | last 12 months | Inclusive ISO `yyyy-MM-dd` lower bound |
| `toDate` | string | today | Inclusive ISO `yyyy-MM-dd` upper bound |

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    { "id": "c001", "name": "Acme d.o.o.", "revenue": 168000.00, "invoiceCount": 3 },
    { "id": "c002", "name": "Delta Corp", "revenue": 145500.00, "invoiceCount": 3 }
  ]
}
```

**Response fields (per row):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC customer id, or the customer name when no record resolves the name |
| `name` | string | Customer display name |
| `revenue` | decimal | Sum of that customer's sales-invoice totals in range (RSD), descending |
| `invoiceCount` | int | Number of sales invoices in range for that customer |

**Error Responses:** same as `/revenue-expense` (401 / 403 / 404 / 500 / 502).

---

## GET /api/v1/analytics/top-items

**Description:** Top items by sales volume (units sold), with the monetary value of those sales.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `top` | int | `10` | Maximum number of items to return (clamped to >= 1) |
| `fromDate` | string | last 12 months | Accepted for API symmetry; the sales-volume basis is a rolling aggregate |
| `toDate` | string | today | As above |

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    { "id": "itm004", "number": "ITM-004", "description": "Blok opeka", "salesVolume": 9800.0, "salesValue": 607600.00 },
    { "id": "itm010", "number": "ITM-010", "description": "Kabl PP-Y 3x1.5", "salesVolume": 8600.0, "salesValue": 989000.00 }
  ]
}
```

**Response fields (per row):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC item id |
| `number` | string | Item number (e.g. `ITM-004`) |
| `description` | string | Item description |
| `salesVolume` | decimal | Units sold (descending sort key) |
| `salesValue` | decimal | Monetary value of those sales (RSD) |

**Note on sales-volume source:** Invoice line items in this BC use free-text descriptions that are not linked to item records, so they cannot be aggregated by item. The sales-volume basis is therefore carried on the item entity (`BcItem.salesVolume` / `salesValue`) — in real BC this would be derived from item-ledger `Sale` entries; the dev mock carries it directly as an additive field.

**Error Responses:** same as `/revenue-expense` (401 / 403 / 404 / 500 / 502).

---

## Related

- `docs/api/sales.md`, `docs/api/purchase.md` — the invoice collections aggregated here
- `docs/api/items.md` — the item collection that carries the sales-volume basis
- `.claude/rules/enums-and-constants.md` — Granularity wire format (`SCREAMING_SNAKE_CASE`)
