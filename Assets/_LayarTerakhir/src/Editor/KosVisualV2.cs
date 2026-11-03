#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Incremental corrections to the furnished scene. Never rebuilds the greybox or removes V1 models.
    public static class KosVisualV2
    {
        private const string Art = "Assets/_LayarTerakhir/assets/Art";
        private const string Mats = Art + "/Generated/Materials/";
        private static Transform details;
        public static Bounds BoundsOf(GameObject go)
        {
            var rs = go.GetComponentsInChildren<Renderer>().Where(r => r.enabled && !(r is LineRenderer)).ToArray();
            if (rs.Length == 0) throw new InvalidOperationException("No visible geometry: " + go.name);
            var b = rs[0].bounds;
            foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
            return b;
        }
        private static GameObject Find(string name)
        {
            // FBX roots often share a name with their scene holder. Always choose the explicit art holder.
            var art=GameObject.Find("Kos_Environment_Art");
            var holder=art ? art.transform.Find(name) ?? art.transform.Find("Kos_V2_Details/"+name) : null;
            return holder ? holder.gameObject : GameObject.Find(name) ?? throw new Exception("Missing scene object: " + name);
        }
        private static void ResetPrefabRoot(GameObject holder,float yaw)
        {
            // Some legacy holders carry their mesh directly (no FBX child);
            // there is nothing to reset — orientation is handled downstream.
            if (holder.transform.childCount == 0) return;
            var model=holder.transform.GetChild(0);
            var source=PrefabUtility.GetCorrespondingObjectFromSource(model);
            if(!source)throw new Exception("Expected FBX instance root: "+holder.name);
            model.localPosition=source.localPosition;model.localScale=source.localScale;
            model.localRotation=Quaternion.Euler(0,yaw,0)*source.localRotation;
            holder.transform.rotation=Quaternion.identity;
        }
        private static Material Mat(string name) => AssetDatabase.LoadAssetAtPath<Material>(Mats + name + ".mat");

        [MenuItem("LayarTerakhir/Environment/Apply furnished Kos V2 corrections")]
        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            // Only this pass's own additions are regenerated on repeat runs.
            var previous = GameObject.Find("Kos_V2_Details");
            if (previous) Object.DestroyImmediate(previous);
            details = new GameObject("Kos_V2_Details").transform;
            details.SetParent(Find("Kos_Environment_Art").transform, false);
            ConfigureNewImports();
            CorrectArchitecture();
            CorrectFurniture();
            DressRoom();
            LightingAndMaterials();
            // V2 furniture is authored pre-layout; the Compact pass owns the
            // final 5.0 x 3.8m arrangement, so run it before gating — the
            // validation below checks the shipped layout, not an intermediate.
            CompactKosPolish.ApplyToOpenScene();
            // Backrest direction is judged against the final desk spot.
            OrientChairToDesk();
            // Compact moves assemblies rigidly, so anything V2 placed by
            // absolute coords pre-layout strands (monitor, chair, window
            // dressing, cables...). Seat everything gameplay-visible here
            // from LIVE bounds — absolute and idempotent.
            SeatFinalLayout();
            foreach (var camera in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (!camera.CompareTag("MainCamera")) continue;
                var data = camera.GetUniversalAdditionalCameraData();
                data.renderPostProcessing = true;
                data.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                camera.allowHDR = true;
            }
            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            KosSceneValidation.Validate();
            Debug.Log("KOS_V2 Applied incremental furnished-room corrections; compact 5.0 x 3.8m layout validated.");
        }

        // Designer Nov 2026: Raka's wardrobe faces the east wall (backwards).
        // Turns it 180° in place so the doors face the room. Position-preserving
        // (rotates about the bounds centre, footprint identical under a half
        // turn), so the CompactKosPolish layout and collider aprons are untouched.
        // Idempotent: skips when the doors already face west. Full Apply() is not
        // used here — its chair pass predates the current scene structure.
        public static void ApplyWardrobeFacing()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var wardrobe = Find("MeshyBedroomWardrobe");
            float yaw = wardrobe.transform.rotation.eulerAngles.y;
            float east = Mathf.Abs(Mathf.DeltaAngle(yaw, 90f));
            float west = Mathf.Abs(Mathf.DeltaAngle(yaw, 270f));
            if (west <= east)
            {
                Debug.Log("KOS_WARDROBE_FACING already west (yaw " + yaw.ToString("F1") + "), no change");
                return;
            }
            var before = BoundsOf(wardrobe);
            wardrobe.transform.RotateAround(before.center, Vector3.up, 180f);
            Physics.SyncTransforms();
            var after = BoundsOf(wardrobe);
            if ((after.center - before.center).magnitude > .01f)
                throw new Exception("Wardrobe drifted while turning");
            FitBox(Find("LemariPakaian").GetComponent<BoxCollider>(), after);
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("KOS_WARDROBE_FACING turned to face the room (yaw " + wardrobe.transform.rotation.eulerAngles.y.ToString("F1") + ")");
        }

        private static void ConfigureNewImports()
        {
            var paths = new[] { "Imported/PolyHaven/plastic_container", "Imported/PolyHaven/wicker_basket_02", "Imported/PolyHaven/fabric_pattern_07" };
            foreach (var relative in paths)
            foreach (var path in Directory.GetFiles(Art + "/" + relative))
            {
                if (AssetImporter.GetAtPath(path) is TextureImporter ti)
                {
                    ti.maxTextureSize = relative.Contains("fabric") ? 1024 : 512;
                    ti.textureType = path.Contains("_nor_gl_") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                    ti.sRGBTexture = !path.Contains("_mask") && !path.Contains("_nor_gl_") && !path.Contains("_arm_");
                    ti.mipmapEnabled = true; ti.anisoLevel = 2;
                    ti.textureCompression = TextureImporterCompression.Compressed;
                    ti.SaveAndReimport();
                }
            }
            // Designer-provided Meshy wardrobe: cap textures like everything
            // else, but KEEP its authored textured material (the clothes must
            // read as clothes, never a flat repo tint).
            foreach (var path in Directory.GetFiles(Art + "/Imported/Meshy/wardrobe"))
            {
                if (AssetImporter.GetAtPath(path) is TextureImporter ti)
                {
                    ti.maxTextureSize = 1024;
                    ti.textureType = path.Contains("_normal") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                    ti.sRGBTexture = !path.Contains("_normal") && !path.Contains("_metallic") && !path.Contains("_roughness");
                    ti.mipmapEnabled = true; ti.anisoLevel = 2;
                    ti.textureCompression = TextureImporterCompression.Compressed;
                    ti.SaveAndReimport();
                }
            }
            foreach (var path in Directory.GetFiles(Art, "*.fbx", SearchOption.AllDirectories)
                .Where(p => p.Contains("/Kos") || p.Contains("plastic_container/") || p.Contains("wicker_basket_02/")))
            {
                var mi = (ModelImporter)AssetImporter.GetAtPath(path);
                mi.importCameras = false; mi.importLights = false; mi.importAnimation = false;
                mi.addCollider = false; mi.materialImportMode = ModelImporterMaterialImportMode.None;
                mi.meshCompression = ModelImporterMeshCompression.Low; mi.SaveAndReimport();
            }
            foreach (var path in Directory.GetFiles(Art + "/Imported/Meshy/wardrobe", "*.fbx"))
            {
                var mi = (ModelImporter)AssetImporter.GetAtPath(path);
                if (mi == null) continue;
                mi.importCameras = false; mi.importLights = false; mi.importAnimation = false;
                mi.addCollider = false;
                mi.meshCompression = ModelImporterMeshCompression.Low; mi.SaveAndReimport();
            }
        }

        private static void CorrectArchitecture()
        {
            // V1's left wall ended at x=1.19, covering 43cm of the door and leaving a west-corner gap.
            foreach (var name in new[] { "SouthLeftDado", "SouthLeftLimewash", "SouthSkirtingLeft" })
            {
                var t = Find(name).transform; var p = t.position; p.x = -1.12f; t.position = p;
                var s = t.localScale; s.x = 3.76f; t.localScale = s;
            }
            foreach (var name in new[] { "SouthRightDado", "SouthRightLimewash", "SouthSkirtingRight" })
            {
                var t = Find(name).transform; var p = t.position; p.x = 2.37f; t.position = p;
                var s = t.localScale; s.x = 1.26f; t.localScale = s;
            }
            // Keep the recognizable existing window; set the glass just inside the closed structural wall.
            var glass = Find("DarkNightBeyondWindow").transform;
            glass.position = new Vector3(2.992f, 1.65f, -.55f);
            Resize(Find("wallWindow"),new Vector3(.12f,1.12f,1.10f));
            SetCenter(Find("wallWindow"), new Vector3(2.965f, 1.65f, -.55f));
            foreach(var r in Find("wallWindow").GetComponentsInChildren<Renderer>())
                r.sharedMaterials=Enumerable.Repeat(r.name.Contains("glass")?Mat("NightWindowGlass"):r.name.Contains("metal")?Mat("DarkOxidisedMetal"):Mat("DoorAndWardrobeWood"),r.sharedMaterials.Length).ToArray();
            var lintel=Find("DoorLintelPlaster").transform;
            lintel.localScale=new Vector3(.98f,lintel.localScale.y,lintel.localScale.z);
            var shelf = Find("wooden_bookshelf_worn");
            SetCenter(shelf, new Vector3(-2.84f, 1.94f, .95f));
            // A small physical gap prevents the wall and bookshelf backs occupying the same plane.
            var wood = Mat("DoorAndWardrobeWood");
            Rail("CurtainRod", new Vector3(2.80f, 2.29f, -1.24f), new Vector3(2.80f, 2.29f, .14f), .014f, Mat("DarkOxidisedMetal"));
            foreach (float z in new[] { -1.24f, .14f })
                Rail("CurtainRodBracket", new Vector3(2.80f, 2.29f, z), new Vector3(2.98f, 2.29f, z), .008f, wood);
            foreach (float y in new[] { .32f, 1.05f, 1.81f })
                DetailBox("DoorHinge", new Vector3(.78f, y, -2.16f), new Vector3(.045f, .075f, .018f), Mat("WornBrass"));
            Rail("DoorHandleStem", new Vector3(1.63f, .99f, -2.17f), new Vector3(1.63f, .99f, -2.12f), .012f, Mat("WornBrass"));
            Rail("DoorHandleLever", new Vector3(1.63f, .99f, -2.12f), new Vector3(1.54f, .99f, -2.12f), .011f, Mat("WornBrass"));
        }

        [MenuItem("LayarTerakhir/Environment/Replace Kos wardrobe with Meshy one")]
        public static void ReplaceWardrobeOnly()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var wardrobe = SpawnMeshyWardrobe();
            FitBox(Find("LemariPakaian").GetComponent<BoxCollider>(), BoundsOf(wardrobe));
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("KOS_WARDROBE_REPLACED Meshy wardrobe with visible clothes at " + wardrobe.transform.position);
        }
        // Designer: Raka's room gets a real wardrobe with visible clothes
        // (designer-provided Meshy generation), replacing the Kenney bookcase.
        // Idempotent: removes any previous holder, then spawns fresh.
        public static GameObject SpawnMeshyWardrobe()
        {
            const string fbx = Art + "/Imported/Meshy/wardrobe/Meshy_AI_Untidy_Bedroom_Wardrobe_20261103_texture_remesh_tri.fbx";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fbx);
            if (!prefab) throw new Exception("Missing Meshy wardrobe FBX: " + fbx);
            var artRoot = Find("Kos_Environment_Art").transform;
            foreach (string stale in new[] { "MeshyBedroomWardrobe", "bookcaseClosedDoors" })
            {
                var t = artRoot.Find(stale);
                if (t) Object.DestroyImmediate(t.gameObject);
                var g = GameObject.Find(stale);
                if (g) Object.DestroyImmediate(g);
            }
            var holder = new GameObject("MeshyBedroomWardrobe");
            holder.transform.SetParent(artRoot, false);
            var model = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            model.transform.SetParent(holder.transform, false);
            // Authored front (+Z: open door, hanging + folded clothes) faces
            // the room (west, -X) — same convention as the old Kenney front.
            // Yaw lives on the holder so facing checks read it directly.
            holder.transform.rotation = Quaternion.Euler(0, 270, 0);
            foreach (var c in model.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c);
            // World fit AFTER yaw: shallow in x (east wall), wide in z.
            // FitWorldSize routes axes through the holder frame, so the
            // pitched Meshy root cannot skew the footprint.
            FitWorldSize(holder, new Vector3(.68f, 1.95f, 1.00f));
            SetCenter(holder, new Vector3(2.12f, .975f, 1.25f));
            Ground(holder);
            BindWardrobeAuthoredMaterial(holder);
            return holder;
        }
        // The remesh FBX references designer-absolute texture paths, so its
        // embedded material imports with no base map (white wardrobe, white
        // clothes). Bind the shipped atlas explicitly; idempotent across
        // reimports since it runs on every spawn.
        private static void BindWardrobeAuthoredMaterial(GameObject holder)
        {
            const string dir = Art + "/Imported/Meshy/wardrobe/";
            var albedo = AssetDatabase.LoadAssetAtPath<Texture2D>(dir + "Meshy_AI_Untidy_Bedroom_Wardrobe_20261103_texture_remesh_tri.png");
            var normal = AssetDatabase.LoadAssetAtPath<Texture2D>(dir + "Meshy_AI_Untidy_Bedroom_Wardrobe_20261103_texture_remesh_tri_normal.png");
            if (!albedo) throw new Exception("Missing wardrobe albedo atlas: " + dir);
            const string matPath = Art + "/Generated/Materials/MeshyWardrobeAuthored.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (!mat) { mat = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(mat, matPath); }
            mat.SetTexture("_BaseMap", albedo);
            if (normal) { mat.SetTexture("_BumpMap", normal); mat.EnableKeyword("_NORMALMAP"); mat.SetFloat("_BumpScale", 1f); }
            mat.SetColor("_BaseColor", Color.white);
            mat.SetFloat("_Metallic", 0f);
            mat.SetFloat("_Smoothness", .35f);
            EditorUtility.SetDirty(mat);
            foreach (var r in holder.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(mat, r.sharedMaterials.Length).ToArray();
            Debug.Log("KOS_WARDROBE_MATERIAL bound authored atlas on " + holder.name);
        }
        // Legacy passes left a nest of same-named holders, 100x-scaled ancestors
        // and children whose meshes were stripped: the art-root holder can
        // end up squashed flat with no usable surface. Rebuild it
        // deterministically: reparent under the art root, reset local pose,
        // drop every child, sweep same-named strays scene-wide, and respawn
        // the FBX model fresh. Downstream Resize/SetCenter/Ground then fit it.
        public static void EnsurePrefabChild(GameObject holder,string fbxPath,Vector3 canonicalXZ)
        {
            var artRoot = Find("Kos_Environment_Art").transform;
            holder.transform.SetParent(artRoot, false);
            // Rough slot only; Resize scales about the holder origin, so the
            // caller must SetCenter AFTER resizing (pivot != mesh center).
            holder.transform.localPosition = new Vector3(canonicalXZ.x, 0, canonicalXZ.y);
            holder.transform.localRotation = Quaternion.identity;
            holder.transform.localScale = Vector3.one;
            foreach (Transform t in holder.transform) Object.DestroyImmediate(t.gameObject);
            // A flattened legacy state can also park the mesh directly on the
            // holder; drop those so the fresh instance is the only geometry.
            foreach (var c in holder.GetComponents<MeshFilter>()) Object.DestroyImmediate(c);
            foreach (var c in holder.GetComponents<MeshRenderer>()) Object.DestroyImmediate(c);
            foreach (var c in holder.GetComponents<SkinnedMeshRenderer>()) Object.DestroyImmediate(c);
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (t && t.name == holder.name && t.gameObject != holder && !t.IsChildOf(holder.transform))
                    Object.DestroyImmediate(t.gameObject);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            if (!prefab) throw new Exception("Missing model: " + fbxPath);
            var model = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            model.transform.SetParent(holder.transform, false);
            var source = PrefabUtility.GetCorrespondingObjectFromSource(model);
            if (source) { model.transform.localPosition = source.transform.localPosition; model.transform.localScale = source.transform.localScale; model.transform.localRotation = source.transform.localRotation; }
            model.name = holder.name + "_Model";
            foreach (var c in model.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c);
            // Leave final centering to the caller: Resize scales about the
            // holder origin, so it must run before any SetCenter.
            Debug.Log("KOS_HOLDER_REBUILT " + holder.name + " from " + fbxPath);
        }
        // Chair backrest vs desk is evaluated on the FINAL (post-Compact)
        // layout, so this runs after the Compact pass: seat the sitter
        // south of the final desk (clear of the desktop footprint), aim the
        // backrest opposite the desk vector, then keep the lived-in tilt.
        // The seat collision is refit here too — the pre-Compact fit rides
        // stale once the assembly moves.
        public static void OrientChairToDesk()
        {
            var chair = Find("painted_wooden_chair_01");
            var desk = Find("wooden_table_02");
            var db = BoundsOf(desk);
            var cb = BoundsOf(chair);
            SetCenter(chair, new Vector3(db.center.x + .05f, cb.center.y, db.min.z - cb.size.z * .5f - .10f));
            Ground(chair);
            cb = BoundsOf(chair);
            var toDesk = BoundsOf(desk).center - cb.center; toDesk.y = 0;
            var high = MeshVertices(chair).Where(v => v.y > cb.min.y + .65f).ToArray();
            if (high.Length > 0 && toDesk.sqrMagnitude > .000001f)
            {
                var back = new Vector3(high.Average(v => v.x), 0, high.Average(v => v.z)) - new Vector3(cb.center.x, 0, cb.center.z);
                if (back.sqrMagnitude > .000001f)
                    chair.transform.RotateAround(BoundsOf(chair).center, Vector3.up, Vector3.SignedAngle(back, -toDesk, Vector3.up));
            }
            chair.transform.RotateAround(BoundsOf(chair).center, Vector3.up, -7);
            Ground(chair);
            var chairB = BoundsOf(chair);
            var seatGo = Find("ChairSeatCollision");
            seatGo.transform.position = new Vector3(chairB.center.x, .24f, chairB.center.z);
            FitBox(seatGo.GetComponent<BoxCollider>(), new Bounds(new Vector3(chairB.center.x, .24f, chairB.center.z), new Vector3(chairB.size.x, .47f, chairB.size.z)));
            Debug.Log("KOS_CHAIR_SEATED south of desk at " + chair.transform.position);
        }
        // Pull nested same-named holders up to the art root (world-stays)
        // until flat, then drop holders left with no mesh. Kills the nesting
        // disease where origin-based Resize/SetCenter pinball children
        // across runs (books stacked inside the cupboard, doubled boxes).
        private static void FlattenNested(string name)
        {
            var artRoot = Find("Kos_Environment_Art").transform;
            bool moved;
            do
            {
                moved = false;
                foreach (var t in artRoot.GetComponentsInChildren<Transform>(true).ToArray())
                {
                    if (t == artRoot || t.name != name || t.parent == artRoot || t.parent == null) continue;
                    if (t.parent.name != name) continue;
                    t.SetParent(artRoot, true);
                    moved = true;
                }
            } while (moved);
            foreach (var t in artRoot.GetComponentsInChildren<Transform>(true).ToArray())
            {
                if (t == artRoot || t.name != name) continue;
                if (!t.GetComponentsInChildren<MeshFilter>(true).Any(f => f.sharedMesh))
                    Object.DestroyImmediate(t.gameObject);
            }
        }
        private static void ArtCable(string name,Vector3[] points,float width,Material material)
        {
            var go = new GameObject(name, typeof(LineRenderer));
            go.transform.SetParent(Find("Kos_Environment_Art").transform, false);
            var line = go.GetComponent<LineRenderer>();
            line.positionCount = points.Length; line.SetPositions(points);
            line.widthMultiplier = width; line.numCornerVertices = 2; line.numCapVertices = 2;
            line.sharedMaterial = material; line.generateLightingData = true;
        }
        // Post-Compact final seating: Compact moves assemblies rigidly, so
        // anything V2 placed by absolute coords pre-layout strands (monitor,
        // window dressing, desk cables...). Seat everything visible here from
        // LIVE bounds — absolute and idempotent, so every Apply converges no
        // matter which legacy generation stranded it.
        private static void SeatFinalLayout()
        {
            var artRoot = Find("Kos_Environment_Art").transform;
            var desk = Find("wooden_table_02");
            var db = BoundsOf(desk);
            float deskTop = db.max.y;
            var director = Object.FindFirstObjectByType<KamarKosPrologDirector>();

            // Monitor back on the desk, screen facing the chair (south).
            var monitorMesh = director.pcMonitorItem.transform.Find("computerScreen").gameObject;
            var screenB = BoundsOf(monitorMesh);
            SetCenter(director.pcMonitorItem.gameObject, new Vector3(db.center.x - .05f, deskTop + screenB.size.y * .5f + .002f, db.center.z + .03f));
            AlignItem(director.pcMonitorItem, BoundsOf(monitorMesh));
            var monitorB = BoundsOf(director.pcMonitorItem.gameObject);

            // Webcam back on top of the reseated screen (Compact.Dress ran pre-seat).
            var webcam = GameObject.Find("BudgetStreamingWebcam");
            if (webcam)
            {
                Resize(webcam, new Vector3(.085f, .055f, .046f));
                SetCenter(webcam, new Vector3(monitorB.center.x, monitorB.max.y + .020f, monitorB.min.z + .015f));
                var lens = webcam.GetComponentsInChildren<Renderer>().Single(r => r.name.Contains("Lens"));
                if (Vector3.Dot(lens.bounds.center - BoundsOf(webcam).center, new Vector3(0, 0, -1)) < 0)
                    webcam.transform.RotateAround(BoundsOf(webcam).center, Vector3.up, 180);
                var lead = GameObject.Find("WebcamUSBLead")?.GetComponent<LineRenderer>();
                if (lead)
                {
                    lead.positionCount = 4;
                    lead.SetPositions(new[]{ webcam.transform.position,
                        new Vector3(monitorB.center.x + .10f, monitorB.max.y, monitorB.max.z),
                        new Vector3(monitorB.center.x + .14f, .78f, monitorB.max.z),
                        new Vector3(monitorB.center.x + .10f, .08f, db.max.z + .02f) });
                }
            }
            var glow = GameObject.Find("PCMonitorGlow");
            if (glow) glow.transform.position = monitorB.center + new Vector3(0, .10f, -.25f);
            var pool = GameObject.Find("DeskLampPool");
            if (pool) pool.transform.position = BoundsOf(Find("desk_lamp_arm_01")).center + Vector3.up * .10f;

            // Desk colliders from the FINAL desk spot. Origins are snapped to
            // the target first so the legacy 6m-era holder offsets (which
            // rode the workstation holder for generations) cannot accumulate.
            var mejaGo = Find("MejaKerjaPC");
            mejaGo.transform.position = new Vector3(db.center.x, deskTop - .036f, db.center.z);
            FitBox(mejaGo.GetComponent<BoxCollider>(), new Bounds(new Vector3(db.center.x, deskTop - .036f, db.center.z), new Vector3(db.size.x, .072f, db.size.z)));
            foreach (var pair in new[] { ("KakiMejaKiri", -.55f), ("KakiMejaKanan", .55f) })
            {
                var legGo = Find(pair.Item1);
                legGo.transform.position = new Vector3(db.center.x + pair.Item2, legGo.transform.position.y, db.center.z);
                var c = legGo.GetComponent<BoxCollider>();
                var b = c.bounds;
                b.center = new Vector3(db.center.x + pair.Item2, b.center.y, db.center.z);
                FitBox(c, b);
            }

            // Window strays back to the east opening (bars inside the glass).
            float glassX = BoundsOf(Find("DarkNightBeyondWindow")).center.x;
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (t.name == "WindowSecurityBar") t.position = new Vector3(glassX - .05f, t.position.y, t.position.z);
                else if (t.name == "WindowSill") t.position = new Vector3(glassX - .06f, t.position.y, t.position.z);
            }

            // Books: flatten nesting, then stack on the live tops.
            FlattenNested("books");
            var cupboard = Find("painted_wooden_cabinet_02");
            var cupTop = BoundsOf(cupboard);
            // The cabinet top face is open at the naive centre point (the
            // ray falls through to an inner shelf), so sample the live top
            // for a real surface and stack there — RestOn is raycast-
            // consistent by construction, which the surface-contact gate
            // requires (gap within [-8mm, +12mm]).
            float stackX = cupTop.center.x, stackZ = cupTop.center.z;
            float bestTop = float.NegativeInfinity;
            for (float ox = -.15f; ox <= .151f; ox += .05f)
                for (float oz = -.10f; oz <= .101f; oz += .05f)
                {
                    try
                    {
                        float h = SurfaceHeight(cupboard, cupTop.center.x + ox, cupTop.center.z + oz);
                        if (h > bestTop) { bestTop = h; stackX = cupTop.center.x + ox; stackZ = cupTop.center.z + oz; }
                    }
                    catch { }
                }
            if (float.IsNegativeInfinity(bestTop) || bestTop < cupTop.max.y - .05f)
                throw new Exception("No book stack surface on cupboard top (best " + bestTop + ")");
            var shelfTop = BoundsOf(Find("wooden_bookshelf_worn"));
            GameObject stackSupport = cupboard;
            foreach (var book in artRoot.Cast<Transform>().Where(t => t.name == "books").Select(t => t.gameObject).OrderBy(g => BoundsOf(g).min.y).ToArray())
            {
                if (BoundsOf(book).center.x < 0)
                {
                    Resize(book, new Vector3(.15f, .09f, .42f));
                    RestOn(book, Find("wooden_bookshelf_worn"), shelfTop.center.x, shelfTop.center.z);
                }
                else
                {
                    Resize(book, new Vector3(.26f, .075f, .11f));
                    RestOn(book, stackSupport, stackX, stackZ);
                    stackSupport = book;
                }
            }

            // Shipping box to its layout spot, collision refit.
            FlattenNested("cardboardBoxOpen");
            var box = Find("cardboardBoxOpen");
            SetCenter(box, new Vector3(2.12f, BoundsOf(box).center.y, -.48f));
            Ground(box);
            var boxColGo = Find("CardboardBoxCollision");
            boxColGo.transform.position = BoundsOf(box).center;
            FitBox(boxColGo.GetComponent<BoxCollider>(), BoundsOf(box));

            // Desk cables: kill every generation's dupes, route ONE from live spots.
            var black = Mat("OldComputerPlastic");
            foreach (var n in new[] { "MonitorSignalCable", "KeyboardCable", "MouseCable" })
                foreach (var g in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(g => g.name == n).ToArray())
                    Object.DestroyImmediate(g);
            var socket = Find("OldWallSocket").transform.position;
            var towerB = BoundsOf(Find("OldPCTower"));
            var kb = BoundsOf(Find("computerKeyboard"));
            var ms = BoundsOf(Find("computerMouse"));
            ArtCable("MonitorSignalCable", new[]{ new Vector3(monitorB.center.x + .10f, monitorB.max.y - .05f, monitorB.max.z + .01f), new Vector3(monitorB.center.x + .10f, .06f, monitorB.max.z + .06f), new Vector3(socket.x, .06f, 1.80f), new Vector3(socket.x, socket.y, socket.z) }, .003f, black);
            ArtCable("KeyboardCable", new[]{ new Vector3(kb.center.x + .05f, kb.max.y, kb.max.z), new Vector3(kb.center.x - .10f, .40f, kb.max.z + .10f), new Vector3(towerB.center.x, .40f, towerB.max.z), new Vector3(towerB.center.x, .12f, towerB.max.z) }, .002f, black);
            ArtCable("MouseCable", new[]{ new Vector3(ms.center.x, ms.max.y, ms.max.z), new Vector3(towerB.center.x + .05f, .30f, towerB.center.z), new Vector3(towerB.center.x + .05f, .12f, towerB.max.z) }, .002f, black);
            Debug.Log("KOS_SEAT_FINAL done desk=" + db.center.ToString("F2") + " monitor=" + monitorB.center.ToString("F2"));
        }
        private static void CorrectFurniture()
        {
            var bed = Find("vintage_day_bed"); Ground(bed);
            var desk = Find("wooden_table_02");
            EnsurePrefabChild(desk, Art + "/Imported/PolyHaven/wooden_table_02/wooden_table_02.fbx", new Vector3(-1.25f, 1.87f));
            var deskPbr = KosVisualUpgrade.Pbr("wooden_table_02", "wooden_table_02");
            foreach (var r in desk.GetComponentsInChildren<Renderer>()) r.sharedMaterials = Enumerable.Repeat(deskPbr, r.sharedMaterials.Length).ToArray();
            ResetPrefabRoot(desk, 0);
            Resize(desk, new Vector3(1.30f, .75f, .65f));
            SetCenter(desk, new Vector3(-1.25f, .381f, 1.87f)); Ground(desk);
            // The desk rides the workstation assembly so Compact layout moves
            // top and legs together; world pose is preserved.
            desk.transform.SetParent(GameObject.Find("Golden_Workstation").transform, true);
            var chair = Find("painted_wooden_chair_01");
            EnsurePrefabChild(chair, Art + "/Imported/PolyHaven/painted_wooden_chair_01/painted_wooden_chair_01.fbx", new Vector3(-1.36f, 1.13f));
            ResetPrefabRoot(chair,180);
            Resize(chair,new Vector3(.40f,.86f,.46f));
            SetCenter(chair, new Vector3(-1.36f, .436f, 1.13f));
            chair.transform.RotateAround(BoundsOf(chair).center, Vector3.up, -7);
            Ground(chair);
            // Seat collision is refit post-Compact in OrientChairToDesk from
            // the FINAL chair spot; a pre-Compact fit rides stale.

            var wardrobe = SpawnMeshyWardrobe();
            FitBox(Find("LemariPakaian").GetComponent<BoxCollider>(), BoundsOf(wardrobe));
            var cupboard = Find("painted_wooden_cabinet_02");
            EnsurePrefabChild(cupboard, Art + "/Imported/PolyHaven/painted_wooden_cabinet_02/painted_wooden_cabinet_02.fbx", new Vector3(1.40f, 1.72f));
            Resize(cupboard, new Vector3(.45f, 1.20f, .30f));
            SetCenter(cupboard, new Vector3(1.40f, BoundsOf(cupboard).center.y, 1.72f));
            Ground(cupboard);
            FitBox(Find("CupboardCollision").GetComponent<BoxCollider>(), BoundsOf(cupboard));
            // MejaKerjaPC + legs are refit post-Compact in SeatFinalLayout
            // from the FINAL desk spot; pre-Compact absolutes strand them.
            // Fit the bed's solid base below the quilt so its phone remains raycast-accessible.
            var bedB = BoundsOf(bed);
            float mattressTop=SurfaceHeight(bed,-2.15f,-.85f)-.015f;
            FitBox(Find("KasurKos").GetComponent<BoxCollider>(), new Bounds(new Vector3(bedB.center.x, (FloorY+mattressTop)*.5f, bedB.center.z), new Vector3(bedB.size.x, mattressTop-FloorY, bedB.size.z)));

            var d = Object.FindFirstObjectByType<KamarKosPrologDirector>();
            var phone = d.smartphoneBedItem.transform.Find("Smartphone").gameObject;
            RestOn(phone, bed, -2.15f, -.85f);
            AlignItem(d.smartphoneBedItem, BoundsOf(phone));
            if (d.phoneBedRedLed) d.phoneBedRedLed.transform.position = BoundsOf(phone).center + Vector3.up * .018f;
            var jacket = d.jacketItem.transform.Find("HangingJacket").gameObject;
            SetCenter(jacket, new Vector3(2.16f, 1.38f, -2.13f));
            AlignItem(d.jacketItem, BoundsOf(jacket));
            Rail("JacketHook", new Vector3(2.16f, 1.72f, -2.23f), new Vector3(2.16f, 1.72f, -2.11f), .007f, Mat("DarkOxidisedMetal"));

            var drawer = d.lockedDrawerItem.transform.Find("cabinetBedDrawer").gameObject;
            SetCenter(drawer, new Vector3(-.90f, .60f, 1.69f));
            var lockGo = d.lockedDrawerItem.transform.Find("CombinationPadlock").gameObject;
            SetCenter(lockGo, new Vector3(-.90f, .59f, 1.528f));
            var drawerB = BoundsOf(drawer); drawerB.Encapsulate(BoundsOf(lockGo)); AlignItem(d.lockedDrawerItem, drawerB);
            var monitorB = BoundsOf(d.pcMonitorItem.transform.Find("computerScreen").gameObject);
            AlignItem(d.pcMonitorItem, monitorB);
            RestOn(Find("computerKeyboard"), desk, -1.32f, 1.70f);
            RestOn(Find("computerMouse"), desk, -.88f, 1.73f);
            RestOn(Find("binder_notebook"), desk, -1.68f, 1.70f);
            RestOn(Find("EnamelMug"), desk, -.70f, 2.04f);
            RestOn(Find("desk_lamp_arm_01"), desk, -1.74f, 2.00f);
            // Speakers ride under the Golden_Workstation assembly, so their world
            // spots drift from the desk; pin both back to authored desk spots
            // instead of trusting wherever the assembly left them.
            var speakers = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t => t.name == "speakerSmall").Select(t => t.gameObject).OrderBy(g => g.transform.position.x).ToArray();
            float[] speakerSpots = { -1.61f, -.80f };
            for (int i = 0; i < speakers.Length && i < speakerSpots.Length; i++)
                RestOn(speakers[i], desk, speakerSpots[i], 2.06f);
            var note = d.stickyNoteItem.transform.Find("StickyNote").gameObject;
            RestOn(note, desk, -.87f, 1.82f); AlignItem(d.stickyNoteItem, BoundsOf(note), .018f);
            foreach (var name in new[] { "OldPCTower", "RefillWaterGallon", "plastic_bottle_gallon", "trashcan", "cardboardBoxOpen" }) Ground(Find(name));
            SetCenter(Find("OldPCTower"),new Vector3(-1.49f,BoundsOf(Find("OldPCTower")).center.y,2.0f));
            foreach (var pair in new[] { ("WaterGallonCollision", "RefillWaterGallon") })
                FitBox(Find(pair.Item1).GetComponent<BoxCollider>(), BoundsOf(Find(pair.Item2)));
            // Books + shipping box are flattened and seated post-Compact in
            // SeatFinalLayout from the FINAL cupboard/shelf/box spots: the
            // pre-Compact loop pinballed nested same-named holders via
            // origin-based Resize, and the box collision rides stale.
            // A forgotten receipt beside the wastebasket; retain the original litter without a repeated diagonal row.
            Find("DiscardedPaper0").transform.position = new Vector3(-.30f, .012f, 1.98f);
            Find("DiscardedPaper1").transform.position = new Vector3(-.37f, .013f, 1.79f);
            Find("DiscardedPaper2").transform.position = new Vector3(-.56f, .012f, 1.91f);
        }

        private static void DressRoom()
        {
            var fabric = Pbr("CurtainCottonV2", "fabric_pattern_07", new Color(.73f, .70f, .58f));
            fabric.SetFloat("_Cull", 0);
            Spawn("Original/Models/KosCurtains.fbx", "KosCurtains", new Vector3(2.78f, 1.58f, -.55f), new Vector3(.08f, 1.44f, 1.30f), 90, fabric);
            var basket = Spawn("Imported/PolyHaven/wicker_basket_02/wicker_basket_02.fbx", "wicker_basket_02", new Vector3(2.57f, .24f, .35f), new Vector3(.40f, .46f, .40f), -12, Pbr("wicker_basket_02", "wicker_basket_02", Color.white));
            Ground(basket); AddSolid(basket);
            // The shipping box is seated post-Compact in SeatFinalLayout from
            // the FINAL layout spot; a pre-Compact absolute goes stale.
            Ground(Find("cardboardBoxOpen"));
            var container = Spawn("Imported/PolyHaven/plastic_container/plastic_container.fbx", "plastic_container", new Vector3(-2.36f, .17f, -2.03f), new Vector3(.48f, .29f, .30f), 0, Pbr("plastic_container", "plastic_container", Color.white));
            Ground(container); AddSolid(container);
            var sandals = Spawn("Original/Models/KosSandals.fbx", "KosSandals", new Vector3(.60f, .04f, -1.85f), new Vector3(.25f, .062f, .27f), -13, Plain("SandalRubberV2", new Color(.15f, .19f, .17f)));
            Ground(sandals);
            AssignSlots(sandals, name => name.Contains("Strap") ? Plain("SandalStrapV2", new Color(.28f, .34f, .30f)) : Mat("SandalRubberV2"));
            var bag = Spawn("Original/Models/KosBackpack.fbx", "KosBackpack", new Vector3(-2.65f, .23f, .19f), new Vector3(.31f, .44f, .25f), -22, Mat("FadedGreenJacket"));
            Ground(bag); AddSolid(bag);
            AssignSlots(bag, name => name.Contains("Metal") ? Mat("DarkOxidisedMetal") : Mat("FadedGreenJacket"));
            var fan = Spawn("Original/Models/KosDeskFan.fbx", "KosDeskFan", new Vector3(1.65f, 1.71f, 2.04f), new Vector3(.24f, .42f, .18f), -18, Plain("FanEnamelV2", new Color(.40f, .49f, .44f), .25f));
            // Use a shallow wall shelf beside the calendar; the cupboard top is already in use by books.
            var fanShelf = DetailBox("FanWallShelf", new Vector3(.06f, 1.31f, 2.02f), new Vector3(.50f, .035f, .34f), Mat("DoorAndWardrobeWood"));
            RestOn(fan, fanShelf, .06f, 2.02f);
            AssignSlots(fan, name => name.Contains("Metal") ? Mat("DarkOxidisedMetal") : Mat("FanEnamelV2"));
            Rail("FanShelfLeftBracket", new Vector3(-.10f, 1.14f, 2.23f), new Vector3(-.10f, 1.29f, 1.93f), .012f, Mat("DarkOxidisedMetal"));
            Rail("FanShelfRightBracket", new Vector3(.22f, 1.14f, 2.23f), new Vector3(.22f, 1.29f, 1.93f), .012f, Mat("DarkOxidisedMetal"));
            var router = Spawn("Original/Models/KosRouter.fbx", "KosRouter", new Vector3(1.66f, 1.65f, 2.06f), new Vector3(.17f, .16f, .105f), 0, Plain("RouterPlasticV2", new Color(.48f, .47f, .40f)));
            var cupTop2 = BoundsOf(Find("painted_wooden_cabinet_02"));
            RestOn(router, Find("painted_wooden_cabinet_02"), cupTop2.center.x + .01f, cupTop2.center.z - .01f);
            // Router sits beside, not through, the narrow stack of old books.
            SetCenter(router, new Vector3(cupTop2.center.x + .01f, BoundsOf(router).center.y, cupTop2.center.z - .07f));
            AssignSlots(router, name => name.Contains("Vent") ? Mat("OldComputerPlastic") : Mat("RouterPlasticV2"));
            var black = Mat("OldComputerPlastic");
            DetailBox("PowerStrip", new Vector3(-1.99f, .028f, 2.10f), new Vector3(.10f, .042f, .25f), Mat("AgedSwitchPlastic"));
            for (int i=0; i<3; i++)
            {
                DetailBox("PowerPlug", new Vector3(-1.99f, .064f, 2.025f+i*.074f), new Vector3(.038f,.04f,.045f), black);
                Rail("PlugCable", new Vector3(-1.99f,.076f,2.025f+i*.074f),new Vector3(-1.79f-i*.09f,.012f,2.20f),.004f,black);
            }
            Rail("MonitorSignalCable",new Vector3(-1.25f,.92f,2.01f),new Vector3(-1.31f,.03f,2.18f),.003f,black);
            Rail("KeyboardCable",new Vector3(-1.32f,.77f,1.78f),new Vector3(-1.34f,.77f,2.17f),.002f,black);
            Rail("MouseCable",new Vector3(-.88f,.78f,1.78f),new Vector3(-1.03f,.77f,2.16f),.002f,black);
            var routerBounds = BoundsOf(router);
            Rail("RouterPowerCable",new Vector3(routerBounds.center.x+.01f,routerBounds.max.y-.05f,routerBounds.max.z-.02f),new Vector3(1.67f,.33f,2.215f),.003f,black);
            DetailBox("RouterWallSocket",new Vector3(1.67f,.32f,2.225f),new Vector3(.10f,.12f,.026f),Mat("AgedSwitchPlastic"));
            DetailBox("RouterAdapter",new Vector3(1.67f,.32f,2.19f),new Vector3(.047f,.06f,.05f),black);
            DetailBox("BedsideSocket",new Vector3(-2.975f,.36f,-.20f),new Vector3(.026f,.12f,.10f),Mat("AgedSwitchPlastic"));
            DetailBox("PhoneCharger",new Vector3(-2.94f,.36f,-.20f),new Vector3(.05f,.05f,.04f),black);
            Cable("LoosePhoneChargeLead", new[] {new Vector3(-2.92f,.36f,-.20f),new Vector3(-2.89f,.015f,-.22f),new Vector3(-2.73f,.015f,-.02f),new Vector3(-2.58f,.016f,-.06f),new Vector3(-2.55f,.10f,-.13f)},.003f,black);
            // The fan's lead follows the wall to the existing socket, clear of all walking routes.
            Cable("FanLead",new[]{new Vector3(.06f,1.34f,2.10f),new Vector3(.06f,.28f,2.21f),new Vector3(-2.25f,.28f,2.21f),new Vector3(-2.25f,.42f,2.20f)},.003f,black);
        }

        private static void LightingAndMaterials()
        {
            var profile = Find("KosGlobalAtmosphere").GetComponent<Volume>().sharedProfile;
            foreach (var component in profile.components) component.active = component is Tonemapping || component is ColorAdjustments || component is Bloom || component is Vignette;
            profile.TryGet<Tonemapping>(out var tone); tone.mode.Override(TonemappingMode.ACES);
            profile.TryGet<ColorAdjustments>(out var color);
            color.postExposure.Override(.25f); color.contrast.Override(4); color.saturation.Override(-8); color.colorFilter.Override(Color.white);
            profile.TryGet<Bloom>(out var bloom); bloom.intensity.Override(.08f); bloom.threshold.Override(1.2f);
            profile.TryGet<Vignette>(out var vignette); vignette.intensity.Override(.16f); vignette.smoothness.Override(.5f);
            EditorUtility.SetDirty(profile);
            var ceiling = Find("WeakTungstenCeiling").GetComponent<Light>();
            ceiling.intensity = 16f; ceiling.range = 8; ceiling.spotAngle = 155;
            ceiling.innerSpotAngle = 95; ceiling.color = new Color(1f, .82f, .60f);
            ceiling.shadowResolution = LightShadowResolution.Medium; ceiling.shadowNormalBias = .05f;
            Find("BareWarmBulb").GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            var window = Find("ColdWindowSpill").GetComponent<Light>();
            // The V1 source was outside the opaque structural wall. Put it on the inner side of the glazing.
            window.transform.position = new Vector3(2.87f, 1.95f, -.55f);
            window.transform.LookAt(new Vector3(-1.8f, .25f, -.25f));
            window.intensity = 1.8f; window.color = new Color(.55f, .67f, .74f); window.shadows = LightShadows.None;
            var fill = Find("WarmWallBounce").GetComponent<Light>();
            fill.intensity = 4.5f; fill.color = new Color(.78f,.72f,.59f);
            var deskLamp = Find("DeskLampPool").GetComponent<Light>(); deskLamp.intensity = 1.5f; deskLamp.color = new Color(1,.78f,.48f);
            var probe = Find("KosReflectionProbe").GetComponent<ReflectionProbe>();
            probe.mode = ReflectionProbeMode.Baked; probe.enabled = false;
            RenderSettings.fog = false; RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
            RenderSettings.reflectionIntensity = .15f;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.16f,.17f,.16f);
            RenderSettings.ambientEquatorColor = new Color(.11f,.12f,.11f);
            RenderSettings.ambientGroundColor = new Color(.06f,.055f,.045f);
            Tune("OldLimewash", new Color(.82f,.80f,.71f), .20f, .55f);
            Tune("PeelingSeaGreenPaint",new Color(.48f,.58f,.47f),.22f,.7f);
            Tune("CeramicTiles",new Color(.85f,.83f,.75f),.35f,.7f);
            Tune("DoorAndWardrobeWood",new Color(.78f,.71f,.59f),.28f,.7f);
            var renderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(Art+"/Generated/KosRenderer.asset");
            foreach (var feature in renderer.rendererFeatures)
            {
                var so = new SerializedObject(feature);
                var downsample = so.FindProperty("m_Settings.Downsample"); if(downsample!=null) downsample.boolValue=true;
                var intensity = so.FindProperty("m_Settings.Intensity"); if(intensity!=null) intensity.floatValue=.75f;
                var radius = so.FindProperty("m_Settings.Radius"); if(radius!=null) radius.floatValue=.14f;
                so.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(feature);
            }
            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(Art+"/Generated/KosPipeline.asset");
            var p = new SerializedObject(pipeline);
            // Serialized enum: Disabled=0, PerPixel=1, PerVertex=2 in this installed URP version.
            p.FindProperty("m_AdditionalLightsRenderingMode").intValue = (int)LightRenderingMode.PerPixel;
            p.FindProperty("m_AdditionalLightsPerObjectLimit").intValue = 4;
            p.FindProperty("m_MSAA").intValue = 1;
            p.FindProperty("m_ShadowDistance").floatValue = 10;
            p.FindProperty("m_AdditionalLightsShadowmapResolution").intValue = 1024;
            p.FindProperty("m_MainLightShadowmapResolution").intValue = 1024;
            p.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(pipeline);
        }

        private static void Tune(string name, Color tint, float smooth, float bump)
        {
            var mat=Mat(name); mat.SetColor("_BaseColor",tint); mat.SetFloat("_Smoothness",smooth);
            mat.SetFloat("_BumpScale",bump); EditorUtility.SetDirty(mat);
        }
        private static Material Plain(string name, Color color, float metallic=0)
        {
            var mat=Mat(name);
            if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,Mats+name+".mat");}
            mat.SetColor("_BaseColor",color);mat.SetFloat("_Smoothness",.22f);mat.SetFloat("_Metallic",metallic);mat.enableInstancing=true;EditorUtility.SetDirty(mat);return mat;
        }
        private static Material Pbr(string name,string asset,Color tint)
        {
            var mat=Plain(name,tint);string dir=Art+"/Imported/PolyHaven/"+asset;
            mat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Directory.GetFiles(dir,"*_diff_*jpg").Single()));
            mat.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Directory.GetFiles(dir,"*_nor_gl_*jpg").Single()));
            var mask=AssetDatabase.LoadAssetAtPath<Texture2D>(dir+"/"+asset+"_mask.png");
            mat.SetTexture("_MetallicGlossMap",mask);mat.SetTexture("_OcclusionMap",mask);mat.SetFloat("_OcclusionStrength",.8f);
            mat.EnableKeyword("_NORMALMAP");mat.EnableKeyword("_METALLICSPECGLOSSMAP");mat.EnableKeyword("_OCCLUSIONMAP");return mat;
        }
        private static GameObject Spawn(string path,string name,Vector3 center,Vector3 size,float yaw,Material mat)
        {
            var go=new GameObject(name);go.transform.SetParent(details,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Art+"/"+path));
            model.transform.SetParent(go.transform,false);go.transform.rotation=Quaternion.Euler(0,yaw,0);
            foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(mat,r.sharedMaterials.Length).ToArray();
            Resize(go,size);SetCenter(go,center);return go;
        }
        private static void AssignSlots(GameObject go,Func<string,Material> material)
        {foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(material(r.name),r.sharedMaterials.Length).ToArray();}
        private static GameObject DetailBox(string name,Vector3 center,Vector3 size,Material mat)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(details,false);go.transform.position=center;go.transform.localScale=size;go.GetComponent<Renderer>().sharedMaterial=mat;return go;
        }
        private static void Rail(string name,Vector3 a,Vector3 b,float radius,Material mat)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cylinder);go.name=name;Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(details,false);go.transform.position=(a+b)*.5f;go.transform.up=(b-a).normalized;
            go.transform.localScale=new Vector3(radius*2,Vector3.Distance(a,b)*.5f,radius*2);go.GetComponent<Renderer>().sharedMaterial=mat;
        }
        private static void Cable(string name,Vector3[] points,float width,Material material)
        {
            var go=new GameObject(name,typeof(LineRenderer));go.transform.SetParent(details,false);var line=go.GetComponent<LineRenderer>();
            line.positionCount=points.Length;line.SetPositions(points);line.widthMultiplier=width;line.numCornerVertices=2;line.numCapVertices=2;line.sharedMaterial=material;line.generateLightingData=true;
        }
        public static void SetCenter(GameObject go,Vector3 center) => go.transform.position+=center-BoundsOf(go).center;
        // Rotation-agnostic world fit: routes each world-axis target through
        // the holder-local axis that feeds it (exact for 90-degree turns, e.g.
        // a yawed holder with a pitched FBX root). Plain Resize assumes an
        // identity holder, which mis-fits those.
        public static void FitWorldSize(GameObject go,Vector3 size)
        {
            for (int iter = 0; iter < 3; iter++)
            {
                var b = BoundsOf(go);
                if (Mathf.Abs(b.size.x - size.x) < .005f && Mathf.Abs(b.size.y - size.y) < .005f && Mathf.Abs(b.size.z - size.z) < .005f) break;
                var m = Matrix4x4.Rotate(go.transform.rotation);
                var s = go.transform.localScale;
                var ns = s;
                bool[] used = new bool[3];
                for (int w = 0; w < 3; w++)
                {
                    int l = 0; float best = -1;
                    for (int ll = 0; ll < 3; ll++) { float a = Mathf.Abs(m[w, ll]); if (!used[ll] && a > best) { best = a; l = ll; } }
                    used[l] = true;
                    float worldSize = w == 0 ? b.size.x : w == 1 ? b.size.y : b.size.z;
                    float target = w == 0 ? size.x : w == 1 ? size.y : size.z;
                    if (worldSize < 1e-6f) continue;
                    float cur = l == 0 ? s.x : l == 1 ? s.y : s.z;
                    float nv = cur * target / worldSize;
                    if (l == 0) ns.x = nv; else if (l == 1) ns.y = nv; else ns.z = nv;
                }
                go.transform.localScale = ns;
            }
        }
        public static void Resize(GameObject go,Vector3 size)
        {
            var b=BoundsOf(go);var s=go.transform.localScale;
            // Objects are yaw-aligned before fitting; world X/Z swap for 90-degree rotations.
            float yaw=Mathf.Abs(Mathf.Sin(go.transform.eulerAngles.y*Mathf.Deg2Rad));
            var factor=new Vector3(size.x/b.size.x,size.y/b.size.y,size.z/b.size.z);
            if(yaw>.99f){float x=factor.x;factor.x=factor.z;factor.z=x;}
            go.transform.localScale=Vector3.Scale(s,factor);
        }
        public static float FloorY => Find("CeramicTileFloor").GetComponent<Renderer>().bounds.max.y;
        private static void Ground(GameObject go) => go.transform.position+=Vector3.up*(FloorY-BoundsOf(go).min.y+.001f);
        public static void FitBox(BoxCollider collider,Bounds bounds)
        {
            collider.center=collider.transform.InverseTransformPoint(bounds.center);
            var s=collider.transform.lossyScale;
            collider.size=new Vector3(bounds.size.x/Mathf.Abs(s.x),bounds.size.y/Mathf.Abs(s.y),bounds.size.z/Mathf.Abs(s.z));
        }
        private static void AlignItem(KosInteractableItem item,Bounds bounds,float minThickness=.025f)
        {
            // Keep the visible descendants fixed while centering the existing interaction root on its geometry.
            Vector3 delta=bounds.center-item.transform.position;
            item.transform.position+=delta;foreach(Transform child in item.transform)child.position-=delta;
            bounds.size=new Vector3(Mathf.Max(bounds.size.x,minThickness),Mathf.Max(bounds.size.y,minThickness),Mathf.Max(bounds.size.z,minThickness));
            FitBox(item.GetComponent<BoxCollider>(),bounds);
        }
        private static void RestOn(GameObject prop,GameObject support,float x,float z)
        {
            float y=SurfaceHeight(support,x,z);
            var b=BoundsOf(prop);SetCenter(prop,new Vector3(x,y+b.size.y*.5f+.001f,z));
        }
        public static Vector3[] MeshVertices(GameObject go) => go.GetComponentsInChildren<MeshFilter>()
            .Where(f=>f.sharedMesh && f.GetComponent<Renderer>().enabled)
            .SelectMany(f=>f.sharedMesh.vertices.Select(v=>f.transform.TransformPoint(v))).ToArray();
        public static float SurfaceHeight(GameObject go,float x,float z)
        {
            // Intersect actual transformed mesh triangles with a vertical ray; bounds alone do not locate a quilt/shelf.
            float top=float.NegativeInfinity;
            foreach(var f in go.GetComponentsInChildren<MeshFilter>())
            {
                if(!f.sharedMesh || !f.GetComponent<Renderer>().enabled)continue;
                var vertices=f.sharedMesh.vertices;var indices=f.sharedMesh.triangles;
                for(int i=0;i<indices.Length;i+=3)
                {
                    var a=f.transform.TransformPoint(vertices[indices[i]]);var b=f.transform.TransformPoint(vertices[indices[i+1]]);var c=f.transform.TransformPoint(vertices[indices[i+2]]);
                    float det=(b.z-c.z)*(a.x-c.x)+(c.x-b.x)*(a.z-c.z);if(Mathf.Abs(det)<.0000001f)continue;
                    float u=((b.z-c.z)*(x-c.x)+(c.x-b.x)*(z-c.z))/det;
                    float v=((c.z-a.z)*(x-c.x)+(a.x-c.x)*(z-c.z))/det;
                    if(u<-.0001f || v<-.0001f || u+v>1.0001f)continue;
                    top=Mathf.Max(top,u*a.y+v*b.y+(1-u-v)*c.y);
                }
            }
            if(float.IsNegativeInfinity(top))throw new Exception("No mesh surface under prop at "+go.name+" "+x+","+z);
            return top;
        }
        private static void AddSolid(GameObject go) {var collider=go.AddComponent<BoxCollider>();FitBox(collider,BoundsOf(go));}
    }
}
#endif
