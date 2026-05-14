# Run Tests — Execution Module

**Purpose:** Test-execution strategies and scope filtering for `/run-tests`.

**Parent:** `.claude/commands/run-tests.md`
**Companion:** `run-tests-framework-commands.md` (framework-specific command reference — Vitest/Jest/Playwright)

---

## Test Type Execution

### 1. Unit Tests

**Command:** `npm test` (or `npm run test:unit`)

What to test:
- Individual functions + components in isolation
- Utilities, helpers, pure functions
- Business logic

**Variants:**
```bash
npm test                            # all unit tests
npm test -- path/to/file.test.ts    # specific file
npm test -- --grep "user"            # match pattern
npm test -- --watch                  # watch mode
```

Expected output sample:
```
✓ UserService.createUser — creates user with valid data
✓ UserService.createUser — throws on invalid email
✓ UserService.createUser — throws on short password
✓ calculateTotal — returns sum of prices
```

---

### 2. Integration Tests

**Command:** `npm run test:integration`

What to test:
- API endpoints (all status codes)
- Database interactions
- Service integrations + external API calls
- Authentication flows

**MANDATORY** — all status codes per `.claude/rules/testing.md`:

| Code | Case |
|------|------|
| 200/201 | Success |
| 400 | Bad request / validation |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not found |
| 500 | Server error |

**Variants:**
```bash
npm run test:integration                    # all
npm run test:integration -- api-users       # specific endpoint
npm run test:integration -- --coverage      # with coverage
```

Validation:
- [ ] Each endpoint has all 6 status-code tests
- [ ] Authentication tested
- [ ] Authorization tested
- [ ] Validation tested
- [ ] Error handling tested

---

### 3. E2E Tests

**Command:** `npm run test:e2e` (or `npx playwright test`)

What to test:
- Critical user flows end-to-end
- Authentication flow
- Checkout / payment flow
- Multi-step processes

**Critical paths to cover:**
- User registration → email verification → login
- Product search → add to cart → checkout → payment
- Admin login → create/edit content → publish

**Variants:**
```bash
npm run test:e2e                                      # all
npx playwright test auth-flow.e2e.test.ts             # specific file
npx playwright test --headed                          # see browser
npx playwright test --project=chromium                # specific browser
```

---

### 4. Coverage Run

**Command:** `npm run test:coverage`

Runs all tests and emits a coverage report.

```bash
npm run test:coverage                                     # run + report
open coverage/index.html                                  # view HTML report
npm run test:coverage -- --coverageThreshold='{"global":{"lines":80}}'
```

Quality gates:
- [ ] Overall coverage ≥ 80%
- [ ] Branch coverage ≥ 75%
- [ ] Function coverage ≥ 85%

Sample output:
```
Coverage Summary:
  Statements : 87.5%  (350/400)
  Branches   : 82.3%  (140/170)
  Functions  : 91.2%  (104/114)
  Lines      : 87.5%  (350/400)
```

---

## Scope Filtering (by argument)

### Story-specific — `/run-tests story US-XXX`

1. Parse story ID from command.
2. Find tests tagged with the story ID.
3. Run matching tests only.

```bash
npm test -- --grep "US-045"        # pattern match
npm test -- __tests__/stories/US-045/   # folder match
```

Tag format inside test files:
```typescript
describe('US-045: User Profile', () => {
  // tests for this story
});
```

---

### File-specific — `/run-tests file <path>`

1. Parse file path from command.
2. Find the corresponding test file.
3. Run that test file.

Test file naming conventions:
- `ComponentName.test.tsx` — components
- `function-name.test.ts` — utilities
- `api-endpoint.integration.test.ts` — API routes

```bash
npm test -- src/services/user.service.test.ts  # direct
npm test -- src/services/user.service.ts       # infer .test neighbor
```

---

## Framework Commands

Verbatim invocation commands for Vitest, Jest, and Playwright — including watch mode, coverage flags, filters, and debug mode — live in **`run-tests-framework-commands.md`**.

---

## Error Handling During Execution

### Test failures

1. Capture failed test details (name, file, line, error message).
2. Identify root cause from the error (DB connection, validation, auth middleware, etc.).
3. Emit findings through the reporting module (`run-tests-reporting.md`) — don't hide failures behind a summary.

Example:
```
✗ POST /api/users — creates user (200)
  Expected status 200, received 500
  Error: Database connection failed
  at test/api/users.test.ts:15:20
```

### Coverage below threshold

1. Identify uncovered files.
2. Show uncovered lines.
3. Suggest where to add tests (prioritize by impact — auth, payments, checkout over utilities).

Example:
```
Coverage below threshold:
- src/services/payment.service.ts: 45% (missing: lines 23-45, 67-89)
- src/utils/validators.ts:         72% (missing: lines 15-18)

Recommendation:
1. Payment error handling  (lines 23-45)
2. Validation edge cases   (lines 15-18)
```

---

## Post-Execution Quality Checks

Before reporting success:
- [ ] All tests executed without errors
- [ ] Coverage meets threshold (80%+)
- [ ] All API status codes tested (200/400/401/403/404/500)
- [ ] No skipped tests (unless intentional + documented)
- [ ] No new warnings / deprecation notices

---

**Version:** 3.3.0
**Last Updated:** 2026-04-21 (split: framework-specific commands moved to companion)
**Related:**
- Parent: `.claude/commands/run-tests.md`
- Reporting: `run-tests-reporting.md` (+ `run-tests-reporting-formats.md`)
- Framework commands: `run-tests-framework-commands.md`
- Rules: `.claude/rules/testing.md`
