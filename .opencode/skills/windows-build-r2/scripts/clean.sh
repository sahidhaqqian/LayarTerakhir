#!/usr/bin/env bash
# Safely remove temporary snapshot data. Logs, result ZIP and status are kept
# unless --purge is given. The live project tree is never touched.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/common.sh"

usage() {
  cat <<'EOF'
Usage: clean.sh [--run <id>] [--purge] [--force]

  (default)  Remove legacy per-run snapshot/ copies (keeps logs/result/status).
  --run <id> Restrict to one run.
  --purge    Remove the whole run directory (logs + result too). Finished runs only.
  --force    Also stop a running build: kills the tmux session and clears the
             active-run marker. Requires --run. Use only when the run is stuck.

The live project, its Library and its Builds/ output are never modified.
The persistent workspace ($BASE/workspace) and last-good output
($BASE/last-good) are build caches and are never touched by clean.sh —
there is intentionally no flag to delete them.
EOF
}

WANT=""; PURGE=0; FORCE=0
while [ $# -gt 0 ]; do
  case "$1" in
    --run) WANT="${2:?--run needs an id}"; shift 2 ;;
    --purge) PURGE=1; shift ;;
    --force) FORCE=1; shift ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown flag: $1" >&2; usage >&2; exit 1 ;;
  esac
done
if [ "$FORCE" -eq 1 ] && [ -z "$WANT" ]; then
  echo "windows-build-r2: --force requires --run <id>" >&2; exit 1
fi

targets=()
if [ -n "$WANT" ]; then targets=("$WANT")
else
  while IFS= read -r r; do [ -n "$r" ] && targets+=("$r"); done < <(ls -1 "$BASE/runs" 2>/dev/null | sort || true)
fi
[ "${#targets[@]}" -gt 0 ] || { echo "windows-build-r2: no runs to clean."; exit 0; }

code=0
for id in "${targets[@]}"; do
  # Safety: resolve strictly under BASE/runs; refuse anything else.
  rd="$BASE/runs/$id"
  case "$(realpath -m "$rd")" in
    "$BASE"/runs/*) ;;
    *) echo "windows-build-r2: refusing unsafe path: $id" >&2; code=1; continue ;;
  esac
  [ -d "$rd" ] || { echo "windows-build-r2: unknown run: $id" >&2; code=1; continue; }
  st="$(jget "$rd" status)"
  active=0
  [ -f "$(active_file)" ] && [ "$(cat "$(active_file)")" = "$id" ] && active=1
  if { [ "$st" = "Running" ] || { [ "$active" -eq 1 ] && tmux_alive; }; } && [ "$FORCE" -eq 0 ]; then
    echo "windows-build-r2: run $id is active; leaving it alone (use --run $id --force to stop it)."
    continue
  fi
  if [ "$FORCE" -eq 1 ]; then
    tmux kill-session -t "$SESSION" 2>/dev/null || true
    [ "$active" -eq 1 ] && rm -f "$(active_file)"
    [ "$st" = "Running" ] && { jset "$rd" status '"Failed"'; jset_str "$rd" steps.clean.error "stopped by clean --force"; jset "$rd" finishedUtc "\"$(utc_now)\""; }
    echo "windows-build-r2: run $id stopped."
  fi
  if [ "$PURGE" -eq 1 ]; then
    if [ "$(jget "$rd" status)" = "Running" ]; then
      echo "windows-build-r2: run $id still Running; not purging." >&2; code=1; continue
    fi
    rm -rf "$rd"
    echo "windows-build-r2: run $id purged (logs + result removed)."
  else
    if [ -d "$rd/snapshot" ]; then rm -rf "$rd/snapshot"; echo "windows-build-r2: run $id snapshot removed (logs/result kept)."
    else echo "windows-build-r2: run $id has no snapshot left."
    fi
  fi
done
exit "$code"
