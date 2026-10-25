"""Download newly licensed audio/glyph/particle sources and record exact hashes.
No synthesized audio, painted sprites or generated images are produced.
SVG rasterization and sample compression preserve the downloaded art.
"""
import hashlib,html,json,re,subprocess,zipfile
from pathlib import Path
import requests
ROOT=Path(__file__).resolve().parents[1];CACHE=ROOT/'Builds/ArtSources/Phase4/Auxiliary';OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase4/Imported';CACHE.mkdir(parents=True,exist_ok=True)
records=[]
def fetch(id,url,author,lic,page,kind,ext,folder):
 p=CACHE/(id+'_'+hashlib.sha256(url.encode()).hexdigest()[:10]+ext)
 if not p.exists():
  r=requests.get(url,timeout=90);r.raise_for_status();p.write_bytes(r.content)
 record=dict(id=id,kind=kind,sourcePage=page,author=author,license=lic,url=url,sourceSha256=hashlib.file_digest(p.open('rb'),'sha256').hexdigest(),sourcePath=str(p.relative_to(ROOT)))
 target=OUT/folder/(id+ext);target.parent.mkdir(parents=True,exist_ok=True)
 if ext in ['.wav','.flac']:
  target=target.with_suffix('.ogg');subprocess.run(['ffmpeg','-y','-loglevel','error','-i',str(p),'-c:a','libvorbis','-q:a','5',str(target)],check=True)
 elif ext=='.svg':
  target=target.with_suffix('.png');import cairosvg;cairosvg.svg2png(url=str(p),write_to=str(target),output_width=512,output_height=512)
 elif ext not in ['.zip','.7z']:target.write_bytes(p.read_bytes())
 if target.exists():record.update(path=str(target.relative_to(ROOT)),sha256=hashlib.file_digest(target.open('rb'),'sha256').hexdigest())
 records.append(record);return p
fetch('Fire','https://opengameart.org/sites/default/files/fire-1_0.ogg','AntumDeluge','CC0-1.0','https://opengameart.org/content/fire-crackling','audio','.ogg','Audio')
fetch('Heartbeat','https://opengameart.org/sites/default/files/heartbeat_slow_0.wav','bart','CC0-1.0','https://opengameart.org/content/heartbeat-sounds','audio','.wav','Audio')
fetch('FireBurst','https://opengameart.org/sites/default/files/foom_0.wav','Clint Bellanger','CC-BY-3.0','https://opengameart.org/content/spell-4-fire','audio','.wav','Audio')
fetch('BambooForest','https://opengameart.org/sites/default/files/Bamboo%20Forest.flac','Marcelo Fernandez (https://soundcloud.com/marcelofernandezmusic)','CC-BY-3.0','https://opengameart.org/content/bamboo-forest','music','.flac','Audio')
for id,slug in [('Footprint','footprint'),('AuthenticSkull','skull-crack'),('DecoyCross','crossed-bones')]:
 fetch(id,'https://game-icons.net/icons/ffffff/transparent/1x1/lorc/'+slug+'.svg','Lorc','CC-BY-3.0','https://game-icons.net/1x1/lorc/'+slug+'.html','glyph','.svg','Glyphs')
fetch('Knock','https://raw.githubusercontent.com/ibrews/Understudy/main/android/app/src/main/res/raw/knock.wav','ibrews','CC0-1.0','https://github.com/ibrews/Understudy/blob/main/LICENSE-SOUNDS.md','audio','.wav','Audio')
fetch('Wind','https://opengameart.org/sites/default/files/wind1.wav','Luke.RUSTLTD','CC0-1.0','https://opengameart.org/content/wind1','audio','.wav','Audio')
fetch('Market','https://opengameart.org/sites/default/files/crowd_shouting_0.ogg','StarNinjas','CC0-1.0','https://opengameart.org/content/crowd-shoutingspeaking-ambience','audio','.ogg','Audio')
fetch('ClosedEye','https://game-icons.net/icons/ffffff/transparent/1x1/delapouite/eyelashes.svg','Delapouite','CC-BY-3.0','https://game-icons.net/1x1/delapouite/eyelashes.html','glyph','.svg','Glyphs')
for id,url,author,page in [('Ghostly','https://opengameart.org/sites/default/files/independent_nu_ljudbank-atmosphere_moody.7z','Independent.nu / ljudbank','https://opengameart.org/content/4-atmospheric-ghostly-loops'),('Water','https://opengameart.org/sites/default/files/ezwa-water_splash.7z','ezwa / qubodup','https://opengameart.org/content/6-short-water-splashes'),('MagicSmoke','https://opengameart.org/sites/default/files/magic%20and%20smoke.zip','StumpyStrust','https://opengameart.org/content/magic-and-smoke-effect')]:
 ext='.7z' if url.endswith('7z') else '.zip';p=fetch(id,url,author,'CC0-1.0',page,'audio-pack' if id!='MagicSmoke' else 'particle-pack',ext,'Packs');dest=CACHE/id;dest.mkdir(exist_ok=True)

 if ext=='.7z':
  import py7zr
  with py7zr.SevenZipFile(p) as archive:archive.extractall(dest)
 else:
  with zipfile.ZipFile(p) as archive:archive.extractall(dest)
 selected=[]
 for f in dest.rglob('*'):
  if f.suffix.lower() in ['.wav','.ogg','.flac','.mp3']:
   dst=OUT/'Audio'/(id+'_'+re.sub('[^a-zA-Z0-9]','_',f.stem)+'.ogg');dst.parent.mkdir(exist_ok=True)
   subprocess.run(['ffmpeg','-y','-loglevel','error','-i',str(f),'-c:a','libvorbis','-q:a','4',str(dst)],check=True)
   selected.append(dict(path=str(dst.relative_to(ROOT)),original=f.name,sha256=hashlib.file_digest(dst.open('rb'),'sha256').hexdigest()))
  elif f.suffix.lower()=='.png':
   dst=OUT/'Particles'/f.name;dst.parent.mkdir(exist_ok=True);dst.write_bytes(f.read_bytes());selected.append(dict(path=str(dst.relative_to(ROOT)),original=f.name,sha256=hashlib.file_digest(dst.open('rb'),'sha256').hexdigest()))
 records[-1]['derived']=selected
 (ROOT/'Audit/phase4-auxiliary-sources.json').write_text(json.dumps(records,indent=2)+'\n')
print('PHASE4_AUXILIARY',len(records),'sources')
