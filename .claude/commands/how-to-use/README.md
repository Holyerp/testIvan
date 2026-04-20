# Command Quick Guides - Index

**Purpose:** Fast reference for AI and developers - which command to use for what task.

**All documentation is in English only.**

---

## 💡 NEW in v3.1: Modular Structure Benefits

**For quick status checking:**
- ✅ Open `DASHBOARD.md` (live view, no commands!)
- ✅ 70% token savings - AI reads only relevant phase
- ✅ Auto-updates during work

**For existing projects:**
- Use `/migrate-to-modular` to upgrade to modular structure

[📖 Learn more](../../.project-management/guides/MODULAR-STRUCTURE-GUIDE.md)

---

## 🎯 Quick Decision Tree

```
START HERE
    │
    ├─ Have client documents (PDF, Word, etc.)?
    │   └─→ Use /process-client-docs
    │       └─→ [See: process-client-docs.md]
    │
    ├─ Project not initialized yet?
    │   └─→ Use /init-project
    │       └─→ [See: start-project.md]
    │
    ├─ Have existing project with old backlog.md?
    │   └─→ Use /migrate-to-modular
    │       └─→ [See: ../../.project-management/guides/MODULAR-STRUCTURE-GUIDE.md]
    │
    ├─ Need to add/edit scope (story, epic, phase)?
    │   └─→ Use /add-scope
    │       └─→ [See: add-requirement.md]
    │
    ├─ Planning features for future version (2.0, 3.0)?
    │   └─→ Use /add-backlog-requirement
    │       └─→ [See: add-backlog-requirement.md]
    │
    ├─ Found a bug that needs fixing?
    │   └─→ Use /add-bug
    │       └─→ [See: add-bug.md]
    │
    ├─ Ready to implement work (feature or bug fix)?
    │   └─→ Use /execute-work
    │       └─→ [See: execute-phase.md]
    │
    ├─ Need to check project status?
    │   └─→ Use /project-status
    │       └─→ [See: check-status.md]
    │
    └─ Need to update documentation?
        └─→ Use /generate-docs
            └─→ [See: generate-documentation.md]
```

---

## 📚 Quick Guides by Task

**Core Workflow Commands (with quick guides):**

| Task | Command | Guide | Lines | Time |
|------|---------|-------|-------|------|
| Start new project | `/init-project` | [start-project.md](./start-project.md) | ~120 | 5-10 min |
| Migrate to modular structure | `/migrate-to-modular` | [MODULAR-STRUCTURE-GUIDE.md](../../.project-management/guides/MODULAR-STRUCTURE-GUIDE.md) | ~150 | 2-5 min |
| Process client documents | `/process-client-docs` | [process-client-docs.md](./process-client-docs.md) | ~120 | 3-5 min |
| Add requirement (story/epic/phase) | `/add-scope add [type]` | [add-requirement.md](./add-requirement.md) | ~150 | 2-5 min |
| Add future requirement (v2.0, v3.0) | `/add-backlog-requirement` | [add-backlog-requirement.md](./add-backlog-requirement.md) | ~120 | 2-3 min |
| Add bug to roadmap | `/add-bug` | [add-bug.md](./add-bug.md) | ~120 | 2-3 min |
| Execute phase/epic/story | `/execute-work [scope]` | [execute-phase.md](./execute-phase.md) | ~150 | varies |
| Execute bug fix | `/execute-work bug BUG-XXX` | [execute-phase.md](./execute-phase.md) | ~150 | varies |
| Check project status | `/project-status` | [check-status.md](./check-status.md) | ~80 | 1 min |
| Generate/update docs | `/generate-docs` | [generate-documentation.md](./generate-documentation.md) | ~100 | 2-3 min |

**Helper Commands (no quick guide - see full command docs):**

| Task | Command | Full Docs | Why No Quick Guide |
|------|---------|-----------|-------------------|
| Promote future requirement | `/promote-requirement US-XXX --to-phase N` | [promote-requirement.md](../promote-requirement.md) | Helper command, rarely used standalone |
| Run tests manually | `/run-tests [type]` | [run-tests.md](../run-tests.md) | Automated in `/execute-work`, manual use is straightforward |
| Update progress manually | `/update-progress` | [update-progress.md](../update-progress.md) | Automated in `/execute-work` (v3.0+), legacy manual mode |

---

## 🤖 For AI: Reading Strategy

**Token-efficient approach:**

1. **Start here** - Read this index (~120 lines)
2. **For core workflow commands** - Read quick guide (80-150 lines)
3. **For helper commands** - Read full command docs directly (150-200 lines)
4. **If details needed** - Read full command docs (200-450 lines)

**Coverage:**
- 9 core workflow commands = Quick guides available
- 3 helper commands = Full docs only (simpler, rarely used)

**Estimated token savings: 60-70% for common tasks**

---

## 📖 Full Documentation

**Quick guides** are in this folder (`how-to-use/`)
**Full command docs** are in parent folder (`.claude/commands/`)

For complete system documentation:
- Main guide: `../../.project-management/README.md`
- Integration: `../../.project-management/INTEGRATION-GUIDE.md`
- File map: `../../.project-management/SYSTEM-OVERVIEW.md`

---

**Created:** 2026-04-02
**Purpose:** AI-optimized command reference
