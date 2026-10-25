"""New CC0 human motion from Quaternius; no authored keyframes or old chapter art."""
import hashlib,html,json,re,shutil,zipfile
from pathlib import Path
import requests
ROOT=Path(__file__).resolve().parents[1];cache=ROOT/'Builds/ArtSources/Phase5';pack=cache/'UniversalAnimationLibrary.zip'
page='https://opengameart.org/content/universal-animation-library'
if not pack.exists():
 r=requests.get(page,timeout=40);r.raise_for_status();urls=[html.unescape(u) for u in re.findall(r'href=["\']([^"\']+)["\']',r.text)];url=next(u for u in urls if '/sites/default/files/' in u and u.lower().endswith('.zip'));r=requests.get(url,timeout=90);r.raise_for_status();pack.write_bytes(r.content)
folder=cache/'UniversalAnimationLibrary'
with zipfile.ZipFile(pack) as z:z.extractall(folder)
source=folder/'Animation Library[Standard]/Unity/AnimationLibrary_Unity_Standard.fbx';target=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Animations/QuaterniusHumanMotion.fbx';shutil.copy2(source,target)
shutil.copy2(folder/'Animation Library[Standard]/License.txt',ROOT/'Assets/_LayarTerakhir/assets/Art/Phase5/UniversalAnimationLibrary-License.txt')
sha=lambda p:hashlib.file_digest(p.open('rb'),'sha256').hexdigest()
record=dict(id='QuaterniusUniversalAnimationLibrary',kind='animation-library',author='Quaternius',license='CC0-1.0',sourcePage=page,creatorPage='https://quaternius.com/packs/universalanimationlibrary.html',sourcePath=str(pack.relative_to(ROOT)),sourceSha256=sha(pack),path=str(target.relative_to(ROOT)),sha256=sha(target),operation='Unmodified Unity FBX from newly downloaded standard pack; original authored Idle/Walk retargeted by Unity; no generated keyframes')
(ROOT/'Audit/phase5-animation-sources.json').write_text(json.dumps([record],indent=2)+'\n');print('P5_NEW_HUMAN_MOTION',target.stat().st_size)
