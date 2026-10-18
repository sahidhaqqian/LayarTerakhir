"""Package the research drop into a single ZIP and write its SHA-256.

The archive keeps the per-asset folder structure and includes the manifest,
evidence, review checklist and pipeline tools, so it can be handed to a
reviewer without the rest of the repository.

Usage:  /usr/bin/python3 ArtSources/AssetResearch/tools/package_zip.py
"""
from pathlib import Path
import hashlib
import zipfile

REPO = Path(__file__).resolve().parents[3]
RESEARCH = REPO / 'ArtSources' / 'AssetResearch'
NAME = 'LayarTerakhir-KosClutter-Assets-2026-10-15'
ZIP_PATH = RESEARCH / (NAME + '.zip')
SHA_PATH = RESEARCH / (NAME + '.zip.sha256')

INCLUDE_DIRS = ['assets', 'manifest', 'evidence', 'tools']
INCLUDE_FILES = ['README.md', 'REVIEW-CHECKLIST.md']


def gather() -> list:
    entries = []
    for directory in INCLUDE_DIRS:
        base = RESEARCH / directory
        entries += sorted(path for path in base.rglob('*') if path.is_file())
    for name in INCLUDE_FILES:
        entries.append(RESEARCH / name)
    return entries


def main() -> None:
    entries = gather()
    with zipfile.ZipFile(ZIP_PATH, 'w', zipfile.ZIP_DEFLATED, compresslevel=9) as archive:
        for path in entries:
            arcname = Path(NAME) / path.relative_to(RESEARCH)
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
