#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class DesignerRevisionTests
    {
        private string directory,previous;
        [SetUp] public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            previous=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            directory=Path.Combine(Path.GetTempPath(),"LayarDesignerTest-"+Guid.NewGuid().ToString("N"));
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",directory);
        }
        [TearDown] public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",previous);
            if(Directory.Exists(directory))Directory.Delete(directory,true);
            else if(File.Exists(directory))File.Delete(directory);
        }
        [Test] public void FamilyPaymentsRequireCallsPersistAndNeverDoubleCharge()
        {
            var c=new GameObject("CampaignTest").AddComponent<CampaignState>();c.StartNewGame();
            c.AddInventoryItem("SMARTPHONE_RAKA");c.AddInventoryItem("EVENT_P0_CONTRACT_RECEIVED");c.Data.wallet_bound=5000000;
            Assert.That(c.TryPayFamilyBill(true),Is.False);Assert.That(c.TryPayFamilyBill(false),Is.False);
            c.AddInventoryItem("EVENT_CALLED_IBU");Assert.That(c.TryPayFamilyBill(true),Is.True);
            Assert.That(c.Data.wallet_bound,Is.EqualTo(3500000));Assert.That(c.Data.spent,Is.EqualTo(1500000));
            Assert.That(c.TryPayFamilyBill(true),Is.False);
            c.AddInventoryItem("EVENT_CALLED_RATNA");Assert.That(c.TryPayFamilyBill(false),Is.True);
            Assert.That(c.Data.wallet_bound,Is.Zero);Assert.That(c.Data.spent,Is.EqualTo(5000000));
            Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.checkpoint_id,Is.EqualTo("PROLOG_KOS_PREPARATION"));
            Assert.That(c.HasInventoryItem("EVENT_PAID_HOSPITAL")&&c.HasInventoryItem("EVENT_PAID_RENT"),Is.True);
            Assert.That(c.TryPayFamilyBill(true)||c.TryPayFamilyBill(false),Is.False);
        }
        [Test] public void FailedPaymentSaveRollsBackEveryWalletAndReceiptChange()
        {
            File.WriteAllText(directory,"A file deliberately occupies the save directory path");
            var c=new GameObject("CampaignTest").AddComponent<CampaignState>();c.StartNewGame();
            c.Data.wallet_bound=5000000;c.AddInventoryItem("EVENT_CALLED_IBU");
            LogAssert.Expect(LogType.Error,new System.Text.RegularExpressions.Regex("Gagal menyimpan checkpoint"));
            Assert.That(c.TryPayFamilyBill(true),Is.False);Assert.That(c.Data.wallet_bound,Is.EqualTo(5000000));
            Assert.That(c.Data.spent,Is.Zero);Assert.That(c.HasInventoryItem("EVENT_PAID_HOSPITAL"),Is.False);
        }
        [Test] public void InsufficientFundsDoNotCreateFakePaymentReceipt()
        {
            var c=new GameObject("CampaignTest").AddComponent<CampaignState>();c.StartNewGame();
            c.Data.wallet_bound=1499999;c.AddInventoryItem("EVENT_CALLED_IBU");
            Assert.That(c.TryPayFamilyBill(true),Is.False);Assert.That(c.HasInventoryItem("EVENT_PAID_HOSPITAL"),Is.False);
            Assert.That(c.Data.wallet_bound,Is.EqualTo(1499999));
        }
        [Test] public void PhoneFocusDoesNotBlockDoorsButMenusStillDo()
        {
            bool beforePhone=UiFocus.Phone,beforeMenu=UiFocus.Menu;
            try
            {
                var door=new GameObject("DoorGuardTest").AddComponent<HingedDoor>();door.hinge=door.transform;
                UiFocus.Phone=true;UiFocus.Menu=false;Assert.That(door.CanInteract(),Is.True);
                UiFocus.Menu=true;Assert.That(door.CanInteract(),Is.False);
            }
            finally{UiFocus.Phone=beforePhone;UiFocus.Menu=beforeMenu;}
        }
        [Test] public void KosHasSeparateFacilitiesColouredWindowsAndPairedGate()
        {
            CheckKosFacilities();
        }
        [Test] public void FullscreenPreferenceNeverOverridesTestsOrExplicitLaunchDimensions()
        {
            Assert.That(DisplayPreferences.ShouldRestore(false,new[]{"Game.exe"},false),Is.True);
            Assert.That(DisplayPreferences.ShouldRestore(true,new[]{"Game.exe"},false),Is.False);
            Assert.That(DisplayPreferences.ShouldRestore(false,new[]{"Game.exe"},true),Is.False);
            foreach(string flag in new[]{"-screen-fullscreen","-screen-width","-screen-height","-reviewAudit","-nographics","-batchmode"})
                Assert.That(DisplayPreferences.ShouldRestore(false,new[]{"Game.exe",flag},false),Is.False,flag);
        }
        private void CheckKosFacilities()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            foreach(string name in new[]{"SharedDiningDoor","BathroomWANITADoor","BathroomPRIADoor","KosCommunalDiningTable","Stove","KitchenSink"})Assert.That(GameObject.Find(name),Is.Not.Null,name);
            Assert.That(Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Count(t=>t.name=="KosFrontWindowGlass"),Is.EqualTo(8));
            Assert.That(RenderSettings.skybox.shader.name,Is.EqualTo("LayarTerakhir/NightSky"));
            var d=Object.FindFirstObjectByType<KosCorridorDirector>();var gate=d.gateHinge.GetComponent<DoubleLeafGate>();
            Assert.That(gate.leftLeaf.localPosition.x,Is.LessThan(0));Assert.That(gate.rightLeaf.localPosition.x,Is.GreaterThan(0));
            gate.SetImmediate(true);Assert.That(Quaternion.Angle(gate.leftLeaf.localRotation,gate.rightLeaf.localRotation),Is.GreaterThan(100));
        }
        [Test] public void AtmajaNoteIsInsideDollReturnsAndHouseNameplateIsGone()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);var d=Object.FindFirstObjectByType<AtmajaDirector>();
            var note=Object.FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).Single(i=>i.action==AtmajaAction.Survey);
            Assert.That(d.LocalPosition(note.transform.position).y,Is.GreaterThan(0));
            Assert.That(GameObject.Find("DamarParkedScooter").GetComponent<AtmajaItem>(),Is.Null);
            var doll=GameObject.Find("SeatedDollOnEndChair");Assert.That(doll,Is.Not.Null);
            Assert.That(doll.GetComponentsInChildren<Renderer>().Any(r=>r.sharedMaterial.name=="DesignerDollPorcelain"),Is.True);
            Assert.That(doll.GetComponentsInChildren<Transform>().All(t=>t.gameObject.layer!=AtmajaPhoneView.ARLayer),Is.True);
            Assert.That(GameObject.Find("AtmajaNameplate"),Is.Null);
            Assert.That(d.gateHinge.GetComponent<DoubleLeafGate>(),Is.Not.Null);
            Assert.That(Object.FindFirstObjectByType<GameInputConfig>().vaultKey,Is.EqualTo(KeyCode.Space));
        }
        [Test] public void BloodClueHasUnobstructedHeadHeightSightLineFromLounge()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);var d=Object.FindFirstObjectByType<AtmajaDirector>();Physics.SyncTransforms();
            Vector3 eye=d.P(-3.96f,4.6f,2.14f),delta=d.arClue.transform.position-eye;
            var hits=Physics.RaycastAll(eye,delta.normalized,delta.magnitude-.03f,~0,QueryTriggerInteraction.Ignore)
                .Where(h=>!h.transform.IsChildOf(d.player.transform)).OrderBy(h=>h.distance).ToArray();
            Assert.That(hits.Length,Is.Zero,"Clue occluded by "+string.Join(", ",hits.Select(h=>h.collider.name+" at "+h.point)));
        }
    }
}
#endif
