# Kepatuhan Prolog — Checklist.MD & GDD

Audit 16 Oktober 2026 terhadap `Checklist.MD` (revisi playtest) dan
`GDD_Layar_Terakhir_Alur_Utama_dan_Detail_Prolog.docx` (Naskah Penuh Prolog).
Status: **sesuai, dengan dua revisi playtest yang disetujui dan empat keterbatasan yang dicatat jujur.**

## Checklist.MD — bagian Prolog

| Tiket | Status | Bukti |
| --- | --- | --- |
| P0-SYS | Sesuai | Laci 4 roda kode `1408` (revisi playtest menggantikan 3 digit GDD), jam bandul 03:45, 3 potret dosa (cangkir → cakaran → tali), unlock AR via modul MataBatin, tutorial matikan layar di pemeriksaan lemari, glitch daya 12% sekali + cadangan tersimpan, chase rail 25 titik, Blitz stun 4 detik. |
| P0-3D | Sesuai | Kos (revisi disetujui 5×3.8 m), rumah dua lantai, dapur + sumur berantai, koridor lukisan (3 potret + plakat kuningan), basement bentuk U (rak arsip tengah, 2 lemari sisi), meja jahit berkolong, cermin besar, rig Nyonya Sri Atmaja (kebaya, leher miring, gunting taman berkarat). |
| P0-AV | Sesuai | Surat kos tanpa VO ibu kos; panggilan Damar otomatis + pesan/dana kontrak di Pesan/Bank; koran 1998; chat per trigger; hujan, derit tali (`AtmajaSynth.Creak`), senandung tembang di pintu kamar utama, gesekan gunting (`AtmajaSynth.Scrape`); title drop LAYAR TERAKHIR; simbol mata tertutup AR di bingkai foto ketiga ("SAKSI MENYIMPAN NAMA, PENONTON HANYA MENYIMPAN WAJAH"). |
| P0-QA | Sesuai | Jalur prolog tamat tanpa collectible; puzzle hanya memakai foto 1% (tanpa Blitz); tertangkap/gagal QTE memulihkan checkpoint + baterai/uang dari snapshot. |

## GDD — Naskah Penuh Prolog per adegan

| Adegan | Status | Catatan |
| --- | --- | --- |
| 1 Kamar Kos | Sesuai | Monolog 32 penonton, 3 ketukan keras + langkah menjauh, surat Rp3.500.000, panggilan/voice note Damar, donasi Rp5.000.000 + pesan kontrak sisa Rp495.000.000, puzzle laci, kunci motor/jaket/powerbank kecil. |
| 2 Halaman | Sesuai | Motor matic + lampu redup, catatan survei, live 14 penonton, chat #1 + sawer 500k, kamboja crouch, vault + chat #2. |
| 3 Lantai 1 | Sesuai | Jendela membanting + 250 penonton, sumur (KLOTAK + bisikan + bonus 150k), ransel/HP retak + animasi tangan transfer chip, filter AR hijau night-vision VHS, sosok berlcheher patah, chat #3 + 1.420 penonton, tulisan darah III:XLV, jarum kuningan, jam 03:45, koran + kunci tangga. |
| 4 Lantai 2 | Sesuai | Jejak tangan berdarah (12 smear di tangga/koridor), 3 potret + plakat persis GDD, senandung dari balik pintu utama, celah ventilasi AR+crouch, buku harian Bima, cangkir (rantai 1), mikro-scare cermin, tirai + air keruh + cakaran (rantai 2), surat wasiat, tali berayun + KREEEK + figur tergantung 0,5 dtk (rantai 3). |
| 5 Kamar Utama/Basement | Sesuai | Karpet tergulung + trapdoor + tangga batu, uap napas di basement dingin, glitch daya 12% sekali, kemunculan Nyonya setelah lampu teplok padam, peringatan merah, stealth + pemeriksaan lemari [F], powerbank 80%, handycam di depan cermin via kolong meja. |
| 6 Chase | Sesuai | Rekaman handycam, cermin retak + pecahan, kejaran dengan musik kendang/saron/sirine (sintesis original), slide pintu jebakan, distorsi FOV koridor + lampu meledak, lompat meja, slow-mo 3 dtk + instruksi Blitz, stun 4 dtk, berguling keluar jendela, lari ke gerbang. |
| 7 Ending | Sesuai | Napas terengah di jalan, siluet di jendela lantai 2 + kilat, 54.820 penonton, chat viral, sawer kumulatif Rp25.450.000, pesan Penonton_Terakhir, shutter "CKREK", kartu judul LAYAR TERAKHIR. |

## Revisi playtest yang disetujui (menyimpang dari GDD 15 Oktober)

1. Panggilan Damar langsung tersambung otomatis; tidak ada layar "14 panggilan tak terjawab + tombol Play" (keputusan playtest 16 Oktober, tercermin di Checklist P0-AV).
2. Gembok laci 4 roda `1408` (tanggal 14 bulan 08) menggantikan 3 digit `148` GDD (tercantum eksplisit di Checklist P0-SYS).
3. Ukuran kamar kos 5×3.8 m (disetujui developer) menggantikan 4×3 m GDD.

## Keterbatasan yang dicatat

- Video handycam diputar audio+subtitle dengan bar sinematik; belum ada render video in-world (tangan-tangan dari cermin tidak divisualkan).
- Musik chase adalah sintesis original pendek (loop 2 birama), bukan rekaman gamelan.
- "Asap hitam" pada rantai pintu utama belum divisualkan; rantai memakai mesh + material logam.
- Pacing 90 menit, acceptance suara, dan FPS Iris Xe tetap memerlukan playthrough manusia; timer uji dipercepat.

## Verifikasi teknis saat audit

- 57/57 tes Unity lulus; validasi scene 0 gagal (259.110 tris, 863 renderer, tekstur ≤1024).
- Windows Mono Development build dibangun ulang setelah polish: 294 file keluaran Unity, 112 assembly, 301 entri ZIP, CRC penuh.
- ZIP `175.324.724 byte`, SHA-256 `203d85655f61bb6353fa1ca6ebe6fb0bfb41654eb81abaaba8b0a966dc3eeaaf`; diunggah ke R2 dan diverifikasi baca-balik penuh (`Audit/r2-windows-build-upload.json`).
- Eksekusi Windows, pacing manusia, dan FPS Iris Xe tetap menjadi tugas audit pengguna.
