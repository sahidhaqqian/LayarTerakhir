"""Original Sri Atmaja character, articulated rigid meshes; no external model or service.
blender -b --python Tools/author_atmaja_character.py
Metres, Blender Z-up, facing -Y; export uses the project's established FBX axes.
"""
from pathlib import Path
import json, math, random
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/Art/Original/Models/SriAtmajaCharacter.fbx'
bpy.ops.wm.read_factory_settings(use_empty=True)
random.seed(1998)
materials = {}
for name, color in {
    'Sri_Cloth': (.60, .57, .49, 1), 'Sri_Batik': (.10, .075, .065, 1),
    'Sri_Skin': (.43, .39, .32, 1), 'Sri_Hair': (.012, .009, .008, 1),
    'Sri_Eye': (.51, .48, .39, 1), 'Sri_Wound': (.13, .052, .04, 1),
    'Sri_Rust': (.24, .10, .055, 1), 'Sri_Rope': (.24, .17, .10, 1)
}.items():
    material = bpy.data.materials.new(name); material.diffuse_color = color; materials[name] = material

def joint(name, parent, at):
    obj = bpy.data.objects.new(name, None); bpy.context.collection.objects.link(obj)
    obj.location = at
    if parent:
        bpy.context.view_layer.update(); obj.parent = parent; obj.matrix_parent_inverse = parent.matrix_world.inverted()
    bpy.context.view_layer.update()
    return obj

def finish(obj, parent, material):
    obj.data.materials.append(materials[material])
    bpy.context.view_layer.update(); obj.parent = parent; obj.matrix_parent_inverse = parent.matrix_world.inverted()
    for polygon in obj.data.polygons: polygon.use_smooth = True
    return obj

def mesh(name, vertices, faces, parent, material, uvs=None):
    data = bpy.data.meshes.new(name); data.from_pydata(vertices, [], faces); data.update()
    obj = bpy.data.objects.new(name, data); bpy.context.collection.objects.link(obj)
    uv = data.uv_layers.new(name='UVMap')
    for polygon in data.polygons:
        for loop in polygon.loop_indices:
            point = vertices[data.loops[loop].vertex_index]
            uv.data[loop].uv = (point[0] * 2 + .5, point[2] * 1.4)
    return finish(obj, parent, material)

def ellipsoid(name, at, scale, parent, material, segments=16, rings=8):
    bpy.ops.mesh.primitive_uv_sphere_add(segments=segments, ring_count=rings, location=at)
    obj = bpy.context.object; obj.name = name; obj.scale = scale
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    return finish(obj, parent, material)

def tube(name, points, radius, parent, material, sides=8):
    points = [Vector(p) for p in points]; vertices=[]; faces=[]
    for index, point in enumerate(points):
        direction=(points[min(index+1,len(points)-1)]-points[max(0,index-1)]).normalized()
        cross=direction.cross(Vector((0,0,1)))
        if cross.length < .01: cross=direction.cross(Vector((0,1,0)))
        cross.normalize(); other=direction.cross(cross).normalized()
        r=radius[index] if isinstance(radius,list) else radius
        for side in range(sides):
            angle=side/sides*math.tau; vertices.append(tuple(point+(cross*math.cos(angle)+other*math.sin(angle))*r))
    for ring in range(len(points)-1):
        for side in range(sides):
            a=ring*sides+side;b=ring*sides+(side+1)%sides;faces.append((a,b,b+sides,a+sides))
    faces.extend([tuple(reversed(range(sides))),tuple((len(points)-1)*sides+i for i in range(sides))])
    return mesh(name,vertices,faces,parent,material)

def cloth(name, profiles, parent, material, sides=32, tattered=False):
    vertices=[];faces=[]
    for row,(z,rx,ry,cy) in enumerate(profiles):
        for side in range(sides):
            angle=side/sides*math.tau
            fold=1+(.025 if material=='Sri_Cloth' else .04)*math.sin(angle*10+row*.5)
            hem=(.014*math.sin(side*4.7)+.012*math.sin(side*1.7)) if tattered and row==0 else 0
            vertices.append((rx*math.cos(angle)*fold,cy+ry*math.sin(angle)*fold,z+hem))
    for row in range(len(profiles)-1):
        for side in range(sides):
            a=row*sides+side;b=row*sides+(side+1)%sides;faces.append((a,b,b+sides,a+sides))
    return mesh(name,vertices,faces,parent,material)

root=joint('SriAtmaja',None,(0,0,0));body=joint('Body',root,(0,0,.88))
cloth('LongBatikSkirt',[(.07,.19,.145,0),(.16,.18,.14,0),(.35,.165,.13,.005),(.55,.15,.13,.01),(.77,.165,.13,.005),(.9,.15,.11,0)],body,'Sri_Batik',tattered=True)
cloth('Kebaya',[(.74,.175,.13,-.015),(.82,.17,.115,-.015),(.96,.135,.11,0),(1.10,.125,.095,0),(1.20,.15,.10,-.015),(1.28,.18,.11,-.015),(1.35,.185,.09,0),(1.40,.12,.065,0)],body,'Sri_Cloth',sides=24,tattered=True)
# Open collar and cloth seams follow the body instead of stacked rings.
tube('KebayaCollar',[(-.09,-.065,1.40),(-.06,-.115,1.32),(0,-.13,1.21),(.06,-.115,1.32),(.09,-.065,1.40)],.008,body,'Sri_Batik',6)
tube('KebayaFrontSeam',[(0,-.13,.77),(0,-.12,.95),(0,-.106,1.10),(0,-.13,1.21)],.004,body,'Sri_Batik',4)
for z in [.86,.94,1.02,1.10,1.18]:ellipsoid('OldClasp',(0,-.13,z),(.007,.005,.008),body,'Sri_Rope',8,4)
for side in [-1,1]:
    arm=joint('UpperArm_L' if side<0 else 'UpperArm_R',body,(side*.18,0,1.34))
    elbow=joint('Forearm_L' if side<0 else 'Forearm_R',arm,(side*.23,-.035,1.07))
    hand=joint('Hand_L' if side<0 else 'Hand_R',elbow,(side*.27,-.11,.79))
    tube('KebayaSleeve',[(side*.18,0,1.34),(side*.22,-.02,1.22),(side*.23,-.035,1.07)],[.068,.058,.047],arm,'Sri_Cloth',12)
    tube('Forearm',[(side*.23,-.035,1.07),(side*.255,-.075,.93),(side*.27,-.11,.80)],[.043,.034,.025],elbow,'Sri_Skin',12)
    ellipsoid('Palm',(side*.27,-.125,.76),(.038,.024,.065),hand,'Sri_Skin',12,6)
    for finger in range(4):
        x=side*.27+(finger-1.5)*.018;length=[.061,.083,.088,.072][finger]
        tube('Finger',[(x,-.13,.72),(x,-.15,.72-length*.55),(x,-.16,.72-length)],[.009,.007,.004],hand,'Sri_Skin',6)
        ellipsoid('DarkFingernail',(x,-.166,.724-length),(.005,.002,.014),hand,'Sri_Hair',8,4)
    tube('Thumb',[(side*.301,-.12,.78),(side*.326,-.145,.744),(side*.324,-.16,.722)],[.012,.009,.006],hand,'Sri_Skin',6)
    leg=joint('Leg_L' if side<0 else 'Leg_R',body,(side*.075,0,.8))
    tube('Calf',[(side*.075,.005,.45),(side*.08,-.005,.23),(side*.08,-.025,.07)],[.047,.035,.025],leg,'Sri_Skin',10)
    ellipsoid('BareFoot',(side*.08,-.055,.045),(.046,.108,.035),leg,'Sri_Skin',12,6)
    for toe in range(4):ellipsoid('Toe',(side*.08+(toe-1.5)*.02,-.14,.034),(.012,.022,.015),leg,'Sri_Skin',8,4)
    if side>0:
        tool=joint('Shears',hand,(side*.27,-.11,.79))
        tube('RustHandleA',[(.27,-.11,.77),(.31,-.13,.66),(.32,-.11,.53)],.016,tool,'Sri_Hair',8)
        tube('RustHandleB',[(.28,-.08,.77),(.37,-.08,.66),(.32,-.09,.53)],.016,tool,'Sri_Hair',8)
        tube('ShearsBladeA',[(.32,-.11,.54),(.35,-.11,.32),(.42,-.11,.035)],[.023,.019,.003],tool,'Sri_Rust',6)
        tube('ShearsBladeB',[(.32,-.08,.54),(.33,-.08,.32),(.395,-.08,.025)],[.022,.018,.003],tool,'Sri_Rust',6)
        ellipsoid('ShearsPivot',(.32,-.095,.54),(.033,.025,.033),tool,'Sri_Rope',12,6)
# Visible neck, ligature and a complete face tilted over the left shoulder.
tube('Neck',[(0,0,1.36),(0,0,1.47)],[.047,.039],body,'Sri_Skin',16)
tube('BruisedLigature',[(-.04,-.018,1.425),(-.025,-.042,1.425),(0,-.047,1.425),(.032,-.035,1.425),(.046,0,1.425)],.010,body,'Sri_Wound',8)
head=joint('Head',body,(0,0,1.45))
face=ellipsoid('Face',(0,-.006,1.565),(.089,.074,.125),head,'Sri_Skin',32,18)
# Shape a jaw and cheekbones rather than leaving the head as a smooth sphere.
for vertex in face.data.vertices:
    z=vertex.co.z
    if z<-.035:vertex.co.x*=.75+max(0,z+.125)*1.8
    if vertex.co.y<-.02 and -.02<z<.055:vertex.co.y-=.004
for side in [-1,1]:
    ellipsoid('Ear',(side*.087,.002,1.57),(.018,.015,.031),head,'Sri_Skin',12,6)
    ellipsoid('EyeSocket',(side*.037,-.069,1.59),(.029,.012,.018),head,'Sri_Wound',16,8)
    ellipsoid('DeadEye',(side*.037,-.078,1.589),(.018,.007,.009),head,'Sri_Eye',16,8)
    ellipsoid('Pupil',(side*.037,-.0845,1.588),(.005,.002,.006),head,'Sri_Hair',12,6)
    tube('Brow',[(side*.015,-.072,1.611),(side*.036,-.076,1.616),(side*.061,-.067,1.610)],.004,head,'Sri_Hair',6)
    tube('LowerLid',[(side*.019,-.078,1.579),(side*.037,-.080,1.576),(side*.055,-.075,1.579)],.003,head,'Sri_Skin',6)
tube('NoseBridge',[(0,-.070,1.599),(0,-.083,1.575),(0,-.099,1.557)],[.008,.010,.013],head,'Sri_Skin',10)
ellipsoid('NoseTip',(0,-.098,1.554),(.018,.013,.011),head,'Sri_Skin',12,6)
for side in [-1,1]:ellipsoid('Nostril',(side*.012,-.107,1.550),(.005,.003,.003),head,'Sri_Wound',8,4)
ellipsoid('Mouth',(0,-.075,1.523),(.028,.006,.011),head,'Sri_Hair',16,8)
tube('LowerLip',[(-.026,-.076,1.52),(0,-.082,1.513),(.025,-.076,1.52)],.004,head,'Sri_Wound',8)
# Hair volume and tapered wet locks, with the front face partly exposed.
ellipsoid('HairScalp',(0,.015,1.626),(.097,.078,.082),head,'Sri_Hair',24,12)
for index in range(22):
    angle=math.pi*.10+index/21*math.pi*1.80
    x=.09*math.cos(angle);y=.073*math.sin(angle)
    if y<-.035 and abs(x)<.04:continue
    length=.29+(index%4)*.045
    points=[(x,y,1.65),(x*1.08,y*1.1,1.53),(x*1.12+.012*math.sin(index),y+.006,1.41),(x*1.04,y+.015,1.65-length)]
    tube('WetHairLock',points,[.018,.019,.013,.001],head,'Sri_Hair',5)
# Apply the scripted broken-neck resting pose after creating the face/hair.
head.rotation_euler=(math.radians(3),math.radians(-79),math.radians(-4))
bpy.context.view_layer.update()
# Merge only meshes sharing both a rigid joint and material. Hierarchy stays articulated.
groups={}
for obj in list(bpy.context.scene.objects):
    if obj.type=='MESH':groups.setdefault((obj.parent.name,obj.data.materials[0].name),[]).append(obj)
for (parent_name,material),objects in groups.items():
    bpy.ops.object.select_all(action='DESELECT')
    for obj in objects:obj.select_set(True)
    bpy.context.view_layer.objects.active=objects[0]
    if len(objects)>1:bpy.ops.object.join()
    bpy.context.object.name=parent_name+'__'+material
bpy.ops.object.select_all(action='SELECT')
bpy.ops.export_scene.fbx(filepath=str(OUT),object_types={'MESH','EMPTY'},bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='STRIP',add_leaf_bones=False)
triangles=sum(sum(len(p.vertices)-2 for p in obj.data.polygons) for obj in bpy.context.scene.objects if obj.type=='MESH')
(ROOT/'Audit/atmaja-character-authoring.json').write_text(json.dumps({'asset':OUT.relative_to(ROOT).as_posix(),'method':'Original Blender geometry and articulated rigid hierarchy; no downloaded character or image input','triangles':triangles,'meshGroups':len(groups),'notes':'Ivory kebaya, batik skirt, tilted neck, modeled face, hands, wet hair, integrated garden shears. Motion is procedural, not motion capture.'},indent=2)+'\n')
print('SRI_ATMAJA_CHARACTER',triangles,'triangles;',len(groups),'rigid material groups')
