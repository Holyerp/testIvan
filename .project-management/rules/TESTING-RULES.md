# Project-Specific Testing Rules

> **Note:** This file contains project-specific testing requirements that extend the general testing rules in `.claude/rules/testing.md`. Include this file ONLY if your project has specific testing requirements beyond the defaults.

---

## When to Use This File

**Create this file if your project has:**
- ✅ Specific critical user flows that must be tested
- ✅ Custom testing patterns or requirements
- ✅ Unique test data management needs
- ✅ Special integration testing requirements
- ✅ Domain-specific test scenarios

**Skip this file if:**
- ❌ Default testing rules are sufficient
- ❌ No custom testing requirements exist
- ❌ General testing.md covers all your needs

**If you don't need custom testing rules:** Delete or ignore this file. Claude will use only the general testing rules from `.claude/rules/testing.md`.

---

## General Testing Requirements

**See `.claude/rules/testing.md` for:**
- Test coverage requirements (80%+ overall, 95%+ critical paths)
- API status code matrix (200/400/401/403/404/500)
- Test types (unit, functional, integration, e2e)
- Test organization and file structure
- Test completion criteria

---

## Project-Specific Critical Paths

**Define the critical user flows that MUST have E2E tests in YOUR project:**

### Example 1: {{CRITICAL_PATH_1_NAME}}

**Flow:** {{DESCRIBE_FLOW}} (e.g., User registration → Email verification → Login)

**Must test:**
- ✅ Happy path (all steps succeed)
- ✅ Validation errors
- ✅ Edge cases (duplicates, timeouts, invalid inputs)

**Test file location:** `{{TEST_FILE_PATH}}` (e.g., `tests/e2e/auth/registration.spec.ts`)

---

### Example 2: {{CRITICAL_PATH_2_NAME}}

**Flow:** {{DESCRIBE_FLOW}} (e.g., Product search → Add to cart → Checkout → Payment)

**Must test:**
- ✅ Happy path (complete purchase)
- ✅ Out of stock scenarios
- ✅ Cart calculations (subtotal, tax, shipping)
- ✅ Payment failures
- ✅ Inventory updates

**Test file location:** `{{TEST_FILE_PATH}}` (e.g., `tests/e2e/checkout/purchase-flow.spec.ts`)

---

### Example 3: {{CRITICAL_PATH_3_NAME}}

**Flow:** {{DESCRIBE_FLOW}} (e.g., Admin login → Create product → Publish)

**Must test:**
- ✅ Happy path (product published)
- ✅ Permission checks (non-admin denied)
- ✅ Validation errors
- ✅ Asset handling (images, files)
- ✅ Public visibility

**Test file location:** `{{TEST_FILE_PATH}}` (e.g., `tests/e2e/admin/product-management.spec.ts`)

---

**Add more critical paths as needed for your project.**

---

## Project-Specific Test Data Requirements

### Seed Data

**Required test data for your project:**

```typescript
// Example: E-commerce seed data
const testData = {
  users: [
    { email: 'test@example.com', role: 'customer' },
    { email: 'admin@example.com', role: 'admin' },
  ],
  products: [
    { name: 'Test Product', price: 9.99, stock: 100 },
  ],
  // Add your seed data structure
};
```

### Test Database Strategy

**Choose your approach:**

- ✅ **Isolated test DB** - Each test suite uses fresh database
- ✅ **Transaction rollback** - Rollback after each test
- ✅ **In-memory DB** - Fast, no cleanup needed
- ✅ **Fixtures** - Predefined data loaded before tests

**Your project uses:** {{TEST_DB_STRATEGY}}

---

## Custom Test Utilities

**Define custom helpers for YOUR project:**

```typescript
// Example: Authentication helper
export async function loginAsTestUser() {
  const response = await request(app)
    .post('/api/auth/login')
    .send({ email: 'test@example.com', password: 'password123' });

  return response.body.token;
}

// Example: Database cleanup
export async function cleanupTestData() {
  await db.users.deleteMany({ email: { $regex: /test.*@example\.com/ } });
}
```

**Your custom helpers:**
```typescript
// Add your project-specific test helpers here
{{CUSTOM_TEST_HELPERS}}
```

---

## Integration Test Requirements

### External Services

**List external services your project integrates with:**

| Service | Purpose | Test Strategy | Mock/Real |
|---------|---------|---------------|-----------|
| {{SERVICE_1}} | {{PURPOSE}} | {{STRATEGY}} | Mock |
| {{SERVICE_2}} | {{PURPOSE}} | {{STRATEGY}} | Real (test env) |

**Example:**

| Service | Purpose | Test Strategy | Mock/Real |
|---------|---------|---------------|-----------|
| Stripe | Payment processing | Mock API responses | Mock |
| SendGrid | Email delivery | Check queue, don't send | Mock |
| AWS S3 | File storage | Use test bucket | Real (test bucket) |

---

## Performance Test Requirements

**Define performance benchmarks for YOUR project:**

| Endpoint | Max Response Time | Concurrent Users | Success Rate |
|----------|-------------------|------------------|--------------|
| {{ENDPOINT_1}} | {{TIME}}ms | {{USERS}} | 99%+ |
| {{ENDPOINT_2}} | {{TIME}}ms | {{USERS}} | 99%+ |

**Example:**

| Endpoint | Max Response Time | Concurrent Users | Success Rate |
|----------|-------------------|------------------|--------------|
| GET /api/products | 200ms | 100 | 99.9% |
| POST /api/orders | 500ms | 50 | 99.5% |

---

## Security Test Requirements

### Authentication Tests

**Project-specific auth tests:**
- [ ] Brute force protection (rate limiting)
- [ ] JWT token expiration
- [ ] Refresh token rotation
- [ ] Session timeout
- [ ] Multi-factor authentication (if applicable)

### Authorization Tests

**Role-based access control:**

| Role | Can Access | Cannot Access |
|------|------------|---------------|
| {{ROLE_1}} | {{ENDPOINTS}} | {{ENDPOINTS}} |
| {{ROLE_2}} | {{ENDPOINTS}} | {{ENDPOINTS}} |

**Example:**

| Role | Can Access | Cannot Access |
|------|------------|---------------|
| Customer | Own orders, profile | Admin panel, other users' data |
| Admin | All endpoints | N/A |

### Data Security Tests

- [ ] SQL injection prevention
- [ ] XSS prevention (input sanitization)
- [ ] CSRF token validation
- [ ] File upload validation (type, size, content)
- [ ] Sensitive data masking (passwords, tokens)

---

## Test Completion Checklist

**Before marking ANY feature complete, verify:**

### Unit Tests
- [ ] All business logic functions tested
- [ ] Edge cases covered
- [ ] Error scenarios validated
- [ ] 80%+ code coverage

### Integration Tests
- [ ] All API endpoints tested
- [ ] All status codes tested (200/400/401/403/404/500)
- [ ] Database interactions validated
- [ ] External service integrations mocked

### E2E Tests
- [ ] All critical paths tested (listed above)
- [ ] Happy path + error scenarios
- [ ] Cross-browser testing (if web app)
- [ ] Mobile responsiveness (if applicable)

### Performance Tests
- [ ] Response time benchmarks met
- [ ] Load tests passed
- [ ] Memory leaks checked

### Security Tests
- [ ] Authentication tests passed
- [ ] Authorization tests passed
- [ ] Input validation tests passed

---

## Test Execution

### Local Development

```bash
# Run all tests
{{TEST_COMMAND_ALL}}

# Run unit tests only
{{TEST_COMMAND_UNIT}}

# Run integration tests
{{TEST_COMMAND_INTEGRATION}}

# Run e2e tests
{{TEST_COMMAND_E2E}}

# Run with coverage
{{TEST_COMMAND_COVERAGE}}
```

**Example:**
```bash
npm test                    # All tests
npm run test:unit          # Unit tests
npm run test:integration   # Integration tests
npm run test:e2e           # E2E tests
npm run test:coverage      # With coverage report
```

### CI/CD Pipeline

**Test stages in CI:**

1. **Stage 1: Unit Tests** (fastest feedback)
2. **Stage 2: Integration Tests** (if unit tests pass)
3. **Stage 3: E2E Tests** (if integration tests pass)
4. **Stage 4: Performance Tests** (before deployment)

**Fail conditions:**
- Any test fails → Block merge
- Coverage drops below 80% → Block merge
- Performance benchmarks not met → Warning (don't block)

---

## Troubleshooting

### Common Issues

**Test database not cleaned:**
```bash
# Solution: Reset test database
{{RESET_DB_COMMAND}}
```

**E2E tests flaky:**
- Add explicit waits for async operations
- Increase timeout for slow operations
- Use retry logic for network-dependent tests

**Coverage not meeting threshold:**
- Check for untested edge cases
- Add tests for error handling
- Test async code paths

---

## Resources

- General testing rules: `.claude/rules/testing.md`
- Test framework docs: {{TEST_FRAMEWORK_DOCS}}
- CI/CD configuration: {{CI_CONFIG_FILE}}

---

**Last Updated:** {{DATE}}
