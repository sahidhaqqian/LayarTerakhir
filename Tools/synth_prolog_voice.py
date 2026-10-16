"""Build disclosed offline Indonesian temporary VO from the actual dialogue strings.
Requires espeak-ng + ffmpeg. No cloud requests, voice cloning, or restricted neural weights.
"""
from pathlib import Path
import re,json,subprocess,tempfile,hashlib,wave,math,struct
ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'Assets/_LayarTerakhir/Audio/Dialogue';OUT.mkdir(parents=True,exist_ok=True)
lines=[]
for path in [ROOT/'Assets/_LayarTerakhir/Scripts/Prolog/KamarKosPrologDirector.cs',ROOT/'Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs']:
    source=path.read_text()
    for speaker,line in re.findall(r'EnqueueSubtitle\(\s*"(Raka|Bisikan|Damar|Bu Haji Ratna)"\s*,\s*"((?:[^"\\]|\\.)*)"',source):
        lines.append((speaker,json.loads('"'+line+'"')))
source=(ROOT/'Assets/_LayarTerakhir/Scripts/UI/PhoneDesktop.cs').read_text()
for key,speaker in [('DamarVoiceNote','Damar'),('RatnaWarning','Bu Haji Ratna'),('Trapped','Raka')]:
    match=re.search(r'const string '+key+r'="((?:[^"\\]|\\.)*)"',source);lines.append((speaker,json.loads('"'+match[1]+'"')))
# The intro constant is used as a variable by the director.
source=(ROOT/'Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs').read_text();m=re.search(r'const string LiveIntro="((?:[^"\\]|\\.)*)"',source);lines.append(('Raka',json.loads('"'+m[1]+'"')))
unique=list(dict.fromkeys(lines));records=[]
with tempfile.TemporaryDirectory(prefix='layar-voice-') as temporary:
    for index,(speaker,text) in enumerate(unique):
        raw=Path(temporary)/f'{index}.wav';name=hashlib.sha256((speaker+'|'+text).encode()).hexdigest()[:12];output=OUT/(name+'.wav')
        voice,pitch,speed=('id',44,184)
        if speaker=='Damar':voice,pitch,speed='id',58,205
        if speaker=='Bu Haji Ratna':voice,pitch,speed='id+f3',60,178
        if speaker=='Bisikan':voice,pitch,speed='id+whisper',46,135
        spoken=text.replace('Rp5.000.000','lima juta rupiah').replace('Rp3.500.000','tiga juta lima ratus ribu rupiah').replace('MataBatin.apk','Mata Batin').replace('filter AR','filter A R')
        subprocess.run(['espeak-ng','-v',voice,'-p',str(pitch),'-s',str(speed),'-a','90','-w',str(raw),'--',spoken],check=True,stdout=subprocess.DEVNULL)
        filters='highpass=f=85,lowpass=f=6800,alimiter=limit=0.72:level=0,afade=t=in:d=0.02'
        if speaker=='Damar':filters='highpass=f=280,lowpass=f=3400,acompressor=threshold=0.16:ratio=2.5,alimiter=limit=0.68:level=0'
        if speaker=='Bisikan':filters='highpass=f=250,lowpass=f=3200,aecho=0.4:0.35:90:0.18,volume=3,alimiter=limit=0.5:level=0'
        subprocess.run(['ffmpeg','-loglevel','error','-y','-i',str(raw),'-af',filters,'-ar','22050','-ac','1','-c:a','pcm_s16le',str(output)],check=True)
        with wave.open(str(output),'rb') as wav:
            samples=struct.unpack('<'+'h'*wav.getnframes(),wav.readframes(wav.getnframes()));seconds=len(samples)/wav.getframerate();peak=max(abs(s) for s in samples)/32768;rms=math.sqrt(sum((s/32768)**2 for s in samples)/len(samples))
        records.append(dict(speaker=speaker,text=text,path=str(output.relative_to(ROOT)),duration=seconds,peak=peak,rms=rms,sha256=hashlib.sha256(output.read_bytes()).hexdigest()))
        assert peak<.85 and rms>.005,(speaker,peak,rms)
report=dict(engine='eSpeak NG 1.51 / Indonesian, disclosed temporary synthetic VO',source='https://github.com/espeak-ng/espeak-ng',engineLicense='GPL-3.0-or-later; synthesis tool is not shipped; original dialogue WAV output only',voiceCloning=False,userApproved=True,entries=records)
(ROOT/'Audit/prolog-voice-manifest.json').write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n')
print('DIALOGUE_WAVS',len(records),'seconds',round(sum(r['duration'] for r in records),1),'maxPeak',max(r['peak'] for r in records))
