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

**Ask questions sequentially:**

**Project Type:**
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

**Backend Framework** (if applicable):
```
Which backend framework?
[1] Express.js
[2] Fastify
[3] NestJS
[4] React Router 7 (full-stack)
[5] Other (specify)
```

**Database:**
```
Which database?
[1] PostgreSQL
[2] MySQL
[3] MongoDB
[4] SQLite
[5] Other (specify)
```

**Frontend Framework:**
```
Which frontend framework?
[1] React
[2] Vue.js
[3] Svelte
[4] Angular
[5] Other (specify)
```

**Styling:**
```
How will you style the application?
[1] Tailwind CSS
[2] CSS Modules
[3] Styled Components
[4] Sass/SCSS
[5] Other (specify)
```

**Testing:**
```
Which testing frameworks?
Unit: [1] Vitest  [2] Jest  [3] Mocha
E2E:  [1] Playwright  [2] Cypress  [3] Puppeteer
```

**Build Tool:**
```
Which build tool?
[1] Vite
[2] Webpack
[3] Turbopack
[4] Other (specify)
```

**Deployment:**
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

**Next Step:** Return to main `init-project.md` STEP 2 (i18n setup)
