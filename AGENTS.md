- Setiap selesai wajib git commit dan push.
- Testing di akhir atau background, prioritaskan syntax check.
- Setiap build ZIP baru wajib upload ke Cloudflare R2:
  rclone copy Builds/LayarTerakhir-Windows-x86_64-Development.zip r2:layarterakhir/Layarterakhir/ -P
- Pastikan upload berhasil sebelum melaporkan selesai.
- Jangan upload ulang jika tidak ada build baru.

## Pembagian peran dan pelaksanaan revisi

- Pengguna adalah desainer game dan pemilik keputusan kreatif. Agen bertugas sebagai implementer/developer: ubah game sesuai desain, revisi, dan hasil playtest yang diminta pengguna.
- Jangan mengartikan "mode designer" sebagai permintaan agar agen hanya membuat konsep, merangkum, atau berhenti sebelum implementasi. Pengguna sedang memberi arahan desain; agen mengerjakannya.
- Dalam konteks melanjutkan revisi game, permintaan "baca goal.md" berarti baca versi terbaru lalu terapkan instruksinya, bukan sekadar membalas bahwa file sudah dibaca. Jika pengguna secara eksplisit meminta diskusi, review, atau rencana saja, ikuti batasan tersebut tanpa mengubah game.
- Kerjakan pilihan teknis yang wajar secara mandiri, pertahankan maksud desain pengguna, dan verifikasi perubahan. Jangan meminta persetujuan ulang untuk perubahan yang sudah diminta; klarifikasi hanya jika ada konflik atau keputusan penting yang benar-benar belum ditentukan.
- Screenshot di folder `Bugs` yang sudah dinyatakan lama oleh pengguna diabaikan. Gunakan catatan playtest terbaru di `goal.md`; pakai screenshot hanya jika pengguna menyatakan bukti tersebut baru/relevan.
- Laporan akhir harus membedakan perubahan yang benar-benar diterapkan dan diverifikasi dari hal yang masih tertunda. Jangan mengaku selesai hanya karena sudah membuat daftar rencana.

## Graphify — wajib setiap sesi sebelum commit dan push

- Graphify CLI yang resmi adalah paket PyPI `graphifyy` (bukan `graphify`); versi proyek dipin di `Tools/graphify-requirements.txt`. Cara instalasi ada di `Tools/GRAPHIFY.md`.
- Setelah perubahan kode terakhir dan sebelum setiap commit/push, jalankan `python3 Tools/update_graphify.py` dari root worktree yang akan di-commit. Perintah ini menjalankan Graphify lokal, memperbarui graph/report/HTML, dan memverifikasi bahwa sumbernya hanya kode yang diizinkan. Jika gagal, perbaiki dulu; jangan melaporkan graph sudah diperbarui.
- Jangan masukkan aset game ke Graphify. `.graphifyignore` memakai allowlist: hanya C# first-party di `Assets/_LayarTerakhir/{Scripts,Editor,Tests}` dan Python/shell di `Tools`. Jangan memindai `Assets` secara keseluruhan, menghapus batasan ignore, memakai semantic/LLM extraction, atau mengirim aset ke API.
- Gambar, audio, video, model 3D, tekstur, material, scene, prefab, `.asset`, `.meta`, screenshot/Bugs, hasil build, cache Unity, paket/vendor, dan arsip selalu di luar graph. Kata/nama aset yang disebut kode bukan berarti file aset tersebut boleh diindeks.
- Commit `graphify-out/graph.json`, `graphify-out/GRAPH_REPORT.md`, `graphify-out/graph.html`, dan `graphify-out/source-manifest.json` bersama perubahan kode terkait. Cache, log, path interpreter lokal, dan sidecar mesin tidak perlu di-commit. Graph hanya peta struktur kode, bukan bukti gameplay/build lulus.
- Saat worktree berisi pekerjaan agen lain, gunakan worktree terpisah untuk branch target agar graph dan commit tidak mengambil WIP mereka. Stage hanya file milik tugas ini; jangan `git add .`. Push ke `main` kecuali pengguna meminta branch lain, tanpa force-push.
- Untuk pertanyaan arsitektur gunakan `graphify query "<pertanyaan>"` / `graphify explain "<simbol>"` dari worktree yang graph-nya sudah diperbarui; tetap cek source untuk memastikan detail.
