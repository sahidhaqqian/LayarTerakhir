"""Generate a few clearer candidates without touching imported Unity clips during tests."""
from pathlib import Path
import json,subprocess
from gradio_client import Client,handle_file
ROOT=Path(__file__).resolve().parents[1];out=ROOT/'Audit/VoiceCandidates';out.mkdir(exist_ok=True)
manifest=json.loads((ROOT/'Audit/prolog-voice-manifest.json').read_text());client=Client('openbmb/VoxCPM-Demo');candidates=[]
for entry in manifest['entries']:
    text=entry['text'];speaker=entry['speaker']
    if text not in ['Gue belum siap berangkat.','Bentar, gue belum mau keluar sebelum cek bawah pintu.','Haus... bukakan...']:continue
    target=out/Path(entry['path']).name
    if speaker=='Bisikan':source=str(ROOT/'Audit/VoiceSamples/Bisikan.wav');delivery='Original slow raspy fictional voice-design sample; exact same spoken line'
    else:
        delivery='Clear natural spoken Indonesian, slightly slower, thoughtful pause, not whispering'
        source=client.predict(text_input=text,control_instruction=delivery,reference_wav_path_input=handle_file(str(ROOT/'Audit/VoiceSamples/Raka.wav')),use_prompt_text=False,prompt_text_input='',cfg_value_input=2.0,do_normalize=False,denoise=False,api_name='/generate')
    filters='highpass=f=70,lowpass=f=8500,loudnorm=I=-18:TP=-3:LRA=9'
    if speaker=='Bisikan':filters='highpass=f=180,lowpass=f=4500,aecho=0.6:0.4:95:0.16,loudnorm=I=-23:TP=-5:LRA=9'
    subprocess.run(['ffmpeg','-loglevel','error','-y','-i',source,'-af',filters,'-ac','1','-ar','24000','-c:a','pcm_s16le',str(target)],check=True)
    candidates.append(dict(speaker=speaker,text=text,path=str(target.relative_to(ROOT)),delivery=delivery))
    print('CANDIDATE',speaker,text,flush=True)
(ROOT/'Audit/voice-candidates.json').write_text(json.dumps(dict(entries=candidates),ensure_ascii=False,indent=2)+'\n')
