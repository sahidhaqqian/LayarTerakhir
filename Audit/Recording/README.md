# Naskah untuk rekaman pengguna

`dialogue-recording.csv` dapat dibuka di spreadsheet. Ada karakter, teks,
pemicu adegan, arahan intonasi, estimasi durasi, dan ID stabil untuk nama file.
`dialogue-recording.json` memuat data yang sama untuk penyambungan audio nanti.
Regenerasi: `python3 Tools/export_dialogue_recording.py`.

Rekam tiap baris terpisah, misalnya `Raka_<id>.wav`, mono PCM 48 kHz,
tanpa musik atau reverb yang dibakar ke file. Sisakan sekitar 0,2 detik napas
di awal/akhir. Estimasi durasi bukan metronom; utamakan pengucapan alami.

- Raka dan Damar harus punya warna suara berbeda, bukan satu suara diganti pitch.
- Ibu terdengar sakit tetapi hangat; Bu Haji Ratna tegas dan manusiawi.
- Damar mengakhiri telepon dengan kepanikan/teriakan sebelum sambungan putus.
- Bu Haji tidak berbicara di adegan surat bawah pintu. Dialognya hanya saat
  pemain benar-benar menelepon kontaknya.
- Kontrak Penonton_Terakhir adalah pesan tertulis, bukan pesan suara.

Tidak ada sintesis suara baru pada revisi ini. Audio placeholder lama tetap
dibedakan dari rekaman final: dialog tambahan memakai subtitle dan menunggu
rekaman pengguna. Jangan melaporkan kualitas VO final sudah terselesaikan.
