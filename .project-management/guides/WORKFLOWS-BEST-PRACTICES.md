# Workflows & Best Practices

**Recommended workflows and best practices for optimal project execution.**

**Version:** 3.0.0
**Last Updated:** 2026-03-27

---

## Table of Contents

1. [Core Concepts](#core-concepts)
2. [Workflows](#workflows)
3. [Best Practices](#best-practices)

---

## Core Concepts

### Phase-Based Structure

```
Project
  └── Phase (1-4 months, major milestone)
      └── Epic (group of related features)
          └── User Story (specific functionality)
              └── Task (implementation steps)
```

**4 Standard Phases:**

1. **Phase 1: Foundation & Setup** (1-2 months)
   - Project setup, infrastructure, authentication
   - Database setup, API foundation
   - CI/CD pipeline
   - **Focus:** Strong foundation for future development

2. **Phase 2: Core Features** (2-3 months)
   - Main product features (P0 priority)
   - User-facing functionality
   - Core business logic
   - **Focus:** Minimum viable product (MVP)

3. **Phase 3: Advanced Features** (2 months)
   - Secondary features (P1 priority)
   - Integrations
   - Admin features
   - **Focus:** Product differentiation

4. **Phase 4: Polish & Launch** (1 month)
   - Optimization, testing, deployment
   - Bug fixes, performance tuning
   - Launch preparation
   - **Focus:** Production readiness

---

### Plan Mode

**What is Plan Mode:**
A mandatory pre-implementation phase where Claude analyzes requirements, creates a detailed plan, and waits for user approval before writing code.

**When It Activates:**
- Automatically when running `/execute-work`
- Automatically when running `/init-project`
- For any command with "plan" in the name

**Plan Mode Workflow:**
```
1. READ ALL CONTEXT
   ✅ Technical spec (.project-management/output/docs/technical-spec.md)
   ✅ Backlog (.project-management/input/backlog.md)
   ✅ Core standards (.CLAUDE.MD)
   ✅ All rules files (.claude/rules/*.md)

2. ANALYZE SCOPE
   → Stories in scope
   → Dependencies between stories
   → Potential risks
   → Test requirements

3. CREATE DETAILED PLAN
   → Tasks for each story
   → Test strategy (unit, integration, E2E)
   → Files to create/modify
   → Estimated effort

4. WAIT FOR APPROVAL
   → Show plan to user
   → Options: [Yes/No/Revise]

5. START IMPLEMENTATION
   → Only after explicit approval
   → Follow approved plan
```

**Why It's Mandatory:**
- Prevents wrong implementations
- Ensures alignment with requirements
- Validates understanding before coding
- Reduces rework and mistakes

---

### Quality Gates

**Story is NOT complete until all gates pass:**

**✅ Testing Gates:**
- All tests passing (unit, integration, E2E)
- Coverage > 80%
- All API status codes tested (200/400/401/403/404/500)

**✅ Code Quality Gates:**
- SOLID principles followed
- DRY (Don't Repeat Yourself) applied
- No linting errors
- Code review passed (if team process)

**✅ Documentation Gates:**
- i18n translations added (if enabled)
- Comments for complex logic
- API documentation updated

**✅ Process Gates:**
- Git commit created (NO AI credits)
- Progress tracking updated
- Blockers documented (if any)

---

## Workflows

### Workflow 1: Automated Phase Execution

**Best for:** Continuous development, automation lovers, well-defined requirements

**Time saved:** ~90% compared to manual workflow

**Process:**
```bash
# 1. Initialize (one-time setup)
/init-project
→ Select tech stack
→ Configure i18n
→ Generate documentation

# 2. Execute entire phase
/execute-work phase 1
→ Choose execution mode: [1] Continuous
→ Approve plan: [Yes]

# Claude now automatically:
# ✅ Implements all stories in Phase 1
# ✅ Runs tests after each story
# ✅ Creates git commits (NO AI credits)
# ✅ Updates progress tracking
# ✅ Continues to next story without pausing

# 3. When Phase 1 complete, continue
/execute-work phase 2
→ Repeat for all phases
```

**Advantages:**
- Fully automated development
- Consistent quality gates
- Automatic testing and commits
- Real-time progress tracking
- Minimal manual intervention

**Disadvantages:**
- Less control over individual stories
- Requires well-defined requirements
- Harder to review each change

**Recommended for:**
- Experienced teams
- Clear requirements
- Trust in automation
- Fast-paced projects

---

### Workflow 2: Controlled Story-by-Story

**Best for:** Learning the system, reviewing each story, complex features, tight oversight

**Time saved:** ~60% compared to manual workflow

**Process:**
```bash
# 1. Initialize
/init-project

# 2. Execute one story at a time
/execute-work story US-001
→ Choose execution mode: [2] Paused
→ Approve plan: [Yes]

# Claude implements US-001:
# ✅ Implements code
# ✅ Writes tests
# ✅ Runs tests
# ✅ Creates git commit
# ✅ Updates progress

# 3. Claude asks permission
Continue with US-002? [Yes/No]

# 4. You control the pace
[Yes] - Continue with next story
[No] - Stop execution (resume later)

# 5. Resume later if needed
/execute-work story US-002
→ Continues where you left off
```

**Advantages:**
- Full control over each story
- Review code before continuing
- Easy to pause and resume
- Learn system incrementally
- Catch issues early

**Disadvantages:**
- Slower than continuous mode
- Requires more user interaction
- Can feel repetitive

**Recommended for:**
- New users learning the system
- Complex/risky features
- Projects requiring tight review
- Prototyping and experimentation

---

### Workflow 3: Manual Testing & Implementation

**Best for:** Test-driven development (TDD), debugging tests, learning, experimentation

**Time saved:** ~30% (mostly from test automation)

**Process:**
```bash
# 1. Implement manually (without /execute-work)
# → Use TodoWrite for task breakdown
# → Write code following standards
# → Commit as you go

# 2. Run tests manually whenever needed
/run-tests all
→ See results immediately
→ Debugging information

# 3. If failures, fix and rerun
/run-tests coverage
→ Check which lines uncovered
→ Add missing tests

# 4. When all pass, update progress
/update-progress
→ Mark stories complete
→ Document achievements

# 5. Create git commit manually
git add .
git commit -m "feat: implement US-001"
git push
```

**Advantages:**
- Full manual control
- Learn by doing
- Flexible approach
- Good for TDD
- Debugging friendly

**Disadvantages:**
- Slower than automation
- Manual progress tracking
- More room for human error
- No automatic quality gates

**Recommended for:**
- TDD practitioners
- Debugging complex tests
- Learning new technologies
- Highly experimental work

---

### Workflow 4: Hybrid Approach

**Best for:** Most projects, balanced control, mixed complexity

**Time saved:** ~70% compared to manual workflow

**Process:**
```bash
# 1. Use /execute-work for simple, well-defined stories
/execute-work story US-001
→ [1] Continuous mode
→ Fully automated

# 2. Manual implementation for complex/experimental stories
# → Implement with TodoWrite
# → Test with /run-tests
# → Commit manually
# → Update progress with /update-progress

# 3. Automated for entire epics of simple stories
/execute-work epic Epic-3
→ [1] Continuous mode
→ Multiple stories automated

# 4. Paused mode for medium complexity
/execute-work epic Epic-4
→ [2] Paused mode
→ Review after each story

# 5. Check status regularly
/project-status
→ Weekly status reports
→ Adjust approach as needed
```

**Advantages:**
- Best of both worlds
- Adapt to story complexity
- Maintain control where needed
- Speed where possible
- Flexibility

**Disadvantages:**
- Requires judgment on when to automate
- Mixed workflow can be inconsistent
- Needs more planning

**Recommended for:**
- Most production projects
- Teams with mixed experience
- Projects with varying complexity
- Balanced control and speed

---

## Best Practices

### 1. Always Use Plan Mode

**Why:** Prevents wrong implementations, ensures alignment with standards

**How:**
```bash
# Plan mode activates automatically:
/execute-work story US-015

# Plan mode shows:
→ What will be implemented
→ Which tests will be written
→ Which files will be modified
→ Estimated effort
→ Potential risks

# Review carefully before approving
[Yes] - Implementation matches expectations
[No] - Cancel and clarify requirements
[Revise] - Modify plan before proceeding
```

**Tips:**
- Read the entire plan before approving
- Check file paths are correct
- Verify test strategy is comprehensive
- Ensure i18n handling (if enabled)
- Validate dependencies are understood

---

### 2. Enable i18n Early (If Needed)

**If your project needs multi-language support:**

```bash
# During /init-project
Configure i18n? [1] Yes

# Select languages early
→ English (en) - Required, always included
→ German (de) - Optional
→ Spanish (es) - Optional
→ French (fr) - Optional
→ Serbian (sr) - Optional
→ [Add more as needed]

# Why early is better:
✅ Easier to add translations as you code
✅ Prevents refactoring later
✅ Enforced by quality gates
✅ Natural part of workflow
```

**What happens when enabled:**
- `I18N-RULES.md` created with guidelines
- Translation file structure defined
- Quality gates enforce translations
- `/execute-work` automatically adds translations

**If you skip i18n initially:**
```bash
# You can enable later, but requires:
→ Create I18N-RULES.md manually
→ Refactor existing code
→ Add translations retroactively
→ More work overall
```

---

### 3. Run Tests Frequently

**Testing strategy:**

```bash
# After implementing each story
/run-tests all
→ Catch issues immediately
→ Ensure nothing broke

# Before marking story complete
/run-tests coverage
→ Verify 80%+ coverage
→ Check untested code paths

# When debugging specific issues
/run-tests story US-015
→ Run only relevant tests
→ Faster feedback loop

# Before commits (if manual workflow)
/run-tests all
→ Never commit failing tests
→ Quality gate enforcement

# Weekly full test suite
/run-tests coverage
→ Generate HTML report
→ Review coverage trends
```

**Test failure strategy:**
1. Run `/run-tests` to identify failures
2. Read failure messages carefully
3. Fix issues in code
4. Re-run tests
5. Repeat until all pass
6. Only then mark story complete

---

### 4. Use Default Stack (If Applicable)

**If building a modern web application:**

```bash
# During /init-project
Choose tech stack: [1] Default HolyEstate Stack

# What you get:
Frontend:
  - React 19 (latest features)
  - React Router 7 with SSR (server-side rendering)
  - Vite (fast build tool)

Backend:
  - Node.js with TypeScript
  - PostgreSQL 16 (latest stable)
  - Prisma 6.19.0 (type-safe ORM)

Testing:
  - Vitest 4.0 (unit + integration)
  - Playwright 1.58.0 (E2E)
  - 848 tests passing (production-proven)

# Benefits:
✅ Production-tested stack
✅ Best practices baked in
✅ Quick project start
✅ Known performance characteristics
✅ Proven scalability
```

**When NOT to use default stack:**
- Mobile app (React Native, Flutter)
- Different database (MongoDB, MySQL)
- Different language (Python, Java, Go)
- Legacy system integration
- Specific client requirements

**For those cases:**
```bash
# Choose AI or Custom
[2] AI Recommendation - Claude suggests best fit
[3] Custom Setup - Full control
```

---

### 5. Update Progress Daily

**End of each day:**
```bash
/update-progress

# Report today's work:
→ Stories completed (US-045, US-046)
→ Tests passing (69/69)
→ Blockers encountered (none)
→ Notes (payment integration complete)

# Why daily updates matter:
✅ Real-time visibility for team
✅ Early blocker detection
✅ Accurate velocity calculation
✅ Better project status reports
✅ Stakeholder confidence
```

**Alternative: Automatic updates**
```bash
# If using /execute-work, progress updates automatically
/execute-work phase 1
→ Progress updated after each story
→ No manual updates needed
```

---

### 6. Review Phase Documentation

**Before starting each phase:**
```bash
# Read the phase file
cat .project-management/output/phases/phase-1.md

# Verify:
✅ Stories make sense
✅ Priorities are correct
✅ Dependencies understood
✅ Estimates reasonable

# Adjust if needed:
→ Edit backlog.md
→ Run /generate-docs
→ Review updated phase file
```

---

### 7. Leverage Execution Modes

**Continuous mode:** Fast, automated
```bash
/execute-work phase 1
[1] Continuous

# Use when:
✅ Requirements clear
✅ Stories well-defined
✅ Trust the system
✅ Want speed
```

**Paused mode:** Controlled, reviewed
```bash
/execute-work phase 1
[2] Paused

# Use when:
✅ Learning system
✅ Complex features
✅ Want to review
✅ Need control
```

---

### 8. Monitor Quality Metrics

**Weekly quality check:**
```bash
/project-status

# Review metrics:
→ Test coverage (target: 80%+)
→ Bug count (trend: decreasing)
→ Velocity (trend: stable or increasing)
→ Blockers (action: resolve quickly)

# Take action if:
❌ Coverage < 80% → Add more tests
❌ Bugs increasing → Focus on quality
❌ Velocity decreasing → Investigate blockers
❌ Blockers unresolved → Prioritize fixes
```

---

### 9. Commit Message Standards

**Automatic commits (via `/execute-work`):**
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

**Key rules:**
- NO AI credits in commits (per git.md)
- Conventional commit format (feat:, fix:, refactor:)
- Include test results
- Reference story ID

---

### 10. Use Status Reports

**Generate weekly reports:**
```bash
/project-status

# Share with:
→ Team members (alignment)
→ Stakeholders (visibility)
→ Management (progress updates)

# Use for:
→ Sprint planning
→ Risk identification
→ Resource allocation
→ Timeline adjustments
```

---

## Common Patterns

### Pattern 1: Starting New Project
```bash
1. /init-project → Setup
2. /execute-work phase 1 → Foundation
3. /project-status → Weekly check
4. /execute-work phase 2 → Core features
5. Continue through all phases
```

### Pattern 2: Daily Development
```bash
Morning:
  → Review yesterday's progress
  → /execute-work story US-XXX

Throughout day:
  → Let /execute-work run
  → Or manual implementation + /run-tests

End of day:
  → /update-progress (if manual)
  → Review completed work
```

### Pattern 3: Debugging
```bash
1. /run-tests all → Identify failures
2. Fix code
3. /run-tests coverage → Verify fix
4. Repeat until all pass
5. /execute-work story US-XXX → Continue
```

---

**Version:** 3.0.0
**Last Updated:** 2026-03-27
**Part of:** Claude Project Management System v3.0
