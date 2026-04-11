# Project Status - Data Collection Module

**Purpose:** Strategies for collecting project data from all relevant files.

**Parent Command:** `/project-status`

---

## File Reading Strategy

### Core Project Files

**MUST read these files:**

1. **`.project-management/input/scope.md`**
   - Project name
   - Project vision
   - Core objectives
   - Target launch date

2. **`.project-management/output/phases/phase-*.md`**
   - Read ALL phase files (phase-1.md, phase-2.md, etc.)
   - Phase status (Planning, In Progress, Completed, On Hold)
   - Story progress per phase
   - Story points completed/total per phase
   - Phase start and end dates

3. **`.project-management/output/progress/current-status.md`**
   - Current phase
   - Current stories being worked on
   - Recent velocity
   - Weekly progress

4. **`.project-management/output/progress/completed.md`**
   - All completed stories with dates
   - Completion velocity history
   - Recent achievements

5. **`.project-management/output/progress/blockers.md`**
   - Active blockers
   - Blocker severity
   - Impact assessment
   - Resolution timeline

6. **`.project-management/input/backlog.md`**
   - Total stories
   - Remaining work
   - Future phases

7. **`.project-management/output/bugs/bug-roadmap.md`** (if exists)
   - Open bugs by severity (Critical, High, Medium, Low)
   - Bug descriptions and affected components
   - Bug creation dates

8. **`.project-management/output/bugs/bug-archive.md`** (if exists)
   - Fixed bugs with resolution dates
   - Bug fixing velocity
   - Recent bug fixes

---

## Data Extraction from Phase Files

### Parse Phase Files

**For each phase-N.md file:**

```markdown
# Phase 1: Foundation

**Duration:** 2026-01-15 to 2026-02-28 (6 weeks)
**Status:** Completed
**Story Points:** 45/45 (100%)

### Epic EPIC-1: Authentication (15 points)
**Status:** Completed

#### US-001: User Registration
- **Story Points:** 5
- **Status:** Completed
- **Completed:** 2026-01-18

#### US-002: User Login
- **Story Points:** 5
- **Status:** Completed
- **Completed:** 2026-01-22
```

**Extract:**
- Phase number and name
- Phase status
- Phase duration
- Story points (completed/total)
- Epic count
- Story count per epic
- Completion dates
- Current active stories

---

## Data Extraction from Progress Files

### current-status.md

**Extract:**
```markdown
# Current Status

**Phase:** Phase 2 - Core Features
**Week:** 2026-03-15 to 2026-03-22
**Velocity:** 25 points/week

**In Progress:**
- US-045: User Profile (5 points) - 60% complete
- US-046: Payment Integration (8 points) - 30% complete

**Completed This Week:**
- US-043: Product Search (5 points)
- US-044: Shopping Cart (8 points)
```

**Data to collect:**
- Current phase
- Current week
- Velocity (points/week)
- In-progress stories
- Weekly completions

---

### completed.md

**Extract:**
```markdown
# Completed Work

## Phase 1: Foundation (45 points)
Completed: 2026-02-28

- US-001: User Registration (5 points) - 2026-01-18
- US-002: User Login (5 points) - 2026-01-22
- US-003: Password Reset (3 points) - 2026-01-25
...

## Phase 2: Core Features (In Progress)

- US-043: Product Search (5 points) - 2026-03-15
- US-044: Shopping Cart (8 points) - 2026-03-18
```

**Data to collect:**
- Total completed points
- Completion dates
- Stories completed per week
- Completion velocity trend

---

### blockers.md

**Extract:**
```markdown
# Current Blockers

## High Impact

### BLOCKER-1: Payment Gateway API Access
- **Story:** US-046
- **Impact:** High
- **Blocked Since:** 2026-03-10
- **Description:** Waiting for production API credentials
- **Resolution:** Expected 2026-03-20

### BLOCKER-2: Database Performance
- **Story:** US-048, US-049
- **Impact:** Medium
- **Blocked Since:** 2026-03-12
- **Description:** Query optimization needed
- **Resolution:** In progress
```

**Data to collect:**
- Blocker count
- Impact levels
- Affected stories
- Duration blocked
- Resolution status

---

## Data Extraction from Bug Files

### bug-roadmap.md

**Extract:**
```markdown
# Bug Roadmap

## =4 Critical (Fix Immediately)

### BUG-001: Payment Processing Failure
- **Severity:** Critical
- **Affected:** Checkout flow
- **Created:** 2026-03-15
- **Status:** New

## =à High (Fix This Week)

### BUG-002: User Session Timeout
- **Severity:** High
- **Affected:** Authentication
- **Created:** 2026-03-14
- **Status:** In Progress
```

**Data to collect:**
- Bug count by severity (Critical, High, Medium, Low)
- Bug statuses (New, In Progress, Fixed)
- Affected components
- Bug age (days since creation)

---

### bug-archive.md

**Extract:**
```markdown
# Bug Archive

## Fixed Bugs (Last 30 Days)

### BUG-003: Profile Image Upload
- **Fixed:** 2026-03-18
- **Time to Fix:** 2 days
- **Severity:** Medium

### BUG-004: Search Results Pagination
- **Fixed:** 2026-03-16
- **Time to Fix:** 1 day
- **Severity:** Low
```

**Data to collect:**
- Bugs fixed this week
- Bugs fixed this month
- Average time to fix
- Bug velocity

---

## Metric Aggregation

### Aggregate Phase Data

**Calculate for all phases:**
```typescript
const phaseData = {
  totalPhases: 4,
  completedPhases: 1,
  currentPhase: 2,
  phases: [
    { number: 1, name: 'Foundation', status: 'Completed', points: 45, completed: 45 },
    { number: 2, name: 'Core', status: 'In Progress', points: 65, completed: 38 },
    { number: 3, name: 'Advanced', status: 'Planning', points: 50, completed: 0 },
    { number: 4, name: 'Polish', status: 'Planning', points: 40, completed: 0 },
  ],
  totalPoints: 200,
  completedPoints: 83,
};
```

---

### Aggregate Story Data

**Calculate story metrics:**
```typescript
const storyData = {
  totalStories: 45,
  completedStories: 18,
  inProgressStories: 3,
  todoStories: 24,
  blockedStories: 2,
};
```

---

### Aggregate Bug Data

**Calculate bug metrics:**
```typescript
const bugData = {
  openBugs: {
    critical: 1,
    high: 2,
    medium: 3,
    low: 1,
    total: 7,
  },
  fixedThisWeek: 3,
  fixedThisMonth: 12,
  avgTimeToFix: 2.5, // days
  bugRate: 0.15, // bugs per story
};
```

---

## Timeline Calculation

### Extract Timeline Data

**From scope.md and phase files:**
```typescript
const timeline = {
  projectStart: '2026-01-15',
  targetLaunch: '2026-06-30',
  currentDate: '2026-03-20',
  daysElapsed: 64,
  daysRemaining: 102,
  totalDuration: 166,
  onSchedule: true, // calculated
};
```

---

## Velocity Calculation

### Calculate Velocity

**From completed.md:**
```typescript
// Stories completed per week (last 4 weeks)
const weeklyCompletions = [
  { week: '2026-02-19', points: 18 },
  { week: '2026-02-26', points: 22 },
  { week: '2026-03-04', points: 20 },
  { week: '2026-03-11', points: 25 },
];

const velocity = {
  current: 25, // this week
  average: 21.25, // avg of last 4 weeks
  trend: 'increasing', // comparing to average
};
```

---

## Test Coverage Data

### Extract from Test Results

**If test coverage report exists:**
```typescript
const coverage = {
  overall: 87,
  statements: 87.5,
  branches: 82.3,
  functions: 91.2,
  lines: 87.5,
  target: 80,
  status: 'passing', // above target
};
```

---

## Data Validation

**Before using collected data:**

- [ ] All phase files read successfully
- [ ] Progress files parsed correctly
- [ ] Bug files read (if exist)
- [ ] Dates are valid and chronological
- [ ] Story points are numbers
- [ ] No missing required fields
- [ ] Calculations are accurate

---

## Error Handling

**If files are missing:**
```
   Warning: .project-management/output/progress/blockers.md not found
   ’ Assuming no blockers
```

**If files are malformed:**
```
   Warning: Could not parse phase-2.md
   ’ Skipping Phase 2 data
```

**If data is inconsistent:**
```
   Warning: Total points in phase-1.md (45) doesn't match sum of story points (42)
   ’ Using phase-level data
```

---

**Related:**
- Parent: `.claude/commands/project-status.md`
- Sibling: `project-status-calculation.md`
- Data sources: `.project-management/output/` and `.project-management/input/`
