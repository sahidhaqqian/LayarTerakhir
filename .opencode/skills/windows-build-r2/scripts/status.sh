#!/usr/bin/env bash
# Show Windows build status and detect failures.
# Exit: 0 succeeded, 1 running, 2 failed/blocked/none.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/common.sh"

usage() {
  cat <<'EOF'
Usage: status.sh [--run <id>] [--json] [--all]

  (no args)   Human-readable summary of the active/latest run.
  --json      Print the raw status.json of the active/latest run.
  --all       One-line table of every recorded run.
EOF
}

JSON=0; ALL=0; WANT=""
while [ $# -gt 0 ]; do
  case "$1" in
    --json) JSON=1; shift ;;
    --all) ALL=1; shift ;;
    --run) WANT="${2:?--run needs an id}"; shift 2 ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown flag: $1" >&2; usage >&2; exit 2 ;;
  esac
done

if [ "$ALL" -eq 1 ]; then
  printf '%-28s %-10s %-9s %s\n' RUN STATUS UPLOAD SOURCE
  for d in "$BASE"/runs/*/; do
    [ -f "$d/status.json" ] || continue
    python3 - "$d/status.json" <<'EOF'
import json,sys
st = json.load(open(sys.argv[1]))
print("%-28s %-10s %-9s %s%s" % (st.get("runId", "?"), st.get("status", "?"),
      st.get("upload", {}).get("status", "?"), st.get("sourceShort", "?"),
      "-dirty" if st.get("sourceDirty") else ""))
EOF
  done
  exit 0
fi

RUN_ID="$(resolve_run "$WANT")"
if [ -z "$RUN_ID" ] || [ ! -f "$(run_dir "$RUN_ID")/status.json" ]; then
  echo "windows-build-r2: no runs recorded yet."
  echo "  start: $SCRIPT_DIR/start.sh   (manual use only, on explicit user request)"
  exit 2
fi
RUN_DIR="$(run_dir "$RUN_ID")"

if [ "$JSON" -eq 1 ]; then cat "$RUN_DIR/status.json"; echo; fi
if [ "$JSON" -eq 1 ]; then
  case "$(jget "$RUN_DIR" status)" in
    Succeeded) exit 0 ;; Running) exit 1 ;; *) exit 2 ;;
  esac
fi

ALIVE="no"; tmux_alive && [ -f "$(active_file)" ] && [ "$(cat "$(active_file)")" = "$RUN_ID" ] && ALIVE="yes"
ALIVE="$ALIVE" SCRIPTS="$SCRIPT_DIR" python3 - "$RUN_DIR/status.json" <<'EOF'
import json,os,sys
st = json.load(open(sys.argv[1]))
up = st.get("upload", {})
arc = st.get("archive", {})
bld = st.get("build", {})
plan = st.get("buildPlan", {})
print("run      : %s [%s]" % (st.get("runId"), st.get("status")))
print("source   : %s%s %s" % (st.get("sourceShort"), " (dirty)" if st.get("sourceDirty") else "", st.get("sourceCommit", "")))
if plan: print("plan     : %s (%s)" % (plan.get("mode"), plan.get("reason", "")))
print("started  : %s   finished: %s" % (st.get("startedUtc"), st.get("finishedUtc") or "-"))
print("tmux live: %s (session %s)" % (os.environ["ALIVE"], st.get("session")))
print("steps    :")
order = ["workspace", "record-prebuild", "unity-build", "record-sources", "package", "upload"]
steps = st.get("steps", {})
for name in order:
    s = steps.get(name)
    if s is None: print("  %-15s pending" % name)
    elif s.get("exit") == 0: print("  %-15s ok" % name)
    else: print("  %-15s EXIT %s: %s" % (name, s.get("exit"), (s.get("error") or "")[:160]))
if bld: print("build    : %s mode=%s%s errors=%s warnings=%s scenes=%s" % (
    bld.get("result"), bld.get("mode"), " (auto fallback from scripts-only)" if bld.get("fallback") else "",
    bld.get("totalErrors"), bld.get("totalWarnings"), bld.get("sceneCount")))
if arc: print("archive  : %s bytes=%s sha256=%s" % (
    arc.get("name"), arc.get("sizeBytes"), (arc.get("sha256") or "")[:16] + "..."))
print("upload   : %s %s" % (up.get("status"), up.get("remote", "")))
s = os.environ["SCRIPTS"]
print("commands : %s/logs.sh --run %s | %s/collect.sh --run %s | %s/clean.sh" % (s, st.get("runId"), s, st.get("runId"), s))
EOF

case "$(jget "$RUN_DIR" status)" in
  Succeeded) exit 0 ;;
  Running) exit 1 ;;
  *) exit 2 ;;
esac
