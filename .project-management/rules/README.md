# Project Rules - Modular System

This folder contains **project-specific rules** that extend the core `.CLAUDE.MD` guidelines.

---

## 📁 File Structure

```
.project-management/rules/
├── README.md              # This file
├── project-rules.md       # MANDATORY - Core project conventions (153 lines)
├── I18N-RULES.md         # CONDITIONAL - Internationalization rules (412 lines)
├── I18N-SETUP.md         # GUIDE - How to setup i18n
└── TESTING-RULES.md      # CONDITIONAL - Project-specific testing (397 lines)
```

---

## 🎯 Modular Rules Pattern

### MANDATORY Files (always read by Claude)

**`project-rules.md`** - Core project-specific rules
- Project overview and tech stack
- Naming conventions
- Business logic rules
- Data validation rules
- API conventions
- Database rules
- Security requirements
- Deployment checklist

✅ **Always read:** This file is part of the core document hierarchy
📏 **Size:** 153 lines (under 200-line target)

---

### CONDITIONAL Files (read only when enabled)

These files follow the **"enable by configuration"** pattern:

#### 1. `I18N-RULES.md` - Internationalization

**Read this file if:**
- ✅ Your project supports multiple languages
- ✅ You need translation requirements
- ✅ File is configured with actual languages (not placeholders)

**Skip this file if:**
- ❌ Single-language project
- ❌ No internationalization needed
- ❌ File contains only placeholders

**How to enable:**
1. Open `I18N-RULES.md`
2. Replace `{{LANGUAGE_2}}`, `{{CODE_2}}`, etc. with actual languages
3. Configure `{{I18N_LIBRARY}}` and `{{TRANSLATION_PATH}}`
4. Claude will automatically enforce translation requirements

**How to disable:**
- Delete the file, OR
- Leave placeholders unconfigured

📏 **Size:** 412 lines (OK because conditional - only read when needed)

---

#### 2. `TESTING-RULES.md` - Project-Specific Testing

**Read this file if:**
- ✅ Your project has specific critical user flows
- ✅ You need custom test utilities
- ✅ You have performance benchmarks
- ✅ You need security test requirements
- ✅ File is configured with actual test paths (not placeholders)

**Skip this file if:**
- ❌ General testing rules are sufficient (see `.claude/rules/testing.md`)
- ❌ No custom testing requirements
- ❌ File contains only placeholders

**How to enable:**
1. Open `TESTING-RULES.md`
2. Define your critical paths (replace `{{CRITICAL_PATH_1_NAME}}`, etc.)
3. Configure test commands and requirements
4. Claude will automatically enforce these tests

**How to disable:**
- Delete the file, OR
- Leave placeholders unconfigured

📏 **Size:** 397 lines (OK because conditional - only read when needed)

---

## 🔄 How Claude Reads These Files

### Document Priority Hierarchy

```
1. Project Planning (.project-management/)
   ├── input/scope.md, input/backlog.md
   └── output/docs/technical-spec.md

2. Core Standards (.CLAUDE.MD)

3. Specialized Rules (.claude/rules/) - ALWAYS READ
   ├── code-quality.md (SOLID & DRY)
   ├── testing.md (general)
   ├── git.md
   ├── database.md
   └── stack-specific.md

4. Project-Specific Rules (.project-management/rules/)
   ├── project-rules.md - ALWAYS READ
   ├── I18N-RULES.md - READ IF i18n enabled
   └── TESTING-RULES.md - READ IF custom testing needed
```

### Conditional Loading Logic

Claude checks:
1. **Does the file exist?**
2. **Is it configured?** (placeholders replaced with real values)
3. **If YES to both:** Apply all rules from that file
4. **If NO to either:** Skip that file entirely

---

## 📏 Why 200 Lines?

**Goal:** Keep all **mandatory** rules files under 200 lines to:
- ✅ Reduce token consumption
- ✅ Faster context loading
- ✅ Easier to read and maintain
- ✅ Modular and focused

**Strategy:**
- **MANDATORY files:** Core rules, always read → Keep under 200 lines
- **CONDITIONAL files:** Extended rules, only read when needed → Can be larger (300-400 lines OK)

---

## ✅ Current Status

### MANDATORY Files (always read)

| File | Lines | Status |
|------|-------|--------|
| `.CLAUDE.MD` | 179 | ✅ Under 200 |
| `code-quality.md` | 199 | ✅ Under 200 |
| `testing.md` | 87 | ✅ Under 200 |
| `git.md` | 105 | ✅ Under 200 |
| `database.md` | 139 | ✅ Under 200 |
| `stack-specific.md` | 166 | ✅ Under 200 |
| `project-rules.md` | 153 | ✅ Under 200 |
| **Total** | **1,028** | ✅ All under 200 |

### CONDITIONAL Files (read only when enabled)

| File | Lines | Read When |
|------|-------|-----------|
| `I18N-RULES.md` | 412 | i18n enabled |
| `TESTING-RULES.md` | 397 | Custom testing needed |
| **Total** | **809** | Only when needed |

**Maximum possible context:**
- **Without conditional:** 1,028 lines (mandatory only)
- **With i18n only:** 1,440 lines (mandatory + i18n)
- **With testing only:** 1,425 lines (mandatory + testing)
- **With both:** 1,837 lines (mandatory + both conditional)

---

## 🎨 How to Add New Conditional Rules

Follow this pattern:

### 1. Create the new rules file

```bash
# Example: Adding performance rules
touch .project-management/rules/PERFORMANCE-RULES.md
```

### 2. Structure the file

```markdown
# Project-Specific Performance Rules

> **Note:** This file contains project-specific performance requirements.
> Include this file ONLY if your project has performance benchmarks.

## When to Use This File

**Create this file if your project has:**
- ✅ Specific performance benchmarks
- ✅ Load testing requirements
- ✅ Response time SLAs

**Skip this file if:**
- ❌ No performance requirements
- ❌ Standard performance is acceptable

**If you don't need performance rules:** Delete or ignore this file.

---

## Performance Benchmarks

**API Response Times:**

| Endpoint | Max Response Time | Target |
|----------|-------------------|--------|
| {{ENDPOINT_1}} | {{TIME}}ms | {{TARGET}} |

... (rest of the rules)
```

### 3. Reference in `project-rules.md`

Add a short section:

```markdown
## Performance Requirements

> **Note:** If your project has specific performance benchmarks,
> see `.project-management/rules/PERFORMANCE-RULES.md`.
> If not needed, delete or ignore that file.

**Quick reference:**
- ✅ **If PERFORMANCE-RULES.md exists:** Follow benchmarks
- ❌ **If missing:** No specific performance requirements
```

### 4. Update `.CLAUDE.MD` hierarchy

Add to the conditional rules list:

```markdown
4. **Project-Specific Rules** (.project-management/rules/)
   - `project-rules.md` - ALWAYS read
   - `I18N-RULES.md` - CONDITIONAL (i18n)
   - `TESTING-RULES.md` - CONDITIONAL (testing)
   - `PERFORMANCE-RULES.md` - CONDITIONAL (performance) ← NEW
```

### 5. Update quality gates (if needed)

```markdown
**Conditional Requirements (check if files exist):**
- [ ] Translations added (if `I18N-RULES.md` exists)
- [ ] Project-specific tests implemented (if `TESTING-RULES.md` exists)
- [ ] Performance benchmarks met (if `PERFORMANCE-RULES.md` exists) ← NEW
```

---

## 🔍 Quick Reference

**To enable i18n:**
1. Edit `I18N-RULES.md` → Replace placeholders with languages
2. Claude will enforce translation requirements

**To enable custom testing:**
1. Edit `TESTING-RULES.md` → Define critical paths
2. Claude will enforce those tests

**To disable a feature:**
- Delete the file, OR
- Leave placeholders unconfigured

**To add new conditional rules:**
- Follow the pattern above
- Keep conditional files descriptive (300-400 lines OK)
- Keep mandatory files under 200 lines

---

**Last Updated:** 2026-03-26
