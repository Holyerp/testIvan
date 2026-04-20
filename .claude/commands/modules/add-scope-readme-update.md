# Add Scope — README.md Update Module

**Purpose:** Update master index statistics when adding/editing stories in modular backlog structure

**Referenced by:** `add-scope.md` STEP 5 (Execute Changes)

---

## When to Use This Module

**ONLY for modular backlog structure:**
- When `structure_type = "modular"` (detected in add-scope-input-parsing.md Section 0.2)
- After adding or editing a story in any `input/backlog/phase-*.md` file
- After adding or editing an epic that changes story counts or points

**Skip for monolithic structure:**
- When `structure_type = "monolithic"`, all updates go to single `backlog.md`
- No README.md to update

---

## Update Trigger Events

**Update README.md when:**

1. ✅ **Story added** → Recalculate total stories, total points, phase breakdown
2. ✅ **Story edited** (points changed) → Recalculate total points, phase breakdown
3. ✅ **Story moved** (to different phase) → Recalculate phase breakdown
4. ✅ **Story deleted** → Recalculate all statistics
5. ✅ **Epic added** → Recalculate epic count
6. ✅ **Epic deleted** → Recalculate epic count, story counts

**Skip update when:**
- ❌ Only editing story description/title (no numeric changes)
- ❌ Only editing acceptance criteria
- ❌ Phase added/edited (README.md only tracks backlog, not execution phases)

---

## STEP 1: Calculate Updated Statistics

**After modifying a backlog file, recalculate:**

### Global Statistics

```javascript
// Read all phase backlog files
const phaseFiles = [
  "input/backlog/phase-1-foundation.md",
  "input/backlog/phase-2-core.md",
  "input/backlog/phase-3-advanced.md",
  "input/backlog/phase-4-polish.md",
  "input/backlog/future.md"
];

let totalStories = 0;
let totalPoints = 0;
let totalEpics = 0;

// Count stories, points, epics per phase
const phaseStats = {};

for (const phaseFile of phaseFiles) {
  const content = readFile(phaseFile);

  // Count epics (## Epic sections)
  const epicCount = countMatches(content, /^## Epic \d+:/gm);

  // Count stories (- **US-XXX**: pattern)
  const storyCount = countMatches(content, /- \*\*US-\d{3}\*\*:/g);

  // Sum story points (- **Story Points:** N pattern)
  const points = sumMatches(content, /- \*\*Story Points:\*\* (\d+)/g);

  phaseStats[phaseFile] = {
    epics: epicCount,
    stories: storyCount,
    points: points
  };

  totalEpics += epicCount;
  totalStories += storyCount;
  totalPoints += points;
}
```

### Priority Breakdown (Optional)

```javascript
// Count by priority (P0/P1/P2/P3)
const priorityStats = {
  P0: { count: 0, points: 0 },
  P1: { count: 0, points: 0 },
  P2: { count: 0, points: 0 },
  P3: { count: 0, points: 0 }
};

for (const phaseFile of phaseFiles) {
  const content = readFile(phaseFile);

  // Extract all stories with priority
  const stories = extractStories(content); // Returns [{id, priority, points}, ...]

  for (const story of stories) {
    if (story.priority in priorityStats) {
      priorityStats[story.priority].count += 1;
      priorityStats[story.priority].points += story.points;
    }
  }
}
```

---

## STEP 2: Update README.md Sections

**File:** `.project-management/input/backlog/README.md`

### Section: Summary Statistics

**Find and replace:**

```markdown
## 📊 Summary Statistics

**Total Epics:** {{EPIC_COUNT}}
**Total Stories:** {{STORY_COUNT}}
**Total Story Points:** {{TOTAL_POINTS}}
```

**Replace with calculated values:**

```markdown
## 📊 Summary Statistics

**Total Epics:** 18
**Total Stories:** 45
**Total Story Points:** 279
```

### Section: By Phase

**Find and replace:**

```markdown
**By Phase:**
- Phase 1 (Foundation): {{P1_STORIES}} stories, {{P1_POINTS}} points
- Phase 2 (Core Features): {{P2_STORIES}} stories, {{P2_POINTS}} points
- Phase 3 (Advanced): {{P3_STORIES}} stories, {{P3_POINTS}} points
- Phase 4 (Polish): {{P4_STORIES}} stories, {{P4_POINTS}} points
- Future (Post-launch): {{FUTURE_STORIES}} stories
```

**Replace with:**

```markdown
**By Phase:**
- Phase 1 (Foundation): 8 stories, 34 points
- Phase 2 (Core Features): 15 stories, 97 points
- Phase 3 (Advanced): 10 stories, 68 points
- Phase 4 (Polish): 8 stories, 55 points
- Future (Post-launch): 4 stories, 25 points
```

### Section: By Priority (Optional)

**Find and replace:**

```markdown
**By Priority:**
- P0 (Must Have): {{P0_COUNT}} stories, {{P0_POINTS}} points
- P1 (Should Have): {{P1_COUNT}} stories, {{P1_POINTS}} points
- P2 (Nice to Have): {{P2_COUNT}} stories, {{P2_POINTS}} points
```

**Replace with:**

```markdown
**By Priority:**
- P0 (Must Have): 12 stories, 89 points
- P1 (Should Have): 18 stories, 112 points
- P2 (Nice to Have): 15 stories, 78 points
```

### Section: Phase Backlogs (Update counts)

**Find and replace each phase:**

```markdown
### [Phase 1: Foundation & Setup](phase-1-foundation.md)
**Goal:** Project infrastructure, authentication, basic setup
**Stories:** {{P1_STORIES}} | **Points:** {{P1_POINTS}}
**Status:** {{P1_STATUS}} ({{P1_COMPLETE}}/{{P1_TOTAL}} completed)
```

**Replace with:**

```markdown
### [Phase 1: Foundation & Setup](phase-1-foundation.md)
**Goal:** Project infrastructure, authentication, basic setup
**Stories:** 8 | **Points:** 34
**Status:** In Progress (3/8 completed)
```

**Note:** Status comes from `output/progress/DASHBOARD.md` or progress tracking files

---

## STEP 3: Update Last Modified Timestamp

**At top of README.md:**

```markdown
**Last Updated:** {{DATE}}
```

**Replace with current date:**

```markdown
**Last Updated:** 2026-04-20
```

---

## STEP 4: Write Updated README.md

**Use Edit tool to replace sections:**

```javascript
// Preserve README.md structure, only update numbers
Edit(
  file_path: "input/backlog/README.md",
  old_string: "**Total Stories:** 44",
  new_string: "**Total Stories:** 45"
);

Edit(
  file_path: "input/backlog/README.md",
  old_string: "**Total Story Points:** 274",
  new_string: "**Total Story Points:** 279"
);

// ... repeat for all statistics
```

**Alternative:** Read entire README.md, replace all {{placeholders}}, write back

---

## Extraction Patterns

### Count Epics

```regex
Pattern: /^## Epic \d+:/gm
Example matches:
- "## Epic 1: User Authentication"
- "## Epic 2: Product Catalog"
```

### Count Stories

```regex
Pattern: /- \*\*US-\d{3}\*\*:/g
Example matches:
- "- **US-001**: User registration"
- "- **US-015**: Product listing"
```

### Extract Story Points

```regex
Pattern: /- \*\*Story Points:\*\* (\d+)/g
Capture group 1: point value
Example matches:
- "- **Story Points:** 5" → 5
- "- **Story Points:** 13" → 13
```

### Extract Priority

```regex
Pattern: /- \*\*Priority:\*\* (P[0-3])/g
Capture group 1: priority level
Example matches:
- "- **Priority:** P0" → P0
- "- **Priority:** P1" → P1
```

---

## Status Calculation (Optional)

**For "Status" field in Phase Backlogs section:**

Read from progress tracking:

```javascript
// Read DASHBOARD.md or phase-N-progress.md
const phaseProgress = readDashboard();

// Extract completion for each phase
const phase1Status = phaseProgress.phases[1];
// Returns: { completed: 3, total: 8, percentage: 37.5 }

// Format status string
const statusText = `${phase1Status.status} (${phase1Status.completed}/${phase1Status.total} completed)`;
// Result: "In Progress (3/8 completed)"
```

**Status values:**
- ✅ Complete - All stories completed (100%)
- 🔄 In Progress - Some stories completed (1-99%)
- ⏸️ Not Started - No stories completed (0%)
- ⚠️ Blocked - Has active blockers

---

## Validation After Update

**After writing README.md, verify:**

1. ✅ **Sum check:** Phase stories = Total stories
   ```
   Phase 1 (8) + Phase 2 (15) + Phase 3 (10) + Phase 4 (8) + Future (4) = 45
   ```

2. ✅ **Sum check:** Phase points = Total points
   ```
   34 + 97 + 68 + 55 + 25 = 279
   ```

3. ✅ **Priority sum:** P0 + P1 + P2 + P3 = Total stories
   ```
   12 + 18 + 15 + 0 = 45
   ```

4. ✅ **File exists:** All referenced phase files exist
   ```
   phase-1-foundation.md ✓
   phase-2-core.md ✓
   phase-3-advanced.md ✓
   phase-4-polish.md ✓
   future.md ✓
   ```

**If validation fails:** Log error, show warning to user

---

## Example: Story Added to Phase 2

**User action:** `/add-scope add story 2 3`

**Changes:**
1. Story US-046 added to `input/backlog/phase-2-core.md` (8 points)
2. Phase 2 now has 16 stories (was 15), 105 points (was 97)
3. Total stories now 46 (was 45), 287 points (was 279)

**README.md updates:**

```diff
 ## 📊 Summary Statistics

 **Total Epics:** 18
-**Total Stories:** 45
-**Total Story Points:** 279
+**Total Stories:** 46
+**Total Story Points:** 287

 **By Phase:**
 - Phase 1 (Foundation): 8 stories, 34 points
-- Phase 2 (Core Features): 15 stories, 97 points
+- Phase 2 (Core Features): 16 stories, 105 points
 - Phase 3 (Advanced): 10 stories, 68 points
```

**Last Updated updated:**

```diff
-**Last Updated:** 2026-04-19
+**Last Updated:** 2026-04-20
```

---

## Integration with DASHBOARD.md

**After updating README.md, also update DASHBOARD.md:**

```javascript
// Update overall metrics
const dashboardMetrics = {
  totalStories: 46,
  totalPoints: 287,
  completedStories: extractFromProgress("completed_count"),
  completedPoints: extractFromProgress("completed_points"),
  // ... recalculate overall progress %
};

updateDashboard(dashboardMetrics);
```

**See:** `live-progress-dashboard.md` for DASHBOARD.md update logic

---

## Performance Optimization

**Cache file reads within single /add-scope run:**

```javascript
const fileCache = {};

function readPhaseBacklog(phaseFile) {
  if (!(phaseFile in fileCache)) {
    fileCache[phaseFile] = readFile(phaseFile);
  }
  return fileCache[phaseFile];
}
```

**Only read files that changed:**

```
Story added to Phase 2:
- Read phase-2-core.md (changed)
- Read README.md (needs update)
- Skip phase-1, phase-3, phase-4, future (unchanged)
```

---

## Error Handling

**Common errors:**

1. **README.md not found**
   ```
   Error: backlog/README.md not found
   → This project uses monolithic backlog (backlog.md exists)
   → README.md update skipped
   ```

2. **Phase file not found**
   ```
   Error: phase-2-core.md referenced in README.md but file not found
   → Run /migrate-to-modular to fix structure
   ```

3. **Invalid sum** (phase counts don't match total)
   ```
   Warning: Phase story counts (44) don't match total (45)
   → Recalculating from scratch...
   ```

4. **Parse error** (can't extract story count)
   ```
   Error: Could not parse story count from phase-2-core.md
   → Manual review needed
   ```

---

## Summary

**This module handles:**
- ✅ Recalculating statistics after backlog changes
- ✅ Updating README.md sections with new counts
- ✅ Validating sums and integrity
- ✅ Updating last modified timestamp
- ✅ Integration with DASHBOARD.md updates

**Used by:**
- `/add-scope` (STEP 5: Execute Changes)

**Related modules:**
- `add-scope-input-parsing.md` - Determines target backlog file
- `live-progress-dashboard.md` - DASHBOARD.md update logic

---

**Version:** 1.0.0
**Created:** 2026-04-20
**Purpose:** Modular backlog README.md maintenance
