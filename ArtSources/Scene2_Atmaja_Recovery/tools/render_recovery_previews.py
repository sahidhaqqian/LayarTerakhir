"""Render the required preview views for the three Scene 2 recovery assets.

Every preview is rendered from the EXPORTED GLB files (not the .blend), so the
images prove the delivered files import and display correctly.

Run headless:
    blender -b -P ArtSources/Scene2_Atmaja_Recovery/tools/render_recovery_previews.py

Outputs: ArtSources/Scene2_Atmaja_Recovery/Previews/*.png
"""
from pathlib import Path
import math

import bpy
from mathutils import Vector

PKG = Path(__file__).resolve().parents[1]
OUT = PKG / 'Previews'
OUT.mkdir(exist_ok=True)

JOBS = [
    ('RumahAtmaja_House.glb', [
        ('house_front', (0.0, -26.0, 6.5), (0.0, 5.0, 5.0), 42),
        ('house_side', (24.0, 5.0, 5.5), (0.0, 5.0, 4.5), 42),
        ('house_three_quarter', (17.0, -17.0, 10.0), (0.0, 4.5, 4.5), 42),
        ('house_material_closeup', (2.6, -8.2, 3.4), (-1.4, 0.0, 2.6), 52),
        ('house_kitchen_window', (9.4, 15.0, 2.3), (6.0, 10.9, 1.6), 46),
    ]),
    ('DamarScooter.glb', [
        ('motorcycle_side', (3.2, 0.05, 0.8), (0.0, 0.05, 0.5), 60),
        ('motorcycle_three_quarter', (2.4, -2.4, 1.35), (0.0, 0.0, 0.48), 60),
        ('motorcycle_front', (0.0, -3.2, 0.95), (0.0, 0.0, 0.55), 60),
    ]),
    ('FallenFrangipani.glb', [
        ('frangipani_silhouette', (3.4, -7.4, 3.1), (0.9, 0.1, 0.8), 45),
        ('frangipani_trunk_roots_closeup', (-4.6, -3.4, 1.8), (-1.0, 0.1, 0.8), 48),
        ('frangipani_three_quarter', (-4.6, -5.6, 2.7), (0.9, 0.0, 0.7), 45),
    ]),
]

EXPORT_DIRS = {
    'RumahAtmaja_House.glb': PKG / '01_House' / 'Export',
    'DamarScooter.glb': PKG / '02_Motorcycle' / 'Export',
    'FallenFrangipani.glb': PKG / '03_Frangipani_Tree' / 'Export',
}


def setup_world_and_lights(center, radius):
    world = bpy.data.worlds.new('PreviewWorld')
    world.use_nodes = True
    background = world.node_tree.nodes['Background']
    background.inputs['Color'].default_value = (0.105, 0.11, 0.125, 1)
    background.inputs['Strength'].default_value = 0.9
    bpy.context.scene.world = world
    sun = bpy.data.lights.new('Sun', 'SUN')
    sun.energy = 3.6
    sun.angle = math.radians(5)
    sun_obj = bpy.data.objects.new('Sun', sun)
    bpy.context.collection.objects.link(sun_obj)
    sun_obj.rotation_euler = (math.radians(52), 0, math.radians(-38))
    fill = bpy.data.lights.new('Fill', 'AREA')
    fill.energy = 420 * max(radius, 1.0)
    fill.size = max(radius * 2.5, 3.0)
    fill_obj = bpy.data.objects.new('Fill', fill)
    bpy.context.collection.objects.link(fill_obj)
    fill_obj.location = center + Vector((-1.6, -1.4, 1.2)).normalized() * max(radius * 2.4, 6)
    fill_obj.rotation_euler = (center - fill_obj.location).to_track_quat('-Z', 'Y').to_euler()


def render_asset(glb_name):
    path = EXPORT_DIRS[glb_name] / glb_name
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.gltf(filepath=str(path))
    meshes = [o for o in bpy.data.objects if o.type == 'MESH']
    tris = sum(max(len(p.vertices) - 2, 1) for o in meshes for p in o.data.polygons)
    low = Vector((math.inf,) * 3)
    high = Vector((-math.inf,) * 3)
    for obj in meshes:
        for corner in obj.bound_box:
            point = obj.matrix_world @ Vector(corner)
            low = Vector(map(min, low, point))
            high = Vector(map(max, high, point))
    center = (low + high) * 0.5
    radius = max((high - low).length * 0.5, 1.0)
    setup_world_and_lights(center, radius)
    camera_data = bpy.data.cameras.new('PreviewCamera')
    camera = bpy.data.objects.new('PreviewCamera', camera_data)
    bpy.context.collection.objects.link(camera)
    bpy.context.scene.camera = camera
    scene = bpy.context.scene
    scene.render.engine = 'CYCLES'
    scene.cycles.device = 'CPU'
    scene.cycles.samples = 48
    scene.cycles.use_denoising = False
    scene.cycles.max_bounces = 4
    scene.render.resolution_x = 1200
    scene.render.resolution_y = 780
    scene.view_settings.view_transform = 'Filmic'
    print(f'IMPORTED {glb_name} tris={tris} bounds={[round(v, 2) for v in (high - low)]}',
          flush=True)
    return camera, scene, meshes, tris


def main():
    results = {}
    for glb_name, views in JOBS:
        camera, scene, meshes, tris = render_asset(glb_name)
        for title, location, target, lens in views:
            camera.data.lens = lens
            camera.location = location
            camera.rotation_euler = (Vector(target) - Vector(location)).to_track_quat(
                '-Z', 'Y').to_euler()
            scene.render.filepath = str(OUT / f'{title}.png')
            bpy.ops.render.render(write_still=True)
            print('RENDERED', title, flush=True)
        results[glb_name] = {'tris': tris}
    print('PREVIEWS_DONE')


if __name__ == '__main__':
    main()
