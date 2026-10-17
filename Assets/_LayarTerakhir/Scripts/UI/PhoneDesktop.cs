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
        private bool dialing,pendingNotification;
        public bool IsCalling=>dialing||CallActive;
        private bool ContractReceived=>CampaignState.Instance&&(CampaignState.Instance.HasInventoryItem("EVENT_P0_CONTRACT_RECEIVED")||CampaignState.Instance.Data.wallet_bound>0||CampaignState.Instance.Data.spent>0);
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
            if(GameInputConfig.Instance&&GameInputConfig.Instance.GetActionDown(GameAction.OpenJournal)){Phone.isPhoneRaised=true;Phone.SyncVisualState();Open(PhoneApp.Journal);}
        }
        public void SetScreenVisible(bool on){if(root)root.SetActive(on&&CurrentApp!=PhoneApp.Camera);if(banner)banner.SetActive(on);if(on&&pendingNotification){pendingNotification=false;ShowNotification();}}
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
            var map=PhoneSkin.Card("CityMap",page.transform,new Vector2(.035f,.34f),new Vector2(.965f,.90f),new Color(.89f,.88f,.83f));map.AddComponent<RectMask2D>();
            var scroll=map.AddComponent<ScrollRect>();scroll.horizontal=scroll.vertical=true;scroll.scrollSensitivity=18;scroll.movementType=ScrollRect.MovementType.Clamped;
            var content=UiWidgets.Rect("NeighbourhoodMap",map.transform,Vector2.zero,Vector2.one).GetComponent<RectTransform>();
            content.gameObject.AddComponent<StoryMapGraphic>();scroll.viewport=map.GetComponent<RectTransform>();scroll.content=content;
            Canvas.ForceUpdateCanvases();content.sizeDelta=scroll.viewport.rect.size*.25f;
            Marker(content,"KosMarker",view.director?"Kos Raka":"Kos Raka • Kamu",new Vector2(.24f,.23f),new Color(.12f,.40f,.67f));Marker(content,"AtmajaMarker",view.director?"Atmaja • Kamu":"Rumah Atmaja",new Vector2(.68f,.76f),new Color(.64f,.20f,.16f));
            UiWidgets.Text("StreetName",content,"JL. MELATI",new Vector2(.02f,.46f),new Vector2(.40f,.51f),14).color=PhoneSkin.Ink;
            UiWidgets.Text("DistrictName",content,"KAMPUNG\nMELATI",new Vector2(.08f,.69f),new Vector2(.35f,.81f),18,TextAnchor.MiddleCenter).color=new Color(.35f,.38f,.34f);
            UiWidgets.Text("RiverName",content,"KALI\nCILIWUNG",new Vector2(.76f,.13f),new Vector2(.98f,.23f),13,TextAnchor.MiddleCenter).color=new Color(.23f,.46f,.53f);
            PhoneSkin.Button("MapZoomIn",map.transform,"+",new Vector2(.81f,.12f),new Vector2(.97f,.24f),()=>content.sizeDelta=Vector2.Min(scroll.viewport.rect.size*1.2f,content.sizeDelta+scroll.viewport.rect.size*.2f));
            PhoneSkin.Button("MapZoomOut",map.transform,"−",new Vector2(.81f,.005f),new Vector2(.97f,.12f),()=>content.sizeDelta=Vector2.Max(Vector2.zero,content.sizeDelta-scroll.viewport.rect.size*.2f));
            UiWidgets.Text("MapCredit",map.transform,"Peta cerita • luring • geser untuk menjelajah",new Vector2(.025f,.005f),new Vector2(.78f,.08f),12).color=PhoneSkin.Ink;
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
            var result=page.transform.Find("DialResult")?.GetComponent<Text>();if(result)result.text=contact+"\nMemanggil…";
            var source=gameObject.AddComponent<AudioSource>();source.outputAudioMixerGroup=PresentationProfile.Load().audio.uiBus;
            source.clip=Resources.Load<AudioClip>("KosAudio/OutgoingCall");source.volume=.45f;if(source.clip)source.Play();
            yield return new WaitForSeconds(3.2f);if(source){source.Stop();Destroy(source);}
            if(contact!="Damar"&&ContractReceived)
            {
                if(result)result.text=contact+"\nTersambung";
                string[] speakers=contact=="Ibu"?new[]{"Ibu","Raka","Ibu","Raka"}:new[]{"Bu Haji Ratna","Raka","Bu Haji Ratna","Raka"};
                string[] lines=contact=="Ibu"?new[]{KosDialogue.MotherHello,KosDialogue.RakaMother,KosDialogue.MotherReply,KosDialogue.RakaMotherGoodbye}:new[]{KosDialogue.RatnaHello,KosDialogue.RakaRatna,KosDialogue.RatnaReply,KosDialogue.RakaRatnaGoodbye};
                for(int i=0;i<lines.Length;i++)
                {
                    float duration=HudController.Instance.PlayPriorityDialogue(speakers[i],lines[i],Mathf.Clamp(lines[i].Length/17f,3,8));
                    yield return new WaitForSeconds(duration+.2f);
                }
                CampaignState.Instance.AddInventoryItem(contact=="Ibu"?"EVENT_CALLED_IBU":"EVENT_CALLED_RATNA");
                string checkpoint=CampaignState.Instance.Data.checkpoint_id;
                CampaignState.Instance.SaveCheckpointAtomic(checkpoint=="PROLOG_KOS_START"?"PROLOG_KOS_PREPARATION":checkpoint);
                if(result)result.text=contact+"\nPanggilan selesai • tagihan tersedia di Bank";
            }
            else if(result)result.text=contact+"\nTidak dapat dihubungi";
            dialing=false;
        }
        public void ReceiveContract(){CampaignState.Instance.AddInventoryItem("EVENT_P0_CONTRACT_RECEIVED");Open(PhoneApp.Home);Notify("Rp5.000.000 diterima");}
        public void Notify(string message)
        {
            latestMessage=message;pendingNotification=true;
            if(Phone.isPhoneRaised&&Phone.isScreenOn){pendingNotification=false;ShowNotification();}
        }
        private void ShowNotification()
        {
            if(banner)Destroy(banner);bannerUntil=Time.unscaledTime+6;banner=PhoneSkin.Card("PhoneNotification",view.Panel.transform,new Vector2(.055f,.765f),new Vector2(.945f,.915f),new Color(.13f,.20f,.27f,.99f));
            var button=banner.AddComponent<Button>();button.targetGraphic=banner.GetComponent<Image>();button.onClick.AddListener(()=>{Destroy(banner);Open(PhoneApp.Messages);});UiWidgets.Text("Preview",banner.transform,"PESAN MASUK\nPenonton_Terakhir",new Vector2(.06f,.10f),new Vector2(.94f,.93f),22);
        }
        private void Messages()
        {
            UiWidgets.Text("ThreadName",page.transform,"Penonton_Terakhir",new Vector2(.075f,.80f),new Vector2(.93f,.89f),27);UiWidgets.Text("MessageTime",page.transform,"Malam ini  •  23:46",new Vector2(.075f,.75f),new Vector2(.93f,.80f),17);
            var card=PhoneSkin.Card("ContractBubble",page.transform,new Vector2(.06f,.22f),new Vector2(.94f,.74f),new Color(.10f,.18f,.25f));bool received=ContractReceived;
            if(received&&Phone.isPhoneRaised&&Phone.isScreenOn)CampaignState.Instance.AddInventoryItem("EVENT_P0_CONTRACT_READ");
            string text=received?"Rp5.000.000 diterima\n\n"+KosDialogue.Contract:"Belum ada pesan.";if(!string.IsNullOrEmpty(latestMessage))text+="\n\n"+latestMessage;
            UiWidgets.ScrollText("Contract",card.transform,text,new Vector2(.06f,.03f),new Vector2(.94f,.97f),23);PhoneSkin.Button("OpenBank",page.transform,"Lihat mutasi di Bank  →",new Vector2(.06f,.115f),new Vector2(.94f,.20f),()=>Open(PhoneApp.Bank));
        }
        private void Bank()
        {
            var c=CampaignState.Instance;var card=PhoneSkin.Card("BalanceCard",page.transform,new Vector2(.06f,.58f),new Vector2(.94f,.87f),new Color(.12f,.24f,.43f));
            UiWidgets.Text("AccountHolder",card.transform,"RAKA WIJAYA\nTotal dana tercatat",new Vector2(.07f,.63f),new Vector2(.93f,.94f),22);UiWidgets.Text("BankBalance",card.transform,Money(c.Data.wallet_legitimate+c.Data.wallet_bound),new Vector2(.07f,.29f),new Vector2(.93f,.65f),40);UiWidgets.Text("BalanceDetails",card.transform,$"Sah {Money(c.Data.wallet_legitimate)}\nKontrak {Money(c.Data.wallet_bound)}",new Vector2(.07f,.04f),new Vector2(.93f,.29f),18);
            UiWidgets.Text("BillHeading",page.transform,"TAGIHAN KELUARGA",new Vector2(.07f,.51f),new Vector2(.93f,.57f),20);
            BillButton(true,.37f);BillButton(false,.23f);
            string history="Pengeluaran tercatat: "+Money(c.Data.spent);
            if(c.HasInventoryItem("EVENT_PAID_HOSPITAL"))history+="\n✓ RS Ibu: Rp1.500.000";
            if(c.HasInventoryItem("EVENT_PAID_RENT"))history+="\n✓ Kos: Rp3.500.000";
            UiWidgets.Text("BankTransactions",page.transform,history,new Vector2(.07f,.095f),new Vector2(.93f,.225f),18);
        }
        private void BillButton(bool hospital,float y)
        {
            var c=CampaignState.Instance;bool called=c.HasInventoryItem(hospital?"EVENT_CALLED_IBU":"EVENT_CALLED_RATNA");
            bool paid=c.HasInventoryItem(hospital?"EVENT_PAID_HOSPITAL":"EVENT_PAID_RENT");
            string label=hospital?"RS Ibu • Rp1.500.000":"Tunggakan kos • Rp3.500.000";
            var button=PhoneSkin.Button("Pay_"+(hospital?"Hospital":"Rent"),page.transform,label+"\n"+(paid?"LUNAS":called?"Bayar tagihan  →":"Telepon "+(hospital?"Ibu":"Bu Haji Ratna")+" dulu"),new Vector2(.06f,y),new Vector2(.94f,y+.125f),()=>ConfirmBill(hospital),PhoneSkin.Blue);
            button.interactable=called&&!paid;
        }
        private void ConfirmBill(bool hospital)
        {
            var confirm=PhoneSkin.Card("TransferConfirmation",page.transform,new Vector2(.035f,.12f),new Vector2(.965f,.88f),PhoneSkin.Ink);
            UiWidgets.Text("Details",confirm.transform,"KONFIRMASI PEMBAYARAN\n\n"+(hospital?"Biaya perawatan Ibu\nRp1.500.000":"Bu Haji Ratna • Tunggakan kos\nRp3.500.000")+"\n\nDana akan dikurangi satu kali. Simpan bukti transfer di aplikasi Bank.",new Vector2(.07f,.29f),new Vector2(.93f,.96f),25);
            PhoneSkin.Button("Confirm",confirm.transform,"Konfirmasi bayar",new Vector2(.06f,.16f),new Vector2(.94f,.28f),()=>
            {
                bool paid=CampaignState.Instance.TryPayFamilyBill(hospital);Open(PhoneApp.Bank);
                if(!paid)HudController.Instance.ShowBroadcastNotification("Transfer belum berhasil. Periksa saldo dan penyimpanan.",4);
                else HudController.Instance.EnqueueSubtitle("Raka",hospital?"Udah terkirim, Bu. Semoga besok Ibu bisa ditangani.":"Lunas, Bu Haji. Setidaknya besok barang gue nggak dikeluarin.",4);
            },PhoneSkin.Blue);
            PhoneSkin.Button("Cancel",confirm.transform,"Batal",new Vector2(.06f,.02f),new Vector2(.94f,.13f),()=>Destroy(confirm));
        }
        private void Journal()
        {
            var c=CampaignState.Instance;var entries=new System.Collections.Generic.List<(string title,string body)>();
            entries.Add(("Tujuan saat ini",HudController.Instance.CurrentObjective));
            if(c.HasInventoryItem("DOC_PROLOG_01_SURAT_KOS"))entries.Add(("Surat peringatan kos","Tunggakan Rp3.500.000. Bu Haji Ratna meminta kepastian sebelum besok pukul 08:00."));
            if(ContractReceived)entries.Add(("Pesan Penonton_Terakhir",KosDialogue.Contract));
            if(c.HasInventoryItem("DIARY_RAKA"))entries.Add(("Buku harian Raka",KosDialogue.Diary));
            if(c.HasInventoryItem(AtmajaDirector.SurveyId))entries.Add(("Catatan survei Damar",AtmajaDirector.SurveyText));
            if(c.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"))entries.Add(("Koran tragedi 1998","Satu keluarga ditemukan tewas. Polisi menduga peristiwa bermula di lantai dua menjelang tengah malam. Ada sesuatu yang sengaja tidak ditulis di koran ini."));
            if(c.HasInventoryItem("EVENT_P0_AR_CLUE_READ"))entries.Add(("Tulisan di dinding","WAKTU KEMATIAN KAMI ADALAH KUNCI.\nIII : XLV"));
            var host=UiWidgets.Panel("JournalList",page.transform,new Vector2(.05f,.12f),new Vector2(.95f,.88f),new Color(0,0,0,.01f));
            var scroll=host.AddComponent<ScrollRect>();scroll.horizontal=false;scroll.scrollSensitivity=30;scroll.movementType=ScrollRect.MovementType.Clamped;
            var viewport=UiWidgets.Rect("Viewport",host.transform,Vector2.zero,Vector2.one).GetComponent<RectTransform>();viewport.gameObject.AddComponent<RectMask2D>();
            var content=UiWidgets.Rect("Entries",viewport, new Vector2(0,1),Vector2.one).GetComponent<RectTransform>();content.pivot=new Vector2(.5f,1);content.sizeDelta=new Vector2(0,entries.Count*98);scroll.viewport=viewport;scroll.content=content;
            for(int i=0;i<entries.Count;i++)
            {
                var entry=entries[i];float top=1-i/(float)entries.Count,bottom=1-(i+1)/(float)entries.Count;
                PhoneSkin.Button("JournalEntry_"+i,content,entry.title+"   ›",new Vector2(.015f,bottom+.012f),new Vector2(.985f,top-.012f),()=>JournalDetail(entry.title,entry.body));
            }
        }
        private void JournalDetail(string title,string body)
        {
            var sheet=PhoneSkin.Card("JournalDetail",page.transform,new Vector2(.025f,.095f),new Vector2(.975f,.90f),PhoneSkin.Ink);
            UiWidgets.Text("EntryTitle",sheet.transform,title,new Vector2(.06f,.84f),new Vector2(.94f,.98f),29);
            UiWidgets.ScrollText("EntryBody",sheet.transform,body,new Vector2(.06f,.14f),new Vector2(.94f,.82f),23);
            PhoneSkin.Button("BackToJournal",sheet.transform,"‹  Semua catatan",new Vector2(.05f,.015f),new Vector2(.95f,.115f),()=>Destroy(sheet));
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
        public const string MotherHello="Nak... ada apa nelepon Ibu malam-malam? Ibu belum tidur, tadi batuknya kambuh lagi.";
        public const string RakaMother="Bu! Raka dapet uang buat biaya rumah sakit Ibu! Besok Ibu bisa lanjut diperiksa. Raka transfer malam ini, ya.";
        public const string MotherReply="Alhamdulillah... makasih, Nak. Tapi kamu jangan memaksakan diri. Kerjanya yang aman, Ibu cuma pengin kamu pulang sehat.";
        public const string RakaMotherGoodbye="Iya, Bu. Doain Raka. Sekarang Ibu istirahat dulu, nanti Raka kabarin lagi.";
        public const string RatnaHello="Halo, Raka? Ada apa malam-malam? Soal kamar itu besok pagi Ibu sudah harus kasih kepastian.";
        public const string RakaRatna="Iya, Bu Haji. Maaf ganggu. Uangnya sudah ada, tiga setengah juta. Saya bayar lewat transfer malam ini. Tolong jangan keluarin barang saya besok.";
        public const string RatnaReply="Ya sudah, Raka. Kalau masuk malam ini, kamarmu tetap aman. Habis transfer simpan buktinya. Lain kali jangan menunggu sampai ditegur, ya.";
        public const string RakaRatnaGoodbye="Iya, Bu. Terima kasih sudah ngasih waktu. Saya transfer sekarang.";
        public const string DamarVoiceNote="Ka... Raka, jangan ke Rumah Atmaja sendirian. Tolong dengerin gue. Aplikasi kamera yang gue oprek dari forum kemarin... itu bukan filter AR biasa, Ka. Rumah ini nggak kosong. Di lantai dua ada suara perempuan nyanyi tembang Jawa, tapi pas gue sorot pakai kamera HP, dia udah berdiri tepat di depan muka gue—";
        public const string DamarScream="Raka! Tolong! Aaaah!";
        public const string ContractReaction="Anjir, Damar dalam bahaya! Duit ini nggak gratis... Untuk sementara gue harus ikutin perintah dari pesan ini.";
        public const string Diary="32 penonton. Tunggakan kos tiga bulan. Besok harus cari cara buat biaya rumah sakit Ibu. Damar belum bisa dihubungi.";
        public const string Contract="Uang muka untuk kosmu dan biaya RS ibumu. Kalau mau temanmu tetap bernapas dan sisa Rp495.000.000 cair, nyalakan live di Rumah Keluarga Atmaja sebelum jam 12 malam. Waktu jalan terus.";
        public const string Trapped="Hmm... kok tiba-tiba ketutup, ya? Kayaknya kita cuma bisa lanjut.";
    }
}
