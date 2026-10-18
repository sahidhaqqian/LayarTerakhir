"""Build the fallen frangipani (kamboja) for LayarTerakhir Prolog Adegan 2.

Method: ADAPTED + PROCEDURAL
  * Base geometry: dead_tree_trunk_02 (Poly Haven, CC0) - gnarled fallen trunk,
    decimated for the game-ready export (source kept untouched in Sources/).
  * Root plate: root_cluster_01 from the approved Scene 2 collection (CC0),
    decimated and re-materialed with the trunk bark for consistency.
  * Procedural: thick blunt Plumeria branches, leaf rosettes and flower
    billboards generated in this script (leaf/flower textures authored with PIL).

Run headless:
    blender -b -P ArtSources/Scene2_Atmaja_Recovery/03_Frangipani_Tree/Blender/build_frangipani.py

Outputs:
    03_Frangipani_Tree/Blender/FallenFrangipani.blend
    03_Frangipani_Tree/Blender/frangipani_build_stats.json
    03_Frangipani_Tree/Export/FallenFrangipani.fbx / .glb
    03_Frangipani_Tree/Textures/leaf.png, flower.png
No Unity files are touched.
"""
from pathlib import Path
import bmesh
import json
import math
import random

import bpy
from mathutils import Matrix, Vector
from PIL import Image, ImageDraw

PKG = Path(__file__).resolve().parents[2]
FRANG = PKG / '03_Frangipani_Tree'
BLEND = FRANG / 'Blender'
EXPORT = FRANG / 'Export'
TEX_OUT = FRANG / 'Textures'
SOURCES = FRANG / 'Sources'
SCENE2 = PKG.parent / 'Scene2_Atmaja_Assets'

TRUNK_FBX = SOURCES / 'dead_tree_trunk_02_1k.fbx'
TRUNK_TEX = SOURCES / 'textures'
ROOT_DIR = SCENE2 / 'Vegetation' / 'root_cluster_01'


# ---------------------------------------------------------------- textures
def author_leaf_texture():
    size = 512
    image = Image.new('RGBA', (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    points = []
    for i in range(60):
        t = i / 59
        x = size * (0.08 + 0.84 * t)
        width = math.sin(math.pi * t) * size * 0.145
        points.append((t, x, width))
    top = []
    bottom = []
    for t, x, width in points:
        bend = (0.5 - t) * 26
        top.append((x, size / 2 - width + bend))
        bottom.append((x, size / 2 + width + bend))
    draw.polygon(top + list(reversed(bottom)), fill=(88, 134, 66, 255))
    for t, x, width in points[3:-3]:
        bend = (0.5 - t) * 26
        draw.line([(x, size / 2 - width * 0.72 + bend), (x, size / 2 + width * 0.72 + bend)],
                  fill=(58, 96, 44, 255), width=2)
    draw.line([(size * 0.1, size / 2 + 8), (size * 0.9, size / 2 - 8)],
              fill=(120, 150, 90, 200), width=3)
    TEX_OUT.mkdir(parents=True, exist_ok=True)
    image.save(TEX_OUT / 'leaf.png')


def author_flower_texture():
    size = 256
    image = Image.new('RGBA', (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    centre = size / 2
    for index in range(5):
        angle = index * 72 - 90
        angle_rad = math.radians(angle)
        petal = Image.new('RGBA', (size, size), (0, 0, 0, 0))
        petal_draw = ImageDraw.Draw(petal)
        petal_draw.ellipse([centre - size * 0.16, centre - size * 0.42,
                            centre + size * 0.16, centre - size * 0.02],
                           fill=(246, 240, 225, 255))
        petal = petal.rotate(angle, resample=Image.BICUBIC, center=(centre, centre))
        image.alpha_composite(petal)
    draw = ImageDraw.Draw(image)
    draw.ellipse([centre - size * 0.09, centre - size * 0.09,
                  centre + size * 0.09, centre + size * 0.09],
                 fill=(232, 196, 64, 255))
    image.save(TEX_OUT / 'flower.png')


def image_material(name, path, alpha=False):
    material = bpy.data.materials.new(name)
    material.use_nodes = True
    tree = material.node_tree
    texture = tree.nodes.new('ShaderNodeTexImage')
    texture.image = bpy.data.images.load(str(path))
    bsdf = tree.nodes['Principled BSDF']
    tree.links.new(texture.outputs['Color'], bsdf.inputs['Base Color'])
    bsdf.inputs['Roughness'].default_value = 0.6
    if alpha:
        tree.links.new(texture.outputs['Alpha'], bsdf.inputs['Alpha'])
        material.blend_method = 'CLIP'
        material.alpha_threshold = 0.4
        material.use_backface_culling = False
    return material


# ------------------------------------------------------------------- mesh
def apply_uv_box(obj, tile=0.55):
    mesh = obj.data
    if not mesh.uv_layers:
        mesh.uv_layers.new()
    uv_layer = mesh.uv_layers.active.data
    for polygon in mesh.polygons:
        normal = polygon.normal.normalized()
        axis = max(range(3), key=lambda index: abs(normal[index]))
        others = [index for index in range(3) if index != axis]
        for loop_index in polygon.loop_indices:
            vertex = mesh.vertices[mesh.loops[loop_index].vertex_index].co
            uv_layer[loop_index].uv = (vertex[others[0]] / tile, vertex[others[1]] / tile)


def tapered_cane(name, points, radius_start, radius_end, material, collection, segments=10):
    bm = bmesh.new()
    rings = []
    count = len(points)
    for index, point in enumerate(points):
        if index == 0:
            direction = (Vector(points[1]) - Vector(point)).normalized()
        elif index == count - 1:
            direction = (Vector(point) - Vector(points[index - 1])).normalized()
        else:
            direction = (Vector(points[index + 1]) - Vector(points[index - 1])).normalized()
        normal = direction.cross(Vector((0, 0, 1)))
        if normal.length < 1e-5:
            normal = direction.cross(Vector((0, 1, 0)))
        normal.normalize()
        binormal = direction.cross(normal).normalized()
        linear = radius_start + (radius_end - radius_start) * index / (count - 1)
        bump = 1.0 + 0.16 * math.sin(index * 1.9 + radius_start * 40)
        radius = linear * bump
        ring = []
        for j in range(segments):
            theta = 2 * math.pi * j / segments
            offset = normal * math.cos(theta) * radius + binormal * math.sin(theta) * radius
            ring.append(bm.verts.new(Vector(point) + offset))
        rings.append(ring)
    for i in range(len(rings) - 1):
        for j in range(segments):
            bm.faces.new((rings[i][j], rings[i][(j + 1) % segments],
                          rings[i + 1][(j + 1) % segments], rings[i + 1][j]))
    bm.faces.new(list(reversed(rings[0])))
    bm.faces.new(rings[-1])
    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    mesh = bpy.data.meshes.new(name)
    bm.to_mesh(mesh)
    bm.free()
    obj = bpy.data.objects.new(name, mesh)
    collection.objects.link(obj)
    obj.data.materials.append(material)
    for polygon in obj.data.polygons:
        polygon.use_smooth = True
    obj.data.use_auto_smooth = True
    obj.data.auto_smooth_angle = math.radians(34)
    apply_uv_box(obj)
    return obj


def card(name, location, size, rotation, material, collection):
    mesh = bpy.data.meshes.new(name)
    half = size / 2
    verts = [(-half, 0, -half), (half, 0, -half), (half, 0, half), (-half, 0, half)]
    mesh.from_pydata(verts, [], [(0, 1, 2, 3)])
    mesh.update()
    uv = mesh.uv_layers.new()
    for loop, co in zip(uv.data, [(0, 0), (1, 0), (1, 1), (0, 1)]):
        loop.uv = co
    obj = bpy.data.objects.new(name, mesh)
    collection.objects.link(obj)
    obj.data.materials.append(material)
    obj.location = location
    obj.rotation_euler = rotation
    return obj


def decimate_object(obj, ratio):
    modifier = obj.modifiers.new('Decimate', 'DECIMATE')
    modifier.ratio = ratio
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.modifier_apply(modifier=modifier.name)


# ------------------------------------------------------------------ build
def build():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    for collection in list(bpy.data.collections):
        bpy.data.collections.remove(collection)
    collection = bpy.data.collections.new('COLL_Frangipani')
    bpy.context.scene.collection.children.link(collection)

    author_leaf_texture()
    author_flower_texture()

    bark = bpy.data.materials.new('FRANG_Bark')
    bark.use_nodes = True
    tree = bark.node_tree
    tree.nodes.clear()
    output = tree.nodes.new('ShaderNodeOutputMaterial')
    bsdf = tree.nodes.new('ShaderNodeBsdfPrincipled')
    tree.links.new(bsdf.outputs['BSDF'], output.inputs['Surface'])
    def load_map(patterns, non_color=False):
        for pattern in patterns:
            for path in sorted(TRUNK_TEX.glob(pattern)):
                texture = tree.nodes.new('ShaderNodeTexImage')
                texture.image = bpy.data.images.load(str(path))
                if non_color:
                    texture.image.colorspace_settings.name = 'Non-Color'
                return texture
        return None
    diffuse = load_map(['*diff*'])
    normal = load_map(['*nor*'], non_color=True)
    arm = load_map(['*arm*'], non_color=True)
    if diffuse:
        tree.links.new(diffuse.outputs['Color'], bsdf.inputs['Base Color'])
    if normal:
        nmap = tree.nodes.new('ShaderNodeNormalMap')
        tree.links.new(normal.outputs['Color'], nmap.inputs['Color'])
        tree.links.new(nmap.outputs['Normal'], bsdf.inputs['Normal'])
    if arm:
        separate = tree.nodes.new('ShaderNodeSeparateColor')
        tree.links.new(arm.outputs['Color'], separate.inputs['Color'])
        tree.links.new(separate.outputs['Green'], bsdf.inputs['Roughness'])
        tree.links.new(separate.outputs['Blue'], bsdf.inputs['Metallic'])

    leaf_material = image_material('FRANG_Leaf', TEX_OUT / 'leaf.png', alpha=True)
    flower_material = image_material('FRANG_Flower', TEX_OUT / 'flower.png', alpha=True)

    # trunk
    bpy.ops.import_scene.fbx(filepath=str(TRUNK_FBX), use_anim=False)
    trunk_objects = [o for o in bpy.data.objects if o.type == 'MESH']
    for obj in trunk_objects:
        for existing in list(obj.users_collection):
            existing.objects.unlink(obj)
        collection.objects.link(obj)
        obj.data.materials.clear()
        obj.data.materials.append(bark)
        decimate_object(obj, 0.45)
    # scale 0.75, sit on the ground, trunk runs +X with root end at x=-1.2
    for obj in trunk_objects:
        obj.matrix_world = Matrix.Translation((0.35, 0.0, 0.26)) @ \
            Matrix.Diagonal(Vector((0.75, 0.75, 0.75)).to_4d()) @ obj.matrix_world
    bpy.context.view_layer.update()

    # root plate (approved Scene 2 asset), rotated to stand as an upended root ball
    root_fbx = next(ROOT_DIR.glob('*.fbx'))
    before = set(bpy.data.objects)
    bpy.ops.import_scene.fbx(filepath=str(root_fbx), use_anim=False)
    keep = [o for o in bpy.data.objects
            if o not in before and o.type == 'MESH']
    for obj in keep:
        for existing in list(obj.users_collection):
            existing.objects.unlink(obj)
        collection.objects.link(obj)
        decimate_object(obj, 0.25)
    root_transform = (Matrix.Rotation(math.radians(-10), 4, 'Z')
                      @ Matrix.Rotation(math.radians(96), 4, 'Y')
                      @ Matrix.Rotation(math.radians(6), 4, 'X')
                      @ Matrix.Diagonal(Vector((0.40, 0.44, 0.44)).to_4d()))
    low = Vector((math.inf,) * 3)
    high = Vector((-math.inf,) * 3)
    for obj in keep:
        obj.matrix_world = root_transform @ obj.matrix_world
    bpy.context.view_layer.update()
    for obj in keep:
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            low = Vector(map(min, low, point))
            high = Vector(map(max, high, point))
    shift = Vector((-1.12, 0.0, 0.02)) - Vector(((low.x + high.x) / 2, (low.y + high.y) / 2, low.z))
    for obj in keep:
        obj.matrix_world = Matrix.Translation(shift) @ obj.matrix_world
    bpy.context.view_layer.update()

    # procedural plumeria branches: thick, irregular, blunt tips
    rng = random.Random(11)
    branch_specs = [
        ((1.55, 0.10, 0.35), (2.30, -0.35, 1.05), (2.85, -0.75, 1.55), 0.105),
        ((2.20, 0.05, 0.42), (2.95, 0.45, 1.05), (3.45, 0.80, 1.60), 0.095),
        ((1.10, -0.10, 0.40), (1.55, -0.85, 1.00), (1.95, -1.35, 1.45), 0.100),
        ((0.55, 0.25, 0.45), (0.60, 1.05, 0.95), (0.75, 1.55, 1.40), 0.090),
        ((1.95, 0.30, 0.45), (2.15, 0.75, 0.95), (2.45, 1.05, 1.30), 0.080),
    ]
    tips = []
    for index, (p0, p1, p2, radius) in enumerate(branch_specs):
        mid = tuple((a + b) / 2 + rng.uniform(-0.06, 0.06)
                    for a, b in zip(p0, p1))
        tapered_cane(f'BRANCH_{index}', [p0, mid, p1, p2], radius, radius * 0.55,
                     bark, collection)
        tips.append((Vector(p2), radius))
        # short fork at the tip keeps the blunt fork silhouette of Plumeria
        direction = (Vector(p2) - Vector(p1)).normalized()
        for side in (-1, 1):
            offset = Vector((0, math.cos(math.radians(35)) * side,
                             math.sin(math.radians(35)))) * 0.22
            end = Vector(p2) + direction * 0.30 + offset
            tapered_cane(f'BRANCH_{index}_fork_{side}', [tuple(p2), tuple(end)],
                         radius * 0.55, radius * 0.40, bark, collection)
            tips.append((end, radius * 0.42))

    # leaf rosettes and flowers at the tips
    for index, (tip, radius) in enumerate(tips):
        leaves = 8
        for leaf_index in range(leaves):
            angle = leaf_index * (360 / leaves) + rng.uniform(-16, 16)
            radial = Vector((math.cos(math.radians(angle)), math.sin(math.radians(angle)),
                             rng.uniform(-0.12, 0.12))).normalized()
            location = Vector(tip) + radial * 0.19 + Vector((0, 0, 0.03))
            rotation = radial.to_track_quat('X', 'Z').to_euler()
            card(f'LEAF_{index}_{leaf_index}', location, 0.44,
                 (rotation.x, rotation.y, rotation.z), leaf_material, collection)
        if index % 3 == 0:
            for flower_index in range(3):
                offset = Vector((rng.uniform(-0.18, 0.18), rng.uniform(-0.18, 0.18),
                                 rng.uniform(0.05, 0.22)))
                card(f'FLOWER_{index}_{flower_index}', Vector(tip) + offset, 0.16,
                     (rng.uniform(-0.5, 0.5), rng.uniform(-0.5, 0.5), rng.uniform(0, 3)),
                     flower_material, collection)

    stats = {}
    total = 0
    for obj in bpy.data.objects:
        if obj.type != 'MESH':
            continue
        tris = sum(max(len(p.vertices) - 2, 1) for p in obj.data.polygons)
        total += tris
        stats[obj.name] = tris
    return {'objects': len(stats), 'triangles': total,
            'materials': sorted({m.name for m in bpy.data.materials}), 'perObject': stats}


def main():
    summary = build()
    BLEND.mkdir(parents=True, exist_ok=True)
    EXPORT.mkdir(parents=True, exist_ok=True)
    (BLEND / 'frangipani_build_stats.json').write_text(json.dumps(summary, indent=1) + '\n')
    bpy.ops.wm.save_as_mainfile(filepath=str(BLEND / 'FallenFrangipani.blend'))
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.gltf(filepath=str(EXPORT / 'FallenFrangipani.glb'),
                              export_format='GLB', export_apply=True)
    bpy.ops.export_scene.fbx(filepath=str(EXPORT / 'FallenFrangipani.fbx'),
                             use_selection=True, path_mode='COPY', embed_textures=True)
    print('FRANGIPANI_BUILD objects=%d triangles=%d' % (summary['objects'], summary['triangles']))


if __name__ == '__main__':
    main()
