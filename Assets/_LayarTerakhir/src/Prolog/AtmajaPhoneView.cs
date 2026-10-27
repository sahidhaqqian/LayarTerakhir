using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
namespace LayarTerakhir.Prolog
{
    public sealed class AtmajaPhoneView:MonoBehaviour
    {
        public AtmajaDirector director;public Camera playerCamera;public Material feedMaterial;
        public const int ARLayer=29;
        private const int GripLayer=ShoulderCameraRig.PhoneGripLayer;
        public Camera FeedCamera {get;private set;}
        public Button LiveButton {get;private set;}
        public GameObject Panel {get;private set;}
        public PhoneDesktop Apps {get;private set;}
        private Button homeButton,stowButton;private CanvasGroup handsetInput;
        private Text stowLabel;private KeyCode lastStowBinding=(KeyCode)(-1);
        public bool HasGraphicsDevice=>SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null;
        public bool IsCameraApp=>Apps&&Apps.CurrentApp==PhoneApp.Camera;
        public bool CinematicHandsetVisible {get;set;}
        public bool ChatFrozen {get;set;}
        private Text header,mode,chat,walkHint;private GameObject cameraRoot;private RawImage cameraImage;private RenderTexture feed;private Material runtimeFeedMaterial;
        private readonly Queue<string> messages=new Queue<string>();
        private readonly Queue<string> pendingMessages=new Queue<string>();
        private bool threatSettled;
        private readonly System.Random chatRandom=new System.Random();
        private float nextChat,nextAmbient,flashUntil,glitchUntil;private string currentRoom="";
        private Image shutter;private AudioSource shutterAudio;private AudioClip shutterClip,switchClip;
        private RawImage focusVignette;private Image thumb;private Texture2D vignetteTexture;
        private Vector2 panelMin,panelMax,cameraMin,cameraMax;
        private bool focusOverride,lastSelfie,cameraHeldRaise;private ScanVisualMode lastMode;private float thumbUntil;
        private bool phoneRaisedBeforeFocus, suppressHeldUntilRelease;
        private PhoneApp appBeforeFocus;
        private Image[] signalBars;
        private FirstPersonController owner;
        private HudPresentation hudPresentation;
        private RectTransform handsetRect;
        private float raiseAmount,viewfinderBlend;
        // A stowed handset rises and lowers in its aiming pose. It must not
        // first appear as a portrait app panel and then stretch across the eye.
        private bool landscapeFromStowed;
        public const float LandscapeHandsetWidth=.165f;
        public const float LandscapeHandsetAspect=1.9f;
        private const float HandPresentationFieldOfView=28f;
        private Text chapterReticle,chapterReadout;private bool chapterTargetVisible;private Vector3 chapterTargetPoint;
        private GameObject firstPersonHand,supportHand;
        private Renderer[] handRenderers,supportRenderers;
        private Vector3 handScale,supportScale;
        private readonly List<Material> handMaterials=new List<Material>();
        private Camera gripCamera;
        private RenderTexture gripTexture;
        private RawImage gripOverlay;
        private GameObject gripOccluder;
        private Renderer[] portraitRenderers;
        private Material gripDepthMaterial;
        private Vector3 depthMaskScale;
        public bool FocusHandsVisible=>firstPersonHand&&supportHand&&firstPersonHand.activeSelf&&supportHand.activeSelf;
        private readonly Vector3[] panelCorners = new Vector3[4];
        public float RaiseAmount => raiseAmount;
        public float LastChatTime {get;private set;}
        public int DisplayedChatCount {get;private set;}
        public float GlitchStrength {get;set;}
        public bool ViewfinderActive {get;private set;}
        public void SetViewfinder(bool on)=>focusOverride=on;
        public bool FramesTarget(GameObject target,float range)
        {
            if(!FeedCamera||!target||!target.activeInHierarchy||!Phone||!Phone.HasCameraPower)return false;
            Vector3 point=target.transform.position+Vector3.up*1.45f;
            Vector3 viewport=FeedCamera.WorldToViewportPoint(point);
            Vector3 delta=point-FeedCamera.transform.position;
            if(viewport.z<=FeedCamera.nearClipPlane||delta.magnitude>range||viewport.x<.06f||viewport.x>.94f||viewport.y<.06f||viewport.y>.94f)return false;
            foreach(var hit in Physics.RaycastAll(FeedCamera.transform.position,delta.normalized,delta.magnitude,~((1<<29)|(1<<2)),QueryTriggerInteraction.Ignore))
            {
                if(hit.transform.IsChildOf(target.transform)||(owner&&hit.transform.IsChildOf(owner.transform)))continue;
                return false;
            }
            return true;
        }
        private void PresentFeed(bool focus)
        {
            int width=focus?Mathf.Clamp(Screen.width,640,960):256;
            int height=focus?Mathf.Max(1,Mathf.RoundToInt(width*9f/16)):448;
            if(feed.width!=width||feed.height!=height)
            {
                FeedCamera.targetTexture=null;cameraImage.texture=null;
                feed.Release();Destroy(feed);
                feed=new RenderTexture(width,height,16,RenderTextureFormat.ARGB32){name="RakaPhoneFeed",antiAliasing=1};
                if(HasGraphicsDevice)feed.Create();
                FeedCamera.targetTexture=feed;cameraImage.texture=feed;
            }
            FeedCamera.aspect=(float)width/height;
        }
        private HudController Hud=>HudController.Instance;private SmartphoneController Phone=>SmartphoneController.Instance;
        private IPhoneChapterContent Chapter=>Phone?Phone.ChapterContent:null;
        public string ClockText=>Chapter is LayarTerakhir.Phase5.Phase5Director?"--:--":Chapter is LayarTerakhir.Phase4.Phase4Hub?"16:45":Chapter is LayarTerakhir.Phase4.Phase4Director?"23:53":Chapter is LayarTerakhir.Phase3.Phase3Hub?"15:30":Chapter is LayarTerakhir.Phase3.Phase3Director?"23:54":Chapter is LayarTerakhir.Phase2.Phase2Hub?"14:00":Chapter is LayarTerakhir.Phase2.Phase2Director?"23:50":Chapter!=null?"23:58":director?"23:52":"23:46";
        public string DateText=>Chapter is LayarTerakhir.Phase5.Phase5Director?"Malam yang sama • Pasar Setan":Chapter is LayarTerakhir.Phase4.Phase4Hub?"Sore • Persiapan Karang Setra":Chapter is LayarTerakhir.Phase4.Phase4Director?"Malam • Karang Setra":Chapter is LayarTerakhir.Phase3.Phase3Hub?"Siang • Persiapan Sindang Laut":Chapter is LayarTerakhir.Phase3.Phase3Director?"Malam • Sindang Laut":Chapter is LayarTerakhir.Phase2.Phase2Hub?"Siang • Persiapan malam kedua":Chapter is LayarTerakhir.Phase2.Phase2Director?"Malam kedua • RSUD Sido Waras":"Kamis, 15 Oktober  •  Hujan";
        private void Start()
        {
            if(!playerCamera){var p=FindFirstObjectByType<FirstPersonController>();if(p)playerCamera=p.GetComponent<InteractionInteractor>().playerCamera;}
            director=director?director:FindFirstObjectByType<AtmajaDirector>();
            owner=playerCamera.GetComponentInParent<FirstPersonController>();
            if(!feedMaterial){var profile=PresentationProfile.Load();if(profile)feedMaterial=profile.phoneFeedMaterial;}
            hudPresentation=Hud.GetComponent<HudPresentation>();UiWidgets.Font=hudPresentation.style.bodyFont;
            var canvas=Hud.GetComponentInChildren<Canvas>().transform;
            Panel=PhoneSkin.Card("RakaSmartphone",canvas,new Vector2(.772f,.045f),new Vector2(.973f,.758f),new Color(.035f,.04f,.045f));
            handsetInput=Panel.AddComponent<CanvasGroup>();
            handsetRect=Panel.GetComponent<RectTransform>();panelMin=handsetRect.anchorMin;panelMax=handsetRect.anchorMax;
            PhoneSkin.Card("GlassScreen",Panel.transform,new Vector2(.018f,.012f),new Vector2(.982f,.988f),new Color(.008f,.012f,.019f));
            PhoneSkin.Card("Earpiece",Panel.transform,new Vector2(.38f,.978f),new Vector2(.62f,.984f),new Color(.23f,.25f,.27f));
            PhoneSkin.Card("FrontLens",Panel.transform,new Vector2(.66f,.970f),new Vector2(.69f,.985f),new Color(.06f,.13f,.18f));
            PhoneSkin.Card("PowerButton",Panel.transform,new Vector2(1,.66f),new Vector2(1.008f,.78f),new Color(.25f,.28f,.3f));
            PhoneSkin.Card("VolumeRocker",Panel.transform,new Vector2(-.008f,.63f),new Vector2(0,.80f),new Color(.25f,.28f,.3f));
            header=UiWidgets.Text("PhoneLiveLabel",Panel.transform,"",new Vector2(.075f,.918f),new Vector2(.925f,.966f),20);
            cameraRoot=UiWidgets.Rect("CameraApp",Panel.transform,new Vector2(.045f,.16f),new Vector2(.955f,.91f));cameraImage=cameraRoot.AddComponent<RawImage>();cameraImage.raycastTarget=false;
            var cameraRect=cameraRoot.GetComponent<RectTransform>();cameraMin=cameraRect.anchorMin;cameraMax=cameraRect.anchorMax;
            chapterReticle=UiWidgets.Text("ChapterHeadReticle",cameraRoot.transform,"+",new Vector2(.5f,.5f),new Vector2(.5f,.5f),40,TextAnchor.MiddleCenter);
            chapterReticle.rectTransform.sizeDelta=new Vector2(60,60);chapterReticle.color=new Color(1,.18f,.12f);chapterReticle.raycastTarget=false;chapterReticle.gameObject.SetActive(false);
            runtimeFeedMaterial=new Material(feedMaterial);cameraImage.material=runtimeFeedMaterial;
            feed=new RenderTexture(256,448,16,RenderTextureFormat.ARGB32){name="RakaPhoneFeed",antiAliasing=1};if(HasGraphicsDevice)feed.Create();cameraImage.texture=feed;
            var cam=new GameObject("RakaPhoneCamera",typeof(Camera));cam.transform.SetParent(playerCamera.transform.root,false);FeedCamera=cam.GetComponent<Camera>();FeedCamera.CopyFrom(playerCamera);FeedCamera.targetTexture=feed;
            FeedCamera.depth=playerCamera.depth-1;FeedCamera.fieldOfView=70;FeedCamera.aspect=256f/448;FeedCamera.GetUniversalAdditionalCameraData().renderPostProcessing=false;FeedCamera.GetUniversalAdditionalCameraData().renderShadows=true;cam.tag="Untagged";
            playerCamera.cullingMask&=~((1<<ARLayer)|(1<<AtmajaHandycamPlayback.RecordingLayer)|(1<<AtmajaMirrorReflection.ReflectionLayer));FeedCamera.enabled=false;
            // Parenting keeps the physical lens current even when a photo is processed
            // during Update, before the presentation's LateUpdate.
            if(owner&&owner.cameraPivot){FeedCamera.transform.SetParent(owner.cameraPivot,false);FeedCamera.transform.localPosition=Vector3.forward*.08f;FeedCamera.transform.localRotation=Quaternion.identity;}
            var handPrefab=Resources.Load<GameObject>("RakaCameraGrip")??Resources.Load<GameObject>("RakaPhoneHand");
            if(handPrefab)
            {
                firstPersonHand=Instantiate(handPrefab,playerCamera.transform,false);
                firstPersonHand.name="RakaPhoneRightGrip";handScale=firstPersonHand.transform.localScale;
                handRenderers=firstPersonHand.GetComponentsInChildren<Renderer>(true);
                foreach(var part in firstPersonHand.GetComponentsInChildren<Transform>(true))part.gameObject.layer=GripLayer;
                foreach(var r in handRenderers){r.shadowCastingMode=ShadowCastingMode.Off;r.renderingLayerMask=ShoulderCameraRig.AvatarRenderingLayer;}
                firstPersonHand.SetActive(false);
                // Mirror the existing grasp by transform only; no authored/exported hand asset.
                supportHand=Instantiate(handPrefab,playerCamera.transform,false);supportHand.name="RakaPhoneLeftGrip";
                supportScale=Vector3.Scale(supportHand.transform.localScale,new Vector3(-1,1,1));supportHand.transform.localScale=supportScale;
                supportRenderers=supportHand.GetComponentsInChildren<Renderer>(true);
                foreach(var part in supportHand.GetComponentsInChildren<Transform>(true))part.gameObject.layer=GripLayer;
                foreach(var r in supportRenderers){r.shadowCastingMode=ShadowCastingMode.Off;r.renderingLayerMask=ShoulderCameraRig.AvatarRenderingLayer;}
                supportHand.SetActive(false);
                // The existing handset supplies depth so fingers behind its body
                // stay hidden, while front fingers can cover the UI bezel naturally.
                gripOccluder=Instantiate(Resources.Load<GameObject>("RakaPhoneHand"),playerCamera.transform,false);gripOccluder.name="ExistingHandsetDepthMask";
                depthMaskScale=gripOccluder.transform.localScale;
                portraitRenderers=gripOccluder.GetComponentsInChildren<Renderer>(true);
                gripDepthMaterial=new Material(Resources.Load<Shader>("PhoneGripDepth"));
                foreach(var part in gripOccluder.GetComponentsInChildren<Transform>(true))part.gameObject.layer=GripLayer;
                foreach(var renderer in portraitRenderers)
                {
                    renderer.renderingLayerMask=ShoulderCameraRig.AvatarRenderingLayer;
                    renderer.enabled=renderer.name.Contains("HandheldPhone");renderer.shadowCastingMode=ShadowCastingMode.Off;
                    if(renderer.enabled)renderer.sharedMaterial=gripDepthMaterial;
                }
                gripOccluder.SetActive(false);
                foreach(var renderer in handRenderers.Concat(supportRenderers))
                    if(renderer.name.Contains("VisibleSkin"))
                    {
                        var material=new Material(renderer.sharedMaterial);material.name="PhoneGripSurface_Runtime";
                        material.SetFloat("_Smoothness",.22f);material.SetFloat("_BumpScale",.45f);
                        if(material.HasProperty("_Metallic"))material.SetFloat("_Metallic",0);
                        material.DisableKeyword("_METALLICSPECGLOSSMAP");
                        renderer.sharedMaterial=material;handMaterials.Add(material);
                    }
                var gripView=new GameObject("RakaPhoneGripCamera",typeof(Camera));gripView.transform.SetParent(playerCamera.transform,false);
                gripCamera=gripView.GetComponent<Camera>();gripCamera.CopyFrom(playerCamera);gripCamera.cullingMask=1<<GripLayer;
                gripCamera.fieldOfView=HandPresentationFieldOfView;
                gripCamera.clearFlags=CameraClearFlags.SolidColor;gripCamera.backgroundColor=Color.clear;gripCamera.allowHDR=false;
                gripCamera.nearClipPlane=.035f;gripCamera.farClipPlane=1.5f;gripCamera.depth=playerCamera.depth-.2f;
                gripCamera.GetUniversalAdditionalCameraData().renderPostProcessing=false;
                gripCamera.GetUniversalAdditionalCameraData().renderShadows=false;
                int width=Mathf.Clamp(Screen.width,640,960),height=Mathf.RoundToInt(width*(float)Screen.height/Mathf.Max(1,Screen.width));
                gripTexture=new RenderTexture(width,height,16,RenderTextureFormat.ARGB32){name="ExistingRakaGripPresentation"};
                if(HasGraphicsDevice)gripTexture.Create();gripCamera.targetTexture=gripTexture;gripCamera.enabled=false;
                var overlay=UiWidgets.Rect("PhoneGripForeground",canvas,Vector2.zero,Vector2.one);
                gripOverlay=overlay.AddComponent<RawImage>();gripOverlay.texture=gripTexture;gripOverlay.raycastTarget=false;gripOverlay.enabled=false;
                playerCamera.cullingMask&=~(1<<GripLayer);
            }
            LiveButton=UiWidgets.Button("MulaiLiveButton",cameraRoot.transform,"MULAI LIVE",new Vector2(.10f,.36f),new Vector2(.90f,.51f),()=>{if(Chapter!=null)Chapter.StartLive();else if(director)director.StartLive();});
            mode=UiWidgets.Text("PhoneMode",Panel.transform,"",new Vector2(.06f,.09f),new Vector2(.94f,.16f),18);
            var home=PhoneSkin.Button("PhoneHome",Panel.transform,"APLIKASI  [Z]",new Vector2(.055f,.026f),new Vector2(.49f,.077f),()=>Apps.Open(PhoneApp.Home));home.GetComponentInChildren<Text>().fontSize=16;
            var stow=PhoneSkin.Button("PhoneStow",Panel.transform,"SIMPAN  [TAB]",new Vector2(.51f,.026f),new Vector2(.945f,.077f),()=>Apps.Stow());stow.GetComponentInChildren<Text>().fontSize=16;
            homeButton=home;stowButton=stow;
            stowLabel=stow.GetComponentInChildren<Text>();
            PhoneSkin.Card("GestureBar",Panel.transform,new Vector2(.37f,.014f),new Vector2(.63f,.019f),new Color(.55f,.57f,.6f));
            chat=UiWidgets.Text("AtmajaLiveChat",canvas,"",new Vector2(.035f,.17f),new Vector2(.32f,.43f),18);chat.color=new Color(.82f,.83f,.77f,.93f);
            walkHint=UiWidgets.Text("PhoneWalkHint",canvas,"WASD jalan · klik kanan angkat kamera · tahan untuk bidik",new Vector2(.42f,.008f),new Vector2(.97f,.04f),16,TextAnchor.MiddleRight);
            Apps=gameObject.AddComponent<PhoneDesktop>();Apps.Initialize(this);Panel.transform.SetAsFirstSibling();chat.transform.SetAsFirstSibling();
            if(gripOverlay)gripOverlay.transform.SetSiblingIndex(Panel.transform.GetSiblingIndex()+1);
            shutter=UiWidgets.Panel("PhoneShutterFlash",cameraRoot.transform,Vector2.zero,Vector2.one,Color.clear).GetComponent<Image>();shutter.raycastTarget=false;
            signalBars=new Image[8];for(int i=0;i<signalBars.Length;i++)
            {signalBars[i]=UiWidgets.Panel("HandsetSignalTear_"+i,Panel.transform,new Vector2(.045f,.15f+i*.09f),new Vector2(.955f,.151f+i*.09f),Color.clear).GetComponent<Image>();signalBars[i].raycastTarget=false;}
            vignetteTexture=CreateVignetteTexture();
            var vignette=UiWidgets.Rect("ViewfinderThreat",cameraRoot.transform,Vector2.zero,Vector2.one);
            focusVignette=vignette.AddComponent<RawImage>();focusVignette.texture=vignetteTexture;focusVignette.raycastTarget=false;focusVignette.color=new Color(1,1,1,0);
            var thumbObj=PhoneSkin.Card("PhoneThumb",Panel.transform,new Vector2(.82f,.055f),new Vector2(.945f,.145f),new Color(.60f,.42f,.28f,.94f));
            thumb=thumbObj.GetComponent<Image>();thumb.raycastTarget=false;thumb.gameObject.SetActive(false);
            if(Phone){lastMode=Phone.currentScanMode;lastSelfie=Phone.isSelfieCamera;}
            shutterAudio=gameObject.AddComponent<AudioSource>();shutterAudio.playOnAwake=false;
            var audio=PresentationProfile.Load()?.audio;if(audio)shutterAudio.outputAudioMixerGroup=audio.uiBus;
            if(Phone.handPhoneMeshRoot)foreach(var r in Phone.handPhoneMeshRoot.GetComponentsInChildren<Renderer>(true))r.enabled=false;
        }
        private void Update()
        {
            if(!Phone||!Apps||!Hud||UiFocus.HardModal||Hud.IsModalOpen)return;
            HandleCameraShortcut();
            if(!UiFocus.Cinematic&&Phone.isPhoneRaised&&Phone.isScreenOn&&IsCameraApp&&(!UiFocus.Phone||ViewfinderActive)&&(!director||!director.Busy&&director.Stage<AtmajaStage.Chase)
                &&GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz)){if(Chapter!=null)Chapter.Capture();else CaptureFlash();}
            if(!director||!Phone.isBroadcastConnected||ChatFrozen||UiFocus.Menu||UiFocus.Cinematic)return;
            bool resolved=director.chase&&director.chase.ThreatDefeated;
            if(resolved!=threatSettled)
            {
                threatSettled=resolved;
                // Do not drain an old panic backlog after the hunter vanished.
                pendingMessages.Clear();messages.Clear();chat.text="";
                nextChat=Time.time;nextAmbient=Time.time+4;
                if(resolved)AddChat("@maya_sari","Udah hilang... tarik napas dulu, Bang.");
            }
            var p=director.LocalPosition(director.player.transform.position);float y=director.player.transform.position.y-director.origin.y;
            string room=y<-.5f?"cellar":y>3.3f?(p.y>5.1f?"hall":p.x< -2.5f?"child":p.x<.25f?"bath":p.x<2.3f?"master":"balcony"):
                p.y>9?"kitchen":p.x>1.6f&&p.x<6.1f&&p.y>1?"dining":p.y>0&&p.x<1.6f?"lounge":"yard";
            bool danger=!resolved&&director.basement&&director.basement.IsHunting;
            bool chase=!resolved&&director.Stage>=AtmajaStage.Chase;
            bool upstairs=!danger&&!chase&&y>3.3f;
            float ambientDelay=resolved?4f+(float)chatRandom.NextDouble()*3f:danger?.55f+(float)chatRandom.NextDouble()*.65f:chase?.45f+(float)chatRandom.NextDouble()*.55f:upstairs?.9f+(float)chatRandom.NextDouble()*1.1f:1f+(float)chatRandom.NextDouble()*2f;
            if(room!=currentRoom||Time.time>=nextAmbient)
            {
                currentRoom=room;nextAmbient=Time.time+ambientDelay;
                string[] lines=resolved?ReliefComments:danger||chase?PanicComments(chase,room):RoomComments(room);
                int burst=resolved?1:danger?3:chase?3:upstairs?2:1+(chatRandom.NextDouble()>.55?1:0);
                int capacity=danger?16:chase?16:upstairs?12:8;
                for(int i=0;i<burst&&pendingMessages.Count<capacity;i++)AddChat(LiveAuthors[chatRandom.Next(LiveAuthors.Length)],lines[chatRandom.Next(lines.Length)]);
            }
            if(pendingMessages.Count>0&&Time.time>=nextChat)
            {
                int batch=danger||chase?2:1;
                for(int i=0;i<batch&&pendingMessages.Count>0;i++)messages.Enqueue(pendingMessages.Dequeue());
                while(messages.Count>(danger||chase?4:upstairs?4:3))messages.Dequeue();
                chat.text=string.Join("\n\n",messages);LastChatTime=Time.time;DisplayedChatCount++;
                nextChat=Time.time+(danger?.4f+(float)chatRandom.NextDouble()*.4f:chase?.3f+(float)chatRandom.NextDouble()*.35f:upstairs?1f+(float)chatRandom.NextDouble()*1.8f:1.2f+(float)chatRandom.NextDouble()*3.3f);
            }
        }
        private static readonly string[] LiveAuthors={"@kopi_susu99","@dinda_ayu","@intan_permata","@bocil_kematian","@rudi_tabuti","@maya_sari","@rizky_pratama_","@hantu_karang01"};
        private static readonly string[] ReliefComments={"Udah tenang sekarang...","Jalur keluarnya sudah kosong, Bang.","Kita masih di sini. Pelan-pelan aja.","Tarik napas dulu, tadi serem banget."};
        private static string[] PanicComments(bool chase,string room)
        {
            if(chase)return new[]{"LARI BANG JANGAN NENGOK!","DIA MASIH DI BELAKANG LU!","TANGGA! TANGGA! CEPET!","KAMERA JANGAN DITUTUP BANG!","BLITZ BANG KALO DIA DEKET!","JANGAN BERHENTI ANJIR!","BLITZ! BLITZ! BLITZ!"};
            return room switch
            {
                "cellar"=>new[]{"ITU GUNTINGNYA KEDENGERAN!","DIA LEWAT KAMAR JAHIT!","MATIIN LAYAR HP BANG!","SEMBUNYI DI LEMARI!","JANGAN NAFAS BANG!","ADA BAYANGAN DI BELAKANG RAK!"},
                "hall"=>new[]{"LANGKAHNYA DI ATAS BANG!","JANGAN LEWAT LORONG ITU!","ITU BAYANGAN APA?!","KAMERA NYA BERGETAR BANG!"},
                "master"=>new[]{"ADA YANG NUNGGU DI KAMAR!","JANGAN DEKETIN CERMIN!","TALI ITU GERAK SENDIRI BANG!"},
                "kitchen"=>new[]{"SUMURNYA BUNYI LAGI!","JANGAN LIAT KE BAWAH!","ADA BAYANGAN DI JENDELA!"},
                _=>new[]{"BANG DI BELAKANG LU!","LARI BANG!","ITU APA BARUSAN?!","KAMERA JANGAN DITUTUP!"}
            };
        }
        private static string[] RoomComments(string room)
        {
            switch(room)
            {
                case "child":return new[]{"Mainannya masih ada semua...","Itu kamar anaknya ya? Sedih banget lihatnya.","Aku denger sesuatu kayak anak kecil tadi.","Jangan sentuh mainan itu bang...","Bonekanya kayak baru dipindahin."};
                case "bath":return new[]{"Kok lantainya basah bang?","Di sini suaranya kayak menggema ya.","Airnya warnanya aneh, jangan disentuh bang.","Tirai itu tadi gerak sendiri kan?","Gue nggak berani lihat cerminnya."};
                case "hall":return new[]{"Lantai atas bikin merinding asli.","Bang pelan-pelan, langkah siapa itu?","Lukisan keluarganya serem juga dilihat malam-malam.","Jangan lewat kamar anak itu bang.","Kayaknya ada yang ngikutin dari ujung lorong."};
                case "master":return new[]{"Kamarnya gede tapi sunyi banget.","Ada suara kain diseret nggak sih?","Gue masih mantau bang, jangan panik.","Itu pintu kamar utama ketutup sendiri?","Wanginya kayak kapur barus banget."};
                case "balcony":return new[]{"Anginnya masuk mic juga bang.","Tuh dari balkon kelihatan halaman.","Aku nggak suka tali itu gerak sendiri.","Jangan deket-deket pagar bang.","Ada bayangan di ujung balkon nggak?"};
                case "cellar":return new[]{"Sinyalnya mulai pecah bang.","Ada yang nyanyi pelan banget...","Jahitannya banyak banget, ini dulu tempat usaha ya?"};
                case "kitchen":return new[]{"Piringnya udah berjamur semua astaga.","Tadi bunyi ember dari mana?","Gue merinding lihat sumurnya."};
                case "dining":return new[]{"Itu di piring... tikus?!","Kayak meja makan ditinggal pas orang-orangnya belum selesai makan.","Bang Damar ninggalin tasnya di situ kenapa ya?"};
                case "lounge":return new[]{"Jamnya masih bunyi padahal rumahnya kosong.","Sofanya dulu pasti bagus, sekarang lembap semua.","Bang ada yang nonton dari belakang nggak sih?"};
                default:return new[]{"Hujannya makin gede ya bang?","Yang baru masuk, ini lagi nyari Bang Damar.","Suaranya masih jelas di sini bang."};
            }
        }
        public void CaptureFlash()
        {
            Phone.PulseShutter();flashUntil=Time.unscaledTime+.22f;
            if(!shutterClip)shutterClip=AtmajaSynth.Shutter();if(shutterAudio)shutterAudio.PlayOneShot(shutterClip,.65f);
        }
        public void Glitch(float seconds,float strength=.8f){glitchUntil=Time.unscaledTime+seconds;GlitchStrength=strength;}
        private static bool LensFocusHeld()
        {
#if ENABLE_INPUT_SYSTEM
            return UnityEngine.InputSystem.Mouse.current!=null&&UnityEngine.InputSystem.Mouse.current.rightButton.isPressed;
#else
            return Input.GetMouseButton(1);
#endif
        }
        private static Texture2D CreateVignetteTexture()
        {
            const int size=128;var texture=new Texture2D(size,size,TextureFormat.RGBA32,false,true){name="PhoneThreatVignette",wrapMode=TextureWrapMode.Clamp,filterMode=FilterMode.Bilinear};
            var pixels=new Color32[size*size];
            for(int y=0;y<size;y++)for(int x=0;x<size;x++)
            {
                float px=(x+.5f)/size*2-1,py=(y+.5f)/size*2-1;
                float radius=Mathf.Sqrt(px*px+py*py);
                float edge=Mathf.SmoothStep(0f,1f,Mathf.InverseLerp(.42f,1.02f,radius));
                byte alpha=(byte)Mathf.RoundToInt(edge*edge*255);
                pixels[y*size+x]=new Color32(210,32,24,alpha);
            }
            texture.SetPixels32(pixels);texture.Apply(false,false);return texture;
        }
        private void HandleCameraShortcut()
        {
            if(UiFocus.Menu||UiFocus.HardModal||Hud.IsModalOpen||UiFocus.Cinematic||(director&&director.Busy))return;
            bool held=LensFocusHeld();
            // Screen-off / stow is explicit and must beat a held aiming shortcut.
            if (held && GameInputConfig.Instance &&
                (GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen) || GameInputConfig.Instance.GetActionDown(GameAction.TogglePhone)))
            { suppressHeldUntilRelease = true; cameraHeldRaise = false; return; }
            if (suppressHeldUntilRelease) { if (!held) suppressHeldUntilRelease = false; return; }
            if(!held)
            {
                if(!cameraHeldRaise)return;
                cameraHeldRaise=false;
                bool scriptedCamera=Phone.CameraLocked||director&&director.chase&&(director.chase.AwaitingPhotos||director.chase.IsFinalFlash);
                if(!scriptedCamera)
                {
                    // Releasing aim returns to the smartphone menu with the
                    // screen active, never to a dark or stowed handset.
                    Phone.isPhoneRaised=true;
                    if(Phone.HasCameraPower)Phone.isScreenOn=true;
                    Phone.SyncVisualState();
                    Apps.Open(PhoneApp.Home);
                }
                return;
            }
            if(!Phone.hasSmartphoneInInventory||!Phone.isScreenOn||!Phone.HasCameraPower)return;
            if(!cameraHeldRaise)
            {
                phoneRaisedBeforeFocus=Phone.isPhoneRaised;appBeforeFocus=Apps.CurrentApp;
                landscapeFromStowed=!phoneRaisedBeforeFocus;
                cameraHeldRaise=true;
            }
            if(!Phone.isPhoneRaised){Phone.isPhoneRaised=true;Phone.SyncVisualState();}
            if(!IsCameraApp)Apps.Open(PhoneApp.Camera);
        }
        private void LateUpdate()
        {
            if(!Panel||!Phone||!Apps)return;
            // PhoneDesktop can process an app shortcut later in Update. The hold
            // remains authoritative through presentation, independent of order.
            HandleCameraShortcut();
            bool allowed=(!Hud.IsModalOpen||(Hud.IsDocumentOpen&&!UiFocus.HardModal))&&(!UiFocus.Cinematic||CinematicHandsetVisible);
            bool active=Phone.isPhoneRaised&&allowed;
            raiseAmount=allowed?Mathf.MoveTowards(raiseAmount,active?1:0,Time.unscaledDeltaTime/.24f):0;
            if(raiseAmount<=0)landscapeFromStowed=false;
            bool loweringAim=landscapeFromStowed&&!active&&raiseAmount>0;
            Panel.SetActive(raiseAmount>0);bool screen=raiseAmount>0&&Phone.isScreenOn&&Phone.HasCameraPower;
            bool canFocus=screen&&IsCameraApp&&!Hud.IsModalOpen&&!UiFocus.HardModal&&!UiFocus.Menu&&(!UiFocus.Cinematic||CinematicHandsetVisible)&&(focusOverride||cameraHeldRaise&&LensFocusHeld());
            ViewfinderActive=canFocus;
            viewfinderBlend=Mathf.MoveTowards(viewfinderBlend,canFocus?1:0,Time.unscaledDeltaTime/.18f);
            var perspective=owner?owner.GetComponent<ShoulderCameraRig>():null;
            if(perspective){bool aimPose=viewfinderBlend>.01f||loweringAim;bool changed=perspective.PhoneAimOverride!=aimPose;perspective.PhoneAimOverride=aimPose;if(changed)perspective.RefreshCameraPose();}
            PresentFeed(ViewfinderActive||loweringAim);
            if(chapterReticle)
            {
                Vector3 v=FeedCamera.WorldToViewportPoint(chapterTargetPoint);
                bool targetOnScreen=chapterTargetVisible&&screen&&IsCameraApp&&v.z>0&&v.x>0&&v.x<1&&v.y>0&&v.y<1;
                chapterReticle.gameObject.SetActive(targetOnScreen);
                if(targetOnScreen){chapterReticle.rectTransform.anchorMin=chapterReticle.rectTransform.anchorMax=new Vector2(v.x,v.y);chapterReticle.rectTransform.anchoredPosition=Vector2.zero;}
            }
            float eased=raiseAmount*raiseAmount*(3-2*raiseAmount);
            if(viewfinderBlend>.001f||landscapeFromStowed)
            {
                float screenAspect=(float)Screen.width/Mathf.Max(1,Screen.height);
                float width=Mathf.Min(.62f,.68f*LandscapeHandsetAspect/screenAspect);
                float halfHeight=width*screenAspect/LandscapeHandsetAspect*.5f;
                float t=landscapeFromStowed?1:viewfinderBlend*viewfinderBlend*(3-2*viewfinderBlend);
                handsetRect.anchorMin=Vector2.Lerp(panelMin,new Vector2(.5f-width*.5f,.47f-halfHeight),t);
                handsetRect.anchorMax=Vector2.Lerp(panelMax,new Vector2(.5f+width*.5f,.47f+halfHeight),t);
                float sway=owner?Mathf.Clamp01(owner.PlanarVelocity.magnitude/4.2f):0;
                handsetRect.anchoredPosition=new Vector2(Mathf.Sin(Time.time*3.5f)*sway*1.3f,
                    landscapeFromStowed?-(Screen.height*.75f)*(1-eased):-35*(1-t));
                handsetRect.localRotation=Quaternion.Euler(0,0,landscapeFromStowed?-5*(1-eased):-4*(1-t));
            }
            else
            {
                handsetRect.anchorMin=panelMin;handsetRect.anchorMax=panelMax;
                float sway=owner&&IsCameraApp&&!Hud.IsModalOpen?Mathf.Clamp01(owner.PlanarVelocity.magnitude/4.2f):0;
                handsetRect.anchoredPosition=new Vector2(Mathf.Sin(Time.time*3.5f)*sway*1.8f,-(handsetRect.rect.height+85)*(1-eased));
                handsetRect.localRotation=Quaternion.Euler(0,0,(1-eased)*-7+Mathf.Sin(Time.time*3.5f)*sway*.35f);
            }
            handsetInput.interactable=active&&!Hud.IsModalOpen&&!ViewfinderActive;handsetInput.blocksRaycasts=active&&!Hud.IsModalOpen&&!ViewfinderActive;
            cameraRoot.SetActive(screen&&(IsCameraApp||loweringAim));Apps.SetScreenVisible(screen&&!loweringAim);FeedCamera.enabled=screen&&(IsCameraApp||loweringAim)&&HasGraphicsDevice;cameraImage.enabled=FeedCamera.enabled;
            var cameraRect=cameraRoot.GetComponent<RectTransform>();
            bool landscapeFeed=ViewfinderActive||loweringAim;
            if(hudPresentation)hudPresentation.SetViewfinderFraming(landscapeFeed);
            cameraRect.anchorMin=landscapeFeed?new Vector2(.07f,.04f):cameraMin;
            cameraRect.anchorMax=landscapeFeed?new Vector2(.93f,.96f):cameraMax;
            cameraRect.offsetMin=cameraRect.offsetMax=Vector2.zero;
            if(Phone.currentScanMode!=lastMode||Phone.isSelfieCamera!=lastSelfie)
            {
                bool changed=Phone.currentScanMode!=lastMode;lastMode=Phone.currentScanMode;lastSelfie=Phone.isSelfieCamera;
                if(changed&&screen)
                {
                    Glitch(.2f,.9f);thumbUntil=Time.unscaledTime+.22f;
                    if(shutterAudio)
                    {
                        if(!switchClip){var audio=PresentationProfile.Load()?.audio;switchClip=audio&&audio.phoneSwitch?audio.phoneSwitch:AtmajaSynth.Tone(.06f,1400,240,.3f);}
                        shutterAudio.PlayOneShot(switchClip,.45f);
                    }
                }
            }
            bool thumbVisible=screen&&!landscapeFeed&&Time.unscaledTime<thumbUntil;
            if(thumb){thumb.gameObject.SetActive(thumbVisible);if(thumbVisible)thumb.rectTransform.anchoredPosition=new Vector2(0,-12f*Mathf.Clamp01((thumbUntil-Time.unscaledTime)/.22f));}
            header.enabled=!landscapeFeed;mode.enabled=!landscapeFeed;
            homeButton.gameObject.SetActive(screen&&!UiFocus.Cinematic&&!Hud.IsModalOpen&&!landscapeFeed);stowButton.gameObject.SetActive(screen&&!UiFocus.Cinematic&&!Hud.IsModalOpen&&!landscapeFeed);
            var stowBinding=GameInputConfig.Instance?GameInputConfig.Instance.togglePhoneKey:KeyCode.Tab;
            if(stowBinding!=lastStowBinding){lastStowBinding=stowBinding;stowLabel.text="SIMPAN  ["+stowBinding.ToString().ToUpperInvariant()+"]";}
            FeedCamera.cullingMask=(playerCamera.cullingMask|((Phone.currentScanMode==ScanVisualMode.AR)?1<<ARLayer:0))&~(1<<ShoulderCameraRig.AvatarLayer);
            if(Chapter!=null&&Phone.currentScanMode==ScanVisualMode.AR)FeedCamera.cullingMask&=~(1<<30); // Chapter illusion surface; physical passage has its own latch.
            RefreshLensPose();
            PresentHand();
            runtimeFeedMaterial.SetFloat("_ARGain",Phone.currentScanMode==ScanVisualMode.AR?2:1);runtimeFeedMaterial.SetFloat("_ReducedGlitch",GameShell.ReducedGlitch?1:0);
            float disturbance=Time.unscaledTime<glitchUntil?GlitchStrength:director&&director.Stage>=AtmajaStage.BasementEntry&&director.Stage<AtmajaStage.EndingVista?.38f:0;
            runtimeFeedMaterial.SetFloat("_Glitch",disturbance);
            if(focusVignette)
            {
                float threat=disturbance;
                if(director&&director.basement&&director.basement.IsHunting)threat=1;
                float pulse=.86f+.14f*Mathf.Sin(Time.unscaledTime*17f);
                focusVignette.color=new Color(1,1,1,ViewfinderActive?Mathf.Clamp01(threat*.56f)*pulse*(GameShell.ReducedGlitch?.4f:1):0);
            }
            float signal=disturbance*(GameShell.ReducedGlitch?.15f:1);
            for(int i=0;i<signalBars.Length;i++)
            {float noise=Mathf.Sin(Mathf.Floor(Time.unscaledTime*13)+i*5.76f);var rect=signalBars[i].rectTransform;rect.anchoredPosition=new Vector2(noise*signal*18,noise*signal*13);signalBars[i].color=new Color(i%2==0?.22f:.65f,.7f,.55f,screen&&noise>.72f?signal*.5f:0);}
            if(shutter)shutter.color=new Color(1,1,1,Mathf.Clamp01((flashUntil-Time.unscaledTime)/.22f)*(GameShell.ReducedGlitch?.45f:1));
            string energyLabel=Phone.RitualCameraPower?"KAPASITOR RITUAL 5%":$"{Phone.DisplayBatteryPercent:0}%";
            header.text=!screen?"":Phone.isBroadcastConnected?$"LIVE • {(Chapter!=null?Chapter.Viewers:director?director.Viewers:14):N0}   |   {energyLabel}":ClockText+$"    LTE    {energyLabel}";
            mode.text=screen&&IsCameraApp?(Chapter!=null?Chapter.CameraStatus:(Phone.currentScanMode==ScanVisualMode.AR?"MATA BATIN / AR":"KAMERA")+$" • {Phone.DisplayBatteryPercent:0}%\n[Z] Aplikasi   [M] Peta"):"";
            LiveButton.gameObject.SetActive(screen&&IsCameraApp&&(director||Chapter!=null)&&!Phone.isBroadcastConnected&&!landscapeFeed);LiveButton.interactable=Chapter!=null?Chapter.CanBeginLive:director&&director.CanBeginLive&&!director.Busy&&!Hud.IsModalOpen;
            UiFocus.Phone=active&&screen&&!Hud.IsModalOpen&&(!IsCameraApp||((director||Chapter!=null)&&!Phone.isBroadcastConnected));
            if(owner&&!UiFocus.HardModal&&!Hud.IsModalOpen&&!UiFocus.Cinematic)owner.SetCursorLock(!UiFocus.Phone||FirstPersonController.PhoneLookHeld);
            walkHint.enabled=UiFocus.Phone&&!UiFocus.Cinematic&&!Hud.IsModalOpen&&!ViewfinderActive;
            chat.enabled=(director||Chapter!=null)&&Phone.isBroadcastConnected&&!Hud.IsModalOpen&&(!UiFocus.Cinematic||CinematicHandsetVisible)&&!ViewfinderActive;
        }
        public void ShowChapterChat(string text)
        {if(chat)chat.text=text;}
        public void ShowChapterReadout(string text,Color color)
        {
            if(!cameraRoot)return;
            if(!chapterReadout)
            {
                chapterReadout=UiWidgets.Text("ChapterSensorReadout",cameraRoot.transform,"",new Vector2(.025f,.72f),new Vector2(.975f,.98f),17,TextAnchor.UpperLeft);
                chapterReadout.raycastTarget=false;
            }
            chapterReadout.text=text;chapterReadout.color=color;
        }
        public void RefreshLensPose()
        {
            if (!FeedCamera || !owner || !owner.cameraPivot || !Phone) return;
            if (Chapter is LayarTerakhir.Phase2.Phase2Director story && story.VideoPlaying)
            {
                FeedCamera.transform.SetPositionAndRotation(story.world.videoCamera.position,story.world.videoCamera.rotation);
                FeedCamera.cullingMask &= ~((1 << ShoulderCameraRig.AvatarLayer)|(1 << GripLayer)|(1 << LayarTerakhir.Phase2.Phase2Lastri.VisibilityLayer));
                return;
            }
            bool selfie = Phone.isSelfieCamera && Phone.selfieModuleUnlocked;
            if (selfie)
            {
                Vector3 face = owner.cameraPivot.position - Vector3.up * .06f;
                Vector3 forward = owner.transform.forward;
                float distance = .72f;
                foreach (var hit in Physics.RaycastAll(face, forward, distance, ~((1 << 26) | (1 << 29) | (1 << 31)), QueryTriggerInteraction.Ignore))
                    if (!hit.transform.IsChildOf(owner.transform)) distance = Mathf.Min(distance, Mathf.Max(.28f, hit.distance - .05f));
                // Offset the physical front lens toward the watched shoulder. Parallel
                // framing leaves Raka beside the threat instead of covering it with his head.
                float shoulderOffset=Chapter is LayarTerakhir.Phase5.Phase5Director ? .32f : Chapter is LayarTerakhir.Phase2.Phase2Director p2 && p2.boss && p2.boss.Active ? .3f : .18f;
                Vector3 lens = face + forward * distance - owner.transform.right * shoulderOffset;
                Vector3 lensRay=lens-face;float clearance=lensRay.magnitude;
                foreach(var hit in Physics.RaycastAll(face,lensRay.normalized,clearance,~((1<<26)|(1<<29)|(1<<31)|(1<<25)|(1<<2)),QueryTriggerInteraction.Ignore))
                    if(!hit.transform.IsChildOf(owner.transform))clearance=Mathf.Min(clearance,Mathf.Max(.02f,hit.distance-.035f));
                lens=face+lensRay.normalized*clearance;
                Vector3 frontRay = -forward;
                if (Chapter is LayarTerakhir.Phase5.Phase5Director)
                    frontRay = Quaternion.AngleAxis(-Mathf.DeltaAngle(0, owner.cameraPivot.localEulerAngles.x), owner.transform.right) * frontRay;
                FeedCamera.transform.SetPositionAndRotation(lens, Quaternion.LookRotation(frontRay, Vector3.up));
                FeedCamera.nearClipPlane = .025f;
                FeedCamera.cullingMask |= 1 << ShoulderCameraRig.AvatarLayer;
            }
            else
            {
                FeedCamera.transform.localPosition = Vector3.forward * .08f; FeedCamera.transform.localRotation = Quaternion.identity;
                FeedCamera.nearClipPlane = .055f;
                FeedCamera.cullingMask &= ~(1 << ShoulderCameraRig.AvatarLayer);
            }
            FeedCamera.cullingMask &= ~(1 << GripLayer);
            if (Chapter is LayarTerakhir.Phase3.Phase3Director)
            {
                playerCamera.cullingMask &= ~(1 << 24);
                if (Phone.isScreenOn && Phone.HasCameraPower && Phone.currentScanMode == ScanVisualMode.UV)
                    FeedCamera.cullingMask |= 1 << 24;
                else FeedCamera.cullingMask &= ~(1 << 24);
                // In the factory only UV distinguishes a false void from the intact floor beneath.
                if(Phone.ChapterContent is LayarTerakhir.Phase3.Phase3Director)
                {
                    if(Phone.currentScanMode==ScanVisualMode.UV)FeedCamera.cullingMask &= ~(1 << 30);
                    else FeedCamera.cullingMask |= playerCamera.cullingMask & (1 << 30);
                }
            }
            if (Chapter is LayarTerakhir.Phase2.Phase2Director)
            {
                playerCamera.cullingMask &= ~(1 << LayarTerakhir.Phase2.Phase2Lastri.VisibilityLayer);
                if (selfie) FeedCamera.cullingMask |= 1 << LayarTerakhir.Phase2.Phase2Lastri.VisibilityLayer;
                else FeedCamera.cullingMask &= ~(1 << LayarTerakhir.Phase2.Phase2Lastri.VisibilityLayer);
            }
        }
        private void OnEnable()
        { RenderPipelineManager.beginCameraRendering += BeforeFeedRender; RenderPipelineManager.endCameraRendering += AfterFeedRender; }
        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= BeforeFeedRender; RenderPipelineManager.endCameraRendering -= AfterFeedRender;
            if (owner) owner.GetComponent<ShoulderCameraRig>()?.SetSelfieFeedCoverage(false);
            cameraHeldRaise = false; suppressHeldUntilRelease = LensFocusHeld();
        }
        private void BeforeFeedRender(ScriptableRenderContext context, Camera camera)
        { if (camera == FeedCamera && Phone && Phone.isSelfieCamera && owner) owner.GetComponent<ShoulderCameraRig>()?.SetSelfieFeedCoverage(true); }
        private void AfterFeedRender(ScriptableRenderContext context, Camera camera)
        { if (camera == FeedCamera && owner) owner.GetComponent<ShoulderCameraRig>()?.SetSelfieFeedCoverage(false); }
        public void ShowChapterTarget(Vector3 point,bool visible,bool valid=true){chapterTargetPoint=point;chapterTargetVisible=visible;if(chapterReticle)chapterReticle.color=valid?new Color(1,.22f,.16f):new Color(1,.7f,.3f);}
        private void PresentHand()
        {
            if(!firstPersonHand)return;
            var rig=owner?owner.GetComponent<ShoulderCameraRig>():null;
            bool visible=Panel.activeSelf&&rig&&rig.IsFirstPerson&&(!UiFocus.Cinematic||CinematicHandsetVisible);
            bool both=visible&&(viewfinderBlend>.01f||landscapeFromStowed);
            firstPersonHand.SetActive(both);if(supportHand)supportHand.SetActive(both);
            if(gripOccluder)gripOccluder.SetActive(visible);
            if(gripCamera){gripCamera.enabled=visible&&HasGraphicsDevice;gripCamera.fieldOfView=HandPresentationFieldOfView;}
            if(gripOverlay)gripOverlay.enabled=visible&&HasGraphicsDevice;
            if(!visible)return;
            bool jacket=CampaignState.Instance&&CampaignState.Instance.HasInventoryItem("JAKET_RAKA");
            // Keep the previously validated portrait grasp for apps/documents.
            // The landscape source pose cannot wrap a handset twice as tall.
            if(portraitRenderers!=null)foreach(var r in portraitRenderers)
                r.enabled=r.name.Contains("HandheldPhone")||!both&&!r.name.Contains("HandsetScreen")&&(r.name.Contains("Jacket")?jacket:!r.name.Contains("TShirt")||!jacket);
            foreach(var r in handRenderers)
                r.enabled=(!r.name.Contains("HandheldPhone")&&!r.name.Contains("HandsetScreen"))&&(r.name.Contains("Jacket")?jacket:!r.name.Contains("TShirt")||!jacket);
            if(supportRenderers!=null)foreach(var r in supportRenderers)r.enabled=(!r.name.Contains("HandheldPhone")&&!r.name.Contains("HandsetScreen"))&&(r.name.Contains("Jacket")?jacket:!r.name.Contains("TShirt")||!jacket);
            handsetRect.GetWorldCorners(panelCorners);
            Vector2 bottom=RectTransformUtility.WorldToScreenPoint(null,panelCorners[0]);
            Vector2 top=RectTransformUtility.WorldToScreenPoint(null,panelCorners[2]);
            Vector2 centre=(bottom+top)*.5f;
            float pixelHeight=(RectTransformUtility.WorldToScreenPoint(null,panelCorners[1])-bottom).magnitude;
            float pixelWidth=(RectTransformUtility.WorldToScreenPoint(null,panelCorners[3])-bottom).magnitude;
            float t=landscapeFromStowed?1:viewfinderBlend*viewfinderBlend*(3-2*viewfinderBlend);
            float physicalHeight=Mathf.Lerp(.165f,LandscapeHandsetWidth/LandscapeHandsetAspect,t);
            var presentationCamera=gripCamera?gripCamera:playerCamera;
            float depth=physicalHeight/(Mathf.Max(.08f,pixelHeight/Screen.height)*2*Mathf.Tan(presentationCamera.fieldOfView*Mathf.Deg2Rad*.5f));
            depth=Mathf.Max((gripCamera?gripCamera.nearClipPlane:.03f)+.025f,depth);
            Vector3 phoneCentre=presentationCamera.ViewportToWorldPoint(new Vector3(centre.x/Screen.width,centre.y/Screen.height,depth));
            float physicalWidth=physicalHeight*pixelWidth/Mathf.Max(1,pixelHeight);
            // Seat the grasp on the bezel instead of floating outside it: the
            // baked grasp origin is the handset edge, so pull both hands
            // slightly inward until the fingers overlap the frame.
            float gripOffset=physicalWidth*.5f*.88f;
            Quaternion handRotation=playerCamera.transform.rotation*handsetRect.localRotation;
            Vector3 edge=handRotation*Vector3.right*gripOffset;
            firstPersonHand.transform.position=phoneCentre+edge;
            firstPersonHand.transform.localScale=handScale;
            // Source grasp origin is the edge, in metres. Mirror around the
            // handset centre; never enlarge the hand to fill a stretched UI.
            firstPersonHand.transform.rotation=handRotation;
            if(both){supportHand.transform.position=phoneCentre-edge;supportHand.transform.rotation=handRotation;supportHand.transform.localScale=supportScale;}
            if(gripOccluder){gripOccluder.transform.SetPositionAndRotation(phoneCentre,firstPersonHand.transform.rotation*Quaternion.Euler(0,180,0));gripOccluder.transform.localScale=Vector3.Scale(depthMaskScale,new Vector3(physicalWidth/.078f,physicalHeight/.165f,Mathf.Lerp(1,.008f/.014f,t)));}
        }
        public void AddChat(string author,string text){if(ChatFrozen)return;pendingMessages.Enqueue($"<color=#C0AA7A>{author}</color> {text}");while(pendingMessages.Count>18)pendingMessages.Dequeue();}
        public void AddChatBurst(string[] authors,string[] lines,int count=3)
        {
            int limit=Mathf.Min(count,authors.Length,lines.Length);
            for(int i=0;i<limit;i++)AddChat(authors[i],lines[i]);
        }
        public void LabelClockModal(){}
        private void OnDestroy(){if(hudPresentation)hudPresentation.SetViewfinderFraming(false);UiFocus.Phone=false;var rig=owner?owner.GetComponent<ShoulderCameraRig>():null;if(rig)rig.PhoneAimOverride=false;if(firstPersonHand)Destroy(firstPersonHand);if(supportHand)Destroy(supportHand);if(gripOccluder)Destroy(gripOccluder);if(gripDepthMaterial)Destroy(gripDepthMaterial);foreach(var material in handMaterials)if(material)Destroy(material);if(gripCamera)Destroy(gripCamera.gameObject);if(gripOverlay)Destroy(gripOverlay.gameObject);if(gripTexture){gripTexture.Release();Destroy(gripTexture);}if(feed){feed.Release();Destroy(feed);}if(runtimeFeedMaterial)Destroy(runtimeFeedMaterial);if(shutterClip)Destroy(shutterClip);if(switchClip&&switchClip.name.Contains("AtmajaSynth"))Destroy(switchClip);if(vignetteTexture)Destroy(vignetteTexture);}
    }
}
