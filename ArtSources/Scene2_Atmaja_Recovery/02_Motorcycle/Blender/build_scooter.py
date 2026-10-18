"""Build Damar's automatic scooter - generic Indonesian commuter matic
(Beat/Vario/Mio class, no brand marks) for LayarTerakhir Prolog Adegan 2.

Method: PROCEDURAL (authored in Blender; no downloaded mesh used)
  * Real-world proportions: length ~1.86 m, wheelbase ~1.25 m, seat ~0.75 m.
  * 14-inch wheels, front disc brake, CVT cover, side stand, mirrors.
  * Procedural PBR factors only - no textures, nothing missing on import.

Run headless:
    blender -b -P ArtSources/Scene2_Atmaja_Recovery/02_Motorcycle/Blender/build_scooter.py

Outputs:
    02_Motorcycle/Blender/DamarScooter.blend
    02_Motorcycle/Blender/scooter_build_stats.json
    02_Motorcycle/Export/DamarScooter.fbx / .glb
No Unity files are touched.
"""
from pathlib import Path
import bmesh
import json
import math

import bpy
from mathutils import Matrix, Vector

PKG = Path(__file__).resolve().parents[2]
MOTO = PKG / '02_Motorcycle'
BLEND = MOTO / 'Blender'
EXPORT = MOTO / 'Export'

COLLECTIONS = {}


def reset():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    for collection in list(bpy.data.collections):
        bpy.data.collections.remove(collection)


def new_collection(name):
    collection = bpy.data.collections.new(name)
    bpy.context.scene.collection.children.link(collection)
    COLLECTIONS[name] = collection
    return collection


def make_material(name, color, roughness=0.5, metallic=0.0):
    material = bpy.data.materials.new(name)
    material.use_nodes = True
    bsdf = material.node_tree.nodes['Principled BSDF']
    bsdf.inputs['Base Color'].default_value = (*color, 1.0)
    bsdf.inputs['Roughness'].default_value = roughness
    bsdf.inputs['Metallic'].default_value = metallic
    return material


def build_materials():
    return {
        'body': make_material('MOTO_BodyPaint', (0.055, 0.06, 0.075), 0.28, 0.35),
        'body_dark': make_material('MOTO_DarkPlastic', (0.02, 0.02, 0.023), 0.55, 0.0),
        'seat': make_material('MOTO_SeatVinyl', (0.018, 0.018, 0.02), 0.48, 0.0),
        'tire': make_material('MOTO_TireRubber', (0.012, 0.012, 0.012), 0.88, 0.0),
        'rim': make_material('MOTO_Rim', (0.42, 0.43, 0.45), 0.32, 0.85),
        'engine': make_material('MOTO_Engine', (0.30, 0.30, 0.31), 0.45, 0.75),
        'chrome': make_material('MOTO_Chrome', (0.75, 0.76, 0.78), 0.14, 1.0),
        'lens': make_material('MOTO_HeadlightLens', (0.75, 0.77, 0.80), 0.12, 0.0),
        'tail': make_material('MOTO_TailLight', (0.42, 0.02, 0.02), 0.22, 0.0),
        'amber': make_material('MOTO_Amber', (0.55, 0.22, 0.02), 0.25, 0.0),
    }


# ----------------------------------------------------------------- shapes
def object_from_bmesh(name, bm, material, collection, smooth=False):
    mesh = bpy.data.meshes.new(name)
    bm.to_mesh(mesh)
    bm.free()
    obj = bpy.data.objects.new(name, mesh)
    collection.objects.link(obj)
    obj.data.materials.append(material)
    if smooth:
        for polygon in obj.data.polygons:
            polygon.use_smooth = True
        obj.data.use_auto_smooth = True
        obj.data.auto_smooth_angle = math.radians(46)
    return obj


def base_box(name, size, location, material, collection, rotation=(0, 0, 0)):
    bm = bmesh.new()
    bmesh.ops.create_cube(bm, size=1.0)
    bmesh.ops.scale(bm, vec=Vector(size), verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[2], 3, 'Z'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[0], 3, 'X'),
                     verts=bm.verts)
    bmesh.ops.translate(bm, vec=Vector(location), verts=bm.verts)
    return object_from_bmesh(name, bm, material, collection)


def soft_box(name, size, location, material, collection, bevel=0.035, segments=2,
             subdiv=1, rotation=(0, 0, 0), smooth=True):
    obj = base_box(name, size, location, material, collection, rotation=rotation)
    if bevel:
        modifier = obj.modifiers.new('Bevel', 'BEVEL')
        modifier.width = bevel
        modifier.segments = segments
        modifier.limit_method = 'ANGLE'
    if subdiv:
        modifier = obj.modifiers.new('Subsurf', 'SUBSURF')
        modifier.levels = subdiv
        modifier.render_levels = subdiv
    bpy.context.view_layer.objects.active = obj
    for modifier in list(obj.modifiers):
        bpy.ops.object.modifier_apply(modifier=modifier.name)
    if smooth:
        for polygon in obj.data.polygons:
            polygon.use_smooth = True
        obj.data.use_auto_smooth = True
        obj.data.auto_smooth_angle = math.radians(46)
    return obj


def cylinder(name, radius, depth, location, material, collection, segments=24,
             rotation=(0, 0, 0), scale=(1, 1, 1), smooth=True):
    bm = bmesh.new()
    bmesh.ops.create_cone(bm, cap_ends=True, cap_tris=False, segments=segments,
                          radius1=radius, radius2=radius, depth=depth)
    bmesh.ops.scale(bm, vec=Vector(scale), verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[0], 3, 'X'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[1], 3, 'Y'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[2], 3, 'Z'),
                     verts=bm.verts)
    bmesh.ops.translate(bm, vec=Vector(location), verts=bm.verts)
    return object_from_bmesh(name, bm, material, collection, smooth=smooth)


def cone(name, radius1, radius2, depth, location, material, collection, segments=24,
         rotation=(0, 0, 0), scale=(1, 1, 1), smooth=True):
    bm = bmesh.new()
    bmesh.ops.create_cone(bm, cap_ends=True, cap_tris=False, segments=segments,
                          radius1=radius1, radius2=radius2, depth=depth)
    bmesh.ops.scale(bm, vec=Vector(scale), verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[0], 3, 'X'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[1], 3, 'Y'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[2], 3, 'Z'),
                     verts=bm.verts)
    bmesh.ops.translate(bm, vec=Vector(location), verts=bm.verts)
    return object_from_bmesh(name, bm, material, collection, smooth=smooth)


def torus(name, major, minor, location, material, collection, major_segments=32,
          minor_segments=12, rotation=(0, 0, 0), scale=(1, 1, 1)):
    bm = bmesh.new()
    first_ring = None
    previous_ring = None
    for i in range(major_segments):
        angle = 2 * math.pi * i / major_segments
        centre = Vector((math.cos(angle) * major, math.sin(angle) * major, 0))
        ring = []
        for j in range(minor_segments):
            theta = 2 * math.pi * j / minor_segments
            normal = Vector((math.cos(angle), math.sin(angle), 0))
            point = centre + normal * math.cos(theta) * minor + \
                Vector((0, 0, math.sin(theta) * minor))
            ring.append(bm.verts.new(point))
        if i == 0:
            first_ring = ring
        if i > 0:
            for j in range(minor_segments):
                v1 = previous_ring[j]
                v2 = previous_ring[(j + 1) % minor_segments]
                v3 = ring[(j + 1) % minor_segments]
                v4 = ring[j]
                bm.faces.new((v1, v2, v3, v4))
        previous_ring = ring
    for j in range(minor_segments):
        bm.faces.new((previous_ring[j], previous_ring[(j + 1) % minor_segments],
                      first_ring[(j + 1) % minor_segments], first_ring[j]))
    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    bmesh.ops.scale(bm, vec=Vector(scale), verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[0], 3, 'X'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[1], 3, 'Y'),
                     verts=bm.verts)
    bmesh.ops.rotate(bm, cent=(0, 0, 0), matrix=Matrix.Rotation(rotation[2], 3, 'Z'),
                     verts=bm.verts)
    bmesh.ops.translate(bm, vec=Vector(location), verts=bm.verts)
    return object_from_bmesh(name, bm, material, collection, smooth=True)


def arc_strip(name, center_y, radius, width, thickness, angle_start, angle_end,
              material, collection, segments=14):
    """Curved fender strip around the X axis (wheel plane is Y-Z)."""
    bm = bmesh.new()
    inner = radius - thickness
    rings = {}
    for i in range(segments + 1):
        angle = math.radians(angle_start + (angle_end - angle_start) * i / segments)
        cy = center_y + math.cos(angle) * radius
        cz = 0.25 + math.sin(angle) * radius
        iy = center_y + math.cos(angle) * inner
        iz = 0.25 + math.sin(angle) * inner
        column = []
        for x, y, z in [(-width / 2, iy, iz), (-width / 2, cy, cz),
                        (width / 2, cy, cz), (width / 2, iy, iz)]:
            column.append(bm.verts.new((x, y, z)))
        rings[i] = column
    for i in range(segments):
        for j in range(4):
            v1 = rings[i][j]
            v2 = rings[i][(j + 1) % 4]
            v3 = rings[i + 1][(j + 1) % 4]
            v4 = rings[i + 1][j]
            bm.faces.new((v1, v2, v3, v4))
    bm.faces.new(rings[0])
    bm.faces.new(list(reversed(rings[segments])))
    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    return object_from_bmesh(name, bm, material, collection, smooth=True)


def swept_bar(name, points, radius, material, collection, segments=12):
    """Round tube through a polyline (used for grab rails / fork legs)."""
    bm = bmesh.new()
    rings = []
    for index, point in enumerate(points):
        if index == 0:
            direction = (Vector(points[1]) - Vector(point)).normalized()
        elif index == len(points) - 1:
            direction = (Vector(point) - Vector(points[index - 1])).normalized()
        else:
            direction = (Vector(points[index + 1]) - Vector(points[index - 1])).normalized()
        normal = direction.cross(Vector((0, 0, 1)))
        if normal.length < 1e-5:
            normal = direction.cross(Vector((0, 1, 0)))
        normal.normalize()
        binormal = direction.cross(normal).normalized()
        ring = []
        for j in range(segments):
            theta = 2 * math.pi * j / segments
            offset = normal * math.cos(theta) * radius + binormal * math.sin(theta) * radius
            ring.append(bm.verts.new(Vector(point) + offset))
        rings.append(ring)
    for i in range(len(rings) - 1):
        for j in range(segments):
            bm.faces.new((rings[i][j], rings[i][(j + 1) % segments],
                          rings[i + 1][(j + 1) % segments], rings[i + 1][j]))
    bm.faces.new(list(reversed(rings[0])))
    bm.faces.new(rings[-1])
    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    return object_from_bmesh(name, bm, material, collection, smooth=True)


# ------------------------------------------------------------------ wheels
def wheel(name, centre, material_rim, material_tire, collection, front=False):
    y = centre[1]
    torus(name + '_tire', 0.192, 0.062, (0, y, 0.25), material_tire, collection,
          scale=(1.0, 1.0, 0.96))
    cylinder(name + '_rim', 0.155, 0.05, (0, y, 0.25), material_rim, collection,
             segments=28, rotation=(math.radians(90), 0, 0))
    cylinder(name + '_hub', 0.045, 0.09, (0, y, 0.25), material_rim, collection,
             segments=16, rotation=(math.radians(90), 0, 0))
    for index in range(5):
        angle = math.radians(index * 72 + 18)
        base_box(f'{name}_spoke_{index}', (0.026, 0.24, 0.022), (0, y, 0.25),
                 material_rim, collection, rotation=(angle, 0, 0))
    if front:
        cylinder(name + '_disc', 0.085, 0.006, (0.045, y, 0.25), material_rim,
                 collection, segments=24, rotation=(math.radians(90), 0, 0))
        soft_box(name + '_caliper', (0.035, 0.06, 0.10), (0.055, y + 0.085, 0.295),
                 material_rim, collection, bevel=0.008, subdiv=0)
        cone(name + '_axle', 0.017, 0.017, 0.17, (0, y, 0.25), material_rim, collection,
             segments=12, rotation=(math.radians(90), 0, 0))


def main_build():
    reset()
    materials = build_materials()
    frame = new_collection('COLL_Scooter')

    # wheels: front at -Y, rear at +Y
    wheel('WHEEL_FRONT', (0, -0.625, 0), materials['rim'], materials['tire'], frame, front=True)
    wheel('WHEEL_REAR', (0, 0.625, 0), materials['rim'], materials['tire'], frame)

    # front fork + fender + handlebar
    for side in (-1, 1):
        cylinder(f'FORK_{side}', 0.022, 0.42, (side * 0.075, -0.585, 0.47),
                 materials['chrome'], frame, segments=12,
                 rotation=(math.radians(14), 0, 0))
    arc_strip('FRONT_FENDER_ARC', -0.625, 0.272, 0.12, 0.03, 38, 162,
              materials['body'], frame, segments=16)
    cylinder('STEER_COLUMN', 0.03, 0.30, (0, -0.50, 0.72), materials['engine'], frame,
             segments=14, rotation=(math.radians(14), 0, 0))
    # handlebar: crossbar + grips + switch cubes
    cylinder('HANDLEBAR', 0.016, 0.62, (0, -0.455, 0.92), materials['chrome'], frame,
             segments=14, rotation=(0, math.radians(90), 0))
    for side in (-1, 1):
        cylinder(f'GRIP_{side}', 0.022, 0.11, (side * 0.255, -0.455, 0.92),
                 materials['body_dark'], frame, segments=14, rotation=(0, math.radians(90), 0))
        soft_box(f'SWITCH_{side}', (0.045, 0.05, 0.035), (side * 0.19, -0.47, 0.925),
                 materials['body_dark'], frame, bevel=0.008, subdiv=0)
        # mirrors
        swept_bar(f'MIRROR_STALK_{side}', [
            (side * 0.165, -0.455, 0.935), (side * 0.21, -0.47, 1.02),
            (side * 0.235, -0.485, 1.10)], 0.008, materials['chrome'], frame, segments=8)
        soft_box(f'MIRROR_{side}', (0.028, 0.085, 0.055), (side * 0.24, -0.49, 1.115),
                 materials['body_dark'], frame, bevel=0.012, subdiv=1)

    # handlebar cowl / dashboard + headlight
    soft_box('BAR_COWL', (0.30, 0.12, 0.14), (0, -0.50, 0.955), materials['body'],
             frame, bevel=0.045, subdiv=2)
    soft_box('DASH', (0.16, 0.05, 0.06), (0, -0.545, 1.00), materials['body_dark'],
             frame, bevel=0.015, subdiv=1)
    cylinder('HEADLIGHT', 0.085, 0.10, (0, -0.53, 0.90), materials['body_dark'], frame,
             segments=20, rotation=(math.radians(90), 0, 0))
    cylinder('HEADLIGHT_LENS', 0.065, 0.015, (0, -0.588, 0.90), materials['lens'], frame,
             segments=20, rotation=(math.radians(90), 0, 0))
    torus('HEADLIGHT_BEZEL', 0.072, 0.009, (0, -0.585, 0.90), materials['chrome'], frame,
          major_segments=24, minor_segments=8, rotation=(math.radians(90), 0, 0))
    for side in (-1, 1):
        cylinder(f'INDICATOR_{side}', 0.016, 0.02, (side * 0.135, -0.565, 0.905),
                 materials['amber'], frame, segments=12, rotation=(math.radians(90), 0, 0))

    # front apron / leg shield (soft curved panel)
    soft_box('APRON', (0.34, 0.15, 0.68), (0, -0.45, 0.62), materials['body'],
             frame, bevel=0.05, subdiv=2, rotation=(math.radians(-12), 0, 0))
    for side in (-1, 1):
        soft_box(f'APRON_SIDE_{side}', (0.045, 0.30, 0.42), (side * 0.145, -0.26, 0.47),
                 materials['body'], frame, bevel=0.02, subdiv=1,
                 rotation=(math.radians(-9), 0, 0))
    soft_box('APRON_LOWER', (0.30, 0.20, 0.22), (0, -0.415, 0.32), materials['body_dark'],
             frame, bevel=0.04, subdiv=1)
    soft_box('APRON_GRILL', (0.16, 0.03, 0.10), (0, -0.515, 0.35), materials['body_dark'],
             frame, bevel=0.012, subdiv=1)

    # floorboard + underbone frame
    soft_box('FLOORBOARD', (0.36, 0.52, 0.045), (0, -0.06, 0.275), materials['body_dark'],
             frame, bevel=0.015, subdiv=1)
    soft_box('FRAME_TUNNEL', (0.20, 0.55, 0.16), (0, -0.05, 0.38), materials['body'],
             frame, bevel=0.03, subdiv=1)
    soft_box('UNDERSEAT_PANEL', (0.26, 0.24, 0.20), (0, 0.12, 0.50), materials['body'],
             frame, bevel=0.04, subdiv=2, rotation=(math.radians(-14), 0, 0))

    # rear body + seat + rear rack
    soft_box('REAR_BODY', (0.32, 0.66, 0.30), (0, 0.33, 0.50), materials['body'],
             frame, bevel=0.06, subdiv=2)
    soft_box('REAR_TAIL', (0.27, 0.18, 0.22), (0, 0.72, 0.58), materials['body'],
             frame, bevel=0.05, subdiv=2, rotation=(math.radians(22), 0, 0))
    soft_box('SEAT', (0.29, 0.60, 0.085), (0, 0.27, 0.72), materials['seat'],
             frame, bevel=0.04, subdiv=2, rotation=(math.radians(-3), 0, 0))
    soft_box('SEAT_FRONT_STEP', (0.24, 0.14, 0.045), (0, -0.03, 0.745), materials['seat'],
             frame, bevel=0.018, subdiv=1)
    swept_bar('GRAB_RAIL_L', [(-0.12, 0.56, 0.76), (-0.13, 0.66, 0.79), (-0.11, 0.74, 0.79)],
              0.012, materials['chrome'], frame, segments=8)
    swept_bar('GRAB_RAIL_R', [(0.12, 0.56, 0.76), (0.13, 0.66, 0.79), (0.11, 0.74, 0.79)],
              0.012, materials['chrome'], frame, segments=8)
    soft_box('TAIL_LIGHT', (0.15, 0.05, 0.045), (0, 0.80, 0.66), materials['tail'],
             frame, bevel=0.012, subdiv=1)
    soft_box('PLATE_HOLDER', (0.14, 0.02, 0.10), (0, 0.82, 0.55), materials['body_dark'],
             frame, bevel=0.006, subdiv=0)

    # rear fender (over tire) + mudflap
    arc_strip('REAR_FENDER', 0.625, 0.278, 0.16, 0.03, 12, 200,
              materials['body'], frame, segments=18)
    soft_box('MUDFLAP', (0.14, 0.03, 0.16), (0, 0.86, 0.30), materials['body_dark'],
             frame, bevel=0.008, subdiv=0, rotation=(math.radians(12), 0, 0))

    # engine block + CVT cover + exhaust
    soft_box('ENGINE_BLOCK', (0.20, 0.42, 0.20), (0, 0.52, 0.36), materials['engine'],
             frame, bevel=0.03, subdiv=1)
    cylinder('CVT_COVER', 0.115, 0.075, (-0.115, 0.50, 0.34), materials['engine'], frame,
             segments=24, rotation=(0, math.radians(90), 0))
    cylinder('CVT_RING', 0.118, 0.02, (-0.145, 0.50, 0.34), materials['body_dark'], frame,
             segments=24, rotation=(0, math.radians(90), 0))
    swept_bar('EXHAUST_PIPE', [(0.06, 0.42, 0.30), (0.10, 0.55, 0.27), (0.12, 0.72, 0.28)],
              0.020, materials['engine'], frame, segments=10)
    cone('MUFFLER', 0.045, 0.062, 0.34, (0.135, 0.88, 0.30), materials['engine'], frame,
         segments=18, rotation=(math.radians(90), 0, 0))
    soft_box('HEAT_SHIELD', (0.045, 0.24, 0.03), (0.115, 0.87, 0.345), materials['chrome'],
             frame, bevel=0.006, subdiv=0)

    # swingarm + side stand
    for side in (-1, 1):
        soft_box(f'SWINGARM_{side}', (0.045, 0.42, 0.055), (side * 0.10, 0.50, 0.26),
                 materials['engine'], frame, bevel=0.012, subdiv=1,
                 rotation=(math.radians(-4), 0, 0))
    swept_bar('SIDE_STAND', [(-0.16, 0.30, 0.24), (-0.245, 0.36, 0.05)], 0.012,
              materials['engine'], frame, segments=8)
    soft_box('SIDE_STAND_FOOT', (0.05, 0.09, 0.015), (-0.245, 0.37, 0.045),
             materials['engine'], frame, bevel=0.004, subdiv=0)

    # stats
    stats = {}
    total = 0
    for obj in bpy.data.objects:
        if obj.type != 'MESH':
            continue
        tris = sum(max(len(p.vertices) - 2, 1) for p in obj.data.polygons)
        total += tris
        stats[obj.name] = tris
    return {'objects': len(stats), 'triangles': total,
            'materials': sorted({m.name for m in bpy.data.materials}), 'perObject': stats}


def main():
    summary = main_build()
    BLEND.mkdir(parents=True, exist_ok=True)
    EXPORT.mkdir(parents=True, exist_ok=True)
    (BLEND / 'scooter_build_stats.json').write_text(json.dumps(summary, indent=1) + '\n')
    bpy.ops.wm.save_as_mainfile(filepath=str(BLEND / 'DamarScooter.blend'))
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.export_scene.gltf(filepath=str(EXPORT / 'DamarScooter.glb'),
                              export_format='GLB', export_apply=True)
    bpy.ops.export_scene.fbx(filepath=str(EXPORT / 'DamarScooter.fbx'),
                             use_selection=True, embed_textures=True)
    print('SCOOTER_BUILD objects=%d triangles=%d' % (summary['objects'], summary['triangles']))


if __name__ == '__main__':
    main()
