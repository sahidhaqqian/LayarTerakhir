"""Fetch the official CC0 house kit; preserve the original archive and license."""
import hashlib
import json
import re
import urllib.request
import zipfile
from pathlib import Path
from datetime import datetime, timezone
ROOT = Path(__file__).resolve().parents[1]
PAGE = 'https://kenney.nl/assets/city-kit-suburban'
CACHE = ROOT / 'Builds/ArtSources/MelatiHouses'

def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    def get(url):
        return urllib.request.urlopen(urllib.request.Request(url, headers={'User-Agent':'LayarTerakhirAssetTool/1.0'}), timeout=90).read()
    html=get(PAGE).decode();url=re.search(r"href=['\"]([^'\"]+\.zip)['\"]",html)[1]
    data=get(url);archive=CACHE/'kenney-city-kit-suburban.zip';archive.write_bytes(data)
    with zipfile.ZipFile(archive) as pack:
        assert pack.testzip() is None
        assert all(not Path(n).is_absolute() and '..' not in Path(n).parts for n in pack.namelist())
        pack.extractall(CACHE/'Source')
        name=next(n for n in pack.namelist() if Path(n).name.lower()=='license.txt')
        license=pack.read(name);assert b'CC0' in license
        (ROOT/'Audit/melati-house-kit-CC0.txt').write_bytes(license)
    (ROOT/'Audit/melati-house-kit-source.json').write_text(json.dumps(dict(fetchedUtc=datetime.now(timezone.utc).isoformat(),page=PAGE,url=url,license='CC0-1.0',sha256=hashlib.sha256(data).hexdigest(),bytes=len(data),selectedNewTypes=['i','l','n','q'],existingAdaptedTypes=['a','b','d','f','h','k','m','p']),indent=2)+'\n')
    print('MELATI_HOUSE_SOURCE_VERIFIED',len(data),hashlib.sha256(data).hexdigest())
if __name__=='__main__':main()
