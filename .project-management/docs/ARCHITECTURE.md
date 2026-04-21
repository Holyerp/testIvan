# System Architecture

**Version:** 3.2.0
**Last Reviewed:** 2026-04-21
**Purpose:** Detailed system architecture and folder structure

---

## 📂 Folder Structure

```
.project-management/
│
├── input/                      # YOU fill these files
│   ├── scope.md               # Project scope, vision, goals
│   ├── backlog/                # Modular backlog (v3.1+)
│   │   ├── README.md              # Master index
│   │   ├── phase-1-foundation.md
│   │   ├── phase-2-core.md
│   │   ├── phase-3-advanced.md
│   │   ├── phase-4-polish.md
│   │   └── future.md              # Post-launch (v2.0/v3.0)
│   ├── backlog-future.md      # Legacy versioned future backlog (managed by /add-backlog-requirement)
│   ├── technologies.md        # Tech stack and tools
│   └── constraints.md         # Timeline, budget, team constraints
│
├── client-input/              # Client documents (optional)
│   ├── README.md             # Guide for document processing
│   └── [client docs...]      # PDFs, Word docs, images, etc.
│
├── output/                     # CLAUDE generates these
│   ├── docs/                  # Generated documentation
│   │   ├── prd.md            # Product Requirements Document
│   │   ├── technical-spec.md # Technical Specification
│   │   ├── architecture.md   # Architecture Document
│   │   └── api-spec.md       # API Specification (optional)
│   │
│   ├── phases/                # Phase plans
│   │   ├── phase-1.md        # Phase 1: Foundation (1-4 months)
│   │   ├── phase-2.md        # Phase 2: Core Features
│   │   ├── phase-3.md        # Phase 3: Advanced Features
│   │   └── phase-4.md        # Phase 4: Polish & Launch
│   │
│   ├── progress/              # Progress tracking (auto-updated by /execute-work)
│   │   ├── DASHBOARD.md           # Live at-a-glance view
│   │   ├── daily-summary.md       # Today's work
│   │   ├── weekly-report.md       # Weekly summary
│   │   ├── current-status.md      # Detailed status snapshot
│   │   ├── completed.md           # Completed work log
│   │   └── blockers.md            # Known blockers and issues
│   │
│   └── bugs/                  # Bug tracking
│       ├── bug-roadmap.md    # Active bugs by severity
│       └── bug-archive.md    # Fixed bugs history
│
├── templates/                  # Templates for generation
│   ├── prd-template.md
│   ├── technical-spec-template.md
│   ├── architecture-template.md
│   ├── phase-template.md
│   ├── phase-progress-template.md
│   ├── progress-template.md
│   ├── bug-template.md
│   └── backlog-future-template.md
│
├── defaults/                   # Default configurations
│   ├── default-stack.md       # HolyEstate default tech stack
│   └── stack-questions.md     # Stack selection questions
│
├── guides/                     # User guides
│   ├── GETTING-STARTED.md
│   ├── WORKFLOWS-BEST-PRACTICES.md
│   ├── COMMANDS-REFERENCE.md
│   ├── FAQ.md
│   └── TROUBLESHOOTING.md
│
├── docs/                       # Additional documentation
│   ├── ARCHITECTURE.md        # This file
│   ├── INPUT-FILES-GUIDE.md   # Input files detailed guide
│   ├── GENERATED-DOCS-GUIDE.md # Generated docs guide
│   ├── WORKFLOWS.md           # Workflow examples
│   ├── BEST-PRACTICES.md      # Best practices
│   ├── REUSE-GUIDE.md         # Reusing for other projects
│   └── MIGRATION-GUIDE.md     # v2.0 → v3.0 migration
│
└── rules/                      # Project-specific rules
    ├── README.md              # Rules overview
    ├── project-rules.md       # Custom project rules
    ├── I18N-RULES.md          # Internationalization rules
    ├── I18N-SETUP.md          # i18n setup guide
    └── TESTING-RULES.md       # Testing standards
```

---

## 🔧 Slash Commands Location

```
.claude/
├── commands/                   # Slash commands
│   ├── init-project.md        # Initialize project
│   ├── process-client-docs.md # Process client documents
│   ├── generate-docs.md       # Generate/update docs
│   ├── execute-work.md        # Execute work with automation
│   ├── add-scope.md           # Add/edit phases, epics, stories
│   ├── add-bug.md             # Add bugs to roadmap
│   ├── add-backlog-requirement.md # Add future requirements
│   ├── promote-requirement.md # Promote future to active
│   ├── project-status.md      # Show project status
│   ├── run-tests.md           # Manual test execution
│   │
│   ├── how-to-use/            # Quick guides (80-150 lines)
│   │   ├── README.md          # Command decision tree
│   │   ├── start-project.md
│   │   ├── add-requirement.md
│   │   ├── add-backlog-requirement.md
│   │   ├── add-bug.md
│   │   ├── execute-phase.md
│   │   ├── check-status.md
│   │   ├── generate-documentation.md
│   │   └── process-client-docs.md
│   │
│   └── modules/               # Command modules (AI optimization)
│       ├── execute-work-plan-mode.md
│       ├── execute-work-implementation.md
│       ├── execute-work-quality-gates.md
│       ├── init-project-stack-selection.md
│       ├── init-project-i18n-setup.md
│       ├── add-scope-input-parsing.md
│       ├── add-scope-renumbering.md
│       ├── add-scope-edit-mode.md
│       ├── project-status-data-collection.md
│       ├── project-status-calculation.md
│       ├── run-tests-execution.md
│       ├── run-tests-reporting.md
│       ├── process-client-docs-reading.md
│       └── process-client-docs-extraction.md
│
└── rules/                      # Code quality rules
    ├── code-quality.md         # SOLID, DRY, clean code
    ├── testing.md              # Test standards
    ├── git.md                  # Git conventions
    ├── database.md             # Database patterns
    └── stack-specific.md       # Stack-specific rules
```

---

## 🔄 Data Flow

```
Input Phase:
User → input/*.md files → /init-project → output/docs/*.md

OR

Client → client-input/*.pdf/docx → /process-client-docs → input/*.md → /init-project

Phase Planning:
output/docs/*.md + input/backlog/phase-N-*.md → /execute-work phase N → output/phases/phase-N.md

Execution:
phase-N.md → Implementation → Auto-testing → Auto-commit → Auto-progress update

Status Tracking:
output/progress/*.md ← Auto-updated during execution
                    ← Manual edits: open files directly
                    → /project-status (on-demand report)
```

---

## 🎯 Key Design Principles

### 1. Clear Separation
- **Input** (user-managed) vs **Output** (Claude-generated)
- **Commands** (what to do) vs **Rules** (how to do it)
- **Templates** (structure) vs **Generated files** (content)

### 2. AI Optimization
- Commands split into modules (<200 lines each)
- Quick guides for common tasks (80-150 lines)
- Full documentation for comprehensive reference
- Clear file path references throughout

### 3. Reusability
- Zero hardcoded project data
- All project info in `input/` folder
- Templates work for any project
- Commands work across all projects

### 4. Automation First
- Progress tracked automatically during execution
- Tests run automatically (second-to-last step)
- Commits created automatically (last step)
- Documentation regenerated on demand

---

## 📋 File Naming Conventions

### Input Files
- Lowercase with hyphens: `backlog-future.md`
- Descriptive names: `constraints.md` not `c.md`

### Output Files
- Phase files: `phase-1.md`, `phase-2.md`
- Progress files: `phase-1-progress.md`
- Bug files: `bug-roadmap.md`, `bug-archive.md`
- Doc files: `prd.md`, `technical-spec.md`, `architecture.md`

### Command Files
- Lowercase with hyphens: `add-scope.md`
- Verb-based names: `execute-work.md`, `generate-docs.md`
- Module prefix: `execute-work-plan-mode.md`

---

[← Back to README](../README.md)
