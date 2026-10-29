using System;
using System.Collections;
using System.Collections.Generic;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LayarTerakhir.UI
{
    public class HudController : MonoBehaviour
    {
        public static HudController Instance { get; private set; }

        [Header("UI Text Elements")]
        public Text objectiveText;
        public Text subtitleText;
        public Text interactionPromptText;
        public Text phoneStatusText;
        public Text broadcastToastText;

        [Header("Modal Panels")]
        public GameObject documentPanel;
        public Text documentTitleText;
        public Text documentBodyText;
        public Button documentCloseButton;
        private Sprite documentPaperSprite;

        public GameObject padlockPanel;
        public Text padlockDigitsText;
        public Text padlockFeedbackText;

        public Image blackFadeImage;

        [Header("Threat Bar")]
        public Image threatBarFill;
        private Image[] threatSegments;
        public Text threatExitKeyText;
        private GameObject threatBarRoot;
        private float _lastThreatFill = -2f;

        public bool IsDocumentOpen => documentPanel != null && documentPanel.activeSelf;

        public bool IsModalOpen =>
            UiFocus.HardModal ||
            (documentPanel != null && documentPanel.activeSelf) ||
            (padlockPanel != null && padlockPanel.activeSelf);

        private readonly Queue<(string speaker, string line, float duration, Vector3? position)> _subtitleQueue =
            new Queue<(string, string, float, Vector3?)>();
        private Coroutine _subtitleRoutine;
        private Coroutine _broadcastRoutine;
        private readonly Dictionary<Button, UnityEngine.Events.UnityAction> _buttonCallbacks =
            new Dictionary<Button, UnityEngine.Events.UnityAction>();
        private Action _onDocumentClosed;
        private Action _onPadlockSolved;
        private int[] _digits = { 0, 0, 0 };
        private int _wrongAttempts;
        private float _retryAfter;
        private string _targetPadlockCode = "148";
        private int _lastPhoneState=-1, _lastPhoneBattery=-1;
        private KeyCode _lastPhoneKey, _lastScreenKey;
        private PresentationProfile _profile;
        public string CurrentObjective { get; private set; } = string.Empty;
        private AudioSource _voice;
        public string CurrentDialogueSpeaker { get; private set; }
        public bool IsDialoguePlaying => _voice && _voice.isPlaying;
        public bool IsHandsetNotification {get;private set;}
        private readonly HashSet<string> _voicedSpeakers=new HashSet<string>();
        public bool HasPlayedVoice(string speaker)=>_voicedSpeakers.Contains(speaker);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            EnsureRuntimeUIBuilt();
            _profile=PresentationProfile.Load();
            if(_profile&&_profile.audio)
            {
                var voiceObject=new GameObject("DialogueVoice");voiceObject.transform.SetParent(transform,false);
                _voice=voiceObject.AddComponent<AudioSource>();_voice.playOnAwake=false;_voice.dopplerLevel=0;
                _voice.outputAudioMixerGroup=_profile.audio.uiBus;_voice.volume=.85f;
            }
        }

        public void EnsureRuntimeUIBuilt()
        {
            if (objectiveText != null && documentPanel != null && padlockPanel != null && blackFadeImage != null)
            {
                BindUIEvents();
                return;
            }

            Canvas canvas = GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("RuntimeHUDCanvas");
                canvasObj.transform.SetParent(transform, false);
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            if (FindFirstObjectByType<EventSystem>() == null)
            {
                GameObject es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
                es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
                es.AddComponent<StandaloneInputModule>();
#endif
            }

            Font defaultFont = LoadSafeFont();

            GameObject fadeObj = CreateUIObject("BlackFadeOverlay", canvas.transform);
            RectTransform fadeRect = fadeObj.GetComponent<RectTransform>();
            StretchFull(fadeRect);
            blackFadeImage = fadeObj.AddComponent<Image>();
            blackFadeImage.color = new Color(0f, 0f, 0f, 0f);
            blackFadeImage.raycastTarget = false;

            GameObject crosshairObj = CreateUIObject("Crosshair", canvas.transform);
            RectTransform crossRect = crosshairObj.GetComponent<RectTransform>();
            crossRect.sizeDelta = new Vector2(8f, 8f);
            Image crossImg = crosshairObj.AddComponent<Image>();
            crossImg.color = new Color(1f, 1f, 1f, 0.75f);
            crossImg.raycastTarget = false;

            objectiveText = CreateText("ObjectiveText", canvas.transform, defaultFont, 22, TextAnchor.UpperLeft);
            RectTransform objRect = objectiveText.GetComponent<RectTransform>();
            objRect.anchorMin = new Vector2(0f, 1f);
            objRect.anchorMax = new Vector2(0f, 1f);
            objRect.pivot = new Vector2(0f, 1f);
            objRect.anchoredPosition = new Vector2(36f, -32f);
            objRect.sizeDelta = new Vector2(850f, 80f);
            objectiveText.color = new Color(1f, 0.92f, 0.65f);

            phoneStatusText = CreateText("PhoneStatusText", canvas.transform, defaultFont, 18, TextAnchor.UpperRight);
            RectTransform phoneRect = phoneStatusText.GetComponent<RectTransform>();
            phoneRect.anchorMin = new Vector2(1f, 1f);
            phoneRect.anchorMax = new Vector2(1f, 1f);
            phoneRect.pivot = new Vector2(1f, 1f);
            phoneRect.anchoredPosition = new Vector2(-36f, -32f);
            phoneRect.sizeDelta = new Vector2(560f, 110f);
            phoneStatusText.text = "[HP BELUM DIAMBIL]";

            broadcastToastText = CreateText("BroadcastToastText", canvas.transform, defaultFont, 20, TextAnchor.MiddleCenter);
            RectTransform toastRect = broadcastToastText.GetComponent<RectTransform>();
            toastRect.anchorMin = new Vector2(0.5f, 1f);
            toastRect.anchorMax = new Vector2(0.5f, 1f);
            toastRect.pivot = new Vector2(0.5f, 1f);
            toastRect.anchoredPosition = new Vector2(0f, -110f);
            toastRect.sizeDelta = new Vector2(1100f, 110f);
            broadcastToastText.color = new Color(1f, 0.25f, 0.25f);
            broadcastToastText.text = string.Empty;

            interactionPromptText = CreateText("InteractionPromptText", canvas.transform, defaultFont, 22, TextAnchor.MiddleCenter);
            RectTransform promptRect = interactionPromptText.GetComponent<RectTransform>();
            promptRect.anchorMin = new Vector2(0.5f, 0.5f);
            promptRect.anchorMax = new Vector2(0.5f, 0.5f);
            promptRect.anchoredPosition = new Vector2(0f, -65f);
            promptRect.sizeDelta = new Vector2(700f, 50f);
            interactionPromptText.text = string.Empty;

            subtitleText = CreateText("SubtitleText", canvas.transform, defaultFont, 22, TextAnchor.LowerCenter);
            RectTransform subRect = subtitleText.GetComponent<RectTransform>();
            subRect.anchorMin = new Vector2(0.5f, 0f);
            subRect.anchorMax = new Vector2(0.5f, 0f);
            subRect.pivot = new Vector2(0.5f, 0f);
            subRect.anchoredPosition = new Vector2(0f, 48f);
            subRect.sizeDelta = new Vector2(1300f, 140f);
            subtitleText.text = string.Empty;

            documentPanel = CreateModalPanel("DocumentModalPanel", canvas.transform, new Vector2(880f, 520f));
            documentTitleText = CreateText("DocumentTitle", documentPanel.transform, defaultFont, 26, TextAnchor.UpperCenter);
            RectTransform docTitleRect = documentTitleText.GetComponent<RectTransform>();
            docTitleRect.anchoredPosition = new Vector2(0f, 200f);
            docTitleRect.sizeDelta = new Vector2(800f, 50f);
            documentTitleText.color = new Color(1f, 0.85f, 0.4f);
            // Kertas asli (CC0 ambientCG) sebagai latar catatan, bukan panel blank (bug 20).
            documentPaperSprite = Resources.Load<Sprite>("Paper/DocumentPaper");

            documentBodyText = CreateText("DocumentBody", documentPanel.transform, defaultFont, 21, TextAnchor.MiddleLeft);
            RectTransform docBodyRect = documentBodyText.GetComponent<RectTransform>();
            docBodyRect.anchoredPosition = new Vector2(0f, 15f);
            docBodyRect.sizeDelta = new Vector2(780f, 300f);

            documentCloseButton = CreateButton("CloseDocumentButton", documentPanel.transform, defaultFont, "[TUTUP DOKUMEN]", new Vector2(0f, -195f), new Vector2(260f, 50f));
            documentPanel.SetActive(false);

            padlockPanel = CreateModalPanel("PadlockModalPanel", canvas.transform, new Vector2(680f, 460f));
            Text padlockHeader = CreateText("PadlockHeader", padlockPanel.transform, defaultFont, 25, TextAnchor.UpperCenter);
            padlockHeader.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 175f);
            padlockHeader.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, 50f);
            padlockHeader.text = "GEMBOK ANGKA 3 DIGIT — LACI MEJA KERJA";

            padlockDigitsText = CreateText("PadlockDigits", padlockPanel.transform, defaultFont, 44, TextAnchor.MiddleCenter);
            padlockDigitsText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 40f);
            padlockDigitsText.GetComponent<RectTransform>().sizeDelta = new Vector2(460f, 80f);
            padlockDigitsText.color = new Color(0.95f, 0.9f, 0.55f);

            padlockFeedbackText = CreateText("PadlockFeedback", padlockPanel.transform, defaultFont, 19, TextAnchor.MiddleCenter);
            padlockFeedbackText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -55f);
            padlockFeedbackText.GetComponent<RectTransform>().sizeDelta = new Vector2(580f, 45f);

            for (int i = 0; i < 3; i++)
            {
                float xPos = (i - 1) * 140f;
                CreateButton($"DigitUp_{i}", padlockPanel.transform, defaultFont, "+", new Vector2(xPos, 110f), new Vector2(80f, 42f));
                CreateButton($"DigitDown_{i}", padlockPanel.transform, defaultFont, "-", new Vector2(xPos, -20f), new Vector2(80f, 42f));
            }

            CreateButton("UnlockPadlockBtn", padlockPanel.transform, defaultFont, "[BUKA GEMBOK]", new Vector2(-120f, -155f), new Vector2(210f, 48f));
            CreateButton("CancelPadlockBtn", padlockPanel.transform, defaultFont, "[BATAL]", new Vector2(120f, -155f), new Vector2(210f, 48f));
            padlockPanel.SetActive(false);
            BindUIEvents();
        }

        // Scene serialization does not retain AddListener delegates. Bind both saved and newly built UI.
        private void BindUIEvents()
        {
            BindButton(documentCloseButton, CloseDocumentModal);
            foreach (Button button in padlockPanel.GetComponentsInChildren<Button>(true))
            {
                if (button.name == "UnlockPadlockBtn") BindButton(button, SubmitPadlockCode);
                else if (button.name == "CancelPadlockBtn") BindButton(button, ClosePadlockModal);
                else
                {
                    for (int slot = 0; slot < 3; slot++)
                    {
                        int index = slot;
                        if (button.name == $"DigitUp_{slot}") BindButton(button, () => AdjustDigit(index, 1));
                        if (button.name == $"DigitDown_{slot}") BindButton(button, () => AdjustDigit(index, -1));
                    }
                }
            }
        }

        private void BindButton(Button button, UnityEngine.Events.UnityAction callback)
        {
            if (button == null) return;
            if (_buttonCallbacks.TryGetValue(button, out var previous)) button.onClick.RemoveListener(previous);
            button.onClick.AddListener(callback);
            _buttonCallbacks[button] = callback;
        }

        public void SetObjective(string objective)
        {
            // Exploration is guided by the world, dialogue and the player's own notes.
            CurrentObjective = string.Empty;
            if (objectiveText != null)
            {
                objectiveText.text = string.Empty;
                objectiveText.gameObject.SetActive(false);
            }
        }

        public void SetInteractionPrompt(string prompt)
        {
            if (interactionPromptText != null)
            {
                interactionPromptText.text = prompt;
            }
        }

        // A compact sound-meter style stack: green stays on while hidden;
        // yellow and red join it as the threat approaches. Null means outside.
        public void SetThreatBar(float? fill)
        {
            if (!fill.HasValue) { if (threatBarRoot) threatBarRoot.SetActive(false); _lastThreatFill = -2f; return; }
            if (!EnsureThreatBarBuilt()) return;
            float v = Mathf.Clamp01(fill.Value);
            _lastThreatFill = v;
            threatBarRoot.SetActive(true);
            for (int i = 0; i < threatSegments.Length; i++)
            {
                int tier = i / 3;
                bool active = tier == 0 || (tier == 1 ? v > .05f : v >= .7f);
                Color color = tier == 0 ? new Color(.25f,.82f,.32f) : tier == 1 ? new Color(1f,.8f,.12f) : new Color(.95f,.12f,.10f);
                color.a = active ? .95f : .12f;
                threatSegments[i].color = color;
            }
            RefreshThreatExitKey();
        }

        private void RefreshThreatExitKey()
        {
            if (threatExitKeyText == null) return;
            string label = "E";
            var input = GameInputConfig.Instance;
            if (input != null)
            {
                try { string key = input.GetActionLabel(GameAction.Interact); if (!string.IsNullOrEmpty(key)) label = key; }
                catch { }
            }
            if (threatExitKeyText.text != label) threatExitKeyText.text = label;
        }

        private bool EnsureThreatBarBuilt()
        {
            if (threatBarRoot && threatSegments != null) return true;
            Canvas canvas = GetComponentInChildren<Canvas>();
            if (canvas == null) return false;
            Transform prior = canvas.transform.Find("ThreatBar");
            if (prior) { prior.gameObject.SetActive(false); Destroy(prior.gameObject); }
            threatBarRoot = CreateUIObject("ThreatBar", canvas.transform);
            var rect = threatBarRoot.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(1,0);
            rect.anchoredPosition = new Vector2(-28,32);
            rect.sizeDelta = new Vector2(42,130);
            threatSegments = new Image[9];
            for (int i = 0; i < threatSegments.Length; i++)
            {
                var segment = CreateUIObject("ThreatSegment" + i, threatBarRoot.transform);
                var sr = segment.GetComponent<RectTransform>();
                sr.anchorMin = sr.anchorMax = new Vector2(.5f,0);
                sr.anchoredPosition = new Vector2(0,36+i*10);
                sr.sizeDelta = new Vector2(28,6);
                threatSegments[i] = segment.AddComponent<Image>();
                threatSegments[i].raycastTarget = false;
            }
            threatBarFill = threatSegments[0];
            threatExitKeyText = CreateText("ThreatExitKey", threatBarRoot.transform, LoadSafeFont(), 16, TextAnchor.MiddleCenter);
            var kr = threatExitKeyText.GetComponent<RectTransform>();
            kr.anchorMin = kr.anchorMax = new Vector2(.5f,0);
            kr.anchoredPosition = new Vector2(0,12);
            kr.sizeDelta = new Vector2(36,24);
            threatExitKeyText.color = new Color(1f,.92f,.65f);
            threatBarRoot.SetActive(false);
            return true;
        }

        public float EnqueueSubtitle(string speaker, string text, float duration = 4.5f)
        {
            var clip=_profile?_profile.Voice(speaker,text):null;
            if(clip)duration=Mathf.Max(duration,clip.length+.25f);
            _subtitleQueue.Enqueue((speaker, text, duration, null));
            if (_subtitleRoutine == null)
            {
                _subtitleRoutine = StartCoroutine(ProcessSubtitlesRoutine());
            }
            return duration;
        }
        public float PlayPriorityDialogue(string speaker,string text,float duration)
        {
            // A phone call must connect now, not wait behind repeated optional item remarks.
            _subtitleQueue.Clear();if(_subtitleRoutine!=null){StopCoroutine(_subtitleRoutine);_subtitleRoutine=null;}
            if(_voice)_voice.Stop();return EnqueueSubtitle(speaker,text,duration);
        }

        public float PlayPriorityWorldDialogue(string speaker,string text,float duration,Vector3 position)
        {
            _subtitleQueue.Clear();if(_subtitleRoutine!=null){StopCoroutine(_subtitleRoutine);_subtitleRoutine=null;}
            if(_voice)_voice.Stop();var clip=_profile?_profile.Voice(speaker,text):null;
            if(clip)duration=Mathf.Max(duration,clip.length+.25f);
            _subtitleQueue.Enqueue((speaker,text,duration,position));_subtitleRoutine=StartCoroutine(ProcessSubtitlesRoutine());return duration;
        }

        public void ShowBroadcastNotification(string message, float duration = 8f)
        {
            if (broadcastToastText == null) return;
            IsHandsetNotification=message.Contains("Penonton_Terakhir")||message.Contains("SAWER")||message.Contains("DONASI");
            if (_broadcastRoutine != null) StopCoroutine(_broadcastRoutine);
            broadcastToastText.text = message.Replace("[GLITCH LAYAR 1 DETIK — KRING!]\n",string.Empty)
                .Replace("[NOTIFIKASI DONASI MASUK: ",string.Empty).Replace("Penonton_Terakhir]","Penonton_Terakhir");
            _broadcastRoutine = StartCoroutine(ClearBroadcastRoutine(duration));
            var view=FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaPhoneView>();
            if(IsHandsetNotification&&view&&view.Apps)view.Apps.Notify(broadcastToastText.text);
        }

        public void UpdatePhoneStatusUI(SmartphoneController phone)
        {
            if (phoneStatusText == null || phone == null)
                return;

            if (!phone.hasSmartphoneInInventory)
            {
                phoneStatusText.text = string.Empty;
                _lastPhoneState=-1;
                return;
            }
            int state=(phone.isPhoneRaised?1:0)|(phone.isScreenOn?2:0)|(phone.isBroadcastConnected?4:0)|((int)phone.currentScanMode<<3)|(phone.RitualCameraPower?64:0);
            int battery=Mathf.Clamp(Mathf.CeilToInt(phone.DisplayBatteryPercent),0,100);
            var input=GameInputConfig.Instance;
            var phoneKey=input?input.togglePhoneKey:KeyCode.Tab;
            var screenKey=input?input.toggleScreenKey:KeyCode.F;
            if(state==_lastPhoneState&&battery==_lastPhoneBattery&&phoneKey==_lastPhoneKey&&screenKey==_lastScreenKey)return;
            _lastPhoneState=state;_lastPhoneBattery=battery;_lastPhoneKey=phoneKey;_lastScreenKey=screenKey;
            string mode=phone.currentScanMode==ScanVisualMode.AR?"MataBatin AR":phone.currentScanMode==ScanVisualMode.UV?"Lensa UV":"Kamera biasa";
            string screen=!phone.isPhoneRaised?"Tersimpan":phone.isScreenOn?mode:"Layar mati · Hemat daya";
            string energy=phone.RitualCameraPower?"Kapasitor ritual 5%":$"{battery}%";
            phoneStatusText.text=$"{energy}    {(phone.isBroadcastConnected?"LIVE":"Offline")}\n{screen}";
            var presentation=GetComponent<HudPresentation>();
            if(presentation&&presentation.phoneHints)
                presentation.phoneHints.text=phone.isPhoneRaised
                    ?$"{phoneKey.ToString().ToUpperInvariant()}  Simpan   ·   {screenKey.ToString().ToUpperInvariant()}  {(phone.isScreenOn?"Matikan layar":"Nyalakan layar")}"
                    :"KLIK KANAN  Angkat kamera   ·   "+phoneKey.ToString().ToUpperInvariant()+"  Aplikasi";
        }

        public void OpenDocumentModal(string title, string body, Action onClosed = null, string documentId = null)
        {
            if (documentPanel == null) return;
            var campaign = CampaignState.Instance;
            if (campaign && !campaign.ArchiveDocument(title, body, documentId))
                ShowBroadcastNotification("Catatan belum tersimpan di HP. Penyimpanan tidak tersedia.", 4);
            // Reading preserves the handset, screen, camera app and live connection.
            UiFocus.Phone=false;
            _onDocumentClosed = onClosed;
            documentTitleText.text = title;
            documentBodyText.text = body;
            // Authored HUD prefabs bypass runtime UI creation; resolve the paper
            // here as well so the reader never falls back to a flat cream panel.
            if (!documentPaperSprite) documentPaperSprite = Resources.Load<Sprite>("Paper/DocumentPaper");
            if (documentPaperSprite != null)
            {
                var bg = documentPanel.GetComponent<Image>();
                if (bg != null) { bg.type = Image.Type.Simple; bg.sprite = documentPaperSprite; bg.color = Color.white; }
                documentTitleText.color = new Color(0.23f, 0.16f, 0.10f);
                documentBodyText.color = new Color(0.16f, 0.13f, 0.10f);
            }
            var closeLabel=documentCloseButton.GetComponentInChildren<Text>();if(closeLabel)closeLabel.text="TUTUP DOKUMEN";
            var documentRect=(RectTransform)documentPanel.transform;
            bool holdingPhone=SmartphoneController.Instance&&SmartphoneController.Instance.isPhoneRaised;
            documentRect.anchorMin=holdingPhone?new Vector2(.055f,.14f):new Vector2(.5f,.5f);
            documentRect.anchorMax=holdingPhone?new Vector2(.68f,.86f):new Vector2(.5f,.5f);
            documentRect.anchoredPosition=Vector2.zero;
            documentRect.sizeDelta=holdingPhone?Vector2.zero:new Vector2(880,520);
            documentPanel.SetActive(true);
            var presentation=GetComponent<HudPresentation>();
            if(presentation&&presentation.documentScroll)
            {
                Canvas.ForceUpdateCanvases();
                presentation.documentScroll.StopMovement();
                presentation.documentScroll.verticalNormalizedPosition=1;
            }
            SetPlayerLockForModal(true);
        }

        public void CloseDocumentModal()
        {
            if (documentPanel == null) return;
            documentPanel.SetActive(false);
            SetPlayerLockForModal(false);
            Action callback = _onDocumentClosed;
            _onDocumentClosed = null;
            callback?.Invoke();
        }

        public void OpenPadlockModal(string targetCode, Action onSolved)
        {
            if (padlockPanel == null) return;
            LowerPhoneForWorldObject();
            _targetPadlockCode = targetCode;
            _onPadlockSolved = onSolved;
            _digits = new int[targetCode.Length];
            ConfigureDigitWheels();
            padlockFeedbackText.text = "Putar roda angka. Temukan petunjuknya di kamar.";
            RefreshPadlockDisplay();
            padlockPanel.SetActive(true);
            SetPlayerLockForModal(true);
        }

        public void ClosePadlockModal()
        {
            if (padlockPanel == null) return;
            _onPadlockSolved = null;
            padlockPanel.SetActive(false);
            SetPlayerLockForModal(false);
        }

        public void SetFadeAlpha(float alpha)
        {
            if (blackFadeImage != null)
            {
                blackFadeImage.transform.SetAsLastSibling();
                Color c = blackFadeImage.color;
                c.a = Mathf.Clamp01(alpha);
                blackFadeImage.color = c;
            }
        }

        public Coroutine FadeBlack(float fromAlpha, float toAlpha, float duration)
        {
            return StartCoroutine(FadeRoutine(fromAlpha, toAlpha, duration));
        }

        private void AdjustDigit(int slot, int delta)
        {
            _digits[slot] = (_digits[slot] + delta + 10) % 10;
            RefreshPadlockDisplay();
        }

        private void RefreshPadlockDisplay()
        {
            if (padlockDigitsText != null)
            {
                padlockDigitsText.text = string.Join("       ",_digits);
            }
        }

        private void SubmitPadlockCode()
        {
            if (padlockPanel == null || !padlockPanel.activeSelf) return;
            if(Time.unscaledTime<_retryAfter){padlockFeedbackText.text="Mekanisme tersangkut. Tunggu sebentar.";return;}
            string entered = string.Concat(_digits);
            if (entered == _targetPadlockCode)
            {
                Action callback = _onPadlockSolved;
                ClosePadlockModal();
                callback?.Invoke();
            }
            else if (padlockFeedbackText != null)
            {
                _wrongAttempts++;if(_wrongAttempts%3==0)_retryAfter=Time.unscaledTime+3;
                padlockFeedbackText.text = "Kombinasi salah. Tidak ada bunyi klik.";
            }
        }

        // Inspecting a physical object lowers the handset. Closing the drawer cannot leave
        // invisible phone-app focus consuming WASD, mouse look and interaction.
        private void LowerPhoneForWorldObject()
        {
            var phone=SmartphoneController.Instance;
            if(phone){phone.isPhoneRaised=false;phone.SyncVisualState();}
            UiFocus.Phone=false;
        }
        private void ConfigureDigitWheels()
        {
            var presentation=GetComponent<HudPresentation>();
            if(!presentation)return;
            foreach(var t in padlockPanel.GetComponentsInChildren<Transform>(true))
                if(t.name.StartsWith("DigitCell_"))t.gameObject.SetActive(false);
                else if((t.name.StartsWith("DigitUp_")||t.name.StartsWith("DigitDown_"))&&int.TryParse(t.name.Substring(t.name.LastIndexOf('_')+1),out int slot))t.gameObject.SetActive(slot<_digits.Length);
            var header=padlockPanel.transform.Find("PadlockHeader").GetComponent<Text>();header.text="LACI MEJA KERJA · "+_digits.Length+" RODA";
            presentation.digitDisplays=new Text[_digits.Length];
            var separator=padlockPanel.transform.Find("DateSeparator");if(separator)separator.gameObject.SetActive(_digits.Length==4);
            else if(_digits.Length==4)
            {var dash=UiWidgets.Text("DateSeparator",padlockPanel.transform,"–",Vector2.zero,Vector2.zero,32,TextAnchor.MiddleCenter);dash.rectTransform.anchorMin=dash.rectTransform.anchorMax=new Vector2(.5f,.5f);dash.rectTransform.anchoredPosition=new Vector2(0,45);dash.rectTransform.sizeDelta=new Vector2(24,82);}
            for(int i=0;i<_digits.Length;i++)
            {
                float x=(i-(_digits.Length-1)*.5f)*115;
                foreach(bool up in new[]{true,false})
                {
                    string name=(up?"DigitUp_":"DigitDown_")+i;
                    var child=padlockPanel.transform.Find(name);
                    var button=child?child.GetComponent<Button>():CreateButton(name,padlockPanel.transform,LoadSafeFont(),up?"+":"−",Vector2.zero,new Vector2(80,42));
                    button.gameObject.SetActive(true);button.GetComponent<RectTransform>().anchoredPosition=new Vector2(x,up?110:-20);
                    int slot=i;BindButton(button,()=>AdjustDigit(slot,up?1:-1));
                }
                var cell=UiWidgets.Panel("DigitCell_Runtime_"+i,padlockPanel.transform,Vector2.zero,Vector2.zero,new Color(.11f,.125f,.11f));
                var rect=cell.GetComponent<RectTransform>();rect.anchorMin=rect.anchorMax=new Vector2(.5f,.5f);rect.anchoredPosition=new Vector2(x,45);rect.sizeDelta=new Vector2(84,82);
                presentation.digitDisplays[i]=UiWidgets.Text("Digit",cell.transform,"0",Vector2.zero,Vector2.one,48,TextAnchor.MiddleCenter);
            }
        }

        private void SetPlayerLockForModal(bool modalOpen)
        {
            FirstPersonController fpp = FindFirstObjectByType<FirstPersonController>();
            if (fpp != null)
            {
                fpp.SetCursorLock(!modalOpen);
            }
        }

        private IEnumerator ProcessSubtitlesRoutine()
        {
            while (_subtitleQueue.Count > 0)
            {
                var (speaker, line, duration, position) = _subtitleQueue.Dequeue();
                CurrentDialogueSpeaker=speaker;
                var clip=_profile?_profile.Voice(speaker,line):null;
                if(clip&&_voice){_voice.spatialBlend=position.HasValue?1:0;if(position.HasValue){_voice.transform.position=position.Value;_voice.minDistance=2;_voice.maxDistance=24;}_voice.clip=clip;_voice.Play();_voicedSpeakers.Add(speaker);}
                if (subtitleText != null)
                {
                    subtitleText.text = !GameShell.Subtitles?string.Empty:speaker=="SFX" ? $"<i>{line.Trim('[',']')}</i>" :
                        string.IsNullOrEmpty(speaker)?line:$"<size=18><b>{speaker.ToUpperInvariant()}</b></size>\n{line}";
                }
                yield return new WaitForSeconds(duration);
            }

            if (subtitleText != null)
                subtitleText.text = string.Empty;

            _subtitleRoutine = null;
            CurrentDialogueSpeaker=string.Empty;
        }

        private IEnumerator ClearBroadcastRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (broadcastToastText != null)
                broadcastToastText.text = string.Empty;
            _broadcastRoutine = null;
        }

        private IEnumerator FadeRoutine(float fromAlpha, float toAlpha, float duration)
        {
            SetFadeAlpha(fromAlpha);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                SetFadeAlpha(Mathf.Lerp(fromAlpha, toAlpha, elapsed / Mathf.Max(0.01f, duration)));
                yield return null;
            }
            SetFadeAlpha(toAlpha);
        }

        private static Font LoadSafeFont()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null) font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (font == null) font = Font.CreateDynamicFontFromOSFont("Liberation Sans", 18);
            return font;
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            return go;
        }

        private static void StretchFull(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static Text CreateText(string name, Transform parent, Font font, int size, TextAnchor anchor)
        {
            GameObject go = CreateUIObject(name, parent);
            Text txt = go.AddComponent<Text>();
            txt.font = font;
            txt.fontSize = size;
            txt.alignment = anchor;
            txt.color = Color.white;
            txt.horizontalOverflow = HorizontalWrapMode.Wrap;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
            txt.raycastTarget = false;
            return txt;
        }

        private static GameObject CreateModalPanel(string name, Transform parent, Vector2 size)
        {
            GameObject panel = CreateUIObject(name, parent);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            Image bg = panel.AddComponent<Image>();
            bg.color = new Color(0.06f, 0.06f, 0.08f, 0.95f);
            return panel;
        }

        private static Button CreateButton(string name, Transform parent, Font font, string label, Vector2 pos, Vector2 size)
        {
            GameObject btnObj = CreateUIObject(name, parent);
            RectTransform rect = btnObj.GetComponent<RectTransform>();
            rect.anchoredPosition = pos;
            rect.sizeDelta = size;

            Image img = btnObj.AddComponent<Image>();
            img.color = new Color(0.22f, 0.22f, 0.26f, 1f);

            Button btn = btnObj.AddComponent<Button>();
            btn.targetGraphic = img;

            Text txt = CreateText("Label", btnObj.transform, font, 18, TextAnchor.MiddleCenter);
            StretchFull(txt.GetComponent<RectTransform>());
            txt.text = label;
            return btn;
        }
    }
}
