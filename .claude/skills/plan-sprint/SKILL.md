---
name: plan-sprint
description: Plan the next sprint by selecting user stories from backlog, estimating capacity, and generating sprint document
---

# Plan Sprint

You are planning the next sprint for the project.

## Your Task

1. **Determine sprint number**:
   - Check `.project-management/output/sprints/` to see existing sprints
   - Plan the next sequential sprint

2. **Read current state**:
   - Read `backlog.md` to see available work
   - Read `current-status.md` to understand progress
   - Read `constraints.md` for team capacity
   - Check previous sprint(s) for velocity data

3. **Select work for sprint**:
   - Prioritize P0 (critical) items first
   - Consider P1 (high) items if capacity allows
   - Respect dependencies between stories
   - Match team capacity from constraints

4. **Create sprint plan**:
   - Use template from `.project-management/templates/sprint-template.md`
   - Fill in all sections completely
   - Break down stories into tasks
   - Assign story points realistically

5. **Update progress tracking**:
   - Update `current-status.md` with new sprint info
   - Log sprint creation

## Sprint Planning Guidelines

### Sprint Duration
**Default:** 2 weeks (10 working days)
- Adjust based on constraints.md if needed
- Keep consistent sprint length across project

### Selecting Work

**Priority Order:**
1. P0 (Critical) - Must be done
2. P1 (High) - Should be done
3. P2 (Medium) - Nice to have
4. P3 (Low) - Future consideration

**Consider:**
- Dependencies: Don't pick items that depend on incomplete work
- Team capacity: From constraints.md (hours/week × team size)
- Velocity: From previous sprints (if available)
- Risk: Balance high-risk items with safer work

### Story Points

**Use Fibonacci sequence:** 1, 2, 3, 5, 8, 13, 21

**Guidelines:**
- 1-2 points: Few hours, straightforward
- 3 points: Half day, well-understood
- 5 points: 1-2 days, some complexity
- 8 points: 2-4 days, significant work
- 13 points: 1 week, complex feature
- 21+ points: Break down into smaller stories

### Team Capacity Calculation

```
Weekly Capacity = (Team Member Hours/Week) × (Story Points per Hour)

Example:
- 2 developers × 40 hours/week = 80 hours
- Assume 1 story point ≈ 4-6 hours
- Sprint capacity ≈ 13-20 story points per developer
- Total capacity ≈ 26-40 story points for 2-week sprint
```

**Adjust for:**
- Meetings (standup, retro, planning): -10%
- Testing and code review: -15%
- Unexpected issues buffer: -10%
- **Realistic capacity: ~65% of theoretical maximum**

### Sprint Goal

Create a clear, concise sprint goal that:
- Describes the sprint's primary objective
- Is achievable within the sprint
- Delivers value to stakeholders
- Aligns with project milestones

**Good Examples:**
- "Implement core authentication (login, registration, password reset)"
- "Build product catalog with search and filtering"
- "Complete checkout flow from cart to payment"

**Bad Examples:**
- "Work on various features" (too vague)
- "Finish entire MVP" (too ambitious)
- "Fix bugs" (not outcome-focused)

## Sprint Plan Structure

### Required Sections

1. **Sprint Overview**
   - Sprint number and name
   - Start/end dates
   - Sprint goal
   - Team capacity

2. **Sprint Backlog**
   - All stories/tasks included
   - Story points for each
   - Assignees (if known)
   - Priorities
   - Dependencies

3. **Acceptance Criteria**
   - For each story
   - Clear, testable criteria
   - Definition of Done

4. **Risks & Mitigation**
   - Identify potential risks
   - Plan mitigation strategies

5. **Success Criteria**
   - How to measure sprint success
   - Demo plan

## Example Sprint Selection

**Sprint 2 Example:**

```
Available Capacity: 30 story points
Previous Velocity: 28 points (Sprint 1)

Selected Work:
✅ US-009: Add to Cart (5 points) - P0
✅ US-010: Checkout Process (8 points) - P0
✅ US-011: Payment Processing (13 points) - P0
✅ US-006: Edit Product Listing (5 points) - P1

Total: 31 points (slightly above velocity, acceptable)

Deferred:
⏸️ US-012: Order Confirmation (3 points) - Will move to Sprint 3
⏸️ US-013: View Orders (5 points) - Depends on US-012

Sprint Goal: "Complete shopping cart and checkout with payment processing"
```

## Quality Checklist

Before completing, verify:
- [ ] Sprint has clear, achievable goal
- [ ] Total story points match team capacity
- [ ] All dependencies are satisfied
- [ ] Work is prioritized correctly
- [ ] Each story has acceptance criteria
- [ ] Definition of Done is clear
- [ ] Risks are identified and mitigated
- [ ] Sprint dates are set
- [ ] Template is fully filled out

## Output to User

```
Sprint {{N}} Planned Successfully! 🎯

Sprint Details:
- Duration: {{START_DATE}} to {{END_DATE}} ({{DAYS}} days)
- Sprint Goal: {{GOAL}}
- Team Capacity: {{CAPACITY}} story points
- Planned Work: {{PLANNED}} story points

Sprint Backlog ({{COUNT}} items):
✅ US-XXX: Feature Name (X points) - P0
✅ US-XXX: Feature Name (X points) - P0
✅ T-XXX: Technical Task (X points) - P0
✅ US-XXX: Feature Name (X points) - P1

Sprint Summary:
- P0 Items: {{P0_COUNT}} ({{P0_POINTS}} points)
- P1 Items: {{P1_COUNT}} ({{P1_POINTS}} points)
- Technical Tasks: {{TASK_COUNT}} ({{TASK_POINTS}} points)

Risks Identified: {{RISK_COUNT}}
Dependencies: {{DEP_COUNT}}

Sprint file created: .project-management/output/sprints/sprint-{{N}}.md

Next Steps:
1. Review sprint plan
2. Start daily standups
3. Use /update-progress to track progress
4. Run /project-status anytime to check status
```

## Special Cases

### First Sprint
- Use conservative estimates (you don't know velocity yet)
- Focus on foundation/infrastructure tasks
- Aim for 60-70% of theoretical capacity
- Include setup and learning time

### After Incomplete Sprint
- Review what wasn't finished and why
- Adjust capacity for next sprint
- Consider carryover work
- Learn from blockers

### Sprint with External Dependencies
- Identify dependency clearly
- Have backup work ready
- Communicate risks upfront
- Plan mitigation strategies

Remember: A well-planned sprint sets the team up for success. Take time to select the right work and set realistic goals.
