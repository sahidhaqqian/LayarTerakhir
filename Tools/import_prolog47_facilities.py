"""Blender conversion of selected CC0 source meshes, keeping author geometry/UVs."""
from pathlib import Path
import bpy
import json
import hashlib

ROOT=Path(__file__).resolve().parents[1]
CACHE=ROOT/'Builds/ArtSources/KosRealWorld'
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities'
records=[]

def export(name,objects,page,author):
    bpy.ops.object.select_all(action='DESELECT')
    for obj in objects:obj.select_set(True)
    folder=OUT/name;folder.mkdir(parents=True,exist_ok=True);path=folder/(name+'.fbx')
    bpy.ops.export_scene.fbx(filepath=str(path),use_selection=True,object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='STRIP')
    colors={}
    for obj in objects:
        for mat in obj.data.materials:
            if not mat:continue
            color=list(mat.diffuse_color)
            if mat.use_nodes:
                node=next((n for n in mat.node_tree.nodes if n.type=='BSDF_PRINCIPLED'),None)
                if node:color=list(node.inputs['Base Color'].default_value)
            colors[mat.name]=color
    (folder/'palette.json').write_text(json.dumps(colors,indent=2)+'\n')
    records.append(dict(name=name,page=page,author=author,license='CC0-1.0',path=path.relative_to(ROOT).as_posix(),sha256=hashlib.sha256(path.read_bytes()).hexdigest(),materialColors=colors))

bpy.ops.wm.open_mainfile(filepath=str(CACHE/'oga-oven-and-fridge.blend'))
obj=bpy.data.objects.get('FridgeNoInterior')
assert obj is not None
export('Fridge',[obj],'https://opengameart.org/content/oven-and-fridge','DREAM_SEARCH_REPEAT')
bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.wm.obj_import(filepath=str(CACHE/'oga-bathroom/Shower.obj'))
objects=[o for o in bpy.context.scene.objects if o.type=='MESH']
export('Shower',objects,'https://opengameart.org/content/bathroom','DREAM_SEARCH_REPEAT')
(ROOT/'Audit/prolog47-converted-cc0-sources.json').write_text(json.dumps(records,indent=2)+'\n')
print('CONVERTED_CC0',len(records))
