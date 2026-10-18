"""Original static low-poly props with modeled edges, seams and fabric folds.
Run: blender -b --python Tools/author_kos_v2.py. Exports only the new V2 assets.
"""
import bpy, math, sys
from pathlib import Path
from mathutils import Vector

OUT = Path(__file__).resolve().parents[1] / 'Assets/_LayarTerakhir/assets/Art/Original/Models'

def clear():
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.object.delete(use_global=False)

def finish(o, name, slot):
    o.name = name
    material = bpy.data.materials.get(slot) or bpy.data.materials.new(slot)
    o.data.materials.append(material)
    return o

def box(name, loc, size, slot, bevel=.01):
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc)
    o = bpy.context.object
    o.scale = size
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    b = o.modifiers.new('Rounded edges', 'BEVEL'); b.width = bevel; b.segments = 3
    bpy.ops.object.modifier_apply(modifier=b.name)
    return finish(o, name, slot)

def tube(name, points, width, slot):
    curve = bpy.data.curves.new(name, 'CURVE'); curve.dimensions = '3D'
    curve.bevel_depth = width; curve.bevel_resolution = 1
    spline = curve.splines.new('POLY'); spline.points.add(len(points)-1)
    for p, v in zip(spline.points, points): p.co = (*v, 1)
    o = bpy.data.objects.new(name, curve); bpy.context.collection.objects.link(o)
    return finish(o, name, slot)

def cylinder(name, loc, radius, depth, slot, rotation=(0, 0, 0), vertices=24):
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth, location=loc, rotation=rotation)
    return finish(bpy.context.object, name, slot)

def export(name):
    if '--curtains-only' in sys.argv and name != 'KosCurtains': return
    for o in list(bpy.context.scene.objects):
        bpy.context.view_layer.objects.active = o
        if o.type == 'CURVE':
            bpy.ops.object.select_all(action='DESELECT'); o.select_set(True)
            bpy.ops.object.convert(target='MESH')
    # Share a renderer per material instead of one for every wire/spoke.
    groups = {}
    for o in list(bpy.context.scene.objects):
        if o.type == 'MESH': groups.setdefault(o.data.materials[0].name, []).append(o)
    for slot, objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for o in objects: o.select_set(True)
        bpy.context.view_layer.objects.active = objects[0]
        if len(objects) > 1: bpy.ops.object.join()
        bpy.context.object.name = slot
    for o in list(bpy.context.scene.objects):
        if o.type != 'MESH': continue
        bpy.ops.object.select_all(action='DESELECT'); o.select_set(True)
        bpy.context.view_layer.objects.active = o
        bpy.ops.object.mode_set(mode='EDIT'); bpy.ops.mesh.select_all(action='SELECT')
        bpy.ops.uv.smart_project(island_margin=.02); bpy.ops.object.mode_set(mode='OBJECT')
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.fbx(filepath=str(OUT / (name + '.fbx')), object_types={'MESH'}, bake_anim=False,
                             axis_forward='-Z', axis_up='Y', add_leaf_bones=False)

clear()
box('Weighted oval fan base', (0, 0, .02), (.23, .17, .04), 'FanEnamel', .035)
cylinder('Fan upright', (0, .025, .12), .018, .18, 'FanMetal')
cylinder('Fan motor', (0, .028, .29), .052, .105, 'FanEnamel', (math.pi/2, 0, 0))
for side in [-1, 1]:
    for radius in [.065, .095, .126]:
        tube('Concentric wire guard', [(radius*math.cos(a), side*.038, .29+radius*math.sin(a))
             for a in [i*math.tau/48 for i in range(49)]], .0018, 'FanMetal')
    for i in range(12):
        a=i*math.tau/12
        tube('Guard spoke', [(.02*math.cos(a), side*.045, .29+.02*math.sin(a)),
             (.126*math.cos(a), side*.038, .29+.126*math.sin(a))], .0014, 'FanMetal')
for i in range(3):
    angle=i*math.tau/3
    blade=box('Fan blade', (.062*math.cos(angle), 0, .29+.062*math.sin(angle)), (.11,.008,.04), 'FanBlade', .018)
    blade.rotation_euler[1]=-angle
cylinder('Fan hub', (0, -.043, .29), .028, .012, 'FanEnamel', (math.pi/2,0,0))
for x in [-.055,-.02,.015]: box('Speed button',(x,-.055,.045),(.02,.018,.01),'FanMetal',.004)
export('KosDeskFan')

clear()
for side in [-1,1]:
    x=side*.072
    sole=box('Worn rubber sole',(x,0,.013),(.108,.265,.026),'SandalRubber',.045)
    sole.rotation_euler[2]=side*.08
    tube('Flip flop strap',[(x-.047,.003,.026),(x,.05,.059),(x+.047,.003,.026)],.009,'SandalStrap')
    tube('Toe post',[(x,.05,.059),(x,.055,.023)],.004,'SandalStrap')
export('KosSandals')

clear()
box('Soft canvas bag',(0,0,.21),(.29,.14,.39),'BagCanvas',.065)
box('Front zip pocket',(0,-.083,.145),(.235,.05,.18),'BagCanvas',.035)
tube('Pocket zipper',[(-.10,-.112,.225),(0,-.117,.235),(.10,-.112,.225)],.002,'BagMetal')
tube('Main zipper',[(-.135,0,.32),(-.10,-.02,.395),(0,-.03,.41),(.10,-.02,.395),(.135,0,.32)],.002,'BagMetal')
for side in [-1,1]:
    tube('Padded shoulder strap',[(side*.09,.06,.35),(side*.13,.14,.29),(side*.12,.16,.12),(side*.09,.06,.045)],.013,'BagStrap')
tube('Carry loop',[(-.035,0,.40),(-.03,0,.44),(.03,0,.44),(.035,0,.40)],.006,'BagStrap')
box('Unbranded patch',(0,-.114,.16),(.045,.003,.025),'BagStrap',.003)
export('KosBackpack')

clear()
# Two cloth panels with irregular hems and sewn folds, local X width / Z vertical.
for side in [-1,1]:
    verts=[]; faces=[]; nx=24; nz=24
    for j in range(nz+1):
        v=j/nz
        for i in range(nx+1):
            u=i/nx
            x=side*(.37+u*.28)
            y=.025*math.cos(u*math.pi*10)+.004*math.sin(v*9+u*8)
            z=.04+v*1.37+.009*math.sin(u*12)*(1-v)
            verts.append((x,y,z))
    for j in range(nz):
        for i in range(nx):
            a=j*(nx+1)+i
            # Mirrored X reverses winding; give both panels +Y normals so their fronts face the room after Unity import.
            faces.append((a,a+1,a+nx+2,a+nx+1) if side<0 else (a,a+nx+1,a+nx+2,a+1))
    mesh=bpy.data.meshes.new('Sewn curtain folds');mesh.from_pydata(verts,[],faces);mesh.update()
    o=bpy.data.objects.new('Curtain panel',mesh);bpy.context.collection.objects.link(o);finish(o,o.name,'CurtainFabric')
    for polygon in mesh.polygons: polygon.use_smooth=True
    for x in [side*(.37+i*.056) for i in range(6)]:
        tube('Curtain loop',[(x,.0,1.40),(x,.025,1.44),(x,-.025,1.44),(x,0,1.40)],.003,'CurtainFabric')
export('KosCurtains')

clear()
box('Vented router',(0,0,.018),(.17,.105,.036),'RouterPlastic',.008)
for x in [-.067,.067]:
    antenna=cylinder('WiFi antenna',(x,.037,.085),.004,.14,'RouterPlastic',vertices=12)
    antenna.rotation_euler[1]=x*1.6
for x in [-.05,-.03,-.01]:box('Router status lens',(x,-.054,.022),(.005,.002,.003),'RouterLED',.001)
for x in [-.055+i*.01 for i in range(12)]:box('Cooling slot',(x,0,.037),(.003,.055,.001),'RouterVent',.001)
export('KosRouter')
print('V2_PROPS_EXPORTED', OUT)
