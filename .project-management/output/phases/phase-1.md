# Phase 1 — Foundation & MVP

**Duration:** TBD (estimated 4–6 weeks)
**Status:** Planning
**Target Completion:** TBD

---

## Phase Goal

Deliver a working authentication system, BC middleware connectivity, and the first business data screens: Dashboard, Customers, and Sales Invoices.

**Success Criteria:**
- Employees can log in with username + password and are redirected by role
- BC middleware successfully authenticates to Azure AD and fetches live data
- Dashboard displays KPI cards with real BC data
- Customers list + detail fully functional
- Sales Posted Invoices list + detail fully functional
- All 7 stories/tasks complete with ≥ 80% test coverage

---

## Stories & Tasks

### T-001: Project Infrastructure Setup (5 pts) — P0
**Status:** Completed
- Monorepo scaffolding, CI/CD, dev environment

### T-002: BC Middleware Foundation (8 pts) — P0
**Status:** Completed
- Azure AD App Registration, OData client, caching layer

### US-001: Employee Login (5 pts) — P0
**Status:** Completed
- Login screen, JWT issuance, role-based redirect

### US-002: Role-Based Access Control (8 pts) — P0
**Status:** Completed
- ADMIN / MANAGER / ACCOUNTING / WAREHOUSE role enforcement

### US-003: Dashboard — KPI Overview (8 pts) — P0
**Status:** Completed
- KPI cards, invoice trend chart, BC connection status

### US-004: Customers — List View (5 pts) — P0
**Status:** Completed
- Paginated customer table, search, sort

### US-005: Customers — Detail View (5 pts) — P0
**Status:** Todo
- Customer profile + invoice history

---

## Phase Metrics

### Planning Estimates
- **Total Story Points:** 47
- **Total Stories:** 5 user stories + 2 technical tasks
- **Estimated Duration:** 4–6 weeks
- **Risk Level:** High (BC API connectivity unknown until T-002)

### Progress Tracking
- **Completed Story Points:** 39 / 47 (83%)
- **Completed Stories:** 6 / 7 (T-001, T-002, US-001, US-002, US-003, US-004)
- **Tests Passing:** 6 backend (CustomerService) + 5 frontend (format helpers) added this story; suite green
- **Code Coverage:** ≥ 80% on BC middleware, auth, and dashboard layers

---

## Dependencies

### External
- Azure AD App Registration: must be available before T-002 (open question — who sets it up?)
- BC OData endpoint access: must be verified (standard vs. custom)

### Internal
- T-001 must complete before any other work
- T-002 must complete before US-003, US-004, US-005 (all require BC data)
- US-001 + US-002 must complete before all protected screens

---

## Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Azure AD App Registration unavailable | High | Medium | Clarify ownership pre-kickoff |
| BC uses custom endpoints | High | Medium | Verify in T-002; abstract BC client |
| BC rate limits in dev | Medium | Low | Mock BC responses for unit tests |

---

## Notes

- Phase 1 Sales Invoices scope: **posted** sales invoices only (simpler, no state transitions)
- Dashboard KPIs initially loaded from BC direct calls; Redis caching added Phase 2+
- i18n: all Phase 1 screens must include `sr.json` + `en.json` translations (per I18N-RULES.md)

---

**Created:** 2026-05-25
**Phase Status:** Planning
