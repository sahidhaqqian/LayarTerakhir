#if UNITY_EDITOR
using System;
using System.Linq;
using LayarTerakhir.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class GoldenHudBuilder
    {
        public const string Root="Assets/_LayarTerakhir/assets/UI/";
        public const string PrefabPath=Root+"Prefabs/GoldenHUD.prefab";
        public static HudController Install()
        {
            System.IO.Directory.CreateDirectory(Root+"Prefabs");
            var style=AssetDatabase.LoadAssetAtPath<HudStyle>(Root+"GoldenHudStyle.asset");
            if(!style){style=ScriptableObject.CreateInstance<HudStyle>();AssetDatabase.CreateAsset(style,Root+"GoldenHudStyle.asset");}
            style.bodyFont=AssetDatabase.LoadAssetAtPath<Font>(Root+"Fonts/Barlow-Regular.ttf");
            style.emphasisFont=AssetDatabase.LoadAssetAtPath<Font>(Root+"Fonts/Barlow-SemiBold.ttf");
            if(!style.bodyFont||!style.emphasisFont)throw new Exception("Bundled OFL fonts are missing.");
            EditorUtility.SetDirty(style);
            var old=Object.FindFirstObjectByType<HudController>();
            var oldCanvas=old?old.GetComponentInChildren<Canvas>():null;
            var oldPresentation=old?old.GetComponent<HudPresentation>():null;
            var owner=GameObject.Find("GameSystems").transform;
            var go=new GameObject("GoldenHUD");var hud=go.AddComponent<HudController>();hud.EnsureRuntimeUIBuilt();
            Style(hud,style);
            PrefabUtility.SaveAsPrefabAsset(go,PrefabPath);Object.DestroyImmediate(go);
            if(old)
            {
                // The original controller lived beside the campaign/director: only replace its UI component and canvas.
                if(oldPresentation)Object.DestroyImmediate(oldPresentation);
                if(oldCanvas)Object.DestroyImmediate(oldCanvas.gameObject);
                if(old.gameObject.name=="GoldenHUD")Object.DestroyImmediate(old.gameObject);
                else Object.DestroyImmediate(old);
            }
            var instance=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath));
            instance.transform.SetParent(owner,false);
            return instance.GetComponent<HudController>();
        }
        private static RectTransform Rect(string name,Transform parent,Vector2 anchor,Vector2 pivot,Vector2 position,Vector2 size)
        {
            var go=new GameObject(name,typeof(RectTransform));var r=(RectTransform)go.transform;r.SetParent(parent,false);
            r.anchorMin=r.anchorMax=anchor;r.pivot=pivot;r.anchoredPosition=position;r.sizeDelta=size;return r;
        }
        private static void Place(RectTransform r,Transform parent,Vector2 anchor,Vector2 pivot,Vector2 pos,Vector2 size)
        {r.SetParent(parent,false);r.anchorMin=r.anchorMax=anchor;r.pivot=pivot;r.anchoredPosition=pos;r.sizeDelta=size;}
        private static Image Image(string name,Transform parent,Vector2 anchor,Vector2 pivot,Vector2 pos,Vector2 size,Color color)
        {var r=Rect(name,parent,anchor,pivot,pos,size);var image=r.gameObject.AddComponent<Image>();image.color=color;image.raycastTarget=false;return image;}
        private static Text Label(string name,Transform parent,string value,Font font,int size,Color color,Vector2 pos,Vector2 area,TextAnchor align=TextAnchor.UpperLeft)
        {
            var r=Rect(name,parent,new Vector2(0,1),new Vector2(0,1),pos,area);var text=r.gameObject.AddComponent<Text>();
            text.text=value;text.font=font;text.fontSize=size;text.color=color;text.alignment=align;text.raycastTarget=false;text.supportRichText=true;
            text.horizontalOverflow=HorizontalWrapMode.Wrap;text.verticalOverflow=VerticalWrapMode.Truncate;return text;
        }
        private static CanvasGroup Group(string name,Transform canvas,Vector2 anchor,Vector2 pivot,Vector2 position,Vector2 size)
        {var r=Rect(name,canvas,anchor,pivot,position,size);var g=r.gameObject.AddComponent<CanvasGroup>();g.alpha=0;g.blocksRaycasts=false;return g;}
        public static void Style(HudController hud,HudStyle style)
        {
            var canvas=hud.GetComponentInChildren<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;
            var scaler=canvas.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution=style.referenceResolution;scaler.matchWidthOrHeight=.5f;
            foreach(var text in canvas.GetComponentsInChildren<Text>(true))
            {text.font=style.bodyFont;text.fontSize=style.bodySize;text.color=style.text;text.raycastTarget=false;text.supportRichText=true;}
            var p=hud.gameObject.AddComponent<HudPresentation>();p.hud=hud;p.style=style;
            var safe=style.safeMargins;
            p.objectiveGroup=Group("Objective",canvas.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(safe.x,-safe.y),new Vector2(620,100));
            Label("Heading",p.objectiveGroup.transform,"TUJUAN",style.emphasisFont,18,style.muted,Vector2.zero,new Vector2(500,24));
            Place(hud.objectiveText.rectTransform,p.objectiveGroup.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(0,-30),new Vector2(620,68));
            hud.objectiveText.fontSize=26;hud.objectiveText.alignment=TextAnchor.UpperLeft;hud.objectiveText.lineSpacing=1.1f;
            Image("Rule",p.objectiveGroup.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(0,-23),new Vector2(32,1),style.accent);

            p.phoneGroup=Group("PhoneStatus",canvas.transform,new Vector2(1,1),new Vector2(1,1),new Vector2(-safe.x,-safe.y),new Vector2(340,132));
            Label("Heading",p.phoneGroup.transform,"SMARTPHONE",style.emphasisFont,18,style.muted,new Vector2(0,0),new Vector2(300,24));
            Place(hud.phoneStatusText.rectTransform,p.phoneGroup.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(0,-30),new Vector2(340,65));
            hud.phoneStatusText.alignment=TextAnchor.UpperLeft;hud.phoneStatusText.fontSize=25;
            p.phoneHints=Label("PhoneControls",p.phoneGroup.transform,"",style.bodyFont,18,style.muted,new Vector2(0,-102),new Vector2(360,25));
            Image("BatteryTrack",p.phoneGroup.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(0,-93),new Vector2(110,2),new Color(.3f,.32f,.28f,.5f));
            p.batteryFill=Image("BatteryFill",p.phoneGroup.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(0,-93),new Vector2(110,2),style.muted).rectTransform;

            p.interactionGroup=Group("Interaction",canvas.transform,new Vector2(.5f,0),new Vector2(.5f,0),new Vector2(0,212),new Vector2(940,45));
            Place(hud.interactionPromptText.rectTransform,p.interactionGroup.transform,new Vector2(.5f,.5f),new Vector2(.5f,.5f),Vector2.zero,new Vector2(940,45));
            hud.interactionPromptText.fontSize=24;hud.interactionPromptText.alignment=TextAnchor.MiddleCenter;
            var shadow=hud.interactionPromptText.gameObject.AddComponent<Shadow>();shadow.effectColor=new Color(0,0,0,.85f);shadow.effectDistance=new Vector2(0,-1);

            p.subtitleGroup=Group("Subtitles",canvas.transform,new Vector2(.5f,0),new Vector2(.5f,0),new Vector2(0,safe.y),new Vector2(1360,150));
            Image("SubtitleBacking",p.subtitleGroup.transform,new Vector2(.5f,.5f),new Vector2(.5f,.5f),Vector2.zero,new Vector2(1360,150),new Color(.02f,.025f,.02f,.72f)).transform.SetAsFirstSibling();
            Place(hud.subtitleText.rectTransform,p.subtitleGroup.transform,new Vector2(.5f,.5f),new Vector2(.5f,.5f),Vector2.zero,new Vector2(1280,136));
            hud.subtitleText.fontSize=style.subtitleSize;hud.subtitleText.alignment=TextAnchor.MiddleCenter;hud.subtitleText.lineSpacing=1.05f;

            p.toastGroup=Group("IncomingMessage",canvas.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(safe.x,-166),new Vector2(900,240));
            Image("MessageBacking",p.toastGroup.transform,new Vector2(0,1),new Vector2(0,1),Vector2.zero,new Vector2(900,240),style.panel).transform.SetAsFirstSibling();
            Image("MessageAccent",p.toastGroup.transform,new Vector2(0,1),new Vector2(0,1),Vector2.zero,new Vector2(2,240),style.accent);
            Label("Heading",p.toastGroup.transform,"PESAN MASUK",style.emphasisFont,18,style.muted,new Vector2(24,-18),new Vector2(800,24));
            Place(hud.broadcastToastText.rectTransform,p.toastGroup.transform,new Vector2(0,1),new Vector2(0,1),new Vector2(24,-52),new Vector2(850,176));
            hud.broadcastToastText.fontSize=24;hud.broadcastToastText.alignment=TextAnchor.UpperLeft;hud.broadcastToastText.color=style.text;

            var backdrop=Image("ModalBackdrop",canvas.transform,new Vector2(.5f,.5f),new Vector2(.5f,.5f),Vector2.zero,Vector2.zero,new Color(0,0,0,.7f));
            backdrop.rectTransform.anchorMin=Vector2.zero;backdrop.rectTransform.anchorMax=Vector2.one;backdrop.rectTransform.offsetMin=backdrop.rectTransform.offsetMax=Vector2.zero;
            backdrop.raycastTarget=true;backdrop.transform.SetSiblingIndex(Mathf.Min(hud.documentPanel.transform.GetSiblingIndex(),hud.padlockPanel.transform.GetSiblingIndex()));p.modalBackdrop=backdrop.gameObject;backdrop.gameObject.SetActive(false);
            var doc=(RectTransform)hud.documentPanel.transform;doc.sizeDelta=new Vector2(920,640);doc.GetComponent<Image>().color=style.paper;
            Place(hud.documentTitleText.rectTransform,doc,new Vector2(.5f,1),new Vector2(.5f,1),new Vector2(0,-40),new Vector2(800,90));
            hud.documentTitleText.font=style.emphasisFont;hud.documentTitleText.fontSize=28;hud.documentTitleText.color=style.ink;hud.documentTitleText.alignment=TextAnchor.UpperLeft;
            var viewport=Rect("DocumentViewport",doc,new Vector2(.5f,.5f),new Vector2(.5f,.5f),new Vector2(0,-8),new Vector2(800,350));viewport.gameObject.AddComponent<RectMask2D>();
            p.documentScroll=viewport.gameObject.AddComponent<ScrollRect>();p.documentScroll.viewport=viewport;p.documentScroll.horizontal=false;p.documentScroll.movementType=ScrollRect.MovementType.Clamped;p.documentScroll.scrollSensitivity=32;
            Place(hud.documentBodyText.rectTransform,viewport,new Vector2(.5f,1),new Vector2(.5f,1),Vector2.zero,new Vector2(800,350));
            hud.documentBodyText.color=style.ink;hud.documentBodyText.fontSize=25;hud.documentBodyText.alignment=TextAnchor.UpperLeft;hud.documentBodyText.lineSpacing=1.18f;
            hud.documentBodyText.verticalOverflow=VerticalWrapMode.Overflow;hud.documentBodyText.raycastTarget=true;
            var fitter=hud.documentBodyText.gameObject.AddComponent<ContentSizeFitter>();fitter.verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            p.documentScroll.content=hud.documentBodyText.rectTransform;
            Place((RectTransform)hud.documentCloseButton.transform,doc,new Vector2(.5f,0),new Vector2(.5f,0),new Vector2(0,30),new Vector2(220,46));
            hud.documentCloseButton.GetComponentInChildren<Text>().text="Tutup dokumen";
            Label("ScrollHint",doc,"Gulir untuk membaca",style.bodyFont,18,new Color(.3f,.31f,.27f),new Vector2(60,-546),new Vector2(360,24));

            var padlock=(RectTransform)hud.padlockPanel.transform;padlock.sizeDelta=new Vector2(680,460);padlock.GetComponent<Image>().color=style.panel;
            var header=padlock.Find("PadlockHeader").GetComponent<Text>();header.text="LACI MEJA KERJA";header.font=style.emphasisFont;header.fontSize=28;header.color=style.text;
            hud.padlockDigitsText.enabled=false;p.digitDisplays=new Text[3];
            for(int i=0;i<3;i++)
            {
                var cell=Image("DigitCell_"+i,padlock,new Vector2(.5f,.5f),new Vector2(.5f,.5f),new Vector2((i-1)*140,45),new Vector2(90,86),new Color(.11f,.125f,.11f));
                p.digitDisplays[i]=Label("Digit",cell.transform,"0",style.emphasisFont,52,style.text,Vector2.zero,new Vector2(90,86),TextAnchor.MiddleCenter);
            }
            hud.padlockFeedbackText.fontSize=21;hud.padlockFeedbackText.color=style.muted;
            foreach(var button in canvas.GetComponentsInChildren<Button>(true))
            {
                var colors=button.colors;colors.normalColor=new Color(.15f,.17f,.14f);colors.highlightedColor=new Color(.29f,.32f,.26f);colors.pressedColor=style.accent;button.colors=colors;
                button.GetComponent<Image>().color=Color.white;var text=button.GetComponentInChildren<Text>();text.color=style.text;text.fontSize=22;
                if(button.name=="UnlockPadlockBtn")text.text="Buka gembok";
                if(button.name=="CancelPadlockBtn")text.text="Batal";
            }
            p.reticle=canvas.transform.Find("Crosshair").GetComponent<Image>();p.reticle.color=new Color(.85f,.84f,.76f,.35f);p.reticle.rectTransform.sizeDelta=new Vector2(3,3);
            hud.phoneStatusText.text=string.Empty;hud.objectiveText.text=string.Empty;hud.subtitleText.text=string.Empty;hud.broadcastToastText.text=string.Empty;
            hud.interactionPromptText.text=string.Empty;hud.blackFadeImage.color=Color.clear;hud.blackFadeImage.transform.SetAsLastSibling();
            hud.documentPanel.SetActive(false);hud.padlockPanel.SetActive(false);
        }
    }
}
#endif
