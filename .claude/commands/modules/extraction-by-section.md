# Client Docs Extraction - By Document Section

**Purpose:** Methods for extracting requirements from specific document sections.

**Parent:** `process-client-docs-extraction.md`

---

## Vision and Goals

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

## Features and Requirements

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

## Technology Stack

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

## Timeline and Constraints

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

## Handling Ambiguities

### Flag Unclear Requirements

**When requirements are unclear:**
```markdown
## ⚠️ NEEDS CLARIFICATION

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

## Epic Organization

### Group Related Stories

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

[← Back to process-client-docs-extraction.md](process-client-docs-extraction.md)
