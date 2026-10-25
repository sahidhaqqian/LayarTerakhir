# Audit kontrak dan cerita Phase 4

25 Oktober 2026. Basis kode `ba029cd`; draft pengguna disalin utuh ke `Builds/ArtSources/Phase4/Phase4-user-original-30c9be1624d6.md` sebelum penyuntingan. Arahan pengguna mengizinkan implementasi serta penyesuaian konflik mekanik. Status tabel adalah kelayakan awal, bukan hasil QA.

| Sistem | Status | Bukti aktual | Keputusan / risiko |
|---|---|---|---|
| Campaign dan file save | ADAPT | `Core/CampaignState.cs`: schema4, `TryCommit`, `.tmp/.bak`, owner attempt | Tambahkan `Phase4Data`, migrasi4→5; pertahankan1→2→3→4 serta seluruh ID P0–P3. P0 |
| Progres ritual | ADAPT | `Phase3Campaign.PrepareRitual/WinRitual` memakai barrier satu arena | P4 memakai transaksi singkat setiap siram/hit/refill/pickup. CP6 rolling dari receipt N/E/W; tidak menahan write barrier sepanjang perburuan. P0 |
| HP, perspektif, input | READY | `SmartphoneController`, `IPhoneChapterContent`, `GameInputConfig` | Satu controller/energi/feed, V perspektif, X Selfie; R/Q/T/C/F/H/B/G sesuai binding nyata. P0 |
| Bukti AR/Thermo/EVP | ADAPT | `Phase2Director`, `Phase3Investigation`, `Phase3EvpSource` | Provider babak, metadata sumber existing, tanpa microphone/recorder global kedua; foto dan pusaka terpisah. P0 |
| Sanity | BUILD | `CampaignData.sanity` ada, P1–P3 belum mengaktifkannya | Pertahankan nilai masuk. Pemulihan persiapan Kos, shrine hutan/desa tersedia gratis. Kemenyan ke100 mengikuti `Rancangan.MD`, mengganti baseline+35. P0 |
| Kendi dan air | BUILD | Inventory/receipt existing; belum ada air P4 | Satu ID Kendi, kapasitas3, volume aktual tersimpan; offering tidak mengisi otomatis; sumur dan tempayan tersambung dapat diulang. P0 |
| Banaspati dan Mbah Suro | BUILD | Threat P3 dapat menjadi acuan LOS/telegraph; Wewe tetap spesifik P3 | Aktor baru, waypoint/NavMesh permukaan dengan visual melayang; satu inti damage, Suro director ilusi. Tidak mengubah AI lama. P1 |
| 12 obor | BUILD | UV layer24/AR layer29 dan feed existing | Empat sektor × tiga kandidat, satu asli per sektor, pola/seed tetap; tiga local Blitz dan satu core. P0 |
| Inventory, Jurnal, uang | READY/ADAPT | `CampaignData` dan `PhoneDesktop`/provider babak | Tidak ada inventori/ledger global baru. PB P4 melalui owner energi existing; konsumsi lama juga harus atomik saat dipakai di P4. P0 |
| Kos | ADAPT | `KamarKosPrologDirector`, `Phase3Hub.TryInstall` | Golden Kos dan komponen Raka/HP/motor adalah kontinuitas kampanye; tidak dihitung sebagai100 aset baru. Isi Desa/Bambu seluruhnya baru. P1 |
| P3→P4 | ADAPT | `CampaignRouting` berhenti pada Factory setelah CP7 | Continue P3_COMPLETE + tiga segel menuju Kos P4; guard mencegah prolog mengulang. P0 |
| P4→P5 | ADAPT | Belum ada scene atau director Phase5 di build | CP8 menyimpan P4_COMPLETE/Pasar Setan. Turun ke vestibule fisik di bawah pendopo pada scene P4; Continue kembali ke boundary itu, tanpa Kos/reset sumber daya. Integrasi seluruh P5 adalah pekerjaan babak berikutnya; jangan load scene yang belum ada atau mengaku P5 selesai. P0 |
| Aset | BUILD | Pustaka lama ada; arahan baru minimum100 | Hitung ≥100 identitas model baru berlisensi yang benar-benar ditempatkan, bukan shader/tekstur/duplikasi. Manifest sumber, hash, impor dan penempatan diverifikasi. P1 |
| VO dan audio | BUILD | Dialog kanon tersedia dalam naskah | Cari audio baru berlisensi; tidak menyebut TTS sebagai akting final. Dialog yang belum direkam mempunyai subtitle dan daftar rekaman manusia. P1 |
| Visual dan performa | BUILD | Pipeline Unity6/URP existing; host Linux | Render setiap area, periksa pencahayaan/material/skala/culling; FPS Iris Xe, emosi, animasi dan pacing memerlukan Windows playtest. P1 |

## Audit cerita dan sasaran mutu

Lima adegan dipertahankan. Rantai1997→1998→1999→2001→2004 dan Mbah Suro sebagai arwah konsisten dengan rancangan global. Kacamata dan catatan menambah jejak Damar tanpa menghadirkannya sebagai NPC bebas sebelum penyelamatan P5. KontrakRp500juta tetap janji antagonis; donasi pembukaRp6.666.000 dana terikat, angkaERROR bukan uang belanja. Empat obor bukan empat lokasi baru: segel desa adalah lokasi keempat, Pasar Setan gerbang kelima.

Kekuatan cerita P4 adalah kecurigaan terhadap penonton dan UI. Agar tetap adil, chat boleh menyamar/berbohong, tetapi binding input, Jurnal faktual dan rajah fisik tidak berubah. J1 mengajarkan klaim salah, J2 nama ditiru, J3 pesan sistem palsu. Catatan Damar mengungkap asal aplikasi sesudah pemain mengalami penipuan; twist empat segel membuka pasar memperoleh foreshadow tanpa mencabut kemampuan kamera atau mengarang korban/quest tambahan. Mbah Suro bukan health bar kedua dan Selatan tidak menambah tembakan kelima.

Referensi mutu DreadOut dipakai sebagai arah: identitas lokal yang kuat, siluet ancaman terbaca, kamera/alat supernatural berguna, audio berarah, dan jeda sebelum ancaman. Jumlah aset tidak membuktikan kesetaraan. Evaluasi hasil harus memakai komposisi/render dan playtest, bukan skor subjektif dokumen. Rujukan: [DreadOut resmi](https://store.steampowered.com/app/269790/DreadOut/), serta audit referensi DreadOut proyek yang sudah tersedia.

## Dependensi dan urutan

1. Kontrak data/schema/routing dan transaksi + dokumen revisi.
2. Sumber aset baru, konversi/optimisasi yang mempertahankan karya pembuat, material dan credits.
3. Kos dan level fisik tiga simpang/empat sektor, collider/navigasi/landmark/recovery.
4. Sanity, chat, investigasi, Jurnal, rahasia, ancaman.
5. Ritual3+1, kegagalan transaksi, sektor persisten, portal boundary.
6. Render, pemeriksaan100 model terpasang, uji jalur gratis, regresi, Windows ZIP dan R2.

Tidak ada klaim implementasi atau180QA lulus pada audit awal ini. Parameter yang telah dipilih mengikuti hierarki global dicatat di Phase4.MD §0.11; arah cerita/nama/tahun kanon tidak diganti diam-diam.

## Koreksi berdasarkan controller runtime

`FirstPersonController.Awake` membatasi berjalan1,7 dan sprint4,2m/s. Banaspati baseline3/4,5/5,2/5 diganti1,5/2,7/3,5/3,7m/s. Gate siram ≤7,5m/LOS berlaku sama pada seluruh kandidat, sehingga expose5s terjangkau tanpa memindah aktor. Bentuk UV tetap penentu keaslian, bukan pesan interaksi. Lihat Phase4.MD REPO-09.

## Guard eksplorasi hutan

Pendopo dipicu oleh kedatangan fisik, bukan wajib memegang tiga receipt chat. Tiga simpang dan tanda tetap ada. Pemain yang memutar luar jalur tidak mendapatkan receipt palsu, tetapi juga tidak dapat mengaktifkan ritual dengan aktor dormant. `Phase4Campaign.BeginRitual` memerlukan receipt Pendopo/reveal, dan `Phase4Banaspati.RitualBegan` memastikan aktor aktif.
