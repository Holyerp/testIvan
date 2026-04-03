# Commands Reference

**Complete reference for all Claude Project Management System commands.**

**Version:** 3.0.0
**Last Updated:** 2026-03-27

---

## Table of Contents

1. [Command Overview](#command-overview)
2. [/init-project](#init-project)
3. [/execute-work](#execute-work)
4. [/add-scope](#add-scope)
5. [/run-tests](#run-tests)
6. [/update-progress](#update-progress)
7. [/project-status](#project-status)
8. [/process-client-docs](#process-client-docs)
9. [/generate-docs](#generate-docs)

---

## Command Overview

### Core Commands

| Command | Purpose | Frequency |
|---------|---------|-----------|
| `/init-project` | Initialize project with tech stack and phases | One-time |
| `/execute-work` | Automated implementation with testing | Daily |
| `/run-tests` | Manual test execution | As needed |
| `/update-progress` | Manual progress tracking | Daily |
| `/project-status` | Generate status report | Weekly |

### Scope Management

| Command | Purpose | Frequency |
|---------|---------|-----------|
| `/add-scope` | Add or edit phases, epics, stories | When scope changes |

### Setup Commands

| Command | Purpose | Frequency |
|---------|---------|-----------|
| `/process-client-docs` | Extract requirements from documents | One-time |
| `/generate-docs` | Regenerate project documentation | As needed |

---

## /init-project

### Purpose
Initialize project with tech stack selection, i18n configuration, and phase structure generation.

### Usage
```bash
/init-project
```

### Interactive Flow

**Step 1: Tech Stack Selection**
```
Choose tech stack approach:

[1] Default HolyEstate Stack
    - React 19 + React Router 7 (SSR)
    - PostgreSQL 16 + Prisma 6.19.0
    - Vitest 4.0 + Playwright 1.58.0
    - Production-tested (848 tests passing)

[2] AI Recommendation
    - Claude analyzes your project needs
    - Suggests optimal technology choices
    - Explains reasoning

[3] Custom Setup
    - Answer 8 detailed questions
    - Full control over all choices
```

**Step 2: i18n Configuration**
```
Enable internationalization (i18n)?

[1] Yes - Configure multi-language support
    → Select languages (en, de, sr, fr, es, etc.)
    → Creates I18N-RULES.md
    → Sets up translation structure

[2] No - Skip i18n (single language only)
```

### What It Does

1. **Reads input files:**
   - `.project-management/input/scope.md`
   - `.project-management/input/backlog.md`
   - `.project-management/input/technologies.md`
   - `.project-management/input/constraints.md`

2. **Generates documentation:**
   - PRD (Product Requirements Document)
   - Technical Specification
   - Architecture Documentation

3. **Creates phase structure:**
   - Phase 1: Foundation & Setup
   - Phase 2: Core Features
   - Phase 3: Advanced Features
   - Phase 4: Polish & Launch

4. **Initializes progress tracking:**
   - Current status file
   - Completed work log
   - Blockers log

### Output Files
```
.project-management/output/
├── docs/
│   ├── prd.md
│   ├── technical-spec.md
│   └── architecture.md
├── phases/
│   ├── phase-1.md
│   ├── phase-2.md
│   ├── phase-3.md
│   └── phase-4.md
└── progress/
    ├── current-status.md
    ├── completed.md
    └── blockers.md

.project-management/rules/
└── I18N-RULES.md (if i18n enabled)
```

### When to Use
- Starting a new project
- Regenerating project structure
- Switching tech stacks

---

## /execute-work

### Purpose
Automated implementation with mandatory plan mode, testing, git commits, and progress tracking.

### Usage
```bash
/execute-work phase 1        # Execute entire Phase 1
/execute-work epic Epic-2    # Execute specific epic
/execute-work story US-015   # Execute single story
```

### Workflow

**Step 1: Mode Selection**
```
Choose execution mode:

[1] Continuous
    - No pauses between stories
    - Fully automated
    - Best for: Well-defined work

[2] Paused
    - Wait for approval after each story
    - Manual control
    - Best for: Complex features, learning
```

**Step 2: Plan Mode (Automatic)**
```
📋 [PLAN MODE ACTIVATED]

Claude:
1. Reads all context (tech spec, backlog, rules)
2. Analyzes scope (stories, dependencies, risks)
3. Creates detailed plan (tasks, tests, estimates)
4. Shows plan for approval

You:
[Yes] - Proceed with implementation
[No] - Cancel execution
[Revise] - Modify plan
```

**Step 3: Implementation (For Each Story)**
```
🚀 [IMPLEMENTATION MODE]

For each story, Claude:
1. Breaks down into tasks (TodoWrite)
2. Implements following standards
3. Writes tests (unit, integration, E2E)
4. Runs tests automatically
5. Validates quality gates
6. Creates git commit (NO AI credits)
7. Updates progress tracking
8. Continues to next story (or pauses)
```

**Step 4: Completion Report**
```
🎉 [PHASE/EPIC/STORY COMPLETED]

Statistics:
- Stories completed
- Story points
- Tests written/passing
- Code coverage
- Git commits
- Duration
- Velocity

Next Steps:
- Continue to next phase/epic
- Review completed work
```

### Quality Gates (Automatic Validation)

Story is NOT marked complete until:
- ✅ All tests passing (unit, integration, E2E)
- ✅ Coverage > 80%
- ✅ All API status codes tested (200/400/401/403/404/500)
- ✅ i18n translations added (if enabled)
- ✅ SOLID & DRY principles followed
- ✅ Git commit created (NO AI credits)
- ✅ Progress tracking updated

### Examples

**Execute entire phase:**
```bash
/execute-work phase 1
# → Implements all epics and stories in Phase 1
# → Can take hours/days depending on scope
# → Use continuous mode for automation
```

**Execute single epic:**
```bash
/execute-work epic Epic-2
# → Implements all stories in Epic-2
# → More focused than full phase
# → Good for testing the system
```

**Execute single story:**
```bash
/execute-work story US-015
# → Implements only US-015
# → Best for learning, complex stories
# → Full control
```

### When to Use
- Daily development work
- Automated implementation
- Enforcing quality gates
- Consistent git commits

---

## /add-scope

### Purpose
Add or edit phases, epics, or stories with automatic renumbering and cross-reference updates.

### Usage
```bash
# ADD new items
/add-scope add phase [position] [--from path/to/file.md]
/add-scope add epic [phase-number] [position] [--from path/to/file.md]
/add-scope add story [phase-number] [epic-number] [--from path/to/file.md]

# EDIT existing items
/add-scope edit phase [phase-number] [--from path/to/file.md]
/add-scope edit epic [phase-number] [epic-number] [--from path/to/file.md]
/add-scope edit story [US-XXX] [--from path/to/file.md]
```

### Workflow

**Step 1: Parse & Validate**
```
Claude:
1. Parses action (add/edit) and scope (phase/epic/story)
2. Validates project is initialized (phase files exist)
3. Reads content from --from file or asks user to describe
4. Reads current project state (all phases, backlog, max IDs)
```

**Step 2: Placement (add only)**
```
Claude shows existing items:
  [1] Phase 1: Foundation (19 points)
  [2] Phase 2: Core Features (42 points)
  [3] Phase 3: Advanced Features (26 points)
  [4] → Append at end

You: Select position (or omit for append)
```

**Step 3: Preview & Confirmation (MANDATORY)**
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SCOPE CHANGE PREVIEW

ACTION: Add
TYPE: Phase
TARGET: Position 2

RENUMBERING:
- Phase 2 "Core Features" → Phase 3
- Phase 3 "Advanced Features" → Phase 4

NEW STORY IDs: US-019, US-020

Proceed? [Yes / No / Revise]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

**Step 4: Execute, Verify & Document**
```
Claude:
1. Applies changes (renaming, inserting, updating)
2. Runs 5 integrity checks (phase continuity, ID uniqueness, etc.)
3. Asks: auto-update docs now or run /generate-docs later?
4. Shows summary report
```

### What It Updates
- Phase files (`phase-*.md`) — rename, content, cross-references
- Backlog (`backlog.md`) — new/updated epics and stories
- Progress (`current-status.md`) — metrics
- Optionally: PRD, tech-spec, architecture (if auto-update chosen)

### Key Rules
- US-XXX story IDs are **never renumbered** (immutable identifiers, zero-padded: US-005)
- Phase files renamed from **highest to lowest** (prevents collisions)
- Story point changes **cascade**: story → epic total → phase total (increases AND decreases)
- Preview is **mandatory** — no changes without user approval
- Epic numbering: **LOCAL** in phase files, **GLOBAL** in backlog
- Story points: **Fibonacci scale only** (1, 2, 3, 5, 8, 13, 21)
- STEP 5 is **atomic** — if any sub-step fails, entire step aborts

### Examples

**Add a new phase at position 2:**
```bash
/add-scope add phase 2
# → Shifts Phase 2→3, Phase 3→4, Phase 4→5
# → Creates new Phase 2 from your description
# → Updates backlog with new epics/stories
```

**Add epic from a file:**
```bash
/add-scope add epic 1 --from docs/notification-epic.md
# → Reads description from file
# → Adds to Phase 1 with auto-assigned story IDs
```

**Edit story points (cascading update):**
```bash
/add-scope edit story US-005
# → Shows current content, asks what to change
# → If points change: updates epic total → phase total → backlog
```

### When to Use
- Adding new features/phases to existing project
- Modifying scope after client feedback
- Restructuring phases or epics
- Updating story details (points, criteria, priority)

---

## /run-tests

### Purpose
Manually execute tests with detailed reporting and analysis.

### Usage
```bash
/run-tests all                # All tests (unit + integration + E2E)
/run-tests unit               # Unit tests only
/run-tests integration        # Integration tests only
/run-tests e2e                # E2E tests only
/run-tests coverage           # All tests with coverage report
/run-tests story US-015       # Tests for specific story
/run-tests file src/api.ts    # Tests for specific file
```

### Test Types

**Unit Tests (`/run-tests unit`)**
- Test individual functions/components
- Fast execution (seconds)
- No external dependencies
- Command: `npm test` or `npm run test:unit`

**Integration Tests (`/run-tests integration`)**
- Test API endpoints
- Database interactions
- Verify status codes (200/400/401/403/404/500)
- Command: `npm run test:integration`

**E2E Tests (`/run-tests e2e`)**
- Test user flows
- Browser automation (Playwright)
- Full application testing
- Command: `npm run test:e2e` or `npx playwright test`

**Coverage (`/run-tests coverage`)**
- All tests with coverage report
- HTML report generated
- Check coverage threshold (80%+)
- Command: `npm run test:coverage`

### Output Format

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🧪 TEST RESULTS
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 SUMMARY:
✅ Unit Tests: 45/45 passed
✅ Integration Tests: 18/18 passed
✅ E2E Tests: 6/6 passed
⏱️  Duration: 12.3s
📈 Coverage: 87% (Target: 80%+)

✅ ALL TESTS PASSED

✅ QUALITY GATES:
- Coverage: ✅ 87% (Target: 80%+)
- API Status Codes: ✅ All tested
- Critical Paths: ✅ E2E tests passing

🎉 Ready to commit!
```

**If tests fail:**
```
❌ FAILED TESTS:
- user.test.ts: Expected 200, got 500
  → File: src/api/user.ts:42
  → Suggestion: Check database connection

- auth.integration.test.ts: Timeout after 5000ms
  → File: src/auth/login.ts
  → Suggestion: Increase timeout or fix async logic
```

### Quality Gates Validation

**Code Coverage:**
- [ ] Overall coverage > 80%
- [ ] Branch coverage > 75%
- [ ] Function coverage > 85%

**API Testing (Integration):**
- [ ] All endpoints have tests
- [ ] All status codes tested (200/400/401/403/404/500)
- [ ] Error handling tested

**Critical Paths (E2E):**
- [ ] Authentication flow tested
- [ ] Main user journeys tested
- [ ] Payment/checkout flow tested (if applicable)

### When to Use
- Before marking story complete
- After implementing features
- Debugging test failures
- Checking coverage
- CI/CD validation

---

## /update-progress

### Purpose
Manually update progress tracking files.

### Usage
```bash
/update-progress
```

### Interactive Flow

Claude asks:
```
1. What has been completed recently?
   → Story IDs and titles

2. What is currently in progress?
   → Story IDs being worked on

3. Any blockers or issues?
   → Description and impact

4. Any bugs discovered?
   → Bug details and severity

5. Changes to timeline or scope?
   → Delays, scope changes
```

### What It Updates

**Phase File (`phase-N.md`):**
- Story statuses (In Progress → Completed)
- Completion timestamps
- Test metrics (tests passed, coverage)
- Git commit hashes
- Phase progress percentage

**Completed Work (`completed.md`):**
```markdown
## Week 12 (2026-03-24 to 2026-03-30)

### Completed Stories
- ✅ US-045: User profile page (5 points)
  - Completed: 2026-03-27
  - Tests: 12 passing
  - Coverage: 89%
  - Commit: 8f074d0

### Key Achievements
- User authentication flow complete
- Profile editing with image upload
```

**Blockers (`blockers.md`):**
```markdown
## Active Blockers

### 🔴 BLK-001: Database migration failing
- **Status:** Active
- **Impact:** High
- **Affected Stories:** US-046, US-047
- **Description:** PostgreSQL version mismatch
- **Reported:** 2026-03-27
- **Action Plan:** Upgrade to PostgreSQL 16
```

**Current Status (`current-status.md`):**
- Overall completion percentage
- Current phase progress
- Velocity (points per week)
- Test coverage
- Bug count
- Timeline status

### When to Use
- End of day updates
- After completing stories manually
- When blockers occur
- Weekly summaries

---

## /project-status

### Purpose
Generate comprehensive project status report.

### Usage
```bash
/project-status
```

### Report Format

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 PROJECT STATUS REPORT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

**Generated:** 2026-03-27
**Project:** E-Commerce Platform
**Current Phase:** Phase 2 - Core Features

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🎯 EXECUTIVE SUMMARY

**Overall Status:** 🟢 On Track

**Key Metrics:**
- Overall Completion: 45%
- Phase 2 Progress: 60%
- Velocity: 25 points/week
- Test Coverage: 87%
- Quality: 🟢 3 bugs

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 📅 CURRENT PHASE STATUS

**Phase 2: Core Features**

Progress: [████████████░░░░░░░░] 60%

- Started: 2026-02-15
- Expected End: 2026-04-30
- Status: 🟢 On Track

**Completed:** 12 / 20 stories
**Story Points:** 65 / 108 points

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## ✅ COMPLETED WORK (Recent)

**Last 7 Days:**
- ✅ US-045: User profile page (5 points)
- ✅ US-046: Payment integration (13 points)

**Key Achievements:**
- User authentication complete
- Payment gateway integrated

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🔴 BLOCKERS & RISKS

**Active Blockers:** 1
- 🔴 Database migration (Impact: High)

**Risks:**
- ⚠️  Phase 2 deadline tight (Probability: Medium)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🧪 QUALITY METRICS

**Testing:**
- Total Tests: 69 (69 passing, 0 failing)
- Coverage: 87% (Target: 80%+)
- API Status Codes: ✅ All tested

**Bugs:**
- Open: 3 (0 Critical, 1 High, 2 Medium)
- Fixed This Week: 5

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🎯 NEXT STEPS

**Immediate (This Week):**
1. Complete US-047 (order tracking)
2. Fix database migration blocker
3. Review Phase 3 backlog
```

### When to Use
- Weekly status updates
- Stakeholder reporting
- Project reviews
- Planning next phases

---

## /process-client-docs

### Purpose
Extract requirements from client documents and generate structured input files.

### Usage
```bash
/process-client-docs
```

### What It Does

1. **Finds and reads documents:**
   - Searches `.project-management/client-input/`
   - Supports: PDF, Word (.docx), Text (.txt, .md), Images (.png, .jpg)
   - Reads all documents completely

2. **Extracts requirements:**
   - Project vision and goals
   - Features (functional & non-functional)
   - Target audience
   - Success criteria
   - Technology preferences
   - Timeline and budget
   - Dependencies, assumptions, risks

3. **Generates input files:**
   - `scope.md` - Project scope and objectives
   - `backlog.md` - User stories organized by epics
   - `technologies.md` - Tech stack and preferences
   - `constraints.md` - Timeline, budget, technical constraints

### When to Use
- Starting with client documents
- Before running `/init-project`
- Converting informal requirements to structured format

---

## /generate-docs

### Purpose
Regenerate project documentation from input files.

### Usage
```bash
/generate-docs
```

### What It Regenerates
- PRD (Product Requirements Document)
- Technical Specification
- Architecture Documentation

### When to Use
- After updating input files
- When requirements change
- To refresh documentation

---

**Version:** 3.0.0
**Last Updated:** 2026-04-02
**Part of:** Claude Project Management System v3.0
