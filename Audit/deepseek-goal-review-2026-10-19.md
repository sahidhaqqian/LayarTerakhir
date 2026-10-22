# Review penerapan goal.md dan revisi lanjutan

Baseline yang diperiksa: `fd908f4` dan commit terbaru `d15d8a8` (19 Oktober 2026).
Batas penilaian: penerapan rekomendasi Gemini di `goal.md`, presentasi pemain/HP,
dan regresi alur prolog. Bukan audit seluruh game atau bukti kualitas setara DreadOut.

## Kesimpulan review

**Sebagian sudah sesuai, tetapi belum lengkap.** Cookie senter/motor, soft shadows,
rim khusus hantu, dither tubuh, bank langkah, mode fokus, dan stun basement sudah
ada. Tes baseline mencatat 109 lulus, tetapi beberapa kasus penting belum diuji.
Perubahan finale menjadi tiga foto dan Flash Beam merupakan desain tambahan pada
commit tersebut; itu bukan persyaratan eksplisit dari `goal.md`. Revisi ini
mempertahankan desain finale tersebut dan mengoreksi validasi bidikannya.

| Bagian | Temuan pada commit yang direview | Tindakan revisi ini |
|---|---|---|
| Senter/motor dan siluet hantu | Soft shadows, cookie 256 px dan rendering layer khusus sudah aktif | Dipertahankan; dokumen standar diperbarui agar sesuai konfigurasi aktual |
| Tubuh dekat tembok | Shader dither dan material runtime sudah digunakan | Dipertahankan, termasuk bayangan tubuh dan pilihan perspektif |
| Mode fokus | Panel HP vertikal hanya memenuhi tinggi layar; feed tetap 256×448 | Fokus mengisi viewport dengan aspek layar, feed maksimal lebar 960 px; kembali 256×448 ketika fokus dilepas |
| Tahan klik kanan | Shortcut hanya memaksa Camera saat awal tekan dan selalu menurunkan HP saat lepas | Tahan selalu memaksa aplikasi Camera, termasuk jika menu dibuka saat tombol masih ditahan; lepas memulihkan aplikasi/status HP sebelumnya; kamera wajib pada finale tetap dipertahankan |
| Validitas foto/Blitz | `TryPhoto` dan stun basement tidak memeriksa target dalam bidikan/terhalang dinding | Memeriksa frustum lensa HP, jarak, baterai hidup, dan raycast penghalang; tembakan meleset tidak memajukan ritual/menghabiskan Blitz |
| Langkah kaki | Fase yang telah ditambah satu disimpan kembali sehingga siklus berikutnya keliru | Unwrap hanya interval saat ini, simpan fase normal, reset saat arah playback berubah; tes sepuluh siklus maju/mundur |
| Materi suara langkah | Tanah/kayu/batu adalah olahan tiga rekaman ubin dengan noise/nada sintetis | Lima rekaman asli per permukaan dari Kenney CC0: concrete, grass, wood; provenance dan lisensi disimpan |
| Peredaman hantu | Filter seretan memakai `CanSeePlayer`, termasuk jarak dan arah pandang; hum dan seretan chase tidak mendapat filter yang sama | Filter terpisah untuk hum dan seretan, raycast geometri tanpa FoV, tetap berjalan saat chase; trigger dan collider diri diabaikan |
| Gerakan patah-patah | Logika baru hanya berjalan pada fallback; cabang animasi authored keluar sebelum efek diterapkan | Cabang authored benar-benar disampel pada langkah waktu 1/12 detik ketika berburu/mengejar; navigasi/collider tetap mulus dan animasi stun dipertahankan |
| Pelarian setelah tiga foto | Tes standalone menemukan hantu yang sudah nonaktif tetap bernavigasi dan menangkap pemain ketika merunduk di luar jendela | Navigasi dan pemeriksaan tangkap mensyaratkan hantu aktif; tes menyentuhkan hantu nonaktif ke pemain dan menunggu melewati masa aman |
| Reset checkpoint | Timer stun sementara tidak direset | Timer dan pose stun direset saat pemulihan checkpoint |
| Musik gamelan dan akting | Musik kejaran, hum, dan dialog sintetis masih ada | Belum diganti: perlu rekaman/instrumen berlisensi dan akting manusia; perubahan filter/footstep bukan pengganti akting |
| Stun saat mengejar di tangga | Commit menyediakan stun basement, belum memperluasnya ke fase chase | Tidak diperluas dalam revisi presentasi ini; perlu keputusan balancing baterai/risiko kejaran |
| Variasi boss Babak 1–5 | Rekomendasi desain jangka panjang | Tidak dikerjakan dalam revisi presentasi prolog ini |

Bukti kode utama: `Prolog/AtmajaPhoneView.cs`, `Prolog/AtmajaChaseSequence.cs`,
`Prolog/AtmajaBasementHorror.cs`, `Prolog/AtmajaThreatPresentation.cs`,
`Audio/PlayerFootsteps.cs`, `Audio/ThreatAudioOcclusion.cs` di
`Assets/_LayarTerakhir/src`. Sumber suara: `Audit/review-footstep-sources.json`.

## Catatan sebelum/sesudah kontrol pemain

- Berjalan dan jongkok: percepatan, kecepatan, collider, postur dan klip gerak
  dari revisi sebelumnya dipertahankan. Yang diperbaiki adalah pemicu suara
  telapak setelah banyak loop animasi dan sumber suara permukaannya.
- Pegangan HP: resource tangan/jari dan transisi angkat/turun sebelumnya tetap.
  Tahan klik kanan dari jurnal/menu kini langsung masuk ke kamera; melepasnya
  memulihkan aplikasi dan status HP sebelum membidik. Dialog/modal wajib tetap
  memegang kontrol supaya puzzle dan pembacaan catatan tidak terlewati.
- Kamera: panel fokus vertikal diganti feed seluas viewport dengan rasio layar
  yang benar. Lensa yang menampilkan bidikan juga dipakai untuk memvalidasi hit.
  Pilihan third-person/first-person dan pembacaan dokumen tetap dipertahankan.
- Hasil native: 30 pemeriksaan dan 17 screenshot pada 1280×720. Screenshot
  `10a-held-fullscreen-viewfinder.png` dan `10b-release-restores-journal.png`
  memperlihatkan kondisi tahan/lepas. Identitas 11 source/resource yang relevan
  diverifikasi pada `deepseek-review-feel-continuity.json`.

## Isolasi pekerjaan

Penyelesaian dikerjakan di worktree `LayarTerakhir-review`. File VRM baru di
workspace asal tidak dipindahkan, diubah, atau dimasukkan ke build revisi ini.
Cache Unity worktree dibangun ulang setelah cache salinan memunculkan referensi
script hilang; tes finale dan suite lengkap kemudian lulus dari cache baru.
Unity juga melakukan migrasi serialisasi otomatis dua URP Asset ke versi 13;
resolusi, jarak bayangan dan preset kualitas utama tidak disetel ulang.

## Batas dan risiko

- Concrete mewakili jalan/perkerasan, grass mewakili tanah. Ini bukan rekaman
  cipratan air di aspal basah atau tanah berlumpur; tidak diklaim demikian.
- Render fokus dan bayangan kamera HP menambah biaya GPU. Batas 960 px bukan
  hasil pengukuran FPS Iris Xe; frame pacing Windows perlu diuji di perangkat asli.
- Raycast audio memodelkan penghalang langsung, bukan simulasi akustik/difraksi.
- Pertarungan baru/variasi boss dan rekaman manusia tetap pekerjaan berikutnya.
- Tidak ada aset/kode proprietary DreadOut yang disalin.

## Validasi dan build

Validasi final 22 Oktober 2026:

- **113/113 tes Unity lulus**, tanpa gagal/skip. Termasuk input tahan/lepas RMB,
  pemaksaan menu HP ke kamera, validitas bidikan, AR, bukti foto, checkpoint,
  save/load, dan hantu nonaktif yang tidak boleh menangkap pemain.
- **Standalone player feel: 30 pemeriksaan lulus, 17 screenshot 1280×720**.
  Run ini bertanggal 19 Oktober; kesinambungan 11 source/resource kamera,
  gerak, HP dan karakter ke revisi final diverifikasi terpisah. Bukan klaim rerun.
- **Standalone prolog final: 153 pemeriksaan lulus, 32 screenshot 1280×720**,
  dimulai dari fixture checkpoint tangga Atmaja (progres kos/lantai dasar sudah
  tersimpan), rute kontinu 128,70 m, tanpa reset checkpoint tersembunyi. Ini
  verifikasi jalur akhir terarah, bukan playtest manual seluruh prolog dari New Game.
  Mencapai judul tamat,
  menyimpan completion, lalu Continue memulihkan judul tanpa membayar donasi ulang.
- Build Linux baru selesai tanpa error; source/aset tetap cocok dengan snapshot
  2.519 file setelah build. Windows x86_64 Development (Mono) juga berhasil,
  0 error. ZIP lengkap 309 file (203.494.655 byte), CRC dan 112 assembly diverifikasi.
  Upload R2 berhasil dan seluruh objek dibaca ulang: ukuran serta SHA-256 sama
  dengan ZIP lokal (`r2-windows-build-upload.json`).
  SHA-256 ZIP: `a203db40a86953554e7c5e3ab00b7ad6ddd76317764574feb921fa7ab7fed74e`.
- Pengujian runtime di atas memakai Linux llvmpipe. Playtest manual Windows,
  akting suara dan FPS/perangkat target tetap perlu dinilai pada komputer pemain.

Bukti: `golden-tests.xml`, `deepseek-review-test-summary.json`,
`Visuals/DeepSeek-Review-Feel/player-audit.json`,
`Visuals/DeepSeek-Review-Prolog-Final/player-audit.json`, dan
`deepseek-review-preservation.json`.
