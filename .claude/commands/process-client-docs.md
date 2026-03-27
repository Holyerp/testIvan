---
name: process-client-docs
description: Extract project requirements from client documents and generate structured input files (scope, backlog, technologies, constraints)
---

# Process Client Documents

You are extracting project requirements from client-provided documents and generating structured input files.

---

## Your Task

1. **Find and read ALL documents** in `.project-management/client-input/`:
   - PDF files (.pdf)
   - Word documents (.docx, .doc)
   - Text files (.txt, .md)
   - Images (.png, .jpg) - mockups, wireframes, diagrams
   - ANY other readable files

2. **Analyze document content** to extract:
   - Project vision and goals
   - Features and requirements
   - Target audience
   - Success criteria
   - Technology mentions
   - Timeline and budget constraints
   - Dependencies and risks

3. **Generate or update input files**:
   - `.project-management/input/scope.md`
   - `.project-management/input/backlog.md`
   - `.project-management/input/technologies.md`
   - `.project-management/input/constraints.md`

4. **Provide summary** of what was extracted and what needs clarification

---

## Important Guidelines

### Reading Documents

**For Each Document:**
- Read the ENTIRE document carefully
- Extract all requirements, features, and context
- Note client's emphasis (helps with priorities)
- Identify implicit requirements
- Look for constraints, assumptions, risks

**For PDFs:**
- Read all pages
- Pay attention to tables, lists, diagrams
- Extract text from images if present

**For Images (mockups/wireframes):**
- Identify UI components and features
- Infer user flows
- Note technology hints (e.g., mobile app, web app)
- Extract any text or labels

**For Word Documents:**
- Read all sections
- Pay attention to headings and structure
- Extract action items and requirements

**For Text/Email:**
- Look for feature requests
- Identify decisions made
- Note questions and answers

---

## Extraction Strategy

### 1. First Pass - Understand Context

Read all documents to understand:
- What is this project about?
- Who is it for?
- What problem does it solve?
- What's the scope?

### 2. Second Pass - Extract Details

**For scope.md:**
- Project name and vision
- Target audience (demographics, roles)
- Core objectives (what must be achieved)
- Success metrics (KPIs)
- Out of scope (what's NOT included)
- Assumptions
- Risks

**For backlog.md:**
- All features mentioned
- Convert to user stories format:
  - "As a [user], I want [feature], so that [benefit]"
- Assign priorities:
  - P0: Critical, must-have (mentioned as essential/required/critical)
  - P1: High priority (mentioned as important/should have)
  - P2: Medium priority (mentioned as nice-to-have/future)
  - P3: Low priority (mentioned as optional/maybe)
- Create acceptance criteria
- Estimate story points (if enough detail)
- Note dependencies

**For technologies.md:**
- Mentioned technologies (React, Node.js, PostgreSQL, etc.)
- Required integrations (Stripe, AWS, APIs)
- Technology constraints (must use X, can't use Y)
- Infrastructure requirements
- Performance requirements

**For constraints.md:**
- Timeline (deadlines, milestones)
- Budget (total budget, monthly costs)
- Team (size, availability, skills)
- Technical constraints (platform, browser support)
- Legal/compliance requirements
- Dependencies on external parties

---

## Handling Incomplete Information

### If Information is Missing:

**Mark sections clearly:**
```markdown
## Target Audience
[NEEDS CLARIFICATION]
Documents don't specify target user demographics.

**Questions for Client:**
1. Who are the primary users?
2. What is their technical proficiency level?
3. Age range and geographic location?
```

**Infer when reasonable:**
- If mockups show mobile app → note mobile-first in technologies.md
- If payment mentioned → add payment processing to backlog
- If admin panel shown → add admin features to backlog

**Don't guess critical details:**
- Don't invent budget numbers
- Don't assume deadlines
- Don't fabricate features not mentioned

---

## Priority Assignment Rules

**P0 (Critical):**
- Explicitly marked as "must have", "required", "essential"
- Core functionality without which app doesn't work
- Example: Authentication, core CRUD operations

**P1 (High):**
- Marked as "should have", "important"
- Important but app could launch without
- Example: Password reset, user profile

**P2 (Medium):**
- Marked as "nice to have", "would like"
- Enhances experience but not critical
- Example: Dark mode, export to CSV

**P3 (Low):**
- Marked as "future", "optional", "maybe"
- Future consideration
- Example: AI recommendations, social sharing

**When in doubt:** Use P1 (high) as default for mentioned features.

---

## User Story Creation

### Convert Requirements to User Stories

**From:** "The system should allow users to create products"

**To:**
```markdown
### US-001: Create Product Listing
**Priority:** P0
**Story:** As a vendor, I want to create product listings so that customers can browse and purchase my products.

**Acceptance Criteria:**
- [ ] Vendor can add product title, description, price
- [ ] Multiple product images can be uploaded (max 5)
- [ ] Product categories and tags are supported
- [ ] Inventory quantity can be set
- [ ] Draft and publish states available

**Estimate:** 8 story points
**Dependencies:** US-000 (User Authentication)
```

### Creating Comprehensive User Stories

Include:
- **Story ID:** US-001, US-002, etc.
- **Title:** Clear, action-oriented
- **Priority:** P0, P1, P2, P3
- **Story format:** As a [user], I want [action], so that [benefit]
- **Acceptance criteria:** Clear, testable conditions (checkbox list)
- **Estimate:** Story points (1, 2, 3, 5, 8, 13, 21)
- **Dependencies:** Other stories that must be done first

---

## Merging with Existing Files

### If Input Files Already Exist:

**DON'T overwrite completely!**

Instead:
1. Read existing content
2. Identify new information from client docs
3. Merge new info with existing:
   - Add new features to backlog
   - Update scope with new objectives
   - Preserve manually added content
   - Mark conflicts clearly

**Example merge:**
```markdown
## Existing in scope.md:
Target Audience: Small business owners

## New from client docs:
Target Audience: Specifically targeting artisan vendors and craftspeople

## Merged result:
Target Audience: Small business owners, specifically artisan vendors and craftspeople who create handmade products
```

---

## Story Point Estimation

Use Fibonacci sequence: 1, 2, 3, 5, 8, 13, 21

**Guidelines:**
- **1-2 points:** Few hours, trivial (e.g., add a button, simple text field)
- **3 points:** Half day, straightforward (e.g., simple form with validation)
- **5 points:** 1-2 days, some complexity (e.g., CRUD operations for one entity)
- **8 points:** 2-4 days, significant work (e.g., complex form with file uploads)
- **13 points:** 1 week, complex feature (e.g., payment integration)
- **21+ points:** Too large, break down into smaller stories

**If unclear:** Use 5 points as default and note "[ESTIMATE TO BE REFINED]"

---

## Quality Checklist

Before completing, verify:

**For scope.md:**
- [ ] Project vision is clear and compelling
- [ ] Target audience is specific
- [ ] Core objectives are measurable
- [ ] Success criteria are defined
- [ ] Out of scope is explicit
- [ ] Assumptions and risks are listed

**For backlog.md:**
- [ ] All mentioned features are included
- [ ] User stories follow proper format
- [ ] Priorities are assigned (P0/P1/P2/P3)
- [ ] Acceptance criteria are testable
- [ ] Dependencies are noted
- [ ] Minimum 10+ user stories (unless very small project)

**For technologies.md:**
- [ ] Mentioned technologies are listed
- [ ] Required integrations are noted
- [ ] Tech constraints are documented
- [ ] Rationale is provided (why this tech stack)

**For constraints.md:**
- [ ] Timeline is documented (if mentioned)
- [ ] Budget is noted (if mentioned)
- [ ] Team size/availability (if mentioned)
- [ ] Technical constraints (if any)
- [ ] Compliance requirements (if any)

---

## Output Format

After processing, provide this summary:

```markdown
# Client Documents Processing Complete

## Documents Processed
✅ client-brief.pdf (5 pages)
✅ requirements.docx (12 pages)
✅ mockups.png (UI design - 8 screens)
✅ timeline.txt (1 page)

## Generated/Updated Files

### ✅ scope.md
- **Project:** E-Commerce Platform for Artisans
- **Vision:** Extracted from project brief
- **Target Audience:** Artisan vendors and customers
- **Core Objectives:** 5 objectives identified
- **Success Criteria:** 4 KPIs defined

### ✅ backlog.md
- **Total User Stories:** 23
  - P0 (Critical): 8 stories (65 points)
  - P1 (High): 10 stories (45 points)
  - P2 (Medium): 5 stories (20 points)
- **Epics:** 5 epics identified
  - Authentication & Authorization
  - Product Management
  - Shopping Cart & Checkout
  - Order Management
  - Vendor Dashboard

### ✅ technologies.md
- **Frontend:** React (inferred from mockups)
- **Backend:** Node.js + Express
- **Database:** PostgreSQL (mentioned in requirements)
- **Integrations:** Stripe (payment), SendGrid (email)

### ✅ constraints.md
- **Timeline:** 3 months to MVP (from timeline.txt)
- **Budget:** $50,000 total (mentioned in brief)
- **Team:** 2 developers, 1 designer (from project brief)
- **Compliance:** GDPR required (European customers)

## Summary Statistics
- Features Identified: 23
- Pages Analyzed: 18
- Images Processed: 1
- Total Story Points: 130

## Items Needing Clarification

⚠️ **High Priority:**
1. **Database choice:** PostgreSQL mentioned but not confirmed - please verify
2. **Mobile app:** Mockups show mobile UI but scope doesn't mention native apps - web-only or native too?
3. **Payment methods:** Stripe mentioned, but should we support PayPal too?

⚠️ **Medium Priority:**
1. User roles: Are there multiple vendor tiers (basic, premium)?
2. Internationalization: Should we support multiple languages?
3. Email templates: Who designs these - client or us?

## Recommendations

💡 **Before running /init-project:**
1. Review generated scope.md and adjust target audience if needed
2. Verify technology choices in technologies.md
3. Confirm priorities in backlog.md (P0 vs P1)
4. Answer clarification questions above
5. Add any missing features to backlog.md

💡 **Next Steps:**
1. Review all generated files in `.project-management/input/`
2. Edit/refine as needed
3. Answer clarification questions
4. Run `/init-project` when ready

## Questions?

If you need me to:
- Elaborate on specific user stories
- Add more detail to any section
- Clarify extraction decisions
- Re-process with additional documents

Just ask!
```

---

## Special Cases

### Multiple Clients/Stakeholders

If documents have conflicting requirements:
```markdown
## [CONFLICT IDENTIFIED]

**Source 1 (client-brief.pdf):**
"Must support 10,000 concurrent users"

**Source 2 (meeting-notes.txt):**
"Start with support for 500 users, scale later"

**Recommendation:** Add to constraints.md as MVP target: 500 users, with scalability plan to 10,000.

**Action Required:** Clarify with client which is the initial target.
```

### Vague Requirements

If requirements are too vague:
```markdown
### US-015: Social Features
**Priority:** P2
**Story:** As a user, I want social features.

[NEEDS DETAIL]
Client mentioned "social features" but didn't specify:
- What social features? (sharing, comments, likes, follows?)
- Which social platforms? (Facebook, Twitter, Instagram?)
- For what purpose? (viral growth, community building?)

**Questions for Client:**
1. What specific social features do you want?
2. How should users share content?
3. Should users be able to follow each other?
```

---

## Technical Requirements Extraction

### From Mockups/Wireframes:

**Identify:**
- Platform (web, mobile, desktop)
- UI framework hints (Material Design → maybe Material-UI)
- Features visible in screens
- User flows
- Required pages/screens

**Example:**
```
Mockups show:
- Responsive design (mobile + desktop views)
- Modern UI with cards and modals
- Real-time updates (chat/notifications)
- File upload with preview
- Complex data tables

→ Suggests: React, modern CSS framework, WebSockets, file handling
```

---

## Remember

- **Be thorough:** Read every document completely
- **Be specific:** Extract concrete requirements, not vague statements
- **Be organized:** Structure user stories clearly
- **Be realistic:** Assign reasonable priorities and estimates
- **Be helpful:** Identify gaps and ask clarifying questions
- **Don't invent:** Only extract what's actually in the documents

**The goal:** Generate input files that are 80% complete, requiring only minor refinement before running `/init-project`.

---

## Final Note

After generating all files, the user should:
1. Review `.project-management/input/scope.md`
2. Review `.project-management/input/backlog.md`
3. Review `.project-management/input/technologies.md`
4. Review `.project-management/input/constraints.md`
5. Make any necessary edits
6. Run `/init-project` to generate complete project documentation

**Success = User can run `/init-project` with minimal edits to generated files.**
