using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed partial class Phase5Director
    {
        private GameObject overlay;
        private readonly Queue<string> chatQueue = new Queue<string>();
        private readonly List<string> chatHistory = new List<string>();
        private float nextChat, falseChatSilence, screenDarkTime, nextPressure;
        public void StartLive()
        {
            if (!CanBeginLive || !Phase5Campaign.StartLive(Campaign)) return;
            Phone.isBroadcastConnected = true;
            Chat("kevin_wijaya", "Bang Raka tolong gue gak bisa nutup aplikasi streaming ini! Tombol keluarnya ilang!");
            Chat("penonton_8821", "Jangan matikan siarannya. Kami ingin melihat lelangnya.");
            Chat("Penonton_Terakhir • Rp66.666.666", "Satu juta pasang mata adalah bahan bakar yang sempurna. Damar di Panggung Lelang. Lewati tiga gerbang kenangan.");
            Hud.EnqueueSubtitle("Raka", "Ya Tuhan... cermin-cermin ini nampilin muka penonton gue sendiri. Mereka semua lagi disedot kesadarannya lewat siaran gue...", 10);
        }
        private void Chat(string who, string text) => chatQueue.Enqueue(who + ": " + text);
        private void TickChat(float dt)
        {
            if (!Phone.isScreenOn)
            {
                screenDarkTime += dt;
                if (screenDarkTime >= balance.silenceSeconds) { falseChatSilence = Elapsed + 8; chatQueue.Clear(); }
                return;
            }
            screenDarkTime = 0;
            if (!FinalChoice && Elapsed >= nextPressure && Elapsed >= falseChatSilence && Elapsed >= clarityUntil && Campaign.Data.sanity <= 40)
            { nextPressure = Elapsed + 12; Chat("SISTEM MATABATIN", boss.Phase == 2 ? "[RUTE AMAN] TENGOK KE DEPAN. ABAIKAN SUHU DI LEHER." : "Kami menonton. Kami menunggu. Jangan berhenti merekam."); }
            if (Elapsed < nextChat || chatQueue.Count == 0 || !Phone.isBroadcastConnected || Elapsed < falseChatSilence) return;
            nextChat = Elapsed + 1.2f;
            chatHistory.Add(chatQueue.Dequeue()); if (chatHistory.Count > 40) chatHistory.RemoveAt(0);
            phoneView.ShowChapterChat(string.Join("\n\n", chatHistory.Skip(Mathf.Max(0, chatHistory.Count - 3))));
        }
        private void SectorChat(int sector)
        {
            if (sector == 1) { Chat("penonton_8821", "Pintu tengah panas. Cepat ke kiri!"); Chat("kevin_wijaya", "Gue lihat tulisan di rangkanya. Cek suhu sendiri bang."); }
            if (sector == 2) { Chat("DamarLive", "KIRI KA! SUARA GUE DI KIRI!"); Chat("penonton_8821", "Kiri. Semua ke kiri."); Hud.EnqueueSubtitle("Sumber suara", "[Dua suara dari cabang jembatan: teriakan menyerupai Damar, dan ketukan Morse.]", 8); }
            if (sector == 3) { Hud.EnqueueSubtitle("Ruang Jahit", "[Gunting: SRAAAK... KLING... Langkah berat berbalas dari balik lemari.]", 8); Chat("MATABATIN • SYSTEM", "JANGAN MATIKAN LAYAR. NYALAKAN SENTER DI DEPAN CERMIN."); }
        }
        private void StageDialogue()
        {
            if (!Completed("P5_STAGE_REACHED") && !Receipt("P5_STAGE_REACHED", "STAGE")) return;
            Hud.EnqueueSubtitle("Raka", "Damar! Dam, bangun! Gue di sini!", 5);
            Hud.EnqueueSubtitle("Damar", "Ka... kenapa lo beneran dateng sambil nyalain live?! Gue udah tulis di catatan... jangan bawa penonton lo ke sini!", 9);
            Hud.EnqueueSubtitle("Raka", "Gue nggak punya pilihan. Dia ngancam bakal bunuh lo kalau gue matiin kamera! Gimana cara gue ngelepasin kurungan ini?!", 8);
            Hud.EnqueueSubtitle("Damar", "Di meja altar ada Catatan Asli Mbah Suro. Pindai tiga bukti terakhir buat memaksa wujud aslinya keluar dari jaringan sinyal!", 9);
        }
        private void UpdateGaze(float dt)
        {
            float floor = Zone == "GATE" ? balance.gateFloor : Zone == "STAGE" || Zone == "MIRROR" ? balance.arenaFloor : balance.sectorFloor;
            if (Campaign.Data.sanity <= floor || !Phone.isScreenOn || FinalChoice) return;
            bool viewed = false; var camera = phoneView.playerCamera;
            foreach (var mirror in world.faceMirrors)
            {
                if (!mirror || !mirror.activeInHierarchy) continue;
                var at = mirror.transform.position; var view = camera.WorldToViewportPoint(at);
                if (view.z > 0 && view.z < 15 && view.x > .15f && view.x < .85f && view.y > .1f && view.y < .9f && ClearLine(camera.transform.position, at, mirror.transform)) { viewed = true; break; }
            }
            if (viewed)
            {
                float rate = Zone == "GATE" ? balance.gateGazeDrain : Zone == "STAGE" || Zone == "MIRROR" ? balance.arenaGazeDrain : balance.sectorGazeDrain;
                Campaign.Data.sanity = Mathf.Max(floor, Campaign.Data.sanity - rate * dt);
            }
            phoneView.GlitchStrength = Elapsed < clarityUntil ? 0 : Mathf.InverseLerp(70, 0, Campaign.Data.sanity) * .2f;
        }
        public bool PopulateApp(PhoneApp app, Transform parent)
        {
            string text;
            if (app == PhoneApp.Journal)
            {
                text = "PASAR SETAN — LAYAR TERAKHIR\n\n" + string.Join("\n", Phase5Ids.Evidence.Select((id, i) =>
                    (Campaign.Data.evidence_ids.Contains(id) ? "[✓] " : "[ ] ") + new[] { "Server Batu + Kabel Urat — foto AR", "Kontrak Darah — Thermo dan UV", "Bisikan Penonton — EVP ASLI" }[i])) +
                    "\n\nKontrak: Thermo " + (Completed("P5_CONTRACT_THERMO_OK") ? "✓" : "—") + " • UV " + (Completed("P5_CONTRACT_UV_OK") ? "✓" : "—") +
                    "\n\n" + ArchiveStatus() + "\n\n" + Phase5Content.Method + "\n\nSTATUS ARENA\nInti " + Phase5Ids.CoreCount(Campaign.Data) + "/3 • Kabel " + Phase5Ids.CableCount(Campaign.Data) + "/3";
                if (FinalChoice) text += "\n\n" + Phase5Content.FinalInstructions + (Phase5Ids.ArchiveComplete(Campaign.Data) ? "\n\n" + Phase5Content.SecretInstructions : "");
                UiWidgets.ScrollText("P5Journal", parent, text, new Vector2(.05f, .36f), new Vector2(.95f, .91f), 22);
                var button = PhoneSkin.Button("P5Confirm", parent, Completed("P5_JOURNAL_CONFIRMED") ? "IDENTITAS TERSIMPAN" : "KONFIRMASI IDENTITAS", new Vector2(.05f, .23f), new Vector2(.95f, .34f), () =>
                {
                    if (Phase5Campaign.ConfirmJournal(Campaign)) { RefreshObjective(); phoneView.Apps.Open(PhoneApp.Journal); }
                    else SaveError();
                });
                button.interactable = Phase5Ids.AllEvidence(Campaign.Data) && Phase5Ids.FourSeals(Campaign.Data) && !Completed("P5_JOURNAL_CONFIRMED");
                PhoneSkin.Button("P5Documents", parent, "Dokumen & Arsip Saksi ›", new Vector2(.05f, .09f), new Vector2(.95f, .21f), () => ShowDocuments(parent)); return true;
            }
            if (app == PhoneApp.Map)
                text = "PASAR SETAN\n\nGerbang → Koridor Beku → Jembatan Gilingan Bambu → Ruang Jahit Cermin → Panggung Lelang\n\nKios Arsip Bisu ada di ceruk kanan gerbang. Ruang Cermin Saksi di sisi timur panggung memiliki jalan pulang sebelum arena terkunci.\n\nDinamo dan Altar Doa ada di gerbang, setiap sektor, Ruang Cermin, dan arena. Air di samping gerbang jembatan. Tiga tiang Lentera di arena menggantikan Flare.";
            else if (app == PhoneApp.Inventory)
                text = "BARANG BAWAAN\n\n" + string.Join("\n", Campaign.Data.inventory.Where(id => id.Contains("POWERBANK") || Phase5Ids.IsFlare(id) || Phase5Ids.IsIncense(id) || id == Phase4Ids.Kendi || id == Phase5Ids.Cup).Select(id => "• " + id.Replace('_', ' '))) +
                    "\n\nAIR: " + Campaign.Data.phase4.water_units + "/3\nSaldo sah Rp" + Campaign.Data.wallet_legitimate.ToString("N0") + "\nDana terikat Rp" + Campaign.Data.wallet_bound.ToString("N0") + " (bukan saldo belanja)";
            else if (app == PhoneApp.Messages) text = "RIWAYAT SIARAN\n\n" + string.Join("\n\n", chatHistory) + "\n\n1.000.000 penonton adalah overlay siaran lokal. Donasi Penonton_Terakhir terikat kontrak.";
            else return false;
            UiWidgets.ScrollText("P5App", parent, text, new Vector2(.05f, .1f), new Vector2(.95f, .91f), 22); return true;
        }
        private string ArchiveStatus() => "Arsip Babak 5: " + (Campaign.Data.secret_items.Contains(Phase5Ids.Secret) ? "1/1" : "0/1") + " • Arsip Saksi " + Phase5Ids.ArchiveCount(Campaign.Data) + "/5\n" +
            string.Join(" • ", LayarTerakhir.Phase1.Phase1Ids.Secrets.Select((id, i) => "Bab " + (i + 1) + (Campaign.Data.secret_items.Contains(id) ? " ✓" : " kosong")));
        private void ShowDocuments(Transform parent)
        {
            var card = PhoneSkin.Card("P5Documents", parent, new Vector2(.01f, .08f), new Vector2(.99f, .93f), PhoneSkin.Ink);
            string text = string.Join("\n\n————\n\n", Campaign.Data.documents.Select(d => d.title + "\n\n" + d.body));
            if (Campaign.Data.secret_items.Contains(Phase5Ids.Secret)) text += "\n\n" + Phase5Content.Archive;
            UiWidgets.ScrollText("Documents", card.transform, text, new Vector2(.05f, .15f), new Vector2(.95f, .97f), 22);
            PhoneSkin.Button("Back", card.transform, "‹ Kembali", new Vector2(.05f, .02f), new Vector2(.95f, .13f), () => Destroy(card));
        }
        public void CloseOverlay() { if (overlay) Destroy(overlay); overlay = null; UiFocus.Clock = false; }
        private Transform Panel(string title, string body)
        {
            CloseOverlay(); overlay = UiWidgets.Panel("P5Overlay", Hud.GetComponentInChildren<Canvas>().transform, new Vector2(.18f, .16f), new Vector2(.82f, .84f), new Color(.014f, .018f, .023f, .99f));
            UiWidgets.Text("Title", overlay.transform, title, new Vector2(.06f, .79f), new Vector2(.94f, .96f), 30);
            UiWidgets.ScrollText("Body", overlay.transform, body, new Vector2(.06f, .39f), new Vector2(.94f, .78f), 23);
            UiFocus.Clock = true; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; return overlay.transform;
        }
        private void OpenShop()
        {
            var page = Panel("KIOS TUMBAL — PERBEKALAN OPSIONAL", "Saldo sah Rp" + Campaign.Data.wallet_legitimate.ToString("N0") + "\nDana terikat tidak dapat dipakai. Semua jalur wajib memiliki pengganti gratis.");
            string[] packs = { "POWERBANK", "FLARE", "KEMENYAN", "CHARGE" };
            string[] labels = { "3 Powerbank • Rp750.000", "4 Flare • Rp600.000", "4 Kemenyan • Rp700.000", "Isi daya • Rp100.000" };
            long[] prices = { 750000, 600000, 700000, 100000 };
            for (int i = 0; i < packs.Length; i++)
            {
                string pack = packs[i]; float y = .30f - i * .064f;
                var b = UiWidgets.Button("Buy" + pack, page, labels[i], new Vector2(.06f, y), new Vector2(.94f, y + .055f), () =>
                { if (Phase5Campaign.Buy(Campaign, pack)) { Phone.RestoreBattery(Campaign.Data.battery); OpenShop(); } else SaveError(); });
                b.interactable = Campaign.Data.wallet_legitimate >= prices[i] && !Completed("P5_SHOP_" + pack) && !Completed("P5_ARENA_LOCK");
            }
            UiWidgets.Button("Close", page, "Kembali", new Vector2(.06f, .025f), new Vector2(.94f, .09f), CloseOverlay);
        }
        private void OpenArenaWarning()
        {
            if (Completed("P5_ARENA_LOCK")) return;
            if (!Completed("P5_JOURNAL_CONFIRMED")) { Hud.ShowBroadcastNotification("Tiga bukti, empat segel, dan konfirmasi Jurnal diperlukan sebelum panggung aktif.", 5); return; }
            var page = Panel("PANGGUNG LELANG — ARENA AKAN TERKUNCI", ArchiveStatus() + "\n\nKembali ke Kios Arsip Bisu melalui Cermin Jalan Pulang bila Pita belum diambil. Sesudah ini jalur balik ditutup. Inti boss tersimpan satu per satu.");
            UiWidgets.Button("Back", page, "Kembali menjelajah", new Vector2(.06f, .22f), new Vector2(.94f, .35f), CloseOverlay);
            UiWidgets.Button("Begin", page, "Aktifkan Panggung Lelang", new Vector2(.06f, .055f), new Vector2(.94f, .19f), () =>
            {
                if (!Phase5Campaign.BeginArena(Campaign)) { SaveError(); return; }
                CloseOverlay(); RefreshWorld(); boss.Restore();
                Hud.EnqueueSubtitle("Penonton_Terakhir", "Akulah alasan kanalmu hidup kembali. Akulah satu juta pasang mata yang menolak tidur demi melihat sahabatmu mati di depan kamera!", 11);
                soundscape.Cue("Reveal", world.bossRoot.transform.position);
            });
        }
    }
}
