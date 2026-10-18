"""Author only the original, unbranded Golden scene microphone and bed mat.
blender -b --python Tools/author_golden_details.py
"""
import bpy, math
from pathlib import Path

OUT = Path(__file__).resolve().parents[1] / 'Assets/_LayarTerakhir/assets/Art/Original/Models'

def clear():
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.object.delete(use_global=False)

def cylinder(name, point, radius, depth, slot, vertices=24):
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth, location=point)
    obj=bpy.context.object; obj.name=name
    mat=bpy.data.materials.get(slot) or bpy.data.materials.new(slot)
    obj.data.materials.append(mat)
    bevel=obj.modifiers.new('Machined rim','BEVEL'); bevel.width=.002; bevel.segments=2
    bpy.ops.object.modifier_apply(modifier=bevel.name)
    return obj

def export(name):
    groups={}
    for obj in list(bpy.context.scene.objects):
        groups.setdefault(obj.data.materials[0].name,[]).append(obj)
    for slot,objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for obj in objects: obj.select_set(True)
        bpy.context.view_layer.objects.active=objects[0]
        if len(objects)>1:bpy.ops.object.join()
        bpy.context.object.name=slot
    for obj in list(bpy.context.scene.objects):
        bpy.ops.object.select_all(action='DESELECT');obj.select_set(True)
        bpy.context.view_layer.objects.active=obj
        bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT')
        bpy.ops.uv.smart_project(island_margin=.02);bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.fbx(filepath=str(OUT/(name+'.fbx')),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)

clear()
cylinder('Weighted base',(0,0,.009),.065,.018,'MicBody')
cylinder('Short stem',(0,0,.070),.008,.12,'MicBody',16)
cylinder('Microphone body',(0,0,.166),.027,.085,'MicBody')
cylinder('Mesh head',(0,0,.228),.029,.06,'MicGrille')
for z in [.203+i*.006 for i in range(9)]:
    bpy.ops.mesh.primitive_torus_add(major_radius=.029,minor_radius=.0012,major_segments=24,minor_segments=4,location=(0,0,z))
    obj=bpy.context.object;obj.data.materials.append(bpy.data.materials['MicBody'])
cylinder('Small status lens',(0,-.026,.16),.002,.001,'MicGrille',12).rotation_euler[0]=math.pi/2
export('KosBudgetMicrophone')

clear()
verts=[];uv=[];faces=[];nx=14;ny=10
for j in range(ny+1):
    for i in range(nx+1):
        u=i/nx;v=j/ny
        # Flat, safe-to-walk mat with a slightly curled hem, not a floating rug.
        height=.002+.006*max(0,(u-.9)*10)*math.sin(v*math.pi)**2
        verts.append(((u-.5)*1.18,(v-.5)*.78,height));uv.append((u,v))
for j in range(ny):
    for i in range(nx):
        a=j*(nx+1)+i;faces.append((a,a+1,a+nx+2,a+nx+1))
mesh=bpy.data.meshes.new('Worn bedside mat');mesh.from_pydata(verts,[],faces);mesh.update()
obj=bpy.data.objects.new('BedMatFabric',mesh);bpy.context.collection.objects.link(obj)
obj.data.materials.append(bpy.data.materials.new('BedMatFabric'))
layer=mesh.uv_layers.new()
for poly in mesh.polygons:
    for loop in poly.loop_indices:layer.data[loop].uv=uv[mesh.loops[loop].vertex_index]
# Preserve the authored continuous cloth UVs, unlike the metal prop's smart unwrap.
bpy.ops.object.select_all(action='SELECT')
bpy.ops.export_scene.fbx(filepath=str(OUT/'KosBedMat.fbx'),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)
print('GOLDEN_DETAILS_EXPORTED',OUT)
