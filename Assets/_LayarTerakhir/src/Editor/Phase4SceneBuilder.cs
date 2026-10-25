#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class Phase4SceneBuilder
    {
        public const string ScenePath = "Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity";
        private const string Art = "Assets/_LayarTerakhir/assets/Art/Phase4/", Imported = Art + "Imported/";
        private static Phase4Director d;
        private static Transform env;
        private static readonly List<Phase4Anchor> anchors = new List<Phase4Anchor>();
        private static readonly List<Phase4Zone> zones = new List<Phase4Zone>();
        private static readonly List<Bounds> wards = new List<Bounds>();
        private static readonly List<Phase3EvpSource> sources = new List<Phase3EvpSource>();
        private static readonly List<Phase4Torch> torches = new List<Phase4Torch>();
        private static readonly List<Usage> usage = new List<Usage>();
        [Serializable] private class Usage { public string name, sourceId, source, purpose; public Vector3 position, size; }
        [Serializable] private class SceneRecord { public string scene, scope; public int distinctNewModels; public Usage[] placements; public string[] anchors; }
        [MenuItem("LayarTerakhir/Phase 4/Build Karang Setra")]
        public static void Build()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport); PrepareMaterials();
            EditorUtility.UnloadUnusedAssetsImmediate(); GC.Collect();
            var kos = EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var original = Object.FindFirstObjectByType<FirstPersonController>(); var systems = GameObject.Find("GameSystems"); var events = Object.FindFirstObjectByType<EventSystem>();
            if (!original || !systems) throw new InvalidOperationException("Approved player systems missing");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive); SceneManager.SetActiveScene(scene);
            var core = Object.Instantiate(systems); core.name = "KarangSetra_GameSystems"; Object.DestroyImmediate(core.GetComponent<KamarKosPrologDirector>());
            foreach (var a in core.GetComponentsInChildren<KosSceneAudio>(true)) Object.DestroyImmediate(a.gameObject);
            var player = Object.Instantiate(original.gameObject); player.name = "Player_RakaWijaya";
            if (events && !events.transform.IsChildOf(systems.transform)) Object.Instantiate(events.gameObject).name = "EventSystem";
            EditorSceneManager.CloseScene(kos, true);
            EditorUtility.UnloadUnusedAssetsImmediate(); GC.Collect();
            d = core.AddComponent<Phase4Director>(); d.player = player.GetComponent<FirstPersonController>(); d.world = core.AddComponent<Phase4World>();
            d.phoneView = core.GetComponent<AtmajaPhoneView>() ?? core.AddComponent<AtmajaPhoneView>(); d.phoneView.playerCamera = player.GetComponent<InteractionInteractor>().playerCamera;
            d.phoneView.feedMaterial = PresentationProfile.Load().phoneFeedMaterial; d.phoneView.playerCamera.farClipPlane = 105;
            var cd = d.phoneView.playerCamera.GetUniversalAdditionalCameraData(); cd.renderPostProcessing = true; cd.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            d.balance = AssetDatabase.LoadAssetAtPath<Phase4Balance>(Art + "Phase4Balance.asset");
            if (!d.balance) { d.balance = ScriptableObject.CreateInstance<Phase4Balance>(); AssetDatabase.CreateAsset(d.balance, Art + "Phase4Balance.asset"); }
            d.entity = core.AddComponent<Phase4Banaspati>(); d.entity.director = d;
            d.ritual = core.AddComponent<Phase4Ritual>(); d.ritual.director = d;
            d.soundscape = core.AddComponent<Phase4Soundscape>(); d.soundscape.director = d;
            d.soundscape.routing = AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");
            d.soundscape.sounds = Sounds();
            env = new GameObject("Desa_Mati_Karang_Setra").transform; anchors.Clear(); zones.Clear(); wards.Clear(); sources.Clear(); torches.Clear(); usage.Clear();
            AssetDatabase.StartAssetEditing();
            try
            {
                Architecture(); Debug.Log("PHASE4_STAGE architecture");
                Investigation(); Debug.Log("PHASE4_STAGE investigation");
                Ritual(); Debug.Log("PHASE4_STAGE ritual");
                Dressing(); Debug.Log("PHASE4_STAGE dressing");
                Actors(); Debug.Log("PHASE4_STAGE actors");
                Lighting(); Debug.Log("PHASE4_STAGE lighting");
            }
            finally { AssetDatabase.StopAssetEditing(); }
            foreach(var source in env.GetComponentsInChildren<Phase4AssetSource>(true))
            {
                if(source.transform.IsChildOf(d.world.banaspati.transform)||source.transform.IsChildOf(d.world.suro.transform)||source.transform.IsChildOf(d.world.mergeSuro.transform)||source.transform.IsChildOf(d.world.hiddenDrawer.transform))continue;
                foreach(var child in source.GetComponentsInChildren<Transform>(true))GameObjectUtility.SetStaticEditorFlags(child.gameObject,StaticEditorFlags.BatchingStatic);
            }
            d.world.anchors = anchors.ToArray(); d.world.zones = zones.ToArray(); d.world.wards = wards.ToArray(); d.world.sources = sources.ToArray(); d.world.torches = torches.ToArray();
            var uv = core.AddComponent<Phase4UvPresentation>(); uv.director = d; uv.marks = env.GetComponentsInChildren<Renderer>(true).Where(r => r.gameObject.layer == 24).ToArray();
            var text = core.AddComponent<AtmajaWorldText>(); text.includeARText = true; text.materialTemplate = NewMaterial("WorldText", "LayarTerakhir/WorldText");
            d.world.portalStairs.SetActive(true); d.world.portalCover.SetActive(false); d.world.illusionWall.SetActive(false);
            var nav = env.gameObject.AddComponent<NavMeshSurface>(); nav.collectObjects = CollectObjects.Children; nav.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            nav.layerMask = ~((1 << 24) | (1 << 26) | (1 << 29) | (1 << 31) | (1 << 2)); nav.overrideVoxelSize = true; nav.voxelSize = .16f;
            Debug.Log("PHASE4_STAGE navigation start");
            nav.BuildNavMesh(); if (!nav.navMeshData) throw new InvalidOperationException("Village navigation unavailable");
            Debug.Log("PHASE4_STAGE navigation complete");
            string navpath = Art + "KarangSetraNavMesh.asset"; if (AssetDatabase.LoadAssetAtPath<NavMeshData>(navpath)) AssetDatabase.DeleteAsset(navpath); AssetDatabase.CreateAsset(nav.navMeshData, navpath);
            d.world.portalStairs.SetActive(false); d.world.portalCover.SetActive(true); d.world.illusionWall.SetActive(true);
            player.transform.SetPositionAndRotation(d.world.Anchor("ENTRY").position, d.world.Anchor("ENTRY").rotation);
            int count = usage.Select(u => u.sourceId).Distinct().Count(); if (count < 100) throw new InvalidOperationException("At least 100 NEW placed model sources required, got " + count);
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).Concat(new[] { new EditorBuildSettingsScene(ScenePath, true) }).ToArray(); AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/phase4-scene-content.json", JsonUtility.ToJson(new SceneRecord { scene = ScenePath, distinctNewModels = count, anchors = anchors.Select(a => a.id).ToArray(), placements = usage.ToArray(), scope = "New village/forest/ritual art; shared campaign player/phone/Kos excluded from count" }, true) + "\n");
            Debug.Log("PHASE4_BUILT distinctModels=" + count + " placements=" + usage.Count);
        }
        private static Transform Point(string name, Vector3 at, Transform parent = null, float yaw = 0)
        { var t = new GameObject(name).transform; t.SetParent(parent ? parent : env, false); t.SetPositionAndRotation(at, Quaternion.Euler(0, yaw, 0)); return t; }
        private static void Anchor(string id, Vector3 at, float yaw = 0) => anchors.Add(new Phase4Anchor { id = id, point = Point("Anchor_" + id, at, null, yaw) });
        private static void Zone(string id, Vector3 center, Vector3 size) => zones.Add(new Phase4Zone { id = id, bounds = new Bounds(center, size) });
        private static GameObject Place(string id, string path, Vector3 at, Vector3 size, float yaw, bool collision, string purpose, bool uniform = false)
        {
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(Imported + path + ".fbx"); if (!asset) throw new InvalidOperationException("Missing new source " + path);
            var root = new GameObject(id + "_" + purpose); root.transform.SetParent(env, false); var fit = new GameObject("SourceFit").transform; fit.SetParent(root.transform, false);
            var go = (GameObject)PrefabUtility.InstantiatePrefab(asset); go.transform.SetParent(fit, false);
            var rs = go.GetComponentsInChildren<Renderer>(); if (rs.Length == 0) throw new InvalidOperationException("Empty new model " + id);
            Bounds b = rs[0].bounds; foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
            float longest = Mathf.Max(b.size.x, b.size.y, b.size.z);
            bool natural = new[]{"grass","moss","root","single_root","rock","coast","sand_rocks","shrub","pine_roots","dry_branches","dead_tree","anthurium"}.Any(prefix=>id.StartsWith(prefix,StringComparison.Ordinal));
            // Flat daggers, congklak boards and tools are sized by their length.
            // Scaling a flat keris by thickness produced an eight-metre blade
            // which physically blocked the altar photograph.
            float extent = !natural && longest > b.size.y * 5 ? longest : b.size.y;
            fit.localScale = uniform ? Vector3.one * (size.y / Mathf.Max(.001f, extent)) : new Vector3(size.x / Mathf.Max(.001f, b.size.x), size.y / Mathf.Max(.001f, b.size.y), size.z / Mathf.Max(.001f, b.size.z));
            foreach (var r in rs) { r.sharedMaterials = SourceSlots(id,r).Select(name => MaterialFor(name,id)).ToArray(); r.shadowCastingMode = ShadowCastingMode.On; }
            var authoredLods = go.GetComponentsInChildren<LODGroup>();
            var lowerLods = new HashSet<Renderer>(authoredLods.SelectMany(l => l.GetLODs().Skip(1)).SelectMany(l => l.renderers));
            lowerLods.ExceptWith(authoredLods.SelectMany(l => l.GetLODs().Take(1)).SelectMany(l => l.renderers));
            if (authoredLods.Length == 0) { var lod = root.AddComponent<LODGroup>(); lod.SetLODs(new[] { new LOD(id == "BambooSet" ? .065f : .009f, rs) }); lod.RecalculateBounds(); }
            b = rs[0].bounds; foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds); fit.position -= new Vector3(b.center.x, b.min.y, b.center.z);
            root.transform.SetPositionAndRotation(at, Quaternion.Euler(0, yaw, 0));
            if (collision) foreach (var mesh in go.GetComponentsInChildren<MeshFilter>())
            {
                if (lowerLods.Contains(mesh.GetComponent<Renderer>())) continue;
                var c = mesh.gameObject.GetComponent<MeshCollider>(); if (!c) c = mesh.gameObject.AddComponent<MeshCollider>(); c.sharedMesh = mesh.sharedMesh;
            }
            if (collision && id != "ForestStairs")
            {
                // The shared agent permits 75cm climbs while Raka steps 30cm.
                // Route around props instead of baking a path over their tops.
                var obstacle = root.AddComponent<NavMeshModifier>(); obstacle.overrideArea = true; obstacle.area = 1;
            }
            var tag = root.AddComponent<Phase4AssetSource>(); tag.sourceId = id; tag.sourcePath = Imported + path + ".fbx"; tag.purpose = purpose;
            usage.Add(new Usage { name = root.name, sourceId = id, source = tag.sourcePath, purpose = purpose, position = at, size = size }); return root;
        }
        private static GameObject Model(string id, Vector3 at, float height, string purpose, float yaw = 0, bool collision = true) => Place(id, "Models/" + id, at, Vector3.one * height, yaw, collision, purpose, true);
        private static GameObject Module(string id, Vector3 at, Vector3 size, string purpose, float yaw = 0, bool collision = true)
        {
            var go = Place(id.StartsWith("Bamboo") ? "BambooSet" : "Joglo", "Modules/" + id, at, size, yaw, collision, purpose);
            // The licensed slab was authored with its broad finished face down.
            // Orient that existing face upward without changing its geometry.
            if (id == "JogloFloor") go.transform.RotateAround(at + Vector3.up * size.y * .5f, Vector3.forward, 180);
            if (id == "JogloRoof" || id == "JogloPendopo" || id == "JogloHouse") SurfaceOn(go, "clay_roof_tiles", new Vector2(size.x/2,size.z/2), true);
            return go;
        }
        private static GameObject Part(string source, int index, Vector3 at, float height, string purpose) => Place(source, "Parts/" + source + "_part_" + index, at, Vector3.one * height, 0, false, purpose, true);
        private static GameObject Proxy(string name, Vector3 center, Vector3 size, Transform parent = null)
        { var g = Point(name, center, parent).gameObject; var c = g.AddComponent<BoxCollider>(); c.size = size; return g; }
        private static void SurfaceOn(GameObject go, string id, Vector2 tiling, bool roofOnly = false)
        {
            // Preserve authored UVs but compensate their existing repeats. The
            // source concrete slab already repeats 56x28; multiplying that by
            // world dimensions made the ground an unreadable subpixel pattern.
            foreach (var renderer in go.GetComponentsInChildren<Renderer>())
            {
                bool IsRoof(Material mat) => mat && mat.name.Contains("Roof_Tiles");
                if (roofOnly && !renderer.sharedMaterials.Any(IsRoof)) continue;
                var mesh = renderer.GetComponent<MeshFilter>()?.sharedMesh;
                var repeat = tiling;
                var source = go.GetComponent<Phase4AssetSource>();
                // The licensed slab's U runs along world Z, V along world X.
                // Narrow roads otherwise stretched a two-metre texture over20m.
                if(source && source.sourcePath.EndsWith("/JogloFloor.fbx",StringComparison.Ordinal))repeat=new Vector2(tiling.y,tiling.x);
                if (mesh && mesh.uv.Length > 0)
                {
                    var uv = mesh.uv;
                    repeat = new Vector2(repeat.x / Mathf.Max(.001f, uv.Max(v => v.x) - uv.Min(v => v.x)), repeat.y / Mathf.Max(.001f, uv.Max(v => v.y) - uv.Min(v => v.y)));
                }
                string key="SurfaceFit_"+id+"_"+repeat.x.ToString("0.0000",System.Globalization.CultureInfo.InvariantCulture)+"_"+repeat.y.ToString("0.0000",System.Globalization.CultureInfo.InvariantCulture);
                if(!materials.TryGetValue(key,out var m)){m=NewMaterial(key,"Universal Render Pipeline/Lit");m.CopyPropertiesFromMaterial(Surface(id));m.SetTextureScale("_BaseMap",repeat);m.SetTextureScale("_BumpMap",repeat);m.SetTextureScale("_MetallicGlossMap",repeat);m.SetTextureScale("_OcclusionMap",repeat);materials[key]=m;EditorUtility.SetDirty(m);}
                renderer.sharedMaterials = renderer.sharedMaterials.Select(old => !roofOnly || IsRoof(old) ? m : old).ToArray();
            }
        }
        private static GameObject Floor(string name, Vector3 center, Vector3 size, string surface)
        {
            var g = Module("JogloFloor", center - Vector3.up * .18f, new Vector3(size.x, .18f, size.z), name, 0, false); SurfaceOn(g, surface, new Vector2(size.x / 2, size.z / 2));
            Proxy(name + "_CollisionOnly", center - Vector3.up * .12f, new Vector3(size.x, .24f, size.z)); return g;
        }
        private static Phase4WorldItem Item(GameObject g, string id, string prompt)
        {
            var c = g.GetComponent<Collider>();
            if (!c)
            {
                var box = g.AddComponent<BoxCollider>(); box.isTrigger = true;
                var renderers = g.GetComponentsInChildren<Renderer>();
                if (renderers.Length > 0)
                {
                    var bounds = renderers[0].bounds; foreach(var r in renderers.Skip(1)) bounds.Encapsulate(r.bounds);
                    box.center = g.transform.InverseTransformPoint(bounds.center); box.size = bounds.size;
                }
                else box.size = new Vector3(.7f,.6f,.7f);
                c = box;
            }
            var i = g.AddComponent<Phase4WorldItem>(); i.director = d; i.id = id; i.prompt = prompt; return i;
        }
        private static Phase4WorldItem Interaction(string name, Vector3 at, string id, string prompt) => Item(Point(name, at).gameObject, id, prompt);
        private static void Label(string name, string text, Vector3 at, float yaw = 0, float size = .15f)
        {
            var t = Point("P4Sign_" + name, at, null, yaw).gameObject.AddComponent<TextMesh>(); t.text = text; t.font = PresentationProfile.Load().bodyFont; t.characterSize = size * .11f; t.fontSize = 64; t.anchor = TextAnchor.MiddleCenter; t.alignment = TextAlignment.Center; t.color = new Color(.76f, .72f, .58f); t.gameObject.layer = 26; t.GetComponent<Renderer>().sharedMaterial = t.font.material;
            if (name.StartsWith("Recovery_") || name.StartsWith("Branch_") || name.StartsWith("Sector_") || name.StartsWith("Hide_"))
            {
                var board = Place("hanging_picture_frame_02", "Models/hanging_picture_frame_02", at - Vector3.up * .32f + Quaternion.Euler(0,yaw,0)*Vector3.forward*.035f, new Vector3(2.6f,.65f,.045f), yaw, true, "Papan petunjuk " + name);
                SurfaceOn(board,"wood_planks_dirt",Vector2.one);
                if(at.y>0)Module("JogloPost",new Vector3(at.x,0,at.z+.08f),new Vector3(.07f,at.y-.3f,.07f),"Penyangga papan petunjuk",yaw,true);
            }
        }
        private static void SolidTrunks(GameObject model)
        {
            var obstacle=model.GetComponent<NavMeshModifier>();if(!obstacle)obstacle=model.AddComponent<NavMeshModifier>();obstacle.overrideArea=true;obstacle.area=1;
            foreach(var mesh in model.GetComponentsInChildren<MeshFilter>())
            {
                var r=mesh.GetComponent<Renderer>(); if(!r)continue;
                if(!r.sharedMaterials.Any(m=>m&&(m.name.Contains("bamboobranch")||m.name.Contains("Bark")||m.name.Contains("Branches")||m.name.Contains("Vines"))))continue;
                var c=mesh.gameObject.GetComponent<MeshCollider>();if(!c)c=mesh.gameObject.AddComponent<MeshCollider>();c.sharedMesh=mesh.sharedMesh;
            }
        }
        private static GameObject Mark(string id, Vector3 at, float size, Color color, Quaternion rotation, Transform parent = null)
        {
            var g = Point("WorldMark_" + id, at, parent).gameObject; g.layer = 24; g.transform.rotation = rotation;
            var s = g.AddComponent<SpriteRenderer>(); s.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Imported + "Glyphs/" + id + ".png"); s.color = color;
            s.sharedMaterial = Glyph(id, Color.white); float width = s.sprite ? s.sprite.bounds.size.x : 5.12f; g.transform.localScale = Vector3.one * size / width; return g;
        }
        private static Light Lamp(string name, Vector3 at, Color color, float intensity, float range, bool shadows = false)
        { var l = Point(name, at).gameObject.AddComponent<Light>(); l.type = LightType.Point; l.color = color; l.intensity = intensity; l.range = range; l.shadows = shadows ? LightShadows.Soft : LightShadows.None; return l; }
        private static Phase4Sound[] Sounds()
        {
            var files = Directory.GetFiles(Imported + "Audio", "*.ogg"); AudioClip Find(string text) => AssetDatabase.LoadAssetAtPath<AudioClip>(files.FirstOrDefault(f => Path.GetFileName(f).StartsWith(text)) ?? "");
            var fire = Find("Fire.ogg"); var ghost = Find("Ghostly_"); var water = Find("Water_"); var score = Find("BambooForest"); var heart = Find("Heartbeat"); var burst = Find("FireBurst");
            return new[] { new Phase4Sound { id = "Ambient", clip = Find("Wind") }, new Phase4Sound { id = "Spirit", clip = ghost }, new Phase4Sound { id = "Score", clip = score }, new Phase4Sound { id = "Market", clip = Find("Market") }, new Phase4Sound { id = "Heartbeat", clip = heart }, new Phase4Sound { id = "Fire", clip = fire }, new Phase4Sound { id = "Water", clip = water } }.Concat(new[] { "Reveal", "FirePass", "Lunge", "EmberWarning", "EmberBurst", "ShieldBreak", "Hit", "Miss", "FalseTorch", "Ritual", "Incense", "Gate", "Shutter", "Drawer", "Knock", "Whisper", "Pour", "Recovery" }.Select(id => new Phase4Sound { id = id, clip = id=="Whisper"||id=="Ritual"?ghost:id=="Incense"?fire:id=="Gate"?Find("Market"):id == "Knock"||id=="EmberWarning"||id=="Drawer"||id=="Shutter" ? Find("Knock") : id=="Pour"||id=="Recovery"?water:burst })).ToArray();
        }
        public static void BuildLinuxReview()
        {
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            var r = BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(), target = BuildTarget.StandaloneLinux64, locationPathName = "Builds/LinuxPhase4/LayarTerakhir.x86_64", options = BuildOptions.Development });
            File.WriteAllText("Audit/phase4-linux-build.json", "{\"result\":\"" + r.summary.result + "\",\"errors\":" + r.summary.totalErrors + ",\"warnings\":" + r.summary.totalWarnings + ",\"bytes\":" + r.summary.totalSize + "}\n");
            if (r.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded || r.summary.totalErrors != 0) throw new Exception("Phase 4 native build failed");
        }
        public static void BuildAndLinuxReview() { Build(); BuildLinuxReview(); }
    }
}
#endif
