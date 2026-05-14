---
name: init-project
description: Initialize project with tech stack selection, i18n configuration, and phase-based structure
---

# Initialize Project

**📖 Quick Start:** See [how-to-use/init-project.md](./how-to-use/init-project.md) for quick guide (~120 lines)

You are initializing a new project with the Claude Project Management System.

---

## Your Task

**🔧 DOCUMENTATION RULES:**
All documentation generated must follow:
- **`CLAUDE.md`** - All documentation in English only, coding standards
- **`.claude/rules/git.md`** - If committing initialization (NO AI credits, conventional commits)

---

### STEP 0: PROJECT STRUCTURE SELECTION

**📖 See:** `modules/init-project-structure-setup.md` for detailed structure configuration

**Summary:**

1. **Ask via AskUserQuestion** (gating, no Skip):

   ```
   question: "What project structure?"
   header: "Project type"
   skippable: false
   options:
     - label: "Backend + Mobile (Recommended)"
       description: "Monorepo with apps/backend + apps/mobile. Best for SaaS with iOS/Android client."
     - label: "Backend Only"
       description: "API-only service. No frontend code in this repo."
     - label: "Backend + Web + Mobile"
       description: "Full monorepo: apps/backend + apps/web + apps/mobile."
     - label: "Web Only"
       description: "Single web app (SPA / SSR). No backend in this repo."
   ```

2. Based on selection, create appropriate folder structure (see `modules/init-project-structure-setup.md` for monorepo scaffolding).

3. For monorepo selections (Backend + Mobile, Backend + Web + Mobile):
   - Create `apps/` directory with backend/mobile/web subdirectories
   - Create `packages/` for shared code (types, utils, api-client)
   - Setup pnpm workspace + Turborepo
   - Create package.json files for each app/package

4. Update `.gitignore` for monorepo structure.

**Output:** Project structure created, ready for tech stack configuration.

---

### STEP 1: TECH STACK SELECTION

**📖 See:** `modules/init-project-stack-selection.md` for detailed stack configuration

**Summary:**

1. **Ask via AskUserQuestion** (gating, no Skip):

   ```
   question: "Tech stack approach?"
   header: "Stack"
   skippable: false
   options:
     - label: "Default (Recommended)"
       description: "React Router 7 + Prisma + Postgres + Zod. Battle-tested defaults from .claude/rules/stack-specific.md."
     - label: "AI suggests"
       description: "Claude proposes a stack based on input/scope.md and constraints.md. You approve or revise."
     - label: "Custom"
       description: "You pick each layer (backend / database / frontend / styling / testing / build / deploy)."
   ```

2. Based on selection, configure stack files (see `modules/init-project-stack-selection.md`). Custom flow invokes a sequence of layer-by-layer AskUserQuestion calls.

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

### STEP 3: READ INPUT FILES AND HANDLE BACKLOG FORMAT

**Read all input files from `.project-management/input/`:**
- `scope.md` - Project scope and objectives
- `backlog/` (preferred) OR `backlog.md` (legacy) - Features and user stories
- `technologies.md` - Technology stack (now configured)
- `constraints.md` - Project constraints

**🔄 BACKLOG FORMAT DETECTION:**

**Check which format exists:**

1. **If `backlog/` directory exists (MODERN):**
   - ✅ Read modular structure: `backlog/README.md`, `backlog/phase-*.md`
   - ✅ This is the preferred format (< 200 lines per file)
   - ✅ Continue to STEP 4

2. **If only `backlog.md` exists (LEGACY):**
   - ⚠️ Detected monolithic backlog (old format)
   - 🔄 **Automatic migration:** Run `/migrate-to-modular` internally
   - ✅ After migration, continue with modular structure
   - 📝 Note in summary: "Migrated monolithic backlog to modular structure"

3. **If neither exists:**
   - ❌ Error: No backlog found. User must run `/process-client-docs` first

**Analyze the inputs to understand:**
- Project goals and vision
- Feature requirements and priorities
- Technical architecture needs
- Timeline and resource constraints

---

### STEP 4: GENERATE DOCUMENTATION

**🌍 CRITICAL: Generate ALL documentation in English only. No exceptions.**

**🔒 Anonymization (mandatory):** Personal names from input docs MUST NOT appear in generated PRD / technical spec / architecture. Replace with role labels (`the PM`, `the tech lead`, `the client`, `the stakeholder`) and source-context phrases (`per our call`, `agreed in the planning meeting`). Full rule: `.claude/rules/anonymization.md`.

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

**Read modular backlog from `backlog/` directory:**

**Note:** At this point, backlog is guaranteed to be in modular format (either generated by `/process-client-docs` or auto-migrated in STEP 3)

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

### STEP 6: POST-GENERATION CLARIFICATION GATE

**📖 See:** `modules/interactive-clarifications.md` for the full loop (STEPS B–G — AskUserQuestion call shape, skip handling, anonymized free-text, answer-application to artefacts).

After STEP 4 produces the documentation set (`output/docs/prd.md`, `technical-spec.md`, `architecture.md`) and STEP 5 creates the phase structure, this step runs the same interactive Q&A gate used by `/process-client-docs` STEP 5.

**Sources of questions:**

1. **TBD markers** in generated docs — grep the generated files for `<!-- TBD: Q-NNN -->`. Each marker references a question by ID. If the corresponding question is not already in `input/open-questions.md`, leave the marker in place (the doc indicates an open ambiguity to resolve manually). If the question IS in `open-questions.md`, include it in the loop.

2. **Existing P0/P1 entries in `input/open-questions.md`** — read the file (created earlier by `/process-client-docs` or prior `/init-project` runs). Filter `Status: Open` entries with `priority: P0` or `P1`.

**Behavior:**

- If both sources yield zero questions → emit `✅ No open clarifications.` and skip the loop.
- Otherwise, build the question list (priority-sorted P0 → P1) and invoke `modules/interactive-clarifications.md` STEPS B–G.
- Skipped questions remain in `open-questions.md` with incremented `Skipped:` count.
- Answered questions update `applies_to` artefacts AND move to the Resolved section of `open-questions.md`.

**Resume later:** the user can run `/resolve-questions` at any time to revisit still-Open entries.

---

### STEP 7: CREATE PROGRESS TRACKING

**Create in `.project-management/output/progress/`:**

**1. `current-status.md`** - Initialize with project start status
- Use `.project-management/templates/progress-template.md`
- Set all metrics to 0%
- Set project start date
- List Phase 1 as current phase

**2. `completed.md`** - Empty initially (for tracking completed work)

**3. `blockers.md`** - Empty initially (for tracking blockers)

---

### STEP 7.5: SCREEN INVENTORY (CONDITIONAL — only if project has a UI)

**Per `.claude/rules/screen-inventory.md`:** scaffold the screen map artifact when the project includes a frontend.

**Trigger logic:**
- Project type from STEP 0:
  - `[1] Backend Only` → **skip this step**
  - `[2] Backend + Mobile App` → **scaffold** for mobile
  - `[3] Backend + Web + Mobile` → **scaffold** for both
  - `[4] Web Only` → **scaffold** for web

**Action when triggered:**
1. Create directory `.project-management/input/screens/`.
2. Copy `.project-management/templates/screen-map-template.md` to `.project-management/input/screens/screen-map.md`.
3. Substitute placeholders: `{{PROJECT_NAME}}`, `{{VERSION}}` (= `0.1.0`), `{{DATE}}` (= today), `{{STATUS}}` (= `Draft`).
4. Leave the screen entries as template placeholders — they will be filled in during `/process-client-docs` (which knows about designs) or hand-curated by the team. The first `/screen-map` run after stories exist will derive the API columns automatically.
5. Inform the user in the STEP 8 summary that the screen map was scaffolded and where to find it.

---

### STEP 8: SUMMARY REPORT

Render the comprehensive summary to the user using the template in `init-project-reference.md`. Substitute actual values for tech stack, i18n status, epic/story/point totals, per-phase breakdown, and next-step commands.

---

## 📚 Module References

**Detailed workflows available in:**
- `modules/init-project-structure-setup.md` - STEP 0 (Project structure: monorepo vs single app)
- `modules/init-project-stack-selection.md` - STEP 1 (Tech stack selection)
- `modules/init-project-i18n-setup.md` - STEP 2 (i18n configuration)
- `modules/interactive-clarifications.md` - STEP 6 (Post-generation clarification gate; reusable across PM commands)

---

## Important Guidelines

- **Use the templates** from `.project-management/templates/` and replace all `{{PLACEHOLDERS}}` with actual data
- **Be comprehensive** - generate complete documentation, don't leave TODOs
- **Follow project rules** in `CLAUDE.md`
- **Prioritize properly** - Organize epics into phases logically
- **Be realistic** - Consider constraints when planning
- **Create actionable structure** - Phase 1 should be achievable and foundational

---

## Files to Generate

Full manifest (project structure / configuration / documentation / phases / progress) is in `init-project-reference.md`.

---

**Version:** 3.0.0
**Updated:** 2026-03-27
