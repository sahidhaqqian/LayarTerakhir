#!/usr/bin/env bash
# Show partial Windows build status. Exit: 0 succeeded, 1 running, 2 failed/blocked/none.
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
# shellcheck disable=SC1091
. "$SCRIPT_DIR/../../windows-build-r2/scripts/common.sh"

usage() {
  cat <<'EOF'
Usage: status.sh [--run <id>] [--json] [--all]

  (no args)   Summary of the latest partial run.
  --json      Print the raw status.json of the latest partial run.
  --all       One-line table of every recorded run (full + partial).
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
  printf '%-34s %-10s %-9s %-14s %s\n' RUN STATUS UPLOAD SPEC SOURCE
  for d in "$BASE"/runs/*/; do
    [ -f "$d/status.json" ] || continue
    python3 - "$d/status.json" <<'EOF'
import json,sys
st = json.load(open(sys.argv[1]))
kind = "partial:" + st.get("buildTag", "?") if st.get("partial") else "full"
print("%-34s %-10s %-9s %-14s %s%s" % (st.get("runId", "?"), st.get("status", "?"),
      st.get("upload", {}).get("status", "?"), kind, st.get("sourceShort", "?"),
      "-dirty" if st.get("sourceDirty") else ""))
EOF
  done
  exit 0
fi

if [ -z "$WANT" ]; then
  WANT="$(for d in "$BASE"/runs/*/; do
    [ -f "$d/status.json" ] || continue
    python3 -c 'import json,sys; st=json.load(open(sys.argv[1])); print(st.get("runId","") if st.get("partial") else "")' "$d/status.json"
  done | sort | tail -n 1)"
fi
if [ -z "$WANT" ] || [ ! -f "$(run_dir "$WANT")/status.json" ]; then
  echo "windows-build-partial: no partial runs recorded yet."
  echo "  start: $SCRIPT_DIR/start.sh --scenes 1,2   (manual use only, on explicit user request)"
  exit 2
fi
RUN_DIR="$(run_dir "$WANT")"
if [ "$JSON" -eq 1 ]; then cat "$RUN_DIR/status.json"; echo; exit 0; fi
python3 - "$RUN_DIR/status.json" <<'EOF'
import json,sys
st = json.load(open(sys.argv[1]))
print("run      :", st.get("runId"), "[" + st.get("status", "?") + "]")
print("spec     :", st.get("sceneSpec"), " tag:", st.get("buildTag"))
print("source   :", st.get("sourceShort"), "(dirty)" if st.get("sourceDirty") else "(clean)")
print("plan     :", "full (partial builds always cook full assets)")
print("started  :", st.get("startedUtc"), "  finished:", st.get("finishedUtc"))
print("tmux live:", "yes" if __import__("subprocess").run(["tmux","has-session","-t","lt-winbuild"],capture_output=True).returncode==0 else "no")
print("steps    :")
for k,v in st.get("steps",{}).items(): print("  %-12s %s" % (k, "ok" if v.get("exit")==0 else v))
b = st.get("build", {})
print("build    :", b.get("result"), "mode=%s errors=%s warnings=%s scenes=%s" % (b.get("mode"), b.get("totalErrors"), b.get("totalWarnings"), b.get("sceneCount")))
a = st.get("archive", {})
if a: print("archive  :", a.get("name"), "bytes=%s sha256=%s..." % (a.get("sizeBytes"), str(a.get("sha256"))[:12]))
u = st.get("upload", {})
print("upload   :", u.get("status"), u.get("remote",""))
EOF
case "$(jget "$RUN_DIR" status)" in
  Succeeded) exit 0 ;;
  Running) exit 1 ;;
  *) exit 2 ;;
esac
