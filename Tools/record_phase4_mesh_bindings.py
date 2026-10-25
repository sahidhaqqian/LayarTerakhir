"""Preserve original downloaded FBX mesh/material slot associations for Unity.
Blender metadata inspection only; no geometry, texture or animation is authored.
"""
import bpy,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
sources=json.loads((ROOT/'Audit/phase4-asset-sources.json').read_text())['sources'];derived={r['sourceId']:r['path'] for r in json.loads((ROOT/'Audit/phase4-import-manifest.json').read_text())['models']};models=[]
for source in sources:
 f=next((f for f in source['files'] if f['role']=='source-model'),None)
 if not f or not f['path'].endswith('.fbx'):continue
 bpy.ops.wm.read_factory_settings(use_empty=True);bpy.ops.import_scene.fbx(filepath=str(ROOT/derived[source['id']]))
 meshes=[]
 for obj in bpy.data.objects:
  if obj.type=='MESH':meshes.append(dict(renderer=obj.name,mesh=obj.data.name,materials=[m.name for i,m in enumerate(obj.data.materials) if i in {p.material_index for p in obj.data.polygons}]))
 models.append(dict(sourceId=source['id'],meshes=meshes));print('P4_SOURCE_SLOTS',source['id'],len(meshes),flush=True)
(ROOT/'Audit/phase4-mesh-material-bindings.json').write_text(json.dumps(dict(models=models),indent=2)+'\n');print('P4_MESH_BINDINGS',len(models))
