---
name: run-tests
description: Run tests manually with detailed reporting and coverage analysis
---

# Run Tests Command

Execute manual testing with comprehensive reporting and analysis.

---

## Usage

```bash
/run-tests all              # Run all tests (unit + integration + E2E)
/run-tests unit             # Unit tests only
/run-tests integration      # Integration tests only
/run-tests e2e              # E2E tests only
/run-tests coverage         # Run with coverage report
/run-tests story US-XXX     # Tests for specific story
/run-tests file <path>      # Tests for specific file
```

---

## 📋 YOUR TASK

### STEP 1: PARSE ARGUMENTS

**Parse the command argument:**
- `all` → Run unit, integration, and E2E tests
- `unit` → Run only unit tests
- `integration` → Run only integration tests
- `e2e` → Run only E2E tests
- `coverage` → Run all with coverage report
- `story US-XXX` → Run tests tagged/related to story US-XXX
- `file <path>` → Run tests for specific file

---

### STEP 2: EXECUTE TESTS

**📖 See:** `modules/run-tests-execution.md` for detailed test execution

**Summary by test type:**

**Unit Tests (`npm test` or `npm run test:unit`):**
- Test individual functions/components
- Fast execution
- No external dependencies

**Integration Tests (`npm run test:integration`):**
- Test API endpoints
- Database interactions
- Must test ALL status codes (200/400/401/403/404/500)

**E2E Tests (`npm run test:e2e` or `npx playwright test`):**
- Test user flows
- Browser automation
- Full application testing

**Coverage (`npm run test:coverage`):**
- Run all tests with coverage
- Generate HTML report
- Check coverage threshold (80%+)

---

### STEP 3: ANALYZE RESULTS

**📖 See:** `modules/run-tests-reporting.md` for detailed reporting

**Capture test output:**
- Total tests run
- Passed / Failed counts
- Execution time
- Coverage percentage
- Failed test details

**Display results:**
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🧪 TEST RESULTS
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 SUMMARY:
✅ Unit Tests: {{passed}}/{{total}} passed
{{✅ Integration Tests: {{passed}}/{{total}} passed}}
{{✅ E2E Tests: {{passed}}/{{total}} passed}}
⏱️  Duration: {{duration}}
📈 Coverage: {{coverage}}% {{(Target: 80%+)}}

{{If all passed:}}
✅ ALL TESTS PASSED

{{If any failed:}}
❌ FAILED TESTS:
- {{test_name}}: {{error_message}}
- {{test_name}}: {{error_message}}
```

---

### STEP 4: VALIDATION CHECKS

**Verify quality gates:**

**Code Coverage:**
- [ ] Overall coverage > 80%
- [ ] Branch coverage > 75%
- [ ] Function coverage > 85%

**API Testing (Integration):**
- [ ] All endpoints have tests
- [ ] All status codes tested (200/400/401/403/404/500)
- [ ] Error handling tested

**Critical Paths (E2E):**
- [ ] Authentication flow tested
- [ ] Main user journeys tested
- [ ] Payment/checkout flow tested (if applicable)

---

### STEP 5: RECOMMENDATIONS

**If tests failed:**
- List failed tests with file locations
- Suggest potential fixes
- Recommend running specific test suites

**If coverage < 80%:**
- Identify uncovered files/functions
- Suggest where to add tests
- Show coverage gaps

**If API codes missing:**
- List endpoints missing status code tests
- Provide test examples
- Link to testing.md requirements

---

## 📚 Module References

**Detailed workflows available in:**
- `modules/run-tests-execution.md` - Test execution details
- `modules/run-tests-reporting.md` - Reporting & analysis

---

## ⚠️ IMPORTANT NOTES

### Test Requirements

**From `.claude/rules/testing.md`:**
- 80%+ coverage mandatory
- All API status codes must be tested
- Critical paths must have E2E tests

**From `.project-management/rules/TESTING-RULES.md` (if exists):**
- Project-specific testing requirements
- Custom test organization
- Additional quality gates

### Quality Gates

**Tests must pass before:**
- Marking story as complete
- Creating git commit
- Merging to main branch
- Deploying to production

### Test Organization

**File naming:**
- Unit: `ComponentName.test.tsx` or `function-name.test.ts`
- Integration: `api-endpoint.integration.test.ts`
- E2E: `user-flow.e2e.test.ts`

**Test location:**
- Unit: Co-located with code or `__tests__/unit/`
- Integration: `__tests__/integration/`
- E2E: `__tests__/e2e/` or `e2e/`

---

## 📝 Example Execution

```bash
# User runs:
/run-tests all

# Claude executes:
🧪 Running All Tests...

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🧪 TEST RESULTS
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 SUMMARY:
✅ Unit Tests: 45/45 passed
✅ Integration Tests: 18/18 passed
✅ E2E Tests: 6/6 passed
⏱️  Duration: 12.3s
📈 Coverage: 87% (Target: 80%+)

✅ ALL TESTS PASSED

✅ QUALITY GATES:
- Coverage: ✅ 87% (Target: 80%+)
- API Status Codes: ✅ All tested
- Critical Paths: ✅ E2E tests passing

🎉 Ready to commit!
```

---

**Version:** 2.0 (Modular)
**Created:** 2026-03-27
**Command Type:** Testing & Quality Assurance
