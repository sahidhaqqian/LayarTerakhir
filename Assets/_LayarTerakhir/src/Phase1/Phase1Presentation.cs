using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    public sealed partial class Phase1Director
    {
        private GameObject overlayPage;
        private bool charging;
        public void Chat(string author, string line)
        {
            chatHistory.Add(author + ": " + line);
            if (chatHistory.Count > 80) chatHistory.RemoveAt(0);
            if (phoneView) phoneView.ShowChapterChat(string.Join("\n\n", chatHistory.Skip(Mathf.Max(0, chatHistory.Count - 3))));
        }
        public bool PopulateApp(PhoneApp app, Transform page)
        {
            if (app == PhoneApp.Journal)
            {
                var data = Campaign.Data;
                string Check(string id, string text) => (data.evidence_ids.Contains(id) ? "[✓] " : "[ ] ") + text;
                string status = "JURNAL KLENIK v2.0\nPENUNGGU LANTAI EMPAT\n\n" +
                    Check(Phase1Ids.Claw, "Jejak fisik — Kamar 402") + "\n" + Check(Phase1Ids.EVP, "Suara gaib — kamar mandi") + "\n" + Check(Phase1Ids.Aura, "Aura kematian — tungku") + "\n\n" +
                    (Phase1Ids.AllEvidence(data) ? "GENDERUWO ARANG\nHalogen memecah perisai. Saat tubuh terekspos, bidik kepala dengan Blitz.\n\n" : "Tiga bukti berbeda diperlukan untuk identifikasi.\n\n") +
                    (Completed("P1_JOURNAL_CONFIRMED") ? "Identitas dikonfirmasi.\n" : "Identitas belum dikonfirmasi.\n") +
                    "\nARSIP SAKSI\nBabak ini: " + (data.secret_items.Contains(Phase1Ids.Secret) ? "1/1 — Negatif Tari" : "0/1 — Periksa arsip di L2") +
                    "\nCampaign: " + Phase1Ids.Secrets.Count(data.secret_items.Contains) + "/5";
                UiWidgets.ScrollText("P1Deduction", page, status, new Vector2(.06f, .42f), new Vector2(.94f, .89f), 23);
                var confirm = PhoneSkin.Button("P1ConfirmIdentity", page, Completed("P1_JOURNAL_CONFIRMED") ? "IDENTITAS TERSIMPAN" : "Konfirmasi identitas", new Vector2(.06f, .31f), new Vector2(.94f, .40f), () =>
                { if (Phase1Ids.AllEvidence(Campaign.Data) && Commit("P1_JOURNAL_CONFIRMED")) phoneView.Apps.Open(PhoneApp.Journal); });
                confirm.interactable = Phase1Ids.AllEvidence(data) && !Completed("P1_JOURNAL_CONFIRMED") && !BossActive;
                PhoneSkin.Button("P1Documents", page, "Dokumen dan catatan  ›", new Vector2(.06f, .21f), new Vector2(.94f, .29f), () => ShowDocuments(page));
                var photoButton=PhoneSkin.Button("P1ArchivePhoto",page,"Arsip Saksi — baca kesaksian",new Vector2(.06f,.11f),new Vector2(.94f,.19f),()=>ShowDocuments(page));
                photoButton.interactable=data.secret_items.Contains(Phase1Ids.Secret);
                return true;
            }
            if (app == PhoneApp.Messages)
            {
                UiWidgets.ScrollText("P1ChatHistory", page, "SIARAN LOKAL • RIWAYAT\n\n" + string.Join("\n\n", chatHistory) +
                    (Completed("P1_EPILOGUE") ? "\n\nPENONTON_TERAKHIR\n" + Phase1Content.Epilogue : ""), new Vector2(.06f, .11f), new Vector2(.94f, .88f), 23); return true;
            }
            if (app == PhoneApp.Map)
            {
                UiWidgets.ScrollText("P1FloorPlan", page, "GRIYA ASRI • DENAH SERVIS\n\nL1  Parkir — pos — pompa — K102\nL2  Kotak surat — arsip — lift barang\nL3  Koridor timur — tangga darurat\nL4  401 / 402 / 403 / 404\n       Kamar mandi — dapur umum\n\nEmpat sisi koridor mengelilingi void.\nGerbang lobi dilas mati.\nLift melayani L1, L2, L3.\n\n" + (Completed("P1_BOSS_WIN") ? "Lift pulih: pilih L1 untuk parkir atau L2 untuk arsip." : "Rem pengaman lift terpengaruh anomali."), new Vector2(.06f, .12f), new Vector2(.94f, .88f), 24); return true;
            }
            if (app == PhoneApp.Inventory)
            {
                string contents = "BARANG BAWAAN\n\n";
                foreach(var pair in new[] { (Phase1Ids.Key, "Kunci lift barang"), (Phase1Ids.Powerbank, "Powerbank servis (+80%)"), (Phase1Ids.Keris, "Pecahan Keris Penumbal"),
                    ("POWERBANK_KECIL_KOS", "Powerbank kecil (+35%)"), ("POWERBANK_BESAR", "Powerbank besar (+80%)"), ("P1_BOLT_CUTTER", "Pemotong besi"), ("P1_DAMAR_CLOTH", "Kain jaket Damar") })
                    if (Campaign.HasInventoryItem(pair.Item1)) contents += "• " + pair.Item2 + "\n\n";
                contents += "Handycam Damar terikat pada jok motor.\n";
                UiWidgets.ScrollText("P1Inventory", page, contents, new Vector2(.06f, .12f), new Vector2(.94f, .88f), 24); return true;
            }
            // Existing Bank remains available, with the campaign's shared write barrier.
            return false;
        }
        private void ShowDocuments(Transform parent)
        {
            var sheet = PhoneSkin.Card("P1Documents", parent, new Vector2(.02f, .10f), new Vector2(.98f, .90f), PhoneSkin.Ink);
            string text = "DOKUMEN TERSIMPAN\n\n";
            foreach(var document in Campaign.Data.documents) text += document.title + "\n" + document.body + "\n\n————\n\n";
            UiWidgets.ScrollText("Text", sheet.transform, text, new Vector2(.06f, .14f), new Vector2(.94f, .97f), 22);
            PhoneSkin.Button("Back", sheet.transform, "‹  Kembali", new Vector2(.06f, .02f), new Vector2(.94f, .12f), () => Destroy(sheet));
        }
        private void ToggleHide(Phase1WorldItem item)
        {
            if (Hidden)
            {
                Hidden = false; player.MovementLocked = false;
                var capsule = player.GetComponent<CharacterController>(); capsule.enabled = false;
                player.transform.SetPositionAndRotation(beforeHiding, beforeHidingRotation); capsule.enabled = true;
                player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = false;
                player.GetComponent<ShoulderCameraRig>().RefreshCameraPose(); hidingItem = null;
            }
            else
            {
                Hidden = true; hidingItem = item; beforeHiding = player.transform.position; beforeHidingRotation = player.transform.rotation;
                var spot = item.transform.Find("HideAnchor");
                if (spot) MovePlayer(spot);
                player.MovementLocked = true; player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = true;
                Hud.ShowBroadcastNotification("Tersembunyi. Matikan layar dengan F. E keluar saat aman.", 4);
            }
        }
        private IEnumerator RideLift(int y)
        {
            if (Busy) yield break;
            Busy = true; UiFocus.Cinematic = true; player.MovementLocked = true;
            // A closed service cabin masks the ride, rather than teleporting in an open corridor.
            Hud.SetFadeAlpha(1); yield return new WaitForSeconds(.25f);
            soundscape?.Play("Lift", world.liftCabin.position);
            yield return new WaitForSeconds(2);
            string anchor = y == 8 ? "L3" : y == 4 ? "MAILBOX" : "PUMP";
            if (y == 8 && !Completed("P1_LIFT_RIDE") && !Commit("P1_LIFT_RIDE", null, "PHASE1_CP3", "L3"))
            { Busy = false; UiFocus.Cinematic = false; player.MovementLocked = false; Hud.SetFadeAlpha(0); yield break; }
            world.liftCabin.position = new Vector3(world.liftCabin.position.x, y, world.liftCabin.position.z);
            MovePlayer(world.Anchor(anchor));
            Hud.SetFadeAlpha(0); UiFocus.Cinematic = false; Busy = false; player.MovementLocked = false;
            if (y == 8 && !Completed("P1_CUT_CHAIN")) Hud.ShowBroadcastNotification("LIFT MACET — panel daya servis tersedia di landing. Isi daya sebelum mengambil cutter.", 5);
        }
        private void ShowLiftDestinations()
        {
            ShowOverlay("LIFT BARANG — REM PULIH", "Pilih tujuan. Lift tidak melayani Lantai 4.",
                "L1 — servis ruang pompa", () => { CloseOverlay(); StartCoroutine(RideLift(0)); },
                "L2 — arsip penghuni", () => { CloseOverlay(); StartCoroutine(RideLift(4)); });
        }
        private IEnumerator Charge(Phase1WorldItem item)
        {
            if (charging) yield break;
            bool arena = item.id == "ARENA_POWER";
            bool full = !arena && !Campaign.Data.phase1.dynamo_full_charge_used_ids.Contains(item.id);
            var last = Campaign.Data.phase1.dynamo_emergency_last_uses.Find(e => e.id == item.id);
            float elapsed = Campaign.Data.phase1.elapsed_gameplay_seconds;
            if (!arena && !full && (Phone.batteryPercent >= 10 || last != null && elapsed - last.at_gameplay_seconds < balance.crankCooldownSeconds))
            { Hud.ShowBroadcastNotification("Dinamo darurat hanya bekerja di bawah 10%, dengan jeda 120 detik.", 4); yield break; }
            charging = true;
            if (!chargeProgress.ContainsKey(item.id)) chargeProgress[item.id] = 0;
            float needed = arena ? balance.arenaChargeSeconds : full ? balance.dynamoSeconds : balance.crankSeconds;
            float lastShown = -1;
            while (!Dead && !Busy && !UiFocus.HardModal && !Hud.IsModalOpen && InHandRange(item, 2.5f) &&
                GameInputConfig.Instance.GetActionHeld(GameAction.Interact))
            {
                if ((arena && BossActive && !threats.Recovering) || (!arena && (BossActive || threats.IsChasing)))
                { Hud.ShowBroadcastNotification(arena ? "Tunggu Genderuwo dalam recovery sebelum menyambungkan daya." : "Ada ancaman dekat. Berlindung dahulu; putaran tersimpan selama sesi.", 3); break; }
                chargeProgress[item.id] += Time.deltaTime;
                int shown = Mathf.FloorToInt(chargeProgress[item.id]);
                if (shown != lastShown) { lastShown = shown; Hud.SetInteractionPrompt($"MEMUTAR DINAMO  {chargeProgress[item.id]:0}/{needed:0} detik • lepas untuk berhenti"); }
                if (chargeProgress[item.id] >= needed)
                {
                    float amount = arena ? Mathf.Min(100, Phone.batteryPercent + balance.arenaChargeAmount) : full ? Mathf.Min(100, Phone.batteryPercent + balance.dynamoAmount) : Mathf.Min(10, Mathf.Max(Phone.batteryPercent, 10));
                    if (arena) { Phone.RestoreBattery(amount); chargeProgress[item.id] = 0; }
                    else if (Campaign.TryCommit(Campaign.Data.checkpoint_id, d =>
                    {
                        d.battery = amount;
                        if (full) d.phase1.dynamo_full_charge_used_ids.Add(item.id);
                        else
                        {
                            var receipt = d.phase1.dynamo_emergency_last_uses.Find(e => e.id == item.id);
                            if (receipt == null) d.phase1.dynamo_emergency_last_uses.Add(new Phase1DynamoUse { id = item.id, at_gameplay_seconds = d.phase1.elapsed_gameplay_seconds });
                            else receipt.at_gameplay_seconds = d.phase1.elapsed_gameplay_seconds;
                        }
                    })) { Phone.RestoreBattery(amount); chargeProgress[item.id] = 0; }
                    else Hud.ShowBroadcastNotification("Pengisian belum tersimpan. Coba lagi.", 3);
                    break;
                }
                yield return null;
            }
            charging = false;
        }
        public void TryBeginBoss()
        {
            if (BossActive || Completed("P1_BOSS_WIN") || Vector3.Distance(player.transform.position, world.Anchor("ALTAR").position) > 2.6f) return;
            if (!Phase1Ids.Ready(Campaign.Data)) { Hud.ShowBroadcastNotification("Ritual membutuhkan tiga bukti unik, konfirmasi Jurnal, dan Pecahan Keris.", 4); return; }
            if (!Campaign.TryCommit("PHASE1_CP5", d => d.phase1.safe_anchor = "ALTAR"))
            { Hud.ShowBroadcastNotification("Checkpoint ritual gagal disimpan. Altar belum diaktifkan.", 4); return; }
            if (!Campaign.BeginCheckpointAttempt(this)) return;
            BossActive = true;
            world.wall404North.SetActive(false); world.wall404South.SetActive(false);
            threats.BeginBoss(); RefreshObjective();
        }
        public void CommitBossVictory()
        {
            if (!BossActive || Completed("P1_BOSS_WIN")) return;
            Busy = true;
            if (!Campaign.TryCommit("PHASE1_CP6", d =>
            {
                d.phase1.Add("P1_BOSS_WIN"); d.phase1.safe_anchor = "AFTER_BOSS";
                d.inventory.Remove(Phase1Ids.Keris);
                if (!d.location_seals.Contains(Phase1Ids.Seal)) d.location_seals.Add(Phase1Ids.Seal);
            }, this))
            { ShowSaveRecovery("Segel belum tersimpan. Pertarungan ditahan dalam kondisi aman.", CommitBossVictory); return; }
            Campaign.EndCheckpointAttempt(this); BossActive = false; Busy = false;
            world.genderuwo.SetActive(false); world.tuyul.SetActive(false); world.cloth.SetActive(true); world.cassette.SetActive(true);
            soundscape?.Play("Seal", world.Anchor("ALTAR").position); RefreshObjective();
        }
        private IEnumerator PlayTape()
        {
            if (Busy) yield break;
            if (Completed("P1_EPILOGUE")) { Read("Kaset Mbah Suro", Phase1Content.Tape, "P1_TAPE_ARCHIVE"); yield break; }
            if(!Completed("P1_TAPE_FINISHED"))
            {
                if (!Commit("P1_TAPE_PLAY")) yield break;
                Busy = true; player.MovementLocked = true;
                soundscape?.Play("Tape", world.cassette.transform.position);
                Hud.EnqueueSubtitle("Kaset Mbah Suro", Phase1Content.Tape, 15);
                yield return new WaitForSeconds(15);
                Busy = false; player.MovementLocked = false;
                if (!Commit("P1_TAPE_FINISHED")) yield break;
            }
            if(Phone.batteryPercent<1){Hud.ShowBroadcastNotification("Video Damar tertunda. Isi HP di panel daya arena, lalu periksa radio lagi.",5);yield break;}
            // The hospital film is a handset transmission, never a playable new scene.
            Phone.isPhoneRaised = true; Phone.isScreenOn = Phone.batteryPercent > 0; Phone.SyncVisualState();
            yield return HospitalTransmission();
            Chat("@reza_arap_kw", "GILA BARUSAN ITU APAAN WOI?!"); Chat("@indah_cahyani", "Tangan abang berdarah. Cepetan turun!");
            Hud.EnqueueSubtitle("Penonton_Terakhir", Phase1Content.Epilogue, 14);
            Hud.ShowBroadcastNotification("142.000 PENONTON  •  Rp78.200.000 total kumulatif siaran (bukan saldo Bank)", 8);
            yield return new WaitForSeconds(14);
            Commit("P1_EPILOGUE");
        }
        private IEnumerator HospitalTransmission()
        {
            Busy=true;player.MovementLocked=true;
            var frame=new RenderTexture(320,512,16,RenderTextureFormat.ARGB32){name="DamarRecordedTransmission"};frame.Create();
            world.transmissionCamera.targetTexture=frame;world.transmissionCamera.enabled=true;
            var panel=PhoneSkin.Card("DamarVideoTransmission",phoneView.Panel.transform,new Vector2(.045f,.16f),new Vector2(.955f,.91f),Color.black);
            var image=UiWidgets.Rect("RecordedVideo",panel.transform,Vector2.zero,Vector2.one).AddComponent<UnityEngine.UI.RawImage>();image.texture=frame;
            UiWidgets.Text("Signal",panel.transform,"PENONTON_TERAKHIR\nREKAMAN DITERIMA",new Vector2(.06f,.82f),new Vector2(.94f,.98f),20).color=new Color(.85f,.25f,.2f);
            yield return new WaitForSeconds(5);
            Destroy(panel);world.transmissionCamera.enabled=false;world.transmissionCamera.targetTexture=null;frame.Release();Destroy(frame);
            Busy=false;player.MovementLocked=false;
        }
        private void ShowExitPanel()
        {
            bool found = Campaign.Data.secret_items.Contains(Phase1Ids.Secret);
            ShowOverlay("TINGGALKAN RUSUNAWA?", "Arsip Babak 1: " + (found ? "1/1 — Negatif Foto Tari tersimpan." : "0/1 — Arsip di Lantai 2 masih dapat diperiksa.") + "\nTujuan berikutnya: RSUD Lama Sido Waras.",
                "Kembali Menjelajah", CloseOverlay, "Lanjutkan", () =>
                {
                    if (!Commit("P1_COMPLETE", d => d.phase1.next_destination = "RSUD Lama Sido Waras", "PHASE1_CP7", "PARKING")) return;
                    CloseOverlay(); Hud.SetFadeAlpha(1); ShowChapterComplete();
                });
        }
        private void ShowChapterComplete()
        {
            ShowOverlay("PENGHUNI LANTAI EMPAT — SELESAI", "Segel Rusunawa tersimpan.\nKembali ke Kos Raka untuk mempersiapkan malam kedua di RSUD Lama Sido Waras.",
                "Menu Utama", () => GameShell.Instance.OpenMain(), "Lanjutkan — Kamar Kos", () =>
                {
                    if (!LayarTerakhir.Phase2.Phase2Campaign.EnterHub(Campaign))
                    { Hud.ShowBroadcastNotification("Transisi belum tersimpan. Periksa segel dan coba lagi.",4); return; }
                    CloseOverlay(); UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Prolog_KamarKos");
                });
        }
        private void ShowSaveRecovery(string reason, Action retry)
        { ShowOverlay("PENYIMPANAN BELUM BERHASIL", reason, "Coba simpan lagi", () => { CloseOverlay(); retry(); }, "Muat checkpoint aman", () => { CloseOverlay(); GameShell.Instance.ReturnAfterDeath("Checkpoint sebelumnya tetap utuh."); }); }
        private void ShowOverlay(string title, string body, string left, Action onLeft, string right, Action onRight)
        {
            CloseOverlay();
            var canvas = Hud.GetComponentInChildren<Canvas>().transform;
            overlayPage = UiWidgets.Panel("P1Choice", canvas, new Vector2(.13f, .20f), new Vector2(.87f, .80f), new Color(.018f, .023f, .025f, .99f));
            UiWidgets.Text("Title", overlayPage.transform, title, new Vector2(.06f, .72f), new Vector2(.94f, .94f), 34);
            UiWidgets.Text("Body", overlayPage.transform, body, new Vector2(.06f, .30f), new Vector2(.94f, .72f), 26);
            UiWidgets.Button("Left", overlayPage.transform, left, new Vector2(.06f, .08f), new Vector2(.48f, .24f), () => onLeft());
            UiWidgets.Button("Right", overlayPage.transform, right, new Vector2(.52f, .08f), new Vector2(.94f, .24f), () => onRight());
            UiFocus.Clock = true; Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
        private void CloseOverlay()
        { if (overlayPage) { overlayPage.SetActive(false); Destroy(overlayPage); } UiFocus.Clock = false; }
    }
}
