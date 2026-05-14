# Quick Reference & Document Index

**Version:** 3.3.0
**Last Updated:** 2026-04-27

A short jumping-off page that points to the canonical docs and surfaces the most-used rules of thumb in one place. Not a content duplicate of `docs/WORKFLOWS.md` or `docs/BEST-PRACTICES.md` — those hold the full material; this file just helps you reach them quickly and keeps a few one-glance tables (Quick Rules, Phase Structure TL;DR, Rules Reference) handy.

Filename is preserved (`WORKFLOWS-BEST-PRACTICES.md`) because many other docs link to it.

---

## Start Here

- **5-minute onboarding:** [../QUICK-START.md](../QUICK-START.md)
- **Detailed onboarding:** [GETTING-STARTED.md](GETTING-STARTED.md)
- **Command reference:** [COMMANDS-REFERENCE.md](COMMANDS-REFERENCE.md)
- **Troubleshooting:** [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
- **FAQ:** [FAQ.md](FAQ.md)

---

## Workflow Examples

All step-by-step workflows are documented in `../docs/WORKFLOWS.md`:

- New project from scratch
- Starting from client documents
- Handling a blocker
- Managing bugs
- Promoting future requirements
- Updating scope mid-project

→ [docs/WORKFLOWS.md](../docs/WORKFLOWS.md)

---

## Best Practices

All best practices and quality metrics are in `../docs/BEST-PRACTICES.md`:

- Input files (scope, backlog, technologies, constraints)
- Plan mode discipline
- Testing coverage (80% minimum; API status code matrix)
- i18n configuration
- Git conventions (conventional commits, no AI credits)
- Progress tracking via DASHBOARD.md

→ [docs/BEST-PRACTICES.md](../docs/BEST-PRACTICES.md)

---

## Quick Rules of Thumb

| Situation | Action |
|-----------|--------|
| Implementing anything non-trivial | `/execute-work` (plan mode is mandatory) |
| Manual workflow | `TodoWrite` + `/run-tests` + DASHBOARD.md |
| Checking status | Open `output/progress/DASHBOARD.md` (live view) |
| Detailed status report | `/project-status` |
| Blocker discovered | Edit `output/progress/blockers.md` directly |
| Scope changes for current phases | `/add-scope add|edit` |
| Scope ideas for future versions | `/add-backlog-requirement` |

---

## Phase Structure (TL;DR)

```
Project
  └── Phase (1-4 months)
      └── Epic
          └── User Story
              └── Task
```

- **Phase 1 — Foundation:** Setup, infra, auth, DB, CI/CD (1-2 months)
- **Phase 2 — Core:** MVP features (2-3 months)
- **Phase 3 — Advanced:** Secondary features, integrations (2 months)
- **Phase 4 — Polish:** Optimization, hardening, launch (1 month)

→ Details: [docs/WORKFLOWS.md](../docs/WORKFLOWS.md)

---

## Rules Reference

These live in `.claude/rules/`:

- [code-quality.md](../../.claude/rules/code-quality.md) — SOLID, DRY (mandatory)
- [testing.md](../../.claude/rules/testing.md) — Test matrix + 80% coverage
- [git.md](../../.claude/rules/git.md) — Conventional commits, NO AI credits
- [documentation.md](../../.claude/rules/documentation.md) — English only, file size limits
- [database.md](../../.claude/rules/database.md) — Migration workflow
- [stack-specific.md](../../.claude/rules/stack-specific.md) — Stack-specific conventions
- [permissions.md](../../.claude/rules/permissions.md) — Settings management

---

**Part of:** Claude Project Management System v3.3
