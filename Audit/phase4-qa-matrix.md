# Phase4 —180 acceptance case

Validasi25Oktober2026: **280/280 regresi PASS**, exit0 (`phase4-regression.xml`, `phase4-regression-run.json`), **35 tes scene PASS di dalam suite regresi** dan **128 pemeriksaan rute native PASS** (`phase4-native-route.json`, `phase4-native-route-run.json`). Rute native memakai input HP/interactor, CharacterController/NavMesh, AI aktif, satu reload scene CP6, tanpa pembelian, sampai turun tangga CP8; persiapan hub dan konfirmasi Jurnal/keluar memanggil adapter yang sama secara langsung. Ini bukan180QA PASS atau playtest manusia Windows.

**ADAPTER_PASS** = transaksi/data teruji; **SCENE_PASS** = geometri/nav/raycast terserialisasi; **NATIVE_PASS** = skenario yang benar-benar tercakup traversal directed; **ASSET_PASS** = hash/sumber/lisensi/penempatan lokal; **PARTIAL** = sebagian orakel; **NOT_RUN** = belum dibuktikan. Seluruh status dibatasi bukti tersebut, termasuk UI/VO/pacing/FPS manusia yang masih terbuka. Nama tes adapter berada di `Assets/_LayarTerakhir/src/Editor/Phase4CampaignTests.cs`.

| ID | Skenario | Status | Orakel / bukti yang diperlukan |
|---|---|---|---|
| QA-001 | Continue dari P3_COMPLETE dengan tiga segel valid | PARTIAL | Babak 4 berawal di Golden Kos, status P0–P3 utuh — Bukti: `HubAndDeparturePreserveAllCampaignResourcesWithoutPurchases; belum input Continue nyata` |
| QA-002 | Coba mulai P4 tanpa P3_COMPLETE dalam build produksi | PARTIAL | Ditolak; dev-only skip tidak menulis segel/inventori palsu — Bukti: `MissingThirdSealOrPreviousCompletionCannotEnterPhase4; guard adapter, belum uji launcher produksi` |
| QA-003 | Muat P3 CP7 berisi Powerbank/Flare/Kemenyan yang belum digunakan | PARTIAL | Semua carry-over ditampilkan sebagai instance sah, tidak hilang — Bukti: `HubAndDeparturePreserveAllCampaignResourcesWithoutPurchases; carry kemenyan/PB diuji, belum UI/flare` |
| QA-004 | Ganti V OTS↔FP sambil R UV v2 aktif | NOT_RUN | FOV/culling/scan konsisten dan kamera pulih |
| QA-005 | Tahan dan lepas RMB dengan AR/UV/Thermo bergantian | NOT_RUN | HP dua tangan bekerja, pose dan view kembali seperti sebelum hold |
| QA-006 | Remap H/R/C/F/G/E melalui GameInputConfig | PARTIAL | Prompt HUD memakai binding aktual; tidak konflik  Label interaksi dan kemenyan mengikuti binding aktual; remap semua tombol saat ancaman belum diuji manual. |
| QA-007 | Tekan F saat membuka Jurnal dekat Banaspati | NOT_RUN | Layar mati darurat dan action lain tidak terpicu |
| QA-008 | Tekan E pada obor sambil HP UV dinaikkan | NOT_RUN | Interaksi sah dilakukan tanpa menurunkan HP paksa |
| QA-009 | Klik kiri pada AR aura altar tanpa mode boss | PARTIAL | Hanya foto 1% jika valid, tidak memicu Blitz 15% — Bukti: `EvidenceAndRelicsRemainSeparateAndARCostsExactlyOne; belum input kamera nyata` |
| QA-010 | Alt-tab saat animasi angkat HP dan kembali ke game | NOT_RUN | Input state tidak macet; timer gameplay mengikuti policy fokus |
| QA-011 | Masuk Kos P4 dari snapshot wallet_legitimate=0 | ADAPTER_PASS | Upgrade UV v2/Sanity gratis, berangkat tetap bisa — Bukti: `HubAndDeparturePreserveAllCampaignResourcesWithoutPurchases` |
| QA-012 | Baca dan lewati forum tragedi 1997 pada run berbeda | NOT_RUN | Dokumen opsional; upgrade/gerbang tetap dapat dicapai kedua run |
| QA-013 | Beli paket Kemenyan/PB/Air dari saldo cukup | ADAPTER_PASS | Debit sekali, item instance unik dan receipt persisted — Bukti: `ShopDebitReceiptAndUniqueInstancesAreOneIdempotentTransaction (3 paket)` |
| QA-014 | Klik beli paket sama dua kali cepat | PARTIAL | Tidak menggandakan item, saldo tak negatif — Bukti: `ShopDebitReceiptAndUniqueInstancesAreOneIdempotentTransaction; repeat adapter, bukan double-click UI` |
| QA-015 | Force quit saat pembelian di tengah commit | PARTIAL | Seluruh transaksi atau rollback; tak ada item tanpa debit — Bukti: `DiskWriteFaultRollsBackTheWholeResourceTransactionAndPreservesFile(purchase); bukan process-kill` |
| QA-016 | Gunakan colokan Kos sekali sebelum berangkat | ADAPTER_PASS | Baterai menjadi 100 sesuai policy; receipt persisted — Bukti: `HubChargeIsExplicitOnceAndCannotBeUsedAfterDeparture` |
| QA-017 | Reload Kos atau Continue dari CP1 setelah colokan | ADAPTER_PASS | Tidak ada pengisian gratis kedua — Bukti: `HubChargeIsExplicitOnceAndCannotBeUsedAfterDeparture` |
| QA-018 | Lihat 780.000 viewer, donasi Rp6.666.000 | NOT_RUN | Donasi tercatat sekali ke wallet_bound, bukan wallet_legitimate |
| QA-019 | Tampilkan akhir Rp999.999.999 [ERROR] | NOT_RUN | Saldo sah, terikat, dan expense tidak berubah oleh efek |
| QA-020 | Selesaikan P4 tanpa membeli ketiga paket toko | NATIVE_PASS | Semua evidence, 4 torches, 4 Blitz, portal berhasil  — Bukti: `phase4-native-route.json`: Rute lengkap zero-purchase |
| QA-021 | Ikuti chat palsu ke arah batu nisan Simpang 1 | NOT_RUN | UV tengkorak ungu dan telegraph terlihat sebelum hazard |
| QA-022 | Lewati simpang 1 lewat beringin kanan tanpa membaca chat | NOT_RUN | Petunjuk lingkungan tetap memadai dan rute berlanjut |
| QA-023 | Amati jejak kaki Damar di jalur kanan menggunakan UV | NOT_RUN | Tanda aman terbedakan dari tengkorak bahaya |
| QA-024 | Periksa username @warga_karang97 tapi arah UV bertentangan | NOT_RUN | Sumber kebenaran world UV, bukan username |
| QA-025 | Simpang 2 muncul dua username hampir sama | NOT_RUN | Pemain dapat membedakan melalui clue dunia, bukan hafalan nama |
| QA-026 | Simpang 2 pemain memilih jalan salah | NOT_RUN | Peringatan/loop back ada; tidak terjebak permanen |
| QA-027 | Simpang 3 chat meniru notifikasi MATABATIN | NOT_RUN | UI host input/pause tidak dibajak oleh pesan authored |
| QA-028 | Matikan seluruh pesan chat dari HUD saat melalui tiga simpang | NOT_RUN | Jalan tetap dapat diselesaikan dengan landmark dan UV |
| QA-029 | Baterai 0% sebelum simpang pertama | PARTIAL | Dinamo manual aman dapat dicapai tanpa UV atau item berbayar — Bukti: `Phase4SceneTests: anchor/recovery reachable; belum route battery0 nyata` |
| QA-030 | Reload setelah dua simpang lalu kembali ke hutan | NOT_RUN | Event chat tidak menggandakan donasi atau memutar penanda salah |
| QA-031 | Mulai dari Sanity 100 ke 70 di hutan terpapar | NOT_RUN | Drain 1 poin/10s game-time, cue tepi dan bisik di threshold |
| QA-032 | Turunkan Sanity dari 71 ke 69 lalu naikkan lagi | NOT_RUN | VFX transition idempoten, tidak menumpuk shader |
| QA-033 | Sanity turun melewati 40 saat Banaspati patroli | NOT_RUN | Deteksi risiko meningkat sesuai config, tidak teleport ke pemain |
| QA-034 | Biarkan Sanity tepat 0% | NOT_RUN | HUNT aktif tanpa Game Over otomatis |
| QA-035 | Pada Sanity 0%, masuk shelter dan gunakan shrine gratis | NOT_RUN | Interaksi selesai, Sanity pulih >0 tanpa uang |
| QA-036 | Sanity 0%, inventory kemenyan 0 dan baterai 0 | PARTIAL | Masih ada jalur fisik ke shelter, charger dan objective — Bukti: `Phase4SceneTests: semua sektor reachable dan recovery tersedia; belum run0/0` |
| QA-037 | Gunakan H dengan kemenyan 0 unit | NOT_RUN | Tidak mengurangi slot negatif atau memicu animasi penggunaan palsu |
| QA-038 | Gunakan satu kemenyan dekat low sanity | PARTIAL | Pulih ke100 mengikuti kontrak global, satu unit berkurang, receipt/item state konsisten — Bukti: `IncenseConsumesCarriedInstanceAndRecoveryCooldownUsesPersistedGameClock; aksi/intonasi belum diuji` |
| QA-039 | Pause/Alt-Tab 2 menit dalam hutan berkabut | NOT_RUN | Tidak terus menguras Sanity di background saat gameplay paused |
| QA-040 | Aktifkan opsi kurangi strobo/blur/camera shake | NOT_RUN | Cue hazard, obor dan informasi objektif tetap terbaca |
| QA-041 | Thermo pada makam lain yang bukan target +66°C | NOT_RUN | Tidak menambah Bukti #1 |
| QA-042 | Rekam Thermo +66°C target Utara dua kali | NOT_RUN | Hanya satu ID evidence |
| QA-043 | Ambil Kendi tanpa memindai makam | ADAPTER_PASS | Pusaka #1 tercatat, Bukti #1 tetap belum ada — Bukti: `EvidenceAndRelicsRemainSeparateAndARCostsExactlyOne` |
| QA-044 | Foto altar Timur dengan mode UV bukan AR | NOT_RUN | Bukti #2 tidak sah; feedback mode salah |
| QA-045 | Foto altar Timur dengan AR, simpan lalu reload | PARTIAL | ID Bukti #2 persisted sekali, efek aura terbaca — Bukti: `EvidenceAndRelicsRemainSeparateAndARCostsExactlyOne; belum feed/render/reload nyata` |
| QA-046 | Ambil Belati dan baca Damar Note terpisah | NOT_RUN | Relic dan dokumen terpisah dan idempoten |
| QA-047 | Rekam audio ambience biasa di sekitar sumur | NOT_RUN | Bukti EVP tidak sah |
| QA-048 | Rekam segmen mantra empat penjuru dengan EVP yang sah | NATIVE_PASS | Bukti #3 tersimpan sekali, Jurnal 3/3 bila lainnya ada  — Bukti: `phase4-native-route.json`: Rekam mantra autentik melalui C6detik; ID evidence tersimpan |
| QA-049 | Ambil Kantong Tanah tanpa merekam mantra | NOT_RUN | Pusaka tercatat sendiri, tidak memalsukan bukti |
| QA-050 | Kunjungi sektor W→E→N sebelum Ritual | NATIVE_PASS | Semua bukti/pusaka bisa dikumpulkan dalam urutan berbeda  — Bukti: `phase4-native-route.json`: Investigasi W→E→N mengumpulkan tiga bukti dan tiga pusaka secara terpisah |
| QA-051 | Konfirmasi Jurnal saat hanya 2 bukti | PARTIAL | Tombol tertutup; ritual tidak terbuka — Bukti: `ConfirmJournal ditolak saat bukti belum lengkap; belum spesifik 2/3 lewat UI` |
| QA-052 | Kumpulkan 3 bukti namun tutup Jurnal tanpa konfirmasi | NOT_RUN | Tidak ada P4_JOURNAL_CONFIRMED otomatis |
| QA-053 | Konfirmasi 3 bukti lalu reload | NOT_RUN | Receipt tersimpan; dideduksi tetap satu kali |
| QA-054 | Coba buka relung Cermin tanpa mendengar ketukan | PARTIAL | Tidak mengeluarkan item, clue visual sewajarnya — Bukti: `OptionalSecretRequiresKnockEyeAndWheelAndDoesNotGateBoss; adapter saja` |
| QA-055 | Verifikasi ketukan relung dengan EVP asli | NOT_RUN | Puzzle state verifikasi tervalidasi, bukan ambience palsu |
| QA-056 | Sorot relung dengan UV namun roda salah arah | NOT_RUN | Relung tetap tertutup, tidak menipu state |
| QA-057 | Selesaikan EVP, UV mata tertutup, roda benar, ambil cermin | NATIVE_PASS | Commit ID unik sebelum popup sukses — Bukti: `phase4-native-route.json`: EVP ketukan + UV mata + roda dua kali + pickup cermin |
| QA-058 | Force quit sewaktu commit pengambilan cermin | PARTIAL | Jika gagal, mirror tersedia lagi; tidak muncul notifikasi sukses palsu — Bukti: `DiskWriteFaultRollsBackTheWholeResourceTransactionAndPreservesFile(secret); bukan force-quit` |
| QA-059 | Mati/reload sesudah cermin sukses terkoleksi | PARTIAL | Mirror hilang dari world, ID tetap di secret_items — Bukti: `Secret unik diuji; pemulihan world prop belum native` |
| QA-060 | Uji 16 kombinasi kepemilikan P1/P2/P3/P4 | NOT_RUN | Arsip N/5 aktual, status lokal 0/1 atau 1/1 |
| QA-061 | Kendi diambil di makam Utara dalam keadaan kosong | NATIVE_PASS | Bisa dibawa ke Sumur Barat untuk diisi tanpa pembayaran — Bukti: `phase4-native-route.json`: Kendi diambil dengan air0, kemudian saluran sumur mengisi3 |
| QA-062 | Isi Kendi di Sumur Keramat hingga kapasitas | NATIVE_PASS | Set unit tepat sesuai config, tidak overfill — Bukti: `phase4-native-route.json`: Hold E pada REFILL_PENDOPO memberi air3 |
| QA-063 | Siram obor palsu dengan air terakhir | NOT_RUN | −40 Sanity, sumur dan pengisian bantu tetap tersedia |
| QA-064 | Coba siram dengan Kendi air=0 | NOT_RUN | Aksi ditolak dengan feedback, no negative amount |
| QA-065 | Taruh Kendi dalam offering lalu mulai ritual | ADAPTER_PASS | Kendi yang sama dapat dipegang kembali untuk menyiram tanpa duplikat — Bukti: `CP5PreservesExactBatterySanityAndActualWater (9 nilai)` |
| QA-066 | Setelah Ritual, buka inventory dan altar secara bergantian | PARTIAL | Tidak muncul dua Kendi aktif sebagai item berbeda — Bukti: `CP5 exact water/inventory; pergantian UI belum diuji` |
| QA-067 | Gunakan paket air toko ketika memiliki item | NOT_RUN | Mengisi Kendi cepat tanpa mengganti sumber air sumur |
| QA-068 | Selesaikan 4 obor dengan 0 paket air toko | NATIVE_PASS | Air gratis cukup, refill dapat diulang — Bukti: `phase4-native-route.json`: Empat obor selesai tanpa paket air; refill gratis setelah tiga sektor |
| QA-069 | Force quit sesudah refill sementara sebelum commit | PARTIAL | Restore air dan obor dalam snapshot konsisten — Bukti: `Fault water transaction diuji; force-quit refill belum diuji` |
| QA-070 | Banaspati mengejar saat pemain menuju sumur kosong | NOT_RUN | Ada jalur/cover ke tempat refill tanpa kematian tak terhindarkan |
| QA-071 | Render tiga obor Utara dari observasi aman | NOT_RUN | Tepat 1 bulat inti ungu dan 2 silang merah |
| QA-072 | Render tiga obor Timur dan Barat | NOT_RUN | Tepat 1 autentik dan 2 palsu pada tiap sektor |
| QA-073 | Render tiga obor Selatan saat final | PARTIAL | Tepat 1 autentik dan 2 palsu; bukan selalu tengah — Bukti: `Satu asli per sektor dan3preset diuji; render belum diperiksa` |
| QA-074 | Ganti seed layout lalu ulang run baru | ADAPTER_PASS | Lokasi obor autentik boleh berubah hanya antar seed run — Bukti: `TorchLayoutHasExactlyOneRealCandidatePerSectorAndPersists (3 preset)` |
| QA-075 | Save/reload setelah pemain melihat obor asli | ADAPTER_PASS | Layout/seed stabil pada save tersebut — Bukti: `TorchLayoutHasExactlyOneRealCandidatePerSectorAndPersists (3 preset)` |
| QA-076 | Sinar UV aktif saat player Sanity=0 | NOT_RUN | Bentuk segel world tetap terbaca tanpa tertutup UI palsu |
| QA-077 | Matikan UV lalu coba tebak berdasarkan chat | NOT_RUN | Desain masih memberi gameplay, tidak memberi info 100% lewat chat |
| QA-078 | Periksa penanda tengkorak ungu pada tanah dekat obor | NOT_RUN | Tetap berarti bahaya, tak tertukar dengan lingkar UV asli |
| QA-079 | Aktifkan mode buta warna/kontras rendah 720p | NOT_RUN | Silhouette bulat/silang/tengkorak terbedakan |
| QA-080 | Player menuang air ke dua decoy berturut-turut | NOT_RUN | Obor asli tetap mungkin diidentifikasi; decoy tidak berubah tipe |
| QA-081 | Pertama tiba Pendopo dan Banaspati reveal | NOT_RUN | Monster muncul jauh, telegraph, tanpa instakill spawn |
| QA-082 | Berjalan, sprint, jongkok pada jarak sama dari AI | NOT_RUN | Sensor suara berbeda sesuai baseline |
| QA-083 | Sanity 0 vs 100 saat Banaspati patroli | NOT_RUN | Radius investigasi meningkat tanpa LOS magis |
| QA-084 | Masuk liang kubur dengan layar HP mati sebelum AI melihat | NOT_RUN | AI melewati/menyelidik lalu pergi |
| QA-085 | Sembunyi di kolong gubuk saat Banaspati sudah mengejar | NOT_RUN | Player harus memutus LOS; hiding bukan invulnerability otomatis |
| QA-086 | Di dalam gentong aktifkan layar F saat Banaspati dekat | NOT_RUN | Stimulus cahaya sah memicu investigasi ancaman |
| QA-087 | Banaspati serang dari udara | NOT_RUN | Serangan punya suara/bayangan/telegraph ≥baseline |
| QA-088 | Wewe dari P3 selesai; masuk P4 | NOT_RUN | Tidak muncul AI P3 atau boss di scene P4 |
| QA-089 | Mbah Suro menulis chat “SYSTEM” palsu | NOT_RUN | Pemain tetap bisa melihat true world UV dan action binding asli |
| QA-090 | Banaspati dekat safe-anchor restore dan reload | NOT_RUN | AI spawn menjauh, ada grace untuk kontrol kembali |
| QA-091 | Masuk Pendopo ritual sebelum bukti/pusaka lengkap | PARTIAL | Batu ritual dapat diperiksa tetapi boss tidak dimulai — Bukti: `BeginRitual guards prasyarat di fixture; reticle native belum diuji` |
| QA-092 | Tiga pusaka terpasang namun Jurnal belum dikonfirmasi | NOT_RUN | Aktivasi ditolak dengan daftar prasyarat yang kurang |
| QA-093 | Konfirmasi ritual setelah semua syarat benar | NATIVE_PASS | CP5 commit sukses sebelum threat memulai fase aktif — Bukti: `phase4-native-route.json`: Tiga ALTAR dipasang melalui E, RITUAL dikonfirmasi dan CP5 aktif |
| QA-094 | Fail write CP5 sebelum cutscene ritual | NOT_RUN | Ritual tidak dimulai, player bisa retry save |
| QA-095 | Urutan pemadaman pertama N lalu E lalu W | ADAPTER_PASS | Ketiga sektor dapat diterima; total tepat 3 local Blitz — Bukti: `AllSixSectorOrdersReloadWithProgressThenExactlyOneCoreSeal (6 urutan)` |
| QA-096 | Urutan pemadaman pertama W lalu N lalu E | NATIVE_PASS | Ketiga sektor dapat diterima dengan progres set sama — Bukti: `phase4-native-route.json`: Ritual W→N→E berhasil |
| QA-097 | Siram obor autentik Utara lalu miss Blitz | PARTIAL | Obor recoverable, boleh isi air ulang dan retry — Bukti: `PaidMissAndCoreTimeoutDoNotEraseFinishedSectors; AI/range retry belum native` |
| QA-098 | Spam Klik Kiri berkali-kali dalam satu exposure | NOT_RUN | Maksimal satu sector hit dan biaya Blitz valid sesuai input policy |
| QA-099 | Menang sektor Timur saat sektor Utara telah committed | NATIVE_PASS | Sektor Utara tetap permanen off; tidak memberi seal dua kali — Bukti: `phase4-native-route.json`: N lalu E setelah W committed; semua receipts tetap ada |
| QA-100 | Coba memicu local Blitz di sektor sudah selesai | NOT_RUN | Tidak boleh menggandakan poin, baterai, atau receipt |
| QA-101 | Masuk Selatan sebelum ketiga sektor N/E/W sukses | ADAPTER_PASS | Sektor final belum mengizinkan ritual/menampilkan gate wajar — Bukti: `SouthCannotStartEarlyAndLocalOrCoreBlitzNeedsExactPrerequisites` |
| QA-102 | Dengan 3 sektor sukses, balik Selatan tanpa kemenyan | NOT_RUN | Shelter netral dan UV memungkinkan identifikasi asli |
| QA-103 | Dengan Sanity 0 di Selatan dan tanpa water pack | NOT_RUN | Shine UV + shelter + refill sumur tetap bisa menyelesaikan |
| QA-104 | Siram obor palsu Selatan saat kejaran | NOT_RUN | −40 sanity ter-clamp ke 0, telegraph, tidak instakill |
| QA-105 | Siram obor asli Selatan setelah tiga local hit | NATIVE_PASS | Hanya SOUTH_OFF, tidak memicu Blitz lokal keempat — Bukti: `phase4-native-route.json`: SOUTH_OFF sesudah tiga lokal tanpa hit lokal tambahan |
| QA-106 | Periksa jumlah seal hits sesudah SOUTH_OFF | NATIVE_PASS | Tiga local hits, core belum completed; bukan empat — Bukti: `phase4-native-route.json`: Hit count tetap3 sesudah Selatan |
| QA-107 | Mbah Suro + Banaspati bergabung di Pendopo | NATIVE_PASS | Satu exposed core, bukan dua enemy health bar  — Bukti: `phase4-native-route.json`: Agent bergerak dan merge inti di Pendopo |
| QA-108 | Blitz miss pada core saat baterai rendah | NOT_RUN | Core exposure bisa diulang setelah charger fisik; sektor tak reset |
| QA-109 | Blitz final valid pada core | NATIVE_PASS | Exactly fourth hit, segel lokasi keempat dikomit CP7 atomik — Bukti: `phase4-native-route.json`: Blitz keempat + lokasi segel keempat tersimpan |
| QA-110 | Player mencoba Blitz kelima setelah menang | ADAPTER_PASS | Tidak menambah segel/biaya Blitz/target boss; foto biasa tetap1% sesudah boss RESOLVED — Bukti: `AllSixSectorOrdersReloadWithProgressThenExactlyOneCoreSeal (repeat core ditolak)` |
| QA-111 | Setelah sektor pertama N sukses, force quit | PARTIAL | Continue CP6 dengan N selesai dan dua sektor lain aktif — Bukti: `CP6 reload diuji; belum process-kill` |
| QA-112 | Setelah sektor pertama W sukses, reload | NATIVE_PASS | W selesai, N dan E tetap bisa urutan bebas — Bukti: `phase4-native-route.json`: Reload seluruh scene CP6 setelah W; W dan Cermin tetap ada |
| QA-113 | Setelah N+W selesai, kalah oleh Banaspati | PARTIAL | Kedua sector receipts tetap utuh; tidak kembali CP5 kosong — Bukti: `CP6 reload diuji; belum mati oleh AI nyata` |
| QA-114 | Setelah tiga sektor selesai, ambil relung rahasia | NOT_RUN | Pickup menulis ke checkpoint tertinggi tanpa menurunkan CP6 |
| QA-115 | Force quit tepat setelah siram obor tapi sebelum Blitz | NOT_RUN | Tidak memuat obor padam dan air bertentangan |
| QA-116 | Force quit saat TryCommit lokal sukses | PARTIAL | Disk merepresentasikan satu keadaan penuh atau rollback — Bukti: `DiskWriteFaultRollsBackTheWholeResourceTransactionAndPreservesFile(hit); bukan process-kill` |
| QA-117 | CP4 pickup dilakukan saat Banaspati chase di dekat makam | NOT_RUN | Restore anchor aman, AI tidak spawn tepat di atas Raka |
| QA-118 | Load CP5 dengan layout seed permanen | ADAPTER_PASS | Susunan asli/palsu tidak berubah secara tidak sah — Bukti: `TorchLayoutHasExactlyOneRealCandidatePerSectorAndPersists` |
| QA-119 | Gagal CP7 write saat final Blitz | NOT_RUN | Tidak membuka portal/segel tanpa commit, ada retry yang jujur |
| QA-120 | CP8 commit sukses lalu load Continue | NATIVE_PASS | Routing ke boundary fisik Pasar Setan setelah CP8 tanpa Kos/epilog ulang; full scene P5 dipasang ketika babak5 diimplementasikan — Bukti: `phase4-native-route.json`: CP8 tersimpan, capsule menuruni tangga nyata dan loader mengembalikan BOUNDARY |
| QA-121 | Migrate save valid P3 ke P4 loader terbaru | PARTIAL | P0–P3, flags, Arsip, ekonomi, inventori dipertahankan — Bukti: `OldSchemasMigrateWithoutErasingPreviousEvidenceOrInventory(1–4); tes regresi global menyusul` |
| QA-122 | Uji snapshot P3 tanpa bukti rahasia terdahulu | NOT_RUN | Count Arsip P4 tidak diasumsikan 4/5 |
| QA-123 | Simulasikan disk penuh saat transaksi Kemenyan | PARTIAL | Rollback saldo sah dan instance, tidak negatif — Bukti: `Write fault purchase PB; bukan disk penuh saat kemenyan` |
| QA-124 | Simulasikan disk penuh saat pickup bukti makam | NOT_RUN | Tidak menghilangkan prop, ID belum dianggap committed |
| QA-125 | Simulasikan disk penuh saat secret Cermin | PARTIAL | Popup tidak muncul dan prop tersedia untuk retry — Bukti: `Write fault secret; popup/world retry belum native` |
| QA-126 | Siram obor benar, simpan di sela panel Jurnal | NOT_RUN | State transient dan data persisten tidak mengandung dua sumber benar |
| QA-127 | Double click konfirmasi turun portal | NOT_RUN | Satu P4_COMPLETE, satu CP8, boundary fisik pada scene P4 tanpa scene P5 fiktif |
| QA-128 | Load setelah 0% Sanity dan 0% battery | PARTIAL | Value tepat snapshot; charger/shrine tetap dapat dicapai — Bukti: `CP5 exact battery termasuk0 + scene recovery; belum sanity0 native` |
| QA-129 | Periksa wallet_bound Rp6.666.000 setelah reload | NATIVE_PASS | Donasi satu kali, tidak masuk wallet_legitimate  — Bukti: `phase4-native-route.json`: Saldo bound6.666.000 tetap satu kali setelah reload scene dan CP8 |
| QA-130 | Bandingkan save CP7 dan CP8 | NATIVE_PASS | CP7 boss won; CP8 exit committed; tidak ada status setengah jadi — Bukti: `phase4-native-route.json`: CP7 kemenangan mendahului CP8 konfirmasi; P4_COMPLETE dan anchorBOUNDARY tersimpan |
| QA-131 | Profil 720p Low pada hutan bambu saat kabut dan Sanity LOW | NOT_RUN | Target ≥30 FPS avg, 1% low ≥24 FPS, keterbacaan terjaga |
| QA-132 | Profil 720p Low saat 12 obor dan Banaspati api aktif | NOT_RUN | Shadow/overdraw tidak melampaui budget target |
| QA-133 | Profil FPS saat UV v2 dan Live Chat bersamaan | NOT_RUN | Tidak membuat camera kedua/draw calls berlipat tak wajar |
| QA-134 | Uji subtitle ketukan sumur, bambu, Banaspati dari jarak aman | NOT_RUN | Cues lokal dan pembicara dapat dibaca tanpa audio |
| QA-135 | Uji EVP saat ambience bambu dan suara tiruan bertumpuk | NOT_RUN | Sumber authored authentik tetap bisa dipilih dengan arah kamera |
| QA-136 | Lihat simbol asli/palsu/tengkorak dengan simulasi buta warna | NOT_RUN | Dibedakan oleh bentuk dan kontras, bukan hanya hue |
| QA-137 | Matikan chromatic aberration/distorsi berat dari Settings | NOT_RUN | Sanity tetap berpengaruh tetapi clues tidak lenyap |
| QA-138 | Audit semua asset imported Desa dan Banaspati | ASSET_PASS | URL/lisensi/source identity dan penempatan: `phase4-asset-verification.json`,116 model /632 penempatan /1465 hash; mutu artistik terpisah. |
| QA-139 | Bandingkan visual Kos P4 dengan Golden Scene P1/P3 | NOT_RUN | Scene bukan duplikat, furniture transform/lighting identitas tetap |
| QA-140 | Windows build dengan laptop Intel Iris Xe RAM8GB, bermain ±78 menit | NOT_RUN | Kualitas visual/FPS/VO diverifikasi nyata; headless tidak dipakai mengklaim PASS |
| QA-141 | Selesaikan Run A tanpa toko, tanpa baca live chat, tanpa rahasia | NOT_RUN | P4_COMPLETE dan CP8 tercapai tanpa paywall |
| QA-142 | Selesaikan Run B dengan semua paket dan Cermin Saksi | NOT_RUN | Progress akhir sama secara main path, Arsip bertambah tepat 1 |
| QA-143 | Ikuti jalur bebas W→E→N saat investigasi & ritual | NOT_RUN | Tidak ada objektif mundur; Selatan tetap terakhir |
| QA-144 | Lakukan ritual dengan Sanity 0 selama beberapa sektor | NOT_RUN | Dengan recovery fisik, kemenangan mungkin; 0 bukan game over otomatis |
| QA-145 | Selesaikan empat obor dan hit counter | NATIVE_PASS | Tiga local + satu core = empat; tidak ada tembakan kelima  — Bukti: `phase4-native-route.json`: Tiga lokal + satu inti = empat; final seal count4 |
| QA-146 | Periksa tahun lore 1997, Mbah Suro wafat sesudah 2004 | NOT_RUN | Teks forum/dokumen/cermin tidak berkontradiksi tak sengaja |
| QA-147 | Periksa pembuka 780k, penutup 999.999, donasi dan kontrak | NOT_RUN | Semua angka kanon muncul sesuai konteks dan ledger tidak terkontaminasi |
| QA-148 | Setelah final boss, kembali ke Sumur untuk Cermin jika terlewat | NOT_RUN | Jalur aman, puzzle berfungsi, tidak restart boss |
| QA-149 | Pada panel portal pilih Kembali Menjelajah lalu turun nanti | NOT_RUN | Tidak ada P4_COMPLETE saat cancel; commit hanya saat konfirmasi |
| QA-150 | P4_COMPLETE ke P5 malam sama dengan baterai 0/sanity 0 | PARTIAL | Tidak muncul Kos palsu; boundary entry Pasar Setan mempunyai charger/perlindungan fisik tanpa softlock; seluruh gameplay P5 di luar lingkupP4 — Bukti: `CP8 boundary routing + scene stairs/recovery; belum native0/0` |
| QA-151 | Coba `G` saat membawa 4 flare lama dari P3 | NOT_RUN | Flare tidak dilempar, stok tidak dikurangi, pesan “Api asing ditolak desa”; masih tersedia untuk P5 |
| QA-152 | Verifikasi layout standar obor asli N/E/W/S | NOT_RUN | Default authored N=Tengah, E=Kiri, W=Kanan, S=Kiri; tanpa duplikat tanda autentik |
| QA-153 | Muat CP5 berulang dan periksa preset/seed obor | PARTIAL | Posisi asli tidak berpindah karena retry, kamera berubah, menu, atau force-quit — Bukti: `Preset stabil di reload adapter; belum force-quit pada semua fase` |
| QA-154 | Bermain dengan preset opsional dan simpan seed | PARTIAL | Seed dipilih sekali pra-ritual; pemilihan tidak berubah saat CP6 rolling — Bukti: `Preset0–2 tersimpan dan6urutan sektor; belum visual retry` |
| QA-155 | Uji tiga cabang hutan Claude dalam urutan J1–J3 | NOT_RUN | J1 kanan, J2 lurus, J3 kanan; jalan salah tetap punya UV + audio telegraph dan route recovery |
| QA-156 | Baterai 0 sebelum simpang J1 pada save valid | PARTIAL | Charger darurat sebelum J1 reachable tanpa UV; tak wajib beli powerbank atau reset Kos — Bukti: `Scene recovery sebelumJ1 tersedia; belum battery0 native` |
| QA-157 | Sanity 0, tanpa kemenyan, di hutan sebelum Pendopo | NOT_RUN | Perlindungan fisik gratis/waypoint aman dapat dicapai walau entitas agresif |
| QA-158 | Drain Sanity pasif 20 menit tanpa kontak ancaman | NOT_RUN | Tidak turun di bawah 35% di hutan / 45% area desa karena waktu saja; kontak/event tetap bisa ke 0 |
| QA-159 | Gunakan Kemenyan sekali dengan Sanity 30 | PARTIAL | Pulih ke100% setelah penggunaan selesai, cooldown12detik; satu instance dan transaksi sesuai kontrak global — Bukti: `Adapter incense100/cooldown; aksi2s belum native` |
| QA-160 | Sanity 0 saat masuk shelter lumbung tanpa Kemenyan | NOT_RUN | Bisa naik bertahap sampai maksimal 70 menurut source physical; tidak kebal damage di luar shelter |
| QA-161 | Banaspati mengenai player dua kali dalam window 10 s | NOT_RUN | Damage pertama, knockback, i-frame 3 s; hit kedua valid sesuai konfigurasi, tidak terjadi double-hit di satu frame |
| QA-162 | Banaspati mengejar player dengan LOS terputus | NOT_RUN | SEARCH 6 s lalu lepas, tidak mengetahui lokasi player permanen |
| QA-163 | Client 30 FPS dan 60 FPS: telegraph semburan Mbah Suro | NOT_RUN | Durasi ≥1,2 s, petunjuk tetap terbaca dan dodge memungkinkan pada kedua FPS |
| QA-164 | Chat asli mengirim klaim yang salah dan hantu mengirim klaim benar | NOT_RUN | Tidak ada sistem auto-label benar dari username; UV world tetap satu-satunya oracle obor |
| QA-165 | Chat 40 pesan dan high Sanity; ukur clipping/rate limit | NOT_RUN | Maksimum 1 pesan/1,2 s saat chase; reticle dan prompt `E` tidak tertutup |
| QA-166 | Kemenyan dipakai saat chat palsu aktif | NOT_RUN | Distorsi chat mereda 20 s baseline; membaca clue dunia masih mungkin tanpa kemenyan |
| QA-167 | Kendi teraktivasi setelah offering tiga pusaka | ADAPTER_PASS | Tepat satu ID inventori aktif, kapasitas3 dan isi aktual0–3; offering tidak menambah air, sumur mengisi secara fisik — Bukti: `CP5PreservesExactBatterySanityAndActualWater` |
| QA-168 | Salah siram obor palsu saat stok 1 | NOT_RUN | Stok jadi 0 satu kali, Sanity −40, transaksi atomic; reload tidak memberi air kembali secara ajaib |
| QA-169 | Save gagal tepat ketika pemakaian Garam | NOT_RUN | Item dan volume air rollback bersama; tidak ada duplikasi garam atau air |
| QA-170 | Hit sektor N sukses kemudian mati di sektor E | NOT_RUN | N tetap committed, E masih aktif; CP6 aman; tidak memulai ulang N |
| QA-171 | Hit sektor E lalu W (N belum) dan Continue | PARTIAL | Receipts E/W retained, N tersisa; Selatan tetap terkunci — Bukti: `Semua6urutan/reload diuji; belum input Continue native` |
| QA-172 | Force-quit sesudah siram asli namun sebelum Blitz | NOT_RUN | Air/torch state dipulihkan dari commit terakhir; tidak ada obor mati gratis atau biaya ganda |
| QA-173 | Force-quit setelah obor S off dan sebelum core Blitz | NOT_RUN | `south_torch_off` tetap ada; restore menuju core; tidak ada local S Blitz |
| QA-174 | Save gagal saat sektor W valid | PARTIAL | Tidak tampil keberhasilan persisten palsu; rollback receipt/baterai sesuai owner global — Bukti: `Write-fault hit atomik; notifikasi runtime belum native` |
| QA-175 | Sukses core lalu save CP7 gagal | NOT_RUN | Portal/kemenangan tidak dicatat sebagai sukses sampai commit; recover sektor yang sah dan ulang core |
| QA-176 | Sukses CP7, ambil Cermin rahasia, keluar P5 | NOT_RUN | Secret ID tetap ada dan CP8 tidak menurunkan anchor/merge boss |
| QA-177 | Dari P4 CP8 masuk P5 dengan Sanity 0% & baterai 0% | PARTIAL | Masuk boundary fisik Pasar Setan tanpa Kos, reset nilai, atau scene load yang belum ada; charger/shrine entry tersedia — Bukti: `CP8 boundary dan recovery fisik tersedia; belum route0/0` |
| QA-178 | Uji tinggi/luas 4 sektor pada Intel Iris Xe target | NOT_RUN | Jalur LOS dan UV bisa dibaca di 720p Low 30 FPS target; penurunan partikel tidak menghapus telegraph |
| QA-179 | Revisi script chat pada Sanity rendah menampilkan `[SYSTEM]` palsu | NOT_RUN | Tidak ada pemanggilan action dispatcher, perubahan input binding, atau checkpoint dari teks chat |
| QA-180 | Jalankan QA lintas Phase 1–3 pascamigrasi dan CP6 rolling P4 | NOT_RUN | Save P0–P3, inventori/arsip, uang dan checkpoint lama tidak diregresi |
