# Claude Project Management System

**Version:** 3.0.0
**Created:** 2026-03-24
**Purpose:** Autonomous project planning, documentation, and progress tracking system for Claude Code

---

## 📋 Table of Contents

1. [Overview](#overview)
2. [Quick Start](#quick-start)
3. [**Integration Guide**](INTEGRATION-GUIDE.md) ⭐ **How everything works together**
4. [System Architecture](#system-architecture)
5. [How to Use](#how-to-use)
6. [Slash Commands](#slash-commands)
7. [Input Files Guide](#input-files-guide)
8. [Generated Documentation](#generated-documentation)
9. [Workflow Examples](#workflow-examples)
10. [Best Practices](#best-practices)
11. [Troubleshooting](#troubleshooting)
12. [Reusing for Other Projects](#reusing-for-other-projects)

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

---

## 🚀 Quick Start

### 🎨 **NEW: Start from Client Documents** (Recommended)

**Perfect for agencies and freelancers getting project briefs from clients!**

#### Step 1: Add Client Documents
```bash
# Copy client's documents to client-input folder
.project-management/client-input/
├── project-brief.pdf
├── requirements.docx
├── mockups.png
└── timeline.txt
```

Supported: PDF, Word (.docx), Text (.txt, .md), Images (wireframes/mockups)

#### Step 2: Process Documents
```bash
/process-client-docs
```

Claude will:
- 📄 Read all documents (PDFs, Word, images, text)
- 🔍 Extract requirements, features, goals
- ✍️ Generate `scope.md`, `backlog.md`, `technologies.md`, `constraints.md`
- 💡 Identify gaps and ask clarifying questions

#### Step 3: Review & Refine
```bash
# Review generated files
.project-management/input/
├── scope.md          # ✅ Auto-generated from client docs
├── backlog.md        # ✅ Auto-generated user stories
├── technologies.md   # ✅ Auto-suggested tech stack
└── constraints.md    # ✅ Auto-extracted timeline/budget

# Edit if needed, then initialize
/init-project
```

**[📖 Full Guide: client-input/README.md](client-input/README.md)**

---

### 📝 **Classic: Manual Entry**

#### Step 1: Fill Input Files Manually

Navigate to `.project-management/input/` and fill out these files:

```bash
.project-management/input/
├── scope.md          # Your project vision, goals, and scope
├── backlog.md        # All features, user stories, and tasks
├── technologies.md   # Tech stack you'll use
└── constraints.md    # Timeline, budget, team constraints
```

Each file has **detailed templates and examples** - just replace the placeholders with your project info.

#### Step 2: Initialize Project

Run the initialization command:

```bash
/init-project
```

Claude will:
- Read all your input files
- Generate complete documentation
- Create Phase 1 plan
- Set up progress tracking

#### Step 3: Start Development

Execute work using automated phase workflow:

```bash
/execute-work phase 1    # Start Phase 1 with automated execution
/project-status          # Check current status anytime
# Progress tracked automatically during execution
```

That's it! Claude now manages your project autonomously.

---

## 🏗️ System Architecture

### Folder Structure

```
.project-management/
│
├── input/                      # YOU fill these files
│   ├── scope.md               # Project scope, vision, goals
│   ├── backlog.md             # Features, user stories, epics
│   ├── technologies.md        # Tech stack and tools
│   └── constraints.md         # Timeline, budget, team constraints
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
│   │   └── ...
│   │
│   └── progress/              # Progress tracking
│       ├── current-status.md # Current project status
│       ├── completed.md      # Completed work log
│       └── blockers.md       # Known blockers and issues
│
├── templates/                  # Templates for generation
│   ├── prd-template.md
│   ├── technical-spec-template.md
│   ├── architecture-template.md
│   ├── sprint-template.md
│   └── progress-template.md
│
└── rules/                      # Project-specific rules
    └── project-rules.md       # Custom rules for this project

.claude/
└── commands/                   # Slash commands
    ├── init-project.md        # Initialize project
    ├── generate-docs.md       # Generate/update docs
    ├── execute-work.md        # Execute work with automated planning
    ├── add-scope.md           # Add/edit phases, epics, stories
    ├── update-progress.md     # Update progress
    └── project-status.md      # Show project status
```

---

## 📖 How to Use

### The Complete Workflow

#### 🎨 Option A: Start from Client Documents (Recommended for Agencies)

```
1. Client Input Phase
   ├─ Receive documents from client
   ├─ Add to .project-management/client-input/
   │  ├─ project-brief.pdf
   │  ├─ requirements.docx
   │  ├─ mockups.png
   │  └─ any other documents
   └─ Run: /process-client-docs
       ├─ Claude extracts requirements
       ├─ Generates input/*.md files
       └─ Asks clarifying questions

2. Review & Refine Phase
   ├─ Review generated input files
   ├─ Answer clarifying questions
   ├─ Edit/refine as needed
   ├─ Add missing information
   └─ Review .CLAUDE.MD for coding standards

3. Initialization
   └─ Run: /init-project
       ├─ Generates all documentation
       ├─ Creates Phase 1 plan (1-4 months)
       └─ Initializes progress tracking

4. Phase Execution (4 Phases Total)
   ├─ Phase 1: Foundation
   │  └─ Run: /execute-work phase 1 (automated execution)
   │
   ├─ Phase 2: Core Features
   │  └─ Run: /execute-work phase 2
   │
   ├─ Phase 3: Advanced Features
   │  └─ Run: /execute-work phase 3
   │
   └─ Phase 4: Polish & Launch
       └─ Run: /execute-work phase 4

5. Ongoing Maintenance
   ├─ Add new client documents as they arrive
   ├─ Re-run: /process-client-docs (merges with existing)
   ├─ Run: /add-scope (add/edit phases, epics, stories)
   ├─ Run: /generate-docs (regenerate docs)
   └─ Track blockers in progress/blockers.md
```

#### 📝 Option B: Manual Entry

```
1. Setup Phase
   ├─ Manually fill input files (scope, backlog, technologies, constraints)
   ├─ Review .CLAUDE.MD for coding standards
   └─ Customize rules/project-rules.md if needed

2. Initialization
   └─ Run: /init-project
       ├─ Generates all documentation
       ├─ Creates Phase 1 plan (1-4 months)
       └─ Initializes progress tracking

3. Phase Execution Cycle (4 Standard Phases)
   ├─ Phase 1: Foundation
   │  └─ Run: /execute-work phase 1 (automated execution)
   │
   ├─ Phase 2: Core Features
   │  └─ Run: /execute-work phase 2
   │
   ├─ Phase 3: Advanced Features
   │  └─ Run: /execute-work phase 3
   │
   └─ Phase 4: Polish & Launch
       └─ Run: /execute-work phase 4

4. Ongoing Maintenance
   ├─ Run: /add-scope (add/edit phases, epics, stories as scope changes)
   ├─ Run: /generate-docs (regenerate docs)
   └─ Progress tracked automatically during execution
```

---

## 🎮 Slash Commands

### `/process-client-docs` 🆕
**Purpose:** Extract requirements from client documents and generate input files

**When to use:**
- When starting with client-provided documents (PDFs, Word, images)
- Before filling input files manually
- To update requirements from new client documents

**What it does:**
- Reads ALL files from `.project-management/client-input/`
- Analyzes PDFs, Word documents, text files, images (mockups/wireframes)
- Extracts project vision, features, constraints, timeline
- Generates or updates:
  - `input/scope.md` - Project vision and goals
  - `input/backlog.md` - User stories with priorities
  - `input/technologies.md` - Tech stack (suggested or extracted)
  - `input/constraints.md` - Timeline, budget, team
- Identifies gaps and asks clarifying questions

**Example:**
```bash
# 1. Add client documents
cp client-brief.pdf .project-management/client-input/
cp requirements.docx .project-management/client-input/
cp mockups.png .project-management/client-input/

# 2. Process documents
/process-client-docs

# Claude generates input files from documents
# Review and edit as needed, then run /init-project
```

**Output:**
- Generated/updated `.project-management/input/*.md` files
- Summary report with extracted info and questions
- List of items needing clarification

**[Full Documentation: client-input/README.md](client-input/README.md)**

---

### `/init-project`
**Purpose:** Initialize a new project with all documentation and first phase

**When to use:**
- Starting a new project
- After filling all input files

**What it does:**
- Reads all input files (scope, backlog, technologies, constraints)
- Generates PRD, Technical Spec, Architecture docs
- Creates Sprint 1 plan
- Initializes progress tracking

**Example:**
```bash
/init-project
```

**Output:**
- `.project-management/output/docs/prd.md`
- `.project-management/output/docs/technical-spec.md`
- `.project-management/output/docs/architecture.md`
- `.project-management/output/phases/phase-1.md`
- `.project-management/output/progress/*.md`

---

### `/add-scope`
**Purpose:** Add or edit phases, epics, or stories with automatic renumbering

**When to use:**
- Adding new features/phases to an existing project
- Modifying scope after client feedback
- Restructuring phases or epics
- Updating story details (points, criteria, priority)

**What it does:**
- Adds new phases, epics, or stories to project documentation
- Edits existing phases, epics, or stories
- Automatically renumbers all affected items (phases, epics)
- Updates cross-references across all project files
- Updates backlog and progress metrics
- Runs integrity checks after changes
- Optionally updates PRD, tech-spec, architecture docs

**Example:**
```bash
# Add new phase at position 2 (existing phases shift)
/add-scope add phase 2

# Add epic to Phase 1 from a file
/add-scope add epic 1 --from docs/notification-epic.md

# Add story to Epic 2 in Phase 1
/add-scope add story 1 2

# Edit an existing story
/add-scope edit story US-005

# Edit a phase
/add-scope edit phase 3
```

**Output:**
- New/updated phase files
- Updated `backlog.md`
- Updated progress metrics
- Integrity check report
- Optional: updated PRD, tech-spec, architecture

---

### `/generate-docs`
**Purpose:** Generate or update project documentation

**When to use:**
- After modifying input files
- When docs are out of sync
- To regenerate specific documents

**What it does:**
- Reads current input files
- Updates or creates documentation
- Maintains consistency across docs

**Example:**
```bash
/generate-docs
```

**Output:**
- Updated documentation in `.project-management/output/docs/`

---

### `/execute-work phase [number]`
**Purpose:** Execute work in a phase with automated planning and implementation

**When to use:**
- Starting Phase 1, 2, 3, or 4
- After completing previous phase
- Ready to begin development work

**What it does:**
- Automatically plans the phase (1-4 months duration)
- Analyzes remaining backlog for the phase
- Selects appropriate work items based on phase milestone
- Executes work with continuous or paused mode
- Tracks progress automatically

**Example:**
```bash
/execute-work phase 1    # Start Foundation phase
/execute-work phase 2    # Start Core Features phase
```

**Output:**
- `.project-management/output/phases/phase-N.md`
- Automatic progress tracking during execution
- Updated `current-status.md` with phase progress

---

### `/update-progress`
**Purpose:** Update project progress tracking (used mainly for manual updates)

**When to use:**
- Manual progress updates if not using automated execution
- After completing stories/tasks outside of /execute-work
- When blockers arise
- Before stakeholder meetings

**Note:** With v3.0 automated execution via `/execute-work phase N`, progress is tracked automatically during work. This command is now primarily for manual adjustments.

**What it does:**
- Asks about recent completions
- Updates progress metrics
- Tracks blockers
- Calculates velocity

**Example:**
```bash
/update-progress
```

Claude will ask you:
- What has been completed?
- What's in progress?
- Any blockers?

**Output:**
- Updated `current-status.md`
- Updated `completed.md`
- Updated `blockers.md`
- Updated current phase file

---

### `/project-status`
**Purpose:** Get comprehensive project status report

**When to use:**
- Weekly status checks
- Before meetings
- When asked "how's the project?"
- To identify issues

**What it does:**
- Analyzes all project data
- Calculates metrics
- Identifies trends
- Provides recommendations

**Example:**
```bash
/project-status
```

**Output:**
- Comprehensive status report showing:
  - Overall progress
  - Current phase status
  - Blockers and risks
  - Quality metrics
  - Timeline adherence
  - Recommendations

---

## 📝 Input Files Guide

### 1. scope.md
**Purpose:** Define what the project is and why it exists

**Key Sections:**
- **Project Vision:** The big picture
- **Target Audience:** Who uses this?
- **Core Objectives:** What must be achieved?
- **Success Criteria:** How do we measure success?
- **Out of Scope:** What we're NOT building
- **Phases:** Standard 4-phase structure (Foundation, Core Features, Advanced Features, Polish & Launch)

**Tips:**
- Be specific and clear
- Focus on the "why" not just the "what"
- Define success measurably
- Explicitly state what's out of scope

---

### 2. backlog.md
**Purpose:** List ALL features, user stories, and requirements

**Key Sections:**
- **Epics:** Large feature groups
- **User Stories:** "As a [user], I want [feature], so that [benefit]"
- **Acceptance Criteria:** Clear, testable criteria
- **Story Points:** Effort estimates (Fibonacci: 1,2,3,5,8,13,21)
- **Priorities:** P0 (critical), P1 (high), P2 (medium), P3 (low)
- **Dependencies:** What must be done first

**Tips:**
- Write clear, specific user stories
- Include acceptance criteria for everything
- Estimate realistically
- Note all dependencies
- Prioritize ruthlessly

**Story Point Reference:**
- 1-2 points: Few hours
- 3 points: Half day
- 5 points: 1-2 days
- 8 points: 2-4 days
- 13 points: 1 week
- 21+ points: Break it down further

Note: In v3.0 phase-based system, story points help estimate phase duration (1-4 months per phase)

---

### 3. technologies.md
**Purpose:** Define the complete technology stack

**Key Sections:**
- **Frontend:** Framework, libraries, tools
- **Backend:** Runtime, framework, database
- **DevOps:** Hosting, CI/CD, monitoring
- **Testing:** Test frameworks, coverage goals
- **Third-party:** External services and APIs

**Tips:**
- Be specific about versions
- Explain why you chose each technology
- List all dependencies
- Include development tools
- Specify performance targets

---

### 4. constraints.md
**Purpose:** Define limitations and boundaries

**Key Sections:**
- **Timeline:** Deadlines, phase milestones (1-4 months per phase)
- **Budget:** Development and operational costs
- **Team:** Size, availability, skills
- **Technical:** Infrastructure, technology restrictions
- **Compliance:** Legal, security requirements
- **Scope:** Must-have vs nice-to-have

**Tips:**
- Be realistic and honest
- Document all constraints
- Identify risks early
- Plan for contingencies
- Update as constraints change

---

## 📚 Generated Documentation

### PRD (Product Requirements Document)
**Audience:** Product managers, stakeholders, developers

**Contains:**
- Product vision and goals
- User personas
- Feature requirements
- User stories
- Success metrics
- Timeline and phase structure (4 standard phases)

**Use for:**
- Alignment on what to build
- Prioritization decisions
- Stakeholder communication

---

### Technical Specification
**Audience:** Developers, architects

**Contains:**
- Technology stack details
- System architecture
- Database schema
- API specifications
- Security implementation
- Testing strategy

**Use for:**
- Development guidance
- Technical decisions
- Code review standards
- Onboarding new developers

---

### Architecture Document
**Audience:** Architects, senior developers

**Contains:**
- System architecture diagrams
- Component design
- Data flow
- Infrastructure plan
- Security architecture
- Performance strategy

**Use for:**
- Architectural decisions
- System design reviews
- Scaling planning
- Technical debt assessment

---

## 💡 Workflow Examples

### Example 1: Starting a New Project

```bash
# Step 1: Fill input files
# Edit .project-management/input/scope.md
# Edit .project-management/input/backlog.md
# Edit .project-management/input/technologies.md
# Edit .project-management/input/constraints.md

# Step 2: Initialize
/init-project

# Claude generates:
# ✅ Complete documentation
# ✅ Phase 1 plan (1-4 months: Foundation)
# ✅ Progress tracking setup

# Step 3: Start automated phase execution
/execute-work phase 1

# Claude automatically:
# ✅ Plans Phase 1 work
# ✅ Executes implementation
# ✅ Tracks progress continuously

# Step 4: Check status anytime
/project-status

# Step 5: Continue to next phases
/execute-work phase 2    # Core Features
/execute-work phase 3    # Advanced Features
/execute-work phase 4    # Polish & Launch
```

---

### Example 2: Executing Next Phase

```bash
# Phase 1 (Foundation) completed

# Step 1: Start next phase with automated execution
/execute-work phase 2

# Claude automatically:
# ✅ Plans Phase 2 (Core Features, 1-4 months)
# ✅ Analyzes remaining backlog
# ✅ Selects appropriate work for Core Features milestone
# ✅ Executes implementation
# ✅ Tracks progress continuously

# Step 2: Monitor progress
/project-status
# Shows Phase 2 progress and overall project status

# Step 3: Continue through all phases
/execute-work phase 3    # Advanced Features
/execute-work phase 4    # Polish & Launch
```

---

### Example 3: Scope Change Mid-Project

```bash
# New feature requested!

# Step 1: Update backlog.md
# Add new user stories with priorities

# Step 2: Optionally update scope.md
# If vision/objectives changed

# Step 3: Regenerate documentation
/generate-docs

# Claude updates:
# ✅ PRD with new features
# ✅ Technical spec with new requirements
# ✅ Architecture if needed

# Step 4: Continue execution if needed
# Continue current phase or start next phase:
/execute-work phase 2
# New features automatically incorporated into phase planning

# Step 5: Update status
/project-status
# See impact on timeline and phase milestones
```

---

### Example 4: Handling a Blocker

```bash
# Discovered critical blocker!

# Step 1: Update progress
/update-progress

# Claude asks about blockers
# You describe: "Payment API down, can't test checkout"

# Claude:
# ✅ Logs blocker in blockers.md
# ✅ Updates current phase status
# ✅ Marks affected tasks as blocked
# ✅ Suggests mitigation

# Step 2: Check impact
/project-status

# Shows:
# 🔴 1 Critical Blocker
# ⚠️ Phase milestone at risk
# 💡 Recommendations: work on unblocked items

# Step 3: When resolved
/update-progress
# Update blocker status to resolved
```

---

## ✅ Best Practices

### Input Files
1. **Fill completely** - Don't leave sections empty
2. **Be specific** - Vague requirements = vague results
3. **Update regularly** - Keep scope and backlog current
4. **Prioritize ruthlessly** - Not everything can be P0
5. **Document constraints** - Better to over-communicate limitations

### Phase Planning
1. **Plan realistic milestones** - Phases last 1-4 months, not 2 weeks
2. **Respect overall velocity** - Use past phase data to plan realistically
3. **Balance risk** - Mix high-risk and low-risk items
4. **Check dependencies** - Don't pick work that can't be completed
5. **Set clear milestones** - Each phase should have a clear milestone goal
6. **Use automated planning** - Let `/execute-work phase N` handle planning automatically

### Progress Tracking
1. **Automated tracking** - With v3.0, progress tracked automatically during `/execute-work`
2. **Be honest** - Don't hide problems or delays
3. **Document blockers** - Report issues as soon as they arise
4. **Celebrate wins** - Completed work logged automatically
5. **Monitor phases** - Track progress across 1-4 month phases, not weekly

### Documentation
1. **Keep in sync** - Regenerate docs when inputs change
2. **Use as reference** - Refer to docs during development
3. **Update don't recreate** - Preserve manual additions
4. **Version control** - Commit docs with code
5. **Share widely** - Make sure team has access

---

## 🐛 Troubleshooting

### Issue: Claude doesn't know where files are

**Solution:**
- Make sure you're using slash commands (they include file paths)
- Claude knows these locations:
  - Input: `.project-management/input/`
  - Output: `.project-management/output/`
  - Templates: `.project-management/templates/`

---

### Issue: Generated docs have placeholders

**Solution:**
- Fill ALL sections in input files
- Don't leave examples or "TODO" markers
- Run `/generate-docs` again after filling inputs properly

---

### Issue: Phase plan seems unrealistic

**Solution:**
- Check `constraints.md` - is timeline realistic for 1-4 month phases?
- Review velocity from previous phases
- Adjust story point estimates in `backlog.md`
- Phase planning is automated via `/execute-work phase N`

---

### Issue: Progress not updating

**Solution:**
- Run `/update-progress` explicitly
- Answer Claude's questions completely
- Check that files in `output/progress/` are writable
- Run `/project-status` to see calculated progress

---

### Issue: Documentation is outdated

**Solution:**
- Update relevant input files first
- Run `/generate-docs` to regenerate
- Claude will update docs based on current inputs

---

## 🔄 Reusing for Other Projects

This entire system is **fully reusable**! Here's how to use it for a new project:

### Method 1: Copy to New Project

```bash
# Copy the entire structure
cp -r .project-management /path/to/new-project/
cp -r .claude /path/to/new-project/
cp .CLAUDE.MD /path/to/new-project/

# Navigate to new project
cd /path/to/new-project

# Clear old data
rm .project-management/output/docs/*
rm .project-management/output/phases/*
rm .project-management/output/progress/*

# Fill new input files
# Edit .project-management/input/scope.md (new project info)
# Edit .project-management/input/backlog.md (new features)
# Edit .project-management/input/technologies.md (new tech stack)
# Edit .project-management/input/constraints.md (new constraints)

# Initialize new project
/init-project

# Done! System is ready for new project
```

---

### Method 2: Create Template

```bash
# Create a template directory
mkdir ~/project-management-template
cp -r .project-management ~/project-management-template/
cp -r .claude ~/project-management-template/
cp .CLAUDE.MD ~/project-management-template/

# Clear output files in template
rm ~/project-management-template/.project-management/output/docs/*
rm ~/project-management-template/.project-management/output/phases/*
rm ~/project-management-template/.project-management/output/progress/*

# For each new project:
cp -r ~/project-management-template/* /path/to/new-project/
# Then fill input files and run /init-project
```

---

### What to Customize per Project

**Always customize:**
- ✅ All files in `input/` directory
- ✅ `rules/project-rules.md` (if project has unique rules)

**Usually keep:**
- ✅ Templates in `templates/` (unless you want different format)
- ✅ Slash commands in `.claude/commands/`
- ✅ `.CLAUDE.MD` coding standards (unless team has different standards)

**Generated per project:**
- ✅ Everything in `output/` (Claude creates these from your inputs)

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
- ✅ Long-term phase tracking (not 2-week cycles)

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

## 📈 Success Metrics

Track these to measure system effectiveness:

**Planning Accuracy:**
- Phase completion rate (target: >80%)
- Overall velocity predictability (variance <15%)

**Documentation Quality:**
- Docs up-to-date (checked weekly)
- Developer satisfaction with docs

**Progress Visibility:**
- Stakeholder satisfaction
- Time saved on status reports

**Risk Management:**
- Blockers identified early
- Issues resolved proactively

---

## 🤝 Contributing & Feedback

This system is designed to evolve with your needs.

**If you find improvements:**
1. Document what works/doesn't work
2. Update templates or commands
3. Share learnings with team

**Common customizations:**
- Phase duration (adjust 1-4 month range)
- Story point scale (use t-shirt sizes instead)
- Additional documentation types
- Custom progress metrics
- Execution mode (continuous vs paused)

---

## 📞 Support

**For questions about:**
- System usage: Refer to this README
- Coding standards: See `.CLAUDE.MD`
- Project-specific rules: See `rules/project-rules.md`
- Slash commands: See `.claude/commands/*.md`

**If something isn't working:**
1. Check input files are complete
2. Verify file permissions
3. Try regenerating with `/generate-docs`
4. Check this troubleshooting section

---

## 📜 Version History

**v3.0.0 (Current)**
- Phase-based system (1-4 months per phase)
- 4 standard phases: Foundation, Core Features, Advanced Features, Polish & Launch
- Automated execution with `/execute-work phase N`
- Continuous and paused execution modes
- Automatic progress tracking
- Plan mode mandatory

**v2.0 (Historical)**
- Sprint-based system (2-week cycles)
- Manual sprint planning with `/plan-sprint`

**v1.0 (2026-03-24)**
- Initial release
- Core slash commands
- Complete template system
- Progress tracking
- Documentation generation

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
- [ ] Reviewed Phase 1 plan (Foundation)
- [ ] Ready to run `/execute-work phase 1`!

---

**Built with Claude Code**
**Designed for autonomous project management**
**Reusable across all projects**

---

For more details on specific commands, see `.claude/commands/[command-name].md`

Happy coding! 🚀
