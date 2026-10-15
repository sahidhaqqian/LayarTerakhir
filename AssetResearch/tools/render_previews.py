"""Render a local preview.png for every downloaded FBX in AssetResearch/assets.

Run headless:
    blender -b -P AssetResearch/tools/render_previews.py -- [asset_id ...]

The render uses Cycles on CPU with a neutral three-point setup so the preview
shows the actual downloaded geometry and textures, not only the source render.
Blender 4.0 is assumed (FBX importer converts to Blender's Z-up on import).
"""
import math
import sys
from pathlib import Path

import bpy
from mathutils import Vector

REPO = Path(__file__).resolve().parents[2]
ASSETS = REPO / 'AssetResearch' / 'assets'


def read_arguments() -> list:
    if '--' in sys.argv:
        return [arg for arg in sys.argv[sys.argv.index('--') + 1:] if not arg.startswith('-')]
    return []


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def import_model(fbx: Path):
    before = set(bpy.data.objects)
    bpy.ops.import_scene.fbx(filepath=str(fbx), use_anim=False)
    return [obj for obj in bpy.data.objects if obj not in before and obj.type == 'MESH']


def rewire_materials(folder: Path) -> int:
    """Rebuild each material from the downloaded 1K JPGs so the preview shows
    the exact textures shipped with this research drop. The source FBX wants
    EXR maps that were intentionally not downloaded (1K JPG is the lightweight
    Unity import path). glTF ARM = R occlusion, G roughness, B metalness."""
    available = {path.name.lower(): path for path in (folder / 'textures').glob('*')}

    def find(suffix: str):
        for name, path in sorted(available.items()):
            if suffix in name:
                return path
        return None

    diffuse = find('_diff_')
    normal = find('_nor_gl_')
    arm = find('_arm_')
    rewired = 0
    for material in list(bpy.data.materials):
        material.use_nodes = True
        tree = material.node_tree
        tree.nodes.clear()
        output = tree.nodes.new('ShaderNodeOutputMaterial')
        bsdf = tree.nodes.new('ShaderNodeBsdfPrincipled')
        tree.links.new(bsdf.outputs['BSDF'], output.inputs['Surface'])
        if 'glass' in material.name.lower() or 'lens' in material.name.lower():
            bsdf.inputs['Base Color'].default_value = (1.0, 1.0, 1.0, 1.0)
            bsdf.inputs['Roughness'].default_value = 0.04
            bsdf.inputs['Transmission Weight'].default_value = 1.0
            bsdf.inputs['IOR'].default_value = 1.45
            rewired += 1
            continue
        if diffuse:
            texture = tree.nodes.new('ShaderNodeTexImage')
            texture.image = bpy.data.images.load(str(diffuse))
            tree.links.new(texture.outputs['Color'], bsdf.inputs['Base Color'])
        if normal:
            texture = tree.nodes.new('ShaderNodeTexImage')
            texture.image = bpy.data.images.load(str(normal))
            texture.image.colorspace_settings.name = 'Non-Color'
            normal_map = tree.nodes.new('ShaderNodeNormalMap')
            tree.links.new(texture.outputs['Color'], normal_map.inputs['Color'])
            tree.links.new(normal_map.outputs['Normal'], bsdf.inputs['Normal'])
        if arm:
            texture = tree.nodes.new('ShaderNodeTexImage')
            texture.image = bpy.data.images.load(str(arm))
            texture.image.colorspace_settings.name = 'Non-Color'
            separate = tree.nodes.new('ShaderNodeSeparateColor')
            tree.links.new(texture.outputs['Color'], separate.inputs['Color'])
            tree.links.new(separate.outputs['Green'], bsdf.inputs['Roughness'])
            tree.links.new(separate.outputs['Blue'], bsdf.inputs['Metallic'])
        rewired += 1
    return rewired


def world_bounds(objects):
    low = Vector((math.inf,) * 3)
    high = Vector((-math.inf,) * 3)
    for obj in objects:
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            low = Vector(map(min, low, point))
            high = Vector(map(max, high, point))
    return low, high


def make_lights(center: Vector, radius: float) -> None:
    specs = [
        ('Key', (1.0, -1.4, 1.3), 320.0 * radius * radius),
        ('Fill', (-1.5, -0.6, 0.4), 110.0 * radius * radius),
        ('Rim', (-0.4, 1.6, 1.0), 180.0 * radius * radius),
    ]
    for name, direction, energy in specs:
        light_data = bpy.data.lights.new(name, 'AREA')
        light_data.energy = energy
        light_data.size = radius * 2.2
        light = bpy.data.objects.new(name, light_data)
        bpy.context.collection.objects.link(light)
        offset = Vector(direction).normalized() * radius * 3.2
        light.location = center + offset
        direction_to = center - light.location
        light.rotation_euler = direction_to.to_track_quat('-Z', 'Y').to_euler()
    world = bpy.data.worlds.new('PreviewWorld')
    world.use_nodes = True
    background = world.node_tree.nodes['Background']
    background.inputs['Color'].default_value = (0.09, 0.088, 0.083, 1.0)
    background.inputs['Strength'].default_value = 1.0
    bpy.context.scene.world = world


def make_camera(center: Vector, max_dim: float) -> None:
    camera_data = bpy.data.cameras.new('PreviewCamera')
    camera_data.lens = 52
    camera = bpy.data.objects.new('PreviewCamera', camera_data)
    bpy.context.collection.objects.link(camera)
    azimuth = math.radians(38)
    elevation = math.radians(22)
    distance = (max_dim * 0.5) / math.tan(camera_data.angle * 0.5) * 1.3
    offset = Vector((math.sin(azimuth) * math.cos(elevation),
                     -math.cos(azimuth) * math.cos(elevation),
                     math.sin(elevation))) * distance
    camera.location = center + offset
    camera.rotation_euler = (center - camera.location).to_track_quat('-Z', 'Y').to_euler()
    bpy.context.scene.camera = camera


def configure_render(size: int = 720, samples: int = 48) -> None:
    scene = bpy.context.scene
    scene.render.engine = 'CYCLES'
    scene.cycles.device = 'CPU'
    scene.cycles.samples = samples
    scene.cycles.use_denoising = False
    scene.cycles.max_bounces = 6
    scene.render.resolution_x = size
    scene.render.resolution_y = size
    scene.render.resolution_percentage = 100
    scene.render.film_transparent = False
    scene.view_settings.view_transform = 'Filmic'


def render_asset(asset_id: str) -> None:
    folder = ASSETS / asset_id
    fbx = next(folder.glob('*.fbx'))
    reset_scene()
    meshes = import_model(fbx)
    if not meshes:
        raise RuntimeError('no mesh imported from ' + str(fbx))
    fixed = rewire_materials(folder)
    low, high = world_bounds(meshes)
    center = (low + high) * 0.5
    size = high - low
    radius = max(size.length * 0.5, 0.001)
    max_dim = max(size.x, size.y, size.z, 0.001)
    make_lights(center, radius)
    make_camera(center, max_dim)
    configure_render()
    bpy.context.scene.render.filepath = str(folder / 'preview.png')
    bpy.ops.render.render(write_still=True)
    print(f'PREVIEW {asset_id} meshes={len(meshes)} textures_fixed={fixed}', flush=True)


def main() -> None:
    requested = read_arguments()
    folders = sorted(path for path in ASSETS.iterdir() if path.is_dir()) if not requested \
        else [ASSETS / asset_id for asset_id in requested]
    for folder in folders:
        if not folder.is_dir():
            print('SKIP missing', folder, flush=True)
            continue
        try:
            render_asset(folder.name)
        except Exception as error:
            print('FAILED preview', folder.name, repr(error), flush=True)


if __name__ == '__main__':
    main()
