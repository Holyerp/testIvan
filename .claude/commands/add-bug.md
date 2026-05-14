---
name: add-bug
description: Add new bugs to the roadmap for tracking and execution
---

# Add Bug Command

**📖 Quick Start:** See [how-to-use/add-bug.md](./how-to-use/add-bug.md) for quick guide (~120 lines)

Add bugs to the bug roadmap with automatic ID assignment and severity-based organization.

**All bug reports must be in English only.**

---

## Usage

```bash
/add-bug                    # Interactive mode (Claude asks questions)
/add-bug --from file.md     # Read bug from file (use bug-template.md format)
```

---

## YOUR TASK — MANDATORY WORKFLOW

**🔧 CRITICAL RULES:**
When executing bug fixes via `/execute-work bug BUG-XXX`, follow:
- **`.claude/rules/code-quality.md`** - SOLID & DRY principles for bug fixes
- **`.claude/rules/testing.md`** - Regression tests, ALL API status codes
- **`.claude/rules/git.md`** - Commit messages (NO AI credits), reference BUG-XXX

---

### STEP 0: ENTER PLAN MODE (MANDATORY)

**🎯 MANDATORY: Always enter plan mode before adding bugs**

**Claude must:**

1. **Parse arguments:**
   - Check for `--from` flag
   - If `--from`: validate file exists and is readable

2. **Read current bug roadmap:**
   - `.project-management/output/bugs/bug-roadmap.md`
   - Find max BUG-XXX ID (e.g., if BUG-005 exists, next will be BUG-006)
   - Count bugs by severity

3. **Read bug information:**
   - If `--from file.md`: parse bug details from file
   - If interactive: prepare questions for user

4. **Create plan showing:**
   - Bug ID to assign (e.g., BUG-006)
   - Severity level
   - Where bug will be added in roadmap
   - Story points estimate (if provided or suggested)

5. **Present plan:**
   ```
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
   📋 ADD BUG - PLAN
   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

   BUG ID: BUG-006
   TITLE: {{Bug title}}
   SEVERITY: {{Critical/High/Medium/Low}}
   STORY POINTS: {{1-13}}
   AFFECTED: {{Component/File}}

   WILL ADD TO:
   - bug-roadmap.md ({{Severity}} section)

   ASSIGN TO PHASE NOW? [Yes/No]
   {{If Yes, ask which phase}}

   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
   Proceed? [Yes / No / Revise]
   ```

6. **Wait for approval**
7. **Only proceed to STEP 1 after approval**

---

### STEP 1: GATHER BUG INFORMATION

**🔒 Anonymization note:** Bug reports go into the persistent roadmap and may be shared with clients. If the user or input file references a person by name as the reporter or affected user (e.g., "Marko reported this", "happens on Ana's account"), substitute role labels (`the QA lead reported …`, `a user account`) or source-context (`per support channel`). Full rule: `.claude/rules/anonymization.md`. Sample customer names in clearly-marked example data are fine.

**If `--from file.md` provided:**
- Read and parse file using bug-template.md format
- Extract: title, severity, component, description, reproduction steps, expected/actual behavior
- Validate all required fields present

**If interactive mode:**

Most fields are free-text intake (title, component, description, reproduction steps, expected/actual behavior, notes — AskUserQuestion is the wrong tool for prose). Two fields use AskUserQuestion: **Severity** (Q2) and **Story Points** (Q8).

1. **Bug Title** (required)
   - Short, descriptive title
   - Example: "User login fails with valid credentials"

2. **Severity** (required) — ask via AskUserQuestion (gating, no Skip):

   ```
   question: "Bug severity?"
   header: "severity"
   skippable: false
   options:
     - label: "Critical"
       description: "System unusable, data loss, or security vulnerability. Production-blocking."
     - label: "High"
       description: "Major functionality broken; workaround exists but degraded UX."
     - label: "Medium"
       description: "Minor functionality affected; easy workaround available."
     - label: "Low"
       description: "Cosmetic issue or nice-to-have fix; no functional impact."
   ```

   The chosen severity routes the bug to the matching section in `bug-roadmap.md` (Critical → 🔴, High → 🟠, Medium → 🟡, Low → 🟢) per STEP 3.

3. **Affected Component/File** (required)
   - Which file, module, or component is affected
   - Example: "src/auth/LoginController.php"

4. **Description** (required)
   - Brief description of what's broken
   - Example: "Users cannot log in even with correct credentials"

5. **Reproduction Steps** (required)
   - Numbered list of steps to reproduce
   - Example:
     1. Navigate to /login
     2. Enter valid credentials
     3. Click login button
     4. See error message

6. **Expected Behavior** (required)
   - What should happen
   - Example: "User should be logged in and redirected to dashboard"

7. **Actual Behavior** (required)
   - What actually happens
   - Example: "Login form shows 'Invalid credentials' error"

8. **Story Points** (optional) — ask via AskUserQuestion (`skippable: true` — Skip uses severity-based suggestion):

   ```
   question: "Story points estimate?"
   header: "points"
   skippable: true
   default: "{{severity_suggested_points}}"
   options:
     - label: "1 — Trivial (Recommended for Low severity)"
       description: "Tiny fix, < 30 min: typo, one-line config, obvious null check."
     - label: "3 — Small"
       description: "Few hours: localized fix, well-understood bug with clear repro."
     - label: "5 — Medium"
       description: "Half-day to full-day: needs investigation, touches multiple files."
   ```

   AskUserQuestion's native `Other` lets the user type a Fibonacci value not in the top 3 (`2`, `8`, `13`). On Skip, use the severity-based suggestion below:

   - Critical: 8–13 points
   - High: 5–8 points
   - Medium: 3–5 points
   - Low: 1–3 points

   When a free-text `Other` answer arrives, validate it is a Fibonacci value (`1, 2, 3, 5, 8, 13`). If not, emit a warning to the STEP 5 summary ("Non-Fibonacci value '<x>' rounded to nearest: <y>") and round up to the next valid value.

9. **Additional Notes** (optional)
   - Screenshots, error logs, environment details

---

### STEP 2: ASSIGN BUG ID

1. **Read bug-roadmap.md**
2. **Find all existing BUG-XXX IDs**
3. **Determine max ID** (e.g., BUG-005)
4. **Assign next sequential ID** (e.g., BUG-006)
5. **Use zero-padded 3-digit format** (BUG-001, BUG-002, ..., BUG-999)

---

### STEP 3: ADD TO ROADMAP

1. **Open `.project-management/output/bugs/bug-roadmap.md`**

2. **Insert bug under correct severity section:**
   - Critical → 🔴 Critical Bugs
   - High → 🟠 High Priority Bugs
   - Medium → 🟡 Medium Priority Bugs
   - Low → 🟢 Low Priority Bugs

3. **Bug format:** see the entry template in `add-bug-reference.md`.

4. **Update summary section** (bug counts)

---

### STEP 4: ASK ABOUT PHASE ASSIGNMENT

**Claude asks:**
```
Do you want to assign this bug to a phase now?

[1] Yes - Assign to phase (prioritize for fixing)
[2] No - Keep in backlog (triage later)
```

**If user selects [1] Yes:**
1. Show available phases
2. Ask which phase
3. Add bug to selected `phase-N.md` file under "Bugs" section
4. Update bug-roadmap.md: "Assigned to Phase: Phase N"
5. Update phase story points total

**If user selects [2] No:**
- Keep in bug-roadmap.md only
- "Assigned to Phase: Backlog"

---

### STEP 5: COMPLETION REPORT

Print the completion-report template from `add-bug-reference.md` with the new bug's ID, title, severity, points, status, assigned-phase context, and updated counts.

---

## Success Criteria

Bug addition is COMPLETE when:
1. [ ] Plan approved by user (STEP 0)
2. [ ] All required information gathered (STEP 1)
3. [ ] Unique BUG-XXX ID assigned (STEP 2)
4. [ ] Bug added to bug-roadmap.md (STEP 3)
5. [ ] Bug counts updated in roadmap summary (STEP 3)
6. [ ] Phase assignment decided (STEP 4)
7. [ ] If assigned, bug added to phase file (STEP 4)
8. [ ] Completion report displayed (STEP 5)

---

## Example Walkthrough

See the interactive-intake worked example in `add-bug-reference.md`.

---

## Key Rules

1. **Always enter plan mode first** (STEP 0)
2. **Bug IDs are sequential** (BUG-001, BUG-002, ...)
3. **Bug IDs are immutable** (never renumber)
4. **All bug reports in English**
5. **Status starts as "New"**
6. **Severity determines roadmap section**
7. **Story points use Fibonacci scale**
8. **Phase assignment is optional at creation**

---

**Version:** 3.0.0
**Created:** 2026-04-02
**Command Type:** Bug Tracking
