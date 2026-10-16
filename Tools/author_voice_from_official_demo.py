"""Render project dialogue through OpenBMB's public VoxCPM2 demo, one request at a time.
Only original fictional dialogue and our original synthetic references are sent. No credentials,
personal audio, saves or model weights are sent. The resulting game remains fully offline.
Requires gradio_client, ffmpeg. Model/source/license links are recorded in the manifest.
"""
from pathlib import Path
import hashlib,json,math,re,struct,subprocess,time,wave
from gradio_client import Client,handle_file

ROOT=Path(__file__).resolve().parents[1];OUT=ROOT/'Assets/_LayarTerakhir/Audio/Dialogue';SAMPLES=ROOT/'Audit/VoiceSamples'
casting={
 'Raka':('Young adult man, warm conversational mid-range','Malam ini gue harus cari Damar. Semoga dia baik-baik aja.'),
 'Damar':('Young adult man, low rough and urgent hushed voice','Raka, tolong dengerin gue. Rumah ini nggak kosong.'),
 'Bisikan':('Elderly woman, low raspy whisper','Haus... bukakan...'),
}
lines=[]
for name in ['Prolog/KamarKosPrologDirector.cs','Prolog/AtmajaDirector.cs']:
    source=(ROOT/'Assets/_LayarTerakhir/Scripts'/name).read_text()
    for speaker,line in re.findall(r'(?:EnqueueSubtitle|PlayPriorityDialogue)\(\s*"(Raka|Bisikan|Damar)"\s*,\s*"((?:[^"\\]|\\.)*)"',source):lines.append((speaker,json.loads('"'+line+'"')))
source=(ROOT/'Assets/_LayarTerakhir/Scripts/UI/PhoneDesktop.cs').read_text()
for key,speaker in [('DamarVoiceNote','Damar'),('Trapped','Raka')]:
    m=re.search(r'const string '+key+r'="((?:[^"\\]|\\.)*)"',source);lines.append((speaker,json.loads('"'+m[1]+'"')))
source=(ROOT/'Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs').read_text();m=re.search(r'const string LiveIntro="((?:[^"\\]|\\.)*)"',source);lines.append(('Raka',json.loads('"'+m[1]+'"')))
progress=ROOT/'Audit/official-voice-progress.json';previous=json.loads(progress.read_text()).get('entries',[]) if progress.exists() else []
cached={(e['speaker'],e['text']):e for e in previous};records=[]
client=Client('openbmb/VoxCPM-Demo')
for index,(speaker,text) in enumerate(dict.fromkeys(lines)):
    name=hashlib.sha256((speaker+'|'+text).encode()).hexdigest()[:12];output=OUT/(name+'.wav');prior=cached.get((speaker,text));start=time.monotonic()
    if prior and output.exists() and hashlib.sha256(output.read_bytes()).hexdigest()==prior['sha256']:
        records.append(prior);print('VOICE_CACHED',index,speaker,flush=True);continue
    spoken=text.replace('MataBatin.apk','Mata Batin').replace('filter AR','filter A R').replace('HP','hape')
    delivery='Natural conversational Indonesian, quietly worried'
    if speaker=='Damar':delivery='Breathless and frightened, urgently warning a close friend, hushed voice'
    elif speaker=='Bisikan':delivery='Slow, ominous, raspy whisper'
    elif '?' in text or '!' in text:delivery='Anxious and urgent, natural spoken Indonesian'
    source_file=client.predict(text_input=spoken,control_instruction=delivery,reference_wav_path_input=handle_file(str(SAMPLES/(speaker+'.wav'))),use_prompt_text=True,prompt_text_input=casting[speaker][1],cfg_value_input=2.0,do_normalize=False,denoise=False,api_name='/generate')
    filters='highpass=f=70,lowpass=f=8500,loudnorm=I=-18:TP=-3:LRA=9'
    if speaker=='Damar':filters='highpass=f=220,lowpass=f=4400,loudnorm=I=-17:TP=-3:LRA=9'
    if speaker=='Bisikan':filters='highpass=f=180,lowpass=f=4500,aecho=0.6:0.4:95:0.16,loudnorm=I=-23:TP=-5:LRA=9'
    subprocess.run(['ffmpeg','-loglevel','error','-y','-i',str(source_file),'-af',filters,'-ac','1','-ar','24000','-c:a','pcm_s16le',str(output)],check=True)
    with wave.open(str(output),'rb') as w:
        rate=w.getframerate();samples=struct.unpack('<'+'h'*w.getnframes(),w.readframes(w.getnframes()))
    duration=len(samples)/rate;peak=max(abs(x) for x in samples)/32768;rms=math.sqrt(sum((s/32768)**2 for s in samples)/len(samples))
    assert .3<duration<65 and .005<rms and peak<.85,(speaker,duration,peak,rms)
    records.append(dict(speaker=speaker,text=text,path=str(output.relative_to(ROOT)),duration=duration,peak=peak,rms=rms,sha256=hashlib.sha256(output.read_bytes()).hexdigest(),delivery=delivery,renderBackend='Official OpenBMB VoxCPM2 demo (backend checkpoint revision not exposed)'))
    progress.write_text(json.dumps(dict(engine='VoxCPM2 official public demo',entries=records),ensure_ascii=False,indent=2)+'\n')
    print('VOICE_RENDERED',index,speaker,round(duration,2),'seconds; request',round(time.monotonic()-start,1),'seconds',flush=True)
report=dict(engine='VoxCPM2 / official OpenBMB public demo, rendered Indonesian neural dialogue',source='https://huggingface.co/openbmb/VoxCPM2',renderService='https://huggingface.co/spaces/openbmb/VoxCPM-Demo',engineLicense='Apache-2.0',syntheticSpeechAllowedByUser=True,subjectiveActingAcceptance='Pending user listening on target hardware',gameRuntimeNetworkRequired=False,voiceCloning='Only original synthetic fictional-character samples; no real human identity cloned',casting=[dict(character=s,design=d[0],transcript=d[1],path=str((SAMPLES/(s+'.wav')).relative_to(ROOT)),source='Original synthetic VoxCPM2 voice design, CPU W8A32, model revision 32279effe8c19989596f05d353d1447f51d9e915') for s,d in casting.items()],entries=records)
(ROOT/'Audit/prolog-voice-manifest.json').write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n')
print('VOICE_BANK_COMPLETE',len(records),'lines',round(sum(e['duration'] for e in records),2),'seconds',flush=True)
