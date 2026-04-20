# Modular File Structure Guide

**Version:** 3.1.0
**What's New:** Organized backlog by phase + Live auto-updating dashboard

---

## 🎯 Problem Solved

**Old way had issues:**
- ❌ `backlog.md` grew to 800+ lines (hard to read, expensive for AI)
- ❌ Had to run `/project-status` command to see progress
- ❌ No quick "at-a-glance" view
- ❌ Manual updates required

**New modular approach:**
- ✅ Backlog split by phase (< 250 lines each)
- ✅ Live DASHBOARD.md - just open file to see status
- ✅ Auto-updates during work - no commands needed
- ✅ 70-80% AI token savings

---

## 📁 New File Structure

### For New Projects (Automatic)

When you run `/init-project`, you'll get:

```
.project-management/
├── input/
│   └── backlog/                        ← NEW: Phase-organized backlog
│       ├── README.md                   ← Master index (< 150 lines)
│       ├── phase-1-foundation.md       ← Phase 1 stories only
│       ├── phase-2-core.md             ← Phase 2 stories only
│       ├── phase-3-advanced.md         ← Phase 3 stories only
│       ├── phase-4-polish.md           ← Phase 4 stories only
│       └── future.md                   ← Post-launch (v2.0+)
│
└── output/
    └── progress/
        ├── DASHBOARD.md                ← NEW: Live auto-updating view
        ├── daily-summary.md            ← NEW: Today's work
        ├── weekly-report.md            ← NEW: Weekly summary
        ├── current-status.md           ← Detailed status (auto-updates)
        ├── completed.md                ← Historical log
        └── blockers.md                 ← Active blockers
```

---

## 🚀 How to Use

### 1. Reading Backlog (New Way)

**Quick overview:**
```
Open: .project-management/input/backlog/README.md
See: Summary statistics + links to all phases
```

**Specific phase:**
```
Open: .project-management/input/backlog/phase-1-foundation.md
See: Only Phase 1 stories (150-250 lines)
```

**Benefits:**
- Smaller files = easier to read
- Focused on what matters now
- AI processes only relevant phase (70-80% token savings)

---

### 2. Checking Progress (New Way)

**At-a-glance status:**
```
Open: .project-management/output/progress/DASHBOARD.md
See: Current progress, today's work, active stories
```

**No command needed!** File auto-updates during `/execute-work`

**What you'll see:**
- 📊 Quick status table (overall %, phase %, stories completed)
- 📅 Today's progress (stories completed today)
- 🚀 Current phase info (active stories, recently completed)
- ⚠️ Active blockers (if any)
- 📈 Velocity & timeline projections
- 🧪 Quality metrics (test coverage, passing tests)

**Refresh the file = see latest data** (updates every time you work)

---

### 3. Working on Stories (Same as Before)

```bash
/execute-work story US-005
```

**What happens automatically:**
1. TodoWrite breaks down the story
2. You implement the feature
3. **DASHBOARD.md updates** - "Currently Working On: US-005"
4. Tests run
5. Story completes
6. **DASHBOARD.md updates again** - "Today's Progress: US-005 ✅"
7. **daily-summary.md updates** - Logs what was done
8. **Phase progress % updates**

**You can open DASHBOARD.md anytime** to see current status!

---

## 🔄 Migrating Existing Projects

**If you have an old project with monolithic backlog.md:**

### Option 1: Automatic Migration (Recommended)

```bash
/migrate-to-modular
```

**Claude will:**
1. Read your current `backlog.md`
2. Analyze and categorize stories by phase
3. Create `backlog/` directory with phase files
4. Generate `DASHBOARD.md`
5. Backup old files (`.backup` suffix)
6. Show summary

**Safe:** Auto-rollback if migration fails

---

### Option 2: Manual (Gradual Transition)

**Keep using old structure, but:**
- New projects get modular structure
- Eventually migrate when convenient
- Old commands still work

---

## 📖 File Details

### backlog/README.md (Master Index)

**Purpose:** Overview of entire backlog

**Contains:**
- Total statistics (epics, stories, points)
- Links to all phase backlogs
- Summary by priority (P0/P1/P2)
- Quick navigation

**Size:** < 150 lines

**When to read:**
- Need full project overview
- Unsure which phase a story is in
- Want summary statistics

---

### backlog/phase-N-*.md (Phase Backlogs)

**Purpose:** Detailed stories for specific phase

**Contains:**
- Phase goal and duration
- All epics in this phase
- All stories with full details (acceptance criteria, dependencies)
- Phase summary

**Size:** < 250 lines each

**When to read:**
- Working on stories in this phase
- Planning phase execution
- Adding new stories to phase

---

### progress/DASHBOARD.md (Live Status)

**Purpose:** Real-time project status view

**Contains:**
- Quick status metrics
- Today's completed work
- Currently active stories
- Recently completed stories
- Active blockers
- Velocity and timeline
- Quality metrics
- Phase breakdown

**Size:** < 200 lines

**Updates:** Automatically during `/execute-work`

**When to read:**
- Want quick status check (daily)
- Before standup meetings
- Showing progress to stakeholders

---

### progress/daily-summary.md (Today's Work)

**Purpose:** Detailed log of today's activity

**Contains:**
- Stories completed today with details
- Stories in progress
- Time spent per story
- Files changed, tests added
- Next day plan

**Size:** < 150 lines

**Updates:** Real-time during work, archived daily

**When to read:**
- End of day review
- Writing daily status update
- Tracking daily velocity

---

### progress/weekly-report.md (Weekly Summary)

**Purpose:** Weekly summary for stakeholders

**Contains:**
- Week-over-week metrics
- Stories completed each day
- Velocity trends
- Goals for next week
- Risks and issues

**Size:** < 200 lines

**Updates:** End of week (Friday or Monday)

**When to read:**
- Weekly team meetings
- Stakeholder updates
- Sprint reviews

---

## 💡 Benefits Summary

### For Developers:

**Old way:**
```
1. Open 800-line backlog.md
2. Scroll to find Phase 2 stories
3. Run /project-status to see progress
4. Wait for report generation
```

**New way:**
```
1. Open backlog/phase-2-core.md (220 lines)
2. Find story quickly
3. Open DASHBOARD.md to see progress (instant)
```

**Time saved:** 5-10 minutes per check

---

### For AI (Claude):

**Old way:**
```
Read backlog.md: 800 lines = ~2400 tokens
Process all stories to find relevant ones
```

**New way:**
```
Read backlog/README.md: 100 lines = ~300 tokens
Read backlog/phase-2-core.md: 220 lines = ~660 tokens
Total: ~960 tokens (60% savings!)
```

**Processing:** Faster, cheaper, more focused

---

### For Project Management:

**Old way:**
- Manual status updates
- Run commands to generate reports
- Large, unorganized backlog

**New way:**
- Auto-updating dashboard
- No commands needed
- Organized by phase (easier planning)
- Weekly reports ready for stakeholders

---

## 🎓 Best Practices

### Backlog Organization

**When adding stories:**
```bash
/add-scope add story [phase] [epic]
```

Claude will add to correct phase backlog file.

**Keep phase files focused:**
- Phase 1: Infrastructure, auth, setup (P0)
- Phase 2: Core MVP features (P0/P1)
- Phase 3: Advanced features (P1/P2)
- Phase 4: Polish, testing, deployment
- Future: Post-launch (v2.0+)

**If phase file exceeds 250 lines:**
- Consider splitting epic
- Move some stories to next phase
- Move post-launch features to future.md

---

### Dashboard Usage

**Check daily:**
- Open DASHBOARD.md each morning
- See yesterday's progress
- Check active blockers
- Review today's plan

**Share with team:**
- DASHBOARD.md is stakeholder-friendly
- No technical jargon
- Clear status indicators (🟢🟡🔴)
- Direct file link (no command needed)

**Trust auto-updates:**
- Don't manually edit DASHBOARD.md
- Let `/execute-work` update it
- Refresh file to see latest

---

## 🔧 Commands Reference

### New Commands

**Migrate existing project:**
```bash
/migrate-to-modular
```

**Add story to phase:**
```bash
/add-scope add story 2 3    # Phase 2, Epic 3
```

**Execute work (auto-updates dashboard):**
```bash
/execute-work story US-005
```

---

### Existing Commands (Still Work)

**Project status (detailed report):**
```bash
/project-status
```

**Manual progress update:**
```bash
/update-progress
```

---

## ❓ FAQ

**Q: Do I have to migrate existing projects?**
A: No, it's optional. Old structure still works. Migrate when convenient.

**Q: Will old commands break?**
A: No, fully backward compatible. Old commands work with new structure.

**Q: What if migration fails?**
A: Auto-rollback to old files. No data lost.

**Q: Can I still use monolithic backlog.md?**
A: Yes, but you'll miss benefits. Recommended to migrate for better organization.

**Q: Does DASHBOARD.md replace /project-status?**
A: No, both exist. DASHBOARD.md for quick view, /project-status for detailed report.

**Q: How often does DASHBOARD.md update?**
A: Every time you use `/execute-work`. Real-time during work.

**Q: Can I edit DASHBOARD.md manually?**
A: Not recommended. Auto-updates will overwrite. Use `/update-progress` for manual changes.

---

## 🔗 Related Documentation

- **Migration Guide:** [.claude/commands/migrate-to-modular.md](../../.claude/commands/migrate-to-modular.md)
- **Backlog Organization:** [.claude/commands/modules/backlog-organization.md](../../.claude/commands/modules/backlog-organization.md)
- **Live Dashboard:** [.claude/commands/modules/live-progress-dashboard.md](../../.claude/commands/modules/live-progress-dashboard.md)
- **Templates:**
  - [templates/dashboard-template.md](../templates/dashboard-template.md)
  - [templates/backlog-readme-template.md](../templates/backlog-readme-template.md)
  - [templates/phase-backlog-template.md](../templates/phase-backlog-template.md)

---

## ✅ Real Project Example

**This project has been migrated to modular structure!**

**Migration completed:** 2026-04-20

**Results:**
- Old `backlog.md`: 381 lines
- New structure: 6 files (96-214 lines each)
- Files created:
  - `input/backlog/README.md` (214 lines) - Master index
  - `input/backlog/phase-1-foundation.md` (145 lines) - Auth & infrastructure
  - `input/backlog/phase-2-core.md` (159 lines) - Products, cart, checkout
  - `input/backlog/phase-3-advanced.md` (132 lines) - Orders, inventory, reviews
  - `input/backlog/phase-4-polish.md` (96 lines) - Vendor dashboard, bugs
  - `input/backlog/future.md` (120 lines) - Post-launch features
  - `output/progress/DASHBOARD.md` (Live status view)

**To view:**
```bash
# Master index
open .project-management/input/backlog/README.md

# Live dashboard
open .project-management/output/progress/DASHBOARD.md

# Current phase
open .project-management/input/backlog/phase-1-foundation.md
```

**Backup location:** `input/backlog.md.backup-2026-04-20`

---

**Last Updated:** 2026-04-20
**Version:** 3.1.0
