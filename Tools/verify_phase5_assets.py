"""Verify provenance/new identities, derived files and actual scene placements.

Reads manifests and precisely named files; never scans old game art contents.
"""
import hashlib,json,re,subprocess
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
def sha(p):return hashlib.file_digest(p.open('rb'),'sha256').hexdigest()
def main():
 src=json.loads((ROOT/'Audit/phase5-asset-sources.json').read_text());imp=json.loads((ROOT/'Audit/phase5-import-manifest.json').read_text())
 old_ph=set();old_uids=set()
 for path in (ROOT/'Audit').glob('*sources*.json'):
  if path.name.startswith('phase5-'):continue
  text=path.read_text();old_ph.update(x.lower() for x in re.findall(r'https://polyhaven.com/a/([A-Za-z0-9_-]+)',text));old_uids.update(re.findall(r'\b[0-9a-f]{32}\b',text))
 for name in subprocess.check_output(['git','ls-files','Assets/_LayarTerakhir/assets/Art'],cwd=ROOT,text=True).splitlines():
  if '/Phase5/' not in name and name.endswith(('.fbx','.glb','.gltf')):old_ph.add(Path(name).stem.lower().removesuffix('_2k').removesuffix('_1k'))
 errors=[];checks=0;ids=set();licenses=[]
 for source in src['sources']:
  identity=source['id'];
  if identity in ids:errors.append('Duplicate identity '+identity)
  ids.add(identity)
  if source.get('sourceUid') in old_uids:errors.append('Previously used hero source '+identity)
  if source['sourcePage'].startswith('https://polyhaven.com/a/') and identity.lower() in old_ph:errors.append('Previously used PolyHaven source '+identity)
  if source.get('license') not in ['CC0-1.0','CC-BY-4.0']:errors.append('Unexpected model license '+identity)
  for f in source['files']:
   path=ROOT/f['path'];checks+=1
   if not path.exists() or sha(path)!=f['sha256']:errors.append('Source hash mismatch '+str(path))
  licenses.append(source)
 for model in imp['models']:
  p=ROOT/model['path'];checks+=1
  if not p.exists() or sha(p)!=model['sha256']:errors.append('Derived hash mismatch '+str(p))
  if model['sourceId'] not in ids:errors.append('Unlicensed derived model '+model['sourceId'])
 aux_path=ROOT/'Audit/phase5-auxiliary-sources.json';aux=json.loads(aux_path.read_text()) if aux_path.exists() else []
 motion_path=ROOT/'Audit/phase5-animation-sources.json'
 if motion_path.exists():aux+=json.loads(motion_path.read_text())
 for row in aux:
  if row.get('kind')=='animation-library':
   raw=ROOT/row['sourcePath'];checks+=1
   if not raw.exists() or sha(raw)!=row['sourceSha256']:errors.append('Animation archive hash mismatch '+str(raw))
  if row.get('path'):
   p=ROOT/row['path'];checks+=1
   if not p.exists() or sha(p)!=row['sha256']:errors.append('Auxiliary hash mismatch '+str(p))
 placed_path=ROOT/'Audit/phase5-scene-content.json';placed=json.loads(placed_path.read_text()) if placed_path.exists() else None
 placed_ids=set(p['sourceId'] for p in placed['placements']) if placed else set()
 if placed and len(placed_ids)<100:errors.append('Fewer than100 distinct placed source models')
 if not placed_ids.issubset(ids):errors.append('Scene placement has an unknown source')
 report=dict(uniqueNewModelSources=sum(r['kind']=='model' for r in licenses),distinctPlacedModels=len(placed_ids),placements=len(placed['placements']) if placed else 0,
   hashChecks=checks,failures=errors,actualSceneManifestPresent=bool(placed),scope='Identity/byte-hash/source/placement provenance only. Does not prove artistic quality, gameplay or Windows performance.')
 (ROOT/'Audit/phase5-asset-verification.json').write_text(json.dumps(report,indent=2)+'\n')
 def author(s):
  a=s.get('author') or 'Poly Haven contributing artists';return ', '.join(a) if isinstance(a,(dict,list)) else str(a)
 text='# Phase5 — sumber model baru dan kredit\n\nSumber berikut diunduh khusus Pasar Setan. Jumlah final dihitung dari manifest scene dan tes Unity, bukan jumlah file. Konversi FBX, LOD/decimation, pemilihan bagian asli, retarget animasi asli, material/channel repacking dan penempatan dilakukan tanpa memodelkan atau melukis artwork baru. Raka/HP/UI/mixer tetap merupakan identitas/sistem campaign bersama dan tidak dihitung sebagai aset baru.\n\nPowered by Poly Haven. Semua sumber Poly Haven CC0: https://polyhaven.com/license . Kredit CC BY mengikuti sumber ke dalam build.\n\n| ID | Pembuat | Lisensi | Sumber | Terpasang |\n|---|---|---|---|---|\n'
 for s in licenses:
  if s['kind']!='model':continue
  text+='| '+s['id']+' | '+author(s)+' | '+s['license']+' | '+s['sourcePage']+' | '+('Ya' if s['id'] in placed_ids else 'Lihat manifest penempatan setelah generasi scene')+' |\n'
 (ROOT/'Audit/phase5-asset-credits.md').write_text(text)
 text='# Phase5 — audio dan simbol\n\nTidak ada TTS/VO AI baru. Dialog manusia Indonesia belum direkam; subtitle dan daftar rekaman dibedakan dari VO final. Sampel yang dipilih ulang untuk beberapa cue tidak dihitung sebagai sumber berbeda.\n\n'
 for s in aux:
  if s.get('kind') in ['cue-binding','cue-alias']:continue
  text+='- **'+s['id']+'** — '+s.get('author','')+'; '+s.get('license','')+'; '+s.get('sourcePage','')+'\n'
 text+='\nLima rajah menggunakan artwork Game-icons.net, CC BY3.0, pembuat dicantumkan di atas. Gamelan memakai rekaman jalanan Bogor oleh shinobidragon04, CC0; versi HQ yang disediakan pemutar publik Freesound. File WAV yang memerlukan login tidak diakses.\n'
 (ROOT/'Audit/phase5-audio-credits.md').write_text(text)
 print(json.dumps(report,indent=2))
 if errors:raise SystemExit(1)
if __name__=='__main__':main()
