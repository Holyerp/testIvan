---
name: add-scope
description: Add or edit a phase, epic, or story with automatic renumbering and cross-reference updates
---

# Add/Edit Scope Command

Add or edit phases, epics, or stories with automatic renumbering and cross-reference updates across all project files.

---

## Module References

| Module | Covers | Steps |
|--------|--------|-------|
| `modules/add-scope-input-parsing.md` | Argument parsing, validation, placement | STEP 0, STEP 2 |
| `modules/add-scope-renumbering.md` | Renumbering, cross-references, integrity | STEP 5, STEP 6 |
| `modules/add-scope-edit-mode.md` | Edit existing items, cascade updates | STEP 3 (edit), STEP 5 (edit) |

---

## Usage

```bash
# ADD new items
/add-scope add phase [position] [--from path/to/file.md]
/add-scope add epic [phase-number] [position] [--from path/to/file.md]
/add-scope add story [phase-number] [epic-number] [--from path/to/file.md]

# EDIT existing items
/add-scope edit phase [phase-number] [--from path/to/file.md]
/add-scope edit epic [phase-number] [epic-number] [--from path/to/file.md]
/add-scope edit story [US-XXX] [--from path/to/file.md]
```

**Arguments:**

| Argument | Required | Description |
|----------|----------|-------------|
| `add` / `edit` | Yes | Action type |
| `phase` / `epic` / `story` | Yes | Scope type |
| `[position]` | No | Insert position (omit = append at end) |
| `[phase-number]` | For epic/story | Target phase (e.g., `1`, `2`) |
| `[epic-number]` | For story | Target epic within phase (e.g., `1`, `3`) |
| `[US-XXX]` | For edit story | Story ID (e.g., `US-005`) |
| `--from path` | No | Read description from file instead of interactive input |

---

## YOUR TASK — MANDATORY WORKFLOW

### STEP 0: PARSE ARGUMENTS & VALIDATE

**See:** `modules/add-scope-input-parsing.md` for complete workflow

1. Parse action (`add` or `edit`), scope type (`phase`, `epic`, `story`)
2. Parse position/identifier and `--from` flag
3. Validate project is initialized (phase files must exist)
4. If `--from` provided → read and parse file content
5. If no `--from` → ask user to describe what to add/edit

**ABORT if no phase files exist:**
```
No phase files found. Run /init-project first.
```

---

### STEP 1: READ CURRENT PROJECT STATE (MANDATORY)

**Read these files and extract key data:**

1. `.project-management/output/phases/phase-*.md` — count phases, extract epic/story structure
2. `.project-management/input/backlog.md` — find MAX US-XXX ID, MAX epic number
3. `.project-management/output/progress/current-status.md` — current metrics

**Display progress:**
```
Reading project state...
  ✅ Phase files: [N] phases found
  ✅ Backlog: max story ID = US-[XXX], max epic = Epic [N]
  ✅ Progress: current phase status loaded
```

**Store:** `max_story_id`, `max_epic_number`, `total_phases`, `existing_phases[]`

---

### STEP 2: DETERMINE PLACEMENT (add only)

**See:** `modules/add-scope-input-parsing.md` — Section 2

**Phase:** Show existing phases, ask insert position (1 to N+1, default = append)
**Epic:** Ask target phase → show epics → ask insert position (default = append)
**Story:** Ask target phase → ask target epic → auto-assign next US-XXX ID

**Skip this step for `edit` action** — identifier already specifies the target.

---

### STEP 3: GENERATE CONTENT

**For `add`:**

| Scope | Template | Generated Sections |
|-------|----------|--------------------|
| Phase | `.project-management/templates/phase-template.md` | Full phase with epics and stories |
| Epic | Match format from existing phase files | Epic heading + story subsections |
| Story | Match format from existing phase files | Single story with all standard fields |

- Assign new US-XXX numbers starting from `max_story_id + 1`
- Assign new epic numbers starting from `max_epic_number + 1` (global) and sequential within phase (local)
- Fill all template fields: Priority, Status (default: "Todo"), Story Points, Acceptance Criteria, Definition of Done

**For `edit`:**

**See:** `modules/add-scope-edit-mode.md` for complete workflow

- Display current content of the target item
- Ask what to change (or apply `--from` file content)
- Generate updated version

---

### STEP 4: PREVIEW & CONFIRMATION (MANDATORY)

**Display before making ANY changes:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SCOPE CHANGE PREVIEW
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

ACTION: [Add / Edit]
TYPE: [Phase / Epic / Story]
TARGET: [Phase N / Epic N in Phase M / US-XXX in Epic N, Phase M]

NEW/UPDATED CONTENT:
────────────────────────────────────────
[Markdown preview of content]
────────────────────────────────────────

{{If renumbering needed:}}
RENUMBERING IMPACT:
- Phase 2 "Core Features" → Phase 3
- Phase 3 "Advanced Features" → Phase 4
- Phase 4 "Polish & Launch" → Phase 5

{{If new stories:}}
NEW STORY IDs: US-019, US-020, US-021

FILES TO MODIFY:
- [list of files that will change]

{{If new files:}}
FILES TO CREATE:
- [list of new files]

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Proceed? [Yes / No / Revise]
```

- **Yes** → STEP 5
- **No** → Abort: "No changes made."
- **Revise** → Ask what to change, regenerate, re-show preview

---

### STEP 5: EXECUTE CHANGES

**See:** `modules/add-scope-renumbering.md` for complete logic

**For `add`:**

| Scope | Actions |
|-------|---------|
| Phase | 1. Rename files HIGHEST→LOWEST 2. Update internal refs 3. Create new phase file 4. Update backlog 5. Update progress |
| Epic | 1. Insert in phase file 2. Renumber subsequent epics 3. Update backlog 4. Update phase metrics |
| Story | 1. Insert in phase file 2. Update backlog 3. Update epic/phase metrics |

**For `edit`:**

**See:** `modules/add-scope-edit-mode.md`

1. Replace target content with updated version
2. Cascade metric updates if story points changed (story → epic total → phase total)
3. Update cross-references if dependencies changed

---

### STEP 6: INTEGRITY CHECK (MANDATORY)

**Run ALL checks after changes:**

| Check | Validates |
|-------|-----------|
| Phase continuity | phase-1.md through phase-N.md, no gaps |
| Story ID uniqueness | No duplicate US-XXX across all files |
| Backlog consistency | Every US-XXX in phases exists in backlog and vice versa |
| Metrics accuracy | Sum of story points matches phase/epic totals |
| Cross-references | No references to non-existent phase numbers |

**Display:**
```
INTEGRITY CHECK:
  ✅ Phase continuity: PASSED (phase-1 through phase-[N])
  ✅ Story ID uniqueness: PASSED ([N] unique IDs)
  ✅ Backlog consistency: PASSED
  ✅ Metrics accuracy: PASSED
  ✅ Cross-references: PASSED
```

**If any check FAILS → See:** `modules/add-scope-renumbering.md` — Section 5.6 for recovery procedures

---

### STEP 7: ASK FOR DOCUMENTATION UPDATE

```
DOCUMENTATION UPDATE

Your changes may affect these documents:
- PRD (prd.md) — new features/stories
- Technical Spec (technical-spec.md) — technical details
- Architecture (architecture.md) — architectural changes

Options:
[1] Auto-update all documents now
    → Updates PRD, tech-spec, and architecture with new content
    → Includes: adding new features to PRD, technical details to spec,
      and architectural changes if the new phase/epic requires them

[2] Run /generate-docs later
    → Only phase files and backlog are updated for now
    → Run /generate-docs when ready to regenerate all docs
```

- **[1]** → Read existing docs, update relevant sections
- **[2]** → Skip, show reminder in summary

---

### STEP 8: SUMMARY REPORT

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ SCOPE [ADDITION / EDIT] COMPLETE
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

COMPLETED:
- [Added Phase 2: "API Integration" — 2 epics, 5 stories, 26 story points]

{{If renumbering occurred:}}
RENUMBERED:
- Phase 2 → Phase 3 (was "Core Features")
- Phase 3 → Phase 4 (was "Advanced Features")
- Phase 4 → Phase 5 (was "Polish & Launch")

{{If new stories:}}
NEW STORY IDs:
- US-019: [Title]
- US-020: [Title]

FILES MODIFIED:
- [list with notes on what changed]

{{If new files:}}
FILES CREATED:
- [list]

INTEGRITY CHECK: ✅ PASSED

{{If docs NOT auto-updated:}}
REMINDER: Run /generate-docs to update PRD, tech-spec, and architecture

NEXT STEPS:
1. Review new content: .project-management/output/phases/phase-[N].md
2. Review updated backlog: .project-management/input/backlog.md
3. Run /project-status for updated metrics
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## Success Criteria

**Scope change is COMPLETE when ALL pass:**

- [ ] Preview approved by user
- [ ] All files renamed without collisions (if renumbering)
- [ ] All cross-references updated to new numbers
- [ ] Backlog matches phase files (every US-XXX consistent)
- [ ] Metrics are accurate (story points sum correctly)
- [ ] No duplicate US-XXX IDs
- [ ] All 5 integrity checks pass
- [ ] Summary report displayed

---

## Error Handling

| Error | Cause | Recovery |
|-------|-------|----------|
| No phase files | Project not initialized | Abort, suggest `/init-project` |
| `--from` file not found | Wrong path | Ask for correct path or manual input |
| Phase number out of range | Invalid position | Show valid range, ask again |
| US-XXX not found (edit) | Invalid story ID | List valid IDs, ask again |
| Rename collision | Bug in rename order | Verify HIGHEST→LOWEST order, retry |
| Integrity check fails | Inconsistent state | See `modules/add-scope-renumbering.md` Section 5.6 |
| Duplicate story title | Potential duplicate | Warn user, let them decide to proceed or cancel |
| Mid-operation cancel | User aborts | Mark partial state, warn which files changed |

---

## Example Walkthrough

### Adding a Phase at Position 2

```
User: /add-scope add phase 2

Claude: Reading project state...
  ✅ Phase files: 4 phases found
  ✅ Backlog: max story ID = US-018, max epic = Epic 7
  ✅ Progress: loaded

Claude: Existing phases:
  [1] Phase 1: Foundation & Setup (19 points)
  [2] Phase 2: Core Features (42 points)
  [3] Phase 3: Advanced Features (26 points)
  [4] Phase 4: Polish & Launch (16 points)
  [5] → Append at end

  Insert position: 2 ← (from user command)

Claude: Describe the new phase (name, goal, epics/stories):

User: "API Integration phase — handles external services.
       Epic: Payment Gateway (Stripe integration, webhook handling)
       Epic: Email Service (transactional emails, templates)"

Claude: [Generates phase content, assigns US-019 through US-024]
        [Shows PREVIEW with renumbering: Phase 2→3, 3→4, 4→5]

User: Yes

Claude: [Renames phase-4→5, phase-3→4, phase-2→3]
        [Creates new phase-2.md]
        [Updates backlog.md with Epic 8, Epic 9]
        [Updates current-status.md]

Claude: INTEGRITY CHECK: ✅ ALL PASSED

Claude: Documentation update?
        [1] Auto-update  [2] Run /generate-docs later

User: 2

Claude: ✅ SCOPE ADDITION COMPLETE
        [Shows summary with all changes]
        REMINDER: Run /generate-docs to update PRD, tech-spec, architecture
```

---

## Key Rules

1. **NEVER skip the preview** — user must approve before any file changes
2. **NEVER renumber US-XXX story IDs** — they are immutable identifiers
3. **ALWAYS rename phase files HIGHEST→LOWEST** — prevents file collisions
4. **ALWAYS run integrity checks** — never skip even for simple additions
5. **ALWAYS update both phase files AND backlog** — they must stay in sync

---

**Version:** 3.0.0
**Created:** 2026-04-01
**Command Type:** Scope Management
