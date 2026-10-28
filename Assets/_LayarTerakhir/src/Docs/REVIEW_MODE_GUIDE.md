# Mode Review — Panduan Sutradara

Mode Review hanya ada di **Unity Editor** dan **development/review build**.
Di rilis produksi, fitur ini tidak ada sama sekali.

## Membuka

- Tekan **F7** kapan saja di dalam game untuk membuka panel bantuan.
- Pada build review (`Review-Phase1..5.cmd`), badge kecil `REVIEW [F7]`
  selalu terlihat di kanan atas sebagai pengingat.
- Semua tombol di panel bisa diklik mouse; tidak wajib hafal keyboard.

## F8 — Kamera terbang

- **WASD** gerak, **mouse** arah pandang, **Q/E** turun/naik,
  **SHIFT** ngebut, **scroll mouse** mengatur kecepatan (0,5–60 m/s).
- Tembus tembok, tanpa gravitasi. Posisi Raka **tidak berubah**.
- Tekan **F8** lagi untuk kembali persis ke posisi semula.
- Slider kecepatan juga ada di panel F7.

## F9 — Klik objek

- Tekan **F9**, lalu **klik objek** di lingkungan (termasuk hasil
  prosedural seperti area interlude).
- Panel kiri bawah menampilkan: nama, path hierarki lengkap, scene aktif,
  koordinat dunia, info renderer (material, bounds) dan collider.
- Objek terpilih diberi **kotak kuning berdenyut**. Material asli tidak
  diubah dan highlight hilang saat F9 ditekan lagi.
- Catatan: F9 tidak bisa dipakai sambil terbang. Keluar F8 dulu, baru F9.

## F10 — Simpan laporan bug

- Opsional: isi **deskripsi singkat** di panel F7 (maks 140 karakter).
- Tekan **F10**. Tersimpan sepasang file dengan nama sama:
  `Bug-YYYYMMDD-HHmmss-fff.png` (tangkapan layar) dan `.json` (metadata:
  scene, babak, checkpoint, posisi kamera, objek terpilih, waktu).
- Panel review otomatis disembunyikan saat memotret.
- Klik **BUKA FOLDER** untuk membuka folder `BugReports`.
  Lokasi: di dalam folder save game (`Application.persistentDataPath`).

## F11 — Bekukan dunia

- Menghentikan NPC, hantu, dan animasi untuk inspeksi visual.
- Kamera review (F8) tetap bisa bergerak.
- Tekan **F11** lagi untuk melanjutkan. Freeze otomatis dilepas saat
  pindah scene agar game tidak macet.

## Pindah scene review

- Tombol babak di panel F7 membuka scene review: Prolog (3 scene),
  Phase 1–4 (hub kos + scene utama), Phase 5 (Pasar Setan).
- **Save asli tidak pernah diubah**: perpindahan memakai slot sementara
  yang sama seperti launcher review, lalu dibuang saat kembali.
- **KEMBALI KE CAMPAIGN** memulihkan checkpoint asli dan scene semula.
- **MUAT ULANG SCENE** memuat ulang scene aktif tanpa menyentuh save.

## Batasan yang disengaja

- F8/F9/F11 tidak aktif saat menu/pause/sinematik terbuka.
- Saat terbang, penyimpanan checkpoint diblokir agar inspeksi tidak
  mengubah progres (kecuali saat excursion review, yang memang memakai
  slot sementara).
- Mode Review tidak mengubah kontrol HP, senter, jurnal, atau combat
  selama tidak aktif.
