"""Repack downloaded ARM channels for URP; no original texture artwork."""
import json
from pathlib import Path
from PIL import Image,ImageOps
ROOT=Path(__file__).resolve().parents[1];folder=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase3/Imported'
records=[]
for path in sorted((folder/'PolyHaven').rglob('*')):
 if path.suffix.lower() not in ['.png','.jpg'] or '_arm.' not in path.name:continue
 out=path.with_name(path.stem+'_MetalSmooth.png')
 if not out.exists() or out.stat().st_mtime < path.stat().st_mtime:
  image=Image.open(path).convert('RGB');r,g,b=image.split()
  temporary=out.with_suffix('.packing.png')
  Image.merge('RGBA',(b,b,b,ImageOps.invert(g))).save(temporary)
  temporary.replace(out)
 records.append(dict(source=str(path.relative_to(ROOT)),output=str(out.relative_to(ROOT)),operation='ARM B metallic -> R; 1-G roughness -> alpha; existing maps only'))
(ROOT/'Audit/phase3-pbr-conversions.json').write_text(json.dumps(records,indent=2)+'\n')
print('PHASE3_MASKS_PACKED',len(records))
