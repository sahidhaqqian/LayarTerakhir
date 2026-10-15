"""Re-verify every downloaded asset against the live Poly Haven API and capture
license evidence.

- Re-fetches the API info/files JSON per asset into manifest/api/ (raw evidence).
- Checks each downloaded FBX/texture MD5 against the API-provided MD5.
- Re-checks SHA-256 and byte size of every file on disk.
- Rewrites evidence/polyhaven-license.txt as readable license statements and
  keeps the raw HTML as evidence/polyhaven-license-raw.html.
- Saves the public API terms of service for the record.

Usage:  /usr/bin/python3 AssetResearch/tools/verify_sources.py
Exits non-zero if any check fails.
"""
from pathlib import Path
import hashlib
import json
import re
import sys
import time

import requests

REPO = Path(__file__).resolve().parents[2]
RESEARCH = REPO / 'AssetResearch'
MANIFEST = RESEARCH / 'manifest' / 'source-manifest.json'
REPORT = RESEARCH / 'manifest' / 'verification-report.json'
API_DIR = RESEARCH / 'manifest' / 'api'
EVIDENCE = RESEARCH / 'evidence'
USER_AGENT = 'LayarTerakhir-AssetResearch/1.0'

SESSION = requests.Session()
SESSION.headers['User-Agent'] = USER_AGENT


def md5(path: Path) -> str:
    return hashlib.md5(path.read_bytes()).hexdigest()


def sha256(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def fetch_json(url: str) -> dict:
    response = SESSION.get(url, timeout=120)
    response.raise_for_status()
    return response.json()


def capture_license_evidence() -> None:
    response = SESSION.get('https://polyhaven.com/license', timeout=120)
    response.raise_for_status()
    (EVIDENCE / 'polyhaven-license-raw.html').write_text(response.text)
    match = re.search(r'\{"props":.*\}\s*</script>', response.text, re.S)
    statements = {}
    if match:
        payload = json.loads(match.group(0)[:-9])
        store = payload['props']['pageProps']['_nextI18Next']['initialI18nStore']['en']
        license_blocks = store.get('license', {})
        keys = ['title', 'description', 'p2', 'p5l1', 'p5l2', 'p5l3', 'p6']
        statements = {key: license_blocks.get(key, '') for key in keys}
    lines = [
        'Poly Haven license evidence',
        'Fetched: ' + time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()),
        'URL: https://polyhaven.com/license',
        'HTTP: ' + str(response.status_code),
        '',
        'Extracted statements (site-wide asset license):',
    ]
    for key, value in statements.items():
        clean = re.sub(r'<[^>]+>', '', str(value)).replace('&amp;', '&')
        lines.append(f'- [{key}] {clean}')
    lines.append('')
    lines.append('Conclusion: Poly Haven grants all assets under CC0 1.0 (public domain,')
    lines.append('commercial use allowed, attribution not required). Raw page kept as')
    lines.append('polyhaven-license-raw.html in this directory.')
    lines.append('')
    (EVIDENCE / 'polyhaven-license.txt').write_text('\n'.join(lines))


def capture_api_tos() -> None:
    url = 'https://raw.githubusercontent.com/Poly-Haven/Public-API/master/ToS.md'
    response = SESSION.get(url, timeout=120)
    response.raise_for_status()
    (EVIDENCE / 'polyhaven-api-tos.md').write_text(
        'Fetched: ' + time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()) + '\n'
        'URL: ' + url + '\n\n' + response.text)


def expected_files(asset_id: str, files: dict) -> dict:
    """Map relative download path -> (url, md5) exactly as fetch_assets.py."""
    expected = {}
    entry = files['fbx']['1k']['fbx']
    expected[str(Path(asset_id) / (asset_id + '_1k.fbx'))] = (entry['url'], entry['md5'])
    for include_name, include_entry in files['gltf']['1k']['gltf']['include'].items():
        if include_name.endswith('.bin'):
            continue
        if not include_name.lower().endswith(('.jpg', '.png', '.jpeg')):
            continue
        name = Path(include_name).name
        expected[str(Path(asset_id) / 'textures' / name)] = (include_entry['url'], include_entry['md5'])
    return expected


def main() -> int:
    records = json.loads(MANIFEST.read_text())
    API_DIR.mkdir(parents=True, exist_ok=True)
    EVIDENCE.mkdir(parents=True, exist_ok=True)
    capture_license_evidence()
    capture_api_tos()

    report = {'verifiedAt': time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()),
              'assets': [], 'passed': True}
    for record in records:
        asset_id = record['id']
        info = fetch_json('https://api.polyhaven.com/info/' + asset_id)
        files = fetch_json('https://api.polyhaven.com/files/' + asset_id)
        (API_DIR / f'{asset_id}.info.json').write_text(json.dumps(info, indent=2) + '\n')
        (API_DIR / f'{asset_id}.files.json').write_text(json.dumps(files, indent=2) + '\n')
        expected = expected_files(asset_id, files)
        checks = []
        for file_entry in record['files']:
            relative = file_entry['file']
            path = RESEARCH / 'assets' / relative
            check = {'file': relative, 'exists': path.is_file()}
            if path.is_file():
                check['sha256'] = sha256(path)
                check['bytes'] = path.stat().st_size
                check['sha256MatchesManifest'] = check['sha256'] == file_entry['sha256']
                check['bytesMatchManifest'] = check['bytes'] == file_entry['bytes']
                if relative in expected:
                    _, api_md5 = expected[relative]
                    check['apiMd5'] = api_md5
                    check['md5'] = md5(path)
                    check['md5MatchesSource'] = check['md5'] == api_md5
            checks.append(check)
        asset_passed = all(
            c.get('exists') and c.get('sha256MatchesManifest', True)
            and c.get('bytesMatchManifest', True) and c.get('md5MatchesSource', True)
            for c in checks)
        expected_missing = [name for name in expected
                            if all(c['file'] != name for c in checks)]
        if expected_missing:
            asset_passed = False
        report['assets'].append({
            'id': asset_id,
            'sourcePage': record['sourcePage'],
            'license': record['license'],
            'passed': asset_passed,
            'expectedButNotDownloaded': expected_missing,
            'checks': checks,
        })
        report['passed'] = report['passed'] and asset_passed
        print(('PASS' if asset_passed else 'FAIL'), asset_id, flush=True)

    REPORT.write_text(json.dumps(report, indent=2) + '\n')
    print('report:', REPORT, 'overall:', 'PASS' if report['passed'] else 'FAIL')
    return 0 if report['passed'] else 1


if __name__ == '__main__':
    raise SystemExit(main())
