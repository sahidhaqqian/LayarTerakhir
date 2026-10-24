"""Convert downloaded hospital meshes / designer Merah VRM, without modeling art.
blender -b -t 2 --python Tools/import_phase2_assets.py
Reuses authored geometry/UVs and existing motions. No mesh primitives, sculpting,
texture painting, generated imagery or new animation clips are created here.
"""
import json
import math
import sys
import struct
from pathlib import Path
import bpy
from mathutils import Vector, Matrix

ROOT=Path(__file__).resolve().parents[1]
SRC=ROOT/'Builds/ArtSources/Phase2'
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase2/Imported'
MATERIALS=[]
MODELS=[]


def reset():bpy.ops.wm.read_factory_settings(use_empty=True)

def upstream(socket):
    todo=[link.from_node for link in socket.links];seen=set()
    while todo:
        node=todo.pop()
        if node.name in seen:continue
        seen.add(node.name)
        if node.type=='TEX_IMAGE':return node.image
        todo.extend(link.from_node for inp in node.inputs for link in inp.links)


def extract_materials(folder):
    folder.mkdir(parents=True,exist_ok=True)
    for mat in bpy.data.materials:
        if not mat.use_nodes:
            MATERIALS.append(dict(name=mat.name,color=list(mat.diffuse_color),roughness=.6,metallic=0));continue
        bs=next((n for n in mat.node_tree.nodes if n.type=='BSDF_PRINCIPLED'),None)
        if not bs:
            record=dict(name=mat.name,color=list(mat.diffuse_color),roughness=.6,metallic=0)
            image=next((n.image for n in mat.node_tree.nodes if n.type=='TEX_IMAGE' and n.image),None)
            if image:
                path=folder/(mat.name.replace('.','_')+'Albedo.png')
                image.filepath_raw=str(path);image.file_format='PNG';image.save();record['Albedo']=str(path.relative_to(ROOT))
            MATERIALS.append(record);continue
        record=dict(name=mat.name,color=list(bs.inputs['Base Color'].default_value),roughness=bs.inputs['Roughness'].default_value,metallic=bs.inputs['Metallic'].default_value)
        for slot,socket in [('Albedo','Base Color'),('Normal','Normal'),('MetalRough','Roughness')]:
            image=upstream(bs.inputs[socket])
            if not image:continue
            filename=mat.name.replace('.','_')+slot+'.png';path=folder/filename
            if max(image.size)>2048:
                ratio=2048/max(image.size);image.scale(round(image.size[0]*ratio),round(image.size[1]*ratio))
            image.filepath_raw=str(path);image.file_format='PNG';image.save()
            record[slot]=str(path.relative_to(ROOT))
        MATERIALS.append(record)


def export(name,objects,folder,rotate_x=0):
    """Bake source transforms and recenter an existing mesh assembly, no new topology."""
    copies=[];rotation=Matrix.Rotation(math.radians(rotate_x),4,'X')
    for original in objects:
        if original.type!='MESH':continue
        copy=original.copy();copy.data=original.data.copy();bpy.context.collection.objects.link(copy)
        world=rotation@original.matrix_world
        copy.parent=None;copy.matrix_world=Matrix.Identity(4);copy.data.transform(world);copies.append(copy)
    points=[Vector(corner) for copy in copies for corner in copy.bound_box]
    low=Vector(tuple(min(p[i] for p in points) for i in range(3)));high=Vector(tuple(max(p[i] for p in points) for i in range(3)))
    origin=Vector(((low.x+high.x)/2,(low.y+high.y)/2,low.z))
    for copy in copies:copy.data.transform(Matrix.Translation(-origin))
    bpy.ops.object.select_all(action='DESELECT')
    for copy in copies:copy.select_set(True)
    path=OUT/folder/(name+'.fbx');path.parent.mkdir(parents=True,exist_ok=True)
    bpy.ops.export_scene.fbx(filepath=str(path),use_selection=True,object_types={'MESH'},bake_anim=False,
        axis_forward='-Z',axis_up='Y',apply_unit_scale=True,use_mesh_modifiers=True,add_leaf_bones=False,path_mode='AUTO')
    MODELS.append(dict(name=name,path=str(path.relative_to(ROOT)),sourceMeshes=[o.name for o in objects],bounds=list(high-low)))
    for copy in copies:bpy.data.objects.remove(copy,do_unlink=True)


def madduck():
    bpy.ops.wm.open_mainfile(filepath=str(SRC/'Madduck/Hospital_assets/hospital.blend'))
    rotations={'Bench':90,'Door_1':90,'Door_swing':90,'Iv_bag_holder':90,'IV_bag':90,
        'Tile_corner':90,'Tile_Corner':90,'Tile_Wall':90,'Tile_Wall_Doorway_1':90,'Tile_Wall_Doorway_2':90,'Tile_Wall_half':90,'Tile_Wall_Window':90}
    wanted=['Bed','Bench','Cabinet_1','Cabinet_2','Cabinet_3','Ceiling_Light','Ceiling_Tile','Chair','Door_1','Door_swing','Exit_sign',
        'Floor_tile_1','Floor_tile_2','IV_bag','Iv_bag_holder','Magazine','Pillar','Table','Tile_corner','Tile_Corner','Tile_Wall',
        'Tile_Wall_Doorway_1','Tile_Wall_Doorway_2','Tile_Wall_half','Tile_Wall_Window','Wheel_chair']
    for name in wanted:export({'Tile_corner':'Tile_InnerCorner','Tile_Corner':'Tile_OuterCorner'}.get(name,name),[bpy.data.objects[name]],'Madduck/Models',rotations.get(name,0))
    # Preserve the author's original maps; only URP mask packing is done later.
    import shutil
    shutil.copytree(SRC/'Madduck/Hospital_assets/textures',OUT/'Madduck/Textures',dirs_exist_ok=True)


def legacy_factors(path):
    raw=path.read_bytes();length=struct.unpack_from('<I',raw,12)[0];doc=json.loads(raw[20:20+length])
    for material in doc.get('materials',[]):
        props=material.get('extensions',{}).get('KHR_materials_pbrSpecularGlossiness')
        if not props:continue
        for record in MATERIALS:
            if record['name']==material.get('name'):
                record.update(color=props.get('diffuseFactor',[1,1,1,1]),roughness=1-props.get('glossinessFactor',1),metallic=0,
                    transparent=material.get('alphaMode')=='BLEND')


def neonatal():
    reset();bpy.ops.import_scene.gltf(filepath=str(SRC/'Neonatal.glb'))
    # Separate the artist's existing glass primitive from the opaque chassis.
    # This preserves all vertices/UVs and permits glass-only collider/shadow exclusion.
    for obj in list(bpy.data.objects):
        if obj.type!='MESH' or len(obj.data.materials)<2:continue
        bpy.ops.object.select_all(action='DESELECT');obj.select_set(True);bpy.context.view_layer.objects.active=obj
        bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.separate(type='MATERIAL');bpy.ops.object.mode_set(mode='OBJECT')
    extract_materials(OUT/'Neonatal/Textures');legacy_factors(SRC/'Neonatal.glb')
    export('Neonatal',list(bpy.data.objects),'Neonatal/Models')


def glbs():
    reset();bpy.ops.import_scene.gltf(filepath=str(SRC/'AutopsyLab.glb'));extract_materials(OUT/'Autopsy/Textures')
    groups={'AutopsyTable':'DissectionTable_Main_20','SurgicalLamp':'MedicalLight_Legs_13','PrivacyScreen':'MedicalPartition_Curtain00_6',
            'Microscope':'Microscope_Base_47','VialRack':'VialHolder_Short_33','LabTripod':'LabTripod_Base_37','GlassSlide':'Microscope_BloodSample_48'}
    for name,root in groups.items():
        node=bpy.data.objects[root];export(name,[node]+list(node.children_recursive),'Autopsy/Models')
    # Additional source objects are genuine, separately modeled laboratory items.
    for name,root in [('BloodVial','Vial_Full_27'),('EmptyVial','Vial_21'),('SampleHolder','VialHolder_Tall_34')]:
        node=bpy.data.objects[root];export(name,[node]+list(node.children_recursive),'Autopsy/Models')
    neonatal()
    for filename in ['Scissors']:
        reset();bpy.ops.import_scene.gltf(filepath=str(SRC/(filename+'.glb')));extract_materials(OUT/filename/'Textures')
        export(filename,list(bpy.data.objects),filename+'/Models')
    reset();bpy.ops.import_scene.fbx(filepath=str(SRC/'Kenney/Models/FBX format/stairs.fbx'))
    export('Stairs',list(bpy.data.objects),'Kenney')


def merah():
    reset();sys.path.insert(0,str(ROOT/'Tools'))
    import prepare_designer_vrm as pipeline
    import prepare_phase1_characters as motions
    path=ROOT/'Assets/_LayarTerakhir/assets/LayarTerakhir/Merah.vrm'
    output=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase2/Characters';output.mkdir(parents=True,exist_ok=True)
    pipeline.OUT=output;pipeline.MODEL=output/'Models';pipeline.TEXTURE=output/'Textures'
    pipeline.MODEL.mkdir(exist_ok=True);pipeline.TEXTURE.mkdir(exist_ok=True)
    gltf=pipeline.read_gltf(path);bpy.ops.import_scene.gltf(filepath=str(path))
    rig=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
    mesh=next(o for o in bpy.context.scene.objects if o.type=='MESH' and any(m.type=='ARMATURE' for m in o.modifiers))
    textures=pipeline.textures('Lastri',gltf);mesh.data.materials[0].name='LastriSurface'
    pipeline.skeleton(rig,mesh,'Lastri',1.8);original=pipeline.simplify(mesh,28000)
    mesh.hide_viewport=True;clips,motion_sources=motions.reuse_motion(rig);mesh.hide_viewport=False
    pipeline.export_fbx(pipeline.MODEL/'Lastri.fbx',[rig,mesh],True)
    (ROOT/'Audit/phase2-lastri-import.json').write_text(json.dumps(dict(source=str(path.relative_to(ROOT)),sourceSha256=pipeline.digest(path),
        originalTriangles=original,gameTriangles=sum(len(p.vertices)-2 for p in mesh.data.polygons),textures=textures,clips=clips,
        motionSources=motion_sources,operation='Designer Merah VRM, converted/optimized; previously authored motion retarget only'),indent=2)+'\n')


def extras():
    MODELS[:]=[m for m in MODELS if m['name'] not in ['Safe','Generator']]
    MATERIALS[:]=[m for m in MATERIALS if not m['name'].startswith(('Safe_','Generator_'))]
    for filename in ['Safe','Generator']:
        reset();bpy.ops.import_scene.gltf(filepath=str(SRC/(filename+'.glb')))
        for material in bpy.data.materials:material.name=filename+'_'+material.name
        if filename=='Safe':
            # Separate the artist's existing front plate/handle from its case,
            # retaining every original face/UV. No replacement geometry.
            original=next(o for o in bpy.data.objects if o.type=='MESH')
            bpy.context.view_layer.objects.active=original;original.select_set(True)
            bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.separate(type='LOOSE');bpy.ops.object.mode_set(mode='OBJECT')
            door=[];case=[]
            for o in [o for o in bpy.data.objects if o.type=='MESH']:
                points=[o.matrix_world@Vector(p) for p in o.bound_box]
                (door if max(p.y for p in points)<=.055 and max(p.z for p in points)>-.38 else case).append(o)
            assert door and case
            for name,parts in [('SafeDoor',door),('SafeBody',case)]:
                bpy.ops.object.select_all(action='DESELECT')
                for o in parts:o.select_set(True)
                bpy.context.view_layer.objects.active=parts[0];bpy.ops.object.join();parts[0].name=name
        extract_materials(OUT/'Additional/Textures')
        export(filename,list(bpy.data.objects),'Additional/Models')


if __name__=='__main__':
    if '--neonatal-only' in sys.argv:
        previous=json.loads((ROOT/'Audit/phase2-import-manifest.json').read_text())
        MATERIALS.extend(m for m in previous['materials'] if '/Neonatal/' not in m.get('Albedo',''))
        MODELS.extend(m for m in previous['models'] if m['name']!='Neonatal')
        neonatal()
    elif '--extras-only' in sys.argv:
        previous=json.loads((ROOT/'Audit/phase2-import-manifest.json').read_text());MATERIALS.extend(previous['materials']);MODELS.extend(previous['models'])
    else:
        madduck();glbs()
        if '--keep-lastri' not in sys.argv:merah()
    if '--neonatal-only' not in sys.argv:extras()
    (ROOT/'Audit/phase2-import-manifest.json').write_text(json.dumps(dict(models=MODELS,materials=MATERIALS),indent=2)+'\n')
    print('PHASE2_IMPORTED',len(MODELS),'source mesh assemblies')
