# Admin User Management API

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

Admin-only management of LOCAL portal accounts (US-021). These endpoints write to the
local Postgres `users` table only — Business Central data is never modified. Every mutation
writes an audit-log row (see [Audit](#audit)).

**Base path:** `/api/v1/admin/users`
**Authentication:** Bearer JWT, **ADMIN role required** (`RequireAdmin` policy). All other
roles receive `403 FORBIDDEN_INSUFFICIENT_ROLE`; missing/invalid token receives
`401 AUTH_REQUIRED`.

**Enums:**
- `role`: `ADMIN` | `MANAGER` | `ACCOUNTING` | `WAREHOUSE`
- `status`: `ACTIVE` | `INACTIVE` (derived from `IsActive`; not settable directly — use `isActive` on update)

---

## GET /api/v1/admin/users

List all portal users. The password hash is never included.

**Authentication:** Bearer token, ADMIN.

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": "8f1c...",
      "name": "Admin User",
      "email": "admin@pinoles.local",
      "username": "admin",
      "role": "ADMIN",
      "status": "ACTIVE",
      "lastLoginAt": "2026-05-25T08:30:00Z",
      "createdAt": "2026-05-25T07:00:00Z"
    }
  ]
}
```

**Error responses:**
- `401 AUTH_REQUIRED` — no/invalid token
- `403 FORBIDDEN_INSUFFICIENT_ROLE` — authenticated but not ADMIN
- `500 INTERNAL_ERROR` — unexpected server error

---

## POST /api/v1/admin/users

Create a new portal user. The temporary password is bcrypt-hashed (cost 12) and never
stored or returned in plaintext.

**Authentication:** Bearer token, ADMIN.

**Request body:**
```json
{
  "name": "Jane Doe",
  "email": "jane@pinoles.local",
  "username": "jane",
  "role": "MANAGER",
  "tempPassword": "TempPass123!"
}
```
- `name` — required
- `email` — optional; must be unique when provided
- `username` — required; must be unique
- `role` — required; one of the role enum values
- `tempPassword` — required; minimum 8 characters

**Response (201 Created):** the created user (same shape as the list item, `status: "ACTIVE"`).

**Error responses:**
- `400 VALIDATION_REQUIRED_FIELDS` — name/username/tempPassword missing
- `400 VALIDATION_PASSWORD_TOO_SHORT` — temp password < 8 chars
- `400 VALIDATION_INVALID_ROLE` — role not in the enum
- `401 AUTH_REQUIRED`
- `403 FORBIDDEN_INSUFFICIENT_ROLE`
- `404 NOT_FOUND_USER` — n/a for create
- `409 CONFLICT_USERNAME_TAKEN` — username already exists
- `409 CONFLICT_EMAIL_TAKEN` — email already exists
- `500 INTERNAL_ERROR`

**Audit:** writes `ADMIN_USER_CREATED`.

---

## PUT /api/v1/admin/users/{id}

Update a user's role and active status. Other fields are immutable through this endpoint.

**Authentication:** Bearer token, ADMIN.

**Request body:**
```json
{
  "role": "ACCOUNTING",
  "isActive": false
}
```
- `role` — required; one of the role enum values
- `isActive` — required boolean (deactivating sets `status` to `INACTIVE`)

**Response (200 OK):** the updated user.

**Guards:**
- The last active ADMIN cannot be deactivated or demoted → `409 CONFLICT_LAST_ADMIN`.

**Error responses:**
- `400 VALIDATION_INVALID_ROLE`
- `401 AUTH_REQUIRED`
- `403 FORBIDDEN_INSUFFICIENT_ROLE`
- `404 NOT_FOUND_USER` — no user with that id
- `409 CONFLICT_LAST_ADMIN` — would remove the last active admin
- `500 INTERNAL_ERROR`

**Audit:** writes `ADMIN_USER_UPDATED`.

---

## POST /api/v1/admin/users/{id}/reset-password

Generate a new temporary password for the user, bcrypt-hash it, revoke the user's active
refresh tokens, and email the temporary password (no-op logging email service in dev). The
plaintext password is never logged or returned.

**Authentication:** Bearer token, ADMIN.

**Request body:** none.

**Response (200 OK):** the user (no password in the body).
```json
{ "success": true, "data": { "id": "...", "username": "jane", "role": "MANAGER", "status": "ACTIVE", "...": "..." } }
```

**Error responses:**
- `401 AUTH_REQUIRED`
- `403 FORBIDDEN_INSUFFICIENT_ROLE`
- `404 NOT_FOUND_USER`
- `500 INTERNAL_ERROR`

**Audit:** writes `ADMIN_PASSWORD_RESET`.

---

## DELETE /api/v1/admin/users/{id}

Delete a portal user.

**Authentication:** Bearer token, ADMIN.

**Response (200 OK):**
```json
{ "success": true, "data": null }
```

**Guards:**
- An admin cannot delete their own account → `409 CONFLICT_CANNOT_DELETE_SELF`.
- The last active ADMIN cannot be deleted → `409 CONFLICT_LAST_ADMIN`.

**Error responses:**
- `401 AUTH_REQUIRED`
- `403 FORBIDDEN_INSUFFICIENT_ROLE`
- `404 NOT_FOUND_USER`
- `409 CONFLICT_CANNOT_DELETE_SELF` — `id` equals the acting admin's id
- `409 CONFLICT_LAST_ADMIN` — would remove the last active admin
- `500 INTERNAL_ERROR`

**Audit:** writes `ADMIN_USER_DELETED`.

---

## Audit

Every mutation stages an `AuditLog` row in the same transaction as the change. Action codes
(SCREAMING_SNAKE_CASE):

| Action | When |
|--------|------|
| `ADMIN_USER_CREATED` | Create |
| `ADMIN_USER_UPDATED` | Update role/status |
| `ADMIN_PASSWORD_RESET` | Password reset |
| `ADMIN_USER_DELETED` | Delete |
| `AUTH_LOGIN_SUCCESS` | Successful login (written by the auth service; also sets `lastLoginAt`) |

The `UserId`/`Username` on the audit row identify the **acting admin**; `Details` describe
the target user and the change. Passwords are never written to the audit log.
