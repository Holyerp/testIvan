# Execute Work — Implementation Loop Module

**Referenced by:** `execute-work.md` STEP 3
**Companion:** `execute-work-progress-updates.md` (per-mode progress-file updates)

This module has **two sections**:

- **§ A — Sub-agent Prompt Template (Continuous mode)** — used by orchestrator in `execute-work.md` STEP 3-A to dispatch each story to a fresh sub-agent.
- **§ B — In-line Workflow (Paused mode)** — used by orchestrator in `execute-work.md` STEP 3-B to execute each story directly with user pauses between.

Both sections enforce the **same quality gates**. Only the dispatch wrapper differs: § A runs in a fresh sub-agent context; § B runs in the orchestrator's own context.

---

## § A — Sub-agent Prompt Template (Continuous mode)

The orchestrator dispatches each story by calling the `Agent` tool with `subagent_type="general-purpose"` and the prompt below. The sub-agent executes the entire per-story workflow in its own clean context, then returns a strict JSON summary. The orchestrator keeps only the summary — no per-story work bleeds into the next dispatch. **This is the auto-context-reset mechanism.**

### A.1 Prompt Template

Substitute the `{{...}}` placeholders before dispatching:

```
You are dispatched as a sub-agent to execute exactly ONE unit of work: {{STORY_ID}}.
You have NO context from previous stories — this is intentional. Treat this as a fresh session.

Unit:        {{STORY_ID}} — {{STORY_TITLE}}
Type:        {{story | bug}}
Phase:       {{N}}
Epic:        {{EPIC_ID}}
Story file:  {{ABSOLUTE_PATH_TO_STORY_FILE}}     # e.g., .project-management/output/phases/phase-2.md
Tracking:    {{Phase Only | Complete}}
Backlog:     {{modular | monolithic}}
Working dir: {{CWD}}

═══════════════════════════════════════════════════════════════
MANDATORY WORKFLOW — every step must complete before you return.
═══════════════════════════════════════════════════════════════

STEP 1 — Read context (do not skip any):
  - {{story file}} — only the section for {{STORY_ID}}
  - .claude/rules/code-quality.md
  - .claude/rules/testing.md
  - .claude/rules/git.md
  - .claude/rules/api-documentation.md          (if story touches an HTTP endpoint)
  - .claude/rules/api-first.md                  (if frontend story)
  - .claude/rules/screen-driven-backlog.md      (if frontend story)
  - .project-management/rules/I18N-RULES.md     (if file exists)
  - .claude/commands/modules/execute-work-implementation.md § B (workflow detail)
  - .claude/commands/modules/execute-work-quality-gates.md
  - .claude/commands/modules/execute-work-progress-updates.md

STEP 2 — Plan with TodoWrite (mirror the § B 3.1 list — implement, test, i18n, run tests, commit, progress).

STEP 3 — Implement all tasks of {{STORY_ID}} per .claude/rules/code-quality.md (SOLID & DRY).

STEP 4 — Write tests per .claude/rules/testing.md:
  - Unit + integration + E2E
  - All API status codes 200/400/401/403/404/500 (if endpoint touched)

STEP 5 — Verify i18n (only if I18N-RULES.md exists).

STEP 6 — If story touched any HTTP endpoint: run api-documentation.md gate
  (schema validation in code, typed response, doc block, no drift between code/docs/tests).

STEP 7 — Run tests:
  - npm test (unit + integration)
  - npm run test:e2e (if E2E suite exists)
  - npm run test:coverage — coverage MUST be ≥ 80%
  If anything fails: fix → re-run. Loop until all gates pass OR you hit a true blocker.

STEP 8 — Update progress files per execute-work-progress-updates.md and tracking mode "{{Tracking}}":
  - Phase Only: phase-N.md only
  - Complete:   phase-N.md + completed.md + current-status.md (+ velocity recalc)
  - Modular backlog: also auto-update DASHBOARD.md per execute-work-dashboard-events.md

STEP 9 — Git commit per .claude/rules/git.md:
  - Conventional commit (feat: / fix: / refactor: / test: / docs:)
  - NO AI credits, NO Co-Authored-By: Claude lines
  - Bug commits reference BUG-XXX

═══════════════════════════════════════════════════════════════
RETURN VALUE — strict JSON only, no surrounding prose.
═══════════════════════════════════════════════════════════════

On success:
{
  "story_id":          "{{STORY_ID}}",
  "status":            "completed",
  "title":             "{{STORY_TITLE}}",
  "points":            N,
  "tests":             { "passed": N, "total": N, "suites": ["unit","integration","e2e"] },
  "coverage":          "XX%",
  "api_codes_tested":  ["200","400","401","403","404","500"],
  "i18n":              "complete | not_required",
  "api_docs":          "clean | not_required",
  "commit_hash":       "abc1234",
  "files_touched":     ["path/to/file.ts", ...],
  "duration_estimate": "Xm",
  "blockers":          []
}

On blocker (cannot complete despite retries — e.g., missing backend endpoint per api-first.md, unfixable test, missing dependency):
{
  "story_id":  "{{STORY_ID}}",
  "status":    "blocked",
  "title":     "{{STORY_TITLE}}",
  "blockers":  ["one-line reason", "another reason"],
  "partial_state": "what was/wasn't done; whether anything was committed",
  "recommended_next": "file backend story BUG-YYY | skip and continue | abort run"
}

NEVER return status "completed" if any quality gate is unmet. NEVER commit work that fails tests.
NEVER add AI attribution to commits.
```

### A.2 Orchestrator handling of the response

After the sub-agent returns:

1. **Parse JSON.** If parse fails → display raw output as fallback, treat as `blocked`, ask user how to proceed.
2. **`status: "completed"`** → display the standard story-completion block (see § B 3.9 template), log summary, dispatch next story.
3. **`status: "blocked"`** → display blocker reason(s) and `recommended_next`, ask user `[Continue with next story / Skip to next epic / Abort run]`.
4. **Keep only the summary** in orchestrator memory. The sub-agent's full transcript is not retained — that IS the reset.

---

## § B — In-line Workflow (Paused mode)

Used directly by the orchestrator when the user selected Paused. The orchestrator's context accumulates across stories — the user manages context manually (e.g., `/clear` between stories if it grows too large).

Repeat for each story in scope.

---

### 3.1 Story Initialization

**Display:**
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🚀 Starting: US-XXX — [Story Title]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Story Points: [N]
Epic:         [Epic Name]
Priority:     [P0/P1/P2]
```

**Break down with TodoWrite:**

```javascript
TodoWrite({
  todos: [
    { content: "Read technical spec for US-XXX context",   activeForm: "Reading technical spec",   status: "in_progress" },
    { content: "Implement task 1: [description]",          activeForm: "Implementing task 1",      status: "pending" },
    { content: "Implement task 2: [description]",          activeForm: "Implementing task 2",      status: "pending" },
    { content: "Write unit tests",                          activeForm: "Writing unit tests",       status: "pending" },
    { content: "Write integration tests",                   activeForm: "Writing integration tests", status: "pending" },
    { content: "Write E2E tests",                           activeForm: "Writing E2E tests",        status: "pending" },
    { content: "Verify i18n translations",                  activeForm: "Verifying i18n translations", status: "pending" },  // if I18N-RULES.md exists
    { content: "Run all tests and verify coverage",         activeForm: "Running tests",            status: "pending" },
    { content: "Create git commit",                         activeForm: "Creating git commit",      status: "pending" },
    { content: "Update progress tracking",                  activeForm: "Updating progress",        status: "pending" },
  ],
})
```

---

### 3.2 Read Story Context

- Re-read technical-spec sections relevant to this story.
- Re-read coding standards.
- Understand acceptance criteria.
- Mark todo completed; move on.

---

### 3.3 Implement Tasks

For each task:

1. Mark as `in_progress`.
2. Read existing code files (`Read` tool — never skip).
3. Implement changes following:
   - SOLID & DRY (`code-quality.md`)
   - Technical-spec patterns
   - Project rules
4. Mark `completed`.

**Non-negotiables:**
- Never skip reading existing files.
- No over-engineered solutions.
- No unrequested features.
- Always follow existing patterns.

---

### 3.4 Write Tests (MANDATORY)

**Unit tests:**
- All new functions / components.
- Edge cases + error handling.
- Co-located `*.test.ts` or `__tests__/unit/`.

**Integration tests** — all API endpoints must cover:

| Code | Case |
|------|------|
| 200/201 | Success |
| 400 | Validation error |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not found |
| 500 | Server error |

Plus database interactions.

**E2E tests (if applicable):** critical user flows (Playwright).

**File naming:**
- Unit:        `ComponentName.test.tsx` / `function-name.test.ts`
- Integration: `api-endpoint.integration.test.ts`
- E2E:         `user-flow.e2e.test.ts`

---

### 3.5 Verify i18n (conditional)

**Only if `.project-management/rules/I18N-RULES.md` exists:**

1. Search for hardcoded user-facing text.
2. Verify all text uses translation keys.
3. Check translation files exist for all configured languages.
4. Add missing keys + update JSON files.

**If I18N-RULES.md is absent:** skip entirely; remove the i18n todo from the list.

---

### 3.6 Run Tests (MANDATORY — second-to-last step)

Mark "Run all tests" todo as `in_progress`. Display: `🧪 Running Tests for US-XXX...`

Execute:
1. Vitest (unit + integration): `npm test`
2. Playwright (E2E): `npm run test:e2e`
3. Coverage: `npm run test:coverage` (must be ≥ 80%)

Results block:

```
📊 Test Results:
✅ Unit Tests:         [X]/[X] passed
✅ Integration Tests:  [Y]/[Y] passed
✅ E2E Tests:          [Z]/[Z] passed
✅ Coverage:           [XX]% (target 80%+)
✅ API Status Codes:   all tested (200/400/401/403/404/500)
{{✅ i18n: all translations present}}
```

Validation details: `execute-work-quality-gates.md`.

---

### 3.7 Git Commit (AUTO — final step)

Mark commit todo as `in_progress`. **CRITICAL:** follow `.claude/rules/git.md` — **NO AI credits!**

```bash
git add .
git commit -m "$(cat <<'EOF'
{{type}}: implement US-XXX {{story title}}

- Task 1: {{description}}
- Task 2: {{description}}

Tests: {{X}}/{{X}} passed
Coverage: {{XX}}%
{{i18n: {{languages}} translations added}}
EOF
)"
```

Commit types: `feat:` · `fix:` · `refactor:` · `test:` · `docs:`.

**Forbidden:**
- ❌ "🤖 Generated with Claude Code"
- ❌ "Co-Authored-By: Claude"
- ❌ any AI attribution

On success: `✅ Git commit created: [commit-hash]`. Mark todo completed.

---

### 3.8 Update Progress Tracking (AUTO)

Mark progress todo `in_progress`. Update behavior depends on the tracking mode selected in STEP 0:

- **Phase Only (faster)** — updates only `output/phases/phase-N.md`.
- **Complete (slower)** — updates phase file + `completed.md` + `current-status.md`; recalculates velocity.

Full templates for both modes (file lists, update templates, display blocks): **`execute-work-progress-updates.md`**.

**Never update `blockers.md` automatically** — blockers need human context; edit the file directly.

Mark todo completed.

---

### 3.9 Story Completion

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ US-XXX COMPLETED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Story:    {{story title}}
Points:   {{points}}
Tests:    {{tests_passed}}/{{tests_total}} passed
Coverage: {{coverage}}%
Commit:   {{commit_hash}}
Duration: {{duration}}
```

Clear TodoWrite for the next story.

---

### 3.10 Pause for user (Paused mode only)

```
⏸️  Pause before next story

Continue with next story?
[Yes]              — Continue
[No]               — Stop (resume later with same command)
[Skip to Epic X]   — Jump to specific epic
```
Wait for user, then proceed to next story (go back to 3.1).

> **Continuous mode never reaches this step** — the orchestrator dispatches each story via § A and never enters § B 3.1–3.10 directly. The auto-reset happens between sub-agent dispatches.

When all stories in scope are done → STEP 4 (completion report).

---

**Version:** 3.3.0
**Last Updated:** 2026-04-29 (added § A sub-agent prompt template for Continuous mode auto-context-reset; § B is the legacy Paused-mode in-line workflow)
**Related:**
- `execute-work-quality-gates.md` — test/coverage validation (same gates for both § A and § B)
- `execute-work-progress-updates.md` — per-mode progress-file templates
- `execute-work-dashboard-events.md` + `-mechanics.md` — DASHBOARD auto-update
