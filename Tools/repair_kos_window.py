"""Reuse the existing CC0 Kenney window, separating glass and removing its kit wall panel.
Run with Blender after the original Kenney Furniture Kit has been extracted to the source directory.
The original source ZIP and license/checksum are recorded in Audit/asset-credits.md.
"""
import bpy, bmesh
from pathlib import Path
ROOT = Path(__file__).resolve().parents[1]
SOURCE = Path('/tmp/kos-asset-sources/Kenney/Models/OBJ format/wallWindow.obj')
bpy.ops.object.select_all(action='SELECT'); bpy.ops.object.delete(use_global=False)
bpy.ops.wm.obj_import(filepath=str(SOURCE), forward_axis='NEGATIVE_Z', up_axis='Y')
for obj in list(bpy.context.scene.objects):
    if obj.type != 'MESH': continue
    bm = bmesh.new(); bm.from_mesh(obj.data)
    faces = [f for f in bm.faces if obj.data.materials[f.material_index].name.startswith('_defaultMat')]
    bmesh.ops.delete(bm, geom=faces, context='FACES')
    bm.to_mesh(obj.data); bm.free()
    bpy.ops.object.select_all(action='DESELECT');obj.select_set(True);bpy.context.view_layer.objects.active=obj
    bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.mesh.separate(type='MATERIAL');bpy.ops.object.mode_set(mode='OBJECT')
for obj in list(bpy.context.scene.objects):
    if obj.type!='MESH':continue
    bpy.ops.object.select_all(action='DESELECT');obj.select_set(True);bpy.context.view_layer.objects.active=obj
    bpy.ops.object.material_slot_remove_unused()
    obj.name='Window_'+obj.data.materials[0].name
    bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.uv.smart_project(angle_limit=1.1,island_margin=.02);bpy.ops.object.mode_set(mode='OBJECT')
bpy.ops.object.select_all(action='SELECT')
bpy.ops.export_scene.fbx(filepath=str(ROOT/'Assets/_LayarTerakhir/Art/Imported/Kenney/wallWindow.fbx'),
    object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)
print('WINDOW_REPAIRED',[(o.name,len(o.data.polygons)) for o in bpy.context.scene.objects if o.type=='MESH'])
