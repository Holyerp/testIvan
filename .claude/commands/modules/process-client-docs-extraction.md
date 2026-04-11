# Process Client Docs - Extraction Module

**Purpose:** Methods for extracting structured requirements from client documents.

**Parent Command:** `/process-client-docs`

---

## Requirement Extraction Principles

### Read Between the Lines

**Extract both explicit and implicit requirements:**

**Explicit (stated directly):**
```
Document: "Users must be able to register with email and password"
Requirement: User registration with email/password
```

**Implicit (implied but not stated):**
```
Document: "Users must be able to register with email and password"
Implicit requirements:
- Email validation
- Password strength requirements
- Email verification
- Account activation
- Login functionality
- Password reset functionality
```

---

### Organize by User Value

**Structure features as Epics and Stories:**

**From document:**
```
"The platform should allow users to manage their profiles,
 browse products, add items to cart, and complete purchases."
```

**Extracted structure:**
```
EPIC-1: User Management
- US-001: User Registration
- US-002: User Login
- US-003: Profile Management
- US-004: Password Reset

EPIC-2: Product Browsing
- US-010: Product Catalog
- US-011: Product Search
- US-012: Product Details

EPIC-3: Shopping Experience
- US-020: Shopping Cart
- US-021: Checkout Flow
- US-022: Payment Processing
```

---

## Extraction by Document Section

### Vision and Goals

**Extract from:**
- Executive summary
- Project overview
- Business objectives

**Generate for scope.md:**
```markdown
## Project Vision
Create an online marketplace connecting local artisans with customers

## Core Objectives
1. Enable artisans to showcase products online
2. Provide seamless purchasing experience
3. Implement secure payment processing
4. Build analytics dashboard for sellers
```

---

### Features and Requirements

**Extract from:**
- Feature lists
- Functional requirements
- User stories (if provided)
- Wireframe labels

**Convert to epics and stories:**

**Document text:**
```
"Users need authentication, profile management,
 and the ability to browse and purchase products."
```

**Extracted stories:**
```
## EPIC-1: Authentication & User Management

### US-001: User Registration
**As a** new user
**I want** to create an account with email and password
**So that** I can access the platform

**Acceptance Criteria:**
- Email validation
- Password strength (8+ chars, 1 uppercase, 1 number)
- Email verification link sent
- Account created only after email verification

**Priority:** P0
**Story Points:** 5
```

---

### Technology Stack

**Extract from:**
- Technical requirements
- Infrastructure mentions
- Constraints
- "Must use" statements

**Generate for technologies.md:**

**Document mentions:**
```
"Frontend should use React"
"Database: PostgreSQL preferred"
"Must integrate with Stripe for payments"
"Mobile responsive design required"
```

**Extracted:**
```markdown
## Frontend
- **Framework:** React (client requirement)
- **Responsive:** Mobile-first design (required)

## Backend
- **Database:** PostgreSQL (preferred by client)
- **API:** RESTful

## Third-Party Integrations
- **Payment:** Stripe (required)

## Requirements
- Mobile responsive design
- Browser support: Chrome, Firefox, Safari (latest 2 versions)
```

---

### Timeline and Constraints

**Extract from:**
- Project timeline
- Deadlines
- Budget mentions
- Team size
- Milestones

**Generate for constraints.md:**

**Document mentions:**
```
"Launch date: June 30, 2026"
"Budget: $50,000"
"Team: 1 developer + AI tools"
"Must comply with GDPR"
```

**Extracted:**
```markdown
## Timeline Constraints
- **Project Start:** January 15, 2026
- **Target Launch:** June 30, 2026
- **Duration:** 5.5 months
- **Milestones:**
  - Phase 1 (Foundation): End of February
  - Phase 2 (Core Features): End of April
  - Phase 3 (Polish): End of June

## Budget Constraints
- **Total Budget:** $50,000
- **Development:** 1 developer + AI tools

## Compliance Requirements
- **GDPR:** Must comply (EU users)
- **PCI DSS:** Required for payment processing
```

---

## Priority Assignment

### Determine Priority Levels

**Based on client emphasis:**

**P0 (Must Have - Launch Blocker):**
- Explicitly stated as "must have"
- Core functionality
- Mentioned multiple times
- Required for launch

**P1 (Should Have - Important):**
- Stated as "should have"
- Important but not critical
- Can be added post-launch if needed

**P2 (Nice to Have):**
- Stated as "could have"
- Optional features
- Future enhancements

**P3 (Future):**
- Ideas mentioned
- Not critical
- Post-launch consideration

**Extraction examples:**
```
"Users MUST be able to register" ’ P0
"Users should receive email notifications" ’ P1
"Users could have a wishlist feature" ’ P2
"In the future, we might add social sharing" ’ P3
```

---

## Story Point Estimation

### Estimate Complexity

**During extraction, assign initial estimates:**

**Complexity factors:**
- Technical difficulty
- Dependencies
- Uncertainty
- Testing requirements

**Estimation scale (Fibonacci):**
- **1 point:** Trivial (simple UI change)
- **2 points:** Simple (basic CRUD)
- **3 points:** Medium (with validation)
- **5 points:** Complex (integration, auth)
- **8 points:** Very complex (payment, search)
- **13 points:** Extremely complex (split into smaller stories)

**Examples:**
```
US-001: User Registration ’ 5 points (auth, validation, email)
US-010: View Product List ’ 2 points (simple read)
US-020: Shopping Cart ’ 5 points (state management, persistence)
US-022: Payment Processing ’ 8 points (third-party, security)
```

---

## Handling Ambiguities

### Flag Unclear Requirements

**When requirements are unclear:**
```markdown
##   NEEDS CLARIFICATION

1. **Payment Gateway**
   - Document mentions "payment processing" but doesn't specify gateway
   - **Assumption:** Using Stripe (industry standard)
   - **Question for client:** Confirm Stripe or specify alternative?

2. **Admin Features**
   - Document mentions "admin dashboard" without details
   - **Assumption:** Basic product/user management
   - **Question for client:** What specific admin features are required?

3. **Mobile App**
   - Document briefly mentions "mobile support"
   - **Assumption:** Responsive web app (not native mobile)
   - **Question for client:** Native app required or responsive web sufficient?
```

---

## Extraction Patterns

### User-Centered Extraction

**Convert features to user stories:**

**Document:** "Product search functionality"

**Extracted:**
```
US-045: Product Search

**As a** customer
**I want to** search for products by keyword
**So that** I can quickly find what I'm looking for

**Acceptance Criteria:**
- Search bar visible on all pages
- Real-time search results
- Search by product name, description, category
- Display relevant results sorted by relevance
- Handle no results gracefully

**Priority:** P0
**Story Points:** 5
**Dependencies:** US-010 (Product Catalog)
```

---

### Non-Functional Requirements

**Extract implicit quality requirements:**

**Document mentions:**
```
"Fast loading times"
"Secure platform"
"Easy to use"
```

**Extracted as:**
```
## Non-Functional Requirements

### Performance
- Page load time < 2 seconds
- API response time < 500ms
- Support 1000 concurrent users

### Security
- HTTPS only
- Password hashing (bcrypt)
- SQL injection prevention
- XSS protection
- CSRF tokens

### Usability
- Mobile responsive
- Accessible (WCAG 2.1 AA)
- Intuitive navigation
- Clear error messages
```

---

## Cross-Document Synthesis

### Combine Information

**From multiple documents:**

**Document 1 (brief.pdf):**
"Users should be able to search products"

**Document 2 (wireframe.png):**
Shows search bar with filter dropdowns

**Document 3 (requirements.docx):**
"Search with category and price filters"

**Combined extraction:**
```
US-045: Product Search with Filters

**As a** customer
**I want to** search products and filter by category and price
**So that** I can narrow down my search results

**Acceptance Criteria:**
- Search bar in header (as per wireframe)
- Filter by category dropdown
- Filter by price range slider
- Combine search + filters
- Update results in real-time
- Show result count

**Source:**
- brief.pdf (p. 4)
- wireframe.png
- requirements.docx (p. 7)

**Priority:** P0
**Story Points:** 8
```

---

## Epic Organization

### Group Related Stories

**Organize stories into epics:**

**Extracted stories:**
- User registration
- User login
- Password reset
- Profile management
- Email verification

**Grouped into epic:**
```
## EPIC-1: User Authentication & Management (20 points)

**Goal:** Enable users to create accounts and manage their profiles

**Stories:**
- US-001: User Registration (5 points)
- US-002: User Login (3 points)
- US-003: Password Reset (3 points)
- US-004: Email Verification (2 points)
- US-005: Profile Management (5 points)
- US-006: Account Settings (2 points)

**Dependencies:** None (foundational epic)
**Priority:** P0
```

---

## Acceptance Criteria Generation

### Create Testable Criteria

**From vague requirement:**
```
Document: "Users can upload profile pictures"
```

**Generated acceptance criteria:**
```
US-007: Profile Picture Upload

**Acceptance Criteria:**
- [ ] User can click "Upload Photo" button
- [ ] File picker opens for image selection
- [ ] Accept formats: JPG, PNG, GIF
- [ ] Max file size: 5MB
- [ ] Image preview before upload
- [ ] Crop/resize option (square, 200x200px)
- [ ] Upload progress indicator
- [ ] Success message after upload
- [ ] Picture displayed immediately
- [ ] Validation errors shown clearly
```

---

## Quality Checks During Extraction

**Ensure:**
- [ ] All features from documents captured
- [ ] Organized into logical epics
- [ ] User stories follow template (As a... I want... So that...)
- [ ] Acceptance criteria are specific and testable
- [ ] Priorities assigned based on client emphasis
- [ ] Story points estimated
- [ ] Dependencies identified
- [ ] Ambiguities flagged for clarification
- [ ] Non-functional requirements included
- [ ] All output in English (translated if needed)

---

## Output File Generation

### Generate scope.md

**From extracted vision, goals, objectives**

### Generate backlog.md

**From extracted epics and stories**

### Generate technologies.md

**From extracted tech stack and constraints**

### Generate constraints.md

**From extracted timeline, budget, compliance**

---

**Related:**
- Parent: `.claude/commands/process-client-docs.md`
- Sibling: `process-client-docs-reading.md`
- Templates: `.project-management/templates/`
- Rules: `.CLAUDE.MD` (English-only policy)
