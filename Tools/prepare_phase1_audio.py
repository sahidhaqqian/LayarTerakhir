"""Fetch credited CC0 performances and prepare chapter-specific, offline sound design."""
import hashlib
import json
import re
import subprocess
import urllib.parse
import urllib.request
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Audio/Phase1'
SOURCE = ROOT / 'Builds/ArtSources/Phase1Audio'
OUT.mkdir(parents=True, exist_ok=True)
SOURCE.mkdir(parents=True, exist_ok=True)


def request(url):
    req = urllib.request.Request(url, headers={'User-Agent': 'LayarTerakhir-AssetPreparation/1.0'})
    with urllib.request.urlopen(req, timeout=90) as response:
        return response.read()


records = []
assets = [
    ('Lift', 'YCbCr', 'https://opengameart.org/content/generator-loop', 'loop_generator_1.mp3', 'atrim=0:2.5,afade=t=in:d=0.1,afade=t=out:st=2:d=0.5,alimiter=limit=0.7'),
    ('Halogen', 'themightyglider', 'https://opengameart.org/content/electric-buzz', 'buzz_0.ogg', 'atrim=0:1.5,alimiter=limit=0.7'),
    ('Kunti', 'AlucardsBride', 'https://commons.wikimedia.org/wiki/File:Cry_of_woman.ogg', 'Cry_of_woman.ogg', 'highpass=f=110,lowpass=f=6500,acompressor=threshold=0.14:ratio=2,afade=t=in:d=0.3,afade=t=out:st=25.5:d=0.8'),
    ('Manifest', 'Darsycho', 'https://opengameart.org/content/monster-snarls', 'monster-snarl-attack.ogg', 'asetrate=44100*0.82,aresample=32000,highpass=f=45,alimiter=limit=0.8'),
    ('Tuyul', 'AntumDeluge', 'https://opengameart.org/content/evil-laugh', 'laugh-evil-1_0.ogg', 'asetrate=44100*1.55,aresample=32000,highpass=f=160,alimiter=limit=0.8'),
]
for name, author, page, suffix, filters in assets:
    html = request(page).decode()
    candidates = [urllib.parse.urljoin(page, x.replace('&amp;', '&')) for x in re.findall(r'href="([^"]+)"', html)]
    url = next(x for x in candidates if urllib.parse.unquote(urllib.parse.urlsplit(x).path).endswith('/' + suffix))
    local = SOURCE / suffix
    if not local.exists():
        local.write_bytes(request(url))
    subprocess.run(['ffmpeg', '-y', '-v', 'error', '-i', str(local), '-af', filters, '-ar', '32000', '-ac', '1', str(OUT / (name + '.wav'))], check=True)
    records.append({'name': name, 'author': author, 'sourcePage': page, 'downloadUrl': url, 'license': 'CC0-1.0',
                    'sourceSha256': hashlib.sha256(local.read_bytes()).hexdigest(), 'filters': filters})

# Build distinct foley from already credited recordings; no spoken TTS dialogue is generated.
def foley(name, source, filters):
    subprocess.run(['ffmpeg', '-y', '-v', 'error', '-i', str(ROOT / source), '-af', filters, '-ar', '32000', '-ac', '1', str(OUT / (name + '.wav'))], check=True)

foley('Drip', 'Assets/_LayarTerakhir/assets/Audio/Imported/RainOnTin.mp3', 'atrim=12:13.2,highpass=f=700,afade=t=in:d=0.1,afade=t=out:st=0.9:d=0.3,alimiter=limit=0.5')
foley('Tape', 'Assets/Resources/KosAudio/CallMetalScrape.wav', 'atrim=0:0.8,highpass=f=350,afade=t=out:st=0.5:d=0.3,alimiter=limit=0.5')
foley('Foot', 'Assets/_LayarTerakhir/assets/Audio/Prepared/FootstepTile0.wav', 'asetrate=32000*0.60,aresample=32000,bass=g=8:f=85,alimiter=limit=0.8')
foley('Impact', 'Assets/Resources/KosAudio/HardDoorKnock.wav', 'asetrate=32000*0.8,aresample=32000,bass=g=5:f=100,alimiter=limit=0.8')
foley('Glass', 'Assets/Resources/KosAudio/CallGlassCutoff.wav', 'highpass=f=300,afade=t=out:st=0.8:d=0.7,alimiter=limit=0.8')
foley('Chain', 'Assets/Resources/KosAudio/CallMetalScrape.wav', 'highpass=f=150,alimiter=limit=0.75')
foley('Windup', 'Assets/_LayarTerakhir/assets/Audio/Phase1/Manifest.wav', 'atrim=0:1.5,afade=t=in:d=0.1,alimiter=limit=0.8')
foley('Seal', 'Assets/Resources/KosAudio/CallGlassCutoff.wav', 'asetrate=32000*0.7,aresample=32000,aecho=0.7:0.5:160|340:0.25|0.15,alimiter=limit=0.75')

credits = '# Phase 1 audio sources\n\nActual recorded performances, processed offline. No DreadOut files or dialogue TTS used for these chapter sounds.\n\n'
for record in records:
    credits += f"- **{record['name']}** — {record['author']}, [source]({record['sourcePage']}), [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/). Filters: `{record['filters']}`.\n"
credits += '\nFoot/Impact/Glass/Chain/Seal/Drip/Tape derive from the existing, credited project foley bank (see Audit/asset-credits.md). Human dialogue/EVP acting remains a production requirement; subtitle text is preserved rather than presented as final recorded acting.\n'
(ROOT / 'Audit/phase1-audio-credits.md').write_text(credits)
(ROOT / 'Audit/phase1-audio-sources.json').write_text(json.dumps(records, indent=2) + '\n')
print('PHASE1_AUDIO_PREPARED', len(list(OUT.glob('*.wav'))))
