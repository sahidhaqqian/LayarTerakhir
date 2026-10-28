# Phase 1 — Complete Visual Overhaul (28 Okt 2026)

Branch: `phase1-overhaul-20261028` (lanjutan overhaul backend 182695a4).
Fokus sesi ini: **kualitas visual seluruh lingkungan**, bukan sistem gameplay.

## Referensi yang dipelajari (R2 `dreadout-referensi/`, diunduh & diinspeksi)

- `contoh_kost.jpg` — rusunawa 4 lantai nyata: parapet panel solid dengan
  tiang vertikal ritmis + rel pipa atas, pintu kayu gelap dengan jendela
  samping tinggi, koridor ubin abu, void terbuka.
- `contoh_lorong_kost.jpg` — koridor sempit: pintu + jendela samping,
  konduit/plafon terekspos, proporsi pintu-dinding realistis.
- `contoh_dari_Dreadout_ruangan_terbengkalai.jpg`, `Main_hall_of_the_school.webp`,
  `Small_Key's_Location.webp`, `School_1.webp`, `PWnaTLQp...jpeg`, `dread9.png` —
  bahasa visual DreadOut: pool senter terang dengan falloff kuat, dinding
  mottled mengelupas dengan band bawah gelap, ubin besar ber-grime, satu
  objek fokus terisolasi oleh cahaya, puing di dasar kolom/dinding.
  Tidak ada aset/desain DreadOut yang disalin; hanya prinsip atmosfer.

## Bahasa visual yang diterapkan

1. **Dua nada dinding koridor**: band wainscot gelap 0–1,1 m + plaster
   mengelupas di atasnya, per lantai (lihat identitas lantai di bawah).
2. **Tekstur mottled nyata**: plaster diganti dari print modular bersih ke
   scan Poly Haven `worn_cracked_plaster` (tint abu-hijau), bump/occlusion
   diperkuat; ubin koridor dibasahi (L1/L3) dan digosongkan (L4).
3. **Parapet referensi foto**: tiang vertikal pada sisi void semua lantai +
   rel tengah karat + handrail pipa kontinu (dari pass sebelumnya).
4. **Servis plafon terekspos**: konduit, cable tray, junction box per lantai;
   ducting/pipa industri di ruang pompa.
5. **Cahaya**: senter HP tetap sumber utama (aturan Rancangan.MD); bulan
   menyapu facade selatan; exposure/kontras volume disetel ulang
   (postExposure .45, contrast 18, saturation −20) agar pusat bingkai
   terbaca seperti pool senter referensi tanpa menghilangkan gelap tepi.
6. **Komposisi per ruang**: setiap ruang penting mendapat furnitur/prop
   fungsional dari 124 model CC0 baru (di bawah), bukan clutter acak.

## Identitas per lantai

- **Exterior**: gerbang besi + pilar, barrier beton, mobil tertutup
  terbengkalai, lampu jalan (model + praktikal hangat), hydrant, ban/rims
  berkarat, barrel, planter, kanopi lantai dasar facade, noda hujan di
  bawah jendela, parapet atap + antena, genangan glossy + noda oli.
- **L1 (lembap/infrastruktur)**: ubin basah, wainscot lumut, ruang pompa
  dengan compressor, drill press, welding cart, tool chest/cart, ducting
  plafon, pipa industri, rak suku cadang, tangga lipat, extinguisher.
- **L2 (bekas dihuni)**: wainscot aus, furnitur kayu painted (sofa, kabinet,
  rak buku, day bed, bed frame), TV tua, boombox, kursi monobloc/sekolah,
  koper, keranjang, papan tulis pengelola, meja bundar galeri.
- **L3 (klaustrofobik)**: ubin basah gelap, band gosong, tanda lantai basah,
  peralatan kebersihan lengkap di ceruk cutter, cart penyimpanan, lampu
  industri gantung + sconce kurung + soket tarik + bohlam telanjang,
  kamera keamanan lift, fluorescent jatuh di lantai.
- **L4 (bekas terbakar/ritual)**: ubin gosong, wainscot arang, furnitur
  gothic 401/402/404, cermin ornamen, bingkai foto, vas keramik/kuningan,
  properti altar diperbesar (candleholders, diya lantern, piring ukir,
  pot tanah), decal noda ritual diperkecil agar terbaca sebagai noda,
  pintu WC kayu, cerobong tungku dapur, meja makan komunal.

## Akuntansi aset (target keras 100+)

- **124 model berbeda baru** dari Poly Haven (CC0-1.0) diunduh dan
  terpasang terlihat di scene. Sumber, penulis, URL file, dan SHA-256:
  `Audit/phase1-overhaul-asset-sources.json`. Penempatan per aset/area:
  `Audit/phase1-overhaul-prop-placement.json`.
- Distribusi terpasang: exterior 22, Lantai 1: 17, Lantai 2: 22,
  Lantai 3: 17, Lantai 4/arena: 31, bersama: 15.
- 15 kandidat di-skip saat fetch karena multi-material/tidak tersedia
  (chainlink fence, aircon, industrial pipes set, extinguisher, dll.);
  builder melewatinya secara eksplisit — tidak diganti primitif.
- **Diganti/ditingkatkan dari pass ini**: tekstur plaster (print bersih →
  scan mengelupas), ubin L1/L3/L4 (varian basah/gosong), varian jendela
  facade (Windows/Broken _2 yang sebelumnya tak terpakai), pintu ruang
  DebrisBlocked (varian Broken_Double), arah cahaya bulan facade,
  exposure volume, ukuran decal altar, komposisi 6 sudut pandang capture.
- Aset sebelumnya (ConcreteInterior, DoorsWindows, 30 prop Poly Haven,
  room dressing) dipertahankan; tidak ada aset DreadOut atau buatan
  sendiri. Semua mesh visual berasal dari sumber berlisensi
  (`originalVisualMeshes 0`).

## Verifikasi visual (bukti dirender Unity, bukan klaim)

- Capture: `Audit/Visuals/Phase1-VisualOverhaul/` — 20 sudut lingkungan
  (exterior/facade, pos satpam, pompa, koridor L1, K102 + tangganya,
  koridor L2/kotak surat, arsip, koridor Kuntilanak, tangga/lift L3,
  void L4, loop timur L4, 401/402, 403, kamar mandi, dapur, altar 404,
  arena halogen, mulut 404, lobi servis) + 2 bidikan RMB.
- Before: `Audit/Visuals/Phase1-Overhaul-Final2/` dan
  `Audit/Visuals/Phase1-Environment-Final/` (pass sebelumnya).
- Temuan inspeksi yang sudah diperbaiki: decal altar terlalu besar →
  diperkecil; view arsip terlalu jauh → didekatkan; view void terhalang
  pier → dipindah ke galeri selatan; view cakaran 402 meleset → diluruskan;
  arah bulan sempat membelakangi facade → dikembalikan menyapu facade;
  plaster terlalu cokelat → tint abu-hijau.
- **Batasan jujur**: host Linux memakai llvmpipe; capture kamera utama
  tanpa senter hampir hitam (termasuk pada binary lama), jadi capture
  memakai viewfinder HP terangkat — pencahayaan gameplay yang sebenarnya.
  Kualitas akhir pada GPU Windows/Iris Xe (bayangan, pantulan basah,
  keterbacaan jarak jauh) **belum** terverifikasi dan menjadi kewenangan
  penerimaan Game Director di Windows. Tidak ada playtest manusia dan
  tidak ada pengukuran FPS pada sesi ini.
- Tes terarah: `Phase1CampaignTests` **57/57 lulus** (termasuk test baru
  `RusunawaVisualOverhaulCoversEveryFloor` yang menegaskan ≥100 model
  overhaul berbeda terpasang, tiang parapet, konduit, dan identitas
  material per lantai) — `Audit/phase1-visual-overhaul-tests.xml`.
  Visual review standalone: **completed, 22 screenshot, semua cek lulus**
  (receipt di folder capture). Run namespace penuh sebelumnya menunjukkan
  10 kegagalan di luar area ini (8 tes Prolog/Kos `FoundationPlayAuditTests`
  tanpa referensi Phase1, 1 `ReviewDirectBootTests` yang membutuhkan
  artefak `Builds/Windows` yang tidak ada di worktree ini, 1 test baru
  yang saat itu belum diberi tiang L3 dan sudah diperbaiki) — bukan
  regresi dari perubahan ini, tetapi juga tidak diklaim lulus.
- Pintu bukti cakaran 402 diganti dari kubus menjadi pintu FBX unduhan
  berpermukaan baja aus setelah capture review menunjukkan kubusnya tidak
  terbaca di feed; goresan lima jari kini terlihat di screenshot 10.

## Yang masih lemah (penilaian art director, bukan checklist)

- Facade masih terbaca datar dari jarak spawn: ritme jendela seragam
  bawaan modul; kanopi + noda hujan membantu tetapi variasi balkon/unit
  membutuhkan modul facade khusus di pass berikutnya.
- Meja metal 403 terbaca terlalu terang/bersih di bawah senter dibanding
  dinding arang; perlu varian material lebih gelap.
- Lantai kamar mandi/dapur masih beton polos di luar area ubin koridor.
- Capture llvmpipe tidak dapat membuktikan kualitas bayangan/refleksi
  final; jangan jadikan folder ini bukti penerimaan visual Windows.
