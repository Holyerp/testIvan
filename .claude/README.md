# Claude Configuration

This folder contains Claude Code configuration files for the project management system.

---

## 📁 Folder Structure

```
.claude/
├── commands/              # Slash commands for project management
│   ├── init-project.md
│   ├── execute-work.md
│   ├── generate-docs.md
│   ├── project-status.md
│   ├── update-progress.md
│   ├── process-client-docs.md
│   └── modules/          # Modular command components
│
├── rules/                # Core coding standards and rules
│   ├── code-quality.md   # SOLID & DRY principles (MANDATORY)
│   ├── testing.md        # Testing requirements
│   ├── git.md           # Git workflow and commit conventions
│   ├── database.md      # Database migration rules
│   └── stack-specific.md # Framework-specific guidelines
│
├── settings.local.json   # Your local permissions (gitignored)
└── settings.example.json # Template for settings (tracked in git)
```

---

## ⚙️ Settings Files

### `settings.local.json` (Your Personal Settings)

**Location:** `.claude/settings.local.json`
**Status:** Gitignored (not tracked in version control)
**Purpose:** Your personal Claude Code permissions

This file contains your user-specific permissions for Claude Code operations. It's gitignored so each developer can have their own preferences.

**Setup (First Time):**
```bash
# Copy the example file to create your local settings
cp .claude/settings.example.json .claude/settings.local.json

# Edit permissions as needed
# (or keep the recommended defaults)
```

---

### `settings.example.json` (Template)

**Location:** `.claude/settings.example.json`
**Status:** Tracked in git
**Purpose:** Template with recommended permissions

This file serves as a template showing the recommended permissions for this project management system.

**Recommended Permissions:**
```json
{
  "permissions": {
    "allow": [
      "SlashCommand(/generate-docs)",  // Auto-generate documentation
      "Bash(git add:*)",               // Auto-stage files
      "Bash(git commit:*)",            // Auto-commit changes
      "Bash(git push:*)"               // Auto-push to remote
    ]
  }
}
```

**Why these permissions?**
- Enables `/execute-work` automation features
- Allows automatic git commits (following git.md rules)
- Enables automatic progress tracking
- Required for continuous execution mode

---

## 🔐 Security Note

**What's gitignored:**
- ✅ `settings.local.json` - Your personal settings
- ✅ Any `*.local.*` files in this folder

**What's tracked:**
- ✅ `settings.example.json` - Template for new users
- ✅ All command files (`commands/*.md`)
- ✅ All rule files (`rules/*.md`)

**Privacy:** Your personal permission preferences stay on your machine and are never committed to git.

---

## 📖 Usage

### For New Users

1. **Copy the example settings:**
   ```bash
   cp .claude/settings.example.json .claude/settings.local.json
   ```

2. **Review and adjust permissions** (optional):
   - Open `.claude/settings.local.json`
   - Modify the `allow`, `deny`, or `ask` arrays as needed
   - Keep recommended permissions for full automation

3. **Start using slash commands:**
   ```bash
   /init-project
   /execute-work phase 1
   /project-status
   ```

---

### For Teams

**Recommended approach:**
1. Keep `settings.example.json` updated with team-recommended permissions
2. Let each developer customize their own `settings.local.json`
3. Document any required permissions in project README

**Alternative (strict team settings):**
- Remove `settings.local.json` from `.gitignore`
- Track it in git
- Everyone uses the same permissions
- (Not recommended - reduces developer flexibility)

---

## 🚀 Automation Features

With recommended permissions enabled, you get:

✅ **Automatic Documentation Generation**
- `/generate-docs` runs without asking for permission
- Updates technical specs, PRD, architecture docs

✅ **Automatic Git Operations**
- Auto-commit after completing stories
- Follows `.claude/rules/git.md` conventions
- NO AI credits in commit messages

✅ **Automatic Progress Tracking**
- Updates progress files during `/execute-work`
- Tracks completed work, blockers, phase status

✅ **Continuous Execution Mode**
- `/execute-work phase N` can run continuously
- Implements multiple stories without pausing
- Auto-commits and auto-tracks progress

---

## 📚 Related Documentation

- **Core Standards:** [/.CLAUDE.MD](../.CLAUDE.MD)
- **Project Management:** [/.project-management/README.md](../.project-management/README.md)
- **Code Quality Rules:** [/rules/code-quality.md](./rules/code-quality.md)
- **Git Workflow:** [/rules/git.md](./rules/git.md)
- **Testing Rules:** [/rules/testing.md](./rules/testing.md)

---

## ❓ FAQ

**Q: Do I need to create `settings.local.json`?**
A: It's recommended but not required. Without it, Claude will ask for permission each time.

**Q: Can I use different permissions than the example?**
A: Yes! Customize `.claude/settings.local.json` to your preferences.

**Q: What happens if I commit `settings.local.json`?**
A: Nothing breaks, but your personal preferences will be shared with the team. It's gitignored to avoid this.

**Q: Can I disable automation?**
A: Yes! Simply don't create `settings.local.json`, or set fewer permissions in the `allow` array.

---

**Built for Claude Code v3.0**
**Updated:** 2026-04-01
