---
name: execute-work
description: Execute a phase, epic, or story with automatic planning, implementation, testing, and progress tracking
---

# Execute Work Command

Execute implementation of a phase, epic, or individual story with full automation.

---

## Usage

```bash
/execute-work phase N          # Execute entire Phase N
/execute-work epic EPIC-X      # Execute Epic X
/execute-work story US-XXX     # Execute single story US-XXX
```

---

## 📋 YOUR TASK - MANDATORY WORKFLOW

### STEP 0: PARSE ARGUMENTS & MODE SELECTION

1. **Parse the command argument:**
   - `phase N` → Execute all epics and stories in Phase N
   - `epic EPIC-X` → Execute all stories in Epic X
   - `story US-XXX` → Execute single story US-XXX

2. **Ask user for execution mode:**
   ```
   "Execution Mode:"
   [1] Continuous (no pauses between stories)
   [2] Paused (wait for approval after each story)
   ```

   Store user's choice for later use.

---

### STEP 1: ENTER PLAN MODE (MANDATORY)

**Display:**
```
📋 [PLAN MODE ACTIVATED]

Analyzing: [Phase N / Epic X / Story US-XXX]
```

#### 1.1 Read ALL Required Context Files

**MANDATORY FILES - Read in this order:**

1. `.project-management/output/docs/technical-spec.md`
   - Architecture decisions
   - Design patterns
   - Tech stack details

2. `.project-management/input/backlog.md`
   - All user stories in scope
   - Epic details
   - Dependencies

3. `.CLAUDE.MD`
   - Core development standards
   - Workflow requirements
   - Quality gates

4. `.claude/rules/code-quality.md`
   - SOLID & DRY principles (MANDATORY)
   - Code standards

5. `.claude/rules/testing.md`
   - Test coverage requirements (80%+)
   - API status code matrix (200/400/401/403/404/500)
   - Test organization

6. `.claude/rules/git.md`
   - Commit message format
   - **CRITICAL:** NO AI credits in commits

7. `.claude/rules/database.md`
   - Migration requirements (if database work)

8. `.claude/rules/stack-specific.md`
   - Framework-specific guidelines

9. `.project-management/rules/project-rules.md`
   - Project-specific conventions
   - Business logic rules

10. `.project-management/rules/I18N-RULES.md` **(CONDITIONAL)**
    - **IF this file exists:** i18n is MANDATORY for all user-facing text
    - **IF file missing:** Skip i18n requirements

11. `.project-management/rules/TESTING-RULES.md` **(CONDITIONAL)**
    - **IF this file exists:** Apply project-specific testing rules
    - **IF file missing:** Use only general testing.md rules

**Display progress:**
```
Context Read:
✅ Technical spec
✅ Backlog
✅ Core standards
✅ Code quality rules
✅ Testing requirements
✅ Git workflow
✅ Project rules
{{✅ i18n rules (if exists)}}
{{✅ Project testing rules (if exists)}}
```

#### 1.2 Analyze Scope

**For Phase execution:**
- List all epics in the phase
- List all stories in each epic
- Identify dependencies between stories
- Calculate total story points

**For Epic execution:**
- List all stories in the epic
- Identify story dependencies
- Calculate total story points

**For Story execution:**
- Read story details from backlog.md
- Identify dependencies
- Break down into tasks

#### 1.3 Create Detailed Plan

**Plan must include:**

```markdown
🎯 SCOPE: [Phase N / Epic X / Story US-XXX]

📊 BREAKDOWN:

{{For Phase/Epic:}}
Epic 1: [Name] ([X] stories, [Y] points)
├─ US-001: [Title] (P0, 5 points)
│  ├─ Task: [Task description]
│  ├─ Task: [Task description]
│  └─ Tests: Unit (3), Integration (2), E2E (1)
├─ US-002: [Title] (P1, 3 points)
│  └─ ...
└─ Dependencies: US-001 → US-002

{{For Single Story:}}
US-XXX: [Title] (P0, 5 points)
├─ Task 1: [Description]
├─ Task 2: [Description]
├─ Task 3: [Description]
└─ Tests Required:
   ├─ Unit: [List]
   ├─ Integration: [List]
   └─ E2E: [List]

📈 ESTIMATES:
- Total Stories: [N]
- Total Story Points: [X]
- Total Tasks: [Y]
- Estimated Duration: [Z] hours/days/weeks
- Tests to Write: ~[N] tests

⚠️ DEPENDENCIES:
- Internal: [List dependencies]
- External: [List external dependencies]

🔴 RISKS:
- [Risk 1]: [Description] (Impact: High/Medium/Low)
- [Risk 2]: [Description] (Impact: High/Medium/Low)

✅ SUCCESS CRITERIA:
- All tests passing (unit, integration, E2E)
- Coverage > 80%
- All API status codes tested (200/400/401/403/404/500)
{{- i18n translations added (if I18N-RULES.md exists)}}
- SOLID & DRY principles followed
- Documentation updated
- Git commits follow conventions (NO AI credits)

🎯 IMPLEMENTATION STRATEGY:
[Explain approach, patterns to use, key decisions]
```

#### 1.4 Wait for User Approval

**Display:**
```
📋 [END OF PLAN]

✅ Proceed with implementation of this plan?

[Yes] - Start implementation
[No] - Cancel execution
[Revise] - Modify plan
```

**Wait for user response.**
- If "Yes" → Continue to STEP 2
- If "No" → Exit command
- If "Revise" → Ask what to change, regenerate plan, wait again

---

### STEP 2: EXIT PLAN MODE → ENTER IMPLEMENTATION MODE

**Display:**
```
🚀 [EXITING PLAN MODE - ENTERING IMPLEMENTATION MODE]

Starting implementation...
Execution Mode: [Continuous / Paused]
```

---

### STEP 3: IMPLEMENTATION LOOP

**For each story in scope (phase, epic, or single story):**

#### 3.1 Story Initialization

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

#### 3.2 Read Story Context

- Re-read technical spec sections relevant to this story
- Re-read coding standards
- Understand acceptance criteria
- Mark todo as completed, move to next

#### 3.3 Implement Tasks

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

#### 3.4 Write Tests (MANDATORY)

**Mark testing tasks as in_progress, then:**

1. **Unit Tests:**
   - Test all new functions/components
   - Test edge cases
   - Test error handling
   - Co-locate with code or in `__tests__/unit/`

2. **Integration Tests:**
   - Test API endpoints
   - **MANDATORY:** Test ALL status codes:
     - 200/201 (success)
     - 400 (validation error)
     - 401 (unauthorized)
     - 403 (forbidden)
     - 404 (not found)
     - 500 (server error)
   - Test database interactions

3. **E2E Tests (if applicable):**
   - Test critical user flows
   - Use Playwright

**Test file naming:**
- Unit: `ComponentName.test.tsx` or `function-name.test.ts`
- Integration: `api-endpoint.integration.test.ts`
- E2E: `user-flow.e2e.test.ts`

**Mark each testing todo as completed after writing tests.**

#### 3.5 Verify i18n (CONDITIONAL)

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

#### 3.6 Run Tests (MANDATORY - PREDZADNJI STEP)

**Mark "Run all tests" todo as in_progress**

**Display:**
```
🧪 Running Tests for US-XXX...
```

1. **Run Vitest (Unit + Integration):**
   ```bash
   npm test
   ```

   **Capture output:**
   - Tests passed/failed
   - Coverage percentage

2. **Run Playwright (E2E):**
   ```bash
   npm run test:e2e
   ```

   **Capture output:**
   - E2E tests passed/failed

3. **Check Coverage:**
   ```bash
   npm run test:coverage
   ```

   **Verify:** Coverage > 80%

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

#### 3.7 Validation Gate

**IF ANY test failed OR coverage < 80% OR i18n missing (when required):**

**Display:**
```
⚠️ VALIDATION FAILED for US-XXX

Issues:
- [List failed tests]
- [Coverage: XX% (need 80%+)]
- [Missing i18n translations: ...]

🔧 Fixing issues...
```

**Fix the issues:**
1. Analyze test failures
2. Fix bugs in code
3. Add missing tests for coverage
4. Add missing i18n translations
5. Re-run tests
6. **REPEAT until ALL tests pass AND coverage > 80% AND i18n complete**

**CRITICAL:** Story is NOT complete until validation passes!

**IF ALL tests passed AND coverage OK AND i18n OK:**

Mark "Run all tests" todo as completed → Proceed to git commit

---

#### 3.8 Git Commit (AUTO - ZADNJI STEP)

**Mark "Create git commit" todo as in_progress**

**CRITICAL: Follow `.claude/rules/git.md` - NO AI CREDITS!**

**Create commit with this exact format:**

```bash
git add .

git commit -m "$(cat <<'EOF'
{{type}}: implement US-XXX {{story title}}

{{List of completed tasks}}
- Task 1: {{description}}
- Task 2: {{description}}
- Task 3: {{description}}

Tests: {{X}}/{{X}} passed
Coverage: {{XX}}%
{{i18n: {{languages}} translations added}}
EOF
)"
```

**Commit type:**
- `feat:` - New feature
- `fix:` - Bug fix
- `refactor:` - Code refactoring
- `test:` - Test additions
- `docs:` - Documentation

**Example commit:**
```bash
git commit -m "$(cat <<'EOF'
feat: implement US-015 user registration form

- Add registration form component
- Implement validation with Zod schema
- Create POST /api/register endpoint
- Add bcrypt password hashing
- Write unit tests for form validation
- Write integration tests for API endpoint
- Add E2E test for registration flow

Tests: 18/18 passed
Coverage: 92%
i18n: en, de, sr translations added
EOF
)"
```

**ABSOLUTELY FORBIDDEN IN COMMITS:**
- ❌ "🤖 Generated with Claude Code"
- ❌ "Co-Authored-By: Claude"
- ❌ ANY AI attribution

**After successful commit:**
```
✅ Git commit created: [commit-hash]
```

Mark "Create git commit" todo as completed

---

#### 3.9 Update Progress Tracking (AUTO)

**Mark "Update progress tracking" todo as in_progress**

**Update `.project-management/output/phases/phase-N.md`:**

1. Find the story section
2. Update status to "Completed"
3. Add completion timestamp
4. Add test metrics
5. Add commit hash
6. Update phase progress metrics:
   - Completed story points
   - Test count
   - Coverage percentage
   - Commit count

**Use Edit tool to update the file.**

**Display:**
```
📊 Progress Updated:
- Phase N: {{completed_points}}/{{total_points}} points ({{percentage}}%)
- Tests: {{total_tests}} passing
- Coverage: {{coverage}}%
```

Mark "Update progress tracking" todo as completed

---

#### 3.10 Story Completion

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

#### 3.11 Check Execution Mode

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

### STEP 4: COMPLETION REPORT

**When ALL stories in scope are completed:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🎉 [Phase N / Epic X / Story US-XXX] - COMPLETED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📈 STATISTICS:

Stories Completed:     {{completed_stories}} / {{total_stories}}
Story Points:          {{completed_points}} / {{total_points}} ({{percentage}}%)
Tests Written:         {{tests_written}}
Tests Passing:         {{tests_passing}} / {{tests_total}}
Code Coverage:         {{coverage}}%
Git Commits:           {{commit_count}}
Duration:              {{duration}}
Average Velocity:      {{velocity}} points/day

✅ QUALITY METRICS:

- SOLID & DRY Compliance:  ✅ Pass
- Test Coverage:           ✅ {{coverage}}% (Target: 80%+)
- API Status Codes:        ✅ All tested
{{- i18n Translations:       ✅ Complete}}
- Linting:                 ✅ No errors
- Git Conventions:         ✅ Followed (NO AI credits)

🎯 NEXT STEPS:

{{If Phase completed:}}
Phase {{N}} is complete! Ready to start Phase {{N+1}}.
Run: /execute-work phase {{N+1}}

{{If Epic completed:}}
Epic {{X}} is complete! Continue with remaining epics in Phase {{N}}.

{{If Story completed:}}
Story US-XXX is complete! Continue with remaining stories.

📊 PHASE PROGRESS:

Phase {{N}}: {{completed_points}}/{{total_points}} points ({{percentage}}%)

[████████████░░░░░░░░] {{percentage}}%

```

---

## IMPORTANT NOTES

### Mandatory Requirements

1. **Plan Mode is MANDATORY** - Never start implementation without plan approval
2. **Tests are MANDATORY** - Story is NOT done until tests pass
3. **Coverage Target: 80%+** - Must be met before marking story complete
4. **API Status Codes** - ALL must be tested (200/400/401/403/404/500)
5. **i18n Compliance** - IF I18N-RULES.md exists, translations are MANDATORY
6. **Git Conventions** - NO AI credits in commits (see git.md)
7. **SOLID & DRY** - Must follow principles from code-quality.md
8. **TodoWrite Usage** - Use TodoWrite for task breakdown and tracking

### Quality Gates

**Story can only be marked COMPLETE when:**
- [x] All tasks implemented
- [x] All tests written (unit, integration, E2E)
- [x] All tests passing
- [x] Coverage > 80%
- [x] All API status codes tested
- [x] i18n translations added (if required)
- [x] SOLID & DRY principles followed
- [x] Git commit created (no AI credits)
- [x] Progress tracking updated

### Error Handling

**If tests fail:**
- Do NOT mark story as complete
- Fix the issues
- Re-run tests
- Repeat until all pass

**If user cancels mid-execution:**
- Mark current story as "In Progress" in phase file
- Update progress with partial completion
- User can resume with same command later

**If dependency is missing:**
- Mark story as "Blocked"
- Update progress file with blocker info
- Continue with non-dependent stories (if any)

---

## Example Execution

```bash
# User runs:
/execute-work phase 1

# Claude asks:
"Execution Mode?"
[1] Continuous
[2] Paused

# User selects: 1

# Claude enters PLAN MODE:
📋 [PLAN MODE ACTIVATED]

Context Read:
✅ Technical spec
✅ Backlog
✅ Core standards
...

[Shows detailed plan]

✅ Proceed? [Yes/No/Revise]

# User: Yes

# Claude enters IMPLEMENTATION MODE:
🚀 [EXITING PLAN MODE - ENTERING IMPLEMENTATION MODE]

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🚀 Starting: US-001 - Project Setup
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

[Implements US-001]
[Writes tests]
[Runs tests - all pass]
[Creates git commit]
[Updates progress]

✅ US-001 COMPLETED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

▶️  Continuing with next story: US-002

[Repeats for all stories]

🎉 Phase 1 - COMPLETED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

[Shows completion report]
```

---

**Version:** 1.0
**Created:** 2026-03-27
**Command Type:** Implementation Automation
