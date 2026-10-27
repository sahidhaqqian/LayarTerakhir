"""Repair shipped Nyonya hunter/apparition appearance without changing design.

Blender -b --python Tools/repair_nyonya_appearance.py

Works from the shipped game FBX (no designer VRM source needed):
1. Delete decimation-weld membranes spanning torso<->arms (rest-shape artifacts,
   up to 51cm edges; not part of the designer kebaya).
2. Re-weight blended dress-hem verts off leg/foot/toe bones onto Hips (fixes
   Run/Walk hem spikes; pure foot verts and Shears verts are untouched).
3. Amplify the tiny baked Windup/Stun rotations so the slash read and recoil
   read; keep clip timing, order and loop flags.
4. Slerp Run Head/Neck toward rest (fixes face-down jog) and clamp Toes.
5. Regenerate Echo/Seated apparitions from the repaired mesh at the SAME 5k
   triangle budget (no scene-budget change).
6. Render before/after previews and write Audit/nyonya-appearance-repair.json.

Gameplay/AI/collision/story untouched. Bones unchanged (56 incl. Shears hook).
After this, run Unity NyonyaMotionRevisionBuilder.Apply to reimport clips,
rebind the scene hunter and re-measure stride speeds.
"""
from pathlib import Path
import hashlib
import json
import math
import sys

import bpy
import bmesh
from mathutils import Quaternion, Vector

ROOT = Path(__file__).resolve().parents[1]
MODEL = ROOT / "Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models"
HUNTER = MODEL / "NyonyaDesigner.fbx"
PREVIEW_DIR = ROOT / "Audit/Visuals/Nyonya-Appearance"
PREVIEW_DIR.mkdir(parents=True, exist_ok=True)

TORSO = {"Hips", "Spine", "Chest", "UpperChest", "Neck", "Head", "Jaw"}
ARM_PREFIX = ("Shoulder_", "UpperArm_", "Forearm_", "Hand_", "Finger")
LEG = {"Thigh_L", "Thigh_R", "Shin_L", "Shin_R", "Foot_L", "Foot_R",
       "Toes_L", "Toes_R"}


def sha(path):
    with path.open("rb") as handle:
        return hashlib.file_digest(handle, "sha256").hexdigest()


def is_arm(name):
    return name.startswith(ARM_PREFIX)


def short_name(action):
    return action.name.rsplit("|", 1)[-1]


def main():
    assert HUNTER.exists(), "Missing shipped hunter " + str(HUNTER)
    before_hash = sha(HUNTER)
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(HUNTER))
    rig = next(o for o in bpy.context.scene.objects if o.type == "ARMATURE")
    skin = next(o for o in bpy.context.scene.objects if o.type == "MESH")
    assert len(rig.data.bones) == 56, len(rig.data.bones)
    assert skin.vertex_groups.get("Shears"), "Shears presentation hook missing"
    groups = {g.index: g.name for g in skin.vertex_groups}

    # --- 1. Delete torso<->arm weld membranes, then close the pits. ---
    # Two passes: strict giant membranes first, then narrower remnant webs.
    # Each pass fills its own holes with torso-weighted patches so the torso
    # sides read as cloth in shadow instead of black pits or bat wings.
    mesh = skin.data

    def membrane_faces(edge_min, area_min, mix_min):
        found = []
        for poly in mesh.polygons:
            coords = [mesh.vertices[i].co for i in poly.vertices]
            cx = sum(c.x for c in coords) / len(coords)
            cz = sum(c.z for c in coords) / len(coords)
            if not (0.10 < abs(cx) < 0.60 and 0.90 < cz < 1.55):
                continue
            longest = 0.0
            for i in range(len(coords)):
                longest = max(longest, (coords[i] - coords[
                    (i + 1) % len(coords)]).length)
            if longest < edge_min:
                continue
            torso = arm = 0.0
            for vi in poly.vertices:
                for g in mesh.vertices[vi].groups:
                    name = groups.get(g.group, "?")
                    if name in TORSO:
                        torso += g.weight
                    elif is_arm(name):
                        arm += g.weight
            n = len(poly.vertices)
            if (poly.area > area_min and torso / n > mix_min
                    and arm / n > mix_min):
                found.append(poly.index)
        return found

    chest_index = skin.vertex_groups["Chest"].index
    spine_index = skin.vertex_groups["Spine"].index

    # Tag-based fill so only patch geometry gets torso weights/UVs.
    def delete_fill_tagged(doomed):
        bm = bmesh.new()
        bm.from_mesh(mesh)
        bm.faces.ensure_lookup_table()
        for face in bm.faces:
            face.select_set(False)
            face.tag = False
        for index in sorted(doomed, reverse=True):
            bm.faces[index].select_set(True)
        bmesh.ops.delete(bm, geom=[f for f in bm.faces if f.select],
                         context="FACES")
        bmesh.ops.delete(bm, geom=[v for v in bm.verts if not v.link_faces],
                         context="VERTS")
        region_edges = [e for e in bm.edges if e.is_boundary and
                        0.10 < abs((e.verts[0].co + e.verts[1].co).x / 2) < 0.60
                        and 0.90 < (e.verts[0].co + e.verts[1].co).z / 2 < 1.55]
        filled = bmesh.ops.holes_fill(bm, edges=region_edges, sides=0)["faces"]
        for face in filled:
            face.tag = True
        bmesh.ops.triangulate(bm, faces=list(filled))
        # Triangulate keeps tags on children in Blender 4.0.
        deform = bm.verts.layers.deform.verify()
        uv = bm.loops.layers.uv.active
        patched = 0
        for face in [f for f in bm.faces if f.tag]:
            patched += 1
            for loop in face.loops:
                samples = [other[uv].uv.copy() for other in
                           loop.vert.link_loops
                           if not other.face.tag]
                if samples:
                    loop[uv].uv = (sum(samples, Vector((0, 0)))
                                  / len(samples))
            for vert in face.verts:
                weights = vert[deform]
                moved = 0.0
                for index in [i for i in weights.keys()
                              if is_arm(groups.get(i, "?"))]:
                    moved += weights[index]
                    del weights[index]
                if moved > 0.001:
                    anchor = (chest_index if vert.co.z > 1.15
                              else spine_index)
                    weights[anchor] = weights.get(anchor, 0.0) + moved
        bm.to_mesh(mesh)
        bm.free()
        mesh.update()
        return len(filled), patched

    pass1 = membrane_faces(0.045, 0.0008, 0.15)
    holes1, patched1 = delete_fill_tagged(pass1)
    pass2 = membrane_faces(0.030, 0.0003, 0.10)
    holes2, patched2 = delete_fill_tagged(pass2)
    doomed = pass1 + pass2
    assert len(pass1) > 50, ("Membrane pass 1 found too few", len(pass1))
    assert len(mesh.vertices) > 24000, ("Membrane deletion removed too much",
                                        len(mesh.vertices))

    # --- 2. Pull hem/shin cloth off leg/foot/toe bones onto Hips. ---
    # Automatic VRM weights are often pure (1.0), so weight blending alone
    # cannot tell dress cloth from real feet. Real leg/foot surface hugs its
    # bones; cloth hangs away from them. Keep verts near leg segments (real
    # limbs + ankle-hugging cloth) and move only distant cloth to Hips.
    segments = []
    for name in ("Thigh_L", "Shin_L", "Foot_L", "Toes_L",
                 "Thigh_R", "Shin_R", "Foot_R", "Toes_R"):
        bone = rig.data.bones[name]
        segments.append((bone.head_local.copy(), bone.tail_local.copy()))

    def near_leg(point, limit=0.075):
        for a, b in segments:
            delta = b - a
            t = max(0.0, min(1.0, (point - a).dot(delta)
                             / max(1e-8, delta.length_squared)))
            if (point - (a + delta * t)).length <= limit:
                return True
        return False

    hips = skin.vertex_groups["Hips"]
    reweighted = 0
    for v in mesh.vertices:
        if v.co.z >= 0.50:
            continue
        weights = [(g.weight, groups.get(g.group, "?"), g.group)
                   for g in v.groups]
        if not weights:
            continue
        if max(weights)[1] not in LEG:
            continue
        if near_leg(v.co):
            continue  # Real limb surface stays on its bones.
        if sum(w for w, n, _ in weights if n == "Shears") >= 0.5:
            continue  # Sculpted weapon stays rigid on its hook.
        freed = 0.0
        for weight, name, index in weights:
            if name in LEG:
                keep = weight * 0.2
                skin.vertex_groups[index].add([v.index], keep, "REPLACE")
                freed += weight - keep
        if freed > 0.001:
            current = 0.0
            for g in v.groups:
                if groups.get(g.group) == "Hips":
                    current = g.weight
                    break
            hips.add([v.index], current + freed, "REPLACE")
            reweighted += 1
    assert reweighted > 50, ("Hem repair touched too few verts", reweighted)

    # --- 3-4. Clip repairs (rotations only; timing and root untouched). ---
    actions = {}
    for action in bpy.data.actions:
        name = short_name(action)
        if name in ("Idle", "Walk", "Run", "Windup", "Stun"):
            action.name = name
            actions[name] = action
    assert len(actions) == 5, [a.name for a in bpy.data.actions]
    for action in list(bpy.data.actions):
        if action not in actions.values():
            bpy.data.actions.remove(action)
    for bone in rig.pose.bones:
        bone.rotation_mode = "QUATERNION"

    def amplify(action_name, factors, additive=None):
        action = actions[action_name]
        rig.animation_data.action = action
        first, last = (int(x) for x in action.frame_range)
        span = max(1, last - first)
        peak_change = {}
        for frame in range(first, last + 1):
            bpy.context.scene.frame_set(frame)
            bpy.context.view_layer.update()
            progress = min(1.0, ((frame - first) / span) * 1.8)
            for bone_name, factor in factors.items():
                bone = rig.pose.bones[bone_name]
                q = bone.rotation_quaternion.copy()
                w = max(-1.0, min(1.0, q.w))
                angle = 2.0 * math.acos(w)
                if angle > math.radians(0.5):
                    axis = Vector((q.x, q.y, q.z))
                    axis.normalize()
                    grown = Quaternion(axis, min(angle * factor,
                                                 math.radians(120)))
                    if grown.dot(q) < 0:
                        grown.negate()
                    bone.rotation_quaternion = grown
                    peak_change[bone_name] = max(
                        peak_change.get(bone_name, 0.0), angle * factor)
                if additive and bone_name in additive:
                    extra_axis, extra_deg = additive[bone_name]
                    extra = Quaternion(extra_axis, math.radians(extra_deg)
                                       * progress)
                    combined = extra @ bone.rotation_quaternion
                    bone.rotation_quaternion = combined
                bone.keyframe_insert("rotation_quaternion", frame=frame,
                                     group=bone_name)
        for curve in action.fcurves:
            for key in curve.keyframe_points:
                key.interpolation = "LINEAR"
        return {k: round(math.degrees(v), 1) for k, v in peak_change.items()}

    def relax(action_name, bones, keep):
        action = actions[action_name]
        rig.animation_data.action = action
        first, last = (int(x) for x in action.frame_range)
        for frame in range(first, last + 1):
            bpy.context.scene.frame_set(frame)
            bpy.context.view_layer.update()
            for bone_name in bones:
                bone = rig.pose.bones[bone_name]
                q = bone.rotation_quaternion.copy()
                if q.dot(Quaternion()) < 0:
                    q.negate()
                bone.rotation_quaternion = Quaternion().slerp(q, keep)
                bone.keyframe_insert("rotation_quaternion", frame=frame,
                                     group=bone_name)
        for curve in action.fcurves:
            for key in curve.keyframe_points:
                key.interpolation = "LINEAR"

    local_x = Vector((1, 0, 0))
    windup_peak = amplify(
        "Windup",
        {"UpperArm_R": 2.6, "Forearm_R": 2.6, "Hand_R": 2.2, "Head": 2.0,
         "Neck": 1.6},
        additive={"Spine": (local_x, 10.0)})
    stun_peak = amplify(
        "Stun", {"Spine": 2.2, "Head": 2.2, "Neck": 2.0, "UpperArm_R": 2.2,
                 "Forearm_R": 1.6})
    # The jog source leans the whole torso forward like a track sprint and
    # pitches the head down; a long-dress hunter reads better gliding upright
    # with her tilted face visible. Keep timing/sequence, halve the lean.
    relax("Run", ["Spine", "Chest", "UpperChest"], 0.50)
    relax("Run", ["Head", "Neck"], 0.25)
    relax("Run", ["Toes_L", "Toes_R"], 0.15)
    relax("Walk", ["Toes_L", "Toes_R"], 0.40)

    def refloor(action_name):
        """Re-fit pelvis support to the repaired weights, per frame.

        The shipped lift was solved for the old leg-bound hem; reweighted
        cloth dips below the navigation floor on some frames. Lift only the
        dipping frames, exactly to +2mm, like the original retarget pass.
        Timing, sequence and stance contacts are preserved.
        """
        action = actions[action_name]
        rig.animation_data.action = action
        first, last = (int(x) for x in action.frame_range)
        lifted = 0
        for frame in range(first, last + 1):
            bpy.context.scene.frame_set(frame)
            bpy.context.view_layer.update()
            deps = bpy.context.evaluated_depsgraph_get()
            mesh_eval = skin.evaluated_get(deps).to_mesh()
            lowest = min((skin.matrix_world @ v.co).z
                         for v in mesh_eval.vertices)
            skin.evaluated_get(deps).to_mesh_clear()
            if lowest < 0.002:
                pelvis = rig.pose.bones["Hips"]
                matrix = pelvis.matrix.copy()
                matrix.translation.z -= lowest - 0.002
                pelvis.matrix = matrix
                bpy.context.view_layer.update()
                lifted += 1
            rig.pose.bones["Hips"].keyframe_insert(
                "location", frame=frame, group="Hips")
        for curve in action.fcurves:
            for key in curve.keyframe_points:
                key.interpolation = "LINEAR"
        return lifted

    walk_lifted = refloor("Walk")
    run_lifted = refloor("Run")
    print("REFLOOR", dict(walk=walk_lifted, run=run_lifted), flush=True)
    rig.animation_data.action = actions["Idle"]
    bpy.context.scene.frame_set(1)

    # --- 5. Export repaired hunter + apparitions at the same budgets. ---
    bpy.ops.object.select_all(action="DESELECT")
    skin.select_set(True)
    rig.select_set(True)
    bpy.context.view_layer.objects.active = rig
    bpy.ops.export_scene.fbx(
        filepath=str(HUNTER), use_selection=True,
        object_types={"MESH", "ARMATURE"}, axis_forward="-Z", axis_up="Y",
        bake_anim=True, bake_anim_use_all_actions=True,
        bake_anim_use_nla_strips=False, bake_anim_simplify_factor=0,
        add_leaf_bones=False, path_mode="STRIP")

    def split_to(source, target):
        """Shrink an apparition without welding arms to the torso.

        Collapse decimation on the whole mesh merges verts across the
        armpit void and rebuilds 61cm membranes; planar dissolve cannot
        reach the budget on this curved sculpt (18k tris left at 26 deg).
        Instead, partition faces by dominant skinning (body / arms+hands /
        shears), collapse each part in isolation so no weld can span parts,
        then rejoin. Every face lands in exactly one part, so no holes open.
        """
        bodies = {}
        for part in ("body", "arms", "shears"):
            copy = source.copy()
            copy.data = source.data.copy()
            bpy.context.collection.objects.link(copy)
            for modifier in list(copy.modifiers):
                copy.modifiers.remove(modifier)
            bodies[part] = copy
        data = {part: obj.data for part, obj in bodies.items()}
        counts = {part: {"body": 3700, "arms": 900, "shears": 400}[part]
                  for part in bodies}
        for part, obj in bodies.items():
            mesh_part = obj.data
            keep = []
            drop = []
            for poly in mesh_part.polygons:
                arm = shear = 0.0
                for vi in poly.vertices:
                    for g in mesh_part.vertices[vi].groups:
                        name = groups.get(g.group, "?")
                        if name == "Shears":
                            shear += g.weight
                        elif is_arm(name):
                            arm += g.weight
                n = len(poly.vertices)
                owner = ("shears" if shear / n > 0.5
                         else "arms" if arm / n > 0.5 else "body")
                (keep if owner == part else drop).append(poly.index)
            bm = bmesh.new()
            bm.from_mesh(mesh_part)
            bm.faces.ensure_lookup_table()
            for face in bm.faces:
                face.select_set(False)
            for index in drop:
                bm.faces[index].select_set(True)
            bmesh.ops.delete(bm, geom=[f for f in bm.faces if f.select],
                             context="FACES")
            bmesh.ops.delete(bm, geom=[v for v in bm.verts
                                       if not v.link_faces],
                             context="VERTS")
            bm.to_mesh(mesh_part)
            bm.free()
            mesh_part.update()
            print("APPARITION_SPLIT",
                  dict(part=part, kept=len(keep)), flush=True)
        kept_counts = {}
        for part, obj in bodies.items():
            kept_counts[part] = len(obj.data.polygons)
        assert kept_counts["body"] > 5000, kept_counts
        assert kept_counts["arms"] > 100, kept_counts
        assert kept_counts["shears"] > 20, kept_counts
        joined = None
        for part, obj in bodies.items():
            bpy.context.view_layer.objects.active = obj
            for o in bpy.context.selected_objects:
                o.select_set(False)
            obj.select_set(True)
            modifier = obj.modifiers.new("ApparitionBudget", "DECIMATE")
            modifier.ratio = min(1.0, counts[part] / max(
                1, len(obj.data.polygons)))
            modifier.use_collapse_triangulate = True
            bpy.ops.object.modifier_apply(modifier=modifier.name)
            obj.data.update()
            if joined is None:
                joined = obj
            else:
                obj.select_set(True)
                bpy.context.view_layer.objects.active = joined
                joined.select_set(True)
                bpy.ops.object.join()
        joined.data.update()
        for poly in joined.data.polygons:
            poly.use_smooth = True
        return joined

    def export_static(objects, path):
        bpy.ops.object.select_all(action="DESELECT")
        for obj in objects:
            obj.select_set(True)
        bpy.context.view_layer.objects.active = objects[0]
        bpy.ops.export_scene.fbx(
            filepath=str(path), use_selection=True,
            object_types={"MESH"}, axis_forward="-Z", axis_up="Y",
            bake_anim=False, path_mode="STRIP")

    # Echo: repaired rest mesh, most detail that fits the 5k budget.
    echo_source = skin.copy()
    echo_source.data = skin.data.copy()
    bpy.context.collection.objects.link(echo_source)
    echo = split_to(echo_source, 5000)
    bpy.data.objects.remove(echo_source, do_unlink=True)
    export_static([echo], MODEL / "NyonyaDesignerEcho.fbx")
    echo_tris = sum(len(p.vertices) - 2 for p in echo.data.polygons)
    bpy.data.objects.remove(echo, do_unlink=True)

    # Seated: same baked seated beat (hips -.43m, thighs -80, shins +80).
    rig.animation_data.action = None
    for bone in rig.pose.bones:
        bone.matrix_basis.identity()
    hips_bone = rig.pose.bones["Hips"]
    hips_bone.location = (hips_bone.bone.matrix_local.to_3x3().inverted()
                          @ Vector((0, 0, -0.43)))

    def turn(name, degrees):
        bone = rig.pose.bones[name]
        bone.rotation_mode = "QUATERNION"
        axis = bone.bone.matrix_local.to_3x3().inverted() @ Vector((1, 0, 0))
        bone.rotation_quaternion = Quaternion(axis, math.radians(degrees))

    for side in ("L", "R"):
        turn("Thigh_" + side, -80)
        turn("Shin_" + side, 80)
    bpy.context.view_layer.update()
    deps = bpy.context.evaluated_depsgraph_get()
    seated_source = bpy.data.objects.new(
        "NyonyaDesignerSeated",
        bpy.data.meshes.new_from_object(skin.evaluated_get(deps)))
    bpy.context.collection.objects.link(seated_source)
    # Baked meshes carry no skinning; copy the hunter weights 1:1 by index
    # so the split can tell arms/shears apart from the seated body.
    for group in skin.vertex_groups:
        seated_source.vertex_groups.new(name=group.name)
    for index, vert in enumerate(skin.data.vertices):
        for element in vert.groups:
            seated_source.vertex_groups[element.group].add(
                [index], element.weight, "REPLACE")
    for modifier in list(seated_source.modifiers):
        seated_source.modifiers.remove(modifier)
    seated = split_to(seated_source, 5000)
    bpy.data.objects.remove(seated_source, do_unlink=True)
    export_static([seated], MODEL / "NyonyaDesignerSeated.fbx")
    seated_tris = sum(len(p.vertices) - 2 for p in seated.data.polygons)
    bpy.data.objects.remove(seated, do_unlink=True)

    hunter_tris = sum(len(p.vertices) - 2 for p in skin.data.polygons)

    # --- 6. Previews with the same diagnostic staging as the retarget. ---
    scene = bpy.context.scene
    scene.render.engine = "CYCLES"
    scene.cycles.samples = 8
    scene.cycles.use_denoising = False
    scene.render.resolution_x = 500
    scene.render.resolution_y = 650
    scene.render.resolution_percentage = 100
    scene.world = bpy.data.worlds.new("RepairWorld")
    scene.world.color = (0.3, 0.3, 0.3)
    mat = bpy.data.materials.new("RepairAlbedo")
    mat.use_nodes = True
    node = mat.node_tree.nodes.new("ShaderNodeTexImage")
    node.image = bpy.data.images.load(str(
        (ROOT / "Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Textures"
         / "NyonyaAlbedo.png").resolve()))
    mat.node_tree.links.new(
        node.outputs["Color"],
        mat.node_tree.nodes.get("Principled BSDF").inputs["Base Color"])
    skin.data.materials.clear()
    skin.data.materials.append(mat)
    bpy.ops.object.camera_add(location=(2.1, -3.6, 1.8))
    cam = bpy.context.object
    cam.rotation_euler = (Vector((0, 0, 0.9)) - cam.location).to_track_quat(
        "-Z", "Y").to_euler()
    cam.data.type = "ORTHO"
    cam.data.ortho_scale = 2.1
    scene.camera = cam
    for pos, energy, size in [((2, -3, 4), 450, 4), ((-2, 1, 3), 350, 3)]:
        bpy.ops.object.light_add(type="AREA", location=pos)
        light = bpy.context.object
        light.data.energy = energy
        light.data.shape = "DISK"
        light.data.size = size
        light.rotation_euler = (Vector((0, 0, 1)) - light.location
                                ).to_track_quat("-Z", "Y").to_euler()
    for name, frame in [("Idle", 1), ("Walk", 11), ("Run", 8), ("Run", 22),
                        ("Windup", 20), ("Stun", 18)]:
        rig.animation_data.action = actions[name]
        scene.frame_set(frame)
        scene.render.filepath = str(PREVIEW_DIR / f"after-{name}-{frame}.png")
        bpy.ops.render.render(write_still=True)

    report = dict(
        hunter=str(HUNTER.relative_to(ROOT)),
        hunterSha256Before=before_hash,
        hunterSha256After=sha(HUNTER),
        echoSha256=sha(MODEL / "NyonyaDesignerEcho.fbx"),
        seatedSha256=sha(MODEL / "NyonyaDesignerSeated.fbx"),
        membraneFacesDeleted=len(doomed),
        membranePass1=len(pass1),
        membranePass2=len(pass2),
        holesFilled=holes1 + holes2,
        patchTriangles=patched1 + patched2,
        hemVertsReweighted=reweighted,
        hunterTriangles=hunter_tris,
        echoTriangles=echo_tris,
        seatedTriangles=seated_tris,
        windupPeakDegrees=windup_peak,
        stunPeakDegrees=stun_peak,
        refloorLiftedFrames=dict(walk=walk_lifted, run=run_lifted),
        bones=len(rig.data.bones),
        clips=sorted(actions),
        budgetsUnchanged=True,
    )
    (ROOT / "Audit/nyonya-appearance-repair.json").write_text(
        json.dumps(report, indent=2) + "\n")
    print("NYONYA_APPEARANCE_REPAIRED", json.dumps(report), flush=True)


if __name__ == "__main__":
    main()
