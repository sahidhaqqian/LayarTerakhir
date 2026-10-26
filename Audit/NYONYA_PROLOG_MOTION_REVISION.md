# Revisi gerak Nyonya — prolog

Tanggal: 26 Oktober 2026. Acuan sebelum revisi: `073a0ab4`.

## Masalah yang ditemukan

- `AtmajaThreatPresentation.LateUpdate` menghentikan playback (`AnimationState.speed = 0`) lalu mengambil pose pada interval 12 fps ketika berburu, sementara posisi AI terus bergerak.
- Pemilihan Walk/Idle menggunakan perpindahan satu frame. Frame tanpa perpindahan bisa memulai transisi ulang dan menghilangkan kesinambungan langkah.
- Run hanya dipilih pada flag pengejaran finale; pursuit basement 2,8 m/s masih memakai Walk.
- Panel tengah jubah pada model hasil rig otomatis memiliki pengaruh tulang tangan/kaki. Gerak yang lebih besar memperlihatkan panel tertarik dan meregang.
- Sebagian bilah gunting terikat ke panggul, sementara gagangnya mengikuti tangan; ayunan yang besar memperlihatkan keduanya terpisah.

## Perubahan

- Playback animasi kontinu, pembacaan kecepatan horizontal yang diperhalus, toleransi singkat untuk update navigasi yang tidak merata, serta ambang jalan/lari dengan hysteresis.
- Fase langkah diteruskan ketika berpindah Walk–Run; cadence diselaraskan dengan ukuran langkah hasil pengukuran clip di Unity.
- Idle/Walk/Run diadaptasi dari **Quaternius Universal Animation Library (CC0)** yang sudah ada di repository. Mesh, UV, tekstur dan anatomi karakter berasal dari model Nyonya yang sudah dipasok. Tidak menggunakan aset DreadOut atau membuat penampilan karakter baru.
- Perbaikan pengaruh tulang pada panel tengah jubah dengan peralihan bobot halus; kaki disesuaikan dengan jubah panjang. Pose kepala dan pegangan gunting dipertahankan. Clip Windup/Stun yang sudah ada tetap digunakan.
- Bilah dan gagang gunting mengikuti tulang Shears yang sudah ada. Adaptasi IK pada lengan kanan menjaga pegangan dan jarak bilah dari lantai, tanpa memindahkan root navigasi.
- Rig tetap beranimasi ketika di luar pandangan kamera. Posisi checkpoint tidak dihitung sebagai langkah; prop gunting yang tidak tersedia tidak menghentikan seluruh playback.
- AI, kecepatan pengejaran, collision, damage, foto ritual, save/checkpoint dan scare AR tiga detik tetap memakai sistem yang sudah ada.

Implementasi: `AtmajaThreatPresentation.cs`, `NyonyaMotionRevisionBuilder.cs`, dan `Tools/retarget_nyonya_motion.py`. Asal motion, hash dan adaptasi rig dicatat di `nyonya-motion-retarget.json`; cadence hasil Unity di `nyonya-motion-stride.json`.

## Validasi

37 skenario unik terverifikasi: 32 lulus pada suite awal; lima kegagalan pemeriksaan diperbaiki, kemudian tujuh kasus terkait lulus saat dijalankan ulang (termasuk idle dan regresi hantu yang sebelumnya sudah lulus). Tidak ada perubahan runtime/aset game di antara dua run. Rincian hasil awal dan hasil ulang tetap tersedia di `nyonya-motion-unity-tests.json`.

- Tes hantu memakai actor yang dikirim dalam scene: deformasi mesh tanpa perpindahan root AI, Walk pada 0,9 m/s, Run pada pursuit basement 2,8 m/s, update gerak tidak merata, stun, berhenti, dan aktivasi ulang checkpoint.
- Tiga pengujian clip memakai matriks bone/bind-pose aktual untuk memeriksa deformasi dan lantai. Pengukuran pertama keliru menghitung skala FBX 100x dua kali; perbandingan CPU skinning dan round-trip Blender/FBX mengungkap kesalahan tes tersebut. Batas penetrasi yang diuji adalah 15 mm pada 24 sampel per clip.
- Tes lama diperbarui agar sesuai dengan revisi Raka sebelumnya: anggaran mesh hasil optimisasi 96k triangle, material kulit/kain matte, serta fill untuk pegangan HP pada first-person. Tes chat menunggu maksimal 11 detik game agar interval acak tidak menghasilkan kegagalan palsu. Perilaku game untuk fitur-fitur ini tidak diubah.
- Regresi yang lulus mencakup ritual tiga foto dan hilangnya Nyonya, chat setelah dikalahkan, scare AR tiga detik, emergency Blitz, pelacakan pemain/occlusion, kamera/HP, save/checkpoint basement, serta alur keberangkatan/prolog.
- Python syntax check dan Graphify lokal lulus; graph hanya memuat kode yang diizinkan, tanpa file aset.

Dokumentasi API untuk pengecekan skala: [Unity SkinnedMeshRenderer.BakeMesh](https://docs.unity.com/en-us/engine/6000.6/script-reference/unityengine/skinnedmeshrenderer/bakemesh). Identitas build dan hasil upload dicatat terpisah. Pemeriksaan visual langsung di Windows tetap diperlukan untuk menerima hasil artistik; host pengujian ini menjalankan Unity Linux.

## Build untuk review

- Windows x86_64 Development, Mono, delapan scene: **Succeeded**, 0 error, 8 warning existing pada diagnostik/shader. Laporan Unity: `nyonya-motion-windows-build.json`.
- Identitas 13.088 file sumber diverifikasi setelah build. ZIP berisi semua file build dan lulus pemeriksaan CRC; rincian di `nyonya-motion-build-summary.json` dan `nyonya-motion-phase5-build-sources.json`.
- `Builds/LayarTerakhir-Windows-x86_64-Development.zip`: **3.024.372.126 byte**.
- SHA-256: `16cb33bdcecfb45de06accd2a291b6f99d83dceb858b03c00dd31ff13b5f7723`.

Upload R2 terverifikasi melalui pembacaan ulang penuh objek remote pada **2026-10-26T17:12:12.993809+00:00**. Ukuran dan SHA-256 cocok dengan ZIP lokal. Bukti: `nyonya-motion-r2-upload.json`; objek: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`.
