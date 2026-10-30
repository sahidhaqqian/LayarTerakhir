using System.Collections;
using System.Collections.Generic;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Interlude
{
    // Daytime side of the interlude, inside Prolog_KoridorKos.
    // Added at runtime by KosCorridorDirector when the checkpoint is an
    // interlude one; the prolog director, rain, night departures and
    // Atmaja-bound triggers are disabled, never edited in the scene file.
    [DefaultExecutionOrder(-5)]
    public sealed class InterludeDirector : MonoBehaviour, IPhoneChapterContent
    {
        public bool Ready { get; private set; }
        public bool Busy { get; private set; }
        public bool CanBeginLive => false;
        public bool CameraLocked => false;
        public int Viewers => 0;
        public string CameraStatus => Stage == "READY" ? "23:40 WIB • SIAP BERANGKAT" : "SIANG HARI • KOS & KAMPUNG MELATI";
        public string ClockLabel => Stage == "READY" ? "18:45" : Stage == "DAY" ? "16:20" : "09:05";
        public string DateLabel => Stage == "READY" ? "Senja • Berangkat ke Griya Asri" : "Sehari setelah Atmaja • Kampung Melati";

        private string Stage => Campaign != null && Campaign.Data.checkpoint_id == InterludeIds.ReadyCheckpoint ? "READY"
            : Campaign != null && Campaign.Data.checkpoint_id == InterludeIds.DayCheckpoint ? "DAY" : "MORNING";

        private KosCorridorDirector corridor;
        private CampaignState Campaign => CampaignState.Instance;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        private GameObject page;
        private bool savedGround;
        private readonly List<Zone> zones = new List<Zone>();
        private bool zonesFired;

        private struct Zone
        {
            public string id, speaker, line;
            public float duration;
            public Bounds bounds;
            public string beat;
        }

        private void Awake()
        {
            corridor = GetComponent<KosCorridorDirector>();
            if (corridor == null || Campaign == null) { Destroy(this); return; }
            Campaign.LoadCheckpoint();
            if (!InterludeIds.IsInterludeData(Campaign.Data)) { Destroy(this); return; }
            // The host Start coroutine ends itself with yield break right after
            // AddComponent returns, so no StopAllCoroutines self-stop is needed.
            corridor.enabled = false;
            foreach (var n in FindObjectsByType<MelatiNeighborhood>(FindObjectsSortMode.None)) n.enabled = false;
            foreach (var dep in FindObjectsByType<MelatiDeparture>(FindObjectsSortMode.None)) dep.enabled = false;
            foreach (var c in FindObjectsByType<ExplorationCommentary>(FindObjectsSortMode.None)) c.enabled = false;
            foreach (var t in FindObjectsByType<KosTravelItem>(FindObjectsSortMode.None)) Destroy(t);
            foreach (var rain in FindObjectsByType<NeighborhoodRain>(FindObjectsSortMode.None)) rain.enabled = false;
        }

        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !InterludeIds.IsInterludeData(Campaign.Data)) yield break;
            PlayerFootsteps.Install(corridor.player, corridor.audioProfile);
            PrologSceneFlow.RestoreBindings();
            Phone.ChapterContent = this;
            Phone.hasSmartphoneInInventory = true;
            Phone.arModuleUnlocked = Campaign.HasInventoryItem("MATABATIN_APK");
            Phone.batteryPercent = Campaign.Data.battery;
            Phone.isPhoneRaised = false;
            Phone.isBroadcastConnected = false;
            Phone.isScreenOn = Phone.batteryPercent > 0;
            Phone.SyncVisualState();
            if (corridor.rain) { corridor.rain.Stop(); corridor.rain.enabled = false; }
            if (corridor.motorHeadlight) corridor.motorHeadlight.enabled = false;

            savedGround = Campaign.Data.checkpoint_id != InterludeIds.MorningCheckpoint;
            var spawn = savedGround ? corridor.spawnGround : corridor.spawnLanding;
            var cc = corridor.player.GetComponent<CharacterController>();
            cc.enabled = false;
            corridor.player.transform.SetPositionAndRotation(spawn.position, spawn.rotation);
            cc.enabled = true;
            if (!savedGround) Campaign.SaveCheckpointAtomic(InterludeIds.MorningCheckpoint);

            corridor.gateHinge.GetComponent<DoubleLeafGate>()?.SetImmediate(true);
            InterludeExterior.DressYard(corridor);
            // Daytime street dressing lives in the scene but stays OFF for
            // the night prolog; only the interlude switches it on.
            // (Transform.Find: GameObject.Find skips inactive objects.)
            var kampung = GameObject.Find("KampungMelati");
            var alley = kampung ? kampung.transform.Find("InterludeAlley") : null;
            if (alley) alley.gameObject.SetActive(true);
            SpawnResidents();
            BindWorldItems();
            InterludeTraffic.Install(corridor);
            BuildZones();
            if (Stage == "READY") ApplyDusk(); else ApplyDay();

            Ready = true;
            Hud.SetFadeAlpha(1);
            yield return Hud.FadeBlack(1, 0, .45f);
            RefreshObjective();
        }

        private void RefreshObjective()
        {
            if (Stage == "READY")
                Hud.SetObjective("Senja — ambil motor di halaman dan berangkat ke Rusunawa Griya Asri.");
            else if (InterludeCampaign.ReadyToRest(Campaign.Data))
                Hud.SetObjective("Hari sudah cukup — naik ke kamar dan istirahat di kasur.");
            else if (!Campaign.Data.inventory.Contains(InterludeIds.RatnaTalk))
                Hud.SetObjective("Siang di kos — sapalah Bu Haji di halaman. Jelajahi kos dan warung.");
            else
                Hud.SetObjective("Siang di kos — " + InterludeCampaign.MissingForRest(Campaign.Data));
        }

        public bool CanUse(string id) => Ready && !Busy && !UiFocus.HardModal && id != null;

        public void Interact(string id)
        {
            if (!CanUse(id)) return;
            if (id == "RATNA") StartCoroutine(RatnaTalk());
            else if (id == "SARI") StartCoroutine(SariTalk());
            else if (id == "VENDOR") StartCoroutine(VendorTalk());
            else if (id == "MOTOR") MotorInteract();
            else if (id == "ROOM") StartCoroutine(ReturnToRoom());
        }

        private IEnumerator RatnaTalk()
        {
            Busy = true;
            bool paid = Campaign.HasInventoryItem("EVENT_PAID_RENT");
            bool first = InterludeCampaign.NoteBeat(Campaign, InterludeIds.RatnaTalk);
            Say("Raka", "Pagi, Bu Haji.", 2.5f);
            yield return new WaitForSeconds(2.6f);
            Say("Bu Haji", "Pagi, Mas Raka. Ya ampun... mukamu pucat kayak habis begadang seminggu. Semalam lembur, ya?", 5f);
            yield return new WaitForSeconds(5.1f);
            Say("Raka", "Kira-kira begitu, Bu. Begadang... kerja.", 3.5f);
            yield return new WaitForSeconds(3.6f);
            if (paid) Say("Bu Haji", "Oh iya, transferan kos sudah masuk. Makasih, Mas. Ibu doain ibumu lekas pulih.", 5f);
            else Say("Bu Haji", "Soal kos, transfernya jangan lewat jatuh tempo ya. Sekarang bisa lewat HP, gampang.", 5f);
            yield return new WaitForSeconds(5.1f);
            if (first) Say("Bu Haji", "Sana, sarapan dulu ke warung Bu Sri. Anak kos lain sudah pada berangkat.", 4f);
            yield return new WaitForSeconds(first ? 4.1f : 0);
            Busy = false;
            RefreshObjective();
        }

        private IEnumerator SariTalk()
        {
            Busy = true;
            InterludeCampaign.NoteBeat(Campaign, InterludeIds.NeighborTalk);
            Say("Mbak Sari", "Pagi, Mas Raka! Tumben kelihatan jam segini. Biasanya masih ngorok.", 4.5f);
            yield return new WaitForSeconds(4.6f);
            Say("Raka", "Pagi, Mbak. Semalam... rame. Live-ku meledak.", 3.5f);
            yield return new WaitForSeconds(3.6f);
            Say("Mbak Sari", "Viral itu, ya? Tetangga sebelah sampai nonton bareng. Hati-hati ya, Mas. Mukamu capek banget.", 5f);
            yield return new WaitForSeconds(5.1f);
            Busy = false;
            RefreshObjective();
        }

        private void Say(string speaker, string line, float duration) =>
            Hud.EnqueueSubtitle(speaker, line, duration);

        private IEnumerator VendorTalk()
        {
            Busy = true;
            Say("Bang Siomay", "Siomay, Mas? Panas masih. Batagor juga ada.", 4f);
            yield return new WaitForSeconds(4.1f);
            Say("Raka", "Nanti, Bang. Masih kenyang.", 2.5f);
            yield return new WaitForSeconds(2.6f);
            Busy = false;
        }

        private void MotorInteract()
        {
            if (Stage == "READY") OpenDeparture();
            else Hud.EnqueueSubtitle("Raka", "Nanti malam. Motor dipanaskan dulu sebelum ke Griya Asri.", 4f);
        }

        private void OpenDeparture()
        {
            Close();
            page = UiWidgets.Panel("InterludeDepart", Hud.GetComponentInChildren<Canvas>().transform,
                new Vector2(.13f, .08f), new Vector2(.87f, .92f), new Color(.014f, .022f, .025f, .99f));
            UiWidgets.Text("Title", page.transform, "BERANGKAT KE RUSUNAWA GRIYA ASRI?",
                new Vector2(.05f, .87f), new Vector2(.88f, .98f), 32);
            UiWidgets.Text("Body", page.transform,
                "Pukul 23:59 • Baterai penuh 100%\nDamar menunggumu di balik cermin Atmaja.\nPastikan HP terisi dan badan siap.",
                new Vector2(.05f, .70f), new Vector2(.94f, .86f), 23);
            UiWidgets.Button("Depart", page.transform, "BERANGKAT • 23:59",
                new Vector2(.06f, .32f), new Vector2(.94f, .425f), () =>
                {
                    if (!LayarTerakhir.Phase1.Phase1Director.PrepareNextNight(Campaign))
                    {
                        Hud.ShowBroadcastNotification("Keberangkatan gagal disimpan; coba lagi.", 4);
                        return;
                    }
                    Close();
                    Ready = false;
                    Hud.SetFadeAlpha(1);
                    SceneManager.LoadSceneAsync(CampaignRouting.RusunawaScene);
                });
            UiWidgets.Button("Close", page.transform, "X", new Vector2(.90f, .90f), new Vector2(.98f, .98f), Close);
            UiFocus.Clock = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private IEnumerator ReturnToRoom()
        {
            Busy = true;
            yield return Hud.FadeBlack(0, 1, .4f);
            SceneManager.LoadSceneAsync(GameShell.KosScene);
        }

        public void StartLive() { }
        public void Capture() { }

        public bool PopulateApp(PhoneApp app, Transform parent)
        {
            if (app != PhoneApp.Journal && app != PhoneApp.Map && app != PhoneApp.Inventory && app != PhoneApp.Messages) return false;
            if (app == PhoneApp.Messages && !Campaign.Data.inventory.Contains(InterludeIds.HookRead))
            {
                InterludeCampaign.NoteBeat(Campaign, InterludeIds.HookRead);
                RefreshObjective();
            }
            string body = app == PhoneApp.Map ? "KAMPUNG MELATI — SIANG\nKos • Warung Bu Sri • Jalan utama.\nMalam ini: Rusunawa Griya Asri, 23:59."
                : app == PhoneApp.Inventory ? InterludeContent.InventoryBody(Campaign.Data)
                : app == PhoneApp.Journal ? InterludeContent.JournalBody(Campaign.Data)
                : InterludeContent.IbuMessageTitle + ":\n" + InterludeContent.IbuMessageBody +
                  "\n\n— — —\n\n" + InterludeContent.HookSender + ":\n" + InterludeContent.HookBody;
            UiWidgets.ScrollText("HubContent", parent, body, new Vector2(.06f, .1f), new Vector2(.94f, .9f), 22);
            return true;
        }

        public void Close()
        {
            if (page) Destroy(page);
            page = null;
            UiFocus.Clock = false;
        }

        private void Update()
        {
            if (!Ready || Busy || UiFocus.HardModal) return;
            var player = corridor.player;
            if (player.transform.position.y < -2) { StartCoroutine(RecoverInsideBuilding()); return; }
            if (!savedGround && player.transform.position.y < .5f && player.GetComponent<CharacterController>().isGrounded)
            {
                savedGround = true;
                Campaign.SaveCheckpointAtomic(InterludeIds.DayCheckpoint);
            }
            CheckZones(player.transform.position);
        }

        private IEnumerator RecoverInsideBuilding()
        {
            Busy = true;
            corridor.player.MovementLocked = true;
            yield return Hud.FadeBlack(0, 1, .2f);
            var spawn = savedGround ? corridor.spawnGround : corridor.spawnLanding;
            var cc = corridor.player.GetComponent<CharacterController>();
            cc.enabled = false;
            corridor.player.transform.SetPositionAndRotation(spawn.position, spawn.rotation);
            cc.enabled = true;
            yield return Hud.FadeBlack(1, 0, .3f);
            corridor.player.MovementLocked = false;
            Busy = false;
        }

        private void CheckZones(Vector3 p)
        {
            foreach (var z in zones)
            {
                string flag = "COMMENT_" + z.id;
                if (!z.bounds.Contains(p) || Campaign.HasInventoryItem(flag)) continue;
                Campaign.AddInventoryItem(flag);
                Say(z.speaker, z.line, z.duration);
                if (!string.IsNullOrEmpty(z.beat)) InterludeCampaign.NoteBeat(Campaign, z.beat);
                if (z.beat == InterludeIds.RatnaTalk || z.beat == InterludeIds.HookRead ||
                    z.beat == InterludeIds.NeighborTalk) RefreshObjective();
            }
        }

        private void AddZone(string id, Vector3 center, Vector3 size, string speaker, string line, float duration, string beat = null) =>
            zones.Add(new Zone { id = id, bounds = new Bounds(center, size), speaker = speaker, line = line, duration = duration, beat = beat });

        private void BuildZones()
        {
            AddZone("INTERLUDE_KITCHEN", corridor.BuildingPoint(new Vector3(-3.15f, 1, 4.25f)), new Vector3(3, 2.5f, 3.5f),
                "Raka", "Dapur bersama. Bau kopi... ada yang sudah masak pagi-pagi.", 4f);
            AddZone("INTERLUDE_YARD", corridor.BuildingPoint(new Vector3(0, 1, -4.2f)), new Vector3(9, 3, 5.5f),
                "Raka", "Motor masih di situ. Nanti malam kita jalan lagi.", 4f);
            AddZone("INTERLUDE_UPPER", corridor.BuildingPoint(new Vector3(0, 4.4f, 6)), new Vector3(2.4f, 2.4f, 12),
                "Raka", "Kamar-kamar masih tutup. Biar mereka istirahat.", 4f);
            AddZone("INTERLUDE_BATH_MIRROR", corridor.BuildingPoint(new Vector3(3.15f, 1, 4.25f)), new Vector3(3, 2.5f, 3.4f),
                "Raka", "Pantulan di kaca wastafel itu... kedipnya telat setengah detik. ...Ah. Kurang tidur.", 6f, InterludeIds.AnomalySeen);
            AddZone("INTERLUDE_DOOR_EKO", corridor.BuildingPoint(new Vector3(.4f, 1, 7.5f)), new Vector3(1.8f, 2.2f, 2f),
                "Raka", "(ketuk) Mas Eko? Berangkat bareng?", 3f, InterludeIds.NeighborTalk);
            AddZone("INTERLUDE_STREET", new Vector3(0, 1, -30), new Vector3(10, 4, 10),
                "SFX", "[ Samar-samar... tembang Jawa dari arah gang? ] Radio tetangga, mungkin. Pagi-pagi sudah nyetel gending.", 6f, InterludeIds.AnomalySeen);
            AddZone("INTERLUDE_WARTEG", new Vector3(5.25f, 1, -30f), new Vector3(2f, 2.5f, 3.5f),
                "Raka", "Warteg Bahari buka... Nasi bungkus lima belas ribu. Nanti aja, perut masih kenyang.", 4.5f);
            AddZone("INTERLUDE_GEROBAK", new Vector3(-4.4f, 1, -20f), new Vector3(3f, 2.5f, 3f),
                "Raka", "Gorengan anget... bau minyaknya khas banget. Khas Indonesia.", 4f);
        }

        private void BindWorldItems()
        {
            var motor = corridor.motor ? corridor.motor.gameObject : null;
            if (motor)
            {
                if (!motor.GetComponent<Collider>()) motor.AddComponent<BoxCollider>();
                var item = motor.AddComponent<InterludeWorldItem>();
                item.director = this; item.id = "MOTOR"; item.prompt = "Motor — berangkat ke Rusunawa (malam nanti)";
            }
            var stairs = new GameObject("InterludeStairsReturn");
            stairs.transform.SetParent(corridor.buildingRoot, false);
            stairs.transform.localPosition = new Vector3(1.8f, .9f, 12.3f);
            var box = stairs.AddComponent<BoxCollider>();
            box.size = new Vector3(.9f, 1.8f, .9f);box.isTrigger=true;
            var room = stairs.AddComponent<InterludeWorldItem>();
            room.director = this; room.id = "ROOM"; room.prompt = "Naik ke kamar kos";
        }

        private GameObject SpawnNpc(string name, Vector3 localPos, float yaw, Color blouse, Color skirt, string id, string prompt)
        {
            var source = GameObject.Find("BuSriShopkeeper");
            if (!source) { Debug.LogError("Interlude NPC source missing for " + id); return null; }
            var npc = Object.Instantiate(source);
            npc.name = name;
            npc.transform.SetParent(corridor.buildingRoot, false);
            npc.transform.localPosition = localPos;
            npc.transform.localRotation = Quaternion.Euler(0, yaw, 0);
            foreach (var old in npc.GetComponentsInChildren<Prolog.MelatiShopkeeper>(true)) InterludeExterior.Drop(old);
            foreach (var renderer in npc.GetComponentsInChildren<Renderer>())
            {
                var mats = renderer.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    if (!mats[i]) continue;
                    string n = mats[i].name;
                    if (n.Contains("Skin") || n.Contains("Hair") || n.Contains("Eyes")) continue;
                    var clone = new Material(mats[i]);
                    clone.color = n.Contains("Blouse") ? blouse : skirt;
                    mats[i] = clone;
                }
                renderer.sharedMaterials = mats;
            }
            var item = npc.AddComponent<InterludeNpc>();
            item.director = this; item.id = id; item.prompt = prompt;
            item.player = corridor.player.transform;
            var heads = npc.GetComponentsInChildren<Transform>(true);
            foreach (var t in heads) if (t.name == "Head") { item.head = t; break; }
            return npc;
        }

        private void SpawnResidents()
        {
            SpawnNpc("IbuHajiRatna", new Vector3(-2.2f, 0, -2.5f), 130,
                new Color(.45f, .16f, .18f), new Color(.25f, .18f, .14f),
                "RATNA", "Bicara dengan Bu Haji Ratna");
            SpawnNpc("MbakSari", new Vector3(2.7f, 0, -5.0f), -60,
                new Color(.16f, .42f, .40f), new Color(.35f, .25f, .18f),
                "SARI", "Bicara dengan Mbak Sari");
            SpawnNpc("BangSiomay", corridor.buildingRoot.InverseTransformPoint(new Vector3(6.8f, 0, -17f)), -90,
                new Color(.20f, .30f, .38f), new Color(.30f, .26f, .20f),
                "VENDOR", "Beli siomay");
        }


        private void ApplyDay()
        {
            InterludeExterior.ApplyDay(corridor.player.GetComponent<InteractionInteractor>().playerCamera);
        }

        private void ApplyDusk()
        {
            InterludeExterior.ApplyDusk(corridor.player.GetComponent<InteractionInteractor>().playerCamera);
        }

        private void OnDestroy()
        {
            Close();
            if (Phone && ReferenceEquals(Phone.ChapterContent, this)) Phone.ChapterContent = null;
        }
    }
}
