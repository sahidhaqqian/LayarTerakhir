"""Protect the historic opening's gameplay/layout while permitting camera grading.

Keep the narrowly scoped canonicalization in sync with AtmajaSceneValidation.
The historic approved byte hash remains unchanged in atmaja-art-manifest.json.
"""
import hashlib
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def opening_structure_sha256(serialized):
    canonical = re.sub(
        r'^  (?:sharedProfile|m_Antialiasing|m_AntialiasingQuality|m_StopNaN|m_Dithering):[^\n]*\n',
        '', serialized, flags=re.M)
    canonical = re.sub(
        r'^  (?:motorKeyItem: \{fileID: 0\}|togglePerspectiveKey: 118)\n',
        '', canonical, flags=re.M)
    return hashlib.sha256(canonical.encode('utf-8')).hexdigest()


def verify_opening_source(root=ROOT):
    source = root / 'Assets/_LayarTerakhir/assets/Scenes/Prolog_KamarKos.unity'
    data = source.read_bytes()
    actual = hashlib.sha256(data).hexdigest()
    historic = json.loads((root / 'Audit/atmaja-art-manifest.json').read_text())['protectedGoldenSceneSha256']
    if actual == historic:
        return dict(method='Historic approved bytes', sha256=actual)
    baseline = json.loads((root / 'Audit/opening-presentation-baseline.json').read_text())
    if baseline['protectedGoldenSceneSha256'] != historic:
        raise ValueError('Opening structure baseline is disconnected from the historic approved scene')
    structure = opening_structure_sha256(data.decode('utf-8'))
    if structure != baseline['structureSha256']:
        raise ValueError('Opening layout/gameplay changed beyond permitted camera presentation')
    return dict(method='Historic approved structure; camera presentation may change', sha256=actual,
                structureSha256=structure, historicSha256=historic, baseCommit=baseline['baseCommit'])
