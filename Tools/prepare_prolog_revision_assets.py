"""Blender 4 import of selected CC0 props. Run after fetch_prolog_revision_assets.py.

Mug is an adaptation of rubberduck's cup, with a small chipped rim and hairline
fracture; it is not a photogrammetry scan of an already broken cup.
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

    bpy.ops.wm.open_mainfile(filepath=str(CACHE / 'Mugs/mugs-1.blend'))
    cup = bpy.data.objects['mug_2']
    cup.data = cup.data.copy()
    for modifier in list(cup.modifiers):
        cup.modifiers.remove(modifier)
    normalize([cup], (.124, .105, .102))
    # A millimetre-scale chip is part of the mesh silhouette, not giant black bars.
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=2, radius=1, location=(.013,-.046,.102))
    cutter = bpy.context.object;cutter.scale=(.004,.009,.006)
    bpy.context.view_layer.objects.active=cup
    chip=cup.modifiers.new('Chipped porcelain rim', 'BOOLEAN');chip.operation='DIFFERENCE';chip.object=cutter
    bpy.ops.object.modifier_apply(modifier=chip.name)
    bpy.data.objects.remove(cutter,do_unlink=True)
    # Follow the actual outer ceramic surface for a fine, uneven fracture.
    curve=bpy.data.curves.new('HairlineFracture','CURVE');curve.dimensions='3D';curve.bevel_depth=.00022;curve.bevel_resolution=1
    points=[]
    for x,z in [(.012,.100),(.011,.094),(.014,.086),(.013,.078),(.016,.069),(.015,.058)]:
        hit,position,normal,_=cup.ray_cast(Vector((x,-1,z)),Vector((0,1,0)))
        if hit:points.append(position+normal*.00012)
    if len(points)>1:
        spline=curve.splines.new('POLY');spline.points.add(len(points)-1)
        for p,co in zip(spline.points,points):p.co=(*co,1)
    fracture=bpy.data.objects.new('HairlineFracture',curve);bpy.context.collection.objects.link(fracture)
    glaze=bpy.data.materials.new('CupHairline');glaze.diffuse_color=(.07,.04,.02,1);curve.materials.append(glaze)
    bpy.ops.object.select_all(action='DESELECT');fracture.select_set(True);bpy.context.view_layer.objects.active=fracture;bpy.ops.object.convert(target='MESH')
    export('ChippedCeramicCup', [cup,fracture])
    for name in ['mug2.png','mug2_norm.png']:
        shutil.copy2(CACHE / 'Mugs' / name, OUT / 'Textures' / name)
    outputs=[]
    for path in sorted(OUT.rglob('*')):
        if path.is_file() and path.suffix != '.meta':
            outputs.append(dict(file=path.relative_to(ROOT).as_posix(), bytes=path.stat().st_size,
                                sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    (ROOT / 'Audit/prolog-revision-derived-assets.json').write_text(json.dumps(outputs,indent=2)+'\n')
    print('PROLOG_REVISION_ASSETS_PREPARED',len(outputs))


if __name__ == '__main__':
    main()
