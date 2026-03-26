---
name: init-project
description: Initialize project documentation by generating PRD, technical spec, and sprint structure from input files
---

# Initialize Project

You are initializing a new project with the Claude Project Management System.

## Your Task

1. **Read all input files** from `.project-management/input/`:
   - `scope.md` - Project scope and objectives
   - `backlog.md` - Features and user stories
   - `technologies.md` - Technology stack
   - `constraints.md` - Project constraints

2. **Analyze the inputs** to understand:
   - Project goals and vision
   - Feature requirements and priorities
   - Technical architecture needs
   - Timeline and resource constraints

3. **Generate initial documentation** in `.project-management/output/docs/`:
   - `prd.md` - Product Requirements Document (use template from `.project-management/templates/prd-template.md`)
   - `technical-spec.md` - Technical Specification (use template)
   - `architecture.md` - System Architecture (use template)

4. **Create initial progress tracking** in `.project-management/output/progress/`:
   - `current-status.md` - Initialize with project start status (use progress template)
   - `completed.md` - Empty initially, for tracking completed work
   - `blockers.md` - Empty initially, for tracking blockers

5. **Create first sprint plan** in `.project-management/output/sprints/`:
   - `sprint-1.md` - Plan first sprint based on P0 priorities from backlog (use sprint template)

## Important Guidelines

- **Use the templates** from `.project-management/templates/` and replace all `{{PLACEHOLDERS}}` with actual data from the input files
- **Be comprehensive** - generate complete documentation, don't leave TODOs
- **Follow the project rules** in `.CLAUDE.MD` and `.project-management/rules/project-rules.md`
- **Prioritize properly** - Focus on P0 (critical) items for Sprint 1
- **Be realistic** - Consider constraints when planning
- **Create actionable plans** - Sprint 1 should be achievable with the team capacity

## What to Generate

### PRD (Product Requirements Document)
- Extract project vision from scope.md
- List all features from backlog.md organized by priority
- Define success metrics
- Document assumptions and risks

### Technical Specification
- Detail the technology stack from technologies.md
- Design database schema based on requirements
- Define API endpoints for features
- Specify security and performance requirements

### Architecture Document
- Create system architecture based on tech stack
- Design component structure
- Define data flow
- Plan deployment architecture

### Sprint 1 Plan
- Select P0 items from backlog
- Break down into tasks
- Assign story points
- Consider team capacity from constraints
- Set realistic sprint goal

### Initial Progress Report
- Set project start date
- Initialize all metrics to 0%
- Set milestones from scope
- List Sprint 1 as current sprint

## After Completion

Provide a summary to the user showing:
- ✅ What documents were generated
- 📊 Project statistics (features count, estimated timeline, etc.)
- 🎯 Sprint 1 goals and duration
- 🚀 Next steps for the user

## Example Output Structure

```
Project Initialized Successfully! 🎉

Generated Documentation:
✅ Product Requirements Document (PRD)
✅ Technical Specification
✅ System Architecture Document
✅ Sprint 1 Plan
✅ Initial Progress Tracking

Project Overview:
- Total Features: 18 (12 P0, 4 P1, 2 P2)
- Estimated Duration: 12 weeks
- Sprint 1: 2 weeks (14 days)
- Sprint 1 Goal: Core authentication and product listing

Next Steps:
1. Review generated documentation in .project-management/output/docs/
2. Adjust Sprint 1 plan if needed
3. Run /project-status to see current state
4. Start development!
```

Remember: Read ALL input files carefully before generating any documentation. The quality of your output depends on understanding the full context.
