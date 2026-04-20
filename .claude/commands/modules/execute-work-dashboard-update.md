# Execute Work — DASHBOARD.md Auto-Update Module

**Purpose:** Auto-update DASHBOARD.md in real-time during work execution for live progress visibility

**Referenced by:** `execute-work.md` STEP 3 (Implementation Loop)

---

## When to Use This Module

**ONLY for modular backlog structure:**
- When `structure_type = "modular"` (detected in execute-work.md STEP 1A)
- When `output/progress/DASHBOARD.md` exists
- During `/execute-work` execution (phase/epic/story/bug)

**Skip for monolithic structure or if DASHBOARD.md doesn't exist:**
- Falls back to standard progress tracking (phase files only)
- User can run `/project-status` manually for reports

---

## Update Trigger Events

**Update DASHBOARD.md automatically when:**

1. ✅ **Story started** → Update "Currently Working On" section
2. ✅ **Tests run** → Update "Quality Metrics" section
3. ✅ **Story completed** → Update "Today's Progress", "Recently Completed", progress %
4. ✅ **Phase completed** → Update "Phase Breakdown" section
5. ✅ **Bug fixed** → Update "Active Blockers" (if bug was a blocker)

---

## EVENT 1: Story Started

**Trigger:** After TodoWrite creates story breakdown, before implementation starts

**What to update:**

### Section: "Currently Working On"

**Before:**
```markdown
**Currently Working On:**
- 🔄 US-005: Product listing screen (8 pts) - 40% complete
```

**After:**
```markdown
**Currently Working On:**
- 🔄 US-006: Product detail screen (5 pts) - 0% complete (just started)
```

**Update Logic:**

```javascript
// Read DASHBOARD.md
const dashboard = readFile("output/progress/DASHBOARD.md");

// Extract "Currently Working On" section
const currentlyWorkingSection = extractSection(dashboard, "## 📅 Today's Progress");

// Replace with new story
const newEntry = `- 🔄 ${storyId}: ${storyTitle} (${points} pts) - 0% complete (just started)`;

// Update DASHBOARD.md
updateSection(dashboard, "Currently Working On:", newEntry);
```

**Timestamp Footer:**
```markdown
_Auto-updated by /execute-work story US-006 at 2026-04-20 14:32:15_
```

---

## EVENT 2: Tests Run

**Trigger:** After tests complete, before marking story complete

**What to update:**

### Section: "Quality Metrics"

**Before:**
```markdown
| Test Coverage | 84% | 80% | 🟢 |
| Passing Tests | 142/145 | 145 | 🟡 |
```

**After:**
```markdown
| Test Coverage | 87% | 80% | 🟢 |
| Passing Tests | 151/151 | 151 | 🟢 |
```

**Update Logic:**

```javascript
// Run tests
const testResults = runTests();

// Calculate new coverage
const newCoverage = testResults.coverage;
const newPassing = testResults.passing;
const newTotal = testResults.total;

// Determine status icon
const coverageStatus = newCoverage >= 80 ? "🟢" : "🟡";
const testStatus = newPassing === newTotal ? "🟢" : "🔴";

// Update DASHBOARD.md
updateMetric(dashboard, "Test Coverage", `${newCoverage}%`, coverageStatus);
updateMetric(dashboard, "Passing Tests", `${newPassing}/${newTotal}`, testStatus);
```

---

## EVENT 3: Story Completed

**Trigger:** After git commit created, after progress tracking updated

**This is the MAIN update event - updates multiple sections!**

### Section 1: "Today's Progress" - Add to completed list

**Before:**
```markdown
**Stories Completed Today:** 1
- ✅ US-005: Product listing screen (5 pts) - Completed at 14:15
```

**After:**
```markdown
**Stories Completed Today:** 2
- ✅ US-005: Product listing screen (5 pts) - Completed at 14:15
- ✅ US-006: Product detail screen (3 pts) - Completed at 16:42
```

### Section 2: "Currently Working On" - Clear or update

**Before:**
```markdown
**Currently Working On:**
- 🔄 US-006: Product detail screen (3 pts) - 85% complete
```

**After (if more stories to execute):**
```markdown
**Currently Working On:**
- 🔄 US-007: Shopping cart UI (8 pts) - 0% complete (just started)
```

**After (if no more stories):**
```markdown
**Currently Working On:**
- None - All stories completed! 🎉
```

### Section 3: "Quick Status" table - Recalculate metrics

**Before:**
```markdown
| **Overall Progress** | 42% | 100% | 🟡 At Risk |
| **Current Phase** | 53% | 100% | 🟢 On Track |
| **Stories Completed** | 18/45 | 45 | 🟡 |
| **Story Points Done** | 97/279 | 279 | 🟡 |
```

**After:**
```markdown
| **Overall Progress** | 44% | 100% | 🟡 At Risk |
| **Current Phase** | 60% | 100% | 🟢 On Track |
| **Stories Completed** | 19/45 | 45 | 🟡 |
| **Story Points Done** | 100/279 | 279 | 🟡 |
```

### Section 4: "Current Phase" progress bar

**Before:**
```markdown
**Progress:** 53% (8/15 stories)
```

**After:**
```markdown
**Progress:** 60% (9/15 stories)
```

### Section 5: "Recently Completed" table

**Before:**
```markdown
| Story | Completed | Points |
|-------|-----------|--------|
| US-005 | 2026-04-20 14:15 | 5 |
| US-004 | 2026-04-19 17:20 | 5 |
| US-003 | 2026-04-19 15:10 | 3 |
```

**After (prepend to list, keep last 5):**
```markdown
| Story | Completed | Points |
|-------|-----------|--------|
| US-006 | 2026-04-20 16:42 | 3 |
| US-005 | 2026-04-20 14:15 | 5 |
| US-004 | 2026-04-19 17:20 | 5 |
| US-003 | 2026-04-19 15:10 | 3 |
| US-002 | 2026-04-19 14:05 | 2 |
```

### Section 6: "Velocity & Timeline" - Recalculate

**Before:**
```markdown
**Current Velocity:** 4.2 points/day
**Projected Completion:** 2026-06-15
```

**After:**
```markdown
**Current Velocity:** 4.5 points/day
**Projected Completion:** 2026-06-12
```

**Calculation:**

```javascript
// Read completed.md or extract from DASHBOARD
const completedPoints = 100; // Total completed
const startDate = parseDate("2026-03-01"); // From constraints.md
const today = parseDate("2026-04-20");
const daysElapsed = daysBetween(startDate, today); // 50 days

const velocity = completedPoints / daysElapsed; // 100 / 50 = 2.0 points/day

const remainingPoints = 279 - 100; // 179 points
const daysNeeded = remainingPoints / velocity; // 179 / 2.0 = 89.5 days
const projectedDate = addDays(today, daysNeeded); // 2026-04-20 + 90 = 2026-07-19
```

### Section 7: "Story Points Completed Today"

**Before:**
```markdown
**Story Points Completed Today:** 5
```

**After:**
```markdown
**Story Points Completed Today:** 8
```

---

## EVENT 4: Phase Completed

**Trigger:** When all stories in a phase are completed

**What to update:**

### Section: "Phase Breakdown"

**Before:**
```markdown
| Phase 2: Core Features | 🔄 Active | 14/15 | 91/97 | 93% |
```

**After:**
```markdown
| Phase 2: Core Features | ✅ Complete | 15/15 | 97/97 | 100% |
```

**Also update:**
- "Current Phase" header (advance to next phase)
- "Overall Progress" percentage
- "Quick Status" table

**Advance to Next Phase:**

```markdown
## 🚀 Current Phase: Phase 3 - Advanced Features

**Goal:** Secondary features and optimization
**Duration:** 2026-05-01 to 2026-06-15
**Progress:** 0% (0/10 stories)
```

---

## EVENT 5: Bug Fixed

**Trigger:** When `/execute-work bug BUG-XXX` completes

**What to update:**

### Section: "Active Blockers"

**Before:**
```markdown
| BLOCKER-001 | API rate limiting | High | US-010, US-011 | 🔴 Unresolved |
```

**After (if bug was related to blocker):**
```markdown
| BLOCKER-001 | API rate limiting | High | US-010, US-011 | 🟢 Resolved (BUG-005 fixed) |
```

Or remove from "Active Blockers" if fully resolved.

---

## Complete Update Flow

**When story US-006 completes:**

```
1. Read DASHBOARD.md
2. Extract current metrics:
   - Total stories: 45
   - Completed stories: 18
   - Total points: 279
   - Completed points: 97
   - Current phase: 2
   - Phase stories: 8/15
   - Phase points: 91/97
3. Read completed story details:
   - Story ID: US-006
   - Story title: "Product detail screen"
   - Story points: 3
   - Completion time: 16:42
4. Recalculate:
   - Completed stories: 18 + 1 = 19
   - Completed points: 97 + 3 = 100
   - Overall progress: (100 / 279) * 100 = 35.8% → 36%
   - Phase stories: 8 + 1 = 9/15
   - Phase points: 91 + 3 = 94/97
   - Phase progress: (94 / 97) * 100 = 96.9% → 97%
   - Velocity: 100 / 50 days = 2.0 points/day
5. Update DASHBOARD.md sections:
   - Quick Status table (overall 36%, phase 97%)
   - Today's Progress (add US-006)
   - Currently Working On (clear or next story)
   - Recently Completed (prepend US-006)
   - Current Phase progress (97%)
   - Velocity (2.0 points/day)
6. Write updated DASHBOARD.md
7. Add timestamp footer
```

---

## Extraction & Calculation Patterns

### Extract Total Stories

```regex
Pattern: /\*\*Stories Completed\*\* \| (\d+)\/(\d+)/
Example: "**Stories Completed** | 18/45"
Captures: [18, 45]
```

### Extract Total Points

```regex
Pattern: /\*\*Story Points Done\*\* \| (\d+)\/(\d+)/
Example: "**Story Points Done** | 97/279"
Captures: [97, 279]
```

### Extract Phase Progress

```regex
Pattern: /\*\*Progress:\*\* (\d+)% \((\d+)\/(\d+) stories\)/
Example: "**Progress:** 53% (8/15 stories)"
Captures: [53, 8, 15]
```

### Extract Today's Completed Count

```regex
Pattern: /\*\*Stories Completed Today:\*\* (\d+)/
Example: "**Stories Completed Today:** 2"
Captures: [2]
```

---

## Validation After Update

**After writing DASHBOARD.md, verify:**

1. ✅ **Metrics consistency:**
   ```
   Overall % = (Completed Points / Total Points) * 100
   Phase % = (Phase Completed / Phase Total) * 100
   ```

2. ✅ **Sum checks:**
   ```
   Phase 1 points + Phase 2 points + ... = Total Points
   ```

3. ✅ **Story counts match:**
   ```
   Today's count <= Total completed count
   ```

4. ✅ **Timestamps valid:**
   ```
   Today's date = Current date (not future, not > 1 day old)
   ```

**If validation fails:** Log warning, continue (don't block execution)

---

## File Write Strategy

**Use Edit tool for targeted updates:**

```javascript
// Example: Update overall progress
Edit({
  file_path: "output/progress/DASHBOARD.md",
  old_string: "| **Overall Progress** | 42% | 100% | 🟡 At Risk |",
  new_string: "| **Overall Progress** | 44% | 100% | 🟡 At Risk |"
});

// Example: Update today's progress
Edit({
  file_path: "output/progress/DASHBOARD.md",
  old_string: "**Stories Completed Today:** 1",
  new_string: "**Stories Completed Today:** 2"
});
```

**Alternative:** Read entire file, replace all metrics, write back (less efficient)

---

## Integration with daily-summary.md

**Also update daily-summary.md in parallel:**

```javascript
// When story completes
updateDailySummary({
  storyId: "US-006",
  storyTitle: "Product detail screen",
  points: 3,
  completedAt: "16:42",
  timeSpent: "1.5 hours",
  filesChanged: 4,
  testsAdded: 6
});
```

**See:** `live-progress-dashboard.md` for daily-summary.md structure

---

## Performance Optimization

**Cache metrics within single /execute-work run:**

```javascript
const metricsCache = {
  totalStories: 45,
  totalPoints: 279,
  completedStories: 18,
  completedPoints: 97,
  // ... other metrics
};

// Update cache as stories complete
function onStoryComplete(storyPoints) {
  metricsCache.completedStories += 1;
  metricsCache.completedPoints += storyPoints;
  // ... recalculate derived metrics
  updateDashboard(metricsCache);
}
```

**Only write DASHBOARD.md when metrics change:**
- Skip update if story started but no metrics changed yet
- Always update on story completion

---

## Error Handling

**Common errors:**

1. **DASHBOARD.md not found**
   ```
   Skip auto-update (modular structure but no dashboard created yet)
   → Suggest running /migrate-to-modular or /init-project
   ```

2. **Parse error** (can't extract metric)
   ```
   Warning: Could not parse overall progress from DASHBOARD.md
   → Skip this update, log error
   → Continue with story execution (don't block work!)
   ```

3. **Metric mismatch** (calculated != stored)
   ```
   Warning: Calculated progress (36%) != stored (42%)
   → Recalculate from scratch using backlog + progress files
   → Overwrite with correct value
   ```

4. **Write failure**
   ```
   Error: Could not write DASHBOARD.md
   → Log error, continue execution
   → User can run /update-progress manually later
   ```

---

## Status Icon Logic

**Overall Status:**
```javascript
function getOverallStatus(progress, velocity, blockers) {
  if (blockers.critical > 0) return "🔴 Off Track";
  if (progress < 50 && velocity < averageVelocity * 0.8) return "🟡 At Risk";
  return "🟢 On Track";
}
```

**Phase Status:**
```javascript
function getPhaseStatus(phaseProgress, daysRemaining) {
  if (phaseProgress === 100) return "✅ Complete";
  if (phaseProgress === 0) return "⏸️ Not Started";
  if (phaseProgress > 0 && daysRemaining < 7) return "⚠️ Rushing";
  return "🔄 In Progress";
}
```

**Metric Status:**
```javascript
function getMetricStatus(value, target) {
  if (value >= target) return "🟢";
  if (value >= target * 0.8) return "🟡";
  return "🔴";
}
```

---

## Summary

**This module handles:**
- ✅ Auto-updating DASHBOARD.md during work execution
- ✅ Real-time progress visibility without manual commands
- ✅ Metric recalculation and validation
- ✅ Integration with daily-summary.md
- ✅ Error handling and fallbacks

**Used by:**
- `/execute-work` (STEP 3: Implementation Loop)

**Updates triggered:**
- Story started → "Currently Working On"
- Tests run → "Quality Metrics"
- Story completed → Multiple sections (main update)
- Phase completed → "Phase Breakdown", advance phase
- Bug fixed → "Active Blockers"

**Related modules:**
- `live-progress-dashboard.md` - DASHBOARD.md structure definition
- `execute-work-implementation.md` - Implementation workflow
- `add-scope-readme-update.md` - Similar pattern for backlog updates

---

**Version:** 1.0.0
**Created:** 2026-04-20
**Purpose:** Autonomous real-time progress tracking
