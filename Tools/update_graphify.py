"""Refresh the local code-only graph and reject media, stale or partial output."""
from __future__ import annotations

import argparse
import hashlib
import json
import os
from pathlib import Path, PurePosixPath
import shutil
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
CODE_ROOTS = {
    "Assets/_LayarTerakhir/src": {".cs"},
    "Tools": {".py", ".sh"},
}
ARTIFACTS = ("graph.json", "GRAPH_REPORT.md", "graph.html")


def digest(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def allowed_source(source: str) -> bool:
    # Source references must remain portable, relative and inside the allowlist.
    path = PurePosixPath(source)
    if not source or "\\" in source or path.is_absolute() or ".." in path.parts:
        return False
    return any(
        path.is_relative_to(base) and path.suffix in suffixes
        and "__pycache__" not in path.parts
        for base, suffixes in CODE_ROOTS.items()
    )


def snapshot(root: Path) -> dict[str, str]:
    sources = {}
    for base, suffixes in CODE_ROOTS.items():
        directory = root / base
        if directory.is_symlink():
            raise ValueError(f"Symlink source directory is not allowed: {base}")
        for path in sorted(directory.rglob("*")):
            if path.suffix not in suffixes or "__pycache__" in path.parts:
                continue
            relative = path.relative_to(root).as_posix()
            if path.is_symlink() or any(p.is_symlink() for p in path.parents if p != root):
                raise ValueError(f"Symlink source is not allowed: {relative}")
            if path.is_file():
                sources[relative] = digest(path)
    if not sources:
        raise ValueError("No first-party source code found")
    return dict(sorted(sources.items()))


def validate_graph(graph: dict, sources: dict[str, str]) -> set[str]:
    if not graph.get("nodes") or not graph.get("links", graph.get("edges")):
        raise ValueError("Graph is empty or incomplete")
    referenced = set()
    for row in graph["nodes"] + graph.get("links", graph.get("edges", [])):
        if row.get("_origin") not in (None, "ast"):
            raise ValueError("Non-local/semantic extraction found in graph")
        kind = row.get("file_type", "code")
        # Graphify also extracts rationale/concepts from source-code comments.
        # Those are safe only when AST-derived and anchored to allowed code.
        if kind not in {"code", "concept", "rationale"}:
            raise ValueError("Non-code node/edge found in graph")
        source = row.get("source_file", "")
        if not isinstance(source, str):
            raise ValueError("Invalid source reference in graph")
        external = kind == "concept" and row.get("external") is True and row.get("type") == "external" and not source
        if kind != "code" and not external and (row.get("_origin") != "ast" or not source):
            raise ValueError("Non-AST concept/rationale found in graph")
        # Unresolved external symbols (UnityEngine, MenuItem, etc.) have no file.
        if source:
            if not allowed_source(source) or source not in sources:
                raise ValueError(f"Forbidden or stale graph source: {source}")
            referenced.add(source)
    return referenced


def read_json(path: Path) -> dict:
    return json.loads(path.read_text(encoding="utf-8"))


def check(root: Path = ROOT) -> dict:
    out = root / "graphify-out"
    receipt = read_json(out / "source-manifest.json")
    sources = snapshot(root)
    if receipt["sources_sha256"] != sources:
        raise ValueError("Code changed since Graphify ran; regenerate before commit/push")
    if receipt["ignore_sha256"] != digest(root / ".graphifyignore"):
        raise ValueError("Graphify ignore rules changed; regenerate the graph")
    for name in ARTIFACTS:
        if receipt["artifacts_sha256"][name] != digest(out / name):
            raise ValueError(f"Graphify artifact changed or stale: {name}")
    validate_graph(read_json(out / "graph.json"), sources)
    return receipt


def update(root: Path = ROOT) -> dict:
    binary = shutil.which("graphify")
    if not binary:
        candidate = Path.home() / ".local/bin/graphify"
        binary = str(candidate) if candidate.is_file() else None
    if not binary:
        raise ValueError("Graphify is not installed; follow Tools/GRAPHIFY.md")
    env = os.environ.copy()
    env.pop("GRAPHIFY_OUT", None)  # Always write to this worktree's graphify-out/.
    env["GRAPHIFY_NO_AUTO_REFRESH"] = "1"
    env["GRAPHIFY_NO_TIPS"] = "1"
    version = subprocess.check_output([binary, "--version"], env=env, text=True).strip()
    expected = "graphify " + (root / "Tools/graphify-requirements.txt").read_text().split("==")[1].strip()
    if version != expected:
        raise ValueError(f"Expected {expected}; found {version}. Follow Tools/GRAPHIFY.md")

    sources = snapshot(root)
    out = root / "graphify-out"
    graph_path = out / "graph.json"
    if graph_path.exists():
        # Never merge an old semantic/media graph into our code-only graph.
        old = read_json(graph_path)
        previous_sources = {
            row["source_file"]: "" for row in old.get("nodes", []) + old.get("links", old.get("edges", []))
            if row.get("source_file")
        }
        validate_graph(old, previous_sources)
    for args in (
        ["extract", ".", "--code-only", "--max-workers", "2"],
        ["cluster-only", ".", "--no-label"],
    ):
        subprocess.run([binary, *args], cwd=root, env=env, check=True)

    if sources != snapshot(root):
        raise ValueError("Code changed while building the graph; rerun Graphify")
    graph = read_json(graph_path)
    referenced = validate_graph(graph, sources)
    manifest = read_json(out / "manifest.json")
    if set(manifest) != set(sources):
        raise ValueError("Graphify discovery does not match the code allowlist")
    for name, row in manifest.items():
        actual = hashlib.md5((root / name).read_bytes()).hexdigest()
        if row.get("ast_hash") != actual:
            raise ValueError(f"AST extraction failed or is stale: {name}")
    # Do not bake a temporary/shared-worktree folder name into the team report.
    report = out / "GRAPH_REPORT.md"
    report_text = report.read_text(encoding="utf-8")
    lines = report_text.splitlines()
    lines[0] = "# Graph Report - LayarTerakhir (local code only)"
    report_text = "\n".join(lines) + "\n"
    report_text = report_text.replace(
        "- Run `git rev-parse HEAD` and compare to check if the graph is stale.",
        "- Commit above is the pre-commit base, not a freshness check. Verify source/artifact hashes with `python3 Tools/update_graphify.py --check`.",
    ).replace(
        "- Run `graphify update .` after code changes (no API cost).",
        "- Regenerate safely with `python3 Tools/update_graphify.py` before commit/push.",
    )
    report.write_text(report_text, encoding="utf-8")
    receipt = {
        "graphify_version": version,
        "mode": "local-code-only; no LLM/community labeling",
        "asset_file_count": 0,
        "code_file_count": len(sources),
        "referenced_source_count": len(referenced),
        "node_count": len(graph["nodes"]),
        "edge_count": len(graph.get("links", graph.get("edges", []))),
        "ignore_sha256": digest(root / ".graphifyignore"),
        "sources_sha256": sources,
        "artifacts_sha256": {name: digest(out / name) for name in ARTIFACTS},
    }
    (out / "source-manifest.json").write_text(
        json.dumps(receipt, indent=2, sort_keys=True) + "\n", encoding="utf-8"
    )
    return check(root)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true", help="Reject stale/unsafe output without rebuilding")
    args = parser.parse_args()
    try:
        receipt = check() if args.check else update()
    except (OSError, ValueError, KeyError, subprocess.CalledProcessError) as exc:
        print(f"Graphify FAILED: {exc}", file=sys.stderr)
        return 1
    print(
        f"Graphify OK: {receipt['code_file_count']} code files, "
        f"{receipt['node_count']} nodes, {receipt['edge_count']} edges; 0 asset files."
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
