"""Repack licensed PBR channels for Unity; no texture painting or new art."""
import argparse,json,hashlib
from pathlib import Path
from PIL import Image,ImageChops
ROOT=Path(__file__).resolve().parents[1]
parser=argparse.ArgumentParser(description=__doc__);parser.add_argument('--only',action='append',default=[]);selected=set(parser.parse_args().only)
p=ROOT/'Audit/phase4-import-manifest.json';doc=json.loads(p.read_text());rows=[]
previous=ROOT/'Audit/phase4-texture-conversions.json'
if selected and previous.exists():rows=[r for r in json.loads(previous.read_text())['files'] if not any('/Textures/'+s+'/' in r['source'] for s in selected)]
for m in doc['materials']:
 if selected and m['sourceId'] not in selected:continue
 alpha=m.get('Alpha');albedo=m.get('Albedo')
 if alpha and albedo and any(k in m['name'].lower() for k in ['leav','leaf','grass','shrub','foliage']):
  with Image.open(ROOT/albedo) as im,Image.open(ROOT/alpha) as mask:
   result=im.convert('RGBA');result.putalpha(mask.convert('L').resize(im.size,Image.Resampling.LANCZOS));target=(ROOT/albedo).with_name(Path(albedo).stem+'_WithAlpha.png');result.save(target,compress_level=3)
  m['AlbedoAlpha']=str(target.relative_to(ROOT));rows.append(dict(source=albedo,sourceAlpha=alpha,target=m['AlbedoAlpha'],sourceSha256=hashlib.file_digest((ROOT/albedo).open('rb'),'sha256').hexdigest(),derivedSha256=hashlib.file_digest(target.open('rb'),'sha256').hexdigest()))
 arm=m.get('Arm') or m.get('MetalRough')
 if not arm:continue
 src=ROOT/arm
 if not src.exists():raise FileNotFoundError(src)
 with Image.open(src) as image:
  red,green,blue=image.convert('RGB').split()
  if 'Arm' not in m:
   # glTF multiplies texture channels by material factors. URP ignores its
   # scalar metallic when a map is enabled, so bake those source factors.
   metallic=max(0,min(1,m.get('metallic',1)));roughness=max(0,min(1,m.get('roughness',1)))
   blue=blue.point(lambda v:round(v*metallic));green=green.point(lambda v:round(v*roughness))
  mask=Image.merge('RGBA',(blue,red if 'Arm' in m else Image.new('L',image.size,255),Image.new('L',image.size,0),ImageChops.invert(green)))
  target=src.with_name(src.stem+'_UnityMask.png');mask.save(target,compress_level=3)
 m['UnityMask']=str(target.relative_to(ROOT));rows.append(dict(source=arm,target=m['UnityMask'],sourceSha256=hashlib.file_digest(src.open('rb'),'sha256').hexdigest(),derivedSha256=hashlib.file_digest(target.open('rb'),'sha256').hexdigest()))
# Surface sources do not appear in the mesh material table.
sources=json.loads((ROOT/'Audit/phase4-asset-sources.json').read_text())
for s in sources['sources']:
 if s['kind']!='surface':continue
 if selected and s['id'] not in selected:continue
 fs={f.get('channel'):f for f in s['files'] if f['role']=='texture'}
 # Arm maps available under the channel name declared by Poly Haven.
 for f in s['files']:
  if f.get('channel','').lower()!='arm':continue
  src=ROOT/f['path'];im=Image.open(src).convert('RGB');r,g,b=im.split();t=src.with_name(src.stem+'_UnityMask.png');Image.merge('RGBA',(b,r,Image.new('L',im.size,0),ImageChops.invert(g))).save(t,compress_level=3)
  rows.append(dict(source=f['path'],target=str(t.relative_to(ROOT)),sourceSha256=hashlib.file_digest(src.open('rb'),'sha256').hexdigest(),derivedSha256=hashlib.file_digest(t.open('rb'),'sha256').hexdigest()))
p.write_text(json.dumps(doc,indent=2)+'\n');(ROOT/'Audit/phase4-texture-conversions.json').write_text(json.dumps(dict(operation='R=metallic B; G=AO R (or 1 for glTF MR); A=1-roughness G. glTF MR channels include original material metallic/roughness factors. Original UVs and source maps preserved.',files=rows),indent=2)+'\n');print('PHASE4_MASKS',len(rows))
