"""Acquire new Pasar Setan art. Reuse download tooling, never previous art.

Count distinct original model IDs only; placement is verified independently.
Originals are cached outside Unity. No modeled/generated art is made here.
"""
import concurrent.futures
import json
import re
import subprocess
from pathlib import Path
import fetch_phase4_assets as source

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/Phase5'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase5/Imported'
# Authored objects grouped by the actual dramatic function, not arbitrary filler.
GROUPS = {
 'GateMarket': '''Barrel_01 Barrel_02 barrel_03 CashRegister_01 CheeseBox_01 CoffeeCart_01
 CoffeeTable_01 Megaphone_01 bar_chair_round_01 brass_blowtorch can_rusted
 carved_wooden_plate chinese_armchair chinese_cabinet chinese_chandelier
 chinese_commode chinese_console_table chinese_screen_panels chinese_sofa
 cigarette_case cigarette_pack dining_table dirty_football fish_knife fishermans_hat
 food_kiwi_01 gallinera_table gate_latch_01 garden_gloves_01 lambis_shell lemon
 lightbulb_01 long_life_food old_tyre outdoor_table_chair_set_01 plastic_crate_01
 plastic_crate_03 plastic_jerrycan modified_thermos pocket_watch postcard_set_01 propane_tank
 rubber_duck_toy russian_food_cans_01 standing_chalkboard_01 street_lamp_01
 street_lamp_02 vintage_suitcase wine_barrel_01 wine_bottles_01 wooden_picnic_table''',
 'FrozenCorridor': '''BarberShopChair_01 GothicBed_01 SchoolDesk_01 WetFloorSign_01
 all_purpose_cleaner bleach_bottle clipboard drain_cleaner exterior_aircon_unit
 industrial_microscope korean_fire_extinguisher_01 multi_cleaner_5_litre
 multi_cleaner_bottle vintage_crutches_01 vintage_microscope''',
 'BambooBridge': '''crowbar_01 handsaw_wood industrial_pipe_lamp industrial_wall_sconce
 modular_airduct_circular_01 modular_airduct_rectangular_01 modular_chainlink_fence
 modular_electric_cables modular_electricity_poles modular_metal_gutter modular_pipes
 modular_pipes_plastic_01 old_drill_press ratchet_wrench rusted_hacksaw
 sledgehammer_01 tire_pump tongue_groove_pliers utility_box_02''',
 'SewingMirror': '''ArmChair_01 Chandelier_02 Chandelier_03 ClassicConsole_01
 ClassicNightstand_01 GothicCabinet_01 GothicCommode_01 Ottoman_01 Shelf_01
 Sofa_01 alarm_clock_01 boombox coffee_table_round_01 drawer_cabinet
 gothic_coffee_table hand_plane_no4 mantel_clock_01 magnifying_glass_01
 side_table_01 side_table_tall_01 sofa_02 sofa_03 vintage_grandfather_clock_01
 vintage_stapler vintage_telephone_wall_clock''',
 'AuctionStage': '''Camera_01 Television_01 Television_02 circuit_board classic_laptop
 filmstrip_projector_8mm security_camera_01 security_camera_02 projector_screen
 wooden_military_crate steel_frame_shelves_03 lightbulb_led signal_flashlight
 vintage_binocular vintage_flashlight modular_fort_01 modular_urban_apartments_facade''',
 'BorderNature': '''bark_debris_01 boulder_01 dead_quiver_branch_01 dead_quiver_branch_02
 dead_quiver_trunk island_tree_01 island_tree_02 island_tree_03 tree_small_02
 tree_stump_01 tree_stump_02 namaqualand_rocks_01''',
}
# These surfaces are also new; never count them towards the 100 models.
SURFACES = ['brown_mud_03', 'concrete_wall_007', 'wooden_rough_planks', 'rusty_metal_04', 'fabric_pattern_07']
HEROES = [
 ('22d13b14e0a4455db9e4fb37cda960cc','FerrisWheel'),
 ('6bb15dc7056241bfac6ee747fc866e45','MarketStall'),
 ('0d0e00e5735440efa86b5da2ea8498b1','SewingMachine'),
 ('4862c3f975244d1c98dba46c4ff2df4c','StoreMannequin'),
 ('63b3a3b35e9242d2b914064af26bf7fe','ViewerHead'),
 ('1091b3ee172f4951aaeafa2581f9de1d','MemoryGhost'),
 ('630b7d4aa7554d5d928e3bd35236f968','ScreenRobe'),
 ('4d1727c7b83e4e6284bbadb63dbb537e','WitnessHands'),
 ('9493e828520c490d85f093a2b5baddbb','BambooMemory'),
 ('1abac8cfc34e45748710f5e479c668a0','JungleMemory'),
 ('78ac8daa3393405792de9c71218ce88d','AntiqueMirror'),
 ('e6edcb46771b4fd1b068c86359b18a43','AtmajaEpilogueHouse'),
 ('d225389de8c24d1c9cf7102c740eccf9','MarketGenerator'),
 ('efa33538faad46e09db685473e6b52f6','StageLantern'),
 ('682af9b4e83b4bcbba145b2596ba2abe','DamarFinale'),
 ('a74239ec0dfe454a906c339071855d87','MorgueStretcher'),
]

def ph(name):
 # The supplied ARM already includes AO/roughness/metalness. Do not request
 # redundant, occasionally missing standalone channels from the mirror.
 meta_path=CACHE/'Metadata'/(name+'.json')
 source.get('https://api.polyhaven.com/files/'+name,meta_path)
 metadata=json.loads(meta_path.read_text());folder=CACHE/'PolyHaven'/name;files=[]
 if name in source.MODELS:
  resolutions=metadata['fbx'];resolution='2k' if '2k' in resolutions else next(iter(resolutions));entry=resolutions[resolution]['fbx']
  target=folder/(name+'.fbx');sha=source.get(entry['url'],target);files.append(dict(role='source-model',path=str(target.relative_to(ROOT)),url=entry['url'],sha256=sha))
 for key,resolutions in metadata.items():
  if not any(key.lower()==suffix or key.lower().endswith('_'+suffix) for suffix in ['diff','diffuse','nor_gl','arm','alpha']):continue
  if '2k' not in resolutions:continue
  formats=resolutions['2k'];ext='jpg' if 'jpg' in formats else 'png' if 'png' in formats else None
  if not ext:continue
  entry=formats[ext];target=folder/Path(entry['url']).name;sha=source.get(entry['url'],target)
  destination=OUT/'Textures'/name/target.name;destination.parent.mkdir(parents=True,exist_ok=True)
  import shutil
  shutil.copy2(target,destination)
  files.append(dict(role='texture',channel=key,path=str(destination.relative_to(ROOT)),sourcePath=str(target.relative_to(ROOT)),url=entry['url'],sha256=sha))
 print('PHASE5_FETCHED',name,len(files),flush=True)
 return dict(id=name,kind='model' if name in source.MODELS else 'surface',sourcePage='https://polyhaven.com/a/'+name,license='CC0-1.0',licenseUrl='https://polyhaven.com/license',providerCredit='Powered by Poly Haven',files=files)

def main():
 CACHE.mkdir(parents=True, exist_ok=True)
 catalog_path=CACHE/'polyhaven-models.json'
 source.get('https://api.polyhaven.com/assets?t=models',catalog_path)
 catalog=json.loads(catalog_path.read_text());old=set()
 for manifest in (ROOT/'Audit').glob('*asset-sources.json'):
  if manifest.name.startswith('phase5-'):continue
  old.update(n.lower() for n in re.findall(r'https://polyhaven.com/a/([A-Za-z0-9_-]+)',manifest.read_text()))
 for n in subprocess.check_output(['git','ls-files','Assets/_LayarTerakhir/assets/Art'],cwd=ROOT,text=True).splitlines():
  if '/Phase5/' not in n and n.endswith(('.fbx','.glb','.gltf')):old.add(Path(n).stem.lower().removesuffix('_2k').removesuffix('_1k'))
 (CACHE/'previous-ph-identities.json').write_text(json.dumps(sorted(old)))
 models={}
 for group, words in GROUPS.items():
  for name in words.split():
   if name not in catalog or name.lower() in old:
    print('PHASE5_SKIPPED_NOT_NEW', name, flush=True); continue
   models[name]=group
 assert len(models)>=100, len(models)
 source.CACHE,source.OUT,source.MODELS,source.SURFACES=CACHE,OUT,list(models),SURFACES
 records=[];failures=[]
 with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
  jobs={pool.submit(ph,n):n for n in list(models)+SURFACES}
  for job in concurrent.futures.as_completed(jobs):
   n=jobs[job]
   try:
    row=job.result();row['function']=models.get(n,'PBR surface');row['author']=catalog.get(n,{}).get('authors',{})
    records.append(row)
   except Exception as e:
    failures.append(dict(id=n,error=str(e))); print('PHASE5_FETCH_FAILED',n,str(e),flush=True)
 for uid,name in HEROES:
  try: records.append(source.objaverse(uid,name))
  except Exception as e: failures.append(dict(id=name,error=str(e)))
 report=dict(sources=sorted(records,key=lambda r:r['id']),failures=failures,uniqueNewModelSources=sum(r['kind']=='model' for r in records),
   policy='New licensed Phase5 models, identity checked against previous source metadata and tracked filenames. Actual placement required; shared campaign player/handset identity excluded.')
 (ROOT/'Audit/phase5-asset-sources.json').write_text(json.dumps(report,indent=2)+'\n')
 print('PHASE5_FETCH_COMPLETE',report['uniqueNewModelSources'],'models',len(failures),'failures',flush=True)
 if sum(r['kind']=='model' for r in records)<100: raise SystemExit(1)

if __name__=='__main__': main()
