# Prolog bug 47 — konsistensi lingkungan dan waktu

Diterapkan pada 29 Oktober 2026, melanjutkan `59d027e1`. Status: implementasi dan validasi terarah selesai; penerimaan visual/playtest desainer masih menunggu.

Penyebab yang dibuktikan dari scene dan source:

- `Prolog_KoridorKos` pada HEAD sebelumnya tidak lagi berisi `InterludeAlley`, dan `KosCorridorDirector.buildingRoot` tersimpan sebagai `fileID: 0`. Revisi scene dasar juga mengembalikan posisi kos ke `(0,0,0)`. Ini menghilangkan scenery tambahan dan menggagalkan startup NPC siang.
- Builder scenery sebelumnya menyimpan grup bangunan OFF untuk prolog malam, lalu director siang mengaktifkannya. Bangunan permanen memang berubah menurut waktu.
- Scene koridor memakai `DesignerNightSky` dengan kamera `Skybox`. `ApplyDay`/`ApplyDusk` hanya mengganti backgroundColor, sehingga skybox malam tetap dirender.

Perubahan yang diterapkan:

- Scenery gang, tujuh rumah tambahan, warteg, dua gerobak, kendaraan parkir, dan kabel dipulihkan ke prefab `Assets/Resources/MelatiPermanentStreet.prefab`. Scene menyimpan instance aktif yang sama pada malam maupun siang.
- Startup kos memulihkan prefab jika pass pembuatan scene berikutnya menghapus extension tersebut. Pemasangan idempoten; jemuran, sandal, galon, dan sapu halaman tersedia pada kedua waktu tanpa duplikasi. Penduduk dan lalu lintas tetap mengikuti babak cerita.
- Referensi gedung dipulihkan, kos/anchor dikembalikan ke kavling `x=-16`, dan pagar tembok halaman dikembalikan melalui helper authoring yang sudah ada. Builder dasar kini mengisi referensi gedung ketika membuatnya.
- Skybox pagi dan senja terpisah disertakan sebagai material Resources. Kamera, fog, ambient, matahari, dan intensitasnya ditetapkan eksplisit. Kamar kos menggunakan waktu langit yang sama dengan halaman dan memperoleh matahari melalui jendela. Material langit malam asli tetap dipertahankan.
- Material makanan warteg disimpan sebagai asset persisten agar tetap terhubung dalam prefab.

Validasi yang benar-benar dijalankan:

- Pemeriksaan delimiter C# dan kompilasi Unity Editor 6000.3.26f1 lulus.
- Tes terarah terakhir **22/22 lulus**: seluruh `InterludeCampaignTests`, enam kasus `PrologEnvironmentContinuityTests`, dan runtime `DaytimeCapsuleReachesShopsAndTrafficHasSeatedRiders`. Cakupan meliputi identitas renderer malam/siang/senja, pemulihan prefab satu kali, malam→pagi→senja→pagi, kembali ke scene malam asli, langit kamar, jalan keberangkatan, dan perjalanan kapsul nyata menuju warung/warteg/vendor. Runtime test memakai EnterPlayMode pada Editor headless, bukan player Windows.
- Percobaan tes pertama menemukan tiga kegagalan akibat referensi gedung yang kosong. Referensi dan posisi diperbaiki sebelum tes diulang; receipt percobaan pertama disimpan untuk penelusuran.
- Enam PNG dari render Unity Editor OpenGL/llvmpipe diperiksa: `Audit/Visuals/Prolog-Continuity/{night,morning,dusk}-{alley,street}.png`. Sudut kamera sama untuk setiap waktu. Rumah dan kendaraan tetap terlihat, pagi memiliki langit biru, malam memakai langit gelap asli. Sampel luminansi langit jalan: malam 0.117, pagi 0.612, senja 0.538.
- Graphify diperbarui dan `--check` lulus: 407 file kode, 5116 node, 19341 edge, **0 file aset**.

Bukti: `prolog-continuity-tests.xml`, `prolog-continuity-test-invocation.json`, `prolog-continuity-author-invocation.json`, `prolog-continuity-capture-invocation.json`, `prolog-continuity-visual-validation.json`, `prolog-continuity-graphify-invocation.json`, dan `prolog-continuity-metrics.jsonl` di folder ini. Receipt tes melaporkan 45.07 detik termasuk startup Editor; authoring 24.87 detik, capture 30.28 detik, Graphify 24.84 detik.

Batas bukti: render menggunakan GPU software Editor; tidak mengukur performa GPU target. Belum ada penerimaan playtest manusia untuk revisi ini. Tidak ada build Windows atau upload rilis pada task ini.
