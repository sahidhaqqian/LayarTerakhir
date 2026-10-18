"""Budget the selected CC0 props and adapt downloaded house meshes for Melati.

Creates game meshes and UVs, not bitmap artwork. Existing house PBR textures
are reused by the Unity material pass. The original downloads remain intact.
"""
from pathlib import Path
import hashlib
import json
import math
import random
import runpy
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/Review'
DEST = ROOT / 'Assets/_LayarTerakhir/assets/Art/Review/Models'
DEST.mkdir(parents=True, exist_ok=True)
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


for source, name, budget in [('painted_wooden_sofa','AtmajaFamilySofa',4000),
                            ('street_rat','DeadStreetRat',900),
                            ('food_apple_01','RottenApple',450),('hamburger_buns','MoldyBread',650)]:
    clean();bpy.ops.import_scene.fbx(filepath=str(CACHE/source/(source+'.fbx')))
    total = sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in meshes())
    for o in meshes():
        bpy.context.view_layer.objects.active=o
        if total>budget:
            modifier=o.modifiers.new('Game mesh budget','DECIMATE');modifier.ratio=budget/total
            bpy.ops.object.modifier_apply(modifier=modifier.name)
        o.data.materials.clear();o.data.materials.append(material('Review_'+source,(.55,.58,.45)))
        for face in o.data.polygons:face.material_index=0
        if source=='street_rat':o.rotation_euler.y+=math.radians(78)
    export(name,'https://polyhaven.com/a/'+source)

# Raka's material-grouped model needs explicit geometry-authored contact markers.
records.append(runpy.run_path(str(ROOT/'Tools/author_raka_scooter.py'))['record'])

# Scene-only LOD variants leave every imported original and approved Kos scene intact.
for source,name,ratio in [
    (ROOT/'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/desk_lamp_arm_01/desk_lamp_arm_01.fbx','LodDeskLamp',.25),
    (ROOT/'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/wooden_bookshelf_worn/wooden_bookshelf_worn.fbx','LodBookshelf',.40),
    (ROOT/'Assets/_LayarTerakhir/assets/Art/Atmaja/Models/shrub_03.fbx','LodShrub',.60)]:
    clean();bpy.ops.import_scene.fbx(filepath=str(source))
    for obj in meshes():
        bpy.context.view_layer.objects.active=obj
        modifier=obj.modifiers.new('Scene review LOD','DECIMATE');modifier.ratio=ratio
        bpy.ops.object.modifier_apply(modifier=modifier.name);obj.data.name=obj.name
    export(name,str(source.relative_to(ROOT)))

# Deep-rim ceramic dinner plate. The underside, glaze cracks and mold patches are physical geometry.
clean();random.seed(1998)
porcelain=material('Review_Ceramic',(.58,.57,.43));mold=material('Review_Mold',(.075,.105,.045));crack=material('Review_Crack',(.13,.12,.075))
profile=[(0,.008),(.08,.008),(.105,.012),(.126,.029),(.146,.035),(.149,.031),(.143,.022),(.123,.012),(.102,0),(.07,-.005),(0,-.005)]
v=[];f=[]
for r,z in profile:
    for i in range(24):a=i*math.tau/24;v.append((r*math.cos(a),r*math.sin(a),z))
for j in range(len(profile)-1):
    for i in range(24):a=j*24+i;b=j*24+(i+1)%24;f.append((a,b,b+24,a+24))
mesh=bpy.data.meshes.new('Ceramic revolved profile');mesh.from_pydata(v,[],f);mesh.materials.append(porcelain);mesh.materials.append(mold);mesh.materials.append(crack)
plate=bpy.data.objects.new('MoldyCeramicPlate',mesh);bpy.context.collection.objects.link(plate)
for i in range(14):
    a=random.random()*math.tau;r=random.uniform(.06,.126);z=.010+max(0,r-.10)*.5
    bpy.ops.mesh.primitive_uv_sphere_add(segments=6,ring_count=3,location=(math.cos(a)*r,math.sin(a)*r,z+.001))
    patch=bpy.context.object;patch.name='Fungal colony';patch.scale=(random.uniform(.006,.019),random.uniform(.005,.014),.0012);patch.data.materials.append(mold)
for i in range(4):
    a=random.random()*math.tau;points=[Vector((math.cos(a)*r,math.sin(a)*r,.01+max(0,r-.10)*.5+.0015)) for r in [.07,.10,.12,.14]]
    curve=bpy.data.curves.new('Glaze fracture','CURVE');curve.dimensions='3D';curve.bevel_depth=.00055;curve.bevel_resolution=0
    spline=curve.splines.new('POLY');spline.points.add(len(points)-1)
    for p,co in zip(spline.points,points):p.co=(*co,1)
    obj=bpy.data.objects.new('Cracked glaze',curve);bpy.context.collection.objects.link(obj);curve.materials.append(crack)
    bpy.context.view_layer.objects.active=obj;obj.select_set(True);bpy.ops.object.convert(target='MESH');obj.select_set(False)
export('MoldyCeramicPlate','Original revolved ceramic profile, cracks and mold geometry')

# Varied downloaded buildings receive meter-based UVs and restrained material regions.
for letter in ['a','b','d','f','h','k','m','p']:
    clean();source=CACHE/'KenneySuburban/Models/GLB format'/('building-type-'+letter+'.glb')
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

(ROOT/'Audit/designer-review-mesh-authoring.json').write_text(json.dumps(dict(meshes=records,houseAdaptation='Kenney CC0 geometry; tropical muted PBR regions with meter-based UVs, no original palette texture'),indent=2)+'\n')
