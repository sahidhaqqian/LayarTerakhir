"""Convert the existing CC0 weapon mesh. Environment uses import_phase1_environment_meshes.py.

blender -b --python Tools/import_phase1_modular.py
Original weapon topology/UVs are retained.
"""
import hashlib
import json
from pathlib import Path

import bpy
from mathutils import Matrix, Vector

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Imported/UnfinishedBuildings'
# Existing CC0 weapon mesh; conversion only, no editing of topology or UVs.
bpy.ops.wm.read_factory_settings(use_empty=True)
weapon_source = ROOT / 'Builds/ArtSources/Phase1External/MrLeapWeapons/obj/kris.obj'
bpy.ops.wm.obj_import(filepath=str(weapon_source))
weapon_out = OUT.parent / 'MrLeapWeapons'
weapon_out.mkdir(exist_ok=True)
bpy.ops.export_scene.fbx(filepath=str(weapon_out/'Kris.fbx'), use_selection=False, object_types={'MESH'}, axis_forward='-Z', axis_up='Y', bake_anim=False, add_leaf_bones=False, path_mode='STRIP')
