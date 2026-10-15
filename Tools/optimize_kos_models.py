import bpy,json,sys
from pathlib import Path
from mathutils import Vector
ROOT=Path('/tmp/kos-asset-sources')
summary=[]
def process(source,target,limit=0):
 bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
 bpy.ops.import_scene.fbx(filepath=str(source))
 objects=[o for o in bpy.context.scene.objects if o.type=='MESH']
 before=0;after=0
 for o in objects:
  bpy.context.view_layer.objects.active=o
  t=sum(len(p.vertices)-2 for p in o.data.polygons);before+=t
  if limit and t>limit:
   m=o.modifiers.new('IntegratedGPU_Reduction','DECIMATE');m.ratio=max(.05,limit/t)
   bpy.ops.object.modifier_apply(modifier=m.name)
  after+=sum(len(p.vertices)-2 for p in o.data.polygons)
 bounds=[o.matrix_world@Vector(v) for o in objects for v in o.bound_box]
 dims=[max(v[i] for v in bounds)-min(v[i] for v in bounds) for i in range(3)]
 mats=sorted({m.name for o in objects for m in o.data.materials if m})
 summary.append({'file':source.name,'beforeTriangles':before,'afterTriangles':after,'dimensionsBlender':dims,'materials':mats,'objects':[o.name for o in objects]})
 if target:
  target.parent.mkdir(parents=True,exist_ok=True)
  bpy.ops.export_scene.fbx(filepath=str(target),use_selection=False,object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)
for i,limit in [('plastic_bottle_gallon',2400),('desk_lamp_arm_01',6500),('binder_notebook',3500)]:
 process(ROOT/'PolyHaven'/i/(i+'.fbx'),ROOT/'optimized'/(i+'.fbx'),limit)
for i in ['vintage_day_bed','wooden_table_02','painted_wooden_chair_01','painted_wooden_cabinet_02','wooden_bookshelf_worn']:
 process(ROOT/'PolyHaven'/i/(i+'.fbx'),None)
(ROOT/'model-processing.json').write_text(json.dumps(summary,indent=2))
print('MODEL_PROCESSING_COMPLETE',json.dumps([{k:v for k,v in i.items() if k not in ['objects','materials']} for i in summary]))
