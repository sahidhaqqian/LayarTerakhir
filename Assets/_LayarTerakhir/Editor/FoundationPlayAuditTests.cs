#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LayarTerakhir.Editor.Tests
{
    // Discovered as Editor tests; each test explicitly enters actual Play Mode headlessly.
    public sealed class FoundationAuditCoroutineHost : MonoBehaviour { }

    public class FoundationPlayAuditTests
    {
        private Keyboard _keyboard;
        private Mouse _mouse;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            SessionState.SetString("LayarAudit.PreviousSaveDirectory",
                Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY") ?? "");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",
                Path.Combine(Path.GetTempPath(), "LayarTerakhir-PlayTest-" + Guid.NewGuid()));
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            yield return new EnterPlayMode();
            PrologSceneFlow.AutomaticTransitions=false; // This fixture specifically tests the approved Kos endpoint.
            SessionState.SetString("LayarAudit.PreviousInputSettings", JsonUtility.ToJson(InputSystem.settings));
            var settings = UnityEngine.Object.Instantiate(InputSystem.settings);
            settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings = settings;
            _keyboard = InputSystem.AddDevice<Keyboard>();
            _mouse = InputSystem.AddDevice<Mouse>();
            Time.timeScale = 10f;
            Assert.That(Application.isPlaying, Is.True);
            int initialFrame = Time.frameCount;
            while (Time.frameCount == initialFrame) yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_keyboard != null) InputSystem.RemoveDevice(_keyboard);
            if (_mouse != null) InputSystem.RemoveDevice(_mouse);
            var settings = InputSystem.settings;
            var restoredSettings = ScriptableObject.CreateInstance<InputSettings>();
            JsonUtility.FromJsonOverwrite(SessionState.GetString("LayarAudit.PreviousInputSettings", "{}"), restoredSettings);
            InputSystem.settings = restoredSettings;
            UnityEngine.Object.Destroy(settings);
            Time.timeScale = 1f;
            yield return new ExitPlayMode();
            string directory = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            string previous = SessionState.GetString("LayarAudit.PreviousSaveDirectory", "");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previous);
            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory)) Directory.Delete(directory, true);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [UnityTest]
        public IEnumerator KosSequenceWithJacketFirst() => RunInPlayerLoop(CompleteKos(true));

        [UnityTest]
        public IEnumerator PauseSettingsAndResumeOwnInputAndTime() => RunInPlayerLoop(PauseSettings());

        private IEnumerator PauseSettings()
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);
            float scale=Time.timeScale;var shell=GameShell.Instance;shell.Pause();yield return null;
            Assert.That(UiFocus.Menu,Is.True);Assert.That(Time.timeScale,Is.Zero);Assert.That(AudioListener.pause,Is.True);
            var settings=UnityEngine.Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None);
            Array.Find(settings,b=>b.name=="Settings").onClick.Invoke();yield return null;
            var slider=Array.Find(UnityEngine.Object.FindObjectsByType<UnityEngine.UI.Slider>(FindObjectsSortMode.None),s=>s.name=="MasterVolume");
            Assert.That(slider,Is.Not.Null);slider.value=.45f;Assert.That(AudioListener.volume,Is.EqualTo(.45f).Within(.01f));
            shell.Resume();Assert.That(UiFocus.Menu,Is.False);Assert.That(Time.timeScale,Is.EqualTo(scale));Assert.That(AudioListener.pause,Is.False);
            AudioListener.volume=1;
        }

        [UnityTest]
        public IEnumerator KosSequenceWithDrawerFirst() => RunInPlayerLoop(CompleteKos(false));

        private IEnumerator CompleteKos(bool jacketFirst)
        {
            var director = Find<KamarKosPrologDirector>();
            var hud = Find<HudController>();
            var campaign = Find<CampaignState>();
            Assert.That(director.suratKosItem.gameObject.activeSelf,Is.False,"Letter must not be lying inside the room before the knocks");
            Assert.That(director.suratKosItem.CanInteract(), Is.False);
            Assert.That(director.smartphoneBedItem.CanInteract(), Is.False);
            // The bed wake/landlady sequence owns input; early door use must not skip it.
            director.kosDoorItem.Interact();
            Assert.That(director.currentStage, Is.EqualTo(KosPrologStage.WakeUpAndKnocks));
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);
            yield return Interact(director.pcMonitorItem, new Vector3(.85f, .05f, -.05f));
            Assert.That(hud.documentBodyText.text, Does.Contain("32"));
            CloseDocument();
            yield return Interact(director.suratKosItem, new Vector3(0.85f, 0.05f, -0.25f));
            Assert.That(hud.IsModalOpen, Is.True);
            Vector3 modalPosition = Find<FirstPersonController>().transform.position;
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState(Key.W));
            yield return new WaitForSeconds(0.5f);
            Assert.That(Vector2.Distance(new Vector2(modalPosition.x, modalPosition.z),
                new Vector2(Find<FirstPersonController>().transform.position.x, Find<FirstPersonController>().transform.position.z)), Is.LessThan(0.001f));
            yield return Press();
            CloseDocument();
            yield return WaitForStage(KosPrologStage.WaitPickupSmartphone);
            yield return Interact(director.smartphoneBedItem, new Vector3(-.8f, .05f, -.98f));
            Assert.That(Find<SmartphoneController>().hasSmartphoneInInventory, Is.True);
            yield return null;
            var view=Find<AtmajaPhoneView>();
            Assert.That(hud.IsModalOpen,Is.False,"Picking up the phone must not open a voice-note document");
            Assert.That(view.Apps.CallActive,Is.True);
            Assert.That(view.Apps.CurrentApp,Is.EqualTo(PhoneApp.Call));
            yield return WaitForStage(KosPrologStage.PreparationAndDrawerPuzzle);
            Assert.That(campaign.Data.wallet_bound, Is.EqualTo(5000000));
            Assert.That(Find<SmartphoneController>().isPhoneRaised,Is.False);
            Assert.That(campaign.HasInventoryItem("EVENT_P0_CONTRACT_READ"),Is.False);
            yield return Press(Key.Tab);view.Apps.Open(PhoneApp.Messages);yield return null;
            Assert.That(campaign.HasInventoryItem("EVENT_P0_CONTRACT_READ"),Is.True);
            yield return new WaitForSeconds(10.2f);
            view.Apps.Open(PhoneApp.Bank);yield return null;
            Assert.That(GameObject.Find("BankBalance").GetComponent<UnityEngine.UI.Text>().text,Does.Contain("5.000.000"));
            view.Apps.Open(PhoneApp.Inventory);yield return null;
            Assert.That(GameObject.Find("InventoryEntries").GetComponent<UnityEngine.UI.Text>().text,Does.Not.Contain("DONASI").And.Not.Contain("Rp"));
            // Actual user action, no direct edits to UiFocus or handset state.
            yield return Press(Key.Tab);yield return null;
            Assert.That(UiFocus.Phone,Is.False);
            yield return Interact(director.stickyNoteItem, new Vector3(.85f, .05f, -.05f));
            Assert.That(hud.documentBodyText.text, Does.Contain("Hari mendahului bulan"));
            CloseDocument();
            yield return Interact(director.wallCalendarItem, new Vector3(.85f, .05f, .5f));
            Assert.That(hud.documentBodyText.text, Does.Contain("tiga hari sebelumnya").And.Not.Contain("1408").And.Not.Contain("14 - 08"));
            CloseDocument();
            if (jacketFirst) yield return Interact(director.jacketItem, new Vector3(1.05f, 0.05f, -0.4f));
            yield return Interact(director.lockedDrawerItem, new Vector3(.95f, .05f, -.65f));
            FoundationAuditTests.Button(hud, "UnlockPadlockBtn").onClick.Invoke();
            Assert.That(director.isDrawerUnlocked, Is.False);
            Assert.That(hud.padlockFeedbackText.text, Does.Contain("salah"));
            FoundationAuditTests.Button(hud, "CancelPadlockBtn").onClick.Invoke();
            yield return Interact(director.lockedDrawerItem, new Vector3(.95f, .05f, -.65f));
            int[] digits = { 1, 4, 0, 8 };
            for (int slot = 0; slot < 4; slot++)
                for (int n = 0; n < digits[slot]; n++) FoundationAuditTests.Button(hud, "DigitUp_" + slot).onClick.Invoke();
            FoundationAuditTests.Button(hud, "UnlockPadlockBtn").onClick.Invoke();
            Assert.That(director.isDrawerUnlocked, Is.True);
            Assert.That(director.hasMotorKey,Is.False,"Motor keys remain on the desktop, not inside the drawer");
            Assert.That(campaign.HasInventoryItem("DOMPET_RAKA")&&campaign.HasInventoryItem("DIARY_RAKA"),Is.True);
            Assert.That(UiFocus.Phone||hud.IsModalOpen||Find<FirstPersonController>().MovementLocked,Is.False,"Closing the drawer releases world input");
            yield return WalkTo(new Vector3(.4f,.05f,-.15f));
            yield return UseFromCurrentFeet(director.motorKeyItem);
            if (!jacketFirst){yield return WalkTo(new Vector3(1.05f,.05f,-.4f));yield return UseFromCurrentFeet(director.jacketItem);}
            Assert.That(director.currentStage, Is.EqualTo(KosPrologStage.ReadyToDepart));
            Assert.That(campaign.Data.inventory, Is.EquivalentTo(new[] {
                "DOC_PROLOG_01_SURAT_KOS", "SMARTPHONE_RAKA", "KUNCI_MOTOR", "POWERBANK_KECIL_KOS", "JAKET_RAKA", "DOMPET_RAKA", "DIARY_RAKA",
                "EVENT_P0_CONTRACT_RECEIVED", "EVENT_P0_CONTRACT_READ", "EVENT_P0_CONTRACT_REACTION" }));
            yield return WalkTo(new Vector3(.9f,.05f,-.6f));yield return UseFromCurrentFeet(director.kosDoorItem);
            Assert.That(director.currentStage, Is.EqualTo(KosPrologStage.Completed));
            yield return new WaitForSeconds(4f);
            Assert.That(hud.blackFadeImage.color.a, Is.EqualTo(1f));
            Assert.That(hud.blackFadeImage.transform.GetSiblingIndex(), Is.EqualTo(hud.blackFadeImage.transform.parent.childCount - 1));
            Assert.That(Find<FirstPersonController>().MovementLocked, Is.True);
            Assert.That(campaign.LoadCheckpoint(), Is.True);
            Assert.That(campaign.Data.checkpoint_id, Is.EqualTo("PROLOG_KOS_COMPLETE"));
            Assert.That(campaign.Data.wallet_bound, Is.EqualTo(5000000));
        }

        [UnityTest]
        public IEnumerator ReloadRestoresKosObjectsBatteryAndWalletWithoutRegrantingConsumables() => RunInPlayerLoop(ReloadRestoresKosObjectsBatteryAndWalletWithoutRegrantingConsumablesRoutine());

        private IEnumerator ReloadRestoresKosObjectsBatteryAndWalletWithoutRegrantingConsumablesRoutine()
        {
            var campaign = Find<CampaignState>();
            campaign.AddInventoryItem("SMARTPHONE_RAKA");
            campaign.AddInventoryItem("JAKET_RAKA");
            campaign.AddInventoryItem("KUNCI_MOTOR");
            campaign.Data.battery = 42f;
            campaign.AddBoundDonation(5000000);
            Assert.That(campaign.SaveCheckpointAtomic("PROLOG_KOS_COMPLETE"), Is.True);
            yield return SceneManager.LoadSceneAsync(PrologKosSceneBuilder.ScenePath);
            yield return null;
            var director = Find<KamarKosPrologDirector>();
            Assert.That(director.currentStage, Is.EqualTo(KosPrologStage.ReadyToDepart));
            Assert.That(director.suratKosItem.gameObject.activeSelf, Is.False);
            Assert.That(director.smartphoneBedItem.gameObject.activeSelf, Is.False);
            Assert.That(director.jacketItem.gameObject.activeSelf, Is.False);
            Assert.That(director.lockedDrawerItem.CanInteract(), Is.False);
            Assert.That(Find<SmartphoneController>().batteryPercent, Is.EqualTo(42f));
            Assert.That(Find<CampaignState>().Data.wallet_bound, Is.EqualTo(5000000));
            Assert.That(Find<CampaignState>().HasInventoryItem("POWERBANK_KECIL_KOS"), Is.False);
        }

        [UnityTest]
        public IEnumerator DepartureCanRetryAfterSaveFailure() => RunInPlayerLoop(DepartureCanRetryAfterSaveFailureRoutine());

        private IEnumerator DepartureCanRetryAfterSaveFailureRoutine()
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);
            var director = Find<KamarKosPrologDirector>();
            director.StopAllCoroutines();
            director.currentStage = KosPrologStage.ReadyToDepart;
            var campaign = Find<CampaignState>();
            Assert.That(campaign.SaveCheckpointAtomic("GOOD"), Is.True);
            string temporaryPath = Path.Combine(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"), "layar_terakhir_save.json.tmp");
            Directory.CreateDirectory(temporaryPath);
            LogAssert.Expect(LogType.Error, new Regex("Gagal menyimpan checkpoint"));
            director.OnObjectInteracted(director.kosDoorItem);
            Assert.That(director.currentStage, Is.EqualTo(KosPrologStage.ReadyToDepart));
            Assert.That(campaign.Data.checkpoint_id, Is.EqualTo("GOOD"));
            Directory.Delete(temporaryPath);
            director.OnObjectInteracted(director.kosDoorItem);
            Assert.That(director.currentStage, Is.EqualTo(KosPrologStage.Completed));
            yield return null;
        }

        [UnityTest]
        public IEnumerator PhoneDrainModesDarkBroadcastAndZeroBatteryRecharge() => RunInPlayerLoop(PhoneDrainModesDarkBroadcastAndZeroBatteryRechargeRoutine());

        private IEnumerator PhoneDrainModesDarkBroadcastAndZeroBatteryRechargeRoutine()
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);
            var phone = Find<SmartphoneController>();
            phone.AcquireSmartphone();
            phone.isBroadcastConnected = true;
            phone.batteryPercent = 100f;
            yield return new WaitForSeconds(2f);
            float normalDrain = 100f - phone.batteryPercent;
            Assert.That(normalDrain, Is.InRange(0.07f, 0.14f));
            yield return Press(Key.Q);
            Assert.That(phone.currentScanMode, Is.EqualTo(ScanVisualMode.Normal));
            phone.arModuleUnlocked = phone.uvModuleUnlocked = true;
            yield return Press(Key.Q);
            Assert.That(phone.currentScanMode, Is.EqualTo(ScanVisualMode.AR));
            yield return Press(Key.R);
            Assert.That(phone.currentScanMode, Is.EqualTo(ScanVisualMode.UV));
            float before = phone.batteryPercent;
            yield return new WaitForSeconds(2f);
            Assert.That(before - phone.batteryPercent, Is.InRange(0.17f, 0.25f));
            yield return Press(Key.F);
            before = phone.batteryPercent;
            yield return new WaitForSeconds(2f);
            Assert.That(phone.batteryPercent, Is.EqualTo(before));
            Assert.That(phone.isBroadcastConnected, Is.True);
            phone.ConsumeBattery(100f);
            Find<CampaignState>().AddInventoryItem("POWERBANK_BESAR");
            yield return Press(Key.B);
            Assert.That(phone.batteryPercent, Is.EqualTo(80f));
            Assert.That(phone.isScreenOn, Is.False);
            yield return Press(Key.F);
            Assert.That(phone.phoneFlashlight.enabled, Is.True);
            yield return Press(Key.Tab);
            before = phone.batteryPercent;
            yield return new WaitForSeconds(2f);
            Assert.That(phone.batteryPercent, Is.EqualTo(before));
            Assert.That(phone.handPhoneMeshRoot.activeSelf, Is.False);
            Assert.That(phone.phoneFlashlight.enabled, Is.False);
            Assert.That(phone.isBroadcastConnected, Is.True);
        }

        [UnityTest]
        public IEnumerator NewBroadcastNotificationKeepsItsOwnDuration() => RunInPlayerLoop(NewBroadcastNotificationKeepsItsOwnDurationRoutine());

        private IEnumerator NewBroadcastNotificationKeepsItsOwnDurationRoutine()
        {
            var hud = Find<HudController>();
            hud.ShowBroadcastNotification("Old", 0.5f);
            yield return new WaitForSeconds(0.2f);
            hud.ShowBroadcastNotification("New", 2f);
            yield return new WaitForSeconds(0.7f);
            Assert.That(hud.broadcastToastText.text, Is.EqualTo("New"));
            yield return new WaitForSeconds(2f);
            Assert.That(hud.broadcastToastText.text, Is.Empty);
        }

        [UnityTest]
        public IEnumerator PhoneAppsAllowActualWasdMovement() => RunInPlayerLoop(PhoneAppsAllowActualWasdMovementRoutine());

        private IEnumerator PhoneAppsAllowActualWasdMovementRoutine()
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);Time.timeScale=1;
            var phone=Find<SmartphoneController>();phone.AcquireSmartphone();
            var view=Find<AtmajaPhoneView>();view.Apps.Open(PhoneApp.Home);yield return null;
            Assert.That(UiFocus.Phone,Is.True);Assert.That(UiFocus.BlocksMovement,Is.False);
            PlacePlayer(new Vector3(.6f,.08f,-.5f));var player=Find<FirstPersonController>();
            var from=player.transform.position;InputSystem.QueueStateEvent(_keyboard,new KeyboardState(Key.W));
            yield return new WaitForSeconds(.16f);yield return Press();
            Assert.That(Vector3.Distance(from,player.transform.position),Is.GreaterThan(.15f));
            Assert.That(phone.isPhoneRaised,Is.True,"Walking must not close the handset");
            Assert.That(PhoneDesktop.Contacts.Length,Is.GreaterThanOrEqualTo(3));
        }

        [UnityTest]
        public IEnumerator OverlappingTabletopPreventsStandingAndStepPop() => RunInPlayerLoop(OverlappingTabletopPreventsStandingAndStepPopRoutine());

        private IEnumerator OverlappingTabletopPreventsStandingAndStepPopRoutine()
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);Time.timeScale=1;
            PlacePlayer(new Vector3(.6f,.08f,-.5f));var player=Find<FirstPersonController>();
            player.crouchHeight=.78f;player.cameraCrouchY=.62f;
            InputSystem.QueueStateEvent(_keyboard,new KeyboardState(Key.LeftCtrl));yield return new WaitForSeconds(.5f);
            var table=GameObject.CreatePrimitive(PrimitiveType.Cube);table.name="RegressionLowTable";
            table.transform.position=player.transform.position+Vector3.up*.91f;table.transform.localScale=new Vector3(1.3f,.12f,1.3f);Physics.SyncTransforms();
            float foot=player.transform.position.y;yield return Press();yield return new WaitForSeconds(.6f);
            Assert.That(player.CanStandUp(),Is.False);Assert.That(player.IsCrouching,Is.True);
            Assert.That(player.GetComponent<CharacterController>().height,Is.LessThan(.84f));
            Assert.That(player.transform.position.y,Is.LessThan(foot+.05f),"Releasing CTRL under the table must not lift the player onto it");
            UnityEngine.Object.Destroy(table);yield return new WaitForSeconds(.5f);Assert.That(player.IsCrouching,Is.False);
        }

        [UnityTest]
        public IEnumerator MovementCollisionAndCrouchHeadroomUseRuntimePhysics() => RunInPlayerLoop(MovementCollisionAndCrouchHeadroomUseRuntimePhysicsRoutine());

        private IEnumerator MovementCollisionAndCrouchHeadroomUseRuntimePhysicsRoutine()
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);
            Time.timeScale = 1f;
            PlacePlayer(new Vector3(1.4f, .05f, 0f));
            var player = Find<FirstPersonController>();
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState(Key.W));
            yield return new WaitForSeconds(3f);
            // Same runtime collision check, now against the compact layout's cupboard front.
            float front=GameObject.Find("CupboardCollision").GetComponent<Collider>().bounds.min.z;
            var capsule=player.GetComponent<CharacterController>();
            float expected=front-capsule.radius-capsule.skinWidth;
            Assert.That(player.transform.position.z, Is.InRange(expected-.04f, expected+.04f));
            yield return Press(Key.LeftCtrl);
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState(Key.LeftCtrl));
            yield return new WaitForSeconds(0.3f);
            Assert.That(player.IsCrouching, Is.True);
            Assert.That(player.GetComponent<CharacterController>().height, Is.LessThan(1.15f));
            var ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ceiling.transform.position = player.transform.position + Vector3.up * 1.5f;
            ceiling.transform.localScale = new Vector3(1f, 0.1f, 1f);
            Physics.SyncTransforms();
            yield return Press();
            yield return new WaitForSeconds(0.2f);
            Assert.That(player.IsCrouching, Is.True);
            UnityEngine.Object.Destroy(ceiling);
            yield return new WaitForSeconds(0.4f);
            Assert.That(player.IsCrouching, Is.False);
            Assert.That(player.GetComponent<CharacterController>().height, Is.GreaterThan(1.6f));
        }

        [UnityTest]
        public IEnumerator PowerbankKeyWorksWithStoredPhone() => RunInPlayerLoop(RechargePhone(false));

        [UnityTest]
        public IEnumerator PowerbankKeyWorksWithDeadPhone() => RunInPlayerLoop(RechargePhone(true));

        private IEnumerator RechargePhone(bool raised)
        {
            yield return WaitForStage(KosPrologStage.WaitReadSuratIbuKos);
            var phone = Find<SmartphoneController>();
            phone.AcquireSmartphone();
            phone.ConsumeBattery(100f);
            phone.isPhoneRaised = raised;
            Find<CampaignState>().AddInventoryItem("POWERBANK_KECIL_KOS");
            yield return Press(Key.B);
            Assert.That(phone.batteryPercent, Is.EqualTo(35f));
            Assert.That(Find<CampaignState>().HasInventoryItem("POWERBANK_KECIL_KOS"), Is.False);
        }

        private static IEnumerator RunInPlayerLoop(IEnumerator scenario)
        {
            var hostObject = new GameObject("FoundationAuditCoroutineHost");
            UnityEngine.Object.DontDestroyOnLoad(hostObject);
            var host = hostObject.AddComponent<FoundationAuditCoroutineHost>();
            bool finished = false;
            Exception failure = null;
            host.StartCoroutine(DriveScenario(scenario, error => { failure = error; finished = true; }));
            float deadline = Time.realtimeSinceStartup + 45f;
            while (!finished && Time.realtimeSinceStartup < deadline) yield return null;
            UnityEngine.Object.Destroy(hostObject);
            Assert.That(finished, Is.True, "Runtime scenario timed out");
            if (failure != null) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
        }

        private static IEnumerator DriveScenario(IEnumerator scenario, Action<Exception> complete)
        {
            var stack = new Stack<IEnumerator>();
            stack.Push(scenario);
            while (stack.Count > 0)
            {
                object instruction = null;
                Exception failure = null;
                bool advanced = false;
                try
                {
                    advanced = stack.Peek().MoveNext();
                    if (advanced) instruction = stack.Peek().Current;
                }
                catch (Exception exception) { failure = exception; }
                if (failure != null) { complete(failure); yield break; }
                if (!advanced) { stack.Pop(); continue; }
                if (instruction is IEnumerator nested) stack.Push(nested);
                else yield return instruction;
            }
            complete(null);
        }

        private IEnumerator Interact(KosInteractableItem item, Vector3 position)
        {
            PlacePlayer(position);
            var player=Find<FirstPersonController>();bool wasEnabled=player.enabled;player.enabled=false;
            var shoulder=player.GetComponent<ShoulderCameraRig>();if(shoulder)shoulder.RefreshCameraPose();
            var interactor = Find<InteractionInteractor>();
            interactor.playerCamera.transform.LookAt(item.transform.position);
            Physics.SyncTransforms();
            yield return null;
            var camera = interactor.playerCamera.transform;
            Physics.Raycast(camera.position, camera.forward, out var hit, interactor.interactionDistance);
            Assert.That(interactor.CurrentTarget, Is.SameAs(item),
                $"Raycast cannot reach {item.itemId}; hit={hit.collider?.name}; origin={camera.position}; direction={camera.forward}; modal={Find<HudController>().IsModalOpen}; inputSingleton={GameInputConfig.Instance != null}");
            var obstruction = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstruction.transform.position = camera.position + camera.forward * 0.4f;
            obstruction.transform.localScale = Vector3.one * 0.1f;
            Physics.SyncTransforms();
            yield return null;
            Assert.That(interactor.CurrentTarget, Is.Null, "World obstruction must block interaction");
            UnityEngine.Object.Destroy(obstruction);
            yield return null;
            yield return Press(Key.E);
            player.enabled=wasEnabled;
        }

        private IEnumerator WalkTo(Vector3 target)
        {
            var player=Find<FirstPersonController>();var cc=player.GetComponent<CharacterController>();player.enabled=false;
            for(int i=0;i<100;i++)
            {
                var delta=target-player.transform.position;delta.y=0;if(delta.magnitude<.08f)break;
                cc.Move(Vector3.ClampMagnitude(delta,.08f)+Vector3.down*.08f);yield return null;
            }
            var remaining=target-player.transform.position;remaining.y=0;player.enabled=true;
            Assert.That(remaining.magnitude,Is.LessThan(.14f),"Preparation items and exit must be reachable with continuous floor walking");
        }
        private IEnumerator UseFromCurrentFeet(KosInteractableItem item)
        {
            var player=Find<FirstPersonController>();bool wasEnabled=player.enabled;player.enabled=false;
            var shoulder=player.GetComponent<ShoulderCameraRig>();if(shoulder)shoulder.RefreshCameraPose();
            var interactor=Find<InteractionInteractor>();interactor.playerCamera.transform.LookAt(item.GetComponent<Collider>().bounds.center);Physics.SyncTransforms();yield return null;
            Assert.That(interactor.CurrentTarget,Is.SameAs(item),"Preparation item must be first-hit reachable from the aisle: "+item.name);
            yield return Press(Key.E);
            player.enabled=wasEnabled;
        }

        private static void PlacePlayer(Vector3 position)
        {
            var player = Find<FirstPersonController>();
            var controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.SetPositionAndRotation(position, Quaternion.identity);
            player.cameraPivot.localRotation = Quaternion.identity;
            Find<InteractionInteractor>().playerCamera.transform.localRotation = Quaternion.identity;
            controller.enabled = true;
        }

        private IEnumerator Press(params Key[] keys)
        {
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState(keys));
            yield return null;
            foreach (Key key in keys)
                Assert.That(_keyboard[key].isPressed, Is.True, "Synthetic keyboard event was not delivered: " + key);
            InputSystem.QueueStateEvent(_keyboard, new KeyboardState());
            yield return null;
        }

        private static IEnumerator WaitForStage(KosPrologStage stage)
        {
            float deadline = Time.realtimeSinceStartup + 20f;
            while (Find<KamarKosPrologDirector>().currentStage != stage && Time.realtimeSinceStartup < deadline)
                yield return null;
            Assert.That(Find<KamarKosPrologDirector>().currentStage, Is.EqualTo(stage));
        }

        private static void CloseDocument()
        {
            var hud = Find<HudController>();
            hud.documentCloseButton.onClick.Invoke();
            Assert.That(hud.IsModalOpen, Is.False);
            // World-object modal owns lowering the phone; the test must not clear focus itself.
        }

        private static T Find<T>() where T : UnityEngine.Object => UnityEngine.Object.FindFirstObjectByType<T>();
    }
}
#endif
