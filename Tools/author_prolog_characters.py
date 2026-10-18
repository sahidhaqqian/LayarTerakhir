"""Clothed, skinned characters with baked joint animation from pinned CC0 data.

Run fetch_character_sources.py, then blender -b --python this file. MakeHuman
graphical data is adapted here without importing/distributing its program code.
Only face/neck/hands/feet of the body are exported; garments cover the body.
"""
from pathlib import Path
import json
import math
import random
import sys
import bpy
import bmesh
from mathutils import Vector, Matrix, Quaternion
from mathutils.kdtree import KDTree

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / 'Builds/ArtSources/MakeHuman'
DEST = ROOT / 'Assets/_LayarTerakhir/Art/Characters'
MODEL = DEST / 'Models'
TEXTURE = DEST / 'Textures'
sys.path.insert(0,str(ROOT/'Tools'))
MODEL.mkdir(parents=True, exist_ok=True)
TEXTURE.mkdir(parents=True, exist_ok=True)
SPEC = json.loads((SOURCE / 'default.mhskel').read_text())
SOURCE_WEIGHTS = json.loads((SOURCE / 'default_weights.mhw').read_text())['weights']
random.seed(1998)
NERA_PALMS={}


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


def add_armature(verts,female=False):
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
    if not female:
        # Add the optional humanoid joints used by the designer's VRM/VRMA convention.
        defs['UpperChest']=(at('spine01'),at('neck01'),'Chest')
        defs['Neck']=(defs['Neck'][0],defs['Neck'][1],'UpperChest')
        for side in ['L','R']:
            defs['Shoulder_'+side]=(at('clavicle.'+side),at('upperarm01.'+side),'UpperChest')
            a,b,_=defs['UpperArm_'+side];defs['UpperArm_'+side]=(a,b,'Shoulder_'+side)
            defs['Toes_'+side]=(at('toe3-1.'+side),at('toe3-1.'+side,'tail'),'Foot_'+side)
            eye=at('eye.'+side) if 'eye.'+side in SPEC['bones'] else defs['Head'][0]+Vector((.03 if side=='L' else -.03,-.075,.07))
            defs['Eye_'+side]=(eye,eye+Vector((0,-.02,0)),'Head')
        from nera_character_adapter import adapt
        hand_report=adapt(verts,[f for f,g in zip(BASE_FACES,BASE_GROUPS) if g=='body'],defs,weights,ROOT/'Audit/nera-raka-hand-adaptation.json')
        for side,suffix in [('left','L'),('right','R')]:NERA_PALMS[suffix]=Vector(hand_report['sides'][side]['gamePalmNormalBlender'])
    data=bpy.data.armatures.new('CharacterSkeleton');rig=bpy.data.objects.new('CharacterRig',data);bpy.context.collection.objects.link(rig)
    bpy.context.view_layer.objects.active=rig;rig.select_set(True);bpy.ops.object.mode_set(mode='EDIT')
    for name,(head,tail,parent) in defs.items():
        bone=data.edit_bones.new(name);bone.head=head;bone.tail=tail if (tail-head).length>.005 else head+Vector((0,0,.03))
    for name,(_,_,parent) in defs.items():
        if parent:data.edit_bones[name].parent=data.edit_bones[parent]
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


def hand_frame(defs,side):
    longitudinal=(defs['Hand_'+side][1]-defs['Hand_'+side][0]).normalized()
    across=defs['Finger2_1_'+side][0]-defs['Finger5_1_'+side][0]
    palm=NERA_PALMS.get(side,across.cross(longitudinal).normalized())
    palm=(palm-longitudinal*palm.dot(longitudinal)).normalized()
    if side not in NERA_PALMS and palm.y>0:palm=-palm
    return longitudinal,palm,longitudinal.cross(palm).normalized()


def set_pose(rig,defs,kind,phase,female):
    for bone in rig.pose.bones:bone.matrix_basis=Matrix.Identity(4)
    installing=kind=='InstallCard'
    crouch='Crouch' in kind or installing;walk='Walk' in kind;run='Run' in kind;phone='Phone' in kind or installing;riding=kind=='Riding';seated=kind=='Seated'
    gait=phase*math.tau
    sink=.48 if crouch else .40 if riding or seated else .09 if run else .03 if walk else 0
    bob=(math.cos(gait*2)*(.012 if run else .006) if walk or run else math.sin(gait)*.002)
    hips_offset=Vector((0,0,-sink+bob))
    hips=rig.pose.bones['Hips'];hips.location=hips.bone.matrix_local.to_3x3().inverted()@hips_offset
    lean=math.radians(29 if crouch else 16 if run else 17 if riding else -9 if kind=='Stun' else 3 if female else 1)
    rig.pose.bones['Spine'].rotation_mode='XYZ';rig.pose.bones['Spine'].rotation_euler.x=lean*.5
    rig.pose.bones['Chest'].rotation_mode='XYZ';rig.pose.bones['Chest'].rotation_euler.x=lean*.5
    if not female and (walk or run):
        sway=math.sin(gait)*(.035 if run else .024)
        rig.pose.bones['Hips'].rotation_mode='XYZ';rig.pose.bones['Hips'].rotation_euler.z=sway
        rig.pose.bones['Chest'].rotation_euler.z=-sway*.8
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
        target=a+axis*distance
        projection=min(l1,max(-l1,(distance*distance+l1*l1-l2*l2)/(2*distance)))
        perpendicular=pole-a-axis*(pole-a).dot(axis)
        if perpendicular.length<.001:perpendicular=axis.cross(Vector((1,0,0)))
        knee=a+axis*projection+perpendicular.normalized()*math.sqrt(max(0,l1*l1-projection*projection))
        matrix_pose(upper,a,knee-a);bpy.context.view_layer.update();matrix_pose(lower,knee,target-knee);bpy.context.view_layer.update()
        if end.startswith('Foot'):
            rest=rig.data.bones[end];matrix_pose(end,target,rest.tail_local-rest.head_local)
        else:
            rest=rig.data.bones[end]
            if phone and end=='Hand_R':
                # Solve palm roll as well as wrist direction: the screen faces Raka,
                # with fingers upright and no hyperextended, downward-facing wrist.
                longitudinal,normal,sideways=hand_frame(defs,'R')
                up=Vector((0,-.08,1)).normalized();palm=Vector((0,1,.08)).normalized()
                source=Matrix((sideways,longitudinal,normal)).transposed()
                destination=Matrix((up.cross(palm),up,palm)).transposed()
                rotation=destination@source.transposed()
                rig.pose.bones[end].matrix=Matrix.Translation(target)@rotation.to_4x4()@rest.matrix_local.to_quaternion().to_matrix().to_4x4()
            elif riding:matrix_pose(end,target,Vector((0,-.09,0)))
            else:matrix_pose(end,target,Vector((0,-.02,-.09)))

    for side,sign in [('L',1),('R',-1)]:
        angle=gait+(0 if side=='L' else math.pi)
        foot=defs['Foot_'+side][0].copy()
        if walk or run:
            amplitude=.25 if crouch else .57 if run else .35
            cycle=(phase+(0 if side=='L' else .5))%1
            stance=.58 if not run else .46
            if cycle<stance:
                foot.y+=amplitude*(-1+2*cycle/stance)
            else:
                swing=(cycle-stance)/(1-stance)
                foot.y+=amplitude*(1-2*(swing*swing*(3-2*swing)))
                foot.z+=math.sin(swing*math.pi)*(.14 if run else .045 if crouch else .085)
        if crouch:foot.x=sign*.13;foot.y-=.14
        if riding or seated:foot=Vector((sign*.23,-.20,.27 if riding else .08))
        limb('Thigh_'+side,'Shin_'+side,'Foot_'+side,foot,Vector((sign*.18,-1.2,.65-sink*.2)))
        bpy.context.view_layer.update();shoulder=rig.pose.bones['UpperArm_'+side].head.copy()
        hand=Vector((shoulder.x+sign*.045,shoulder.y-.035,.83-sink))
        if walk or run:hand.y+=math.sin(angle)*(.22 if run else .13);hand.z+=.04
        if phone and side=='R':hand=Vector((-.19,-.34,1.31-sink*.85))
        if crouch and side=='L':hand=Vector((.22,-.34,.63))
        if installing and side=='L':
            reach=math.sin(min(1,phase*2.8)*math.pi*.5)
            hand=Vector((.29,-.28,.64)).lerp(Vector((-.045,-.40,.88)),reach)
        if riding:hand=Vector((sign*.245,-.49,1.05))
        if seated:hand=Vector((sign*.18,-.27,.75))
        if female and kind=='Windup':hand=Vector((-.22,-.38,1.69)) if side=='R' else Vector((.32,-.34,1.17))
        if female and kind=='Stun':hand=Vector((.19,-.13,1.48)) if side=='L' else Vector((-.29,-.06,.89))
        limb('UpperArm_'+side,'Forearm_'+side,'Hand_'+side,hand,Vector((sign*.36,-.16,.94-sink*.35)))
        grip=(phone and side=='R') or (female and side=='R') or riding
        longitudinal,palm,sideways=hand_frame(defs,side)
        for finger in range(1,6):
            for segment in range(1,4):
                bone=rig.pose.bones.get(f'Finger{finger}_{segment}_{side}')
                if bone:
                    # MakeHuman finger rolls differ. Flex around each joint's own
                    # knuckle axis towards the palm, never an arbitrary Euler X.
                    direction=bone.bone.tail_local-bone.bone.head_local
                    axis=direction.normalized().cross(palm).normalized()
                    local_axis=bone.bone.matrix_local.to_quaternion().inverted()@axis
                    degrees=([14,26,16] if finger==1 else [8,18,12] if phone else [18,48,22])[segment-1] if grip else [5,9,5][segment-1]
                    bone.rotation_mode='QUATERNION';bone.rotation_quaternion=Quaternion(local_axis,math.radians(degrees))
                    if phone and side=='R' and finger==1 and segment==1:
                        # Oppose the thumb along the handset edge instead of leaving
                        # the rest-pose thumb splayed out beside an otherwise closed grip.
                        thumb_side=1 if (bone.bone.head_local-defs['Hand_R'][0]).dot(sideways)>0 else -1
                        desired=(longitudinal*.8+palm*.5-sideways*thumb_side*.25).normalized()
                        rest=bone.bone.matrix_local.to_quaternion()
                        bone.rotation_quaternion=rest.inverted()@direction.normalized().rotation_difference(desired)@rest
    bpy.context.view_layer.update()


def actions(rig,defs,female):
    specifications=[('Idle',2.8),('Walk',1.2 if female else .85),('Run',.65),('Windup',.8),('Stun',1.2)] if female else [
        ('Idle',2.8),('Walk',.85),('Run',.65),('Crouch',2.0),('CrouchWalk',.85),
        ('PhoneIdle',2.8),('PhoneWalk',.85),('PhoneRun',.65),('PhoneCrouch',2.0),('PhoneCrouchWalk',.85),('Riding',2.0),('InstallCard',6.8)]
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
    raw,vertices,convert=morph(female);rig,weights,defs=add_armature(vertices,female)
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
        if (not female and center.z>1.20) or sum(influences.values())>.26 or female and center.z<.20 or not female and sum(sum(weights[i].get(n,0) for n in ['UpperArm_L','UpperArm_R','Forearm_L','Forearm_R']) for i,_ in face)/len(face)>.35:visible.append(face)
    body=create_mesh('VisibleSkin',vertices,visible,BASE_UV,mats['Skin'],weights);decimate(body,.50 if female else 1.0);skin(body,rig)
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
        outfit=asset_mesh(SOURCE/'system/clothes/male_casualsuit05','male_casualsuit05',raw,convert,material('Raka_Clothes',(1,1,1),clothes_path),weights,tree)
        # The source contains four disconnected sewn garments. Split topology,
        # not a horizontal plane through the waistband and jacket tails.
        bm=bmesh.new();bm.from_mesh(outfit.data);bm.verts.ensure_lookup_table()
        remaining=set(bm.verts);components=[]
        while remaining:
            seed=remaining.pop();component={seed};pending=[seed]
            while pending:
                current=pending.pop()
                for edge in current.link_edges:
                    other=edge.other_vert(current)
                    if other in remaining:remaining.remove(other);component.add(other);pending.append(other)
            components.append(component)
        pants=min(components,key=lambda vs:min(v.co.z for v in vs))
        pants_indices={v.index for v in pants}
        bm.free()
        trousers=outfit.copy();trousers.data=outfit.data.copy();bpy.context.collection.objects.link(trousers)
        for obj,keep_pants in [(outfit,False),(trousers,True)]:
            bm=bmesh.new();bm.from_mesh(obj.data);bm.verts.ensure_lookup_table()
            bmesh.ops.delete(bm,geom=[v for v in bm.verts if (v.index in pants_indices)!=keep_pants],context='VERTS')
            bm.to_mesh(obj.data);bm.free();decimate(obj,.85);skin(obj,rig)
        outfit.name='RakaJacketOutfit';trousers.name='RakaTrousers'
        shirt_faces=[]
        for face,group in zip(BASE_FACES,BASE_GROUPS):
            if group!='body':continue
            c=sum((vertices[i] for i,_ in face),Vector())/len(face)
            arm=sum(sum(weights[i].get(n,0) for n in ['UpperArm_L','UpperArm_R']) for i,_ in face)/len(face)
            if .90<c.z<1.48 and abs(c.x)<.25 or arm>.6 and c.z>1.25:shirt_faces.append(face)
        tshirt=create_mesh('RakaTShirt',vertices,shirt_faces,BASE_UV,material('Raka_TShirt',(.09,.12,.15)),weights)
        bm=bmesh.new();bm.from_mesh(tshirt.data)
        bmesh.ops.bisect_plane(bm,geom=list(bm.verts)+list(bm.edges)+list(bm.faces),plane_co=Vector((0,0,.965)),plane_no=Vector((0,0,1)),clear_inner=True)
        bmesh.ops.bisect_plane(bm,geom=list(bm.verts)+list(bm.edges)+list(bm.faces),plane_co=Vector((0,0,1.445)),plane_no=Vector((0,0,1)),clear_outer=True)
        bm.to_mesh(tshirt.data);bm.free();tshirt.data.update()
        pant_waist=[v.co.copy() for v in trousers.data.vertices if .84<v.co.z<1.10]
        for v in tshirt.data.vertices:
            v.co+=v.normal*.018
            if v.co.z<1.12:
                radial=Vector((v.co.x,v.co.y,0));direction=radial.normalized()
                envelope=max(p.dot(direction) for p in pant_waist)+.018
                blend=max(0,min(1,(1.12-v.co.z)/.08))
                desired=radial.lerp(direction*max(envelope,radial.length),blend)
                v.co.x=desired.x;v.co.y=desired.y

        # The overlapping waistband is a single pelvis attachment in both
        # garments, so a deep bend cannot expose skin between separate weights.
        for obj in [tshirt,trousers]:
            hips=obj.vertex_groups.get('Hips') or obj.vertex_groups.new(name='Hips')
            for v in obj.data.vertices:
                if (obj==tshirt and v.co.z<1.06) or (obj==trousers and v.co.z>.84):
                    for group in obj.vertex_groups:group.remove([v.index])
                    hips.add([v.index],1,'REPLACE')
        skin(tshirt,rig)
        # Sew a straight, loose hem onto the shirt's real lower boundary.
        # It overlaps the complete jeans waistband without following buttock
        # topology or leaving a skin-coloured strip visible during a bend.
        pant_waist=[v.co.copy() for v in trousers.data.vertices if .84<v.co.z<1.10]
        points=[];faces=[];bm=bmesh.new();bm.from_mesh(tshirt.data)
        for edge in bm.edges:
            if not edge.is_boundary or max(v.co.z for v in edge.verts)>1.02:continue
            a,b=[v.co.copy() for v in edge.verts];index=len(points)
            low=[]
            for point in [a,b]:
                outward=Vector((point.x,point.y,0)).normalized()
                lower=point+Vector((0,0,-.045));low.append(lower)
            points.extend([a,b,low[1],low[0]])
            normal=(b-a).cross(low[0]-a);outward=Vector((a.x+b.x,a.y+b.y,0))
            faces.append(tuple(index+i for i in ([0,1,2,3] if normal.dot(outward)>0 else [3,2,1,0])))
        bm.free()
        hem=geometry('RakaTShirtHem',points,faces,bpy.data.materials['Raka_TShirt'],'Hips',rig)
        bm=bmesh.new();bm.from_mesh(hem.data);bmesh.ops.remove_doubles(bm,verts=list(bm.verts),dist=.0001);bmesh.ops.recalc_face_normals(bm,faces=list(bm.faces));bm.to_mesh(hem.data);bm.free();hem.data.update()

        shoes_path=copy_texture(SOURCE/'system/clothes/shoes01/shoes01_diffuse.png','RakaShoes')
        shoes=asset_mesh(SOURCE/'system/clothes/shoes01','shoes01',raw,convert,material('Raka_Shoes',(1,1,1),shoes_path),weights,tree);decimate(shoes,.45);skin(shoes,rig)
        # World handset is visible only in phone poses, managed by AnimatedRakaAvatar.
        phone=material('Raka_Phone',(.018,.02,.023));hand=defs['Hand_R'][0]
        longitudinal,palm,sideways=hand_frame(defs,'R')
        def phone_point(x,y,z):return hand+sideways*x+longitudinal*y+palm*z
        points=[phone_point(dx,dy,dz) for dy in [.030,.195] for dz in [.018,.032] for dx in [-.039,.039]]
        handset=geometry('HandheldPhone',points,[(0,1,3,2),(4,6,7,5),(0,4,5,1),(2,3,7,6),(0,2,6,4),(1,5,7,3)],phone,'Hand_R',rig)
        display=material('Raka_PhoneScreen',(.07,.13,.15))
        screen=geometry('HandsetScreen',[phone_point(x,y,.033) for x,y in [(-.034,.037),(.034,.037),(.034,.187),(-.034,.187)]],[(0,1,2,3)],display,'Hand_R',rig)
        screen.parent=handset
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
    max_gap=0
    for pose in (['Idle','Run'] if female else ['Idle','Walk','Run','Crouch','CrouchWalk','PhoneWalk','Riding']):
        for phase in [0,.25,.5,.75]:
            set_pose(rig,defs,pose,phase,female)
            for side in ['L','R']:
                for a,b in [('Thigh_','Shin_'),('Shin_','Foot_'),('UpperArm_','Forearm_'),('Forearm_','Hand_')]:
                    max_gap=max(max_gap,(rig.pose.bones[a+side].tail-rig.pose.bones[b+side].head).length)
    records['maxLimbJointGapMetres']=round(max_gap,6)
    if not female:
        bm=bmesh.new();bm.from_mesh(body.data)
        neck_edges=[e for e in bm.edges if e.is_boundary and all(1.30<v.co.z<1.50 and abs(v.co.x)<.18 for v in e.verts)]
        records['openNeckBoundaryEdges']=len(neck_edges);bm.free()
        assert records['openNeckBoundaryEdges']==0,'An open seam remains in the visible neck'

    assert max_gap<.003,('Disconnected limb during locomotion',max_gap)

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
    bpy.context.scene.eevee.use_soft_shadows=True;bpy.context.scene.eevee.taa_render_samples=16 if '--quick-preview' in sys.argv else 64
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
    if not female:
        bpy.data.objects['HandheldPhone'].hide_render=False
        rig.animation_data.action=None;set_pose(rig,defs,'PhoneIdle',0,False)
        camera.location=(-.65,-1.18,1.53);camera.rotation_euler=(Vector((-.19,-.34,1.38))-camera.location).to_track_quat('-Z','Y').to_euler();camera_data.lens=70
        scene.render.resolution_x=768;scene.render.resolution_y=768;scene.render.filepath=str(review/'RakaPhoneGrip.png');bpy.ops.render.render(write_still=True)
        bpy.data.objects['HandheldPhone'].hide_render=True
        for pose in ['Idle','Crouch','CrouchWalk','Riding']:
            set_pose(rig,defs,pose,.25,False)
            for jacket in [False,True]:
                bpy.data.objects['RakaJacketOutfit'].hide_render=not jacket
                bpy.data.objects['RakaTShirt'].hide_render=jacket
                bpy.data.objects['RakaTShirtHem'].hide_render=jacket
                camera.location=(1.2,3.2,1.5)
                camera.rotation_euler=(Vector((0,0,.86))-camera.location).to_track_quat('-Z','Y').to_euler();camera_data.lens=62
                scene.render.filepath=str(review/('RakaRear-'+pose+('-Jacket' if jacket else '-TShirt')+'.png'));bpy.ops.render.render(write_still=True)

    return records


if __name__ == '__main__':
    report=([json.loads((ROOT/'Audit/rigged-character-authoring.json').read_text())['characters'][0]] if '--male-only' in sys.argv else [make_character(True)])+[make_character(False)]
    (ROOT/'Audit/rigged-character-authoring.json').write_text(json.dumps(dict(sourceManifest='Audit/character-source-manifest.json',generatedCloth='Audit/character-generated-cloth.json',method='CC0 MakeHuman graphical data adapted into clothed, budgeted skinned meshes; original baked joint animation and garment/tool geometry; generated opaque ivory lace and CC0 PolyHaven jacquard',characters=report),indent=2)+'\n')
