---
name: update-progress
description: Update project progress tracking files based on completed work, in-progress tasks, and blockers
---

# Update Progress

You are updating the project progress tracking based on current development status.

## Your Task

1. **Gather current state information** by asking the user:
   - What has been completed recently?
   - What is currently in progress?
   - Are there any blockers or issues?
   - Any bugs discovered?
   - Any changes to timeline or scope?

2. **Read existing progress files**:
   - `current-status.md` - Current overall status
   - `completed.md` - Previously completed work
   - `blockers.md` - Known blockers
   - Current sprint file from `sprints/sprint-N.md`

3. **Update progress files**:
   - Update `current-status.md` with latest information
   - Add newly completed items to `completed.md`
   - Add/update/resolve blockers in `blockers.md`
   - Update current sprint file with task status

4. **Calculate metrics**:
   - Sprint progress percentage
   - Overall project completion
   - Velocity (if sprint completed)
   - Burndown data

5. **Provide summary** to user with key insights

## Information to Gather from User

Use the `AskUserQuestion` tool if needed to gather:

### Completed Work
- What stories/tasks were finished?
- When were they completed?
- Were there any deviations from plan?

### In Progress Work
- What's currently being worked on?
- By whom?
- Expected completion date?
- Any issues or concerns?

### Blockers
- Any new blockers?
- Status of existing blockers?
- Impact on timeline?
- Mitigation plans?

### Sprint Status
- Is sprint on track?
- Will sprint goal be met?
- Need to adjust scope?

## Progress Updates to Make

### 1. Update current-status.md

**Update these sections:**

```markdown
## Executive Summary
- Overall Status: 🟢/🟡/🔴 based on progress
- Progress: X% Complete (calculate from completed vs total work)
- Update key highlights with recent completions

## Sprint Progress
- Update completed story points
- Update in-progress items
- Update remaining work
- Calculate sprint completion %

## Completed Work
- Add new completions with dates
- Include description and impact

## Work In Progress
- Update status of active tasks
- Update progress percentages
- Adjust expected completion dates

## Blockers & Issues
- Add new blockers
- Update existing blockers
- Mark resolved blockers

## Quality Metrics
- Update test coverage if changed
- Update bug counts
- Update performance metrics if measured

## Team Velocity
- Update current sprint data
- Calculate velocity if sprint completed
```

### 2. Update completed.md

Add entries in this format:

```markdown
## {{DATE}}

### ✅ {{STORY_ID}}: {{TITLE}}
**Type:** Feature / Bug Fix / Task / Technical Debt
**Story Points:** {{POINTS}}
**Completed By:** {{DEVELOPER}}
**Sprint:** Sprint {{N}}

**Description:**
{{WHAT_WAS_DONE}}

**Impact:**
{{HOW_IT_HELPS}}

**Related PRs:**
- {{PR_LINK_1}}
- {{PR_LINK_2}}

**Notes:**
{{ANY_IMPORTANT_NOTES}}
```

### 3. Update blockers.md

```markdown
## Active Blockers

### 🔴 BLOCKER-{{ID}}: {{TITLE}}
**Status:** Active / In Progress / Resolved
**Severity:** Critical / High / Medium / Low
**Reported:** {{DATE}}
**Affected Work:** {{STORIES_AFFECTED}}
**Assigned To:** {{OWNER}}

**Description:**
{{DETAILED_DESCRIPTION}}

**Impact:**
- {{IMPACT_1}}
- {{IMPACT_2}}

**Mitigation Plan:**
{{WHAT_ARE_WE_DOING}}

**Resolution Target:** {{DATE}}

**Updates:**
- {{DATE}}: {{UPDATE_1}}
- {{DATE}}: {{UPDATE_2}}
```

### 4. Update Current Sprint File

Update sprint file to reflect current status:

```markdown
## Sprint Progress Tracking

### Tasks Status

| Task ID | Title | Assignee | Status | Progress |
|---------|-------|----------|--------|----------|
| US-001 | Feature X | John | ✅ Done | 100% |
| US-002 | Feature Y | Jane | 🔄 In Progress | 60% |
| US-003 | Feature Z | John | ⏸️ Blocked | 0% |
| US-004 | Feature W | Jane | 📋 Todo | 0% |

### Burndown Data

| Day | Date | Planned Remaining | Actual Remaining | Notes |
|-----|------|-------------------|------------------|-------|
| Day 1 | {{DATE}} | 30 | 30 | Sprint started |
| Day 2 | {{DATE}} | 27 | 28 | Slower than expected |
| Day 3 | {{DATE}} | 24 | 23 | Back on track |
...
```

## Metrics to Calculate

### Sprint Progress
```
Sprint Completion % = (Completed Points / Total Planned Points) × 100
```

### Overall Project Progress
```
Project Completion % = (Total Completed Points / Total Estimated Points) × 100
```

### Sprint Velocity
```
Velocity = Total Story Points Completed in Sprint
```

### Sprint Burndown
```
Remaining Points = Total Sprint Points - Completed Points
Ideal Remaining = Total Sprint Points × (Days Left / Total Sprint Days)
```

### Bug Metrics
```
Bug Creation Rate = New Bugs / Time Period
Bug Resolution Rate = Fixed Bugs / Time Period
Bug Backlog = Open Bugs - Closed Bugs
```

## Status Indicators

Use these indicators consistently:

**Overall Status:**
- 🟢 **On Track:** Meeting targets, no major issues
- 🟡 **At Risk:** Some concerns, monitoring needed
- 🔴 **Off Track:** Significant issues, intervention required

**Task Status:**
- ✅ **Done:** Completed and verified
- 🔄 **In Progress:** Currently being worked
- ⏸️ **Blocked:** Cannot proceed due to blocker
- 📋 **Todo:** Not yet started
- ⚠️ **At Risk:** In progress but facing issues

## Quality Checklist

Before completing, verify:
- [ ] All recent completions are documented
- [ ] In-progress work status is current
- [ ] Blockers are up to date
- [ ] Metrics are recalculated
- [ ] Sprint status is accurate
- [ ] Dates are correct
- [ ] Status indicators are appropriate
- [ ] Next steps are clear

## Output to User

```
Progress Updated Successfully! 📊

Sprint {{N}} Status:
- Completion: {{PERCENTAGE}}% ({{COMPLETED}}/{{TOTAL}} points)
- Status: {{STATUS_INDICATOR}} {{STATUS_TEXT}}
- Days Remaining: {{DAYS}}

Recent Completions:
✅ US-XXX: Feature Name (X points)
✅ US-XXX: Feature Name (X points)
✅ BUG-XXX: Bug Fix (X points)

Currently In Progress:
🔄 US-XXX: Feature Name - 60% complete
🔄 US-XXX: Feature Name - 30% complete

Active Blockers: {{COUNT}}
{{#if blockers}}
🔴 BLOCKER-XXX: {{TITLE}} - {{STATUS}}
{{/if}}

Project Overview:
- Overall Progress: {{PROJECT_PERCENTAGE}}%
- Sprint Velocity: {{VELOCITY}} points ({{TREND}})
- Bugs: {{OPEN_BUGS}} open, {{FIXED_BUGS}} fixed this sprint
- Quality: Test coverage {{COVERAGE}}%

Timeline Status:
- Current Milestone: {{MILESTONE}} - {{MILESTONE_STATUS}}
- On track for {{TARGET_DATE}}: {{YES/NO}}

Updated Files:
✅ .project-management/output/progress/current-status.md
✅ .project-management/output/progress/completed.md
{{#if new_blockers}}
✅ .project-management/output/progress/blockers.md
{{/if}}
✅ .project-management/output/sprints/sprint-{{N}}.md

Next Steps:
{{NEXT_STEPS}}

Recommendations:
{{RECOMMENDATIONS}}
```

## When to Use This Command

Use `/update-progress` regularly:

- **Daily/Every Few Days:** For active sprints
- **After Completing Stories:** Document completions immediately
- **When Blockers Arise:** Track blockers as they happen
- **End of Sprint:** Final update before sprint review
- **Before Stakeholder Meetings:** Get current status
- **When Asked:** By running `/project-status` (which triggers this)

## Special Cases

### Sprint Completed
- Mark sprint as completed in sprint file
- Calculate final velocity
- Move incomplete work to backlog or next sprint
- Conduct sprint retrospective
- Update team velocity averages

### Major Blocker
- Escalate in status report
- Update risk assessment
- Consider sprint goal adjustment
- Communicate to stakeholders

### Scope Change
- Document the change
- Update backlog
- Recalculate timelines
- Update affected documentation

Remember: Accurate progress tracking enables better decision-making and keeps stakeholders informed. Update frequently and honestly.
