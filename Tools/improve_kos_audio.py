"""Original offline foley + directed neural retakes for the revised Kos opening.

Only fictional dialogue and our original synthetic reference voices go to OpenBMB's public
supported demo API. No personal recordings/accounts/saves. Apache-2.0 source recorded in credits.
Reference-only controllable cloning deliberately avoids neutral prompt-audio continuation.
"""
from pathlib import Path
import array, hashlib, json, math, random, re, subprocess, wave

ROOT = Path(__file__).resolve().parents[1]
RATE = 24000

def write_pcm(path, values):
    path.parent.mkdir(parents=True, exist_ok=True)
    data = array.array('h', (round(max(-.89, min(.89, v)) * 32767) for v in values))
    with wave.open(str(path), 'wb') as f:
        f.setparams((1, 2, RATE, 0, 'NONE', 'not compressed')); f.writeframes(data.tobytes())

def foley():
    out = ROOT / 'Assets/Resources/KosAudio'
    rng = random.Random(14808)
    def knock(t):
        attack = min(1, t/.0015)
        body = .48*math.sin(2*math.pi*118*t)*math.exp(-t*15)
        panel = .24*math.sin(2*math.pi*237*t)*math.exp(-t*24)
        return attack*(body+panel+.35*rng.uniform(-1,1)*math.exp(-t*75))
    def ring(t):
        phase = t % 3.6
        if phase > 1.35: return 0
        pulse = phase % .34
        env = min(1, pulse/.012)*math.exp(-pulse*9)
        return .18*env*(math.sin(2*math.pi*740*t)+.7*math.sin(2*math.pi*988*t))
    def outgoing(t):
        return .17*(math.sin(2*math.pi*425*t)+.35*math.sin(2*math.pi*450*t)) if t%3.2<.9 else 0
    def scrape(t):
        env = math.sin(math.pi*t/1.2)**2
        return env*(.12*rng.uniform(-1,1)+.13*math.sin(2*math.pi*(680*t+15*math.sin(t*25))))
    def glass(t):
        return (.3*rng.uniform(-1,1)+.12*math.sin(2*math.pi*2700*t)) * math.exp(-t*7) * min(1,t/.002)
    for name, seconds, fn in [('HardDoorKnock',.48,knock),('IncomingCall',3.6,ring),('OutgoingCall',3.2,outgoing),('CallMetalScrape',1.2,scrape),('CallGlassCutoff',.8,glass)]:
        write_pcm(out/(name+'.wav'), (fn(n/RATE) for n in range(round(seconds*RATE))))
        print('ORIGINAL_FOLEY', name, flush=True)

def render():
    from gradio_client import Client, handle_file
    manifest_path = ROOT/'Audit/prolog-voice-manifest.json'
    manifest = json.loads(manifest_path.read_text())
    desktop = (ROOT/'Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs').read_text()
    def constant(name): return re.search(r'const string '+name+r'="([^"]+)"', desktop)[1]
    damar = constant('DamarVoiceNote')
    direction = {
        ('Damar',damar): 'Frightened young man, trembling breathless delivery, panicked urgent whisper rising to terror. Uneven pace, gasping pauses, increasingly alarmed. Speak Indonesian, not a neutral narrator.',
        ('Damar',constant('DamarScream')): 'A terrified man cries desperately for help, loud raw panicked shouting, ending with an extended scream of terror! Not calmly reading words.',
        ('Raka',constant('ContractReaction')): 'Shocked and frightened young Indonesian man, sharp alarm at first, then worried and reluctantly resolute. Natural emotional conversation with breaths and pauses.',
        ('Raka','Dam?! Halo, Damar?! Sial, nomornya nggak aktif!'): 'Frightened and disbelieving, calling urgently to a friend, raising voice at Halo Damar, frustrated exclamation at the end.',
        ('Raka','Tiga setengah juta besok pagi... Gue cari ke mana jam segini?!'): 'A worried young man under financial pressure, starts quietly stunned, a frustrated anxious question at the end, expressive Indonesian speech.',
        ('Raka','Kebuka. Powerbank, dompet, sama buku harian gue masih di sini.'): 'Quiet relieved natural Indonesian conversation, small pause after Kebuka.',
    }
    progress_path=ROOT/'Audit/kos-directed-retakes.json'
    cache=json.loads(progress_path.read_text()).get('entries',[]) if progress_path.exists() else []
    cached={(x['speaker'],x['text']):x for x in cache}; records=[]
    client=Client('openbmb/VoxCPM-Demo')
    for (speaker,text),delivery in direction.items():
        path=ROOT/'Assets/_LayarTerakhir/assets/Audio/Dialogue'/(hashlib.sha256((speaker+'|'+text).encode()).hexdigest()[:12]+'.wav')
        old=cached.get((speaker,text))
        if old and path.exists() and hashlib.sha256(path.read_bytes()).hexdigest()==old['sha256']:
            records.append(old);continue
        spoken=text.replace('filter AR','filter A R').replace('HP','hape')
        source=client.predict(text_input=spoken,control_instruction=delivery,
            reference_wav_path_input=handle_file(str(ROOT/'Audit/VoiceSamples'/(speaker+'.wav'))),
            use_prompt_text=False,prompt_text_input='',cfg_value_input=2.0,do_normalize=False,denoise=False,api_name='/generate')
        filters='highpass=f=100,lowpass=f=8000,loudnorm=I=-18:TP=-4:LRA=12'
        if speaker=='Damar': filters='highpass=f=220,lowpass=f=4100,loudnorm=I=-18:TP=-4:LRA=12'
        subprocess.run(['ffmpeg','-loglevel','error','-y','-i',str(source),'-af',filters,'-ac','1','-ar',str(RATE),'-c:a','pcm_s16le',str(path)],check=True)
        with wave.open(str(path),'rb') as f:
            duration=f.getnframes()/f.getframerate(); samples=array.array('h',f.readframes(f.getnframes()))
        peak=max(abs(s) for s in samples)/32768; rms=math.sqrt(sum((s/32768)**2 for s in samples)/len(samples))
        assert .3<duration<65 and .005<rms and peak<.86, (speaker,duration,rms,peak)
        entry=dict(speaker=speaker,text=text,path=str(path.relative_to(ROOT)),duration=duration,peak=peak,rms=rms,sha256=hashlib.sha256(path.read_bytes()).hexdigest(),delivery=delivery,renderBackend='Official OpenBMB VoxCPM2 demo; reference-only controllable cloning, not neutral transcript continuation')
        records.append(entry);progress_path.write_text(json.dumps(dict(entries=records),ensure_ascii=False,indent=2)+'\n')
        print('DIRECTED_RETAKE',speaker,round(duration,2),text,flush=True)
    updates={(x['speaker'],x['text']):x for x in records}
    entries=[]
    for e in manifest['entries']:
        if e['text']=='Kebuka. Powerbank-nya masih penuh.': continue
        entries.append(updates.pop((e['speaker'],e['text']),e))
    entries.extend(updates.values());manifest['entries']=entries
    manifest['directedRevision']='Reference-only emotional retakes; separate Damar distress cry; user acting acceptance still pending'
    manifest_path.write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n')
    (ROOT/'Audit/official-voice-progress.json').write_text(json.dumps(dict(engine=manifest['engine'],entries=entries),ensure_ascii=False,indent=2)+'\n')
    print('DIRECTED_BANK_COMPLETE',len(entries),flush=True)

if __name__=='__main__':
    foley();render()
