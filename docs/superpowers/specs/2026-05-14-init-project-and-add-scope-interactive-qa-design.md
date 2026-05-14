# Interactive Q&A Pattern — Extension to `/init-project`, `/add-scope`, `/add-bug`

**Date:** 2026-05-14
**Status:** Approved (brainstorming complete; `/add-bug` scope added post-sign-off, plan updated)
**Builds on:** commit `232f0dd` (feat: interactive clarification gate for /process-client-docs)

---

## Context

The interactive Q&A pattern is active in `/process-client-docs` STEP 5: each extracted clarification is asked via `AskUserQuestion` with options + a `Skip — answer later` button. Skipped questions land in `.project-management/input/open-questions.md` and resume via `/resolve-questions`. The reusable loop is documented in `.claude/commands/modules/interactive-clarifications.md`.

The user wants the same pattern in `/init-project` and `/add-scope`. Exploration revealed each command has a different shape:

- **`/init-project`** — 5 upfront decision points (project type, stack approach, custom-stack sub-questions, i18n yes/no, languages). 4/5 are natural AskUserQuestion fits. The 5th (custom stack) needs decomposition. Already has anonymization phase but no clarification gate.
- **`/add-scope`** — 12 decision points but only 3 fit the pattern (action, scope-type, "update docs now?"). The rest are mandatory free-text content intake (titles, names, criteria) or dynamic menus (1..N positions, 1..M epics) where Skip would break the workflow.

This spec defines a minimal, focused extension that respects each command's shape rather than forcing a uniform pattern.

## Goals

1. **`/init-project`** — Replace narrative numbered menus in STEP 0/1/2 with `AskUserQuestion` for the 5 decision points. Add a post-generation clarification gate (new STEP 6) analogous to `/process-client-docs` STEP 5.
2. **`/add-scope`** — Convert the 3 fit-for-pattern decisions (action, scope-type, docs-cascade) to `AskUserQuestion`. Leave content intake and dynamic menus untouched.
3. **`/add-bug`** — Convert 3 enum-style decisions to `AskUserQuestion`: **Severity** (gating, 4 options Critical/High/Medium/Low), **Story Points** (deferable, top-3 Fibonacci + Other for 2/8/13), and **STEP 4 Phase Assignment** (Yes/No + dynamic phase-pick when ≤ 4 phases, numeric fallback otherwise). Free-text intake (title, description, reproduction steps) stays narrative.
4. **Schema evolution** — Add one optional `skippable` flag to the question schema in `modules/interactive-clarifications.md`. Backward compatible: existing `/process-client-docs` questions without the flag default to `skippable: true`.

## Non-Goals

- Expanding to `/execute-work` or `/add-bug` (deferred — next round once these two prove value).
- Extracting an `interactive-input-collector` abstraction layer (YAGNI — three commands, not thirty).
- Refactoring `/add-scope` edit-mode submenus (6–8 options each, exceeds the 4-option ceiling).
- Replacing free-text content intake in `/add-scope` (title, name, acceptance criteria) — `AskUserQuestion` is the wrong tool for open prose.

---

## Schema Change

`modules/interactive-clarifications.md` gains one optional field on each question entry:

```yaml
- id: Q-NNN              # single Q-NNN namespace; `category` carries source-domain info
  category: project-type
  priority: P0
  skippable: false        # NEW — default true for backward compat
  question: "Project type?"
  default: "Backend + Mobile"
  options: [ ... ]
```

**Loop behavior** (STEP C of the module):

- `skippable: true` (default, unchanged) — Loop appends `Skip — answer later` as 4th option. On skip, persist to `input/open-questions.md`. This is the existing `/process-client-docs` behavior.
- `skippable: false` — Loop does NOT append Skip. AskUserQuestion's native `Other` (free-text) remains available. User must select before the command proceeds. Used for gating questions where a default would break the rest of the flow.

Existing `/process-client-docs` extraction schema does not emit `skippable`; questions without it are treated as `true`. No migration of existing data required.

---

## `/init-project` Integration

### Decision points (5)

| ID | Location | Question | `skippable` | Options |
|----|----------|----------|-------------|---------|
| IP-0.1 | `init-project.md` STEP 0 | "Project type?" | `false` | 4: Backend Only / **Backend + Mobile** *(Recommended)* / Backend + Web + Mobile / Web Only |
| IP-1.1 | `init-project.md` STEP 1 | "Tech stack approach?" | `false` | 3: **Default** *(Recommended)* / AI suggests / Custom |
| IP-1.2a–g | `init-project-stack-selection.md` (only if IP-1.1 = Custom) | One question per layer: backend / database / frontend / styling / testing / build / deploy | `true` (skip = use default) | Top 3 most-popular options + AskUserQuestion's native `Other` |
| IP-2.1 | `init-project.md` STEP 2 | "Enable i18n?" | `false` | 2: Yes / **No** *(Recommended)* |
| IP-2.2 | `init-project.md` STEP 2 (only if IP-2.1 = Yes) | "Default language?" + iterative "Add another?" loop | `true` | Top 3 languages + `Other` |

### Multi-language flow (IP-2.2)

Replaces the current comma-separated text input. After selecting the default language, the loop asks `"Add another language?"` (Yes / No / Done). If Yes, fire IP-2.2 again with the same options. If No or Done, exit. This is slower per language than text input, but every interaction is a clear click — no parsing of free-form strings.

### Custom stack flow (IP-1.2)

Currently a sequence of narrative menus with 5–7 options each. The new flow shows the top 3 most-common choices per layer + AskUserQuestion's native `Other` for free-text. Free-text values pass through `.claude/rules/anonymization.md` (defensive — stack names shouldn't contain PII but the rule is the boundary). The chosen value lands in `input/technologies.md`.

**Coverage assumption:** the top 3 cover ~90% of choices per layer. Power users who want something exotic use `Other`. If feedback indicates the top 3 is wrong for a given layer, the schema entry is updated — no code change needed.

### New STEP 6 — Post-generation clarification gate

After STEP 5 (output generation), `/init-project` scans the generated files (`output/docs/prd.md`, `technical-spec.md`, `architecture.md`) for `<!-- TBD: Q-NNN -->` markers AND reads existing P0/P1 entries from `input/open-questions.md`. If anything found, invoke `modules/interactive-clarifications.md` STEPS B–G with that combined list.

This mirrors the `/process-client-docs` STEP 5 pattern. The user sees what's already captured before answering, so the questions are informed by the generated artefacts.

### Files modified

- `.claude/commands/init-project.md` — STEP 0/1/2 narrative menus → AskUserQuestion calls; new STEP 6 (~50 lines net change)
- `.claude/commands/modules/init-project-stack-selection.md` — Custom flow becomes a sequence of AskUserQuestion calls (~40 lines)
- `.claude/commands/modules/init-project-i18n-setup.md` — Yes/No + iterative language loop (~30 lines)

---

## `/add-scope` Integration

### Decision points (3)

| ID | Location | Question | `skippable` | Options |
|----|----------|----------|-------------|---------|
| AS-1 | `add-scope-input-parsing.md` §0.1 | "What action?" | `false` | 2: **Add new** *(Recommended)* / Edit existing |
| AS-2 | `add-scope-input-parsing.md` §0.1 | "What type of scope?" | `false` | 3: Phase / Epic / **Story** *(Recommended)* |
| AS-3 | `add-scope.md` STEP 7 | "Update PRD, technical spec, and architecture docs now?" | `true` (deferable) | 2: **Yes — update now** *(Recommended)* / No — I'll cascade later |

### Out of scope for `/add-scope`

- **Position / target phase / target epic** (DP-3/4/5) — dynamic bounds (1..N), free-text numeric stays.
- **Content intake** (DP-6/7/8) — phase name, epic name, story title, acceptance criteria — narrative prose, AskUserQuestion is the wrong tool.
- **Edit-mode submenus** (DP-9/10/11) — 6–8 options + multi-select; exceeds the ceiling. Stay narrative.
- **Renumbering / readme update** — internal logic, no user surface.

### AS-3 skip handling (docs-cascade)

When the user skips "Update docs now?", append to `input/open-questions.md`:

```yaml
id: Q-NNN                 # sequential after existing entries — single Q-NNN namespace across all commands
category: docs-cascade
priority: P2
question: "Update PRD, technical spec, and architecture docs for US-XXX?"
default: "Yes (auto-cascade)"
impact: "Documentation drift — docs reference older scope until /generate-docs runs"
applies_to:
  - output/docs/prd.md
  - output/docs/technical-spec.md
  - output/docs/architecture.md
notes: "Story US-XXX [added | edited]; documentation not yet updated"
```

Resume via `/resolve-questions Q-NNN` (specific entry) or `/generate-docs` directly.

**Question ID namespace** (clarification, applies to all commands): `Q-NNN` is a single sequential namespace across the repo's `input/open-questions.md`. The previous `/process-client-docs` exploration suggested namespaced IDs (`Q-IP-001`, `Q-AS-NNN`); for this round we keep one flat namespace to match the existing template (`Q-001`, `Q-002`, …). The `category:` field carries the source-domain information instead (`project-type`, `docs-cascade`, `email-provider`, …).

### Files modified

- `.claude/commands/modules/add-scope-input-parsing.md` — §0.1 narrative menus → two AskUserQuestion calls (~20 lines)
- `.claude/commands/add-scope.md` — STEP 7 binary prompt → AskUserQuestion + skip-handling for docs-cascade (~10 lines)
- `.claude/commands/add-scope-reference.md` — STEP 7 template / quick reference notes the new skip semantics (~5 lines)

---

## `/add-bug` Integration

### Decision points (3)

| ID | Location | Question | `skippable` | Options |
|----|----------|----------|-------------|---------|
| AB-1 | `add-bug.md` STEP 1 Q2 (Severity) | "Bug severity?" | `false` | 4: Critical / High / Medium / Low |
| AB-2 | `add-bug.md` STEP 1 Q8 (Story Points) | "Story points estimate?" | `true` | Top 3: **1** *(Trivial — recommended for Low)* / 3 / 5 + native `Other` for 2 / 8 / 13 |
| AB-3 | `add-bug.md` STEP 4 (Phase Assignment) | "Assign this bug to a phase now?" | `true` | 2: Yes — assign / **No — keep in Backlog** *(Recommended)*. If "Yes" → second AskUserQuestion for phase pick (≤ 4 phases) or numeric fallback (> 4). |

### Out of scope for `/add-bug`

- **Title, Affected Component, Description, Reproduction Steps, Expected/Actual Behavior, Additional Notes** — narrative free-text intake, AskUserQuestion is the wrong tool. Same principle as `/add-scope` content intake (DP-6/7/8).
- **Bug ID assignment, severity-section routing, summary update** (STEP 2/3/5) — internal logic, no user surface.

### AB-2 (Story Points) free-text validation

When user picks `Other` and types a value, validate it is a Fibonacci value (`1, 2, 3, 5, 8, 13`). If not, round up to the next Fibonacci value and emit a warning to the STEP 5 summary (`"Non-Fibonacci value '<x>' rounded to nearest: <y>"`). This mirrors the existing behavior in `/add-scope` for story-point fields.

### AB-3 (Phase Assignment) skip handling

When the user skips, append to `input/open-questions.md`:

```yaml
id: Q-NNN
category: bug-triage
priority: P2
status: Open
question: "Assign BUG-XXX ({{bug_title}}, severity {{severity}}) to a phase?"
default: "Backlog (no phase)"
impact: "Bug remains in Backlog until triaged; not scheduled into any phase"
applies_to:
  - output/bugs/bug-roadmap.md
notes: "Created on {{date}}; severity {{severity}}"
```

Resume via `/resolve-questions --priority P2` (or specific `Q-NNN`).

### AB-3 dynamic phase-pick rationale

Typical projects have 1–4 phases (Foundation / Core / Advanced / Polish per the default templates). For those, AskUserQuestion fits cleanly with one option per phase. For larger projects with > 4 phases, the 4-option ceiling forces a numeric-input fallback — the command discovers `phase-*.md` files at runtime and renders a numbered menu. The user sees a consistent click-based UX in the common case and a fallback only when needed.

### Files modified

- `.claude/commands/add-bug.md` — STEP 1 Severity + Story Points narrative menus → AskUserQuestion (~30 lines); STEP 4 Phase Assignment → AskUserQuestion + skip + dynamic phase-pick (~40 lines)
- `.claude/commands/add-bug-reference.md` — STEP 4 reference notes new patterns (~10 lines)

---

## All Files Affected

**Modified (no new files):**

| Path | Change |
|------|--------|
| `.claude/commands/modules/interactive-clarifications.md` | Document `skippable` flag semantics in STEP C; update schema example (~15 lines) |
| `.claude/commands/init-project.md` | STEP 0/1/2 menus → AskUserQuestion; new STEP 6 (~50 lines) |
| `.claude/commands/modules/init-project-stack-selection.md` | Custom flow → AskUserQuestion sequence with top-3 + Other (~40 lines) |
| `.claude/commands/modules/init-project-i18n-setup.md` | Yes/No + iterative language loop (~30 lines) |
| `.claude/commands/modules/add-scope-input-parsing.md` | §0.1 → AskUserQuestion (~20 lines) |
| `.claude/commands/add-scope.md` | STEP 7 → AskUserQuestion + AS-3 skip (~10 lines) |
| `.claude/commands/add-scope-reference.md` | STEP 7 reference update (~5 lines) |
| `.claude/commands/add-bug.md` | STEP 1 Severity + Story Points → AskUserQuestion; STEP 4 Phase Assignment → AskUserQuestion + skip + dynamic phase-pick (~70 lines) |
| `.claude/commands/add-bug-reference.md` | STEP 4 reference update (~10 lines) |
| `CHANGELOG.md` | New "Unreleased" entry (~15 lines) |

---

## Verification

End-to-end checks after implementation:

1. **`/init-project` smoke** — run in an empty test directory. Click through IP-0.1/1.1/2.1/2.2. Output structure matches the chosen options.
2. **Custom stack flow** — IP-1.1 = Custom. For each sub-question, pick one native option and one `Other` with free-text. `input/technologies.md` reflects both choices; free-text values are anonymized (defensive pass).
3. **Multi-language loop** — IP-2.1 = Yes → default English → Add another? Yes → German → Add another? Yes → Other "Slovenian" → Add another? No. `input/i18n.md` (or equivalent) contains `[en, de, sl]`.
4. **Post-generation gate** — manually insert `<!-- TBD: Q-X -->` into `output/docs/prd.md` (or pre-stage an entry in `input/open-questions.md`). STEP 6 detects and asks.
5. **`/add-scope` AS-1/AS-2** — UI shows 2 buttons for action, 3 for scope-type. No Skip button (gating). Invalid input reprompts.
6. **`/add-scope` AS-3 skip** — finish a story add → click Skip on "Update docs?". `input/open-questions.md` contains a new entry with `category: docs-cascade`, `priority: P2`, `applies_to: [3 doc paths]`.
7. **`/resolve-questions` on AS-3 entry** — `/resolve-questions --priority P2` surfaces the docs-cascade question. Answer Yes → entry moves to Resolved.
8. **Audit hook** — `bash .claude/hooks/audit-pm.sh` must be green: no broken refs, no file-size violations, how-to-use coverage intact.
9. **Backward compat** — run `/process-client-docs` on an existing brief. STEP 5 still works exactly as before (no `skippable` field in extracted questions → treated as `true`).

---

## Risks and Mitigations

| Risk | Mitigation |
|------|------------|
| AskUserQuestion has no native multi-select + free-text combo, so multi-language UX becomes an iterative "Add another?" loop | Documented in how-to-use; each click is unambiguous (preferred over parsing comma-separated input) |
| Top-3 + Other coverage may be wrong for a given stack layer | Schema entries are config, not code — adjust based on usage |
| `skippable: false` could trap the user if AskUserQuestion has a runtime issue | Module STEP A builder detects `skippable: false` and includes a fallback: on tool failure, apply `default:` and emit a warning to the summary instead of blocking |
| `/init-project` STEP 6 may pile cognitive load (post-gen gate asks questions about freshly-generated docs the user hasn't yet read) | Only fires when TBD markers exist OR `open-questions.md` has open entries — not aggressive; quiet by default |
| Two commands now share a schema flag (`skippable`) — coupling | The flag is purely advisory to the loop in `modules/interactive-clarifications.md`. Each command's question list is its own concern. The module is the only consumer. |

---

## Out of Scope (explicitly deferred)

- `/execute-work` pattern adoption (post-sign-off scope expansion brought `/add-bug` in; `/execute-work` remains deferred).
- `interactive-input-collector` abstraction layer (Approach B from brainstorming — YAGNI at four commands).
- `/add-scope` edit-mode submenu refactor (DP-9/10/11 — too many options).
- Replacing free-text content intake in `/add-scope` (DP-6/7/8) or `/add-bug` (title, description, reproduction steps, expected/actual, notes).
- Post-creation clarification gate in `/add-bug` — STEP 1 ambiguities (e.g., unclear repro steps) stay free-text; no new gate beyond AB-3 skip.
