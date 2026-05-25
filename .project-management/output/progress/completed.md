# Completed Work — Pinoles

**Project:** Pinoles — Internal Business Portal
**Last Updated:** 2026-05-25

---

**Phase 1 — Foundation & MVP: COMPLETE** (47 / 47 pts, 100% — 5 user stories + 2 technical tasks)

**Phase 2 — Core Documents: IN PROGRESS** (40 / 52 pts, 77% — 6 user stories + 1 technical task complete)

---

## Completed Stories

### US-001: Employee Login
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 5
**Commit:** See git log

**Summary:**
- POST /api/v1/auth/login — BCrypt verify (cost 12), JWT access token (8h) + rotating refresh token (7d, httpOnly cookie), rate limit 5 attempts per IP per 15 min
- POST /api/v1/auth/refresh — rotates refresh token; old token revoked
- POST /api/v1/auth/logout — revokes refresh token in DB, clears cookie
- LoginRateLimiter (singleton, in-memory, per-IP, 5 attempts / 15 min window)
- IAuthService + AuthService (Application.Auth), ITokenService + TokenService (Infrastructure.Auth), JwtOptions
- JwtSecurityToken via System.IdentityModel.Tokens.Jwt — HMAC SHA-256, issuer + audience validation
- Dev seed: admin / manager / accounting / warehouse test users (BCrypt cost 12) on startup
- Login page: Next.js 14 App Router `(auth)/login`, React Hook Form + Zod validation, Pinoles design tokens (pine-navy background, white card)
- i18n: sr.json + en.json updated with auth error codes (AUTH_REFRESH_TOKEN_MISSING, AUTH_INVALID_REFRESH_TOKEN)
- Backend tests: AuthServiceTests (7 tests), LoginRateLimiterTests (5 tests)
- Frontend tests: LoginPage (7 tests) — all pass

### US-002: Role-Based Access Control
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 8
**Commit:** fbbd154

**Summary:**
- GET /api/v1/users/me — returns current user info (RequireAuthorization)
- JWT OnChallenge/OnForbidden handlers return JSON envelope (401/403)
- Authorization policies: RequireAdmin, RequireFinancial, RequireDashboard
- UserRoles constants (ADMIN/MANAGER/ACCOUNTING/WAREHOUSE)
- Zustand auth store (sessionStorage-backed, initFromSession)
- AuthProvider client component
- RoleGuard component (role array check)
- useRequireAuth hook (redirects to /login if unauthenticated)
- Protected layout wraps pages with AuthProvider + Sidebar
- Sidebar with role-filtered navigation
- 403 forbidden page
- i18n: nav.invoices + errors.forbidden* keys (sr + en)
- Tests: UserRoles (3), UsersEndpoints (3), RoleGuard frontend (5)

### US-003: Dashboard — KPI Overview
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 8
**Commit:** See git log

**Summary:**
- GET /api/v1/dashboard/kpis (RequireDashboard policy — all roles)
- IDashboardService/DashboardService: aggregates BC customers, open/overdue invoices, vendors, 6-month trend
- 5-minute cache via ICacheService; 502 INTEGRATION_BC_UNAVAILABLE on BC failure
- BcCustomer/BcSalesInvoice/BcVendor DTOs; mock invoices spread across 6 months
- Frontend: 4 KPI cards (RSD currency formatting), CSS bar-chart trend, BC status badge (mock/connected), loading skeleton, error state
- i18n: dashboard section (sr + en)
- Tests: 5 backend (DashboardService) + 3 frontend (KpiCard)

### US-004: Customers — List View
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 5
**Commit:** See git log

**Summary:**
- GET /api/v1/customers (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403)
- ICustomerService/CustomerService: server-side pagination/search($filter)/sort($orderby) via BC OData
- PagedResultDto<T> + CustomerListItemDto; BcCustomer extended with City
- Mock: expanded customer set for pagination demo
- Frontend: searchable (debounced) + sortable paginated table, RSD formatting, skeleton/empty/error states
- lib/format.ts helpers (formatRsd, totalPages)
- i18n: customers section (sr + en)
- Tests: 6 backend (CustomerService) + 5 frontend (format helpers)

### US-005: Customers — Detail View
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 5
**Commit:** See git log

**Summary:**
- GET /api/v1/customers/{id} (RequireFinancial; WAREHOUSE 403); 404 NOT_FOUND_CUSTOMER; 502 on BC failure
- CustomerService.GetCustomerByIdAsync: profile + last 10 invoices (filtered by customerName, ordered postingDate desc)
- CustomerDetailDto / CustomerProfileDto / CustomerInvoiceDto
- MockBcHttpClient.GetByIdAsync returns real customer for known id, null for unknown; customer mock data refactored to single GetMockCustomerData() source; salesInvoices mock now honors customerName eq filter
- Frontend: detail page (profile header, balance/balanceDue cards, recent invoices table, status badges), 404 + loading + error states, back link; list rows link to detail
- i18n: customerDetail section (sr + en)
- Tests: 3 backend (GetCustomerByIdAsync) + 3 frontend (invoiceStatusKey); MockBcHttpClientTests GetByIdAsync reconciled (known id non-null, unknown id/entityset null)

### US-006: Sales Invoices — List View
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Three BC-backed endpoints under `/api/v1/sales` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/invoices` (open), GET `/posted-invoices`, GET `/credit-memos`. Canonical envelope `{ success, data: PagedResultDto }`; 502 INTEGRATION_BC_UNAVAILABLE on BC failure.
- `ISalesService`/`SalesService`: one service drives all three collections via an `entitySet` param (DRY). Uses shared `BcListQuery` for page/sort/filter; OData filter built from search (number/customerName contains) + status eq + postingDate ge/le date range; single quotes escaped. UI sort keys mapped: `date`→postingDate, `dueDate`→dueDate, `amount`→totalAmountIncludingTax (allow-listed).
- `SalesInvoiceListItemDto` + `SalesInvoiceMapper` (IBcMapper): normalizes BC status casing to SCREAMING_SNAKE wire value (Open→OPEN, Paid→PAID, Partially Paid→PARTIAL, unknown→OPEN). `BcSalesInvoice` extended with `DueDate`.
- MockBcHttpClient: new entity sets `salesInvoices` (12, incl. overdue + partial + paid), `salesInvoicesPosted` (8), `salesCreditMemos` (5); dueDate = postingDate+30d. Generalized in-memory filter honors contains-search, status eq, date range, plus the existing `customerName eq` path. Dashboard open-invoice reads preserved.
- Frontend `app/(protected)/sales/invoices/page.tsx` (SalesInvoiceListScreen): Open/Posted/Credit Memos tabs (remount per tab), FilterPanel (search + status select + date range), EntityTable (Number/Customer/Date/Due Date/Amount/Status; sortable date/dueDate/amount), overdue amber row highlight, status badges, row→`/sales/invoices/{id}`, RBAC guard.
- `lib/format.ts`: pure `isOverdue(status, dueDate, today)` + `statusBadgeClass(status)`. `EntityTable` extended with optional `rowClassName` prop (backward-compatible; existing tests green).
- API doc: `docs/api/sales.md` (query params, success example, 401/403/502, OPEN|PARTIAL|PAID enum). i18n: `sales` section + `nav.salesInvoices` (sr + en). Sidebar nav entry added.
- Tests: backend +17 (SalesService ×12, SalesInvoiceMapper ×5 methods incl. theory); frontend +9 (isOverdue ×5, statusBadgeClass ×4). Combined: 87 backend + 50 frontend passing.

### US-007: Sales Invoice — Detail View
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two BC-backed detail endpoints under `/api/v1/sales` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/invoices/{id}`, GET `/posted-invoices/{id}`. Canonical envelope `{ success, data }`; 404 `NOT_FOUND_SALES_INVOICE` for unknown id; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure.
- First use of BC `$expand` for line items. `IBcHttpClient.GetByIdAsync` extended with an optional `BcQueryOptions` param (models real BC `$expand=salesInvoiceLines`); both `BcHttpClient` (appends query string) and `MockBcHttpClient` updated; existing `customers` call site reconciled. The mock ignores the literal `$expand` string but always populates lines.
- `SalesInvoiceDetailDto` (Header + Lines + Totals) + `BcSalesInvoiceLine`; `BcSalesInvoice` extended with `BillToAddress`, `PaymentTerms`, `SalesInvoiceLines` (list fields preserved — US-006 tests green).
- `SalesInvoiceDetailMapper` (IBcMapper): reuses `SalesInvoiceMapper.NormalizeStatus` (DRY); computes totals from lines — subtotal = Σ lineAmount, vatAmount = Σ(lineAmount × vatPercent/100), total = subtotal + vatAmount.
- `SalesService.GetInvoiceByIdAsync(entitySet, id)`: $expand lines, returns null when not found (→ 404). Detail mapper injected + registered in Program.cs.
- MockBcHttpClient: `GetByIdAsync` for salesInvoices/salesInvoicesPosted/salesCreditMemos returns populated header (billToAddress, paymentTerms) + 3 realistic line items (VAT 20%) for known ids, null for unknown; clones the shared list source so it is never mutated. Customer GetByIdAsync behavior preserved.
- Frontend `app/(protected)/sales/invoices/[id]/page.tsx` (SalesInvoiceDetailScreen): header card (number, customer, bill-to, dates, payment terms), status badge + overdue hint, line-items table (description/qty/unit price/VAT%/line total), right-aligned totals block; 404 "Invoice not found", loading skeleton, error banner, back link; RBAC guard.
- `lib/format.ts`: pure `computeInvoiceTotals(lines)` helper (client-side reconciliation/fallback). i18n: `salesDetail` section + `errors.NOT_FOUND_SALES_INVOICE` (sr + en); reuses `sales.status.*`.
- API doc: `docs/api/sales.md` detail endpoints (path param, success example with header/lines/totals, 401/403/404 NOT_FOUND_SALES_INVOICE/502).
- Tests: backend +17 (SalesService detail ×7, SalesInvoiceDetailMapper ×6, MockBcHttpClient ×4); frontend +4 (computeInvoiceTotals). Combined: 104 backend + 54 frontend passing; lint clean.

### US-008: Sales Credit Memos — List & Detail
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two new BC-backed endpoints under `/api/v1/sales` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/credit-memos/{id}` (detail, 404 `NOT_FOUND_SALES_CREDIT_MEMO`) and GET `/posted-credit-memos` (list). The `/credit-memos` list already existed (US-006). Canonical envelope; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure.
- Maximal reuse: credit memos share `ISalesService`/`SalesService`, the invoice mappers, `SalesInvoiceListItemDto`/`SalesInvoiceDetailDto`, and the `BcSalesInvoice` type — no duplicated list/detail logic. The service re-normalizes status for credit-memo entity sets (`salesCreditMemos`, `salesCreditMemosPosted`) so they surface only OPEN | POSTED.
- Credit-memo status enum = OPEN | POSTED (SCREAMING_SNAKE wire value). Added `SalesInvoiceMapper.NormalizeCreditMemoStatus` (Open→OPEN, Posted→POSTED, invoice-only Paid/Partial→OPEN); `NormalizeStatus` extended additively with Posted→POSTED so invoice OPEN/PARTIAL/PAID normalization is unchanged.
- Not-found code parameterized: `SalesEndpoints.Detail` now takes the code + message so invoices keep `NOT_FOUND_SALES_INVOICE` (US-007 behavior + tests unchanged) and credit memos return `NOT_FOUND_SALES_CREDIT_MEMO`.
- MockBcHttpClient: existing `salesCreditMemos` (5) switched to Open/Posted semantics; new `salesCreditMemosPosted` entity set (4 posted, status "Posted") in both collection + GetById paths; line items reused from the invoice mock. Existing Mock/Dashboard/US-006/US-007 tests preserved.
- Frontend: dedicated `app/(protected)/sales/credit-memos/page.tsx` (SalesCreditMemoListScreen — Open / Posted tabs over `/credit-memos` and `/posted-credit-memos`, FilterPanel + EntityTable, row→detail) and `[id]/page.tsx` detail (reuses US-007 detail layout by minimal copy — kept US-007 page untouched to protect its tests; no overdue concept for credit memos). Sidebar nav entry "Credit Memos" added (ADMIN/MANAGER/ACCOUNTING).
- `lib/format.ts`: pure `creditMemoStatusBadgeClass` (POSTED green, OPEN blue). i18n: `creditMemos` + `creditMemoDetail` sections + `nav.creditMemos` + `errors.NOT_FOUND_SALES_CREDIT_MEMO` (sr + en).
- API doc: `docs/api/sales.md` extended with credit-memo detail + posted-credit-memos endpoints, the OPEN|POSTED credit-memo status enum, and consumer links.
- Tests: backend +14 (SalesService credit-memo ×7, SalesInvoiceMapper NormalizeCreditMemoStatus theory ×7 cases); frontend +3 (creditMemoStatusBadgeClass). Combined: 118 backend + 57 frontend passing; lint clean.

### US-009: Purchase Invoices — List View
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Three BC-backed endpoints under `/api/v1/purchase` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/invoices` (open), GET `/posted-invoices`, GET `/credit-memos`. Canonical envelope `{ success, data: PagedResultDto }`; 502 INTEGRATION_BC_UNAVAILABLE on BC failure. Purchase analogue of US-006 with a vendor party instead of a customer.
- `IPurchaseService`/`PurchaseService`: one service drives all three collections via an `entitySet` param (DRY), mirroring `SalesService`. Uses shared `BcListQuery`; OData filter built from search (number/vendorName contains) + status eq + postingDate ge/le; single quotes escaped. UI sort keys mapped date→postingDate, dueDate→dueDate, amount→totalAmountIncludingTax (allow-listed). Credit-memo entity set re-normalized to OPEN | POSTED.
- `BcPurchaseInvoice` + `PurchaseInvoiceListItemDto` + `PurchaseInvoiceMapper` (IBcMapper): maps vendorName; normalizes BC status casing to SCREAMING_SNAKE wire value.
- **Rule of Three refactor:** extracted shared `Application/Mapping/InvoiceStatus.cs` static helper (`Normalize` + `NormalizeCreditMemo`) — now the single source of truth for status normalization used by BOTH sales and purchase mappers. `SalesInvoiceMapper.NormalizeStatus`/`NormalizeCreditMemoStatus` refactored to delegate to it (existing sales tests stay green).
- MockBcHttpClient: generalized the in-memory invoice filter into `CreateMockDocumentCollection`/`ApplyDocumentFilter` parameterized by the party field name (customerName vs vendorName) — sales path delegates to it (DRY); new entity sets `purchaseInvoices` (10, incl. overdue + partial + paid), `purchaseInvoicesPosted` (6), `purchaseCreditMemos` (4); 3 extra mock vendors added for variety. dueDate = postingDate+30d. All existing sales/customer/dashboard/mock tests preserved.
- Frontend `app/(protected)/purchase/invoices/page.tsx` (PurchaseInvoiceListScreen): Open/Posted/Credit Memos tabs (remount per tab), FilterPanel (search + status select + date range), EntityTable (Number/Vendor/Date/Due Date/Amount/Status; sortable date/dueDate/amount), overdue amber row highlight (reuses isOverdue), status badges (reuses statusBadgeClass), row→`/purchase/invoices/{id}`, RBAC guard. Reuses all shared infra — no new frontend helper introduced.
- API doc: `docs/api/purchase.md` (query params, success example, 401/403/404/500/502, OPEN|PARTIAL|PAID + credit-memo OPEN|POSTED enums). i18n: `purchase` section + `nav.purchaseInvoices` (sr + en). Sidebar nav entry "Purchase Invoices" added.
- Tests: backend +40 (PurchaseService ×14, PurchaseInvoiceMapper ×4 methods incl. theory, InvoiceStatus shared-helper ×17 cases). Frontend unchanged (reused already-tested helpers). Combined: 158 backend + 57 frontend passing; lint clean.

### US-010: Purchase Invoice — Detail View
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two BC-backed detail endpoints under `/api/v1/purchase` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/invoices/{id}`, GET `/posted-invoices/{id}`. Canonical envelope `{ success, data }`; 404 `NOT_FOUND_PURCHASE_INVOICE` for unknown id; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Purchase analogue of US-007, reusing the parameterized not-found-code Detail handler pattern from US-008.
- `PurchaseInvoiceDetailDto` (Header + Lines + Totals, vendor-oriented header with `ourReference`) + `BcPurchaseInvoiceLine`; `BcPurchaseInvoice` extended with `PaymentTerms`, `OurReference`, `PurchaseInvoiceLines` (US-009 list fields preserved — those tests stay green).
- `PurchaseInvoiceDetailMapper` (IBcMapper): reuses shared `InvoiceStatus.Normalize` (DRY) and the newly extracted `InvoiceTotals` helper for the money math.
- **DRY refactor:** extracted `Application/Common/InvoiceTotals.cs` (generic `Compute<TLine>(lines, amountSelector, vatSelector)`) — single source of truth for subtotal/VAT/total. Sales detail mapper refactored to use it (existing sales detail tests stay green). 2nd consumer of identical totals logic → clean extraction.
- `PurchaseService.GetInvoiceByIdAsync(entitySet, id)`: `$expand` lines via `BcQueryOptions`, returns null when not found (→ 404). Detail mapper injected + registered in Program.cs.
- MockBcHttpClient: `GetByIdAsync` for purchaseInvoices/purchaseInvoicesPosted returns populated header (paymentTerms, ourReference) + 3 realistic line items (VAT 20%) for known ids, null for unknown; clones the shared US-009 list source so it is never mutated. Existing customer/sales GetByIdAsync behavior preserved.
- Frontend `app/(protected)/purchase/invoices/[id]/page.tsx` (PurchaseInvoiceDetailScreen): header card (number, vendor, our reference, dates, payment terms), status badge + overdue hint, line-items table, right-aligned totals block; 404 "Invoice not found", loading skeleton, error banner, back link to `/purchase/invoices`; RBAC guard. Mirrors US-007; reuses formatRsd/isOverdue/statusBadgeClass — no new frontend helper introduced.
- API doc: `docs/api/purchase.md` v1.1 — two detail endpoints (path param, success example with header/lines/totals, 401/403/404 NOT_FOUND_PURCHASE_INVOICE/500/502). i18n: `purchaseDetail` section (sr + en); reuses `purchase.status.*`.
- Tests: backend +18 (PurchaseService detail ×8, PurchaseInvoiceDetailMapper ×6, InvoiceTotals ×4); frontend unchanged (reused already-tested helpers). Combined: 176 backend + 57 frontend passing; lint clean.

### US-011: Vendors — List View
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- BC-backed `GET /api/v1/vendors` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): paginated/searchable/sortable vendor list. Canonical envelope `{ success, data: PagedResultDto }`; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Vendor analogue of US-004 (customers list).
- `VendorListItemDto` (id, number, displayName, city, balance, phone); `BcVendor` extended with `City` + `Phone` (additive — dashboard vendor count untouched). `VendorMapper` (IBcMapper) mirrors CustomerMapper.
- `IVendorService` + `VendorService`: reuses shared `BcListQuery` (page/pageSize clamp, sort allow-list `displayName`/`balance`, single-quote-escaped contains-filter on displayName/number). Registered in Program.cs (scoped service + singleton mapper + `MapVendorsEndpoints`).
- MockBcHttpClient: `vendors` set expanded from 5 → 10 vendors with city + phone; extracted `GetMockVendorData()` single source; added in-memory contains-filter (reuses `ApplyCustomerFilter`) + `$orderby` sort (displayName/balance, asc/desc). Honors Top/Skip/Count. DashboardServiceTests vendor count + MockBcHttpClientTests stay green.
- Frontend `app/(protected)/vendors/page.tsx` (VendorListScreen): EntityTable columns Name(sortable), Number, City, Balance(formatRsd, right-aligned, sortable), Phone; debounced search (name or number) resets page; row click → `/vendors/{id}` (detail in US-012); loading skeleton / empty / error via EntityTable; RBAC guard `['ADMIN','MANAGER','ACCOUNTING']`. Reuses EntityTable/FilterPanel/usePaginatedQuery/formatRsd — no new helper.
- Sidebar: added Vendors → `/vendors` (ADMIN/MANAGER/ACCOUNTING). i18n: `vendors` section (sr + en); reuses pagination labels; `nav.vendors` already present.
- API doc: `docs/api/vendors.md` v1.0 — endpoint, RequireFinancial auth, query params (page/pageSize/search/sortBy/sortDir), success example PagedResultDto, errors 400/401/403/404/500/502.
- Tests: backend +13 (VendorService ×10, VendorMapper ×3); frontend unchanged (reused already-tested helpers). Combined: 189 backend + 57 frontend passing; lint clean.

### US-012: Vendors — Detail View
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two BC-backed detail endpoints under `/api/v1/vendors` (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/{id}` (full detail = profile + purchase history) and GET `/{id}/invoices` (purchase history only). Canonical envelope `{ success, data }`; 404 `NOT_FOUND_VENDOR` for unknown id; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Vendor analogue of US-005 (customer detail), reusing the same not-found + envelope patterns.
- `VendorDetailDto` (VendorProfileDto + reuses `PurchaseInvoiceListItemDto` for history rows — DRY, no new invoice DTO). `BcVendor` extended additively with `Address`, `Email`, `VatNumber`, `PaymentTerms` (US-011 list + dashboard vendor count untouched — those tests stay green).
- `VendorService.GetVendorByIdAsync(id)`: `GetByIdAsync<BcVendor>("vendors", id)` → null when not found (→ 404); fetches last 20 posted purchase invoices filtered by `vendorName eq '<displayName>'` (single quotes escaped, OrderBy postingDate desc, Top 20) and maps each via the injected shared `PurchaseInvoiceMapper` (DRY — no duplicated row logic). `GetVendorInvoicesForEndpointAsync(id)` backs the `/{id}/invoices` route (null vendor → 404). Both added to `IVendorService`; mapper injected + already-registered in Program.cs.
- MockBcHttpClient: `GetMockVendorData()` extended with address/email/vatNumber/paymentTerms via a `Vendor(...)` factory; added `vendors` branch to `GetByIdAsync` (known id → populated record, unknown → null). Posted purchase invoice mock vendorNames already match vendor displayNames (e.g. "Supplier A d.o.o." → ppi001/ppi006), so v001 history is non-empty. All existing vendor-list/purchase/dashboard/customer-detail/mock tests preserved.
- Frontend `app/(protected)/vendors/[id]/page.tsx` (VendorDetailScreen): profile header (name/number/city) + profile card (address, phone, email, VAT number) + financial summary cards (balance formatRsd, payment terms) + purchase-history table (Number/Date/Amount/Status badge, last 20 posted invoices); 404 "Vendor not found", loading skeleton, error banner, back link to `/vendors`; RBAC guard `['ADMIN','MANAGER','ACCOUNTING']`. Reuses formatRsd + statusBadgeClass + `purchase.status.*` labels — no new frontend helper introduced.
- API doc: `docs/api/vendors.md` v1.1 — two detail endpoints (path param, success example profile+invoices, errors 401/403/404 NOT_FOUND_VENDOR/500/502) + consumer link. i18n: `vendorDetail` section (sr + en); reuses `purchase.status.*` for status badges.
- Tests: backend +9 (VendorService detail: known/unknown id, profile field mapping incl. Address/Email/VatNumber, invoices list present + non-empty for known vendor, history capped at 20 + status normalized, `/{id}/invoices` known/unknown). CreateService helper updated for new ctor param. Frontend unchanged (reused already-tested helpers). Combined: 197 backend + 57 frontend passing; lint clean.

---

## Completed Technical Tasks

### T-001: Project Infrastructure Setup
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 5
**Commit:** See git log

**Summary:**
- pnpm workspace + Turborepo monorepo root config
- `apps/web`: Next.js 14 App Router skeleton (TypeScript, Tailwind CSS with Pinoles palette, ESLint, Prettier, Vitest, next-intl)
- `apps/api`: .NET 8 Web API skeleton (Clean Architecture folders, Swagger, Serilog, EF Core packages, xUnit test project)
- `packages/types`: Shared TypeScript types (UserRole, User, AuthTokens, PaginatedResponse, API envelope types)
- GitHub Actions CI workflow (web + api jobs)
- i18n: `sr.json` + `en.json` base message files
- Base API client with typed error handling (ApiError class)
- 3 unit tests on API client (success, error, fallback code)

### T-002: BC Middleware Foundation
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 8
**Commit:** 054bce2

**Summary:**
- IBcHttpClient interface + real BcHttpClient (Azure AD OAuth2 client credentials)
- MockBcHttpClient for dev (BC.UseMock=true) with realistic sample data (customers, invoices, vendors)
- BcAuthService: Azure.Identity ClientSecretCredential with in-process token caching
- BcQueryOptions: OData query builder ($filter, $select, $top, $skip, $count, $orderby, $expand)
- ICacheService interface + MemoryCacheService (IMemoryCache, Redis-ready)
- EF Core entities: User, RefreshToken, AuditLog with PinolesDbContext
- InitialCreate migration (manually crafted — dotnet ef not installed)
- 10 unit tests (MockBcHttpClient ×7, BcQueryOptions ×3, MemoryCacheService ×4 — see Tests/Infrastructure/)

### T-003: Document List Infrastructure
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Shared frontend list/detail foundation so US-006–US-013 do not reinvent tables, pagination, filtering, and BC mapping.
- `components/entity-table.tsx` — generic `EntityTable<T>`: controlled sort + pagination, skeleton/empty/error states, optional row click, right-aligned numeric columns, Pinoles palette. Purely presentational (no fetching).
- `components/filter-panel.tsx` — generic `FilterPanel` supporting search (internally debounced ~400ms), select, dateRange, amountRange fields via a small config.
- `lib/hooks/use-paginated-query.ts` — `usePaginatedQuery<T>` encapsulating the list-fetch pattern (page/sort/filter state, Bearer token from auth store, canonical envelope, totalPages from lib/format). Built on useState/useEffect to match Phase 1 (no new dependency added).
- `lib/query.ts` — pure `buildListQueryString()` helper (extracted for unit testing the query-string building).
- Customers list page refactored onto EntityTable + FilterPanel + usePaginatedQuery (DRY proof) — all existing behavior preserved (debounced search, sort by number/name, pagination, row→detail, RBAC guard).
- Backend BC mapper pattern: `Application/Mapping/IBcMapper<TSource,TTarget>` + reference `CustomerMapper` (BcCustomer → CustomerListItemDto), injected into CustomerService and registered in Program.cs.
- `Application/Common/BcListQuery` — shared static helper building BcQueryOptions (page/pageSize clamping, sort allow-list, OData filter via injected builder, single-quote escaping); CustomerService now uses it.
- Tests: backend +11 (CustomerMapper ×3, BcListQuery ×8); frontend +14 (EntityTable ×9, buildListQueryString ×5). Existing CustomerServiceTests updated for new constructor and still green.
- Combined suites: 66 backend + 41 frontend passing; lint clean.
