# Phase 2 — Core Features

**Goal:** Deliver the primary shopping experience — product management, browse, cart, and checkout.
**Total items:** 9
**Total points:** 56
**Status:** Todo

---

## Epic 1 (cont.): Auth polish

### US-003: Password Reset
**Priority:** P1
**Estimate:** 3 points
**Story:** As a user, I want to reset my password if I forget it.

**Acceptance Criteria:**
- [ ] Request reset via email
- [ ] Reset link expires after 1 hour
- [ ] Set new password
- [ ] Confirmation email after reset

**Dependencies:** US-001

---

## Epic 2: Product Management

### US-005: Create Product Listing
**Priority:** P0
**Estimate:** 8 points
**Story:** As a vendor, I want to create product listings so customers can buy my products.

**Acceptance Criteria:**
- [ ] Title, description, price fields
- [ ] Up to 5 product images
- [ ] Categories and tags
- [ ] Inventory quantity
- [ ] Draft / publish states

**Dependencies:** US-001, US-004

---

### US-006: Edit Product Listing
**Priority:** P0
**Estimate:** 5 points
**Story:** As a vendor, I want to edit listings to keep information current.

**Acceptance Criteria:**
- [ ] Update all product fields
- [ ] Immediate save
- [ ] Audit log of changes
- [ ] Add/remove images

**Dependencies:** US-005

---

### US-007: Delete Product Listing
**Priority:** P1
**Estimate:** 3 points
**Story:** As a vendor, I want to delete products I no longer sell.

**Acceptance Criteria:**
- [ ] Soft delete (archived)
- [ ] Confirmation dialog
- [ ] Block delete if active orders
- [ ] Restore within 30 days

**Dependencies:** US-005

---

### US-008: Product Search
**Priority:** P0
**Estimate:** 8 points
**Story:** As a customer, I want to search for products.

**Acceptance Criteria:**
- [ ] Full-text on title + description
- [ ] Sorted by relevance
- [ ] Filter by category, price, vendor
- [ ] Pagination
- [ ] Autocomplete

**Dependencies:** US-005

---

## Epic 3: Cart & Checkout

### US-009: Add to Cart
**Priority:** P0
**Estimate:** 5 points
**Story:** As a customer, I want a cart so I can buy multiple items.

**Acceptance Criteria:**
- [ ] Add with quantity
- [ ] Persists across sessions
- [ ] Shows total price
- [ ] Update quantity from cart
- [ ] Remove items

**Dependencies:** US-005

---

### US-010: Checkout Process
**Priority:** P0
**Estimate:** 8 points
**Story:** As a customer, I want to check out and pay.

**Acceptance Criteria:**
- [ ] Cart review
- [ ] Shipping address
- [ ] Billing address (or same)
- [ ] Shipping method
- [ ] Order summary
- [ ] Proceed to payment

**Dependencies:** US-009

---

### US-011: Payment Processing
**Priority:** P0
**Estimate:** 13 points
**Story:** As a customer, I want to pay securely.

**Acceptance Criteria:**
- [ ] Stripe/PayPal integration
- [ ] Secure card entry (no plaintext)
- [ ] Payment confirmation
- [ ] Order created on success
- [ ] Error handling for failures

**Dependencies:** US-010

---

### US-012: Order Confirmation
**Priority:** P0
**Estimate:** 3 points
**Story:** As a customer, I want order confirmation.

**Acceptance Criteria:**
- [ ] Confirmation email with details
- [ ] Order number generated
- [ ] Receipt/invoice available
- [ ] Redirect to success page

**Dependencies:** US-011

---

## Phase Summary

- **P0:** 8 items, 53 points
- **P1:** 1 item, 3 points

**Navigation:** [← Phase 1](phase-1-foundation.md) · [Index](README.md) · [Phase 3 →](phase-3-advanced.md) · [Dashboard](../../output/progress/DASHBOARD.md)
