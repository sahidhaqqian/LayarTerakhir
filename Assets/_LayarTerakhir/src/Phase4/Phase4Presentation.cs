using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed partial class Phase4Director
    {
        private GameObject overlay;
        private readonly List<string> chatHistory = new List<string>();
        private readonly Queue<string> chatQueue = new Queue<string>();
        private float nextChat;
        private float nextFearChat;
        public void StartLive()
        {
            if (!CanBeginLive || !Receipt("P4_START_LIVE", SafeAnchor, null, d =>
            {
                d.wallet_bound += 6666000;
                d.phase4.money_ledger.Add(new Phase1MoneyReceipt { id = "P4_DONATION_ARRIVAL", amount = 6666000, bound = true });
            })) return;
            Phone.isBroadcastConnected = true;
            Chat("rina_cirebon", "Bambu kanan bergerak sendiri. Bang, periksa tanda di tanah dulu.");
            Chat("Penonton_Terakhir • Rp6.666.000", "Selamat datang di kampung halaman Mbah Suro, Raka. Damar sudah menunggumu di dalam lingkaran api.");
            Hud.EnqueueSubtitle("Raka", "Gue di Karang Setra. Kalau ada yang ngetik pakai nama Damar, jangan langsung percaya. Gue periksa sendiri.", 9);
        }
        private void Chat(string who, string text) => chatQueue.Enqueue(who + ": " + text);
        private void TickChat()
        {
            if (Elapsed >= nextFearChat && Campaign.Data.sanity < 40 && Completed("P4_BANASPATI_REVEAL") && !Completed("P4_BOSS_WIN") && !ClarityActive)
            { nextFearChat = Elapsed + 12; Chat("Damar_1997", "Jangan baca tanahnya, Ka. Liat api yang paling terang. Gue di sana."); }
            if (chatQueue.Count == 0 || Elapsed < nextChat || !Phone.isBroadcastConnected) return;
            nextChat = Elapsed + 1.2f; chatHistory.Add(chatQueue.Dequeue());
            if (chatHistory.Count > 40) chatHistory.RemoveAt(0);
            phoneView.ShowChapterChat(string.Join("\n\n", chatHistory.Skip(Mathf.Max(0, chatHistory.Count - 3))));
        }
        private void QueueBranchChat(int branch)
        {
            if (branch == 1) { Chat("warga_karang97", "Belok kiri ke nisan. Jangan nyalain UV, nanti ketahuan."); Chat("rina_cirebon", "Ada jejak di tanah dekat beringin kanan. Cek sendiri bang."); }
            if (branch == 2) { Chat("rina_cireb0n", "Lurus aja. Tanah merah aman kok."); Chat("rina_cirebon", "Itu bukan akun gue. Tapi jangan percaya nama gue doang, cek tanahnya."); }
            if (branch == 3) { Chat("MATABATIN • SYSTEM", "RUTE AMAN DIPINDAH. LARI KE SUARA DAMAR."); Chat("warga_karang97", "Jejaknya masih di tanah, Ka. Sistem juga bisa bohong."); }
        }
        public bool ConfirmJournal()
        {
            bool ok = Phase4Campaign.ConfirmJournal(Campaign, SafeAnchor);
            if (ok) { Hud.ShowBroadcastNotification("IDENTITAS & METODE RUWATAN TERSIMPAN", 5); RefreshObjective(); }
            else SaveError(); return ok;
        }
        public bool PopulateApp(PhoneApp app, Transform parent)
        {
            string text;
            if (app == PhoneApp.Journal)
            {
                text = "KARANG SETRA — API DI EMPAT PENJURU\n\n" + string.Join("\n", Phase4Ids.Evidence.Select((id, i) =>
                    (Campaign.Data.evidence_ids.Contains(id) ? "[✓] " : "[ ] ") + new[] { "Bara makam +66°C — Thermo", "Aura Altar — foto AR", "Mantra Empat Penjuru — EVP 6 detik" }[i])) +
                    "\n\n" + string.Join("\n", Phase4Ids.Relics.Select(id => (Campaign.Data.relic_ids.Contains(id) ? "[✓] " : "[ ] ") + Phase4Content.Label(id))) +
                    "\n\n" + ArchiveStatus() + "\n\n" + (Completed("P4_JOURNAL_CONFIRMED") ? Phase4Content.Method : "Cocokkan ketiga bukti, lalu konfirmasi identitas.");
                UiWidgets.ScrollText("P4Journal", parent, text, new Vector2(.05f, .36f), new Vector2(.95f, .91f), 22);
                var b = PhoneSkin.Button("P4Confirm", parent, Completed("P4_JOURNAL_CONFIRMED") ? "IDENTITAS TERSIMPAN" : "KONFIRMASI IDENTITAS & RITUAL", new Vector2(.05f, .23f), new Vector2(.95f, .34f), () => { if (ConfirmJournal()) phoneView.Apps.Open(PhoneApp.Journal); });
                b.interactable = Phase4Ids.AllEvidence(Campaign.Data) && !Completed("P4_JOURNAL_CONFIRMED");
                PhoneSkin.Button("Documents", parent, "Dokumen & Arsip Saksi ›", new Vector2(.05f, .09f), new Vector2(.95f, .21f), () => ShowDocuments(parent)); return true;
            }
            if (app == PhoneApp.Map)
                text = "DESA MATI KARANG SETRA\n\nMotor → tiga simpang hutan → Pendopo\nUTARA: makam panas / Kendi\nTIMUR: gubuk panggung / altar / Belati\nBARAT: sumur / mantra / Tanah / air\nSELATAN: kebun bambu / obor terakhir\n\nJalur mengelilingi Pendopo menghubungkan semua sektor. Dinamo dan Batu Ruwatan ada di pintu hutan, Pendopo, serta tempat berlindung tiap sektor. Saluran air di tiap sektor terhubung ke sumur; isi ulang tidak terbatas.\n\nTanda UV dunia dapat dipercaya. Arsip relung Barat opsional; tidak mengunci gerbang.";
            else if (app == PhoneApp.Inventory)
            {
                text = "BARANG BAWAAN\n\n" + string.Join("\n", Campaign.Data.inventory.Select(Phase4Content.Label).Where(x => x != null).Select(x => "• " + x)) +
                    "\n\nAIR KENDI: " + State.water_units + "/3\nKemenyan: "+GameInputConfig.Instance.GetActionLabel(GameAction.UseIncense)+" • proses 2 detik, jeda 12 detik. Dinamo dan Batu Ruwatan gratis.";
                string salt = Campaign.Data.inventory.FirstOrDefault(id => id.StartsWith("P4_GARAM_"));
                if (salt != null) PhoneSkin.Button("Salt", parent, "Gunakan Garam Ruwatan", new Vector2(.05f, .06f), new Vector2(.95f, .18f), () => { if (Phase4Campaign.Salt(Campaign, SafeAnchor)) phoneView.Apps.Open(app); else SaveError(); });
            }
            else if (app == PhoneApp.Messages)
                text = "RIWAYAT SIARAN\n\n" + string.Join("\n\n", chatHistory) + (Completed("P4_START_LIVE") ? "\n\nDonasi Rp6.666.000 adalah dana terikat." : "") + (Completed("P4_FIFTH_GATE_REVEAL") ? "\n\n" + Phase4Content.FinalMessage : "");
            else return false;
            UiWidgets.ScrollText("P4App", parent, text, new Vector2(.05f, app == PhoneApp.Inventory ? .21f : .1f), new Vector2(.95f, .91f), 22); return true;
        }
        private string ArchiveStatus() => "Arsip Babak 4: " + (Campaign.Data.secret_items.Contains(Phase4Ids.Secret) ? "1/1" : "0/1") + " • Arsip Saksi " + Phase1Ids.Secrets.Count(Campaign.Data.secret_items.Contains) + "/5";
        private void ShowDocuments(Transform parent)
        {
            var card = PhoneSkin.Card("P4Documents", parent, new Vector2(.01f, .08f), new Vector2(.99f, .93f), PhoneSkin.Ink);
            string text = string.Join("\n\n————\n\n", Campaign.Data.documents.Select(x => x.title + "\n\n" + x.body));
            if (Campaign.Data.secret_items.Contains(Phase4Ids.Secret)) text += "\n\n" + Phase4Content.Archive;
            UiWidgets.ScrollText("Documents", card.transform, text, new Vector2(.05f, .15f), new Vector2(.95f, .97f), 22);
            PhoneSkin.Button("Back", card.transform, "‹ Kembali", new Vector2(.05f, .02f), new Vector2(.95f, .13f), () => Destroy(card));
        }
        private IEnumerator Epilogue()
        {
            StoryPlaying = true; entity.Suspend();
            if (!Receipt("P4_FIFTH_GATE_REVEAL", "PORTAL")) { StoryPlaying = false; yield break; }
            soundscape.Cue("Gate", world.Anchor("PORTAL").position);
            Hud.EnqueueSubtitle("Raka", "Empat segel... kenapa jumlah penonton masih naik? Damar! Lo di bawah sana?", 8);
            Chat("SISTEM", "999.999 PENONTON • SALDO TERDETEKSI Rp999.999.999 [SINYAL RUSAK]");
            Chat("Penonton_Terakhir", Phase4Content.FinalMessage);
            float t = 0; while (t < 7) { if (ClockRunning) t += Time.deltaTime; yield return null; }
            Hud.EnqueueSubtitle("Damar", "[Suara jauh dari tangga, tertutup riuh pasar: Ka... jangan percaya kontraknya!]", 7);
            StoryPlaying = false; RefreshObjective();
        }
        private void RemoveOverlay() { if (overlay) Destroy(overlay); overlay = null; UiFocus.Clock = false; }
        public void CloseOverlay() { RemoveOverlay(); }
        public void OpenExitPanel()
        {
            if(StoryPlaying){Hud.ShowBroadcastNotification("Dengarkan pesan dari bawah Pendopo terlebih dahulu.",3);return;}
            if (!Completed("P4_FIFTH_GATE_REVEAL")) { Hud.ShowBroadcastNotification("Periksa pesan dan gerbang terlebih dahulu.", 3); return; }
            RemoveOverlay(); overlay = UiWidgets.Panel("P4Exit", Hud.GetComponentInChildren<Canvas>().transform, new Vector2(.16f, .2f), new Vector2(.84f, .8f), new Color(.014f, .022f, .025f, .99f));
            UiWidgets.Text("Title", overlay.transform, "GERBANG KELIMA — PASAR SETAN", new Vector2(.06f, .76f), new Vector2(.94f, .96f), 30);
            UiWidgets.Text("Status", overlay.transform, ArchiveStatus() + "\n\nEmpat segel tersimpan. Setelah turun, perjalanan berlanjut pada malam yang sama. Periksa desa bila masih ada yang tertinggal.", new Vector2(.06f, .36f), new Vector2(.94f, .73f), 24);
            UiWidgets.Button("Back", overlay.transform, "Kembali menjelajah", new Vector2(.06f, .19f), new Vector2(.94f, .31f), CloseOverlay);
            UiWidgets.Button("Descend", overlay.transform, "Turun ke gerbang Pasar Setan", new Vector2(.06f, .03f), new Vector2(.94f, .16f), () => ConfirmExit());
            UiFocus.Clock = true; Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
        public bool ConfirmExit()
        {
            if (!overlay || overlay.name != "P4Exit" || !Phase4Campaign.Finish(Campaign)) { SaveError(); return false; }
            RemoveOverlay(); soundscape.Cue("Shutter", player.transform.position);
            Hud.SetObjective("Turun melalui tangga batu. Gerbang Pasar Setan menunggu di bawah Pendopo."); return true;
        }
        private void ShowBoundary()
        {
            Hud.SetFadeAlpha(0); Hud.SetObjective("BERSAMBUNG — PASAR SETAN • Empat segel tersimpan. Damar berada di balik gerbang.");
            // A physical, persisted vestibule: no nonexistent Phase 5 scene or Kos return.
            player.MovementLocked = false;
        }
    }
}
