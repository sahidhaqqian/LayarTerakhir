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
forward=(middle-wrist).normalized()
side=index-pinky;side=(side-forward*side.dot(forward)).normalized()
normal=forward.cross(side).normalized()
# Source palm width -> 75mm. DreadOut-style landscape grasp: fingers up the
# back of the handset, index over the top bezel, thumb on the lower front bezel.
scale=.075/(index-pinky).length
palm=(wrist+middle)*.5
rotation=Matrix((-side,forward,normal))
transform=rotation.to_4x4()
for i in range(3):
    for j in range(3):transform[i][j]*=scale
transform.translation=Vector((.05,-.035,.03))-transform.to_3x3()@palm
inverse=transform.inverted()
target=inverse@Vector((.014,-.055,-.014))
chain=[rig.pose.bones[n] for n in ['thumb.01.R','thumb.02.R','thumb.03.R']]
for iteration in range(100):
    for bone in reversed(chain):
        bpy.context.view_layer.update()
        pivot=bone.head.copy();tip=chain[-1].tail.copy()
        a=tip-pivot;b=target-pivot
        if min(a.length,b.length)<1e-6:continue
        q=a.rotation_difference(b)
        angle=min(q.angle,.18)
        if angle<1e-6:continue
        q=Quaternion(q.axis,angle)
        bone.matrix=Matrix.Translation(pivot)@q.to_matrix().to_4x4()@Matrix.Translation(-pivot)@bone.matrix
    if (transform@chain[-1].tail-Vector((.014,-.055,-.014))).length<.001:break
# DreadOut reference: index curls over the top onto the front bezel; the
# remaining fingers stay together behind the set, hidden by the occluder.
front_dir=transform.to_quaternion().inverted()@Vector((0,0,-1))
palm_axis=transform.to_quaternion().inverted()@Vector((0,0,1))
FLEX={2:[30,47,37],3:[4,10,8],4:[3,8,6],5:[2,6,5]}
ADDUCT={2:-15,3:0,4:18,5:30}
for f in range(2,6):
    for seg in range(1,4):
        bone=rig.pose.bones.get(f'f_{["index","middle","ring","pinky"][f-2]}.{seg:02d}.R')
        if not bone:continue
        direction=(bone.bone.tail_local-bone.bone.head_local).normalized()
        bend=direction.cross(front_dir)
        if bend.length<1e-6:continue
        inverse_bone=bone.bone.matrix_local.to_quaternion().inverted()
        quaternion=Quaternion(inverse_bone@bend.normalized(),math.radians(FLEX[f][seg-1]))
        if seg==1 and ADDUCT[f]!=0:
            quaternion=Quaternion(inverse_bone@palm_axis,math.radians(ADDUCT[f]))@quaternion
        bone.rotation_mode='QUATERNION'
        bone.rotation_quaternion=quaternion
bpy.context.view_layer.update()
deps=bpy.context.evaluated_depsgraph_get();evaluated=mesh.evaluated_get(deps);data=evaluated.to_mesh()
assert len(data.vertices)==len(mesh.data.vertices),'Source rig must preserve topology'
selected_groups={g.index for g in mesh.vertex_groups if g.name.endswith('.R') and any(s in g.name for s in ['forearm','hand','palm','f_','thumb'])}
selected=[p for p in mesh.data.polygons if sum(sum(g.weight for g in mesh.data.vertices[v].groups if g.group in selected_groups) for v in p.vertices)/len(p.vertices)>.7]
points=[];faces=[];uvs=[];mapping={}
to_rig=rig.matrix_world.inverted()@evaluated.matrix_world
# Unity camera-space -> Blender coordinates for standard FBX export.
axis=Matrix(((1,0,0),(0,0,-1),(0,1,0)))
for polygon in selected:
    face=[];coords=[]
    for vertex,loop in zip(polygon.vertices,polygon.loop_indices):
        if vertex not in mapping:
            mapping[vertex]=len(points);points.append(axis@(transform@(to_rig@data.vertices[vertex].co)))
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
    gripStyle='DreadOut landscape: fingers up, index over top bezel, thumb lower front bezel',
    fingerFlexDegrees=FLEX,fingerAdductionDegrees=ADDUCT,
    triangles=sum(len(p.vertices)-2 for p in existing.polygons),thumbTipErrorMetres=(transform@chain[-1].tail-Vector((.014,-.055,-.014))).length)
(ROOT/'Audit/phase1-sourced-hand.json').write_text(json.dumps(record,indent=2)+'\n')
print('SOURCED_PHONE_GRASP',record)
