"""Reduce clothing topology while retaining the original FBX animation/rig records.

blender -b -t 2 --python Tools/optimize_raka_geometry.py -- --output /tmp/Raka.fbx
Only the body Geometry and its skin-cluster index/weight arrays are transplanted.
The original file IDs, bone bind transforms, 16 clips, phone, and metadata survive.
"""
import argparse
import array
import hashlib
import json
from pathlib import Path
import sys
import tempfile

import bpy
import numpy as np
from io_scene_fbx import encode_bin, parse_fbx

ROOT = Path(__file__).resolve().parents[1]
MODEL = ROOT / 'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/RakaDesigner.fbx'
TARGET = 67200


def position(vertex):
    return tuple(round(float(x), 7) for x in vertex.co)


def protected_vertices(body):
    names = {g.index: g.name for g in body.vertex_groups}
    return {v.index for v in body.data.vertices if v.co.z > 1.36 or any(
        names[g.group].startswith(('Finger', 'Hand_', 'Foot_', 'Toes_'))
        and g.weight > .005 for g in v.groups)}


def reduce_body(body, target=TARGET):
    """Keep face/neck/finger geometry exact; collapse lower-detail clothing edges."""
    before = len(body.data.polygons)
    if before <= target:
        return dict(before=before, after=before, alreadyWithinBudget=True)
    protected = protected_vertices(body)
    protected_positions = {position(body.data.vertices[i]) for i in protected}
    protected_faces = {tuple(sorted(position(body.data.vertices[i]) for i in face.vertices))
                       for face in body.data.polygons if all(i in protected for i in face.vertices)}
    body.data.calc_normals_split()
    def corners(face):
        uv = body.data.uv_layers.active.data
        return tuple(sorted((position(body.data.vertices[body.data.loops[i].vertex_index]),
                             tuple(round(float(v), 7) for v in uv[i].uv)) for i in face.loop_indices))
    protected_normals = {corners(face): {position(body.data.vertices[body.data.loops[i].vertex_index]):
                         tuple(body.data.loops[i].normal) for i in face.loop_indices}
                         for face in body.data.polygons if all(i in protected for i in face.vertices)}
    names = {g.index: g.name for g in body.vertex_groups}
    def weighted_position(vertex):
        return position(vertex), tuple(sorted((names[g.group], round(g.weight, 6))
                                              for g in vertex.groups if g.weight > 0))
    protected_weights = {weighted_position(body.data.vertices[i]) for i in protected}
    bpy.context.view_layer.objects.active = body
    for obj in bpy.context.selected_objects:
        obj.select_set(False)
    body.select_set(True)
    group = body.vertex_groups.new(name='BudgetProtected')
    group.add(list(protected), 1, 'REPLACE')
    modifier = body.modifiers.new('ClothingTopology', 'DECIMATE')
    modifier.ratio = target / before
    modifier.vertex_group = group.name
    modifier.vertex_group_factor = 1000
    modifier.invert_vertex_group = True
    modifier.use_collapse_triangulate = True
    # Apply in bind space, before the retained armature modifier.
    while body.modifiers.find(modifier.name) > 0:
        bpy.ops.object.modifier_move_up(modifier=modifier.name)
    bpy.ops.object.modifier_apply(modifier=modifier.name)
    body.vertex_groups.remove(body.vertex_groups['BudgetProtected'])
    remaining = {position(v) for v in body.data.vertices}
    faces = {tuple(sorted(position(body.data.vertices[i]) for i in face.vertices))
             for face in body.data.polygons}
    assert protected_positions <= remaining, 'Protected face/hand vertex moved'
    assert protected_faces <= faces, 'Protected face/hand triangle removed'
    assert protected_weights <= {weighted_position(v) for v in body.data.vertices}, 'Protected skinning changed'
    body.data.calc_normals_split()
    normals = [tuple(loop.normal) for loop in body.data.loops]
    matched = set()
    for face in body.data.polygons:
        signature = corners(face)
        if signature in protected_normals:
            matched.add(signature)
            for i in face.loop_indices:
                normals[i] = protected_normals[signature][position(body.data.vertices[body.data.loops[i].vertex_index])]
    assert set(protected_normals) <= matched, 'Protected UV mapping changed'
    body.data.normals_split_custom_set(normals)
    assert len(body.data.polygons) <= target
    return dict(before=before, after=len(body.data.polygons),
                protectedVertices=len(protected_positions), protectedFaces=len(protected_faces),
                protectedVerticesPreserved=True, protectedFacesPreserved=True,
                protectedUvsNormalsAndWeightsPreserved=True)


def child(element, name):
    return next(e for e in element.elems if e.id == name)


def objects(root):
    return {e.props[0]: e for e in child(root, b'Objects').elems}


def semantic_hash(element):
    digest = hashlib.sha256()
    def visit(e):
        digest.update(e.id + bytes(e.props_type))
        for prop in e.props:
            digest.update(prop.tobytes() if isinstance(prop, array.array)
                          else prop if isinstance(prop, bytes) else repr(prop).encode())
            digest.update(b'\0')
        for part in e.elems:
            visit(part)
        digest.update(b'END')
    visit(element)
    return digest.hexdigest()


def geometry(root):
    table = objects(root)
    model = next(e for e in table.values()
                 if e.id == b'Model' and e.props[1].split(b'\0')[0] == b'VisibleSkin')
    return next(table[e.props[1]] for e in child(root, b'Connections').elems
                if e.props[0] == b'OO' and e.props[2] == model.props[0]
                and table[e.props[1]].id == b'Geometry')


def clusters(root, geo):
    table = objects(root)
    links = [(e.props[1], e.props[2]) for e in child(root, b'Connections').elems
             if e.props[0] == b'OO']
    skin = next(a for a, b in links if b == geo.props[0] and table[a].id == b'Deformer')
    result = {}
    for cluster, parent in links:
        if parent == skin and table[cluster].props[-1] == b'Cluster':
            bone = next(table[a] for a, b in links if b == cluster and table[a].id == b'Model')
            result[bone.props[1]] = table[cluster]
    return result


def encoded(element):
    out = encode_bin.FBXElem(element.id)
    methods = dict(B='bool', C='char', Z='int8', Y='int16', I='int32', L='int64',
                   F='float32', D='float64', R='bytes', S='string', i='int32_array',
                   l='int64_array', f='float32_array', d='float64_array',
                   b='bool_array', c='byte_array')
    for kind, value in zip(element.props_type, element.props):
        getattr(out, 'add_' + methods[chr(kind)])(value)
    out.elems = [encoded(e) for e in element.elems]
    return out


def optimize_file(source, output, report_path, target=TARGET):
    original_bytes = source.read_bytes()
    original, version = parse_fbx.parse(str(source))
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(source))
    body = bpy.data.objects['VisibleSkin']
    protected_positions = {position(body.data.vertices[i]) for i in protected_vertices(body)}
    stats = reduce_body(body, target)
    for obj in bpy.context.scene.objects:
        if obj.type == 'ARMATURE':
            obj.data.pose_position = 'REST'
    with tempfile.TemporaryDirectory(prefix='raka-geometry-') as tmp:
        temp = Path(tmp) / 'geometry.fbx'
        bpy.ops.export_scene.fbx(filepath=str(temp), use_selection=False,
            object_types={'MESH', 'ARMATURE', 'EMPTY'}, axis_forward='-Z', axis_up='Y',
            bake_anim=False, add_leaf_bones=False, path_mode='STRIP')
        replacement, _ = parse_fbx.parse(str(temp))
    old_geo, new_geo = geometry(original), geometry(replacement)
    old_clusters, new_clusters = clusters(original, old_geo), clusters(replacement, new_geo)
    assert old_clusters.keys() == new_clusters.keys(), 'Skin bone set changed'
    # Match exported geometry to the original mesh coordinate system, while
    # retaining the original FBX bone bind matrices (Blender re-expresses them).
    vertices = np.asarray(child(new_geo, b'Vertices').props[0]).reshape(-1, 3)
    assert protected_positions <= {tuple(round(float(x), 7) for x in v) for v in vertices}
    bind_hashes = {name: [semantic_hash(e) for e in cluster.elems
                         if e.id not in [b'Indexes', b'Weights']]
                   for name, cluster in old_clusters.items()}
    changed_ids = {old_geo.props[0]} | {e.props[0] for e in old_clusters.values()}
    immutable = {ident: semantic_hash(e) for ident, e in objects(original).items()
                 if ident not in changed_ids}
    top_level = {e.id: semantic_hash(e) for e in original.elems if e.id != b'Objects'}
    old_geo.elems[:] = new_geo.elems
    for name, old in old_clusters.items():
        new = new_clusters[name]
        for prop in [b'Indexes', b'Weights']:
            previous = next((e for e in old.elems if e.id == prop), None)
            replacement = next((e for e in new.elems if e.id == prop), None)
            if previous is None and replacement is None:
                continue  # FBX also contains clusters for unweighted bones.
            if replacement is None:
                assert not previous.props[0], 'A formerly weighted bone lost every influence: ' + repr(name)
                continue
            if previous is None:
                old.elems.append(replacement)
            else:
                old.elems[old.elems.index(previous)] = replacement
    output.parent.mkdir(parents=True, exist_ok=True)
    encode_bin.write(str(output), encoded(original), version)
    verified, _ = parse_fbx.parse(str(output))
    verified_objects = objects(verified)
    assert all(semantic_hash(verified_objects[ident]) == digest for ident, digest in immutable.items())
    assert all(semantic_hash(child(verified, name)) == digest for name, digest in top_level.items())
    assert semantic_hash(geometry(verified)) == semantic_hash(old_geo)
    for name, cluster in clusters(verified, geometry(verified)).items():
        assert bind_hashes[name] == [semantic_hash(e) for e in cluster.elems
                                    if e.id not in [b'Indexes', b'Weights']]
    animation_records = [e for e in verified_objects.values() if e.id.startswith(b'Animation')]
    stats.update(source=str(source), sourceSha256=hashlib.sha256(original_bytes).hexdigest(),
        output=str(output), outputSha256=hashlib.sha256(output.read_bytes()).hexdigest(),
        preservedObjectRecords=len(immutable), preservedAnimationRecords=len(animation_records),
        animationAndRigRecordsUnchanged=True, originalFileIdsAndConnectionsUnchanged=True,
        phoneGeometryAndMaterialsUnchanged=True, originalBindTransformsUnchanged=True,
        skinClusters=len(old_clusters))
    report_path.write_text(json.dumps(stats, indent=2) + '\n')
    print('RAKA_TOPOLOGY', json.dumps(stats), flush=True)
    return stats


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--source', type=Path, default=MODEL)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--report', type=Path, required=True)
    parser.add_argument('--target', type=int, default=TARGET)
    args = parser.parse_args(sys.argv[sys.argv.index('--') + 1:])
    optimize_file(args.source, args.output, args.report, args.target)
