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

        public GameObject padlockPanel;
        public Text padlockDigitsText;
        public Text padlockFeedbackText;

        public Image blackFadeImage;

        [Header("Threat Bar")]
        public Image threatBarFill;
        public Text threatExitKeyText;
        private GameObject threatBarRoot;
        private GameObject prologThreatMeter;
        private GameObject prologWardrobeSlats;
        private Text prologThreatExitKey;
        private Image[] prologThreatSegments;
        private static Sprite _notebookSheetSprite;
        private static readonly System.Collections.Generic.Dictionary<DocPaper, Sprite> _paperSprites = new System.Collections.Generic.Dictionary<DocPaper, Sprite>();
        private static Font _kalamFont, _courierFont;
        public enum DocPaper { Notebook, RedEnvelope, BloodyNote, Newsprint, DiaryNote, WillNote, WallCalendar }
        // Paper + font routing per playtest notes: the kos letter arrives in a red
        // envelope, Damar's survey/diary/will match the grimy Outlast reference
        // (handwriting, blood spots, crumpled), the 1998 clipping is typewritten
        // newsprint, and Raka's wall calendar opens as its own calendar sheet
        // (red month band, ringed 17th, handwritten note) instead of plain text.
        // Everything else keeps the lined notebook.
        public static DocPaper PaperFor(string documentId, string title)
        {
            if (documentId == "DOC_KOS_KALENDER_AGUSTUS" || (title ?? "").Contains("KALENDER")) return DocPaper.WallCalendar;
            if (documentId == "DOC_PROLOG_01_SURAT_KOS") return DocPaper.RedEnvelope;
            if (documentId == "DOC_PROLOG_02_SURVEI_DAMAR" || (title ?? "").Contains("DOKUMEN #2") || (title ?? "").Contains("SURVEI DAMAR")) return DocPaper.BloodyNote;
            if (documentId == "DOC_PROLOG_03_KORAN_1998" || (title ?? "").Contains("DOKUMEN #3") || (title ?? "").Contains("KORAN 1998")) return DocPaper.Newsprint;
            if (documentId == "DOC_PROLOG_04_DIARY_BIMA" || (title ?? "").Contains("DOKUMEN #4") || (title ?? "").Contains("HARIAN BIMA")) return DocPaper.DiaryNote;
            if (documentId == "DOC_PROLOG_05_WASIAT_NYONYA" || (title ?? "").Contains("DOKUMEN #5") || (title ?? "").Contains("WASIAT NYONYA")) return DocPaper.WillNote;
            return DocPaper.Notebook;
        }
        public static bool IsAtmajaNoteDocument(string documentId, string title) => PaperFor(documentId, title) != DocPaper.Notebook;
        public static Font DocFont(DocPaper paper)
        {
            if (paper == DocPaper.Newsprint)
                return _courierFont ? _courierFont : (_courierFont = Resources.Load<Font>("Fonts/CourierPrime-Regular"));
            if (paper == DocPaper.BloodyNote || paper == DocPaper.DiaryNote || paper == DocPaper.WillNote || paper == DocPaper.WallCalendar)
                return _kalamFont ? _kalamFont : (_kalamFont = Resources.Load<Font>("Fonts/Kalam-Regular"));
            return null;
        }
        public static Sprite DocPaperSprite(DocPaper paper)
        {
            if (_paperSprites.TryGetValue(paper, out var cached) && cached) return cached;
            var made = GeneratePaper(paper);
            _paperSprites[paper] = made;
            return made;
        }
        public static Sprite AtmajaNoteSprite() => DocPaperSprite(DocPaper.BloodyNote);
        // Deterministic runtime paper: crumpled folds, blood spots / ink smudges,
        // grain and edge vignette. Generated in code so no binary art is required.
        private static Sprite GeneratePaper(DocPaper paper)
        {
            Color baseCol; int seed; float blood, smudge, crease;
            switch (paper)
            {
                case DocPaper.RedEnvelope: baseCol = new Color(.45f, .08f, .07f); seed = 11; blood = 0f; smudge = .25f; crease = .5f; break;
                case DocPaper.Newsprint: baseCol = new Color(.68f, .67f, .61f); seed = 13; blood = 0f; smudge = 1f; crease = .8f; break;
                case DocPaper.DiaryNote: baseCol = new Color(.64f, .61f, .53f); seed = 14; blood = .8f; smudge = .3f; crease = 1f; break;
                case DocPaper.WillNote: baseCol = new Color(.60f, .57f, .49f); seed = 15; blood = .6f; smudge = .4f; crease = .9f; break;
                case DocPaper.WallCalendar: baseCol = new Color(.80f, .76f, .64f); seed = 16; blood = 0f; smudge = .3f; crease = .25f; break;
                default: baseCol = new Color(.66f, .64f, .56f); seed = 12; blood = 1f; smudge = .3f; crease = 1f; break;
            }
            const int size = 384;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false, false);
            tex.filterMode = FilterMode.Bilinear; tex.wrapMode = TextureWrapMode.Clamp;
            var rand = new System.Random(seed * 7919 + 17);
            float w1 = 2f + (float)rand.NextDouble() * 3f, w2 = 2f + (float)rand.NextDouble() * 3f;
            float p1 = (float)rand.NextDouble() * 6.28f, p2 = (float)rand.NextDouble() * 6.28f;
            int creases = 4 + rand.Next(4);
            var lines = new Vector4[creases];
            for (int i = 0; i < creases; i++)
            {
                float a = (float)rand.NextDouble() * 6.28f, off = (float)rand.NextDouble() * 2f - 1f;
                lines[i] = new Vector4(Mathf.Cos(a), Mathf.Sin(a), off, .5f + (float)rand.NextDouble());
            }
            int blobs = Mathf.RoundToInt(blood * 3.2f);
            var blot = new Vector4[6];
            for (int i = 0; i < blot.Length; i++)
            {
                bool edgeZone = rand.NextDouble() < .7;
                float bx = edgeZone ? (rand.NextDouble() < .5 ? (float)rand.NextDouble() * .28f : .72f + (float)rand.NextDouble() * .28f) : (float)rand.NextDouble();
                float by = (float)rand.NextDouble();
                blot[i] = new Vector4(bx, by, 10f + (float)rand.NextDouble() * 20f, i < blobs ? 1f : 0f);
            }
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float u = x / (float)(size - 1), v = y / (float)(size - 1);
                    float shade = .90f + .10f * Mathf.Sin(u * w1 + p1) * Mathf.Sin(v * w2 + p2);
                    float grain = ((float)rand.NextDouble() - .5f) * .09f;
                    float r = baseCol.r * shade + grain, g = baseCol.g * shade + grain, b = baseCol.b * shade + grain;
                    float edge = Mathf.Min(1f, Mathf.Min(u, 1f - u) * 4f) * Mathf.Min(1f, Mathf.Min(v, 1f - v) * 4f);
                    float tone = .62f + .38f * edge;
                    r *= tone; g *= tone; b *= tone;
                    if (crease > 0)
                        for (int i = 0; i < lines.Length; i++)
                        {
                            float dist = Mathf.Abs(u * lines[i].x + v * lines[i].y - lines[i].z);
                            float wdt = .004f * lines[i].w;
                            if (dist < wdt * 3f)
                            {
                                float k = (1f - dist / (wdt * 3f)) * crease;
                                float lift = dist < wdt ? .10f * k : -.08f * k;
                                r += lift; g += lift; b += lift;
                            }
                        }
                    if (blood > 0)
                        for (int i = 0; i < blot.Length; i++)
                        {
                            if (blot[i].w <= 0) continue;
                            float dx = (u - blot[i].x) * size, dy = (v - blot[i].y) * size;
                            float dd = Mathf.Sqrt(dx * dx + dy * dy);
                            if (dd < blot[i].z)
                            {
                                float a = (1f - dd / blot[i].z) * .75f * blood;
                                r = r * (1 - a) + .30f * a; g = g * (1 - a) + .035f * a; b = b * (1 - a) + .025f * a;
                            }
                            else if (dd < blot[i].z * 3.0f && rand.NextDouble() < .05)
                            {
                                r = r * .6f + .30f * .4f; g = g * .6f + .035f * .4f; b = b * .6f + .025f * .4f;
                            }
                        }
                    if (smudge > 0 && rand.NextDouble() < .0016 * smudge)
                    {
                        float k = .25f * smudge;
                        r = r * (1 - k) + .12f * k; g = g * (1 - k) + .11f * k; b = b * (1 - k) + .10f * k;
                    }
                    // Wall calendar: printed red month band across the sheet top.
                    // The modal title sits on this band; the grid + note live below.
                    if (paper == DocPaper.WallCalendar && v > .84f)
                    {
                        float band = .85f + .15f * Mathf.Sin(u * 40f);
                        r = r * .15f + .60f * band; g = g * .15f + .10f * band; b = b * .15f + .08f * band;
                    }
                    tex.SetPixel(x, y, new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), 1f));
                }
            tex.Apply(false, false);
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(.5f, .5f), 100f);
        }
        // Test helper: does the generated paper actually carry blood?
        public static bool DocPaperHasBlood(DocPaper paper)
        {
            var sp = DocPaperSprite(paper);
            if (!sp || !sp.texture) return false;
            var t = sp.texture;
            double sum = 0; int n = 0;
            for (int y = 0; y < t.height; y += 8)
                for (int x = 0; x < t.width; x += 8)
                { sum += t.GetPixel(x, y).r; n++; }
            double avg = sum / Mathf.Max(1, n);
            if (avg < .38) return false;
            int hits = 0;
            for (int y = 0; y < t.height; y += 4)
                for (int x = 0; x < t.width; x += 4)
                {
                    var c = t.GetPixel(x, y);
                    if (c.r > c.g + .12f && c.r > c.b + .12f && c.r < avg * .8f) hits++;
                }
            return hits > 8;
        }
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
                // Baked scene HUDs predate the round-dot fix: enforce it on
                // every load, not just on first build.
                EnforceCrosshair();
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
            // Designer Nov: the center dot must be ROUND, not a square.
            // Generated circle sprite with a soft edge; 12px idle reads
            // without covering small pickups at range.
            crossRect.anchorMin = new Vector2(0.5f, 0.5f);
            crossRect.anchorMax = new Vector2(0.5f, 0.5f);
            crossRect.pivot = new Vector2(0.5f, 0.5f);
            crossRect.anchoredPosition = Vector2.zero;
            crossRect.sizeDelta = new Vector2(8f, 8f);
            Image crossImg = crosshairObj.AddComponent<Image>();
            crossImg.sprite = CircleSprite();
            crossImg.color = new Color(1f, 1f, 1f, 0.9f);
            crossImg.raycastTarget = false;
            EnforceCrosshair();

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

        // Prolog wardrobe: cumulative green / yellow / red segments at lower right,
        // framed in a dark VU-meter housing with [E] Keluar badge and subtle wooden wardrobe louvers.
        public void SetPrologThreatMeter(float? danger)
        {
            if(!danger.HasValue)
            {
                if(prologThreatMeter)prologThreatMeter.SetActive(false);
                if(prologWardrobeSlats)prologWardrobeSlats.SetActive(false);
                return;
            }
            var canvas=GetComponentInChildren<Canvas>();if(!canvas)return;
            if(!prologWardrobeSlats)
            {
                prologWardrobeSlats=CreateUIObject("PrologWardrobeSlats",canvas.transform);
                var slatsRect=prologWardrobeSlats.GetComponent<RectTransform>();
                slatsRect.anchorMin=Vector2.zero;slatsRect.anchorMax=Vector2.one;
                slatsRect.offsetMin=slatsRect.offsetMax=Vector2.zero;
                // Left/right heavy wooden wardrobe stiles + horizontal louver slats with faint crevice light between them
                void Bar(string name,Vector2 aMin,Vector2 aMax,Color col)
                {
                    var b=CreateUIObject(name,prologWardrobeSlats.transform);
                    var r=b.GetComponent<RectTransform>();r.anchorMin=aMin;r.anchorMax=aMax;r.offsetMin=r.offsetMax=Vector2.zero;
                    var img=b.AddComponent<Image>();img.color=col;img.raycastTarget=false;
                }
                Color woodDark=new Color(.11f,.072f,.045f,.96f);
                Color woodMid=new Color(.165f,.112f,.068f,.94f);
                Color woodBevel=new Color(.27f,.185f,.115f,.85f);
                Color crevice=new Color(.20f,.24f,.22f,.14f);
                Bar("LeftStile",new Vector2(0,0),new Vector2(.135f,1),woodDark);
                Bar("LeftStileEdge",new Vector2(.135f,0),new Vector2(.141f,1),woodBevel);
                Bar("RightStileEdge",new Vector2(.859f,0),new Vector2(.865f,1),woodBevel);
                Bar("RightStile",new Vector2(.865f,0),new Vector2(1,1),woodDark);
                Bar("CenterMullion",new Vector2(.486f,0),new Vector2(.514f,1),woodDark);
                for(int s=0;s<7;s++)
                {
                    float y0=s/7f;
                    Bar("LouverWood_"+s,new Vector2(.141f,y0),new Vector2(.859f,Mathf.Min(1f,y0+.084f)),s%2==0?woodDark:woodMid);
                    Bar("LouverBevel_"+s,new Vector2(.141f,Mathf.Min(1f,y0+.080f)),new Vector2(.859f,Mathf.Min(1f,y0+.085f)),woodBevel);
                    Bar("LouverCrevice_"+s,new Vector2(.145f,Mathf.Min(1f,y0+.085f)),new Vector2(.855f,Mathf.Min(1f,y0+.142f)),crevice);
                }
                prologWardrobeSlats.transform.SetAsFirstSibling();
            }
            if(!prologThreatMeter)
            {
                prologThreatMeter=CreateUIObject("PrologThreatMeter",canvas.transform);
                var rect=prologThreatMeter.GetComponent<RectTransform>();
                rect.anchorMin=rect.anchorMax=new Vector2(1,0);rect.pivot=new Vector2(1,0);
                rect.anchoredPosition=new Vector2(-24,26);rect.sizeDelta=new Vector2(148,136);
                var housing=prologThreatMeter.AddComponent<Image>();
                housing.color=new Color(.035f,.042f,.04f,.88f);housing.raycastTarget=false;
                var border=CreateUIObject("HousingAccent",prologThreatMeter.transform);
                var bRect=border.GetComponent<RectTransform>();bRect.anchorMin=new Vector2(1,0);bRect.anchorMax=new Vector2(1,1);
                bRect.pivot=new Vector2(1,.5f);bRect.anchoredPosition=Vector2.zero;bRect.sizeDelta=new Vector2(3,0);
                var bImg=border.AddComponent<Image>();bImg.color=new Color(.42f,.36f,.24f,.7f);bImg.raycastTarget=false;

                var fnt=objectiveText?objectiveText.font:Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                var titleGo=CreateUIObject("MeterLabel",prologThreatMeter.transform);
                var tRect=titleGo.GetComponent<RectTransform>();tRect.anchorMin=tRect.anchorMax=new Vector2(0,1);
                tRect.pivot=new Vector2(0,1);tRect.anchoredPosition=new Vector2(12,-10);tRect.sizeDelta=new Vector2(96,20);
                var tTxt=titleGo.AddComponent<Text>();tTxt.font=fnt;tTxt.fontSize=13;tTxt.alignment=TextAnchor.UpperLeft;
                tTxt.color=new Color(.78f,.76f,.68f,.9f);tTxt.text="ANCAMAN";tTxt.raycastTarget=false;

                var badgeGo=CreateUIObject("ExitKeyBadge",prologThreatMeter.transform);
                var kRect=badgeGo.GetComponent<RectTransform>();kRect.anchorMin=kRect.anchorMax=new Vector2(0,0);
                kRect.pivot=Vector2.zero;kRect.anchoredPosition=new Vector2(12,12);kRect.sizeDelta=new Vector2(92,34);
                var kBg=badgeGo.AddComponent<Image>();kBg.color=new Color(.12f,.13f,.12f,.95f);kBg.raycastTarget=false;
                var kTxtGo=CreateUIObject("ExitKeyText",badgeGo.transform);
                var ktRect=kTxtGo.GetComponent<RectTransform>();ktRect.anchorMin=Vector2.zero;ktRect.anchorMax=Vector2.one;
                ktRect.offsetMin=ktRect.offsetMax=Vector2.zero;
                prologThreatExitKey=kTxtGo.AddComponent<Text>();prologThreatExitKey.font=fnt;prologThreatExitKey.fontSize=15;
                prologThreatExitKey.alignment=TextAnchor.MiddleCenter;prologThreatExitKey.color=new Color(.94f,.92f,.84f,1f);
                prologThreatExitKey.text="[E] Keluar";prologThreatExitKey.raycastTarget=false;

                prologThreatSegments=new Image[12];
                for(int i=0;i<12;i++)
                {
                    var go=CreateUIObject("ThreatSegment_"+i,prologThreatMeter.transform);
                    var segment=go.GetComponent<RectTransform>();segment.anchorMin=segment.anchorMax=new Vector2(1,0);
                    segment.pivot=new Vector2(1,0);segment.anchoredPosition=new Vector2(-12,12+i*9.2f);segment.sizeDelta=new Vector2(24,6.5f);
                    prologThreatSegments[i]=go.AddComponent<Image>();prologThreatSegments[i].raycastTarget=false;
                }
            }
            if(threatBarRoot)threatBarRoot.SetActive(false);
            prologWardrobeSlats.SetActive(true);
            prologWardrobeSlats.transform.SetAsFirstSibling();
            prologThreatMeter.SetActive(true);
            prologThreatMeter.transform.SetAsLastSibling();
            if(prologThreatExitKey)
            {
                string key="E";
                if(GameInputConfig.Instance!=null){try{string k=GameInputConfig.Instance.GetActionLabel(GameAction.Interact);if(!string.IsNullOrEmpty(k))key=k;}catch{}}
                prologThreatExitKey.text=$"[{key}] Keluar";
            }
            float value=Mathf.Clamp01(danger.Value);
            int active=value>=.7f?12:value>=.3f?8:Mathf.Max(2,Mathf.RoundToInt(value*13f));
            for(int i=0;i<12;i++)
            {
                Color color=i<4?new Color(.22f,.78f,.32f):i<8?new Color(.96f,.78f,.16f):new Color(.92f,.14f,.12f);
                prologThreatSegments[i].color=i<active?color:new Color(color.r*.18f,color.g*.18f,color.b*.18f,.62f);
            }
        }

        public bool IsPrologThreatMeterVisible=>prologThreatMeter!=null&&prologThreatMeter.activeSelf;

        // Threat proximity bar for hiding spots. Null hides everything.
        // Fill = danger: full red = threat at the locker, green sliver = far,
        // bar gone = quiet = all-clear. The E keycap below always shows while
        // hidden so the exit key is never forgotten. HUD-owned, never the
        // phone screen — visible with the handset lowered or off.
        public void SetThreatBar(float? fill)
        {
            if (!fill.HasValue) { if (threatBarRoot) threatBarRoot.SetActive(false); _lastThreatFill = -2f; return; }
            if (!EnsureThreatBarBuilt()) return;
            float v = Mathf.Clamp01(fill.Value);
            if (Mathf.Approximately(v, _lastThreatFill) && threatBarRoot.activeSelf) return;
            _lastThreatFill = v;
            threatBarRoot.SetActive(true);
            if (threatBarFill) { threatBarFill.gameObject.SetActive(v > 0.001f); threatBarFill.fillAmount = v; threatBarFill.color = ThreatBarColor(v); }
            RefreshThreatExitKey();
        }

        private static Color ThreatBarColor(float v)
        {
            Color green = new Color(0.25f, 0.75f, 0.25f, 0.92f);
            Color yellow = new Color(0.95f, 0.8f, 0.15f, 0.92f);
            Color red = new Color(0.78f, 0.07f, 0.06f, 0.95f);
            if (v < 0.5f) return Color.Lerp(green, yellow, v * 2f);
            return Color.Lerp(yellow, red, (v - 0.5f) * 2f);
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
            if (threatBarRoot) return true;
            Canvas canvas = GetComponentInChildren<Canvas>();
            if (canvas == null) return false;
            Transform existing = canvas.transform.Find("ThreatBar");
            if (existing != null)
            {
                threatBarRoot = existing.gameObject;
                Transform fillT = existing.Find("ThreatBarFill");
                if (fillT != null) threatBarFill = fillT.GetComponent<Image>();
                Transform keyT = existing.Find("ThreatExitKeyFrame/ThreatExitKeyInner/ThreatExitKey");
                if (keyT != null) threatExitKeyText = keyT.GetComponent<Text>();
                return threatBarFill != null;
            }
            threatBarRoot = CreateUIObject("ThreatBar", canvas.transform);
            RectTransform rootRect = threatBarRoot.GetComponent<RectTransform>();
            rootRect.anchorMin = new Vector2(0.5f, 0f);
            rootRect.anchorMax = new Vector2(0.5f, 0f);
            rootRect.pivot = new Vector2(0.5f, 0f);
            rootRect.anchoredPosition = new Vector2(0f, 196f);
            rootRect.sizeDelta = new Vector2(340f, 76f);
            GameObject barBg = CreateUIObject("ThreatBarBg", threatBarRoot.transform);
            RectTransform bgRect = barBg.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0f, 1f);
            bgRect.anchorMax = new Vector2(1f, 1f);
            bgRect.pivot = new Vector2(0.5f, 1f);
            bgRect.anchoredPosition = Vector2.zero;
            bgRect.sizeDelta = new Vector2(340f, 16f);
            Image bg = barBg.AddComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.45f);
            bg.raycastTarget = false;
            GameObject fillObj = CreateUIObject("ThreatBarFill", threatBarRoot.transform);
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0f, 1f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.pivot = new Vector2(0.5f, 1f);
            fillRect.anchoredPosition = new Vector2(0f, -2f);
            fillRect.sizeDelta = new Vector2(336f, 12f);
            threatBarFill = fillObj.AddComponent<Image>();
            threatBarFill.type = Image.Type.Filled;
            threatBarFill.fillMethod = Image.FillMethod.Horizontal;
            threatBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            threatBarFill.color = new Color(0.78f, 0.07f, 0.06f, 0.95f);
            threatBarFill.raycastTarget = false;
            threatBarFill.fillAmount = 0f;
            GameObject keyFrame = CreateUIObject("ThreatExitKeyFrame", threatBarRoot.transform);
            RectTransform keyFrameRect = keyFrame.GetComponent<RectTransform>();
            keyFrameRect.anchorMin = new Vector2(0.5f, 0f);
            keyFrameRect.anchorMax = new Vector2(0.5f, 0f);
            keyFrameRect.pivot = new Vector2(0.5f, 0f);
            keyFrameRect.anchoredPosition = new Vector2(0f, 0f);
            keyFrameRect.sizeDelta = new Vector2(46f, 46f);
            Image keyBg = keyFrame.AddComponent<Image>();
            keyBg.color = new Color(0.55f, 0.42f, 0.22f, 0.9f);
            keyBg.raycastTarget = false;
            GameObject keyInner = CreateUIObject("ThreatExitKeyInner", keyFrame.transform);
            RectTransform keyInnerRect = keyInner.GetComponent<RectTransform>();
            keyInnerRect.anchorMin = Vector2.zero;
            keyInnerRect.anchorMax = Vector2.one;
            keyInnerRect.offsetMin = new Vector2(3f, 3f);
            keyInnerRect.offsetMax = new Vector2(-3f, -3f);
            Image keyInnerBg = keyInner.AddComponent<Image>();
            keyInnerBg.color = new Color(0.05f, 0.05f, 0.07f, 0.95f);
            keyInnerBg.raycastTarget = false;
            threatExitKeyText = CreateText("ThreatExitKey", keyInner.transform, LoadSafeFont(), 26, TextAnchor.MiddleCenter);
            RectTransform keyTextRect = threatExitKeyText.GetComponent<RectTransform>();
            keyTextRect.anchorMin = Vector2.zero;
            keyTextRect.anchorMax = Vector2.one;
            keyTextRect.offsetMin = Vector2.zero;
            keyTextRect.offsetMax = Vector2.zero;
            threatExitKeyText.color = new Color(1f, 0.92f, 0.65f);
            threatExitKeyText.text = "E";
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

        private void ApplyNotebookDocumentTheme(RectTransform documentRect)
        {
            var panelImage=documentPanel.GetComponent<Image>();
            if(panelImage)
            {
                if(!_notebookSheetSprite)
                {
                    var tex=Resources.Load<Texture2D>("PrologNotebookSheet");
                    if(tex)_notebookSheetSprite=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(.5f,.5f),100f);
                }
                if(_notebookSheetSprite)
                {
                    panelImage.sprite=_notebookSheetSprite;
                    panelImage.type=Image.Type.Simple;
                    panelImage.color=Color.white;
                }
            }
            Color inkTitle=new Color(.15f,.09f,.06f,.98f);
            Color inkBody=new Color(.18f,.12f,.08f,.95f);
            if(documentTitleText)
            {
                documentTitleText.color=inkTitle;
                documentTitleText.font=LoadSafeFont();
                documentTitleText.alignment=TextAnchor.UpperCenter;
                documentTitleText.fontSize=26;
                var tr=documentTitleText.rectTransform;
                tr.anchorMin=new Vector2(0,1);tr.anchorMax=new Vector2(1,1);tr.pivot=new Vector2(.5f,1);
                tr.offsetMin=new Vector2(118,-122);tr.offsetMax=new Vector2(-64,-36);
            }
            var presentation=GetComponent<HudPresentation>();
            if(presentation&&presentation.documentScroll&&presentation.documentScroll.viewport)
            {
                var vp=presentation.documentScroll.viewport;
                vp.anchorMin=Vector2.zero;vp.anchorMax=Vector2.one;vp.pivot=new Vector2(.5f,.5f);
                vp.offsetMin=new Vector2(118,96);vp.offsetMax=new Vector2(-64,-185);
            }
            if(documentBodyText)
            {
                documentBodyText.color=inkBody;
                documentBodyText.font=LoadSafeFont();
                documentBodyText.alignment=TextAnchor.MiddleLeft;
                // Body ruling on PrologNotebookSheet.png: pitch 32px from y=158 at 720px.
                // The held panel spans 0.78 of the 1080 reference height, so on-screen
                // pitch is 32*0.78*1080/720=37.4px. A 22px body at 1.70 spacing hits
                // 37.4px per line and lands on the rules instead of drifting across them.
                // Viewport top is inset to the first body rule (the ContentSizeFitter
                // owns the content rect, so the viewport is the alignment lever).
                documentBodyText.fontSize=22;
                documentBodyText.lineSpacing=1.70f;
                var br=documentBodyText.rectTransform;
                br.anchorMin=new Vector2(0,1);br.anchorMax=new Vector2(1,1);br.pivot=new Vector2(.5f,1);
                br.offsetMin=new Vector2(150,br.offsetMin.y);br.offsetMax=new Vector2(0,0);
            }
            if(documentCloseButton)
            {
                var btnImg=documentCloseButton.GetComponent<Image>();
                if(btnImg)btnImg.color=new Color(.22f,.15f,.10f,.94f);
                var lbl=documentCloseButton.GetComponentInChildren<Text>();
                if(lbl)lbl.color=new Color(.95f,.91f,.82f,1f);
            }
            var scrollHint=documentRect.Find("ScrollHint");
            if(scrollHint)
            {
                var shTxt=scrollHint.GetComponent<Text>();
                if(shTxt)shTxt.color=new Color(.34f,.24f,.16f,.85f);
                var shRect=scrollHint.GetComponent<RectTransform>();
                if(shRect)
                {
                    shRect.anchorMin=shRect.anchorMax=new Vector2(0,0);
                    shRect.pivot=Vector2.zero;
                    shRect.anchoredPosition=new Vector2(122,36);
                }
            }
        }

        // Outlast-style paper themes for Rumah Atmaja documents and the red kos
        // envelope: opaque generated paper (blood spots, crumpled folds), even ink
        // layout, no ruled lines to fight. Everything else keeps the notebook.
        private void ApplyPaperTheme(RectTransform documentRect,DocPaper paper)
        {
            var panelImage=documentPanel.GetComponent<Image>();
            if(panelImage)
            {
                panelImage.sprite=DocPaperSprite(paper);
                panelImage.type=Image.Type.Simple;
                panelImage.color=Color.white;
            }
            Color ink,btnInk,hintInk;
            if(paper==DocPaper.RedEnvelope){ink=new Color(.96f,.90f,.76f,.98f);btnInk=new Color(.96f,.90f,.76f,1f);hintInk=new Color(.80f,.70f,.58f,.9f);}
            else if(paper==DocPaper.Newsprint){ink=new Color(.09f,.09f,.09f,.97f);btnInk=new Color(.93f,.92f,.86f,1f);hintInk=new Color(.25f,.25f,.22f,.9f);}
            else{ink=new Color(.14f,.09f,.05f,.97f);btnInk=new Color(.93f,.88f,.76f,1f);hintInk=new Color(.30f,.22f,.14f,.9f);}
            var face=DocFont(paper);
            // Raka's wall calendar: cream month title on the printed red band,
            // pencil-dark handwriting for the grid + note below the band.
            Color titleInk=ink,bodyInk=ink;
            float titleSize=30,bodySize=24,viewTop=-152f;
            if(paper==DocPaper.WallCalendar)
            {
                titleInk=new Color(.97f,.90f,.76f,.98f);
                bodyInk=new Color(.16f,.13f,.10f,.97f);
                titleSize=32;bodySize=23;viewTop=-172f;
            }
            if(documentTitleText)
            {
                documentTitleText.color=titleInk;
                if(face)documentTitleText.font=face;
                documentTitleText.alignment=TextAnchor.UpperLeft;
                documentTitleText.fontSize=(int)titleSize;
                var tr=documentTitleText.rectTransform;
                tr.anchorMin=new Vector2(0,1);tr.anchorMax=new Vector2(1,1);tr.pivot=new Vector2(.5f,1);
                tr.offsetMin=new Vector2(90,-132);tr.offsetMax=new Vector2(-60,-52);
            }
            var presentation=GetComponent<HudPresentation>();
            if(presentation&&presentation.documentScroll&&presentation.documentScroll.viewport)
            {
                var vp=presentation.documentScroll.viewport;
                vp.anchorMin=Vector2.zero;vp.anchorMax=Vector2.one;vp.pivot=new Vector2(.5f,.5f);
                vp.offsetMin=new Vector2(90,110);vp.offsetMax=new Vector2(-60,viewTop);
            }
            if(documentBodyText)
            {
                documentBodyText.color=bodyInk;
                if(face)documentBodyText.font=face;
                documentBodyText.alignment=TextAnchor.UpperLeft;
                documentBodyText.fontSize=(int)bodySize;
                documentBodyText.lineSpacing=1.5f;
                var br=documentBodyText.rectTransform;
                br.anchorMin=new Vector2(0,1);br.anchorMax=new Vector2(1,1);br.pivot=new Vector2(.5f,1);
                br.offsetMin=new Vector2(0,br.offsetMin.y);br.offsetMax=new Vector2(0,0);
            }
            if(documentCloseButton)
            {
                var btnImg=documentCloseButton.GetComponent<Image>();
                if(btnImg)btnImg.color=new Color(.16f,.11f,.07f,.96f);
                var lbl=documentCloseButton.GetComponentInChildren<Text>();
                if(lbl)lbl.color=btnInk;
            }
            var scrollHint=documentRect.Find("ScrollHint");
            if(scrollHint)
            {
                var shTxt=scrollHint.GetComponent<Text>();
                if(shTxt)shTxt.color=hintInk;
                var shRect=scrollHint.GetComponent<RectTransform>();
                if(shRect)
                {
                    shRect.anchorMin=shRect.anchorMax=new Vector2(0,0);
                    shRect.pivot=Vector2.zero;
                    shRect.anchoredPosition=new Vector2(94,40);
                }
            }
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
            var closeLabel=documentCloseButton.GetComponentInChildren<Text>();if(closeLabel)closeLabel.text="TUTUP CATATAN";
            var documentRect=(RectTransform)documentPanel.transform;
            bool holdingPhone=SmartphoneController.Instance&&SmartphoneController.Instance.isPhoneRaised;
            documentRect.anchorMin=holdingPhone?new Vector2(.055f,.11f):new Vector2(.5f,.5f);
            documentRect.anchorMax=holdingPhone?new Vector2(.685f,.89f):new Vector2(.5f,.5f);
            documentRect.anchoredPosition=Vector2.zero;
            documentRect.sizeDelta=holdingPhone?Vector2.zero:new Vector2(880,580);
            if(IsAtmajaNoteDocument(documentId,title))ApplyPaperTheme(documentRect,PaperFor(documentId,title));
            else ApplyNotebookDocumentTheme(documentRect);
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

        private static Sprite circleSprite;
        // Public so the scene-baking editor pass (separate assembly) can use
        // the same dot. Generated once, shared by every HUD.
        public static Sprite CircleSprite()
        {
            if (circleSprite) return circleSprite;
            const int s = 64;
            var tex = new Texture2D(s, s, TextureFormat.ARGB32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            for (int y = 0; y < s; y++) for (int x = 0; x < s; x++)
            {
                float dx = (x + .5f) / s * 2 - 1, dy = (y + .5f) / s * 2 - 1;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float a = Mathf.Clamp01((1 - d) * 48);
                tex.SetPixel(x, y, new Color(1, 1, 1, a));
            }
            tex.Apply();
            circleSprite = Sprite.Create(tex, new Rect(0, 0, s, s), new Vector2(.5f, .5f), s);
            return circleSprite;
        }
        // Styles the center dot on every load. Baked scene HUDs keep their
        // old square dot unless this runs — a sprite miss here is exactly
        // why players still saw a square after the round-dot change.
        private void EnforceCrosshair()
        {
            var canvas = GetComponentInChildren<Canvas>();
            if (!canvas) return;
            var t = canvas.transform.Find("Crosshair");
            if (!t) return;
            var rect = t.GetComponent<RectTransform>();
            if (rect)
            {
                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(.5f, .5f);
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = new Vector2(8f, 8f);
            }
            var img = t.GetComponent<Image>();
            if (img)
            {
                img.sprite = CircleSprite();
                img.color = new Color(1f, 1f, 1f, 0.9f);
                img.raycastTarget = false;
            }
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
