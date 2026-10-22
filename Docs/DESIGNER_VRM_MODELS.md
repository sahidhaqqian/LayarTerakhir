# Model karakter VRM milik desainer

Revisi 22 Oktober 2026 memakai model yang disediakan pemilik proyek. File VRM
mentah tetap utuh di `Assets/_LayarTerakhir/assets/LayarTerakhir/` pada workspace
asal; file itu tidak digandakan ke Git atau dimuat oleh runtime. Hasil siap game
berada di `Assets/_LayarTerakhir/assets/Art/DesignerCharacters/`.

## Yang dipasang

- **Raka:** menggantikan resource `RakaCharacter` dan `RakaPhoneHand`; terlihat
  pada kamera bahu, first-person hand, berjalan, berlari, jongkok dan naik motor.
- **Nyonya Atmaja:** menggantikan model pemburu dan empat penampakannya yang
  sudah ada: AR duduk, cermin, kilasan gantung, serta sosok di luar rumah.
  Akar actor, collider, AI, pemicu, koordinat dan status cerita tetap memakai
  sistem yang ada. Kemiringan kepala dan gunting berasal dari model desainer.
- **Damar:** disiapkan sebagai prefab authoring
  `DesignerCharacters/Prefabs/DamarDesignerCharacter.prefab`. Rekaman handycam
  yang ada memakai sudut pandang Damar, bukan tubuh Damar yang terlihat; tidak
  ditambahkan kemunculan/adegan baru untuk memaksakan modelnya masuk.
- Arwah, Genderuwo, Merah, Putih, Tuyul dan Wewe tetap menjadi sumber milik
  desainer. Belum dipasangkan karena tidak ada actor yang sesuai pada prolog ini.

## Adaptasi yang diperlukan

VRM yang diberikan tidak memuat klip animasi. Konversi offline mempertahankan
geometri/UV/tekstur karakter, mengurangi topologi, menormalkan ukuran manusia,
dan membakar animasi ke kontrak `Animation` yang sudah dipakai game. Tidak ada
plugin VRM atau solver baru yang berjalan di player.

Raka memakai 16 klip yang sama: idle, berjalan, berlari, jongkok normal/rendah,
varian HP, naik motor dan memasang kartu. Saat naik motor, torso disesuaikan ke posisi
setang sampai kedua lengan dapat menjangkau; panggul tetap bertumpu pada jok.
Pose memakai authoring IK first-party
yang sudah ada, dengan penyesuaian ruang kepala dan siku untuk proporsi baru.
Ibu jari kanan sumber tidak mempunyai vertex berbobot; tiga sendinya dipasang
ke permukaan ibu jari lalu diberi bobot sebelum genggaman HP dikalibrasi.
Kalibrasi tersimpan di `Audit/designer-raka-grip.json` dan diverifikasi lagi pada
hasil impor Unity di seluruh fase klip HP, bukan hanya pose diam Blender.
Crossfade kaki merunduk juga dikoreksi pada tulang menggunakan solver dua sendi
yang sudah dipakai alignment motor; collider/root tidak dipindahkan. Keyword
normal dan metallic mask diikat ulang setelah material beralih ke shader
camera fade, agar pakaian tidak berubah menjadi permukaan logam.

Nyonya sumber sudah dalam pose lengan menekuk, sementara rig otomatisnya masih
berbentuk T. Sendi lengan disesuaikan ke artwork, kemudian diberi lima klip
idle/walk/run/windup/stun. Kepala tidak ditambah kemiringan 76 derajat lagi.
Gunting pada mesh sumber mendapat bone `Shears` untuk hook presentasi yang
sudah ada; tidak dibuat senjata duplikat. Penampakan statis dibakar dari model
yang sama dengan anggaran lebih rendah.

Pakaian Raka baru merupakan satu mesh berjaket. Penampilannya mengikuti artwork
tersebut sejak awal, tanpa varian kaus lama. Item `JAKET_RAKA` dan pemeriksaan
progres pengambilannya tetap ada. Ini perubahan visual yang perlu dinilai
desainer saat review, bukan klaim bahwa sumber menyediakan dua pakaian.

## Anggaran dan sumber

| Karakter | Triangle sumber | Triangle tubuh siap game | Pemakaian |
|---|---:|---:|---|
| Raka | 793.072 | 32.000 | Player aktif + proxy HP |
| Nyonya | 2.137.712 | 21.999 | Pemburu aktif |
| Nyonya penampakan | Sumber sama | Maks. 5.000 per bentuk | Duduk dan kilasan |
| Damar | 933.765 | Maks. 22.000 | Prefab authoring, tidak dimuat prolog |

Albedo karakter maksimal 2048 px, normal serta metallic/smoothness 1024 px,
tetap mipmapped dan terkompresi. Tekstur environment tetap maksimal 1024 px.
Metal/roughness glTF dipetakan ke metallic merah dan smoothness alpha URP;
normal dan mask diimpor sebagai data linear. Validasi total triangle scene
Atmaja, pemburu dan Raka dinaikkan dari 300 ribu menjadi 350 ribu untuk model
ini; itu anggaran authoring, bukan bukti FPS pada komputer target.

Hash sumber, keluaran dan anggaran ada di `Audit/designer-vrm-authoring.json`.
Sumber ini adalah kiriman desainer; jangan mengatribusikannya sebagai aset CC0
MakeHuman dari model lama. Model lama tetap tersedia di direktori asalnya.

## Menghasilkan ulang

1. Salin hanya tiga VRM yang diperlukan ke `Builds/ArtSources/DesignerVRM/`, atau
   berikan `--source-dir` yang menunjuk sumber aslinya. Tool hanya membaca VRM.
2. Untuk regenerasi kalibrasi, pasang SciPy 1.14.1 pada Python authoring yang
   dapat dibaca Blender (NumPy bawaan Blender 4.0.2 kompatibel). SciPy tidak
   masuk ke dependency Unity. Instalasi dari root proyek:
   `python3 -m pip install --target Builds/ArtSources/CharacterPython --no-deps scipy==1.14.1`.
   Lokasi alternatif dapat diberikan lewat `LAYAR_CHARACTER_PYTHON_PATH`. Kalibrasi dengan hash sumber yang sama dipakai
   ulang; sumber berbeda akan meminta perhitungan ulang.
3. Jalankan Blender untuk setiap role:

   ```sh
   blender -b --python Tools/prepare_designer_vrm.py -- --role Raka
   blender -b --python Tools/prepare_designer_vrm.py -- --role Nyonya
   blender -b --python Tools/prepare_designer_vrm.py -- --role Damar
   ```

4. Jalankan `python3 Tools/run_golden_checks.py --stage designer-models`, atau
   menu Unity **LayarTerakhir → Characters → Apply designer VRM models**.
5. Jalankan tes geometri/player, suite regresi, serta capture standalone sebelum
   build Windows. Detail hasil revisi ini ada di `Audit/designer-vrm-review.md`.

Penilaian gaya dan kualitas dibanding model lama tetap milik desainer.
