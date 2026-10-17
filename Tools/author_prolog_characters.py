"""Clothed, skinned characters with baked joint animation from pinned CC0 data.

Run fetch_character_sources.py, then blender -b --python this file. MakeHuman
graphical data is adapted here without importing/distributing its program code.
Only face/neck/hands/feet of the body are exported; garments cover the body.
"""
from pathlib import Path
import json
import math
import random
import bpy
import bmesh
from mathutils import Vector, Matrix
from mathutils.kdtree import KDTree

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / 'Builds/ArtSources/MakeHuman'
DEST = ROOT / 'Assets/_LayarTerakhir/Art/Characters'
MODEL = DEST / 'Models'
TEXTURE = DEST / 'Textures'
MODEL.mkdir(parents=True, exist_ok=True)
TEXTURE.mkdir(parents=True, exist_ok=True)
SPEC = json.loads((SOURCE / 'default.mhskel').read_text())
SOURCE_WEIGHTS = json.loads((SOURCE / 'default_weights.mhw').read_text())['weights']
random.seed(1998)


def obj_data(path):
    vertices, uv, faces, groups = [], [], [], []
    group = ''
    for line in path.read_text().splitlines():
        p = line.split()
        if not p:
            continue
        if p[0] == 'v': vertices.append(Vector(map(float, p[1:4])))
        elif p[0] == 'vt': uv.append(tuple(map(float, p[1:3])))
        elif p[0] == 'g': group = p[1]
        elif p[0] == 'f':
            indices = [v.split('/') for v in p[1:]]
            faces.append([(int(v[0])-1, int(v[1])-1 if len(v)>1 and v[1] else 0) for v in indices])
            groups.append(group)
    return vertices, uv, faces, groups


BASE, BASE_UV, BASE_FACES, BASE_GROUPS = obj_data(SOURCE / 'base.obj')


def morph(female):
    vertices = [v.copy() for v in BASE]
    targets = [('asian-female-young.target', .82), ('asian-female-old.target', .18)] if female else [('asian-male-young.target', 1)]
    for name, strength in targets:
        for line in (SOURCE / name).read_text().splitlines():
            p = line.split()
            if p and not p[0].startswith('#'):
                vertices[int(p[0])] += Vector(map(float, p[1:4])) * strength
    body_ids = {i for face, group in zip(BASE_FACES, BASE_GROUPS) if group == 'body' for i, _ in face}
    low = min(vertices[i].y for i in body_ids)
    high = max(vertices[i].y for i in body_ids)
    scale = (1.64 if female else 1.74) / (high-low)
    def convert(v): return Vector((v.x, -v.z, v.y-low)) * scale
    return vertices, [convert(v) for v in vertices], convert


def material(name, color, texture=None, alpha=False):
    mat = bpy.data.materials.new(name)
    mat.diffuse_color = (*color, 1)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = (*color, 1)
    bsdf.inputs['Roughness'].default_value = .68
    if texture:
        image = bpy.data.images.load(str(texture), check_existing=True)
        node = mat.node_tree.nodes.new('ShaderNodeTexImage'); node.image = image
        multiply = mat.node_tree.nodes.new('ShaderNodeMixRGB')
        multiply.blend_type = 'MULTIPLY'; multiply.inputs[0].default_value = 1
        multiply.inputs[2].default_value = (*color, 1)
        mat.node_tree.links.new(node.outputs['Color'], multiply.inputs[1])
        mat.node_tree.links.new(multiply.outputs[0], bsdf.inputs['Base Color'])
        if alpha:
            mat.node_tree.links.new(node.outputs['Alpha'], bsdf.inputs['Alpha'])
            mat.blend_method = 'CLIP'
    return mat


def copy_texture(source, name):
    target = TEXTURE / (name+'.png')
    image = bpy.data.images.load(str(source), check_existing=False)
    longest = max(image.size)
    if longest > 1024: image.scale(round(image.size[0]*1024/longest), round(image.size[1]*1024/longest))
    image.filepath_raw = str(target); image.file_format = 'PNG'; image.save()
    bpy.data.images.remove(image)
    return target


def create_mesh(name, vertices, faces, uv, mat, weights=None):
    data=bpy.data.meshes.new(name); data.from_pydata(vertices,[],[[i for i,_ in face] for face in faces]);data.update()
    obj=bpy.data.objects.new(name,data);bpy.context.collection.objects.link(obj);obj.data.materials.append(mat)
    layer=data.uv_layers.new(name='UVMap')
    for polygon, face in zip(data.polygons,faces):
        polygon.use_smooth=True
        for loop,(_,vt) in zip(polygon.loop_indices,face):layer.data[loop].uv=uv[vt] if uv else (vertices[data.loops[loop].vertex_index][0]*2,vertices[data.loops[loop].vertex_index][2]*2)
    if weights:
        groups={n:obj.vertex_groups.new(name=n) for n in {n for row in weights for n in row}}
        for i,row in enumerate(weights):
            total=sum(row.values())
            for bone,w in row.items():
                if w>.0001:groups[bone].add([i],w/total,'REPLACE')
    # Cropped body surfaces must not retain the unused full-body/helper vertices.
    bm=bmesh.new();bm.from_mesh(data)
    bmesh.ops.delete(bm,geom=[v for v in bm.verts if not v.link_faces],context='VERTS')
    bm.to_mesh(data);bm.free();data.update()
    return obj


def decimate(obj, ratio):
    if ratio>=1:return
    bpy.context.view_layer.objects.active=obj
    modifier=obj.modifiers.new('Budgeted topology','DECIMATE');modifier.ratio=ratio
    modifier.use_collapse_triangulate=True
    bpy.ops.object.modifier_apply(modifier=modifier.name)


def asset_mesh(folder, stem, raw, convert, mat, rig_weights, tree):
    vertices,uv,faces,_=obj_data(folder/(stem+'.obj'))
    mapping=[];scales=[1,1,1];reading=False
    for line in (folder/(stem+'.mhclo')).read_text().splitlines():
        p=line.split()
        if not p or p[0].startswith('#'):continue
        if p[0] in ['x_scale','y_scale','z_scale']:
            axis=['x_scale','y_scale','z_scale'].index(p[0]);scales[axis]=abs(raw[int(p[1])][axis]-raw[int(p[2])][axis])/float(p[3])
        elif p[0]=='verts':reading=True
        elif p[0]=='delete_verts':break
        elif reading and p[0].isdigit():mapping.append(p)
    result=[]
    for row in mapping[:len(vertices)]:
        if len(row)==1:point=raw[int(row[0])]
        else:point=sum((raw[int(row[i])]*float(row[i+3]) for i in range(3)),Vector())+Vector([float(row[6+i])*scales[i] for i in range(3)])
        result.append(convert(point))
    assert len(result)==len(vertices),(stem,len(result),len(vertices))
    weights=[dict(rig_weights[tree.find(v)[1]]) for v in result]
    return create_mesh(stem,result,faces,uv,mat,weights)


def add_armature(verts):
    def joint(key):return sum((verts[i] for i in SPEC['joints'][key]),Vector())/len(SPEC['joints'][key])
    def at(bone,end='head'):return joint(SPEC['bones'][bone][end])
    defs={
        'Hips':(at('root'),at('spine04'),None),
        'Spine':(at('spine04'),at('spine02'),'Hips'),
        'Chest':(at('spine02'),at('neck01'),'Spine'),
        'Neck':(at('neck01'),at('head'),'Chest'),
        'Head':(at('head'),at('head','tail'),'Neck'),
        'Jaw':(at('jaw'),at('jaw','tail'),'Head')}
    direct={'root':'Hips','pelvis.L':'Hips','pelvis.R':'Hips','spine05':'Hips','spine04':'Spine','spine03':'Spine','spine02':'Chest','spine01':'Chest','neck01':'Neck','neck02':'Neck','neck03':'Neck','head':'Head','jaw':'Jaw'}
    for side in ['L','R']:
        suffix='_'+side
        defs.update({
            'UpperArm'+suffix:(at('upperarm01.'+side),at('lowerarm01.'+side),'Chest'),
            'Forearm'+suffix:(at('lowerarm01.'+side),at('wrist.'+side),'UpperArm'+suffix),
            'Hand'+suffix:(at('wrist.'+side),at('finger3-1.'+side),'Forearm'+suffix),
            'Thigh'+suffix:(at('upperleg01.'+side),at('lowerleg01.'+side),'Hips'),
            'Shin'+suffix:(at('lowerleg01.'+side),at('foot.'+side),'Thigh'+suffix),
            'Foot'+suffix:(at('foot.'+side),at('toe3-1.'+side),'Shin'+suffix)})
        for prefix,dest in [('upperarm','UpperArm'),('lowerarm','Forearm'),('wrist','Hand'),('upperleg','Thigh'),('lowerleg','Shin'),('foot','Foot')]:
            for name in SPEC['bones']:
                if name.startswith(prefix) and name.endswith('.'+side):direct[name]=dest+suffix
        for name in SPEC['bones']:
            if name.endswith('.'+side) and name.startswith(('finger','metacarpal')):direct[name]='Hand'+suffix
            if name.endswith('.'+side) and name.startswith('toe'):direct[name]='Foot'+suffix
        for finger in range(1,6):
            for segment in range(1,4):
                old=f'finger{finger}-{segment}.{side}'
                if old not in SPEC['bones']:continue
                name=f'Finger{finger}_{segment}_{side}'
                defs[name]=(at(old),at(old,'tail'),'Hand'+suffix if segment==1 else f'Finger{finger}_{segment-1}_{side}')
                direct[old]=name
    def mapped(name):
        if name in direct:return direct[name]
        parent=SPEC['bones'][name]['parent']
        return mapped(parent) if parent else 'Hips'
    weights=[{} for _ in verts]
    for old,rows in SOURCE_WEIGHTS.items():
        dest=mapped(old)
        for index,weight in rows:
            if index<len(weights):weights[index][dest]=weights[index].get(dest,0)+weight
    for row in weights:
        if not row:row['Hips']=1
    data=bpy.data.armatures.new('CharacterSkeleton');rig=bpy.data.objects.new('CharacterRig',data);bpy.context.collection.objects.link(rig)
    bpy.context.view_layer.objects.active=rig;rig.select_set(True);bpy.ops.object.mode_set(mode='EDIT')
    for name,(head,tail,parent) in defs.items():
        bone=data.edit_bones.new(name);bone.head=head;bone.tail=tail if (tail-head).length>.005 else head+Vector((0,0,.03))
        if parent:bone.parent=data.edit_bones[parent]
    bpy.ops.object.mode_set(mode='OBJECT')
    return rig,weights,defs


def geometry(name, points, faces, mat, bone, rig):
    obj=create_mesh(name,points,[[(i,0) for i in f] for f in faces],[],mat,[{bone:1} for _ in points]);skin(obj,rig);return obj


def skin(obj,rig):
    obj.parent=rig;mod=obj.modifiers.new('Skin deformation','ARMATURE');mod.object=rig


def tube(name, points, widths, mat, bone, rig, sides=8):
    verts=[];faces=[]
    for i,p in enumerate(points):
        p=Vector(p);direction=Vector(points[min(i+1,len(points)-1)])-Vector(points[max(0,i-1)])
        side=direction.normalized().cross(Vector((0,0,1)))
        if side.length<.01:side=Vector((1,0,0))
        side.normalize();other=direction.normalized().cross(side)
        r=widths[i] if isinstance(widths,list) else widths
        for j in range(sides):verts.append(p+(side*math.cos(j*math.tau/sides)+other*math.sin(j*math.tau/sides))*r)
    for i in range(len(points)-1):
        for j in range(sides):
            a=i*sides+j;b=i*sides+(j+1)%sides;faces.append((a,b,b+sides,a+sides))
    return geometry(name,verts,faces,mat,bone,rig)


def set_pose(rig,defs,kind,phase,female):
    for bone in rig.pose.bones:bone.matrix_basis=Matrix.Identity(4)
    crouch='Crouch' in kind;walk='Walk' in kind;run='Run' in kind;phone='Phone' in kind;riding=kind=='Riding';seated=kind=='Seated'
    gait=phase*math.tau
    sink=.61 if crouch else .40 if riding or seated else 0
    bob=(math.cos(gait*2)*(.012 if run else .006) if walk or run else math.sin(gait)*.002)
    hips_offset=Vector((0,0,-sink+bob))
    hips=rig.pose.bones['Hips'];hips.location=hips.bone.matrix_local.to_3x3().inverted()@hips_offset
    lean=math.radians(38 if crouch else 16 if run else 9 if riding else -9 if kind=='Stun' else 3 if female else 1)
    rig.pose.bones['Spine'].rotation_mode='XYZ';rig.pose.bones['Spine'].rotation_euler.x=lean*.5
    rig.pose.bones['Chest'].rotation_mode='XYZ';rig.pose.bones['Chest'].rotation_euler.x=lean*.5
    bpy.context.view_layer.update()
    head=rig.pose.bones['Head'];head.rotation_mode='QUATERNION'
    world_axis=head.bone.matrix_local.to_3x3().inverted()@Vector((0,1,0))
    from mathutils import Quaternion
    head.rotation_quaternion=Quaternion(world_axis,math.radians(76+math.sin(gait)*.6 if female else math.sin(gait)*.4))
    if kind=='Windup':rig.pose.bones['Jaw'].rotation_mode='XYZ';rig.pose.bones['Jaw'].rotation_euler.x=math.radians(18)
    bpy.context.view_layer.update()

    def matrix_pose(name,at,direction,twist=0):
        bone=rig.pose.bones[name];rest=bone.bone
        rotation=(rest.tail_local-rest.head_local).rotation_difference(direction.normalized())
        matrix=Matrix.Translation(at)@rotation.to_matrix().to_4x4()@rest.matrix_local.to_quaternion().to_matrix().to_4x4()
        bone.matrix=matrix

    def limb(upper,lower,end,target,pole):
        bpy.context.view_layer.update();a=rig.pose.bones[upper].head.copy()
        l1=rig.data.bones[upper].length;l2=rig.data.bones[lower].length
        direction=target-a;distance=min(max(direction.length,.01),l1+l2-.001);axis=direction.normalized()
        projection=min(l1,max(-l1,(distance*distance+l1*l1-l2*l2)/(2*distance)))
        perpendicular=pole-a-axis*(pole-a).dot(axis)
        if perpendicular.length<.001:perpendicular=axis.cross(Vector((1,0,0)))
        knee=a+axis*projection+perpendicular.normalized()*math.sqrt(max(0,l1*l1-projection*projection))
        matrix_pose(upper,a,knee-a);bpy.context.view_layer.update();matrix_pose(lower,knee,target-knee);bpy.context.view_layer.update()
        if end.startswith('Foot'):
            rest=rig.data.bones[end];matrix_pose(end,target,rest.tail_local-rest.head_local)
        else:
            rest=rig.data.bones[end];matrix_pose(end,target,(Vector((0,-.03,.08)) if phone else Vector((0,-.02,-.09))))

    for side,sign in [('L',1),('R',-1)]:
        angle=gait+(0 if side=='L' else math.pi)
        foot=defs['Foot_'+side][0].copy()
        if walk or run:
            amplitude=.16 if crouch else .50 if run else .28
            foot.y+=math.cos(angle)*amplitude;foot.z+=max(0,math.sin(angle))*(.12 if run else .04 if crouch else .07)
        if crouch:foot.x=sign*.15;foot.y-=.06
        if riding or seated:foot=Vector((sign*.17,-.36,.30 if riding else .08))
        limb('Thigh_'+side,'Shin_'+side,'Foot_'+side,foot,Vector((sign*.18,-1.2,.65-sink*.2)))
        bpy.context.view_layer.update();shoulder=rig.pose.bones['UpperArm_'+side].head.copy()
        hand=Vector((shoulder.x+sign*.045,shoulder.y-.035,.83-sink))
        if walk or run:hand.y+=math.cos(angle)*(.22 if run else .11);hand.z+=.04
        if phone and side=='R':hand=Vector((-.19,-.34,1.31-sink*.85))
        if crouch and side=='L':hand=Vector((.29,-.28,.64))
        if riding:hand=Vector((sign*.30,-.60,1.02))
        if seated:hand=Vector((sign*.18,-.27,.75))
        if female and kind=='Windup':hand=Vector((-.22,-.38,1.69)) if side=='R' else Vector((.32,-.34,1.17))
        if female and kind=='Stun':hand=Vector((.19,-.13,1.48)) if side=='L' else Vector((-.29,-.06,.89))
        limb('UpperArm_'+side,'Forearm_'+side,'Hand_'+side,hand,Vector((sign*.60,-.01,1.05-sink*.5)))
        grip=(phone and side=='R') or (female and side=='R') or riding
        for finger in range(1,6):
            for segment in range(1,4):
                bone=rig.pose.bones.get(f'Finger{finger}_{segment}_{side}')
                if bone:
                    bone.rotation_mode='XYZ';bone.rotation_euler.x=math.radians((22 if finger==1 else 48) if grip else 8)
    bpy.context.view_layer.update()


def actions(rig,defs,female):
    specifications=[('Idle',2.8),('Walk',1.2 if female else .85),('Run',.65),('Windup',.8),('Stun',1.2)] if female else [
        ('Idle',2.8),('Walk',.85),('Run',.65),('Crouch',2.0),('CrouchWalk',.85),
        ('PhoneIdle',2.8),('PhoneWalk',.85),('PhoneRun',.65),('PhoneCrouch',2.0),('PhoneCrouchWalk',.85),('Riding',2.0)]
    for name,duration in specifications:
        action=bpy.data.actions.new(name);action.use_fake_user=True;rig.animation_data_create();rig.animation_data.action=action
        count=round(duration*24)
        for frame in range(count+1):
            bpy.context.scene.frame_set(frame+1);set_pose(rig,defs,name,frame/max(1,count),female)
            for bone in rig.pose.bones:
                bone.keyframe_insert('location',frame=frame+1,group=bone.name)
                bone.keyframe_insert('rotation_quaternion' if bone.rotation_mode=='QUATERNION' else 'rotation_euler',frame=frame+1,group=bone.name)
        for curve in action.fcurves:
            for key in curve.keyframe_points:key.interpolation='LINEAR'
    rig.animation_data.action=bpy.data.actions['Idle'];bpy.context.scene.frame_set(1)
    return [n for n,_ in specifications]


def make_character(female):
    bpy.ops.wm.read_factory_settings(use_empty=True);bpy.context.scene.render.fps=24
    raw,vertices,convert=morph(female);rig,weights,defs=add_armature(vertices)
    tree=KDTree(len(vertices))
    for i,v in enumerate(vertices):tree.insert(v,i)
    tree.balance()
    skin_path=copy_texture(SOURCE/'system/skins'/('middleage_asian_female/middleage_lightskinned_female_diffuse2.png' if female else 'young_asian_male/young_lightskinned_male_diffuse3.png'),'SriSkin' if female else 'RakaSkin')
    hair_name='long01' if female else 'short04'
    hair_path=copy_texture(SOURCE/'system/hair'/hair_name/(hair_name+'_diffuse.png'),'SriHair' if female else 'RakaHair')
    eye_path=copy_texture(SOURCE/'system/eyes/materials/brown_eye.png','BrownEyes')
    mats={
        'Skin':material('Sri_Skin' if female else 'Raka_Skin',(.76,.80,.75) if female else (1,1,1),skin_path),
        'Hair':material('Sri_Hair' if female else 'Raka_Hair',(.05,.045,.04),hair_path,True),
        'Eyes':material('Sri_Eye' if female else 'Raka_Eye',(.65,.72,.64) if female else (1,1,1),eye_path)}
    exposed_bones=['Head','Neck','Jaw','Hand_L','Hand_R']+[name for name in defs if name.startswith('Finger')]
    visible=[]
    for face,group in zip(BASE_FACES,BASE_GROUPS):
        if group!='body':continue
        center=sum((vertices[i] for i,_ in face),Vector())/len(face)
        influences={n:sum(weights[i].get(n,0) for i,_ in face)/len(face) for n in exposed_bones}
        if sum(influences.values())>.26 or female and center.z<.20:visible.append(face)
    body=create_mesh('VisibleSkin',vertices,visible,BASE_UV,mats['Skin'],weights);decimate(body,.50);skin(body,rig)
    eyes=asset_mesh(SOURCE/'system/eyes/low-poly','low-poly',raw,convert,mats['Eyes'],weights,tree);skin(eyes,rig)
    hair=asset_mesh(SOURCE/'system/hair'/hair_name,hair_name,raw,convert,mats['Hair'],weights,tree)
    decimate(hair,.80)
    for group in list(hair.vertex_groups):hair.vertex_groups.remove(group)
    hair.vertex_groups.new(name='Head').add(list(range(len(hair.data.vertices))),1,'REPLACE')
    if female:
        pivot=defs['Head'][0];tilt=Matrix.Translation(pivot)@Matrix.Rotation(math.radians(76),4,'Y')@Matrix.Translation(-pivot)
        for vertex in hair.data.vertices:
            p=vertex.co.copy();blend=max(0,min(1,(p.z-1.30)/.28));desired=p.lerp(tilt@p,blend)
            vertex.co=tilt.inverted()@desired
    skin(hair,rig)
    if female:
        cloth_path=TEXTURE/'SriKebayaLace-v1.png'
        assert cloth_path.exists(),'Generate/copy the recorded original lace texture before authoring'
        skirt_path=copy_texture(ROOT/'Assets/_LayarTerakhir/Art/Imported/PolyHaven/quatrefoil_jacquard_fabric/quatrefoil_jacquard_fabric_Diffuse_1k.jpg','SriSkirtJacquard')
        cloth=material('Sri_Cloth',(.83,.81,.75),cloth_path);batik=material('Sri_Batik',(.16,.095,.055),skirt_path)
        garment=[]
        for face,group in zip(BASE_FACES,BASE_GROUPS):
            if group!='body':continue
            center=sum((vertices[i] for i,_ in face),Vector())/len(face)
            excluded=sum(sum(weights[i].get(n,0) for n in exposed_bones) for i,_ in face)/len(face)
            arm=sum(sum(weights[i].get(n,0) for n in ['UpperArm_L','UpperArm_R','Forearm_L','Forearm_R']) for i,_ in face)/len(face)
            if arm>.5 and excluded<.34:garment.append(face)
        blouse=create_mesh('KebayaSleeves',vertices,garment,BASE_UV,cloth,weights)
        bpy.context.view_layer.objects.active=blouse
        smooth=blouse.modifiers.new('Tailored cloth surface','SMOOTH');smooth.factor=.72;smooth.iterations=16
        bpy.ops.object.modifier_apply(modifier=smooth.name)
        blouse.data.update()
        for v in blouse.data.vertices:v.co+=v.normal*.014
        for loop in blouse.data.loops:
            v=blouse.data.vertices[loop.vertex_index].co
            blouse.data.uv_layers.active.data[loop.index].uv=(v.x*3,v.z*3)
        decimate(blouse,.45);skin(blouse,rig)
        # Independent, backed torso panels: a modest garment, not inflated body topology.
        rows=[(.85,.175,.105),(.94,.168,.105),(1.04,.163,.108),(1.15,.194,.125),
              (1.25,.213,.120),(1.30,.214,.113),(1.34,.165,.090),(1.38,.100,.069),(1.42,.066,.060)]
        points=[];faces=[];panel_weights=[]
        for z,width,depth in rows:
            for col in range(40):
                a=col/40*math.tau;p=Vector((math.cos(a)*width,math.sin(a)*depth,z))
                points.append(p);panel_weights.append(dict(weights[tree.find(p)[1]]))
        for row in range(len(rows)-1):
            for col in range(40):
                a=row*40+col;b=row*40+(col+1)%40
                faces.append([(a,0),(b,0),(b+40,0),(a+40,0)])
        torso=create_mesh('TailoredIvoryKebaya',points,faces,[],cloth,panel_weights);skin(torso,rig)
        points=[];faces=[];skirt_weights=[]
        for row in range(13):
            z=.08+row/12*.87;radius=.23-.075*(row/12)
            for col in range(32):
                a=col/32*math.tau;fold=1+.035*math.sin(a*13+row*.25)
                p=Vector((math.cos(a)*radius*fold,math.sin(a)*radius*.73*fold,z));points.append(p)
                skirt_weights.append({'Hips':.6,'Thigh_L':.2+max(0,p.x)*.5,'Thigh_R':.2+max(0,-p.x)*.5})
        for row in range(12):
            for col in range(32):a=row*32+col;b=row*32+(col+1)%32;faces.append([(a,0),(b,0),(b+32,0),(a+32,0)])
        skirt=create_mesh('LongBatikSkirt',points,faces,[],batik,skirt_weights);skin(skirt,rig)
        rust=material('Sri_Rust',(.17,.075,.035));seam=material('Sri_Seam',(.24,.20,.15))
        tube('KebayaSeam',[(0,-.132,.88),(0,-.132,1.05),(0,-.15,1.23),(0,-.08,1.39)],.0035,seam,'Chest',rig,5)
        hand=defs['Hand_R'][0];direction=(defs['Hand_R'][1]-hand).normalized()
        # A real slender tool, attached to the palm instead of covering the camera with a mitten.
        tube('Shears', [hand+direction*.055,hand+direction*.26,hand+direction*.61+Vector((0,-.04,0))],[.012,.016,.002],rust,'Hand_R',rig,8)
        tube('ShearsSecondBlade',[hand+direction*.055+Vector((0,.04,0)),hand+direction*.26,hand+direction*.61+Vector((0,.04,0))],[.012,.016,.002],rust,'Hand_R',rig,8)
    else:
        clothes_path=copy_texture(SOURCE/'system/clothes/male_casualsuit05/male_casualsuit05_diffuse.png','RakaClothes')
        normal_path=copy_texture(SOURCE/'system/clothes/male_casualsuit05/male_casualsuit05_normal.png','RakaClothesNormal')
        outfit=asset_mesh(SOURCE/'system/clothes/male_casualsuit05','male_casualsuit05',raw,convert,material('Raka_Clothes',(1,1,1),clothes_path),weights,tree);decimate(outfit,.7);skin(outfit,rig)
        shoes_path=copy_texture(SOURCE/'system/clothes/shoes01/shoes01_diffuse.png','RakaShoes')
        shoes=asset_mesh(SOURCE/'system/clothes/shoes01','shoes01',raw,convert,material('Raka_Shoes',(1,1,1),shoes_path),weights,tree);decimate(shoes,.45);skin(shoes,rig)
        # World handset is visible only in phone poses, managed by AnimatedRakaAvatar.
        phone=material('Raka_Phone',(.018,.02,.023));hand=defs['Hand_R'][0]
        basis=rig.data.bones['Hand_R'].matrix_local.to_3x3()
        points=[hand+basis@Vector((dx,dy,dz)) for dy in [-.02,.12] for dz in [-.008,.008] for dx in [-.035,.035]]
        geometry('HandheldPhone',points,[(0,1,3,2),(4,6,7,5),(0,4,5,1),(2,3,7,6),(0,2,6,4),(1,5,7,3)],phone,'Hand_R',rig)
    clips=actions(rig,defs,female)
    meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
    triangles=sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in meshes)
    name='SriAtmajaRigged' if female else 'RakaRigged'
    bpy.ops.object.select_all(action='DESELECT');rig.select_set(True)
    for mesh in meshes:mesh.select_set(True)
    bpy.context.view_layer.objects.active=rig
    bpy.ops.export_scene.fbx(filepath=str(MODEL/(name+'.fbx')),use_selection=True,object_types={'MESH','ARMATURE'},axis_forward='-Z',axis_up='Y',bake_anim=True,bake_anim_use_all_actions=True,bake_anim_use_nla_strips=False,bake_anim_simplify_factor=.25,add_leaf_bones=False,path_mode='STRIP')
    records=dict(character=name,triangles=triangles,renderers=len(meshes),bones=len(rig.data.bones),clips=clips)
    records['poseBoundsMetres']={}
    rig.animation_data.action=None
    for pose in (['Idle','Run','Seated'] if female else ['Idle','Run','Crouch','CrouchWalk','PhoneCrouch','Riding']):
        set_pose(rig,defs,pose,0,female);deps=bpy.context.evaluated_depsgraph_get()
        points=[o.matrix_world@Vector(corner) for original in meshes for o in [original.evaluated_get(deps)] for corner in o.bound_box]
        records['poseBoundsMetres'][pose]=dict(min=[round(min(p[i] for p in points),4) for i in range(3)],max=[round(max(p[i] for p in points),4) for i in range(3)])
    if female:
        for variant,pose in [('SriAtmajaEcho','Idle'),('SriAtmajaSeated','Seated')]:
            rig.animation_data.action=None;set_pose(rig,defs,pose,0,True);bpy.context.view_layer.update()
            deps=bpy.context.evaluated_depsgraph_get();copies=[]
            for original in meshes:
                data=bpy.data.meshes.new_from_object(original.evaluated_get(deps),depsgraph=deps)
                copy=bpy.data.objects.new('Echo_'+original.name,data);bpy.context.collection.objects.link(copy);copies.append(copy)
                for mat in original.data.materials:
                    if mat.name not in [m.name for m in data.materials]:data.materials.append(mat)
                decimate(copy,.15)
            bpy.ops.object.select_all(action='DESELECT')
            for copy in copies:copy.select_set(True)
            bpy.context.view_layer.objects.active=copies[0]
            bpy.ops.export_scene.fbx(filepath=str(MODEL/(variant+'.fbx')),use_selection=True,object_types={'MESH'},axis_forward='-Z',axis_up='Y',bake_anim=False,path_mode='STRIP')
            records[variant+'Triangles']=sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in copies)
            for copy in copies:bpy.data.objects.remove(copy,do_unlink=True)
    print('SKINNED_CHARACTER_AUTHORED',records,flush=True)
    rig.animation_data.action=None;set_pose(rig,defs,'Idle',0,female)
    if not female:bpy.data.objects['HandheldPhone'].hide_render=True
    bpy.context.scene.render.engine='BLENDER_EEVEE';bpy.context.scene.eevee.use_gtao=True
    bpy.context.scene.eevee.use_soft_shadows=True;bpy.context.scene.eevee.taa_render_samples=64
    bpy.context.scene.world=bpy.data.worlds.new('PreviewWorld');bpy.context.scene.world.color=(.055,.055,.055)
    bpy.ops.mesh.primitive_plane_add(size=200,location=(0,0,-.025));bpy.context.object.data.materials.append(material('PreviewGround',(.07,.075,.08)))
    for at,power,size in [((-2,-3,4),230,2.0),((2,-1,2.5),80,1.8),((0,2,3),160,1.2)]:
        data=bpy.data.lights.new('PreviewLight','AREA');data.energy=power;data.size=size
        obj=bpy.data.objects.new('PreviewLight',data);bpy.context.collection.objects.link(obj);obj.location=at;obj.rotation_euler=(Vector((0,0,1))-obj.location).to_track_quat('-Z','Y').to_euler()
    camera_data=bpy.data.cameras.new('PreviewCamera');camera=bpy.data.objects.new('PreviewCamera',camera_data);bpy.context.collection.objects.link(camera)
    camera.location=(1.1,-3.7,1.8);camera.rotation_euler=(Vector((0,0,.95))-camera.location).to_track_quat('-Z','Y').to_euler();camera_data.lens=58
    scene=bpy.context.scene;scene.camera=camera;scene.render.resolution_x=768;scene.render.resolution_y=1024;scene.render.resolution_percentage=100
    review=ROOT/'Audit/Visuals/Character-Authoring';review.mkdir(parents=True,exist_ok=True)
    scene.render.image_settings.file_format='PNG';scene.render.filepath=str(review/(name+'.png'));bpy.ops.render.render(write_still=True)
    return records


report=[make_character(True),make_character(False)]
(ROOT/'Audit/rigged-character-authoring.json').write_text(json.dumps(dict(sourceManifest='Audit/character-source-manifest.json',generatedCloth='Audit/character-generated-cloth.json',method='CC0 MakeHuman graphical data adapted into clothed, budgeted skinned meshes; original baked joint animation and garment/tool geometry; generated opaque ivory lace and CC0 PolyHaven jacquard',characters=report),indent=2)+'\n')
