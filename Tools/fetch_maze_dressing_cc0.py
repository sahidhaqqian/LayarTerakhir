#!/usr/bin/env python3
"""Download realistic CC0 Poly Haven props dressing the Prolog basement maze.

Rooms (one slice at a time, see argv):
  R1 east archive : cardboard_box_01, vintage_suitcase, CheeseBox_01
  R2 south crypt  : brass_pot_02, ceramic_pot, ceramic_vase_02
  R3 flooded store: wooden_bucket_01, bleach_bottle, dustpan

Only the 1k FBX plus diffuse + tangent normal maps are fetched (roughness/
metal stays scalar in the authored URP materials). Provenance merges into
Audit/maze-dressing-cc0-sources.json keyed by asset id. No original artwork
is modeled.

Usage: python3 Tools/fetch_maze_dressing_cc0.py R1
       python3 Tools/fetch_maze_dressing_cc0.py R1 R2 R3
"""
import hashlib
import json
import sys
import urllib.request
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / "Builds/ArtSources/MazeDressing"
OUT = ROOT / "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven"
RECEIPT = ROOT / "Audit/maze-dressing-cc0-sources.json"
CACHE.mkdir(parents=True, exist_ok=True)

ROOMS = {
    "R1": ["cardboard_box_01", "vintage_suitcase", "cement_bag"],
    "R2": ["brass_pot_02", "ceramic_pot", "ceramic_vase_02"],
    "R3": ["wooden_bucket_01", "bleach_bottle", "dustpan"],
}

# asset id -> [(filename, kind)] ; kind drives the URP wiring later.
# URLs follow the dl.polyhaven.org layout (verified 206 for R1 set).
FILES = {
    "cardboard_box_01": ["cardboard_box_01_diff_1k.jpg", "cardboard_box_01_nor_gl_1k.exr"],
    "vintage_suitcase": ["vintage_suitcase_01_diff_1k.png", "vintage_suitcase_01_nor_gl_1k.png",
                         "vintage_suitcase_02_diff_1k.png", "vintage_suitcase_02_nor_gl_1k.png"],
    "CheeseBox_01": ["CheeseBox_01_diff_1k.jpg", "CheeseBox_01_nor_gl_1k.exr"],
    "cement_bag": ["cement_bag_diff_1k.jpg", "cement_bag_nor_gl_1k.exr"],
    "brass_pot_02": ["brass_pot_02_diff_1k.jpg", "brass_pot_02_nor_gl_1k.exr"],
    "ceramic_pot": ["ceramic_pot_diff_1k.jpg", "ceramic_pot_nor_gl_1k.exr"],
    "ceramic_vase_02": ["ceramic_vase_02_diff_1k.jpg", "ceramic_vase_02_nor_gl_1k.exr"],
    "wooden_bucket_01": ["wooden_bucket_01_diff_1k.jpg", "wooden_bucket_01_nor_gl_1k.exr"],
    "bleach_bottle": ["bleach_bottle_01_diff_1k.png", "bleach_bottle_01_nor_gl_1k.png"],
    "dustpan": ["dustpan_diff_1k.jpg", "dustpan_nor_gl_1k.exr"],
}

BASE = "https://dl.polyhaven.org/file/ph-assets/Models"


def get(url, dest):
    if dest.exists() and dest.stat().st_size > 0:
        return
    req = urllib.request.Request(url, headers={"User-Agent": "LayarTerakhir-CC0-fetch/1.0"})
    with urllib.request.urlopen(req, timeout=240) as r, open(dest, "wb") as f:
        f.write(r.read())


def subdir(fname):
    ext = fname.rsplit(".", 1)[1]
    return {"jpg": "jpg", "png": "png", "exr": "exr", "fbx": "fbx"}[ext]


def fetch_asset(aid):
    files = []
    outdir = OUT / aid
    outdir.mkdir(parents=True, exist_ok=True)
    jobs = [(f"{aid}_1k.fbx", f"{BASE}/fbx/1k/{aid}/{aid}_1k.fbx")]
    for fn in FILES[aid]:
        jobs.append((fn, f"{BASE}/{subdir(fn)}/1k/{aid}/{fn}"))
    for fname, url in jobs:
        cached = CACHE / fname
        get(url, cached)
        target = outdir / fname
        target.write_bytes(cached.read_bytes())
        files.append(dict(path=target.relative_to(ROOT).as_posix(), url=url,
                           sha256=hashlib.sha256(target.read_bytes()).hexdigest(),
                           bytes=target.stat().st_size))
    return dict(name=aid, page=f"https://polyhaven.com/a/{aid}",
                license="CC0-1.0", licenseUrl="https://polyhaven.com/license", files=files)


def main():
    rooms = sys.argv[1:] or ["R1"]
    assert all(r in ROOMS for r in rooms), f"rooms must be in {sorted(ROOMS)}"
    receipt = json.loads(RECEIPT.read_text()) if RECEIPT.exists() else {}
    total = 0
    for room in rooms:
        for aid in ROOMS[room]:
            rec = fetch_asset(aid)
            receipt[aid] = rec
            total += len(rec["files"])
            print(" ", room, aid, sum(f["bytes"] for f in rec["files"]), "bytes")
    RECEIPT.write_text(json.dumps(receipt, indent=1) + "\n")
    print("MAZE_DRESSING_SOURCES", total, "files for", ",".join(rooms))


if __name__ == "__main__":
    main()
