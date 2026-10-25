#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        [Serializable] private class MatRow { public string name, sourceId, Albedo, AlbedoAlpha, Normal, Alpha, UnityMask; public float[] color; public float roughness, metallic; public bool transparent; }
        [Serializable] private class MatManifest { public MatRow[] materials; }
        [Serializable] private class MeshBinding { public string renderer, mesh; public string[] materials; }
        [Serializable] private class SourceBinding { public string sourceId; public MeshBinding[] meshes; }
        [Serializable] private class Bindings { public SourceBinding[] models; }
        private static Bindings meshBindings;
        private static MatManifest importedMaterials;
        private static readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, MatRow> unnamedMaterial = new Dictionary<string, MatRow>();
        private static string Normalize(string value) => new string((value ?? "").Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        private static void PrepareMaterials()
        {
            Directory.CreateDirectory(Art + "Materials"); AssetDatabase.Refresh(); materials.Clear();
            importedMaterials = JsonUtility.FromJson<MatManifest>(File.ReadAllText("Audit/phase5-import-manifest.json")); unnamedMaterial.Clear();
            meshBindings = JsonUtility.FromJson<Bindings>(File.ReadAllText("Audit/phase5-mesh-material-bindings.json"));
            foreach (string file in Directory.GetFiles(Imported, "*.fbx", SearchOption.AllDirectories))
            {
                var i = AssetImporter.GetAtPath(file) as ModelImporter; if (!i) continue;
                if (i.materialImportMode == ModelImporterMaterialImportMode.ImportStandard && i.materialName == ModelImporterMaterialName.BasedOnMaterialName && i.materialSearch == ModelImporterMaterialSearch.Local) continue;
                i.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                i.materialName = ModelImporterMaterialName.BasedOnMaterialName; i.materialSearch = ModelImporterMaterialSearch.Local;
                i.importCameras = i.importLights = false; i.importAnimation = file.Contains("Animations/"); if(i.importAnimation)i.animationType=ModelImporterAnimationType.Generic; i.SaveAndReimport();
            }
            // Some imported PH FBXs expose their primary material as "Lit".
            // Resolve it by eliminating the embedded named slots, never by using
            // the first texture or sharing a generic white material across props.
            foreach (string file in Directory.GetFiles(Imported + "Models", "*.fbx"))
            {
                string id = Path.GetFileNameWithoutExtension(file);
                var named = new HashSet<string>(AssetDatabase.LoadAllAssetsAtPath(file).OfType<Material>().Select(m => Normalize(m.name)));
                var remaining = importedMaterials.materials.Where(r => r.sourceId == id && !named.Contains(Normalize(r.name))).ToArray();
                if (remaining.Length == 1) unnamedMaterial[id] = remaining[0];
            }
            foreach (string file in Directory.GetFiles(Imported, "*", SearchOption.AllDirectories).Where(f => f.EndsWith(".png") || f.EndsWith(".jpg")))
            {
                var i = AssetImporter.GetAtPath(file) as TextureImporter; if (!i) continue;
                bool normal = file.Contains("nor_gl") || file.Contains("Normal.png");
                bool linear = normal || file.Contains("UnityMask") || file.Contains("MetalRough") || file.Contains("_arm_") || file.Contains("_rough_") || file.Contains("_ao_");
                bool glyph = file.Contains("Glyphs/"); var type = normal ? TextureImporterType.NormalMap : glyph ? TextureImporterType.Sprite : TextureImporterType.Default;
                bool critical = new[]{"/ScreenRobe/","/MemoryGhost/","/DamarFinale/","/AntiqueMirror/","/MarketStall/","/AtmajaEpilogueHouse/","/brown_mud_03/","/concrete_wall_007/","/wooden_rough_planks/"}.Any(file.Contains);
                int textureLimit = glyph ? 512 : critical ? 2048 : 1024;
                if (i.maxTextureSize == textureLimit && i.textureType == type && i.sRGBTexture == !linear && i.alphaIsTransparency == !linear) continue;
                i.maxTextureSize = textureLimit; i.textureType = type; i.sRGBTexture = !linear; i.alphaIsTransparency = !linear;
                i.mipmapEnabled = !glyph; i.textureCompression = TextureImporterCompression.Compressed; i.anisoLevel = 4; i.SaveAndReimport();
            }
            foreach (var f in Directory.GetFiles(Imported + "Audio", "*.ogg"))
            {
                var i = AssetImporter.GetAtPath(f) as AudioImporter; if (!i) continue;
                var settings = i.defaultSampleSettings; settings.loadType = AudioClipLoadType.CompressedInMemory; settings.compressionFormat = AudioCompressionFormat.Vorbis; settings.quality = .65f;
                i.defaultSampleSettings = settings; i.forceToMono = true; i.SaveAndReimport();
            }
        }
        private static string[] SourceSlots(string source, Renderer renderer)
        {
            var model = meshBindings.models.FirstOrDefault(m => m.sourceId == source);
            if (model == null) return renderer.sharedMaterials.Select(m => m ? m.name : source).ToArray();
            string Trim(string v) => System.Text.RegularExpressions.Regex.Replace(v ?? "", @"(?:[._]\d{3})+$", "");
            var mf = renderer.GetComponent<MeshFilter>(); string mesh = mf && mf.sharedMesh ? mf.sharedMesh.name : "";
            var binding = model.meshes.FirstOrDefault(m => m.renderer == renderer.name) ??
                model.meshes.FirstOrDefault(m => Normalize(Trim(m.renderer)) == Normalize(Trim(renderer.name))) ??
                model.meshes.FirstOrDefault(m => Normalize(Trim(m.renderer)) == Normalize(Trim(mesh))) ??
                model.meshes.FirstOrDefault(m => Normalize(Trim(m.mesh)) == Normalize(Trim(mesh)));
            if (binding == null && model.meshes.Length == 1) binding = model.meshes[0];
            if (binding == null && model.meshes.Select(m=>string.Join("|",m.materials)).Distinct().Count()==1) binding=model.meshes[0];
            // FBX curves without an assigned slot get a Unity default slot. Use the
            // most frequent explicitly authored material of this same licensed model.
            if (binding != null && binding.materials.Length == 0 && renderer.sharedMaterials.Length > 0)
            {
                string primary = model.meshes.SelectMany(b => b.materials).GroupBy(n => n).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
                if (primary != null) return Enumerable.Repeat(primary, renderer.sharedMaterials.Length).ToArray();
            }
            if (binding == null || binding.materials.Length != renderer.sharedMaterials.Length)
                throw new InvalidOperationException("Source material slots differ: " + source + " / " + renderer.name + " / " + mesh);
            return binding.materials;
        }
        private static Material MaterialFor(string name, string source)
        {
            string key = source + "_" + (name ?? source); if (materials.TryGetValue(key, out var m)) return m;
            m = NewMaterial(key, "Universal Render Pipeline/Lit");
            var row = importedMaterials.materials.FirstOrDefault(r => r.sourceId == source && Normalize(r.name) == Normalize(name));
            if (row == null)
            {
                var choices = importedMaterials.materials.Where(r => r.sourceId == source).ToArray();
                if (choices.Length == 1) row = choices[0];
                else if (name == "Lit" && unnamedMaterial.TryGetValue(source, out var missing)) row = missing;
                else throw new InvalidOperationException("Unmapped licensed material: " + source + " / " + name);
            }
            m.enableInstancing = true; m.SetFloat("_Cull", source == "modular_urban_apartments_facade" ? 0 : 2); m.SetFloat("_Smoothness", .2f); m.SetFloat("_Metallic", 0);
            if (row != null)
            {
                if (row.color != null && row.color.Length == 4) m.SetColor("_BaseColor", new Color(row.color[0], row.color[1], row.color[2], row.color[3]));
                SetMap(m, "_BaseMap", string.IsNullOrEmpty(row.AlbedoAlpha) ? row.Albedo : row.AlbedoAlpha); SetMap(m, "_BumpMap", row.Normal); SetMap(m, "_MetallicGlossMap", row.UnityMask); SetMap(m, "_OcclusionMap", row.UnityMask);
                m.SetFloat("_Smoothness", Mathf.Clamp(1 - row.roughness, .04f, .58f)); m.SetFloat("_Metallic", row.metallic);
                if (!string.IsNullOrEmpty(row.Normal)) m.EnableKeyword("_NORMALMAP");
                if (!string.IsNullOrEmpty(row.UnityMask)) { m.EnableKeyword("_METALLICSPECGLOSSMAP"); m.EnableKeyword("_OCCLUSIONMAP"); m.SetFloat("_OcclusionStrength", .65f); m.SetFloat("_Smoothness", 1); }
                string lower = key.ToLowerInvariant();
                if (!string.IsNullOrEmpty(row.AlbedoAlpha) || lower.Contains("alpha") || lower.Contains("leav") || lower.Contains("leaf") || lower.Contains("grass") || lower.Contains("shrub") || lower.Contains("foliage") || lower.Contains("hair") || lower.Contains("saclar") || source == "BambooMemory")
                { m.SetFloat("_Cull", 0); m.SetFloat("_AlphaClip", 1); m.SetFloat("_Cutoff", .38f); m.EnableKeyword("_ALPHATEST_ON"); }
                if (lower.Contains("glass")) { m.SetFloat("_AlphaClip", 0); m.DisableKeyword("_ALPHATEST_ON"); m.SetColor("_BaseColor", new Color(.16f, .18f, .2f, 1)); m.SetFloat("_Smoothness", .78f); m.SetFloat("_Metallic", .92f); }
            }
            materials[key] = m; EditorUtility.SetDirty(m); return m;
        }
        private static Material NewMaterial(string key, string shader)
        {
            string file = Art + "Materials/" + string.Concat(key.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_')) + ".mat";
            var m = AssetDatabase.LoadAssetAtPath<Material>(file);
            if (!m) { m = new Material(Shader.Find(shader)); AssetDatabase.CreateAsset(m, file); }
            m.name = key;
            return m;
        }
        private static void SetMap(Material m, string property, string path) { if (!string.IsNullOrEmpty(path)) m.SetTexture(property, AssetDatabase.LoadAssetAtPath<Texture2D>(path)); }
        private static void SetSurface(Material m, string source)
        {
            string folder = Imported + "Textures/" + source; var fs = Directory.GetFiles(folder);
            SetMap(m, "_BaseMap", fs.FirstOrDefault(f => f.Contains("_diff_") && !f.EndsWith(".meta")));
            var normal = fs.FirstOrDefault(f => f.Contains("_nor_gl_") && !f.EndsWith(".meta")); SetMap(m, "_BumpMap", normal);
            var mask = fs.FirstOrDefault(f => f.Contains("UnityMask.png") && !f.EndsWith(".meta")); SetMap(m, "_MetallicGlossMap", mask); SetMap(m, "_OcclusionMap", mask);
            if (normal != null) m.EnableKeyword("_NORMALMAP"); if (mask != null) { m.EnableKeyword("_METALLICSPECGLOSSMAP"); m.EnableKeyword("_OCCLUSIONMAP"); }
            m.SetFloat("_Smoothness", mask != null ? 1 : .12f); m.SetFloat("_OcclusionStrength", .65f); m.SetFloat("_Cull", 0);
        }
        private static Material Surface(string id)
        {
            string key = "Ground_" + id; if (materials.TryGetValue(key, out var m)) return m;
            m = NewMaterial(key, "Universal Render Pipeline/Lit"); SetSurface(m, id);
            m.SetFloat("_BumpScale", .45f);m.SetColor("_BaseColor",Color.white);
            materials[key] = m; return m;
        }
        private static Material Glyph(string id, Color color)
        {
            string key = "Glyph_" + id + color; if (materials.TryGetValue(key, out var m)) return m;
            m = NewMaterial(key, "Sprites/Default"); m.SetTexture("_MainTex", AssetDatabase.LoadAssetAtPath<Texture2D>(Imported + "Glyphs/" + id + ".png")); m.SetColor("_Color", color); materials[key] = m; return m;
        }
    }
}
#endif
