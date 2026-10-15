"""Measure the real imported bounds of every downloaded FBX and record them in
manifest/measured-bounds.json.

Run headless:
    blender -b -P AssetResearch/tools/measure_bounds.py

The API 'dimensions' field is millimetres in the source scene arrangement;
this measurement confirms what Unity will import, including scatter-style sets
whose pieces are laid out separately in the source file.
"""
import json
from pathlib import Path

import bpy
from mathutils import Vector

REPO = Path(__file__).resolve().parents[2]
ASSETS = REPO / 'AssetResearch' / 'assets'
OUT = REPO / 'AssetResearch' / 'manifest' / 'measured-bounds.json'


def measure(fbx: Path):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(fbx), use_anim=False)
    low = Vector((1e9,) * 3)
    high = Vector((-1e9,) * 3)
    for obj in bpy.data.objects:
        if obj.type != 'MESH':
            continue
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            low = Vector(map(min, low, point))
            high = Vector(map(max, high, point))
    return [round(value, 4) for value in (high - low)]


result = {}
for folder in sorted(path for path in ASSETS.iterdir() if path.is_dir()):
    fbx = next(folder.glob('*.fbx'))
    result[folder.name] = measure(fbx)
    print('BOUNDS', folder.name, result[folder.name], flush=True)
OUT.write_text(json.dumps(result, indent=2) + '\n')
print('wrote', OUT)
