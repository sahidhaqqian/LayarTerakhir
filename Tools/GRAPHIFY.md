# Graphify: peta kode, bukan aset game

Paket resmi: [`graphifyy`](https://github.com/Graphify-Labs/graphify), versi dipin
di `graphify-requirements.txt`. CLI-nya bernama `graphify`. C#, Python, dan shell
diparse lokal menggunakan AST; tidak perlu API key atau upload aset.

## Instalasi terisolasi (Linux, Python 3.10+)

Dari root repo:

```bash
python3 -m venv /root/.local/share/layarterakhir-graphify
/root/.local/share/layarterakhir-graphify/bin/python -m pip install -r Tools/graphify-requirements.txt
```

Pastikan direktori bin venv ada di PATH, atau buat symlink `graphify` di
`/root/.local/bin` **hanya jika path tersebut belum ada**. Instalasi sesi ini
menggunakan symlink itu. Alternatif untuk mesin lain:
`uv tool install graphifyy==0.9.92`, lalu tambahkan tool bin ke PATH.

## Setiap sesi, sebelum commit dan push

```bash
python3 Tools/update_graphify.py
python3 Tools/update_graphify.py --check
```

Perintah pertama menjalankan `graphify extract . --code-only --max-workers 2`,
lalu `graphify cluster-only . --no-label`. Ekstraksi incremental memperbarui
kode yang berubah dan menghapus sumber yang tidak lagi ada; pelabelan komunitas
tidak menggunakan LLM. Jangan mengganti dengan ekstraksi semantic.

Allowlist `.graphifyignore` hanya menerima `.cs` di direktori first-party
`Assets/_LayarTerakhir/src` serta `.py`/`.sh` di `Tools`.
Seluruh direktori lain ditolak secara default. Media, scene/prefab, `.meta`,
material, asset Unity, screenshot, Bugs, paket/vendor dan build tidak termasuk.
Source memang berada di bawah `Assets/`, tetapi file aset gamenya tidak dibaca.
Pipeline menolak graph non-kode, referensi file di luar allowlist, hash AST
yang gagal/stale, dan daftar input yang tidak sesuai. Nama/path aset yang muncul
sebagai string dalam source bukan file aset yang diindeks.

Stage hanya perubahan tugas sendiri dan empat hasil portabel berikut:

```bash
git add -- graphify-out/graph.json graphify-out/GRAPH_REPORT.md graphify-out/graph.html graphify-out/source-manifest.json
```

`--check` membandingkan SHA-256 seluruh kode, aturan ignore dan tiga hasil
Graphify, sehingga perubahan sesudah update harus diikuti update ulang.
File lokal/caches di `graphify-out` di-ignore. Jangan memasang hook bawaan
post-commit otomatis: kita harus memperbarui dan stage graph **sebelum** commit,
serta tidak memindai WIP agen lain. Gunakan worktree terpisah jika diperlukan.
`built_at_commit` dari Graphify menunjuk HEAD sebelum regenerasi; hash sumber
di `source-manifest.json` menjelaskan isi kode yang sebenarnya dianalisis.

## Menggunakan graph

```bash
graphify query "FirstPersonController" --budget 1000
graphify explain "KamarKosPrologDirector"
```

Buka `graphify-out/graph.html` di browser untuk visualisasi. Graph JSON/report
bisa digunakan offline; visualisasi HTML bawaan Graphify memakai library CDN.
Peta ini membantu navigasi struktur kode, bukan pengganti tes Unity/gameplay.

## Tes tooling (tanpa menjalankan Unity)

```bash
python3 -m py_compile Tools/update_graphify.py Tools/tests/test_update_graphify.py
python3 -m unittest discover -s Tools/tests -v
```

Untuk ikut menjalankan dua tes integrasi discovery/ignore dengan engine
Graphify yang benar-benar terpasang (termasuk 80 kombinasi jebakan ekstensi
aset di dalam direktori kode), gunakan interpreter venv:

```bash
/root/.local/share/layarterakhir-graphify/bin/python -m unittest discover -s Tools/tests -v
```

Python sistem tanpa paket Graphify tetap menjalankan tes unit dan melewati
dua tes integrasi tersebut dengan keterangan eksplisit.
