# Frequently Asked Questions

**Version:** 3.3.0
**Last Updated:** 2026-04-21

---

## When to Read This File

**FAQ answers "how should I use this?"** — conceptual questions about workflow, choices, and tradeoffs. It's the right place when you're trying to decide *how* to approach something, not when something is broken.

| You are asking... | Read this |
|---|---|
| When should I use /execute-work vs manual? | **FAQ** (here) |
| What's the difference between phase/epic/story? | **FAQ** |
| Why mandatory plan mode? | **FAQ** |
| `/execute-work` returns "command not found" | [TROUBLESHOOTING.md](TROUBLESHOOTING.md) |
| Tests keep failing; how do I diagnose? | [TROUBLESHOOTING.md](TROUBLESHOOTING.md) |
| Progress files look stale | [TROUBLESHOOTING.md](TROUBLESHOOTING.md) |

**Rule of thumb:** "How do I…?" → FAQ. "Why doesn't X work?" → TROUBLESHOOTING.

---

## Quick Links

- **Troubleshooting (symptom → fix):** [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
- **Getting Started:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Commands Reference:** [COMMANDS-REFERENCE.md](COMMANDS-REFERENCE.md)

---

## Execution & Automation

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

## Configuration

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

## Git & Commits

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

## Phases & Workflow

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

## Quality & Testing

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

## Additional Resources

### Documentation
- **Troubleshooting:** [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
- **Getting Started:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Commands Reference:** [COMMANDS-REFERENCE.md](COMMANDS-REFERENCE.md)
- **Workflows:** [WORKFLOWS-BEST-PRACTICES.md](WORKFLOWS-BEST-PRACTICES.md)
- **Core Standards:** [../.CLAUDE.MD](../../.CLAUDE.MD)

### Support
- **GitHub Issues:** Report bugs or request features
- **Community:** Share experiences and tips
- **Updates:** Check [../CHANGELOG.md](../../CHANGELOG.md) for latest changes

---

**Version:** 3.3.0
**Last Updated:** 2026-04-21
**Part of:** Claude Project Management System v3.3
