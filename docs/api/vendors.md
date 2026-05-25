# Vendors API

**Version:** 1.1
**Last Updated:** 2026-05-25
**Status:** Active

Read-only vendor endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. They use `IVendorService` (`BcListQuery` + `VendorMapper` for the list; `GetByIdAsync` + the shared `PurchaseInvoiceMapper` for the detail/history) and the canonical response envelope. The list mirrors the customer list (US-004); the detail mirrors the customer detail (US-005). It is the vendor analogue.

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

## GET /api/v1/vendors/{id}

**Description:** Vendor detail — profile (header + financial summary) plus the vendor's purchase history (last 20 posted purchase invoices, newest first).

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Path Parameters:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC vendor id (e.g. `v001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "vendor": {
      "id": "v001",
      "number": "V001",
      "displayName": "Supplier A d.o.o.",
      "address": "Bulevar Kralja Aleksandra 73, Beograd",
      "city": "Beograd",
      "phone": "+381 11 2345678",
      "email": "info@supplier-a.rs",
      "vatNumber": "RS101234567",
      "balance": 55000.00,
      "paymentTerms": "30 dana"
    },
    "invoices": [
      {
        "id": "ppi001",
        "number": "PPI-001",
        "vendorName": "Supplier A d.o.o.",
        "postingDate": "2026-02-25",
        "dueDate": "2026-03-27",
        "amount": 56000.00,
        "status": "PAID"
      }
    ]
  }
}
```

**Response fields (`vendor`):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC vendor id |
| `number` | string | Vendor number (e.g. `V001`) |
| `displayName` | string | Vendor display name |
| `address` | string | Vendor street address |
| `city` | string | Vendor city |
| `phone` | string | Vendor phone number |
| `email` | string | Vendor email |
| `vatNumber` | string | Vendor VAT / tax number (PIB) |
| `balance` | decimal | Outstanding balance (RSD) |
| `paymentTerms` | string | Payment terms label (e.g. `30 dana`) |

**Response fields (`invoices[]`)** — reuses the purchase invoice list-item shape:

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC posted purchase invoice id |
| `number` | string | Invoice number |
| `vendorName` | string | Vendor display name |
| `postingDate` | string | Posting date, ISO `yyyy-MM-dd` |
| `dueDate` | string | Due date, ISO `yyyy-MM-dd` |
| `amount` | decimal | Total including tax (RSD) |
| `status` | string | `OPEN` \| `PARTIAL` \| `PAID` (SCREAMING_SNAKE wire format) |

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireFinancial, e.g. `WAREHOUSE` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — vendor id does not exist (`{ "success": false, "error": "Vendor not found", "code": "NOT_FOUND_VENDOR" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/vendors/{id}/invoices

**Description:** Vendor purchase history only — the last 20 posted purchase invoices for the vendor, newest first. Same data as the `invoices` array returned by `GET /api/v1/vendors/{id}`.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Path Parameters:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC vendor id (e.g. `v001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": "ppi001",
      "number": "PPI-001",
      "vendorName": "Supplier A d.o.o.",
      "postingDate": "2026-02-25",
      "dueDate": "2026-03-27",
      "amount": 56000.00,
      "status": "PAID"
    }
  ]
}
```

`data` is an array of purchase invoice list-items (same shape as `vendors/{id}` → `invoices[]` above). May be empty if the vendor has no posted purchase invoices.

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — role not in RequireFinancial (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — vendor id does not exist (`{ "success": false, "error": "Vendor not found", "code": "NOT_FOUND_VENDOR" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## Consumers

- Frontend list: `apps/web/app/(protected)/vendors/page.tsx` (US-011, VendorListScreen — paginated/searchable/sortable table; row click → `/vendors/{id}`).
- Frontend detail: `apps/web/app/(protected)/vendors/[id]/page.tsx` (US-012, VendorDetailScreen — profile header, financial summary, purchase history; consumes `GET /api/v1/vendors/{id}`).
