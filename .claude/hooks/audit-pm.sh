#!/usr/bin/env bash
# audit-pm.sh — mechanical audit checks for the Claude Project Management framework.
#
# Runs from the project root. Emits a sectioned markdown report on stdout suitable
# for ingestion by the /audit-pm slash command (which layers judgement-based checks
# on top and prioritizes findings).
#
# Requires: bash, git (optional), standard POSIX tools.
# Invoked by: .claude/commands/audit-pm.md (the /audit-pm command).
#
# Usage:
#   bash .claude/hooks/audit-pm.sh [--root <path>]
#
# Exit code is always 0 — the point is to report, not gate.

set -u  # no -e: we don't want to stop on first missing file
ROOT="."

while [ $# -gt 0 ]; do
  case "$1" in
    --root) ROOT="$2"; shift 2 ;;
    *) shift ;;
  esac
done

cd "$ROOT" 2>/dev/null || { echo "ERROR: can't cd to $ROOT"; exit 0; }

# ---------- helpers ----------

section() { printf '\n## %s\n\n' "$1"; }
subsection() { printf '\n### %s\n\n' "$1"; }
finding() { printf -- '- %s: %s\n' "$1" "$2"; }  # severity, message
ok() { printf -- '- ✅ %s\n' "$1"; }

count_lines() { wc -l < "$1" 2>/dev/null | tr -d ' '; }

has() { [ -e "$1" ]; }
hasfile() { [ -f "$1" ]; }
hasdir() { [ -d "$1" ]; }

# Detect backlog structure type: "modular" | "monolithic" | "none"
detect_structure() {
  if hasdir ".project-management/input/backlog" && hasfile ".project-management/input/backlog/README.md"; then
    echo "modular"
  elif hasfile ".project-management/input/backlog.md"; then
    echo "monolithic"
  else
    echo "none"
  fi
}

# ---------- report header ----------

printf '# PM Framework Audit\n\n'
printf '**Generated:** %s\n' "$(date -u +%Y-%m-%dT%H:%M:%SZ)"
printf '**Root:** %s\n' "$(pwd)"
STRUCTURE=$(detect_structure)
printf '**Backlog structure:** %s\n' "$STRUCTURE"

# ---------- A. Structure health ----------

section "A. Structure health"

# Required top-level files
for f in .CLAUDE.MD README.md CHANGELOG.md; do
  if hasfile "$f"; then ok "$f exists"; else finding "🔴 CRITICAL" "Required file missing: $f"; fi
done

# Required rules
for r in code-quality.md testing.md git.md documentation.md permissions.md; do
  if hasfile ".claude/rules/$r"; then ok ".claude/rules/$r exists"; else finding "🟠 HIGH" "Missing rule: .claude/rules/$r"; fi
done

# PM system core
for f in .project-management/README.md .project-management/input/scope.md; do
  if hasfile "$f"; then ok "$f exists"; else finding "🟠 HIGH" "Missing core file: $f"; fi
done

# Modular structure integrity
if [ "$STRUCTURE" = "modular" ]; then
  for p in phase-1-foundation phase-2-core phase-3-advanced phase-4-polish future; do
    if hasfile ".project-management/input/backlog/$p.md"; then
      ok "backlog/$p.md exists"
    else
      finding "🟡 MEDIUM" "Modular backlog incomplete: missing $p.md"
    fi
  done
fi

# ---------- B. File size violations ----------

section "B. File-size limits (documentation.md §2.1)"

# Backlog files — 200 line cap
if hasdir ".project-management/input/backlog"; then
  for f in .project-management/input/backlog/*.md; do
    [ -f "$f" ] || continue
    lines=$(count_lines "$f")
    if [ "${lines:-0}" -gt 200 ]; then
      finding "⚠️ warn" "$f is $lines lines (cap: 200)"
    fi
  done
fi
if [ "$STRUCTURE" = "monolithic" ]; then
  lines=$(count_lines ".project-management/input/backlog.md")
  finding "ℹ️ info" "Monolithic backlog.md is $lines lines (consider /migrate-to-modular)"
fi

# Modules
if hasdir ".claude/commands/modules"; then
  for f in .claude/commands/modules/*.md; do
    [ -f "$f" ] || continue
    lines=$(count_lines "$f")
    if [ "${lines:-0}" -gt 600 ]; then
      finding "🔴 hard-max" "$f is $lines lines (HARD MAX: 600)"
    elif [ "${lines:-0}" -gt 300 ]; then
      finding "🟡 ideal" "$f is $lines lines (ideal: ≤300)"
    fi
  done
fi

# Top-level commands (not modules/, not how-to-use/)
if hasdir ".claude/commands"; then
  for f in .claude/commands/*.md; do
    [ -f "$f" ] || continue
    lines=$(count_lines "$f")
    if [ "${lines:-0}" -gt 300 ]; then
      finding "🟡 soft" "$f is $lines lines (soft target: ≤300)"
    fi
  done
fi

# ---------- C. Version consistency ----------

section "C. Version consistency"

# Collect all **Version:** X.Y.Z strings from top-level + .project-management docs
version_report=$(
  {
    grep -rhE '^\*\*Version:\*\*[[:space:]]+[0-9]+\.[0-9]+\.[0-9]+' \
      README.md CHANGELOG.md .CLAUDE.MD \
      .project-management/*.md \
      .project-management/docs/*.md \
      .project-management/guides/*.md 2>/dev/null
  } | sed -E 's/^\*\*Version:\*\*[[:space:]]+//; s/[[:space:]]+\(.*$//' | sort -u
)
distinct_versions=$(printf '%s\n' "$version_report" | sort -u | grep -c '^[0-9]')
if [ "$distinct_versions" -gt 1 ]; then
  finding "🟠 HIGH" "Multiple distinct **Version:** strings in docs:"
  printf '%s\n' "$version_report" | sed 's/^/  - /'
else
  ok "All **Version:** strings agree: $version_report"
fi

# CHANGELOG coverage for current version
if hasfile CHANGELOG.md && hasfile README.md; then
  cur_version=$(grep -hE '^\*\*Version:\*\*' README.md 2>/dev/null | head -1 | sed -E 's/^\*\*Version:\*\*[[:space:]]+([0-9]+\.[0-9]+\.[0-9]+).*/\1/')
  if [ -n "$cur_version" ]; then
    if grep -qE "^## \[?$cur_version\]?" CHANGELOG.md; then
      ok "CHANGELOG.md has entry for v$cur_version"
    else
      finding "🟠 HIGH" "CHANGELOG.md missing entry for v$cur_version (claimed in README.md)"
    fi
  fi
fi

# ---------- D. Broken internal references ----------

section "D. Broken internal references"

# Find [text](relative/path.md) links whose target is missing.
# Only scan .project-management and .claude (framework docs).
broken_count=0
check_links() {
  local from_file="$1"
  local from_dir; from_dir="$(dirname "$from_file")"
  # Extract markdown links: [text](target) where target doesn't start with http:// or #
  grep -oE '\[[^]]+\]\([^)]+\)' "$from_file" 2>/dev/null \
    | sed -E 's/.*\(([^)]+)\)/\1/' \
    | grep -vE '^(https?://|#|mailto:)' \
    | while read -r target; do
        # Strip anchor
        target="${target%%#*}"
        [ -z "$target" ] && continue
        # Resolve relative to from_dir
        resolved=$(cd "$from_dir" 2>/dev/null && cd "$(dirname "$target")" 2>/dev/null && pwd)/$(basename "$target")
        if [ ! -e "$resolved" ]; then
          # Try path as-is (relative to from_dir, with no resolve trick)
          if [ ! -e "$from_dir/$target" ]; then
            printf '  - %s → %s\n' "$from_file" "$target"
          fi
        fi
      done
}

tmp_broken=$(mktemp)
# Skip templates/ (paths use {{placeholders}}, instantiated at copy time, not valid as-written)
for f in $(find .project-management .claude -name "*.md" -type f 2>/dev/null \
  | grep -vE '/(templates|client-input|examples)/'); do
  check_links "$f" 2>/dev/null \
    | grep -vE '\{\{.*\}\}|\$\{.*\}|\$[A-Z_]+' \
    >> "$tmp_broken"
done

if [ -s "$tmp_broken" ]; then
  broken_count=$(wc -l < "$tmp_broken" | tr -d ' ')
  finding "🔴 CRITICAL" "$broken_count broken internal link(s):"
  head -30 "$tmp_broken"
  if [ "$broken_count" -gt 30 ]; then
    printf '  - ... and %s more (showing first 30)\n' "$((broken_count - 30))"
  fi
else
  ok "No broken internal markdown links"
fi
rm -f "$tmp_broken"

# ---------- E. Backlog structure mismatches ----------

section "E. Backlog structure references"

if [ "$STRUCTURE" = "modular" ]; then
  # Any doc still referencing `input/backlog.md` singular is likely stale
  # Exception: MIGRATION-GUIDE, MODULAR-STRUCTURE-GUIDE, WHATS-NEW legitimately discuss the legacy form
  stale=$(grep -rlE 'input/backlog\.md' \
    .project-management/docs .project-management/guides .project-management/README.md \
    .project-management/INTEGRATION-GUIDE.md .project-management/SYSTEM-OVERVIEW.md \
    .project-management/QUICK-START.md 2>/dev/null \
    | grep -vE '(MIGRATION-GUIDE|MODULAR-STRUCTURE-GUIDE|WHATS-NEW)' || true)
  if [ -n "$stale" ]; then
    finding "🟠 HIGH" "Docs reference monolithic \`input/backlog.md\` despite modular structure:"
    printf '%s\n' "$stale" | sed 's/^/  - /'
  else
    ok "No stale monolithic references in core docs"
  fi
fi

# ---------- F. Legacy / deprecated refs ----------

section "F. Legacy & deprecated references"

# Empty output/sprints/ dir (v2.0 artifact)
if hasdir ".project-management/output/sprints"; then
  finding "🟡 MEDIUM" "Legacy v2.0 directory still exists: .project-management/output/sprints/"
fi

# Removed commands
for cmd in /plan-sprint /update-progress; do
  removed_hits=$(grep -rlE "${cmd}([[:space:]]|$|\`|\")" \
    .project-management .claude --include='*.md' 2>/dev/null \
    | xargs -I{} grep -lE "${cmd}([[:space:]]|$|\`|\")" {} 2>/dev/null \
    | grep -vE '(CHANGELOG|MIGRATION-GUIDE|COMMAND-STATUS)' || true)
  if [ -n "$removed_hits" ]; then
    hit_count=$(printf '%s\n' "$removed_hits" | wc -l | tr -d ' ')
    finding "🟡 MEDIUM" "Removed command '$cmd' still referenced in $hit_count file(s) outside history docs"
    printf '%s\n' "$removed_hits" | head -5 | sed 's/^/  - /'
  fi
done

# Phantom files
for phantom in MIGRATION-COMPLETE.md test-migration; do
  hits=$(grep -rl "$phantom" .project-management .claude --include='*.md' 2>/dev/null \
    | grep -vE '(CHANGELOG|README\.md$)' || true)
  if [ -n "$hits" ]; then
    finding "🟡 MEDIUM" "Reference to non-existent \`$phantom\` in:"
    printf '%s\n' "$hits" | head -3 | sed 's/^/  - /'
  fi
done

# ---------- G. How-to-use coverage ----------

section "G. How-to-use guide coverage"

if hasdir ".claude/commands/how-to-use"; then
  # Commands without guides
  for cmd_file in .claude/commands/*.md; do
    [ -f "$cmd_file" ] || continue
    base=$(basename "$cmd_file" .md)
    case "$base" in
      COMMAND-TEMPLATE) continue ;;
    esac
    if [ ! -f ".claude/commands/how-to-use/$base.md" ]; then
      finding "🟡 MEDIUM" "Command /$base has no how-to-use guide"
    fi
  done
  # Orphan guides
  for guide in .claude/commands/how-to-use/*.md; do
    [ -f "$guide" ] || continue
    base=$(basename "$guide" .md)
    [ "$base" = "README" ] && continue
    if [ ! -f ".claude/commands/$base.md" ]; then
      finding "🟡 MEDIUM" "how-to-use/$base.md has no matching command (/$base)"
    fi
  done
fi

# ---------- done ----------

section "Summary"
printf 'Audit complete. Review the sections above; critical findings (🔴) block the next release, HIGH (🟠) should be scheduled, MEDIUM (🟡) are housekeeping.\n'

exit 0
