#!/usr/bin/env bash
# Launch an isolated partial Windows build (prolog + selected phases) in tmux.
# Separate skill from windows-build-r2: shared workspace cache + run lock only.
# Never writes the full pipeline's output, last-good, ZIP, R2 key or receipts.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# Reuse shared paths/helpers (read-only); this skill writes only its own runs.
# shellcheck disable=SC1091
. "$SCRIPT_DIR/../../windows-build-r2/scripts/common.sh"
# Partial players are smaller (fewer scenes); 30 GiB headroom is enough.
# (The full pipeline's own 50 GiB check is untouched.)
MIN_FREE_BYTES=$((30 * 1024 * 1024 * 1024))

PART_SKILL_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

usage() {
  cat <<'EOF'
Usage: start.sh [--scenes 1,2] [--overwrite-r2] [--skip-upload] [--dry-run]

  Scene picker (prolog is mandatory and always built first):
    0 = Prolog saja (KamarKos + RumahAtmaja + KoridorKos, startup)
    1 = Phase 1 Babak1_Rusunawa
    2 = Phase 2 Babak2_RSUD
    3 = Phase 3 Babak3_PabrikGula
    4 = Phase 4 KarangSetra
    5 = Phase 5 PasarSetan
  Examples: --scenes 1,2  -> prolog + phase 1 + 2 (TAG Prolog-P1P2)
            --scenes 3    -> prolog + phase 3 (TAG Prolog-P3)
            --scenes 0    -> prolog only (TAG Prolog)
  Without --scenes, an interactive picker is shown. full/all is refused
  here — full releases are the job of windows-build-r2.

  --overwrite-r2  Allow replacing the existing tagged R2 release object.
  --skip-upload   Build + package only; skip the R2 upload step.
  --dry-run       Check prerequisites and print the plan; start nothing.

Partial builds always cook full assets (no scripts-only patching) into
Builds/Windows-<TAG>/, refresh only last-good-<tag>/, and upload to their
own R2 key. The full release is never touched.

Manual use only: run when the user explicitly requests a partial Windows build.
EOF
}

# normalize_spec <raw> — prints "TAG|spec" or exits 1.
normalize_spec() {
  python3 - "$1" <<'EOF'
import re,sys
raw = sys.argv[1] or ''
s = raw.strip().lower()
if s in ('', 'full', 'all'):
    sys.exit("refuse-full")
inc = set()
for tok in re.split(r'[,\s;+]+', s):
    t = tok.strip().lower()
    if t in ('prolog', 'p', '0', ''):
        continue
    if len(t) == 1 and t in '12345':
        inc.add(int(t)); continue
    sys.exit("bad-token:" + tok)
digits = ''.join(str(p) for p in sorted(inc))
tag = 'Prolog' if not digits else 'Prolog-' + '-'.join('P' + d for d in digits)
print(tag + '|' + (','.join(str(p) for p in sorted(inc)) if inc else 'prolog'))
EOF
}

pick_interactive() {
  cat >&2 <<'EOF'
Pilih phase yang ikut dibuild (prolog selalu ikut + jadi startup):
  0 = Prolog saja
  1 = Phase 1 Babak1_Rusunawa
  2 = Phase 2 Babak2_RSUD
  3 = Phase 3 Babak3_PabrikGula
  4 = Phase 4 KarangSetra
  5 = Phase 5 PasarSetan
Ketik mis. 1,2  (atau 0 untuk prolog saja):
EOF
  local ans
  IFS= read -r ans </dev/tty || { echo "windows-build-partial: no input; aborted." >&2; exit 1; }
  printf '%s' "$ans"
}

SPEC=""; OVERWRITE_R2=0; SKIP_UPLOAD=0; DRY_RUN=0
while [ $# -gt 0 ]; do
  case "$1" in
    --scenes) SPEC="${2:?--scenes needs a value like 1,2}"; shift 2 ;;
    --scenes=*) SPEC="${1#--scenes=}"; shift ;;
    --overwrite-r2) OVERWRITE_R2=1; shift ;;
    --skip-upload) SKIP_UPLOAD=1; shift ;;
    --dry-run) DRY_RUN=1; shift ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown flag: $1" >&2; usage >&2; exit 1 ;;
  esac
done
[ -n "$SPEC" ] || SPEC="$(pick_interactive)"
NORM="$(normalize_spec "$SPEC")" || {
  case "$NORM" in
    refuse-full) echo "windows-build-partial: 'full' refused — use windows-build-r2 for full releases." >&2 ;;
    bad-token:*) echo "windows-build-partial: bad token '${NORM#bad-token:}' (use numbers 1-5, e.g. 1,2)." >&2 ;;
    *) echo "windows-build-partial: invalid spec '$SPEC'." >&2 ;;
  esac
  exit 1
}
TAG="${NORM%%|*}"; CANON="${NORM#*|}"
case "$TAG" in *[^A-Za-z0-9-]*|"") echo "windows-build-partial: internal tag error." >&2; exit 1;; esac
ZIP="LayarTerakhir-Windows-x86_64-$TAG.zip"
OUTSUB="Windows-$TAG"
LASTGOOD_P="$BASE/last-good-$(echo "$TAG" | tr 'A-Z' 'a-z')"

mkdir -p "$BASE/runs"
exec 9>"$BASE/.start.lock"
flock -n 9 || { echo "windows-build-partial: another start is in progress; try again." >&2; exit 1; }

prereqs_ok=1
for c in tmux rclone rsync python3 git; do
  have "$c" || { echo "windows-build-partial: missing required tool: $c" >&2; prereqs_ok=0; }
done
[ -x "$UNITY_BIN" ] || { echo "windows-build-partial: Unity not found: $UNITY_BIN" >&2; prereqs_ok=0; }
rclone listremotes 2>/dev/null | grep -qx "r2:" || { echo "windows-build-partial: rclone remote 'r2:' is not configured" >&2; prereqs_ok=0; }
avail=$(df --output=avail -B1 "$BASE" 2>/dev/null | tail -n 1 | tr -d ' ' || echo "")
if [ -n "$avail" ] && [ "$avail" -lt "$MIN_FREE_BYTES" ]; then
  echo "windows-build-partial: only ${avail} bytes free under $BASE, need 50 GiB" >&2; prereqs_ok=0
fi
[ "$prereqs_ok" -eq 1 ] || exit 1

if [ -f "$(active_file)" ]; then
  prev="$(cat "$(active_file)")"
  prev_status="$(jget "$(run_dir "$prev")" status 2>/dev/null || echo "")"
  if [ "$prev_status" = "Running" ] || tmux_alive; then
    echo "windows-build-partial: build already running: $prev" >&2
    echo "  status: $PART_SKILL_DIR/scripts/status.sh | logs: $PART_SKILL_DIR/scripts/logs.sh" >&2
    exit 1
  fi
  echo "windows-build-partial: clearing stale active-run marker: $prev" >&2
  [ "$DRY_RUN" -eq 0 ] && rm -f "$(active_file)"
elif tmux_alive; then
  echo "windows-build-partial: orphan tmux session '$SESSION' exists with no active run." >&2
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
  echo "windows-build-partial DRY RUN — prerequisites OK, nothing started."
  echo "  live root : $LIVE_ROOT"
  echo "  commit    : $SHORT (dirty=$DIRTY)"
  echo "  spec      : $CANON  (tag $TAG)"
  echo "  output    : Builds/$OUTSUB/  last-good: $LASTGOOD_P"
  echo "  archive   : $ZIP"
  echo "  r2 key    : ${R2DIR}${ZIP}"
  echo "  mode      : full (partial builds always cook full assets)"
  empty="$(mktemp -d)"
  rsync -a --dry-run --stats --exclude-from="$SCRIPT_DIR/../windows-build-r2/scripts/workspace-excludes.txt" "$LIVE_ROOT/" "$empty/" 2>/dev/null | grep -E "^(Number of files|Total file size)" || true
  rmdir "$empty"
  exit 0
fi

STAMP=$(date -u +%Y%m%d-%H%M%S)
RUN_ID="$STAMP-$SHORT-partial-${TAG,,}"
[ "$DIRTY" -eq 1 ] && RUN_ID="$RUN_ID-dirty"
n=1; while [ -e "$BASE/runs/$RUN_ID" ]; do n=$((n+1)); RUN_ID="$STAMP-$SHORT-partial-${TAG,,}-$n"; done
RUN_DIR="$BASE/runs/$RUN_ID"
mkdir -p "$RUN_DIR/logs" "$RUN_DIR/result"
cat >"$RUN_DIR/config.env" <<EOF
BASE="$BASE"
SESSION="$SESSION"
RUN_ID="$RUN_ID"
RUN_DIR="$RUN_DIR"
LIVE_ROOT="$LIVE_ROOT"
SKILL_DIR="$PART_SKILL_DIR"
OVERWRITE_R2="$OVERWRITE_R2"
SKIP_UPLOAD="$SKIP_UPLOAD"
HEAD="$HEAD"
SHORT="$SHORT"
DIRTY="$DIRTY"
SCENES_SPEC="$CANON"
BUILD_TAG="$TAG"
PART_ZIP_NAME="$ZIP"
OUT_SUBDIR="$OUTSUB"
LASTGOOD_P="$LASTGOOD_P"
EOF
HEAD="$HEAD" SHORT="$SHORT" DIRTY="$DIRTY" RUN_ID="$RUN_ID" SESSION="$SESSION" \
LIVE_ROOT="$LIVE_ROOT" STATUS_TXT="$STATUS_TXT" SCENES_SPEC="$CANON" BUILD_TAG="$TAG" ZIP_NAME="$ZIP" python3 - "$RUN_DIR" <<'EOF'
import json,os,sys,datetime
run = sys.argv[1]
now = datetime.datetime.now(datetime.timezone.utc).isoformat()
st = {
    "runId": os.environ["RUN_ID"], "session": os.environ["SESSION"],
    "liveRoot": os.environ["LIVE_ROOT"],
    "sourceCommit": os.environ["HEAD"], "sourceShort": os.environ["SHORT"],
    "sourceDirty": os.environ["DIRTY"] == "1",
    "sourceStatus": os.environ.get("STATUS_TXT", ""),
    "partial": True, "sceneSpec": os.environ["SCENES_SPEC"],
    "buildTag": os.environ["BUILD_TAG"], "archiveName": os.environ["ZIP_NAME"],
    "buildPlan": {"mode": "full", "reason": "partial builds always cook full assets"},
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

if ! tmux new-session -d -s "$SESSION" -c "$RUN_DIR" "$PART_SKILL_DIR/scripts/pipeline-partial.sh" "$RUN_DIR"; then
  rm -f "$(active_file)"
  echo "windows-build-partial: failed to create tmux session" >&2
  exit 1
fi

echo "windows-build-partial started: $RUN_ID (commit $SHORT, dirty=$DIRTY, spec=$CANON, tag=$TAG)"
echo "  output: Builds/$OUTSUB/  archive: $ZIP"
echo "  status: $PART_SKILL_DIR/scripts/status.sh"
echo "  logs:   $PART_SKILL_DIR/scripts/logs.sh"
echo "The full pipeline, its last-good player and its R2 release are untouched."
