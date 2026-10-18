"""Healthy, fully clothed Bu Sri; original mesh adaptation of pinned CC0 sources."""
import json
import bpy
from mathutils import Vector
from mathutils.kdtree import KDTree
import author_prolog_characters as character

bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.context.scene.render.fps=24
raw,vertices,convert=character.morph(True)
rig,weights,defs=character.add_armature(vertices,True)
tree=KDTree(len(vertices))
for i,v in enumerate(vertices):tree.insert(v,i)
tree.balance()
skin=character.material('VendorSkin',(1,1,1),character.TEXTURE/'SriSkin.png')
hair=character.material('VendorHair',(.13,.10,.08),character.TEXTURE/'SriHair.png',True)
eye=character.material('VendorEyes',(1,1,1),character.TEXTURE/'BrownEyes.png')
blouse=character.material('VendorBlouse',(.38,.49,.36))
batik=character.material('VendorBatik',(.22,.27,.34),character.TEXTURE/'SriSkirtJacquard.png')
bodyfaces=[];clothfaces=[]
for face,group in zip(character.BASE_FACES,character.BASE_GROUPS):
    if group!='body':continue
    center=sum((vertices[i] for i,_ in face),Vector())/len(face)
    hands=sum(sum(weights[i].get(n,0) for n in defs if n.startswith(('Hand','Finger'))) for i,_ in face)/len(face)
    arms=sum(sum(weights[i].get(n,0) for n in ['UpperArm_L','UpperArm_R','Forearm_L','Forearm_R']) for i,_ in face)/len(face)
    if center.z>1.30 or hands>.15 or center.z<.18:bodyfaces.append(face)
    if .83<center.z<1.39 and hands<.25 or arms>.55 and hands<.30:clothfaces.append(face)
body=character.create_mesh('BuSriVisibleSkin',vertices,bodyfaces,character.BASE_UV,skin,weights);character.skin(body,rig)
top=character.create_mesh('BuSriBlouse',vertices,clothfaces,character.BASE_UV,blouse,weights)
for v in top.data.vertices:v.co+=v.normal*.018
character.skin(top,rig)
for folder,stem,mat in [('eyes/low-poly','low-poly',eye),('hair/long01','long01',hair)]:
    mesh=character.asset_mesh(character.SOURCE/'system'/folder,stem,raw,convert,mat,weights,tree)
    character.skin(mesh,rig)
points=[];faces=[];bindings=[]
for row in range(15):
    z=.13+row/14*.81;radius=.26-.08*row/14
    for col in range(40):
        angle=col/40*character.math.tau
        p=Vector((character.math.cos(angle)*radius,character.math.sin(angle)*radius*.76,z));points.append(p)
        bindings.append({'Hips':1})
for row in range(14):
    for col in range(40):
        a=row*40+col;b=row*40+(col+1)%40;faces.append([(a,0),(b,0),(b+40,0),(a+40,0)])
skirt=character.create_mesh('BuSriLongSkirt',points,faces,[],batik,bindings);character.skin(skirt,rig)
action=bpy.data.actions.new('Idle');rig.animation_data_create();rig.animation_data.action=action
for frame in range(73):
    bpy.context.scene.frame_set(frame+1);character.set_pose(rig,defs,'Idle',frame/72,False)
    for bone in rig.pose.bones:
        bone.keyframe_insert('location',frame=frame+1)
        bone.keyframe_insert('rotation_quaternion' if bone.rotation_mode=='QUATERNION' else 'rotation_euler',frame=frame+1)
bpy.ops.object.select_all(action='SELECT')
bpy.context.view_layer.objects.active=rig
path=character.MODEL/'BuSriVendor.fbx'
bpy.ops.export_scene.fbx(filepath=str(path),use_selection=True,object_types={'MESH','ARMATURE'},axis_forward='-Z',axis_up='Y',bake_anim=True,bake_anim_use_all_actions=True,add_leaf_bones=False,path_mode='STRIP')
(character.ROOT/'Audit/bu-sri-vendor-authoring.json').write_text(json.dumps({'source':'Audit/character-source-manifest.json','model':str(path.relative_to(character.ROOT)),'healthyHeadPose':True,'weapon':False,'floorLengthSkirt':True},indent=2)+'\n')
