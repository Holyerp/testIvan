# Execute Phase/Epic/Story - Quick Guide

**Use when:** Ready to implement planned work (phase, epic, or story)
**Command:** `/execute-work [scope]`
**Time:** Varies (minutes to hours depending on scope)
**Automation:** Full - planning → implementation → testing → commit → progress tracking

**All documentation is in English only.**

---

## 🎯 What It Does

Fully automated implementation workflow:
1. **Plan Mode** - Creates detailed plan, waits for approval
2. **Implementation** - Writes code following SOLID & DRY
3. **Testing** - Writes and runs tests (80%+ coverage required)
4. **Quality Gates** - Validates all requirements met
5. **Git Commit** - Auto-commits (NO AI credits)
6. **Progress Tracking** - Updates phase files and metrics

---

## 📋 Command Formats

```bash
# Execute entire phase (all epics and stories)
/execute-work phase 1

# Execute single epic (all stories in epic)
/execute-work epic EPIC-1

# Execute single story
/execute-work story US-005
```

---

## 🎮 Execution Modes

### Mode 1: Continuous vs Paused

Claude asks at start:
```
Execution Mode:
[1] Continuous - Auto-continues to next story without pausing
[2] Paused - Wait for approval after each story
```

**Recommendation:** Use Continuous for trusted, well-defined work.

### Mode 2: Progress Tracking

Claude asks at start:
```
Progress Tracking Mode:
[1] Phase Only (faster - updates only phase file)
[2] Complete (slower - updates all progress files)
```

**Recommendation:** Use "Phase Only" for faster execution. For complete tracking later, re-run in Complete mode or edit the progress files directly.

**Time difference:** Complete mode adds ~10-30 seconds per story.

---

## 📝 Quick Steps

### STEP 0: ENTER PLAN MODE

**🎯 MANDATORY: Always enter plan mode before execution**

1. Claude reads:
   - Technical specification
   - Backlog
   - Phase plan
   - Core coding standards

2. Analyzes scope:
   - Story breakdown
   - Dependencies
   - Risks
   - Success criteria

3. Creates detailed plan with:
   - Implementation steps
   - Testing strategy
   - Estimated story points
   - Quality gates

4. Presents plan for approval
5. Waits for [Yes/No/Revise]

**Only proceeds to implementation after you approve.**

### STEP 1: Choose Execution Modes

After plan approval:
1. Select **Execution Mode** (Continuous / Paused)
2. Select **Progress Tracking** (Phase Only / Complete)

### STEP 2: Implementation Loop

**For each story in scope:**

1. **TodoWrite Breakdown** - Creates task list for story
2. **Read Story Context** - Gets details from technical spec
3. **Implement** - Writes code following SOLID & DRY principles
4. **Write Tests** - Unit, integration, E2E tests
5. **Run Tests** - Must pass, coverage > 80%
6. **Verify i18n** - If I18N-RULES.md exists
7. **Git Commit** - Auto-commit (NO AI attribution)
8. **Update Progress** - Phase file or all progress files
9. **Continue/Pause** - Based on execution mode

### STEP 3: Quality Gates

**Story marked complete ONLY when:**
- ✅ All tasks implemented
- ✅ All tests written (unit, integration, E2E)
- ✅ All tests passing
- ✅ Coverage > 80%
- ✅ All API status codes tested (200/400/401/403/404/500)
- ✅ i18n translations added (if required)
- ✅ SOLID & DRY principles followed
- ✅ Git commit created (no AI credits)

**If tests fail:** Claude fixes issues, re-runs tests, repeats until all pass.

### STEP 4: Completion Report

Shows:
- Stories completed
- Story points completed
- Tests written/passing
- Code coverage
- Git commits created
- Next steps

---

## 🔍 Example Walkthrough

```
You: /execute-work phase 1

Claude: [STEP 0: Plan Mode]
        Reading context...
        ✅ Technical spec
        ✅ Backlog
        ✅ Phase 1 plan (18 stories, 89 points)
        ✅ Core standards

        PLAN:
        Scope: Phase 1 - Foundation & Setup
        Stories: 18 (US-001 through US-018)
        Epics: 3 (Project Setup, Database, Authentication)

        Implementation Strategy:
        1. US-001: Project initialization
        2. US-002: Database migrations setup
        [... detailed plan for all 18 stories]

        Risks: None identified
        Success Criteria: All tests pass, 80%+ coverage

        Proceed? [Yes/No/Revise]

You: Yes

Claude: [Execution Mode?]
        [1] Continuous  [2] Paused

You: 1

Claude: [Progress Tracking?]
        [1] Phase Only  [2] Complete

You: 1

Claude: 🚀 Starting implementation...
        Execution Mode: Continuous
        Progress Tracking: Phase Only

        ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        🚀 US-001: Project Setup
        ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        [TodoWrite: 5 tasks created]
        [Implementing task 1/5...]
        [Writing tests...]
        [Running tests... ✅ All pass]
        [Git commit created]
        [Progress updated]

        ✅ US-001 COMPLETED (5 points)
        ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        ▶️ Continuing with US-002...

        [... repeats for all 18 stories]

        🎉 PHASE 1 - COMPLETED
        ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        Stories: 18/18 completed
        Points: 89/89 (100%)
        Tests: 156 written, 156 passing
        Coverage: 87%
        Commits: 18

        NEXT STEPS:
        Ready to start Phase 2!
        Run: /execute-work phase 2
```

---

## ⚠️ Common Issues

| Issue | Solution | Reference |
|-------|----------|-----------|
| Tests fail during execution | Claude auto-fixes and retries | Quality gates module |
| Dependency missing | Marked as "Blocked", continues with other stories | Full docs "Error Handling" |
| Coverage below 80% | Claude adds more tests until threshold met | `.claude/rules/testing.md` |
| User cancels mid-execution | Marks current story as "In Progress", resume later | Full docs "Error Handling" |

---

## 🎯 After Execution

**If you chose "Phase Only" tracking:**

Re-run `/execute-work` in Complete mode next time, or edit the progress files
(`completed.md`, `current-status.md`, `blockers.md`) directly. The `/update-progress`
command was removed in v3.2.0.

**Check overall status:**
```bash
/project-status
```

**Continue to next phase:**
```bash
/execute-work phase 2
```

---

## 📚 Full Documentation

**This is a quick guide (150 lines).**

For complete details, see: [`.claude/commands/execute-work.md`](../execute-work.md) (291 lines)

Includes:
- Detailed implementation loop
- Quality gate validation procedures
- Error handling and recovery
- Module references for plan mode, implementation, and quality gates
