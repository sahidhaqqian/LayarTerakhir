#!/usr/bin/env bash
# Copy partial-build receipts (and optionally the tagged ZIP) into the live tree.
# Writes ONLY under Audit/PartialBuilds/<tag>/ (+ tagged ZIP in Builds/).
# Never touches release receipts, the full archive, or gameplay code/scenes/assets.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/../../windows-build-r2/scripts/common.sh"

usage() {
  cat <<'EOF'
Usage: collect.sh [--run <id>] [--with-zip] [--force]

  Copies from the finished partial run into the live tree:
    result/receipts/*.json -> Audit/PartialBuilds/<tag>/ (tagged provenance,
                             summaries, invocation records, R2 upload receipt)
  --with-zip  Also copy the tagged ZIP + .sha256 into live Builds/.
              Refuses to overwrite an existing file unless --force is given.

Run only after status.sh reports Succeeded.
EOF
}

WANT=""; WITH_ZIP=0; FORCE=0
while [ $# -gt 0 ]; do
  case "$1" in
    --run) WANT="${2:?--run needs an id}"; shift 2 ;;
    --with-zip) WITH_ZIP=1; shift ;;
    --force) FORCE=1; shift ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown flag: $a" >&2; usage >&2; exit 1 ;;
  esac
done

if [ -z "$WANT" ]; then
  WANT="$(for d in "$BASE"/runs/*/; do
    [ -f "$d/status.json" ] || continue
    python3 -c 'import json,sys; st=json.load(open(sys.argv[1])); print(st.get("runId","") if st.get("partial") else "")' "$d/status.json"
  done | sort | tail -n 1)"
fi
[ -n "$WANT" ] || { echo "windows-build-partial: no partial runs recorded yet." >&2; exit 1; }
RUN_DIR="$(run_dir "$WANT")"
[ "$(jget "$RUN_DIR" status)" = "Succeeded" ] || {
  echo "windows-build-partial: run $WANT is not Succeeded; refusing to collect." >&2; exit 1; }
TAG="$(jget "$RUN_DIR" buildTag)"
ZIPN="$(jget "$RUN_DIR" archiveName)"
[ -f "$RUN_DIR/result/$ZIPN" ] || {
  echo "windows-build-partial: run $WANT has no result ZIP; nothing to collect." >&2; exit 1; }

DEST="$LIVE_ROOT/Audit/PartialBuilds/$TAG"
mkdir -p "$DEST"
for f in "$RUN_DIR"/result/receipts/*; do
  [ -f "$f" ] || continue
  cp "$f" "$DEST/$(basename "$f")"
  echo "collected Audit/PartialBuilds/$TAG/$(basename "$f")"
done

if [ "$WITH_ZIP" -eq 1 ]; then
  dest="$LIVE_ROOT/Builds/$ZIPN"
  if [ -e "$dest" ] && [ "$FORCE" -eq 0 ]; then
    echo "windows-build-partial: live $dest already exists; rerun with --force to replace it." >&2
    exit 1
  fi
  cp "$RUN_DIR/result/$ZIPN" "$RUN_DIR/result/$ZIPN.sha256" "$LIVE_ROOT/Builds/"
  echo "collected Builds/$ZIPN"
fi

echo "Run $WANT collected. Review with git status/diff, then commit and push."
