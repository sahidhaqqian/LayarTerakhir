#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using LayarTerakhir.Core;
using LayarTerakhir.Diagnostics;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LayarTerakhir.Review
{
    // Reusable director review mode: F7 help, F8 fly, F9 inspect, F10 capture, F11 freeze.
    // Inactive by default; while inactive it only polls function keys and changes nothing.
    public sealed class DeveloperReviewMode : MonoBehaviour
    {
        public static DeveloperReviewMode Instance { get; private set; }
        public static bool IsReviewSession { get; private set; }

        public ReviewFlyCamera Fly { get; private set; }
        public ReviewObjectInspector Inspector { get; private set; }
        public ReviewBugCapture Capture { get; private set; }
        public ReviewWorldFreeze Freeze { get; private set; }
        public ReviewPhaseNavigator Navigator { get; private set; }

        public bool HelpOpen { get; private set; }
        public string BugDescription = "";

        public static bool InputFieldFocused
        {
            get
            {
                var current = EventSystem.current ? EventSystem.current.currentSelectedGameObject : null;
                return current && current.GetComponent<InputField>();
            }
        }

        private GameObject canvas;
        private GameObject badge;
        private Text badgeLabel;
        private GameObject helpPanel;
        private Text helpKeys;
        private Button flyButton, inspectButton, freezeButton;
        private Slider speedSlider;
        private Text speedLabel;
        private InputField descriptionField;
        private GameObject inspectorPanel;
        private Text inspectorText;
        private Text toastText;
        private float toastUntil;

        // Last selection snapshot, kept so F10 stays precise after F9 is closed.
        private ReviewBugCapture.SelectionDto lastSelection;

        public bool AnyActive => (Fly && Fly.Active) || (Inspector && Inspector.Active)
            || (Freeze && Freeze.Frozen) || (Navigator && Navigator.ExcursionActive);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindFirstObjectByType<DeveloperReviewMode>()) return;
            try { IsReviewSession = ReviewDirectBoot.ReviewPhaseFor(Environment.GetCommandLineArgs()) > 0; }
            catch { IsReviewSession = false; }
            var go = new GameObject("DeveloperReviewMode");
            DontDestroyOnLoad(go);
            go.AddComponent<DeveloperReviewMode>();
        }

        private void Awake()
        {
            if (Instance && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            Fly = gameObject.AddComponent<ReviewFlyCamera>();
            Inspector = gameObject.AddComponent<ReviewObjectInspector>();
            Capture = gameObject.AddComponent<ReviewBugCapture>();
            Freeze = gameObject.AddComponent<ReviewWorldFreeze>();
            Navigator = gameObject.AddComponent<ReviewPhaseNavigator>();
        }

        private void Start()
        {
            BuildUi();
            RefreshUi();
            if (IsReviewSession)
                Toast("Mode review siap: F7 bantuan, F8 terbang, F9 klik objek, F10 laporan, F11 beku.");
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            // Refs died with the old scene; freeze must never stall a fresh boot.
            if (Fly) Fly.ExitForSceneLoad();
            if (Inspector) Inspector.ExitForSceneLoad();
            if (Freeze) Freeze.ReleaseForSceneLoad(scene.name);
            lastSelection = null;
            RefreshUi();
        }

        // Called before navigator-driven loads (modes already target a dead scene).
        public void SuspendForSceneLoad()
        {
            if (Fly) Fly.ExitForSceneLoad();
            if (Inspector) Inspector.ExitForSceneLoad();
            if (Freeze && Freeze.Frozen) Freeze.Unfreeze();
            lastSelection = null;
            RefreshUi();
        }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            var keys = Keyboard.current;
            if (keys != null)
            {
                if (keys.f7Key.wasPressedThisFrame) ToggleHelp();
                else if (keys.f8Key.wasPressedThisFrame) ToggleFly();
                else if (keys.f9Key.wasPressedThisFrame) ToggleInspect();
                else if (keys.f10Key.wasPressedThisFrame) CaptureBug();
                else if (keys.f11Key.wasPressedThisFrame) ToggleFreeze();
            }
#endif
            if (toastText && Time.unscaledTime > toastUntil) toastText.text = "";
            if (badge) badge.SetActive(HelpOpen || AnyActive || IsReviewSession);
        }

        public void ToggleHelp()
        {
            HelpOpen = !HelpOpen;
            RefreshUi();
        }

        public void ToggleFly()
        {
            if (Fly.Active) { Fly.Disable(); RefreshUi(); return; }
            if (!GuardGameplay("terbang")) return;
            if (Inspector.Active) Inspector.Disable();
            var player = FindFirstObjectByType<FirstPersonController>();
            if (Fly.Enable(player))
            {
                Toast("Kamera terbang ON: WASD + mouse, Q/E bawah/atas, SHIFT cepat, scroll = kecepatan.");
            }
            else Toast("Kamera terbang gagal: tidak ada kamera aktif.");
            RefreshUi();
        }

        public void ToggleInspect()
        {
            if (Inspector.Active) { Inspector.Disable(); RefreshUi(); return; }
            if (!GuardGameplay("klik objek")) return;
            if (Fly.Active)
            {
                Toast("Keluar dulu dari kamera terbang (F8), baru F9 klik objek.");
                return;
            }
            var player = FindFirstObjectByType<FirstPersonController>();
            if (Inspector.Enable(player))
            {
                Toast("Klik objek ON: klik objek untuk info + kotak kuning. ESC tidak menutup; tekan F9 lagi.");
            }
            else Toast("Klik objek gagal.");
            RefreshUi();
        }

        public void CaptureBug()
        {
            if (Capture.Busy) return;
            if (Navigator.Loading) { Toast("Tunggu scene selesai dimuat."); return; }
            SyncDescription();
            Capture.Capture(BugDescription);
        }

        public void ToggleFreeze()
        {
            if (Freeze.Toggle())
            {
                Toast(Freeze.Frozen ? "Dunia DIBEKUKAN (F11 lepas). Kamera review tetap jalan."
                    : "Dunia jalan lagi.");
            }
            RefreshUi();
        }

        private bool GuardGameplay(string what)
        {
            if (UiFocus.Menu || GameShell.AwaitingStart || GameShell.MenuSceneLoading)
            {
                Toast("Tutup menu dulu sebelum " + what + ".");
                return false;
            }
            if (UiFocus.Cinematic)
            {
                Toast("Tunggu sinematik selesai sebelum " + what + ".");
                return false;
            }
            if (Navigator.Loading)
            {
                Toast("Tunggu scene selesai dimuat.");
                return false;
            }
            return true;
        }

        public void Toast(string message)
        {
            if (!toastText) return;
            toastText.text = message ?? "";
            toastUntil = Time.unscaledTime + 6f;
        }

        public void SetOverlayVisible(bool visible)
        {
            if (canvas) canvas.SetActive(visible);
        }

        public void RefreshInspectorPanel()
        {
            if (Inspector && Inspector.Selected) CacheSelection(Inspector.Selected, Inspector.HitPoint);
            if (inspectorText)
                inspectorText.text = Inspector ? Inspector.CurrentReport : "";
            RefreshUi();
        }

        private void CacheSelection(GameObject go, Vector3 point)
        {
            if (!go) return;
            lastSelection = new ReviewBugCapture.SelectionDto
            {
                name = go.name,
                hierarchyPath = ReviewHierarchy.BuildPath(go.transform),
                scene = go.scene.name,
                position = new ReviewBugCapture.VectorDto(go.transform.position),
                rotationEuler = new ReviewBugCapture.VectorDto(go.transform.rotation.eulerAngles),
                scale = new ReviewBugCapture.VectorDto(go.transform.localScale),
                renderer = ReviewHierarchy.DescribeRenderer(go),
                collider = ReviewHierarchy.DescribeCollider(go),
            };
        }

        public ReviewBugCapture.SelectionDto LiveOrLastSelection(GameObject live)
        {
            if (live)
            {
                CacheSelection(live, live.transform.position);
                return lastSelection;
            }
            return lastSelection;
        }

        // ---- UI (runtime-built, above the pause menu so Restore stays reachable) ----

        private void BuildUi()
        {
            if (FindFirstObjectByType<EventSystem>() == null)
            {
                var es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
                es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
                es.AddComponent<StandaloneInputModule>();
#endif
            }
            canvas = new GameObject("ReviewModeCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.transform.SetParent(transform, false);
            var c = canvas.GetComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            c.sortingOrder = 4500;
            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1600, 900);

            badge = UiWidgets.Panel("ReviewBadge", canvas.transform,
                new Vector2(0.86f, 0.93f), new Vector2(0.985f, 0.975f), new Color(0.02f, 0.03f, 0.03f, 0.82f));
            badgeLabel = UiWidgets.Text("Label", badge.transform, "REVIEW [F7]",
                Vector2.zero, Vector2.one, 20, TextAnchor.MiddleCenter);
            var badgeButton = badge.AddComponent<Button>();
            badgeButton.targetGraphic = badge.GetComponent<Image>();
            badgeButton.onClick.AddListener(ToggleHelp);

            helpPanel = UiWidgets.Panel("ReviewHelp", canvas.transform,
                new Vector2(0.63f, 0.05f), new Vector2(0.985f, 0.92f), new Color(0.012f, 0.019f, 0.021f, 0.94f));
            UiWidgets.Text("Title", helpPanel.transform, "MODE REVIEW — SUTRADARA",
                new Vector2(0.04f, 0.945f), new Vector2(0.96f, 0.985f), 24);
            helpKeys = UiWidgets.ScrollText("Keys", helpPanel.transform, HelpText(),
                new Vector2(0.04f, 0.80f), new Vector2(0.96f, 0.94f), 20);

            flyButton = UiWidgets.Button("Fly", helpPanel.transform, "TERBANG [F8]",
                new Vector2(0.04f, 0.735f), new Vector2(0.35f, 0.785f), ToggleFly);
            inspectButton = UiWidgets.Button("Inspect", helpPanel.transform, "KLIK OBJEK [F9]",
                new Vector2(0.365f, 0.735f), new Vector2(0.655f, 0.785f), ToggleInspect);
            freezeButton = UiWidgets.Button("Freeze", helpPanel.transform, "BEKU [F11]",
                new Vector2(0.67f, 0.735f), new Vector2(0.96f, 0.785f), ToggleFreeze);

            speedLabel = UiWidgets.Text("SpeedLabel", helpPanel.transform, "Kecepatan terbang",
                new Vector2(0.04f, 0.685f), new Vector2(0.96f, 0.725f), 20);
            speedSlider = UiWidgets.Slider("Speed", helpPanel.transform, "",
                new Vector2(0.04f, 0.645f), new Vector2(0.96f, 0.685f),
                ReviewFlyCamera.MinSpeed, ReviewFlyCamera.MaxSpeed, ReviewFlyCamera.DefaultSpeed,
                v => { if (Fly) Fly.FlySpeed = v; RefreshSpeedLabel(); });

            UiWidgets.Text("DescLabel", helpPanel.transform, "Deskripsi bug (opsional, lalu F10):",
                new Vector2(0.04f, 0.60f), new Vector2(0.96f, 0.64f), 20);
            descriptionField = BuildInputField("BugDescription", helpPanel.transform,
                new Vector2(0.04f, 0.55f), new Vector2(0.78f, 0.60f));
            UiWidgets.Button("ClearDesc", helpPanel.transform, "HAPUS",
                new Vector2(0.795f, 0.55f), new Vector2(0.96f, 0.60f),
                () => { BugDescription = ""; if (descriptionField) descriptionField.text = ""; });

            UiWidgets.Button("Capture", helpPanel.transform, "SIMPAN LAPORAN [F10]",
                new Vector2(0.04f, 0.495f), new Vector2(0.52f, 0.545f), CaptureBug);
            UiWidgets.Button("Folder", helpPanel.transform, "BUKA FOLDER",
                new Vector2(0.535f, 0.495f), new Vector2(0.96f, 0.545f),
                () => ReviewBugCapture.OpenReportsFolder());

            UiWidgets.Button("Restore", helpPanel.transform, "KEMBALI KE CAMPAIGN",
                new Vector2(0.04f, 0.44f), new Vector2(0.52f, 0.49f),
                () => { if (Navigator) Navigator.RestoreOrigin(); });
            UiWidgets.Button("Reload", helpPanel.transform, "MUAT ULANG SCENE",
                new Vector2(0.535f, 0.44f), new Vector2(0.96f, 0.49f),
                () => { if (Navigator) Navigator.ReloadScene(); });

            UiWidgets.Text("PhaseLabel", helpPanel.transform, "Pindah scene review (save asli aman):",
                new Vector2(0.04f, 0.395f), new Vector2(0.96f, 0.435f), 20);
            var targets = ReviewPhaseNavigator.Targets;
            for (int i = 0; i < targets.Length; i++)
            {
                string id = targets[i].id;
                string label = targets[i].label;
                int row = i / 2, col = i % 2;
                float top = 0.385f - row * 0.052f;
                UiWidgets.Button("Target_" + id, helpPanel.transform, label,
                    new Vector2(col == 0 ? 0.04f : 0.51f, top - 0.047f),
                    new Vector2(col == 0 ? 0.49f : 0.96f, top),
                    () => { if (Navigator) Navigator.JumpTo(id); });
            }

            inspectorPanel = UiWidgets.Panel("ReviewInspector", canvas.transform,
                new Vector2(0.015f, 0.06f), new Vector2(0.45f, 0.42f),
                new Color(0.012f, 0.019f, 0.021f, 0.90f));
            UiWidgets.Text("Title", inspectorPanel.transform, "INFO OBJEK [F9]",
                new Vector2(0.03f, 0.90f), new Vector2(0.97f, 0.97f), 22);
            inspectorText = UiWidgets.ScrollText("Info", inspectorPanel.transform, "",
                new Vector2(0.03f, 0.03f), new Vector2(0.97f, 0.89f), 20);

            var toastBg = UiWidgets.Panel("ReviewToast", canvas.transform,
                new Vector2(0.20f, 0.005f), new Vector2(0.80f, 0.05f),
                new Color(0.02f, 0.03f, 0.03f, 0.80f));
            toastText = UiWidgets.Text("Text", toastBg.transform, "",
                new Vector2(0.02f, 0f), new Vector2(0.98f, 1f), 21, TextAnchor.MiddleCenter);
        }

        private InputField BuildInputField(string fieldName, Transform parent, Vector2 min, Vector2 max)
        {
            var go = UiWidgets.Panel(fieldName, parent, min, max, new Color(0.10f, 0.12f, 0.12f, 0.97f));
            var text = UiWidgets.Text("Text", go.transform, "",
                new Vector2(0.02f, 0f), new Vector2(0.98f, 1f), 20);
            text.raycastTarget = true;
            text.resizeTextForBestFit = false;
            var placeholder = UiWidgets.Text("Placeholder", go.transform, "cth: tembok tembus pandang di koridor",
                new Vector2(0.02f, 0f), new Vector2(0.98f, 1f), 20);
            placeholder.color = new Color(0.55f, 0.53f, 0.48f);
            var field = go.AddComponent<InputField>();
            field.textComponent = text;
            field.placeholder = placeholder;
            field.lineType = InputField.LineType.SingleLine;
            field.characterLimit = 140;
            field.onValueChanged.AddListener(v => BugDescription = v ?? "");
            return field;
        }

        private static string HelpText()
        {
            return "F7 panel ini • F8 kamera terbang • F9 klik objek • F10 simpan laporan • F11 bekukan dunia\n"
                + "TERBANG: WASD gerak, mouse arah, Q/E turun/naik, SHIFT ngebut, scroll atur kecepatan.\n"
                + "Posisi Raka tidak berubah; tekan F8 lagi untuk kembali normal.\n"
                + "KLIK: tekan F9 lalu klik objek. Info nama, path, koordinat, renderer/collider tampil kiri bawah + kotak kuning.\n"
                + "LAPORAN: isi deskripsi singkat (opsional), tekan F10. PNG + JSON tersimpan di folder BugReports.\n"
                + "BEKU: F11 menghentikan NPC/hantu/animasi untuk inspeksi; kamera review tetap jalan.\n"
                + "PINDAH SCENE: tombol babak di bawah membuka scene review tanpa mengubah save asli. KEMBALI KE CAMPAIGN untuk pulang.\n"
                + "Hanya ada di Editor dan development build; tidak ada di rilis produksi.";
        }

        private void SyncDescription()
        {
            if (descriptionField) BugDescription = descriptionField.text ?? "";
        }

        private void RefreshSpeedLabel()
        {
            if (speedLabel && Fly)
                speedLabel.text = "Kecepatan terbang: " + Fly.FlySpeed.ToString("F1") + " m/s (scroll saat terbang)";
        }

        private void RefreshUi()
        {
            if (canvas == null) return;
            bool showBadge = HelpOpen || AnyActive || IsReviewSession;
            if (badge) badge.SetActive(showBadge);
            if (badgeLabel)
            {
                string state = HelpOpen || AnyActive || IsReviewSession ? "REVIEW [F7]" : "";
                if (Fly && Fly.Active) state += " • TERBANG";
                if (Inspector && Inspector.Active) state += " • KLIK";
                if (Freeze && Freeze.Frozen) state += " • BEKU";
                if (Navigator && Navigator.ExcursionActive) state += " • " + Navigator.ExcursionTarget;
                badgeLabel.text = state;
            }
            if (helpPanel) helpPanel.SetActive(HelpOpen);
            if (speedSlider && Fly && Math.Abs(speedSlider.value - Fly.FlySpeed) > 0.001f)
                speedSlider.value = Fly.FlySpeed;
            RefreshSpeedLabel();
            SetButtonLabel(flyButton, (Fly && Fly.Active ? ">> TERBANG ON [F8]" : "TERBANG [F8]"));
            SetButtonLabel(inspectButton, (Inspector && Inspector.Active ? ">> KLIK ON [F9]" : "KLIK OBJEK [F9]"));
            SetButtonLabel(freezeButton, (Freeze && Freeze.Frozen ? ">> BEKU [F11]" : "BEKU [F11]"));
            if (inspectorPanel) inspectorPanel.SetActive(Inspector && Inspector.Active);
            if (inspectorText && Inspector) inspectorText.text = Inspector.CurrentReport;
        }

        private static void SetButtonLabel(Button button, string words)
        {
            if (!button) return;
            var label = button.GetComponentInChildren<Text>();
            if (label) label.text = words;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
#endif
