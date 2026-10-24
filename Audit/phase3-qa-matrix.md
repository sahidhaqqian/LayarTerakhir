# Phase 3 — matriks 146 acceptance case

Setiap status dibatasi bukti: PASS_UNIT menguji kontrak data/scene, PASS_DIRECTED memakai player Linux dengan input produksi dan capsule yang diarahkan skrip, PASS_REPO memeriksa artefak; PARTIAL masih membutuhkan bagian acceptance lain. NOT_RUN berarti belum diuji. Tidak satu pun status tersebut membuktikan kualitas Windows, pacing, atau keseluruhan 146 acceptance case. Bukti akhir dan keterbatasan ada di phase3-implementation.md.

| ID | Kondisi | Status | Bukti / yang masih diperlukan |
|---|---|---|---|
| QA-01 | Save P2_COMPLETE plus segel RSUD → masuk P3 | PASS_DIRECTED | phase3-native-main-result.json: save P2 lengkap valid → Golden Kos → factory melalui transaksi kampanye produksi. |
| QA-02 | Save tanpa P2_COMPLETE ditolak | PASS_UNIT | Phase3CampaignTests.EnterHubRequiresRealPreviousChapterReceiptsAndBothSeals: gate receipt P2. |
| QA-03 | P2_COMPLETE tanpa segel RSUD | PASS_UNIT | EnterHubRequiresRealPreviousChapterReceiptsAndBothSeals: dua segel wajib, tanpa grant. |
| QA-04 | New Game vs Continue P3 | PASS_UNIT | NewGameDoesNotInheritPhase3AndSavedContinueRemainsExplicit. |
| QA-05 | Kamera V pada P3 | NOT_RUN | Runtime: V tetap ganti perspektif OTS/FP, tidak menjadi Selfie |
| QA-06 | Selfie X pada P3 | NOT_RUN | Runtime: X sesuai binding, tidak menghapus UV R atau recorder C |
| QA-07 | Tahan lalu lepas RMB | PARTIAL | Native: RMB nyata dari inventory memaksa kamera, release memulihkan app/perspektif. Akurasi jari/pose/komposisi tetap review Windows. |
| QA-08 | Modul P3 unlock dua kali | PARTIAL | Unlock/keberangkatan tanpa biaya diuji unit/native; deduplikasi memakai Contains pada sistem upgrade existing. Klik modul berulang melalui UI perlu playtest. |
| QA-09 | Kos Golden Scene bukan versi baru 4×3 | PASS_REPO | Phase3Hub dipasang di Golden Kos existing; builder hanya menyalin sistem kampanye untuk factory. |
| QA-10 | Rute motor Kos ke pabrik | PARTIAL | Transaksi berangkat dan CP1 factory diuji native; interaksi motor/hub dan komposisi transisi 23:45→23:54 masih review manusia. |
| QA-11 | Shop tanpa uang sah | PASS_UNIT | ZeroWalletUnlockAndExactDepartureDoNotInventResources; shop tidak wajib. |
| QA-12 | Paket powerbank besar 3 unit | PASS_UNIT | PackPurchaseIsOneTransactionAndOneReceipt(POWERBANK,750000,3), termasuk reload dan pembelian ulang. |
| QA-13 | Paket flare 4 unit | PASS_UNIT | PackPurchaseIsOneTransactionAndOneReceipt(FLARE,1000000,4). |
| QA-14 | Paket kemenyan 2 unit | PASS_UNIT | PackPurchaseIsOneTransactionAndOneReceipt(KEMENYAN,350000,2). |
| QA-15 | Pembelian gagal saat atomic save | PASS_UNIT | ShopWriteFailureRollsBackBalanceItemsAndReceiptTogether, fault injection filesystem. |
| QA-16 | 0% baterai di pelataran | NOT_RUN | Runtime: Layar normal mati, charger fisik dapat diakses, tidak game over otomatis |
| QA-17 | AR atau UV normal drain | NOT_RUN | Unit: Total profil aktif 0,10 poin/s, bukan kamera 0,05 ditambah 0,10 lagi |
| QA-18 | Foto bukti berulang | NOT_RUN | Unit: 1% per aksi foto valid, bukti hanya dihitung sekali |
| QA-19 | Blitz 15% dan cooldown 6 detik | NOT_RUN | Unit: Tidak bisa serang sah jika baterai kurang atau masih cooldown |
| QA-20 | Milestone donasi Rp312,5 juta | PARTIAL | Native ending dan CP7 mempertahankan saldo sah0; milestone naratif tidak diberikan sebagai uang belanja. Tampilan overlay perlu review. |
| QA-21 | Forum dilewatkan | NOT_RUN | Playtest: EVP/UV tetap terajar lewat lingkungan; tidak ada gate yang membutuhkan baca forum |
| QA-22 | Chat/live tidak dibuka | NOT_RUN | Playtest: Petunjuk rute aman dan generator tetap tersedia dari lingkungan |
| QA-23 | Damar palsu dari kiri | NOT_RUN | Runtime: EVP merah bergerigi, TIRUAN, sound source konsisten |
| QA-24 | Warna EVP dimatikan/high contrast | NOT_RUN | Accessibility: Teks TIRUAN/ASLI dan pola gelombang berbeda tetap terbaca |
| QA-25 | UV rajah X pada kiri | NOT_RUN | Visual test: Semua papan rapuh punya mark X yang konsisten saat UV aktif |
| QA-26 | Cabang kanan tanpa lompatan wajib | PASS_DIRECTED | Capsule produksi melintasi jalur kanan sampai G-ACC tanpa jump; bukan penilaian rasa kontrol manusia. |
| QA-27 | Spawn 0% sebelum jembatan | NOT_RUN | Runtime: Charger manual di landing aman tanpa menyeberang papan rusak |
| QA-28 | Mati jatuh ke gilingan | NOT_RUN | Runtime: Retry di CP aman, tidak spawn di lubang/collider mati |
| QA-29 | Panel gilingan masuk dari jalur kanan | PASS_DIRECTED | Jalur kanan, raycast reticle, dan tiga E crank produksi berhasil di standalone Linux. |
| QA-30 | Jembatan kiri terpicu saat speed berbeda | NOT_RUN | Playtest: Telegraph audio/visual berjalan, collider dan kill volume sinkron |
| QA-31 | Tarikan G-ACCESS 1/3/2 ulang | PARTIAL | Tiga crank bertempo berhasil; urutan spam/interupsi1/3/2 belum disimulasikan seluruhnya. |
| QA-32 | G-ACCESS selesai lalu reload | NOT_RUN | Runtime: Gerbang tetap aktif, lampu pintu menyala dan tarikan tidak mengulang reward |
| QA-33 | G-ACCESS dibanding G-N | PASS_REPO | CrankAccess hanya mengubah receipt akses; array Spent dan token boss dimiliki Phase3Boss terpisah. |
| QA-34 | G-ACCESS bukan pengisi baterai HP | PASS_REPO | CrankAccess tidak memanggil RestoreBattery/ChargeArena; charger memiliki interaksi lain. |
| QA-35 | Pintu terbuka saat player melintasi ambang | NOT_RUN | Runtime: Tidak menjepit player bila collider/animasi pergantian state |
| QA-36 | Foto ayunan sebelum mengambil boneka | PASS_DIRECTED | AR dwell dan LMB menangkap bukti, kemudian E mengambil boneka; evidence tidak memberikan pusaka otomatis. |
| QA-37 | Ambil boneka sebelum foto ayunan | NOT_RUN | Runtime: Ayunan masih dapat difoto; bukti tidak otomatis ada |
| QA-38 | Foto ayunan tidak fokus | NOT_RUN | Unit: Ray/dwell gagal tidak menambah set evidence |
| QA-39 | Membaca log Mandor di ayunan | PASS_DIRECTED | Buku Mandor diambil melalui reticle/E produksi; dokumen tersimpan dalam campaign yang kemudian dimuat sebagai CP7. |
| QA-40 | Kembali ke gilingan setelah masuk ketel | NOT_RUN | Playtest: Gerbang akses dua arah, jalur tidak menutup sendiri |
| QA-41 | Wewe reveal pertama | NOT_RUN | Playtest: Audio/visual telegraph mendahului kendali ancaman fatal |
| QA-42 | Wewe spawn di koridor | NOT_RUN | Runtime: Tidak spawn tepat di tubuh Raka atau titik checkpoint |
| QA-43 | Loker besi dengan F | NOT_RUN | Runtime: Stimulus cahaya mati; AI boleh mencari last-known dan pergi |
| QA-44 | Loker dengan HP menyala | NOT_RUN | Runtime: Cahaya menarik Wewe sesuai LOS; tidak invisibility gratis |
| QA-45 | Berdiri terang di depan Wewe walau F | NOT_RUN | Runtime: LOS masih berlaku, layar mati bukan perlindungan mutlak |
| QA-46 | AI kehilangan player | NOT_RUN | Runtime: Kembali patroli dalam batas waktu, tidak camp permanen di pintu wajib |
| QA-47 | Catwalk dua celah pada kontrol Raka aktual | PARTIAL | Rute kantor bertingkat dan kembali dilewati capsule; jalur NavMesh terhubung. Rasa kontrol/tangga Windows belum diterima. |
| QA-48 | Wewe melintas 2 tingkat | PARTIAL | Navigasi fisik dua tingkat diuji dan render model tersedia; animasi/ceiling transition saat gameplay Windows belum direview. |
| QA-49 | Wewe mengejar saat membaca dokumen | NOT_RUN | Playtest: Interaksi punya kondisi adil dan recovery; tidak memberi save fatal |
| QA-50 | Masuk kembali bangsal setelah boss | NOT_RUN | Runtime: Wewe tetap RESOLVED, pemain bisa mencapai kantor mandor untuk arsip |
| QA-51 | Interkom sumber benar EVP v2 | PASS_DIRECTED | Interkom actual C hold6.4s setelah segmen ASLI menambah ID EVP; bukan rekaman mikrofon. |
| QA-52 | Bunyi tiruan Damar di kantor | PARTIAL | IntercomMetadataIsDeterministicAndNeverUsesMicrophone memverifikasi segmen Mimic. Review waveform/audio manusia belum selesai. |
| QA-53 | EVP terputus di tengah | PASS_DIRECTED | HoldC .9s lalu release tidak memberikan evidence; rekaman baru kemudian berhasil. |
| QA-54 | EVP v2 tanpa mikrofon OS | PARTIAL | Metadata authored, tidak mengakses Microphone; uji instalasi offline Windows belum dijalankan. |
| QA-55 | Sabit diambil sebelum EVP | PASS_DIRECTED | Sabit diambil sebelum EVP, dan bukti interkom tetap harus direkam sendiri. |
| QA-56 | EVP selesai sebelum sabit diambil | NOT_RUN | Runtime: Bukti tidak otomatis memberikan sabit, tetap tersedia |
| QA-57 | Rajah cerita UV pada bagasse | PASS_DIRECTED | Lensa UV, LOS/dwell dan LMB produksi menambahkan bukti rajah; pusaka tetap diambil terpisah. |
| QA-58 | Foto ulang rajah | NOT_RUN | Unit: Counter tetap 3 ID maksimum; tidak menduplikasi dari photo album |
| QA-59 | Selendang diambil sebelum foto | NOT_RUN | Runtime: Dinding tetap bisa dipindai, bukti tetap tersedia |
| QA-60 | Damar card dan sepatu kiri | PARTIAL | Kartu pers dan sepatu diambil melalui E terpisah; kartu tidak memberikan sepatu. Cabang tanpa sepatu/pose final belum lulus. |
| QA-61 | Tiga ketukan vent tanpa EVP | NOT_RUN | Runtime: Katup 09 tidak memberi collectible tanpa validasi sumber yang benar |
| QA-62 | EVP tiga ketukan hijau | PASS_DIRECTED | phase3-native-secret-partial-result.json: ketukan VENT09 direkam independen; keseluruhan rute detour berakhir tertangkap AI, bukan PASS end-to-end. |
| QA-63 | Suara Damar merah di balkon | NOT_RUN | Runtime: TIRUAN tidak membuka loker, tidak memaksa masuk jalur rapuh |
| QA-64 | UV pagar menampilkan tulisan | PARTIAL | Clue UV tercapai melalui input nyata pada detour dan gambar balkonUV tersedia; keterbacaan Windows perlu review. |
| QA-65 | Panah UV ke katup 09 | PARTIAL | Detour mencapai katup09 dari balkon tanpa jump; seluruh fairness jalur manusia/Wewe tetap terbuka. |
| QA-66 | Katup salah / E spam | NOT_RUN | Unit: Tidak membuka loker 09 atau menggandakan reward |
| QA-67 | Pelat diambil satu kali | PARTIAL | Pickup pelat tersimpan dalam route detour melalui E; replay berulang/reload pasca-pickup belum diuji sebagai acceptance lengkap. |
| QA-68 | P1/P2 rahasia dilewatkan | NOT_RUN | Unit: Setelah P3 pickup Arsip bisa 1/5, tidak otomatis 3/5 |
| QA-69 | Autosave pelat gagal | NOT_RUN | Fault injection: Pickup belum sukses dan pelat tetap ada untuk dicoba kembali |
| QA-70 | Ambil pelat setelah boss | NOT_RUN | Runtime: Rute ke balkon tersedia sebelum motor complete dan secret tetap permanen |
| QA-71 | Pintu aula dengan kurang satu pusaka | PARTIAL | ExactGateRequirementsCannotBeSubstituted(relic). Prompt dan setiap variasi rute perlu playtest. |
| QA-72 | Pintu aula setelah 3 pusaka | PARTIAL | Tiga pusaka dan Jurnal membuka hall; altar dan E decoy berhasil. Seluruh variasi bolak-balik sebelum start belum diuji. |
| QA-73 | Tiga bukti tanpa konfirmasi Jurnal | PASS_UNIT | ExactGateRequirementsCannotBeSubstituted(journal/evidence/relic). |
| QA-74 | Duduk altar tanpa 3 pusaka | NOT_RUN | Runtime: Preview ditolak jelas; tidak ada inventory hilang |
| QA-75 | Pasang preview lalu tutup inspeksi | NOT_RUN | Unit: Pusaka tetap tersedia dan pintu terbuka |
| QA-76 | Boneka Damar tanpa E | NOT_RUN | Runtime: Boss tetap idle walau dekat/menoleh/pergi |
| QA-77 | CP5 commit gagal | NOT_RUN | Fault injection: Tidak terjadi scene lock, tidak terjadi transfer pusaka/AI |
| QA-78 | CP5 sukses lalu die midfight | PARTIAL | TwentyAbortedAttemptsRestoreWholeCP5AndBlockEveryOrdinaryWrite; kematian fisik setiap lokasi belum seluruhnya diuji. |
| QA-79 | Continue setelah crash di boss | PASS_UNIT | TwentyAbortedAttemptsRestoreWholeCP5AndBlockEveryOrdinaryWrite: rollback seluruh snapshot dan owner lock. |
| QA-80 | CP5 save ke slot global existing | PASS_REPO | CampaignState schema4 dan attempt owner existing; tidak ada save manager paralel. |
| QA-81 | Tanpa flare sama sekali | PASS_DIRECTED | 77 assertion main: generator N→S→E memberi tiga hit dan CP6, tanpa belanja/flare. |
| QA-82 | Cahaya aktif saat Wewe di luar cone | NOT_RUN | Unit: Sumber tidak rusak, cooldown lalu bisa dicoba kembali |
| QA-83 | Lampu mengenai ilusi suara saja | NOT_RUN | Unit: Boss tidak menjadi exposed tanpa overlap bos nyata |
| QA-84 | Wewe masuk cone | PASS_DIRECTED | Wewe benar-benar dipancing ke setiap beam; E menyalakan sumber, shield exposed lalu Blitz sukses. |
| QA-85 | Miss Blitz dalam window | PARTIAL | Budget:20 miss saat Stalk tidak menyegel atau membakar generator. Miss di dalam window Exposed spesifik belum diuji. |
| QA-86 | Timeout 5 detik | NOT_RUN | Unit: Lampu utuh, player dapat lure ulang setelah cooldown |
| QA-87 | Sukses pada lampu N | PASS_DIRECTED | HitN memberi satu lapisan; S dan E masih dapat digunakan kemudian untuk dua lapisan lain. |
| QA-88 | Satu lampu dicoba untuk hit kedua | NOT_RUN | Unit: Hit sukses tidak dihitung lagi dari sumber USED |
| QA-89 | Urutan generator E-N-S | NOT_RUN | Playtest: Tiga lampu mana pun urutannya membolehkan finish |
| QA-90 | Blitz kurang 15% saat exposed | NOT_RUN | Runtime: Tidak menyegel, charger recover path tetap tersedia |
| QA-91 | Masuk boss baterai 0% | PASS_DIRECTED | phase3-native-budget-result.json: fixture boss eksplisit 0%, ritual CP5 nyata, rak daya ditahan delapan detik memulihkan +25. |
| QA-92 | Masuk boss baterai 1%, 14%, 15% | PARTIAL | CP5PreservesExactEnergyAndInventory menguji 0/1/14/15/29/44/45/80/100; bukan seluruh variasi boss fisik. |
| QA-93 | Miss Blitz berulang sampai 0% | PASS_DIRECTED | Dua puluh miss nyata memakai LMB/energi produksi dan reset alcove. Ini fixture boss sintetis, bukan keseluruhan investigasi. |
| QA-94 | Dua charger arena re-arm per interlude | PARTIAL | Native budget memverifikasi satu token bersama habis dan dapat direset manual. Re-arm setelah dua hit menang memerlukan full-route. |
| QA-95 | Kabel kain Wewe tarik cycle3 tanpa flare | PASS_DIRECTED | Siklus3: real UV aim berkelanjutan memutus simpul; E-lamp tetap diperlukan untuk lapisan ketiga, tanpa flare. |
| QA-96 | Siklus 3 dengan flare tersisa | PARTIAL | VictoryConsumesOnlyAttemptFlaresAndKeepsOldChapterSealsEconomyAndSanity memverifikasi konsumsi commit; deployment fisik perlu uji. |
| QA-97 | Flare miss/timeout | NOT_RUN | Runtime: Flare habis untuk attempt ini; generator belum sukses tetap tersedia |
| QA-98 | Tiga segel sukses lalu CP6 gagal | PASS_UNIT | CP6WriteFaultRollsBackEveryFieldAndCanRetry: tidak menambahkan segel sebelum commit berhasil. |
| QA-99 | Runtuh lantai semasa pause/window | NOT_RUN | Runtime: Timer tidak bergerak ketika input/gameplay tak tersedia |
| QA-100 | Siklus boss pada FPS 30 dan 60 | NOT_RUN | Windows playtest: Window 5s dan overlap beam konsisten terhadap frame rate |
| QA-101 | Boss menang menyegel sekali | PASS_UNIT | VictoryConsumesOnlyAttemptFlaresAndKeepsOldChapterSealsEconomyAndSanity: segel terdahulu, ekonomi dan Sanity dipertahankan. |
| QA-102 | Pusaka pindah ke dudukan saat CP6 | PARTIAL | Transaksi CP6 memindahkan tiga pusaka dan jalur pulang tetap playable; review visual dudukan/postboss hall belum lulus. |
| QA-103 | Epilog menampilkan 666000 viewers | PARTIAL | Epilog menampilkan data kanon666000; saldo sah tetap0 pada CP7. Overlay belum dicapture/diterima di Windows. |
| QA-104 | Live chat palsu serempak | PARTIAL | Epilog chat selesai dan eksplorasi kembali; variasi tanpa kartu/sepatu dan review narasi visual masih belum lulus. |
| QA-105 | Pesan Penonton_Terakhir Rp9999999 | PARTIAL | Save native CP7 memuat wallet_bound9999999, saldo sah0; pengulangan khusus event epilog perlu acceptance tambahan. |
| QA-106 | Epilog tanpa auto fade | PASS_DIRECTED | Setelah epilog pemain masih berjalan475m total rute, mencapai motor dan membatalkan panel tiga kali; tidak auto fade. |
| QA-107 | Panel motor Kembali Menjelajah | PASS_DIRECTED | Tiga pembatalan panel nyata mempertahankan P3_COMPLETE=false; konfirmasi berikutnya baru menyimpan CP7. |
| QA-108 | Panel Lanjutkan + save gagal | PASS_UNIT | ExitNeedsEpilogueAndCP7FailureDoesNotCompleteOrDuplicate: gagal tulis tidak complete, sukses tepat sekali. |
| QA-109 | Asset audit pabrik | PASS_REPO | phase3-asset-sources/import-manifest/pbr-conversions/audio-sources; credits pembuat/lisensi. Tidak memakai art DreadOut. |
| QA-110 | Build Windows Iris Xe + QA regresi P0/P1/P2 | NOT_RUN | Perlu playtest Windows/Iris Xe, FPS/1% low dan memori. Build lintas platform tidak membuktikan eksekusi Windows. |
| QA-111 | CP3 ketika Buku Mandor/Boneka/Bukti AR ayunan dilewatkan | NOT_RUN | PlayMode: CP3 tetap tersimpan; bangsal dapat dicapai, lalu pemain kembali ke ayunan dan pickup selesai tanpa softlock |
| QA-112 | EVP interkom 8 s `TIRUAN` + 3 s `ASLI` secara periodik | PARTIAL | Metadata 8/3s diuji batasnya; native hold6s mencakup ASLI dan interrupted tidak memberi evidence. Rekaman6s red-only spesifik perlu uji. |
| QA-113 | Mengejar sumber gelombang hijau di lantai rapuh | NOT_RUN | Playtest: Label `ASLI` bukan jaminan pijakan aman; rajah UV/fisik tetap indikator independen |
| QA-114 | Jatuh dari celah bangsal tingkat 2 / balkon 09 | NOT_RUN | PlayMode: `FRAGILE_DROP` memindah ke lantai bawah dengan jalur kembali; bukan Game Over tak terduga |
| QA-115 | Siklus 1–2 menang via flare, 3 generator masih utuh | NOT_RUN | Unit: Siklus 3 tidak mematikan dua generator belum dipakai; pemain bebas memilih satu dari tiga |
| QA-116 | Fase 3 dengan 0 flare serta 0 baterai sebelum tarikan | NOT_RUN | Windows playtest: Counter sinar generator dapat memutus kain; akses recharge manual dan final Blitz tetap mungkin |
| QA-117 | Tahan UV 1,0 s pada simpul kain tanpa flare | PASS_DIRECTED | UV simpul memutus tarikan tanpa flare dan tanpa menambah lapisan segel; lapisan berikutnya menuntut beam + Blitz. |
| QA-118 | Flare digunakan untuk counter kain lalu Blitz gagal | NOT_RUN | PlayMode: Flare instance habis, generator belum `used`, pemain tetap dapat ulang lewat charger |
| QA-119 | Wewe menemukan layar hidup saat pemain bersembunyi | NOT_RUN | Playtest: Ancaman mengikuti LOS/stimulus, bukan secara ajaib menganggap bersembunyi aman |
| QA-120 | Save setelah bukti EVP tetapi sebelum AI Search selesai | NOT_RUN | Fault injection: Restore pada anchor aman; tidak menggandakan bukti atau mengunci Wewe dalam ambush permanen |
| QA-121 | Baterai boss 0 setelah 20 miss dan seluruh jatah pengisian fase terpakai | PASS_DIRECTED | 182 assertion pada player Linux; reset manual +25 berulang, berkas CP5 byte-identical dan Abort/Load mengembalikan baterai0 dan inventori persis. |
| QA-122 | Interaksi gerbang servis aula selama boss | NOT_RUN | PlayMode: Gerbang keluar tetap dilas; alcove servis terpisah, tidak membuka jalur skip boss |
| QA-123 | Sepatu Damar tidak diambil namun boss dan epilog selesai | NOT_IMPLEMENTED | Cabang cerita tanpa sepatu tersedia, tetapi pose menggenggam sepatu/pegangan pagar sinematik final belum dibuat. |
| QA-124 | Setelah boss, pemain mencoba mengikuti instruksi chat palsu loncat ke tungku | NOT_RUN | Playtest: Chat tetap narasi; pagar/collider mencegah *death by misleading directive* |
| QA-125 | Backup `.bak` pulih setelah korup `.json` dan upgrade P2/P3 tetap ada | PASS_UNIT | InvalidFactoryAnchorIsRejectedAndExplicitBackupRetainsProgress; primary ditolak lalu backup existing dimuat eksplisit. Tidak mengklaim tombol recovery UI Windows. |
| QA-126 | Art/audio golden pass per area vs headless-only | PARTIAL | 13 gambar normal/UV produksi software GPU Linux, fixture posed/frozen; lihat Phase3Screenshots. Belum Windows/art/audio golden acceptance. |
| QA-127 | CP2 disimpan sebelum umpan Damar, CP3 setelah ayunan; cek urutan dan reload | PASS_DIRECTED | CP2 nyata mendahului jebakan pertama, CP3 pada bangsal; receipt gilingan tidak melompat CP4. Unit backtracking monotonik juga lulus. |
| QA-128 | Jurnal/chat sedang terbuka, Wewe mendekat; tekan `F` sambil RMB diangkat | NOT_RUN | Runtime input: Layar padam lewat input prioritas benar; tidak ada klik ganda maupun exploit pause |
| QA-129 | Ketuk `C` cepat vs tahan `C` pada interkom/ventilasi | PARTIAL | TapC dua kali menyalakan/mematikan analyzer; hold terputus dan hold valid diuji native. Remap recorder belum diuji khusus. |
| QA-130 | Audit metadata `EvpSourceBus` seluruh emitter | PASS_REPO | Emitter factory mengklasifikasikan Mimic/Authentic/Ambient; interkom memakai authored segment8/3. Tidak membaca microphone. |
| QA-131 | Speaker interkom merah, ketukan hijau, dan ambience aktif bertumpuk | PARTIAL | Native memilih interkom dekat meski vent lebih tinggi priority; rekaman valid. Persepsi audio tumpang tindih tetap review Windows. |
| QA-132 | Spawn jebakan floor dinamis 1–6 m dari pemain dan tepat di bawah kaki | NOT_RUN | Unit+scene: Spawn ditolak dalam radius 6m, on-foot dan anchor CP; tidak terjadi *instant trap* |
| QA-133 | Kombinasi tile semua rute ke Mandor/Gudang/Aula/charger | PARTIAL | SafeAnchorHasCompletePhysicalNavRouteFromYard + UpperRoutesAndSecretHaveConnectedPhysicalNavigation; seluruh kombinasi runtuh dan lebar efektif perlu uji manusia. |
| QA-134 | Tepi papan gilingan 0,8m vs inti jebakan pada FPS30/60 | NOT_RUN | Physics+Windows: Tepi memberi peringatan 1,0s; timer runtuh fase belajar 1,2s/normal 0,6s tidak menumpuk menjadi *grace* tak terbatas |
| QA-135 | Rekaman EVP di Kantor dan scan UV Gudang selama Wewe patroli | PARTIAL | Main route menyelesaikan EVP dan UV dengan AI aktif. Detour rahasia sempat tertangkap saat kembali; fairness/pacing perlu playtest. |
| QA-136 | Transisi Wewe `PATROL_CEILING`→`PATROL_FLOOR` saat karakter/prop dekat landing | NOT_RUN | PlayMode: Drop hanya pada anchor kosong; bunyi `BRUK!` terdengar; AI tak tembus atap/teleport di kepala |
| QA-137 | Spam `E` + `R` + `C` + `F` sambil memegang RMB dekat tuas/katup | NOT_RUN | Input automation: Tepat satu pemilik input per frame, tidak mengambil item sekaligus membidik/Blitz |
| QA-138 | Sembunyi di loker dengan layar mati tetapi pintu terbuka/LOS belum putus | NOT_RUN | Runtime: Tidak mendapatkan status aman otomatis; Wewe melihat jika LOS nyata masih ada |
| QA-139 | Muat campaign yang mengandung alias ID Argon dan ID v2.1 sekaligus | PASS_UNIT | AliasesNormalizeWithoutDoubleEvidenceOrRelics; LegacySchemasMigrateWithoutLosingCampaign(1/2/3). |
| QA-140 | Audit scene art terhadap manifest pabrik | PASS_REPO | phase3-scene-content.json: 46 jenis PROP non-arsitektur, manifest posisi/skala; FactoryLibraryContainsThirtyDistinctNewNonArchitecturalProps. |
| QA-141 | Enam nama korban tambahan belum disetujui penulis | PASS_REPO | Tidak mengarang enam nama. Slot rusak dipertahankan sebagai teks naratif belum terbaca, sesuai Phase3Content. |
| QA-142 | Ambience uap/lori keras saat telegraph Wewe, derit, dan suara ibu palsu | NOT_RUN | Windows audio: TelegraphBus ducking/prioritas efektif; subtitle bukan satu-satunya petunjuk; bus tidak memaksa AudioMixer paralel |
| QA-143 | Ambil Bukti #1/Boneka/Dok Mandor di gilingan, mati sebelum masuk Bangsal | PARTIAL | BacktrackingCannotRegressCheckpoint ditambah directed native pickup CP2; simulasi mati tepat sesudah setiap pickup belum seluruhnya diuji. |
| QA-144 | Bandingkan CP3 yang sudah tercapai tanpa Bukti1/Pusaka1 lalu kembali ke ayunan | PARTIAL | BacktrackingCannotRegressCheckpoint(CP4,CP2/CP3) memverifikasi monotonik; traversal dengan setiap kombinasi skip masih perlu uji. |
| QA-145 | Input UV simpul kain siklus 3 saat 0% atau cukup baterai, dibanding generator ketiga | PARTIAL | Counter UV dengan energi cukup lulus. Variasi0% dan beam-counter knot tanpa UV belum dimainkan. |
| QA-146 | Setelah CP6, pemain mengikuti komentar palsu untuk meloncat ke tungku | NOT_RUN | Playtest: Tidak ada kill plane/endless fall baru; narasi tidak menjadi petunjuk tugas; panel motor dan sekret tetap bisa diakses |

Status akhir kandidat (bukan penerimaan seluruh QA): NOT_IMPLEMENTED=1, NOT_RUN=68, PARTIAL=33, PASS_DIRECTED=21, PASS_REPO=8, PASS_UNIT=15.
Bukti utama: 77 assertion directed sampai CP7; 182 assertion budget pada fixture terpisah; 201 regresi dan57 P3 overlap; 13 render fixture. Seluruh NOT_RUN/PARTIAL/NOT_IMPLEMENTED tetap pekerjaan terbuka.
