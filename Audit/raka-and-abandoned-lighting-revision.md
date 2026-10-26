# Revisi Raka dan pencahayaan — 26 Oktober 2026

Permintaan: penampilan Raka, tangan/HP dan jongkok terasa lebih wajar; bangunan terbengkalai gelap sehingga HP menjadi penerangan utama. Ini revisi presentasi dan aset turunan, bukan sistem gameplay baru.

## Sebelum dan perubahan

- Sebelumnya model VRM 793.072 segitiga dipangkas menjadi sekitar 32 ribu. Turunan baru memakai 96.000 segitiga, rig 55 tulang dan tekstur asli. Identitas/model milik desainer tetap; tidak membuat sculpt/tekstur baru. Permukaan pakaian dibuat non-metalik dengan smoothness .20 supaya tidak tampak seperti plastik basah.
- Gerak berjalan, berlari dan jongkok memakai performa authored dari [Quaternius Universal Animation Library](https://quaternius.com/packs/universalanimationlibrary.html), CC0. Retarget dilakukan pada rig yang sama, dengan penyesuaian kontak kaki, clearance kapsul dan lengan HP. Tidak mengambil aset DreadOut. `Riding` dan `InstallCard` tetap menggunakan performa existing.
- Semua 16 nama klip Legacy dipertahankan. Kecepatan playback mengikuti kecepatan kaki menapak yang diukur dari klip Unity, bukan konstanta lama. Batas kecepatan controller, input, collision dan root gameplay dipertahankan.
- Grip HP menggunakan jari existing, handset 78 × 165 mm dengan kedalaman casing 11 mm. Penempatan dicek terhadap kulit dan kelima jari; toleransi impor tes tetap 1,5 mm penetrasi dan 4 mm kontak, tidak dilonggarkan demi lulus.
- Case dan layar HP kini ikut disembunyikan bersama ketika disimpan; capture tampak depan menemukan layar terpisah yang sebelumnya tertinggal di tangan kosong.
- Jongkok memakai pose kerangka yang mengikuti headroom existing 1 m / .78 m, dengan kaki dan lutut diarahkan ke support plane. Tidak mengecilkan tubuh secara nonuniform. Tumpuan lunge sumber diadaptasi menjadi kaki di bawah pelvis, dan lengan tidak ikut terpuntir saat torso menyesuaikan headroom. Rotasi kaki authored dipertahankan setelah IK runtime. Jika retarget/crossfade membuat kedua sol terangkat bersamaan, pelvis visual diturunkan ke kaki penumpu terendah. Collider dan root pemain tetap.
- Body fill dikurangi dan mengikuti kondisi HP di lokasi gelap. Lampu dekoratif tidak lagi meminta puluhan shadow map sekaligus, agar bayangan senter tidak tersisih oleh lampu jalan.

## Lokasi gelap dan pengecualian yang disengaja

Outage mencakup Rumah Atmaja, Rusunawa, RSUD, Pabrik Gula, Karang Setra dan Pasar Setan. Lampu listrik dekoratif tetap mati meski flicker/budget lama mencoba menyalakannya. Ambient sangat rendah, bukan hitam absolut. HP dan spill dekat existing tetap memakai status baterai/layar yang sama.

Referensi lampu puzzle/ritual tetap dikendalikan director masing-masing: halogen, lampu operasi, ritual pabrik, obor, generator dan lampu jatuh. Api/lentera asli, motor dan rekaman handycam dipertahankan. Kos/lingkungan yang masih berpenghuni tidak dinyatakan bangunan terbengkalai. Set epilog berpenghuni/fajar tetap punya pencahayaan naratif. Tidak mengubah baterai pemain, Sanity, consumable, save/checkpoint atau logika deteksi AI.

## Sumber dan reproduksi

- `Audit/raka-presentation-retarget.json`: identitas SHA-256 VRM, motion CC0 dan FBX hasil, pemetaan klip dan fit handset.
- `Audit/phase5-animation-sources.json`: URL pembuat, lisensi dan SHA-256 unduhan motion sumber.
- `Tools/retarget_raka_presentation.py`: Blender, retarget sumber read-only dan derivasi model; tidak mengubah VRM asli. Cache baseline dapat direkonstruksi dari commit yang dipin.
- `LayarTerakhir.Editor.RakaPresentationRevisionBuilder.ApplyAndMeasure`: importer, referensi prefab, material dan pengukuran kecepatan kaki. Hasil di `Audit/raka-presentation-stride.json`.

## Status validasi

- EditMode: **370/370 lulus**, mencakup 13 fixture karakter/pencahayaan, rancangan prolog, scene dan campaign Phase 1–5. Bukti: `raka-lighting-regression-editmode.xml`. Tes kontak kaki mencakup 24 fase langkah, lima bobot crossfade dan endpoint bobot penuh.
- Native player: **32/32 pemeriksaan lulus**, input keyboard/mouse dan CharacterController nyata pada Melati; fixed simulation 1/30 dan renderer llvmpipe, bukan benchmark FPS. Maksimum deviasi sol saat jalan jongkok **0,433 mm**, overage headroom 0. Pose rendah .7687 m dan dengan HP .7690 m tetap di bawah kapsul .78 m. Klik kanan tahan/lepas, pemaksaan kamera dari menu, pemulihan aplikasi, baca catatan dengan feed aktif, dan checkpoint save/load lulus. Bukti: `raka-lighting-native-player.json`.
- Pencahayaan native: **6/6 lokasi**, 12 render HP mati/menyala, 144 fixture dekoratif masuk outage; beam mengikuti status HP pada semua scene. `raka-lighting-native-locations.json` menyimpan luminance, jumlah fixture dan catatan fixture. RSUD/Karang Setra mencatat peringatan NavMesh karena director dipause; capture ini tidak mengklaim validasi navigasi/AI.
- Percobaan enam scene dalam satu proses llvmpipe mencapai batas 4,5 GiB dan terhenti OOM; bukan hasil lulus. Bukti di `raka-lighting-review-limit.json`. Suite final menjalankan satu proses baru per scene dan selesai seluruhnya. Ini bukan pengukuran kebutuhan RAM Windows.
- Native zero-resource: **148/148 lulus**, PHASE4_CP8 → PHASE5_CP9 / NORMAL, 439,3 m gerak aktual. Termasuk kematian/Continue fase ketiga, persistensi inti/kabel, AR/EVP, Selfie-Blitz, dinamo dan jongkok di kanopi pemulihan. Mode headless (`rendered: false`), sehingga membuktikan input/progres/fisika, bukan tampilan boss atau wayfinding manusia dalam gelap. Bukti: `raka-lighting-zero-resource-native.json`.
- Windows: Unity 6000.3.26f1, Win64 Mono Development, **8 scene, 0 error, 8 warning** pada BuildReport. Log juga mencatat warning compiler lama pada diagnostik Phase 2 dan shader ray tracing paket URP yang tidak didukung host build. Tidak mengubah sistem tersebut dalam revisi ini. ZIP lolos pemeriksaan PE AMD64, kelengkapan seluruh file, SHA-256 dan CRC. R2 selesai dengan pembacaan ulang penuh yang cocok dengan ZIP lokal.
- Hasil tes bukan persetujuan artistik pengguna atau benchmark GPU Windows.

## Capture sebelum/sesudah

Lihat `Visuals/Raka-Lighting-Revision/`. File `before-*` berasal dari binary lama; route baseline berhenti pada pemeriksaan nama tangan yang sudah usang, sehingga bukan baseline QA lulus. File `after-*` berasal dari route native 32/32 lulus. Dua foto dengan nama `front-review-light` memakai lampu fixture sementara untuk menilai bentuk tubuh; itu bukan penerangan gameplay dan tidak diaktifkan dalam game normal. Capture lainnya memakai pencahayaan gameplay.

## Batas pemeriksaan

Playtest manual Windows pada GPU pengguna tetap diperlukan untuk penilaian rasa gerak, tampilan jari/animasi dari sudut lain, keterbacaan ruang pada pengaturan monitor berbeda, serta FPS/VRAM. Capture Linux menggunakan llvmpipe dan simulasi tetap; tidak mengklaim kualitas setara DreadOut/Outlast atau seluruh game bebas bug. Grafik dan suasana bukan hasil yang bisa dinyatakan sempurna hanya dari jumlah tes.

## Build untuk review

- `Builds/LayarTerakhir-Windows-x86_64-Development.zip` — 3.024.156.802 byte.
- SHA-256: `28175c05da6f73e7724e925857a79162cfcb1b4a49399d5de2d923b68f05e2a0`.
- R2: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`, diverifikasi 26 Oktober 2026 12:44:47 UTC melalui readback seluruh objek. Receipt: `raka-lighting-r2-windows-build-upload.json`.
- Sumber build dibekukan/dicocokkan pada 13.080 file di `raka-lighting-phase5-build-sources.json`.
- Build Windows belum dijalankan pada Windows di mesin ini; validasi runtime menggunakan player Linux.
