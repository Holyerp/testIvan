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

**1. Parse the command argument:**
- `phase N` → Execute all epics and stories in Phase N
- `epic EPIC-X` → Execute all stories in Epic X
- `story US-XXX` → Execute single story US-XXX

**2. Ask user for execution mode:**
```
"Execution Mode:"
[1] Continuous (no pauses between stories)
[2] Paused (wait for approval after each story)
```

Store user's choice for later use.

---

### STEP 1: ENTER PLAN MODE (MANDATORY)

**📖 See:** `modules/execute-work-plan-mode.md` for complete plan mode workflow

**Summary:**
1. Read ALL required context files (technical-spec, backlog, rules)
2. Analyze scope (phase/epic/story breakdown)
3. Create detailed plan with estimates, risks, success criteria
4. Wait for user approval ([Yes/No/Revise])

**Output:** Detailed plan approved by user

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

**📖 See:** `modules/execute-work-implementation.md` for complete implementation workflow

**Summary for each story:**
1. Initialize story with TodoWrite breakdown
2. Read story context from technical spec
3. Implement tasks following SOLID & DRY
4. Write tests (unit, integration, E2E)
5. Verify i18n (if I18N-RULES.md exists)
6. Run tests → **PREDZADNJI STEP**
   - See `modules/execute-work-quality-gates.md` for validation
7. Create git commit (NO AI credits) → **ZADNJI STEP**
8. Update progress tracking
9. Check execution mode (continue or pause)

**Quality Gate:** Tests must pass, coverage > 80%, all API codes tested, i18n complete

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

## 📚 Module References

**Detailed workflows available in:**
- `modules/execute-work-plan-mode.md` - STEP 1 (Plan mode)
- `modules/execute-work-implementation.md` - STEP 3 (Implementation loop)
- `modules/execute-work-quality-gates.md` - Validation & quality checks

---

## ⚠️ IMPORTANT NOTES

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
- Fix the issues (see `modules/execute-work-quality-gates.md`)
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

## 📝 Example Execution

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

**Version:** 3.0.0
**Created:** 2026-03-27
**Command Type:** Implementation Automation
