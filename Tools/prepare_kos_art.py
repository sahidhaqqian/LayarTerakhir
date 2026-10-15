"""Prepare downloaded CC0 assets and original environmental print textures.
Run with /usr/bin/python3 (Pillow), after placing verified sources in --source.
"""
from pathlib import Path
from PIL import Image,ImageDraw,ImageFont,ImageFilter,ImageChops
import json,shutil,random,math,argparse
args=argparse.ArgumentParser();args.add_argument('--source',default='/tmp/kos-asset-sources');opt=args.parse_args()
ROOT=Path(__file__).resolve().parents[1];SRC=Path(opt.source);ART=ROOT/'Assets/_LayarTerakhir/Art'
manifest=json.loads((SRC/'download-manifest.json').read_text())
for record in manifest:
 i=record['id'];folder=ART/'Imported/PolyHaven'/i;folder.mkdir(parents=True,exist_ok=True)
 if record['kind']=='model':
  src=SRC/'optimized'/(i+'.fbx')
  if not src.exists():src=SRC/'PolyHaven'/i/(i+'.fbx')
  shutil.copy2(src,folder/(i+'.fbx'))
 limit=512 if i in ['plastic_bottle_gallon','binder_notebook','desk_lamp_arm_01'] else 1024
 for source in (SRC/'PolyHaven'/i).glob('*.jpg'):
  if not any('_'+c+'_' in source.name for c in ['diff','nor_gl','arm','rough']):continue
  im=Image.open(source).convert('RGB');im.thumbnail((limit,limit));im.save(folder/source.name,quality=94)
 arm=list(folder.glob('*_arm_*jpg'))
 if arm:
  im=Image.open(arm[0]).convert('RGB');ao,rough,metal=im.split()
  packed=Image.merge('RGBA',(metal,ao,Image.new('L',im.size,0),ImageChops.invert(rough)))
  packed.save(folder/(i+'_mask.png'))
 else:
  rough=next(iter(folder.glob('*_rough_*jpg')),None)
  if rough:
   im=Image.open(rough).convert('L');Image.merge('RGBA',(Image.new('L',im.size,0),Image.new('L',im.size,255),Image.new('L',im.size,0),ImageChops.invert(im))).save(folder/(i+'_mask.png'))
 names=[p.name for p in folder.glob('*') if p.is_file()]
 record['importedDirectory']=str(folder.relative_to(ROOT));record['importedFiles']=names;record['maxTextureSize']=limit
kenney=['computerKeyboard','computerMouse','computerScreen','bookcaseClosedDoors','doorwayFront','wallWindow','books','cardboardBoxOpen','ceilingFan','trashcan','speakerSmall','coatRack']
kdir=ART/'Imported/Kenney';kdir.mkdir(parents=True,exist_ok=True)
for i in kenney:shutil.copy2(SRC/'Kenney/Models/FBX format'/(i+'.fbx'),kdir/(i+'.fbx'))
shutil.copy2(SRC/'Kenney/License.txt',kdir/'License.txt')
(ROOT/'Audit/asset-download-manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
if (SRC/'model-processing.json').exists():shutil.copy2(SRC/'model-processing.json',ROOT/'Audit/model-processing.json')
T=ART/'Original/Textures';T.mkdir(parents=True,exist_ok=True)
F=Path('/usr/share/fonts/truetype/dejavu');rng=random.Random(148)
def font(size,bold=False,italic=False):return ImageFont.truetype(str(F/('DejaVuSans'+('-Bold' if bold else '-Oblique' if italic else '')+'.ttf')),size)
def paper(size,color):
 im=Image.new('RGB',size,color);noise=Image.effect_noise(size,10).convert('L');noise=Image.merge('RGB',(noise,noise,noise));return Image.blend(im,noise,.08)
def save(im,name):im.save(T/(name+'.png'))
im=paper((512,700),(221,211,176));d=ImageDraw.Draw(im);d.rectangle((0,0,512,98),fill=(105,33,26));d.text((26,14),'AGUSTUS',font=font(43,True),fill=(237,223,181));d.text((413,20),'08',font=font(42,True),fill='white');d.text((28,116),'SEN  SEL  RAB  KAM  JUM  SAB  MIN',font=font(16,True),fill=(50,51,43))
for day in range(1,32):
 x=37+(day-1)%7*67;y=170+(day-1)//7*73;d.text((x,y),str(day),font=font(27,True),fill=(140,31,26) if day%7==0 else (57,58,47))
 if day==14:d.ellipse((x-12,y-8,x+43,y+42),outline=(165,23,22),width=5)
d.text((58,582),'Ultah Ibu  14 - 08',font=font(29,italic=True),fill=(141,25,21));d.line((37,642,467,642),fill=(74,62,41),width=2);save(im,'CalendarAugust')
im=paper((512,512),(216,197,98));d=ImageDraw.Draw(im)
for y,line in enumerate(['Password gembok:', 'Tanggal & bulan', 'ulang tahun Ibu', 'biar nggak lupa.']):d.text((36,55+y*82),line,font=font(33,italic=True),fill=(48,48,35))
save(im,'StickyNote')
im=paper((512,640),(174,64,54));d=ImageDraw.Draw(im);d.text((35,30),'PERINGATAN',font=font(39,True),fill=(35,24,22));d.line((35,93,478,93),fill=(56,32,24),width=3)
for j,line in enumerate(['Raka,','Tunggakan kos 3 bulan','dan uang listrik:','Rp3.500.000','Besok pagi jam 08.00','harus sudah ditransfer.','','Bu Haji Ratna']):d.text((36,126+j*52),line,font=font(29,True if j==3 else False),fill=(44,28,24))
save(im,'RentWarning')
im=Image.new('RGB',(1024,640),(12,19,22));d=ImageDraw.Draw(im);d.rectangle((0,0,1024,81),fill=(86,21,26));d.text((28,20),'MALAM KERAMAT',font=font(34,True),fill=(229,217,197));d.text((767,29),'RAKA WIJAYA',font=font(21),fill=(193,177,159));d.rectangle((0,82,195,640),fill=(19,27,28))
for j,line in enumerate(['DASHBOARD','KONTEN','ANALITIK','KOMENTAR','PENGHASILAN']):d.text((18,125+j*58),line,font=font(18,True),fill=(137,153,149))
d.text((238,112),'DASHBOARD KREATOR',font=font(29,True),fill=(219,219,202));d.text((239,177),'STATUS KANAL: OFFLINE',font=font(24,True),fill=(158,103,80));d.text((239,248),'32',font=font(112,True),fill=(214,219,202));d.text((431,281),'PENONTON',font=font(24,True),fill=(135,154,145));d.text((431,322),'RATA-RATA TERAKHIR',font=font(17),fill=(110,128,122));d.line([(244,435),(330,432),(390,461),(452,449),(515,490),(580,474),(647,526),(740,518),(798,550),(942,555)],fill=(153,63,53),width=4);d.text((239,589),'Tidak ada siaran aktif',font=font(20),fill=(119,136,127));save(im,'MonitorDashboard')
im=paper((512,704),(38,51,49));d=ImageDraw.Draw(im)
for y,text,size in [(45,'MALAM',63),(123,'KERAMAT',52),(430,'SIARAN MISTERI',30),(490,'RAKA WIJAYA',24)]:d.text((29,y),text,font=font(size,True),fill=(190,178,146))
d.ellipse((132,235,377,394),outline=(128,68,46),width=5);d.line((170,348,224,282,286,348,341,277),fill=(128,68,46),width=4);save(im,'ChannelPoster')
im=paper((512,640),(205,198,168));d=ImageDraw.Draw(im);d.text((41,37),'PERATURAN KOS',font=font(36,True),fill=(47,51,42))
for j,line in enumerate(['JAGA KEBERSIHAN','MATIKAN LAMPU','BILA KELUAR KAMAR','JANGAN BERISIK','','Terima kasih.']):d.text((42,142+j*61),line,font=font(26,True if j<4 else False),fill=(58,61,49))
save(im,'KosRules')
im=paper((512,256),(171,85,36));d=ImageDraw.Draw(im);d.rectangle((14,16,497,240),outline=(220,174,69),width=8);d.text((48,52),'MI INSTAN',font=font(60,True),fill=(229,211,158));d.text((51,154),'RASA AYAM',font=font(28,True),fill=(60,50,31));save(im,'NoodlePacket')
im=paper((512,256),(214,211,181));d=ImageDraw.Draw(im);d.text((32,20),'AIR MINUM',font=font(47,True),fill=(32,80,82));d.text((35,95),'ISI ULANG',font=font(38,True),fill=(49,96,94));d.text((37,180),'JAGA KEBERSIHAN WADAH',font=font(20),fill=(58,77,67));save(im,'WaterLabel')
im=paper((512,512),(118,99,64));d=ImageDraw.Draw(im)
for y in range(0,512,8):d.line((0,y,512,y),fill=(95+(y%3)*5,81,52),width=2)
for y in [30,53,76,430,453,476]:d.rectangle((0,y,512,y+9),fill=(61,74,61))
save(im,'WovenMat')
# Transparent grime and contact-darkening masks, authored without external photographs.
im=Image.new('RGBA',(512,512),(0,0,0,0));d=ImageDraw.Draw(im)
for i in range(140):
 x=rng.randint(0,512);y=rng.randint(160,510);r=rng.randint(5,55)
 d.ellipse((x-r,y-r//2,x+r,y+r),fill=(48,43,24,rng.randint(8,48)))
for i in range(25):
 x=rng.randint(15,495);y=rng.randint(0,180);d.line((x,y,x+rng.randint(-10,10),y+rng.randint(80,300)),fill=(57,47,26,rng.randint(10,38)),width=rng.randint(2,9))
im=im.filter(ImageFilter.GaussianBlur(4));save(im,'DampStain')
im=Image.new('RGBA',(128,128),(0,0,0,0));pix=im.load()
for y in range(128):
 for x in range(128):
  r=((x-63.5)/64)**2+((y-63.5)/64)**2;pix[x,y]=(12,13,10,int(max(0,1-r)**2*85))
save(im,'ContactShade')
print('PREPARED',len(manifest),'CC0 assets and original labels/textures.')
