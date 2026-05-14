# Migration Guide: Sprint-Based → Phase-Based System

**Version:** 2.0 → 3.0
**Date:** 2026-03-27

This guide helps you migrate existing projects from the old sprint-based system to the new phase-based system with automation.

---

## What Changed

### Old System (v2.0)
- Sprint-based planning (2-week cycles)
- Manual execution with TodoWrite
- Manual testing
- Manual git commits
- Manual progress updates
- Command: `/plan-sprint N`

### New System (v3.0)
- **Phase-based planning** (1-4 month cycles)
- **Automated execution** with `/execute-work`
- **Automatic testing** (Vitest + Playwright)
- **Automatic git commits** (following git.md rules)
- **Automatic progress tracking**
- **Plan mode** (mandatory before implementation)
- **Default stack system** (3 options)
- **i18n setup** during initialization
- Commands: `/execute-work phase/epic/story`, `/run-tests`

---

## Migration Steps

### Step 1: Backup Existing Project

```bash
# Create backup of current state
cp -r .project-management .project-management.backup
cp CLAUDE.md CLAUDE.md.backup
```

### Step 2: Update System Files

```bash
# Pull latest version
git pull origin main

# Or manually copy new files:
cp -r /path/to/new/system/.project-management/* .project-management/
cp /path/to/new/system/CLAUDE.md CLAUDE.md
cp -r /path/to/new/system/.claude/commands/* .claude/commands/
```

### Step 3: Convert Sprints to Phases

**Manually organize your sprints into phases:**

1. Read your existing sprint files in `.project-management/output/sprints/`
2. Group related sprints into phases:
   - **Phase 1: Foundation** - Sprints 1-2 (setup, auth, infrastructure)
   - **Phase 2: Core Features** - Sprints 3-6 (main features)
   - **Phase 3: Advanced Features** - Sprints 7-10 (secondary features)
   - **Phase 4: Polish** - Sprints 11-12 (optimization, launch)

3. Create phase files using the template:
   ```bash
   cp .project-management/templates/phase-template.md .project-management/output/phases/phase-1.md
   ```

4. Fill in phase details from your sprint data

### Step 4: Configure Tech Stack

If you don't have a detailed `technologies.md`:

```bash
# Option 1: Use default stack
cp .project-management/defaults/default-stack.md .project-management/input/technologies.md

# Option 2: Run init-project to configure
/init-project
```

### Step 5: Configure i18n (Optional)

**If your project needs multi-language support:**

1. Run `/init-project` and select i18n configuration
2. Or manually create `.project-management/rules/I18N-RULES.md`
3. Create translation file structure:
   ```bash
   mkdir -p public/locales/{en,de,sr}
   touch public/locales/en/translation.json
   ```

**If you don't need i18n:**
- Delete `.project-management/rules/I18N-RULES.md` if it exists
- Skip this step

### Step 6: Update Backlog

Update `.project-management/input/backlog.md` to assign epics to phases:

```markdown
## Epic 1: Project Setup & Infrastructure
**Phase:** Phase 1 - Foundation & Setup
**Priority:** P0
**Story Points:** 21

### User Stories
...
```

### Step 7: Test New System

```bash
# Check project status
/project-status

# Test running tests
/run-tests all

# Try executing a single story (in plan mode)
/execute-work story US-001
```

---

## Key Differences in Workflow

### Old Workflow
```
/plan-sprint 1
→ Manual implementation
→ Manual testing (npm test)
→ Manual git commit
→ /update-progress        (historical; command removed in v3.2.0)
```

### New Workflow
```
/execute-work phase 1
→ [PLAN MODE] Creates plan, waits for approval
→ [AUTO] Implements all stories
→ [AUTO] Runs tests after each story
→ [AUTO] Git commit when tests pass
→ [AUTO] Updates progress
→ [AUTO] Continues to next story
```

---

## Command Changes

### Removed Commands
- ❌ `/plan-sprint N` → Use `/execute-work phase N` instead
- ❌ `/update-progress` (removed in v3.2.0) → Progress is auto-tracked by `/execute-work`; live view is `output/progress/DASHBOARD.md`

### New Commands
- ✅ `/execute-work phase/epic/story` - Automated execution with plan mode
- ✅ `/run-tests all/unit/e2e/coverage/story` - Manual test execution

### Updated Commands
- ✅ `/init-project` - Now includes stack & i18n configuration
- ✅ `/project-status` - Shows phase progress instead of sprint progress

### Unchanged Commands
- ✅ `/process-client-docs` - Still works the same
- ✅ `/generate-docs` - Still works the same

---

## File Structure Changes

### Old Structure
```
.project-management/
├── output/
│   ├── sprints/
│   │   ├── sprint-1.md
│   │   ├── sprint-2.md
│   │   └── ...
```

### New Structure
```
.project-management/
├── defaults/              # NEW
│   ├── default-stack.md
│   └── stack-questions.md
├── output/
│   ├── phases/           # NEW (replaces sprints/)
│   │   ├── phase-1.md
│   │   ├── phase-2.md
│   │   ├── phase-3.md
│   │   └── phase-4.md
```

---

## Breaking Changes

### 1. Sprint Files No Longer Used
- Old sprint files in `.project-management/output/sprints/` are not read by new system
- Must migrate to phase structure

### 2. /plan-sprint Command Removed
- Use `/execute-work` instead
- Old command will not work

### 3. Manual Workflow Changed
- `/execute-work` requires plan mode approval
- Cannot skip planning phase
- Tests run automatically before marking stories done

---

## Backwards Compatibility

### What Still Works
- ✅ All `.claude/rules/*.md` files
- ✅ `.project-management/rules/project-rules.md`
- ✅ `.project-management/input/*` files
- ✅ `.project-management/output/docs/*` files
- ✅ TodoWrite tool usage
- ✅ Manual implementation (without `/execute-work`)

### What Doesn't Work
- ❌ `/plan-sprint` command
- ❌ Sprint files won't be read by new commands
- ❌ Old progress tracking format

---

## Rollback Instructions

**If you need to rollback to v2.0:**

```bash
# Restore backup
rm -rf .project-management
mv .project-management.backup .project-management

rm CLAUDE.md
mv CLAUDE.md.backup CLAUDE.md

# Restore old commands
git checkout v2.0 -- .claude/commands/
```

---

## FAQ

### Q: Do I have to use `/execute-work`?
**A:** No, you can still implement manually with TodoWrite and run tests manually with `/run-tests`. But `/execute-work` provides full automation.

### Q: Can I still do 2-week sprints?
**A:** Yes, you can plan phases as 2-week increments. Phases are flexible durations.

### Q: What if I don't want automatic git commits?
**A:** Use manual workflow. Implement with TodoWrite, test with `/run-tests`, and commit manually.

### Q: Do I need to configure i18n?
**A:** Only if your project requires multi-language support. Otherwise, skip it.

### Q: Can I mix old and new systems?
**A:** Not recommended. Choose one system and stick with it for consistency.

---

## Support

**Issues?**
- Check `CLAUDE.md` for updated workflow
- Read command files in `.claude/commands/`
- Review this guide

**Questions?**
- Ask Claude: "Explain the new execute-work command"
- Ask Claude: "How do phases work?"

---

**Version:** 1.0
**Last Updated:** 2026-03-27
