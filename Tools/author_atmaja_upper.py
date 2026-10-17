"""Adegan 4-7 upper floor + basement original props. Blender 4, run headless:
blender -b --python Tools/author_atmaja_upper.py
No staging or Golden assets are edited.
"""
from pathlib import Path
import bpy, json, math
import random
from mathutils import Vector
random.seed(7)
ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/Art/Original/Models'
OUT.mkdir(parents=True, exist_ok=True)
manifest = json.loads((ROOT / 'Audit/atmaja-art-manifest.json').read_text())

def clear():
    bpy.ops.wm.read_factory_settings(use_empty=True)

def mat(name):
    return bpy.data.materials.get(name) or bpy.data.materials.new(name)

def box(name, p, s, m, bevel=.01):
    bpy.ops.mesh.primitive_cube_add(size=1, location=p)
    o = bpy.context.object
    o.name = name
    o.scale = s
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    o.data.materials.append(mat(m))
    if bevel:
        mod = o.modifiers.new('Worn edge bevel', 'BEVEL')
        mod.width = bevel
        mod.segments = 2
        bpy.ops.object.modifier_apply(modifier=mod.name)
    return o

def ball(name, p, s, m, rotation=(0, 0, 0)):
    bpy.ops.mesh.primitive_uv_sphere_add(segments=16, ring_count=10, location=p)
    o = bpy.context.object
    o.name = name
    o.scale = s
    o.rotation_euler = rotation
    o.data.materials.append(mat(m))
    return o

def cylinder(name, p, r, d, m, rotation=(0, 0, 0)):
    bpy.ops.mesh.primitive_cylinder_add(vertices=32, radius=r, depth=d, location=p, rotation=rotation)
    o = bpy.context.object
    o.name = name
    o.data.materials.append(mat(m))
    return o

def tube(name, points, r, m):
    data = bpy.data.curves.new(name, 'CURVE')
    data.dimensions = '3D'
    data.bevel_depth = r
    data.bevel_resolution = 1
    spline = data.splines.new('POLY')
    spline.points.add(len(points) - 1)
    for v, p in zip(spline.points, points):
        v.co = (*p, 1)
    o = bpy.data.objects.new(name, data)
    bpy.context.collection.objects.link(o)
    o.data.materials.append(mat(m))
    return o

def export(name):
    for o in list(bpy.context.scene.objects):
        if o.type == 'CURVE':
            bpy.ops.object.select_all(action='DESELECT')
            o.select_set(True)
            bpy.context.view_layer.objects.active = o
            bpy.ops.object.convert(target='MESH')
    for o in list(bpy.context.scene.objects):
        if o.type != 'MESH':
            continue
        pts = [o.matrix_world @ Vector(v.co) for v in o.data.vertices]
        if len(pts) == 8 and all((tuple((abs(a) - 1) < 1e-5 for a in point)) for point in [p.xyz for p in pts]):
            bpy.data.objects.remove(o, do_unlink=True)
    groups = {}
    for o in list(bpy.context.scene.objects):
        if o.type != 'MESH':
            continue
        groups.setdefault((o.name, o.data.materials[0].name), []).append(o)
    for (role, m), objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for o in objects:
            o.select_set(True)
        bpy.context.view_layer.objects.active = objects[0]
        if len(objects) > 1:
            bpy.ops.object.join()
        o = bpy.context.object
        o.name = 'Static__' + m
        if not o.data.uv_layers:
            o.data.uv_layers.new()
        uv = o.data.uv_layers.active.data
        for poly in o.data.polygons:
            normal = o.matrix_world.to_3x3() @ poly.normal
            axis = max(range(3), key=lambda i: abs(normal[i]))
            others = [i for i in range(3) if i != axis]
            for index in poly.loop_indices:
                p = o.matrix_world @ o.data.vertices[o.data.loops[index].vertex_index].co
                uv[index].uv = (p[others[0]], p[others[1]])
    bpy.ops.object.select_all(action='SELECT')
    print('DEBUG_SELECTED',[o.name for o in bpy.context.scene.objects if o.select_get()])
    bpy.ops.export_scene.fbx(filepath=str(OUT / (name + '.fbx')), object_types={'MESH'}, bake_anim=False,
                             axis_forward='-Z', axis_up='Y', path_mode='STRIP')

wood = 'House_MAT_WoodDoor'; brass = 'Detail_AgedBrass'; enamel = 'Upper_EnamelWhite'; glass = 'Detail_MirrorGlass'
rope = 'Upper_OldRope'; iron = 'Upper_IronBlack'; rust = 'Upper_RustShears'; kebaya = 'Upper_KebayaDirty'
cloth = 'Detail_ARCloth'; hair = 'Detail_ARHair'; skin = 'Detail_ARSkin'; stone = 'House_MAT_Concrete'

# --- Bathtub: freestanding enamel tub with chipped rim, tide marks, twin brass rails.
clear()
box('Tub outer shell', (0, 0, .30), (1.52, .72, .52), enamel, .05)
box('Tub inner cavity', (0, .015, .335), (1.40, .60, .44), enamel, .03)
cylinder('Tub drain plug', (0, -.315, .10), .05, .02, brass, (math.pi / 2, 0, 0))
for sx in [-1, 1]:
    box('Tub feet', (sx * .62, 0, .045), (.10, .62, .09), brass, .012)
    tube('Tub hand rail', [(sx * .70, 0, .20), (sx * .78, -.16, .16)], .017, brass)
for sy in [-1, 1]:
    cylinder('Tub overflow lip', (0, sy * .365, .50), .05, .05, enamel)
    cylinder('Tub rim edge', (0, sy * .315, .05), .05, .05, enamel)
for i in range(6):
    box('Tub chipped enamel patch', (-.10 + i * .10, .02 * random.uniform(-1, 1), -.155), (.05, .03, .02), stone)
    box('Tub tide grime line', (0, .01 * random.uniform(-1, 1), -.155 + i * .06), (1.5, .5, .01), stone)
export('UpperBathtub')

# --- Old pedestal sink with twin brass taps and chipped porcelain.
clear()
cylinder('Sink basin', (0, 0, .78), .23, .16, enamel)
cylinder('Sink bowl interior', (0, 0, .85), .20, .13, enamel)
cylinder('Sink pedestal column', (0, 0, .28), .06, .62, stone)
cylinder('Sink pedestal foot', (0, 0, .02), .13, .04, stone)
for sx in [-1, 1]:
    tube('Sink tap column', [(sx * .16, 0, .86), (sx * .16, 0, .98), (sx * .10, 0, 1.0)], .019, brass)
    ball('Sink tap cross handle', (sx * .10, 0, 1.03), (.035, .035, .035), brass)
    cylinder('Sink tap spout head', (0, .14, .96), .014, .10, brass, (0, 0, 0))
box('Sink back splash', (0, -.20, .74), (.48, .20, .14), enamel, .01)
for sx in [-1, 1]:
    ball('Sink chipped rim', (sx * .20, 0, .79), (.035, .035, .02), glass)
export('UpperSinkPedestal')

# --- Tall standing dressing mirror (Adegan 5-6 hero prop).
clear()
box('Mirror frame body', (0, 0, 1.02), (.90, .13, 1.86), wood, .02)
box('Mirror back board', (0, .055, 1.02), (.84, .03, 1.80), wood, .008)
cylinder('Mirror glass disc', (-.04, 0, 1.02), .40, .17, glass, (0, math.pi / 2, 0))
for sy in [-1, 1]:
    box('Mirror foot blocks', (0, sy * .34, .03), (.26, .20, .06), wood, .008)
    box('Mirror stile leg', (0, sy * .30, .70), (.12, .09, 1.30), wood, .012)
for sy in [-1, 1]:
    ball('Mirror frame ornament', (0, sy * .28, 1.90), (.05, .05, .03), brass)
export('UpperStandingMirror')

# --- Standing Sri Atmaja figure: torn kebaya, broken neck conviction.
clear()
rings = [
    (.10, .17, .18, -.02), (.20, .15, .17, 0), (.33, .13, .16, 0), (.49, .11, .15, 0),
    (.68, .12, .14, .05), (.90, .13, .17, .05), (.99, .15, .20, .10)
]
for cz, rx, ry, fold in rings:
    pts = []
    for i in range(24):
        a = i * math.pi * 2 / 24
        wob = 1 + .04 * math.sin(a * 6 + cz * 3)
        pts.append((rx * math.cos(a) * wob, ry * math.sin(a) * wob, cz + fold))
    tube('Kebaya drapery ring', pts, .012, kebaya)
cylinder('Kebaya bodice', (0, 0, .52), .19, .74, kebaya)
for sy in [-1, 1]:
    tube('Kebaya sleeve arm', [(sy * .16, 0, .96), (sy * .22, -.08, .76), (sy * .05, -.16, .55)], .055, kebaya)
    ball('Hollow hand', (sy * .02, -.19, .54), (.05, .075, .03), skin)
    box('Bare foot', (sy * .08, -.02, .025), (.09, .20, .05), skin)
tube('Broken neck', [(0, 0, 1.02), (-.14, -.02, 1.06)], .04, skin)
ball('Tilted head', (-.23, -.02, 1.08), (.11, .09, .15), skin, (0, math.pi / 2, 0))
ball('Long dark hair cap', (-.235, .004, 1.11), (.17, .095, .10), hair)
for i in range(15):
    x = -.35 + i * .022
    y = .025 + .02 * math.sin(i)
    tube('Fallen hair strand', [(x, y, 1.10), (x - .02, y - .01, .92), (x + .03, y - .02, .68 + (i % 3) * .02)], .012, hair)
for x in [-.25, -.16]:
    ball('Hollow eye', (x, -.10, 1.08), (.02, .008, .012), hair)
tube('Neck ligature rope', [(0, 0, 1.00), (.02, .02, 1.02)], .02, rope)
export('UpperStandingNyonya')

# --- Adegan 4 AR hanged figure: hangs from rope pivot, cloth drape, collapsed head.
clear()
rings = [
    (.94, .15), (.88, .17), (.79, .19), (.68, .23), (.56, .23), (.46, .21), (.36, .18), (.25, .15), (.15, .13)
]
for cz, ry in rings:
    tube('Hanged cloth ring', [((ry * math.cos(i * math.pi * 2 / 24)), ry * math.sin(i * math.pi * 2 / 24), cz) for i in range(24)], .014, kebaya)
cylinder('Hanged bodice tube', (0, 0, .58), .19, .72, kebaya)
box('Noose cord', (0, 0, .15), (.04, .30, .05), rope, .004)
for sy in [-1, 1]:
    ball('Collapsed bare foot tip', (sy * .08, .16, .02), (.055, .10, .04), skin)
tube('Drooping neck', [(0, 0, .98), (-.10, -.03, 1.01)], .035, skin)
ball('Hanging head base', (-.16, -.03, 1.03), (.11, .08, .15), skin, (0, math.pi / 2, 0))
ball('Hanging hair cap', (-.17, .005, 1.05), (.16, .09, .10), hair)
for i in range(9):
    x = -.30 + i * .03
    tube('Hanging hair strand', [(x, .02, 1.06), (x, .01, .92), (x + .02, .005, .74)], .013, hair)
tube('Neck ligature knot', [(0, 0, 1.0), (.01, .02, 1.03)], .018, rope)
export('UpperHangedNyonya')

# --- Old garden shears, rusted with stretched blade (dragged weapon).
clear()
for sy in [-1, 1]:
    box('Shears blade ' + ('L' if sy > 0 else 'R'), (.22, sy * .012, 0), (.44, .018, .030), rust, .004)
    tube('Shears handle', [(-.02, sy * .02, 0), (-.18, sy * .05, -.20)], .016, iron)
    tube('Shears grip', [(-.18, sy * .05, -.20), (-.13, sy * .05, -.34)], .026, iron)
cylinder('Shears pivot', (0, 0, 0), .022, .05, brass, (0, math.pi / 2, 0))
export('UpperShearsRust')

# --- Manual sewing machine silhouette (no logos): body, wheel, needle, treadle bars.
clear()
box('Sewing machine base plate', (0, 0, .04), (.34, .16, .08), iron, .008)
for sx in [-1, 1]:
    box('Machine pillar', (sx * .12, 0, .24), (.05, .12, .34), iron, .006)
tube('Machine arm bridge', [(-.12, 0, .40), (0, 0, .44), (.12, 0, .40)], .02, iron)
cylinder('Machine hand wheel', (0, .08, .30), .085, .02, brass, (0, math.pi / 2, 0))
cylinder('Machine hand wheel hub', (0, .08, .30), .025, .05, iron, (0, math.pi / 2, 0))
box('Machine needle bar', (0, -.07, .33), (.015, .02, .10), iron, .002)
cylinder('Machine needle', (0, -.07, .26), .004, .12, brass)
box('Machine top rail', (0, 0, .41), (.30, .14, .05), iron, .006)
for i in range(3):
    tube('Machine stitch arm detail', [(.09 - i * .09, 0, .40), (.09 - i * .09, .02, .35)], .006, iron)
export('UpperSewingMachine')

# --- Long sewing workbench with slatted top; fits the kolong-sneak layout.
clear()
box('Bench top', (0, 0, .845), (2.2, .72, .055), wood, .02)
for sy in [-1, 1]:
    box('Bench cross rail', (0, sy * .34, .78), (2.10, .06, .10), wood, .01)
for sx in [-1, 1]:
    for sy in [-1, 1]:
        box('Bench leg', (sx * 1.00, sy * .30, .42), (.09, .09, .85), wood, .01)
        box('Bench shoe', (sx * 1.00, sy * .30, .05), (.13, .13, .06), wood, .008)
for sx in [-1, 1]:
    box('Bench side stretcher', (sx * .90, 0, .22), (2.0, .06, .08), wood, .01)
for i in range(3):
    cylinder('Bench dowel anchor', (-1.02 + i * 1.02, 0, .62), .03, .06, iron, (math.pi / 2, 0, 0))
export('UpperSewingBench')

# --- Hanging rag sheet at basement stair exit.
clear()
box('Rag fabric top roll', (0, 0, 2.05), (.86, .06, .10), cloth, .02)
for i in range(9):
    x = -.38 + i * .095
    hang = 1.34 + .18 * ((i * 5) % 7) / 6
    tube('Rag drop cloth', [(x, 0, 2.02), (x + .01, 0, hang)], .028, kebaya)
    box('Rag heavy edge', (x, 0, hang), (.085, .02, .02), kebaya)
    box('Rag sway fold', (x * 1.06, 0.015, hang + .30), (.02, .055, .16), kebaya)
export('UpperHangingRag')

# --- Basement trapdoor slab with plank lines and brass hinge bolts.
clear()
box('Trapdoor plank', (0, 0, .055), (.90, .90, .09), wood, .015)
for i in range(3):
    cylinder('Trapdoor plank ridge', (-.28 + i * .28, 0, .056), .02, .86, wood, (0, math.pi / 2, 0))
for sx in [-1, 1]:
    for sy in [-1, 1]:
        box('Trapdoor hinge bolt', (sx * .38, sy * .38, .03), (.03, .03, .02), brass)
cylinder('Trapdoor ring pull', (.15, 0, .10), .03, .02, brass, (math.pi / 2, 0, 0))
export('UpperTrapdoorSlab')

for name, color, rough, metal in [
    (enamel, (.55, .56, .53, 1), .30, .10),
    (glass, (.30, .36, .42, 1), .04, 1),
    (rope, (.36, .28, .17, 1), 1, 0),
    (iron, (.05, .05, .06, 1), .45, .70),
    (rust, (.28, .10, .04, 1), .85, .35),
    (kebaya, (.55, .52, .44, 1), .95, 0),
]:
    if not any(m['name'] == name for m in manifest['materials']):
        manifest['materials'].append(dict(name=name, color=list(color), roughness=rough,
                                          metallic=metal, alphaClip=False, albedo='', normal='', mask=''))
(ROOT / 'Audit/atmaja-art-manifest.json').write_text(json.dumps(manifest, indent=2) + '\n')
print('ATMAJA_UPPER_ORIGINALS_EXPORTED')
