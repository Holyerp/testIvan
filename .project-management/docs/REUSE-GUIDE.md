# Reusing for Other Projects

**Version:** 3.3.0
**Last Reviewed:** 2026-04-21
**Purpose:** Guide for reusing this project management system across projects

---

## Overview

This entire project management system is **fully reusable**! Use it for any project by copying the structure and customizing input files.

**Benefits:**
- ✅ Same workflow across all projects
- ✅ Consistent documentation structure
- ✅ Familiar commands for Claude
- ✅ Proven templates and processes
- ✅ Saves setup time

---

## 🚀 Method 1: Copy to New Project

### Step-by-Step

```bash
# =====================================
# STEP 1: Copy Structure
# =====================================
# Copy entire project management system
cp -r .project-management /path/to/new-project/
cp -r .claude /path/to/new-project/
cp CLAUDE.md /path/to/new-project/

# Navigate to new project
cd /path/to/new-project

# =====================================
# STEP 2: Clear Old Data
# =====================================
# Remove previous project's generated files
rm .project-management/output/docs/*
rm .project-management/output/phases/*
rm .project-management/output/progress/*
rm .project-management/output/bugs/*

# =====================================
# STEP 3: Clear Client Documents (if any)
# =====================================
rm .project-management/client-input/*
# Keep README.md in client-input

# =====================================
# STEP 4: Reset Input Files
# =====================================
# Option A: Start fresh
rm .project-management/input/*
# Then add template files from templates folder

# Option B: Edit existing
# Edit input files with new project info
# - scope.md
# - backlog/   (modular structure)
# - technologies.md
# - constraints.md

# =====================================
# STEP 5: Initialize New Project
# =====================================
/init-project

# Claude generates all documentation for new project

# =====================================
# STEP 6: Start Development
# =====================================
/execute-work phase 1

# Done! System ready for new project
```

### What Gets Copied

**Structure (reusable):**
- ✅ `.project-management/templates/` - All templates
- ✅ `.project-management/guides/` - User guides
- ✅ `.project-management/docs/` - System documentation
- ✅ `.project-management/defaults/` - Default configurations
- ✅ `.claude/commands/` - All slash commands
- ✅ `.claude/rules/` - Code quality rules
- ✅ `CLAUDE.md` - Coding standards

**Data (project-specific, clear or replace):**
- ❌ `.project-management/input/*` - Replace with new project data
- ❌ `.project-management/output/*` - Delete, Claude regenerates
- ❌ `.project-management/client-input/*` - Delete old client docs

---

## 📦 Method 2: Create Template

### One-Time Setup

```bash
# =====================================
# Create Template Directory
# =====================================
mkdir ~/project-management-template

# Copy complete structure
cp -r .project-management ~/project-management-template/
cp -r .claude ~/project-management-template/
cp CLAUDE.md ~/project-management-template/

# =====================================
# Clean Output Files in Template
# =====================================
cd ~/project-management-template

# Remove generated files
rm .project-management/output/docs/*
rm .project-management/output/phases/*
rm .project-management/output/progress/*
rm .project-management/output/bugs/*

# Remove client documents
rm .project-management/client-input/*
# Keep README.md

# =====================================
# Clear Input Files (Optional)
# =====================================
# Option A: Keep as examples
# Leave input files as reference

# Option B: Clear for fresh start
rm .project-management/input/*
# Add blank template files

# =====================================
# Template Ready!
# =====================================
```

### Using Template for New Projects

```bash
# For each new project:
cp -r ~/project-management-template/* /path/to/new-project/
cd /path/to/new-project

# Fill input files
# Run /init-project
# Start development with /execute-work phase 1
```

---

## 🔧 Method 3: Git Repository Template

### Create GitHub Template Repository

```bash
# =====================================
# STEP 1: Create New Repo
# =====================================
# On GitHub, create new repository
# Name: "project-management-template"

# =====================================
# STEP 2: Push Template
# =====================================
git init
git add .project-management .claude CLAUDE.md
git commit -m "Initial template"
git remote add origin git@github.com:yourusername/project-management-template.git
git push -u origin main

# =====================================
# STEP 3: Mark as Template
# =====================================
# On GitHub:
# Settings → Template repository → ✅ Check

# =====================================
# STEP 4: Use Template for New Projects
# =====================================
# On GitHub:
# Use this template → Create a new repository
# Clone and start working
```

### Advantages
- ✅ Version controlled template
- ✅ Easy updates to template
- ✅ Share across team
- ✅ Track template evolution

---

## 🎨 Customization Guide

### Always Customize

**Input Files** (`.project-management/input/`)
- ✅ `scope.md` - New project vision and goals
- ✅ `backlog/` - New features and user stories (modular)
- ✅ `technologies.md` - New tech stack
- ✅ `constraints.md` - New timeline and constraints

**Project Rules** (`.project-management/rules/`)
- ✅ `project-rules.md` - Project-specific coding rules (if unique)

### Usually Keep As-Is

**Templates** (`.project-management/templates/`)
- ✅ Keep unless you want different documentation format
- Templates are project-agnostic

**Commands** (`.claude/commands/`)
- ✅ Keep - they work for all projects
- Only modify if you add custom commands

**Code Quality Rules** (`.claude/rules/`)
- ✅ Keep - universal best practices
- Only modify for team-specific standards

**Coding Standards** (`CLAUDE.md`)
- ✅ Keep unless team has different standards
- Customize for company/team conventions

### Sometimes Customize

**Default Stack** (`.project-management/defaults/`)
- 🔄 Update `default-stack.md` if you have different defaults
- 🔄 Update `stack-questions.md` for different tech stacks

**i18n Rules** (`.project-management/rules/I18N-*.md`)
- 🔄 Keep if project supports multiple languages
- Delete if single-language project

**Testing Rules** (`.project-management/rules/TESTING-RULES.md`)
- 🔄 Adjust coverage targets if needed
- Keep core testing principles

### Never Customize

**System Documentation** (`.project-management/docs/`)
- ❌ These explain the system itself
- Keep for reference

**User Guides** (`.project-management/guides/`)
- ❌ How to use the system
- Keep for team onboarding

---

## 🔄 Updating Template

### When Template Improves

If you make improvements to the system (better templates, new commands):

```bash
# =====================================
# Update Template Repository
# =====================================
cd ~/project-management-template

# Copy improvements
cp -r /path/to/improved-project/.project-management/templates/* \
      .project-management/templates/

cp -r /path/to/improved-project/.claude/commands/* \
      .claude/commands/

# Commit changes
git add .
git commit -m "Improve templates based on Project X learnings"
git push
```

### Applying Template Updates to Existing Projects

```bash
# =====================================
# Option A: Manual Updates
# =====================================
# Copy specific improved files
cp ~/project-management-template/.project-management/templates/prd-template.md \
   .project-management/templates/

# =====================================
# Option B: Selective Merge
# =====================================
# Use git to merge template updates
git remote add template git@github.com:yourusername/project-management-template.git
git fetch template
git cherry-pick <commit-hash>

# =====================================
# Option C: Full Refresh (Careful!)
# =====================================
# Only update structure, preserve project data
cp -r ~/project-management-template/.project-management/templates/* \
      .project-management/templates/

# DON'T overwrite input/ or output/ folders!
```

---

## 📋 Checklist for New Project Setup

### Pre-Setup
- [ ] Copied `.project-management/` folder
- [ ] Copied `.claude/` folder
- [ ] Copied `CLAUDE.md` file
- [ ] Cleared `output/docs/*`
- [ ] Cleared `output/phases/*`
- [ ] Cleared `output/progress/*`
- [ ] Cleared `output/bugs/*`
- [ ] Cleared `client-input/*` (keep README)

### Input Files
- [ ] Filled `input/scope.md` completely
- [ ] Populated `input/backlog/` with phase files (or let `/init-project` generate them)
- [ ] Filled `input/technologies.md` with tech stack
- [ ] Filled `input/constraints.md` with constraints
- [ ] Reviewed for completeness (no TODOs/placeholders)

### Customization (if needed)
- [ ] Updated `rules/project-rules.md` if unique rules
- [ ] Updated `CLAUDE.md` if different standards
- [ ] Updated `defaults/default-stack.md` if applicable
- [ ] Removed `rules/I18N-*.md` if not needed

### Initialization
- [ ] Ran `/init-project` successfully
- [ ] Reviewed generated `output/docs/prd.md`
- [ ] Reviewed generated `output/docs/technical-spec.md`
- [ ] Reviewed generated `output/docs/architecture.md`
- [ ] Reviewed `output/phases/phase-1.md`

### Ready to Start
- [ ] Ready to run `/execute-work phase 1`
- [ ] Team has access to documentation
- [ ] Git repository initialized
- [ ] First commit made

---

## 🎯 Common Customization Scenarios

### Scenario 1: Different Phase Duration

```markdown
# In constraints.md, specify:
Timeline:
- Phase 1: 2 months (instead of 1-4)
- Phase 2: 2 months
- Phase 3: 3 months
- Phase 4: 1 month
```

### Scenario 2: Different Story Point Scale

```markdown
# In backlog/README.md header, note:
Story Points (T-Shirt Sizes):
- XS: 1 point
- S: 2 points
- M: 3 points
- L: 5 points
- XL: 8 points
```

### Scenario 3: Additional Documentation Types

```bash
# Add new template:
cp custom-diagram-template.md .project-management/templates/

# Reference in /generate-docs command
# Or create custom slash command
```

### Scenario 4: Company-Specific Standards

```markdown
# In CLAUDE.md, add section:
## Company Standards

### Naming Conventions
- [Company-specific rules]

### Code Review Process
- [Company-specific process]

### Deployment Workflow
- [Company-specific workflow]
```

---

## 💡 Tips for Team Adoption

### Onboarding New Team Members

1. **Share system documentation**
   - README.md for overview
   - docs/ folder for details

2. **Walk through one project**
   - Show input files
   - Demonstrate commands
   - Explain generated docs

3. **Practice on small project**
   - Let them initialize a test project
   - Guide through first phase
   - Answer questions

### Team Standards

**Document in `CLAUDE.md`:**
- Coding conventions
- Testing requirements
- Git workflow
- Code review process

**Keep consistent across projects:**
- Same template usage
- Same command patterns
- Same documentation structure

---

[← Back to README](../README.md)
