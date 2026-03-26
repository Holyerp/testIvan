# Integration Guide - How Everything Works Together

**Purpose:** This document explains how `.CLAUDE.MD`, project management system, and all other files work together without conflicts.

---

## 🎯 Quick Answer: What Should Claude Read?

Claude automatically reads files **in this priority order:**

### 1️⃣ Project Planning Context (if planning/starting work)
```
.project-management/input/scope.md          ← What are we building?
.project-management/input/backlog.md        ← What features do we need?
.project-management/output/sprints/sprint-N.md  ← What's in current sprint?
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
│    ├─ backlog.md        ← WHAT: All features & priorities      │
│    ├─ technologies.md   ← WHAT: Tech stack decisions           │
│    └─ constraints.md    ← WHAT: Deadlines, budget, team        │
│                                                                  │
│  Commands: /init-project, /generate-docs, /plan-sprint          │
└────────────────────────────────────────────────────────────────┘
                              ↓
┌────────────────────────────────────────────────────────────────┐
│                    SPRINT LEVEL                                 │
│  .project-management/output/sprints/sprint-N.md                │
│    ├─ User Story US-001: Login Feature                         │
│    ├─ User Story US-002: Registration                          │
│    └─ Tasks for 2-week sprint                                  │
│                                                                  │
│  Commands: /plan-sprint N, /update-progress, /project-status   │
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
                              ↓
┌────────────────────────────────────────────────────────────────┐
│                    DOCUMENTATION LEVEL                          │
│  .project-management/output/docs/                              │
│    ├─ prd.md              ← Generated: Product requirements    │
│    ├─ technical-spec.md   ← Generated: API specs, architecture │
│    └─ architecture.md     ← Generated: System design           │
│                                                                  │
│  Commands: /generate-docs (regenerates from input files)       │
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
   - output/docs/prd.md
   - output/docs/technical-spec.md
   - output/docs/architecture.md
   - output/sprints/sprint-1.md
5. Claude initializes: output/progress/*.md
```

**What Claude reads:**
- ✅ Input files (scope, backlog, technologies, constraints)
- ✅ Templates (to generate output)
- ❌ NOT .CLAUDE.MD yet (not coding yet)

---

### Scenario 2: Implementing a Feature from Sprint

**User:** "Implement US-005: Create Product Listing"

**Claude's Process:**
```
1. Claude reads: output/sprints/sprint-N.md (find US-005)
2. Claude reads: output/docs/technical-spec.md (API specs, DB schema)
3. Claude reads: .CLAUDE.MD (coding standards)
4. Claude uses: TodoWrite to break down US-005
   ├─ Create Product model
   ├─ Create POST /api/products endpoint
   ├─ Create product form UI
   ├─ Write unit tests
   └─ Write integration tests
5. Claude implements: Following .CLAUDE.MD standards
6. Claude runs: /update-progress (mark US-005 complete)
```

**What Claude reads:**
- ✅ Current sprint file (for user story details)
- ✅ Technical spec (for implementation details)
- ✅ .CLAUDE.MD (for coding standards)
- ✅ Backlog (for acceptance criteria)
- ❌ NOT scope.md (already translated to sprint)

---

### Scenario 3: Planning Next Sprint

**User:** "Let's plan Sprint 2"

**Claude's Process:**
```
1. User runs: /plan-sprint 2
2. Claude reads:
   - input/backlog.md (available features)
   - output/progress/current-status.md (what's done)
   - input/constraints.md (team capacity)
   - output/sprints/sprint-1.md (previous velocity)
3. Claude analyzes:
   - Remaining P0/P1 features
   - Team velocity from Sprint 1
   - Dependencies
4. Claude generates: output/sprints/sprint-2.md
5. Claude updates: output/progress/current-status.md
```

**What Claude reads:**
- ✅ Backlog (what's available)
- ✅ Constraints (team capacity)
- ✅ Previous sprint (velocity)
- ✅ Progress status
- ❌ NOT .CLAUDE.MD (not coding, just planning)

---

### Scenario 4: Updating Progress

**User:** "/update-progress"

**Claude's Process:**
```
1. Claude asks: "What did you complete?"
2. User responds: "Completed US-005 and US-006"
3. Claude reads:
   - output/sprints/sprint-N.md (current sprint)
   - output/progress/current-status.md
   - output/progress/completed.md
4. Claude updates:
   - Marks US-005, US-006 as done in sprint file
   - Adds entries to completed.md
   - Recalculates metrics in current-status.md
   - Updates sprint completion percentage
5. Claude reports: Progress summary
```

**What Claude reads:**
- ✅ Current sprint file
- ✅ Progress files
- ❌ NOT .CLAUDE.MD (not coding, just tracking)
- ❌ NOT backlog (sprint is already planned)

---

## ⚠️ Conflict Resolution Rules

### What if files conflict?

**Priority (highest to lowest):**

1. **`input/scope.md` and `input/backlog.md`**
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
Is this project planning? (keywords: "plan sprint", "initialize", "status")
    ↓ YES
    Read: input files, output/sprints/, output/progress/
    Skip: .CLAUDE.MD (not coding yet)
    ↓ NO
    ↓
Is this feature implementation? (keywords: "implement", "create", "add feature")
    ↓ YES
    Read: current sprint, technical-spec.md, .CLAUDE.MD
    Use: TodoWrite for task breakdown
    ↓ NO
    ↓
Is this code modification? (keywords: "fix", "refactor", "update code")
    ↓ YES
    Read: existing code, .CLAUDE.MD
    Skip: sprint files (ad-hoc change)
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

**Planning a sprint?**
→ Read: `input/backlog.md`, `output/progress/`, `input/constraints.md`

**Implementing a feature?**
→ Read: `output/sprints/sprint-N.md`, `output/docs/technical-spec.md`, `.CLAUDE.MD`

**Writing code?**
→ Read: `.CLAUDE.MD`, `rules/project-rules.md`, existing code

**Updating progress?**
→ Read: `output/sprints/`, `output/progress/`

**Generating docs?**
→ Read: `input/*.md`, `templates/*.md`

---

## 🔧 Tools Usage Rules

### TodoWrite
**When:** Implementing a single feature or user story
**Scope:** Task-level (hours to days)
**Example:** Breaking down US-005 into 5 implementation tasks

### /plan-sprint
**When:** Planning a 2-week sprint
**Scope:** Sprint-level (weeks)
**Example:** Selecting 8-10 user stories for Sprint 2

### /update-progress
**When:** Logging completed work
**Scope:** Progress tracking
**Example:** Marking US-005 as complete, adding to progress log

### /project-status
**When:** Need overall project health
**Scope:** Project-level
**Example:** Weekly status check for stakeholders

---

## ✅ Validation Checklist

Before Claude acts, verify:

**For Planning:**
- [ ] Read all input files in `.project-management/input/`
- [ ] Check current sprint in `output/sprints/`
- [ ] Review constraints (team capacity, timeline)

**For Implementation:**
- [ ] Read current sprint plan
- [ ] Read technical spec for API/architecture details
- [ ] Read `.CLAUDE.MD` for coding standards
- [ ] Use TodoWrite to track implementation tasks

**For Progress Updates:**
- [ ] Read current sprint file
- [ ] Read progress files
- [ ] Update metrics accurately

**For Documentation:**
- [ ] Read all input files
- [ ] Use templates correctly
- [ ] Replace all placeholders

---

## 🚨 Common Mistakes to Avoid

### ❌ DON'T:
- Don't read `.CLAUDE.MD` when planning (it's for coding)
- Don't read `input/backlog.md` when coding (read sprint plan instead)
- Don't use `/plan-sprint` for individual tasks (use TodoWrite)
- Don't use TodoWrite for sprint planning (use `/plan-sprint`)
- Don't skip reading technical-spec.md when implementing

### ✅ DO:
- Read context files BEFORE acting
- Use appropriate tool for the scope (TodoWrite vs /plan-sprint)
- Follow document hierarchy (inputs → sprints → implementation)
- Update progress after completing sprint tasks
- Check for project-rules.md overrides

---

## 📖 Summary

**Simple Rule:**

1. **Planning project/sprint?** → Use `.project-management/` system
2. **Implementing feature?** → Use TodoWrite + `.CLAUDE.MD`
3. **Writing code?** → Follow `.CLAUDE.MD` standards
4. **Tracking progress?** → Use `/update-progress`

**Everything has its place. No conflicts. No confusion.**

---

**Related Files:**
- [Main README](.project-management/README.md) - Complete system guide
- [.CLAUDE.MD](../.CLAUDE.MD) - Coding standards
- [Project Rules](rules/project-rules.md) - Project-specific overrides

---

**Last Updated:** 2026-03-24
**Version:** 1.0
