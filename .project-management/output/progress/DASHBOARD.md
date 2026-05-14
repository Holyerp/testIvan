# Live Progress Dashboard

**Status:** Not yet generated.

This file is auto-populated by `/execute-work` the first time you run a phase, epic, or story. It shows live progress, currently active work, recent completions, quality metrics, and phase breakdown.

## How to populate

1. Customize `.project-management/input/` (scope, backlog, technologies, constraints) — see root README §Quick Start.
2. Run `/init-project` to generate PRD, technical spec, and phase structure.
3. Run `/execute-work phase 1` (or `epic EPIC-X`, `story US-XXX`).

After the first execution, this file is rewritten with sections for Currently Working On, Today's Progress, Recently Completed, Quality Metrics, and Phase Breakdown — all auto-updated as you work.

## Manual edits

This file is overwritten by `/execute-work`. Do not hand-edit. To adjust progress, edit the underlying phase backlog files in `.project-management/input/backlog/` or the per-phase progress files in `output/phases/`.

For a full event mapping (which actions trigger which dashboard updates), see `.claude/commands/modules/execute-work-dashboard-events.md`.
