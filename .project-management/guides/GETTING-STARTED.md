# Getting Started with Claude Project Management System

**Quick start guide for new users - get up and running in 5 minutes.**

**Version:** 3.1.0
**Last Updated:** 2026-04-20

---

## Table of Contents

1. [Quick Start (5 Minutes)](#quick-start-5-minutes)
2. [What's New in v3.0](#whats-new-in-v30)
3. [Installation](#installation)
4. [First-Time Setup](#first-time-setup)
5. [Your First Phase](#your-first-phase)
6. [Quick Reference](#quick-reference)

---

## Quick Start (5 Minutes)

### New Project Setup

```bash
# 1. Initialize project
/init-project

# Choose tech stack:
[1] Default HolyEstate Stack (React 19 + RR7 + Prisma)
[2] AI Recommendation (analyzes your needs)
[3] Custom Setup (answer questions)

# Configure i18n:
[1] Yes - Enable multi-language
[2] No - Skip i18n

# 2. Start Phase 1
/execute-work phase 1

# 3. Choose execution mode:
[1] Continuous (no pauses)
[2] Paused (wait after each story)

# 4. Approve plan
[Yes] - Start implementation

# Claude now:
✅ Implements all stories
✅ Runs tests automatically
✅ Creates git commits (no AI credits)
✅ Updates progress tracking
✅ Continues to next story
```

**Result:** Automated development with full quality gates! 🚀

---

## What's New in v3.1

### Major Changes from v3.0

| Feature | v3.0 (Old) | v3.1 (New) |
|---------|------------|------------|
| **Backlog** | Single backlog.md | Modular (phase-1.md, phase-2.md, etc.) |
| **File Size** | 800+ lines | < 250 lines per file (70% smaller!) |
| **Progress View** | Run `/project-status` | Open DASHBOARD.md (instant!) |
| **Updates** | Manual commands | Auto-updates during work |
| **AI Processing** | Read entire backlog | Read only relevant phase (70% faster) |
| **Migration** | N/A | `/migrate-to-modular` command |
| **Templates** | 11 templates | 16 templates (progress tracking added) |

---

### v3.0 Features (Still Available)

| Feature | v2.0 (Old) | v3.0+ |
|---------|------------|-------|
| **Planning** | Sprint-based (2 weeks) | Phase-based (1-4 months) |
| **Execution** | Manual with TodoWrite | Automated with `/execute-work` |
| **Testing** | Manual (npm test) | Automatic (Vitest + Playwright) |
| **Git Commits** | Manual | Automatic (NO AI credits) |
| **Progress Tracking** | Manual updates | Automatic updates |
| **Plan Mode** | Optional | Mandatory before implementation |
| **Tech Stack** | Manual setup | 3 options (Default/AI/Custom) |
| **i18n Support** | Manual config | Setup during init-project |
| **Test Commands** | N/A | `/run-tests` for manual testing |

---

### New Commands in v3.1

**✅ `/migrate-to-modular`**
- Migrate existing project to modular structure
- Splits backlog.md into phase files
- Creates DASHBOARD.md and progress tracking
- Backs up original files
- One command to upgrade!

---

### Commands from v3.0

**✅ `/execute-work phase/epic/story`**
- Automated execution with plan mode
- Auto-testing before marking done
- Auto git commits (following git.md rules)
- Auto progress updates
- Continuous or paused modes

**✅ `/run-tests all/unit/e2e/coverage/story`**
- Manual test execution
- Detailed reporting
- Coverage analysis
- API status code validation

**✅ `/init-project` (Enhanced)**
- Tech stack selection (3 options)
- i18n configuration
- Phase structure generation
- Default HolyEstate stack available

---

### Deprecated Commands

**❌ `/plan-sprint N`** - Replaced by `/execute-work phase N`

---

## Installation

### Prerequisites

- ✅ Git repository
- ✅ Claude Code CLI installed
- ✅ Basic understanding of Agile concepts

---

### Installation Steps

```bash
# 1. Navigate to your project
cd /path/to/your/project

# 2. Copy the system
cp -r /path/to/claude_repo/.project-management .
cp -r /path/to/claude_repo/.claude .
cp /path/to/claude_repo/.CLAUDE.MD .

# 3. Verify structure
ls -la .project-management/

# Expected output:
# .project-management/
# ├── input/           (your requirements)
# ├── output/          (generated docs)
# ├── rules/           (project rules)
# ├── defaults/        (tech stack templates)
# └── guides/          (documentation)
```

---

## Understanding Modular Structure (v3.1)

### What You'll Get

**After running `/init-project`, you'll have:**

```
input/backlog/               ← Organized by phase
├── README.md                ← Quick overview (< 150 lines)
├── phase-1-foundation.md    ← Phase 1 stories only
├── phase-2-core.md          ← Phase 2 stories only
├── phase-3-advanced.md      ← Phase 3 stories only
├── phase-4-polish.md        ← Phase 4 stories only
└── future.md                ← Post-launch features

output/progress/
├── DASHBOARD.md             ← Live view (auto-updates!)
├── daily-summary.md         ← Today's work log
├── weekly-report.md         ← Weekly summary
├── current-status.md        ← Detailed status
├── completed.md             ← Historical log
└── blockers.md              ← Active blockers
```

**Benefits:**
- ✅ Smaller files (< 250 lines each) - easier to read
- ✅ DASHBOARD.md shows live progress - no commands needed
- ✅ 70% faster for AI - processes only relevant phase
- ✅ Auto-updates during work - always current

**Quick tip:** Bookmark `output/progress/DASHBOARD.md` - your at-a-glance project status!

---

## First-Time Setup

### Step 1: Initialize Project

```bash
/init-project
```

**Tech Stack Selection:**
```
[1] Default HolyEstate Stack
    - React 19 + React Router 7 (SSR)
    - PostgreSQL 16 + Prisma 6.19.0
    - Vitest 4.0 + Playwright 1.58.0
    - Production-tested (848 tests passing)
    ✅ Best for: Modern web applications

[2] AI Recommendation
    - Claude analyzes your project
    - Suggests optimal stack
    - Explains technology choices
    ✅ Best for: Uncertain about tech choices

[3] Custom Setup
    - Answer 8 questions
    - Full control over choices
    ✅ Best for: Specific requirements
```

**i18n Configuration:**
```
[1] Yes - Configure i18n
    → Select languages (en, de, sr, fr, etc.)
    → Creates I18N-RULES.md
    → Sets up translation file structure
    ✅ Choose if: Multi-language support needed

[2] No - Skip i18n
    → No translation requirements
    ✅ Choose if: Single language only
```

**Claude generates:**
- ✅ PRD (Product Requirements Document)
- ✅ Technical Specification
- ✅ Architecture Documentation
- ✅ Phase structure (4 phases)
- ✅ Initial progress tracking

---

### Step 2: Review Generated Documentation

```bash
# Check generated docs
ls .project-management/output/docs/
# → prd.md
# → technical-spec.md
# → architecture.md

# Check phase structure
ls .project-management/output/phases/
# → phase-1.md (Foundation & Setup)
# → phase-2.md (Core Features)
# → phase-3.md (Advanced Features)
# → phase-4.md (Polish & Launch)

# Check progress tracking
ls .project-management/output/progress/
# → current-status.md
# → completed.md
# → blockers.md
```

**Review checklist:**
- [ ] PRD matches your project vision
- [ ] Technical spec includes all requirements
- [ ] Phase breakdown makes sense
- [ ] Technologies align with your needs

---

### Step 3: Start Development

```bash
# Option 1: Execute entire phase (recommended)
/execute-work phase 1

# Option 2: Execute single epic
/execute-work epic Epic-1

# Option 3: Execute single story
/execute-work story US-001
```

---

## Your First Phase

### Understanding Phase Structure

```
Project
  └── Phase (1-4 months, major milestone)
      └── Epic (group of related features)
          └── User Story (specific functionality)
              └── Task (implementation steps)
```

**4 Standard Phases:**

1. **Phase 1: Foundation & Setup** (1-2 months)
   - Project setup, infrastructure, authentication
   - Database setup, API foundation
   - CI/CD pipeline

2. **Phase 2: Core Features** (2-3 months)
   - Main product features (P0 priority)
   - User-facing functionality
   - Core business logic

3. **Phase 3: Advanced Features** (2 months)
   - Secondary features (P1 priority)
   - Integrations
   - Admin features

4. **Phase 4: Polish & Launch** (1 month)
   - Optimization, testing, deployment
   - Bug fixes, performance
   - Launch preparation

---

### Execution Modes

When you run `/execute-work`, Claude asks for execution mode:

**[1] Continuous Mode**
```
✅ Best for: Automation, well-defined stories
✅ Behavior: Implements all stories without pausing
✅ When to use: Trust the plan, want speed
```

**[2] Paused Mode**
```
✅ Best for: Learning, complex features, review
✅ Behavior: Waits for approval after each story
✅ When to use: Want control, review each story
```

---

### Plan Mode (Mandatory)

**What is Plan Mode:**
A mandatory pre-implementation phase where Claude analyzes requirements, creates a detailed plan, and waits for user approval before writing code.

**Plan Mode Workflow:**
```
1. READ ALL CONTEXT
   ✅ Technical spec
   ✅ Backlog
   ✅ Core standards
   ✅ All rules files

2. ANALYZE SCOPE
   → Stories, dependencies, risks

3. CREATE DETAILED PLAN
   → Tasks, tests, estimates

4. WAIT FOR APPROVAL
   → [Yes/No/Revise]

5. START IMPLEMENTATION
   → Only after approval
```

---

### Quality Gates

**Story is NOT complete until:**
- ✅ All tests passing (unit, integration, E2E)
- ✅ Coverage > 80%
- ✅ All API status codes tested (200/400/401/403/404/500)
- ✅ i18n translations added (if enabled)
- ✅ SOLID & DRY principles followed
- ✅ Git commit created (NO AI credits)
- ✅ Progress tracking updated

---

## Quick Reference

### Essential Commands
```bash
/init-project              # Initialize project (one-time)
/execute-work phase 1      # Automated phase execution
/execute-work story US-001 # Single story execution
/run-tests all             # Manual testing
/project-status            # Comprehensive status report
# Open output/progress/DASHBOARD.md for live progress (auto-updated)
```

---

### File Locations
```
Input (you edit):
  .project-management/input/
    ├── scope.md            (project scope)
    ├── backlog.md          (user stories)
    ├── technologies.md     (tech stack)
    └── constraints.md      (limitations)

Output (generated):
  .project-management/output/
    ├── docs/               (PRD, tech spec, architecture)
    ├── phases/             (phase-1.md to phase-4.md)
    └── progress/           (current status, completed, blockers)

Rules (standards):
  .CLAUDE.MD                (core coding standards)
  .claude/rules/*.md        (specialized rules)
  .project-management/rules/*.md (project rules)
```

---

### Quality Gates Checklist
```
Before marking story complete:
✅ All tests passing
✅ Coverage > 80%
✅ API codes tested (200/400/401/403/404/500)
✅ i18n translations (if enabled)
✅ SOLID & DRY followed
✅ Git commit created
✅ Progress updated
```

---

### Next Steps

**Now that you're set up:**

1. **Learn the commands** → See `COMMANDS-REFERENCE.md`
2. **Understand workflows** → See `WORKFLOWS-BEST-PRACTICES.md`
3. **Troubleshooting** → See `TROUBLESHOOTING.md` (or `FAQ.md`)

---

**🎉 Ready to start!** Initialize your project with `/init-project` and begin automated development with `/execute-work`. 🚀

---

**Version:** 3.2.0
**Last Updated:** 2026-04-21
**Part of:** Claude Project Management System v3.2
