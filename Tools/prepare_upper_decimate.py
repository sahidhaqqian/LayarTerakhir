"""Cap per-mesh polycounts on staged PolyHaven props and our own heavy originals so the
two-story Atmaja scene fits the integrated-GPU triangle budget. Caps are absolute, so the
script converges: re-running only cuts meshes still above their cap.
Keeps material slots so the Unity builder can re-texture per slot.
blender -b --python Tools/prepare_upper_decimate.py
"""
import bpy
from pathlib import Path
ROOT = Path(__file__).resolve().parents[1]
POLYHAVEN = ROOT / 'Assets/_LayarTerakhir/Art/Imported/PolyHaven'
ORIGINALS = ROOT / 'Assets/_LayarTerakhir/Art/Original/Models'
CAPS = {
    'book_encyclopedia_set_01': 120,
    'wooden_candlestick': 2500,
    'old_bed_frame': 1200,
    'metal_tool_chest': 400,
    'wooden_crate_01': 400,
    'wooden_crate_02': 400,
    'Chandelier_01': 800,
    'Lantern_01': 500,
    'vintage_cabinet_01': 2500,
    'vintage_video_camera': 800,
    'hanging_picture_frame_03': 600,
    'fancy_picture_frame_02': 300,
    'pot_enamel_01': 900,
    'ornate_mirror_01': 900,
    'steel_frame_shelves_02': 500,
    'vintage_oil_lamp': 700,
    'throw_pillows_01': 400,
    'wooden_stool_01': 500,
    'Rockingchair_01': 500,
    'painted_wooden_shelves': 300,
    'painted_wooden_nightstand': 300,
    'vintage_wooden_drawer_01': 800,
    'alarm_clock_01': 800,
    'hanging_picture_frame_01': 600,
    'fancy_picture_frame_01': 600,
}
ORIGINAL_CAPS = {
    'UpperBathtub': 900,
    'UpperSinkPedestal': 600,
    'UpperStandingMirror': 900,
    'UpperStandingNyonya': 1500,
    'UpperHangedNyonya': 1500,
    'UpperShearsRust': 400,
    'UpperSewingMachine': 1200,
    'UpperSewingBench': 700,
    'UpperHangingRag': 400,
    'UpperTrapdoorSlab': 300,
}


def cap_fbx(path, cap):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(path))
    for o in list(bpy.context.scene.objects):
        if o.type != 'MESH':
            continue
        if o.name == 'Cube' and len(o.data.vertices) == 8:
            bpy.data.objects.remove(o, do_unlink=True)
            continue
        count = len(o.data.polygons)
        if count > cap:
            bpy.ops.object.select_all(action='DESELECT')
            o.select_set(True)
            bpy.context.view_layer.objects.active = o
            mod = o.modifiers.new('IrisXe cap', 'DECIMATE')
            mod.ratio = cap / count
            bpy.ops.object.modifier_apply(modifier=mod.name)
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.fbx(filepath=str(path), use_selection=True,
                              object_types={'MESH'}, bake_anim=False, axis_forward='-Z', axis_up='Y',
                              path_mode='STRIP')
    tris = sum(len(o.data.polygons) for o in bpy.context.scene.objects if o.type == 'MESH')
    return tris


for asset_id, cap in CAPS.items():
    fbx = POLYHAVEN / asset_id / (asset_id + '.fbx')
    if not fbx.exists():
        print('SKIP missing', asset_id)
        continue
    print('CAPPED', asset_id, 'tris=', cap_fbx(fbx, cap))
for asset_id, cap in ORIGINAL_CAPS.items():
    fbx = ORIGINALS / (asset_id + '.fbx')
    if not fbx.exists():
        print('SKIP missing', asset_id)
        continue
    print('CAPPED', asset_id, 'tris=', cap_fbx(fbx, cap))
print('UPPER_DECIMATE_DONE')
