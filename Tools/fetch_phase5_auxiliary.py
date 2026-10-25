"""New licensed chapter audio and symbols; no synthesis, drawn or AI art."""
import hashlib,html,json,re,shutil,subprocess,zipfile
from pathlib import Path
import requests
ROOT=Path(__file__).resolve().parents[1];CACHE=ROOT/'Builds/ArtSources/Phase5/Auxiliary';OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5/Imported';CACHE.mkdir(parents=True,exist_ok=True)
records=[]
def sha(p):return hashlib.file_digest(p.open('rb'),'sha256').hexdigest()
def download(url,name):
 p=CACHE/name
 if not p.exists():
  r=requests.get(url,timeout=90);r.raise_for_status();p.write_bytes(r.content)
 return p

def source(id,url,author,license,page,extension,folder):
 p=download(url,id+extension);target=OUT/folder/(id+extension);target.parent.mkdir(parents=True,exist_ok=True)
 if extension in ['.wav','.mp3','.flac']:
  target=target.with_suffix('.ogg');subprocess.run(['ffmpeg','-y','-loglevel','error','-i',str(p),'-c:a','libvorbis','-q:a','4',str(target)],check=True)
 elif extension=='.svg':
  import cairosvg
  target=target.with_suffix('.png');cairosvg.svg2png(url=str(p),write_to=str(target),output_width=512,output_height=512)
 elif extension!='.zip':shutil.copy2(p,target)
 row=dict(id=id,author=author,license=license,sourcePage=page,url=url,sourcePath=str(p.relative_to(ROOT)),sourceSha256=sha(p))
 if target.exists():row.update(path=str(target.relative_to(ROOT)),sha256=sha(target))
 records.append(row);return p

def oga_file(slug,ending):
 url='https://opengameart.org/content/'+slug;r=requests.get(url,timeout=40);r.raise_for_status();(CACHE/(slug+'.html')).write_text(r.text)
 links=[html.unescape(u) for u in re.findall(r'href=[\"\']([^\"\']+)[\"\']',r.text)]
 return next(u for u in links if '/sites/default/files/' in u and u.lower().endswith(ending))

source('Ambient','https://opengameart.org/sites/default/files/ambient_horror_0.ogg','techiew','CC0-1.0','https://opengameart.org/content/ambient-horror','.ogg','Audio')
source('Score','https://opengameart.org/sites/default/files/evil_0.ogg','congusbongus','CC0-1.0','https://opengameart.org/content/a-lurking-evil-horror-ambience','.ogg','Audio')
source('Generator','https://opengameart.org/sites/default/files/loop_generator_1.mp3','YCbCr','CC0-1.0','https://opengameart.org/content/generator-loop','.mp3','Audio')
source('Cold','https://opengameart.org/sites/default/files/continuousspark.wav','Brian MacIntosh / BMacZero','CC0-1.0','https://opengameart.org/content/electricity-sound-effects-0','.wav','Audio')
source('Spark','https://opengameart.org/sites/default/files/spark.wav','Brian MacIntosh / BMacZero','CC0-1.0','https://opengameart.org/content/electricity-sound-effects-0','.wav','Audio')
rpg=source('RpgSounds',oga_file('50-rpg-sound-effects','.zip'),'Kenney','CC0-1.0','https://opengameart.org/content/50-rpg-sound-effects','.zip','Packs')
with zipfile.ZipFile(rpg) as z:z.extractall(CACHE/'RpgSounds')
print('RPG_FILES',[str(f.relative_to(CACHE/'RpgSounds')) for f in (CACHE/'RpgSounds').rglob('*.ogg')],flush=True)
for id,slug in [('Danger','chewed-skull'),('OpenEye','eyeball'),('ClosedEye','semi-closed-eye'),('AuthenticThread','linked-rings'),('FalseThread','chained-arrow-heads')]:
 try:source(id,'https://game-icons.net/icons/ffffff/transparent/1x1/lorc/'+slug+'.svg','Lorc','CC-BY-3.0','https://game-icons.net/1x1/lorc/'+slug+'.html','.svg','Glyphs')
 except requests.RequestException as e:print('GLYPH_FAILED',id,str(e),flush=True)
# Public HQ preview explicitly served by Freesound's player; the original
# WAV download requires login and is not requested. The recording is CC0.
source('GamelanMarket','https://cdn.freesound.org/previews/234/234519_3889502-hq.mp3','shinobidragon04','CC0-1.0','https://freesound.org/people/shinobidragon04/sounds/234519/','.mp3','Audio')
source('Rain',oga_file('light-rain-and-heavy-rain','.mp3'),'haruta','CC0-1.0','https://opengameart.org/content/light-rain-and-heavy-rain','.mp3','Audio')
source('FireSheet',oga_file('animated-particle-effects-2','fire_02.png'),'para','CC0-1.0','https://opengameart.org/content/animated-particle-effects-2','.png','VFX')
source('Water',oga_file('water-splash-and-sand-footsteps','splash1_0.wav'),'Peludo','CC0-1.0','https://opengameart.org/content/water-splash-and-sand-footsteps','.wav','Audio')
# Contextual cue bindings retain the exact licensed source sample; copied
# filenames are not counted as different assets.
cue_files={'Knock':'bookClose','HotDoor':'metalPot3','Mirror':'creak2','Cable':'knifeSlice2','Incense':'cloth3','Lunge':'drawKnife3','Reveal':'doorClose_4','FireWarning':'metalLatch','Shield':'metalPot2','Core':'chop','Miss':'creak1','Light':'metalClick'}
for cue,filename in cue_files.items():
 src=next((CACHE/'RpgSounds').rglob(filename+'.ogg'));target=OUT/'Audio'/(cue+'.ogg');shutil.copy2(src,target)
 records.append(dict(id=cue,kind='cue-binding',author='Kenney',license='CC0-1.0',sourcePage='https://opengameart.org/content/50-rpg-sound-effects',sourcePath=str(src.relative_to(ROOT)),sourceSha256=sha(src),path=str(target.relative_to(ROOT)),sha256=sha(target),operation='Unmodified sample selection from newly downloaded pack; not counted as a distinct source'))
for cue,original in {'Crowd':'Ambient','EndingBad':'Score','EndingNormal':'Rain','EndingTrue':'GamelanMarket','EndingSecret':'GamelanMarket'}.items():
 src=OUT/'Audio'/(original+'.ogg');target=OUT/'Audio'/(cue+'.ogg');shutil.copy2(src,target)
 records.append(dict(id=cue,kind='cue-alias',sourceId=original,path=str(target.relative_to(ROOT)),sha256=sha(target)))
(ROOT/'Audit/phase5-auxiliary-sources.json').write_text(json.dumps(records,indent=2)+'\n')
print('PHASE5_AUXILIARY',len(records),'sources',flush=True)
