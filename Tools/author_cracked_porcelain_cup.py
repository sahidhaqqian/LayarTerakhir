"""Original Blender 4 ceramic evidence cup, authored in metres without source models.

Run: blender -b --python Tools/author_cracked_porcelain_cup.py
The lathed wall contains a recessed fracture and a chipped lip. The cavity,
closed ceramic base, open handle and inset milk are actual mesh geometry.
The editable .blend stays outside Assets so Unity imports only the game FBX.
"""
import argparse
import hashlib
import json
import math
import sys
import time
from pathlib import Path

import bmesh
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[1]
ART = ROOT / 'Assets/_LayarTerakhir/assets/Art/PrologRevision'
SOURCE = ROOT / 'ArtSources/Prolog/CrackedPorcelainCup.blend'
PREVIEW = ROOT / 'Audit/Visuals/Prolog-Cup-Blender'
HEIGHT = .095


def material(name, color, roughness):
    result = bpy.data.materials.new(name)
    result.diffuse_color = (*color, 1)
    result.use_nodes = True
    bsdf = result.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = (*color, 1)
    bsdf.inputs['Roughness'].default_value = roughness
    return result


def outer_radius(height):
    # A slight flare, a rounded heel, and an ordinary thin ceramic wall.
    return .0324 + .0055 * ((max(.009, height) - .009) / .085) ** .8


def crack_angle(height):
    controls = [(.044, -1.78), (.052, -1.79), (.060, -1.812),
                (.069, -1.798), (.077, -1.826), (.085, -1.81), (.095, -1.837)]
    for (za, aa), (zb, ab) in zip(controls, controls[1:]):
        if height <= zb:
            t = max(0, min(1, (height - za) / (zb - za)))
            return aa + (ab - aa) * t
    return controls[-1][1]


def shell(glaze, core, fracture):
    # Continuous cross-section: underside -> foot -> outer wall -> round lip
    # -> inner wall -> bowl floor. It closes at the axis, never across the mouth.
    profile = [(0, .0038, 'base'), (.021, .0038, 'base'),
               (.0235, .0025, 'base'), (.0245, .0003, 'base'),
               (.0275, .0003, 'base'), (.0285, .002, 'base'),
               (.029, .0046, 'outer'), (.031, .007, 'outer')]
    heights = [.010 + i * (.0836 / 26) for i in range(27)]
    profile += [(outer_radius(z), z, 'outer') for z in heights]
    lip_center = outer_radius(.0936) - .0014
    profile += [(lip_center + math.cos(i * math.pi / 8) * .0014,
                 .0936 + math.sin(i * math.pi / 8) * .0014, 'lip') for i in range(1, 9)]
    profile += [(outer_radius(z) - .0028, z, 'inner') for z in reversed(heights[:-1])]
    profile += [(.0275, .0094, 'inner'), (.020, .0091, 'inner'), (0, .0091, 'inner')]
    # Extra angular samples create a sub-millimetre V groove while the remainder
    # of the vessel retains a modest game mesh density.
    angles = sorted(set([-math.pi + j * math.tau / 64 for j in range(64)] +
                        [-.040, -.025, -.012, -.006, -.002, 0, .002, .006, .012, .025, .040]))
    count = len(angles)
    vertices, faces, slots, coords = [], [], [], []
    for radius, z, region in profile:
        for angle in angles:
            theta = crack_angle(z) + angle
            wear = .000025 * math.sin(theta * 7 + z * 130) if radius else 0
            taper = max(0, min(1, (z - .044) / .020))
            groove_width = .0048 * taper
            groove = .00034 * taper * max(0, 1 - abs(angle) / max(.00001, groove_width))
            r = radius + wear + (groove if region == 'inner' else -groove)
            # A small asymmetric rim chip follows the real ceramic cross-section.
            rim_taper = max(0, min(1, (z - .090) / .004))
            chip = .00145 * rim_taper * max(0, 1 - abs(angle + .004) / .041) ** .72
            vertices.append((r * math.cos(theta), r * math.sin(theta), z - chip))
    for ring in range(len(profile) - 1):
        za, zb = profile[ring][1], profile[ring + 1][1]
        region = profile[ring][2]
        for j, angle in enumerate(angles):
            following = (j + 1) % count
            faces.append((ring*count+j, ring*count+following,
                          (ring+1)*count+following, (ring+1)*count+j))
            middle = (angle + (angles[following] if following else math.pi)) * .5
            height = (za + zb) * .5
            taper = max(0, min(1, (height - .044) / .020))
            deep = abs(middle) < .0022 * taper and height > .047
            chip = height > .092 and abs(middle + .004) < .030
            slots.append(2 if deep else 1 if chip else 0)
            u0 = (angle + math.pi) / math.tau
            u1 = ((angles[following] + math.pi) / math.tau) if following else 1
            coords.append(((u0, ring/(len(profile)-1)), (u1, ring/(len(profile)-1)),
                           (u1, (ring+1)/(len(profile)-1)), (u0, (ring+1)/(len(profile)-1))))
    mesh = bpy.data.meshes.new('ClosedCeramicWallWithRecessedFracture')
    mesh.from_pydata(vertices, [], faces)
    mesh.materials.clear()
    for m in [glaze, core, fracture]:
        mesh.materials.append(m)
    uv = mesh.uv_layers.new(name='CeramicUV')
    for face, slot, points in zip(mesh.polygons, slots, coords):
        face.material_index = slot
        face.use_smooth = slot == 0
        for loop, point in zip(face.loop_indices, points):
            uv.data[loop].uv = point
    bm = bmesh.new(); bm.from_mesh(mesh)
    bmesh.ops.remove_doubles(bm, verts=list(bm.verts), dist=.0000001)
    bmesh.ops.recalc_face_normals(bm, faces=list(bm.faces))
    bm.to_mesh(mesh); bm.free(); mesh.update()
    obj = bpy.data.objects.new('PorcelainShell', mesh)
    bpy.context.collection.objects.link(obj)
    return obj


def handle(glaze):
    # Thick at the attachment pads and thinner along the open C-shaped grip.
    path = [(outer_radius(.078)-.001, 0, .078), (.048, 0, .081),
            (.060, 0, .074), (.064, 0, .060), (.060, 0, .044),
            (.048, 0, .032), (outer_radius(.031)-.001, 0, .031)]
    curve = bpy.data.curves.new('RoundedOpenCupHandle', 'CURVE')
    curve.dimensions = '3D'; curve.resolution_u = 5
    curve.bevel_depth = .0042; curve.bevel_resolution = 2; curve.use_fill_caps = True
    spline = curve.splines.new('BEZIER'); spline.bezier_points.add(len(path)-1)
    for i, (point, position) in enumerate(zip(spline.bezier_points, path)):
        point.co = position; point.handle_left_type = point.handle_right_type = 'AUTO'
        point.radius = 1.12 if i in (0, len(path)-1) else .9
    obj = bpy.data.objects.new('PorcelainHandle', curve)
    bpy.context.collection.objects.link(obj); curve.materials.append(glaze)
    bpy.ops.object.select_all(action='DESELECT'); obj.select_set(True)
    bpy.context.view_layer.objects.active = obj; bpy.ops.object.convert(target='MESH')
    bm=bmesh.new();bm.from_mesh(obj.data)
    bmesh.ops.remove_doubles(bm,verts=list(bm.verts),dist=.0000001)
    bmesh.ops.recalc_face_normals(bm,faces=list(bm.faces))
    bm.to_mesh(obj.data);bm.free();obj.data.update()
    for poly in obj.data.polygons:
        poly.use_smooth = True
    return obj


def milk_surface(milk):
    sides = 64
    radius = outer_radius(.073) - .00285
    vertices = [(0, 0, .073)]
    for ring in [radius-.001, radius]:
        for i in range(sides):
            angle = i*math.tau/sides
            vertices.append((ring*math.cos(angle), ring*math.sin(angle), .073 + (.00065 if ring==radius else 0)))
    faces = []
    for i in range(sides):
        j=(i+1)%sides
        faces.append((0, i+1, j+1))
        faces.append((i+1, sides+i+1, sides+j+1, j+1))
    mesh=bpy.data.meshes.new('InsetMilkMeniscus');mesh.from_pydata(vertices, [], faces)
    mesh.materials.append(milk)
    obj=bpy.data.objects.new('SpoiledMilk',mesh);bpy.context.collection.objects.link(obj)
    for poly in mesh.polygons:poly.use_smooth=True
    return obj


def inspect_meshes(objects):
    metrics=[]
    for obj in objects:
        bm=bmesh.new();bm.from_mesh(obj.data)
        metrics.append(dict(name=obj.name, vertices=len(bm.verts),
                            triangles=sum(len(f.verts)-2 for f in bm.faces),
                            boundaryEdges=sum(e.is_boundary for e in bm.edges),
                            nonManifoldEdges=sum(not e.is_manifold for e in bm.edges)))
        bm.free()
    body=next(m for m in metrics if m['name']=='PorcelainShell')
    assert body['boundaryEdges']==body['nonManifoldEdges']==0, body
    grip=next(m for m in metrics if m['name']=='PorcelainHandle')
    assert grip['boundaryEdges']==grip['nonManifoldEdges']==0, grip
    assert sum(m['triangles'] for m in metrics)<14000, metrics
    return metrics


def studio(objects):
    scene=bpy.context.scene
    scene.render.engine='CYCLES';scene.cycles.samples=32;scene.cycles.use_denoising=False
    scene.render.resolution_x=960;scene.render.resolution_y=960;scene.render.resolution_percentage=100
    scene.world=bpy.data.worlds.new('CupStudioWorld');scene.world.color=(.12,.12,.12)
    scene.view_settings.view_transform='AgX'
    scene.view_settings.exposure=-1.5
    camera_data=bpy.data.cameras.new('CupReviewCamera');camera=bpy.data.objects.new('CupReviewCamera',camera_data)
    bpy.context.collection.objects.link(camera);scene.camera=camera
    camera_data.lens=65;camera_data.clip_start=.003
    for name, position, energy, size in [
        ('LargeSoftKey',(-.12,-.15,.23),12,.16),
        ('RimSoftbox',(.16,.06,.17),9,.10),
        ('GentleFill',(-.1,.1,.08),3,.12)]:
        data=bpy.data.lights.new(name,'AREA');data.energy=energy;data.shape='DISK';data.size=size
        light=bpy.data.objects.new(name,data);bpy.context.collection.objects.link(light);light.location=position
        light.rotation_euler=(Vector((0,0,.05))-light.location).to_track_quat('-Z','Y').to_euler()
    bpy.ops.mesh.primitive_plane_add(size=200,location=(0,0,0))
    floor=bpy.context.object;floor.name='StudioBackdrop';floor.data.materials.append(material('StudioSlate',(.018,.022,.025),.75))
    views=[('cup-front',(.145,-.255,.16),(0,0,.049)),
           ('cup-fracture-close',(-.012,-.14,.11),(-.010,-.034,.075)),
           ('cup-cavity',(.14,-.18,.23),(0,0,.052))]
    PREVIEW.mkdir(parents=True,exist_ok=True)
    # Save the authored model and its inspectable studio, without importing it to Unity.
    camera.location=views[0][1];camera.rotation_euler=(Vector(views[0][2])-camera.location).to_track_quat('-Z','Y').to_euler()
    SOURCE.parent.mkdir(parents=True,exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE),check_existing=False)
    return views


def build_cup(previews=False):
    started=time.monotonic()
    bpy.ops.wm.read_factory_settings(use_empty=True)
    glaze=material('CeramicGlaze',(.78,.76,.69),.22)
    core=material('CeramicCore',(.59,.53,.43),.68)
    fracture=material('CeramicFracture',(.095,.061,.037),.82)
    milk=material('SpoiledMilk',(.68,.66,.53),.35)
    objects=[shell(glaze,core,fracture),handle(glaze),milk_surface(milk)]
    metrics=inspect_meshes(objects)
    ART.joinpath('Models').mkdir(parents=True,exist_ok=True)
    bpy.ops.object.select_all(action='DESELECT')
    for obj in objects:obj.select_set(True)
    bpy.context.view_layer.objects.active=objects[0]
    fbx=ART/'Models/CrackedPorcelainCup.fbx'
    bpy.ops.export_scene.fbx(filepath=str(fbx),use_selection=True,object_types={'MESH'},bake_anim=False,
                             axis_forward='-Z',axis_up='Y',path_mode='STRIP')
    views=studio(objects)
    if previews:
        for name, position, target in views:
            camera=bpy.context.scene.camera;camera.location=position
            camera.rotation_euler=(Vector(target)-camera.location).to_track_quat('-Z','Y').to_euler()
            bpy.context.scene.render.filepath=str(PREVIEW/(name+'.png'));bpy.ops.render.render(write_still=True)
    receipt=dict(originalModel=True,externalModelSources=[],units='metres',
                 nominalHeight=HEIGHT,nominalWallThickness=.0028,fractureDepth=.00034,
                 fractureMaxWidth=.00036,rimChipDepth=.00145,milkHeight=.073,
                 meshes=metrics,triangles=sum(m['triangles'] for m in metrics),
                 source=SOURCE.relative_to(ROOT).as_posix(),fbx=fbx.relative_to(ROOT).as_posix(),
                 sourceSha256=hashlib.sha256(SOURCE.read_bytes()).hexdigest(),
                 fbxSha256=hashlib.sha256(fbx.read_bytes()).hexdigest(),
                 previewsRendered=previews,elapsedSeconds=time.monotonic()-started)
    (ROOT/'Audit/prolog-cup-authoring.json').write_text(json.dumps(receipt,indent=2)+'\n')
    print('ORIGINAL_PORCELAIN_CUP_AUTHORED',json.dumps(receipt),flush=True)
    return receipt


if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--no-previews',action='store_true')
    arguments=sys.argv[sys.argv.index('--')+1:] if '--' in sys.argv else []
    build_cup(previews=not parser.parse_args(arguments).no_previews)
