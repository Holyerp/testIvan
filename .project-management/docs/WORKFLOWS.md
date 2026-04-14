# Workflow Examples

**Version:** 3.0.0
**Purpose:** Step-by-step workflow examples for common scenarios

---

## Overview

This guide shows complete workflows for common project management scenarios.

**Topics covered:**
1. Starting a new project
2. Executing phases
3. Handling scope changes
4. Managing blockers
5. Adding bugs
6. Planning future versions

---

## 🚀 Example 1: Starting a New Project

### Option A: From Client Documents (Recommended)

```bash
# =====================================
# STEP 1: Add Client Documents
# =====================================
# Copy client's documents to client-input folder
.project-management/client-input/
├── project-brief.pdf
├── requirements.docx
├── mockups.png
└── timeline.txt

# =====================================
# STEP 2: Process Documents
# =====================================
/process-client-docs

# Claude will:
# ✅ Read all documents (PDFs, Word, images, text)
# ✅ Extract requirements, features, goals
# ✅ Generate scope.md, backlog.md, technologies.md, constraints.md
# ✅ Ask clarifying questions

# =====================================
# STEP 3: Review & Refine
# =====================================
# Review generated files:
.project-management/input/
├── scope.md          # ✅ Auto-generated
├── backlog.md        # ✅ Auto-generated
├── technologies.md   # ✅ Auto-suggested
└── constraints.md    # ✅ Auto-extracted

# Edit if needed, add missing information

# =====================================
# STEP 4: Initialize Project
# =====================================
/init-project

# Claude generates:
# ✅ Complete documentation (PRD, tech spec, architecture)
# ✅ Phase 1 plan (Foundation, 1-4 months)
# ✅ Progress tracking setup

# =====================================
# STEP 5: Start Development
# =====================================
/execute-work phase 1

# Claude automatically:
# ✅ Plans Phase 1 work
# ✅ Executes implementation with quality gates
# ✅ Runs tests (second-to-last step)
# ✅ Creates commit (last step)
# ✅ Tracks progress continuously

# =====================================
# STEP 6: Monitor Progress
# =====================================
/project-status

# Shows:
# ✅ Overall progress
# ✅ Current phase status
# ✅ Completed work
# ✅ Active bugs
# ✅ Blockers and risks

# =====================================
# STEP 7: Continue Through Phases
# =====================================
/execute-work phase 2    # Core Features
/execute-work phase 3    # Advanced Features
/execute-work phase 4    # Polish & Launch
```

### Option B: Manual Entry

```bash
# =====================================
# STEP 1: Fill Input Files Manually
# =====================================
# Navigate to .project-management/input/
# Edit these files:
├── scope.md          # Project vision, goals, phases
├── backlog.md        # All features and user stories
├── technologies.md   # Tech stack
└── constraints.md    # Timeline, budget, team

# =====================================
# STEP 2: Initialize Project
# =====================================
/init-project

# (Same as Option A from here)
# =====================================
# STEP 3: Start Development
# =====================================
/execute-work phase 1

# =====================================
# STEP 4: Continue Through Phases
# =====================================
/execute-work phase 2
/execute-work phase 3
/execute-work phase 4
```

---

## 🔄 Example 2: Executing Next Phase

```bash
# Phase 1 (Foundation) is completed

# =====================================
# STEP 1: Start Next Phase
# =====================================
/execute-work phase 2

# Claude automatically:
# ✅ Plans Phase 2 (Core Features, 1-4 months)
# ✅ Analyzes remaining backlog
# ✅ Selects appropriate work for milestone
# ✅ Executes implementation
# ✅ Tracks progress continuously

# =====================================
# STEP 2: Monitor Progress
# =====================================
/project-status

# Shows Phase 2 progress:
# - Stories completed
# - Stories in progress
# - Stories remaining
# - Phase timeline adherence
# - Overall project health

# =====================================
# STEP 3: Continue Through Remaining Phases
# =====================================
/execute-work phase 3    # Advanced Features
/execute-work phase 4    # Polish & Launch
```

---

## 📝 Example 3: Scope Change Mid-Project

```bash
# New feature requested from client!

# =====================================
# STEP 1: Add New Requirement
# =====================================
/add-scope add story [phase] [epic]

# OR manually update backlog.md with new stories

# =====================================
# STEP 2: Update Scope (if needed)
# =====================================
# Edit input/scope.md
# If vision/objectives changed

# =====================================
# STEP 3: Regenerate Documentation
# =====================================
/generate-docs

# Claude updates:
# ✅ PRD with new features
# ✅ Technical spec with new requirements
# ✅ Architecture if needed
# ✅ Phase plans

# =====================================
# STEP 4: Continue Execution
# =====================================
# New features automatically included in:
/execute-work phase 2

# OR execute specific story:
/execute-work story US-XXX

# =====================================
# STEP 5: Check Impact on Timeline
# =====================================
/project-status

# Shows updated:
# - Phase completion estimates
# - Timeline impact
# - Resource requirements
```

---

## 🚫 Example 4: Handling a Blocker

```bash
# Critical blocker discovered!

# =====================================
# STEP 1: Report Blocker
# =====================================
/update-progress

# Claude asks: "Any blockers?"
# You describe: "Payment API down, can't test checkout"

# Claude:
# ✅ Logs blocker in output/progress/blockers.md
# ✅ Updates current phase status
# ✅ Marks affected stories as blocked
# ✅ Suggests mitigation strategies

# =====================================
# STEP 2: Check Impact
# =====================================
/project-status

# Shows:
# 🔴 1 Critical Blocker
# ⚠️ Phase milestone at risk
# 💡 Recommendations: work on unblocked stories

# =====================================
# STEP 3: Work on Unblocked Items
# =====================================
# Continue with stories not affected by blocker

# =====================================
# STEP 4: When Resolved
# =====================================
/update-progress

# Update blocker status to resolved
# Resume blocked stories
```

---

## 🐛 Example 5: Managing Bugs

```bash
# Bug discovered during development or testing

# =====================================
# STEP 1: Add Bug to Roadmap
# =====================================
/add-bug

# Claude asks:
# - Bug description
# - Steps to reproduce
# - Expected vs actual behavior
# - Severity (Critical, High, Medium, Low)
# - Story points for fix effort

# Bug added to:
# .project-management/output/bugs/bug-roadmap.md

# =====================================
# STEP 2: Prioritize Bug
# =====================================
# Bugs organized by severity:
# - Critical: Fix immediately
# - High: Fix this phase
# - Medium: Fix when possible
# - Low: Backlog

# =====================================
# STEP 3: Fix Critical/High Bugs
# =====================================
/execute-work bug BUG-001

# Claude:
# ✅ Plans bug fix
# ✅ Implements fix
# ✅ Runs tests
# ✅ Verifies fix
# ✅ Updates bug status
# ✅ Moves to bug-archive.md

# =====================================
# STEP 4: Track Bug Metrics
# =====================================
/project-status

# Shows:
# - Active bugs by severity
# - Bugs fixed this phase
# - Bug fix velocity
```

---

## 🔮 Example 6: Planning Future Versions

```bash
# Client wants features for "later" (post-launch)

# =====================================
# STEP 1: Add to Future Backlog
# =====================================
/add-backlog-requirement story

# Claude asks:
# - Story description
# - Target version (2.0, 3.0, Unversioned)
# - Acceptance criteria
# - Story points

# Added to:
# .project-management/input/backlog-future.md

# NOT added to active backlog or phases

# =====================================
# STEP 2: Organize by Version
# =====================================
# backlog-future.md structure:
# - Version 2.0 (post-launch, 1-3 months)
# - Version 3.0 (major features, 6-12+ months)
# - Unversioned (ideas, no timeline)

# =====================================
# STEP 3: When Ready to Implement
# =====================================
# Promote future requirement to active:
/promote-requirement US-XXX --to-phase N

# Moves from backlog-future.md to:
# ✅ input/backlog.md
# ✅ output/phases/phase-N.md
# ✅ Status: Future → Todo

# =====================================
# STEP 4: Execute as Normal
# =====================================
/execute-work story US-XXX
```

---

## 🔧 Example 7: Adding Requirements to Active Project

```bash
# Need to add phase/epic/story to active development

# =====================================
# Add New Phase (rare)
# =====================================
/add-scope add phase 2

# Inserts new phase at position 2
# Existing phases renumber (2→3, 3→4, etc.)

# =====================================
# Add New Epic to Phase
# =====================================
/add-scope add epic 1

# Adds epic to Phase 1
# Updates backlog.md with global epic number

# =====================================
# Add Story to Epic
# =====================================
/add-scope add story 1 2

# Adds story to Epic 2 in Phase 1
# Assigns unique US-XXX ID

# =====================================
# Add from External File
# =====================================
/add-scope add epic 1 --from docs/notification-epic.md

# Reads epic content from file
# Useful for complex requirements

# =====================================
# Edit Existing Story
# =====================================
/add-scope edit story US-005

# Update story details
# Change points, criteria, priority

# =====================================
# Integrity Checks Run Automatically
# =====================================
# After every /add-scope, Claude runs 5 checks:
# ✅ Phase numbering
# ✅ Epic numbering (local in phases, global in backlog)
# ✅ Story IDs (US-XXX unique across project)
# ✅ Cross-references
# ✅ Progress metrics
```

---

## 📊 Example 8: Status Reporting

```bash
# Weekly status check or before meetings

# =====================================
# STEP 1: Get Comprehensive Status
# =====================================
/project-status

# Claude analyzes:
# ✅ Overall project progress
# ✅ Current phase status
# ✅ Completed work (all phases)
# ✅ Active bugs by severity
# ✅ Blockers and risks
# ✅ Timeline adherence
# ✅ Quality metrics
# ✅ Recommendations

# =====================================
# STEP 2: Review Key Metrics
# =====================================
# Report shows:
# - Phase progress (X of Y stories completed)
# - Overall progress (across all phases)
# - Bug metrics (active, fixed, severity breakdown)
# - Velocity trends
# - Timeline status (on track / at risk / delayed)

# =====================================
# STEP 3: Share with Stakeholders
# =====================================
# Copy report to:
# - Slack/Discord
# - Email
# - Project management tool
# - Meeting notes
```

---

[← Back to README](../README.md)
