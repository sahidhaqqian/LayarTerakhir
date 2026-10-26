"""Retarget existing CC0 human locomotion onto the supplied, fitted Nyonya rig.

Blender -b -t 2 --python-exit-code 1 --python Tools/retarget_nyonya_motion.py
Reuses the artist's mesh, texture UVs, bent anatomy and shears. No new sculpt,
texture or original animation performance. Existing attack/recoil clips remain.
"""
import bpy
import hashlib
import json
import subprocess
import sys
from pathlib import Path
from mathutils import Matrix, Quaternion, Vector

ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'Builds/QA/NyonyaRevision'
MODEL=ROOT/'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/NyonyaDesigner.fbx'
SOURCE=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Animations/QuaterniusHumanMotion.fbx'
BASELINE=OUT/'Baseline/NyonyaDesigner.fbx'
BASE_COMMIT='073a0ab4bf1f83bea69c72875ca83cbd743c0794'
sys.path.insert(0,str(ROOT/'Tools'))
from retarget_raka_presentation import update,world_rotation,solve,evaluated_points,export_fbx

def sha(path):
    with path.open('rb') as handle:return hashlib.file_digest(handle,'sha256').hexdigest()

def build():
    OUT.mkdir(parents=True,exist_ok=True)
    if not BASELINE.exists():
        BASELINE.parent.mkdir(parents=True,exist_ok=True)
        with BASELINE.open('wb') as handle:
            subprocess.run(['git','show',BASE_COMMIT+':'+str(MODEL.relative_to(ROOT))],cwd=ROOT,stdout=handle,check=True)
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(BASELINE))
    target=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
    objects=set(bpy.context.scene.objects)
    skin=next(o for o in objects if o.type=='MESH')
    baseline_actions={a.name.rsplit('|',1)[-1]:a for a in bpy.data.actions}
    old_fps=bpy.context.scene.render.fps
    preserved={}
    for name in ['Windup','Stun']:
        action=baseline_actions[name];target.animation_data.action=action;preserved[name]=[]
        for frame in range(int(action.frame_range.x),int(action.frame_range.y)+1):
            bpy.context.scene.frame_set(frame);update()
            preserved[name].append((frame,{b.name:(b.location.copy(),b.matrix_basis.to_quaternion().copy(),b.scale.copy()) for b in target.pose.bones}))
    target.animation_data.action=None
    for b in target.pose.bones:b.rotation_mode='QUATERNION';b.matrix_basis=Matrix.Identity(4)
    update()
    target_rest={n:b.matrix_local.to_quaternion() for n,b in target.data.bones.items()}
    hip=target.data.bones['Hips'].head_local.copy()
    # The old automatic binding left the low blade on the pelvis while the
    # handle followed the hand. Keep this existing, outboard prop rigidly in
    # its existing Shears bone; never blend it into the central robe repair.
    weapon_vertices=set()
    for v in skin.data.vertices:
        held=any(skin.vertex_groups[g.group].name=='Shears' and g.weight>.5 for g in v.groups)
        if held or (v.co.x<-.25 and v.co.z<.68):
            weapon_vertices.add(v.index)
            for group in skin.vertex_groups:group.remove([v.index])
            skin.vertex_groups['Shears'].add([v.index],1,'REPLACE')
    # Automatic VRM weights assigned several central robe panels to a nearby
    # hand/leg. Repair their binding; retain every supplied vertex, UV and face.
    # The separate, outboard sculpted arms and held shears are excluded.
    repaired=0
    def smooth(value):
        value=max(0,min(1,value));return value*value*(3-2*value)
    for v in skin.data.vertices:
        if v.index in weapon_vertices or abs(v.co.x)>.25 or not .17<v.co.z<1.16:continue
        blend=smooth((.25-abs(v.co.x))/.07)*smooth((v.co.z-.17)/.08)*smooth((1.16-v.co.z)/.12)
        t=smooth((v.co.z-.88)/.20)
        weights={g.group:g.weight*(1-blend) for g in v.groups}
        for name,weight in [('Hips',1-t),('Spine',t)]:
            index=skin.vertex_groups[name].index;weights[index]=weights.get(index,0)+blend*weight
        weights=sorted(weights.items(),key=lambda item:-item[1])[:4];total=sum(w for _,w in weights)
        for group in skin.vertex_groups:group.remove([v.index])
        for index,weight in weights:
            if weight>0:skin.vertex_groups[index].add([v.index],weight/total,'REPLACE')
        repaired+=1
    before=set(bpy.context.scene.objects)
    bpy.ops.import_scene.fbx(filepath=str(SOURCE))
    source_objects=set(bpy.context.scene.objects)-before
    source=next(o for o in source_objects if o.type=='ARMATURE');source.matrix_world=Matrix.Identity(4)
    clips={a.name.rsplit('|',1)[-1]:a for a in bpy.data.actions if a not in baseline_actions.values()}
    source.animation_data.action=clips['Idle_Loop'];bpy.context.scene.frame_set(1);update()
    reference={n:b.matrix.to_quaternion().copy() for n,b in source.pose.bones.items()}
    reference_positions={n:b.head.copy() for n,b in source.pose.bones.items()}
    source_hip=source.pose.bones['DEF-hips'].head.copy()
    scale=hip.z/source_hip.z
    mapping={'Hips':'DEF-hips','Spine':'DEF-spine.001','Chest':'DEF-spine.002','UpperChest':'DEF-spine.003','Neck':'DEF-neck','Head':'DEF-head'}
    for side in ['L','R']:
        for part,origin in [('Shoulder','shoulder'),('UpperArm','upper_arm'),('Forearm','forearm'),('Hand','hand'),('Thigh','thigh'),('Shin','shin'),('Foot','foot'),('Toes','toe')]:mapping[part+'_'+side]='DEF-'+origin+'.'+side
    fps=bpy.context.scene.render.fps
    actions=[];report=[]
    for name,source_name in [('Idle','Idle_Loop'),('Walk','Walk_Loop'),('Run','Jog_Fwd_Loop')]:
        source.animation_data.action=clips[source_name]
        action=bpy.data.actions.new('REVISED_'+name);target.animation_data.action=action;actions.append(action)
        count=int(round(source.animation_data.action.frame_range.y-source.animation_data.action.frame_range.x));corrections=[]
        for frame in range(count+1):
            bpy.context.scene.frame_set(int(source.animation_data.action.frame_range.x)+frame);update()
            for b in target.pose.bones:b.matrix_basis=Matrix.Identity(4)
            update()
            for dest,origin in mapping.items():
                delta=source.pose.bones[origin].matrix.to_quaternion()@reference[origin].inverted()
                # Preserve the sculpt's deliberately long, bent arms and grasp.
                # Add the authored swing relative to its idle, not a T-pose.
                # This supplied robe is weighted to the pelvis, not simulated
                # cloth. Keep its support upright while the chest/limbs move.
                strength=.25 if dest=='Hips' else .45 if dest.startswith(('Shoulder','UpperArm','Forearm','Hand')) else .7 if dest in ['Head','Neck'] else .85 if dest.startswith(('Thigh','Shin')) else 1
                delta=Quaternion().slerp(delta,strength)
                q=delta@target_rest[dest];b=target.pose.bones[dest]
                if dest=='Hips':
                    offset=(source.pose.bones[origin].head-source_hip)*scale;offset.z=max(-.02,min(.02,offset.z))
                    if name=='Run':offset.x*=.8;offset.y*=.8
                    matrix=q.to_matrix().to_4x4();matrix.translation=hip+offset;b.matrix=matrix
                else:world_rotation(b,q)
                update()
            for side in ['L','R']:
                foot=target.pose.bones['Foot_'+side]
                origin='DEF-foot.'+side
                offset=(source.pose.bones[origin].head-reference_positions[origin])*scale
                # A long robe needs shorter, quicker strides than the source
                # athletic jog. Preserve its authored timing and foot sequence.
                if name=='Run':offset.x*=.8;offset.y*=.8
                goal=target.data.bones['Foot_'+side].head_local.copy()+offset
                sole=target.data.bones['Foot_'+side].head_local.z
                goal.z=max(sole,min(goal.z,sole+.07))
                pole=target.data.bones['Shin_'+side].head_local.copy()+Vector((0,-.25,0))
                solve(target,'Thigh_'+side,'Shin_'+side,'Foot_'+side,goal,pole)
                world_rotation(foot,target_rest['Foot_'+side]);update()
                world_rotation(target.pose.bones['Toes_'+side],target_rest['Toes_'+side]);update()
            # Preserve the supplied grasp and keep the long existing blade
            # clear of the floor. Follow the source hand path with an arm IK
            # constraint, instead of raising the whole hunter to clear it.
            hand=target.data.bones['Hand_R'].head_local.copy()
            hand+=(source.pose.bones['DEF-hand.R'].head-reference_positions['DEF-hand.R'])*scale*.35
            hand.z=max(hand.z,target.data.bones['Hand_R'].head_local.z)
            solve(target,'UpperArm_R','Forearm_R','Hand_R',hand,target.data.bones['Forearm_R'].head_local.copy())
            world_rotation(target.pose.bones['Hand_R'],target_rest['Hand_R']);update()
            # The robe is part of the supplied weighted mesh. Fit its support
            # to the floor offline, without moving the AI/navigation root.
            minimum=float(evaluated_points(skin)[:,2].min())
            pelvis=target.pose.bones['Hips'];matrix=pelvis.matrix.copy();matrix.translation.z-=minimum;pelvis.matrix=matrix;update();corrections.append(abs(minimum))
            for b in target.pose.bones:
                b.keyframe_insert('rotation_quaternion',frame=frame+1,group=b.name)
                b.keyframe_insert('location',frame=frame+1,group=b.name)
        for curve in action.fcurves:
            for key in curve.keyframe_points:key.interpolation='LINEAR'
        action.use_fake_user=True
        report.append(dict(clip=name,source=source_name,frames=count+1,fps=fps,maxRobeSupportCorrectionMetres=max(corrections)))
        print('NYONYA_RETARGET',report[-1],flush=True)
    for obj in source_objects:bpy.data.objects.remove(obj,do_unlink=True)
    for name,samples in preserved.items():
        action=bpy.data.actions.new('REVISED_'+name);target.animation_data.action=action;actions.append(action)
        for frame,bones in samples:
            at=1+(frame-1)*fps/old_fps
            for n,(location,rotation,scale0) in bones.items():
                b=target.pose.bones[n];b.location=location;b.rotation_quaternion=rotation;b.scale=scale0
                for prop in ['location','rotation_quaternion','scale']:b.keyframe_insert(prop,frame=at,group=n)
        action.use_fake_user=True
    target.animation_data.action=None
    for action in list(bpy.data.actions):
        if action not in actions:bpy.data.actions.remove(action)
    for action in actions:action.name=action.name.removeprefix('REVISED_')
    target.animation_data.action=next(a for a in actions if a.name=='Idle');bpy.context.scene.frame_set(1);update()
    export_fbx(MODEL,list(objects),True)
    (ROOT/'Audit/nyonya-motion-retarget.json').write_text(json.dumps(dict(baselineCommit=BASE_COMMIT,baselineSha256=sha(BASELINE),sourceMotion=str(SOURCE.relative_to(ROOT)),sourceSha256=sha(SOURCE),license='CC0-1.0, Quaternius Universal Animation Library',sourcePage='https://quaternius.com/packs/universalanimationlibrary.html',meshTriangles=len(skin.data.polygons),meshAndTextureUnchanged=True,repairedRobeWeights=repaired,rigidShearsVertices=len(weapon_vertices),retainedClips=['Windup','Stun'],clips=report,outputSha256=sha(MODEL)),indent=2)+'\n')
    bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'Nyonya-revised.blend'))
    print('NYONYA_RETARGET_COMPLETE',flush=True)

if __name__=='__main__':build()
