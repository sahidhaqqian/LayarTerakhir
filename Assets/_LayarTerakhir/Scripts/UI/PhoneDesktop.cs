using System.Text;
using System.Globalization;
using LayarTerakhir.Core;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace LayarTerakhir.UI
{
    public enum PhoneApp { Home,Camera,Map,Messages,Journal,Inventory,Bank,Call }
    public sealed class PhoneDesktop:MonoBehaviour
    {
        public PhoneApp CurrentApp {get;private set;}
        public bool TravelSelection {get;private set;}
        public bool CallActive {get;private set;}
        private AtmajaPhoneView view;private GameObject root,page,banner;private bool lastAR;
        private float callStarted,bannerUntil;private Text callTimer;private string latestMessage;
        public string DialedContact {get;private set;}
        public static readonly string[] Contacts={"Damar", "Ibu", "Bu Haji Ratna"};
        private static readonly string[] ContactNumbers={"08•• •••• 2140", "08•• •••• 5729", "08•• •••• 0800"};
        private bool dialing;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private static string Money(long amount)=>"Rp"+amount.ToString("N0",CultureInfo.GetCultureInfo("id-ID"));
        public void Initialize(AtmajaPhoneView owner){view=owner;root=UiWidgets.Rect("PhoneApps",view.Panel.transform,new Vector2(.045f,.09f),new Vector2(.955f,.91f));Open(PhoneApp.Home);}
        private void Update()
        {
            if(banner&&Time.unscaledTime>bannerUntil){Destroy(banner);banner=null;}
            if(callTimer&&CallActive){int seconds=Mathf.FloorToInt(Time.time-callStarted);callTimer.text=$"{seconds/60:00}:{seconds%60:00}  •  Tersambung";}
            if(!Phone||!Phone.hasSmartphoneInInventory||!root||UiFocus.Cinematic||HudController.Instance.IsModalOpen)return;
            if(Keyboard.current!=null)
            {
                if(Keyboard.current.zKey.wasPressedThisFrame&&Phone.isPhoneRaised)Open(CurrentApp==PhoneApp.Home?PhoneApp.Camera:PhoneApp.Home);
                if(Keyboard.current.mKey.wasPressedThisFrame){Phone.isPhoneRaised=true;Open(PhoneApp.Map);Phone.SyncVisualState();}
            }
            bool ar=Phone.currentScanMode==ScanVisualMode.AR;if(ar&&!lastAR&&Phone.isPhoneRaised)Open(PhoneApp.Camera);lastAR=ar;
        }
        public void SetScreenVisible(bool on){if(root)root.SetActive(on&&CurrentApp!=PhoneApp.Camera);if(banner)banner.SetActive(on);}
        public void Stow(){Phone.isPhoneRaised=false;Phone.SyncVisualState();UiFocus.Phone=false;}
        public void Open(PhoneApp app)
        {
            CurrentApp=app;TravelSelection=false;if(page){page.SetActive(false);Destroy(page);}if(!root)return;
            root.SetActive(app!=PhoneApp.Camera);page=PhoneSkin.Card("App_"+app,root.transform,Vector2.zero,Vector2.one,app==PhoneApp.Home?new Color(.045f,.09f,.14f):PhoneSkin.Ink);
            if(app==PhoneApp.Camera)return;if(app==PhoneApp.Home){Home();return;}if(app==PhoneApp.Call){CallScreen();return;}
            string title=app==PhoneApp.Map?"Peta":app==PhoneApp.Messages?"Pesan":app==PhoneApp.Bank?"Bank":app==PhoneApp.Journal?"Jurnal":"Tas";
            UiWidgets.Text("AppTitle",page.transform,title,new Vector2(.075f,.91f),new Vector2(.93f,.99f),34);
            PhoneSkin.Button("BackHome",page.transform,"‹  Aplikasi",new Vector2(.06f,.015f),new Vector2(.94f,.085f),()=>Open(PhoneApp.Home));
            if(app==PhoneApp.Map)Map();else if(app==PhoneApp.Messages)Messages();else if(app==PhoneApp.Journal)Journal();else if(app==PhoneApp.Inventory)Inventory();else if(app==PhoneApp.Bank)Bank();
        }
        private void Home()
        {
            UiWidgets.Text("Clock",page.transform,view.director?"23:52":"23:46",new Vector2(.08f,.77f),new Vector2(.92f,.94f),82,TextAnchor.MiddleCenter);
            UiWidgets.Text("Date",page.transform,"Kamis, 15 Oktober  •  Hujan",new Vector2(.08f,.72f),new Vector2(.92f,.78f),20,TextAnchor.MiddleCenter);
            var widget=PhoneSkin.Card("ChannelWidget",page.transform,new Vector2(.07f,.59f),new Vector2(.93f,.70f),new Color(.10f,.18f,.24f));
            UiWidgets.Text("ChannelWidgetText",widget.transform,"MALAM KERAMAT\n"+(Phone.isBroadcastConnected?"Siaran langsung berlangsung":"Kanal offline  •  32 penonton terakhir"),new Vector2(.07f,.06f),new Vector2(.93f,.94f),21);
            string[] labels={"Kamera","Peta","Pesan","Jurnal","Tas","Bank","Setelan","Telepon"};string[] glyphs={"Camera","Map","Messages","Journal","Inventory","Bank","Settings","Call"};
            Color[] colors={new Color(.28f,.33f,.42f),new Color(.16f,.43f,.41f),new Color(.18f,.46f,.68f),new Color(.61f,.43f,.27f),new Color(.34f,.38f,.29f),new Color(.24f,.37f,.65f),new Color(.35f,.38f,.44f),new Color(.24f,.51f,.36f)};
            for(int i=0;i<labels.Length;i++)
            {
                int index=i;float x=.065f+i%3*.315f,y=.40f-i/3*.17f;var slot=UiWidgets.Rect("AppSlot_"+i,page.transform,new Vector2(x,y),new Vector2(x+.245f,y+.16f));
                var b=PhoneSkin.Button("AppButton_"+i,slot.transform,"",new Vector2(.12f,.33f),new Vector2(.88f,1),()=>
                {if(index==6){GameShell.Instance?.ShowSettings();return;}Open(index==0?PhoneApp.Camera:index==1?PhoneApp.Map:index==2?PhoneApp.Messages:index==3?PhoneApp.Journal:index==4?PhoneApp.Inventory:index==5?PhoneApp.Bank:PhoneApp.Call);},colors[i]);
                PhoneSkin.Icon(glyphs[i],b.transform,new Vector2(.19f,.16f),new Vector2(.81f,.84f));UiWidgets.Text("AppLabel",slot.transform,labels[i],Vector2.zero,new Vector2(1,.31f),20,TextAnchor.MiddleCenter);
            }
        }
        private void Map()
        {
            var map=PhoneSkin.Card("CityMap",page.transform,new Vector2(.035f,.34f),new Vector2(.965f,.90f),new Color(.75f,.81f,.79f));map.AddComponent<Mask>().showMaskGraphic=true;
            var image=UiWidgets.Rect("JakartaMapImage",map.transform,Vector2.zero,Vector2.one).AddComponent<RawImage>();image.texture=Resources.Load<Texture2D>("Phone/JakartaStreetMap");image.raycastTarget=false;
            Canvas.ForceUpdateCanvases();var rect=map.GetComponent<RectTransform>().rect;
            float uvWidth=image.texture?rect.width/rect.height*image.texture.height/image.texture.width*.38f:.38f;image.uvRect=new Rect(.30f,.40f,uvWidth,.38f);
            Vector2[] route={new Vector2(.20f,.23f),new Vector2(.20f,.36f),new Vector2(.39f,.36f),new Vector2(.39f,.55f),new Vector2(.68f,.55f),new Vector2(.68f,.76f)};
            for(int i=1;i<route.Length;i++){var a=Vector2.Scale(route[i-1]-Vector2.one*.5f,rect.size);var b=Vector2.Scale(route[i]-Vector2.one*.5f,rect.size);UiWidgets.Line("RouteCasing",map.transform,a,b,Color.white,11);UiWidgets.Line("Route",map.transform,a,b,PhoneSkin.Blue,6);}
            Marker(map.transform,"KosMarker","Kos Raka",new Vector2(.20f,.23f),new Color(.12f,.40f,.67f));Marker(map.transform,"AtmajaMarker","Rumah Atmaja",new Vector2(.68f,.76f),new Color(.64f,.20f,.16f));
            UiWidgets.Text("MapCredit",map.transform,"Jakarta • Kirill Shrayber / CC0\nLokasi cerita & rute ilustratif",new Vector2(.025f,.005f),new Vector2(.97f,.10f),14).color=PhoneSkin.Ink;
            var corridor=FindFirstObjectByType<KosCorridorDirector>();bool ready=corridor&&corridor.CanDepart;
            UiWidgets.Text("DestinationLabel",page.transform,"RUMAH KELUARGA ATMAJA\n"+(ready?"Motor siap di gerbang kos":"Pilih keberangkatan di gerbang kos"),new Vector2(.07f,.24f),new Vector2(.93f,.33f),22);
            var button=PhoneSkin.Button("AtmajaDestination",page.transform,ready?"Berangkat ke Atmaja  →":"Pergi ke gerbang untuk berangkat",new Vector2(.06f,.145f),new Vector2(.94f,.23f),()=>{if(corridor&&corridor.CanDepart)corridor.RequestDeparture();},PhoneSkin.Blue);button.interactable=ready;
            var future=PhoneSkin.Button("LockedDestinations",page.transform,"5 tujuan berikutnya  ›",new Vector2(.06f,.09f),new Vector2(.94f,.14f),ShowFutureDestinations);future.GetComponentInChildren<Text>().fontSize=17;TravelSelection=ready;
        }
        private void ShowFutureDestinations()
        {
            var sheet=PhoneSkin.Card("FutureDestinations",page.transform,new Vector2(.035f,.095f),new Vector2(.965f,.90f),PhoneSkin.Ink);
            UiWidgets.Text("Title",sheet.transform,"TUJUAN PERJALANAN",new Vector2(.06f,.87f),new Vector2(.94f,.98f),26);
            string[] places={"Rusunawa Griya Asri","RSUD Lama Sido Waras","Pabrik Gula Sindang Laut","Desa Karang Setra","Pasar Setan"};
            for(int i=0;i<places.Length;i++){float y=.69f-i*.125f;var row=PhoneSkin.Card("LockedPlace"+i,sheet.transform,new Vector2(.04f,y),new Vector2(.96f,y+.11f),new Color(.09f,.13f,.18f));UiWidgets.Text("Name",row.transform,places[i]+"\nTERKUNCI",new Vector2(.06f,.05f),new Vector2(.94f,.95f),22);}
            PhoneSkin.Button("CloseDestinations",sheet.transform,"‹  Kembali ke peta",new Vector2(.05f,.02f),new Vector2(.95f,.105f),()=>Destroy(sheet));
        }
        private void Marker(Transform parent,string id,string label,Vector2 at,Color color)
        {
            var pin=PhoneSkin.Card(id,parent,at-new Vector2(.055f,.045f),at+new Vector2(.055f,.045f),color);PhoneSkin.Icon("Home",pin.transform,new Vector2(.12f,.12f),new Vector2(.88f,.88f));
            var card=PhoneSkin.Card(id+"Label",parent,at+new Vector2(-.15f,.05f),at+new Vector2(.25f,.12f),new Color(.035f,.06f,.08f,.95f));UiWidgets.Text("Name",card.transform,label,new Vector2(.05f,0),new Vector2(.95f,1),18,TextAnchor.MiddleCenter);
        }
        public void OpenTravelMap(){Phone.isPhoneRaised=true;Phone.isScreenOn=Phone.batteryPercent>0;Phone.SyncVisualState();Open(PhoneApp.Map);}
        public void BeginDamarCall(){CallActive=true;callStarted=Time.time;Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.SyncVisualState();Open(PhoneApp.Call);}
        public void EndDamarCall(){CallActive=false;Open(PhoneApp.Call);}
        private void CallScreen()
        {
            if(!CallActive){ContactsScreen();return;}
            UiWidgets.Text("CallNetwork",page.transform,CallActive?"PANGGILAN SUARA":"TELEPON",new Vector2(.08f,.85f),new Vector2(.92f,.94f),20,TextAnchor.MiddleCenter);
            var avatar=PhoneSkin.Card("DamarAvatar",page.transform,new Vector2(.32f,.59f),new Vector2(.68f,.79f),new Color(.17f,.29f,.35f));UiWidgets.Text("Initial",avatar.transform,"D",Vector2.zero,Vector2.one,76,TextAnchor.MiddleCenter);
            UiWidgets.Text("CallerName",page.transform,"Damar",new Vector2(.08f,.46f),new Vector2(.92f,.57f),48,TextAnchor.MiddleCenter);UiWidgets.Text("CallerRole",page.transform,"Kameramen",new Vector2(.08f,.42f),new Vector2(.92f,.48f),21,TextAnchor.MiddleCenter);
            callTimer=UiWidgets.Text("CallTimer",page.transform,CallActive?"00:00  •  Tersambung":"Panggilan terputus",new Vector2(.08f,.33f),new Vector2(.92f,.40f),23,TextAnchor.MiddleCenter);
            PhoneSkin.Icon("Call",page.transform,new Vector2(.39f,.15f),new Vector2(.61f,.27f),CallActive?new Color(.48f,.8f,.65f):new Color(.85f,.3f,.27f));UiWidgets.Text("CallHistory",page.transform,"14 panggilan tak terjawab  •  21:15",new Vector2(.07f,.06f),new Vector2(.93f,.13f),19,TextAnchor.MiddleCenter);
        }
        private void ContactsScreen()
        {
            UiWidgets.Text("ContactsTitle",page.transform,"Telepon",new Vector2(.075f,.88f),new Vector2(.93f,.98f),34);
            UiWidgets.Text("MissedCallHistory",page.transform,"Damar · 14 panggilan tak terjawab\nMalam ini, 21:15",new Vector2(.075f,.72f),new Vector2(.93f,.86f),21);
            for(int i=0;i<Contacts.Length;i++)
            {
                string contact=Contacts[i];float y=.54f-i*.15f;
                string detail=ContactNumbers[i];
                PhoneSkin.Button("Contact_"+i,page.transform,contact+"  ·  "+detail+"   ↗",new Vector2(.06f,y),new Vector2(.94f,y+.125f),()=>StartCoroutine(DialContact(contact)));
            }
            PhoneSkin.Button("ContactsHome",page.transform,"‹  Aplikasi",new Vector2(.06f,.015f),new Vector2(.94f,.085f),()=>Open(PhoneApp.Home));
            if(!string.IsNullOrEmpty(DialedContact))UiWidgets.Text("DialResult",page.transform,DialedContact+"\nTidak dapat dihubungi",new Vector2(.075f,.11f),new Vector2(.93f,.23f),22);
        }
        private System.Collections.IEnumerator DialContact(string contact)
        {
            if(dialing||CallActive)yield break;dialing=true;
            DialedContact=contact;Open(PhoneApp.Call);
            var result=GameObject.Find("DialResult").GetComponent<Text>();result.text=contact+"\nMemanggil…";
            var source=gameObject.AddComponent<AudioSource>();source.outputAudioMixerGroup=PresentationProfile.Load().audio.uiBus;
            source.clip=Resources.Load<AudioClip>("KosAudio/OutgoingCall");source.volume=.45f;if(source.clip)source.Play();
            yield return new WaitForSeconds(3.2f);if(source){source.Stop();Destroy(source);}
            dialing=false;
            if(CurrentApp==PhoneApp.Call&&!CallActive)Open(PhoneApp.Call);
        }
        public void ReceiveContract(){Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.SyncVisualState();Open(PhoneApp.Messages);Notify("Rp5.000.000 diterima");}
        public void Notify(string message)
        {
            latestMessage=message;if(banner)Destroy(banner);bannerUntil=Time.unscaledTime+9;banner=PhoneSkin.Card("PhoneNotification",view.Panel.transform,new Vector2(.07f,.77f),new Vector2(.93f,.91f),new Color(.13f,.20f,.27f,.99f));
            var button=banner.AddComponent<Button>();button.targetGraphic=banner.GetComponent<Image>();button.onClick.AddListener(()=>{Destroy(banner);Open(PhoneApp.Messages);});UiWidgets.Text("Preview",banner.transform,"PESAN MASUK\nPenonton_Terakhir",new Vector2(.06f,.10f),new Vector2(.94f,.93f),22);
        }
        private void Messages()
        {
            UiWidgets.Text("ThreadName",page.transform,"Penonton_Terakhir",new Vector2(.075f,.80f),new Vector2(.93f,.89f),27);UiWidgets.Text("MessageTime",page.transform,"Malam ini  •  23:46",new Vector2(.075f,.75f),new Vector2(.93f,.80f),17);
            var card=PhoneSkin.Card("ContractBubble",page.transform,new Vector2(.06f,.22f),new Vector2(.94f,.74f),new Color(.10f,.18f,.25f));bool received=CampaignState.Instance&&CampaignState.Instance.Data.wallet_bound>0;
            string text=received?"Rp5.000.000 diterima\n\n"+KosDialogue.Contract:"Belum ada pesan.";if(!string.IsNullOrEmpty(latestMessage))text+="\n\n"+latestMessage;
            UiWidgets.ScrollText("Contract",card.transform,text,new Vector2(.06f,.03f),new Vector2(.94f,.97f),23);PhoneSkin.Button("OpenBank",page.transform,"Lihat mutasi di Bank  →",new Vector2(.06f,.115f),new Vector2(.94f,.20f),()=>Open(PhoneApp.Bank));
        }
        private void Bank()
        {
            var c=CampaignState.Instance;var card=PhoneSkin.Card("BalanceCard",page.transform,new Vector2(.06f,.58f),new Vector2(.94f,.87f),new Color(.12f,.24f,.43f));
            UiWidgets.Text("AccountHolder",card.transform,"RAKA WIJAYA\nTotal dana tercatat",new Vector2(.07f,.63f),new Vector2(.93f,.94f),22);UiWidgets.Text("BankBalance",card.transform,Money(c.Data.wallet_legitimate+c.Data.wallet_bound),new Vector2(.07f,.29f),new Vector2(.93f,.65f),40);UiWidgets.Text("BalanceDetails",card.transform,$"Sah {Money(c.Data.wallet_legitimate)}\nKontrak {Money(c.Data.wallet_bound)}",new Vector2(.07f,.04f),new Vector2(.93f,.29f),18);
            var t=new StringBuilder("RINCIAN DANA\n\n");if(c.Data.wallet_bound>0)t.AppendLine($"↓ Penonton_Terakhir\n  {Money(c.Data.wallet_bound)}\n  Total dana terikat kontrak\n");if(c.Data.wallet_legitimate>0)t.AppendLine($"Dana sah\n{Money(c.Data.wallet_legitimate)}\n");t.Append($"Total pengeluaran: {Money(c.Data.spent)}");UiWidgets.ScrollText("BankTransactions",page.transform,t.ToString(),new Vector2(.075f,.12f),new Vector2(.93f,.54f),22);
        }
        private void Journal()
        {
            var c=CampaignState.Instance;var t=new StringBuilder("DOKUMEN DITEMUKAN\n\n");if(c.HasInventoryItem("DOC_PROLOG_01_SURAT_KOS"))t.AppendLine("01 • Surat peringatan kos\nTunggakan Rp3.500.000; besok 08:00.\n");if(c.HasInventoryItem("DIARY_RAKA"))t.AppendLine("Buku harian Raka\nCatatan pribadi dari laci meja.\n"+KosDialogue.Diary+"\n");if(c.HasInventoryItem(AtmajaDirector.SurveyId))t.AppendLine("02 • Survei Damar\nPintu depan dirantai. Lorong timur menuju jendela dapur.\n");if(c.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"))t.AppendLine("03 • Koran tragedi Atmaja, 1998\n");if(c.HasInventoryItem("EVENT_P0_AR_CLUE_READ"))t.AppendLine("Petunjuk AR: III : XLV\n");if(t.Length<30)t.Append("Belum ada dokumen. Periksa benda di sekitar.");UiWidgets.ScrollText("JournalEntries",page.transform,t.ToString(),new Vector2(.075f,.12f),new Vector2(.93f,.87f));
        }
        private void Inventory()
        {
            var c=CampaignState.Instance;var t=new StringBuilder("BARANG BAWAAN\n\n");foreach(var item in new[]{("SMARTPHONE_RAKA","Smartphone Raka"),("KUNCI_MOTOR","Kunci motor"),("JAKET_RAKA","Jaket"),("POWERBANK_KECIL_KOS","Powerbank kecil"),("DOMPET_RAKA","Dompet Raka"),("DIARY_RAKA","Buku harian Raka"),("MATABATIN_APK","Modul MataBatin"),("ITEM_P0_CLOCK_HANDS","Jarum kuningan"),("KUNCI_TANGGA_TENGAH","Kunci tangga")})if(c.HasInventoryItem(item.Item1))t.AppendLine("• "+item.Item2+"\n");UiWidgets.ScrollText("InventoryEntries",page.transform,t.ToString(),new Vector2(.075f,.12f),new Vector2(.93f,.87f));
            if(c.HasInventoryItem("DIARY_RAKA"))PhoneSkin.Button("ReadDiary",page.transform,"Baca buku harian  →",new Vector2(.06f,.13f),new Vector2(.94f,.22f),()=>Open(PhoneApp.Journal));
        }
        private void OnDestroy(){UiFocus.Phone=false;if(root)Destroy(root);}
    }
    public static class KosDialogue
    {
        public const string DamarVoiceNote="Ka... Raka, jangan ke Rumah Atmaja sendirian. Tolong dengerin gue. Aplikasi kamera yang gue oprek dari forum kemarin... itu bukan filter AR biasa, Ka. Rumah ini nggak kosong. Di lantai dua ada suara perempuan nyanyi tembang Jawa, tapi pas gue sorot pakai kamera HP, dia udah berdiri tepat di depan muka gue—";
        public const string DamarScream="Raka! Tolong! Aaaah!";
        public const string ContractReaction="Anjir, Damar dalam bahaya! Duit ini nggak gratis... Untuk sementara gue harus ikutin perintah dari pesan ini.";
        public const string Diary="32 penonton. Tunggakan kos tiga bulan. Besok harus cari cara buat biaya rumah sakit Ibu. Damar belum bisa dihubungi.";
        public const string Contract="Uang muka untuk kosmu dan biaya RS ibumu. Kalau mau temanmu tetap bernapas dan sisa Rp495.000.000 cair, nyalakan live di Rumah Keluarga Atmaja sebelum jam 12 malam. Waktu jalan terus.";
        public const string Trapped="Hmm... kok tiba-tiba ketutup, ya? Kayaknya kita cuma bisa lanjut.";
    }
}
