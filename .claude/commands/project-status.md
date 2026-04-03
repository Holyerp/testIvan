---
name: project-status
description: Generate comprehensive project status report showing progress, completed work, blockers, and next steps
---

# Project Status

**📖 Quick Start:** See [how-to-use/check-status.md](./how-to-use/check-status.md) for quick guide (~80 lines)

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

### STEP 1: Read Project Files

**📖 See:** `modules/project-status-data-collection.md` for detailed data collection

**Read all relevant files:**
- `.project-management/input/scope.md` - Project scope
- `.project-management/output/phases/phase-*.md` - All phase files
- `.project-management/output/progress/current-status.md` - Current status
- `.project-management/output/progress/completed.md` - Completed work
- `.project-management/output/progress/blockers.md` - Blockers
- `.project-management/input/backlog.md` - Remaining work
- `.project-management/output/bugs/bug-roadmap.md` - Open bugs by severity
- `.project-management/output/bugs/bug-archive.md` - Fixed bugs history

---

### STEP 2: Calculate Metrics

**📖 See:** `modules/project-status-calculation.md` for detailed calculations

**Calculate key metrics:**
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
- `modules/project-status-data-collection.md` - Data collection strategies
- `modules/project-status-calculation.md` - Metrics calculation formulas

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

**Version:** 3.0.0
**Created:** 2026-03-27
**Command Type:** Reporting & Analytics
