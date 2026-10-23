"""Blender: orient the publisher's stair mesh for Unity. No new topology/UVs."""
import hashlib
import json
import math
from pathlib import Path

import bpy
from mathutils import Matrix, Vector

ROOT = Path(__file__).resolve().parents[1]
source = ROOT/'Builds/ArtSources/Phase1External/ConcreteInterior/Modular Concrete Interior/Modular Concrete Interior.blend'
output = ROOT/'Assets/_LayarTerakhir/assets/Art/Phase1/Imported/ConcreteInterior/StairsForward.fbx'
bpy.ops.wm.read_factory_settings(use_empty=True)
with bpy.data.libraries.load(str(source), link=False) as (available, selected):
    selected.objects = ['Stairs']
obj = selected.objects[0]
bpy.context.collection.objects.link(obj)
rotation = Matrix.Rotation(math.pi/2, 4, 'Z')
transform = rotation @ obj.matrix_world
points = [transform @ Vector(p) for p in obj.bound_box]
centre = sum(points, Vector())/8
obj.data.transform(Matrix.Translation(-centre) @ transform)
obj.matrix_world = Matrix.Identity(4)
obj.select_set(True)
bpy.context.view_layer.objects.active = obj
bpy.ops.export_scene.fbx(filepath=str(output), use_selection=True, object_types={'MESH'}, axis_forward='-Z', axis_up='Y',
                        bake_anim=False, add_leaf_bones=False, path_mode='STRIP')
(ROOT/'Audit/phase1-stair-import.json').write_text(json.dumps(dict(sourcePage='https://axel-kulomaa.itch.io/modular-concrete-interior',
    license='CC0-1.0', sourceSha256=hashlib.sha256(source.read_bytes()).hexdigest(), conversionOnly=True,
    sourceObject='Stairs', polygons=len(obj.data.polygons), output=str(output.relative_to(ROOT))),indent=2)+'\n')
