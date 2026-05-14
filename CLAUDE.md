# AI Developer Guidelines

**Reusable project management system for Claude-assisted development.**

Provides: Planning, phases, client doc processing, standards, progress tracking, automated execution.

**Setup:** Copy `.project-management/` → Customize `input/*.md` → Run `/init-project`

---

## 📋 DOCUMENT HIERARCHY

**Read priority order:**

1. **Project Planning** (`.project-management/`)
   - `input/scope.md`, `input/backlog/` (modular: `phase-*.md` + `future.md` + `README.md`)
   - `input/screens/screen-map.md` (web CMS / mobile / web+admin projects only)
   - `output/docs/technical-spec.md`
   - `output/phases/phase-N.md`

2. **Core Standards** (`CLAUDE.md` - this file)

3. **Specialized Rules** (`.claude/rules/`) - load by category; mandatory always, conditional when the trigger applies

   **Always (every story / bug):**
   - `code-quality.md` - **SOLID & DRY principles (MANDATORY)**
   - `documentation.md` - **Writing rules: English-only, file sizes, style, quality checklist (MANDATORY)**
   - `documentation-templates.md` - Templates: user stories, technical tasks, bugs, API endpoint docs
   - `documentation-extras.md` - Code comments, diagrams, tools, good/bad examples
   - `permissions.md` - **Settings file behavior: NEVER auto-modify settings.json (CRITICAL)**
   - `permissions-patterns.md` / `permissions-examples.md` - Pattern syntax + full settings.json templates
   - `testing.md` - Test types, API status-code matrix (200/400/401/403/404/500), coverage
   - `git.md` - Conventional commits, NO AI credits
   - `stack-specific.md` - Middleware, response envelope, Zod env schema, performance patterns

   **Conditional — load when the trigger fires:**
   - `api-documentation.md` - When HTTP endpoint touched: schema validation + doc block + drift check
   - `api-versioning.md` - When HTTP endpoint changed: `/api/v{N}/` versioning + change-propagation gate (docs + Zod + ALL tests + consumer code in same PR)
   - `api-first.md` - When frontend story: Phase A contract verification before any frontend code
   - `error-handling-and-logging.md` - When handler / service / logger touched: typed errors at single boundary, structured logs, NO PII / secrets
   - `security-and-auth.md` - When auth / authorization / session / secret touched: default-deny middleware, IDOR check, cookie config, security headers, audit log
   - `enums-and-constants.md` - When DB / enum / state value touched: `SCREAMING_SNAKE_CASE` wire format across all layers
   - `database.md` - When schema / migration touched: Prisma migration workflow, never `db push` in prod
   - `screen-driven-backlog.md` - When frontend (web/mobile) story written: one screen per story + `**API Endpoints Used:**` table
   - `screen-inventory.md` - When project is web CMS / mobile / web+admin: consolidated screen map (`input/screens/screen-map.md`)
   - `anonymization.md` - When generating PRD / backlog / spec / status from input docs: replace personal names with role labels (`the PM`, `the client`, `the stakeholder`)

4. **Project-Specific Rules** (`.project-management/rules/`)
   - `project-rules.md` - **ALWAYS read** (core project conventions)
   - `I18N-RULES.md` - **CONDITIONAL** (read ONLY if i18n is required)
   - `TESTING-RULES.md` - **CONDITIONAL** (read ONLY if project-specific testing needed)

**Conditional Rules Pattern:**
- ✅ **If file exists AND is configured:** Apply all rules from that file
- ❌ **If file missing or disabled:** Skip those requirements entirely
- 📝 **How to disable:** Delete the file or leave placeholders unconfigured

**Conflicts:** Project rules > Specialized rules > Core standards

---

## 🎯 COMMANDS & TOOLS

**PM:** `/init-project`, `/project-status` (live view: `output/progress/DASHBOARD.md`)
**Execution:** `/execute-work phase/epic/story` (with automatic plan mode, testing, commits)
**Testing:** `/run-tests all/unit/integration/e2e/coverage/story`
**Implementation:** TodoWrite (break down stories, only ONE task `in_progress`)
**Open questions:** `/process-client-docs` runs an interactive Q&A gate (STEP 5) — skipped questions persist to `input/open-questions.md`; resume with `/resolve-questions [--priority Px | Q-NNN]`. Pattern lives in `.claude/commands/modules/interactive-clarifications.md` for future commands.

---

## CRITICAL PRE-IMPLEMENTATION

**Before ANY code changes:**

1. **READ TECHNICAL SPEC**
   - **MANDATORY:** Consult `output/docs/technical-spec.md` first
   - Understand architecture decisions and patterns
   - Follow established conventions
   - If spec conflicts with code, ask which is correct

2. **READ EXISTING CODE**
   - Read files before modifying
   - Understand current architecture
   - Never assume - verify by reading

3. **PLAN (MANDATORY FOR IMPLEMENTATION)**
   - Phase/Epic/Story: `/execute-work` (auto-enters plan mode)
   - Feature: TodoWrite
   - **Plan mode REQUIRED** before any implementation
   - Update status in real-time

4. **ANALYZE**
   - [ ] Read technical spec
   - [ ] Read related files
   - [ ] Check patterns
   - [ ] Identify breaking changes
   - [ ] Review dependencies

---

## CODING STANDARDS

**Naming:** Files `kebab-case.ts`, Components `PascalCase`, Functions `camelCase`, Constants `UPPER_SNAKE_CASE`
**Functions:** Max 50 lines, single responsibility, pure when possible, clear names
**Imports:** 1) External libraries 2) Internal absolute 3) Relative 4) Styles

---

## MUST DO

- **Error Handling:** Try-catch async, meaningful messages
- **Testing:** ALL features, ALL API codes (200/400/401/403/404/500), 80%+ coverage → `.claude/rules/testing.md`
- **Security:** No secrets, validate inputs, parameterized queries, OWASP Top 10
- **Documentation:** **MANDATORY** - Update ALL relevant docs (tech spec, API docs, README, CHANGELOG, user guide)
- **Documentation Language:** **ALL documentation MUST be written in English only**:
  - ✅ Code comments and docstrings
  - ✅ README files and guides
  - ✅ Technical specs and architecture docs
  - ✅ API documentation
  - ✅ CHANGELOG and release notes
  - ✅ Commit messages
  - ✅ Project management documents (scope, backlog, phases)
  - ✅ User stories and acceptance criteria
  - ❌ **NO exceptions** - English is mandatory for ALL written documentation
  - ℹ️ Note: This applies to documentation only, not application i18n content
- **Code Quality:** SOLID & DRY principles (MANDATORY) → `.claude/rules/code-quality.md`

---

## MUST NOT DO

❌ **Over-engineering:** No unrequested features, no out-of-scope refactoring
❌ **Premature abstractions:** No helpers for one-time use, 3 similar lines > abstraction
❌ **Unnecessary mods:** No docstrings on unchanged code, no self-evident comments
❌ **Compatibility hacks:** Delete unused code completely
❌ **Excessive validation:** Only at system boundaries
❌ **Guessing:** Ask questions if unclear

---

## GIT & DATABASE

**Git:** Conventional commits (`feat:`, `fix:`), **NO AI credits** → `.claude/rules/git.md`
**Database:** Always migrations (not `db push`), review SQL → `.claude/rules/database.md`

---

## 📋 PLAN MODE (MANDATORY)

**What is Plan Mode:**
Plan mode is a required pre-implementation phase where Claude analyzes requirements, creates a detailed plan, and waits for user approval before writing code.

**When Plan Mode Activates:**
- **Automatically** when running `/execute-work phase/epic/story`
- **Automatically** when running `/init-project`
- **Automatically** for any command with "plan" in the name
- **Manually** when user says "enter plan mode" or "plan this"

**Plan Mode Workflow:**

```
📋 [PLAN MODE ACTIVATED]

Step 1: READ ALL CONTEXT
✅ Technical spec
✅ Backlog
✅ Core standards (CLAUDE.md)
✅ Code quality rules (SOLID & DRY)
✅ Testing requirements
✅ Git workflow
✅ Project-specific rules
{{✅ i18n rules (if exists)}}
{{✅ Project testing rules (if exists)}}

Step 2: ANALYZE SCOPE
- Identify all stories/tasks in scope
- Map dependencies
- Calculate estimates
- Identify risks

Step 3: CREATE DETAILED PLAN
🎯 Scope: [What will be implemented]
📊 Breakdown: [Tasks, subtasks, tests]
📈 Estimates: [Time, story points, tests]
⚠️  Dependencies: [What's needed first]
🔴 Risks: [Potential issues]
✅ Success Criteria: [Definition of done]

Step 4: WAIT FOR APPROVAL
✅ Proceed with implementation? [Yes/No/Revise]

[EXITING PLAN MODE - ENTERING IMPLEMENTATION MODE]
```

**Plan Mode Requirements:**
1. NEVER skip plan mode for `/execute-work`
2. NEVER start coding without plan approval
3. ALWAYS read ALL required context files
4. ALWAYS show detailed breakdown
5. ALWAYS wait for explicit user approval

**Exit Plan Mode:**
- User responds "Yes" → Start implementation
- User responds "No" → Cancel and exit
- User responds "Revise" → Modify plan and re-present

---

## WORKFLOW

### Automated Workflow (with /execute-work)
```
0. PLAN MODE → Analyze, create plan, get approval
1. IMPLEMENT → Code changes following standards
2. TEST → Auto-run tests (unit, integration, E2E)
3. VALIDATE → Coverage > 80%, all status codes, i18n
4. COMMIT → Auto-commit (NO AI credits)
5. UPDATE → Auto-update progress tracking
6. REPEAT → Next story
```

### Manual Workflow (without /execute-work)
```
0. CONTEXT → Phase, user story, tech spec
1. UNDERSTAND → Read, ask questions
2. PLAN → TodoWrite breakdown (or enter plan mode manually)
3. ANALYZE → Read files, patterns
4. IMPLEMENT → Code changes
5. TEST → Run tests (/run-tests or manually)
6. VERIFY → Review, security
7. UPDATE → DASHBOARD.md auto-updates on /execute-work (edit progress files manually if outside automation)
8. COMMIT → When requested (follow git.md rules)
```

---

## QUALITY GATES - MASTER CHECKLIST

**Before marking ANY task complete:**

**Code:**
- [ ] SOLID & DRY principles followed
- [ ] No TypeScript/linting errors
- [ ] Follows project conventions

**Testing:**
- [ ] All tests passing (unit, integration, e2e)
- [ ] All API codes tested (200/400/401/403/404/500)

**Documentation:**
- [ ] Tech spec consulted & updated (if architecture/schema changed)
- [ ] API docs, README, CHANGELOG updated
- [ ] User guide updated (if user-facing)

**Conditional Requirements (check if files exist):**
- [ ] Translations added (if `I18N-RULES.md` exists)
- [ ] Project-specific tests implemented (if `TESTING-RULES.md` exists)

**Security & Quality:**
- [ ] No secrets committed
- [ ] No security vulnerabilities
- [ ] No over-engineering

---

## TECHNICAL DEBT & PM INTEGRATION

**Tech Debt:** When touching ANY file, fix related tech debt incrementally
**PM Flow:** Phase → Tech spec → Backlog → Epic → Story → `/execute-work` → Auto-test → Auto-commit → Auto-update

---

## RELATED DOCS

**Project & docs:**
- 📋 [Project Management](.project-management/README.md)
- 📖 [Documentation rules](.claude/rules/documentation.md) — language, file size, style
- 📖 [Documentation templates](.claude/rules/documentation-templates.md) — user stories, tasks, bugs, API endpoint docs
- 📖 [Documentation extras](.claude/rules/documentation-extras.md) — code comments, diagrams, tools

**Code & quality:**
- ⭐ [Code Quality - SOLID & DRY](.claude/rules/code-quality.md) **← MANDATORY**
- 🧪 [Testing](.claude/rules/testing.md) — API status-code matrix
- 📝 [Git](.claude/rules/git.md) — conventional commits, NO AI credits
- ⚙️ [Stack-Specific](.claude/rules/stack-specific.md) — middleware, response envelope

**API / HTTP layer:**
- 🌐 [API documentation](.claude/rules/api-documentation.md) — schema validation + doc block + drift check
- 🔢 [API versioning](.claude/rules/api-versioning.md) — `/api/v{N}/` + change-propagation gate
- 🔌 [API-first](.claude/rules/api-first.md) — contract verification before frontend code

**Backend safety:**
- 🚨 [Error handling & logging](.claude/rules/error-handling-and-logging.md) — typed errors, structured logs, no PII
- 🔐 [Security & auth](.claude/rules/security-and-auth.md) — default-deny, IDOR, cookie sessions, headers
- 🗄️ [Database](.claude/rules/database.md) — migrations only
- 🏷️ [Enums & constants](.claude/rules/enums-and-constants.md) — `SCREAMING_SNAKE_CASE` wire format

**Frontend (web / mobile):**
- 📱 [Screen-driven backlog](.claude/rules/screen-driven-backlog.md) — one screen per story
- 🗺️ [Screen inventory](.claude/rules/screen-inventory.md) — consolidated screen map (web CMS / mobile)

**Settings & PII:**
- 🚫 [Permissions](.claude/rules/permissions.md) — NEVER auto-modify settings.json
- 📐 [Permissions patterns](.claude/rules/permissions-patterns.md) / [examples](.claude/rules/permissions-examples.md)
- 👤 [Anonymization](.claude/rules/anonymization.md) — role labels in generated docs

---

**Version:** 3.3.0 (Rules Expansion + Cross-Layer Conventions)
**Updated:** 2026-05-11
