"""Cache pinned CC0 MakeHuman data; no MakeHuman program code is shipped.

The ignored Builds/ArtSources cache is never a Graphify input. Only selected
derived, clothed FBX models and 1K textures are imported into the game.
"""
from pathlib import Path
import hashlib
import json
import urllib.request
import zipfile

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/MakeHuman'
REVISION = 'a8bc2d54ff0ac92e78ff71431b1023eda42bf482'
REPOSITORY = 'https://raw.githubusercontent.com/makehumancommunity/makehuman/'
PACK = 'https://files.makehumancommunity.org/asset_packs/makehuman_system_assets/makehuman_system_assets_cc0.zip'
PACK_SHA256 = 'b542127a8e25547c7c29c19f2d1d2adb9a664c80396ecd694095dbc8028a0107'
FILES = ['LICENSE.ASSETS.md', 'makehuman/data/3dobjs/base.obj',
         'makehuman/data/rigs/default.mhskel', 'makehuman/data/rigs/default_weights.mhw',
         'makehuman/data/targets/macrodetails/asian-female-young.target',
         'makehuman/data/targets/macrodetails/asian-female-old.target',
         'makehuman/data/targets/macrodetails/asian-male-young.target']
SELECT = ['skins/middleage_asian_female/', 'skins/young_asian_male/',
          'hair/long01/', 'hair/short04/', 'eyes/low-poly/',
          'clothes/male_casualsuit05/', 'clothes/shoes01/',
          'eyes/materials/brown_eye.png', 'eyes/materials/brown.mhmat']


def sha(path):
    with path.open('rb') as source:
        return hashlib.file_digest(source, 'sha256').hexdigest()


def fetch(url, path):
    if path.exists():
        return
    with urllib.request.urlopen(url, timeout=90) as source, path.with_suffix('.partial').open('wb') as output:
        while block := source.read(1024 * 1024):
            output.write(block)
    path.with_suffix('.partial').rename(path)


def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    records = []
    for name in FILES:
        url = REPOSITORY + REVISION + '/' + name
        target = CACHE / Path(name).name
        fetch(url, target)
        records.append(dict(source=url, cachedPath=target.relative_to(ROOT).as_posix(), sha256=sha(target)))
    archive = CACHE / 'system-assets.zip'
    fetch(PACK, archive)
    assert sha(archive) == PACK_SHA256, 'Source pack changed: inspect the official release before changing its pinned hash'
    with zipfile.ZipFile(archive) as assets:
        assert assets.testzip() is None
        for name in assets.namelist():
            if name.endswith('/') or not any(name.startswith(p) for p in SELECT):
                continue
            assert '..' not in Path(name).parts and not Path(name).is_absolute()
            target = CACHE / 'system' / name
            if not target.exists():
                assets.extract(name, CACHE / 'system')
            records.append(dict(source=PACK + '#/' + name,
                                cachedPath=target.relative_to(ROOT).as_posix(), sha256=sha(target)))
    (ROOT / 'Audit/makehuman-assets-CC0.txt').write_bytes((CACHE / 'LICENSE.ASSETS.md').read_bytes())
    report = dict(repositoryRevision=REVISION, license='CC0-1.0 graphical assets only',
                  licenseSource='https://github.com/makehumancommunity/makehuman/blob/' + REVISION + '/LICENSE.md',
                  communityLicense='https://static.makehumancommunity.org/about/license.html',
                  systemAssets='https://static.makehumancommunity.org/assets/assetpacks/makehuman_system_assets.html',
                  packUrl=PACK, packSha256=PACK_SHA256, assets=records,
                  scope='Only graphical data. No AGPL application code, source generator or service is included in the game.')
    (ROOT / 'Audit/character-source-manifest.json').write_text(json.dumps(report, indent=2) + '\n')
    print('CHARACTER_CC0_SOURCES_VERIFIED', len(records), 'selected source files')


if __name__ == '__main__':
    main()
