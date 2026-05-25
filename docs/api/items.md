# Items API

**Version:** 1.0
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

**Related:**
- `apps/api/Application/Items/ItemService.cs` — list service (query builder + mapper)
- `apps/api/Application/Mapping/ItemMapper.cs` — `BcItem` → `ItemListItemDto` projection (computes `isLowStock`)
- `apps/api/Presentation/Endpoints/ItemsEndpoints.cs` — endpoint definition (`RequireWarehouse`)
- `docs/api/vendors.md` — the financial-module list analogue this endpoint mirrors
