"""Export tangan Meshy pose approved ke PhoneCameraGrip.fbx game.
Pose: ibu jari MCP -6.6deg + IP -3.3deg, rapat tengah 2mm, scale S=0.0642,
similarity s=165/180 ke box game 165mm, tangan kanan saja (builder mirror kiri).
Guides: Thumb/Index/Middle (snap permukaan) + Wrist + Origin/Right/Up/Forward.
Jalan : blender -b --python Tools/export_meshy_grasp_game.py
Lanjut: Unity RebuildPhoneGrasp -> PlayerFeelTests.
"""
import bpy, math
import numpy as np
from mathutils import Euler
S = 0.0642
W,H,T = 0.165, 0.165/1.9, 0.008
GLB = "/root/projects/LayarTerakhir/Assets/_LayarTerakhir/assets/Art/Phase1/Characters/Models/Meshy_AI_Invisible_Frame_20261105_character.glb"
OUT = "/root/projects/LayarTerakhir/Assets/_LayarTerakhir/assets/Art/Phase1/Characters/Models/PhoneCameraGrip.fbx"
bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.gltf(filepath=GLB)
rig = next(o for o in bpy.data.objects if o.type == 'ARMATURE')
mesh = next(o for o in bpy.data.objects if o.type == 'MESH' and o.name == 'Mesh_0')
for o in [o for o in bpy.data.objects if o.type=='MESH' and o.name!='Mesh_0']:
    bpy.data.objects.remove(o, do_unlink=True)
rig.scale = (S,S,S)
bpy.context.view_layer.update()
# approved thumb pose ONLY
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
print("POSED", flush=True)
# bake
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
bpy.context.view_layer.objects.active = mesh
for m in [m for m in mesh.modifiers if m.type=='ARMATURE']:
    bpy.ops.object.modifier_apply(modifier=m.name)
bpy.ops.object.parent_clear(type='CLEAR_KEEP_TRANSFORM')
bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
# split + shift toward center 2mm
bpy.ops.object.mode_set(mode='EDIT')
bpy.ops.mesh.select_all(action='DESELECT')
bpy.ops.object.mode_set(mode='OBJECT')
P = np.array([tuple(v.co) for v in mesh.data.vertices])
for i in np.where(P[:,0]<0)[0]:
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
print("BAKED_SPLIT_SHIFT", flush=True)
# gather world verts (FIT frame)
deps = bpy.context.evaluated_depsgraph_get()
Pall = []
owners = []
for o in bpy.data.objects:
    if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.')):
        ev = o.evaluated_get(deps)
        Pall.append(np.array([tuple(ev.matrix_world @ v.co) for v in ev.data.vertices]))
P = np.vstack(Pall)
print("FIT span x:", round(P[:,0].min(),3), round(P[:,0].max(),3), flush=True)
# similarity: uniform s about preview-phone center C, then FIT->Unity map
s = (W/0.18)
C = np.array([0.0, 0.00805, 0.05735])
P1 = C + s*(P - C)
# preview-center -> Unity-box-center: Ux = s*Fx - W/2, Uy = s*(Fz-0.05735), Uz = s*(Fy-0.00805)
U = np.empty_like(P)
U[:,0] = C[0] + s*(P[:,0]-C[0]) - W/2
U[:,1] = s*(P[:,2]-C[2])
U[:,2] = s*(P[:,1]-C[1])
print("U span x:", round(U[:,0].min(),3), round(U[:,0].max(),3), flush=True)
print("U span y:", round(U[:,1].min(),3), round(U[:,1].max(),3), flush=True)
print("U span z:", round(U[:,2].min(),3), round(U[:,2].max(),3), flush=True)
print("SEAT_OK", flush=True)

# ---- keep RIGHT hand only (Unity x >= -W/2), drop left ----
# write seated Unity coords back per-object, then delete left verts
deps2 = bpy.context.evaluated_depsgraph_get()
for o in [o for o in bpy.data.objects if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.'))]:
    ev = o.evaluated_get(deps2)
    Pw = np.array([tuple(ev.matrix_world @ v.co) for v in ev.data.vertices])
    P1o = C + s*(Pw - C)
    Uo = np.empty_like(P1o)
    Uo[:,0] = C[0] + s*(Pw[:,0]-C[0]) - W/2
    Uo[:,1] = s*(Pw[:,2]-C[2])
    Uo[:,2] = s*(Pw[:,1]-C[1])
    o.location.identity() if False else None
    # reset object transform to identity, bake into verts via inverse
    import mathutils as _mu
    M = o.matrix_world.copy()
    Mi = M.inverted()
    for v, p in zip(o.data.vertices, Uo):
        o.data.vertices[v.index].co = Mi @ _mu.Vector(tuple(p))
    o.matrix_world.identity()
bpy.context.view_layer.update()
# delete left (x < -W/2 in Unity coords == FBX x same)
for o in [o for o in bpy.data.objects if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.'))]:
    bpy.ops.object.select_all(action='DESELECT')
    o.select_set(True)
    bpy.context.view_layer.objects.active = o
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.select_all(action='DESELECT')
    bpy.ops.object.mode_set(mode='OBJECT')
    Q = np.array([tuple(v.co) for v in o.data.vertices])
    # NOTE: verts currently in FBX?? no - still Unity (FBX map applied below). select by Unity x
    for i in np.where(Q[:,0] < -W/2-0.001)[0]:
        o.data.vertices[i].select = True
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.delete(type='VERT')
    bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
for o in [o for o in bpy.data.objects if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.'))]:
    if len(o.data.vertices) == 0:
        bpy.data.objects.remove(o, do_unlink=True)
        print("DROP-EMPTY", o.name if False else "left-part", flush=True)
nR = sum(len(o.data.vertices) for o in bpy.data.objects if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.')))
print("RIGHT-ONLY n=", nR, flush=True)
# ---- Unity -> FBX (verified builder map inverse): FBX=(Ux,-Uz,Uy) ----
meshes = [o for o in bpy.data.objects if o.type=='MESH' and (o.name=='Mesh_0' or o.name.startswith('Mesh_0.'))]
for o in meshes:
    Q = np.array([tuple(v.co) for v in o.data.vertices])
    F = np.empty_like(Q); F[:,0]=Q[:,0]; F[:,1]=-Q[:,2]; F[:,2]=Q[:,1]
    for v, p in zip(o.data.vertices, F):
        v.co = tuple(p)
main = meshes[0]
main.name = 'PhoneHand_VisibleSkin'
main.data.name = 'PhoneHand_VisibleSkin'
for o in meshes[1:]:
    bpy.data.objects.remove(o, do_unlink=True)
mesh = main
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
bpy.context.view_layer.objects.active = mesh
bpy.ops.object.mode_set(mode='EDIT')
bpy.ops.mesh.select_all(action='SELECT')
bpy.ops.mesh.normals_make_consistent(inside=False)
bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
print("XFORM_OK", flush=True)
# ---- guides (FBX frame; targets from seated preview tips) ----
_Pf = np.array([tuple(v.co) for v in mesh.data.vertices])
def _fbx_of_U(p):
    return np.array([p[0], -p[2], p[1]])
# preview fingertip FIT coords -> Unity mapping (same similarity)
_ftipFIT = {'Thumb': np.array([0.0365,-0.0300,0.0123]), 'Index': np.array([0.0566,0.0341,0.0918]), 'Middle': np.array([0.0576,0.0076,0.1020])}
_targets_U = {}
for k, f in _ftipFIT.items():
    _targets_U[k] = np.array([C[0]+s*(f[0]-C[0])-W/2, s*(f[2]-C[2]), s*(f[1]-C[1])])
    print("TARGET_U", k, tuple(round(float(v)*1000,1) for v in _targets_U[k]), flush=True)
guides = {'GripOrigin': (0,0,0), 'GripRight': (0.1,0,0), 'GripUp': (0,0,0.1), 'GripForward': (0,-0.1,0)}
for name, tu in _targets_U.items():
    tf = _fbx_of_U(np.array(tu))
    d = np.linalg.norm(_Pf - tf, axis=1)
    guides['Grip'+name+'Contact'] = tuple(_Pf[int(np.argmin(d))])
    print("GUIDE", name, "->", tuple(round(v,4) for v in guides['Grip'+name+'Contact']), flush=True)
_outer = _Pf[_Pf[:,0]>0.0]
guides['GripWrist'] = tuple(float(v) for v in _outer.mean(axis=0)) if len(_outer) else (0.05, 0.0, 0.0)
print("GUIDE Wrist", tuple(round(v,4) for v in guides['GripWrist']), flush=True)
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
for gname, gpos in guides.items():
    bpy.ops.object.empty_add(location=gpos)
    bpy.context.active_object.name = gname
    bpy.context.active_object.select_set(True)
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
for o in bpy.data.objects:
    if o.type == 'EMPTY':
        o.select_set(True)
bpy.context.view_layer.objects.active = mesh
print("EXPORT_SEL", sorted([o.name for o in bpy.context.selected_objects]), flush=True)
bpy.ops.export_scene.fbx(filepath=OUT, use_selection=True, object_types={'MESH','EMPTY'},
                         axis_forward='-Z', axis_up='Y', bake_anim=False,
                         add_leaf_bones=False, path_mode='STRIP')
print("EXPORT_OK", OUT, flush=True)
