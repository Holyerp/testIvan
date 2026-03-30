---
name: init-project
description: Initialize project with tech stack selection, i18n configuration, and phase-based structure
---

# Initialize Project

You are initializing a new project with the Claude Project Management System.

---

## Your Task

### STEP 1: TECH STACK SELECTION

**📖 See:** `modules/init-project-stack-selection.md` for detailed stack configuration

**Summary:**
1. Ask user to choose configuration method ([1] Default / [2] AI / [3] Custom)
2. Process selection:
   - **Default:** Copy default-stack.md to technologies.md
   - **AI:** Analyze project needs, generate recommendation
   - **Custom:** Interactive questions, generate custom stack
3. Display stack summary

**Output:** `technologies.md` configured with chosen tech stack

---

### STEP 2: INTERNATIONALIZATION (i18n) SETUP

**📖 See:** `modules/init-project-i18n-setup.md` for detailed i18n configuration

**Summary:**
1. Ask if project needs multi-language support ([1] Yes / [2] No)
2. If Yes:
   - Ask which languages to support
   - Create `I18N-RULES.md` with language list
   - Create translation file structure (public/locales/{lang}/translation.json)
   - Create sample translation files
3. If No:
   - Skip i18n setup
   - Do NOT create I18N-RULES.md

**Output:** i18n configured (if enabled) with translation files and rules

---

### STEP 3: READ INPUT FILES

**Read all input files from `.project-management/input/`:**
- `scope.md` - Project scope and objectives
- `backlog.md` - Features and user stories
- `technologies.md` - Technology stack (now configured)
- `constraints.md` - Project constraints

**Analyze the inputs to understand:**
- Project goals and vision
- Feature requirements and priorities
- Technical architecture needs
- Timeline and resource constraints

---

### STEP 4: GENERATE DOCUMENTATION

**Generate initial documentation in `.project-management/output/docs/`:**

**1. `prd.md` - Product Requirements Document**
- Use template: `.project-management/templates/prd-template.md`
- Extract project vision from scope.md
- List all features from backlog.md organized by priority
- Define success metrics
- Document assumptions and risks

**2. `technical-spec.md` - Technical Specification**
- Use template: `.project-management/templates/technical-spec-template.md`
- Detail the technology stack from technologies.md
- Design database schema based on requirements
- Define API endpoints for features
- Specify security and performance requirements

**3. `architecture.md` - System Architecture**
- Use template: `.project-management/templates/architecture-template.md`
- Create system architecture based on tech stack
- Design component structure
- Define data flow
- Plan deployment architecture

---

### STEP 5: CREATE PHASE STRUCTURE

**Analyze backlog.md and organize into phases:**

**Create `.project-management/output/phases/` directory**

**Generate initial phase files based on epic priorities:**

**Phase 1: Foundation & Setup**
- All "Project Setup", "Infrastructure", "Authentication" epics
- Estimated: 1-2 months

**Phase 2: Core Features**
- Main product features (P0 priority)
- Estimated: 2-3 months

**Phase 3: Advanced Features**
- Secondary features (P1 priority)
- Estimated: 2 months

**Phase 4: Polish & Launch**
- Final optimizations, testing, deployment
- Estimated: 1 month

**For each phase, create `phase-N.md` using `.project-management/templates/phase-template.md`**

---

### STEP 6: CREATE PROGRESS TRACKING

**Create in `.project-management/output/progress/`:**

**1. `current-status.md`** - Initialize with project start status
- Use `.project-management/templates/progress-template.md`
- Set all metrics to 0%
- Set project start date
- List Phase 1 as current phase

**2. `completed.md`** - Empty initially (for tracking completed work)

**3. `blockers.md`** - Empty initially (for tracking blockers)

---

### STEP 7: SUMMARY REPORT

**Provide comprehensive summary to user:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🎉 PROJECT INITIALIZED SUCCESSFULLY
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📦 TECH STACK:
{{If default stack:}}
✅ Default HolyEstate Stack
   - React 19 + React Router 7 (SSR)
   - PostgreSQL 16 + Prisma 6.19.0
   - Vitest 4.0 + Playwright 1.58.0

{{If AI recommended:}}
🤖 AI-Recommended Stack
   - {{stack summary}}

{{If custom:}}
🛠️ Custom Stack
   - {{stack summary}}

🌐 INTERNATIONALIZATION:
{{If i18n enabled:}}
✅ Enabled
   - Default: English (en)
   - Additional: {{languages}}
   - Translation files created

{{If i18n disabled:}}
❌ Disabled (no translation requirements)

📄 GENERATED DOCUMENTATION:
✅ Product Requirements Document (PRD)
✅ Technical Specification
✅ System Architecture Document
✅ Phase Structure (4 phases)
✅ Initial Progress Tracking

📊 PROJECT OVERVIEW:
- Total Epics: {{epic_count}}
- Total Stories: {{story_count}}
- Total Story Points: {{total_points}}
- Estimated Duration: {{weeks}} weeks

📅 PHASE BREAKDOWN:
Phase 1: Foundation & Setup ({{points_1}} points, {{weeks_1}} weeks)
Phase 2: Core Features ({{points_2}} points, {{weeks_2}} weeks)
Phase 3: Advanced Features ({{points_3}} points, {{weeks_3}} weeks)
Phase 4: Polish & Launch ({{points_4}} points, {{weeks_4}} weeks)

🎯 NEXT STEPS:

1. Review generated documentation:
   - .project-management/output/docs/prd.md
   - .project-management/output/docs/technical-spec.md
   - .project-management/output/phases/phase-1.md

2. Start Phase 1:
   /execute-work phase 1

3. Check project status:
   /project-status

4. Run tests:
   /run-tests all

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🚀 Ready to start development!
```

---

## 📚 Module References

**Detailed workflows available in:**
- `modules/init-project-stack-selection.md` - STEP 1 (Tech stack selection)
- `modules/init-project-i18n-setup.md` - STEP 2 (i18n configuration)

---

## Important Guidelines

- **Use the templates** from `.project-management/templates/` and replace all `{{PLACEHOLDERS}}` with actual data
- **Be comprehensive** - generate complete documentation, don't leave TODOs
- **Follow project rules** in `.CLAUDE.MD`
- **Prioritize properly** - Organize epics into phases logically
- **Be realistic** - Consider constraints when planning
- **Create actionable structure** - Phase 1 should be achievable and foundational

---

## Files to Generate

**Configuration:**
- `.project-management/input/technologies.md` (if not exists or update)
- `.project-management/rules/I18N-RULES.md` (if i18n enabled)
- `public/locales/{lang}/translation.json` (if i18n enabled)

**Documentation:**
- `.project-management/output/docs/prd.md`
- `.project-management/output/docs/technical-spec.md`
- `.project-management/output/docs/architecture.md`

**Phases:**
- `.project-management/output/phases/phase-1.md`
- `.project-management/output/phases/phase-2.md`
- `.project-management/output/phases/phase-3.md`
- `.project-management/output/phases/phase-4.md`

**Progress:**
- `.project-management/output/progress/current-status.md`
- `.project-management/output/progress/completed.md`
- `.project-management/output/progress/blockers.md`

---

**Version:** 2.0 (Modular)
**Updated:** 2026-03-27
