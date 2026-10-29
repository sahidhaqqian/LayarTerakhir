"""Blender 4 import of selected CC0 props. Run after fetch_prolog_revision_assets.py.

The evidence cup is authored from scratch by author_cracked_porcelain_cup.py.
"""
from pathlib import Path
import json
import hashlib
import math
import shutil
import bpy
from mathutils import Vector, Matrix

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/PrologRevision'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/PrologRevision'


def normalize(objects, size):
    points = [o.matrix_world @ Vector(v) for o in objects for v in o.bound_box]
    low = Vector(tuple(min(p[i] for p in points) for i in range(3)))
    high = Vector(tuple(max(p[i] for p in points) for i in range(3)))
    scale = Vector(tuple(size[i] / (high[i]-low[i]) for i in range(3)))
    centre = (low + high) * .5
    centre.z = low.z
    for o in objects:
        matrix = o.matrix_world.copy()
        for v in o.data.vertices:
            p = matrix @ v.co - centre
            v.co = Vector(tuple(p[i]*scale[i] for i in range(3)))
        o.matrix_world = Matrix.Identity(4)


def export(name, objects):
    bpy.ops.object.select_all(action='DESELECT')
    for o in objects:
        o.hide_set(False)
        o.hide_viewport = False
        o.hide_render = False
        o.select_set(True)
    bpy.context.view_layer.objects.active = objects[0]
    bpy.ops.export_scene.fbx(filepath=str(OUT / 'Models' / (name + '.fbx')),
                             use_selection=True, object_types={'MESH'}, bake_anim=False,
                             axis_forward='-Z', axis_up='Y', path_mode='STRIP')


def main():
    (OUT / 'Models').mkdir(parents=True, exist_ok=True)
    (OUT / 'Textures').mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.read_factory_settings(use_empty=True)
    tub_folder = CACHE / 'BathTube/BathTube'
    bpy.ops.wm.obj_import(filepath=str(tub_folder / 'BathTube.obj'), forward_axis='NEGATIVE_Y', up_axis='Z')
    tub = [o for o in bpy.context.scene.objects if o.type == 'MESH']
    for o in tub:
        o.matrix_world = Matrix.Rotation(-math.pi/2, 4, "X") @ o.matrix_world
    bpy.context.view_layer.update()
    normalize(tub, (1.8, .88, .64))
    export('ClawfootBathtub', tub)
    for name in ['BathTubeP1.jpg', 'BathTubeP2-3.jpg']:
        shutil.copy2(tub_folder / name, OUT / 'Textures' / name)

    bpy.ops.wm.open_mainfile(filepath=str(CACHE / 'ladder3_0.blend'))
    source = bpy.data.objects['ladder_mid']
    # Six original ladder segments, retaining regular rungs and human scale.
    source.data = source.data.copy()
    normalize([source], (.66, .105, 1.1))
    ladder = [source]
    for n in range(1,6):
        o = source.copy();o.data=source.data.copy();bpy.context.collection.objects.link(o)
        o.location.z = n*1.1
        ladder.append(o)
    export('CellarAccessLadder', ladder)

    # The designer requested an original Blender cup instead of a downloaded mug.
    import sys
    sys.path.insert(0, str(ROOT / 'Tools'))
    from author_cracked_porcelain_cup import build_cup
    build_cup(previews=False)
    outputs=[]
    for path in sorted(OUT.rglob('*')):
        if path.is_file() and path.suffix != '.meta':
            outputs.append(dict(file=path.relative_to(ROOT).as_posix(), bytes=path.stat().st_size,
                                sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    (ROOT / 'Audit/prolog-revision-derived-assets.json').write_text(json.dumps(outputs,indent=2)+'\n')
    print('PROLOG_REVISION_ASSETS_PREPARED',len(outputs))


if __name__ == '__main__':
    main()
