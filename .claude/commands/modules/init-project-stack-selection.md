# Init Project - Tech Stack Selection Module

**Referenced by:** `init-project.md` STEP 1

---

## STEP 1: TECH STACK SELECTION

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

### Option 1: Default HolyEstate Stack

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

### Option 2: AI Recommendation

**If user selects [2]:**

**1. Read project inputs:**
- `.project-management/input/scope.md`
- `.project-management/input/backlog.md`
- `.project-management/input/constraints.md`

**2. Analyze requirements:**
- Project type (e-commerce, SaaS, dashboard, etc.)
- Features needed (auth, payments, real-time, etc.)
- Scale requirements
- Team size and experience
- Budget constraints
- Performance requirements

**3. Generate recommendation:**

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

**4. Wait for approval:**
- If "Yes": Write to `technologies.md`
- If "Modify": Ask what to change, regenerate
- If "Start Over": Go back to stack selection

---

### Option 3: Custom Setup

**If user selects [3]:**

**Use `.project-management/defaults/stack-questions.md` as a guide.**

> **Custom stack flow:** the user answers a sequence of single-layer AskUserQuestion calls. Top-3 most-common options per layer cover ~90% of choices; AskUserQuestion's native `Other` handles the rest with free-text + anonymization. Skip on any layer = use the recommended default.

#### Layer: project-type

```
question: "Project type?"
header: "project-type"
skippable: true
default: "Web Application (Full-stack)"
options:
  - label: "Web Application (Full-stack) (Recommended)"
    description: "SSR or hybrid web app with backend + frontend in one codebase"
  - label: "API Backend (REST/GraphQL)"
    description: "Backend-only service exposing HTTP/RPC APIs"
  - label: "Frontend Only (SPA)"
    description: "Client-side single-page app consuming an external API"
applies_to: [ input/technologies.md ]
```

#### Layer: backend

```
question: "Backend framework?"
header: "backend"
skippable: true
default: "Node.js + Express"
options:
  - label: "Node.js + Express (Recommended)"
    description: "Mature, minimal, widely understood — safe default for REST APIs"
  - label: "NestJS"
    description: "Opinionated, DI-driven, TypeScript-first — suits larger teams"
  - label: "Fastify"
    description: "High-performance Node.js framework with schema-based validation"
applies_to: [ input/technologies.md ]
```

#### Layer: database

```
question: "Database choice?"
header: "database"
skippable: true
default: "PostgreSQL"
options:
  - label: "PostgreSQL (Recommended)"
    description: "Battle-tested relational DB; pairs cleanly with Prisma"
  - label: "MySQL"
    description: "Widely supported relational DB; common in legacy stacks"
  - label: "MongoDB"
    description: "Document store; suits flexible/unstructured data"
applies_to: [ input/technologies.md ]
```

#### Layer: frontend

```
question: "Frontend framework?"
header: "frontend"
skippable: true
default: "React Router 7"
options:
  - label: "React Router 7 (Recommended)"
    description: "Full-stack React with SSR — matches the project's default stack"
  - label: "Next.js"
    description: "React meta-framework with SSR/ISR/RSC — strong ecosystem"
  - label: "Vite + React SPA"
    description: "Client-only SPA with fast dev server; no SSR"
applies_to: [ input/technologies.md ]
```

#### Layer: styling

```
question: "Styling approach?"
header: "styling"
skippable: true
default: "Tailwind CSS"
options:
  - label: "Tailwind CSS (Recommended)"
    description: "Utility-first CSS; fast iteration, small bundle"
  - label: "CSS Modules"
    description: "Scoped class names per component; no runtime overhead"
  - label: "shadcn/ui (Tailwind preset)"
    description: "Tailwind + copy-paste accessible components"
applies_to: [ input/technologies.md ]
```

#### Layer: testing

```
question: "Testing frameworks?"
header: "testing"
skippable: true
default: "Vitest + Playwright"
options:
  - label: "Vitest + Playwright (Recommended)"
    description: "Fast unit tests (Vitest) + reliable cross-browser E2E (Playwright)"
  - label: "Jest + Cypress"
    description: "Established unit (Jest) + interactive E2E (Cypress) combo"
  - label: "Vitest only (unit)"
    description: "Unit/integration only; defer E2E to a later phase"
applies_to: [ input/technologies.md ]
```

#### Layer: build

```
question: "Build tool?"
header: "build"
skippable: true
default: "Vite"
options:
  - label: "Vite (Recommended)"
    description: "Fast dev server + Rollup-based production build"
  - label: "Turbopack"
    description: "Next.js's incremental bundler (alpha-quality outside Next)"
  - label: "Webpack"
    description: "Mature, configurable bundler — heavier and slower than Vite"
applies_to: [ input/technologies.md ]
```

#### Layer: deploy

```
question: "Deployment target?"
header: "deploy"
skippable: true
default: "Railway"
options:
  - label: "Railway (Recommended)"
    description: "Zero-config PaaS with Postgres add-ons; matches default stack"
  - label: "Vercel"
    description: "Best-in-class for Next.js + static + serverless edge"
  - label: "AWS"
    description: "Full control + scale; highest ops complexity"
applies_to: [ input/technologies.md ]
```

**Per-layer behavior:**

- AskUserQuestion's native `Other` option lets the user type a custom value.
- On Skip, the loop uses `default:` and logs to `input/open-questions.md` per the standard module flow.
- On any free-text `Other` answer, run through `.claude/rules/anonymization.md` §3–4 (defensive — stack names usually have no PII but the rule is the boundary).
- Write the chosen value into `input/technologies.md` under the corresponding `## <Layer>` heading.

**After all layers answered:**
- Generate `technologies.md` with all selections
- Display summary
- Continue to i18n setup

---

**Next Step:** Return to main `init-project.md` STEP 2 (i18n setup)
