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

**If `--from file.md` provided:**
- Read and parse file using bug-template.md format
- Extract: title, severity, component, description, reproduction steps, expected/actual behavior
- Validate all required fields present

**If interactive mode:**

Ask user for:

1. **Bug Title** (required)
   - Short, descriptive title
   - Example: "User login fails with valid credentials"

2. **Severity** (required)
   - Critical: System unusable, data loss, security vulnerability
   - High: Major functionality broken, workaround exists
   - Medium: Minor functionality affected, easy workaround
   - Low: Cosmetic issues, nice-to-have fixes

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

8. **Story Points** (optional)
   - Fibonacci scale: 1, 2, 3, 5, 8, 13
   - If not provided, Claude suggests based on severity and description:
     - Critical: 8-13 points
     - High: 5-8 points
     - Medium: 3-5 points
     - Low: 1-3 points

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

3. **Use standard format:**
   ```markdown
   ### BUG-XXX: [Bug Title]

   **Status:** New
   **Severity:** [Critical/High/Medium/Low]
   **Reported:** [YYYY-MM-DD]
   **Story Points:** [1-13]
   **Affected Component:** [Component/File]
   **Assigned to Phase:** Backlog

   **Description:**
   [Bug description]

   **Reproduction Steps:**
   1. Step 1
   2. Step 2

   **Expected Behavior:**
   [Expected behavior]

   **Actual Behavior:**
   [Actual behavior]

   **Additional Notes:**
   [Notes if any]
   ```

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

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ BUG ADDED TO ROADMAP
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

BUG ID: BUG-006
TITLE: User login fails with valid credentials
SEVERITY: High
STORY POINTS: 5
STATUS: New

ADDED TO:
- bug-roadmap.md (High Priority section)
{{- phase-2.md (if assigned)}}

CURRENT BUG COUNTS:
- Critical: {{X}}
- High: {{X}}
- Medium: {{X}}
- Low: {{X}}
- Total Open: {{X}}

NEXT STEPS:
{{If assigned to phase:}}
1. Bug ready for execution: /execute-work bug BUG-006
2. Or execute full phase: /execute-work phase {{N}}

{{If in backlog:}}
1. Triage and assign to phase when ready
2. Check status: /project-status
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

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

```
User: /add-bug

Claude: [STEP 0: Plan Mode]
        Reading bug-roadmap.md...
        Current max ID: BUG-005
        Next ID: BUG-006

        Ready to gather bug information.
        Proceed? [Yes/No/Revise]

User: Yes

Claude: [STEP 1: Gather Information]

        Bug Title?

User: Login fails with valid credentials

Claude: Severity? [Critical/High/Medium/Low]

User: High

Claude: Affected Component/File?

User: src/auth/LoginController.php

[... continues gathering all information]

Claude: [STEP 3: Adding to roadmap]
        ✅ Added BUG-006 to High Priority section
        ✅ Updated bug counts

        [STEP 4: Phase Assignment]
        Assign to phase now?
        [1] Yes  [2] No

User: 1

Claude: Available phases:
        [1] Phase 1: Foundation
        [2] Phase 2: Core Features

        Which phase?

User: 2

Claude: ✅ Assigned to Phase 2

        [Shows completion report]
```

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
