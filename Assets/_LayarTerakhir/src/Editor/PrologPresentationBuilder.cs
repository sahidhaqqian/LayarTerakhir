#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Actor/presentation pass. Story objects, collision, checkpoints and room layout stay authored.
    public static class PrologPresentationBuilder
    {
        private const string Root="Assets/_LayarTerakhir/assets/Art/Characters/";
        public static void RebuildRakaOnly()
        {
            materials.Clear();
            foreach(var guid in AssetDatabase.FindAssets("t:Material",new[]{Root+"Materials"}))
            {var material=AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));materials[material.name]=material;}
            Surface("Raka_TShirt",null,new Color(.14f,.17f,.2f));Surface("Raka_DenimWaistband",null,new Color(.035f,.065f,.095f));ImportModel("RakaRigged",true);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Root+"Models/RakaRigged.fbx"));
            ApplyMaterials(model);model.name="RakaCharacter";
            var avatar=model.AddComponent<AnimatedRakaAvatar>();avatar.motion=BindMotion(model,"RakaRigged");
            avatar.handset=model.GetComponentsInChildren<Transform>(true).First(t=>t.name=="HandheldPhone").gameObject;
            PrefabUtility.SaveAsPrefabAsset(model,"Assets/Resources/RakaCharacter.prefab");Object.DestroyImmediate(model);AssetDatabase.SaveAssets();
            if(File.Exists(Root+"Models/RakaPhoneHand.fbx"))
            {
                ImportModel("RakaPhoneHand",false);
                var hand=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Root+"Models/RakaPhoneHand.fbx"));
                ApplyMaterials(hand);hand.name="RakaPhoneHand";
                PrefabUtility.SaveAsPrefabAsset(hand,"Assets/Resources/RakaPhoneHand.prefab");Object.DestroyImmediate(hand);AssetDatabase.SaveAssets();
            }
        }
        private static readonly Dictionary<string,Material> materials=new Dictionary<string,Material>();
        private static string Hash(string path){using var sha=SHA256.Create();return BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(path))).Replace("-","").ToLowerInvariant();}
        [MenuItem("LayarTerakhir/Prolog/Apply skinned character presentation")]
        public static void Apply()
        {
            string kos=Hash(PrologKosSceneBuilder.ScenePath),corridor=Hash(KosCorridorBuilder.ScenePath);
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            materials.Clear();Directory.CreateDirectory(Root+"Materials");
            foreach(string name in new[]{"SriSkin","RakaSkin","SriHair","RakaHair","BrownEyes","SriKebayaLace-v1","SriSkirtJacquard","RakaClothes","RakaClothesNormal","RakaShoes"})
            {
                string path=Root+"Textures/"+name+".png";var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                importer.maxTextureSize=1024;importer.mipmapEnabled=true;importer.textureCompression=TextureImporterCompression.Compressed;
                importer.textureType=name.EndsWith("Normal")?TextureImporterType.NormalMap:TextureImporterType.Default;
                importer.alphaIsTransparency=name.EndsWith("Hair");importer.SaveAndReimport();
            }
            Surface("Sri_Skin","SriSkin",new Color(.77f,.81f,.75f));Surface("Sri_Hair","SriHair",new Color(.32f,.30f,.28f),true);
            Surface("Sri_Eye","BrownEyes",new Color(.65f,.73f,.67f));Surface("Sri_Cloth","SriKebayaLace-v1",new Color(.83f,.81f,.75f));
            Surface("Sri_Batik","SriSkirtJacquard",new Color(.16f,.095f,.055f));Surface("Sri_Rust",null,new Color(.17f,.075f,.035f));Surface("Sri_Seam",null,new Color(.24f,.20f,.15f));
            Surface("Raka_Skin","RakaSkin",Color.white);Surface("Raka_Hair","RakaHair",new Color(.28f,.25f,.23f),true);
            Surface("Raka_Eye","BrownEyes",Color.white);Surface("Raka_Clothes","RakaClothes",new Color(.85f,.92f,.85f));
            Surface("Raka_PhoneScreen",null,new Color(.07f,.13f,.15f));
            Surface("Raka_DenimWaistband",null,new Color(.035f,.065f,.095f));
            Surface("Raka_Shoes","RakaShoes",new Color(.60f,.60f,.57f));Surface("Raka_Phone",null,new Color(.018f,.02f,.023f));
            materials["Raka_Clothes"].SetTexture("_BumpMap",Texture("RakaClothesNormal"));materials["Raka_Clothes"].EnableKeyword("_NORMALMAP");
            foreach(string name in new[]{"SriAtmajaRigged","RakaRigged"})ImportModel(name,true);
            foreach(string name in new[]{"SriAtmajaEcho","SriAtmajaSeated"})ImportModel(name,false);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();
            var hunter=Replace(d.basement.nyonya,"SriAtmajaRigged",0);var motion=BindMotion(hunter,"SriAtmajaRigged");
            Transform Bone(string name)=>hunter.GetComponentsInChildren<Transform>(true).First(t=>t.name==name);
            var threat=d.basement.nyonya.GetComponent<AtmajaThreatPresentation>();
            threat.body=Bone("Hips");threat.leftArm=Bone("UpperArm_L");threat.rightArm=Bone("UpperArm_R");
            threat.leftLeg=Bone("Thigh_L");threat.rightLeg=Bone("Thigh_R");threat.head=Bone("Head");threat.weapon=Bone("Shears");
            threat.authoredMotion=motion;d.basement.shearsProp=threat.weapon.gameObject;
            var smoke=new GameObject("BlitzRecoilSmoke",typeof(ParticleSystem));smoke.transform.SetParent(d.basement.nyonya.transform,false);smoke.transform.position=threat.head.position;
            var ps=smoke.GetComponent<ParticleSystem>();ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            var main=ps.main;main.playOnAwake=false;main.loop=false;main.duration=1.1f;main.startLifetime=1.3f;main.startSize=new ParticleSystem.MinMaxCurve(.06f,.18f);main.startSpeed=.18f;main.startColor=new Color(.8f,.82f,.76f,.45f);main.maxParticles=24;
            var emission=ps.emission;emission.rateOverTime=12;var shape=ps.shape;shape.shapeType=ParticleSystemShapeType.Sphere;shape.radius=.14f;
            var smokeRenderer=ps.GetComponent<ParticleSystemRenderer>();smokeRenderer.sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/CompletionSealSmoke.mat");smokeRenderer.shadowCastingMode=ShadowCastingMode.Off;threat.blitzSmoke=ps;
            Replace(d.chase.streetFigure,"SriAtmajaEcho",0);
            Replace(d.upper.mirrorFlashFigure,"SriAtmajaEcho",AtmajaMirrorReflection.ReflectionLayer);
            Replace(d.upper.hangedFlashFigure,"SriAtmajaEcho",0);
            Replace(d.arFigure,"SriAtmajaSeated",AtmajaPhoneView.ARLayer);
            d.arFigure.transform.position=d.P(3.7f,4.05f,.06f);
            var raka=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Root+"Models/RakaRigged.fbx"));
            ApplyMaterials(raka);raka.name="RakaCharacter";var rakaMotion=BindMotion(raka,"RakaRigged");
            var avatar=raka.AddComponent<AnimatedRakaAvatar>();avatar.motion=rakaMotion;avatar.handset=raka.GetComponentsInChildren<Transform>(true).First(t=>t.name=="HandheldPhone").gameObject;
            PrefabUtility.SaveAsPrefabAsset(raka,"Assets/Resources/RakaCharacter.prefab");Object.DestroyImmediate(raka);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            if(kos!=Hash(PrologKosSceneBuilder.ScenePath)||corridor!=Hash(KosCorridorBuilder.ScenePath))throw new Exception("Character presentation modified approved Kos scene bytes");
            File.WriteAllText("Audit/character-presentation-authoring.json",JsonUtility.ToJson(new Receipt{utc=DateTime.UtcNow.ToString("o"),kosSha256=kos,corridorSha256=corridor,atmajaSha256=Hash(AtmajaSceneBuilder.ScenePath),rakaClips=11,nyonyaClips=5,approvedSceneBytesPreserved=true},true)+"\n");
            Debug.Log("CHARACTER_PRESENTATION_APPLIED skinned Raka/Nyonya, baked clips, low-cost apparition meshes, hybrid camera resource");
        }
        [Serializable] private class Receipt{public string utc,kosSha256,corridorSha256,atmajaSha256;public int rakaClips,nyonyaClips;public bool approvedSceneBytesPreserved;}
        private static Texture2D Texture(string name)=>AssetDatabase.LoadAssetAtPath<Texture2D>(Root+"Textures/"+name+".png");
        private static void Surface(string name,string texture,Color color,bool hair=false)
        {
            string path=Root+"Materials/"+name+".mat";var material=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!material){material=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(material,path);}
            material.SetColor("_BaseColor",color);material.SetTexture("_BaseMap",texture==null?null:Texture(texture));material.SetFloat("_Smoothness",hair?.34f:name.EndsWith("Eye")?.36f:.23f);
            if(hair){material.SetFloat("_AlphaClip",1);material.SetFloat("_Cutoff",.32f);material.SetFloat("_Cull",0);material.EnableKeyword("_ALPHATEST_ON");material.renderQueue=2450;}
            EditorUtility.SetDirty(material);materials[name]=material;
        }
        private static string ClipName(string value)=>value.Contains("|")?value.Substring(value.LastIndexOf('|')+1):value;
        private static void ImportModel(string name,bool animate)
        {
            var importer=(ModelImporter)AssetImporter.GetAtPath(Root+"Models/"+name+".fbx");
            // Retain the authored player foot contacts instead of reducing joint keys.
            if(name=="RakaRigged")importer.animationCompression=ModelImporterAnimationCompression.Off;
            importer.importCameras=false;importer.importLights=false;importer.addCollider=false;importer.importAnimation=animate;importer.animationType=animate?ModelImporterAnimationType.Legacy:ModelImporterAnimationType.None;importer.SaveAndReimport();
            if(!animate)return;
            var clips=importer.defaultClipAnimations;
            foreach(var clip in clips){clip.name=ClipName(clip.name);clip.loopTime=clip.name!="Windup"&&clip.name!="Stun"&&clip.name!="InstallCard";clip.wrapMode=clip.loopTime?WrapMode.Loop:WrapMode.ClampForever;}
            importer.clipAnimations=clips;importer.SaveAndReimport();
        }
        private static Animation BindMotion(GameObject model,string asset)
        {
            var animation=model.GetComponent<Animation>();if(!animation)animation=model.AddComponent<Animation>();animation.playAutomatically=false;animation.cullingType=AnimationCullingType.BasedOnRenderers;
            foreach(var clip in AssetDatabase.LoadAllAssetsAtPath(Root+"Models/"+asset+".fbx").OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")))
            {string name=ClipName(clip.name);animation.AddClip(clip,name);if(name=="Idle")animation.clip=clip;}
            if(!animation["Idle"])throw new Exception("Missing imported idle clip on "+asset);
            return animation;
        }
        private static void ApplyMaterials(GameObject model)
        {
            foreach(var renderer in model.GetComponentsInChildren<Renderer>(true))
            {
                renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>materials.TryGetValue(m.name,out var replacement)?replacement:throw new Exception("Unknown character material "+m.name)).ToArray();
                if(renderer is SkinnedMeshRenderer skin)skin.quality=SkinQuality.Bone4;
            }
        }
        private static GameObject Replace(GameObject actor,string asset,int layer)
        {
            foreach(var child in actor.transform.Cast<Transform>().ToArray())Object.DestroyImmediate(child.gameObject);
            actor.transform.localScale=Vector3.one;
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Root+"Models/"+asset+".fbx"));model.transform.SetParent(actor.transform,false);ApplyMaterials(model);
            foreach(var t in model.GetComponentsInChildren<Transform>(true))t.gameObject.layer=layer;
            return model;
        }
    }
}
#endif
