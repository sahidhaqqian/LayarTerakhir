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
        private Vector2 subtitleMin,subtitleMax,subtitleOffsetMin,subtitleOffsetMax,subtitlePivot;
        private bool narrowSubtitle;
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
            var rect=hud.subtitleText.rectTransform;subtitleMin=rect.anchorMin;subtitleMax=rect.anchorMax;subtitleOffsetMin=rect.offsetMin;subtitleOffsetMax=rect.offsetMax;subtitlePivot=rect.pivot;
            foreach(var button in GetComponentsInChildren<Button>(true))button.onClick.AddListener(Click);
        }
        private void Click(){if(audioRig)audioRig.PlayUI();}
        private void LateUpdate()
        {
            if(!hud||!style)return;
            bool modal=hud.IsModalOpen;
            if(objective!=hud.objectiveText.text){objective=hud.objectiveText.text;changedAt=Time.unscaledTime;}
            Fade(objectiveGroup,!modal&&!string.IsNullOrEmpty(objective) ? (Time.unscaledTime-changedAt>12 ? .65f : 1) : 0);
            Fade(interactionGroup,!modal&&!string.IsNullOrEmpty(hud.interactionPromptText.text)?1:0);
            Fade(subtitleGroup,!string.IsNullOrEmpty(hud.subtitleText.text)?1:0);
            Fade(toastGroup,!modal&&!hud.IsHandsetNotification&&!string.IsNullOrEmpty(hud.broadcastToastText.text)?1:0);
            var phone=SmartphoneController.Instance;
            bool narrow=phone&&phone.isPhoneRaised&&!modal;
            if(narrow!=narrowSubtitle)
            {
                narrowSubtitle=narrow;var rect=hud.subtitleText.rectTransform;
                if(narrow){rect.anchorMin=new Vector2(.035f,0);rect.anchorMax=new Vector2(.66f,0);rect.pivot=new Vector2(.5f,0);rect.offsetMin=new Vector2(0,45);rect.offsetMax=new Vector2(0,210);}
                else{rect.anchorMin=subtitleMin;rect.anchorMax=subtitleMax;rect.pivot=subtitlePivot;rect.offsetMin=subtitleOffsetMin;rect.offsetMax=subtitleOffsetMax;}
            }
            // Handset status lives inside its bezel. External status is only for a stowed phone.
            Fade(phoneGroup,!modal&&phone&&phone.hasSmartphoneInInventory&&!phone.isPhoneRaised?1:0);
            if(modalBackdrop && modalBackdrop.activeSelf!=modal)modalBackdrop.SetActive(modal);
            if(reticle)reticle.enabled=!modal&&!UiFocus.Cinematic&&!UiFocus.Phone&&hud.blackFadeImage.color.a<.01f;
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
