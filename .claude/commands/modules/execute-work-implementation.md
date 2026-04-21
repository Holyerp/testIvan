# Execute Work - Implementation Loop Module

**Referenced by:** `execute-work.md` STEP 3

---

## STEP 3: IMPLEMENTATION LOOP

**For each story in scope (phase, epic, or single story):**

---

### 3.1 Story Initialization

**Display:**
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🚀 Starting: US-XXX - [Story Title]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Story Points: [N]
Epic: [Epic Name]
Priority: [P0/P1/P2]
```

**Use TodoWrite tool to create task breakdown:**
```javascript
TodoWrite({
  todos: [
    { content: "Read technical spec for US-XXX context", activeForm: "Reading technical spec", status: "in_progress" },
    { content: "Implement task 1: [description]", activeForm: "Implementing task 1", status: "pending" },
    { content: "Implement task 2: [description]", activeForm: "Implementing task 2", status: "pending" },
    { content: "Write unit tests", activeForm: "Writing unit tests", status: "pending" },
    { content: "Write integration tests", activeForm: "Writing integration tests", status: "pending" },
    { content: "Write E2E tests", activeForm: "Writing E2E tests", status: "pending" },
    { content: "Verify i18n translations", activeForm: "Verifying i18n translations", status: "pending" },  // if I18N-RULES.md exists
    { content: "Run all tests and verify coverage", activeForm: "Running tests", status: "pending" },
    { content: "Create git commit", activeForm: "Creating git commit", status: "pending" },
    { content: "Update progress tracking", activeForm: "Updating progress", status: "pending" }
  ]
})
```

---

### 3.2 Read Story Context

- Re-read technical spec sections relevant to this story
- Re-read coding standards
- Understand acceptance criteria
- Mark todo as completed, move to next

---

### 3.3 Implement Tasks

**For each task:**
1. Mark task as "in_progress" in TodoWrite
2. Read existing code files (use Read tool)
3. Implement changes following:
   - SOLID & DRY principles (code-quality.md)
   - Technical spec patterns
   - Project rules
4. Mark task as "completed" in TodoWrite
5. Move to next task

**IMPORTANT:**
- NEVER skip reading existing files
- NEVER create over-engineered solutions
- NEVER add unrequested features
- ALWAYS follow existing patterns

---

### 3.4 Write Tests (MANDATORY)

**Mark testing tasks as in_progress, then:**

**1. Unit Tests:**
- Test all new functions/components
- Test edge cases
- Test error handling
- Co-locate with code or in `__tests__/unit/`

**2. Integration Tests:**
- Test API endpoints
- **MANDATORY:** Test ALL status codes:
  - 200/201 (success)
  - 400 (validation error)
  - 401 (unauthorized)
  - 403 (forbidden)
  - 404 (not found)
  - 500 (server error)
- Test database interactions

**3. E2E Tests (if applicable):**
- Test critical user flows
- Use Playwright

**Test file naming:**
- Unit: `ComponentName.test.tsx` or `function-name.test.ts`
- Integration: `api-endpoint.integration.test.ts`
- E2E: `user-flow.e2e.test.ts`

**Mark each testing todo as completed after writing tests.**

---

### 3.5 Verify i18n (CONDITIONAL)

**IF `.project-management/rules/I18N-RULES.md` exists:**

**Mark i18n todo as in_progress, then:**
1. Search for any hardcoded user-facing text
2. Verify all text uses translation keys
3. Check translation files exist for all configured languages
4. If missing translations:
   - Add translation keys
   - Update translation JSON files
5. Mark i18n todo as completed

**IF I18N-RULES.md does NOT exist:**
- Skip this step entirely
- Remove i18n todo from list

---

### 3.6 Run Tests (MANDATORY - SECOND-TO-LAST STEP)

**Mark "Run all tests" todo as in_progress**

**Display:** `🧪 Running Tests for US-XXX...`

**Execute:**

1. **Run Vitest (Unit + Integration):** `npm test`
2. **Run Playwright (E2E):** `npm run test:e2e`
3. **Check Coverage:** `npm run test:coverage` (must be > 80%)

**Display results:**
```
📊 Test Results:
✅ Unit Tests: [X]/[X] passed
✅ Integration Tests: [Y]/[Y] passed
✅ E2E Tests: [Z]/[Z] passed
✅ Coverage: [XX]% (Target: 80%+)
✅ API Status Codes: All tested (200/400/401/403/404/500)
{{✅ i18n: All translations present}}
```

**See:** `modules/execute-work-quality-gates.md` for validation details

---

### 3.7 Git Commit (AUTO - FINAL STEP)

**Mark "Create git commit" todo as in_progress**

**CRITICAL: Follow `.claude/rules/git.md` - NO AI CREDITS!**

**Commit format:**
```bash
git add .

git commit -m "$(cat <<'EOF'
{{type}}: implement US-XXX {{story title}}

{{List of completed tasks}}
- Task 1: {{description}}
- Task 2: {{description}}

Tests: {{X}}/{{X}} passed
Coverage: {{XX}}%
{{i18n: {{languages}} translations added}}
EOF
)"
```

**Commit types:** `feat:` | `fix:` | `refactor:` | `test:` | `docs:`

**ABSOLUTELY FORBIDDEN:**
- ❌ "🤖 Generated with Claude Code"
- ❌ "Co-Authored-By: Claude"
- ❌ ANY AI attribution

**After commit:** `✅ Git commit created: [commit-hash]`

Mark "Create git commit" todo as completed

---

### 3.8 Update Progress Tracking (AUTO)

**Mark "Update progress tracking" todo as in_progress**

**Progress tracking depends on mode selected in STEP 0:**

---

#### Mode 1: Phase Only (Faster)

**Update ONLY `.project-management/output/phases/phase-N.md`:**
1. Find the story section
2. Update status to "Completed"
3. Add completion timestamp
4. Add test metrics
5. Add commit hash
6. Update phase progress metrics

**Display:**
```
📊 Progress Updated (Phase Only):
- Phase N: {{completed_points}}/{{total_points}} points ({{percentage}}%)
- Tests: {{total_tests}} passing
- Coverage: {{coverage}}%

ℹ️  For complete tracking later: re-run in Complete mode or edit progress files directly.
```

---

#### Mode 2: Complete (Slower - Full Update)

**Update ALL progress files:**

**1. Update Phase File (`.project-management/output/phases/phase-N.md`):**
- Find the story section
- Update status to "Completed"
- Add completion timestamp, test metrics, commit hash
- Update phase progress metrics

**2. Update Completed Work (`.project-management/output/progress/completed.md`):**
- Add to current week section (or create new week section)
```markdown
## Week {{WEEK_NUMBER}} ({{DATE_RANGE}})

### Completed Stories
- ✅ US-XXX: {{story_title}} ({{points}} points)
  - Completed: {{DATE}}
  - Tests: {{test_count}} passing
  - Coverage: {{coverage}}%
  - Commit: {{commit_hash}}
```

**3. Update Current Status (`.project-management/output/progress/current-status.md`):**
- Recalculate overall completion percentage
- Update phase progress
- Recalculate velocity: `Story Points Completed / Weeks Elapsed`
- Update test coverage metrics
- Update timeline status

**4. Skip Blockers File:**
- **DO NOT** update `blockers.md` automatically
- Blockers require manual input — edit `blockers.md` directly

**Display:**
```
📊 Progress Updated (Complete):

✅ PHASE FILE:
- Phase N: {{completed_points}}/{{total_points}} points ({{percentage}}%)

✅ COMPLETED WORK:
- Added US-XXX to Week {{WEEK_NUMBER}}

✅ CURRENT STATUS:
- Overall Completion: {{old}}% → {{new}}% (+{{delta}}%)
- Velocity: {{velocity}} points/week
- Tests: {{total_tests}} passing
- Coverage: {{coverage}}%

ℹ️  Note: Blockers not updated (edit blockers.md directly when needed)
```

---

Mark "Update progress tracking" todo as completed

---

### 3.9 Story Completion

**Display:**
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ US-XXX COMPLETED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Story: {{story title}}
Points: {{points}}
Tests: {{tests_passed}}/{{tests_total}} passed
Coverage: {{coverage}}%
Commit: {{commit_hash}}
Duration: {{duration}}
```

**Clear TodoWrite for next story.**

---

### 3.10 Check Execution Mode

**IF execution mode is "Paused":**
```
⏸️  Pause before next story

Continue with next story?
[Yes] - Continue
[No] - Stop execution (can resume later)
[Skip to Epic X] - Jump to specific epic
```
**Wait for user response.**

**IF execution mode is "Continuous":**
```
▶️  Continuing with next story: US-YYY
```

**Proceed to next story in scope (go back to 3.1).**

---

**Next Step:** Continue loop or proceed to STEP 4 (completion report) when all stories done
