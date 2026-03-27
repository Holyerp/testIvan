---
name: project-status
description: Generate comprehensive project status report showing progress, completed work, blockers, and next steps
---

# Project Status

You are providing a comprehensive overview of the current project status.

## Your Task

1. **Read all relevant files**:
   - `.project-management/input/scope.md` - Project scope
   - `.project-management/output/progress/current-status.md` - Current status
   - `.project-management/output/progress/completed.md` - Completed work
   - `.project-management/output/progress/blockers.md` - Blockers
   - `.project-management/output/sprints/sprint-*.md` - All sprint files
   - `.project-management/input/backlog.md` - Remaining work

2. **Analyze the current state**:
   - Calculate overall project completion
   - Assess current sprint status
   - Identify trends (velocity, quality, etc.)
   - Evaluate risks and blockers
   - Check timeline adherence

3. **Generate comprehensive status report**:
   - Executive summary with key metrics
   - Sprint status
   - Overall project health
   - Risks and blockers
   - Quality metrics
   - Timeline status
   - Recommendations

4. **Present in clear, visual format**:
   - Use status indicators (🟢🟡🔴)
   - Include progress bars conceptually
   - Show trends (↑↓→)
   - Highlight critical items

## Status Report Structure

### 1. Executive Summary

```markdown
# Project Status Report

**Generated:** {{DATE}}
**Project:** {{PROJECT_NAME}}
**Overall Status:** 🟢 On Track / 🟡 At Risk / 🔴 Off Track

## Quick Overview

| Metric | Status | Value |
|--------|--------|-------|
| Overall Progress | {{INDICATOR}} | {{PERCENTAGE}}% |
| Current Sprint | {{INDICATOR}} | Sprint {{N}} - {{STATUS}} |
| Timeline | {{INDICATOR}} | {{AHEAD/ON TIME/DELAYED}} |
| Budget | {{INDICATOR}} | {{PERCENTAGE}}% used |
| Quality | {{INDICATOR}} | {{METRICS}} |
| Team Velocity | {{TREND}} | {{VELOCITY}} points/sprint |

**Critical Items Requiring Attention:** {{COUNT}}
```

### 2. Current Sprint Status

```markdown
## Current Sprint: Sprint {{N}}

**Sprint Goal:** {{GOAL}}
**Duration:** {{START}} to {{END}} ({{DAYS_ELAPSED}}/{{TOTAL_DAYS}} days)
**Status:** {{STATUS}}

### Progress

**Story Points:**
- Planned: {{PLANNED}}
- Completed: {{COMPLETED}}
- In Progress: {{IN_PROGRESS}}
- Remaining: {{REMAINING}}
- Completion: {{PERCENTAGE}}%

**Task Breakdown:**
- ✅ Done: {{DONE_COUNT}} tasks ({{DONE_POINTS}} points)
- 🔄 In Progress: {{PROGRESS_COUNT}} tasks ({{PROGRESS_POINTS}} points)
- ⏸️ Blocked: {{BLOCKED_COUNT}} tasks ({{BLOCKED_POINTS}} points)
- 📋 Todo: {{TODO_COUNT}} tasks ({{TODO_POINTS}} points)

### Sprint Health
{{HEALTH_ASSESSMENT}}

**Will Sprint Goal Be Met?** {{YES/LIKELY/AT_RISK/NO}}
**Confidence Level:** {{HIGH/MEDIUM/LOW}}
```

### 3. Overall Project Status

```markdown
## Overall Project Health

### Completion Status

**Features:**
- Total Features: {{TOTAL}}
- Completed: {{COMPLETED}} ({{PERCENTAGE}}%)
- In Progress: {{IN_PROGRESS}}
- Not Started: {{NOT_STARTED}}

**By Priority:**
- P0 (Critical): {{COMPLETED}}/{{TOTAL}} ({{PERCENTAGE}}%)
- P1 (High): {{COMPLETED}}/{{TOTAL}} ({{PERCENTAGE}}%)
- P2 (Medium): {{COMPLETED}}/{{TOTAL}} ({{PERCENTAGE}}%)

**Story Points:**
- Total Estimated: {{TOTAL_POINTS}}
- Completed: {{COMPLETED_POINTS}} ({{PERCENTAGE}}%)
- Remaining: {{REMAINING_POINTS}}

### Progress Trend
{{TREND_ANALYSIS}}

**Visual Representation:**
Progress: [████████░░] {{PERCENTAGE}}%
```

### 4. Timeline Status

```markdown
## Timeline & Milestones

**Project Timeline:**
- Start Date: {{START_DATE}}
- Target Completion: {{TARGET_DATE}}
- Current Date: {{CURRENT_DATE}}
- Time Elapsed: {{ELAPSED}} / {{TOTAL}} days ({{PERCENTAGE}}%)
- Time Remaining: {{REMAINING}} days

**Status:** {{ON_TIME/AHEAD/DELAYED}}

### Milestones

| Milestone | Target Date | Status | Completion |
|-----------|-------------|--------|------------|
| {{MILESTONE_1}} | {{DATE}} | {{STATUS}} | {{PCT}}% |
| {{MILESTONE_2}} | {{DATE}} | {{STATUS}} | {{PCT}}% |
| {{MILESTONE_3}} | {{DATE}} | {{STATUS}} | {{PCT}}% |

**Timeline Risk:** {{LOW/MEDIUM/HIGH}}
**Projected Completion:** {{DATE}} ({{AHEAD/ON_TIME/DELAYED}})
```

### 5. Blockers & Risks

```markdown
## Blockers & Risks

### 🔴 Critical Blockers ({{COUNT}})

{{#each critical_blockers}}
#### BLOCKER-{{ID}}: {{TITLE}}
- **Impact:** {{IMPACT}}
- **Affected Work:** {{AFFECTED}}
- **Status:** {{STATUS}}
- **Owner:** {{OWNER}}
- **Target Resolution:** {{DATE}}
{{/each}}

### ⚠️ Active Risks ({{COUNT}})

{{#each risks}}
#### RISK-{{ID}}: {{TITLE}}
- **Probability:** {{PROBABILITY}}
- **Impact:** {{IMPACT}}
- **Mitigation:** {{MITIGATION}}
- **Status:** {{STATUS}}
{{/each}}

**Risk Level:** {{LOW/MEDIUM/HIGH}}
```

### 6. Quality Metrics

```markdown
## Quality & Technical Health

### Code Quality
- **Test Coverage:** {{PERCENTAGE}}% (Target: 80%)
- **Passing Tests:** {{PASSING}}/{{TOTAL}}
- **Linting Issues:** {{COUNT}}
- **Type Coverage:** {{PERCENTAGE}}%

### Bugs
- **Open Bugs:** {{OPEN}}
  - Critical (P0): {{P0}}
  - High (P1): {{P1}}
  - Medium (P2): {{P2}}
  - Low (P3): {{P3}}
- **Fixed This Sprint:** {{FIXED}}
- **Bug Trend:** {{TREND}}

### Performance
- **API Response Time (p95):** {{TIME}}ms (Target: <500ms)
- **Page Load Time:** {{TIME}}s (Target: <2s)
- **Lighthouse Score:** {{SCORE}}/100
- **Uptime:** {{PERCENTAGE}}%

**Quality Status:** {{GOOD/NEEDS_ATTENTION/CRITICAL}}
```

### 7. Team Velocity & Productivity

```markdown
## Team Performance

### Velocity Trend

| Sprint | Planned | Completed | Velocity | Trend |
|--------|---------|-----------|----------|-------|
| Sprint {{N-2}} | {{PTS}} | {{PTS}} | {{VEL}} | - |
| Sprint {{N-1}} | {{PTS}} | {{PTS}} | {{VEL}} | {{TREND}} |
| Sprint {{N}} | {{PTS}} | {{PTS}}* | {{VEL}}* | {{TREND}} |

*Current sprint in progress

**Average Velocity:** {{AVG}} points/sprint
**Velocity Trend:** {{IMPROVING/STABLE/DECLINING}}

### Team Capacity
- **Available Capacity:** {{CAPACITY}} points/sprint
- **Current Utilization:** {{PERCENTAGE}}%
- **Status:** {{OPTIMAL/OVERCOMMITTED/UNDERUTILIZED}}
```

### 8. Recommendations & Next Steps

```markdown
## Recommendations

### Immediate Actions Required
{{#if critical_items}}
1. {{ACTION_1}}
2. {{ACTION_2}}
3. {{ACTION_3}}
{{else}}
✅ No immediate actions required
{{/if}}

### Short-term Recommendations (This Sprint)
1. {{RECOMMENDATION_1}}
2. {{RECOMMENDATION_2}}
3. {{RECOMMENDATION_3}}

### Medium-term Recommendations (Next 2 Sprints)
1. {{RECOMMENDATION_1}}
2. {{RECOMMENDATION_2}}

### Strategic Recommendations
{{STRATEGIC_RECOMMENDATIONS}}

## Next Steps

**For Development Team:**
- {{STEP_1}}
- {{STEP_2}}

**For Stakeholders:**
- {{STEP_1}}
- {{STEP_2}}

**For Next Sprint:**
- {{STEP_1}}
- {{STEP_2}}
```

## Analysis Guidelines

### Determine Overall Status

**🟢 On Track:**
- Sprint completion >70%
- No critical blockers
- Timeline on target
- Velocity stable or improving
- Quality metrics meeting targets

**🟡 At Risk:**
- Sprint completion 40-70%
- 1-2 significant blockers
- Timeline slightly delayed (<1 week)
- Velocity declining
- Some quality concerns

**🔴 Off Track:**
- Sprint completion <40%
- Multiple critical blockers
- Timeline delayed >1 week
- Velocity significantly down
- Quality metrics below target

### Calculate Projected Completion

```
Remaining Story Points = Total Points - Completed Points
Average Velocity = Sum of Last 3 Sprints / 3
Remaining Sprints = Remaining Points / Average Velocity
Projected Completion = Current Date + (Remaining Sprints × Sprint Length)
```

### Identify Trends

- **Velocity:** Compare last 3 sprints
- **Quality:** Track test coverage, bug trends over time
- **Progress:** Compare planned vs actual completion rates
- **Blockers:** Frequency and resolution time

### Generate Recommendations

Base recommendations on:
- Identified risks and blockers
- Trend analysis
- Deviations from plan
- Resource utilization
- Quality metrics

## Output Format

Present the status report in a clear, scannable format:

```
📊 PROJECT STATUS REPORT
Generated: {{DATE}}

═══════════════════════════════════════════════════

🎯 EXECUTIVE SUMMARY

Overall Status: 🟢 ON TRACK
Project Progress: ████████░░ 75% Complete
Current Sprint: Sprint 4 - 🟡 At Risk (80% complete, 2 days left)
Timeline: On schedule for {{DATE}} completion

Critical Alerts: 1 ⚠️
- BLOCKER-003: API integration delayed

═══════════════════════════════════════════════════

📈 KEY METRICS

Sprint Progress:     [████████░░] 80% (24/30 points)
Overall Progress:    [███████░░░] 75% (180/240 points)
Team Velocity:       28 points/sprint (↑ improving)
Test Coverage:       82% (✅ above target)
Open Bugs:           8 (3 P0, 5 P1)
Budget Used:         65% (🟢 on track)

═══════════════════════════════════════════════════

✅ RECENT WINS

• Completed payment integration (13 points)
• Shipped order management dashboard
• Achieved 82% test coverage milestone
• Fixed all P0 bugs from last sprint

═══════════════════════════════════════════════════

⚠️ ATTENTION NEEDED

🔴 BLOCKER-003: Third-party API integration delayed
   Impact: Cannot complete order tracking feature
   Action: Switch to alternative provider (in progress)
   Target: Resolved by {{DATE}}

═══════════════════════════════════════════════════

📅 UPCOMING

Next Sprint: Sprint 5 starts {{DATE}}
Focus: Order fulfillment and notifications
Planned: 30 story points, 8 features

═══════════════════════════════════════════════════

💡 RECOMMENDATIONS

1. Address BLOCKER-003 urgently (blocks 2 features)
2. Allocate time for P0 bug fixes in Sprint 5
3. Consider adding QA resource (bug rate increasing)
4. Schedule performance optimization sprint soon

═══════════════════════════════════════════════════

🔍 DETAILED REPORTS

For more details, see:
- Current Status: .project-management/output/progress/current-status.md
- Sprint 4 Plan: .project-management/output/sprints/sprint-4.md
- Blockers: .project-management/output/progress/blockers.md

═══════════════════════════════════════════════════
```

## When to Use This Command

Run `/project-status`:
- **Regularly:** Weekly or bi-weekly for ongoing monitoring
- **Before Meetings:** Stakeholder updates, sprint reviews
- **When Uncertain:** To get current snapshot
- **After Major Events:** Completing features, discovering blockers
- **On Request:** When someone asks "how's the project going?"

## Quality Checklist

Before presenting status, verify:
- [ ] All data is current
- [ ] Metrics are accurately calculated
- [ ] Status indicators are appropriate
- [ ] Trends are correctly identified
- [ ] Recommendations are actionable
- [ ] Critical items are highlighted
- [ ] Report is easy to scan/read

Remember: A good status report provides clarity, identifies issues early, and guides decision-making. Be honest and accurate - don't sugarcoat problems.
