using System.Collections.Generic;
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
        private readonly System.Random chatRandom=new System.Random();
        private float nextChat,nextAmbient,flashUntil,glitchUntil;private string currentRoom="";
        private Image shutter;private AudioSource shutterAudio;private AudioClip shutterClip;
        private Image[] signalBars;
        public float LastChatTime {get;private set;}
        public int DisplayedChatCount {get;private set;}
        public float GlitchStrength {get;set;}
        private HudController Hud=>HudController.Instance;private SmartphoneController Phone=>SmartphoneController.Instance;
        private void Start()
        {
            if(!playerCamera){var p=FindFirstObjectByType<FirstPersonController>();if(p)playerCamera=p.GetComponent<InteractionInteractor>().playerCamera;}
            director=director?director:FindFirstObjectByType<AtmajaDirector>();
            if(!feedMaterial){var profile=PresentationProfile.Load();if(profile)feedMaterial=profile.phoneFeedMaterial;}
            UiWidgets.Font=Hud.GetComponent<HudPresentation>().style.bodyFont;
            var canvas=Hud.GetComponentInChildren<Canvas>().transform;
            Panel=PhoneSkin.Card("RakaSmartphone",canvas,new Vector2(.772f,.045f),new Vector2(.973f,.758f),new Color(.16f,.18f,.20f));
            handsetInput=Panel.AddComponent<CanvasGroup>();
            PhoneSkin.Card("GlassScreen",Panel.transform,new Vector2(.018f,.012f),new Vector2(.982f,.988f),new Color(.008f,.012f,.019f));
            PhoneSkin.Card("Earpiece",Panel.transform,new Vector2(.38f,.978f),new Vector2(.62f,.984f),new Color(.23f,.25f,.27f));
            PhoneSkin.Card("FrontLens",Panel.transform,new Vector2(.66f,.970f),new Vector2(.69f,.985f),new Color(.06f,.13f,.18f));
            PhoneSkin.Card("PowerButton",Panel.transform,new Vector2(1,.66f),new Vector2(1.008f,.78f),new Color(.25f,.28f,.3f));
            PhoneSkin.Card("VolumeRocker",Panel.transform,new Vector2(-.008f,.63f),new Vector2(0,.80f),new Color(.25f,.28f,.3f));
            header=UiWidgets.Text("PhoneLiveLabel",Panel.transform,"",new Vector2(.075f,.918f),new Vector2(.925f,.966f),20);
            cameraRoot=UiWidgets.Rect("CameraApp",Panel.transform,new Vector2(.045f,.16f),new Vector2(.955f,.91f));cameraImage=cameraRoot.AddComponent<RawImage>();cameraImage.raycastTarget=false;
            runtimeFeedMaterial=new Material(feedMaterial);cameraImage.material=runtimeFeedMaterial;
            feed=new RenderTexture(256,448,16,RenderTextureFormat.ARGB32){name="RakaPhoneFeed",antiAliasing=1};if(HasGraphicsDevice)feed.Create();cameraImage.texture=feed;
            var cam=new GameObject("RakaPhoneCamera",typeof(Camera));cam.transform.SetParent(playerCamera.transform.root,false);FeedCamera=cam.GetComponent<Camera>();FeedCamera.CopyFrom(playerCamera);FeedCamera.targetTexture=feed;
            FeedCamera.depth=playerCamera.depth-1;FeedCamera.fieldOfView=70;FeedCamera.aspect=256f/448;FeedCamera.GetUniversalAdditionalCameraData().renderPostProcessing=false;FeedCamera.GetUniversalAdditionalCameraData().renderShadows=false;cam.tag="Untagged";
            playerCamera.cullingMask&=~((1<<ARLayer)|(1<<AtmajaHandycamPlayback.RecordingLayer)|(1<<AtmajaMirrorReflection.ReflectionLayer));FeedCamera.enabled=false;
            LiveButton=UiWidgets.Button("MulaiLiveButton",cameraRoot.transform,"MULAI LIVE",new Vector2(.10f,.36f),new Vector2(.90f,.51f),()=>{if(director)director.StartLive();});
            mode=UiWidgets.Text("PhoneMode",Panel.transform,"",new Vector2(.06f,.09f),new Vector2(.94f,.16f),18);
            var home=PhoneSkin.Button("PhoneHome",Panel.transform,"APLIKASI  [Z]",new Vector2(.055f,.026f),new Vector2(.49f,.077f),()=>Apps.Open(PhoneApp.Home));home.GetComponentInChildren<Text>().fontSize=16;
            var stow=PhoneSkin.Button("PhoneStow",Panel.transform,"SIMPAN  [TAB]",new Vector2(.51f,.026f),new Vector2(.945f,.077f),()=>Apps.Stow());stow.GetComponentInChildren<Text>().fontSize=16;
            homeButton=home;stowButton=stow;
            stowLabel=stow.GetComponentInChildren<Text>();
            PhoneSkin.Card("GestureBar",Panel.transform,new Vector2(.37f,.014f),new Vector2(.63f,.019f),new Color(.55f,.57f,.6f));
            chat=UiWidgets.Text("AtmajaLiveChat",canvas,"",new Vector2(.035f,.17f),new Vector2(.32f,.43f),18);chat.color=new Color(.82f,.83f,.77f,.93f);
            walkHint=UiWidgets.Text("PhoneWalkHint",canvas,"WASD jalan · tahan klik kanan untuk menoleh",new Vector2(.52f,.008f),new Vector2(.97f,.04f),16,TextAnchor.MiddleRight);
            Apps=gameObject.AddComponent<PhoneDesktop>();Apps.Initialize(this);Panel.transform.SetAsFirstSibling();chat.transform.SetAsFirstSibling();
            shutter=UiWidgets.Panel("PhoneShutterFlash",cameraRoot.transform,Vector2.zero,Vector2.one,Color.clear).GetComponent<Image>();shutter.raycastTarget=false;
            signalBars=new Image[8];for(int i=0;i<signalBars.Length;i++)
            {signalBars[i]=UiWidgets.Panel("HandsetSignalTear_"+i,Panel.transform,new Vector2(.045f,.15f+i*.09f),new Vector2(.955f,.151f+i*.09f),Color.clear).GetComponent<Image>();signalBars[i].raycastTarget=false;}
            shutterAudio=gameObject.AddComponent<AudioSource>();shutterAudio.playOnAwake=false;
            var audio=PresentationProfile.Load()?.audio;if(audio)shutterAudio.outputAudioMixerGroup=audio.uiBus;
            if(Phone.handPhoneMeshRoot)foreach(var r in Phone.handPhoneMeshRoot.GetComponentsInChildren<Renderer>(true))r.enabled=false;
        }
        private void Update()
        {
            if(!Phone||!Apps||!Hud||UiFocus.HardModal||Hud.IsModalOpen)return;
            if(!UiFocus.Cinematic&&Phone.isPhoneRaised&&Phone.isScreenOn&&IsCameraApp&&!UiFocus.Phone&&(!director||!director.Busy&&director.Stage<AtmajaStage.Chase)
                &&GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz))CaptureFlash();
            if(!director||!Phone.isBroadcastConnected||ChatFrozen||UiFocus.Menu||UiFocus.Cinematic)return;
            var p=director.LocalPosition(director.player.transform.position);float y=director.player.transform.position.y-director.origin.y;
            string room=y<-.5f?"cellar":y>3.3f?(p.y>5.1f?"hall":p.x< -2.5f?"child":p.x<.25f?"bath":p.x<2.3f?"master":"balcony"):
                p.y>9?"kitchen":p.x>1.6f&&p.x<6.1f&&p.y>1?"dining":p.y>0&&p.x<1.6f?"lounge":"yard";
            if(room!=currentRoom||Time.time>=nextAmbient)
            {
                currentRoom=room;nextAmbient=Time.time+9+(float)chatRandom.NextDouble()*8;
                string[] lines=RoomComments(room);
                if(pendingMessages.Count<4)AddChat(new[]{"@kopi_susu99","@dinda_ayu","@intan_permata","@bocil_kematian","@rudi_tabuti"}[chatRandom.Next(5)],lines[chatRandom.Next(lines.Length)]);
            }
            if(pendingMessages.Count>0&&Time.time>=nextChat)
            {
                messages.Enqueue(pendingMessages.Dequeue());while(messages.Count>3)messages.Dequeue();
                chat.text=string.Join("\n\n",messages);LastChatTime=Time.time;DisplayedChatCount++;
                nextChat=Time.time+2+(float)chatRandom.NextDouble();
            }
        }
        private static string[] RoomComments(string room)
        {
            switch(room)
            {
                case "child":return new[]{"Mainannya masih ada semua...","Itu kamar anaknya ya? Sedih banget lihatnya.","Aku denger sesuatu kayak anak kecil tadi."};
                case "bath":return new[]{"Kok lantainya basah bang?","Di sini suaranya kayak menggema ya.","Airnya warnanya aneh, jangan disentuh bang."};
                case "hall":return new[]{"Lantai atas bikin merinding asli.","Bang pelan-pelan, langkah siapa itu?","Lukisan keluarganya serem juga dilihat malam-malam."};
                case "master":return new[]{"Kamarnya gede tapi sunyi banget.","Ada suara kain diseret nggak sih?","Gue masih mantau bang, jangan panik."};
                case "balcony":return new[]{"Anginnya masuk mic juga bang.","Tuh dari balkon kelihatan halaman.","Aku nggak suka tali itu gerak sendiri."};
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
        private void LateUpdate()
        {
            if(!Panel||!Phone||!Apps)return;
            bool active=Phone.isPhoneRaised&&(!Hud.IsModalOpen||(Hud.IsDocumentOpen&&!UiFocus.HardModal))&&(!UiFocus.Cinematic||CinematicHandsetVisible);Panel.SetActive(active);bool screen=active&&Phone.isScreenOn&&Phone.batteryPercent>0;
            handsetInput.interactable=!Hud.IsModalOpen;handsetInput.blocksRaycasts=!Hud.IsModalOpen;
            cameraRoot.SetActive(screen&&IsCameraApp);Apps.SetScreenVisible(screen);FeedCamera.enabled=screen&&IsCameraApp&&HasGraphicsDevice;cameraImage.enabled=FeedCamera.enabled;
            homeButton.gameObject.SetActive(screen&&!UiFocus.Cinematic&&!Hud.IsModalOpen);stowButton.gameObject.SetActive(screen&&!UiFocus.Cinematic&&!Hud.IsModalOpen);
            var stowBinding=GameInputConfig.Instance?GameInputConfig.Instance.togglePhoneKey:KeyCode.Tab;
            if(stowBinding!=lastStowBinding){lastStowBinding=stowBinding;stowLabel.text="SIMPAN  ["+stowBinding.ToString().ToUpperInvariant()+"]";}
            FeedCamera.cullingMask=(playerCamera.cullingMask|((Phone.currentScanMode==ScanVisualMode.AR)?1<<ARLayer:0))&~(1<<ShoulderCameraRig.AvatarLayer);
            var owner=FindFirstObjectByType<FirstPersonController>();
            if(owner&&owner.cameraPivot)FeedCamera.transform.SetPositionAndRotation(owner.cameraPivot.position+owner.transform.forward*.26f,owner.cameraPivot.rotation);
            runtimeFeedMaterial.SetFloat("_ARGain",Phone.currentScanMode==ScanVisualMode.AR?2:1);runtimeFeedMaterial.SetFloat("_ReducedGlitch",GameShell.ReducedGlitch?1:0);
            float disturbance=Time.unscaledTime<glitchUntil?GlitchStrength:director&&director.Stage>=AtmajaStage.BasementEntry&&director.Stage<AtmajaStage.EndingVista?.38f:0;
            runtimeFeedMaterial.SetFloat("_Glitch",disturbance);
            float signal=disturbance*(GameShell.ReducedGlitch?.15f:1);
            for(int i=0;i<signalBars.Length;i++)
            {float noise=Mathf.Sin(Mathf.Floor(Time.unscaledTime*13)+i*5.76f);var rect=signalBars[i].rectTransform;rect.anchoredPosition=new Vector2(noise*signal*18,noise*signal*13);signalBars[i].color=new Color(i%2==0?.22f:.65f,.7f,.55f,screen&&noise>.72f?signal*.5f:0);}
            if(shutter)shutter.color=new Color(1,1,1,Mathf.Clamp01((flashUntil-Time.unscaledTime)/.22f)*(GameShell.ReducedGlitch?.45f:1));
            header.text=!screen?"":Phone.isBroadcastConnected?$"LIVE • {(director?director.Viewers:14):N0}   |   {Phone.batteryPercent:0}%":(director?"23:52":"23:46")+$"    LTE    {Phone.batteryPercent:0}%";
            mode.text=screen&&IsCameraApp?(Phone.currentScanMode==ScanVisualMode.AR?"MATA BATIN / AR":"KAMERA")+$" • {Phone.batteryPercent:0}%\n[Z] Aplikasi   [M] Peta":"";
            LiveButton.gameObject.SetActive(screen&&IsCameraApp&&director&&!Phone.isBroadcastConnected);LiveButton.interactable=director&&director.CanBeginLive&&!director.Busy&&!Hud.IsModalOpen;
            UiFocus.Phone=active&&screen&&!Hud.IsModalOpen&&(!IsCameraApp||(director&&!Phone.isBroadcastConnected));
            var player=FindFirstObjectByType<FirstPersonController>();if(player&&!UiFocus.HardModal&&!Hud.IsModalOpen&&!UiFocus.Cinematic)player.SetCursorLock(!UiFocus.Phone||FirstPersonController.PhoneLookHeld);
            walkHint.enabled=UiFocus.Phone&&!UiFocus.Cinematic&&!Hud.IsModalOpen;
            chat.enabled=director&&Phone.isBroadcastConnected&&!Hud.IsModalOpen&&(!UiFocus.Cinematic||CinematicHandsetVisible);
        }
        public void AddChat(string author,string text){if(ChatFrozen)return;pendingMessages.Enqueue($"<color=#C0AA7A>{author}</color> {text}");while(pendingMessages.Count>18)pendingMessages.Dequeue();}
        public void LabelClockModal(){}
        private void OnDestroy(){UiFocus.Phone=false;if(feed){feed.Release();Destroy(feed);}if(runtimeFeedMaterial)Destroy(runtimeFeedMaterial);if(shutterClip)Destroy(shutterClip);}
    }
}
