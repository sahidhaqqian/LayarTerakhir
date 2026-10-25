# Phase 5 — 180 QA dan bukti tambahan

Status penerimaan manusia tidak disamakan dengan hasil unit, scene, posed render atau driver native. Semua baris tetap **MANUAL_PENDING** sampai skenario dimainkan dan dinilai sebagaimana tertulis. Kolom dukungan hanya menunjuk ruang lingkup otomasi yang relevan, bukan PASS keseluruhan skenario.

## Artefak yang tersedia

- `Audit/phase5-campaign-tests.xml`: {'path': 'Audit/phase5-campaign-tests.xml', 'result': 'Passed', 'total': '56', 'passed': '56', 'failed': '0'}
- `Audit/phase5-scene-tests.xml`: {'path': 'Audit/phase5-scene-tests.xml', 'result': 'Passed', 'total': '26', 'passed': '26', 'failed': '0'}
- `Audit/phase5-regression-tests.xml`: {'path': 'Audit/phase5-regression-tests.xml', 'result': 'Failed(Child)', 'total': '429', 'passed': '427', 'failed': '2'}
- `Audit/phase5-focused-tests.xml`: {'path': 'Audit/phase5-focused-tests.xml', 'result': 'Passed', 'total': '232', 'passed': '232', 'failed': '0'}
- `Audit/phase5-native-route.json`: {'path': 'Audit/phase5-native-route.json', 'result': True, 'method': 'Actual zero-resource Phase4 handoff; real keyboard/mouse locomotion, interactions, scanning and boss route'}
- `Audit/phase5-native-visual.json`: {'path': 'Audit/phase5-native-visual.json', 'result': True, 'method': 'Posed composition fixtures, not gameplay acceptance'}
- `Audit/phase5-ending-bad.json`: {'path': 'Audit/phase5-ending-bad.json', 'result': True, 'method': 'Prepared FINAL_CHOICE fixture; actual input, controller collision, cutscene restart and CP9 persistence'}
- `Audit/phase5-ending-normal.json`: {'path': 'Audit/phase5-ending-normal.json', 'result': True, 'method': 'Prepared FINAL_CHOICE fixture; actual input, controller collision, cutscene restart and CP9 persistence'}
- `Audit/phase5-ending-true.json`: {'path': 'Audit/phase5-ending-true.json', 'result': True, 'method': 'Prepared FINAL_CHOICE fixture; actual input, controller collision, cutscene restart and CP9 persistence'}
- `Audit/phase5-ending-secret.json`: {'path': 'Audit/phase5-ending-secret.json', 'result': True, 'method': 'Prepared FINAL_CHOICE fixture; actual input, controller collision, cutscene restart and CP9 persistence'}
- `Audit/phase5-crash-probes.json`: {'path': 'Audit/phase5-crash-probes.json', 'result': True, 'method': 'Actual isolated native player process termination after flushed temp or after atomic replace; fresh native loader verification. Not physical gameplay inputs.'}

## Daftar 180 QA

| ID | Skenario | Penerimaan | Dukungan otomatis parsial |
|---|---|---|---|
| QA-001 | Continue dari `PHASE4_CP8` dengan empat segel valid | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-002 | Mulai P5 tanpa `P4_COMPLETE` atau tanpa empat segel (build produksi) | MANUAL_PENDING |  |
| QA-003 | Muat save P4 berisi Flare/Kemenyan/Powerbank belum terpakai | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-004 | Muat save yang berhenti di boundary P4 sebelum router terhubung | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-005 | Ganti `V` OTS↔FP sambil UV/AR aktif | MANUAL_PENDING |  |
| QA-006 | Tekan `X` (Selfie) dan verifikasi hint | MANUAL_PENDING |  |
| QA-007 | Remap `X/Q/R/T/C/F/G/H/E` via `GameInputConfig` | MANUAL_PENDING |  |
| QA-008 | Tahan/lepas RMB dengan AR/UV/Thermo bergantian | MANUAL_PENDING |  |
| QA-009 | `G` saat Flare 4 | MANUAL_PENDING |  |
| QA-010 | Alt-tab saat animasi angkat HP | MANUAL_PENDING |  |
| QA-011 | Masuk dengan `wallet_legitimate=0`, baterai 0, Sanity 0 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-012 | Lihat viewer 1.000.000 dan donasi Rp66.666.666 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-013 | Reload setelah donasi | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-014 | Beli Powerbank ×3 dengan saldo cukup | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-015 | Klik beli paket sama dua kali cepat | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-016 | Force quit saat commit pembelian | MANUAL_PENDING |  |
| QA-017 | Saldo 0: semua tombol Kios | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-018 | Baca Kertas Aturan lalu lewati di run lain | MANUAL_PENDING |  |
| QA-019 | Lihat kios saat chat menyuruh “jangan matikan siarannya” | MANUAL_PENDING |  |
| QA-020 | Selesaikan P5 tanpa membeli apa pun | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-021 | Periksa Kios Arsip Bisu tanpa membayar | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-022 | Layar mati 2 s di depan kursi | MANUAL_PENDING |  |
| QA-023 | Layar mati ≥3 s di depan kursi | MANUAL_PENDING |  |
| QA-024 | Nyalakan UV (bukan AR) di depan laci | MANUAL_PENDING |  |
| QA-025 | AR di depan kursi/laci | MANUAL_PENDING |  |
| QA-026 | EVP merekam kalimat Mimic dari cermin | MANUAL_PENDING |  |
| QA-027 | EVP merekam “Saksi tidak membeli korban” Authentic | MANUAL_PENDING |  |
| QA-028 | Ambil pita → `TryCommit` sukses | MANUAL_PENDING |  |
| QA-029 | Disk penuh saat pickup | MANUAL_PENDING |  |
| QA-030 | Mati/reload setelah pita diambil | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-031 | Lihat cermin lama di gerbang | MANUAL_PENDING | Audit/phase5-scene-tests.xml (serialized/physics scope only) |
| QA-032 | Sanity ke 0 di sektor | MANUAL_PENDING |  |
| QA-033 | Pulihkan lewat Altar Doa pada Sanity 0 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-034 | Kemenyan `H` dengan stok 0 | MANUAL_PENDING |  |
| QA-035 | Kemenyan `H` dengan stok 1 | MANUAL_PENDING |  |
| QA-036 | Pause/alt-tab 2 menit | MANUAL_PENDING |  |
| QA-037 | Chat `SYSTEM` palsu muncul | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-038 | Chat 40 pesan saat chase | MANUAL_PENDING |  |
| QA-039 | Opsi kurangi strobo/blur/shake | MANUAL_PENDING |  |
| QA-040 | Replika masuk Sanity 0 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-041 | Masuk koridor dengan replika di 8 m | MANUAL_PENDING |  |
| QA-042 | Selfie (`X`) menahan replika 1,0 s | MANUAL_PENDING |  |
| QA-043 | Thermo pintu kiri | MANUAL_PENDING |  |
| QA-044 | Thermo pintu kanan | MANUAL_PENDING |  |
| QA-045 | Thermo pintu tengah | MANUAL_PENDING |  |
| QA-046 | Chat/banner menyebut “tengah panas” | MANUAL_PENDING |  |
| QA-047 | Masuk pintu kiri/kanan | MANUAL_PENDING |  |
| QA-048 | Masuk pintu tengah sebelum replika menerkam | MANUAL_PENDING |  |
| QA-049 | Baterai 0 di awal koridor | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-050 | Mati di koridor lalu reload | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-051 | EVP cabang kiri | MANUAL_PENDING |  |
| QA-052 | EVP cabang kanan | MANUAL_PENDING |  |
| QA-053 | Chat serempak ke kiri | MANUAL_PENDING |  |
| QA-054 | UV pada papan kanan | MANUAL_PENDING | Audit/phase5-scene-tests.xml (serialized/physics scope only) |
| QA-055 | Injak papan Rajah Ungu | MANUAL_PENDING |  |
| QA-056 | Siram Obor Santet dengan Kendi air>0 | MANUAL_PENDING |  |
| QA-057 | Siram dengan Kendi air=0 | MANUAL_PENDING |  |
| QA-058 | Isi ulang di titik sebelah gerbang | MANUAL_PENDING |  |
| QA-059 | Save tanpa Kendi (item hilang) | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-060 | Force quit setelah siram sebelum commit | MANUAL_PENDING |  |
| QA-061 | Masuk gelap total dengan layar menyala | MANUAL_PENDING |  |
| QA-062 | `F` lalu merunduk di lemari | MANUAL_PENDING |  |
| QA-063 | Sembunyi di kolong meja saat dikejar | MANUAL_PENDING |  |
| QA-064 | Sprint vs jalan vs jongkok pada jarak sama | MANUAL_PENDING |  |
| QA-065 | Kontak pertama | MANUAL_PENDING |  |
| QA-066 | Kontak kedua dalam 10 s | MANUAL_PENDING |  |
| QA-067 | AR di cermin tanpa mode AR | MANUAL_PENDING |  |
| QA-068 | AR di Cermin Rias | MANUAL_PENDING |  |
| QA-069 | Replika spawn di anchor | MANUAL_PENDING |  |
| QA-070 | Mati lalu reload di sektor 3 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-071 | Dialog Damar diulang | MANUAL_PENDING |  |
| QA-072 | AR pada Server Batu | MANUAL_PENDING |  |
| QA-073 | Thermo `-- °C` pada Kontrak saja | MANUAL_PENDING |  |
| QA-074 | UV Mata Terbuka saja | MANUAL_PENDING |  |
| QA-075 | Thermo lalu UV (atau sebaliknya) | MANUAL_PENDING |  |
| QA-076 | EVP Pilar Layar (Authentic) | MANUAL_PENDING |  |
| QA-077 | Konfirmasi Jurnal dengan 2 bukti | MANUAL_PENDING |  |
| QA-078 | Konfirmasi dengan 3 bukti, empat segel | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-079 | Tutup Jurnal tanpa konfirmasi | MANUAL_PENDING |  |
| QA-080 | Konfirmasi tanpa salah satu segel (dev hack) | MANUAL_PENDING |  |
| QA-081 | Ruang Cermin Saksi menampilkan N/5 | MANUAL_PENDING |  |
| QA-082 | Cermin Jalan Pulang ke gerbang sebelum CP6 | MANUAL_PENDING |  |
| QA-083 | Konfirmasi arena | MANUAL_PENDING |  |
| QA-084 | Fail write CP6 | MANUAL_PENDING |  |
| QA-085 | Fase 1: UV pada 4 sosok | MANUAL_PENDING |  |
| QA-086 | Bayangan layar disentuh | MANUAL_PENDING |  |
| QA-087 | Pancing sosok asli ke Zona Generator, nyalakan sakelar | MANUAL_PENDING |  |
| QA-088 | Blitz tepat kepala lensa | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-089 | Miss Blitz Fase 1 | MANUAL_PENDING |  |
| QA-090 | Fase 2: Thermo `-30°C`, Selfie rahang, Flare, Blitz | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-091 | Fase 2 dengan Flare 0 | MANUAL_PENDING |  |
| QA-092 | Fase 2 tanpa Kemenyan | MANUAL_PENDING |  |
| QA-093 | Gelombang api: layar mati di balik peti | MANUAL_PENDING |  |
| QA-094 | Peti kayu bukan invulnerability | MANUAL_PENDING | Audit/phase5-scene-tests.xml (serialized/physics scope only) |
| QA-095 | AR memperlihatkan tiga Kabel Roh | MANUAL_PENDING |  |
| QA-096 | Tahan `E` 2 s lalu batal | MANUAL_PENDING |  |
| QA-097 | Putus kabel 1, mati, reload | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-098 | Putus ketiga kabel | MANUAL_PENDING |  |
| QA-099 | Blitz Inti 3 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-100 | Kematian di Fase 3 setelah Inti 1–2 committed | MANUAL_PENDING |  |
| QA-101 | Masuk `FINAL_CHOICE` | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-102 | Tahan `F` 5 s sebelum `FINAL_CHOICE` | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-103 | LMB pada Damar sebelum state | MANUAL_PENDING |  |
| QA-104 | Tiga aksi dasar terlihat tanpa Secret | MANUAL_PENDING |  |
| QA-105 | Aksi Secret terlihat tanpa lima ID | MANUAL_PENDING |  |
| QA-106 | Dua aksi ending hampir bersamaan | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-107 | Input ending setelah `endingId!=NONE` | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-108 | Kapasitor setelah LMB ending | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-109 | Sanity 0 pada `FINAL_CHOICE` | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-110 | Tidak ada timer fatal | MANUAL_PENDING |  |
| QA-111 | Arahkan kamera belakang ke Damar <2 s | MANUAL_PENDING |  |
| QA-112 | Dwell 2 s, label TUMBAL ≥0,5 s, LMB | MANUAL_PENDING |  |
| QA-113 | Turunkan HP sebelum LMB | MANUAL_PENDING |  |
| QA-114 | Epilog Bad: saldo Rp500.000.000 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-115 | Epilog Bad: akun `Penonton_Terakhir` | MANUAL_PENDING |  |
| QA-116 | Di Z_EDGE tahan `F` 5 s | MANUAL_PENDING |  |
| QA-117 | Lepas `F` sebelum 5 s | MANUAL_PENDING |  |
| QA-118 | Lepas F pada 4,9s lalu tekan lagi | MANUAL_PENDING |  |
| QA-119 | Epilog Normal: HP hancur | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-120 | Epilog Normal: Damar di halaman Atmaja | MANUAL_PENDING |  |
| QA-121 | Papah Damar ke Z_PORTAL dengan `E` | MANUAL_PENDING |  |
| QA-122 | Urutan `X→Q→R` benar | MANUAL_PENDING |  |
| QA-123 | Urutan salah (`R` sebelum `Q`) | MANUAL_PENDING |  |
| QA-124 | AR dan UV di gameplay biasa | MANUAL_PENDING |  |
| QA-125 | LMB pada inti di Selfie | MANUAL_PENDING |  |
| QA-126 | Epilog True: 05:30, azan, Rp125.000.000 | MANUAL_PENDING |  |
| QA-127 | Lima ID valid; validasi di cermin | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-128 | Tahan `E` 3 s di Cermin Saksi lalu batal | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-129 | Mode Arsip: bidik Damar | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-130 | Mode Arsip: cermin berbidikan emas + LMB | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-131 | Migrasi save P4 valid | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-132 | Inti 1 sukses, force quit | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-133 | Inti 1–2 sukses, mati Fase 3 | MANUAL_PENDING |  |
| QA-134 | Fail write saat inti pecah | MANUAL_PENDING |  |
| QA-135 | Fail write `FINAL_CHOICE_ENTER` | MANUAL_PENDING |  |
| QA-136 | Crash setelah `ending_id` terkunci | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-137 | Double click ending | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-138 | Lock ending gagal tulis | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-139 | `wallet_bound` Rp66.666.666 setelah reload | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-140 | Bandingkan save CP8 vs CP9 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-141 | 720p Low gerbang dengan banyak cermin | MANUAL_PENDING |  |
| QA-142 | 720p Low arena dengan gelombang api | MANUAL_PENDING |  |
| QA-143 | UV + AR bergantian di fase 1/3 | MANUAL_PENDING |  |
| QA-144 | Simulasi buta warna | MANUAL_PENDING |  |
| QA-145 | Subtitle Morse/api/ketukan | MANUAL_PENDING |  |
| QA-146 | Opsi kurangi flash cermin (True) | MANUAL_PENDING |  |
| QA-147 | Peringatan konten Bad ending | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-148 | Audit semua asset Pasar Setan | MANUAL_PENDING |  |
| QA-149 | Kit visual replika P5 baru | MANUAL_PENDING | Audit/phase5-scene-tests.xml (serialized/physics scope only) |
| QA-150 | Windows Iris Xe RAM 8GB, ±75 menit | MANUAL_PENDING |  |
| QA-151 | Replika Kuntilanak Merah bukan boss | MANUAL_PENDING |  |
| QA-152 | Segel P1–P4 setelah semua replika | MANUAL_PENDING |  |
| QA-153 | Thermo bukan oracle pintu karena chat | MANUAL_PENDING |  |
| QA-154 | Kendi tunggal sepanjang P4→P5 | MANUAL_PENDING |  |
| QA-155 | Dua inti dalam satu jendela | MANUAL_PENDING |  |
| QA-156 | Blitz di luar jendela | MANUAL_PENDING |  |
| QA-157 | Generator sakelar tanpa sosok di zona | MANUAL_PENDING |  |
| QA-158 | Lentera: ambil, jatuhkan, cooldown 15 s | MANUAL_PENDING |  |
| QA-159 | Flare dan Lentera bersamaan | MANUAL_PENDING |  |
| QA-160 | Chat palsu menyuruh menoleh di Fase 2 | MANUAL_PENDING |  |
| QA-161 | Layar mati 3 s menghentikan chat palsu | MANUAL_PENDING |  |
| QA-162 | Kapasitor 5% vs baterai 100% sebelum ending | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-163 | Normal tanpa LMB | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-164 | Bad lalu coba True | MANUAL_PENDING |  |
| QA-165 | True lalu coba Secret | MANUAL_PENDING |  |
| QA-166 | Secret tanpa Pita (lima ID tidak lengkap) | MANUAL_PENDING |  |
| QA-167 | Secret dengan lima ID benar tetapi tanpa validasi | MANUAL_PENDING |  |
| QA-168 | Arsip N/5 dengan kombinasi kepemilikan P1–P5 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-169 | Pita diambil setelah kembali dari ruang cermin | MANUAL_PENDING |  |
| QA-170 | Cermin Jalan Pulang setelah CP6 | MANUAL_PENDING |  |
| QA-171 | Baterai 0 saat CP6 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-172 | Sanity 0, baterai 0, tanpa kemenyan, Flare 0 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-173 | Satu juta wajah/chat | MANUAL_PENDING |  |
| QA-174 | Chat satu juta humanis | MANUAL_PENDING |  |
| QA-175 | Nama korban baru tidak dikarang | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-176 | Dua cutscene bersamaan | MANUAL_PENDING |  |
| QA-177 | `CP9` gagal tulis setelah cutscene | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-178 | Selesaikan Run A: tanpa toko, tanpa chat, tanpa rahasia | MANUAL_PENDING |  |
| QA-179 | Selesaikan Run B: semua paket + lima Arsip | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |
| QA-180 | Regresi P1–P4 pascamigrasi dan CP7 rolling P5 | MANUAL_PENDING | Audit/phase5-campaign-tests.xml (transaction/loader scope only) |

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
