#!/usr/bin/env bash
# View Windows build logs. Defaults to the active/latest run's pipeline log.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/common.sh"

usage() {
  cat <<'EOF'
Usage: logs.sh [--run <id>] [--list] [-f] [name] [-n N]

  (no args)  Tail of pipeline.log for the active/latest run.
  --list     List available log files with sizes.
  -f         Follow the log (like tail -f).
  name       One of: pipeline sync snapshot build unity package upload
              (snapshot = legacy per-run copy; sync = workspace sync log.
              build = effective attempt; per-attempt logs are
              stage-windows-scripts-only.log / stage-windows-full.log.)
  -n N       Tail lines (default 100).

Examples:
  logs.sh --list
  logs.sh unity
  logs.sh -f
EOF
}

WANT=""; LIST=0; FOLLOW=0; NAME="pipeline"; LINES=100
while [ $# -gt 0 ]; do
  case "$1" in
    --run) WANT="${2:?--run needs an id}"; shift 2 ;;
    --list) LIST=1; shift ;;
    -f) FOLLOW=1; shift ;;
    -n) LINES="${2:?--n needs a number}"; shift 2 ;;
    -h|--help) usage; exit 0 ;;
    -*) echo "Unknown flag: $1" >&2; usage >&2; exit 1 ;;
    *) NAME="$1"; shift ;;
  esac
done

RUN_ID="$(resolve_run "$WANT")"
if [ -z "$RUN_ID" ] || [ ! -d "$(run_dir "$RUN_ID")/logs" ]; then
  echo "windows-build-r2: no logs recorded yet." >&2; exit 1
fi
LOGS="$(run_dir "$RUN_ID")/logs"

if [ "$LIST" -eq 1 ]; then
  echo "run: $RUN_ID"
  ls -l --block-size=K "$LOGS" | tail -n +2
  exit 0
fi

case "$NAME" in
  pipeline) FILE="$LOGS/pipeline.log" ;;
  snapshot|sync) FILE="$LOGS/sync.log"; [ -f "$FILE" ] || FILE="$LOGS/snapshot.log" ;;
  build) FILE="$LOGS/stage-windows.log" ;;
  unity) FILE="$LOGS/windows-development.log" ;;
  package) FILE="$LOGS/package.log" ;;
  upload) FILE="$LOGS/upload.log" ;;
  *) FILE="$LOGS/$NAME"; [ -f "$FILE" ] || FILE="$LOGS/$NAME.log" ;;
esac
[ -f "$FILE" ] || { echo "windows-build-r2: no such log yet: $NAME (run $RUN_ID)" >&2; exit 1; }
echo "run: $RUN_ID  file: $FILE"
if [ "$FOLLOW" -eq 1 ]; then tail -n "$LINES" -f "$FILE"; else tail -n "$LINES" "$FILE"; fi
