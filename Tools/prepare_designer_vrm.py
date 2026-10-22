"""Bake designer VRM 1.0 models into the game's existing legacy animation contract.

Run with Blender: blender -b --python Tools/prepare_designer_vrm.py -- --role Raka
Inputs default to Builds/ArtSources/DesignerVRM. Originals are read only. This is
an offline art step; the player needs neither a VRM importer nor network access.
"""
from pathlib import Path
import argparse
import ast
import hashlib
import json
import math
import sys

import bpy
import bmesh
import numpy as np
from mathutils import Matrix, Quaternion, Vector

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/DesignerCharacters'
MODEL = OUT / 'Models'
TEXTURE = OUT / 'Textures'
for folder in [MODEL, TEXTURE]:
    folder.mkdir(parents=True, exist_ok=True)

# Share the already shipped gait/IK authoring, without running its MakeHuman
# source-loading/module side effects or duplicating a second animation system.
pose_source = ROOT / 'Tools/author_prolog_characters.py'
tree = ast.parse(pose_source.read_text())
shared = [n for n in tree.body if isinstance(n, ast.FunctionDef) and n.name in
          {'hand_frame', 'set_pose', 'actions', 'export_phone_hand'}]
assert len(shared) == 4


class DesignerHeadroom(ast.NodeTransformer):
    def visit_Assign(self, node):
        # The designer's baggy replacement has a taller torso/hair envelope.
        # Bend its spine further instead of shrinking collision or scaling a squat.
        if any(isinstance(t, ast.Name) and t.id == 'lean' for t in node.targets):
            class Lean(ast.NodeTransformer):
                def visit_Constant(self, value):
                    if value.value == 58:
                        return ast.copy_location(ast.Constant(78), value)
                    if value.value == 35:
                        return ast.copy_location(ast.Constant(48), value)
                    return value
            node.value = Lean().visit(node.value)
        if any(isinstance(t, ast.Name) and t.id == 'hand' for t in node.targets) and any(
                isinstance(n, ast.Constant) and n.value == 1.34 for n in ast.walk(node.value)):
            # Keep the replacement's hair/face clear in the deep crouch, and
            # leave its wider cuff outside the lower edge of the phone case.
            vector = node.value.args[0]
            vector.elts[0] = ast.parse('-.24 if low else -.18', mode='eval').body
            vector.elts[1] = ast.parse("rig.pose.bones['Head'].head.y - .22 if low else -.36", mode='eval').body
            ast.fix_missing_locations(node)
        if any(isinstance(t, ast.Name) and t.id == 'hips_offset' for t in node.targets):
            node.value.args[0].elts[2] = ast.parse('-sink + bob + (.025 if low else 0)', mode='eval').body
            ast.fix_missing_locations(node)
        if any(isinstance(t, ast.Name) and t.id == 'elbow' for t in node.targets) and any(
                isinstance(n, ast.Constant) and n.value == 1.04 for n in ast.walk(node.value)):
            class Elbow(ast.NodeTransformer):
                def visit_Constant(self, value):
                    return ast.copy_location(ast.Constant(.38), value) if value.value == .29 else value
            node.value = Elbow().visit(node.value)
            node.value.args[0].elts[2] = ast.parse('.12 if low else .30 if crouch and not installing else 1.04 - sink * .55', mode='eval').body
            ast.fix_missing_locations(node)
        if any(isinstance(t, ast.Name) and t.id == 'centre' for t in node.targets):
            delta = ast.parse('pose.to_3x3() @ grip_offset(longitudinal, sideways, palm)', mode='eval').body
            node.value = ast.copy_location(ast.BinOp(left=node.value, op=ast.Add(), right=delta), node.value)
            ast.fix_missing_locations(node)
        return self.generic_visit(node)


shared = [DesignerHeadroom().visit(n) for n in shared]
NERA_PALMS = {}
PHONE_FINGER_FLEX = {2: [30, 0, 90], 3: [-10, 30, 80], 4: [-10, 20, 80], 5: [-10, 0, 10]}
exec(compile(ast.Module(body=shared, type_ignores=[]), str(pose_source), 'exec'), globals())
shipped_set_pose = set_pose
GRIP_DATA = {}


def grip_offset(longitudinal, sideways, palm):
    offset = GRIP_DATA.get('phoneOffset', [0, 0, 0])
    return longitudinal * offset[0] + sideways * offset[1] + palm * offset[2]


def set_pose(rig, defs, kind, phase, female):
    shipped_set_pose(rig, defs, kind, phase, female)
    if not female and ('Phone' in kind or kind == 'InstallCard') and GRIP_DATA:
        for name, quaternion in GRIP_DATA['fingerQuaternions'].items():
            rig.pose.bones[name].rotation_mode = 'QUATERNION'
            rig.pose.bones[name].rotation_quaternion = Quaternion(quaternion)
        bpy.context.view_layer.update()


def digest(path):
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()


def canonical(name):
    direct = {'hips': 'Hips', 'spine': 'Spine', 'chest': 'Chest',
              'upperChest': 'UpperChest', 'neck': 'Neck', 'head': 'Head', 'jaw': 'Jaw'}
    if name in direct:
        return direct[name]
    for side, suffix in [('left', '_L'), ('right', '_R')]:
        if not name.startswith(side):
            continue
        part = name[len(side):]
        mapped = {'Shoulder': 'Shoulder', 'UpperArm': 'UpperArm', 'LowerArm': 'Forearm',
                  'Hand': 'Hand', 'UpperLeg': 'Thigh', 'LowerLeg': 'Shin',
                  'Foot': 'Foot', 'Toes': 'Toes', 'Eye': 'Eye'}
        if part in mapped:
            return mapped[part] + suffix
        for index, finger in enumerate(['Thumb', 'Index', 'Middle', 'Ring', 'Little'], 1):
            if not part.startswith(finger):
                continue
            segment = part[len(finger):]
            order = ['Metacarpal', 'Proximal', 'Distal'] if finger == 'Thumb' else ['Proximal', 'Intermediate', 'Distal']
            return f'Finger{index}_{order.index(segment) + 1}' + suffix
    raise ValueError('Unknown VRM humanoid bone: ' + name)


def read_gltf(path):
    import struct
    with path.open('rb') as stream:
        assert struct.unpack('<III', stream.read(12))[:2] == (0x46546C67, 2)
        length, kind = struct.unpack('<II', stream.read(8))
        assert kind == 0x4E4F534A
        return json.loads(stream.read(length))


def material(name, color):
    mat = bpy.data.materials.get(name) or bpy.data.materials.new(name)
    mat.diffuse_color = (*color, 1)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = (*color, 1)
    bsdf.inputs['Roughness'].default_value = .7
    return mat


def save_image(image, path, limit=2048):
    if max(image.size) > limit:
        scale = limit / max(image.size)
        image.scale(round(image.size[0] * scale), round(image.size[1] * scale))
    image.filepath_raw = str(path)
    image.file_format = 'PNG'
    image.save()


def textures(role, gltf):
    """Extract exact glTF slots, preserving normal and metal/rough channel semantics."""
    source = gltf['materials'][0]
    slots = {'Albedo': source['pbrMetallicRoughness']['baseColorTexture']['index'],
             'Normal': source['normalTexture']['index'],
             'MetalRough': source['pbrMetallicRoughness']['metallicRoughnessTexture']['index']}
    result = {}
    # Blender's import images retain the glTF image index in their creation order.
    images = [image for image in bpy.data.images if image.type == 'IMAGE']
    # Resolve by the imported material socket rather than assuming image ordering.
    nodes = next(m for m in bpy.data.materials if m.use_nodes).node_tree.nodes
    bsdf = next(n for n in nodes if n.type == 'BSDF_PRINCIPLED')
    def image_upstream(socket):
        pending = [link.from_node for link in socket.links]
        seen = set()
        while pending:
            node = pending.pop()
            if node.name in seen:
                continue
            seen.add(node.name)
            if node.type == 'TEX_IMAGE':
                return node.image
            pending.extend(link.from_node for input in node.inputs for link in input.links)
        return None
    resolved = {'Albedo': image_upstream(bsdf.inputs['Base Color']),
                'Normal': image_upstream(bsdf.inputs['Normal']),
                'MetalRough': image_upstream(bsdf.inputs['Roughness'])}
    for slot in slots:
        image = resolved[slot]
        assert image, (role, slot, [i.name for i in images])
        path = TEXTURE / f'{role}{slot}.png'
        save_image(image, path, 2048 if slot == 'Albedo' else 1024)
        result[slot] = str(path.relative_to(ROOT))
    packed = resolved['MetalRough']
    pixels = np.array(packed.pixels[:], dtype=np.float32).reshape((-1, 4))
    mask = np.zeros_like(pixels)
    mask[:, 0] = pixels[:, 2] * source['pbrMetallicRoughness'].get('metallicFactor', 1)
    mask[:, 3] = 1 - pixels[:, 1] * source['pbrMetallicRoughness'].get('roughnessFactor', 1)
    image = bpy.data.images.new(role + 'MetalSmooth', width=packed.size[0], height=packed.size[1], alpha=True)
    image.colorspace_settings.name = 'Non-Color'
    image.pixels.foreach_set(mask.reshape(-1))
    path = TEXTURE / f'{role}MetalSmooth.png'
    save_image(image, path, 1024)
    result['MetalSmooth'] = str(path.relative_to(ROOT))
    # The intermediary packed glTF texture is not needed by Unity/the build.
    (TEXTURE / f'{role}MetalRough.png').unlink()
    del result['MetalRough']
    return result


def bind(obj, rig):
    obj.parent = rig
    obj.matrix_parent_inverse = Matrix.Identity(4)
    if not any(mod.type == 'ARMATURE' for mod in obj.modifiers):
        obj.modifiers.new('CharacterSkin', 'ARMATURE').object = rig


def rigid_mesh(name, points, faces, mat, bone, rig):
    mesh = bpy.data.meshes.new(name)
    mesh.from_pydata(points, [], faces)
    mesh.materials.append(mat)
    mesh.update()
    mesh.uv_layers.new(name='UVMap')
    obj = bpy.data.objects.new(name, mesh)
    bpy.context.collection.objects.link(obj)
    obj.vertex_groups.new(name=bone).add(list(range(len(points))), 1, 'REPLACE')
    bind(obj, rig)
    return obj


def skeleton(rig, mesh, role, height):
    bpy.context.view_layer.update()
    points = [mesh.matrix_world @ vertex.co for vertex in mesh.data.vertices]
    low = min(p.z for p in points)
    scale = height / (max(p.z for p in points) - low)
    feet = [(rig.matrix_world @ rig.data.bones[name].head_local).x for name in ['leftFoot', 'rightFoot']]
    change = Matrix.Scale(scale, 4) @ Matrix.Translation(Vector((-sum(feet) / 2, 0, -low)))
    mesh_matrix = change @ mesh.matrix_world
    for vertex in mesh.data.vertices:
        vertex.co = mesh_matrix @ vertex.co
    mesh.parent = None
    mesh.matrix_world = Matrix.Identity(4)
    old_names = {b.name: canonical(b.name) for b in rig.data.bones}
    for group in mesh.vertex_groups:
        if group.name in old_names:
            group.name = old_names[group.name]
    bpy.context.view_layer.objects.active = rig
    bpy.ops.object.mode_set(mode='EDIT')
    original_world = rig.matrix_world.copy()
    for bone in rig.data.edit_bones:
        bone.head = change @ original_world @ bone.head
        bone.tail = change @ original_world @ bone.tail
        bone.name = old_names[bone.name]
    rig.matrix_world = Matrix.Identity(4)
    bones = rig.data.edit_bones
    next_bone = {'Hips': 'Spine', 'Spine': 'Chest', 'Chest': 'UpperChest',
                 'UpperChest': 'Neck', 'Neck': 'Head'}
    for side in ['L', 'R']:
        for a, b in [('Shoulder', 'UpperArm'), ('UpperArm', 'Forearm'), ('Forearm', 'Hand'),
                     ('Hand', 'Finger3_1'), ('Thigh', 'Shin'), ('Shin', 'Foot'), ('Foot', 'Toes')]:
            next_bone[a + '_' + side] = b + '_' + side
        for finger in range(1, 6):
            for index in [1, 2]:
                next_bone[f'Finger{finger}_{index}_{side}'] = f'Finger{finger}_{index + 1}_{side}'
    for name, child in next_bone.items():
        bones[name].tail = bones[child].head
    # glTF leaf tails are importer guesses, not measured finger/eye lengths.
    for side in ['L', 'R']:
        for finger in range(1, 6):
            previous = bones[f'Finger{finger}_2_{side}']
            last = bones[f'Finger{finger}_3_{side}']
            last.tail = last.head + (previous.tail - previous.head) * .75
    bones['Head'].tail = bones['Head'].head + Vector((0, 0, .15))
    bones['Jaw'].tail = bones['Jaw'].head + Vector((0, -.025, -.02))
    for bone in bones:
        if (bone.tail - bone.head).length < .003:
            bone.tail = bone.head + Vector((0, 0, .01))
    bpy.ops.object.mode_set(mode='OBJECT')
    rig.name = 'CharacterRig'
    bind(mesh, rig)
    bpy.context.view_layer.update()
    return {b.name: (b.head_local.copy(), b.tail_local.copy(), b.parent.name if b.parent else None) for b in rig.data.bones}


def simplify(mesh, target):
    bpy.context.view_layer.objects.active = mesh
    for obj in bpy.context.selected_objects:
        obj.select_set(False)
    mesh.select_set(True)
    # Collapse high-resolution duplicate geometry but retain UV boundaries and
    # original skin weights. Decimation precedes animation, never posed baking.
    original = len(mesh.data.polygons)
    modifier = mesh.modifiers.new('GameTopologyBudget', 'DECIMATE')
    modifier.ratio = min(1, target / original)
    modifier.use_collapse_triangulate = True
    bpy.ops.object.modifier_move_up(modifier=modifier.name)
    bpy.ops.object.modifier_apply(modifier=modifier.name)
    mesh.data.update()
    assert len(mesh.data.polygons) <= target + 100
    for poly in mesh.data.polygons:
        poly.use_smooth = True
    return original


def add_phone(rig, defs):
    longitudinal, palm, sideways = hand_frame(defs, 'R')
    origin = defs['Hand_R'][0]
    offset = GRIP_DATA.get('phoneOffset', [0, 0, 0])
    def point(x, y, z):
        return origin + longitudinal * (.09 - x + offset[0]) + sideways * (y + offset[1]) + palm * (.045 + z + offset[2])
    points = [point(x, y, z) for y in [-.0825, .0825] for z in [-.007, .007] for x in [-.039, .039]]
    phone = rigid_mesh('HandheldPhone', points, [(0, 1, 3, 2), (4, 6, 7, 5), (0, 4, 5, 1),
                       (2, 3, 7, 6), (0, 2, 6, 4), (1, 5, 7, 3)], material('DesignerPhone', (.018, .02, .023)), 'Hand_R', rig)
    screen = rigid_mesh('HandsetScreen', [point(x, y, .008) for x, y in [(-.034, -.075), (.034, -.075),
                        (.034, .075), (-.034, .075)]], [(0, 1, 2, 3)], material('DesignerPhoneScreen', (.07, .13, .15)), 'Hand_R', rig)
    screen.parent = phone
    return phone


def repair_raka_thumb(rig, mesh, defs):
    """The supplied VRM has zero vertices weighted to its right thumb bones.

    Fit those three joints to the actual sculpted thumb and skin that digit;
    retaining the automatic T-rig would leave an immobile thumb beside the phone.
    """
    origin = defs['Hand_R'][0]
    along = (defs['Hand_R'][1] - origin).normalized()
    across = defs['Finger2_1_R'][0] - defs['Finger5_1_R'][0]
    across = (across - along * across.dot(along)).normalized()
    normal = along.cross(across).normalized()
    def point(u, v, w):
        return origin + along * u + across * v + normal * w
    landmarks = [point(.012, .025, -.015), point(.028, .049, -.020),
                 point(.050, .057, -.018), point(.065, .056, -.016)]
    bpy.context.view_layer.objects.active = rig
    bpy.ops.object.mode_set(mode='EDIT')
    for i in range(3):
        bone = rig.data.edit_bones[f'Finger1_{i + 1}_R']
        bone.head, bone.tail = landmarks[i], landmarks[i + 1]
    bpy.ops.object.mode_set(mode='OBJECT')
    groups = [mesh.vertex_groups.get(f'Finger1_{i}_R') or mesh.vertex_groups.new(name=f'Finger1_{i}_R') for i in [1, 2, 3]]
    count = 0
    for vertex in mesh.data.vertices:
        delta = vertex.co - origin
        u, v, w = delta.dot(along), delta.dot(across), delta.dot(normal)
        if not (.005 < u < .079 and .032 < v < .078 and -.050 < w < .012):
            continue
        # Smoothly blend through knuckles rather than assigning a detached rigid digit.
        blend = max(0, min(1, (v - .032) / .014))
        if blend <= 0:
            continue
        existing = [(g.group, g.weight) for g in vertex.groups]
        for group in mesh.vertex_groups:
            group.remove([vertex.index])
        for index, weight in existing:
            if weight * (1 - blend) > .0001:
                mesh.vertex_groups[index].add([vertex.index], weight * (1 - blend), 'REPLACE')
        t = max(0, min(2, (u - .020) / .022))
        lower = min(1, int(t))
        fraction = min(1, t - lower)
        groups[lower].add([vertex.index], blend * (1 - fraction), 'ADD')
        groups[lower + 1].add([vertex.index], blend * fraction, 'ADD')
        count += 1
    assert count >= 15, ('No actual thumb surface found', count)
    print('VRM_THUMB_RESKINNED', count, flush=True)
    return {b.name: (b.head_local.copy(), b.tail_local.copy(), b.parent.name if b.parent else None) for b in rig.data.bones}


def export_fbx(path, objects, animate):
    bpy.ops.object.select_all(action='DESELECT')
    for obj in objects:
        obj.select_set(True)
    bpy.context.view_layer.objects.active = objects[0]
    bpy.ops.export_scene.fbx(filepath=str(path), use_selection=True, object_types={'MESH', 'ARMATURE', 'EMPTY'},
                            axis_forward='-Z', axis_up='Y', bake_anim=animate, bake_anim_use_all_actions=True,
                            bake_anim_use_nla_strips=False, bake_anim_simplify_factor=0, add_leaf_bones=False,
                            path_mode='STRIP')


def fit_nyonya_rest(rig, mesh):
    """Fit the automatic VRM's T-arm bones to the supplied bent-arm artwork.

    These normalized source-space landmarks are art adaptation, not AI/collision
    changes. The source already contains its head tilt, shears and long dress.
    """
    scale = 1.64 / 1.9262359142303467
    centre = .12564425
    def at(x, y, z):
        return Vector(((x - centre) * scale, y * scale, z * scale))
    wrist = {'R': at(-.25, -.035, .80), 'L': at(.43, -.035, .80)}
    bpy.context.view_layer.objects.active = rig
    bpy.ops.object.mode_set(mode='EDIT')
    bones = rig.data.edit_bones
    for side, elbow in [('R', at(-.50, 0, 1.70)), ('L', at(.70, 0, 1.60))]:
        hand = bones['Hand_' + side]
        old_wrist = hand.head.copy()
        rotation = (hand.tail - hand.head).normalized().rotation_difference(Vector((0, -.05, -1)).normalized())
        for bone in bones:
            if bone.name.startswith('Finger') and bone.name.endswith('_' + side):
                bone.head = wrist[side] + rotation @ (bone.head - old_wrist)
                bone.tail = wrist[side] + rotation @ (bone.tail - old_wrist)
        bones['Forearm_' + side].head = elbow
        bones['Forearm_' + side].tail = wrist[side]
        bones['UpperArm_' + side].tail = elbow
        hand.head = wrist[side]
        hand.tail = bones['Finger3_1_' + side].head
    bpy.ops.object.mode_set(mode='OBJECT')
    segments = {b.name: (b.head_local.copy(), b.tail_local.copy()) for b in rig.data.bones
                if b.name in ['Hips', 'Spine', 'Chest', 'UpperChest', 'Neck', 'Head',
                              'UpperArm_L', 'Forearm_L', 'Hand_L', 'UpperArm_R', 'Forearm_R', 'Hand_R']}
    groups = {name: mesh.vertex_groups.get(name) or mesh.vertex_groups.new(name=name) for name in segments}
    def distance(point, a, b):
        delta = b - a
        return (point - (a + delta * max(0, min(1, (point - a).dot(delta) / max(.000001, delta.length_squared))))).length
    for vertex in mesh.data.vertices:
        point = vertex.co
        if point.z < .59 or abs(point.x) < .16:
            continue
        if point.x < -.15 and point.z < .80:
            weighted = [('Hand_R', 1)]  # The sculpted shears remain in her grasp.
        else:
            nearest = sorted((distance(point, a, b), name) for name, (a, b) in segments.items())[:3]
            values = [(name, 1 / (.035 + d) ** 6) for d, name in nearest]
            total = sum(w for _, w in values)
            weighted = [(name, w / total) for name, w in values]
        for group in mesh.vertex_groups:
            group.remove([vertex.index])
        for name, weight in weighted:
            groups[name].add([vertex.index], weight, 'REPLACE')
    return {b.name: (b.head_local.copy(), b.tail_local.copy(), b.parent.name if b.parent else None) for b in rig.data.bones}


def nyonya_pose(rig, kind, phase):
    for bone in rig.pose.bones:
        bone.matrix_basis = Matrix.Identity(4)
    def turn(name, axis, degrees):
        bone = rig.pose.bones[name]
        bone.rotation_mode = 'QUATERNION'
        bone.rotation_quaternion = Quaternion(bone.bone.matrix_local.to_3x3().inverted() @ Vector(axis), math.radians(degrees))
    gait = phase * math.tau
    moving = kind in ['Walk', 'Run']
    strength = 1.6 if kind == 'Run' else 1
    if moving:
        for side, sign in [('L', 1), ('R', -1)]:
            turn('Thigh_' + side, (1, 0, 0), math.sin(gait) * 10 * sign * strength)
            turn('Shin_' + side, (1, 0, 0), max(0, -math.sin(gait) * sign) * -12 * strength)
            turn('UpperArm_' + side, (1, 0, 0), math.sin(gait) * 3 * sign)
    turn('Head', (0, 1, 0), math.sin(gait) * 2)
    turn('Spine', (1, 0, 0), math.sin(gait * 2) * .8)
    if kind == 'Windup':
        progress = min(1, phase * 1.8)
        turn('UpperArm_R', (1, 0, 0), -5 * progress)
        turn('Forearm_R', (1, 0, 0), -8 * progress)
        turn('Hand_R', (1, 0, 0), -35 * progress)
        turn('Head', (0, 1, 0), 8 * progress)
    elif kind == 'Stun':
        recoil = math.sin(min(1, phase * 1.8) * math.pi * .5)
        turn('Spine', (1, 0, 0), -14 * recoil)
        turn('Head', (0, 1, 0), -16 * recoil)
        turn('UpperArm_R', (1, 0, 0), 20 * recoil)
    elif kind == 'Seated':
        hips = rig.pose.bones['Hips']
        hips.location = hips.bone.matrix_local.to_3x3().inverted() @ Vector((0, 0, -.43))
        for side in ['L', 'R']:
            turn('Thigh_' + side, (1, 0, 0), -80)
            turn('Shin_' + side, (1, 0, 0), 80)
    bpy.context.view_layer.update()


def bind_nyonya_shears(rig, mesh):
    """Give the sculpted weapon the existing live shears-bone presentation hook."""
    bpy.context.view_layer.objects.active = rig
    bpy.ops.object.mode_set(mode='EDIT')
    hand = rig.data.edit_bones['Hand_R']
    blade = rig.data.edit_bones.new('Shears')
    blade.head = hand.head
    blade.tail = hand.head + Vector((-.06, 0, -.18))
    blade.parent = hand
    bpy.ops.object.mode_set(mode='OBJECT')
    group = mesh.vertex_groups.new(name='Shears')
    hand_group = mesh.vertex_groups['Hand_R'].index
    origin = rig.data.bones['Hand_R'].head_local
    count = 0
    for vertex in mesh.data.vertices:
        held = sum(g.weight for g in vertex.groups if g.group == hand_group)
        if held < .5 or vertex.co.z > origin.z - .05 or vertex.co.x > origin.x + .08:
            continue
        for old in mesh.vertex_groups:
            old.remove([vertex.index])
        group.add([vertex.index], 1, 'REPLACE')
        count += 1
    assert count > 40, ('No sculpted shears geometry bound', count)
    print('VRM_SHEARS_BOUND', count, flush=True)


def nyonya_actions(rig):
    specs = [('Idle', 2.8), ('Walk', 1.2), ('Run', .65), ('Windup', .8), ('Stun', 1.2)]
    for name, duration in specs:
        action = bpy.data.actions.new(name)
        action.use_fake_user = True
        rig.animation_data_create()
        rig.animation_data.action = action
        count = round(duration * 24)
        for frame in range(count + 1):
            bpy.context.scene.frame_set(frame + 1)
            nyonya_pose(rig, name, frame / count)
            for bone in rig.pose.bones:
                bone.keyframe_insert('location', frame=frame + 1, group=bone.name)
                bone.keyframe_insert('rotation_quaternion' if bone.rotation_mode == 'QUATERNION' else 'rotation_euler', frame=frame + 1, group=bone.name)
        for curve in action.fcurves:
            for key in curve.keyframe_points:
                key.interpolation = 'LINEAR'
    rig.animation_data.action = bpy.data.actions['Idle']
    bpy.context.scene.frame_set(1)
    return [n for n, _ in specs]


def apparition(rig, mesh, kind):
    rig.animation_data.action = None
    nyonya_pose(rig, 'Seated' if kind == 'Seated' else 'Idle', 0)
    deps = bpy.context.evaluated_depsgraph_get()
    evaluated = mesh.evaluated_get(deps)
    copy = bpy.data.objects.new('NyonyaDesigner' + kind, bpy.data.meshes.new_from_object(evaluated))
    bpy.context.collection.objects.link(copy)
    simplify(copy, 5000)
    export_fbx(MODEL / ('NyonyaDesigner' + kind + '.fbx'), [copy], False)
    bpy.data.objects.remove(copy, do_unlink=True)


def build(role, source_dir):
    global GRIP_DATA
    bpy.ops.wm.read_factory_settings(use_empty=True)
    path = source_dir / (role + '.vrm')
    gltf = read_gltf(path)
    assert 'VRMC_vrm' in gltf.get('extensionsUsed', [])
    bpy.ops.import_scene.gltf(filepath=str(path))
    print('VRM_STAGE imported', role, flush=True)
    rigs = [o for o in bpy.context.scene.objects if o.type == 'ARMATURE']
    assert len(rigs) == 1
    rig = rigs[0]
    meshes = [o for o in bpy.context.scene.objects if o.type == 'MESH' and any(m.type == 'ARMATURE' for m in o.modifiers)]
    assert len(meshes) == 1
    mesh = meshes[0]
    # Remove the Blender glTF importer's helper shapes; they are bone widgets.
    for obj in list(bpy.context.scene.objects):
        if obj not in [rig, mesh]:
            bpy.data.objects.remove(obj, do_unlink=True)
    texture_record = textures(role, gltf)
    print('VRM_STAGE textures', role, flush=True)
    mesh.name = 'VisibleSkin' if role == 'Raka' else role + 'DesignerBody'
    mesh.data.materials[0].name = role + 'DesignerSurface'
    height = {'Raka': 1.74, 'Damar': 1.76, 'Nyonya': 1.64}[role]
    defs = skeleton(rig, mesh, role, height)
    print('VRM_STAGE skeleton', role, flush=True)
    original_triangles = simplify(mesh, 32000 if role == 'Raka' else 22000)
    print('VRM_STAGE simplified', role, len(mesh.data.polygons), flush=True)
    if role == 'Nyonya':
        defs = fit_nyonya_rest(rig, mesh)
    if role == 'Raka':
        defs = repair_raka_thumb(rig, mesh, defs)
        grip_path = ROOT / 'Audit/designer-raka-grip.json'
        GRIP_DATA = json.loads(grip_path.read_text()) if grip_path.exists() else {}
        if GRIP_DATA.get('sourceSha256') != digest(path) or GRIP_DATA.get('rigRevision') != 3:
            from fit_designer_phone_grip import solve
            mesh.hide_viewport = True
            GRIP_DATA = solve(rig, mesh, defs, shipped_set_pose, hand_frame)
            GRIP_DATA['sourceSha256'] = digest(path)
            GRIP_DATA['rigRevision'] = 3
            grip_path.write_text(json.dumps(GRIP_DATA, indent=2) + '\n')
            mesh.hide_viewport = False
        assert GRIP_DATA['maxPenetrationMetres'] < .0012, 'Handset still penetrates the fitted hand'
        assert max(GRIP_DATA['fingerContactGapsMetres']) < .0035, 'A finger does not brace the handset'
        add_phone(rig, defs)
    if role == 'Nyonya':
        bind_nyonya_shears(rig, mesh)
    # IK sampling needs the bones, not thousands of repeated mesh deformations.
    # Keep every source surface out of the dependency graph during key authoring.
    for obj in bpy.context.scene.objects:
        if obj.type == 'MESH': obj.hide_viewport = True
    clips = nyonya_actions(rig) if role == 'Nyonya' else actions(rig, defs, False)
    for obj in bpy.context.scene.objects:
        if obj.type == 'MESH': obj.hide_viewport = False
    print('VRM_STAGE animated', role, flush=True)
    objects = [o for o in bpy.context.scene.objects if o.type in {'ARMATURE', 'MESH', 'EMPTY'}]
    export_fbx(MODEL / f'{role}Designer.fbx', objects, True)
    bpy.ops.wm.save_as_mainfile(filepath=str(ROOT / 'Builds/ArtSources/DesignerVRM' / (role + '-game.blend')))
    if role == 'Raka':
        export_phone_hand(rig, defs, [o for o in objects if o.type == 'MESH'])
        (MODEL / 'RakaPhoneHand.fbx').rename(MODEL / 'RakaDesignerPhoneHand.fbx')
    elif role == 'Nyonya':
        apparition(rig, mesh, 'Echo')
        apparition(rig, mesh, 'Seated')
    print('VRM_STAGE exported', role, flush=True)
    record = {'role': role, 'sourceFile': path.name, 'sourceSha256': digest(path),
              'sourceBytes': path.stat().st_size, 'originalTriangles': original_triangles,
              'gameTriangles': sum(len(p.vertices) - 2 for p in mesh.data.polygons),
              'gameVertices': len(mesh.data.vertices), 'bones': len(rig.data.bones),
              'heightMetres': height, 'textures': texture_record, 'clips': clips,
              'sharedPoseSourceSha256': digest(pose_source)}
    report = ROOT / 'Audit/designer-vrm-authoring.json'
    records = json.loads(report.read_text()) if report.exists() else {'characters': {}}
    records['characters'][role] = record
    report.write_text(json.dumps(records, indent=2) + '\n')
    bpy.ops.wm.save_as_mainfile(filepath=str(ROOT / 'Builds/ArtSources/DesignerVRM' / (role + '-game.blend')))
    print('DESIGNER_VRM_BAKED', json.dumps(record), flush=True)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--source-dir', type=Path, default=ROOT / 'Builds/ArtSources/DesignerVRM')
    parser.add_argument('--role', choices=['Raka', 'Damar', 'Nyonya'], required=True)
    args = parser.parse_args(sys.argv[sys.argv.index('--') + 1:] if '--' in sys.argv else [])
    build(args.role, args.source_dir)
