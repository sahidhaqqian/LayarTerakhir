"""Read newly imported source parts/bounds for safe module selection; no edits."""
import bpy
from pathlib import Path
from mathutils import Vector
ROOT=Path(__file__).resolve().parents[1]
for name in ['modular_fort_01','modular_urban_apartments_facade','MarketStall','ScreenRobe','WitnessHands','FerrisWheel']:
 bpy.ops.wm.read_factory_settings(use_empty=True)
 bpy.ops.import_scene.fbx(filepath=str(ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Models'/(name+'.fbx')))
 print('SOURCE',name,flush=True)
 for o in bpy.data.objects:
  if o.type!='MESH':continue
  p=[o.matrix_world@Vector(v) for v in o.bound_box];lo=[min(v[i] for v in p) for i in range(3)];hi=[max(v[i] for v in p) for i in range(3)]
  print(o.name,'size',[round(hi[i]-lo[i],3) for i in range(3)],'min',[round(v,3) for v in lo], 'mats',[m.name for m in o.data.materials],flush=True)
