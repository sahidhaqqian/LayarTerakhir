"""Freeze/check the source identity used for the designer-review player builds.

This is build provenance, not a claim of manual Windows gameplay acceptance.
"""
import argparse
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import subprocess

ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / "Audit/designer-build-sources.json"


def snapshot():
    base = ROOT / "Assets/_LayarTerakhir"
    settings = [ROOT / "ProjectSettings" / name for name in
                ("ProjectSettings.asset", "GraphicsSettings.asset", "QualitySettings.asset", "EditorBuildSettings.asset")]
    # Build provenance includes meshes/textures/materials and importer metadata too.
    # This hashes local files only; Graphify's separate code-only allowlist is unchanged.
    files = sorted({p for p in base.rglob("*") if p.is_file()} |
                   {p for p in (ROOT / "Assets/Resources").rglob("*") if p.is_file()} |
                   set(settings) | {ROOT / "Packages/manifest.json", ROOT / "Packages/packages-lock.json"})
    return {p.relative_to(ROOT).as_posix(): hashlib.sha256(p.read_bytes()).hexdigest() for p in files}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true")
    parser.add_argument("--output", type=Path, default=OUTPUT)
    args = parser.parse_args()
    output = args.output if args.output.is_absolute() else ROOT / args.output
    assert output.resolve().is_relative_to((ROOT / "Audit").resolve())
    sources = snapshot()
    if args.check:
        saved = json.loads(output.read_text())
        assert saved["sources"] == sources, "Source files changed after the reviewed build snapshot"
        print(f"DESIGNER_SOURCES_VERIFIED {len(sources)} files")
        return
    head = subprocess.check_output(["git", "rev-parse", "HEAD"], cwd=ROOT, text=True).strip()
    output.write_text(json.dumps({"recordedUtc": datetime.now(timezone.utc).isoformat(),
        "baseCommit": head, "includesUncommittedDesignerChanges": True,
        "description": "First-party game directory and resources (code, scenes, art, audio and importer metadata), package locks and player settings frozen before player builds; local SHA-256 provenance only",
        "sources": sources}, indent=2) + "\n")
    print(f"DESIGNER_SOURCES_RECORDED {len(sources)} files")


if __name__ == "__main__":
    main()
