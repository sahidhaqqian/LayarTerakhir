"""Package the Scene 2 recovery drop into Scene2_Atmaja_Recovery.zip + SHA-256.

Usage:  /usr/bin/python3 ArtSources/Scene2_Atmaja_Recovery/tools/package_recovery_zip.py
"""
from pathlib import Path
import hashlib
import zipfile

PKG = Path(__file__).resolve().parents[1]
NAME = 'Scene2_Atmaja_Recovery'
ZIP_PATH = PKG / (NAME + '.zip')
SHA_PATH = PKG / (NAME + '.zip.sha256')

INCLUDE_DIRS = ['01_House', '02_Motorcycle', '03_Frangipani_Tree', 'Previews',
                'Licenses', 'tools']
INCLUDE_FILES = ['asset_manifest.json', 'ASSET_ASSEMBLY_REPORT.md', 'README.md']


def gather():
    entries = []
    for directory in INCLUDE_DIRS:
        base = PKG / directory
        entries += sorted(path for path in base.rglob('*') if path.is_file())
    for name in INCLUDE_FILES:
        entries.append(PKG / name)
    return entries


def main():
    entries = gather()
    with zipfile.ZipFile(ZIP_PATH, 'w', zipfile.ZIP_DEFLATED, compresslevel=9) as archive:
        for path in entries:
            arcname = Path(NAME) / path.relative_to(PKG)
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
