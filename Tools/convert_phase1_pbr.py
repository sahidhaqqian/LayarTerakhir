"""Convert downloaded PBR channels for URP. Does not synthesize textures/artwork."""
from pathlib import Path
from PIL import Image, ImageOps

ROOT = Path(__file__).resolve().parents[1]
IMPORTED = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Imported'
for source in IMPORTED.glob('*/*ARM.*'):
    if source.suffix not in ('.png', '.jpg'):
        continue
    ao, roughness, metal = Image.open(source).convert('RGB').split()
    blank = Image.new('L', ao.size)
    prefix = source.name.split('ARM.')[0]
    Image.merge('RGBA', (metal, blank, blank, ImageOps.invert(roughness))).save(source.with_name(prefix + 'MetalSmooth.png'))
    Image.merge('RGB', (ao, ao, ao)).save(source.with_name(prefix + 'Occlusion.png'))
print('PHASE1_PBR_CHANNELS_CONVERTED')
