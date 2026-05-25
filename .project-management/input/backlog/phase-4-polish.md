# Phase 4 — Analytics & Polish

**Goal:** Management analytics, export functionality, notifications, user management admin panel, and audit log.
**Total items:** 5 (1 technical task + 4 user stories)
**Total points:** 27
**Status:** Todo

---

## Technical Tasks

### T-005: Export & Notification Infrastructure
**Priority:** P2
**Estimate:** 5 points
**Description:** PDF/Excel export utilities and notification system (in-app + email for overdue/low-stock).

**Tasks:**
- [ ] Integrate `jsPDF` + `xlsx` (or server-side PDF generation via .NET)
- [ ] Create `ExportService` (PDF template for invoices, Excel for lists)
- [ ] Build in-app notification system (bell icon, notification list)
- [ ] Notification triggers: overdue invoice (daily check), low-stock alert (on item load)
- [ ] Email notification via MailKit (.NET) for configured alert thresholds

---

## Epic 8: Analytics

### US-020: Analytics Dashboard
**Priority:** P2
**Estimate:** 8 points
**Story:** As a manager, I want to view revenue and expense charts with period comparison so that I can analyze business performance.

**Type:** Frontend (Web)
**Screen:** AnalyticsDashboardScreen

**Acceptance Criteria:**
- [ ] Revenue vs. expenses bar/line chart — monthly, quarterly, yearly view toggle
- [ ] Top 10 customers by revenue table
- [ ] Top 10 items by sales volume table
- [ ] Period comparison: current vs. prior period (delta %, delta amount)
- [ ] Date range picker for custom period selection
- [ ] Export to PDF/Excel button (Phase 4 feature)

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/analytics/revenue-expense | Revenue/expense time series |
| GET | /api/v1/analytics/top-customers | Top customers by revenue |
| GET | /api/v1/analytics/top-items | Top items by sales |

**Dependencies:** T-002, US-002

---

## Epic 9: Administration

### US-021: User Management — Admin Panel
**Priority:** P2
**Estimate:** 8 points
**Story:** As an admin, I want to manage employee accounts and role assignments so that I can control system access.

**Type:** Frontend (Web)
**Screen:** UserManagementScreen

**Acceptance Criteria:**
- [ ] List of all users: name, email, role, last login, status (Active/Inactive)
- [ ] Create new user (name, email, role, temp password)
- [ ] Edit user: change role, activate/deactivate
- [ ] Reset password (sends email to user)
- [ ] Cannot delete own account or the last admin
- [ ] All actions logged in audit log

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/admin/users | User list |
| POST | /api/v1/admin/users | Create user |
| PUT | /api/v1/admin/users/{id} | Update user (role, status) |
| POST | /api/v1/admin/users/{id}/reset-password | Trigger password reset |

**Dependencies:** US-002

---

### US-022: Settings — BC Connection Configuration
**Priority:** P2
**Estimate:** 3 points
**Story:** As an admin, I want to view and test the Business Central connection configuration so that I can verify the integration is working.

**Type:** Frontend (Web)
**Screen:** SettingsScreen

**Acceptance Criteria:**
- [ ] Display current BC tenant config: tenant ID, company ID, environment name
- [ ] "Test Connection" button — shows success/failure + last successful sync time
- [ ] BC API credentials are NOT displayed (read-only connection status)
- [ ] Shows last sync timestamps per entity type

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/settings/bc-config | BC connection info (non-sensitive) |
| POST | /api/v1/settings/bc-config/test | Trigger connection test |

**Dependencies:** T-002

---

### US-023: Audit Log — View
**Priority:** P2
**Estimate:** 3 points
**Story:** As an admin, I want to view the audit log so that I can see who viewed which documents and when.

**Type:** Frontend (Web)
**Screen:** AuditLogScreen

**Acceptance Criteria:**
- [ ] Paginated audit log table: timestamp, user, action type, entity type, entity ID
- [ ] Filter: date range, user, action type (LOGIN / VIEW / EXPORT / ADMIN)
- [ ] Export to Excel
- [ ] Admin-only access (403 for other roles)

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/admin/audit-log | Paginated audit log |

**Dependencies:** US-021

---

## Phase Summary

- **P2:** 5 items, 27 points

**Navigation:** [← Phase 3](phase-3-advanced.md) · [Future →](future.md) · [Dashboard](../../output/progress/DASHBOARD.md)
