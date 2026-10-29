#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Interlude;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class PrologEnvironmentContinuityTests
    {
        [TearDown] public void Cleanup() => EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

        [Test] public void NightPrologLoadsTheSamePermanentStreetAsMorning()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var alley = GameObject.Find("KampungMelati").transform.Find("InterludeAlley");
            Assert.That(alley.gameObject.activeInHierarchy, Is.True, "Bug 47: houses and shops disappeared at night");
            var corridor = Object.FindFirstObjectByType<KosCorridorDirector>();
            InterludeExterior.EnsureNeighborhood(corridor);
            int[] visible = VisibleEnvironment(corridor);
            int props = corridor.buildingRoot.Find("KosYardLife").childCount;
            InterludeExterior.EnsureNeighborhood(corridor);
            InterludeExterior.ApplyDay(Camera.main);
            CollectionAssert.AreEqual(visible, VisibleEnvironment(corridor));
            InterludeExterior.ApplyDusk(Camera.main);
            CollectionAssert.AreEqual(visible, VisibleEnvironment(corridor));
            Assert.That(corridor.buildingRoot.Find("KosYardLife").childCount, Is.EqualTo(props), "Shared yard setup must be idempotent");
            Assert.That(props, Is.GreaterThan(10));
        }

        [Test] public void NextMorningReplacesTheAuthoredNightSkyAndResetsAfterDusk()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var night = RenderSettings.skybox;
            Assert.That(night, Is.Not.Null, "Regression starts from the actual night sky");
            var camera = Camera.main;
            InterludeExterior.ApplyDay(camera);
            AssertSky(camera, false);
            Assert.That(RenderSettings.skybox, Is.Not.SameAs(night));
            InterludeExterior.ApplyDusk(camera);
            AssertSky(camera, true);
            InterludeExterior.ApplyDay(camera);
            AssertSky(camera, false);
            Assert.That(RenderSettings.sun.enabled, Is.True);
            Assert.That(RenderSettings.sun.intensity, Is.EqualTo(1.15f));
            // Loading the prolog again restores its authored night profile;
            // the shared night material must never be edited by a day pass.
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            Assert.That(RenderSettings.skybox, Is.SameAs(night));
        }

        [Test] public void RebuiltKosSceneRecoversItsPermanentSceneryExactlyOnce()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var kampung = GameObject.Find("KampungMelati").transform;
            var original = kampung.Find("InterludeAlley");
            int renderers = original.GetComponentsInChildren<Renderer>().Length;
            Object.DestroyImmediate(original.gameObject);
            var corridor = Object.FindFirstObjectByType<KosCorridorDirector>();
            InterludeExterior.EnsureNeighborhood(corridor);
            InterludeExterior.EnsureNeighborhood(corridor);
            Assert.That(kampung.Cast<Transform>().Count(t => t.name == "InterludeAlley"), Is.EqualTo(1));
            Assert.That(kampung.Find("InterludeAlley").GetComponentsInChildren<Renderer>().Length, Is.EqualTo(renderers));
        }

        [TestCase(false)] [TestCase(true)]
        public void MorningRoomAndYardShareTheCampaignSky(bool dusk)
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var camera = Camera.main;
            InterludeExterior.ApplyRoom(camera, dusk);
            AssertSky(camera, dusk);
            Assert.That(RenderSettings.sun, Is.Not.Null);
            Assert.That(RenderSettings.sun.enabled, Is.True);
            InterludeExterior.ApplyRoom(camera, dusk);
            Assert.That(Object.FindObjectsByType<Light>(FindObjectsSortMode.None).Count(l => l.type == LightType.Directional), Is.EqualTo(1));
        }

        [Test] public void NightStreetSceneryLeavesTheDepartureRoadClear()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            InterludeExterior.EnsureNeighborhood(Object.FindFirstObjectByType<KosCorridorDirector>());
            Object.FindFirstObjectByType<FirstPersonController>().GetComponent<CharacterController>().enabled = false;
            Physics.SyncTransforms();
            var start = new Vector3(0, .28f, -14.5f);
            var hits = Physics.CapsuleCastAll(start + Vector3.up * .32f, start + Vector3.up * 1.43f,
                .32f, Vector3.back, 79.5f, ~0, QueryTriggerInteraction.Ignore);
            Assert.That(hits, Is.Empty, "Permanent scenery must leave the night departure road open: " + string.Join(",", hits.Select(h => h.collider.name)));
        }

        private static int[] VisibleEnvironment(KosCorridorDirector corridor)
        {
            return GameObject.Find("KampungMelati").GetComponentsInChildren<Renderer>()
                .Concat(corridor.buildingRoot.GetComponentsInChildren<Renderer>())
                .Select(r => r.GetInstanceID()).OrderBy(id => id).ToArray();
        }

        private static void AssertSky(Camera camera, bool dusk)
        {
            var sky = Resources.Load<Material>(dusk ? "InterludeDuskSky" : "InterludeDaySky");
            Assert.That(sky, Is.Not.Null, "Day and dusk skies must be included in the player");
            Assert.That(RenderSettings.skybox, Is.SameAs(sky));
            Assert.That(camera.clearFlags, Is.EqualTo(CameraClearFlags.Skybox));
            Assert.That(RenderSettings.fogDensity, Is.EqualTo(dusk ? .008f : .0025f));
            Assert.That(RenderSettings.ambientMode, Is.EqualTo(AmbientMode.Trilight));
            Assert.That(RenderSettings.ambientSkyColor.maxColorComponent, dusk ? Is.LessThan(.3f) : Is.GreaterThan(.6f));
        }
    }
}
#endif
