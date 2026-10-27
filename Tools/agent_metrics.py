"""Track agent, validation, and build durations to identify bottlenecks.

Every entry is one measured wall-clock duration, never an estimate.
Usage:
  python3 Tools/agent_metrics.py log --kind <kind> --label <label> --seconds <s> [--detail ...]
  python3 Tools/agent_metrics.py backfill   # import existing Audit invocation receipts once
  python3 Tools/agent_metrics.py report      # bottleneck summary from Audit/agent-metrics.jsonl

kinds: validation | test | build | upload | explore | implement
"""
import argparse
import glob
import json
import os
import sys
from datetime import datetime, timezone

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
STORE = os.path.join(ROOT, "Audit", "agent-metrics.jsonl")


def utcnow():
    return datetime.now(timezone.utc).isoformat()


def append(entry):
    entry["utc"] = utcnow()
    with open(STORE, "a", encoding="utf-8") as f:
        f.write(json.dumps(entry, ensure_ascii=False) + "\n")
    print("metrics logged: %s %s %ss" % (entry["kind"], entry["label"], entry["seconds"]))


def cmd_log(args):
    append({
        "kind": args.kind,
        "label": args.label,
        "seconds": float(args.seconds),
        "detail": args.detail or "",
        "source": "agent",
    })


def existing_keys():
    keys = set()
    if not os.path.exists(STORE):
        return keys
    with open(STORE, encoding="utf-8") as f:
        for line in f:
            try:
                e = json.loads(line)
                keys.add((e.get("source"), e.get("label"), e.get("seconds")))
            except ValueError:
                pass
    return keys


def cmd_backfill(_args):
    seen = existing_keys()
    added = 0
    for path in sorted(glob.glob(os.path.join(ROOT, "Audit", "*invocation*.json"))):
        try:
            with open(path, encoding="utf-8") as f:
                data = json.load(f)
        except (ValueError, OSError):
            continue
        started, ended = data.get("startedUtc"), data.get("endedUtc")
        if not started or not ended or ended in (None, "Running"):
            continue
        try:
            seconds = (datetime.fromisoformat(ended) - datetime.fromisoformat(started)).total_seconds()
        except ValueError:
            continue
        if seconds <= 0:
            continue
        stage = data.get("stage", "?")
        kind = {"test": "test", "player": "test", "windows": "build", "linux": "build"}.get(stage, "validation")
        entry = {"kind": kind, "label": stage + ":" + os.path.basename(path),
                 "seconds": round(seconds, 1), "detail": "", "source": "receipt"}
        if (entry["source"], entry["label"], entry["seconds"]) in seen:
            continue
        append(entry)
        seen.add((entry["source"], entry["label"], entry["seconds"]))
        added += 1
    print("backfill added %d entries" % added)


def load():
    entries = []
    if not os.path.exists(STORE):
        return entries
    with open(STORE, encoding="utf-8") as f:
        for line in f:
            try:
                entries.append(json.loads(line))
            except ValueError:
                pass
    return entries


def cmd_report(_args):
    entries = load()
    if not entries:
        print("no metrics yet")
        return
    by_kind = {}
    for e in entries:
        by_kind.setdefault(e.get("kind", "?"), []).append(float(e.get("seconds", 0)))
    print("kind            n      total      avg      max")
    for kind in sorted(by_kind):
        vals = by_kind[kind]
        print("%-15s %4d %8ds %8ds %8ds" % (kind, len(vals), sum(vals), sum(vals) / len(vals), max(vals)))
    print("\nslowest 10 runs:")
    for e in sorted(entries, key=lambda x: -float(x.get("seconds", 0)))[:10]:
        print("  %6ds  %-10s  %s" % (e.get("seconds", 0), e.get("kind"), e.get("label")))
    print("\nbaseline: %d measured runs in %s (wall-clock, no estimates)" % (len(entries), STORE))


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    sub = parser.add_subparsers(dest="command", required=True)
    p_log = sub.add_parser("log")
    p_log.add_argument("--kind", required=True,
                       choices=["validation", "test", "build", "upload", "explore", "implement"])
    p_log.add_argument("--label", required=True)
    p_log.add_argument("--seconds", required=True)
    p_log.add_argument("--detail", default="")
    p_log.set_defaults(func=cmd_log)
    p_back = sub.add_parser("backfill")
    p_back.set_defaults(func=cmd_backfill)
    p_rep = sub.add_parser("report")
    p_rep.set_defaults(func=cmd_report)
    args = parser.parse_args()
    args.func(args)


if __name__ == "__main__":
    sys.exit(main())
