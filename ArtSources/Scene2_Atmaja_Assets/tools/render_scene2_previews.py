"""Measure and render local previews for every Scene 2 model.

Run headless:
    blender -b -P ArtSources/Scene2_Atmaja_Assets/tools/render_scene2_previews.py

Outputs:
  * manifest/measured-bounds.json   {asset_id: {bounds: [x,y,z], triangles: n}}
  * <category>/<asset>/preview.png  Cycles CPU render of the shipped model

Wiring rules per source (files were downloaded by fetch_scene2_assets.py):
  * polyhaven model      - diff/nor_gl/arm set found in textures/
  * opengameart wooden-door-0  - Wooden_Door_basecolor/normal
  * opengameart breakable-doors-and-windows - BASE_COLOR_1/NORMAL/ROUGHNESS/METALLIC
  * opengameart chain    - DefaultMaterial BaseColor/NormalOpenGL/Roughness/Metallic
  * opengameart small-old-house - OBJ with bundled MTL/textures
"""
from pathlib import Path
import json
import math
import sys

import bpy
from mathutils import Vector

REPO = Path(__file__).resolve().parents[3]
PACKAGE = REPO / 'ArtSources' / 'Scene2_Atmaja_Assets'
MANIFEST = PACKAGE / 'manifest' / 'source-manifest.json'
OUT = PACKAGE / 'manifest' / 'measured-bounds.json'


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def import_model(path: Path):
    before = set(bpy.data.objects)
    if path.suffix.lower() == '.fbx':
        bpy.ops.import_scene.fbx(filepath=str(path), use_anim=False)
    elif path.suffix.lower() == '.obj':
        bpy.ops.wm.obj_import(filepath=str(path))
    return [obj for obj in bpy.data.objects if obj not in before and obj.type == 'MESH']


def import_images(names):
    images = {}
    for name in names:
        path = PACKAGE / name
        if path.is_file():
            images[path.name] = path
    return images


def find_file(folder: Path, *parts):
    for path in sorted(folder.glob('*')):
        lowered = path.name.lower()
        if all(part in lowered for part in parts):
            return path
    return None


def wire_ph(folder: Path) -> None:
    textures = folder / 'textures'
    diffuse = find_file(textures, '_diff_')
    normal = find_file(textures, '_nor_gl_')
    arm = find_file(textures, '_arm_')
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
            continue
        if diffuse:
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(diffuse))
            tree.links.new(tex.outputs['Color'], bsdf.inputs['Base Color'])
        if normal:
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(normal))
            tex.image.colorspace_settings.name = 'Non-Color'
            nmap = tree.nodes.new('ShaderNodeNormalMap')
            tree.links.new(tex.outputs['Color'], nmap.inputs['Color'])
            tree.links.new(nmap.outputs['Normal'], bsdf.inputs['Normal'])
        if arm:
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(arm))
            tex.image.colorspace_settings.name = 'Non-Color'
            separate = tree.nodes.new('ShaderNodeSeparateColor')
            tree.links.new(tex.outputs['Color'], separate.inputs['Color'])
            tree.links.new(separate.outputs['Green'], bsdf.inputs['Roughness'])
            tree.links.new(separate.outputs['Blue'], bsdf.inputs['Metallic'])


def wire_named(folder: Path, base, normal, rough=None, metal=None, glass_names=()):
    base_path = PACKAGE / base if base else None
    normal_path = PACKAGE / normal if normal else None
    rough_path = PACKAGE / rough if rough else None
    metal_path = PACKAGE / metal if metal else None
    for material in list(bpy.data.materials):
        material.use_nodes = True
        tree = material.node_tree
        tree.nodes.clear()
        output = tree.nodes.new('ShaderNodeOutputMaterial')
        bsdf = tree.nodes.new('ShaderNodeBsdfPrincipled')
        tree.links.new(bsdf.outputs['BSDF'], output.inputs['Surface'])
        if material.name in glass_names or 'glass' in material.name.lower():
            bsdf.inputs['Base Color'].default_value = (1.0, 1.0, 1.0, 1.0)
            bsdf.inputs['Roughness'].default_value = 0.04
            bsdf.inputs['Transmission Weight'].default_value = 1.0
            continue
        if base_path and base_path.is_file():
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(base_path))
            tree.links.new(tex.outputs['Color'], bsdf.inputs['Base Color'])
        if normal_path and normal_path.is_file():
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(normal_path))
            tex.image.colorspace_settings.name = 'Non-Color'
            nmap = tree.nodes.new('ShaderNodeNormalMap')
            tree.links.new(tex.outputs['Color'], nmap.inputs['Color'])
            tree.links.new(nmap.outputs['Normal'], bsdf.inputs['Normal'])
        if rough_path and rough_path.is_file():
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(rough_path))
            tex.image.colorspace_settings.name = 'Non-Color'
            tree.links.new(tex.outputs['Color'], bsdf.inputs['Roughness'])
        if metal_path and metal_path.is_file():
            tex = tree.nodes.new('ShaderNodeTexImage')
            tex.image = bpy.data.images.load(str(metal_path))
            tex.image.colorspace_settings.name = 'Non-Color'
            tree.links.new(tex.outputs['Color'], bsdf.inputs['Metallic'])


def bounds_and_tris(objects):
    low = Vector((math.inf,) * 3)
    high = Vector((-math.inf,) * 3)
    triangles = 0
    for obj in objects:
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            low = Vector(map(min, low, point))
            high = Vector(map(max, high, point))
        for polygon in obj.data.polygons:
            triangles += max(len(polygon.vertices) - 2, 1)
    return low, high, triangles


def setup_lights_camera(center, radius, max_dim):
    for name, direction, energy in [
        ('Key', (1.0, -1.4, 1.3), 320.0), ('Fill', (-1.5, -0.6, 0.4), 110.0),
        ('Rim', (-0.4, 1.6, 1.0), 180.0)]:
        data = bpy.data.lights.new(name, 'AREA')
        data.energy = energy * radius * radius
        data.size = radius * 2.2
        light = bpy.data.objects.new(name, data)
        bpy.context.collection.objects.link(light)
        light.location = center + Vector(direction).normalized() * radius * 3.2
        light.rotation_euler = (center - light.location).to_track_quat('-Z', 'Y').to_euler()
    world = bpy.data.worlds.new('PreviewWorld')
    world.use_nodes = True
    world.node_tree.nodes['Background'].inputs['Color'].default_value = (0.09, 0.088, 0.083, 1)
    bpy.context.scene.world = world
    camera_data = bpy.data.cameras.new('PreviewCamera')
    camera_data.lens = 52
    camera = bpy.data.objects.new('PreviewCamera', camera_data)
    bpy.context.collection.objects.link(camera)
    azimuth, elevation = math.radians(38), math.radians(22)
    distance = (max_dim * 0.5) / math.tan(camera_data.angle * 0.5) * 1.3
    camera.location = center + Vector((math.sin(azimuth) * math.cos(elevation),
                                       -math.cos(azimuth) * math.cos(elevation),
                                       math.sin(elevation))) * distance
    camera.rotation_euler = (center - camera.location).to_track_quat('-Z', 'Y').to_euler()
    bpy.context.scene.camera = camera


def configure_render():
    scene = bpy.context.scene
    scene.render.engine = 'CYCLES'
    scene.cycles.device = 'CPU'
    scene.cycles.samples = 20
    scene.cycles.use_denoising = False
    scene.cycles.max_bounces = 4
    scene.render.resolution_x = scene.render.resolution_y = 560
    scene.view_settings.view_transform = 'Filmic'


def model_path(record: dict) -> Path:
    asset_id = record['id']
    category = record['category']
    folder = PACKAGE / category / asset_id
    if asset_id == 'wooden-door-0':
        return folder / 'Wooden_Door.fbx'
    if asset_id == 'breakable-doors-and-windows':
        # render a representative pair: the double broken door
        return folder / 'fbx' / 'Door_DoorFrame_Broken_Double.fbx'
    if asset_id == 'chain':
        return folder / 'chain.fbx'
    if asset_id == 'small-old-house':
        return folder / 'House.obj'
    return folder / (asset_id + '_1k.fbx')


def wire_for(record: dict, folder: Path) -> None:
    asset_id = record['id']
    if record['source'] == 'polyhaven':
        wire_ph(folder)
    elif asset_id == 'wooden-door-0':
        wire_named(folder, 'Story_Objects/wooden-door-0/Wooden_Door_basecolor.png',
                   'Story_Objects/wooden-door-0/Wooden_Door_normal.png')
    elif asset_id == 'breakable-doors-and-windows':
        wire_named(folder,
                   'Story_Objects/breakable-doors-and-windows/textures/BASE_COLOR_1_BreakableEntrances.png',
                   'Story_Objects/breakable-doors-and-windows/textures/NORMAL_BreakableEntrances.png',
                   'Story_Objects/breakable-doors-and-windows/textures/ROUGHNESS_BreakableEntrances.png',
                   'Story_Objects/breakable-doors-and-windows/textures/METALLIC_BreakableEntrances.png')
    elif asset_id == 'chain':
        wire_named(folder, 'Story_Objects/chain/textures/DefaultMaterial_Base_Color.png',
                   'Story_Objects/chain/textures/DefaultMaterial_Normal_OpenGL.png',
                   'Story_Objects/chain/textures/DefaultMaterial_Roughness.png',
                   'Story_Objects/chain/textures/DefaultMaterial_Metallic.png')
    # small-old-house keeps its OBJ MTL wiring


def main() -> None:
    records = json.loads(MANIFEST.read_text())
    measured = {}
    for record in records:
        if record.get('type') == 'texture':
            continue
        asset_id = record['id']
        folder = PACKAGE / record['category'] / asset_id
        path = model_path(record)
        preview = folder / 'preview.png'
        try:
            reset_scene()
            meshes = import_model(path)
            if not meshes:
                raise RuntimeError('no mesh imported from ' + str(path))
            wire_for(record, folder)
            low, high, triangles = bounds_and_tris(meshes)
            size = high - low
            center = (low + high) * 0.5
            radius = max(size.length * 0.5, 0.001)
            max_dim = max(size.x, size.y, size.z, 0.001)
            setup_lights_camera(center, radius, max_dim)
            configure_render()
            bpy.context.scene.render.filepath = str(preview)
            bpy.ops.render.render(write_still=True)
            measured[asset_id] = {
                'bounds': [round(value, 4) for value in size],
                'triangles': triangles,
            }
            print(f'RENDERED {asset_id} tris={triangles} bounds={measured[asset_id]["bounds"]}',
                  flush=True)
        except Exception as error:
            print('FAILED', asset_id, repr(error), flush=True)
    OUT.write_text(json.dumps(measured, indent=2) + '\n')
    print('wrote', OUT)


if __name__ == '__main__':
    main()
