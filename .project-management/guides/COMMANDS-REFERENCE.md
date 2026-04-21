# Commands Reference

**Quick reference and navigation guide for all Claude Project Management System commands.**

**Version:** 3.2.0
**Last Updated:** 2026-04-21

---

## Quick Command Index

| I want to... | Use this command | Quick Guide |
|--------------|------------------|-------------|
| Start new project | `/init-project` | [How-to](../.claude/commands/how-to-use/init-project.md) |
| Process client docs | `/process-client-docs` | [How-to](../.claude/commands/how-to-use/process-client-docs.md) |
| Add requirement (story/epic/phase) | `/add-scope add [type]` | [How-to](../.claude/commands/how-to-use/add-scope.md) |
| Add future requirement (v2.0, v3.0) | `/add-backlog-requirement` | [How-to](../.claude/commands/how-to-use/add-backlog-requirement.md) |
| Add bug to roadmap | `/add-bug` | [How-to](../.claude/commands/how-to-use/add-bug.md) |
| Execute phase work | `/execute-work phase N` | [How-to](../.claude/commands/how-to-use/execute-work.md) |
| Fix a bug | `/execute-work bug BUG-XXX` | [How-to](../.claude/commands/how-to-use/execute-work.md) |
| Promote future requirement | `/promote-requirement US-XXX --to-phase N` | Full docs only |
| Run tests manually | `/run-tests [scope]` | Full docs only |
| Check project status | `/project-status` | [How-to](../.claude/commands/how-to-use/check-status.md) |
| Update documentation | `/generate-docs` | [How-to](../.claude/commands/how-to-use/generate-docs.md) |

**For detailed documentation:** See full command files in [../.claude/commands/](../.claude/commands/)

---

## Command Categories

### 🚀 Setup & Initialization

**`/process-client-docs`**
- **Purpose:** Extract requirements from client documents (PDFs, Word, images)
- **When:** Before `/init-project`, when starting from client briefs
- **Output:** Generates `input/*.md` files from documents
- **Quick guide:** [process-client-docs.md](../.claude/commands/how-to-use/process-client-docs.md)
- **Full docs:** [../.claude/commands/process-client-docs.md](../.claude/commands/process-client-docs.md)

**`/init-project`**
- **Purpose:** Initialize project with tech stack, i18n, and phase structure
- **When:** One-time, after filling input files or processing client docs
- **Output:** Complete documentation + Phase 1 plan
- **Quick guide:** [start-project.md](../.claude/commands/how-to-use/init-project.md)
- **Full docs:** [../.claude/commands/init-project.md](../.claude/commands/init-project.md)

**`/generate-docs`**
- **Purpose:** Regenerate project documentation from input files
- **When:** After updating scope, backlog, or technologies
- **Output:** Updated PRD, tech spec, architecture docs
- **Quick guide:** [generate-documentation.md](../.claude/commands/how-to-use/generate-docs.md)
- **Full docs:** [../.claude/commands/generate-docs.md](../.claude/commands/generate-docs.md)

---

### 🎯 Execution & Development

**`/execute-work`**
- **Purpose:** Automated implementation with quality gates
- **Usage:** `/execute-work {phase N | epic EPIC-X | story US-XXX | bug BUG-XXX}`
- **When:** Daily development work
- **Features:** Plan mode → Implementation → Testing → Commit → Progress tracking
- **Quick guide:** [execute-phase.md](../.claude/commands/how-to-use/execute-work.md)
- **Full docs:** [../.claude/commands/execute-work.md](../.claude/commands/execute-work.md)

**`/run-tests`**
- **Purpose:** Manual test execution with detailed reporting
- **Usage:** `/run-tests {all | unit | integration | e2e | coverage | story US-XXX | file path}`
- **When:** Manual testing, debugging, coverage checks
- **Output:** Test results, coverage reports, failure details
- **Full docs:** [../.claude/commands/run-tests.md](../.claude/commands/run-tests.md)

---

### 📝 Scope & Requirements

**`/add-scope`**
- **Purpose:** Add or edit phases, epics, stories with automatic renumbering
- **Usage:**
  - `/add-scope add {phase N | epic N | story PHASE EPIC}`
  - `/add-scope edit {phase N | epic EPIC-X | story US-XXX}`
- **When:** Scope changes, new features, editing requirements
- **Features:** Auto-renumbering, cross-reference updates, integrity checks
- **Quick guide:** [add-requirement.md](../.claude/commands/how-to-use/add-scope.md)
- **Full docs:** [../.claude/commands/add-scope.md](../.claude/commands/add-scope.md)

**`/add-backlog-requirement`**
- **Purpose:** Add requirements to future backlog (v2.0, v3.0, beyond)
- **Usage:** `/add-backlog-requirement {story | epic}`
- **When:** Planning post-launch features, collecting ideas for later
- **Output:** Added to `backlog-future.md` (NOT active development)
- **Quick guide:** [add-backlog-requirement.md](../.claude/commands/how-to-use/add-backlog-requirement.md)
- **Full docs:** [../.claude/commands/add-backlog-requirement.md](../.claude/commands/add-backlog-requirement.md)

**`/promote-requirement`**
- **Purpose:** Move future requirement to active development
- **Usage:** `/promote-requirement US-XXX --to-phase N`
- **When:** Ready to implement a future requirement
- **Output:** Moves from `backlog-future.md` to `backlog.md` and `phase-N.md`
- **Full docs:** [../.claude/commands/promote-requirement.md](../.claude/commands/promote-requirement.md)

---

### 🐛 Bug Management

**`/add-bug`**
- **Purpose:** Add bugs to roadmap with severity-based organization
- **Usage:** `/add-bug [--from file.md]`
- **When:** Bug discovered, client reports issue
- **Output:** Added to `bug-roadmap.md` with automatic BUG-XXX ID
- **Features:** Severity levels (Critical/High/Medium/Low), story point estimates
- **Quick guide:** [add-bug.md](../.claude/commands/how-to-use/add-bug.md)
- **Full docs:** [../.claude/commands/add-bug.md](../.claude/commands/add-bug.md)

---

### 📊 Tracking & Reporting

**`/project-status`**
- **Purpose:** Comprehensive project status report
- **Usage:** `/project-status`
- **When:** Weekly checks, before meetings
- **Output:** Progress, bugs, blockers, quality metrics, recommendations
- **Quick guide:** [check-status.md](../.claude/commands/how-to-use/check-status.md)
- **Full docs:** [../.claude/commands/project-status.md](../.claude/commands/project-status.md)

**Progress tracking**
- Progress is now updated automatically during `/execute-work`
- Open `../output/progress/DASHBOARD.md` for live view
- No manual command — the `/update-progress` command was removed in v3.2.0

---

## Command Usage Patterns

### Starting a New Project

```bash
# Option A: From client documents
cp client-docs/* .project-management/client-input/
/process-client-docs
# → Review generated input files
/init-project

# Option B: Manual entry
# → Fill input/*.md files manually
/init-project
```

### Development Workflow

```bash
# Automated workflow (recommended)
/execute-work phase 1
/execute-work story US-015
/execute-work bug BUG-001

# Manual workflow
# → Implement code manually
/run-tests all
# → Fix issues
# → Commit manually
# → Open DASHBOARD.md (auto-updated) to check progress
```

### Scope Management

```bash
# Add new requirements
/add-scope add story 1 2
/add-scope add epic 1

# Plan for future
/add-backlog-requirement story
# → Add to Version 2.0 backlog

# When ready to implement future requirement
/promote-requirement US-045 --to-phase 2
```

### Monitoring & Reporting

```bash
# Check status
/project-status

# Manual testing
/run-tests coverage
/run-tests e2e

# Update docs after changes
/generate-docs
```

---

## Command Decision Tree

```
Starting project?
├─ Have client docs? → /process-client-docs → /init-project
└─ No client docs? → Fill input files → /init-project

Ready to develop?
├─ Want automation? → /execute-work phase N
├─ Complex task? → Manual + /run-tests
└─ Bug fix? → /execute-work bug BUG-XXX

Scope changed?
├─ Active development? → /add-scope add [type]
├─ Future version? → /add-backlog-requirement
└─ Promote future? → /promote-requirement US-XXX

Need status?
├─ Live view? → Open DASHBOARD.md (auto-updated)
└─ Detailed report? → /project-status

Tests or docs?
├─ Run tests? → /run-tests [scope]
└─ Update docs? → /generate-docs
```

---

## Getting Help

### Quick Guides (80-150 lines)
- **Start project:** [../.claude/commands/how-to-use/init-project.md](../.claude/commands/how-to-use/init-project.md)
- **Execute work:** [../.claude/commands/how-to-use/execute-work.md](../.claude/commands/how-to-use/execute-work.md)
- **Add requirements:** [../.claude/commands/how-to-use/add-scope.md](../.claude/commands/how-to-use/add-scope.md)
- **Add bugs:** [../.claude/commands/how-to-use/add-bug.md](../.claude/commands/how-to-use/add-bug.md)
- **Check status:** [../.claude/commands/how-to-use/check-status.md](../.claude/commands/how-to-use/check-status.md)
- **Process client docs:** [../.claude/commands/how-to-use/process-client-docs.md](../.claude/commands/how-to-use/process-client-docs.md)
- **Generate docs:** [../.claude/commands/how-to-use/generate-docs.md](../.claude/commands/how-to-use/generate-docs.md)

### Full Documentation (200-450 lines)
- **All commands:** [../.claude/commands/](../.claude/commands/)
- **Command modules:** [../.claude/commands/modules/](../.claude/commands/modules/)

### Additional Resources
- **FAQ:** [FAQ.md](FAQ.md)
- **Troubleshooting:** [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
- **Workflows:** [WORKFLOWS-BEST-PRACTICES.md](WORKFLOWS-BEST-PRACTICES.md)
- **Getting Started:** [GETTING-STARTED.md](GETTING-STARTED.md)

---

**Version:** 3.2.0
**Last Updated:** 2026-04-21
**Part of:** Claude Project Management System v3.2

**Note:** This is a quick reference. For detailed command documentation, see individual command files in `../.claude/commands/`.
