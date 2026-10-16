"""Adopt reviewed clearer candidates, updating clip hashes/durations and provenance together."""
from pathlib import Path
import hashlib,json,math,shutil,struct,wave
ROOT=Path(__file__).resolve().parents[1]
manifest_path=ROOT/'Audit/prolog-voice-manifest.json';manifest=json.loads(manifest_path.read_text())
candidates=json.loads((ROOT/'Audit/voice-candidates.json').read_text())['entries']
by_line={(e['speaker'],e['text']):e for e in candidates}
for entry in manifest['entries']:
    candidate=by_line.get((entry['speaker'],entry['text']))
    if not candidate:continue
    source=ROOT/candidate['path'];target=ROOT/entry['path'];shutil.copyfile(source,target)
    with wave.open(str(target),'rb') as wav:
        rate=wav.getframerate();samples=struct.unpack('<'+'h'*wav.getnframes(),wav.readframes(wav.getnframes()))
    entry.update(duration=len(samples)/rate,peak=max(abs(x) for x in samples)/32768,rms=math.sqrt(sum((s/32768)**2 for s in samples)/len(samples)),sha256=hashlib.sha256(target.read_bytes()).hexdigest(),delivery=candidate['delivery'])
    if entry['speaker']=='Bisikan':entry['renderBackend']='Local CPU VoxCPM2 original voice-design sample; matches exact dialogue; no second synthesis'
manifest_path.write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n')
(ROOT/'Audit/official-voice-progress.json').write_text(json.dumps(dict(engine='VoxCPM2 official public demo plus exact original whisper sample',entries=manifest['entries']),ensure_ascii=False,indent=2)+'\n')
print('SELECTED_VOICE_REPAIRS',len(candidates),'totalSeconds',sum(e['duration'] for e in manifest['entries']))
