# Add Scope — Input Parsing Module

**Referenced by:** `add-scope.md` STEP 0 (Sections 0.1-0.3) & STEP 2 (Sections 2.1-2.3)

---

## STEP 0: PARSE ARGUMENTS & VALIDATE

### 0.1 Parse Command Arguments

**Claude extracts arguments in strict left-to-right order:**

**1. Action** (required): `add` or `edit`

If missing or invalid → Claude asks:
```
Which action?
[1] Add — Add a new phase, epic, or story
[2] Edit — Modify an existing phase, epic, or story
```

**2. Scope type** (required): `phase`, `epic`, or `story`

If missing or invalid → Claude asks:
```
What do you want to [add/edit]?
[1] Phase — A complete development phase
[2] Epic — A feature group within a phase
[3] Story — A single user story within an epic
```

**3. Position/Identifier** (context-dependent):

| Action + Scope | Argument Order | Example | Default |
|---|---|---|---|
| `add phase` | `[position]` | `/add-scope add phase 2` | Append at end |
| `add epic` | `[phase-number] [position]` | `/add-scope add epic 2 1` (Phase 2, position 1) | Append at end of phase |
| `add story` | `[phase-number] [epic-number]` | `/add-scope add story 1 3` (Phase 1, Epic 3) | Auto-assign US-XXX |
| `edit phase` | `[phase-number]` | `/add-scope edit phase 3` | — (must specify) |
| `edit epic` | `[phase-number] [epic-number]` | `/add-scope edit epic 2 1` (Phase 2, Epic 1) | — (must specify) |
| `edit story` | `[US-XXX]` | `/add-scope edit story US-005` | — (must specify) |

**US-XXX format:** Must be zero-padded, 3 digits: `US-005`, NOT `US-5`. If wrong format → Claude shows: "Story ID must be in format US-XXX (e.g., US-005)."

**4. --from flag** (optional): path to content file

If provided → Claude validates file exists:
- **File found** → read content, store as `input_content`
- **File NOT found** → error and offer alternatives (max 2 retries):
```
File not found: [path]

[1] Enter correct file path
[2] Describe the content manually
```
After 2 failed path attempts → Claude requires option [2] (manual input).

---

### 0.2 Validate Project State

**Claude checks:** Do phase files exist in `.project-management/output/phases/`?

- **Directory AND phase files exist** → continue
- **Directory exists but no phase files** → abort:
```
No phase files found in .project-management/output/phases/
Run /init-project first to set up the project structure.
```
- **Directory doesn't exist** → abort:
```
Project management structure not found.
Create .project-management/ and run /init-project first.
```

---

### 0.3 Gather Content (if no --from file)

**Claude prompts based on action + scope type:**

#### Add Phase
```
Describe the new phase:

1. Phase name (e.g., "API Integration & External Services")
2. Phase goal/description
3. Estimated duration (e.g., "6 weeks", "2 months")
4. Epics and stories (describe features — I'll structure them)

Write as much or as little as you want. I'll fill in details using the project template.
```

**Claude parses response for:**
- Phase name [REQUIRED] — if missing, ask follow-up
- Phase goal [REQUIRED] — if missing, ask follow-up
- Epics/stories described [OPTIONAL] — Claude generates if not provided
- Duration [OPTIONAL] — Claude estimates if not provided

#### Add Epic
```
Describe the new epic:

1. Epic name (e.g., "Notification System")
2. What this epic covers
3. User stories (or describe features — I'll create stories)
4. Priority (P0/P1/P2)

I'll generate acceptance criteria and story point estimates.
```

**Claude parses for:** Epic name [REQUIRED], description [OPTIONAL], stories [OPTIONAL], priority [OPTIONAL, default P1].

#### Add Story
```
Describe the new user story:

1. Story title (e.g., "Email Notifications for Orders")
2. User story: "As a [role], I want to [action] so that [benefit]"
3. Acceptance criteria (or describe what should work)
4. Priority (P0/P1/P2)
5. Estimate in story points (1/2/3/5/8/13) — or I'll estimate

I'll generate the full story with Definition of Done.
```

**Claude parses for:** Title [REQUIRED], user story [OPTIONAL — Claude generates if missing], acceptance criteria [OPTIONAL], priority [OPTIONAL, default P1], estimate [OPTIONAL — Claude estimates if missing].

**Story point rules:** Fibonacci scale only (1, 2, 3, 5, 8, 13, 21). If user provides non-Fibonacci (e.g., "4" or "small"), Claude converts to nearest Fibonacci value and shows conversion in preview.

#### Edit Phase
```
What to change about Phase [N]: [Name]?

[1] Phase name/goal
[2] Duration/dates
[3] Status
[4] Add epic to this phase
[5] Remove an epic from this phase
[6] Describe changes freely
```
Selecting [4] → Claude automatically executes `/add-scope add epic [N]` with current context preserved. User does NOT need to type a new command.

#### Edit Epic
```
What to change about Epic [N]: [Name] in Phase [M]?

[1] Epic name/description
[2] Priority
[3] Status
[4] Dependencies
[5] Add story to this epic
[6] Remove a story from this epic
[7] Describe changes freely
```
Selecting [5] → Claude automatically executes `/add-scope add story [M] [N]`.
Selecting [6] → Claude shows stories in epic, asks which to remove, requires confirmation.

#### Edit Story
```
What to change about [US-XXX]: [Title]?

[1] Story title/description
[2] Acceptance criteria
[3] Story points
[4] Priority
[5] Dependencies
[6] Status
[7] Technical notes
[8] Describe changes freely
```

**Multiple changes:** User can select multiple options or use [8] to describe several changes at once. Claude applies ALL changes in a single operation.

---

## STEP 2: DETERMINE PLACEMENT (add only)

**For `edit` actions: Claude SKIPs all of STEP 2 (sections 2.1-2.3). The identifier already specifies the target.**

### 2.1 Phase Placement

**Claude displays current phases and asks for position:**
```
PHASE PLACEMENT

Existing phases:
  [1] Phase 1: [Name] ([N] points, [Status])
  [2] Phase 2: [Name] ([N] points, [Status])
  [3] Phase 3: [Name] ([N] points, [Status])
  [4] Phase 4: [Name] ([N] points, [Status])
  [5] → Append at end (becomes Phase 5)

Insert at position (1-5, default = 5):
```

**Position semantics:**
- Position 1 = insert BEFORE current Phase 1 (Phase 1 becomes Phase 2, etc.)
- Position K (2 to N) = insert BEFORE current Phase K (Phase K and above shift right by 1)
- Position N+1 = append AFTER current Phase N (no renumbering)

**If position provided in command args** → Claude uses it directly, skips prompt.

**Validation:** Valid range is 1 to N+1. If input is 0, negative, or > N+1 → Claude shows valid range and re-prompts (max 3 attempts, then abort).

**Claude stores:** `insert_position`

### 2.2 Epic Placement

**Step 1 — Claude asks for target phase:**
```
EPIC PLACEMENT — Select Phase

[1] Phase 1: [Name]
[2] Phase 2: [Name]
[3] Phase 3: [Name]
[4] Phase 4: [Name]

Target phase:
```

**Step 2 — Claude asks for position within phase:**
```
EPIC PLACEMENT — Position in Phase [N]

Current epics in Phase [N]:
  [1] Epic 1: [Name] ([N] points)
  [2] Epic 2: [Name] ([N] points)
  [3] → Append (becomes Epic 3)

Insert at position (default = append):
```

**Note:** Position is LOCAL to this phase. Epic 1-3 in Phase 2 are independent from Epic 1-2 in Phase 1.

**Claude stores:** `target_phase`, `insert_position`

### 2.3 Story Placement

**Step 1:** Claude asks for target phase (same prompt as 2.2 Step 1)

**Step 2 — Claude asks for target epic:**
```
STORY PLACEMENT — Select Epic in Phase [N]

[1] Epic 1: [Name] ([N] stories, [N] points)
[2] Epic 2: [Name] ([N] stories, [N] points)

Target epic:
```

**Step 3 — Claude auto-assigns story ID:**
```
New story will be assigned ID: US-[max_story_id + 1]
```

Story is appended at end of the epic's story list (no position selection for stories).

**Claude stores:** `target_phase`, `target_epic`, `new_story_id`

---

## Validation Rules

### Position
- Must be a positive integer within valid range (1 to total_items + 1)
- Validated immediately after user input in STEP 2
- Out of range → show error with valid range, re-prompt (max 3 attempts, then abort)

### Content
- Phase: must have at least a name AND goal (both REQUIRED)
- Epic: must have at least a name (REQUIRED)
- Story: must have at least a title (REQUIRED)
- Missing required fields → Claude asks follow-up before proceeding

### Edit References
- Phase number must correspond to existing `phase-N.md` file
- Epic number must exist within the specified phase file
- US-XXX must exist in at least one phase file
- If story found in backlog but NOT in any phase file → error: "Orphaned story found. Run /project-status to investigate."
- Not found → Claude shows list of valid options:
```
US-[XXX] not found in any phase file. Available stories:
  US-001: User Registration (Phase 1, Epic 1)
  US-002: User Login (Phase 1, Epic 1)
  US-005: Create Product Listing (Phase 2, Epic 2)
  ...
```

---

**Version:** 3.0.0
**Last Updated:** 2026-04-02
