# Hooks

Shell scripts invoked by Claude Code lifecycle events. Wired up in `.claude/settings.example.json`.

**Requires:** `jq`, `git`, `bash`.

---

## Scripts

### `post-write-validations.sh`

**Event:** PostToolUse (Write | Edit)
**Purpose:** Enforce documentation.md §2.1 file-size limits and remind about backlog sync.

Emits warnings (never blocks) when:

| Condition | Severity | Rule |
|-----------|----------|------|
| `.project-management/input/backlog/*.md` > 200 lines | ⚠️ warning | documentation.md §2.1 |
| `.claude/commands/modules/*.md` > 600 lines | 🔴 hard max | documentation.md §2.1 |
| `.claude/commands/modules/*.md` > 300 lines | 🟡 ideal | documentation.md §2.1 |
| `.claude/commands/*.md` (top level) > 300 lines | 🟡 soft target | documentation.md §2.1 |
| Edit touches `.project-management/input/backlog/phase-*.md` | ℹ️ reminder | Verify README.md totals still match |

### `stop-changelog-check.sh`

**Event:** Stop
**Purpose:** Remind about CHANGELOG at session end.

Warns when the branch has commits ahead of its upstream (or `main` / `master`) but none of them modified `CHANGELOG.md`.

---

## Activating the Hooks

Hooks live in `.claude/settings.example.json` (tracked in git so framework copies inherit them). Claude Code loads `.claude/settings.json`, which is **gitignored** — so to activate hooks for a given checkout:

```bash
cp .claude/settings.example.json .claude/settings.json
```

Then restart your Claude Code session (or open `/hooks` once) so the config watcher picks up the new settings file.

If `.claude/settings.json` already exists, merge the `hooks` block from `settings.example.json` into it — don't overwrite the whole file (see `.claude/rules/permissions.md`).

---

## Customizing or Disabling

- **Disable one hook:** edit `settings.json`, remove the corresponding entry from `hooks.PostToolUse` or `hooks.Stop`, or set its command to `true` (no-op).
- **Change thresholds:** edit the relevant script directly (`post-write-validations.sh`). Thresholds are plain numbers, easy to tweak.
- **Disable all hooks globally:** add `"disableAllHooks": true` at the top level of `settings.json`.

---

## Design Principles

1. **Never block.** All hooks emit warnings via `systemMessage`; none return a blocking decision. File-size discipline is a gentle nudge, not a gate.
2. **Silent on success.** A hook that didn't fire leaves zero output. Only actionable messages appear.
3. **Self-contained.** Each script handles its own JSON parsing (via `jq`) and failure modes (missing files, no git upstream, etc.). If a dependency is missing, the hook exits silently rather than erroring.
