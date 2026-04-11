# Project Status - Calculation Module

**Purpose:** Formulas and methods for calculating project metrics and status indicators.

**Parent Command:** `/project-status`

---

## Completion Percentage Calculations

### Overall Project Completion

**Formula:**
```typescript
const overallCompletion = (completedPoints / totalPoints) * 100;

// Example:
// Completed: 83 points
// Total: 200 points
// Result: (83 / 200) * 100 = 41.5%
```

**Round to 1 decimal:**
```typescript
const rounded = Math.round(overallCompletion * 10) / 10;
// 41.5%
```

---

### Phase Completion

**Formula:**
```typescript
const phaseCompletion = (phaseCompletedPoints / phaseTotalPoints) * 100;

// Example - Phase 2:
// Completed: 38 points
// Total: 65 points
// Result: (38 / 65) * 100 = 58.5%
```

---

### Story Completion Rate

**By count:**
```typescript
const storyCompletionRate = (completedStories / totalStories) * 100;

// Example:
// Completed: 18 stories
// Total: 45 stories
// Result: (18 / 45) * 100 = 40%
```

---

## Velocity Calculations

### Current Velocity

**Points per week:**
```typescript
// Points completed in current week
const currentVelocity = pointsCompletedThisWeek;

// Example: 25 points this week
```

---

### Average Velocity

**Last N weeks (default: 4):**
```typescript
const averageVelocity = totalPointsLastNWeeks / numberOfWeeks;

// Example:
// Week 1: 18 points
// Week 2: 22 points
// Week 3: 20 points
// Week 4: 25 points
// Result: (18 + 22 + 20 + 25) / 4 = 21.25 points/week
```

---

### Velocity Trend

**Compare current to average:**
```typescript
const velocityTrend = currentVelocity > averageVelocity * 1.1
  ? 'increasing' // > 10% above average
  : currentVelocity < averageVelocity * 0.9
  ? 'decreasing' // < 10% below average
  : 'stable';    // within 10%

// Example:
// Current: 25 points
// Average: 21.25 points
// Result: 25 > 21.25 * 1.1 (23.375) ’ 'increasing'
```

---

### Stories Per Week

**Average stories completed:**
```typescript
const storiesPerWeek = completedStories / weeksElapsed;

// Example:
// Completed: 18 stories
// Weeks elapsed: 9 weeks
// Result: 18 / 9 = 2 stories/week
```

---

## Timeline Calculations

### Days Elapsed

**From project start to today:**
```typescript
const projectStart = new Date('2026-01-15');
const today = new Date();
const daysElapsed = Math.floor((today - projectStart) / (1000 * 60 * 60 * 24));

// Example:
// Start: 2026-01-15
// Today: 2026-03-20
// Result: 64 days
```

---

### Days Remaining

**From today to target launch:**
```typescript
const targetLaunch = new Date('2026-06-30');
const today = new Date();
const daysRemaining = Math.floor((targetLaunch - today) / (1000 * 60 * 60 * 24));

// Example:
// Today: 2026-03-20
// Launch: 2026-06-30
// Result: 102 days
```

---

### Estimated Completion Date

**Based on current velocity:**
```typescript
const remainingPoints = totalPoints - completedPoints;
const weeksNeeded = remainingPoints / averageVelocity;
const estimatedDate = new Date();
estimatedDate.setDate(estimatedDate.getDate() + weeksNeeded * 7);

// Example:
// Remaining: 117 points
// Velocity: 21.25 points/week
// Weeks needed: 117 / 21.25 = 5.5 weeks
// Today: 2026-03-20
// Result: 2026-04-28
```

---

### Timeline Status

**Determine if on track:**
```typescript
const estimatedCompletion = new Date(/* calculated above */);
const targetLaunch = new Date('2026-06-30');
const buffer = (targetLaunch - estimatedCompletion) / (1000 * 60 * 60 * 24);

const timelineStatus = buffer > 14
  ? 'on-track'   // > 2 weeks buffer
  : buffer > 0
  ? 'at-risk'    // 0-2 weeks buffer
  : 'delayed';   // past deadline

// Example:
// Estimated: 2026-04-28
// Target: 2026-06-30
// Buffer: 63 days
// Result: 'on-track'
```

---

## Bug Metrics Calculations

### Bug Count by Severity

**Count from bug-roadmap.md:**
```typescript
const bugCounts = {
  critical: 1,  // =4 Critical section
  high: 2,      // =à High section
  medium: 3,    // =á Medium section
  low: 1,       // =â Low section
  total: 7,     // Sum of all
};
```

---

### Bug Rate

**Bugs per completed story:**
```typescript
const bugRate = totalBugs / completedStories;

// Example:
// Total bugs (open + fixed): 19 bugs
// Completed stories: 18 stories
// Result: 19 / 18 = 1.06 bugs/story
```

---

### Bug Fixing Velocity

**Bugs fixed per week:**
```typescript
const bugsFixedThisWeek = 3;     // from bug-archive.md
const bugsFixedThisMonth = 12;   // from bug-archive.md
const avgBugsPerWeek = bugsFixedThisMonth / 4;

// Example: 12 / 4 = 3 bugs/week
```

---

### Average Time to Fix

**From bug-archive.md:**
```typescript
// Extract "Time to Fix" for each bug
const fixTimes = [2, 1, 3, 2, 1]; // days
const avgTimeToFix = fixTimes.reduce((a, b) => a + b) / fixTimes.length;

// Example: (2 + 1 + 3 + 2 + 1) / 5 = 1.8 days
```

---

## Quality Metrics Calculations

### Test Coverage

**From test coverage report:**
```typescript
const coverage = {
  overall: 87,      // % from coverage report
  target: 80,       // from .claude/rules/testing.md
  status: 87 >= 80 ? 'passing' : 'failing',
};
```

---

### Code Quality Score

**Based on compliance checks:**
```typescript
const qualityChecks = {
  solidDry: true,          // SOLID & DRY followed
  testsPassing: true,      // All tests pass
  coverageTarget: true,    // Coverage > 80%
  apiCodesTested: true,    // All 6 status codes
  noLintErrors: true,      // 0 linting errors
};

const qualityScore = Object.values(qualityChecks).filter(Boolean).length;
const maxScore = Object.keys(qualityChecks).length;
const qualityPercentage = (qualityScore / maxScore) * 100;

// Example: 5/5 = 100%
```

---

## Status Indicator Calculations

### Overall Status

**Determine project health:**
```typescript
const overallStatus =
  (timelineStatus === 'on-track' &&
   blockerCount === 0 &&
   velocityTrend !== 'decreasing')
    ? '=â On Track'
  : (timelineStatus === 'at-risk' ||
     blockerCount > 0 && blockerCount <= 2 ||
     velocityTrend === 'decreasing')
    ? '=á At Risk'
    : '=4 Delayed';

// Factors:
// - Timeline status
// - Number of blockers
// - Velocity trend
// - Bug count (critical bugs)
```

---

### Phase Status

**For current phase:**
```typescript
const phaseStatus =
  (phaseCompletion >= 90 && daysToPhaseEnd > 7)
    ? '=â On Track'
  : (phaseCompletion >= 60 || daysToPhaseEnd > 14)
    ? '=á At Risk'
    : '=4 Delayed';
```

---

## Progress Bar Generation

### Calculate Progress Bar

**ASCII progress bar:**
```typescript
function generateProgressBar(percentage, width = 20) {
  const filled = Math.round((percentage / 100) * width);
  const empty = width - filled;
  return '[' + 'ˆ'.repeat(filled) + '‘'.repeat(empty) + ']';
}

// Examples:
// 41.5% ’ [ˆˆˆˆˆˆˆˆ‘‘‘‘‘‘‘‘‘‘‘‘]
// 58.5% ’ [ˆˆˆˆˆˆˆˆˆˆˆ‘‘‘‘‘‘‘‘‘]
// 100%  ’ [ˆˆˆˆˆˆˆˆˆˆˆˆˆˆˆˆˆˆˆˆ]
```

---

## Recommendations Engine

### Generate Recommendations

**Based on calculated metrics:**
```typescript
const recommendations = [];

// Coverage below threshold
if (coverage.overall < 80) {
  recommendations.push({
    type: 'critical',
    message: `Increase test coverage from ${coverage.overall}% to 80%+`,
  });
}

// Velocity decreasing
if (velocityTrend === 'decreasing') {
  recommendations.push({
    type: 'warning',
    message: 'Velocity is decreasing. Investigate blockers or resource constraints.',
  });
}

// Behind schedule
if (timelineStatus === 'delayed') {
  recommendations.push({
    type: 'critical',
    message: 'Project is behind schedule. Consider reducing scope or extending timeline.',
  });
}

// High blocker count
if (blockerCount > 3) {
  recommendations.push({
    type: 'warning',
    message: `${blockerCount} active blockers. Prioritize blocker resolution.`,
  });
}

// High bug count
if (bugCounts.critical > 0) {
  recommendations.push({
    type: 'critical',
    message: `${bugCounts.critical} critical bugs. Fix immediately before continuing.`,
  });
}
```

---

## Metric Formatting

### Format Percentages

```typescript
function formatPercentage(value) {
  return Math.round(value * 10) / 10 + '%';
}
// 41.5 ’ "41.5%"
```

---

### Format Dates

```typescript
function formatDate(date) {
  return date.toISOString().split('T')[0];
}
// 2026-03-20T00:00:00.000Z ’ "2026-03-20"
```

---

### Format Duration

```typescript
function formatDuration(days) {
  if (days < 7) return `${days} days`;
  const weeks = Math.floor(days / 7);
  const remainingDays = days % 7;
  return remainingDays > 0
    ? `${weeks} weeks, ${remainingDays} days`
    : `${weeks} weeks`;
}
// 64 days ’ "9 weeks, 1 day"
```

---

## Validation

**Validate calculated metrics:**

- [ ] Percentages are 0-100
- [ ] Dates are chronological
- [ ] Velocity is positive
- [ ] Story counts match totals
- [ ] Point calculations are accurate
- [ ] No division by zero

---

**Related:**
- Parent: `.claude/commands/project-status.md`
- Sibling: `project-status-data-collection.md`
- Rules: `.claude/rules/testing.md` (for coverage targets)
