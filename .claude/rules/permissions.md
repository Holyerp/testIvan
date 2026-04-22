# Permissions Management Rules

**Version:** 1.0
**Last Updated:** 2026-04-20
**Status:** Active

---

## 🚨 CRITICAL: Settings File Behavior

### The Problem

**Claude Code default behavior:**
When adding a new permission, Claude may overwrite the entire `.claude/settings.json` file, deleting all previous permissions. This forces users to re-approve everything.

**Example of BAD behavior:**
```json
// Before: User has all permissions
{
  "permissions": {
    "allow": ["Bash(*)", "Read(**)", "Write(**)", "Edit(**)", ...]
  }
}

// After: Claude adds ONE new permission and DELETES everything else
{
  "permissions": {
    "allow": ["Bash(npm install)"]  // ❌ EVERYTHING ELSE GONE
  }
}
```

---

## ✅ CORRECT Behavior (MANDATORY)

### Rule 1: NEVER Modify settings.json Automatically

**If you need to add a permission:**

1. ✅ **Ask the user** to add it manually
2. ✅ **Provide exact syntax** for copy-paste
3. ❌ **NEVER write to settings.json** automatically

**Example - Correct Approach:**

```
User: "Run npm install"

Claude: "I need permission to run npm install.

Please add this to .claude/settings.json:

{
  "permissions": {
    "allow": [
      "Bash(*)",           // Keep existing
      "Read(**)",          // Keep existing
      ...                  // Keep all existing
      "Bash(npm install)"  // Add this line
    ]
  }
}

Or, for broader permissions, you can use:
  "Bash(npm *)"  // Allows all npm commands

Would you like to add this permission?"
```

---

## 📋 Recommended Settings Structure

### Option A: Broad Permissions (Recommended for trusted environments)

**File: `.claude/settings.json`**
```json
{
  "permissions": {
    "allow": [
      "Bash(*)",
      "Read(**)",
      "Edit(**)",
      "Write(**)",
      "Glob(**)",
      "Grep(**)",
      "SlashCommand(*)",
      "Skill(*)",
      "TodoWrite",
      "AskUserQuestion",
      "WebFetch(*)",
      "WebSearch(*)"
    ],
    "deny": [
      "Bash(rm -rf /)",
      "Bash(rm -rf *)",
      "Bash(git push --force *)",
      "Bash(git reset --hard *)"
    ],
    "ask": []
  },
  "defaultMode": "acceptEdits",
  "_comment": "Broad permissions for development. Claude should NEVER modify this file automatically."
}
```

**Benefits:**
- ✅ No interruptions during development
- ✅ Claude can work autonomously
- ✅ Dangerous commands still blocked (rm -rf, force push)

**Use when:**
- Working in trusted development environment
- Want fast, uninterrupted workflow
- Trust Claude with broad access

---

### Option B: Granular Permissions (More restrictive)

**File: `.claude/settings.json`**
```json
{
  "permissions": {
    "allow": [
      "Bash(npm *)",
      "Bash(pnpm *)",
      "Bash(git status)",
      "Bash(git add *)",
      "Bash(git commit *)",
      "Bash(git push)",
      "Bash(ls *)",
      "Bash(cat *)",
      "Bash(mkdir *)",
      "Read(**)",
      "Edit(**)",
      "Write(**)",
      "Glob(**)",
      "Grep(**)",
      "SlashCommand(*)",
      "TodoWrite"
    ],
    "deny": [
      "Bash(rm *)",
      "Bash(git push --force *)",
      "Bash(git reset --hard *)"
    ],
    "ask": [
      "Bash(docker *)",
      "Bash(kubectl *)",
      "WebFetch(*)",
      "WebSearch(*)"
    ]
  },
  "defaultMode": "acceptEdits"
}
```

**Benefits:**
- ✅ More control over what Claude can do
- ✅ Review sensitive operations
- ⚠️ May require more approvals

**Use when:**
- Working in production environment
- Need audit trail
- Want explicit control

---

## 🛡️ Safety Guidelines

### Always Deny These Patterns

**Destructive file operations:**
```json
"deny": [
  "Bash(rm -rf /)",
  "Bash(rm -rf *)",
  "Bash(rm -rf .)",
  "Bash(rm -rf ~)"
]
```

**Dangerous git operations:**
```json
"deny": [
  "Bash(git push --force *)",
  "Bash(git push -f *)",
  "Bash(git reset --hard *)",
  "Bash(git clean -fd *)"
]
```

**System-level operations (optional):**
```json
"deny": [
  "Bash(sudo *)",
  "Bash(chmod 777 *)",
  "Bash(chown *)"
]
```

---

## 📝 Pattern Matching Guide

### Wildcard Patterns

**`*` - Matches anything in that position**
```json
"Bash(git *)"        // Allows: git status, git commit, git push, etc.
"Bash(npm install *)" // Allows: npm install, npm install react, etc.
"Read(**)"           // Allows: Read any file in any directory
"Edit(src/**)"       // Allows: Edit any file in src/ directory
```

**Exact match (no wildcards):**
```json
"Bash(git status)"   // Allows ONLY: git status
"Read(.env)"         // Allows ONLY: Read .env file
```

### Pattern Specificity

**Order from most specific to least specific:**
```json
{
  "deny": [
    "Bash(git push --force *)"  // Most specific - blocks force push
  ],
  "allow": [
    "Bash(git push origin main)", // Specific branch
    "Bash(git push)",              // Any push
    "Bash(git *)"                  // All git commands
  ]
}
```

**Priority:** `deny` rules ALWAYS override `allow` rules

---

## 🔧 Common Permission Patterns

### Development Workflow
```json
{
  "allow": [
    "Bash(npm *)",
    "Bash(pnpm *)",
    "Bash(yarn *)",
    "Bash(git status)",
    "Bash(git add *)",
    "Bash(git commit *)",
    "Bash(git push)",
    "Bash(git diff *)",
    "Bash(git log *)",
    "Read(**)",
    "Edit(**)",
    "Write(**)",
    "Glob(**)",
    "Grep(**)"
  ]
}
```

### Testing & CI
```json
{
  "allow": [
    "Bash(npm test)",
    "Bash(npm run test:*)",
    "Bash(jest *)",
    "Bash(vitest *)",
    "Bash(playwright *)"
  ]
}
```

### Docker & Containers
```json
{
  "ask": [
    "Bash(docker *)",
    "Bash(docker-compose *)",
    "Bash(kubectl *)"
  ]
}
```

---

## 🚦 When Claude Needs New Permission

### Correct Response Template

**When blocked by permissions:**

```markdown
I need permission to run: `[COMMAND]`

**Why I need this:**
[Brief explanation of why this command is needed]

**Add this to .claude/settings.json:**

Under `"allow"` array, add:
  "Bash([PATTERN])"

**Recommended pattern:**
- For this specific command: "Bash([exact command])"
- For broader access: "Bash([tool] *)"

**Example:**
{
  "permissions": {
    "allow": [
      // ... existing permissions ...
      "Bash([PATTERN])"  // ← Add this line
    ]
  }
}

After updating, I'll continue with the task.
```

**Example - Real scenario:**

```markdown
I need permission to run: `pnpm install`

**Why I need this:**
To install project dependencies defined in package.json.

**Add this to .claude/settings.json:**

Under `"allow"` array, add:
  "Bash(pnpm *)"

This will allow all pnpm commands (install, add, remove, etc.)

**Or for just install:**
  "Bash(pnpm install)"

**Example:**
{
  "permissions": {
    "allow": [
      "Bash(*)",
      "Read(**)",
      "Edit(**)",
      "Bash(pnpm *)"  // ← Add this line
    ]
  }
}

After updating, I'll continue with the task.
```

---

## ⚠️ Settings File Corruption Recovery

### If settings.json gets overwritten/corrupted:

**STEP 1: Restore from example**
```bash
cp .claude/settings.example.json .claude/settings.json
```

**STEP 2: Or restore from git**
```bash
git restore .claude/settings.json
```

**STEP 3: Or recreate manually**
Copy the "Option A: Broad Permissions" template from above.

---

## 📊 Recommended Setup for This Project

**For this project management system:**

```json
{
  "permissions": {
    "allow": [
      "Bash(*)",
      "Read(**)",
      "Edit(**)",
      "Write(**)",
      "Glob(**)",
      "Grep(**)",
      "SlashCommand(*)",
      "Skill(*)",
      "TodoWrite",
      "AskUserQuestion",
      "WebFetch(*)",
      "WebSearch(*)"
    ],
    "deny": [
      "Bash(rm -rf *)",
      "Bash(git push --force *)",
      "Bash(git reset --hard *)"
    ],
    "ask": []
  },
  "defaultMode": "acceptEdits"
}
```

**Rationale:**
- Broad permissions for development productivity
- Slash commands need `SlashCommand(*)` for all commands
- TodoWrite needed for progress tracking
- Dangerous operations explicitly blocked

---

## 🎯 Best Practices Summary

### DO:
- ✅ Use broad patterns (`Bash(*)`) for trusted environments
- ✅ Always include safety deny rules
- ✅ Copy from `settings.example.json` as starting point
- ✅ Keep `settings.json` in git for team consistency
- ✅ Document any custom patterns in project README

### DON'T:
- ❌ Let Claude modify `settings.json` automatically
- ❌ Grant permissions without deny rules for safety
- ❌ Use overly restrictive permissions (slows workflow)
- ❌ Forget to backup before experimenting with patterns

---

## 🔍 Troubleshooting

### "Permission denied" messages keep appearing

**Solution:** Use broader patterns
```json
// Instead of:
"Bash(npm install)",
"Bash(npm start)",
"Bash(npm test)"

// Use:
"Bash(npm *)"
```

### Claude asks for permission already granted

**Solution:** Check pattern matching
```json
// If you have: "Bash(git commit -m *)"
// But Claude asks for: "Bash(git commit -m 'message')"
// Change to: "Bash(git commit *)" or "Bash(git *)"
```

### Settings file gets corrupted

**Solution:**
1. Restore from `settings.example.json`
2. Or use git: `git restore .claude/settings.json`
3. Add to commit message: Never auto-modify settings.json

---

## 📚 Related Documentation

- `.claude/settings.example.json` - Template for new projects
- `.gitignore` - Settings that should be ignored
- `.CLAUDE.MD` - Main Claude configuration

---

**Version:** 1.0
**Created:** 2026-04-20
**Last Updated:** 2026-04-20
**Status:** ✅ Active
