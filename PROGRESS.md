# HolyEstate — Progress Tracker

> Updated: 2026-03-23

---

## Overview

| Phase                            | Status  | Epics | Tasks Done | Tasks Total |
| -------------------------------- | ------- | ----- | ---------- | ----------- |
| Phase 1 — AI Deal Analyzer (MVP) | Active  | 12    | 92         | 92          |
| Phase 2 — Smart Deal Discovery   | Active  | 7     | 99         | 103         |
| Phase 3 — AI Acquisition Engine  | Planned | 4     | —          | —           |
| Phase 4 — Infrastructure Layer   | Planned | 4     | —          | —           |

---

## Phase 1 — AI Deal Analyzer (Foundation MVP)

**Goal:** Prove that the platform increases decision confidence and speed for single-deal evaluation.

**Success signals:**

- Users report: "We analyze deals faster and feel safer."
- Repeat usage + willingness to pay

---

### Epic 0: Project Scaffolding

| #   | Task                                                       | Status | Tests |
| --- | ---------------------------------------------------------- | ------ | ----- |
| 0.1 | Initialize React Router v7 project with TypeScript + Vite  | [x]    | —     |
| 0.2 | Configure Tailwind CSS v4 + shadcn/ui                      | [x]    | —     |
| 0.3 | Set up Prisma with PostgreSQL + initial schema + migration | [x]    | —     |
| 0.4 | Configure react-i18next + remix-i18next (SSR) with EN/DE   | [x]    | —     |
| 0.5 | Set up ESLint (flat config), Prettier, Husky, lint-staged  | [x]    | —     |
| 0.6 | Set up Vitest + Testing Library + MSW                      | [x]    | 31/31 |
| 0.7 | Create base layout components (app shell, header, sidebar) | [x]    | —     |
| 0.8 | Configure environment variables (.env, .env.example)       | [x]    | —     |
| 0.9 | Seed Swiss reference data (all 26 cantons)                 | [x]    | —     |

---

### Epic 1: Authentication

| #   | Task                                                      | Status | Tests |
| --- | --------------------------------------------------------- | ------ | ----- |
| 1.1 | Implement session management (createCookieSessionStorage) | [x]    | 5/5   |
| 1.2 | Implement registration (form + action + bcryptjs hashing) | [x]    | 10/10 |
| 1.3 | Implement login (form + action + password verification)   | [x]    | 8/8   |
| 1.4 | Implement logout (session destroy)                        | [x]    | 2/2   |
| 1.5 | Create requireUser helper for protected routes            | [x]    | 6/6   |
| 1.6 | Add EN/DE translations for auth pages                     | [x]    | —     |
| 1.7 | Write integration tests for auth (200, 400, 401, 403)     | [x]    | 31/31 |

---

### Epic 2: Investor Profile

| #   | Task                                                                                                          | Status | Tests |
| --- | ------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| 2.1 | Create profile form (react-hook-form + Zod validation)                                                        | [x]    | 5/5   |
| 2.2 | Implement profile create/update action                                                                        | [x]    | 14/14 |
| 2.3 | Strategy selection (BUY_AND_HOLD, YIELD, VALUE_ADD, FLIP)                                                     | [x]    | —     |
| 2.4 | Financial inputs (available cash, monthly income)                                                             | [x]    | —     |
| 2.5 | Target canton selection (multi-select, all 26 cantons)                                                        | [x]    | —     |
| 2.6 | Risk tolerance selection                                                                                      | [x]    | —     |
| 2.7 | Add EN/DE translations for profile                                                                            | [x]    | —     |
| 2.8 | Write unit + integration tests for profile                                                                    | [x]    | 19/19 |
| 2.9 | Add preferred property types to profile (multi-select, preselects Discover filters along with target cantons) | [x]    | —     |

---

### Epic 3: Property Input

| #   | Task                                                           | Status | Tests |
| --- | -------------------------------------------------------------- | ------ | ----- |
| 3.1 | Create property form (react-hook-form + Zod validation)        | [x]    | 5/5   |
| 3.2 | Implement property CRUD (create, read, update, delete)         | [x]    | 10/10 |
| 3.3 | Property type selection (APARTMENT, HOUSE, MULTI_FAMILY, etc.) | [x]    | —     |
| 3.4 | Condition selection + financial fields (rent, costs)           | [x]    | —     |
| 3.5 | Property list view (with data table)                           | [x]    | —     |
| 3.6 | Add EN/DE translations for property                            | [x]    | —     |
| 3.7 | Write unit + integration tests for property                    | [x]    | 25/25 |

---

### Epic 4: Analysis Engine (Core)

| #   | Task                                                                     | Status | Tests |
| --- | ------------------------------------------------------------------------ | ------ | ----- |
| 4.1 | Financing feasibility calculator (equity, affordability, mortgage range) | [x]    | 14/14 |
| 4.2 | Investment analysis calculator (price/sqm, yields, cashflow scenarios)   | [x]    | 21/21 |
| 4.3 | Risk analysis calculator (pricing, vacancy, renovation risks)            | [x]    | 16/16 |
| 4.4 | Investment scoring (weighted composite + category scores)                | [x]    | 10/10 |
| 4.5 | Warning generation (programmatic, threshold-based)                       | [x]    | 19/19 |
| 4.6 | Analysis orchestrator (runs full pipeline, saves result)                 | [x]    | 23/23 |
| 4.7 | Write comprehensive unit tests for all calculators                       | [x]    | 68/68 |
| 4.8 | Write integration tests for full analysis pipeline                       | [x]    | 11/11 |

---

### Epic 4b: Development Potential Engine

| #     | Task                                                                                       | Status | Tests |
| ----- | ------------------------------------------------------------------------------------------ | ------ | ----- |
| 4b.1  | Define Swiss zoning types enum + development potential constants (FAR/utilization ratios)  | [x]    | 9/9   |
| 4b.2  | Add zoning fields to Property model (zoningType, existingBuiltAreaSqm) + migration         | [x]    | —     |
| 4b.3  | Update property form with zoning/buildability input fields                                 | [x]    | —     |
| 4b.4  | Development potential calculator (max buildable area, unused potential %)                  | [x]    | 43/43 |
| 4b.5  | Investment insight signals (underutilization, renovation vs rebuild, expansion potential)  | [x]    | 43/43 |
| 4b.6  | Risk indicators (planning complexity, permit risk)                                         | [x]    | 43/43 |
| 4b.7  | Update analysis orchestrator + Analysis model to include developmentPotential JSON         | [x]    | 11/11 |
| 4b.8  | Add EN/DE translations for development potential (with indicative/non-binding disclaimers) | [x]    | —     |
| 4b.9  | Write comprehensive unit tests for development potential calculator                        | [x]    | 43/43 |
| 4b.10 | Development potential results card in analysis view                                        | [x]    | —     |

---

### Epic 5: Results Dashboard & Analysis View

| #   | Task                                                                     | Status | Tests |
| --- | ------------------------------------------------------------------------ | ------ | ----- |
| 5.1 | Analysis results page (all sections: financing, investment, risk, score) | [x]    | —     |
| 5.2 | Score gauge / visualization component                                    | [x]    | —     |
| 5.3 | Cashflow chart (base / pessimistic / optimistic with Recharts)           | [x]    | —     |
| 5.4 | Risk indicators display                                                  | [x]    | —     |
| 5.5 | Warnings list component                                                  | [x]    | —     |
| 5.6 | Dashboard page (past analyses list, sortable)                            | [x]    | —     |
| 5.7 | Add EN/DE translations for analysis & dashboard                          | [x]    | —     |
| 5.8 | Write functional + integration tests                                     | [x]    | 20/20 |

---

### Epic 6: Export & Polish

| #   | Task                                              | Status | Tests |
| --- | ------------------------------------------------- | ------ | ----- |
| 6.1 | PDF export of decision pack (@react-pdf/renderer) | [x]    | 12/12 |
| 6.2 | Responsive design polish (mobile + desktop)       | [x]    | —     |
| 6.3 | Loading states, error boundaries, empty states    | [x]    | 6/6   |
| 6.4 | Financing cost breakdown + real monthly cost      | [x]    | 6/6   |
| 6.5 | Automatic rent estimation (market + yield-based)  | [x]    | 11/11 |
| 6.6 | Educational info panels (investment + cashflow)   | [x]    | 3/3   |
| 6.7 | Estimate built area button (living area × 1.25)   | [x]    | —     |
| 6.8 | Year of last renovation field + risk calculator   | [x]    | 5/5   |

---

### Epic 7: Company / Legal Entity Investor Support

| #   | Task                                                         | Status | Tests |
| --- | ------------------------------------------------------------ | ------ | ----- |
| 7.1 | Investor type toggle (NATURAL_PERSON / COMPANY) + DSCR logic | [x]    | 20/20 |

---

### Epic 8: Verified Reference Data & Municipality-Level Granularity

| #   | Task                                                                                                                                         | Status | Tests |
| --- | -------------------------------------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| 8.1 | Replace generated canton seed data with verified BFS/SNB numbers (prices, rents) + add municipality-level vacancy & population from BFS data | [x]    | 9/9   |
| 8.2 | Full municipality coverage (all 2,121) + postal code (PLZ) support with auto-fill                                                            | [x]    | 18/18 |
| 8.3 | Address autocomplete with geo.admin.ch (swisstopo) — auto-fills PLZ, municipality, canton                                                    | [x]    | 15/15 |

**What was done in 8.1:**

- Replaced all 26 canton entries with verified BFS/SNB numbers (prices from Neho, vacancy from BFS Leerwohnungszählung 2024/2025, population growth from BFS STATPOP 2024)
- Added `SwissMunicipalityData` model (bfsNumber, name, cantonCode, vacancyRatePercent, population, populationGrowthPercent)
- Seeded 86 representative municipalities covering all 26 cantons
- Updated analysis engine: municipality-level vacancy + population override canton values when property has municipality set
- Added municipality autocomplete to property form (datalist + useFetcher)
- Created `/api/municipalities` API route for autocomplete
- Added EN/DE translations for municipality hint
- All downstream analysis modules unchanged (same `CantonData` interface)

**What was done in 8.2:**

- Created `scripts/import-swiss-data.ts` — reproducible import from swisstopo Amtliches Ortschaftenverzeichnis (official PLZ directory)
- Expanded from 86 to all 2,121 Swiss municipalities (from official swisstopo data)
- Added `SwissPostalCode` model (postalCode, locality, bfsNumber, cantonCode) with 4,073 PLZ entries
- Made municipality stats (vacancy, population, popGrowth) nullable — ~80 have data, rest fall back to canton-level
- Extended `/api/municipalities` with PLZ-based search (numeric queries auto-detected)
- Added PLZ auto-fill: typing 3-4 digit PLZ auto-suggests municipality and canton
- Added Swiss PLZ format validation (4 digits) to property schema
- Added PLZ-based municipality fallback in analysis service
- Null-safe municipality stat merging (municipality may exist but have null stats)
- Added EN/DE translations for PLZ hints and validation messages
- 364 tests passing (9 new tests)

**What was done in 8.3:**

- Created `/api/address-search` API route — server-side proxy to geo.admin.ch SearchServer
- Parses geo.admin.ch response into structured fields: streetAddress, postalCode, city, canton
- Created reusable `AddressAutocomplete` component with debounced search (300ms), keyboard navigation (Arrow keys, Enter, Escape), accessible (combobox role, listbox, aria attributes)
- Created generic `useDebounce` hook (`app/hooks/use-debounce.ts`)
- Integrated into both property forms (new.tsx + edit.tsx) — selecting an address auto-fills PLZ, municipality, and canton
- Added EN/DE translations for address hints
- 379 tests passing (15 new tests)

---

### Epic 9: Configurable LTV (Loan-to-Value)

| #   | Task                                                                  | Status | Tests |
| --- | --------------------------------------------------------------------- | ------ | ----- |
| 9.1 | Configurable LTV: schema, calculator, forms, validation, translations | [x]    | 7/7   |

**What was done in 9.1:**

- Added `defaultLtvPercent` (Int, default 80) to InvestorProfile — applies to both natural persons and companies
- Added `desiredLtvPercent` (Int?, nullable) to Property — overrides profile default per property
- Updated financing calculator: mortgage derived from desired LTV (`purchasePrice * ltvPercent / 100`) instead of remaining cash
- Added `hasEnoughEquity` check: validates investor has enough cash for their chosen LTV equity portion
- Added `desiredLtvPercent` to FinancingResult for display
- Updated analysis pipeline: property override → profile default → 80% fallback
- Added LTV field to investor profile form (0–80%, step 5)
- Added LTV override field to property forms (new + edit), optional
- Updated validation schemas with FINMA-compliant 0–80% range
- Added EN/DE translations for LTV fields
- 388 tests passing (7 new tests)

---

### Epic 10: Automatic Zoning & Buildability Data from Address

**Goal:** Automatically detect zoning type AND buildability parameters (actual FAR, max stories, noise sensitivity, heritage protection) when a property address is entered. All auto-detected data is **used in the development potential calculation** — not just stored.

**Current limitation:** The calculator uses only 3 inputs (plotArea, zoningType, existingBuiltArea) with hardcoded generic FAR values. The `maxFloors` field exists in constants but is never used.

| #     | Task                                                                                                                                     | Status | Tests |
| ----- | ---------------------------------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| 10.1  | Schema: add buildability fields (lat/lng, noiseSensitivity, hasHeritageProtection, actualFar, maxFloors, maxBuildingHeightM) + migration | [x]    | —     |
| 10.2  | Extract lat/lng from geo.admin.ch address search response (already in API attrs)                                                         | [x]    | —     |
| 10.3  | Zoning lookup API route `/api/zoning-lookup` with geodienste.ch OGC integration (~18 cantons)                                            | [x]    | 11/11 |
| 10.4  | Canton Zurich WFS integration (maps.zh.ch) for ZH properties                                                                             | [x]    | 11/11 |
| 10.5  | GeoAdmin ch.are.bauzonen fallback for remaining cantons (GL, GR, TI, OW, NW, VD)                                                         | [x]    | 11/11 |
| 10.6  | Zone name parser: extract FAR, stories, height from cantonal zone descriptions                                                           | [x]    | 42/42 |
| 10.7  | geodienste.ch noise sensitivity (Lärmempfindlichkeitsstufen) + overlapping zones queries                                                 | [x]    | 11/11 |
| 10.8  | Enhanced calculator: use actual FAR, maxFloors, noise, heritage in development potential calc                                            | [x]    | 63/63 |
| 10.9  | Pipeline integration: pass new fields through analysis service → engine → calculator                                                     | [x]    | —     |
| 10.10 | Property form: trigger zoning lookup on address select, auto-fill all buildability fields                                                | [x]    | —     |
| 10.11 | Results display: show stories, footprint/floor, noise, heritage, data source in results card                                             | [x]    | —     |
| 10.12 | EN/DE translations for all new labels (zoning hints, noise, heritage, data source)                                                       | [x]    | —     |
| 10.13 | Tests: zoning parser, lookup API, enhanced calculator with new parameters                                                                | [x]    | 73/73 |

**Architecture:**

```
Address selected (AddressAutocomplete onSelect)
  → extract lat/lng from geo.admin.ch response (already in attrs)
  → store lat/lng on Property
  → trigger GET /api/zoning-lookup?lat=...&lng=...&canton=...
      → if canton on geodienste.ch (~18 cantons):
          query 3 collections in parallel:
            1. grundnutzung → zone type, cantonal name → parse FAR/stories from name
            2. laermempfindlichkeitsstufen → noise sensitivity (ES_I–ES_IV)
            3. ueberlagernde_nutzung → heritage protection, other overlays
      → else if canton = ZH:
          query maps.zh.ch WFS → typ_gde_abkuerzung, vollgeschosse_max
      → else (GL, GR, TI, OW, NW, VD):
          query GeoAdmin ch.are.bauzonen → broad category only
      → parse zone name for embedded FAR/stories/height
      → return structured ZoningLookupResult
  → auto-fill: zoningType, actualFar, maxFloors, noiseSensitivity, hasHeritageProtection
  → at analysis time: calculator uses actual values when available, falls back to generic table
```

**New DB fields on Property:**

| Field                   | Type           | Purpose                                     |
| ----------------------- | -------------- | ------------------------------------------- |
| `latitude`              | Decimal(10,7)? | For zoning API queries                      |
| `longitude`             | Decimal(10,7)? | For zoning API queries                      |
| `noiseSensitivity`      | String(5)?     | ES_I, ES_II, ES_III, ES_IV                  |
| `hasHeritageProtection` | Boolean?       | From overlapping zones                      |
| `actualFar`             | Decimal(5,3)?  | Municipality-specific AZ from API/zone name |
| `maxFloors`             | Int?           | From API (e.g. ZH vollgeschosse_max)        |
| `maxBuildingHeightM`    | Decimal(5,1)?  | If parseable from zone name                 |

**How new data affects the calculation:**

| Parameter               | Effect on Calculator                                                           |
| ----------------------- | ------------------------------------------------------------------------------ |
| `actualFar`             | Overrides generic ZONING_FAR table — uses real municipality AZ with ±10% range |
| `maxFloors`             | Overrides generic maxFloors — enables per-floor footprint calculation          |
| `noiseSensitivity`      | ES_III/IV + residential zone → raises permitRisk to HIGH                       |
| `hasHeritageProtection` | Forces renovationVsRebuild=RENOVATE, planningComplexity=HIGH, permitRisk=HIGH  |
| `maxBuildingHeightM`    | Stored for display; future: constrain buildable volume                         |

**Data sources (all free, no auth):**

| Source                   | Coverage       | Provides                                                          | Endpoint                                                                           |
| ------------------------ | -------------- | ----------------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| geodienste.ch OGC API    | ~18 cantons    | Zone type + name, noise sensitivity, overlapping zones (heritage) | `grundnutzung`, `laermempfindlichkeitsstufen`, `ueberlagernde_nutzung` collections |
| Canton Zurich WFS        | ZH only        | Zone abbrev + `vollgeschosse_max`                                 | `maps.zh.ch/wfs/OGDZHWFS` ogd-0156 layer                                           |
| GeoAdmin ch.are.bauzonen | All 26 cantons | 9 broad categories (fallback)                                     | `api3.geo.admin.ch/rest/services/api/MapServer/identify`                           |

**Zone name parsing patterns:**

| Value   | Regex Pattern                                   | Example              |
| ------- | ----------------------------------------------- | -------------------- |
| FAR     | `/AZ\s*[\=:]?\s*(\d+[.,]\d+)/i`                 | "AZ 0.6" → 0.6       |
| Stories | `/(\d+)[\s-]*geschossig/i` or `/W(\d)/`         | "dreigeschossig" → 3 |
| Height  | `/(\d+(?:[.,]\d+)?)\s*m.*(?:Höhe\|hoch\|max)/i` | "max. 12m" → 12      |

**Zoning type mapping rules:**

| Our Type       | Detection Pattern                                       |
| -------------- | ------------------------------------------------------- |
| W2–W5          | "Wohnzone" + story count from name/abbreviation         |
| WG2–WG4        | "Wohn-/Gewerbezone" or "Gewerbe" + "Wohn" + story count |
| K              | "Kernzone" or abbreviation starts with "K"              |
| I              | "Industriezone" or "Industrie"                          |
| G              | "Gewerbezone" (not "Wohn-/Gewerbe")                     |
| OE             | "öffentliche Bauten"                                    |
| LANDWIRTSCHAFT | "Landwirtschaft"                                        |

**GeoAdmin fallback mapping:**

| `ch_code_hn`                 | Maps to | Notes                        |
| ---------------------------- | ------- | ---------------------------- |
| 11 (Wohnzonen)               | W3      | Safe middle (no story count) |
| 12 (Arbeitszonen)            | I       | Industry/employment          |
| 13 (Mischzonen)              | WG3     | Safe middle                  |
| 14 (Zentrumszonen)           | K       | Center ≈ Kernzone            |
| 15 (Öffentliche Nutzungen)   | OE      | Direct mapping               |
| 16 (Eingeschränkte Bauzonen) | OTHER   | Restricted zones             |

**Key challenges:**

- Cantonal heterogeneity: each canton names zones differently; regex pattern matching needed
- Zurich (ZH) requires separate WFS integration (not on geodienste.ch)
- Cantons GL, GR, TI, OW, NW, VD not on geodienste.ch → GeoAdmin fallback (broad category only)
- Coordinate conversion: geodienste.ch accepts WGS84, Zurich WFS requires LV95 (EPSG:2056)
- Zone name parsing is inherently fuzzy — some cantons embed FAR in name, others don't
- Heritage detection from overlapping zones requires keyword matching ("Denkmalschutz", "Ortsbildschutz", etc.)

**What was done:**

- Added 7 new buildability fields to Property model (latitude, longitude, noiseSensitivity, hasHeritageProtection, actualFar, maxFloors, maxBuildingHeightM) + migration
- Extended `ParsedAddress` to include lat/lng from geo.admin.ch response
- Created `app/lib/analysis/zoning-parser.server.ts` — extracts FAR, stories, height from cantonal zone names using regex patterns; maps zone names to our ZoningType enum
- Created `app/routes/api/zoning-lookup.ts` — cascading API integration: ZH → maps.zh.ch WFS, ~18 cantons → geodienste.ch OGC API (3 collections in parallel), remaining → GeoAdmin fallback
- Enhanced development potential calculator: uses actual FAR from API (±10% range, falls back to generic table), actual maxFloors from API, heritage protection forces RENOVATE + HIGH complexity/risk, noise ES_III/IV raises permit risk for residential zones
- Updated full pipeline: analysis service → engine → calculator passes all new fields
- Property forms (new + edit): selecting an address triggers `/api/zoning-lookup`, auto-fills zoningType select + all buildability hidden fields; shows status badges (loading/detected/unavailable)
- Added validation schema fields for all buildability data (latitude, longitude, noiseSensitivity, hasHeritageProtection, actualFar, maxFloors, maxBuildingHeightM)
- Updated development potential results card: heritage warning, noise warning, max floors, footprint/floor, noise sensitivity, heritage status, data source (api vs generic)
- Added EN/DE translations for all new labels (15+ new keys across analysis.json and property.json)
- 461 tests passing (73 new tests: 42 zoning parser, 11 zoning lookup API, 20 enhanced calculator)

---

### Epic 11: Investment Suggestion (Optimal Portfolio Selection)

**Goal:** Given the user's saved & analyzed properties, suggest the best subset to purchase that maximizes portfolio quality while respecting equity budget and FINMA affordability constraints. Shows comparison scenarios (buy top 1, 2, 3…) and a sensitivity note ("with CHF X more, you could also add…").

**Scope (Phase 1):** Saved properties only. No manual pin/exclude. Respects per-property LTV overrides.

| #     | Task                                                                                                            | Status | Tests |
| ----- | --------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| 11.1  | Portfolio optimizer: `optimizePortfolio(candidates, constraints)` — greedy knapsack by score, equity tiebreaker | [x]    | 24/24 |
| 11.2  | Multi-scenario comparison: generate "buy top N" scenarios (N=1…max affordable) with combined metrics            | [x]    | 24/24 |
| 11.3  | Sensitivity analysis: "with CHF X more equity, you could also add property Y" (max 5, sorted by cheapest)       | [x]    | 24/24 |
| 11.4  | Service: `listPropertiesForSuggestion()` — fetch properties with latest analysis + financing/investment results | [x]    | —     |
| 11.5  | Route + loader: `/app/properties/suggest` — fetch profile + analyzed properties, run optimizer, return results  | [x]    | —     |
| 11.6  | UI: suggestion page — profile summary, recommended portfolio, scenario comparison, sensitivity cards            | [x]    | —     |
| 11.7  | "Suggest Investment" button on Properties list page (disabled if no profile)                                    | [x]    | —     |
| 11.8  | Info banner: "N properties have no analysis and will not be included in the suggestion"                         | [x]    | —     |
| 11.9  | EN/DE translations for all suggestion UI labels                                                                 | [x]    | —     |
| 11.10 | Tests: optimizer (greedy selection, budget constraint, affordability constraint, DSCR filter, edge cases)       | [x]    | 24/24 |

**Algorithm:**

- **Filter**: exclude properties without analyses or that individually exceed remaining equity
- **Rank**: sort by strategy-specific efficiency metric (score/equity for BUY_AND_HOLD, yield/equity for YIELD, etc.)
- **Greedy select**: pick top-ranked properties one by one until budget or affordability is exhausted
- **Constraints**: total equity used ≤ `availableCash`; for natural persons: cumulative stress-test monthly ≤ 33% of `monthlyIncome`; for companies: each property DSCR ≥ 1.0
- **Per-property LTV**: uses property's `desiredLtvPercent` override if set, otherwise profile's `defaultLtvPercent`

**Scenarios output:**

| Scenario  | Shows                                                                    |
| --------- | ------------------------------------------------------------------------ |
| Buy top 1 | Best single property — score, yield, equity needed, monthly cost         |
| Buy top 2 | Best pair — combined metrics, diversification benefit                    |
| Buy top N | Up to max affordable — total equity, combined yield, affordability ratio |

**Sensitivity note:** After selecting optimal set, check: what's the cheapest excluded property? Report: "With CHF {delta} more equity, you could also add {property name} (score: {X})."

**UI sections:**

1. Budget summary bar (available / allocated / remaining equity)
2. Recommended portfolio (selected property cards with green highlight)
3. Portfolio metrics (total equity, combined yield, weighted avg score, affordability ratio)
4. Scenario comparison table (buy 1 vs 2 vs 3…)
5. Sensitivity note callout
6. Info banner for unanalyzed properties count
7. Excluded properties (collapsible, with skip reason per property)

---

### Backlog — Future Improvements

| #   | Task                                                                                                                               | Priority |
| --- | ---------------------------------------------------------------------------------------------------------------------------------- | -------- |
| B.1 | Commercial municipality-level price/rent data via Wüest Partner or IAZI API integration                                            | High     |
| B.2 | Property-type granularity (luxury, new-build, etc.) for more accurate benchmarks                                                   | Medium   |
| B.3 | ~~Integration with listing data from platforms like Homegate/ImmoScout24~~ → Moved to P2-E1                                        | —        |
| B.4 | Admin panel: dashboard for import triggers, import history/stats, listing management (delist/flag), user management, system health | Medium   |

---

## Phase 2 — Smart Deal Discovery (Prioritization at Scale)

**Goal:** Move from analyzing one deal to prioritizing the right deals and reducing search time.

**Success signals:**

- Users report: "The platform finds deals we would have missed."
- Increased deal throughput per acquisition team

### Epics

| #     | Epic                                                                     | Status  |
| ----- | ------------------------------------------------------------------------ | ------- |
| P2-E1 | Data Intake Pipeline (listing normalization, de-duplication, enrichment) | Done    |
| P2-E2 | Automated Listing Collection (scrape/API from accessible portals)        | Done    |
| P2-E3 | AI-Driven Deal Prioritization (shortlist, fit-scoring, alerts)           | Active  |
| P2-E4 | Hedonic Property Valuation & Niederstwertprinzip                         | Planned |
| P2-E5 | Invite-Only Agent/Supplier Access Portal                                 | Done    |
| P2-E6 | Controlled Off-Market Opportunity Submissions                            | Planned |
| P2-E7 | Investor Listing Submissions (Sell Properties to Other Investors)        | Done    |

---

### P2-E1: Data Intake Pipeline — Detailed Tasks

#### Listing Import (URL paste + Screenshot fallback → Prefilled Property Form)

**Goal:** User pastes a listing URL — the system fetches the page server-side and uses Claude Haiku to extract property data and pre-fill the form. If the fetch is blocked (403), the user is prompted to upload a screenshot of the listing, which Claude Haiku reads via vision to extract the same fields.

| #       | Task                                                                                          | Status | Tests |
| ------- | --------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-1.1  | Schema: add `SCREENSHOT_IMPORT` to PropertySource enum + migration                            | [x]    | —     |
| P2-1.2  | LLM extraction service: text/HTML → structured property data (Claude Haiku API)               | [x]    | 37/37 |
| P2-1.3  | LLM vision extraction service: screenshot image → structured property data (Claude Haiku API) | [x]    | 37/37 |
| P2-1.4  | Server-side fetch service: fetch URL, strip scripts/CSS/SVG, return clean text for LLM        | [x]    | 37/37 |
| P2-1.5  | API route `POST /api/import-listing`: accepts URL, fetches page, runs LLM extraction          | [x]    | 9/9   |
| P2-1.6  | API route `POST /api/import-screenshot`: accepts image upload, runs LLM vision extraction     | [x]    | 9/9   |
| P2-1.7  | Import UI on property creation page: URL paste field + "Import" button                        | [x]    | —     |
| P2-1.8  | Screenshot upload UI: shown when URL fetch fails (403), drag-and-drop + file picker           | [x]    | —     |
| P2-1.9  | Pre-fill UX: populate form fields from extraction result, highlight auto-filled fields        | [x]    | —     |
| P2-1.10 | EN/DE translations for import UI (URL input, screenshot upload, status messages, errors)      | [x]    | —     |
| P2-1.11 | Unit + integration tests for fetch service, LLM extraction, vision extraction, API routes     | [x]    | 55/55 |

**What was done in P2-E1:**

- Installed `@anthropic-ai/sdk` for Claude Haiku integration
- Added `SCREENSHOT_IMPORT` to PropertySource enum + migration
- Created `app/lib/services/listing-import.server.ts` — core extraction service:
  - `cleanHtmlForExtraction`: strips script/style/svg/nav/footer/header tags, decodes entities (including German umlauts), collapses whitespace, truncates to 15K chars
  - `fetchListingPage`: 10s timeout, returns `{ blocked: true }` for 403/429
  - `extractListingFromText`: sends cleaned text to Claude Haiku, parses JSON response
  - `extractListingFromScreenshot`: sends base64 image to Claude Haiku vision, parses JSON response
  - `validateExtractedData`: validates cantons, property types, conditions, PLZ format, year ranges, positive numbers
- Created `app/routes/api/import-listing.ts` — POST action, validates URL, fetches page, returns extracted data or blocked status
- Created `app/routes/api/import-screenshot.ts` — POST action, validates image file (type, size), converts to base64, returns extracted data
- Created `app/components/property/listing-import.tsx` — self-contained UI component with state machine (idle/fetching/blocked/uploading/extracted/error), two `useFetcher` instances
- Integrated into both `new.tsx` and `edit.tsx` property forms with auto-fill (DOM manipulation + controlled Select states), auto-fill badges (Sparkles icon), source tracking hidden fields
- Added `ANTHROPIC_API_KEY` to `.env.example`
- Added EN/DE translations (~20 keys each for import UI)
- Extended `createProperty` to accept optional `source`/`sourceUrl`
- 516 tests passing (55 new tests: 37 service, 9 import-listing API, 9 import-screenshot API)

**Architecture:**

```
User pastes URL → POST /api/import-listing
  → Server fetches URL (standard HTTP)
  → If 200: strip scripts/CSS/SVG → send clean text to Claude Haiku → structured fields
  → If 403/blocked: return { blocked: true } to frontend
      → Frontend shows: "This site blocks automated access. Please upload a screenshot."
      → User uploads screenshot → POST /api/import-screenshot
      → Send image to Claude Haiku (vision) → structured fields
  → Pre-fill property form with extracted data
  → Store sourceUrl + importSource ("URL_IMPORT" | "SCREENSHOT_IMPORT") on property
```

**LLM extraction (Claude Haiku — `claude-haiku-4-5-20251001`):**

Both text and vision extraction use the same output schema. The LLM is prompted to extract these fields from whatever content it receives (HTML text or screenshot image):

| Target Field       | Description                                                 |
| ------------------ | ----------------------------------------------------------- |
| title              | Listing title                                               |
| address            | Full street address                                         |
| postalCode         | Swiss PLZ (4 digits)                                        |
| city               | City / municipality name                                    |
| canton             | 2-letter canton code                                        |
| purchasePrice      | Purchase price in CHF                                       |
| livingAreaSqm      | Living area in m²                                           |
| plotAreaSqm        | Plot / land area in m²                                      |
| rooms              | Number of rooms                                             |
| yearBuilt          | Year of construction                                        |
| yearLastRenovation | Year of last renovation                                     |
| propertyType       | APARTMENT, HOUSE, MULTI_FAMILY, COMMERCIAL, LAND, MIXED_USE |
| condition          | NEW, GOOD, FAIR, RENOVATION_NEEDED, UNKNOWN                 |
| currentMonthlyRent | Current monthly rent (if rental property or investment)     |
| description        | Property description text                                   |

**Accessible vs blocked portals (research March 2026):**

| Works (server-side fetch) | Blocked (403 / bot protection)         |
| ------------------------- | -------------------------------------- |
| Flatfox.ch                | Homegate (DataDome + Cloudflare)       |
| alle-immobilien.ch        | ImmoScout24.ch (DataDome + Cloudflare) |
| iCasa.ch                  | newhome.ch                             |
| immobilier.ch             |                                        |
| Comparis (detail pages)   | Comparis (search pages — DataDome)     |

**Future fallback options (backlog):**

| Approach                      | Notes                                                                               |
| ----------------------------- | ----------------------------------------------------------------------------------- |
| Bookmarklet                   | Runs as user on listing page, bypasses all protection. Best UX after initial setup. |
| Browser extension             | Same as bookmarklet but persistent. Requires Chrome Web Store publishing.           |
| Headless browser (Playwright) | Automated but heavy (~130MB), fragile against fingerprinting.                       |

---

### P2-E2: Automated Listing Collection — Research & Tasks

**Goal:** Automatically collect and ingest property listings from Swiss portals that allow server-side access. Start with Flatfox + alle-immobilien.ch only, validate the pipeline, then expand to other portals.

**Scope (Phase 2 MVP):** Flatfox + alle-immobilien.ch only. Limited to latest 30 listings per portal (configurable) to control costs on test/staging. Delta import — only import new listings since last sync.

#### Portal Accessibility Research (March 2026)

**Phase 2 MVP portals:**

| Portal                 | Listings | Method                                                          | Notes                                                                                                                                                                                           |
| ---------------------- | -------- | --------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Flatfox.ch**         | ~34,000  | **Public REST API** (`/api/v1/public-listing/?offer_type=SALE`) | Offset pagination, `ordering=-created` for newest first, rich JSON (price, address, rooms, area, lat/lng, images). No auth.                                                                     |
| **alle-immobilien.ch** | ~40,000  | Server-rendered HTML                                            | `?sort=dd&pageNum=N`, 20 items/page. Uses `api.homegate.ch` backend (403 for direct API). **robots.txt: Crawl-delay: 600** (10 min between requests). No structured data — HTML parsing needed. |

**Deferred portals (expand after MVP validation):**

| Portal        | Listings       | Method                                    | Notes                                        |
| ------------- | -------------- | ----------------------------------------- | -------------------------------------------- |
| iCasa.ch      | ~8,800         | JSON in HTML (`window.archiveJsResponse`) | Independent portal. Clean data.              |
| immobilier.ch | Suisse Romande | Server-rendered HTML                      | French-speaking CH only. Schema.org in HTML. |

**Blocked portals (manual URL/screenshot import via P2-E1 only):**

| Portal         | Protection            | Notes                                                                                                                                                  |
| -------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Homegate       | DataDome + Cloudflare | #1/#2 portal. No public read API.                                                                                                                      |
| ImmoScout24.ch | DataDome + Cloudflare | #1/#2 portal. No public read API.                                                                                                                      |
| Comparis       | DataDome              | Detail pages accessible (relaxed rules for SEO), search pages blocked. Works for P2-E1 manual URL import but unreliable for automated bulk collection. |
| newhome.ch     | 403                   | Cantonal banks consortium.                                                                                                                             |

**Key insight:** SMG Swiss Marketplace Group owns most Swiss RE portals (Homegate, ImmoScout24, Flatfox, alle-immobilien, ImmoStreet, home.ch). Many share the same `api.homegate.ch` backend. alle-immobilien.ch listings link to homegate.ch detail pages with the same listing IDs.

#### Flatfox API Details

```
GET https://flatfox.ch/api/v1/public-listing/?offer_type=SALE&ordering=-created&limit=30

Response: {
  "count": 34000,
  "next": "https://flatfox.ch/api/v1/public-listing/?...&offset=30",
  "previous": null,
  "results": [{
    "pk": 1234567,
    "slug": "...",
    "offer_type": "SALE",
    "price_display": 750000,
    "number_of_rooms": 3.5,
    "surface_living": 92,
    "surface_property": 200,
    "street": "Multenrain 10",
    "zipcode": "5037",
    "city": "Muhen",
    "state": "AG",
    "latitude": 47.33,
    "longitude": 8.05,
    "year_built": 1990,
    "year_renovated": 2018,
    "object_category": "APPT",
    "created": "2026-03-05T10:00:00+01:00",
    "description_title": "...",
    "description": "...",
    ...
  }]
}
```

Delta import: use `ordering=-created`, store last sync timestamp, stop when encountering older listings.

#### alle-immobilien.ch Scraping Details

```
GET https://www.alle-immobilien.ch/de/kaufen?sort=dd&pageNum=1

- 20 listings per page, server-rendered HTML
- No JSON-LD or Schema.org structured data
- Each listing in `.item` div with: title, address, price, rooms, area, image, detail link
- Detail links point to homegate.ch: https://www.homegate.ch/kaufen/{listingId}
- Listing IDs: 10-digit numbers (same as Homegate IDs)
- robots.txt: Crawl-delay: 600 (10 min between requests!)
- For 30 listings: 2 pages = ~10 min total
```

#### Deduplication Strategy

**Problem 1 — Same-portal re-import (delta):**

- Track `externalListingId` per portal (e.g., `flatfox:1234567`, `alle-immobilien:4002986195`)
- On re-run, skip listings whose external ID already exists
- Store `lastSyncedAt` per portal to know where to resume

**Problem 2 — Cross-portal duplicates (same property on Flatfox + alle-immobilien):**

- Stage 1: Block by postal code (narrow comparison set)
- Stage 2: Fuzzy address match within same PLZ (normalized street + number)
- Stage 3: Confirm with attribute similarity (price ±10%, area ±10%, rooms match)
- If duplicate found: link to existing property, log the additional source
- Conservative: only auto-merge at high confidence, flag uncertain matches

**Schema additions:**

- `externalListingId` (String, unique per portal) on imported properties
- `importedFrom` (String: "flatfox", "alle-immobilien") for portal tracking
- `lastSyncedAt` per portal (separate sync tracking table or config)

#### Tasks

| #      | Task                                                                                                        | Status | Tests   |
| ------ | ----------------------------------------------------------------------------------------------------------- | ------ | ------- |
| P2-2.1 | Schema: add listing import tracking fields (externalListingId, importedFrom, ImportSync table) + migration  | [x]    | —       |
| P2-2.2 | Flatfox connector: fetch SALE listings via API, normalize to unified schema, respect 30-listing limit       | [x]    | 11/11   |
| P2-2.3 | alle-immobilien.ch connector: fetch+parse HTML, normalize to unified schema, respect crawl-delay + 30 limit | [x]    | 12/12   |
| P2-2.4 | Listing normalization service: map portal-specific fields → Property creation data                          | [x]    | 28/28   |
| P2-2.5 | Delta import: track last sync per portal, only import new listings (by externalListingId + timestamp)       | [x]    | 11/11   |
| P2-2.6 | Cross-portal deduplication: PLZ blocking + address similarity + attribute confirmation                      | [x]    | 29/29   |
| P2-2.7 | Import trigger UI: button to run import per portal, show progress/results, listing count config             | [x]    | —       |
| P2-2.8 | EN/DE translations for import collection UI                                                                 | [x]    | —       |
| P2-2.9 | Unit + integration tests for connectors, normalization, delta import, deduplication                         | [x]    | 104/104 |

**What was done in P2-E2:**

- Added `PORTAL_IMPORT` to PropertySource enum, `externalListingId`/`importedFrom` to Property model (with unique constraint), `ImportSync` model for per-portal sync tracking. Migration `add_portal_import_tracking`.
- Created `app/lib/services/portal-listing.server.ts` — unified `PortalListing` interface, `normalizeAddress` (Swiss abbreviations), `mapPropertyType` (Flatfox/German types → enum), `normalizeToPropertyInput` (validates required fields, returns null if incomplete)
- Created `app/lib/services/connectors/flatfox.server.ts` — fetches Flatfox REST API (`/api/v1/public-listing/?offer_type=SALE`), maps JSON fields to PortalListing, delta import via `lastSyncedAt` timestamp, canton name→code mapping, 10s timeout
- Created `app/lib/services/connectors/alle-immobilien.server.ts` — HTML scraping with regex-based parsing (`parseSearchPage`), Swiss price format handling (CHF 1'250'000.–), canton lookup via SwissPostalCode table, configurable crawl delay (`ALLE_IMMOBILIEN_CRAWL_DELAY_MS`, default 5000ms), `guessPropertyType` from listing titles
- Created `app/lib/services/deduplication.server.ts` — 3-stage algorithm: PLZ blocking (single DB query), address similarity (Levenshtein-based with normalization shortcuts), attribute confirmation (price ±10%, area ±10%, rooms exact). Confidence levels: high/medium/low
- Created `app/lib/services/listing-collection.server.ts` — orchestrator: upserts ImportSync as in_progress, fetches from connector, delta check via existing externalListingId Set, normalizes each listing, dedup check, calls createProperty, updates ImportSync with final counts
- Created `app/routes/api/portal-import.ts` — POST triggers import (validates portal + limit 1-100), GET returns sync history
- Created `app/routes/app/import.tsx` — Import page with listing limit config (1-100, default 30), grid of PortalImportCard components
- Created `app/components/import/portal-import-card.tsx` — portal card with name/description, last sync time, import button via useFetcher, success/error result display
- Extended `createProperty` in property.server.ts with `externalListingId`/`importedFrom` options
- Added sidebar "Import" nav item with Download icon
- Added EN/DE translations (~15 keys each in import.json + nav.import in common.json)
- Added `IMPORT_LISTING_LIMIT` and `ALLE_IMMOBILIEN_CRAWL_DELAY_MS` env vars
- 620 tests passing (104 new: 28 normalization, 11 flatfox connector, 12 alle-immobilien connector, 29 deduplication, 11 orchestrator, 13 API route)

**Deferred tasks (expand after MVP validation):**

| #       | Task                                                                  | Status   | Notes                                            |
| ------- | --------------------------------------------------------------------- | -------- | ------------------------------------------------ |
| P2-2.10 | iCasa.ch connector                                                    | Deferred | Expand after Flatfox + alle-immobilien validated |
| P2-2.11 | immobilier.ch connector                                               | Deferred | French-speaking CH only                          |
| P2-2.12 | Scheduled ingestion: cron/background job to poll portals periodically | Deferred | After manual trigger works                       |
| P2-2.13 | Ingestion dashboard: collected listings, source, status, last sync    | Deferred | After basic import UI works                      |

---

### P2-E3: AI-Driven Deal Prioritization — Detailed Tasks

#### Architecture: Global Listings + Per-User Analysis

**Problem:** Currently all imported listings are stored as per-user `Property` records. This causes duplication across users, prevents global discovery, and blocks AI-driven deal ranking. The roadmap requires a shared listing pool that the system scores against each investor's profile.

**Solution:** Separate `Listing` (global, system-level) from `Property` (user-owned, for analysis).

```
Listing (global, shared)
├── id
├── source (FLATFOX, ALLE_IMMOBILIEN, AGENT, OFF_MARKET)
├── externalId (unique per source)
├── submittedByUserId?      ← for P2-E4 agent submissions
├── status (ACTIVE, DELISTED, SOLD)
├── all property data fields (address, price, rooms, canton, PLZ, etc.)
├── condition                ← estimated from yearBuilt/yearRenovated (see heuristic below)
├── lastSeenAt               ← for freshness tracking
└── createdAt

Property (user-owned, for analysis)
├── id
├── userId
├── sourceListingId? → Listing  ← linked if from global pool
├── source (MANUAL, URL_IMPORT, SCREENSHOT_IMPORT, FROM_LISTING)
├── all property data fields     ← copied from Listing or entered manually
├── user can freely edit
└── Analysis[] runs against this

ListingScore (pre-computed ranking cache)
├── listingId → Listing
├── userId → User
├── overallScore (0–100)
├── categoryScores JSON (financing, investment, risk, location)
├── keyWarnings JSON
├── computedAt
└── unique(listingId, userId)
```

**Flow:**

1. System imports → creates/updates `Listing` records (one copy per real-world property)
2. Condition heuristic estimates condition from yearBuilt/yearRenovated at normalization time
3. For each new Listing, run full analysis engine against all matching investor profiles → store `ListingScore`
4. Investor opens "Discover" → sees listings ranked by their personal score
5. Investor clicks "Save for analysis" → creates `Property` copy linked to `Listing`
6. Investor edits `Property` freely, runs full analysis → stored as `Analysis` (same as today)
7. Manual property creation works exactly as today (no `sourceListingId`)

**Condition estimation heuristic (deterministic, at normalization time):**

```
If yearRenovated exists and < 5 years ago  → GOOD
If yearRenovated exists and < 15 years ago → FAIR
If yearBuilt > 2020                        → NEW
If yearBuilt > 2000                        → GOOD
If yearBuilt > 1980                        → FAIR
If yearBuilt <= 1980                       → RENOVATION_NEEDED
No year data                               → UNKNOWN
```

**Why run the full analysis engine (not just basic filtering):**

- The analysis engine is deterministic and fast — pure math, no LLM calls, ~50ms per property
- 200 listings × 50 profiles = 10,000 scores — ~8 min total, easily parallelizable
- Incremental: only re-score when a new listing arrives or a profile changes
- This IS the differentiator: "scores 82/100 because yield is above canton average, financing fits your cash, vacancy risk is low" — not just "it's in Zurich under 2M"

**What changes from P2-E2:**

| Concern           | P2-E2 (current)             | P2-E3 (new)                                         |
| ----------------- | --------------------------- | --------------------------------------------------- |
| Portal import     | Creates `Property` per user | Creates `Listing` (global, one copy)                |
| Import trigger    | Each user clicks "Import"   | System-level (admin or cron)                        |
| Deduplication     | Per-user fuzzy matching     | Global, simpler (unique `externalId + source`)      |
| User's properties | Mix of imported + manual    | Only their own: manual + saved listings             |
| Browse/discover   | Not possible                | Users browse global `Listing` pool, ranked by score |
| Analysis          | Runs on `Property`          | Still runs on `Property` (unchanged)                |

#### Tasks

| #        | Task                                                                                                           | Status | Tests |
| -------- | -------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-3.1   | Schema: Create `Listing` model (source, externalId, status, all property fields, lastSeenAt) + migration       | [x]    | —     |
| P2-3.2   | Schema: Add `sourceListingId` FK to `Property` + `FROM_LISTING` to PropertySource enum + migration             | [x]    | —     |
| P2-3.3   | Schema: Create `ListingScore` model (listingId, userId, scores, warnings, computedAt) + migration              | [x]    | —     |
| P2-3.4   | Condition heuristic: estimate condition from yearBuilt/yearRenovated during listing normalization              | [x]    | 10/10 |
| P2-3.5   | Refactor portal import pipeline: connectors create/update `Listing` instead of per-user `Property`             | [x]    | 10/10 |
| P2-3.6   | Global deduplication: cross-portal dedup on `Listing` table (unique externalId+source, PLZ+address similarity) | [x]    | 11/11 |
| P2-3.7   | Listing service: CRUD for global listings (create, findById, list with filters, updateStatus)                  | [x]    | 51/51 |
| P2-3.8   | Shadow analysis service: run analysis engine on Listing × investor profile, store as `ListingScore`            | [x]    | 11/11 |
| P2-3.9   | Batch scoring: on new listing score against matching profiles; on profile change re-score relevant listings    | [x]    | —     |
| P2-3.10  | "Save for analysis": create `Property` copy from `Listing` with `sourceListingId` link                         | [x]    | —     |
| P2-3.11  | "Discover" page: browse listings ranked by personal score, filter by canton/type/price range                   | [x]    | —     |
| P2-3.12  | Listing detail view: score breakdown, key metrics, warnings, "Save for analysis" button                        | [x]    | —     |
| P2-3.13  | Listing freshness: track `lastSeenAt`, mark `DELISTED` when no longer found on re-import                       | [x]    | —     |
| P2-3.14  | Migrate existing imported properties: backfill `Listing` records from existing portal-imported `Property` data | [x]    | —     |
| P2-3.15a | Admin role: add `role` to User model, `requireAdmin` helper, protect import route behind admin check           | [x]    | 14/14 |
| P2-3.15b | Cron import endpoint: stateless key-authenticated route that runs all portal imports (Railway cron-ready)      | [ ]    | —     |
| P2-3.16  | EN/DE translations for Discover page, listing cards, score display, save actions                               | [x]    | —     |
| P2-3.17  | Unit + integration tests for listing service, shadow analysis, batch scoring, save-for-analysis, Discover API  | [x]    | 72/72 |

---

### P2-E4: Hedonic Property Valuation & Niederstwertprinzip (PriceHubble)

**Goal:** Integrate PriceHubble's hedonic valuation API to model how banks estimate property value. Swiss banks apply the **Niederstwertprinzip** (lower value principle): LTV is calculated against `min(purchasePrice, bankValuation)`. Currently the platform always uses `purchasePrice` as LTV basis, which is optimistic for overpriced properties.

**Why it matters:**

- For below-market properties (purchase < valuation): no impact, bank uses purchase price — but surfacing the gap validates the deal
- For above-market properties (purchase > valuation): bank caps the mortgage at their lower valuation, requiring more equity than expected — currently not modeled
- Accurate bank valuation estimates improve DSCR, equity, and affordability calculations
- Hedonic rent estimates replace our rough cantonal averages with micro-location-aware figures

**Provider: PriceHubble** (pricehubble.com)

- REST API at `https://api.pricehubble.com/api/v1`
- Auth: username/password → bearer token (12h TTL), credentials from Account Manager (no self-service)
- Covers Switzerland as first-class market, includes location scores (scores only available for `countryCode: "CH"`)
- No official JS/TS SDK — build thin REST client
- Pricing: enterprise/B2B, negotiated with sales team
- **Credentials obtained** — env vars: `PRICEHUBBLE_USERNAME`, `PRICEHUBBLE_PASSWORD`

**Scope: We only use the Valuation API** (`POST /valuation/property_value`). No market/offer_search or other endpoints.

**Authentication (docs: https://docs.pricehubble.com/international/authentication/)**

- Endpoint: `POST https://api.pricehubble.com/auth/login/credentials`
- Request: `{ "username": "string", "password": "string" }`
- Response: `{ "access_token": "string", "expires_in": 43200 }` (12h = 43200s)
- Usage: `Authorization: Bearer <access_token>` header on all subsequent requests
- No refresh endpoint — re-authenticate after expiry
- **WARNING:** Excessive auth requests trigger a temporary ban. Cache the token and only re-auth on expiry or 401.

**Valuation endpoint (docs: https://docs.pricehubble.com/international/valuation/)**

- Endpoint: `POST https://api.pricehubble.com/api/v1/valuation/property_value`
- Header: `Content-Type: application/json`, `Authorization: Bearer <token>`
- Batch: up to 50 properties per call, up to 50 dates per call (but not 50×50)
- Clients MUST be tolerant of additional response fields (backward compat: new fields may appear)

**Request format:**

```json
{
  "dealType": "sale" | "rent",
  "countryCode": "CH",
  "valuationInputs": [
    {
      "property": {
        "location": {
          "address": { "postCode": "8001", "city": "Zürich", "street": "Bahnhofstrasse", "houseNumber": "1" }
          // OR "coordinates": { "latitude": 47.3769, "longitude": 8.5417 }
        },
        "propertyType": { "code": "apartment" | "house" | "multi_family_house", "subcode": "apartment_normal" },
        "buildingYear": 1990,
        "livingArea": 120,
        "numberOfRooms": 4.5,
        "numberOfBathrooms": 2,
        "balconyArea": 10,
        "gardenArea": 50,
        "numberOfIndoorParkingSpaces": 1,
        "numberOfOutdoorParkingSpaces": 0,
        "floorNumber": 3,
        "hasLift": true,
        "energyLabel": "B",
        "quality": { "bathrooms": "normal", "kitchen": "normal", "flooring": "normal", "windows": "normal" },
        "condition": { "bathrooms": "well_maintained", "kitchen": "well_maintained", "flooring": "well_maintained", "windows": "well_maintained" }
      }
    }
  ],
  "returnScores": true
}
```

**Property type codes:** `"apartment"` (subcode: `"apartment_normal"`), `"house"`, `"multi_family_house"`

- Cannot mix `multi_family_house` with `apartment`/`house` in the same batch

**Quality values:** `"simple"`, `"normal"`, `"high_quality"`, `"luxury"`
**Condition values:** `"renovation_needed"`, `"well_maintained"`, `"new_or_recently_renovated"`
**numberOfRooms:** must be between 1 and 12

**Response format:**

```json
{
  "valuations": [
    [
      {
        "salePrice": 1200000,
        "salePriceRange": { "lower": 1050000, "upper": 1350000 },
        "rentGross": 3200,
        "rentGrossRange": { "lower": 2800, "upper": 3600 },
        "rentNet": 2800,
        "rentNetRange": { "lower": 2400, "upper": 3200 },
        "confidence": "good",
        "confidenceScore": 0.85,
        "currency": "CHF",
        "coordinates": { "latitude": 47.3769, "longitude": 8.5417 },
        "scores": { "location": 0.78 },
        "status": { "code": 0, "message": "" }
      }
    ]
  ]
}
```

- Structure: `valuations[i][j]` = valuation for property i on date j

**Error handling (docs: https://docs.pricehubble.com/international/error_handling/)**

- 200: Success (may contain local errors per property in `status` field)
- 400: Bad request (e.g. `"numberOfRooms: Must be between 1 and 12"`)
- 401: Unauthorized — `{ "error": "invalid_token", "error_description": "The access token is invalid or has expired" }`
- 403: Forbidden
- 404: Not found
- 500: Internal server error
- Local errors: individual property failures within 200 response via `status.code` != 0

**Fetch trigger & caching rules:**

- **Trigger:** PriceHubble is called **only when a user clicks "Run Analysis"** on a property — not on listing import or scoring.
- **Two API calls per fetch:** one with `dealType: "sale"` (bank valuation), one with `dealType: "rent"` (hedonic rent).
- **Cache lookup order** (before making any API call):
  1. Check the **Property** record for stored valuation data
  2. If none, check the **source Listing** record (if property was saved from a listing — another user may have already triggered a fetch)
  3. If none found → call PriceHubble API
- **Cache write-back:** After a successful API call, save results to **both**:
  - The **Property** record (so re-analyzing skips the API)
  - The **source Listing** record if one exists (so any other user who saved the same listing benefits)
- **Result:** Each unique listing/property gets at most **one** PriceHubble API call ever, regardless of how many users analyze it.
- **No cache expiry** for now — once fetched, data is reused indefinitely.
- **Graceful degradation:** If PriceHubble is unavailable, credentials are unset, or the API returns an error, the analysis runs without valuation data (falls back to current behavior: purchasePrice as LTV basis, cantonal avg for rent).

**Implementation approach:**

1. ~~Obtain API credentials from PriceHubble sales~~ ✅ Done
2. Build PriceHubble API client with token caching (12h TTL, re-auth on 401)
3. Map our Property fields → PriceHubble input format
4. Schema: add valuation fields to Property and Listing models
5. Valuation service: cache lookup (Property → Listing) → API fetch → write-back to both
6. Apply `min(purchasePrice, hedonicValue)` as Belehnungswert in financing engine
7. Use hedonic rent estimate as primary rent source (fallback to cantonal avg if API unavailable)
8. Surface valuation data in analysis detail + PDF

#### Tasks

**A. API Client & Infrastructure**

| #      | Task                                                                                                                                                                                                      | Status | Tests |
| ------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-4.1 | Obtain PriceHubble API credentials (contact sales, negotiate pricing, get sandbox access)                                                                                                                 | [x]    | —     |
| P2-4.2 | PriceHubble API client: auth with token caching (12h TTL), auto-refresh on expiry, re-auth on 401                                                                                                         | [x]    | 13/13 |
| P2-4.3 | Valuation service: fetch sale price + rent in two calls, with cache lookup (Property → source Listing → API) and write-back to both                                                                       | [x]    | 16/16 |
| P2-4.4 | Schema migration: add valuation fields to Property model (`hedonicSalePrice`, `hedonicRentGrossMonthly`, `hedonicConfidence`, etc.)                                                                       | [x]    | —     |
| P2-4.5 | Schema migration: add same valuation fields to Listing model (shared cache for all users who save the same listing)                                                                                       | [x]    | —     |
| P2-4.6 | Property field mapping: convert our Property/Listing model fields to PriceHubble input format                                                                                                             | [x]    | 21/21 |
| P2-4.7 | Environment config: `PRICEHUBBLE_USERNAME`, `PRICEHUBBLE_PASSWORD` env vars + docs                                                                                                                        | [x]    | —     |
| P2-4.8 | Graceful degradation: fall back to current behavior when PriceHubble is unavailable or credentials unset                                                                                                  | [x]    | —     |

**B. Financing Engine — Niederstwertprinzip**

| #       | Task                                                                                                     | Status | Tests |
| ------- | -------------------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-4.9  | Add `bankValuation`, `belehnungswert`, `niederstwertApplied` to analysis pipeline input/output types     | [x]    | —     |
| P2-4.10 | Apply Niederstwertprinzip in financing engine: Belehnungswert = `min(purchasePrice, bankValuation)`      | [x]    | 36/36 |
| P2-4.11 | Recalculate mortgage amount, equity needed, LTV, amortization, DSCR against Belehnungswert               | [x]    | —     |
| P2-4.12 | Add valuation gap field: `(purchasePrice - bankValuation) / bankValuation * 100` (positive = overpaying) | [x]    | —     |

**C. Rent Estimate Integration**

| #       | Task                                                                                                             | Status | Tests |
| ------- | ---------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-4.13 | Use PriceHubble rent estimate as primary rent source in analysis pipeline (above cantonal avg, below user input) | [x]    | 30/30 |
| P2-4.14 | Add `hedonicRentEstimate` to InvestmentResult (show alongside market/target estimates)                           | [x]    | —     |
| P2-4.15 | Update rent source logic: user → PriceHubble hedonic → cantonal average → none                                   | [x]    | —     |

**D. UI & Display**

| #       | Task                                                                                                     | Status | Tests |
| ------- | -------------------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-4.16 | Financing card: show bank valuation, Belehnungswert, Niederstwert badge                                 | [x]    | —     |
| P2-4.17 | Investment card: show hedonic rent estimate with rent source notice                                      | [x]    | —     |
| P2-4.18 | Analysis detail: valuation data flows through existing JSON columns automatically                        | [x]    | —     |
| P2-4.19 | PDF export: include bank valuation and hedonic rent data                                                 | [x]    | —     |
| P2-4.20 | Warnings: NIEDERSTWERT_APPLIED + PRICEHUBBLE_UNAVAILABLE                                                | [x]    | 31/31 |
| P2-4.21 | Warnings: HEDONIC_LOW_CONFIDENCE + USING_HEDONIC_RENT                                                   | [x]    | —     |

**E. Translations & Testing**

| #       | Task                                                                                             | Status | Tests |
| ------- | ------------------------------------------------------------------------------------------------ | ------ | ----- |
| P2-4.22 | EN/DE translations: valuation fields, confidence labels, warnings, section titles                | [x]    | —     |
| P2-4.23 | Unit tests: PriceHubble client (auth, token caching, valuation requests, error handling)         | [x]    | 13/13 |
| P2-4.24 | Unit tests: field mapping (Property → PriceHubble input), graceful degradation                   | [x]    | 21/21 |
| P2-4.25 | Unit tests: valuation cache lookup (Property → Listing → API) and write-back                     | [x]    | 8/8   |
| P2-4.26 | Unit tests: Niederstwertprinzip in financing engine (overprice, underprice, equal, no valuation) | [x]    | 36/36 |
| P2-4.27 | Unit tests: rent source priority (user → hedonic → cantonal → none)                              | [x]    | 30/30 |
| P2-4.28 | Integration tests: analysis pipeline with PriceHubble data (mocked API)                          | [x]    | 17/17 |
| P2-4.29 | Loading UX: spinner button + stepped progress messages while analysis runs                       | [x]    | —     |

**What was done in P2-E4:**

- Schema migration `add_hedonic_valuation_fields`: added 8 fields to both Property and Listing (`hedonicSalePrice`, `hedonicSalePriceLower`, `hedonicSalePriceUpper`, `hedonicRentGrossMonthly`, `hedonicRentNetMonthly`, `hedonicConfidence`, `hedonicConfidenceScore`, `hedonicLocationScore`).
- Created `app/lib/pricehubble/auth.server.ts` — module-level token cache, `getPriceHubbleToken()` with 1-min expiry buffer, `isPriceHubbleConfigured()`, `clearCachedToken()`.
- Created `app/lib/pricehubble/mapper.server.ts` — maps PropertyType/Condition enums to PriceHubble API codes, `buildPriceHubbleInput()` returns null for LAND or missing location data.
- Created `app/lib/pricehubble/valuation.server.ts` — fetches sale + rent valuations in two API calls, 401 retry with token refresh, rent failure non-critical.
- Created `app/lib/pricehubble/cache.server.ts` — two-level cache: check Property → check source Listing → API call → write-back to both.
- Financing engine Niederstwertprinzip: `belehnungswert = min(purchasePrice, bankValuation)`, mortgage/amortization/LTV now based on belehnungswert. Returns `bankValuation`, `belehnungswert`, `niederstwertApplied`.
- Investment engine hedonic rent: inserted hedonic between user and market_estimate in rent priority. Returns `hedonicRentEstimate`.
- 4 new warning codes: `NIEDERSTWERT_APPLIED`, `HEDONIC_LOW_CONFIDENCE`, `USING_HEDONIC_RENT`, `PRICEHUBBLE_UNAVAILABLE`.
- Engine pipeline wired: passes hedonic fields through financing, investment, and warnings calculators.
- Service orchestrator: PriceHubble fetch/cache step inserted between data loading and pipeline execution.
- UI: financing card shows bank valuation + Niederstwert badge, investment card shows hedonic rent row with blue info notice.
- PDF export: bank valuation, belehnungswert, Niederstwert note, hedonic rent estimate.
- EN/DE translations for all new fields, warnings, and UI labels.
- Added `PRICEHUBBLE_USERNAME`/`PRICEHUBBLE_PASSWORD` to `.env.example` and deployment docs.
- 848 tests passing (124 new: 13 auth, 21 mapper, 8 valuation, 8 cache, 5 financing, 4 investment, 8 warnings, 4 integration + fixture updates across existing test files).

---

### P2-E5: Invite-Only Agent/Supplier Access Portal — Detailed Tasks

**Goal:** Give local real estate agents a way to submit listings directly to the platform, unlocking off-portal supply for investors on the Discover page.

**Key decisions:**

- Agents are a new `AGENT` UserRole (reuse User table + same auth)
- Invite-only: admin creates invite → generates token + link → agent accepts
- Auto-publish: agent submissions go live immediately (status=ACTIVE), scored against all investor profiles
- Agent submissions create `Listing` with `source=AGENT`, `submittedByUserId` set

| #       | Task                                                                                                                          | Status | Tests |
| ------- | ----------------------------------------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-5.1  | Schema: Add `AGENT` to UserRole, create `AgentProfile` + `AgentInvite` models, add `submittedByUserId` to Listing + migration | [x]    | —     |
| P2-5.2  | Auth: `requireAgent`, `requireAgentOrAdmin` helpers + role-based login redirect                                               | [x]    | —     |
| P2-5.3  | Validation schemas: `agentInviteSchema`, `agentProfileSchema`, `agentListingSchema`                                           | [x]    | —     |
| P2-5.4  | Email service: Resend integration, `sendAgentInviteEmail` + env vars (`RESEND_API_KEY`, `APP_URL`)                            | [x]    | —     |
| P2-5.5  | Agent service: invite creation/acceptance (with email), profile CRUD, agent list, stats                                       | [x]    | 10/10 |
| P2-5.6  | Agent listing service: submit, update, delist, list submissions + trigger scoring                                             | [x]    | 12/12 |
| P2-5.7  | Agent layout + sidebar (agent-specific app shell, `requireAgent` in loader)                                                   | [x]    | —     |
| P2-5.8  | Agent invite acceptance page: validate token, set password, create profile                                                    | [x]    | —     |
| P2-5.9  | Agent dashboard: stats cards + recent submissions                                                                             | [x]    | —     |
| P2-5.10 | Agent listings index: grid of submissions with status badges                                                                  | [x]    | —     |
| P2-5.11 | Agent listing submission page: form + create action                                                                           | [x]    | —     |
| P2-5.12 | Agent listing detail/edit page: view, edit, delist                                                                            | [x]    | —     |
| P2-5.13 | Agent profile page: company info + covered cantons                                                                            | [x]    | —     |
| P2-5.14 | Admin agents list page + invite button                                                                                        | [x]    | —     |
| P2-5.15 | Admin invite agent page: form + send invite email                                                                             | [x]    | —     |
| P2-5.16 | Admin agent detail page: profile + submissions                                                                                | [x]    | —     |
| P2-5.17 | Route registration + admin sidebar nav item                                                                                   | [x]    | —     |
| P2-5.18 | EN/DE translations: agent namespace + common nav keys                                                                         | [x]    | —     |
| P2-5.19 | Unit tests: agent service (10) + agent listing service (12)                                                                   | [x]    | 22/22 |

**What was done in P2-E5:**

- Added `AGENT` to UserRole enum, created `AgentProfile` (company, phone, website, cantons) and `AgentInvite` (token, expiry, acceptance tracking) models, added `submittedByUserId` to Listing with User relation. Migration `add_agent_portal`.
- Created `app/lib/services/email.server.ts` — thin Resend wrapper with `sendEmail` and `sendAgentInviteEmail`, graceful when `RESEND_API_KEY` not set.
- Created `app/lib/services/agent.server.ts` — invite creation (token + user + email), invite acceptance (validate + set password + create profile), profile CRUD, listAgents, getAgentStats, getAgentDetail.
- Created `app/lib/services/agent-listing.server.ts` — submit (creates Listing with source=AGENT, triggers scoring), update (ownership check), delist, list submissions, get single.
- Added `requireAgent` and `requireAgentOrAdmin` auth helpers. Updated login to redirect agents to `/app/agent/dashboard`.
- Added validation schemas: `agentInviteSchema`, `agentProfileSchema`, `agentListingSchema`.
- Created agent layout (`routes/app/agent/layout.tsx`) with `requireAgent` guard and separate `AgentAppLayout` + `AgentSidebar` components.
- Created agent portal pages: dashboard (stats + recent), listings index (grid), new listing form, listing detail/edit/delist, profile editor with canton selector.
- Created admin agent management pages: agents list with submission counts, invite form with copy-to-clipboard link, agent detail with profile + submissions.
- Created public invite acceptance page (`auth/agent-invite/:token`) — validates token, sets password, creates profile, redirects to agent dashboard.
- Added "Agents" nav item to investor sidebar (admin-only, via `useMatches` role check).
- Added `agent` i18n namespace with EN/DE translations (~80 keys each).
- Added `APP_URL` env var for invite link generation.
- 724 tests passing (22 new: 10 agent service, 12 agent listing service).

**New env vars:**

- `RESEND_API_KEY` — Resend API key for sending emails (optional in dev)
- `APP_URL` — Base URL for invite links (defaults to `http://localhost:3000`)

---

### P2-E7: Investor Listing Submissions — Detailed Tasks

**Goal:** Enable investors to list properties for sale on the platform, visible to all other investors on the Discover page alongside portal-imported and agent-submitted listings. Build a shared listing submission flow reusable by both investors and agents.

**Key decisions:**

- Investors create listings from scratch (no "list existing Property" flow for now)
- Auto-publish: investor listings go live immediately (approval flow deferred to backlog)
- Source badges on Discover: **"Public"** (all scraped portals), **"Agent"**, **"Investor"** — only these 3
- Contact info (email/phone) shown directly on listing detail for AGENT/INVESTOR sources
- Own listings hidden from own Discover feed
- Shared submission form + service extracted from agent flow, parameterized by source
- Investor routes under `/app/my-listings/`

**Architecture:**

```
Shared listing submission service (source-agnostic)
├── createListing(db, userId, data, source)    ← AGENT or INVESTOR
├── updateListing(db, userId, listingId, data) ← ownership check
├── delistListing(db, userId, listingId)       ← ownership check
├── listMySubmissions(db, userId, source?)      ← filter by source
└── getMyListing(db, userId, listingId)         ← ownership check

Shared ListingSubmissionForm component
├── All property fields (address, price, area, rooms, etc.)
├── Contact info fields (email, phone)
├── Used by both /app/my-listings/new and /app/agent/listings/new
└── Source passed as prop (controls minor UI differences)

Discover page changes
├── Source badge: "Public" | "Agent" | "Investor"
├── Contact info on listing detail (AGENT/INVESTOR only)
└── Self-exclusion filter (hide own listings)
```

#### Tasks

**A. Schema & Shared Infrastructure**

| #      | Task                                                                                            | Status | Tests |
| ------ | ----------------------------------------------------------------------------------------------- | ------ | ----- |
| P2-7.1 | Schema: Add `INVESTOR` to Listing source enum + migration                                       | [x]    | —     |
| P2-7.2 | Schema: Add contact fields to Listing (`contactEmail`, `contactPhone`) + migration              | [x]    | —     |
| P2-7.3 | Shared listing submission service: extract from agent-listing.server.ts, parameterize by source | [x]    | —     |
| P2-7.4 | Shared validation schema: extend agentListingSchema with contact fields, reuse for both roles   | [x]    | —     |
| P2-7.5 | Shared ListingSubmissionForm component: extract from agent listings/new.tsx, source as prop     | [x]    | —     |

**B. Investor Listing Routes**

| #      | Task                                                                                       | Status | Tests |
| ------ | ------------------------------------------------------------------------------------------ | ------ | ----- |
| P2-7.6 | Route `/app/my-listings/` index: list investor's own listings with status badges, stats    | [x]    | —     |
| P2-7.7 | Route `/app/my-listings/new`: create listing form using shared component (source=INVESTOR) | [x]    | —     |
| P2-7.8 | Route `/app/my-listings/$id`: detail view, edit mode, delist action                        | [x]    | —     |
| P2-7.9 | Sidebar nav: add "My Listings" item for investors                                          | [x]    | —     |

**C. Discover Page Enhancements**

| #       | Task                                                                                    | Status | Tests |
| ------- | --------------------------------------------------------------------------------------- | ------ | ----- |
| P2-7.10 | Source badges on listing cards: "Public" (FLATFOX/ALLE_IMMOBILIEN), "Agent", "Investor" | [x]    | —     |
| P2-7.11 | Contact info display on listing detail page for AGENT/INVESTOR sources (email/phone)    | [x]    | —     |
| P2-7.12 | Self-exclusion: filter out own listings from Discover feed query                        | [x]    | —     |

**D. Agent Flow Refactor**

| #       | Task                                                                            | Status | Tests |
| ------- | ------------------------------------------------------------------------------- | ------ | ----- |
| P2-7.13 | Refactor agent listing routes to use shared submission service + form component | [x]    | —     |

**E. Translations & Testing**

| #       | Task                                                                                     | Status | Tests |
| ------- | ---------------------------------------------------------------------------------------- | ------ | ----- |
| P2-7.14 | EN/DE translations: my-listings UI, source badges, contact info labels, form fields      | [x]    | —     |
| P2-7.15 | Unit tests: shared listing submission service (create, update, delist, ownership checks) | [x]    | 14/14 |
| P2-7.16 | Integration tests: investor listing routes (create, edit, delist, list)                  | [x]    | 3/3   |

#### Backlog — Deferred Tasks

| #       | Task                                                                | Priority | Notes                                           |
| ------- | ------------------------------------------------------------------- | -------- | ----------------------------------------------- |
| P2-7.B1 | Admin approval flow for investor listings before publishing         | Medium   | Investors aren't vetted like invite-only agents |
| P2-7.B2 | Fraud/report mechanism: other investors can flag suspect listings   | Medium   | "Report listing" button → admin review queue    |
| P2-7.B3 | "List existing Property for sale": convert owned Property → Listing | Low      | Pre-fill from Property data, link back          |

**What was done in P2-E7:**

- Added `INVESTOR` to `ListingSource` enum + `contactEmail`/`contactPhone` fields on Listing model. Migration applied.
- Created shared listing submission service (`listing-submission.server.ts`): `createListing`, `updateListing`, `delistListing`, `listMySubmissions`, `getMyListing` — parameterized by source (AGENT or INVESTOR), with ownership checks.
- Created shared `ListingSubmissionForm` component: all property fields + contact info, source passed as prop, used by both investor and agent flows.
- Created investor routes: `/app/my-listings/` index (list own listings with status badges), `/app/my-listings/new` (create form), `/app/my-listings/$id` (detail/edit/delist).
- Added "My Listings" sidebar nav item for investors.
- Added source badges on Discover listing cards: "Public" (FLATFOX/ALLE_IMMOBILIEN), "Agent", "Investor".
- Added contact info display (email/phone) on Discover detail page for AGENT/INVESTOR sources.
- Added self-exclusion filter: own listings hidden from Discover feed.
- Refactored agent listing routes to use shared submission service and form component.
- Added `listing-submission` i18n namespace with EN/DE translations.
- 777 tests passing (17 new: 14 listing-submission service, 3 listing self-exclusion).

---

## Phase 3 — AI Acquisition Engine (Learning + Matching)

**Goal:** Become the decision layer by learning from behavior and outcomes; make recommendations investor-specific.

**Success signals:**

- Users treat the system as the default decision tool.

### Epics

| #     | Epic                                                                | Status  |
| ----- | ------------------------------------------------------------------- | ------- |
| P3-E1 | Market Opportunity Radar (undervaluation signals, pattern matching) | Planned |
| P3-E2 | Investor–Property Matching (fit score, purchase likelihood)         | Planned |
| P3-E3 | Supply-Side Integration (agent readiness visibility, match quality) | Planned |
| P3-E4 | Feedback Loop / Data Moat (interaction tracking, outcome learning)  | Planned |

---

## Phase 4 — Infrastructure Layer (Long-term)

**Goal:** Become essential infrastructure for Swiss real-estate investment intelligence.

**Success signals:**

- Ecosystem dependence: investors + suppliers + partners rely on the intelligence layer.

### Epics

| #     | Epic                                                       | Status  |
| ----- | ---------------------------------------------------------- | ------- |
| P4-E1 | Predictive Off-Market Supply Signals                       | Planned |
| P4-E2 | Bank Integrations / Pre-Qualification Flows                | Planned |
| P4-E3 | Transaction Workflow Support (checklists, partner network) | Planned |
| P4-E4 | Institutional-Grade Analytics and Benchmarks               | Planned |
