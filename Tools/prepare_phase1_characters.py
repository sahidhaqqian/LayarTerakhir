"""Offline adaptation of the designer's three supplied VRMs; originals stay read-only.

blender -b --python Tools/prepare_phase1_characters.py -- --role Genderuwo
Uses the established VRM surface/skeleton exporter, no game/runtime VRM dependency.
"""
import argparse
import json
import sys
from pathlib import Path

import bpy
from mathutils import Matrix

sys.path.insert(0, str(Path(__file__).resolve().parent))
import prepare_designer_vrm as pipeline

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Characters'
pipeline.OUT = OUT
pipeline.MODEL = OUT / 'Models'
pipeline.TEXTURE = OUT / 'Textures'
for folder in [pipeline.MODEL, pipeline.TEXTURE]:
    folder.mkdir(parents=True, exist_ok=True)


def reuse_motion(rig):
    """Retarget existing project motions, including their bind-pose correction."""
    bases = [
        ('RakaDesigner.fbx', {'Idle', 'Walk', 'Run'}),
        ('NyonyaDesigner.fbx', {'Windup', 'Stun'}),
    ]
    targets = []
    idle = {}
    sources = []
    for filename, wanted in bases:
        path = ROOT / 'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models' / filename
        prior = set(bpy.data.objects)
        before_actions = set(bpy.data.actions)
        bpy.ops.import_scene.fbx(filepath=str(path), use_anim=True)
        imported = set(bpy.data.objects) - prior
        donor = next(o for o in imported if o.type == 'ARMATURE')
        actions = list(set(bpy.data.actions) - before_actions)
        for action in sorted(actions, key=lambda a: a.name):
            name = action.name.split('|')[-1]
            if name not in wanted:
                continue
            donor.animation_data_create()
            donor.animation_data.action = action
            target = bpy.data.actions.new('Retarget_' + name)
            target.use_fake_user = True
            rig.animation_data_create()
            rig.animation_data.action = target
            first, last = (int(v) for v in action.frame_range)
            for frame in range(first, last + 1):
                bpy.context.scene.frame_set(frame)
                for bone in rig.pose.bones:
                    original = donor.pose.bones.get(bone.name)
                    if original is None:
                        continue
                    source_basis = original.bone.matrix_local.to_quaternion()
                    target_basis = bone.bone.matrix_local.to_quaternion()
                    delta = source_basis @ original.matrix_basis.to_quaternion() @ source_basis.inverted()
                    if filename.startswith('Raka') and name == 'Idle' and frame == first:
                        idle[bone.name] = delta.copy()
                    elif filename.startswith('Nyonya') and bone.name in idle:
                        # The old Nyonya sculpt has lowered arms in its rest mesh.
                        # Reuse the existing humanoid idle correction for VRM T-poses.
                        delta = delta @ idle[bone.name]
                    bone.matrix_basis = Matrix.Identity(4)
                    bone.rotation_mode = 'QUATERNION'
                    bone.rotation_quaternion = target_basis.inverted() @ delta @ target_basis
                    bone.keyframe_insert('rotation_quaternion', frame=frame-first+1, group=bone.name)
            targets.append((name, target))
        for obj in imported:
            bpy.data.objects.remove(obj, do_unlink=True)
        rig.animation_data.action = None
        for action in actions:
            bpy.data.actions.remove(action)
        sources.append({'path':str(path.relative_to(ROOT)), 'sha256':pipeline.digest(path)})
    assert set(dict(targets)) == {'Idle', 'Walk', 'Run', 'Windup', 'Stun'}, [n for n, _ in targets]
    for name, target in targets:
        target.name = name
    rig.animation_data.action = dict(targets)['Idle']
    bpy.context.scene.frame_set(1)
    return [n for n, _ in targets], sources


def build(role, motion_only=False):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    source = ROOT / 'Assets/_LayarTerakhir/assets/LayarTerakhir' / (role + '.vrm')
    if motion_only:
        record=json.loads((ROOT/('Audit/phase1-'+role.lower()+'-authoring.json')).read_text())
        bpy.ops.import_scene.fbx(filepath=str(pipeline.MODEL/(role+'.fbx')),use_anim=False)
        rig=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
        mesh=next(o for o in bpy.context.scene.objects if o.type=='MESH' and any(m.type=='ARMATURE' for m in o.modifiers))
        original=record['sourceTriangles'];texture_record=record['textures'];height=record['height']
    else:
        gltf = pipeline.read_gltf(source)
        bpy.ops.import_scene.gltf(filepath=str(source))
        rig = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
        mesh = next(o for o in bpy.context.scene.objects if o.type == 'MESH' and any(m.type == 'ARMATURE' for m in o.modifiers))
        for obj in list(bpy.context.scene.objects):
            if obj not in [rig, mesh]:
                bpy.data.objects.remove(obj, do_unlink=True)
        texture_record = pipeline.textures(role, gltf)
        mesh.data.materials[0].name = role + 'Phase1Surface'
        height = {'Genderuwo': 2.5, 'Putih': 1.8, 'Tuyul': .85}[role]
        pipeline.skeleton(rig, mesh, role, height)
        original = pipeline.simplify(mesh, 24000 if role == 'Genderuwo' else 18000 if role == 'Putih' else 9000)
    mesh.hide_viewport = True
    clips, motion_sources = reuse_motion(rig)
    mesh.hide_viewport = False
    pipeline.export_fbx(pipeline.MODEL / (role + '.fbx'), [rig, mesh], True)
    record = {'role': role, 'sourceSha256': pipeline.digest(source), 'sourceTriangles': original,
              'gameTriangles': sum(len(p.vertices) - 2 for p in mesh.data.polygons), 'height': height,
              'textures': texture_record, 'clips': clips, 'motionSources': motion_sources, 'motionOperation': 'retarget existing rotation deltas, no original motion authored'}
    report = ROOT / ('Audit/phase1-' + role.lower() + '-authoring.json')
    report.write_text(json.dumps(record, indent=2) + '\n')
    print('PHASE1_CHARACTER', json.dumps(record), flush=True)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--role', required=True, choices=['Genderuwo', 'Putih', 'Tuyul'])
    parser.add_argument('--motion-only', action='store_true', help='Retarget existing imported mesh, leaving geometry/textures unchanged')
    args = parser.parse_args(sys.argv[sys.argv.index('--') + 1:] if '--' in sys.argv else [])
    build(args.role,args.motion_only)
