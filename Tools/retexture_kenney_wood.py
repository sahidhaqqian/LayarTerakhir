"""Re-unwrap CC0 Kenney wood pieces for tiling PBR rather than palette colours."""
import bpy,json
from pathlib import Path
root=Path(__file__).resolve().parents[1];source=Path('/tmp/kos-asset-sources/Kenney/Models/OBJ format');dest=root/'Assets/_LayarTerakhir/assets/Art/Imported/Kenney'
for name in ['bookcaseClosedDoors','cabinetBedDrawer','doorwayFront','wallWindow']:
 bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
 bpy.ops.wm.obj_import(filepath=str(source/(name+'.obj')),forward_axis='NEGATIVE_Z',up_axis='Y')
 for obj in list(bpy.context.scene.objects):
  if obj.type!='MESH':continue
  bpy.ops.object.select_all(action='DESELECT');obj.select_set(True);bpy.context.view_layer.objects.active=obj
  bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.uv.smart_project(angle_limit=1.1,island_margin=.02);bpy.ops.object.mode_set(mode='OBJECT')
 bpy.ops.object.select_all(action='SELECT')
 bpy.ops.export_scene.fbx(filepath=str(dest/(name+'.fbx')),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)
 print('KENNEY_WOOD_REUNWRAPPED',name)
