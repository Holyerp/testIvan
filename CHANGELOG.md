# Changelog

All notable changes to the Claude Project Management System will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [3.2.0] - 2026-04-21

### Removed
- **`/update-progress` command** — DASHBOARD.md auto-updates during `/execute-work` cover its role; manual edits to progress files are done directly. This removes a long-stale PENDING integration.

### Added
- **Meta-repo migrated to modular backlog** — the framework now dogfoods its own `input/backlog/` split (phase-1..4 + future + README) plus `output/progress/` live files (DASHBOARD, daily-summary, weekly-report, current-status, completed, blockers).
- **Formal CHANGELOG entries** for 3.1 and 3.2 (previously missing).

### Fixed
- **Broken `FAQ-TROUBLESHOOTING.md` links** — 8 references across 5 files now point at the correct `guides/FAQ.md` / `guides/TROUBLESHOOTING.md`.
- **Dead references removed** — `MIGRATION-COMPLETE.md` and `test-migration/` (never existed on disk).
- **Version strings unified** — root README, `.project-management/README.md`, and CHANGELOG now agree on v3.2.0 as current.

### Changed
- **`/migrate-to-modular` repositioned** — marked legacy-only in primary docs. New projects use `/init-project` / `/process-client-docs` (direct modular generation).
- **~20 `/update-progress` references scrubbed** across docs, modules, and how-to-use guides; readers now routed to DASHBOARD.md or direct file edits.
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
- **Plan mode section** in `.CLAUDE.MD`
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
- **.CLAUDE.MD** - Added plan mode section, updated commands
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
