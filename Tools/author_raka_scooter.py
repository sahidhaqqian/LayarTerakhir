"""Keep the optimized scooter, restore its closed saddle, and export physical rider contacts.

The prepared Damar model is grouped by material, so its old part names are gone.
Closed seat geometry comes from the original recovery .blend; neither original is changed.
Run with Blender --background --python Tools/author_raka_scooter.py.
"""
from pathlib import Path
import hashlib
import json
import bpy
from mathutils import Vector, Matrix
from mathutils.bvhtree import BVHTree

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT/'Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx'
RECOVERY = ROOT/'ArtSources/Scene2_Atmaja_Recovery/02_Motorcycle/Blender/DamarScooter.blend'
OUTPUT = ROOT/'Assets/_LayarTerakhir/assets/Art/Review/Models/RakaScooter.fbx'

bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.fbx(filepath=str(SOURCE))
seat = next(o for o in bpy.context.scene.objects if o.type == 'MESH' and 'SeatVinyl' in o.name)
seat_name = seat.name
seat_material = seat.data.materials[0]
with bpy.data.libraries.load(str(RECOVERY), link=False) as (available, loaded):
    loaded.objects = [name for name in ['SEAT', 'SEAT_FRONT_STEP'] if name in available.objects]
assert len(loaded.objects) == 2, 'Original closed saddle pieces are required'
vertices, faces = [], []
for obj in loaded.objects:
    bpy.context.collection.objects.link(obj)
    evaluated = obj.evaluated_get(bpy.context.evaluated_depsgraph_get())
    mesh = evaluated.to_mesh()
    start = len(vertices)
    vertices.extend(tuple(obj.matrix_world @ vertex.co) for vertex in mesh.vertices)
    faces.extend(tuple(start+i for i in polygon.vertices) for polygon in mesh.polygons)
    evaluated.to_mesh_clear()
    bpy.data.objects.remove(obj, do_unlink=True)
mesh = bpy.data.meshes.new('RakaClosedSaddle')
mesh.from_pydata(vertices, [], faces);mesh.materials.append(seat_material)
old_mesh = seat.data;seat.data = mesh;seat.matrix_world = Matrix.Identity(4)
if old_mesh.users == 0:bpy.data.meshes.remove(old_mesh)
seat.name = seat_name

# Source geometry: GRIP +/-1 centres; FLOORBOARD top; the front half of the closed SEAT.
contacts = {
    'RiderSeatSurface': (0, .14, .7625),
    'RiderGripLeft': (-.255, -.455, .92),
    'RiderGripRight': (.255, -.455, .92),
    'RiderFootLeft': (-.115, -.06, .2975),
    'RiderFootRight': (.115, -.06, .2975),
}
for name, position in contacts.items():
    obj = bpy.data.objects.new(name, None);bpy.context.collection.objects.link(obj)
    obj.location = Vector(position);obj.empty_display_size = .025
rubber = next(o for o in bpy.context.scene.objects if o.type=='MESH' and 'DarkPlastic' in o.name)
surface = BVHTree.FromObject(rubber, bpy.context.evaluated_depsgraph_get())
grip_surface_distances = {}
for name in ['RiderGripLeft','RiderGripRight']:
    point = Vector(contacts[name]);nearest = surface.find_nearest(rubber.matrix_world.inverted() @ point)[0]
    distance = (rubber.matrix_world @ nearest-point).length
    assert distance < .035, (name, distance)
    grip_surface_distances[name] = distance
bpy.ops.object.select_all(action='DESELECT')
objects = [o for o in bpy.context.scene.objects if o.type in {'MESH', 'EMPTY'}]
for obj in objects:obj.select_set(True)
bpy.context.view_layer.objects.active = seat
bpy.ops.export_scene.fbx(filepath=str(OUTPUT), use_selection=True, object_types={'MESH','EMPTY'},
    axis_forward='-Z', axis_up='Y', bake_anim=False, add_leaf_bones=False, path_mode='STRIP')
triangles = sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in objects if o.type=='MESH')
record = dict(name='RakaScooter',source='Existing optimized Damar scooter; original closed recovery saddle and geometry-authored contacts',
    triangles=triangles,sha256=hashlib.sha256(OUTPUT.read_bytes()).hexdigest())
receipt = dict(**record, contactsBlenderMetres=contacts,
    gripSurfaceDistanceMetres=grip_surface_distances,
    preparedSourceSha256=hashlib.sha256(SOURCE.read_bytes()).hexdigest(),
    recoverySourceSha256=hashlib.sha256(RECOVERY.read_bytes()).hexdigest())
(ROOT/'Audit/raka-scooter-contact-authoring.json').write_text(json.dumps(receipt,indent=2)+'\n')
manifest = ROOT/'Audit/designer-review-mesh-authoring.json'
if manifest.exists():
    data=json.loads(manifest.read_text());data['meshes']=[record if r['name']=='RakaScooter' else r for r in data['meshes']]
    manifest.write_text(json.dumps(data,indent=2)+'\n')
print('RAKA_SCOOTER_CONTACTS_AUTHORED', triangles, len(contacts), flush=True)
