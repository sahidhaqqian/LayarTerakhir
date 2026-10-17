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
        private Button homeButton,stowButton;
        private Text stowLabel;private KeyCode lastStowBinding=(KeyCode)(-1);
        public bool HasGraphicsDevice=>SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null;
        public bool IsCameraApp=>Apps&&Apps.CurrentApp==PhoneApp.Camera;
        public bool CinematicHandsetVisible {get;set;}
        public bool ChatFrozen {get;set;}
        private Text header,mode,chat,walkHint;private GameObject cameraRoot;private RawImage cameraImage;private RenderTexture feed;private Material runtimeFeedMaterial;
        private readonly Queue<string> messages=new Queue<string>();
        private HudController Hud=>HudController.Instance;private SmartphoneController Phone=>SmartphoneController.Instance;
        private void Start()
        {
            if(!playerCamera){var p=FindFirstObjectByType<FirstPersonController>();if(p)playerCamera=p.GetComponent<InteractionInteractor>().playerCamera;}
            director=director?director:FindFirstObjectByType<AtmajaDirector>();
            if(!feedMaterial){var profile=PresentationProfile.Load();if(profile)feedMaterial=profile.phoneFeedMaterial;}
            UiWidgets.Font=Hud.GetComponent<HudPresentation>().style.bodyFont;
            var canvas=Hud.GetComponentInChildren<Canvas>().transform;
            Panel=PhoneSkin.Card("RakaSmartphone",canvas,new Vector2(.772f,.045f),new Vector2(.973f,.758f),new Color(.16f,.18f,.20f));
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
            if(Phone.handPhoneMeshRoot)foreach(var r in Phone.handPhoneMeshRoot.GetComponentsInChildren<Renderer>(true))r.enabled=false;
        }
        private void LateUpdate()
        {
            if(!Panel||!Phone||!Apps)return;
            bool active=Phone.isPhoneRaised&&!Hud.IsModalOpen&&(!UiFocus.Cinematic||CinematicHandsetVisible);Panel.SetActive(active);bool screen=active&&Phone.isScreenOn&&Phone.batteryPercent>0;
            cameraRoot.SetActive(screen&&IsCameraApp);Apps.SetScreenVisible(screen);FeedCamera.enabled=screen&&IsCameraApp&&HasGraphicsDevice;cameraImage.enabled=FeedCamera.enabled;
            homeButton.gameObject.SetActive(screen&&!UiFocus.Cinematic);stowButton.gameObject.SetActive(screen&&!UiFocus.Cinematic);
            var stowBinding=GameInputConfig.Instance?GameInputConfig.Instance.togglePhoneKey:KeyCode.Tab;
            if(stowBinding!=lastStowBinding){lastStowBinding=stowBinding;stowLabel.text="SIMPAN  ["+stowBinding.ToString().ToUpperInvariant()+"]";}
            FeedCamera.cullingMask=playerCamera.cullingMask|((Phone.currentScanMode==ScanVisualMode.AR)?1<<ARLayer:0);
            var owner=FindFirstObjectByType<FirstPersonController>();
            if(owner&&owner.cameraPivot)FeedCamera.transform.SetPositionAndRotation(owner.cameraPivot.position+owner.transform.forward*.26f,owner.cameraPivot.rotation);
            runtimeFeedMaterial.SetFloat("_ARGain",Phone.currentScanMode==ScanVisualMode.AR?2:1);runtimeFeedMaterial.SetFloat("_ReducedGlitch",GameShell.ReducedGlitch?1:0);
            header.text=!screen?"":Phone.isBroadcastConnected?$"LIVE • {(director?director.Viewers:14):N0}   |   {Phone.batteryPercent:0}%":(director?"23:52":"23:46")+$"    LTE    {Phone.batteryPercent:0}%";
            mode.text=screen&&IsCameraApp?(Phone.currentScanMode==ScanVisualMode.AR?"MATA BATIN / AR":"KAMERA")+$" • {Phone.batteryPercent:0}%\n[Z] Aplikasi   [M] Peta":"";
            LiveButton.gameObject.SetActive(screen&&IsCameraApp&&director&&!Phone.isBroadcastConnected);LiveButton.interactable=director&&director.CanBeginLive&&!director.Busy;
            UiFocus.Phone=active&&screen&&(!IsCameraApp||(director&&!Phone.isBroadcastConnected));
            var player=FindFirstObjectByType<FirstPersonController>();if(player&&!UiFocus.HardModal&&!Hud.IsModalOpen&&!UiFocus.Cinematic)player.SetCursorLock(!UiFocus.Phone||FirstPersonController.PhoneLookHeld);
            walkHint.enabled=UiFocus.Phone&&!UiFocus.Cinematic&&!Hud.IsModalOpen;
            chat.enabled=director&&Phone.isBroadcastConnected&&!Hud.IsModalOpen&&(!UiFocus.Cinematic||CinematicHandsetVisible);
        }
        public void AddChat(string author,string text){if(ChatFrozen)return;messages.Enqueue($"<color=#C0AA7A>{author}</color> {text}");while(messages.Count>3)messages.Dequeue();if(chat)chat.text=string.Join("\n\n",messages);}
        public void LabelClockModal(){}
        private void OnDestroy(){UiFocus.Phone=false;if(feed){feed.Release();Destroy(feed);}if(runtimeFeedMaterial)Destroy(runtimeFeedMaterial);}
    }
}
