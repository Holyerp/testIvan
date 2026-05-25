# Credit Documents API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Read-only unified list + detail endpoints for correction documents backed by Microsoft Dynamics 365 Business Central (BC) via `IBcHttpClient` (US-016). A single collection mixes three document types — credit memos, debit memos, and storno (cancellation) invoices — each referencing the original invoice it corrects.

**Base path:** `/api/v1/credit-documents`
**Authentication:** Bearer JWT. Authorization policy `RequireFinancial` — roles `ADMIN`, `MANAGER`, `ACCOUNTING`. `WAREHOUSE` is denied (403).

---

## Type enum (response `type` field, request `type` filter)

Wire values are `SCREAMING_SNAKE_CASE` (per `.claude/rules/enums-and-constants.md`). The mapper normalizes BC casing to these values; the frontend maps each to a localized label via i18next key `creditDocuments.type.<VALUE>`.

| Wire value | Meaning |
|------------|---------|
| `CREDIT_MEMO` | Credit memo — a credit issued against the original invoice |
| `DEBIT_MEMO` | Debit memo — an additional charge against the original invoice |
| `STORNO` | Storno — a cancellation (reversal) of the original invoice |

## Status enum (response `status` field)

Correction documents use the OPEN | POSTED lifecycle (not the OPEN/PARTIAL/PAID invoice lifecycle).

| Wire value | Meaning | BC source values |
|------------|---------|------------------|
| `OPEN` | Draft / unposted | `Open` (and any unknown) |
| `POSTED` | Booked | `Posted` |

---

## GET /api/v1/credit-documents

**Description:** Paginated, filtered list of correction documents (credit memos, debit memos, storno) across the unified `creditDocuments` BC collection.

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Query parameters:**

| Param | Type | Default | Notes |
|-------|------|---------|-------|
| `page` | int | 1 | Clamped to ≥ 1 |
| `pageSize` | int | 20 | Reset to 20 if < 1 or > 100 |
| `search` | string | — | Matches `number` or `partyName` (case-insensitive contains) |
| `sortBy` | string | `date` | Allowed: `date`, `number`, `amount` |
| `sortDir` | string | `asc` | `asc` or `desc` |
| `type` | string | — | One of `CREDIT_MEMO`, `DEBIT_MEMO`, `STORNO`. Unknown values are ignored |
| `fromDate` | string | — | ISO `yyyy-MM-dd`; `postingDate >= fromDate` |
| `toDate` | string | — | ISO `yyyy-MM-dd`; `postingDate <= toDate` |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "cd001",
        "number": "CN-2026-001",
        "type": "CREDIT_MEMO",
        "partyName": "Acme d.o.o.",
        "postingDate": "2026-01-05",
        "amount": 12000.00,
        "originalInvoiceNumber": "SI-001",
        "status": "POSTED"
      }
    ],
    "total": 12,
    "page": 1,
    "pageSize": 20
  }
}
```

**Error responses:**
- `401 Unauthorized` — missing/invalid token (`AUTH_REQUIRED`)
- `403 Forbidden` — authenticated but lacking a financial role, e.g. `WAREHOUSE` (`FORBIDDEN_INSUFFICIENT_ROLE`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC integration unavailable (`INTEGRATION_BC_UNAVAILABLE`)

---

## GET /api/v1/credit-documents/{id}

**Description:** Detail of a single correction document — header (incl. document type, party, posting date, status, and the original-invoice reference), line items, and computed totals (subtotal, VAT, total).

**Authentication:** `RequireFinancial` (ADMIN / MANAGER / ACCOUNTING)

**Path parameters:**

| Param | Type | Notes |
|-------|------|-------|
| `id` | string | Credit-document id |

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "header": {
      "id": "cd001",
      "number": "CN-2026-001",
      "type": "CREDIT_MEMO",
      "partyName": "Acme d.o.o.",
      "postingDate": "2026-01-05",
      "originalInvoiceNumber": "SI-001",
      "status": "POSTED"
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
      "subtotal": 50000.00,
      "vatAmount": 10000.00,
      "total": 60000.00
    }
  }
}
```

**Error responses:**
- `401 Unauthorized` — missing/invalid token (`AUTH_REQUIRED`)
- `403 Forbidden` — lacking a financial role (`FORBIDDEN_INSUFFICIENT_ROLE`)
- `404 Not Found` — unknown id (`NOT_FOUND_CREDIT_DOCUMENT`)
- `500 Internal Server Error` — unexpected server error
- `502 Bad Gateway` — BC integration unavailable (`INTEGRATION_BC_UNAVAILABLE`)

---

## Notes

- The `type` field is a cross-layer enum: the allowed wire values above are the single source of truth shared by the backend (`CreditDocumentType`), the OData filter, and the frontend i18n labels.
- Line items and totals reuse the sales-invoice line/totals shape (`SalesInvoiceLineDto` / `SalesInvoiceTotalsDto`) — a correction document carries the same money math.
- In mock mode (`BC.UseMock=true`) the `creditDocuments` collection provides ~12 records mixing all three types, each referencing an existing mock sales invoice number.
