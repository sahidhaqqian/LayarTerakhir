# Refactor Candidates — Layar Terakhir (17 Oktober 2026)

Audit kualitas kode first-party (71 file C#, 12.017 baris, 92 tipe pada
`Assets/_LayarTerakhir/{Scripts,Editor}`). **Audit-only: tidak ada perubahan kode.** Semua
item diberi `risiko` vs `manfaat`. Tidak ada tipe first-party yang terbukti mati; daftar ini
adalah utang teknis, bukan sampah.

## Metodologi

- Ekstraksi tipe (`class/struct/interface/enum`) + cross-file reference search per nama tipe.
- Sensitivitas false-positive: entry-point reflection (`-executeMethod`, NUnit, `RuntimeInitializeOnLoadMethod`) dikonfirmasi via data `Audit/*-invocation.json` + receipt manifest — kelas-kelas tsb match usage: contoh `WindowsDevelopmentBuild.Build` dipanggil 3× invocation, `AtmajaSceneValidation.Run` dsb.
- Duplikasi nama metode antar file: lifecycle (Update/Start/Awake) memang muncul per-MonoBehaviour; `Build/Run/Apply/Install` antar Editor tool adalah idiom entry-point per tool, bukan duplikasi implementasi.
- `TempShaftProbe` tidak muncul di invocation JSON aktif, tetapi outputnya tercatat (`Audit/designer-corridor-probe-before-fix.txt`) — keputusan keep/lookup di pertimbangkan ke pemilik di cleanup-candidates.md.

## Tabel utama

| # | Perbaikan | Bukti | Risiko rework | Manfaat | Prioritas |
| --- | --- | --- | --- | --- | --- |
| 1 | `HudController` god-class | 31 kelas merujuknya; menyatukan subsistem subtitle, save/recharge, notification, dialog priority | tinggi | besar | sedang (lakukan saat stable, tidak masa prolog) |
| 2 | `KosVisualUpgrade` (710 baris) + `KosVisualV2` (442) + `CompactKosPolish` | 3 tool dressing efek berurutan berbagi pola material/load; perlu peta supersesi | sedang | menengah | rendah (dokumen saja siapa menggantikan siapa) |
| 3 | Duplikat string path builder | `AtmajaUpperBuilder` membangun `Art/Atmaja/Textures/<stem>_diff_1k.jpg` dsb. tiap pemanggilan; `AtmajaSceneBuilder` juga | kecil | kecil | rendah |
| 4 | `Debug.Log` tersebar | 87 hit di 24 file (termasuk beberapa untuk debugging pemain) | kecil | kecil | rendah |
| 5 | Parsing argumen audit diulang | `DisplayPreferences` dan `GameShell` masing-masing meng-parse command line (`EndsWith("Audit")`, `-batchmode`) dengan pola sendiri | kecil | sedang | rendah |
| 6 | Tata direktori | `Scripts/` berisi Prolog, UI, Audio, Player, Smartphone, Core, Diagnostics — sudah modular; tidak ada boundary yang buruk | — | — | tidak perlu sekarang |
| 7 | `deepseek.md`/`Phase*.MD` status legacy | menyimpan progres milestone yang telah selesai | tidak terutama | dokumentasi owner | keep |

## Dead code analysis (hasil)

Tipe tanpa referensi lintas file (28) — **semua hidup dengan bukti**:
- 13 test classes (`LayarTerakhir.Editor.Tests.*`) — dijalankan Test Runner (67/67 lulus).
- 6 entry-point tools (`GoldenLinuxBuild`, `GoldenSceneProduction`, `WindowsDevelopmentBuild`, `PrologMilestoneValidator`, `PlaytestReviewValidation`, `KosVisualCapture`, `KosVisualUpgrade`) — dipanggil via `-executeMethod` di `Audit/*.json`.
- 3 diagnostics self-install (`AtmajaPlayerAudit`, `GoldenPlayerAudit`, `ReviewPlayerAudit`) + `DevelopmentScreenshotCapture` — `RuntimeInitializeOnLoadMethod` + command-line flag yang sekarang dipakai pipeline audit hari ini (`review-player-invocation.json` dll.).
- 6 nested types lokal (`EndingType`, `Zone`, `PhoneGlyph`, `BuildRecord`, `BuildFileRecord`, `VisualAudit`) — dipakai same-file.

**Kesimpulan**: tidak ada first-party dead code. Ketiga diagnostics audit sudah berada di
balik `#if UNITY_EDITOR || DEVELOPMENT_BUILD` sehingga release build standar tidak membawanya
— keputusan membuangnya sepenuhnya tersisa ke pemilik (efeknya: pipeline audit perlu
menginstal ulang mekanisme serupa kalau dibutuhkan lagi).

## Duplikasi implementasi (hasil)

- `KosCorridorBuilder` + `KosCorridorBuilding` = **satu partial class** (bukan duplikasi).
- `Update/Start/Awake/LateUpdate/OnDestroy` lulus karena per-MonoBehaviour; tidak dianggap duplikat.
- `Build/Run/Apply/Install` berulang antar Editor tools = masing-masing entry-point; tidak terdeteksi duplikasi body. Resembles: `Textured()` di di `AtmajaUpperBuilder` vs `AtmajaSceneBuilder` mengandung beberapa teks sama (~6-10 baris) — terpetakan pada tabel #3 (duplikat konstanta path).

## Harmonisasi yang disarankan (jika owner memilih refactor)

1. Ekstrak konstanta `AssetPaths` (path art/material) antar builder → menghindari typo string yang membuat generator gagal saat dijalankan ulang.
2. `HudController`: pecah menjadi `HudSubtitles`, `HudNotifications`, `HudPhoneFeedController` — hanya bila waktu regression test cukup (67/67 tests saat ini berbasis EditMode + Play audit; split besar perlu test baru).
3. Masukkan `Debug.Log` duplikat ke `#[Conditional("LAYAR_DEBUG")]`; logging pada audit diagnostics tetap seperti sekarang.
