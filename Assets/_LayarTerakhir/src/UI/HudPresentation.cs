using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.UI
{
    // Pure presentation around the existing HUD's text fields and modal callbacks.
    public sealed class HudPresentation : MonoBehaviour
    {
        public HudStyle style;
        public HudController hud;
        public CanvasGroup objectiveGroup, interactionGroup, phoneGroup, subtitleGroup, toastGroup;
        public GameObject modalBackdrop;
        public RectTransform batteryFill;
        public Image reticle;
        public Text phoneHints;
        public ScrollRect documentScroll;
        public Text[] digitDisplays;
        private string objective;
        private float changedAt;
        private int battery=-1;
        private string digits;
        private KosSceneAudio audioRig;
        private string lastSubtitle;
        private bool lastNarrow;
        private int lastScreenWidth;
        private bool viewfinderFraming;
        public void SetViewfinderFraming(bool held)
        {
            if(viewfinderFraming==held||!toastGroup)return;
            viewfinderFraming=held;
            var rect=toastGroup.GetComponent<RectTransform>();
            rect.anchorMin=rect.anchorMax=held?new Vector2(.5f,1):Vector2.zero;
            rect.pivot=held?new Vector2(.5f,1):Vector2.zero;
            rect.anchoredPosition=held?new Vector2(0,-16):new Vector2(54,190);
        }
        private void Start()
        {
            audioRig=FindFirstObjectByType<KosSceneAudio>();
            // Ordinary world/save feedback stays readable even with a dead/stowed phone.
            // Only actual incoming donor messages belong in the handset.
            if(toastGroup)
            {
                var group=toastGroup.GetComponent<RectTransform>();group.anchorMin=group.anchorMax=Vector2.zero;group.pivot=Vector2.zero;group.anchoredPosition=new Vector2(54,190);group.sizeDelta=new Vector2(900,112);
                var heading=toastGroup.transform.Find("Heading");if(heading)heading.gameObject.SetActive(false);
                foreach(string name in new[]{"MessageBacking","MessageAccent"}){var child=toastGroup.transform.Find(name) as RectTransform;if(child)child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,112);}
                hud.broadcastToastText.rectTransform.anchoredPosition=new Vector2(24,-14);hud.broadcastToastText.rectTransform.sizeDelta=new Vector2(850,88);
            }
            foreach(var button in GetComponentsInChildren<Button>(true))button.onClick.AddListener(Click);
        }
        private void Click(){if(audioRig)audioRig.PlayUI();}
        private void LateUpdate()
        {
            if(!hud||!style)return;
            bool modal=hud.IsModalOpen;
            if(objective!=hud.objectiveText.text){objective=hud.objectiveText.text;changedAt=Time.unscaledTime;}
            Fade(objectiveGroup,!modal&&!UiFocus.Cinematic&&!string.IsNullOrEmpty(objective)&&Time.unscaledTime-changedAt<7 ? 1 : 0);
            Fade(interactionGroup,!modal&&!UiFocus.Cinematic&&!string.IsNullOrEmpty(hud.interactionPromptText.text)?1:0);
            Fade(subtitleGroup,!string.IsNullOrEmpty(hud.subtitleText.text)?1:0);
            Fade(toastGroup,!modal&&!hud.IsHandsetNotification&&!string.IsNullOrEmpty(hud.broadcastToastText.text)?1:0);
            var phone=SmartphoneController.Instance;
            bool narrow=phone&&phone.isPhoneRaised&&!modal;
            if(subtitleGroup&&(lastSubtitle!=hud.subtitleText.text||lastNarrow!=narrow||lastScreenWidth!=Screen.width))
            {
                lastSubtitle=hud.subtitleText.text;lastNarrow=narrow;lastScreenWidth=Screen.width;
                var group=subtitleGroup.GetComponent<RectTransform>();
                group.anchorMin=new Vector2(.045f,0);group.anchorMax=new Vector2(narrow?.735f:.955f,0);
                group.pivot=new Vector2(.5f,0);group.offsetMin=new Vector2(0,42);
                var text=hud.subtitleText;text.horizontalOverflow=HorizontalWrapMode.Wrap;text.verticalOverflow=VerticalWrapMode.Truncate;
                text.resizeTextForBestFit=false;text.fontSize=Mathf.Min(style.subtitleSize,28);
                var rect=text.rectTransform;rect.anchorMin=Vector2.zero;rect.anchorMax=Vector2.one;rect.offsetMin=new Vector2(22,12);rect.offsetMax=new Vector2(-22,-12);
                Canvas.ForceUpdateCanvases();
                float height=Mathf.Clamp(text.preferredHeight+28,82,265);
                group.offsetMax=new Vector2(0,42+height);
                var backing=subtitleGroup.transform.Find("SubtitleBacking") as RectTransform;
                if(backing){backing.anchorMin=Vector2.zero;backing.anchorMax=Vector2.one;backing.offsetMin=backing.offsetMax=Vector2.zero;}
            }
            // Handset status lives inside its bezel. External status is only for a stowed phone.
            Fade(phoneGroup,!modal&&!UiFocus.Cinematic&&phone&&phone.hasSmartphoneInInventory&&!phone.isPhoneRaised?1:0);
            if(modalBackdrop && modalBackdrop.activeSelf!=modal)modalBackdrop.SetActive(modal);
            if(reticle)reticle.enabled=!modal&&!UiFocus.Cinematic&&hud.blackFadeImage.color.a<.01f;
            if(hud.padlockDigitsText && digits!=hud.padlockDigitsText.text)
            {
                digits=hud.padlockDigitsText.text;int slot=0;
                if(digitDisplays!=null)foreach(char c in digits)if(c>='0'&&c<='9'&&slot<digitDisplays.Length)digitDisplays[slot++].text=c.ToString();
            }
            if(phone&&batteryFill)
            {
                int level=Mathf.Clamp(Mathf.CeilToInt(phone.batteryPercent),0,100);
                if(level!=battery){battery=level;batteryFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,1.1f*level);batteryFill.GetComponent<Image>().color=level<=15?style.danger:style.muted;}
            }
        }
        private void Fade(CanvasGroup group,float target)
        {if(group)group.alpha=Mathf.MoveTowards(group.alpha,target,Time.unscaledDeltaTime/Mathf.Max(.05f,style.fadeDuration));}
    }
}
