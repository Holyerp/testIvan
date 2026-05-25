# Completed Work — Pinoles

**Project:** Pinoles — Internal Business Portal
**Last Updated:** 2026-05-25

---

---

## Completed Stories

### US-001: Employee Login
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 5
**Commit:** See git log

**Summary:**
- POST /api/v1/auth/login — BCrypt verify (cost 12), JWT access token (8h) + rotating refresh token (7d, httpOnly cookie), rate limit 5 attempts per IP per 15 min
- POST /api/v1/auth/refresh — rotates refresh token; old token revoked
- POST /api/v1/auth/logout — revokes refresh token in DB, clears cookie
- LoginRateLimiter (singleton, in-memory, per-IP, 5 attempts / 15 min window)
- IAuthService + AuthService (Application.Auth), ITokenService + TokenService (Infrastructure.Auth), JwtOptions
- JwtSecurityToken via System.IdentityModel.Tokens.Jwt — HMAC SHA-256, issuer + audience validation
- Dev seed: admin / manager / accounting / warehouse test users (BCrypt cost 12) on startup
- Login page: Next.js 14 App Router `(auth)/login`, React Hook Form + Zod validation, Pinoles design tokens (pine-navy background, white card)
- i18n: sr.json + en.json updated with auth error codes (AUTH_REFRESH_TOKEN_MISSING, AUTH_INVALID_REFRESH_TOKEN)
- Backend tests: AuthServiceTests (7 tests), LoginRateLimiterTests (5 tests)
- Frontend tests: LoginPage (7 tests) — all pass

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

### T-002: BC Middleware Foundation
**Completed:** 2026-05-25
**Phase:** 1 — Foundation
**Story Points:** 8
**Commit:** 054bce2

**Summary:**
- IBcHttpClient interface + real BcHttpClient (Azure AD OAuth2 client credentials)
- MockBcHttpClient for dev (BC.UseMock=true) with realistic sample data (customers, invoices, vendors)
- BcAuthService: Azure.Identity ClientSecretCredential with in-process token caching
- BcQueryOptions: OData query builder ($filter, $select, $top, $skip, $count, $orderby, $expand)
- ICacheService interface + MemoryCacheService (IMemoryCache, Redis-ready)
- EF Core entities: User, RefreshToken, AuditLog with PinolesDbContext
- InitialCreate migration (manually crafted — dotnet ef not installed)
- 10 unit tests (MockBcHttpClient ×7, BcQueryOptions ×3, MemoryCacheService ×4 — see Tests/Infrastructure/)
