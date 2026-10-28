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

SNAP="$WORKSPACE"
LOGS="$RUN_DIR/logs"
RESULT="$RUN_DIR/result"
mkdir -p "$LOGS" "$RESULT/receipts" "$WORKSPACE" "$LASTGOOD"
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

echo "windows-build-r2 pipeline $RUN_ID (commit $SHORT, dirty=$DIRTY, mode=${BUILD_MODE:-full})"

# 1. Persistent incremental workspace. Synced from the live tree every run;
# the workspace keeps its own Unity Library/Bee cache and previous player
# output across runs. Caches are additive-only here — never deleted.
STEP="workspace"
if ! rsync -a --delete --exclude-from="$SKILL_DIR/scripts/workspace-excludes.txt" "$LIVE_ROOT/" "$WORKSPACE/" >"$LOGS/sync.log" 2>&1; then
  die 2 "workspace sync failed; see logs/sync.log"
fi
if [ -d "$LIVE_ROOT/Library" ]; then
  if ! rsync -a "$LIVE_ROOT/Library/" "$WORKSPACE/Library/" >"$LOGS/library-sync.log" 2>&1; then
    die 2 "workspace Library sync failed; see logs/library-sync.log"
  fi
fi
[ -f "$WORKSPACE/Tools/package_windows_build.py" ] || die 2 "workspace missing Tools/pipeline"
[ -f "$WORKSPACE/ProjectSettings/EditorBuildSettings.asset" ] || die 2 "workspace missing ProjectSettings"
[ -n "$(ls "$WORKSPACE"/Assets/_LayarTerakhir/assets/LayarTerakhir/*.vrm 2>/dev/null | head -n 1)" ] \
  || die 2 "workspace lost git-ignored .vrm build inputs"
[ -d "$WORKSPACE/Assets/_LayarTerakhir/assets/Art/Phase2/Imported/Madduck" ] \
  || die 2 "workspace lost git-ignored Madduck build inputs"
jset_str "$RUN_DIR" steps.workspace.files "$(grep -c . "$LOGS/sync.log" 2>/dev/null || echo 0)"
step_ok workspace

# Workspace has no .git (excluded); point git at the live repo for provenance only.
export GIT_DIR="$LIVE_ROOT/.git"

# 2. Pre-build source record: proves the GIT_DIR trick and freezes pre-build identity.
STEP="record-prebuild"
if ! (cd "$SNAP" && python3 Tools/record_designer_sources.py --output Audit/winbuild-prebuild-sources.json >"$LOGS/record-prebuild.log" 2>&1); then
  die 2 "pre-build source record failed; see logs/record-prebuild.log"
fi
step_ok record-prebuild

# 3. Unity Windows build via the existing pipeline.
# The workspace Library/Bee cache and previous player output persist across
# runs, so incremental (and scripts-only) builds stay fast.
# Safety: WINDOWS_KEEP_PREVIOUS=1 keeps the previous player in place as the
# scripts-only base (the run_golden_checks move-aside is skipped); the
# last-good copy below is the real protection, restored if a build fails.
STEP="unity-build"
export WINDOWS_KEEP_PREVIOUS=1
PLAYER_EXE="$WORKSPACE/Builds/Windows/LayarTerakhir.exe"
LASTGOOD_EXE="$LASTGOOD/Builds/Windows/LayarTerakhir.exe"
EFFECTIVE_MODE="${BUILD_MODE:-full}"
if [ "$EFFECTIVE_MODE" = "scripts-only" ] && [ ! -f "$PLAYER_EXE" ]; then
  if [ -f "$LASTGOOD_EXE" ]; then
    echo "workspace player base missing; restoring from last-good"
    mkdir -p "$WORKSPACE/Builds/Windows"
    rsync -a --delete "$LASTGOOD/Builds/Windows/" "$WORKSPACE/Builds/Windows/" >"$LOGS/lastgood-restore.log" 2>&1 \
      || die 2 "last-good restore failed; see logs/lastgood-restore.log"
  else
    echo "no previous player output anywhere; falling back to full build"
    EFFECTIVE_MODE="full"
    jset_str "$RUN_DIR" build.planFallback "no player base for scripts-only"
  fi
fi
attempt_build() { # <mode: scripts-only|full>
  if [ "$1" = "scripts-only" ]; then export WINDOWS_BUILD_SCRIPTS_ONLY=1
  else unset WINDOWS_BUILD_SCRIPTS_ONLY; fi
  (cd "$WORKSPACE" && python3 Tools/run_golden_checks.py --stage windows >"$LOGS/stage-windows-$1.log" 2>&1)
}
build_code=0; FALLBACK=0
if [ "$EFFECTIVE_MODE" = "scripts-only" ]; then
  attempt_build scripts-only || build_code=$?
  cp "$LOGS/stage-windows-scripts-only.log" "$LOGS/stage-windows.log"
  if [ "$build_code" -ne 0 ]; then
    echo "scripts-only attempt failed (exit $build_code); falling back to full incremental build (cache kept)"
    FALLBACK=1; build_code=0
    attempt_build full || build_code=$?
    cp "$LOGS/stage-windows-full.log" "$LOGS/stage-windows.log"
  fi
else
  attempt_build full || build_code=$?
  cp "$LOGS/stage-windows-full.log" "$LOGS/stage-windows.log"
fi
for f in windows-build-invocation.json windows-development.log windows-development.console.log windows-development-build-report.json; do
  [ -f "$WORKSPACE/Builds/Logs/$f" ] && cp "$WORKSPACE/Builds/Logs/$f" "$LOGS/" || true
done
FINAL_MODE="$EFFECTIVE_MODE"
if [ "$FALLBACK" -eq 1 ]; then FINAL_MODE="full"; fi
python3 - "$RUN_DIR" "$FINAL_MODE" "$FALLBACK" <<'EOF' || true
import json,sys
run, mode, fallback = sys.argv[1], sys.argv[2], sys.argv[3] == "1"
try:
    rep = json.load(open(run + "/logs/windows-development-build-report.json"))
    st = json.load(open(run + "/status.json"))
    st["build"] = {k: rep.get(k) for k in
        ("result", "target", "scriptingBackend", "options", "totalErrors",
         "totalWarnings", "totalSizeBytes", "durationSeconds")}
    st["build"]["sceneCount"] = len(rep.get("scenes", []))
    st["build"]["mode"] = mode
    st["build"]["fallback"] = fallback
    st["build"]["cacheReused"] = True
    st["build"]["unityVersion"] = rep.get("unityVersion", "")
    # The report must show the mechanism that actually ran.
    if mode == "scripts-only" and "ScriptsOnly" not in str(st["build"]["options"]):
        st["build"]["mode"] = "full"
        st["build"]["note"] = "report shows full options; treated as full build"
    open(run + "/status.json", "w").write(json.dumps(st, indent=2))
except Exception as e:
    print("build report parse skipped:", e)
EOF
FINAL_MODE="$(jget "$RUN_DIR" build.mode)"; [ -n "$FINAL_MODE" ] || FINAL_MODE="$EFFECTIVE_MODE"
if [ "$build_code" -ne 0 ]; then
  if [ -f "$LASTGOOD_EXE" ]; then
    echo "build failed; restoring workspace player base from last-good"
    rsync -a --delete "$LASTGOOD/Builds/Windows/" "$WORKSPACE/Builds/Windows/" >"$LOGS/lastgood-restore.log" 2>&1 || true
  fi
  [ "$FALLBACK" -eq 1 ] && extra=" (after scripts-only -> full fallback)" || extra=""
  die 2 "Unity Windows build failed$extra (exit $build_code); see logs/stage-windows.log and logs/windows-development.log. Last-good output preserved."
fi
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
# WINDOWS_KEEP_PREVIOUS retains the previous run's archive in the persistent
# workspace, but the packager refuses to overwrite an existing archive. Move
# the stale files into this run dir first (the authoritative copy already
# lives in the previous run's result/).
if [ -e "$WORKSPACE/Builds/$ZIP_NAME" ] || [ -e "$WORKSPACE/Builds/$ZIP_NAME.sha256" ]; then
  mkdir -p "$RUN_DIR/stale-archive"
  mv -f "$WORKSPACE/Builds/$ZIP_NAME" "$WORKSPACE/Builds/$ZIP_NAME.sha256" "$RUN_DIR/stale-archive/" 2>/dev/null || true
  echo "moved stale workspace archive to $RUN_DIR/stale-archive"
fi
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

# 5b. The build AND package both succeeded: refresh the last-good player
# copy and its metadata. This is the ONLY write to last-good, so a failed
# build can never overwrite a working player.
mkdir -p "$LASTGOOD/Builds/Windows"
if ! rsync -a --delete "$WORKSPACE/Builds/Windows/" "$LASTGOOD/Builds/Windows/" >"$LOGS/lastgood-refresh.log" 2>&1; then
  die 2 "last-good refresh failed; see logs/lastgood-refresh.log"
fi
python3 - "$RUN_DIR" "$LASTGOOD_META" <<'EOF' || true
import datetime,json,sys
run, meta = sys.argv[1], sys.argv[2]
st = json.load(open(run + "/status.json"))
bld = st.get("build", {})
open(meta, "w").write(json.dumps({
    "commit": st.get("sourceCommit", ""),
    "dirty": bool(st.get("sourceDirty")),
    "runId": st.get("runId", ""),
    "utc": datetime.datetime.now(datetime.timezone.utc).isoformat(),
    "mode": bld.get("mode", ""),
    "unityVersion": bld.get("unityVersion", ""),
}, indent=2) + "\n")
EOF
[ -f "$LASTGOOD_META" ] || die 2 "last-good metadata was not written"

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

# 7. Receipts beside the ZIP so collect/clean stay safe.
for f in designer-build-sources.json golden-build-summary.json atmaja-build-summary.json \
         golden-windows-invocation.json atmaja-windows-invocation.json r2-windows-build-upload.json; do
  [ -f "$SNAP/Audit/$f" ] && cp "$SNAP/Audit/$f" "$RESULT/receipts/" || true
done
for f in windows-build-verification.json windows-build-files.sha256 windows-development-build-report.json windows-build-invocation.json; do
  [ -f "$SNAP/Builds/Logs/$f" ] && cp "$SNAP/Builds/Logs/$f" "$RESULT/receipts/" || true
done

echo "windows-build-r2 pipeline $RUN_ID complete."
