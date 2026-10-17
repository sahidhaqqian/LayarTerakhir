# Deep Repository Audit — Layar Terakhir (17 Oktober 2026)

Audit menyeluruh berbasis bukti; **tidak ada file yang dihapus atau diubah** pada tahap ini
(audit-only). Semua temuan bersifat rekomendasi berbukti; keputusan pembersihan menunggu
pemilik proyek. Selain 4 deliverable ini dan `Audit/file-inventory.csv`, tidak ada perubahan
ke skenario, prefab, material, `.meta`, atau kode gameplay.

## 1. Cakupan dan metode

| Dimensi | Verifikasi yang dijalankan |
| --- | --- |
| Pipeline static | CRUD full-file inventory 45.455 file (termasuk Library 41.036 cache) ke `Audit/file-inventory.csv` |
| GUID map | 1.029 `.meta` di `Assets/`, 0 duplikat GUID, 0 file tanpa `.meta` |
| Closure analisis | BFS dari 5 akar runtime (3 scene Build Settings + `SampleScene` (dipakai test) + seluruh `Assets/Resources/`) melalui referensi GUID di 428 file serialized → 658 aset berada di dalam closure |
| Referensi dinamis | `Resources.Load` ×3 (`KosAudio/*`, `LayarPresentation`), 0 AssetBundle, 0 Addressables (paket tidak terinstal), 102 aset dikutip via string di kode Editor/generator, `AddComponent<T>` dicatat per nama kelas |
| Duplikasi konten | SHA-256 seluruh biner di `Assets/` → 23 grup duplikat persis, 19,3MB redundan (rincian di `cleanup-candidates.md`) |
| Kualitas kode | 71 file C# pertama (12.017 baris, 92 tipe), analisis cross-class references, coupling, nama metode duplikat, TODO/HACK/Debug.Log |
| Validasi Tacit | Unity 6000.3.26f1 batchmode: **compile 0 error CS / 0 warning**, **EditMode NUnit 67/67 PASS** (`-testFilter LayarTerakhir.Editor.Tests`) |
| Graphify | Receipt `graphify-out/source-manifest.json` diperiksa; jangan lupa 0 file aset masuk graph **by design** (lihat §4) |

File yang **belum bisa diverifikasi penuh**: aset di belakang nama dinamis (builder membangun
path dari concat string, mis. `stem + "_diff_1k.jpg"`), dan konten audio/dialog baru
sudah siap tetapi belum dipetakan secara serialized (lihat NEEDS INVESTIGATION di
`cleanup-candidates.md`). Asumsi "tidak terreferensi = aman hapus" tidak diterapkan pada
kelompok itu (aturan no. 2 pada tugas audit).

## 2. Ukuran dan penghematan realistis

Total worktree: **4,7GB**, tetapi jumlah ini **bukan** ukuran "repo":
- Git tracked: **3.296 file, ~525MB** (dari 4.707 path-discovery aset source telah dikutip)
- `Library/` (ignored, re-generable): 2.965MB, 41.036 file — termasuk PackageCache 1.387MB
- `Builds/` (ignored, artefak build): 1.134MB — semua hasil build re-producible; deliverable ZIP 174,5MB + `.sha256` sudah terverifikasi di R2 (`r2-windows-build-upload.json`, full readback match)
- `Audit/` (lacak + bukti): 127MB — visual proof 326 file direferensikan eksplisit oleh laporan `.md` audit
- `AssetResearch/`, `Scene2_Atmaja_Assets/`, `Scene2_Atmaja_Recovery/`: 417MB — bukti lisensi CC0/CC-BY + berkas .blend/.py authored yang **tidak dapat diunduh ulang**; sesuai aturan "jangan hapus aset berlisensi/license evidence" → disimpan

Top 30 file dan top 20 folder ada di §5 laporan `file-inventory.csv` (kolom size_bytes).
Pola dominan: paket Burst dylib (144MB), permainan `sharedassets*.assets.resS` yang direplikasi
5× (Builds/Windows ×3 + LinuxAudit + PlayerDataCache), ArtifactDB 37,9MB.

### Penghematan realistis (bukti sudah lengkap, menunggu keputusan pemilik)
| Sumber | Ukuran | Basis bukti |
| --- | --- | --- |
| Duplikasi aset (satu salinan persis digunakan, salinan kembar tidak terreferensi) | **19,3MB** | closure + SHA-256 identik (cleanup-candidates §SAFE-1) |
| TutorialInfo Readme template Unity (tak dipakai game) | ~0,2MB | 0 referensi GUID/kode; standard template |
| Library cache (re-generable, bisa dibersihkan kapan pun via Unity) | ~2,9GB (op) | `Library/` paket cache bisa di-reimport; bukan bagian repo |
| `Builds/Previous`, `Builds/LinuxAudit` | ~645MB (op) | gitignored; dipertahankan karena laporan audit `playtest-fixes-2026-10-16.md`, `kos-final-refinement.md`, dan invocation JSON aktif hari ini merujuk eksplisit ke folder ini sebagai kebijakan "preserved, not deleted" — hapus hanya jika pemilik menyetujui perubahan kebijakan |
| Deliverable Graphify (graph.html dsb. >5MB tracked) | ~5MB (op) | dikontrak AGENTS.md, disimpan |

**Kesimpulan ukuran**: repo git (526MB) didominasi aset pertama + konten berlisensi; sudah
tidak ada sampah besar baru yang bisa dibuang **dengan bukti kuat** kecuali campuran kecil
di §SAFE. Rasio sampah nyata sangat rendah — pembersihan bukan fase yang membebani proyek
ini buat saat ini; risiko broken dependensi jauh lebih besar daripada manfaat ruang.

## 3. Temuan kunci integritas

1. **0 GUID collision**, **0 aset tanpa `.meta`**, **0 missing script** (`m_Script: {fileID: 0}` tidak ada di 3 scene + 6 prefab produksi).
2. 18 "broken reference" tersisa ternyata **false positive**: berkas debug bawaan URP (probe volume/shader debug GUID seperti `03cfc49…`, `b9a23f86…`) yang dikutip `PC_Renderer.asset`/`KosRenderer.asset`/`DefaultVolumeProfile.asset` — GUID yang sama juga dikutip di aset sample milik paket URP sendiri (`Samples~/SharedAssets/Settings/SamplesForwardRenderer.asset`). Bukan kerusakan proyek.
3. Diagnostics yang terlihat GUID-orphan tetap hidup: file di `Scripts/Diagnostics/*` **self-install** via `RuntimeInitializeOnLoadMethod` + flag command line (`-atmajaAudit`, `-goldenAudit`, `-reviewAudit`, `-kosCapture`) — dipakai pipeline audit yang berjalan hari ini (bukti: invocation JSON di `Audit/`).
4. `KosCorridorBuilder` + `KosCorridorBuilding` adalah **satu partial class**, bukan duplikasi.
5. Semua 6 prefab di `Production/Prefabs` dimuat via editor builder (`GoldenSceneProduction`, `CompactKosPolish`) atau dikutip scene `Prolog_KamarKos` — dependensi generator-time, wajib disimpan (lihat §KEEP di cleanup-candidates.md).
6. 4 dialog WAV baru di `Audio/Dialogue/` belum terikat ke scene/prefab/presentation — dialog hasil revisi designer "dialog untuk rekaman pengguna" yang menunggu wiring (item 6 goal.md). **Bukan sampah**; masuk daftar NEEDS INVESTIGATION.

## 4. Mengapa audit Graphify lapor "0 assets"

Deteksi: `Tools/update_graphify.py` + `.graphifyignore` (deny-by-default) **membatasi** discovery hanya
ke `.cs` di `Assets/_LayarTerakhir/{Scripts,Editor,Tests}` plus `Tools/**/*.{py,sh}`; receipt
terminales: `mode: "local-code-only; no LLM/community labeling"`, `code_file_count: 109`,
`referenced_source_count: 109`, `asset_file_count: 0`.

Jadi "0 assets" **bukan kegagalan pemindaian**, tetapi kontrak Graphify dalam proyek ini
(dokumentasi `Tools/GRAPHIFY.md` + AGENTS.md): Graphify sengaja hanya memetakan kode
first-party; pemantauan aset dilakukan terpisah lewat audit GUID seperti pada repositori ini.
Scannernya terbukti benar-benar membaca seluruh 109 source: `sources_sha256` +
`allowed_source()` di `Tools/update_graphify.py` memvalidasi setiap sumber berasal dari
allowlist (`code_file_count == referenced_source_count == 109`), dan graph hasilnya
mengandung 1.529 node / 5.086 edge dari 109 file tersebut. Aset yang tidak masuk graph
tidak berarti tidak diaudit; auditnya ada di `Audit/file-inventory.csv` + closure pasal §1.

## 5. Validasi

- Unity batchmode `6000.3.26f1`, serangkaian EditMode: **67/67 PASSED**, 0 gagal, 0 skipped (3,45s; result XML tersimpan di pemroses lokal — hasil bertanda result="Passed"; tidak di-commit karena berisi runner state yang churned tiap run).
- Compiler: `grep 'error CS\|warning CS'` pada log Unity → **0** untuk keduanya.
- Status git selama audit: hanya artifact baru di `Audit/` (4 JSON validator berubah = timestamp churn; direvert agar deliverable bersih).
- Keterbatasan eksplisit: bukan Play Mode (nographics), tidak memverifikasi visual per frame, tidak memverifikasi target iOS/Mac; properti runtime build tidak diuji di sini (bukan inti cleanup).

## 6. Rekomendasi prioritas

1. **Aman untuk dieksekusi sekarang** — lihat `cleanup-candidates.md` §SAFE (19,3MB duplikat terbukti + Readme template Unity). Pembersihan kecil, nol risiko dependensi.
2. **Tunggu keputusan kebijakan** di `cleanup-candidates.md` §NEEDS (4 dialog WAV baru, sisa tool debug, historis Builds/Previous) — bukti tidak lengkap untuk "pasti aman".
3. **Refaktor kode** — lihat `refactor-candidates.md`; Saya urutkan berdasarkan risiko vs manfaat; dua item berflag rata-rata (KosVisual* trilogy, HudController coupling 31).
4. Dokumentasi (Phase*.MD, deepseek.md, Review-Argon.md) = user-authored design docs; audit tidak menandai satu pun sebagai sampah kecuali bagian status memang usang; direkan design owner yang memutuskannya.
