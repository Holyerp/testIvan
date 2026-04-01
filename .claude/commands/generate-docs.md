---
name: generate-docs
description: Generate or update project documentation (PRD, technical spec, architecture docs) from input files
---

# Generate Documentation

You are generating or updating project documentation based on current input files.

**🌍 CRITICAL: Generate ALL documentation in English only. No exceptions.**

## Your Task

1. **Read ALL input files** from `.project-management/input/`:
   - `scope.md` - Project scope
   - `backlog.md` - Features backlog
   - `technologies.md` - Tech stack
   - `constraints.md` - Constraints

2. **Check existing documentation** in `.project-management/output/docs/`:
   - See what already exists
   - Identify what needs to be updated vs created

3. **Generate or update documentation** (ALL in English):
   - `prd.md` - Product Requirements Document
   - `technical-spec.md` - Technical Specification
   - `architecture.md` - Architecture Document
   - `api-spec.md` - API Specification (if needed)

4. **Use templates** from `.project-management/templates/` and replace placeholders with actual content

## When to Use This Command

- After modifying input files (scope, backlog, technologies, constraints)
- When documentation is out of sync with current requirements
- To regenerate specific documents
- After major project changes

## Documentation to Generate

### Product Requirements Document (PRD)
**Source Data:**
- Vision: scope.md → Vision section
- Features: backlog.md → All user stories and epics
- Success Metrics: scope.md → Success Criteria
- Timeline: constraints.md → Timeline Constraints

**What to Include:**
- Executive summary
- Product overview and vision
- User personas (infer from target audience)
- Complete feature list organized by priority
- User stories with acceptance criteria
- Non-functional requirements
- Assumptions and risks
- Timeline and phases

### Technical Specification
**Source Data:**
- Tech Stack: technologies.md → All technology sections
- Features: backlog.md → Technical requirements
- Constraints: constraints.md → Technical constraints
- Architecture: Infer from technologies and requirements

**What to Include:**
- Complete technology stack breakdown
- System architecture overview
- Frontend architecture (components, routing, state)
- Backend architecture (API, services, database)
- Database schema design for all entities
- API endpoint specifications
- Security implementation details
- Testing strategy
- Deployment strategy

### Architecture Document
**Source Data:**
- Technologies: technologies.md → Infrastructure, hosting, etc.
- Requirements: backlog.md + scope.md → Functional requirements
- Constraints: constraints.md → Performance, scalability needs

**What to Include:**
- System context diagram
- Container diagram
- Component architecture
- Data architecture and flow
- Security architecture
- Infrastructure architecture
- Integration architecture
- Performance considerations
- Technology rationale

## Guidelines

### Be Comprehensive
- Don't leave placeholders or TODOs
- Fill in all sections completely
- Use actual data from input files

### Be Consistent
- Ensure all documents align with each other
- Use same terminology across documents
- Reference same features/components

### Be Realistic
- Consider constraints when designing
- Align with team capacity
- Account for technical limitations

### Be Actionable
- Provide concrete specifications
- Include measurable success criteria
- Give clear implementation guidance

## Quality Checklist

Before completing, verify:
- [ ] All input files were read and analyzed
- [ ] All templates were used correctly
- [ ] All placeholders ({{VARIABLES}}) are replaced
- [ ] Documentation is internally consistent
- [ ] Technical decisions are justified
- [ ] Constraints are respected
- [ ] Security requirements are addressed
- [ ] Performance targets are specified
- [ ] API endpoints match features
- [ ] Database schema supports all features

## Output to User

After generating documentation, provide:

```
Documentation Generated Successfully! 📚

Updated Documents:
✅ Product Requirements Document
   - 18 features documented
   - 5 user personas defined
   - Success metrics specified

✅ Technical Specification
   - Complete tech stack defined
   - 15 API endpoints specified
   - Database schema with 8 tables
   - Security implementation detailed

✅ Architecture Document
   - System architecture designed
   - Component breakdown complete
   - Infrastructure plan defined
   - Technology choices justified

Key Decisions:
- Using PostgreSQL for relational data needs
- React Router v6 for frontend routing
- JWT-based authentication
- Microservices considered but monolith chosen for MVP

Next Steps:
1. Review generated docs in .project-management/output/docs/
2. Run /plan-sprint to create sprint plans
3. Start implementation following technical spec
```

## Special Instructions

- If a document already exists, UPDATE it rather than overwrite completely
- Preserve any manual additions or notes
- Add a "Last Updated" timestamp
- Increment version number if applicable
- Log changes in revision history

Remember: Documentation quality directly impacts development success. Take time to create thorough, accurate, and useful documentation.
