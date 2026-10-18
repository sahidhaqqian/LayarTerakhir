#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class AtmajaPlayTests
    {
        private Keyboard keyboard;
        [UnitySetUp] public IEnumerator SetUp()
        {
            SessionState.SetString("AtmajaAudit.PreviousSave",Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")??"");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",Path.Combine(Path.GetTempPath(),"AtmajaPlayTest-"+Guid.NewGuid()));
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            yield return new EnterPlayMode();
            SessionState.SetString("AtmajaAudit.Input",JsonUtility.ToJson(InputSystem.settings));
            var settings=Object.Instantiate(InputSystem.settings);
            settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            InputSystem.settings=settings;keyboard=InputSystem.AddDevice<Keyboard>();Time.timeScale=5;
            yield return null;
        }
        [UnityTearDown] public IEnumerator TearDown()
        {
            if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
            var old=InputSystem.settings;var settings=ScriptableObject.CreateInstance<InputSettings>();
            JsonUtility.FromJsonOverwrite(SessionState.GetString("AtmajaAudit.Input","{}"),settings);InputSystem.settings=settings;Object.Destroy(old);
            Time.timeScale=1;yield return new ExitPlayMode();
            var directory=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",SessionState.GetString("AtmajaAudit.PreviousSave",""));
            if(!string.IsNullOrEmpty(directory)&&Directory.Exists(directory))Directory.Delete(directory,true);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
        [UnityTest] public IEnumerator SurveyLiveDonationCrouchAndRemappedVault()=>Run(Exterior());
        [UnityTest] public IEnumerator OnePhoneARClockZeroBatteryAndCheckpointResume()=>Run(Interior());
        [UnityTest] public IEnumerator PhoneAppsExposeCampaignAndAllowStowing()=>Run(PhoneApps());
        [UnityTest] public IEnumerator KosBothStairFlightsAndSharedRoomsAreContinuouslyWalkable()=>Run(KosNavigation());
        [UnityTest] public IEnumerator DiningSidesBackpackAndCornerRoomAreContinuouslyWalkable()=>Run(DiningNavigation());
        [UnityTest] public IEnumerator UpperEvidenceUsesPhoneLensAndAcceptsTargetSurface()=>Run(UpperPhonePhotos());
        [UnityTest] public IEnumerator AtmajaStairsReachUpperCorridorWithoutTeleport()=>Run(UpperStairNavigation());
        [UnityTest] public IEnumerator BasementPowerAndDonationSurviveContinueWithoutDuplication()=>Run(BasementPower());
        [UnityTest] public IEnumerator HunterSeesThePlayerButNeverThroughArchiveShelves()=>Run(HunterSight());
        [UnityTest] public IEnumerator LockerWarningAllowsScreenInputAndSafeManualExit()=>Run(LockerInput());
        [UnityTest] public IEnumerator InterruptedHandycamCanBePlayedAgainAfterContinue()=>Run(InterruptedRecording());
        [UnityTest] public IEnumerator FinishedPrologRestoresTheTitleWithoutAnotherDonation()=>Run(FinishedProlog());
        [UnityTest] public IEnumerator MissedEmergencyBlitzReturnsToMenuAndContinueRestoresChase()=>Run(BlitzRetry());
        [UnityTest] public IEnumerator MasterCellarAndClosingTrapdoorHaveConnectedCapsuleRoutes()=>Run(FinaleGeometry());
        [UnityTest] public IEnumerator HybridCameraUsesRemappedInputAndPreservesChoiceDuringForcedCloseups()=>Run(HybridCamera());
        [UnityTest] public IEnumerator EveryOpenedDocumentPersistsItsFullTextInPhoneNotes()=>Run(DocumentArchive());
        [UnityTest] public IEnumerator JacketPickupChangesTheBodyAndShutterPulsesTheHandset()=>Run(OutfitAndShutter());
        [UnityTest] public IEnumerator LiveCommentsArriveIndividuallyAtTwoToThreeSecondIntervals()=>Run(StaggeredChat());
        [UnityTest] public IEnumerator WardrobeExitDoesNotReenterOnTheSameInteractPress()=>Run(WardrobeExitDebounce());
        [UnityTest] public IEnumerator ExpandedCellarHasRunnableConnectedCorridors()=>Run(ExpandedCellarNavigation());
        [UnityTest] public IEnumerator NeighborhoodIsWalkableBeforeTravelAndShopPurchasesPersist()=>Run(NeighborhoodShop());
        [UnityTest] public IEnumerator HunterDoesNotAbandonStationaryRakaForTheWindow()=>Run(HunterTracksRaka());
        private IEnumerator HunterTracksRaka()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var c=CampaignState.Instance;
            foreach(var id in new[]{"EVENT_P0_BASEMENT_ENTERED","EVENT_P0_CHASE_STARTED","EVENT_P0_HANDYCAM_PLAYED","EVENT_P0_POWERBANK_TAKEN",AtmajaBasementHorror.HandycamId})c.AddInventoryItem(id);
            Assert.That(c.SaveCheckpointAtomic(AtmajaChaseSequence.ChaseCheckpoint),Is.True);d.RestoreWorld();
            d.PlacePlayer(d.P(1.4f,5.45f,-2.79f),Quaternion.identity);
            // Occlude sight, so this checks pursuit routing rather than death UI.
            var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.transform.SetPositionAndRotation(d.P(1.4f,5.45f,-1.6f),Quaternion.LookRotation(d.north));wall.transform.localScale=new Vector3(2.5f,2.4f,2.5f);Physics.SyncTransforms();
            yield return new WaitForSeconds(12);
            Assert.That(d.chase.FollowsPlayerInBasement,Is.True,"The hunter keeps pursuing the sheltered basement player");
            Assert.That(d.chase.chaserFigure.transform.position.y,Is.LessThan(-2),"The basement hunter must remain on the basement floor with Raka");Object.DestroyImmediate(wall);
        }
        [UnityTest] public IEnumerator OpeningTheWellPullsRakaDownAndWaitsAtTheMainMenu()=>Run(WellDeath());
        private IEnumerator DocumentArchive()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var c=CampaignState.Instance;var phone=SmartphoneController.Instance;var hud=HudController.Instance;
            const string title="CATATAN REVIEW LENGKAP",body="Halaman pertama.\n\nBaris rahasia di bagian akhir: jam tidak pernah melupakan penghuninya.";
            phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            hud.OpenDocumentModal(title,body);yield return null;
            Assert.That(c.Data.documents.Single(n=>n.title==title).body,Is.EqualTo(body));
            Assert.That(phone.isPhoneRaised&&phone.isScreenOn&&d.phoneView.IsCameraApp,Is.True);
            hud.CloseDocumentModal();Assert.That(c.LoadCheckpoint(),Is.True);d.phoneView.Apps.Open(PhoneApp.Journal);yield return null;
            var entry=Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).Single(b=>b.name.StartsWith("JournalEntry_")&&b.GetComponentInChildren<UnityEngine.UI.Text>().text.Contains(title));
            entry.onClick.Invoke();yield return null;
            Assert.That(GameObject.Find("EntryBody").GetComponentInChildren<UnityEngine.UI.Text>().text,Is.EqualTo(body));
            Assert.That(hud.CurrentObjective,Is.Empty);Assert.That(hud.objectiveText.gameObject.activeSelf,Is.False);
            hud.OpenDocumentModal(title,body);hud.CloseDocumentModal();Assert.That(c.Data.documents.Count(n=>n.title==title),Is.EqualTo(1));
        }
        private IEnumerator OutfitAndShutter()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var c=CampaignState.Instance;var phone=SmartphoneController.Instance;
            var rig=d.player.GetComponent<ShoulderCameraRig>();c.Data.inventory.Remove("JAKET_RAKA");yield return null;yield return null;
            var renderers=rig.Avatar.GetComponentsInChildren<Renderer>(true);var shirt=renderers.Single(r=>r.name=="RakaTShirt");var jacket=renderers.Single(r=>r.name=="RakaJacketOutfit");
            Assert.That(shirt.enabled&&!jacket.enabled,Is.True,"The opening outfit must actually show the T-shirt");
            c.AddInventoryItem("JAKET_RAKA");yield return null;yield return null;Assert.That(jacket.enabled&&!shirt.enabled,Is.True);
            d.player.enabled=false;yield return null;yield return null;Assert.That(jacket.enabled&&!shirt.enabled,Is.True,"A contextual traversal must not render both outfits");d.player.enabled=true;
            phone.isPhoneRaised=phone.isScreenOn=phone.isBroadcastConnected=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            GameInputConfig.Instance.RemapAction(GameAction.CaptureOrBlitz,KeyCode.P);yield return null;
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.P));yield return null;yield return null;
            Assert.That(phone.phoneFlashlight.enabled&&phone.phoneFlashlight.intensity>40,Is.True,"A shutter press must pulse the physical lamp");
            var flash=GameObject.Find("PhoneShutterFlash").GetComponent<UnityEngine.UI.Image>();Assert.That(flash.color.a,Is.GreaterThan(0));
            phone.isScreenOn=false;phone.SyncVisualState();InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
            Assert.That(phone.phoneFlashlight.enabled,Is.False,"An interrupted flash must respect an off screen");
        }
        private IEnumerator StaggeredChat()
        {
            yield return new WaitForSeconds(2);Time.timeScale=1;var view=Find<AtmajaPhoneView>();var phone=SmartphoneController.Instance;
            phone.isBroadcastConnected=true;view.AddChat("@review_a","Pesan satu");view.AddChat("@review_b","Pesan dua");view.AddChat("@review_c","Pesan tiga");
            yield return null;yield return null;int count=view.DisplayedChatCount;float at=view.LastChatTime;
            Assert.That(count,Is.EqualTo(1));yield return new WaitForSeconds(1.7f);Assert.That(view.DisplayedChatCount,Is.EqualTo(count));
            float end=Time.time+1.5f;while(view.DisplayedChatCount==count&&Time.time<end)yield return null;
            Assert.That(view.DisplayedChatCount,Is.EqualTo(count+1));Assert.That(view.LastChatTime-at,Is.InRange(2f,3.12f));
        }
        private IEnumerator WardrobeExitDebounce()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var b=d.basement;var c=CampaignState.Instance;
            c.AddInventoryItem("EVENT_P0_BASEMENT_ENTERED");c.SaveCheckpointAtomic("PROLOG_BASEMENT_ENTRY");d.RestoreWorld();
            b.nyonya.SetActive(false);d.player.enabled=true;d.PlacePlayer(b.exitAnchor1.position,b.exitAnchor1.rotation);
            b.wardrobeCol1.GetComponent<AtmajaItem>().Interact();yield return new WaitForSeconds(.4f);Assert.That(b.IsHidden,Is.True);
            yield return Press(Key.E);yield return null;Assert.That(b.IsHidden,Is.False);yield return new WaitForSeconds(.3f);Assert.That(b.IsHidden,Is.False);
            Assert.That(d.player.MovementLocked,Is.False);Assert.That(Vector3.Distance(d.player.transform.position,b.exitAnchor1.position),Is.LessThan(.25f));
        }
        private IEnumerator ExpandedCellarNavigation()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();d.basement.enabled=false;d.basement.nyonya.SetActive(false);d.PlacePlayer(d.P(4.9f,6,-2.79f),Quaternion.identity);
            foreach(var p in new[]{d.P(7.2f,6,-2.79f),d.P(7.2f,11.5f,-2.79f),d.P(10.1f,11.5f,-2.79f),d.P(10.1f,2.1f,-2.79f),d.P(7.2f,2.1f,-2.79f),d.P(7.2f,6,-2.79f)})yield return WalkCapsule(d.player,p);
        }
        private IEnumerator NeighborhoodShop()
        {
            yield return new WaitForSeconds(2);var c=CampaignState.Instance;c.Data.inventory.Remove("POWERBANK_KECIL_KOS");c.Data.wallet_legitimate=100000;c.Data.wallet_bound=0;c.Data.spent=0;
            Assert.That(c.SaveCheckpointAtomic("PROLOG_KOS_YARD"),Is.True);yield return SceneManager.LoadSceneAsync(PrologSceneFlow.CorridorScene);yield return new WaitForSeconds(2);
            var corridor=Find<KosCorridorDirector>();var street=Find<MelatiNeighborhood>();var p=corridor.player;
            var cc=p.GetComponent<CharacterController>();cc.enabled=false;p.transform.position=corridor.motor.position+corridor.motor.right*.9f;p.transform.position=new Vector3(p.transform.position.x,.08f,p.transform.position.z);cc.enabled=true;
            Assert.That(corridor.CanDepart,Is.True);
            foreach(var point in new[]{new Vector3(0,.04f,-5),new Vector3(0,.04f,-12)})yield return WalkCapsule(p,point);
            yield return null;Assert.That(street.Reached,Is.True);
            Assert.That(SceneManager.GetActiveScene().name,Is.EqualTo(PrologSceneFlow.CorridorScene),"The neighborhood is reached on foot without starting a ride");
            foreach(var point in new[]{new Vector3(1.15f,.04f,-24),new Vector3(4.9f,.04f,-24)})yield return WalkCapsule(p,point);
            var shop=Find<MelatiShop>();var eye=p.GetComponent<InteractionInteractor>().playerCamera;p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();eye.transform.LookAt(shop.GetComponent<Collider>().bounds.center);yield return null;yield return null;
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(shop));yield return Press(Key.E);Assert.That(shop.IsOpen,Is.True);
            Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).Single(b=>b.name=="ShopBuy_POWERBANK_KECIL_KOS").onClick.Invoke();
            c=CampaignState.Instance;Assert.That(c.Data.wallet_legitimate,Is.EqualTo(55000));Assert.That(c.Data.spent,Is.EqualTo(45000));Assert.That(c.HasInventoryItem("POWERBANK_KECIL_KOS"),Is.True);
            Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.wallet_legitimate,Is.EqualTo(55000));Assert.That(shop.Buy("POWERBANK_KECIL_KOS",45000),Is.False);Assert.That(c.Data.spent,Is.EqualTo(45000));
            shop.Close();Assert.That(UiFocus.Clock,Is.False);
            foreach(var point in new[]{new Vector3(0,.04f,-24),new Vector3(0,.04f,-50),new Vector3(23,.04f,-50),new Vector3(0,.04f,-50),new Vector3(0,.04f,-87)})yield return WalkCapsule(p,point);
            Assert.That(Find<NeighborhoodRain>().GetComponent<ParticleSystem>().isPlaying,Is.True);
        }
        private IEnumerator WellDeath()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var c=CampaignState.Instance;c.AddInventoryItem("EVENT_P0_KITCHEN_ENTERED");c.SaveCheckpointAtomic("PROLOG_ATMAJA_KITCHEN");d.RestoreWorld();
            d.PlacePlayer(d.P(3.9f,11.2f,.36f),Quaternion.LookRotation(-d.east));yield return new WaitForSeconds(3);yield return new WaitUntil(()=>!d.Busy);
            var center=d.P(2.85f,11.2f,.3f);Assert.That(Physics.RaycastAll(center,Vector3.down,3.5f).Where(h=>!h.transform.IsChildOf(d.player.transform)).Any(h=>h.collider.name.Contains("Floor")||h.collider.name.Contains("PLINTH")),Is.False,"The opened well needs an actual hole through all floor slabs");
            Item(AtmajaAction.Well).Interact();yield return new WaitUntil(()=>GameShell.AwaitingStart);
            Assert.That(UiFocus.Menu&&Time.timeScale==0,Is.True);Assert.That(d.player.transform.position.y,Is.LessThan(-2));
            yield return new WaitForSecondsRealtime(.25f);Assert.That(GameShell.AwaitingStart,Is.True,"The well death must not automatically restart the kitchen");
        }
        private IEnumerator HybridCamera()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var p=d.player;
            var rig=p.GetComponent<ShoulderCameraRig>();var input=GameInputConfig.Instance;
            var eye=p.GetComponent<InteractionInteractor>().playerCamera;
            d.PlacePlayer(d.P(8.2f,-2,.06f),Quaternion.LookRotation(d.north));p.enabled=true;
            yield return null;yield return null;
            Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.OverShoulder));
            Assert.That(input.GetKeyForAction(GameAction.TogglePerspective),Is.EqualTo(KeyCode.V));
            Assert.That(input.modeSelfieKey,Is.EqualTo(KeyCode.X),"Old serialized V selfie binding must migrate without colliding with perspective");
            var avatar=rig.Avatar.GetComponent<AnimatedRakaAvatar>();Assert.That(avatar,Is.Not.Null);
            Assert.That(avatar.motion.GetClipCount(),Is.EqualTo(12));
            Assert.That(rig.Avatar.GetComponentsInChildren<SkinnedMeshRenderer>().Length,Is.GreaterThanOrEqualTo(5));
            Assert.That(d.phoneView.FeedCamera.cullingMask&(1<<ShoulderCameraRig.AvatarLayer),Is.Zero,"Raka's shoulder/handset must not obscure the separate phone feed");
            var bounce=p.transform.Find("RakaSoftBounce").GetComponent<Light>();
            Assert.That(bounce.cullingMask,Is.EqualTo(1<<ShoulderCameraRig.AvatarLayer));Assert.That(bounce.shadows,Is.EqualTo(LightShadows.None));
            Assert.That((uint)bounce.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>().renderingLayers,Is.EqualTo(ShoulderCameraRig.AvatarRenderingLayer));
            Assert.That(rig.Avatar.GetComponentsInChildren<Renderer>(true).All(r=>(r.renderingLayerMask&ShoulderCameraRig.AvatarRenderingLayer)!=0),Is.True,"Only avatar renderers opt into the body fill's URP rendering layer");
            long wallet=CampaignState.Instance.Data.wallet_bound;var checkpoint=CampaignState.Instance.Data.checkpoint_id;
            yield return Press(Key.V);
            Assert.That(rig.IsFirstPerson,Is.True);Assert.That(Vector3.Distance(eye.transform.position,p.cameraPivot.position),Is.LessThan(.001f));
            Assert.That(bounce.enabled,Is.False,"First-person does not need the visual-only body fill");
            Assert.That(rig.Avatar.GetComponentsInChildren<Renderer>(true).All(r=>r.shadowCastingMode==UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly),Is.True);
            yield return Press(Key.V);Assert.That(rig.IsFirstPerson,Is.False);Assert.That(rig.CurrentDistance,Is.GreaterThan(1));
            rig.FirstPersonOverride=true;rig.RefreshCameraPose();yield return Press(Key.V);
            Assert.That(rig.IsFirstPerson,Is.True);Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.OverShoulder));
            rig.FirstPersonOverride=false;rig.RefreshCameraPose();Assert.That(rig.IsFirstPerson,Is.False);
            rig.SetPerspective(PlayerPerspective.FirstPerson);rig.FirstPersonOverride=true;rig.FirstPersonOverride=false;
            Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.FirstPerson));
            input.RemapAction(GameAction.TogglePerspective,KeyCode.L);yield return Press(Key.V);
            Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.FirstPerson));
            yield return Press(Key.L);Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.OverShoulder));
            input.RemapAction(GameAction.TogglePerspective,KeyCode.V);
            GameShell.Instance.ShowSettings();yield return null;
            var setting=GameObject.Find("PerspectiveMode").GetComponent<UnityEngine.UI.Button>();setting.onClick.Invoke();yield return null;
            Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.FirstPerson));
            GameShell.Instance.Resume();rig.SetPerspective(PlayerPerspective.OverShoulder);p.enabled=false;
            var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.name="HybridCameraOccluder";
            wall.transform.SetPositionAndRotation(p.cameraPivot.TransformPoint(0,0,-.55f),p.transform.rotation);wall.transform.localScale=new Vector3(4,3,.12f);
            Physics.SyncTransforms();rig.RefreshCameraPose();Assert.That(rig.CurrentDistance,Is.LessThan(.9f));
            Assert.That(rig.Avatar.GetComponentsInChildren<Renderer>(true).All(r=>r.shadowCastingMode==UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly),Is.True,"A squeezed camera must not render inside Raka's head");
            Object.DestroyImmediate(wall);Physics.SyncTransforms();rig.RefreshCameraPose();Assert.That(rig.CurrentDistance,Is.GreaterThan(1));
            var feet=p.transform.position;rig.Riding=true;rig.RefreshCameraPose();
            Assert.That(rig.Avatar.localPosition.y,Is.GreaterThan(.25f),"The seated clip compensates for the existing motor mount anchor");
            Assert.That(p.transform.position,Is.EqualTo(feet),"Rider pose compensation must only move the visual mesh");
            rig.Riding=false;rig.RefreshCameraPose();Assert.That(rig.Avatar.localPosition,Is.EqualTo(Vector3.zero));
            Assert.That(CampaignState.Instance.Data.wallet_bound,Is.EqualTo(wallet));Assert.That(CampaignState.Instance.Data.checkpoint_id,Is.EqualTo(checkpoint));
        }
        private IEnumerator FinaleGeometry()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var p=d.player;var cc=p.GetComponent<CharacterController>();
            d.PlacePlayer(d.P(1.3f,5.75f,3.83f),Quaternion.identity);
            foreach(var point in new[]{d.P(1.1f,4.25f,3.83f),d.P(.8f,4.25f,3.83f),d.P(.8f,2.6f,3.83f)})yield return WalkCapsule(p,point);
            d.PlacePlayer(d.basement.spawnBasement.position,d.basement.spawnBasement.rotation);yield return null;yield return null;
            // This test isolates physical clearance. Other tests exercise the live hunter/locker.
            d.basement.StopAllCoroutines();d.basement.enabled=false;d.basement.nyonya.SetActive(false);
            foreach(var point in new[]{d.basement.exitAnchor1.position,d.P(1.45f,4,-2.79f),d.P(1.45f,5.4f,-2.79f),d.P(2.1f,5.45f,-2.79f)})yield return WalkCapsule(p,point);
            var item=Item(AtmajaAction.Powerbank);var eye=p.GetComponent<InteractionInteractor>().playerCamera;
            p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            eye.transform.LookAt(item.GetComponent<Collider>().bounds.center);yield return null;yield return null;
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(item),"Bench powerbank is visible and reachable from its front");
            d.SetStage(AtmajaStage.Chase);d.chase.cellarEgressFloor.SetActive(true);d.chase.slideGate.SetActive(true);
            d.PlacePlayer(d.upper.trapdoorTop.position,d.upper.trapdoorTop.rotation);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));p.enabled=true;yield return new WaitForSeconds(.5f);p.enabled=false;
            Assert.That(p.IsCrouching,Is.True);
            yield return WalkCapsule(p,d.P(1,2.5f,3.83f));yield return WalkCapsule(p,d.P(.8f,2.7f,3.83f));
            Assert.That(p.transform.position.y,Is.GreaterThan(3.75f),"Closing trapdoor landing must not drop the player back into the shaft");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());p.enabled=true;yield return new WaitForSeconds(.5f);p.enabled=false;
            Assert.That(p.IsCrouching,Is.False);
            d.upper.bathDoor.SetOpenImmediate(true);d.upper.childDoor.SetOpenImmediate(true);
            foreach(var point in new[]{d.P(.8f,4.25f,3.83f),d.P(1.1f,4.7f,3.83f),d.P(1.3f,5.75f,3.83f),d.P(-3.3f,5.75f,3.83f)})yield return WalkCapsule(p,point);
        }
        private IEnumerator BlitzRetry()
        {
            yield return new WaitForSeconds(2);Time.timeScale=1;var d=SeedBasement();var c=CampaignState.Instance;
            foreach(var id in new[]{"EVENT_P0_BASEMENT_ENTERED","EVENT_P0_CHASE_STARTED","EVENT_P0_HANDYCAM_PLAYED","EVENT_P0_POWERBANK_TAKEN",AtmajaBasementHorror.HandycamId})c.AddInventoryItem(id);
            c.Data.battery=80;Assert.That(c.SaveCheckpointAtomic(AtmajaChaseSequence.ChaseCheckpoint),Is.True);d.RestoreWorld();
            SmartphoneController.Instance.batteryPercent=80;yield return new WaitForSeconds(1.8f);
            d.PlacePlayer(d.chase.cellarExitApproach.position,Quaternion.identity);
            d.player.enabled=true;
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftShift));yield return null;yield return new WaitForSeconds(2.6f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());Assert.That(d.chase.Traversing,Is.False);
            d.PlacePlayer(d.chase.diningVaultStart.position,Quaternion.identity);yield return Press(Key.Space);yield return new WaitForSeconds(1);
            d.PlacePlayer(d.vaultInside.position,d.vaultInside.rotation);yield return null;yield return null;
            Assert.That(d.chase.AwaitingBlitz,Is.True);Assert.That(Time.timeScale,Is.EqualTo(.15f));
            GameShell.Instance.Pause();yield return new WaitForSecondsRealtime(3.2f);
            Assert.That(d.chase.AwaitingBlitz,Is.True,"Pause must freeze the real-time QTE deadline");GameShell.Instance.Resume();
            yield return new WaitForSecondsRealtime(3.3f);
            Assert.That(GameShell.AwaitingStart&&UiFocus.Menu,Is.True,"Failure must wait in the main menu");
            Assert.That(Time.timeScale,Is.Zero);var deathPosition=d.player.transform.position;
            yield return new WaitForSecondsRealtime(.25f);Assert.That(d.player.transform.position,Is.EqualTo(deathPosition));
            GameShell.Instance.Continue();
            yield return new WaitUntil(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene&&Find<AtmajaDirector>()!=d);
            yield return new WaitForSeconds(2);d=Find<AtmajaDirector>();c=CampaignState.Instance;
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.Chase));Assert.That(Time.timeScale,Is.EqualTo(1));
            Assert.That(SmartphoneController.Instance.batteryPercent,Is.InRange(79.5f,80));
            Assert.That(d.GetComponents<AudioSource>().Any(a=>a.clip&&a.clip.name=="AtmajaSynth_ChaseLoop"&&a.isPlaying),Is.True,"Explicit Continue restarts chase music");
            Assert.That(Vector3.Distance(d.player.transform.position,d.chase.spawnChase.position),Is.LessThan(.3f));
            // Recovery covers the window traverse and the mandatory crouch under
            // the branch, while the window action itself still has four seconds.
            yield return new WaitForSeconds(1.8f);d.PlacePlayer(d.chase.cellarExitApproach.position,Quaternion.identity);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftShift));yield return null;yield return new WaitForSeconds(2.6f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());d.PlacePlayer(d.chase.diningVaultStart.position,Quaternion.identity);
            yield return Press(Key.Space);yield return new WaitForSeconds(1);d.PlacePlayer(d.vaultInside.position,d.vaultInside.rotation);yield return null;yield return null;
            Assert.That(d.chase.TryEmergencyBlitz(),Is.True);yield return Press(Key.Space);yield return new WaitForSeconds(1.2f);
            Assert.That(d.chase.IsGateRun&&d.chase.IsStunned,Is.True,"Leaving the window must not cancel the remaining Blitz stun");
            var stopped=d.chase.chaserFigure.transform.position;yield return new WaitForSeconds(.6f);
            Assert.That(d.chase.chaserFigure.transform.position,Is.EqualTo(stopped),"The outdoor hunter must stay still until recovery");
            yield return new WaitForSeconds(2.3f);Assert.That(d.chase.IsStunned,Is.True,"Recovery must cover the mandatory branch crawl");
            yield return new WaitForSeconds(6);Assert.That(d.chase.IsStunned,Is.False,"Recovery must finish ten game seconds after firing");
        }
        private AtmajaDirector SeedBasement()
        {
            var d=Find<AtmajaDirector>();var c=CampaignState.Instance;
            foreach(var id in new[]{"SMARTPHONE_RAKA","MATABATIN_APK","EVENT_P0_GATE_OPEN","EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN","EVENT_P0_STAIRS_UNLOCKED","EVENT_P0_UPPER_REACHED","EVENT_P0_CHAIN1_BROKEN","EVENT_P0_CHAIN2_BROKEN","EVENT_P0_CHAIN3_BROKEN","EVENT_P0_MASTER_OPEN","EVENT_P0_CARPET_LIFTED","EVENT_P0_TRAPDOOR_OPEN"})c.AddInventoryItem(id);
            d.RestoreWorld();d.player.enabled=false;return d;
        }
        private IEnumerator BasementPower()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var c=CampaignState.Instance;var phone=SmartphoneController.Instance;
            phone.batteryPercent=c.Data.battery=63;long money=c.Data.wallet_bound;
            d.PlacePlayer(d.basement.spawnBasement.position,d.basement.spawnBasement.rotation);yield return null;yield return null;
            Assert.That(c.HasInventoryItem("EVENT_P0_POWER_GLITCH"),Is.True);Assert.That(phone.batteryPercent,Is.InRange(11.9f,12f));
            Assert.That(c.Data.wallet_bound,Is.EqualTo(money+2000000));
            var before=d.basement.nyonya.transform.position;Item(AtmajaAction.Powerbank).Interact();
            Assert.That(phone.batteryPercent,Is.InRange(79.9f,80f));Assert.That(d.basement.nyonya.transform.position,Is.EqualTo(before));
            Assert.That(c.HasInventoryItem(AtmajaBasementHorror.PowerbankId),Is.False,"Tutorial pack consumption must be persisted atomically");
            Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.battery,Is.EqualTo(80));
            Assert.That(c.Data.inventory.Any(id=>id.StartsWith("RESERVE_BATTERY_",StringComparison.Ordinal)),Is.False);
            d.RestoreWorld();Assert.That(c.Data.wallet_bound,Is.EqualTo(money+2000000));
            Assert.That(d.basement.powerbankObj.activeSelf,Is.True,"Used pack must still allow emergency USB recharging");
            phone.batteryPercent=0;Item(AtmajaAction.Powerbank).Interact();yield return new WaitForSeconds(3);
            Assert.That(phone.batteryPercent,Is.InRange(34.5f,35f));Assert.That(phone.isScreenOn,Is.True);
            Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.battery,Is.EqualTo(35));
        }
        private IEnumerator HunterSight()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var b=d.basement;var c=CampaignState.Instance;
            c.AddInventoryItem("EVENT_P0_BASEMENT_ENTERED");d.RestoreWorld();b.enabled=false;
            b.nyonya.transform.SetPositionAndRotation(d.P(1.4f,4.3f,-2.79f),Quaternion.LookRotation(d.north));
            d.PlacePlayer(d.P(1.4f,5.1f,-2.79f),Quaternion.identity);Physics.SyncTransforms();
            Assert.That(b.CanSeePlayer(8),Is.True,"Own/player hulls must not blind the hunter");
            Assert.That(d.chase.CanCatchPlayer(),Is.True,"A close unobstructed hunter can catch Raka");
            var blocker=GameObject.CreatePrimitive(PrimitiveType.Cube);blocker.name="TestCatchOccluder";
            blocker.transform.SetPositionAndRotation(d.P(1.4f,4.7f,-1.65f),Quaternion.LookRotation(d.north));blocker.transform.localScale=new Vector3(.5f,2.4f,.08f);Physics.SyncTransforms();
            Assert.That(d.chase.CanCatchPlayer(),Is.False,"Nearby stairs or furniture must block a chase capture");Object.DestroyImmediate(blocker);
            b.nyonya.transform.SetPositionAndRotation(d.P(1.4f,3.2f,-2.79f),Quaternion.LookRotation(d.east));
            d.PlacePlayer(d.P(4.3f,3.2f,-2.79f),Quaternion.identity);Physics.SyncTransforms();
            Assert.That(b.CanSeePlayer(8),Is.False,"Opaque archive shelf must actually break line of sight");
        }
        private IEnumerator LockerInput()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var b=d.basement;var c=CampaignState.Instance;var phone=SmartphoneController.Instance;
            c.AddInventoryItem("EVENT_P0_BASEMENT_ENTERED");c.AddInventoryItem("EVENT_P0_POWER_GLITCH");c.Data.battery=12;
            Assert.That(c.SaveCheckpointAtomic("PROLOG_BASEMENT_ENTRY"),Is.True);d.RestoreWorld();
            phone.batteryPercent=12;phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();
            b.nyonya.transform.position=b.exitAnchor1.position;Physics.SyncTransforms();
            var item=b.wardrobeCol1.GetComponent<AtmajaItem>();item.Interact();yield return null;
            Time.timeScale=1;Assert.That(b.IsHidden&&d.player.MovementLocked,Is.True);
            var rig=d.player.GetComponent<ShoulderCameraRig>();Assert.That(rig.IsFirstPerson,Is.True);Assert.That(rig.SelectedPerspective,Is.EqualTo(PlayerPerspective.OverShoulder));
            yield return new WaitForSeconds(.4f);Assert.That(b.InspectingLocker,Is.True,"Warning must start before the screen can expose Raka");
            yield return Press(Key.F);Assert.That(phone.isScreenOn,Is.False,"The real screen binding remains live while hidden");
            yield return new WaitForSeconds(5.4f);Assert.That(b.IsHidden,Is.True);Assert.That(b.InspectingLocker,Is.False);
            var at=b.nyonya.transform.position;yield return new WaitForSeconds(.7f);Assert.That(Vector3.Distance(at,b.nyonya.transform.position),Is.GreaterThan(.05f),"Patrol must continue while the player remains hidden");
            yield return Press(Key.E);Assert.That(b.IsHidden,Is.False);Assert.That(d.player.MovementLocked,Is.False);
            Assert.That(rig.IsFirstPerson,Is.False,"Exiting a real wardrobe restores the shoulder selection");
            Assert.That(Vector3.Distance(d.player.transform.position,b.exitAnchor1.position),Is.LessThan(.2f));
        }
        private IEnumerator InterruptedRecording()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var c=CampaignState.Instance;
            foreach(var id in new[]{"EVENT_P0_BASEMENT_ENTERED","EVENT_P0_POWERBANK_TAKEN","EVENT_P0_HANDYCAM_PLAYED",AtmajaBasementHorror.HandycamId})c.AddInventoryItem(id);
            c.Data.battery=80;Assert.That(c.SaveCheckpointAtomic("PROLOG_BASEMENT_ENTRY"),Is.True);d.RestoreWorld();
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.BasementStealth),"Legacy saves marked video played before it finished; offer replay instead of a dead chase objective");
            SmartphoneController.Instance.batteryPercent=80;Item(AtmajaAction.Handycam).Interact();yield return null;
            Assert.That(d.chase.recording.IsPlaying&&d.Busy&&UiFocus.Cinematic,Is.True);Assert.That(c.HasInventoryItem("EVENT_P0_CHASE_STARTED"),Is.False);
            // Simulate closing the process during the recording, then use the saved campaign.
            yield return SceneManager.LoadSceneAsync(PrologSceneFlow.AtmajaScene);yield return new WaitForSeconds(2);
            d=Find<AtmajaDirector>();c=CampaignState.Instance;
            Assert.That(d.Busy||UiFocus.Cinematic||d.chase.recording.IsPlaying,Is.False);Assert.That(d.basement.handycamObj.activeSelf,Is.True);
            Assert.That(c.HasInventoryItem(AtmajaBasementHorror.HandycamId),Is.True);Item(AtmajaAction.Handycam).Interact();yield return null;
            Assert.That(d.chase.recording.IsPlaying,Is.True,"An already acquired handycam remains replayable after Continue");
        }
        private IEnumerator FinishedProlog()
        {
            yield return new WaitForSeconds(2);var d=SeedBasement();var c=CampaignState.Instance;
            foreach(var id in new[]{"EVENT_P0_CHASE_STARTED","EVENT_P0_ENDING_STARTED","EVENT_P0_FINAL_SAWER","PROLOG_COMPLETE"})c.AddInventoryItem(id);
            c.Data.wallet_bound=20450000;c.Data.spent=5000000;
            Assert.That(c.SaveCheckpointAtomic("PROLOG_COMPLETE"),Is.True);d.RestoreWorld();yield return null;
            Assert.That(d.chase.finale.IsCompletionVisible&&UiFocus.Cinematic,Is.True);
            Assert.That(GameObject.Find("TitleDrop").GetComponent<UnityEngine.UI.Text>().text,Is.EqualTo("LAYAR TERAKHIR"));
            Assert.That(GameObject.Find("PrologMainMenu").GetComponent<UnityEngine.UI.Button>().interactable,Is.True);
            Assert.That(c.Data.wallet_bound,Is.EqualTo(20450000));Assert.That(c.Data.location_seals.Count,Is.Zero);
            d.RestoreWorld();yield return null;Assert.That(c.Data.wallet_bound,Is.EqualTo(20450000));
        }
        [UnityTest] public IEnumerator EnclosedStairsHideBehindARealDoorAndKeepGuards()=>Run(StairEnclosure());
        private IEnumerator StairEnclosure()
        {
            yield return new WaitForSeconds(1);var d=Find<AtmajaDirector>();var c=CampaignState.Instance;
            d.player.enabled=false;
            Assert.That(d.stairDoorMotion&&d.stairDoorLeaf,Is.True);
            Assert.That(Object.FindObjectsByType<HingedDoor>(FindObjectsSortMode.None).Any(door=>door.name.Contains("Storage")),Is.False);
            Assert.That(GameObject.Find("StorageDoor"),Is.Null);
            foreach(float x in new[]{-5f,-3.55f,-1f,1f})
            {
                var from=d.P(x,4.8f,2.1f);var to=d.P(-1.2f,8.15f,1.35f);
                Assert.That(Physics.Linecast(from,to,out var hit,~0,QueryTriggerInteraction.Ignore),Is.True);
                Assert.That(hit.collider.name.StartsWith("StairFlight"),Is.False,"Front view must hit an opaque occluder before the steps");
                var ray=to-from;Assert.That(Physics.RaycastAll(from,ray.normalized,ray.magnitude,~0,QueryTriggerInteraction.Ignore).Any(h=>h.collider.name.StartsWith("StairEnclosure")||h.collider.name=="LockedCentralStairDoor"),Is.True,"The enclosure must physically close every front sightline");
            }
            c.AddInventoryItem("EVENT_P0_STAIRS_UNLOCKED");d.RestoreWorld();Physics.SyncTransforms();
            Assert.That(d.stairDoorMotion.IsOpen,Is.True);Assert.That(d.stairDoorLeaf.GetComponent<Collider>().enabled,Is.True,"Opened door remains physically hinged beside the jamb");
            d.SetStage(AtmajaStage.EvidenceHunt);Assert.That(Item(AtmajaAction.StairDoor).CanInteract(),Is.False);
            Item(AtmajaAction.StairDoor).Interact();Assert.That(d.Stage,Is.EqualTo(AtmajaStage.EvidenceHunt),"An already open stair door cannot rewind the upper-floor checkpoint");
            Assert.That(d.stairDoorLeaf.transform.position.y,Is.GreaterThan(1),"Door must not be buried below the house");
            foreach(var point in new[]{d.P(-3.55f,5.1f,.54f),d.P(-3.55f,8.2f,.54f),d.P(-1.2f,8.2f,1.4f)})yield return WalkCapsule(d.player,point);
            var cc=d.player.GetComponent<CharacterController>();
            for(int n=0;n<14;n++){cc.Move(-d.north*.1f+Vector3.down*.12f);yield return null;}
            Assert.That(d.LocalPosition(d.player.transform.position).y,Is.GreaterThan(7.65f),"Continuous stair guard blocks walking through the inner railing");
            Assert.That(GameObject.Find("StairFlightLower").GetComponent<MeshCollider>().sharedMesh,Is.Not.Null);
            Assert.That(GameObject.Find("StairFlightUpper").GetComponent<MeshCollider>().sharedMesh,Is.Not.Null);
        }
        [UnityTest] public IEnumerator ReadingNotesPreservesHeldPhoneCameraAndLocksMovement()=>Run(DocumentPhone());
        private IEnumerator DocumentPhone()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;var hud=HudController.Instance;
            var p=d.player;d.PlacePlayer(d.P(-3.55f,5.1f,.54f),Quaternion.LookRotation(d.north));
            CampaignState.Instance.AddInventoryItem("SMARTPHONE_RAKA");phone.hasSmartphoneInInventory=true;
            phone.isPhoneRaised=phone.isScreenOn=phone.isBroadcastConnected=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            var start=p.transform.position;hud.OpenDocumentModal("Catatan uji","HP tetap di tangan saat catatan dibaca.");yield return null;yield return null;
            Assert.That(phone.isPhoneRaised&&phone.isScreenOn&&phone.isBroadcastConnected,Is.True);
            Assert.That(d.phoneView.Panel.activeSelf,Is.True);Assert.That(d.phoneView.FeedCamera.enabled,Is.EqualTo(d.phoneView.HasGraphicsDevice));
            Assert.That(UiFocus.Phone,Is.False);Assert.That(d.phoneView.Panel.GetComponent<CanvasGroup>().interactable,Is.False);
            Assert.That(((RectTransform)hud.documentPanel.transform).anchorMax.x,Is.LessThan(((RectTransform)d.phoneView.Panel.transform).anchorMin.x),"Note and handset occupy separate readable columns");
            p.enabled=true;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.W));yield return new WaitForSeconds(.4f);
            Assert.That(Vector3.Distance(start,p.transform.position),Is.LessThan(.04f),"Reading blocks world movement while keeping the camera feed active");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());hud.CloseDocumentModal();yield return null;
            Assert.That(phone.isPhoneRaised&&phone.isScreenOn,Is.True);Assert.That(d.phoneView.Panel.GetComponent<CanvasGroup>().interactable,Is.True);
            phone.isPhoneRaised=false;phone.isScreenOn=false;phone.SyncVisualState();hud.OpenDocumentModal("Catatan kedua","Tidak memaksa menyalakan HP.");yield return null;
            Assert.That(phone.isPhoneRaised||phone.isScreenOn,Is.False);hud.CloseDocumentModal();
        }
        private IEnumerator UpperStairNavigation()
        {
            yield return new WaitForSeconds(1);var d=Find<AtmajaDirector>();var c=CampaignState.Instance;
            foreach(var id in new[]{"EVENT_P0_GATE_OPEN","EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN","MATABATIN_APK","EVENT_P0_STAIRS_UNLOCKED"})c.AddInventoryItem(id);
            d.RestoreWorld();var p=d.player;p.enabled=false;
            foreach(var point in new[]{d.P(-3.55f,5.1f,.54f),d.P(-3.55f,8.2f,.54f),d.P(1.05f,8.2f,2.2f),d.P(1.05f,6.925f,2.4f),d.P(-3.3f,6.925f,3.82f),d.P(-3.3f,5.75f,3.82f),d.P(-1.1f,5.75f,3.82f)})yield return WalkCapsule(p,point,float.PositiveInfinity,.025f);
            Assert.That(p.transform.position.y,Is.GreaterThan(3.75f));
            yield return WalkCapsule(p,d.P(-3,5.75f,3.82f),float.PositiveInfinity,.025f);
            var phone=SmartphoneController.Instance;phone.arModuleUnlocked=true;phone.isPhoneRaised=phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;
            d.phoneView.Apps.Open(PhoneApp.Camera);phone.SyncVisualState();
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));p.enabled=true;yield return new WaitForSeconds(.6f);p.enabled=false;
            Assert.That(p.IsCrouching,Is.True);yield return WalkCapsule(p,d.P(-3,4.6f,3.82f));
            Assert.That(d.LocalPosition(p.transform.position).y,Is.LessThan(5),"AR/crouch vent must have a real opening through the child-room wall");
        }
        private IEnumerator UpperPhonePhotos()
        {
            yield return new WaitForSeconds(1);
            var d=Find<AtmajaDirector>();var c=CampaignState.Instance;var phone=SmartphoneController.Instance;
            foreach(var id in new[]{"EVENT_P0_GATE_OPEN","EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN","MATABATIN_APK","EVENT_P0_STAIRS_UNLOCKED","EVENT_P0_UPPER_REACHED","EVENT_P0_CURTAIN_OPEN",AtmajaUpperDirector.DiaryId})c.AddInventoryItem(id);
            d.RestoreWorld();d.SetStage(AtmajaStage.EvidenceHunt);d.player.enabled=false;
            phone.arModuleUnlocked=true;phone.currentScanMode=ScanVisualMode.AR;phone.isPhoneRaised=phone.isScreenOn=true;phone.batteryPercent=100;
            d.phoneView.Apps.Open(PhoneApp.Camera);phone.SyncVisualState();yield return null;
            var lens=d.phoneView.FeedCamera;
            Transform[] targets={d.upper.cupAim,d.upper.scratchAim,d.upper.ropeAim};
            for(int i=0;i<3;i++)
            {
                var target=targets[i];Vector3 side=i==0?d.north:-d.north;
                lens.transform.position=target.position+side*1.2f+Vector3.up*(i==0?.7f:i==2?-.5f:.1f);
                lens.transform.LookAt(target);Physics.SyncTransforms();d.upper.TryPhotoTick();
                var delta=target.position-lens.transform.position;
                var hits=Physics.RaycastAll(lens.transform.position,delta.normalized,delta.magnitude,~0,QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance);
                Assert.That(c.HasInventoryItem("EVENT_P0_CHAIN"+(i+1)+"_BROKEN"),Is.True,"Actual phone lens must photograph evidence "+(i+1)+"; stage="+d.Stage+"; AR="+phone.currentScanMode+"; hits="+string.Join(", ",hits.Select(h=>h.collider.name+" at "+h.point)));
                yield return null;
            }
            Assert.That(d.upper.ChainsBroken,Is.EqualTo(3));
        }
        private IEnumerator KosNavigation()
        {
            yield return SceneManager.LoadSceneAsync(PrologSceneFlow.CorridorScene);yield return new WaitForSeconds(1);
            var corridor=Find<KosCorridorDirector>();var p=corridor.player;p.enabled=false;
            Assert.That(p.transform.position.y,Is.GreaterThan(3));Assert.That(corridor.CanDepart,Is.False);
            foreach(var point in new[]{new Vector3(-.6f,3.3f,12.7f),new Vector3(-.6f,1.65f,16.4f),new Vector3(1.75f,1.65f,16.4f),new Vector3(1.75f,0,12.6f),new Vector3(0,0,12.6f),new Vector3(0,0,11.5f),new Vector3(0,0,7.5f)})yield return WalkCapsule(p,point);
            Assert.That(p.transform.position.y,Is.LessThan(.15f));
            GameObject.Find("SharedDiningDoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,new Vector3(-2.4f,0,7.5f));yield return WalkCapsule(p,new Vector3(0,0,7.5f));yield return WalkCapsule(p,new Vector3(0,0,4.25f));
            GameObject.Find("DAPUR BERSAMADoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,new Vector3(-2.7f,0,4.25f));yield return WalkCapsule(p,new Vector3(0,0,4.25f));
            GameObject.Find("KAMAR MANDIDoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,new Vector3(2.1f,0,4.25f));
            foreach(var cubicle in new[]{("WANITA",5.2f),("PRIA",3.3f)})
            {
                yield return WalkCapsule(p,new Vector3(2.1f,0,cubicle.Item2));
                GameObject.Find("Bathroom"+cubicle.Item1+"Door").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
                yield return WalkCapsule(p,new Vector3(3.45f,0,cubicle.Item2));yield return WalkCapsule(p,new Vector3(2.1f,0,cubicle.Item2));
                yield return WalkCapsule(p,new Vector3(2.1f,0,4.25f));
            }
            yield return WalkCapsule(p,new Vector3(0,0,4.25f));yield return WalkCapsule(p,new Vector3(0,0,.8f));
            Assert.That(corridor.entranceDoor.IsOpen,Is.False);corridor.entranceDoor.Interact();yield return new WaitForSeconds(1.2f);
            Assert.That(corridor.entranceDoor.IsOpen,Is.True);yield return WalkCapsule(p,new Vector3(0,0,-4.7f));
            Assert.That(corridor.CanDepart,Is.True);Assert.That(CampaignState.Instance.Data.checkpoint_id,Is.EqualTo("PROLOG_KOS_YARD"));
        }
        private IEnumerator DiningNavigation()
        {
            yield return new WaitForSeconds(1);var d=Find<AtmajaDirector>();var p=d.player;
            foreach(var id in new[]{AtmajaDirector.SurveyId,"EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN"})CampaignState.Instance.AddInventoryItem(id);
            d.RestoreWorld();p.enabled=false;
            foreach(var point in new[]{d.P(5.15f,9.8f,.37f),d.P(4.1f,9.8f,.37f),d.P(4.1f,8.4f,.54f),d.P(5.35f,8,.54f),d.P(5.35f,6.2f,.54f)})yield return WalkCapsule(p,point,.7f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));p.enabled=true;yield return new WaitForSeconds(.6f);p.enabled=false;
            Assert.That(p.IsCrouching,Is.True);Assert.That(p.GetComponent<CharacterController>().height,Is.LessThan(.81f));
            Assert.That(p.GetComponent<CharacterController>().stepOffset,Is.LessThanOrEqualTo(.06f));
            yield return WalkCapsule(p,d.P(4.6f,6.2f,.54f),.7f);
            var eye=p.GetComponent<InteractionInteractor>().playerCamera;eye.transform.LookAt(d.damarPhone.transform.position);yield return null;yield return null;
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(Item(AtmajaAction.DamarPhone)),"Backpack phone must be reachable from the side without a tabletop pose");
            yield return Press(Key.E);yield return new WaitUntil(()=>!d.Busy);Assert.That(CampaignState.Instance.HasInventoryItem("MATABATIN_APK"),Is.True);
            yield return WalkCapsule(p,d.P(5.35f,6.2f,.54f),.7f);InputSystem.QueueStateEvent(keyboard,new KeyboardState());p.enabled=true;yield return new WaitForSeconds(.6f);p.enabled=false;
            foreach(var point in new[]{d.P(5.35f,1.3f,.54f),d.P(2.1f,1.3f,.54f),d.P(2.1f,4.1f,.54f),d.P(.8f,4.1f,.54f),d.P(-1.4f,4.1f,.54f),d.P(-1.4f,3.3f,.54f)})yield return WalkCapsule(p,point,.7f);
            GameObject.Find("FrontSittingDoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,d.P(-1.4f,1.5f,.54f),.7f);yield return WalkCapsule(p,d.P(-4.4f,1.5f,.54f),.7f);
            eye.transform.LookAt(d.clockHands.transform.position);yield return null;yield return null;
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(Item(AtmajaAction.ClockHands)));yield return Press(Key.E);
            Assert.That(CampaignState.Instance.HasInventoryItem("ITEM_P0_CLOCK_HANDS"),Is.True);
            foreach(var point in new[]{d.P(-1.4f,1.5f,.54f),d.P(-1.4f,4.1f,.54f),d.P(-3.7f,4.6f,.54f),d.P(-4.7f,5.8f,.54f)})yield return WalkCapsule(p,point,.7f);
        }
        private static IEnumerator WalkCapsule(FirstPersonController p,Vector3 target,float maxHeight=float.PositiveInfinity,float tolerance=.1f)
        {
            p.enabled=false;var cc=p.GetComponent<CharacterController>();int stalled=0;
            for(int frame=0;frame<500;frame++)
            {
                var delta=target-p.transform.position;delta.y=0;if(delta.magnitude<tolerance)break;
                var before=p.transform.position;cc.Move(Vector3.ClampMagnitude(delta,.16f)+Vector3.down*.18f);
                Assert.That(p.transform.position.y,Is.LessThanOrEqualTo(maxHeight),"Feet must stay below tabletop height");
                if(Vector3.Distance(before,p.transform.position)<.003f)stalled++;else stalled=0;if(stalled>15)break;yield return null;
            }
            var remaining=target-p.transform.position;remaining.y=0;
            Assert.That(remaining.magnitude,Is.LessThan(.16f),"Continuous CC route blocked at "+p.transform.position+" on way to "+target);
        }
        private IEnumerator PhoneApps()
        {
            yield return new WaitForSeconds(2);var view=Find<AtmajaPhoneView>();var phone=SmartphoneController.Instance;
            phone.isPhoneRaised=true;phone.isScreenOn=true;phone.SyncVisualState();view.Apps.Open(PhoneApp.Home);yield return null;
            Assert.That(view.Panel.GetComponentsInChildren<UnityEngine.UI.Button>().Length,Is.GreaterThanOrEqualTo(7));
            CampaignState.Instance.AddInventoryItem(AtmajaDirector.SurveyId);view.Apps.Open(PhoneApp.Journal);yield return null;
            var entries=Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).First(b=>b.name.StartsWith("JournalEntry_")&&b.GetComponentInChildren<UnityEngine.UI.Text>().text.Contains("survei Damar"));
            Assert.That(entries.GetComponentInParent<UnityEngine.UI.ScrollRect>().vertical,Is.True);
            entries.onClick.Invoke();yield return null;
            Assert.That(GameObject.Find("EntryTitle").GetComponent<UnityEngine.UI.Text>().text,Does.Contain("survei Damar"));
            view.Apps.Open(PhoneApp.Inventory);yield return null;Assert.That(GameObject.Find("InventoryEntries").GetComponent<UnityEngine.UI.Text>().text,Does.Contain("Smartphone Raka"));
            Assert.That(UiFocus.Phone,Is.True);yield return Press(Key.Tab);yield return null;
            Assert.That(phone.isPhoneRaised,Is.False);Assert.That(UiFocus.Phone,Is.False);
        }
        private IEnumerator Exterior()
        {
            yield return new WaitForSeconds(2);
            var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            Assert.That(d.StartLive(),Is.False,"Cannot broadcast before attempting the locked front door");
            Item(AtmajaAction.FrontDoor).Interact();yield return new WaitForSeconds(2);
            Assert.That(campaign.HasInventoryItem("EVENT_P0_FRONT_DOOR_TRIED"),Is.True);Assert.That(d.HasSurvey,Is.False);
            yield return Press(Key.Tab);Assert.That(phone.isPhoneRaised,Is.True);
            d.phoneView.Apps.Open(PhoneApp.Camera);
            d.phoneView.LiveButton.onClick.Invoke();yield return null;
            Assert.That(phone.isBroadcastConnected,Is.True);Assert.That(d.Viewers,Is.EqualTo(14));
            d.PlacePlayer(d.P(8.2f,1,.06f),Quaternion.LookRotation(d.north));d.player.enabled=false;yield return null;yield return null;
            Assert.That(campaign.Data.wallet_bound,Is.EqualTo(5500000));
            d.PlacePlayer(d.P(8.2f,4.4f,.06f),Quaternion.LookRotation(d.north));var cc=d.player.GetComponent<CharacterController>();
            cc.Move(d.north*3);Assert.That(d.LocalPosition(d.player.transform.position).y,Is.LessThan(5.5f));Assert.That(d.CanVault(),Is.False);
            d.PlacePlayer(d.P(8.2f,4.4f,.06f),Quaternion.LookRotation(d.north));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));d.player.enabled=true;yield return new WaitForSeconds(.5f);
            Assert.That(d.player.IsCrouching,Is.True);d.player.enabled=false;
            cc.height=d.player.crouchHeight;cc.center=Vector3.up*cc.height*.5f;
            cc.Move(d.north*1.1f);yield return null;cc.Move(d.north*1.3f);yield return null;
            Assert.That(d.TreeCleared,Is.True);Assert.That(campaign.Data.wallet_bound,Is.EqualTo(5500000));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());
            d.PlacePlayer(d.vaultOutside.position,d.vaultOutside.rotation);
            var camera=d.player.GetComponent<InteractionInteractor>().playerCamera;
            camera.transform.LookAt(Vector3.Lerp(d.vaultOutside.position,d.vaultInside.position,.65f)+Vector3.up*1.3f);
            GameInputConfig.Instance.vaultKey=KeyCode.V;
            yield return Press(Key.Space);Assert.That(d.Stage,Is.EqualTo(AtmajaStage.SidePassage));
            yield return Press(Key.V);yield return new WaitForSeconds(1.5f);d.player.enabled=false;
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.Kitchen));Assert.That(d.kitchenShutter.activeSelf,Is.True);
            Assert.That(Vector3.Distance(d.player.transform.position,d.vaultInside.position),Is.LessThan(.3f));
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo("PROLOG_ATMAJA_KITCHEN"));
            d.PlacePlayer(d.P(5,10.9f,.37f),Quaternion.LookRotation(d.north));
            camera=d.phoneView.FeedCamera;camera.transform.position=d.P(5,10.9f,1.7f);camera.transform.LookAt(d.P(2.7f,11.2f,.75f));Physics.SyncTransforms();
            Assert.That(d.TryPhotographWell(),Is.True,"Optional bonus needs the actual well sight line");
            Assert.That(campaign.Data.wallet_legitimate,Is.EqualTo(150000));
            Assert.That(d.TryPhotographWell(),Is.False,"No repeated well farming");Assert.That(campaign.Data.wallet_legitimate,Is.EqualTo(150000));
        }
        private IEnumerator Interior()
        {
            yield return new WaitForSeconds(2);
            var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            foreach(var id in new[]{AtmajaDirector.SurveyId,"EVENT_P0_LIVE_STARTED","EVENT_P0_SIDE_CHAT","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN"})campaign.AddInventoryItem(id);
            campaign.Data.wallet_bound=5500000;d.RestoreWorld();phone.isBroadcastConnected=true;
            // Exhaust both battery and one-shot powerbank: the real emergency port must still recover play.
            campaign.Data.inventory.Remove("POWERBANK_KECIL_KOS");phone.ConsumeBattery(100);Item(AtmajaAction.Charge).Interact();yield return new WaitForSeconds(3.5f);
            Assert.That(phone.batteryPercent,Is.GreaterThan(34));Assert.That(phone.isScreenOn,Is.True);
            d.PlacePlayer(d.P(3.6f,7.4f,.54f),Quaternion.LookRotation(d.north));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));yield return new WaitForSeconds(.5f);
            Item(AtmajaAction.DamarPhone).Interact();InputSystem.QueueStateEvent(keyboard,new KeyboardState());
            Assert.That(phone.arModuleUnlocked,Is.False,"The lens must stay locked until installation completes");
            yield return new WaitUntil(()=>!d.Busy);
            Assert.That(phone.arModuleUnlocked,Is.True);Assert.That(campaign.Data.upgrades.Count(x=>x=="MATABATIN_AR"),Is.EqualTo(1));
            Assert.That(Object.FindObjectsByType<SmartphoneController>(FindObjectsSortMode.None).Length,Is.EqualTo(1));
            phone.isPhoneRaised=true;phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;yield return null;yield return null;
            Assert.That(d.arFigure.activeSelf,Is.True);Assert.That(d.phoneView.FeedCamera.enabled,Is.EqualTo(d.phoneView.HasGraphicsDevice));
            Assert.That(d.phoneView.playerCamera.cullingMask&(1<<AtmajaPhoneView.ARLayer),Is.Zero);
            phone.isScreenOn=false;yield return null;yield return null;
            Assert.That(d.phoneView.FeedCamera.enabled,Is.False);Assert.That(d.arFigure.activeSelf,Is.False);Assert.That(phone.isBroadcastConnected,Is.True);
            Assert.That(d.phoneView.Panel.GetComponentInChildren<UnityEngine.UI.RawImage>(true).enabled,Is.False);
            phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;d.phoneView.Apps.Open(PhoneApp.Camera);
            d.PlacePlayer(d.P(-3.7f,4.6f,.54f),Quaternion.LookRotation(-d.east));d.player.enabled=true;
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return new WaitForSeconds(.6f);d.player.enabled=false;
            var direction=d.arClue.transform.position-d.player.cameraPivot.position;d.player.cameraPivot.rotation=Quaternion.LookRotation(direction);
            yield return null;yield return null;
            Assert.That(campaign.HasInventoryItem("EVENT_P0_AR_CLUE_READ"),Is.True,"Clue must actually be aimed at before solving");
            Item(AtmajaAction.ClockHands).Interact();Item(AtmajaAction.Clock).Interact();
            var puzzle=Find<ClockPuzzleView>();puzzle.Confirm();Assert.That(campaign.HasInventoryItem("KUNCI_TANGGA_TENGAH"),Is.False);
            puzzle.AdjustHour(3);puzzle.AdjustMinute(45);puzzle.Confirm();Assert.That(HudController.Instance.documentBodyText.text,Does.Contain("TRAGEDI KELUARGA ATMAJA"));
            // Closing the process mid-document retains the complete note and its read receipt.
            yield return SceneManager.LoadSceneAsync(AtmajaSceneBuilder.ScenePath);yield return new WaitForSeconds(2);
            d=Find<AtmajaDirector>();campaign=CampaignState.Instance;phone=SmartphoneController.Instance;
            Assert.That(campaign.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"),Is.True);
            Assert.That(campaign.Data.documents.Any(n=>n.title.Contains("DOKUMEN #3")&&n.body.Contains("Nyonya Sri Atmaja (38)")),Is.True);
            phone.isPhoneRaised=true;phone.isScreenOn=true;d.phoneView.Apps.Open(PhoneApp.Journal);yield return null;
            var newspaper=Object.FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).Single(b=>b.name.StartsWith("JournalEntry_")&&b.GetComponentInChildren<UnityEngine.UI.Text>().text.Contains("KLIPING KORAN"));
            newspaper.onClick.Invoke();yield return null;Assert.That(GameObject.Find("EntryBody").GetComponentInChildren<UnityEngine.UI.Text>().text,Does.Contain("Nyonya Sri Atmaja (38)"));
            d.phoneView.Apps.Open(PhoneApp.Camera);phone.isPhoneRaised=false;phone.SyncVisualState();yield return null;
            Item(AtmajaAction.StairDoor).Interact();phone.batteryPercent=12;phone.SyncVisualState();campaign.Data.battery=12;
            Assert.That(campaign.SaveCheckpointAtomic("PROLOG_ATMAJA_STAIRS_UNLOCKED"),Is.True);
            yield return SceneManager.LoadSceneAsync(PrologKosSceneBuilder.ScenePath);
            float deadline=Time.realtimeSinceStartup+15;
            while(SceneManager.GetActiveScene().name!=PrologSceneFlow.AtmajaScene&&Time.realtimeSinceStartup<deadline)yield return null;
            Assert.That(SceneManager.GetActiveScene().name,Is.EqualTo(PrologSceneFlow.AtmajaScene));yield return new WaitForSeconds(2);
            d=Find<AtmajaDirector>();campaign=CampaignState.Instance;
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.StairCheckpoint));Assert.That(campaign.Data.wallet_bound,Is.EqualTo(5500000));
            Assert.That(campaign.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"),Is.True);Assert.That(SmartphoneController.Instance.arModuleUnlocked,Is.True);
            Assert.That(SmartphoneController.Instance.batteryPercent,Is.InRange(11,12));
            Assert.That(Vector3.Distance(d.player.transform.position,d.spawnStairs.position),Is.LessThan(.3f));
        }
        private IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private static T Find<T>()where T:Object=>Object.FindFirstObjectByType<T>();
        private static AtmajaItem Item(AtmajaAction action)=>Object.FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).First(x=>x.action==action);
        private static UnityEngine.UI.Button Button(string name)=>FoundationAuditTests.Button(HudController.Instance,name);
        private static void CloseDocument()=>HudController.Instance.documentCloseButton.onClick.Invoke();
        // Execute waits on the player loop, while forwarding NUnit assertion failures to the Editor runner.
        private static IEnumerator Run(IEnumerator scenario)
        {
            var go=new GameObject("AtmajaTestHost");Object.DontDestroyOnLoad(go);var host=go.AddComponent<FoundationAuditCoroutineHost>();bool done=false;Exception failure=null;
            host.StartCoroutine(Drive(scenario,e=>{failure=e;done=true;}));float deadline=Time.realtimeSinceStartup+60;
            while(!done&&Time.realtimeSinceStartup<deadline)yield return null;
            Object.Destroy(go);Assert.That(done,Is.True,"Atmaja runtime scenario timeout");if(failure!=null)throw failure;
        }
        private static IEnumerator Drive(IEnumerator scenario,Action<Exception> complete)
        {
            var stack=new Stack<IEnumerator>();stack.Push(scenario);
            while(stack.Count>0)
            {
                object next=null;Exception failure=null;bool more=false;
                try{more=stack.Peek().MoveNext();if(more)next=stack.Peek().Current;}catch(Exception e){failure=e;}
                if(failure!=null){complete(failure);yield break;}if(!more){stack.Pop();continue;}
                if(next is IEnumerator nested)stack.Push(nested);else yield return next;
            }
            complete(null);
        }
    }
}
#endif
