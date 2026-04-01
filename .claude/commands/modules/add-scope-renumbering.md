# Add Scope — Renumbering Module

**Referenced by:** `add-scope.md` STEP 5 & STEP 6

---

## STEP 5: EXECUTE CHANGES — Renumbering & Cross-References

This module handles all file mutations: renaming, content updates, insertions, and metric recalculations.

---

## 5.1 Phase Renumbering (add phase at position N)

### 5.1.1 Rename Phase Files

**CRITICAL: Rename from HIGHEST number downward to prevent collisions.**

```
Given: insert_position = N, total_phases = T

For i = T down to N:
  Rename: phase-{i}.md → phase-{i+1}.md
```

**Example — insert at position 2 with 4 existing phases:**
```
phase-4.md → phase-5.md   ← rename highest first
phase-3.md → phase-4.md
phase-2.md → phase-3.md
→ Position 2 is now free for the new phase
```

### 5.1.2 Update Content in Each Renamed File

**For each renamed file (old_N → new_N), find and replace:**

| Find | Replace | Notes |
|------|---------|-------|
| `# Phase {old_N}:` | `# Phase {new_N}:` | Title heading |
| `Phase {old_N}` (in prose) | `Phase {new_N}` | Whole-word match only, not inside filenames |
| `Phase {{NEXT_PHASE}}` refs | Update to `{new_N + 1}` | Carry Over section |
| `Phase {old_N} -` | `Phase {new_N} -` | Final Metrics heading |

### 5.1.3 Update Cross-References in ALL Phase Files

**Scan every phase file (including non-renamed ones):**
- Any `Phase {X}` reference where X >= insert_position → increment by 1
- Example: Phase 1 says "blocks Phase 2" and Phase 2 shifted to 3 → update to "blocks Phase 3"

**Exception:** References to phases BEFORE insert_position stay unchanged.

### 5.1.4 Create New Phase File

1. Read template: `.project-management/templates/phase-template.md`
2. Replace all `{{PLACEHOLDERS}}` with content from user input
3. Assign story IDs: `US-{max_story_id + 1}`, `US-{max_story_id + 2}`, ...
4. Use phase-local epic numbering: Epic 1, Epic 2, ...
5. Set `**Status:** Planning`
6. Write to: `.project-management/output/phases/phase-{N}.md`

### 5.1.5 Update Backlog

1. Read `.project-management/input/backlog.md`
2. Add new epics with next global numbers: `## Epic {max_epic_number + 1}: {Name}`
3. Add stories under each epic with assigned US-XXX IDs
4. Write updated backlog

### 5.1.6 Update Progress

1. Read `.project-management/output/progress/current-status.md`
2. Update: total phases count, total stories count, total story points
3. Add new phase to phase list (if tracked individually)
4. Update any phase number references that shifted
5. Write updated progress

---

## 5.2 Epic Renumbering (add epic to phase at position P)

### 5.2.1 Insert in Phase File

1. Read target: `phase-{target_phase}.md`
2. Find insertion point using `### Epic {N}:` headers as boundaries
3. Insert new epic markdown block at position P

### 5.2.2 Renumber Subsequent Epics

**Within the phase file only (epic numbering is LOCAL to each phase):**

```
For each epic at position >= P:
  Find: ### Epic {N}:
  Replace: ### Epic {N+1}:
```

### 5.2.3 Update Phase Metrics

In the same phase file:
- `Total Story Points: {old + new_epic_points}`
- `Total Stories: {old + new_story_count}`
- `Total Epics: {old + 1}`

### 5.2.4 Update Backlog

1. Add epic with next global number: `## Epic {max_epic_number + 1}: {Name}`
2. Add all stories with assigned US-XXX IDs under it

---

## 5.3 Story Addition (add story to epic)

### 5.3.1 Insert in Phase File

1. Read target phase file
2. Find epic section: `### Epic {target_epic}:`
3. Find last `#### US-{XXX}:` within that epic
4. Insert new story after it (before next epic or section boundary)

### 5.3.2 Update Epic Total

```
Find: ### Epic {N}: {Name} ({old_points} story points)
Replace: ### Epic {N}: {Name} ({old_points + new_points} story points)
```

### 5.3.3 Update Phase Metrics

- `Total Story Points: {old + new_points}`
- `Total Stories: {old + 1}`

### 5.3.4 Update Backlog

Find matching epic in `backlog.md`, append story:
```markdown
### US-{XXX}: {Title}
**Priority:** {Priority}
**Story:** {User story format}

**Acceptance Criteria:**
- [ ] {Criterion 1}
- [ ] {Criterion 2}

**Estimate:** {N} story points
**Dependencies:** {Dependencies or "None"}
```

---

## 5.4 Append (No Renumbering Path)

When position = total + 1 (appending at end):

| Scope | Actions | Renumbering |
|-------|---------|-------------|
| Phase | Create new `phase-{T+1}.md` + update backlog + update progress | None |
| Epic | Append epic section at end of phase | None |
| Story | Append story at end of epic | None |

This is the simplest path — only new content creation and metric updates.

---

## 5.5 Cross-Reference Rules

### References That MUST Be Updated

| Location | What Changes | Trigger |
|----------|-------------|---------|
| Phase file titles | `# Phase N:` heading | Phase insertion |
| Phase file prose | `Phase N` mentions | Phase insertion |
| Carry Over sections | "Next Phase" refs | Phase insertion |
| Progress current-status | Phase numbers, metrics | Any add |
| Backlog epic numbers | `## Epic N:` | Epic addition |
| Phase metric totals | Story points, story count | Epic/Story add |
| Epic heading totals | `({N} story points)` | Story add |

### References That NEVER Change

| Reference | Reason |
|-----------|--------|
| US-XXX story IDs | Immutable identifiers |
| Story dependency refs | US-XXX based (stable) |
| Git commit references | Historical records |
| Progress log entries | Historical — add "(formerly Phase N)" note if needed |

---

## 5.6 Integrity Verification & Recovery

**Run after ALL mutations complete.**

### Check 1: Phase File Continuity

**Verify:** `phase-1.md` through `phase-N.md` exist with no gaps.

**If FAILS (gap detected):**
```
Phase continuity FAILED: gap between phase-[X] and phase-[Y]

Recovery: Checking for misnamed files...
  [If found]: Rename to fill gap
  [If not found]: ERROR — manual intervention required.
    Missing file: phase-[X+1].md
    Action: Review phase directory and backlog for orphaned content
```

### Check 2: Story ID Uniqueness

**Verify:** No duplicate US-XXX across all phase files and backlog.

**If FAILS (duplicate found):**
```
Story ID uniqueness FAILED: US-[XXX] appears in:
  - phase-[A].md (Epic [B])
  - phase-[C].md (Epic [D])

Recovery: Assigning new ID to duplicate...
  US-[XXX] in phase-[C].md → US-[max+1]
  Updating backlog reference...
```

### Check 3: Backlog Consistency

**Verify:** Every US-XXX in phase files exists in backlog, and vice versa.

**If FAILS:**
```
Backlog consistency FAILED:
  Missing from backlog: US-[XXX] (found in phase-[N].md)
  Missing from phases: US-[YYY] (found in backlog.md)

Recovery:
  Adding US-[XXX] to backlog under correct epic...
  Flagging US-[YYY] — may be from a removed phase. Remove from backlog? [Yes/No]
```

### Check 4: Metrics Accuracy

**Verify:** Sum of individual story points = phase/epic totals.

**If FAILS:**
```
Metrics accuracy FAILED:
  Phase [N] total says [X] points, actual sum is [Y] points
  Epic [M] total says [A] points, actual sum is [B] points

Recovery: Updating totals to match actual sums...
  Phase [N]: [X] → [Y] points
  Epic [M]: [A] → [B] points
```

### Check 5: No Dangling References

**Verify:** Every `Phase {N}` reference points to an existing `phase-N.md`.

**If FAILS:**
```
Dangling reference FAILED:
  phase-[X].md references "Phase [Y]" but phase-[Y].md does not exist

Recovery: Likely a missed renumbering update.
  Updating reference: Phase [Y] → Phase [correct_number]
```

---

**Version:** 3.0.0
**Created:** 2026-04-01
