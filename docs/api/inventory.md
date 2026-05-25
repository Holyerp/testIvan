# Inventory API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only warehouse inventory overview endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. They use `IInventoryService`, which aggregates the warehouse `items` collection (and each item's stock-by-location data) and returns the canonical response envelope. They blend the Dashboard KPI-aggregation pattern (US-003) with the warehouse item / stock-by-location data (US-017 / US-018). The summary is cached for the BC cache window (5 minutes) per filter combination. WAREHOUSE module (US-019).

**Base path:** `/api/v1/inventory`
**Authentication:** Bearer JWT. Authorization policy `RequireWarehouse` — roles `ADMIN`, `MANAGER`, `WAREHOUSE`. `ACCOUNTING` is denied (403).

---

## GET /api/v1/inventory/summary

**Description:** Inventory KPI summary — total items, total stock value, and the count of items below their minimum stock. Optionally narrowed by location and/or category before aggregating.

**Authentication:** `RequireWarehouse` (ADMIN / MANAGER / WAREHOUSE)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `location` | string | — | Exact-match filter on item location (e.g. `MAGACIN-1`, `MAGACIN-2`) applied before aggregating |
| `category` | string | — | Exact-match filter on item category (e.g. `GRAĐEVINA`, `ALATI`, `ELEKTRO`, `FARBE`) applied before aggregating |

`location` and `category` combine with logical AND when both are supplied. Filter values are escaped per OData string-literal rules.

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "totalItems": 12,
    "totalStockValue": 1234560.00,
    "itemsBelowMinimum": 4
  }
}
```

**Response fields (`data`):**

| Field | Type | Description |
|-------|------|-------------|
| `totalItems` | int | Number of items in scope (after any filter) |
| `totalStockValue` | decimal | Σ `quantityOnHand` × `unitCost` across items in scope (RSD) |
| `itemsBelowMinimum` | int | Count of items where `quantityOnHand < minimumStock` |

**Error Responses:**
- `400 Bad Request` — malformed query parameters
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireWarehouse, e.g. `ACCOUNTING` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/inventory/locations

**Description:** Stock-by-location breakdown — per warehouse location, the number of stock rows, total quantity, and total value. Grouped across every item's stock-by-location data (US-018).

**Authentication:** `RequireWarehouse` (ADMIN / MANAGER / WAREHOUSE)

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    { "location": "MAGACIN-1", "itemCount": 12, "totalQuantity": 4200, "totalValue": 720000.00 },
    { "location": "MAGACIN-2", "itemCount": 12, "totalQuantity": 3100, "totalValue": 514560.00 }
  ]
}
```

**Response fields (`data[]`):**

| Field | Type | Description |
|-------|------|-------------|
| `location` | string | Warehouse location code (e.g. `MAGACIN-1`) |
| `itemCount` | int | Number of stock rows at that location |
| `totalQuantity` | decimal | Total quantity on hand at that location |
| `totalValue` | decimal | Total value at that location (Σ row quantity × the item's unit cost, RSD); the per-location values reconcile with `summary.totalStockValue` |

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireWarehouse, e.g. `ACCOUNTING` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/inventory/low-stock

**Description:** Items whose quantity on hand is below their minimum stock threshold, sorted by quantity on hand ascending (most depleted first).

**Authentication:** `RequireWarehouse` (ADMIN / MANAGER / WAREHOUSE)

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    { "id": "itm006", "number": "ITM-006", "description": "Brusilica ugaona 125mm", "quantityOnHand": 5, "minimumStock": 10, "location": "MAGACIN-1" },
    { "id": "itm009", "number": "ITM-009", "description": "Izolaciona traka", "quantityOnHand": 6, "minimumStock": 25, "location": "MAGACIN-2" }
  ]
}
```

**Response fields (`data[]`):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC item id (used for the detail route `/items/{id}`) |
| `number` | string | Item number (e.g. `ITM-006`) |
| `description` | string | Item description / name |
| `quantityOnHand` | decimal | Current quantity on hand |
| `minimumStock` | decimal | Minimum stock threshold |
| `location` | string | Item primary location code (e.g. `MAGACIN-1`) |

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireWarehouse, e.g. `ACCOUNTING` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

**Related:**
- `apps/api/Application/Inventory/InventoryService.cs` — aggregation service (summary + by location + low stock)
- `apps/api/Application/DTOs/InventorySummaryDto.cs` — `InventorySummaryDto` / `InventoryLocationDto` / `LowStockItemDto`
- `apps/api/Presentation/Endpoints/InventoryEndpoints.cs` — endpoint definitions (`RequireWarehouse`)
- `docs/api/items.md` — the item list / detail / ledger endpoints whose data this overview aggregates
