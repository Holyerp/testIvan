# Execute Work - Quality Gates Module

**Referenced by:** `execute-work-implementation.md` Step 3.6 & 3.7

---

## Validation Gate (Step 3.7)

**Triggered after:** Running all tests (Step 3.6)

---

### Validation Checks

**IF ANY test failed OR coverage < 80% OR i18n missing (when required):**

**Display:**
```
⚠️ VALIDATION FAILED for US-XXX

Issues:
- [List failed tests]
- [Coverage: XX% (need 80%+)]
- [Missing i18n translations: ...]

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

6. **REPEAT** until ALL tests pass AND coverage > 80% AND i18n complete

---

### CRITICAL Rules

**Story is NOT complete until:**
- ✅ All tests passing (unit, integration, E2E)
- ✅ Coverage > 80%
- ✅ All API status codes tested (200/400/401/403/404/500)
- ✅ i18n translations present (if required)
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
- [ ] Coverage > 80%
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
- [ ] API docs updated (if API changes)
- [ ] README updated (if user-facing changes)
- [ ] Comments added for complex logic

### Security
- [ ] No secrets committed
- [ ] No security vulnerabilities
- [ ] Input validation present
- [ ] OWASP Top 10 checked

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
