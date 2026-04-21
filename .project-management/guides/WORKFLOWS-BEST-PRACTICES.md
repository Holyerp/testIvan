# Workflows & Best Practices

**Quick reference for workflows and best practices. For detailed information, see linked documentation.**

**Version:** 3.0.0
**Last Updated:** 2026-03-27

---

## Quick Links

- **Detailed workflows:** [../docs/WORKFLOWS.md](../docs/WORKFLOWS.md)
- **Detailed best practices:** [../docs/BEST-PRACTICES.md](../docs/BEST-PRACTICES.md)
- **Getting started:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Commands reference:** [COMMANDS-REFERENCE.md](COMMANDS-REFERENCE.md)

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

1. **Phase 1: Foundation** (1-2 months) - Setup, infrastructure, auth, database, CI/CD
2. **Phase 2: Core Features** (2-3 months) - Main features, MVP functionality
3. **Phase 3: Advanced Features** (2 months) - Secondary features, integrations, admin
4. **Phase 4: Polish & Launch** (1 month) - Optimization, testing, deployment

**[Detailed phase guide →](../docs/WORKFLOWS.md)**

---

### Plan Mode

**What:** Mandatory pre-implementation phase for analysis and planning
**When:** Automatically activated by `/execute-work` and `/init-project`

**Quick workflow:**
```
1. READ context (tech spec, backlog, standards, rules)
2. ANALYZE scope (stories, dependencies, risks)
3. CREATE plan (implementation steps)
4. PRESENT to user for approval
5. WAIT for confirmation
6. IMPLEMENT only after approval
```

**Why mandatory:** Prevents mistakes, ensures quality, allows review before coding

---

### Quality Gates

**Automatic checks during `/execute-work`:**
- ✅ All tests passing
- ✅ Coverage > 80%
- ✅ All API status codes tested (200/400/401/403/404/500)
- ✅ i18n translations (if enabled)
- ✅ SOLID & DRY principles
- ✅ Git commit created (NO AI credits)
- ✅ Progress updated

**Cannot be skipped in automated workflow.**

---

## Common Workflows

### Workflow 1: Automated Phase Execution (Recommended)

```bash
# Start phase with automation
/execute-work phase 1

# Choose mode
[1] Continuous - Auto-execute all stories
[2] Paused - Prompt after each story

# Claude handles:
✅ Plan mode for each story
✅ Implementation
✅ Testing (unit + integration + e2e)
✅ Git commit (conventional format, no AI credits)
✅ Progress tracking
```

**Best for:** Most development work, production code

**[Detailed workflow →](../docs/WORKFLOWS.md#example-2-executing-next-phase)**

---

### Workflow 2: Story-by-Story Execution

```bash
# Execute single stories with control
/execute-work story US-001
/execute-work story US-002
/execute-work story US-003

# Each story:
→ Plan mode (review before implementation)
→ Implementation with quality gates
→ Auto-testing
→ Auto-commit
→ Progress update
```

**Best for:** When you want granular control over each story

---

### Workflow 3: Manual Implementation

```bash
# Implement code manually (your choice of tools)

# Test when ready
/run-tests all
/run-tests coverage

# Fix issues if needed

# Commit manually
git add .
git commit -m "feat: implement US-015"

# Progress: open output/progress/DASHBOARD.md (auto-updated when you use /execute-work).
# Manual edits: open the progress file directly.
```

**Best for:** Exploratory work, learning new tech, prototyping

**[Detailed workflow →](../docs/WORKFLOWS.md#example-1-starting-a-new-project)**

---

### Workflow 4: Hybrid Approach (Flexible)

```bash
# Mix automated and manual as needed
/execute-work story US-001    # Simple story → automated
# Manual implementation US-002 # Complex story → manual
/execute-work epic Epic-3     # Entire epic → automated
/run-tests all                # Manual test run
# Progress auto-updates via /execute-work → open DASHBOARD.md
```

**Best for:** Most projects - use right tool for each task

---

## Quick Best Practices

### 1. Always Use Plan Mode
- ✅ Mandatory in `/execute-work` (automatic)
- ✅ Review plans before approval
- ✅ Catch issues early

### 2. Enable i18n Early
- ✅ Configure during `/init-project` if needed
- ✅ Harder to add later
- ✅ Quality gate enforces translations

**[Detailed i18n guide →](../rules/I18N-SETUP.md)**

### 3. Run Tests Frequently
- ✅ Automatic in `/execute-work` (second-to-last step)
- ✅ Manual: `/run-tests all`
- ✅ Coverage: `/run-tests coverage`
- ✅ Target: > 80% coverage

**[Detailed testing guide →](../rules/TESTING-RULES.md)**

### 4. Use Default Stack (If Applicable)
- ✅ Production-tested (848 tests passing)
- ✅ React 19 + PostgreSQL 16 + Vitest + Playwright
- ✅ Or choose AI recommendation
- ✅ Or fully custom

### 5. Commit Message Standards
- ✅ Conventional commits (feat:, fix:, refactor:, etc.)
- ✅ **NO AI credits** (per git.md rules)
- ✅ Automatic in `/execute-work` (last step)
- ✅ Include test results and coverage

**[Detailed git conventions →](../.claude/rules/git.md)**

### 6. Monitor Quality Metrics
```bash
# Check project health
/project-status

# Shows:
- Progress (phases, stories, bugs)
- Quality metrics (tests, coverage)
- Blockers and risks
- Recommendations
```

### 7. Progress Tracking
- ✅ Automatic in `/execute-work` (DASHBOARD.md, daily-summary.md, etc. refresh on story completion)
- ✅ Live view: open `output/progress/DASHBOARD.md` — always current
- ✅ Manual adjustments: edit progress files directly (the `/update-progress` command was removed in v3.2.0)

### 8. Use Status Reports
```bash
# Weekly status check
/project-status

# Before meetings
/project-status

# Share with stakeholders
```

### 9. Leverage Execution Modes
```bash
# Continuous mode - speed (auto-execute all)
/execute-work phase 1 → [1] Continuous

# Paused mode - control (prompt after each)
/execute-work phase 1 → [2] Paused
```

### 10. Handle Blockers Promptly
```bash
# When blocked
Edit output/progress/blockers.md → Document blocker

# Check impact
/project-status → See blocker impact

# When resolved
Edit blockers.md → Mark as resolved
```

**[Detailed best practices →](../docs/BEST-PRACTICES.md)**

---

## Common Patterns

### Starting New Project
```bash
# With client docs
/process-client-docs → /init-project → /execute-work phase 1

# Manual entry
Fill input/*.md → /init-project → /execute-work phase 1
```

### Daily Development
```bash
# Automated
/execute-work phase N → Continue work → /project-status

# Manual
Implement → /run-tests → Commit → Open DASHBOARD.md (auto-updated) to verify
```

### Debugging Issues
```bash
/run-tests coverage → Fix issues → /run-tests all → Resume /execute-work
```

### Scope Changes
```bash
/add-scope add story → /generate-docs → Continue /execute-work
```

### Bug Fixes
```bash
/add-bug → /execute-work bug BUG-XXX → Verify fix
```

---

## Quick Decision Tree

```
What are you doing?

Starting project?
  → /init-project → Choose automation level

Daily development?
  → Simple stories? → /execute-work
  → Complex work? → Manual + /run-tests

Scope changed?
  → /add-scope → /generate-docs

Found bug?
  → /add-bug → /execute-work bug BUG-XXX

Need status?
  → /project-status

Tests failing?
  → /run-tests coverage → Fix → /run-tests all
```

---

## Additional Resources

### Detailed Documentation
- **Complete workflows:** [../docs/WORKFLOWS.md](../docs/WORKFLOWS.md) - Step-by-step examples
- **Complete best practices:** [../docs/BEST-PRACTICES.md](../docs/BEST-PRACTICES.md) - Guidelines and metrics
- **Architecture:** [../docs/ARCHITECTURE.md](../docs/ARCHITECTURE.md) - System structure
- **Input files:** [../docs/INPUT-FILES-GUIDE.md](../docs/INPUT-FILES-GUIDE.md) - Filling project data

### Quick Guides
- **Getting started:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Commands:** [COMMANDS-REFERENCE.md](COMMANDS-REFERENCE.md)
- **FAQ:** [FAQ.md](FAQ.md)
- **Troubleshooting:** [TROUBLESHOOTING.md](TROUBLESHOOTING.md)

### Rules & Standards
- **Code quality:** [../.claude/rules/code-quality.md](../.claude/rules/code-quality.md)
- **Testing:** [../.claude/rules/testing.md](../.claude/rules/testing.md)
- **Git conventions:** [../.claude/rules/git.md](../.claude/rules/git.md)
- **Project rules:** [../rules/project-rules.md](../rules/project-rules.md)

---

**Version:** 3.0.0
**Last Updated:** 2026-03-27
**Part of:** Claude Project Management System v3.0

**Note:** This is a quick reference. For comprehensive workflows and best practices, see the detailed documentation linked above.
