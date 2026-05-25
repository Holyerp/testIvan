# Completed Work — Pinoles

**Project:** Pinoles — Internal Business Portal
**Last Updated:** 2026-05-25

---

---

## Completed Stories

*(none yet)*

---

## Completed Technical Tasks

### T-001: Project Infrastructure Setup
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 5
**Commit:** See git log

**Summary:**
- pnpm workspace + Turborepo monorepo root config
- `apps/web`: Next.js 14 App Router skeleton (TypeScript, Tailwind CSS with Pinoles palette, ESLint, Prettier, Vitest, next-intl)
- `apps/api`: .NET 8 Web API skeleton (Clean Architecture folders, Swagger, Serilog, EF Core packages, xUnit test project)
- `packages/types`: Shared TypeScript types (UserRole, User, AuthTokens, PaginatedResponse, API envelope types)
- GitHub Actions CI workflow (web + api jobs)
- i18n: `sr.json` + `en.json` base message files
- Base API client with typed error handling (ApiError class)
- 3 unit tests on API client (success, error, fallback code)
