# Project Scope

**Project Name:** Pinoles — Internal Business Portal
**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

---

## Project Vision

Pinoles is an internal business web application serving as an employee portal. The application connects to **Microsoft Dynamics 365 Business Central** (SaaS cloud) via a .NET 8 / C# middleware layer and displays key business data in a readable, modern interface.

Employees — without direct access to Business Central — can view invoices, customers, vendors, warehouse status, advance invoices, and other financial documents in a single central place, with clearly defined roles and access control.

---

## Target Audience

- **Primary:** Internal employees (accounting, finance, warehouse/logistics)
- **Secondary:** Managers and executives (KPI overview, analytics)
- **Tertiary:** System administrators (user management, BC configuration)

---

## Core Objectives

1. **Centralized data access:** Provide employees with a single portal to access all Business Central data without requiring direct BC licenses.

2. **Role-based access control:** Ensure each employee sees only the modules relevant to their role (accounting, warehouse, management, admin).

3. **BC integration:** Seamlessly read data from Microsoft Dynamics 365 Business Central via OData v4 REST API through a .NET 8 middleware layer.

4. **Modern UI/UX:** Deliver a fast, responsive, and intuitive interface that improves upon the native BC experience, using the established Pinoles design system.

5. **Analytics & reporting:** Provide management with KPI dashboards, revenue/expense charts, and period-comparison analytics.

---

## Success Criteria

- All four user roles (Admin, Manager, Accounting, Warehouse) can access their respective modules
- BC data displays within 2 seconds of page load
- Zero unauthorized cross-role data access
- 95%+ uptime during business hours
- All BC entity types (invoices, customers, vendors, items) successfully integrated and displayed

---

## Out of Scope (v1)

- Creating or editing documents in BC (read-only portal only)
- Mobile native application (responsive desktop is sufficient)
- Multi-tenant support (one BC tenant per deployment)
- Real-time WebSocket sync (polling/on-demand fetch is sufficient)
- Integration with other ERP systems
- Offline mode

---

## Project Phases

**Phase 1 — Foundation & MVP:**
- Authentication + JWT + role-based redirect
- Dashboard with KPI cards (mock → real data)
- BC middleware setup + Azure AD / OAuth 2.0 service principal auth
- Customers (list + detail view)
- Sales Posted Invoices (list + detail view)

**Phase 2 — Core Documents:**
- All Sales documents: invoices, credit memos, posted variants
- All Purchase documents: invoices, credit memos, posted variants
- Vendors (list + detail view)
- Universal search and filtering on all entity lists

**Phase 3 — Extended Modules:**
- Advance invoices (sales + purchase)
- Credit notes / storno invoices / debit memos
- Warehouse and inventory (items list, stock levels, low-stock alerts)
- Full role-based module access enforcement (locked modules per role)

**Phase 4 — Analytics & Polish:**
- Analytics charts — revenue/expense by period (Recharts or Chart.js)
- Export to PDF / Excel
- Notifications (overdue deadlines, low stock alerts)
- User management admin panel
- Audit log (who viewed what)

---

## Stakeholders

| Role | Responsibility |
|------|----------------|
| Client / Project Owner | Final decisions, BC tenant access, Azure AD |
| Tech Lead | Architecture, .NET middleware, Azure AD App Registration |
| Frontend Developer | Next.js UI implementation |
| PM | Requirements, prioritization |

---

## Dependencies

- **Microsoft Dynamics 365 Business Central (SaaS)** — OData v4 REST API
- **Azure Active Directory** — App Registration for service-principal auth (OAuth 2.0 client credentials)
- **.NET 8 / C#** — Middleware Web API layer
- **Next.js 14+ (App Router)** — Frontend framework
- **PostgreSQL** — User roles, caching, audit log storage

---

## Assumptions

- Users access the application from modern browsers (Chrome, Firefox, Safari, Edge — last 2 versions)
- BC tenant uses standard (not custom extension) OData v4 API endpoints
- Azure AD App Registration is available for service-principal authentication setup
- Internet connectivity is always available (no offline mode required)
- Single BC tenant and single company per deployment

---

## Risks

1. **Risk:** BC tenant uses custom/localized API endpoints (BiH/SRB localization)
   - **Mitigation:** Verify standard vs. custom endpoints early in Phase 1; create abstraction layer in middleware

2. **Risk:** Azure AD App Registration setup delays (responsibility unclear)
   - **Mitigation:** Clarify whether client or dev team handles registration before Phase 1 starts (open question)

3. **Risk:** Advance invoice format varies by BC locale
   - **Mitigation:** Investigate advance invoice format during Phase 1 research; design flexible mapping layer

4. **Risk:** BC API rate limits cause performance degradation
   - **Mitigation:** Implement server-side response caching in middleware (5–15 min TTL for list endpoints)

---

**Last Updated:** 2026-05-25
