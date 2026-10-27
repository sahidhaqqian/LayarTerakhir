#!/usr/bin/env bash
# Copy build receipts (and optionally the ZIP) from a finished run into the
# live tree. Only Audit receipts and Builds/ output are touched — never
# gameplay code, scenes or assets.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/common.sh"

usage() {
  cat <<'EOF'
Usage: collect.sh [--run <id>] [--with-zip] [--force]

  Copies from the finished run into the live tree:
    result/receipts/*.json -> Audit/ (source provenance, build summaries,
                             invocation records, R2 upload receipt)
  --with-zip  Also copy the ZIP + .sha256 into live Builds/.
              Refuses to overwrite an existing archive unless --force is
              given (the old archive is moved aside under Builds/Previous/).

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
    *) echo "Unknown flag: $1" >&2; usage >&2; exit 1 ;;
  esac
done

RUN_ID="$(resolve_run "$WANT")"
[ -n "$RUN_ID" ] || { echo "windows-build-r2: no runs recorded yet." >&2; exit 1; }
RUN_DIR="$(run_dir "$RUN_ID")"
[ "$(jget "$RUN_DIR" status)" = "Succeeded" ] || {
  echo "windows-build-r2: run $RUN_ID is not Succeeded; refusing to collect." >&2; exit 1; }
[ -f "$RUN_DIR/result/$ZIP_NAME" ] || {
  echo "windows-build-r2: run $RUN_ID has no result ZIP; nothing to collect." >&2; exit 1; }

for f in designer-build-sources.json golden-build-summary.json atmaja-build-summary.json \
         golden-windows-invocation.json atmaja-windows-invocation.json r2-windows-build-upload.json; do
  if [ -f "$RUN_DIR/result/receipts/$f" ]; then
    cp "$RUN_DIR/result/receipts/$f" "$LIVE_ROOT/Audit/$f"
    echo "collected Audit/$f"
  fi
done

if [ "$WITH_ZIP" -eq 1 ]; then
  dest="$LIVE_ROOT/Builds/$ZIP_NAME"
  if [ -e "$dest" ] && [ "$FORCE" -eq 0 ]; then
    echo "windows-build-r2: live $dest already exists; rerun with --force to replace it (old file is moved aside, not deleted)." >&2
    exit 1
  fi
  if [ -e "$dest" ]; then
    aside="$LIVE_ROOT/Builds/Previous/collect-$(date -u +%Y%m%d-%H%M%S)"
    mkdir -p "$aside"
    mv "$dest" "$dest.sha256" "$aside/" 2>/dev/null || mv "$dest" "$aside/"
    echo "moved previous live archive aside to $aside"
  fi
  cp "$RUN_DIR/result/$ZIP_NAME" "$RUN_DIR/result/$ZIP_NAME.sha256" "$LIVE_ROOT/Builds/"
  echo "collected Builds/$ZIP_NAME"
fi

echo "Run $RUN_ID collected. Review with git status/diff, then commit and push."
