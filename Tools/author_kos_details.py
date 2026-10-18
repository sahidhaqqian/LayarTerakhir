"""Original unbranded kos props. Run with Blender 4: blender -b --python Tools/author_kos_details.py.
Exports static FBX meshes; no gameplay or animations. Dimensions are in metres.
"""
import bpy,math
from pathlib import Path
from mathutils import Vector,Matrix
OUT=Path(__file__).resolve().parents[1]/'Assets/_LayarTerakhir/assets/Art/Original/Models'
OUT.mkdir(parents=True,exist_ok=True)
def clear():
 bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
def mat(name,color):
 m=bpy.data.materials.get(name) or bpy.data.materials.new(name);m.diffuse_color=(*color,1);return m
metal=mat('Metal',(.16,.14,.11));plastic=mat('Plastic',(.075,.08,.075));fabric=mat('Fabric',(.13,.18,.13));ceramic=mat('Ceramic',(.72,.69,.58));water=mat('WaterPlastic',(.08,.25,.32));brass=mat('Brass',(.42,.3,.12));coffee=mat('Coffee',(.025,.012,.006))
def finish(o,name,m):
 o.name=name;o.data.materials.clear();o.data.materials.append(m)
 for p in o.data.polygons:p.use_smooth=True
 return o
def cube(name,loc,scale,m,bevel=.005):
 bpy.ops.mesh.primitive_cube_add(size=1,location=loc);o=bpy.context.object;o.scale=scale;bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
 b=o.modifiers.new('Soft manufactured edges','BEVEL');b.width=bevel;b.segments=3
 bpy.ops.object.modifier_apply(modifier=b.name)
 o.modifiers.new('Weighted corner normals','WEIGHTED_NORMAL')
 return finish(o,name,m)
def cyl(name,loc,r,depth,m,rotation=(0,0,0),vertices=24):
 bpy.ops.mesh.primitive_cylinder_add(vertices=vertices,radius=r,depth=depth,location=loc,rotation=rotation);return finish(bpy.context.object,name,m)
def curve(name,points,r,m):
 d=bpy.data.curves.new(name,'CURVE');d.dimensions='3D';d.bevel_depth=r;d.bevel_resolution=2
 s=d.splines.new('POLY');s.points.add(len(points)-1)
 for p,v in zip(s.points,points):p.co=(*v,1)
 o=bpy.data.objects.new(name,d);bpy.context.collection.objects.link(o);o.data.materials.append(m);return o
def lathe(name,profile,m,segments=40):
 verts=[];faces=[]
 for j,(r,z) in enumerate(profile):
  for i in range(segments):
   a=i/segments*math.tau;rr=r*(1+.004*math.sin(a*5+z*24));verts.append((rr*math.cos(a),rr*math.sin(a),z))
 for j in range(len(profile)-1):
  for i in range(segments):
   n=(i+1)%segments;a=j*segments+i;b=j*segments+n;faces.append((a,b,b+segments,a+segments))
 mesh=bpy.data.meshes.new(name);mesh.from_pydata(verts,[],faces);mesh.update();o=bpy.data.objects.new(name,mesh);bpy.context.collection.objects.link(o);finish(o,name,m)
 return o
def export(name):
 for o in list(bpy.context.scene.objects):
  bpy.context.view_layer.objects.active=o;o.select_set(True)
  if o.type=='CURVE':bpy.ops.object.convert(target='MESH')
 for o in list(bpy.context.scene.objects):
  if o.type=='MESH':
   bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o
   bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.uv.smart_project(island_margin=.02);bpy.ops.object.mode_set(mode='OBJECT')
 bpy.ops.object.select_all(action='SELECT')
 bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
 bpy.ops.export_scene.fbx(filepath=str(OUT/(name+'.fbx')),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',add_leaf_bones=False)
clear()
lathe('Chipped enamel mug',[(0,0),(.034,0),(.041,.008),(.044,.08),(.043,.093),(.039,.094),(.038,.086),(.036,.014),(0,.014)],ceramic)
pts=[(.041+.026*math.cos(a),0,.05+.035*math.sin(a)) for a in [(-math.pi*.5)+i/24*math.pi for i in range(25)]]
curve('Loop handle',pts,.006,ceramic);cyl('Dark coffee', (0,0,.071),.036,.001,coffee);export('EnamelMug')
clear()
profile=[(0,0),(.115,0),(.145,.015),(.15,.04)]
for z in [.07,.13,.19,.25,.31]:profile.extend([(.15,z),(.145,z+.014),(.15,z+.028)])
profile.extend([(.15,.36),(.12,.41),(.055,.44),(.046,.46),(.046,.49),(0,.49)])
lathe('Refillable water gallon',profile,water);cyl('Screw cap',(0,0,.49),.051,.035,plastic);export('RefillWaterGallon')
clear()
cube('Steel computer case',(0,0,.2),(.19,.38,.4),plastic,.012)
cube('Front panel',(0,-.195,.2),(.174,.012,.368),metal,.005)
for z in [.12+i*.012 for i in range(11)]:cube('Air vent',(0,-.204,z),(.13,.003,.004),plastic,.001)
for z in [.3,.335]:cube('Drive bay',(0,-.206,z),(.135,.005,.022),plastic,.001)
cyl('Power switch',(.055,-.21,.275),.008,.004,brass,(math.pi/2,0,0))
for x in [-.026,-.005]:cube('USB socket',(x,-.21,.275),(.012,.003,.005),plastic,.001)
for x in [-.065,.065]:
 for y in [-.13,.13]:cyl('Rubber foot',(x,y,-.005),.016,.012,plastic)
export('OldPCTower')
clear()
cube('Brass padlock body',(0,0,.025),(.045,.022,.048),brass,.004)
pts=[(-.015,0,.045),(-.015,0,.065)]+[(.015*math.cos(a),0,.065+.015*math.sin(a)) for a in [math.pi-i/16*math.pi for i in range(17)]]+[(.015,0,.045)]
curve('Steel shackle',pts,.003,metal)
for x in [-.012,0,.012]:cyl('Combination wheel',(x,-.013,.023),.007,.009,metal,(0,math.pi/2,0))
export('CombinationPadlock')
clear()
cube('Phone case',(0,0,.006),(.073,.145,.012),plastic,.008)
cube('Glass display',(0,0,.0125),(.063,.125,.002),metal,.005)
cyl('Front camera',(.021,.062,.014),.002,.001,plastic)
for x in [-.006,-.003,0,.003,.006]:cube('Earpiece',(x,.065,.014),(.0015,.001,.001),metal,.0001)
export('Smartphone')
for o in list(bpy.context.scene.objects):o.matrix_world=Matrix.Rotation(math.pi/2,4,'X')@o.matrix_world
export('SmartphoneUpright')
clear()
# Sewn jacket surfaces with a collar notch and folds. No simulation or runtime script.
for back in [False,True]:
 verts=[];faces=[];nu=18;nv=26
 for j in range(nv+1):
  v=j/nv;z=.06+v*.55;width=.17+.035*v
  for i in range(nu+1):
   u=i/nu*2-1;x=u*width;collar=max(0,1-abs(u)*3.5)*(.055 if not back else .015)*v**12
   y=(-1 if back else 1)*(.035+.015*math.cos(u*2.7))+.009*math.sin(u*17+v*4)+.004*math.sin(v*31+u*5)
   verts.append((x,y,z-collar))
 for j in range(nv):
  for i in range(nu):
   a=j*(nu+1)+i;faces.append((a,a+1,a+nu+2,a+nu+1) if back else (a+nu+1,a+nu+2,a+1,a))
 mesh=bpy.data.meshes.new('Sewn fabric');mesh.from_pydata(verts,[],faces);mesh.update();o=bpy.data.objects.new('Back cloth' if back else 'Front cloth',mesh);bpy.context.collection.objects.link(o);finish(o,o.name,fabric)
for side in [-1,1]:
 a=Vector((side*.18,0,.565));b=Vector((side*.32,0,.16));d=b-a
 bpy.ops.mesh.primitive_cone_add(vertices=16,radius1=.044,radius2=.068,depth=d.length,location=(a+b)/2);o=bpy.context.object;o.rotation_euler=d.to_track_quat('Z','Y').to_euler();finish(o,'Sleeve',fabric)
curve('Zipper',[(0,.057,.08),(0,.059,.49),(0,.055,.55)],.0017,brass)
for side in [-1,1]:curve('Pocket seam',[(side*.06,.057,.20),(side*.13,.052,.26)],.0014,metal)
curve('Hanger hook',[(0,0,.605),(0,0,.645),(.017,0,.66),(.025,0,.65),(.02,0,.64)],.002,metal)
export('HangingJacket')
print('ORIGINAL_KOS_PROPS_EXPORTED',OUT)
