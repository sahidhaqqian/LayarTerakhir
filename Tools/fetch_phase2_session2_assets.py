"""Additional licensed ritual/set assets; preserves Session 1 provenance."""
import json,zipfile,sys,subprocess,urllib.parse,re
from pathlib import Path
import requests
sys.path.insert(0,str(Path(__file__).resolve().parent))
import fetch_phase2_assets as base
base.objaverse('30f3ad44e42342b7b32a3b2cd49517fd','Furnace')
base.objaverse('018f4f36fb154171912bdacbf0e6415c','Rope')
base.polyhaven('pull_chain_light_socket')
with zipfile.ZipFile(base.SOURCE/'KenneyParticles.zip') as z:
 (base.OUT/'KenneyParticles/Fire.png').write_bytes(z.read('PNG (Transparent)/fire_01.png'))
base.RECORDS.append(dict(id='Kenney particle fire',sourcePage='https://kenney.nl/assets/particle-pack',author='Kenney',license='CC0-1.0',operation='Unmodified fire_01.png; particle configuration, no generated texture'))
src=base.ROOT/'Builds/ArtSources/Phase2Audio';out=base.ROOT/'Assets/_LayarTerakhir/assets/Audio/Phase2'
for name,page,match,author in [('Glass','https://opengameart.org/content/glass-break','glass_breaking.wav','Till Behrend / TinyWorlds'),('Chain','https://opengameart.org/content/chain-winch-sounds','winch.zip','bart')]:
 html=requests.get(page,timeout=45).text
 links=[urllib.parse.urljoin(page,u.replace('&amp;','&')) for u in re.findall('href="([^"]+)"',html)]
 url=next(u for u in links if match in urllib.parse.unquote(u));dest=src/match;digest=base.download(url,dest)
 if dest.suffix=='.zip':
  with zipfile.ZipFile(dest) as z:z.extractall(src/'Winch')
  dest=next(p for p in (src/'Winch').rglob('*') if p.suffix.lower() in ['.wav','.ogg'])
 subprocess.run(['ffmpeg','-y','-v','error','-i',str(dest),'-ar','32000','-ac','1','-af','alimiter=limit=0.65',str(out/(name+'.wav'))],check=True)
 base.RECORDS.append(dict(id=name,author=author,sourcePage=page,url=url,license='CC0-1.0',sourceSha256=digest))
url='https://soundbible.com/grab.php?id=563&type=wav'
dest=src/'CameraShutter.wav';digest=base.download(url,dest)
subprocess.run(['ffmpeg','-y','-v','error','-i',str(dest),'-ar','32000','-ac','1',str(out/'Shutter.wav')],check=True)
base.RECORDS.append(dict(id='Shutter',author='Mike Koenig',sourcePage='https://soundbible.com/563-Camera-Shutter-Click.html',url=url,license='CC-BY-3.0',sourceSha256=digest))
(base.ROOT/'Audit/phase2-session2-asset-sources.json').write_text(json.dumps(base.RECORDS,indent=2)+'\n')
print('SESSION2_ASSETS_FETCHED',len(base.RECORDS))
