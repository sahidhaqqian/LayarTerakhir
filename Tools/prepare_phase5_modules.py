"""Select original licensed panels/props for level assembly; no new geometry."""
import bpy,json,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1];sys.path.insert(0,str(ROOT/'Tools'))
import import_phase2_assets as c
c.OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5/Imported';rows=[]
def load(source):
 c.reset();bpy.ops.import_scene.fbx(filepath=str(c.OUT/'Models'/(source+'.fbx')))
def select(stem):
 return [o for o in bpy.data.objects if o.type=='MESH' and (o.name==stem or o.name.startswith(stem+'.'))]
def export(name,objects,source,rotation=0):
 if not objects:raise ValueError('Missing original source part '+name)
 c.export(name,objects,'Modules',rotation);r=c.MODELS[-1];r.update(sourceId=source,operation='Selection/rotation/recentering of original licensed geometry; no authored vertices, UVs or textures');rows.append(r)
load('modular_urban_apartments_facade')
export('FacadeWall',select('wall_standard_standard_01'),'modular_urban_apartments_facade')
export('FacadeFloor',select('wall_standard_standard_01'),'modular_urban_apartments_facade',-90)
export('FacadeWindow',select('window_centered_large_01'),'modular_urban_apartments_facade')
export('FacadeDoorFrame',select('wall_door_centered_large_01'),'modular_urban_apartments_facade')
export('FacadeDoor',select('door_centered_large_01'),'modular_urban_apartments_facade')
export('FacadePost',select('wall_pier_corner_01'),'modular_urban_apartments_facade')
load('modular_fort_01')
export('FortGate',select('modular_fort_01_wall_thin_gate_01'),'modular_fort_01')
load('modular_electric_cables')
export('StraightCable',select('cable_10cm'),'modular_electric_cables')
load('WitnessHands')
export('WitnessHand',select('Object_4')+select('Object_5'),'WitnessHands')
load('projector_screen')
# Keep the creator's complete screen frame for mirror/screen gameplay props.
export('MirrorScreen',[o for o in bpy.data.objects if o.type=='MESH'],'projector_screen')
for name, source in [('ChainlinkPanel','modular_chainlink_fence'),('AirductCircular','modular_airduct_circular_01'),('AirductRectangular','modular_airduct_rectangular_01'),('MetalGutter','modular_metal_gutter'),('PipeSteel','modular_pipes'),('PipePlastic','modular_pipes_plastic_01')]:
 load(source);meshes=[o for o in bpy.data.objects if o.type=='MESH']
 candidates=[o for o in meshes if 'straight' in o.name.lower() or 'panel' in o.name.lower() or 'fence_straight' in o.name.lower()] or meshes
 chosen=max(candidates,key=lambda o:len(o.data.polygons))
 export(name,[chosen],source)
(ROOT/'Audit/phase5-module-manifest.json').write_text(json.dumps(rows,indent=2)+'\n')
print('PHASE5_MODULES',len(rows))
