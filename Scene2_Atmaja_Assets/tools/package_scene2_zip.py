"""Package the Scene 2 drop into a single ZIP and write its SHA-256.

The archive keeps the category/asset folder structure and includes the manifest,
evidence, preview sheets and pipeline tools so it can be handed to a reviewer
without the rest of the repository. The .cache/ download cache and any existing
ZIP are excluded.

Usage:  /usr/bin/python3 Scene2_Atmaja_Assets/tools/package_scene2_zip.py
"""
from pathlib import Path
import hashlib
import zipfile

REPO = Path(__file__).resolve().parents[2]
PACKAGE = REPO / 'Scene2_Atmaja_Assets'
NAME = 'Scene2_Atmaja_Assets-2026-10-15'
ZIP_PATH = PACKAGE / (NAME + '.zip')
SHA_PATH = PACKAGE / (NAME + '.zip.sha256')

INCLUDE_DIRS = ['Architecture', 'Vegetation', 'Ground_Materials', 'Props',
                'Story_Objects', 'Previews', 'manifest', 'evidence', 'tools']
INCLUDE_FILES = ['README.md', 'asset-manifest.json', 'asset-licenses.md', 'missing-assets.md']


def gather() -> list:
    entries = []
    for directory in INCLUDE_DIRS:
        base = PACKAGE / directory
        entries += sorted(path for path in base.rglob('*') if path.is_file())
    for name in INCLUDE_FILES:
        entries.append(PACKAGE / name)
    return entries


def main() -> None:
    entries = gather()
    with zipfile.ZipFile(ZIP_PATH, 'w', zipfile.ZIP_DEFLATED, compresslevel=9) as archive:
        for path in entries:
            arcname = Path(NAME) / path.relative_to(PACKAGE)
            archive.write(path, arcname.as_posix())
    digest = hashlib.sha256(ZIP_PATH.read_bytes()).hexdigest()
    SHA_PATH.write_text(f'{digest}  {ZIP_PATH.name}\n')
    with zipfile.ZipFile(ZIP_PATH) as archive:
        bad = archive.testzip()
        count = len(archive.namelist())
    print('zip:', ZIP_PATH)
    print('entries:', count, 'crc:', 'OK' if bad is None else 'FAILED ' + str(bad))
    print('bytes:', ZIP_PATH.stat().st_size)
    print('sha256:', digest)


if __name__ == '__main__':
    main()
