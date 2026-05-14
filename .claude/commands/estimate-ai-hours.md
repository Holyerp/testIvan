---
name: estimate-ai-hours
description: Summarize backlog effort and convert to AI rapid-development hours (24/7 Claude agent)
---

# Estimate AI Hours

**📖 Quick Start:** See [how-to-use/estimate-ai-hours.md](./how-to-use/estimate-ai-hours.md) for quick guide (~100 lines)

**Purpose:** Sum total / done / remaining story points across the backlog and convert that effort into a single number — **AI rapid-development hours**, the time a 24/7 Claude agent needs to finish the remaining scope.

**Two primary moments:**
1. **Right after `/process-client-docs`** — backlog exists, no execution data yet → "this scope is N AI-hours" (initial estimate for client / pricing).
2. **During development** — DASHBOARD.md is populated → "M AI-hours remaining" (projected finish).

The command never fails for missing files. If only `input/backlog/` exists, that is enough.

**All documentation is in English only.**

---

## Usage

```bash
/estimate-ai-hours [--phase=N] [--breakdown=phase|priority|both] [--speed=0.25] [--overhead=1.20] [--wall-clock=1.0] [--hours-per-point=...]
```

**Arguments:**
| Argument | Required | Default | Description |
|----------|----------|---------|-------------|
| `--phase=N` | No | all | Restrict report to one phase only |
| `--breakdown` | No | `both` | Sections to render (`phase`, `priority`, or `both`) |
| `--speed` | No | `0.25` | SPEED_FACTOR — see conversion module |
| `--overhead` | No | `1.20` | OVERHEAD_FACTOR — see conversion module |
| `--wall-clock` | No | `1.0` | WALL_CLOCK_DIVISOR (1.0 = 24/7, 3.0 = ~8h/day) |
| `--hours-per-point` | No | mid-band | Custom SP→hours mapping (e.g. `1=2,5=14,8=30`) |

---

## Module References

| Module | Covers |
|--------|--------|
| `modules/estimate-ai-hours-conversion.md` | SP→team-hours table, SPEED/OVERHEAD factors, calibration |
| `modules/project-status-data-collection.md` | Reused: source-priority logic (DASHBOARD → phase files → backlog) |

---

## YOUR TASK

### STEP 0 — Enter plan mode (MANDATORY)

Per `COMMAND-TEMPLATE.md` STEP 0. Even though this command is read-only and does not write to disk, the framework requires plan-mode confirmation.

1. **Read context:**
   - `.project-management/input/backlog/README.md` — totals
   - `.project-management/input/backlog/phase-*.md` — per-story points
   - `.project-management/output/progress/DASHBOARD.md` — if populated (Mode B)
   - `.project-management/output/phases/phase-*.md` — if present (Mode C)
2. **Parse CLI flags** — apply overrides; default to mid-band, `0.25`, `1.20`, `1.0`.
3. **Detect mode** — A / B / C per STEP 1.
4. **Show plan:** detected mode, phases in scope, override values, expected output sections.
5. **Wait for `Yes / No / Revise`.** Only proceed on `Yes`.

---

### STEP 1 — Detect mode and read sources

**Detection order (first match wins):**

```
if DASHBOARD.md exists AND first line ≠ "**Status:** Not yet generated.":
    mode = "B"  (Live)
elif any output/phases/phase-*.md contains a line "**Status:** Completed":
    mode = "C"  (Phase-file fallback)
else:
    mode = "A"  (Initial estimate — backlog only)
```

**Per mode, read:**

| Mode | Read | Extract |
|------|------|---------|
| A | `input/backlog/README.md` + `input/backlog/phase-*.md` | total points per phase, points + priority per story |
| B | `output/progress/DASHBOARD.md` (Phase Breakdown table, Recently Completed table) **plus** mode-A reads (for total scope and per-story details) | Done points per phase, last-updated timestamp |
| C | `output/phases/phase-*.md` (per-story `**Status:**` lines) **plus** mode-A reads | Done points per phase by aggregating Completed stories |

**Source-priority reasoning** is identical to `modules/project-status-data-collection.md` §"Reading Strategy". Reuse, don't duplicate.

**Story parsing in backlog files (regex):**

```
Pattern A — story header:    ^### (US-\d+|T-\d+|BUG-\d+): (.+)$
Pattern B — priority line:   ^\*\*Priority:\*\* (P[0-3])$
Pattern C — estimate line:   ^\*\*Estimate:\*\* (\d+) points?$
Pattern D — phase status:    ^\*\*Status:\*\* (Todo|In Progress|Completed)$
Pattern E — done in phase:   same as D, scoped to output/phases/phase-N.md per-story blocks
```

If a story is missing `**Estimate:**`, count it as `0` and add to a `Stories without estimate` warning list. Do not silently skip.

---

### STEP 2 — Compute

Per story: `team_hours = MAP[points]` (mid-band table in `estimate-ai-hours-conversion.md`).

Per phase: `done_pts = Σ Completed-story pts (B/C only; A → 0)` · `remaining_pts = total - done` · `phase_team_h = Σ MAP[remaining.pts]` · `phase_ai_h = phase_team_h × SPEED × OVERHEAD`.

Project totals: sum phases (exclude `future.md`).
Wall-clock: `days = (total_remaining_ai_h / 24) × WALL_CLOCK_DIVISOR` · `finish = today + days`.
Flag stories ≥21 pts under `Stories flagged for breakdown`.

---

### STEP 3 — Render the report

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🤖 AI RAPID-DEVELOPMENT ESTIMATE
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Generated: <today>
Mode: <A | B | C> — <description>
Source: <DASHBOARD.md | output/phases/ | input/backlog/ only>
Conversion: speed=<S> · overhead=<O> · hours/pt=<mid-band|custom> · wall-clock=<W>

OVERALL
  Total scope:     <X> points  (~<Th> team-hours, ~<Ah> AI-hours)
  Done:            <X> points  (~<Th> team-hours, ~<Ah> AI-hours)
  Remaining:       <X> points  (~<Th> team-hours, ~<Ah> AI-hours)
  Progress:        <%>

PER PHASE                                                         [if --breakdown=phase|both]
  Phase 1 — <name>     <icon> <done>/<total> pts  (<%>)  — ~<N> AI-hours left
  ...

PER PRIORITY (remaining only)                                     [if --breakdown=priority|both]
  P0  <pts> pts → ~<N> AI-hours
  P1  <pts> pts → ~<N> AI-hours
  P2  <pts> pts → ~<N> AI-hours

WALL-CLOCK PROJECTION
  Remaining: <Ah> AI-hours = ~<D> days continuous (wall-clock=<W>)
  If start now (<today>): finish ~<finish_date>

NOTES
  · Stories flagged for breakdown (≥21 pts): <list or "none">
  · Stories without estimate: <list or "none">
  · Override defaults: /estimate-ai-hours --speed=0.30 --overhead=1.15
  · Tune SPEED_FACTOR after Phase 1 completes — see modules/estimate-ai-hours-conversion.md §Calibration.
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

**Status icons:** ✅ done · 🔄 active (some progress) · ⏳ not started · — N/A. (Same convention as `project-status-reference.md`.)

---

## Success Criteria

1. [ ] Plan approved (STEP 0)
2. [ ] Mode correctly detected and shown in the header
3. [ ] All phases present in `input/backlog/` (excluding `future.md`) appear in the per-phase breakdown
4. [ ] Conversion factors printed in the header match the actual factors used
5. [ ] No file writes — command is read-only
6. [ ] Stories without `**Estimate:**` are surfaced in NOTES, not silently dropped

---

## Error Handling

| Error | Recovery |
|-------|----------|
| `input/backlog/` missing | Tell user to run `/process-client-docs` first; abort |
| `input/backlog/README.md` missing but phase files exist | Read phase files only; warn in NOTES |
| `--phase=N` with no matching file | List available phases; abort |
| Story has `Estimate: 21+ points` | Flag for breakdown, count as 64h placeholder |
| Story missing `**Estimate:**` | Count as 0, list in NOTES |

---

## Key Rules

**Always:** show detected mode in header · print actual conversion factors · treat Mode A as expected (not error) · reuse source-priority logic from `modules/project-status-data-collection.md`.

**Never:** write to any file (read-only) · hide the conversion math · silently drop stories without estimates.

---

**Version:** 1.0.0 · **Created:** 2026-05-06 · **Type:** Reporting & Analytics (estimation)
