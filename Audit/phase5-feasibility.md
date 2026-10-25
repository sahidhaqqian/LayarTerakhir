# Audit awal Phase 5 — Pasar Setan

25 Oktober 2026; basis `5ab018b`. Audit dilakukan sebelum perubahan kode/scene. Draft desainer 1.0-PARITY-SPEC dibaca seluruhnya (1470 baris); salinan byte asli disimpan di `Builds/ArtSources/Phase5/Phase5-user-original-*.md`. `Rancangan.MD`, kontrak Phase1–4 dan source terkait diperiksa secara terarah. Ini laporan kelayakan, **bukan bukti build atau 180 QA lulus**.

| Prioritas | Sistem | Status | Bukti aktual | Penyesuaian implementasi |
|---|---|---|---|---|
| P0 | Campaign/save | ADAPT | `Core/CampaignState.cs`: schema5, `TryCommit`, `.tmp/.bak`, rollback seluruh data | Tambah Phase5Data dan migrasi5→6 melalui loader yang sama; validasi receipt/inti/kabel/ending, pertahankan migrasi1→5. Tidak ada save manager baru. |
| P0 | Ending | ADAPT | `CampaignData.ending`, `EndingType {NONE,BAD,NORMAL,TRUE,SECRET}` sudah tersedia pada basis | **Rename enum global existing ke EndingId dengan nilai numerik0–4 yang sama**, sesuai tambahan pengguna. Tidak membuat enum/field ending kedua. Receipt berada pada Phase5Data; kapasitor diturunkan dari receipt FINAL_CHOICE dan ending global; ending hanya satu sumber kebenaran. CP9 setelah sinematik selesai; crash setelah lock memutar ending yang sama. |
| P0 | Handoff P4 | ADAPT | `CampaignRouting.ContinueScene`, `GameShell.Continue`, `Phase4Presentation.ConfirmExit/ShowBoundary`, `Phase4Campaign.Finish` | CP8 valid + empat segel → gerbang P5, tanpa Kos. Pemain aktif tetap menuruni tangga P4, baru melewati boundary fisik. Continue lama dapat langsung ke P5 setelah transaksi valid; gagal tulis menjaga save P4. Jangan menyalin vestibule/tangga lama ke scene baru. |
| P0 | HP/energi | ADAPT | `SmartphoneController` satu baterai, cost foto1/Blitz15/cooldown6; provider `IPhoneChapterContent` | Provider P5, daftar instance PB P5 ditambahkan pada owner existing. FINAL_CHOICE memerlukan override kapasitor **state-lokal** agar kamera tetap dapat digunakan saat baterai persisten0; drain dan cost normal tetap berlaku di luar finale. |
| P0 | Kamera/input | READY/ADAPT | `GameInputConfig`, `ShoulderCameraRig`, `AtmajaPhoneView`: V perspektif, X Selfie, RMB tahan | Tidak mengganti locomotion/grip/kamera global. Sync X→Q→R hanya state ending; AR/UV tetap eksklusif normal. Hint dibaca dari binding. |
| P0 | Kendi | ADAPT | `Phase4Ids.Kendi`, `Phase4Data.water_units` merupakan owner volume saat ini | Pertahankan instance **dan owner air yang sama**, tidak menduplikasi volume di Phase5Data. Fallback cawan hanya untuk save tanpa Kendi; sumber volume tetap tunggal. |
| P0 | Bukti/arsip | READY/ADAPT | Global `evidence_ids`, `secret_items`, `archive_prepared`; lima ID `Phase1Ids.Secrets`; EVP `Phase3EvpSource` | Tiga ID P5, kontrak dua subreceipt, validasi lima ID aktual; pita tersimpan sebelum prop/popup. Nama korban hanya Tari, Lastri/anak, sembilan anak buruh, warga Karang Setra yang kanon. |
| P0 | Inti/kabel/checkpoint | BUILD | Pola atomik singkat `Phase4Campaign.Receipt/Shot`, CP6 rolling | P5 CP7 rolling, tepat tiga inti berurutan dan tiga kabel; biaya Blitz diserap sekali dari owner HP. Tidak memakai barrier panjang P3; miss inti3 membuka ulang jendela tanpa menghidupkan kabel/api lagi. |
| P0 | Uang/toko | READY/ADAPT | Wallet sah/terikat/spent dan receipt atomik existing | Harga baseline dokumen; saldo0 tetap playable. Donasi66.666.666 sekali ke dana terikat. Nilai epilog500juta/125juta hanya narasi. |
| P0 | Sanity/recovery | ADAPT | P4 nilai global, tier70/40, kemenyan ke100/cooldown12; pemulihan8s/10s | Gaze baru memakai nilai lama termasuk0. Dinamo+25/8s, altar+15/10s/cooldown30; proteksi hanya collider/LOS, tidak flag invulnerable. |
| P1 | Replika/boss | BUILD | Tidak ada Phase5Director atau encounter P5 pada basis | Aktor P5 terpisah, bukan menghidupkan boss/segel lama. Telegraph nyata; fase1 UV+generator, fase2 Selfie+Thermo+cahaya, fase3 AR+kabel+cover. |
| P1 | Aset ≥100 | BUILD | Arahan terbaru pengguna melarang aset fase terdahulu; §0.9/§7.2 draft masih mengizinkan kit replika lama | Arahan pengguna menang: **lingkungan dan visual replika baru**, minimum100 sumber model baru yang ditempatkan; Raka/HP/Damar sebagai identitas campaign tetap, tidak dihitung. Cek metadata ID/hash terdahulu, tanpa memilah isi ratusan aset lama. Manifest sumber/lisensi/impor/penempatan dan kredit. |
| P1 | Rendering | ADAPT | Unity6000.3/URP, UVlayer24, ARlayer29, phone feed existing | Satu scene P5 dengan interest zones/culling aktor; mesh/material sumber asli, LOD, ≤24 wajah aktif, cermin tanpa rekursi; bukan jutaan UI. IrisXe hanya target sampai benchmark Windows nyata. |
| P1 | Audio/VO | ADAPT/BUILD | Mixer/subtitle/cinematic focus existing; dialog P5 belum direkam manusia | Audio baru berlisensi, subtitle semua clue, daftar rekaman manusia. Tidak mengklaim TTS sebagai VO final atau akting lengkap. |
| P1 | Sinematik/credits | ADAPT/BUILD | `HudController` fade/subtitle dan `UiFocus.Cinematic`; belum ada pipeline empat ending final | Director babak memakai fasilitas tersebut untuk blocking kamera/aktor, empat epilog berbeda dan credits. Jangan menambah framework cutscene/save paralel. |
| P1 | QA/build | BUILD | Tool isolated Unity dan packaging/upload R2 existing | Uji sintaks dahulu, tes migrasi/transaksi/softlock/regresi, jalur native/capture; Windows ZIP+R2 verified. 180 QA masing-masing punya evidence/status, bukan PASS massal dari headless. |

## Audit cerita

Pasar Setan gerbang kelima, bukan segel kelima. Empat segel lama tidak berubah. Hantu sektor merupakan proyeksi memori Penonton_Terakhir, Mbah Suro tidak hidup kembali. Damar masih di kurungan sampai ketiga kabel dan inti selesai. Malam langsung dari Karang Setra, epilog True05:30. Latar1997/98/99/2001/04 dan identitas korban tetap. Viewer999.999→1.000.000 merupakan overlay, bukan jumlah objek. Secret memperluas pembebasan korban lama; True tetap menyelamatkan Raka/Damar/penonton. Tidak ada quest atau nama korban baru.

## Register penyesuaian ke Phase5.MD

REPO-01 schema6 melalui migrasi nyata; REPO-02 pakai EndingId global; REPO-03 volume Kendi tetap phase4.water_units; REPO-04 P4 boundary/Continue; REPO-05 kapasitor kamera terpisah; REPO-06 larangan reuse kit art dan ≥100 model baru; REPO-07 CP7 rolling/miss inti3; REPO-08 satu scene interest zones; REPO-09 nilai tuning baseline bukan bukti pacing; REPO-10 VO/Windows GPU/manual180QA tetap memerlukan penerimaan nyata. Keputusan teknis P5-D01–20 mengikuti baseline yang sudah ditulis dan instruksi implementasi pengguna; tidak menunda implementasi untuk persetujuan ulang, tidak menambah kanon atau replay ending otomatis.

## Dependensi dan risiko

1. Rekonsiliasi dokumen dan kontrak data/router/energi. Risiko P0: rollback penuh, ending tunggal, snapshot lama.
2. Unduh sumber baru legal dan import/material binding; risiko P1: skala, polycount, lisensi, kesesuaian Indonesia. Jumlah model dibuktikan penempatan, tidak dihitung dari folder.
3. Rute fisik gerbang/sektor/panggung, collider/LOS/anchor/recovery. Bukti clue dunia tetap dapat dipercaya.
4. Investigasi/arsip/stealth, boss3inti/3kabel dan pemulihan; ending physical-input dan sinematik.
5. Syntax/regresi/fault injection, native route/render, Windows review/R2, Graphify, commit/push. Keberhasilan headless tidak membuktikan kualitas visual/VO/75menit atau FPS target.
