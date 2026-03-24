# HolyEstate — Technical Plan

> **Source of truth** for architecture, schema, tech decisions, and conventions.
> Updated: 2026-03-22

---

## Table of Contents

1. [Product Overview](#1-product-overview)
2. [Tech Stack](#2-tech-stack)
3. [Architecture](#3-architecture)
4. [Database Schema](#4-database-schema)
5. [Authentication & Sessions](#5-authentication--sessions)
6. [Internationalization (i18n)](#6-internationalization-i18n)
7. [Project Structure](#7-project-structure)
8. [Analysis Engine (Deterministic)](#8-analysis-engine-deterministic)
9. [Swiss Real Estate Reference Data](#9-swiss-real-estate-reference-data)
10. [API Design & Conventions](#10-api-design--conventions)
11. [Security](#11-security)
12. [Deployment](#12-deployment)
13. [Environment Variables](#13-environment-variables)
14. [Design System & Branding](#14-design-system--branding)
15. [Changelog](#15-changelog)

---

## 1. Product Overview

**HolyEstate** is an AI investment decision engine for Swiss real estate that helps professional investors **reduce risk** and **buy faster**.

**Product rule:** Every feature must answer:

1. Does it reduce risk?
2. Does it speed up decision-making?

If not — don't build it.

**Target users:** Small real-estate companies / professional investors (acquisition teams).

**Phase 1 scope:** AI Deal Analyzer (Foundation MVP) — single-deal evaluation with financing feasibility, investment analysis, risk analysis, and a transparent investment score. **No AI/LLM in Phase 1** — all calculations are deterministic. AI integration begins in Phase 2+.

---

## 2. Tech Stack

### Core Framework

| Package             | Version | Purpose                                        |
| ------------------- | ------- | ---------------------------------------------- |
| react               | ^19.0.0 | UI library                                     |
| react-dom           | ^19.0.0 | DOM rendering                                  |
| react-router        | ^7.13.0 | Full-stack framework (framework mode with SSR) |
| @react-router/dev   | ^7.13.0 | React Router dev tooling / Vite plugin         |
| @react-router/node  | ^7.13.0 | Node.js adapter for SSR                        |
| @react-router/serve | ^7.13.0 | Production server                              |
| typescript          | ^5.7.0  | Type system                                    |
| vite                | ^6.1.0  | Build tool                                     |

### Database & ORM

| Package        | Version | Purpose                |
| -------------- | ------- | ---------------------- |
| prisma         | ^6.19.0 | CLI and migration tool |
| @prisma/client | ^6.19.0 | Database client        |
| PostgreSQL     | 16.x    | Database engine        |

### Authentication & Security

| Package                             | Version        | Purpose            |
| ----------------------------------- | -------------- | ------------------ |
| bcryptjs                            | ^2.4.3         | Password hashing   |
| Built-in createCookieSessionStorage | (react-router) | Session management |

Cookie-based sessions (no JWT).

### Forms & Validation

| Package             | Version | Purpose                                  |
| ------------------- | ------- | ---------------------------------------- |
| react-hook-form     | ^7.54.0 | Form state management                    |
| @hookform/resolvers | ^3.9.0  | Zod resolver                             |
| zod                 | ^3.24.0 | Schema validation (shared client/server) |

### Internationalization

| Package                          | Version | Purpose                                |
| -------------------------------- | ------- | -------------------------------------- |
| i18next                          | ^24.2.0 | Core i18n framework                    |
| react-i18next                    | ^15.4.0 | React bindings                         |
| remix-i18next                    | ^7.4.0  | SSR integration for RR7 framework mode |
| i18next-fs-backend               | ^2.6.0  | Server-side translation loading        |
| i18next-http-backend             | ^3.0.0  | Client-side translation loading        |
| i18next-browser-languagedetector | ^8.0.0  | Auto-detect user language              |

### UI & Styling

| Package                  | Version  | Purpose                             |
| ------------------------ | -------- | ----------------------------------- |
| tailwindcss              | ^4.1.0   | Utility-first CSS (v4 Oxide engine) |
| @tailwindcss/vite        | ^4.1.0   | Vite plugin                         |
| shadcn/ui                | latest   | Component library                   |
| radix-ui                 | latest   | Primitives (shadcn dependency)      |
| lucide-react             | ^0.475.0 | Icons                               |
| clsx                     | ^2.1.0   | Conditional classes                 |
| tailwind-merge           | ^3.0.0   | Class conflict resolution           |
| class-variance-authority | ^0.7.0   | Component variants                  |

### Data Tables & Charts

| Package               | Version | Purpose             |
| --------------------- | ------- | ------------------- |
| @tanstack/react-table | ^8.21.0 | Headless data table |
| recharts              | ^2.15.0 | Charting            |

### PDF Generation

| Package             | Version | Purpose                   |
| ------------------- | ------- | ------------------------- |
| @react-pdf/renderer | ^4.3.0  | PDF from React components |

### Email

| Package                 | Version | Purpose                             |
| ----------------------- | ------- | ----------------------------------- |
| resend                  | ^4.1.0  | Email delivery (production)         |
| react-email             | ^3.0.0  | Email templates as React components |
| @react-email/components | ^0.0.31 | Email primitives                    |

Dev: console fallback. Prod: Resend API.

### Testing

| Package                     | Version | Purpose                      |
| --------------------------- | ------- | ---------------------------- |
| vitest                      | ^4.0.0  | Unit/integration test runner |
| @testing-library/react      | ^16.0.0 | Component testing            |
| @testing-library/jest-dom   | ^6.6.0  | DOM matchers                 |
| @testing-library/user-event | ^14.6.0 | User interaction simulation  |
| msw                         | ^2.7.0  | API mocking                  |
| @playwright/test            | ^1.58.0 | E2E testing                  |
| @vitest/coverage-v8         | ^4.0.0  | Code coverage                |

### Dev Tooling

| Package                     | Version | Purpose                  |
| --------------------------- | ------- | ------------------------ |
| eslint                      | ^9.0.0  | Linting (flat config)    |
| prettier                    | ^3.4.0  | Formatting               |
| prettier-plugin-tailwindcss | ^0.6.0  | Tailwind class sorting   |
| husky                       | ^9.1.0  | Git hooks                |
| lint-staged                 | ^15.3.0 | Pre-commit lint runner   |
| tsx                         | ^4.19.0 | TS execution for scripts |

---

## 3. Architecture

### High-Level Diagram

```
┌──────────────────────────────────────────────────────┐
│                   Client (Browser)                    │
│  React Router v7 — SSR + Client-side Navigation       │
│  ┌────────────┐ ┌────────────┐ ┌──────────────────┐  │
│  │  Auth       │ │  Profile   │ │  Deal Analyzer   │  │
│  │  Pages      │ │  Setup     │ │  (Core Product)  │  │
│  └────────────┘ └────────────┘ └──────────────────┘  │
├──────────────────────────────────────────────────────┤
│           Server (React Router Loaders/Actions)       │
│  ┌────────────┐ ┌────────────┐ ┌──────────────────┐  │
│  │  Auth       │ │  Profile   │ │  Analysis        │  │
│  │  Service    │ │  Service   │ │  Engine Service  │  │
│  └────────────┘ └────────────┘ └──────────────────┘  │
│                       │                               │
│  ┌──────────────────────────────────────────────────┐ │
│  │       Deterministic Analysis Pipeline            │ │
│  │  Financing → Investment → Risk → Score           │ │
│  └──────────────────────────────────────────────────┘ │
├──────────────────────────────────────────────────────┤
│  PostgreSQL (Prisma)    │  Swiss Reference Data       │
│  - Users                │  - BFS (vacancy, prices)    │
│  - InvestorProfiles     │  - SNB (mortgage rates)     │
│  - Properties           │  - Cantonal benchmarks      │
│  - Analyses             │  (seeded/cached in DB)      │
└──────────────────────────────────────────────────────┘
```

### Key Architectural Decisions

| Decision             | Choice                                                        | Rationale                                                   |
| -------------------- | ------------------------------------------------------------- | ----------------------------------------------------------- |
| Rendering            | SSR with React Router v7 framework mode                       | SEO for landing, fast first paint, server-side data loading |
| Auth                 | Cookie-based sessions (bcryptjs + createCookieSessionStorage) | Simple, secure, no token management                         |
| Data access          | Prisma ORM                                                    | Type-safe DB access, migration system                       |
| Analysis engine      | Deterministic calculations (no AI in Phase 1)                 | Predictable, testable, no API costs                         |
| State management     | Server state via loaders/actions, minimal client state        | Leverages React Router's data model                         |
| Forms                | react-hook-form + Zod                                         | Progressive enhancement, shared validation                  |
| i18n                 | react-i18next + remix-i18next                                 | SSR-compatible, proven ecosystem                            |
| Styling              | Tailwind v4 + shadcn/ui                                       | Rapid development, consistent design                        |
| Swiss reference data | Seeded in DB from public sources (BFS, SNB)                   | Offline-capable, fast queries, updatable                    |

### Request Flow

1. Browser requests route → React Router SSR
2. `loader` runs server-side → authenticates via cookie session
3. Service layer fetches data via Prisma (user-scoped)
4. Response rendered server-side → hydrated client-side
5. Form submissions → `action` handlers → service layer → redirect

### Analysis Pipeline (Phase 1)

All calculations are **deterministic** — no LLM/AI calls.

```
Input: Property + InvestorProfile + SwissReferenceData
  │
  ├─→ Financing Feasibility Calculator
  │     - Equity check (20% rule)
  │     - Affordability (stress-rate 5%, 33% threshold)
  │     - Mortgage range estimation
  │     - Feasibility score (0-100)
  │
  ├─→ Investment Analysis Calculator
  │     - Price per sqm vs cantonal benchmark
  │     - Gross yield = (annualRent / purchasePrice) × 100
  │     - Net yield = ((annualRent - costs) / totalInvestment) × 100
  │     - Cashflow scenarios (base / pessimistic / optimistic)
  │
  ├─→ Risk Analysis Calculator
  │     - Pricing risk (deviation from benchmark)
  │     - Vacancy risk (cantonal vacancy rate + indicators)
  │     - Renovation risk (age, condition → cost ranges)
  │
  ├─→ Development Potential Calculator
  │     - Max buildable floor area (from zoning type × plot area × FAR)
  │     - Existing built area vs max → unused potential %
  │     - Underutilization indicator (LOW / MEDIUM / HIGH)
  │     - Renovation vs rebuild signal
  │     - Expansion potential indicator
  │     - Planning complexity (LOW / MEDIUM / HIGH)
  │     - Permit risk (rough estimate)
  │     - All outputs marked indicative / non-binding
  │
  └─→ Investment Score Calculator
        - Weighted composite: Financial (30%) + Location (25%) + Risk (25%) + Financing-fit (20%)
        - Category scores (0-100)
        - Overall score (0-100)
        - Programmatic explanations & warnings
```

---

## 4. Database Schema

### Models

#### User

| Field        | Type          | Notes                      |
| ------------ | ------------- | -------------------------- |
| id           | String (cuid) | PK                         |
| email        | String        | Unique                     |
| name         | String        |                            |
| passwordHash | String        | bcryptjs                   |
| locale       | String        | "en" or "de", default "en" |
| createdAt    | DateTime      |                            |
| updatedAt    | DateTime      |                            |

#### InvestorProfile

| Field                  | Type          | Notes                                                       |
| ---------------------- | ------------- | ----------------------------------------------------------- |
| id                     | String (cuid) | PK                                                          |
| userId                 | String        | FK → User, unique (one profile per user)                    |
| investorType           | Enum          | NATURAL_PERSON (default), COMPANY                           |
| availableCash          | Decimal       | CHF                                                         |
| monthlyIncome          | Decimal       | CHF (gross household, natural persons)                      |
| strategy               | Enum          | BUY_AND_HOLD, YIELD, VALUE_ADD, FLIP                        |
| targetCantons          | String[]      | Array of canton codes                                       |
| preferredPropertyTypes | String[]      | Array of PropertyType values (preselects Discover filters)  |
| riskTolerance          | Enum          | LOW, MEDIUM, HIGH                                           |
| annualRevenue          | Decimal?      | CHF (company investors only)                                |
| existingDebt           | Decimal?      | CHF (company investors only)                                |
| companyEquity          | Decimal?      | CHF (company investors only)                                |
| defaultLtvPercent      | Int           | Default 80, FINMA max LTV (0–80%) for mortgage calculations |
| createdAt              | DateTime      |                                                             |
| updatedAt              | DateTime      |                                                             |

#### Property

| Field                  | Type           | Notes                                                       |
| ---------------------- | -------------- | ----------------------------------------------------------- |
| id                     | String (cuid)  | PK                                                          |
| userId                 | String         | FK → User                                                   |
| source                 | Enum           | MANUAL, URL_IMPORT                                          |
| sourceUrl              | String?        | Nullable, for URL imports                                   |
| title                  | String?        | Listing title                                               |
| address                | String         | Full address                                                |
| canton                 | String         | 2-letter code (ZH, BE, etc.)                                |
| municipality           | String?        |                                                             |
| postalCode             | String?        |                                                             |
| purchasePrice          | Decimal        | CHF                                                         |
| livingAreaSqm          | Decimal        |                                                             |
| plotAreaSqm            | Decimal?       |                                                             |
| rooms                  | Decimal        | e.g. 3.5                                                    |
| yearBuilt              | Int?           |                                                             |
| yearLastRenovation     | Int?           | Year of last major renovation                               |
| condition              | Enum           | NEW, GOOD, FAIR, RENOVATION_NEEDED, UNKNOWN                 |
| propertyType           | Enum           | APARTMENT, HOUSE, MULTI_FAMILY, COMMERCIAL, LAND, MIXED_USE |
| currentMonthlyRent     | Decimal?       | Actual rent if already rented                               |
| estimatedMonthlyRent   | Decimal?       | Estimated market rent                                       |
| additionalCostsMonthly | Decimal?       | Nebenkosten                                                 |
| zoningType             | Enum?          | Swiss zoning classification                                 |
| existingBuiltAreaSqm   | Decimal?       | Current built floor area (sqm)                              |
| desiredLtvPercent      | Int?           | Override profile LTV for this property (0–80%)              |
| latitude               | Decimal(10,7)? | For zoning API queries                                      |
| longitude              | Decimal(10,7)? | For zoning API queries                                      |
| noiseSensitivity       | String(5)?     | ES_I, ES_II, ES_III, ES_IV                                  |
| hasHeritageProtection  | Boolean?       | From overlapping zones                                      |
| actualFar              | Decimal(5,3)?  | Municipality-specific AZ from API/zone name                 |
| maxFloors              | Int?           | From API (e.g. ZH vollgeschosse_max)                        |
| maxBuildingHeightM     | Decimal(5,1)?  | If parseable from zone name                                 |
| description            | String?        | Free text notes                                             |
| hedonicSalePrice       | Decimal(14,2)? | PriceHubble hedonic sale valuation (bank valuation proxy)   |
| hedonicSalePriceLower  | Decimal(14,2)? | PriceHubble valuation range lower bound                     |
| hedonicSalePriceUpper  | Decimal(14,2)? | PriceHubble valuation range upper bound                     |
| hedonicRentGrossMonthly | Decimal(10,2)? | PriceHubble gross rent estimate (monthly)                  |
| hedonicRentNetMonthly  | Decimal(10,2)? | PriceHubble net rent estimate (monthly)                     |
| hedonicConfidence      | String(20)?    | Valuation confidence level (good/medium/low)                |
| hedonicConfidenceScore | Decimal(5,4)?  | Confidence score 0.0–1.0                                    |
| hedonicLocationScore   | Decimal(5,4)?  | Location quality score 0.0–1.0                              |
| createdAt              | DateTime       |                                                             |
| updatedAt              | DateTime       |                                                             |

#### Analysis

| Field                | Type          | Notes                                                |
| -------------------- | ------------- | ---------------------------------------------------- |
| id                   | String (cuid) | PK                                                   |
| userId               | String        | FK → User                                            |
| propertyId           | String        | FK → Property                                        |
| profileId            | String        | FK → InvestorProfile                                 |
| status               | Enum          | COMPLETED, FAILED                                    |
| overallScore         | Int           | 0-100                                                |
| financingScore       | Int           | 0-100                                                |
| investmentScore      | Int           | 0-100                                                |
| locationScore        | Int           | 0-100                                                |
| riskScore            | Int           | 0-100                                                |
| financingFitScore    | Int           | 0-100                                                |
| financingFeasibility | Json          | Detailed breakdown                                   |
| investmentAnalysis   | Json          | Detailed breakdown                                   |
| riskAnalysis         | Json          | Detailed breakdown                                   |
| scoreBreakdown       | Json          | Explanation per category                             |
| cashflowScenarios    | Json          | Base / pessimistic / optimistic                      |
| developmentPotential | Json?         | Development potential analysis (nullable for legacy) |
| warnings             | String[]      | Human-readable warnings                              |
| createdAt            | DateTime      |                                                      |
| updatedAt            | DateTime      |                                                      |

#### SwissCantonData (Reference Data)

| Field                   | Type          | Notes                                |
| ----------------------- | ------------- | ------------------------------------ |
| id                      | String (cuid) | PK                                   |
| cantonCode              | String        | Unique, 2-letter (ZH, BE, GE, etc.)  |
| cantonName              | String        | Full name                            |
| avgPricePerSqmApartment | Decimal       | CHF/sqm                              |
| avgPricePerSqmHouse     | Decimal       | CHF/sqm                              |
| avgRentPerSqmApartment  | Decimal       | Monthly CHF/sqm (BFS Mietpreisindex) |
| avgRentPerSqmHouse      | Decimal       | Monthly CHF/sqm (BFS Mietpreisindex) |
| vacancyRatePercent      | Decimal       | %                                    |
| populationGrowthPercent | Decimal       | % annual                             |
| updatedAt               | DateTime      | When data was last refreshed         |

#### SwissMunicipalityData (Reference Data)

| Field                   | Type          | Notes                                                 |
| ----------------------- | ------------- | ----------------------------------------------------- |
| id                      | String (cuid) | PK                                                    |
| bfsNumber               | Int           | Unique — BFS Gemeindenummer                           |
| name                    | String        | Municipality name (indexed)                           |
| cantonCode              | String(2)     | Parent canton (indexed)                               |
| vacancyRatePercent      | Decimal(5,2)? | BFS Leerwohnungszählung % (nullable — ~80 have data)  |
| population              | Int?          | BFS STATPOP permanent residents (nullable)            |
| populationGrowthPercent | Decimal(5,2)? | Annual % growth (nullable)                            |
| updatedAt               | DateTime      | When data was last refreshed                          |
| postalCodes             | Relation      | SwissPostalCode[] — PLZ entries for this municipality |

All 2,121 Swiss municipalities seeded from swisstopo Amtliches Ortschaftenverzeichnis. ~80 municipalities have vacancy/population data; the rest fall back to canton-level data in the analysis engine.

#### SwissPostalCode (Reference Data)

| Field      | Type          | Notes                                          |
| ---------- | ------------- | ---------------------------------------------- |
| id         | String (cuid) | PK                                             |
| postalCode | String(4)     | Swiss PLZ (indexed)                            |
| locality   | String        | Locality name (e.g. "Zürich")                  |
| bfsNumber  | Int           | FK → SwissMunicipalityData.bfsNumber (indexed) |
| cantonCode | String(2)     | Canton code                                    |

Composite unique: `[postalCode, locality]`. 4,073 PLZ entries seeded from swisstopo data. Used for PLZ-based municipality autocomplete and auto-fill in property forms.

#### Listing (Global Listings)

| Field              | Type              | Notes                                                       |
| ------------------ | ----------------- | ----------------------------------------------------------- |
| id                 | String (cuid)     | PK                                                          |
| source             | ListingSource     | FLATFOX, ALLE_IMMOBILIEN, AGENT, INVESTOR, OFF_MARKET       |
| externalId         | String            | Portal-specific ID or generated for agent/investor listings |
| status             | ListingStatus     | ACTIVE (default), DELISTED, SOLD                            |
| sourceUrl          | String?           | Original listing URL                                        |
| title              | String?           | Listing title                                               |
| address            | String?           | Full address                                                |
| canton             | String            | 2-letter code                                               |
| municipality       | String?           |                                                             |
| postalCode         | String?           |                                                             |
| purchasePrice      | Decimal           | CHF                                                         |
| livingAreaSqm      | Decimal           | sqm                                                         |
| plotAreaSqm        | Decimal?          | sqm                                                         |
| rooms              | Decimal?          | e.g. 3.5                                                    |
| yearBuilt          | Int?              |                                                             |
| yearLastRenovation | Int?              |                                                             |
| condition          | PropertyCondition | Default UNKNOWN                                             |
| propertyType       | PropertyType      |                                                             |
| latitude           | Decimal(10,7)?    |                                                             |
| longitude          | Decimal(10,7)?    |                                                             |
| description        | String?           | Free text                                                   |
| contactEmail       | String?           | Submitter contact email (agent/investor listings)           |
| contactPhone       | String?           | Submitter contact phone (agent/investor listings)           |
| submittedByUserId  | String?           | FK → User (for agent/investor submitted listings)           |
| hedonicSalePrice       | Decimal(14,2)?    | PriceHubble hedonic sale valuation (shared cache)           |
| hedonicSalePriceLower  | Decimal(14,2)?    | PriceHubble valuation range lower bound                     |
| hedonicSalePriceUpper  | Decimal(14,2)?    | PriceHubble valuation range upper bound                     |
| hedonicRentGrossMonthly | Decimal(10,2)?   | PriceHubble gross rent estimate (monthly)                   |
| hedonicRentNetMonthly  | Decimal(10,2)?    | PriceHubble net rent estimate (monthly)                     |
| hedonicConfidence      | String(20)?       | Valuation confidence level (good/medium/low)                |
| hedonicConfidenceScore | Decimal(5,4)?     | Confidence score 0.0–1.0                                    |
| hedonicLocationScore   | Decimal(5,4)?     | Location quality score 0.0–1.0                              |
| lastSeenAt         | DateTime          | Last seen in portal sync                                    |
| createdAt          | DateTime          |                                                             |
| updatedAt          | DateTime          |                                                             |

Unique constraint: `[externalId, source]`. Indexes on `status`, `canton`, `postalCode`. The `submittedByUserId` relation enables agents and investors to manage their own listings.

### Prisma Enums

```prisma
enum Strategy {
  BUY_AND_HOLD
  YIELD
  VALUE_ADD
  FLIP
}

enum RiskTolerance {
  LOW
  MEDIUM
  HIGH
}

enum PropertySource {
  MANUAL
  URL_IMPORT
}

enum PropertyCondition {
  NEW
  GOOD
  FAIR
  RENOVATION_NEEDED
  UNKNOWN
}

enum PropertyType {
  APARTMENT
  HOUSE
  MULTI_FAMILY
  COMMERCIAL
  LAND
  MIXED_USE
}

enum AnalysisStatus {
  COMPLETED
  FAILED
}

enum ListingStatus {
  ACTIVE
  DELISTED
  SOLD
}

enum ListingSource {
  FLATFOX
  ALLE_IMMOBILIEN
  AGENT
  INVESTOR
  OFF_MARKET
}

enum ZoningType {
  W2          // Wohnzone 2 Geschosse
  W3          // Wohnzone 3 Geschosse
  W4          // Wohnzone 4 Geschosse
  W5          // Wohnzone 5+ Geschosse
  WG2         // Wohn-/Gewerbezone 2 Geschosse
  WG3         // Wohn-/Gewerbezone 3 Geschosse
  WG4         // Wohn-/Gewerbezone 4+ Geschosse
  K           // Kernzone (core/center zone)
  I           // Industriezone
  G           // Gewerbezone (commercial)
  OE          // Zone für öffentliche Bauten
  LANDWIRTSCHAFT  // Agricultural zone
  OTHER       // Other / unknown
}
```

---

## 5. Authentication & Sessions

### Approach

- **bcryptjs** for password hashing (cost factor 12)
- **createCookieSessionStorage** from React Router for session management
- Session cookie: `__holyestate_session`, httpOnly, secure, sameSite=lax
- Session contains: `userId`, `locale`

### Auth Flow

1. **Register:** Validate input → hash password → create User → set session cookie → redirect to `/app/profile`
2. **Login:** Validate input → find user by email → verify password → set session cookie → redirect to `/app/dashboard`
3. **Logout:** Destroy session → redirect to `/`

### Route Protection

- `requireUser(request)` helper in loaders/actions
- Returns `User` object or throws redirect to `/login`
- All `/app/*` routes use this in their loaders

---

## 6. Internationalization (i18n)

### Setup

- **remix-i18next** for SSR integration with React Router v7 framework mode
- **i18next-fs-backend** loads translations server-side from `app/locales/`
- **i18next-http-backend** loads translations client-side via `/locales/` endpoint
- **i18next-browser-languagedetector** detects browser language

### Translation Files

```
app/locales/
  ├── en/
  │   ├── common.json       # Shared UI (nav, buttons, labels)
  │   ├── auth.json          # Login, register, password
  │   ├── profile.json       # Investor profile
  │   ├── property.json      # Property input
  │   ├── analysis.json      # Analysis results, scores
  │   └── landing.json       # Landing page
  └── de/
      ├── common.json
      ├── auth.json
      ├── profile.json
      ├── property.json
      ├── analysis.json
      └── landing.json
```

### Namespaces

| Namespace | Content                                        |
| --------- | ---------------------------------------------- |
| common    | Navigation, buttons, generic labels, errors    |
| auth      | Login/register forms, password validation      |
| profile   | Investor profile fields, strategies, cantons   |
| property  | Property form fields, types, conditions        |
| analysis  | Score labels, risk indicators, financial terms |
| landing   | Marketing copy, CTAs                           |

### Language Switching

- User preference stored in `User.locale` field
- Can be changed in settings
- URL-based detection as fallback
- Default: `en`

---

## 7. Project Structure

```
holyestate/
├── app/
│   ├── components/
│   │   ├── ui/                     # shadcn/ui components
│   │   ├── layout/
│   │   │   ├── app-layout.tsx      # Authenticated app shell
│   │   │   ├── header.tsx
│   │   │   ├── sidebar.tsx
│   │   │   └── footer.tsx
│   │   ├── auth/
│   │   │   ├── login-form.tsx
│   │   │   └── register-form.tsx
│   │   ├── profile/
│   │   │   └── profile-form.tsx
│   │   ├── property/
│   │   │   └── property-form.tsx
│   │   ├── analysis/
│   │   │   ├── score-gauge.tsx
│   │   │   ├── financing-card.tsx
│   │   │   ├── investment-card.tsx
│   │   │   ├── risk-card.tsx
│   │   │   ├── cashflow-chart.tsx
│   │   │   └── warnings-list.tsx
│   │   └── charts/
│   │       └── ...
│   ├── lib/
│   │   ├── auth.server.ts          # Session helpers, requireUser
│   │   ├── db.server.ts            # Prisma client singleton
│   │   ├── validation.ts           # Shared Zod schemas
│   │   ├── i18n/
│   │   │   ├── i18n.ts             # i18next config
│   │   │   ├── i18n.server.ts      # Server-side i18n init
│   │   │   └── i18next.server.ts   # remix-i18next instance
│   │   ├── services/
│   │   │   ├── user.server.ts      # User CRUD
│   │   │   ├── profile.server.ts   # InvestorProfile CRUD
│   │   │   ├── property.server.ts  # Property CRUD
│   │   │   ├── analysis.server.ts  # Analysis orchestration
│   │   │   └── listing-submission.server.ts # Shared listing create/update/delist (agent + investor)
│   │   ├── analysis/
│   │   │   ├── engine.server.ts    # Main orchestrator
│   │   │   ├── financing.server.ts # Financing feasibility
│   │   │   ├── investment.server.ts# Investment analysis
│   │   │   ├── risk.server.ts      # Risk analysis
│   │   │   ├── scoring.server.ts    # Investment scoring
│   │   │   ├── development.server.ts # Development potential calculator
│   │   │   └── constants.ts        # Swiss RE constants & rules
│   │   └── utils/
│   │       ├── cn.ts               # clsx + tailwind-merge
│   │       └── format.ts           # Number/currency formatting (CHF)
│   ├── locales/
│   │   ├── en/                     # English translations
│   │   └── de/                     # German translations
│   ├── routes/
│   │   ├── _landing.tsx            # Public landing page (index)
│   │   ├── auth/
│   │   │   ├── login.tsx
│   │   │   ├── register.tsx
│   │   │   └── logout.tsx
│   │   ├── app/
│   │   │   ├── layout.tsx          # Authenticated layout wrapper
│   │   │   ├── dashboard.tsx       # Dashboard (past analyses)
│   │   │   ├── profile.tsx         # Investor profile
│   │   │   ├── properties/
│   │   │   │   ├── index.tsx       # Property list
│   │   │   │   ├── new.tsx         # Add property
│   │   │   │   └── $id.tsx         # View/edit property
│   │   │   ├── analyses/
│   │   │   │   ├── new.tsx         # Run analysis (select property + profile)
│   │   │   │   └── $id.tsx         # View analysis results
│   │   │   └── my-listings/
│   │   │       ├── index.tsx       # Investor listing index (my submissions)
│   │   │       ├── new.tsx         # Create investor listing
│   │   │       └── $id.tsx         # View/edit/delist investor listing
│   │   └── api/
│   │       └── locales.ts          # Serve translation files
│   ├── root.tsx
│   ├── routes.ts                   # Route configuration
│   └── app.css                     # Tailwind imports
├── prisma/
│   ├── schema.prisma
│   ├── migrations/
│   └── seed.ts                     # Swiss reference data seeding
├── public/
│   └── favicon.ico
├── __tests__/
│   ├── unit/
│   │   ├── analysis/               # Analysis engine unit tests
│   │   ├── services/               # Service layer tests
│   │   └── validation/             # Schema validation tests
│   └── integration/
│       ├── auth.test.ts            # Auth flow tests
│       ├── profile.test.ts         # Profile CRUD tests
│       ├── property.test.ts        # Property CRUD tests
│       └── analysis.test.ts        # Analysis pipeline tests
├── e2e/
│   └── *.spec.ts                   # Playwright E2E tests
├── docs/
│   ├── api/
│   │   └── openapi.yaml
│   ├── user-guide/
│   ├── developer/
│   │   └── TECH_DEBT.md
│   ├── compliance/
│   └── deployment/
├── react-router.config.ts
├── vite.config.ts
├── tsconfig.json
├── eslint.config.js
├── .prettierrc
├── package.json
├── TECHNICAL_PLAN.md
├── PROGRESS.md
└── ROADMAP.md
```

---

## 8. Analysis Engine (Deterministic)

### Swiss Financial Constants

```typescript
// Phase 1 — hardcoded, derived from Swiss regulations and market standards
export const SWISS_RE_CONSTANTS = {
  // FINMA / lending standards
  MIN_EQUITY_RATIO: 0.2, // 20% minimum equity
  MIN_HARD_EQUITY_RATIO: 0.1, // 10% must be non-pension
  STRESS_TEST_RATE: 0.05, // 5% imputed interest rate
  AFFORDABILITY_THRESHOLD: 0.33, // Max 33% of gross income
  MAINTENANCE_COST_RATIO: 0.01, // 1% of property value/year
  AMORTIZATION_YEARS: 15, // Amortize to 65% LTV in 15 years

  // Transaction costs
  NOTARY_FEES_PERCENT: 0.005, // ~0.5% (varies by canton)
  TRANSFER_TAX_PERCENT: 0.01, // ~1% (varies by canton)
  LAND_REGISTRY_PERCENT: 0.002, // ~0.2%

  // Yield assumptions
  MANAGEMENT_COST_RATIO: 0.05, // 5% of gross rent
  VACANCY_ALLOWANCE_DEFAULT: 0.03, // 3% default if no data

  // Renovation cost ranges (CHF/sqm)
  RENOVATION_COSTS: {
    NEW: { min: 0, max: 50 },
    GOOD: { min: 50, max: 200 },
    FAIR: { min: 200, max: 600 },
    RENOVATION_NEEDED: { min: 600, max: 1500 },
    UNKNOWN: { min: 200, max: 800 },
  },

  // Cashflow scenario adjustments
  SCENARIOS: {
    optimistic: { vacancyAdj: -0.5, rentAdj: 1.05, rateAdj: -0.005 },
    base: { vacancyAdj: 1.0, rentAdj: 1.0, rateAdj: 0 },
    pessimistic: { vacancyAdj: 1.5, rentAdj: 0.95, rateAdj: 0.01 },
  },
};
```

### PriceHubble Hedonic Valuation Integration

**Purpose:** Swiss banks calculate LTV against `min(purchasePrice, bankValuation)` (Niederstwertprinzip). PriceHubble provides hedonic property valuations and rent estimates via REST API.

**Architecture:**

- **Auth client** (`app/lib/pricehubble/auth.server.ts`): Module-level token cache (12h TTL), auto-refresh with 1-min buffer, returns null when unconfigured or on error.
- **Mapper** (`app/lib/pricehubble/mapper.server.ts`): Maps PropertyType/Condition enums to PriceHubble API codes. Returns null for unsupported types (LAND) or missing location data.
- **Valuation client** (`app/lib/pricehubble/valuation.server.ts`): Two API calls per property (sale + rent), 401 retry with token refresh, rent failure is non-critical.
- **Cache service** (`app/lib/pricehubble/cache.server.ts`): Two-level cache — check Property → check source Listing → API call → write-back to both. No cache expiry.

**Niederstwertprinzip (financing engine):**

```
belehnungswert = bankValuation != null ? min(purchasePrice, bankValuation) : purchasePrice
```

Mortgage amount, LTV, amortization target all use `belehnungswert` as basis. `totalInvestment` stays based on `purchasePrice` (buyer pays actual price).

**Rent priority:** `user (currentMonthlyRent || estimatedMonthlyRent) → hedonic → market_estimate → none`

**Graceful degradation:** If PriceHubble is unconfigured, unavailable, or returns an error, the analysis runs with current behavior (purchasePrice as LTV basis, cantonal averages for rent). A `PRICEHUBBLE_UNAVAILABLE` warning informs the user.

**Warning codes:** `NIEDERSTWERT_APPLIED`, `HEDONIC_LOW_CONFIDENCE` (score < 0.5), `USING_HEDONIC_RENT`, `PRICEHUBBLE_UNAVAILABLE`

### Development Potential Constants

```typescript
// Simplified Swiss zoning rules — Floor Area Ratio (Ausnützungsziffer)
// These are indicative averages; actual values vary by municipality
export const ZONING_FAR: Record<
  ZoningType,
  { min: number; max: number; typical: number; maxFloors: number }
> = {
  W2: { min: 0.25, max: 0.4, typical: 0.3, maxFloors: 2 },
  W3: { min: 0.4, max: 0.6, typical: 0.5, maxFloors: 3 },
  W4: { min: 0.6, max: 0.9, typical: 0.7, maxFloors: 4 },
  W5: { min: 0.8, max: 1.4, typical: 1.0, maxFloors: 5 },
  WG2: { min: 0.3, max: 0.5, typical: 0.4, maxFloors: 2 },
  WG3: { min: 0.5, max: 0.7, typical: 0.6, maxFloors: 3 },
  WG4: { min: 0.7, max: 1.2, typical: 0.9, maxFloors: 4 },
  K: { min: 0.6, max: 1.5, typical: 1.0, maxFloors: 5 },
  I: { min: 0.4, max: 1.0, typical: 0.6, maxFloors: 3 },
  G: { min: 0.4, max: 0.8, typical: 0.6, maxFloors: 3 },
  OE: { min: 0.3, max: 0.8, typical: 0.5, maxFloors: 4 },
  LANDWIRTSCHAFT: { min: 0, max: 0.1, typical: 0.05, maxFloors: 2 },
  OTHER: { min: 0.2, max: 0.6, typical: 0.4, maxFloors: 3 },
};

// Single source of truth for zoning type list — used by validation + UI
export const ZONING_TYPES = Object.keys(ZONING_FAR) as ZoningType[];
```

### Development Potential Calculator

**Input:** plotAreaSqm + zoningType + existingBuiltAreaSqm (optional)

**Output (JSON):**

```typescript
{
  maxBuildableAreaSqm: {
    min: number;
    max: number;
    typical: number;
  }
  existingBuiltAreaSqm: number;
  unusedPotentialPercent: number; // 0–100
  underutilizationLevel: "LOW" | "MEDIUM" | "HIGH";
  renovationVsRebuild: "RENOVATE" |
    "CONSIDER_REBUILD" |
    "REBUILD_LIKELY" |
    "NOT_APPLICABLE";
  expansionPotential: "NONE" | "LOW" | "MEDIUM" | "HIGH";
  planningComplexity: "LOW" | "MEDIUM" | "HIGH";
  permitRisk: "LOW" | "MEDIUM" | "HIGH";
  disclaimer: string; // Always: "Indicative estimate — verification required"
}
```

**UX rules:**

- NEVER say "You can build X m²"
- ALWAYS say "Estimated buildable area based on public zoning data — verification required"
- All values shown as ranges, not absolutes

### Scoring Weights

| Category      | Weight | Measures                                            |
| ------------- | ------ | --------------------------------------------------- |
| Financial     | 30%    | Yield, cashflow positivity, price vs benchmark      |
| Location      | 25%    | Vacancy rate, population growth, demand indicators  |
| Risk          | 25%    | Pricing risk, renovation risk, vacancy risk         |
| Financing Fit | 20%    | Equity sufficiency, affordability margin, LTV ratio |

### Warning Generation

Programmatic warnings (no AI) based on thresholds:

- Equity shortfall warning
- Affordability over threshold
- Price significantly above benchmark (>15%)
- High vacancy area (>3%)
- Old building without recent renovation
- Negative cashflow in base scenario
- High underutilization detected (>50% unused potential)
- Rebuild likely more viable than renovation
- Agricultural zone — very limited development potential

---

## 9. Swiss Real Estate Reference Data

### Public Data Sources

Phase 1 uses curated reference data seeded into the `SwissCantonData` table. Data is sourced from:

| Data               | Source                           | URL                                                                  | Update Frequency |
| ------------------ | -------------------------------- | -------------------------------------------------------------------- | ---------------- |
| Price indices      | SNB Data Portal                  | https://data.snb.ch/en/topics/uvo                                    | Quarterly        |
| Price per sqm      | Wüest Partner (free index)       | https://www.wuest.io/online_services_classic/transaktionspreisindex/ | Quarterly        |
| Vacancy rates      | BFS Leerwohnungszählung          | https://opendata.swiss (search "Leerwohnungen")                      | Annual (June)    |
| Mortgage ref. rate | BWO (Federal Office for Housing) | https://www.bwo.admin.ch                                             | Quarterly        |
| Construction costs | BFS Baupreisindex                | https://opendata.swiss (Baupreisindex dataset)                       | Semi-annual      |
| General datasets   | opendata.swiss portal            | https://opendata.swiss/en                                            | Varies           |

### Seeding Strategy

1. Curate cantonal averages from above sources into a seed file (`prisma/seed.ts`)
2. Cover all 26 cantons with price/sqm, vacancy rate, population growth
3. Seed runs on `prisma migrate reset` and can be run manually
4. **Phase 2+**: Automate data refresh via scheduled jobs hitting SNB/BFS APIs

### SNB API Access

```
Base URL: https://data.snb.ch/api/cube/{cubeId}/data/csv/{language}
Example:  https://data.snb.ch/api/cube/plimoinreg/data/csv/en
Docs:     https://data.snb.ch/en/help_api
```

---

## 10. API Design & Conventions

### Route Pattern

React Router v7 framework mode — no separate API layer. Server logic runs in `loader` and `action` functions within route files.

### Conventions

- **Loaders** return data objects directly (no `json()` wrapper — see CLAUDE.md §9 TD-001)
- **Actions** process form submissions and redirect on success
- **Error responses** use `throw new Response(message, { status })`
- **Validation** runs server-side with Zod, errors returned to form via `useActionData`
- **Services** accept `db: PrismaClient` (or tenant-scoped equivalent) as first parameter

### Data Access Pattern

```typescript
// Service function signature
export async function getPropertiesByUser(db: PrismaClient, userId: string) {
  return db.property.findMany({
    where: { userId },
    orderBy: { createdAt: "desc" },
  });
}

// Route loader usage
export async function loader({ request }: Route.LoaderArgs) {
  const user = await requireUser(request);
  const properties = await getPropertiesByUser(db, user.id);
  return { properties };
}
```

---

## 11. Security

| Concern          | Approach                                                          |
| ---------------- | ----------------------------------------------------------------- |
| Auth             | bcryptjs (cost 12), cookie sessions, httpOnly + secure + sameSite |
| Data isolation   | All queries scoped by userId                                      |
| Input validation | Zod schemas on all inputs, server-side                            |
| XSS              | React's default escaping + no dangerouslySetInnerHTML             |
| CSRF             | SameSite=lax cookies + React Router form handling                 |
| SQL injection    | Prisma parameterized queries                                      |
| Rate limiting    | Middleware on auth routes (future: all routes)                    |
| Secrets          | Environment variables, never in code                              |
| HTTPS            | Enforced at Railway deployment level                              |

---

## 12. Deployment

### Platform: Railway

- **Web service**: Node.js (React Router serve)
- **Database**: Railway PostgreSQL 16.x
- **Build command**: `npm run build`
- **Start command**: `npm run start` (runs `react-router-serve ./build/server/index.js`)
- **Migrations**: `npx prisma migrate deploy` (runs automatically in Railway deploy)

### CI/CD

- Railway auto-deploys from `main` branch
- Pre-deploy: `prisma migrate deploy`
- Health check: `GET /` returns 200

---

## 13. Environment Variables

| Variable                       | Description                                                        | Required  |
| ------------------------------ | ------------------------------------------------------------------ | --------- |
| DATABASE_URL                   | PostgreSQL connection string                                       | Yes       |
| SESSION_SECRET                 | Cookie session encryption key                                      | Yes       |
| NODE_ENV                       | "development" or "production"                                      | Yes       |
| RESEND_API_KEY                 | Resend email service API key                                       | Prod only |
| PRICEHUBBLE_USERNAME           | PriceHubble API username (hedonic valuation)                       | No        |
| PRICEHUBBLE_PASSWORD           | PriceHubble API password (hedonic valuation)                       | No        |
| APP_URL                        | Application base URL (for invite links, emails)                    | Prod only |
| ANTHROPIC_API_KEY              | Anthropic API key for Claude Haiku (listing import extraction)     | Phase 2+  |
| IMPORT_LISTING_LIMIT           | Max listings per portal import (default 30, range 1-100)           | No        |
| ALLE_IMMOBILIEN_CRAWL_DELAY_MS | Delay between page fetches for alle-immobilien.ch (default 5000ms) | No        |
| PORT                           | Server port (default 3000)                                         | No        |

---

## 14. Design System & Branding

### Brand

- **Name**: HolyEstate
- **Tagline**: "Reduce risk. Buy faster."

### Color Palette

| Token         | Color                             | Usage                          |
| ------------- | --------------------------------- | ------------------------------ |
| Primary       | Green (#16a34a / green-600)       | CTAs, primary actions, scores  |
| Primary Light | Light green (#bbf7d0 / green-200) | Backgrounds, highlights        |
| Surface       | White (#ffffff)                   | Card backgrounds, main content |
| Background    | Light gray (#f9fafb / gray-50)    | Page background                |
| Border        | Gray (#e5e7eb / gray-200)         | Borders, dividers              |
| Text          | Dark gray (#111827 / gray-900)    | Primary text                   |
| Text Muted    | Medium gray (#6b7280 / gray-500)  | Secondary text                 |
| Danger        | Red (#dc2626 / red-600)           | Errors, high risk              |
| Warning       | Amber (#d97706 / amber-600)       | Warnings, medium risk          |
| Success       | Green (#16a34a / green-600)       | Success, low risk              |

### Typography

- Font: System font stack (via Tailwind defaults)
- Headings: `font-semibold`
- Body: `font-normal`

---

## 15. Changelog

| Date       | Change                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Author |
| ---------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------ |
| 2026-02-22 | Initial technical plan created                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        | —      |
| 2026-02-22 | Added Development Potential Estimate feature to Phase 1: ZoningType enum, Property fields (zoningType, existingBuiltAreaSqm), Analysis field (developmentPotential JSON), development.server.ts calculator, zoning FAR constants                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      | —      |
| 2026-02-23 | DRY refactor: ZONING_TYPES derived from ZONING_FAR keys in constants.ts. Validation and UI forms import from single source instead of hardcoding the 13 zoning type strings                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           | —      |
| 2026-02-22 | Epic 5: Added Results Dashboard — analysis detail page, analyses list, dashboard with stats, score gauge SVG, cashflow chart (Recharts lazy-loaded), risk/financing/investment cards, score breakdown table, warnings list, "Run Analysis" button on property edit page. Added recharts dependency. 277 tests passing.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | —      |
| 2026-02-22 | Epic 6: Export & Polish — PDF export via @react-pdf/renderer (server-side API route at /api/analyses/:id/pdf), mobile-responsive navigation (hamburger menu + sidebar overlay), global loading bar (useNavigation), route-level error boundary, reusable EmptyState + Skeleton components. 295 tests passing. Phase 1 complete (60/60 tasks).                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         | —      |
| 2026-02-27 | Task 6.5: Automatic rent estimation — added avgRentPerSqmApartment/House to SwissCantonData, rent-estimator.server.ts (market + yield-based), RentSource tracking, USING_ESTIMATED_RENT warning, UI/PDF rent estimate display. 318 tests.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | —      |
| 2026-02-27 | Task 6.6: Educational info panels — reusable InfoPanel component (collapsible), added to Investment Analysis (yield formulas + Swiss benchmarks) and Cashflow Scenarios (scenario assumptions). EN/DE translations. 321 tests.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        | —      |
| 2026-02-27 | Task 6.7: Estimate built area button — property form "Estimate" button computes existingBuiltAreaSqm as livingAreaSqm × 1.25 (rough gross floor area). Both new/edit forms. EN/DE translations.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       | —      |
| 2026-02-28 | Task 6.8: Year of last renovation — added yearLastRenovation Int? to Property model, risk calculator uses effective age (yearLastRenovation ?? yearBuilt), new "recently renovated" rationale. Migration, validation, forms, EN/DE translations. 5 new risk tests.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    | —      |
| 2026-02-28 | Task 7.1: Company/Legal entity investor type — InvestorType enum (NATURAL_PERSON/COMPANY), company fields (annualRevenue, existingDebt, companyEquity), DSCR-based affordability for companies (NOI/Debt Service), dual-path financing calculator, company-aware scoring + warnings, profile form toggle, financing card + PDF DSCR display. Migration, EN/DE translations. 346 tests (20 new).                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       | —      |
| 2026-02-28 | Task 8.1: Verified reference data + municipality granularity — SwissMunicipalityData model (bfsNumber, name, cantonCode, vacancy, population, popGrowth), 86 municipalities seeded across all 26 cantons, canton data updated with verified BFS/Neho numbers (prices, rents, vacancy rates, population growth), analysis engine merges municipality-level vacancy/population into CantonData when available, `/api/municipalities` autocomplete API, property form datalist autocomplete, EN/DE translations. Migration `add_swiss_municipality_data`. 355 tests (9 new).                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | —      |
| 2026-03-01 | Task 8.2: Full municipality coverage + PLZ support — expanded from 86 to 2,121 municipalities (swisstopo Amtliches Ortschaftenverzeichnis), added SwissPostalCode model (4,073 PLZ entries), made municipality stats nullable (~80 have data, rest fall back to canton), PLZ-based search in `/api/municipalities` API (numeric queries auto-detected), PLZ auto-fill in property forms (auto-fills municipality + canton), Swiss PLZ format validation (4 digits), PLZ-based municipality fallback in analysis service, null-safe stat merging, `scripts/import-swiss-data.ts` reproducible import script. Migration `add_postal_codes_nullable_stats`. 364 tests (18 new).                                                                                                                                                                                                                                                                                                                                                                                          | —      |
| 2026-03-01 | Task 8.3: Address autocomplete — `/api/address-search` server-side proxy to geo.admin.ch SearchServer (swisstopo), `AddressAutocomplete` component (debounced search, keyboard nav, a11y), `useDebounce` hook, property form integration (selecting address auto-fills PLZ + municipality + canton). EN/DE translations. 379 tests (15 new).                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          | —      |
| 2026-03-01 | Task 9.1: Configurable LTV — added `defaultLtvPercent` (Int, default 80) to InvestorProfile and `desiredLtvPercent` (Int?, nullable) to Property. Financing calculator now derives mortgage from desired LTV (`purchasePrice × ltvPercent / 100`) instead of remaining cash. Added `hasEnoughEquity` check with -15 feasibility score penalty. LTV resolution chain: property override → profile default → 80% fallback. Works for both NATURAL_PERSON and COMPANY investor types. Profile form LTV field (0–80%, step 5), property form LTV override field. Zod validation, EN/DE translations. Migration `add_configurable_ltv`. 388 tests (7 new).                                                                                                                                                                                                                                                                                                                                                                                                                 | —      |
| 2026-03-02 | Epic 10: Automatic Zoning & Buildability Data — 7 new fields on Property (latitude, longitude, noiseSensitivity, hasHeritageProtection, actualFar, maxFloors, maxBuildingHeightM). `/api/zoning-lookup` route with 3 data sources: geodienste.ch OGC API (~18 cantons, 3 collections in parallel), Canton Zurich WFS (maps.zh.ch, WGS84→LV95 conversion), GeoAdmin ch.are.bauzonen (fallback). Zone name parser extracts FAR/stories/height from cantonal descriptions. Enhanced calculator uses actual FAR (±10% range), actual maxFloors, heritage forces RENOVATE + HIGH risk, noise ES_III/IV raises permit risk for residential. Property forms auto-trigger zoning lookup on address select, auto-fill zoningType + buildability fields. Results card shows floors, footprint/floor, noise, heritage, data source. EN/DE translations. Migration `add_buildability_fields`. 461 tests (73 new).                                                                                                                                                                 | —      |
| 2026-03-03 | P2-E1: Listing Import — `@anthropic-ai/sdk` + Claude Haiku integration for property data extraction from listing URLs and screenshots. Core service (`listing-import.server.ts`): HTML cleaning, server-side fetch with blocked detection (403/429), LLM text extraction, LLM vision extraction, field validation. API routes: `POST /api/import-listing` (URL → fetch → extract), `POST /api/import-screenshot` (image → vision extract). `ListingImport` UI component (state machine, two fetchers, drag-and-drop screenshot upload). Pre-fill integration in new.tsx + edit.tsx (controlled selects, DOM manipulation, auto-fill badges). Extended `createProperty` with source/sourceUrl options. `SCREENSHOT_IMPORT` added to PropertySource enum. `ANTHROPIC_API_KEY` env var. EN/DE translations (~20 keys). Migration `add_screenshot_import_source`. 516 tests (55 new).                                                                                                                                                                                     | —      |
| 2026-03-06 | P2-E2: Automated Listing Collection — Flatfox REST API connector + alle-immobilien.ch HTML scraper. Unified `PortalListing` interface with normalization (address normalizer, property type mapping, required field validation). Cross-portal deduplication: 3-stage algorithm (PLZ blocking → Levenshtein address similarity → attribute confirmation: price ±10%, area ±10%, rooms exact). Delta import via `ImportSync` table (`lastSyncedAt` per portal per user, `externalListingId` tracking). Import orchestrator ties connectors + normalization + delta + dedup + property creation. API route `POST /api/portal-import` (trigger) + `GET` (sync history). Import UI page with portal cards, listing limit config (1-100), `useFetcher` integration. Sidebar nav "Import" item. Schema: `PORTAL_IMPORT` source, `externalListingId`/`importedFrom` on Property (unique constraint), `ImportSync` model. Env vars: `IMPORT_LISTING_LIMIT`, `ALLE_IMMOBILIEN_CRAWL_DELAY_MS`. EN/DE translations. Migration `add_portal_import_tracking`. 620 tests (104 new). | —      |
| 2026-03-22 | P2-E7: Investor Listing Submissions — `INVESTOR` added to ListingSource enum, `contactEmail` and `contactPhone` fields added to Listing model. Shared `listing-submission.server.ts` service (create/update/delist, used by both agent and investor flows). Investor listing routes: `/app/my-listings` (index), `/app/my-listings/new` (create), `/app/my-listings/:id` (view/edit/delist). Migration `add_investor_listing_source`. EN/DE translations.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | —      |
