---
name: update-progress
description: Update project progress tracking files based on completed work, in-progress tasks, and blockers
---

# Update Progress

Update project progress tracking based on current development status.

---

## Usage

```bash
/update-progress
```

Updates progress files with latest completed work, in-progress tasks, and blockers.

---

## 📋 YOUR TASK

### STEP 1: Gather Current State

**Ask user about:**
- What has been completed recently?
- What is currently in progress?
- Any blockers or issues?
- Any bugs discovered?
- Changes to timeline or scope?

**Use `AskUserQuestion` tool if needed for structured input.**

---

### STEP 2: Read Existing Progress Files

**Read all progress tracking files:**
- `.project-management/output/phases/phase-N.md` - Current phase
- `.project-management/output/progress/current-status.md` - Overall status
- `.project-management/output/progress/completed.md` - Completed work
- `.project-management/output/progress/blockers.md` - Active blockers

---

### STEP 3: Update Progress Files

#### 1. Update Phase File (`phase-N.md`)

**For each completed story:**
- Change status from "In Progress" → "Completed"
- Add completion timestamp
- Add test metrics (tests passed, coverage)
- Add git commit hash
- Update phase metrics:
  - Completed story points
  - Progress percentage
  - Test count
  - Coverage percentage

#### 2. Update `completed.md`

**Add newly completed work:**
```markdown
## Week {{WEEK_NUMBER}} ({{DATE_RANGE}})

### Completed Stories
- ✅ US-XXX: {{story_title}} ({{points}} points)
  - Completed: {{DATE}}
  - Tests: {{test_count}} passing
  - Coverage: {{coverage}}%
  - Commit: {{commit_hash}}

### Key Achievements
- {{achievement_1}}
- {{achievement_2}}
```

#### 3. Update `blockers.md`

**Add new blockers:**
```markdown
## Active Blockers

### 🔴 {{blocker_id}}: {{blocker_title}}
- **Status:** Active
- **Impact:** High | Medium | Low
- **Affected Stories:** US-XXX, US-YYY
- **Description:** {{details}}
- **Reported:** {{DATE}}
- **Action Plan:** {{plan}}
```

**Resolve completed blockers:**
- Move to "Resolved Blockers" section
- Add resolution date and notes

#### 4. Update `current-status.md`

**Update metrics:**
- Overall completion percentage
- Current phase progress
- Velocity
- Test coverage
- Bug count
- Timeline status

---

### STEP 4: Calculate Metrics

**Completion Percentage:**
```
(Completed Story Points / Total Story Points) × 100
```

**Phase Progress:**
```
(Phase Completed Points / Phase Total Points) × 100
```

**Velocity (if applicable):**
```
Story Points Completed / Weeks Elapsed
```

**Update progress bars conceptually:**
```
Phase N: [████████░░░░░░░░] 50%
```

---

### STEP 5: Summary Report

**Display update summary:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 PROGRESS UPDATED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

✅ COMPLETED THIS UPDATE:
- US-XXX: {{story_title}} ({{points}} points)
- US-YYY: {{story_title}} ({{points}} points)

Total: {{completed_count}} stories, {{points_total}} points

📈 UPDATED METRICS:
- Phase {{N}} Progress: {{old}}% → {{new}}% ({{delta}}%)
- Overall Completion: {{old}}% → {{new}}% ({{delta}}%)
- Velocity: {{velocity}} points/week
- Test Coverage: {{coverage}}%

🔴 BLOCKERS:
- Added: {{new_blockers_count}}
- Resolved: {{resolved_blockers_count}}
- Active: {{active_blockers_count}}

📝 FILES UPDATED:
✅ phase-{{N}}.md
✅ current-status.md
✅ completed.md
✅ blockers.md

🎯 NEXT:
{{next_steps}}
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## ⚠️ IMPORTANT NOTES

### Update Frequency

**Recommended:**
- After completing each story (automatic with `/execute-work`)
- Daily standup updates
- End of week summaries
- Phase completion

### Data Integrity

**Ensure:**
- Timestamps are accurate
- Story points calculated correctly
- Metrics add up (no double counting)
- Blocker statuses are current
- Git commit hashes are correct

### Quality Checks

**Before finishing:**
- [ ] All completed stories documented
- [ ] Metrics calculated correctly
- [ ] Blockers status updated
- [ ] Files saved properly
- [ ] Summary accurate

---

## 📝 Example Execution

```bash
# User runs:
/update-progress

# Claude asks:
What has been completed recently?

# User responds:
"Completed US-045 (user profile) and US-046 (payment integration)"

# Claude updates files and displays:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 PROGRESS UPDATED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

✅ COMPLETED THIS UPDATE:
- US-045: User profile page (5 points)
- US-046: Payment integration (13 points)

Total: 2 stories, 18 points

📈 UPDATED METRICS:
- Phase 2 Progress: 45% → 60% (+15%)
- Overall Completion: 35% → 42% (+7%)
- Velocity: 18 points/week
- Test Coverage: 87%

📝 FILES UPDATED:
✅ phase-2.md
✅ current-status.md
✅ completed.md

🎯 NEXT:
Continue with US-047 (order tracking)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

**Version:** 3.0.0
**Created:** 2026-03-27
**Command Type:** Progress Tracking
