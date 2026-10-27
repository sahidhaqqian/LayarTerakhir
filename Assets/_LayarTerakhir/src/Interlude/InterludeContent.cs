namespace LayarTerakhir.Interlude
{
    // Approved Indonesian dialogue and phone copy for the daytime interlude.
    // Kept in one place so the Kos-room hub and the corridor director agree.
    public static class InterludeContent
    {
        public const string HookSender = "NOMOR TAK DIKENAL";
        public const string HookBody =
            "Tidurlah, Raka. Malam ini pukul 23:59, datang ke Rusunawa Terbengkalai Griya Asri. " +
            "Bawa kameramu. Baterai penuh, nyali penuh. Kalau kau berani mematikan siaranmu, " +
            "kepala Damar yang akan tampil di layar kameramu.";

        public const string IbuMessageTitle = "Ibu";
        public const string IbuMessageBody =
            "Nak, transfer deposit rumah sakit sudah masuk. Ibumu stabil. " +
            "Jangan lupa makan. Ibu bangga sama kamu.";

        public static string RecapBody(LayarTerakhir.Core.CampaignData d)
        {
            bool rent = d.inventory.Contains("EVENT_PAID_RENT");
            bool hospital = d.inventory.Contains("EVENT_PAID_HOSPITAL");
            return "MALAM KERAMAT — REKAP SEMALAM\n" +
                "Penonton puncak: 54.820  •  Saweran: Rp25.450.000\n\n" +
                "Kos: " + (rent ? "LUNAS — terima kasih, Bu Haji." : "BELUM DIBAYAR — bisa transfer lewat HP (Bank).") + "\n" +
                "Deposit RS Ibu: " + (hospital ? "MASUK — Ibumu stabil." : "BELUM DIBAYAR — telepon Ibu dulu, lalu bayar lewat HP (Bank).") + "\n\n" +
                "Damar masih hilang. Satu pesan belum dibaca di HP (TAB, lalu Pesan).\n" +
                "Bu Haji menunggumu di halaman. Warung Bu Sri buka seperti biasa.";
        }

        public static string JournalBody(LayarTerakhir.Core.CampaignData d)
        {
            bool anomaly = d.inventory.Contains(InterludeIds.AnomalySeen);
            return "CATATAN SIANG — RAKA\n\n" +
                "Semalam gue selamat dari Rumah Atmaja. Damar belum ketemu.\n" +
                "Kos " + (d.inventory.Contains("EVENT_PAID_RENT") ? "sudah lunas" : "belum lunas") +
                ", deposit RS Ibu " + (d.inventory.Contains("EVENT_PAID_HOSPITAL") ? "sudah masuk" : "belum masuk") + ".\n" +
                "Malam ini: Rusunawa Griya Asri, 23:59. Gue yang milih jalan ini.\n" +
                (anomaly ? "\nSiang ini ada yang aneh. Pantulan... suara itu. Gue catat biar nggak lupa." : "");
        }

        public static string InventoryBody(LayarTerakhir.Core.CampaignData d)
        {
            string text = "BAWAAN RAKA\n\n";
            foreach (string id in d.inventory)
            {
                string label = ItemLabel(id);
                if (label != null) text += "• " + label + "\n";
            }
            return text;
        }

        public static string ItemLabel(string id)
        {
            switch (id)
            {
                case "SMARTPHONE_RAKA": return "Smartphone Raka (+ MataBatin)";
                case "KUNCI_MOTOR": return "Kunci motor";
                case "JAKET_RAKA": return "Jaket";
                case "POWERBANK_KECIL_KOS": return "Powerbank kecil";
                case "DOMPET_RAKA": return "Dompet";
                case "DIARY_RAKA": return "Buku harian";
                case "AIR_MINERAL": return "Air mineral";
                case "BEKAL_MI": return "Bekal mi instan";
                default: return null;
            }
        }
    }
}
