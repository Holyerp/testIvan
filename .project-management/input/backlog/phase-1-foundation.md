# Phase 1 — Foundation & MVP

**Goal:** Authentication, BC middleware setup, and first business data screens.
**Total items:** 7 (2 technical tasks + 5 user stories)
**Total points:** 47
**Status:** Todo

---

## Technical Tasks

### T-001: Project Infrastructure Setup
**Priority:** P0
**Estimate:** 5 points
**Description:** Monorepo scaffolding, Next.js app, .NET 8 Web API skeleton, shared types, CI/CD pipeline.

**Tasks:**
- [ ] Initialize pnpm workspace + Turborepo config
- [ ] Scaffold `apps/web` (Next.js 14, App Router, TypeScript, Tailwind CSS)
- [ ] Scaffold `apps/api` (.NET 8 Web API, Clean Architecture folders)
- [ ] Configure ESLint + Prettier (frontend) and EditorConfig + Roslyn (backend)
- [ ] Setup GitHub Actions CI (lint + test on PR; build + deploy on main)
- [ ] Configure environment variables structure (.env.local, Azure App Settings)

---

### T-002: BC Middleware Foundation
**Priority:** P0
**Estimate:** 8 points
**Description:** .NET 8 middleware that authenticates to Azure AD and proxies OData v4 calls from Business Central to the frontend.

**Tasks:**
- [ ] Azure AD App Registration setup (service principal, client credentials)
- [ ] Implement `BcAuthService` — OAuth 2.0 client credentials token acquisition + refresh
- [ ] Create typed BC HTTP client with `HttpClientFactory`
- [ ] Implement base OData query builder (support `$filter`, `$select`, `$top`, `$skip`, `$count`)
- [ ] Add response caching layer (in-memory for dev, Redis-ready interface for prod)
- [ ] Write integration tests for BC auth + one sample entity fetch

---

## Epic 1: Authentication & Role Management

### US-001: Employee Login
**Priority:** P0
**Estimate:** 5 points
**Story:** As an employee, I want to log in with username and password so that I can access the portal.

**Type:** Frontend (Web)
**Screen:** LoginScreen

**Acceptance Criteria:**
- [ ] Login form with username + password fields
- [ ] Validates input before submission (non-empty, password ≥ 8 chars)
- [ ] POST `/api/v1/auth/login` — receives JWT access + refresh tokens on success
- [ ] Invalid credentials show user-friendly error message
- [ ] Rate limit: 5 failed attempts per 15 min → "Too many attempts" message
- [ ] Successful login redirects to role-appropriate default screen
- [ ] Session persists on page refresh (JWT stored securely)

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| POST | /api/v1/auth/login | Authenticate user, receive JWT |
| POST | /api/v1/auth/refresh | Refresh expired access token |

**Dependencies:** T-001, T-002

---

### US-002: Role-Based Access Control
**Priority:** P0
**Estimate:** 8 points
**Story:** As the system, I need to enforce module access based on user role so that employees only see data relevant to them.

**Type:** Frontend + Backend
**Screen:** (cross-cutting — affects all screens)

**Acceptance Criteria:**
- [ ] Roles defined: `ADMIN`, `MANAGER`, `ACCOUNTING`, `WAREHOUSE`
- [ ] Backend enforces role on every protected endpoint (middleware)
- [ ] Frontend reads role from JWT claims and hides/shows sidebar modules accordingly
- [ ] Accessing a locked module returns 403 + user-friendly "Access denied" page
- [ ] Admin can assign roles to users

**Dependencies:** US-001

---

### US-003: Dashboard — KPI Overview
**Priority:** P0
**Estimate:** 8 points
**Story:** As a manager, I want to see KPI cards on the dashboard so that I can get a quick overview of the business status.

**Type:** Frontend (Web)
**Screen:** DashboardScreen

**Acceptance Criteria:**
- [ ] 4 KPI cards: Total open invoices count, Total outstanding amount, Advance payments balance, Inventory value
- [ ] Invoice trend bar chart (last 6 months)
- [ ] Recent activity section: last 5 posted sales invoices
- [ ] BC connection status badge (Connected / Disconnected)
- [ ] Data loads within 2 seconds
- [ ] Shows loading skeleton while fetching

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/dashboard/summary | KPI card data |
| GET | /api/v1/dashboard/invoice-trend | Monthly invoice chart data |
| GET | /api/v1/dashboard/recent-activity | Last 5 invoices |
| GET | /api/v1/health/bc-status | BC connection status |

**Dependencies:** T-002, US-002

---

### US-004: Customers — List View
**Priority:** P0
**Estimate:** 5 points
**Story:** As an accounting employee, I want to browse the customer list so that I can find and select a customer.

**Type:** Frontend (Web)
**Screen:** CustomerListScreen

**Acceptance Criteria:**
- [ ] Paginated table: customer name, number, city, balance due, phone
- [ ] Search by customer name or number (debounced, 300ms)
- [ ] Sort by name, balance (ascending/descending)
- [ ] Loading state with skeleton rows
- [ ] Empty state when no results
- [ ] Clicking a row navigates to CustomerDetailScreen

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/customers | Paginated customer list with search/sort |

**Dependencies:** T-002, US-002

---

### US-005: Customers — Detail View
**Priority:** P0
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view a customer's full profile so that I can see their contact info, balance, and invoice history.

**Type:** Frontend (Web)
**Screen:** CustomerDetailScreen

**Acceptance Criteria:**
- [ ] Customer header: name, number, address, phone, email, VAT number
- [ ] Financial summary: balance due, credit limit, payment terms
- [ ] Invoice history table (last 20 posted sales invoices): number, date, amount, status
- [ ] Breadcrumb navigation back to customer list
- [ ] 404 page if customer ID not found

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/customers/{id} | Customer detail |
| GET | /api/v1/customers/{id}/invoices | Customer invoice history |

**Dependencies:** US-004

---

## Phase Summary

- **P0:** 7 items, 47 points
- **P1:** 0
- **P2:** 0

**Navigation:** [← Index](README.md) · [Phase 2 →](phase-2-core.md) · [Dashboard](../../output/progress/DASHBOARD.md)
