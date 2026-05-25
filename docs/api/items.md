# Items API

**Version:** 1.1
**Last Updated:** 2026-05-25
**Status:** Active

Read-only warehouse item (product / inventory) endpoints backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient`. They use `IItemService` (`BcListQuery` + `ItemMapper`) and the canonical response envelope. The list mirrors the vendor list (US-011) but belongs to the WAREHOUSE module rather than the financial one. This is the first warehouse-module endpoint (US-017).

**Base path:** `/api/v1/items`
**Authentication:** Bearer JWT. Authorization policy `RequireWarehouse` — roles `ADMIN`, `MANAGER`, `WAREHOUSE`. `ACCOUNTING` is denied (403).

---

## GET /api/v1/items

**Description:** Paginated, searchable, sortable, filterable list of warehouse items with stock levels.

**Authentication:** `RequireWarehouse` (ADMIN / MANAGER / WAREHOUSE)

**Query Parameters:**

| Param | Type | Default | Description |
|-------|------|---------|-------------|
| `page` | int | `1` | 1-based page number (clamped to >= 1) |
| `pageSize` | int | `20` | Items per page (reset to 20 if < 1 or > 100) |
| `search` | string | — | Case-insensitive contains-match on item `number` OR `description` |
| `sortBy` | string | `name` | One of `name` (→ description), `quantity` (→ quantity on hand), `unitCost` (anything else falls back to `name`) |
| `sortDir` | string | `asc` | `asc` or `desc` |
| `category` | string | — | Exact-match filter on item category (e.g. `GRAĐEVINA`, `ALATI`, `ELEKTRO`, `FARBE`) |
| `location` | string | — | Exact-match filter on item location (e.g. `MAGACIN-1`, `MAGACIN-2`) |

`search`, `category`, and `location` combine with logical AND when more than one is supplied.

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "itm002",
        "number": "ITM-002",
        "description": "Čelična šipka 12mm",
        "category": "GRAĐEVINA",
        "unitOfMeasure": "M",
        "quantityOnHand": 30,
        "minimumStock": 100,
        "unitCost": 320.00,
        "isLowStock": true
      }
    ],
    "total": 12,
    "page": 1,
    "pageSize": 20
  }
}
```

**Response fields (`items[]`):**

| Field | Type | Description |
|-------|------|-------------|
| `id` | string | BC item id (used for the detail route `/items/{id}`) |
| `number` | string | Item number (e.g. `ITM-001`) |
| `description` | string | Item description / name |
| `category` | string | Item category (e.g. `GRAĐEVINA`, `ALATI`, `ELEKTRO`, `FARBE`) |
| `unitOfMeasure` | string | Unit of measure (e.g. `KOM`, `KG`, `M`) |
| `quantityOnHand` | decimal | Current quantity on hand |
| `minimumStock` | decimal | Minimum stock threshold |
| `unitCost` | decimal | Unit cost (RSD) |
| `isLowStock` | boolean | `true` when `quantityOnHand < minimumStock` (computed server-side in `ItemMapper`) |

**Error Responses:**
- `400 Bad Request` — malformed query parameters
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireWarehouse, e.g. `ACCOUNTING` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — unknown route
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/items/{id}

**Description:** Full item detail — profile, stock split by warehouse location, and the most recent (up to 20) item ledger entries (stock movements). The item detail screen (US-018). Mirrors the vendor detail endpoint (US-012).

**Authentication:** `RequireWarehouse` (ADMIN / MANAGER / WAREHOUSE)

**Path Parameters:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC item id (e.g. `itm001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "item": {
      "id": "itm001",
      "number": "ITM-001",
      "description": "Cement 25kg",
      "category": "GRAĐEVINA",
      "unitOfMeasure": "KG",
      "quantityOnHand": 120,
      "minimumStock": 50,
      "unitCost": 650.00,
      "unitPrice": 780.00,
      "isLowStock": false
    },
    "stockByLocation": [
      { "location": "MAGACIN-1", "quantityOnHand": 72, "quantityReserved": 7.2 },
      { "location": "MAGACIN-2", "quantityOnHand": 48, "quantityReserved": 0 }
    ],
    "recentLedgerEntries": [
      { "date": "2026-05-23", "entryType": "SALE", "quantity": -5, "remaining": 120 },
      { "date": "2026-05-19", "entryType": "PURCHASE", "quantity": 20, "remaining": 125 }
    ]
  }
}
```

**Response fields:**

| Field | Type | Description |
|-------|------|-------------|
| `item` | object | Item profile (extends the list shape with `unitPrice`); `isLowStock` computed server-side (`quantityOnHand < minimumStock`) |
| `stockByLocation[]` | array | Per-location stock; `quantityOnHand` reconciles with `item.quantityOnHand` |
| `stockByLocation[].location` | string | Warehouse location code (e.g. `MAGACIN-1`) |
| `stockByLocation[].quantityReserved` | decimal | Quantity reserved at that location |
| `recentLedgerEntries[]` | array | Up to 20 most recent movements, newest first (same shape as `GET /{id}/ledger-entries`) |

The `recentLedgerEntries[].entryType` field is an enum — see the allowed values under `GET /{id}/ledger-entries` below.

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireWarehouse, e.g. `ACCOUNTING` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — item id does not exist (`{ "success": false, "code": "NOT_FOUND_ITEM" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

## GET /api/v1/items/{id}/ledger-entries

**Description:** The most recent (up to 20) item ledger entries (stock movements) for an item, newest first.

**Authentication:** `RequireWarehouse` (ADMIN / MANAGER / WAREHOUSE)

**Path Parameters:**

| Param | Type | Description |
|-------|------|-------------|
| `id` | string | BC item id (e.g. `itm001`) |

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    { "date": "2026-05-23", "entryType": "SALE", "quantity": -5, "remaining": 120 },
    { "date": "2026-05-19", "entryType": "PURCHASE", "quantity": 20, "remaining": 125 },
    { "date": "2026-05-14", "entryType": "TRANSFER", "quantity": -3, "remaining": 105 }
  ]
}
```

**Response fields (`data[]`):**

| Field | Type | Description |
|-------|------|-------------|
| `date` | string | Movement date, ISO `yyyy-MM-dd` |
| `entryType` | string (enum) | Movement type — see allowed values below |
| `quantity` | decimal | Signed quantity: `+` inbound, `−` outbound |
| `remaining` | decimal | Running balance after this entry |

**`entryType` allowed values** (SCREAMING_SNAKE_CASE wire format; the frontend maps each to an i18n label under `items.ledgerType.*`):

| Value | Meaning |
|-------|---------|
| `PURCHASE` | Inbound from a purchase |
| `SALE` | Outbound to a sale |
| `ADJUSTMENT` | Manual stock correction |
| `TRANSFER` | Movement between locations |

**Error Responses:**
- `401 Unauthorized` — missing/invalid token (`{ "success": false, "code": "AUTH_REQUIRED" }`)
- `403 Forbidden` — authenticated but role not in RequireWarehouse, e.g. `ACCOUNTING` (`{ "success": false, "code": "FORBIDDEN_INSUFFICIENT_ROLE" }`)
- `404 Not Found` — item id does not exist (`{ "success": false, "code": "NOT_FOUND_ITEM" }`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC upstream unavailable (`{ "success": false, "code": "INTEGRATION_BC_UNAVAILABLE" }`)

---

**Related:**
- `apps/api/Application/Items/ItemService.cs` — list + detail + ledger service
- `apps/api/Application/Mapping/ItemMapper.cs` — `BcItem` → `ItemListItemDto` projection (computes `isLowStock`)
- `apps/api/Application/Mapping/ItemDetailMapper.cs` — detail profile + stock-by-location + ledger-entry projections
- `apps/api/Domain/Constants/ItemLedgerEntryType.cs` — `entryType` wire values (PURCHASE / SALE / ADJUSTMENT / TRANSFER)
- `apps/api/Presentation/Endpoints/ItemsEndpoints.cs` — endpoint definitions (`RequireWarehouse`)
- `docs/api/vendors.md` — the financial-module detail analogue this endpoint mirrors
