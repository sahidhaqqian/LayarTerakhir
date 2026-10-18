# Sumber aset Layar Terakhir

Folder ini mengumpulkan tiga paket riset dan authoring yang sebelumnya berada
langsung di root repository:

```text
ArtSources/
  AssetResearch/           sumber prop kamar kos, pratinjau dan bukti lisensi
  Scene2_Atmaja_Assets/    koleksi sumber model/material lingkungan Atmaja
  Scene2_Atmaja_Recovery/  sumber Blender, kitbash dan ekspor rumah/motor/kamboja
```

- [Paket prop kos](AssetResearch/README.md)
- [Paket lingkungan Atmaja](Scene2_Atmaja_Assets/README.md)
- [Paket authoring Atmaja](Scene2_Atmaja_Recovery/README.md)

Ketiganya tetap di luar `Assets/` agar Unity tidak mengimpor ulang sumber,
pratinjau, arsip dan model duplikat. Versi yang diintegrasikan ke game berada di
`Assets/_LayarTerakhir/assets/Art/`; kode game tetap di
`Assets/_LayarTerakhir/src/`.

Jalankan perintah pada README tiap paket dari root repository. Tool produksi di
`Tools/` dan skrip paket sudah menggunakan lokasi `ArtSources/`. Identitas paket,
struktur internal, bukti lisensi dan berkas biner sumber dipertahankan. Laporan
historis di `Audit/` tetap mencatat lokasi pada waktu pengujian aslinya.

Folder ini tidak menjadi input Graphify. Hanya source first-party yang diizinkan
di `Assets/_LayarTerakhir/src/` dan `Tools/` yang masuk graph.
