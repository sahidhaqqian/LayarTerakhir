"""Extract authored architectural/bamboo modules without creating geometry or UVs.
blender -b -t 2 --python Tools/prepare_phase4_modules.py
"""
import sys,json
from pathlib import Path
import bpy
from mathutils import Vector
ROOT=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(ROOT/'Tools'))
import import_phase2_assets as c
c.OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase4/Imported'
rows=[]
def load(name):
 c.reset();bpy.ops.import_scene.gltf(filepath=str(ROOT/'Builds/ArtSources/Phase4/Models'/(name+'.glb')))
 for m in list(bpy.data.materials):m.name='P4_'+name+'_'+m.name
 bpy.context.view_layer.update()
def bounds(o):
 p=[o.matrix_world@Vector(v) for v in o.bound_box];return [min(v[i] for v in p) for i in range(3)],[max(v[i] for v in p) for i in range(3)]
def export(name,objects,source):
 if not objects:raise ValueError('Empty source selection '+name)
 c.export(name,objects,'Modules');r=c.MODELS[-1];r['sourceId']=source;r['operation']='Selection of licensed original geometry, transforms baked; no modeled geometry';rows.append(r)
load('Joglo')
meshes=[o for o in bpy.data.objects if o.type=='MESH']
raw_bounds=bounds
spatial={o.name:raw_bounds(o) for o in meshes}
def bounds(o):return spatial.get(o.name) or raw_bounds(o)
def within(o,x0,x1,y0,y1):
 lo,hi=bounds(o);return lo[0]>=x0-.15 and hi[0]<=x1+.15 and lo[1]>=y0-.15 and hi[1]<=y1+.15
export('JogloPendopo',[o for o in meshes if within(o,.4,6.3,-1.1,3.6) and any('Roof_Tiles' in m.name or 'Wood___' in m.name for m in o.data.materials)],'Joglo')
export('JogloHouse',[o for o in meshes if within(o,.4,6.3,4.1,10.4)],'Joglo')
export('JogloRoof',[o for o in meshes if any('Roof_Tiles' in m.name for m in o.data.materials) and within(o,.4,6.3,-1.1,3.6)],'Joglo')
flat=[]
for o in meshes:
 lo,hi=bounds(o)
 if hi[2]-lo[2]<.2 and hi[0]-lo[0]>2 and hi[1]-lo[1]>2:flat.append(o)
flat.sort(key=lambda o:(bounds(o)[1][0]-bounds(o)[0][0])*(bounds(o)[1][1]-bounds(o)[0][1]),reverse=True)
export('JogloFloor',flat[:1],'Joglo')
vertical=[o for o in meshes if any('Brick' in m.name for m in o.data.materials) and max(bounds(o)[1][i]-bounds(o)[0][i] for i in [0,1])>.5]
export('JogloWall',vertical[:1],'Joglo')
posts=[o for o in meshes if any('Paint___Light_Gray' in m.name for m in o.data.materials) and bounds(o)[1][2]-bounds(o)[0][2]>.7 and max(bounds(o)[1][i]-bounds(o)[0][i] for i in [0,1])<.15]
export('JogloPost',posts[:1],'Joglo')
export('JogloGate',[o for o in meshes if within(o,-1.8,.1,-4.8,-3.4)],'Joglo')
load('BambooSet')
for name,names in [('BambooA',['Object_15','Object_16']),('BambooB',['Object_33','Object_34','Object_35']),('BambooC',['Object_38','Object_39','Object_40']),('BambooD',['Object_50','Object_51','Object_52'])]:
 export(name,[bpy.data.objects[n] for n in names if n in bpy.data.objects],'BambooSet')
load('StoneWell')
# Exclude the creator's display ground, loose preview cable and suspended sphere;
# retain the actual authored well wall, roof, posts, shaft, water and bucket.
well=[o for o in bpy.data.objects if o.type=='MESH' and any(any(k in m.name for k in ['Brick','Roof','Wood_Sides','Water','material_7','Bucket']) for m in o.data.materials)]
export('StoneWellAssembly',well,'StoneWell')
(ROOT/'Audit/phase4-module-manifest.json').write_text(json.dumps(rows,indent=2)+'\n')
print('PHASE4_MODULES',len(rows))
