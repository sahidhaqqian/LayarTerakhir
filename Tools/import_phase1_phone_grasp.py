"""Pose/extract the downloaded CC0 FPS hand with its source rig; no new geometry/UVs.

blender -b --python Tools/import_phase1_phone_grasp.py
"""
import hashlib
import json
import math
import shutil
from pathlib import Path

import bpy
from mathutils import Matrix, Quaternion, Vector

ROOT=Path(__file__).resolve().parents[1]
SOURCE=ROOT/'Builds/ArtSources/Phase1External/FpsArms/FPS ARMS RIG 1.blend'
MODEL=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase1/Characters/Models/PhoneCameraGrip.fbx'
TEXTURE=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase1/Characters/Textures/PhoneCameraGripDiffuse.png'
bpy.ops.wm.open_mainfile(filepath=str(SOURCE))
if bpy.context.mode!='OBJECT':bpy.ops.object.mode_set(mode='OBJECT')
rig=next(o for o in bpy.data.objects if o.type=='ARMATURE')
mesh=next(o for o in bpy.data.objects if o.type=='MESH')
rig.animation_data_clear()
for bone in rig.pose.bones:
    bone.matrix_basis=Matrix.Identity(4)
    for constraint in list(bone.constraints):bone.constraints.remove(constraint)
bpy.context.view_layer.update()
wrist=rig.data.bones['hand.R'].head_local
index=rig.data.bones['f_index.01.R'].head_local
pinky=rig.data.bones['f_pinky.01.R'].head_local
middle=rig.data.bones['f_middle.01.R'].head_local
axis=Matrix(((1,0,0),(0,0,-1),(0,1,0)))
# DreadOut landscape grasp: the palms brace the side edges, the index curls
# over the top corner onto the front, the thumb presses the lower front corner,
# and the remaining fingers stay wrapped behind the set.
hand_forward=(middle-wrist).normalized()
hand_side=index-pinky;hand_side=(hand_side-hand_forward*hand_side.dot(hand_forward)).normalized()
hand_normal=hand_forward.cross(hand_side).normalized()
thumb=rig.data.bones['thumb.01.R'].head_local
if hand_normal.dot(thumb-((wrist+middle)*.5))<0:hand_normal=-hand_normal
Wu=axis.inverted()@wrist
Lu=(axis.inverted()@(wrist+hand_forward))-Wu;Lu.normalize()
Au=(axis.inverted()@(wrist+hand_side))-Wu;Au.normalize()
Nu=(axis.inverted()@(wrist+hand_normal))-Wu;Nu.normalize()
Wp=Vector((.082,-.060,.010))
Lp=Vector((0,.82,.57)).normalized()
Np=Vector((-1,0,0));Np=(Np-Lp*Np.dot(Lp)).normalized()
Ap=Np.cross(Lp).normalized()
scale=.105/(index-pinky).length
Ms=Matrix((Lu,Au,Nu)).transposed()
Mt=Matrix((Lp,Ap,Np)).transposed()
R=Mt@Ms.transposed()*scale
Tp=Vector((.022,-.062,-.012))
Tb=axis@(Wu+R.inverted()@(Tp-Wp))
chain=[rig.pose.bones[n] for n in ['thumb.01.R','thumb.02.R','thumb.03.R']]
for iteration in range(200):
    for bone in reversed(chain):
        bpy.context.view_layer.update()
        pivot=bone.head.copy();tip=chain[-1].tail.copy()
        a=tip-pivot;b=Tb-pivot
        if min(a.length,b.length)<1e-6:continue
        q=a.rotation_difference(b)
        angle=min(q.angle,.14)
        if angle<1e-6:continue
        q=Quaternion(q.axis,angle)
        bone.matrix=Matrix.Translation(pivot)@q.to_matrix().to_4x4()@Matrix.Translation(-pivot)@bone.matrix
    if (chain[-1].tail-Tb).length<.001:break
FLEX={2:[15,45,45],3:[-5,0,0],4:[-8,0,0],5:[-10,0,0]}
fingers=['index','middle','ring','pinky']
for f in range(2,6):
    for seg in range(1,4):
        bone=rig.pose.bones.get(f'f_{fingers[f-2]}.{seg:02d}.R')
        if not bone:continue
        direction=(bone.bone.tail_local-bone.bone.head_local).normalized()
        bend=direction.cross(hand_normal)
        if bend.length<1e-6:continue
        local_ax=bone.bone.matrix_local.to_quaternion().inverted()@bend.normalized()
        bone.rotation_mode='QUATERNION'
        bone.rotation_quaternion=Quaternion(local_ax,math.radians(FLEX[f][seg-1]))
bpy.context.view_layer.update()
deps=bpy.context.evaluated_depsgraph_get();evaluated=mesh.evaluated_get(deps);data=evaluated.to_mesh()
assert len(data.vertices)==len(mesh.data.vertices),'Source rig must preserve topology'
selected_groups={g.index for g in mesh.vertex_groups if g.name.endswith('.R') and any(s in g.name for s in ['forearm','hand','palm','f_','thumb'])}
selected=[p for p in mesh.data.polygons if sum(sum(g.weight for g in mesh.data.vertices[v].groups if g.group in selected_groups) for v in p.vertices)/len(p.vertices)>.7]
points=[];faces=[];uvs=[];mapping={}
to_rig=rig.matrix_world.inverted()@evaluated.matrix_world
for polygon in selected:
    face=[];coords=[]
    for vertex,loop in zip(polygon.vertices,polygon.loop_indices):
        if vertex not in mapping:
            mapping[vertex]=len(points);points.append(axis@(Wp+R@(axis.inverted()@(to_rig@data.vertices[vertex].co)-Wu)))
        face.append(mapping[vertex]);coords.append(mesh.data.uv_layers.active.data[loop].uv.copy())
    faces.append(face);uvs.append(coords)
evaluated.to_mesh_clear()
existing=bpy.data.meshes.new('SourceParaHandPose');existing.from_pydata(points,[],faces);existing.update()
layer=existing.uv_layers.new(name='UVMap')
for polygon,coords in zip(existing.polygons,uvs):
    polygon.use_smooth=True
    for loop,uv in zip(polygon.loop_indices,coords):layer.data[loop].uv=uv
hand=bpy.data.objects.new('PhoneHand_VisibleSkin',existing);bpy.context.collection.objects.link(hand)
bpy.ops.object.select_all(action='DESELECT');hand.select_set(True)
for name,point in [('GripOrigin',(0,0,0)),('GripRight',(.1,0,0)),('GripUp',(0,.1,0)),('GripForward',(0,0,.1))]:
    node=bpy.data.objects.new(name,None);bpy.context.collection.objects.link(node);node.location=axis@Vector(point);node.select_set(True)
bpy.context.view_layer.objects.active=hand
bpy.ops.export_scene.fbx(filepath=str(MODEL),use_selection=True,object_types={'MESH','EMPTY'},axis_forward='-Z',axis_up='Y',bake_anim=False,add_leaf_bones=False,path_mode='STRIP')
shutil.copy2(SOURCE.parent/'new_diff.png',TEXTURE)
record=dict(sourcePage='https://opengameart.org/content/fps-arms-rigged-only',author='para / MakeHuman',license='CC0-1.0',
    archiveSha256=hashlib.sha256((SOURCE.parent.parent/'fps-arms.7z').read_bytes()).hexdigest(),
    sourceSha256=hashlib.sha256(SOURCE.read_bytes()).hexdigest(),originalTopologyAndUVs=True,
    conversion='Source rig posed; existing right forearm/hand faces extracted; no original hand/model artwork',
    gripStyle='DreadOut landscape: palms brace the side edges, index over the top corner, thumb on the lower front corner, other fingers wrapped behind the set',
    fingerFlexDegrees=FLEX,
    triangles=sum(len(p.vertices)-2 for p in existing.polygons),thumbTipErrorMetres=(chain[-1].tail-Tb).length)
(ROOT/'Audit/phase1-sourced-hand.json').write_text(json.dumps(record,indent=2)+'\n')
print('SOURCED_PHONE_GRASP',record)
