# System Overview - Complete File Map

**Purpose:** Quick reference showing ALL files in the system and their purpose.

---

## 📁 Complete File Structure

```
testProject/
│
├── 📄 README.md                                    # Project overview, quick start
├── 📄 .CLAUDE.MD                                   # Coding standards (UPDATED with integration)
│
├── 📂 .claude/
│   └── 📂 commands/                               # Slash commands for automation
│       ├── process-client-docs.md                 # 🆕 Extract requirements from client docs
│       ├── init-project.md                        # Initialize project with docs & Phase 1
│       ├── generate-docs.md                       # Generate/update all documentation
│       ├── execute-work.md                        # Execute work with automated phase planning
│       ├── update-progress.md                     # Update progress tracking (mostly automated)
│       └── project-status.md                      # Get comprehensive status report
│
└── 📂 .project-management/
    │
    ├── 📄 README.md                               # Complete system documentation (80+ sections)
    ├── 📄 INTEGRATION-GUIDE.md                    # ⭐ How everything works together
    ├── 📄 SYSTEM-OVERVIEW.md                      # This file - complete file map
    │
    ├── 📂 client-input/                           # 🆕 👤 CLIENT DOCUMENTS GO HERE
    │   ├── README.md                              # How to use client-input folder
    │   └── (add client docs: PDFs, Word, images)  # Your client's documents
    │
    ├── 📂 input/                                  # 👤 USER FILLS OR AUTO-GENERATED
    │   ├── scope.md                               # Project vision, goals, objectives
    │   ├── backlog.md                             # All features, user stories, priorities
    │   ├── technologies.md                        # Complete tech stack
    │   └── constraints.md                         # Timeline, budget, team constraints
    │
    ├── 📂 output/                                 # 🤖 CLAUDE GENERATES THESE
    │   ├── 📂 docs/                               # Generated documentation
    │   │   ├── prd.md                             # Product Requirements Document
    │   │   ├── technical-spec.md                  # Technical Specification
    │   │   ├── architecture.md                    # System Architecture Document
    │   │   └── api-spec.md                        # API Specification (optional)
    │   │
    │   ├── 📂 phases/                             # Phase plans
    │   │   ├── phase-1.md                         # Phase 1: Foundation (1-4 months)
    │   │   ├── phase-2.md                         # Phase 2: Core Features
    │   │   └── ...                                # Additional phases (3 & 4)
    │   │
    │   └── 📂 progress/                           # Progress tracking
    │       ├── current-status.md                  # Current project status & metrics
    │       ├── completed.md                       # Log of completed work
    │       └── blockers.md                        # Active blockers & issues
    │
    ├── 📂 templates/                              # Templates for documentation
    │   ├── prd-template.md                        # PRD template with placeholders
    │   ├── technical-spec-template.md             # Tech spec template
    │   ├── architecture-template.md               # Architecture doc template
    │   ├── phase-template.md                      # Phase plan template
    │   └── progress-template.md                   # Progress report template
    │
    └── 📂 rules/                                  # Project-specific rules
        └── project-rules.md                       # Custom rules (can override .CLAUDE.MD)
```

---

## 🎯 File Purposes - Quick Reference

### Documentation Files

| File | Purpose | Who Updates | When to Read |
|------|---------|-------------|--------------|
| `README.md` (root) | Project overview | Manual | Starting project |
| `.CLAUDE.MD` | Coding standards | Manual | While coding |
| `.project-management/README.md` | Complete PM system guide | Manual | Learning the system |
| `INTEGRATION-GUIDE.md` | How everything works | Manual | **READ FIRST** |
| `SYSTEM-OVERVIEW.md` | This file - file map | Manual | Quick reference |

### Input Files (You Fill)

| File | Purpose | When to Fill | When Claude Reads |
|------|---------|--------------|-------------------|
| `input/scope.md` | Project vision & goals | Once (start), update as needed | Planning, doc generation |
| `input/backlog.md` | All features & stories | Ongoing (add features) | Sprint planning, doc generation |
| `input/technologies.md` | Tech stack decisions | Once (start), update if stack changes | Doc generation, architecture |
| `input/constraints.md` | Timeline, budget, team | Once (start), update as needed | Sprint planning |

### Generated Files (Claude Creates)

| File | Purpose | When Generated | When to Read |
|------|---------|----------------|--------------|
| `output/docs/prd.md` | Product requirements | `/init-project`, `/generate-docs` | Planning features |
| `output/docs/technical-spec.md` | Technical details | `/init-project`, `/generate-docs` | **While implementing** |
| `output/docs/architecture.md` | System design | `/init-project`, `/generate-docs` | Planning architecture |
| `output/phases/phase-N.md` | Phase plan | `/execute-work phase N` | **During phase (1-4 months)** |
| `output/progress/current-status.md` | Project status | `/update-progress`, `/project-status` | Checking progress |
| `output/progress/completed.md` | Work log | `/update-progress` | Retrospectives |
| `output/progress/blockers.md` | Issues log | `/update-progress` | Identifying blockers |

### Command Files

| File | Purpose | When to Run |
|------|---------|-------------|
| `.claude/commands/process-client-docs.md` | 🆕 Extract from client docs | Before init, when client sends docs |
| `.claude/commands/init-project.md` | Initialize project | Once at start (after inputs ready) |
| `.claude/commands/generate-docs.md` | Update docs | When inputs change |
| `.claude/commands/execute-work.md` | Execute phase work | Start Phase 1, 2, 3, or 4 (1-4 months each) |
| `.claude/commands/update-progress.md` | Manual progress updates | As needed (automated in execute-work) |
| `.claude/commands/project-status.md` | Check status | Anytime |

### Template Files

| File | Purpose | Used By |
|------|---------|---------|
| `templates/prd-template.md` | PRD structure | `/generate-docs` |
| `templates/technical-spec-template.md` | Tech spec structure | `/generate-docs` |
| `templates/architecture-template.md` | Architecture structure | `/generate-docs` |
| `templates/phase-template.md` | Phase plan structure | `/execute-work` |
| `templates/progress-template.md` | Progress report structure | `/update-progress` |

---

## 🔍 Decision Tree: Which File Should I Check?

### I want to...

**Start a new project**
→ Fill: `input/*.md` (all 4 files)
→ Run: `/init-project`

**Know what to build**
→ Read: `input/scope.md`, `input/backlog.md`

**Know how to build it**
→ Read: `output/docs/technical-spec.md`

**See current phase tasks**
→ Read: `output/phases/phase-N.md` (latest)

**Follow coding standards**
→ Read: `.CLAUDE.MD`

**Understand the system**
→ Read: `INTEGRATION-GUIDE.md` ⭐

**Check project status**
→ Run: `/project-status`

**Execute phase work**
→ Run: `/execute-work phase N`

**Check progress**
→ Run: `/project-status` (progress tracked automatically)

**Update documentation**
→ Edit: `input/*.md` files
→ Run: `/generate-docs`

---

## 📊 Data Flow

```
USER INPUT
    ↓
input/scope.md
input/backlog.md
input/technologies.md
input/constraints.md
    ↓
    ↓ /init-project or /generate-docs
    ↓
GENERATED DOCS
    ↓
output/docs/prd.md
output/docs/technical-spec.md
output/docs/architecture.md
    ↓
    ↓ /execute-work phase N
    ↓
PHASE PLANS (Automated)
    ↓
output/phases/phase-N.md
    ↓
    ↓ Automated execution
    ↓
CODE (following .CLAUDE.MD)
    ↓
    ↓ /update-progress
    ↓
PROGRESS TRACKING
    ↓
output/progress/current-status.md
output/progress/completed.md
output/progress/blockers.md
```

---

## 🔄 Typical Workflow

### 🎨 Workflow A: Starting from Client Documents (Recommended)

### Phase 0: Client Input (Once)
```
1. Add client documents to client-input/
   ├─ project-brief.pdf
   ├─ requirements.docx
   ├─ mockups.png
   └─ timeline.txt
2. Run /process-client-docs      ← Extract requirements
3. Review generated input/*.md files
4. Edit/refine as needed
```

### Phase 1: Initialization (Once)
```
1. Run /init-project             ← Generate everything
   (input files already generated from Phase 0)
```

### Phase 2: Execution (4 Standard Phases, 1-4 months each)
```
Phase Execution:
1. Run /execute-work phase 1    ← Foundation (automated planning + execution)
2. Run /execute-work phase 2    ← Core Features
3. Run /execute-work phase 3    ← Advanced Features
4. Run /execute-work phase 4    ← Polish & Launch

Progress tracked automatically during execution
Check status: /project-status
```

### Phase 3: Maintenance (Ongoing)
```
As needed:
- Run /project-status           ← Check health
- Add new client docs to client-input/
- Run /process-client-docs      ← Merge new requirements
- Run /generate-docs            ← Update docs (if inputs change)
- Edit input/backlog.md         ← Add new features manually
```

---

### 📝 Workflow B: Manual Entry (Traditional)

### Phase 1: Setup (Once)
```
1. Manually fill input/scope.md           ← Define project
2. Manually fill input/backlog.md         ← List all features
3. Manually fill input/technologies.md    ← Choose tech stack
4. Manually fill input/constraints.md     ← Set limitations
5. Run /init-project                      ← Generate everything
```

### Phase 2: Execution (4 Standard Phases)
```
1. Run /execute-work phase 1    ← Foundation (1-4 months)
2. Run /execute-work phase 2    ← Core Features (1-4 months)
3. Run /execute-work phase 3    ← Advanced Features (1-4 months)
4. Run /execute-work phase 4    ← Polish & Launch (1-4 months)

Automated planning, execution, and progress tracking
```

---

## 🎨 Visual System Map

```
┌─────────────────────────────────────────────────────────────┐
│                        USER LAYER                            │
│  You interact with:                                          │
│  - input/*.md files (define project)                         │
│  - Slash commands (/init-project, /execute-work, etc.)      │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    AUTOMATION LAYER                          │
│  Claude reads:                                               │
│  - .claude/commands/*.md (command instructions)              │
│  - templates/*.md (how to generate docs)                     │
│  - INTEGRATION-GUIDE.md (what to read when)                  │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                     OUTPUT LAYER                             │
│  Claude generates:                                           │
│  - output/docs/*.md (documentation)                          │
│  - output/phases/*.md (phase plans)                          │
│  - output/progress/*.md (progress tracking)                  │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                   IMPLEMENTATION LAYER                       │
│  You code following:                                         │
│  - .CLAUDE.MD (coding standards)                             │
│  - output/docs/technical-spec.md (architecture)              │
│  - output/phases/phase-N.md (current phase)                  │
└─────────────────────────────────────────────────────────────┘
```

---

## ✅ System Health Check

Before using the system, verify:

**Required Files Exist:**
- [ ] `.CLAUDE.MD` exists and updated (v1.1+)
- [ ] `.project-management/README.md` exists
- [ ] `.project-management/INTEGRATION-GUIDE.md` exists
- [ ] All 4 input files exist (scope, backlog, technologies, constraints)
- [ ] All 5 command files exist in `.claude/commands/`
- [ ] All 5 template files exist in `templates/`
- [ ] Output directories exist (docs, phases, progress)

**Documentation is Linked:**
- [ ] Root README links to INTEGRATION-GUIDE.md
- [ ] .CLAUDE.MD mentions project management system
- [ ] Project management README links to INTEGRATION-GUIDE.md

**Clear Hierarchy:**
- [ ] Priority order is documented (scope → tech spec → .CLAUDE.MD)
- [ ] TodoWrite vs /execute-work distinction is clear
- [ ] Conflict resolution rules are defined

---

## 🚀 Quick Start Checklist

**For New Users:**
- [ ] Read `INTEGRATION-GUIDE.md` ⭐
- [ ] Read `.project-management/README.md`
- [ ] Fill all 4 input files
- [ ] Run `/init-project`
- [ ] Review generated documentation
- [ ] Start Sprint 1

**For Ongoing Projects:**
- [ ] Read current phase: `output/phases/phase-N.md`
- [ ] Check status: `/project-status`
- [ ] Execute phase work: `/execute-work phase N`
- [ ] Follow standards: `.CLAUDE.MD`

---

## 📈 System Statistics

**Total Files Created:** 20+
**Command Files:** 5
**Template Files:** 5
**Documentation Files:** 3 (README, INTEGRATION, OVERVIEW)
**Input Files:** 4
**Generated Files:** Variable (depends on project)

**Lines of Documentation:** 2000+
**Coverage:**
- ✅ Project planning
- ✅ Sprint management
- ✅ Documentation generation
- ✅ Progress tracking
- ✅ Coding standards
- ✅ Integration guide
- ✅ Complete workflows

---

## 🎯 Success Criteria

**This system is working well if:**
- ✅ Claude knows what to read without asking
- ✅ No conflicts between files
- ✅ Clear separation: planning vs implementation
- ✅ Automated documentation generation
- ✅ Consistent progress tracking
- ✅ Reusable on other projects
- ✅ Easy to onboard new team members

---

**Last Updated:** 2026-03-24
**System Version:** 3.0.0
**Status:** Production Ready ✅
