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
        public bool HasGraphicsDevice=>SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null;
        public bool IsCameraApp=>Apps&&Apps.CurrentApp==PhoneApp.Camera;
        private Text header,mode,chat;private GameObject cameraRoot;private RawImage cameraImage;private RenderTexture feed;private Material runtimeFeedMaterial;
        private readonly Queue<string> messages=new Queue<string>();
        private HudController Hud=>HudController.Instance;private SmartphoneController Phone=>SmartphoneController.Instance;
        private void Start()
        {
            if(!playerCamera){var p=FindFirstObjectByType<FirstPersonController>();if(p)playerCamera=p.GetComponent<InteractionInteractor>().playerCamera;}
            director=director?director:FindFirstObjectByType<AtmajaDirector>();
            if(!feedMaterial){var profile=PresentationProfile.Load();if(profile)feedMaterial=profile.phoneFeedMaterial;}
            UiWidgets.Font=Hud.GetComponent<HudPresentation>().style.bodyFont;
            var canvas=Hud.GetComponentInChildren<Canvas>().transform;
            Panel=UiWidgets.Panel("RakaSmartphone",canvas,new Vector2(.70f,.10f),new Vector2(.97f,.80f),new Color(.015f,.019f,.021f,.995f));
            header=UiWidgets.Text("PhoneLiveLabel",Panel.transform,"",new Vector2(.06f,.90f),new Vector2(.94f,.98f),23);
            cameraRoot=UiWidgets.Rect("CameraApp",Panel.transform,new Vector2(.045f,.15f),new Vector2(.955f,.89f));cameraImage=cameraRoot.AddComponent<RawImage>();cameraImage.raycastTarget=false;
            runtimeFeedMaterial=new Material(feedMaterial);cameraImage.material=runtimeFeedMaterial;
            feed=new RenderTexture(320,400,16,RenderTextureFormat.ARGB32){name="RakaPhoneFeed",antiAliasing=1};if(HasGraphicsDevice)feed.Create();cameraImage.texture=feed;
            var cam=new GameObject("RakaPhoneCamera",typeof(Camera));cam.transform.SetParent(playerCamera.transform,false);FeedCamera=cam.GetComponent<Camera>();FeedCamera.CopyFrom(playerCamera);FeedCamera.targetTexture=feed;
            FeedCamera.depth=playerCamera.depth-1;FeedCamera.fieldOfView=70;FeedCamera.aspect=.8f;FeedCamera.GetUniversalAdditionalCameraData().renderPostProcessing=false;FeedCamera.GetUniversalAdditionalCameraData().renderShadows=false;cam.tag="Untagged";
            playerCamera.cullingMask&=~(1<<ARLayer);FeedCamera.enabled=false;
            LiveButton=UiWidgets.Button("MulaiLiveButton",cameraRoot.transform,"MULAI LIVE",new Vector2(.10f,.36f),new Vector2(.90f,.51f),()=>{if(director)director.StartLive();});
            mode=UiWidgets.Text("PhoneMode",Panel.transform,"",new Vector2(.06f,.055f),new Vector2(.94f,.145f),18);
            UiWidgets.Button("PhoneHome",Panel.transform,"APLIKASI  [Z]",new Vector2(.20f,.005f),new Vector2(.80f,.055f),()=>Apps.Open(PhoneApp.Home));
            chat=UiWidgets.Text("AtmajaLiveChat",canvas,"",new Vector2(.035f,.17f),new Vector2(.32f,.43f),18);chat.color=new Color(.82f,.83f,.77f,.93f);
            Apps=gameObject.AddComponent<PhoneDesktop>();Apps.Initialize(this);Panel.transform.SetAsFirstSibling();chat.transform.SetAsFirstSibling();
            if(Phone.handPhoneMeshRoot)foreach(var r in Phone.handPhoneMeshRoot.GetComponentsInChildren<Renderer>(true))r.enabled=false;
        }
        private void LateUpdate()
        {
            if(!Panel||!Phone||!Apps)return;
            bool active=Phone.isPhoneRaised&&!Hud.IsModalOpen;Panel.SetActive(active);bool screen=active&&Phone.isScreenOn&&Phone.batteryPercent>0;
            cameraRoot.SetActive(screen&&IsCameraApp);Apps.SetScreenVisible(screen);FeedCamera.enabled=screen&&IsCameraApp&&HasGraphicsDevice;cameraImage.enabled=FeedCamera.enabled;
            FeedCamera.cullingMask=playerCamera.cullingMask|((Phone.currentScanMode==ScanVisualMode.AR)?1<<ARLayer:0);
            runtimeFeedMaterial.SetFloat("_ARGain",Phone.currentScanMode==ScanVisualMode.AR?2:1);runtimeFeedMaterial.SetFloat("_ReducedGlitch",GameShell.ReducedGlitch?1:0);
            header.text=Phone.isBroadcastConnected?$"LIVE • {(director?director.Viewers:14):N0} penonton":(director?"23:52":"23:46")+" • MALAM KERAMAT";
            mode.text=!screen?"LAYAR MATI • Siaran tetap terhubung":IsCameraApp?(Phone.currentScanMode==ScanVisualMode.AR?"MATA BATIN / AR":"KAMERA")+$" • {Phone.batteryPercent:0}%\n[Z] Aplikasi   [M] Peta":"[TAB] Simpan • [F] Layar";
            LiveButton.gameObject.SetActive(screen&&IsCameraApp&&director&&!Phone.isBroadcastConnected);LiveButton.interactable=director&&director.HasSurvey&&!director.Busy;
            UiFocus.Phone=active&&screen&&(!IsCameraApp||(director&&!Phone.isBroadcastConnected));
            var player=FindFirstObjectByType<FirstPersonController>();if(player&&!UiFocus.HardModal&&!Hud.IsModalOpen)player.SetCursorLock(!UiFocus.Phone);
            chat.enabled=director&&Phone.isBroadcastConnected&&!Hud.IsModalOpen;
        }
        public void AddChat(string author,string text){messages.Enqueue($"<color=#C0AA7A>{author}</color> {text}");while(messages.Count>3)messages.Dequeue();if(chat)chat.text=string.Join("\n\n",messages);}
        public void LabelClockModal(){}
        private void OnDestroy(){UiFocus.Phone=false;if(feed){feed.Release();Destroy(feed);}if(runtimeFeedMaterial)Destroy(runtimeFeedMaterial);}
    }
}
