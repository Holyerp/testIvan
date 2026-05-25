# Search API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Universal cross-entity search backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. Implemented by `ISearchService` (`SearchService`), which aggregates the four existing list services — `ICustomerService`, `IVendorService`, `ISalesService`, `IPurchaseService` — for a single query term and maps each list item into a uniform `SearchHitDto`. It does not query BC ad-hoc; it reuses the list services so OData filter escaping and paging stay in one place.

**Base path:** `/api/v1/search`
**Authentication:** Bearer JWT. Authorization policy `RequireDashboard` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`, `WAREHOUSE`. Search itself is available to ALL authenticated roles.

**RBAC (per-group data access):** the four entity types (customers, vendors, sales invoices, purchase invoices) are financial data. `WAREHOUSE` has no access to them (this mirrors the `RequireFinancial` gating on the list endpoints). A caller whose roles do not include a financial role (`ADMIN` / `MANAGER` / `ACCOUNTING`) — i.e. a `WAREHOUSE`-only caller — receives an empty result for every group, and BC is never queried for that caller. This keeps search reachable for everyone while returning only what each role may see.

---

## GET /api/v1/search

**Description:** Cross-entity search returning grouped hits across customers, vendors, sales invoices and purchase invoices. Each group is capped at `limit` hits.

**Authentication:** `RequireDashboard` (ADMIN / MANAGER / ACCOUNTING / WAREHOUSE)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `q` | string | — | Search term. Case-insensitive contains-match. If null/whitespace or shorter than 2 characters, all groups are returned empty (200, not 400) — this simplifies the debounced UI. |
| `limit` | int | `5` | Max hits per group. Clamped to the range `1..20`. |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "customers": [
      { "id": "c001", "type": "CUSTOMER", "title": "Acme d.o.o.", "subtitle": "C001" }
    ],
    "vendors": [
      { "id": "v001", "type": "VENDOR", "title": "Supplier A d.o.o.", "subtitle": "V001" }
    ],
    "salesInvoices": [
      { "id": "inv001", "type": "SALES_INVOICE", "title": "SI-001", "subtitle": "Acme d.o.o." }
    ],
    "purchaseInvoices": [
      { "id": "pinv001", "type": "PURCHASE_INVOICE", "title": "PI-001", "subtitle": "Supplier A d.o.o." }
    ]
  }
}
```

A `WAREHOUSE`-only caller, or a query shorter than 2 characters, returns all four groups as empty arrays:
```json
{
  "success": true,
  "data": { "customers": [], "vendors": [], "salesInvoices": [], "purchaseInvoices": [] }
}
```

**Response fields (each hit in every group):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC entity id. Used to build the detail route per type (see below). |
| `type` | string | Entity-type discriminator (SCREAMING_SNAKE wire value, see allowed values below). |
| `title` | string | Primary label — customer/vendor display name, or invoice number. |
| `subtitle` | string | Secondary label — customer/vendor number, or the invoice's party name. |

**Allowed `type` values** (cross-layer enum, SCREAMING_SNAKE_CASE wire format):

| Value | Group | Title source | Subtitle source | Frontend detail route |
|-------|-------|--------------|-----------------|-----------------------|
| `CUSTOMER` | `customers` | `displayName` | `number` | `/customers/{id}` |
| `VENDOR` | `vendors` | `displayName` | `number` | `/vendors/{id}` |
| `SALES_INVOICE` | `salesInvoices` | `number` | `customerName` | `/sales/invoices/{id}` |
| `PURCHASE_INVOICE` | `purchaseInvoices` | `number` | `vendorName` | `/purchase/invoices/{id}` |

The frontend maps `type` to an i18n group label (`search.group.<TYPE>`) and to the detail route above.

**Error Responses:**
- `400 Bad Request` — malformed query parameters (e.g. non-integer `limit`)
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireDashboard (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`). Note: WAREHOUSE is NOT forbidden here — it is allowed but receives empty financial groups.
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "error": "Search is unavailable", "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## Consumers

- Frontend topbar search: `apps/web/components/global-search.tsx` (US-013, SearchResultsScreen / global topbar search box). Debounced (300ms, min 2 chars), grouped dropdown, keyboard navigation; "View all" links to the respective list screen with `?search=` prefilled. Pure helpers (`flattenHits`, `hitHref`, `nextIndex`) live in `apps/web/lib/search.ts`.
- Mounted in the protected layout topbar: `apps/web/app/(protected)/layout.tsx`.
