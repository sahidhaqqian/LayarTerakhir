"""Convert additional licensed meshes only; no original modeling/animation."""
import sys,json
from pathlib import Path
import bpy
sys.path.insert(0,str(Path(__file__).resolve().parent))
import import_phase2_assets as base
for name in ['Furnace','Rope']:
 base.reset();bpy.ops.import_scene.gltf(filepath=str(base.SRC/(name+'.glb')))
 for material in bpy.data.materials: material.name='Ritual_'+name+'_'+material.name
 base.extract_materials(base.OUT/'Ritual/Textures')
 base.export(name,list(bpy.data.objects),'Ritual/Models')
base.reset();bpy.ops.import_scene.gltf(filepath=str(base.SRC/'AutopsyLab.glb'))
for o in bpy.data.objects:
 if any(t in o.name for t in ['MedicalLight','Curtain']):print('SOURCE_PART',o.name,o.type)
# The cloth is the artist's existing curtain geometry, flattened/fitted at placement.
node=bpy.data.objects['MedicalPartition_Curtain00_6']
cloth=[node]+list(node.children_recursive)
head=bpy.data.objects['MedicalLight_Arm_11']
base.export('SurgicalHead',[head]+list(head.children_recursive),'Ritual/Models')
base.export('BurntCloth',cloth,'Ritual/Models')
(base.ROOT/'Audit/phase2-session2-import-manifest.json').write_text(json.dumps(dict(materials=base.MATERIALS,models=base.MODELS),indent=2)+'\n')
