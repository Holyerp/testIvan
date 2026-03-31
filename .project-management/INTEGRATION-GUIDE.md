# Integration Guide - How Everything Works Together

**Purpose:** This document explains how `.CLAUDE.MD`, project management system, and all other files work together without conflicts.

---

## 🎯 Quick Answer: What Should Claude Read?

Claude automatically reads files **in this priority order:**

### 1️⃣ Project Planning Context (if planning/starting work)
```
.project-management/input/scope.md          ← What are we building?
.project-management/input/backlog.md        ← What features do we need?
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
│    ├─ backlog.md        ← WHAT: All features & priorities      │
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
   - output/phases/phase-1.md (through phase-4.md)
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
   ├─ Create Product model
   ├─ Create POST /api/products endpoint
   ├─ Create product form UI
   ├─ Write unit tests
   └─ Write integration tests
5. Claude implements: Following .CLAUDE.MD standards
6. Progress tracked: Automatically during /execute-work
```

**What Claude reads:**
- ✅ Current phase file (for user story details)
- ✅ Technical spec (for implementation details)
- ✅ .CLAUDE.MD (for coding standards)
- ✅ Backlog (for acceptance criteria)
- ❌ NOT scope.md (already translated to phase)

---

### Scenario 3: Executing Next Phase

**User:** "Let's start Phase 2" or "/execute-work phase 2"

**Claude's Process:**
```
1. User runs: /execute-work phase 2
2. Claude automatically plans Phase 2:
   - input/backlog.md (available features for Core Features)
   - output/progress/current-status.md (what's done)
   - input/constraints.md (project timeline)
   - output/phases/phase-1.md (previous phase velocity)
3. Claude analyzes:
   - Remaining P0/P1 features for Core Features milestone
   - Overall velocity from Phase 1
   - Dependencies
4. Claude generates: output/phases/phase-2.md
5. Claude executes: Work items in Phase 2
6. Claude tracks: Progress automatically during execution
```

**What Claude reads:**
- ✅ Backlog (what's available)
- ✅ Constraints (timeline)
- ✅ Previous phase (velocity)
- ✅ Progress status
- ✅ .CLAUDE.MD (for implementation during execution)

---

### Scenario 4: Updating Progress

**User:** "/update-progress" (manual update, v3.0 tracks automatically)

**Claude's Process:**
```
1. Claude asks: "What did you complete?"
2. User responds: "Completed US-005 and US-006"
3. Claude reads:
   - output/phases/phase-N.md (current phase)
   - output/progress/current-status.md
   - output/progress/completed.md
4. Claude updates:
   - Marks US-005, US-006 as done in phase file
   - Adds entries to completed.md
   - Recalculates metrics in current-status.md
   - Updates phase completion percentage
5. Claude reports: Progress summary

Note: In v3.0, progress is tracked automatically during /execute-work.
This command is mainly for manual adjustments.
```

**What Claude reads:**
- ✅ Current phase file
- ✅ Progress files
- ❌ NOT .CLAUDE.MD (not coding, just tracking)
- ❌ NOT backlog (phase is already planned)

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
→ Automatic during `/execute-work`, or manual via `/update-progress`

**Generating docs?**
→ Read: `input/*.md`, `templates/*.md`

---

## 🔧 Tools Usage Rules

### TodoWrite
**When:** Implementing a single feature or user story
**Scope:** Task-level (hours to days)
**Example:** Breaking down US-005 into 5 implementation tasks

### /execute-work phase N
**When:** Starting or continuing work in a phase
**Scope:** Phase-level (1-4 months)
**Example:** Automated planning and execution of Phase 1 (Foundation)
**Note:** Replaces old /plan-sprint command with automated workflow

### /update-progress
**When:** Manual progress updates (mostly automated in v3.0)
**Scope:** Progress tracking
**Example:** Manually marking US-005 as complete if not using /execute-work

### /project-status
**When:** Need overall project health
**Scope:** Project-level
**Example:** Status check across all phases for stakeholders

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

**For Progress Updates:**
- [ ] Read current phase file
- [ ] Read progress files
- [ ] Update metrics accurately (or use automated tracking)

**For Documentation:**
- [ ] Read all input files
- [ ] Use templates correctly
- [ ] Replace all placeholders

---

## 🚨 Common Mistakes to Avoid

### ❌ DON'T:
- Don't read `.CLAUDE.MD` when planning (it's for coding)
- Don't read `input/backlog.md` when coding (read phase plan instead)
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
5. **Tracking progress?** → Automatic in `/execute-work`, or manual via `/update-progress`

**Everything has its place. No conflicts. No confusion.**

---

**Related Files:**
- [Main README](.project-management/README.md) - Complete system guide
- [.CLAUDE.MD](../.CLAUDE.MD) - Coding standards
- [Project Rules](rules/project-rules.md) - Project-specific overrides

---

**Last Updated:** 2026-03-30
**Version:** 3.0.0
