"""Author original Indonesian neural voices; no actor/celebrity recordings or cloud credentials.
Build dependency (not shipped): voxcpm 2, torch CPU, ffmpeg. Model: openbmb/VoxCPM2 (Apache-2.0).
Generated reference samples are reused to keep each fictional character's timbre consistent.
"""
from pathlib import Path
import argparse, ctypes, gc, hashlib, json, re, subprocess, tempfile, time
import numpy as np
import soundfile as sf
import torch
from voxcpm import VoxCPM

ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'Assets/_LayarTerakhir/assets/Audio/Dialogue'
SAMPLES=ROOT/'Audit/VoiceSamples'
parser=argparse.ArgumentParser();parser.add_argument('--samples-only',action='store_true');parser.add_argument('--review-only',action='store_true');args=parser.parse_args()
OUT.mkdir(parents=True,exist_ok=True);SAMPLES.mkdir(parents=True,exist_ok=True)
torch.set_num_threads(3)
print('Loading neural voice model on CPU',flush=True)
class WeightOnlyLinear(torch.nn.Module):
    """Per-output-channel W8A32: preserve activations, stop predictor and FSQ precision."""
    def __init__(self,source):
        super().__init__();weight=source.weight.detach().float();scale=weight.abs().amax(dim=1,keepdim=True).clamp_min(1e-8)/127
        self.register_buffer('qweight',(weight/scale).round().clamp(-127,127).to(torch.int8));self.register_buffer('scale',scale)
        self.register_buffer('bias',source.bias.detach().float().clone() if source.bias is not None else None)
    def forward(self,x):return torch.nn.functional.linear(x,self.qweight.float()*self.scale,self.bias)
if 'model' not in globals():
    torch.set_default_dtype(torch.bfloat16)
    model=VoxCPM.from_pretrained('openbmb/VoxCPM2',device='cpu',load_denoiser=False,optimize=False)
    torch.set_default_dtype(torch.float32)
    for lm in (model.tts_model.base_lm,model.tts_model.residual_lm):lm.setup_cache(1,1024,'cpu',torch.float32)
    gc.collect();ctypes.CDLL('libc.so.6').malloc_trim(0)
    names=sorted((m.weight.numel(),n) for n,m in model.tts_model.named_modules() if isinstance(m,torch.nn.Linear) and n.startswith(('base_lm.','residual_lm.','feat_encoder.','feat_decoder.')))
    for count,name in names:
        parent_name,_,key=name.rpartition('.');parent=model.tts_model.get_submodule(parent_name)
        packed=WeightOnlyLinear(getattr(parent,key));setattr(parent,key,packed);del packed
        gc.collect();ctypes.CDLL('libc.so.6').malloc_trim(0)
    model.tts_model.float();model.tts_model.config.dtype='float32'
print('CPU W8A32 authoring model ready',flush=True)
from voice_cpu_optimize import optimize_model
optimize_model(model)
def synthesize(text,seed,**kwargs):
    # Stateful streaming VAE keeps authoring memory bounded even for a long call.
    chunks=list(model.generate_streaming(text=text,seed=seed,cfg_value=2.0,inference_timesteps=10,retry_badcase=False,max_len=650,**kwargs))
    return np.concatenate(chunks)
casting={
 'Raka':('A young adult Indonesian man, warm mid-range voice, conversational and slightly worried',701,'Malam ini gue harus cari Damar. Semoga dia baik-baik aja.',1.0),
 'Damar':('A young adult Indonesian man, deep rough voice, breathless and frightened, urgent hushed speech',1703,'Raka, tolong dengerin gue. Rumah ini nggak kosong.',1.04),
 'Bisikan':('An elderly Indonesian woman, a low raspy whisper, slow and ominous',3907,'Haus... bukakan...',.85),
}
prompts={};sample_records=[]
for speaker,(design,seed,spoken,speed) in casting.items():
    path=SAMPLES/(speaker+'.wav')
    if not path.exists():
        torch.manual_seed(seed)
        if speaker=='Damar • dari lantai atas':emotion='A man screaming in sharp pain, then urgently calling Raka, raw and frightened'
        wav=synthesize('('+design+')'+spoken,seed)
        sf.write(path,wav,model.tts_model.sample_rate)
        assert 1<len(wav)/model.tts_model.sample_rate<16,(speaker,'Unstable design sample; audition/retry instead of shipping',len(wav)/model.tts_model.sample_rate)
    prompts[speaker]=str(path)
    sample_records.append(dict(character=speaker,design=design,seed=seed,transcript=spoken,path=str(path.relative_to(ROOT)),source='Original synthetic voice design; not a real person reference'))
    print('CAST_SAMPLE',speaker,path,flush=True)
if args.samples_only:raise SystemExit(0)
lines=[]
for name in ['Prolog/KamarKosPrologDirector.cs','Prolog/AtmajaDirector.cs']:
    source=(ROOT/'Assets/_LayarTerakhir/src'/name).read_text()
    for speaker,line in re.findall(r'(?:EnqueueSubtitle|PlayPriorityDialogue)\(\s*"(Raka|Bisikan|Damar)"\s*,\s*"((?:[^"\\]|\\.)*)"',source):lines.append((speaker,json.loads('"'+line+'"')))
source=(ROOT/'Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs').read_text()
for key,speaker in [('DamarVoiceNote','Damar'),('DamarScream','Damar'),('ContractReaction','Raka'),('Trapped','Raka')]:
    m=re.search(r'const string '+key+r'="((?:[^"\\]|\\.)*)"',source);lines.append((speaker,json.loads('"'+m[1]+'"')))
source=(ROOT/'Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs').read_text();m=re.search(r'const string LiveIntro="((?:[^"\\]|\\.)*)"',source);lines.append(('Raka',json.loads('"'+m[1]+'"')))
prior_bank=json.loads((ROOT/'Audit/prolog-voice-manifest.json').read_text()) if args.review_only else {}
cast_voice={'Damar • dari lantai atas':'Damar'}
if args.review_only:
    lines=[]
    for filename in ['Prolog/AtmajaReviewMoments.cs','Prolog/MelatiNeighborhood.cs']:
        source=(ROOT/'Assets/_LayarTerakhir/src'/filename).read_text()
        for speaker,text in re.findall(r'(?:EnqueueSubtitle|PlayPriorityDialogue|PlayPriorityWorldDialogue)\(\s*"([^"]+)"\s*,\s*"((?:[^"\\]|\\.)*)"',source):
            if speaker in casting or speaker in cast_voice:lines.append((speaker,json.loads('"'+text+'"')))
    source=(ROOT/'Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs').read_text()
    text=re.search(r'const string DamarVoiceNote="((?:[^"\\]|\\.)*)"',source)[1]
    lines.append(('Damar',json.loads('"'+text+'"')))
    lines.append(('Raka','Sial pintunya dikunci, aku harus cari jalan lain.'))
    old_keys={(e['speaker'],e['text']) for e in prior_bank.get('entries',[])}
    lines=[key for key in lines if key not in old_keys]
progress_path=ROOT/('Audit/review-neural-voice-progress.json' if args.review_only else 'Audit/neural-voice-progress.json')
previous=json.loads(progress_path.read_text()) if progress_path.exists() else {}
previous_entries={(e['speaker'],e['text']):e for e in previous.get('entries',[])} if previous.get('engine')=='VoxCPM2' else {}
records=[]
with tempfile.TemporaryDirectory(prefix='layar-neural-audio-') as temporary:
    for index,(speaker,text) in enumerate(dict.fromkeys(lines)):
        name=hashlib.sha256((speaker+'|'+text).encode()).hexdigest()[:12];output=OUT/(name+'.wav');raw=Path(temporary)/'raw.wav';start=time.monotonic()
        cached=previous_entries.get((speaker,text))
        if cached and output.exists() and hashlib.sha256(output.read_bytes()).hexdigest()==cached['sha256']:
            records.append(cached);print('VO_CACHED',index,speaker,flush=True);continue
        voice=cast_voice.get(speaker,speaker)
        spoken=text.replace('MataBatin.apk','Mata Batin').replace('filter AR','filter A R').replace('HP','hape')
        torch.manual_seed(casting[voice][1]+index)
        emotion='Conversational, quietly worried'
        if voice=='Damar':emotion='Breathless, frightened, urgently warning a close friend in a hushed voice'
        elif voice=='Bisikan':emotion='A slow raspy whisper, ominous'
        elif '?' in text or '!' in text:emotion='Anxious and urgent, natural spoken Indonesian'
        if speaker=='Damar • dari lantai atas':emotion='A man screaming in sharp pain, then urgently calling Raka, raw and frightened'
        wav=synthesize('('+emotion+')'+spoken,casting[voice][1]+index,reference_wav_path=prompts[voice],prompt_wav_path=prompts[voice],prompt_text=casting[voice][2])
        sf.write(raw,wav,model.tts_model.sample_rate)
        filters='highpass=f=70,lowpass=f=8500,loudnorm=I=-18:TP=-3:LRA=9'
        if voice=='Damar':filters='highpass=f=220,lowpass=f=4400,loudnorm=I=-17:TP=-3:LRA=9'
        if voice=='Bisikan':filters='highpass=f=180,lowpass=f=4500,aecho=0.6:0.4:95:0.16,loudnorm=I=-23:TP=-5:LRA=9'
        subprocess.run(['ffmpeg','-loglevel','error','-y','-i',str(raw),'-af',filters,'-ac','1','-ar','24000','-c:a','pcm_s16le',str(output)],check=True)
        audio,rate=sf.read(output);peak=float(np.max(np.abs(audio)));rms=float(np.sqrt(np.mean(audio**2)))
        assert peak<.85 and rms>.005 and len(audio)/rate>.3,(speaker,peak,rms)
        records.append(dict(speaker=speaker,text=text,path=str(output.relative_to(ROOT)),duration=len(audio)/rate,peak=peak,rms=rms,sha256=hashlib.sha256(output.read_bytes()).hexdigest()))
        progress_path.write_text(json.dumps(dict(engine='VoxCPM2',entries=records),ensure_ascii=False,indent=2)+'\n')
        print('VO',index,speaker,round(len(audio)/rate,2),'sec; render',round(time.monotonic()-start,1),'sec',flush=True)
report=dict(engine='VoxCPM2 / openbmb/VoxCPM2, Indonesian neural TTS, CPU offline',source='https://huggingface.co/openbmb/VoxCPM2',engineLicense='Apache-2.0',voiceCloning='Only self-generated fictional character reference samples; no human identity cloned',userApproved=True,casting=sample_records,entries=records)
if args.review_only:
    keys={(e['speaker'],e['text']) for e in records}
    combined=[e for e in prior_bank['entries'] if (e['speaker'],e['text']) not in keys and not (e['speaker']=='Damar' and 'dari forum kemarin' in e['text'])]+records
    local_report=report;report=prior_bank;report['entries']=combined;report['designerReviewVoices']=local_report
(ROOT/'Audit/prolog-voice-manifest.json').write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n')
print('NEURAL_VOICE_BANK_COMPLETE',len(records),flush=True)
