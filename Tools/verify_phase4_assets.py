"""Verify local source provenance and distinct placed models; never indexes art in Graphify."""
import hashlib,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
def digest(p):
 with p.open('rb') as f:return hashlib.file_digest(f,'sha256').hexdigest()
sources=json.loads((ROOT/'Audit/phase4-asset-sources.json').read_text());models=json.loads((ROOT/'Audit/phase4-import-manifest.json').read_text());scene=json.loads((ROOT/'Audit/phase4-scene-content.json').read_text());aux=json.loads((ROOT/'Audit/phase4-auxiliary-sources.json').read_text());failures=[];checked=0
for row in sources['sources']:
 for f in row['files']:
  p=ROOT/f['path'];checked+=1
  if not p.exists() or digest(p)!=f['sha256']:failures.append(dict(file=f['path'],problem='source hash mismatch or missing'))
for m in models['models']:
 p=ROOT/m['path'];checked+=1
 if not p.exists() or digest(p)!=m['sha256']:failures.append(dict(file=m['path'],problem='derived model hash mismatch'))
conversions=json.loads((ROOT/'Audit/phase4-texture-conversions.json').read_text())
for row in conversions['files']:
 for path_key,hash_key in [('source','sourceSha256'),('target','derivedSha256')]:
  p=ROOT/row[path_key];checked+=1
  if not p.is_file() or digest(p)!=row[hash_key]:failures.append(dict(file=row[path_key],problem='PBR texture conversion hash mismatch'))
known={m['sourceId'] for m in models['models']};placed={p['sourceId'] for p in scene['placements']}
assert placed<=known,(placed-known)
assert len(placed)>=100,len(placed)
assert len(placed)==scene['distinctNewModels']
for p in scene['placements']:
 assert p['source'].startswith('Assets/_LayarTerakhir/assets/Art/Phase4/Imported/'),p
 assert (ROOT/p['source']).is_file(),p
 assert p['purpose'].strip(),p
for r in aux:
 p=ROOT/r['sourcePath'];assert digest(p)==r['sourceSha256'],r['id'];checked+=1
 for f in ([r] if r.get('path') else r.get('derived',[])):
  p=ROOT/f['path'];assert digest(p)==f['sha256'],f['path'];checked+=1
# Compare source identities, not the contents of older game assets.
old_ids=set();old_uids=set()
for p in (ROOT/'Audit').glob('*asset-sources*.json'):
 if 'phase4' in p.name:continue
 old=json.loads(p.read_text());rows=old.get('sources',[]) if isinstance(old,dict) else old
 if isinstance(rows,list):
  for r in rows:
   if not isinstance(r,dict):continue
   if r.get('id'):old_ids.add(r['id'].casefold())
   if r.get('sourceUid'):old_uids.add(r['sourceUid'])
for r in sources['sources']:
 if r['kind'] not in ['model','character']:continue
 assert r['id'].casefold() not in old_ids,'Old source identity reused '+r['id']
 assert not r.get('sourceUid') or r['sourceUid'] not in old_uids,r['id']
 assert r.get('license') and r.get('sourcePage'),r['id']
report=dict(ok=not failures,distinctDownloadedModels=len(known),distinctPlacedNewModels=len(placed),placements=len(scene['placements']),hashedFiles=checked,failures=failures,scope='Local provenance, declared license metadata and source identity / placement verification. Not an art-quality, navigation or gameplay certification.',sharedCampaignAssetsExcluded=['Golden Kos','Raka','Smartphone','UI framework','shared code'],placedSourceIds=sorted(placed))
(ROOT/'Audit/phase4-asset-verification.json').write_text(json.dumps(report,indent=2)+'\n');print(json.dumps({k:v for k,v in report.items() if k not in ['placedSourceIds']}));assert not failures
