#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Incremental production pass: never regenerate the approved Kos/ground-floor composition.
    public static class PrologCompletionBuilder
    {
        private static AtmajaDirector d;
        private static Transform root;
        private const string Materials = "Assets/_LayarTerakhir/Art/Atmaja/Materials/";
        private static Vector3 P(float x, float z, float y) => d.P(x, z, y);
        private static GameObject Find(string name) => UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<Transform>(true)).FirstOrDefault(t => t.name == name)?.gameObject;
        private static T Component<T>() where T : Component => d.GetComponent<T>() ? d.GetComponent<T>() : d.gameObject.AddComponent<T>();
        private static string Hash(string path) { using var sha = SHA256.Create(); return BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(path))).Replace("-", "").ToLowerInvariant(); }
        [MenuItem("LayarTerakhir/Prolog/Complete Atmaja finale (incremental)")]
        public static void Apply()
        {
            string kosBefore = Hash(PrologKosSceneBuilder.ScenePath), corridorBefore = Hash(KosCorridorBuilder.ScenePath);
            var scene = EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            d = Object.FindFirstObjectByType<AtmajaDirector>();
            var old = Find("PrologCompletionArt"); if (old) Object.DestroyImmediate(old);
            root = new GameObject("PrologCompletionArt").transform; root.SetParent(Find("Atmaja_Environment_Art").transform, false);
            d.chase.recording = Component<AtmajaHandycamPlayback>(); d.chase.finale = Component<PrologFinaleView>();
            var atmosphere = Component<AtmajaEvidenceAtmosphere>(); atmosphere.director = d;
            UpperPresentation(atmosphere); CellarLayout(atmosphere); ChaseLayout(); RecordingSet(); ThreatPresentation();
            var eye = d.player.GetComponent<InteractionInteractor>().playerCamera;
            eye.cullingMask &= ~((1 << AtmajaMirrorReflection.ReflectionLayer) | (1 << AtmajaHandycamPlayback.RecordingLayer));
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene); AssetDatabase.SaveAssets();
            if (Hash(PrologKosSceneBuilder.ScenePath) != kosBefore || Hash(KosCorridorBuilder.ScenePath) != corridorBefore) throw new Exception("Approved Kos scenes were changed by continuation pass");
            File.WriteAllText("Audit/prolog-completion-authoring.json", JsonUtility.ToJson(new Authoring {
                utc = DateTime.UtcNow.ToString("o"), kosSha256 = kosBefore, corridorSha256 = corridorBefore,
                atmajaSha256 = Hash(AtmajaSceneBuilder.ScenePath), recordingHands = d.chase.recording.reachingHands.Length,
                patrolPoints = d.basement.waypoints.Length, chasePoints = d.chase.rail.Length, approvedScenesPreserved = true
            }, true) + "\n");
            Debug.Log("PROLOG_COMPLETION authored upper evidence, cellar stealth, original handycam recording, escape route and ending");
        }
        [Serializable] private class Authoring { public string utc, kosSha256, corridorSha256, atmajaSha256; public int recordingHands, patrolPoints, chasePoints; public bool approvedScenesPreserved; }
        private static Material Mat(string name, Color color, bool unlit = false)
        {
            string path = Materials + name + ".mat";
            var m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!m) { m = new Material(Shader.Find(unlit ? "Universal Render Pipeline/Unlit" : "Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m, path); }
            if(m.HasProperty("_BaseColor"))m.SetColor("_BaseColor", color);else if(m.HasProperty("_Color"))m.SetColor("_Color",color);
            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", .18f); EditorUtility.SetDirty(m); return m;
        }
        private static Material Existing(string name) => AssetDatabase.LoadAssetAtPath<Material>(Materials + name + ".mat");
        private static GameObject Part(string name, Transform parent, Vector3 at, Vector3 size, Material material, PrimitiveType primitive = PrimitiveType.Cube, bool solid = false)
        {
            var g = GameObject.CreatePrimitive(primitive); g.name = name; g.transform.SetParent(parent, false); g.transform.localPosition = at; g.transform.localScale = size;
            if(primitive==PrimitiveType.Sphere||primitive==PrimitiveType.Capsule)g.GetComponent<MeshFilter>().sharedMesh=RoundMesh();
            g.GetComponent<Renderer>().sharedMaterial = material;
            if (!solid) Object.DestroyImmediate(g.GetComponent<Collider>()); return g;
        }
        private static Mesh RoundMesh()
        {
            const string path="Assets/_LayarTerakhir/Art/Atmaja/Models/CompletionRound.asset";
            var prior=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(prior)return prior;
            const int sides=10,rings=7;var vertices=new List<Vector3>();var uv=new List<Vector2>();var faces=new List<int>();
            for(int ring=0;ring<=rings;ring++)for(int side=0;side<=sides;side++)
            {float a=side/(float)sides*Mathf.PI*2,b=ring/(float)rings*Mathf.PI;vertices.Add(new Vector3(Mathf.Sin(b)*Mathf.Cos(a),Mathf.Cos(b),Mathf.Sin(b)*Mathf.Sin(a))*.5f);uv.Add(new Vector2(side/(float)sides,ring/(float)rings));}
            for(int ring=0;ring<rings;ring++)for(int side=0;side<sides;side++)
            {int a=ring*(sides+1)+side,b=a+sides+1;faces.AddRange(new[]{a,b,a+1,a+1,b,b+1});}
            var mesh=new Mesh{name="CompletionRound140Triangles"};mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(faces,0);mesh.RecalculateNormals();mesh.RecalculateBounds();AssetDatabase.CreateAsset(mesh,path);return mesh;
        }
        private static Transform Anchor(string name, Vector3 position, Vector3 forward)
        {
            var t = new GameObject(name).transform; t.SetParent(root, false); t.SetPositionAndRotation(position, Quaternion.LookRotation(forward)); return t;
        }
        private static void Move(GameObject go, Vector3 center)
        { if (go) go.transform.position += center - KosVisualV2.BoundsOf(go).center; }
        private static void Refit(GameObject go)
        {foreach(var collider in go.GetComponents<BoxCollider>())Object.DestroyImmediate(collider);AtmajaSceneBuilder.FitCollider(go);}
        private static GameObject Model(string asset, string name, Vector3 center, Vector3 size, Material material)
        {
            var g = new GameObject(name); g.transform.SetParent(root, false);
            string path = "Assets/_LayarTerakhir/Art/Original/Models/" + asset + ".fbx";
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path)); instance.transform.SetParent(g.transform, false);
            foreach (var r in instance.GetComponentsInChildren<Renderer>()) r.sharedMaterials = Enumerable.Repeat(material, r.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(g, size); KosVisualV2.SetCenter(g, center); return g;
        }
        private static void Layer(GameObject g, int layer) { foreach (var t in g.GetComponentsInChildren<Transform>(true)) t.gameObject.layer = layer; }
        private static void UpperPresentation(AtmajaEvidenceAtmosphere atmosphere)
        {
            var u = d.upper;
            var wood = Existing("House_MAT_WoodDoor"); var rust = Existing("Upper_SteelRust");
            var red = Mat("CompletionEvidenceInk", new Color(.38f, .018f, .009f), true);
            const string photoPath="Assets/_LayarTerakhir/Art/Atmaja/Textures/AtmajaFamilyScratched-v1.png";
            var importer=(TextureImporter)AssetImporter.GetAtPath(photoPath);importer.textureType=TextureImporterType.Default;importer.sRGBTexture=true;importer.maxTextureSize=1024;importer.mipmapEnabled=true;importer.anisoLevel=2;importer.textureCompression=TextureImporterCompression.Compressed;importer.SaveAndReimport();
            var photograph=Mat("CompletionAtmajaFamilyPhoto",new Color(.60f,.58f,.52f));photograph.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(photoPath));EditorUtility.SetDirty(photograph);
            for(int i=0;i<3;i++)
            {
                float x=i==0?-2.05f:i==1?.1f:2.12f;
                Move(Find("PortraitDosa"+(i+1)),P(x,5.325f,5.35f));
                var image=Part("ScratchedFamilyPhotograph"+(i+1),root,Vector3.zero,new Vector3(i==2?.38f:.49f,i==2?.50f:.60f,1),photograph,PrimitiveType.Quad);image.transform.SetPositionAndRotation(P(x,5.365f,5.35f),Quaternion.LookRotation(-d.north));
                var backing=Find("PortraitDosa"+(i+1)+"_PlaqueBack");if(backing)backing.transform.position=P(x,5.295f,i==2?4.88f:4.82f);
                for(int gouge=0;gouge<3;gouge++){var oldGouge=Find("PortraitDosa"+(i+1)+"_Gouge"+gouge);if(oldGouge)Object.DestroyImmediate(oldGouge);}
                var plaque=Find("PortraitDosa"+(i+1)+"_Plaque");if(plaque)
                {
                    plaque.transform.position=P(x,5.31f,i==2?4.88f:4.82f);
                    var bounds=plaque.GetComponent<Renderer>().bounds;var text=plaque.GetComponent<TextMesh>();
                    if(bounds.size.x>.001f&&bounds.size.y>.001f)text.characterSize*=Mathf.Min((i==2?.38f:.47f)/bounds.size.x,.14f/bounds.size.y);
                }
                var ar=u.portraitBloodAR[i];if(ar)
                {
                    ar.transform.position=P(x,5.375f,4.42f);var bounds=ar.GetComponent<Renderer>().bounds;var text=ar.GetComponent<TextMesh>();
                    if(bounds.size.x>.001f&&bounds.size.y>.001f)text.characterSize*=Mathf.Min(.55f/bounds.size.x,.30f/bounds.size.y);
                    foreach(var child in ar.GetComponentsInChildren<Transform>(true))if(child.name.Contains("ClosedEye")){var pos=child.localPosition;pos.y=.23f;child.localPosition=pos;}
                }
            }
            var worldText=Component<AtmajaWorldText>();var fontMaterial=Mat("CompletionWorldText",Color.white);
            fontMaterial.shader=Shader.Find("LayarTerakhir/WorldText");fontMaterial.shaderKeywords=Array.Empty<string>();fontMaterial.SetColor("_Color",Color.white);EditorUtility.SetDirty(fontMaterial);worldText.materialTemplate=fontMaterial;
            // Replace explanatory world text with actual scratches and a broken vent outline.
            if (u.ventHoleAR)
            {
                var text = u.ventHoleAR.GetComponent<TextMesh>(); if (text) Object.DestroyImmediate(text);
                var renderer = u.ventHoleAR.GetComponent<MeshRenderer>(); if (renderer) Object.DestroyImmediate(renderer);
                for (int i = 0; i < 5; i++)
                { var strip = Part("VentBrokenLath", root, Vector3.zero, new Vector3(.045f, .14f, .055f), wood); strip.transform.position = P(-3.35f + i * .17f, 5.2f, 4.93f); strip.transform.rotation = Quaternion.LookRotation(d.north) * Quaternion.Euler(0, 0, i * 13 - 24); }
            }
            var scratch = u.bathScratchesAR;
            if (scratch)
            {
                var text = scratch.GetComponent<TextMesh>(); if (text) Object.DestroyImmediate(text);
                var renderer = scratch.GetComponent<MeshRenderer>(); if (renderer) Object.DestroyImmediate(renderer);
                scratch.transform.SetParent(u.bathDoor.transform, true);
                for (int i = scratch.transform.childCount - 1; i >= 0; i--) Object.DestroyImmediate(scratch.transform.GetChild(i).gameObject);
                for (int hand = 0; hand < 5; hand++) for (int finger = 0; finger < 4; finger++)
                {
                    float x = -.27f + hand * .115f + finger * .025f;
                    var mark = Part("ChildNailGouge", scratch.transform, new Vector3(x, -.27f + hand % 3 * .17f, 0), new Vector3(.007f, .12f + finger * .025f, .004f), red);
                    mark.transform.localRotation = Quaternion.Euler(0, 0, -18 + hand * 8);
                }
                Layer(scratch, AtmajaPhoneView.ARLayer);
                u.scratchAim.SetParent(u.bathDoor.transform, true);
            }
            var target = Find("BathScratchTarget"); if (target) { var collider = target.GetComponent<Collider>(); if (collider) collider.enabled = false; }
            if (u.bathCurtain)
            {
                Move(u.bathCurtain, P(-1.1f, 1.6f, 5.05f)); u.bathCurtain.transform.localScale = new Vector3(1.8f, 1.9f, .045f);
                var rod = Part("BathCurtainRod", root, Vector3.zero, new Vector3(1.95f, .035f, .035f), rust); rod.transform.position = P(-1.1f, 1.6f, 6.05f);
                for (int i = 0; i < 9; i++)
                {
                    var fold = Part("BathCurtainFold", root, Vector3.zero, new Vector3(.06f, 1.85f, .045f), Existing("Upper_CurtainDirty"));
                    fold.transform.position = P(-1.9f + i * .2f, 1.57f, 5.05f); fold.transform.SetParent(u.bathCurtain.transform, true);
                }
            }
            if (u.bathWater) { Move(u.bathWater, P(-1.1f, .8f, 4.15f)); u.bathWater.transform.localScale = new Vector3(1.34f, .025f, .56f); }
            var bed=Find("MasterBed");if(bed){KosVisualV2.Resize(bed,new Vector3(1.0f,.65f,1.9f));Move(bed,P(1.75f,3.1f,4.15f));Refit(bed);}
            var chandelier=Find("VoidChandelier");if(chandelier){KosVisualV2.Resize(chandelier,new Vector3(.8f,1.0f,.8f));Move(chandelier,P(3.7f,4.9f,6.35f));Refit(chandelier);}
            var chandelierLight=Find("ChandelierLight");if(chandelierLight)chandelierLight.transform.position=P(3.7f,4.9f,6.1f);
            var mattress=Find("MasterMattress");if(mattress){mattress.transform.localScale=new Vector3(.92f,.18f,1.8f);mattress.transform.position=P(1.75f,3.1f,4.3f);}
            Move(Find("MasterPillow"),P(1.75f,2.4f,4.46f));Move(Find("MasterNightstand"),P(2.05f,1.8f,4.13f));Move(Find("MasterAlarmClock"),P(2.05f,1.8f,4.53f));
            Move(Find("MasterRocker"),P(2.0f,1.1f,4.32f));Move(Find("MasterDresser"),P(2.05f,.35f,4.45f));Move(Find("MasterCandle"),P(2.05f,.35f,4.85f));
            // Open into the rooms. The old leaves spanned the entire 1.1m corridor,
            // making the return chase impassable after exploring the bathroom/master.
            u.masterDoor.hinge.SetPositionAndRotation(P(1.85f,5.2f,3.82f),Quaternion.identity);
            u.masterDoor.transform.SetPositionAndRotation(P(1.3f,5.2f,4.92f),Quaternion.LookRotation(d.north));u.masterDoor.angle=-95;
            u.bathDoor.hinge.SetPositionAndRotation(P(-.65f,5.2f,3.82f),Quaternion.identity);
            u.bathDoor.transform.SetPositionAndRotation(P(-1.1f,5.2f,4.82f),Quaternion.LookRotation(d.north));u.bathDoor.angle=-95;
            u.childDoor.angle=95;
            Move(Find("BathSink"),P(-.05f,3.5f,4.35f));Move(Find("BathMirror"),P(.1f,3.5f,5.0f));
            // A shaped enamel cup replaces the old solid cube while retaining its interaction root.
            if (u.childCup)
            {
                u.childCup.GetComponent<Renderer>().enabled = false;
                var oldCup=Find("BimaMilkCupMesh");if(oldCup)Object.DestroyImmediate(oldCup);
                var cup = Model("EnamelMug", "BimaMilkCupMesh", P(-5, .62f, 4.66f), new Vector3(.09f, .09f, .09f), Existing("ChildCupWhite")); cup.transform.SetParent(u.childCup.transform, true);
                var glow = new GameObject("MilkCupARAura", typeof(Light)); glow.transform.SetParent(root, false); glow.transform.position = u.cupAim.position + Vector3.up * .1f;
                var light = glow.GetComponent<Light>(); light.type = LightType.Point; light.range = .7f; light.intensity = .25f; light.color = new Color(1, .05f, .02f); light.shadows = LightShadows.None; atmosphere.cupAura = light;
            }
            var reflection = Component<AtmajaMirrorReflection>(); reflection.director = d;
            var plane = Part("BathroomMirrorSurface", root, Vector3.zero, new Vector3(.43f, .64f, 1), Mat("CompletionMirrorGlass", new Color(.55f, .60f, .58f), true), PrimitiveType.Quad);
            plane.transform.SetPositionAndRotation(P(.035f, 3.5f, 5.0f), Quaternion.LookRotation(-d.east));
            reflection.mirrorPlane = plane.transform; reflection.surface = plane.GetComponent<Renderer>(); reflection.reflectionMaterial = plane.GetComponent<Renderer>().sharedMaterial;
            u.mirrorSpot.position=plane.transform.position;
            if (u.mirrorFlashFigure) { Layer(u.mirrorFlashFigure, AtmajaMirrorReflection.ReflectionLayer); Move(u.mirrorFlashFigure, P(-1.65f, 3.25f, 4.7f)); }
            for (int i = 0; i < 3; i++)
            {
                var go = new GameObject("SealBlackSmoke" + i, typeof(ParticleSystem)); go.transform.SetParent(root, false); go.transform.position = u.chainLinks[i].transform.position;
                var smoke = go.GetComponent<ParticleSystem>(); smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var main = smoke.main; main.loop = true; main.startLifetime = 1.8f; main.startSpeed = .06f; main.startSize = new ParticleSystem.MinMaxCurve(.10f, .22f);
                main.startColor = new Color(.045f, .04f, .055f, .42f); main.maxParticles = 16; main.simulationSpace = ParticleSystemSimulationSpace.World; main.gravityModifier = -.015f;
                var emit = smoke.emission; emit.rateOverTime = 5;
                var shape = smoke.shape; shape.shapeType = ParticleSystemShapeType.Box; shape.scale = new Vector3(.75f, .04f, .08f);
                var r = go.GetComponent<ParticleSystemRenderer>(); r.sharedMaterial = SmokeMaterial(); r.shadowCastingMode = ShadowCastingMode.Off; r.receiveShadows = false;
                atmosphere.sealSmoke[i] = smoke;
            }
        }
        private static Material SmokeMaterial()
        {
            const string path=Materials+"CompletionSealSmoke.mat";
            var mat=AssetDatabase.LoadAssetAtPath<Material>(path);if(mat)return mat;
            var tex=new Texture2D(32,32,TextureFormat.RGBA32,false){name="SoftSealSmoke",wrapMode=TextureWrapMode.Clamp};var pixels=new Color[32*32];
            for(int y=0;y<32;y++)for(int x=0;x<32;x++){float distance=new Vector2((x-15.5f)/15.5f,(y-15.5f)/15.5f).magnitude;pixels[y*32+x]=new Color(1,1,1,Mathf.Pow(Mathf.Clamp01(1-distance),2));}
            tex.SetPixels(pixels);tex.Apply();AssetDatabase.CreateAsset(tex,"Assets/_LayarTerakhir/Art/Atmaja/Textures/CompletionSmoke.asset");
            mat=new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));mat.SetTexture("_BaseMap",tex);mat.SetColor("_BaseColor",Color.white);
            mat.SetFloat("_Surface",1);mat.SetFloat("_Blend",0);mat.SetInt("_SrcBlend",(int)BlendMode.SrcAlpha);mat.SetInt("_DstBlend",(int)BlendMode.OneMinusSrcAlpha);mat.SetInt("_ZWrite",0);mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");mat.renderQueue=3000;
            AssetDatabase.CreateAsset(mat,path);return mat;
        }
        private static void CellarLayout(AtmajaEvidenceAtmosphere atmosphere)
        {
            var b = d.basement;
            // Remove impossible 80cm risers from the cellar floor. The steep connecting shaft
            // is a game-owned traverse; the U-shaped stealth room is continuously walkable.
            foreach (var step in root.parent.GetComponentsInChildren<Transform>(true).Where(t => t.name == "CellarStep").ToArray()) Object.DestroyImmediate(step.gameObject);
            b.spawnBasement = Anchor("CompletionCellarSpawn", P(1.35f, 2.55f, -2.79f), d.north);
            Move(b.handycamObj,P(1.05f,5.45f,-2.7f));
            // A palm-sized pickup must not trip the crouched capsule during stealth.
            var handycamCollider=b.handycamObj.GetComponent<Collider>();if(handycamCollider)handycamCollider.isTrigger=true;
            var ward = Find("CellarWardrobe1"); if (ward) { ward.transform.rotation = Quaternion.LookRotation(d.east); Move(ward, P(.65f, 2.8f, -1.85f)); }
            b.hideAnchor1 = Anchor("CompletionHide1", P(.65f, 2.8f, -2.79f), d.east);
            b.exitAnchor1 = Anchor("CompletionHideExit1", P(1.42f, 2.8f, -2.79f), d.north);
            b.hideAnchor2 = Anchor("CompletionHide2", P(4.95f, 1.05f, -2.79f), -d.east);
            b.exitAnchor2 = Anchor("CompletionHideExit2", P(4.05f, 1.05f, -2.79f), d.north);
            var patrol = new List<Transform>();
            foreach (var point in new[] { new Vector2(4.3f, 3.9f), new Vector2(4.3f, 2.65f), new Vector2(4.0f, 1.45f), new Vector2(2.8f, 1.4f), new Vector2(1.4f, 1.4f), new Vector2(1.4f, 2.8f), new Vector2(1.4f, 4.3f), new Vector2(1.4f, 5.45f), new Vector2(2.1f, 5.6f), new Vector2(3.8f, 5.6f), new Vector2(4.3f, 5.6f) })
                patrol.Add(Anchor("CompletionPatrol", P(point.x, point.y, -2.79f), d.north));
            b.waypoints = patrol.ToArray(); b.inspectAnchor1 = patrol[5]; b.inspectAnchor2 = patrol[2];
            // Camera-local slats make hiding readable without adding a second interface.
            var slats = new GameObject("WardrobeInteriorSlats"); slats.transform.SetParent(d.player.cameraPivot, false);
            var old = d.player.cameraPivot.Find("WardrobeInteriorSlats"); if (old && old != slats.transform) Object.DestroyImmediate(old.gameObject);
            slats.transform.localPosition = new Vector3(0, 0, .30f);
            var dark = Mat("CompletionWardrobeDark", new Color(.018f, .011f, .007f));
            for (int i = 0; i < 7; i++) Part("InteriorWoodLouver", slats.transform, new Vector3(0, (i - 3) * .082f, .02f), new Vector3(.65f, .056f, .015f), dark);
            foreach (int side in new[] { -1, 1 }) Part("InteriorWoodStile", slats.transform, new Vector3(side * .28f, 0, 0), new Vector3(.12f, .65f, .05f), dark);
            slats.SetActive(false); atmosphere.wardrobeSlats = slats;
        }
        private static void ChaseLayout()
        {
            var c = d.chase;
            c.cellarExitApproach = Anchor("CompletionCellarExit", P(1.2f, 1.7f, -2.79f), d.north);
            c.slideExit = Anchor("CompletionSlideExit", P(1.0f, 2.7f, 3.83f), d.north);
            c.slideGate.transform.position = P(1.0f, 1.7f, 4.98f); c.slideGate.transform.localScale = new Vector3(.9f, .10f, 1.05f);
            c.slideGate.transform.rotation=Quaternion.LookRotation(d.north)*Quaternion.Euler(8,0,0);
            c.slideGate.GetComponent<Renderer>().enabled = true; c.slideGate.SetActive(false);
            c.cellarEgressFloor=Part("CellarClosingStairLanding",root,Vector3.zero,new Vector3(.9f,.12f,1.9f),Existing("House_MAT_WoodDoor"),PrimitiveType.Cube,true);
            c.cellarEgressFloor.transform.position=P(1.0f,1.4f,3.75f);c.cellarEgressFloor.SetActive(false);
            c.spawnChase = Anchor("CompletionChaseSpawn", P(1.5f, 5.3f, -2.79f), -d.north);
            c.diningVaultStart = Anchor("CompletionTableApproach", P(5.15f, 6.5f, .54f), d.north);
            c.diningVaultEnd = Anchor("CompletionTableLanding", P(5.15f, 8.0f, .54f), d.north);
            c.diningBlocker.transform.position = P(5.1f, 7.25f, 1.03f); c.diningBlocker.transform.localScale = new Vector3(1.75f, .98f, .5f); c.diningBlocker.SetActive(false);
            var furniture = new List<GameObject>();
            foreach (var item in root.parent.GetComponentsInChildren<Transform>(true).Where(t => t.name == "FallenVaultTable" || t.name == "FallenVaultChair").ToArray())
            {
                if (item.name == "FallenVaultTable") { KosVisualV2.Resize(item.gameObject, new Vector3(1.65f, .90f, .50f)); Move(item.gameObject, P(5.1f, 7.25f, 1.0f)); }
                else { Move(item.gameObject, P(4.2f, 7.15f + furniture.Count * .25f, .8f)); foreach (var col in item.GetComponentsInChildren<Collider>()) col.enabled = false; }
                furniture.Add(item.gameObject); item.gameObject.SetActive(false);
            }
            c.fallenFurniture = furniture.ToArray();
            var data = new[] {
                new Vector3(4.3f,3.9f,-2.79f),new Vector3(4.0f,1.4f,-2.79f),new Vector3(1.4f,1.4f,-2.79f),new Vector3(1.2f,1.7f,-2.79f),
                new Vector3(1.0f,1.2f,.1f),new Vector3(1.0f,.95f,3.83f),new Vector3(1.0f,2.7f,3.83f),new Vector3(1.0f,4.7f,3.83f),new Vector3(1.0f,5.75f,3.83f),
                new Vector3(-3.3f,5.75f,3.83f),new Vector3(-3.3f,6.925f,3.83f),new Vector3(1.05f,6.925f,2.2f),new Vector3(1.05f,8.2f,2.2f),new Vector3(-3.55f,8.2f,.54f),
                new Vector3(-3.55f,5.1f,.54f),new Vector3(-1.4f,4.1f,.54f),new Vector3(.8f,4.1f,.54f),new Vector3(2.1f,4.1f,.54f),new Vector3(2.1f,1.3f,.54f),new Vector3(5.35f,1.3f,.54f),
                new Vector3(5.35f,6.5f,.54f),new Vector3(5.15f,8.0f,.54f),new Vector3(4.1f,8.4f,.54f),new Vector3(4.1f,9.8f,.37f),new Vector3(5.0f,10.9f,.37f),
                new Vector3(8.2f,10.975f,.06f),new Vector3(8.2f,7.0f,.06f),new Vector3(8.2f,4.1f,.06f),new Vector3(8.2f,-3.0f,.06f),new Vector3(0,-3.0f,.06f),new Vector3(0,-10.5f,.06f)
            };
            c.rail = data.Select(p => Anchor("CompletionChaseRail", P(p.x, p.y, p.z), -d.north)).ToArray();
            c.windowExitSpot.position = P(8.2f, 10.975f, .06f); c.windowBlockSpot.position = P(5.6f, 10.975f, .37f);
            c.corridorLights = root.parent.GetComponentsInChildren<Light>(true).Where(l => l.name == "CorridorLight").ToArray();
            c.houseLookSpot.rotation = Quaternion.LookRotation(P(1.08f, -.55f, 5.0f) - c.spawnStreet.position);
        }
        private static void RecordingSet()
        {
            var playback = d.chase.recording;
            var set = new GameObject("DamarRecordedMirrorSet"); set.transform.SetParent(root, false); set.transform.position = new Vector3(0, -45, 0);
            playback.recordingSet = set;
            var concrete = Existing("Upper_ConcretePaint"); var wood = Existing("House_MAT_WoodDoor"); var rust = Existing("Upper_SteelRust");
            var black = Mat("CompletionRecordingBlack", new Color(.012f,.018f,.021f));
            var pale = Mat("CompletionWitnessSkin", new Color(.38f,.42f,.39f));
            var bulb = Mat("CompletionMarketBulb", new Color(.75f,.30f,.09f), true);
            var cloth = Mat("CompletionMarketCloth", new Color(.18f,.04f,.035f));
            Part("RecordedStoneFloor", set.transform, new Vector3(0,-.08f,4), new Vector3(8,.15f,15), concrete);
            Part("RecordedCeiling", set.transform, new Vector3(0,3,2), new Vector3(7,.14f,5), black);
            foreach(int side in new[]{-1,1})
            {
                Part("MirrorFrameUpright",set.transform,new Vector3(side*1.1f,1.35f,3.8f),new Vector3(.16f,2.7f,.18f),wood);
                Part("CellarMirrorSideWall",set.transform,new Vector3(side*2.6f,1.4f,3.9f),new Vector3(2.8f,2.8f,.18f),concrete);
            }
            Part("MirrorFrameCrown",set.transform,new Vector3(0,2.7f,3.8f),new Vector3(2.35f,.18f,.22f),wood);
            Part("MirrorFrameFoot",set.transform,new Vector3(0,.12f,3.8f),new Vector3(2.35f,.16f,.18f),wood);
            Part("MirrorDarkThreshold",set.transform,new Vector3(0,.08f,4.3f),new Vector3(2.1f,.1f,1),black);
            for(int i=0;i<4;i++)foreach(int side in new[]{-1,1})
            {
                var stall = new GameObject("PasarMalamStall").transform; stall.SetParent(set.transform,false);stall.localPosition=new Vector3(side*2.1f,0,5.5f+i*2.2f);
                Part("StallCounter",stall,new Vector3(0,.8f,0),new Vector3(1.6f,.12f,1.15f),wood);
                Part("FadedRedAwning",stall,new Vector3(0,2.1f,0),new Vector3(1.9f,.08f,1.5f),cloth);
                foreach(int x in new[]{-1,1})Part("StallPole",stall,new Vector3(x*.8f,1.05f,.5f),new Vector3(.05f,2.1f,.05f),rust);
                for(int j=0;j<3;j++)Part("MarketOfferings",stall,new Vector3((j-1)*.35f,.96f,0),new Vector3(.19f,.22f,.19f),pale,PrimitiveType.Sphere);
                Part("WatchingFigureBody",stall,new Vector3(0,1.35f,-.4f),new Vector3(.32f,.82f,.25f),black,PrimitiveType.Capsule);
                Part("WatchingFigureFace",stall,new Vector3(0,1.93f,-.4f),new Vector3(.17f,.23f,.17f),pale,PrimitiveType.Sphere);
            }
            for(int i=0;i<18;i++)
                Part("PasarStringLight",set.transform,new Vector3(Mathf.Lerp(-2.7f,2.7f,i%6/5f),2.7f-Mathf.Sin(i%6/5f*Mathf.PI)*.4f,5.5f+i/6*3.2f),new Vector3(.06f,.09f,.06f),bulb,PrimitiveType.Sphere);
            var hands=new List<Transform>();
            for(int i=0;i<10;i++)
            {
                var arm=new GameObject("PaleReachingArm").transform;arm.SetParent(set.transform,false);
                arm.localPosition=new Vector3((i%2==0?-1:1)*(.5f+i%3*.18f),.35f+i%4*.42f,4.15f+i%2*.18f);
                Part("Forearm",arm,new Vector3(0,0,-.35f),new Vector3(.08f,.7f,.08f),pale,PrimitiveType.Capsule).transform.localRotation=Quaternion.Euler(90,0,0);
                Part("Palm",arm,new Vector3(0,0,-.75f),new Vector3(.13f,.045f,.18f),pale,PrimitiveType.Sphere);
                for(int f=0;f<5;f++)
                {var finger=Part("LongFinger",arm,new Vector3((f-2)*.028f,-.025f,-.91f+Mathf.Abs(f-2)*.012f),new Vector3(.018f,.23f,.025f),pale,PrimitiveType.Capsule);finger.transform.localRotation=Quaternion.Euler(90,0,(f-2)*7);}
                hands.Add(arm);
            }
            playback.reachingHands=hands.ToArray();
            var camera=new GameObject("DamarRecordingCamera",typeof(Camera));camera.transform.SetParent(set.transform,false);
            playback.recordingCamera=camera.GetComponent<Camera>();playback.recordingCamera.enabled=false;playback.recordingCamera.cullingMask=1<<AtmajaHandycamPlayback.RecordingLayer;
            playback.recordingCamera.fieldOfView=65;playback.recordingCamera.nearClipPlane=.04f;playback.recordingCamera.farClipPlane=25;playback.recordingCamera.backgroundColor=Color.black;playback.recordingCamera.clearFlags=CameraClearFlags.SolidColor;
            playback.recordingCamera.GetUniversalAdditionalCameraData().renderPostProcessing=false;playback.recordingCamera.GetUniversalAdditionalCameraData().renderShadows=false;
            playback.cameraStart=Anchor("DamarRecordingStart",set.transform.TransformPoint(new Vector3(0,1.35f,.7f)),Vector3.forward);
            playback.cameraEnd=Anchor("DamarRecordingPulled",set.transform.TransformPoint(new Vector3(.12f,.55f,4.5f)),new Vector3(.1f,.15f,1));
            foreach(var at in new[]{new Vector3(-.7f,2.2f,2.6f),new Vector3(1,2.4f,6),new Vector3(-1,2.4f,10)})
            {var g=new GameObject("RecordedPractical",typeof(Light));g.transform.SetParent(set.transform,false);g.transform.localPosition=at;var l=g.GetComponent<Light>();l.type=LightType.Point;l.range=7;l.intensity=1.8f;l.color=new Color(.75f,.44f,.23f);l.shadows=LightShadows.None;l.cullingMask=1<<AtmajaHandycamPlayback.RecordingLayer;}
            Layer(set,AtmajaHandycamPlayback.RecordingLayer);set.SetActive(false);
        }
        private static void ThreatPresentation()
        {
            const string path="Assets/_LayarTerakhir/Art/Original/Models/SriAtmajaCharacter.fbx";
            var importer=(ModelImporter)AssetImporter.GetAtPath(path);importer.importCameras=false;importer.importLights=false;importer.importAnimation=false;importer.addCollider=false;importer.SaveAndReimport();
            var palette=new Dictionary<string,Material>();
            foreach(var entry in new[]{("Sri_Cloth",new Color(.72f,.69f,.60f)),("Sri_Batik",new Color(.12f,.085f,.065f)),("Sri_Skin",new Color(.43f,.39f,.32f)),("Sri_Hair",new Color(.012f,.009f,.008f)),("Sri_Eye",new Color(.51f,.48f,.39f)),("Sri_Wound",new Color(.13f,.052f,.04f)),("Sri_Rust",new Color(.24f,.10f,.055f)),("Sri_Rope",new Color(.24f,.17f,.10f))})
            {
                var material=Mat("Completion"+entry.Item1,entry.Item2);
                if(entry.Item1=="Sri_Cloth"||entry.Item1=="Sri_Batik")
                {
                    // The source jacquard albedo is red. Keep its weave normal on the
                    // ivory kebaya; only the dark skirt inherits the coloured pattern.
                    material.SetTexture("_BaseMap",entry.Item1=="Sri_Batik"?AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/Art/Atmaja/Textures/quatrefoil_jacquard_fabric_diff_1k.jpg"):null);
                    material.SetTextureScale("_BaseMap",new Vector2(1.8f,1.8f));
                    var normal=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/Art/Atmaja/Textures/quatrefoil_jacquard_fabric_nor_gl_1k.jpg");
                    if(normal){material.SetTexture("_BumpMap",normal);material.SetFloat("_BumpScale",.3f);material.EnableKeyword("_NORMALMAP");}
                }
                EditorUtility.SetDirty(material);palette[entry.Item1]=material;
            }
            Transform Replace(GameObject actor,int layer)
            {
                foreach(var child in actor.transform.Cast<Transform>().ToArray())Object.DestroyImmediate(child.gameObject);
                actor.transform.localScale=Vector3.one;
                var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));model.transform.SetParent(actor.transform,false);model.name="SriAtmajaCharacterRig";
                foreach(var renderer in model.GetComponentsInChildren<Renderer>())renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>palette.TryGetValue(m.name,out var replacement)?replacement:palette["Sri_Cloth"]).ToArray();
                Layer(model,layer);return model.transform;
            }
            Transform Bone(Transform model,string name)=>model.GetComponentsInChildren<Transform>(true).First(t=>t.name==name);
            var enemy=d.basement.nyonya;var rig=Replace(enemy,0);
            var col=enemy.GetComponent<BoxCollider>();if(col){col.center=new Vector3(0,.81f,0);col.size=new Vector3(.5f,1.62f,.42f);}
            var visual=enemy.GetComponent<AtmajaThreatPresentation>();if(!visual)visual=enemy.AddComponent<AtmajaThreatPresentation>();
            visual.director=d;visual.body=Bone(rig,"Body");visual.weapon=Bone(rig,"Shears");d.basement.shearsProp=visual.weapon.gameObject;
            visual.leftArm=Bone(rig,"UpperArm_L");visual.rightArm=Bone(rig,"UpperArm_R");visual.leftLeg=Bone(rig,"Leg_L");visual.rightLeg=Bone(rig,"Leg_R");visual.head=Bone(rig,"Head");
            var smoke=new GameObject("BlitzRecoilSmoke",typeof(ParticleSystem));smoke.transform.SetParent(enemy.transform,false);smoke.transform.position=visual.head.position+Vector3.up*.04f;
            var ps=smoke.GetComponent<ParticleSystem>();ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            var main=ps.main;main.playOnAwake=false;main.loop=false;main.duration=1.2f;main.startLifetime=1.25f;main.startSize=new ParticleSystem.MinMaxCurve(.06f,.18f);main.startSpeed=.18f;main.startColor=new Color(.8f,.82f,.76f,.45f);main.maxParticles=24;
            var emit=ps.emission;emit.rateOverTime=12;
            var shape=ps.shape;shape.shapeType=ParticleSystemShapeType.Sphere;shape.radius=.14f;
            var renderer=ps.GetComponent<ParticleSystemRenderer>();renderer.sharedMaterial=SmokeMaterial();renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=false;visual.blitzSmoke=ps;
            if(d.chase.streetFigure)Replace(d.chase.streetFigure,0);
            if(d.upper.mirrorFlashFigure)Replace(d.upper.mirrorFlashFigure,AtmajaMirrorReflection.ReflectionLayer);
        }
    }
}
#endif
