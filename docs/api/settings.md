# Settings API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Admin-only, read-only view of the Business Central connection configuration plus an on-demand
connectivity probe (US-022). These endpoints never modify any data.

**SECURITY:** The BC service-principal credentials (`ClientId`, `ClientSecret`) are **NEVER
returned** by any endpoint here. Only the non-sensitive connection target is exposed
(tenant — masked, company, environment, base URL, mock-mode flag). The tenant id is masked to
its first few characters; the full GUID is not echoed.

**Base path:** `/api/v1/settings`
**Authentication:** Bearer JWT, **ADMIN role required** (`RequireAdmin` policy). All other
roles receive `403 FORBIDDEN_INSUFFICIENT_ROLE`; missing/invalid token receives
`401 AUTH_REQUIRED`.

**Enums:**
- `environment`: free-form string derived from the BC base URL (e.g. `production`, `sandbox`); always `Mock` when `useMock` is `true`.

---

## GET /api/v1/settings/bc-config

Return the non-sensitive BC connection configuration and per-entity last-successful-sync
timestamps. Credentials are not part of the response shape.

**Authentication:** Bearer token, ADMIN.

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "tenantId": "tena…",
    "companyId": "company-001",
    "environment": "Mock",
    "baseUrl": "https://api.businesscentral.dynamics.com/v2.0/production",
    "useMock": true,
    "lastSync": [
      { "entityType": "customers", "lastSyncedAt": "2026-05-25T10:00:00.000Z" },
      { "entityType": "salesInvoices", "lastSyncedAt": null },
      { "entityType": "vendors", "lastSyncedAt": null },
      { "entityType": "items", "lastSyncedAt": null }
    ]
  }
}
```

`lastSyncedAt` is an ISO-8601 UTC timestamp, or `null` when that entity has not been synced
since startup (the UI renders this as "Never"). A successful `POST /bc-config/test` updates
every tracked entity's timestamp.

**Error responses:**
- `401 AUTH_REQUIRED` — no/invalid token
- `403 FORBIDDEN_INSUFFICIENT_ROLE` — authenticated but not ADMIN
- `500 INTERNAL_ERROR` — unexpected server error

---

## POST /api/v1/settings/bc-config/test

Trigger an on-demand BC connectivity probe (a lightweight `Top=1` read against the `customers`
collection). The endpoint **always returns 200** when the probe machinery runs; the
connectivity result (success or failure) is carried **inside the payload** under `data.success`.
A BC outage is reported as `data.success = false` with `data.errorCode`, **not** as an HTTP
error status. Credentials are never echoed in the result or in logs.

**Authentication:** Bearer token, ADMIN.

**Request body:** none.

**Response (200 OK) — success:**
```json
{
  "success": true,
  "data": {
    "success": true,
    "durationMs": 12,
    "lastSuccessfulSyncAt": "2026-05-25T10:00:00.000Z",
    "errorCode": null
  }
}
```

**Response (200 OK) — BC unreachable (probe ran, connectivity failed):**
```json
{
  "success": true,
  "data": {
    "success": false,
    "durationMs": 4012,
    "lastSuccessfulSyncAt": null,
    "errorCode": "INTEGRATION_BC_UNAVAILABLE"
  }
}
```

In mock mode (`BC.UseMock=true`) the probe always succeeds.

**Error responses:**
- `401 AUTH_REQUIRED` — no/invalid token
- `403 FORBIDDEN_INSUFFICIENT_ROLE` — authenticated but not ADMIN
- `500 INTERNAL_ERROR` — the probe machinery itself faulted (not a BC connectivity failure — those are reported inside `data`)

---

**Related:**
- `.claude/rules/api-documentation.md` — endpoint doc requirements
- `.claude/rules/security-and-auth.md` — RequireAdmin, no-credential-leak, no PII/secrets in logs
- `docs/api/admin-users.md` — sibling ADMIN-only endpoint group (US-021)
