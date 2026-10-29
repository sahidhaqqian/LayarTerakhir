# Revisi playtest prolog 28–46 — 29 Oktober 2026

Foto sumber: sembilan PNG dari `r2:layarterakhir/Layarterakhir/bug/`, dibaca lokal; tidak ada unggahan R2 dalam revisi ini.

| Bug | Implementasi |
|---|---|
| 28 | Hapus komentar live tentang mainan dan boneka yang tidak ada. |
| 29–30 | Buku harian tetap dapat dibaca dengan visual binder notebook unduhan; putar lampu belajar dan arahkan sorotan ke buku. |
| 31–32 | Ganti rakitan kasur/bantal/selimut dengan set Vintage Day Bed; hapus lemari/rak yang rusak dan tempatkan tempat tidur di sudut itu. |
| 33 | Ganti mug prosedural dengan cangkir CC0 unduhan yang diadaptasi memakai serpihan bibir dan retakan halus. Ini bukan hasil scan cangkir retak asli. |
| 34 | Hapus bilah kubus bekas cakar, gunakan foto cakaran asli pada dinding bangunan dari Katmai National Park (PD-US-NPS), diproyeksikan melalui UV crop di dinding kamar mandi. Target foto tidak mengikuti pintu. |
| 35 | Pangkas permukaan atas plafon bawah yang koplanar dengan lantai 3,82m; pisahkan finish kayu dari ubin kamar mandi, pertahankan collider/plafon bawah. |
| 36 | Bak berkaki berongga unduhan, orientasi tegak, permukaan darah oval di bawah bibir bak; dibuka lewat tirai. |
| 37 | Lebarkan bagian utama kamar ke timur menjadi 4,65m, pisahkan dari balkon, set tempat tidur lebih besar, kursi menghadap ruang, jalur pintu dan trapdoor dipertahankan. |
| 38 | Hapus lintel generik mengambang dekat lampu dan tali. Header pintu bernama tetap ada. |
| 39 | Tambah 33 objek sajian/pangan di meja, tanpa collider yang menghalangi jalur/vault. |
| 40 | Meter sembunyi kecil kanan bawah: tiga segmen hijau tetap aktif; tiga kuning ikut aktif saat mendekat; tiga merah ikut aktif saat dekat. Keluar lemari menyembunyikan meter. |
| 41 | Blitz defensif tetap bisa dipakai setelah rekaman: biaya baterai yang sama, stun dua detik, tidak menaikkan hitungan tiga foto akhir. |
| 42 | Buka tiga partisi labirin basement 11,8 × 11,9m sehingga area tambahan dapat dijelajahi bebas. |
| 43 | Tangga besi unduhan setinggi 6,6m dengan rute naik/turun, lubang visual menembus lapisan lantai/plafon, dan lampu penanda di kaki tangga. |
| 44 | Nyonya langsung muncul di titik jendela, tanpa glide dari jalur pengejaran; collision dengan kapsul Raka diabaikan saat menghadang. |
| 45 | Hapus overlay tombol/instruksi kejaran, termasuk HADAPI DIA dan TEROBOS JENDELA, serta kedua objective tersebut. |
| 46 | Binding hunter/apparition dikembalikan ke model animasi Nyonya yang sudah tersedia di proyek (scene awal memakai model lama). Setelah muncul di jendela, Nyonya berjalan pelan menuju pemain dan menyerang bila pemain membiarkannya mendekat; recoil foto tetap berlaku. |

## Sumber aset

- [Poly Haven binder notebook](https://polyhaven.com/a/binder_notebook), [Vintage Day Bed](https://polyhaven.com/a/vintage_day_bed), CC0.
- [OpenGameArt Bath Tube — plaggy](https://opengameart.org/content/bath-tube), [Free Mug Pack #1 — rubberduck](https://opengameart.org/content/free-mug-pack-1), [Metal ladder — nublet](https://opengameart.org/content/metal-ladder-low-poly), CC0.
- [Foto cakaran pada dinding bangunan — Katmai National Park](https://commons.wikimedia.org/wiki/File:Bear_damage_to_fish_freezing_building_at_Brooks_Camp_(20529901541).jpg), Public Domain / PD-US-NPS. Foto asli dipertahankan; shader mengambil area bekas cakar dan menyesuaikan warna AR.
- URL unduhan, SHA-256, checksum penerbit, dan lisensi: `prolog-revision-asset-sources.json`; keluaran lokal: `prolog-revision-derived-assets.json`.

## Verifikasi

- Syntax Python dan kompilasi authoring Unity: lulus.
- Delapan render Editor dari scene aktual diperiksa untuk orientasi aset, lantai, ruang, sajian, dan tangga. Ini bukan capture standalone atau bukti performa Windows.
- Tes akhir terarah **24/24 lulus**, exit code 0; XML `prolog-review-final-tests.xml`, receipt `prolog-review-final-invocation.json`. Total 26 tes unik berstatus lulus setelah mengambil hasil terbaru tiap tes; dua hasil terdahulu dipakai ulang untuk live chat dan daun pintu yang kodenya tidak berubah. Pemeriksaan geometri terakhir setelah memindahkan kabinet: **7/7 lulus**, `prolog-review-shaft-tests.xml`. Rincian semua percobaan dan capture: `prolog-review-validation.json`.
- Penerimaan visual/desain dan playtest manusia belum dilakukan. Model cangkir yang sudah retak dari sumber aslinya belum tersedia bebas unduh; pengganti adaptasi dinyatakan apa adanya. Kandidat berbayar: https://free3d.com/3d-model/ceramic-cup-scratched-cracked-vintage-5362.html (Scyrus, harga halaman saat diperiksa US$19). Keputusan aset bug 33 menunggu pengguna.

Temuan tambahan yang diperbaiki saat validasi: bob visual tangga sebelumnya menambah posisi tubuh setiap frame; sekarang hanya memengaruhi pitch kamera. Peti yang memotong rute diperlebar dipindahkan ke tepi. Kabinet radio di lantai dasar digeser keluar dari volume lintasan tangga agar tidak menembusnya. Capture ambush tetap memeriksa penghalang.
