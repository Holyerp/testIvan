# Execute Work — Implementation Loop Module

**Referenced by:** `execute-work.md` STEP 3
**Companion:** `execute-work-progress-updates.md` (per-mode progress-file updates)

---

## STEP 3 — Implementation Loop

Repeat for each story in scope (phase, epic, or single story).

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

### 3.10 Check Execution Mode

**Paused:**
```
⏸️  Pause before next story

Continue with next story?
[Yes]              — Continue
[No]               — Stop (resume later with same command)
[Skip to Epic X]   — Jump to specific epic
```
Wait for user.

**Continuous:**
```
▶️  Continuing with next story: US-YYY
```

Proceed to next story (go back to 3.1). When all stories in scope are done → STEP 4 (completion report).

---

**Version:** 3.2.0
**Last Updated:** 2026-04-21 (split: progress-update templates moved to companion)
**Related:**
- `execute-work-quality-gates.md` — test/coverage validation
- `execute-work-progress-updates.md` — per-mode progress-file templates
- `execute-work-dashboard-events.md` + `-mechanics.md` — DASHBOARD auto-update
