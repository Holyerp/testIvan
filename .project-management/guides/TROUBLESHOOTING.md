# Troubleshooting Guide

**Common issues and solutions for the Claude Project Management System v3.0**

**Version:** 3.2.0
**Last Updated:** 2026-04-21

---

## Quick Links

- **FAQ:** [FAQ.md](FAQ.md)
- **Getting Started:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Migration from v2.0:** [Migration section](#migration-from-v20) (below)

---

## Table of Contents

1. [Commands Not Working](#issue-commands-not-working)
2. [Tests Failing Repeatedly](#issue-tests-failing-repeatedly)
3. [Plan Mode Taking Too Long](#issue-plan-mode-taking-too-long)
4. [Coverage Below 80%](#issue-coverage-below-80)
5. [i18n Translations Missing](#issue-i18n-translations-missing)
6. [Git Commit Failing](#issue-git-commit-failing)
7. [Progress Not Updating](#issue-progress-not-updating)
8. [Migration from v2.0](#migration-from-v20)

---

## Issue: Commands Not Working

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
# project-status.md
# process-client-docs.md
# generate-docs.md

# 4. Restart Claude Code session
# Exit and restart Claude
```

---

## Issue: Tests Failing Repeatedly

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

### A. Database connection issues
```bash
# Check database running
docker ps | grep postgres

# Verify connection string
cat .env | grep DATABASE_URL

# Run migrations
npm run migrate
```

### B. Async/timeout issues
```javascript
// Increase timeout in test
test('async operation', async () => {
  // ...
}, 10000) // 10 second timeout
```

### C. Missing test setup/teardown
```javascript
// Add proper cleanup
afterEach(async () => {
  await prisma.user.deleteMany()
})
```

### D. Environment variables missing
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

## Issue: Plan Mode Taking Too Long

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
vim .project-management/input/backlog/phase-1-foundation.md   # (or relevant phase file)

# Split large epic:
Epic-2: User Management (30 stories)
→ Epic-2A: User Authentication (10 stories)
→ Epic-2B: User Profiles (10 stories)
→ Epic-2C: User Settings (10 stories)

# Regenerate docs
/generate-docs
```

---

## Issue: Coverage Below 80%

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

## Issue: i18n Translations Missing

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

## Issue: Git Commit Failing

**Symptom:** `/execute-work` fails at commit step

**Common causes:**

### A. Git not initialized
```bash
git status
# fatal: not a git repository

# Solution:
git init
git add .
git commit -m "Initial commit"
```

### B. Git hooks failing
```bash
# Pre-commit hook might be failing
# Check .git/hooks/pre-commit

# Temporarily disable (not recommended):
git commit --no-verify

# Better: Fix the hook issue
```

### C. No staged changes
```bash
# If no files changed, commit fails
# This is usually correct behavior

# Verify story actually made changes:
git status
git diff
```

---

## Issue: Progress Not Updating

**Symptom:** `/project-status` shows outdated information

**Solution:**
```bash
# 1. Check if using /execute-work
# Progress updates automatically

# 2. If manual workflow, edit progress files directly
#    (the /update-progress command was removed in v3.2.0)

# 3. Verify progress files
ls .project-management/output/progress/
# DASHBOARD.md
# daily-summary.md
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
vim .project-management/input/backlog/phase-1-foundation.md   # (or relevant phase file)

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
See [../docs/MIGRATION-GUIDE.md](../docs/MIGRATION-GUIDE.md) (if exists)

---

## Additional Help

### Documentation
- **FAQ:** [FAQ.md](FAQ.md)
- **Getting Started:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Commands Reference:** [COMMANDS-REFERENCE.md](COMMANDS-REFERENCE.md)
- **Workflows:** [WORKFLOWS-BEST-PRACTICES.md](WORKFLOWS-BEST-PRACTICES.md)
- **Core Standards:** [../.CLAUDE.MD](../.CLAUDE.MD)
- **Integration Guide:** [../INTEGRATION-GUIDE.md](../INTEGRATION-GUIDE.md)

### Support
- **GitHub Issues:** Report bugs or request features
- **Community:** Share experiences and tips
- **Updates:** Check [../CHANGELOG.md](../CHANGELOG.md) for latest changes

---

**Version:** 3.2.0
**Last Updated:** 2026-04-21
**Part of:** Claude Project Management System v3.2
