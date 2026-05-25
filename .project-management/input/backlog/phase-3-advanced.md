# Phase 3 — Extended Modules

**Goal:** Advance invoices, credit documents, warehouse/inventory, and full role-based module enforcement.
**Total items:** 7 (1 technical task + 6 user stories)
**Total points:** 42
**Status:** Todo

---

## Technical Tasks

### T-004: Role Guard & Module Access Infrastructure
**Priority:** P1
**Estimate:** 5 points
**Description:** Implement full role-based module locking in both frontend (route guards) and backend (endpoint-level authorization).

**Tasks:**
- [ ] Define `MODULE_ACCESS` map: which roles can access which modules
- [ ] Frontend: `<RoleGuard>` component that renders 403 page for unauthorized modules
- [ ] Backend: `[RequireRole(...)]` attribute / middleware applied to all controller groups
- [ ] Admin: module access configuration UI (Phase 4)
- [ ] Write tests: 403 for each locked module per role

---

## Epic 5: Advance Invoices

### US-014: Sales Advance Invoices — List & Detail
**Priority:** P1
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view advance sales invoices so that I can track advance payment requests sent to customers.

**Type:** Frontend (Web)
**Screen:** SalesAdvanceInvoiceListScreen

**Acceptance Criteria:**
- [ ] List: advance invoice number, customer, date, amount, status (Open/Partially Paid/Paid)
- [ ] Detail: full header + line items + payment tracking section
- [ ] Filter: status, date range, customer
- [ ] Status badge clearly distinguishes advance from regular invoice

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/sales/advance-invoices | Sales advance invoice list |
| GET | /api/v1/sales/advance-invoices/{id} | Advance invoice detail |

**Dependencies:** T-003, T-004

---

### US-015: Purchase Advance Invoices — List & Detail
**Priority:** P1
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view purchase advance invoices so that I can track advance payments made to vendors.

**Type:** Frontend (Web)
**Screen:** PurchaseAdvanceInvoiceListScreen

**Acceptance Criteria:**
- [ ] List: same columns as sales advance invoices, vendor-side data
- [ ] Detail: full header + line items + payment tracking
- [ ] Filter: status, date range, vendor

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/purchase/advance-invoices | Purchase advance invoices |
| GET | /api/v1/purchase/advance-invoices/{id} | Advance invoice detail |

**Dependencies:** US-014

---

## Epic 6: Credit Documents

### US-016: Credit Notes & Storno Invoices — List & Detail
**Priority:** P2
**Estimate:** 5 points
**Story:** As an accounting employee, I want to view credit notes and storno invoices so that I can track corrections and cancellations.

**Type:** Frontend (Web)
**Screen:** CreditNoteListScreen

**Acceptance Criteria:**
- [ ] Unified list: credit memos + debit memos + storno invoices
- [ ] Type column distinguishes document types
- [ ] Detail: references the original invoice number
- [ ] Filter: type, date range, customer/vendor

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/credit-documents | Credit notes, debit memos, storno |
| GET | /api/v1/credit-documents/{id} | Credit document detail |

**Dependencies:** T-003

---

## Epic 7: Warehouse & Inventory

### US-017: Items — List View
**Priority:** P1
**Estimate:** 5 points
**Story:** As a warehouse employee, I want to browse the items/products list with current stock levels so that I can monitor inventory.

**Type:** Frontend (Web)
**Screen:** ItemListScreen

**Acceptance Criteria:**
- [ ] Paginated table: item number, description, unit of measure, quantity on hand, unit cost
- [ ] Filter: category, location
- [ ] Sort: name, quantity, unit cost
- [ ] Low-stock warning badge (when quantity < minimum stock)
- [ ] Row click → ItemDetailScreen

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/items | Item list with stock levels |

**Dependencies:** T-003, T-004

---

### US-018: Items — Detail View
**Priority:** P1
**Estimate:** 5 points
**Story:** As a warehouse employee, I want to view an item's full detail so that I can see stock by location and recent movements.

**Type:** Frontend (Web)
**Screen:** ItemDetailScreen

**Acceptance Criteria:**
- [ ] Item header: number, description, category, unit of measure, unit cost, unit price
- [ ] Stock by location table: location name, quantity on hand, quantity reserved
- [ ] Recent item ledger entries (last 20 movements): date, type, quantity, remaining
- [ ] Low-stock alert if below minimum
- [ ] 404 if item not found

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/items/{id} | Item detail |
| GET | /api/v1/items/{id}/ledger-entries | Stock movements |

**Dependencies:** US-017

---

### US-019: Inventory — Stock Overview
**Priority:** P2
**Estimate:** 5 points
**Story:** As a warehouse manager, I want a summary view of inventory by location so that I can see total stock values per warehouse.

**Type:** Frontend (Web)
**Screen:** InventoryOverviewScreen

**Acceptance Criteria:**
- [ ] Summary cards: total items, total stock value, items below minimum stock count
- [ ] Stock by location breakdown table
- [ ] Low-stock items list (sorted by quantity ascending)
- [ ] Filter: location, category

**API Endpoints Used:**
| Method | Path | Purpose |
|--------|------|---------|
| GET | /api/v1/inventory/summary | Inventory KPI summary |
| GET | /api/v1/inventory/locations | Stock by location |
| GET | /api/v1/inventory/low-stock | Items below minimum threshold |

**Dependencies:** US-017

---

## Phase Summary

- **P1:** 5 items, 25 points (incl. T-004)
- **P2:** 2 items, 10 points

**Navigation:** [← Phase 2](phase-2-core.md) · [Phase 4 →](phase-4-polish.md) · [Dashboard](../../output/progress/DASHBOARD.md)
