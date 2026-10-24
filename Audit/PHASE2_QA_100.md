# Phase 2 — matriks seluruh 100 QA

Cakupan setiap ID dipertahankan. **LULUS TERARAH** berarti bagian mekanik yang disebut benar-benar diuji; bukan sertifikasi manual Windows atau acceptance kualitas. **SEBAGIAN** berarti skenario lengkap belum terbukti. Semua kasus gabungan mempertahankan batas bukti; jumlah tes Unity tidak diubah menjadi klaim100/100.

Bukti: [unit](phase2-session2-editmode.xml), [jalur utama](phase2-session2-full-route.json), [tanpa arsip](phase2-session2-no-secret-route.json), [recovery](phase2-session2-boss-recovery.json), [edge investigasi](phase2-session2-investigation-edges.json), [safety](phase2-session2-safety.json), [zero victory/fault](phase2-session2-zero-victory.json).

Null renderer memakai simulasi2× dan kapsul collision nyata, dengan fixture eksplisit untuk kasus tepi. Render software Linux1× terpisah dari bukti logika dan bukan benchmark GPU Windows. Input virtual/poses fixture bukan playtest manusia.

| ID | Status | Skenario | Bukti dan batas yang tersisa |
|---|---|---|---|
| QA-01 | LULUS TERARAH | Main tanpa belanja dan tanpa saweran | Dua jalur HUB→CP7 tanpa uang/belanja, tanpa live manual; satu dengan arsip dan satu tanpa arsip. |
| QA-02 | LULUS TERARAH | Lewati semua chat petunjuk | Log/papan/sensor serta catatan suster/papan04 dipakai pada jalur yang tidak membuka live. |
| QA-03 | SEBAGIAN | Pindai Poli 03 tiga kali; foto aura tiga kali | Validator set/receipt dan scan jalur diuji; tiga pengulangan masing-masing sensor/foto belum dimainkan semua. |
| QA-04 | LULUS TERARAH | Rekam EVP Inkubator 04, ambil perekam Damar secara terpisah lalu load | EVP, pickup Damar dan reload investigasi terpisah pada jalur penuh. |
| QA-05 | LULUS TERARAH | Foto aura tanpa ambil Gunting; ambil Gunting tanpa foto aura | Foto aura mendahului pickup Gunting, dengan assertion tidak memberi pusaka silang; transaksi menyimpan ID terpisah. |
| QA-06 | LULUS TERARAH | Buka Jurnal dengan 0, 2, 3 bukti (termasuk duplikat) | Gate unit menolak bukti pengganti, pusaka kurang dan Jurnal absen; konfirmasi eksplisit pada kedua jalur. |
| QA-07 | LULUS TERARAH | Tiga bukti + konfirmasi + tiga pusaka + `E` di meja | E meja aktual menulis CP5 lalu write barrier; gate negatif dan jangkauan diuji. |
| QA-08 | LULUS TERARAH | Blitz saat perisai Kabut Sengkala utuh | Blitz shielded berbiaya15 tidak memberi hit atau menghabiskan lampu pada native recovery. |
| QA-09 | LULUS TERARAH | Cahaya tepat, Blitz miss/timeout dalam 5 detik | Rear miss, timeout lima detik dan retry mempertahankan sumber pada native recovery. |
| QA-10 | LULUS TERARAH | Blitz berkali-kali dalam satu window/siklus | Klik kedua pada cooldown tidak memberi hit/debit tambahan; sukses memindah state dari EXPOSED. |
| QA-11 | SEBAGIAN | Bawa tiga pusaka tetapi bukti/Jurnal kurang; masuk OR, periksa meja, keluar | Gate persiapan negatif unit dan kabut/door derived; jalur masuk aktual dengan tiga pusaka. |
| QA-12 | SEBAGIAN | Ambil `SECRET_P2_LABEL_PASIEN` dua kali | Validasi unique ID dan rollback secret unit; pickup D1 aktual sekali per jalur, belum dua E pada objek yang sama. |
| QA-13 | SEBAGIAN | Mati/reload sesudah ambil label | Secret melewati reload investigasi dan CP7; seluruh variasi kematian sesudah pickup belum dimainkan. |
| QA-14 | SEBAGIAN | Selesai boss tanpa label | Jalur tanpa arsip kembali secara fisik ke D1 setelah CP6 pada baterai0. |
| QA-15 | LULUS TERARAH | `P2_COMPLETE` | CP6/CP7 atomik, reload disk dan reload scene; saldo/Sanity/inventori/segel tetap konsisten. |
| QA-16 | LULUS TERARAH | Tarik laci salah tanpa memindai; tutup dengan `E` dalam 1,5 s; ulangi tanpa menutup; coba B2 | Edge fixture menguji E cancel, B2 -18, gas6s tanpa fatal/debit dan gerak×0.8. |
| QA-17 | SEBAGIAN | Baterai sangat rendah sebelum Poli/C3/EVP/AR | Recovery IGD dan arena aktual; variasi baterai rendah di setiap sensor eksplorasi belum dimainkan semua. |
| QA-18 | SEBAGIAN | Belanja: beli powerbank ×2, spam beli, reload di tengah transaksi | Unit belanja paket unik/debit/receipt/reload; process kill persis saat write belum diinjeksikan. |
| QA-19 | SEBAGIAN | Spam kode `1603` benar dan interaksi pintu | Keypad1603 aktual dan receipt idempoten; spam kode berulang belum dimainkan pada semua timing UI. |
| QA-20 | SEBAGIAN | Attach pertama: menoleh dan biarkan meter penuh selama jendela belajar; ulang setelah lepas pertama atau 30s | Edge fixture menguji menoleh nonfatal saat learning dan berakhir lebih awal sesudah hold; menunggu meter penuh selama seluruh30s belum dijalankan. |
| QA-21 | LULUS TERARAH | Hold 5 s: berhenti, keluar frame, lalu lanjut | Hold dari perpindahan kapsul, berhenti dan keluar Selfie tetap menyimpan akumulasi; detach aktual. |
| QA-22 | SEBAGIAN | Bersembunyi + `F`; nyalakan layar saat `LOITER` | Safety menguji hide+layar0 enam detik dan E exit; menyalakan layar di tengah loiter belum diuji. |
| QA-23 | SEBAGIAN | Selfie terangkat lalu `E` pada kunci, laci, sakelar, brankas | E actual pada kunci/laci/sakelar dengan ponsel; bukan bukti semua pose/remap/perspektif. |
| QA-24 | SEBAGIAN | Buka/tutup Jurnal berulang saat attached; coba dokumen/keypad modal | Source satu grace per attach dan modal reject tersedia; spam UI lengkap belum dimainkan. |
| QA-25 | SEBAGIAN | Mati di kamar mayat sebelum dan setelah checkpoint bukti/pusaka | GRAB/Continue CP3 dan reload CP4 jalur nyata; kombinasi kematian sebelum/sesudah setiap pickup belum semuanya. |
| QA-26 | BUTUH WINDOWS | FPS saat boss (ilusi + generator + hujan + chat + Selfie render) | Memerlukan profil Windows/GPU; Null renderer dan llvmpipe bukan acceptance FPS. |
| QA-27 | SEBAGIAN | Rahasia P1 ada/tidak; ulangi setelah ambil Label | Panel memakai irisan lima ID kanon; kombinasi P1 secret dan ID asing belum seluruhnya di-run ulang pada P2. |
| QA-28 | LULUS TERARAH | Aksi epilog/motor ditekan berulang | Receipt ekonomi/segel atomik unit, video replay dan panel berulang native. |
| QA-29 | LULUS TERARAH | Pause/settings saat window 5 s atau interlude | Pause EXPOSED mempertahankan window, energi dan posisi agent; recovery clock juga memakai clock yang sama. |
| QA-30 | SEBAGIAN | Video Damar dan perekam setelah reload | Video replay sesudah epilog diuji tanpa reward ganda; reload arsip teks dan perekam terpisah. |
| QA-31 | SEBAGIAN | Baterai 0 sebelum Poli/EVP | IGD normal/emergency crank dari0 diuji pada safety; setiap charger eksplorasi belum dimainkan dari0. |
| QA-32 | SEBAGIAN | Battery 0 di PREP, TRACK, setelah miss berulang dan setelah CP6; tanpa flare/powerbank | Power recovery TRACK, miss dan generator repeatable aktual; zero-CP6 terpisah pada zero-victory fixture. |
| QA-33 | SEBAGIAN | Mati boss lalu retry 20 kali | 20 rollback snapshot unit dan satu reset lampu/flare native; bukan20 playthrough mati boss penuh. |
| QA-34 | SEBAGIAN | Sensor Poli: lintas sekilas vs stabil ≥1,5 s dalam ≤3 m | Poli stabil aktual serta target/range scene; lintasan sekilas0.1s belum diujikan terpisah. |
| QA-35 | SEBAGIAN | Laci C3: AR saja, Thermo saja, keduanya | Rantai bergantung receipt dwell Thermo; jalur aktual; seluruh kombinasi AR-only/Thermo-only belum dijalankan. |
| QA-36 | SEBAGIAN | Pindai laci sambil berjalan; kerucut >15°; jarak >1,5 m | Range/cone source dan target fisik; sweeping saat berjalan di luar batas belum lengkap. |
| QA-37 | SEBAGIAN | Masuk/keluar basement, reload dan ganti area berulang | Gain exterior/telegraph terpisah source; perlu penerimaan pendengaran dan repeated traversal. |
| QA-38 | SEBAGIAN | Keypad `1603` tanpa membaca chat | Puzzle1603 dijalankan tanpa chat/live; clue tersimpan dan kode bukan label pintu. |
| QA-39 | SEBAGIAN | Brankas AR vs Catatan Suster (Lastri 26) + papan 04; forum/Inkubator dilewati; battery0 setelah kode diketahui | 2604 dibuka melalui deduksi suster sebelum Inkubator; operasi pada0 dan semua kode salah belum dimainkan. |
| QA-40 | SEBAGIAN | Ambil Kunci Teralis bersama laporan; naik ke L2 tanpa C3; save/load | Kunci terpisah dari C3 diuji unit/referensi; jalur naik sebelum C3 belum end-to-end. |
| QA-41 | LULUS TERARAH | Save Sanity 0 di P2 | Sanity0/23 tetap pada unit/transaksi dan jalur lengkap; tidak ada drain P2. |
| QA-42 | SEBAGIAN | Kemenyan; `G` di luar boss | Kemenyan tetap dalam snapshot/CP6 dan flare hanya TRACK; keyboardG di luar boss belum seluruh konteks. |
| QA-43 | LULUS TERARAH | Overlay Rp164,8 juta vs saldo; donasi terikat; belanja | 164.8M hanya overlay; bound donation, wallet0, belanja dan replay tidak bercampur pada tes. |
| QA-44 | SEBAGIAN | Migrasi save P0/P1 valid; file duplikat/korup/versi asing; masuk tanpa prasyarat | Migrasi1/2→3 dan future schema diuji; semua byte-corruption/backup dan entry UI belum dieksplorasi. |
| QA-45 | SEBAGIAN | Hold maju 5s dengan kecepatan kapsul aktual ≥0,8m/s, akselerasi/crouch/collision dan loop XZ | Kapsul bergerak melalui collision nyata, hold kumulatif; bukan semua crouch/akselerasi pada tiap lintasan. |
| QA-46 | LULUS TERARAH | Attach normal setelah jendela belajar, tanpa Selfie, tanpa formalin/segmen gelap atau UI grace | Native normal GRAB diukur sekitar10s meter+2s rambut setelah attach; telegraph terpisah. |
| QA-47 | LULUS TERARAH | Tembok bata Inkubator: lintasi, matikan AR, baterai 0% | Crossing AR permanen, pulang pada0 dan reload jalur aktual. |
| QA-48 | SEBAGIAN | Yaw kontrol melintasi cone 50° cepat dan ≥0,25s; ulang OTS/FP/crouch | Belajar/nonfatal dan GRAB diuji; sweep yaw50°/0.25s OTS/FP/crouch belum lengkap. |
| QA-49 | SEBAGIAN | Selfie: sebagian bounds tertutup, objek kecil sesaat, lalu dinding solid >0,25s | Frustum9-anchor/LOS source dan render nyata; seluruh kombinasi occlusion singkat/tembok belum digerakkan. |
| QA-50 | SEBAGIAN | Setelah `MORGUE_CLEARED` | Interval45s memakai MorgueCleared; belum diukur dengan run45s tersendiri. |
| QA-51 | SEBAGIAN | Patroli L2: segmen padam, zona tenang | Edge fixture L2 flicker/padam dan quiet-zone0; kenyamanan navigasi manual masih perlu review. |
| QA-52 | SEBAGIAN | Toggle C, rekam 6s pada ≤2m dan LOS; C lagi/layar mati/jarak putus; inkubator salah | EVP04 enam detik valid dijalankan; interruptC/range/salah inkubator belum semua variasi. |
| QA-53 | SEBAGIAN | Boss: 1–2 Blitz sukses lalu force quit dan load | 20 snapshot rollback mencakup perubahan RAM, write barrier dan disk CP5; force-kill native pada hit1/2 belum dijalankan. |
| QA-54 | LULUS TERARAH | `P2_BOSS_WIN` dengan simulasi disk-write gagal | Disk-write failure CP6 nyata di unit, rollback dan retry satu victory tanpa half-seal. |
| QA-55 | SEBAGIAN | Pilih tiap ilusi/asli dengan reticle Thermo Selfie, cahaya palsu dan jarak dekat | Native reticle memilih asli-20/ilusi-6; lampu hanya ilusi/cooldown12; pop-respawn6s belum disampling lengkap. |
| QA-56 | LULUS TERARAH | Blitz Selfie vs Blitz belakang | Blitz Selfie dan rear sesudah berbalik sukses pada jalur; rear menghadap menjauh gagal pada recovery. |
| QA-57 | BUTUH PLAYTEST | Profil playtest target 82 menit di build nyata | Membutuhkan playtest manusia bertanda waktu; perjalanan otomatis tidak membuktikan82menit. |
| QA-58 | SEBAGIAN | Mati setelah ambil Gunting, sebelum foto aura | Transaksi Gunting dan foto terpisah; jalur foto-dulu dan reload; death tepat antara keduanya belum native. |
| QA-59 | SEBAGIAN | Masuk HUB battery0/1/19/85, pilih colokan atau tolak, lalu berangkat/reload | Unit carry-over0/1/14/67/100 dan charge sekali; 19/85 serta variasi UI fisik belum seluruhnya. |
| QA-60 | LULUS TERARAH | Mati/Continue sepuluh kali dari CP3 | Native sepuluh CP3 load/restore mempertahankan baterai snapshot; unit20 retry boss. |
| QA-61 | LULUS TERARAH | Baterai habis saat Lastri menempel di kamar mayat | Safety HP0+hide6s+E keluar berhasil; tidak ada WATCH_ONLY. |
| QA-62 | LULUS TERARAH | Setelah boss, kembali ke basement/L1 dengan baterai 0% | Pulang L2→basement/L1/IGD/motor dengan0 tanpa AI/gas setelah kemenangan. |
| QA-63 | SEBAGIAN | Selfie terangkat, E pada kunci/Gunting/Gelang/sakelar; coba dari balik tembok | Interaksi ray aktual dan LOS source/unit; seluruh pickup dari balik tembok belum dimainkan. |
| QA-64 | SEBAGIAN | E pickup ketika HAIR_TOUCH sebelum dan tepat pada deadline GRAB | Urutan tick fatal sebelum interactor ada; boundary E pada tepat deadline belum diinjeksikan. |
| QA-65 | LULUS TERARAH | `E` di meja operasi dari jauh vs dalam radius | Native far-table ditolak dan E dekat menulis CP5. |
| QA-66 | LULUS TERARAH | Dua pusaka + tiga bukti di pintu OR; lalu tiga pusaka; lalu setelah CP6 | Gate tiga pusaka/unit dan pintu CP6 terbuka walau inventori pusaka pindah ke meja. |
| QA-67 | BUTUH PLAYTEST | Tiga menit di L2: lorong, Inkubator, Jaga Suster | Perlu tiga menit navigasi manusia untuk penerimaan area/clue, bukan timer otomatis. |
| QA-68 | LULUS TERARAH | Pulang L2→L1→IGD→motor setelah boss | Kapsul benar-benar turun tangga dan mencapai motor; backtrack basement pada0. |
| QA-69 | LULUS TERARAH | Panel motor → Kembali Menjelajah → save/reload | Tiga pembatalan panel tidak menyimpan COMPLETE; reload akhir terpisah. |
| QA-70 | LULUS TERARAH | Tekan Lanjutkan berkali-kali atau simulasi tulis CP7 gagal | Unit CP7 I/O failure/retry; native panel/fade/posisi fault injection pada zero-victory/exit-fault run. |
| QA-71 | LULUS TERARAH | Setelah `P2_CROSS_INKUBATOR_WALL`, baterai 0% di L2/L1 | Bukaan fisik dua arah tetap saat baterai0, termasuk setelah reload. |
| QA-72 | SEBAGIAN | Dinamo Pos Petugas: 3%, mulai crank, Lastri menempel setelah 30 s, kembali | Safety membuktikan crank20+26s akumulatif; attach sesudah tepat30s di DUTY belum diuji. |
| QA-73 | SEBAGIAN | Emergency crank 9%, berulang setelah cooldown | Emergency crank cap10 dan cooldown120 ditolak pada safety; semua charger/cooldown selesai belum diuji. |
| QA-74 | SEBAGIAN | Daftar 17/16, D1 17°C AR17, D2-D4 12°C dan kunci; coba buka D1 dengan kunci tanpa scan | D1 dibuka kunci tanpa scan pada jalur; source17/16/17°C dan AR17; semua D2-D4 belum diperiksa manual. |
| QA-75 | SEBAGIAN | Jejak Damar di terowongan servis | Jejak Damar ada sebagai interaksi non-gating; belum diverifikasi reward-free pada directed run. |
| QA-76 | LULUS TERARAH | Jalur dev tanpa `P1_COMPLETE` | Fixture dev-only opt-in/save terisolasi dan prasyarat produksi ditolak unit. |
| QA-77 | LULUS TERARAH | Buka P2 dari CP7 P1 dan inspeksi scene Kos | Kos existing dipakai tanpa resize, Prolog guard actual pada dua run. |
| QA-78 | SEBAGIAN | OTS/FP tahan/lepas RMB dari app dan toggle X/T/Q; F/Tab selama hold lalu fokus hilang | Safety RMB/X/F/Tab; remap, semua app/filter/perspektif dan release-focus matrix belum lengkap. |
| QA-79 | BUTUH PLAYTEST | Tukar binding X/V lewat konfigurasi input yang diizinkan | Butuh playtest binding ulang; belum diuji end-to-end oleh native input. |
| QA-80 | BUTUH WINDOWS | Alt-tab saat attach, dokumen, dan EXPOSED; buka pause/settings | Pause engine diuji; Alt-tab Windows saat tiap state butuh Windows nyata. |
| QA-81 | SEBAGIAN | Normal→Selfie→Thermo→AR/EVP, layar mati dan hard modal/pause | Drain/cost global dan pause diuji; seluruh kombinasi overlay/hard-modal belum diprofil numerik. |
| QA-82 | SEBAGIAN | Memulai P2 dengan battery P1=0,1,19,85 lalu menolak colokan Kos | Unit menolak auto-charge carry-over, native departure aktual; semua9 boundary investigasi belum dimainkan. |
| QA-83 | SEBAGIAN | CP5 battery0/1/14/15/29/44/45/80/100; generator sudah dipakai; miss hingga habis | 9 batas snapshot CP5 unit; recovery0 actual repeatable. Bukan9 pertarungan lengkap setelah miss hingga habis. |
| QA-84 | SEBAGIAN | Lastri menempel ketika mencoba dinamo basement setelah 30/45s crank | Crank akumulatif berhenti saat attached source/safety sebagian; fixture30/45 di DUTY belum dijalankan. |
| QA-85 | SEBAGIAN | Spam Jurnal/inventory/keypad/E/F/X/RMB saat attached | Satu handset/input pemilik existing; spam semua tombol saat attached belum lengkap. |
| QA-86 | SEBAGIAN | Selfie dekat dinding/tepi meja, semua perspektif; rotasi cepat dan quiet zone pada battery0 | Collision lensa seluruh segmen dan quiet-zone0 tersedia; inspeksi semua sudut OTS/FP dekat tembok masih diperlukan. |
| QA-87 | SEBAGIAN | Load schema1/2 valid, schema3 hasil migrasi, future schema dan .bak | Migrasi nested, schema future, data valid ditolak/diterima sesuai unit; backup korup lengkap belum dimainkan. |
| QA-88 | SEBAGIAN | Trigger sensor suhu/EVP/AR/foto dua kali termasuk lewat Continue | Receipt dan ID set unit; semua sensor dua kali sesudah Continue belum diputar satu per satu. |
| QA-89 | SEBAGIAN | Force quit sebelum/saat/setelah shop dan charge HUB, lalu Continue | Unit transaksi shop/charge rollback dan reload; force-quit tiap timing write belum native. |
| QA-90 | SEBAGIAN | Masuk IGD dari scene Kos 23:45→23:50 dan buka live lagi setelah reload | Hub14:00/depart23:45/IGD23:50 source/provider dan live idempoten; kedua run tanpa live sampai epilog. |
| QA-91 | SEBAGIAN | Periksa seluruh naskah/clue/kanon dan dokumen sumber | Teks kode/suhu/usia/korban/akhir kanon diselaraskan; VO manusia belum tersedia. |
| QA-92 | LULUS TERARAH | Tiga lampu LO/UV-N/UV-S diperiksa dari posisi sakelar dan semua arah lari | Unit radius/jangkauan/LOS dan cahaya kepala; ketiga lampu benar-benar dipakai pada kedua jalur. |
| QA-93 | LULUS TERARAH | Saat TRACK bidik ilusi, lalu EXPOSED bidik asli; klik cepat pada satu window | Shielded/away miss berbiaya15/6, ilusi bersih saat exposure, satu hit per siklus. |
| QA-94 | SEBAGIAN | Matikan aplikasi saat 1–2 hit dan setelah `P2_BOSS_WIN` sebelum/selama tulis CP6 | Write barrier dan CP6 failure/rollback unit + scene restore; proses-kill saat filesystem replace belum dilakukan. |
| QA-95 | LULUS TERARAH | Dari CP6 battery0 selesaikan epilog dengan generator fisik; lalu ke motor dan D1 | Zero-victory native memastikan0%CP6 tetap tenang, generator fisik dan epilog, pulang0; D1 melalui run tanpa arsip. |
| QA-96 | LULUS TERARAH | Pilih `Kembali Menjelajah` 3×, lalu `Lanjutkan` dan fault injection CP7 | Panel dibatalkan3× serta native fault CP7 tanpa fade/shutter, kemudian retry berhasil. |
| QA-97 | SEBAGIAN | Kombinasi P1 secret ada/tidak ada, P2 secret ada/tidak ada | Panel global memakai irisan ID; matriks empat kombinasi P1/P2 secret belum semuanya dimainkan. |
| QA-98 | SEBAGIAN | Audit aset scene RSUD saat kelak tersedia | 49 model path baru, URL/hash/lisensi dan builder/scene placement nyata; penerimaan artistik dan acting manusia masih perlu desainer. |
| QA-99 | BUTUH WINDOWS | Profil Windows GPU sasaran dengan world+feed Selfie+grip dan transparansi boss | Windows/GPU/IrisXe8GB dan1%low belum tersedia; tidak menganggap render software sebagai hasil sasaran. |
| QA-100 | SEBAGIAN | Dua run HUB→CP7: tanpa live manual/belanja/secret dan dengan arsip lengkap | Dua run penuh tanpa gate belanja/live; arsip ada/tidak; pacing82menit tetap menunggu playtest manusia. |

## Gate yang belum terpenuhi

- Acceptance seluruh100 skenario, playtest Windows pada GPU sasaran, performa60FPS/1%low dan pacing82menit belum terbukti.
- Rekaman VO manusia sesuai dialog Indonesia belum tersedia; teks kanon dan cue nonverbal berlisensi tersedia.
- Visual/acting, variasi occlusion, remapping dan stress input perlu penerimaan manusia; tidak diklaim setara DreadOut/Outlast.

Rekap: {"LULUS TERARAH": 35, "SEBAGIAN": 59, "BUTUH WINDOWS": 3, "BUTUH PLAYTEST": 3}.
