using LayarTerakhir.Core;
namespace LayarTerakhir.Phase4
{
    public static class Phase4Content
    {
        public const string ForumTitle = "Tragedi Desa Karang Setra — 1997";
        public const string Forum = "Desa Karang Setra adalah kampung di kaki bukit, dikelilingi bambu dan kuburan tua. Pada malam gerhana 1997, Mbah Suro membakar warganya dalam ritual pembukaan Pasar Setan.\n\nIa kemudian meninggalkan jejak di rumah Atmaja (1998), Pabrik Sindang Laut (1999), RSUD (2001), dan Rusunawa (2004). Sosok yang terlihat sekarang adalah arwahnya.\n\nEmpat obor asli mengikat Banaspati. Api biasa tidak melukainya. Tanda dunia yang terlihat dengan UV dapat dipercaya; komentar siaran dapat disusupi. Matikan layar, merunduk, dan putus pandangan di balik perlindungan fisik. Pulihkan diri di Batu Ruwatan. Tidak perlu membayar untuk selamat.";
        public const string Book = "Empat obor penjuru desa adalah nyawa Banaspati peliharaanku. Selama keempatnya menyala, cahaya kamera tak sanggup melukai kami. Hanya mereka yang membawa 3 Tumbal Tanah Kubur dan memadamkan 4 Obor Santet Asli dengan air ruwatan dapat memutus ikatannya. Obor palsu bertanda Rajah Silang Merah akan membalas!\n\n[Catatan Raka]\nTiga pusaka, bukan tiga kantong tanah: Kendi, Belati Perak, dan Kantong Tanah Kubur. Utara, Timur, Barat boleh diselesaikan dengan urutan apa pun. Pancing Banaspati mendekat, periksa UV, siram, lalu satu Blitz saat inti terbuka. Selatan terakhir: siram saja, kemudian kembali ke Pendopo untuk Blitz penutup. Total empat, bukan lima. Air sumur tidak habis.";
        public const string DamarNote = "Ka, gue akhirnya ngerti kenapa Penonton_Terakhir nyuruh lo livestream dari malam pertama. MataBatin.apk yang gue unduh dari deep-web dibuat pengikut Mbah Suro! Ketakutan penonton disedot lewat siaran buat membuka gerbang kelima: Pasar Setan. Gue diseret masuk ke kawah di belakang pendopo desa ini... Tolong hancurin Mbah Suro, Ka!";
        public const string Method = "BANASPATI & ARWAH MBAH SURO\n\nPasang Kendi, Belati, dan Tanah pada tiga dudukan. UV memperlihatkan rajah tengkorak asli atau silang palsu. Pancing Banaspati dekat obor sebelum menyiram. Utara / Timur / Barat: satu siraman asli membuka inti selama 5 detik; satu Blitz mengenai inti. Sektor berhasil tersimpan. Salah bidik tetap membayar 15 daya dan cooldown 6 detik.\n\nSelatan terbuka setelah ketiganya selesai. Siram obor asli, kembali ke Pendopo, bidik inti gabungan untuk Blitz keempat. Saat inti menutup, tunggu jeda dan coba lagi; air dan dinamo tersedia gratis. Flare tidak digunakan di sini.";
        public const string FinalMessage = "Kerja bagus, Raka Wijaya. Empat segel Panca-Sengkala telah lengkap di ponselmu. Satu juta jiwa penonton menyerahkan kesadaran mereka untuk membuka gerbang kami. Tidak ada lagi waktu istirahat di kamar kosmu. Turunlah ke bawah pendopo... Damar dan kontrak Rp500.000.000-mu sedang dilelang di Pasar Setan.";
        public const string Archive = "KEPING CERMIN SAKSI — MATA TERTUTUP\n\nPantulan memperlihatkan warga Karang Setra dikunci di rumah mereka sebelum api 1997 dinyalakan. Mbah Suro memimpin pembakaran itu; ia bukan korban desa. Sesudahnya ia masih terlibat dalam tragedi Atmaja, pabrik, rumah sakit, dan rusun. Kini yang merawat apinya adalah arwahnya. Nama para warga tidak terbaca. Kesaksian ini menyimpan perbuatan mereka yang membakar, bukan mengarang nama mereka yang mati.";
        public static string Label(string id)
        {
            if (id == Phase4Ids.Kendi) return "Kendi Air Sumur Keramat";
            if (id == Phase4Ids.Blade) return "Belati Perak Ruwatan";
            if (id == Phase4Ids.Soil) return "Kantong Tanah Kubur Keramat";
            if (id.StartsWith("P4_POWERBANK")) return "Powerbank besar (+80)";
            if (id.StartsWith("P4_GARAM")) return "Garam Ruwatan (isi Kendi 3/3)";
            if (Phase4Ids.IsIncense(id)) return "Kemenyan Bakar (pulihkan Sanity setelah 2 detik)";
            return LayarTerakhir.Phase3.Phase3Content.ItemLabel(id);
        }
    }
}
