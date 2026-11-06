"""Preview genggaman Meshy + HP game asli (tampak depan).
Alur sama seperti meshy_grasp_redbox.py, kotak merah diganti mesh
Smartphone.fbx (landscape, kaca ke kamera) sesuai CATATAN_POSISI.md.
Input : GLB Meshy (lihat atas) + Assets Smartphone.fbx (repo)
Output: Audit/Visuals/Feel-Meshy/mentah_hp_nyata_depan.png
Jalan : blender -b --python Tools/meshy_grasp_realphone.py
"""
import bpy, math
import numpy as np
from mathutils import Euler, Vector
S = 0.0642
# ---- hands (same approved pose) ----
bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.gltf(filepath="/tmp/opencode/tangan/mentah/Meshy_AI_Invisible_Frame_20261105_character.glb")
rig = next(o for o in bpy.data.objects if o.type == 'ARMATURE')
rig.scale = (S,S,S)
bpy.context.view_layer.update()
bpy.context.view_layer.objects.active = rig
bpy.ops.object.mode_set(mode='POSE')
for bn, ang in (('Bone_024', math.radians(-6.6)), ('Bone_038', math.radians(-6.6))):
    rig.pose.bones[bn].rotation_mode = 'XYZ'
    rig.pose.bones[bn].rotation_euler = Euler((ang,0,0),'XYZ')
for bn, ang in (('Bone_023', math.radians(-3.3)), ('Bone_037', math.radians(-3.3))):
    rig.pose.bones[bn].rotation_mode = 'XYZ'
    rig.pose.bones[bn].rotation_euler = Euler((ang,0,0),'XYZ')
bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
mesh = next(o for o in bpy.data.objects if o.type == 'MESH' and o.name == 'Mesh_0')
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
bpy.context.view_layer.objects.active = mesh
for m in [m for m in mesh.modifiers if m.type=='ARMATURE']:
    bpy.ops.object.modifier_apply(modifier=m.name)
bpy.ops.object.parent_clear(type='CLEAR_KEEP_TRANSFORM')
bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
bpy.ops.object.mode_set(mode='EDIT')
bpy.ops.mesh.select_all(action='DESELECT')
bpy.ops.object.mode_set(mode='OBJECT')
Pm = np.array([tuple(v.co) for v in mesh.data.vertices])
for i in np.where(Pm[:,0]<0)[0]:
    mesh.data.vertices[i].select = True
bpy.ops.object.mode_set(mode='EDIT')
bpy.ops.mesh.separate(type='SELECTED')
bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
for o in bpy.data.objects:
    if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.')):
        c = float(np.mean([v.co[0] for v in o.data.vertices]))
        o.location.x += 0.002 if c<0 else -0.002
bpy.context.view_layer.update()
print("HANDS_OK", flush=True)
# ---- real phone ----
bpy.ops.import_scene.fbx(filepath="/root/projects/LayarTerakhir/Assets/_LayarTerakhir/assets/Art/Original/Models/Smartphone.fbx")
phone_parts = [o for o in bpy.data.objects if o.type=='MESH' and 'Mesh_0' not in o.name]
print("PARTS:", [(o.name) for o in phone_parts][:8], flush=True)
case = next(o for o in phone_parts if 'case' in o.name.lower())
glass = next((o for o in phone_parts if 'glass' in o.name.lower() or 'display' in o.name.lower()), None)
P = np.array([tuple((case.matrix_world @ v.co)) for v in case.data.vertices])
mn, mx = P.min(0), P.max(0)
print("CASE span:", tuple(round(float(v)*1000,1) for v in (mx-mn)), "center:", tuple(round(float(v)*1000,1) for v in ((mn+mx)/2)), flush=True)
if glass is not None:
    G = np.array([tuple((glass.matrix_world @ v.co)) for v in glass.data.vertices])
    print("GLASS center:", tuple(round(float(v)*1000,1) for v in (G.mean(0))), flush=True)
print("HP_OK", flush=True)

# ---- seat phone (clean: apply transforms, then pure numpy) ----
bpy.ops.object.select_all(action='DESELECT')
for o in phone_parts:
    o.select_set(True)
bpy.context.view_layer.objects.active = phone_parts[0]
bpy.ops.object.parent_clear(type='CLEAR_KEEP_TRANSFORM')
bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
bpy.context.view_layer.update()
phone_parts = [o for o in phone_parts if 'Icosphere' not in o.name]
for o in [o for o in bpy.data.objects if o.type=='MESH' and 'Icosphere' in o.name]:
    bpy.data.objects.remove(o, do_unlink=True)
Q = np.vstack([np.array([tuple(v.co) for v in o.data.vertices]) for o in phone_parts])
print("CLEAN span:", tuple(round(float(v)*1000,1) for v in (Q.max(0)-Q.min(0))), flush=True)
sx, sy, sz = 0.18/0.145, 0.008/0.012, 0.07875/0.073
counts = []
off = 0
for o in phone_parts:
    n = len(o.data.vertices)
    P = Q[off:off+n]; off += n
    A = np.empty_like(P); A[:,0]=P[:,1]*sx; A[:,1]=-P[:,2]*sy; A[:,2]=-P[:,0]*sz
    counts.append(A)
A = np.vstack(counts)
t = np.array([0.0 - (A[:,0].min()+A[:,0].max())/2, 0.008 - (A[:,1].min()+A[:,1].max())/2, 0.018 - A[:,2].min()])
print("SEAT2 t=", tuple(round(float(v)*1000,1) for v in t), flush=True)
A = A + t
off = 0
for o in phone_parts:
    n = len(o.data.vertices)
    for v, p in zip(o.data.vertices, A[off:off+n]):
        v.co = tuple(p)
    off += n
bpy.context.view_layer.update()
_F = np.vstack([np.array([tuple(v.co) for v in o.data.vertices]) for o in phone_parts])
print("SEATED2 span:", tuple(round(float(v)*1000,1) for v in (_F.max(0)-_F.min(0))), "min:", tuple(round(float(v)*1000,1) for v in _F.min(0)), flush=True)
# simple clean materials
body = bpy.data.materials.new('PhoneBody'); body.use_nodes = True
body.node_tree.nodes['Principled BSDF'].inputs['Base Color'].default_value = (0.05,0.05,0.06,1)
body.node_tree.nodes['Principled BSDF'].inputs['Roughness'].default_value = 0.4
scr = bpy.data.materials.new('PhoneScreen'); scr.use_nodes = True
sbsdf = scr.node_tree.nodes['Principled BSDF']
sbsdf.inputs['Base Color'].default_value = (0.75,0.88,1.0,1)
for o in phone_parts:
    n = o.name.lower()
    o.data.materials.clear()
    o.data.materials.append(scr if ('glass' in n or 'display' in n or 'camera' in n) else body)
scene = bpy.context.scene
scene.render.resolution_x = 1280; scene.render.resolution_y = 720
scene.render.film_transparent = True
cam = bpy.data.objects.new('Cam', bpy.data.cameras.new('Cam'))
scene.collection.objects.link(cam); scene.camera = cam
cam.data.lens = 50
cam.location = (0, -0.9, 0.060)
d = Vector((0, -0.031, 0.060)) - cam.location
cam.rotation_euler = d.to_track_quat('-Z','Y').to_euler()
for nm, loc, e in [('Key',(0.5,0.5,1.0),5.0),('Fill',(-0.5,-0.5,0.3),2.0)]:
    l = bpy.data.objects.new(nm, bpy.data.lights.new(nm,'SUN'))
    scene.collection.objects.link(l); l.data.energy = e; l.location = loc
scene.render.filepath = 'Audit/Visuals/Feel-Meshy/mentah_hp_nyata_depan.png'
bpy.ops.render.render(write_still=True)
print('HPNYATA_OK', flush=True)
