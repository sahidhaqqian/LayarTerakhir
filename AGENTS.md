- Setiap selesai wajib git commit dan push.
- Testing di akhir atau background, prioritaskan syntax check.
- Setiap build ZIP baru wajib upload ke Cloudflare R2:
  rclone copy Builds/LayarTerakhir-Windows-x86_64-Development.zip r2:layarterakhir/Layarterakhir/ -P
- Pastikan upload berhasil sebelum melaporkan selesai.
- Jangan upload ulang jika tidak ada build baru.
