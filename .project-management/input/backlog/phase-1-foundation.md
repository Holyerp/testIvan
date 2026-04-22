# Phase 1 — Foundation

**Goal:** Establish project infrastructure, database, API skeleton, and authentication core.
**Total items:** 6 (3 technical + 3 user stories)
**Total points:** 37
**Status:** Todo

---

## Technical Tasks (Infrastructure)

### T-001: Setup Project Infrastructure
**Priority:** P0
**Estimate:** 5 points
**Description:** Initialize Node.js project, set up React with React Router, configure build tools.

**Tasks:**
- [ ] Initialize npm project
- [ ] Setup React with Vite
- [ ] Configure React Router v7
- [ ] Setup ESLint and Prettier
- [ ] Configure TypeScript

---

### T-002: Database Schema Design
**Priority:** P0
**Estimate:** 8 points
**Description:** Design and implement database schema (Prisma, PostgreSQL).

**Tasks:**
- [ ] Design ER diagram
- [ ] Create Prisma migrations
- [ ] Implement User, Product, Order, Review models
- [ ] Setup relationships and indexes
- [ ] Seed data for development

---

### T-003: API Architecture
**Priority:** P0
**Estimate:** 8 points
**Description:** Set up RESTful API foundation.

**Tasks:**
- [ ] Define API routes and endpoints
- [ ] Implement middleware (auth, validation, error handling)
- [ ] Setup request/response schemas
- [ ] API documentation (OpenAPI)
- [ ] Rate limiting

---

## Epic 1: User Authentication & Authorization (core)

### US-001: User Registration
**Priority:** P0
**Estimate:** 5 points
**Story:** As a new user, I want to register an account so that I can access the platform.

**Acceptance Criteria:**
- [ ] Email + password registration
- [ ] Email validation
- [ ] Password policy: min 8 chars, mixed case, number
- [ ] Confirmation email sent
- [ ] Email verification link

**Dependencies:** T-002, T-003

---

### US-002: User Login
**Priority:** P0
**Estimate:** 3 points
**Story:** As a registered user, I want to log in so that I can access my account.

**Acceptance Criteria:**
- [ ] Email + password login
- [ ] Invalid credentials show error
- [ ] "Remember me" option
- [ ] 24-hour inactivity timeout
- [ ] Rate-limit failed attempts

**Dependencies:** US-001

---

### US-004: Role-Based Access Control
**Priority:** P0
**Estimate:** 8 points
**Story:** As a platform, I need distinct roles (Customer, Vendor, Admin) with different permissions.

**Acceptance Criteria:**
- [ ] Customer: browse + purchase
- [ ] Vendor: manage products + orders
- [ ] Admin: full access
- [ ] Backend enforces permissions
- [ ] UI adapts to role

**Dependencies:** US-001

---

## Phase Summary

- **P0:** 6 items, 37 points
- **P1:** 0
- **P2:** 0

**Navigation:** [← Index](README.md) · [Phase 2 →](phase-2-core.md) · [Dashboard](../../output/progress/DASHBOARD.md)
