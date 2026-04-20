# Live Progress Dashboard Module

**Purpose:** Auto-update progress files during work execution for real-time visibility without running commands.

**Used by:** `/execute-work`, `/update-progress`

---

## Problem

**Current approach:**
- ❌ Must run `/project-status` to see progress
- ❌ Progress files not automatically updated
- ❌ No quick "at-a-glance" view
- ❌ User can't just open a file to see status

**Better approach:**
- ✅ DASHBOARD.md in root - always current, read anytime
- ✅ Auto-update during `/execute-work`
- ✅ Live progress files in output/progress/
- ✅ No command needed - just open file

---

## Dashboard File Structure

### Location

```
.project-management/
└── output/
    └── progress/
        ├── DASHBOARD.md              ← Main live dashboard
        ├── current-status.md         ← Detailed current status
        ├── daily-summary.md          ← Today's work summary
        ├── weekly-report.md          ← This week's summary
        ├── completed.md              ← All completed work log
        └── blockers.md               ← Active blockers
```

---

## DASHBOARD.md (Main Live View)

**Purpose:** One-file overview - always current, updated automatically

**Location:** `.project-management/output/progress/DASHBOARD.md`

**Content:**

```markdown
# 📊 Project Dashboard

**Last Updated:** {{TIMESTAMP}} (auto-updated during work)
**Current Phase:** Phase {{PHASE_NUMBER}} - {{PHASE_NAME}}

---

## 🎯 Quick Status

| Metric | Value | Target | Status |
|--------|-------|--------|--------|
| **Overall Progress** | {{OVERALL_PCT}}% | 100% | {{OVERALL_STATUS}} |
| **Current Phase** | {{PHASE_PCT}}% | 100% | {{PHASE_STATUS}} |
| **Stories Completed** | {{COMPLETED}}/{{TOTAL}} | {{TOTAL}} | {{STORY_STATUS}} |
| **Story Points Done** | {{POINTS_DONE}}/{{POINTS_TOTAL}} | {{POINTS_TOTAL}} | {{POINTS_STATUS}} |
| **Test Coverage** | {{COVERAGE}}% | 80% | {{TEST_STATUS}} |

**Legend:** 🟢 On Track | 🟡 At Risk | 🔴 Off Track

---

## 📅 Today's Progress ({{TODAY_DATE}})

**Stories Completed Today:** {{TODAY_COUNT}}
- ✅ US-005: Product listing screen (5 pts) - Completed at 14:32
- ✅ US-006: Product detail screen (3 pts) - Completed at 16:15

**Currently Working On:**
- 🔄 US-007: Shopping cart UI (8 pts) - 60% complete

**Story Points Completed Today:** {{TODAY_POINTS}}

---

## 🚀 Current Phase: {{PHASE_NAME}}

**Goal:** {{PHASE_GOAL}}
**Duration:** {{START_DATE}} to {{END_DATE}}
**Progress:** {{PHASE_PCT}}% ({{PHASE_DONE}}/{{PHASE_TOTAL}} stories)

### Active Stories

| Story | Status | Progress | Assignee |
|-------|--------|----------|----------|
| US-007 | 🔄 In Progress | 60% | - |
| US-008 | 📋 Todo | 0% | - |
| US-009 | 📋 Todo | 0% | - |

### Recently Completed

| Story | Completed | Points |
|-------|-----------|--------|
| US-006 | 2026-04-14 16:15 | 3 |
| US-005 | 2026-04-14 14:32 | 5 |
| US-004 | 2026-04-13 17:20 | 5 |

---

## ⚠️ Active Blockers

{{#if blockers}}
**Count:** {{BLOCKER_COUNT}}

| ID | Title | Severity | Affected Stories | Status |
|----|-------|----------|------------------|--------|
| BLOCKER-001 | API rate limiting issue | High | US-010, US-011 | 🔴 Unresolved |
{{else}}
✅ No active blockers
{{/if}}

---

## 📈 Velocity & Timeline

**Current Velocity:** {{VELOCITY}} points/day
**Average Velocity:** {{AVG_VELOCITY}} points/day
**Velocity Trend:** {{TREND}} ({{TREND_PCT}}%)

**Projected Completion:** {{PROJECTED_DATE}}
**Target Completion:** {{TARGET_DATE}}
**Timeline Status:** {{TIMELINE_STATUS}}

---

## 🧪 Quality Metrics

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| Test Coverage | {{COVERAGE}}% | 80% | {{COV_STATUS}} |
| Passing Tests | {{PASSING}}/{{TOTAL_TESTS}} | {{TOTAL_TESTS}} | {{TEST_STATUS}} |
| Linting Errors | {{LINT_ERRORS}} | 0 | {{LINT_STATUS}} |
| Open Bugs | {{BUGS_OPEN}} | < 5 | {{BUG_STATUS}} |

---

## 📊 Phase Breakdown

| Phase | Status | Stories | Points | Progress |
|-------|--------|---------|--------|----------|
| Phase 1: Foundation | ✅ Complete | 12/12 | 47/47 | 100% |
| Phase 2: Core Features | 🔄 Active | 8/15 | 34/62 | 53% |
| Phase 3: Advanced | ⏸️ Pending | 0/10 | 0/45 | 0% |
| Phase 4: Polish | ⏸️ Pending | 0/8 | 0/31 | 0% |

---

## 🔗 Quick Links

- **[Current Phase Plan](../phases/phase-2.md)** - Detailed phase 2 stories
- **[Backlog](../../input/backlog/)** - All project backlogs
- **[Detailed Status](current-status.md)** - Full status report
- **[Completed Work](completed.md)** - Complete history
- **[Blockers](blockers.md)** - All blockers (active & resolved)

---

**💡 Tip:** This file updates automatically during `/execute-work`. No need to run commands - just refresh to see latest progress!

**Last Auto-Update:** {{LAST_UPDATE_COMMAND}} at {{LAST_UPDATE_TIME}}
```

---

## Auto-Update Triggers

### When to Update DASHBOARD.md

**During `/execute-work`:**
1. **Story started** → Update "Currently Working On"
2. **Story completed** → Update "Today's Progress" + "Recently Completed"
3. **Test run** → Update "Quality Metrics"
4. **Blocker encountered** → Update "Active Blockers"
5. **Phase completed** → Update "Phase Breakdown"

**Frequency:** After every significant event

### Update Process

**STEP 1: Calculate new metrics**
```typescript
const metrics = {
  overallProgress: (completedStories / totalStories) * 100,
  phaseProgress: (completedInPhase / totalInPhase) * 100,
  todayPoints: sumPointsCompletedToday(),
  velocity: calculateVelocity(),
  // ...
};
```

**STEP 2: Read template (or previous dashboard)**

**STEP 3: Replace placeholders with current data**

**STEP 4: Write DASHBOARD.md**

**STEP 5: Add timestamp footer**
```markdown
---
_Auto-updated by /execute-work story US-007 at 2026-04-14 16:45:32_
```

---

## current-status.md (Detailed Status)

**Purpose:** Comprehensive status report (generated by `/project-status` or auto-updated)

**Content:** Full report with:
- Detailed phase breakdown
- All stories with status
- Complete blocker list
- Full quality metrics
- Budget tracking
- Timeline analysis

**Size:** Can be 500+ lines (comprehensive)

**Update:** Less frequent than DASHBOARD.md (once daily or on-demand)

---

## daily-summary.md (Today's Work)

**Purpose:** What happened today

**Location:** `.project-management/output/progress/daily-summary.md`

**Content:**

```markdown
# Daily Summary - {{DATE}}

**Stories Completed:** {{COUNT}}
**Story Points:** {{POINTS}}
**Bugs Fixed:** {{BUGS}}
**Tests Added:** {{TESTS}}

---

## ✅ Completed Stories

### US-005: Product listing screen (5 points)
**Completed:** 14:32
**Time Spent:** 3.5 hours
**Files Changed:** 8 files
**Tests Added:** 12 tests

**What Was Done:**
- Created product grid component
- Implemented infinite scroll
- Added loading skeleton
- Wrote unit tests
- Wrote integration tests

---

### US-006: Product detail screen (3 points)
**Completed:** 16:15
**Time Spent:** 1.5 hours
**Files Changed:** 4 files
**Tests Added:** 6 tests

**What Was Done:**
- Created detail view component
- Implemented image carousel
- Added "Add to Cart" button
- Wrote component tests

---

## 🔄 In Progress

### US-007: Shopping cart UI (8 points)
**Started:** 16:20
**Progress:** 60%
**Time Spent So Far:** 45 minutes

**Done:**
- Cart item list component
- Quantity controls

**Remaining:**
- Remove item swipe gesture
- Subtotal calculation
- Tests

---

## 📊 Summary

**Total Working Time:** 5.5 hours
**Average Points per Hour:** 1.45
**Velocity Today:** 8 points (completed)

---

**Next Day Plan:**
- Finish US-007: Shopping cart UI
- Start US-008: Checkout flow
```

**Update:** Real-time during work, archived at end of day

---

## weekly-report.md (This Week)

**Purpose:** Weekly summary for stakeholders

**Location:** `.project-management/output/progress/weekly-report.md`

**Content:**

```markdown
# Weekly Report - Week of {{WEEK_START}}

**Week:** {{WEEK_NUMBER}}/{{TOTAL_WEEKS}}
**Period:** {{START_DATE}} to {{END_DATE}}

---

## 📊 Summary

| Metric | This Week | Last Week | Change |
|--------|-----------|-----------|--------|
| Stories Completed | {{THIS_WEEK}} | {{LAST_WEEK}} | {{CHANGE}} |
| Story Points | {{THIS_POINTS}} | {{LAST_POINTS}} | {{POINTS_CHANGE}} |
| Velocity | {{THIS_VEL}} | {{LAST_VEL}} | {{VEL_CHANGE}} |
| Bugs Fixed | {{BUGS_THIS}} | {{BUGS_LAST}} | {{BUGS_CHANGE}} |

---

## ✅ Completed This Week

**Total:** {{COUNT}} stories, {{POINTS}} points

### Monday ({{DATE}})
- US-001: Feature X (5 pts)
- US-002: Feature Y (3 pts)

### Tuesday ({{DATE}})
- US-003: Feature Z (8 pts)

### Wednesday ({{DATE}})
- US-004: Feature W (5 pts)
- US-005: Feature V (5 pts)

...

---

## 🎯 Goals for Next Week

**Target:** {{NEXT_WEEK_POINTS}} points, {{NEXT_WEEK_STORIES}} stories

**Planned Stories:**
- US-010: {{TITLE}} ({{POINTS}} pts)
- US-011: {{TITLE}} ({{POINTS}} pts)
- US-012: {{TITLE}} ({{POINTS}} pts)

---

## ⚠️ Risks & Issues

{{#if risks}}
- {{RISK_1}}
- {{RISK_2}}
{{else}}
✅ No major risks identified
{{/if}}

---

## 📈 Trends

**Velocity Trend:** {{TREND_DIRECTION}}
**Quality Trend:** {{QUALITY_TREND}}
**Blocker Trend:** {{BLOCKER_TREND}}
```

**Update:** End of each week (Friday evening or Monday morning)

---

## Integration with /execute-work

### Update Sequence During Work

**When `/execute-work story US-XXX` runs:**

```
1. TodoWrite creates tasks
2. Implementation begins
   → Update DASHBOARD.md: "Currently Working On"
   → Update daily-summary.md: "In Progress" section

3. Tests run
   → Update DASHBOARD.md: "Quality Metrics"

4. Story completed
   → Update DASHBOARD.md:
      - "Today's Progress" (add story)
      - "Recently Completed" (add to list)
      - "Current Phase" progress %
      - "Story Points Done"
   → Update daily-summary.md:
      - Move from "In Progress" to "Completed Stories"
      - Calculate time spent
   → Update completed.md:
      - Append new entry with full details

5. Phase completed
   → Update DASHBOARD.md:
      - "Phase Breakdown" (mark phase complete)
      - Advance to next phase
   → Generate weekly-report.md if end of week
```

---

## File Size Guidelines

| File | Target Size | Update Frequency |
|------|-------------|------------------|
| DASHBOARD.md | < 200 lines | Real-time (every event) |
| daily-summary.md | < 150 lines | Real-time, archived daily |
| weekly-report.md | < 200 lines | Weekly |
| current-status.md | < 400 lines | Daily or on-demand |
| completed.md | Unlimited | Append-only log |
| blockers.md | < 100 lines | When blockers change |

---

## Example Update Log

**In DASHBOARD.md footer:**

```markdown
---

## 📝 Recent Updates

- 2026-04-14 16:45 - US-007 started (shopping cart UI)
- 2026-04-14 16:15 - US-006 completed (product detail)
- 2026-04-14 14:32 - US-005 completed (product listing)
- 2026-04-14 10:00 - Phase 2 started
```

---

## Benefits

**For Users:**
- ✅ No commands needed - just open DASHBOARD.md
- ✅ Always current - updates during work
- ✅ Quick overview in < 1 minute
- ✅ Detailed reports available if needed

**For AI:**
- ✅ Smaller focused files (< 200 lines each)
- ✅ Only read relevant files
- ✅ Clear separation of concerns

**For Stakeholders:**
- ✅ Weekly reports ready
- ✅ Real-time visibility
- ✅ No manual status meetings

---

## Migration for Existing Projects

**Add to existing projects:**

```bash
# Create progress dashboard
/init-dashboard

# Or manual:
mkdir -p .project-management/output/progress/
cp templates/dashboard-template.md output/progress/DASHBOARD.md
# Populate with current data
```

**Backward compatible:** Old progress files still work

---

## Summary

**New Progress System:**
1. **DASHBOARD.md** - Quick view, always current (< 200 lines)
2. **daily-summary.md** - Today's work (< 150 lines)
3. **weekly-report.md** - Week summary (< 200 lines)
4. **current-status.md** - Full detailed status (< 400 lines)
5. **completed.md** - Historical log (unlimited)
6. **blockers.md** - Active blockers (< 100 lines)

**Auto-updates during `/execute-work`** - no manual commands needed!

---

**Related:**
- Used by: `/execute-work`, `/update-progress`, `/migrate-to-modular`
- Backlog organization: `backlog-organization.md`

---

## ✅ Implementation Status

**This module has been successfully implemented in this project:**

**Implementation Date:** 2026-04-20

**Results:**
- ✅ Created DASHBOARD.md (218 lines) - Live auto-updating dashboard
- ✅ Created daily-summary.md (59 lines) - Today's work tracking
- ✅ Created weekly-report.md (84 lines) - Weekly summaries
- ✅ Created current-status.md (89 lines) - Detailed status breakdown
- ✅ Created completed.md (35 lines) - Historical completion log
- ✅ Created blockers.md (43 lines) - Active blocker tracking
- ✅ All files within target size limits

**Files Created:**
- `output/progress/DASHBOARD.md` (218 lines) ✅
- `output/progress/current-status.md` (89 lines) ✅
- `output/progress/completed.md` (35 lines) ✅
- `output/progress/blockers.md` (43 lines) ✅
- `output/progress/daily-summary.md` (59 lines) ✅
- `output/progress/weekly-report.md` (84 lines) ✅

**Key Features Implemented:**
- Quick status metrics table
- Today's progress tracking
- Current phase breakdown
- Active blockers display
- Velocity and timeline projections
- Quality metrics section
- Phase-by-phase breakdown

**See:** `MIGRATION-COMPLETE.md` for full report

**Last Updated:** 2026-04-20
