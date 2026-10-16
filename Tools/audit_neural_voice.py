"""Independent local ASR intelligibility check; not a substitute for listening/acting review."""
from pathlib import Path
import argparse,json
import soundfile as sf
import numpy as np
from scipy.signal import resample_poly
from faster_whisper import WhisperModel
ROOT=Path(__file__).resolve().parents[1]
p=argparse.ArgumentParser();p.add_argument('--samples',action='store_true');p.add_argument('--manifest',default='Audit/prolog-voice-manifest.json');args=p.parse_args()
model=WhisperModel('base',device='cpu',compute_type='int8',cpu_threads=1)
paths=list((ROOT/'Audit/VoiceSamples').glob('*.wav')) if args.samples else [ROOT/e['path'] for e in json.loads((ROOT/args.manifest).read_text())['entries']]
records=[]
for path in paths:
    wav,rate=sf.read(path,dtype='float32');wav=resample_poly(wav,16000,rate)
    segments,info=model.transcribe(wav,language='id',beam_size=5,vad_filter=True)
    text=' '.join(s.text.strip() for s in segments)
    records.append(dict(path=str(path.relative_to(ROOT)),transcript=text,duration=info.duration))
    print(path.name,text,flush=True)
(ROOT/('Audit/voice-sample-asr.json' if args.samples else 'Audit/neural-voice-asr.json')).write_text(json.dumps(dict(method='faster-whisper 1.2.1 / Whisper base INT8 CPU, language=id; diagnostic, not human acceptance',entries=records),ensure_ascii=False,indent=2)+'\n')
