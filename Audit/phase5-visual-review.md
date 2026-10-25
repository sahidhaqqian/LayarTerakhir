# Phase 5 — pemeriksaan presentasi standalone

Capture terakhir: `phase5-native-visual.json`, 25 Oktober 2026 23:34 UTC, Linux development / OpenGL / llvmpipe pada 1280×720. Dua belas gambar utuh disimpan di `Visuals/Phase5/`. Fixture ini mengatur pose/kamera dan menonaktifkan simulasi untuk komposisi; bukan playthrough atau pengukuran FPS Windows. Subtitle pembuka pada fixture tidak membuktikan timing subtitle di lokasi yang difoto.

| Bagian | Masalah sebelum revisi sesi ini | Perubahan dan bukti akhir | Batas penilaian |
|---|---|---|---|
| Gerbang/pasar | Ambient standalone terlalu gelap, material karakter memantul berlebihan, lane kurang terisi | Ambient SH eksplisit, lampu praktikal, meja baru dan material karakter khusus instance; `01-market.png` | Karakter dan jalur terbaca, tetapi perpaduan gaya kios/prop masih perlu review artistik. |
| Koridor Beku | Pintu berpotensi hilang dari satu sisi akibat backface dan collision facade | Material facade dua sisi dan collider pintu nyata; `03-frozen-corridor.png` serta scene tests | Dinding/ruang masih berulang; jangan menyebut kepadatan dekorasi sudah final. |
| Jembatan | Jalur dan cover perlu terpisah dari dekor | Source papan, railing dan bambu baru; `04-bamboo-bridge.png`, tes hole/collider/NavMesh | Fixture tidak membuktikan seluruh animasi papan runtuh; pemeriksaan fisik dan route native terpisah. |
| Arena | Tripod ikut melayang bersama layar; perlindungan dinamo terlalu pendek | Hanya modul layar yang digantung, cage dari panel pagar sumber, cover diperpanjang dengan collider fisik; `08-boss-posed.png` | Arena sengaja terbuka untuk navigasi boss, masih perlu penilaian artistik dan telegraph di hardware target. |
| Damar/epilog | Pose tangan terangkat karena referensi rig retarget; rumah berada di tepi ground | Motion Idle/Walk Quaternius baru, normalisasi pose import Unity, ground dan pijakan rumah diperbaiki; `09-epilogue-normal-posed.png`, `phase5-human-idle-validation.json` | Kedua tangan di bawah kepala; belum akting sinematik final. Horizon set dawn masih terbuka dan butuh review komposisi manusia. |

`09-epilogue-{bad,normal,true,secret}-posed.png` memperlihatkan empat fixture kamera, bukan empat cutscene lengkap. Keempat alur input, lock, replay dan CP9 memiliki laporan native tersendiri. Ending TRUE dan NORMAL berbagi lokasi fajar sesuai cerita; membedakan pilihan tidak bergantung pada foto pose ini.

Tidak ada klaim kesetaraan DreadOut dari capture ini. VO manusia, konsistensi gaya semua model, motion/ekspresi sinematik, pacing, serta profil GPU Windows adalah batas penerimaan yang masih terbuka. Build Windows disediakan untuk review langsung, dengan panduan `phase5-windows-review.md`.
