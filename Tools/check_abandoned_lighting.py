"""Capture each abandoned scene in a separate bounded-parent player process.

Use Tools/launch_phase5_player.py --lighting-suite to contain renderer memory.
Posed capture only: does not replace traversal, navigation or puzzle QA.
"""
import json
import subprocess
import sys
from pathlib import Path

ROOT=Path(__file__).resolve().parents[1]
SCENES=['Prolog_RumahAtmaja','Babak1_Rusunawa','Babak2_RSUD','Babak3_PabrikGula','Babak4_KarangSetra','Babak5_PasarSetan']
receipts=[]
for scene in SCENES:
    output=ROOT/'Builds/QA/RakaRevision/LightingFinal'/scene
    subprocess.run([sys.executable,str(ROOT/'Tools/run_phase5_player.py'),'--output',str(output),'--lighting-review','--lighting-scene',scene],cwd=ROOT,check=True)
    receipt=json.loads((output/'lighting-review.json').read_text())
    assert receipt['completed'] and not receipt['errors'],receipt
    assert len(receipt['frames'])==2 and all(f['beamEnabled']==f['phone'] for f in receipt['frames']),receipt
    assert all(f['unpoweredFixtures']>0 for f in receipt['frames']),receipt
    receipts.append(receipt)
    print('LIGHTING_REVIEW_PASS',scene,flush=True)
(ROOT/'Audit/raka-lighting-native-locations.json').write_text(json.dumps(dict(completed=True,scope='Six isolated posed rendering reviews; paused directors; not gameplay/navigation or hardware FPS validation',locations=receipts),indent=2)+'\n')
