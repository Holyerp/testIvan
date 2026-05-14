# Add Bug — Reference

Companion to `add-bug.md`. Holds the bug-entry template, the completion-report template, and a worked example.

---

## Bug Entry Template (for `bug-roadmap.md`)

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

**Severity sections** in `bug-roadmap.md`:

| Severity | Section |
|----------|---------|
| Critical | 🔴 Critical Bugs |
| High     | 🟠 High Priority Bugs |
| Medium   | 🟡 Medium Priority Bugs |
| Low      | 🟢 Low Priority Bugs |

---

## Completion Report Template (STEP 5)

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

## Example Walkthrough — Interactive Intake

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

## Story-Point Heuristic by Severity

If the user doesn't provide points, suggest based on severity:

| Severity | Suggested points |
|----------|------------------|
| Critical | 8-13 |
| High     | 5-8 |
| Medium   | 3-5 |
| Low      | 1-3 |

Always confirm before locking in.

---

**Version:** 3.3.0
**Created:** 2026-04-21 (extracted from `add-bug.md` to meet documentation.md §2.1 soft target)
**Parent:** `add-bug.md`
