# Phase 3 — Advanced Features

**Goal:** Post-purchase workflows: order management for both customers and vendors, and inventory tracking.
**Total items:** 3
**Total points:** 21
**Status:** Todo

---

## Epic 4: Order Management

### US-013: View Orders (Customer)
**Priority:** P1
**Estimate:** 5 points
**Story:** As a customer, I want to view my order history.

**Acceptance Criteria:**
- [ ] List of all orders with status
- [ ] Order details (items, shipping, payment)
- [ ] Tracking info when available
- [ ] Reorder option
- [ ] Download invoice

**Dependencies:** US-011

---

### US-014: Manage Orders (Vendor)
**Priority:** P1
**Estimate:** 8 points
**Story:** As a vendor, I want to manage orders for my products.

**Acceptance Criteria:**
- [ ] View all orders with my products
- [ ] Update status (processing, shipped, delivered)
- [ ] Add tracking number
- [ ] Notifications for new orders
- [ ] Filter and search

**Dependencies:** US-011, US-004

---

## Epic 5: Inventory Management

### US-015: Track Inventory
**Priority:** P1
**Estimate:** 8 points
**Story:** As a vendor, I want to track inventory to avoid overselling.

**Acceptance Criteria:**
- [ ] Inventory decremented on purchase
- [ ] Low stock alerts
- [ ] Block purchase when out of stock
- [ ] Bulk inventory updates
- [ ] Inventory audit log

**Dependencies:** US-005, US-011

---

## Phase Summary

- **P1:** 3 items, 21 points

**Navigation:** [← Phase 2](phase-2-core.md) · [Index](README.md) · [Phase 4 →](phase-4-polish.md) · [Dashboard](../../output/progress/DASHBOARD.md)
