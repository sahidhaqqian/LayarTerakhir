using System.Text;
using LayarTerakhir.Core;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using UnityEngine;
using UnityEngine.InputSystem;
namespace LayarTerakhir.UI
{
    public enum PhoneApp { Home,Camera,Map,Messages,Journal,Inventory }
    public sealed class PhoneDesktop:MonoBehaviour
    {
        public PhoneApp CurrentApp {get;private set;}
        public bool TravelSelection {get;private set;}
        private AtmajaPhoneView view;private GameObject root,page;private bool lastAR;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        public void Initialize(AtmajaPhoneView owner){view=owner;root=UiWidgets.Rect("PhoneApps",view.Panel.transform,new Vector2(.045f,.15f),new Vector2(.955f,.89f));Open(PhoneApp.Home);}
        private void Update()
        {
            if(!Phone||!Phone.hasSmartphoneInInventory||!root||UiFocus.Cinematic||HudController.Instance.IsModalOpen)return;
            if(Keyboard.current!=null)
            {
                if(Keyboard.current.zKey.wasPressedThisFrame&&Phone.isPhoneRaised)Open(CurrentApp==PhoneApp.Home?PhoneApp.Camera:PhoneApp.Home);
                if(Keyboard.current.mKey.wasPressedThisFrame){Phone.isPhoneRaised=true;Open(PhoneApp.Map);Phone.SyncVisualState();}
            }
            bool ar=Phone.currentScanMode==ScanVisualMode.AR;if(ar&&!lastAR&&Phone.isPhoneRaised)Open(PhoneApp.Camera);lastAR=ar;
        }
        public void SetScreenVisible(bool on){if(root)root.SetActive(on&&CurrentApp!=PhoneApp.Camera);}
        public void Open(PhoneApp app)
        {
            CurrentApp=app;TravelSelection=false;if(page)Destroy(page);if(!root)return;
            page=UiWidgets.Panel("App_"+app,root.transform,Vector2.zero,Vector2.one,new Color(.025f,.033f,.038f,.99f));
            if(app==PhoneApp.Camera){root.SetActive(false);return;}if(app==PhoneApp.Home){Home();return;}
            UiWidgets.Text("AppTitle",page.transform,app.ToString().ToUpperInvariant(),new Vector2(.07f,.87f),new Vector2(.93f,.98f),28);
            UiWidgets.Button("BackHome",page.transform,"‹ APLIKASI",new Vector2(.05f,.01f),new Vector2(.95f,.10f),()=>Open(PhoneApp.Home));
            if(app==PhoneApp.Map)Map();else if(app==PhoneApp.Messages)Messages();else if(app==PhoneApp.Journal)Journal();else if(app==PhoneApp.Inventory)Inventory();
        }
        private void Home()
        {
            UiWidgets.Text("Clock",page.transform,view.director?"23:52":"23:46",new Vector2(.08f,.72f),new Vector2(.92f,.94f),66,TextAnchor.MiddleCenter);
            UiWidgets.Text("Date",page.transform,"RAKA WIJAYA • MALAM KERAMAT",new Vector2(.08f,.64f),new Vector2(.92f,.74f),18,TextAnchor.MiddleCenter);
            string[] labels={"KAMERA\nMalam Keramat","PETA\nTujuan perjalanan","PESAN\nDamar · 14 panggilan","JURNAL\nCatatan & dokumen","TAS\nInventori","SETELAN\nAudio & kontrol"};
            for(int i=0;i<6;i++)
            {
                int index=i;float x=.07f+(i%2)*.46f,y=.46f-(i/2)*.20f;
                var b=UiWidgets.Button("AppButton_"+i,page.transform,labels[i],new Vector2(x,y),new Vector2(x+.40f,y+.17f),()=>{if(index==5){GameShell.Instance?.ShowSettings();return;}Open(index==0?PhoneApp.Camera:index==1?PhoneApp.Map:index==2?PhoneApp.Messages:index==3?PhoneApp.Journal:PhoneApp.Inventory);});
                b.GetComponentInChildren<UnityEngine.UI.Text>().fontSize=20;
            }
        }
        private void Map()
        {
            UiWidgets.Text("District",page.transform,"PETA KOTA / RUTE MALAM",new Vector2(.07f,.75f),new Vector2(.93f,.87f),20);
            var map=UiWidgets.Panel("CityMap",page.transform,new Vector2(.05f,.40f),new Vector2(.95f,.74f),new Color(.09f,.12f,.13f));
            for(int i=0;i<6;i++)UiWidgets.Line("Road"+i,map.transform,new Vector2(-145+i*57,-100),new Vector2(-130+i*57,100),new Color(.21f,.25f,.25f),5);
            for(int i=0;i<4;i++)UiWidgets.Line("CrossRoad"+i,map.transform,new Vector2(-170,-85+i*53),new Vector2(170,-68+i*53),new Color(.21f,.25f,.25f),4);
            UiWidgets.Line("RouteA",map.transform,new Vector2(-105,-65),new Vector2(-30,-20),new Color(.62f,.35f,.23f),7);UiWidgets.Line("RouteB",map.transform,new Vector2(-30,-20),new Vector2(93,57),new Color(.62f,.35f,.23f),7);
            UiWidgets.Text("KosMarker",map.transform,"● KOS RAKA",new Vector2(.01f,.02f),new Vector2(.55f,.25f),18);UiWidgets.Text("AtmajaMarker",map.transform,"● ATMAJA",new Vector2(.44f,.70f),new Vector2(.99f,.96f),18);
            var corridor=FindFirstObjectByType<KosCorridorDirector>();bool ready=corridor&&corridor.CanDepart;
            var b=UiWidgets.Button("AtmajaDestination",page.transform,ready?"RUMAH KELUARGA ATMAJA\nPilih tujuan & berangkat":"RUMAH KELUARGA ATMAJA\nBerangkat dari gerbang kos",new Vector2(.05f,.26f),new Vector2(.95f,.38f),()=>{if(ready)corridor.RequestDeparture();});b.GetComponentInChildren<UnityEngine.UI.Text>().fontSize=21;
            b.interactable=ready;
            UiWidgets.Text("LockedDestinations",page.transform,"TERKUNCI: Griya Asri • RSUD • Pabrik\nKarang Setra • Pasar Setan\nTerbuka mengikuti progres cerita.",new Vector2(.07f,.10f),new Vector2(.93f,.25f),18);TravelSelection=ready;
        }
        public void OpenTravelMap(){Phone.isPhoneRaised=true;Phone.isScreenOn=Phone.batteryPercent>0;Phone.SyncVisualState();Open(PhoneApp.Map);}
        private void Messages()
        {
            UiWidgets.Text("DamarThread",page.transform,"DAMAR (KAMERAMEN)\n14 panggilan tak terjawab • 21:15\n\nVoice note terakhir tersimpan.",new Vector2(.07f,.48f),new Vector2(.93f,.83f),22);
            UiWidgets.Button("ReplayDamar",page.transform,"▶ PUTAR PESAN SUARA",new Vector2(.07f,.35f),new Vector2(.93f,.46f),()=>HudController.Instance.EnqueueSubtitle("Damar",KosDialogue.DamarVoiceNote,22));
            UiWidgets.Text("Contract",page.transform,"PENONTON_TERAKHIR\n"+(CampaignState.Instance.Data.wallet_bound>0?"Nyalakan live di Atmaja sebelum tengah malam.":"Belum ada pesan."),new Vector2(.07f,.12f),new Vector2(.93f,.33f),20);
        }
        private void Journal()
        {
            var c=CampaignState.Instance;var t=new StringBuilder("DOKUMEN DITEMUKAN\n\n");
            if(c.HasInventoryItem("DOC_PROLOG_01_SURAT_KOS"))t.AppendLine("01 • Surat peringatan kos\nTunggakan Rp3.500.000; besok 08:00.\n");
            if(c.HasInventoryItem(AtmajaDirector.SurveyId))t.AppendLine("02 • Survei Damar\nPintu depan dirantai. Lorong timur menuju jendela dapur.\n");
            if(c.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"))t.AppendLine("03 • Koran tragedi Atmaja, 1998\n");
            if(c.HasInventoryItem("EVENT_P0_AR_CLUE_READ"))t.AppendLine("Petunjuk AR: III : XLV\n");
            if(t.Length<30)t.Append("Belum ada dokumen. Periksa benda di sekitar.");
            UiWidgets.ScrollText("JournalEntries",page.transform,t.ToString(),new Vector2(.07f,.13f),new Vector2(.93f,.84f));
        }
        private void Inventory()
        {
            var c=CampaignState.Instance;var t=new StringBuilder("BARANG BAWAAN\n\n");
            foreach(var item in new[]{("SMARTPHONE_RAKA","Smartphone Raka"),("KUNCI_MOTOR","Kunci motor"),("JAKET_RAKA","Jaket"),("POWERBANK_KECIL_KOS","Powerbank kecil"),("MATABATIN_APK","Modul MataBatin"),("ITEM_P0_CLOCK_HANDS","Jarum kuningan"),("KUNCI_TANGGA_TENGAH","Kunci tangga")})if(c.HasInventoryItem(item.Item1))t.AppendLine("• "+item.Item2+"\n");
            t.Append($"\nDONASI\nSah Rp{c.Data.wallet_legitimate:N0}\nKontrak Rp{c.Data.wallet_bound:N0}");UiWidgets.ScrollText("InventoryEntries",page.transform,t.ToString(),new Vector2(.07f,.13f),new Vector2(.93f,.84f));
        }
        private void OnDestroy(){UiFocus.Phone=false;if(root)Destroy(root);}
    }
    public static class KosDialogue
    {
        public const string DamarVoiceNote="Ka... Raka, jangan ke Rumah Atmaja sendirian. Tolong dengerin gue. Aplikasi kamera yang gue oprek dari forum kemarin... itu bukan filter AR biasa, Ka. Rumah ini nggak kosong. Di lantai dua ada suara perempuan nyanyi tembang Jawa, tapi pas gue sorot pakai kamera HP, dia udah berdiri tepat di depan muka gue—";
        public const string RatnaWarning="Raka, ini peringatan terakhir dari saya. Tunggakan kos kamu sudah tiga bulan ditambah uang listrik totalnya tiga juta lima ratus ribu rupiah. Besok pagi jam delapan kalau belum ditransfer, gembok kamar saya ganti.";
        public const string Trapped="Hmm... kok tiba-tiba ketutup, ya? Kayaknya kita cuma bisa lanjut.";
    }
}
