# Project Status - Data Collection Module

**Purpose:** Methods for collecting project data for status reporting.

**Parent:** `.claude/commands/project-status.md`

---

## Data Sources

### Phase Files
- **Location:** `.project-management/output/phases/phase-*.md`
- **Read:** All phase files (1, 2, 3, 4)
- **Extract:**
  - Phase metadata (title, goal, duration)
  - Stories (US-XXX) with status (Todo/In Progress/Completed)
  - Story points
  - Dependencies
  - Epics

### Backlog
- **Location:** `.project-management/input/backlog.md`
- **Extract:**
  - All user stories with IDs
  - Story points
  - Priorities (P0/P1/P2/P3)
  - Epic groupings

### Progress Files
- **Location:** `.project-management/output/progress/`
- **Files:**
  - `phase-1-progress.md` - Phase 1 tracking
  - `phase-2-progress.md` - Phase 2 tracking (etc.)
  - `completed.md` - Completed work log
  - `blockers.md` - Active blockers

- **Extract:**
  - Completion dates
  - Time spent per story
  - Blocker details (severity, status, resolution)

### Bug Files
- **Location:** `.project-management/output/bugs/`
- **Files:**
  - `bug-roadmap.md` - Active bugs
  - `bug-archive.md` - Fixed bugs

- **Extract:**
  - Bug IDs (BUG-XXX)
  - Severity (Critical/High/Medium/Low)
  - Status (New/In Progress/Fixed/Verified)
  - Created/Fixed dates

### Constraints
- **Location:** `.project-management/input/constraints.md`
- **Extract:**
  - Project start date
  - Target launch date
  - Team size
  - Budget

---

## Data Aggregation

### Story Statistics
```javascript
{
  total: count of all stories,
  completed: count where status="Completed",
  in_progress: count where status="In Progress",
  todo: count where status="Todo",
  blocked: count where blocked=true,
  by_priority: {
    P0: { total, completed },
    P1: { total, completed },
    P2: { total, completed }
  },
  by_phase: {
    1: { total, completed, in_progress },
    2: { total, completed, in_progress },
    ...
  }
}
```

### Story Points Summary
```javascript
{
  total: sum of all story points,
  completed: sum where status="Completed",
  remaining: total - completed,
  velocity: completed / days_elapsed
}
```

### Bug Statistics
```javascript
{
  active: {
    critical: count,
    high: count,
    medium: count,
    low: count
  },
  fixed: {
    total: count,
    by_week: array of weekly counts
  },
  avg_fix_time: average days to fix
}
```

### Timeline Data
```javascript
{
  start_date: from constraints.md,
  target_date: from constraints.md,
  days_elapsed: today - start_date,
  days_remaining: target_date - today,
  estimated_completion: calculated from velocity
}
```

---

## Parsing Methods

### Extract Story Status from Phase File
```
Pattern: ### US-XXX: [Title]
Next line: **Status:** Todo/In Progress/Completed
Extract: US-XXX, status
```

### Extract Bug Severity from Bug Roadmap
```
Sections: ## Critical, ## High, ## Medium, ## Low
For each section: extract BUG-XXX IDs
Map: BUG-XXX → severity
```

### Extract Completion Dates from Progress Files
```
Pattern: - [x] US-XXX completed (YYYY-MM-DD)
Extract: US-XXX → completion_date
```

### Extract Blockers
```
From blockers.md:
Pattern: ## [BLOCKER-XXX] Title
Extract: blocker_id, severity, status, affected_stories
```

---

## Validation

**Ensure data integrity:**
- All story IDs in phase files exist in backlog
- Story points are Fibonacci numbers
- Dates are valid (YYYY-MM-DD format)
- Statuses are valid (Todo/In Progress/Completed)
- Bug severities are valid (Critical/High/Medium/Low)

**If validation fails:** Report errors, skip invalid entries.

---

## Caching Strategy

**For performance:**
- Cache file reads (memoize within single /project-status run)
- Don't cache across runs (data changes frequently)
- Parse each file only once per run

---

[← Back to project-status.md](../project-status.md)
