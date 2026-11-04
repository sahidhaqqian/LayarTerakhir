"""Pose validation for the manual Nyonya rig: retarget Mixamo FBX poses and render.
Run: blender -b --python Tools/pose_test_nyonya.py -- [--pose name] (default: all)
Renders to /tmp/opencode/nyonya-rig/pose_<name>.png
"""
import argparse
import sys
from pathlib import Path

import bpy
from mathutils import Euler, Matrix, Quaternion, Vector

TMP = Path('/tmp/opencode/nyonya-rig')
BLEND = TMP / 'nyonya_rig.blend'
POSES = TMP / 'poses'

SIDES = {'Left': '_L', 'Right': '_R'}
FINGERS = [('Thumb', 'Finger1'), ('Index', 'Finger2'), ('Middle', 'Finger3'),
           ('Ring', 'Finger4'), ('Pinky', 'Finger5')]
CORE = [('Hips', 'Hips'), ('Spine', 'Spine'), ('Spine1', 'Chest'), ('Spine2', 'UpperChest'),
        ('Neck', 'Neck'), ('Head', 'Head')]
LIMBS = [('Shoulder', 'Shoulder'), ('Arm', 'UpperArm'), ('ForeArm', 'Forearm'), ('Hand', 'Hand'),
         ('UpLeg', 'Thigh'), ('Leg', 'Shin'), ('Foot', 'Foot'), ('ToeBase', 'Toes')]


def build_map(src_bones, dst_bones):
    src, dst = set(src_bones), set(dst_bones)
    plain = {n.split(':')[-1]: n for n in src_bones}
    pairs = [(plain[a], b) for a, b in CORE if a in plain and b in dst]
    for side, suf in SIDES.items():
        for a, b in LIMBS:
            if side + a in plain and b + suf in dst:
                pairs.append((plain[side + a], b + suf))
        for fsrc, fdst in FINGERS:
            for seg in (1, 2, 3):
                if f'{side}Hand{fsrc}{seg}' in plain and f'{fdst}_{seg}{suf}' in dst:
                    pairs.append((plain[f'{side}Hand{fsrc}{seg}'], f'{fdst}_{seg}{suf}'))
    return pairs


ORDER = (['Hips', 'Spine', 'Chest', 'UpperChest', 'Neck', 'Head', 'Jaw', 'Eye_L', 'Eye_R']
         + [f'{a}{s}' for s in ('_L', '_R') for a in
            ('Thigh', 'Shin', 'Foot', 'Toes', 'Shoulder', 'UpperArm', 'Forearm', 'Hand')]
         + [f'Finger{f}_{g}{s}' for s in ('_L', '_R') for f in (1, 2, 3, 4, 5) for g in (1, 2, 3)])


def eval_pose_matrix(rig, bone_name):
    # PoseBone.matrix via view_layer reads back stale in background mode;
    # the evaluated depsgraph copy is the ground truth.
    dg = bpy.context.evaluated_depsgraph_get()
    return rig.evaluated_get(dg).pose.bones[bone_name].matrix.copy()


def snapshot_action_bases(src_rig, frame):
    """Per-bone local basis at `frame`, read straight from the action
    fcurves (never from evaluated pose state, which reads back stale in
    background mode). Returns {bone_name: Matrix}."""
    import re
    action = src_rig.animation_data.action
    loc, rotq, rote, scl = {}, {}, {}, {}
    if action:
        for fc in action.fcurves:
            m = re.match(r'pose\.bones\["([^"]+)"\]\.(location|rotation_quaternion|rotation_euler|scale)', fc.data_path)
            if not m:
                continue
            name, kind, idx = m.group(1), m.group(2), fc.array_index
            val = fc.evaluate(frame)
            if kind == 'location':
                loc.setdefault(name, [0.0, 0.0, 0.0])[idx] = val
            elif kind == 'rotation_quaternion':
                rotq.setdefault(name, [1.0, 0.0, 0.0, 0.0])[idx] = val
            elif kind == 'rotation_euler':
                rote.setdefault(name, [0.0, 0.0, 0.0])[idx] = val
            elif kind == 'scale':
                scl.setdefault(name, [1.0, 1.0, 1.0])[idx] = val
    bases = {}
    for b in src_rig.data.bones:
        n = b.name
        basis = Matrix.Translation(Vector(loc.get(n, (0, 0, 0))))
        if n in rotq:
            basis @= Quaternion(rotq[n]).normalized().to_matrix().to_4x4()
        elif n in rote:
            basis @= Euler(rote[n], 'XYZ').to_matrix().to_4x4()
        if n in scl:
            s = scl[n]
            basis @= Matrix.Diagonal((*s, 1.0))
        bases[n] = basis
    return bases


def fk_chain(rig, bases):
    """Armature-space pose matrices from explicit bases (pure math, no
    depsgraph reads). Returns {bone_name: Matrix}."""
    posed = {}
    arm_w = rig.matrix_world.copy()
    for b in rig.data.bones:
        if b.parent is None:
            posed[b.name] = arm_w @ bases.get(b.name, Matrix.Identity(4)) @ b.matrix_local
        else:
            posed[b.name] = (posed[b.parent.name] @ bases.get(b.name, Matrix.Identity(4))
                             @ b.matrix_local)
    return posed


def apply_pose(dst_rig, src_rig, frame):
    # Retarget via Blender's own Copy Rotation constraints (WORLD/WORLD
    # with offset = rest-difference preserving). Blender evaluates the
    # chain internally, so no hand-rolled basis math is needed at all.
    # Pure joint angles transfer; rest lengths never change.
    scene = bpy.context.scene
    mesh = next(o for o in bpy.context.scene.objects if o.type == 'MESH' and 'Nyonya' in o.name)
    for pb in dst_rig.pose.bones:
        for c in list(pb.constraints):
            pb.constraints.remove(c)
        pb.location = Vector((0, 0, 0))
        pb.rotation_mode = 'QUATERNION'
        pb.rotation_quaternion = Quaternion((1, 0, 0, 0))
    pair_of = dict(PAIRS)
    inv = {d: s for s, d in PAIRS}
    for dst_name in ORDER:
        if dst_name not in inv:
            continue
        src_name = inv[dst_name]
        c = dst_rig.pose.bones[dst_name].constraints.new('COPY_ROTATION')
        c.target = src_rig
        c.subtarget = src_name
        c.target_space = 'WORLD'
        c.owner_space = 'WORLD'
        c.use_offset = True
        c.mix_mode = 'REPLACE'
    scene.frame_set(frame)
    bpy.context.view_layer.update()
    mesh.hide_viewport = False
    bpy.context.view_layer.update()


def render_to(path, rig):
    scene = bpy.context.scene
    scene.render.engine = 'BLENDER_EEVEE'
    scene.render.resolution_x, scene.render.resolution_y = 640, 960
    scene.render.film_transparent = True
    cam = bpy.data.cameras.new('pc')
    co = bpy.data.objects.new('pc', cam)
    scene.collection.objects.link(co)
    scene.camera = co
    mesh = next(o for o in bpy.context.scene.objects if o.type == 'MESH' and 'Nyonya' in o.name)
    ws = [mesh.matrix_world @ v.co for v in mesh.data.vertices]
    cx = sum(p.x for p in ws) / len(ws)
    cy = sum(p.y for p in ws) / len(ws)
    cz = sum(p.z for p in ws) / len(ws)
    co.location = (cx, cy - 4.2, cz)
    d = Vector((cx, cy, cz)) - Vector(co.location)
    co.rotation_euler = d.to_track_quat('-Z', 'Y').to_euler()
    if 'psun' not in bpy.data.objects:
        sun = bpy.data.lights.new('psun', 'SUN')
        so = bpy.data.objects.new('psun', sun)
        scene.collection.objects.link(so)
        so.name = 'psun'
    scene.render.filepath = str(path)
    bpy.ops.render.render(write_still=True)
    bpy.data.objects.remove(co, do_unlink=True)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--pose', default='all')
    args = parser.parse_args(sys.argv[sys.argv.index('--') + 1:] if '--' in sys.argv else [])
    bpy.ops.wm.open_mainfile(filepath=str(BLEND))
    dst_rig = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
    names = [p for p in POSES.glob('*.fbx') if args.pose in ('all', p.stem)]
    global PAIRS
    for pose in sorted(names):
        for o in list(bpy.context.scene.objects):
            if o.name.startswith('mixamorig'):
                bpy.data.objects.remove(o, do_unlink=True)
        for a in list(bpy.data.actions):
            if a.users == 0:
                bpy.data.actions.remove(a)
        before = set(bpy.context.scene.objects)
        bpy.ops.import_scene.fbx(filepath=str(pose))
        src_rig = next(o for o in set(bpy.context.scene.objects) - before if o.type == 'ARMATURE')
        PAIRS = build_map([b.name for b in src_rig.data.bones],
                          [b.name for b in dst_rig.data.bones])
        print('POSE_MAP', pose.stem, len(PAIRS), flush=True)
        action = None
        if src_rig.animation_data and src_rig.animation_data.action:
            action = src_rig.animation_data.action
        frames = action.frame_range if action else (1, 1)
        mid = int((frames[0] + frames[1]) / 2)
        for pb in dst_rig.pose.bones:
            pb.rotation_mode = 'QUATERNION'
            pb.rotation_quaternion = Quaternion((1, 0, 0, 0))
            pb.location = Vector((0, 0, 0))
        apply_pose(dst_rig, src_rig, mid)
        render_to(TMP / f'pose_{pose.stem}.png', dst_rig)
        print('POSE_RENDERED', pose.stem, flush=True)
        bpy.data.objects.remove(src_rig, do_unlink=True)


if __name__ == '__main__':
    main()
