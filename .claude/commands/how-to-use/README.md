# Command Quick Guides - Index

**Purpose:** Fast reference for AI and developers - which command to use for what task.

**All documentation is in English only.**

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

| Task | Command | Guide | Lines | Time |
|------|---------|-------|-------|------|
| Add requirement (story/epic/phase) | `/add-scope add [type]` | [add-requirement.md](./add-requirement.md) | ~150 | 2-5 min |
| Add future requirement (v2.0, v3.0) | `/add-backlog-requirement` | [add-backlog-requirement.md](./add-backlog-requirement.md) | ~120 | 2-3 min |
| Add bug to roadmap | `/add-bug` | [add-bug.md](./add-bug.md) | ~120 | 2-3 min |
| Start new project | `/init-project` | [start-project.md](./start-project.md) | ~120 | 5-10 min |
| Execute phase/epic/story | `/execute-work [scope]` | [execute-phase.md](./execute-phase.md) | ~150 | varies |
| Execute bug fix | `/execute-work bug BUG-XXX` | [execute-phase.md](./execute-phase.md) | ~150 | varies |
| Check project status | `/project-status` | [check-status.md](./check-status.md) | ~80 | 1 min |
| Generate/update docs | `/generate-docs` | [generate-documentation.md](./generate-documentation.md) | ~100 | 2-3 min |
| Process client documents | `/process-client-docs` | [process-client-docs.md](./process-client-docs.md) | ~120 | 3-5 min |

---

## 🤖 For AI: Reading Strategy

**Token-efficient approach:**

1. **Start here** - Read this index (100 lines)
2. **Read relevant quick guide** - Only the guide needed for current task (80-150 lines)
3. **Read full command docs** - Only if quick guide insufficient (200-450 lines)

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
