# System Overview — File Map

**Version:** 3.3.0
**Last Updated:** 2026-04-21
**Purpose:** Flat, one-page index of every file in the framework — what it is, where it lives, who updates it. For workflow explanations, see [README.md](README.md) or [INTEGRATION-GUIDE.md](INTEGRATION-GUIDE.md).

---

## Directory Tree

```
project-root/
│
├── README.md                                   # Project-level intro + quick start
├── CLAUDE.md                                  # Core coding standards
├── CHANGELOG.md                                # Version history
│
├── .claude/
│   ├── README.md
│   ├── commands/                               # Slash commands (12 total)
│   │   ├── init-project.md
│   │   ├── process-client-docs.md
│   │   ├── generate-docs.md
│   │   ├── execute-work.md
│   │   ├── add-scope.md
│   │   ├── add-backlog-requirement.md
│   │   ├── promote-requirement.md
│   │   ├── add-bug.md
│   │   ├── run-tests.md
│   │   ├── project-status.md
│   │   ├── audit-pm.md                         # framework health audit
│   │   ├── migrate-to-modular.md               # legacy-only
│   │   ├── COMMAND-TEMPLATE.md                 # meta: template for new commands
│   │   ├── how-to-use/                         # 12 quick guides (~80-150 lines each)
│   │   └── modules/                            # 26+ modular command internals
│   ├── hooks/                                  # Validation + audit shell scripts
│   │   ├── README.md
│   │   ├── post-write-validations.sh           # file-size + backlog sync (PostToolUse)
│   │   ├── stop-changelog-check.sh             # CHANGELOG coverage (Stop)
│   │   └── audit-pm.sh                         # /audit-pm mechanical checks
│   ├── rules/                                  # Coding + process rules
│   │   ├── code-quality.md
│   │   ├── testing.md
│   │   ├── git.md
│   │   ├── database.md
│   │   ├── documentation.md
│   │   ├── permissions.md
│   │   └── stack-specific.md
│   ├── settings.example.json
│   └── settings.local.json                     # gitignored
│
└── .project-management/
    ├── README.md                               # PM system guide
    ├── QUICK-START.md                          # 5-minute onboarding
    ├── INTEGRATION-GUIDE.md                    # How CLAUDE.md + PM system work together
    ├── COMMAND-STATUS.md                       # Modular-structure integration status
    ├── SYSTEM-OVERVIEW.md                      # (this file)
    ├── WHATS-NEW.md                            # Version highlights
    │
    ├── client-input/                           # 👤 Raw client documents
    │   └── README.md
    │
    ├── input/                                  # 👤 USER-MANAGED
    │   ├── scope.md
    │   ├── backlog/                            # Modular backlog (v3.1+)
    │   │   ├── README.md
    │   │   ├── phase-1-foundation.md
    │   │   ├── phase-2-core.md
    │   │   ├── phase-3-advanced.md
    │   │   ├── phase-4-polish.md
    │   │   └── future.md
    │   ├── backlog-future.md                   # v2.0/v3.0 requirements
    │   ├── technologies.md
    │   └── constraints.md
    │
    ├── output/                                 # 🤖 CLAUDE-GENERATED
    │   ├── docs/
    │   │   ├── prd.md
    │   │   ├── technical-spec.md
    │   │   ├── architecture.md
    │   │   └── api-spec.md                     # optional
    │   ├── phases/
    │   │   └── phase-N.md                      # per phase
    │   ├── progress/                           # Auto-updated by /execute-work
    │   │   ├── DASHBOARD.md                    # Live view
    │   │   ├── daily-summary.md
    │   │   ├── weekly-report.md
    │   │   ├── current-status.md
    │   │   ├── completed.md
    │   │   └── blockers.md
    │   └── bugs/
    │       ├── bug-roadmap.md
    │       └── bug-archive.md
    │
    ├── templates/                              # Document templates
    │   ├── prd-template.md
    │   ├── technical-spec-template.md
    │   ├── architecture-template.md
    │   ├── phase-template.md
    │   ├── phase-backlog-template.md
    │   ├── backlog-readme-template.md
    │   ├── phase-progress-template.md
    │   ├── progress-template.md
    │   ├── dashboard-template.md
    │   ├── daily-summary-template.md
    │   ├── weekly-report-template.md
    │   ├── current-status-template.md
    │   ├── completed-template.md
    │   ├── blockers-template.md
    │   ├── bug-template.md
    │   ├── backlog-future-template.md
    │   └── technical-plan-template.md
    │
    ├── defaults/
    │   ├── default-stack.md
    │   └── stack-questions.md
    │
    ├── docs/                                   # Long-form reference docs
    │   ├── ARCHITECTURE.md
    │   ├── INPUT-FILES-GUIDE.md
    │   ├── GENERATED-DOCS-GUIDE.md
    │   ├── WORKFLOWS.md
    │   ├── BEST-PRACTICES.md
    │   ├── REUSE-GUIDE.md
    │   └── MIGRATION-GUIDE.md
    │
    ├── guides/                                 # Concise user guides
    │   ├── GETTING-STARTED.md
    │   ├── COMMANDS-REFERENCE.md
    │   ├── WORKFLOWS-BEST-PRACTICES.md         # pointer file
    │   ├── MODULAR-STRUCTURE-GUIDE.md
    │   ├── FAQ.md                              # "how do I...?"
    │   └── TROUBLESHOOTING.md                  # "why doesn't X work?"
    │
    ├── examples/
    │   ├── README.md
    │   └── backlog-monorepo-example.md
    │
    └── rules/                                  # Project-specific rule overrides
        ├── README.md
        ├── project-rules.md
        ├── I18N-RULES.md
        ├── I18N-SETUP.md
        └── TESTING-RULES.md
```

---

## Where to Look for What

| I want to... | Start here |
|---|---|
| Understand the whole system | [README.md](README.md) |
| Get started in 5 minutes | [QUICK-START.md](QUICK-START.md) |
| See how CLAUDE.md + PM system cohabit | [INTEGRATION-GUIDE.md](INTEGRATION-GUIDE.md) |
| Know who updates which file | This file (columns below) |
| Find a command | [guides/COMMANDS-REFERENCE.md](guides/COMMANDS-REFERENCE.md) |
| See current progress | [output/progress/DASHBOARD.md](output/progress/DASHBOARD.md) |
| Fix a specific problem | [guides/TROUBLESHOOTING.md](guides/TROUBLESHOOTING.md) |
| Decide between approaches | [guides/FAQ.md](guides/FAQ.md) |

---

## Who Updates What

| Path | Updated by | Read by |
|------|-----------|---------|
| `input/scope.md` | User | `/init-project`, `/generate-docs`, planning |
| `input/backlog/` | User + `/add-scope` + `/process-client-docs` | `/execute-work`, `/generate-docs` |
| `input/technologies.md` | User | `/init-project`, `/generate-docs`, architecture |
| `input/constraints.md` | User | `/execute-work` (timeline, budget) |
| `output/docs/*` | `/init-project`, `/generate-docs` | Implementation |
| `output/phases/phase-N.md` | `/execute-work` | Current phase work |
| `output/progress/DASHBOARD.md` | `/execute-work` auto-update | Anyone checking status |
| `output/progress/current-status.md` | `/project-status` | Detailed reports |
| `output/progress/completed.md` | `/execute-work` (append-only) | Retrospectives |
| `output/progress/blockers.md` | User (manual) | Everyone |
| `output/bugs/*` | `/add-bug`, `/execute-work bug BUG-XXX` | Bug triage |
| `.claude/commands/*` | Framework maintainer | Claude (during slash command invocation) |
| `.claude/rules/*` | Framework maintainer / user override | Claude (while coding) |

---

## Command Count (v3.2)

- **12 slash commands:** init-project, process-client-docs, generate-docs, execute-work, add-scope, add-backlog-requirement, promote-requirement, add-bug, run-tests, project-status, audit-pm, migrate-to-modular (legacy)
- **12 how-to-use quick guides:** one per slash command
- **26+ modules:** internal logic split by concern (init-project, execute-work, add-scope, run-tests, project-status, process-client-docs, live-progress, backlog-organization)
- **3 hook scripts:** post-write-validations.sh, stop-changelog-check.sh, audit-pm.sh
- **7 rules:** code-quality, testing, git, database, documentation, permissions, stack-specific

---

**Part of:** Claude Project Management System v3.3
