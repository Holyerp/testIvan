---
name: run-tests
description: Manually run project tests (all, unit, integration, e2e, coverage, or for specific file/story)
---

# Run Tests Command

Manually execute tests for the project with detailed reporting.

---

## Usage

```bash
/run-tests all                          # Run all tests (unit, integration, E2E)
/run-tests unit                         # Run unit tests only
/run-tests integration                  # Run integration tests only
/run-tests e2e                          # Run E2E tests only
/run-tests coverage                     # Run all tests with coverage report
/run-tests file <path>                  # Run tests for specific file
/run-tests story US-XXX                 # Run tests related to specific story
/run-tests watch                        # Run tests in watch mode
```

---

## 📋 YOUR TASK

### STEP 1: Parse Command Arguments

**Extract the test scope from the command:**

```javascript
const args = userCommand.split(' ');
const scope = args[1]; // 'all', 'unit', 'integration', 'e2e', 'coverage', 'file', 'story', 'watch'
const target = args[2]; // file path or story ID (if applicable)
```

**Display:**
```
🧪 Running Tests: {{scope}} {{target}}
```

---

### STEP 2: Determine Test Commands

**Based on scope, determine which commands to run:**

#### Scope: `all`
```bash
npm test                    # Vitest (unit + integration)
npm run test:e2e            # Playwright (E2E)
```

#### Scope: `unit`
```bash
npm run test:unit
# OR if not defined:
npm test -- src/**/*.test.{ts,tsx} --exclude **/*.integration.test.*
```

#### Scope: `integration`
```bash
npm run test:integration
# OR if not defined:
npm test -- src/**/*.integration.test.{ts,tsx}
```

#### Scope: `e2e`
```bash
npm run test:e2e
# OR if not defined:
npx playwright test
```

#### Scope: `coverage`
```bash
npm run test:coverage
# OR if not defined:
npm test -- --coverage
```

#### Scope: `file <path>`
```bash
npm test -- {{file_path}}
```

#### Scope: `story US-XXX`
1. Search codebase for files related to story US-XXX
2. Find test files matching those files
3. Run tests for those specific files

#### Scope: `watch`
```bash
npm run test:watch
# OR if not defined:
npm test -- --watch
```

---

### STEP 3: Check if Test Commands Exist

**Read `package.json` to verify test scripts exist:**

```javascript
// Read package.json
const packageJson = Read('/path/to/package.json');

// Check if scripts exist
const hasTestUnit = packageJson.scripts['test:unit'];
const hasTestIntegration = packageJson.scripts['test:integration'];
const hasTestE2E = packageJson.scripts['test:e2e'];
const hasTestCoverage = packageJson.scripts['test:coverage'];
const hasTestWatch = packageJson.scripts['test:watch'];
```

**If script is missing AND needed:**

Display warning:
```
⚠️  Warning: `npm run {{script}}` not defined in package.json

Using fallback command:
{{fallback_command}}

💡 Tip: Add to package.json for better organization:
"scripts": {
  "{{script}}": "{{suggested_command}}"
}
```

---

### STEP 4: Execute Tests

**For each test command determined in STEP 2:**

```bash
# Display before running:
"Running: {{command}}"

# Execute:
Bash({{command}})

# Capture output
```

**Display loading:**
```
🧪 Running {{test_type}} tests...

⏳ This may take a moment...
```

---

### STEP 5: Parse Test Results

**From Vitest output, extract:**

```javascript
// Example Vitest output:
// Test Files  12 passed (12)
//      Tests  248 passed (248)
//   Duration  3.45s

const testResults = {
  files: {
    passed: 12,
    failed: 0,
    total: 12
  },
  tests: {
    passed: 248,
    failed: 0,
    skipped: 0,
    total: 248
  },
  duration: '3.45s'
};
```

**From Playwright output, extract:**

```javascript
// Example Playwright output:
// Running 15 tests using 3 workers
//   15 passed (2.1s)

const e2eResults = {
  passed: 15,
  failed: 0,
  flaky: 0,
  skipped: 0,
  total: 15,
  duration: '2.1s'
};
```

**From Coverage output, extract:**

```javascript
// Example coverage output:
// Statements   : 87.5% ( 350/400 )
// Branches     : 82.3% ( 141/171 )
// Functions    : 89.1% ( 82/92 )
// Lines        : 88.2% ( 338/383 )

const coverage = {
  statements: { pct: 87.5, covered: 350, total: 400 },
  branches: { pct: 82.3, covered: 141, total: 171 },
  functions: { pct: 89.1, covered: 82, total: 92 },
  lines: { pct: 88.2, covered: 338, total: 383 }
};
```

---

### STEP 6: Display Results

#### 6.1 Success Case (All Tests Passed)

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ ALL TESTS PASSED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 TEST RESULTS:

Unit Tests:
  ✅ Files:  {{unit_files_passed}}/{{unit_files_total}} passed
  ✅ Tests:  {{unit_tests_passed}}/{{unit_tests_total}} passed
  ⏱️  Duration: {{unit_duration}}

Integration Tests:
  ✅ Files:  {{int_files_passed}}/{{int_files_total}} passed
  ✅ Tests:  {{int_tests_passed}}/{{int_tests_total}} passed
  ⏱️  Duration: {{int_duration}}

E2E Tests:
  ✅ Tests:  {{e2e_passed}}/{{e2e_total}} passed
  ⏱️  Duration: {{e2e_duration}}

TOTAL:
  ✅ {{total_passed}}/{{total_tests}} tests passed
  ⏱️  Total Duration: {{total_duration}}

{{If coverage was run:}}
📈 CODE COVERAGE:

  Statements:  {{statements_pct}}% ({{statements_covered}}/{{statements_total}})
  Branches:    {{branches_pct}}% ({{branches_covered}}/{{branches_total}})
  Functions:   {{functions_pct}}% ({{functions_covered}}/{{functions_total}})
  Lines:       {{lines_pct}}% ({{lines_covered}}/{{lines_total}})

  {{If coverage >= 80%:}}
  ✅ Coverage Target Met ({{coverage_pct}}% >= 80%)
  {{Else:}}
  ⚠️  Coverage Below Target ({{coverage_pct}}% < 80%)
     Need {{missing_pct}}% more coverage

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

#### 6.2 Failure Case (Some Tests Failed)

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
❌ TESTS FAILED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 TEST RESULTS:

Unit Tests:
  {{If unit_failed > 0:}}
  ❌ Files:  {{unit_files_passed}}/{{unit_files_total}} passed ({{unit_files_failed}} failed)
  ❌ Tests:  {{unit_tests_passed}}/{{unit_tests_total}} passed ({{unit_tests_failed}} failed)
  {{Else:}}
  ✅ Files:  {{unit_files_passed}}/{{unit_files_total}} passed
  ✅ Tests:  {{unit_tests_passed}}/{{unit_tests_total}} passed

Integration Tests:
  {{If int_failed > 0:}}
  ❌ Files:  {{int_files_passed}}/{{int_files_total}} passed ({{int_files_failed}} failed)
  ❌ Tests:  {{int_tests_passed}}/{{int_tests_total}} passed ({{int_tests_failed}} failed)
  {{Else:}}
  ✅ Files:  {{int_files_passed}}/{{int_files_total}} passed
  ✅ Tests:  {{int_tests_passed}}/{{int_tests_total}} passed

E2E Tests:
  {{If e2e_failed > 0:}}
  ❌ Tests:  {{e2e_passed}}/{{e2e_total}} passed ({{e2e_failed}} failed)
  {{Else:}}
  ✅ Tests:  {{e2e_passed}}/{{e2e_total}} passed

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🔍 FAILED TESTS:

{{For each failed test:}}
❌ {{test_file_path}}
   Test: {{test_name}}
   Error: {{error_message}}

   {{Stack trace (first 10 lines)}}

   💡 Suggested Fix:
   {{Analyze error and suggest fix}}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🛠️  NEXT STEPS:

1. Fix the {{failed_count}} failing test(s) above
2. Run tests again: /run-tests all
3. When all pass, mark story as complete

📝 Common Fixes:
- Check test assertions match expected behavior
- Verify mock data is correct
- Ensure async operations complete before assertions
- Check for race conditions in tests
- Verify database seeding/cleanup

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

### STEP 7: Check API Status Code Coverage (for Integration Tests)

**If running integration tests, verify all status codes are tested:**

**Required status codes for EVERY API endpoint:**
- 200/201 (Success)
- 400 (Bad Request)
- 401 (Unauthorized)
- 403 (Forbidden)
- 404 (Not Found)
- 500 (Server Error)

**Search test files for status code assertions:**

```javascript
// Search for patterns like:
expect(response.status).toBe(200)
expect(response.statusCode).toBe(400)
expect.status(401)
```

**Display status code coverage:**

```
📋 API STATUS CODE COVERAGE:

Endpoint: POST /api/users
  ✅ 201 - User created successfully
  ✅ 400 - Invalid email format
  ✅ 401 - No authentication token
  ✅ 403 - Insufficient permissions
  ⚠️  404 - Not applicable (always creates)
  ✅ 500 - Database error handling

Endpoint: GET /api/users/:id
  ✅ 200 - User found
  ✅ 400 - Invalid ID format
  ✅ 401 - No authentication
  ✅ 403 - Access denied
  ✅ 404 - User not found
  ✅ 500 - Database error

{{If any status codes missing:}}
⚠️  WARNING: Missing status code tests:

Endpoint: PUT /api/users/:id
  ❌ 403 - Forbidden test missing
  ❌ 500 - Error handling test missing

💡 Tip: Add tests for these status codes before marking story complete.
```

---

### STEP 8: Analyze Test Trends (Optional Enhancement)

**If you have access to previous test runs, show trends:**

```
📈 TEST TRENDS:

Current Run:    {{current_passed}}/{{current_total}} passed ({{current_pct}}%)
Previous Run:   {{prev_passed}}/{{prev_total}} passed ({{prev_pct}}%)

Trend: {{increasing / stable / decreasing}}

{{If decreasing:}}
⚠️  Warning: Test passing rate decreased
   {{new_failing_count}} test(s) that were passing are now failing

{{If new tests added:}}
✨ {{new_test_count}} new test(s) added since last run
```

---

### STEP 9: Suggest Actions

**Based on test results, suggest next actions:**

```
🎯 RECOMMENDED ACTIONS:

{{If all tests passed AND coverage >= 80%:}}
✅ All quality gates passed!
   Ready to mark story as complete.

{{If all tests passed BUT coverage < 80%:}}
⚠️  Tests pass but coverage is low ({{coverage}}%)
   Add {{missing_coverage}}% more coverage:
   - Focus on: {{uncovered_files}}
   - Add tests for edge cases
   - Test error handling paths

{{If tests failed:}}
🔧 Fix failing tests first:
   Priority: {{high_priority_failures}}
   Quick wins: {{easy_fixes}}

{{If missing API status codes:}}
📝 Add missing API status code tests:
   {{list_missing_codes}}

{{If i18n enabled and text found:}}
🌐 Check for hardcoded text:
   {{files_with_hardcoded_text}}
```

---

## Advanced Features

### Running Tests for Specific Story

**When user runs `/run-tests story US-015`:**

1. **Find related files:**
   ```bash
   # Search for US-015 references in code
   git log --all --grep="US-015" --name-only

   # Or search file contents
   grep -r "US-015" src/
   ```

2. **Find corresponding test files:**
   ```javascript
   // If source file is: src/components/UserForm.tsx
   // Look for test files:
   - src/components/UserForm.test.tsx
   - src/components/__tests__/UserForm.test.tsx
   - tests/unit/UserForm.test.tsx
   ```

3. **Run tests for those files:**
   ```bash
   npm test -- src/components/UserForm.test.tsx
   ```

4. **Display story-specific results:**
   ```
   🧪 Tests for Story US-015: User Registration Form

   Related Files:
   - src/components/UserForm.tsx
   - src/api/register.ts
   - src/utils/validation.ts

   Test Files:
   ✅ src/components/UserForm.test.tsx (12 tests passed)
   ✅ src/api/register.integration.test.ts (6 tests passed)
   ✅ tests/e2e/registration.e2e.test.ts (3 tests passed)

   Total: 21/21 tests passed for US-015
   ```

---

## Error Handling

### No package.json Found
```
❌ Error: package.json not found

This command requires a Node.js project with package.json.
Ensure you're in the project root directory.
```

### No Test Scripts Defined
```
⚠️  Warning: No test scripts found in package.json

Please add test scripts:
"scripts": {
  "test": "vitest",
  "test:unit": "vitest run src/**/*.test.{ts,tsx}",
  "test:integration": "vitest run src/**/*.integration.test.{ts,tsx}",
  "test:e2e": "playwright test",
  "test:coverage": "vitest run --coverage",
  "test:watch": "vitest watch"
}

Or use default stack configuration: /init-project
```

### Tests Not Found for Story
```
⚠️  Warning: No tests found for story US-015

Possible reasons:
1. Story not yet implemented
2. Tests not written yet
3. Story ID not referenced in code/commits

💡 Tip: Reference story ID in:
- File names: user-form.US-015.test.tsx
- Commit messages: "feat: implement US-015 ..."
- Test descriptions: describe('US-015: User Form', ...)
```

---

## Example Executions

### Example 1: Run All Tests

```
User: /run-tests all

Claude:
🧪 Running Tests: all

Running: npm test
⏳ This may take a moment...

Running: npm run test:e2e
⏳ This may take a moment...

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ ALL TESTS PASSED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 TEST RESULTS:

Unit Tests:
  ✅ Files:  12/12 passed
  ✅ Tests:  248/248 passed
  ⏱️  Duration: 3.45s

E2E Tests:
  ✅ Tests:  15/15 passed
  ⏱️  Duration: 12.3s

TOTAL:
  ✅ 263/263 tests passed
  ⏱️  Total Duration: 15.75s

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

### Example 2: Run with Coverage

```
User: /run-tests coverage

Claude:
🧪 Running Tests: coverage

Running: npm run test:coverage
⏳ This may take a moment...

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ ALL TESTS PASSED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 TEST RESULTS:
  ✅ 248/248 tests passed

📈 CODE COVERAGE:

  Statements:  87.5% (350/400)
  Branches:    82.3% (141/171)
  Functions:   89.1% (82/92)
  Lines:       88.2% (338/383)

  ✅ Coverage Target Met (87.5% >= 80%)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

### Example 3: Tests Failed

```
User: /run-tests all

Claude:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
❌ TESTS FAILED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 TEST RESULTS:

Unit Tests:
  ❌ Files:  11/12 passed (1 failed)
  ❌ Tests:  246/248 passed (2 failed)

🔍 FAILED TESTS:

❌ src/components/UserForm.test.tsx
   Test: should validate email format
   Error: Expected "invalid" but received "valid"

   at line 45: expect(result).toBe('invalid')

   💡 Suggested Fix:
   The email validation logic may be incorrect.
   Check the validation regex in UserForm.tsx

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🛠️  NEXT STEPS:

1. Fix the 2 failing test(s) above
2. Run tests again: /run-tests all
3. When all pass, mark story as complete

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

**Version:** 1.0
**Created:** 2026-03-27
**Command Type:** Testing Utility
