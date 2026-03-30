# Changelog

All notable changes to the Claude Project Management System will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
- **Tests run automatically** - Before commits (predzadnji step)
- **Commits created automatically** - After tests pass (zadnji step)
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

[3.0.0]: https://github.com/nnikolic/claude_repo/compare/v2.0.0...v3.0.0
[2.0.0]: https://github.com/nnikolic/claude_repo/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/nnikolic/claude_repo/releases/tag/v1.0.0
