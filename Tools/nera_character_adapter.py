"""Use the user's Nera hand fitter without modifying its protected converter.

MakeHuman's authored body weights remain in place. Hands are measured in a
temporary T-pose frame, then Nera's exclusive surface regions are mapped back
to the game's skeleton. No detector, service, face stage or runtime dependency.
"""
from pathlib import Path
import hashlib
import json
import sys
import numpy as np
from mathutils import Vector

NERA = Path('/root/Projects/convertervrm/Nera-VRM-AutoRig-v0.10-face-phase1')


def adapt(vertices, faces, defs, weights, report):
    assert NERA.is_dir(), 'The designer-provided convertervrm checkout is required for authoring'
    sys.path.insert(0, str(NERA))
    from finger_geometry import fit_points, weights_for_hand, DIGITS
    def gltf(v): return np.array([v[0], v[2], -v[1]])
    def blender(v): return Vector((float(v[0]), float(-v[2]), float(v[1])))
    points = np.array([gltf(v) for v in vertices])
    posed = points.copy(); body = {}; rotations = {}
    for side, suffix, sign in [('left', 'L', 1), ('right', 'R', -1)]:
        wrist = gltf(defs['Hand_' + suffix][0]); elbow = gltf(defs['Forearm_' + suffix][0])
        rotation = Vector(wrist - elbow).rotation_difference(Vector((sign, 0, 0))).to_matrix()
        matrix = np.array(rotation); mask = points[:, 0] * sign > .35
        posed[mask] = (points[mask] - wrist) @ matrix.T + wrist
        body[side + 'Hand'] = wrist; body[side + 'LowerArm'] = matrix @ (elbow - wrist) + wrist
        rotations[side] = (matrix, wrist)
    triangles = np.array([(f[0][0], f[i][0], f[i + 1][0]) for f in faces for i in range(1, len(f) - 1)])
    # Dense surface samples help section fitting; original exported topology is preserved.
    samples = np.concatenate([posed, posed[triangles].mean(1),
                              (posed[triangles[:, 0]] + posed[triangles[:, 1]]) * .5,
                              (posed[triangles[:, 1]] + posed[triangles[:, 2]]) * .5])
    fitted, diagnostics = fit_points(samples, body)
    mapping = {}
    for side, suffix in [('left', 'L'), ('right', 'R')]:
        matrix, wrist = rotations[side]; info = diagnostics['sides'][side]
        owner, amount = weights_for_hand(posed, side, info, triangles)
        for digit_index, digit in enumerate(DIGITS):
            names = info['digits'][digit]['names']
            for segment, name in enumerate(names):
                game_name = f'Finger{digit_index + 1}_{segment + 1}_{suffix}';mapping[name] = game_name
                head = blender(matrix.T @ (fitted[name] - wrist) + wrist)
                next_position = fitted[names[segment + 1]] if segment < 2 else np.array(info['digits'][digit]['tip'])
                tail = blender(matrix.T @ (next_position - wrist) + wrist)
                assert .004 < (tail - head).length < .09, (name, 'Invalid fitted phalanx')
                if digit=='Thumb':continue
                defs[game_name] = (head, tail, 'Hand_' + suffix if segment == 0 else f'Finger{digit_index + 1}_{segment}_{suffix}')
        changed = 0
        for index in np.flatnonzero(owner >= 0):
            d = int(owner[index]); row = weights[index]
            if d==0 or sum(value for name,value in row.items() if name.startswith('Finger1_'))>.05:continue
            hand_weight = sum(value for name, value in row.items() if name == 'Hand_' + suffix or name.startswith('Finger') and name.endswith('_' + suffix))
            if hand_weight < .2: continue
            for name in list(row):
                if name == 'Hand_' + suffix or name.startswith('Finger') and name.endswith('_' + suffix):del row[name]
            for segment, value in enumerate(amount[index]):
                if value > 1e-5:row[f'Finger{d + 1}_{segment + 1}_{suffix}'] = float(value * hand_weight)
            remainder = max(0, 1 - float(amount[index].sum())) * hand_weight
            if remainder:row['Hand_' + suffix] = remainder
            changed += 1
        info['gameWeightedVertices'] = changed
        info['gamePalmNormalBlender'] = list(blender(matrix.T @ np.array([0.,-1.,0.])))
        assert changed > 40, (side, 'No effective Nera hand adaptation')
    diagnostics['sourceModules'] = {name: hashlib.sha256((NERA / name).read_bytes()).hexdigest()
        for name in ['finger_geometry.py', 'finger_fit_v081.py']}
    diagnostics['checkout'] = str(NERA)
    diagnostics['thumbPolicy']='Preserve authored MakeHuman thumb joints and weights; Nera geometry measurement retained for diagnosis, long digits use Nera surface regions'
    diagnostics['gameBoneMap'] = mapping
    Path(report).write_text(json.dumps(diagnostics, indent=2) + '\n')
    print('NERA_HANDS_ADAPTED', {s: d['gameWeightedVertices'] for s, d in diagnostics['sides'].items()}, flush=True)
    return diagnostics
