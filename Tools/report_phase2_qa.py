"""Publish all 100 specification cases with bounded, evidence-based coverage."""
import json,re,xml.etree.ElementTree as E
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
for name in ['full-route','no-secret-route','boss-recovery','investigation-edges','safety','zero-victory']:
 data=json.loads((ROOT/'Audit'/('phase2-session2-'+name+'.json')).read_text());assert data.get('completed') is True,name
unit=E.parse(ROOT/'Audit/phase2-session2-editmode.xml').getroot();assert unit.get('failed')=='0' and unit.get('result')=='Passed'

spec=(ROOT/'Phase2.MD').read_text();section=spec[spec.index('### 10.1 QA wajib'):spec.index('### 10.3 Definition')]
rows={}
for line in section.splitlines():
 cells=[x.strip() for x in line.strip().split('|')[1:-1]]
 if cells and re.fullmatch(r'QA-\d{2,3}',cells[0]):rows[int(cells[0][3:])]=cells
assert set(rows)==set(range(1,101)),len(rows)
# A test-case count is deliberately not used as a substitute for 100 design cases.
notes={
1:'Dua jalur HUB→CP7 tanpa uang/belanja, tanpa live manual; satu dengan arsip dan satu tanpa arsip.',
2:'Log/papan/sensor serta catatan suster/papan04 dipakai pada jalur yang tidak membuka live.',
3:'Validator set/receipt dan scan jalur diuji; tiga pengulangan masing-masing sensor/foto belum dimainkan semua.',
4:'EVP, pickup Damar dan reload investigasi terpisah pada jalur penuh.',
5:'Foto aura mendahului pickup Gunting, dengan assertion tidak memberi pusaka silang; transaksi menyimpan ID terpisah.',
6:'Gate unit menolak bukti pengganti, pusaka kurang dan Jurnal absen; konfirmasi eksplisit pada kedua jalur.',
7:'E meja aktual menulis CP5 lalu write barrier; gate negatif dan jangkauan diuji.',
8:'Blitz shielded berbiaya15 tidak memberi hit atau menghabiskan lampu pada native recovery.',
9:'Rear miss, timeout lima detik dan retry mempertahankan sumber pada native recovery.',
10:'Klik kedua pada cooldown tidak memberi hit/debit tambahan; sukses memindah state dari EXPOSED.',
11:'Gate persiapan negatif unit dan kabut/door derived; jalur masuk aktual dengan tiga pusaka.',
12:'Validasi unique ID dan rollback secret unit; pickup D1 aktual sekali per jalur, belum dua E pada objek yang sama.',
13:'Secret melewati reload investigasi dan CP7; seluruh variasi kematian sesudah pickup belum dimainkan.',
14:'Jalur tanpa arsip kembali secara fisik ke D1 setelah CP6 pada baterai0.',
15:'CP6/CP7 atomik, reload disk dan reload scene; saldo/Sanity/inventori/segel tetap konsisten.',
16:'Edge fixture menguji E cancel, B2 -18, gas6s tanpa fatal/debit dan gerak×0.8.',
17:'Recovery IGD dan arena aktual; variasi baterai rendah di setiap sensor eksplorasi belum dimainkan semua.',
18:'Unit belanja paket unik/debit/receipt/reload; process kill persis saat write belum diinjeksikan.',
19:'Keypad1603 aktual dan receipt idempoten; spam kode berulang belum dimainkan pada semua timing UI.',
20:'Edge fixture menguji menoleh nonfatal saat learning dan berakhir lebih awal sesudah hold; menunggu meter penuh selama seluruh30s belum dijalankan.',
21:'Hold dari perpindahan kapsul, berhenti dan keluar Selfie tetap menyimpan akumulasi; detach aktual.',
22:'Safety menguji hide+layar0 enam detik dan E exit; menyalakan layar di tengah loiter belum diuji.',
23:'E actual pada kunci/laci/sakelar dengan ponsel; bukan bukti semua pose/remap/perspektif.',
24:'Source satu grace per attach dan modal reject tersedia; spam UI lengkap belum dimainkan.',
25:'GRAB/Continue CP3 dan reload CP4 jalur nyata; kombinasi kematian sebelum/sesudah setiap pickup belum semuanya.',
26:'Memerlukan profil Windows/GPU; Null renderer dan llvmpipe bukan acceptance FPS.',
27:'Panel memakai irisan lima ID kanon; kombinasi P1 secret dan ID asing belum seluruhnya di-run ulang pada P2.',
28:'Receipt ekonomi/segel atomik unit, video replay dan panel berulang native.',
29:'Pause EXPOSED mempertahankan window, energi dan posisi agent; recovery clock juga memakai clock yang sama.',
30:'Video replay sesudah epilog diuji tanpa reward ganda; reload arsip teks dan perekam terpisah.',
31:'IGD normal/emergency crank dari0 diuji pada safety; setiap charger eksplorasi belum dimainkan dari0.',
32:'Power recovery TRACK, miss dan generator repeatable aktual; zero-CP6 terpisah pada zero-victory fixture.',
33:'20 rollback snapshot unit dan satu reset lampu/flare native; bukan20 playthrough mati boss penuh.',
34:'Poli stabil aktual serta target/range scene; lintasan sekilas0.1s belum diujikan terpisah.',
35:'Rantai bergantung receipt dwell Thermo; jalur aktual; seluruh kombinasi AR-only/Thermo-only belum dijalankan.',
36:'Range/cone source dan target fisik; sweeping saat berjalan di luar batas belum lengkap.',
37:'Gain exterior/telegraph terpisah source; perlu penerimaan pendengaran dan repeated traversal.',
38:'Puzzle1603 dijalankan tanpa chat/live; clue tersimpan dan kode bukan label pintu.',
39:'2604 dibuka melalui deduksi suster sebelum Inkubator; operasi pada0 dan semua kode salah belum dimainkan.',
40:'Kunci terpisah dari C3 diuji unit/referensi; jalur naik sebelum C3 belum end-to-end.',
41:'Sanity0/23 tetap pada unit/transaksi dan jalur lengkap; tidak ada drain P2.',
42:'Kemenyan tetap dalam snapshot/CP6 dan flare hanya TRACK; keyboardG di luar boss belum seluruh konteks.',
43:'164.8M hanya overlay; bound donation, wallet0, belanja dan replay tidak bercampur pada tes.',
44:'Migrasi1/2→3 dan future schema diuji; semua byte-corruption/backup dan entry UI belum dieksplorasi.',
45:'Kapsul bergerak melalui collision nyata, hold kumulatif; bukan semua crouch/akselerasi pada tiap lintasan.',
46:'Native normal GRAB diukur sekitar10s meter+2s rambut setelah attach; telegraph terpisah.',
47:'Crossing AR permanen, pulang pada0 dan reload jalur aktual.',
48:'Belajar/nonfatal dan GRAB diuji; sweep yaw50°/0.25s OTS/FP/crouch belum lengkap.',
49:'Frustum9-anchor/LOS source dan render nyata; seluruh kombinasi occlusion singkat/tembok belum digerakkan.',
50:'Interval45s memakai MorgueCleared; belum diukur dengan run45s tersendiri.',
51:'Edge fixture L2 flicker/padam dan quiet-zone0; kenyamanan navigasi manual masih perlu review.',
52:'EVP04 enam detik valid dijalankan; interruptC/range/salah inkubator belum semua variasi.',
53:'20 snapshot rollback mencakup perubahan RAM, write barrier dan disk CP5; force-kill native pada hit1/2 belum dijalankan.',
54:'Disk-write failure CP6 nyata di unit, rollback dan retry satu victory tanpa half-seal.',
55:'Native reticle memilih asli-20/ilusi-6; lampu hanya ilusi/cooldown12; pop-respawn6s belum disampling lengkap.',
56:'Blitz Selfie dan rear sesudah berbalik sukses pada jalur; rear menghadap menjauh gagal pada recovery.',
57:'Membutuhkan playtest manusia bertanda waktu; perjalanan otomatis tidak membuktikan82menit.',
58:'Transaksi Gunting dan foto terpisah; jalur foto-dulu dan reload; death tepat antara keduanya belum native.',
59:'Unit carry-over0/1/14/67/100 dan charge sekali; 19/85 serta variasi UI fisik belum seluruhnya.',
60:'Native sepuluh CP3 load/restore mempertahankan baterai snapshot; unit20 retry boss.',
61:'Safety HP0+hide6s+E keluar berhasil; tidak ada WATCH_ONLY.',
62:'Pulang L2→basement/L1/IGD/motor dengan0 tanpa AI/gas setelah kemenangan.',
63:'Interaksi ray aktual dan LOS source/unit; seluruh pickup dari balik tembok belum dimainkan.',
64:'Urutan tick fatal sebelum interactor ada; boundary E pada tepat deadline belum diinjeksikan.',
65:'Native far-table ditolak dan E dekat menulis CP5.',
66:'Gate tiga pusaka/unit dan pintu CP6 terbuka walau inventori pusaka pindah ke meja.',
67:'Perlu tiga menit navigasi manusia untuk penerimaan area/clue, bukan timer otomatis.',
68:'Kapsul benar-benar turun tangga dan mencapai motor; backtrack basement pada0.',
69:'Tiga pembatalan panel tidak menyimpan COMPLETE; reload akhir terpisah.',
70:'Unit CP7 I/O failure/retry; native panel/fade/posisi fault injection pada zero-victory/exit-fault run.',
71:'Bukaan fisik dua arah tetap saat baterai0, termasuk setelah reload.',
72:'Safety membuktikan crank20+26s akumulatif; attach sesudah tepat30s di DUTY belum diuji.',
73:'Emergency crank cap10 dan cooldown120 ditolak pada safety; semua charger/cooldown selesai belum diuji.',
74:'D1 dibuka kunci tanpa scan pada jalur; source17/16/17°C dan AR17; semua D2-D4 belum diperiksa manual.',
75:'Jejak Damar ada sebagai interaksi non-gating; belum diverifikasi reward-free pada directed run.',
76:'Fixture dev-only opt-in/save terisolasi dan prasyarat produksi ditolak unit.',
77:'Kos existing dipakai tanpa resize, Prolog guard actual pada dua run.',
78:'Safety RMB/X/F/Tab; remap, semua app/filter/perspektif dan release-focus matrix belum lengkap.',
79:'Butuh playtest binding ulang; belum diuji end-to-end oleh native input.',
80:'Pause engine diuji; Alt-tab Windows saat tiap state butuh Windows nyata.',
81:'Drain/cost global dan pause diuji; seluruh kombinasi overlay/hard-modal belum diprofil numerik.',
82:'Unit menolak auto-charge carry-over, native departure aktual; semua9 boundary investigasi belum dimainkan.',
83:'9 batas snapshot CP5 unit; recovery0 actual repeatable. Bukan9 pertarungan lengkap setelah miss hingga habis.',
84:'Crank akumulatif berhenti saat attached source/safety sebagian; fixture30/45 di DUTY belum dijalankan.',
85:'Satu handset/input pemilik existing; spam semua tombol saat attached belum lengkap.',
86:'Collision lensa seluruh segmen dan quiet-zone0 tersedia; inspeksi semua sudut OTS/FP dekat tembok masih diperlukan.',
87:'Migrasi nested, schema future, data valid ditolak/diterima sesuai unit; backup korup lengkap belum dimainkan.',
88:'Receipt dan ID set unit; semua sensor dua kali sesudah Continue belum diputar satu per satu.',
89:'Unit transaksi shop/charge rollback dan reload; force-quit tiap timing write belum native.',
90:'Hub14:00/depart23:45/IGD23:50 source/provider dan live idempoten; kedua run tanpa live sampai epilog.',
91:'Teks kode/suhu/usia/korban/akhir kanon diselaraskan; VO manusia belum tersedia.',
92:'Unit radius/jangkauan/LOS dan cahaya kepala; ketiga lampu benar-benar dipakai pada kedua jalur.',
93:'Shielded/away miss berbiaya15/6, ilusi bersih saat exposure, satu hit per siklus.',
94:'Write barrier dan CP6 failure/rollback unit + scene restore; proses-kill saat filesystem replace belum dilakukan.',
95:'Zero-victory native memastikan0%CP6 tetap tenang, generator fisik dan epilog, pulang0; D1 melalui run tanpa arsip.',
96:'Panel dibatalkan3× serta native fault CP7 tanpa fade/shutter, kemudian retry berhasil.',
97:'Panel global memakai irisan ID; matriks empat kombinasi P1/P2 secret belum semuanya dimainkan.',
98:'49 model path baru, URL/hash/lisensi dan builder/scene placement nyata; penerimaan artistik dan acting manusia masih perlu desainer.',
99:'Windows/GPU/IrisXe8GB dan1%low belum tersedia; tidak menganggap render software sebagai hasil sasaran.',
100:'Dua run penuh tanpa gate belanja/live; arsip ada/tidak; pacing82menit tetap menunggu playtest manusia.'}
# Only narrow completed mechanics receive directed-pass labels; combined/manual cases remain partial.
directed={1,2,4,5,6,7,8,9,10,15,16,21,28,29,41,43,46,47,54,56,60,61,62,65,66,68,69,70,71,76,77,92,93,95,96}
windows={26,80,99};manual={57,67,79}
proof='[unit](phase2-session2-editmode.xml), [jalur utama](phase2-session2-full-route.json), [tanpa arsip](phase2-session2-no-secret-route.json), [recovery](phase2-session2-boss-recovery.json), [edge investigasi](phase2-session2-investigation-edges.json), [safety](phase2-session2-safety.json), [zero victory/fault](phase2-session2-zero-victory.json)'
lines=['# Phase 2 — matriks seluruh 100 QA','','Cakupan setiap ID dipertahankan. **LULUS TERARAH** berarti bagian mekanik yang disebut benar-benar diuji; bukan sertifikasi manual Windows atau acceptance kualitas. **SEBAGIAN** berarti skenario lengkap belum terbukti. Semua kasus gabungan mempertahankan batas bukti; jumlah tes Unity tidak diubah menjadi klaim100/100.','','Bukti: '+proof+'.','','Null renderer memakai simulasi2× dan kapsul collision nyata, dengan fixture eksplisit untuk kasus tepi. Render software Linux1× terpisah dari bukti logika dan bukan benchmark GPU Windows. Input virtual/poses fixture bukan playtest manusia.','','| ID | Status | Skenario | Bukti dan batas yang tersisa |','|---|---|---|---|']
counts={}
for n in range(1,101):
 status='BUTUH WINDOWS' if n in windows else 'BUTUH PLAYTEST' if n in manual else 'LULUS TERARAH' if n in directed else 'SEBAGIAN'
 counts[status]=counts.get(status,0)+1
 title=rows[n][-2]
 lines.append(f'| QA-{n:02} | {status} | {title} | {notes[n]} |')
lines+=['','## Gate yang belum terpenuhi','','- Acceptance seluruh100 skenario, playtest Windows pada GPU sasaran, performa60FPS/1%low dan pacing82menit belum terbukti.','- Rekaman VO manusia sesuai dialog Indonesia belum tersedia; teks kanon dan cue nonverbal berlisensi tersedia.','- Visual/acting, variasi occlusion, remapping dan stress input perlu penerimaan manusia; tidak diklaim setara DreadOut/Outlast.','','Rekap: '+json.dumps(counts,ensure_ascii=False)+'.']
(ROOT/'Audit/PHASE2_QA_100.md').write_text('\n'.join(lines)+'\n')
print('QA_MATRIX_WRITTEN',len(rows),counts)
