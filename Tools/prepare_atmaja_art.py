"""Blender 4: reviewed staging -> optimized Unity FBX/material data, no downloads.
blender -b --python Tools/prepare_atmaja_art.py
Preserves both research drops. Repairs scooter wheels and opens the actual kitchen aperture.
"""
from pathlib import Path
import bpy, json, math, hashlib, shutil
from mathutils import Vector, Matrix

ROOT=Path(__file__).resolve().parents[1]
REC=ROOT/'Scene2_Atmaja_Recovery';STAGE=ROOT/'Scene2_Atmaja_Assets'
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Atmaja';OUT.mkdir(parents=True,exist_ok=True)
(OUT/'Models').mkdir(exist_ok=True);(OUT/'Textures').mkdir(exist_ok=True)
materials=[];models=[];sources=[]

def checked(path):
    path=Path(path);digest=hashlib.sha256(path.read_bytes()).hexdigest()
    sources.append(dict(path=str(path.relative_to(ROOT)),sha256=digest));return path

def source_texture(path):
    if not path or not Path(path).is_file():return ''
    path=checked(Path(path));dest=OUT/'Textures'/path.name
    if not dest.exists():shutil.copy2(path,dest)
    return str(dest.relative_to(ROOT))

def upstream_image(socket):
    visited=set()
    def visit(s):
        for link in s.links:
            n=link.from_node
            if n in visited:continue
            visited.add(n)
            if n.type=='TEX_IMAGE' and n.image:return n.image
            for i in n.inputs:
                found=visit(i)
                if found:return found
    return visit(socket)

def image_path(image):
    if image is None:return None
    p=Path(bpy.path.abspath(image.filepath))
    if p.is_file():return p
    # Recovery .blend files are source records; resolve relocated loose maps by exact name.
    matches=list(REC.rglob(p.name))
    return matches[0] if matches else None

def material_record(mat,prefix,override=None):
    name=prefix+'_'+mat.name.replace('.','_')
    bsdf=next((n for n in mat.node_tree.nodes if n.type=='BSDF_PRINCIPLED'),None) if mat.use_nodes else None
    color=list(bsdf.inputs['Base Color'].default_value) if bsdf else list(mat.diffuse_color)
    rough=float(bsdf.inputs['Roughness'].default_value) if bsdf else .8
    metal=float(bsdf.inputs['Metallic'].default_value) if bsdf else 0
    paths={}
    if bsdf:
        for key,slot in [('albedo','Base Color'),('normal','Normal'),('arm','Roughness')]:
            paths[key]=image_path(upstream_image(bsdf.inputs[slot]))
    if override:paths.update(override)
    alpha=any(x in mat.name.lower() for x in ['leaf','leaves','flower','foliage'])
    record=dict(name=name,color=color,roughness=rough,metallic=metal,alphaClip=alpha,
                albedo=source_texture(paths.get('albedo')),normal=source_texture(paths.get('normal')),
                arm=source_texture(paths.get('arm')))
    materials.append(record);mat.name=name;return name

def world_bounds(obj):
    p=[obj.matrix_world@Vector(v) for v in obj.bound_box]
    return [min(v[i] for v in p) for i in range(3)],[max(v[i] for v in p) for i in range(3)]

def export(name,ratio=1,house=False):
    groups={};hulls=[]
    for obj in list(bpy.context.scene.objects):
        if obj.type!='MESH':continue
        if house and (obj.name.startswith(('PLINTH','WALL','ANNEX_','VERANDA_platform','VERANDA_step')) or
                      (obj.name.startswith('WIN_') and obj.name.endswith('_glass')) or
                      (obj.name.startswith('DOOR_') and '_leaf_' in obj.name)):
            low,high=world_bounds(obj);hulls.append(dict(name=obj.name,low=low,high=high))
        if ratio<1 and len(obj.data.polygons)>600:
            bpy.context.view_layer.objects.active=obj
            m=obj.modifiers.new('IrisXe mesh reduction','DECIMATE');m.ratio=ratio;bpy.ops.object.modifier_apply(modifier=m.name)
        role='KitchenGlass' if house and (obj.name.startswith('KITCHEN_broken') or obj.name.startswith('WIN_kitchen_broken')) else 'Static'
        key=(role,obj.data.materials[0].name if obj.data.materials else 'Unassigned')
        groups.setdefault(key,[]).append(obj)
    for (role,mat),objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for obj in objects:obj.select_set(True)
        bpy.context.view_layer.objects.active=objects[0]
        if len(objects)>1:bpy.ops.object.join()
        bpy.context.object.name=role+'__'+mat
    if house:
        for h in hulls:
            marker=bpy.data.objects.new('COL_'+h['name'],None);bpy.context.collection.objects.link(marker)
            marker.location=[(a+b)/2 for a,b in zip(h['low'],h['high'])]
            marker.scale=[max(.001,b-a) for a,b in zip(h['low'],h['high'])]
        for n,p in [('Origin',(0,0,0)),('East',(1,0,0)),('North',(0,1,0)),('Up',(0,0,1))]:
            marker=bpy.data.objects.new('AXIS_'+n,None);bpy.context.collection.objects.link(marker);marker.location=p
    bpy.ops.object.select_all(action='SELECT')
    path=OUT/'Models'/(name+'.fbx')
    bpy.ops.export_scene.fbx(filepath=str(path),object_types={'MESH','EMPTY'},use_selection=True,bake_anim=False,
        axis_forward='-Z',axis_up='Y',add_leaf_bones=False,path_mode='STRIP',embed_textures=False)
    mesh=[o for o in bpy.context.scene.objects if o.type=='MESH']
    models.append(dict(name=name,path=str(path.relative_to(ROOT)),triangles=sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in mesh),
                       renderers=len(mesh),hulls=hulls,sourceCoordinateNote='Blender Z-up, front -Y; Unity import inspected separately'))

# House: source .blend retains full PBR links. No changes to the recovery source files.
bpy.ops.wm.open_mainfile(filepath=str(checked(REC/'01_House/Blender/RumahAtmaja_House.blend')))
for obj in list(bpy.context.scene.objects):
    if obj.name=='WIN_kitchen_broken_glass' or obj.name.startswith(('WIN_kitchen_broken_mullion','WIN_kitchen_broken_muntin')):
        bpy.data.objects.remove(obj,do_unlink=True)
    elif obj.type=='MESH' and obj.name.startswith('WALL_BACK'):
        lo,hi=world_bounds(obj)
        if lo[0]<4.6 and hi[0]>3.4 and lo[2]<2.8:
            mat=obj.data.materials[0];bpy.data.objects.remove(obj,do_unlink=True)
            pieces=[]
            if lo[0]<3.4:pieces.append((lo,[3.4,hi[1],hi[2]]))
            if hi[0]>4.6:pieces.append(([4.6,lo[1],lo[2]],hi))
            if hi[2]>2.8:pieces.append(([max(lo[0],3.4),lo[1],2.8],[min(hi[0],4.6),hi[1],hi[2]]))
            for i,(a,b) in enumerate(pieces):
                bpy.ops.mesh.primitive_cube_add(size=1,location=[(x+y)/2 for x,y in zip(a,b)])
                part=bpy.context.object;part.name='WALL_KitchenPassage_'+str(i);part.scale=[y-x for x,y in zip(a,b)];part.data.materials.append(mat)
for mat in list(bpy.data.materials):
    if mat.users:material_record(mat,'House')
export('RumahAtmaja',house=True)

# Repair tires/rims from the existing recovery mesh; lift the seat visibly as scripted.
bpy.ops.wm.open_mainfile(filepath=str(checked(REC/'02_Motorcycle/Blender/DamarScooter.blend')))
for obj in list(bpy.context.scene.objects):
    if obj.name.startswith('WHEEL_') and not any(s in obj.name for s in ['spoke','caliper']):
        y=-.625 if 'FRONT' in obj.name else .625;centre=Vector((0,y,.25))
        axis=Vector((0,0,1)) if '_tire' in obj.name else Vector((0,-1,0))
        rotation=axis.rotation_difference(Vector((1,0,0))).to_matrix().to_4x4()
        obj.data.transform(Matrix.Translation(centre)@rotation@Matrix.Translation(-centre))
    if obj.name=='SEAT':
        pivot=Vector((0,-.03,.72));obj.data.transform(Matrix.Translation(pivot)@Matrix.Rotation(math.radians(12),4,'X')@Matrix.Translation(-pivot))
note=bpy.data.materials.new('SurveyNotebookPaper');note.diffuse_color=(.7,.66,.50,1)
bpy.ops.mesh.primitive_cube_add(size=1,location=(0,.26,.677));obj=bpy.context.object;obj.name='SurveyNotebookUnderSeat';obj.scale=(.12,.18,.018);obj.data.materials.append(note)
for mat in list(bpy.data.materials):
    if mat.users:material_record(mat,'Scooter')
export('DamarScooter',.7)

bpy.ops.wm.open_mainfile(filepath=str(checked(REC/'03_Frangipani_Tree/Blender/FallenFrangipani.blend')))
# A grounded uprooted root with a raised arch over the narrow side route, not a floating tree.
for obj in list(bpy.context.scene.objects):
    if obj.type!='MESH':continue
    inv=obj.matrix_world.inverted()
    for v in obj.data.vertices:
        p=obj.matrix_world@v.co
        if 1.15<p.x<3.9:
            blend=min(1,(p.x-1.15)/.35,(3.9-p.x)/.35)
            p.z+=max(0,1.24-p.z)*max(0,blend);v.co=inv@p
for mat in list(bpy.data.materials):
    if mat.users:material_record(mat,'Tree')
export('FallenFrangipani',.22)

selection={'large_iron_gate':.28,'shrub_03':.45,'fern_02':.45,'grass_bermuda_01':1,
           'plastic_monobloc_chair_01':1,'planter_pot_clay':1,'wooden_bucket_01':.6,'security_light':.5}
manifest=json.loads((STAGE/'asset-manifest.json').read_text())
for asset_id,ratio in selection.items():
    record=next(r for r in manifest['assets'] if r['id']==asset_id)
    # Verify exact staging bytes (previews are not game assets).
    for entry in record['files']:
        p=STAGE/entry['path'];assert hashlib.sha256(p.read_bytes()).hexdigest()==entry['sha256'],p
    bpy.ops.wm.read_factory_settings(use_empty=True)
    folder=STAGE/record['category']/asset_id
    fbx=next(folder.glob('*.fbx'));bpy.ops.import_scene.fbx(filepath=str(checked(fbx)),use_anim=False)
    tex=folder/'textures'
    maps={key:next(tex.glob('*'+suffix+'*'),None) for key,suffix in [('albedo','_diff_'),('normal','_nor_gl_'),('arm','_arm_')]}
    for mat in list(bpy.data.materials):
        if mat.users:material_record(mat,asset_id,maps)
    export(asset_id,ratio)

# Terrain/material-only sets already supplied in the staging drop.
for asset_id in ['brown_mud_03','stony_dirt_path','mossy_cobblestone','dirty_concrete','concrete_floor_damaged_01']:
    record=next(r for r in manifest['assets'] if r['id']==asset_id);folder=STAGE/record['category']/asset_id
    maps={key:source_texture(next(folder.glob('*'+suffix+'*'),None)) for key,suffix in [('albedo','_diff_'),('normal','_nor_gl_'),('arm','_arm_')]}
    materials.append(dict(name=asset_id,color=[1,1,1,1],roughness=.8,metallic=0,alphaClip=False,**maps))
receipt=dict(materials=materials,models=models,sources=sources,
    protectedGoldenSceneSha256=hashlib.sha256((ROOT/'Assets/_LayarTerakhir/assets/Scenes/Prolog_KamarKos.unity').read_bytes()).hexdigest())
(ROOT/'Audit/atmaja-art-manifest.json').write_text(json.dumps(receipt,indent=2)+'\n')
print('ATMAJA_PREPARED',[(m['name'],m['triangles'],m['renderers']) for m in models])
