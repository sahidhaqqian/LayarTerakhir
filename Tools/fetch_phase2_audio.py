"""Download new CC0 recorded performances/ambience, no synthesized dialogue."""
import hashlib,json,re,subprocess,urllib.parse,zipfile
from pathlib import Path
import requests
ROOT=Path(__file__).resolve().parents[1]
SRC=ROOT/'Builds/ArtSources/Phase2Audio';OUT=ROOT/'Assets/_LayarTerakhir/assets/Audio/Phase2'
SRC.mkdir(parents=True,exist_ok=True);OUT.mkdir(parents=True,exist_ok=True)
records=[]
def fetch(page,match,name,author):
 html=requests.get(page,timeout=60).text
 links=[urllib.parse.urljoin(page,u.replace('&amp;','&')) for u in re.findall('href="([^"]+)"',html)]
 url=next(u for u in links if match in urllib.parse.unquote(u))
 path=SRC/name
 if not path.exists():
  r=requests.get(url,timeout=120);r.raise_for_status();path.write_bytes(r.content)
 records.append(dict(name=name,sourcePage=page,url=url,license='CC0-1.0',author=author,sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
 if path.suffix=='.zip':
  with zipfile.ZipFile(path) as z:z.extractall(SRC/path.stem)
 return path

def convert(source,name,filters):
 subprocess.run(['ffmpeg','-y','-v','error','-i',str(source),'-af',filters,'-ar','32000','-ac','1',str(OUT/(name+'.wav'))],check=True)

fetch('https://opengameart.org/content/ghost-monster-voice-moaning-growling','qubodup-GhostMoans.zip','Moans.zip','qubodup')
fetch('https://opengameart.org/content/horror-breathing','horrorbreathing.zip','Breathing.zip','primbal')
fetch('https://opengameart.org/content/the-shop','the_shop_free_sfx_wav.zip','Mechanical.zip','LEGIT Audio')
moans=sorted(p for p in (SRC/'Moans').rglob('*') if p.suffix.lower() in ['.ogg','.wav'])
breath=next(p for p in (SRC/'Breathing').rglob('*') if p.suffix.lower()=='.wav')
mechanical=sorted(p for p in (SRC/'Mechanical').rglob('*') if p.suffix.lower()=='.wav')
print('MECHANICAL',[(p.name,str(p.relative_to(SRC))) for p in mechanical],flush=True)
print('MOANS',[p.name for p in moans],flush=True)
convert(moans[0],'Moan','atrim=0:3,afade=t=in:d=0.15,afade=t=out:st=2.4:d=0.6,alimiter=limit=0.65')
convert(breath,'Breath','atrim=0:6,highpass=f=90,alimiter=limit=0.6')
# Clip choices are semantic author names, validated in the import report.
room=next((p for p in mechanical if any(t in p.name.lower() for t in ['fridge','refrig','freezer'])),mechanical[0])
convert(room,'Refrigerator','atrim=0:25,highpass=f=35,alimiter=limit=0.45')
wind=fetch('https://opengameart.org/content/rain-on-window-loop','rain_on_window_loop.wav','Rain.wav','alxl')
convert(wind,'Exterior','atrim=0:25,alimiter=limit=0.45')
baby=fetch('https://opengameart.org/content/baby-sound-effect','Baby 01_0.ogg','Baby.ogg','TokyoGeisha')
convert(baby,'Baby','highpass=f=180,lowpass=f=3800,alimiter=limit=0.4')
convert(moans[-1],'Echo','atrim=0:4,highpass=f=150,lowpass=f=3200,alimiter=limit=0.5')
convert(breath,'Hiss','atrim=0:1.5,highpass=f=2200,alimiter=limit=0.35')
(ROOT/'Audit/phase2-audio-sources.json').write_text(json.dumps(dict(sources=records,roomSelected=room.name,exteriorSelected=wind.name,
 note='Nonverbal source performances and mechanical ambience only. Canon dialogue/EVP/Damar use subtitles pending human VO; no AI dialogue generated.'),indent=2)+'\n')
