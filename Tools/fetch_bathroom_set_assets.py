"""Import selected cached CC0 bathroom models for the Kos bathroom expansion.

No downloads (archives were fetched earlier from OpenGameArt CC0 pages and are
cached under Builds/ArtSources/BathroomSet). No original artwork is modeled.
Records source pages, authors, licenses and file hashes in
Audit/bathroom-set-cc0-sources.json.
"""
from pathlib import Path
import hashlib
import json
import shutil
from PIL import Image, ImageOps

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/BathroomSet'
TREATS = CACHE / 'Tiny_Treats_Bubbly_Bathroom_1.0'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet'
OUT.mkdir(parents=True, exist_ok=True)
records = []


def sha(path):
    return hashlib.sha256(Path(path).read_bytes()).hexdigest()


def copy(source, target):
    target.parent.mkdir(parents=True, exist_ok=True)
    shutil.copyfile(source, target)
    return dict(path=target.relative_to(ROOT).as_posix(), sha256=sha(target))


# Tiny Treats Bubbly Bathroom 1.0 by Isa Lousberg (www.isalousberg.com), CC0-1.0.
treats_files = [
    'bath', 'mirror', 'toothbrush_blue', 'toothbrush_pink', 'toothbrush_cup',
    'wall_shelf', 'container_bathroom_C_blue', 'towel_blue',
]
treats_entry = dict(
    name='Tiny Treats - Bubbly Bathroom set (subset)',
    page='https://opengameart.org/content/tiny-treats-bubbly-bathroom-set',
    author='Isa Lousberg', license='CC0-1.0', files=[],
)
for name in treats_files:
    treats_entry['files'].append(
        copy(TREATS / 'Assets/fbx' / (name + '.fbx'), OUT / 'TinyTreats' / (name + '.fbx')))
treats_entry['files'].append(
    copy(TREATS / 'Assets/Textures/tiny_treats_texture_1.png', OUT / 'TinyTreats/tiny_treats_texture_1.png'))
treats_entry['files'].append(copy(TREATS / 'License.txt', OUT / 'TinyTreats/License.txt'))
records.append(treats_entry)

# Bucket 3 by plaggy, CC0-1.0.
bucket_entry = dict(
    name='Bucket 3', page='https://opengameart.org/content/bucket-3',
    author='plaggy', license='CC0-1.0', files=[],
)
for fname in ['Bucket3.FBX', 'Bucket3Albedo.png', 'Bucket3Metal.png', 'Bucket3AO.png']:
    bucket_entry['files'].append(copy(CACHE / fname, OUT / 'Bucket3' / fname))
bucket_entry['files'].append(copy(CACHE / 'Bucket3Normal.png', OUT / 'Bucket3/Bucket3_Normal.png'))
bucket_entry['files'].append(copy(CACHE / 'Bucket3Rough.png', OUT / 'Bucket3/Bucket3Rough.png'))
rough = Image.open(OUT / 'Bucket3/Bucket3Rough.png').convert('L')
ao = Image.open(OUT / 'Bucket3/Bucket3AO.png').convert('L')
metal = Image.open(OUT / 'Bucket3/Bucket3Metal.png').convert('L')
mask_path = OUT / 'Bucket3/Bucket3_mask.png'
Image.merge('RGBA', (metal, ao, Image.new('L', rough.size, 0), ImageOps.invert(rough))).save(mask_path)
bucket_entry['files'].append(dict(path=mask_path.relative_to(ROOT).as_posix(), sha256=sha(mask_path)))
records.append(bucket_entry)

# Toilet paper by TyberiusGames, CC0-1.0.
tp_entry = dict(
    name='Toilet paper', page='https://opengameart.org/content/toilet-paper',
    author='TyberiusGames', license='CC0-1.0', files=[],
)
tp_entry['files'].append(copy(CACHE / 'toiletpaper.fbx', OUT / 'ToiletPaper/toiletpaper.fbx'))
records.append(tp_entry)

(ROOT / 'Audit/bathroom-set-cc0-sources.json').write_text(json.dumps(records, indent=2) + '\n')
print('BATHROOM_SET_SOURCES', len(records))
