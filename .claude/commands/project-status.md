---
name: project-status
description: Generate comprehensive project status report showing progress, completed work, blockers, and next steps
---

# Project Status

**📖 Quick Start:** See [how-to-use/project-status.md](./how-to-use/project-status.md) for quick guide (~80 lines)

Generate a comprehensive overview of the current project status.

---

## Usage

```bash
/project-status
```

Generates detailed status report with metrics, progress, blockers, and recommendations.

---

## 📋 YOUR TASK

**🔧 REFERENCE:**
Quality metrics calculated based on:
- **`.claude/rules/testing.md`** - Test coverage targets, API status code requirements
- **`.claude/rules/code-quality.md`** - SOLID & DRY compliance checks

---

## 🚀 OPTIMIZATION: Use DASHBOARD.md First!

**IMPORTANT:** Before reading all files, check for DASHBOARD.md - it contains pre-calculated metrics!

**Optimized Flow:**
1. ✅ Check if `.project-management/output/progress/DASHBOARD.md` exists
2. ✅ If YES → Read DASHBOARD.md, extract all metrics (saves 60-70% tokens!)
3. ✅ If NO → Calculate metrics from scratch (legacy mode)

**See:** `modules/project-status-calculation.md` for optimization details

---

### STEP 1: Detect Project Structure & Read Files

**📖 See:** `modules/project-status-data-collection.md` for detailed data collection

**STEP 1A: Detect Structure Type**

Check which backlog structure is in use:
```
if exists(".project-management/input/backlog/README.md"):
    → MODULAR structure (new)
    → Read from backlog/README.md + backlog/phase-*.md
else if exists(".project-management/input/backlog.md"):
    → MONOLITHIC structure (legacy)
    → Read from backlog.md
```

**STEP 1B: Read Core Files (All Structures)**

**Priority 1 - Live Dashboard (if exists):**
- `.project-management/output/progress/DASHBOARD.md` - Pre-calculated metrics (READ FIRST!)

**Priority 2 - Project Definition:**
- `.project-management/input/scope.md` - Project scope

**Priority 3 - Backlog (structure-dependent):**
- **Modular structure:** `.project-management/input/backlog/README.md` - Master index with statistics
- **Legacy structure:** `.project-management/input/backlog.md` - Full backlog

**Priority 4 - Execution & Progress:**
- `.project-management/output/phases/phase-*.md` - All phase files
- `.project-management/output/progress/current-status.md` - Current status
- `.project-management/output/progress/completed.md` - Completed work
- `.project-management/output/progress/blockers.md` - Blockers

**Priority 5 - Quality & Bugs:**
- `.project-management/output/bugs/bug-roadmap.md` - Open bugs by severity
- `.project-management/output/bugs/bug-archive.md` - Fixed bugs history

---

### STEP 2: Calculate or Extract Metrics

**📖 See:** `modules/project-status-calculation.md` for detailed calculations

**Optimized Approach:**

**If DASHBOARD.md exists (FAST PATH):**
- ✅ Extract pre-calculated metrics from DASHBOARD.md:
  - Overall completion percentage
  - Phase progress (current + overall)
  - Story points (completed / total)
  - Velocity (points per week)
  - Active blockers count
  - Test coverage
  - Timeline status
- ✅ Only read additional files for details not in DASHBOARD (e.g., detailed bug breakdown)
- ⚡ **Result:** 60-70% faster, uses pre-calculated data

**If DASHBOARD.md doesn't exist (LEGACY PATH):**
- Calculate all metrics from scratch:
  - Overall completion percentage
  - Phase progress (current + overall)
  - Story points (completed / total)
  - Velocity (points per week)
  - Test coverage
  - Quality metrics:
    - Bug counts by severity (Critical, High, Medium, Low) from bug-roadmap.md
    - Fixed bugs (last 7 days, last 30 days) from bug-archive.md
    - Bug rate (bugs per story)
    - Tech debt items
  - Timeline adherence (on track / delayed)

---

### STEP 3: Generate Status Report

**Display comprehensive report:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 PROJECT STATUS REPORT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

**Generated:** {{DATE}}
**Project:** {{PROJECT_NAME}}
**Current Phase:** Phase {{N}} - {{PHASE_NAME}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🎯 EXECUTIVE SUMMARY

**Overall Status:** {{🟢 On Track | 🟡 At Risk | 🔴 Delayed}}

**Key Metrics:**
- Overall Completion: {{XX}}%
- Phase {{N}} Progress: {{YY}}%
- Velocity: {{Z}} points/week
- Test Coverage: {{CC}}%
- Quality: {{🟢🟡🔴}} {{bug_count}} bugs

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 📅 CURRENT PHASE STATUS

**Phase {{N}}: {{PHASE_NAME}}**

Progress: [████████████░░░░░░░░] {{percentage}}%

- Started: {{START_DATE}}
- Expected End: {{END_DATE}}
- Status: {{🟢 On Track | 🟡 At Risk | 🔴 Delayed}}

**Completed:** {{completed_stories}} / {{total_stories}} stories
**Story Points:** {{completed_points}} / {{total_points}} points

**Current Sprint/Week:**
- Working on: {{CURRENT_STORIES}}
- Completed this week: {{WEEKLY_COMPLETED}}
- Blocked: {{BLOCKED_COUNT}} stories

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 📈 OVERALL PROJECT PROGRESS

**All Phases:**
{{For each phase:}}
- Phase {{N}}: [███░░░░░] {{percentage}}% ({{status}})

**Timeline:**
- Project Start: {{START_DATE}}
- Target Launch: {{LAUNCH_DATE}}
- Days Elapsed: {{ELAPSED_DAYS}}
- Days Remaining: {{REMAINING_DAYS}}
- On Schedule: {{✅ Yes | ⚠️ At Risk | ❌ Delayed}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## ✅ COMPLETED WORK (Recent)

**Last 7 Days:**
{{List completed stories:}}
- ✅ US-XXX: {{story_title}} ({{points}} points)
- ✅ US-YYY: {{story_title}} ({{points}} points)

**Key Achievements:**
- {{achievement_1}}
- {{achievement_2}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🔴 BLOCKERS & RISKS

**Active Blockers:** {{blocker_count}}
{{If blockers exist:}}
- 🔴 {{blocker_1}} (Impact: High/Medium/Low)
- 🔴 {{blocker_2}} (Impact: High/Medium/Low)

**Risks:**
- ⚠️  {{risk_1}} (Probability: High/Medium/Low)
- ⚠️  {{risk_2}} (Probability: High/Medium/Low)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🧪 QUALITY METRICS

**Testing:**
- Total Tests: {{test_count}} ({{passing}} passing, {{failing}} failing)
- Coverage: {{coverage}}% (Target: 80%+)
- API Status Codes: {{✅ All tested | ⚠️ Missing}}

**Code Quality:**
- SOLID & DRY: {{✅ Compliant | ⚠️ Issues}}
- Linting Errors: {{error_count}}
- Tech Debt Items: {{debt_count}}

**Bugs:**
- Open: {{open_bugs}} (🔴 Critical: {{critical}}, 🟠 High: {{high}}, 🟡 Medium: {{medium}}, 🟢 Low: {{low}})
- Fixed This Week: {{fixed_bugs}}
- Fixed This Month: {{fixed_month}}
- Bug Rate: {{rate}} bugs/story
- Source: `.project-management/output/bugs/bug-roadmap.md`

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 📊 VELOCITY & TRENDS

**Velocity (Story Points per Week):**
- Current: {{current_velocity}}
- Average: {{avg_velocity}}
- Trend: {{↑ Increasing | → Stable | ↓ Decreasing}}

**Completion Rate:**
- Stories/Week: {{stories_per_week}}
- Estimated Completion: {{estimated_date}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🎯 NEXT STEPS

**Immediate (This Week):**
1. {{next_task_1}}
2. {{next_task_2}}
3. {{next_task_3}}

**Upcoming (Next Week):**
1. {{upcoming_task_1}}
2. {{upcoming_task_2}}

**Phase {{N+1}} Preview:**
- Start Date: {{next_phase_start}}
- Focus: {{next_phase_focus}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 💡 RECOMMENDATIONS

{{Based on analysis:}}
- ✅ {{recommendation_1}}
- ⚠️  {{recommendation_2}}
- 🔴 {{critical_action_needed}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 📚 Module References

**Detailed workflows available in:**
- `modules/project-status-data-collection.md` - Data collection strategies (✅ Updated for DASHBOARD.md + modular backlog)
- `modules/project-status-calculation.md` - Metrics calculation formulas (✅ Updated with optimization flow)

---

## 🔄 Backward Compatibility

**This command automatically detects and supports:**

1. **Modular Backlog Structure (NEW):**
   - Reads from `input/backlog/README.md`
   - Uses pre-calculated metrics from `output/progress/DASHBOARD.md`
   - 60-70% token savings
   - Faster execution

2. **Monolithic Backlog Structure (LEGACY):**
   - Reads from `input/backlog.md`
   - Calculates all metrics from scratch
   - Still fully functional
   - Consider running `/migrate-to-modular` to upgrade

**Detection is automatic** - no user action needed!

---

## ⚠️ IMPORTANT NOTES

### Status Indicators

**Overall Status:**
- 🟢 **On Track** - Meeting deadlines, good velocity, no major blockers
- 🟡 **At Risk** - Some delays, velocity decreasing, blockers present
- 🔴 **Delayed** - Behind schedule, multiple blockers, needs intervention

### Metric Calculations

**Completion Percentage:**
```
(Completed Story Points / Total Story Points) × 100
```

**Velocity:**
```
Story Points Completed / Weeks Elapsed
```

**Timeline Status:**
```
If (Days Remaining > Estimated Days Needed) → On Track
Else If (Days Remaining > 80% of Estimated) → At Risk
Else → Delayed
```

### Quality Checks

**Before generating report:**
- [ ] All phase files read
- [ ] Progress files analyzed
- [ ] Metrics calculated correctly
- [ ] Trends identified
- [ ] Recommendations based on data

---

## 📝 Example Execution

```bash
# User runs:
/project-status

# Claude generates:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 PROJECT STATUS REPORT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

**Generated:** 2026-03-27
**Project:** E-Commerce Platform
**Current Phase:** Phase 2 - Core Features

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

## 🎯 EXECUTIVE SUMMARY

**Overall Status:** 🟢 On Track

**Key Metrics:**
- Overall Completion: 45%
- Phase 2 Progress: 60%
- Velocity: 25 points/week
- Test Coverage: 87%
- Quality: 🟢 3 bugs

[... detailed report ...]

🎯 NEXT STEPS:
1. Complete US-045 (user profile)
2. Start US-046 (payment integration)
3. Review Phase 3 backlog
```

---

## ✅ Modular Structure Support

**Status:** ✅ Fully Integrated (2026-04-20)

**New Features:**
- ✅ Auto-detects modular vs monolithic backlog structure
- ✅ Reads pre-calculated metrics from DASHBOARD.md (60-70% faster)
- ✅ Uses backlog/README.md for summary statistics
- ✅ Falls back to legacy structure if needed
- ✅ Fully backward compatible

**Performance:**
- **With DASHBOARD.md:** ~550 tokens (reads pre-calculated metrics)
- **Without DASHBOARD.md:** ~2,400 tokens (calculates from scratch)
- **Token Savings:** 60-70% when using modular structure

**See:**
- `COMMAND-STATUS.md` - Implementation tracking
- `modules/backlog-organization.md` - Modular backlog structure
- `modules/live-progress-dashboard.md` - DASHBOARD.md auto-updates

---

**Version:** 3.2.0
**Created:** 2026-03-27
**Updated:** 2026-04-20 (Modular structure support)
**Command Type:** Reporting & Analytics
