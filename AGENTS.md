- Setiap selesai wajib git commit dan push.
- Testing di akhir atau background, prioritaskan syntax check, validasi ringan, dan tes yang relevan.
- Optimasi iterasi Unity: utamakan syntax check, kompilasi, dan validasi ringan.
- Gunakan `-testFilter` Unity untuk menjalankan hanya tes yang relevan dengan fungsi yang diubah.
- Jalankan targeted test sekali setelah perubahan terkait selesai, bukan setiap edit kecil.
- Hindari regresi EditMode/PlayMode penuh kecuali milestone besar, perubahan sistem kritis bersama, atau diminta eksplisit.
- Tes mahal boleh di background bila tepat, dengan exit code dan hasil tes yang andal.
- Pakai ulang hasil tes yang ada bila kode relevan tidak berubah.
- Jaga cache Unity Library dan hindari restart Editor yang tidak perlu.
- Jangan jalankan lebih dari satu instance Unity Editor pada project yang sama.
- Jika targeted test gagal, investigasi dan perbaiki sebelum commit.
- Windows build TIDAK wajib setiap task; hanya buat jika pengguna memintanya secara eksplisit, dan hanya melalui skill `windows-build-r2` (`.opencode/skills/windows-build-r2/`).
- Upload ke Cloudflare R2 hanya jika pengguna memintanya secara eksplisit, dan hanya melalui skill `windows-build-r2` (upload otomatis hanya setelah build dan validasi ZIP sukses; skill mencegah overwrite rilis).
- Jangan menjalankan build Windows langsung di direktori kerja aktif atau mengunggah manual via `rclone`; snapshot terisolasi + sesi tmux detached wajib agar development bisa lanjut.
- Jika upload diminta, pastikan upload berhasil sebelum melaporkan selesai.
- Jangan upload ulang jika tidak ada build baru.

## Pembagian peran dan pelaksanaan revisi

- Pengguna adalah desainer game dan pemilik keputusan kreatif. Agen bertugas sebagai implementer/developer: ubah game sesuai desain, revisi, dan hasil playtest yang diminta pengguna.
- Jangan mengartikan "mode designer" sebagai permintaan agar agen hanya membuat konsep, merangkum, atau berhenti sebelum implementasi. Pengguna sedang memberi arahan desain; agen mengerjakannya.
- Dalam konteks melanjutkan revisi game, permintaan "baca goal.md" berarti baca versi terbaru lalu terapkan instruksinya, bukan sekadar membalas bahwa file sudah dibaca. Jika pengguna secara eksplisit meminta diskusi, review, atau rencana saja, ikuti batasan tersebut tanpa mengubah game.
- Kerjakan pilihan teknis yang wajar secara mandiri, pertahankan maksud desain pengguna, dan verifikasi perubahan. Jangan meminta persetujuan ulang untuk perubahan yang sudah diminta; klarifikasi hanya jika ada konflik atau keputusan penting yang benar-benar belum ditentukan.
- Folder `Bugs` sudah dihapus (17 Oktober 2026) karena tidak dipakai lagi; gunakan catatan playtest terbaru di `goal.md` sebagai referensi.
- Laporan akhir harus membedakan perubahan yang benar-benar diterapkan dan diverifikasi dari hal yang masih tertunda. Jangan mengaku selesai hanya karena sudah membuat daftar rencana.

## Graphify — wajib setiap sesi sebelum commit dan push

- Graphify CLI yang resmi adalah paket PyPI `graphifyy` (bukan `graphify`); versi proyek dipin di `Tools/graphify-requirements.txt`. Cara instalasi ada di `Tools/GRAPHIFY.md`.
- Setelah perubahan kode terakhir dan sebelum setiap commit/push, jalankan `python3 Tools/update_graphify.py` dari root worktree yang akan di-commit. Perintah ini menjalankan Graphify lokal, memperbarui graph/report/HTML, dan memverifikasi bahwa sumbernya hanya kode yang diizinkan. Jika gagal, perbaiki dulu; jangan melaporkan graph sudah diperbarui.
- Jangan masukkan aset game ke Graphify. `.graphifyignore` memakai allowlist: hanya C# first-party di `Assets/_LayarTerakhir/src` dan Python/shell di `Tools`. Jangan memindai `Assets` secara keseluruhan, menghapus batasan ignore, memakai semantic/LLM extraction, atau mengirim aset ke API.
- Gambar, audio, video, model 3D, tekstur, material, scene, prefab, `.asset`, `.meta`, screenshot/Bugs, hasil build, cache Unity, paket/vendor, dan arsip selalu di luar graph. Kata/nama aset yang disebut kode bukan berarti file aset tersebut boleh diindeks.
- Commit `graphify-out/graph.json`, `graphify-out/GRAPH_REPORT.md`, `graphify-out/graph.html`, dan `graphify-out/source-manifest.json` bersama perubahan kode terkait. Cache, log, path interpreter lokal, dan sidecar mesin tidak perlu di-commit. Graph hanya peta struktur kode, bukan bukti gameplay/build lulus.
- Saat worktree berisi pekerjaan agen lain, gunakan worktree terpisah untuk branch target agar graph dan commit tidak mengambil WIP mereka. Stage hanya file milik tugas ini; jangan `git add .`. Push ke `main` kecuali pengguna meminta branch lain, tanpa force-push.
- Untuk pertanyaan arsitektur gunakan `graphify query "<pertanyaan>"` / `graphify explain "<simbol>"` dari worktree yang graph-nya sudah diperbarui; tetap cek source untuk memastikan detail.

## Graphify — navigasi wajib dan efisiensi token

- Sebelum menelusuri atau mengubah kode, gunakan `graphify query "<topik>" --budget 900` untuk menemukan file dan dependensi relevan. Gunakan `graphify explain "<simbol>"` bila diperlukan.
- Jangan membaca seluruh graph.json atau melakukan pencarian repository luas jika informasi sudah tersedia melalui Graphify. Baca hanya bagian source yang diperlukan.
- Jangan mengulang query, membaca ulang file, atau menjalankan audit umum tanpa kebutuhan jelas. Gunakan hasil pencarian sebelumnya selama masih relevan.
- Graphify hanya mencakup kode yang diizinkan; untuk scene, prefab, material, aset, dan referensi Unity gunakan pemeriksaan terarah pada file aslinya. Jangan rebuild graph jika tidak diperlukan, kecuali pembaruan wajib sebelum commit/push.

## Efisiensi Agent — Graphify, Unity MCP, RTK
- Gunakan Graphify untuk navigasi struktur kode sebelum membaca banyak file C#.
- Gunakan MCP `unity-prefab-parser` untuk inspeksi scene/prefab Unity jika tersedia; jangan dump seluruh YAML kecuali diperlukan.
- Gunakan RTK untuk output terminal: `rtk git status`, `rtk git diff`, `rtk ls`, dan subcommand RTK lain yang didukung.
- Untuk log error, stack trace, dan debugging mendalam, gunakan command asli jika output RTK menyembunyikan informasi penting.
- Jangan menjalankan `rtk gain` pada setiap task; itu hanya statistik penghematan token.
- Hindari membaca seluruh repository, mengulang tool call yang sama, dan memasukkan log panjang ke konteks tanpa alasan.
- Jangan mengganggu pekerjaan agent lain atau melakukan commit terhadap perubahan milik agent lain.
