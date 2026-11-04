"""Manual 55-bone VRM-standard rig for the Meshy Nyonya sculpt.

Run: blender -b --python Tools/rig_nyonya_meshy.py -- --source <glb> [--stage draft|final]
--stage draft: rig + skin + export to /tmp/opencode/nyonya-rig/nyonya_rigged.fbx (pose validation only)
--stage final: also bake textures/FBX into Assets, apparitions and authoring record.

Bone placement is explicit per-bone (manual rigging), fitted to measured mesh
landmarks of THIS sculpt - no skin is transferred from any other model.
"""
import argparse
import json
import sys
from pathlib import Path

import bpy
import numpy as np
from mathutils import Matrix, Vector

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / 'Tools'))
import prepare_designer_vrm as P

TMP = Path('/tmp/opencode/nyonya-rig')
TARGET_TRIS = 22000
GAME_HEIGHT = 1.64


def mesh_landmarks(mesh):
    ws = [mesh.matrix_world @ v.co for v in mesh.data.vertices]
    xs = [p.x for p in ws]
    ys = [p.y for p in ws]
    zs = [p.z for p in ws]
    info = {'xmin': min(xs), 'xmax': max(xs), 'zmin': min(zs), 'zmax': max(zs)}
    # body centerline per z slice (torso mass only: |x| < 60% halfwidth, y all)
    hw = max(abs(info['xmin']), abs(info['xmax']))
    slices = {}
    z = info['zmin']
    while z < info['zmax']:
        band = [p for p in ws if abs(p.z - z) < 0.025 and abs(p.x) < hw * 0.62]
        if len(band) > 200:
            slices[round(z, 3)] = (sum(p.x for p in band) / len(band),
                                   sum(p.y for p in band) / len(band))
        z += 0.05

    def center(at_z):
        keys = sorted(slices)
        if at_z <= keys[0]:
            return slices[keys[0]]
        if at_z >= keys[-1]:
            return slices[keys[-1]]
        for a, b in zip(keys, keys[1:]):
            if a <= at_z <= b:
                t = (at_z - a) / (b - a)
                return (slices[a][0] * (1 - t) + slices[b][0] * t,
                        slices[a][1] * (1 - t) + slices[b][1] * t)
        return slices[keys[-1]]

    info['center'] = center
    # head blob: top verts, split hair mass vs skull by y (face side -y)
    ztop = info['zmax'] - 0.30 * (info['zmax'] - info['zmin'])
    top = [p for p in ws if p.z > ztop]
    info['head_centroid'] = tuple(sum(c) / len(top) for c in zip(*top))
    # arms: verts outside torso width, per side, in arm z range
    arms = {}
    for side, sign in [('L', 1), ('R', -1)]:
        cand = [p for p in ws if (p.x * sign) > hw * 0.55 and 0.0 < p.z < 1.45]
        if not cand:
            arms[side] = None
            continue
        cand.sort(key=lambda p: p.z)
        n = len(cand)
        top3 = cand[int(n * 0.66):]
        mid = cand[int(n * 0.33):int(n * 0.66)]
        bot = cand[:int(n * 0.33)]
        shoulder = (sum(p.x for p in top3) / len(top3), sum(p.y for p in top3) / len(top3),
                    max(p.z for p in top3))
        elbow = (sum(p.x for p in mid) / len(mid), sum(p.y for p in mid) / len(mid),
                 sum(p.z for p in mid) / len(mid))
        wrist = (sum(p.x for p in bot) / len(bot), sum(p.y for p in bot) / len(bot),
                 min(p.z for p in bot))
        hand = [p for p in bot if p.z < wrist[2] + 0.06]
        fingertip = (sum(p.x for p in hand) / len(hand), sum(p.y for p in hand) / len(hand),
                     min(p.z for p in hand)) if hand else wrist
        arms[side] = {'shoulder': shoulder, 'elbow': elbow, 'wrist': wrist, 'tip': fingertip,
                      'count': n}
    info['arms'] = arms
    # feet: lowest verts cluster in y (toes point -y)
    zbot = info['zmin'] + 0.06
    bot = [p for p in ws if p.z < zbot]
    info['foot_y'] = sum(p.y for p in bot) / len(bot)
    info['foot_x'] = sum(p.x for p in bot) / len(bot)
    info['ankle_z'] = info['zmin'] + 0.09
    # width profile: robust torso edge per z (median-x centered, IQR-based)
    prof = {}
    z = info['zmin']
    while z < info['zmax']:
        band = [p for p in ws if abs(p.z - z) < 0.025]
        if len(band) > 500:
            mx = sorted(p.x for p in band)[len(band) // 2]
            dev = sorted(abs(p.x - mx) for p in band)
            prof[round(z, 3)] = (mx, dev[int(len(dev) * 0.85)])
        z += 0.05
    info['profile'] = prof
    print('RIG_PROFILE', json.dumps({k: [round(v[0], 3), round(v[1], 3)] for k, v in prof.items()}),
          flush=True)
    info['halfwidth'] = hw
    return info


SIDES = ['L', 'R']


def build_armature(info):
    bpy.ops.object.armature_add(enter_editmode=True, location=(0, 0, 0))
    rig = bpy.context.view_layer.objects.active
    rig.name = 'NyonyaMeshyRig'
    bones = rig.data.edit_bones
    bpy.data.armatures[rig.data.name].edit_bones.remove(bones[0])
    defs = {}  # name -> (head, tail, parent or None)

    cx = info['center']
    prof = info['profile']
    fx, fy = info['foot_x'], info['foot_y']
    az = info['ankle_z']

    def edge(at_z, fallback):
        zs = sorted(prof)
        key = min(zs, key=lambda k: abs(k - at_z))
        if abs(key - at_z) > 0.08:
            return fallback
        return prof[key][1]

    # legs (dress hides them; symmetric, from measured ankle/feet)
    for side, s in [('L', 1), ('R', -1)]:
        lx = 0.09 * s
        defs[f'Thigh_{side}'] = (Vector((lx, cx(0.0)[1], 0.0)), Vector((lx, cx(-0.45)[1], -0.45)), 'Hips')
        defs[f'Shin_{side}'] = (Vector((lx, cx(-0.45)[1], -0.45)), Vector((lx, fy, az)), f'Thigh_{side}')
        defs[f'Foot_{side}'] = (Vector((lx, fy, az)), Vector((lx, fy - 0.02, az - 0.055)), f'Shin_{side}')
        defs[f'Toes_{side}'] = (Vector((lx, fy - 0.02, az - 0.055)), Vector((lx, fy - 0.14, az - 0.05)), f'Foot_{side}')
    # spine chain along measured centerline (sculpt hunches; bones follow it)
    defs['Hips'] = (Vector((cx(-0.05)[0], cx(-0.05)[1], -0.05)), Vector((cx(0.10)[0], cx(0.10)[1], 0.10)), None)
    defs['Spine'] = (Vector((cx(0.10)[0], cx(0.10)[1], 0.10)), Vector((cx(0.25)[0], cx(0.25)[1], 0.25)), 'Hips')
    defs['Chest'] = (Vector((cx(0.25)[0], cx(0.25)[1], 0.25)), Vector((cx(0.38)[0], cx(0.38)[1], 0.38)), 'Spine')
    defs['UpperChest'] = (Vector((cx(0.38)[0], cx(0.38)[1], 0.38)), Vector((cx(0.47)[0], cx(0.47)[1], 0.47)), 'Chest')
    defs['Neck'] = (Vector((cx(0.47)[0], cx(0.47)[1], 0.47)), Vector((cx(0.54)[0], cx(0.54)[1], 0.54)), 'UpperChest')
    # head follows the sculpted tilt (broken neck to her left)
    hx, hy, hz = info['head_centroid']
    nx, ny = cx(0.54)
    nz = 0.54
    head_dir = Vector((hx - nx, hy - ny, hz - nz))
    if head_dir.length < 0.05:
        head_dir = Vector((0.15, -0.05, 0.25))
    head_dir.normalize()
    head_base = Vector((nx, ny, nz))
    head_tip = head_base + head_dir * 0.30
    defs['Head'] = (head_base, head_tip, 'Neck')
    fwd = Vector((head_dir.x * 0.4, -1.0, head_dir.z * 0.4)).normalized()
    defs['Jaw'] = (head_base + head_dir * 0.16 + fwd * 0.05,
                   head_base + head_dir * 0.06 + fwd * 0.09, 'Head')
    for side, s in [('L', 1), ('R', -1)]:
        eye = head_base + head_dir * 0.20 + fwd * 0.10 + Vector((0.035 * s, 0, 0))
        defs[f'Eye_{side}'] = (eye, eye + fwd * 0.02, 'Head')
    # arms hang just outside the dress silhouette (measured clusters:
    # right forearm+shears at x~-0.2, left arm inside dress edge).
    # explicit symmetric hang; tube grabs the limb, not the skirt.
    for side in SIDES:
        s = 1 if side == 'L' else -1
        sh = (0.19 * s, cx(0.38)[1], 0.38)
        el = (0.21 * s, cx(0.03)[1] - 0.01, 0.03)
        wr = (0.232 * s, cx(-0.28)[1] - 0.02, -0.28)
        tip = (0.235 * s, -0.03, -0.38)
        sh, el, wr, tip = map(Vector, (sh, el, wr, tip))
        defs[f'Shoulder_{side}'] = (Vector((0.05 * s, cx(0.42)[1], 0.42)), sh, 'UpperChest')
        defs[f'UpperArm_{side}'] = (sh, el, f'Shoulder_{side}')
        defs[f'Forearm_{side}'] = (el, wr, f'UpperArm_{side}')
        defs[f'Hand_{side}'] = (wr, tip, f'Forearm_{side}')
        # fingers continue the hand direction, slight fan + curl
        hdir = (tip - wr)
        if hdir.length < 0.02:
            hdir = Vector((0, -0.01, -0.05))
        hdir.normalize()
        side_v = Vector((0.018 * s, 0, 0))
        for finger in range(1, 6):
            fx_off = (finger - 3) * 0.012 * s
            if finger == 1:  # thumb leaves from the wrist side
                base = wr + hdir * 0.015 + Vector((-0.018 * s, -0.012, 0.005))
                segs = [0.030, 0.026, 0.022]
                d = (hdir * 0.7 + Vector((-0.35 * s, -0.35, -0.1))).normalized()
            else:
                base = tip + Vector((fx_off, 0, 0.004))
                segs = [0.028, 0.024, 0.020]
                d = (hdir + Vector((fx_off * 2.0, 0, -0.15))).normalized()
            prev = base
            parent = f'Hand_{side}'
            for seg in range(1, 4):
                nxt = prev + d * segs[seg - 1]
                name = f'Finger{finger}_{seg}_{side}'
                defs[name] = (prev, nxt, parent)
                parent = name
                prev = nxt
                d = (d + Vector((0, 0, -0.12))).normalized()
    for name, (head, tail, parent) in defs.items():
        b = bones.new(name)
        b.head, b.tail = head, tail
    for name, (head, tail, parent) in defs.items():
        if parent:
            bones[name].parent = bones[parent]
    # sculpted-shears hook in her right grasp (Unity weapon binding)
    wr = bones['Hand_R'].head
    sh = bones.new('Shears')
    sh.head = wr + Vector((0, 0, 0.01))
    sh.tail = wr + Vector((-0.06, 0, -0.18))
    sh.parent = bones['Hand_R']
    # tails = child heads (pipeline convention); leaves keep explicit tails
    linked = {'Hips': 'Spine', 'Spine': 'Chest', 'Chest': 'UpperChest', 'UpperChest': 'Neck',
              'Neck': 'Head'}
    for side in SIDES:
        for a, b in [('Shoulder', 'UpperArm'), ('UpperArm', 'Forearm'), ('Forearm', 'Hand'),
                     ('Hand', 'Finger3_1'), ('Thigh', 'Shin'), ('Shin', 'Foot'), ('Foot', 'Toes')]:
            linked[a + '_' + side] = b + '_' + side
        for finger in range(1, 6):
            for seg in [1, 2]:
                linked[f'Finger{finger}_{seg}_{side}'] = f'Finger{finger}_{seg + 1}_{side}'
    for name, child in linked.items():
        bones[name].tail = bones[child].head
    for b in bones:
        if (b.tail - b.head).length < 0.004:
            b.tail = b.head + Vector((0, 0, 0.01))
    bpy.ops.object.mode_set(mode='OBJECT')
    return rig


def bone_segment_points(rig, names):
    return [(np.array(tuple(rig.data.bones[n].head_local)),
             np.array(tuple(rig.data.bones[n].tail_local)), n) for n in names]


def blend_weights(verts, gidx, segs, topk=6, power=2.0, soft=0.06):
    sub = verts[gidx]
    heads = np.array([h for h, t, n in segs])
    tails = np.array([t for h, t, n in segs])
    ab = tails - heads
    denom = np.sum(ab ** 2, axis=1)
    denom[denom < 1e-12] = 1e-12
    out = {}
    nseg = len(segs)
    k = min(topk, nseg)
    use_all = nseg <= topk
    for start in range(0, len(sub), 4096):
        chunk = sub[start:start + 4096]
        ap = chunk[:, None, :] - heads[None, :, :]
        t = np.clip(np.sum(ap * ab[None, :, :], axis=2) / denom[None, :], 0.0, 1.0)
        closest = heads[None, :, :] + t[:, :, None] * ab[None, :, :]
        dist = np.sqrt(np.sum((chunk[:, None, :] - closest) ** 2, axis=2))
        w = 1.0 / (dist + soft) ** power
        if use_all:
            idx = np.argsort(-w, axis=1)[:, :k]
        else:
            idx = np.argpartition(-w, k, axis=1)[:, :k]
        vals = np.take_along_axis(w, idx, axis=1)
        vals /= np.sum(vals, axis=1, keepdims=True) + 1e-12
        for r in range(len(chunk)):
            vi = int(gidx[start + r])
            out[vi] = [(segs[idx[r, j]][2], float(vals[r, j]))
                       for j in range(k) if vals[r, j] > 0.001]
    return out


def skin_heat(mesh, rig):
    # Bone-heat fails on the fused Meshy shells (dress+body+hair in one mesh),
    # and a global distance blend lets hanging arms steal dress verts.
    # So: anatomical region pools, distance blend WITHIN each pool.
    bpy.context.view_layer.update()
    for b in rig.data.bones:
        mesh.vertex_groups.new(name=b.name)
    spine_names = ['Hips', 'Spine', 'Chest', 'UpperChest', 'Thigh_L', 'Thigh_R',
                   'Shin_L', 'Shin_R', 'Foot_L', 'Foot_R', 'Toes_L', 'Toes_R',
                   'Shoulder_L', 'Shoulder_R', 'UpperArm_L', 'UpperArm_R']
    head_names = ['Head', 'Neck', 'Shoulder_L', 'Shoulder_R', 'UpperChest']
    spine_segs = bone_segment_points(rig, spine_names)
    head_segs = bone_segment_points(rig, head_names)
    arm_segs = {}
    for side in ('L', 'R'):
        segs = bone_segment_points(rig, [f'Shoulder_{side}', f'UpperArm_{side}',
                                         f'Forearm_{side}', f'Hand_{side}',
                                         'UpperChest', 'Chest'])
        for f in range(1, 6):
            for g in range(1, 4):
                segs.extend(bone_segment_points(rig, [f'Finger{f}_{g}_{side}']))
        arm_segs[side] = segs
    # sculpted shears blade hangs below her right grip: rigid with the hand
    # (scaled space: handle at the wrist, tip down-left at z~0.33)
    wrist_r = np.array(tuple(rig.data.bones['Hand_R'].head_local))
    blade_tip = np.array([-0.270, wrist_r[1] - 0.05, 0.330])
    arm_segs['R'].append((wrist_r, blade_tip, 'Hand_R'))
    verts = np.array([tuple(v.co) for v in mesh.data.vertices], dtype=np.float64)
    groups = mesh.vertex_groups
    # NOTE: skinning runs AFTER the 1.64 m scale, so all thresholds below
    # are in scaled space (feet z~0, head top z~1.64).
    region = np.full(len(verts), 'body', dtype=object)
    region[verts[:, 2] > 1.25] = 'head'
    region[(verts[:, 2] > 1.10) & (verts[:, 2] <= 1.25)] = 'neck'
    for side, s in (('L', 1), ('R', -1)):
        segs = np.array([(h, t) for h, t, n in arm_segs[side]])
        h = segs[:, 0, :]
        t = segs[:, 1, :]
        ab = t - h
        dd = np.sum(ab ** 2, axis=1) + 1e-12
        ap = verts[:, None, :] - h[None, :, :]
        tt = np.clip(np.sum(ap * ab[None, :, :], axis=2) / dd[None, :], 0.0, 1.0)
        cl = h[None, :, :] + tt[:, :, None] * ab[None, :, :]
        dmin = np.sqrt(np.sum((verts[:, None, :] - cl) ** 2, axis=2)).min(axis=1)
        tube = (dmin < 0.09) & (verts[:, 2] < 1.18)
        region[tube & (verts[:, 0] * s > 0)] = 'arm' + side
    pools = {'head': head_segs, 'neck': head_segs, 'body': spine_segs,
             'armL': arm_segs['L'], 'armR': arm_segs['R']}
    for name in ('head', 'neck', 'body', 'armL', 'armR'):
        idx = np.where(region == name)[0]
        if len(idx) == 0:
            continue
        res = blend_weights(verts, idx, pools[name])
        for vi, weights in res.items():
            for bn, w in weights:
                groups[bn].add([vi], w, 'REPLACE')
        print('RIG_REGION', name, len(idx), flush=True)
    # topological smoothing: average each vert with its edge-neighbours so
    # no single edge spans a weight cliff (the flat-sheet artefact)
    used = sorted({g.group for v in mesh.data.vertices for g in v.groups})
    col = {gi: c for c, gi in enumerate(used)}
    nverts = len(mesh.data.vertices)
    Wmat = np.zeros((nverts, len(used)))
    for v in mesh.data.vertices:
        for g in v.groups:
            Wmat[v.index, col[g.group]] = g.weight
    maxdeg = 0
    tmp = [[] for _ in range(nverts)]
    for e in mesh.data.edges:
        a, b = e.vertices
        tmp[a].append(b)
        tmp[b].append(a)
        maxdeg = max(maxdeg, len(tmp[a]), len(tmp[b]))
    nbr = np.full((nverts, maxdeg), -1)
    for i, lst in enumerate(tmp):
        nbr[i, :len(lst)] = lst
    for _ in range(2):
        acc = Wmat.copy()
        for k in range(maxdeg):
            m = nbr[:, k] >= 0
            acc[m] += 0.5 * Wmat[nbr[m, k]]
        acc /= acc.sum(axis=1, keepdims=True) + 1e-12
        Wmat = acc
    for v in mesh.data.vertices:
        for g in list(v.groups):
            mesh.vertex_groups[g.group].remove([v.index])
    colname = {c: gi for gi, c in col.items()}
    for vi in range(nverts):
        for c, w in enumerate(Wmat[vi]):
            if w > 0.0005:
                mesh.vertex_groups[colname[c]].add([vi], float(w), 'REPLACE')
    print('RIG_STAGE smoothed', flush=True)
    print('RIG_STAGE skinned', len(verts), flush=True)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--source', type=Path, required=True)
    parser.add_argument('--stage', choices=['draft', 'final'], default='draft')
    args = parser.parse_args(sys.argv[sys.argv.index('--') + 1:] if '--' in sys.argv else [])
    bpy.ops.wm.read_factory_settings(use_empty=True)
    gltf = P.read_gltf(args.source)
    bpy.ops.import_scene.gltf(filepath=str(args.source))
    rigs = [o for o in bpy.context.scene.objects if o.type == 'ARMATURE']
    for r in rigs:
        bpy.data.objects.remove(r, do_unlink=True)
    mesh = next(o for o in bpy.context.scene.objects if o.type == 'MESH')
    print('RIG_STAGE imported', mesh.name, len(mesh.data.vertices), flush=True)
    if args.stage == 'draft':
        TMP.mkdir(parents=True, exist_ok=True)
    tex = P.textures('Nyonya', gltf)
    print('RIG_STAGE textures', flush=True)
    if args.stage == 'draft':
        # keep the live game textures untouched; park draft copies in TMP
        import shutil, subprocess
        for key in ('Albedo', 'Normal', 'MetalSmooth'):
            live = ROOT / tex[key]
            if live.exists():
                shutil.move(str(live), str(TMP / live.name))
        subprocess.run(['git', 'checkout', '--',
                        'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Textures/NyonyaAlbedo.png',
                        'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Textures/NyonyaNormal.png',
                        'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Textures/NyonyaMetalSmooth.png'],
                       cwd=str(ROOT), check=False)
    mesh.name = 'NyonyaDesignerBody'
    mesh.data.materials[0].name = 'NyonyaDesignerSurface'
    original = P.simplify(mesh, TARGET_TRIS)
    print('RIG_STAGE simplified', len(mesh.data.polygons), flush=True)
    info = mesh_landmarks(mesh)
    print('RIG_LANDMARKS', json.dumps({k: (list(v) if isinstance(v, tuple) else v)
                                       for k, v in info.items() if k != 'center'}, default=str), flush=True)
    rig = build_armature(info)
    print('RIG_STAGE armature', len(rig.data.bones), flush=True)
    # scale to game height, feet on zero
    ws = [mesh.matrix_world @ v.co for v in mesh.data.vertices]
    h = max(p.z for p in ws) - min(p.z for p in ws)
    change = Matrix.Scale(GAME_HEIGHT / h, 4) @ Matrix.Translation(Vector((0, 0, -min(p.z for p in ws))))
    for v in mesh.data.vertices:
        v.co = change @ v.co
    bpy.context.view_layer.objects.active = rig
    bpy.ops.object.mode_set(mode='EDIT')
    ow = rig.matrix_world.copy()
    for b in rig.data.edit_bones:
        b.head = change @ ow @ b.head
        b.tail = change @ ow @ b.tail
    rig.matrix_world = Matrix.Identity(4)
    bpy.ops.object.mode_set(mode='OBJECT')
    rig.name = 'CharacterRig'
    skin_heat(mesh, rig)
    # every vert must be weighted; nearest-bone fallback for heat misses
    unweighted = [v for v in mesh.data.vertices if not v.groups]
    print('RIG_STAGE unweighted', len(unweighted), flush=True)
    if unweighted:
        bnames = [b.name for b in rig.data.bones]
        bheads = np.array([tuple(rig.data.bones[n].head_local) for n in bnames])
        for v in unweighted:
            d = np.sum((bheads - np.array(tuple(v.co))) ** 2, axis=1)
            g = mesh.vertex_groups.get(bnames[int(np.argmin(d))])
            g.add([v.index], 1.0, 'REPLACE')
    P.bind(mesh, rig)
    bpy.context.view_layer.update()
    defs = {b.name: ([round(v, 4) for v in b.head_local], [round(v, 4) for v in b.tail_local],
                     b.parent.name if b.parent else None) for b in rig.data.bones}
    print('RIG_BONES', len(defs), sorted(defs), flush=True)
    if args.stage == 'draft':
        TMP.mkdir(parents=True, exist_ok=True)
        out = TMP / 'nyonya_rigged.fbx'
        P.export_fbx(out, [mesh, rig], False)
        bpy.ops.wm.save_as_mainfile(filepath=str(TMP / 'nyonya_rig.blend'))
        print('RIG_DRAFT_EXPORTED', str(out), flush=True)
    else:
        # Final: sculpted blade rides rigidly with the grip, so the Hand_R
        # driven Shears bone (Unity weapon hook) carries it in every clip.
        # Blade line measured on the baked mesh (scaled space): grip
        # (-0.21,-0.076,0.55) to tip (-0.358,-0.228,0.05). Thin tube only:
        # a box here would eat dress skirt.
        handle = np.array([-0.210, -0.076, 0.550])
        tip = np.array([-0.358, -0.228, 0.050])
        direction = tip - handle
        denom = float(np.dot(direction, direction)) + 1e-12
        sg = mesh.vertex_groups['Shears']
        moved = 0
        for v in mesh.data.vertices:
            p = np.array(tuple(v.co))
            t = float(np.clip(np.dot(p - handle, direction) / denom, 0.0, 1.0))
            if np.sum((p - (handle + t * direction)) ** 2) > 0.035 ** 2:
                continue
            for g in list(v.groups):
                mesh.vertex_groups[g.group].remove([v.index])
            sg.add([v.index], 1.0, 'REPLACE')
            moved += 1
        print('RIG_SHEARS_BOUND', moved, flush=True)
        for o in bpy.context.scene.objects:
            if o.type == 'MESH':
                o.hide_viewport = True
        clips = P.nyonya_actions(rig)
        for o in bpy.context.scene.objects:
            if o.type == 'MESH':
                o.hide_viewport = False
        objects = [o for o in bpy.context.scene.objects if o.type in {'ARMATURE', 'MESH', 'EMPTY'}]
        P.export_fbx(P.MODEL / 'NyonyaDesigner.fbx', objects, True)
        P.apparition(rig, mesh, 'Echo')
        P.apparition(rig, mesh, 'Seated')
        game_tris = sum(len(p.vertices) - 2 for p in mesh.data.polygons)
        record = {'role': 'Nyonya', 'sourceFile': args.source.name,
                  'sourceSha256': P.digest(args.source),
                  'sourceBytes': args.source.stat().st_size,
                  'originalTriangles': original,
                  'gameTriangles': game_tris, 'gameVertices': len(mesh.data.vertices),
                  'bones': len(rig.data.bones), 'sourceHumanoidBones': 55,
                  'heightMetres': GAME_HEIGHT, 'textures': tex, 'clips': clips,
                  'sharedPoseSourceSha256': P.digest(P.pose_source),
                  'rig': 'manual-55-meshy'}
        report = ROOT / 'Audit/designer-vrm-authoring.json'
        records = json.loads(report.read_text()) if report.exists() else {'characters': {}}
        records['characters']['Nyonya'] = record
        report.write_text(json.dumps(records, indent=2) + '\n')
        print('RIG_FINAL_EXPORTED', json.dumps(record), flush=True)


if __name__ == '__main__':
    main()
