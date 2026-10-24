"""URP channel packing of downloaded PBR maps; no painted/generated artwork."""
from pathlib import Path
from PIL import Image,ImageOps
root=Path(__file__).resolve().parents[1]
art=root/'Assets/_LayarTerakhir/assets/Art/Phase2'
for rough in (art/'Imported/Madduck/Textures').rglob('*Roughness.png'):
    stem=rough.name[:-len('Roughness.png')]
    metal=rough.with_name(stem+'Metallic.png')
    if not metal.exists():continue
    a=Image.open(rough).convert('L');r=Image.open(metal).convert('L');empty=Image.new('L',a.size)
    Image.merge('RGBA',(r,empty,empty,ImageOps.invert(a))).save(rough.with_name(stem+'MetalSmooth.png'))
for source in (art/'Imported').rglob('*ARM.*'):
    if source.suffix=='.meta':continue
    a,r,m=Image.open(source).convert('RGB').split();empty=Image.new('L',a.size)
    Image.merge('RGBA',(m,empty,empty,ImageOps.invert(r))).save(source.with_name(source.stem[:-3]+'MetalSmooth.png'))
    Image.merge('RGB',(a,a,a)).save(source.with_name(source.stem[:-3]+'Occlusion.png'))
for source in (art/'Imported').rglob('*MetalRough.png'):
    image=Image.open(source).convert('RGB');a,r,m=image.split();empty=Image.new('L',a.size)
    Image.merge('RGBA',(m,empty,empty,ImageOps.invert(r))).save(source.with_name(source.stem[:-len('MetalRough')]+'MetalSmooth.png'))
print('P2_PBR_PACKING_OK')
