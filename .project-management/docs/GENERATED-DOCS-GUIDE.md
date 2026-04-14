# Generated Documentation Guide

**Version:** 3.0.0
**Purpose:** Understanding Claude-generated project documentation

---

## Overview

After running `/init-project` or `/generate-docs`, Claude generates comprehensive project documentation from your input files.

**Location:** `.project-management/output/docs/`

**Generated files:**
1. `prd.md` - Product Requirements Document
2. `technical-spec.md` - Technical Specification
3. `architecture.md` - Architecture Document
4. `api-spec.md` - API Specification (optional)

---

## 📋 PRD (Product Requirements Document)

**Filename:** `prd.md`
**Template:** `.project-management/templates/prd-template.md`

### Purpose
Define WHAT to build and WHY

### Audience
- Product managers
- Stakeholders
- Developers (context)
- QA/Testing teams

### Key Sections

**1. Product Vision**
- Derived from `input/scope.md`
- Big picture goals
- Problem being solved

**2. Target Users**
- User personas
- User needs and pain points
- Use cases

**3. Feature Requirements**
- Derived from `input/backlog.md`
- All epics and stories
- Organized by priority

**4. User Stories**
- Complete list with acceptance criteria
- Story points and priorities
- Dependencies

**5. Success Metrics**
- KPIs from `input/scope.md`
- Measurable goals
- Quality standards

**6. Timeline**
- Phase structure (4 phases, 1-4 months each)
- Phase milestones
- Launch timeline

### When to Use

✅ Alignment meetings with stakeholders
✅ Prioritization decisions
✅ Scope validation
✅ Communicating project goals to team
✅ Onboarding new team members

### Regenerating

```bash
# After updating scope.md or backlog.md
/generate-docs
```

---

## 🔧 Technical Specification

**Filename:** `technical-spec.md`
**Template:** `.project-management/templates/technical-spec-template.md`

### Purpose
Define HOW to build the product technically

### Audience
- Developers (primary)
- Architects
- DevOps engineers
- Technical leads

### Key Sections

**1. Technology Stack**
- Derived from `input/technologies.md`
- All frameworks and libraries
- Versions and justifications

**2. System Architecture**
- High-level architecture
- Component relationships
- Data flow diagrams

**3. Database Schema**
- All tables/collections
- Relationships
- Indexes and constraints

**4. API Specifications**
- All endpoints
- Request/response formats
- Authentication/authorization

**5. Security Implementation**
- Authentication strategy
- Authorization rules
- Data protection
- Security best practices

**6. Testing Strategy**
- Unit test approach
- Integration test approach
- E2E test approach
- Coverage goals (80%+ recommended)

**7. Performance Requirements**
- Response time targets
- Load capacity
- Scalability considerations

### When to Use

✅ Development planning
✅ Technical decision-making
✅ Code review reference
✅ API implementation
✅ Database design
✅ Security audits

### Regenerating

```bash
# After updating technologies.md or backlog.md
/generate-docs
```

---

## 🏗️ Architecture Document

**Filename:** `architecture.md`
**Template:** `.project-management/templates/architecture-template.md`

### Purpose
Define overall system design and structure

### Audience
- System architects
- Senior developers
- DevOps engineers
- Technical decision-makers

### Key Sections

**1. System Overview**
- High-level architecture
- Design principles
- Technology choices

**2. Component Architecture**
- Frontend components
- Backend services
- Database design
- External integrations

**3. Data Architecture**
- Data models
- Data flow
- Storage strategy
- Caching strategy

**4. Infrastructure**
- Hosting environment
- Deployment architecture
- CI/CD pipeline
- Monitoring and logging

**5. Security Architecture**
- Authentication flow
- Authorization model
- Data encryption
- Security layers

**6. Performance & Scalability**
- Performance targets
- Scaling strategy
- Load balancing
- Caching strategy

**7. Integration Points**
- Third-party APIs
- External services
- Webhooks
- Event streams

### When to Use

✅ Architectural decision reviews
✅ System design discussions
✅ Infrastructure planning
✅ Scaling strategy
✅ Technical debt assessment
✅ Security reviews

### Regenerating

```bash
# After major architectural changes
/generate-docs
```

---

## 📡 API Specification (Optional)

**Filename:** `api-spec.md`
**Generated:** Only if API endpoints defined in backlog

### Purpose
Complete API reference documentation

### Key Sections

**1. Authentication**
- Auth methods
- Token management
- Refresh strategies

**2. Endpoints**
- All REST/GraphQL endpoints
- Request formats
- Response formats
- Error codes

**3. Error Handling**
- Error response structure
- HTTP status codes
- Error messages

**4. Rate Limiting**
- Rate limit policies
- Throttling rules

**5. Versioning**
- API version strategy
- Breaking change policy

### When to Use

✅ API development
✅ Frontend-backend integration
✅ Third-party integrations
✅ API documentation for clients

---

## 🔄 Documentation Lifecycle

### Initial Generation

```bash
# Fill input files first
# Edit: scope.md, backlog.md, technologies.md, constraints.md

# Initialize project
/init-project

# Claude generates ALL documentation automatically
```

### Updating Documentation

**When to regenerate:**
- ✅ Input files changed (scope, backlog, technologies)
- ✅ New features added to backlog
- ✅ Tech stack changes
- ✅ Major architectural changes

**How to regenerate:**
```bash
/generate-docs
```

**What happens:**
- Claude reads current input files
- Updates all documentation
- Preserves manual additions (if marked)
- Maintains consistency across docs

### Version Control

**Best practices:**
- ✅ Commit generated docs with code
- ✅ Track changes in git
- ✅ Review doc diffs before committing
- ✅ Keep docs and code in sync

---

## 📊 Documentation Quality Checklist

After generation, verify:

- [ ] No placeholder text (e.g., `[TODO]`, `[FILL IN]`)
- [ ] All sections filled completely
- [ ] Input files were complete before generation
- [ ] Tech stack versions specified
- [ ] Success metrics are measurable
- [ ] All user stories included
- [ ] Dependencies documented
- [ ] Timeline is realistic
- [ ] Security considerations included
- [ ] Testing strategy defined

**If you see placeholders:**
```bash
# Fill missing sections in input files
# Then regenerate:
/generate-docs
```

---

## 💡 Tips for Quality Documentation

### 1. Complete Input Files First
Don't run `/init-project` or `/generate-docs` with incomplete input files. Fill everything first.

### 2. Be Specific
Vague inputs = vague documentation. Specificity is key.

### 3. Keep in Sync
When code changes, update input files and regenerate docs.

### 4. Use as Living Documents
Documentation should evolve with the project.

### 5. Share with Team
Make sure everyone has access and reads the docs.

---

## 🆚 Documentation vs Input Files

| Input Files | Generated Docs |
|-------------|----------------|
| YOU write | CLAUDE writes |
| Source of truth | Derived from inputs |
| Simple, focused | Comprehensive, detailed |
| Update anytime | Regenerate from inputs |
| 4 files | 3-4 files |
| Human-editable | Auto-generated |

**Golden rule:** Edit input files, not generated docs (they'll be overwritten).

---

[← Back to README](../README.md)
