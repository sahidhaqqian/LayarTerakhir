#!/usr/bin/env bash
# Shared paths and helpers for the windows-build-r2 skill.
# Source this file; do not execute it directly.
set -euo pipefail

SKILL_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
LIVE_ROOT="$(git -C "$SKILL_DIR" rev-parse --show-toplevel 2>/dev/null || echo "")"
if [ -z "$LIVE_ROOT" ]; then
  echo "windows-build-r2: skill is not inside a git worktree" >&2
  exit 1
fi

# All run state lives OUTSIDE the live project so the build never touches it.
# WINBUILD_BASE / WINBUILD_SESSION overrides exist for isolated testing only.
BASE="${WINBUILD_BASE:-/tmp/opencode/windows-build-r2}"
SESSION="${WINBUILD_SESSION:-lt-winbuild}"
# Persistent incremental workspace: synced from the live tree every run,
# keeping its own Unity Library/Bee cache and previous player output so
# scripts-only and incremental builds stay fast. Never deleted by the skill.
WORKSPACE="$BASE/workspace"
# Last known-good player output + build metadata. Refreshed only after a
# successful build AND package; used to restore the workspace base if a
# build fails, and to decide scripts-only eligibility for the next run.
LASTGOOD="$BASE/last-good"
LASTGOOD_META="$LASTGOOD/meta.json"
R2DIR="r2:layarterakhir/Layarterakhir/"
ZIP_NAME="LayarTerakhir-Windows-x86_64-Development.zip"
UNITY_BIN="${UNITY_EDITOR:-/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity}"
# Workspace (~18 GB Library seed + ~5 GB player output + work files) plus
# per-run results (logs + ~3 GB ZIP). The workspace Library/Bee cache and
# last-good output are build caches: this skill never deletes them.
MIN_FREE_BYTES=$((50 * 1024 * 1024 * 1024))

utc_now() { date -u +%Y-%m-%dT%H:%M:%SZ; }
run_dir() { echo "$BASE/runs/$1"; }
active_file() { echo "$BASE/active-run"; }

have() { command -v "$1" >/dev/null 2>&1; }
tmux_alive() { tmux has-session -t "$SESSION" 2>/dev/null; }

latest_run() {
  if [ -d "$BASE/runs" ]; then ls -1 "$BASE/runs" 2>/dev/null | sort | tail -n 1; fi
}

resolve_run() {
  if [ -n "${1:-}" ]; then echo "$1"; return; fi
  if [ -f "$(active_file)" ]; then cat "$(active_file)"; return; fi
  latest_run
}

# jset <run_dir> <dotted.key> <json_value> — merge one value into status.json.
jset() {
  python3 - "$1" "$2" "$3" <<'EOF'
import json,sys
path = sys.argv[1] + "/status.json"
with open(path) as f: st = json.load(f)
val = json.loads(sys.argv[3])
node = st
parts = sys.argv[2].split(".")
for k in parts[:-1]:
    if k not in node or not isinstance(node[k], dict): node[k] = {}
    node = node[k]
node[parts[-1]] = val
tmp = path + ".tmp"
with open(tmp, "w") as f: json.dump(st, f, indent=2)
import os
os.replace(tmp, path)
EOF
}

# jset_str <run_dir> <dotted.key> <raw_string>
jset_str() {
  local enc
  enc=$(printf '%s' "$3" | python3 -c 'import json,sys; print(json.dumps(sys.stdin.read()))')
  jset "$1" "$2" "$enc"
}

# jget <run_dir> <dotted.key> — print scalar/JSON, or empty string.
jget() {
  python3 - "$1" "$2" <<'EOF'
import json,sys
try:
    with open(sys.argv[1] + "/status.json") as f: st = json.load(f)
    node = st
    for k in sys.argv[2].split("."): node = node[k]
    if isinstance(node, (dict, list)): print(json.dumps(node))
    elif node is None: print("")
    else: print(node)
except Exception:
    print("")
EOF
}

# r2_remote_exists — return 0 if the release ZIP already exists on R2,
# 1 if absent, 2 on rclone/network failure. Read-only; never uploads.
r2_remote_exists() {
  local out err
  err="$(mktemp)"
  if ! out=$(rclone lsjson "$R2DIR" --files-only --no-mimetype 2>"$err"); then
    cat "$err" >&2; rm -f "$err"; return 2
  fi
  rm -f "$err"
  ZIP_NAME="$ZIP_NAME" python3 -c \
    'import json,os,sys; sys.exit(0 if any(e.get("Name") == os.environ["ZIP_NAME"] for e in json.loads(sys.stdin.read() or "[]")) else 1)' \
    <<<"$out"
}
