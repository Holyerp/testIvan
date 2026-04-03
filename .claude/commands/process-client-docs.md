---
name: process-client-docs
description: Extract project requirements from client documents and generate structured input files (scope, backlog, technologies, constraints)
---

# Process Client Documents

**📖 Quick Start:** See [how-to-use/process-client-docs.md](./how-to-use/process-client-docs.md) for quick guide (~120 lines)

Extract project requirements from client-provided documents and generate structured input files.

**🌍 CRITICAL: Generate ALL output files in English only, regardless of source document language.**

---

## Usage

```bash
/process-client-docs
```

Reads all documents from `.project-management/client-input/` and generates/updates input files.

---

## 📋 YOUR TASK

**🔧 DOCUMENTATION RULES:**
All extracted requirements and generated files must follow:
- **`.CLAUDE.MD`** - All documentation in English only (translate non-English sources)
- **`.claude/rules/git.md`** - If committing extracted requirements (NO AI credits, conventional commits)

---

### STEP 0: ENTER PLAN MODE (MANDATORY)

**🎯 MANDATORY: Always enter plan mode before processing client documents**

**Claude must:**

1. **Scan client-input folder:**
   - List all files in `.project-management/client-input/`
   - Identify file types (PDF, Word, images, text)
   - Check file accessibility

2. **Quick preview of each document:**
   - Read first few pages/sections
   - Identify document purpose (requirements, mockups, timeline, etc.)
   - Note language (will translate to English if needed)

3. **Analyze extraction strategy:**
   - Which docs contain requirements
   - Which docs contain technical specs
   - Which docs contain constraints
   - How to organize into input files

4. **Create processing plan:**
   ```
   CLIENT DOCUMENT PROCESSING PLAN

   DOCUMENTS FOUND:
   - project-brief.pdf (5 pages, English)
   - requirements.docx (15 pages, English)
   - mockups.png (3 images)

   EXTRACTION PLAN:
   1. project-brief.pdf → scope.md (vision, goals)
   2. requirements.docx → backlog.md (25 user stories)
   3. mockups.png → reference in stories
   4. Suggest tech stack → technologies.md
   5. Extract timeline → constraints.md

   ESTIMATED OUTPUT:
   - scope.md: ~100 lines
   - backlog.md: ~350 lines (25 stories)
   - technologies.md: ~60 lines (suggested)
   - constraints.md: ~50 lines

   Proceed? [Yes / No / Revise]
   ```

5. **Wait for user approval**
6. **Only proceed to STEP 1 after approval**

---

### STEP 1: Find and Read Documents

**📖 See:** `modules/process-client-docs-reading.md` for detailed document reading strategies

**Summary:**
- Find ALL documents in `.project-management/client-input/`
- Support: PDF, Word (.docx), Text (.txt, .md), Images (.png, .jpg)
- Read each document completely
- Extract requirements, features, context

---

### STEP 2: Extract Requirements

**📖 See:** `modules/process-client-docs-extraction.md` for detailed extraction strategies

**Extract from documents:**
- Project vision and goals
- Features and requirements (functional & non-functional)
- Target audience and user personas
- Success criteria and KPIs
- Technology mentions or preferences
- Timeline and budget constraints
- Dependencies, assumptions, risks
- Implicit requirements (read between the lines)

---

### STEP 3: Generate Input Files

**Generate or update 4 input files:**

#### 1. `scope.md` - Project Scope
- Project name and vision
- Target audience
- Core objectives (3-5 key goals)
- Success criteria
- Out of scope (explicitly NOT included)
- Stakeholders
- Dependencies, assumptions, risks

#### 2. `backlog.md` - Product Backlog
- Organize by Epics
- Write user stories (As a... I want... So that...)
- Add acceptance criteria
- Assign priorities (P0/P1/P2/P3)
- Estimate story points
- Identify dependencies

#### 3. `technologies.md` - Tech Stack
- Extract mentioned technologies
- Note client preferences
- Document constraints (must use X, cannot use Y)
- Add recommendations if gaps exist

#### 4. `constraints.md` - Project Constraints
- Timeline constraints (deadlines, milestones)
- Budget constraints
- Team constraints (size, skills, availability)
- Technical constraints (existing systems, infrastructure)
- Compliance/legal requirements (GDPR, HIPAA, etc.)
- Quality requirements (performance, security, scalability)

---

### STEP 4: Summary Report

**Display comprehensive summary:**

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📄 CLIENT DOCUMENTS PROCESSED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📂 DOCUMENTS READ:
✅ {{document_1.pdf}} ({{pages}} pages)
✅ {{document_2.docx}} ({{pages}} pages)
✅ {{mockup.png}} (wireframe analyzed)
{{...}}

📊 EXTRACTED INFORMATION:

Project Overview:
- Name: {{project_name}}
- Type: {{project_type}}
- Target Audience: {{audience}}

Features Identified:
- Total Epics: {{epic_count}}
- Total User Stories: {{story_count}}
- Priorities: P0 ({{p0_count}}), P1 ({{p1_count}}), P2 ({{p2_count}})

Tech Stack:
- Mentioned: {{tech_list}}
- Preferences: {{preferences}}
- Constraints: {{constraints}}

Constraints:
- Timeline: {{timeline}}
- Budget: {{budget}}
- Team: {{team_size}}

📝 FILES GENERATED/UPDATED:
✅ .project-management/input/scope.md
✅ .project-management/input/backlog.md
✅ .project-management/input/technologies.md
✅ .project-management/input/constraints.md

⚠️  NEEDS CLARIFICATION:
- [List items that are unclear or missing]
- [Questions to ask the client]
- [Assumptions made that need validation]

🎯 NEXT STEPS:
1. Review generated files for accuracy
2. Clarify ambiguous items with client
3. Run /init-project to generate documentation
```

---

## 📚 Module References

**Detailed workflows available in:**
- `modules/process-client-docs-reading.md` - Document reading strategies
- `modules/process-client-docs-extraction.md` - Requirements extraction methods

---

## ⚠️ IMPORTANT NOTES

### Extraction Principles

**Be Thorough:**
- Read EVERY document completely
- Extract ALL requirements (explicit and implicit)
- Don't skip tables, diagrams, or images

**Be Structured:**
- Organize by epics and stories
- Assign priorities based on client emphasis
- Use consistent format

**Be Smart:**
- Infer implicit requirements
- Read between the lines
- Note ambiguities for clarification

### Quality Checks

**Before finishing:**
- [ ] All documents read and processed
- [ ] All 4 input files generated/updated
- [ ] Epics and stories well-organized
- [ ] Priorities assigned logically
- [ ] Constraints documented
- [ ] Ambiguities flagged for clarification

### Common Pitfalls

**Avoid:**
- ❌ Skipping documents or sections
- ❌ Missing implicit requirements
- ❌ Poor organization (dumping everything in one epic)
- ❌ Not flagging ambiguities
- ❌ Guessing instead of asking for clarification

---

## 📝 Example Execution

```bash
# User runs:
/process-client-docs

# Claude finds documents:
📂 Found 3 documents in client-input/

# Claude processes:
🔍 Reading: project-proposal.pdf (12 pages)...
🔍 Reading: wireframes.png (mockup)...
🔍 Reading: requirements-email.txt...

# Claude extracts and generates:
📄 Extracting requirements...
✍️  Generating scope.md...
✍️  Generating backlog.md...
✍️  Generating technologies.md...
✍️  Generating constraints.md...

# Claude displays summary:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📄 CLIENT DOCUMENTS PROCESSED
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

[Complete summary with extracted info]

🎯 NEXT STEPS:
1. Review generated files
2. Ask client about [X, Y, Z]
3. Run /init-project
```

---

**Version:** 3.0.0
**Created:** 2026-03-27
**Command Type:** Requirements Engineering
