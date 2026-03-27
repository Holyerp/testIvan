---
name: init-project
description: Initialize project with tech stack selection, i18n configuration, and phase-based structure
---

# Initialize Project

You are initializing a new project with the Claude Project Management System.

---

## Your Task

### STEP 1: TECH STACK SELECTION

**Ask user to choose tech stack configuration method:**

```
🎯 Tech Stack Configuration

How would you like to configure the technology stack?

[1] Default HolyEstate Stack (React 19 + RR7 + Prisma + Railway)
    ✅ Production-tested (848 tests passing)
    ✅ Full-stack with SSR
    ✅ Quick start

[2] AI Recommendation (Analyze project requirements)
    🤖 AI analyzes your project needs
    📊 Suggests optimal stack
    💡 Explains technology choices

[3] Custom Setup (Answer questions step-by-step)
    ❓ Interactive configuration
    🛠️ Full control over choices
    ⚙️ Tailored to your needs
```

**Wait for user selection: 1, 2, or 3**

---

#### Option 1: Default HolyEstate Stack

**If user selects [1]:**

1. Read `.project-management/defaults/default-stack.md`
2. Copy all technologies to `.project-management/input/technologies.md`
3. Display summary:

```
✅ Default HolyEstate Stack Configured

Core Framework:
- React 19.0.0 + React Router 7.13.0 (SSR)
- TypeScript 5.7.0 + Vite 6.1.0

Database:
- PostgreSQL 16.x + Prisma 6.19.0

Testing:
- Vitest 4.0.0 + Playwright 1.58.0

See full stack in: .project-management/input/technologies.md
```

---

#### Option 2: AI Recommendation

**If user selects [2]:**

1. **Read project inputs:**
   - `.project-management/input/scope.md`
   - `.project-management/input/backlog.md`
   - `.project-management/input/constraints.md`

2. **Analyze requirements:**
   - Project type (e-commerce, SaaS, dashboard, etc.)
   - Features needed (auth, payments, real-time, etc.)
   - Scale requirements
   - Team size and experience
   - Budget constraints
   - Performance requirements

3. **Generate recommendation:**

```
🤖 AI Stack Recommendation

Based on your project analysis:

Project Type: {{type}} (e-commerce / SaaS / dashboard / etc.)
Key Features: {{features}}
Scale: {{scale}} (small / medium / large)
Team: {{team_size}} developer(s)

📦 RECOMMENDED STACK:

Frontend:
- {{framework}} {{version}}
  Why: {{reason}}

Backend:
- {{backend}} {{version}}
  Why: {{reason}}

Database:
- {{database}} {{version}}
  Why: {{reason}}

Testing:
- {{test_framework}}
  Why: {{reason}}

Deployment:
- {{hosting}}
  Why: {{reason}}

💰 Estimated Monthly Cost: {{cost}}
⏱️  Setup Time: {{time}}
📈 Scalability: {{scalability}}

Approve this stack? [Yes/Modify/Start Over]
```

4. **Wait for approval**
   - If "Yes": Write to `technologies.md`
   - If "Modify": Ask what to change, regenerate
   - If "Start Over": Go back to stack selection

---

#### Option 3: Custom Setup

**If user selects [3]:**

**Use `.project-management/defaults/stack-questions.md` as a guide.**

**Ask questions sequentially:**

1. **Project Type:**
   ```
   What type of project are you building?
   [1] Web Application (Full-stack)
   [2] API Backend (REST/GraphQL)
   [3] Frontend Only (SPA)
   [4] Mobile App
   [5] Desktop App
   [6] CLI Tool
   [7] Library/Package
   ```

2. **Backend Framework** (if applicable):
   ```
   Which backend framework?
   [1] Express.js
   [2] Fastify
   [3] NestJS
   [4] React Router 7 (full-stack)
   [5] Other (specify)
   ```

3. **Database:**
   ```
   Which database?
   [1] PostgreSQL
   [2] MySQL
   [3] MongoDB
   [4] SQLite
   [5] Other (specify)
   ```

4. **Frontend Framework:**
   ```
   Which frontend framework?
   [1] React
   [2] Vue.js
   [3] Svelte
   [4] Angular
   [5] Other (specify)
   ```

5. **Styling:**
   ```
   How will you style the application?
   [1] Tailwind CSS
   [2] CSS Modules
   [3] Styled Components
   [4] Sass/SCSS
   [5] Other (specify)
   ```

6. **Testing:**
   ```
   Which testing frameworks?
   Unit: [1] Vitest  [2] Jest  [3] Mocha
   E2E:  [1] Playwright  [2] Cypress  [3] Puppeteer
   ```

7. **Build Tool:**
   ```
   Which build tool?
   [1] Vite
   [2] Webpack
   [3] Turbopack
   [4] Other (specify)
   ```

8. **Deployment:**
   ```
   Where will you deploy?
   [1] Railway
   [2] Vercel
   [3] AWS
   [4] DigitalOcean
   [5] Other (specify)
   ```

**After all questions answered:**
- Generate `technologies.md` with all selections
- Display summary
- Continue to i18n setup

---

### STEP 2: INTERNATIONALIZATION (i18n) SETUP

**Ask user about i18n requirements:**

```
🌐 Internationalization (i18n)

Does your project need multi-language support?

[1] Yes - Configure i18n
[2] No - Skip i18n
```

**Wait for user selection: 1 or 2**

---

#### If User Selects [1] Yes - Configure i18n:

**Ask which languages to support:**

```
Which languages do you want to support?

Select all that apply (multi-select):

[✓] English (en) - Default (required)
[ ] German (de)
[ ] French (fr)
[ ] Spanish (es)
[ ] Italian (it)
[ ] Portuguese (pt)
[ ] Dutch (nl)
[ ] Polish (pl)
[ ] Russian (ru)
[ ] Chinese (zh)
[ ] Japanese (ja)
[ ] Korean (ko)
[ ] Serbian (sr)
[ ] Croatian (hr)
[ ] Other (specify)

Type language codes separated by commas: en,de,sr
```

**After user provides languages:**

1. **Create `.project-management/rules/I18N-RULES.md`:**

```markdown
# Internationalization (i18n) Rules

**Status:** ✅ ENABLED

---

## Supported Languages

**Default Language:** English (en)

**Additional Languages:**
{{For each selected language:}}
- {{language_name}} ({{code}})

---

## Framework

**i18n Library:** i18next + react-i18next + remix-i18next
(From default stack)

**Translation Files Location:**
```
public/locales/
├── en/
│   └── translation.json
├── de/
│   └── translation.json
└── sr/
    └── translation.json
```

---

## Requirements

### All User-Facing Text MUST Be Translatable

**✅ CORRECT:**
```typescript
import { useTranslation } from 'react-i18next';

function Welcome() {
  const { t } = useTranslation();
  return <h1>{t('welcome.title')}</h1>;
}
```

**❌ INCORRECT:**
```typescript
function Welcome() {
  return <h1>Welcome to our app</h1>; // Hardcoded!
}
```

---

## Task Completion Criteria

**Story is NOT complete until:**
- [ ] All user-facing text uses translation keys
- [ ] Translation keys added to ALL language files ({{languages}})
- [ ] No hardcoded text in components
- [ ] Translation keys follow naming convention: `section.subsection.key`

---

## Translation Key Naming Convention

```
auth.login.title             → "Login"
auth.login.emailLabel        → "Email Address"
auth.login.passwordLabel     → "Password"
auth.login.submitButton      → "Sign In"
auth.login.forgotPassword    → "Forgot Password?"

dashboard.welcome            → "Welcome, {{name}}!"
dashboard.stats.users        → "Total Users"
dashboard.stats.revenue      → "Revenue"

errors.network.title         → "Connection Error"
errors.network.message       → "Please check your internet connection"
errors.validation.required   → "This field is required"
```

---

## Validation Before Marking Story Complete

**Execute these checks:**

1. **Search for hardcoded strings:**
   ```bash
   grep -r "\"[A-Z].*\"" src/components --include="*.tsx"
   grep -r "'[A-Z].*'" src/components --include="*.tsx"
   ```

2. **Verify translation files exist:**
   ```bash
   ls public/locales/{{lang}}/translation.json
   ```

3. **Check all keys present in all languages:**
   - en/translation.json has key `auth.login.title`
   - de/translation.json has key `auth.login.title`
   - sr/translation.json has key `auth.login.title`

---

**Last Updated:** {{date}}
```

2. **Create initial translation file structure:**

```bash
mkdir -p public/locales/{en,de,sr}
touch public/locales/en/translation.json
touch public/locales/de/translation.json
touch public/locales/sr/translation.json
```

3. **Create sample translation.json:**

```json
{
  "common": {
    "loading": "Loading...",
    "save": "Save",
    "cancel": "Cancel",
    "delete": "Delete",
    "edit": "Edit"
  },
  "auth": {
    "login": {
      "title": "Login",
      "emailLabel": "Email Address",
      "passwordLabel": "Password",
      "submitButton": "Sign In"
    }
  }
}
```

4. **Display confirmation:**

```
✅ i18n Configured Successfully

Default Language: English (en)
Additional Languages: German (de), Serbian (sr)

Translation Files Created:
✅ public/locales/en/translation.json
✅ public/locales/de/translation.json
✅ public/locales/sr/translation.json

⚠️  IMPORTANT: All user-facing text MUST use translation keys!

See rules: .project-management/rules/I18N-RULES.md
```

---

#### If User Selects [2] No - Skip i18n:

**Display:**
```
✅ i18n Skipped

No translation requirements.
All text can be hardcoded.

I18N-RULES.md will not be created.
```

**Do NOT create I18N-RULES.md file.**

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

1. **`prd.md`** - Product Requirements Document
   - Use template: `.project-management/templates/prd-template.md`
   - Extract project vision from scope.md
   - List all features from backlog.md organized by priority
   - Define success metrics
   - Document assumptions and risks

2. **`technical-spec.md`** - Technical Specification
   - Use template: `.project-management/templates/technical-spec-template.md`
   - Detail the technology stack from technologies.md
   - Design database schema based on requirements
   - Define API endpoints for features
   - Specify security and performance requirements

3. **`architecture.md`** - System Architecture
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

1. **`current-status.md`** - Initialize with project start status
   - Use `.project-management/templates/progress-template.md`
   - Set all metrics to 0%
   - Set project start date
   - List Phase 1 as current phase

2. **`completed.md`** - Empty initially (for tracking completed work)

3. **`blockers.md`** - Empty initially (for tracking blockers)

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

**Version:** 3.0 (Phase-Based with Stack & i18n)
**Updated:** 2026-03-27
