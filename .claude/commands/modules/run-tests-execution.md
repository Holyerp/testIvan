# Run Tests - Execution Module

**Purpose:** Detailed test execution strategies for all test types.

**Parent Command:** `/run-tests`

---

## Test Type Execution

### 1. Unit Tests

**Command:** `npm test` or `npm run test:unit`

**What to test:**
- Individual functions
- Components in isolation
- Utilities and helpers
- Pure functions
- Business logic

**Execution steps:**
```bash
# Run all unit tests
npm test

# Run specific test file
npm test -- path/to/file.test.ts

# Run tests matching pattern
npm test -- --grep "user"

# Run with watch mode
npm test -- --watch
```

**Expected output:**
```
 UserService.createUser - creates user with valid data
 UserService.createUser - throws error with invalid email
 UserService.createUser - throws error with short password
 calculateTotal - returns sum of prices
 calculateTotal - handles empty array
```

---

### 2. Integration Tests

**Command:** `npm run test:integration`

**What to test:**
- API endpoints
- Database interactions
- Service integrations
- External API calls
- Authentication flows

**MANDATORY: Test ALL status codes per `.claude/rules/testing.md`:**
- 200/201 - Success
- 400 - Bad request / validation errors
- 401 - Unauthorized
- 403 - Forbidden
- 404 - Not found
- 500 - Server error

**Execution steps:**
```bash
# Run all integration tests
npm run test:integration

# Run specific endpoint tests
npm run test:integration -- api-users

# Run with coverage
npm run test:integration -- --coverage
```

**Expected output:**
```
 POST /api/users - creates user (200)
 POST /api/users - rejects invalid email (400)
 POST /api/users - requires authentication (401)
 POST /api/users - prevents non-admin access (403)
 GET /api/users/:id - returns 404 for non-existent user
 POST /api/users - handles database errors (500)
```

**Validation:**
- [ ] Each endpoint has 6 tests (one per status code)
- [ ] Authentication tested
- [ ] Authorization tested
- [ ] Validation tested
- [ ] Error handling tested

---

### 3. E2E Tests

**Command:** `npm run test:e2e` or `npx playwright test`

**What to test:**
- User flows
- Critical paths
- Authentication flow
- Checkout/payment flow
- Multi-step processes

**Execution steps:**
```bash
# Run all E2E tests
npm run test:e2e

# Run specific test file
npx playwright test auth-flow.e2e.test.ts

# Run in headed mode (with browser visible)
npx playwright test --headed

# Run with specific browser
npx playwright test --project=chromium
```

**Expected output:**
```
 User can register and login
 User can add product to cart
 User can complete checkout
 Admin can create product
 User can reset password
```

**Critical paths to test:**
- User registration ’ Email verification ’ Login
- Product search ’ Add to cart ’ Checkout ’ Payment
- Admin login ’ Create/edit content ’ Publish

---

### 4. Coverage Tests

**Command:** `npm run test:coverage`

**What it does:**
- Runs all tests (unit + integration + E2E)
- Generates coverage report
- Shows uncovered lines

**Execution steps:**
```bash
# Run with coverage
npm run test:coverage

# View HTML report
open coverage/index.html

# Check coverage threshold
npm run test:coverage -- --coverage --coverageThreshold='{"global":{"lines":80}}'
```

**Expected output:**
```
Coverage Summary:
  Statements   : 87.5% ( 350/400 )
  Branches     : 82.3% ( 140/170 )
  Functions    : 91.2% ( 104/114 )
  Lines        : 87.5% ( 350/400 )
```

**Quality gates:**
- [ ] Overall coverage > 80%
- [ ] Branch coverage > 75%
- [ ] Function coverage > 85%

---

## Test Execution by Argument Type

### Story-Specific Tests (`/run-tests story US-XXX`)

**Process:**
1. Parse story ID from command
2. Find tests tagged with story ID
3. Run matching tests only

**Implementation:**
```bash
# Find tests with story tag
npm test -- --grep "US-045"

# Or run tests in specific folder
npm test -- __tests__/stories/US-045/
```

**Tag format in tests:**
```typescript
describe('US-045: User Profile', () => {
  // Tests for story US-045
});
```

---

### File-Specific Tests (`/run-tests file <path>`)

**Process:**
1. Parse file path from command
2. Find corresponding test file
3. Run that test file

**Test file naming conventions:**
- `ComponentName.test.tsx` for components
- `function-name.test.ts` for utilities
- `api-endpoint.integration.test.ts` for API routes

**Execution:**
```bash
# Test specific file
npm test -- src/services/user.service.test.ts

# Or infer test file from source file
npm test -- src/services/user.service.ts
# ’ Runs src/services/user.service.test.ts
```

---

## Framework-Specific Execution

### Vitest (Modern, Fast)

```bash
# Run all tests
vitest

# Run with coverage
vitest --coverage

# Run specific file
vitest run path/to/test.ts

# Watch mode
vitest watch
```

### Jest (Traditional)

```bash
# Run all tests
jest

# Run with coverage
jest --coverage

# Run specific file
jest path/to/test.ts

# Watch mode
jest --watch
```

### Playwright (E2E)

```bash
# Run all E2E tests
npx playwright test

# Run specific test
npx playwright test auth-flow.e2e.test.ts

# Headed mode (with browser visible)
npx playwright test --headed

# Debug mode
npx playwright test --debug

# Specific browser
npx playwright test --project=chromium
```

---

## Error Handling During Execution

### Test Failures

**When tests fail:**
1. Capture failed test details
2. Note error messages
3. Identify file and line number
4. Suggest potential fixes

**Example failure:**
```
 POST /api/users - creates user (200)
  Expected status 200, received 500
  Error: Database connection failed
  at test/api/users.test.ts:15:20
```

**Action:**
- Show full error message to user
- Identify root cause (database, validation, etc.)
- Recommend fixes

---

### Coverage Below Threshold

**When coverage < 80%:**
1. Identify uncovered files
2. Show uncovered lines
3. Suggest where to add tests

**Example:**
```
Coverage below threshold:
- src/services/payment.service.ts: 45% (missing: lines 23-45, 67-89)
- src/utils/validators.ts: 72% (missing: lines 15-18)

Recommendation:
Add tests for:
1. Payment error handling (lines 23-45)
2. Validation edge cases (lines 15-18)
```

---

## Quality Checks After Execution

**Before reporting success:**
- [ ] All tests executed without errors
- [ ] Coverage meets threshold (80%+)
- [ ] All API status codes tested (200/400/401/403/404/500)
- [ ] No skipped tests (unless intentional)
- [ ] No warnings or deprecation notices

---

**Related:**
- Parent: `.claude/commands/run-tests.md`
- Sibling: `run-tests-reporting.md`
- Rules: `.claude/rules/testing.md`
