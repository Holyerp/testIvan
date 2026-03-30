# Claude Project Management System - User Guide

**Complete guide for phase-based project management with automated execution.**

**Version:** 3.0 (Phase-Based with Automation)
**Last Updated:** 2026-03-27

---

## Table of Contents

1. [Quick Start (5 Minutes)](#quick-start-5-minutes)
2. [What's New in v3.0](#whats-new-in-v30)
3. [Core Concepts](#core-concepts)
4. [Getting Started](#getting-started)
5. [Commands Reference](#commands-reference)
6. [Workflows](#workflows)
7. [Best Practices](#best-practices)
8. [FAQ & Troubleshooting](#faq--troubleshooting)

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

## What's New in v3.0

### Major Changes from v2.0

| Feature | v2.0 (Old) | v3.0 (New) |
|---------|------------|------------|
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

### New Commands

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

## Core Concepts

### Phase-Based Structure

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

2. **Phase 2: Core Features** (2-3 months)
   - Main product features (P0 priority)

3. **Phase 3: Advanced Features** (2 months)
   - Secondary features (P1 priority)

4. **Phase 4: Polish & Launch** (1 month)
   - Optimization, testing, deployment

---

### Plan Mode

**What is Plan Mode:**
A mandatory pre-implementation phase where Claude analyzes requirements, creates a detailed plan, and waits for user approval before writing code.

**When It Activates:**
- Automatically when running `/execute-work`
- Automatically when running `/init-project`
- For any command with "plan" in the name

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

## Getting Started

### Prerequisites

- ✅ Git repository
- ✅ Claude Code CLI installed
- ✅ Basic understanding of Agile concepts

---

### Installation

```bash
# 1. Navigate to your project
cd /path/to/your/project

# 2. Copy the system
cp -r /path/to/claude_repo/.project-management .
cp -r /path/to/claude_repo/.claude .
cp /path/to/claude_repo/.CLAUDE.MD .

# 3. Verify structure
ls -la .project-management/
```

---

### First-Time Setup

#### Step 1: Initialize Project

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

[2] AI Recommendation
    - Claude analyzes your project
    - Suggests optimal stack
    - Explains technology choices

[3] Custom Setup
    - Answer 8 questions
    - Full control over choices
```

**i18n Configuration:**
```
[1] Yes - Configure i18n
    → Select languages (en, de, sr, fr, etc.)
    → Creates I18N-RULES.md
    → Sets up translation file structure

[2] No - Skip i18n
    → No translation requirements
```

**Claude generates:**
- ✅ PRD (Product Requirements Document)
- ✅ Technical Specification
- ✅ Architecture Documentation
- ✅ Phase structure (4 phases)
- ✅ Initial progress tracking

---

#### Step 2: Review Generated Documentation

```bash
# Check generated docs
ls .project-management/output/docs/
# prd.md
# technical-spec.md
# architecture.md

# Check phase structure
ls .project-management/output/phases/
# phase-1.md
# phase-2.md
# phase-3.md
# phase-4.md
```

---

#### Step 3: Start Development

```bash
# Execute Phase 1
/execute-work phase 1

# OR execute single epic
/execute-work epic Epic-1

# OR execute single story
/execute-work story US-001
```

---

## Commands Reference

### `/init-project`

**Purpose:** Initialize project with tech stack selection, i18n configuration, and phase structure

**Usage:**
```bash
/init-project
```

**What it does:**
1. Tech stack selection (Default/AI/Custom)
2. i18n configuration (Yes/No + languages)
3. Reads input files (scope, backlog, technologies, constraints)
4. Generates documentation (PRD, tech spec, architecture)
5. Creates phase structure (4 phases)
6. Creates progress tracking

**Output:**
- `.project-management/output/docs/*.md`
- `.project-management/output/phases/*.md`
- `.project-management/output/progress/*.md`
- `.project-management/rules/I18N-RULES.md` (if i18n enabled)

---

### `/execute-work phase/epic/story`

**Purpose:** Automated execution with plan mode, testing, commits, and progress tracking

**Usage:**
```bash
/execute-work phase 1        # Execute entire Phase 1
/execute-work epic Epic-2    # Execute Epic 2
/execute-work story US-015   # Execute single story
```

**Workflow:**
```
1. MODE SELECTION
   [1] Continuous
   [2] Paused (wait after each story)

2. PLAN MODE (automatic)
   → Reads all context
   → Creates detailed plan
   → Waits for approval

3. IMPLEMENTATION (for each story)
   → Implements following standards
   → Runs tests (Vitest + Playwright)
   → Validates quality gates
   → Creates git commit (NO AI credits)
   → Updates progress tracking

4. COMPLETION REPORT
   → Statistics
   → Quality metrics
   → Next steps
```

**Quality Gates (automatic validation):**
- All tests must pass
- Coverage > 80%
- All API status codes (200/400/401/403/404/500)
- i18n translations (if enabled)
- SOLID & DRY compliance

---

### `/run-tests [scope]`

**Purpose:** Manually run tests with detailed reporting

**Usage:**
```bash
/run-tests all                # All tests
/run-tests unit               # Unit tests only
/run-tests integration        # Integration tests only
/run-tests e2e                # E2E tests only
/run-tests coverage           # With coverage report
/run-tests story US-015       # Tests for specific story
/run-tests file src/api.ts    # Tests for specific file
```

**Output:**
- Test results (passed/failed)
- Coverage metrics
- API status code coverage
- Suggestions for fixes (if failures)

---

### `/update-progress`

**Purpose:** Manually update progress tracking

**Usage:**
```bash
/update-progress
```

**Claude asks:**
- What did you complete?
- Any blockers?
- Tests passing?
- Notes?

**Updates:**
- Phase progress file
- Story statuses
- Test metrics
- Blockers log

---

### `/project-status`

**Purpose:** Generate comprehensive status report

**Usage:**
```bash
/project-status
```

**Report includes:**
- Overall completion %
- Phase breakdown
- Velocity trends
- Test metrics
- Blockers
- Recommendations

---

## Workflows

### Workflow 1: Automated Phase Execution

**Best for:** Continuous development, automation lovers

```bash
# 1. Initialize (one-time)
/init-project

# 2. Execute entire phase
/execute-work phase 1

# 3. Choose mode
[1] Continuous (recommended)

# 4. Approve plan
[Yes]

# Claude now:
# ✅ Implements all stories in Phase 1
# ✅ Runs tests after each story
# ✅ Creates git commits
# ✅ Updates progress
# ✅ Continues automatically

# 5. When Phase 1 complete
/execute-work phase 2
```

**Time saved:** ~90% compared to manual workflow

---

### Workflow 2: Controlled Story-by-Story

**Best for:** Learning, reviewing each story, complex features

```bash
# 1. Initialize
/init-project

# 2. Execute one story at a time
/execute-work story US-001

# 3. Choose paused mode
[2] Paused

# 4. Approve plan
[Yes]

# Claude implements US-001:
# ✅ Implements
# ✅ Tests
# ✅ Commits
# ✅ Updates progress

# 5. Ask permission for next story
Continue with US-002? [Yes/No]

# 6. You control pace
[Yes] - Continue
[No] - Stop (resume later)
```

---

### Workflow 3: Manual Testing

**Best for:** Test-driven development, debugging tests

```bash
# 1. Implement manually (without /execute-work)
# Use TodoWrite for task breakdown

# 2. Run tests manually
/run-tests all

# 3. If failures, fix and rerun
/run-tests coverage

# 4. When all pass, update progress
/update-progress

# 5. Create git commit manually
git commit -m "feat: implement US-001"
```

---

### Workflow 4: Hybrid Approach

**Best for:** Most projects, balanced control

```bash
# 1. Use /execute-work for simple stories
/execute-work story US-001
→ Fully automated

# 2. Manual for complex stories
→ Implement with TodoWrite
→ Test with /run-tests
→ Commit manually

# 3. Automated for entire epics
/execute-work epic Epic-3
→ Multiple stories automated
```

---

## Best Practices

### 1. Always Use Plan Mode

**Why:** Prevents wrong implementations, ensures standards

```bash
# Automatically activated:
/execute-work story US-015

# Plan mode shows:
→ What will be implemented
→ Which tests will be written
→ Which files will be modified
→ Estimated time

# Review and approve before implementation
```

---

### 2. Enable i18n Early (if needed)

**If your project needs multi-language:**
```bash
# During /init-project
[1] Yes - Configure i18n

# Select languages early
→ English (required)
→ German, Spanish, French (optional)

# Why: Easier to add translations as you go
```

---

### 3. Run Tests Frequently

```bash
# After implementing story
/run-tests coverage

# Before marking complete
/run-tests all

# When debugging
/run-tests story US-015
```

---

### 4. Use Default Stack (if applicable)

**If building modern web app:**
```bash
# During /init-project
[1] Default HolyEstate Stack

# Benefits:
✅ Production-tested
✅ 848 tests passing
✅ Best practices baked in
✅ Quick start
```

---

### 5. Update Progress Daily

```bash
# End of each day
/update-progress

# Report:
→ Stories completed
→ Tests passing
→ Blockers (if any)

# Why: Real-time visibility
```

---

## FAQ & Troubleshooting

### Q: When should I use `/execute-work` vs manual implementation?

**Use `/execute-work` when:**
- ✅ Stories are well-defined
- ✅ You want automation
- ✅ Quality gates enforced automatically

**Use manual when:**
- ✅ Exploring new architecture
- ✅ Learning new technology
- ✅ Highly experimental work

---

### Q: Can I stop `/execute-work` mid-execution?

**Yes!**
```
Choose paused mode:
[2] Paused

After each story:
Continue? [No] - Stops execution

Resume later:
/execute-work phase 1 (continues from where it stopped)
```

---

### Q: What if tests fail during `/execute-work`?

**Claude automatically:**
1. Detects test failure
2. Keeps story as "in_progress"
3. Analyzes failure
4. Fixes issues
5. Re-runs tests
6. Repeats until all pass

**Story is NOT marked complete until tests pass!**

---

### Q: How do I disable i18n?

**Option 1: During init-project**
```bash
[2] No - Skip i18n
```

**Option 2: After init (if already enabled)**
```bash
rm .project-management/rules/I18N-RULES.md
```

---

### Q: Can I customize the default stack?

**Yes!**
```bash
# Option 1: Choose custom during init
/init-project
[3] Custom Setup
→ Answer questions

# Option 2: Edit technologies.md after init
vim .project-management/input/technologies.md
/generate-docs
```

---

### Q: What happens to git commits?

**Automatic commits (via `/execute-work`):**
- Created after each story
- Follow conventional commit format
- **NO AI credits** (per git.md rules)
- Include test results

**Example:**
```
feat: implement US-015 real-time notifications

- WebSocket server setup
- Notification component
- Database triggers
- Unit tests (8 passed)
- Integration tests (3 passed)
- E2E tests (1 passed)

Tests: 12/12 passed
Coverage: 89%
```

---

### Q: How do phases differ from sprints?

| Aspect | Sprints (v2.0) | Phases (v3.0) |
|--------|----------------|---------------|
| Duration | 2 weeks (fixed) | 1-4 months (flexible) |
| Scope | 10-20 stories | Multiple epics |
| Planning | Manual | Automated |
| Focus | Time-boxed iteration | Major milestone |
| Velocity | Points/sprint | Points/phase |

---

### Troubleshooting: Commands not working

**Issue:** `/execute-work` not found

**Solution:**
```bash
# 1. Check if command file exists
ls .claude/commands/execute-work.md

# 2. If missing, copy from template
cp /path/to/claude_repo/.claude/commands/execute-work.md .claude/commands/

# 3. Restart Claude Code session
```

---

### Troubleshooting: Tests failing repeatedly

**Issue:** `/execute-work` stuck fixing tests

**Solution:**
```bash
# 1. Cancel execution (Ctrl+C or [No] in paused mode)

# 2. Run tests manually to see details
/run-tests coverage

# 3. Fix manually if needed

# 4. Resume execution
/execute-work story US-015
```

---

### Troubleshooting: Plan mode taking too long

**Issue:** Plan mode analyzing for > 5 minutes

**Solution:**
```bash
# Possible causes:
→ Too many stories (break into smaller epics)
→ Complex dependencies (simplify)
→ Large technical spec (focus sections)

# Workaround: Execute smaller scope
/execute-work story US-015
# Instead of:
/execute-work phase 1
```

---

## Migration from v2.0

**See:** `.project-management/docs/MIGRATION-GUIDE.md`

**Quick summary:**
1. Backup existing project
2. Update system files
3. Convert sprints to phases
4. Test new `/execute-work` command
5. Use `/run-tests` for manual testing

---

## Additional Resources

- **`.CLAUDE.MD`** - Core coding standards
- **`INTEGRATION-GUIDE.md`** - How system works
- **`MIGRATION-GUIDE.md`** - v2.0 → v3.0 upgrade
- **`.claude/rules/*.md`** - Specialized rules
- **`.project-management/defaults/`** - Tech stack templates

---

## Quick Reference

### Essential Commands
```bash
/init-project              # Initialize project
/execute-work phase 1      # Automated execution
/run-tests all             # Manual testing
/update-progress           # Manual progress update
/project-status            # Status report
```

### File Locations
```
Input (you edit):
  .project-management/input/*.md

Output (generated):
  .project-management/output/docs/*.md
  .project-management/output/phases/*.md

Rules (standards):
  .CLAUDE.MD
  .claude/rules/*.md
  .project-management/rules/*.md
```

### Quality Gates Checklist
```
Before story complete:
✅ All tests passing
✅ Coverage > 80%
✅ API codes tested (200/400/401/403/404/500)
✅ i18n translations (if enabled)
✅ SOLID & DRY followed
✅ Git commit created
✅ Progress updated
```

---

**🎉 Ready to start!** Initialize your project with `/init-project` and begin automated development with `/execute-work`. 🚀

---

**Version:** 3.0
**Last Updated:** 2026-03-27
**System:** Phase-Based with Automation
