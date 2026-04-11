# Process Client Docs - Reading Module

**Purpose:** Strategies for reading and understanding different types of client documents.

**Parent Command:** `/process-client-docs`

---

## Document Discovery

### Find All Documents

**Search in:**
```bash
.project-management/client-input/
```

**Supported file types:**
- PDF (`.pdf`)
- Word Documents (`.docx`, `.doc`)
- Text files (`.txt`, `.md`)
- Images (`.png`, `.jpg`, `.jpeg`)
- Spreadsheets (`.xlsx`, `.csv`) - optional

**Discovery process:**
```typescript
// 1. List all files
const files = readDirectory('.project-management/client-input/');

// 2. Filter by supported types
const supportedFiles = files.filter(f =>
  /\.(pdf|docx?|txt|md|png|jpe?g|xlsx|csv)$/i.test(f)
);

// 3. Categorize by type
const documents = {
  pdfs: supportedFiles.filter(f => /\.pdf$/i.test(f)),
  word: supportedFiles.filter(f => /\.docx?$/i.test(f)),
  text: supportedFiles.filter(f => /\.(txt|md)$/i.test(f)),
  images: supportedFiles.filter(f => /\.(png|jpe?g)$/i.test(f)),
  spreadsheets: supportedFiles.filter(f => /\.(xlsx|csv)$/i.test(f)),
};
```

---

## Reading Strategies by File Type

### PDF Documents

**Reading approach:**
1. Read entire document page by page
2. Extract text content
3. Identify sections (headers, lists, tables)
4. Capture structure and hierarchy

**Content to extract:**
- Headings and sections
- Paragraphs of text
- Bulleted/numbered lists
- Tables and data
- Page numbers for reference

**Tools:**
- Claude Code's Read tool (supports PDFs)
- Extract both text and visual layout

**Example extraction:**
```
Page 1:
  Heading: "Project Requirements Document"
  Section: "Overview"
  Text: "The project aims to create..."

Page 2:
  Heading: "Features"
  List:
    - User authentication
    - Product catalog
    - Shopping cart
```

---

### Word Documents (.docx)

**Reading approach:**
1. Read complete document
2. Preserve formatting (bold, italic, lists)
3. Extract headings hierarchy (H1, H2, H3)
4. Capture tables and images

**Content to extract:**
- Document structure (headings)
- Formatted text (bold = emphasis)
- Lists and bullet points
- Tables with data
- Comments and track changes (if present)

**Example:**
```
# Main Heading
## Sub Heading
- **Bold text** indicates priority
- _Italic text_ indicates notes
- Regular text is standard requirement
```

---

### Text Files (.txt, .md)

**Reading approach:**
1. Read entire file
2. Parse Markdown syntax (if .md)
3. Identify structure from formatting
4. Extract sections and lists

**Markdown parsing:**
```markdown
# Project Name
## Requirements
- Feature 1
- Feature 2

## Timeline
Launch: Q2 2026
```

**Plain text parsing:**
```
PROJECT NAME: E-Commerce Platform
REQUIREMENTS:
  - User authentication
  - Product catalog
TIMELINE:
  Launch: Q2 2026
```

---

### Images (Wireframes, Mockups)

**Reading approach:**
1. View image visually
2. Identify type (wireframe, mockup, diagram)
3. Extract visual elements
4. Note labels and annotations
5. Describe user flow or layout

**What to extract:**
- Screen layouts
- Component placement
- User interaction flows
- Button labels and text
- Navigation structure

**Example description:**
```
Wireframe: Login Screen
- Header: "Welcome Back"
- Input fields: Email, Password
- Button: "Login"
- Link: "Forgot Password?"
- Link: "Sign Up"
Layout: Centered, mobile-responsive
```

**Use for:**
- Creating user stories
- Defining UI requirements
- Understanding user flow
- Reference in acceptance criteria

---

### Spreadsheets (.xlsx, .csv)

**Reading approach:**
1. Read each sheet/tab
2. Identify headers
3. Extract rows of data
4. Understand relationships

**Common uses:**
- User stories list
- Feature priorities
- Timeline/Gantt chart
- Budget breakdown

**Example:**
```
Sheet: "User Stories"
ID | Story | Priority | Points
US-001 | User registration | P0 | 5
US-002 | User login | P0 | 3
US-003 | Password reset | P1 | 3
```

---

## Document Type Identification

### Identify Document Purpose

**Categorize documents:**

1. **Requirements Document**
   - Contains features, user stories, functional requirements
   - Keywords: "requirements", "features", "must have", "should have"
   - Extract: Features, epics, stories

2. **Project Brief/Proposal**
   - Contains vision, goals, objectives
   - Keywords: "vision", "objectives", "goals", "purpose"
   - Extract: Scope, vision, objectives

3. **Technical Specification**
   - Contains tech stack, architecture, infrastructure
   - Keywords: "technology", "stack", "architecture", "database"
   - Extract: Technologies, constraints

4. **Timeline/Schedule**
   - Contains dates, milestones, deadlines
   - Keywords: "timeline", "schedule", "deadline", "milestone"
   - Extract: Constraints, phases

5. **Wireframes/Mockups**
   - Visual representations of UI
   - File types: Images, design files
   - Extract: UI requirements, user flows

6. **Meeting Notes/Emails**
   - Informal requirements and decisions
   - Keywords: "decided", "agreed", "must", "need"
   - Extract: Requirements, assumptions, decisions

---

## Reading Techniques

### Sequential Reading

**For comprehensive documents:**
1. Read from beginning to end
2. Take notes on each section
3. Mark important items
4. Highlight ambiguities

---

### Scanning

**For quick overview:**
1. Read headings and subheadings
2. Skim first/last paragraphs
3. Note bullet points
4. Check tables/diagrams

---

### Targeted Reading

**For specific information:**
1. Search for keywords (e.g., "must", "should", "requirements")
2. Jump to relevant sections
3. Extract specific data
4. Cross-reference with other documents

---

## Language Detection and Translation

### Detect Document Language

**Check for non-English documents:**
```typescript
// Common non-English indicators
const nonEnglishIndicators = {
  serbian: ['projekat', 'zahtevi', 'korisnik'],
  spanish: ['proyecto', 'requisitos', 'usuario'],
  french: ['projet', 'exigences', 'utilisateur'],
  // etc.
};

// Sample first few paragraphs
// Detect language
// Flag for translation
```

**CRITICAL per `.CLAUDE.MD`:**
- ALL output MUST be in English
- Translate non-English sources during extraction
- Preserve meaning, not literal translation

---

## Handling Multiple Documents

### Reading Order Strategy

**Recommended order:**
1. **Project brief/proposal** (understand vision first)
2. **Requirements document** (get detailed features)
3. **Wireframes/mockups** (visualize the product)
4. **Technical specs** (understand tech constraints)
5. **Timeline/budget** (know constraints)
6. **Meeting notes** (fill in gaps)

---

### Cross-Referencing

**Link information across documents:**
```
Document 1 (brief.pdf): "Users should be able to search products"
Document 2 (wireframe.png): Shows search bar in header
Document 3 (requirements.docx): "Search with filters by category, price"

Combined requirement:
US-045: Product Search
- Search bar in header
- Filter by category and price range
- Real-time results
```

---

## Extraction Notes

### Take Structured Notes

**While reading, capture:**
```markdown
# Document: project-proposal.pdf

## Vision
"Create marketplace for local artisans"

## Features Mentioned
- [ ] User registration (p. 3)
- [ ] Product listings (p. 4)
- [ ] Shopping cart (p. 5)

## Technology Preferences
- Must use React (p. 8)
- PostgreSQL preferred (p. 9)

## Timeline
- Launch: June 2026 (p. 12)

## Ambiguities
- Payment gateway not specified
- Admin features unclear
- Mobile app mentioned but no details
```

---

## Quality Checks During Reading

**Ensure you:**
- [ ] Read EVERY document completely
- [ ] Don't skip tables, diagrams, footnotes
- [ ] Note page/section references
- [ ] Flag unclear requirements
- [ ] Identify implicit requirements
- [ ] Cross-reference contradictions
- [ ] Capture ALL features (no cherry-picking)

---

## Error Handling

**If file cannot be read:**
```
   Warning: Unable to read 'corrupted-file.pdf'
   ’ Skipping this file
   ’ Notify user after processing
```

**If file is empty:**
```
   Warning: 'empty-doc.docx' contains no content
   ’ Skipping this file
```

**If format is unsupported:**
```
   Warning: 'design.sketch' format not supported
   ’ Skipping this file
   ’ Suggest conversion to PDF or image
```

---

**Related:**
- Parent: `.claude/commands/process-client-docs.md`
- Sibling: `process-client-docs-extraction.md`
- Rules: `.CLAUDE.MD` (English-only documentation)
