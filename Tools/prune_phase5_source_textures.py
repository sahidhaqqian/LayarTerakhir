"""Keep unused original source maps in the download cache, outside Unity Assets.
No renderer texture is resized, recompressed or removed. Checks material/scene
GUID references before relocating only new Phase5 copies.
"""
import hashlib,json,re,shutil
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1];ART=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5'
def digest(p):return hashlib.file_digest(p.open('rb'),'sha256').hexdigest()
def rewrite(value,mapping):
 if isinstance(value,str):return mapping.get(value,value)
 if isinstance(value,list):return [rewrite(x,mapping) for x in value]
 if isinstance(value,dict):return {k:rewrite(v,mapping) for k,v in value.items()}
 return value
manifest=ROOT/'Audit/phase5-import-manifest.json';doc=json.loads(manifest.read_text());used=set()
for row in doc['materials']:
 for key in ['Albedo','AlbedoAlpha','Normal','UnityMask']:
  if row.get(key):used.add(row[key])
for p in (ART/'Imported/Textures').rglob('*'):
 if p.is_file() and any('/'+s+'/' in str(p) for s in ['brown_mud_03','concrete_wall_007','wooden_rough_planks','rusty_metal_04','fabric_pattern_07']) and any(t in p.name for t in ['_diff_','_nor_gl_','UnityMask.png']):used.add(str(p.relative_to(ROOT)))
refs=set()
for p in [*(ART/'Materials').glob('*.mat'),ROOT/'Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity']:
 refs.update(re.findall(r'guid: ([0-9a-f]{32})',p.read_text()))
source=json.loads((ROOT/'Audit/phase5-asset-sources.json').read_text());originals={f['path']:f.get('sourcePath') for s in source['sources'] for f in s['files']}
mapping={};rows=[]
for p in (ART/'Imported/Textures').rglob('*'):
 old=str(p.relative_to(ROOT))
 if not p.is_file() or p.suffix not in ['.png','.jpg'] or old in used:continue
 meta=p.with_name(p.name+'.meta')
 if meta.exists():
  match=re.search(r'guid: ([0-9a-f]{32})',meta.read_text());assert not match or match[1] not in refs,'Renderer references '+old
 cached=originals.get(old)
 if not cached or not (ROOT/cached).is_file():cached='Builds/ArtSources/Phase5/UnusedSourceTextures/'+str(p.relative_to(ART/'Imported/Textures'))
 target=ROOT/cached;target.parent.mkdir(parents=True,exist_ok=True);sha=digest(p);size=p.stat().st_size
 if target.exists():assert digest(target)==sha;p.unlink()
 else:shutil.move(p,target)
 meta.unlink(missing_ok=True);mapping[old]=cached;rows.append(dict(originalAssetCopy=old,preservedSource=cached,bytes=size,sha256=sha))
for name in ['phase5-asset-sources.json','phase5-import-manifest.json','phase5-texture-conversions.json','phase5-legacy-material-conversions.json']:
 p=ROOT/'Audit'/name;p.write_text(json.dumps(rewrite(json.loads(p.read_text()),mapping),indent=2)+'\n')
report=ROOT/'Audit/phase5-source-copy-compaction.json'
previous=json.loads(report.read_text()).get('files',[]) if report.exists() else []
previous.extend(rows);report.write_text(json.dumps(dict(scope='Unused original source copies only; all referenced renderer maps unchanged',preservedBytes=sum(r['bytes'] for r in previous),files=previous),indent=2)+'\n')
print('P5_UNUSED_SOURCE_COPIES_CACHED',len(rows),sum(r['bytes'] for r in rows))
