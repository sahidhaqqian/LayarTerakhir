"""Translate downloaded glTF specular/gloss workflow to nonmetallic URP maps.

No painted/generated textures: the original gloss alpha is channel-repacked.
"""
import hashlib,json,struct
from pathlib import Path
from PIL import Image
ROOT=Path(__file__).resolve().parents[1]
p=ROOT/'Audit/phase5-import-manifest.json';doc=json.loads(p.read_text());conversions=[]
for source in json.loads((ROOT/'Audit/phase5-asset-sources.json').read_text())['sources']:
 f=next((f for f in source['files'] if f['role']=='source-model' and f['path'].endswith('.glb')),None)
 if not f:continue
 raw=(ROOT/f['path']).read_bytes();n=struct.unpack_from('<I',raw,12)[0];gltf=json.loads(raw[20:20+n])
 for material in gltf.get('materials',[]):
  legacy=material.get('extensions',{}).get('KHR_materials_pbrSpecularGlossiness')
  if not legacy:continue
  name='P5_'+source['id']+'_'+material.get('name','')
  for row in doc['materials']:
   if row['sourceId']!=source['id'] or row['name']!=name:continue
   row.update(color=legacy.get('diffuseFactor',[1,1,1,1]),roughness=1-legacy.get('glossinessFactor',1),metallic=0)
   image=row.get('MetalRough');row['legacySpecularGlossiness']=True
   if image:
    src=ROOT/image
    with Image.open(src) as im:
     a=im.convert('RGBA').getchannel('A');factor=legacy.get('glossinessFactor',1);a=a.point(lambda v:round(v*factor));zero=Image.new('L',im.size,0);one=Image.new('L',im.size,255)
     target=src.with_name(src.stem+'_LegacyUnityMask.png');Image.merge('RGBA',(zero,one,zero,a)).save(target,compress_level=3)
    row['UnityMask']=str(target.relative_to(ROOT));conversions.append(dict(sourceId=source['id'],source=image,target=row['UnityMask'],operation='R=0 metallic, G=1 AO, A=original specular/gloss texture alpha times gloss factor',sha256=hashlib.file_digest(target.open('rb'),'sha256').hexdigest()))
   else:row.pop('UnityMask',None)
p.write_text(json.dumps(doc,indent=2)+'\n');(ROOT/'Audit/phase5-legacy-material-conversions.json').write_text(json.dumps(conversions,indent=2)+'\n');print('PHASE5_LEGACY_MATERIALS',len(conversions))
