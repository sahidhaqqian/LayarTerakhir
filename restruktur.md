# TASK: Repository Structure Migration — Layar Terakhir

Lakukan restrukturisasi direktori project Unity **Layar Terakhir** agar mengikuti konsep organisasi ProjectNera, yaitu pemisahan jelas antara `assets/` dan `src/`.

**PENTING: INI BUKAN REFACTOR CODE.** Jangan mengubah implementasi, gameplay, arsitektur, atau perilaku game. Fokus hanya pada reorganisasi file dan perbaikan referensi path yang diperlukan.

## 1. Expected Directory Structure

Target struktur:

```text
LayarTerakhir/
├── Assets/
│   └── _LayarTerakhir/
│       ├── assets/
│       │   ├── Scenes/
│       │   ├── Models/
│       │   ├── Textures/
│       │   ├── Materials/
│       │   ├── Audio/
│       │   ├── Animations/
│       │   ├── Prefabs/
│       │   ├── UI/
│       │   ├── Lighting/
│       │   └── ...
│       │
│       └── src/
│           ├── Core/
│           ├── Gameplay/
│           ├── Player/
│           ├── Interaction/
│           ├── Inventory/
│           ├── Dialogue/
│           ├── Audio/
│           ├── UI/
│           ├── Scenes/
│           ├── Editor/
│           ├── Tests/
│           └── ...
│
├── Packages/
├── ProjectSettings/
├── Tools/
├── Docs/
└── AGENTS.md
```

Struktur di atas adalah referensi target, bukan daftar folder wajib. Pertahankan subfolder tambahan yang memang dibutuhkan proyek. Jangan membuat folder kosong yang tidak diperlukan.

Jangan memaksakan semua file Unity ke dua folder tersebut jika melanggar konvensi engine. Pertahankan folder khusus seperti `Resources`, `StreamingAssets`, `Plugins`, `Editor`, dan dependensi pihak ketiga pada lokasi yang kompatibel.

## 2. STRICT RULES — NO REFACTOR

DILARANG:
- Refactor source code.
- Mengubah logika C#.
- Mengubah nama class, method, property, atau namespace.
- Mengubah gameplay, scene behavior, UI behavior, atau sistem interaksi.
- Mengubah isi scene dan prefab kecuali referensi path yang benar-benar diperlukan.
- Mengganti atau membuat ulang GUID Unity.
- Menghapus aset, script, atau dependensi yang masih digunakan.
- Melakukan optimasi, code cleanup, atau redesign arsitektur.
- Menginstal dependensi baru tanpa kebutuhan langsung.

DIIZINKAN:
- Memindahkan file dan folder.
- Mengorganisasi subfolder sesuai kategori.
- Memperbaiki referensi path yang terdampak migrasi.
- Menyesuaikan konfigurasi assembly, tooling, dan build yang bergantung pada path.
- Mengubah dokumentasi struktur direktori.

**Setiap perubahan isi kode wajib terbatas pada referensi lokasi file yang berubah. Jangan melakukan perubahan lain.**

## 3. Migration Procedure

1. Baca `AGENTS.md` dan audit seluruh repository.
2. Petakan struktur folder lama ke struktur target.
3. Identifikasi semua dependency dan hardcoded path sebelum memindahkan file.
4. Pastikan working tree aman dan buat checkpoint Git.
5. Lakukan pemindahan secara bertahap menggunakan `git mv` bila memungkinkan.
6. Pindahkan file `.meta` bersama file atau folder terkait, dengan GUID tetap identik.
7. Perbarui seluruh referensi path yang terdampak, termasuk:
   - `AssetDatabase.LoadAssetAtPath`
   - `AssetDatabase.FindAssets` dan path editor lainnya
   - `Resources.Load`, jika lokasi relatif Resources berubah
   - Scene loading dan build scene references
   - Assembly definitions (`.asmdef`/`.asmref`)
   - Build scripts, automation, dan konfigurasi test
   - Dokumentasi atau tooling yang menggunakan path lama
8. Verifikasi tidak ada missing script, missing asset, broken prefab, atau broken scene reference.
9. Jalankan verifikasi compile/build jika Unity Editor tersedia. Jika tidak, lakukan static validation dan laporkan batas pengujiannya.
10. Ikuti aturan commit/push yang berlaku di `AGENTS.md`, setelah validasi yang tersedia dilakukan.

Untuk folder root seperti `AssetResearch`, `Scene2_Atmaja_Assets`, dan `Scene2_Atmaja_Recovery`, audit dulu apakah isinya aset aktif, referensi penelitian, atau backup. Pindahkan hanya yang sesuai kategori dan aman. Jangan menghapus data atau memindahkan backup ke jalur runtime tanpa alasan.

Jangan menyentuh folder generated seperti `Library/`, `Temp/`, `Obj/`, `Logs/`, atau `Build/` untuk tujuan restrukturisasi source.

## 4. Validation Requirements

Wajib pastikan:

- Seluruh GUID Unity dipertahankan.
- File `.meta` tetap berpasangan dengan asetnya.
- Tidak ada file hilang setelah migrasi.
- Semua script masih direferensikan dengan benar.
- Referensi scene dan prefab tetap valid.
- Tidak ada perubahan gameplay.
- Tidak ada perubahan kode selain referensi path yang diperlukan.
- Semua tooling penting tetap menggunakan path yang benar.

Gunakan `git diff --find-renames` dan pemeriksaan sebelum/sesudah untuk membuktikan bahwa perubahan hanya berupa perpindahan file dan penyesuaian referensi.

## 5. Final Report

Setelah selesai, laporkan:

1. Struktur direktori akhir.
2. Jumlah file yang dipindahkan.
3. Mapping folder lama → folder baru.
4. Daftar file kode/config yang referensi path-nya diubah.
5. Hasil validasi GUID, assets, prefab, scene, compile, dan build.
6. Perubahan yang sengaja tidak dilakukan beserta alasannya.
7. Status Git commit/push.

**PRIORITAS UTAMA: Struktur repository rapi, seluruh functionality existing tetap identik, dan tidak ada refactor.**

Jangan mulai mengembangkan fitur baru atau melanjutkan implementasi gameplay. Selesaikan hanya tugas restrukturisasi ini.
