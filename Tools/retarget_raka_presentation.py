"""Retarget sourced CC0 motion onto the existing Raka rig; preserve controller ownership.

Blender -b -t 2 --python Tools/retarget_raka_presentation.py
The designer VRM and Quaternius FBX are read-only inputs. No new sculpt,
textures, or original animation performance is generated.
"""
import bpy
import hashlib
import json
import math
import sys
import subprocess
import numpy as np
from pathlib import Path
from mathutils import Vector, Matrix, Quaternion

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Builds/QA/RakaRevision'
MODEL = ROOT / 'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/RakaDesigner.fbx'
SOURCE = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Animations/QuaterniusHumanMotion.fbx'
ORIGINAL = ROOT / 'Assets/_LayarTerakhir/assets/LayarTerakhir/Raka.vrm'
BASELINE = OUT / 'Baseline/RakaDesigner.fbx'
BASELINE_COMMIT = '1bcb2152c459e9da4a35bef3d64a1527a9f4fa1a'
sys.path.insert(0, str(ROOT / 'Tools'))
from prepare_designer_vrm import skeleton, simplify, repair_raka_thumb, export_fbx
from optimize_raka_geometry import reduce_body


def digest(path):
    with path.open('rb') as handle:
        return hashlib.file_digest(handle, 'sha256').hexdigest()


def update():
    bpy.context.view_layer.update()


def world_rotation(bone, rotation):
    m = rotation.to_matrix().to_4x4()
    m.translation = bone.head.copy()
    bone.matrix = m


def solve(rig, upper, lower, end, goal, pole):
    a, b, c = [rig.pose.bones[n] for n in (upper, lower, end)]
    start = a.head.copy()
    l1, l2 = (b.head-start).length, (c.head-b.head).length
    delta = goal-start
    distance = min(max(delta.length, .001), l1+l2-.0001)
    direction = delta.normalized()
    bend = pole-start
    bend = (bend-direction*bend.dot(direction)).normalized()
    if bend.length < .1:
        bend = Vector((0,-1,0))
    along = (l1*l1-l2*l2+distance*distance)/(2*distance)
    knee = start+direction*along+bend*math.sqrt(max(0,l1*l1-along*along))
    q = (b.head-start).rotation_difference(knee-start)
    world_rotation(a, q@a.matrix.to_quaternion()); update()
    q = (c.head-b.head).rotation_difference(goal-b.head)
    world_rotation(b, q@b.matrix.to_quaternion()); update()


def build():
    if not BASELINE.exists():
        BASELINE.parent.mkdir(parents=True,exist_ok=True)
        with BASELINE.open('wb') as handle:
            subprocess.run(['git','show',BASELINE_COMMIT+':'+str(MODEL.relative_to(ROOT))],cwd=ROOT,stdout=handle,check=True)
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(BASELINE))
    target = next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
    target.name = 'CharacterRig'
    asset_objects = set(bpy.context.scene.objects)
    old = {a.name.rsplit('|',1)[-1]:a for a in bpy.data.actions}
    target_fps = bpy.context.scene.render.fps
    target.animation_data.action = old['PhoneIdle']; bpy.context.scene.frame_set(1); update()
    phone_hand_rotation = target.pose.bones['Hand_R'].matrix.to_quaternion().copy()
    phone_forearm_rotation = target.pose.bones['Forearm_R'].matrix.to_quaternion().copy()
    phone_elbow_direction = (target.pose.bones['Forearm_R'].head-target.pose.bones['Hand_R'].head).normalized()
    phone_fingers = {b.name:b.matrix_basis.to_quaternion().copy() for b in target.pose.bones if b.name.startswith('Finger') and b.name.endswith('_R')}
    legacy_samples = {}
    for name in ['Riding', 'InstallCard']:
        target.animation_data.action=old[name]
        legacy_samples[name]=[]
        for frame in range(int(old[name].frame_range.x),int(old[name].frame_range.y)+1):
            bpy.context.scene.frame_set(frame);update()
            legacy_samples[name].append((frame,{b.name:(b.location.copy(),b.matrix_basis.to_quaternion().copy(),b.scale.copy()) for b in target.pose.bones}))
    target.animation_data.action = None
    for b in target.pose.bones:
        b.rotation_mode='QUATERNION';b.matrix_basis = Matrix.Identity(4)
    update()

    # Re-import the supplied artwork, at a less destructive geometry budget.
    before = set(bpy.context.scene.objects)
    bpy.ops.import_scene.gltf(filepath=str(ORIGINAL))
    imported = set(bpy.context.scene.objects)-before
    source_rig = next(o for o in imported if o.type=='ARMATURE')
    skin = next(o for o in imported if o.type=='MESH' and any(m.type=='ARMATURE' for m in o.modifiers))
    skeleton(source_rig, skin, 'Raka', 1.74)
    errors = [(target.data.bones[n].head_local-source_rig.data.bones[n].head_local).length for n in ['Hips','Head','Hand_L','Hand_R','Foot_L','Foot_R']]
    if max(errors)>.003:
        raise RuntimeError('Designer geometry no longer matches the retained rig: '+str(errors))
    old_skin = next(o for o in asset_objects if o.type=='MESH' and 'VisibleSkin' in o.name)
    materials = list(old_skin.data.materials)
    original_triangles = simplify(skin, 96000)
    for modifier in skin.modifiers:
        if modifier.type=='ARMATURE': modifier.object=target
    skin.parent=target; skin.matrix_world=Matrix.Identity(4)
    skin.data.materials.clear()
    for material in materials:skin.data.materials.append(material)
    bpy.data.objects.remove(old_skin, do_unlink=True);asset_objects.remove(old_skin)
    defs = {b.name:(b.head_local.copy(),b.tail_local.copy(),b.parent.name if b.parent else None) for b in target.data.bones}
    # The supplied auto-rig has no original right-thumb weights; retain the
    # existing geometric thumb repair on the higher-detail derivative.
    repair_raka_thumb(target, skin, defs)
    for obj in imported:
        if obj!=skin:bpy.data.objects.remove(obj,do_unlink=True)
    skin.name='VisibleSkin';asset_objects.add(skin)

    before=set(bpy.context.scene.objects)
    bpy.ops.import_scene.fbx(filepath=str(SOURCE))
    motion_objects=set(bpy.context.scene.objects)-before
    source=next(o for o in motion_objects if o.type=='ARMATURE')
    source.matrix_world=Matrix.Identity(4) # Remove the library's FBX-facing root, matching the existing target axes.
    source_actions={a.name.rsplit('|',1)[-1]:a for a in bpy.data.actions if a not in old.values()}
    fps=bpy.context.scene.render.fps
    mapping={'Hips':'DEF-hips','Spine':'DEF-spine.001','Chest':'DEF-spine.002','UpperChest':'DEF-spine.003','Neck':'DEF-neck','Head':'DEF-head'}
    for side in ['L','R']:
        for dest,origin in [('Shoulder','shoulder'),('UpperArm','upper_arm'),('Forearm','forearm'),('Hand','hand'),('Thigh','thigh'),('Shin','shin'),('Foot','foot'),('Toes','toe')]:mapping[dest+'_'+side]='DEF-'+origin+'.'+side
        for f,name in enumerate(['thumb','f_index','f_middle','f_ring','f_pinky'],1):
            for j in range(1,4):mapping[f'Finger{f}_{j}_{side}']=f'DEF-{name}.{j:02d}.{side}'
    assert all(n in source.data.bones for n in mapping.values())
    hip=target.data.bones['Hips'].head_local.copy()
    source_hip=source.data.bones['DEF-hips'].head_local.copy()
    scale=hip.z/source_hip.z
    rests={n:b.matrix_local.to_quaternion() for n,b in source.data.bones.items()}
    target_rests={n:b.matrix_local.to_quaternion() for n,b in target.data.bones.items()}
    source.animation_data.action=source_actions['Crouch_Idle_Loop']
    bpy.context.scene.frame_set(int(source.animation_data.action.frame_range.x));update()
    crouch_stance={side:(source.pose.bones['DEF-foot.'+side].head-source.data.bones['DEF-foot.'+side].head_local)*scale for side in ['L','R']}
    for obj in bpy.context.scene.objects:
        if obj.type=='MESH':obj.hide_viewport=True
    clips={'Idle':'Idle_Loop','Walk':'Walk_Loop','Run':'Sprint_Loop','Crouch':'Crouch_Idle_Loop','CrouchWalk':'Crouch_Fwd_Loop','LowCrouch':'Crouch_Idle_Loop','LowCrouchWalk':'Crouch_Fwd_Loop'}
    clips.update({'Phone'+k:v for k,v in list(clips.items())})
    actions=[]; report=[]
    for name,source_name in clips.items():
        src=source_actions[source_name]
        source.animation_data.action=src
        action=bpy.data.actions.new('REVISED_'+name);target.animation_data.action=action;actions.append(action)
        frames=int(round(src.frame_range.y-src.frame_range.x))
        floor_by_side={side:float('inf') for side in ['L','R']}
        if 'Crouch' in name:
            # Source toe-standing contact must not become a hovering flat
            # foot after clearance retargeting. Preserve authored lift above
            # each foot's own support height, not its T-pose ankle height.
            for sample in range(frames+1):
                bpy.context.scene.frame_set(int(src.frame_range.x)+sample);update()
                for side in floor_by_side:floor_by_side[side]=min(floor_by_side[side],source.pose.bones['DEF-foot.'+side].head.z)
        maximum=0; minima=[]
        for frame in range(frames+1):
            bpy.context.scene.frame_set(int(src.frame_range.x)+frame);update()
            for b in target.pose.bones:b.matrix_basis=Matrix.Identity(4)
            update()
            # World-space rotation deltas retain authored human timing and
            # knee/arm planes across rigs with different local bone rolls.
            for dest,origin in mapping.items():
                q=source.pose.bones[origin].matrix.to_quaternion()@rests[origin].inverted()@target_rests[dest]
                bone=target.pose.bones[dest]
                if dest=='Hips':
                    delta=source.pose.bones[origin].head-source_hip
                    m=q.to_matrix().to_4x4();m.translation=hip+Vector((delta.x*scale,delta.y*scale,delta.z*scale));bone.matrix=m
                else:world_rotation(bone,q)
                update()
            crouch='Crouch' in name;low='Low' in name
            if crouch:
                pelvis=target.pose.bones['Hips'];head=target.pose.bones['Head']
                ceiling=.765 if low else .985
                excess=max(0,head.head.z+.245-ceiling)
                m=pelvis.matrix.copy();m.translation.z-=min(excess,max(0,m.translation.z-(.34 if low else .42)));pelvis.matrix=m;update()
                spine=target.pose.bones['Spine'];base=spine.matrix.to_quaternion();head_rotation=head.matrix.to_quaternion().copy()
                # Clearance adaptation is skeletal IK, never nonuniform mesh scale.
                for degrees in range(0,76,2):
                    world_rotation(spine,Quaternion((1,0,0),math.radians(degrees))@base);update()
                    world_rotation(head,head_rotation);update()
                    if head.head.z+.245<=ceiling:break
                # Keep the authored arm swing in world space after fitting the
                # torso under the capsule, instead of pitching elbows sideways
                # with the clearance lean (the old chicken-wing silhouette).
                for side in ['L','R']:
                    for part in ['UpperArm','Forearm','Hand']:
                        dest=part+'_'+side;origin=mapping[dest]
                        world_rotation(target.pose.bones[dest],source.pose.bones[origin].matrix.to_quaternion()@rests[origin].inverted()@target_rests[dest]);update()
            for side in ['L','R']:
                origin='DEF-foot.'+side
                delta=(source.pose.bones[origin].head-source.data.bones[origin].head_local)*scale
                goal=target.data.bones['Foot_'+side].head_local+delta
                if crouch:
                    goal.x-=crouch_stance[side].x;goal.y-=crouch_stance[side].y
                    # Short supported steps instead of the source's long
                    # lunge, especially under .78 m clearance. Keep the rear
                    # foot underneath the pelvis so trousers/knees stay clear.
                    goal.y=target.data.bones['Foot_'+side].head_local.y+(goal.y-target.data.bones['Foot_'+side].head_local.y)*(.6 if low else .75)
                    goal.y=min(goal.y,target.pose.bones['Hips'].head.y+.025)
                goal.z=max(target.data.bones['Foot_'+side].head_local.z,goal.z)
                if crouch:goal.z=target.data.bones['Foot_'+side].head_local.z+max(0,(source.pose.bones[origin].head.z-floor_by_side[side])*scale)
                foot_rotation=target.pose.bones['Foot_'+side].matrix.to_quaternion().copy()
                toe_offset=target.data.bones['Foot_'+side].matrix_local.to_3x3().inverted()@(target.data.bones['Toes_'+side].head_local-target.data.bones['Foot_'+side].head_local)
                toe_height=goal.z+(foot_rotation@toe_offset).z
                grounded=crouch or toe_height<.025
                if grounded:foot_rotation=target_rests['Foot_'+side]
                source_knee=source.pose.bones['DEF-shin.'+side].head
                source_pelvis=source.pose.bones['DEF-hips'].head
                pole=target.pose.bones['Hips'].head+(source_knee-source_pelvis)*scale
                if crouch:
                    # A very low capsule requires knees forward/up, not a
                    # source knee pole below the target's shorter pelvis.
                    pole=target.pose.bones['Thigh_'+side].head+Vector((0,-1,.25))
                solve(target,'Thigh_'+side,'Shin_'+side,'Foot_'+side,goal,pole)
                world_rotation(target.pose.bones['Foot_'+side],foot_rotation);update()
                if grounded:world_rotation(target.pose.bones['Toes_'+side],target_rests['Toes_'+side]);update()
            if name.startswith('Phone'):
                head=target.pose.bones['Head'].head
                goal=Vector((-.18,head.y-.35,head.z-.16))
                solve(target,'UpperArm_R','Forearm_R','Hand_R',goal,goal+phone_elbow_direction)
                # Preserve the source grip's anatomical wrist angle as the
                # elbow moves, rather than twisting a fixed world-space hand
                # through its own sleeve while sprinting or crouching.
                world_rotation(target.pose.bones['Hand_R'],target.pose.bones['Forearm_R'].matrix.to_quaternion()@phone_forearm_rotation.inverted()@phone_hand_rotation)
                for n,q in phone_fingers.items():target.pose.bones[n].rotation_quaternion=q
                update()
            maximum=max(maximum,target.pose.bones['Head'].head.z+.245)
            minima.append(min(target.pose.bones['Foot_L'].head.z,target.pose.bones['Foot_R'].head.z))
            for bone in target.pose.bones:
                bone.rotation_mode='QUATERNION'
                bone.keyframe_insert('rotation_quaternion',frame=frame+1,group=bone.name)
                bone.keyframe_insert('location',frame=frame+1,group=bone.name)
        for curve in action.fcurves:
            for key in curve.keyframe_points:key.interpolation='LINEAR'
        report.append(dict(clip=name,source=source_name,frames=frames+1,fps=fps,estimatedHeadTop=maximum,minAnkle=min(minima)))
        print('RETARGETED',report[-1],flush=True)
    for obj in motion_objects:bpy.data.objects.remove(obj,do_unlink=True)
    for name,samples in legacy_samples.items():
        action=bpy.data.actions.new('REVISED_'+name);actions.append(action);target.animation_data.action=action
        for frame,bones in samples:
            at=1+(frame-1)*fps/target_fps
            for n,(location,rotation,scale0) in bones.items():
                b=target.pose.bones[n];b.location=location;b.rotation_quaternion=rotation;b.scale=scale0
                b.keyframe_insert('location',frame=at,group=n);b.keyframe_insert('rotation_quaternion',frame=at,group=n);b.keyframe_insert('scale',frame=at,group=n)
    keep=actions
    target.animation_data.action=None
    for action in list(bpy.data.actions):
        if action not in keep:bpy.data.actions.remove(action)
    for action in keep:
        action.name=action.name.replace('REVISED_','').rsplit('|',1)[-1]
        action.use_fake_user=True
    target.animation_data.action=next(a for a in actions if a.name=='Idle')
    bpy.context.scene.frame_set(1)
    for obj in asset_objects:
        if obj.type=='MESH':obj.hide_viewport=False
    target.name='CharacterRig';update()
    target.animation_data.action=next(a for a in actions if a.name=='PhoneIdle')
    bpy.context.scene.frame_set(1);update()
    bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'Raka-calibration.blend'))
    grip=calibrate_phone(target,skin,asset_objects)
    topology=reduce_body(skin)
    target.animation_data.action=next(a for a in actions if a.name=='Idle');bpy.context.scene.frame_set(1);update()
    export_fbx(MODEL,list(asset_objects),True)
    report_data=dict(sourceVrm=str(ORIGINAL.relative_to(ROOT)),sourceVrmSha256=digest(ORIGINAL),sourceMotion=str(SOURCE.relative_to(ROOT)),sourceMotionSha256=digest(SOURCE),license='CC0-1.0 (Quaternius Universal Animation Library)',originalTriangles=original_triangles,gameTriangles=len(skin.data.polygons),originalMotionPerformance=False,clips=report,output=str(MODEL.relative_to(ROOT)),outputSha256=digest(MODEL),operations='Read-only supplied artwork; protected high-detail geometry derivative, authored CC0 locomotion retarget, clearance/foot/hand IK; existing Riding and InstallCard retained')
    report_data['gripCalibration']=grip
    report_data['geometryOptimization']=topology
    report_data['operations']+='; clothing topology reduced with exact protected head/hand/foot geometry'
    (ROOT/'Audit/raka-presentation-retarget.json').write_text(json.dumps(report_data,indent=2)+'\n')
    bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'Raka-revised.blend'))
    print('RAKA_RETARGET_COMPLETE',MODEL,flush=True)


def evaluated_points(obj):
    deps=bpy.context.evaluated_depsgraph_get();evaluated=obj.evaluated_get(deps);mesh=evaluated.to_mesh()
    points=np.asarray([tuple(evaluated.matrix_world@v.co) for v in mesh.vertices]);evaluated.to_mesh_clear();return points


def calibrate_phone(rig,skin,objects):
    phone=next(o for o in objects if o.type=='MESH' and o.name=='HandheldPhone')
    corners=evaluated_points(phone);centre=corners.mean(axis=0)
    edges=sorted(corners[1:]-corners[0],key=lambda v:np.dot(v,v));axes=[]
    for edge in edges:
        axis=edge/np.linalg.norm(edge)
        if all(abs(np.dot(axis,x))<.01 for x in axes):axes.append(axis)
    assert len(axes)==3
    axes=np.asarray(axes);extent=np.max(np.abs((corners-centre)@axes.T),axis=0)
    original_thickness=float(2*extent[0])
    # Keep the existing handset's width/height; a plausible 11 mm cased depth
    # fits the actual source fingers without intersecting either side.
    thickness_scale=min(1,.011/original_thickness)
    extent[0]*=thickness_scale
    points=(evaluated_points(skin)-centre)@axes.T
    masks=[]
    group_names={g.index:g.name for g in skin.vertex_groups}
    for vertex in skin.data.vertices:
        sums=np.zeros(5)
        for group in vertex.groups:
            name=group_names[group.group]
            if name.startswith('Finger') and name.endswith('_R'):sums[int(name[6])-1]+=group.weight
        masks.append(sums>.25)
    masks=np.asarray(masks)
    baseline_signed=extent-np.abs(points)
    baseline_gap=np.sqrt(np.sum(np.minimum(0,baseline_signed)**2,axis=1))
    print('PHONE_BASELINE',float(np.max(np.min(baseline_signed,axis=1))),[float(np.min(baseline_gap[masks[:,i]])) for i in range(5)],flush=True)
    candidates=[]
    for x in np.linspace(-.003,.003,7):
        for y in np.linspace(-.003,.003,7):
            for z in np.linspace(-.002,.002,5):
                offset=np.asarray([x,y,z]);signed=extent-np.abs(points-offset)
                depth=float(np.max(np.min(signed,axis=1)))
                gaps=np.sqrt(np.sum(np.minimum(0,signed)**2,axis=1))
                contacts=np.asarray([np.min(gaps[masks[:,i]]) for i in range(5)])
                if depth<.0011 and max(contacts)<.0038:candidates.append((float(np.linalg.norm(offset)),depth,offset,contacts))
    if not candidates:raise RuntimeError('No phone fit preserves all five finger contacts')
    _,depth,offset,contacts=min(candidates,key=lambda v:(v[0],v[1]));world_delta=Vector(axes.T@offset)
    # A tiny geometric placement correction only. The numerical Jacobian
    # handles FBX bone parenting without changing finger/hand performance.
    for obj in objects:
        if obj.type!='MESH' or obj==skin:continue
        base=obj.data.vertices[0].co.copy();world0=evaluated_points(obj)[0];columns=[]
        for axis in range(3):
            obj.data.vertices[0].co=base;obj.data.vertices[0].co[axis]+=.001;obj.data.update();update()
            columns.append((evaluated_points(obj)[0]-world0)/.001)
        obj.data.vertices[0].co=base
        posed=evaluated_points(obj)
        desired=(posed-centre)@axes[0]*(thickness_scale-1)
        world_changes=desired[:,None]*axes[0]+np.asarray(world_delta)
        local_changes=world_changes@np.linalg.inv(np.asarray(columns).T).T
        for vertex,delta in zip(obj.data.vertices,local_changes):vertex.co+=Vector(delta)
        obj.data.update();update()
    result=dict(worldOffsetMetres=list(world_delta),originalThicknessMetres=original_thickness,worldHandsetThicknessMetres=original_thickness*thickness_scale,maximumPenetrationMetres=depth,fingerContactGapsMetres=contacts.tolist())
    print('PHONE_FIT',result,flush=True);return result


if __name__=='__main__':
    if '--resume-calibration' in sys.argv:
        bpy.ops.wm.open_mainfile(filepath=str(OUT/'Raka-calibration.blend'))
        rig=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
        skin=next(o for o in bpy.context.scene.objects if o.type=='MESH' and o.name=='VisibleSkin')
        objects=set(bpy.context.scene.objects)
        result=calibrate_phone(rig,skin,objects)
        topology=reduce_body(skin)
        rig.animation_data.action=next(a for a in bpy.data.actions if a.name=='Idle');bpy.context.scene.frame_set(1);update()
        export_fbx(MODEL,list(objects),True)
        path=ROOT/'Audit/raka-presentation-retarget.json';record=json.loads(path.read_text())
        record['outputSha256']=digest(MODEL);record['gripCalibration']=result;record['plantedFootRotationCorrected']=True
        record['gameTriangles']=len(skin.data.polygons);record['geometryOptimization']=topology
        path.write_text(json.dumps(record,indent=2)+'\n');bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'Raka-revised.blend'))
        print('RAKA_CALIBRATION_COMPLETE',flush=True)
    else:build()
