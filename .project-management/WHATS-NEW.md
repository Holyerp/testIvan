# What's New - Client Document Processing

**Version:** 1.1
**Date:** 2026-03-25
**Feature:** Automatic requirement extraction from client documents

---

## 🎉 New Feature: Process Client Documents

You can now **automatically generate** project requirements from client-provided documents!

### What This Means

Instead of manually filling `scope.md`, `backlog.md`, etc., you can:

1. **Drop client documents** (PDFs, Word, images) into a folder
2. **Run one command**: `/process-client-docs`
3. **Get auto-generated** requirements files

Perfect for **agencies, freelancers, and consultants** who receive project briefs from clients.

---

## 📂 What Was Added

### 1. New Folder: `client-input/`

**Location:** `.project-management/client-input/`

**Purpose:** Place all client-provided documents here

**Supported formats:**
- ✅ PDF files (.pdf)
- ✅ Word documents (.docx, .doc)
- ✅ Text files (.txt, .md)
- ✅ Images (.png, .jpg) - mockups, wireframes, diagrams

**Privacy:** This folder is gitignored - client documents won't be committed

---

### 2. New Command: `/process-client-docs`

**Location:** `.claude/commands/process-client-docs.md`

**What it does:**
1. Reads ALL documents from `client-input/`
2. Analyzes content using Claude's multimodal capabilities
3. Extracts:
   - Project vision and goals → `scope.md`
   - Features and requirements → `backlog.md` (as user stories)
   - Technology mentions → `technologies.md`
   - Timeline, budget, team → `constraints.md`
4. Generates user stories with priorities (P0, P1, P2, P3)
5. Creates acceptance criteria
6. Estimates story points
7. Identifies gaps and asks clarifying questions

**Example usage:**
```bash
# Add documents
cp client-brief.pdf .project-management/client-input/
cp requirements.docx .project-management/client-input/
cp mockups.png .project-management/client-input/

# Process
/process-client-docs

# Review generated files, then initialize
/init-project
```

---

### 3. Documentation: `client-input/README.md`

**Location:** `.project-management/client-input/README.md`

**Contents:**
- Complete guide on how to use the client-input folder
- Supported file formats
- Best practices for organizing documents
- Tips for best extraction results
- Troubleshooting guide
- Real-world workflow examples

---

### 4. Example Document: `EXAMPLE-project-brief.txt`

**Location:** `.project-management/client-input/EXAMPLE-project-brief.txt`

**Purpose:**
- Shows what a good client brief looks like
- Demonstrates the level of detail Claude can extract
- Use as a template for your own projects
- Test the `/process-client-docs` command

**Contains:**
- Project overview and vision
- Target market
- Core requirements (must-have, nice-to-have, future)
- Technical requirements
- Timeline and budget
- Team composition
- Success metrics
- Constraints and assumptions
- Risks and mitigation

---

### 5. Updated Documentation

**Files updated:**
- `.project-management/README.md` - Added client-docs workflow
- `.project-management/SYSTEM-OVERVIEW.md` - Added client-input to file map
- `.gitignore` - Client documents are private (not committed)

---

## 🚀 How to Use

### Quick Start

```bash
# 1. Add your client's documents
cp /path/to/client-documents/* .project-management/client-input/

# 2. Process documents
/process-client-docs

# 3. Review generated input files
# Edit .project-management/input/*.md if needed

# 4. Initialize project
/init-project

# Done! Complete project documentation generated
```

---

## 💡 Use Cases

### 1. Agency Receiving RFP
```
Client sends:
- RFP.pdf
- Wireframes.fig (exported as PNG)
- Meeting-notes.txt

You run:
/process-client-docs

Result:
- 35 user stories extracted
- Project scope defined
- Timeline and budget captured
```

### 2. Freelancer Getting Project Brief
```
Client emails:
- Project-brief.docx
- Feature-list.txt

You run:
/process-client-docs

Result:
- scope.md fully populated
- backlog.md with 20 stories
- Clarification questions identified
```

### 3. Consultant Analyzing Requirements
```
Client provides:
- Technical-spec.pdf
- Mockups/ folder (10 images)
- Constraints.txt

You run:
/process-client-docs

Result:
- Technologies extracted from tech spec
- Features inferred from mockups
- Constraints captured
```

---

## 🎯 What Gets Extracted

### From Any Document → scope.md
- **Project Vision:** Overall purpose and goals
- **Target Audience:** Who will use the platform
- **Core Objectives:** Main goals to achieve
- **Success Criteria:** KPIs and metrics
- **Out of Scope:** What's NOT included
- **Assumptions:** Stated assumptions
- **Risks:** Identified risks and mitigation

### From Requirements → backlog.md
- **User Stories:** In proper format
  ```
  As a [user], I want [feature], so that [benefit]
  ```
- **Priorities:** P0 (must-have), P1 (should-have), P2 (nice-to-have)
- **Acceptance Criteria:** Testable conditions
- **Story Points:** Estimated effort (Fibonacci)
- **Dependencies:** What must be done first

### From Technical Docs → technologies.md
- **Tech Stack:** Mentioned technologies
- **Integrations:** External services (Stripe, AWS, etc.)
- **Constraints:** Technology restrictions
- **Performance Targets:** Load time, scalability requirements

### From Timeline/Budget → constraints.md
- **Timeline:** Deadlines and milestones
- **Budget:** Total and breakdown
- **Team:** Size, availability, skills
- **Compliance:** GDPR, PCI, etc.

---

## 🔍 What Claude Reads

### Text Extraction:
- **PDFs:** All pages, including tables and lists
- **Word:** Full document with formatting preserved
- **Text files:** Complete content

### Image Analysis:
- **Mockups:** UI components, screens, user flows
- **Wireframes:** Layout, features, navigation
- **Diagrams:** Architecture, data flow, relationships
- **Screenshots:** Existing systems, examples

### Smart Extraction:
- Identifies headers and structure
- Recognizes lists and requirements
- Infers priorities from emphasis
- Detects implicit requirements
- Identifies conflicts between documents
- Notes missing information

---

## ⚡ Benefits

### For You:
- ✅ **Save Time:** No manual transcription of client requirements
- ✅ **Reduce Errors:** No missing requirements
- ✅ **Structured Output:** Proper user stories, priorities, estimates
- ✅ **Comprehensive:** Claude reads everything thoroughly
- ✅ **Ask Questions:** Claude identifies gaps and ambiguities
- ✅ **Iterative:** Add more documents anytime, Claude merges info

### For Your Client:
- ✅ **Fast Turnaround:** From brief to project plan in minutes
- ✅ **Clear Requirements:** Well-structured, unambiguous specs
- ✅ **Transparency:** All requirements documented clearly
- ✅ **Professional:** Consistent, thorough documentation

---

## 🔄 Iterative Process

### Round 1:
```
client-input/
└── project-brief.pdf

Run: /process-client-docs
Result: Basic scope and 15 user stories

Gaps: Missing mockups, unclear on some features
```

### Round 2:
```
client-input/
├── project-brief.pdf (already processed)
├── mockups.png (NEW)
└── feature-clarifications.txt (NEW)

Run: /process-client-docs again
Result: Updated with 8 more stories from mockups, clarifications merged
```

### Round 3:
```
Manually edit:
- input/backlog.md (adjust priorities)
- input/scope.md (refine vision)

Run: /init-project
Result: Final documentation generated
```

---

## 📊 Comparison

### Before (Manual):
```
1. Read client brief (30 min)
2. Extract requirements manually (2 hours)
3. Write user stories (2 hours)
4. Prioritize and estimate (1 hour)
5. Format into templates (1 hour)

Total: ~6 hours
```

### After (Automatic):
```
1. Add documents to folder (2 min)
2. Run /process-client-docs (1 min)
3. Review generated files (30 min)
4. Edit if needed (30 min)

Total: ~1 hour
```

**Time Saved: ~5 hours per project!**

---

## 🛠️ Technical Details

### How It Works:

1. **Document Discovery:**
   - Scans `client-input/` folder
   - Identifies all readable files
   - Sorts by name (use numbered prefixes for order)

2. **Content Extraction:**
   - PDFs: Text + images extracted
   - Word: Full content with structure
   - Images: Visual analysis + OCR
   - Text: Plain text reading

3. **Analysis:**
   - Identifies requirements and features
   - Converts to user story format
   - Assigns priorities based on emphasis
   - Creates acceptance criteria
   - Estimates story points

4. **Generation:**
   - Uses templates from `templates/` folder
   - Replaces placeholders with extracted content
   - Maintains consistency across files
   - Formats properly (markdown)

5. **Validation:**
   - Checks for completeness
   - Identifies missing information
   - Asks clarifying questions
   - Provides recommendations

---

## 🔒 Privacy & Security

### Gitignore Protection:
```gitignore
# Client documents won't be committed
.project-management/client-input/*
!.project-management/client-input/README.md
!.project-management/client-input/EXAMPLE-*.txt
```

### Best Practices:
- ✅ Client documents stay local
- ✅ Don't commit sensitive docs
- ✅ Clear folder between projects
- ✅ Use secure file transfers from clients
- ✅ Delete docs after project setup if needed

---

## 📖 Examples to Try

### 1. Test with Example:
```bash
# The example brief is already there
/process-client-docs

# Claude will extract:
# - Project: E-Commerce for Artisans
# - 23 user stories
# - Timeline: 10 weeks
# - Budget: $75,000
```

### 2. Add Your Own:
```bash
# Add your client's documents
cp /path/to/your-project-brief.pdf client-input/

# Process
/process-client-docs

# Review and refine
```

---

## ❓ FAQ

**Q: Can I mix manual and automatic?**
A: Yes! Run `/process-client-docs` first, then manually edit generated files.

**Q: What if extraction is wrong?**
A: Edit the generated input files. They're just starting points.

**Q: Can I re-run after adding more docs?**
A: Yes! Claude will merge new info with existing files.

**Q: What if client brief is incomplete?**
A: Claude will identify gaps and ask questions. Fill in missing info manually.

**Q: Does it work with non-English documents?**
A: Yes, Claude can read multiple languages, though English works best.

**Q: Can I use images only (no text docs)?**
A: Yes! Claude can extract from mockups/wireframes alone.

---

## 🎓 Best Practices

### 1. Organize Documents:
```
client-input/
├── 01-brief.pdf          # Main brief (read first)
├── 02-requirements.docx  # Detailed requirements
├── 03-mockups.png        # UI designs
├── 04-timeline.txt       # Deadlines
└── 05-notes.md           # Additional context
```

Use numbers so Claude processes in logical order.

### 2. Include Context:
- Project brief or RFP
- Meeting notes with decisions
- Email threads with clarifications
- Mockups and wireframes
- Technical constraints

### 3. Review Before Init:
- Always review generated files
- Adjust priorities if needed
- Fill in any [NEEDS CLARIFICATION] sections
- Add missing details

### 4. Iterate:
- First pass: Get 80% from documents
- Second pass: Add more docs, re-run
- Third pass: Manual refinement
- Then: /init-project

---

## 🔗 Related Documentation

- [Client Input README](client-input/README.md) - Full usage guide
- [System Overview](SYSTEM-OVERVIEW.md) - Complete file map
- [Integration Guide](INTEGRATION-GUIDE.md) - How everything works
- [Main README](README.md) - Complete system documentation

---

## 🎉 Try It Now!

```bash
# Test with the example brief
/process-client-docs

# Or add your own documents
cp /path/to/client-docs/* .project-management/client-input/
/process-client-docs

# Review generated files
ls .project-management/input/

# Initialize when ready
/init-project
```

**Welcome to automated project planning!** 🚀
