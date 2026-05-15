# Changelog

All notable changes to the Claude Project Management System will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added

- **Interactive clarification gate (`/process-client-docs` STEP 5)** — after extraction, the command no longer dumps a flat "Items Needing Clarification" list. It runs `AskUserQuestion` for each open question one-by-one with concrete option buttons + an explicit `Skip — answer later` option. The previous Blocker/Important/Nice-to-know taxonomy is now formalized as P0/P1/P2 priorities.
- **`.claude/commands/modules/interactive-clarifications.md`** — reusable Q&A loop (STEPS A–G: schema, AskUserQuestion call shape, skip handling, free-text anonymization, answer-application to artefacts). Documented integration targets: `/init-project`, `/add-scope`, `/execute-work`, `/add-bug` (deferred).
- **`.project-management/templates/open-questions-template.md`** — schema for the new `input/open-questions.md` (per-question block with Status, Priority, Category, Asked During, Skipped count, Question, Default, Impact, Options Presented, Notes). Resolved questions move to a `## Resolved Questions` archive section.
- **`/resolve-questions` command** (`.claude/commands/resolve-questions.md` + `resolve-questions-reference.md`) — re-runs the interactive loop on still-Open entries in `input/open-questions.md`. Filters: `--priority P0|P1|P2` or single `Q-NNN`. Updates artefacts referenced by `applies_to` paths and moves answered entries to the Resolved archive.
- **Structured clarification schema** in `modules/extraction-by-section.md` and `modules/extraction-quality-output.md` — replaces free-text bullets with YAML schema (id, category, priority, question, default, impact, options, applies_to, notes) that feeds the interactive loop. `<!-- TBD: Q-NNN -->` markers inserted into target artefacts during STEP 3 are replaced when answers come in.
- **`/init-project` STEP 0/1/2 → AskUserQuestion** — Project type, stack approach, i18n yes/no are now gating decisions (`skippable: false`) with native UI buttons replacing the numbered narrative menus.
- **`/init-project` Custom stack flow → AskUserQuestion sequence** — Each layer (backend / database / frontend / styling / testing / build / deploy) asks one AskUserQuestion with top-3 most-common options + native Other for free-text. Skip = use recommended default; free-text passes through the anonymization rule.
- **`/init-project` STEP 6 — post-generation clarification gate** — After PRD / technical-spec / architecture are generated, scan for `<!-- TBD: Q-NNN -->` markers AND read open P0/P1 entries in `input/open-questions.md`. If found, invoke the interactive Q&A loop. Mirrors `/process-client-docs` STEP 5.
- **`/init-project` i18n iterative language loop** — Replaces the prior comma-separated text input with AskUserQuestion + "Add another?" loop. ISO code lookup table maps free-text answers.
- **`/add-scope` action + scope-type → AskUserQuestion** — Two gating decisions (`skippable: false`). Position / target-phase / target-epic remain free-text numeric (dynamic bounds); content intake stays narrative (AskUserQuestion wrong tool for prose).
- **`/add-scope` STEP 7 docs-cascade → AskUserQuestion** — Three outcomes: Yes (run /generate-docs now), No (manual later), Skip (log P2 docs-cascade entry to `input/open-questions.md`).
- **`/add-bug` Severity → AskUserQuestion** — Gating (`skippable: false`), 4 fixed options (Critical / High / Medium / Low). Replaces the narrative bullet list; routes the bug into the matching section of `bug-roadmap.md`.
- **`/add-bug` Story Points → AskUserQuestion** — Deferable (`skippable: true`), top-3 Fibonacci values (1 / 3 / 5) + native Other for 2 / 8 / 13. Skip uses the severity-based suggestion. Non-Fibonacci free-text rounds up with a warning.
- **`/add-bug` STEP 4 Phase Assignment → AskUserQuestion** — Three outcomes: Yes (chains a second AskUserQuestion for phase pick when ≤ 4 phases, numeric fallback for > 4), No (Backlog, Recommended), Skip (log P2 `bug-triage` entry to `input/open-questions.md`).

### Schema

- **`skippable` flag added to question schema** in `modules/interactive-clarifications.md`. Optional, default `true`. Set `false` for gating questions where the command cannot proceed without a choice. Existing `/process-client-docs` questions without the flag continue to work unchanged.

### Changed

- **`process-client-docs.md` STEP 4 summary** — clarification section now shows counts by priority (P0/P1/P2) and forwards to STEP 5 (the interactive gate) rather than printing the full list. Final next-steps (STEP 6) updated to suggest `/resolve-questions --priority P0` if blockers remain.
- **Free-text answer handling** — `Other` answers in the interactive loop pass through `.claude/rules/anonymization.md` §3–4 before being persisted to `open-questions.md` or downstream artefacts.

---

## [3.3.0] - 2026-05-11

### Added

- **`.claude/rules/anonymization.md`** — Personal names from input documents (briefs, meeting notes, emails) MUST be replaced with role labels (`the PM`, `the client`, `the stakeholder`) and source-context phrases (`per our call`, `agreed in the planning meeting`) in any generated project artifact (PRD, backlog, technical spec, status, ADR). Enforced via gates in `/process-client-docs` STEP 2.5 (anonymization pass), `/init-project` STEP 4, `/generate-docs` Guidelines, and `/add-bug` STEP 1.
- **`.claude/rules/enums-and-constants.md`** — Cross-layer naming convention: every enum-like value crossing layer boundaries (database ↔ backend ↔ frontend ↔ mobile) uses `SCREAMING_SNAKE_CASE` as the wire format. Format selected because the default stack (Prisma + Postgres + TypeScript + Zod + optional RN / Kotlin / Swift) requires zero mapping with it. Includes per-layer code recipes, source-of-truth principle (one place per enum), strict separation from i18n display labels.
- **`.claude/rules/api-versioning.md`** — `/api/v{N}/` URL-path versioning formalized (was already de facto convention in examples). Adds the load-bearing **change-propagation gate (§5)**: when a backend endpoint contract changes, the same PR MUST update docs + Zod request/response schemas + ALL tests touching the endpoint + in-repo consumer code. A test failing due to a contract change is a signal to re-check breaking vs non-breaking, not "expected breakage". Deprecation policy in §6 (parallel `v{N}` / `v{N+1}` maintenance, 6-month sunset, `Deprecation` + `Sunset` response headers).
- **`.claude/rules/error-handling-and-logging.md`** — Five mandatory properties per error: typed → single boundary → canonical envelope → structured log → tests. Defines 9-category error taxonomy mapped to HTTP statuses and `SCREAMING_SNAKE_CASE` `code` prefixes. Single `AppError` base + one subclass per category. Hard prohibition on logging PII / secrets / auth bodies (with `pino` redaction config). `request_id` correlation via `X-Request-Id` header. "Log once at the boundary, never duplicate across nested catches." Error tracker (Sentry / equivalent) mandatory in production with PII scrubbed.
- **`.claude/rules/security-and-auth.md`** — Stack-specific (React Router 7 cookie sessions, not JWT). Default-deny middleware (`requireAuth` + `requireRole`), distinct resource-level ownership check (prevents IDOR — most common bug behind "endpoint protected but any user reads any record"). bcryptjs cost 12, password min 8 enforced via Zod (not just UI). Rate limits on auth endpoints (login 5/15min, signup 5/hr, password-reset 3/hr). Session lifecycle (rotate on login, invalidate-all on password change). Full security-headers spec (CSP, HSTS, X-Frame-Options, X-Content-Type-Options, Referrer-Policy, Permissions-Policy). SSRF guard. 7-category audit log (login success/failure, role grant/revoke, permission denial). `npm audit` clean in CI.
- **`.claude/rules/screen-inventory.md` + `/screen-map` slash command** — For web CMS / mobile / web-with-admin projects: consolidated `input/screens/screen-map.md` artifact with hand-curated navigation hierarchy (ASCII tree with auth gates marked on each node) + per-screen registry. API-endpoint columns are **generated** by `/screen-map` from frontend story tables (per `screen-driven-backlog.md`) — never hand-maintained, preventing drift. Drift detection in §4 of the file. Auto-scaffolded by `/init-project` for project types 2 (Backend + Mobile), 3 (Full Monorepo), 4 (Web Only). Skipped for backend-only / CLI / library / simple SPA (≤ 5 screens).
- **`/screen-map` command** (`.claude/commands/screen-map.md`) — Reads frontend stories from `input/backlog/phase-*.md`, refreshes the API endpoint columns + Status fields in the screen map, reports drift. Includes how-to-use guide. Template `screen-map-template.md` added to `.project-management/templates/`.
- **Quality gates in `/execute-work`** — `execute-work-quality-gates.md` extended with three new gates that fire when the relevant code is touched: **API Documentation Gate** (now includes versioning subsection), **Error Handling & Logging Gate** (11 checklist items), **Security & Auth Gate** (11 checklist items, replaces the prior skeletal "Security" section).
- **Sub-agent wiring** — `execute-work-implementation.md` § A.1 STEP 1 reading list reorganized into conditional groups (always / endpoint touched / data-model touched / frontend / input-doc consuming) so sub-agents in clean context load exactly the rules their story needs. Screen-map refresh trigger added at § A.2 step 3a (Continuous) and § B 3.8 (Paused).
- **DASHBOARD event** — `execute-work-dashboard-events.md` §3.8 documents the screen-map refresh trigger for completed frontend stories.

### Changed

- **`documentation.md` split** (487 → 196 lines) into three companion files: `documentation.md` (core writing rules: language, file size, style, quality checklist), `documentation-templates.md` (147 lines — user story / task / bug / API endpoint templates), `documentation-extras.md` (174 lines — code comments, diagrams, tools, good/bad examples). All cross-references updated (6 places in 4 files): `§4.1` → `documentation-templates.md §1.1`, `§6` / `§6.1` → `documentation-templates.md §2` / `§2.1`.
- **`permissions.md` split** (484 → 176 lines) into three companion files: `permissions.md` (core rules: critical settings behavior, safety patterns, best practices, troubleshooting), `permissions-patterns.md` (175 lines — pattern syntax + common patterns + "permission needed" response template), `permissions-examples.md` (176 lines — full settings.json examples: Option A broad / Option B granular / recommended setup / corruption recovery). All external references (`settings.example.json`, `.claude/hooks/README.md`) continue to point at the main file, which still contains the critical rules.
- **`/execute-work` "CRITICAL RULES" list** expanded from 7 to ~15 entries, grouped by stage (always / endpoint touched / data-model touched / frontend / input-doc consuming). Conditional rules clearly marked.
- **`/execute-work` Paused-mode workflow** (`execute-work.md` STEP 3-B steps 5, 8, 11) now calls out each gate explicitly instead of pointing generically at `quality-gates.md`. Step 11 invokes `/screen-map` for frontend stories on completion.
- **`/init-project`** — new STEP 6.5 auto-scaffolds `input/screens/screen-map.md` from the template for project types that include a UI.
- **`CLAUDE.md`** — Specialized Rules section regrouped into "Always" + "Conditional"; rule list expanded from 7 to 20 entries; stale `input/backlog.md` reference corrected to modular `input/backlog/`; RELATED DOCS at the bottom expanded from 6 to 20 grouped links.

### Fixed

- **AI-attribution conflict resolved** — `documentation.md` §4.4 previously showed `Generated with Claude Code` + `Co-Authored-By: Claude` as part of a "correct" commit template while `git.md` explicitly prohibited them under `❌ NO AI credits`. §4.4 now defers to `git.md` and notes the prohibition. The commit-type list (paraphrased duplicate between the two files) also consolidated into `git.md` (which gained the missing `perf:` type).
- **Duplicate API response format block** — `api-documentation.md` §2.2 inline `{ "success": true, "data": {...} }` / `{ "success": false, "error": ..., "code": ... }` block replaced with a cross-reference to `stack-specific.md` §"Node.js Backend" (the canonical source).
- **Duplicate user-story template** — `screen-driven-backlog.md` §3 no longer reproduces the base template from `documentation-templates.md` §1.1; lists only the four extra fields frontend stories add (Type, Screen, API Endpoints Used, API contract status).

---

## [3.2.0] - 2026-04-21

### Removed
- **`/update-progress` command** — DASHBOARD.md auto-updates during `/execute-work` cover its role; manual edits to progress files are done directly. This removes a long-stale PENDING integration.

### Added
- **`/audit-pm` command** — framework health audit that scans for version drift, broken links, stale references, file-size violations, legacy artifacts, and how-to-use coverage gaps. Two-layer design: `.claude/hooks/audit-pm.sh` runs deterministic checks; the slash command layers judgement calls on top and produces a prioritized 🔴/🟠/🟡/🟢 report.
- **Validation hooks** — `.claude/hooks/post-write-validations.sh` (PostToolUse, Write|Edit) enforces documentation.md §2.1 file-size limits and reminds about backlog README sync; `.claude/hooks/stop-changelog-check.sh` (Stop) warns when unpushed commits don't touch CHANGELOG. Wired into `.claude/settings.example.json`.
- **`/migrate-to-modular` how-to-use guide** — brings how-to-use coverage to 12/12 commands.
- **Meta-repo migrated to modular backlog** — the framework now dogfoods its own `input/backlog/` split (phase-1..4 + future + README) plus `output/progress/` live files (DASHBOARD, daily-summary, weekly-report, current-status, completed, blockers).
- **Formal CHANGELOG entries** for 3.1 and 3.2 (previously missing).

### Fixed
- **Broken `FAQ-TROUBLESHOOTING.md` links** — 8 references across 5 files now point at the correct `guides/FAQ.md` / `guides/TROUBLESHOOTING.md`.
- **Broken relative paths in guides** — 42 `[text](../path)` links in FAQ/TROUBLESHOOTING/COMMANDS-REFERENCE corrected to `../../path` (they resolved to `.project-management/.claude/...` instead of the root `.claude/`).
- **Stale how-to-use references** — `./start-project.md`, `./generate-documentation.md`, `./check-status.md` renamed references updated after the file renames.
- **Dead references removed** — `MIGRATION-COMPLETE.md` and `test-migration/` (never existed on disk).
- **Version strings unified** — root README, `.project-management/README.md`, and CHANGELOG now agree on v3.2.0 as current. All `**Version:** 3.1.0` strings across 10 files bumped to 3.2.0.
- **Audit script refinement** — skips `templates/`, `examples/`, `client-input/`, `COMMAND-TEMPLATE.md`, and strips fenced code blocks before link extraction; filters inline-code backlog mentions and historical/migration context when flagging legacy command references.

### Changed
- **`/migrate-to-modular` repositioned** — marked legacy-only in primary docs. New projects use `/init-project` / `/process-client-docs` (direct modular generation).
- **~20 `/update-progress` references scrubbed** across docs, modules, and how-to-use guides; readers now routed to DASHBOARD.md or direct file edits.
- **how-to-use guide renames** — `check-status.md` → `project-status.md`; four other legacy names fixed earlier in this release for command-name parity.
- **Oversized modules split** (4 modules > 400 lines → 8 modules ≤ 280 lines): `execute-work-dashboard-update`, `live-progress-dashboard`, `init-project-structure-setup`, `add-scope-readme-update`.
- **Duplicate docs trimmed** — `guides/WORKFLOWS-BEST-PRACTICES.md` reduced from 341 → 97 lines (pointer file); `USER-GUIDE.md` retired; `SYSTEM-OVERVIEW.md` narrowed to flat file map (388 → 187 lines).
- **FAQ vs TROUBLESHOOTING boundary sharpened** — each file now has a "When to Read This File" table distinguishing "How do I…?" from "Why doesn't X work?".
- **`.claude/commands/how-to-use/README.md`** — removed the `/update-progress` row from the helper-commands table.

---

## [3.1.0] - 2026-04-20

### Added
- **Modular backlog structure** — `input/backlog/` split by phase (< 200 lines each) replacing monolithic `backlog.md`.
- **Live DASHBOARD.md** — `output/progress/DASHBOARD.md` auto-updates during `/execute-work` (real-time progress without running commands).
- **Daily and weekly summary files** — `daily-summary.md`, `weekly-report.md` under `output/progress/`.
- **`/migrate-to-modular` command** — one-shot migration from monolithic to modular structure for existing projects.
- **Direct modular generation** — `/init-project` and `/process-client-docs` produce the modular layout directly.
- **Modular structure guide** — `guides/MODULAR-STRUCTURE-GUIDE.md`.
- **Supporting modules** — `backlog-organization.md`, `live-progress-dashboard.md`, `execute-work-dashboard-update.md`, `add-scope-readme-update.md`, `init-project-structure-setup.md`.

### Changed
- **`/project-status`, `/add-scope`, `/execute-work`** integrated with modular structure (auto-detect monolithic vs modular, read only the relevant phase file).
- **~69% average token savings per command** (project-status, add-scope, execute-work).

---

## [Unreleased]

### Added

#### Bug Tracking System (2026-04-02)
- **`/add-bug` command** - Add bugs to roadmap with severity-based organization
- **Bug roadmap** - `.project-management/output/bugs/bug-roadmap.md` (Critical/High/Medium/Low sections)
- **Bug archive** - `.project-management/output/bugs/bug-archive.md` for fixed bugs history
- **Bug template** - Standardized format for reporting bugs
- **Bug execution** - `/execute-work bug BUG-XXX` integration
- **Bug metrics** - Integrated into `/project-status` command

#### Future Backlog System (2026-04-02)
- **`/add-backlog-requirement` command** - Add requirements for Version 2.0, 3.0, or unversioned ideas
- **`/promote-requirement` command** - Move future requirements to active development phases
- **Future backlog file** - `.project-management/input/backlog-future.md` (organized by version)
- **Sequential ID management** - US-XXX IDs span both active and future backlogs to prevent collisions
- **Version targeting** - 2.0 (post-launch), 3.0 (major future), Unversioned (ideas)

#### Documentation Enhancements (2026-04-02)
- **AI-optimized how-to guides** - Quick guides (80-150 lines) for all commands in `.claude/commands/how-to-use/`
- **Rules references** - All 10 command files now explicitly reference `.claude/rules/` files
- **Command index** - Decision tree and quick reference table in `how-to-use/README.md`
- **English-only enforcement** - All Serbian words translated, comprehensive policy

### Changed

#### Commands Enhanced
- **All command files** - Added explicit references to code-quality.md, testing.md, git.md rules
- **`/execute-work`** - Enhanced with rules guidance for SOLID & DRY, testing, git conventions
- **`/project-status`** - Now includes bug metrics from bug-roadmap and bug-archive

#### Documentation Improvements
- **README.md** - Updated Quick Start (removed `/plan-sprint`, added new commands)
- **Templates** - Updated progress-template.md to use "phase" terminology instead of "sprint"
- **Input files** - Updated instructions to reference "phases" instead of "sprints"

### Fixed
- **Sprint terminology** - Replaced outdated "sprint" references with "phase" in templates
- **Serbian translations** - "PREDZADNJI STEP" → "SECOND-TO-LAST STEP", "ZADNJI STEP" → "FINAL STEP"

---

## [3.0.0] - 2026-03-27

### 🎉 Major Release: Phase-Based System with Automation

Complete overhaul from sprint-based to phase-based planning with full automation capabilities.

---

### Added

#### Core Features
- **Phase-based planning system** - Replace 2-week sprints with 1-4 month phases
- **Automated execution** - `/execute-work` command for phase/epic/story automation
- **Mandatory plan mode** - Required planning phase before implementation
- **Auto-testing integration** - Vitest + Playwright run automatically as second-to-last step
- **Auto-git commits** - Professional commits created automatically as last step (NO AI credits)
- **Auto-progress tracking** - Real-time updates to phase progress files
- **Default tech stack system** - 3 options: Default HolyEstate / AI Recommendation / Custom
- **i18n configuration** - Multi-language setup during project initialization
- **Manual testing command** - `/run-tests` for on-demand test execution

#### Commands
- `/execute-work phase N` - Execute entire Phase N with full automation
- `/execute-work epic EPIC-X` - Execute specific Epic X
- `/execute-work story US-XXX` - Execute single story US-XXX
- `/run-tests all/unit/integration/e2e/coverage/story/file` - Manual test execution

#### Modules (AI Optimization)
- `modules/execute-work-plan-mode.md` - Plan mode workflow (183 lines)
- `modules/execute-work-implementation.md` - Implementation loop (256 lines)
- `modules/execute-work-quality-gates.md` - Quality validation (192 lines)
- `modules/init-project-stack-selection.md` - Stack selection (220 lines)
- `modules/init-project-i18n-setup.md` - i18n configuration (240 lines)

#### Documentation
- **MIGRATION-GUIDE.md** - Complete guide for v2.0 → v3.0 migration
- **USER-GUIDE.md** - Completely rewritten (3779→857 lines, 77% reduction)
- **Plan mode section** in `CLAUDE.md`
- **Conditional rules pattern** - i18n and testing rules only applied if files exist

#### Files & Structure
- Phase templates (`phase-template.md`, `phase-progress-template.md`)
- Default stack specification (`default-stack.md`, `stack-questions.md`)
- Phase output directory (`.project-management/output/phases/`)
- Module directory (`.claude/commands/modules/`)

---

### Changed

#### Architecture
- **Sprint-based → Phase-based** - Planning cycles now 1-4 months instead of 2 weeks
- **Manual → Automated** - Full automation with quality gates
- **Modular command structure** - Main commands reference detailed modules

#### Commands (Breaking Changes)
- **Enhanced `/init-project`** - Now includes tech stack selection and i18n setup (400→656 lines)
- **Modularized `/execute-work`** - Reduced from 702→251 lines (64% reduction)
- **Modularized `/init-project`** - Reduced from 656→257 lines (61% reduction)
- **Modularized `/run-tests`** - Reduced from 645→222 lines (66% reduction)

#### Workflow
- **Plan mode is now mandatory** - Cannot skip planning phase
- **Tests run automatically** - Before commits (second-to-last step)
- **Commits created automatically** - After tests pass (final step)
- **Progress updated automatically** - Real-time phase tracking

#### Documentation
- **README.md** - Updated for v3.0 workflow
- **CLAUDE.md** - Added plan mode section, updated commands
- **.gitignore** - Changed from `sprints/*` to `phases/*`
- **All documentation in English** - No Serbian text remaining

---

### Removed

#### Deprecated (Breaking Changes)
- ❌ `/plan-sprint` command - Replaced by `/execute-work phase N`
- ❌ Sprint-based files - `sprint-1.md`, `sprint-*.md` no longer used
- ❌ `current-status.md` - Replaced by phase progress files
- ❌ Sprint directory structure - Moved to phases

---

### Fixed

- **gitignore** - Updated to use `phases/*` instead of `sprints/*`
- **English documentation** - All Serbian text removed from commands
- **File organization** - v2.0 artifacts cleaned up

---

### Performance & Optimization

#### AI Optimization
- **64% reduction** in main command file sizes (2,003→730 lines)
- **Modular structure** - AI reads only what it needs
- **Token efficiency** - Reduced token usage by ~60%
- **Faster processing** - Smaller files load faster

#### Code Quality
- **All main commands < 260 lines** - Best practice for AI
- **All modules < 260 lines** - Consistent structure
- **Clear separation of concerns** - Easier to maintain

---

### Migration

**From v2.0 to v3.0:**
- See [MIGRATION-GUIDE.md](.project-management/docs/MIGRATION-GUIDE.md) for detailed instructions
- Sprint files must be manually converted to phases
- `/plan-sprint` command no longer works
- Old progress tracking format incompatible

---

### Technical Details

**Default Tech Stack (HolyEstate):**
- React 19.0.0 + React Router 7.13.0 (SSR)
- TypeScript 5.7.0 + Vite 6.1.0
- PostgreSQL 16.x + Prisma 6.19.0
- Vitest 4.0.0 + Playwright 1.58.0
- 848 tests passing in production

**Quality Gates:**
- 80%+ code coverage required
- All API status codes tested (200/400/401/403/404/500)
- i18n translations mandatory (if enabled)
- SOLID & DRY principles enforced
- Git conventions followed (NO AI credits)

---

### Credits

**Development:** Milos Ilic with Claude Code assistance
**Testing:** Production-tested on HolyEstate project
**Documentation:** Complete rewrite for v3.0

---

## [2.0.0] - 2026-03-XX

### Added
- Sprint-based planning system
- Manual execution with TodoWrite
- Basic testing guidelines
- Project initialization

### Changed
- Initial structure and templates

---

## [1.0.0] - 2026-03-XX

### Added
- Initial project management framework
- Basic templates and structure
- Documentation standards

---

[3.2.0]: https://github.com/nnikolic/claude_repo/compare/v3.1.0...v3.2.0
[3.1.0]: https://github.com/nnikolic/claude_repo/compare/v3.0.0...v3.1.0
[3.0.0]: https://github.com/nnikolic/claude_repo/compare/v2.0.0...v3.0.0
[2.0.0]: https://github.com/nnikolic/claude_repo/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/nnikolic/claude_repo/releases/tag/v1.0.0
