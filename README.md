# Claude Project Management System

**Reusable project management framework for Claude-assisted development.**

A comprehensive system that provides structured planning, sprint management, documentation standards, and progress tracking for AI-assisted software projects.

## 🎯 What This Provides

- **Sprint Planning** - Structured sprint workflow with capacity estimation
- **Documentation Standards** - PRD, technical specs, architecture docs
- **Client Doc Processing** - Extract requirements from client documents
- **Progress Tracking** - Real-time task and sprint status updates
- **Code Quality Standards** - SOLID/DRY principles, testing requirements
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

### Project Management
- `/init-project` - Initialize project documentation
- `/plan-sprint N` - Plan sprint N with user stories
- `/update-progress` - Update task and sprint progress
- `/project-status` - Generate comprehensive status report
- `/process-client-docs` - Extract requirements from client documents
- `/generate-docs` - Generate or update project documentation

### During Development
Use `TodoWrite` tool to break down user stories into tasks and track implementation progress.

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

```
1. SETUP
   └─> Customize input files → /init-project

2. SPRINT PLANNING
   └─> /plan-sprint N → Review sprint plan

3. DEVELOPMENT
   └─> Read sprint plan → Break down stories (TodoWrite)
       → Implement → Test → Update progress

4. TRACKING
   └─> /update-progress → /project-status

5. REPEAT
   └─> Next sprint
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

**Version:** 2.0 (Modular)
**Updated:** 2026-03-26
