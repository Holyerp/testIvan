# Add Scope — Input Parsing Module

**Referenced by:** `add-scope.md` STEP 0 & STEP 2

---

## STEP 0: PARSE ARGUMENTS & VALIDATE

### 0.1 Parse Command Arguments

**Extract from user input in this order:**

**1. Action** (required): `add` or `edit`

If missing or invalid → ask:
```
Which action?
[1] Add — Add a new phase, epic, or story
[2] Edit — Modify an existing phase, epic, or story
```

**2. Scope type** (required): `phase`, `epic`, or `story`

If missing or invalid → ask:
```
What do you want to [add/edit]?
[1] Phase — A complete development phase
[2] Epic — A feature group within a phase
[3] Story — A single user story within an epic
```

**3. Position/Identifier** (context-dependent):

| Action + Scope | Required Arguments | Default |
|---|---|---|
| `add phase` | `[position]` | Append at end |
| `add epic` | `[phase-number]` + `[position]` | Append at end of phase |
| `add story` | `[phase-number]` + `[epic-number]` | Append, auto-assign US-XXX |
| `edit phase` | `[phase-number]` | — (must specify) |
| `edit epic` | `[phase-number]` + `[epic-number]` | — (must specify) |
| `edit story` | `[US-XXX]` | — (must specify) |

**4. --from flag** (optional): path to content file

If provided → validate file exists:
- File found → read content, store as `input_content`
- File NOT found → error and offer alternatives:
```
File not found: [path]

[1] Enter correct file path
[2] Describe the content manually
```

---

### 0.2 Validate Project State

**Check:** Do phase files exist in `.project-management/output/phases/`?

- **Yes** → continue
- **No** → abort:
```
No phase files found in .project-management/output/phases/
Run /init-project first to set up the project structure.
```

---

### 0.3 Gather Content (if no --from file)

**Prompt depends on action + scope type:**

#### Add Phase
```
Describe the new phase:

1. Phase name (e.g., "API Integration & External Services")
2. Phase goal/description
3. Estimated duration (e.g., "6 weeks", "2 months")
4. Epics and stories (describe features — I'll structure them)

Write as much or as little as you want. I'll fill in details using the project template.
```

#### Add Epic
```
Describe the new epic:

1. Epic name (e.g., "Notification System")
2. What this epic covers
3. User stories (or describe features — I'll create stories)
4. Priority (P0/P1/P2)

I'll generate acceptance criteria and story point estimates.
```

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

#### Edit Phase
```
What to change about Phase [N]: [Name]?

[1] Phase name/goal
[2] Duration/dates
[3] Status
[4] Add epic to this phase → redirects to: /add-scope add epic [N]
[5] Remove an epic from this phase
[6] Describe changes freely
```

#### Edit Epic
```
What to change about Epic [N]: [Name] in Phase [M]?

[1] Epic name/description
[2] Priority
[3] Status
[4] Dependencies
[5] Add story → redirects to: /add-scope add story [M] [N]
[6] Remove a story
[7] Describe changes freely
```

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

---

## STEP 2: DETERMINE PLACEMENT (add only)

**Skip entirely for `edit` actions.**

### 2.1 Phase Placement

**Display current phases and ask for position:**
```
PHASE PLACEMENT

Existing phases:
  [1] Phase 1: [Name] ([N] points, [Status])
  [2] Phase 2: [Name] ([N] points, [Status])
  [3] Phase 3: [Name] ([N] points, [Status])
  [4] Phase 4: [Name] ([N] points, [Status])
  [5] → Append at end (becomes Phase 5)

Insert at position (1-[N+1], default = append):
```

**Rules:**
- If position provided in command args → use directly (skip prompt)
- Position 1 = insert before Phase 1 (all shift right)
- Position N+1 = append (no renumbering needed)
- Position 2 to N = insert, shift everything from that position onward

**Store:** `insert_position`

### 2.2 Epic Placement

**Step 1 — Select target phase:**
```
EPIC PLACEMENT — Select Phase

[1] Phase 1: [Name]
[2] Phase 2: [Name]
[3] Phase 3: [Name]
[4] Phase 4: [Name]

Target phase:
```

**Step 2 — Select position within phase:**
```
EPIC PLACEMENT — Position in Phase [N]

Current epics:
  [1] Epic 1: [Name] ([N] points)
  [2] Epic 2: [Name] ([N] points)
  [3] → Append (becomes Epic 3)

Insert at position (default = append):
```

**Store:** `target_phase`, `insert_position`

### 2.3 Story Placement

**Step 1:** Select target phase (same prompt as 2.2 Step 1)

**Step 2 — Select target epic:**
```
STORY PLACEMENT — Select Epic in Phase [N]

[1] Epic 1: [Name] ([N] stories, [N] points)
[2] Epic 2: [Name] ([N] stories, [N] points)

Target epic:
```

**Step 3 — Auto-assign story ID:**
```
New story ID: US-[max_story_id + 1]
```

**Store:** `target_phase`, `target_epic`, `new_story_id`

---

## Validation Rules

### Position
- Must be a positive integer
- Must be ≤ total_items + 1 (append allowed, gaps not allowed)
- Out of range → show error with valid range, ask again

### Content
- Phase: must have at least a name and goal
- Epic: must have at least a name
- Story: must have at least a title
- Missing required fields → ask user to provide them

### Edit References
- Phase number must correspond to an existing `phase-N.md`
- Epic number must exist within the specified phase
- US-XXX must exist in at least one phase file or backlog
- Not found → show error with list of valid options:
```
US-[XXX] not found. Available stories:
  US-001: User Registration (Phase 1, Epic 1)
  US-002: User Login (Phase 1, Epic 1)
  ...
```

---

**Version:** 3.0.0
**Created:** 2026-04-01
