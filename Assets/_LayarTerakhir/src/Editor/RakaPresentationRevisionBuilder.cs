#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static class RakaPresentationRevisionBuilder
    {
        [Serializable] private sealed class StrideMetric {public string clip;public float plantedFootSpeed, signedSpeed;}
        [Serializable] private sealed class StrideMetrics {public StrideMetric[] clips;}
        [MenuItem("LayarTerakhir/Characters/Refresh Raka sourced presentation")]
        public static void ApplyAndMeasure()
        {
            Apply();
            var actor=UnityEngine.Object.Instantiate(Resources.Load<GameObject>("RakaCharacter"));
            var metrics=new List<StrideMetric>();
            try
            {
                var avatar=actor.GetComponent<AnimatedRakaAvatar>();
                foreach(string name in new[]{"Walk","Run","CrouchWalk","LowCrouchWalk"})
                {
                    var clip=avatar.motion.GetClip(name);var feet=new[]{"Foot_L","Foot_R"}.Select(n=>actor.GetComponentsInChildren<Transform>().Single(t=>t.name==n)).ToArray();
                    var positions=new Vector3[2,121];var lowest=new[]{float.PositiveInfinity,float.PositiveInfinity};
                    for(int frame=0;frame<=120;frame++)
                    {
                        clip.SampleAnimation(actor,clip.length*frame/120);
                        for(int side=0;side<2;side++){positions[side,frame]=actor.transform.InverseTransformPoint(feet[side].position);lowest[side]=Mathf.Min(lowest[side],positions[side,frame].y);}
                    }
                    var speeds=new List<float>();
                    for(int side=0;side<2;side++)for(int frame=1;frame<=120;frame++)
                    {
                        if(positions[side,frame].y>lowest[side]+.012f||positions[side,frame-1].y>lowest[side]+.012f)continue;
                        float speed=-(positions[side,frame].z-positions[side,frame-1].z)/(clip.length/120);
                        if(Mathf.Abs(speed)>.05f)speeds.Add(speed);
                    }
                    if(speeds.Count<5)throw new InvalidOperationException("No measurable stance for "+name);
                    speeds.Sort();float signed=speeds[speeds.Count/2];float measured=Mathf.Abs(signed);
                    metrics.Add(new StrideMetric{clip=name,plantedFootSpeed=measured,signedSpeed=signed});
                    Debug.Log("RAKA_STRIDE "+name+" metresPerSecond="+measured+" signed="+signed);
                }
            }
            finally{UnityEngine.Object.DestroyImmediate(actor);}
            var root=PrefabUtility.LoadPrefabContents("Assets/Resources/RakaCharacter.prefab");
            try
            {
                var avatar=root.GetComponent<AnimatedRakaAvatar>();
                avatar.walkStrideSpeed=metrics[0].plantedFootSpeed;avatar.runStrideSpeed=metrics[1].plantedFootSpeed;
                avatar.crouchStrideSpeed=metrics[2].plantedFootSpeed;avatar.lowCrouchStrideSpeed=metrics[3].plantedFootSpeed;
                PrefabUtility.SaveAsPrefabAsset(root,"Assets/Resources/RakaCharacter.prefab");
            }
            finally{PrefabUtility.UnloadPrefabContents(root);}
            File.WriteAllText("Audit/raka-presentation-stride.json",JsonUtility.ToJson(new StrideMetrics{clips=metrics.ToArray()},true)+"\n");AssetDatabase.SaveAssets();
        }

        public static void BakeCallContacts()
        {
            const string path="Assets/Resources/RakaCharacter.prefab";
            var root=PrefabUtility.LoadPrefabContents(path);GameObject sample=null;
            try
            {
                sample=UnityEngine.Object.Instantiate(root);
                if(!sample.GetComponent<AnimatedRakaAvatar>().BakeCallContactProfile(root.GetComponent<AnimatedRakaAvatar>()))
                    throw new InvalidOperationException("Cannot derive handset/head contact frames from the actual Raka meshes");
                PrefabUtility.SaveAsPrefabAsset(root,path);
            }
            finally{if(sample)UnityEngine.Object.DestroyImmediate(sample);PrefabUtility.UnloadPrefabContents(root);}
            AssetDatabase.SaveAssets();Debug.Log("RAKA_CALL_CONTACTS_BAKED nonReadablePlayerMeshes=true");
        }

        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            const string model="Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/RakaDesigner.fbx";
            var importer=(ModelImporter)AssetImporter.GetAtPath(model);
            importer.animationType=ModelImporterAnimationType.Legacy;
            importer.importAnimation=true;
            importer.importNormals=ModelImporterNormals.Import;
            importer.animationCompression=ModelImporterAnimationCompression.Off;
            var takes=importer.defaultClipAnimations;
            foreach(var take in takes)
            {
                take.name=take.name.Split('|').Last();
                take.wrapMode=take.name=="InstallCard"?WrapMode.ClampForever:WrapMode.Loop;
                take.loopTime=take.name!="InstallCard";
            }
            importer.clipAnimations=takes;importer.SaveAndReimport();
            var clips=AssetDatabase.LoadAllAssetsAtPath(model).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")).ToArray();
            if(clips.Length!=16)throw new InvalidOperationException("Raka must retain 16 existing clip names; found "+clips.Length+": "+string.Join(",",clips.Select(c=>c.name)));
            const string prefab="Assets/Resources/RakaCharacter.prefab";
            var root=PrefabUtility.LoadPrefabContents(prefab);
            try
            {
                var motion=root.GetComponent<Animation>();
                foreach(var name in motion.Cast<AnimationState>().Select(s=>s.name).ToArray())motion.RemoveClip(name);
                foreach(var clip in clips)motion.AddClip(clip,clip.name);
                motion.clip=clips.Single(c=>c.name=="Idle");motion.playAutomatically=false;
                var avatar=root.GetComponent<AnimatedRakaAvatar>();avatar.motion=motion;avatar.sourcedLocomotion=true;
                PrefabUtility.SaveAsPrefabAsset(root,prefab);
            }
            finally{PrefabUtility.UnloadPrefabContents(root);}
            const string materialPath="Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Materials/RakaDesignerSurface.mat";
            var material=AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            material.SetFloat("_Metallic",0);material.SetFloat("_Smoothness",.20f);material.SetFloat("_BumpScale",.50f);
            material.DisableKeyword("_METALLICSPECGLOSSMAP");EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            BakeCallContacts();
            Debug.Log("RAKA_PRESENTATION_APPLIED sourceMotion=Quaternius_CC0 clips=16 suppliedVRM=true gameplayControllerUnchanged=true");
        }
    }
}
#endif
