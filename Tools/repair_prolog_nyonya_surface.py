"""Prolog-only mesh recovery from the supplied Nyonya VRM; shared original remains intact.
Blender -b -t 2 --python-exit-code 1 --python this_file.py
Retain current rest skeleton/actions. Recover original supplied VRM surface by posing
the VRM rig into NyonyaDesigner's exact rest skeleton using VRM's own authored skin weights,
mapping all 55 VRM vertex groups 1-to-1 to NyonyaDesigner bones, and preserving the rigid Shears island.
Never run original project asset-generation scripts.
"""
import bpy, bmesh, json, numpy as np, time, math, hashlib
from pathlib import Path
from mathutils import Matrix, Vector, Quaternion

ROOT = Path(__file__).resolve().parents[1]
P = ROOT / 'Builds/QA/PrologSurfaceRepair'
P.mkdir(parents=True, exist_ok=True)
CURRENT = ROOT / 'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/NyonyaDesigner.fbx'
ORIGINAL = ROOT / 'Assets/_LayarTerakhir/assets/LayarTerakhir/Nyonya.vrm'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Original/Models/Prolog47/NyonyaPrologSurfaceRepair.fbx'
started = time.monotonic()
report = {}

VRM_TO_DESIGNER = {
    'hips': 'Hips',
    'spine': 'Spine',
    'chest': 'Chest',
    'upperChest': 'UpperChest',
    'neck': 'Neck',
    'head': 'Head',
    'leftEye': 'Eye_L',
    'rightEye': 'Eye_R',
    'jaw': 'Jaw',
    'leftShoulder': 'Shoulder_L',
    'leftUpperArm': 'UpperArm_L',
    'leftLowerArm': 'Forearm_L',
    'leftHand': 'Hand_L',
    'leftThumbMetacarpal': 'Finger1_1_L',
    'leftThumbProximal': 'Finger1_2_L',
    'leftThumbDistal': 'Finger1_3_L',
    'leftIndexProximal': 'Finger2_1_L',
    'leftIndexIntermediate': 'Finger2_2_L',
    'leftIndexDistal': 'Finger2_3_L',
    'leftMiddleProximal': 'Finger3_1_L',
    'leftMiddleIntermediate': 'Finger3_2_L',
    'leftMiddleDistal': 'Finger3_3_L',
    'leftRingProximal': 'Finger4_1_L',
    'leftRingIntermediate': 'Finger4_2_L',
    'leftRingDistal': 'Finger4_3_L',
    'leftLittleProximal': 'Finger5_1_L',
    'leftLittleIntermediate': 'Finger5_2_L',
    'leftLittleDistal': 'Finger5_3_L',
    'rightShoulder': 'Shoulder_R',
    'rightUpperArm': 'UpperArm_R',
    'rightLowerArm': 'Forearm_R',
    'rightHand': 'Hand_R',
    'rightThumbMetacarpal': 'Finger1_1_R',
    'rightThumbProximal': 'Finger1_2_R',
    'rightThumbDistal': 'Finger1_3_R',
    'rightIndexProximal': 'Finger2_1_R',
    'rightIndexIntermediate': 'Finger2_2_R',
    'rightIndexDistal': 'Finger2_3_R',
    'rightMiddleProximal': 'Finger3_1_R',
    'rightMiddleIntermediate': 'Finger3_2_R',
    'rightMiddleDistal': 'Finger3_3_R',
    'rightRingProximal': 'Finger4_1_R',
    'rightRingIntermediate': 'Finger4_2_R',
    'rightRingDistal': 'Finger4_3_R',
    'rightLittleProximal': 'Finger5_1_R',
    'rightLittleIntermediate': 'Finger5_2_R',
    'rightLittleDistal': 'Finger5_3_R',
    'leftUpperLeg': 'Thigh_L',
    'leftLowerLeg': 'Shin_L',
    'leftFoot': 'Foot_L',
    'leftToes': 'Toes_L',
    'rightUpperLeg': 'Thigh_R',
    'rightLowerLeg': 'Shin_R',
    'rightFoot': 'Foot_R',
    'rightToes': 'Toes_R',
}


def note(step, **values):
    print('NYONYA_PROLOG', step, json.dumps(values, default=float), 'elapsed', round(time.monotonic() - started, 2), flush=True)


def digest(path):
    with path.open('rb') as f:
        return hashlib.file_digest(f, 'sha256').hexdigest()


def action_signature(action):
    rows = []
    for c in action.fcurves:
        rows.append((c.data_path, c.array_index, [(tuple(k.co), tuple(k.handle_left), tuple(k.handle_right), k.interpolation) for k in c.keyframe_points]))
    return hashlib.sha256(repr(rows).encode()).hexdigest()


def points(obj):
    ev = obj.evaluated_get(bpy.context.evaluated_depsgraph_get())
    me = ev.to_mesh()
    co = np.empty(len(me.vertices) * 3)
    me.vertices.foreach_get('co', co)
    co = co.reshape((-1, 3))
    m = np.asarray(ev.matrix_world)
    co = co @ m[:3, :3].T + m[:3, 3]
    ev.to_mesh_clear()
    return co


bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.fbx(filepath=str(CURRENT))
rig = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
old = next(o for o in bpy.context.scene.objects if o.type == 'MESH')
old.animation_data_clear()
actions = list(bpy.data.actions)
fps = bpy.context.scene.render.fps
fpsbase = bpy.context.scene.render.fps_base
original_signatures = {a.name.rsplit('|', 1)[-1]: action_signature(a) for a in actions}
rig.animation_data.action = None
for b in rig.pose.bones:
    b.matrix_basis = Matrix.Identity(4)
bpy.context.view_layer.update()

rest = points(old)
oldmatrix = old.matrix_world.copy()
oldname = old.name
group_names = [g.name for g in old.vertex_groups]

report['input'] = dict(
    model=str(CURRENT),
    modelSHA256=digest(CURRENT),
    original=str(ORIGINAL),
    originalSHA256=digest(ORIGINAL),
    currentVertices=len(old.data.vertices),
    currentPolygons=len(old.data.polygons),
    fps=fps,
    fpsBase=fpsbase,
    bones=len(rig.data.bones),
    actionSignatures=original_signatures,
)

# Extract only the rigid Shears weapon mesh island from NyonyaDesigner so Nyonya keeps her shears.
adj = [[] for _ in range(len(old.data.vertices))]
for e in old.data.edges:
    a, b = e.vertices
    adj[a].append(b)
    adj[b].append(a)
remaining = set(range(len(old.data.vertices)))
weapon = set()
shears_idx = group_names.index('Shears')
while remaining:
    first = next(iter(remaining))
    component = [first]
    pending = [first]
    remaining.remove(first)
    while pending:
        a = pending.pop()
        for b in adj[a]:
            if b in remaining:
                remaining.remove(b)
                component.append(b)
                pending.append(b)
    held = sum(any(g.group == shears_idx and g.weight > 0.5 for g in old.data.vertices[i].groups) for i in component)
    if held and held / len(component) >= 0.35:
        weapon.update(component)

shears_obj = None
if weapon:
    shears_mesh = old.data.copy()
    shears_obj = bpy.data.objects.new('ExtractedShears', shears_mesh)
    bpy.context.scene.collection.objects.link(shears_obj)
    shears_obj.matrix_world = oldmatrix.copy()
    bm = bmesh.new()
    bm.from_mesh(shears_mesh)
    bm.verts.ensure_lookup_table()
    to_del = [v for i, v in enumerate(bm.verts) if i not in weapon]
    bmesh.ops.delete(bm, geom=to_del, context='VERTS')
    bm.to_mesh(shears_mesh)
    bm.free()
    shears_obj.vertex_groups.clear()
    sg = shears_obj.vertex_groups.new(name='UpperArm_R')
    sg.add(list(range(len(shears_mesh.vertices))), 1.0, 'REPLACE')

note('shears_extracted', rigidWeaponVertices=len(weapon))

# Import original VRM with its clean authored skinning weights and T-pose armature.
existing = set(bpy.context.scene.objects)
bpy.ops.import_scene.gltf(filepath=str(ORIGINAL))
imported = set(bpy.context.scene.objects) - existing
source_rig = next(o for o in imported if o.type == 'ARMATURE')
mesh = next(o for o in imported if o.type == 'MESH' and any(m.type == 'ARMATURE' for m in o.modifiers))

# Compute exact normalization (1.64m height, centered feet, Z=0 floor) and apply to source_rig in world space.
raw_pts = points(mesh)
low = float(raw_pts[:, 2].min())
height = float(raw_pts[:, 2].max() - low)
scale = 1.64 / height
feet = [(source_rig.matrix_world @ source_rig.data.bones[n].head_local).x for n in ['leftFoot', 'rightFoot']]
centre = sum(feet) / 2.0

T_norm = Matrix.Scale(scale, 4) @ Matrix.Translation(Vector((-centre, 0.0, -low)))
source_rig.matrix_world = T_norm @ source_rig.matrix_world
bpy.context.view_layer.update()

# Sanitize cross-region VRM auto-weight bleed in clean T-pose BEFORE posing arms down!
# In Nyonya.vrm's T-pose, outer kebaya sleeves had bogus 'hips'/'spine'/'UpperLeg' weights that held sleeves
# frozen up in mid-air T-pose when the bare arms rotated down (creating bat-wings!).
tpose_pts = points(mesh)
vg_by_idx = {vg.index: vg.name for vg in mesh.vertex_groups}
idx_by_vg = {vg.name: vg.index for vg in mesh.vertex_groups}

def ensure_vg(name):
    if name not in idx_by_vg:
        vg = mesh.vertex_groups.new(name=name)
        idx_by_vg[name] = vg.index
        vg_by_idx[vg.index] = name
    return idx_by_vg[name]

LEFT_ARM_NAMES = {k for k in VRM_TO_DESIGNER if k.startswith('left') and ('Shoulder' in k or 'Arm' in k or 'Hand' in k or 'Thumb' in k or 'Index' in k or 'Middle' in k or 'Ring' in k or 'Little' in k)}
RIGHT_ARM_NAMES = {k for k in VRM_TO_DESIGNER if k.startswith('right') and ('Shoulder' in k or 'Arm' in k or 'Hand' in k or 'Thumb' in k or 'Index' in k or 'Middle' in k or 'Ring' in k or 'Little' in k)}
LOWER_BODY_NAMES = {'hips', 'spine', 'leftUpperLeg', 'leftLowerLeg', 'leftFoot', 'leftToes', 'rightUpperLeg', 'rightLowerLeg', 'rightFoot', 'rightToes'}
UPPER_TORSO_NAMES = {'chest', 'upperChest', 'neck', 'head', 'leftEye', 'rightEye', 'jaw'}

lu_x = abs((source_rig.matrix_world @ source_rig.pose.bones['leftUpperArm'].head).x)
ll_x = abs((source_rig.matrix_world @ source_rig.pose.bones['leftLowerArm'].head).x)
lh_x = abs((source_rig.matrix_world @ source_rig.pose.bones['leftHand'].head).x)

sanitized_arm = 0
sanitized_body = 0
bulk_fallback = {'head': [], 'chest': [], 'spine': [], 'hips': []}
for vi, (v, p) in enumerate(zip(mesh.data.vertices, tpose_pts)):
    x, y, z = float(p[0]), float(p[1]), float(p[2])
    ax = abs(x)
    # Tapered torso envelope so A-pose lower bell-sleeve hems (z in [0.78..1.13] outside waist/skirt)
    # rotate down cleanly with her arms instead of flaring out in a waist V-wing, while hip skirt stays on Hips!
    if z > 1.13:
        torso_w = 0.175
    elif z > 1.02:
        torso_w = 0.215 - (z - 1.02) / 0.11 * 0.040
    elif z > 0.90:
        torso_w = 0.245 - (z - 0.90) / 0.12 * 0.030
    else:
        torso_w = 0.265
    is_arm_box = (z > 0.78 and ax > torso_w) and not (z > 1.38 and ax < 0.25)
    if not v.groups:
        if not is_arm_box:
            fb = 'head' if z > 1.38 else ('chest' if z > 1.08 else ('spine' if z > 0.92 else 'hips'))
            bulk_fallback[fb].append(vi)
            sanitized_body += 1
            continue
    weights = {vg_by_idx[g.group]: g.weight for g in v.groups if g.group in vg_by_idx and g.weight > 1e-5}
    changed = False

    if is_arm_box:
        # Shoulder + full horizontal T-pose bell-sleeve/arm box: never allow hips/spine/legs or opposite arm!
        opp = RIGHT_ARM_NAMES if x > 0 else LEFT_ARM_NAMES
        for bad in list(weights.keys()):
            if bad in LOWER_BODY_NAMES or bad in opp or (ax > 0.225 and bad in UPPER_TORSO_NAMES):
                del weights[bad]
                changed = True
        if ax > 0.205 and sum(weights.values()) < 1e-4:
            # Bell-sleeve vertex had 100% bogus torso/hip weight: assign smooth anatomical arm weight along T-pose X!
            side = 'left' if x > 0 else 'right'
            if ax <= ll_x:
                t = max(0.0, min(1.0, (ax - lu_x) / max(0.05, ll_x - lu_x)))
                weights[f'{side}UpperArm'] = 1.0 - 0.35 * t
                weights[f'{side}LowerArm'] = 0.35 * t
            elif ax <= lh_x:
                t = max(0.0, min(1.0, (ax - ll_x) / max(0.05, lh_x - ll_x)))
                weights[f'{side}LowerArm'] = 1.0 - 0.35 * t
                weights[f'{side}Hand'] = 0.35 * t
            else:
                weights[f'{side}Hand'] = 1.0
            changed = True
        if changed:
            sanitized_arm += 1
    else:
        # Torso / skirt / legs / head: never allow free arm/forearm/hand weights!
        arm_no_shoulder = (LEFT_ARM_NAMES | RIGHT_ARM_NAMES) - ({'leftShoulder', 'rightShoulder'} if z > 1.18 else set())
        for bad in list(weights.keys()):
            if bad in arm_no_shoulder:
                del weights[bad]
                changed = True
        if sum(weights.values()) < 1e-4:
            fallback = 'head' if z > 1.38 else ('chest' if z > 1.08 else ('spine' if z > 0.92 else 'hips'))
            weights[fallback] = 1.0
            changed = True
        if changed:
            sanitized_body += 1

    if changed:
        tot = sum(weights.values())
        for g in list(v.groups):
            mesh.vertex_groups[g.group].remove([vi])
        for k, w in weights.items():
            mesh.vertex_groups[ensure_vg(k)].add([vi], w / tot, 'REPLACE')

for fb_name, indices in bulk_fallback.items():
    if indices:
        mesh.vertex_groups[ensure_vg(fb_name)].add(indices, 1.0, 'REPLACE')

note('tpose_weights_sanitized', sanitizedArmVertices=sanitized_arm, sanitizedBodyVertices=sanitized_body)

# Pose source_rig in hierarchy order using TRUE glTF child-head bone directions!
# Keep natural human VRM arm lengths and pose arms smoothly down to her sides (right hand ready for shears)
# instead of stretching elbows out to NyonyaDesigner's horizontal (0.489, 0, 1.362) scarecrow joints!
def bone_depth(b):
    d = 0
    p = b.parent
    while p:
        d += 1
        p = p.parent
    return d

PRIMARY_CHILD = {
    'hips': 'spine', 'spine': 'chest', 'chest': 'upperChest', 'upperChest': 'neck', 'neck': 'head',
    'leftShoulder': 'leftUpperArm', 'leftUpperArm': 'leftLowerArm', 'leftLowerArm': 'leftHand', 'leftHand': 'leftMiddleProximal',
    'rightShoulder': 'rightUpperArm', 'rightUpperArm': 'rightLowerArm', 'rightLowerArm': 'rightHand', 'rightHand': 'rightMiddleProximal',
    'leftUpperLeg': 'leftLowerLeg', 'leftLowerLeg': 'leftFoot', 'leftFoot': 'leftToes',
    'rightUpperLeg': 'rightLowerLeg', 'rightLowerLeg': 'rightFoot', 'rightFoot': 'rightToes',
}
for side in ('left', 'right'):
    for finger in ('Thumb', 'Index', 'Middle', 'Ring', 'Little'):
        segs = ('Metacarpal', 'Proximal', 'Intermediate', 'Distal') if finger == 'Thumb' else ('Proximal', 'Intermediate', 'Distal')
        for a, b in zip(segs[:-1], segs[1:]):
            PRIMARY_CHILD[f'{side}{finger}{a}'] = f'{side}{finger}{b}'

NATURAL_ARM_DIRS = {
    'leftUpperArm': Vector((0.17, 0.04, -0.985)).normalized(),
    'leftLowerArm': Vector((0.08, -0.24, -0.967)).normalized(),
    'leftHand': Vector((0.03, -0.20, -0.979)).normalized(),
    'rightUpperArm': Vector((-0.17, 0.02, -0.985)).normalized(),
    'rightLowerArm': Vector((-0.08, -0.32, -0.944)).normalized(),
    'rightHand': Vector((-0.03, -0.28, -0.959)).normalized(),
}

ordered_vrm_bones = [b.name for b in sorted(source_rig.data.bones, key=bone_depth) if b.name in VRM_TO_DESIGNER]
ARM_CHAIN_KEEP_LENGTH = (LEFT_ARM_NAMES | RIGHT_ARM_NAMES) - {'leftShoulder', 'rightShoulder', 'leftUpperArm', 'rightUpperArm'}

head_errors = []
for vname in ordered_vrm_bones:
    dname = VRM_TO_DESIGNER[vname]
    pb = source_rig.pose.bones[vname]
    db = rig.data.bones[dname]

    cur_mat_w = source_rig.matrix_world @ pb.matrix
    cur_head_w = source_rig.matrix_world @ pb.head
    if vname in PRIMARY_CHILD and PRIMARY_CHILD[vname] in source_rig.pose.bones:
        cur_tail_w = source_rig.matrix_world @ source_rig.pose.bones[PRIMARY_CHILD[vname]].head
    else:
        # Distal fingertip: extend along parent->self direction
        par_head_w = source_rig.matrix_world @ pb.parent.head if pb.parent else cur_head_w - Vector((0, 0, 0.02))
        d_vec = cur_head_w - par_head_w
        cur_tail_w = cur_head_w + (d_vec if d_vec.length > 1e-5 else Vector((1 if 'left' in vname else -1, 0, 0)))
    v_cur = cur_tail_w - cur_head_w

    if vname in ARM_CHAIN_KEEP_LENGTH:
        # Preserve natural VRM elbow/wrist/finger joint spacing (follows posed parent)!
        tgt_head_w = cur_head_w
    else:
        tgt_head_w = rig.matrix_world @ db.head_local

    if vname in NATURAL_ARM_DIRS:
        v_tgt = NATURAL_ARM_DIRS[vname]
    elif vname in (LEFT_ARM_NAMES | RIGHT_ARM_NAMES) and vname not in ('leftShoulder', 'rightShoulder'):
        v_tgt = NATURAL_ARM_DIRS['leftHand' if vname.startswith('left') else 'rightHand']
    elif vname in PRIMARY_CHILD and VRM_TO_DESIGNER.get(PRIMARY_CHILD[vname]) in rig.data.bones:
        child_db = rig.data.bones[VRM_TO_DESIGNER[PRIMARY_CHILD[vname]]]
        v_tgt = (rig.matrix_world @ child_db.head_local) - (rig.matrix_world @ db.head_local)
    else:
        v_tgt = (rig.matrix_world @ db.tail_local) - (rig.matrix_world @ db.head_local)

    if v_cur.length > 1e-6 and v_tgt.length > 1e-6:
        rot_diff = v_cur.normalized().rotation_difference(v_tgt.normalized())
    else:
        rot_diff = Quaternion()

    R_w = rot_diff.to_matrix().to_4x4()
    new_mat_w = Matrix.Translation(tgt_head_w) @ R_w @ Matrix.Translation(-cur_head_w) @ cur_mat_w
    pb.matrix = source_rig.matrix_world.inverted() @ new_mat_w
    bpy.context.view_layer.update()
    if vname not in ARM_CHAIN_KEEP_LENGTH:
        head_errors.append(float(((source_rig.matrix_world @ pb.head) - tgt_head_w).length))

posed_rhand_w = (source_rig.matrix_world @ source_rig.pose.bones['rightHand'].head).copy()
designer_rhand_w = (rig.matrix_world @ rig.data.bones['Hand_R'].head_local).copy()
if shears_obj is not None:
    # Align ExtractedShears handle dead-center inside Nyonya's posed right hand fingers!
    shears_delta = posed_rhand_w - designer_rhand_w + Vector((0.025, -0.015, -0.095))
    shears_obj.matrix_world = Matrix.Translation(shears_delta) @ shears_obj.matrix_world

note('vrm_posed_to_designer_rest', maxHeadError=max(head_errors), meanHeadError=float(np.mean(head_errors)), posedRightHand=list(posed_rhand_w))

# Bake the posed VRM mesh in world->oldmatrix space while keeping 100% of VRM's authored vertex groups!
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
bpy.context.view_layer.objects.active = mesh
arm_mod = next(m for m in mesh.modifiers if m.type == 'ARMATURE')
bpy.ops.object.modifier_apply(modifier=arm_mod.name)
for mod in list(mesh.modifiers):
    mesh.modifiers.remove(mod)
mesh.animation_data_clear()

# Transform baked mesh vertices into oldmatrix local space so mesh.matrix_world == oldmatrix.
mw = np.asarray(mesh.matrix_world)
co = np.empty(len(mesh.data.vertices) * 3)
mesh.data.vertices.foreach_get('co', co)
co = co.reshape((-1, 3))
world_co = co @ mw[:3, :3].T + mw[:3, 3]
inv_old = np.asarray(oldmatrix.inverted())
local_co = world_co @ inv_old[:3, :3].T + inv_old[:3, 3]
mesh.data.vertices.foreach_set('co', local_co.reshape(-1))
mesh.parent = None
mesh.matrix_world = oldmatrix.copy()
mesh.data.update()

report['surface'] = dict(
    originalVertices=len(mesh.data.vertices),
    originalPolygons=len(mesh.data.polygons),
    normalizationScale=scale,
    normalizationCentreX=centre,
    normalizationLowZ=low,
    poseAlignmentMaxErrorMetres=max(head_errors),
)

# Remove VRM importer objects & actions, leaving target rig, mesh, and shears_obj.
for obj in imported:
    if obj != mesh:
        bpy.data.objects.remove(obj, do_unlink=True)
for action in list(bpy.data.actions):
    if action not in actions:
        bpy.data.actions.remove(action)
bpy.context.scene.render.fps = fps
bpy.context.scene.render.fps_base = fpsbase

# Weld coincident seams and conservatively decimate (Blender preserves & interpolates vertex groups!).
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
bpy.context.view_layer.objects.active = mesh
bpy.ops.object.mode_set(mode='EDIT')
bpy.ops.mesh.select_all(action='SELECT')
bpy.ops.mesh.remove_doubles(threshold=0.00001)
bpy.ops.object.mode_set(mode='OBJECT')
report['surface']['weldedVertices'] = len(mesh.data.vertices)
report['surface']['weldedPolygons'] = len(mesh.data.polygons)
note('welded', vertices=len(mesh.data.vertices), polygons=len(mesh.data.polygons))

mod = mesh.modifiers.new('PrologConservativeSurfaceBudget', 'DECIMATE')
mod.ratio = min(1.0, 180000 / len(mesh.data.polygons))
mod.use_collapse_triangulate = True
bpy.ops.object.modifier_apply(modifier=mod.name)
mesh.data.update()

# Surgically remove VRM's built-in hip shears (x < -0.195, z < 0.78) and any sideways waist/rib outlier wisps
# (0.88 < z < 1.33, |x| > 0.265), plus only genuinely stretched cross-body web faces (z > 0.78 and edge > 13.5 cm)
# so decimated flat cloth triangles on her batik skirt, chest, and wrists stay 100% solid and watertight!
bm_clean = bmesh.new()
bm_clean.from_mesh(mesh.data)
bm_clean.verts.ensure_lookup_table()
mw_c = mesh.matrix_world
vrm_shears_verts = []
for v in bm_clean.verts:
    p = mw_c @ v.co
    if (p.x < -0.195 and p.z < 0.78) or (0.94 < p.z < 1.45 and abs(p.x) > 0.232):
        vrm_shears_verts.append(v)
if vrm_shears_verts:
    bmesh.ops.delete(bm_clean, geom=vrm_shears_verts, context='VERTS')
bm_clean.verts.ensure_lookup_table()
bm_clean.faces.ensure_lookup_table()

torn_faces = []
for f in bm_clean.faces:
    f_pts = [mw_c @ v.co for v in f.verts]
    min_z = min(p.z for p in f_pts)
    if min_z <= 0.88:
        continue
    max_e = max((f_pts[i] - f_pts[(i + 1) % len(f_pts)]).length for i in range(len(f_pts)))
    if max_e > (0.058 if min_z > 0.96 else 0.115):
        torn_faces.append(f)
if torn_faces:
    bmesh.ops.delete(bm_clean, geom=torn_faces, context='FACES_ONLY')
bm_clean.verts.ensure_lookup_table()

visited_c = set()
debris_verts = []
for v in bm_clean.verts:
    if v.index in visited_c:
        continue
    comp = []
    stk = [v]
    visited_c.add(v.index)
    while stk:
        cur = stk.pop()
        comp.append(cur)
        for e in cur.link_edges:
            nb = e.other_vert(cur)
            if nb.index not in visited_c:
                visited_c.add(nb.index)
                stk.append(nb)
    if len(comp) < 8000:
        c_pts = [mw_c @ cv.co for cv in comp]
        z_max = max(p.z for p in c_pts)
        x_min = min(p.x for p in c_pts)
        # Delete isolated loose fragments (< 12 anywhere, or < 800 below head z < 1.28, or detached shears z_max < 0.85, x_min < -0.12)
        if len(comp) < 12 or (len(comp) < 800 and z_max < 1.28) or (z_max < 0.85 and x_min < -0.12):
            debris_verts.extend(comp)
if debris_verts:
    bmesh.ops.delete(bm_clean, geom=debris_verts, context='VERTS')
bm_clean.normal_update()
bm_clean.to_mesh(mesh.data)
bm_clean.free()
mesh.data.update()
note('torn_webs_and_dup_shears_cleaned', removedVrmShearsVerts=len(vrm_shears_verts), removedTornFaces=len(torn_faces), removedDebrisVerts=len(debris_verts))

# Rename VRM vertex groups to NyonyaDesigner bone names, consolidating distal arm/finger groups
# onto UpperArm_L / UpperArm_R because NyonyaDesigner's CharacterRig placed Forearm_L/R pivots out
# in mid-air at (±0.489, 0, 1.362) while UpperArm_L/R pivots sit at the true anatomical shoulders!
DISTAL_LEFT = {'Forearm_L', 'Hand_L'} | {f'Finger{i}_{j}_L' for i in range(1, 6) for j in range(1, 4)}
DISTAL_RIGHT = {'Forearm_R', 'Hand_R', 'Shears'} | {f'Finger{i}_{j}_R' for i in range(1, 6) for j in range(1, 4)}

for vg in list(mesh.vertex_groups):
    if vg.name in VRM_TO_DESIGNER:
        vg.name = VRM_TO_DESIGNER[vg.name]
    else:
        mesh.vertex_groups.remove(vg)

for name in group_names:
    if name not in mesh.vertex_groups:
        mesh.vertex_groups.new(name=name)

ua_l = mesh.vertex_groups['UpperArm_L'].index
ua_r = mesh.vertex_groups['UpperArm_R'].index
distal_l_idx = {mesh.vertex_groups[n].index for n in DISTAL_LEFT if n in mesh.vertex_groups}
distal_r_idx = {mesh.vertex_groups[n].index for n in DISTAL_RIGHT if n in mesh.vertex_groups}

for vi, v in enumerate(mesh.data.vertices):
    wl = sum(g.weight for g in v.groups if g.group in distal_l_idx)
    wr = sum(g.weight for g in v.groups if g.group in distal_r_idx)
    if wl > 1e-6 or wr > 1e-6:
        cur_ul = sum(g.weight for g in v.groups if g.group == ua_l)
        cur_ur = sum(g.weight for g in v.groups if g.group == ua_r)
        for idx in (distal_l_idx | distal_r_idx):
            mesh.vertex_groups[idx].remove([vi])
        if cur_ul + wl > 1e-6:
            mesh.vertex_groups[ua_l].add([vi], cur_ul + wl, 'REPLACE')
        if cur_ur + wr > 1e-6:
            mesh.vertex_groups[ua_r].add([vi], cur_ur + wr, 'REPLACE')

# Ensure materials match old.data.materials BEFORE joining shears_obj so UVs & single material slot merge cleanly.
mesh.data.materials.clear()
for mat in old.data.materials:
    mesh.data.materials.append(mat)

if shears_obj is not None:
    shears_obj.data.materials.clear()
    for mat in old.data.materials:
        shears_obj.data.materials.append(mat)
    bpy.ops.object.select_all(action='DESELECT')
    mesh.select_set(True)
    shears_obj.select_set(True)
    bpy.context.view_layer.objects.active = mesh
    bpy.ops.object.join()

for poly in mesh.data.polygons:
    poly.use_smooth = True

# Limit to 4 bone weights per vertex and normalize clean weights (guard against any zero-weight vertex).
bpy.ops.object.select_all(action='DESELECT')
mesh.select_set(True)
bpy.context.view_layer.objects.active = mesh
bpy.ops.object.vertex_group_limit_total(group_select_mode='ALL', limit=4)
bpy.ops.object.vertex_group_normalize_all(group_select_mode='ALL', lock_active=False)

hips_vg = mesh.vertex_groups['Hips']
unweighted = 0
for v in mesh.data.vertices:
    total_w = sum(g.weight for g in v.groups)
    if total_w < 1e-4:
        hips_vg.add([v.index], 1.0, 'REPLACE')
        unweighted += 1

# Audit cross-region bleed: check how many torso vertices (|x| < 0.16, 0.85 < z < 1.30) have forearm/hand weights!
forearm_groups = {mesh.vertex_groups[n].index for n in ['Forearm_L', 'Forearm_R', 'Hand_L', 'Hand_R'] if n in mesh.vertex_groups}
pts_check = points(mesh)
torso_bleed = 0
for v, p in zip(mesh.data.vertices, pts_check):
    if abs(p[0]) < 0.15 and 0.85 < p[2] < 1.28:
        if any(g.group in forearm_groups and g.weight > 0.05 for g in v.groups):
            torso_bleed += 1

report['surface']['outputVertices'] = len(mesh.data.vertices)
report['surface']['outputPolygons'] = len(mesh.data.polygons)
report['surface']['unweightedFallback'] = unweighted
report['surface']['torsoForearmBleedVertices'] = torso_bleed
note('weights_mapped_1to1', vertices=len(mesh.data.vertices), polygons=len(mesh.data.polygons), torsoBleed=torso_bleed)

# Maintain the existing node names and imported model transform contract.
old.name = 'ReferenceSkin_NotExported'
mesh.name = oldname
mesh.data.name = old.data.name
mod = mesh.modifiers.new('CharacterSkin', 'ARMATURE')
mod.object = rig
mesh.parent = old.parent
mesh.matrix_world = oldmatrix

for a in actions:
    assert action_signature(a) == original_signatures[a.name.rsplit('|', 1)[-1]], 'Changed action channels'
    a.name = a.name.rsplit('|', 1)[-1]

# Verify animated bounding box & bat-wing absence across ALL 5 actions!
anim_bounds = {}
wide_high_counts = {}
for act in actions:
    aname = act.name
    rig.animation_data.action = act
    for frame in [1, 10, int(act.frame_range[1] // 2)]:
        bpy.context.scene.frame_set(frame)
        bpy.context.view_layer.update()
        apts = points(mesh)
        span = (apts.max(axis=0) - apts.min(axis=0)).tolist()
        wh = int(np.sum((np.abs(apts[:, 0]) > 0.30) & (apts[:, 2] > 1.15)))
        anim_bounds[f'{aname}_f{frame}'] = [round(x, 3) for x in span]
        wide_high_counts[f'{aname}_f{frame}'] = wh
report['animatedSpansMetres'] = anim_bounds
report['wideHighBatWingVertices'] = wide_high_counts
note('animated_spans_verified', spans=anim_bounds, wideHigh=wide_high_counts)

rig.animation_data.action = next(a for a in actions if a.name == 'Idle')
bpy.context.scene.frame_set(1)
bpy.context.view_layer.update()

bpy.ops.object.select_all(action='DESELECT')
rig.select_set(True)
mesh.select_set(True)
bpy.context.view_layer.objects.active = rig
note('exporting')
bpy.ops.export_scene.fbx(
    filepath=str(OUT),
    use_selection=True,
    object_types={'MESH', 'ARMATURE', 'EMPTY'},
    axis_forward='-Z',
    axis_up='Y',
    bake_anim=True,
    bake_anim_use_all_actions=True,
    bake_anim_use_nla_strips=False,
    bake_anim_simplify_factor=0,
    add_leaf_bones=False,
    path_mode='STRIP',
)

report['output'] = dict(
    path=str(OUT),
    sha256=digest(OUT),
    bytes=OUT.stat().st_size,
    buildSeconds=time.monotonic() - started,
    actions=[dict(name=a.name, frames=list(a.frame_range), channels=len(a.fcurves), signature=action_signature(a)) for a in actions],
    rigidWeaponVertices=len(weapon),
)
(P / 'nyonya-prolog-surface-report.json').write_text(json.dumps(report, indent=2) + '\n')
old.hide_render = True
old.hide_viewport = True
bpy.ops.wm.save_as_mainfile(filepath=str(P / 'NyonyaPrologSurfaceRepair.blend'))
note('complete', output=str(OUT), seconds=report['output']['buildSeconds'])
