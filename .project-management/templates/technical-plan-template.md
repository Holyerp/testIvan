# Technical Plan - {{PROJECT_NAME}}

**Version:** 1.0
**Date:** {{DATE}}
**Status:** Living Document
**Author:** {{AUTHOR}}

> **Note:** This is a LIVING document. Update it as the project evolves. All technical decisions, architecture changes, and implementation details should be documented here.

---

## 1. Product Overview

### Product Vision
{{PRODUCT_VISION}}

### Product Rules (The North Star)
**Every feature must answer YES to:**
1. Does it solve the core problem?
2. Does it deliver measurable value?
3. Is it aligned with project goals?

**Example:** "Does it reduce user friction? Does it speed up the workflow?"

### Target Users
{{TARGET_USERS}}

### Core Value Proposition
{{VALUE_PROPOSITION}}

---

## 2. Complete Tech Stack

### Frontend

| Package | Version | Purpose | Documentation |
|---------|---------|---------|---------------|
| {{FRAMEWORK}} | {{VERSION}} | UI framework | {{DOCS_LINK}} |
| {{ROUTER}} | {{VERSION}} | Client-side routing | {{DOCS_LINK}} |
| {{STATE_MGMT}} | {{VERSION}} | State management | {{DOCS_LINK}} |
| {{UI_LIBRARY}} | {{VERSION}} | Component library | {{DOCS_LINK}} |
| {{FORM_LIBRARY}} | {{VERSION}} | Form handling | {{DOCS_LINK}} |
| {{VALIDATION}} | {{VERSION}} | Schema validation | {{DOCS_LINK}} |

**Example:**
| Package | Version | Purpose | Documentation |
|---------|---------|---------|---------------|
| React | 19.0.0 | UI framework | https://react.dev |
| React Router | 7.13.0 | SSR framework mode | https://reactrouter.com |
| Zustand | 5.0.0 | Global state | https://zustand.docs.dev |
| shadcn/ui | latest | UI components | https://ui.shadcn.com |
| react-hook-form | 7.54.0 | Form state | https://react-hook-form.com |
| zod | 3.24.0 | Validation (shared client/server) | https://zod.dev |

### Backend

| Package | Version | Purpose | Documentation |
|---------|---------|---------|---------------|
| {{RUNTIME}} | {{VERSION}} | Server runtime | {{DOCS_LINK}} |
| {{FRAMEWORK}} | {{VERSION}} | Backend framework | {{DOCS_LINK}} |
| {{DATABASE}} | {{VERSION}} | Primary database | {{DOCS_LINK}} |
| {{ORM}} | {{VERSION}} | Database ORM | {{DOCS_LINK}} |
| {{AUTH}} | {{VERSION}} | Authentication | {{DOCS_LINK}} |

### Testing

| Package | Version | Purpose | Documentation |
|---------|---------|---------|---------------|
| {{TEST_RUNNER}} | {{VERSION}} | Test runner | {{DOCS_LINK}} |
| {{TEST_LIBRARY}} | {{VERSION}} | Component testing | {{DOCS_LINK}} |
| {{E2E_TOOL}} | {{VERSION}} | E2E testing | {{DOCS_LINK}} |
| {{MSW}} | {{VERSION}} | API mocking | {{DOCS_LINK}} |

### DevOps & Tooling

| Package | Version | Purpose | Documentation |
|---------|---------|---------|---------------|
| {{LINTER}} | {{VERSION}} | Code linting | {{DOCS_LINK}} |
| {{FORMATTER}} | {{VERSION}} | Code formatting | {{DOCS_LINK}} |
| {{BUILD_TOOL}} | {{VERSION}} | Build tool | {{DOCS_LINK}} |
| {{CI_CD}} | N/A | Deployment | {{DOCS_LINK}} |

---

## 3. Architecture

### 3.1 High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         CLIENT                               │
│  ┌───────────────────────────────────────────────────────┐  │
│  │             React Application (SPA/SSR)               │  │
│  │                                                       │  │
│  │  - Components (UI)                                    │  │
│  │  - State Management                                   │  │
│  │  - Client-side Routing                                │  │
│  │  - API Client                                         │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          │
                          │ HTTPS / REST
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                    APPLICATION SERVER                        │
│  ┌───────────────────────────────────────────────────────┐  │
│  │              Backend API (Node.js/etc)                │  │
│  │                                                       │  │
│  │  - API Routes                                         │  │
│  │  - Business Logic (Services)                          │  │
│  │  - Authentication & Authorization                     │  │
│  │  - Data Validation                                    │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          │
                          │ SQL/ORM
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                      DATABASE LAYER                          │
│  ┌───────────────────────────────────────────────────────┐  │
│  │         PostgreSQL / MongoDB / MySQL                  │  │
│  │                                                       │  │
│  │  - User Data                                          │  │
│  │  - Business Data                                      │  │
│  │  - Session Storage (if applicable)                    │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                          │
                          │ API Calls
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                   EXTERNAL SERVICES                          │
│                                                              │
│  - Payment Gateway (Stripe, PayPal)                          │
│  - Email Service (SendGrid, AWS SES)                         │
│  - File Storage (AWS S3, Cloudinary)                         │
│  - Authentication Provider (Auth0, OAuth)                    │
│  - Analytics (Google Analytics, Mixpanel)                    │
└─────────────────────────────────────────────────────────────┘
```

### 3.2 Architecture Decisions

| Decision | Choice | Rationale | Alternatives Considered |
|----------|--------|-----------|------------------------|
| **Frontend Framework** | {{FRAMEWORK}} | {{RATIONALE}} | {{ALTERNATIVES}} |
| **State Management** | {{STATE_MGMT}} | {{RATIONALE}} | {{ALTERNATIVES}} |
| **Database** | {{DATABASE}} | {{RATIONALE}} | {{ALTERNATIVES}} |
| **Authentication** | {{AUTH_STRATEGY}} | {{RATIONALE}} | {{ALTERNATIVES}} |
| **Hosting** | {{HOSTING}} | {{RATIONALE}} | {{ALTERNATIVES}} |

**Example:**
| Decision | Choice | Rationale | Alternatives Considered |
|----------|--------|-----------|------------------------|
| Frontend Framework | React 19 | Latest features, excellent TypeScript support, large ecosystem | Vue 3, Svelte, Angular |
| State Management | Zustand | Simple API, minimal boilerplate, good TypeScript | Redux Toolkit, Jotai, Context API |
| Database | PostgreSQL | ACID compliance, strong relational model, JSON support | MongoDB, MySQL, SQLite |
| Authentication | JWT + Cookie | Stateless, scalable, works with SSR | Session-based, OAuth only |
| Hosting | Railway | Auto-deployment, managed PostgreSQL, simple setup | Vercel, AWS, DigitalOcean |

### 3.3 Request Flow

```
User Action
    │
    ▼
┌──────────────────┐
│   UI Component   │
│   (React)        │
└──────────────────┘
    │
    │ onClick / onSubmit
    ▼
┌──────────────────┐
│   Form Handler   │
│   (react-hook-   │
│    form + zod)   │
└──────────────────┘
    │
    │ Client Validation
    ▼
┌──────────────────┐
│   API Client     │
│   (fetch/axios)  │
└──────────────────┘
    │
    │ HTTPS Request
    ▼
┌──────────────────┐
│   API Endpoint   │
│   (Route)        │
└──────────────────┘
    │
    │ Server Validation
    ▼
┌──────────────────┐
│   Middleware     │
│   (Auth, etc)    │
└──────────────────┘
    │
    │ Authentication OK
    ▼
┌──────────────────┐
│   Controller     │
│   (Handler)      │
└──────────────────┘
    │
    │ Call Service
    ▼
┌──────────────────┐
│   Service        │
│   (Business      │
│    Logic)        │
└──────────────────┘
    │
    │ Database Query
    ▼
┌──────────────────┐
│   Database       │
│   (PostgreSQL)   │
└──────────────────┘
    │
    │ Return Data
    ▼
┌──────────────────┐
│   Response       │
│   (JSON)         │
└──────────────────┘
    │
    │ HTTPS Response
    ▼
┌──────────────────┐
│   UI Update      │
│   (React)        │
└──────────────────┘
```

---

## 4. Database Schema

### 4.1 Entity Relationship Diagram

```
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│    User     │       │   Product   │       │    Order    │
├─────────────┤       ├─────────────┤       ├─────────────┤
│ id          │───┐   │ id          │───┐   │ id          │
│ email       │   │   │ name        │   │   │ userId      │──┐
│ passwordHash│   │   │ price       │   │   │ totalAmount │  │
│ role        │   │   │ userId      │──┘│   │ status      │  │
│ createdAt   │   │   │ createdAt   │   │   │ createdAt   │  │
└─────────────┘   │   └─────────────┘   │   └─────────────┘  │
                  │                     │            │        │
                  │                     └────────────┼────────┘
                  │                                  │
                  └──────────────────────────────────┘

```

### 4.2 Prisma Schema (Example)

```prisma
model User {
  id           String    @id @default(uuid())
  email        String    @unique
  passwordHash String
  role         UserRole  @default(USER)
  profile      Profile?
  products     Product[]
  orders       Order[]
  createdAt    DateTime  @default(now())
  updatedAt    DateTime  @updatedAt

  @@index([email])
  @@map("users")
}

enum UserRole {
  USER
  VENDOR
  ADMIN
}

model Profile {
  id        String   @id @default(uuid())
  userId    String   @unique
  user      User     @relation(fields: [userId], references: [id], onDelete: Cascade)
  firstName String?
  lastName  String?
  avatar    String?
  bio       String?
  createdAt DateTime @default(now())
  updatedAt DateTime @updatedAt

  @@map("profiles")
}

model Product {
  id          String   @id @default(uuid())
  name        String
  description String?
  price       Decimal  @db.Decimal(10, 2)
  userId      String
  user        User     @relation(fields: [userId], references: [id], onDelete: Cascade)
  createdAt   DateTime @default(now())
  updatedAt   DateTime @updatedAt

  @@index([userId])
  @@map("products")
}

model Order {
  id          String      @id @default(uuid())
  userId      String
  user        User        @relation(fields: [userId], references: [id], onDelete: Cascade)
  totalAmount Decimal     @db.Decimal(10, 2)
  status      OrderStatus @default(PENDING)
  createdAt   DateTime    @default(now())
  updatedAt   DateTime    @updatedAt

  @@index([userId])
  @@map("orders")
}

enum OrderStatus {
  PENDING
  PROCESSING
  SHIPPED
  DELIVERED
  CANCELLED
}
```

### 4.3 Database Indexes

| Table | Index | Fields | Purpose |
|-------|-------|--------|---------|
| users | email_idx | email | Fast user lookup by email |
| products | user_id_idx | userId | Query all products by user |
| orders | user_id_idx | userId | Query all orders by user |
| orders | status_idx | status | Filter orders by status |

---

## 5. Authentication & Sessions

### Authentication Strategy
{{AUTH_STRATEGY}}

**Example:**
- **Strategy:** JWT tokens stored in HTTP-only cookies
- **Session Duration:** 7 days (refresh token), 15 minutes (access token)
- **Password Hashing:** bcrypt with salt rounds 10
- **Token Storage:** HTTP-only, Secure, SameSite=Strict cookies

### Auth Flow

```
1. User submits credentials (email + password)
2. Server validates credentials against database
3. Server generates JWT access token (15min) + refresh token (7 days)
4. Server sets HTTP-only cookies with tokens
5. Client includes cookie automatically in requests
6. Server middleware validates token on each request
7. Grant/deny access based on role and permissions
```

### Protected Routes

| Route | Required Role | Description |
|-------|---------------|-------------|
| `/app/*` | USER | Authenticated users only |
| `/admin/*` | ADMIN | Admin users only |
| `/vendor/*` | VENDOR | Vendor users only |
| `/api/*` | Varies | API endpoints (check per route) |

---

## 6. Internationalization (i18n)

> **Note:** Include this section only if your project requires multi-language support.

### Supported Languages
- {{LANG_1}} ({{CODE_1}}) - Primary
- {{LANG_2}} ({{CODE_2}}) - Secondary
- {{LANG_3}} ({{CODE_3}}) - Additional

**Example:**
- English (en) - Primary
- German (de) - Secondary
- Spanish (es) - Additional

### i18n Stack
{{I18N_STACK}}

**Example:**
- i18next 24.2.0
- react-i18next 15.4.0
- Translation files in `/locales/[lang]/[namespace].json`

### Translation Workflow
1. Add keys to `locales/en/*.json` (primary language)
2. Add translations to other language files
3. Use `useTranslation()` hook in components
4. Test language switching

---

## 7. Project Structure

```
project-root/
├── app/                          # Application code
│   ├── components/              # React components
│   │   ├── ui/                  # shadcn/ui components
│   │   ├── layout/              # Layout components
│   │   ├── features/            # Feature-specific components
│   │   └── common/              # Shared components
│   ├── lib/                     # Shared libraries
│   │   ├── services/            # Business logic (*.server.ts)
│   │   ├── utils/               # Helper functions
│   │   ├── validation/          # Zod schemas
│   │   ├── auth.server.ts       # Authentication helpers
│   │   └── db.server.ts         # Database client
│   ├── routes/                  # Route handlers
│   │   ├── _auth/               # Auth routes (login, register)
│   │   ├── _app/                # Protected app routes
│   │   └── api/                 # API endpoints
│   ├── types/                   # TypeScript types
│   ├── root.tsx                 # Root component
│   └── app.css                  # Global styles
├── prisma/                      # Database
│   ├── schema.prisma            # Prisma schema
│   ├── migrations/              # Migration files
│   └── seed.ts                  # Seed data
├── public/                      # Static assets
├── __tests__/                   # Tests
│   ├── unit/                    # Unit tests
│   ├── integration/             # Integration tests
│   └── e2e/                     # E2E tests
├── docs/                        # Documentation
├── .env.example                 # Environment variables template
├── package.json                 # Dependencies
├── tsconfig.json                # TypeScript config
└── vite.config.ts               # Vite config
```

### File Naming Conventions
- **Components:** `PascalCase.tsx` (e.g., `UserProfile.tsx`)
- **Services:** `kebab-case.server.ts` (e.g., `user.server.ts`)
- **Utils:** `kebab-case.ts` (e.g., `format-date.ts`)
- **Routes:** `kebab-case.tsx` (e.g., `user-profile.tsx`)
- **Types:** `kebab-case.types.ts` (e.g., `user.types.ts`)
- **Tests:** `*.test.ts` or `*.test.tsx`

### Server-Only Code
**Pattern:** Use `.server.ts` suffix for server-only code that should never be bundled for client.

**Example:**
- `auth.server.ts` - Authentication logic
- `db.server.ts` - Database client
- `email.server.ts` - Email sending
- `user.server.ts` - User service

---

## 8. Business Logic & Domain Rules

### Core Business Rules

{{BUSINESS_RULES}}

**Example:**
1. **User Registration:**
   - Users must be 18+ years old
   - Email verification required within 24 hours
   - Password must meet complexity requirements

2. **Product Management:**
   - Product prices must be positive numbers
   - Maximum 5 images per product
   - Draft products not visible to customers

3. **Order Processing:**
   - Order total must match sum of line items + tax
   - Payment must complete before order confirmation
   - Orders cannot be modified after payment

### Validation Rules

**Client-side (Zod):**
```typescript
const userSchema = z.object({
  email: z.string().email(),
  password: z.string().min(8),
  age: z.number().min(18),
});
```

**Server-side (same schema):**
```typescript
export async function registerUser(data: unknown) {
  const validated = userSchema.parse(data); // Throws if invalid
  // ...
}
```

### Domain Constants

```typescript
export const CONSTANTS = {
  MIN_AGE: 18,
  MAX_PRODUCT_IMAGES: 5,
  MAX_CART_ITEMS: 50,
  SESSION_DURATION_DAYS: 7,
  PASSWORD_MIN_LENGTH: 8,
} as const;
```

---

## 9. API Design & Conventions

### RESTful Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| **POST** | `/api/auth/register` | Register new user | No |
| **POST** | `/api/auth/login` | Login user | No |
| **POST** | `/api/auth/logout` | Logout user | Yes |
| **GET** | `/api/users/me` | Get current user | Yes |
| **PUT** | `/api/users/me` | Update current user | Yes |
| **GET** | `/api/products` | List products | No |
| **POST** | `/api/products` | Create product | Yes (VENDOR) |
| **GET** | `/api/products/:id` | Get product | No |
| **PUT** | `/api/products/:id` | Update product | Yes (OWNER) |
| **DELETE** | `/api/products/:id` | Delete product | Yes (OWNER) |

### Request/Response Format

**Standard Success Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "name": "Product Name",
    "price": 99.99
  }
}
```

**Standard Error Response:**
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid email format",
    "details": {
      "field": "email",
      "value": "invalid-email"
    }
  }
}
```

### Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| `VALIDATION_ERROR` | 400 | Request validation failed |
| `UNAUTHORIZED` | 401 | Authentication required |
| `FORBIDDEN` | 403 | Insufficient permissions |
| `NOT_FOUND` | 404 | Resource not found |
| `CONFLICT` | 409 | Resource already exists |
| `INTERNAL_ERROR` | 500 | Server error |

---

## 10. Security

### Security Measures

**Authentication:**
- ✅ bcrypt password hashing (salt rounds: 10)
- ✅ HTTP-only, Secure cookies
- ✅ SameSite=Strict for CSRF protection
- ✅ Token expiration (access: 15min, refresh: 7 days)

**Input Validation:**
- ✅ Zod schema validation on client and server
- ✅ SQL injection prevention (Prisma parameterized queries)
- ✅ XSS prevention (React auto-escapes, DOMPurify for rich text)

**Authorization:**
- ✅ Role-based access control (RBAC)
- ✅ Resource ownership checks
- ✅ Protected routes with middleware

**Data Protection:**
- ✅ HTTPS only (enforced)
- ✅ Environment variables for secrets
- ✅ No secrets in client bundle
- ✅ Rate limiting on API endpoints

**OWASP Top 10 Compliance:**
- ✅ A01: Broken Access Control - RBAC implemented
- ✅ A02: Cryptographic Failures - bcrypt, HTTPS
- ✅ A03: Injection - Parameterized queries
- ✅ A04: Insecure Design - Security by design
- ✅ A05: Security Misconfiguration - Secure defaults
- ✅ A06: Vulnerable Components - npm audit
- ✅ A07: Authentication Failures - Strong auth
- ✅ A08: Data Integrity - Validation
- ✅ A09: Logging Failures - Error logging
- ✅ A10: SSRF - Input validation

---

## 11. Deployment

### Hosting Platform
{{HOSTING_PLATFORM}}

**Example:** Railway (auto-deploy from `main` branch)

### Environment Variables

| Variable | Description | Required | Example |
|----------|-------------|----------|---------|
| `DATABASE_URL` | PostgreSQL connection string | Yes | `postgresql://user:pass@host:5432/db` |
| `SESSION_SECRET` | Secret for session encryption | Yes | `random-32-char-string` |
| `NODE_ENV` | Environment (development/production) | Yes | `production` |
| `PORT` | Server port | No | `3000` |
| `STRIPE_SECRET_KEY` | Stripe API key | Yes | `sk_live_...` |
| `SENDGRID_API_KEY` | SendGrid API key | Yes | `SG.xxx` |

### Deployment Process

```
1. Push to main branch
2. CI/CD triggers (GitHub Actions / Railway)
3. Run tests
4. Run linter
5. Build application
6. Run database migrations (prisma migrate deploy)
7. Deploy to production
8. Health check
```

### Health Check Endpoint

```typescript
// GET /api/health
{
  "status": "ok",
  "timestamp": "2026-03-25T10:00:00Z",
  "database": "connected",
  "version": "1.0.0"
}
```

---

## 12. Design System & Branding

### Color Palette

| Color | Hex | Usage |
|-------|-----|-------|
| Primary | `#3B82F6` | Buttons, links, primary actions |
| Secondary | `#8B5CF6` | Secondary actions, highlights |
| Success | `#10B981` | Success messages, confirmations |
| Warning | `#F59E0B` | Warnings, caution messages |
| Error | `#EF4444` | Error messages, destructive actions |
| Background | `#FFFFFF` | Page background |
| Foreground | `#1F2937` | Text color |

### Typography

| Element | Font | Size | Weight |
|---------|------|------|--------|
| Heading 1 | Inter | 2.25rem (36px) | 700 |
| Heading 2 | Inter | 1.875rem (30px) | 700 |
| Heading 3 | Inter | 1.5rem (24px) | 600 |
| Body | Inter | 1rem (16px) | 400 |
| Small | Inter | 0.875rem (14px) | 400 |

### Component Library
{{COMPONENT_LIBRARY}}

**Example:** shadcn/ui + Radix UI

---

## 13. Performance Targets

| Metric | Target | Measurement |
|--------|--------|-------------|
| First Contentful Paint (FCP) | < 1.5s | Lighthouse |
| Largest Contentful Paint (LCP) | < 2.5s | Lighthouse |
| Time to Interactive (TTI) | < 3.5s | Lighthouse |
| Cumulative Layout Shift (CLS) | < 0.1 | Lighthouse |
| API Response Time (p95) | < 500ms | Server logs |
| Database Query Time (p95) | < 100ms | Database logs |
| Lighthouse Score | > 90 | Lighthouse CI |

### Optimization Strategies
- ✅ Code splitting and lazy loading
- ✅ Image optimization (WebP, responsive images)
- ✅ CDN for static assets
- ✅ Database query optimization (indexes)
- ✅ Caching (Redis, in-memory)
- ✅ Compression (gzip, brotli)

---

## 14. Monitoring & Observability

### Monitoring Stack
{{MONITORING_STACK}}

**Example:**
- **APM:** Sentry (error tracking)
- **Logging:** Winston (structured logs)
- **Analytics:** Google Analytics / Mixpanel
- **Uptime:** UptimeRobot

### Key Metrics to Track
- Error rate (errors per minute)
- Response time (p50, p95, p99)
- Request rate (requests per minute)
- Database connection pool usage
- Memory usage
- CPU usage
- Active users

### Alerting Rules
| Condition | Alert | Action |
|-----------|-------|--------|
| Error rate > 1% | Critical | Page on-call engineer |
| API p95 > 1s | Warning | Investigate performance |
| Database connections > 80% | Warning | Scale database |
| Disk usage > 85% | Warning | Clean up or scale storage |

---

## 15. Changelog

> Keep this section updated with all major changes to the technical plan.

| Date | Version | Author | Changes |
|------|---------|--------|---------|
| 2026-03-25 | 1.0 | {{AUTHOR}} | Initial technical plan created |
| {{DATE}} | 1.1 | {{AUTHOR}} | {{CHANGES}} |

**Format:**
```
### YYYY-MM-DD - v1.1
**Changed:**
- Updated database schema to include `avatar` field in User model
- Migrated from Context API to Zustand for state management

**Added:**
- Redis caching for product listings
- Webhook endpoint for Stripe payment events

**Fixed:**
- Race condition in order processing
- Memory leak in WebSocket connection

**Deprecated:**
- Old authentication flow (will be removed in v2.0)
```

---

## 16. Future Considerations

### Planned Improvements
{{FUTURE_IMPROVEMENTS}}

**Example:**
- Mobile native app (React Native)
- Real-time notifications (WebSockets)
- Advanced analytics dashboard
- Multi-tenant architecture
- Microservices migration (if scale requires)

### Technical Debt
{{TECHNICAL_DEBT}}

**Example:**
- TD-001: Refactor route handlers to use proper return types
- TD-002: Add request/response logging middleware
- TD-003: Implement proper error boundary in React app

---

## 17. References

### Documentation
- [Project README](../../README.md)
- [API Documentation](../../docs/api/README.md)
- [User Guide](../../docs/user-guide/README.md)
- [Deployment Guide](../../docs/deployment/README.md)

### External Resources
- {{FRAMEWORK}} Docs: {{LINK}}
- {{DATABASE}} Docs: {{LINK}}
- {{HOSTING}} Docs: {{LINK}}

---

**Document Owner:** {{OWNER}}
**Last Updated:** {{DATE}}
**Status:** Living Document (update as project evolves)

---

## How to Use This Document

1. **Fill in all `{{PLACEHOLDERS}}`** with your project details
2. **Update regularly** as architecture evolves
3. **Reference during development** for technical decisions
4. **Share with team** for alignment
5. **Keep changelog updated** with all major changes
6. **Review quarterly** to ensure it stays current

This is a **living document** - it should always reflect the current state of the technical implementation.
