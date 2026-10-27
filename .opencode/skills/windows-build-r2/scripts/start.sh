#!/usr/bin/env bash
# Launch an isolated Windows x86_64 Mono build + ZIP + R2 upload in tmux.
# The whole pipeline runs detached; this command returns immediately.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/common.sh"

usage() {
  cat <<'EOF'
Usage: start.sh [--overwrite-r2] [--skip-upload] [--full-build] [--dry-run]

  --overwrite-r2  Allow replacing the existing R2 release object.
                  Without it, the run stops before upload if a release exists.
  --skip-upload   Build + package only; skip the R2 upload step.
  --full-build    Force a full incremental build even when a scripts-only
                  build would be eligible (e.g. after a Unity upgrade or a
                  suspicious incremental state).
  --dry-run       Check prerequisites and print the plan; start nothing.

Build mode is chosen automatically: scripts-only (BuildOptions.BuildScriptsOnly,
code changes only) when every change since the last successful build is
compatible C# (or build-irrelevant files); otherwise a full incremental build.
Scripts-only reuses the persistent workspace Library/Bee cache and the
previous player output, and falls back to a full build automatically if the
scripts-only attempt fails.

Manual use only: run when the user explicitly requests a Windows build / R2 upload.
EOF
}

# path_eligible_scripts_only <path> — exit 0 when a change to this path can
# be covered by a scripts-only rebuild. Conservative by design: anything not
# explicitly known-safe forces a full build, so no generated build can ever
# silently miss a change.
path_eligible_scripts_only() {
  local lower="${1,,}"
  case "$1" in
    ProjectSettings/*|Packages/*) return 1 ;;
  esac
  case "$1" in
    Assets/*.cs|Assets/*.cs.meta|Assets/*.asmdef) return 0 ;;
  esac
  case "$1" in
    Assets/*) return 1 ;;
  esac
  # Outside Assets/ the player build only sees compiled C#; everything else
  # is either ignored or provably build-irrelevant.
  case "$1" in
    Audit/*|Docs/*|Tools/*|.opencode/*|graphify-out/*|ArtSources/*|Builds/*) return 0 ;;
  esac
  case "$1" in
    .gitignore|.gitattributes|.editorconfig) return 0 ;;
  esac
  case "$lower" in
    *.md|*.markdown|*.txt|*.json|*.py|*.sh|*.yml|*.yaml|*.toml|*.cfg|*.ini|*.docx|*.pdf|*.xlsx|*.pptx|*.csv) return 0 ;;
  esac
  return 1
}

# decide_build_mode — prints "<mode>|<reason>" and returns 0.
# mode is "full" unless every change since the last good build is eligible.
decide_build_mode() {
  if [ ! -f "$LASTGOOD_META" ]; then echo "full|no previous successful build in $LASTGOOD"; return 0; fi
  local base unity_required unity_built
  base=$(python3 -c 'import json,sys;print(json.load(open(sys.argv[1])).get("commit",""))' "$LASTGOOD_META" 2>/dev/null)
  [ -n "$base" ] || { echo "full|last-good metadata has no commit"; return 0; }
  git -C "$LIVE_ROOT" merge-base --is-ancestor "$base" HEAD 2>/dev/null \
    || { echo "full|$base is not an ancestor of HEAD (history changed)"; return 0; }
  unity_built=$(python3 -c 'import json,sys;print(json.load(open(sys.argv[1])).get("unityVersion",""))' "$LASTGOOD_META" 2>/dev/null)
  unity_required=$(grep -m1 m_EditorVersion "$LIVE_ROOT/ProjectSettings/ProjectVersion.txt" 2>/dev/null | sed 's/.*: //;s/[^0-9a-zA-Z.+-]//g')
  if [ -n "$unity_built" ] && [ -n "$unity_required" ] && [ "$unity_built" != "$unity_required" ]; then
    echo "full|Unity version changed ($unity_built -> $unity_required)"; return 0
  fi
  local changed ineligible
  changed=$( { git -C "$LIVE_ROOT" diff --name-only "$base" HEAD 2>/dev/null
    git -C "$LIVE_ROOT" status --porcelain=v1 -uall --no-renames 2>/dev/null | sed 's/^.\{3\}//; s/^"//; s/"$//'; } | sed '/^$/d' | sort -u )
  ineligible=""
  while IFS= read -r f; do
    [ -n "$f" ] || continue
    path_eligible_scripts_only "$f" || ineligible="${ineligible}${ineligible:+, }$f"
  done <<<"$changed"
  if [ -n "$ineligible" ]; then
    echo "full|non-code changes since $base: $(echo "$ineligible" | tr ' ' '\n' | head -n 5 | tr '\n' ' ')..."
  else
    echo "scripts-only|only compatible C# / build-irrelevant files changed since $base"
  fi
  return 0
}

OVERWRITE_R2=0; SKIP_UPLOAD=0; DRY_RUN=0; FULL_BUILD=0
for a in "$@"; do
  case "$a" in
    --overwrite-r2) OVERWRITE_R2=1 ;;
    --skip-upload) SKIP_UPLOAD=1 ;;
    --full-build) FULL_BUILD=1 ;;
    --dry-run) DRY_RUN=1 ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown flag: $a" >&2; usage >&2; exit 1 ;;
  esac
done

mkdir -p "$BASE/runs"
exec 9>"$BASE/.start.lock"
flock -n 9 || { echo "windows-build-r2: another start is in progress; try again." >&2; exit 1; }

prereqs_ok=1
for c in tmux rclone rsync python3 git; do
  have "$c" || { echo "windows-build-r2: missing required tool: $c" >&2; prereqs_ok=0; }
done
[ -x "$UNITY_BIN" ] || { echo "windows-build-r2: Unity not found: $UNITY_BIN" >&2; prereqs_ok=0; }
rclone listremotes 2>/dev/null | grep -qx "r2:" || { echo "windows-build-r2: rclone remote 'r2:' is not configured" >&2; prereqs_ok=0; }
avail=$(df --output=avail -B1 "$BASE" 2>/dev/null | tail -n 1 | tr -d ' ' || echo "")
if [ -n "$avail" ] && [ "$avail" -lt "$MIN_FREE_BYTES" ]; then
  echo "windows-build-r2: only ${avail} bytes free under $BASE, need 50 GiB" >&2; prereqs_ok=0
fi
[ "$prereqs_ok" -eq 1 ] || exit 1

# Duplicate-build guard: one active run at a time.
if [ -f "$(active_file)" ]; then
  prev="$(cat "$(active_file)")"
  prev_status="$(jget "$(run_dir "$prev")" status 2>/dev/null || echo "")"
  if [ "$prev_status" = "Running" ] || tmux_alive; then
    echo "windows-build-r2: build already running: $prev" >&2
    echo "  status: $SCRIPT_DIR/status.sh | logs: $SCRIPT_DIR/logs.sh" >&2
    exit 1
  fi
  echo "windows-build-r2: clearing stale active-run marker: $prev" >&2
  [ "$DRY_RUN" -eq 0 ] && rm -f "$(active_file)"
elif tmux_alive; then
  echo "windows-build-r2: orphan tmux session '$SESSION' exists with no active run." >&2
  echo "  Inspect with: tmux capture-pane -p -t $SESSION | tail" >&2
  echo "  Then clear with: $SCRIPT_DIR/clean.sh --force" >&2
  exit 1
fi

HEAD=$(git -C "$LIVE_ROOT" rev-parse HEAD)
SHORT=$(git -C "$LIVE_ROOT" rev-parse --short HEAD)
DIRTY=0; STATUS_TXT=""
if [ -n "$(git -C "$LIVE_ROOT" status --porcelain)" ]; then
  DIRTY=1
  STATUS_TXT=$(git -C "$LIVE_ROOT" status --porcelain | sed -n '1,20p')
fi

if [ "$DRY_RUN" -eq 1 ]; then
  echo "windows-build-r2 DRY RUN — prerequisites OK, nothing started."
  echo "  live root : $LIVE_ROOT"
  echo "  commit    : $SHORT (dirty=$DIRTY)"
  echo "  unity     : $UNITY_BIN"
  echo "  session   : $SESSION (detached tmux)"
  echo "  workspace : $WORKSPACE (persistent Library/Bee cache + player base)"
  echo "  last-good : $([ -f "$LASTGOOD_META" ] && cat "$LASTGOOD_META" || echo "(none yet)")"
  echo "  flags     : overwrite-r2=$OVERWRITE_R2 skip-upload=$SKIP_UPLOAD full-build=$FULL_BUILD"
  decision="$(decide_build_mode)"
  if [ "$FULL_BUILD" -eq 1 ]; then
    echo "  build mode: full (forced by --full-build; auto choice was: $decision)"
  else
    echo "  build mode: $decision"
  fi
  empty="$(mktemp -d)"
  rsync -a --dry-run --stats --exclude-from="$SCRIPT_DIR/workspace-excludes.txt" "$LIVE_ROOT/" "$empty/" 2>/dev/null | grep -E "^(Number of files|Total file size)" || true
  rmdir "$empty"
  echo "  plan      : sync workspace -> Unity build -> record sources -> package+ZIP -> R2 upload"
  exit 0
fi

BUILD_MODE="full"; BUILD_REASON="forced by --full-build"
if [ "$FULL_BUILD" -eq 0 ]; then
  decision="$(decide_build_mode)"
  BUILD_MODE="${decision%%|*}"; BUILD_REASON="${decision#*|}"
fi

STAMP=$(date -u +%Y%m%d-%H%M%S)
RUN_ID="$STAMP-$SHORT"
[ "$DIRTY" -eq 1 ] && RUN_ID="$RUN_ID-dirty"
n=1; while [ -e "$BASE/runs/$RUN_ID" ]; do n=$((n+1)); RUN_ID="$STAMP-$SHORT-$n"; done
RUN_DIR="$BASE/runs/$RUN_ID"
mkdir -p "$RUN_DIR/logs" "$RUN_DIR/result"
cat >"$RUN_DIR/config.env" <<EOF
BASE="$BASE"
SESSION="$SESSION"
RUN_ID="$RUN_ID"
RUN_DIR="$RUN_DIR"
LIVE_ROOT="$LIVE_ROOT"
SKILL_DIR="$SKILL_DIR"
OVERWRITE_R2="$OVERWRITE_R2"
SKIP_UPLOAD="$SKIP_UPLOAD"
HEAD="$HEAD"
SHORT="$SHORT"
DIRTY="$DIRTY"
BUILD_MODE="$BUILD_MODE"
BUILD_REASON="$BUILD_REASON"
EOF
HEAD="$HEAD" SHORT="$SHORT" DIRTY="$DIRTY" RUN_ID="$RUN_ID" SESSION="$SESSION" \
LIVE_ROOT="$LIVE_ROOT" STATUS_TXT="$STATUS_TXT" BUILD_MODE="$BUILD_MODE" BUILD_REASON="$BUILD_REASON" python3 - "$RUN_DIR" <<'EOF'
import json,os,sys,datetime
run = sys.argv[1]
now = datetime.datetime.now(datetime.timezone.utc).isoformat()
st = {
    "runId": os.environ["RUN_ID"], "session": os.environ["SESSION"],
    "liveRoot": os.environ["LIVE_ROOT"],
    "sourceCommit": os.environ["HEAD"], "sourceShort": os.environ["SHORT"],
    "sourceDirty": os.environ["DIRTY"] == "1",
    "sourceStatus": os.environ.get("STATUS_TXT", ""),
    "buildPlan": {"mode": os.environ["BUILD_MODE"], "reason": os.environ["BUILD_REASON"]},
    "startedUtc": now, "finishedUtc": None, "status": "Running",
    "steps": {}, "build": {}, "archive": {},
    "upload": {"status": "pending"},
    "overwriteR2": False, "skipUpload": False,
}
open(run + "/status.json", "w").write(json.dumps(st, indent=2))
EOF
jset "$RUN_DIR" overwriteR2 "$([ "$OVERWRITE_R2" -eq 1 ] && echo true || echo false)"
jset "$RUN_DIR" skipUpload "$([ "$SKIP_UPLOAD" -eq 1 ] && echo true || echo false)"
echo "$RUN_ID" > "$(active_file)"

if ! tmux new-session -d -s "$SESSION" -c "$RUN_DIR" "$SKILL_DIR/scripts/pipeline.sh" "$RUN_DIR"; then
  rm -f "$(active_file)"
  echo "windows-build-r2: failed to create tmux session" >&2
  exit 1
fi

echo "windows-build-r2 started: $RUN_ID (commit $SHORT, dirty=$DIRTY, mode=$BUILD_MODE)"
echo "  mode reason: $BUILD_REASON"
echo "  status: $SCRIPT_DIR/status.sh"
echo "  logs:   $SCRIPT_DIR/logs.sh"
echo "Development can continue; the live tree and Library are untouched."
