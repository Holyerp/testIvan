# Run Tests — Framework Commands

Companion to `run-tests-execution.md`. Verbatim invocation reference for the test runners this framework supports (Vitest, Jest, Playwright). Keeps the parent module focused on flow + scope; this module is a lookup table.

---

## Vitest (modern, fast — default for this framework)

```bash
vitest                               # run all tests
vitest run                           # run once (no watch)
vitest watch                         # watch mode
vitest --coverage                    # run + coverage
vitest run path/to/test.ts           # specific file
vitest run --reporter=verbose        # verbose output
vitest run -t "user service"         # filter by test name pattern
vitest --ui                          # open UI dashboard
```

**With npm scripts** (typical `package.json`):
```bash
npm test                    # → vitest
npm run test:watch          # → vitest watch
npm run test:coverage       # → vitest --coverage
```

---

## Jest (traditional)

```bash
jest                                 # run all
jest --watch                         # watch mode
jest --coverage                      # run + coverage
jest path/to/test.ts                 # specific file
jest -t "user service"               # filter by name pattern
jest --updateSnapshot                # update snapshots
jest --detectOpenHandles             # diagnose hanging handles
jest --bail                          # stop on first failure
```

**Coverage threshold enforcement:**
```bash
jest --coverage \
  --coverageThreshold='{"global":{"lines":80,"branches":75,"functions":85}}'
```

---

## Playwright (E2E)

```bash
npx playwright test                         # run all E2E
npx playwright test auth-flow.e2e.test.ts   # specific file
npx playwright test --headed                # with browser visible
npx playwright test --debug                 # debugger / step through
npx playwright test --project=chromium      # specific browser
npx playwright test --project=webkit        # Safari-equivalent
npx playwright test --project=firefox
npx playwright test --grep "login"          # filter by title
npx playwright test --workers=1             # serial (debugging)
npx playwright test --trace=on              # capture traces
npx playwright show-report                  # open HTML report
npx playwright codegen http://localhost:3000  # record a new test
```

**Install browsers** (first-time setup):
```bash
npx playwright install              # all supported
npx playwright install chromium     # just Chromium
```

---

## Common Filters (framework-agnostic)

| Goal | Vitest | Jest | Playwright |
|------|--------|------|-----------|
| By name pattern | `-t "pattern"` | `-t "pattern"` | `--grep "pattern"` |
| By file | `vitest run <path>` | `jest <path>` | `playwright test <path>` |
| Watch mode | `vitest watch` | `jest --watch` | N/A (re-run manually) |
| Coverage | `--coverage` | `--coverage` | use separate tool (istanbul/c8) |
| Update snapshots | `-u` | `--updateSnapshot` | N/A |
| Bail on first failure | `--bail 1` | `--bail` | `--max-failures=1` |

---

## Detecting Which Framework a Project Uses

Look at `package.json`:
- `"vitest"` in `devDependencies` → Vitest.
- `"jest"` in `devDependencies` → Jest.
- `"@playwright/test"` in `devDependencies` → Playwright for E2E.

Many projects use Vitest (or Jest) **plus** Playwright (E2E) — not mutually exclusive.

`/execute-work` runs whatever `npm test` and `npm run test:e2e` are wired to in `package.json` — framework detection is only needed when those scripts are missing or you need to invoke directly.

---

**Version:** 3.2.0
**Created:** 2026-04-21 (split from `run-tests-execution.md`)
**Parent:** `run-tests-execution.md`
