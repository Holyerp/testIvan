# Claude Project Management System - User Guide

**Phase-based project management with automated execution.**

**Version:** 3.0.0
**Last Updated:** 2026-03-27

---

## 📚 Documentation Index

This guide has been split into focused sections for better readability and AI optimization:

### 🚀 [Getting Started](guides/GETTING-STARTED.md)
**For new users** - Get up and running in 5 minutes
- Quick Start (5 minutes)
- What's New in v3.0
- Installation steps
- First-time setup
- Your first phase
- Quick reference

**Read this first if:** You're new to the system or starting a new project

---

### 📖 [Commands Reference](guides/COMMANDS-REFERENCE.md)
**For daily usage** - Complete command documentation
- `/init-project` - Initialize project
- `/execute-work` - Automated implementation
- `/run-tests` - Manual testing
- `/update-progress` - Progress tracking
- `/project-status` - Status reports
- `/process-client-docs` - Requirements extraction
- `/generate-docs` - Documentation regeneration

**Read this when:** You need to look up command syntax or options

---

### 🔄 [Workflows & Best Practices](guides/WORKFLOWS-BEST-PRACTICES.md)
**For optimization** - Learn the best approaches
- Core concepts (phases, plan mode, quality gates)
- 4 workflows (automated, controlled, manual, hybrid)
- 10 best practices
- Common patterns

**Read this when:** You want to optimize your workflow or learn advanced techniques

---

### ❓ [FAQ & Troubleshooting](guides/FAQ-TROUBLESHOOTING.md)
**For problem solving** - Answers and solutions
- Frequently asked questions
- Troubleshooting guides
- Migration from v2.0
- Common issues and fixes

**Read this when:** You encounter issues or have questions

---

## ⚡ Quick Reference

### Essential Commands
```bash
/init-project              # Initialize project (one-time)
/execute-work phase 1      # Automated phase execution
/execute-work story US-001 # Single story execution
/run-tests all             # Manual testing
/update-progress           # Manual progress update
/project-status            # Status report
```

---

### 5-Minute Quick Start

```bash
# 1. Initialize
/init-project
→ Choose tech stack: [1] Default / [2] AI / [3] Custom
→ Configure i18n: [1] Yes / [2] No

# 2. Execute
/execute-work phase 1
→ Choose mode: [1] Continuous / [2] Paused
→ Approve plan: [Yes]

# 3. Done!
# Claude automatically:
# ✅ Implements all stories
# ✅ Runs tests
# ✅ Creates commits
# ✅ Updates progress
```

---

### File Structure
```
.project-management/
├── input/              (your requirements)
│   ├── scope.md
│   ├── backlog.md
│   ├── technologies.md
│   └── constraints.md
├── output/             (generated)
│   ├── docs/          (PRD, tech spec, architecture)
│   ├── phases/        (phase-1.md to phase-4.md)
│   └── progress/      (status, completed, blockers)
├── rules/             (project-specific rules)
│   └── I18N-RULES.md (if i18n enabled)
├── defaults/          (tech stack templates)
└── guides/            (this documentation)
    ├── GETTING-STARTED.md
    ├── COMMANDS-REFERENCE.md
    ├── WORKFLOWS-BEST-PRACTICES.md
    └── FAQ-TROUBLESHOOTING.md

.claude/
├── commands/          (slash commands)
│   └── modules/       (command modules)
└── rules/             (coding standards)
```

---

### Quality Gates Checklist
```
Before story marked complete:
✅ All tests passing
✅ Coverage > 80%
✅ API codes tested (200/400/401/403/404/500)
✅ i18n translations (if enabled)
✅ SOLID & DRY followed
✅ Git commit created (NO AI credits)
✅ Progress updated
```

---

### Phase Structure
```
Project
  └── Phase (1-4 months)
      └── Epic (feature group)
          └── User Story (functionality)
              └── Task (implementation step)

4 Standard Phases:
1. Foundation & Setup (1-2 months)
2. Core Features (2-3 months)
3. Advanced Features (2 months)
4. Polish & Launch (1 month)
```

---

## 🎯 What's New in v3.0

| Feature | v2.0 | v3.0 |
|---------|------|------|
| **Planning** | Sprint-based (2 weeks) | Phase-based (1-4 months) |
| **Execution** | Manual | Automated (`/execute-work`) |
| **Testing** | Manual | Automatic + `/run-tests` |
| **Commits** | Manual | Automatic (NO AI credits) |
| **Progress** | Manual | Automatic |
| **Plan Mode** | Optional | Mandatory |
| **Tech Stack** | Manual | 3 options (Default/AI/Custom) |
| **i18n** | Manual | Setup during init |

---

## 📖 Where to Go Next

**If you're new:**
→ Read [Getting Started](guides/GETTING-STARTED.md)
→ Try the 5-minute quick start
→ Initialize your first project

**If you're implementing:**
→ Check [Commands Reference](guides/COMMANDS-REFERENCE.md)
→ Look up specific commands
→ Use `/execute-work` for automation

**If you're optimizing:**
→ Read [Workflows & Best Practices](guides/WORKFLOWS-BEST-PRACTICES.md)
→ Learn the 4 workflow patterns
→ Apply best practices

**If you have issues:**
→ Check [FAQ & Troubleshooting](guides/FAQ-TROUBLESHOOTING.md)
→ Find your specific issue
→ Apply the solution

---

## 🔗 Additional Resources

- **`.CLAUDE.MD`** - Core coding standards
- **`INTEGRATION-GUIDE.md`** - How the system works
- **`CHANGELOG.md`** - Version history
- **`RELEASE-NOTES-v3.0.md`** - Release information
- **`.claude/rules/*.md`** - Specialized coding rules
- **`.project-management/defaults/`** - Tech stack templates

---

## 🎉 Ready to Start!

1. Read [Getting Started](guides/GETTING-STARTED.md)
2. Run `/init-project`
3. Execute with `/execute-work phase 1`
4. Watch automated development in action! 🚀

---

**Version:** 3.0.0
**Last Updated:** 2026-03-27
**System:** Claude Project Management System (Phase-Based with Automation)
