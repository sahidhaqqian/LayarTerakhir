"""Prepare the licensed full-scale Innerscene sedan for static street dressing.

blender -b -t 2 --python Tools/prepare_neighborhood_sedan.py
Original OBJ and CC0 provenance remain in ArtSources/NeighborhoodVehicles.
"""
import bpy
import json
import math
from pathlib import Path
from mathutils import Vector

ROOT=Path(__file__).resolve().parents[1]
SOURCE=ROOT/'ArtSources/NeighborhoodVehicles/InnersceneSedan/red-four-door-sedan.obj'
OUTPUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Imported/NeighborhoodVehicles/NeighborhoodSedan.fbx'
SURFACES={
    'paint':((.25,.28,.30,1),.45,.45),
    'black':((.025,.028,.03,1),.08,.65),
    'rubber':((.018,.02,.022,1),0,.85),
    'chrome':((.45,.47,.48,1),.8,.3),
    'steel':((.21,.23,.24,1),.7,.5),
    'glass':((.025,.045,.055,1),.25,.12),
    'interior':((.045,.042,.038,1),0,.85),
    'clearlens':((.43,.46,.47,1),.35,.2),
    'redlens':((.26,.025,.02,1),.1,.3),
    'white':((.66,.65,.61,1),0,.65),
}
bpy.ops.wm.read_factory_settings(use_empty=True)
# The public OBJ endpoint omits its referenced MTL. Define project-owned
# material values by the original usemtl roles, without inventing source textures.
roles=['black.001','chrome','clearlens','glass.001','interior','paint','redlens','rubber.001','steel.001','white']
mtl=[]
for name in roles:
    color,metal,rough=SURFACES[name.split('.')[0]]
    mtl.extend([f'newmtl {name}',f'Kd {color[0]} {color[1]} {color[2]}',f'Pm {metal}',f'Pr {rough}',''])
SOURCE.with_name('sedan_red.mtl').write_text('\n'.join(mtl))
bpy.ops.wm.obj_import(filepath=str(SOURCE),forward_axis='Y',up_axis='Z')
meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
before=sum(len(o.data.loop_triangles) for o in meshes)
for obj in meshes:
    # Brake drill recesses, individual lugs and valve stems are invisible at
    # parking distance; retain the body, glazing, lamps, tyres and wheel rims.
    if any(n in obj.name for n in ['Rotor_drill_recess','Lug_','Valve_stem']):
        bpy.data.objects.remove(obj,do_unlink=True);continue
    obj.rotation_euler.z+=math.pi/2
    bpy.context.view_layer.objects.active=obj;obj.select_set(True)
    bpy.ops.object.transform_apply(location=False,rotation=True,scale=True)
    obj.select_set(False)
for material in list(bpy.data.materials):
    role=material.name.split('.')[0]
    color,metal,rough=SURFACES.get(role,SURFACES['black'])
    material.name='StreetSedan_'+role;material.diffuse_color=color;material.use_nodes=True
    shader=material.node_tree.nodes.get('Principled BSDF')
    shader.inputs['Base Color'].default_value=color
    shader.inputs['Metallic'].default_value=metal
    shader.inputs['Roughness'].default_value=rough
meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
bpy.ops.object.select_all(action='SELECT');bpy.context.view_layer.objects.active=meshes[0]
bpy.ops.object.join();body=bpy.context.object;body.name='NeighborhoodSedan'
bpy.ops.object.transform_apply(location=True,rotation=True,scale=True)
# Remove duplicate slots before splitting by material for bounded draw calls.
canonical={}
for i,mat in enumerate(body.data.materials):canonical.setdefault(mat.name,i)
for face in body.data.polygons:face.material_index=canonical[body.data.materials[face.material_index].name]
body.data.calc_loop_triangles();input_triangles=len(body.data.loop_triangles)
if input_triangles>12000:
    decimate=body.modifiers.new('StreetDistanceBudget','DECIMATE');decimate.ratio=12000/input_triangles
    bpy.ops.object.modifier_apply(modifier=decimate.name)
body.data.calc_loop_triangles();triangles=len(body.data.loop_triangles)
# Normalize the pivot to the ground and the footprint centre, preserving real units.
points=[v.co for v in body.data.vertices]
minimum=Vector(tuple(min(v[i] for v in points) for i in range(3)))
maximum=Vector(tuple(max(v[i] for v in points) for i in range(3)))
offset=Vector(((minimum.x+maximum.x)/2,(minimum.y+maximum.y)/2,minimum.z))
for v in body.data.vertices:v.co-=offset
for face in body.data.polygons:face.use_smooth=True
OUTPUT.parent.mkdir(parents=True,exist_ok=True)
bpy.ops.export_scene.fbx(filepath=str(OUTPUT),use_selection=True,object_types={'MESH'},
    bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False,path_mode='STRIP')
record=dict(source=str(SOURCE.relative_to(ROOT)),output=str(OUTPUT.relative_to(ROOT)),
    inputTriangles=input_triangles,outputTriangles=triangles,dimensionsMetres=list(maximum-minimum),
    surfaceRoles=[m.name for m in body.data.materials],staticOnly=True)
assert len(body.data.materials)>=7, 'Material roles were lost during OBJ import'
(ROOT/'Audit/neighborhood-sedan-preparation.json').write_text(json.dumps(record,indent=2)+'\n')
print('SEDAN_PREPARED',json.dumps(record),flush=True)
# Offline review image; the final acceptance evidence comes from Unity.
floor=bpy.data.materials.new('PreviewGround');floor.diffuse_color=(.13,.13,.12,1)
bpy.ops.mesh.primitive_plane_add(size=200);bpy.context.object.data.materials.append(floor)
cam_data=bpy.data.cameras.new('PreviewCamera');cam=bpy.data.objects.new('PreviewCamera',cam_data);bpy.context.collection.objects.link(cam)
cam.location=(5,-6,3);cam.rotation_euler=(Vector((0,0,.7))-cam.location).to_track_quat('-Z','Y').to_euler();cam_data.lens=52
scene=bpy.context.scene;scene.camera=cam
for pos,power,size in [((2,-3,6),1500,5),((-4,-1,3),800,4)]:
    data=bpy.data.lights.new('PreviewSoftbox','AREA');data.energy=power;data.shape='DISK';data.size=size
    light=bpy.data.objects.new('PreviewSoftbox',data);scene.collection.objects.link(light);light.location=pos;light.rotation_euler=(Vector((0,0,.5))-light.location).to_track_quat('-Z','Y').to_euler()
scene.render.engine='CYCLES';scene.cycles.samples=16;scene.cycles.use_denoising=False;scene.render.resolution_x=960;scene.render.resolution_y=540;scene.render.resolution_percentage=100
scene.world=bpy.data.worlds.new('PreviewWorld');scene.world.color=(.18,.18,.18);scene.render.filepath='/root/unity-logs/recovery-sedan-preview.png'
bpy.ops.render.render(write_still=True)
