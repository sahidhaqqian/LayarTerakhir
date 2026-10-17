# Kamera hibrida dan karakter — 17 Oktober 2026

Status: kamera dan presentasi diterapkan, bukti pengujian tersedia, build Windows baru terunggah dan terverifikasi di R2.

Default eksplorasi sekarang **over-the-shoulder third-person**. **V** memilih
first-person dan kembali; Settings menyediakan pilihan yang sama dan remap tombol.
Pilihan disimpan antar-scene/launch. V lama untuk modul selfie dipindah ke X.
Lemari dan standoff Blitz memakai first-person sementara tanpa menimpa pilihan;
cutscene tetap mengendalikan kamera. Spherecast mendekatkan kamera ketika terhalang,
dan tubuh menjadi shadows-only di first-person atau jarak bahu di bawah 0,9 m.
Feed HP mengecualikan layer avatar sehingga fotografi tetap menggunakan lensa HP.
Cahaya pantul tanpa shadow memakai rendering layer khusus tubuh dan mati di first-person.
Pemisahan mengikuti [mekanisme Rendering Layers URP](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/features/rendering-layers-lights.html).

Raka kini memakai mesh skinned 14.336 segitiga, 48 tulang, 11 klip untuk diam,
jalan, lari, jongkok, HP dan motor. Nyonya memakai 15.803 segitiga, 48 tulang,
lima klip untuk diam, patroli, kejar, ancaman dan recoil. Aparisi memakai varian
statis sekitar 2,4k segitiga. Gerak root tetap milik CharacterController/AI;
animasi hanya mengubah tulang. Tinggi pose jongkok Raka di hasil authoring 0,982–0,988 m.

Sumber grafis MakeHuman CC0 dan hash versi disimpan di
[manifest sumber](character-source-manifest.json), dengan [lisensi penuh](makehuman-assets-CC0.txt).
Kebaya memakai [tekstur renda](../Assets/_LayarTerakhir/Art/Characters/Textures/SriKebayaLace-v1.png)
yang dibuat melalui **imagegen bawaan**; [prompt lengkap dan hash](character-generated-cloth.json).
Rok gelap mengadaptasi tekstur Poly Haven CC0 yang sudah dikreditkan.
Model dan gerak ini merupakan adaptasi/authoring proyek, bukan aset DreadOut atau motion capture.
[Pratinjau Blender](Visuals/Character-Authoring) hanya bukti authoring, bukan render game.

Scene kos dan koridor tetap byte-identical. Pass presentasi hanya mengganti visual
aktor Atmaja dan prefab Raka bersama. Material URP mengalami normalisasi alias
saat validasi; tujuh file dan rekonstruksi hash sebelum normalisasi dicatat di
[receipt](character-material-normalization.json). Sumber build dibekukan ulang setelahnya.
Validasi gabungan lingkungan, Nyonya dan Raka: **291.968 segitiga**, tekstur maksimum
1024px. Graphify hanya memindai kode; tidak mengindeks aset.

Kelanjutan final di standalone Linux lulus **130 pemeriksaan / 26 gambar**, berjalan
lebih dari 130 m tanpa reset checkpoint tersembunyi. V diperiksa lewat event keyboard,
foto AR kamar mandi dijalankan dalam first-person; lemari/Blitz memulihkan pilihan.
[Receipt](Visuals/Hybrid-Camera-Final/player-audit.json) dan
[invocation](golden-prolog-player-invocation.json) merekam platform, GPU dan hasil.
Bandingkan [OTS](Visuals/Hybrid-Camera-Final/01-upper-corridor-and-seals.png) dengan
[first-person](Visuals/Hybrid-Camera-Final/01a-first-person-same-corridor.png).
[Standoff](Visuals/Hybrid-Camera-Final/19-emergency-blitz-standoff.png) dan
[title / Continue](Visuals/Hybrid-Camera-Final/24-continue-completed-prolog.png)
berasal dari render game aktual. Navigasi audit menggunakan substep kapsul terarah,
bukan playtest manual; capture membekukan waktu permainan.

Runner awal tetap membawa status jongkok sesudah mengambil handycam. Runner final
melepas CTRL setelah keluar ke lorong yang lega, kemudian memakai kecepatan sprint.
Uji akhir menggunakan AI pengejar yang sama dan memeriksa nol reset checkpoint tersembunyi.

Pembuka dan lantai dasar lulus 50 gambar uji dari menu sampai checkpoint tangga,
termasuk kedua tagihan, jurnal, tangga kos, perjalanan motor, foto AR dan jam.
[Receipt pembuka](Visuals/Hybrid-Camera-Opening-Final/player-audit.json) memakai
[rekaman sumber sebelum koreksi duduk motor](hybrid-opening-build-sources.json).
Pose duduk yang turun 0,4 m kini dikompensasi hanya pada origin model; posisi root
pemain/collider tidak dipindah. Shot motor diuji ulang lewat fixture terpisah yang
mencatat origin pemain/motor/avatar dan posisi kepala.

Satu percobaan suite berhenti sebelum tes pertama ketika garbage collector aset
native Unity menunggu Mono TLS. [Catatan retry](hybrid-test-runner-retry.json)
memisahkan percobaan itu dari hasil suite yang diterima.

Suite Unity lulus **75/75, nol skip**, termasuk V/remap/Settings, override sementara,
collision camera, isolasi feed HP, layer pencahayaan dan kompensasi pose motor.
[XML](golden-tests.xml), [invocation](golden-test-invocation.json) dan
[rekaman sumber suite](hybrid-suite-build-sources.json) menyimpan bukti.
Dua nilai offset shot parkir terakhir digeser untuk menghindari tiang teras;
perubahan framing itu diverifikasi lewat fixture motor setelah suite, dengan
raycast penghalang, viewport kepala dan tiga capture mount/ride/arrival. Fixture
memberi waktu pemuatan Atmaja sampai 120 detik pada host software rendering;
aturan perjalanan game tetap memakai alur yang sama.

Fixture motor final lulus **12 pemeriksaan / 3 gambar**:
[receipt](Visuals/Hybrid-Rider-Final/player-audit.json),
[shot duduk yang bebas tiang](Visuals/Hybrid-Rider-Final/01-mounted-rider.png),
[melewati gerbang](Visuals/Hybrid-Rider-Final/02-rider-through-gate.png),
[kembali berdiri di Atmaja](Visuals/Hybrid-Rider-Final/03-arrival-standing.png).
Fixture ini memakai save halaman terisolasi dan memanggil departure kontekstual;
pemilihan perjalanan melalui UI telah diuji pada pembuka 50 gambar di atas.

Sintaks **82 C# / 41 Python** bersih dan **17 tes tooling Python** lulus tanpa skip.
Graphify diperbarui sesudah perubahan kode terakhir: **123 file kode, nol aset**.
Windows x86_64 Mono Development selesai dengan **0 error / 0 warning**.
ZIP lengkap berisi **304 file / 180,144,189 byte**; semua CRC ZIP dan
keutuhan file build diperiksa. Snapshot akhir mencakup 2.058 file game/resource/meta/settings.
Source snapshot diperiksa lagi sesudah build, dan DLL memuat kamera, animasi,
rendering layer avatar serta fixture rider terbaru.

Unggahan baru ke `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip` berhasil. Objek R2 dibaca ulang seluruhnya;
SHA-256 sama dengan ZIP lokal: `4b6936a90ecd97bdad38812575a3ed0eb0315f35d158c1815c78ac95bc4b47ec`.
[Bukti unggahan](r2-windows-build-upload.json), [ringkasan build](golden-build-summary.json),
[snapshot akhir](prolog-presentation-build-sources.json).

Runtime/FPS Windows belum diuji; pengujian gameplay/gambar menggunakan standalone Linux
llvmpipe. File Windows sudah diverifikasi sebagai PE32+ AMD64; hal itu tidak mengukur
performa target. Paket menyertakan panduan kontrol/playtest dan lisensi grafis MakeHuman.
Kesetaraan artistik DreadOut, naturalitas gerak/ekspresi wajah, acting VO final,
pacing manual dan FPS Windows/Iris Xe tetap memerlukan penilaian/playtest tersendiri.
