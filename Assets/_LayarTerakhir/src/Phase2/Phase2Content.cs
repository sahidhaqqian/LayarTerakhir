namespace LayarTerakhir.Phase2
{
    public static class Phase2Content
    {
        public static string ItemLabel(string id)
        {
            if(id==LayarTerakhir.Core.Phase2Ids.Gunting)return "Gunting Tali Pusar Pusaka";
            if(id==LayarTerakhir.Core.Phase2Ids.Gelang)return "Gelang Identitas Bayi Lastri";
            if(id==LayarTerakhir.Core.Phase2Ids.Blood)return "Botol Darah Segel Mbah Suro";
            if(id==LayarTerakhir.Core.Phase2Ids.Key)return "Kunci Teralis Lantai 2";
            if(id==LayarTerakhir.Core.Phase2Ids.ArchiveKey)return "Kunci Laci Arsip";
            if(id.StartsWith("P2_POWERBANK")||id.StartsWith("P1_POWERBANK")||id=="POWERBANK_BESAR")return "Powerbank besar (+80%)";
            if(id=="POWERBANK_KECIL_KOS")return "Powerbank kecil (+35%)";
            if(id.StartsWith("P2_KEMENYAN"))return "Kemenyan Bakar (belum dapat dipakai)";
            if(id.StartsWith("P2_FLARE"))return "Flare Merah Darurat (ritual)";
            if(id=="SMARTPHONE_RAKA")return "Smartphone Raka";
            if(id=="JAKET_RAKA")return "Jaket Raka";
            if(id=="MATABATIN_APK")return "MataBatin.apk — AR";
            if(id==LayarTerakhir.Phase1.Phase1Ids.Keris)return "Pecahan Keris Penumbal";
            return null;
        }
        public const string ForumTitle = "Tragedi RSUD Sido Waras — 2001";
        public const string Forum = "RSUD Lama Sido Waras ditutup paksa tahun 2001 setelah ditemukan 17 jasad bayi dan seorang pasien bernama Lastri (26) tewas kehabisan darah di Ruang Bersalin Lantai 2. Dokter kepala saat itu diduga bersekutu dengan dukun Mbah Suro untuk melakukan ritual penumbalan janin (Panca-Sengkala).\n\nPeringatan bagi penjelajah: Arwah Lastri berubah menjadi Kuntilanak Merah. Ia selalu berdiri tepat di belakang punggung korbannya. Kamu tidak akan bisa melihatnya dengan mata biasa atau kamera belakang—gunakan cermin atau kamera depan (Selfie) untuk melihat apakah rambut merahnya sudah menyentuh bahumu.";
        public const string LogTitle = "Buku Jaga Perawat Malam — 11 Oktober 2001";
        public const string Log = "Kode pintu Kamar Mayat selalu diganti mengikuti suhu ruang poliklinik tempat pasien terakhir itu pertama kali diperiksa (tanpa tanda minus), lalu digabung dengan nomor ruang poliklinik yang sama.\n\nCatatan: pasien terakhir malam itu seorang ibu hamil, Lastri, yang diperiksa di poli kandungan. Sejak ia meninggal, ruang poliklinik itu suhunya selalu membeku di bawah nol derajat.";
        public const string AutopsyTitle = "Laporan Otopsi Rahasia — Dr. Hendra";
        public const string Autopsy = "Jasad Lastri disimpan di laci B2, namun janin dan Gunting Tali Pusar Pusaka milik Mbah Suro saya sembunyikan di laci lain yang suhunya paling dingin di ruangan ini.\n\nJangan pernah membuka laci yang lebih hangat dari −20°C, karena laci-laci lainnya sudah dipasangi jebakan gas formalin.\n\nKunci teralis lantai dua terlampir.";
        public const string NurseTitle = "Catatan Suster Jaga — Lantai 2";
        public const string Nurse = "Obat-obatan dan botol dari Mbah Suro saya simpan di brankas, kodenya gampang dihafal: usia pasien terakhir yang melahirkan di sini, lalu nomor inkubator bayinya. Pasien itu Lastri, 26 tahun. Jangan sampai dokter kepala tahu botol itu masih ada.";
        public const string DamarTitle = "Perekam Suara Damar — Bangsal Bersalin";
        public const string Damar = "Ka, kalau lo nemu rekaman ini di lantai dua RS Sido Waras... berarti gue udah diseret lebih jauh lagi. Orang berjubah yang nyulik gue lewat cermin itu nyebut dirinya abdi Penonton Terakhir. Dia bawa gue lewat terowongan bawah tanah menuju bekas Pabrik Gula Sindang Laut. Tolong bertahan, Ka... jangan biarin penonton di live chat lo mati sia-sia.";
        public const string DamarVideoDescription = "Video delapan detik memperlihatkan Damar terikat di dekat tungku pembakaran batu bara raksasa yang mati. Terdengar dengusan napas dan rantai besi diseret di lantai pabrik.";
        public const string FinalMessage = "Dua segel Panca-Sengkala telah kau buka, Raka. Lihat penontonmu—tiga ratus ribu pasang mata kini menatap layarmu tanpa berkedip. Semakin banyak yang menonton, semakin tipis batas antara duniamu dan Pasar Setan. Besok siang gunakan uangmu untuk membeli Lensa UV Rajah dan Flare sebanyak mungkin... karena besok malam pukul 23:59, tungku Pabrik Gula Belanda 'Sindang Laut' akan menyala untuk kalian berdua.";
        public const string EVP = "Kembalikan... anakku... di meja operasi...";
        public const string SecretNote = "Yang dihapus dari daftar tidak membeku bersama jenazah.";
        public const string Method = "ENTITAS TERIDENTIFIKASI: KUNTILANAK MERAH RUANG BERSALIN (LASTRI)\n\nLetakkan 3 Pusaka di Meja Operasi Utama. Pantau serangannya dari belakang lewat KAMERA SELFIE, hancurkan Perisai Kabut Sengkala dengan LAMPU OPERASI / FLARE, lalu segel dengan BLITZ KAMERA.";
    }
}
