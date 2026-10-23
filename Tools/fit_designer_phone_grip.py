"""Offline, geometry-based grasp fitting for the designer's replacement Raka.

Called by prepare_designer_vrm.py in Blender. SciPy is authoring-only; no solver,
Python dependency, IK system or extra component is included in the game player.
"""
import math
import os
import sys
from pathlib import Path

import bpy
import numpy as np
from mathutils import Quaternion, Vector

SURFACE_BARYCENTRICS=np.array([(i/4,j/4,(4-i-j)/4) for i in range(5) for j in range(5-i) if max(i,j,4-i-j)<4])

def sample_surface(values,triangles):
    """Sample existing triangle interiors without altering source geometry."""
    if not len(triangles):return values
    sampled=np.einsum('kt,ntd->nkd',SURFACE_BARYCENTRICS,values[triangles])
    return np.concatenate([values,sampled.reshape((-1,values.shape[1]))])


def solve(rig, mesh, defs, pose, hand_frame, handset_extents=None, contact_faces=None):
    # Optional externally installed authoring environment (kept outside Assets).
    extra = Path(os.environ.get('LAYAR_CHARACTER_PYTHON_PATH',
                               Path(__file__).resolve().parents[1] / 'Builds/ArtSources/CharacterPython'))
    if extra.is_dir():
        sys.path.insert(0, str(extra))
    from scipy.optimize import least_squares

    pose(rig, defs, 'PhoneIdle', .25, False)
    bpy.context.view_layer.update()
    names = [f'Finger{finger}_{segment}_R' for finger in range(1, 6) for segment in range(1, 4)]
    bones = list(rig.data.bones)
    index = {b.name: i for i, b in enumerate(bones)}
    inverse = np.array([np.array(b.matrix_local.inverted(), dtype=np.float64) for b in bones])
    groups = {g.index: g.name for g in mesh.vertex_groups}
    vertices = []
    weights = []
    masks = []
    finger_weights=[]
    source_rows={}
    for vertex in mesh.data.vertices:
        row = [(groups[g.group], g.weight) for g in vertex.groups if g.weight > .0001]
        if sum(w for name, w in row if name in names or name in ['Hand_R', 'Forearm_R']) < .25:
            continue
        source_rows[vertex.index]=len(vertices);vertices.append([*vertex.co, 1])
        weight = np.zeros(len(bones))
        for name, w in row:
            if name in index:
                weight[index[name]] += w
        weights.append(weight / weight.sum())
        digits=[sum(w for name,w in row if name.startswith('Finger'+str(f)) and name.endswith('_R')) for f in range(1,6)]
        finger_weights.append(digits);masks.append([w>.25 for w in digits])
    vertices = np.array(vertices, dtype=np.float64)
    weights = np.array(weights)
    masks = np.array(masks, dtype=bool)
    triangles=np.empty((0,3),dtype=int)
    if contact_faces is not None:
        mesh.data.calc_loop_triangles()
        triangles=np.array([[source_rows[i] for i in t.vertices] for t in mesh.data.loop_triangles if all(i in source_rows for i in t.vertices)],dtype=int)
        masks=sample_surface(np.array(finger_weights),triangles)>.25
    used = np.flatnonzero(np.max(weights, axis=0) > 0)
    weights = weights[:, used]
    inverse = inverse[used]
    base = {name: rig.pose.bones[name].rotation_quaternion.copy() for name in names}
    longitudinal, palm, sideways = hand_frame(defs, 'R')
    local_axes = {}
    for name in names:
        bone = rig.data.bones[name]
        direction = (bone.tail_local - bone.head_local).normalized()
        local_axes[name] = bone.matrix_local.to_quaternion().inverted() @ direction.cross(palm).normalized()
    # Pose and rest hand transforms make every candidate independent of the
    # locomotion phase: the same calibrated grasp is baked into all phone clips.
    rest_hand_inverse = rig.data.bones['Hand_R'].matrix_local.inverted()
    phone_center = defs['Hand_R'][0] + longitudinal * .09 + palm * .045
    phone_axes = np.array([tuple(longitudinal), tuple(sideways), tuple(palm)]).T
    extents = np.array(handset_extents if handset_extents is not None else [.039, .0825, .007])
    initial = np.zeros(20)  # 15 curls, thumb opposition/roll, 3 handset offsets.
    initial[17:] = [.00001, .00001, .00001]

    def evaluate(x):
        for i, name in enumerate(names):
            bone = rig.pose.bones[name]
            bone.rotation_mode = 'QUATERNION'
            bone.rotation_quaternion = base[name] @ Quaternion(local_axes[name], math.radians(float(x[i])))
            if i == 0:
                bone.rotation_quaternion @= Quaternion((0, 0, 1), math.radians(float(x[15])))
                bone.rotation_quaternion @= Quaternion((1, 0, 0), math.radians(float(x[16])))
        bpy.context.view_layer.update()
        matrices = np.array([np.array(rig.pose.bones[bones[i].name].matrix, dtype=np.float64) for i in used]) @ inverse
        transformed = np.einsum('bij,vj->vbi', matrices, vertices)
        points = np.einsum('vb,vbi->vi', weights, transformed)[:, :3]
        if contact_faces is not None:points=sample_surface(points,triangles)
        hand = rig.pose.bones['Hand_R'].matrix @ rest_hand_inverse
        rot = np.array(hand.to_3x3(), dtype=np.float64)
        center = np.array(hand @ phone_center, dtype=np.float64) + rot @ phone_axes @ x[17:]
        axes = rot @ phone_axes
        coords = (points - center) @ axes
        signed = extents - np.abs(coords)
        penetration = np.maximum(0, np.min(signed, axis=1))
        gap = np.linalg.norm(np.minimum(0, signed), axis=1)
        contact_values=[]
        for f in range(5):
            face_gap=gap
            if contact_faces is not None:
                # Optional landscape presentation: opposed thumb on its chosen side/front face,
                # four fingers on the back. Default portrait fitting is unchanged.
                if contact_faces[f]=='side':
                    face_gap=np.sqrt((coords[:,0]+extents[0])**2+
                                     np.maximum(0,np.abs(coords[:,1])-extents[1])**2+
                                     np.maximum(0,np.abs(coords[:,2])-extents[2])**2)
                else:
                    face_gap=np.sqrt(np.maximum(0,np.abs(coords[:,0])-extents[0])**2+
                                     np.maximum(0,np.abs(coords[:,1])-extents[1])**2+
                                     (coords[:,2]-contact_faces[f]*extents[2])**2)
            contact_values.append(np.min(face_gap[masks[:,f]]) if np.any(masks[:,f]) else 1)
        contact=np.array(contact_values)
        glass=np.zeros(len(coords))
        if contact_faces is not None:
            inside=(np.abs(coords[:,0])<extents[0]*.86)&(np.abs(coords[:,1])<extents[1]*.92)
            # The opposed thumb may overlap the outer lower corner, as in a
            # real grip. Keep the central feed clear rather than pinning the
            # thumb behind the phone merely to satisfy a screen-plane test.
            thumb_corner=(coords[:,0]<-extents[0]*.64)&(coords[:,1]<.01)
            inside &= ~thumb_corner
            glass=np.where(inside,np.maximum(0,coords[:,2]+extents[2]),0)
        return penetration,contact,glass

    def residual(x):
        penetration, contact, glass = evaluate(x)
        parts=[np.maximum(0, penetration - .0002) * 14000,
                               np.maximum(0, contact - .0018) * 2000,
                               x[:17] * .0004, x[17:] * .2]
        if contact_faces is not None:parts.append(np.maximum(0,glass-.0002)*14000)
        return np.concatenate(parts)

    def jacobian(x):
        base_residual = residual(x)
        columns = []
        for i in range(len(x)):
            step = .4 if i < 17 else .0003
            other = x.copy()
            other[i] += step
            columns.append((residual(other) - base_residual) / step)
        return np.array(columns).T

    before = evaluate(initial)
    best = None
    starts=[(shift,0,0) for shift in [0,.012,-.012]]
    if contact_faces is not None:
        # An opposed thumb may need to go around the rim. A fit starting on
        # the rear face otherwise sticks behind the penetration barrier.
        starts += [(0,angle,roll) for angle,roll in [(65,25),(-65,-25),(90,-35),(-90,35),(35,70),(-35,-70)]]
    for shift,opposition,roll in starts:
        start = initial.copy()
        start[19] = shift + .00001
        start[15]=opposition;start[16]=roll
        result = least_squares(residual, start, jac=jacobian,
                               bounds=(np.array([-105] * 17 + [-.055, -.035, -.045]),
                                       np.array([105] * 17 + [.055, .035, .045])),
                               max_nfev=180, ftol=1e-9, xtol=1e-8, gtol=1e-8)
        penetration,contact,glass = evaluate(result.x)
        print('GRIP_CANDIDATE', float(penetration.max()), contact.tolist(), flush=True)
        score = max(0, float(penetration.max()) - .001) * 10 + max(0, float(contact.max()) - .003)+max(0,float(glass.max())-.001)*10
        if best is None or score < best[0]:
            best = (score, result.x.copy())
        if penetration.max() < .001 and contact.max() < .003 and glass.max()<.001:
            break
    if contact_faces is not None:
        # The coarse finite-difference search can stop a fraction of a
        # millimetre short of contact. Calibrate the phone plane without
        # moving vertices or exceeding the existing import tolerances.
        valid=[]
        for delta in np.linspace(-.0008,.0008,33):
            trial=best[1].copy();trial[19]+=delta
            penetration,contact,glass=evaluate(trial)
            if penetration.max()<.0012 and contact.max()<.0039 and glass.max()<.0012:
                valid.append((abs(delta),trial))
        if valid:
            best=(0,min(valid,key=lambda candidate:candidate[0])[1])
    evaluate(best[1])
    quaternions = {name: list(rig.pose.bones[name].rotation_quaternion) for name in names}
    penetration,contact,glass = evaluate(best[1])
    return {'fingerQuaternions': quaternions, 'phoneOffset': best[1][17:].tolist(),
            'maxPenetrationMetres': float(penetration.max()), 'fingerContactGapsMetres': contact.tolist(),
            'maxGlassIntrusionMetres':float(glass.max()),
            'beforeMaxPenetrationMetres': float(before[0].max()), 'beforeFingerContactGapsMetres': before[1].tolist(),
            'fittedVertices': len(vertices),'fittedSurfaceSamples':len(masks)}
