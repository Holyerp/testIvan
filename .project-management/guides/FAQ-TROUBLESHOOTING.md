# FAQ & Troubleshooting

**Frequently asked questions and troubleshooting guide.**

**Version:** 3.0.0
**Last Updated:** 2026-03-27

---

## Table of Contents

1. [Frequently Asked Questions](#frequently-asked-questions)
2. [Troubleshooting](#troubleshooting)
3. [Migration from v2.0](#migration-from-v20)

---

## Frequently Asked Questions

### Q: When should I use `/execute-work` vs manual implementation?

**Use `/execute-work` when:**
- ✅ Stories are well-defined and clear
- ✅ You want automation and speed
- ✅ Quality gates should be enforced automatically
- ✅ Consistent commits and tracking needed
- ✅ Production-quality code required

**Use manual implementation when:**
- ✅ Exploring new architecture or patterns
- ✅ Learning new technology or framework
- ✅ Highly experimental or research work
- ✅ Prototyping and proof-of-concept
- ✅ Need maximum flexibility

**Hybrid approach (recommended):**
```bash
# Simple stories → Automated
/execute-work story US-001

# Complex stories → Manual
# Implement with TodoWrite
# Test with /run-tests
# Commit manually

# Entire epics → Automated (if stories simple)
/execute-work epic Epic-2
```

---

### Q: Can I stop `/execute-work` mid-execution?

**Yes! You have full control.**

**Option 1: Use paused mode**
```bash
/execute-work phase 1
Choose mode: [2] Paused

# After each story:
Continue with US-002? [Yes/No]
→ [No] - Stops execution cleanly

# Resume later:
/execute-work phase 1
→ Continues from where it stopped
```

**Option 2: Cancel during continuous mode**
```bash
# Press Ctrl+C or interrupt Claude
# Current story marked as "In Progress"
# Resume later with same command
```

**What happens when you stop:**
- Current story status saved
- Progress tracking updated
- Blockers documented (if any)
- Can resume anytime with same command

---

### Q: What if tests fail during `/execute-work`?

**Claude automatically handles test failures:**

```
1. Detects test failure
   → Story stays "in_progress"

2. Analyzes failure
   → Reads error messages
   → Identifies root cause

3. Fixes issues
   → Updates code
   → Addresses test failures

4. Re-runs tests
   → Validates fixes

5. Repeats until all pass
   → Only then marks story complete
```

**Story is NOT marked complete until tests pass!**

**If stuck in fix loop (rare):**
```bash
# Cancel execution
[No] (in paused mode) or Ctrl+C

# Debug manually
/run-tests coverage
→ See detailed failure info

# Fix the issues manually

# Resume execution
/execute-work story US-015
→ Continues with fixed code
```

---

### Q: How do I disable i18n?

**Option 1: During `/init-project` (best)**
```bash
/init-project
Configure i18n? [2] No - Skip i18n
```

**Option 2: After initialization (if already enabled)**
```bash
# Remove the i18n rules file
rm .project-management/rules/I18N-RULES.md

# i18n will no longer be enforced
# Existing translations remain but not required
```

**Option 3: Temporarily disable for one story**
```bash
# If I18N-RULES.md exists but you need to skip translations:
# Comment out i18n requirements temporarily
# Not recommended - breaks consistency
```

---

### Q: Can I customize the default stack?

**Yes! Multiple options:**

**Option 1: Choose custom during initialization**
```bash
/init-project
Choose tech stack: [3] Custom Setup
→ Answer 8 detailed questions
→ Full control over all choices
```

**Option 2: Edit technologies.md after initialization**
```bash
# Edit the technologies file
vim .project-management/input/technologies.md

# Make your changes (add/remove technologies)

# Regenerate documentation
/generate-docs

# Technical spec updated with new stack
```

**Option 3: Hybrid approach**
```bash
# Start with default stack
[1] Default HolyEstate Stack

# Later, add technologies to technologies.md
# Keep core stack, add extras (Redis, Elasticsearch, etc.)

# Regenerate docs
/generate-docs
```

---

### Q: What happens to git commits?

**Automatic commits (via `/execute-work`):**

**Format:**
```
feat: implement US-015 real-time notifications

- WebSocket server setup
- Notification component
- Database triggers
- Unit tests (8 passed)
- Integration tests (3 passed)
- E2E tests (1 passed)

Tests: 12/12 passed
Coverage: 89%
```

**Key features:**
- Created after each story
- Conventional commit format (feat:, fix:, refactor:, etc.)
- **NO AI credits** (per git.md rules)
- Include test results and coverage
- Reference story ID
- Descriptive and detailed

**Manual commits (manual workflow):**
```bash
# You create commits yourself
git add .
git commit -m "feat: implement US-015"
git push

# Follow same format as automatic commits
# NO AI credits
# Conventional commits
```

---

### Q: How do phases differ from sprints?

| Aspect | Sprints (v2.0) | Phases (v3.0) |
|--------|----------------|---------------|
| **Duration** | 2 weeks (fixed) | 1-4 months (flexible) |
| **Scope** | 10-20 stories | Multiple epics (50+ stories) |
| **Planning** | Manual planning each sprint | Automatic from backlog |
| **Focus** | Time-boxed iteration | Major milestone |
| **Velocity** | Points/sprint | Points/phase |
| **Flexibility** | Rigid 2-week cycle | Adapt to project needs |
| **Completion** | Sprint review | Phase completion report |

**Why phases are better:**
- Align with actual project milestones
- Reduce planning overhead
- More flexibility in timing
- Better for large projects
- Natural progression (Foundation → Features → Polish)

---

### Q: Can I use both `/execute-work` and manual implementation?

**Yes! Hybrid approach is recommended.**

```bash
# Day 1: Automated for simple stories
/execute-work story US-001
/execute-work story US-002

# Day 2: Manual for complex story
# Implement US-003 manually
# Test with /run-tests
# Commit manually

# Day 3: Automated for entire epic
/execute-work epic Epic-2

# Day 4: Manual debugging
/run-tests all
# Fix issues
# Continue with /execute-work
```

**Benefits:**
- Use the right tool for each task
- Automation where possible
- Manual control where needed
- Flexibility

---

### Q: What are quality gates and can I skip them?

**Quality gates are automatic checks:**
- ✅ All tests passing
- ✅ Coverage > 80%
- ✅ All API status codes tested
- ✅ i18n translations (if enabled)
- ✅ SOLID & DRY principles
- ✅ Git commit created
- ✅ Progress updated

**Can you skip them?**
- **With `/execute-work`:** No, enforced automatically
- **With manual workflow:** Yes, but not recommended

**Why gates matter:**
- Prevent technical debt
- Ensure consistent quality
- Catch issues early
- Maintain test coverage
- Production-ready code

---

### Q: How do I add more languages to i18n later?

**After initial setup:**

```bash
# 1. Edit I18N-RULES.md
vim .project-management/rules/I18N-RULES.md

# 2. Add new language to list
Supported Languages:
- en (English) - Primary
- de (German)
- es (Spanish) - NEW
- fr (French) - NEW

# 3. Update translation structure
Add files:
- src/i18n/translations/es.json
- src/i18n/translations/fr.json

# 4. Notify team
# New stories will include new languages
# Backfill existing translations as needed
```

---

## Troubleshooting

### Issue: Commands Not Working

**Symptom:** `/execute-work` or other commands not found

**Solution:**
```bash
# 1. Check if command file exists
ls .claude/commands/execute-work.md
ls .claude/commands/run-tests.md

# 2. If missing, copy from template
cp /path/to/claude_repo/.claude/commands/*.md .claude/commands/

# 3. Verify directory structure
ls -la .claude/commands/

# Expected:
# execute-work.md
# init-project.md
# run-tests.md
# update-progress.md
# project-status.md
# process-client-docs.md
# generate-docs.md

# 4. Restart Claude Code session
# Exit and restart Claude
```

---

### Issue: Tests Failing Repeatedly

**Symptom:** `/execute-work` stuck in test fix loop

**Diagnosis:**
```bash
# 1. Cancel execution
[No] in paused mode or Ctrl+C

# 2. Run tests manually to see detailed output
/run-tests coverage

# 3. Identify the issue
→ Read error messages carefully
→ Check test file: __tests__/...
→ Check implementation file: src/...
```

**Common causes:**

**A. Database connection issues**
```bash
# Check database running
docker ps | grep postgres

# Verify connection string
cat .env | grep DATABASE_URL

# Run migrations
npm run migrate
```

**B. Async/timeout issues**
```javascript
// Increase timeout in test
test('async operation', async () => {
  // ...
}, 10000) // 10 second timeout
```

**C. Missing test setup/teardown**
```javascript
// Add proper cleanup
afterEach(async () => {
  await prisma.user.deleteMany()
})
```

**D. Environment variables missing**
```bash
# Copy example env
cp .env.example .env.test

# Set test-specific values
NODE_ENV=test
```

**Solution:**
```bash
# 4. Fix the issue manually

# 5. Verify fix
/run-tests all

# 6. Resume execution if needed
/execute-work story US-015
```

---

### Issue: Plan Mode Taking Too Long

**Symptom:** Plan mode analyzing for > 5 minutes

**Possible causes:**
- Too many stories in scope (large phase/epic)
- Complex dependencies
- Large technical specification
- Many rules files to read

**Solution 1: Execute smaller scope**
```bash
# Instead of entire phase:
/execute-work phase 1

# Execute single epic:
/execute-work epic Epic-1

# Or single story:
/execute-work story US-001
```

**Solution 2: Simplify technical spec**
```bash
# Edit technical spec
vim .project-management/output/docs/technical-spec.md

# Break into smaller sections
# Remove redundant information
# Focus on current phase only
```

**Solution 3: Break epic into smaller epics**
```bash
# Edit backlog
vim .project-management/input/backlog.md

# Split large epic:
Epic-2: User Management (30 stories)
→ Epic-2A: User Authentication (10 stories)
→ Epic-2B: User Profiles (10 stories)
→ Epic-2C: User Settings (10 stories)

# Regenerate docs
/generate-docs
```

---

### Issue: Coverage Below 80%

**Symptom:** Tests pass but coverage < 80%

**Diagnosis:**
```bash
# 1. Run coverage report
/run-tests coverage

# 2. Open HTML report
open coverage/index.html

# 3. Identify uncovered files/functions
→ Red lines = not covered
→ Yellow lines = partially covered
→ Green lines = covered
```

**Solution:**
```bash
# 4. Add missing tests

# Example: Uncovered error handling
// Before: Only testing success case
test('creates user', async () => {
  const user = await createUser({ name: 'John' })
  expect(user.name).toBe('John')
})

// After: Test error cases too
test('creates user - validation error', async () => {
  await expect(createUser({ name: '' }))
    .rejects.toThrow('Name required')
})

test('creates user - duplicate email', async () => {
  await createUser({ email: 'test@test.com' })
  await expect(createUser({ email: 'test@test.com' }))
    .rejects.toThrow('Email exists')
})

# 5. Re-run coverage
/run-tests coverage

# 6. Repeat until > 80%
```

---

### Issue: i18n Translations Missing

**Symptom:** Quality gate fails due to missing translations

**Diagnosis:**
```bash
# Check I18N-RULES.md
cat .project-management/rules/I18N-RULES.md

# Expected languages listed
# Translation files should exist
```

**Solution:**
```bash
# 1. Check translation files exist
ls src/i18n/translations/
# en.json ✅
# de.json ✅
# es.json ❌ Missing!

# 2. Create missing translation file
touch src/i18n/translations/es.json

# 3. Add translations
{
  "welcome": "Bienvenido",
  "login": "Iniciar sesión",
  ...
}

# 4. Use translation keys in code
// Instead of:
<h1>Welcome</h1>

// Use:
<h1>{t('welcome')}</h1>

# 5. Verify all keys translated
npm run i18n:validate
```

---

### Issue: Git Commit Failing

**Symptom:** `/execute-work` fails at commit step

**Common causes:**

**A. Git not initialized**
```bash
git status
# fatal: not a git repository

# Solution:
git init
git add .
git commit -m "Initial commit"
```

**B. Git hooks failing**
```bash
# Pre-commit hook might be failing
# Check .git/hooks/pre-commit

# Temporarily disable (not recommended):
git commit --no-verify

# Better: Fix the hook issue
```

**C. No staged changes**
```bash
# If no files changed, commit fails
# This is usually correct behavior

# Verify story actually made changes:
git status
git diff
```

---

### Issue: Progress Not Updating

**Symptom:** `/project-status` shows outdated information

**Solution:**
```bash
# 1. Check if using /execute-work
# Progress updates automatically

# 2. If manual workflow, update manually
/update-progress

# 3. Verify progress files
ls .project-management/output/progress/
# current-status.md
# completed.md
# blockers.md

# 4. Check phase file
cat .project-management/output/phases/phase-1.md
# Verify story statuses correct

# 5. Regenerate status
/project-status
```

---

## Migration from v2.0

### Overview

Migrating from sprint-based v2.0 to phase-based v3.0.

### Migration Steps

**1. Backup existing project**
```bash
# Create backup
cp -r .project-management .project-management.backup
cp -r .claude .claude.backup
cp .CLAUDE.MD .CLAUDE.MD.backup
```

**2. Update system files**
```bash
# Copy new system files
cp -r /path/to/claude_repo/.project-management .
cp -r /path/to/claude_repo/.claude .
cp /path/to/claude_repo/.CLAUDE.MD .

# Keep your input files (scope, backlog, etc.)
cp .project-management.backup/input/* .project-management/input/
```

**3. Convert sprints to phases**
```bash
# Read your old sprint files
cat .project-management/output/sprints/sprint-1.md

# Organize stories into phases
# Edit backlog to group by epics
vim .project-management/input/backlog.md

# Regenerate with phases
/init-project
```

**4. Test new commands**
```bash
# Test execution
/execute-work story US-001
[2] Paused mode (for testing)

# Verify it works correctly
# Check tests run
# Check commit created
# Check progress updated
```

**5. Update workflows**
```bash
# Old: Manual workflow with TodoWrite
# New: Use /execute-work for automation

# Old: Manual testing
# New: /run-tests command

# Old: Manual git commits
# New: Automatic via /execute-work
```

### Breaking Changes

**Removed:**
- `/plan-sprint` command (replaced by `/execute-work`)
- Sprint-based structure (replaced by phases)
- Manual progress tracking requirement (automatic in `/execute-work`)

**Changed:**
- File locations (sprints/ → phases/)
- Workflow (manual → automated)
- Testing approach (manual npm test → /run-tests)

**Added:**
- `/execute-work` command
- `/run-tests` command
- Plan mode (mandatory)
- Tech stack selection
- i18n configuration

### Migration Guide

**Detailed guide:**
See `.project-management/docs/MIGRATION-GUIDE.md` (if exists)

---

## Additional Help

### Documentation
- **Getting Started:** `guides/GETTING-STARTED.md`
- **Commands Reference:** `guides/COMMANDS-REFERENCE.md`
- **Workflows:** `guides/WORKFLOWS-BEST-PRACTICES.md`
- **Core Standards:** `.CLAUDE.MD`
- **Integration Guide:** `docs/INTEGRATION-GUIDE.md`

### Support
- **GitHub Issues:** Report bugs or request features
- **Community:** Share experiences and tips
- **Updates:** Check CHANGELOG.md for latest changes

---

**Version:** 3.0.0
**Last Updated:** 2026-03-27
**Part of:** Claude Project Management System v3.0
