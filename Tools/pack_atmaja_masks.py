"""Postprocess selected staging maps to 512/1024px and URP masks; no network or scene edits."""
from pathlib import Path
import json
from PIL import Image,ImageChops
ROOT=Path(__file__).resolve().parents[1]
data=json.loads((ROOT/'Audit/atmaja-art-manifest.json').read_text())
for m in data['materials']:
    arm=m.get('arm','')
    if not arm:continue
    p=ROOT/arm;im=Image.open(p).convert('RGB');im.thumbnail((1024,1024))
    if 'Roughness' in p.name:
        rough=im.getchannel('R');ao=Image.new('L',im.size,255);metal=Image.new('L',im.size,0)
    else:ao,rough,metal=im.split()
    target=ROOT/'Assets/_LayarTerakhir/Art/Atmaja/Textures'/(m['name']+'_mask.png')
    Image.merge('RGBA',(metal,ao,Image.new('L',im.size,0),ImageChops.invert(rough))).save(target)
    m['mask']=str(target.relative_to(ROOT))
(ROOT/'Audit/atmaja-art-manifest.json').write_text(json.dumps(data,indent=2)+'\n')
