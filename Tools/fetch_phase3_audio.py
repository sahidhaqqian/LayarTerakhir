"""Download and convert new CC0 industrial sound cues; no synthesis or AI VO."""
import sys,re,json,zipfile,urllib.parse,subprocess
from pathlib import Path
import requests
sys.path.insert(0,str(Path(__file__).resolve().parent))
import fetch_phase3_assets as base
OUT=base.ROOT/'Assets/_LayarTerakhir/assets/Audio/Phase3';OUT.mkdir(parents=True,exist_ok=True)
records=[]
for page,author,wanted in [
 ('https://opengameart.org/content/factory-ambiance','yd',['Factory.ogg']),
 ('https://opengameart.org/content/mechanical-sounds','Brian MacIntosh / BMacZero',['clank1.wav','rattle1.wav','squeakyclick1.wav','mechanical1.wav','mechanical2.wav']),
 ('https://opengameart.org/content/mechanical-explosion','Spring Spring',['mechanical_explosion.wav']),
 ('https://opengameart.org/content/searching','yd',['Searching.ogg']),
 ('https://opengameart.org/content/ghostly-humming','AuraVoice / Nocturnal_Vanguard',['ghostly_humming.ogg'])]:
 html=requests.get(page,timeout=45).text
 assert 'CC0' in html
 links=[urllib.parse.urljoin(page,u.replace('&amp;','&')) for u in re.findall('href="([^"]+)"',html)]
 for name in wanted:
  url=next(u for u in links if urllib.parse.unquote(u).split('/')[-1]==name)
  raw=base.SOURCE/'Audio'/name;sha=base.get(url,raw)
  dest=OUT/(name.rsplit('.',1)[0]+'.ogg')
  subprocess.run(['ffmpeg','-y','-v','error','-i',str(raw),'-ac','1','-ar','32000','-c:a','libvorbis','-q:a','4',str(dest)],check=True)
  records.append(dict(name=name,author=author,sourcePage=page,url=url,license='CC0-1.0',sourceSha256=sha,file=str(dest.relative_to(base.ROOT))))
for page,author,archive_name,stem in [
 ('https://opengameart.org/content/the-shop','LEGIT Audio','legit_audio_-_the_shop_free_sfx_wav.zip','Shop'),
 ('https://opengameart.org/content/ghost-monster-voice-moaning-growling','qubodup','qubodup-GhostMoans.zip','Ghost')]:
 html=requests.get(page,timeout=45).text;assert 'CC0' in html
 links=[urllib.parse.urljoin(page,u.replace('&amp;','&')) for u in re.findall('href="([^\"]+)"',html)]
 url=next(u for u in links if urllib.parse.unquote(u).split('/')[-1]==archive_name)
 raw=base.SOURCE/'Audio'/archive_name;sha=base.get(url,raw)
 with zipfile.ZipFile(raw) as z:
  for i,name in enumerate(sorted(n for n in z.namelist() if n.lower().endswith(('.wav','.ogg')))):
   src=base.SOURCE/'Audio'/stem/Path(name).name;src.parent.mkdir(parents=True,exist_ok=True);src.write_bytes(z.read(name))
   dest=OUT/(stem+'_'+str(i)+'.ogg')
   subprocess.run(['ffmpeg','-y','-v','error','-i',str(src),'-ac','1','-ar','32000','-c:a','libvorbis','-q:a','4',str(dest)],check=True)
   records.append(dict(name=name,author=author,sourcePage=page,url=url,license='CC0-1.0',sourceSha256=sha,file=str(dest.relative_to(base.ROOT))))
# A new smoke sprite from the author's archive; no previous chapter sprite is copied.
url='https://kenney.nl/media/pages/assets/particle-pack/f8fe0f8cb8-1677578741/kenney_particle-pack.zip'
z=base.SOURCE/'KenneyParticles.zip';sha=base.get(url,z)
p=base.OUT/'Particles';p.mkdir(parents=True,exist_ok=True)
with zipfile.ZipFile(z) as archive:
 for name,out in [('smoke_04.png','Steam.png'),('spark_05.png','Spark.png')]:
  (p/out).write_bytes(archive.read('PNG (Transparent)/'+name))
 (p/'License.txt').write_bytes(archive.read('License.txt'))
records.append(dict(name='Kenney particle pack — smoke04/spark05',sourcePage='https://kenney.nl/assets/particle-pack',license='CC0-1.0',sourceSha256=sha))
(base.ROOT/'Audit/phase3-audio-sources.json').write_text(json.dumps(records,indent=2)+'\n')
print('PHASE3_NEW_AUDIO',len(records),flush=True)
