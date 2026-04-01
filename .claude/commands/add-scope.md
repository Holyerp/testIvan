---
name: add-scope
description: Add or edit a phase, epic, or story with automatic renumbering and cross-reference updates
---

# Add/Edit Scope Command

Add or edit phases, epics, or stories with automatic renumbering and cross-reference updates across all project files.

**NOTE:** All documentation content must be in English per `.project-management/README.md` language policy. Non-English input will be translated to English before saving.

**Context:** For how this command fits with others, see `.project-management/INTEGRATION-GUIDE.md`.

---

## Module References

| Module | Covers | Sections |
|--------|--------|----------|
| `modules/add-scope-input-parsing.md` | Argument parsing, validation, placement | 0.1-0.3, 2.1-2.3 |
| `modules/add-scope-renumbering.md` | Renumbering, cross-references, integrity, recovery | 5.1-5.6 |
| `modules/add-scope-edit-mode.md` | Edit existing items, cascade updates, removal | 3.1-3.3, 5.1-5.4 |

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

**Argument order is strict — left to right:**

| Argument | Required | Format | Description |
|----------|----------|--------|-------------|
| `add` / `edit` | Yes | keyword | Action type |
| `phase` / `epic` / `story` | Yes | keyword | Scope type |
| `[phase-number]` | For epic/story | integer (e.g., `2`) | Target phase |
| `[epic-number]` | For story | integer (e.g., `1`) | Target epic within phase |
| `[position]` | No | integer (e.g., `3`) | Insert position (omit = append at end) |
| `[US-XXX]` | For edit story | zero-padded (e.g., `US-005`, NOT `US-5`) | Story ID |
| `--from path` | No | file path | Read description from file |

**Examples of correct argument order:**
```bash
/add-scope add epic 2 1              # Add epic to Phase 2 at position 1
/add-scope add story 1 3             # Add story to Phase 1, Epic 3
/add-scope edit story US-005         # Edit story US-005
/add-scope add phase 2 --from x.md  # Add phase at position 2 from file
```

---

## YOUR TASK — MANDATORY WORKFLOW

### STEP 0: PARSE ARGUMENTS & VALIDATE

**See:** `modules/add-scope-input-parsing.md` Sections 0.1-0.3

**Claude:**
1. Parse action (`add` or `edit`), scope type (`phase`, `epic`, `story`)
2. Parse position/identifier and `--from` flag
3. Validate project is initialized (phase files must exist)
4. If `--from` provided → read and parse file content
5. If no `--from` → prompt user to describe what to add/edit

**ABORT if no phase files exist:**
```
No phase files found in .project-management/output/phases/
Run /init-project first to initialize the project structure.
```

---

### STEP 1: READ CURRENT PROJECT STATE

**Claude reads these files and extracts key data:**

1. `.project-management/output/phases/phase-*.md` — count phases, extract epic/story structure
2. `.project-management/input/backlog.md` — find MAX US-XXX ID, MAX global epic number
3. `.project-management/output/progress/current-status.md` — current metrics

**Claude displays progress:**
```
Reading project state...
  ✅ Phase files: [N] phases found (phase-1 through phase-[N])
  ✅ Backlog: max story ID = US-[XXX], max global epic = Epic [N]
  ✅ Progress: current phase status loaded
```

**Claude stores (defaults if empty project):**
- `max_story_id` — highest US-XXX number found across ALL phases AND backlog (take higher). Default: `0`
- `max_epic_number` — highest global epic number in backlog. Default: `0`
- `total_phases` — number of existing phase files. Must be ≥ 1.
- `existing_phases[]` — list of phase names/numbers with story point totals

**Note:** `max_story_id` is extracted from BOTH phase files and backlog.md — use the higher value to avoid ID collisions.

---

### STEP 2: DETERMINE PLACEMENT (add only)

**For `edit` actions: SKIP all of STEP 2 (sections 2.1-2.3). The identifier already specifies the target.**

**See:** `modules/add-scope-input-parsing.md` Sections 2.1-2.3

**Claude:**
- **Phase:** Show existing phases, ask insert position (1 to N+1, default = append)
- **Epic:** Ask target phase → show epics in that phase → ask insert position (default = append)
- **Story:** Ask target phase → ask target epic → auto-assign next US-XXX ID (`max_story_id + 1`)

---

### STEP 3: GENERATE CONTENT

**For `add` — Claude generates using these formats:**

| Scope | Template Source | Key Fields Generated |
|-------|---------------|---------------------|
| Phase | `.project-management/templates/phase-template.md` | Full phase with epics and stories, all `{{PLACEHOLDERS}}` replaced |
| Epic | Match heading format from existing phase files | `### Epic {N}: {NAME} ({POINTS} story points)` + story subsections |
| Story | Match story format from existing phase files | `#### US-{XXX}: {TITLE}` + all standard fields |

**Numbering rules:**
- New US-XXX IDs: sequential from `max_story_id + 1`. When adding multiple stories, increment for EACH: max+1, max+2, max+3...
- Epic numbering in phase files: LOCAL (sequential within that phase: Epic 1, Epic 2...)
- Epic numbering in backlog: GLOBAL (sequential across entire project: Epic 8, Epic 9...)
- Story points: Fibonacci scale only (1, 2, 3, 5, 8, 13, 21). If user provides non-Fibonacci, convert to nearest.
- Default status for all new items: `Todo`
- All fields must be populated: Priority, Status, Story Points, Acceptance Criteria, Definition of Done

**For `edit` — See:** `modules/add-scope-edit-mode.md` Sections 3.1-3.3

**Claude:**
1. Display current content of target item
2. Ask what to change (or apply `--from` file content — replaces target content, preview before saving)
3. Generate updated version
4. User can change MULTIPLE fields in single edit — apply all changes together

---

### STEP 4: PREVIEW & CONFIRMATION

**Claude displays before making ANY file changes:**

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

**IF renumbering occurs:** Show this section. **OTHERWISE:** Skip.
RENUMBERING IMPACT:
- Phase 2 "Core Features" → Phase 3
- Phase 3 "Advanced Features" → Phase 4
- Phase 4 "Polish & Launch" → Phase 5

**IF new stories created:** Show this section. **OTHERWISE:** Skip.
NEW STORY IDs: US-019, US-020, US-021

FILES TO UPDATE:
- [list of files that will change, with brief note on what changes]

**IF new files created:** Show this section. **OTHERWISE:** Skip.
FILES TO CREATE:
- [list of new files]

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

**User responds:**
```
Proceed? [Yes / No / Revise]
```

- **Yes** → Claude proceeds to STEP 5
- **No** → Claude aborts: "No changes made."
- **Revise** → Claude returns to STEP 3 (GENERATE CONTENT). User can modify content only — position is locked once set in STEP 2. Re-show preview after revision.

---

### STEP 5: EXECUTE CHANGES

**See:** `modules/add-scope-renumbering.md` Sections 5.1-5.4

**STEP 5 is ATOMIC. If ANY sub-step fails: Claude ABORTs the entire step, displays which modifications succeeded and which failed, and asks user to manually fix or retry.**

**Claude executes for `add`:**

| Scope | Actions (in order) |
|-------|-------------------|
| Phase | 1. Rename files HIGHEST→LOWEST 2. Update internal refs in ALL renamed files 3. Update cross-refs in ALL other phase files 4. Create new phase file from template 5. Update backlog 6. Update progress |
| Epic | 1. Insert epic section in phase file 2. Renumber subsequent epics in that phase (LOCAL numbering) 3. Add epic + stories to backlog (GLOBAL numbering) 4. Update phase metrics |
| Story | 1. Insert story in phase file under correct epic 2. Add story to backlog under correct epic 3. Update epic total story points 4. Update phase metrics |

**Claude executes for `edit` — See:** `modules/add-scope-edit-mode.md` Sections 5.1-5.4

1. Replace target content with updated version
2. Cascade metric updates if story points changed (story → epic total → phase total). Works for both increases AND decreases.
3. Update cross-references if dependencies changed

---

### STEP 6: INTEGRITY CHECK

**Claude runs ALL checks after changes. If ANY check FAILS → Claude ABORTs remaining steps, displays failure details, and directs user to recovery procedures.**

| # | Check | Validates |
|---|-------|-----------|
| 1 | Phase continuity | `phase-1.md` through `phase-N.md` exist, no gaps. Each file's internal heading matches filename. |
| 2 | Story ID uniqueness | No duplicate US-XXX across all phase files and backlog |
| 3 | Backlog consistency | Every US-XXX in phases exists in backlog AND vice versa |
| 4 | Metrics accuracy | Sum of individual story points = epic totals = phase totals |
| 5 | Cross-references | Every `Phase {N}` reference points to existing `phase-N.md` |

**Claude displays:**
```
INTEGRITY CHECK:
  ✅ Phase continuity: PASSED (phase-1 through phase-[N])
  ✅ Story ID uniqueness: PASSED ([N] unique IDs)
  ✅ Backlog consistency: PASSED
  ✅ Metrics accuracy: PASSED
  ✅ Cross-references: PASSED
```

**IF any check FAILS:** See `modules/add-scope-renumbering.md` Section 5.6 for specific recovery procedures per check.

---

### STEP 7: ASK FOR DOCUMENTATION UPDATE

**Claude asks:**
```
DOCUMENTATION UPDATE

Your changes may affect these documents:
- PRD (prd.md) — new features/stories
- Technical Spec (technical-spec.md) — technical details
- Architecture (architecture.md) — architectural changes

Options:
[1] Auto-update affected documents now
    → Claude updates only the sections related to your changes
    → Includes: new features in PRD, technical details in spec,
      architectural changes if the new phase/epic requires them

[2] Run /generate-docs later
    → Only phase files and backlog are updated for now
    → Run: /generate-docs when ready to regenerate all docs
```

**If docs (prd.md, technical-spec.md, architecture.md) don't exist yet:** Skip option [1], show only option [2] with note: "Run /init-project or /generate-docs to create these documents first."

- **User selects [1]** → Claude reads existing docs, updates relevant sections with new content
- **User selects [2]** → Claude skips, shows reminder in summary report

---

### STEP 8: SUMMARY REPORT

**Claude displays:**
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ SCOPE [ADDITION / EDIT] COMPLETE
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

COMPLETED:
- [Added Phase 2: "API Integration" — 2 epics, 5 stories, 26 story points]

**IF renumbering occurred:**
RENUMBERED:
- Phase 2 → Phase 3 (was "Core Features")
- Phase 3 → Phase 4 (was "Advanced Features")
- Phase 4 → Phase 5 (was "Polish & Launch")

**IF new stories created:**
NEW STORY IDs:
- US-019: [Title]
- US-020: [Title]

FILES UPDATED:
- [list with notes on what changed]

**IF new files created:**
FILES CREATED:
- [list]

INTEGRITY CHECK: ✅ PASSED

**IF docs NOT auto-updated:**
REMINDER: Run /generate-docs to update PRD, tech-spec, and architecture

NEXT STEPS:
1. Review new content: .project-management/output/phases/phase-[N].md
2. Review updated backlog: .project-management/input/backlog.md
3. Run /project-status to verify updated metrics
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## Success Criteria

Scope change is COMPLETE when ALL the following pass:

1. Preview approved by user (STEP 4)
2. All files renamed without collisions, if renumbering was needed (STEP 5)
3. All cross-references updated to new numbers (STEP 5)
4. Backlog matches phase files — every US-XXX is consistent (STEP 6, Check 3)
5. Metrics are accurate — story points sum correctly at epic and phase level (STEP 6, Check 4)
6. No duplicate US-XXX IDs (STEP 6, Check 2)
7. All 5 integrity checks pass (STEP 6)
8. Summary report displayed (STEP 8)

---

## Error Handling

| Error | Cause | Recovery | Details in |
|-------|-------|----------|------------|
| No phase files | Project not initialized | Abort, suggest `/init-project` | STEP 0 |
| `--from` file not found | Wrong path | Ask for correct path or manual input (max 2 retries) | `add-scope-input-parsing.md` 0.1 |
| Phase number out of range | Invalid position | Show valid range, re-prompt (max 3 attempts) | `add-scope-input-parsing.md` 2.1 |
| US-XXX not found (edit) | Invalid story ID | List valid IDs with locations, ask again | `add-scope-input-parsing.md` Validation |
| US-XXX format wrong | e.g., `US-5` instead of `US-005` | Show correct format: `US-XXX` (zero-padded, 3 digits) | STEP 0 |
| Rename collision | Rename order violated | Abort STEP 5 immediately, list affected files | `add-scope-renumbering.md` 5.1.1 |
| Integrity check fails | Inconsistent state after changes | Abort remaining steps, run recovery | `add-scope-renumbering.md` 5.6 |
| Duplicate story title | Potential duplicate content | Warn user, let them decide to proceed or cancel | STEP 3 |
| Mid-operation cancel | User aborts during STEP 5 | List which files were already modified, suggest manual review | STEP 5 |
| Partial STEP 5 failure | File write error mid-operation | Abort, display succeeded/failed modifications | STEP 5 |

---

## Example Walkthrough

### Adding a Phase at Position 2

```
User: /add-scope add phase 2

Claude: [STEP 0] Parsing: action=add, type=phase, position=2
Claude: [STEP 1] Reading project state...
  ✅ Phase files: 4 phases found (phase-1 through phase-4)
  ✅ Backlog: max story ID = US-018, max global epic = Epic 7
  ✅ Progress: Phase 1 in progress

Claude: [STEP 2] Position 2 selected (from command args).
  Current phases:
    Phase 1: Foundation & Setup (19 points)
    Phase 2: Core Features (42 points) ← will become Phase 3
    Phase 3: Advanced Features (26 points) ← will become Phase 4
    Phase 4: Polish & Launch (16 points) ← will become Phase 5

Claude: Describe the new phase (name, goal, epics/stories):

User: "API Integration phase — handles external services.
       Epic: Payment Gateway (Stripe integration, webhook handling)
       Epic: Email Service (transactional emails, templates)"

Claude: [STEP 3] Generating phase content...
  Assigned: Epic 1 (local), Epic 8 (global backlog), US-019 through US-024
  Template: phase-template.md filled with user content

Claude: [STEP 4] Shows SCOPE CHANGE PREVIEW:
  - ACTION: Add, TYPE: Phase, TARGET: Position 2
  - RENUMBERING: Phase 2→3, 3→4, 4→5
  - NEW STORY IDs: US-019 through US-024
  - FILES TO UPDATE: phase-2→3, phase-3→4, phase-4→5, backlog, progress
  - FILES TO CREATE: phase-2.md

User: Yes

Claude: [STEP 5] Executing changes...
  Renamed: phase-4.md → phase-5.md ✅
  Renamed: phase-3.md → phase-4.md ✅
  Renamed: phase-2.md → phase-3.md ✅
  Updated internal refs in phase-3, phase-4, phase-5 ✅
  Updated cross-refs in phase-1 ✅
  Created: phase-2.md ✅
  Updated: backlog.md (added Epic 8, Epic 9) ✅
  Updated: current-status.md ✅

Claude: [STEP 6] INTEGRITY CHECK: ✅ ALL PASSED

Claude: [STEP 7] Documentation update?
  [1] Auto-update  [2] Run /generate-docs later

User: 2

Claude: [STEP 8]
  ✅ SCOPE ADDITION COMPLETE
  COMPLETED: Phase 2 "API Integration" — 2 epics, 6 stories, 34 points
  RENUMBERED: Phase 2→3, 3→4, 4→5
  NEW STORY IDs: US-019 through US-024
  INTEGRITY CHECK: ✅ PASSED
  REMINDER: Run /generate-docs to update PRD, tech-spec, architecture
```

---

## Key Rules

1. **NEVER skip the preview** — user must approve before any file changes
2. **NEVER renumber US-XXX story IDs** — they are immutable identifiers
3. **ALWAYS rename phase files HIGHEST→LOWEST** — prevents file collisions
4. **ALWAYS run integrity checks** — never skip, even for simple additions
5. **ALWAYS update both phase files AND backlog** — they must stay in sync
6. **Story points use Fibonacci scale** — 1, 2, 3, 5, 8, 13, 21 only
7. **Epic numbering is LOCAL in phase files, GLOBAL in backlog** — see STEP 3
8. **STEP 5 is atomic** — if any sub-step fails, abort entire step

---

## Notation Reference

| Format | Meaning | Example |
|--------|---------|---------|
| `phase-{N}` | Phase filename | `phase-2.md` |
| `Phase {N}` | Phase reference in prose | `Phase 2` |
| `[N]` | User option in prompt | `[1] Phase 1: Foundation` |
| `US-{XXX}` | Story ID (zero-padded) | `US-005` |
| `Epic {N}` (in phase file) | Local epic number | `Epic 2` in Phase 1 |
| `Epic {N}` (in backlog) | Global epic number | `Epic 8` |

---

**Version:** 3.0.0
**Last Updated:** 2026-04-02
**Command Type:** Scope Management
