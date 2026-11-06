#!/usr/bin/env bash
# Isolated partial Windows build + tagged ZIP + own R2 key. Runs in detached tmux.
# Separate skill from windows-build-r2: shares only the workspace cache and the
# run lock. Never writes the full pipeline's output, last-good, ZIP or receipts.
set -euo pipefail
RUN_DIR="${1:?usage: pipeline-partial.sh <run-dir>}"
# shellcheck disable=SC1091
. "$RUN_DIR/config.env"
export WINBUILD_BASE="$BASE" WINBUILD_SESSION="$SESSION"
export WINDOWS_BUILD_SCENES="$SCENES_SPEC" WINDOWS_BUILD_TAG="$BUILD_TAG"
# Shared helpers only (no writes to the full pipeline's paths).
# shellcheck disable=SC1091
. "$SKILL_DIR/../windows-build-r2/scripts/common.sh"
# config.env carries the tagged archive name; restore it here because
# common.sh defaults ZIP_NAME to the full release archive.
ZIP_NAME="$PART_ZIP_NAME"

SNAP="$WORKSPACE"
LOGS="$RUN_DIR/logs"
RESULT="$RUN_DIR/result"
OUTDIR="$SNAP/Builds/$OUT_SUBDIR"
# Workspace has no .git (excluded); point git at the live repo for provenance only.
export GIT_DIR="$LIVE_ROOT/.git"
mkdir -p "$LOGS" "$RESULT/receipts" "$WORKSPACE" "$LASTGOOD_P"
exec > >(tee -a "$LOGS/pipeline.log") 2>&1
set -x

STEP="init"
finish() {
  local code=$?
  set +e
  trap - EXIT
  set +x
  jset "$RUN_DIR" finishedUtc "\"$(utc_now)\"" 2>/dev/null || true
  if [ "$code" -eq 0 ]; then jset "$RUN_DIR" status '"Succeeded"' 2>/dev/null || true
  else jset "$RUN_DIR" status '"Failed"' 2>/dev/null || true; fi
  if [ -f "$BASE/active-run" ] && [ "$(cat "$BASE/active-run")" = "$RUN_ID" ]; then
    rm -f "$BASE/active-run"
  fi
  echo "windows-build-partial run $RUN_ID finished with exit $code"
  exit "$code"
}
trap finish EXIT

die() { # <code> <message>
  echo "windows-build-partial FAILED [$STEP]: $2" >&2
  jset "$RUN_DIR" "steps.$STEP.exit" "$1"
  jset_str "$RUN_DIR" "steps.$STEP.error" "$2"
  exit "$1"
}

step_ok() { # <step>
  jset "$RUN_DIR" "steps.$1.exit" 0
}

echo "windows-build-partial pipeline $RUN_ID (commit $SHORT, dirty=$DIRTY, spec=$SCENES_SPEC, tag=$BUILD_TAG)"

# 1. Persistent incremental workspace (shared cache with the full pipeline).
STEP="workspace"
if ! rsync -a --delete --exclude-from="$SKILL_DIR/../windows-build-r2/scripts/workspace-excludes.txt" "$LIVE_ROOT/" "$WORKSPACE/" >"$LOGS/sync.log" 2>&1; then
  die 2 "workspace sync failed; see logs/sync.log"
fi
if [ -d "$LIVE_ROOT/Library" ]; then
  if ! rsync -a "$LIVE_ROOT/Library/" "$WORKSPACE/Library/" >"$LOGS/library-sync.log" 2>&1; then
    die 2 "workspace Library sync failed; see logs/library-sync.log"
  fi
fi
step_ok workspace

# 2. Pre-build source record.
STEP="record-prebuild"
if ! (cd "$SNAP" && python3 Tools/record_designer_sources.py --output Audit/winbuild-prebuild-sources.json >"$LOGS/record-prebuild.log" 2>&1); then
  die 2 "pre-build source record failed; see logs/record-prebuild.log"
fi
step_ok record-prebuild

# 3. Unity Windows build (partial scenes, always full asset cook).
STEP="unity-build"
PLAYER_EXE="$OUTDIR/LayarTerakhir.exe"
LASTGOOD_EXE="$LASTGOOD_P/Builds/$OUT_SUBDIR/LayarTerakhir.exe"
if [ ! -f "$PLAYER_EXE" ] && [ -f "$LASTGOOD_EXE" ]; then
  mkdir -p "$OUTDIR"
  rsync -a --delete "$LASTGOOD_P/Builds/$OUT_SUBDIR/" "$OUTDIR/" >"$LOGS/lastgood-restore.log" 2>&1 \
    || echo "no previous partial player output; falling back to full build"
fi
if ! (cd "$SNAP" && python3 Tools/run_golden_checks.py --stage windows >"$LOGS/stage-windows.log" 2>&1); then
  build_code=$?
  if [ -f "$LASTGOOD_EXE" ]; then
    echo "partial build failed; restoring workspace player base from per-tag last-good"
    rsync -a --delete "$LASTGOOD_P/Builds/$OUT_SUBDIR/" "$OUTDIR/" >"$LOGS/lastgood-restore.log" 2>&1 || true
  fi
  die 2 "Unity partial Windows build failed (exit $build_code); see logs/stage-windows.log and logs/windows-development.log. Per-tag last-good preserved."
fi
for f in windows-build-invocation.json windows-development.log windows-development.console.log windows-development-build-report.json; do
  [ -f "$SNAP/Builds/Logs/$f" ] && cp "$SNAP/Builds/Logs/$f" "$LOGS/" || true
done
python3 - "$RUN_DIR" <<'EOF' || true
import json,sys
run = sys.argv[1]
st = json.load(open(run + "/status.json"))
try:
    rep = json.load(open(run + "/logs/windows-development-build-report.json"))
    st["build"] = {k: rep.get(k) for k in
        ("result", "target", "scriptingBackend", "options", "totalErrors",
         "totalWarnings", "totalSizeBytes", "durationSeconds")}
    st["build"]["sceneCount"] = len(rep.get("scenes", []))
    st["build"]["scenes"] = rep.get("scenes", [])
    st["build"]["mode"] = "full"
    st["build"]["cacheReused"] = True
    st["build"]["unityVersion"] = rep.get("unityVersion", "")
except Exception as e:
    print("build report parse skipped:", e)
open(run + "/status.json", "w").write(json.dumps(st, indent=2))
EOF
FINAL_MODE="$(jget "$RUN_DIR" build.mode)"; [ -n "$FINAL_MODE" ] || FINAL_MODE="full"
[ "$(jget "$RUN_DIR" build.result 2>/dev/null || echo "")" = "Succeeded" ] || {
  if [ -f "$LASTGOOD_EXE" ]; then
    echo "build failed; restoring workspace player base from per-tag last-good"
    rsync -a --delete "$LASTGOOD_P/Builds/$OUT_SUBDIR/" "$OUTDIR/" >"$LOGS/lastgood-restore.log" 2>&1 || true
  fi
  die 2 "Unity partial Windows build failed; see logs/stage-windows.log and logs/windows-development.log. Per-tag last-good preserved."
}
step_ok unity-build

# 4. Post-build source record.
STEP="record-sources"
if ! (cd "$SNAP" && python3 Tools/record_designer_sources.py >"$LOGS/record.log" 2>&1); then
  die 2 "source record failed; see logs/record.log"
fi
step_ok record-sources

# 5. Package + ZIP validation (subset-aware asserts, tagged archive).
STEP="package"
if [ -e "$SNAP/Builds/$ZIP_NAME" ] || [ -e "$SNAP/Builds/$ZIP_NAME.sha256" ]; then
  mkdir -p "$RUN_DIR/stale-archive"
  mv -f "$SNAP/Builds/$ZIP_NAME" "$SNAP/Builds/$ZIP_NAME.sha256" "$RUN_DIR/stale-archive/" 2>/dev/null || true
  echo "moved stale workspace archive aside into $RUN_DIR/stale-archive"
fi
if ! (cd "$SNAP" && python3 Tools/package_windows_build.py --scenes-spec "$SCENES_SPEC" --build-tag "$BUILD_TAG" >"$LOGS/package.log" 2>&1); then
  die 2 "packaging/ZIP validation failed; see logs/package.log"
fi
[ -f "$SNAP/Builds/$ZIP_NAME" ] || die 2 "packager reported success but ZIP is missing"
cp "$SNAP/Builds/$ZIP_NAME" "$SNAP/Builds/$ZIP_NAME.sha256" "$RESULT/"
python3 - "$RUN_DIR" "$RESULT/$ZIP_NAME" <<'EOF'
import hashlib,json,sys,os
run, arc = sys.argv[1], sys.argv[2]
st = json.load(open(run + "/status.json"))
h = hashlib.sha256()
with open(arc, "rb") as f:
    for b in iter(lambda: f.read(1024 * 1024), b""): h.update(b)
st["archive"] = {"name": os.path.basename(arc), "sizeBytes": os.path.getsize(arc), "sha256": h.hexdigest()}
open(run + "/status.json", "w").write(json.dumps(st, indent=2))
EOF
step_ok package

# 5b. Refresh ONLY the per-tag last-good on success. The full last-good is never touched.
mkdir -p "$LASTGOOD_P/Builds/$OUT_SUBDIR"
if ! rsync -a --delete "$OUTDIR/" "$LASTGOOD_P/Builds/$OUT_SUBDIR/" >"$LOGS/lastgood-refresh.log" 2>&1; then
  die 2 "per-tag last-good refresh failed; see logs/lastgood-refresh.log"
fi
python3 - "$RUN_DIR" "$LASTGOOD_P/meta.json" <<'EOF' || true
import datetime,json,sys
run, meta = sys.argv[1], sys.argv[2]
st = json.load(open(run + "/status.json"))
bld = st.get("build", {})
open(meta, "w").write(json.dumps({
    "commit": st.get("sourceCommit", ""),
    "dirty": bool(st.get("sourceDirty")),
    "runId": st.get("runId", ""),
    "utc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
    "mode": "full",
    "sceneSpec": st.get("sceneSpec", ""),
    "buildTag": st.get("buildTag", ""),
    "unityVersion": bld.get("unityVersion", ""),
}, indent=2) + "\n")
EOF
[ -f "$LASTGOOD_P/meta.json" ] || die 2 "per-tag last-good metadata was not written"

# 6. Upload (skipped on request, blocked on existing tagged release without --overwrite-r2).
STEP="upload"
if [ "$SKIP_UPLOAD" -eq 1 ]; then
  jset "$RUN_DIR" upload.status '"skipped"'
  step_ok upload
else
  guard=0; r2_remote_exists || guard=$?
  if [ "$guard" -eq 0 ]; then
    if [ "$OVERWRITE_R2" -eq 1 ]; then
      echo "Tagged remote release exists; overwrite explicitly allowed (--overwrite-r2)."
    else
      jset "$RUN_DIR" upload.status '"blocked-existing"'
      die 3 "R2 release $ZIP_NAME already exists; rerun start.sh with --overwrite-r2 to replace it intentionally. Local ZIP preserved in result/."
    fi
  elif [ "$guard" -ne 1 ]; then
    jset "$RUN_DIR" upload.status '"failed"'
    die 3 "rclone pre-upload check failed; verify rclone/r2 access and retry."
  fi
  if ! (cd "$SNAP" && python3 Tools/upload_windows_build.py --archive-name "$ZIP_NAME" >"$LOGS/upload.log" 2>&1); then
    jset "$RUN_DIR" upload.status '"failed"'
    die 3 "R2 upload/readback verification failed; see logs/upload.log"
  fi
  python3 - "$RUN_DIR" "$WORKSPACE" <<'EOF' || true
import json,sys
run, ws = sys.argv[1], sys.argv[2]
rec = json.load(open(ws + "/Audit/r2-windows-build-upload.json"))
st = json.load(open(run + "/status.json"))
st["upload"] = {"status": "verified", "remote": rec["remote"],
                "sizeBytes": rec["sizeBytes"], "sha256": rec["sha256"],
                "verifiedUtc": rec["verifiedUtc"]}
open(run + "/status.json", "w").write(json.dumps(st, indent=2))
EOF
  step_ok upload
fi

# 7. Tagged receipts beside the ZIP (never the release receipt names).
TAGL="$(echo "$BUILD_TAG" | tr 'A-Z' 'a-z')"
for pair in "designer-build-sources.json:designer-build-sources-$TAGL.json" \
            "golden-build-summary.json:golden-build-summary-$TAGL.json" \
            "atmaja-build-summary.json:atmaja-build-summary-$TAGL.json" \
            "golden-windows-invocation.json:golden-windows-invocation-$TAGL.json" \
            "atmaja-windows-invocation.json:atmaja-windows-invocation-$TAGL.json" \
            "r2-windows-build-upload.json:r2-partial-upload-$TAGL.json"; do
  src="${pair%%:*}"; dst="${pair##*:}"
  [ -f "$SNAP/Audit/$src" ] && cp "$SNAP/Audit/$src" "$RESULT/receipts/$dst" || true
done
for pair in "windows-build-verification.json:windows-build-verification-$TAGL.json" \
            "windows-build-files.sha256:windows-build-files-$TAGL.sha256" \
            "windows-development-build-report.json:windows-development-build-report-$TAGL.json" \
            "windows-build-invocation.json:windows-build-invocation-$TAGL.json"; do
  src="${pair%%:*}"; dst="${pair##*:}"
  [ -f "$SNAP/Builds/Logs/$src" ] && cp "$SNAP/Builds/Logs/$src" "$RESULT/receipts/$dst" || true
done

echo "windows-build-partial pipeline $RUN_ID complete."
