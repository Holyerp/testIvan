---
name: execute-work
description: Execute a phase, epic, or story with automatic planning, implementation, testing, and progress tracking
---

# Execute Work Command

**📖 Quick Start:** See [how-to-use/execute-work.md](./how-to-use/execute-work.md) for quick guide (~150 lines)

Execute implementation of a phase, epic, or individual story with full automation.

---

## Usage

```bash
/execute-work phase N          # Execute entire Phase N
/execute-work epic EPIC-X      # Execute Epic X
/execute-work story US-XXX     # Execute single story US-XXX
/execute-work bug BUG-XXX      # Execute bug fix for BUG-XXX
```

---

## 📋 YOUR TASK — MANDATORY WORKFLOW

**🔧 CRITICAL RULES** — read before any implementation:
- `.claude/rules/code-quality.md` — SOLID & DRY (mandatory)
- `.claude/rules/testing.md` — Testing requirements, API status matrix, coverage targets
- `.claude/rules/git.md` — Commit format (NO AI credits), conventional commits
- `.CLAUDE.MD` — Core standards and workflow

---

### STEP 0 — Parse arguments & mode selection

**Parse the command argument:**
- `phase N` → all epics + stories in Phase N
- `epic EPIC-X` → all stories in Epic X
- `story US-XXX` → single story
- `bug BUG-XXX` → bug fix flow

**Ask the user for two modes:**

```
Execution Mode:
[1] Continuous (no pauses between stories)
[2] Paused (wait for approval after each story)

Progress Tracking Mode:
[1] Phase Only (faster — updates only phase file)
[2] Complete (slower — updates all progress files)
```

Store both choices. For trade-offs, see `execute-work-reference.md` → Execution Modes.

---

### STEP 1 — Detect structure & enter plan mode (MANDATORY)

**1A. Detect backlog structure:**

```
if exists(".project-management/input/backlog/README.md"):
    structure_type = "modular"      # input/backlog/phase-*.md
else if exists(".project-management/input/backlog.md"):
    structure_type = "monolithic"   # input/backlog.md
```

**1B. Read context files** — see `modules/execute-work-plan-mode.md` for the full read list per structure type.

**1C. Analyze & plan** — create a detailed plan with estimates, risks, success criteria. Wait for user approval (`Yes / No / Revise`).

**For bugs:** read `output/bugs/bug-roadmap.md`, analyze affected component, plan fix with root-cause analysis + regression-test requirements.

**Output:** approved plan + detected `structure_type`.

---

### STEP 2 — Exit plan mode → implementation

```
🚀 [EXITING PLAN MODE — ENTERING IMPLEMENTATION MODE]
Execution Mode: [Continuous / Paused]
```

---

### STEP 3 — Implementation loop

**For each story/bug** (detailed workflow in `modules/execute-work-implementation.md`):

1. Break down with TodoWrite.
2. Auto-update `DASHBOARD.md` → "Currently Working On" *(modular only)*.
3. Read context (story from phase backlog / bug from bug-roadmap).
4. Implement following `.claude/rules/code-quality.md` (SOLID & DRY).
5. Write tests following `.claude/rules/testing.md` (unit + integration + E2E + all API status codes 200/400/401/403/404/500).
6. Verify i18n (if `.project-management/rules/I18N-RULES.md` exists).
7. **Second-to-last step:** run tests (see `modules/execute-work-quality-gates.md`); auto-update DASHBOARD "Quality Metrics".
8. **Final step:** git commit per `.claude/rules/git.md` (NO AI credits). Bug commits reference `BUG-XXX`.
9. Update progress tracking (phase file + DASHBOARD auto-update + completed.md / daily-summary.md per Complete mode).
10. Check execution mode; continue or pause.

**Quality gate:** tests pass, coverage ≥ 80%, all API codes tested, i18n complete.

**Auto-update triggers (modular only):** story started → Currently Working On; tests run → Quality Metrics; story completed → Today's Progress + Recently Completed + progress %; phase completed → Phase Breakdown.

Full event mapping: `modules/execute-work-dashboard-events.md` + `modules/execute-work-dashboard-mechanics.md`.

---

### STEP 4 — Completion report

When all stories in scope are done, emit the standard completion block with:

- stories completed / total, points done / total, velocity
- tests written / passing, coverage %
- SOLID & DRY compliance, lint, git conventions (all ✅)
- next steps (advance phase / continue epic / next story)

Template and full field list: `execute-work-reference.md` → Completion Report Template.

---

## 📚 Module References

| Module | Covers |
|--------|--------|
| `modules/execute-work-plan-mode.md` | STEP 1 plan mode workflow |
| `modules/execute-work-implementation.md` | STEP 3 implementation loop |
| `modules/execute-work-quality-gates.md` | Test + coverage validation |
| `modules/execute-work-dashboard-events.md` | DASHBOARD auto-update triggers |
| `modules/execute-work-dashboard-mechanics.md` | DASHBOARD update internals |
| `execute-work-reference.md` | Modes, quality gates, error handling, examples |

---

## Mandatory Requirements (summary)

1. **Plan mode is mandatory** — no implementation without approval.
2. **Tests are mandatory** — story is not done until tests pass.
3. **Coverage ≥ 80%** — enforced before completion.
4. **All API status codes tested** — 200/400/401/403/404/500.
5. **i18n compliance** — if `I18N-RULES.md` exists, translations are mandatory.
6. **Git conventions** — NO AI credits; conventional commits.
7. **SOLID & DRY** — per `.claude/rules/code-quality.md`.
8. **TodoWrite** — used for task breakdown and tracking.

Full quality-gate checklist + error handling: `execute-work-reference.md`.

---

## Backward Compatibility

Auto-detects modular vs monolithic backlog — no user action needed. See `execute-work-reference.md` → Backward Compatibility for the trade-offs (token usage, DASHBOARD availability, auto-update scope).

---

**Version:** 3.2.0
**Created:** 2026-03-27
**Updated:** 2026-04-21 (split: reference moved to execute-work-reference.md)
**Command Type:** Implementation Automation
