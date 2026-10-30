using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Interlude
{
    // Morning side of the daytime interlude, inside the approved Golden Kos.
    // Runtime interaction owners only; room geometry is never rebuilt.
    public sealed class InterludeHub : MonoBehaviour, IPhoneChapterContent
    {
        public bool Ready { get; private set; }
        public bool CanBeginLive => false;
        public bool CameraLocked => false;
        public int Viewers => 0;
        public string CameraStatus => Stage == "READY" ? "23:40 WIB • SIAP BERANGKAT" : "SIANG HARI • KAMAR KOS";
        public string ClockLabel => Stage == "READY" ? "23:40" : Stage == "DAY" ? "15:40" : "08:12";
        public string DateLabel => Stage == "READY" ? "Malam • Berangkat ke Griya Asri" : "Sehari setelah Atmaja • Kos Raka";

        private string Stage => Campaign != null && Campaign.Data.checkpoint_id == InterludeIds.ReadyCheckpoint ? "READY"
            : Campaign != null && Campaign.Data.checkpoint_id == InterludeIds.DayCheckpoint ? "DAY" : "MORNING";

        private GameObject page;
        private CampaignState Campaign => CampaignState.Instance;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;

        public static void Install(KamarKosPrologDirector prolog)
        {
            prolog.enabled = false;
            var hub = prolog.gameObject.AddComponent<InterludeHub>();
            foreach (var item in FindObjectsByType<KosInteractableItem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                Object.Destroy(item);
            hub.Bind(prolog.pcMonitorItem ? prolog.pcMonitorItem.gameObject : GameObject.Find("Golden_Workstation"), "PC", "PC — rekap siaran & kabar");
            hub.Bind(prolog.kosDoorItem ? prolog.kosDoorItem.gameObject : null, "EXIT", "Keluar ke halaman kos");
            var socket = GameObject.Find("PhoneCharger") ?? GameObject.Find("OldWallSocket");
            hub.Bind(socket, "CHARGE", "Colokan — isi HP untuk malam ini");
            var bed = GameObject.Find("vintage_day_bed");
            hub.Bind(bed, "BED", "Istirahat — lanjutkan malam nanti");
            if (prolog.smartphoneBedItem) prolog.smartphoneBedItem.gameObject.SetActive(false);
            if (prolog.suratKosItem) prolog.suratKosItem.gameObject.SetActive(false);
            if (prolog.phoneBedRedLed) prolog.phoneBedRedLed.enabled = false;
            if (prolog.environmentAudio) prolog.environmentAudio.enabled = false;
        }

        private void Bind(GameObject target, string id, string prompt)
        {
            if (!target) { Debug.LogError("Interlude HUB missing interaction " + id); return; }
            target.SetActive(true);
            if (!target.GetComponent<Collider>()) target.AddComponent<BoxCollider>();
            var item = target.AddComponent<InterludeWorldItem>(); item.hub = this; item.id = id; item.prompt = prompt;
        }

        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !InterludeIds.IsInterludeData(Campaign.Data)) yield break;
            Phone.ChapterContent = this;
            Phone.hasSmartphoneInInventory = true;
            Phone.arModuleUnlocked = Campaign.HasInventoryItem("MATABATIN_APK");
            Phone.isPhoneRaised = false;
            Phone.isBroadcastConnected = false;
            Phone.batteryPercent = Campaign.Data.battery;
            Phone.isScreenOn = Phone.batteryPercent > 0;
            Phone.SyncVisualState();
            ApplyLighting();
            Ready = true;
            Hud.SetFadeAlpha(0);
            if (!Campaign.HasInventoryItem(InterludeIds.Woke))
            {
                InterludeCampaign.NoteBeat(Campaign, InterludeIds.Woke);
                Hud.EnqueueSubtitle("Raka", "Pagi... badan remuk semua. Semalam... gila.", 4f);
                Hud.EnqueueSubtitle("Raka", "Hidup terus. Kos, Ibu, Damar. Satu-satu, Ka.", 4f);
            }
            RefreshObjective();
        }

        private void ApplyLighting()
        {
            bool dusk = Stage == "READY";
            foreach (var light in FindObjectsByType<Light>(FindObjectsSortMode.None))
                if (light.type == LightType.Directional)
                {
                    light.color = dusk ? new Color(.95f, .55f, .38f) : new Color(1f, .94f, .83f);
                    light.intensity = dusk ? .55f : 1.4f;
                    light.transform.rotation = dusk
                        ? Quaternion.Euler(18, -60, 0)
                        : Quaternion.Euler(46, -35, 0);
                }
            if (dusk)
            {
                RenderSettings.ambientLight = new Color(.16f, .13f, .12f);
                RenderSettings.ambientSkyColor = new Color(.20f, .15f, .14f);
                RenderSettings.ambientEquatorColor = new Color(.12f, .11f, .10f);
                RenderSettings.ambientGroundColor = new Color(.07f, .06f, .06f);
            }
            else
            {
                RenderSettings.ambientLight = new Color(.42f, .43f, .43f);
                RenderSettings.ambientSkyColor = new Color(.40f, .44f, .48f);
                RenderSettings.ambientEquatorColor = new Color(.25f, .27f, .28f);
                RenderSettings.ambientGroundColor = new Color(.14f, .13f, .12f);
            }
        }

        private void RefreshObjective()
        {
            if (Stage == "READY")
                Hud.SetObjective("Malam tiba — keluar ke halaman, ambil motor dan berangkat ke Rusunawa.");
            else if (InterludeCampaign.ReadyToRest(Campaign.Data))
                Hud.SetObjective("Hari sudah cukup — kembali ke kamar dan istirahat di kasur.");
            else if (Campaign.Data.checkpoint_id == InterludeIds.MorningCheckpoint)
                Hud.SetObjective("Pagi di kos — keluar dan sapalah Bu Haji di halaman. [TAB] cek HP.");
            else
                Hud.SetObjective("Siang di kos — jelajahi halaman dan warung. " + InterludeCampaign.MissingForRest(Campaign.Data));
        }

        public bool CanUse(string id) => Ready && !UiFocus.HardModal && id != null;

        public void Interact(string id)
        {
            if (!Ready || UiFocus.HardModal) return;
            if (id == "PC") OpenPC();
            else if (id == "CHARGE")
            {
                if (InterludeCampaign.ChargeHub(Campaign))
                {
                    Phone.RestoreBattery(100);
                    Hud.ShowBroadcastNotification("HP terisi penuh. Baterai aktual tersimpan untuk malam ini.", 4);
                }
                else Hud.ShowBroadcastNotification(Campaign.Data.inventory.Contains(InterludeIds.Charged)
                    ? "Pengisian siang ini sudah dipakai." : "Pengisian gagal disimpan; coba lagi.", 4);
            }
            else if (id == "BED") TryRest();
            else if (id == "EXIT") StartCoroutine(ExitToYard());
        }

        private void TryRest()
        {
            if (Stage == "READY")
            {
                Hud.EnqueueSubtitle("Raka", "Nggak bisa tidur lagi. Waktunya berangkat.", 3f);
                return;
            }
            string missing = InterludeCampaign.MissingForRest(Campaign.Data);
            if (!string.IsNullOrEmpty(missing))
            {
                Hud.EnqueueSubtitle("Raka", "Belum mengantuk. " + missing, 4f);
                return;
            }
            StartCoroutine(RestSequence());
        }

        private IEnumerator RestSequence()
        {
            Ready = false;
            yield return Hud.FadeBlack(0, 1, 1.2f);
            if (!InterludeCampaign.RestUntilNight(Campaign))
            {
                yield return Hud.FadeBlack(1, 0, .8f);
                Ready = true;
                Hud.ShowBroadcastNotification("Istirahat gagal disimpan; coba lagi.", 4);
                yield break;
            }
            Phone.batteryPercent = Campaign.Data.battery;
            Phone.SyncVisualState();
            ApplyLighting();
            yield return Hud.FadeBlack(1, 0, 1.2f);
            Hud.EnqueueSubtitle("Raka", "23:40. Waktunya berangkat.", 3.5f);
            Hud.EnqueueSubtitle("Raka", "Gue harus ke Griya Asri malam ini. Damar nunggu.", 4f);
            Ready = true;
            RefreshObjective();
        }

        private IEnumerator ExitToYard()
        {
            Ready = false;
            yield return Hud.FadeBlack(0, 1, .4f);
            yield return PrologSceneFlow.LoadCorridor();
        }

        public void OpenPC()
        {
            Close();
            Panel("PC RAKA — REKAP", InterludeContent.RecapBody(Campaign.Data));
            Button("PCBank", "BANK: transfer kos/RS lewat HP (TAB → Bank)", .32f, () =>
            {
                Close();
                Hud.EnqueueSubtitle("Raka", "Transfernya lewat HP aja. Buka Bank, pilih tagihan.", 4f);
            });
            Button("PCClose", "TUTUP", .18f, Close);
        }

        private void Panel(string title, string body)
        {
            page = UiWidgets.Panel("InterludeHubPC", Hud.GetComponentInChildren<Canvas>().transform,
                new Vector2(.13f, .08f), new Vector2(.87f, .92f), new Color(.014f, .022f, .025f, .99f));
            UiWidgets.Text("Title", page.transform, title, new Vector2(.05f, .87f), new Vector2(.88f, .98f), 32);
            UiWidgets.ScrollText("Body", page.transform, body, new Vector2(.05f, .05f), new Vector2(.94f, .86f), 23);
            UiWidgets.Button("Close", page.transform, "X", new Vector2(.90f, .90f), new Vector2(.98f, .98f), Close);
            UiFocus.Clock = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Button(string id, string text, float y, UnityEngine.Events.UnityAction action) =>
            UiWidgets.Button(id, page.transform, text, new Vector2(.06f, y), new Vector2(.94f, y + .105f), action);

        public void Close()
        {
            if (page) Destroy(page);
            page = null;
            UiFocus.Clock = false;
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
            string body = app == PhoneApp.Map ? "KOS RAKA — KAMPUNG MELATI\nPagi yang tenang. Motor di halaman.\nMalam ini: Rusunawa Griya Asri, 23:59."
                : app == PhoneApp.Inventory ? InterludeContent.InventoryBody(Campaign.Data)
                : app == PhoneApp.Journal ? InterludeContent.JournalBody(Campaign.Data)
                : IbuThread() + "\n\n— — —\n\n" + HookThread();
            UiWidgets.ScrollText("HubContent", parent, body, new Vector2(.06f, .1f), new Vector2(.94f, .9f), 22);
            return true;
        }

        private string IbuThread() =>
            InterludeContent.IbuMessageTitle + ":\n" + InterludeContent.IbuMessageBody;

        private string HookThread() =>
            InterludeContent.HookSender + ":\n" + InterludeContent.HookBody;

        private void OnDestroy()
        {
            Close();
            if (Phone && ReferenceEquals(Phone.ChapterContent, this)) Phone.ChapterContent = null;
        }
    }
}
