using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Phase2
{
    public sealed class Phase2Hub : MonoBehaviour, IPhoneChapterContent
    {
        public bool Ready { get; private set; }
        public bool CanBeginLive => false;
        public bool CameraLocked => false;
        public int Viewers => 0;
        public string CameraStatus => "14:00 WIB • KOS RAKA / PERSIAPAN RSUD";
        private GameObject page;
        private AtmajaPhoneView view;
        private CampaignState Campaign => CampaignState.Instance;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        public static void Install(KamarKosPrologDirector prolog)
        {
            prolog.enabled = false;
            var hub = prolog.gameObject.AddComponent<Phase2Hub>();
            // Only runtime interaction owners change. The enlarged Golden Kos is never rebuilt.
            foreach (var item in FindObjectsByType<KosInteractableItem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                Object.Destroy(item);
            hub.Bind(prolog.pcMonitorItem ? prolog.pcMonitorItem.gameObject : GameObject.Find("Golden_Workstation"), "PC", "Riset forum / Pasar Gaib");
            if(prolog.motorKeyItem)hub.Bind(prolog.motorKeyItem.gameObject, "DEPART", "Kunci motor — berangkat ke RSUD");
            hub.Bind(prolog.kosDoorItem ? prolog.kosDoorItem.gameObject : null, "DEPART", "Berangkat ke RSUD Sido Waras");
            var socket = GameObject.Find("PhoneCharger") ?? GameObject.Find("OldWallSocket");
            hub.Bind(socket, "CHARGE", "Colokan — isi HP untuk malam ini");
            if (prolog.smartphoneBedItem) prolog.smartphoneBedItem.gameObject.SetActive(false);
            if (prolog.suratKosItem) prolog.suratKosItem.gameObject.SetActive(false);
            if (prolog.phoneBedRedLed) prolog.phoneBedRedLed.enabled = false;
            if (prolog.environmentAudio) prolog.environmentAudio.enabled = false;
        }
        private void Bind(GameObject target, string id, string prompt)
        {
            if (!target) { Debug.LogError("P2 HUB missing interaction " + id); return; }
            target.SetActive(true);
            if(!target.GetComponent<Collider>())target.AddComponent<BoxCollider>();
            var item = target.AddComponent<Phase2WorldItem>(); item.hub = this; item.id = id; item.prompt = prompt;
        }
        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !CampaignRouting.IsPhase2(Campaign.Data)) yield break;
            if (Campaign.Data.phase2.Completed("P2_DEPART_KOS")) { SceneManager.LoadSceneAsync(CampaignRouting.HospitalScene); yield break; }
            view = FindFirstObjectByType<AtmajaPhoneView>();
            Phone.ChapterContent = this; Phone.hasSmartphoneInInventory = true; Phone.isPhoneRaised = false;
            Phone.isBroadcastConnected = false; Phone.batteryPercent = Campaign.Data.battery;
            Phone.isScreenOn = Phone.batteryPercent > 0; Phone.selfieModuleUnlocked = Campaign.Data.phase2.Completed("P2_UPGRADE_UNLOCK");
            Phone.isSelfieCamera = false; Phone.SyncVisualState();
            // A daytime lighting override on the existing approved scene, no geometry edits.
            foreach (var light in FindObjectsByType<Light>(FindObjectsSortMode.None))
                if (light.type == LightType.Directional) { light.color = new Color(1, .94f, .83f); light.intensity = 1.4f; light.transform.rotation = Quaternion.Euler(46, -35, 0); }
            RenderSettings.ambientLight = new Color(.42f, .43f, .43f);
            RenderSettings.ambientSkyColor = new Color(.40f,.44f,.48f);
            RenderSettings.ambientEquatorColor = new Color(.25f,.27f,.28f);
            RenderSettings.ambientGroundColor = new Color(.14f,.13f,.12f);
            Ready = true; Hud.SetFadeAlpha(0); Hud.SetObjective("14:00 WIB — Buka PC: riset lokasi kedua dan pasang modul Smartphone. Pintu kos untuk berangkat.");
        }
        public void Interact(string id)
        {
            if (!Ready || UiFocus.HardModal) return;
            if (id == "PC") OpenPC();
            else if (id == "CHARGE")
            {
                if (Phase2Campaign.ChargeHub(Campaign)) { Phone.RestoreBattery(100); Hud.ShowBroadcastNotification("HP terisi penuh. Pengisian persiapan malam ini tersimpan.", 4); }
                else Hud.ShowBroadcastNotification(Campaign.Data.phase2.Completed("P2_KOS_CHARGED") ? "Pengisian persiapan malam ini sudah dipakai." : "Pengisian gagal disimpan; coba lagi.", 4);
            }
            else if (id == "DEPART") OpenDeparture();
        }
        public void OpenPC()
        {
            Close(); Panel("PC RAKA — 14:00 WIB", "Forum Urban Legend / Pasar Gaib\nSaldo tersedia: Rp" + Campaign.Data.wallet_legitimate.ToString("N0"));
            Button("Forum", "Baca: Tragedi RSUD Sido Waras", .62f, () =>
            {
                if (!Campaign.TryCommit(Campaign.Data.checkpoint_id, d =>
                {
                    d.phase2.Add("P2_FORUM_READ"); Phase2Campaign.Unlock(d);
                    if(!d.inventory.Contains("P2_DOC_FORUM"))d.inventory.Add("P2_DOC_FORUM");
                    if (!d.documents.Exists(doc => doc.title == Phase2Content.ForumTitle)) d.documents.Add(new PhoneDocument(Phase2Content.ForumTitle, Phase2Content.Forum));
                })) { SaveError(); return; }
                Phone.selfieModuleUnlocked = true; Close(); Hud.OpenDocumentModal(Phase2Content.ForumTitle, Phase2Content.Forum);
            });
            Button("Modules", "Pasang Selfie Mode + ThermoKlenik.apk • GRATIS", .49f, () =>
            {
                if (Campaign.TryCommit(Campaign.Data.checkpoint_id, Phase2Campaign.Unlock))
                { Phone.selfieModuleUnlocked = true; Close(); Hud.ShowBroadcastNotification("Modul terpasang: X Selfie • T ThermoKlenik. Ubah tombol lewat Pengaturan.", 6); }
                else SaveError();
            });
            Shop("POWERBANK", "Powerbank besar ×2 (+80) • Rp500.000", .36f, 500000);
            Shop("KEMENYAN", "Kemenyan Bakar ×2 • Rp350.000", .23f, 350000);
            Shop("FLARE", "Flare Merah Darurat ×3 • Rp750.000", .10f, 750000);
        }
        private void Shop(string id, string title, float y, long price)
        {
            bool bought = Campaign.Data.phase2.Completed("P2_SHOP_PACK_" + id);
            var b = Button(id, bought ? title + " • TERBELI" : title, y, () =>
            { if (Phase2Campaign.Buy(Campaign, id)) OpenPC(); else SaveError(); });
            b.interactable = !bought && Campaign.Data.wallet_legitimate >= price;
        }
        private void OpenDeparture()
        {
            Close(); Panel("BERANGKAT KE RSUD SIDO WARAS?", "Pukul 23:45 WIB • Baterai aktual: " + Phone.batteryPercent.ToString("0") + "%\nPersediaan opsional. Modul wajib dipasang otomatis jika belum.\nKamu boleh kembali ke meja untuk mengisi HP sebelum pergi.");
            Button("Depart", "BERANGKAT • 23:45 WIB", .32f, () =>
            {
                if (!Phase2Campaign.Depart(Campaign)) { SaveError(); return; }
                Close(); Ready = false; Hud.SetFadeAlpha(1); SceneManager.LoadSceneAsync(CampaignRouting.HospitalScene);
            });
        }
        private void Panel(string title, string body)
        {
            page = UiWidgets.Panel("Phase2HubPC", Hud.GetComponentInChildren<Canvas>().transform, new Vector2(.13f, .08f), new Vector2(.87f, .92f), new Color(.014f, .022f, .025f, .99f));
            UiWidgets.Text("Title", page.transform, title, new Vector2(.05f,.87f), new Vector2(.88f,.98f), 32);
            UiWidgets.Text("Body", page.transform, body, new Vector2(.05f,.70f), new Vector2(.94f,.86f), 23);
            UiWidgets.Button("Close",page.transform,"X",new Vector2(.90f,.90f),new Vector2(.98f,.98f),Close);
            UiFocus.Clock = true; Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
        private UnityEngine.UI.Button Button(string id, string text, float y, UnityEngine.Events.UnityAction action) =>
            UiWidgets.Button(id,page.transform,text,new Vector2(.06f,y),new Vector2(.94f,y+.105f),action);
        private void SaveError() => Hud.ShowBroadcastNotification("Penyimpanan belum berhasil. Saldo dan barang tetap utuh; coba lagi.", 5);
        public void Close() { if (page) Destroy(page); page = null; UiFocus.Clock = false; }
        public void StartLive() { }
        public void Capture() { }
        public bool PopulateApp(PhoneApp app, Transform parent)
        {
            if (app != PhoneApp.Journal && app != PhoneApp.Map && app != PhoneApp.Inventory && app != PhoneApp.Messages) return false;
            string body = app == PhoneApp.Map ? "KOS RAKA → RSUD LAMA SIDO WARAS\nBerangkat dari kunci motor di meja.\nTiba 23:50 WIB." :
                app == PhoneApp.Inventory ? InventoryText() :
                "PERSIAPAN MALAM KEDUA\n\n" + (Campaign.Data.phase2.Completed("P2_FORUM_READ") ? Phase2Content.Forum : "Buka PC untuk meriset RSUD Lama Sido Waras.");
            UiWidgets.ScrollText("HubContent",parent,body,new Vector2(.06f,.1f),new Vector2(.94f,.9f),22); return true;
        }
        private string InventoryText()
        {
            string text="PERSEDIAAN\n\n";
            foreach(var id in Campaign.Data.inventory){var label=Phase2Content.ItemLabel(id);if(label!=null)text+="• "+label+"\n";}
            return text;
        }
        private void OnDestroy() { if (page) Close(); if (Phone && ReferenceEquals(Phone.ChapterContent,this)) Phone.ChapterContent = null; }
    }
}
