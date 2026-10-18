"""Adapt four additional downloaded CC0 houses with meter UVs and existing PBR regions.
Run using Blender: blender -b --python Tools/author_melati_houses.py
"""
from pathlib import Path
import hashlib, json
import bpy
from mathutils import Vector
ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/MelatiHouses'
DEST = ROOT / 'Assets/_LayarTerakhir/assets/Art/Review/Models'
records = []
def clean():
    bpy.ops.wm.read_factory_settings(use_empty=True)


def material(name, color):
    m = bpy.data.materials.new(name); m.diffuse_color = (*color, 1)
    return m


def meshes(): return [o for o in bpy.context.scene.objects if o.type == 'MESH']


def export(name, source):
    bpy.ops.object.select_all(action='DESELECT')
    for o in meshes(): o.select_set(True)
    bpy.context.view_layer.objects.active = meshes()[0]
    path = DEST / (name + '.fbx')
    bpy.ops.export_scene.fbx(filepath=str(path), use_selection=True, object_types={'MESH'},
        axis_forward='-Z', axis_up='Y', bake_anim=False, add_leaf_bones=False, path_mode='STRIP')
    triangles = sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in meshes())
    records.append(dict(name=name, source=source, triangles=triangles,
        sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    print('REVIEW_MESH', name, triangles, flush=True)


# Varied downloaded buildings receive meter-based UVs and restrained material regions.
for letter in ['i','l','n','q']:
    clean();source=CACHE/'Source/Models/GLB format'/('building-type-'+letter+'.glb')
    bpy.ops.import_scene.gltf(filepath=str(source))
    objects=meshes();points=[o.matrix_world@v.co for o in objects for v in o.data.vertices]
    lo=Vector(tuple(min(p[i] for p in points) for i in range(3)));hi=Vector(tuple(max(p[i] for p in points) for i in range(3)))
    scale=7.2/max(hi.x-lo.x,hi.y-lo.y);center=(lo+hi)*.5
    mats=[material('Review_HouseWall',(.42,.43,.34)),material('Review_HouseRoof',(.22,.105,.065)),material('Review_HouseWood',(.15,.095,.055)),material('Review_HouseGlass',(.09,.12,.12)),material('Review_HouseConcrete',(.23,.24,.22))]
    for obj in objects:
        transform=obj.matrix_world.copy();old_materials=list(obj.data.materials);old_uv=obj.data.uv_layers.active
        image=next((n.image for m in old_materials if m and m.use_nodes for n in m.node_tree.nodes if n.type=='TEX_IMAGE' and n.image),None)
        pixels=list(image.pixels) if image else [];image_width=image.size[0] if image else 0
        regions=[]
        for face in obj.data.polygons:
            uv=old_uv.data[face.loop_indices[0]].uv if old_uv else (0,0)
            if image:
                x=min(image.size[0]-1,int(uv[0]*image.size[0]));y=min(image.size[1]-1,int(uv[1]*image.size[1]));offset=(y*image_width+x)*4;r,g,b=pixels[offset:offset+3]
            else:r,g,b=.5,.5,.4
            regions.append(3 if b>r*1.16 and b>g*.95 else 1 if r>g*1.35 and r>b*1.4 else 2 if r>g*1.1 and max(r,g,b)<.5 else 4 if max(r,g,b)<.23 else 0)
        for v in obj.data.vertices:
            p=transform@v.co;v.co=Vector(((p.x-center.x)*scale,(p.y-center.y)*scale,(p.z-lo.z)*scale))
        obj.matrix_world.identity();obj.data.materials.clear()
        for m in mats:obj.data.materials.append(m)
        obj.data.update();uv_layer=obj.data.uv_layers.active or obj.data.uv_layers.new()
        for face,region in zip(obj.data.polygons,regions):
            face.material_index=region;normal=face.normal;axis=max(range(3),key=lambda a:abs(normal[a]));axes=[a for a in range(3) if a!=axis]
            for loop in face.loop_indices:
                co=obj.data.vertices[obj.data.loops[loop].vertex_index].co;uv_layer.data[loop].uv=(co[axes[0]]*.65,co[axes[1]]*.65)
        obj.name='MelatiHouse_'+letter
    export('MelatiHouse_'+letter,'https://kenney.nl/assets/city-kit-suburban#building-type-'+letter)

(ROOT/'Audit/melati-house-authoring.json').write_text(json.dumps(records,indent=2)+'\n')
