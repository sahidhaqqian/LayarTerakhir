#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using UnityEditor;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static class PlaytestRevisionBuilder
    {
        [Serializable] private class VoiceManifest {public VoiceEntry[] entries;}
        [Serializable] private class VoiceEntry {public string speaker,text,path;}
        [MenuItem("LayarTerakhir/Prolog/Build reviewed playtest revision")]
        public static void Build()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            ConfigureDocumentPaper();
            var map=(TextureImporter)AssetImporter.GetAtPath("Assets/Resources/Phone/JakartaStreetMap.png");
            if(map){map.maxTextureSize=1024;map.mipmapEnabled=false;map.wrapMode=TextureWrapMode.Clamp;map.textureCompression=TextureImporterCompression.Compressed;map.SaveAndReimport();}
            AtmajaSceneBuilder.Build();
            ImportPresentation();KosCorridorBuilder.Build();
            Debug.Log("PLAYTEST_REVISION_BUILD complete; Golden room scene preserved");
        }
        [MenuItem("LayarTerakhir/Prolog/Refine Kos only (preserve both approved scenes)")]
        public static void RefineKos()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            ImportPresentation();KosCorridorBuilder.Build();
            Debug.Log("KOS_REFINEMENT_BUILD complete; Golden and Atmaja source scenes not rewritten");
        }
        private static void ConfigureDocumentPaper()
        {
            // Kertas catatan asli (CC0 ambientCG Paper001) sebagai Sprite UI (bug 20).
            const string path = "Assets/Resources/Paper/DocumentPaper.jpg";
            if (AssetImporter.GetAtPath(path) is TextureImporter ti)
            {
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Single;
                ti.sRGBTexture = true;
                ti.maxTextureSize = 1024;
                ti.mipmapEnabled = true;
                ti.textureCompression = TextureImporterCompression.Compressed;
                ti.SaveAndReimport();
            }
        }
        private static void ImportPresentation()
        {
            ConfigureDocumentPaper();
            foreach(var path in Directory.GetFiles("Assets/Resources/KosAudio","*.wav"))
            {
                var importer=(AudioImporter)AssetImporter.GetAtPath(path);importer.forceToMono=true;var settings=importer.defaultSampleSettings;settings.loadType=AudioClipLoadType.DecompressOnLoad;settings.compressionFormat=AudioCompressionFormat.ADPCM;importer.defaultSampleSettings=settings;importer.SaveAndReimport();
            }
            foreach(var path in Directory.GetFiles("Assets/_LayarTerakhir/assets/Audio/Dialogue","*.wav"))
            {
                var importer=(AudioImporter)AssetImporter.GetAtPath(path);importer.forceToMono=true;var settings=importer.defaultSampleSettings;settings.loadType=AudioClipLoadType.CompressedInMemory;settings.compressionFormat=AudioCompressionFormat.Vorbis;settings.quality=.7f;settings.sampleRateSetting=AudioSampleRateSetting.OverrideSampleRate;settings.sampleRateOverride=22050;importer.defaultSampleSettings=settings;importer.SaveAndReimport();
            }
            Directory.CreateDirectory("Assets/Resources");AssetDatabase.Refresh();
            const string profilePath="Assets/Resources/LayarPresentation.asset";var profile=AssetDatabase.LoadAssetAtPath<PresentationProfile>(profilePath);
            if(!profile){profile=ScriptableObject.CreateInstance<PresentationProfile>();AssetDatabase.CreateAsset(profile,profilePath);}
            profile.bodyFont=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");profile.audio=AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");profile.phoneFeedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/PhoneFeed.mat");
            var voices=JsonUtility.FromJson<VoiceManifest>(File.ReadAllText("Audit/prolog-voice-manifest.json"));profile.voices=voices.entries.Select(v=>new PresentationProfile.VoiceLine{speaker=v.speaker,text=v.text,clip=AssetDatabase.LoadAssetAtPath<AudioClip>(v.path)}).ToArray();
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
        }
    }
}
#endif
