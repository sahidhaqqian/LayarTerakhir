"""Convert downloaded licensed geometry; preserve UVs, never model new art.

blender -b -t 2 --python Tools/import_phase4_assets.py
Original high-resolution files remain in the download cache. Derived FBX files
use bounded polygon counts; material provenance remains tied to each source.
"""
import hashlib
import json
import re
import sys
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / 'Tools'))
import import_phase2_assets as conversion

CACHE = ROOT / 'Builds/ArtSources/Phase4'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase4/Imported'
conversion.SRC, conversion.OUT = CACHE, OUT
conversion.MODELS.clear()
conversion.MATERIALS.clear()
source_manifest = json.loads((ROOT / 'Audit/phase4-asset-sources.json').read_text())
only_heroes = '--heroes' in sys.argv or '--only-id' in sys.argv
selected = {row['id'] for row in source_manifest['sources'] if row.get('sourceUid')} if only_heroes else None
if '--only-id' in sys.argv:
    selected = {sys.argv[sys.argv.index('--only-id') + 1]}
previous_path = ROOT / 'Audit/phase4-import-manifest.json'
previous = json.loads(previous_path.read_text()) if only_heroes and previous_path.exists() else {}
records = [r for r in previous.get('models', []) if r['sourceId'] not in selected] if only_heroes else []
materials = [r for r in previous.get('materials', []) if r['sourceId'] not in selected] if only_heroes else []
failures = []


def compact(value):
    return re.sub('[^a-z0-9]', '', value.lower().replace('_2k', '').replace('_1k', ''))


def material_records(source):
    files = [f for f in source['files'] if f['role'] == 'texture']
    for mat in list(bpy.data.materials):
        original = mat.name
        mat.name = 'P4_' + source['id'] + '_' + original
        bsdf = next((n for n in mat.node_tree.nodes if n.type == 'BSDF_PRINCIPLED'), None) if mat.use_nodes else None
        row = dict(name=mat.name, original=original, sourceId=source['id'],
                   color=list(bsdf.inputs['Base Color'].default_value) if bsdf else list(mat.diffuse_color),
                   roughness=float(bsdf.inputs['Roughness'].default_value) if bsdf else .65,
                   metallic=float(bsdf.inputs['Metallic'].default_value) if bsdf else 0,
                   transparent=mat.blend_method == 'BLEND' and 'glass' in original.lower())
        for label, slot, suffixes in [('Albedo', 'Base Color', ['diff', 'diffuse']), ('Normal', 'Normal', ['nor_gl']),
                                      ('Arm', 'Roughness', ['arm']), ('Alpha', 'Alpha', ['alpha'])]:
            choices = [f for f in files if any(f['channel'].lower() == s or f['channel'].lower().endswith('_' + s) for s in suffixes)]
            image = conversion.upstream(bsdf.inputs[slot]) if bsdf else None
            selected = next((f for f in choices if image and compact(Path(f['path']).stem) == compact(Path(image.filepath).stem)), None)
            if not selected and len(choices) == 1:
                selected = choices[0]
            if not selected and choices:
                ranked = sorted(choices, key=lambda f: len(set(compact(original).split()).intersection(compact(f['channel']).split())), reverse=True)
                selected = next((f for f in ranked if compact(original) in compact(f['channel']) or compact(f['channel'].rsplit('_', 1)[0]) in compact(original)), None)
            if selected:
                row[label] = selected['path']
        materials.append(row)


for source in source_manifest['sources']:
    if source['kind'] not in ['model', 'character']:
        continue
    if only_heroes and source['id'] not in selected:
        continue
    model = next((f for f in source['files'] if f['role'] == 'source-model'), None)
    if not model:
        continue
    path = ROOT / model['path']
    try:
        conversion.reset()
        if path.suffix == '.fbx':
            bpy.ops.import_scene.fbx(filepath=str(path))
            bpy.ops.file.find_missing_files(directory=str(path.parent))
        else:
            bpy.ops.import_scene.gltf(filepath=str(path))
            if source['id'] in ['Joglo', 'Robe', 'HumanSkeleton']:
                # Separate original material groups only; no sculpting or new topology.
                for obj in list(bpy.data.objects):
                    if obj.type != 'MESH' or len(obj.data.materials) < 2:
                        continue
                    bpy.ops.object.select_all(action='DESELECT')
                    obj.select_set(True)
                    bpy.context.view_layer.objects.active = obj
                    bpy.ops.object.mode_set(mode='EDIT')
                    bpy.ops.mesh.select_all(action='SELECT')
                    bpy.ops.mesh.separate(type='MATERIAL')
                    bpy.ops.object.mode_set(mode='OBJECT')
        source_meshes = [o for o in bpy.data.objects if o.type == 'MESH']
        if not source_meshes:
            raise ValueError('Source has no meshes')
        if path.suffix == '.glb':
            for mat in list(bpy.data.materials):
                mat.name = 'P4_' + source['id'] + '_' + mat.name
            start = len(conversion.MATERIALS)
            conversion.extract_materials(OUT / 'Textures' / source['id'])
            for item in conversion.MATERIALS[start:]:
                item['sourceId'] = source['id']
                materials.append(item)
        else:
            material_records(source)
        before = sum(sum(len(p.vertices) - 2 for p in obj.data.polygons) for obj in source_meshes)
        for obj in source_meshes:
            triangles = sum(len(p.vertices) - 2 for p in obj.data.polygons)
            ceiling = 40000 if source.get('sourceUid') else 18000
            if triangles > ceiling:
                modifier = obj.modifiers.new('Derived LOD preserving licensed source UVs', 'DECIMATE')
                modifier.ratio = ceiling / triangles
                bpy.context.view_layer.objects.active = obj
                bpy.ops.object.modifier_apply(modifier=modifier.name)
        after = sum(sum(len(p.vertices) - 2 for p in obj.data.polygons) for obj in source_meshes)
        conversion.export(source['id'], source_meshes, 'Models')
        exported = conversion.MODELS[-1]
        target = ROOT / exported['path']
        row = dict(sourceId=source['id'], sourceSha256=model['sha256'], path=exported['path'],
                   sha256=hashlib.file_digest(target.open('rb'), 'sha256').hexdigest(),
                   sourceTriangles=before, derivedTriangles=after, bounds=exported['bounds'],
                   sourceMeshes=exported['sourceMeshes'], sourcePage=source['sourcePage'])
        if source.get('sourceUid'):
            row['parts'] = []
            for index, obj in enumerate(source_meshes):
                if not obj.data.polygons:
                    continue
                name = source['id'] + '_part_' + str(index)
                conversion.export(name, [obj], 'Parts')
                part = dict(conversion.MODELS[-1])
                part['materials'] = [m.name for m in obj.data.materials]
                part['offset'] = list(obj.matrix_world.translation)
                row['parts'].append(part)
        # Complete natural clumps in these source catalogues may be placed separately.
        if source['id'].startswith(('grass_medium_', 'rock_moss_set_')) and len(source_meshes) > 1:
            row['variants'] = []
            for index, obj in enumerate(source_meshes):
                if not obj.data.polygons:
                    continue
                name = source['id'] + '_part_' + str(index)
                conversion.export(name, [obj], 'Variants')
                row['variants'].append(conversion.MODELS[-1])
        records.append(row)
        print('PHASE4_IMPORTED', source['id'], before, '->', after, exported['bounds'], flush=True)
    except Exception as error:
        failures.append(dict(id=source['id'], error=str(error)))
        print('PHASE4_IMPORT_FAILED', source['id'], str(error), flush=True)

(ROOT / 'Audit/phase4-import-manifest.json').write_text(json.dumps(dict(models=records, materials=materials, failures=failures), indent=2) + '\n')
print('PHASE4_IMPORT_COMPLETE', len(records), 'models', len(failures), 'failures', flush=True)
