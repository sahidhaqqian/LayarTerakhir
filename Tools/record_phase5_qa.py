"""Record evidence without upgrading unit/render support into manual acceptance."""
import json,re,xml.etree.ElementTree as ET
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
rows=[]
for line in (ROOT/'Phase5.MD').read_text().splitlines():
 if re.match(r'\| QA-\d{3} \|',line):
  p=[p.strip() for p in line.strip('|').split('|')]
  rows.append(dict(id=p[0],scenario=p[1],expected=' | '.join(p[2:]),acceptance='MANUAL_PENDING',support=[]))
assert len(rows)==180 and len({r['id'] for r in rows})==180
artifacts=[]
for name in ['phase5-campaign-tests.xml','phase5-scene-tests.xml','phase5-regression-tests.xml','phase5-focused-tests.xml']:
 path=ROOT/'Audit'/name
 if not path.exists():continue
 run=ET.parse(path).getroot();artifacts.append(dict(path='Audit/'+name,result=run.get('result'),total=run.get('total'),passed=run.get('passed'),failed=run.get('failed')))
for name in ['phase5-native-route.json','phase5-native-visual.json',*[f'phase5-ending-{s.lower()}.json' for s in ['BAD','NORMAL','TRUE','SECRET']],'phase5-crash-probes.json']:
 path=ROOT/'Audit'/name
 if path.exists():
  data=json.loads(path.read_text());artifacts.append(dict(path='Audit/'+name,result=data.get('completed',data.get('passed')),method=data.get('method','Explicit isolated process fault probes')))
for row in rows:
 text=(row['scenario']+' '+row['expected']).lower()
 if any(w in text for w in ['save','reload','cp7','cp8','cp9','ending','donasi','saldo','duplik','arsip','baterai 0','sanity 0','migrasi','checkpoint']):row['support'].append('Audit/phase5-campaign-tests.xml (transaction/loader scope only)')
 if any(w in text for w in ['navmesh','lantai','collider','material','100 aset','100 model','sumber','plank','papan runtuh']):row['support'].append('Audit/phase5-scene-tests.xml (serialized/physics scope only)')
text='# Phase 5 — 180 QA dan bukti tambahan\n\nStatus penerimaan manusia tidak disamakan dengan hasil unit, scene, posed render atau driver native. Semua baris tetap **MANUAL_PENDING** sampai skenario dimainkan dan dinilai sebagaimana tertulis. Kolom dukungan hanya menunjuk ruang lingkup otomasi yang relevan, bukan PASS keseluruhan skenario.\n\n## Artefak yang tersedia\n\n'
for a in artifacts:text+='- `'+a['path']+'`: '+str(a)+'\n'
text+='\n## Daftar 180 QA\n\n| ID | Skenario | Penerimaan | Dukungan otomatis parsial |\n|---|---|---|---|\n'
for row in rows:text+='| '+row['id']+' | '+row['scenario'].replace('|',' / ')+' | '+row['acceptance']+' | '+'; '.join(row['support'])+' |\n'
text+='''
## Tambahan wajib desainer

| Kasus | Bukti dan ruang lingkup |
|---|---|
| Baterai0, Sanity0, saldo0, tanpa consumable | Campaign tests memeriksa carry-over/recovery/cost; native route memeriksa input dan akses fisik bila artefaknya completed=true. |
| Crash inti boss dan crash lock ending | Fault probe benar-benar menghentikan proses player miliknya pada temp-flushed/replaced; verifikasi loader di proses baru dicatat terpisah. Simulasi stale.tmp unit bukan pengganti probe ini. |
| Double-click / dua ending | Campaign tests duplicate shot/lock tidak mengurangi ulang, ending pertama menolak ending berikutnya; driver menguji input prematur/lock. |
| Lima ID Arsip unik vs kurang | Campaign tests identitas canonical, penolakan lima ID sembarang, validasi tidak memilih SECRET. |
| Reload inti1, inti2, kabel | Campaign tests setiap receipt CP7; native route mencatat reload scene nyata bila berhasil. |
| Continue PHASE4_CP8 | Fixture diambil dari save hasil native Phase4 aktual; driver menjalankan GameShell.Continue. |
| Regresi save Phase1–4 | Lihat hasil case per suite pada phase5-regression-tests.xml; gagal/skipped tidak disebut PASS. |
| CP8/CP9 dan kapasitor | Campaign tests save-failure/lock/cutscene completion; empat ending driver menggunakan fixture FINAL_CHOICE eksplisit dan bukan bukti investigasi penuh. |

VO manusia, kualitas artistik setara DreadOut, pacing75menit, aksesibilitas semua perangkat, dan FPS Windows/IrisXe memerlukan penerimaan di perangkat target. Linux software renderer tidak dipakai untuk klaim FPS Windows.
'''
(ROOT/'Audit/phase5-qa-matrix.md').write_text(text)
(ROOT/'Audit/phase5-qa-evidence.json').write_text(json.dumps(dict(specificationCases=180,manualAccepted=0,rows=rows,artifacts=artifacts),indent=2)+'\n')
print('Recorded180 specification cases; no false manual PASS')
