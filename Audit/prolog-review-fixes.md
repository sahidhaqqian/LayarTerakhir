# Perbaikan playtest prolog — 26 Oktober 2026

## Dinding kos lantai 1

Ditemukan segmen koridor berakhir di z=11, sementara sekat kamar belakang berakhir di z=12. Pemain dapat menerobos ke kamar barat lantai 1 melalui celah tersebut. Sisi timur juga punya celah visual meskipun return tangga menyulitkan kapsul menembusnya.

Dua segmen dinding memakai mesh/material/collider dinding existing, menutup z=11..12. Akses tangga z=12..13 tetap terbuka. Perubahan tersimpan di `Prolog_KoridorKos.unity`; generator `KosCorridorBuilding.HallWallWithDoorways` juga diperbaiki agar rebuild tidak membuka lubang lagi. Kamar pembuka Golden tidak dibangun ulang.

## Streaming sesudah Nyonya kalah

`AtmajaChaseSequence.ThreatDefeated` membaca state existing dan status figur sesudah Flash Beam. `AtmajaPhoneView` menghapus antrean serta tampilan komentar panik lama, menggantinya dengan komentar lega satu per satu pada interval lebih renggang. Petunjuk menuju gerbang memakai bahasa keluar, tanpa mengarang pengejaran baru. Ketika checkpoint mengulang pengejaran, komentar panik kembali sesuai ancaman nyata. Reaksi ending tentang Damar tetap mengikuti adegan existing.

## Mengambil kartu dari HP Damar

Pose `InstallCard` sebelumnya dimulai saat Raka masih di kolong meja. Sekarang HP diambil rendah, Raka mundur menggunakan CharacterController sambil merunduk, lalu memeriksa headroom dan berdiri sebelum pose pemasangan dimulai. Jalur dari belakang meja tidak lagi memotong kaki meja secara diagonal. Kamera close-up memakai sphere cast terhadap geometri ruangan; retakan HP ikut bergerak bersama prop.

Jika ruang keluar tetap terhalang, kartu/APK belum diberikan, HP kembali ke posisi semula dan kontrol pemain dikembalikan sehingga bisa mencoba lagi. Commit checkpoint dan unlock AR tetap memakai alur existing, sesudah presentasi berhasil.

## Penampakan AR di kursi

Jendela penampakan dibatasi tiga detik waktu permainan aktif. Sesudahnya figur hilang meskipun AR tetap menyala. Toggle AR dan Continue tidak mengulang penampakan yang sudah tercatat melalui `EVENT_P0_AR_REVEALED`. Petunjuk AR untuk puzzle jam tetap tersedia terpisah.

## Verifikasi

- **41/41 pengujian Unity lulus**, bukti `prolog-review-tests.xml`.
- Kapsul berdiri/jongkok diuji dari dua arah pada dinding yang ditutup. Uji mereproduksi celah barat ketika segmen dinonaktifkan, kemudian membuktikan collision setelah dipulihkan.
- Rute kedua flight tangga dan fasilitas kos tetap bisa dilalui.
- Play Mode memeriksa tiga foto Nyonya, penghapusan backlog panik, petunjuk keluar yang tenang, AR tiga detik, clue jam, toggle/reload, pengambilan kartu dari sisi/belakang, dan jalur terhalang.
- Saat pose pemasangan aktif, sampel vertex mesh Raka dan posisi HP diperiksa terhadap bounds collider permukaan meja: tidak berpotongan.
- Regresi presentasi pemain dan interaksi/desain existing ikut lulus. Tidak mengubah sistem save, inventori, AR, maupun schema campaign.

Pengujian berjalan di Unity Editor dengan renderer nonaktif. Penilaian visual dan playtest manual Windows masih diperlukan. Build Windows selesai dengan 8 scene, 0 error dan 8 warning existing pada BuildReport. ZIP lengkap lolos CRC dan pemeriksaan runtime PE AMD64.

## Build untuk diuji

- `Builds/LayarTerakhir-Windows-x86_64-Development.zip` — 3.024.159.748 byte.
- SHA-256: `67b30f6e8a12da3d249c5ba5137b744ef5a7fd971cfa007541e7e92760b6ac5e`.
- R2: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`, pembacaan ulang penuh cocok pada 26 Oktober 2026 14:11:46 UTC.
- Receipt: `prolog-review-r2-upload.json`; build: `prolog-review-windows-build.json`; 13.084 sumber beku: `prolog-review-phase5-build-sources.json`.
