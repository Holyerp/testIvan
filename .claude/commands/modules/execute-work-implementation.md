# Execute Work — Implementation Loop Module

**Referenced by:** `execute-work.md` STEP 3
**Companion:** `execute-work-progress-updates.md` (per-mode progress-file updates)
**File-size class:** technical-spec (≤ 600 lines per `.claude/rules/documentation.md` §2.1). This module holds two prompt templates plus orchestrator handling, so it exceeds the 300-line "templates" target by design — the alternative is a four-way split that fragments closely-coupled logic.

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
You are dispatched as a sub-agent to execute exactly ONE unit of work: {{UNIT_ID}}.
You have NO context from previous units — this is intentional. Treat this as a fresh session.

Unit:        {{UNIT_ID}} — {{UNIT_TITLE}}                # US-XXX or BUG-XXX
Type:        {{story | bug}}
Phase:       {{N}}                                       # for stories; "n/a" for bugs
Epic:        {{EPIC_ID}}                                  # for stories; "n/a" for bugs
Unit file:   {{ABSOLUTE_PATH_TO_UNIT_FILE}}              # stories: .project-management/output/phases/phase-N.md
                                                         # bugs:    .project-management/output/bugs/bug-roadmap.md
Tracking:    {{Phase Only | Complete}}
Backlog:     {{modular | monolithic}}
Working dir: {{CWD}}

═══════════════════════════════════════════════════════════════
AUTONOMY BOUNDARY — you may NOT do any of the following:
═══════════════════════════════════════════════════════════════

- Do NOT call slash commands (/add-scope, /add-bug, /promote-requirement,
  /execute-work, etc.). Slash commands are orchestrator-level.
- Do NOT file new backend stories or bugs yourself. On any contract gap
  per .claude/rules/api-first.md, return status:"blocked" with a clear
  recommended_next describing the backend work needed; the orchestrator
  files it.
- Do NOT ask the user questions. You cannot receive answers. If you
  need a decision, return status:"blocked" with a question in blockers.
- Do NOT modify .project-management/output/bugs/bug-roadmap.md unless
  this unit IS a bug fix (Type == "bug").
- Do NOT modify .project-management/input/backlog/* (phase backlogs).
  Story content is read-only from your perspective; only the progress
  state in phase-N.md / bug-roadmap.md / completed.md / current-status.md
  / DASHBOARD.md is yours to update per execute-work-progress-updates.md.

═══════════════════════════════════════════════════════════════
MANDATORY WORKFLOW — every step must complete before you return.
═══════════════════════════════════════════════════════════════

STEP 1 — Read context (do not skip any applicable file):

  Always:
  - {{Unit file}} — only the section for {{UNIT_ID}}
  - .claude/rules/code-quality.md
  - .claude/rules/testing.md
  - .claude/rules/git.md
  - .claude/rules/stack-specific.md
  - .claude/rules/documentation-templates.md
  - .CLAUDE.MD

  If unit touches an HTTP endpoint (handler / service / route file):
  - .claude/rules/api-documentation.md
  - .claude/rules/api-versioning.md             (versioning + change-propagation gate: docs + schemas + ALL related tests + consumer code in same PR)
  - .claude/rules/error-handling-and-logging.md (typed errors, canonical envelope, structured logs, no PII/secrets)
  - .claude/rules/security-and-auth.md          (default-deny middleware, resource-level/IDOR, cookie session config, security headers, audit log)

  If unit touches a data model / migration / enum:
  - .claude/rules/database.md
  - .claude/rules/enums-and-constants.md        (SCREAMING_SNAKE_CASE wire format across all layers)

  If frontend story (Type: Frontend per .claude/rules/screen-driven-backlog.md):
  - .claude/rules/api-first.md                  (Phase A contract verification before any frontend code)
  - .claude/rules/screen-driven-backlog.md      (one-screen-per-story + API endpoints table)
  - .claude/rules/screen-inventory.md           (if input/screens/screen-map.md exists — derives API cols from this story's table on completion)

  If unit generates any artifact that may carry input-document content
  (PRD, scope, backlog edits, status, technical spec, ADR):
  - .claude/rules/anonymization.md              (replace personal names with role labels: the PM, the client, the stakeholder)

  If i18n is in scope:
  - .project-management/rules/I18N-RULES.md     (only if file exists)

  Workflow / orchestration:
  - .claude/commands/modules/execute-work-implementation.md § B (workflow detail)
  - .claude/commands/modules/execute-work-quality-gates.md
  - .claude/commands/modules/execute-work-progress-updates.md
  - .claude/commands/modules/run-tests-framework-commands.md (for project test commands)

STEP 2 — Plan with TodoWrite (mirror the § B 3.1 list — implement, test, i18n, run tests, commit, progress).

STEP 3 — Implement all tasks of {{UNIT_ID}} per .claude/rules/code-quality.md (SOLID & DRY).

STEP 4 — Write tests per .claude/rules/testing.md:
  - Unit + integration + E2E
  - All API status codes 200/400/401/403/404/500 (if endpoint touched)
  - For bugs: include a regression test that fails without the fix and passes with it.

STEP 5 — Verify i18n (only if I18N-RULES.md exists).

STEP 6 — If unit touched any HTTP endpoint: run api-documentation.md gate
  (schema validation in code, typed response, doc block, no drift between code/docs/tests).

STEP 7 — Run tests using project-detected commands per
  .claude/commands/modules/run-tests-framework-commands.md
  (auto-detect package manager from lockfile: pnpm-lock.yaml → pnpm,
  yarn.lock → yarn, bun.lockb → bun, otherwise npm).
  Required gates:
    - All test suites pass (unit + integration + E2E if applicable)
    - Coverage ≥ 80% (inclusive — exactly 80.0% passes)
    - Linter clean (no errors)
  If anything fails: fix → re-run. Loop until all gates pass OR you hit a true blocker.

STEP 8 — Update progress files per execute-work-progress-updates.md and tracking mode "{{Tracking}}":
  - For STORIES (Type == "story"):
      Phase Only: phase-N.md only
      Complete:   phase-N.md + completed.md + current-status.md (+ velocity recalc)
      Modular backlog: also fire DASHBOARD EVENT 3 (story completed)
        per execute-work-dashboard-events.md
  - For BUGS (Type == "bug"):
      In bug-roadmap.md: update BUG-XXX status from "Open" / "In Progress"
      to "Fixed" with commit hash + ISO date.
      Modular backlog: fire DASHBOARD EVENT 5 (bug fixed) per
        execute-work-dashboard-events.md
      Do NOT touch phase-N.md or completed.md for bug fixes (bugs are
      tracked separately).

  Screen-map refresh signal (DO NOT run /screen-map yourself):
  - If unit_type == "story" AND the story is a frontend story
    (Type: Frontend per .claude/rules/screen-driven-backlog.md)
    AND .project-management/input/screens/screen-map.md exists,
    set "screen_map_refresh_needed": true in the JSON return.
    The orchestrator (running outside the sub-agent's clean context)
    will invoke /screen-map after dispatch returns. The sub-agent
    must NOT invoke /screen-map itself.

STEP 9 — Git commit per .claude/rules/git.md:
  - Conventional commit (feat: / fix: / refactor: / test: / docs:)
  - Bug commits use "fix:" and reference BUG-XXX in the body
  - NO AI credits, NO Co-Authored-By: Claude lines

═══════════════════════════════════════════════════════════════
RETURN VALUE — strict JSON only, no surrounding prose.
═══════════════════════════════════════════════════════════════

To return status:"completed", EVERY field below must be filled with
truthful evidence. Any gate that was not met → return "blocked" instead.

On success:
{
  "unit_id":             "{{UNIT_ID}}",
  "unit_type":           "story | bug",
  "status":              "completed",
  "title":               "{{UNIT_TITLE}}",
  "points":              N,                              # n/a for bugs
  "tests":               { "passed": N, "total": N, "suites": ["unit","integration","e2e"] },
  "coverage":            "XX.X%",                        # numeric, e.g. "84.2%"
  "api_codes_tested":    ["200","400","401","403","404","500"],   # or [] if no endpoint
  "i18n":                "complete | not_required",
  "api_docs":            "clean | not_required",
  "frontend_contract":   "verified | not_applicable",    # api-first.md Phase A status
  "linter":              "clean | warnings | not_run",
  "quality_gates_passed": ["tests","coverage","linter","api_docs","i18n","frontend_contract"],
                         # MUST list every gate that was actually checked.
                         # Missing any required gate → status MUST be "blocked".
  "commit_hash":         "abc1234",
  "files_touched":       ["path/to/file.ts", ...],
  "duration_estimate":   "Xm",
  "screen_map_refresh_needed": true | false,    # true if frontend story AND screen-map.md exists
  "blockers":            []
}

On blocker (cannot complete despite retries):
{
  "unit_id":           "{{UNIT_ID}}",
  "unit_type":         "story | bug",
  "status":            "blocked",
  "title":             "{{UNIT_TITLE}}",
  "blockers":          ["one-line reason", "another reason"],
  "partial_state":     "what was/wasn't done; whether anything was committed",
  "recommended_next":  "<one of: 'orchestrator should file backend story for: <gap>' | 'skip and continue' | 'abort run' | 'investigate <reason>'>"
}

ENFORCEMENT:
- status:"completed" REQUIRES every quality gate evidence field set with
  truthful values. If you didn't run the linter, set "linter":"not_run"
  AND return "blocked" — do NOT claim completed.
- For frontend stories per .claude/rules/api-first.md, "frontend_contract"
  MUST be "verified" before completing. If contract gaps exist, return
  blocked with recommended_next describing the backend gap.
- NEVER commit work that fails tests.
- NEVER add AI attribution to commits.
```

### A.2 Orchestrator handling of the response

After the sub-agent returns, the orchestrator MUST:

1. **Parse JSON.**
   - On parse failure → display raw output as fallback, treat as `blocked` with reason `"sub-agent returned malformed JSON"`, run reconciliation (step 5), then ask user how to proceed.
   - On Agent tool error (sub-agent crashed, dispatch refused, network failure, unknown subagent_type) → no JSON exists. Treat as `blocked` with reason `"sub-agent dispatch failed: <error>"`, run reconciliation (step 5). For repeated dispatch failures see § A.3.

2. **Validate gate evidence on `status: "completed"`.** Reject the success claim and re-classify as `blocked` if ANY of:
   - `quality_gates_passed` is missing or empty.
   - For frontend stories: `frontend_contract` ≠ `"verified"`.
   - For units that touched an endpoint: `api_docs` ≠ `"clean"` OR `api_codes_tested` does not include all of `200/400/401/403/404/500`.
   - `linter` == `"not_run"` (linter must be either `clean` or `warnings`; `not_run` is treated as gate failure).
   - `coverage` parses to a number below 80.0.
   - `commit_hash` is missing or empty.

3. **`status: "completed"` (validated)** → display the standard unit-completion block (see § B 3.9 template, adapted for stories vs bugs), log the summary, proceed to dispatch the next unit.

   3a. **Screen-map refresh (frontend stories).** If the sub-agent returned `"screen_map_refresh_needed": true`, the orchestrator invokes `/screen-map` (skill) BEFORE dispatching the next unit. This regenerates the API columns and Status in `input/screens/screen-map.md` from the latest stories — keeps the consolidated screen view in sync with the backlog. If `/screen-map` reports drift items in its summary, the orchestrator surfaces them in the unit-completion block but does NOT block the run.

4. **`status: "blocked"`** → display blocker reason(s) and `recommended_next`. Run reconciliation (step 5). Then ask user `[Continue with next unit / Skip to next epic / Abort run]`. If `recommended_next` proposes filing a backend story, the orchestrator (NOT the sub-agent) does so via `/add-scope` or by appending to the bug roadmap, after user confirmation.

5. **DASHBOARD reconciliation (modular backlog only).** When this story produced `blocked`, malformed JSON, or a dispatch failure:
   - Revert the "Currently Working On" entry that the orchestrator wrote in STEP 3-A.2 — either clear it or replace it with the next unit being dispatched.
   - Do NOT fire EVENT 3 / EVENT 5 (story / bug completed) — only the sub-agent fires those, and only on real success.
   - Append a one-line note to the dashboard's "Recently Completed" or "Blockers" surface so the user sees that something stopped (per execute-work-dashboard-events.md).

6. **Keep only the summary** in orchestrator memory. The sub-agent's full transcript is not retained — that IS the reset.

### A.3 Dispatch failure fallback

If the `Agent` tool itself errors out (e.g., the configured `subagent_type` does not exist on this install, the tool is refused, or the sub-agent crashes immediately without producing JSON), the orchestrator should NOT abort the entire run.

Behavior:

1. On the **first** dispatch failure of a run, display once:
   ```
   ⚠️  Sub-agent dispatch failed for {{UNIT_ID}}: <error>.
   Falling back to in-line execution (§ B) for this unit only.
   If failures continue, the orchestrator will fall back for the rest of the run.
   ```
   Then execute § B in-line for that unit.

2. On the **second consecutive** dispatch failure, display once:
   ```
   ⚠️  Two consecutive sub-agent dispatch failures.
   Falling back to in-line execution (§ B) for the remainder of this run.
   Continuous-mode auto-context-reset is disabled until next /execute-work invocation.
   ```
   Continue the rest of the run via § B.

3. The user may abort at any point with the standard interrupt.

This keeps the framework usable even if `general-purpose` is reconfigured or unavailable on a given install.

---

## § B — In-line Workflow (Paused mode)

Used directly by the orchestrator when the user selected Paused. The orchestrator's context accumulates across stories within the run. If the context grows too large, the user can stop at the next pause (`[No]`) and start a fresh `/execute-work` invocation — running `/clear` mid-run wipes the approved plan and abandons the in-progress story, so it is not a recommended way to "free up context."

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

**Screen-map refresh (frontend stories only).** If the completed unit is a frontend story (Type: Frontend per `.claude/rules/screen-driven-backlog.md`) AND `.project-management/input/screens/screen-map.md` exists, invoke `/screen-map` after the progress files are updated. The command regenerates the API endpoint columns and Status field in the screen map from the latest stories. If `/screen-map` reports drift items, display them in the unit-completion summary but do NOT block — drift is informational. Skip the refresh if the story is backend-only / bug / story without a `**Screen:**` field, OR if the screen-map file does not exist (project is API-only / simple SPA per `.claude/rules/screen-inventory.md` §1).

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

**Version:** 3.3.2
**Last Updated:** 2026-04-29 (3.3.0 § A template; 3.3.1 review-blocker/major fixes; 3.3.2 file-size-class header note, /clear guidance corrected)
**Related:**
- `execute-work-quality-gates.md` — test/coverage validation (same gates for both § A and § B)
- `execute-work-progress-updates.md` — per-mode progress-file templates
- `execute-work-dashboard-events.md` + `-mechanics.md` — DASHBOARD auto-update
