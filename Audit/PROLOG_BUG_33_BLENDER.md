# Bug 33 — cangkir original Blender

Permintaan lanjutan pengguna: **buat sendiri pakai Blender**. Cangkir adaptasi unduhan diganti model original; tidak ada model berbayar atau model pihak ketiga sebagai dasarnya.

- Sumber editable: `ArtSources/Prolog/CrackedPorcelainCup.blend`.
- Generator yang dapat diulang: `Tools/author_cracked_porcelain_cup.py`.
- Ekspor game: `Assets/_LayarTerakhir/assets/Art/PrologRevision/Models/CrackedPorcelainCup.fbx`.
- Tinggi sekitar 9,5 cm; dinding 2,8 mm, gagang terbuka dan dasar tertutup. Alur retak sedalam 0,34 mm masuk ke mesh keramik, dengan serpihan kecil di bibir. Susu berada di dalam, di bawah bibir.
- 11.044 segitiga; badan dan gagang tidak memiliki tepi terbuka/non-manifold. Permukaan susu sengaja berupa permukaan tipis.
- Material keramik glasir, inti keramik, retakan, dan susu memakai material original; tidak memakai tekstur cangkir lama.
- Scene mempertahankan interaksi cangkir dan titik foto AR. Proporsi diskalakan seragam, dasar cangkir diletakkan di atas meja.

## Bukti

Tiga render Blender: `Audit/Visuals/Prolog-Cup-Blender/`. Tiga capture dari scene Unity: `Audit/Visuals/Prolog-Cup-Unity/`. Capture Unity menggunakan kamera pemeriksaan Editor; bukan screenshot player Windows.

Validasi mesh Blender lulus; **3/3 tes terarah Unity lulus**, exit code 0: rongga/retakan/gagang, posisi di meja/target foto, dan alur foto bukti tiga rantai. Hasil final dicatat dalam `prolog-cup-validation.json`. Bukti authoring dan hash model: `prolog-cup-authoring.json`.

Reproduksi:

```sh
blender -b -t 8 --python-exit-code 1 --python Tools/author_cracked_porcelain_cup.py
```

Gunakan `-- --no-previews` untuk hanya menulis model/source tanpa merender tiga preview. Di Unity, jalankan `PrologReviewRevision.ApplyCupOnly` atau menu LayarTerakhir → Prolog → Apply original Blender cup (bug 33). Pipeline revisi penuh juga memakai model ini.

Penerimaan visual oleh desainer masih menunggu playtest manusia.
