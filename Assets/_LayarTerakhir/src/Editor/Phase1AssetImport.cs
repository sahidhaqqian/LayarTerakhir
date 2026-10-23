#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Editor
{
    public static partial class Phase1SceneBuilder
    {
        private const string Imported = Art + "Imported/";
        [Serializable] private sealed class ImportedSceneReceipt
        {
            public string scene,utc;
            public int renderers,triangles,uniqueMeshes,missingScripts,missingMaterials,originalVisualMeshes;
            public string[] meshSources;
            public bool runtimePerformanceMeasured=false;
        }
        private static void ValidateImportedScene(Scene scene)
        {
            var renderers=scene.GetRootGameObjects().SelectMany(root=>root.GetComponentsInChildren<Renderer>(true)).ToArray();
            var meshes=scene.GetRootGameObjects().SelectMany(root=>root.GetComponentsInChildren<MeshFilter>(true)).Select(f=>f.sharedMesh)
                .Concat(scene.GetRootGameObjects().SelectMany(root=>root.GetComponentsInChildren<SkinnedMeshRenderer>(true)).Select(r=>r.sharedMesh)).Where(m=>m).ToArray();
            var paths=meshes.Select(AssetDatabase.GetAssetPath).Distinct().ToArray();
            // Unity's existing phone/UI primitives remain permitted. New chapter
            // visual geometry must resolve to a downloaded/user source, not a builder mesh.
            int original=paths.Count(p=>p.StartsWith(Art+"Meshes/",StringComparison.Ordinal));
            int missing=renderers.Count(r=>r.sharedMaterials.Any(m=>!m));
            if(original>0||missing>0)throw new InvalidOperationException("Chapter visual source/material validation failed");
            int triangles=meshes.Sum(mesh=>Enumerable.Range(0,mesh.subMeshCount).Sum(i=>(int)mesh.GetIndexCount(i)/3));
            var report=new ImportedSceneReceipt{scene=ScenePath,utc=DateTime.UtcNow.ToString("o"),renderers=renderers.Length,triangles=triangles,
                uniqueMeshes=meshes.Distinct().Count(),missingScripts=0,missingMaterials=missing,originalVisualMeshes=original,meshSources=paths.OrderBy(p=>p).ToArray()};
            File.WriteAllText("Audit/phase1-scene-validation.json",JsonUtility.ToJson(report,true)+"\n");
        }
        private static void ImportExternalSurfaces()
        {
            var blood=Plain("External_ExileBlood",new Color(.4f,.2f,.15f),.05f);
            blood.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Imported+"ExileBlood/blood.png"));
            blood.SetFloat("_AlphaClip",1);blood.SetFloat("_Cutoff",.05f);blood.EnableKeyword("_ALPHATEST_ON");
            blood.SetOverrideTag("RenderType","TransparentCutout");blood.renderQueue=2450;
            foreach (string folder in Directory.GetDirectories(Imported))
            {
                string id = Path.GetFileName(folder);
                if (!File.Exists(folder + "/" + id + ".fbx")) continue;
                var importer = (ModelImporter)AssetImporter.GetAtPath(folder + "/" + id + ".fbx");
                importer.importAnimation = importer.importCameras = importer.importLights = importer.addCollider = false;
                importer.materialImportMode = ModelImporterMaterialImportMode.None; importer.SaveAndReimport();
                var material = Plain("External_" + id, Color.white, 1);
                foreach (string slot in new[] { "Diffuse", "Normal", "MetalSmooth", "Occlusion" })
                {
                    string path = Directory.GetFiles(folder, id + slot + ".*").FirstOrDefault(p => !p.EndsWith(".meta"));
                    if (path == null) throw new InvalidOperationException("Missing imported PBR channel: " + id + slot);
                    var textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
                    textureImporter.textureType = slot == "Normal" ? TextureImporterType.NormalMap : TextureImporterType.Default;
                    textureImporter.sRGBTexture = slot == "Diffuse"; textureImporter.maxTextureSize = 1024;
                    textureImporter.mipmapEnabled = true; textureImporter.SaveAndReimport();
                    material.SetTexture(slot == "Diffuse" ? "_BaseMap" : slot == "Normal" ? "_BumpMap" : slot == "MetalSmooth" ? "_MetallicGlossMap" : "_OcclusionMap", AssetDatabase.LoadAssetAtPath<Texture2D>(path));
                }
                material.EnableKeyword("_NORMALMAP"); material.EnableKeyword("_METALLICSPECGLOSSMAP"); material.EnableKeyword("_OCCLUSIONMAP");
                EditorUtility.SetDirty(material);
            }
        }
        // Fit an existing source mesh by transform only. No vertices, topology or UVs are authored.
        private static Mesh ImportedMesh(string path)
        {
            var mesh = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Mesh>().FirstOrDefault();
            if (!mesh) throw new InvalidOperationException("Missing source mesh: " + path);
            return mesh;
        }
        private static GameObject MeshInstance(string name, Mesh mesh, Vector3 position, Vector3 size, Material material, bool collider, Transform parent)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent, false); var bounds = mesh.bounds;
            var worldScale=new Vector3(size.x / bounds.size.x, size.y / bounds.size.y, size.z / bounds.size.z);
            var parentScale=parent?parent.lossyScale:Vector3.one;
            go.transform.localScale = new Vector3(worldScale.x/parentScale.x,worldScale.y/parentScale.y,worldScale.z/parentScale.z);
            go.transform.position = position - Vector3.Scale(bounds.center,worldScale);
            go.GetComponent<MeshFilter>().sharedMesh = mesh; go.GetComponent<MeshRenderer>().sharedMaterial = material;
            if(parent==current)GameObjectUtility.SetStaticEditorFlags(go,StaticEditorFlags.BatchingStatic);
            if (collider) { var hit = go.AddComponent<BoxCollider>(); hit.center = bounds.center; hit.size = bounds.size; }
            return go;
        }
        private static GameObject SourceProp(string id, string name, Vector3 centre, float height, bool collider = true, Vector3? fit = null, Material material = null, string path = null, Transform parent = null,float maxDimension=0)
        {
            path = path ?? Imported + id + "/" + id + ".fbx";
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (!asset) throw new InvalidOperationException("Missing existing/downloaded prop " + path);
            var root = new GameObject(name); root.transform.SetParent(parent ? parent : current, false); root.transform.position = centre;
            root.transform.rotation=Quaternion.identity;
            var inherited=root.transform.parent.lossyScale;
            root.transform.localScale=new Vector3(1/inherited.x,1/inherited.y,1/inherited.z);
            var visual = (GameObject)PrefabUtility.InstantiatePrefab(asset); visual.transform.SetParent(root.transform, false);
            // Keep the FBX import-axis rotation and scale. Resetting that rotation
            // lays upright scanned props on their side and creates oversized blockers.
            visual.transform.localPosition = Vector3.zero;
            foreach (var hit in visual.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(hit);
            var renderers = visual.GetComponentsInChildren<Renderer>();
            var bounds = renderers[0].bounds; foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            Vector3 size = fit ?? bounds.size * (maxDimension>0?maxDimension/Mathf.Max(bounds.size.x,bounds.size.y,bounds.size.z):height/bounds.size.y);
            Vector3 scale = new Vector3(size.x / bounds.size.x, size.y / bounds.size.y, size.z / bounds.size.z);
            visual.transform.localPosition = -(bounds.center-centre);
            root.transform.localScale=Vector3.Scale(root.transform.localScale,scale);
            var surface = material ? material : AssetDatabase.LoadAssetAtPath<Material>(Art + "Materials/External_" + id + ".mat");
            if (surface) foreach (var renderer in renderers) renderer.sharedMaterials = renderer.sharedMaterials.Select(_ => surface).ToArray();
            else AssignImportedMaterials(visual);
            if (collider) root.AddComponent<BoxCollider>().size = bounds.size;
            return root;
        }
    }
}
#endif
