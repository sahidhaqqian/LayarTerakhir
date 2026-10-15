"""Original, unbranded city-kos props. Blender 4: blender -b --python Tools/author_compact_kos.py.
Static meshes only; no cloth simulation or new gameplay dependencies.
"""
import bpy, math, sys
from pathlib import Path

OUT=Path(__file__).resolve().parents[1]/'Assets/_LayarTerakhir/Art/Original/Models'

def clear():
    bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)

def finish(obj,name,slot):
    obj.name=name;obj.data.materials.append(bpy.data.materials.get(slot) or bpy.data.materials.new(slot));return obj

def box(name,loc,size,slot,bevel=.003):
    bpy.ops.mesh.primitive_cube_add(size=1,location=loc);obj=bpy.context.object;obj.scale=size
    bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
    if bevel:
        mod=obj.modifiers.new('Worn rounded edge','BEVEL');mod.width=bevel;mod.segments=2;bpy.ops.object.modifier_apply(modifier=mod.name)
    return finish(obj,name,slot)

def cyl(name,loc,radius,depth,slot,rotation=(0,0,0),verts=16):
    bpy.ops.mesh.primitive_cylinder_add(vertices=verts,radius=radius,depth=depth,location=loc,rotation=rotation)
    return finish(bpy.context.object,name,slot)

def tube(name,points,radius,slot):
    data=bpy.data.curves.new(name,'CURVE');data.dimensions='3D';data.bevel_depth=radius;data.bevel_resolution=1
    spline=data.splines.new('POLY');spline.points.add(len(points)-1)
    for point,value in zip(spline.points,points):point.co=(*value,1)
    obj=bpy.data.objects.new(name,data);bpy.context.collection.objects.link(obj);return finish(obj,name,slot)

def export(name,unwrap=True):
    for obj in list(bpy.context.scene.objects):
        if obj.type=='CURVE':
            bpy.ops.object.select_all(action='DESELECT');obj.select_set(True);bpy.context.view_layer.objects.active=obj;bpy.ops.object.convert(target='MESH')
    groups={}
    for obj in list(bpy.context.scene.objects):groups.setdefault(obj.data.materials[0].name,[]).append(obj)
    for slot,objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for obj in objects:obj.select_set(True)
        bpy.context.view_layer.objects.active=objects[0]
        if len(objects)>1:bpy.ops.object.join()
        bpy.context.object.name=slot
    if unwrap:
        for obj in list(bpy.context.scene.objects):
            bpy.ops.object.select_all(action='DESELECT');obj.select_set(True);bpy.context.view_layer.objects.active=obj
            bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.uv.smart_project(island_margin=.015);bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.fbx(filepath=str(OUT/(name+'.fbx')),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)

if '--stored-only' in sys.argv:
    # Folded portable backdrop: telescopic uprights lowered, cloth gathered in accordion pleats.
    # A genuine storage state, not a full-width screen squeezed non-uniformly in Unity.
    clear()
    for x in [-.22,.22]:
        cyl('Collapsed upright',(x,0,.80),.012,1.56,'BackdropStand')
        cyl('Locking collar',(x,0,.89),.018,.045,'BackdropStand')
        box('Closed low foot',(x,0,.018),(.045,.23,.024),'BackdropStand',.004)
    cyl('Shortened crossbar',(0,0,1.57),.012,.52,'BackdropStand',(0,math.pi/2,0))
    verts=[];faces=[];nx=32;nz=20
    for j in range(nz+1):
        v=j/nz
        for i in range(nx+1):
            u=i/nx
            y=.035*math.cos(u*math.pi*16)+.005*math.sin(v*9+u*13)
            verts.append(((u-.5)*.42,y,.32+v*1.23-.018*math.sin(u*math.pi)*(1-v)))
    for j in range(nz):
        for i in range(nx):
            a=j*(nx+1)+i;faces.append((a,a+1,a+nx+2,a+nx+1))
    mesh=bpy.data.meshes.new('Accordion folded cloth');mesh.from_pydata(verts,[],faces);mesh.update()
    obj=bpy.data.objects.new('GreenFabric',mesh);bpy.context.collection.objects.link(obj);finish(obj,obj.name,'GreenFabric')
    solid=obj.modifiers.new('Real cloth backface','SOLIDIFY');solid.thickness=.002;bpy.context.view_layer.objects.active=obj;bpy.ops.object.modifier_apply(modifier=solid.name)
    for polygon in obj.data.polygons:polygon.use_smooth=True
    for z in [.72,1.26]:box('Cloth storage strap',(0,-.041,z),(.43,.012,.035),'BackdropStand',.004)
    for x in [-.18,.18]:box('Retained clamp',(x,0,1.56),(.03,.04,.05),'BackdropStand',.003)
    export('KosStoredGreenScreen')
    print('STORED_GREEN_SCREEN_EXPORTED',OUT)
    raise SystemExit

clear()
for x in [-.80,.80]:
    cyl('Telescoping upright',(x,0,1.005),.012,1.99,'BackdropStand')
    cyl('Height collar',(x,0,1.18),.017,.045,'BackdropStand')
    box('Low T foot',(x,0,.012),(.11,.42,.024),'BackdropStand',.008)
    for y in [-.19,.19]:box('Rubber foot',(x,y,.013),(.12,.045,.026),'BackdropStand',.008)
cyl('Crossbar',(0,0,2.00),.012,1.70,'BackdropStand',(0,math.pi/2,0))
verts=[];faces=[];nx=36;nz=24
for j in range(nz+1):
    v=j/nz
    for i in range(nx+1):
        u=i/nx;x=(u-.5)*1.42
        depth=.010*math.sin(u*math.pi*18)*(.3+.7*v)+.006*math.sin(u*11+v*9)
        verts.append((x,depth,.20+v*1.77-.018*math.sin(u*math.pi)*(1-v)))
for j in range(nz):
    for i in range(nx):
        a=j*(nx+1)+i;faces.append((a,a+1,a+nx+2,a+nx+1))
mesh=bpy.data.meshes.new('Creased green fabric');mesh.from_pydata(verts,[],faces);mesh.update()
obj=bpy.data.objects.new('GreenFabric',mesh);bpy.context.collection.objects.link(obj);finish(obj,obj.name,'GreenFabric')
solid=obj.modifiers.new('Real cloth backface','SOLIDIFY');solid.thickness=.002;bpy.context.view_layer.objects.active=obj;bpy.ops.object.modifier_apply(modifier=solid.name)
for polygon in obj.data.polygons:polygon.use_smooth=True
for x in [-.68,-.34,0,.34,.68]:box('Spring clamp',(x,0,1.99),(.035,.025,.055),'BackdropStand',.003)
for x in [-.71,.71]:tube('Sewn side hem',[(x,.002,.2),(x,.002,1.97)],.004,'GreenFabric')
export('KosStreamerGreenScreen')

clear()
box('Webcam body',(0,0,.028),(.085,.035,.036),'WebcamShell',.009)
box('Monitor clip',(0,.012,0),(.035,.044,.018),'WebcamShell',.004)
cyl('Lens bezel',(0,-.020,.028),.012,.008,'WebcamShell',(math.pi/2,0,0),24)
cyl('Glass lens',(0,-.0245,.028),.008,.002,'WebcamLens',(math.pi/2,0,0),24)
export('KosBudgetWebcam')

clear()
for side in [-1,1]:
    x=side*.055
    tube('Dropped cotton sock',[(x,-.08,.018),(x,.012,.027),(x+side*.012,.075,.048)],.024,'SockCotton')
    cyl('Rolled cuff',(x+side*.012,.075,.049),.029,.018,'SockRib',verts=20)
export('KosDroppedSocks')

clear()
# Folded newspaper has a creased page and coarse printed columns, not a fictional plot document.
verts=[];faces=[];nx=12;ny=10
for j in range(ny+1):
    for i in range(nx+1):
        u=i/nx;v=j/ny
        verts.append(((u-.5)*.38,(v-.5)*.29,.002+.006*abs(u-.5)+.009*max(0,(u-.85)/.15)*v))
for j in range(ny):
    for i in range(nx):
        a=j*(nx+1)+i;faces.append((a,a+1,a+nx+2,a+nx+1))
mesh=bpy.data.meshes.new('Folded newsprint');mesh.from_pydata(verts,[],faces);mesh.update();obj=bpy.data.objects.new('Newsprint',mesh);bpy.context.collection.objects.link(obj);finish(obj,obj.name,'Newsprint')
box('Masthead',(0,.105,.006),(.33,.026,.0005),'NewsprintInk',0)
box('Section rule',(0,.080,.006),(.33,.002,.0005),'NewsprintInk',0)
for column in range(3):
    for row in range(17):
        width=.085 if row%5 else .067
        box('Small type',(-.113+column*.113,.062-row*.010,.007), (width,.0016,.0004),'NewsprintInk',0)
export('KosOldNewspaper')

clear()
box('Folded tee',(0,0,.014),(.29,.24,.028),'ShirtCotton',.018)
for x in [-.11,.11]:box('Folded sleeve',(x,.05,.026),(.065,.12,.018),'ShirtCotton',.014).rotation_euler[2]=x*1.2
tube('Neck rib',[(.042*math.cos(a),.09+.035*math.sin(a),.031) for a in [i*math.pi/16 for i in range(17)]],.004,'ShirtRib')
tube('Fold seam',[(-.13,-.1,.03),(0,-.103,.032),(.13,-.1,.03)],.0015,'ShirtRib')
export('KosFoldedTshirt')
print('COMPACT_KOS_PROPS_EXPORTED',OUT)
