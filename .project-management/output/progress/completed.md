# Completed Work — Pinoles

**Project:** Pinoles — Internal Business Portal
**Last Updated:** 2026-05-25

---

**Phase 1 — Foundation & MVP: COMPLETE** (47 / 47 pts, 100% — 5 user stories + 2 technical tasks)

**Phase 2 — Core Documents: COMPLETE** (52 / 52 pts, 100% — 8 user stories + 1 technical task complete)

**Phase 3 — Extended Modules: COMPLETE** (42 / 42 pts, 100% — 6 user stories + 1 technical task complete)

**Phase 4 — Analytics & Polish: COMPLETE** (27 / 27 pts, 100% — 4 user stories + 1 technical task complete)

**PROJECT COMPLETE** — all 4 phases done, 168 / 168 story points (100%): 23 user stories + 5 technical tasks.

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

### US-013: Universal Search
**Completed:** 2026-05-25
**Phase:** 2 — Core Documents
**Story Points:** 8
**Commit:** See git log

**Summary:**
- BC-backed `GET /api/v1/search?q={query}&limit={n}` (RequireDashboard — ALL roles incl. WAREHOUSE): cross-entity search returning grouped hits (customers, vendors, sales invoices, purchase invoices), max `limit` per group (default 5, clamped 1..20). Canonical envelope `{ success, data: SearchResultsDto }`; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Short/empty query (< 2 chars) returns 200 with empty groups (simplifies the debounced UI).
- `ISearchService`/`SearchService`: aggregates the four existing list services (`ICustomerService`, `IVendorService`, `ISalesService`, `IPurchaseService`) — no ad-hoc BC queries (DRY; OData escaping/paging stay in the list services). Sequential awaits (mock-safe). Maps each list item → uniform `SearchHitDto` (Title/Subtitle/Type per type) via small private mappers.
- **RBAC:** the four entity types are financial data; WAREHOUSE has no access. `SearchAsync` reads the caller's roles (passed from the endpoint via `ClaimsPrincipal`, same "role"/ClaimTypes.Role read as GetMe) and returns all-empty groups when the caller lacks a financial role (ADMIN/MANAGER/ACCOUNTING) — BC is never queried for a WAREHOUSE-only caller. Search itself stays reachable for every role.
- `SearchResultsDto` + `SearchHitDto`; `Type` is a cross-layer SCREAMING_SNAKE_CASE wire enum: CUSTOMER | VENDOR | SALES_INVOICE | PURCHASE_INVOICE → frontend maps to i18n group labels + detail routes. Documented allowed values.
- Frontend global topbar search added to the protected layout (`app/(protected)/layout.tsx` gains a header bar) via `components/global-search.tsx`: Cmd/Ctrl+K focuses the input; debounced 300ms, min 2 chars; Bearer-token fetch; grouped dropdown (group shown only if it has hits, header label + up to 5 rows of Title/Subtitle + "View all" link to the list screen with `?search=` prefilled); keyboard nav (Arrow up/down across the flattened ordered hit list with wrap-around, Enter opens highlighted hit, Esc closes/blurs); click-outside + click-to-open; loading + "No results" states; accessible (role=combobox/listbox/option, aria-selected). Pine palette.
- **Pure FE helpers** extracted into `lib/search.ts` for testability: `flattenHits(results)` (ordered flatten customers→vendors→sales→purchase), `hitHref(hit)` (type→detail route, id URL-encoded), `nextIndex(current,total,delta)` (wrap-around). `SearchResults`/`SearchHit` types mirror the backend wire shape.
- List-page search seeding: customers + vendors list pages read `?search=` from the URL (`useSearchParams`) and seed the filter (`extraParams` + FilterPanel initial value) so the "View all" link lands on a prefiltered list. Sales/purchase "View all" links target their list screens.
- API doc: `docs/api/search.md` v1.0 (method, path, RequireDashboard auth, q+limit params, grouped success example, WAREHOUSE-empty-groups RBAC note, 401/403/404/500/502, Type enum + per-type route table). i18n: `search` section (placeholder, loading, noResults, viewAll, group labels keyed by Type) in sr + en.
- Tests: backend +11 (SearchServiceTests: grouped results for a financial role, all-four-groups for a broad term, per-group limit + clamp, WAREHOUSE-only all-empty, short/empty query empty, hit mapping for each type). Frontend +14 (search.test.ts: flattenHits order/count/empty, hitHref all 4 types + URL-encoding, nextIndex wrap-around incl. -1→0 and total=0). Combined: 208 backend + 71 frontend passing; lint clean.

### US-014: Sales Advance Invoices — List & Detail
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two BC-backed endpoints under the existing `/api/v1/sales` group (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/advance-invoices` (list) and GET `/advance-invoices/{id}` (detail). Canonical envelope `{ success, data }`; 404 `NOT_FOUND_SALES_ADVANCE_INVOICE` for unknown id; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Advance invoices track advance-payment requests sent to customers.
- **Q-003 standard-schema assumption:** the advance-invoice format is unconfirmed by the client (standard BC schema vs. BiH/SRB localized). Implemented against the **STANDARD BC advance-invoice schema** (header + line items + payment status), isolated behind `IBcMapper` (`SalesAdvanceInvoiceDetailMapper`) + the `salesAdvanceInvoices` mock collection so a localized format is a low-cost mapper + mock swap. Documented in `docs/api/sales-advance.md` and the commit message.
- **Maximal reuse:** the list reuses `SalesService.GetInvoicesAsync("salesAdvanceInvoices", …)` + `SalesInvoiceListItemDto` (same columns, no new list DTO/mapper). The detail adds `GetAdvanceInvoiceByIdAsync` + a new `SalesAdvanceInvoiceDetailDto` (reuses the existing Header/Line/Totals DTOs + a `PaymentTrackingDto { amount, amountPaid, remaining }`). `SalesAdvanceInvoiceDetailMapper` reuses `SalesInvoiceMapper.NormalizeStatus`, `InvoiceTotals`, `BcListQuery` — no duplicated list/detail logic.
- **Payment tracking:** `amount` = document total (line-derived); `amountPaid` reconciled from the normalized payment status (PAID → full, PARTIAL → half, OPEN → 0); `remaining` recomputed so the `amount = amountPaid + remaining` invariant holds exactly regardless of the amount base BC reports against.
- MockBcHttpClient: new `salesAdvanceInvoices` collection (8 advance invoices, numbers `SA-2026-00N`, status mix Open/Partially Paid/Paid) in both collection + GetById paths (reuses the generalized document-collection helper + `FindInvoiceWithLines`). Existing mock/dashboard/sales/purchase tests preserved.
- Frontend `app/(protected)/sales/advance-invoices/page.tsx` (list — Number/Customer/Date/Amount/Status, FilterPanel search+status+date-range, sortable date/amount, row → detail, purple "Advance" tag distinguishing it from regular invoices) + `[id]/page.tsx` (detail — header card + payment-tracking section [Amount/Paid/Remaining] + line-items table + totals; 404 → not-found; "Advance" tag). RBAC guard `['ADMIN','MANAGER','ACCOUNTING']`. Reuses EntityTable/FilterPanel/usePaginatedQuery/formatRsd/statusBadgeClass — no new frontend helper introduced.
- API doc: `docs/api/sales-advance.md` v1.0 (both endpoints, RequireFinancial, query/path params, success examples incl. payment tracking, 400/401/403/404 NOT_FOUND_SALES_ADVANCE_INVOICE/500/502, OPEN|PARTIAL|PAID enum, Q-003 schema note); cross-link added to `sales.md`. i18n: `salesAdvance` + `salesAdvanceDetail` sections + `nav.salesAdvanceInvoices` + `errors.NOT_FOUND_SALES_ADVANCE_INVOICE` (sr + en). Sidebar nav entry "Advance Invoices" (financial module).
- Tests: backend +15 (SalesAdvanceInvoiceTests: list returns items / pageSize / total / status normalization / status+search filter; detail known/unknown id, totals, payment-tracking invariant + PAID/OPEN/PARTIAL states + amount = document total + status normalization). Existing `SalesServiceTests`/`SearchServiceTests` ctor updated for the new mapper. Frontend unchanged (reused already-tested helpers; pages are out of unit-coverage scope). Combined: 243 backend + 118 frontend passing; lint clean; coverage gate green.

### US-015: Purchase Advance Invoices — List & Detail
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two BC-backed endpoints under the existing `/api/v1/purchase` group (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/advance-invoices` (list) and GET `/advance-invoices/{id}` (detail). Canonical envelope `{ success, data }`; 404 `NOT_FOUND_PURCHASE_ADVANCE_INVOICE` for unknown id; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Purchase (vendor-side) analogue of US-014; advance invoices track advance payments made to vendors.
- **Q-003 standard-schema assumption:** the advance-invoice format is unconfirmed by the client (standard BC schema vs. BiH/SRB localized). Implemented against the **STANDARD BC advance-invoice schema** (header + line items + payment status), isolated behind `IBcMapper` (`PurchaseAdvanceInvoiceDetailMapper`) + the `purchaseAdvanceInvoices` mock collection so a localized format is a low-cost mapper + mock swap. Documented in `docs/api/purchase-advance.md` and the commit message.
- **Maximal reuse:** the list reuses `PurchaseService.GetInvoicesAsync("purchaseAdvanceInvoices", …)` + `PurchaseInvoiceListItemDto` (same columns, no new list DTO/mapper). The detail adds `GetAdvanceInvoiceByIdAsync` + a new `PurchaseAdvanceInvoiceDetailDto` (reuses the existing purchase Header/Line/Totals DTOs + the generic `PaymentTrackingDto` introduced by US-014). `PurchaseAdvanceInvoiceDetailMapper` reuses shared `InvoiceStatus.Normalize`, `InvoiceTotals`, `BcListQuery` — no duplicated list/detail logic.
- **Payment tracking:** `amount` = document total (line-derived); `amountPaid` reconciled from the normalized payment status (PAID → full, PARTIAL → half, OPEN → 0); `remaining` recomputed so the `amount = amountPaid + remaining` invariant holds exactly regardless of the amount base BC reports against.
- MockBcHttpClient: new `purchaseAdvanceInvoices` collection (8 advance invoices, numbers `PA-2026-00N`, vendor names from existing mock vendors, status mix Open/Partially Paid/Paid) in both collection + GetById paths (reuses the generalized document-collection helper + `FindPurchaseInvoiceWithLines`). Existing mock/dashboard/sales/purchase tests preserved.
- Frontend `app/(protected)/purchase/advance-invoices/page.tsx` (list — Number/Vendor/Date/Amount/Status, FilterPanel search+status+date-range, sortable date/amount, row → detail, purple "Advance" tag distinguishing it from regular purchase invoices) + `[id]/page.tsx` (detail — header card [vendor, our reference, dates, payment terms] + payment-tracking section [Amount/Paid/Remaining] + line-items table + totals; 404 → not-found; "Advance" tag). RBAC guard `['ADMIN','MANAGER','ACCOUNTING']`. Reuses EntityTable/FilterPanel/usePaginatedQuery/formatRsd/statusBadgeClass — no new frontend helper introduced.
- API doc: `docs/api/purchase-advance.md` v1.0 (both endpoints, RequireFinancial, query/path params, success examples incl. payment tracking, 400/401/403/404 NOT_FOUND_PURCHASE_ADVANCE_INVOICE/500/502, OPEN|PARTIAL|PAID enum, Q-003 schema note). i18n: `purchaseAdvance` + `purchaseAdvanceDetail` sections (sr + en); reuses `purchase.status.*` labels. Sidebar nav entry "Purchase Advance Invoices" (financial module).
- Tests: backend +17 (PurchaseAdvanceInvoiceTests: list returns items / pageSize / total / vendorName populated / status normalization / status+search filter; detail known/unknown id, totals, payment-tracking invariant + PAID/OPEN/PARTIAL states + amount = document total + status normalization). Existing `PurchaseServiceTests`/`SearchServiceTests` ctor updated for the new mapper. Frontend unchanged (reused already-tested helpers; pages are out of unit-coverage scope). Combined: 259 backend + 118 frontend passing; lint clean; coverage gate green.

### US-016: Credit Notes & Storno Invoices — List & Detail
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Two BC-backed endpoints in a new `/api/v1/credit-documents` group (RequireFinancial — ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403): GET `/` (unified list) and GET `/{id}` (detail). Canonical envelope `{ success, data }`; 404 `NOT_FOUND_CREDIT_DOCUMENT` for unknown id; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. A single `creditDocuments` collection mixes credit memos, debit memos, and storno (cancellation) invoices, each referencing the original invoice it corrects.
- **Cross-layer document-type enum:** `Domain/Constants/CreditDocumentType` (CREDIT_MEMO | DEBIT_MEMO | STORNO, `All` + `IsValid`) is the single source of truth for the SCREAMING_SNAKE wire values shared by the backend, the OData `type eq` filter, and the frontend i18n labels (per enums-and-constants.md). Documented allowed values in `docs/api/credit-documents.md`.
- **Maximal reuse:** `ICreditDocumentService`/`CreditDocumentService` mirrors `SalesService` — uses shared `BcListQuery` (page/sort/filter; allow-list date/number/amount), `InvoiceTotals`, and reuses `SalesInvoiceLineDto` + `SalesInvoiceTotalsDto` for the detail (no new line/totals DTO). New `BcCreditDocument` BC DTO (adds `Type` + `OriginalInvoiceNumber`, reuses `salesInvoiceLines` $expand), `CreditDocumentListItemDto`, `CreditDocumentDetailDto` (+ header). `CreditDocumentMapper`/`CreditDocumentDetailMapper` normalize Type via `NormalizeType` and Status via shared `InvoiceStatus.NormalizeCreditMemo` (OPEN | POSTED).
- **Filters:** search (number / partyName contains), `type eq '<wire value>'` (validated against `CreditDocumentType.IsValid` — unknown types ignored, defends against injection), posting-date range (ge/le); single quotes escaped.
- MockBcHttpClient: new `creditDocuments` collection (12 records evenly mixing the 3 types, numbers CN-/DN-/ST-, each referencing an existing mock sales-invoice number like SI-001) in both collection + GetById paths (reuses the generalized document-collection helper with `partyName` as the party field + a new `FindCreditDocumentWithLines`). Added a `type eq` clause to the shared in-memory filter. All existing mock/dashboard/sales/purchase tests preserved.
- Frontend `app/(protected)/credit-documents/page.tsx` (list — Number(sortable)/Type(badge)/Party/Date(sortable)/Original Invoice/Amount(sortable)/Status(badge); FilterPanel search + type select [All/Credit Memo/Debit Memo/Storno → `type` param] + date range; row → detail) + `[id]/page.tsx` (detail — header with type + status badges, prominent original-invoice reference, header card, line-items table, totals; 404 `NOT_FOUND_CREDIT_DOCUMENT` → not-found + back link). RBAC guard `['ADMIN','MANAGER','ACCOUNTING']`. Reuses EntityTable/FilterPanel/usePaginatedQuery/formatRsd/creditMemoStatusBadgeClass.
- New pure FE helper `creditDocumentTypeBadgeClass(type)` in `lib/format.ts` (CREDIT_MEMO green, DEBIT_MEMO amber, STORNO red, unknown gray) — unit-tested. Sidebar nav entry "Credit Documents" → `/credit-documents` (financial module).
- i18n: `creditDocuments` + `creditDocumentDetail` sections (sr + en) incl. type labels keyed by wire value (`type.CREDIT_MEMO` = "Knjižno odobrenje"/"Credit Memo", `.DEBIT_MEMO` = "Knjižno zaduženje"/"Debit Memo", `.STORNO` = "Storno"); reuses OPEN/POSTED status labels.
- API doc: `docs/api/credit-documents.md` v1.0 (both endpoints, RequireFinancial, query/path params incl. `type`, success examples, 401/403/404 NOT_FOUND_CREDIT_DOCUMENT/500/502, allowed Type enum CREDIT_MEMO|DEBIT_MEMO|STORNO + Status OPEN|POSTED).
- Tests: backend +21 (CreditDocumentServiceTests ×17: items across all types, pageSize, total, type filter narrows to one type / narrows results, search by number / party, status + type normalization, original-invoice mapping, invalid-type ignored, detail known/storno/unknown id, totals; CreditDocumentTypeTests ×4 incl. IsValid truth table). Frontend +4 (creditDocumentTypeBadgeClass). Combined: 284 backend + 122 frontend passing; lint clean; coverage gate green.

### US-017: Items — List View
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** See git log

**Summary:**
- **First WAREHOUSE-module screen.** BC-backed `GET /api/v1/items` (RequireWarehouse — ADMIN/MANAGER/WAREHOUSE; ACCOUNTING 403): paginated/searchable/sortable/filterable item list with stock levels. Canonical envelope `{ success, data: PagedResultDto }`; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure. Warehouse analogue of the vendor list (US-011), using the WAREHOUSE policy (T-004) rather than the financial one.
- `BcItem` BC DTO (number, description, category, location, unitOfMeasure, quantityOnHand, minimumStock, unitCost, unitPrice) + `ItemListItemDto` (+ computed `isLowStock`). `ItemMapper` (IBcMapper) mirrors VendorMapper and computes `IsLowStock = QuantityOnHand < MinimumStock` server-side (single source of the threshold).
- `IItemService` + `ItemService`: reuses shared `BcListQuery` (page/pageSize clamp, single-quote-escaped contains-filter on number/description). Sort allow-list `description`/`quantityOnHand`/`unitCost` with UI key mapping (name→description, quantity→quantityOnHand, unitCost→unitCost). Appends `category eq` + `location eq` equality filters (ANDed with search when present). Registered in Program.cs (scoped service + singleton mapper + `MapItemsEndpoints`).
- MockBcHttpClient: new `items` collection (12 items, numbers `ITM-001`…, categories GRAĐEVINA/ALATI/ELEKTRO/FARBE, locations MAGACIN-1/2, UoM KOM/KG/M; several below minimum stock so the low-stock badge has data). New `ApplyItemFilter` (contains number/description + category eq + location eq) + `ApplyItemSort` (description/quantityOnHand/unitCost). Honors Top/Skip/Count. Additive — all existing mock/dashboard tests preserved.
- Frontend `app/(protected)/items/page.tsx` (ItemListScreen): EntityTable columns Number(link), Description(sortable, link), Unit, Qty on Hand (right-aligned, sortable, amber "Low stock" badge when isLowStock), Unit Cost (formatRsd, right-aligned, sortable); FilterPanel search (number/description) + category select + location select; amber row tint via `rowClassName` for low-stock rows; row click → `/items/{id}` (detail in US-018). **Guarded with `useRequireModule('warehouse')`** so ACCOUNTING is redirected to /403 while ADMIN/MANAGER/WAREHOUSE pass.
- Sidebar: added "Items" (sr "Artikli") → `/items` tagged `module: 'warehouse'` — first warehouse nav entry; now visible to ADMIN/MANAGER/WAREHOUSE and hidden from ACCOUNTING. Sidebar test updated (WAREHOUSE now sees Dashboard + Items).
- New pure FE helper `lowStockRowClass(isLowStock)` in `lib/format.ts` (amber tint vs none) — unit-tested. i18n: `items` section (sr + en).
- API doc: `docs/api/items.md` v1.0 (endpoint, RequireWarehouse auth, query params incl. category/location, success example with isLowStock, errors 400/401/403/404/500/502).
- Tests: backend +17 (ItemService ×14: items, pageSize, total, invalid page→1, invalid pageSize→20, search narrows, search-by-number, category/location filter narrows, category+search AND, field mapping incl. IsLowStock true/false, sort quantity/name/unitCost; ItemMapper ×4: all fields, IsLowStock true/at-threshold/above, new-instance). Frontend +2 (lowStockRowClass). Combined: 302 backend + 124 frontend passing; lint clean; coverage gate green.

### US-018: Items — Detail View
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** See git log

**Summary:**
- Warehouse item DETAIL screen — the item analogue of vendor detail (US-012). Two BC-backed endpoints on the existing `/api/v1/items` group (RequireWarehouse — ADMIN/MANAGER/WAREHOUSE; ACCOUNTING 403): `GET /api/v1/items/{id}` (profile + stock by location + recent ledger entries) and `GET /api/v1/items/{id}/ledger-entries` (last 20 movements). Both 404 `NOT_FOUND_ITEM` for unknown ids; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure.
- `ItemDetailDto` (ItemProfileDto + List<StockByLocationDto> + List<ItemLedgerEntryDto>). `ItemProfileDto` extends the list shape with `unitPrice` + computed `isLowStock`. BC raw shapes `BcStockByLocation` + `BcItemLedgerEntry`; `BcItem` extended with a `StockByLocation` expand navigation (empty for the list query, ignored by the list mapper).
- New mappers `ItemDetailMapper` (BcItem→ItemProfileDto, computes IsLowStock), `StockByLocationMapper`, `ItemLedgerEntryMapper` (normalizes BC entry-type casing to the wire value). Registered in Program.cs. `ItemService` extended (constructor now injects the 3 new mappers): `GetItemByIdAsync` (null→404, mirrors VendorService) + `GetItemLedgerEntriesAsync(top=20)` (null→404). Ledger fetched from a new mock `itemLedgerEntries` collection filtered by `itemId eq` (single-quote-escaped), `date desc`, capped at top.
- **Cross-layer enum:** `Domain/Constants/ItemLedgerEntryType` (PURCHASE/SALE/ADJUSTMENT/TRANSFER, SCREAMING_SNAKE wire value) with `IsValid` + `Normalize` (BC casing → wire value, unknown→ADJUSTMENT). Frontend maps values to i18n labels (`items.ledgerType.*`); allowed values documented in items.md.
- MockBcHttpClient (additive): `GetByIdAsync("items", id)` returns a populated item with stock-by-location (2 locations, ~60/40 split summing to the item total + reserved) for known ids, null otherwise; `itemLedgerEntries` collection (~12 deterministic movements/item, mixed types, signed quantity, running `remaining` ending at current qty on hand). All existing mock/list/dashboard behavior preserved.
- Frontend `app/(protected)/items/[id]/page.tsx` (ItemDetailScreen), guarded with `useRequireModule('warehouse')`: header (number h1 + description), low-stock alert banner when isLowStock, profile card (category/UoM/unitCost/unitPrice via formatRsd), stock-by-location table, recent ledger table (date, type badge via i18n, signed quantity colored, remaining). 404 → "Item not found" + back link; loading skeleton + error banner. Single fetch (`/items/{id}` includes recentLedgerEntries).
- New pure FE helpers in `lib/format.ts`: `ledgerTypeKey` (wire value → i18n key, unknown→ADJUSTMENT) + `formatSignedQuantity` (+prefix for inbound) — both unit-tested.
- API doc: `docs/api/items.md` → v1.1 (both detail endpoints, RequireWarehouse auth, path params, success examples incl. stock-by-location + ledger entries, errors incl. 404 NOT_FOUND_ITEM/502, allowed EntryType enum values).
- Tests: backend +13 (ItemService ×9: detail known→profile incl. unitPrice, unknown→null, IsLowStock true/false, stock-by-location present+sums-to-total+reserved≥0, ledger present/capped/normalized, newest-remaining=qtyOnHand, ledger known→list, unknown→null, top cap; ItemLedgerEntryType ×4 theories/facts: IsValid, All length, wire values, Normalize). Frontend +2 (ledgerTypeKey, formatSignedQuantity). Combined: 327 backend + 129 frontend passing; lint clean; coverage gate green (format.ts 100%).

### US-019: Inventory — Stock Overview
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** See git log

**Summary:**
- **Final Phase 3 story — Phase 3 now COMPLETE (42/42).** WAREHOUSE-module inventory overview screen blending the Dashboard KPI-aggregation pattern (US-003) with the item / stock-by-location data (US-017/018). Three BC-backed endpoints in a new `/api/v1/inventory` group (RequireWarehouse — ADMIN/MANAGER/WAREHOUSE; ACCOUNTING 403): `GET /summary` (optional `location`/`category` filters), `GET /locations`, `GET /low-stock`. Canonical envelope `{ success, data }`; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure.
- `InventorySummaryDto` (totalItems, totalStockValue = Σ quantityOnHand×unitCost, itemsBelowMinimum), `InventoryLocationDto` (location, itemCount, totalQuantity, totalValue), `LowStockItemDto` (id, number, description, quantityOnHand, minimumStock, location). `IInventoryService`/`InventoryService` injects `IBcHttpClient` + `ICacheService` + `BcOptions`.
- **Aggregation:** summary fetches the `items` collection (optional location/category `eq` filters, single-quotes escaped) and aggregates in memory (mirrors DashboardService over BC collections); cached 5 min via `ICacheService` per filter combination (key `inventory:summary:{location}:{category}`). By-location groups each item's stock-by-location rows (resolved via `GetByIdAsync` $expand, US-018) into per-location item count / total quantity / total value, allocating each item's value proportionally so per-location values reconcile with the overall stock value. Low-stock filters `quantityOnHand < minimumStock`, sorted ascending by quantity.
- Mock: reuses the existing `items` (12, several below minimum) + stock-by-location data (US-017/018) — no new entity set. All existing mock/dashboard/item tests preserved.
- Frontend `app/(protected)/inventory/page.tsx` (InventoryOverviewScreen), guarded with `useRequireModule('warehouse')`: 3 summary cards (Total Items, Total Stock Value via formatRsd, Items Below Minimum — amber when > 0) with location/category select filters that refetch the summary; stock-by-location table (Location/Item Count/Total Quantity/Total Value); low-stock table (Number/Description/Qty on Hand/Minimum/Location) with amber rows, sorted by quantity ascending, rows link to `/items/{id}`. Loading skeleton + error banner mirror the dashboard.
- New pure FE helpers in `lib/format.ts`: `sortByQuantityAsc` (defensive client re-sort, non-mutating) + `belowMinimumCardClass` (amber when count > 0) — both unit-tested. Sidebar: added "Inventory" (sr "Zalihe") → `/inventory` tagged `module: 'warehouse'`. i18n: `inventory` section (sr + en).
- API doc: `docs/api/inventory.md` v1.0 (all three endpoints, RequireWarehouse auth, query params, success examples, errors 400/401/403/404/500/502).
- Tests: backend +10 (InventoryServiceTests: summary positive totals / below-minimum=4 matches mock / stock-value>threshold / category+location filter narrows / cached second call; by-location groups consistent + value reconciles with summary; low-stock only-below-minimum sorted ascending + count matches summary). Frontend +5 (sortByQuantityAsc ×3, belowMinimumCardClass ×2). Combined: 337 backend + 134 frontend passing; lint clean; coverage gate green (format.ts 100%).

---

## Phase 3 — COMPLETE

**Completed:** 2026-05-25 — all 6 user stories (US-014…US-019) + T-004 done, 42/42 story points (100%). Advance invoices (sales + purchase), credit/debit/storno documents, warehouse items list + detail + inventory overview, and cross-layer role-based module locking (RequireWarehouse) all shipped. Ready for Phase 4.

---

### US-020: Analytics Dashboard
**Completed:** 2026-05-25
**Phase:** 4 — Analytics & Polish
**Story Points:** 8
**Commit:** See git log

**Summary:**
- Management analytics screen (FINANCIAL module — RequireFinancial: ADMIN/MANAGER/ACCOUNTING; WAREHOUSE 403) blending the Dashboard KPI-aggregation pattern (US-003) over sales (revenue) + purchase (expense) invoices, plus top customers / top items, with period comparison and PDF/Excel export (T-005). Three BC-backed endpoints in a new `/api/v1/analytics` group: `GET /revenue-expense` (granularity + date-range), `GET /top-customers`, `GET /top-items`. Canonical envelope `{ success, data }`; 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure.
- **Cross-layer Granularity enum** (`Domain/Constants/Granularity` — MONTHLY | QUARTERLY | YEARLY, SCREAMING_SNAKE wire value, `IsValid` + `Normalize` → invalid/missing falls back to MONTHLY). Frontend maps to i18n labels (`analytics.granularity.*`); allowed values documented in `docs/api/analytics.md`.
- DTOs `RevenueExpensePointDto` (period/revenue/expense/profit), `RevenueExpenseSeriesDto` (points + comparison + granularity), `PeriodComparisonDto` (current vs prior totals + delta %), `TopCustomerDto`, `TopItemDto`. `IAnalyticsService`/`AnalyticsService` injects `IBcHttpClient` + `ICacheService` + `BcOptions`.
- **Aggregation:** revenue = sales-invoice totals, expense = purchase-invoice totals, bucketed by period per granularity (MONTHLY → yyyy-MM, QUARTERLY → yyyy-Qn, YEARLY → yyyy); profit = revenue − expense per bucket. Range defaults to last 12 months. PeriodComparison compares the resolved range (current) vs the immediately-preceding equal-length window (prior); delta % = (cur−prior)/prior×100 rounded to 1 dp, **guarded against prior=0** (returns 0). Revenue-expense result cached 5 min via `ICacheService`, keyed by granularity + range (mirrors DashboardService). OData filter values escaped; in-memory aggregation over fetched collections (mirrors DashboardService). Top customers group sales invoices by customerName (id resolved from customers mock, name fallback). 
- **Top items by sales volume:** invoice line items in this BC use free-text descriptions not linked to item records, so they cannot be aggregated by item. Sourced from an additive `salesVolume`/`salesValue` basis on the `BcItem` entity + items mock (in real BC derived from item-ledger SALE entries). Additive — list/detail/inventory mappers ignore the new fields; all existing item/inventory/dashboard tests preserved.
- Mock: sales + purchase invoice collections extended with 5 older records each (8–22 months ago) so the period comparison has a non-trivial prior window; items mock gains the sales-volume basis. No new entity set.
- Frontend `app/(protected)/analytics/page.tsx` (AnalyticsDashboardScreen), guarded `useRequireAuth(['ADMIN','MANAGER','ACCOUNTING'])`: granularity toggle (Monthly/Quarterly/Yearly) + date-range picker (from/to date inputs) that refetch the series; period-comparison cards (current vs prior revenue + expense, delta % colored green up / red down with arrow + delta amount via formatRsd); revenue-vs-expense paired CSS bar chart (NO chart lib — reuses dashboard's bar approach, paired bars per period + legend); top-10 customers table (Name/Revenue/Invoice count) + top-10 items table (Number/Description/Sales volume/Sales value); PDF + Excel export buttons wiring `exportToPdf`/`exportToExcel` (T-005) over the revenue-expense series; loading skeletons + error banner.
- New pure FE helpers in `lib/analytics.ts` (unit-tested): `deltaPercent` (prior=0 guard), `deltaClass`, `deltaArrow`, `granularityLabelKey`, `barHeightPercent`, `seriesMax` + `GRANULARITIES` const. Sidebar: added "Analytics" (sr "Analitika") → `/analytics` (financial module). i18n: `analytics` section incl. nested `granularity` labels (sr + en).
- API doc: `docs/api/analytics.md` v1.0 (all three endpoints, RequireFinancial auth, granularity/top/date-range params, success examples, allowed Granularity enum MONTHLY|QUARTERLY|YEARLY, errors 401/403/404/500/502, top-items source note).
- Tests: backend +13 (AnalyticsServiceTests: revenue/expense points positive + profit=revenue−expense; granularity buckets differ (monthly ≥ quarterly ≥ yearly, monthly > yearly) + quarterly label format; invalid granularity → MONTHLY; non-trivial comparison + delta-zero-prior guard; cached second call; top-customers ordered desc + capped + invoiceCount>0; top-items ordered desc + capped). Frontend +24 (analytics.test.ts: deltaPercent ×4 incl. prior=0, deltaClass ×3, deltaArrow ×3, granularityLabelKey ×4, barHeightPercent ×3, seriesMax ×2). `analytics.ts` at 100% coverage; gate green.
- Combined suites: 362 backend + 173 frontend passing; lint clean; coverage gate green.

### US-021: User Management — Admin Panel
**Completed:** 2026-05-25
**Phase:** 4 — Analytics & Polish
**Story Points:** 8
**Commit:** See git log

**Summary:**
- **First WRITE-ops story.** All writes target the LOCAL Postgres `users` table only — Business Central stays read-only. Admin-only screen (UserManagementScreen) + four endpoints in a new `/api/v1/admin/users` group under the `RequireAdmin` policy (ADMIN only; all other roles → 403). Canonical envelope `{ success, data }` / `{ success, error, code }`.
- **Endpoints:** `GET /` (list, no password hash), `POST /` (create — validates role + unique username/email, bcrypt-hashes temp password cost 12, 201), `PUT /{id}` (update role + active status), `POST /{id}/reset-password` (regenerate CSPRNG temp password, bcrypt-hash, revoke the user's active refresh tokens, email via the T-005 no-op `IEmailService`), `DELETE /{id}` (with guards). Actor read from JWT claims (NameIdentifier/Name) for audit attribution + the self-delete guard.
- **Guards:** cannot delete own account (`409 CONFLICT_CANNOT_DELETE_SELF`); cannot delete OR deactivate/demote the last active admin (`409 CONFLICT_LAST_ADMIN`). Error codes SCREAMING_SNAKE; mapped to 400/404/409/500 at the endpoint.
- **Entity + migration:** `User` gained `Name` (required), `Email` (nullable, unique-check in service), `LastLoginAt` (nullable) additively. Hand-authored EF migration `20260525010000_AddUserProfileFields` (.cs + .Designer.cs with `[Migration]`+`[DbContext]` attributes + updated `PinolesDbContextModelSnapshot`) — verified discoverable + ordered by a new `MigrationsDiscoveryTests` reflection test (guards the prior "MigrateAsync silently skips a migration" bug; InMemory tests do not exercise migrations). Seeder sets Name/Email on the 4 seed users.
- **Audit logging wired:** new `IAuditWriter`/`AuditWriter` (stages an `AuditLog` row in the same SaveChanges as the mutation) + `AuditActions` constants. Every admin action audits `ADMIN_USER_CREATED` / `_UPDATED` / `_DELETED` / `ADMIN_PASSWORD_RESET` (actor id/username + target details, NO passwords). `AuthService.LoginAsync` now also writes `AUTH_LOGIN_SUCCESS` and sets `LastLoginAt` on successful login — giving US-023 (audit log view) real data. Plaintext passwords are never logged or returned.
- **Frontend:** `app/(protected)/admin/users/page.tsx` guarded by `useRequireModule('admin')`; EntityTable (Name/Email/Role/Last Login/Status badge) + per-row Edit / Reset Password / Delete; Create + Edit modals (react-hook-form + zod); reset + delete confirm dialogs; guard errors mapped from error code to friendly i18n messages; Bearer-token fetches; success/error banners; list refetch after mutations. Sidebar gains "User Management" (admin module → ADMIN only). i18n: `userManagement` section (sr + en) with role + status labels keyed by wire value.
- New pure FE helpers in `lib/admin/users.ts` (unit-tested, 100% cov): `statusFromActive`, `userStatusBadgeClass`, `errorMessageKey` (code→i18n key), `createUserSchema` + `editUserSchema` (zod). API doc: `docs/api/admin-users.md` v1.0 (all endpoints, RequireAdmin auth, request/response examples, ACTIVE|INACTIVE + role enums, error codes 400/401/403/404/409/500, self/last-admin guards, audit table).
- Tests: backend +18 (UserAdminServiceTests ×14: list hash-free, create hashes+audits, invalid role, duplicate username, short password, update role/status+audit, not-found, deactivate last admin blocked, deactivate admin when another exists, reset changes hash+sends email+revokes tokens+audits, reset not-found, delete-self blocked, delete-last-admin blocked, delete not-found, delete succeeds+audits; AuthServiceTests +2: login writes audit row + sets LastLoginAt, failed login writes no audit; MigrationsDiscoveryTests ×2). Frontend +16 (admin/users.test.ts).
- Combined suites: 381 backend + 191 frontend passing; lint clean; coverage gate green.

### US-022: Settings — BC Connection Configuration
**Completed:** 2026-05-25
**Phase:** 4 — Analytics & Polish
**Story Points:** 3
**Commit:** See git log

**Summary:**
- Admin-only, read-only SettingsScreen + two endpoints in a new `/api/v1/settings` group under the `RequireAdmin` policy (ADMIN only; all other roles → 403; missing/invalid token → 401). Canonical envelope `{ success, data }` / `{ success, error, code }`.
- **Endpoints:** `GET /bc-config` (non-sensitive BC connection config + per-entity last-sync timestamps) and `POST /bc-config/test` (on-demand connectivity probe). The probe always returns HTTP 200 — connectivity success/failure is carried INSIDE the payload (`data.success`); a BC outage surfaces as `data.success=false` + `data.errorCode="INTEGRATION_BC_UNAVAILABLE"`, not an HTTP error. A 500 `INTERNAL_ERROR` only fires if the probe machinery itself faults.
- **SECURITY (hard requirement):** the BC service-principal credentials (`BcOptions.ClientId` / `ClientSecret`) are NEVER read or returned. `BcConfigDto` exposes only TenantId (masked to its first 4 chars + ellipsis), CompanyId, Environment (derived from BaseUrl; "Mock" in mock mode), BaseUrl, and UseMock. A dedicated test seeds a known secret + client id into the test `BcOptions` and asserts neither value appears in the serialized DTO. Probe failures log the failure fact + timing only (no credential).
- `ISettingsService`/`SettingsService` injects `IOptions<BcOptions>` + `IBcHttpClient` + `ICacheService`. `GetBcConfigAsync` reads per-entity last-sync timestamps (customers/salesInvoices/vendors/items) from the cache (null → "Never" in the UI). `TestConnectionAsync` performs a lightweight `Top=1` `customers` read, measures duration via `Stopwatch`, records the success time against every tracked entity in the cache, and returns `{ success, durationMs, lastSuccessfulSyncAt }`; a thrown BC error maps to `{ success=false, errorCode=INTEGRATION_BC_UNAVAILABLE }`. In mock mode the probe always succeeds. Registered scoped in Program.cs + `MapSettingsEndpoints`.
- **Frontend** `app/(protected)/settings/page.tsx` guarded by `useRequireModule('admin')`: "BC Connection" card (Tenant ID, Company ID, Environment, Base URL, Mode + amber "Mock" / green "Live" badge mirroring the dashboard status badge — explicitly no credentials), "Test Connection" button (spinner while testing → green "✓ Connected in {ms}ms, last sync {time}" or red "✕ Connection failed" result), and a "Last Sync" table (entity type + timestamp / "Never"); Bearer-token fetches; loading skeleton + error banner. Sidebar gains "Settings" → `/settings` (admin module → ADMIN only).
- New pure FE helpers in `lib/settings.ts` (unit-tested, 100% cov): `connectionModeBadgeClass`, `connectionModeKey`, `formatEntityType` (camelCase → "Title Case"), `formatSyncTimestamp` (null/unparseable → never label). i18n: `settings` section (sr + en). API doc `docs/api/settings.md` v1.0 (both endpoints, RequireAdmin auth, success examples, explicit no-credentials note, in-payload success/failure note, errors 401/403/500).
- Tests: backend +9 (SettingsServiceTests: GetBcConfig returns non-sensitive fields, does NOT expose client secret/id [serialized-DTO assertion], masks tenant id, lists tracked entities, last-sync null before any probe; TestConnection mock-mode success [durationMs≥0 + timestamp set + null errorCode], records last-sync for subsequent config read, throwing-BC stub → failure + INTEGRATION_BC_UNAVAILABLE, live-mode derives environment from base URL). Frontend +12 (settings.test.ts: badge class mock/live, mode key, formatEntityType camelCase/single/empty, formatSyncTimestamp null/unparseable/valid).
- Combined suites: 390 backend + 201 frontend passing; lint clean; coverage gate green (settings.ts 100%).

### US-023: Audit Log — View
**Completed:** 2026-05-25
**Phase:** 4 — Analytics & Polish
**Story Points:** 3
**Commit:** See git log

**Summary:**
- **Final story of Phase 4 and the entire project.** Admin-only AuditLogScreen + one endpoint in a new `/api/v1/admin/audit-log` group under the `RequireAdmin` policy (ADMIN only; all other roles → 403; missing/invalid token → 401). Reads the LOCAL Postgres `audit_logs` table only (the rows written by US-021 login + admin actions) — Business Central is never touched, so the 502 path does not apply. Canonical envelope `{ success, data: PagedResultDto<AuditLogEntryDto> }`.
- **Endpoint:** `GET /api/v1/admin/audit-log?page&pageSize&category&username&fromDate&toDate` — paginated, newest-first (OrderByDescending CreatedAt). Filters: `category` (mapped to an action-code prefix/substring), `username` (case-insensitive contains), and a CreatedAt date range (`fromDate` inclusive, `toDate` inclusive of the whole day). page/pageSize clamped (default 20, max 100). Local EF Core LINQ query over `PinolesDbContext.AuditLogs` (NOT BcListQuery — that is for BC/OData).
- **Cross-layer category enum:** `Domain/Constants/AuditActionCategory` (LOGIN | VIEW | EXPORT | ADMIN, SCREAMING_SNAKE wire value) is the single source of truth, with `IsValid`, `All`, and a `Categorize(action)` mapping (`AUTH_LOGIN*` → LOGIN, `*EXPORT*` → EXPORT, `*VIEW*` → VIEW, `ADMIN_*` and everything else → ADMIN). VIEW/EXPORT buckets return empty until such events are emitted — the filter still works. `AuditLogEntryDto` exposes Id, ISO Timestamp, Username, granular Action, computed Category, best-effort EntityType/EntityId (parsed from the action code + the `id=...` token in Details), and Details. No passwords/tokens are surfaced (the audit table never stores them).
- `IAuditLogService`/`AuditLogService` injects `PinolesDbContext`; registered scoped in Program.cs + `MapAuditLogEndpoints`.
- **Frontend** `app/(protected)/admin/audit-log/page.tsx` guarded by `useRequireModule('admin')`: `usePaginatedQuery` against the endpoint; FilterPanel (category select [All/Login/View/Export/Admin → `category` param], username search, date range → `fromDate`/`toDate`); EntityTable columns Timestamp (formatted), User, Category (badge), Action (granular code), Entity Type, Entity ID; "Export to Excel" button wiring the T-005 `exportToExcel` over the current rows (Timestamp/User/Action/Category/Entity Type/Entity ID columns); loading/empty/error states; Bearer-token fetch. Sidebar gains "Audit Log" (sr "Dnevnik aktivnosti") → `/admin/audit-log` (admin module → ADMIN only).
- New pure FE helpers in `lib/admin/audit-log.ts` (unit-tested, 100% cov): `isAuditCategory`, `categoryLabelKey` (unknown → ADMIN fallback), `categoryBadgeClass` (LOGIN blue / VIEW gray / EXPORT green / ADMIN amber), `formatAuditTimestamp` (null/invalid → placeholder) + `AUDIT_CATEGORIES` const. i18n: `auditLog` section (sr + en) with category labels keyed by wire value (`category.LOGIN` = "Prijava"/"Login", `.VIEW` = "Pregled"/"View", `.EXPORT` = "Izvoz"/"Export", `.ADMIN` = "Administracija"/"Admin").
- API doc: `docs/api/audit-log.md` v1.0 (endpoint, RequireAdmin auth, query params incl. category/username/date range, success example PagedResultDto, allowed Category enum LOGIN|VIEW|EXPORT|ADMIN, errors 401/403/500).
- Tests: backend +24 (AuditLogServiceTests ×12: newest-first order, category ADMIN/LOGIN narrows, category EXPORT empty when no such events, username case-insensitive filter, date-range narrows, pageSize respected + total + no page overlap, pageSize clamped to 100, category computed + entity parsed, login entry null entity, negative/zero paging clamped; AuditActionCategoryTests ×12: Categorize truth table incl. null/empty/unknown→ADMIN, IsValid truth table, All length, wire values). Frontend +19 (admin/audit-log.test.ts: isAuditCategory, AUDIT_CATEGORIES order, categoryLabelKey fallback, categoryBadgeClass all 4 + unknown, formatAuditTimestamp null/invalid/valid). `audit-log.ts` at 100% coverage; gate green.
- Combined suites: 421 backend + 214 frontend passing; lint clean; coverage gate green.

---

## Phase 4 — COMPLETE

**Completed:** 2026-05-25 — all 4 user stories (US-020…US-023) + T-005 done, 27/27 story points (100%). Analytics dashboard with charts + period comparison, PDF/Excel export + in-app notifications + email infra, admin user management (first write-ops), BC settings view + connectivity probe, and the audit-log viewer all shipped.

## PROJECT COMPLETE

**Completed:** 2026-05-25 — Pinoles is at **168/168 story points (100%)** across all four phases: 23 user stories + 5 technical tasks. Phase 1 Foundation & MVP (47), Phase 2 Core Documents (52), Phase 3 Extended Modules (42), Phase 4 Analytics & Polish (27). Read-only employee portal over Microsoft Dynamics 365 Business Central with role-based module locking, full financial + warehouse document coverage, universal search, analytics, admin user management, settings, and audit logging. Final combined suite: 421 backend + 214 frontend tests passing; lint clean; coverage gate green.

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

### T-004: Role Guard & Module Access Infrastructure
**Completed:** 2026-05-25
**Phase:** 3 — Extended Modules
**Story Points:** 5
**Commit:** f4790a3

**Summary:**
- One source of truth for module → allowed-roles, enforced on BOTH layers, with tests proving 403 per role for locked modules. Module model: Dashboard (all 4 roles), Financial (ADMIN/MANAGER/ACCOUNTING), Warehouse (ADMIN/MANAGER/WAREHOUSE), Admin (ADMIN). WAREHOUSE: no financial, yes warehouse + dashboard; ACCOUNTING: financial + dashboard, no warehouse.
- Backend: added `RequireWarehouse` authorization policy (ADMIN/MANAGER/WAREHOUSE) to Program.cs alongside the existing RequireAdmin/RequireFinancial/RequireDashboard (all kept intact — Phase 2 endpoints unaffected). New `Domain/Constants/ModuleAccess.cs` (Dashboard/Financial/Warehouse/Admin role arrays + `CanAccess(moduleRoles, role)`), built on the existing UserRoles constants. No placeholder endpoints (real warehouse endpoints arrive in US-017+).
- Frontend: new `lib/auth/module-access.ts` (`MODULE_ACCESS` map + `canAccessModule(module, role)` — null/unknown role → false, mirrors the BE map). New `lib/hooks/use-require-module.ts` whole-page module guard mirroring the fixed fresh-`getState()` pattern from use-require-auth (initFromSession then decide off fresh state: unauthenticated → /login, no module access → /403; no stale-closure redirect). Sidebar refactored: each NAV_ITEM tagged with a `module` and filtered via `canAccessModule` instead of hand-listed role arrays (dashboard=dashboard, customers/vendors/sales/purchase=financial); existing behavior + sidebar tests preserved; no warehouse nav links added (routes don't exist yet). RoleGuard (US-002) left as-is for inline element gating.
- Tests: backend +8 (ModuleAccessTests truth table — Dashboard all roles, Financial excludes WAREHOUSE, Warehouse excludes ACCOUNTING, Admin only ADMIN, ADMIN+MANAGER full access, unknown role false). Frontend +11 (module-access.test.ts truth table 4 modules × 4 roles + null/undefined/unknown = 6 cases; use-require-module.test.tsx 5 cases — unauthenticated → /login, WAREHOUSE on financial → /403, WAREHOUSE on warehouse → no redirect, ADMIN on financial → no redirect, null user → /403; mocks useAuthStore incl. .getState). All new lib/components files at 100% coverage — 80% gate green.
- Combined suites: 228 backend + 118 frontend passing; lint clean.

### T-005: Export & Notification Infrastructure
**Completed:** 2026-05-25
**Phase:** 4 — Analytics & Polish
**Story Points:** 5
**Commit:** See git log

**Summary:**
- **First Phase 4 task — shared infra US-020 (analytics export) + US-023 (audit-log export) + US-021 (password-reset email) consume.** Three deliverables: client-side export utilities, an in-app notification system (BC-backed endpoint + topbar bell), and a no-op dev email service.
- **(A) Export utilities** — `apps/web/lib/export.ts` (deps `xlsx`, `jspdf`, `jspdf-autotable` added to `apps/web/package.json`; root `pnpm-lock.yaml` updated). Pure data-shaping core (`toRowMatrix`, `extractHeaders`, `rowToCells`) isolated from the library + browser-download side effects; `exportToExcel` builds an `.xlsx` worksheet (header row + data) and downloads `${filename}.xlsx`; `exportToPdf` builds a titled PDF table via jsPDF + autotable and downloads `${filename}.pdf`. `ExportColumn<T> { header, value(row) }` model mirrors EntityTable columns. No API endpoint — export runs entirely client-side.
- **(B) Notification system** — BC-backed `GET /api/v1/notifications` (RequireDashboard — ALL roles; list itself role-filtered). `INotificationService`/`NotificationService` aggregates two kinds from the existing list services (DRY — no ad-hoc BC queries): `OVERDUE_INVOICE` (open sales invoices past `dueDate`, not PAID; WARNING, CRITICAL ≥30 days overdue; cap 10) and `LOW_STOCK` (items where `isLowStock`; WARNING; cap 10). Each kind cached 60s via `ICacheService`. **Role filtering mirrors the financial/warehouse module model:** financial roles (ADMIN/MANAGER/ACCOUNTING) see OVERDUE_INVOICE, warehouse-capable roles (ADMIN/MANAGER/WAREHOUSE) see LOW_STOCK; ADMIN/MANAGER see both; WAREHOUSE never sees financial overdue alerts. Endpoint reads roles from `ClaimsPrincipal` (same pattern as SearchEndpoints/GetMe); 502 `INTEGRATION_BC_UNAVAILABLE` on BC failure.
- `NotificationDto` (id, type, title, message, severity, link). **Cross-layer enums (SCREAMING_SNAKE wire value):** `type` = OVERDUE_INVOICE | LOW_STOCK; `severity` = INFO | WARNING | CRITICAL — frontend maps to i18n labels (`notifications.type.*` / `notifications.severity.*`); documented allowed values.
- Frontend `components/notification-bell.tsx` added to the protected-layout topbar (`app/(protected)/layout.tsx`) next to global search: bell icon button with unread-count badge, Bearer-token fetch, dropdown listing notifications (severity-colored dot, title, message, click → navigate to `link`), loading + empty ("No notifications") states, accessible (aria-label/haspopup/expanded, role=menu/menuitem), click-outside close. Pure helpers `notificationSeverityClass` + `unreadCount` extracted into `lib/notifications.ts` (unit-tested). i18n: `notifications` section (sr + en).
- **(C) Email service** — `IEmailService` (`Application/Interfaces/IEmailService.cs`) + dev no-op `LoggingEmailService` (`Infrastructure/Email/LoggingEmailService.cs`): logs recipient + subject only (NOT the body — may contain PII/secrets per logging rules) via the structured logger and does NOT send. Registered as `IEmailService` in Program.cs. Production wires a real MailKit SMTP impl via config (not added now); US-021 password reset depends on the interface.
- API doc: `docs/api/notifications.md` v1.0 (endpoint, RequireDashboard auth + role-based filtering table, success/empty examples, Type + Severity enum tables, 400/401/403/404/500/502; export = client-side utility note; email-service note).
- Tests: backend +13 (NotificationServiceTests ×11: financial role → overdue, warehouse role → low-stock, warehouse does NOT see overdue, financial does NOT see low-stock, ADMIN + MANAGER see both, severity/type wire values, overdue link → /sales/invoices/{id} + message, low-stock link → /items/{id} + WARNING, no-roles empty, per-kind cap ≤10; LoggingEmailServiceTests ×2: SendAsync does not throw + completes synchronously). Frontend +20 (export.test.ts ×13: toRowMatrix/extractHeaders/rowToCells + exportToExcel/exportToPdf smoke with stubbed download; notifications.test.ts ×8 wait — severity class incl. unknown-fallback, unreadCount incl. null; notification-bell.test.tsx ×4: renders + Bearer fetch, badge + dropdown items, navigate on click, empty state + no badge). `export.ts` + `notifications.ts` at 100% coverage; gate green.
- Combined suites: 350 backend + 156 frontend passing; lint clean; coverage gate green.
