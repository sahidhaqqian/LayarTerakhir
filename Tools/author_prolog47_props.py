"""Original, unbranded Prolog repair props in metres. Blender, static FBX only.

Run: blender -b --python Tools/author_prolog47_props.py
Surfaces are authored meshes (hollow porcelain, sewn cloth, curved paper), not
Unity placeholder assemblies. Existing characters and their grip profiles are untouched.
"""
from pathlib import Path
import hashlib
import json
import math
import os
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Original/Models/Prolog47'
OUT.mkdir(parents=True, exist_ok=True)
RECORDS = []


def clear():
    bpy.ops.wm.read_factory_settings(use_empty=True)


def material(name):
    return bpy.data.materials.get(name) or bpy.data.materials.new(name)


def mesh(name, vertices, faces, mat, smooth=True):
    data = bpy.data.meshes.new(name)
    data.from_pydata(vertices, [], faces)
    data.update()
    obj = bpy.data.objects.new(name, data)
    bpy.context.collection.objects.link(obj)
    data.materials.append(material(mat))
    for face in data.polygons:
        face.use_smooth = smooth
    return obj


def rounded(name, position, size, mat, bevel=.012):
    bpy.ops.mesh.primitive_cube_add(size=1, location=position)
    obj = bpy.context.object
    obj.name = name
    obj.scale = size
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    obj.data.materials.append(material(mat))
    mod = obj.modifiers.new('Manufactured rounded edge', 'BEVEL')
    mod.width = bevel
    mod.segments = 4
    bpy.ops.object.modifier_apply(modifier=mod.name)
    obj.modifiers.new('Face weighted normals', 'WEIGHTED_NORMAL')
    return obj


def tube(name, points, radius, mat, cyclic=False):
    data = bpy.data.curves.new(name, 'CURVE')
    data.dimensions = '3D'
    data.bevel_depth = radius
    data.bevel_resolution = 2
    spline = data.splines.new('POLY')
    spline.points.add(len(points)-1)
    for point, value in zip(spline.points, points):
        point.co = (*value, 1)
    spline.use_cyclic_u = cyclic
    obj = bpy.data.objects.new(name, data)
    bpy.context.collection.objects.link(obj)
    obj.data.materials.append(material(mat))
    return obj


def export(name):
    selected=os.environ.get("PROLOG_PROP_FILTER", "").split(",")
    if selected!=[""] and name not in selected:return
    for obj in list(bpy.context.scene.objects):
        if obj.type == 'CURVE':
            bpy.ops.object.select_all(action='DESELECT')
            obj.select_set(True)
            bpy.context.view_layer.objects.active = obj
            bpy.ops.object.convert(target='MESH')
    for obj in list(bpy.context.scene.objects):
        if obj.type != 'MESH':
            continue
        bpy.ops.object.select_all(action='DESELECT')
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        if not obj.data.uv_layers:
            bpy.ops.object.mode_set(mode='EDIT')
            bpy.ops.mesh.select_all(action='SELECT')
            bpy.ops.uv.smart_project(island_margin=.02)
            bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.fbx(filepath=str(OUT / (name+'.fbx')), object_types={'MESH'},
                             bake_anim=False, axis_forward='-Z', axis_up='Y',
                             add_leaf_bones=False, path_mode='STRIP', use_mesh_modifiers=True)
    path = OUT / (name+'.fbx')
    RECORDS.append(dict(name=name, path=path.relative_to(ROOT).as_posix(),
                        sha256=hashlib.sha256(path.read_bytes()).hexdigest(),
                        triangles=sum(len(p.vertices)-2 for o in bpy.context.scene.objects if o.type=='MESH' for p in o.data.polygons)))


def pillow(name, center, width, length, height):
    # Closed sewn cushion: domed faces pinch into a continuous piped seam.
    vertices, faces = [], []
    nu, nv = 24, 16
    for side in (-1, 1):
        for j in range(nv+1):
            v=j/nv*2-1
            for i in range(nu+1):
                u=i/nu*2-1
                dome=max(0, (1-u*u)*(1-v*v))**.45
                x=u*width/2*(1-.08*abs(v)**10)
                y=v*length/2*(1-.10*abs(u)**10)
                z=side*(.006+height/2*dome)+.003*math.sin(u*17+v*7)*dome
                vertices.append((center[0]+x,center[1]+y,center[2]+z))
        offset=0 if side==-1 else (nu+1)*(nv+1)
        for j in range(nv):
            for i in range(nu):
                a=offset+j*(nu+1)+i
                f=(a,a+1,a+nu+2,a+nu+1)
                faces.append(tuple(reversed(f)) if side==-1 else f)
    count=(nu+1)*(nv+1)
    border=list(range(nu+1))+[j*(nu+1)+nu for j in range(1,nv+1)]+[nv*(nu+1)+i for i in range(nu-1,-1,-1)]+[j*(nu+1) for j in range(nv-1,0,-1)]
    for a,b in zip(border,border[1:]+border[:1]):
        faces.append((a,b,b+count,a+count))
    mesh(name,vertices,faces,'Repair_Linen')
    tube(name+' sewn piping',[(vertices[i][0],vertices[i][1],center[2]) for i in border],.002,'Repair_Linen',True)


def bed(name, width, double=False):
    clear()
    length=2.06
    rounded('Recessed bed frame',(0,0,.27),(width+.09,length+.07,.16),'Repair_BedWood',.018)
    rounded('Solid headboard',(0,-1.06,.52),(width+.10,.065,.92),'Repair_BedWood',.025)
    for x in (-width*.4,width*.4):
        for y in (-.86,.86):
            rounded('Tapered timber foot',(x,y,.135),(.075,.075,.27),'Repair_BedWood',.009)
    rounded('Quilted mattress',(0,0,.44),(width,2.03,.23),'Repair_Mattress',.075)
    for z in (.36,.52):
        points=[]
        for i in range(96):
            a=i/96*math.tau
            points.append((math.copysign(abs(math.cos(a))**.22,a and math.cos(a) or 1)*(width/2-.02),math.copysign(abs(math.sin(a))**.22,math.sin(a))*.995,z))
        tube('Mattress bound seam',points,.0025,'Repair_Linen',True)
    if double:
        for x in (-width*.25,width*.25):pillow('Full size sleeping pillow',(x,-.67,.61),.65,.43,.15)
    else:pillow('Full size sleeping pillow',(0,-.67,.61),.68,.43,.15)
    # Duvet drapes over each long edge and foot, preserving a soft raised top.
    vertices,faces=[],[]
    nx,ny=56,56
    for j in range(ny+1):
        v=j/ny;y=-.36+v*1.51
        foot=max(0,(y-1.025)/.125)
        for i in range(nx+1):
            u=i/nx*2-1;x=u*(width/2+.16)
            edge=max(0,(abs(x)-width/2-.012)/.148)
            z=.595-.29*edge**.6-.24*foot**.65
            z+=.006*math.sin(x*19+y*8)+.003*math.sin(y*29-x*6)
            z+=.002*(math.cos(i/7*math.pi)**14+math.cos(j/7*math.pi)**14)
            vertices.append((x,y,z))
    for j in range(ny):
        for i in range(nx):
            a=j*(nx+1)+i;faces.append((a,a+1,a+nx+2,a+nx+1))
    obj=mesh('Draped stitched cotton blanket',vertices,faces,'Repair_Blanket')
    solid=obj.modifiers.new('Cotton thickness','SOLIDIFY');solid.thickness=.008
    export(name)


bed('SingleBedSet',1.08)
bed('DoubleBedSet',1.6,True)

clear()
# Open notebook has separate curved leaves, exposed page edges and a recessed spine.
for side in (-1,1):
    cover=rounded('Worn cloth cover',(side*.076,0,.006),(.151,.218,.008),'Repair_BookCloth',.004)
    for layer in range(5):
        vertices,faces=[],[]
        for j in range(17):
            y=-.103+j/16*.206
            for i in range(25):
                t=i/24;x=side*(.004+t*.144)
                z=.013+layer*.0008+.012*math.sin(t*math.pi)*(.85+.15*math.cos(y*20))+.004*t*t
                vertices.append((x,y,z))
        for j in range(16):
            for i in range(24):
                a=j*25+i;f=(a,a+1,a+26,a+25);faces.append(tuple(reversed(f)) if side<0 else f)
        mesh('Curved ruled leaf',vertices,faces,'Repair_Paper')
tube('Bound spine',[(0,-.108,.009),(0,.108,.009)],.004,'Repair_BookCloth')
export('OpenDiary')

clear()
# Thrown ceramic body, genuinely open inside, with a fine chipped, branching crack.
vertices,faces=[],[];n=96
profile=[(.0,.002),(.032,.002),(.038,.007),(.044,.025),(.046,.073),(.045,.093),(.041,.094),(.040,.082),(.039,.023),(.029,.012),(0,.012)]
for j,(r,z) in enumerate(profile):
    for i in range(n):
        a=.65+i/n*math.tau
        chip=max(0,1-abs(i-n*.015)/2)*(.0025 if j in (5,6) else 0)
        vertices.append((r*math.cos(a),r*math.sin(a),z-chip))
for j in range(len(profile)-1):
    for i in range(n):
        nxt=(i+1)%n;a=j*n+i;b=j*n+nxt;c=(j+1)*n+nxt;d=(j+1)*n+i
        faces.append((a,b,c,d))
mesh('Fine cracked porcelain',vertices,faces,'Repair_CupCeramic')
tube('Ceramic loop handle',[(.042+.029*math.cos(a),0,.050+.032*math.sin(a)) for a in [-math.pi/2+i/40*math.pi for i in range(41)]],.0055,'Repair_CupCeramic')
def hairline_point(t, angle_offset=0):
    z=.089-.034*t
    radius=(.045+(.093-z)*.05 if z>.073 else .044+.002*(z-.025)/.048)+.00014
    angle=.65-.014*t+.012*math.sin(t*math.pi*3)+.004*math.sin(t*math.pi*13)+angle_offset
    return (radius*math.cos(angle),radius*math.sin(angle),z)
main_crack=[hairline_point(i/48) for i in range(49)]
tube('Irregular glazed crack',main_crack,.00034,'Repair_CupCrack')
# Two short, tapering branches and a tiny glaze chip keep the fracture from
# reading as a machine-cut seam. The branch joins remain hairline thin.
for start,direction in ((.36,-1),(.36,1)):
    branch=[hairline_point(start+i/18*.12,-direction*i/18*.14) for i in range(19)]
    tube('Fine crack branch '+str(start)+' '+str(direction),branch,.00022,'Repair_CupCrack')
export('CrackedMilkCup')

clear()
# One continuous hollow enamel tub; rounded rectangular rim and sloping inner bowl.
vertices,faces=[],[];n=96
rings=[(.62,.22,.13),(.72,.27,.20),(.77,.31,.43),(.80,.34,.55),(.84,.38,.56),(.87,.40,.54),(.85,.39,.47),(.76,.32,.12),(.65,.25,.08)]
for rx,ry,z in rings:
    for i in range(n):
        a=i/n*math.tau
        x=rx*math.copysign(abs(math.cos(a))**.55,math.cos(a));y=ry*math.copysign(abs(math.sin(a))**.55,math.sin(a))
        vertices.append((x,y,z))
for j in range(len(rings)-1):
    for i in range(n):
        a=j*n+i;b=j*n+(i+1)%n;faces.append((a,b,b+n,a+n))
faces.append(tuple(reversed(range(n))));faces.append(tuple((len(rings)-1)*n+i for i in range(n)))
mesh('Hollow enamel bath with rolled rim',vertices,[tuple(reversed(f)) for f in faces],'Repair_Ceramic')
for x in (-.60,.60):
    for y in (-.25,.25):rounded('Cast iron foot',(x,y,.06),(.11,.11,.12),'Repair_Iron',.025)
tube('Gooseneck brass tap',[(-.68,-.3,.54),(-.68,-.3,.72),(-.58,-.3,.76),(-.49,-.3,.72),(-.49,-.3,.66)],.012,'Repair_Brass')
export('BloodBathTub')

clear()
# Openable wall grille, slats are bevelled sheet metal; no X motifs.
for x in (-.43,.43):rounded('Grille frame',(x,0,0),(.035,.035,.67),'Repair_Iron',.004)
for z in (-.32,.32):rounded('Grille frame',(0,0,z),(.86,.035,.035),'Repair_Iron',.004)
for z in [-.26+i*.065 for i in range(9)]:
    obj=rounded('Sloping ventilation louvre',(0,0,z),(.82,.075,.014),'Repair_Iron',.004);obj.rotation_euler.x=math.radians(25)
export('VentGrille')

clear()
# Wooden shaft ladder with a clear 30 cm rung spacing.
for x in (-.31,.31):rounded('Ladder upright',(x,0,1.72),(.065,.085,3.44),'Repair_BedWood',.008)
for z in [.22+i*.30 for i in range(11)]:rounded('Ladder rung',(0,-.007,z),(.63,.075,.055),'Repair_BedWood',.006)
export('CellarLadder')

clear()
rounded('Painted flush door',(0,0,1.05),(.90,.045,2.10),'Repair_DoorPaint',.005)
for z in (.49,1.52):
    rounded('Recessed painted panel',(0,-.025,z),(.69,.008,.73),'Repair_DoorInset',.012)
    rounded('Recessed painted panel',(0,.025,z),(.69,.008,.73),'Repair_DoorInset',.012)
for side in (-1,1):
    rounded('Lever escutcheon',(.33,side*.034,1.02),(.038,.012,.18),'Repair_Brass',.009)
    tube('Brushed metal lever',[(.33,side*.039,1.06),(.33,side*.075,1.06),(.23,side*.075,1.06)],.009,'Repair_Brass')
export('KosPanelDoor')

clear()
# Two foot-shaped rubber soles and raised flexible V straps, in metres.
# Keep the original generic pair elsewhere in the project unchanged.
outline=[(0,.143),(.039,.133),(.054,.088),(.048,.015),(.037,-.085),
         (.023,-.122),(-.016,-.124),(-.034,-.093),(-.033,-.020),
         (-.047,.062),(-.039,.119)]
for foot in (-1,1):
    def place(point):
        x,y,z=point;x*=foot
        angle=foot*-.10
        return (foot*.065+x*math.cos(angle)-y*math.sin(angle),
                y*math.cos(angle)+x*math.sin(angle),z)
    edge=[]
    for i in range(len(outline)):
        p0=Vector(outline[(i-1)%len(outline)]);p1=Vector(outline[i])
        p2=Vector(outline[(i+1)%len(outline)]);p3=Vector(outline[(i+2)%len(outline)])
        for j in range(6):
            t=j/6
            edge.append(.5*((2*p1)+(-p0+p2)*t+(2*p0-5*p1+4*p2-p3)*t*t+(-p0+3*p1-3*p2+p3)*t*t*t))
    edge.reverse()
    count=len(edge);vs=[place((p.x,p.y,z)) for z in (.003,.018) for p in edge]
    vs.extend([place((0,0,.003)),place((0,0,.017))]);fs=[]
    for i in range(count):
        j=(i+1)%count
        fs.extend([(i,j,j+count,i+count),(count*2,j,i),(count*2+1,i+count,j+count)])
    mesh('Shaped rubber sole',vs,[tuple(reversed(f)) for f in fs] if foot<0 else fs,'Repair_SandalRubber')
    tube('Bound sole edge',[place((p.x,p.y,.015)) for p in edge],.0015,'Repair_SandalStrap',True)
    tube('Soft toe post',[place((-.004,.072,.018)),place((-.004,.072,.041))],.0026,'Repair_SandalStrap')
    for side in (-1,1):
        vs=[];fs=[]
        for j in range(25):
            t=j/24
            x=-.004+(side*.044+.004)*t;y=.072-.100*t
            z=.041+.012*math.sin(t*math.pi)-.017*t
            dx=side*.044+.004;dy=-.100;norm=math.hypot(dx,dy)
            for k in (-1,1):vs.append(place((x+k*(-dy/norm)*.006,y+k*(dx/norm)*.006,z)))
        for j in range(24):a=j*2;fs.append((a,a+2,a+3,a+1))
        ribbon=mesh('Curved V strap',vs,[tuple(reversed(f)) for f in fs] if foot<0 else fs,'Repair_SandalStrap')
        mod=ribbon.modifiers.new('Flexible rubber thickness','SOLIDIFY');mod.thickness=.003
export('IndonesianFlipFlops')

report=ROOT/'Audit/prolog47-original-props.json'
if os.environ.get('PROLOG_PROP_FILTER') and report.exists():
    prior={m['name']:m for m in json.loads(report.read_text())['models']}
    prior.update({m['name']:m for m in RECORDS});RECORDS=list(prior.values())
report.write_text(json.dumps(dict(author='Layar Terakhir project — authored for this repair',license='Project original',tool=bpy.app.version_string,models=RECORDS),indent=2)+'\n')
print('PROLOG47_MODELS',len(RECORDS))
