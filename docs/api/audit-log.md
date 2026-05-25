# Audit Log API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Admin-only read view over the LOCAL Postgres `audit_logs` table (US-023). Returns a paginated,
newest-first slice of security and administrative events recorded by US-021 (login + admin
actions). This endpoint reads local data only — Business Central is never touched, so the
`502 INTEGRATION_BC_UNAVAILABLE` path does not apply.

**Base path:** `/api/v1/admin/audit-log`
**Authentication:** Bearer JWT, **ADMIN role required** (`RequireAdmin` policy). All other
roles receive `403 FORBIDDEN_INSUFFICIENT_ROLE`; missing/invalid token receives
`401 AUTH_REQUIRED`.

**Enums:**
- `category`: `LOGIN` | `VIEW` | `EXPORT` | `ADMIN` (SCREAMING_SNAKE wire values). Granular
  action codes are mapped to a category: `AUTH_LOGIN*` → `LOGIN`, `*EXPORT*` → `EXPORT`,
  `*VIEW*` → `VIEW`, `ADMIN_*` and everything else → `ADMIN`.

---

## GET /api/v1/admin/audit-log

Return a paginated, newest-first list of audit-log entries, optionally filtered by category,
username, and date range.

**Authentication:** Bearer token, ADMIN.

**Query parameters:**
- `page` — page number, 1-based (default `1`; values `< 1` clamp to `1`).
- `pageSize` — rows per page (default `20`, max `100`; values out of range are clamped).
- `category` — optional; one of `LOGIN` | `VIEW` | `EXPORT` | `ADMIN`. Unknown values are
  ignored (treated as no category filter).
- `username` — optional; case-insensitive "contains" match on the actor username.
- `fromDate` — optional; ISO date or datetime. Filters `CreatedAt >= fromDate` (UTC).
- `toDate` — optional; ISO date or datetime. Filters `CreatedAt <= toDate`; a bare date is
  treated as inclusive of the whole day (until 23:59:59.9999999 UTC).

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "3f2a8c10-1b2c-4d5e-8f90-abcdef012345",
        "timestamp": "2026-05-25T08:30:00.0000000Z",
        "username": "admin",
        "action": "ADMIN_USER_CREATED",
        "category": "ADMIN",
        "entityType": "USER",
        "entityId": "abc-123",
        "details": "Created user jane (id=abc-123, role=MANAGER)"
      },
      {
        "id": "9d1e7b44-aa55-4c33-9911-001122334455",
        "timestamp": "2026-05-25T08:15:00.0000000Z",
        "username": "admin",
        "action": "AUTH_LOGIN_SUCCESS",
        "category": "LOGIN",
        "entityType": null,
        "entityId": null,
        "details": null
      }
    ],
    "total": 2,
    "page": 1,
    "pageSize": 20
  }
}
```

Field notes:
- `timestamp` — ISO 8601 (UTC) of `CreatedAt`.
- `action` — the granular SCREAMING_SNAKE action code stored in the DB (e.g.
  `AUTH_LOGIN_SUCCESS`, `ADMIN_USER_CREATED`, `ADMIN_USER_UPDATED`, `ADMIN_USER_DELETED`,
  `ADMIN_PASSWORD_RESET`).
- `category` — one of the four category enum values above, derived from `action`.
- `entityType` / `entityId` — best-effort extraction from the action code + `details` text
  (e.g. `USER` + the `id=...` token); `null` when nothing can be inferred.
- `details` — the free-text detail string the audit writer recorded. The audit table never
  stores passwords or tokens, so none are surfaced here.

**Error responses:**
- `401 AUTH_REQUIRED` — no/invalid token
- `403 FORBIDDEN_INSUFFICIENT_ROLE` — authenticated but not ADMIN
- `500 INTERNAL_ERROR` — unexpected server error

---

## Related

- `docs/api/admin-users.md` — the admin actions (US-021) that write the `ADMIN_*` entries shown here
- `.claude/rules/api-documentation.md` — endpoint doc requirements
- `.claude/rules/enums-and-constants.md` — SCREAMING_SNAKE wire format for the `category` enum
