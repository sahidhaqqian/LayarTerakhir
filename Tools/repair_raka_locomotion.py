"""Repair only standing leg curves in the current Raka FBX; keep all asset IDs.

blender -b -t 2 --python Tools/repair_raka_locomotion.py -- --output <fbx>
Uses the licensed, already tracked Quaternius source. No mesh regeneration.
"""
import argparse
import copy
import hashlib
import json
import math
import sys
import tempfile
from pathlib import Path

import bpy
from mathutils import Matrix, Vector
from io_scene_fbx import parse_fbx, encode_bin

sys.path.insert(0, str(Path(__file__).resolve().parent))
from optimize_raka_geometry import child, objects, encoded, semantic_hash
from retarget_raka_presentation import solve, world_rotation, update, source_leg_bend

ROOT = Path(__file__).resolve().parents[1]
MODEL = ROOT / 'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/RakaDesigner.fbx'
SOURCE = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Animations/QuaterniusHumanMotion.fbx'
CLIPS = {'Walk': 'Walk_Loop', 'Run': 'Sprint_Loop',
         'PhoneWalk': 'Walk_Loop', 'PhoneRun': 'Sprint_Loop'}
BONES = {part+'_'+side for part in ['Thigh', 'Shin', 'Foot', 'Toes'] for side in ['L', 'R']}


def curves(root):
    """Index FBX channels by clip/bone/property/axis, independent of file IDs."""
    table = objects(root)
    links = child(root, b'Connections').elems
    result = {}
    for curve in table.values():
        if curve.id != b'AnimationCurve':
            continue
        node_link = next(e for e in links if e.props[1] == curve.props[0])
        node = node_link.props[2]
        model_link = next(e for e in links if e.props[1] == node and e.props[0] == b'OP')
        layer = next(e.props[2] for e in links if e.props[1] == node and e.props[0] == b'OO')
        stack = next(table[e.props[2]] for e in links if e.props[1] == layer)
        clip = stack.props[1].split(b'\0')[0].decode().rsplit('|', 1)[-1]
        bone = table[model_link.props[2]].props[1].split(b'\0')[0].decode()
        key = (clip, bone, model_link.props[3].decode(), node_link.props[3].decode())
        assert key not in result, key
        result[key] = curve
    return result


def bind_matrices(root):
    table = objects(root)
    result = {}
    for pose in table.values():
        if pose.id != b'Pose':
            continue
        for node in pose.elems:
            if node.id != b'PoseNode':
                continue
            model = table[child(node, b'Node').props[0]]
            result[model.props[1]] = list(child(node, b'Matrix').props[0])
    return result


def verify(path, report):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(path))
    target=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
    actions={a.name.rsplit('|',1)[-1]:a for a in bpy.data.actions}
    rest={n:b.matrix_local.to_quaternion() for n,b in target.data.bones.items()}
    bpy.ops.import_scene.fbx(filepath=str(SOURCE))
    source=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE' and o!=target)
    source.matrix_world=Matrix.Identity(4)
    source_actions={a.name.rsplit('|',1)[-1]:a for a in bpy.data.actions if a not in actions.values()}
    sr={n:b.matrix_local.to_quaternion() for n,b in source.data.bones.items()}
    maximum_rotation=0;maximum_plane=0;maximum_length=0
    for name,source_name in CLIPS.items():
        target.animation_data.action=actions[name];source.animation_data.action=source_actions[source_name]
        previous={}
        for frame in range(1,int(actions[name].frame_range.y)+1):
            bpy.context.scene.frame_set(frame);update()
            for side in ['L','R']:
                a,b,c=[target.pose.bones[n+'_'+side].head.copy() for n in ['Thigh','Shin','Foot']]
                for part,origin in [('Foot','foot'),('Toes','toe')]:
                    src='DEF-'+origin+'.'+side
                    expected=source.pose.bones[src].matrix.to_quaternion()@sr[src].inverted()@rest[part+'_'+side]
                    actual=target.pose.bones[part+'_'+side].matrix.to_quaternion()
                    error=math.degrees(actual.rotation_difference(expected).angle)
                    error=min(error,360-error);maximum_rotation=max(maximum_rotation,error)
                bend=source_leg_bend(source,side,a,c,previous)
                axis=(c-a).normalized();actual=b-a-axis*(b-a).dot(axis)
                if actual.length>.001:maximum_plane=max(maximum_plane,math.degrees(actual.normalized().angle(bend)))
                for parent,child_name in [('Thigh','Shin'),('Shin','Foot')]:
                    original=(target.data.bones[child_name+'_'+side].head_local-target.data.bones[parent+'_'+side].head_local).length
                    posed=(target.pose.bones[child_name+'_'+side].head-target.pose.bones[parent+'_'+side].head).length
                    maximum_length=max(maximum_length,abs(posed-original))
    assert maximum_rotation<.1,maximum_rotation
    assert maximum_plane<1,maximum_plane
    assert maximum_length<.0001,maximum_length
    data=json.loads(report.read_text())
    data['reimportVerification']=dict(maximumSourceFootToeRotationErrorDegrees=maximum_rotation,
        maximumKneePlaneErrorDegrees=maximum_plane,maximumLimbLengthErrorMetres=maximum_length)
    report.write_text(json.dumps(data,indent=2)+'\n')
    print('REIMPORTED_CURVES_VERIFIED',data['reimportVerification'],flush=True)


def repair(output, report):
    original, version = parse_fbx.parse(str(MODEL))
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(MODEL))
    target = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
    asset_objects = set(bpy.context.scene.objects)
    actions = {a.name.rsplit('|', 1)[-1]: a for a in bpy.data.actions}
    target_rests = {n: b.matrix_local.to_quaternion() for n, b in target.data.bones.items()}
    skin = next(o for o in asset_objects if o.type == 'MESH' and o.name == 'VisibleSkin')
    foot_indices = {}
    for side in ['L', 'R']:
        groups = {skin.vertex_groups[n].index for n in ['Foot_'+side, 'Toes_'+side]}
        foot_indices[side] = [v.index for v in skin.data.vertices
                              if sum(g.weight for g in v.groups if g.group in groups) > .5]
        assert foot_indices[side]
    bpy.ops.import_scene.fbx(filepath=str(SOURCE))
    source_objects = set(bpy.context.scene.objects)-asset_objects
    source = next(o for o in source_objects if o.type == 'ARMATURE')
    source.matrix_world = Matrix.Identity(4)
    source_actions = {a.name.rsplit('|', 1)[-1]: a for a in bpy.data.actions if a not in actions.values()}
    source_rests = {n: b.matrix_local.to_quaternion() for n, b in source.data.bones.items()}
    rows = []
    for name, source_name in CLIPS.items():
        action = actions[name]
        source_action = source_actions[source_name]
        target.animation_data.action = action
        source.animation_data.action = source_action
        first, last = map(int, action.frame_range)
        previous_bend = {}
        # Collect before inserting keys so evaluating frame N never interpolates
        # an already repaired frame N-1 into the original animation.
        samples = []
        for frame in range(first, last+1):
            bpy.context.scene.frame_set(frame); update()
            for side in ['L', 'R']:
                thigh, shin, foot, toe = [target.pose.bones[n+'_'+side] for n in ['Thigh', 'Shin', 'Foot', 'Toes']]
                goal = foot.head.copy()
                old_knee = shin.head.copy()
                old_foot = foot.matrix.to_quaternion().copy()
                bend=source_leg_bend(source,side,thigh.head,goal,previous_bend)
                pole=thigh.head+bend
                rotations = {}
                for dest, origin in [('Foot', 'foot'), ('Toes', 'toe')]:
                    src = 'DEF-'+origin+'.'+side
                    rotations[dest] = source.pose.bones[src].matrix.to_quaternion() @ source_rests[src].inverted() @ target_rests[dest+'_'+side]
                solve(target, thigh.name, shin.name, foot.name, goal, pole)
                world_rotation(foot, rotations['Foot']); update(); world_rotation(toe, rotations['Toes']); update()
                evaluated = skin.evaluated_get(bpy.context.evaluated_depsgraph_get())
                mesh = evaluated.to_mesh()
                floor = min((evaluated.matrix_world @ mesh.vertices[i].co).z for i in foot_indices[side])
                evaluated.to_mesh_clear()
                lift = max(0, -floor)
                if lift:
                    goal.z += lift
                    solve(target, thigh.name, shin.name, foot.name, goal, pole)
                    world_rotation(foot, rotations['Foot']); update(); world_rotation(toe, rotations['Toes']); update()
                rows.append(dict(clip=name, frame=frame, side=side, clearanceLift=lift,
                                 kneeDelta=(shin.head-old_knee).length,
                                 footRotationDelta=math.degrees(old_foot.rotation_difference(rotations['Foot']).angle)))
            samples.append((frame, {n: target.pose.bones[n].rotation_quaternion.copy() for n in BONES}))
        for frame, rotations in samples:
            for n, q in rotations.items():
                bone = target.pose.bones[n]; bone.rotation_mode = 'QUATERNION'; bone.rotation_quaternion = q
                bone.keyframe_insert('rotation_quaternion', frame=frame, group=n)
        for curve in action.fcurves:
            for key in curve.keyframe_points:
                key.interpolation = 'LINEAR'
    for obj in source_objects:
        bpy.data.objects.remove(obj, do_unlink=True)
    for action in list(bpy.data.actions):
        if action not in [actions[n] for n in CLIPS]: bpy.data.actions.remove(action)
    for name in CLIPS:
        action = actions[name]; action.name = name; action.use_fake_user = True
    target.animation_data.action = actions['Walk']
    bpy.context.scene.frame_set(1); update()
    bpy.ops.object.select_all(action='SELECT')
    with tempfile.TemporaryDirectory(prefix='raka-curves-') as tmp:
        candidate = Path(tmp)/'candidate.fbx'
        bpy.ops.export_scene.fbx(filepath=str(candidate), use_selection=True,
            object_types={'MESH', 'ARMATURE', 'EMPTY'}, axis_forward='-Z', axis_up='Y',
            bake_anim=True, bake_anim_use_all_actions=True, bake_anim_use_nla_strips=False,
            bake_anim_simplify_factor=0, add_leaf_bones=False, path_mode='STRIP')
        replacement, _ = parse_fbx.parse(str(candidate))
    # Blender's default Model pose can differ. The invariant here is bind basis,
    # not default posed Euler rotations; retain all original Model/Pose records.
    old_bind, new_bind = bind_matrices(original), bind_matrices(replacement)
    bind_error = max(max(abs(x-y) for x, y in zip(v, new_bind[n])) for n, v in old_bind.items())
    assert bind_error < .005, bind_error
    old_curves, new_curves = curves(original), curves(replacement)
    changed = set()
    for key, old in old_curves.items():
        if key[0] in CLIPS and key[1] in BONES and key[2] == 'Lcl Rotation':
            old.elems[:] = copy.deepcopy(new_curves[key].elems)
            changed.add(old.props[0])
    assert len(changed) == len(CLIPS)*len(BONES)*3, len(changed)
    immutable = {i: semantic_hash(e) for i, e in objects(original).items() if i not in changed}
    top_level = {e.id: semantic_hash(e) for e in original.elems if e.id != b'Objects'}
    output.parent.mkdir(parents=True, exist_ok=True)
    encode_bin.write(str(output), encoded(original), version)
    verified, _ = parse_fbx.parse(str(output))
    assert all(semantic_hash(objects(verified)[i]) == h for i, h in immutable.items())
    assert all(semantic_hash(child(verified, n)) == h for n, h in top_level.items())
    report.parent.mkdir(parents=True, exist_ok=True)
    report.write_text(json.dumps(dict(sourceSha256=hashlib.sha256(MODEL.read_bytes()).hexdigest(),
        outputSha256=hashlib.sha256(output.read_bytes()).hexdigest(), changedRotationChannels=len(changed),
        preservedObjects=len(immutable), bindMatrixMaximumDifference=bind_error,
        geometryWeightsMaterialsHierarchyIdsAndUnrelatedAnimationsUnchanged=True, samples=rows), indent=2)+'\n')
    print('RAKA_LEG_REPAIR', len(changed), 'channels; preserved', len(immutable), 'records', flush=True)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--report', type=Path, default=ROOT/'Audit/raka-locomotion-recovery.json')
    parser.add_argument('--verify-only',action='store_true')
    args = parser.parse_args(sys.argv[sys.argv.index('--')+1:])
    if not args.verify_only:repair(args.output, args.report)
    verify(args.output,args.report)
