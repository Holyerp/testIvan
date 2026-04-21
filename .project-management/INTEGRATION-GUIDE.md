# Integration Guide - How Everything Works Together

**Purpose:** This document explains how `.CLAUDE.MD`, project management system, and all other files work together without conflicts.

---

## 🎯 Quick Answer: What Should Claude Read?

Claude automatically reads files **in this priority order:**

### 1️⃣ Project Planning Context (if planning/starting work)
```
.project-management/input/scope.md          ← What are we building?
.project-management/input/backlog/          ← What features do we need? (modular)
.project-management/output/phases/phase-N.md  ← What's in current phase?
```

### 2️⃣ Technical Decisions (if implementing)
```
.project-management/output/docs/technical-spec.md  ← HOW to build (architecture, APIs)
.project-management/input/technologies.md          ← WHAT technologies to use
```

### 3️⃣ Coding Standards (while writing code)
```
.CLAUDE.MD                                  ← HOW to write code (standards, patterns)
.project-management/rules/project-rules.md  ← Project-specific overrides
```

---

## 📊 Visual Hierarchy

```
┌────────────────────────────────────────────────────────────────┐
│                    PROJECT LEVEL                                │
│  .project-management/input/                                     │
│    ├─ scope.md          ← WHAT: Project vision & goals         │
│    ├─ backlog/          ← WHAT: All features (modular by phase)│
│    ├─ technologies.md   ← WHAT: Tech stack decisions           │
│    └─ constraints.md    ← WHAT: Deadlines, budget, team        │
│                                                                  │
│  Commands: /init-project, /generate-docs, /execute-work         │
└────────────────────────────────────────────────────────────────┘
                              ↓
┌────────────────────────────────────────────────────────────────┐
│                    PHASE LEVEL                                  │
│  .project-management/output/phases/phase-N.md                  │
│    ├─ User Story US-001: Login Feature                         │
│    ├─ User Story US-002: Registration                          │
│    └─ Tasks for 1-4 month phase                                │
│                                                                  │
│  Commands: /execute-work phase N, /project-status              │
└────────────────────────────────────────────────────────────────┘
                              ↓
┌────────────────────────────────────────────────────────────────┐
│                  IMPLEMENTATION LEVEL                           │
│  TodoWrite (during coding session)                              │
│    ├─ Task: Create login API endpoint                          │
│    ├─ Task: Create login UI form                               │
│    ├─ Task: Write tests                                        │
│    └─ Task: Update documentation                               │
│                                                                  │
│  Tool: TodoWrite (marks in_progress/completed in real-time)    │
└────────────────────────────────────────────────────────────────┘
                              ↓
┌────────────────────────────────────────────────────────────────┐
│                     CODE LEVEL                                  │
│  .CLAUDE.MD - Coding standards while writing code              │
│    ├─ Naming conventions (camelCase, PascalCase)               │
│    ├─ Code organization (src/ structure)                       │
│    ├─ Security practices (validation, sanitization)            │
│    ├─ Testing requirements (80% coverage)                      │
│    └─ Git workflow (commit messages)                           │
│                                                                  │
│  .project-management/rules/project-rules.md (overrides)        │
└────────────────────────────────────────────────────────────────┘
```

---

## 🔀 When to Use What

### Scenario 1: Starting a New Project

**User:** "I want to build an e-commerce platform"

**Claude's Process:**
```
1. User fills: .project-management/input/*.md files
2. User runs: /init-project
3. Claude reads: ALL input files
4. Claude generates:
   - output/docs/prd.md, technical-spec.md, architecture.md
   - output/phases/phase-1.md through phase-4.md
5. Claude initializes: output/progress/*.md
```

**What Claude reads:**
- ✅ Input files (scope, backlog, technologies, constraints)
- ✅ Templates (to generate output)
- ❌ NOT .CLAUDE.MD yet (not coding yet)

---

### Scenario 2: Implementing a Feature from Phase

**User:** "Implement US-005: Create Product Listing" or "/execute-work phase 1"

**Claude's Process:**
```
1. Claude reads: output/phases/phase-N.md (find US-005)
2. Claude reads: output/docs/technical-spec.md (API specs, DB schema)
3. Claude reads: .CLAUDE.MD (coding standards)
4. Claude uses: TodoWrite to break down US-005
5. Claude implements: Following .CLAUDE.MD standards
6. Progress tracked: Automatically during /execute-work
```

**What Claude reads:**
- ✅ Current phase file (user story details)
- ✅ Technical spec (implementation details)
- ✅ .CLAUDE.MD (coding standards)
- ✅ Backlog (acceptance criteria)
- ❌ NOT scope.md (already translated to phase)

---

### Scenario 3: Executing Next Phase

**User:** "Let's start Phase 2" or "/execute-work phase 2"

**Claude's Process:**
```
1. Claude analyzes:
   - input/backlog/ (available features, modular)
   - output/progress/DASHBOARD.md (what's done)
   - output/phases/phase-1.md (previous velocity)
2. Claude generates: output/phases/phase-2.md
3. Claude executes: Work items in Phase 2
4. Progress tracked: Automatically
```

---

### Scenario 4: Updating Progress

**Note:** Progress is tracked automatically during `/execute-work` — the live view is `output/progress/DASHBOARD.md`.
Manual adjustments: edit the progress files directly. The `/update-progress` command was removed in v3.2.0.

---

## ⚠️ Conflict Resolution Rules

### Priority (highest to lowest):

1. **`input/scope.md` and `input/backlog/`**
   - **Source of truth** for WHAT to build
   - If scope says "no authentication", don't build authentication

2. **`output/docs/technical-spec.md`**
   - **Source of truth** for HOW to build
   - Generated from inputs, describes architecture

3. **`.project-management/rules/project-rules.md`**
   - **Project-specific overrides**
   - Can override `.CLAUDE.MD` standards
   - Example: "Use snake_case instead of camelCase"

4. **`.CLAUDE.MD`**
   - **General coding standards**
   - Applies unless overridden by project-rules.md

**Example Conflict:**
- `.CLAUDE.MD` says: "Use camelCase for variables"
- `rules/project-rules.md` says: "Use snake_case for variables"
- **Winner:** `rules/project-rules.md` (project-specific wins)

---

## 🧠 How Claude Decides What to Read

### Decision Tree:

```
User request received
    ↓
Is this project planning? (keywords: "execute work", "initialize", "status")
    ↓ YES
    Read: input files, output/phases/, output/progress/
    Skip: .CLAUDE.MD (not coding yet)
    ↓ NO
    ↓
Is this feature implementation? (keywords: "implement", "create", "add feature")
    ↓ YES
    Read: current phase, technical-spec.md, .CLAUDE.MD
    Use: TodoWrite for task breakdown
    ↓ NO
    ↓
Is this code modification? (keywords: "fix", "refactor", "update code")
    ↓ YES
    Read: existing code, .CLAUDE.MD
    Skip: phase files (ad-hoc change)
    ↓ NO
    ↓
Is this documentation update? (keywords: "regenerate docs", "update docs")
    ↓ YES
    Read: input files, templates
    Generate: output/docs/
```

---

## 📋 Cheat Sheet for Claude

### Before ANY action, ask yourself:

**Am I...**

**Planning the project?**
→ Read: `input/*.md`, run commands from `.claude/commands/`

**Executing a phase?**
→ Run: `/execute-work phase N` (automated planning + execution)

**Implementing a feature?**
→ Read: `output/phases/phase-N.md`, `output/docs/technical-spec.md`, `.CLAUDE.MD`

**Writing code?**
→ Read: `.CLAUDE.MD`, `rules/project-rules.md`, existing code

**Updating progress?**
→ Automatic during `/execute-work`. Manual edits: open DASHBOARD.md directly.

**Generating docs?**
→ Read: `input/*.md`, `templates/*.md`

---

## 📘 Command Selection Guide

### For AI: How to choose the right command

**Quick reference guides available in:** `.claude/commands/how-to-use/`

**Decision tree:**

1. **User provides external documents (PDF, Word, images)?**
   → Use `/process-client-docs` first

2. **Project not initialized yet?**
   → Use `/init-project` (one-time setup)

3. **User wants to add/change scope (story, epic, phase)?**
   → Use `/add-scope add [type]` or `/add-scope edit [type]`
   → For current phases (1-4) only

4. **User wants to add requirement for future version (2.0, 3.0)?**
   → Use `/add-backlog-requirement story|epic`
   → For post-launch features, not current phases

5. **User wants to track a bug?**
   → Use `/add-bug`

6. **User wants to implement work (phase, epic, story)?**
   → Use `/execute-work [scope]` (supports: phase N, epic X, story US-XXX, bug BUG-XXX)

7. **User wants status update?**
   → Use `/project-status`

8. **User wants to update docs?**
   → Use `/generate-docs`

**Token efficiency:**
- Quick guides: 80-150 lines
- Full command docs: 200-450 lines
- **Always read quick guide first** for 60-70% token savings

---

## 🔧 Tools Usage Rules

### TodoWrite
**When:** Implementing a single feature or user story
**Scope:** Task-level (hours to days)

### /execute-work phase N
**When:** Starting or continuing work in a phase
**Scope:** Phase-level (1-4 months)

### DASHBOARD.md (live progress)
**When:** Always current — auto-updated during `/execute-work`
**Scope:** Progress tracking (no command — `/update-progress` was removed in v3.2.0)

### /project-status
**When:** Need overall project health
**Scope:** Project-level

---

## ✅ Validation Checklist

Before Claude acts, verify:

**For Planning:**
- [ ] Read all input files in `.project-management/input/`
- [ ] Check current phase in `output/phases/`
- [ ] Review constraints (timeline for 1-4 month phases)

**For Execution:**
- [ ] Use `/execute-work phase N` for automated planning + execution
- [ ] Read current phase plan
- [ ] Read technical spec for API/architecture details
- [ ] Read `.CLAUDE.MD` for coding standards
- [ ] Use TodoWrite to track implementation tasks
- [ ] Progress tracked automatically

**For Documentation:**
- [ ] Read all input files
- [ ] Use templates correctly
- [ ] Replace all placeholders

---

## 🚨 Common Mistakes to Avoid

### ❌ DON'T:
- Don't read `.CLAUDE.MD` when planning (it's for coding)
- Don't read all of `input/backlog/` when coding (read the current phase file instead)
- Don't use `/execute-work` for individual tasks (use TodoWrite)
- Don't use TodoWrite for phase planning (use `/execute-work phase N`)
- Don't skip reading technical-spec.md when implementing
- Don't manually track progress when using /execute-work (it's automatic)

### ✅ DO:
- Read context files BEFORE acting
- Use appropriate tool for the scope (TodoWrite vs /execute-work)
- Follow document hierarchy (inputs → phases → implementation)
- Use automated execution via /execute-work for phase-level work
- Check for project-rules.md overrides

---

## 📖 Summary

**Simple Rule:**

1. **Planning project?** → Use `.project-management/` system with `/init-project`
2. **Executing phase work?** → Use `/execute-work phase N` (automated planning + execution)
3. **Implementing feature?** → Use TodoWrite + `.CLAUDE.MD`
4. **Writing code?** → Follow `.CLAUDE.MD` standards
5. **Tracking progress?** → Automatic in `/execute-work`; open DASHBOARD.md for the live view

**Everything has its place. No conflicts. No confusion.**

---

**Related Files:**
- [Main README](README.md) - Complete system guide
- [.CLAUDE.MD](../.CLAUDE.MD) - Coding standards
- [Project Rules](rules/project-rules.md) - Project-specific overrides

---

**Last Updated:** 2026-04-21
**Version:** 3.2.0
