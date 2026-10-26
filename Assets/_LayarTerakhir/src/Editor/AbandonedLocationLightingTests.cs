#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Phase5;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class AbandonedLocationLightingTests
    {
        [TearDown] public void EmptyScene() => EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);

        [TestCase("Prolog_RumahAtmaja")]
        [TestCase("Babak1_Rusunawa")]
        [TestCase("Babak2_RSUD")]
        [TestCase("Babak3_PabrikGula")]
        [TestCase("Babak4_KarangSetra")]
        [TestCase("Babak5_PasarSetan")]
        public void ActualAbandonedSceneHasAnOutageWithoutDisablingRitualPower(string name)
        {
            EditorSceneManager.OpenScene("Assets/_LayarTerakhir/assets/Scenes/"+name+".unity");
            var player=Object.FindFirstObjectByType<FirstPersonController>();Assert.That(player,Is.Not.Null);
            var policy=player.GetComponent<AbandonedLocationLighting>()??player.gameObject.AddComponent<AbandonedLocationLighting>();
            policy.Configure();
            Assert.That(policy.UnpoweredLightCount,Is.GreaterThan(0));
            Assert.That(RenderSettings.ambientLight.maxColorComponent,Is.LessThan(.03f));
            Light[] ritual=new Light[0];
            var p1=Object.FindFirstObjectByType<Phase1Director>();if(p1)ritual=p1.world.halogenLights;
            var p2=Object.FindFirstObjectByType<Phase2Director>();if(p2)ritual=p2.world.ritualLights;
            var p3=Object.FindFirstObjectByType<Phase3Director>();if(p3)ritual=p3.world.ritualLights;
            var p4=Object.FindFirstObjectByType<Phase4Director>();if(p4)ritual=p4.world.torches.Select(t=>t.practical).ToArray();
            var p5=Object.FindFirstObjectByType<Phase5Director>();if(p5)ritual=new[]{p5.world.generatorLight,p5.world.droppedLight};
            foreach(var light in ritual)light.enabled=true;
            if(p2)foreach(var light in p2.world.corridorLights)light.enabled=true; // existing Lastri flicker / restore path
            policy.EnforceOutage();
            foreach(var light in ritual)Assert.That(light.enabled,Is.True,"Gameplay power must still be controlled by its puzzle: "+light.name);
            var phone=Object.FindFirstObjectByType<SmartphoneController>();Assert.That(phone,Is.Not.Null);
            Assert.That(phone.phoneFlashlight,Is.Not.Null);
            phone.phoneFlashlight.enabled=true;policy.EnforceOutage();
            Assert.That(phone.phoneFlashlight.enabled,Is.True,"Outage must never disable the player's actual phone beam");
            if(p2)foreach(var light in p2.world.corridorLights)Assert.That(light.enabled,Is.False,"Unpowered corridor fixture relit: "+light.name);
        }

        [Test] public void OccupiedKosAndItsNeighborhoodAreNotDeclaredAbandoned()
        {
            Assert.That(AbandonedLocationLighting.IsAbandoned("Prolog_KamarKos"),Is.False);
            Assert.That(AbandonedLocationLighting.IsAbandoned("Prolog_KoridorKos"),Is.False);
        }

        [Test] public void FinalDawnReleasesOnlyTheAuthoredEpilogueLights()
        {
            EditorSceneManager.OpenScene("Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity");
            var player=Object.FindFirstObjectByType<FirstPersonController>();
            var p5=Object.FindFirstObjectByType<Phase5Director>();
            var dawnLights=p5.world.epilogueRoot.GetComponentsInChildren<Light>(true).Where(l=>l.enabled).ToArray();
            Assert.That(dawnLights.Length,Is.GreaterThan(0));
            var policy=player.GetComponent<AbandonedLocationLighting>()??player.gameObject.AddComponent<AbandonedLocationLighting>();
            policy.Configure();
            // The director reapplies chapter lighting after an asynchronous
            // save/phone setup; that must not silently cancel the outage.
            Object.FindFirstObjectByType<Phase5Lighting>().Apply();policy.EnforceOutage();
            Assert.That(RenderSettings.ambientLight.maxColorComponent,Is.LessThan(.03f));
            Object.FindFirstObjectByType<Phase5Lighting>().Apply(true);policy.EnforceOutage();
            Assert.That(RenderSettings.ambientLight.maxColorComponent,Is.GreaterThan(.3f));
            Assert.That(policy.NightActive,Is.False);
            foreach(var light in dawnLights)Assert.That(light.enabled,Is.True);
            foreach(var light in p5.world.stageLights)Assert.That(light.enabled,Is.False);
        }
    }
}
#endif
