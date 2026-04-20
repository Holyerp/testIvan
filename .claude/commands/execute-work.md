---
name: execute-work
description: Execute a phase, epic, or story with automatic planning, implementation, testing, and progress tracking
---

# Execute Work Command

**📖 Quick Start:** See [how-to-use/execute-phase.md](./how-to-use/execute-phase.md) for quick guide (~150 lines)

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

## 📋 YOUR TASK - MANDATORY WORKFLOW

**🔧 CRITICAL RULES TO FOLLOW:**
Before ANY implementation, you MUST read and follow these rules:
- **`.claude/rules/code-quality.md`** - SOLID & DRY principles (MANDATORY for ALL code)
- **`.claude/rules/testing.md`** - Testing requirements, API status code matrix, coverage targets
- **`.claude/rules/git.md`** - Commit message format (NO AI credits), conventional commits
- **`.CLAUDE.MD`** - Core standards and workflow

**When to read:**
- Plan mode: Read ALL rules to create accurate plan
- Implementation: Follow code-quality.md and testing.md during coding
- Commit: Follow git.md for commit messages (NO AI credits)

---

### STEP 0: PARSE ARGUMENTS & MODE SELECTION

**1. Parse the command argument:**
- `phase N` → Execute all epics and stories in Phase N
- `epic EPIC-X` → Execute all stories in Epic X
- `story US-XXX` → Execute single story US-XXX
- `bug BUG-XXX` → Execute bug fix for BUG-XXX

**2. Ask user for execution mode:**
```
"Execution Mode:"
[1] Continuous (no pauses between stories)
[2] Paused (wait for approval after each story)
```

**3. Ask user for progress tracking mode:**
```
"Progress Tracking Mode:"
[1] Phase Only (faster - updates only phase file)
[2] Complete (slower - updates all progress files)

ℹ️  Recommendation: Use "Phase Only" for faster execution.
   You can run /update-progress later for complete tracking.

   Complete mode updates:
   - Phase file (phase-N.md)
   - Completed work log (completed.md)
   - Current status (current-status.md)
   - Overall metrics and velocity

   Note: Complete mode may add 10-30 seconds per story.
```

Store both choices for later use.

---

### STEP 1: DETECT STRUCTURE & ENTER PLAN MODE (MANDATORY)

**STEP 1A: Detect Backlog Structure (Automatic)**

```
if exists(".project-management/input/backlog/README.md"):
    → MODULAR structure (new)
    → Backlog files: input/backlog/phase-*.md
    → Master index: input/backlog/README.md
    → Dashboard: output/progress/DASHBOARD.md (if exists)
else if exists(".project-management/input/backlog.md"):
    → MONOLITHIC structure (legacy)
    → Backlog file: input/backlog.md
```

Store detected `structure_type` for use in STEP 3.

**STEP 1B: Read Context Files**

**📖 See:** `modules/execute-work-plan-mode.md` for complete plan mode workflow

**Read based on detected structure:**

**Modular Structure:**
1. **Identify target phase** for story/epic/bug
2. Read `input/backlog/README.md` - Master index with statistics
3. Read `input/backlog/phase-N-*.md` - Relevant phase backlog file only (not all files!)
4. Read `output/progress/DASHBOARD.md` - Current progress metrics (if exists)
5. Read `output/phases/phase-N.md` - Execution phase file
6. Read technical spec, rules files
7. For bugs: read `bug-roadmap.md`, affected component files

**Legacy Structure:**
1. Read `input/backlog.md` - Full backlog
2. Read `output/phases/phase-N.md` - Execution phase file
3. Read technical spec, rules files
4. For bugs: read `bug-roadmap.md`, affected component files

**STEP 1C: Analyze & Plan**

1. Analyze scope:
   - Phase/epic/story: breakdown, dependencies, estimates
   - Bug: reproduction steps, affected code, root cause analysis
2. Create detailed plan with estimates, risks, success criteria
3. Wait for user approval ([Yes/No/Revise])

**Bug-Specific Plan Requirements:**
- Read bug details from `.project-management/output/bugs/bug-roadmap.md`
- Analyze affected component/file
- Plan fix approach with root cause analysis
- Include regression test requirements
- Estimate fix complexity (story points)

**Output:** Detailed plan approved by user + `structure_type` detected

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

**📖 See:**
- `modules/execute-work-implementation.md` - Complete implementation workflow
- `modules/execute-work-dashboard-update.md` - DASHBOARD.md auto-update logic (NEW!)

**Summary for each story/bug:**
1. Initialize story/bug with TodoWrite breakdown
2. **Auto-update DASHBOARD.md (if exists):** "Currently Working On" section
3. Read context:
   - Story: from technical spec (or from relevant phase backlog if modular)
   - Bug: from bug-roadmap.md + affected component
4. Implement tasks following `.claude/rules/code-quality.md` (SOLID & DRY principles)
5. Write tests following `.claude/rules/testing.md`:
   - Story: unit, integration, E2E
   - Bug: regression test + existing test updates
   - ALL API status codes: 200/400/401/403/404/500
6. Verify i18n (if I18N-RULES.md exists)
7. Run tests → **SECOND-TO-LAST STEP**
   - See `modules/execute-work-quality-gates.md` for validation
   - **Auto-update DASHBOARD.md:** Quality metrics section
8. Create git commit following `.claude/rules/git.md` (NO AI credits, conventional commits) → **FINAL STEP**
   - Bug commits: reference BUG-XXX in message
9. Update progress tracking:
   - **Modular structure:**
     - Update `output/phases/phase-N.md` (execution phase)
     - **Auto-update DASHBOARD.md:** Today's Progress, Recently Completed, Phase Progress, Overall Progress
     - Update `output/progress/daily-summary.md` (move from "In Progress" to "Completed")
     - Update `output/progress/completed.md` (append completion entry)
     - If tracking mode is "Complete": update all progress files
   - **Legacy structure:**
     - Update `output/phases/phase-N.md`
     - Update progress files based on tracking mode
   - Bug: update bug status (New → In Progress → Fixed), move to archive when complete
10. Check execution mode (continue or pause)

**Quality Gate:** Tests must pass, coverage > 80%, all API codes tested, i18n complete

**🚀 Auto-Updates (Modular Structure Only):**
- **Story started** → Update DASHBOARD.md "Currently Working On"
- **Tests run** → Update DASHBOARD.md "Quality Metrics"
- **Story completed** → Update DASHBOARD.md "Today's Progress", "Recently Completed", progress %
- **Phase completed** → Update DASHBOARD.md "Phase Breakdown"

**See:** `modules/execute-work-dashboard-update.md` for detailed update logic

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
Progress Tracking:     {{Phase Only / Complete}}

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

{{If Progress Tracking Mode was "Phase Only":}}
ℹ️  Note: Only phase file was updated during execution.
   Run /update-progress for complete tracking (completed.md, current-status.md, blockers.md)

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
- `modules/execute-work-dashboard-update.md` - DASHBOARD.md auto-update logic (NEW!)

---

## 🔄 Backward Compatibility & Modular Structure Support

**This command automatically detects and supports:**

1. **Modular Backlog Structure (NEW):**
   - Reads from relevant `input/backlog/phase-*.md` file only (not entire backlog!)
   - Auto-updates `output/progress/DASHBOARD.md` in real-time
   - Updates `daily-summary.md` during work
   - Updates master index `input/backlog/README.md` statistics
   - 60-70% token savings (reads only relevant phase backlog)
   - Always up-to-date progress without running commands

2. **Monolithic Backlog Structure (LEGACY):**
   - Reads from single `input/backlog.md`
   - Updates phase files and progress files
   - Still fully functional
   - Consider running `/migrate-to-modular` to upgrade

**Detection is automatic** - no user action needed!

**Auto-Updates (Modular Structure Only):**

When DASHBOARD.md exists, it's automatically updated during work:

1. **Story started** → "Currently Working On" section updated
2. **Tests run** → "Quality Metrics" section updated
3. **Story completed** → "Today's Progress", "Recently Completed", progress % updated
4. **Phase completed** → "Phase Breakdown" section updated

**Result:** Real-time project visibility without running `/project-status`!

**See:** `modules/execute-work-dashboard-update.md` for update logic

---

## ⚠️ IMPORTANT NOTES

### Execution Modes

**1. Execution Mode (Continuous vs Paused):**
- **Continuous:** Auto-continues to next story without pausing
- **Paused:** Waits for approval after each story

**2. Progress Tracking Mode:**
- **Phase Only (Recommended):** Faster execution, updates only `phase-N.md`
  - Best for: Long phases with many stories
  - Time saved: ~10-30 seconds per story
  - Run `/update-progress` later for complete tracking

- **Complete:** Slower execution, updates ALL progress files
  - Updates: `phase-N.md`, `completed.md`, `current-status.md`
  - Best for: Small phases, final phase completion, or when you need real-time comprehensive tracking
  - Note: Does NOT update `blockers.md` (requires manual input)

### Mandatory Requirements

1. **Plan Mode is MANDATORY** - Never start implementation without plan approval
2. **Tests are MANDATORY** - Story is NOT done until tests pass
3. **Coverage Target: 80%+** - Must be met before marking story complete
4. **API Status Codes** - ALL must be tested (200/400/401/403/404/500) per `.claude/rules/testing.md`
5. **i18n Compliance** - IF I18N-RULES.md exists, translations are MANDATORY
6. **Git Conventions** - NO AI credits in commits, conventional format per `.claude/rules/git.md`
7. **SOLID & DRY** - Must follow principles from `.claude/rules/code-quality.md`
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

## ✅ Modular Structure Support

**Status:** ✅ Integrated (2026-04-20)

**New Features:**
- ✅ Auto-detects modular vs monolithic backlog structure
- ✅ Reads only relevant phase backlog file (60-70% token savings)
- ✅ Auto-updates DASHBOARD.md during work execution
- ✅ Updates daily-summary.md in real-time
- ✅ Updates README.md master index statistics
- ✅ Fully backward compatible with legacy structure

**Performance:**
- **Modular:** Reads 1 phase file (~150 lines) vs entire backlog (~800 lines)
- **Auto-updates:** DASHBOARD.md stays current without manual commands
- **Token Savings:** 60-70% per execution

**See:**
- `COMMAND-STATUS.md` - Implementation tracking
- `modules/execute-work-dashboard-update.md` - Auto-update logic
- `modules/backlog-organization.md` - Modular backlog structure

---

**Version:** 3.1.0
**Created:** 2026-03-27
**Updated:** 2026-04-20 (Modular structure support + DASHBOARD auto-updates)
**Command Type:** Implementation Automation
