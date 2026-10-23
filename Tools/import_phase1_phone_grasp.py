"""Fit/bake a landscape grasp from the existing clothed Raka rig, without new art.

blender -b --python-exit-code 1 --python Tools/import_phase1_phone_grasp.py
Uses the existing geometry fitter; SciPy is an offline authoring dependency only.
"""
import ast
import hashlib
import json
import math
import sys
from pathlib import Path

import bpy
import numpy as np
from mathutils import Matrix, Quaternion, Vector

ROOT=Path(__file__).resolve().parents[1]
SOURCE=ROOT/'Assets/_LayarTerakhir/assets/Art/Characters/Models/RakaRigged.fbx'
MODEL=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase1/Characters/Models/PhoneCameraGrip.fbx'
sys.path.insert(0,str(ROOT/'Tools'))
from fit_designer_phone_grip import solve, sample_surface

bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.fbx(filepath=str(SOURCE))
rig=next(o for o in bpy.data.objects if o.type=='ARMATURE')
mesh=next(o for o in bpy.data.objects if o.type=='MESH' and o.name=='VisibleSkin')
rig.animation_data_clear()
for bone in rig.pose.bones:
    bone.matrix_basis=Matrix.Identity(4)
    for constraint in list(bone.constraints):bone.constraints.remove(constraint)
# Match the existing fitter's coordinate contract, including imported FBX units.
bpy.ops.object.select_all(action='DESELECT')
rig.select_set(True)
for part in bpy.data.objects:
    if part.type=='MESH':part.select_set(True)
bpy.context.view_layer.objects.active=rig
bpy.ops.object.transform_apply(location=True,rotation=True,scale=True)
defs={b.name:(b.head_local.copy(),b.tail_local.copy(),b.parent.name if b.parent else None) for b in rig.data.bones}
NERA_PALMS={}
PHONE_FINGER_FLEX={2:[30,0,90],3:[-10,30,80],4:[-10,20,80],5:[-10,0,10]}
tree=ast.parse((ROOT/'Tools/author_prolog_characters.py').read_text())
functions=[n for n in tree.body if isinstance(n,ast.FunctionDef) and n.name in {'hand_frame','set_pose'}]
assert len(functions)==2
exec(compile(ast.Module(body=functions,type_ignores=[]),str(ROOT/'Tools/author_prolog_characters.py'),'exec'),globals())
base_pose=set_pose
prior={'fingerQuaternions':{}}
def calibrated_pose(rig,defs,kind,phase,female):
    base_pose(rig,defs,kind,phase,female)
    for name,values in prior['fingerQuaternions'].items():
        rig.pose.bones[name].rotation_mode='QUATERNION'
        rig.pose.bones[name].rotation_quaternion=Quaternion(values)
    bpy.context.view_layer.update()

width=.165;height=width/1.9;thickness=.008
fit=solve(rig,mesh,defs,calibrated_pose,hand_frame,handset_extents=[width/2,height/2,thickness/2],contact_faces=['side',-1,-1,-1,-1])
assert fit['maxPenetrationMetres']<.0012 and fit['maxGlassIntrusionMetres']<.0015,fit
assert max(fit['fingerContactGapsMetres'])<.004,fit
longitudinal,palm,sideways=hand_frame(defs,'R')
pose=rig.pose.bones['Hand_R'].matrix@rig.data.bones['Hand_R'].matrix_local.inverted()
shift=fit['phoneOffset']
centre=pose@(defs['Hand_R'][0]+longitudinal*(.09+shift[0])+sideways*shift[1]+palm*(.045+shift[2]))
axes=[(pose.to_3x3()@v).normalized() for v in (-longitudinal,sideways,-palm)]
# Guide-based Unity import uses a right-edge origin, in metres.
axis=Matrix(((1,0,0),(0,0,-1),(0,1,0)))
def canonical(point):
    delta=point-centre
    return Vector((delta.dot(axes[0])-width/2,delta.dot(axes[1]),delta.dot(axes[2])))
indices={g.index for g in mesh.vertex_groups if g.name in ['Forearm_R','Hand_R'] or g.name.startswith('Finger') and g.name.endswith('_R')}
selected=[p for p in mesh.data.polygons if sum(sum(g.weight for g in mesh.data.vertices[i].groups if g.group in indices) for i in p.vertices)/len(p.vertices)>.65]
deps=bpy.context.evaluated_depsgraph_get();evaluated=mesh.evaluated_get(deps);data=evaluated.to_mesh()
points=[];faces=[];uvs=[];mapping={};contacts={};gaps={};digit_weights=[]
for polygon in selected:
    face=[];coords=[]
    # Reverse winding for this reflected source basis, preserving topology and UVs.
    for vertex,loop in zip(reversed(polygon.vertices),reversed(polygon.loop_indices)):
        if vertex not in mapping:
            mapping[vertex]=len(points)
            point=canonical(evaluated.matrix_world@data.vertices[vertex].co)
            points.append(axis@point)
            digit_weights.append([sum(g.weight for g in mesh.data.vertices[vertex].groups if mesh.vertex_groups[g.group].name.startswith('Finger'+str(finger)) and mesh.vertex_groups[g.group].name.endswith('_R')) for finger in range(1,6)])
        face.append(mapping[vertex]);coords.append(mesh.data.uv_layers.active.data[loop].uv.copy())
    faces.append(face);uvs.append(coords)
evaluated.to_mesh_clear()
existing=bpy.data.meshes.new('DesignerRakaLandscapeGrasp');existing.from_pydata(points,[],faces);existing.update();existing.calc_loop_triangles()
triangles=np.array([tuple(t.vertices) for t in existing.loop_triangles],dtype=int)
canonical_points=sample_surface(np.array([tuple(axis.inverted()@p) for p in points]),triangles)
sample_weights=sample_surface(np.array(digit_weights),triangles)
for values,weights in zip(canonical_points,sample_weights):
    point=Vector(values)
    for finger,name in enumerate(['Thumb','Index','Middle','Ring','Pinky']):
        face_gap=math.sqrt(point.x**2+max(0,abs(point.y)-height/2)**2+max(0,abs(point.z)-thickness/2)**2) if finger==0 else math.sqrt(max(0,-width-point.x,point.x)**2+max(0,abs(point.y)-height/2)**2+(point.z-thickness/2)**2)
        if weights[finger]>.25 and face_gap<gaps.get(name,float('inf')):gaps[name]=face_gap;contacts[name]=tuple(point)
penetration=max(min(p[0]+width,-p[0],p[1]+height/2,height/2-p[1],p[2]+thickness/2,thickness/2-p[2]) for p in canonical_points)
glass=max([max(0,thickness/2-p[2]) for p in canonical_points if abs(p[0]+width/2)<width*.43 and abs(p[1])<height*.46 and not (p[0]>-width*.18 and p[1]<.01)]+[0])
assert penetration<.0015 and glass<.0015 and len(contacts)==5 and max(gaps.values())<.004,(penetration,glass,gaps)
layer=existing.uv_layers.new(name='UVMap')
for polygon,coords in zip(existing.polygons,uvs):
    polygon.use_smooth=True
    for loop,uv in zip(polygon.loop_indices,coords):layer.data[loop].uv=uv
hand=bpy.data.objects.new('PhoneHand_VisibleSkin',existing);bpy.context.collection.objects.link(hand)
copies=[hand]
# Retain the existing jacket/T-shirt cuffs rather than exposing a cut forearm.
for original in list(bpy.data.objects):
    if original.type!='MESH' or not any(name in original.name for name in ['Jacket','TShirt']):continue
    groups={g.index for g in original.vertex_groups if g.name in ['Forearm_R','Hand_R']}
    selected_parts=[p for p in original.data.polygons if sum(sum(g.weight for g in original.data.vertices[i].groups if g.group in groups) for i in p.vertices)/len(p.vertices)>.65]
    if not selected_parts:continue
    evaluated=original.evaluated_get(deps);data=evaluated.to_mesh();pp=[];ff=[];uu=[];mm={}
    for polygon in selected_parts:
        face=[];coords=[]
        for vertex,loop in zip(reversed(polygon.vertices),reversed(polygon.loop_indices)):
            if vertex not in mm:mm[vertex]=len(pp);pp.append(axis@canonical(evaluated.matrix_world@data.vertices[vertex].co))
            face.append(mm[vertex]);coords.append(original.data.uv_layers.active.data[loop].uv.copy())
        ff.append(face);uu.append(coords)
    evaluated.to_mesh_clear();part=bpy.data.meshes.new('PhoneHand_'+original.name);part.from_pydata(pp,[],ff);part.update()
    uv=part.uv_layers.new(name='UVMap')
    for polygon,coords in zip(part.polygons,uu):
        polygon.use_smooth=True
        for loop,value in zip(polygon.loop_indices,coords):uv.data[loop].uv=value
    node=bpy.data.objects.new(part.name,part);bpy.context.collection.objects.link(node);copies.append(node)
bpy.ops.object.select_all(action='DESELECT')
for copy in copies:copy.select_set(True)
for name,point in [('GripOrigin',(0,0,0)),('GripRight',(.1,0,0)),('GripUp',(0,.1,0)),('GripForward',(0,0,.1)),('GripWrist',tuple(canonical(rig.matrix_world@rig.pose.bones['Hand_R'].head)))]+[('Grip'+name+'Contact',point) for name,point in contacts.items()]:
    node=bpy.data.objects.new(name,None);bpy.context.collection.objects.link(node);node.location=axis@Vector(point);node.select_set(True)
bpy.context.view_layer.objects.active=hand
bpy.ops.export_scene.fbx(filepath=str(MODEL),use_selection=True,object_types={'MESH','EMPTY'},axis_forward='-Z',axis_up='Y',bake_anim=False,add_leaf_bones=False,path_mode='STRIP')
record=dict(source=str(SOURCE.relative_to(ROOT)),sourceSha256=hashlib.sha256(SOURCE.read_bytes()).hexdigest(),
    provenance='Existing project CC0 MakeHuman Raka rig, skin and clothing; no new model artwork',license='CC0-1.0',
    originalTopologyAndUVs=True,conversion='Source rig posed with the existing geometry fitter; existing right forearm/hand faces extracted',
    handsetWidthMetres=width,handsetAspect=1.9,handsetThicknessMetres=thickness,canonicalOrigin='right handset edge',
    maximumSkinPenetrationMetres=max(0,penetration),maximumGlassIntrusionMetres=glass,surfaceSamples=len(canonical_points),fingerSurfaceGapsMetres=gaps,actualContactsMetres=contacts,
    triangles=sum(sum(len(p.vertices)-2 for p in copy.data.polygons) for copy in copies),fit=fit)
(ROOT/'Audit/phase1-sourced-hand.json').write_text(json.dumps(record,indent=2)+'\n')
print('DESIGNER_LANDSCAPE_GRASP',record,flush=True)
