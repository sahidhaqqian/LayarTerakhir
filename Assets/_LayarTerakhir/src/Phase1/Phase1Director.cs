using System;
using System.Collections;
using System.Collections.Generic;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    [DefaultExecutionOrder(-10)]
    public sealed partial class Phase1Director : MonoBehaviour, IPhoneChapterContent, IContextInteractionResolver
    {
        public FirstPersonController player;
        public Phase1World world;
        public Phase1Balance balance;
        public AtmajaPhoneView phoneView;
        public Phase1Threats threats;
        public Phase1Soundscape soundscape;
        public bool Ready { get; private set; }
        public bool Busy { get; private set; }
        public bool Dead { get; private set; }
        public bool Hidden { get; private set; }
        public bool BossActive { get; private set; }
        public bool CameraLocked => Ready && !Dead && threats && threats.KuntiPursuing;
        public bool CanBeginLive => Ready && !Busy && !Dead && !Completed("P1_START_LIVE");
        public int Viewers => Completed("P1_EPILOGUE") ? 142000 : Completed("P1_CUT_CHAIN") ? 82700 : 18500;
        public Phase1Stage Stage => Phase1Ids.Stage(Campaign.Data, BossActive);
        public string CameraStatus => CameraLocked ? "TETAP TATAP • MUNDUR KE TANGGA" : recordingEVP ?
            $"EVP  •  MEREKAM {evpProgress:0.0}/{balance.evpSeconds:0}s" : BossActive ? threats.BossStatus :
            Phone.currentScanMode == ScanVisualMode.AR ? "MATA BATIN / AR  •  [C] EVP" : "KAMERA  •  [Q] AR  [C] EVP";
        public bool OwnsInteraction => Ready && (Hidden || threats && threats.KuntiManifested && !Completed("P1_CUT_CHAIN"));
        public CampaignState Campaign => CampaignState.Instance;
        public SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        private Phase1WorldItem hidingItem;
        private Vector3 beforeHiding;
        private Quaternion beforeHidingRotation;
        private bool cutterInAttempt, passageRevealed, recordingEVP, visitedPump;
        private float passageGrace, evpProgress;
        private string lastObjective;
        private readonly Dictionary<string, float> chargeProgress = new Dictionary<string, float>();
        private readonly List<string> chatHistory = new List<string>();

        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !CampaignRouting.IsPhase1(Campaign.Data))
            { GameShell.Instance?.ReturnAfterDeath("Checkpoint Rusunawa belum tersedia. Lanjutkan dari akhir prolog."); yield break; }
            Phone.ChapterContent = this;
            player.GetComponent<InteractionInteractor>().ContextResolver = this;
            Phone.hasSmartphoneInInventory = true; Phone.arModuleUnlocked = true;
            Phone.batteryPercent = Campaign.Data.battery; Phone.isPhoneRaised = false;
            Phone.isScreenOn = Phone.batteryPercent > 0; Phone.currentScanMode = ScanVisualMode.Normal;
            Phone.isBroadcastConnected = Completed("P1_START_LIVE"); Phone.SyncVisualState();
            RestoreWorld(); Ready = true;
            Hud.SetFadeAlpha(0);
            if (Stage == Phase1Stage.Complete) { ShowChapterComplete(); yield break; }
            if (!Completed("P1_APP_PATCH")) yield return Arrival();
            if (Campaign.Data.evidence_ids.Contains(Phase1Ids.EVP) && !Completed("P1_EVP_AMBUSH_DONE")) threats.BeginBathroomAmbush();
            RefreshObjective();
        }

        public static bool PrepareNextNight(CampaignState campaign)
        {
            if (!campaign || campaign.Data.chapter_id != 0 || campaign.Data.checkpoint_id != "PROLOG_COMPLETE") return false;
            bool entered = campaign.TryCommit("PHASE1_CP1", d =>
            {
                d.chapter_id = 1; d.phase1 = new Phase1Data(); d.phase1.Add("P1_ARRIVAL");
                // Designer revision: charge once overnight on the Prolog -> P1 transition.
                // Checkpoint loads/retries never call this chapter-zero-only transaction.
                d.battery=100;
                // Keep actual sanity, money, upgrades and inventory; no starter powerbank.
            });
            if (entered) campaign.SnapshotPhaseEntry();
            return entered;
        }
        public bool Completed(string id) => Campaign && Campaign.Data.phase1.Completed(id);
        public bool Commit(string id, Action<CampaignData> mutate = null, string checkpoint = null, string anchor = null)
        {
            if (Completed(id)) return true;
            bool ok = Campaign.TryCommit(checkpoint ?? Campaign.Data.checkpoint_id, d =>
            {
                if (!d.phase1.Add(id)) return;
                mutate?.Invoke(d);
                if (anchor != null) d.phase1.safe_anchor = anchor;
            });
            if (!ok) Hud.ShowBroadcastNotification("Penyimpanan gagal. Progres belum diubah; coba lagi.", 4);
            else RefreshObjective();
            return ok;
        }
        public void RestoreWorld()
        {
            Busy = Dead = Hidden = BossActive = recordingEVP = cutterInAttempt = false; evpProgress = 0;
            UiFocus.Cinematic = UiFocus.Phone = false;
            player.MovementLocked = false;
            player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = false;
            MovePlayer(world.Anchor(Campaign.Data.phase1.safe_anchor));
            world.passageWall.SetActive(!Completed("P1_CROSS_102"));
            world.passageVeil.SetActive(!Completed("P1_CROSS_102"));
            world.chain.SetActive(!Completed("P1_CUT_CHAIN")); world.cutter.SetActive(!Completed("P1_CUT_CHAIN"));
            bool won = Completed("P1_BOSS_WIN");
            world.wall404North.SetActive(!won); world.wall404South.SetActive(!won);
            world.altarFog.SetActive(!won && !Campaign.HasInventoryItem(Phase1Ids.Keris));
            world.wall401.SetActive(!Completed("P1_GENDER_MANIFEST"));
            world.keris.SetActive(!Campaign.Data.relic_ids.Contains(Phase1Ids.Keris));
            world.secret.SetActive(!Campaign.Data.secret_items.Contains(Phase1Ids.Secret));
            if(world.safeRoomDoor)world.safeRoomDoor.SetOpenImmediate(true);
            world.cloth.SetActive(won && !Completed("P1_DAMAR_CLOTH")); world.cassette.SetActive(won);
            if (threats) threats.Restore();
        }
        private void Update()
        {
            if (!Ready || Dead || UiFocus.HardModal || UiFocus.Cinematic || Busy || Hud.IsModalOpen) return;
            Campaign.Data.phase1.elapsed_gameplay_seconds += Time.deltaTime;
            RescueFromPit();
            if (!Completed("P1_CROSS_102"))
            {
                bool reveal = CameraCanScan && Phone.currentScanMode == ScanVisualMode.AR && Aim(world.passageWall.transform.position, 5, .08f);
                if (reveal) { passageRevealed = true; passageGrace = 3; }
                else passageGrace = Mathf.Max(0, passageGrace - Time.deltaTime);
                // Never close a collider on an occupied doorway or stair landing.
                bool occupied = Vector3.Distance(player.transform.position, world.passageWall.transform.position - Vector3.up) < 3;
                bool open = passageRevealed && (passageGrace > 0 || occupied);
                if (world.passageWall.activeSelf == open) world.passageWall.SetActive(!open);
            }
            world.altarFog.SetActive(!Completed("P1_BOSS_WIN") && !Campaign.HasInventoryItem(Phase1Ids.Keris));
            if (GameInputConfig.Instance.GetActionDown(GameAction.ModeEVP) && !CameraLocked && !BossActive)
            {
                recordingEVP = !recordingEVP && CameraCanScan;
                if (recordingEVP) phoneView.Apps.Open(PhoneApp.Camera);
                else evpProgress = 0;
            }
            if (recordingEVP)
            {
                if (!CameraCanScan || Vector3.Distance(player.transform.position, world.evpPoint.position) > 3 || Hidden)
                { recordingEVP = false; evpProgress = 0; }
                else
                {
                    evpProgress += Time.deltaTime;
                    if (evpProgress >= balance.evpSeconds)
                    {
                        recordingEVP = false;
                        if (AddEvidence(Phase1Ids.EVP))
                        { Hud.EnqueueSubtitle("EVP", Phase1Content.EVP, 7); threats.BeginBathroomAmbush(); }
                    }
                }
            }
        }
        private void RescueFromPit()
        {
            // No shaft or courtyard fall may strand the player below their current level.
            // The L4 stair shaft rect excludes the flight itself (x 18.8..21.2) so normal
            // climbs and the epilogue descent never trigger a rescue.
            Vector3 p = player.transform.position;
            bool inShaftPit = p.x > 17 && p.x < 24 && p.z > -10 && p.z < -2 && !(p.x > 18.8f && p.x < 21.2f);
            bool shaftFall = inShaftPit && p.y < 11.5f &&
                (Stage == Phase1Stage.Investigation || Stage == Phase1Stage.Boss || Stage == Phase1Stage.Epilogue);
            bool courtyardFall = (Stage == Phase1Stage.Kuntilanak || Stage == Phase1Stage.Investigation ||
                Stage == Phase1Stage.Boss || Stage == Phase1Stage.Epilogue) && p.y < 2f;
            bool fellThroughWorld = p.y < -2f;
            if (!shaftFall && !courtyardFall && !fellThroughWorld) return;
            MovePlayer(world.Anchor(Campaign.Data.phase1.safe_anchor));
            Hud.ShowBroadcastNotification("Raka memanjat kembali ke jalur aman.", 3);
        }
        public bool CameraCanScan => Ready && !Dead && !Busy && Phone && Phone.isPhoneRaised && Phone.isScreenOn && Phone.batteryPercent > 0 &&
            phoneView && phoneView.IsCameraApp && !UiFocus.Phone && !UiFocus.HardModal && !Hud.IsModalOpen;
        public bool Aim(Vector3 point, float range, float margin = .15f)
        {
            if (!phoneView || !phoneView.FeedCamera) return false;
            var camera = phoneView.FeedCamera;
            Vector3 delta = point - camera.transform.position, v = camera.WorldToViewportPoint(point);
            if (delta.magnitude > range || v.z <= 0 || v.x < margin || v.x > 1 - margin || v.y < margin || v.y > 1 - margin) return false;
            return ClearLine(camera.transform.position, point);
        }
        public bool ClearLine(Vector3 from, Vector3 to,Transform targetObject=null)
        {
            Vector3 delta = to - from;
            foreach (var hit in Physics.RaycastAll(from, delta.normalized, Mathf.Max(0, delta.magnitude - .12f), ~(1 << 26 | 1 << 29 | 1 << 2), QueryTriggerInteraction.Ignore))
                if ((!targetObject||!hit.transform.IsChildOf(targetObject))&&!hit.transform.IsChildOf(player.transform) && !hit.transform.IsChildOf(world.genderuwo.transform) &&
                    !hit.transform.IsChildOf(world.kuntilanak.transform)) return false;
            return true;
        }
        public bool AddEvidence(string id)
        {
            if (Campaign.Data.evidence_ids.Contains(id)) return true;
            if (!Commit("P1_EVIDENCE_" + id, d => d.evidence_ids.Add(id), "PHASE1_CP4", "INVESTIGATION")) return false;
            Hud.ShowBroadcastNotification("Bukti tersimpan di Jurnal. " + Phase1Ids.EvidenceCount(Campaign.Data) + "/3", 4);
            return true;
        }
        public void Capture()
        {
            if (!CameraCanScan) return;
            if (threats.TryCapture()) return;
            bool ar = Phone.currentScanMode == ScanVisualMode.AR;
            bool claw = Stage == Phase1Stage.Investigation && Aim(world.clawAim.position, 4);
            bool aura = Stage == Phase1Stage.Investigation && ar && Aim(world.auraAim.position, 4);
            bool sign = ar && Campaign.Data.phase1.signboard_challenge == Phase1Challenge.Offered && Aim(world.Anchor("SIGNBOARD").position, 6);
            if (!Phone.TryCaptureEnergy(false)) { Hud.ShowBroadcastNotification("Baterai tidak cukup untuk foto.", 2); return; }
            phoneView.CaptureFlash();
            if (claw) AddEvidence(Phase1Ids.Claw);
            else if (aura) AddEvidence(Phase1Ids.Aura);
            else if (sign && Commit("P1_SAWER_SIGNBOARD", d =>
                { d.phase1.signboard_challenge = Phase1Challenge.Succeeded; Phase1Ids.Donation(d, "P1_SAWER_SIGNBOARD", 1000000, false); }))
            { Chat("@sultan_sawer", "Empat belas orang... Foto diterima. Rp1.000.000 terkirim."); soundscape?.VictimEcho();StartCoroutine(ShowVictimEcho()); }
        }
        private IEnumerator ShowVictimEcho()
        {
            if(!world.victimEcho)yield break;
            world.victimEcho.SetActive(true);yield return new WaitForSeconds(2);world.victimEcho.SetActive(false);
        }
        public bool CanInteract(Phase1WorldItem item)
        {
            if (!Ready || Busy || Dead || UiFocus.Cinematic || UiFocus.HardModal || (BossActive && !item.id.StartsWith("HALOGEN_") && item.id != "ARENA_POWER")) return false;
            if (Hidden) return item == hidingItem;
            if (item.id == "KERIS") return Campaign.Data.evidence_ids.Contains(Phase1Ids.Aura) && !Campaign.Data.relic_ids.Contains(Phase1Ids.Keris);
            if (item.id == "CLOTH" || item.id == "TAPE") return Completed("P1_BOSS_WIN");
            if (item.id == "SECRET") return Completed("P1_ARCHIVE_REVEAL") && !Campaign.Data.secret_items.Contains(Phase1Ids.Secret);
            if (item.id == "CUTTER") return threats.KuntiManifested && !cutterInAttempt;
            if (item.id == "CHAIN") return cutterInAttempt && !Completed("P1_CUT_CHAIN");
            return true;
        }
        public string ItemPrompt(Phase1WorldItem item)
        {
            if (Hidden && item == hidingItem) return "Keluar dari persembunyian";
            if (item.id.StartsWith("DYNAMO")) return "Tahan E — putar dinamo manual";
            if (item.id == "ARENA_POWER") return "Tahan E — sambungkan daya darurat";
            return item.prompt;
        }
        public void Interact(Phase1WorldItem item)
        {
            if (!CanInteract(item)) return;
            switch(item.id)
            {
                case "GUARD_LOG": Read(Phase1Content.GuardTitle, Phase1Content.Guard, "P1_GUARD_LOG"); break;
                case "MANAGER": Read(Phase1Content.ManagerTitle, Phase1Content.Manager, "P1_DOC_MANAGER"); break;
                case "TARI_DIARY": Read(Phase1Content.TariTitle, Phase1Content.Tari, "P1_DOC_TARI"); break;
                case "MAILBOX":
                    if (Completed("P1_MAILBOX_207")) { Hud.ShowBroadcastNotification("Kotak 207 kosong. Kunci lift sudah dibawa.", 3); break; }
                    Hud.OpenPadlockModal("644", () =>
                    { if (Commit("P1_MAILBOX_207", d => { d.inventory.Add(Phase1Ids.Key); d.inventory.Add(Phase1Ids.Powerbank); }, "PHASE1_CP2", "MAILBOX")) soundscape?.Play("Unlock", item.transform.position); });
                    break;
                case "ARCHIVE_LABEL":
                    if (CameraCanScan && Phone.currentScanMode == ScanVisualMode.AR)
                    { if (Commit("P1_ARCHIVE_REVEAL")) Read("Daftar arsip penghuni", "Foto yang tidak terbakar disimpan di belakang nama yang dicoret.\n\nTARI — 404\nNama disilang tinta hitam, bukan cap merah MENUNGGAK.", "P1_ARCHIVE_LABEL"); }
                    else Hud.ShowBroadcastNotification("Ada tulisan samar pada lembar arsip. Periksa dengan lensa AR.", 3);
                    break;
                case "SECRET":
                    if (Commit("P1_SECRET_COLLECTED", d => d.secret_items.Add(Phase1Ids.Secret)))
                    { world.secret.SetActive(false); Read("Negatif Foto Tari", "Empat belas penghuni hidup berdiri di lapangan jemuran. Di balik negatif tertulis: Kami pernah punya nama.\n\nArsip Saksi — 1/5", "P1_SECRET_NOTE"); }
                    break;
                case "LIFT":
                    if (!Campaign.HasInventoryItem(Phase1Ids.Key)) Hud.ShowBroadcastNotification("Panel memerlukan kunci kontak lift barang. Periksa kotak 207.", 4);
                    else if (Completed("P1_BOSS_WIN")) ShowLiftDestinations();
                    else if (!Completed("P1_LIFT_RIDE")) StartCoroutine(RideLift(8));
                    else Hud.ShowBroadcastNotification("Rem pengaman tertahan anomali di lantai empat.", 4);
                    break;
                case "CUTTER": StartCoroutine(PickupCutter()); break;
                case "CHAIN": if (!threats.CheckKuntiCatch()) StartCoroutine(CutChain()); break;
                case "KERIS":
                    if (Commit("P1_TAKE_KERIS", d => { d.inventory.Add(Phase1Ids.Keris); d.relic_ids.Add(Phase1Ids.Keris); }, "PHASE1_CP4", "INVESTIGATION")) world.keris.SetActive(false);
                    break;
                case "ALTAR": TryBeginBoss(); break;
                case "CLOTH":
                    if (Commit("P1_DAMAR_CLOTH", d => d.inventory.Add("P1_DAMAR_CLOTH")))
                    { world.cloth.SetActive(false); Hud.EnqueueSubtitle("Raka", "Ini sobekan jaket yang dipakai Damar kemarin... Berarti cermin di Rumah Atmaja emang terhubung ke tempat-tempat ritual Mbah Suro.", 8); }
                    break;
                case "TAPE": if (!Completed("P1_DAMAR_CLOTH")) Hud.ShowBroadcastNotification("Periksa kain jaket Damar di altar dahulu.", 3); else StartCoroutine(PlayTape()); break;
                case "MOTOR": if (Completed("P1_EPILOGUE")) ShowExitPanel(); else Hud.ShowBroadcastNotification("Damar belum ditemukan. Cari akses ke Rusunawa.", 3); break;
                case "ARENA_POWER": if (BossActive || Completed("P1_BOSS_WIN")) StartCoroutine(Charge(item)); break;
                default:
                    if (item.id.StartsWith("HALOGEN_")) threats.UseHalogen(item.id);
                    else if (item.id.StartsWith("DYNAMO")) { if (!BossActive) StartCoroutine(Charge(item)); }
                    else if (item.id.StartsWith("HIDE")) ToggleHide(item);
                    break;
            }
        }
        private void Read(string title, string text, string id)
        {
            if (BossActive) return;
            // ArchiveDocument is already transactional. Record the receipt in the same campaign.
            Hud.OpenDocumentModal(title, text, () => { if (!Completed(id)) Commit(id); }, id);
        }
        public void EnterZone(string id)
        {
            if (!Ready || Dead || Busy || BossActive) return;
            switch(id)
            {
                case "PUMP":
                    visitedPump=true;
                    if (!Completed("P1_START_LIVE")) { Hud.ShowBroadcastNotification("Nyalakan Live lewat tombol di aplikasi Kamera sebelum masuk.", 3); return; }
                    Commit("P1_ENTER_PUMP", d => { if (d.phase1.signboard_challenge == Phase1Challenge.Offered) d.phase1.signboard_challenge = Phase1Challenge.Expired; }, "PHASE1_CP1", "PUMP"); break;
                case "CROSS_102":
                    if (passageRevealed && Commit("P1_CROSS_102", null, null, "MAILBOX")) { world.passageWall.SetActive(false); world.passageVeil.SetActive(false); } break;
                case "TUYUL": if (!Completed("P1_TUYUL_L2")) threats.BeginTuyul(false); break;
                case "KUNTI": if (Completed("P1_LIFT_RIDE") && !Completed("P1_CUT_CHAIN")) threats.ManifestKunti(); break;
                case "GENDER":
                    if (Completed("P1_CUT_CHAIN") && !Completed("P1_GENDER_MANIFEST") && Commit("P1_GENDER_MANIFEST"))
                    { world.wall401.SetActive(false); threats.ManifestGender(); Chat("@siska_eee", "MATIIN LAYAR KALAU DIA DEKAT. Ada lemari di kamar 403!"); }
                    break;
                case "EVP_HINT": if (!Campaign.Data.evidence_ids.Contains(Phase1Ids.EVP)) Hud.ShowBroadcastNotification("Statis dari kamar mandi. [C] aktifkan EVP di dekat radio.", 4); break;
            }
        }
        public IInteractable ResolveContextInteraction()
        {
            if (Hidden) return hidingItem;
            if (!threats || !threats.KuntiManifested) return null;
            Phase1WorldItem item = cutterInAttempt ? world.chainItem : world.cutterItem;
            if (CanInteract(item) && InHandRange(item, cutterInAttempt ? 1.6f : 1.3f)) return item;
            // The service dynamo stays usable until the pursuit starts; the panel
            // is on the escape path side, so reaching it never turns the camera.
            if (!cutterInAttempt && world.servicePanelItem && CanInteract(world.servicePanelItem) && InHandRange(world.servicePanelItem, 2.5f)) return world.servicePanelItem;
            return null;
        }
        public bool InHandRange(Phase1WorldItem item, float radius)
        {
            Vector3 hand = player.transform.position + Vector3.up;
            Vector3 point = item.transform.position;
            return Vector3.Distance(hand, point) <= radius && ClearLine(hand, point,item.transform);
        }
        private IEnumerator Arrival()
        {
            string opening = Campaign.HasInventoryItem("EVENT_PAID_RENT") && Campaign.HasInventoryItem("EVENT_PAID_HOSPITAL") ?
                "Kos udah lunas, deposit operasi Ibu udah masuk. Tapi Damar masih terjebak di balik cermin sialan itu... Empat segel harus gue kumpulin." :
                "Damar masih terjebak di balik cermin sialan itu... Empat segel harus gue kumpulin. Kalau gue mundur malam ini, Damar mati.";
            Hud.EnqueueSubtitle("Raka", opening, 9);
            yield return new WaitForSeconds(9);
            if (Commit("P1_APP_PATCH",data=>Phase1Ids.Donation(data,"P1_APP_PATCH",10000000,true))) { Hud.ShowBroadcastNotification("23:58 — JURNAL KLENIK v2.0 terpasang. Dana kontrak Rp10.000.000 tercatat sekali.", 6); Chat("Penonton_Terakhir", "Empat belas penghuni terbakar pada 2004. Cari tiga bukti kematiannya. Cahaya meruntuhkan perisainya."); }
            // Only authored transactions enter the ledger. Never top the wallet up to the narrative milestone.
        }
        public void StartLive()
        {
            if (!CanBeginLive || !Commit("P1_START_LIVE", d => d.phase1.signboard_challenge = Phase1Challenge.Offered)) return;
            Phone.isBroadcastConnected = true; Phone.SyncVisualState(); phoneView.Apps.Open(PhoneApp.Camera);
            Chat("@detektif_malam", "18K penonton! Bang Damar belum ketemu?");
            Chat("@sultan_sawer", "Foto AR papan nama bercak darah. Kalau muncul penampakan, gue cairin sejuta!");
            Hud.EnqueueSubtitle("Raka", "Malam ini gue di Rusunawa Griya Asri. Gue cuma butuh mata kalian buat bantu gue selamat.", 7);
            // Live may be started after entering the pump room: no artificial wall,
            // and no lost OnTriggerEnter that leaves the chapter objective behind.
            if(visitedPump)
                EnterZone("PUMP");
        }
        private IEnumerator PickupCutter()
        {
            if (cutterInAttempt) yield break;
            Busy = true; player.MovementLocked = true;
            yield return new WaitForSeconds(.65f);
            cutterInAttempt = true; world.cutter.SetActive(false);
            Phone.isPhoneRaised = true; Phone.isScreenOn = Phone.batteryPercent > 0; Phone.currentScanMode = ScanVisualMode.Normal; Phone.SyncVisualState();
            phoneView.Apps.Open(PhoneApp.Camera);
            Busy = false; player.MovementLocked = false; threats.BeginKuntiPursuit();
            Chat("@bambang_gentolet", "Bang... kok gue ikut denger suara nangis?");
        }
        private IEnumerator CutChain()
        {
            Busy = true; player.MovementLocked = true; threats.FreezeKunti = true;
            soundscape?.Play("Chain", world.chain.transform.position);
            yield return new WaitForSeconds(balance.chainSeconds);
            if (!Commit("P1_CUT_CHAIN", d => d.inventory.Add("P1_BOLT_CUTTER"), "PHASE1_L4_LANDING", "L4"))
            { Busy = false; ShowSaveRecovery("Rantai belum tersimpan. Kamera dan ancaman ditahan agar kamu bisa mencoba lagi.", () => StartCoroutine(CutChain())); yield break; }
            world.chain.SetActive(false); threats.ResolveKunti();
            Busy = false; player.MovementLocked = false; RefreshObjective();
        }
        public void MovePlayer(Transform anchor)
        {
            var capsule = player.GetComponent<CharacterController>(); capsule.enabled = false;
            player.transform.SetPositionAndRotation(anchor.position, anchor.rotation); player.SetViewPitch(0);
            capsule.enabled = true; player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            Physics.SyncTransforms();
        }
        public void Die(string reason)
        {
            if (Dead || Busy || threats && threats.FreezeKunti) return;
            Dead = true; Phone.isBroadcastConnected = false;
            Campaign.EndCheckpointAttempt(this);
            soundscape?.Play("Impact", player.transform.position);
            GameShell.Instance?.ReturnAfterDeath(reason + "  Continue memuat checkpoint aman terakhir.");
        }
        private void RefreshObjective()
        {
            if (!Campaign) return;
            string objective = Stage switch
            {
                Phase1Stage.Arrival => "Periksa pos satpam. Angkat HP dan mulai Live lewat aplikasi Kamera.",
                Phase1Stage.Pump => "Cari saluran ruang pompa di belakang gedung.",
                Phase1Stage.Passage102 => "Periksa anomali Kamar 102 dengan lensa AR.",
                Phase1Stage.Mailbox => "Temukan kode kotak surat 207 dari papan status, log, dan denah.",
                Phase1Stage.Lift => "Gunakan kunci kontak pada lift barang.",
                Phase1Stage.Kuntilanak => "Isi daya di panel bila perlu. Telusuri koridor dan ambil pemotong besi.",
                Phase1Stage.Investigation => $"Bukti {Phase1Ids.EvidenceCount(Campaign.Data)}/3. Selidiki 402, kamar mandi, dan tungku. Konfirmasi Jurnal sebelum ritual.",
                Phase1Stage.Boss => "Pancing Genderuwo ke halogen. Tarik tuas, bidik kepala, Blitz dalam 5 detik.",
                Phase1Stage.Epilogue => "Periksa kain jaket dan kaset di altar Kamar 404.",
                Phase1Stage.Return => "Turun lewat lift ke L1. Opsional: ambil arsip Tari di L2. Kembali ke motor.",
                _ => "Babak 1 selesai. Tujuan tersimpan: RSUD Lama Sido Waras."
            };
            if (objective != lastObjective) { lastObjective = objective; Hud.SetObjective(objective); }
        }
        private void OnDestroy()
        {
            if (Campaign) Campaign.EndCheckpointAttempt(this);
            if (Phone && ReferenceEquals(Phone.ChapterContent, this)) Phone.ChapterContent = null;
            UiFocus.Cinematic = UiFocus.Phone = false;
        }
    }
}
