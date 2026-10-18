"""Build Rumah Atmaja - two-story Javanese-Dutch colonial house exterior for
LayarTerakhir Prolog Adegan 2.

Method: KITBASH + PROCEDURAL (see ArtSources/Scene2_Atmaja_Recovery/asset_manifest.json)
  * Real downloaded components: TowerOverhangModularKit (CC-BY 4.0, Trey Ramm /
    minime453) columns, louvered shutters, trims, supports.
  * CC0 Poly Haven PBR materials reused from the approved Scene 2 collection.
  * Walls, openings, roof structure, veranda and annex authored procedurally.

Run headless:
    blender -b -P ArtSources/Scene2_Atmaja_Recovery/01_House/Blender/build_house.py

Outputs:
    01_House/Blender/RumahAtmaja_House.blend
    01_House/Blender/house_build_stats.json
    01_House/Export/RumahAtmaja_House.fbx / .glb
    01_House/Export/Modules/*.fbx
No Unity files are touched.
"""
from pathlib import Path
import bmesh
import json
import math

import bpy
from mathutils import Matrix, Vector

PKG = Path(__file__).resolve().parents[2]
HOUSE = PKG / '01_House'
TEX = HOUSE / 'Textures'
KIT = HOUSE / 'Sources' / 'TowerOverhangKit'
BLEND = HOUSE / 'Blender'
EXPORT = HOUSE / 'Export'
MODULES = EXPORT / 'Modules'

TILE = json.loads((HOUSE / 'Sources' / 'texture_tile_sizes.json').read_text())

COLLECTIONS = {}


def reset():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    for collection in list(bpy.data.collections):
        bpy.data.collections.remove(collection)


def new_collection(name):
    collection = bpy.data.collections.new(name)
    bpy.context.scene.collection.children.link(collection)
    COLLECTIONS[name] = collection
    return collection


# --------------------------------------------------------------- materials
def resolve_texture(name):
    local = TEX / name
    if local.is_file():
        return local
    source = KIT / 'Textures' / name
    if source.is_file():
        return source
    return local


def make_material(name, diffuse=None, normal=None, arm=None, base_color=(0.5, 0.5, 0.5, 1),
                  roughness=0.8, metallic=0.0):
    material = bpy.data.materials.new(name)
    material.use_nodes = True
    tree = material.node_tree
    tree.nodes.clear()
    output = tree.nodes.new('ShaderNodeOutputMaterial')
    bsdf = tree.nodes.new('ShaderNodeBsdfPrincipled')
    bsdf.inputs['Base Color'].default_value = base_color
    bsdf.inputs['Roughness'].default_value = roughness
    bsdf.inputs['Metallic'].default_value = metallic
    tree.links.new(bsdf.outputs['BSDF'], output.inputs['Surface'])
    if diffuse:
        texture = tree.nodes.new('ShaderNodeTexImage')
        texture.image = bpy.data.images.load(str(resolve_texture(diffuse)))
        tree.links.new(texture.outputs['Color'], bsdf.inputs['Base Color'])
    if normal:
        texture = tree.nodes.new('ShaderNodeTexImage')
        texture.image = bpy.data.images.load(str(resolve_texture(normal)))
        texture.image.colorspace_settings.name = 'Non-Color'
        nmap = tree.nodes.new('ShaderNodeNormalMap')
        tree.links.new(texture.outputs['Color'], nmap.inputs['Color'])
        tree.links.new(nmap.outputs['Normal'], bsdf.inputs['Normal'])
    if arm:
        texture = tree.nodes.new('ShaderNodeTexImage')
        texture.image = bpy.data.images.load(str(resolve_texture(arm)))
        texture.image.colorspace_settings.name = 'Non-Color'
        separate = tree.nodes.new('ShaderNodeSeparateColor')
        tree.links.new(texture.outputs['Color'], separate.inputs['Color'])
        tree.links.new(separate.outputs['Green'], bsdf.inputs['Roughness'])
        tree.links.new(separate.outputs['Blue'], bsdf.inputs['Metallic'])
    return material


def build_materials():
    materials = {}
    materials['plaster_low'] = make_material(
        'MAT_PlasterLower', 'worn_mossy_plasterwall_diff_1k.jpg',
        'worn_mossy_plasterwall_nor_gl_1k.jpg', 'worn_mossy_plasterwall_arm_1k.jpg')
    materials['plaster_up'] = make_material(
        'MAT_PlasterUpper', 'grey_plaster_03_diff_1k.jpg',
        'grey_plaster_03_nor_gl_1k.jpg', 'grey_plaster_03_arm_1k.jpg')
    materials['plaster_damaged'] = make_material(
        'MAT_PlasterDamaged', 'damaged_plaster_diff_1k.jpg',
        'damaged_plaster_nor_gl_1k.jpg', 'damaged_plaster_arm_1k.jpg')
    materials['brick'] = make_material(
        'MAT_BrickBase', 'brick_moss_001_diff_1k.jpg',
        'brick_moss_001_nor_gl_1k.jpg', 'brick_moss_001_arm_1k.jpg')
    materials['concrete'] = make_material(
        'MAT_Concrete', 'concrete_moss_diff_1k.jpg',
        'concrete_moss_nor_gl_1k.jpg', 'concrete_moss_arm_1k.jpg')
    materials['roof'] = make_material(
        'MAT_RoofTiles', 'clay_roof_tiles_02_diff_1k_weathered.jpg',
        'clay_roof_tiles_02_nor_gl_1k.jpg', 'clay_roof_tiles_02_arm_1k.jpg')
    materials['roof_annex'] = make_material(
        'MAT_RoofTilesAnnex', 'clay_roof_tiles_03_diff_1k.jpg',
        'clay_roof_tiles_03_nor_gl_1k.jpg', 'clay_roof_tiles_03_arm_1k.jpg')
    materials['wood'] = make_material(
        'MAT_WoodDoor', 'wood_planks_grey_diff_1k.jpg',
        'wood_planks_grey_nor_gl_1k.jpg', 'wood_planks_grey_arm_1k.jpg')
    materials['wood_white'] = make_material(
        'MAT_WoodFrameWhite', 'wood_peeling_paint_weathered_diff_1k.jpg',
        'wood_peeling_paint_weathered_nor_gl_1k.jpg',
        'wood_peeling_paint_weathered_arm_1k.jpg')
    materials['trim_teak'] = make_material(
        'MAT_TrimTeak', 'WoodTrimSheet_BaseColor_Teak.png',
        'WoodTrimSheet_Normal.png', 'WoodTrimSheet_Roughness.png')
    materials['rust'] = make_material(
        'MAT_RustMetal', 'rusty_metal_sheet_diff_1k.jpg',
        'rusty_metal_sheet_nor_gl_1k.jpg', 'rusty_metal_sheet_arm_1k.jpg')
    materials['glass'] = make_material('MAT_GlassDark', base_color=(0.045, 0.05, 0.06, 1),
                                       roughness=0.28, metallic=0.0)
    return materials


# --------------------------------------------------------------- primitives
def apply_box_uv(obj, tile, forced_axis=None):
    mesh = obj.data
    if not mesh.uv_layers:
        mesh.uv_layers.new()
    uv_layer = mesh.uv_layers.active.data
    matrix = obj.matrix_world
    for polygon in mesh.polygons:
        normal = (matrix.to_3x3() @ polygon.normal).normalized()
        axis = forced_axis if forced_axis is not None else \
            max(range(3), key=lambda index: abs(normal[index]))
        others = [index for index in range(3) if index != axis]
        for loop_index in polygon.loop_indices:
            vertex = mesh.vertices[mesh.loops[loop_index].vertex_index].co
            world = matrix @ vertex
            uv_layer[loop_index].uv = (world[others[0]] / tile, world[others[1]] / tile)


def make_mesh_object(name, verts, faces, material, collection, tile=None, forced_axis=None):
    mesh = bpy.data.meshes.new(name)
    mesh.from_pydata([tuple(v) for v in verts], [], faces)
    mesh.validate()
    mesh.update()
    bm = bmesh.new()
    bm.from_mesh(mesh)
    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    bm.to_mesh(mesh)
    bm.free()
    obj = bpy.data.objects.new(name, mesh)
    collection.objects.link(obj)
    if material:
        obj.data.materials.append(material)
    if tile:
        apply_box_uv(obj, tile, forced_axis=forced_axis)
    return obj


def add_box(name, min_corner, max_corner, material, collection, tile, forced_axis=None):
    x0, y0, z0 = min_corner
    x1, y1, z1 = max_corner
    verts = [(x0, y0, z0), (x1, y0, z0), (x1, y1, z0), (x0, y1, z0),
             (x0, y0, z1), (x1, y0, z1), (x1, y1, z1), (x0, y1, z1)]
    faces = [(0, 1, 2, 3), (4, 7, 6, 5), (0, 4, 5, 1), (1, 5, 6, 2),
             (2, 6, 7, 3), (3, 7, 4, 0)]
    return make_mesh_object(name, verts, faces, material, collection, tile,
                            forced_axis=forced_axis)


def add_slab(name, points, thickness, material, collection, tile, uv_axis=None):
    points = [Vector(p) for p in points]
    normal = (points[1] - points[0]).cross(points[2] - points[0]).normalized()
    lower = [point - normal * thickness for point in points]
    count = len(points)
    verts = [tuple(point) for point in points] + [tuple(point) for point in lower]
    faces = [tuple(range(count)), tuple(range(2 * count - 1, count - 1, -1))]
    for index in range(count):
        nxt = (index + 1) % count
        faces.append((index, nxt, count + nxt, count + index))
    return make_mesh_object(name, verts, faces, material, collection, tile,
                            forced_axis=uv_axis)


# ----------------------------------------------------------------- kit
def import_kit_piece(name, collection):
    before = set(bpy.data.objects)
    bpy.ops.import_scene.fbx(filepath=str(KIT / f'TowerOverhang_{name}.fbx'), use_anim=False)
    new_objects = [obj for obj in bpy.data.objects
                   if obj not in before and obj.type == 'MESH']
    for obj in new_objects:
        obj.name = f'KIT_{name}_{obj.name}'
        for existing in list(obj.users_collection):
            existing.objects.unlink(obj)
        collection.objects.link(obj)
    bpy.context.view_layer.update()
    return new_objects


def piece_bounds(objects):
    low = Vector((math.inf,) * 3)
    high = Vector((-math.inf,) * 3)
    for obj in objects:
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            low = Vector(map(min, low, point))
            high = Vector(map(max, high, point))
    return low, high


def place_piece(objects, target_low_center=(0, 0, 0), rotation_z=0.0,
                scale=(1.0, 1.0, 1.0)):
    """Move kit piece so its low-center lands on target; scale then rotate in local."""
    low, high = piece_bounds(objects)
    anchor = Vector(((low.x + high.x) / 2, (low.y + high.y) / 2, low.z))
    transform = (Matrix.Translation(Vector(target_low_center))
                 @ Matrix.Rotation(rotation_z, 4, 'Z')
                 @ Matrix.Diagonal(Vector(scale) .to_4d())
                 @ Matrix.Translation(-anchor))
    for obj in objects:
        obj.matrix_world = transform @ obj.matrix_world
    bpy.context.view_layer.update()


def finish_piece(objects, material, tile):
    for obj in objects:
        bpy.context.view_layer.objects.active = obj
        obj.select_set(True)
        bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
        obj.select_set(False)
        if material is not None:
            obj.data.materials.clear()
            obj.data.materials.append(material)
        if tile:
            apply_box_uv(obj, tile)


# ---------------------------------------------------------------- walls
def wall_x(prefix, x_start, x_end, fixed_y, thickness, base_z, top_z, openings,
           material, tile, collection):
    y0, y1 = fixed_y - thickness / 2, fixed_y + thickness / 2
    cursor = x_start
    index = 0
    for (o0, o1, z0, z1) in sorted(openings, key=lambda o: o[0]):
        if o0 - cursor > 1e-4:
            add_box(f'{prefix}_pier_{index}', (cursor, y0, base_z), (o0, y1, top_z),
                    material, collection, tile)
        if z0 - base_z > 1e-4:
            add_box(f'{prefix}_sill_{index}', (o0, y0, base_z), (o1, y1, z0),
                    material, collection, tile)
        if top_z - z1 > 1e-4:
            add_box(f'{prefix}_lintel_{index}', (o0, y0, z1), (o1, y1, top_z),
                    material, collection, tile)
        cursor = o1
        index += 1
    if x_end - cursor > 1e-4:
        add_box(f'{prefix}_pier_{index}', (cursor, y0, base_z), (x_end, y1, top_z),
                material, collection, tile)


def wall_y(prefix, y_start, y_end, fixed_x, thickness, base_z, top_z, openings,
           material, tile, collection):
    x0, x1 = fixed_x - thickness / 2, fixed_x + thickness / 2
    cursor = y_start
    index = 0
    for (o0, o1, z0, z1) in sorted(openings, key=lambda o: o[0]):
        if o0 - cursor > 1e-4:
            add_box(f'{prefix}_pier_{index}', (x0, cursor, base_z), (x1, o0, top_z),
                    material, collection, tile)
        if z0 - base_z > 1e-4:
            add_box(f'{prefix}_sill_{index}', (x0, o0, base_z), (x1, o1, z0),
                    material, collection, tile)
        if top_z - z1 > 1e-4:
            add_box(f'{prefix}_lintel_{index}', (x0, o0, z1), (x1, o1, top_z),
                    material, collection, tile)
        cursor = o1
        index += 1
    if y_end - cursor > 1e-4:
        add_box(f'{prefix}_pier_{index}', (x0, cursor, base_z), (x1, y_end, top_z),
                material, collection, tile)


# ------------------------------------------------------------- openings
def window_module(prefix, wall_axis, wall_pos, center_along, sill, width, height,
                  material_frame, material_glass, collection, outward,
                  shutters=True, tile_frame=None):
    tile_frame = tile_frame or TILE['wood_peeling_paint_weathered']
    half = width / 2
    z0, z1 = sill, sill + height
    d0, d1 = wall_pos - 0.14, wall_pos + 0.14
    face = wall_pos + outward * 0.175

    if wall_axis == 'x':
        def box(name, a0, a1, zz0, zz1, material, tile):
            return add_box(name, (a0, d0, zz0), (a1, d1, zz1), material, collection, tile)
        box(f'{prefix}_frame_left', center_along - half, center_along - half + 0.09, z0, z1,
            material_frame, tile_frame)
        box(f'{prefix}_frame_right', center_along + half - 0.09, center_along + half, z0, z1,
            material_frame, tile_frame)
        box(f'{prefix}_frame_bottom', center_along - half, center_along + half, z0, z0 + 0.09,
            material_frame, tile_frame)
        box(f'{prefix}_frame_top', center_along - half, center_along + half, z1 - 0.09, z1,
            material_frame, tile_frame)
        box(f'{prefix}_mullion', center_along - 0.025, center_along + 0.025, z0 + 0.08, z1 - 0.08,
            material_frame, tile_frame)
        for index, fraction in enumerate((0.33, 0.66)):
            zz = z0 + 0.09 + (z1 - z0 - 0.18) * fraction
            box(f'{prefix}_muntin_{index}', center_along - half + 0.07, center_along + half - 0.07,
                zz - 0.015, zz + 0.015, material_frame, tile_frame)
        add_box(f'{prefix}_sill', (center_along - half - 0.07, d0 - 0.05, z0 - 0.07),
                (center_along + half + 0.07, d1 + 0.05, z0), material_frame, collection, tile_frame)
        add_box(f'{prefix}_glass', (center_along - half + 0.05, wall_pos - 0.005, z0 + 0.05),
                (center_along + half - 0.05, wall_pos + 0.005, z1 - 0.05),
                material_glass, collection, 1.0)
    else:
        def box(name, a0, a1, zz0, zz1, material, tile):
            return add_box(name, (d0, a0, zz0), (d1, a1, zz1), material, collection, tile)
        box(f'{prefix}_frame_left', center_along - half, center_along - half + 0.09, z0, z1,
            material_frame, tile_frame)
        box(f'{prefix}_frame_right', center_along + half - 0.09, center_along + half, z0, z1,
            material_frame, tile_frame)
        box(f'{prefix}_frame_bottom', center_along - half, center_along + half, z0, z0 + 0.09,
            material_frame, tile_frame)
        box(f'{prefix}_frame_top', center_along - half, center_along + half, z1 - 0.09, z1,
            material_frame, tile_frame)
        box(f'{prefix}_mullion', center_along - 0.025, center_along + 0.025, z0 + 0.08, z1 - 0.08,
            material_frame, tile_frame)
        for index, fraction in enumerate((0.33, 0.66)):
            zz = z0 + 0.09 + (z1 - z0 - 0.18) * fraction
            box(f'{prefix}_muntin_{index}', center_along - half + 0.07, center_along + half - 0.07,
                zz - 0.015, zz + 0.015, material_frame, tile_frame)
        add_box(f'{prefix}_sill', (d0 - 0.05, center_along - half - 0.07, z0 - 0.07),
                (d1 + 0.05, center_along + half + 0.07, z0), material_frame, collection, tile_frame)
        add_box(f'{prefix}_glass', (wall_pos - 0.005, center_along - half + 0.05, z0 + 0.05),
                (wall_pos + 0.005, center_along + half - 0.05, z1 - 0.05),
                material_glass, collection, 1.0)

    if not shutters:
        return
    for side in (-1, 1):
        pieces = import_kit_piece('Shutter01' if side < 0 else 'Shutter02', collection)
        height_scale = (z1 - z0) / 1.35
        if wall_axis == 'x':
            outward_offset = (face - wall_pos) * 2 + 0.04
            target = (center_along + side * (half + 0.30), wall_pos + outward_offset, z0)
            rotation = math.radians(90)
            scale = (1.0, 1.0, height_scale)
        else:
            outward_offset = (face - wall_pos) * 2 + 0.04
            target = (wall_pos + outward_offset, center_along + side * (half + 0.30), z0)
            rotation = 0.0
            scale = (1.0, 1.0, height_scale)
        place_piece(pieces, target_low_center=target, rotation_z=rotation, scale=scale)
        finish_piece(pieces, bpy.data.materials['MAT_TrimTeak'], 1.0)


def door_module(prefix, wall_axis, wall_pos, center_along, base_z, width, height,
                material, material_glass, collection, double=True, transom=True,
                tile=None):
    tile = tile or TILE['wood_planks_grey']
    d0, d1 = wall_pos - 0.12, wall_pos + 0.12
    half = width / 2
    z0, z1 = base_z, base_z + height

    def leaf_range(index):
        leaf_w = (width - 0.04) / 2 if double else width - 0.04
        a0 = center_along - half + 0.02 + index * (leaf_w + (0.04 if double else 0))
        return a0, a0 + leaf_w

    if wall_axis == 'x':
        add_box(f'{prefix}_frame_l', (center_along - half - 0.06, d0 - 0.03, z0 - 0.02),
                (center_along - half, d1 + 0.03, z1 + 0.04), material, collection, tile)
        add_box(f'{prefix}_frame_r', (center_along + half, d0 - 0.03, z0 - 0.02),
                (center_along + half + 0.06, d1 + 0.03, z1 + 0.04), material, collection, tile)
        add_box(f'{prefix}_frame_top', (center_along - half - 0.06, d0 - 0.03, z1),
                (center_along + half + 0.06, d1 + 0.03, z1 + 0.04), material, collection, tile)
        for index in range(2 if double else 1):
            a0, a1 = leaf_range(index)
            add_box(f'{prefix}_leaf_{index}', (a0, wall_pos - 0.035, z0 + 0.02),
                    (a1, wall_pos + 0.035, z1 - 0.02), material, collection, tile)
            for panel_index, (f0, f1) in enumerate(((0.08, 0.44), (0.52, 0.92))):
                add_box(f'{prefix}_panel_{index}_{panel_index}',
                        (a0 + 0.09, wall_pos - 0.054, z0 + 0.02 + (z1 - z0 - 0.1) * f0),
                        (a1 - 0.09, wall_pos - 0.048, z0 + 0.02 + (z1 - z0 - 0.1) * f1),
                        material, collection, tile)
        if transom:
            add_box(f'{prefix}_transom_glass', (center_along - half + 0.03, wall_pos - 0.006, z1 + 0.06),
                    (center_along + half - 0.03, wall_pos + 0.006, z1 + 0.42),
                    material_glass, collection, 1.0)
            add_box(f'{prefix}_transom_bar', (center_along - half - 0.04, d0 - 0.02, z1 + 0.42),
                    (center_along + half + 0.04, d1 + 0.02, z1 + 0.47), material, collection, tile)
    else:
        add_box(f'{prefix}_frame_l', (d0 - 0.03, center_along - half - 0.06, z0 - 0.02),
                (d1 + 0.03, center_along - half, z1 + 0.04), material, collection, tile)
        add_box(f'{prefix}_frame_r', (d0 - 0.03, center_along + half, z0 - 0.02),
                (d1 + 0.03, center_along + half + 0.06, z1 + 0.04), material, collection, tile)
        add_box(f'{prefix}_frame_top', (d0 - 0.03, center_along - half - 0.06, z1),
                (d1 + 0.03, center_along + half + 0.06, z1 + 0.04), material, collection, tile)
        for index in range(2 if double else 1):
            a0, a1 = leaf_range(index)
            add_box(f'{prefix}_leaf_{index}', (wall_pos - 0.035, a0, z0 + 0.02),
                    (wall_pos + 0.035, a1, z1 - 0.02), material, collection, tile)
            for panel_index, (f0, f1) in enumerate(((0.08, 0.44), (0.52, 0.92))):
                add_box(f'{prefix}_panel_{index}_{panel_index}',
                        (wall_pos - 0.054, a0 + 0.09, z0 + 0.02 + (z1 - z0 - 0.1) * f0),
                        (wall_pos - 0.048, a1 - 0.09, z0 + 0.02 + (z1 - z0 - 0.1) * f1),
                        material, collection, tile)
        if transom:
            add_box(f'{prefix}_transom_glass', (wall_pos - 0.006, center_along - half + 0.03, z1 + 0.06),
                    (wall_pos + 0.006, center_along + half - 0.03, z1 + 0.42),
                    material_glass, collection, 1.0)
            add_box(f'{prefix}_transom_bar', (d0 - 0.02, center_along - half - 0.04, z1 + 0.42),
                    (d1 + 0.02, center_along + half + 0.04, z1 + 0.47), material, collection, tile)


def broken_window_glass(prefix, wall_axis, wall_pos, center_along, sill, width, height,
                        material_glass, collection, seed=7):
    import random
    rng = random.Random(seed)
    cols, rows = 5, 4
    a0 = center_along - width / 2 + 0.06
    z0 = sill + 0.06
    cell_w = (width - 0.12) / cols
    cell_h = (height - 0.12) / rows
    for row in range(rows):
        for col in range(cols):
            if rng.random() < 0.45 and row < 3:
                continue
            lo = a0 + col * cell_w
            hi = lo + cell_w
            zlo, zhi = z0 + row * cell_h, z0 + (row + 1) * cell_h
            if wall_axis == 'x':
                add_box(f'{prefix}_pane_{row}_{col}', (lo, wall_pos - 0.004, zlo),
                        (hi, wall_pos + 0.004, zhi), material_glass, collection, 1.0)
            else:
                add_box(f'{prefix}_pane_{row}_{col}', (wall_pos - 0.004, lo, zlo),
                        (wall_pos + 0.004, hi, zhi), material_glass, collection, 1.0)
    for index in range(3):
        along = a0 + rng.uniform(0.1, width - 0.3)
        size = rng.uniform(0.05, 0.12)
        if wall_axis == 'x':
            add_box(f'{prefix}_shard_{index}', (along, wall_pos - 0.06, sill + 0.004),
                    (along + size, wall_pos - 0.01, sill + 0.012),
                    material_glass, collection, 1.0)
        else:
            add_box(f'{prefix}_shard_{index}', (wall_pos - 0.06, along, sill + 0.004),
                    (wall_pos - 0.01, along + size, sill + 0.012),
                    material_glass, collection, 1.0)


# ----------------------------------------------------------------- roofs
def roof_hip(material, collection, eave_z, eave_x, front_y, back_y, rise, ridge_half,
             thickness=0.15):
    center_y = (front_y + back_y) / 2
    ridge_z = eave_z + rise
    front = [(-eave_x, front_y, eave_z), (eave_x, front_y, eave_z),
             (ridge_half, center_y, ridge_z), (-ridge_half, center_y, ridge_z)]
    back = [(eave_x, back_y, eave_z), (-eave_x, back_y, eave_z),
            (-ridge_half, center_y, ridge_z), (ridge_half, center_y, ridge_z)]
    left = [(-eave_x, back_y, eave_z), (-eave_x, front_y, eave_z),
            (-ridge_half, center_y, ridge_z)]
    right = [(eave_x, front_y, eave_z), (eave_x, back_y, eave_z),
             (ridge_half, center_y, ridge_z)]
    tile = TILE['clay_roof_tiles_02']
    add_slab('ROOF_main_front', front, thickness, material, collection, tile)
    add_slab('ROOF_main_back', back, thickness, material, collection, tile)
    add_slab('ROOF_main_left', left, thickness, material, collection, tile)
    add_slab('ROOF_main_right', right, thickness, material, collection, tile)
    return ridge_z


def ridge_prism(name, p0, p1, width, height, material, collection, tile):
    direction = (Vector(p1) - Vector(p0)).normalized()
    side = direction.cross(Vector((0, 0, 1))).normalized()
    up = Vector((0, 0, 1))
    base0 = Vector(p0) - direction * 0.12
    base1 = Vector(p1) + direction * 0.12
    verts = []
    for base in (base0, base1):
        verts += [tuple(base - side * width / 2), tuple(base + side * width / 2),
                  tuple(base + up * height)]
    faces = [(0, 1, 2), (5, 4, 3), (0, 3, 4, 1), (1, 4, 5, 2), (2, 5, 3, 0)]
    return make_mesh_object(name, verts, faces, material, collection, tile)


# ------------------------------------------------------------------ build
def build():
    reset()
    materials = build_materials()
    walls = new_collection('COLL_Walls')
    openings = new_collection('COLL_Openings')
    veranda = new_collection('COLL_Veranda')
    roof = new_collection('COLL_Roof')
    annex = new_collection('COLL_Annex')

    x0, x1 = -6.0, 6.0
    y0, y1 = 0.0, 9.0
    wall_t = 0.35
    plinth_top = 0.45
    mid_z = 4.2
    eave_wall_z = 7.4
    annex_x0, annex_x1 = 2.0, 6.0
    annex_y1 = 12.2
    annex_top = 3.15

    # plinth
    add_box('PLINTH', (x0 - 0.28, y0 - 0.28, 0.0), (x1 + 0.28, y1 + 0.28, plinth_top + 0.05),
            materials['brick'], walls, TILE['brick_moss_001'])
    add_box('PLINTH_annex', (annex_x0 - 0.2, y1 - 0.2, 0.0), (annex_x1 + 0.2, annex_y1 + 0.2, 0.34),
            materials['brick'], walls, TILE['brick_moss_001'])

    # front and back walls (along X)
    front_openings = [
        (-0.80, 0.80, plinth_top, 3.45),
        (-3.70, -2.75, 0.95, 2.85),
        (2.75, 3.70, 0.95, 2.85),
        (-3.70, -2.75, 4.95, 6.85),
        (-1.55, -0.60, 4.95, 6.85),
        (0.60, 1.55, 4.95, 6.85),
        (2.75, 3.70, 4.95, 6.85),
    ]
    wall_x('WALL_FRONT', x0, x1, y0, wall_t, plinth_top, eave_wall_z, front_openings,
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], walls)
    back_openings = [
        (-3.00, -2.00, plinth_top, 2.90),
        (0.65, 1.55, 1.00, 2.50),
        (-3.05, -2.15, 4.95, 6.45),
        (0.65, 1.55, 4.95, 6.45),
    ]
    wall_x('WALL_BACK', x0, x1, y1, wall_t, plinth_top, eave_wall_z, back_openings,
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], walls)

    # east and west walls (along Y)
    east_openings = [
        (1.70, 2.60, 0.95, 2.85),
        (4.30, 5.20, 0.95, 2.85),
        (1.60, 2.50, 4.95, 6.85),
        (3.55, 4.45, 4.95, 6.85),
        (5.50, 6.40, 4.95, 6.85),
    ]
    wall_y('WALL_EAST', y0, y1, x1, wall_t, plinth_top, eave_wall_z, east_openings,
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], walls)
    west_openings = [
        (2.10, 3.00, 0.95, 2.85),
        (5.00, 5.90, 0.95, 2.85),
        (2.10, 3.00, 4.95, 6.85),
        (5.00, 5.90, 4.95, 6.85),
    ]
    wall_y('WALL_WEST', y0, y1, x0, wall_t, plinth_top, eave_wall_z, west_openings,
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], walls)

    # damaged patches + upper floor band
    add_box('PATCH_damaged_front', (-5.65, -0.19, 0.6), (-4.55, -0.17, 2.3),
            materials['plaster_damaged'], walls, TILE['damaged_plaster'])
    add_box('PATCH_damaged_east', (6.17, 3.1, 0.5), (6.19, 4.75, 2.9),
            materials['plaster_damaged'], walls, TILE['damaged_plaster'])
    add_box('BAND_mid', (x0 - 0.30, y0 - 0.30, mid_z - 0.16), (x1 + 0.30, y1 + 0.30, mid_z),
            materials['concrete'], walls, TILE['concrete_moss'])
    add_box('BAND_annex', (annex_x0 - 0.22, y1 - 0.22, 2.78),
            (annex_x1 + 0.22, annex_y1 + 0.22, 2.94),
            materials['concrete'], annex, TILE['concrete_moss'])

    # veranda
    add_box('VERANDA_platform', (-3.6, -2.7, 0.05), (3.6, 0.0, plinth_top),
            materials['concrete'], veranda, TILE['concrete_moss'])
    for index in range(3):
        z_hi = 0.05 + (index + 1) * 0.135
        add_box(f'VERANDA_step_{index}', (-0.9, -3.65 + index * 0.32, 0.0),
                (0.9, -3.65 + (index + 1) * 0.32 + 0.02, min(z_hi, plinth_top)),
                materials['concrete'], veranda, TILE['concrete_moss'])
    add_box('VERANDA_beam', (-3.9, -2.55, 3.95), (3.9, -2.25, 4.15),
            materials['trim_teak'], veranda, 1.0)
    for x in (-3.35, -1.12, 1.12, 3.35):
        column = import_kit_piece('Column', veranda)
        place_piece(column, target_low_center=(x, -2.40, 0.45), rotation_z=0.0,
                    scale=(1.0, 1.0, (3.95 - 0.45) / 2.11))
        finish_piece(column, materials['trim_teak'], 1.0)
        joist = import_kit_piece('WoodenSupportStraight', veranda)
        place_piece(joist, target_low_center=(x, -0.15, 4.20), rotation_z=math.radians(-90),
                    scale=((2.55) / 1.169, 1.0, 1.0))
        finish_piece(joist, materials['trim_teak'], 1.0)
    top_trim = import_kit_piece('TopTrim_2x1', veranda)
    place_piece(top_trim, target_low_center=(0.0, -2.62, 4.00), rotation_z=math.radians(90),
                scale=(1.0, 3.85, 1.0))
    finish_piece(top_trim, materials['trim_teak'], 1.0)
    add_slab('ROOF_veranda', [(-3.9, -2.85, 4.20), (3.9, -2.85, 4.20),
                              (3.9, 0.0, 4.75), (-3.9, 0.0, 4.75)], 0.1,
             materials['roof'], veranda, TILE['clay_roof_tiles_02'], uv_axis=1)

    # openings
    door_module('DOOR_main', 'x', y0, 0.0, plinth_top, 1.60, 2.55,
                materials['wood'], materials['glass'], openings, double=True, transom=True)
    door_module('DOOR_back', 'x', y1, -2.50, plinth_top, 1.00, 2.20,
                materials['wood'], materials['glass'], openings, double=False, transom=False)
    window_module('WIN_f_gL', 'x', y0, -3.225, 0.95, 0.95, 1.90,
                  materials['wood_white'], materials['glass'], openings, outward=-1)
    window_module('WIN_f_gR', 'x', y0, 3.225, 0.95, 0.95, 1.90,
                  materials['wood_white'], materials['glass'], openings, outward=-1)
    for index, along in enumerate((-3.225, -1.075, 1.075, 3.225)):
        window_module(f'WIN_f_u{index}', 'x', y0, along, 4.95, 0.95, 1.90,
                      materials['wood_white'], materials['glass'], openings, outward=-1)
    window_module('WIN_b_w', 'x', y1, 1.10, 1.00, 0.90, 1.50,
                  materials['wood_white'], materials['glass'], openings, outward=1)
    for index, along in enumerate((-2.60, 1.10)):
        window_module(f'WIN_b_u{index}', 'x', y1, along, 4.95, 0.90, 1.50,
                      materials['wood_white'], materials['glass'], openings, outward=1)
    for index, along in enumerate((2.15, 4.75)):
        window_module(f'WIN_e_g{index}', 'y', x1, along, 0.95, 0.90, 1.90,
                      materials['wood_white'], materials['glass'], openings, outward=1)
    for index, along in enumerate((2.05, 4.00, 5.95)):
        window_module(f'WIN_e_u{index}', 'y', x1, along, 4.95, 0.90, 1.90,
                      materials['wood_white'], materials['glass'], openings, outward=1)
    for index, along in enumerate((2.55, 5.45)):
        window_module(f'WIN_w_g{index}', 'y', x0, along, 0.95, 0.90, 1.90,
                      materials['wood_white'], materials['glass'], openings, outward=-1)
        window_module(f'WIN_w_u{index}', 'y', x0, along, 4.95, 0.90, 1.90,
                      materials['wood_white'], materials['glass'], openings, outward=-1)

    # annex with broken kitchen window
    wall_y('ANNEX_EAST', y1, annex_y1, annex_x1, 0.25, 0.30, annex_top,
           [(10.40, 11.55, 0.95, 2.30)],
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], annex)
    wall_y('ANNEX_WEST', y1, annex_y1, annex_x0, 0.25, 0.30, annex_top, [],
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], annex)
    wall_x('ANNEX_BACK', annex_x0, annex_x1, annex_y1, 0.25, 0.30, annex_top,
           [(3.40, 4.60, 0.30, 2.30)],
           materials['plaster_low'], TILE['worn_mossy_plasterwall'], annex)
    door_module('DOOR_annex', 'x', annex_y1, 4.0, 0.34, 0.9, 1.95,
                materials['wood'], materials['glass'], annex, double=False, transom=False)
    window_module('WIN_kitchen_broken', 'y', annex_x1, 10.975, 0.95, 1.15, 1.35,
                  materials['wood'], materials['glass'], annex, outward=1,
                  shutters=False, tile_frame=TILE['wood_planks_grey'])
    broken_window_glass('KITCHEN_broken', 'y', annex_x1, 10.975, 0.95, 1.15, 1.35,
                        materials['glass'], annex)
    hanging = import_kit_piece('Shutter02', annex)
    present = Matrix.Translation((annex_x1 + 0.16, 11.75, 1.08)) @ Matrix.Rotation(math.radians(-22), 4, 'Y')
    low, high = piece_bounds(hanging)
    anchor = Vector(((low.x + high.x) / 2, (low.y + high.y) / 2, low.z))
    for obj in hanging:
        obj.matrix_world = present @ Matrix.Translation(-anchor) @ obj.matrix_world
    finish_piece(hanging, materials['trim_teak'], 1.0)

    # roofs
    overhang = 0.825
    half_depth = 5.5
    rise = 3.1
    eave_edge_z = eave_wall_z - overhang * rise / half_depth
    ridge_z = roof_hip(materials['roof'], roof, eave_edge_z, 7.15, -1.0, 10.0, rise, 1.65)
    ridge_prism('RIDGE_main', (-1.65, 4.5, ridge_z + 0.02), (1.65, 4.5, ridge_z + 0.02),
                0.34, 0.16, materials['roof_annex'], roof, TILE['clay_roof_tiles_03'])
    hips = [
        ((-7.15, -1.0, eave_edge_z), (-1.65, 4.5, ridge_z)),
        ((7.15, -1.0, eave_edge_z), (1.65, 4.5, ridge_z)),
        ((7.15, 10.0, eave_edge_z), (1.65, 4.5, ridge_z)),
        ((-7.15, 10.0, eave_edge_z), (-1.65, 4.5, ridge_z)),
    ]
    for index, (p0, p1) in enumerate(hips):
        ridge_prism(f'RIDGE_hip_{index}', p0, p1, 0.3, 0.14,
                    materials['roof_annex'], roof, TILE['clay_roof_tiles_03'])
    add_slab('ROOF_annex', [(annex_x0 - 0.35, 8.8, 4.15), (annex_x1 + 0.35, 8.8, 4.15),
                            (annex_x1 + 0.35, annex_y1 + 0.45, 3.05),
                            (annex_x0 - 0.35, annex_y1 + 0.45, 3.05)], 0.1,
             materials['roof_annex'], roof, TILE['clay_roof_tiles_03'], uv_axis=1)
    add_box('GUTTER_front', (-7.0, -1.20, eave_edge_z - 0.18), (7.0, -0.98, eave_edge_z - 0.02),
            materials['rust'], roof, TILE['rusty_metal_sheet'])
    add_box('PIPE_downpipe_w', (-6.42, -0.30, 0.1), (-6.28, -0.16, eave_edge_z - 0.18),
            materials['rust'], roof, TILE['rusty_metal_sheet'])
    add_box('PIPE_downpipe_e', (6.28, -0.30, 0.1), (6.42, -0.16, eave_edge_z - 0.18),
            materials['rust'], roof, TILE['rusty_metal_sheet'])
    add_box('PIPE_elbow_w', (-6.49, -1.12, eave_edge_z - 0.34), (-6.28, -0.16, eave_edge_z - 0.20),
            materials['rust'], roof, TILE['rusty_metal_sheet'])
    add_box('PIPE_elbow_e', (6.28, -1.12, eave_edge_z - 0.34), (6.49, -0.16, eave_edge_z - 0.20),
            materials['rust'], roof, TILE['rusty_metal_sheet'])
    for index, x in enumerate([round(-6.6 + i * 0.75, 2) for i in range(18)]):
        add_box(f'RAFTER_front_{index}', (x - 0.05, -1.02, eave_edge_z - 0.26),
                (x + 0.05, -0.10, eave_edge_z - 0.12), materials['wood'], roof,
                TILE['wood_planks_grey'])

    # stats
    stats = {}
    total = 0
    for obj in bpy.data.objects:
        if obj.type != 'MESH':
            continue
        tris = sum(max(len(p.vertices) - 2, 1) for p in obj.data.polygons)
        total += tris
        stats[obj.name] = {'tris': tris, 'materials': [m.name for m in obj.data.materials]}
    return {
        'objects': len([o for o in bpy.data.objects if o.type == 'MESH']),
        'triangles': total,
        'materials': sorted({m.name for m in bpy.data.materials}),
        'perObject': stats,
    }


def main():
    summary = build()
    BLEND.mkdir(parents=True, exist_ok=True)
    EXPORT.mkdir(parents=True, exist_ok=True)
    MODULES.mkdir(parents=True, exist_ok=True)
    (BLEND / 'house_build_stats.json').write_text(json.dumps(summary, indent=1) + '\n')
    bpy.ops.wm.save_as_mainfile(filepath=str(BLEND / 'RumahAtmaja_House.blend'))
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.gltf(filepath=str(EXPORT / 'RumahAtmaja_House.glb'),
                              export_format='GLB', export_apply=True)
    bpy.ops.export_scene.fbx(filepath=str(EXPORT / 'RumahAtmaja_House.fbx'),
                             use_selection=True, path_mode='COPY', embed_textures=True,
                             mesh_smooth_type='FACE')
    for collection_name, file_name in [('COLL_Walls', 'Module_Walls'),
                                       ('COLL_Openings', 'Module_Openings'),
                                       ('COLL_Veranda', 'Module_Veranda'),
                                       ('COLL_Roof', 'Module_Roof'),
                                       ('COLL_Annex', 'Module_Annex')]:
        bpy.ops.object.select_all(action='DESELECT')
        for obj in bpy.data.collections[collection_name].objects:
            obj.select_set(True)
        bpy.ops.export_scene.fbx(filepath=str(MODULES / f'{file_name}.fbx'),
                                 use_selection=True, path_mode='COPY', embed_textures=True,
                                 mesh_smooth_type='FACE')
    print('HOUSE_BUILD objects=%d triangles=%d' % (summary['objects'], summary['triangles']))


if __name__ == '__main__':
    main()
