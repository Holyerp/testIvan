# Project Constraints — Pinoles

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

---

## Technical Constraints

### Must Use
- **Microsoft Dynamics 365 Business Central** — client's existing ERP (no replacement)
- **Azure** hosting — natural fit for BC ecosystem + Azure AD
- **Next.js 14+ (App Router)** — selected tech stack
- **.NET 8 / C#** — middleware layer (BC SDK ecosystem, OData client libraries)
- **JWT authentication** — stateless, compatible with Next.js SSR and API routes
- **OData v4** — BC REST API protocol (cannot be changed)

### Cannot Use
- Mobile-native frameworks in v1 (web-only)
- Real-time WebSockets (polling is acceptable)
- Direct database calls to BC (only via OData REST API)
- Monolithic write operations to BC (read-only portal)

### BC API Constraints
- Rate limits apply on BC OData endpoints (exact limits per tenant)
- OAuth 2.0 Client Credentials flow required (service principal, not user-delegated)
- Azure AD App Registration must be set up (responsibility TBD — open question)
- BC API supports pagination via `$top` / `$skip` / `$count` OData parameters

---

## Scope Constraints

### Features That Must Be Included (MVP)
1. **Authentication with roles** — required for any data access
2. **BC API connectivity** — core value proposition of the application
3. **Customers & Sales Invoices** — minimum viable data for accounting users
4. **Dashboard KPIs** — minimum viable for management users

### Features Not in v1
1. **Document creation/editing in BC** — read-only portal only
2. **Mobile native app** — responsive desktop is sufficient
3. **Multi-tenant** — single BC tenant per deployment
4. **Real-time sync** — on-demand fetch is acceptable
5. **Offline mode** — always-online assumption

---

## Compliance & Legal

- **Data Privacy:** Internal business data — standard company data protection policies apply
- **Access logging:** Audit log is a Phase 4 feature (who viewed which document)
- **HTTPS:** Enforced in production (Azure provides SSL termination)

---

## Open Questions (blocking or near-blocking)

1. Does the BC tenant use standard or custom/extended API endpoints?
2. What is the format of advance invoices in this BC tenant — standard or BiH/SRB localization?
3. Is offline mode required, or is always-online acceptable?
4. Who is responsible for Azure AD App Registration — the client or the dev team?
5. Is an audit log required (who viewed what)? [Planned for Phase 4]

---

## Quality Constraints

- **Code Coverage:** 80%+ (unit + integration tests)
- **TypeScript strict mode:** Required on frontend
- **Linting:** ESLint (frontend) + Roslyn analyzers (.NET) — must pass before merge
- **Code Review:** Required for all PRs targeting main
- **API Documentation:** All .NET API endpoints documented via Swagger/OpenAPI

---

## Assumptions

1. Standard OData v4 BC endpoints are available (no custom extensions required for MVP)
2. Azure AD tenant is accessible and App Registration can be created
3. Single BC company selected at deployment time (configured via env vars)
4. Users access application from corporate network or VPN (no public internet exposure required for v1)
5. Design follows the Pinoles prototype visual system without major redesigns

---

## Summary Table

| Constraint Type | Key Limitation | Impact |
|----------------|----------------|--------|
| Technical | Read-only BC integration via OData v4 | Medium — shapes all data layers |
| Technical | Azure hosting (BC ecosystem lock-in) | Low — straightforward |
| Technical | .NET 8 middleware required | Medium — two-language monorepo |
| Scope | No document creation/editing in v1 | Low — simplifies scope |
| Scope | Desktop-only (no mobile native) | Low — known constraint |
| Open Question | BC tenant endpoint type (standard/custom) | High — affects Phase 1 integration |
| Open Question | Azure AD App Registration ownership | High — blocks Phase 1 start |

---

**Last Updated:** 2026-05-25
