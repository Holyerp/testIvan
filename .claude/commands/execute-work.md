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

**Optional:** pass mode preferences inline to skip the prompt (positional digits OR named flags; see STEP 0 for the full mapping):

```bash
/execute-work story US-MOB-023 1 2                              # Continuous + Complete
/execute-work phase 1 --mode=continuous --tracking=phase-only   # named flags
/execute-work epic EPIC-3 2                                     # Paused; will ask for tracking
```

---

## 📋 YOUR TASK — MANDATORY WORKFLOW

**🔧 CRITICAL RULES** — read before any implementation:
- `.claude/rules/code-quality.md` — SOLID & DRY (mandatory)
- `.claude/rules/testing.md` — Testing requirements, API status matrix, coverage targets
- `.claude/rules/api-documentation.md` — API schema validation + matching docs (STRICT for public endpoints, SOFT for `@internal`)
- `.claude/rules/api-first.md` — Frontend (web/mobile) cannot start until API contract is verified; gaps block & file backend work
- `.claude/rules/screen-driven-backlog.md` — Web/mobile stories = 1 screen + listed API endpoints (wizard exception)
- `.claude/rules/git.md` — Commit format (NO AI credits), conventional commits
- `.CLAUDE.MD` — Core standards and workflow

---

### STEP 0 — Parse arguments & mode selection

**Parse the scope argument:**
- `phase N` → all epics + stories in Phase N
- `epic EPIC-X` → all stories in Epic X
- `story US-XXX` → single story
- `bug BUG-XXX` → bug fix flow

**Parse optional inline mode arguments** — both formats are supported (and may be mixed):

**Positional digits** (after the scope): `<execMode> <trackingMode>` where each digit is `1` or `2`.

```
/execute-work story US-MOB-023 1 2     # Continuous + Complete
/execute-work phase 1 1                # Continuous; tracking missing → ask
/execute-work epic EPIC-3 2 1          # Paused + Phase Only
```

**Named flags** (any position after the scope):

```
/execute-work story US-MOB-023 --mode=continuous --tracking=complete
/execute-work phase 1 --mode=c                          # alias: c|continuous, p|paused
/execute-work phase 1 --tracking=p                      # alias: p|phase-only, c|complete
/execute-work story US-XXX --mode=paused --tracking=p   # mixed forms allowed
```

**Mapping table:**

| Input | Execution Mode | Tracking Mode |
|-------|----------------|---------------|
| `1` / `--mode=1` / `--mode=c` / `--mode=continuous` | Continuous | — |
| `2` / `--mode=2` / `--mode=p` / `--mode=paused` | Paused | — |
| `1` / `--tracking=1` / `--tracking=p` / `--tracking=phase` / `--tracking=phase-only` | — | Phase Only |
| `2` / `--tracking=2` / `--tracking=c` / `--tracking=complete` | — | Complete |

> **Positional disambiguation:** if exactly two trailing digits are present, the first is Execution Mode and the second is Tracking Mode. If only one trailing digit is present, treat it as Execution Mode (the more user-facing choice). Named flags always take precedence over positional values when both are supplied.

**Prompt only for what is missing.** If both modes were supplied (positional or named), proceed silently — do **not** show the menu. If one was supplied, ask only for the other. If none were supplied, show the full menu below.

```
Execution Mode:
[1] Continuous (fresh sub-agent per story — clean context, auto-reset between stories)
[2] Paused (in-line execution — wait for approval after each story; user controls context manually)

Progress Tracking Mode:
[1] Phase Only (faster — updates only phase file)
[2] Complete (slower — updates all progress files)
```

After resolving both modes, **echo the resolved choices** so the user can spot a misparse:

```
Resolved modes:
  Execution:  Continuous (sub-agent dispatch)
  Tracking:   Complete
```

Store both choices. For trade-offs, see `execute-work-reference.md` → Execution Modes.

**Why this matters:** Continuous mode dispatches each story into a fresh sub-agent so the orchestrator never accumulates story-by-story context. The orchestrator keeps only structured summaries. This is the recommended mode for any phase/epic with 3+ stories.

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

**1D. API contract verification (frontend stories only)** — if the story is web or mobile per `.claude/rules/screen-driven-backlog.md`, run the Phase A checklist from `.claude/rules/api-first.md` §3 *before* exiting plan mode:
- List every screen the story touches (wizard = enumerate steps)
- For each screen, list every API endpoint it calls (method + path)
- For each endpoint, confirm doc exists, request schema covers UI inputs, response shape covers UI outputs, error states are distinguishable, auth matches
- **Result:** ✅ contract complete (proceed to STEP 2) **OR** ⚠️ gaps documented → mark story `Blocked by: <backend story/bug>`, file the backend work via `/add-scope` or bug roadmap, and **do not exit plan mode for this story**. Move on to other stories or return to user.

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

**Branch by execution mode** — the per-story workflow is the same; only the dispatch wrapper differs.

#### STEP 3-A — Continuous mode (sub-agent dispatch, RECOMMENDED)

For each story/bug in scope:

1. **Display:** `🚀 Dispatching US-XXX in fresh sub-agent (clean context)...`
2. **Auto-update `DASHBOARD.md`** → "Currently Working On" *(modular only — orchestrator does this BEFORE dispatch so DASHBOARD reflects current work even while sub-agent runs)*.
3. **Dispatch via Agent tool** with `subagent_type="general-purpose"` and the per-story prompt from `modules/execute-work-implementation.md` § A (Sub-agent Prompt Template). Pass:
   - Story ID, title, phase/epic
   - Absolute path to story file (e.g., `.project-management/output/phases/phase-N.md`)
   - Tracking mode (Phase Only / Complete)
   - Execution context (modular vs monolithic backlog)
4. **Sub-agent executes the full per-story workflow in its own clean context** — reads rules, implements, writes tests, runs tests (≥80% coverage, all API codes), verifies i18n + API docs, updates progress files, creates git commit. Returns a structured JSON summary.
5. **Orchestrator parses summary:**
   - `status: "completed"` → display story summary block, log, proceed to next story
   - `status: "blocked"` → display blocker reason, ask user `[Continue with next story / Skip / Abort]`
6. **Orchestrator keeps ONLY the summary** (~1KB) — no story-level work bleeds into the next dispatch. This is the auto-reset.

When all stories in scope are dispatched and summaries collected → STEP 4.

#### STEP 3-B — Paused mode (in-line execution, manual control)

For each story/bug in scope, the orchestrator itself executes the per-story workflow (no sub-agent). This is the legacy behavior — the orchestrator's context accumulates, and the user can hit `/clear` manually between stories if needed.

Workflow per story (detailed in `modules/execute-work-implementation.md` § B):

1. Break down with TodoWrite.
2. Auto-update `DASHBOARD.md` → "Currently Working On" *(modular only)*.
3. Read context (story from phase backlog / bug from bug-roadmap).
4. **For frontend (web/mobile) stories:** before implementation, re-confirm Phase A from `.claude/rules/api-first.md` is still ✅ — endpoints exist, docs match, schema covers UI inputs/outputs, error states distinguishable. If any contract gap is detected now (backend changed, doc drifted), STOP, file backend gap, mark story Blocked. Do not stub the frontend.
5. Implement following `.claude/rules/code-quality.md` (SOLID & DRY).
6. Write tests following `.claude/rules/testing.md` (unit + integration + E2E + all API status codes 200/400/401/403/404/500).
7. Verify i18n (if `.project-management/rules/I18N-RULES.md` exists).
8. **If the story added/changed any HTTP endpoint:** verify `.claude/rules/api-documentation.md` — schema validation in code, typed response, doc block per `documentation.md` §6.1, drift check against tests. STRICT for public endpoints; `@internal`-tagged endpoints follow SOFT tier.
9. **Second-to-last step:** run tests (see `modules/execute-work-quality-gates.md`); auto-update DASHBOARD "Quality Metrics".
10. **Final step:** git commit per `.claude/rules/git.md` (NO AI credits). Bug commits reference `BUG-XXX`.
11. Update progress tracking (phase file + DASHBOARD auto-update + completed.md / daily-summary.md per Complete mode).
12. Pause and ask user `[Yes / No / Skip to Epic X]`.

#### Common quality gate (both modes)

Tests pass, coverage ≥ 80%, all API codes tested, i18n complete, API docs match implementation (when endpoints touched). The same gate applies whether the work is done by orchestrator (Paused) or sub-agent (Continuous).

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
| `modules/execute-work-implementation.md` | § A sub-agent prompt template (Continuous), § B in-line workflow (Paused) |
| `modules/execute-work-quality-gates.md` | Test + coverage validation (same gates for both modes) |
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

**Version:** 3.3.0
**Created:** 2026-03-27
**Updated:** 2026-04-29 (Continuous mode dispatches each story to a fresh sub-agent for auto-context-reset; Paused mode unchanged)
**Command Type:** Implementation Automation
