"""Project-authored louver vent; run with blender --background --python this file."""
import math
from pathlib import Path
import bpy
from mathutils import Vector
ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/PrologRevision/Models'
SOURCE=ROOT/'Assets/_LayarTerakhir/assets/ArtSources/Prolog'
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)

def material(name,color,metal,rough):
    mat=bpy.data.materials.new(name);mat.diffuse_color=(*color,1);mat.use_nodes=True
    shader=mat.node_tree.nodes.get('Principled BSDF');shader.inputs['Base Color'].default_value=(*color,1)
    shader.inputs['Metallic'].default_value=metal;shader.inputs['Roughness'].default_value=rough
    return mat
paint=material('VentAgedEnamel',(.27,.30,.25),.15,.65)
metal=material('VentScrewSteel',(.14,.15,.13),.7,.35)

def box(name,at,size,mat,bevel=.004):
    bpy.ops.mesh.primitive_cube_add(size=1,location=at);obj=bpy.context.object;obj.name=name
    obj.dimensions=size;bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
    obj.data.materials.append(mat)
    modifier=obj.modifiers.new('Soft manufactured edges','BEVEL');modifier.width=bevel;modifier.segments=3
    bpy.context.view_layer.objects.active=obj;bpy.ops.object.modifier_apply(modifier=modifier.name)
    obj.data.use_auto_smooth=True
    modifier=obj.modifiers.new('Corner normals','WEIGHTED_NORMAL');modifier.keep_sharp=True
    bpy.ops.object.modifier_apply(modifier=modifier.name)
    return obj

def combine(parts,name):
    bpy.ops.object.select_all(action='DESELECT')
    for obj in parts:obj.select_set(True)
    bpy.context.view_layer.objects.active=parts[0];bpy.ops.object.join();parts[0].name=name
    bpy.context.scene.cursor.location=(0,0,0);bpy.ops.object.origin_set(type='ORIGIN_CURSOR')
    return parts[0]
frame=[]
for sign in (-1,1):
    frame.append(box('FrameVertical',(sign*.43,0,0),(.055,.07,1.05),paint))
    frame.append(box('FrameHorizontal',(0,0,sign*.50),(.87,.07,.055),paint))
combine(frame,'VentFixedFrame')
leaf=[]
for sign in (-1,1):
    leaf.append(box('LeafVertical',(sign*.387,.048,0),(.035,.035,.965),paint,.003))
    leaf.append(box('LeafHorizontal',(0,.048,sign*.465),(.80,.035,.035),paint,.003))
for i in range(7):
    slat=box('FoldedLouver',(0,.043,-.375+i*.125),(.735,.018,.155),paint,.003)
    slat.rotation_euler.x=math.radians(-28)
    leaf.append(slat)
for x in (-.385,.385):
    for z in (-.465,.465):
        bpy.ops.mesh.primitive_uv_sphere_add(segments=12,ring_count=6,radius=1,location=(x,.073,z))
        screw=bpy.context.object;screw.name='Screw';screw.scale=(.009,.0035,.009);screw.data.materials.append(metal);leaf.append(screw)
        leaf.append(box('ScrewSlot',(x,.077,z),(.010,.0015,.0015),metal,.0003))
leaf.append(box('PullTab',(0,.075,-.458),(.085,.027,.025),metal,.004))
combine(leaf,'VentLeaf')
for obj in bpy.context.scene.objects:
    if obj.type=='MESH':
        bpy.context.view_layer.objects.active=obj;obj.select_set(True)
        bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);obj.select_set(False)
OUT.mkdir(parents=True,exist_ok=True);SOURCE.mkdir(parents=True,exist_ok=True)
bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE/'PrologVentilation.blend'))
bpy.ops.export_scene.fbx(filepath=str(OUT/'PrologVentilation.fbx'),use_selection=False,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y')
print('PROLOG_VENT_EXPORTED')
