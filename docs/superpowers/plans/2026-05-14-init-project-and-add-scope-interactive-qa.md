# Interactive Q&A Extension to /init-project and /add-scope — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Extend the interactive `AskUserQuestion` Q&A pattern (currently active in `/process-client-docs` STEP 5) to `/init-project` (5 decision points + new post-generation gate) and `/add-scope` (3 decision points), with a backward-compatible `skippable` schema flag.

**Architecture:** Schema change first (one optional flag), then per-command integration. Each command edits a small set of existing module files — no new files needed. The reusable loop in `modules/interactive-clarifications.md` is the single consumer of `skippable`. Existing `/process-client-docs` continues unchanged (no `skippable` field in extracted questions → treated as `true`).

**Tech Stack:** Markdown command/module files only (no application code). Verification is via `.claude/hooks/audit-pm.sh`, file-size `wc -l`, and `grep` reference checks. There is NO running test suite for these files — verification is structural and behavioral (does the doc instruct Claude to do the right thing).

**Spec:** `docs/superpowers/specs/2026-05-14-init-project-and-add-scope-interactive-qa-design.md`

---

## File Map

| File | Action | Purpose |
|------|--------|---------|
| `.claude/commands/modules/interactive-clarifications.md` | Modify | Add `skippable` flag semantics (Task 1) |
| `.claude/commands/init-project.md` | Modify | STEP 0/1/2 → AskUserQuestion; new STEP 6 post-gen gate (Tasks 2, 5) |
| `.claude/commands/modules/init-project-stack-selection.md` | Modify | Custom flow → AskUserQuestion sequence with top-3 + Other (Task 3) |
| `.claude/commands/modules/init-project-i18n-setup.md` | Modify | Yes/No + iterative language loop (Task 4) |
| `.claude/commands/modules/add-scope-input-parsing.md` | Modify | §0.1 action + scope-type → AskUserQuestion (Task 6) |
| `.claude/commands/add-scope.md` | Modify | STEP 7 docs-update → AskUserQuestion + skip handling (Task 7) |
| `.claude/commands/add-scope-reference.md` | Modify | STEP 7 reference notes new skip semantics (Task 7) |
| `CHANGELOG.md` | Modify | Append to existing "Unreleased" section (Task 8) |

No new files. No template changes (`open-questions-template.md` already supports the entries produced by these commands).

---

## Task 1: Add `skippable` flag to interactive-clarifications module

**Files:**
- Modify: `.claude/commands/modules/interactive-clarifications.md`

**Goal:** Document the new optional `skippable: true|false` field on each question. Loop respects it in STEP C (omits Skip button when `false`).

- [ ] **Step 1: Update the Question Schema section**

Edit `.claude/commands/modules/interactive-clarifications.md`. Find the schema block under `## Question Schema (input)` (around line 14). Add the `skippable` line right after `priority`:

```yaml
- id: Q-001                     # sequential, zero-padded
  category: authentication      # 1–12 chars, kebab-case (used as header chip)
  priority: P0                  # P0 / P1 / P2
  skippable: true               # OPTIONAL — default true. Set false for gating
                                # questions where the command cannot proceed
                                # without a choice (project type, scope type, …).
  question: "Authentication strategy — JWT or cookie sessions?"
  default: "cookie sessions"    # recommended fallback if skipped
  ...
```

- [ ] **Step 2: Update the Constraints sub-list**

In the same Schema section, find the `Constraints:` bullet list (right after the YAML block). Append a new bullet:

```markdown
- `skippable` (optional, default `true`): when `false`, the loop omits the `Skip — answer later` option. Use only for questions that gate downstream work (e.g. project type). Default behavior (`true`) is the existing `/process-client-docs` behavior — Skip is always available.
```

- [ ] **Step 3: Update STEP C (loop description)**

Find the section `## STEP C — Loop: one AskUserQuestion call per question` (around line 70). Locate the AskUserQuestion call template. Replace it with:

```
AskUserQuestion({
  questions: [{
    question: "{{entry.question}}",
    header: "{{entry.category | truncate(12)}}",
    multiSelect: false,
    options: [
      ...entry.options,                              // up to 3
      ...(entry.skippable !== false                  // omit Skip if skippable === false
        ? [{
            label: "Skip — answer later",
            description: "Log this question to input/open-questions.md and continue. Resolve later with /resolve-questions."
          }]
        : [])
    ]
  }]
})
```

Then add this paragraph immediately below the call template:

```markdown
**Skip semantics:**

- When `entry.skippable` is omitted or `true` (default), the loop appends `Skip — answer later`. On skip, run STEP D (persist to `open-questions.md`).
- When `entry.skippable` is `false` (gating question), the loop does NOT append Skip. The user must select an option (or use AskUserQuestion's native `Other` for free-text). If the AskUserQuestion tool itself fails, fall back to `entry.default` and emit a warning to the STEP G summary — never block indefinitely.
```

- [ ] **Step 4: Verify the file is still under the budget**

Run: `wc -l .claude/commands/modules/interactive-clarifications.md`

Expected: under 200 lines (currently 166, this change adds ~15-20).

- [ ] **Step 5: Verify with audit hook**

Run: `bash .claude/hooks/audit-pm.sh 2>&1 | tail -30`

Expected: no 🔴 or 🟠 findings; only benign 🟡 entries if any (e.g. existing `extraction-quality-output.md` 311 lines).

- [ ] **Step 6: Commit**

```bash
git add .claude/commands/modules/interactive-clarifications.md
git commit -m "$(cat <<'EOF'
feat(clarifications): add optional `skippable` flag to question schema

When `skippable: false`, the interactive loop omits the
"Skip — answer later" option. Used for gating questions where the
command cannot proceed without a choice (project type, scope type).

Default remains `true` (backward compatible — existing
/process-client-docs questions without the flag continue to work
exactly as before).
EOF
)"
```

---

## Task 2: /init-project — convert STEP 0/1/2 to AskUserQuestion

**Files:**
- Modify: `.claude/commands/init-project.md` (STEP 0 and STEP 1 summaries — narrative menus → AskUserQuestion invocations)

**Goal:** The two top-level decisions (project type, stack approach) become `AskUserQuestion` calls with `skippable: false`. STEP 2 (i18n) handled in Task 4.

- [ ] **Step 1: Update STEP 0 (Project Structure Selection)**

Edit `.claude/commands/init-project.md`. Find `### STEP 0: PROJECT STRUCTURE SELECTION` (line 23). Replace the entire `**Summary:**` block (lines 27-41) with:

```markdown
**Summary:**

1. **Ask via AskUserQuestion** (gating, no Skip):

   ```
   question: "What project structure?"
   header: "Project type"
   skippable: false
   options:
     - label: "Backend + Mobile (Recommended)"
       description: "Monorepo with apps/backend + apps/mobile. Best for SaaS with iOS/Android client."
     - label: "Backend Only"
       description: "API-only service. No frontend code in this repo."
     - label: "Backend + Web + Mobile"
       description: "Full monorepo: apps/backend + apps/web + apps/mobile."
     - label: "Web Only"
       description: "Single web app (SPA / SSR). No backend in this repo."
   ```

2. Based on selection, create appropriate folder structure (see `modules/init-project-structure-setup.md` for monorepo scaffolding).

3. For monorepo selections (Backend + Mobile, Backend + Web + Mobile):
   - Create `apps/` directory with backend/mobile/web subdirectories
   - Create `packages/` for shared code (types, utils, api-client)
   - Setup pnpm workspace + Turborepo
   - Create package.json files for each app/package

4. Update `.gitignore` for monorepo structure.

**Output:** Project structure created, ready for tech stack configuration.
```

- [ ] **Step 2: Update STEP 1 (Tech Stack Selection summary)**

Find `### STEP 1: TECH STACK SELECTION` (line 45). Replace the `**Summary:**` block (lines 49-55, the lines starting from "1. Ask user to choose configuration method") with:

```markdown
**Summary:**

1. **Ask via AskUserQuestion** (gating, no Skip):

   ```
   question: "Tech stack approach?"
   header: "Stack"
   skippable: false
   options:
     - label: "Default (Recommended)"
       description: "React Router 7 + Prisma + Postgres + Zod. Battle-tested defaults from .claude/rules/stack-specific.md."
     - label: "AI suggests"
       description: "Claude proposes a stack based on input/scope.md and constraints.md. You approve or revise."
     - label: "Custom"
       description: "You pick each layer (backend / database / frontend / styling / testing / build / deploy)."
   ```

2. Based on selection, configure stack files (see `modules/init-project-stack-selection.md`). Custom flow invokes a sequence of layer-by-layer AskUserQuestion calls.
```

(Keep all subsequent narrative around STEP 1 unchanged.)

- [ ] **Step 3: Verify file size**

Run: `wc -l .claude/commands/init-project.md`

Expected: under 300 lines.

- [ ] **Step 4: Verify no other `[1] / [2] / [3]` style menus remain in STEP 0 or STEP 1**

Run: `grep -nE "^\s*-\s+\[[0-9]\]" .claude/commands/init-project.md | head -20`

Expected: zero matches in STEP 0 / STEP 1 sections (other narrative numbered lists may remain).

- [ ] **Step 5: Commit**

```bash
git add .claude/commands/init-project.md
git commit -m "$(cat <<'EOF'
feat(init-project): convert STEP 0 and STEP 1 to AskUserQuestion

Project type and stack approach are gating decisions — they get
`skippable: false` so the user must choose. Native UI buttons
replace the numbered narrative menu.

STEP 2 (i18n) is converted in a follow-up commit; the Custom stack
sequence likewise.
EOF
)"
```

---

## Task 3: /init-project — Custom stack flow → AskUserQuestion sequence

**Files:**
- Modify: `.claude/commands/modules/init-project-stack-selection.md`

**Goal:** When user picks "Custom" in STEP 1, run a sequence of `AskUserQuestion` calls (one per stack layer: backend, database, frontend, styling, testing, build, deploy). Each shows top-3 most-common options + AskUserQuestion's native `Other` for free-text. `skippable: true` — Skip = use default; free-text answers anonymized before being written to `input/technologies.md`.

- [ ] **Step 1: Locate the Custom flow section**

Run: `grep -n "Custom" .claude/commands/modules/init-project-stack-selection.md | head -10`

Expected: matches around lines 130–215 per spec exploration.

- [ ] **Step 2: Read the existing Custom flow**

Run: `sed -n '125,220p' .claude/commands/modules/init-project-stack-selection.md`

Identify the existing layer-by-layer numbered menus (backend, db, frontend, etc.). Note their current option lists.

- [ ] **Step 3: Replace each layer's narrative menu with an AskUserQuestion block**

For each layer (backend, database, frontend, styling, testing, build, deploy), replace the numbered menu with the following template. Use the existing option lists from the file to populate the top-3 in each block.

Template (instantiate per layer):

```markdown
#### Layer: <layer-name>

**Ask via AskUserQuestion** (`skippable: true` — Skip = use default):

```
question: "<Layer> choice?"
header: "<layer>"                    # 1–12 chars, kebab-case
skippable: true
default: "<recommended-option-label>"
options:
  - label: "<Top 1 option> (Recommended)"
    description: "<one-line rationale>"
  - label: "<Top 2 option>"
    description: "<one-line rationale>"
  - label: "<Top 3 option>"
    description: "<one-line rationale>"
applies_to: [ input/technologies.md ]
```

AskUserQuestion's native `Other` option lets the user type a custom value. On Skip, the loop uses `default:` and logs to `input/open-questions.md` per the standard module flow.

On any free-text `Other` answer, run through `.claude/rules/anonymization.md` §3–4 (defensive — stack names usually have no PII but the rule is the boundary). Write the chosen value into `input/technologies.md` under the corresponding `## <Layer>` heading.
```

Concretely (use these option lists — derived from the file's existing menus, picking the top 3 most common per layer):

| Layer | Top 1 (Recommended) | Top 2 | Top 3 |
|-------|---------------------|-------|-------|
| Backend | Node.js + Express | NestJS | Fastify |
| Database | PostgreSQL | MySQL | MongoDB |
| Frontend | React Router 7 | Next.js | Vite + React SPA |
| Styling | Tailwind CSS | CSS Modules | shadcn/ui (Tailwind preset) |
| Testing | Vitest + Playwright | Jest + Cypress | Vitest only (unit) |
| Build | Vite | Turbopack | Webpack |
| Deploy | Railway | Vercel | AWS |

For each of those 7 layers, emit one AskUserQuestion block in the file (replacing the existing numbered menu for that layer). If the existing file has a different layer breakdown, adapt to match (do NOT invent layers; mirror what's already there).

- [ ] **Step 4: Add a top-of-section note explaining the flow**

Above the first AskUserQuestion block, add:

```markdown
> **Custom stack flow:** the user answers a sequence of single-layer AskUserQuestion calls. Top-3 most-common options per layer cover ~90% of choices; AskUserQuestion's native `Other` handles the rest with free-text + anonymization. Skip on any layer = use the recommended default.
```

- [ ] **Step 5: Verify file size**

Run: `wc -l .claude/commands/modules/init-project-stack-selection.md`

Expected: under 300 lines (module budget per `.claude/rules/documentation.md` §2.1). Current count after edit should be in the 240–280 range.

- [ ] **Step 6: Verify no `[N]` style numbered options remain in Custom flow**

Run: `awk '/Custom/,/^---/' .claude/commands/modules/init-project-stack-selection.md | grep -cE "^\s*\[[0-9]\]"`

Expected: 0.

- [ ] **Step 7: Commit**

```bash
git add .claude/commands/modules/init-project-stack-selection.md
git commit -m "$(cat <<'EOF'
feat(init-project): convert Custom stack flow to AskUserQuestion

Each layer (backend / database / frontend / styling / testing / build /
deploy) now uses one AskUserQuestion call with top-3 most-common
options + native Other for free-text. Skip uses the recommended
default and logs to open-questions.md.

Free-text Other answers pass through the anonymization rule before
being persisted to input/technologies.md.
EOF
)"
```

---

## Task 4: /init-project — i18n flow with iterative language loop

**Files:**
- Modify: `.claude/commands/modules/init-project-i18n-setup.md`

**Goal:** Replace the binary Yes/No prompt and comma-separated language input with two AskUserQuestion patterns: (a) gating Yes/No for "enable i18n", (b) iterative "default language" + "add another?" loop.

- [ ] **Step 1: Read current file**

Run: `cat .claude/commands/modules/init-project-i18n-setup.md`

Note the existing Yes/No prompt (around lines 9-20) and the comma-separated language input (around lines 28-52).

- [ ] **Step 2: Replace the i18n Yes/No section**

Find the section that asks "Enable i18n?" (the binary prompt around lines 9-20). Replace with:

```markdown
## Step 1: Enable i18n?

**Ask via AskUserQuestion** (gating, no Skip):

```
question: "Enable internationalization (i18n)?"
header: "i18n"
skippable: false
options:
  - label: "No (Recommended)"
    description: "English only. Add i18n later if needed via /add-scope."
  - label: "Yes — multiple languages"
    description: "Set up i18next + locale files. You'll pick the default + additional languages next."
```

If "No" → exit this module, default to English only.
If "Yes" → proceed to Step 2.
```

- [ ] **Step 3: Replace the comma-separated language input with iterative loop**

Find the section that asks for language codes (around lines 28-52). Replace with:

```markdown
## Step 2: Default language

**Ask via AskUserQuestion** (`skippable: true` — Skip = English):

```
question: "Default (primary) language?"
header: "Language"
skippable: true
default: "English"
options:
  - label: "English (Recommended)"
    description: "Code: en. Most common default."
  - label: "Spanish"
    description: "Code: es."
  - label: "German"
    description: "Code: de."
applies_to: [ input/i18n.md ]
```

The user can pick the AskUserQuestion native `Other` to type any other language (free-text). The free-text passes through the anonymization rule (defensive) before being persisted with its ISO code derived from a lookup table in this file.

## Step 3: Additional languages (loop)

After the default is set, ask iteratively:

```
question: "Add another language?"
header: "More langs"
skippable: false
options:
  - label: "No — I'm done"
    description: "Finalize i18n config with the languages chosen so far."
  - label: "Yes — add another"
    description: "Pick another language to support."
```

If "Yes — add another" → fire the same AskUserQuestion as Step 2 (default language). Loop until user picks "No — I'm done".

The user can pick the AskUserQuestion native `Other` to type any non-listed language (free-text → anonymized → ISO code lookup → persisted).

## ISO code lookup table

Used to derive a language code from a free-text `Other` answer:

| Free-text | ISO 639-1 |
|-----------|-----------|
| English   | en |
| Spanish   | es |
| German    | de |
| French    | fr |
| Italian   | it |
| Portuguese| pt |
| Dutch     | nl |
| Polish    | pl |
| Russian   | ru |
| Serbian   | sr |
| Croatian  | hr |
| Slovenian | sl |
| Czech     | cs |
| Slovak    | sk |

If the free-text doesn't match the table, emit a warning to the STEP G summary ("Language '<x>' not recognized; using as-is, may need manual ISO code correction in input/i18n.md") and proceed.
```

- [ ] **Step 4: Verify file size**

Run: `wc -l .claude/commands/modules/init-project-i18n-setup.md`

Expected: ~80-100 lines (under 200 module budget).

- [ ] **Step 5: Verify with audit hook**

Run: `bash .claude/hooks/audit-pm.sh 2>&1 | grep -E "🔴|🟠"`

Expected: no output (no critical or high findings).

- [ ] **Step 6: Commit**

```bash
git add .claude/commands/modules/init-project-i18n-setup.md
git commit -m "$(cat <<'EOF'
feat(init-project): i18n setup uses AskUserQuestion + iterative loop

Yes/No gate for enabling i18n (no Skip). Default language picked
from top-3 + native Other. Additional languages added via an
iterative "Add another?" loop — replaces the prior comma-separated
text input. ISO code lookup table maps free-text Other answers.
EOF
)"
```

---

## Task 5: /init-project — new STEP 6 post-generation clarification gate

**Files:**
- Modify: `.claude/commands/init-project.md`

**Goal:** After STEP 5 (output generation), scan generated files for `<!-- TBD: Q-NNN -->` markers AND read existing P0/P1 entries from `input/open-questions.md`. If anything found, invoke `modules/interactive-clarifications.md` STEPS B–G with the combined list.

- [ ] **Step 1: Find the current end-of-flow location**

Run: `grep -n "^###\|^## " .claude/commands/init-project.md | head -20`

Identify the final STEP heading (likely STEP 5 or similar). The new STEP 6 will be inserted immediately after it.

- [ ] **Step 2: Insert STEP 6 section**

Locate the line immediately after the last existing STEP section (before `---` or end-of-document). Insert this new section:

```markdown
### STEP 6: POST-GENERATION CLARIFICATION GATE

**📖 See:** `modules/interactive-clarifications.md` for the full loop (STEPS B–G — AskUserQuestion call shape, skip handling, anonymized free-text, answer-application to artefacts).

After STEP 5 produces the documentation set (`output/docs/prd.md`, `technical-spec.md`, `architecture.md`), this step runs the same interactive Q&A gate used by `/process-client-docs` STEP 5.

**Sources of questions:**

1. **TBD markers** in generated docs — grep the generated files for `<!-- TBD: Q-NNN -->`. Each marker references a question by ID. If the corresponding question is not already in `input/open-questions.md`, leave the marker in place (the doc indicates an open ambiguity to resolve manually). If the question IS in `open-questions.md`, include it in the loop.

2. **Existing P0/P1 entries in `input/open-questions.md`** — read the file (created earlier by `/process-client-docs` or prior `/init-project` runs). Filter `Status: Open` entries with `priority: P0` or `P1`.

**Behavior:**

- If both sources yield zero questions → emit `✅ No open clarifications.` and skip the loop.
- Otherwise, build the question list (priority-sorted P0 → P1) and invoke `modules/interactive-clarifications.md` STEPS B–G.
- Skipped questions remain in `open-questions.md` with incremented `Skipped:` count.
- Answered questions update `applies_to` artefacts AND move to the Resolved section of `open-questions.md`.

**Resume later:** the user can run `/resolve-questions` at any time to revisit still-Open entries.

---
```

- [ ] **Step 3: Verify the new section is recognized in the module-references table**

Find the `## 📚 Module References` table in the same file. Confirm that `modules/interactive-clarifications.md` is listed. If not, add this row:

```markdown
| `modules/interactive-clarifications.md` | STEP 6 — post-generation clarification gate (reusable across PM commands) |
```

If the table doesn't exist (depending on current file shape), skip this step and add a `**Related:**` bullet at the bottom of STEP 6 pointing to the module.

- [ ] **Step 4: Verify file size**

Run: `wc -l .claude/commands/init-project.md`

Expected: under 300 lines.

- [ ] **Step 5: Verify reference resolves**

Run: `grep -nl "interactive-clarifications" .claude/commands/init-project.md`

Expected: returns the file path (reference present).

- [ ] **Step 6: Run audit hook**

Run: `bash .claude/hooks/audit-pm.sh 2>&1 | tail -25`

Expected: section D shows `✅ No broken internal markdown links`.

- [ ] **Step 7: Commit**

```bash
git add .claude/commands/init-project.md
git commit -m "$(cat <<'EOF'
feat(init-project): add STEP 6 post-generation clarification gate

After generating PRD / technical-spec / architecture, scan for
<!-- TBD: Q-NNN --> markers and read open P0/P1 entries from
input/open-questions.md. If found, invoke the interactive Q&A
loop from modules/interactive-clarifications.md.

Mirrors the /process-client-docs STEP 5 pattern. Resume later via
/resolve-questions.
EOF
)"
```

---

## Task 6: /add-scope — convert §0.1 action + scope-type to AskUserQuestion

**Files:**
- Modify: `.claude/commands/modules/add-scope-input-parsing.md`

**Goal:** Replace the two narrative numbered menus in §0.1 (Action and Scope type) with `AskUserQuestion` calls, both `skippable: false` (gating).

- [ ] **Step 1: Locate the existing menus**

Run: `sed -n '10,30p' .claude/commands/modules/add-scope-input-parsing.md`

Confirms the two menus at lines 14-20 (Action) and 22-29 (Scope type).

- [ ] **Step 2: Replace the Action prompt block**

Find the block that begins at `**1. Action** (required): \`add\` or \`edit\`.` (around line 14). Replace lines 14-20 with:

```markdown
**1. Action** (required): `add` or `edit`.

If missing/invalid, ask via AskUserQuestion (gating, no Skip):

```
question: "What action?"
header: "Action"
skippable: false
options:
  - label: "Add new (Recommended)"
    description: "Add a new phase, epic, or story."
  - label: "Edit existing"
    description: "Modify an existing phase, epic, or story."
```
```

- [ ] **Step 3: Replace the Scope type prompt block**

Find the block at `**2. Scope type** (required): \`phase\`, \`epic\`, or \`story\`.` (around line 22). Replace lines 22-29 with:

```markdown
**2. Scope type** (required): `phase`, `epic`, or `story`.

If missing/invalid, ask via AskUserQuestion (gating, no Skip):

```
question: "What type of scope to {{action}}?"
header: "Scope"
skippable: false
options:
  - label: "Story (Recommended)"
    description: "A single user story within an epic."
  - label: "Phase"
    description: "A complete development phase."
  - label: "Epic"
    description: "A feature group within a phase."
```
```

(Note: `{{action}}` resolves to "add" or "edit" based on the answer from §0.1 step 1.)

- [ ] **Step 4: Verify the table and rest of the file remain intact**

Run: `grep -n "Position / Identifier" .claude/commands/modules/add-scope-input-parsing.md`

Expected: still matches (the dynamic position/identifier table that follows is unchanged — it's not a candidate for AskUserQuestion per the spec).

- [ ] **Step 5: Verify file size**

Run: `wc -l .claude/commands/modules/add-scope-input-parsing.md`

Expected: under 200 lines (module budget).

- [ ] **Step 6: Commit**

```bash
git add .claude/commands/modules/add-scope-input-parsing.md
git commit -m "$(cat <<'EOF'
feat(add-scope): convert action + scope-type prompts to AskUserQuestion

Both are gating decisions (`skippable: false`) — the command cannot
proceed without them. Native UI buttons replace the numbered menus.

Position / target-phase / target-epic remain free-text numeric input
because their option counts are dynamic (1..N) and exceed the
4-option ceiling. Content intake (title, name, criteria) likewise
stays narrative — AskUserQuestion is the wrong tool for prose.
EOF
)"
```

---

## Task 7: /add-scope — STEP 7 docs-cascade with skip handling

**Files:**
- Modify: `.claude/commands/add-scope.md`
- Modify: `.claude/commands/add-scope-reference.md`

**Goal:** Replace the binary "Update docs now?" prompt at STEP 7 with an `AskUserQuestion` call (`skippable: true`). On skip, append a new entry to `input/open-questions.md` with `category: docs-cascade`, `priority: P2`.

- [ ] **Step 1: Locate STEP 7 in add-scope.md**

Run: `sed -n '148,165p' .claude/commands/add-scope.md`

Confirms STEP 7 at line 151-154 with the binary Yes/No prompt.

- [ ] **Step 2: Replace STEP 7 with AskUserQuestion + skip-handling**

Replace lines 151-154 with:

```markdown
### STEP 7: Documentation Update

**Ask via AskUserQuestion** (`skippable: true` — Skip logs as deferred decision):

```
question: "Update PRD, technical spec, and architecture docs now?"
header: "Docs"
skippable: true
default: "Yes — update now"
options:
  - label: "Yes — update now (Recommended)"
    description: "Invoke /generate-docs immediately to keep docs in sync with the scope change."
  - label: "No — I'll cascade later"
    description: "Skip the docs update for now. Run /generate-docs manually before the next sprint review."
```

**Skip handling:** if the user picks `Skip — answer later`, append a new entry to `input/open-questions.md` (created if missing, from `.project-management/templates/open-questions-template.md`):

```yaml
id: Q-NNN                 # sequential after existing entries — single Q-NNN namespace
category: docs-cascade
priority: P2
status: Open
question: "Update PRD, technical spec, and architecture docs for {{scope_change_summary}}?"
default: "Yes (auto-cascade)"
impact: "Documentation drift — docs reference older scope until /generate-docs runs"
applies_to:
  - output/docs/prd.md
  - output/docs/technical-spec.md
  - output/docs/architecture.md
notes: "{{action}} {{scope_type}} {{identifier}} on {{date}}; documentation not yet updated"
```

The user can resume via `/resolve-questions --priority P2` or `/resolve-questions Q-NNN`, or run `/generate-docs` directly when ready.
```

- [ ] **Step 3: Update add-scope-reference.md**

Read: `cat .claude/commands/add-scope-reference.md`

Find any block that documents the old STEP 7 / "Documentation Update" prompt. Replace the relevant lines with a short reference:

```markdown
### STEP 7 — Documentation Update

Uses AskUserQuestion with three outcomes:
- **Yes — update now (Recommended)** → invoke `/generate-docs` immediately.
- **No — I'll cascade later** → skip the update; user runs `/generate-docs` manually.
- **Skip — answer later** → log a `docs-cascade` P2 entry in `input/open-questions.md` (resume via `/resolve-questions`).

Full template definition lives in `add-scope.md` STEP 7. The skip entry follows the schema from `.project-management/templates/open-questions-template.md`.
```

If `add-scope-reference.md` doesn't have an existing STEP 7 section, append the block above before the final `**Status:**` or end-of-file marker.

- [ ] **Step 4: Verify file sizes**

Run: `wc -l .claude/commands/add-scope.md .claude/commands/add-scope-reference.md`

Expected:
- `add-scope.md`: under 300 lines
- `add-scope-reference.md`: under 300 lines (template budget)

- [ ] **Step 5: Verify references**

Run:
```bash
grep -n "open-questions.md\|docs-cascade\|resolve-questions" .claude/commands/add-scope.md .claude/commands/add-scope-reference.md
```

Expected: matches in both files showing the new skip semantics.

- [ ] **Step 6: Run audit hook**

Run: `bash .claude/hooks/audit-pm.sh 2>&1 | tail -25`

Expected: no 🔴 / 🟠 findings.

- [ ] **Step 7: Commit**

```bash
git add .claude/commands/add-scope.md .claude/commands/add-scope-reference.md
git commit -m "$(cat <<'EOF'
feat(add-scope): STEP 7 docs-cascade uses AskUserQuestion with skip

Three outcomes: Yes (run /generate-docs now), No (manual later),
or Skip (log a P2 docs-cascade entry to input/open-questions.md).
Skip semantics persist the deferred decision so it surfaces in
/resolve-questions later.

Per the spec, this is the only post-creation clarification in
/add-scope — content intake and dynamic-bound menus stay narrative.
EOF
)"
```

---

## Task 8: Update CHANGELOG

**Files:**
- Modify: `CHANGELOG.md`

**Goal:** Append entries to the existing `[Unreleased]` section describing the /init-project and /add-scope changes.

- [ ] **Step 1: Read current Unreleased section**

Run: `sed -n '10,30p' CHANGELOG.md`

Confirms the existing `[Unreleased]` block from the prior commit (`232f0dd`).

- [ ] **Step 2: Append new entries**

Find the existing `### Added` block under `## [Unreleased]`. Append these bullets at the end of the `### Added` list:

```markdown
- **`/init-project` STEP 0/1/2 → AskUserQuestion** — Project type, stack approach, i18n yes/no are now gating decisions (`skippable: false`) with native UI buttons replacing the numbered narrative menus.
- **`/init-project` Custom stack flow → AskUserQuestion sequence** — Each layer (backend / database / frontend / styling / testing / build / deploy) asks one AskUserQuestion with top-3 most-common options + native Other for free-text. Skip = use recommended default; free-text passes through the anonymization rule.
- **`/init-project` STEP 6 — post-generation clarification gate** — After PRD / technical-spec / architecture are generated, scan for `<!-- TBD: Q-NNN -->` markers AND read open P0/P1 entries in `input/open-questions.md`. If found, invoke the interactive Q&A loop. Mirrors `/process-client-docs` STEP 5.
- **`/init-project` i18n iterative language loop** — Replaces the prior comma-separated text input with AskUserQuestion + "Add another?" loop. ISO code lookup table maps free-text answers.
- **`/add-scope` action + scope-type → AskUserQuestion** — Two gating decisions (`skippable: false`). Position / target-phase / target-epic remain free-text numeric (dynamic bounds); content intake stays narrative (AskUserQuestion wrong tool for prose).
- **`/add-scope` STEP 7 docs-cascade → AskUserQuestion** — Three outcomes: Yes (run /generate-docs now), No (manual later), Skip (log P2 docs-cascade entry to `input/open-questions.md`).

### Schema

- **`skippable` flag added to question schema** in `modules/interactive-clarifications.md`. Optional, default `true`. Set `false` for gating questions where the command cannot proceed without a choice. Existing `/process-client-docs` questions without the flag continue to work unchanged.
```

- [ ] **Step 3: Verify CHANGELOG format**

Run: `sed -n '10,45p' CHANGELOG.md`

Expected: Unreleased section now contains both the prior `/process-client-docs` entries AND the new /init-project + /add-scope entries.

- [ ] **Step 4: Commit**

```bash
git add CHANGELOG.md
git commit -m "$(cat <<'EOF'
docs(changelog): /init-project + /add-scope interactive Q&A entries

Adds Unreleased entries for:
- /init-project STEP 0/1/2 + STEP 6 post-gen gate + Custom stack
  sequence + i18n iterative loop
- /add-scope action/scope-type + STEP 7 docs-cascade
- Schema: `skippable` flag (backward compatible)
EOF
)"
```

---

## Task 9: End-to-end verification

**Files:** None modified. Verification-only task.

**Goal:** Run the spec's verification checklist (§Verification) and document the result.

- [ ] **Step 1: File-size audit**

Run:
```bash
wc -l \
  .claude/commands/modules/interactive-clarifications.md \
  .claude/commands/init-project.md \
  .claude/commands/modules/init-project-stack-selection.md \
  .claude/commands/modules/init-project-i18n-setup.md \
  .claude/commands/modules/add-scope-input-parsing.md \
  .claude/commands/add-scope.md \
  .claude/commands/add-scope-reference.md
```

Expected: each file under its respective budget (modules ≤ 200 / commands ≤ 300 / references ≤ 300 per `documentation.md` §2.1). Note benign 🟡 ideal warnings if any.

- [ ] **Step 2: Audit hook**

Run: `bash .claude/hooks/audit-pm.sh 2>&1 | tee /tmp/audit-result.txt`

Expected: zero 🔴 (critical) and zero 🟠 (high) findings. Capture 🟡 findings to /tmp/audit-result.txt for review (benign).

- [ ] **Step 3: Cross-reference integrity check**

Run:
```bash
for ref in "skippable" "interactive-clarifications" "open-questions.md" "docs-cascade" "resolve-questions"; do
  echo "--- $ref ---"
  grep -rln "$ref" --include="*.md" .claude/ .project-management/templates/ CLAUDE.md CHANGELOG.md 2>/dev/null
done
```

Expected: every term resolves to files; no dangling mentions.

- [ ] **Step 4: Schema backward-compat dry-run check**

Run:
```bash
grep -A 5 "id: Q-001" .claude/commands/modules/extraction-by-section.md .claude/commands/modules/extraction-quality-output.md 2>/dev/null | head -20
```

Expected: the existing `/process-client-docs` schema examples do NOT contain `skippable:` — confirming backward compatibility (those questions default to `skippable: true`).

- [ ] **Step 5: Verify newly emitted question YAML blocks are well-formed**

Run: `grep -nE "^\s*skippable:\s*(true|false)" .claude/commands/ -r --include="*.md"`

Expected: every `skippable:` line uses literal `true` or `false` (no smart-quotes, no typos). Count should match the number of YAML question blocks introduced across Tasks 2/3/4/5/6/7.

- [ ] **Step 6: Smoke-test commit log readability**

Run: `git log --oneline -15`

Expected: clear sequence of 8 commits (one per task) with conventional commit prefixes (`feat(...):` or `docs(...):`), no AI attribution.

- [ ] **Step 7: Write a verification summary at the end of this plan**

Append to this plan file:

```markdown
## Verification Results (filled at execution time)

- File-size audit: ✅ / ⚠️ <details>
- Audit hook: ✅ / 🟡 <count> ideal warnings / 🟠 <details>
- Cross-reference integrity: ✅ / ⚠️ <details>
- Backward-compat dry-run: ✅ / ⚠️ <details>
- YAML well-formedness: ✅ / ⚠️ <details>
- Commit log readability: ✅ / ⚠️ <details>

**Verdict:** READY-TO-MERGE / NEEDS-FIXES
```

- [ ] **Step 8: Commit the verification summary**

```bash
git add docs/superpowers/plans/2026-05-14-init-project-and-add-scope-interactive-qa.md
git commit -m "$(cat <<'EOF'
docs(plan): record verification results for interactive Q&A extension

Closes the implementation plan with concrete pass/fail results
from the spec's verification checklist.
EOF
)"
```

---

## Self-Review Notes

- **Spec coverage:** All 4 design sections covered. Section 1 (Schema) → Task 1. Section 2 (/init-project, 5 DPs + STEP 6) → Tasks 2, 3, 4, 5. Section 3 (/add-scope, 3 DPs) → Tasks 6, 7. Section 4 (Verification) → Task 9. CHANGELOG → Task 8.
- **No placeholders:** Every code/markdown block is the literal content the engineer will paste. No "TBD", "TODO", or "similar to Task N" patterns. Top-3 stack option lists in Task 3 are concrete.
- **Type consistency:** `skippable` flag spelled identically across all tasks. `Q-NNN` namespace used consistently (no `Q-IP-`, `Q-AS-` variants — that decision was locked in during spec self-review). `applies_to` field consistent across all YAML blocks. Category names use kebab-case throughout (`project-type`, `docs-cascade`, `i18n`).
- **Backward compatibility:** Task 1 documents that absent `skippable` → defaults to `true`. Task 9 Step 4 explicitly verifies this via dry-run on existing `/process-client-docs` schema.
- **Sequencing:** Task 1 must come first (schema is the foundation). Tasks 2-5 (/init-project) can be done in order. Tasks 6-7 (/add-scope) are independent of /init-project. Task 8 (CHANGELOG) after 1-7. Task 9 (verification) last.
