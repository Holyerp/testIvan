# Execute Work - Quality Gates Module

**Referenced by:** `execute-work-implementation.md` Step 3.6 & 3.7

---

## Validation Gate (Step 3.7)

**Triggered after:** Running all tests (Step 3.6)

---

### Validation Checks

**IF ANY test failed OR coverage < 80% OR i18n missing (when required) OR API docs gate failed (when endpoints touched):**

**Display:**
```
⚠️ VALIDATION FAILED for US-XXX

Issues:
- [List failed tests]
- [Coverage: XX% (need 80%+)]
- [Missing i18n translations: ...]
- [API docs gate: missing schema / missing doc block / drift between code and doc]

🔧 Fixing issues...
```

---

### Fix Loop

**Execute these steps until validation passes:**

1. **Analyze test failures**
   - Read error messages
   - Identify root cause
   - Check related code

2. **Fix bugs in code**
   - Update implementation
   - Follow SOLID & DRY principles
   - Test locally

3. **Add missing tests for coverage**
   - Write additional unit tests
   - Cover edge cases
   - Test error scenarios

4. **Add missing i18n translations** (if I18N-RULES.md exists)
   - Find hardcoded text
   - Add translation keys
   - Update all language JSON files

5. **Re-run tests**
   - Run full test suite
   - Check coverage report
   - Verify all status codes tested

6. **Verify API documentation gate** (only if story touched any HTTP endpoint)
   - Per `.claude/rules/api-documentation.md`: schema validation in code, typed response, doc block per `documentation-templates.md` §2.1, drift check
   - STRICT for public endpoints; SOFT for `@internal`-tagged handlers
   - Mismatch between schema / response type / docs / tests is a blocker — fix before continuing

7. **REPEAT** until ALL tests pass AND coverage ≥ 80% AND i18n complete AND API docs gate clean

---

### CRITICAL Rules

**Story is NOT complete until:**
- ✅ All tests passing (unit, integration, E2E)
- ✅ Coverage ≥ 80%
- ✅ All API status codes tested (200/400/401/403/404/500)
- ✅ i18n translations present (if required)
- ✅ API documentation gate clean (if endpoints touched) — see `.claude/rules/api-documentation.md`
- ✅ SOLID & DRY principles followed
- ✅ No linting errors

---

### Validation Passed

**IF ALL tests passed AND coverage OK AND i18n OK:**

**Display:**
```
✅ VALIDATION PASSED for US-XXX

All checks completed:
✅ Tests: {{X}}/{{X}} passed
✅ Coverage: {{XX}}% (Target: 80%+)
✅ API Status Codes: All tested
{{✅ i18n: All languages present}}
{{✅ API Docs Gate: schema + typed response + doc block aligned}}
✅ Code Quality: SOLID & DRY compliant
```

**Mark "Run all tests" todo as completed**

**Proceed to:** Git commit (Step 3.8)

---

## Quality Gates Checklist

**Before marking story complete:**

### Code Quality
- [ ] SOLID & DRY principles followed
- [ ] No TypeScript/linting errors
- [ ] Follows project conventions
- [ ] No over-engineering
- [ ] No unused code

### Testing
- [ ] All tests passing (unit, integration, e2e)
- [ ] Coverage ≥ 80%
- [ ] All API codes tested (200/400/401/403/404/500)
- [ ] Edge cases covered
- [ ] Error scenarios tested

### i18n (Conditional - if I18N-RULES.md exists)
- [ ] No hardcoded user-facing text
- [ ] All text uses translation keys
- [ ] Translation files updated for all languages
- [ ] Keys follow naming convention

### Documentation
- [ ] Tech spec consulted
- [ ] README updated (if user-facing changes)
- [ ] Comments added for complex logic

### Frontend (Web/Mobile) Gate (Conditional — only if story is a frontend story)
Refs: `.claude/rules/api-first.md`, `.claude/rules/screen-driven-backlog.md`

- [ ] Story scoped to one screen (or wizard with all steps enumerated)
- [ ] Story title follows `Screen — Action` pattern
- [ ] **API Endpoints Used** table present (method + path + purpose + doc reference)
- [ ] Phase A contract verification ✅ — every UI input maps to request schema, every UI output maps to response shape, error states distinguishable, auth matches
- [ ] If gaps were found at plan time: backend story/bug filed, frontend resumed only after gap closed
- [ ] No invented response shapes, no stubs masking missing fields

### API Documentation Gate (Conditional — only if HTTP endpoints touched)
Ref: `.claude/rules/api-documentation.md` + `.claude/rules/api-versioning.md`

**Public / client-facing endpoints (STRICT):**
- [ ] Endpoint under `/api/v{N}/...` (per `.claude/rules/api-versioning.md` §2)
- [ ] Request body / params / query validated by schema (Zod/Joi/etc.) at handler boundary
- [ ] Response shape declared as typed interface
- [ ] Doc block exists per `.claude/rules/documentation-templates.md` §2.1 (or OpenAPI entry if project uses it)
- [ ] All status codes from `.claude/rules/testing.md` matrix documented
- [ ] Allowed enum values listed in doc for any enum-typed field (per `.claude/rules/enums-and-constants.md`)
- [ ] No drift: field names + status codes match between schema, response type, docs, tests

**If the change modifies an existing endpoint (per `.claude/rules/api-versioning.md` §5):**
- [ ] Doc updated in the same commit (request/response examples, status codes, "Last Updated")
- [ ] Zod request + response schemas updated (single source of truth, not duplicated)
- [ ] **ALL tests touching this endpoint re-run** and pass — not just new tests. Use `grep -rln "<path>\|<handlerName>" tests/` to find them.
- [ ] No test was "fixed" by mirroring the new shape without confirming the change is non-breaking per §3.2
- [ ] If breaking change (§3.1) → new major version path (`v{N+1}`), old version doc marked Deprecated with sunset date, `Deprecation: true` + `Sunset` headers emitted on old version
- [ ] In-repo consumer code (frontend / mobile) updated for new shape

**Internal endpoints (SOFT — handler tagged `@internal`):**
- [ ] `@internal` JSDoc/TSDoc tag present, explains caller + input/output
- [ ] Handler guarded against malformed input
- [ ] Even for internal endpoints, the change-propagation gate (`.claude/rules/api-versioning.md` §5) still applies to internal callers

### Error Handling & Logging Gate (Conditional — only if handler / service / logger config touched)
Ref: `.claude/rules/error-handling-and-logging.md`

- [ ] All thrown errors are `AppError` subclasses (or known library errors mapped at the single boundary)
- [ ] Error responses follow the canonical envelope (`success: false`, `error`, `code`)
- [ ] Every new error `code` is `SCREAMING_SNAKE_CASE` and listed in the endpoint doc (per `.claude/rules/enums-and-constants.md`)
- [ ] No `catch` block silently swallows; either visible recovery or `log + rethrow`
- [ ] Structured logger used (no `console.log` in production paths); single log at the boundary, not duplicated across layers
- [ ] No PII / secrets / full auth/payment bodies logged; redaction config covers any new sensitive keys (per `.claude/rules/anonymization.md`)
- [ ] `request_id` propagated through async boundaries and returned in `X-Request-Id` response header
- [ ] Tests cover every error path produced by this change (status, `code`, envelope); redaction config has a test

### Security & Auth Gate
Ref: `.claude/rules/security-and-auth.md`

- [ ] New route uses `requireAuth` (and `requireRole` if applicable), unless explicitly public-allowlisted (default deny)
- [ ] Resource-level ownership/role check present where path/body refers to a resource (IDOR prevention)
- [ ] No plaintext password, token, or session value in logs, error responses, or test fixtures
- [ ] Zod validates body / params / query at handler boundary
- [ ] Cookie config: `httpOnly: true`, `secure` in prod, `sameSite` set, `maxAge` set, `secrets: [env.SESSION_SECRET]`
- [ ] Security headers present (CSP, HSTS, X-Frame-Options, X-Content-Type-Options, Referrer-Policy)
- [ ] New env vars validated by Zod env schema and added to `.env.example`
- [ ] Auth-route changes covered by tests: 401, 403, resource-level (IDOR), rate-limit
- [ ] Security events emitted to audit log (login success/failure, role change, permission denial) per §7.1
- [ ] `npm audit` clean — no new `high`/`critical` advisories
- [ ] No secrets committed; OWASP Top 10 considerations reviewed

---

## Error Handling Strategies

### Test Failures

**Common issues:**
- **Import errors:** Check file paths, ensure modules exist
- **Type errors:** Verify TypeScript types, check interfaces
- **Assertion failures:** Review expected vs actual, update logic
- **Timeout errors:** Increase timeout, optimize code, check async/await

**Resolution:**
1. Read full error stack trace
2. Locate failing test and code
3. Fix root cause
4. Re-run specific test
5. Run full suite when fixed

---

### Coverage < 80%

**Common issues:**
- Missing branch coverage (if/else not both tested)
- Missing edge cases (null, undefined, empty arrays)
- Missing error handling tests
- Untested utility functions

**Resolution:**
1. Run `npm run test:coverage` for detailed report
2. Check uncovered lines in HTML report
3. Write tests for uncovered code
4. Focus on critical paths first
5. Re-run coverage check

---

### Missing i18n

**Common issues:**
- Hardcoded strings in JSX: `<h1>Welcome</h1>`
- Hardcoded strings in error messages
- Missing translation keys in JSON files
- Keys not synced across languages

**Resolution:**
1. Search for hardcoded text: `grep -r "\"[A-Z]" src/`
2. Replace with `{t('key')}`
3. Add key to all language files
4. Follow naming convention: `section.subsection.key`
5. Verify all languages have the key

---

## Next Step

**After validation passes:**
- Return to `execute-work-implementation.md` Step 3.8 (Git Commit)
