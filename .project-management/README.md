# Claude Project Management System

**Version:** 3.0.0
**Created:** 2026-03-24
**Purpose:** Autonomous project planning, documentation, and progress tracking system for Claude Code

---

## 📋 Quick Navigation

| I want to... | Read this |
|--------------|-----------|
| **Understand the system** | [Overview](#overview) below |
| **Get started quickly** | [Quick Start](#quick-start) below |
| **Learn system architecture** | [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) |
| **Fill input files** | [docs/INPUT-FILES-GUIDE.md](docs/INPUT-FILES-GUIDE.md) |
| **Understand generated docs** | [docs/GENERATED-DOCS-GUIDE.md](docs/GENERATED-DOCS-GUIDE.md) |
| **See workflow examples** | [docs/WORKFLOWS.md](docs/WORKFLOWS.md) |
| **Learn best practices** | [docs/BEST-PRACTICES.md](docs/BEST-PRACTICES.md) |
| **Reuse for other projects** | [docs/REUSE-GUIDE.md](docs/REUSE-GUIDE.md) |
| **Find command reference** | [guides/COMMANDS-REFERENCE.md](guides/COMMANDS-REFERENCE.md) |
| **Troubleshoot issues** | [guides/FAQ-TROUBLESHOOTING.md](guides/FAQ-TROUBLESHOOTING.md) |
| **Migrate from v2.0** | [docs/MIGRATION-GUIDE.md](docs/MIGRATION-GUIDE.md) |

---

## ⚠️ IMPORTANT: How Everything Works Together

**Before using this system, read:** [INTEGRATION-GUIDE.md](INTEGRATION-GUIDE.md)

This guide explains:
- ✅ What Claude should read and when
- ✅ How `.CLAUDE.MD` and project management system work together
- ✅ When to use TodoWrite vs /execute-work
- ✅ Document hierarchy and conflict resolution
- ✅ No conflicts, no confusion!

**TL;DR:**
- **Planning?** → Use automated phase planning with `/execute-work phase N`
- **Coding?** → Follow `.CLAUDE.MD` standards
- **Tracking?** → Automatic progress tracking during execution

[📖 Read Full Integration Guide](INTEGRATION-GUIDE.md)

---

## 🎯 Overview

The **Claude Project Management System** is a complete project management framework that enables Claude to:

- ✅ **Autonomously read** project scope, backlog, and constraints
- ✅ **Generate comprehensive documentation** (PRD, Technical Spec, Architecture)
- ✅ **Plan phases** based on priorities and project scope
- ✅ **Track progress** throughout development
- ✅ **Provide status reports** on-demand
- ✅ **Identify risks and blockers** proactively
- ✅ **Work on any project** - fully reusable structure

This system follows a **structured, opinionated approach** to project management, ensuring Claude always knows:
- 📂 Where to find input data
- 📝 Where to create documentation
- 📊 How to track progress
- 🎯 What to prioritize

**📖 Detailed architecture:** [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)

---

## 🚀 Quick Start

### 🎨 Option A: Start from Client Documents (Recommended)

**Perfect for agencies and freelancers getting project briefs from clients!**

```bash
# STEP 1: Add client documents
.project-management/client-input/
├── project-brief.pdf
├── requirements.docx
├── mockups.png
└── timeline.txt

# STEP 2: Process documents
/process-client-docs
# → Claude extracts requirements and generates input files

# STEP 3: Review & edit generated files
.project-management/input/
├── scope.md
├── backlog.md
├── technologies.md
└── constraints.md

# STEP 4: Initialize project
/init-project
# → Claude generates all documentation and Phase 1 plan

# STEP 5: Start development
/execute-work phase 1
# → Automated execution with testing and progress tracking
```

**📖 Full guide:** [client-input/README.md](client-input/README.md)

---

### 📝 Option B: Manual Entry

```bash
# STEP 1: Fill input files manually
.project-management/input/
├── scope.md          # Project vision, goals, phases
├── backlog.md        # All features and user stories
├── technologies.md   # Tech stack
└── constraints.md    # Timeline, budget, team

# STEP 2: Initialize project
/init-project
# → Claude generates all documentation and Phase 1 plan

# STEP 3: Start development
/execute-work phase 1    # Foundation (1-4 months)
/execute-work phase 2    # Core Features
/execute-work phase 3    # Advanced Features
/execute-work phase 4    # Polish & Launch

# Check status anytime
/project-status
```

**📖 Detailed input file guide:** [docs/INPUT-FILES-GUIDE.md](docs/INPUT-FILES-GUIDE.md)
**📖 Complete workflows:** [docs/WORKFLOWS.md](docs/WORKFLOWS.md)

---

## 🎮 Slash Commands Quick Reference

| I want to... | Use this command | Quick Guide |
|--------------|------------------|-------------|
| Start new project | `/init-project` | [How-to](../.claude/commands/how-to-use/start-project.md) |
| Process client docs | `/process-client-docs` | [How-to](../.claude/commands/how-to-use/process-client-docs.md) |
| Add requirement (story/epic/phase) | `/add-scope add [type]` | [How-to](../.claude/commands/how-to-use/add-requirement.md) |
| Add future requirement (v2.0, v3.0) | `/add-backlog-requirement` | [How-to](../.claude/commands/how-to-use/add-backlog-requirement.md) |
| Add bug to roadmap | `/add-bug` | [How-to](../.claude/commands/how-to-use/add-bug.md) |
| Execute phase work | `/execute-work phase N` | [How-to](../.claude/commands/how-to-use/execute-phase.md) |
| Fix a bug | `/execute-work bug BUG-XXX` | [How-to](../.claude/commands/how-to-use/execute-phase.md) |
| Promote future requirement | `/promote-requirement US-XXX --to-phase N` | See command docs |
| Check project status | `/project-status` | [How-to](../.claude/commands/how-to-use/check-status.md) |
| Update documentation | `/generate-docs` | [How-to](../.claude/commands/how-to-use/generate-documentation.md) |

**📖 Full command documentation:** [guides/COMMANDS-REFERENCE.md](guides/COMMANDS-REFERENCE.md)

**🤖 For AI:** Quick guides are 80-150 lines (vs 200-450 lines full docs). Read quick guide first for 60-70% token savings on common tasks.

---

## 📚 Documentation & Guides

### Core Documentation
- **[System Architecture](docs/ARCHITECTURE.md)** - Folder structure and data flow
- **[Input Files Guide](docs/INPUT-FILES-GUIDE.md)** - How to fill scope, backlog, technologies, constraints
- **[Generated Docs Guide](docs/GENERATED-DOCS-GUIDE.md)** - Understanding PRD, tech spec, architecture docs
- **[Workflows](docs/WORKFLOWS.md)** - Step-by-step examples for common scenarios
- **[Best Practices](docs/BEST-PRACTICES.md)** - Guidelines and success metrics
- **[Reuse Guide](docs/REUSE-GUIDE.md)** - Using this system for other projects

### User Guides
- **[Getting Started](guides/GETTING-STARTED.md)** - Detailed onboarding guide
- **[Commands Reference](guides/COMMANDS-REFERENCE.md)** - All slash commands explained
- **[Workflows & Best Practices](guides/WORKFLOWS-BEST-PRACTICES.md)** - Advanced workflow patterns
- **[FAQ & Troubleshooting](guides/FAQ-TROUBLESHOOTING.md)** - Common issues and solutions

### Integration & Migration
- **[Integration Guide](INTEGRATION-GUIDE.md)** - How everything works together
- **[Migration Guide](docs/MIGRATION-GUIDE.md)** - Upgrading from v2.0 to v3.0

---

## 📊 System Benefits

### For Developers
- ✅ Clear requirements and specifications
- ✅ Structured phase plans (1-4 months)
- ✅ Consistent documentation
- ✅ Defined coding standards
- ✅ Automated execution workflow

### For Project Managers
- ✅ Automated phase planning and execution
- ✅ Real-time progress visibility
- ✅ Risk identification
- ✅ Status reports on-demand
- ✅ Long-term phase tracking

### For Stakeholders
- ✅ Clear project vision
- ✅ Transparent progress
- ✅ Predictable delivery
- ✅ Risk awareness

### For Claude
- ✅ Always knows where to find data
- ✅ Consistent structure across projects
- ✅ Clear priorities and constraints
- ✅ Can work autonomously

---

## 🎉 Getting Started Checklist

Before starting your project:

- [ ] Filled `input/scope.md` completely
- [ ] Filled `input/backlog.md` with all features
- [ ] Filled `input/technologies.md` with tech stack
- [ ] Filled `input/constraints.md` with realistic constraints
- [ ] Reviewed `.CLAUDE.MD` coding standards
- [ ] Customized `rules/project-rules.md` if needed
- [ ] Ran `/init-project` successfully
- [ ] Reviewed generated documentation
- [ ] Reviewed Phase 1 plan
- [ ] Ready to run `/execute-work phase 1`!

---

## 📜 Version History

**v3.0.0 (Current)**
- Phase-based system (1-4 months per phase)
- 4 standard phases: Foundation, Core Features, Advanced Features, Polish & Launch
- Automated execution with `/execute-work phase N`
- Automatic progress tracking
- Bug tracking system
- Future backlog system

**[📖 Full changelog](../CHANGELOG.md)**

---

## 🔄 Reusing for Other Projects

This system is **fully reusable**! Copy to any new project:

```bash
# Copy structure
cp -r .project-management /path/to/new-project/
cp -r .claude /path/to/new-project/
cp .CLAUDE.MD /path/to/new-project/

# Clear old data, fill new input files, run /init-project
```

**📖 Complete reuse guide:** [docs/REUSE-GUIDE.md](docs/REUSE-GUIDE.md)

---

**Built with Claude Code**
**Designed for autonomous project management**
**Reusable across all projects**

---

For detailed information on any topic, see the links above or explore `.claude/commands/` for command documentation.

Happy coding! 🚀
