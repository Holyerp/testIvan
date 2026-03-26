# Claude Project Management System - User Guide

**Complete guide to autonomous project planning and AI-assisted development.**

---

## Table of Contents

1. [Quick Start (5 Minutes)](#quick-start-5-minutes)
2. [What This System Does](#what-this-system-does)
3. [Getting Started](#getting-started)
4. [Workflows](#workflows)
5. [Commands Reference](#commands-reference)
6. [Customization Guide](#customization-guide)
7. [How Claude Uses This](#how-claude-uses-this)
8. [FAQ & Troubleshooting](#faq--troubleshooting)
9. [Best Practices](#best-practices)

---

## Quick Start (5 Minutes)

### Choose Your Path

**PATH A: I Have Client Documents** (PDF, Word, mockups)
```bash
# 1. Copy documents to client-input folder
cp client-brief.pdf .project-management/client-input/

# 2. Process documents (Claude extracts requirements)
/process-client-docs

# 3. Review generated files
ls .project-management/input/

# 4. Initialize project
/init-project
```

**Result:** Complete project documentation generated in 3 minutes! ✅

---

**PATH B: I'll Fill Forms Manually**
```bash
# 1. Fill input files
# Edit these 4 files:
.project-management/input/scope.md
.project-management/input/backlog.md
.project-management/input/technologies.md
.project-management/input/constraints.md

# 2. Initialize project
/init-project
```

**Result:** Project documentation generated from your input! ✅

---

### What Just Happened?

```
Input (you provide)          Claude Processing          Output (generated)
─────────────────           ──────────────────         ──────────────────
scope.md                →   Reads & analyzes      →   PRD
backlog.md              →   Generates docs        →   Technical Spec
technologies.md         →   Creates sprints       →   Architecture Doc
constraints.md          →   Plans timeline        →   Sprint Plans
```

---

### Next Steps

1. **Review Generated Docs:** Check `output/docs/` folder
2. **Start First Sprint:** Run `/plan-sprint 1`
3. **Begin Development:** Pick a story, implement, update progress
4. **Track Progress:** Run `/project-status` anytime

💡 **Pro Tip:** Read `.project-management/INTEGRATION-GUIDE.md` to understand how everything works together.

---

## What This System Does

### The Problem It Solves

**Without this system:**
- ❌ Manual planning takes hours
- ❌ Requirements scattered across emails/docs
- ❌ Inconsistent documentation
- ❌ Progress tracking is manual and tedious
- ❌ Coding standards vary between developers
- ❌ Onboarding new team members takes days

**With this system:**
- ✅ Automated planning in minutes
- ✅ Requirements extracted from client docs automatically
- ✅ Professional documentation generated
- ✅ Progress tracked automatically
- ✅ Consistent coding standards enforced
- ✅ New developers productive in hours

---

### Core Capabilities

#### 1. **Autonomous Project Planning**
Claude reads your requirements and automatically generates:
- Product Requirements Document (PRD)
- Technical Specification
- Architecture Documentation
- Sprint Plans
- Progress Trackers

#### 2. **Client Document Processing** 🔥
Upload client documents (PDF, Word, images) and Claude:
- Extracts features and requirements
- Creates user stories with priorities
- Generates acceptance criteria
- Estimates story points
- Identifies gaps and asks questions

#### 3. **Sprint-Based Development**
Structured workflow with:
- 2-week sprint planning
- Velocity tracking
- Capacity management
- Progress reporting
- Blocker identification

#### 4. **Comprehensive Coding Standards**
Enforces best practices:
- SOLID & DRY principles
- API testing (all 6 status codes)
- Database migrations workflow
- Git conventions (NO AI credits)
- Security requirements

#### 5. **Progress Tracking**
Real-time visibility:
- Phase/Epic/Task hierarchy
- Test count tracking
- Completion percentages
- Velocity trends
- Blocker logging

---

### How It Works (30,000-foot View)

```
┌─────────────────────────────────────────────────────────┐
│                    INPUT LAYER                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │
│  │ Client Docs  │  │  Scope/      │  │  Project     │ │
│  │ (PDF, Word)  │  │  Backlog     │  │  Rules       │ │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘ │
└─────────┼──────────────────┼──────────────────┼─────────┘
          │                  │                  │
          ▼                  ▼                  ▼
┌─────────────────────────────────────────────────────────┐
│                 AUTOMATION LAYER                        │
│  ┌──────────────────────────────────────────────────┐  │
│  │          Claude Reads & Analyzes                 │  │
│  │  • Extracts requirements from documents          │  │
│  │  • Creates user stories with priorities          │  │
│  │  • Generates comprehensive documentation         │  │
│  │  • Plans sprints with capacity management        │  │
│  └──────────────────┬───────────────────────────────┘  │
└─────────────────────┼───────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────┐
│                   OUTPUT LAYER                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │
│  │  PRD         │  │  Tech Spec   │  │  Sprint      │ │
│  │  (Generated) │  │  (Generated) │  │  Plans       │ │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘ │
└─────────┼──────────────────┼──────────────────┼─────────┘
          │                  │                  │
          ▼                  ▼                  ▼
┌─────────────────────────────────────────────────────────┐
│               IMPLEMENTATION LAYER                      │
│  • Developers follow generated docs                     │
│  • Claude enforces coding standards                     │
│  • Progress tracked automatically                       │
│  • Quality gates ensure completion                      │
└─────────────────────────────────────────────────────────┘
```

---

### Key Benefits

| Audience | Benefits |
|----------|----------|
| **Developers** | Clear requirements, structured work, automatic documentation, coding standards enforced |
| **Project Managers** | Automated tracking, instant status, velocity metrics, predictable delivery |
| **Clients** | Transparent progress, professional documentation, clear communication |
| **Teams** | Consistent standards, easy onboarding, reduced meetings, better collaboration |

---

### Real-World Impact

**Time Savings:**
- Requirements extraction: **5 hours → 1 hour** (80% reduction)
- Project documentation: **2 days → 5 minutes** (99% reduction)
- Sprint planning: **3 hours → 10 minutes** (94% reduction)
- Progress reporting: **1 hour/week → 5 minutes** (92% reduction)

**Quality Improvements:**
- Test coverage: **60% → 85%** (enforced standards)
- Documentation completeness: **40% → 95%** (automated generation)
- Coding standards adherence: **70% → 98%** (Claude enforcement)

---

## Getting Started

### Prerequisites

Before you begin, ensure you have:

- ✅ **Git repository** for your project
- ✅ **Claude Code CLI** installed and working
- ✅ **Basic understanding** of Agile/Scrum concepts (sprints, user stories)
- ⭐ **Optional:** Client documents (PDF, Word, mockups) to process

---

### Installation & Setup

#### Step 1: Copy System to Your Project

```bash
# Navigate to your project
cd /path/to/your/project

# Copy the entire .project-management system
cp -r /path/to/claude_repo/.project-management .
cp -r /path/to/claude_repo/.claude .
cp /path/to/claude_repo/.CLAUDE.MD .
```

#### Step 2: Verify File Structure

Your project should now have:

```
your-project/
├── .CLAUDE.MD                    ← Core coding standards
├── .claude/
│   ├── rules/                    ← Specialized rules (5 files)
│   └── commands/                 ← Slash commands (6 files)
├── .project-management/
│   ├── input/                    ← YOU FILL THESE
│   │   ├── scope.md
│   │   ├── backlog.md
│   │   ├── technologies.md
│   │   └── constraints.md
│   ├── templates/                ← Used for generation
│   ├── rules/                    ← Project-specific rules
│   ├── client-input/             ← Drop client docs here
│   └── output/                   ← CLAUDE GENERATES THESE
│       ├── docs/
│       ├── sprints/
│       └── progress/
└── your-source-code/
```

---

### Understanding the File Structure

#### 📝 Files YOU Touch (Input)

| File | Purpose | When to Edit |
|------|---------|--------------|
| `input/scope.md` | Project vision, goals, objectives | At project start, when scope changes |
| `input/backlog.md` | All features as user stories | At start, when adding features |
| `input/technologies.md` | Tech stack decisions | At start, when tech changes |
| `input/constraints.md` | Timeline, budget, team size | At start, when constraints change |
| `rules/project-rules.md` | Core project-specific rules (mandatory) | When you need custom standards |
| `rules/I18N-RULES.md` | i18n requirements (conditional) | If your project needs multiple languages |
| `rules/TESTING-RULES.md` | Custom testing rules (conditional) | If your project has specific test requirements |
| `client-input/*` | Client documents | When you receive client docs |

#### 🤖 Files CLAUDE Generates (Output)

| File | Purpose | When Generated |
|------|---------|----------------|
| `output/docs/prd.md` | Product Requirements Document | `/init-project` or `/generate-docs` |
| `output/docs/technical-spec.md` | Technical Specification | `/init-project` or `/generate-docs` |
| `output/docs/architecture.md` | Architecture Documentation | `/init-project` or `/generate-docs` |
| `output/sprints/sprint-N.md` | Sprint plan with stories | `/plan-sprint N` |
| `output/progress/progress-*.md` | Progress tracker | `/update-progress` |

#### 📋 Templates (Don't Edit Unless Customizing)

| File | Purpose |
|------|---------|
| `templates/prd-template.md` | Template for PRD generation |
| `templates/technical-spec-template.md` | Template for tech spec |
| `templates/sprint-template.md` | Template for sprint plans |
| `templates/progress-template.md` | Template for progress tracking |

---

### First-Time Configuration

#### 1. Review Core Standards (`.CLAUDE.MD`)

Open `.CLAUDE.MD` and review:
- ✅ Coding standards (naming, functions, imports)
- ✅ Testing requirements (80% coverage, API matrix)
- ✅ Security practices
- ✅ Git workflow
- ✅ Quality gates

**Most projects:** No changes needed, standards are comprehensive.

**If needed:** You can override in `project-rules.md`

---

#### 2. Customize Project Rules

Open `.project-management/rules/project-rules.md`:

**Required:** Fill project overview
```markdown
## Project Overview
- **Project Name:** Your Project Name
- **Type:** Web Application / Mobile App / API / etc.
- **Tech Stack:** React, Node.js, PostgreSQL, etc.
```

**Optional:** Configure i18n if needed

If your project needs multiple languages, edit `I18N-RULES.md`:
```markdown
**MANDATORY**: All user-facing features MUST include translations for:

- ✅ **English** (default) - `en.json`
- ✅ **German** - `de.json`
- ✅ **French** - `fr.json`
```

💡 **Don't need i18n?** Delete `I18N-RULES.md` file.

---

#### 3. Set Testing Requirements

**General testing** (applies to all projects - in `.claude/rules/testing.md`):
```markdown
**Minimum Coverage:**
- Overall code coverage: **80%+**
- Critical path coverage: **95%+**
- API status codes: **200/400/401/403/404/500**
```

**Project-specific testing** (optional - in `TESTING-RULES.md`):

If you need custom critical paths or test utilities, edit `TESTING-RULES.md`.

💡 **Don't need custom testing?** Delete `TESTING-RULES.md` file.

Adjust if your project needs different thresholds.

---

#### 4. Git Ignore Configuration

Ensure `.gitignore` includes:

```gitignore
# Client documents (private)
.project-management/client-input/*
!.project-management/client-input/README.md
!.project-management/client-input/EXAMPLE-*.txt

# Generated output (commit or ignore based on preference)
# .project-management/output/
```

**Decision:** Do you want to commit generated docs?
- ✅ **Yes:** Comment out the output line (team sees docs in git)
- ❌ **No:** Keep it (regenerate on demand)

---

## Workflows

### Workflow A: Starting with Client Documents

**When to use:** Agency work, client projects, RFPs, project briefs

**Time required:** 15-30 minutes

---

#### Step-by-Step Process

**1. Gather Client Documents**

Collect everything the client sent:
- 📄 Project briefs (PDF, Word)
- 📊 Requirements documents
- 🖼️ Mockups and wireframes (PNG, JPG)
- 📝 Meeting notes
- 📧 Email threads with decisions

**2. Organize Documents**

Copy to `client-input/` with numbered prefixes (processed in order):

```bash
cp client-brief.pdf .project-management/client-input/01-brief.pdf
cp requirements.docx .project-management/client-input/02-requirements.docx
cp mockups.png .project-management/client-input/03-mockups.png
cp notes.txt .project-management/client-input/04-notes.txt
```

💡 **Pro Tip:** Number files so Claude processes in logical order.

**3. Process Documents**

```bash
/process-client-docs
```

**What Claude does:**
- Reads all documents (PDF, Word, images)
- Extracts features and requirements
- Converts to user stories
- Assigns priorities (P0, P1, P2, P3)
- Creates acceptance criteria
- Estimates story points
- Identifies gaps and missing info

**Output:** All 4 input files generated!
- ✅ `input/scope.md` - Vision and goals extracted
- ✅ `input/backlog.md` - User stories created (with priorities)
- ✅ `input/technologies.md` - Tech stack identified
- ✅ `input/constraints.md` - Timeline/budget captured

**4. Review Generated Files**

```bash
# Check extracted requirements
cat .project-management/input/scope.md
cat .project-management/input/backlog.md
```

**Look for:**
- ✅ All features captured
- ✅ Priorities make sense
- ✅ No `[NEEDS CLARIFICATION]` placeholders
- ✅ Story points reasonable

**5. Refine if Needed**

Edit generated files to:
- Adjust priorities
- Clarify ambiguous stories
- Add missing features
- Fix inaccuracies

**6. Initialize Project**

```bash
/init-project
```

**What Claude generates:**
- ✅ PRD (Product Requirements Document)
- ✅ Technical Specification
- ✅ Architecture Documentation
- ✅ Initial progress tracker

**7. Start First Sprint**

```bash
/plan-sprint 1
```

Sprint plan created with selected stories! 🎉

---

#### Real Example: Agency Receives RFP

**Scenario:** Client sends `RFP-ecommerce-platform.pdf` (30 pages)

**Process:**
```bash
# 1. Copy document
cp ~/Downloads/RFP-ecommerce-platform.pdf .project-management/client-input/

# 2. Process
/process-client-docs

# Claude extracts:
# - 35 user stories
# - 8 epics
# - Timeline: 12 weeks
# - Budget: $80,000
# - Tech: React, Node.js, PostgreSQL

# 3. Review
cat .project-management/input/backlog.md
# All 35 stories extracted with priorities!

# 4. Initialize
/init-project

# 5. Start
/plan-sprint 1
```

**Result:** From RFP to sprint plan in **10 minutes**!

---

### Workflow B: Manual Project Setup

**When to use:** Internal projects, from scratch, no client docs

**Time required:** 1-2 hours (depending on project complexity)

---

#### Step-by-Step Process

**1. Fill Scope (Vision & Goals)**

Open `.project-management/input/scope.md` and fill:

```markdown
## Project Vision

**Vision Statement:**
Build a task management app for remote teams that emphasizes simplicity and real-time collaboration.

## Project Objectives

**Primary Objectives:**
1. Enable teams to create and manage tasks in real-time
2. Provide simple, intuitive UI requiring no training
3. Support 100+ concurrent users with <200ms response time

## Target Audience

**Primary Users:**
- Remote team leads (25-45 years old)
- Distributed development teams (5-50 members)
- Tech-savvy, expect modern UX

## Success Criteria

**Quantitative:**
- 1,000 active teams in first 6 months
- 90% task completion rate
- <2% error rate

**Qualitative:**
- "Easiest task manager we've used" - user feedback
- Zero training required for basic features
```

💡 **Pro Tip:** Be specific! Vague scope leads to vague documentation.

---

**2. Fill Backlog (User Stories)**

Open `.project-management/input/backlog.md` and create user stories:

```markdown
## Epic 1: User Authentication

### US-001: User Registration
**As a** new user
**I want to** create an account with email and password
**So that** I can access the platform

**Acceptance Criteria:**
- [ ] Email validation (valid format)
- [ ] Password strength requirements (8+ chars, uppercase, lowercase, number)
- [ ] Confirmation email sent
- [ ] Account created in database
- [ ] User redirected to dashboard after registration

**Priority:** P0 (Must Have)
**Story Points:** 5
**Dependencies:** None

### US-002: User Login
**As a** registered user
**I want to** log in with my credentials
**So that** I can access my tasks

**Acceptance Criteria:**
- [ ] Email and password validated
- [ ] Session created on successful login
- [ ] Error message for invalid credentials
- [ ] "Remember me" option available
- [ ] Redirect to last viewed page

**Priority:** P0 (Must Have)
**Story Points:** 3
**Dependencies:** US-001
```

**Structure:**
- Group by Epics (related features)
- Use proper user story format
- Include detailed acceptance criteria
- Set priorities (P0, P1, P2, P3)
- Estimate story points (Fibonacci: 1, 2, 3, 5, 8, 13)
- Note dependencies

💡 **Pro Tip:** 20-30 stories is typical for a mid-size project.

---

**3. Fill Technologies (Tech Stack)**

Open `.project-management/input/technologies.md`:

```markdown
## Frontend

### Core Framework
- **Framework:** React 19.0.0
- **Language:** TypeScript 5.7.0
- **Build Tool:** Vite 6.1.0
- **Package Manager:** npm

### UI & Styling
- **CSS Framework:** Tailwind CSS 4.1.0
- **Component Library:** shadcn/ui + Radix UI
- **Icons:** Lucide React

## Backend

### Runtime & Framework
- **Runtime:** Node.js 20.x LTS
- **Framework:** Express.js 4.18.0
- **Language:** TypeScript 5.7.0

### Database
- **Type:** PostgreSQL 16.x
- **ORM:** Prisma 6.19.0
- **Why:** ACID compliance, strong relational model

## Testing
- **Unit Tests:** Vitest 4.0.0
- **E2E Tests:** Playwright 1.58.0
- **Coverage Tool:** Istanbul (c8)
```

💡 **Pro Tip:** Include version numbers and explain WHY for major choices.

---

**4. Fill Constraints (Limitations)**

Open `.project-management/input/constraints.md`:

```markdown
## Timeline

**Project Start:** 2026-04-01
**Target Completion:** 2026-07-01 (3 months)
**Milestones:**
- Month 1: Authentication + Task CRUD
- Month 2: Real-time collaboration + Teams
- Month 3: Polish + Testing + Deployment

## Budget

**Total Budget:** $45,000
**Breakdown:**
- Development: $35,000 (77%)
- Infrastructure: $5,000 (11%)
- Third-party Services: $3,000 (7%)
- Contingency: $2,000 (5%)

## Team

**Team Size:** 2 developers + 1 designer
**Availability:**
- Dev 1: 40 hours/week (full-time)
- Dev 2: 20 hours/week (part-time)
- Designer: 10 hours/week (contractor)

**Capacity:** ~70 hours/week development

## Compliance

- **GDPR:** Required (European users)
- **Data Encryption:** At rest and in transit
- **Password Storage:** Bcrypt hashing required
```

---

**5. Initialize Project**

```bash
/init-project
```

Claude generates complete documentation! ✅

**6. Review Generated Docs**

```bash
ls .project-management/output/docs/
# prd.md
# technical-spec.md
# architecture.md

# Check technical spec
cat .project-management/output/docs/technical-spec.md
```

**Verify:**
- ✅ All user stories included
- ✅ Tech stack documented
- ✅ Architecture makes sense
- ✅ API endpoints defined

**7. Start First Sprint**

```bash
/plan-sprint 1
```

Sprint plan created! 🎉

---

### Workflow C: Daily Development

**When to use:** Every day during active development

---

#### Morning (Start of Day)

**1. Check Current Sprint**

```bash
cat .project-management/output/sprints/sprint-1.md
```

**Review:**
- ✅ Sprint goal
- ✅ Your assigned stories
- ✅ Story priorities
- ✅ Dependencies

**2. Pick a Story**

Choose from sprint backlog:
- Start with highest priority (P0)
- Check dependencies are complete
- Verify acceptance criteria clear

**Example:** US-005: Create Product Listing

---

#### During Development (Implementation)

**3. Read Technical Spec**

```bash
cat .project-management/output/docs/technical-spec.md
```

**Look for:**
- ✅ Architecture patterns to follow
- ✅ API conventions
- ✅ Database schema
- ✅ Error handling patterns

⚠️ **CRITICAL:** Always read tech spec before implementing! It's your source of truth.

**4. Use TodoWrite for Task Breakdown**

Break down user story into implementation tasks:

```
User Story: US-005 Create Product Listing

TodoWrite Tasks:
1. Create Product model in database
2. Create API endpoint GET /api/products
3. Create ProductList component
4. Add tests (unit, integration, e2e)
5. Update technical spec with API docs
```

**5. Implement Following Standards**

Claude enforces:
- ✅ SOLID & DRY principles (`.claude/rules/code-quality.md`)
- ✅ API testing matrix (`.claude/rules/testing.md`)
- ✅ Git conventions (`.claude/rules/git.md`)
- ✅ Database migrations (`.claude/rules/database.md`)

**6. Run Tests**

```bash
npm test
npm run typecheck
npm run lint
```

All must pass before marking task complete!

---

#### End of Day (Progress Update)

**7. Update Progress**

```bash
/update-progress
```

Claude will ask:
- What did you complete today?
- Any blockers encountered?
- Tests passing?

**Example:**
```
Completed:
- US-005: Create Product Listing (done)
- Tests: 12/12 passing
- Docs: API endpoint documented

Blockers:
- None
```

**8. Review Status**

```bash
/project-status
```

See:
- ✅ Sprint progress (% complete)
- ✅ Velocity trend
- ✅ Test metrics
- ✅ Blockers (if any)

---

#### When Blocked

**9. Log Blocker Immediately**

```bash
/update-progress
```

Report blocker:
```
Blocker:
- ID: BLOCK-001
- Impact: High
- Description: Third-party API rate limit hit (500/day, need 2000)
- Mitigation: Contacted vendor for limit increase, ETA 2 days
- Workaround: Using cached data for now
```

💡 **Pro Tip:** Don't hide blockers! Early visibility enables solutions.

---

### Workflow D: Sprint Planning

**When to use:** End of current sprint, planning next sprint

**Time required:** 30-60 minutes

---

#### Step-by-Step Process

**1. Review Current Sprint Completion**

```bash
cat .project-management/output/sprints/sprint-1.md
```

**Check:**
- ✅ Completed stories
- ✅ Incomplete stories (move to next sprint)
- ✅ Test counts
- ✅ Velocity achieved

**2. Calculate Velocity**

```
Sprint 1: Planned 21 points, Completed 18 points = 18 velocity
Sprint 2: Planned 24 points, Completed 22 points = 22 velocity

Average Velocity: (18 + 22) / 2 = 20 points per sprint
```

**3. Plan Next Sprint**

```bash
/plan-sprint 3
```

**Claude will:**
- Review backlog
- Check team capacity
- Consider velocity (20 points)
- Select stories (P0 first)
- Check dependencies
- Create sprint plan

**4. Review Sprint Plan**

```bash
cat .project-management/output/sprints/sprint-3.md
```

**Verify:**
- ✅ Sprint goal clear
- ✅ Stories achievable (18-22 points)
- ✅ Dependencies met
- ✅ No conflicts in assignments

**5. Adjust if Needed**

Edit sprint file to:
- Swap stories
- Adjust priorities
- Add/remove based on capacity

**6. Communicate to Team**

Share sprint plan:
- Sprint goal
- Selected stories
- Individual assignments
- Key milestones

---

#### Real Example: Planning Sprint 3

**Context:**
- Team: 2 developers
- Sprint 1 velocity: 18 points
- Sprint 2 velocity: 22 points
- Average: 20 points
- Backlog: 15 stories remaining (total 65 points)

**Planning:**
```bash
/plan-sprint 3

# Claude selects:
# - US-015: Real-time notifications (8 points) - P0
# - US-016: Team dashboard (5 points) - P0
# - US-020: Email digests (5 points) - P1
# - US-023: Dark mode (2 points) - P2
# Total: 20 points (matches velocity)
```

**Sprint 3 Goal:** Enable real-time collaboration with notifications and team visibility

---

### Workflow E: Scope Changes

**When to use:** Client adds features, requirements change mid-project

**Time required:** 15-30 minutes

---

#### Step-by-Step Process

**1. Document New Requirements**

**Option A:** Client sends new documents
```bash
cp new-features.pdf .project-management/client-input/
/process-client-docs
```

**Option B:** Manual addition
Edit `.project-management/input/backlog.md`:
```markdown
### US-NEW: Export to PDF
**As a** user
**I want to** export my tasks to PDF
**So that** I can share with stakeholders

**Priority:** P1 (Should Have)
**Story Points:** 8
```

**2. Regenerate Documentation**

```bash
/generate-docs
```

Claude updates:
- ✅ PRD with new features
- ✅ Technical spec with architecture changes
- ✅ Progress tracker with new totals

**3. Assess Impact**

Review updated docs:
```bash
cat .project-management/output/docs/technical-spec.md
```

**Check:**
- Database schema changes needed?
- New API endpoints required?
- Architecture changes?
- Timeline impact?

**4. Communicate Changes**

Prepare impact analysis:
```
New Feature: Export to PDF (US-NEW)

Impact:
- Story Points: 8 (will affect Sprint 4)
- Technical: New PDF generation library needed
- Timeline: +3 days (1 sprint slot)
- Budget: +$2,000 (third-party PDF service)

Options:
1. Add to Sprint 4 (delays other features)
2. Add to Sprint 5 (no delay, +1 sprint)
3. Descope P2 features to fit
```

**5. Update Sprint Plans**

If approved:
```bash
# Replan affected sprint
/plan-sprint 4
```

**6. Update Client**

Share:
- Updated PRD
- Timeline impact
- Cost impact (if any)
- New sprint plans

---

### Workflow F: Handling Blockers

**When to use:** When progress is blocked by external factors

---

#### Step-by-Step Process

**1. Identify Blocker**

Common blockers:
- Third-party API issues
- Dependency on another team
- Unclear requirements
- Technical limitations
- Resource unavailable

**2. Log Immediately**

```bash
/update-progress
```

Example blocker:
```
Blocker: BLOCK-003
Impact: High (blocks US-020, US-021)
Type: External Dependency
Description: Payment gateway API in maintenance mode
Expected Resolution: 2 days (per vendor)
Affected Stories: US-020, US-021 (13 points)
Owner: John Smith
Mitigation Plan:
  1. Switch to sandbox mode for testing
  2. Complete UI/UX work
  3. Integration when API returns
```

**3. Assess Sprint Impact**

Calculate:
```
Sprint 4 Plan: 22 points
Blocked: 13 points (59%)
At Risk: Yes

Options:
1. Wait (risk missing sprint goal)
2. Pull other stories from backlog
3. Work on non-blocked stories
```

**4. Mitigate**

Actions:
- Work around blocker if possible
- Pull replacement stories
- Document workaround
- Set follow-up date

**5. Communicate**

Update stakeholders:
- Blocker details
- Impact assessment
- Mitigation plan
- New timeline (if affected)

**6. Track Resolution**

```bash
/update-progress

# When resolved:
Blocker BLOCK-003: RESOLVED
Resolution Date: 2026-04-15
Actual Impact: 1 day delay (within buffer)
Lessons Learned: Add vendor status checks to daily routine
```

---

## Commands Reference

> **Note:** Commands in this system are called "**Skills**" in Claude Code. When you see `/command-name`, you're actually invoking a Skill.

### How Skills Work

**Skills are auto-discovered from:**
- **Project-level:** `.claude/skills/<skill-name>/SKILL.md` (this project only)
- **Personal-level:** `~/.claude/skills/<skill-name>/SKILL.md` (all projects)

**No registration needed!** Claude Code automatically discovers and registers any `SKILL.md` files when it starts.

---

### ⚠️ IMPORTANT: System Requirement

**Skills require `ripgrep` to be installed!**

Claude Code uses `ripgrep` for skill discovery. Without it, skills won't be found even if correctly configured.

**Installation:**
```bash
# Ubuntu/Debian
sudo apt-get install ripgrep

# Configure environment
export USE_BUILTIN_RIPGREP=0
echo 'export USE_BUILTIN_RIPGREP=0' >> ~/.bashrc

# Restart Claude Code CLI
```

**Verify installation:**
```bash
/doctor    # Checks if ripgrep is working
which rg   # Should show path to ripgrep binary
```

If skills still don't work after installing ripgrep, restart your Claude Code session.

---

### Skill File Structure

Each skill MUST follow this exact structure:

```
.claude/skills/
└── process-client-docs/          ← Directory named after skill
    └── SKILL.md                   ← MUST be named "SKILL.md"
```

**SKILL.md contents:**
```markdown
---
name: process-client-docs           ← Creates /process-client-docs command
description: Short description      ← Helps Claude decide when to use it
---

# Skill Title

Your skill instructions and markdown content here...
```

**YAML frontmatter fields:**
- `name` (required): Creates the `/slash-command`
- `description` (required): Guides Claude on when to use the skill
- `disable-model-invocation` (optional): Restrict to manual invocation only
- `user-invocable` (optional): Whether skill appears in menu
- `context: fork` (optional): Run skill in isolated subagent

---

### Verifying Your Skills

To see all registered skills:
```bash
# List all available skills
/skills

# Or just type / in Claude Code to see autocomplete menu
/
```

If your skill doesn't appear:
1. Check file is at `.claude/skills/<skill-name>/SKILL.md`
2. Verify YAML frontmatter has `name:` and `description:`
3. Restart Claude Code session (or start a new conversation)

---

### Command Decision Tree

```
Need to...
├─ Process client documents? → /process-client-docs
├─ Start new project? → /init-project
├─ Update documentation? → /generate-docs
├─ Plan next sprint? → /plan-sprint [N]
├─ Update progress? → /update-progress
└─ Check project status? → /project-status
```

---

### `/process-client-docs`

**What it does:** Extracts requirements from client documents (PDF, Word, images)

**When to use:**
- ✅ Client sends project brief
- ✅ You have mockups/wireframes
- ✅ RFP documents received
- ✅ Meeting notes to process

**When NOT to use:**
- ❌ No client documents available
- ❌ Starting from scratch
- ❌ Requirements already in backlog

---

#### Usage

```bash
# 1. Add documents to folder
cp client-brief.pdf .project-management/client-input/

# 2. Run command
/process-client-docs

# 3. Review generated files
ls .project-management/input/
```

---

#### What Claude Does

1. **Scans folder** for all documents
2. **Reads content:**
   - PDFs: Full text + images
   - Word: Full content
   - Images: Visual analysis + OCR
   - Text: Plain text reading
3. **Analyzes and extracts:**
   - Features → user stories
   - Goals → scope
   - Tech mentions → technologies
   - Timeline/budget → constraints
4. **Generates:**
   - `scope.md` with vision
   - `backlog.md` with user stories
   - `technologies.md` with stack
   - `constraints.md` with limitations
5. **Identifies gaps** and asks questions

---

#### Example Output

**Input:** `client-brief.pdf` (25 pages)

**Generated `backlog.md`:**
```markdown
## Epic 1: User Authentication (extracted from pages 3-5)

### US-001: User Registration
**As a** new user
**I want to** register with email
**So that** I can access the platform

**Priority:** P0 (mentioned as "critical requirement")
**Story Points:** 5 (estimated based on complexity)
**Acceptance Criteria:**
- [ ] Email validation
- [ ] Password requirements (8+ chars, mixed case, number)
- [ ] Confirmation email sent

### US-002: Social Login
**As a** user
**I want to** login with Google/Facebook
**So that** I don't need another password

**Priority:** P1 (mentioned as "nice to have")
**Story Points:** 8
```

**Total extracted:** 23 user stories, 5 epics, timeline, budget

---

#### Files Modified

- ✅ `.project-management/input/scope.md` (created/updated)
- ✅ `.project-management/input/backlog.md` (created/updated)
- ✅ `.project-management/input/technologies.md` (created/updated)
- ✅ `.project-management/input/constraints.md` (created/updated)

---

#### Common Issues

**Issue:** "No documents found"
**Solution:** Check path: `.project-management/client-input/`

**Issue:** "Extraction incomplete"
**Solution:** Add more documents, re-run command (it merges)

**Issue:** "Wrong priorities assigned"
**Solution:** Edit `backlog.md` manually after generation

---

#### Pro Tips

💡 **Tip 1:** Number files for processing order (01-brief.pdf, 02-mockups.png)

💡 **Tip 2:** Include meeting notes as .txt files for context

💡 **Tip 3:** Re-run command after adding more docs (it merges new info)

💡 **Tip 4:** Always review generated files - Claude makes educated guesses

---

### `/init-project`

**What it does:** Generates complete project documentation from input files

**When to use:**
- ✅ After filling input files (manual or via `/process-client-docs`)
- ✅ Starting new project
- ✅ First-time setup complete

**When NOT to use:**
- ❌ Input files not filled yet
- ❌ Just updating existing docs (use `/generate-docs`)

---

#### Usage

```bash
# 1. Ensure input files filled
ls .project-management/input/
# scope.md ✓
# backlog.md ✓
# technologies.md ✓
# constraints.md ✓

# 2. Run command
/init-project

# 3. Review generated docs
ls .project-management/output/docs/
```

---

#### What Claude Does

1. **Reads all input files:**
   - scope.md (vision, goals)
   - backlog.md (user stories)
   - technologies.md (tech stack)
   - constraints.md (timeline, budget)
2. **Analyzes:**
   - Identifies epics and phases
   - Calculates project duration
   - Estimates total effort
   - Identifies critical paths
3. **Generates documentation:**
   - PRD (Product Requirements)
   - Technical Specification
   - Architecture Document
   - Initial Progress Tracker
4. **Creates folder structure:**
   - `output/docs/` for documentation
   - `output/sprints/` for sprint plans (empty initially)
   - `output/progress/` for tracking

---

#### Example Output

**Input:**
- 25 user stories
- 3 months timeline
- React + Node.js stack

**Generated:**
```
output/docs/
├── prd.md (35 pages)
│   ├── Product vision
│   ├── Target audience
│   ├── Feature breakdown (25 stories)
│   ├── Success metrics
│   └── Roadmap (3 phases)
│
├── technical-spec.md (42 pages)
│   ├── Architecture overview
│   ├── Database schema
│   ├── API endpoints (RESTful)
│   ├── Authentication flow
│   ├── Tech stack details
│   └── Security measures
│
└── architecture.md (28 pages)
    ├── System architecture diagram
    ├── Component breakdown
    ├── Data flow
    ├── Deployment architecture
    └── Infrastructure requirements
```

---

#### Files Created

- ✅ `.project-management/output/docs/prd.md`
- ✅ `.project-management/output/docs/technical-spec.md`
- ✅ `.project-management/output/docs/architecture.md`
- ✅ `.project-management/output/progress/progress-initial.md`

---

#### Common Issues

**Issue:** "Input files have placeholders"
**Solution:** Fill all `{{PLACEHOLDER}}` values in input files

**Issue:** "Generated docs too generic"
**Solution:** Be more specific in input files (especially scope and backlog)

**Issue:** "Tech stack not detailed"
**Solution:** Add version numbers and reasons in `technologies.md`

---

#### Pro Tips

💡 **Tip 1:** Run this ONCE at project start, then use `/generate-docs` for updates

💡 **Tip 2:** Review generated docs immediately - easier to fix now than later

💡 **Tip 3:** Commit generated docs to git for team visibility

💡 **Tip 4:** Share PRD with client for validation before development starts

---

### `/generate-docs`

**What it does:** Regenerates documentation when input files change

**When to use:**
- ✅ Scope changed (features added/removed)
- ✅ Tech stack changed
- ✅ Timeline/budget adjusted
- ✅ Backlog updated with new stories

**When NOT to use:**
- ❌ First-time setup (use `/init-project`)
- ❌ No changes to input files
- ❌ Just updating progress (use `/update-progress`)

---

#### Usage

```bash
# 1. Edit input files
vim .project-management/input/backlog.md
# Add new user story US-030

# 2. Regenerate docs
/generate-docs

# 3. Review updated docs
git diff .project-management/output/docs/
```

---

#### What Claude Does

1. **Reads updated input files**
2. **Compares with existing docs**
3. **Updates:**
   - PRD with new features
   - Technical spec with architecture changes
   - Progress tracker with updated totals
4. **Preserves:**
   - Sprint plans (not regenerated)
   - Progress history
   - Custom changes (if noted)

---

#### Example Scenario

**Change:** Client adds "Export to PDF" feature

**Actions:**
```bash
# 1. Add to backlog
echo "### US-030: Export to PDF" >> input/backlog.md
# (fill details)

# 2. Regenerate
/generate-docs

# 3. Claude updates:
# - PRD: New feature section added
# - Tech spec: PDF library added to stack
# - Progress: Total story points increased
```

---

#### Files Modified

- ✅ `.project-management/output/docs/prd.md` (updated)
- ✅ `.project-management/output/docs/technical-spec.md` (updated)
- ✅ `.project-management/output/docs/architecture.md` (updated if needed)
- ✅ `.project-management/output/progress/progress-*.md` (updated)

---

### `/plan-sprint [N]`

**What it does:** Plans a sprint by selecting stories from backlog

**When to use:**
- ✅ Starting first sprint
- ✅ Beginning next sprint (previous complete)
- ✅ Replanning current sprint (scope change)

**When NOT to use:**
- ❌ Mid-sprint (wait until sprint ends)
- ❌ Backlog empty
- ❌ Input files not generated

---

#### Usage

```bash
# Plan first sprint
/plan-sprint 1

# Plan next sprint
/plan-sprint 2

# Replan current sprint (if scope changed)
/plan-sprint 2  # overwrite existing
```

---

#### What Claude Does

1. **Analyzes:**
   - Team capacity (from constraints.md)
   - Velocity (from previous sprints)
   - Backlog priorities (P0, P1, P2, P3)
   - Story dependencies
2. **Selects stories:**
   - Prioritizes P0 first
   - Checks dependencies met
   - Respects capacity/velocity
   - Balances workload
3. **Creates sprint plan:**
   - Sprint goal
   - Selected stories (detailed)
   - Capacity calculation
   - Task breakdown
   - Acceptance criteria
4. **Generates file:**
   - `output/sprints/sprint-N.md`

---

#### Example Output

**Input:**
- Team capacity: 80 hours/week
- Velocity: 20 points/sprint (average)
- Backlog: 15 stories remaining

**Generated `sprint-2.md`:**
```markdown
# Sprint 2 Plan

**Sprint Goal:** Implement real-time notifications and team dashboard

**Duration:** 2 weeks (2026-04-01 to 2026-04-14)

**Team Capacity:** 80 hours (2 developers)

**Planned Story Points:** 20 points (matches velocity)

---

## Selected Stories

### US-015: Real-time Notifications (8 points) - P0
**As a** user
**I want to** receive real-time notifications
**So that** I'm updated on task changes

**Assigned to:** Developer 1
**Estimated Hours:** 32 hours

**Tasks:**
- [ ] WebSocket server setup
- [ ] Notification component
- [ ] Database triggers
- [ ] Tests (unit, integration, e2e)

**Acceptance Criteria:**
- [ ] Notifications appear in <1 second
- [ ] Works with multiple tabs
- [ ] Persists on page reload

---

### US-016: Team Dashboard (5 points) - P0
**As a** team lead
**I want to** see team progress dashboard
**So that** I can monitor velocity

**Assigned to:** Developer 2
**Estimated Hours:** 20 hours
...

---

**Total:** 4 stories, 20 points, 80 hours
```

---

#### Files Created

- ✅ `.project-management/output/sprints/sprint-N.md`

---

#### Common Issues

**Issue:** "Sprint seems too ambitious"
**Solution:** Edit sprint file, remove stories, adjust manually

**Issue:** "Dependencies not met"
**Solution:** Complete prerequisite stories first

**Issue:** "Velocity keeps increasing unrealistically"
**Solution:** Adjust constraints.md with realistic capacity

---

#### Pro Tips

💡 **Tip 1:** First sprint: aim for 60-70% capacity (unknown velocity)

💡 **Tip 2:** Review sprint plan with team before starting

💡 **Tip 3:** Keep sprint goal clear and focused (1 sentence)

💡 **Tip 4:** Balance P0/P1 stories (don't overload with P0 only)

---

### `/update-progress`

**What it does:** Updates progress tracker with completed work and blockers

**When to use:**
- ✅ End of day (daily update)
- ✅ Story completed
- ✅ Blocker encountered
- ✅ Sprint completed

**When NOT to use:**
- ❌ Nothing to report
- ❌ Mid-task (wait until task done)

---

#### Usage

```bash
# Daily update
/update-progress

# Claude will prompt:
# - What did you complete?
# - Any blockers?
# - Tests passing?
```

---

#### What Claude Does

1. **Prompts for info:**
   - Completed stories
   - Test counts
   - Blockers (if any)
   - Notes
2. **Updates progress tracker:**
   - Marks stories complete
   - Updates test metrics
   - Logs blockers
   - Calculates percentages
3. **Calculates:**
   - Sprint completion %
   - Velocity trend
   - Risk indicators
4. **Generates report:**
   - Updated progress-*.md file

---

#### Example Interaction

```
Claude: What work was completed today?
You: US-015 Real-time Notifications - fully complete

Claude: How many tests are passing for US-015?
You: 12/12 (8 unit, 3 integration, 1 e2e)

Claude: Any blockers encountered?
You: None

Claude: Any notes or observations?
You: WebSocket implementation simpler than expected,
     may have extra time this sprint

✅ Progress updated!
Sprint 2: 40% complete (2 of 5 stories done)
Velocity on track: 8 of 20 points complete
```

---

#### Files Modified

- ✅ `.project-management/output/progress/progress-sprint-2.md` (updated)

---

### `/project-status`

**What it does:** Generates comprehensive status report

**When to use:**
- ✅ Stakeholder meetings
- ✅ Check project health
- ✅ Before sprint planning
- ✅ Weekly team syncs

**When NOT to use:**
- ❌ Just checking simple info (read files directly)

---

#### Usage

```bash
/project-status
```

---

#### What Claude Does

1. **Aggregates data:**
   - All sprints
   - Velocity trends
   - Test metrics
   - Blockers
   - Phase progress
2. **Calculates:**
   - Overall completion %
   - Predicted end date
   - Budget spent
   - Risk level
3. **Generates report:**
   - Executive summary
   - Detailed metrics
   - Graphs (ASCII)
   - Recommendations

---

#### Example Output

```markdown
# Project Status Report

**Date:** 2026-04-15
**Project:** Task Management Platform
**Overall Status:** 🟢 On Track

---

## Executive Summary

**Progress:** 45% Complete (18 of 40 stories done)

**Sprint Status:**
- Sprint 1: ✅ Complete (18/21 points)
- Sprint 2: 🔄 In Progress (12/20 points, 60%)

**Key Metrics:**
- Velocity: 20 points/sprint (stable)
- Test Coverage: 87% (target: 80%)
- Bugs: 3 open (2 P3, 1 P2)

**Timeline:** On Track (3 of 12 weeks elapsed)

**Budget:** $15,000 of $45,000 spent (33%, on budget)

---

## Velocity Trend

Sprint 1: ████████████████░░ 18 pts
Sprint 2: ████████████░░░░░░ 12 pts (projected: 20)
Average: 19 pts/sprint

---

## Risks & Blockers

🟡 BLOCK-001: Third-party API rate limit (LOW impact)
   Status: Mitigation in progress
   ETA: 2 days

---

## Recommendations

1. ✅ Maintain current velocity
2. ⚠️ Monitor BLOCK-001 resolution
3. ✅ Continue test coverage (above target)
```

---

#### Files Created

- ✅ `.project-management/output/progress/status-[DATE].md`

---

## Customization Guide

### Coding Standards Customization

#### Editing Core Standards

**File:** `.CLAUDE.MD`

**Safe to modify:**
- Naming conventions (if different)
- Function length limits
- Import order
- Testing coverage targets

**Example: Change function length**
```markdown
### Functions
- Max 50 lines  ← Change to 40 or 60 if needed
```

⚠️ **Warning:** `.CLAUDE.MD` must stay under 200 lines!

---

#### Project-Specific Overrides

**File:** `.project-management/rules/project-rules.md`

**How it works:**
```
.CLAUDE.MD says: "Max 50 lines per function"
project-rules.md says: "Max 30 lines per function"

Result: 30 lines wins (project-specific overrides core)
```

**Example override:**
```markdown
## Project-Specific Coding Standards

### Function Length
**Override:** Max 30 lines per function
**Reason:** Codebase emphasizes readability over brevity
```

---

### Internationalization (i18n)

#### Deciding If You Need i18n

**Enable i18n if:**
- ✅ Targeting multiple countries
- ✅ International clients
- ✅ Localization required

**Skip i18n if:**
- ❌ Single-language market
- ❌ Internal tool
- ❌ MVP without international plans

---

#### Enabling i18n

**1. Open I18N-RULES.md**
```bash
vim .project-management/rules/I18N-RULES.md
```

**2. Configure languages**
```markdown
**MANDATORY**: All user-facing features MUST include translations for:

- ✅ **English** (default) - `en.json`
- ✅ **German** - `de.json`      ← Your language
- ✅ **French** - `fr.json`       ← Your language
- ✅ **Spanish** - `es.json`      ← Add or remove
```

**3. Set technology**
```markdown
**Technology:** react-i18next

**Translation files location:** /public/locales/
```

**4. Save and commit**

✅ **Result:** Claude will now check for translations before marking tasks complete!

---

#### Disabling i18n

**Delete the I18N-RULES.md file:**

```bash
rm .project-management/rules/I18N-RULES.md
```

Or leave placeholders unconfigured.

✅ **Result:** Claude skips translation checks.

---

#### Supported Languages

| Language | Code | Example |
|----------|------|---------|
| English | `en` | Default |
| German | `de` | Deutsch |
| French | `fr` | Français |
| Spanish | `es` | Español |
| Italian | `it` | Italiano |
| Portuguese | `pt` | Português |
| Dutch | `nl` | Nederlands |
| Polish | `pl` | Polski |
| Russian | `ru` | Русский |
| Japanese | `ja` | 日本語 |
| Chinese | `zh` | 中文 |

See `.project-management/rules/I18N-SETUP.md` for complete setup guide and language codes.

---

### Testing Requirements

#### General Testing (Always Applied)

**File:** `.claude/rules/testing.md`

**Default coverage targets:**
- Overall code coverage: **80%+**
- Critical path coverage: **95%+**
- New code coverage: **85%+**
- API status codes: **200/400/401/403/404/500** (all mandatory)

✅ **These rules apply to ALL projects automatically.**

---

#### Project-Specific Testing (Optional)

**File:** `.project-management/rules/TESTING-RULES.md`

**Use this file if your project needs:**
- ✅ Specific critical user flows
- ✅ Custom test utilities
- ✅ Performance benchmarks
- ✅ Security test requirements

**To enable:**

**1. Open TESTING-RULES.md**
```bash
vim .project-management/rules/TESTING-RULES.md
```

**2. Define critical paths**
```markdown
### 1. User Registration Flow

**Flow:** User registration → Email verification → Login

**Must test:**
- ✅ Happy path (all steps succeed)
- ✅ Email validation errors
- ✅ Duplicate email handling
```

**3. Configure test commands**
```markdown
npm test                    # All tests
npm run test:unit          # Unit tests
npm run test:e2e           # E2E tests
```

**4. Save and commit**

✅ **Result:** Claude will enforce these project-specific tests!

---

#### Disabling Project-Specific Testing

**Delete the TESTING-RULES.md file:**

```bash
rm .project-management/rules/TESTING-RULES.md
```

Or leave placeholders unconfigured.

✅ **Result:** Claude uses only general testing rules from `.claude/rules/testing.md`.

---

### Sprint Duration

#### Changing Sprint Length

**Default:** 2 weeks

**To change:**

1. Edit `.project-management/input/constraints.md`:
```markdown
## Sprint Configuration

**Sprint Duration:** 1 week     ← Changed from 2
**Sprints per Phase:** 4         ← Adjust accordingly
```

2. Adjust capacity in constraints.md:
```markdown
## Team

**Capacity:** 40 hours/week    ← For 1-week sprints
```

**Impact:**
- Shorter sprints = more frequent planning
- Velocity will be proportionally lower (10 pts/week vs 20 pts/2-weeks)

---

### Story Point Scale

#### Using T-Shirt Sizes

**Default:** Fibonacci (1, 2, 3, 5, 8, 13)

**To use T-shirts:**

1. Document in project-rules.md:
```markdown
## Story Point Scale

**Scale:** T-Shirt Sizes (XS, S, M, L, XL)

**Mapping:**
- XS = 1-2 points (1-4 hours)
- S = 3-5 points (4-8 hours)
- M = 8 points (8-16 hours)
- L = 13 points (16-24 hours)
- XL = 21 points (24+ hours)

**Usage:** Estimate in T-shirts, convert to Fibonacci for velocity
```

2. Use T-shirts in backlog.md:
```markdown
### US-001: User Registration
**Estimate:** M (8 points)
```

---

### Template Customization

#### Modifying Documentation Templates

**Location:** `.project-management/templates/`

**Example: Add company branding to PRD**

1. Edit `prd-template.md`:
```markdown
# Product Requirements Document

**Company:** {{COMPANY_NAME}}        ← Added
**Logo:** [Company Logo Here]        ← Added
**Project Name:** {{PROJECT_NAME}}
```

2. When `/init-project` runs, Claude uses your template

⚠️ **Warning:** Keep placeholders in `{{BRACKETS}}` format

---

#### Adding Custom Sections

**Example: Add "Competitor Analysis" to PRD**

Edit `prd-template.md`:
```markdown
---

## Competitor Analysis

### Competitor 1: {{COMPETITOR_1}}
**Strengths:** {{STRENGTHS_1}}
**Weaknesses:** {{WEAKNESSES_1}}
**Our Advantage:** {{ADVANTAGE_1}}

### Competitor 2: {{COMPETITOR_2}}
...
```

Fill in `scope.md`:
```markdown
## Competitor Information

**Competitor 1:** Asana
**Strengths:** Established brand, rich features
**Weaknesses:** Complex UI, expensive
**Our Advantage:** Simplicity, affordable pricing
```

---

### Adding Custom Commands

#### Creating New Slash Command

**Example:** Create `/deploy-check` command

1. Create file:
```bash
vim .claude/commands/deploy-check.md
```

2. Add instructions:
```markdown
# Deploy Readiness Check

Check if the project is ready for deployment.

## Steps

1. **Run all tests**
   - Check test results
   - Ensure 100% passing

2. **Check documentation**
   - README updated?
   - API docs current?
   - Deployment guide exists?

3. **Verify environment**
   - Environment variables set?
   - Database migrations applied?
   - SSL certificates valid?

4. **Security scan**
   - No secrets committed?
   - Dependencies up to date?
   - Security headers configured?

5. **Generate report**
   Create deployment readiness report with:
   - ✅ Ready items
   - ⚠️ Warnings
   - ❌ Blockers

**Output:** deployment-readiness-report.md
```

3. Use command:
```bash
/deploy-check
```

Claude follows your instructions!

---

## How Claude Uses This System

### Document Reading Priority

**Claude reads in this order:**

```
1. HIGHEST PRIORITY
   ↓
   .project-management/input/scope.md
   .project-management/input/backlog.md
   (SOURCE OF TRUTH for WHAT to build)
   ↓
2. HIGH PRIORITY
   ↓
   .project-management/output/docs/technical-spec.md
   (SOURCE OF TRUTH for HOW to build)
   ↓
3. MEDIUM PRIORITY
   ↓
   .project-management/rules/project-rules.md
   (Project-specific core rules - ALWAYS read)
   ↓
4. CONDITIONAL RULES (read only if enabled)
   ↓
   .project-management/rules/I18N-RULES.md
   (If i18n is configured)
   ↓
   .project-management/rules/TESTING-RULES.md
   (If project-specific testing is needed)
   ↓
5. STANDARD PRIORITY
   ↓
   .CLAUDE.MD (Core coding standards)
   .claude/rules/*.md (Specialized rules)
   ↓
6. CONTEXT-SPECIFIC
   ↓
   .project-management/output/sprints/sprint-N.md
   (When working on sprint tasks)
```

---

### Conditional Rules Pattern

**How it works:**

```
1. Check if conditional rules file exists
   ↓
2. Check if file is configured (not just placeholders)
   ↓
3. If BOTH true → Apply all rules from that file
   ↓
4. If EITHER false → Skip that file entirely
```

**Examples:**

**Scenario A: i18n Enabled**
```
I18N-RULES.md exists + configured with languages
→ Claude enforces translation requirements
```

**Scenario B: i18n Disabled**
```
I18N-RULES.md deleted OR has placeholders only
→ Claude skips translation checks
```

---

### Conflict Resolution

**If documents conflict:**

```
Example Conflict:
- scope.md says: "Support 1000 users"
- technical-spec.md says: "Support 500 users"

Resolution:
1. scope.md WINS (it's the source of truth)
2. Claude will ask: "Conflict detected - which is correct?"
3. You clarify
4. Run /generate-docs to fix technical-spec.md
```

**Priority order:**
```
scope.md > backlog.md > technical-spec.md >
project-rules.md > conditional rules > .CLAUDE.MD
```

---

### Planning Phase

#### What Claude Does During `/init-project`

```
Step 1: Read Input Files
├─ scope.md → Extract vision, goals, success criteria
├─ backlog.md → Count stories, epics, calculate total points
├─ technologies.md → Identify tech stack
└─ constraints.md → Get timeline, budget, team size

Step 2: Analyze & Calculate
├─ Identify phases (based on epics)
├─ Calculate project duration (points / velocity)
├─ Estimate team capacity (hours/week * weeks)
├─ Identify critical paths
└─ Detect risks

Step 3: Generate Documentation
├─ PRD: Product vision + features breakdown
├─ Technical Spec: Architecture + API design
└─ Architecture Doc: System design + deployment

Step 4: Create Structure
├─ output/docs/ (generated docs)
├─ output/sprints/ (empty, for future sprint plans)
└─ output/progress/ (initial tracker)
```

---

#### What Claude Does During `/plan-sprint N`

```
Step 1: Analyze Context
├─ Read constraints.md → Get team capacity
├─ Read previous sprints → Calculate velocity
├─ Read backlog.md → Get available stories
└─ Read progress → Check dependencies

Step 2: Select Stories
├─ Filter by priority (P0 first)
├─ Check dependencies (prerequisite stories done?)
├─ Calculate capacity (hours available)
├─ Match to velocity (20 points typical)
└─ Balance workload (distribute evenly)

Step 3: Create Sprint Plan
├─ Set sprint goal (1 sentence)
├─ List selected stories (detailed)
├─ Break down into tasks
├─ Assign to team members
└─ Include acceptance criteria

Step 4: Generate File
└─ output/sprints/sprint-N.md
```

---

### Implementation Phase

#### What Claude Does During Feature Development

```
Step 1: Read Technical Spec
├─ Check architecture patterns
├─ Review API conventions
├─ Check database schema
└─ Understand error handling

Step 2: Read Coding Standards
├─ .claude/rules/code-quality.md → SOLID & DRY
├─ .claude/rules/testing.md → Testing requirements
├─ .claude/rules/git.md → Git conventions
└─ .claude/rules/database.md → Migration workflow

Step 3: Use TodoWrite
├─ Break user story into tasks
├─ Mark task as in_progress
├─ Implement following standards
├─ Mark task as completed
└─ Update ALL tasks before story complete

Step 4: Enforce Quality Gates
Before marking story complete, verify:
├─ [ ] Code follows SOLID & DRY
├─ [ ] All tests passing (200/400/401/403/404/500)
├─ [ ] Technical spec consulted
├─ [ ] Documentation updated
├─ [ ] Translations added (if I18N-RULES.md exists)
└─ [ ] Project-specific tests implemented (if TESTING-RULES.md exists)
```

---

### TodoWrite vs /plan-sprint

**Key Difference:**

| Aspect | TodoWrite | /plan-sprint |
|--------|-----------|--------------|
| **Scope** | Single user story | Entire sprint (10-20 stories) |
| **Granularity** | Implementation tasks | User stories |
| **Duration** | Hours to days | 2 weeks |
| **Tool** | TodoWrite tool | Slash command |
| **When** | During implementation | Sprint planning |
| **Output** | Task list (transient) | Sprint file (permanent) |

**Example:**

**Sprint Plan (high-level):**
```
Sprint 2:
- US-015: Real-time Notifications (8 points)
- US-016: Team Dashboard (5 points)
- US-020: Email Digests (5 points)
- US-023: Dark Mode (2 points)
```

**TodoWrite (detailed tasks for US-015):**
```
TodoWrite for US-015:
1. Create WebSocket server setup
2. Implement notification queue
3. Build notification component
4. Add database triggers
5. Write unit tests (8 tests)
6. Write integration tests (3 tests)
7. Write e2e test (1 test)
8. Update technical spec with WebSocket architecture
9. Update API docs
```

---

### Quality Gates

#### What Prevents Task Completion

Claude checks **Master Checklist** before marking any task complete:

```markdown
**Before marking ANY task complete:**

**Code:**
- [ ] SOLID & DRY principles followed
- [ ] No TypeScript/linting errors
- [ ] Follows project conventions

**Testing:**
- [ ] All tests passing (unit, integration, e2e)
- [ ] All API codes tested (200/400/401/403/404/500)

**Documentation:**
- [ ] Technical spec consulted before implementation
- [ ] Technical spec updated (if architecture/schema changed)
- [ ] API docs updated (if applicable)
- [ ] README/CHANGELOG updated
- [ ] User guide updated (if user-facing feature)
- [ ] Translations added (if i18n required)

**Security & Quality:**
- [ ] No secrets committed
- [ ] No security vulnerabilities
- [ ] No over-engineering
```

**If ANY checkbox fails → Task NOT complete!**

---

#### Example: Task Marked Incomplete

```
You: "US-015 is done"

Claude checks:
✅ Code follows SOLID & DRY
✅ No TypeScript errors
✅ All tests passing (12/12)
✅ Tech spec consulted
❌ API docs NOT updated (WebSocket endpoint missing)
❌ Translations NOT added (notification text hardcoded)

Claude: "Task is NOT complete. Missing:
1. API docs need WebSocket endpoint documentation
2. Translations needed for notification messages"

Status: in_progress (not completed)
```

---

### Decision Trees

#### "Am I Planning or Implementing?"

```
Question: Should I use TodoWrite or /plan-sprint?

Start
  ↓
  Am I starting a new sprint?
  ├─ YES → Use /plan-sprint [N]
  └─ NO
      ↓
      Am I implementing a specific user story?
      ├─ YES → Use TodoWrite
      └─ NO
          ↓
          Am I tracking daily progress?
          └─ YES → Use /update-progress
```

---

#### "Which Document Should I Read?"

```
Question: I need to implement a feature

Start
  ↓
  Do I understand WHAT to build?
  ├─ NO → Read: scope.md, backlog.md
  └─ YES
      ↓
      Do I understand HOW to build it?
      ├─ NO → Read: technical-spec.md
      └─ YES
          ↓
          Do I know the coding standards?
          ├─ NO → Read: .CLAUDE.MD, .claude/rules/*.md
          └─ YES
              ↓
              START IMPLEMENTING
```

---

## FAQ & Troubleshooting

### Setup Issues

#### "Commands not found"

**Symptoms:**
```bash
/init-project
# Command not recognized
```

**Cause:** Commands not in `.claude/commands/` folder

**Solution:**
```bash
# Check if commands exist
ls .claude/commands/

# If missing, copy from template
cp -r /path/to/claude_repo/.claude .
```

---

#### "Files not generating"

**Symptoms:** `/init-project` runs but no output files created

**Cause:** Input files have placeholders or are empty

**Solution:**
```bash
# Check input files
cat .project-management/input/scope.md

# If you see {{PLACEHOLDER}}, fill them:
vim .project-management/input/scope.md
# Replace all {{PLACEHOLDERS}} with actual content
```

---

#### "Wrong file structure"

**Symptoms:** Commands fail with "File not found"

**Cause:** Incorrect folder structure

**Solution:**
```bash
# Verify structure
tree -L 3 .project-management/

# Should have:
# .project-management/
# ├── input/
# ├── output/
# ├── templates/
# ├── rules/
# └── client-input/

# If missing, recreate:
mkdir -p .project-management/{input,output,templates,rules,client-input}
```

---

### Planning Issues

#### "Sprint seems unrealistic"

**Symptoms:** Sprint has too many/few stories

**Cause:** Velocity or capacity incorrect

**Solution:**
```bash
# Check constraints
cat .project-management/input/constraints.md

# Update team capacity:
## Team
**Capacity:** 80 hours/week  ← Adjust to actual

# Replan sprint
/plan-sprint 2
```

---

#### "Priorities confusing"

**Symptoms:** P0 stories not in sprint, P3 stories included

**Cause:** Backlog priorities incorrect

**Solution:**
```bash
# Review backlog priorities
cat .project-management/input/backlog.md

# Fix priorities:
# P0 = Must have (critical)
# P1 = Should have (important)
# P2 = Nice to have (optional)
# P3 = Future (not this release)

# Regenerate sprint
/plan-sprint 2
```

---

### Implementation Issues

#### "Claude doesn't know what to do"

**Symptoms:** Claude asks many clarifying questions during implementation

**Cause:** Technical spec missing or vague

**Solution:**
```bash
# Check if technical spec exists
cat .project-management/output/docs/technical-spec.md

# If missing, generate:
/init-project

# If vague, add details to input files:
vim .project-management/input/backlog.md
# Add detailed acceptance criteria

/generate-docs
```

---

#### "Standards conflicting"

**Symptoms:** Claude mentions conflicting rules

**Cause:** `.CLAUDE.MD` and `project-rules.md` have different standards

**Solution:**
```
Priority order (project-rules.md wins):
project-rules.md > .CLAUDE.MD

Action:
1. Decide which standard to use
2. Update project-rules.md with clear override
3. Document WHY (so team understands)
```

---

#### "Tech spec out of date"

**Symptoms:** Tech spec doesn't match current implementation

**Cause:** Scope changed but docs not regenerated

**Solution:**
```bash
# Update input files with changes
vim .project-management/input/backlog.md

# Regenerate documentation
/generate-docs

# Verify tech spec updated
git diff .project-management/output/docs/technical-spec.md
```

---

### Documentation Issues

#### "Docs have placeholders"

**Symptoms:** Generated docs show `{{PLACEHOLDER}}`

**Cause:** Input files not fully filled

**Solution:**
```bash
# Find all placeholders
grep -r "{{" .project-management/input/

# Fill each one
vim .project-management/input/scope.md
# Replace {{PROJECT_NAME}} with actual name

# Regenerate
/generate-docs
```

---

#### "Docs out of sync"

**Symptoms:** PRD says one thing, technical spec says another

**Cause:** Manual edits to output docs

**Solution:**
```
⚠️ Rule: NEVER manually edit output/ docs!

Correct approach:
1. Edit input files (input/*.md)
2. Run /generate-docs
3. Output files regenerated consistently

If you need permanent changes:
→ Edit templates/ instead
```

---

### Integration Issues

#### "Client docs not processing"

**Symptoms:** `/process-client-docs` finds no documents

**Cause:** Documents in wrong folder

**Solution:**
```bash
# Check correct path
ls .project-management/client-input/

# If empty, add documents
cp ~/Downloads/client-brief.pdf .project-management/client-input/

# Retry
/process-client-docs
```

---

#### "Extraction incomplete"

**Symptoms:** Only some features extracted from client docs

**Cause:** Complex document structure

**Solution:**
```bash
# Add more documents for context
cp meeting-notes.txt .project-management/client-input/
cp mockups.png .project-management/client-input/

# Re-run (it merges new info)
/process-client-docs

# Manually add missing features
vim .project-management/input/backlog.md
```

---

### Progress Tracking

#### "Velocity calculation wrong"

**Symptoms:** Velocity doesn't match actual completion

**Cause:** Story points changed after sprint started

**Solution:**
```
⚠️ Rule: Don't change story points mid-sprint!

If points change:
1. Note in sprint retrospective
2. Use original points for velocity
3. Update backlog for next sprint
```

---

#### "Completed work missing"

**Symptoms:** `/project-status` doesn't show completed stories

**Cause:** Forgot to run `/update-progress`

**Solution:**
```bash
# Update progress immediately
/update-progress

# Confirm in report
/project-status
```

---

## Best Practices

### Project Setup Best Practices

#### 1. Start with Client Docs When Possible

**Why:** Saves 5+ hours of manual transcription

**How:**
```bash
# 1. Collect ALL client materials
# 2. Organize with numbered prefixes
cp 01-brief.pdf client-input/
cp 02-mockups.png client-input/
cp 03-notes.txt client-input/

# 3. Process
/process-client-docs

# 4. Review & refine
vim .project-management/input/backlog.md
```

---

#### 2. Be Specific in Scope

**❌ Bad:**
```markdown
## Project Vision
Build a better task manager
```

**✅ Good:**
```markdown
## Project Vision
Build a task management app for remote teams of 5-50 people that:
- Emphasizes simplicity (zero training required)
- Supports real-time collaboration
- Handles 100+ concurrent users with <200ms response time
- Integrates with Slack and Microsoft Teams

**Differentiator:** Simplest UI in the market, specifically designed for non-technical users
```

**Why:** Specific scope generates specific documentation.

---

#### 3. Prioritize Ruthlessly

**Rule:** If you have 40 user stories, 25% should be P0 (10 stories max)

**Good distribution:**
- P0 (Must Have): 25% (critical features)
- P1 (Should Have): 35% (important features)
- P2 (Nice to Have): 30% (enhancements)
- P3 (Future): 10% (future roadmap)

**Why:** Prevents scope creep, focuses MVP.

---

#### 4. Estimate Conservatively

**First project with team:** Use 60-70% of capacity for first sprint

**Example:**
```markdown
Team Capacity: 80 hours/week
First Sprint: Plan 50-60 hours (12-15 points)

Why: Unknown velocity, learning curve, unexpected issues
```

After 2-3 sprints, adjust to 80-90% capacity.

---

#### 5. Document Constraints Honestly

**Don't:**
```markdown
**Budget:** $100,000  ← Aspirational
**Timeline:** 6 months ← Hopeful
```

**Do:**
```markdown
**Budget:** $60,000 (hard cap, cannot exceed)
**Timeline:** 4 months (client deadline, non-negotiable)
**Risks:** Tight timeline, need to descope if velocity < 20 pts/sprint
```

**Why:** Claude plans realistically when given realistic constraints.

---

### Sprint Planning Best Practices

#### 1. First Sprint: 60-70% Capacity

**Why:** Unknown velocity, team learning curve

**Example:**
```bash
Team: 80 hours/week capacity
First Sprint: Plan 12-15 points (vs. 20 typical)

After Sprint 1: Measure actual velocity
- If completed 18 points → velocity = 18
- Sprint 2: Plan 18-20 points
```

---

#### 2. Balance Risk Across Sprint

**Don't:** Pack all P0 stories in one sprint

**Do:**
```
Sprint 2 Plan:
- US-015: Real-time Notifications (8 pts) - P0 [HIGH RISK]
- US-016: Team Dashboard (5 pts) - P0 [LOW RISK]
- US-020: Email Digests (5 pts) - P1 [LOW RISK]
- US-023: Dark Mode (2 pts) - P2 [LOW RISK]

Balance: 1 high-risk, 3 low-risk stories
```

**Why:** Reduces sprint failure risk.

---

#### 3. Check Dependencies

**Before sprint planning:**
```bash
# Review backlog for dependencies
cat .project-management/input/backlog.md | grep "Dependencies"

# Example:
US-020: Dependencies: US-015 (notifications must work first)

# Rule: Don't plan US-020 until US-015 is complete
```

---

#### 4. Set Clear Sprint Goals

**❌ Bad:**
```
Sprint 2 Goal: Complete 4 stories
```

**✅ Good:**
```
Sprint 2 Goal: Enable real-time collaboration with notifications and team dashboard
```

**Why:** Focused goal guides decisions during sprint.

---

#### 5. Review Velocity Trends

**Look for patterns:**
```
Sprint 1: 18 points
Sprint 2: 22 points  ← Increasing (good, team improving)
Sprint 3: 15 points  ← Drop! (investigate)

Questions to ask when velocity drops:
- Blockers encountered?
- Scope creep in stories?
- Team availability changed?
- Story points underestimated?
```

---

### Development Best Practices

#### 1. Read Tech Spec Before Implementing

**Every time:**
```bash
# Before starting US-015
cat .project-management/output/docs/technical-spec.md

# Check:
- Architecture patterns
- API conventions
- Database schema
- Error handling
```

**Why:** Tech spec is source of truth for HOW to build.

---

#### 2. Use TodoWrite for Features

**When starting user story:**
```
1. Read story acceptance criteria
2. Create TodoWrite list with tasks
3. Mark first task in_progress
4. Implement
5. Mark completed
6. Move to next task

Rule: Only ONE task in_progress at a time
```

**Why:** Tracks progress, prevents context switching.

---

#### 3. Update Progress Frequently

**Recommended:** Daily at end of day

```bash
# End of each day
/update-progress

# Report:
- Stories completed today
- Test counts
- Blockers (if any)
```

**Why:** Real-time visibility, early blocker detection.

---

#### 4. Log Blockers Immediately

**Don't wait:**
```bash
# As soon as you discover blocker
/update-progress

Blocker: BLOCK-003
Impact: High
Description: Third-party API rate limit exceeded
ETA Resolution: 2 days
Mitigation: Using cached data for now
```

**Why:** Enables proactive solutions, communicates risks early.

---

#### 5. Follow Coding Standards

**Claude enforces:**
- SOLID & DRY (`.claude/rules/code-quality.md`)
- Testing (`.claude/rules/testing.md`)
- Git (`.claude/rules/git.md`)
- Database (`.claude/rules/database.md`)

**Your job:** Review and understand these standards.

**Claude's job:** Enforce them during development.

---

### Documentation Best Practices

#### 1. Keep Docs in Sync

**Rule:** Regenerate docs immediately after scope changes

```bash
# Client adds feature
vim .project-management/input/backlog.md
# Add US-030: Export to PDF

# Regenerate IMMEDIATELY
/generate-docs

# Verify
git diff .project-management/output/docs/
```

**Why:** Out-of-date docs lead to wrong implementations.

---

#### 2. Update After Scope Changes

**Scope change workflow:**
```
1. Document change in input files
2. Run /generate-docs
3. Review impact in output docs
4. Communicate to stakeholders
5. Replan sprints if needed
```

---

#### 3. Use as Reference During Dev

**Before implementing:**
```bash
# Read technical spec section
grep -A 20 "API Endpoints" .project-management/output/docs/technical-spec.md

# Follow documented patterns
```

**Don't:** Implement from memory or assumptions.

---

#### 4. Version Control Everything

**Commit to git:**
```bash
git add .project-management/
git commit -m "docs: update project documentation with new features"
```

**Why:** Team sees docs, can review, history tracked.

---

#### 5. Share with Team

**Daily:**
- Sprint plan (who's working on what)
- Progress tracker (current status)

**Weekly:**
- Project status report
- Velocity trends
- Blockers

**Why:** Transparency builds trust, enables collaboration.

---

### Client Communication

#### 1. Process Docs Immediately

**When client sends materials:**
```bash
# Same day
cp client-materials/* .project-management/client-input/
/process-client-docs

# Review
cat .project-management/input/backlog.md

# Send back
"We extracted 25 user stories. Please review and confirm priorities."
```

**Why:** Shows responsiveness, validates understanding early.

---

#### 2. Ask Clarifying Questions

**Don't assume:**
```
After /process-client-docs:

Check for [NEEDS CLARIFICATION] markers:

US-010: Payment Integration
**As a** user
**I want to** pay with credit card
**So that** I can complete purchase

[NEEDS CLARIFICATION: Which payment provider - Stripe, PayPal, or both?]

Ask client before proceeding.
```

---

#### 3. Share Generated Plans

**After `/init-project`:**
```bash
# Generate PDF (or share markdown)
# Send to client:
- PRD (product vision)
- Timeline (phases and milestones)
- Sprint 1 plan

"Here's our plan based on your brief. Please review and confirm."
```

**Why:** Aligns expectations, catches misunderstandings early.

---

#### 4. Weekly Status Updates

**Every Friday:**
```bash
/project-status

# Send report with:
- Sprint progress (%)
- Completed stories
- Upcoming stories
- Blockers (if any)
- Next week plan
```

**Why:** Transparent communication builds trust.

---

#### 5. Transparent Blocker Communication

**Don't hide blockers:**
```
Email to client:

"Update: We've encountered a blocker (BLOCK-003) with the payment API.

Impact: 2-day delay on payment integration (US-010)
Mitigation: Completed UI work, integration when API available
Revised Timeline: Sprint 3 completion now April 17 (was April 15)

Questions?"
```

**Why:** Clients appreciate honesty, can help resolve blockers.

---

### Team Collaboration

#### 1. Daily Standup with Sprint File

**Each morning:**
```bash
# Open sprint file
cat .project-management/output/sprints/sprint-2.md

Standup format:
- Yesterday: US-015 completed (tested, docs updated)
- Today: Starting US-016 (team dashboard)
- Blockers: None

# All team members reference same sprint file
```

**Why:** Single source of truth, aligned focus.

---

#### 2. Shared Access to Docs

**Setup:**
```bash
# Commit docs to git
git add .project-management/output/
git commit -m "docs: sprint 2 plan"
git push

# Team pulls
git pull

# Everyone sees same docs
```

**Why:** No "but I didn't know" excuses.

---

#### 3. Consistent Updates

**Team rule:**
```
Every team member runs /update-progress daily

Result:
- Project status always current
- Velocity calculated accurately
- Blockers visible immediately
```

---

#### 4. Code Review with Standards

**During PR review:**
```bash
# Reviewer checks:
cat .claude/rules/code-quality.md
cat .claude/rules/testing.md

# Verify:
- [ ] SOLID & DRY followed?
- [ ] All 6 API status codes tested?
- [ ] Documentation updated?

# Reference in PR comments:
"Missing test for 404 status (see .claude/rules/testing.md)"
```

**Why:** Standards-based reviews, not subjective opinions.

---

#### 5. Retrospectives with Data

**End of sprint:**
```bash
/project-status

Review:
- Velocity (planned vs actual)
- Test metrics
- Blocker count
- Story completion rate

Discuss:
- What went well
- What to improve
- Action items
```

**Why:** Data-driven improvements.

---

### What NOT to Do (Anti-patterns)

#### ❌ 1. Don't Skip Reading Tech Spec

**Bad:**
```
Developer: "I'll just implement this feature based on the user story"
[Implements using wrong patterns, different API conventions]
```

**Good:**
```
Developer: "Let me check the tech spec first"
cat .project-management/output/docs/technical-spec.md
[Follows documented patterns, consistent with project]
```

---

#### ❌ 2. Don't Over-Commit in Sprints

**Bad:**
```
Team velocity: 20 points
Sprint plan: 30 points  ← 50% over capacity!
Result: Sprint failure, burnout
```

**Good:**
```
Team velocity: 20 points
Sprint plan: 18-22 points  ← Within capacity
Result: Sprint success, sustainable pace
```

---

#### ❌ 3. Don't Hide Blockers

**Bad:**
```
Developer: [Blocked for 3 days, doesn't report]
Sprint end: "Sorry, couldn't finish, was blocked"
```

**Good:**
```
Developer: [Blocked day 1]
/update-progress → Logs blocker immediately
Team/client: Helps resolve blocker
Result: Blocker resolved in 1 day
```

---

#### ❌ 4. Don't Let Docs Go Stale

**Bad:**
```
Week 1: Generate docs
Week 5: [Scope changed, docs not updated]
Week 8: Tech spec doesn't match implementation
Result: Confusion, wrong implementations
```

**Good:**
```
Week 1: Generate docs
Week 5: [Scope changed]
→ /generate-docs immediately
Week 8: Docs match implementation perfectly
```

---

#### ❌ 5. Don't Mix Planning and Implementation

**Bad:**
```
Developer: [Working on US-015]
Also: Planning Sprint 3
Also: Updating progress
Result: Context switching, slower progress
```

**Good:**
```
Planning time: /plan-sprint 3 (dedicated time)
Development time: Implement US-015 (focused)
Update time: /update-progress at end of day
Result: Focused work, faster progress
```

---

#### ❌ 6. Don't Ignore Testing Requirements

**Bad:**
```
Developer: "Tests done"
Only tested: 200 OK
Missing: 400, 401, 403, 404, 500
Result: Production bugs
```

**Good:**
```
Developer: "Tests done"
Tested: ALL 6 status codes (per .claude/rules/testing.md)
Result: Robust, production-ready code
```

---

### Real Examples

#### Example 1: Reducing Sprint Failure

**Before:**
```
Team: New to Agile
Sprint 1-3: 40% failure rate (stories incomplete)
Problem: Over-committing (30 points when velocity = 18)
```

**After implementing system:**
```
Sprint 4: /plan-sprint 4
→ Claude plans 18 points (matches velocity)
→ All stories completed

Sprint 5-10: 10% failure rate
Improvement: 40% → 10% (75% reduction)
```

**Key change:** Let Claude plan based on velocity, not wishful thinking.

---

#### Example 2: Client Doc Processing

**Before:**
```
Client sends 40-page RFP
Manual process: 8 hours to extract requirements
Result: Missed 3 features, wrong priorities on 5 features
```

**After:**
```
/process-client-docs
Time: 15 minutes (including review)
Result: All 28 features extracted correctly, priorities accurate
Savings: 7.75 hours (97% reduction)
```

---

#### Example 3: Onboarding New Developer

**Before:**
```
New developer joins team
Reads scattered docs, asks many questions
Productive: Week 3

Time to productivity: 3 weeks
```

**After:**
```
New developer joins team
Reads:
1. USER-GUIDE.md (this file)
2. Technical spec (generated)
3. Current sprint plan

Productive: Day 2

Time to productivity: 2 days
Improvement: 15x faster
```

**Key:** Comprehensive, generated docs make onboarding instant.

---

## Additional Resources

### Documentation Files

- **INTEGRATION-GUIDE.md** - How all pieces fit together
- **SYSTEM-OVERVIEW.md** - Complete file map
- **WHATS-NEW.md** - Latest features
- **rules/README.md** - Modular rules system explained
- **rules/I18N-SETUP.md** - Internationalization setup guide
- **rules/I18N-RULES.md** - i18n requirements (if enabled)
- **rules/TESTING-RULES.md** - Custom testing rules (if enabled)

### Example Files

- **client-input/EXAMPLE-project-brief.txt** - Sample client brief
- **input/*.md** - Templates with examples

### Online Resources

- **Claude Code Docs:** https://code.claude.com/docs
- **Project Management Best Practices:** (your company wiki)

---

## Need Help?

### Getting Support

1. **Read FAQ** (above) for common issues
2. **Check INTEGRATION-GUIDE.md** for system details
3. **Review SYSTEM-OVERVIEW.md** for file locations
4. **Ask team lead** for project-specific questions

---

## Quick Reference Cards

### Command Cheat Sheet

```
/process-client-docs  → Extract from PDF/Word/images
/init-project         → Generate initial documentation
/generate-docs        → Regenerate after changes
/plan-sprint [N]      → Plan sprint N
/update-progress      → Log completed work
/project-status       → Generate status report
```

---

### File Location Cheat Sheet

```
Input (you edit):
.project-management/input/scope.md
.project-management/input/backlog.md
.project-management/input/technologies.md
.project-management/input/constraints.md

Output (generated):
.project-management/output/docs/prd.md
.project-management/output/docs/technical-spec.md
.project-management/output/sprints/sprint-N.md
.project-management/output/progress/progress-*.md

Standards (mandatory):
.CLAUDE.MD
.claude/rules/code-quality.md
.claude/rules/testing.md
.claude/rules/git.md
.claude/rules/database.md
.project-management/rules/project-rules.md

Standards (conditional - only if enabled):
.project-management/rules/I18N-RULES.md
.project-management/rules/TESTING-RULES.md
```

---

### Priority Cheat Sheet

```
P0 = Must Have (critical, blocks launch)
P1 = Should Have (important, high value)
P2 = Nice to Have (optional, enhances UX)
P3 = Future (roadmap, not this release)

Distribution:
P0: 25% of stories
P1: 35% of stories
P2: 30% of stories
P3: 10% of stories
```

---

### Story Point Cheat Sheet

```
Fibonacci Scale:
1 = Trivial (1-2 hours)
2 = Simple (2-4 hours)
3 = Easy (4-8 hours)
5 = Medium (8-16 hours)
8 = Complex (16-24 hours)
13 = Very Complex (24-40 hours)
21 = Epic (break down!)

Rule: Stories > 13 points should be broken down
```

---

**Version:** 1.0
**Last Updated:** 2026-03-25
**Feedback:** Report issues or suggestions to your team lead

---

**🎉 Congratulations!** You now have everything you need to use the Claude Project Management System effectively. Start with the Quick Start, refer back to specific sections as needed, and enjoy autonomous project planning! 🚀
