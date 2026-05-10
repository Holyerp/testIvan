---
name: process-client-docs
description: Extract project requirements from client documents and generate structured input files (scope, backlog, technologies, constraints)
---

# Process Client Documents

**📖 Quick Start:** See [how-to-use/process-client-docs.md](./how-to-use/process-client-docs.md) for quick guide (~120 lines)

Extract project requirements from client-provided documents (PDF / Word / images / text) and generate structured input files.

**🌍 CRITICAL:** Generate ALL output files in English only, regardless of source document language.

---

## Usage

```bash
/process-client-docs
```

Reads all documents from `.project-management/client-input/` and generates/updates input files.

---

## 📋 YOUR TASK

**🔧 DOCUMENTATION RULES** — extracted requirements and generated files must follow:
- `.CLAUDE.MD` — all documentation in English (translate non-English sources)
- `.claude/rules/git.md` — if committing, NO AI credits, conventional commits

---

### STEP 0 — Enter plan mode (MANDATORY)

Always enter plan mode before processing client documents.

1. **Scan `client-input/`** — list files, identify types (PDF / Word / images / text), check accessibility.
2. **Quick preview** — read first few pages/sections of each doc; identify purpose and language (translate → English if needed).
3. **Analyze extraction strategy** — which docs contain requirements / tech / constraints; how to organize into input files.
4. **Create processing plan** (format below).
5. **Wait for user approval** (`Yes / No / Revise`).
6. Only proceed to STEP 1 after approval.

**Plan format:**

```
CLIENT DOCUMENT PROCESSING PLAN

DOCUMENTS FOUND:
- project-brief.pdf (5 pages, English)
- requirements.docx (15 pages, English)
- mockups.png (3 images)

EXTRACTION PLAN:
1. project-brief.pdf → scope.md (vision, goals)
2. requirements.docx → backlog/ modular structure (~25 user stories)
3. mockups.png → referenced in stories
4. Suggest tech stack → technologies.md
5. Extract timeline → constraints.md

ESTIMATED OUTPUT:
- scope.md          ~100 lines
- backlog/          6 files, each < 200 lines
- technologies.md   ~60 lines
- constraints.md    ~50 lines

Proceed? [Yes / No / Revise]
```

---

### STEP 1 — Find & read documents

**📖 See:** `modules/process-client-docs-reading.md` for detailed reading strategies.

- Find ALL documents in `.project-management/client-input/`.
- Support: PDF, Word (`.docx`), text (`.txt`, `.md`), images (`.png`, `.jpg`).
- Read each document completely — not just first page / summary.

---

### STEP 2 — Extract requirements

**📖 See:** `modules/process-client-docs-extraction.md` for detailed extraction.

Extract from documents:

- Project vision and goals
- Features (functional + non-functional)
- Target audience and user personas
- Success criteria and KPIs
- Technology mentions / preferences
- Timeline and budget constraints
- Dependencies, assumptions, risks
- **Implicit requirements** — read between the lines

---

### STEP 2.5 — Anonymize personal information (MANDATORY)

**📖 Rule:** `.claude/rules/anonymization.md` (full role-mapping table, source-context substitutions, before/after examples, edge cases).

**TL;DR:** Names extracted from input documents MUST NOT appear in generated artifacts. Before writing any output in STEP 3, do an anonymization pass on the extracted content:

1. **Replace personal names with role labels.** Pick from the project's role inventory: `PM`, `client PM`, `tech lead`, `team lead`, `designer`, `UX lead`, `QA lead`, `DevOps`, `client`, `stakeholder`, `finance stakeholder`, `legal`, `end user`. When the role is unclear, prefer `the stakeholder` over guessing.

   - "Marko said …" → "The PM said …" or "Per our planning call …"
   - "Ana wants the dashboard to …" → "Per stakeholder feedback, the dashboard should …"

2. **Replace attributions with source channels.** Use phrases like *"Per our call,"* / *"Agreed in the planning meeting:"* / *"Per stakeholder email,"* / *"Decided on the call:"* — preserve provenance without identity.

3. **Drop, do not paraphrase, personal contact details.** Email addresses, phone numbers, personal Slack/GitHub handles → removed entirely or replaced with `the stakeholder`.

4. **Keep:** the client *company* name, generic role titles, product/tool names (Slack, Jira, Stripe …), and placeholder names in clearly-marked examples (`user@example.com`, `Jane Doe` as a sample). Anonymize only *individuals* who are project actors.

5. **Self-check before STEP 3.** Mentally (or with grep on the extraction notes) scan for first names that appeared in input docs. If any survive into the planned output, fix before writing.

If you encounter a quote that loses meaning when anonymized, keep the quote and attribute it to the role (e.g., `the client PM said: "..."`) — never invent a quote, never leak a name.

The summary report (STEP 4) MUST confirm the anonymization pass ran and flag any names that could not be cleanly removed (so the user can decide).

---

### STEP 3 — Generate input files

**📖 See:** `process-client-docs-reference.md` for the full file-format spec per output file.

Generate or update:

1. **`input/scope.md`** — project vision + audience + 3-5 objectives + success criteria + out-of-scope + stakeholders + deps/assumptions/risks.
2. **`input/backlog/`** — **modular structure**, generated **directly** (no migration step):
   - `README.md` (< 200 lines) — master index + totals + links to phase files
   - `phase-1-foundation.md` — infrastructure, auth, DB, core API (P0)
   - `phase-2-core.md` — main features, critical flows (P0/P1)
   - `phase-3-advanced.md` — secondary features (P1/P2)
   - `phase-4-polish.md` — testing, deployment, bug fixes (P2)
   - `future.md` — post-launch (P3 or "Future" marked)
3. **`input/technologies.md`** — mentioned tech + client preferences + constraints + recommendations (if gaps).
4. **`input/constraints.md`** — timeline, budget, team, technical, compliance (GDPR/HIPAA), quality (perf/security/scale).

**⚠️ File-size enforcement (strict — per `.claude/rules/documentation.md` §2.1):**
- Each backlog file **must be < 200 lines**. If a phase exceeds 200 lines, split into sub-phases (e.g. `phase-2a-core-features.md` + `phase-2b-integrations.md`).
- `README.md` **must be < 200 lines** (summary + links only).
- Target: 150-180 lines per file.

**Templates:**
- `.project-management/templates/phase-backlog-template.md` — phase files
- `.project-management/templates/backlog-readme-template.md` — README

---

### STEP 4 — Summary report

Emit the standard summary (full template in `process-client-docs-reference.md`):

- Documents read (with page counts)
- Extracted info — project overview, epic/story totals, priorities breakdown, tech, constraints
- Files generated (listed with line counts)
- **Anonymization pass:** confirm ran per `.claude/rules/anonymization.md`. List any personal names found in inputs and the role label each was mapped to. Flag any name that could not be cleanly anonymized (e.g., embedded in a load-bearing quote) for user review.
- **Ambiguities flagged** for client clarification
- Next steps (review → clarify → `/estimate-ai-hours` → `/init-project`)

---

## 📚 Module References

| Module | Covers |
|--------|--------|
| `modules/process-client-docs-reading.md` | STEP 1 reading per format |
| `modules/process-client-docs-extraction.md` | STEP 2 extraction patterns |
| `process-client-docs-reference.md` | STEP 3 file specs + STEP 4 template + quality checks + example |

---

## Extraction Principles (summary)

- **Be thorough** — read every doc completely; don't skip tables/diagrams/images.
- **Be structured** — organize by epics + stories; assign priorities from client emphasis.
- **Be smart** — infer implicit requirements; flag ambiguities rather than guess.

Full checklist + common pitfalls: `process-client-docs-reference.md`.

---

**Version:** 3.2.0
**Created:** 2026-03-27
**Updated:** 2026-04-21 (split: templates + summary moved to process-client-docs-reference.md)
**Command Type:** Requirements Engineering
