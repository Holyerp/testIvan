# Custom Stack Configuration Questions

**Purpose:** This file contains questions Claude asks when user selects "Custom Stack" option during project initialization.

---

## Question Flow

### 1. Project Type
**Question:** "What type of project are you building?"

**Options:**
- Web Application (Full-stack)
- API Backend (REST/GraphQL)
- Frontend Only (SPA)
- Mobile App (React Native)
- Desktop App (Electron)
- CLI Tool
- Library/Package

**Why:** Determines overall architecture and required technologies.

---

### 2. Backend Framework (if applicable)

**Question:** "Which backend framework do you prefer?"

**Options:**
- **Node.js:**
  - Express.js (minimalist, flexible)
  - Fastify (high performance)
  - NestJS (enterprise, Angular-style)
  - React Router 7 (full-stack with SSR)
  - Koa (modern, lightweight)
- **Other:**
  - Python (FastAPI, Django, Flask)
  - PHP (Laravel, Symfony)
  - Go (Gin, Echo)
  - Ruby (Rails, Sinatra)
  - Java (Spring Boot)

**Follow-up:** "TypeScript or JavaScript?" (for Node.js)

---

### 3. Database

**Question:** "Which database(s) will you use?"

**Options:**
- **Relational:**
  - PostgreSQL (recommended for most projects)
  - MySQL / MariaDB
  - SQLite (lightweight, local dev)
- **NoSQL:**
  - MongoDB (document store)
  - Redis (caching, sessions)
  - DynamoDB (AWS)
- **ORM/ODM:**
  - Prisma (TypeScript, auto-generated types)
  - TypeORM (decorator-based)
  - Sequelize (mature, feature-rich)
  - Mongoose (MongoDB)
  - Drizzle (SQL-like, type-safe)

---

### 4. Frontend Framework

**Question:** "Which frontend framework?"

**Options:**
- React (most popular, large ecosystem)
- Vue.js (progressive, beginner-friendly)
- Svelte (compile-time, no virtual DOM)
- Angular (enterprise, opinionated)
- Solid.js (React-like, better performance)
- Vanilla JS (no framework)

**If React selected:**
- "React version?" (18.x / 19.x)
- "Full-stack framework?" (React Router 7 / Next.js / Remix / None)

---

### 5. State Management (if applicable)

**Question:** "How will you manage state?"

**Options:**
- **Global State:**
  - Redux Toolkit (most popular)
  - Zustand (lightweight, simple)
  - Jotai (atomic state)
  - MobX (reactive)
  - Context API (built-in)
- **Server State:**
  - TanStack Query (React Query)
  - SWR (Vercel)
  - RTK Query (Redux)
- **Form State:**
  - React Hook Form (performance)
  - Formik (feature-rich)
  - Native (uncontrolled forms)

---

### 6. Styling

**Question:** "How will you style your application?"

**Options:**
- Tailwind CSS (utility-first, recommended)
- CSS Modules (scoped CSS)
- Styled Components (CSS-in-JS)
- Emotion (CSS-in-JS)
- Sass/SCSS (preprocessor)
- Vanilla CSS (no framework)

**If Tailwind selected:**
- "Component library?" (shadcn/ui / Headless UI / DaisyUI / None)

---

### 7. Authentication

**Question:** "How will you handle authentication?"

**Options:**
- **Strategy:**
  - JWT (stateless, scalable)
  - Session-based (cookies, server state)
  - OAuth 2.0 / OpenID Connect (third-party)
  - Passkeys (WebAuthn, passwordless)
- **Provider:**
  - Custom implementation
  - Auth0 (managed service)
  - Clerk (modern, developer-friendly)
  - Supabase Auth (open-source)
  - Firebase Auth
  - AWS Cognito

---

### 8. Validation

**Question:** "Schema validation library?"

**Options:**
- Zod (TypeScript-first, recommended)
- Yup (JSON Schema-like)
- Joi (Node.js classic)
- Ajv (JSON Schema validator)
- class-validator (decorator-based)

---

### 9. Testing

**Question:** "Which testing frameworks?"

**Options:**
- **Unit Testing:**
  - Vitest (fast, Vite-based)
  - Jest (most popular)
  - Mocha (flexible)
- **Component Testing:**
  - React Testing Library
  - Enzyme
  - Testing Library (for other frameworks)
- **E2E Testing:**
  - Playwright (modern, recommended)
  - Cypress (popular, mature)
  - Puppeteer (Chrome only)

---

### 10. Internationalization (i18n)

**Question:** "Do you need multi-language support?"

**Options:**
- Yes → Configure languages
- No → Skip i18n

**If Yes:**
"Which languages?" (multi-select)
- English (en) - Default
- German (de)
- French (fr)
- Spanish (es)
- Italian (it)
- Portuguese (pt)
- Dutch (nl)
- Polish (pl)
- Russian (ru)
- Chinese (zh)
- Japanese (ja)
- Korean (ko)
- Serbian (sr)
- Croatian (hr)
- Other (specify)

**Framework:**
- i18next (React, Vue, Node.js)
- react-intl (React)
- vue-i18n (Vue)
- Built-in (Angular, Svelte)

---

### 11. Build Tool

**Question:** "Which build tool?"

**Options:**
- Vite (fast, modern, recommended)
- Webpack (mature, configurable)
- Turbopack (Next.js, experimental)
- esbuild (extremely fast)
- Rollup (library bundling)
- Parcel (zero-config)

---

### 12. Deployment

**Question:** "Where will you deploy?"

**Options:**
- **Full-Stack:**
  - Railway (simple, managed DB)
  - Vercel (Next.js optimized)
  - Netlify (Jamstack)
  - Fly.io (global edge)
  - Render (free tier)
- **Cloud Providers:**
  - AWS (scalable, complex)
  - Google Cloud Platform
  - Azure
  - DigitalOcean
- **Containers:**
  - Docker + Docker Compose
  - Kubernetes
- **Self-hosted:**
  - VPS (own server)

---

### 13. Additional Services

**Question:** "Will you use any of these services?" (multi-select)

**Options:**
- Email (Resend, SendGrid, AWS SES, Mailgun)
- File Storage (AWS S3, Cloudinary, Supabase Storage)
- Payment Processing (Stripe, PayPal)
- Analytics (Google Analytics, Plausible, PostHog)
- Error Tracking (Sentry, Rollbar, Bugsnag)
- Monitoring (New Relic, Datadog)
- CMS (Strapi, Contentful, Sanity)

---

### 14. Development Tools

**Question:** "Which dev tools do you want configured?" (multi-select)

**Options:**
- ESLint (linting)
- Prettier (code formatting)
- Husky (git hooks)
- lint-staged (pre-commit linting)
- Commitlint (commit message validation)
- TypeScript (type checking)
- Docker (containerization)
- GitHub Actions / GitLab CI (CI/CD)

---

## AI Analysis Mode

**When user selects "AI Recommendation":**

Claude will:
1. Read `.project-management/input/scope.md`
2. Read `.project-management/input/backlog.md`
3. Read `.project-management/input/constraints.md`
4. Analyze project requirements
5. Suggest optimal tech stack based on:
   - Project type (e-commerce, SaaS, dashboard, etc.)
   - Team size and experience
   - Performance requirements
   - Budget constraints
   - Time constraints
   - Scalability needs
6. Explain why each technology was chosen
7. Ask for user approval or modifications

**Example AI Suggestion:**

```markdown
Based on your project requirements:

**Recommended Stack:**

Frontend:
- React 19 + React Router 7 (SSR for better SEO)
- Tailwind CSS 4 (fast styling, Oxide engine)
- shadcn/ui (accessible components)

Backend:
- React Router 7 (unified frontend/backend)
- PostgreSQL 16 (relational data)
- Prisma 6 (type-safe ORM)

Rationale:
- E-commerce needs SEO → React Router 7 SSR
- Complex product catalog → PostgreSQL relational DB
- Small team (1-2 devs) → Full-stack framework simplicity
- Budget-conscious → Railway deployment (affordable)

Approve this stack? [Yes/Modify/Start Over]
```

---

## Output Format

After gathering answers, Claude generates:

1. **Updated `technologies.md`** with all selections
2. **`package.json`** dependencies
3. **Configuration files** (tsconfig.json, eslint.config.js, etc.)
4. **Environment variables template** (.env.example)
5. **Initial project structure**

---

**Version:** 1.0
**Last Updated:** 2026-03-27
