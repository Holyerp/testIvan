# Quick Start (5 Minutes)

**Ultra-simple guide to get started immediately.**

**Version:** 3.1.0

---

## 🆕 Brand New Project

### Step 1: Copy Files
```bash
cd /path/to/your/project
cp -r /path/to/claude_repo/.project-management .
cp -r /path/to/claude_repo/.claude .
cp /path/to/claude_repo/.CLAUDE.MD .
```

### Step 2: Define Your Project
Fill one file: `.project-management/input/scope.md`

Minimum info needed:
- What you're building (project name + description)
- Main features (bullet list)
- Target users (who will use it)

### Step 3: Initialize
```bash
/init-project
```

Choose options:
- Tech stack: **[1] Default** (recommended for web apps)
- i18n: **[2] No** (unless you need multi-language)

**Result:** System creates everything automatically!
- ✅ Modular backlog (organized by phase)
- ✅ Technical spec
- ✅ Phase plans
- ✅ Progress tracking files
- ✅ DASHBOARD.md (live status view)

### Step 4: Start Development
```bash
/execute-work phase 1
```

Choose mode:
- **[1] Continuous** - runs all stories automatically
- **[2] Paused** - waits for approval after each story

Approve plan: **[Yes]**

**Done!** Claude now:
- ✅ Implements stories
- ✅ Runs tests automatically
- ✅ Creates git commits (no AI credits)
- ✅ Updates progress automatically

### Step 5: Check Progress Anytime
```bash
open .project-management/output/progress/DASHBOARD.md
```

**No commands needed!** File auto-updates as you work.

---

## 🔄 Legacy Project (monolithic `backlog.md`)

Only needed for older projects. `/init-project` and `/process-client-docs` already generate the modular structure directly.

**One-shot legacy upgrade:**
```bash
/migrate-to-modular       # legacy-only — skip for new projects
```

What it does: splits `backlog.md` into phase files, creates DASHBOARD.md + other progress files, and backs up the original. Takes 2-5 minutes.

After migration:
```bash
open .project-management/output/progress/DASHBOARD.md
/execute-work phase 2     # or wherever you left off
```

Details: [guides/MODULAR-STRUCTURE-GUIDE.md](guides/MODULAR-STRUCTURE-GUIDE.md)

---

## 📊 Understanding the Files

**Input (what you define):**
```
input/
├── scope.md                    ← Your project definition
├── backlog/                    ← Auto-generated from scope
│   ├── README.md               ← Master overview
│   ├── phase-1-foundation.md   ← Phase 1 stories
│   ├── phase-2-core.md         ← Phase 2 stories
│   └── ...
├── technologies.md             ← Tech stack
└── constraints.md              ← Timeline, budget
```

**Output (what system generates):**
```
output/
├── docs/
│   ├── prd.md                  ← Product requirements
│   └── technical-spec.md       ← Technical specification
├── phases/
│   └── phase-*.md              ← Execution plans
└── progress/
    ├── DASHBOARD.md            ← 👈 OPEN THIS FOR STATUS!
    ├── daily-summary.md        ← Today's work
    ├── completed.md            ← History log
    └── blockers.md             ← Current issues
```

**Key file:** `DASHBOARD.md` - Your at-a-glance project status!

---

## 🎯 Common Commands

| I want to... | Command |
|--------------|---------|
| Start new project | `/init-project` |
| Upgrade legacy project | `/migrate-to-modular` (legacy-only) |
| Add a feature | `/add-scope add story [phase] [epic]` |
| Work on phase | `/execute-work phase 1` |
| Work on single story | `/execute-work story US-001` |
| Check status | Open `DASHBOARD.md` (instant!) |
| Detailed report | `/project-status` |
| Run tests manually | `/run-tests all` |

---

## 💡 Pro Tips

**Fastest workflow:**
1. ✅ Fill `scope.md` (5 min)
2. ✅ Run `/init-project` (auto-generates everything)
3. ✅ Run `/execute-work phase 1` (starts work)
4. ✅ Open `DASHBOARD.md` when you want to check progress

**No manual updates needed!** Everything auto-updates during work.

**Token savings:** 70% faster for AI with modular structure!

---

## 🆘 Need Help?

**Full documentation:**
- [README.md](README.md) - Complete system overview
- [GETTING-STARTED.md](guides/GETTING-STARTED.md) - Detailed setup guide
- [MODULAR-STRUCTURE-GUIDE.md](guides/MODULAR-STRUCTURE-GUIDE.md) - Understanding modular backlog
- [Command Index](../.claude/commands/how-to-use/README.md) - All commands explained

**Stuck?** Check [FAQ](guides/FAQ.md) or [Troubleshooting](guides/TROUBLESHOOTING.md)

---

**That's it! You're ready to build. 🚀**

Start with `/init-project` and let Claude handle the rest!
