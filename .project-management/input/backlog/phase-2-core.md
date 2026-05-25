# Phase 2 — Core Documents

**Goal:** Complete coverage of Sales and Purchase document types, Vendors, and universal search/filtering.
**Total items:** 9 (1 technical task + 8 user stories)
**Total points:** 52
**Status:** Todo

---

## Technical Tasks

### T-003: Document List Infrastructure
**Priority:** P0
**Estimate:** 5 points
**Description:** Shared list + detail infrastructure: reusable table component, pagination hook, filter panel, BC entity mapper pattern.

**Tasks:**
- [ ] Build `EntityTable` component (sortable columns, pagination, search)
- [ ] Build reusable `FilterPanel` component (date range, status dropdown, amount range)
- [ ] Create `usePaginatedQuery` hook for BC-backed lists
- [ ] Design BC entity mapper interfaces (BC OData response → Pinoles UI model)
- [ ] Write unit tests for mapper functions

---

## Epic 2: Sales Documents

### US-006: Sales Invoices — List View
**Priority:** P0
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view all open sales invoices so that I can track outstanding receivables.

**Type:** Frontend (Web)
**Screen:** SalesInvoiceListScreen

**Acceptance Criteria:**
- [ ] Paginated table: invoice number, customer name, date, due date, amount, status (Open/Partial/Paid)
- [ ] Filter by: status, date range, customer
- [ ] Sort by: date, due date, amount
- [ ] Overdue invoices highlighted in amber/red
- [ ] Navigate to SalesInvoiceDetailScreen on row click
- [ ] Tabs for: Open Invoices / Posted Invoices / Credit Memos

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/sales/invoices | Open sales invoices (paginated + filtered) |
| GET | /api/v1/sales/posted-invoices | Posted sales invoices |
| GET | /api/v1/sales/credit-memos | Sales credit memos |

**Dependencies:** T-003, US-002

---

### US-007: Sales Invoice — Detail View
**Priority:** P0
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view the full details of a sales invoice so that I can verify amounts and line items.

**Type:** Frontend (Web)
**Screen:** SalesInvoiceDetailScreen

**Acceptance Criteria:**
- [ ] Invoice header: number, customer, bill-to address, date, due date, payment terms
- [ ] Line items table: description, quantity, unit price, VAT %, line total
- [ ] Invoice totals: subtotal, VAT amount, total
- [ ] Status badge (Open / Partially Paid / Paid / Overdue)
- [ ] Back navigation to invoice list
- [ ] 404 if invoice ID not found

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/sales/invoices/{id} | Sales invoice detail |
| GET | /api/v1/sales/posted-invoices/{id} | Posted invoice detail |

**Dependencies:** US-006

---

### US-008: Sales Credit Memos — List & Detail
**Priority:** P1
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view sales credit memos so that I can track credit notes issued to customers.

**Type:** Frontend (Web)
**Screen:** SalesCreditMemoListScreen

**Acceptance Criteria:**
- [ ] List with same columns/filters as sales invoice list (adapted for credit memos)
- [ ] Detail view with header + line items + totals
- [ ] Status indicator (Open / Posted)
- [ ] Posted credit memos shown in separate tab on SalesInvoiceListScreen

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/sales/credit-memos | Sales credit memo list |
| GET | /api/v1/sales/credit-memos/{id} | Credit memo detail |
| GET | /api/v1/sales/posted-credit-memos | Posted credit memos |

**Dependencies:** US-006

---

## Epic 3: Purchase Documents

### US-009: Purchase Invoices — List View
**Priority:** P0
**Estimate:** 5 points
**Story:** As an accounting employee, I want to see all purchase invoices so that I can track payables to vendors.

**Type:** Frontend (Web)
**Screen:** PurchaseInvoiceListScreen

**Acceptance Criteria:**
- [ ] Paginated table: invoice number, vendor name, date, due date, amount, status
- [ ] Filter: status, date range, vendor
- [ ] Tabs: Open / Posted / Credit Memos
- [ ] Overdue invoices highlighted
- [ ] Row click → PurchaseInvoiceDetailScreen

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/purchase/invoices | Purchase invoices |
| GET | /api/v1/purchase/posted-invoices | Posted purchase invoices |
| GET | /api/v1/purchase/credit-memos | Purchase credit memos |

**Dependencies:** T-003, US-002

---

### US-010: Purchase Invoice — Detail View
**Priority:** P0
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view the full details of a purchase invoice so that I can verify vendor billing.

**Type:** Frontend (Web)
**Screen:** PurchaseInvoiceDetailScreen

**Acceptance Criteria:**
- [ ] Invoice header: number, vendor, date, due date, payment terms, our reference
- [ ] Line items table: description, quantity, unit price, VAT %, line total
- [ ] Invoice totals: subtotal, VAT, grand total
- [ ] Status badge
- [ ] 404 if not found

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/purchase/invoices/{id} | Purchase invoice detail |
| GET | /api/v1/purchase/posted-invoices/{id} | Posted invoice detail |

**Dependencies:** US-009

---

## Epic 4: Vendors

### US-011: Vendors — List View
**Priority:** P1
**Estimate:** 5 points
**Story:** As an accounting employee, I want to browse the vendor list so that I can quickly find a vendor.

**Type:** Frontend (Web)
**Screen:** VendorListScreen

**Acceptance Criteria:**
- [ ] Paginated table: vendor name, number, city, balance, phone
- [ ] Search by name or number
- [ ] Sort by name, balance
- [ ] Row click → VendorDetailScreen

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/vendors | Paginated vendor list |

**Dependencies:** T-003, US-002

---

### US-012: Vendors — Detail View
**Priority:** P1
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view a vendor's full profile so that I can see their contact info and purchase history.

**Type:** Frontend (Web)
**Screen:** VendorDetailScreen

**Acceptance Criteria:**
- [ ] Vendor header: name, number, address, phone, email, VAT number
- [ ] Financial summary: balance, payment terms
- [ ] Purchase history: last 20 posted invoices
- [ ] Breadcrumb back to vendor list
- [ ] 404 if not found

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/vendors/{id} | Vendor detail |
| GET | /api/v1/vendors/{id}/invoices | Vendor purchase history |

**Dependencies:** US-011

---

### US-013: Universal Search
**Priority:** P1
**Estimate:** 8 points
**Story:** As any employee, I want to search across customers, vendors, and invoices from a single search bar so that I can find records quickly.

**Type:** Frontend (Web)
**Screen:** SearchResultsScreen

**Acceptance Criteria:**
- [ ] Global search bar in topbar (Cmd/Ctrl+K shortcut)
- [ ] Returns grouped results: Customers, Vendors, Sales Invoices, Purchase Invoices
- [ ] Max 5 results per group, with "View all" link
- [ ] Debounced input (300ms), min 2 characters
- [ ] Keyboard navigation (arrows + Enter to open)
- [ ] Result click navigates to respective detail screen

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/search?q={query}&limit={n} | Cross-entity search |

**Dependencies:** T-003

---

## Phase Summary

- **P0:** 5 items, 25 points (incl. T-003)
- **P1:** 4 items, 23 points
- **P2:** 0

**Navigation:** [← Phase 1](phase-1-foundation.md) · [Phase 3 →](phase-3-advanced.md) · [Dashboard](../../output/progress/DASHBOARD.md)
