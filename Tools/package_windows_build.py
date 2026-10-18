"""Verify real Unity output, then ZIP the complete player for Windows transfer.
Usage: python3 Tools/package_windows_build.py
Requires a completed WindowsDevelopmentBuild invocation and its real BuildReport.
"""
from pathlib import Path
from datetime import datetime,timezone
import argparse,hashlib,json,struct,zipfile,zlib,shutil
from record_designer_sources import snapshot
root=Path(__file__).resolve().parents[1];out=root/'Builds/Windows';logs=root/'Builds/Logs'
parser=argparse.ArgumentParser(description=__doc__)
parser.add_argument('--source-record',default='Audit/designer-build-sources.json')
args=parser.parse_args()
source_record=json.loads((root/args.source_record).read_text())
assert source_record['sources']==snapshot(),'Source changed after the designer build snapshot'
voices=json.loads((root/'Audit/prolog-voice-manifest.json').read_text())
assert 'VoxCPM2' in voices['engine'] and len(voices['entries'])>=17,'Wrong/stale speech bank'
# This manifest verifies the retained placeholder bank, not final acting quality.
# Ibu/Ratna conversations use subtitles pending the designer's human recordings.
for voice in voices['entries']:
 assert hashlib.sha256((root/voice['path']).read_bytes()).hexdigest()==voice['sha256'],'Stale voice hash: '+voice['path']
report=json.loads((logs/'windows-development-build-report.json').read_text())
invocation=json.loads((logs/'windows-build-invocation.json').read_text())
assert invocation['exitCode']==0,'Unity did not exit successfully'
assert invocation.get('status')=='Finished','Build invocation has not completed'
assert datetime.fromisoformat(invocation['startedUtc'])<=datetime.fromisoformat(report['startedUtc'])<=datetime.fromisoformat(report['endedUtc'])<=datetime.fromisoformat(invocation['endedUtc']),'Build report is stale or does not belong to this invocation'
assert report['result']=='Succeeded' and report['totalErrors']==0,'Unity build failed'
assert report['target']=='StandaloneWindows64' and report['scriptingBackend']=='Mono2x'
assert 'Development' in report['options']
assert report['scenes'][0]=='Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity'
assert 'Assets/_LayarTerakhir/Scenes/Prolog_RumahAtmaja.unity' in report['scenes'],'Atmaja continuation missing from build'
assert report['scenes'][2]=='Assets/_LayarTerakhir/Scenes/Prolog_KoridorKos.unity','Two-floor kos corridor missing from build'
required=['LayarTerakhir.exe','UnityPlayer.dll','UnityCrashHandler64.exe',
 'MonoBleedingEdge/EmbedRuntime/mono-2.0-bdwgc.dll','MonoBleedingEdge/EmbedRuntime/MonoPosixHelper.dll',
 'MonoBleedingEdge/etc/mono/config','LayarTerakhir_Data/globalgamemanagers',
 'LayarTerakhir_Data/globalgamemanagers.assets','LayarTerakhir_Data/boot.config','LayarTerakhir_Data/level0',
 'LayarTerakhir_Data/resources.assets','LayarTerakhir_Data/sharedassets0.assets','LayarTerakhir_Data/level2',
 'LayarTerakhir_Data/Managed/Assembly-CSharp.dll','LayarTerakhir_Data/Managed/Unity.InputSystem.dll',
 'LayarTerakhir_Data/Managed/Unity.RenderPipelines.Universal.Runtime.dll',
 'LayarTerakhir_Data/Resources/unity default resources','LayarTerakhir_Data/Resources/unity_builtin_extra']
for name in required:assert (out/name).is_file() and (out/name).stat().st_size>0,name
architectures={}
for name in required[:1]+['UnityPlayer.dll','MonoBleedingEdge/EmbedRuntime/mono-2.0-bdwgc.dll']:
 with (out/name).open('rb') as f:
  assert f.read(2)==b'MZ',name;f.seek(0x3c);offset=struct.unpack('<I',f.read(4))[0];f.seek(offset)
  assert f.read(4)==b'PE\0\0' and struct.unpack('<H',f.read(2))[0]==0x8664,name
  f.seek(offset+24);assert struct.unpack('<H',f.read(2))[0]==0x20b,name
  architectures[name]='PE32+ AMD64 (0x8664)'
for entry in report['files']:
 p=Path(entry['path']);assert p.is_relative_to(out),str(p)
 assert p.is_file() and p.stat().st_size==entry['sizeBytes'],str(p)
assemblies=json.loads((out/'LayarTerakhir_Data/ScriptingAssemblies.json').read_text())['names']
assert 'Assembly-CSharp.dll' in assemblies and 'Assembly-CSharp-Editor.dll' not in assemblies
for name in assemblies:assert (out/'LayarTerakhir_Data/Managed'/name).is_file(),name
assert b'DevelopmentScreenshotCapture' in (out/'LayarTerakhir_Data/Managed/Assembly-CSharp.dll').read_bytes(),'Development screenshot utility is missing'
scene_data=(out/'LayarTerakhir_Data/level0').read_bytes()
for marker in ['Kos_Environment_Art','Kos_V2_Details','KosDeskFan','KosCurtains','CeramicTileFloor','WeakTungstenCeiling','CalendarAugust','vintage_day_bed','GoldenHUD','GoldenAudioRig','Golden_Dressing','BedsideWornStool','Compact_Kos_Details','StreamerGreenScreen','BudgetStreamingWebcam','ReadingPadOnStool','BedsidePlasticThermos','WiredGamepadOnBed']:assert marker.encode() in scene_data,marker
assert b'Prolog_KamarKos' in (out/'LayarTerakhir_Data/globalgamemanagers').read_bytes()
atmaja_data=(out/'LayarTerakhir_Data/level1').read_bytes()
for marker in ['Atmaja_Environment_Art','DamarParkedScooter','FallenKamboja','KitchenVaultOutside','LongDiningTable','GrandfatherClock','AR_SeatedSriAtmaja','ChildBedroomDoor','VentPassGate','DamarHandycam','Spawn_Basement','Spawn_Chase','DamarSurveyInsideDining','EmptyARScareChair','DesignerReviewEnvironment','WellCover','ExpandedCellarEastWall']:
 assert marker.encode() in atmaja_data,marker
if 'prolog' in args.source_record.lower():
 for marker in ['PrologCompletionArt','DamarRecordedMirrorSet','CompletionCellarExit','CompletionTableLanding','ScratchedFamilyPhotograph1']:
  assert marker.encode() in atmaja_data,marker
assert b'SeatedDollOnEndChair' not in atmaja_data,'The AR scare chair must be empty in normal sight'
assert b'Prolog_RumahAtmaja' in (out/'LayarTerakhir_Data/globalgamemanagers').read_bytes()
corridor_data=(out/'LayarTerakhir_Data/level2').read_bytes()
for marker in ['Spawn_KosFloor2','StairA_9','StairB_9','DAPUR BERSAMA','KAMAR MANDI','RakaMotor','RakaMotorHeadlight','DoubleLeafIronGate','KosStreetDeparture','KosMainEntranceDoor','StairwellFoundation','UpperLandingGuard','StairInnerGuardA_9','RoomCell_F2_21','KosPitchedRoof','SharedDiningDoor','BathroomWANITADoor','BathroomPRIADoor','KosFrontWindowGlass','KampungMelati','WarungSalesCounter','MelatiContinuousRain','MelatiAtmajaSign','BuSriShopkeeper']:
 assert marker.encode() in corridor_data,marker
for blocker in ['MelatiDistrictBoundary','MelatiStreetEnd','MelatiCrossroadEnd','StreetEndBoundary','KosGateBoundary']:
 assert blocker.encode() not in corridor_data,'Removed pedestrian barrier still packaged: '+blocker
runtime=(out/'LayarTerakhir_Data/Managed/Assembly-CSharp.dll').read_bytes()
for marker in ['GameShell','PhoneDesktop','PhoneSkin','KosRoomCorrections','ClockPuzzleView','ClockAmbience','KosCorridorDirector','KosTravelItem','ReviewPlayerAudit','ShoulderCameraRig','ExplorationCommentary','StoryMapGraphic','DoubleLeafGate','TryPayFamilyBill','DisplayPreferences','AtmajaUpperDirector','AtmajaBasementHorror','AtmajaChaseSequence','AtmajaReviewMoments','ARPhotoGlimmer','MelatiNeighborhood','MelatiShop','NeighborhoodRain','MotorcycleRiderAlignment','ArchiveDocument']:
 assert marker.encode() in runtime,marker
if 'presentation' in args.source_record.lower():
 for marker in ['PlayerPerspective','TogglePerspective','AnimatedRakaAvatar']:
  assert marker.encode() in runtime,marker
 assert b'RakaCharacter' in (out/'LayarTerakhir_Data/resources.assets').read_bytes(),'Skinned Raka prefab missing from resources'
if 'prolog' in args.source_record.lower():
 for marker in ['PrologFinaleView','AtmajaHandycamPlayback','AtmajaMirrorReflection','AtmajaThreatPresentation','PrologCompletionAudit']:
  assert marker.encode() in runtime,marker
assert b'LayarPresentation' in (out/'LayarTerakhir_Data/resources.assets').read_bytes(),'Presentation/voice resource missing'
shutil.copy2(root/'Audit/asset-credits.md',out/'Asset-Credits.txt')
shutil.copy2(root/'Audit/windows-kos-capture.md',out/'Screenshot-Guide.txt')
shutil.copy2(root/'Assets/_LayarTerakhir/UI/Fonts/OFL.txt',out/'Barlow-OFL.txt')
shutil.copy2(root/'Audit/golden-windows-review.md',out/'Golden-Review-Guide.txt')
shutil.copy2(root/'Audit/atmaja-windows-playtest.md',out/'Atmaja-Playtest-Guide.txt')
shutil.copy2(root/'Audit/Recording/dialogue-recording.csv',out/'Dialogue-For-Recording.csv')
shutil.copy2(root/'Audit/Recording/README.md',out/'Dialogue-Recording-Readme.txt')
shutil.copy2(root/'Audit/dejavu-font-license.txt',out/'DejaVu-Font-License.txt')
if (root/'Audit/makehuman-assets-CC0.txt').exists():shutil.copy2(root/'Audit/makehuman-assets-CC0.txt',out/'MakeHuman-Graphical-Assets-CC0.txt')
shutil.copy2(root/'Audit/kenney-suburban-CC0.txt',out/'Kenney-Suburban-CC0.txt')
files=sorted(p for p in out.rglob('*') if p.is_file());manifest=[]
for p in files:
 digest=hashlib.sha256();crc=0
 with p.open('rb') as f:
  for block in iter(lambda:f.read(1024*1024),b''):digest.update(block);crc=zlib.crc32(block,crc)
 manifest.append({'path':p.relative_to(out).as_posix(),'sizeBytes':p.stat().st_size,'sha256':digest.hexdigest(),'crc32':crc&0xffffffff})
archive=root/'Builds/LayarTerakhir-Windows-x86_64-Development.zip';partial=archive.with_suffix('.zip.partial')
assert not archive.exists() and not partial.exists(),'Preserve the existing archive before packaging a new version'
with zipfile.ZipFile(partial,'x',compression=zipfile.ZIP_DEFLATED,compresslevel=6) as z:
 for p in files:z.write(p,Path('Windows')/p.relative_to(out))
with zipfile.ZipFile(partial) as z:
 assert z.testzip() is None,'ZIP CRC check failed'
 assert set(z.namelist())=={'Windows/'+f['path'] for f in manifest}
 for f in manifest:
  info=z.getinfo('Windows/'+f['path']);assert info.file_size==f['sizeBytes'] and info.CRC==f['crc32'],f['path']
partial.rename(archive)
with archive.open('rb') as f:archive_sha=hashlib.file_digest(f,'sha256').hexdigest()
archive.with_suffix('.zip.sha256').write_text(f'{archive_sha}  {archive.name}\n')
(logs/'windows-build-files.sha256').write_text(''.join(f"{f['sha256']}  {f['path']}\n" for f in manifest))
verification={'verifiedUtc':datetime.now(timezone.utc).isoformat(),'unityExitCode':invocation['exitCode'],
 'result':report['result'],'target':report['target'],'scriptingBackend':report['scriptingBackend'],'development':True,
 'startupScene':report['scenes'][0],'executable':str(out/'LayarTerakhir.exe'),'nativeArchitectures':architectures,
 'unityReportedFilesVerified':len(report['files']),'scriptingAssembliesVerified':len(assemblies),
 'unityReportedSizeBytes':report['totalSizeBytes'],'buildFileCount':len(files),
 'totalBuildSizeBytes':sum(f['sizeBytes'] for f in manifest),'archivePath':str(archive),
 'archiveSizeBytes':archive.stat().st_size,'archiveSha256':archive_sha,'zipCrcVerified':True,'zipContainsAllBuildFiles':True,
 'windowsRuntimeTested':False,'files':manifest}
verification['sourceSceneSha256']=hashlib.sha256((root/'Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity').read_bytes()).hexdigest()
verification['atmajaSceneSha256']=hashlib.sha256((root/'Assets/_LayarTerakhir/Scenes/Prolog_RumahAtmaja.unity').read_bytes()).hexdigest()
assert verification['sourceSceneSha256']==json.loads((root/'Audit/atmaja-art-manifest.json').read_text())['protectedGoldenSceneSha256'],'Golden source scene changed'
verification['developmentScreenshotUtilityIncluded']=True
verification['includedScenes']=report['scenes']
(logs/'windows-build-verification.json').write_text(json.dumps(verification,indent=2)+'\n')
summary={k:v for k,v in verification.items() if k not in ['files','nativeArchitectures']}
(root/'Audit/golden-build-summary.json').write_text(json.dumps(summary,indent=2)+'\n')
(root/'Audit/atmaja-build-summary.json').write_text(json.dumps(summary,indent=2)+'\n')
shutil.copy2(logs/'windows-build-invocation.json',root/'Audit/golden-windows-invocation.json')
shutil.copy2(logs/'windows-build-invocation.json',root/'Audit/atmaja-windows-invocation.json')
print(json.dumps(summary,indent=2))
