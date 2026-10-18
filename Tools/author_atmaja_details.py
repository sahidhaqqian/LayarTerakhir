"""Original static dining furniture, clock and AR apparition, Blender 4.
Run after prepare_atmaja_art.py / pack_atmaja_masks.py. No staging or Golden assets edited.
"""
from pathlib import Path
import bpy,json,math
ROOT=Path(__file__).resolve().parents[1];OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Atmaja/Models'
manifest=json.loads((ROOT/'Audit/atmaja-art-manifest.json').read_text())

def clear():bpy.ops.wm.read_factory_settings(use_empty=True)
def mat(name):return bpy.data.materials.get(name) or bpy.data.materials.new(name)
def box(name,p,s,m,bevel=.01):
    bpy.ops.mesh.primitive_cube_add(size=1,location=p);o=bpy.context.object;o.name=name;o.scale=s
    bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o.data.materials.append(mat(m))
    if bevel:
        mod=o.modifiers.new('Worn edge bevel','BEVEL');mod.width=bevel;mod.segments=2;bpy.ops.object.modifier_apply(modifier=mod.name)
    return o
def ball(name,p,s,m,rotation=(0,0,0)):
    bpy.ops.mesh.primitive_uv_sphere_add(segments=16,ring_count=10,location=p);o=bpy.context.object;o.name=name;o.scale=s;o.rotation_euler=rotation;o.data.materials.append(mat(m));return o
def cylinder(name,p,r,d,m,rot=(0,0,0)):
    bpy.ops.mesh.primitive_cylinder_add(vertices=32,radius=r,depth=d,location=p,rotation=rot);o=bpy.context.object;o.name=name;o.data.materials.append(mat(m));return o
def tube(name,points,r,m):
    data=bpy.data.curves.new(name,'CURVE');data.dimensions='3D';data.bevel_depth=r;data.bevel_resolution=1
    spline=data.splines.new('POLY');spline.points.add(len(points)-1)
    for v,p in zip(spline.points,points):v.co=(*p,1)
    o=bpy.data.objects.new(name,data);bpy.context.collection.objects.link(o);o.data.materials.append(mat(m));return o
def cloth(name,rings,m):
    verts=[];faces=[];n=24
    for z,ry,rx,cy in rings:
        for i in range(n):
            a=i*math.pi*2/n;fold=1+.04*math.sin(a*8+z*3);verts.append((rx*math.cos(a)*fold,cy+ry*math.sin(a)*fold,z))
    for j in range(len(rings)-1):
        for i in range(n):a=j*n+i;b=j*n+(i+1)%n;faces.append((a,b,b+n,a+n))
    mesh=bpy.data.meshes.new(name);mesh.from_pydata(verts,[],faces);mesh.update();o=bpy.data.objects.new(name,mesh);bpy.context.collection.objects.link(o);o.data.materials.append(mat(m));return o
def export(name):
    for o in list(bpy.context.scene.objects):
        if o.type=='CURVE':bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o;bpy.ops.object.convert(target='MESH')
    groups={}
    for o in list(bpy.context.scene.objects):
        if o.type!='MESH':continue
        role=o.name if o.name in ['ClockHourBlade','ClockMinuteBlade','ClockDrawer'] else 'Static'
        groups.setdefault((role,o.data.materials[0].name),[]).append(o)
    for (role,m),objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for o in objects:o.select_set(True)
        bpy.context.view_layer.objects.active=objects[0]
        if len(objects)>1:bpy.ops.object.join()
        o=bpy.context.object;o.name=role+'__'+m
        # Physical box-projected grain; source texture tiling remains in metres.
        if not o.data.uv_layers:o.data.uv_layers.new()
        uv=o.data.uv_layers.active.data
        for poly in o.data.polygons:
            normal=o.matrix_world.to_3x3()@poly.normal;axis=max(range(3),key=lambda i:abs(normal[i]));others=[i for i in range(3) if i!=axis]
            for index in poly.loop_indices:
                p=o.matrix_world@o.data.vertices[o.data.loops[index].vertex_index].co;uv[index].uv=(p[others[0]],p[others[1]])
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.fbx(filepath=str(OUT/(name+'.fbx')),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='STRIP')

wood='House_MAT_WoodDoor';brass='Detail_AgedBrass';clothmat='Detail_ARCloth';hair='Detail_ARHair';skin='Detail_ARSkin'
clear()
box('Dining table top',(0,0,.90),(1.45,3.1,.075),wood,.025)
for x in [-.61,.61]:
    box('Long apron',(x,0,.82),(.065,2.88,.11),wood,.012)
    for y in [-1.37,1.37]:
        box('Tapered banquet leg',(x,y,.43),(.09,.09,.86),wood,.012)
        box('Leg shoe',(x,y,.07),(.115,.115,.10),wood,.008)
export('LongDiningTable')

clear()
box('Clock back',(0,.12,1.10),(.62,.07,2.12),wood,.018)
for x in [-.28,.28]:box('Carved side pilaster',(x,0,1.10),(.085,.32,2.08),wood,.012)
for z,w,d in [(.07,.76,.46),(.16,.70,.41),(1.45,.66,.39),(2.10,.72,.42),(2.18,.78,.47)]:box('Clock cornice',(0,0,z),(w,d,.075),wood,.012)
cylinder('Dial',(0,-.19,1.78),.235,.022,'Scooter_SurveyNotebookPaper',(math.pi/2,0,0))
box('ClockHourBlade',(0,-.209,1.835),(.027,.015,.15),brass,.004)
box('ClockMinuteBlade',(0,-.220,1.87),(.018,.014,.21),brass,.003)
cylinder('Dial hub',(0,-.23,1.78),.028,.02,brass,(math.pi/2,0,0))
box('Pendulum rod',(0,-.06,.98),(.018,.022,.64),brass,.004)
ball('Pendulum bob',(0,-.07,.65),(.13,.035,.13),brass)
box('ClockDrawer',(0,-.04,.28),(.47,.34,.19),wood,.01)
cylinder('Drawer knob',(0,-.225,.28),.022,.04,brass,(math.pi/2,0,0))
export('GrandfatherClock')

clear()
cloth('Seated skirt',[(.07,.11,.14,-.25),(.25,.13,.15,-.23),(.43,.23,.23,-.17),(.58,.18,.21,0)],clothmat)
cloth('Kebaya blouse',[(.52,.14,.20,0),(.68,.13,.18,0),(.88,.12,.23,0),(1.02,.11,.22,0),(1.07,.08,.10,0)],clothmat)
for side in [-1,1]:
    tube('Sleeve arm',[(side*.21,0,.97),(side*.28,-.12,.77),(side*.19,-.28,.63)],.065,clothmat)
    ball('Resting hand',(side*.17,-.32,.62),(.06,.09,.035),skin)
    ball('Bare foot',(side*.10,-.30,.045),(.065,.15,.035),skin)
tube('Bent neck',[(0,0,1.04),(-.13,-.015,1.13)],.045,skin)
ball('Tilted head',(-.21,-.025,1.14),(.11,.09,.15),skin,(0,math.pi/2,0))
ball('Long dark hair cap',(-.21,.007,1.17),(.17,.095,.10),hair)
for i in range(15):
    x=-.35+i*.022;y=.025+.02*math.sin(i)
    tube('Fallen hair strand',[(x,y,1.20),(x-.015,y-.01,.99),(x+.025,y-.02,.76+(i%3)*.025)],.012,hair)
for x in [-.25,-.15]:ball('Hollow eye',(x,-.109,1.15),(.022,.008,.013),hair)
export('ARSeatedAtmaja')

for name,color,rough,metal in [(brass,[.37,.26,.10,1],.50,.6),(clothmat,[.26,.29,.25,1],.95,0),(hair,[.018,.02,.017,1],.86,0),(skin,[.31,.33,.28,1],.82,0)]:
    manifest['materials'].append(dict(name=name,color=color,roughness=rough,metallic=metal,alphaClip=False,albedo='',normal='',mask=''))
(ROOT/'Audit/atmaja-art-manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
print('ATMAJA_ORIGINAL_DETAILS_EXPORTED')
