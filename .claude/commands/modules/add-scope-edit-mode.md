# Add Scope — Edit Mode Module

**Referenced by:** `add-scope.md` STEP 3 (edit) & STEP 5 (edit)

---

## Overview

Edit mode modifies existing phases, epics, or stories. Changes cascade through metrics and cross-references automatically. No new items are created — only existing content is updated.

---

## STEP 3 (Edit): LOAD & DISPLAY CURRENT CONTENT

### 3.1 Edit Phase

1. Read `phase-{N}.md`
2. Display summary:
```
EDITING: Phase {N}: {Name}

Current State:
- Status: {Status}
- Duration: {Start} to {End}
- Epics: {count}
- Stories: {count}
- Total Story Points: {points}
- Goal: {goal}

Epics in this phase:
  Epic 1: {Name} ({N} points, {status})
  Epic 2: {Name} ({N} points, {status})
```

3. If `--from` file → parse and apply changes
4. If no `--from` → ask:
```
What to change?
[1] Phase name/goal
[2] Duration/dates
[3] Status
[4] Add epic → redirects to /add-scope add epic [N]
[5] Remove an epic
[6] Describe changes freely
```

### 3.2 Edit Epic

1. Read target phase file, extract specific epic section
2. Display:
```
EDITING: Epic {N}: {Name} (Phase {M})

Current State:
- Priority: {P0/P1/P2}
- Status: {Status}
- Total Story Points: {points}
- Dependencies: {deps}

Stories:
  US-{XXX}: {Title} ({N} points, {status})
  US-{XXX}: {Title} ({N} points, {status})
```

3. If `--from` file → parse and apply
4. If no `--from` → ask:
```
What to change?
[1] Epic name/description
[2] Priority
[3] Status
[4] Dependencies
[5] Add story → redirects to /add-scope add story [M] [N]
[6] Remove a story
[7] Describe changes freely
```

### 3.3 Edit Story

1. **Search ALL phase files** for US-XXX
   - Not found → error with list of valid story IDs
   - Found → note which phase and epic it belongs to
2. Display:
```
EDITING: US-{XXX}: {Title}

Location: Phase {M} → Epic {N}: {Epic Name}

- Story Points: {points}
- Priority: {P0/P1/P2}
- Status: {status}
- Dependencies: {deps}

User Story:
  {As a [role], I want to [action] so that [benefit]}

Acceptance Criteria:
  - [ ] {Criterion 1}
  - [ ] {Criterion 2}

Technical Notes:
  {notes}
```

3. If `--from` file → parse and apply
4. If no `--from` → ask:
```
What to change?
[1] Story title/description
[2] Acceptance criteria
[3] Story points
[4] Priority
[5] Dependencies
[6] Status
[7] Technical notes
[8] Describe changes freely
```

---

## STEP 5 (Edit): APPLY CHANGES

### 5.1 Phase Edits

| Change | Actions |
|--------|---------|
| Name/Goal | Update heading in phase file. Update `current-status.md` if it lists names. |
| Status | Update `**Status:**` field. Update `current-status.md`. |
| Duration/Dates | Update `**Duration:**`, `**Started:**`, `**Target Completion:**` fields. |
| Remove epic | See Section 5.4 (Removing Items). |

### 5.2 Epic Edits

| Change | Actions |
|--------|---------|
| Name/Description | Update heading in phase file + entry in `backlog.md`. |
| Priority/Status | Update fields in phase file. |
| Dependencies | Update in phase file. Verify new deps reference valid items. |
| Remove story | See Section 5.4 (Removing Items). |

### 5.3 Story Edits

| Change | Actions | Cascading? |
|--------|---------|------------|
| Title/Description | Update in phase file + `backlog.md` | No |
| Acceptance criteria | Replace section in phase file + `backlog.md` | No |
| Priority | Update in phase file + `backlog.md` | No |
| Dependencies | Update in phase file + `backlog.md`. Verify valid US-XXX refs. | No |
| Status | Update in phase file. If → "Completed": update progress metrics. | Yes → progress |
| Technical notes | Update in phase file | No |
| **Story points** | **See Section 5.3.1 — cascading update** | **Yes → epic → phase** |

#### 5.3.1 Story Points Change (Cascading)

This is the most complex edit — changes propagate upward:

```
1. Update story: **Story Points:** {old} → {new}
2. Calculate: diff = new - old
3. Update epic total:
   Find: ### Epic {N}: {Name} ({old_epic_total} story points)
   Replace: ### Epic {N}: {Name} ({old_epic_total + diff} story points)
4. Update phase metrics:
   Total Story Points: {old_phase_total + diff}
5. Update backlog:
   **Estimate:** {new} story points
```

**Example:** US-005 changes from 5 → 8 points (diff = +3):
```
Story US-005: 5 → 8 points
Epic "Product Management": 21 → 24 points
Phase 2 total: 45 → 48 points
Backlog US-005 estimate: 5 → 8 points
```

---

### 5.4 Removing Items

**Removing an Epic from a Phase:**

1. **Check for dependencies** — scan all stories in ALL phases for deps on stories being removed
2. If dependencies found → warn BEFORE proceeding:
```
WARNING: Removing this epic will break dependencies:
  US-015 depends on US-010 (being removed)
  US-018 depends on US-014 (being removed)

Proceed anyway? [Yes / No]
```
3. If user confirms:
   - Remove epic section from phase file
   - Renumber subsequent epics: Epic 3 → Epic 2 (if Epic 2 removed)
   - Remove epic's stories from `backlog.md`
   - Update phase metrics (subtract points and story count)

**Removing a Story from an Epic:**

1. Check for dependencies (same as above)
2. If user confirms:
   - Remove story section from phase file
   - Remove from `backlog.md`
   - Update epic total story points (subtract)
   - Update phase metrics (subtract)

---

## Edit with --from File

When `--from` is provided, parse the file to determine changes and show a diff preview:

```
EDIT PREVIEW: US-{XXX}: {Title}

CHANGES:
  Story Points: 5 → 8 (+3)
  Priority: P1 → P0
  Acceptance Criteria:
    + [ ] New criterion added
    - [ ] Old criterion removed

CASCADE EFFECTS:
  Epic "Product Management": 21 → 24 points
  Phase 2 total: 45 → 48 points

FILES TO MODIFY:
  - .project-management/output/phases/phase-2.md
  - .project-management/input/backlog.md

Apply changes? [Yes / No / Revise]
```

---

## Error Handling

| Error | Recovery |
|-------|----------|
| Item not found | Show list of valid options with IDs and locations |
| Broken dependencies after removal | Warn user with specific dep list, require confirmation |
| Story points change fails cascade | Recalculate all totals from scratch (sum individual stories) |
| File changed during edit | Re-read file, reapply changes, warn user |
| --from file format unrecognized | Show expected format, ask user to clarify which fields to update |

### Item Not Found
```
[Phase N / Epic N in Phase M / US-XXX] not found.

Available [phases / epics / stories]:
  [numbered list of valid options with names and IDs]
```

### Concurrent Modification
If a file was modified between read and write:
```
File changed: {filename}
Re-reading current state and reapplying changes...
[Reapply and show updated preview]
```

---

**Version:** 3.0.0
**Created:** 2026-04-01
