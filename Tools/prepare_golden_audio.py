"""Decode the verified real recordings/OGGs using libsndfile; trim, EQ and seam loops.
No external source is synthesized or relabelled as an original recording.
"""
from pathlib import Path
import ctypes as C, json, zipfile, wave, hashlib
import numpy as np

ROOT=Path(__file__).resolve().parents[1];TMP=Path('/tmp/layar-golden-sources')
IMPORTED=ROOT/'Assets/_LayarTerakhir/assets/Audio/Imported';OUT=ROOT/'Assets/_LayarTerakhir/assets/Audio/Prepared';OUT.mkdir(parents=True,exist_ok=True)
class Info(C.Structure):
    _fields_=[('frames',C.c_int64),('samplerate',C.c_int),('channels',C.c_int),('format',C.c_int),('sections',C.c_int),('seekable',C.c_int)]
sf=C.CDLL('libsndfile.so.1');sf.sf_open.argtypes=[C.c_char_p,C.c_int,C.POINTER(Info)];sf.sf_open.restype=C.c_void_p
sf.sf_readf_float.argtypes=[C.c_void_p,C.POINTER(C.c_float),C.c_int64];sf.sf_readf_float.restype=C.c_int64
sf.sf_close.argtypes=[C.c_void_p];sf.sf_strerror.argtypes=[C.c_void_p];sf.sf_strerror.restype=C.c_char_p
manifest=[]
def read(path):
    info=Info();handle=sf.sf_open(str(path).encode(),0x10,C.byref(info))
    assert handle,sf.sf_strerror(None)
    data=np.empty((info.frames,info.channels),np.float32)
    count=sf.sf_readf_float(handle,data.ctypes.data_as(C.POINTER(C.c_float)),info.frames);sf.sf_close(handle)
    return data[:count].mean(axis=1).astype(np.float64),info.samplerate
def write(name,data,rate,source,loop=False,cutoff=0,rms=None,peak=-8,seconds=0):
    data=data-data.mean()
    if seconds:data=data[:int(seconds*rate)]
    if cutoff:
        bins=np.fft.rfftfreq(len(data),1/rate);weights=1/(1+(bins/cutoff)**6)
        data=np.fft.irfft(np.fft.rfft(data)*weights,n=len(data))
    target_rate=32000
    data=np.interp(np.arange(round(len(data)*target_rate/rate))*rate/target_rate,np.arange(len(data)),data);rate=target_rate
    if loop:
        n=min(int(.4*rate),len(data)//8);weights=np.linspace(0,1,n)
        first=data[:n]*weights+data[-n:]*(1-weights);data=data[:-n];data[:n]=first
    else:
        n=min(int(.008*rate),len(data)//4);data[:n]*=np.linspace(0,1,n);data[-n:]*=np.linspace(1,0,n)
    level=np.sqrt(np.mean(data*data));assert level>1e-7,source
    if rms is not None:data*=10**(rms/20)/level
    else:data*=10**(peak/20)/max(abs(data))
    if max(abs(data))>.94:data*=.94/max(abs(data))
    pcm=(data*32767).astype('<i2');path=OUT/(name+'.wav')
    with wave.open(str(path),'wb') as f:f.setnchannels(1);f.setsampwidth(2);f.setframerate(rate);f.writeframes(pcm.tobytes())
    manifest.append(dict(name=name,source=source,path=str(path.relative_to(ROOT)),seconds=len(data)/rate,channels=1,sampleRate=rate,
        rmsDbFS=float(20*np.log10(np.sqrt(np.mean(data*data)))),peakDbFS=float(20*np.log10(max(abs(data)))),loop=loop,lowpassHz=cutoff,sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
for raw,name,duration,cutoff,level in [('RainOnTin','RainRoof',24,5500,-24),('VentilationFan','PCFan',14,1800,-28),('ApartmentRoomTone','RoomTone',14,2600,-34)]:
    data,rate=read(IMPORTED/(raw+'.mp3'));write(name,data[int(.4*rate):],rate,raw,True,cutoff,level,seconds=duration)
for raw,name,duration,peak in [('ClothingRustle','ClothPickup',1.4,-14),('MotorcycleStart','DepartureEngine',8,-12)]:
    data,rate=read(IMPORTED/(raw+'.mp3'));onset=np.flatnonzero(abs(data)>max(abs(data))*.04)
    start=max(0,int(onset[0])-.1*rate) if len(onset) else 0
    write(name,data[int(start):],rate,raw,seconds=duration,peak=peak)
selections=[('impact-sounds','impactWood_heavy_000','DoorKnock',-9),('impact-sounds','impactWood_medium_001','DoorLatch',-13),
            ('impact-sounds','impactMetal_light_000','DrawerUnlock',-15),('impact-sounds','impactBell_heavy_000','DonationChime',-17),
            ('ui-audio','click1','UIConfirm',-22),('ui-audio','switch1','PhoneSwitch',-21)]
for i in range(3):selections.append(('impact-sounds','footstep_concrete_00'+str(i),'FootstepTile'+str(i),-13))
for pack,file,name,peak in selections:
    with zipfile.ZipFile(TMP/(pack+'.zip')) as z:
        member='Audio/'+file+'.ogg';payload=z.read(member)
    raw=TMP/(file+'.ogg');raw.write_bytes(payload);data,rate=read(raw)
    write(name,data,rate,'Kenney '+pack+' / '+member,peak=peak)
# Original short mechanical vibration texture, explicitly identified as authored synthesis.
rate=32000;t=np.arange(int(1.2*rate))/rate
envelope=np.maximum(0,np.sin(np.pi*np.minimum(1,(t%1.2)/.34))) * ((t%1.2)<.34)
signal=(np.sin(2*np.pi*145*t)+.3*np.sin(2*np.pi*290*t))*.1*envelope
write('PhoneVibration',signal,rate,'Original procedural mechanical vibration; no external recording',loop=True,rms=-28)
(ROOT/'Audit/golden-audio-processing.json').write_text(json.dumps(manifest,indent=2)+'\n')
print('AUDIO_PREPARED',len(manifest),'clips',sum((OUT/(x['name']+'.wav')).stat().st_size for x in manifest),'bytes',flush=True)
