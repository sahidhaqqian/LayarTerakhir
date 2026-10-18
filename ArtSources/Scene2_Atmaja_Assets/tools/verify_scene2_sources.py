"""Re-verify every Scene 2 asset against its live source and capture license
evidence.

- Poly Haven: re-fetches API info/files JSON (raw evidence in manifest/api/),
  checks each shipped file's MD5 against the API MD5 where applicable.
- OpenGameArt: re-downloads the source archives (cache kept in .cache/),
  checks the archive SHA-256, re-extracts unmodified members and checks their
  SHA-256 against the shipped files, and captures the item pages plus the
  extracted license statements.
- All files: SHA-256 + byte size against manifest/source-manifest.json.
- Captures the Poly Haven license/ToS evidence.

Usage:  /usr/bin/python3 ArtSources/Scene2_Atmaja_Assets/tools/verify_scene2_sources.py
Exits non-zero if any check fails.
"""
from pathlib import Path
import hashlib
import io
import json
import re
import sys
import tempfile
import time
import zipfile

import requests
from PIL import Image

REPO = Path(__file__).resolve().parents[3]
PACKAGE = REPO / 'ArtSources' / 'Scene2_Atmaja_Assets'
MANIFEST = PACKAGE / 'manifest' / 'source-manifest.json'
REPORT = PACKAGE / 'manifest' / 'verification-report.json'
API_DIR = PACKAGE / 'manifest' / 'api'
EVIDENCE = PACKAGE / 'evidence'
CACHE = PACKAGE / '.cache'
USER_AGENT = 'LayarTerakhir-AssetResearch/2.0'

SESSION = requests.Session()
SESSION.headers['User-Agent'] = USER_AGENT


def md5_bytes(payload: bytes) -> str:
    return hashlib.md5(payload).hexdigest()


def sha256_bytes(payload: bytes) -> str:
    return hashlib.sha256(payload).hexdigest()


def fetch_bytes(url: str) -> bytes:
    response = SESSION.get(url, timeout=300)
    response.raise_for_status()
    return response.content


def capture_ph_license() -> None:
    response = SESSION.get('https://polyhaven.com/license', timeout=120)
    response.raise_for_status()
    (EVIDENCE / 'polyhaven-license-raw.html').write_text(response.text)
    match = re.search(r'\{"props":.*\}\s*</script>', response.text, re.S)
    statements = {}
    if match:
        payload = json.loads(match.group(0)[:-9])
        store = payload['props']['pageProps']['_nextI18Next']['initialI18nStore']['en']
        license_blocks = store.get('license', {})
        for key in ['title', 'description', 'p2', 'p5l1', 'p5l2', 'p5l3', 'p6']:
            statements[key] = license_blocks.get(key, '')
    lines = [
        'Poly Haven license evidence (Scene 2 drop)',
        'Fetched: ' + time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()),
        'URL: https://polyhaven.com/license',
        'HTTP: ' + str(response.status_code),
        '',
        'Extracted statements (site-wide asset license):',
    ]
    for key, value in statements.items():
        clean = re.sub(r'<[^>]+>', '', str(value)).replace('&amp;', '&')
        lines.append(f'- [{key}] {clean}')
    lines += [
        '',
        'Conclusion: Poly Haven grants all assets under CC0 1.0 (public domain,',
        'commercial use allowed, attribution not required). Raw page kept as',
        'polyhaven-license-raw.html in this directory.',
    ]
    (EVIDENCE / 'polyhaven-license.txt').write_text('\n'.join(lines) + '\n')
    tos = SESSION.get('https://raw.githubusercontent.com/Poly-Haven/Public-API/master/ToS.md',
                      timeout=120)
    tos.raise_for_status()
    (EVIDENCE / 'polyhaven-api-tos.md').write_text(
        'Fetched: ' + time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()) + '\n'
        'URL: https://github.com/Poly-Haven/Public-API/blob/master/ToS.md\n\n' + tos.text)


OGA_MEMBER_MAP = {
    'wooden-door-0': {
        'Story_Objects/wooden-door-0/Wooden_Door.fbx': ('Wooden Door.zip', 'Wooden Door.fbx'),
    },
    'breakable-doors-and-windows': {
        **{'Story_Objects/breakable-doors-and-windows/fbx/' + name: (
            'doorsandwindows_fbx_gltf_textures_blend.zip', 'FBXs/' + name)
           for name in [
               'Door_DoorFrame_Single_1.fbx', 'Door_DoorFrame_Single_2.fbx',
               'Door_DoorFrame_Double.fbx',
               'Door_DoorFrame_Broken_Single_1.fbx', 'Door_DoorFrame_Broken_single_2.fbx',
               'Door_DoorFrame_Broken_Double.fbx',
               'Windows_Window_1.fbx', 'Windows_Window_2.fbx',
               'Windows_BrokenWindow_1.fbx', 'Windows_BrokenWindow_2.fbx']},
        'Story_Objects/breakable-doors-and-windows/README.txt': (
            'doorsandwindows_fbx_gltf_textures_blend.zip', 'README.txt'),
    },
    'chain': {
        'Story_Objects/chain/chain.fbx': ('chain.7z', 'chain.FBX'),
    },
    'small-old-house': {
        'Architecture/small-old-house/House.obj': ('House.zip', 'House.obj'),
        'Architecture/small-old-house/House.mtl': ('House.zip', 'House.mtl'),
        'Architecture/small-old-house/readme.txt': ('House.zip', 'readme.txt'),
    },
}


def capture_oga_evidence() -> dict:
    pages = {
        'wooden-door-0': 'https://opengameart.org/content/wooden-door-0',
        'breakable-doors-and-windows': 'https://opengameart.org/content/breakable-doors-and-windows',
        'chain': 'https://opengameart.org/content/chain',
        'small-old-house': 'https://opengameart.org/content/small-old-house',
    }
    lines = [
        'OpenGameArt license evidence (Scene 2 drop)',
        'Fetched: ' + time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()),
        '',
    ]
    results = {}
    for slug, url in pages.items():
        html = fetch_bytes(url).decode('utf-8', 'replace')
        (EVIDENCE / f'opengameart-{slug}.html').write_text(html)
        plain = re.sub(r'<script.*?</script>', ' ', html, flags=re.S)
        plain = re.sub(r'<[^>]+>', ' ', plain)
        plain = re.sub(r'\s+', ' ', plain)
        match = re.search(r'License\(s\):(.{0,80}?)Collections', plain)
        license_text = match.group(1).strip() if match else 'NOT FOUND'
        lines += [f'{slug}: {url}', f'  License(s) field: {license_text}', '']
        results[slug] = {'url': url, 'licenseField': license_text}
    lines += [
        'Note: the per-item license shown on the page is the authoritative license for',
        'these OpenGameArt files. The wooden door, the breakable doors/windows pack and the',
        'small old house require credit (CC-BY) as noted in asset-licenses.md; the chain is',
        'CC BY 4.0 (credit Kutejnikov). Raw pages retained next to this file.',
    ]
    (EVIDENCE / 'opengameart-license-notes.txt').write_text('\n'.join(lines) + '\n')
    return results


def expected_ph_map(record: dict, files: dict) -> dict:
    """relative shipped path -> (source url, api md5)"""
    expected = {}
    asset_id = record['id']
    if record.get('type') == 'texture':
        for key, suffix in (('Diffuse', 'diff'), ('nor_gl', 'nor_gl'), ('arm', 'arm')):
            entry = files[key]['1k']['jpg']
            rel = f"{record['category']}/{asset_id}/{asset_id}_{suffix}_1k.jpg"
            expected[rel] = (entry['url'], entry['md5'])
        return expected
    entry = files['fbx']['1k']['fbx']
    rel = f"{record['category']}/{asset_id}/{asset_id}_1k.fbx"
    expected[rel] = (entry['url'], entry['md5'])
    for include_name, include_entry in files['gltf']['1k']['gltf']['include'].items():
        if include_name.endswith('.bin') or not include_name.lower().endswith(('.jpg', '.png')):
            continue
        rel = f"{record['category']}/{asset_id}/textures/{Path(include_name).name}"
        expected[rel] = (include_entry['url'], include_entry['md5'])
    return expected


def verify_ph(record: dict, report: dict) -> None:
    asset_id = record['id']
    info = SESSION.get('https://api.polyhaven.com/info/' + asset_id, timeout=120).json()
    files = SESSION.get('https://api.polyhaven.com/files/' + asset_id, timeout=120).json()
    API_DIR.mkdir(parents=True, exist_ok=True)
    (API_DIR / f'{asset_id}.info.json').write_text(json.dumps(info, indent=2) + '\n')
    (API_DIR / f'{asset_id}.files.json').write_text(json.dumps(files, indent=2) + '\n')
    expected = expected_ph_map(record, files)
    checks = []
    for file_entry in record['files']:
        relative = file_entry['path']
        path = PACKAGE / relative
        check = {'file': relative, 'exists': path.is_file()}
        if path.is_file():
            payload = path.read_bytes()
            check['sha256'] = sha256_bytes(payload)
            check['bytes'] = len(payload)
            check['sha256MatchesManifest'] = check['sha256'] == file_entry['sha256']
            check['bytesMatchManifest'] = check['bytes'] == file_entry['bytes']
            if relative in expected:
                _, api_md5 = expected[relative]
                check['apiMd5'] = api_md5
                check['md5'] = md5_bytes(payload)
                check['md5MatchesSource'] = check['md5'] == api_md5
        checks.append(check)
    missing_expected = [name for name in expected
                        if all(c['file'] != name for c in checks)]
    passed = all(
        c.get('exists') and c.get('sha256MatchesManifest', True)
        and c.get('bytesMatchManifest', True) and c.get('md5MatchesSource', True)
        for c in checks) and not missing_expected
    report['assets'].append({
        'id': asset_id, 'source': 'polyhaven', 'sourcePage': record['sourcePage'],
        'license': record['license'], 'passed': passed,
        'expectedButNotDownloaded': missing_expected, 'checks': checks,
    })
    report['passed'] = report['passed'] and passed
    print(('PASS' if passed else 'FAIL'), asset_id, flush=True)


def verify_oga(record: dict, report: dict) -> None:
    asset_id = record['id']
    archive_info = record['sourceArchive']
    archive_url = archive_info['url']
    CACHE.mkdir(parents=True, exist_ok=True)
    name = archive_url.rsplit('/', 1)[-1]
    cached = CACHE / name
    if cached.is_file():
        archive_bytes = cached.read_bytes()
    else:
        archive_bytes = fetch_bytes(archive_url)
        cached.write_bytes(archive_bytes)
    archive_sha = sha256_bytes(archive_bytes)
    checks = [{'file': name, 'kind': 'sourceArchive',
               'sha256MatchesManifest': archive_sha == archive_info['sha256'],
               'sha256': archive_sha, 'bytes': len(archive_bytes)}]
    member_map = OGA_MEMBER_MAP[asset_id]
    extracted = {}
    if name.endswith('.zip'):
        with zipfile.ZipFile(io.BytesIO(archive_bytes)) as archive:
            for _, (_, member) in member_map.items():
                if member not in extracted:
                    extracted[member] = archive.read(member)
    else:
        import py7zr
        with tempfile.TemporaryDirectory() as tmp:
            with py7zr.SevenZipFile(io.BytesIO(archive_bytes)) as archive:
                archive.extract(path=tmp, targets=list({m for _, m in member_map.values()}))
            for member in {m for _, m in member_map.values()}:
                extracted[member] = (Path(tmp) / member).read_bytes()
    for relative, (_, member) in member_map.items():
        path = PACKAGE / relative
        check = {'file': relative, 'exists': path.is_file(), 'member': member}
        if path.is_file():
            payload = path.read_bytes()
            check['sha256'] = sha256_bytes(payload)
            check['sha256MatchesArchiveMember'] = check['sha256'] == sha256_bytes(extracted[member])
        checks.append(check)
    for file_entry in record['files']:
        if file_entry['path'] in member_map:
            continue
        relative = file_entry['path']
        path = PACKAGE / relative
        check = {'file': relative, 'exists': path.is_file()}
        if path.is_file():
            payload = path.read_bytes()
            check['sha256'] = sha256_bytes(payload)
            check['bytes'] = len(payload)
            check['sha256MatchesManifest'] = check['sha256'] == file_entry['sha256']
            check['bytesMatchManifest'] = check['bytes'] == file_entry['bytes']
            if relative.lower().endswith(('.png', '.jpg', '.jpeg')) and 'textures' in relative:
                image = Image.open(io.BytesIO(payload))
                check['dimensions'] = list(image.size)
                check['withinTexLimit'] = max(image.size) <= 1024
        checks.append(check)
    passed = all(
        c.get('exists', True) and c.get('sha256MatchesManifest', True)
        and c.get('bytesMatchManifest', True)
        and c.get('sha256MatchesArchiveMember', True)
        and c.get('withinTexLimit', True) for c in checks)
    report['assets'].append({
        'id': asset_id, 'source': 'opengameart', 'sourcePage': record['sourcePage'],
        'license': record['license'], 'passed': passed, 'checks': checks,
    })
    report['passed'] = report['passed'] and passed
    print(('PASS' if passed else 'FAIL'), asset_id, flush=True)


def main() -> int:
    records = json.loads(MANIFEST.read_text())
    EVIDENCE.mkdir(parents=True, exist_ok=True)
    capture_ph_license()
    license_fields = capture_oga_evidence()
    report = {
        'verifiedAt': time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()),
        'ogaLicenseFields': license_fields,
        'assets': [],
        'passed': True,
    }
    for record in records:
        try:
            if record['source'] == 'polyhaven':
                verify_ph(record, report)
            else:
                verify_oga(record, report)
        except Exception as error:
            report['assets'].append({'id': record['id'], 'passed': False,
                                     'error': repr(error)})
            report['passed'] = False
            print('FAIL', record['id'], repr(error), flush=True)
    REPORT.write_text(json.dumps(report, indent=2) + '\n')
    print('report:', REPORT, 'overall:', 'PASS' if report['passed'] else 'FAIL')
    return 0 if report['passed'] else 1


if __name__ == '__main__':
    raise SystemExit(main())
