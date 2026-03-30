# Claude Project Management System

**Version:** 1.0
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
- ✅ When to use TodoWrite vs /plan-sprint
- ✅ Document hierarchy and conflict resolution
- ✅ No conflicts, no confusion!

**TL;DR:**
- **Planning?** → Use project management commands
- **Coding?** → Follow `.CLAUDE.MD` standards
- **Tracking?** → Use `/update-progress`

[📖 Read Full Integration Guide](INTEGRATION-GUIDE.md)

---

## 🎯 Overview

The **Claude Project Management System** is a complete project management framework that enables Claude to:

- ✅ **Autonomously read** project scope, backlog, and constraints
- ✅ **Generate comprehensive documentation** (PRD, Technical Spec, Architecture)
- ✅ **Plan sprints** based on priorities and team capacity
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
- Create Sprint 1 plan
- Set up progress tracking

#### Step 3: Start Development

Follow the sprint plan and update progress:

```bash
/project-status       # Check current status
/update-progress      # Log completed work
/plan-sprint 2        # Plan next sprint when ready
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
│   ├── sprints/               # Sprint plans
│   │   ├── sprint-1.md       # Sprint 1 plan
│   │   ├── sprint-2.md       # Sprint 2 plan
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
    ├── plan-sprint.md         # Plan next sprint
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
       ├─ Creates Sprint 1 plan
       └─ Initializes progress tracking

4. Sprint Cycle (Repeat)
   ├─ Development
   │  ├─ Work on sprint tasks
   │  ├─ Run: /update-progress (regularly)
   │  └─ Run: /project-status (check status)
   │
   ├─ Sprint Review
   │  ├─ Mark completed work
   │  └─ Document learnings
   │
   └─ Sprint Planning
       └─ Run: /plan-sprint N (plan next sprint)

5. Ongoing Maintenance
   ├─ Add new client documents as they arrive
   ├─ Re-run: /process-client-docs (merges with existing)
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
       ├─ Creates Sprint 1 plan
       └─ Initializes progress tracking

3. Sprint Cycle (Repeat)
   ├─ Development
   │  ├─ Work on sprint tasks
   │  ├─ Run: /update-progress (regularly)
   │  └─ Run: /project-status (check status)
   │
   ├─ Sprint Review
   │  ├─ Mark completed work
   │  └─ Document learnings
   │
   └─ Sprint Planning
       └─ Run: /plan-sprint N (plan next sprint)

4. Ongoing Maintenance
   ├─ Update input files as scope changes
   ├─ Run: /generate-docs (regenerate docs)
   └─ Track blockers in progress/blockers.md
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
**Purpose:** Initialize a new project with all documentation and first sprint

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
- `.project-management/output/sprints/sprint-1.md`
- `.project-management/output/progress/*.md`

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

### `/plan-sprint [number]`
**Purpose:** Plan the next sprint

**When to use:**
- End of current sprint
- Starting a new sprint
- Need to adjust sprint scope

**What it does:**
- Analyzes remaining backlog
- Considers team velocity
- Selects appropriate work items
- Creates detailed sprint plan

**Example:**
```bash
/plan-sprint 2
```

**Output:**
- `.project-management/output/sprints/sprint-2.md`
- Updated `current-status.md` with new sprint info

---

### `/update-progress`
**Purpose:** Update project progress tracking

**When to use:**
- Daily or every few days
- After completing stories/tasks
- When blockers arise
- Before stakeholder meetings

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
- Updated current sprint file

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
  - Sprint status
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
- **Phases:** How to break down the project

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
- **Timeline:** Deadlines, milestones
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
- Timeline and phases

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
# ✅ Sprint 1 plan (2 weeks)
# ✅ Progress tracking setup

# Step 3: Start development
# Work on Sprint 1 tasks...

# Step 4: Track progress (weekly)
/update-progress

# Step 5: Check status anytime
/project-status
```

---

### Example 2: Planning Next Sprint

```bash
# Current sprint ending soon

# Step 1: Update progress with sprint completion
/update-progress
# Claude asks: What was completed? Any issues?

# Step 2: Plan next sprint
/plan-sprint 2

# Claude:
# ✅ Analyzes remaining backlog
# ✅ Considers team velocity from Sprint 1
# ✅ Selects appropriate work (P0/P1 items)
# ✅ Creates Sprint 2 plan

# Step 3: Review and start Sprint 2
# Review sprint-2.md, adjust if needed, begin work
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

# Step 4: Replan if necessary
# If current sprint affected:
/plan-sprint 3
# Incorporate new high-priority work

# Step 5: Update status
/project-status
# See impact on timeline and scope
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
# ✅ Updates current sprint status
# ✅ Marks affected tasks as blocked
# ✅ Suggests mitigation

# Step 2: Check impact
/project-status

# Shows:
# 🔴 1 Critical Blocker
# ⚠️ Sprint goal at risk
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

### Sprint Planning
1. **Don't overcommit** - Better to under-promise and over-deliver
2. **Respect velocity** - Use past sprint data to plan realistically
3. **Balance risk** - Mix high-risk and low-risk items
4. **Check dependencies** - Don't pick work that can't be completed
5. **Set clear goals** - Each sprint should have a clear objective

### Progress Tracking
1. **Update frequently** - Daily or every 2-3 days is ideal
2. **Be honest** - Don't hide problems or delays
3. **Document blockers** - Report issues as soon as they arise
4. **Celebrate wins** - Log completed work immediately
5. **Learn continuously** - Use retrospectives to improve

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

### Issue: Sprint plan seems unrealistic

**Solution:**
- Check `constraints.md` - is team capacity accurate?
- Review velocity from previous sprints
- Adjust story point estimates in `backlog.md`
- Run `/plan-sprint N` again with updated data

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
rm .project-management/output/sprints/*
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
rm ~/project-management-template/.project-management/output/sprints/*
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
- ✅ Structured sprint plans
- ✅ Consistent documentation
- ✅ Defined coding standards

### For Project Managers
- ✅ Automated planning and tracking
- ✅ Real-time progress visibility
- ✅ Risk identification
- ✅ Status reports on-demand

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
- Sprint completion rate (target: >80%)
- Velocity predictability (variance <15%)

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
- Sprint length (change from 2 weeks)
- Story point scale (use t-shirt sizes instead)
- Additional documentation types
- Custom progress metrics
- Team-specific workflows

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
- [ ] Reviewed Sprint 1 plan
- [ ] Ready to start development!

---

**Built with Claude Code**
**Designed for autonomous project management**
**Reusable across all projects**

---

For more details on specific commands, see `.claude/commands/[command-name].md`

Happy coding! 🚀
