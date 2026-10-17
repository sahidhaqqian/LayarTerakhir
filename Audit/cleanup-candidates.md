# Cleanup Candidates — Deep Audit Layar Terakhir (17 Oktober 2026)

Klasifikasi: `SAFE TO DELETE` (bukti kuat), `NEEDS INVESTIGATION`, `KEEP`, `REFACTOR`.
**Tidak ada penghapusan yang dilakukan pada tahap audit** — setiap entri menyertakan bukti dan
dampak potensialnya. Angka ukuran mencakup file + `.meta`-nya.

## SAFE TO DELETE (bukti lengkap; menunggu persetujuan eksekusi)

### SAFE-1. Duplikat tekstur konten persis (19,3MB)

Basis bukti:
1. SHA-256 identik dengan salinan kembar yang **terreferensi** oleh builder aktif
   (`AtmajaUpperBuilder.Textured()` membangun path `Art/Atmaja/Textures/<stem>_diff|_nor_gl|_arm_1k.jpg`).
2. GUID dari salinan mencurigai **tidak muncul** di 428 file serialized yang dipindai
   (closure orphan) dan tidak di-load lewat `Resources.Load`.
3. Pola nama `_Diffuse_1k` (huruf kapital D, penamaan bawaan unduhan PolyHaven) **tidak ada**
   di kode first-party (`grep` hasil kosong) — salinan tidak disentuh builder/scene.

| Salinan tidak terreferensi (identik byte-for-byte dengan salinan yang digunakan) | Ukuran |
| --- | --- |
| `Art/Imported/PolyHaven/decrepit_wallpaper/` ×3 jpg (Diffuse, nor_gl, arm) | 1,9MB |
| `Art/Imported/PolyHaven/old_wood_floor/` ×3 jpg | 1,7MB |
| `Art/Imported/PolyHaven/stained_pine/` ×3 jpg | 2,2MB |
| `Art/Imported/PolyHaven/ceiling_interior/` ×3 jpg | 3,1MB |
| `Art/Imported/PolyHaven/quatrefoil_jacquard_fabric/` ×3 jpg | 2,6MB |
| `Art/Imported/PolyHaven/painted_concrete/` ×3 jpg | 2,4MB |
| `Art/Imported/PolyHaven/dirty_tiles/` ×3 jpg | 1,6MB |
| `Art/Atmaja/Textures/security_light_security_light_glass_mask.png` | 1,9MB |
| `Art/Atmaja/Textures/security_light_security_light_bulb_mask.png` | 1,9MB |

Dampak: nol terhadap visual/gameplay — versi yang tersisa dan dipakai builder tetap ada.
Aturan "jangan menghapus aset berlisensi/license evidence" tidak dilanggar: file license
evidence tidak disentuh, dan yang dihapus hanya duplikat persis (lisensi tetap terpenuhi oleh
salinan kembar yang dipakai).
Catatan: file lain di `Art/Imported/PolyHaven` yang juga GUID-orphan **tidak** masuk daftar
ini karena buildernya memuat nama berpola CONCAT string per-stem; mereka wajib verifikasi
per-stem secara manual sebelum berstatus aman (lihat NEEDS item 8).

### SAFE-2. Template bawaan Unity: Readme + TutorialInfo (redundan dengan docs proyek)

- `Assets/Readme.asset` (+meta), `Assets/TutorialInfo/` (Layout.wlt, Scripts/Readme.cs,
  Scripts/Editor/ReadmeEditor.cs + metas) — 13 file / 0,1MB.
- Bukti: GUID tidak muncul dalam closure maupun serialized; kelas Readme/ReadmeEditor tidak
  punya referensi cross-class; dokumentasi proyek (README.MD, ARCHITECTURE.MD, GDD) mandiri.
- `Assets/Scenes/SampleScene.unity` dari template yang sama TIDAK dihapus — dipakai
  `FoundationAuditTests` → masuk KEEP.

Dampak: murni kosmetik; tidak mengubah alur kerja Unity maupun gameplay.

---

## NEEDS INVESTIGATION (bukti tidak cukup untuk menghapus)

1. **4 WAV dialog baru `Audio/Dialogue/`** (`2352514cfda0.wav`, `619849bcfa57.wav`,
   `bdd9f805ea45.wav`, `fc309d829b2b.wav`) — belum dikutip scene/prefab/
   `PresentationProfile:LayarPresentation`. Namun goal.md item 6 baru saja menyiapkan dialog
   untuk rekaman pengguna; kemungkinan besar akan di-wire nanti lewat
   `PresentationProfile.voices` (inspector). KEEP, cek ulang iterasi berikut.
2. **`Editor/TempShaftProbe.cs`** — util debug berjudul "Temp"; outputnya terdokumentasi
   pada `Audit/designer-corridor-probe-before-fix.txt` pada revisi designer kemarin; tidak ada
   invocation JSON aktif untuknya. KEEP (biaya simpan kecil).
3. **`Editor/KosRoomSurvey.cs`** — utility survey 33 baris; tanpa rekam invocation;
   KEEP sampai pemilik menyatakan sudah ditinggalkan.
4. **Keluarga editor visual**: `KosVisualCapture/Upgrade/V2/KosRenderDiagnosis` — dipakai oleh
   pipeline yang tercatat (`Audit/golden-*.json`, invocation stacks) dan mendefinisikan stage
   pada scene produksi; jangan dihapus meskipun ada sebagian pola duplikat antar-stage (dipetakan
   ke refactor-candidates.md sebagai konsolidasi, bukan penghapusan).
5. **`Audit/VoiceSamples`, `VoiceCandidates`, `Recording`, `VoiceAuthoringAttempts`** — bahan
   rekaman/sintesis yang mungkin dipakai ulang ketika designer merekam lagi; KEEP.
6. **`Builds/Previous/**` (~300MB)** — gitignored; berisi snapshot rollback golden;
   `playtest-fixes-2026-10-16.md` menyatakan eksplisit "Build lama disimpan di
   Builds/Previous/, tidak dihapus". Menghapusnya bertentangan dengan catatan kebijakan
   dokumen; diperlukan putusan pemilik bila mau melepas kebijakan itu.
7. **`Assets/Scenes/SampleScene.unity`** — tidak ada di Build Settings, tetapi dipakai
   `FoundationAuditTests.cs` → KEEP.
8. **Aset model/tekstur PolyHaven lain yang GUID-orphan (~53MB termasuk meta)** — banyak yang
   di-load builder lewat path hasil CONCAT (`<stem>_1k.fbx`); bukti static tidak cukup
   menunjukkan amat tidak terpakai. Perlakukan sebagai kandidat per-stem di fase eksekusi,
   bukan bulk-delete.

---

## KEEP (bukti penggunaan aktif atau kontrak proyek)

| Item | Bukti |
| --- | --- |
| `Library/` (41.036 file, 2,9GB) | Cache Unity standar; gitignored; re-generable; bukan beban repo |
| `Builds/Windows`, `Builds/LinuxAudit`, ZIP + `.sha256` | Deliverable aktif; hash sama dengan records R2 (`r2-windows-build-upload.json`); invocation JSON hari ini memakai `Builds/LinuxAudit/LayarTerakhir.x86_64` |
| `Audit/Visuals/**` (326 file, 120MB) | Direferensikan eksplisit oleh 10 laporan `.md` audit sebagai evidence before/after |
| `AssetResearch/`, `Scene2_Atmaja_Assets/`, `Scene2_Atmaja_Recovery/` (797 file, 417MB) | Bahan sumber + bukti lisensi CC0/CC-BY; .blend & build script authored tidak dapat diunduh ulang; aturan "jangan hapus aset berlisensi" |
| `Scene2_*/manifest/**`, `asset-licenses.md`, license raw | Kontrak license evidence |
| `Scene2_*/.cache`, zips | Sudah gitignored; boleh dipakai ulang on-disk |
| `Docs/GOLDEN_SCENE_STANDARD.md` | Standar dokumentasi aktif |
| 3 scene Build Settings + `SampleScene` | Akar runtime / akar test |
| 6 prefab `Production/Prefabs/` | Dimuat builder generator (`GoldenSceneProduction`, `CompactKosPolish`) + dikutip scene kos |
| `Assets/Settings/*`, `Production/Presets/*` | GUID-nya jumlah terselesaikan oleh profiles PC_Renderer/KosRenderer/volume |
| `Assets/Resources/**` (16 file) | 3 panggilan `Resources.Load` aktif + root closure |
| `graphify-out` (4 file tracked) | Kontrak AGENTS.md (graph.json, graph.html, GRAPH_REPORT.md, source-manifest.json) |
| `Audit/*.md`, `results.json`, test XML historis | Dokumentasi milestone yang nyata |
| `Audio/Prepared/*`, `Audio/Imported/*` yang berada di closure | GUID-nya dikutip KosAudioProfile.asset; file license tetap disimpan |
| `Art/Atmaja/Textures/<stem>_diff/_nor_gl/_arm_1k.jpg` yang aktif | Dikutip builder sebagai base map / bump / metallic-occlusion |
| Seluruh `Audit/*-invocation.json` | Jejak pipeline reproduktif per milestone |

---

## REFACTOR (bukan sampah; lihat `refactor-candidates.md` untuk rincian)

1. `HudController` — coupling 31 tipe (god-class): risiko rework tinggi, manfaat besar.
2. `KosVisualUpgrade`/`KosVisualV2`/`CompactKosPolish` — 3 stage dressing (±1.350 baris);
   alur jangan diubah, hanya butuh peta supersesi antar stage.
3. Sebaran `Debug.Log` di 24 file — beratnya hanya di build development/debug.
4. Konstanta path aset dibangun berulang di builders (`Art/Atmaja/Textures/…`) → ekstrak konstanta.
5. `deepseek.md`, `Review-Argon.md` — brief historis; arsip catatan desain, bukan sampah.
