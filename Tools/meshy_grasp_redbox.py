"""Preview genggaman Meshy + proxy HP kotak merah (tampak depan).
Alur sesuai CATATAN_POSISI.md (Audit/Visuals/Feel-Meshy/):
tangan GLB mentah, ibu jari -6.6/-3.3 deg, rapat tengah 2mm, box 320x140mm.
Input : GLB Meshy dari R2 tangan/ -> taruh di /tmp/opencode/tangan/mentah/
        (Meshy_AI_Invisible_Frame_20261105_character.glb)
Output: Audit/Visuals/Feel-Meshy/mentah_hp_gede_depan.png
Jalan : blender -b --python Tools/meshy_grasp_redbox.py
"""
import bpy, math
import numpy as np
from mathutils import Euler
S = 0.0642
W,H,T = 0.32, 0.14, 0.008
bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.gltf(filepath="/tmp/opencode/tangan/mentah/Meshy_AI_Invisible_Frame_20261105_character.glb")
rig = next(o for o in bpy.data.objects if o.type == 'ARMATURE')
rig.scale = (S,S,S)
bpy.context.view_layer.update()
bpy.context.view_layer.objects.active = rig
bpy.ops.object.mode_set(mode='POSE')
# thumbs only: MCP (024/038) + IP (023/037), rotate local X to drop tips
rig.pose.bones['Bone_024'].rotation_mode = 'XYZ'
rig.pose.bones['Bone_024'].rotation_euler = Euler((math.radians(-6.6),0,0),'XYZ')
rig.pose.bones['Bone_023'].rotation_mode = 'XYZ'
rig.pose.bones['Bone_023'].rotation_euler = Euler((math.radians(-3.3),0,0),'XYZ')
rig.pose.bones['Bone_038'].rotation_mode = 'XYZ'
rig.pose.bones['Bone_038'].rotation_euler = Euler((math.radians(-6.6),0,0),'XYZ')
rig.pose.bones['Bone_037'].rotation_mode = 'XYZ'
rig.pose.bones['Bone_037'].rotation_euler = Euler((math.radians(-3.3),0,0),'XYZ')
bpy.context.view_layer.update()
def tip(bn_tip):
    rb = rig.data.bones[bn_tip]
    return np.array(rig.matrix_world @ (rig.pose.bones[bn_tip].matrix @ rb.matrix_local.inverted() @ rb.tail_local))
for tip_bn, label in [('Bone_035','ThumbR'),('Bone_021','ThumbL'),('Bone_025','IndexR')]:
    print(label, tuple(round(v*1000,1) for v in tip(tip_bn)), flush=True)
bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
# bake pose, split L/R, shift each 2mm toward center (phone stays)
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
        print("SHIFT", o.name, "dx=%+.1fmm" % (0.002*1000 if c<0 else -0.002*1000), flush=True)
bpy.context.view_layer.update()
# phone down 5mm: bottom edge now z=-10mm
bpy.ops.mesh.primitive_cube_add(size=1)
box = bpy.context.active_object
box.scale = (W/2, T/2, H/2); box.location = (0, -0.031, -0.012+H/2)
mat = bpy.data.materials.new('PhoneRed'); mat.use_nodes = True
bsdf = mat.node_tree.nodes['Principled BSDF']
bsdf.inputs['Base Color'].default_value = (0.9,0.1,0.1,1)
bsdf.inputs['Alpha'].default_value = 0.45
mat.blend_method = 'BLEND'
box.data.materials.append(mat)
scene = bpy.context.scene
scene.render.resolution_x = 1280; scene.render.resolution_y = 720
scene.render.film_transparent = True
cam = bpy.data.objects.new('Cam', bpy.data.cameras.new('Cam'))
scene.collection.objects.link(cam); scene.camera = cam
cam.data.lens = 50
from mathutils import Vector
cam.location = (0, -0.9, 0.060)
d = Vector((0, -0.031, 0.060)) - cam.location
cam.rotation_euler = d.to_track_quat('-Z','Y').to_euler()
for nm, loc, e in [('Key',(0.5,0.5,1.0),5.0),('Fill',(-0.5,-0.5,0.3),2.0)]:
    l = bpy.data.objects.new(nm, bpy.data.lights.new(nm,'SUN'))
    scene.collection.objects.link(l); l.data.energy = e; l.location = loc
scene.render.filepath = 'Audit/Visuals/Feel-Meshy/mentah_hp_gede_depan.png'
bpy.ops.render.render(write_still=True)
print('THUMBDOWN_OK', flush=True)
