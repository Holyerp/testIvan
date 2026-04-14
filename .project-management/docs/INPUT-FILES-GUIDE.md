# Input Files Guide

**Version:** 3.0.0
**Purpose:** Detailed guide for filling project input files

---

## Overview

Input files define your project scope, features, tech stack, and constraints. Fill these before running `/init-project`.

**Location:** `.project-management/input/`

**Required files:**
1. `scope.md` - Project vision, goals, phases
2. `backlog.md` - Active features and user stories
3. `technologies.md` - Tech stack
4. `constraints.md` - Timeline, budget, team limits

**Optional files:**
5. `backlog-future.md` - Future requirements (v2.0, v3.0+)

---

## 1. scope.md

**Purpose:** Define what the project is and why it exists

### Key Sections

**Project Vision**
- The big picture - what are you building?
- Why does it matter?
- What problem does it solve?

**Target Audience**
- Who will use this product?
- What are their needs and pain points?
- User personas if applicable

**Core Objectives**
- What MUST be achieved?
- Top 3-5 critical objectives
- Measurable goals

**Success Criteria**
- How do we measure success?
- Quantifiable metrics (users, revenue, performance)
- Quality standards

**Out of Scope**
- What we're NOT building (explicitly state this)
- Features deferred to future versions
- Constraints on what's included

**Phases**
- Standard 4-phase structure:
  - Phase 1: Foundation (1-4 months)
  - Phase 2: Core Features (1-4 months)
  - Phase 3: Advanced Features (1-4 months)
  - Phase 4: Polish & Launch (1-4 months)

### Best Practices

✅ **Be specific and clear** - Vague vision = vague results
✅ **Focus on WHY** - Not just what you're building
✅ **Define success measurably** - "Fast" is not measurable, "<100ms API response" is
✅ **Explicitly state out of scope** - Prevents scope creep
✅ **Use realistic phase durations** - 1-4 months per phase, not 2 weeks

---

## 2. backlog.md

**Purpose:** List ALL active features, user stories, and requirements

### Key Sections

**Epics**
- Large feature groups
- Contains multiple related user stories
- Example: "User Authentication Epic"

**User Stories**
- Format: "As a [user], I want [feature], so that [benefit]"
- Clear, concise, user-focused
- One story = one deliverable feature

**Acceptance Criteria**
- Clear, testable conditions for completion
- Format: Given/When/Then or bullet points
- Must be verifiable

**Story Points**
- Fibonacci scale: 1, 2, 3, 5, 8, 13, 21
- Estimate effort, not time
- 21+ points? Break it down!

**Priorities**
- **P0** - Critical (must have)
- **P1** - High (should have)
- **P2** - Medium (nice to have)
- **P3** - Low (wishlist)

**Dependencies**
- What must be done first?
- Cross-story dependencies
- External dependencies (APIs, services)

### Story Point Reference

| Points | Effort | Duration (Approx) |
|--------|--------|-------------------|
| 1-2 | Trivial | Few hours |
| 3 | Small | Half day |
| 5 | Medium | 1-2 days |
| 8 | Large | 2-4 days |
| 13 | X-Large | 1 week |
| 21+ | Too big | Break it down! |

**Note:** In phase-based system (v3.0), story points help estimate phase duration (1-4 months per phase).

### Best Practices

✅ **Write clear user stories** - Follow "As a/I want/So that" format
✅ **Include acceptance criteria** - Every story needs testable criteria
✅ **Estimate realistically** - Don't underestimate complexity
✅ **Note all dependencies** - Helps with phase planning
✅ **Prioritize ruthlessly** - Not everything can be P0
✅ **One story = one feature** - Don't bundle multiple features

---

## 3. technologies.md

**Purpose:** Define the complete technology stack

### Key Sections

**Frontend**
- Framework (React, Vue, Angular)
- State management (Redux, Zustand, Jotai)
- Routing library
- UI libraries and components
- Build tools (Vite, Webpack)

**Backend**
- Runtime (Node.js, Python, Go)
- Framework (Express, FastAPI, Gin)
- Database (PostgreSQL, MongoDB, MySQL)
- ORM/Query builder (Prisma, TypeORM, SQLAlchemy)
- Authentication (JWT, OAuth, sessions)

**DevOps**
- Hosting (Vercel, AWS, Heroku)
- CI/CD (GitHub Actions, GitLab CI)
- Monitoring (Sentry, LogRocket)
- Analytics (Google Analytics, Mixpanel)

**Testing**
- Unit tests (Vitest, Jest, pytest)
- Integration tests (Supertest, pytest)
- E2E tests (Playwright, Cypress)
- Coverage goals (80%+ recommended)

**Third-party Services**
- Payment providers (Stripe, PayPal)
- Email services (SendGrid, Mailgun)
- Storage (S3, Cloudinary)
- APIs and integrations

### Best Practices

✅ **Be specific about versions** - "React 19.0.0" not just "React"
✅ **Explain technology choices** - Why this stack?
✅ **List ALL dependencies** - Don't assume anything
✅ **Include dev tools** - Linters, formatters, etc.
✅ **Specify performance targets** - Response times, load times
✅ **Document third-party services** - APIs, credentials needed

---

## 4. constraints.md

**Purpose:** Define limitations and boundaries

### Key Sections

**Timeline**
- Project deadline
- Phase milestones (1-4 months each)
- Key delivery dates
- Launch date

**Budget**
- Development costs
- Infrastructure costs
- Third-party service costs
- Licensing fees

**Team**
- Team size
- Developer availability (full-time/part-time)
- Skill levels and expertise
- Working hours and time zones

**Technical Constraints**
- Infrastructure limits
- Technology restrictions
- Performance requirements
- Security requirements
- Browser/device support

**Compliance**
- Legal requirements (GDPR, CCPA)
- Industry regulations
- Security standards (SOC 2, ISO)
- Accessibility standards (WCAG)

**Scope Constraints**
- Must-have vs nice-to-have features
- Features explicitly out of scope
- Resource limits

### Best Practices

✅ **Be realistic and honest** - Over-promising leads to failure
✅ **Document ALL constraints** - Surprises derail projects
✅ **Identify risks early** - Budget, timeline, technical risks
✅ **Plan for contingencies** - Buffer time and budget
✅ **Update as constraints change** - Living document

---

## 5. backlog-future.md (Optional)

**Purpose:** Track future requirements (Version 2.0, 3.0, beyond)

### When to Use

- Planning post-launch features
- Collecting ideas for "later"
- Client requests not needed in Phases 1-4
- Backlog grooming for future versions

### Structure

**Organized by version:**
- **Version 2.0** - Post-launch enhancements (1-3 months after)
- **Version 3.0** - Major future features (6-12+ months)
- **Unversioned** - Ideas and experiments (no timeline)

### Key Differences from backlog.md

| Active Backlog | Future Backlog |
|----------------|----------------|
| Phases 1-4 | Version 2.0+ |
| Status: Todo/In Progress | Status: Future |
| Assigned to phases | NOT assigned |
| Immediate work | Later work |

### Promoting to Active

When ready to implement future requirement:
```bash
/promote-requirement US-XXX --to-phase N
```

This moves requirement from `backlog-future.md` to `backlog.md` and assigns to phase.

---

## 🎨 Starting from Client Documents

Instead of manually filling input files, you can process client documents:

1. **Add client documents** to `.project-management/client-input/`
   - PDFs, Word docs, text files, images (mockups/wireframes)

2. **Run command:**
   ```bash
   /process-client-docs
   ```

3. **Claude generates** all input files automatically:
   - Extracts requirements → `scope.md`
   - Creates user stories → `backlog.md`
   - Suggests tech stack → `technologies.md`
   - Identifies constraints → `constraints.md`

4. **Review and refine** generated files
5. **Run `/init-project`** to start project

**Full guide:** [client-input/README.md](../client-input/README.md)

---

## 🔄 Updating Input Files

Input files are **living documents**. Update them when:

- Client requests new features → Update `backlog.md`
- Scope changes → Update `scope.md`
- Tech stack changes → Update `technologies.md`
- Timeline shifts → Update `constraints.md`
- Planning future versions → Update `backlog-future.md`

**After updating, regenerate docs:**
```bash
/generate-docs
```

---

[← Back to README](../README.md)
