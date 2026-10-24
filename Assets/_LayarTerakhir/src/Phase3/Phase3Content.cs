using LayarTerakhir.Core;
namespace LayarTerakhir.Phase3
{
    public static class Phase3Content
    {
        public const string ForumTitle="Misteri Mbok Gombel di Pabrik Gula Sindang Laut";
        public const string Forum="Pabrik Gula Sindang Laut dibangun tahun 1912 dan berhenti beroperasi tahun 1999 setelah 9 anak buruh tebu hilang tanpa jejak di malam pesta giling. Warga percaya anak-anak disembunyikan Wewe Gombel — Ibu Asuh Semu — di rangka besi atap ketel.\n\nJangan langsung berlari ke suara tangisan atau teman yang meminta tolong. Periksa sumber memakai EVP. Gunakan UV Rajah untuk melihat silang pada lantai rapuh. Suara ASLI bukan jaminan pijakan aman.\n\nRumah Atmaja adalah asal petunjuk. Rusunawa, RSUD dan Pabrik adalah tiga gerbang pertama Panca-Sengkala; dua ujung masih kosong.";
        public const string Mandor="Mbah Suro meminta tiga tumbal anak buruh untuk ditanam di bawah tungku uap agar gilingan tahun ini menghasilkan uang berlipat. Tapi sejak ritual malam Jumat Kliwon itu, sosok perempuan raksasa berambut putih yang dipanggil Mbok Gombel turun dari atap pabrik. Dia menyembunyikan anak-anak itu di dalam kepompong kain goni dan menipu siapa pun yang mencarinya dengan suara tangisan palsu. Satu-satunya cara membakar sarangnya adalah menyalakan Generator Lampu Sorot Tungku dan membaca jalur dengan Lensa UV Rajah, lalu memancingnya ke cahaya lampu sorot.";
        public const string Damar="Ka, video gue di tungku pabrik ini direkam kemarin siang! Sekarang Mbah Suro bawa gue keluar pabrik menuju Desa Mati Karang Setra. Hati-hati sama penonton di Live Chat lo, Ka... mereka bukan manusia semua!";
        public const string Method="WEWE GOMBEL — IBU ASUH SEMU\nTiga pusaka di dudukan; periksa karung. Baca lantai lewat UV. Pancing Wewe ke generator N/S/E, nyalakan, lalu bidik dada/wajah dan Blitz dalam 5 detik. Lampu korslet hanya sesudah hit berhasil. Flare opsional. Pada tarikan kain, sorot simpul dengan UV satu detik atau putus dengan sorotan generator yang masih utuh. Daya dipulihkan manual saat jeda; reset jatah di alcove servis bila habis.";
        public const string FinalMessage="Tiga segel lokasi telah terkumpul, Raka. Kau sadar sekarang? Enam ratus ribu penonton di layarmu sedang membuka pintu dari sisi mereka, dan sebagian dari penghuni Pasar Setan sudah mulai ikut mengetik di kolom komentar siaranmu. Besok malam adalah malam penyegelan keempat. Datanglah ke tempat kelahiran Mbah Suro di Desa Mati Karang Setra. Di sana, sahabatmu sedang menunggu di tengah lingkaran api Banaspati.";
        public const string Archive="PELAT NOMOR BURUH 09 — 1999\nSembilan slot kesaksian anak buruh: tiga korban ritual awal, enam korban berikutnya. Sembilan kolom nama tergerus karat, hanya nomor buruh yang tersisa. Mereka bukan angka hasil gilingan.";
        public static string ItemLabel(string id)
        {
            if(id==Phase3Ids.Doll)return "Boneka Jerami Tebu";
            if(id==Phase3Ids.Sickle)return "Sabit Tebu Berukir Rajah";
            if(id==Phase3Ids.Cloth)return "Selendang Batik Terbakar";
            if(id==Phase3Ids.Shoe)return "Sepatu kiri Damar";
            if(id.StartsWith("P3_POWERBANK_"))return "Powerbank besar (+80)";
            if(Phase3Ids.IsFlare(id))return "Flare Industri";
            if(id.StartsWith("P3_KEMENYAN"))return "Kemenyan Bakar — disimpan untuk Babak 4";
            return LayarTerakhir.Phase2.Phase2Content.ItemLabel(id);
        }
    }
}
