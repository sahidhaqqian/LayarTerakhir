using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    // Scene-local extension: Golden HUD prefab/logic is reused unchanged. Camera only runs with an active screen.
    public sealed class AtmajaPhoneView:MonoBehaviour
    {
        public AtmajaDirector director;
        public Camera playerCamera;
        public Material feedMaterial;
        public const int ARLayer=29;
        public Camera FeedCamera {get;private set;}
        public Button LiveButton {get;private set;}
        public GameObject Panel {get;private set;}
        public bool HasGraphicsDevice=>SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null;
        private Text header,mode,chat;
        private Image arTint;
        private RawImage cameraImage;
        private RenderTexture feed;
        private Material runtimeFeedMaterial;
        private readonly Queue<string> messages=new Queue<string>();
        private bool selecting;
        private HudController Hud=>HudController.Instance;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private Font font;
        private void Start()
        {
            font=Hud.GetComponent<HudPresentation>().style.bodyFont;
            var canvas=Hud.GetComponentInChildren<Canvas>().transform;
            Panel=Rect("AtmajaPhone",canvas,new Vector2(.71f,.12f),new Vector2(.96f,.72f));
            var bg=Panel.AddComponent<Image>();bg.color=new Color(.025f,.029f,.027f,.98f);
            var screen=Rect("CameraFeed",Panel.transform,new Vector2(.045f,.15f),new Vector2(.955f,.85f));
            var raw=screen.AddComponent<RawImage>();cameraImage=raw;raw.raycastTarget=false;
            runtimeFeedMaterial=new Material(feedMaterial);raw.material=runtimeFeedMaterial;
            feed=new RenderTexture(320,400,16,RenderTextureFormat.ARGB32){name="AtmajaPhoneFeed",antiAliasing=1};if(HasGraphicsDevice)feed.Create();raw.texture=feed;
            var cam=new GameObject("AtmajaPhoneCamera",typeof(Camera));cam.transform.SetParent(playerCamera.transform,false);
            FeedCamera=cam.GetComponent<Camera>();FeedCamera.CopyFrom(playerCamera);FeedCamera.targetTexture=feed;
            FeedCamera.depth=playerCamera.depth-1;FeedCamera.fieldOfView=70;FeedCamera.aspect=.8f;
            FeedCamera.GetUniversalAdditionalCameraData().renderPostProcessing=false;
            FeedCamera.GetUniversalAdditionalCameraData().renderShadows=false;
            cam.tag="Untagged";
            playerCamera.cullingMask&=~(1<<ARLayer);FeedCamera.enabled=false;
            var tint=Rect("ARTint",screen.transform,Vector2.zero,Vector2.one);arTint=tint.AddComponent<Image>();arTint.color=new Color(.16f,.8f,.35f,.16f);arTint.raycastTarget=false;
            header=Label("PhoneLiveLabel",Panel.transform,new Vector2(.06f,.86f),new Vector2(.94f,.98f),22);
            mode=Label("PhoneMode",Panel.transform,new Vector2(.06f,.02f),new Vector2(.94f,.14f),18);
            var button=Rect("MulaiLiveButton",Panel.transform,new Vector2(.10f,.36f),new Vector2(.90f,.51f));
            button.AddComponent<Image>().color=new Color(.39f,.29f,.15f,1);LiveButton=button.AddComponent<Button>();LiveButton.targetGraphic=button.GetComponent<Image>();
            var label=Label("MulaiLiveLabel",button.transform,Vector2.zero,Vector2.one,24);label.text="MULAI LIVE";label.alignment=TextAnchor.MiddleCenter;
            LiveButton.onClick.AddListener(()=>director.StartLive());
            chat=Label("AtmajaLiveChat",canvas,new Vector2(.035f,.17f),new Vector2(.32f,.43f),18);chat.color=new Color(.82f,.83f,.77f,.93f);chat.supportRichText=true;
            Panel.transform.SetAsFirstSibling();chat.transform.SetAsFirstSibling();
            foreach(var r in Phone.handPhoneMeshRoot.GetComponentsInChildren<Renderer>())r.enabled=false;
        }
        private GameObject Rect(string name,Transform parent,Vector2 min,Vector2 max)
        {var go=new GameObject(name,typeof(RectTransform));var rt=go.GetComponent<RectTransform>();rt.SetParent(parent,false);rt.anchorMin=min;rt.anchorMax=max;rt.offsetMin=rt.offsetMax=Vector2.zero;return go;}
        private Text Label(string name,Transform parent,Vector2 min,Vector2 max,int size)
        {var go=Rect(name,parent,min,max);var t=go.AddComponent<Text>();t.font=font;t.fontSize=size;t.color=new Color(.9f,.88f,.78f);t.raycastTarget=false;t.verticalOverflow=VerticalWrapMode.Truncate;return t;}
        private void LateUpdate()
        {
            if(!Panel||!Phone)return;
            bool active=Phone.isPhoneRaised&&!Hud.IsModalOpen;
            Panel.SetActive(active);
            bool screen=active&&Phone.isScreenOn&&Phone.batteryPercent>0;
            FeedCamera.enabled=screen&&HasGraphicsDevice;
            cameraImage.enabled=screen&&HasGraphicsDevice;
            FeedCamera.cullingMask=playerCamera.cullingMask|((Phone.currentScanMode==ScanVisualMode.AR)?1<<ARLayer:0);
            arTint.enabled=screen&&Phone.currentScanMode==ScanVisualMode.AR;
            runtimeFeedMaterial.SetFloat("_ARGain",arTint.enabled?3.2f:1);
            header.text=Phone.isBroadcastConnected?$"LIVE  •  {director.Viewers:N0} Penonton":"MALAM KERAMAT • OFFLINE";
            mode.text=!screen?"LAYAR MATI • Koneksi live tetap aktif":Phone.arModuleUnlocked?$"[{GameInputConfig.Instance.modeARKey}] MataBatin AR • {Phone.batteryPercent:0}%":"KAMERA BIASA • 23:52 WIB";
            LiveButton.gameObject.SetActive(screen&&!Phone.isBroadcastConnected);
            LiveButton.interactable=director.HasSurvey&&!director.Busy;
            bool choose=screen&&!Phone.isBroadcastConnected&&!director.Busy;
            if(choose){director.player.MovementLocked=true;director.player.SetCursorLock(false);}
            else if(selecting&&!director.Busy){director.player.MovementLocked=false;director.player.SetCursorLock(!Hud.IsModalOpen);}
            selecting=choose;
            chat.enabled=Phone.isBroadcastConnected&&!Hud.IsModalOpen;
            // The radio connection survives a dark/stored screen; the extra camera never does.
        }
        public void AddChat(string author,string text)
        {messages.Enqueue($"<color=#C0AA7A>{author}</color>  {text}");while(messages.Count>3)messages.Dequeue();if(chat)chat.text=string.Join("\n\n",messages);}
        public void LabelClockModal()
        {
            foreach(var t in Hud.padlockPanel.GetComponentsInChildren<Text>(true))
                if(t.text.Contains("LACI MEJA KERJA"))t.text="JAM BANDUL • 0H : MM";
            Hud.padlockFeedbackText.text="Pasang jarum dan atur waktu kematian • JAM : MENIT\nDigit kiri = jam, dua digit kanan = menit.";
            var unlock=Hud.padlockPanel.GetComponentsInChildren<Button>(true).FirstOrDefault(b=>b.name=="UnlockPadlockBtn");
            if(unlock){var t=unlock.GetComponentInChildren<Text>();if(t)t.text="ATUR JAM";}
        }
        private void OnDestroy(){if(feed){feed.Release();Destroy(feed);}if(runtimeFeedMaterial)Destroy(runtimeFeedMaterial);}
    }
}
