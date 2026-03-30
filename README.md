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

## 🚀 Quick Start

### 1. Copy to Your Project

```bash
# Copy the system to your project
cp -r .project-management /path/to/your/project/
cp .CLAUDE.MD /path/to/your/project/
cp -r .claude /path/to/your/project/
```

### 2. Customize Input Files

Edit the files in `.project-management/input/`:
- `scope.md` - Define project scope and goals
- `backlog.md` - List user stories and features
- `technologies.md` - Specify tech stack
- `constraints.md` - Document limitations and requirements

### 3. Initialize Project

Run in Claude Code:
```
/init-project
```

This generates:
- Product Requirements Document (PRD)
- Technical Specification
- Sprint structure

### 4. Plan Your First Sprint

```
/plan-sprint 1
```

## 📋 Available Commands

### Project Initialization
- `/init-project` - Initialize project with tech stack selection, i18n configuration, and phase structure

### Automated Execution
- `/execute-work phase N` - Execute entire Phase N (all epics and stories)
- `/execute-work epic EPIC-X` - Execute specific Epic X (all stories)
- `/execute-work story US-XXX` - Execute single story US-XXX
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

### Progress & Status
- `/update-progress` - Update phase and story progress
- `/project-status` - Generate comprehensive status report
- `/process-client-docs` - Extract requirements from client documents
- `/generate-docs` - Generate or update project documentation

### Manual Development
Use `TodoWrite` tool to break down user stories into tasks and track implementation progress when not using `/execute-work`.

## 📁 Directory Structure

```
.project-management/
├── input/              # Project definition (customize these)
│   ├── scope.md
│   ├── backlog.md
│   ├── technologies.md
│   └── constraints.md
├── output/
│   ├── docs/           # Generated documentation
│   │   ├── prd.md
│   │   └── technical-spec.md
│   ├── sprints/        # Sprint plans
│   │   └── sprint-N.md
│   └── progress/       # Progress tracking
│       ├── completed.md
│       ├── in-progress.md
│       └── blockers.md
├── client-input/       # Raw client documents
└── templates/          # Document templates

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
   └─> /project-status → Review progress
       → /run-tests coverage → Check test coverage

4. REPEAT
   └─> /execute-work phase 2 → Continue with next phase
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
   └─> /update-progress → /project-status

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

**Version:** 3.0 (Phase-Based with Automation)
**Updated:** 2026-03-27

---

## 🆕 What's New in v3.0

**Major Changes:**
- ✅ **Phase-based system** replaces sprint-based system
- ✅ **Automated execution** with `/execute-work` command
- ✅ **Plan mode** mandatory before implementation
- ✅ **Auto-testing** with Vitest + Playwright
- ✅ **Auto-commits** following git rules (NO AI credits)
- ✅ **Auto-progress** tracking updates
- ✅ **Default tech stack** system (3 options)
- ✅ **i18n configuration** during initialization
- ✅ **Manual testing** with `/run-tests` command

**Migration:** See [MIGRATION-GUIDE.md](.project-management/docs/MIGRATION-GUIDE.md) for upgrading from v2.0
