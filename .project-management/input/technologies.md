# Technology Stack — Pinoles

**Version:** 1.0
**Last Updated:** 2026-05-25
**Status:** Active

---

## Frontend

### Core Framework
- **Framework:** Next.js 14+ (App Router)
- **Language:** TypeScript 5.x
- **Package Manager:** pnpm
- **Build Tool:** Next.js built-in (Turbopack / Webpack)

### Styling
- **CSS Framework:** Tailwind CSS 3.x
- **Custom Variables:** Pinoles design-system palette (see Design System section)
- **Component Library:** Custom components aligned to prototype
- **Icons:** Tabler Icons
- **Fonts:** DM Sans (300/400/500/600/700), DM Mono (400/500) via Google Fonts / local

### State Management
- **Server State:** Next.js App Router fetch + React Query (TanStack Query)
- **Form State:** React Hook Form + Zod
- **Global State:** Zustand (for auth session, UI state)

### Data Fetching
- **HTTP Client:** Fetch API (native) with typed wrappers
- **API Client:** Auto-generated or hand-written TypeScript types from .NET API

### Additional Libraries
- **Date Handling:** date-fns
- **Charts:** Recharts (Phase 4)
- **Tables:** TanStack Table
- **Exports:** jsPDF + xlsx (Phase 4)
- **Notifications:** react-hot-toast

---

## Backend

### Middleware Web API (.NET 8)
- **Runtime:** .NET 8 (LTS)
- **Framework:** ASP.NET Core Web API
- **Language:** C# 12
- **Architecture:** Clean Architecture (Controllers → Services → BC Client)

### Business Central Integration
- **Integration:** Microsoft Dynamics 365 Business Central REST API (OData v4)
- **Authentication toward BC:** OAuth 2.0 Client Credentials (Azure AD service principal)
- **Client library:** HttpClient with typed BC response models
- **BC API base:** `https://api.businesscentral.dynamics.com/v2.0/{tenant}/api/v2.0/companies({id})/`

### Database (PostgreSQL)
- **Engine:** PostgreSQL 16.x
- **ORM:** Entity Framework Core 8.x (for .NET) with code-first migrations
- **Use cases:** User accounts, role assignments, session tokens, BC response cache, audit log

### Authentication (toward Frontend)
- **Strategy:** JWT Bearer tokens (issued by .NET API on login)
- **Password Hashing:** BCrypt.Net (cost factor 12)
- **Session management:** JWT (access token 8h) + refresh token pattern (7-day rotation)

### API Design
- **Architecture:** RESTful
- **Versioning:** URL path: `/api/v1/`
- **Validation:** FluentValidation / Data Annotations + Zod on frontend
- **Documentation:** Swagger / OpenAPI 3.0 (auto-generated from controllers)

### Email Service
- **Provider:** SMTP via .NET MailKit (for password reset, notifications)

---

## Infrastructure & DevOps

### Hosting
- **Platform:** Microsoft Azure
  - Frontend: Azure Static Web Apps or Azure App Service (Node.js)
  - Backend .NET API: Azure App Service (Linux, .NET 8)
  - Database: Azure Database for PostgreSQL (Flexible Server)
  - Cache: Azure Cache for Redis (optional, Phase 2+)

### CI/CD
- **Pipeline:** GitHub Actions
- **On PR:** Run tests, linting, type-check
- **On merge to main:** Build + deploy to Azure

### Monitoring & Logging
- **Error Tracking:** Application Insights (Azure) or Sentry
- **Logging:** Structured logging with Serilog (.NET) + Next.js logging
- **Uptime:** Azure Monitor

### Environment Variables
- **Management:** Azure App Service App Settings (production) / .env.local (development)
- **Validation:** Zod env schema (frontend) + IOptions pattern (backend)

---

## Testing

### Frontend Testing
- **Unit / Component Tests:** Vitest + React Testing Library
- **E2E Tests:** Playwright
- **Coverage Tool:** c8 / Istanbul

### Backend Testing (.NET)
- **Unit Tests:** xUnit + Moq
- **Integration Tests:** WebApplicationFactory + Testcontainers (PostgreSQL)
- **Coverage Target:** 80%+

---

## Code Quality

### Linting & Formatting
- **Frontend:** ESLint + Prettier (TypeScript rules)
- **Backend (.NET):** EditorConfig + StyleCop / Roslyn analyzers
- **Git Hooks:** Husky + lint-staged (frontend)

### Type Safety
- **Frontend:** TypeScript strict mode
- **Backend:** Nullable reference types enabled (C# `<Nullable>enable</Nullable>`)

---

## Design System

### Color Palette (from Pinoles prototype)
```css
--pine-navy:        #1B3F6B   /* sidebar, primary color */
--pine-navy-mid:    #254d80
--pine-green:       #6ab04c   /* accent, active elements */
--pine-green-light: #7cc95e
--pine-green-pale:  #e8f5e3   /* badge backgrounds */
--pine-green-dark:  #4a8a32
```

### Layout
- Sidebar (200px, dark navy) + Main area (topbar + content)
- Responsive for desktop (min 1024px)

---

## Project Structure (Monorepo — pnpm workspace)

```
pinoles/
├── apps/
│   ├── web/                  # Next.js 14 App Router
│   │   ├── app/              # Routes (App Router)
│   │   ├── components/       # UI components
│   │   ├── lib/              # API clients, utilities
│   │   ├── public/
│   │   └── package.json
│   │
│   ├── api/                  # .NET 8 Web API (middleware)
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Models/
│   │   ├── Infrastructure/   # EF Core, BC client
│   │   └── Pinoles.Api.csproj
│   │
│   └── mobile/               # React Native (future phase)
│
├── packages/
│   ├── types/                # Shared TypeScript types
│   └── ui/                   # Shared UI components (future)
│
├── pnpm-workspace.yaml
└── turbo.json
```

---

## Performance Targets

| Metric | Target |
|--------|--------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3.5s |
| BC API response (p95) | < 2s (includes network + BC processing) |
| Internal API response (p95) | < 500ms |
| Database Query (p95) | < 100ms |

---

## Browser Support

- Chrome (last 2 versions)
- Firefox (last 2 versions)
- Safari (last 2 versions)
- Edge (last 2 versions)
- Desktop-first (min 1024px width)

---

**Last Updated:** 2026-05-25
