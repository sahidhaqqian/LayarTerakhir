"""Convert licensed new Phase 3 source meshes, preserving artist UVs and animation.
No mesh primitives, new topology, painted textures, or authored motion are created.
"""
import sys,json,re
from pathlib import Path
import bpy
sys.path.insert(0,str(Path(__file__).resolve().parent))
import import_phase2_assets as conversion
ROOT=Path(__file__).resolve().parents[1]
SRC=ROOT/'Builds/ArtSources/Phase3';OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase3/Imported'
conversion.SRC=SRC;conversion.OUT=OUT
conversion.MATERIALS.clear();conversion.MODELS.clear()
for name in ['Wewe','Sack','IndustrialBuilding','Doll','Boiler','Sickle','Locker','Cloth','MineCart','Shoe','Chimneys','Gear']:
 conversion.reset();bpy.ops.import_scene.gltf(filepath=str(SRC/(name+'.glb')))
 for material in list(bpy.data.materials):material.name='P3_'+name+'_'+material.name
 conversion.extract_materials(OUT/'Converted/Textures')
 if name=='Wewe':
  for helper in list(bpy.data.objects):
   if helper.type=='MESH' and helper.name=='Icosphere' and not helper.modifiers and not helper.data.materials:bpy.data.objects.remove(helper,do_unlink=True)
  meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
  bpy.ops.object.select_all(action='DESELECT')
  for o in meshes:o.select_set(True)
  bpy.context.view_layer.objects.active=meshes[0]
  bpy.ops.object.join()
  # Preserve the source artist's complete rig and its supplied Mixamo motion.
  p=OUT/'Converted/Models/Wewe.fbx';p.parent.mkdir(parents=True,exist_ok=True)
  bpy.ops.export_scene.fbx(filepath=str(p),object_types={'MESH','ARMATURE','EMPTY'},bake_anim=True,bake_anim_use_all_actions=True,add_leaf_bones=False,axis_forward='-Z',axis_up='Y')
  conversion.MODELS.append(dict(name=name,path=str(p.relative_to(ROOT)),clips=[a.name for a in bpy.data.actions],source='artist rig/animation; no new clip',removedSourceHelpers=['unskinned materialless Icosphere']))
 else:
  if name in ['Boiler','IndustrialBuilding']:
   for obj in list(bpy.data.objects):
    if obj.type=='MESH' and len(obj.data.polygons)>25000:
     dec=obj.modifiers.new('Licensed mesh optimization','DECIMATE');dec.ratio=25000/len(obj.data.polygons)
     bpy.context.view_layer.objects.active=obj;bpy.ops.object.modifier_apply(modifier=dec.name)
  conversion.export(name,list(bpy.data.objects),'Converted/Models')
modules=[]
for group in ['modular_factory_facade','modular_fire_escape','modular_wooden_pier','modular_industrial_pipes_01','overhead_crane']:
 conversion.reset();bpy.ops.import_scene.fbx(filepath=str(OUT/'PolyHaven'/group/(group+'.fbx')))
 for obj in [o for o in bpy.data.objects if o.type=='MESH']:
  name=re.sub('[^A-Za-z0-9_-]','_',obj.name)
  if group=='modular_wooden_pier' and name=='modular_wooden_pier_planks' or group=='modular_fire_escape' and name=='modular_fire_escape_platform_middle':
   tris=sum(len(p.vertices)-2 for p in obj.data.polygons)
   target=500 if group=='modular_wooden_pier' else 192
   if tris>target:
    dec=obj.modifiers.new('Floor repeat optimization preserving source UVs','DECIMATE');dec.ratio=target/tris
    bpy.context.view_layer.objects.active=obj;bpy.ops.object.modifier_apply(modifier=dec.name)
  conversion.export(name,[obj],'Modules/'+group)
  modules.append(dict(group=group,name=name,materials=[m.name for m in obj.data.materials],bounds=conversion.MODELS[-1]['bounds']))
report=dict(models=conversion.MODELS,materials=conversion.MATERIALS,modules=modules)
(ROOT/'Audit/phase3-import-manifest.json').write_text(json.dumps(report,indent=2)+'\n')
print('PHASE3_IMPORT_COMPLETE',len(modules),'source modules',flush=True)
