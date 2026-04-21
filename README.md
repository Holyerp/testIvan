# Claude Project Management System

**Reusable project management framework for Claude-assisted development.**

A comprehensive system that provides structured planning, phase management, automated execution, documentation standards, and progress tracking for AI-assisted software projects with a single developer + AI tools workflow.

## 🎯 What This Provides

- **Phase-Based Planning** - Organize work into 1-4 month phases with epics and stories
- **Automated Execution** - `/execute-work` command handles implementation, testing, commits, and progress
- **Default Tech Stack** - Production-tested HolyEstate stack (React 19 + RR7 + Prisma + Railway)
- **Plan Mode** - Mandatory planning phase before implementation with user approval
- **Auto Testing** - Vitest + Playwright tests run automatically before marking stories complete
- **Auto Git Commits** - Professional commits (NO AI credits) created automatically
- **i18n Support** - Multi-language configuration during project initialization
- **Documentation Standards** - PRD, technical specs, architecture docs
- **Client Doc Processing** - Extract requirements from client documents
- **Progress Tracking** - Real-time phase and story status updates
- **Code Quality Standards** - SOLID/DRY principles, testing requirements (80%+ coverage)
- **Git Workflow** - Conventional commits and best practices

---

## 💡 Current Structure: Modular Backlog + Live Dashboard (v3.1+)

**Automatically organized backlog:**
- 📂 Backlog split by phase (< 200 lines each) — easier to read
- 📊 Live DASHBOARD.md — see progress instantly, no commands
- ⚡ ~70% token savings for AI — processes only the relevant phase
- 🔄 Auto-updates — DASHBOARD refreshes during work

**For new projects:** Modular structure is generated directly by `/init-project` or `/process-client-docs`.

**For legacy projects** (monolithic `backlog.md`): `/migrate-to-modular` still exists as a one-shot upgrade path.

[📖 Learn more about modular structure](.project-management/guides/MODULAR-STRUCTURE-GUIDE.md)

---

## 🚀 Quick Start

### 1. Copy to Your Project

```bash
# Copy the system to your project
cp -r .project-management /path/to/your/project/
cp .CLAUDE.MD /path/to/your/project/
cp -r .claude /path/to/your/project/
```

**Also merge these `.gitignore` entries into your project's existing `.gitignore`** (don't overwrite — your project already has language/framework-specific patterns):

```gitignore
# Claude settings: keep out of git (use settings.example.json as template)
.claude/settings.json
.claude/settings.local.json

# Client docs (privacy)
.project-management/client-input/*
!.project-management/client-input/README.md

# Regenerated output files (recreated by /execute-work)
.project-management/output/docs/*
.project-management/output/phases/*
.project-management/output/progress/*
!.project-management/output/docs/.gitkeep
!.project-management/output/phases/.gitkeep
!.project-management/output/progress/.gitkeep
```

Without these, personal `settings.local.json` (may contain tokens), client-private docs, and constantly-regenerated progress files would leak into git.

### 2. Initialize Git + Initial Commit

If your project is not already a git repository, do this **before running any `/` commands**:

```bash
cd /path/to/your/project
git init -b main
git add .
git commit -m "chore: add Claude PM system framework"
```

Why this matters:
- `/execute-work` creates commits automatically per story — it needs a git repo to commit into
- The bundled `stop-changelog-check.sh` hook compares `HEAD` against `main` — it silently no-ops on a non-git repo but can't warn you about missing CHANGELOG entries
- Having a clean baseline commit makes it easy to see exactly what Claude changed during phase execution

If your project is already a git repo, just run `git add .` + commit the framework files once and continue.

### 3. Customize Input Files

Edit the files in `.project-management/input/`:
- `scope.md` - Define project scope and goals
- `backlog/` directory - List user stories by phase (created automatically)
- `technologies.md` - Specify tech stack
- `constraints.md` - Document limitations and requirements

💡 **Note:** New projects get modular backlog structure automatically!

### 4. Initialize Project

Run in Claude Code:
```
/init-project
```

This generates:
- Product Requirements Document (PRD)
- Technical Specification
- Phase structure (4 phases: Foundation → Core → Advanced → Polish)

### 5. Start Development

```
/execute-work phase 1
```

### 6. Check Progress Anytime

**🚀 Instant View (Recommended):**
```
Open: .project-management/output/progress/DASHBOARD.md
```
- Always current (auto-updates during work)
- No commands needed
- See: overall progress, today's work, active stories, quality metrics

**📊 Detailed Report (Optional):**
```
/project-status
```
- Comprehensive status report
- Calculates all metrics
- Exports to file

💡 **Tip:** DASHBOARD.md updates automatically as you work (no command needed).

## 📋 Available Commands

### Project Initialization
- `/init-project` - Initialize project with tech stack selection, i18n configuration, and phase structure (creates modular backlog automatically)
- `/migrate-to-modular` - Legacy one-shot upgrade from old `backlog.md` to modular structure (new projects don't need this)

### Automated Execution
- `/execute-work phase N` - Execute entire Phase N (all epics and stories)
- `/execute-work epic EPIC-X` - Execute specific Epic X (all stories)
- `/execute-work story US-XXX` - Execute single story US-XXX
- `/execute-work bug BUG-XXX` - Execute bug fix for BUG-XXX
  - Automatically enters **plan mode** before implementation
  - Runs tests automatically (Vitest + Playwright)
  - Creates git commits automatically (NO AI credits)
  - Updates progress tracking automatically
  - Supports continuous or pause-after-story modes

### Testing
- `/run-tests all` - Run all tests (unit, integration, E2E)
- `/run-tests unit` - Unit tests only
- `/run-tests integration` - Integration tests only
- `/run-tests e2e` - E2E tests only
- `/run-tests coverage` - Run with coverage report
- `/run-tests story US-XXX` - Tests for specific story
- `/run-tests file <path>` - Tests for specific file

### Scope Management
- `/add-scope add phase|epic|story` - Add or edit phases, epics, stories in active development
- `/add-backlog-requirement story|epic` - Add requirements to future backlog (Version 2.0, 3.0)
- `/promote-requirement US-XXX --to-phase N` - Move future requirement to active phase
- `/add-bug` - Add bugs to roadmap for tracking and execution

### Progress & Status
- `/project-status` - Generate comprehensive status report (includes bug metrics)
- Open `.project-management/output/progress/DASHBOARD.md` for live, auto-updated progress
- `/process-client-docs` - Extract requirements from client documents
- `/generate-docs` - Generate or update project documentation

### Manual Development
Use `TodoWrite` tool to break down user stories into tasks and track implementation progress when not using `/execute-work`.

## 📁 Directory Structure

```
.project-management/
├── input/              # Project definition (customize these)
│   ├── scope.md
│   ├── backlog/        # Modular backlog (split by phase)
│   │   ├── README.md                # Master index (< 150 lines)
│   │   ├── phase-1-foundation.md    # Phase 1 stories only
│   │   ├── phase-2-core.md          # Phase 2 stories only
│   │   ├── phase-3-advanced.md      # Phase 3 stories only
│   │   ├── phase-4-polish.md        # Phase 4 stories only
│   │   └── future.md                # Post-launch (v2.0, v3.0)
│   ├── technologies.md
│   └── constraints.md
├── output/
│   ├── docs/           # Generated documentation
│   │   ├── prd.md
│   │   └── technical-spec.md
│   ├── phases/         # Phase execution plans
│   │   └── phase-N.md
│   ├── bugs/           # Bug tracking
│   │   ├── bug-roadmap.md
│   │   └── bug-archive.md
│   └── progress/       # Progress tracking
│       ├── DASHBOARD.md         # ✨ NEW: Live view (auto-updates!)
│       ├── daily-summary.md     # ✨ NEW: Today's work
│       ├── weekly-report.md     # ✨ NEW: Weekly summary
│       ├── current-status.md    # Detailed status
│       ├── completed.md         # Historical log
│       └── blockers.md          # Active blockers
├── client-input/       # Raw client documents
└── templates/          # Document templates (16 templates)

.claude/
├── rules/              # Development standards
│   ├── code-quality.md
│   ├── testing.md
│   ├── git.md
│   ├── database.md
│   └── stack-specific.md
└── commands/           # Claude Code slash commands

.CLAUDE.MD              # Main AI developer guidelines
```

## 🔄 Workflow

### Automated Workflow (Recommended)
```
1. SETUP
   └─> Customize input files → /init-project
       → Select tech stack (Default/AI/Custom)
       → Configure i18n (optional)

2. EXECUTION
   └─> /execute-work phase 1
       → [PLAN MODE] Creates plan, waits for approval
       → [AUTO] Implements stories following standards
       → [AUTO] Runs tests (must pass before continuing)
       → [AUTO] Creates git commits (NO AI credits)
       → [AUTO] Updates progress tracking
       → Repeats for all stories in phase

3. TRACKING
   └─> Open DASHBOARD.md → See real-time progress (instant!)
       → Or run /project-status → Detailed report
       → /run-tests coverage → Check test coverage

4. REPEAT
   └─> /execute-work phase 2 → Continue with next phase
       → DASHBOARD.md stays updated automatically
```

### Manual Workflow (Alternative)
```
1. SETUP
   └─> Same as automated

2. PLANNING
   └─> Review phase plan manually

3. DEVELOPMENT
   └─> Read phase plan → Break down stories (TodoWrite)
       → Implement → Test manually (/run-tests)
       → Commit manually → Update progress

4. TRACKING
   └─> Open DASHBOARD.md (auto-updated) → /project-status (on demand)

5. REPEAT
   └─> Next phase
```

## 📖 Key Documents

### Must Read Before Starting
1. `.CLAUDE.MD` - Core AI developer guidelines
2. `.project-management/output/docs/technical-spec.md` - Technical specification
3. `.claude/rules/code-quality.md` - SOLID & DRY principles (mandatory)

### Read Conditionally
- `.claude/rules/testing.md` - Testing requirements
- `.claude/rules/git.md` - Git workflow
- `.claude/rules/database.md` - Migration standards
- `.claude/rules/stack-specific.md` - Framework guidelines

## 🎯 Core Principles

### MUST DO
- ✅ Follow SOLID & DRY principles
- ✅ Write tests for all features (80%+ coverage)
- ✅ Update documentation (tech spec, API docs, README)
- ✅ Handle errors properly
- ✅ Validate security (OWASP Top 10)

### MUST NOT DO
- ❌ Over-engineer solutions
- ❌ Create premature abstractions
- ❌ Add unrequested features
- ❌ Leave unused code
- ❌ Commit secrets

## 🧪 Quality Gates

Before marking any task complete:

**Code:**
- [ ] SOLID & DRY principles followed
- [ ] No TypeScript/linting errors
- [ ] Follows project conventions

**Testing:**
- [ ] All tests passing
- [ ] All API codes tested (200/400/401/403/404/500)

**Documentation:**
- [ ] Tech spec consulted & updated
- [ ] API docs, README, CHANGELOG updated

**Security:**
- [ ] No secrets committed
- [ ] No security vulnerabilities

## 🛠️ Tech Stack Support

Works with any tech stack. Customize `.claude/rules/stack-specific.md` for your framework-specific guidelines.

## 📝 License

This is a reusable template system. Copy and modify freely for your projects.

## 🤝 Contributing

This is a personal project management system. Feel free to fork and adapt to your needs.

---

**Version:** 3.2.0 (Direct Modular Generation + Cleanup)
**Updated:** 2026-04-21

---

## 🆕 What's New in v3.2.0

**Cleanup & correctness:**
- ✅ **`/update-progress` removed** — DASHBOARD.md auto-update during `/execute-work` replaces it
- ✅ **Meta-repo self-hosted on modular structure** — the framework's own backlog is now split by phase
- ✅ **Broken FAQ/troubleshooting links fixed** across all docs
- ✅ **Dead references removed** (MIGRATION-COMPLETE.md, test-migration/)
- ✅ **Version strings unified** across all top-level docs
- ✅ **`/migrate-to-modular` marked legacy-only** (`/init-project` and `/process-client-docs` generate modular directly)

For the full version history and feature-by-feature detail, see [CHANGELOG.md](CHANGELOG.md).

**Migration:** See [MIGRATION-GUIDE.md](.project-management/docs/MIGRATION-GUIDE.md) for upgrading from v2.0
