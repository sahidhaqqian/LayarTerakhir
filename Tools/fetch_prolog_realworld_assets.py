"""Fetch realistic CC0 paper + bloody handprint from the internet for the prolog.

- ambientCG Paper001 (CC0): clean paper grain for the document modal.
- Wikimedia Commons "Serbian bloody handprint symbol.svg" (CC0): real
  handprint decal for the floor-2 blood trail (dialogue promises it).
- OGA blood-splatter (CC0, already vendored under Phase1/Imported/ExileBlood
  and credited in Audit/phase1-asset-credits.md) is reused for floor stains.

Writes Audit/prolog-realworld-source.json (urls, licenses, sha256).
Requires cairosvg for the SVG->PNG conversion (pip install cairosvg).
"""
import hashlib
import json
import urllib.request
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
UA = {'User-Agent': 'LayarTerakhirAssetTool/1.0'}


def get(url, timeout=120):
    req = urllib.request.Request(url, headers=UA)
    with urllib.request.urlopen(req, timeout=timeout) as r:
        return r.read()


def sha256(data):
    return hashlib.sha256(data).hexdigest()


def main():
    CACHE = ROOT / 'Builds/ArtSources/PrologRealWorld'
    CACHE.mkdir(parents=True, exist_ok=True)
    sources = {}

    paper_url = 'https://ambientcg.com/get?file=Paper001_1K-JPG.zip'
    paper_zip = get(paper_url)
    (CACHE / 'Paper001_1K-JPG.zip').write_bytes(paper_zip)
    import zipfile
    with zipfile.ZipFile(CACHE / 'Paper001_1K-JPG.zip') as z:
        color = z.read('Paper001_1K-JPG_Color.jpg')
    dest = ROOT / 'Assets/Resources/Paper/DocumentPaper.jpg'
    dest.parent.mkdir(parents=True, exist_ok=True)
    dest.write_bytes(color)
    sources['ambientcg-paper001'] = {
        'page': 'https://ambientcg.com/view?id=Paper001',
        'url': paper_url, 'license': 'CC0-1.0',
        'licenseUrl': 'https://creativecommons.org/publicdomain/zero/1.0/',
        'archive': str((CACHE / 'Paper001_1K-JPG.zip').relative_to(ROOT)),
        'file': str(dest.relative_to(ROOT)),
        'bytes': len(color), 'sha256': sha256(color)}

    svg_url = ('https://upload.wikimedia.org/wikipedia/commons/2/26/'
               'Serbian_bloody_handprint_symbol.svg')
    svg = get(svg_url)
    (CACHE / 'handprint.svg').write_bytes(svg)
    try:
        import cairosvg
    except ImportError:
        raise SystemExit('pip install cairosvg is required for the SVG conversion')
    png = cairosvg.svg2png(bytestring=svg, write_to=None, output_width=512)
    dest = (ROOT / 'Assets/_LayarTerakhir/assets/Art/Atmaja/Textures'
            / 'BloodyHandprint.png')
    dest.parent.mkdir(parents=True, exist_ok=True)
    dest.write_bytes(png)
    sources['wikimedia-handprint'] = {
        'page': ('https://commons.wikimedia.org/wiki/'
                 'File:Serbian_bloody_handprint_symbol.svg'),
        'url': svg_url, 'license': 'CC0-1.0',
        'licenseUrl': 'https://creativecommons.org/publicdomain/zero/1.0/',
        'file': str(dest.relative_to(ROOT)),
        'bytes': len(png), 'sha256': sha256(png)}
    sources['oga-blood-splatter-reuse'] = {
        'page': 'https://opengameart.org/content/blood-splatter',
        'license': 'CC0-1.0',
        'file': ('Assets/_LayarTerakhir/assets/Art/Phase1/Imported/'
                 'ExileBlood/blood.png'),
        'note': ('Vendored + credited in Audit/phase1-asset-credits.md; '
                 'reused for floor-2 stains')}

    out = ROOT / 'Audit/prolog-realworld-source.json'
    out.write_text(json.dumps(
        {'fetchedUtc': datetime.now(timezone.utc).isoformat(),
         'sources': sources}, indent=2) + '\n')
    print('MANIFEST', out, flush=True)


if __name__ == '__main__':
    main()
