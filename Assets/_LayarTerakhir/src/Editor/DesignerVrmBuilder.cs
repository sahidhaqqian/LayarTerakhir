#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Offline visual replacement. Actor roots, collision, AI, story and saves retain ownership.
    public static class DesignerVrmBuilder
    {
        public const string Root = "Assets/_LayarTerakhir/assets/Art/DesignerCharacters/";

        [MenuItem("LayarTerakhir/Characters/Apply designer VRM models")]
        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            foreach (var role in new[] { "Raka", "Nyonya", "Damar" }) Surface(role);
            var raka = Actor("RakaDesigner", true);
            raka.name = "RakaCharacter";
            var avatar = raka.AddComponent<AnimatedRakaAvatar>();
            avatar.motion = raka.GetComponent<Animation>();
            avatar.handset = Bone(raka, "HandheldPhone").gameObject;
            PrefabUtility.SaveAsPrefabAsset(raka, "Assets/Resources/RakaCharacter.prefab");
            Object.DestroyImmediate(raka);
            var hand = Actor("RakaDesignerPhoneHand", false);
            hand.name = "RakaPhoneHand";
            PrefabUtility.SaveAsPrefabAsset(hand, "Assets/Resources/RakaPhoneHand.prefab");
            Object.DestroyImmediate(hand);
            // Damar currently speaks from a first-person handycam recording. Keep his
            // supplied full-body model ready without adding a new actor/story beat.
            var damar = Actor("DamarDesigner", true);
            Directory.CreateDirectory(Root + "Prefabs");
            PrefabUtility.SaveAsPrefabAsset(damar, Root + "Prefabs/DamarDesignerCharacter.prefab");
            Object.DestroyImmediate(damar);

            var scene = EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            if (!d || !d.basement || !d.basement.nyonya) throw new Exception("Missing existing Atmaja actor bindings");
            var hunter = Replace(d.basement.nyonya, "NyonyaDesigner", true, 0);
            var visual = d.basement.nyonya.GetComponent<AtmajaThreatPresentation>();
            visual.body = Bone(hunter, "Hips");
            visual.leftArm = Bone(hunter, "UpperArm_L");
            visual.rightArm = Bone(hunter, "UpperArm_R");
            visual.leftLeg = Bone(hunter, "Thigh_L");
            visual.rightLeg = Bone(hunter, "Thigh_R");
            visual.head = Bone(hunter, "Head");
            visual.weapon = Bone(hunter, "Shears");
            visual.authoredMotion = hunter.GetComponent<Animation>();
            d.basement.shearsProp = visual.weapon.gameObject;
            Replace(d.chase.streetFigure, "NyonyaDesignerEcho", false, 0);
            Replace(d.upper.mirrorFlashFigure, "NyonyaDesignerEcho", false, AtmajaMirrorReflection.ReflectionLayer);
            Replace(d.upper.hangedFlashFigure, "NyonyaDesignerEcho", false, 0);
            Replace(d.arFigure, "NyonyaDesignerSeated", false, AtmajaPhoneView.ARLayer);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            RakaPresentationRevisionBuilder.ApplyAndMeasure();
            Debug.Log("DESIGNER_VRM_APPLIED Raka body/phone hand, Nyonya hunter/apparitions; Damar authoring prefab prepared");
        }

        private static Transform Bone(GameObject model, string name) =>
            model.GetComponentsInChildren<Transform>(true).Single(t => t.name == name);

        private static Material Surface(string role)
        {
            Directory.CreateDirectory(Root + "Materials");
            Texture2D Texture(string slot, bool normal = false, bool linear = false)
            {
                string path = Root + "Textures/" + role + slot + ".png";
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (!importer) throw new Exception("Missing designer texture " + path);
                importer.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !linear && !normal;
                importer.maxTextureSize = slot == "Albedo" ? 2048 : 1024;
                importer.mipmapEnabled = true;
                importer.textureCompression = TextureImporterCompression.Compressed;
                importer.alphaIsTransparency = false;
                importer.SaveAndReimport();
                return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
            string path = Root + "Materials/" + role + "DesignerSurface.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!mat) { mat = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(mat, path); }
            mat.name = role + "DesignerSurface";
            mat.SetColor("_BaseColor", Color.white);
            mat.SetTexture("_BaseMap", Texture("Albedo"));
            mat.SetTexture("_BumpMap", Texture("Normal", true));
            // Nyonya's kebaya/skin blow out to white under the blitz/flash and
            // her compressed normal map reads harshly faceted; keep her matte
            // while other roles retain their reviewed response.
            bool nyonya = role == "Nyonya";
            mat.SetFloat("_BumpScale", nyonya ? .4f : .65f);
            mat.EnableKeyword("_NORMALMAP");
            mat.SetTexture("_MetallicGlossMap", Texture("MetalSmooth", false, true));
            mat.SetFloat("_Smoothness", nyonya ? .32f : .65f);
            // Cloth/skin stay dielectric if a platform strips a map variant.
            // The metallic mask still supplies the small zipper/tool details.
            mat.SetFloat("_Metallic", 0);
            mat.EnableKeyword("_METALLICSPECGLOSSMAP");
            mat.SetFloat("_Cull", 0); // Source VRMs have double-sided cloth.
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static GameObject Actor(string name, bool animate)
        {
            string path = Root + "Models/" + name + ".fbx";
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (!importer) throw new Exception("Missing baked designer model " + path);
            importer.importCameras = importer.importLights = importer.addCollider = false;
            importer.importAnimation = animate;
            importer.animationType = animate ? ModelImporterAnimationType.Legacy : ModelImporterAnimationType.None;
            importer.animationCompression = ModelImporterAnimationCompression.Off;
            importer.isReadable = false;
            importer.SaveAndReimport();
            string ClipName(string clip) => clip.Contains("|") ? clip.Substring(clip.LastIndexOf('|') + 1) : clip;
            if (animate)
            {
                var clips = importer.defaultClipAnimations;
                foreach (var clip in clips)
                {
                    clip.name = ClipName(clip.name);
                    clip.loopTime = clip.name != "Windup" && clip.name != "Stun" && clip.name != "InstallCard";
                    clip.wrapMode = clip.loopTime ? WrapMode.Loop : WrapMode.ClampForever;
                }
                importer.clipAnimations = clips;
                importer.SaveAndReimport();
            }
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            foreach (var renderer in model.GetComponentsInChildren<Renderer>(true))
            {
                renderer.sharedMaterials = renderer.sharedMaterials.Select(m =>
                {
                    if (m.name.EndsWith("DesignerSurface")) return AssetDatabase.LoadAssetAtPath<Material>(Root + "Materials/" + m.name + ".mat");
                    string target = Root + "Materials/" + m.name + ".mat";
                    var material = AssetDatabase.LoadAssetAtPath<Material>(target);
                    if (!material)
                    {
                        material = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = m.name };
                        material.SetColor("_BaseColor", m.name.Contains("Screen") ? new Color(.07f, .13f, .15f) : new Color(.018f, .02f, .023f));
                        material.SetFloat("_Smoothness", .35f);
                        AssetDatabase.CreateAsset(material, target);
                    }
                    return material;
                }).ToArray();
                if (renderer is SkinnedMeshRenderer skin) { skin.quality = SkinQuality.Bone4; skin.updateWhenOffscreen = false; }
            }
            if (animate)
            {
                var animation = model.GetComponent<Animation>();
                if (!animation) animation = model.AddComponent<Animation>();
                animation.playAutomatically = false;
                foreach (var clip in AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Where(c => !c.name.StartsWith("__preview__")))
                    animation.AddClip(clip, ClipName(clip.name));
                animation.clip = animation.GetClip("Idle");
                if (!animation.clip) throw new Exception("Designer model lacks the existing Idle animation contract");
            }
            return model;
        }

        private static GameObject Replace(GameObject actor, string asset, bool animate, int layer)
        {
            if (!actor) throw new Exception("Missing existing apparition root");
            foreach (var child in actor.transform.Cast<Transform>().ToArray())
                if (child.GetComponentsInChildren<Renderer>(true).Any(r => !(r is ParticleSystemRenderer)))
                    Object.DestroyImmediate(child.gameObject);
            var model = Actor(asset, animate);
            model.transform.SetParent(actor.transform, false);
            foreach (var node in model.GetComponentsInChildren<Transform>(true)) node.gameObject.layer = layer;
            return model;
        }
    }
}
#endif
