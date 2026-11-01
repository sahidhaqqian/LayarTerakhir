#if UNITY_EDITOR
using System;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace LayarTerakhir.Editor.Tests
{
    public class FoundationAuditTests
    {
        private Keyboard _keyboard;
        private string _saveDirectory;
        private string _previousSaveDirectory;

        [SetUp]
        public void SetUp()
        {
            _previousSaveDirectory = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            _saveDirectory = Path.Combine(Path.GetTempPath(), "LayarTerakhir-Test-" + Guid.NewGuid());
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", _saveDirectory);
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            Invoke(Find<CampaignState>(), "Awake");
            Invoke(Find<GameInputConfig>(), "Awake");
            Invoke(Find<HudController>(), "Awake");
            Invoke(Find<SmartphoneController>(), "Awake");
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_keyboard != null) InputSystem.RemoveDevice(_keyboard);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", _previousSaveDirectory);
            if (Directory.Exists(_saveDirectory)) Directory.Delete(_saveDirectory, true);
        }

        [Test]
        public void SavedDocumentCloseButtonClosesAndCallsBackOnce()
        {
            var hud = Find<HudController>();
            int calls = 0;
            hud.OpenDocumentModal("Audit", "Document", () => calls++);
            hud.documentCloseButton.onClick.Invoke();
            Assert.That(hud.IsModalOpen, Is.False);
            hud.documentCloseButton.onClick.Invoke();
            Assert.That(calls, Is.EqualTo(1));
        }

        [Test]
        public void DocumentThemesSplitKosNotebookAndAtmajaNote()
        {
            var hud = Find<HudController>();
            hud.documentCloseButton.onClick.Invoke();
            // Damar's survey note matches the Outlast reference: handwriting font,
            // blood-spotted crumpled paper, even top-left layout.
            hud.OpenDocumentModal("CATATAN SURVEI DAMAR • DOKUMEN #2", "Baris satu.\nBaris dua.", null, "DOC_PROLOG_02_SURVEI_DAMAR");
            Assert.That(hud.IsDocumentOpen, Is.True);
            var panel = hud.documentPanel.GetComponent<UnityEngine.UI.Image>();
            Assert.That(panel.sprite, Is.SameAs(HudController.DocPaperSprite(HudController.DocPaper.BloodyNote)), "Survey uses the bloody note paper");
            Assert.That(hud.documentBodyText.font, Is.SameAs(Resources.Load<Font>("Fonts/Kalam-Regular")), "Survey uses the handwriting font");
            Assert.That(HudController.DocPaperHasBlood(HudController.DocPaper.BloodyNote), Is.True, "Survey paper carries blood spots");
            Assert.That(hud.documentBodyText.alignment, Is.EqualTo(TextAnchor.UpperLeft), "Note body is evenly top-left");
            hud.documentCloseButton.onClick.Invoke();
            // The 1998 clipping is typewritten newsprint.
            hud.OpenDocumentModal("KLIPING KORAN 1998 • DOKUMEN #3", "Baris satu.", null, "DOC_PROLOG_03_KORAN_1998");
            Assert.That(hud.documentBodyText.font, Is.SameAs(Resources.Load<Font>("Fonts/CourierPrime-Regular")), "Newspaper uses the typewriter font");
            Assert.That(HudController.DocPaperHasBlood(HudController.DocPaper.Newsprint), Is.False, "Newsprint carries ink, not blood");
            hud.documentCloseButton.onClick.Invoke();
            // The kos letter arrives in a red envelope, not the notebook.
            hud.OpenDocumentModal("SURAT PERINGATAN TERAKHIR (ITEM DOKUMEN #1)", "Baris satu.\nBaris dua.", null, "DOC_PROLOG_01_SURAT_KOS");
            Assert.That(panel.sprite, Is.SameAs(HudController.DocPaperSprite(HudController.DocPaper.RedEnvelope)), "Kos letter uses the red envelope paper");
            Assert.That(HudController.DocPaperHasBlood(HudController.DocPaper.RedEnvelope), Is.False, "Envelope has no blood");
            hud.documentCloseButton.onClick.Invoke();
            // Anything else keeps the lined notebook, aligned to its ruling.
            hud.OpenDocumentModal("STICKY NOTE DI MEJA PC", "Baris satu.", null, null);
            var tex = Resources.Load<Texture2D>("PrologNotebookSheet");
            Assert.That(tex, Is.Not.Null);
            Assert.That(hud.documentBodyText.fontSize * hud.documentBodyText.lineSpacing, Is.EqualTo(37.4f).Within(.05f), "Notebook pitch matches the 32px ruling at 37.4px");
            var vp = hud.GetComponent<HudPresentation>().documentScroll.viewport;
            Assert.That(vp.offsetMax.y, Is.EqualTo(-185f).Within(1f), "Viewport starts on the first ruled line");
            hud.documentCloseButton.onClick.Invoke();
            Assert.That(hud.IsModalOpen, Is.False);
        }

        [Test]
        public void MonitorDashboardFollowsMeasuredGlassPlaneWithinTwoMillimeters()
        {
            LayarTerakhir.Prolog.KosRoomCorrections.AlignMonitor();
            var screen=GameObject.Find("MonitorDashboard").transform;
            var normal=new Vector3(0,.1314f,-.9913f);
            float separation=Vector3.Dot(screen.position-new Vector3(-.7251642f,1.203817f,1.4236f),normal);
            Assert.That(separation,Is.InRange(.0001f,.002f));
            Assert.That(Vector3.Dot(-screen.forward,normal),Is.GreaterThan(.9999f));
        }

        [Test]
        public void PhysicalDrawerLowersPhoneAndReleasesItsFocusWithoutRemovingOtherLocks()
        {
            var phone=Find<SmartphoneController>();var player=Find<FirstPersonController>();var hud=Find<HudController>();
            phone.hasSmartphoneInInventory=phone.isPhoneRaised=true;UiFocus.Phone=true;player.MovementLocked=true;
            hud.OpenPadlockModal("1408",()=>{});hud.ClosePadlockModal();
            Assert.That(phone.isPhoneRaised||UiFocus.Phone||hud.IsModalOpen,Is.False);
            Assert.That(player.MovementLocked,Is.True,"Closing a modal must not undo unrelated cinematic locks");
        }

        [TestCase("148")]
        [TestCase("1408")]
        public void SavedPadlockButtonsSolveOnce(string code)
        {
            var hud = Find<HudController>();
            hud.EnsureRuntimeUIBuilt();
            hud.EnsureRuntimeUIBuilt();
            int calls = 0;
            hud.OpenPadlockModal(code, () => calls++);
            for (int slot = 0; slot < code.Length; slot++)
            {
                Button(hud, "DigitDown_" + slot).onClick.Invoke();
                Button(hud, "DigitUp_" + slot).onClick.Invoke();
                for (int n = 0; n < code[slot]-'0'; n++)
                    Button(hud, "DigitUp_" + slot).onClick.Invoke();
            }
            Button(hud, "UnlockPadlockBtn").onClick.Invoke();
            Assert.That(hud.IsModalOpen, Is.False);
            Button(hud, "UnlockPadlockBtn").onClick.Invoke();
            Assert.That(calls, Is.EqualTo(1));
        }

        [Test]
        public void SavedPadlockRejectsWrongCodeAndCancelCloses()
        {
            var hud = Find<HudController>();
            int calls = 0;
            hud.OpenPadlockModal("148", () => calls++);
            Button(hud, "UnlockPadlockBtn").onClick.Invoke();
            Assert.That(hud.padlockFeedbackText.text, Does.Contain("salah"));
            Assert.That(hud.IsModalOpen, Is.True);
            Button(hud, "CancelPadlockBtn").onClick.Invoke();
            Assert.That(hud.IsModalOpen, Is.False);
            Assert.That(calls, Is.Zero);
        }

        [Test]
        public void PadlockCallbackCannotRepeatAfterSuccessfulSubmission()
        {
            var hud = Find<HudController>();
            int calls = 0;
            hud.OpenPadlockModal("000", () => calls++);
            Invoke(hud, "SubmitPadlockCode");
            Invoke(hud, "SubmitPadlockCode");
            Assert.That(calls, Is.EqualTo(1));
        }

        [Test]
        public void PhoneRigUsesCameraLocalOffset()
        {
            var phone = Find<SmartphoneController>();
            Assert.That(Vector3.Distance(phone.handPhoneMeshRoot.transform.localPosition,
                new Vector3(0.24f, -0.18f, 0.38f)), Is.LessThan(0.001f));
            Assert.That(phone.handPhoneMeshRoot.GetComponent<Collider>(), Is.Null);
        }

        [Test]
        public void BuildStartsInKos()
        {
            Assert.That(EditorBuildSettings.scenes[0].enabled, Is.True);
            Assert.That(EditorBuildSettings.scenes[0].path, Is.EqualTo(PrologKosSceneBuilder.ScenePath));
        }

        [Test]
        public void InitialPlayerCapsuleDoesNotOverlapFurniture()
        {
            var cc = Find<FirstPersonController>().GetComponent<CharacterController>();
            Physics.SyncTransforms();
            Vector3 bottom = cc.transform.position + Vector3.up * cc.radius;
            Vector3 top = cc.transform.position + Vector3.up * (cc.height - cc.radius);
            foreach (var collider in Physics.OverlapCapsule(bottom, top, cc.radius))
                Assert.That(collider == cc, Is.True, "Spawn overlaps " + collider.name);
        }

        [TestCase("Assets/Scenes/SampleScene.unity")]
        [TestCase(PrologKosSceneBuilder.ScenePath)]
        public void SceneHasNoMissingScriptsOrSerializedObjectReferences(string path)
        {
            var scene = EditorSceneManager.OpenScene(path);
            foreach (var root in scene.GetRootGameObjects())
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                Assert.That(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject), Is.Zero, transform.name);
                foreach (var component in transform.GetComponents<Component>())
                {
                    var property = new SerializedObject(component).GetIterator();
                    while (property.Next(true))
                        if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
                            Assert.That(property.objectReferenceInstanceIDValue, Is.Zero, transform.name + "." + property.propertyPath);
                }
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void NewGameArchivesPreviousSlotByteForByte(bool corrupt)
        {
            var c=Find<CampaignState>();c.AddInventoryItem("PREVIOUS_CAMPAIGN");
            Assert.That(c.SaveCheckpointAtomic("PROLOG_ATMAJA_KITCHEN"),Is.True);
            string slot=Path.Combine(_saveDirectory,"layar_terakhir_save.json");
            if(corrupt)File.WriteAllText(slot,"{ incomplete old save");
            string before=File.ReadAllText(slot);
            Assert.That(c.HasValidCheckpoint,Is.EqualTo(!corrupt));
            Assert.That(c.BeginNewCampaignPreservingPrevious(),Is.True);
            string[] archived=Directory.GetFiles(Path.Combine(_saveDirectory,"ArchivedCampaigns"),"*.json");
            Assert.That(archived.Length,Is.EqualTo(1));Assert.That(File.ReadAllText(archived[0]),Is.EqualTo(before));
            Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.checkpoint_id,Is.EqualTo("PROLOG_KOS_START"));
            Assert.That(c.HasInventoryItem("PREVIOUS_CAMPAIGN"),Is.False);
        }

        [Test]
        public void SaveReplacementKeepsBackupAndRestoresFullSnapshot()
        {
            var campaign = Find<CampaignState>();
            campaign.AddInventoryItem("SMARTPHONE_RAKA");
            Assert.That(campaign.AddInventoryItem("SMARTPHONE_RAKA"), Is.False);
            Assert.That(campaign.SaveCheckpointAtomic("FIRST"), Is.True);
            campaign.Data.battery = 37.5f;
            campaign.AddBoundDonation(5000000);
            campaign.AddInventoryItem("JAKET_RAKA");
            Assert.That(campaign.SaveCheckpointAtomic("SECOND"), Is.True);
            campaign.StartNewGame();
            Assert.That(campaign.LoadCheckpoint(), Is.True);
            Assert.That(campaign.Data.checkpoint_id, Is.EqualTo("SECOND"));
            Assert.That(campaign.Data.battery, Is.EqualTo(37.5f));
            Assert.That(campaign.Data.wallet_bound, Is.EqualTo(5000000));
            Assert.That(campaign.Data.inventory, Is.EquivalentTo(new[] { "SMARTPHONE_RAKA", "JAKET_RAKA" }));
            Assert.That(campaign.LoadBackupCheckpoint(), Is.True);
            Assert.That(campaign.Data.checkpoint_id, Is.EqualTo("FIRST"));
            Assert.That(campaign.Data.wallet_bound, Is.Zero);
        }

        [Test]
        public void FailedSavePreservesPreviousFileAndInMemoryCheckpoint()
        {
            var campaign = Find<CampaignState>();
            Assert.That(campaign.SaveCheckpointAtomic("GOOD"), Is.True);
            string path = Path.Combine(_saveDirectory, "layar_terakhir_save.json");
            string before = File.ReadAllText(path);
            Directory.CreateDirectory(path + ".tmp");
            LogAssert.Expect(LogType.Error, new Regex("Gagal menyimpan checkpoint"));
            Assert.That(campaign.SaveCheckpointAtomic("FAILED"), Is.False);
            Assert.That(File.ReadAllText(path), Is.EqualTo(before));
            Assert.That(campaign.Data.checkpoint_id, Is.EqualTo("GOOD"));
        }

        [TestCase("{bad json")]
        [TestCase("{}")]
        [TestCase("{\"schema_version\":99}")]
        [TestCase("{\"schema_version\":1,\"battery\":-1}")]
        [TestCase("{\"schema_version\":1,\"sanity\":101}")]
        [TestCase("{\"schema_version\":1,\"inventory\":[\"KEY\",\"KEY\"]}")]
        [TestCase("{\"schema_version\":1,\"wallet_bound\":-1}")]
        [TestCase("{\"schema_version\":1,\"ending\":99}")]
        public void CorruptSaveDoesNotReplaceLiveData(string json)
        {
            var campaign = Find<CampaignState>();
            campaign.AddInventoryItem("KEEP_ME");
            var before = campaign.Data;
            Directory.CreateDirectory(_saveDirectory);
            File.WriteAllText(Path.Combine(_saveDirectory, "layar_terakhir_save.json"), json);
            Assert.That(campaign.LoadCheckpoint(), Is.False);
            Assert.That(campaign.Data, Is.SameAs(before));
            Assert.That(campaign.HasInventoryItem("KEEP_ME"), Is.True);
        }

        [Test]
        public void NewGameResetsInventoryAndWallet()
        {
            var campaign = Find<CampaignState>();
            campaign.AddInventoryItem("KEY");
            campaign.AddBoundDonation(5000000);
            campaign.StartNewGame();
            Assert.That(campaign.Data.inventory, Is.Empty);
            Assert.That(campaign.Data.wallet_bound, Is.Zero);
            Assert.That(campaign.Data.checkpoint_id, Is.EqualTo("PROLOG_KOS_START"));
        }

        [TestCase(KeyCode.UpArrow, Key.UpArrow)]
        [TestCase(KeyCode.Alpha1, Key.Digit1)]
        [TestCase(KeyCode.RightControl, Key.RightCtrl)]
        public void RemappedMovementReadsNonDefaultKeys(KeyCode binding, Key key)
        {
            var input = Find<GameInputConfig>();
            input.RemapAction(GameAction.MoveForward, binding);
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState(key));
            InputSystem.Update();
            Assert.That(input.GetMovementInput().y, Is.EqualTo(1f));
        }

        internal static T Find<T>() where T : UnityEngine.Object => UnityEngine.Object.FindFirstObjectByType<T>();

        internal static void Invoke(object target, string method)
        {
            target.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(target, null);
        }

        internal static Button Button(HudController hud, string name)
        {
            foreach (var button in hud.GetComponentsInChildren<Button>(true))
                if (button.name == name) return button;
            throw new InvalidOperationException("Missing UI button: " + name);
        }
    }
}
#endif
