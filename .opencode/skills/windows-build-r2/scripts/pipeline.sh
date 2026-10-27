#!/usr/bin/env bash
# Isolated Windows build + ZIP + R2 upload. Runs inside detached tmux.
# Never run by hand except for debugging a finished run.
set -euo pipefail
RUN_DIR="${1:?usage: pipeline.sh <run-dir>}"
# shellcheck disable=SC1091
. "$RUN_DIR/config.env"
export WINBUILD_BASE="$BASE" WINBUILD_SESSION="$SESSION"
# shellcheck disable=SC1091
. "$SKILL_DIR/scripts/common.sh"

SNAP="$RUN_DIR/snapshot"
LOGS="$RUN_DIR/logs"
RESULT="$RUN_DIR/result"
mkdir -p "$LOGS" "$RESULT/receipts"
exec > >(tee -a "$LOGS/pipeline.log") 2>&1
set -x

STEP="init"
finish() {
  set +e
  local code=$?
  trap - EXIT
  set +x
  jset "$RUN_DIR" finishedUtc "\"$(utc_now)\"" 2>/dev/null || true
  if [ "$code" -eq 0 ]; then jset "$RUN_DIR" status '"Succeeded"' 2>/dev/null || true
  else jset "$RUN_DIR" status '"Failed"' 2>/dev/null || true; fi
  if [ -f "$BASE/active-run" ] && [ "$(cat "$BASE/active-run")" = "$RUN_ID" ]; then
    rm -f "$BASE/active-run"
  fi
  echo "windows-build-r2 run $RUN_ID finished with exit $code"
  exit "$code"
}
trap finish EXIT

die() { # <code> <message>
  echo "windows-build-r2 FAILED [$STEP]: $2" >&2
  jset "$RUN_DIR" "steps.$STEP.exit" "$1"
  jset_str "$RUN_DIR" "steps.$STEP.error" "$2"
  exit "$1"
}

step_ok() { # <step>
  jset "$RUN_DIR" "steps.$1.exit" 0
}

echo "windows-build-r2 pipeline $RUN_ID (commit $SHORT, dirty=$DIRTY)"

# 1. Isolated snapshot. The live tree is only READ (rsync source).
STEP="snapshot"
if ! rsync -a --exclude-from="$SKILL_DIR/scripts/snapshot-excludes.txt" "$LIVE_ROOT/" "$SNAP/" >"$LOGS/snapshot.log" 2>&1; then
  die 2 "rsync snapshot failed; see logs/snapshot.log"
fi
[ -f "$SNAP/Tools/package_windows_build.py" ] || die 2 "snapshot missing Tools/pipeline"
[ -f "$SNAP/ProjectSettings/EditorBuildSettings.asset" ] || die 2 "snapshot missing ProjectSettings"
[ -n "$(ls "$SNAP"/Assets/_LayarTerakhir/assets/LayarTerakhir/*.vrm 2>/dev/null | head -n 1)" ] \
  || die 2 "snapshot lost git-ignored .vrm build inputs"
[ -d "$SNAP/Assets/_LayarTerakhir/assets/Art/Phase2/Imported/Madduck" ] \
  || die 2 "snapshot lost git-ignored Madduck build inputs"
jset_str "$RUN_DIR" steps.snapshot.files "$(grep -c . "$LOGS/snapshot.log" 2>/dev/null || echo 0)"
step_ok snapshot

# Snapshot has no .git (excluded); point git at the live repo for provenance only.
export GIT_DIR="$LIVE_ROOT/.git"

# 2. Pre-build source record: proves the GIT_DIR trick and freezes pre-build identity.
STEP="record-prebuild"
if ! (cd "$SNAP" && python3 Tools/record_designer_sources.py --output Audit/winbuild-prebuild-sources.json >"$LOGS/record-prebuild.log" 2>&1); then
  die 2 "pre-build source record failed; see logs/record-prebuild.log"
fi
step_ok record-prebuild

# 3. Unity Windows build via the existing pipeline (fresh Library import inside snapshot).
STEP="unity-build"
build_code=0
(cd "$SNAP" && python3 Tools/run_golden_checks.py --stage windows >"$LOGS/stage-windows.log" 2>&1) || build_code=$?
for f in windows-build-invocation.json windows-development.log windows-development.console.log windows-development-build-report.json; do
  [ -f "$SNAP/Builds/Logs/$f" ] && cp "$SNAP/Builds/Logs/$f" "$LOGS/" || true
done
python3 - "$RUN_DIR" <<'EOF' || true
import json,sys
run = sys.argv[1]
try:
    rep = json.load(open(run + "/logs/windows-development-build-report.json"))
    st = json.load(open(run + "/status.json"))
    st["build"] = {k: rep.get(k) for k in
        ("result", "target", "scriptingBackend", "options", "totalErrors",
         "totalWarnings", "totalSizeBytes", "durationSeconds")}
    st["build"]["sceneCount"] = len(rep.get("scenes", []))
    open(run + "/status.json", "w").write(json.dumps(st, indent=2))
except Exception as e:
    print("build report parse skipped:", e)
EOF
[ "$build_code" -eq 0 ] || die 2 "Unity Windows build failed (exit $build_code); see logs/stage-windows.log and logs/windows-development.log"
step_ok unity-build

# 4. Post-build source record: the build stamps PlayerSettings, so the
# provenance freeze happens here, exactly as in the last verified release.
STEP="record-sources"
if ! (cd "$SNAP" && python3 Tools/record_designer_sources.py >"$LOGS/record.log" 2>&1); then
  die 2 "source record failed; see logs/record.log"
fi
step_ok record-sources

# 5. Package + ZIP validation (asserts runtime markers, scene order, CRC).
STEP="package"
if ! (cd "$SNAP" && python3 Tools/package_windows_build.py >"$LOGS/package.log" 2>&1); then
  die 2 "packaging/ZIP validation failed; see logs/package.log"
fi
[ -f "$SNAP/Builds/$ZIP_NAME" ] || die 2 "packager reported success but ZIP is missing"
cp "$SNAP/Builds/$ZIP_NAME" "$SNAP/Builds/$ZIP_NAME.sha256" "$RESULT/"
python3 - "$RUN_DIR" "$RESULT/$ZIP_NAME" <<'EOF'
import hashlib,json,sys
run, arc = sys.argv[1], sys.argv[2]
st = json.load(open(run + "/status.json"))
h = hashlib.sha256()
with open(arc, "rb") as f:
    for b in iter(lambda: f.read(1024 * 1024), b""): h.update(b)
import os
st["archive"] = {"name": os.path.basename(arc), "sizeBytes": os.path.getsize(arc), "sha256": h.hexdigest()}
open(run + "/status.json", "w").write(json.dumps(st, indent=2))
EOF
step_ok package

# 6. Upload (skipped on request, blocked on existing release without --overwrite-r2).
STEP="upload"
if [ "$SKIP_UPLOAD" -eq 1 ]; then
  jset "$RUN_DIR" upload.status '"skipped"'
  step_ok upload
else
  guard=0; r2_remote_exists || guard=$?
  if [ "$guard" -eq 0 ]; then
    if [ "$OVERWRITE_R2" -eq 1 ]; then
      echo "Remote release exists; overwrite explicitly allowed (--overwrite-r2)."
    else
      jset "$RUN_DIR" upload.status '"blocked-existing"'
      die 3 "R2 release $ZIP_NAME already exists; rerun start.sh with --overwrite-r2 to replace it intentionally. Local ZIP preserved in result/."
    fi
  elif [ "$guard" -ne 1 ]; then
    jset "$RUN_DIR" upload.status '"failed"'
    die 3 "rclone pre-upload check failed; verify rclone/r2 access and retry."
  fi
  if ! (cd "$SNAP" && python3 Tools/upload_windows_build.py >"$LOGS/upload.log" 2>&1); then
    jset "$RUN_DIR" upload.status '"failed"'
    die 3 "R2 upload/readback verification failed; see logs/upload.log"
  fi
  python3 - "$RUN_DIR" <<'EOF' || true
import json,sys
run = sys.argv[1]
rec = json.load(open(run + "/snapshot/Audit/r2-windows-build-upload.json"))
st = json.load(open(run + "/status.json"))
st["upload"] = {"status": "verified", "remote": rec["remote"],
                "sizeBytes": rec["sizeBytes"], "sha256": rec["sha256"],
                "verifiedUtc": rec["verifiedUtc"]}
open(run + "/status.json", "w").write(json.dumps(st, indent=2))
EOF
  step_ok upload
fi

# 7. Receipts beside the ZIP so collect/clean stay safe.
for f in designer-build-sources.json golden-build-summary.json atmaja-build-summary.json \
         golden-windows-invocation.json atmaja-windows-invocation.json r2-windows-build-upload.json; do
  [ -f "$SNAP/Audit/$f" ] && cp "$SNAP/Audit/$f" "$RESULT/receipts/" || true
done
for f in windows-build-verification.json windows-build-files.sha256 windows-development-build-report.json windows-build-invocation.json; do
  [ -f "$SNAP/Builds/Logs/$f" ] && cp "$SNAP/Builds/Logs/$f" "$RESULT/receipts/" || true
done

echo "windows-build-r2 pipeline $RUN_ID complete."
