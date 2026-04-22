# Init Project — Structure Setup Module

**Purpose:** Ask the user which project layout to use and produce the directory skeleton + config stubs.

**Parent command:** `/init-project`
**Companion module:** `init-project-monorepo-templates.md` (concrete package.json / turbo.json / pnpm-workspace.yaml templates)

---

## STEP 0: Project Structure Selection

Present this prompt to the user:

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🏗️  PROJECT STRUCTURE SETUP
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

What type of project are you building?

[1] Backend Only
    → Single backend API application
    → Express/NestJS/FastAPI server

[2] Backend + Mobile App (Monorepo) ⭐ RECOMMENDED
    → Backend API + React Native/Flutter mobile
    → Shared TypeScript types and utilities
    → Single repo, unified development

[3] Backend + Web + Mobile (Full Monorepo)
    → Backend API + Web + Mobile
    → Maximum code sharing

[4] Web Only
    → Frontend-only application
    → React/Next.js/Vue
    → Consumes external APIs

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Enter your choice [1-4]:
```

---

## Option 1: Backend Only

**Structure:**
```
project-root/
├── src/
├── tests/
├── package.json
├── tsconfig.json
└── .project-management/
```

**Setup steps:**
1. Standard single-app structure.
2. No workspace config needed.
3. Single `package.json`.
4. Proceed to tech-stack selection.

**`technologies.md` note:**
```markdown
**Project Type:** Backend Only
**Repository Type:** Single Application
```

---

## Option 2: Backend + Mobile (Monorepo) ⭐

**Structure:**
```
project-root/
├── apps/
│   ├── backend/
│   └── mobile/
├── packages/
│   ├── shared-types/
│   ├── api-client/
│   └── shared-utils/
├── .project-management/
├── package.json              # Root workspace config
├── pnpm-workspace.yaml
├── turbo.json
└── .gitignore
```

**Concrete templates** (package.json, turbo.json, pnpm-workspace.yaml, shared packages, .gitignore, README): see **`init-project-monorepo-templates.md`**.

**`technologies.md` note:**
```markdown
**Project Type:** Backend + Mobile App
**Repository Type:** Monorepo (pnpm workspaces + Turborepo)

**Applications:**
- Backend: Node.js + TypeScript + Express (or user-selected stack)
- Mobile: React Native + Expo + TypeScript

**Shared Packages:**
- shared-types: TypeScript interfaces/types
- api-client: API wrapper for mobile
- shared-utils: Common utilities

**Package Manager:** pnpm 9.x
**Build Tool:** Turborepo 2.x
```

---

## Option 3: Backend + Web + Mobile (Full Monorepo)

**Structure:** like Option 2 plus `apps/web/` and optionally `packages/ui-components/`.

```
project-root/
├── apps/
│   ├── backend/
│   ├── web/
│   └── mobile/
├── packages/
│   ├── shared-types/
│   ├── api-client/
│   ├── ui-components/        # Shared UI (if using React Native Web)
│   └── shared-utils/
├── package.json
├── pnpm-workspace.yaml
└── turbo.json
```

**Additional setup:**
- Add `apps/web/package.json` (Next.js/Remix/Vite).
- Add `packages/ui-components/` if using shared React components.
- Same workspace + Turborepo config as Option 2 (see templates module).

---

## Option 4: Web Only

**Structure:**
```
project-root/
├── src/
├── public/
├── package.json
└── .project-management/
```

**Setup steps:**
1. Standard frontend structure.
2. No monorepo.
3. Configure for Next.js/Vite/CRA.

---

## Backlog Prefix Convention (monorepo)

Use component prefixes in `input/backlog/phase-*.md` stories for monorepo projects:

```markdown
## Epic 1: User Authentication
- **US-001**: [BE] JWT authentication API (5 pts)
- **US-002**: [Mobile] Login screen with biometric (3 pts)
- **US-003**: [Shared] Auth types and interfaces (2 pts)

## Epic 3: Shopping Cart
- **US-008**: [BE] Cart session management (5 pts)
- **US-009**: [Mobile] Cart UI with animations (8 pts)
- **US-010**: [Full-stack] Cart sync across devices (13 pts)
```

**Prefix legend:**
- `[BE]` — Backend only
- `[Mobile]` — Mobile only
- `[Web]` — Web only (Option 3)
- `[Shared]` — Shared package
- `[Full-stack]` — Touches multiple apps

---

## Post-Setup Actions

After the structure is created:

1. Run initial install: `pnpm install`
2. Verify workspace setup: `pnpm list --depth 0`
3. Update `technologies.md` with structure details
4. Proceed to **STEP 1: Tech Stack Selection** (`init-project-stack-selection.md`)

---

## Summary Display

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ PROJECT STRUCTURE CREATED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📦 Repository Type: Monorepo (Backend + Mobile)

📁 Structure Created:
   ├── apps/
   │   ├── backend/     ✅
   │   └── mobile/      ✅
   ├── packages/
   │   ├── shared-types/    ✅
   │   ├── api-client/      ✅
   │   └── shared-utils/    ✅
   └── Configuration files  ✅ (pnpm, turbo, .gitignore)

🔧 Package Manager: pnpm 9.x
🚀 Build Tool: Turborepo 2.x

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

**Version:** 2.0.0 (split from the original combined module)
**Last Updated:** 2026-04-21
**Related:**
- Parent: `.claude/commands/init-project.md`
- Templates: `init-project-monorepo-templates.md`
- Next: `init-project-stack-selection.md`
