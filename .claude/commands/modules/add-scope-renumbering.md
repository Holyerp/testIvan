# Add Scope — Renumbering Module

**Referenced by:** `add-scope.md` STEP 5 (Sections 5.1-5.4) & STEP 6 (Section 5.6)

---

## STEP 5: EXECUTE CHANGES — Renumbering & Cross-References

This module handles all file mutations: renaming, content updates, insertions, and metric recalculations.

**Atomicity rule:** If ANY sub-step fails, Claude ABORTs the entire STEP 5, displays which modifications succeeded and which failed, and asks user to manually review.

---

## 5.1 Phase Renumbering (add phase at position N)

### 5.1.1 Rename Phase Files

**CRITICAL: Claude MUST rename from HIGHEST number downward to prevent file collisions. NEVER iterate forward (N up to T) — only backward (T down to N).**

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

**If rename fails (file locked, permission error):** Abort immediately. Do NOT continue to next rename. Display: "Rename failed: phase-{i}.md → phase-{i+1}.md. Reason: [error]. Aborting. Files renamed so far: [list]."

### 5.1.2 Update Content in Each Renamed File

**Claude opens each renamed file and performs find-and-replace:**

| Find (exact match) | Replace With | Location in File |
|---------------------|-------------|-----------------|
| `# Phase {old_N}:` | `# Phase {new_N}:` | Title heading (line 1) |
| `Phase {old_N}` in prose | `Phase {new_N}` | Whole-word match, NOT inside filenames like `phase-N.md` |
| References in "Carry Over to Next Phase" section | Update `Phase {old_N + 1}` → `Phase {new_N + 1}` | Carry Over section near end of file |
| `Phase {old_N} -` in Final Metrics | `Phase {new_N} -` | Final Metrics heading |

**String matching rule:** Use exact string match (no regex). If format doesn't match exactly, Claude flags it and continues — does NOT abort for formatting mismatches.

### 5.1.3 Update Cross-References in ALL Phase Files

**Claude scans EVERY file for `Phase {X}` references where X >= insert_position:**

Files to scan:
1. ALL phase files (including non-renamed ones, e.g., `phase-1.md`)
2. `.project-management/input/backlog.md`
3. `.project-management/output/progress/current-status.md`

For each `Phase {X}` reference where X >= insert_position → replace with `Phase {X + 1}`.

**Exception:** References to phases BEFORE insert_position stay unchanged.

### 5.1.4 Create New Phase File

**Claude:**
1. Read template: `.project-management/templates/phase-template.md`
   - If template missing → abort: "Template file not found. Cannot create phase without template."
2. Replace ALL `{{PLACEHOLDERS}}` with content from user input:
   - `{{PHASE_NUMBER}}` → insert_position
   - `{{PHASE_NAME}}` → from user input
   - `{{PHASE_GOAL_DESCRIPTION}}` → from user input
   - `{{STATUS}}` → `Planning`
   - Epic and story sections → generated from input content
3. Assign story IDs: `US-{max_story_id + 1}`, `US-{max_story_id + 2}`, ... (increment for EACH story)
4. Use phase-LOCAL epic numbering: Epic 1, Epic 2, ...
5. Write to: `.project-management/output/phases/phase-{N}.md`

### 5.1.5 Update Backlog

**Claude:**
1. Read `.project-management/input/backlog.md`
2. Add new epics with GLOBAL numbers (increment `max_epic_number` for EACH):
   - First epic: `## Epic {max_epic_number + 1}: {Name}`
   - Second epic: `## Epic {max_epic_number + 2}: {Name}`
3. Add stories under each epic with assigned US-XXX IDs
4. Write updated backlog

### 5.1.6 Update Progress

**Claude:**
1. Read `.project-management/output/progress/current-status.md`
2. Update: total phases count, total stories count, total story points
3. Add new phase to phase list (if tracked individually)
4. Update any `Phase {N}` references that shifted (same rule as 5.1.3)
5. Write updated progress

---

## 5.2 Epic Renumbering (add epic to phase at position P)

### 5.2.1 Insert in Phase File

**Claude:**
1. Read target: `.project-management/output/phases/phase-{target_phase}.md`
2. Find insertion point using `### Epic {N}:` headers as section boundaries
3. Insert new epic markdown block at position P:
```markdown
### Epic {P}: {Name} ({points} story points)

**Priority:** {P0/P1/P2}
**Status:** Todo
**Dependencies:** {deps or "None"}

**User Stories:**

#### US-{XXX}: {Story Title}
- **Story Points:** {N}
- **Priority:** {P0/P1/P2}
- **Status:** Todo
...
```
4. Write updated file to same path: `.project-management/output/phases/phase-{target_phase}.md`

### 5.2.2 Renumber Subsequent Epics

**Epic numbering is LOCAL to each phase file. Claude renumbers within the target phase only:**

```
For each epic at position >= P (in the same phase file):
  Find: ### Epic {N}:
  Replace: ### Epic {N+1}:
```

This does NOT affect epic numbering in other phase files or in backlog (backlog uses global numbers).

### 5.2.3 Update Phase Metrics

In the same phase file, Claude updates:
- `Total Story Points: {old + new_epic_points}`
- `Total Stories: {old + new_story_count}`
- `Total Epics: {old + 1}`

### 5.2.4 Update Backlog

**Claude adds epic to backlog with GLOBAL numbering:**
1. New epic: `## Epic {max_epic_number + 1}: {Name}`
2. Add all stories with assigned US-XXX IDs under it
3. If adding multiple epics: increment for EACH (max+1, max+2, ...)

---

## 5.3 Story Addition (add story to epic)

### 5.3.1 Insert in Phase File

**Claude:**
1. Read target phase file
2. Find epic section: `### Epic {target_epic}:`
3. Find last `#### US-{XXX}:` within that epic (scan until next `### Epic` or end of section)
4. Insert new story after last story in that epic
5. Write updated file

### 5.3.2 Update Epic Total

```
Find: ### Epic {N}: {Name} ({old_points} story points)
Replace: ### Epic {N}: {Name} ({old_points + new_points} story points)
```

If heading format doesn't match exactly → Claude recalculates by summing all story points in the epic and writes the correct total.

### 5.3.3 Update Phase Metrics

- `Total Story Points: {old + new_points}`
- `Total Stories: {old + 1}`

### 5.3.4 Update Backlog

Claude finds matching epic in `backlog.md` and appends story:
```markdown
### US-{XXX}: {Title}
**Priority:** {Priority}
**Story:** As a [role], I want to [action] so that [benefit]

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
| Phase | Create `phase-{T+1}.md` + update backlog + update progress | None |
| Epic | Append epic section at end of phase's epics | None |
| Story | Append story at end of epic's stories | None |

This is the simplest path — only content creation and metric updates. No file renames.

---

## 5.5 Cross-Reference Rules

### References That MUST Be Updated

| Location | What Changes | When |
|----------|-------------|------|
| Phase file titles | `# Phase N:` heading | Phase insertion |
| Phase file prose | `Phase N` mentions | Phase insertion |
| Carry Over sections | "Next Phase" refs | Phase insertion |
| Progress current-status | Phase numbers, all metrics | Any add |
| Backlog epic numbers | `## Epic N:` (global) | Epic addition |
| Phase metric totals | Story points, story count, epic count | Epic/Story add |
| Epic heading totals | `({N} story points)` | Story add |

### References That NEVER Change

| Reference | Reason |
|-----------|--------|
| US-XXX story IDs | Immutable identifiers — never renumbered |
| Story dependency refs (US-XXX) | Based on immutable IDs |
| Git commit references | Historical records |
| Progress log entries | Historical — add "(formerly Phase N)" note if needed |

---

## 5.6 Integrity Verification & Recovery

**Claude runs after ALL mutations complete. If ANY check fails → abort remaining steps (STEP 7, 8).**

### Check 1: Phase File Continuity

**Claude verifies:** `phase-1.md` through `phase-N.md` exist with no gaps. Also: each file's internal `# Phase {N}:` heading matches its filename number.

**IF FAILS:**
```
Phase continuity FAILED: gap between phase-[X] and phase-[Y]
OR: phase-[N].md heading says "Phase [M]" (mismatch)

Recovery:
  [Gap]: Check for misnamed files. If found → rename to fill gap.
         If not found → ABORT. Manual intervention required.
  [Mismatch]: Update heading to match filename.
```

### Check 2: Story ID Uniqueness

**Claude verifies:** No duplicate US-XXX across all phase files and backlog.

**IF FAILS:**
```
Story ID uniqueness FAILED: US-[XXX] appears in:
  - phase-[A].md (Epic [B])
  - phase-[C].md (Epic [D])

Recovery:
  Assign new ID to duplicate in phase-[C].md:
  US-[XXX] → US-[next_available]
  Verify next_available not already used (check incrementally).
  Update backlog reference.
```

### Check 3: Backlog Consistency

**Claude verifies:** Every US-XXX in phase files exists in backlog AND vice versa.

**IF FAILS:**
```
Backlog consistency FAILED:
  Missing from backlog: US-[XXX] (found in phase-[N].md)
  Missing from phases: US-[YYY] (found in backlog.md only)

Recovery:
  Missing from backlog → add entry to backlog under correct epic.
  Missing from phases → ask user:
    "US-[YYY] exists in backlog but not in any phase. Remove from backlog? [Yes/No]"
    If No → flag in current-status.md as "Orphaned story — manual review needed". Continue.
```

### Check 4: Metrics Accuracy

**Claude verifies:** Sum of individual story points = epic totals = phase totals.

**IF FAILS:**
```
Metrics accuracy FAILED:
  Phase [N] total: listed [X] points, actual sum [Y] points
  Epic [M] total: listed [A] points, actual sum [B] points

Recovery:
  Update all totals to match actual sums.
  Phase [N]: [X] → [Y] points
  Epic [M]: [A] → [B] points
  Log: "Metrics auto-corrected."
```

### Check 5: No Dangling References

**Claude verifies:** Every `Phase {N}` reference in all files points to existing `phase-N.md`.

**IF FAILS:**
```
Dangling reference FAILED:
  phase-[X].md references "Phase [Y]" but phase-[Y].md does not exist

Recovery:
  Find ALL instances of "Phase [Y]" in the file.
  Replace ALL with "Phase [correct_number]".
  Log count of replacements.
```

---

**Version:** 3.0.0
**Last Updated:** 2026-04-02
