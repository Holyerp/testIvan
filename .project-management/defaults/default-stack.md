# Default HolyEstate Tech Stack

**Version:** 1.0
**Last Updated:** 2026-03-27
**Production Status:** ✅ Tested with 848 passing tests on Railway

---

## Core Framework

| Package | Version | Purpose |
|---------|---------|---------|
| **react** | ^19.0.0 | UI library |
| **react-dom** | ^19.0.0 | DOM rendering |
| **react-router** | ^7.13.0 | Full-stack framework (framework mode with SSR) |
| **@react-router/dev** | ^7.13.0 | React Router dev tooling / Vite plugin |
| **@react-router/node** | ^7.13.0 | Node.js adapter for SSR |
| **@react-router/serve** | ^7.13.0 | Production server |
| **typescript** | ^5.7.0 | Type system |
| **vite** | ^6.1.0 | Build tool |

**Why React Router 7 Framework Mode:**
- Single framework for both frontend and backend
- Built-in SSR/SSG support
- Type-safe routing and data loading
- No need for separate Express/Fastify backend

---

## Database & ORM

| Package | Version | Purpose |
|---------|---------|---------|
| **prisma** | ^6.19.0 | CLI and migration tool |
| **@prisma/client** | ^6.19.0 | Database client |
| **PostgreSQL** | 16.x | Database engine |

**Why Prisma:**
- Excellent TypeScript integration
- Auto-generated types from schema
- Migration system for production safety
- Great developer experience

---

## Authentication & Security

| Package | Version | Purpose |
|---------|---------|---------|
| **bcryptjs** | ^2.4.3 | Password hashing |
| **Built-in createCookieSessionStorage** | (react-router) | Session management |

**Authentication Strategy:** Cookie-based sessions (no JWT)

---

## Forms & Validation

| Package | Version | Purpose |
|---------|---------|---------|
| **react-hook-form** | ^7.54.0 | Form state management |
| **@hookform/resolvers** | ^3.9.0 | Zod resolver |
| **zod** | ^3.24.0 | Schema validation (shared client/server) |

**Why This Stack:**
- Type-safe validation across frontend and backend
- Excellent performance
- Minimal re-renders

---

## Internationalization (i18n)

| Package | Version | Purpose |
|---------|---------|---------|
| **i18next** | ^24.2.0 | Core i18n framework |
| **react-i18next** | ^15.4.0 | React bindings |
| **remix-i18next** | ^7.4.0 | SSR integration for RR7 framework mode |
| **i18next-fs-backend** | ^2.6.0 | Server-side translation loading |
| **i18next-http-backend** | ^3.0.0 | Client-side translation loading |
| **i18next-browser-languagedetector** | ^8.0.0 | Auto-detect user language |

**Configuration:**
- Translation files location: `/public/locales/{lang}/translation.json`
- Supported languages: Configured during project init
- SSR-compatible with React Router 7

---

## UI & Styling

| Package | Version | Purpose |
|---------|---------|---------|
| **tailwindcss** | ^4.1.0 | Utility-first CSS (v4 Oxide engine) |
| **@tailwindcss/vite** | ^4.1.0 | Vite plugin |
| **shadcn/ui** | latest | Component library |
| **radix-ui** | latest | Primitives (shadcn dependency) |
| **lucide-react** | ^0.475.0 | Icons |
| **clsx** | ^2.1.0 | Conditional classes |
| **tailwind-merge** | ^3.0.0 | Class conflict resolution |
| **class-variance-authority** | ^0.7.0 | Component variants |

**Why Tailwind CSS v4:**
- New Oxide engine (Rust-based, 10x faster)
- Better performance than v3
- Zero-runtime CSS-in-JS alternative

**Why shadcn/ui:**
- Copy-paste components (not npm package)
- Full control over component code
- Built on Radix UI (accessible by default)

---

## Data Tables & Charts

| Package | Version | Purpose |
|---------|---------|---------|
| **@tanstack/react-table** | ^8.21.0 | Headless data table |
| **recharts** | ^2.15.0 | Charting library |

---

## PDF Generation

| Package | Version | Purpose |
|---------|---------|---------|
| **@react-pdf/renderer** | ^4.3.0 | PDF from React components |

---

## Email

| Package | Version | Purpose |
|---------|---------|---------|
| **resend** | ^4.1.0 | Email delivery (production) |
| **react-email** | ^3.0.0 | Email templates as React components |
| **@react-email/components** | ^0.0.31 | Email primitives |

**Configuration:**
- Dev: Console fallback
- Prod: Resend API

---

## Testing

| Package | Version | Purpose |
|---------|---------|---------|
| **vitest** | ^4.0.0 | Unit/integration test runner |
| **@testing-library/react** | ^16.0.0 | Component testing |
| **@testing-library/jest-dom** | ^6.6.0 | DOM matchers |
| **@testing-library/user-event** | ^14.6.0 | User interaction simulation |
| **msw** | ^2.7.0 | API mocking |
| **@playwright/test** | ^1.58.0 | E2E testing |
| **@vitest/coverage-v8** | ^4.0.0 | Code coverage |

**Why Vitest:**
- Faster than Jest (Vite-powered)
- Better TypeScript support
- Compatible with Jest API

**Test Commands:**
```bash
npm test                    # All unit/integration tests
npm run test:unit          # Unit tests only
npm run test:integration   # Integration tests only
npm run test:e2e           # E2E tests (Playwright)
npm run test:coverage      # With coverage report
npm run test:watch         # Watch mode
```

**Coverage Requirements:**
- Overall: 80%+
- Critical paths: 95%+
- API status codes: 200/400/401/403/404/500 (all mandatory)

---

## Dev Tooling

| Package | Version | Purpose |
|---------|---------|---------|
| **eslint** | ^9.0.0 | Linting (flat config) |
| **prettier** | ^3.4.0 | Formatting |
| **prettier-plugin-tailwindcss** | ^0.6.0 | Tailwind class sorting |
| **husky** | ^9.1.0 | Git hooks |
| **lint-staged** | ^15.3.0 | Pre-commit lint runner |
| **tsx** | ^4.19.0 | TS execution for scripts |

---

## Deployment

| Service | Purpose |
|---------|---------|
| **Railway** | Hosting + managed PostgreSQL |

**Why Railway:**
- Simple deployment (git push)
- Managed PostgreSQL included
- Environment variables management
- Automatic SSL certificates
- Cost-effective for MVP/production

---

## Project Structure

```
project-root/
├── app/                          # React Router app directory
│   ├── routes/                   # Route modules (SSR)
│   ├── components/               # React components
│   ├── lib/                      # Utilities
│   ├── services/                 # Business logic
│   └── styles/                   # Global styles
│
├── prisma/
│   ├── schema.prisma            # Database schema
│   ├── migrations/              # Migration files
│   └── seed.ts                  # Seed data
│
├── public/
│   └── locales/                 # i18n translations
│       ├── en/
│       │   └── translation.json
│       └── [other-languages]/
│
├── tests/
│   ├── unit/                    # Unit tests
│   ├── integration/             # Integration tests
│   └── e2e/                     # E2E tests (Playwright)
│
├── .env.example                 # Environment variables template
├── package.json
├── tsconfig.json
├── vite.config.ts
├── tailwind.config.ts
└── playwright.config.ts
```

---

## Environment Variables

**Required:**
```bash
# Database
DATABASE_URL="postgresql://user:pass@host:5432/dbname"

# Session
SESSION_SECRET="your-secret-key-min-32-chars"

# Email (Production)
RESEND_API_KEY="re_xxxxx"

# Optional
NODE_ENV="development|production"
PORT="3000"
```

**Validation:**
Use Zod schemas to validate environment variables at startup.

---

## Performance Targets

- **First Contentful Paint:** < 1.5s
- **Time to Interactive:** < 3.5s
- **API Response Time (p95):** < 500ms
- **Database Query Time (p95):** < 100ms
- **Lighthouse Score:** > 90

---

## Browser Support

**Desktop:**
- Chrome (last 2 versions)
- Firefox (last 2 versions)
- Safari (last 2 versions)
- Edge (last 2 versions)

**Mobile:**
- iOS Safari (last 2 versions)
- Chrome Android (last 2 versions)

**Responsive Breakpoints:**
- Mobile: 375px - 768px
- Tablet: 768px - 1024px
- Desktop: 1024px+

---

## Security Practices

- ✅ HTTPS only (enforced)
- ✅ CORS properly configured
- ✅ Rate limiting on API endpoints
- ✅ Input validation (Zod schemas)
- ✅ SQL injection prevention (Prisma parameterized queries)
- ✅ XSS prevention (React auto-escaping + CSP headers)
- ✅ CSRF protection (cookie SameSite attribute)
- ✅ Dependency scanning (npm audit + Snyk)
- ✅ Secrets management (never commit to git)

---

## Production Readiness

**This stack is:**
- ✅ Battle-tested (848 tests passing)
- ✅ Production-deployed (Railway)
- ✅ Type-safe (end-to-end TypeScript)
- ✅ Performant (Tailwind v4 Oxide, Vite, React 19)
- ✅ Maintainable (Prisma migrations, clear structure)
- ✅ Scalable (React Router SSR, PostgreSQL, Railway)

---

**Questions about this stack?** See `.project-management/docs/STACK-FAQ.md`
