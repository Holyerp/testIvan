# Vendors API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only vendor list endpoint backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. The list endpoint uses `IVendorService` (`BcListQuery` + `VendorMapper`) and the canonical paginated response shape. Mirrors the customer list (US-004); it is the vendor analogue.

**Base path:** `/api/v1/vendors`
**Authentication:** Bearer JWT. Authorization policy `RequireFinancial` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`. `WAREHOUSE` is denied (403).

---

## GET /api/v1/vendors

**Description:** Paginated, searchable, sortable list of vendors.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `page` | int | `1` | 1-based page number (clamped to >= 1) |
| `pageSize` | int | `20` | Items per page (reset to 20 if < 1 or > 100) |
| `search` | string | — | Case-insensitive contains-match on vendor `displayName` OR `number` |
| `sortBy` | string | `displayName` | One of `displayName`, `balance` (anything else falls back to `displayName`) |
| `sortDir` | string | `asc` | `asc` or `desc` |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "v001",
        "number": "V001",
        "displayName": "Supplier A d.o.o.",
        "city": "Beograd",
        "balance": 55000.00,
        "phone": "+381 11 2345678"
      }
    ],
    "total": 10,
    "page": 1,
    "pageSize": 20
  }
}
```

**Response fields (`items[]`):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC vendor id (used for the detail route `/vendors/{id}`) |
| `number` | string | Vendor number (e.g. `V001`) |
| `displayName` | string | Vendor display name |
| `city` | string | Vendor city |
| `balance` | decimal | Outstanding balance (RSD) |
| `phone` | string | Vendor phone number |

**Error Responses:**
- `400 Bad Request` — malformed query parameters
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial, e.g. `WAREHOUSE` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## Consumers

- Frontend list: `apps/web/app/(protected)/vendors/page.tsx` (US-011, VendorListScreen — paginated/searchable/sortable table; row click → `/vendors/{id}`, detail in US-012).
