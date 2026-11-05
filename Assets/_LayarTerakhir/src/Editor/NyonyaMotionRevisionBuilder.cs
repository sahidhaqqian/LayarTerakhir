#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // Refresh only the existing hunter. No replacement actor, AI or scene rebuild.
    public static class NyonyaMotionRevisionBuilder
    {
        [Serializable] private sealed class Metric {public string clip;public float strideSpeed, lowFootMedian, forwardRange;public int stanceSamples,backwardSamples;}
        [Serializable] private sealed class Report {public Metric[] clips;public int vertices,bones;}
        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            const string path=DesignerVrmBuilder.Root+"Models/NyonyaDesigner.fbx";
            var importer=(ModelImporter)AssetImporter.GetAtPath(path);
            importer.animationType=ModelImporterAnimationType.Legacy;importer.importAnimation=true;
            importer.importNormals=ModelImporterNormals.Import;importer.animationCompression=ModelImporterAnimationCompression.Off;
            var takes=importer.defaultClipAnimations;
            foreach(var take in takes)
            {
                take.name=take.name.Split('|').Last();
                take.loopTime=take.name!="Stun"&&take.name!="Windup";
                take.wrapMode=take.loopTime?WrapMode.Loop:WrapMode.ClampForever;
            }
            importer.clipAnimations=takes;importer.SaveAndReimport();
            var clips=AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")).ToArray();
            if(clips.Length!=5||new[]{"Idle","Walk","Run","Windup","Stun"}.Any(n=>clips.All(c=>c.name!=n)))throw new InvalidOperationException("The hunter must retain its five existing animation bindings");
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=UnityEngine.Object.FindFirstObjectByType<AtmajaDirector>();
            var pose=d.basement.nyonya.GetComponent<AtmajaThreatPresentation>();var motion=pose.authoredMotion;
            if(!motion||!pose.body||!pose.weapon)throw new InvalidOperationException("Existing hunter rig bindings missing");
            foreach(var name in motion.Cast<AnimationState>().Select(s=>s.name).ToArray())motion.RemoveClip(name);
            foreach(var clip in clips)motion.AddClip(Sanitized(clip),clip.name);
            motion.clip=Sanitized(clips.Single(c=>c.name=="Idle"));motion.playAutomatically=false;motion.cullingType=AnimationCullingType.AlwaysAnimate;
            foreach(var skin in motion.GetComponentsInChildren<SkinnedMeshRenderer>(true))skin.updateWhenOffscreen=true;
            var metrics=new List<Metric>();
            // Measure a temporary copy: never leave the serialized scene in a sampled pose.
            var sample=UnityEngine.Object.Instantiate(motion.gameObject);
            try
            {
                var feet=new[]{"Foot_L","Foot_R"}.Select(n=>sample.GetComponentsInChildren<Transform>().Single(t=>t.name==n)).ToArray();
                foreach(string name in new[]{"Walk","Run"})
                {
                    var clip=clips.Single(c=>c.name==name);var points=new Vector3[2,121];var floor=new[]{float.PositiveInfinity,float.PositiveInfinity};
                    for(int f=0;f<=120;f++)
                    {
                        clip.SampleAnimation(sample,clip.length*f/120);
                        for(int s=0;s<2;s++){points[s,f]=sample.transform.InverseTransformPoint(feet[s].position);floor[s]=Mathf.Min(floor[s],points[s,f].y);}
                    }
                    var speeds=new List<float>();var backward=new List<float>();float range=0;
                    for(int s=0;s<2;s++)
                    {
                        float min=float.PositiveInfinity,max=float.NegativeInfinity;
                        for(int f=0;f<=120;f++){min=Mathf.Min(min,points[s,f].z);max=Mathf.Max(max,points[s,f].z);}
                        range+=max-min;
                    }
                    for(int s=0;s<2;s++)for(int f=1;f<=120;f++)
                    {
                        float speed=-(points[s,f].z-points[s,f-1].z)/(clip.length/120);
                        if(speed>.08f)backward.Add(speed);
                        if(points[s,f].y>floor[s]+.025f||points[s,f-1].y>floor[s]+.025f)continue;
                        if(speed>.08f)speeds.Add(speed);
                    }
                    if(speeds.Count<5)throw new InvalidOperationException("Insufficient forward stance samples: "+name);
                    speeds.Sort();backward.Sort();
                    // The fitted jog has a flight phase and nonuniform foot
                    // speed. Calibrate its cycle from the measured left+right
                    // sweep distance, rather than a few low-foot samples.
                    metrics.Add(new Metric{clip=name,strideSpeed=name=="Run"?range/clip.length:speeds[speeds.Count/2],lowFootMedian=speeds[speeds.Count/2],forwardRange=range/2,stanceSamples=speeds.Count,backwardSamples=backward.Count});
                }
                var skin=sample.GetComponentInChildren<SkinnedMeshRenderer>();
                File.WriteAllText("Audit/nyonya-motion-stride.json",JsonUtility.ToJson(new Report{clips=metrics.ToArray(),vertices=skin.sharedMesh.vertexCount,bones=skin.bones.Length},true)+"\n");
            }
            finally{UnityEngine.Object.DestroyImmediate(sample);}
            pose.walkStrideSpeed=metrics[0].strideSpeed;pose.runStrideSpeed=metrics[1].strideSpeed;
            PrefabUtility.RecordPrefabInstancePropertyModifications(motion);
            foreach(var skin in motion.GetComponentsInChildren<SkinnedMeshRenderer>(true))PrefabUtility.RecordPrefabInstancePropertyModifications(skin);
            EditorUtility.SetDirty(pose);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            Debug.Log("NYONYA_MOTION_APPLIED walk="+pose.walkStrideSpeed+" run="+pose.runStrideSpeed+" existingAIAndCollision=true");
        }

        // The FBX takes bake a 100x unit-conversion scale curve onto
        // CharacterRig. The scene rig is pinned to unit scale (see
        // DesignerVrmBuilder.Replace), so every played frame re-applies
        // 100x and the hunter's body explodes into an invisible blob.
        // Strip every m_LocalScale curve; bones never need scale animation.
        public const string ClipFolder=DesignerVrmBuilder.Root+"Clips/";
        public static AnimationClip Sanitized(AnimationClip source)
        {
            Directory.CreateDirectory(ClipFolder);
            string dest=ClipFolder+"Nyonya"+source.name+".anim";
            var copy=new AnimationClip{name=source.name,legacy=source.legacy,wrapMode=source.wrapMode,frameRate=source.frameRate};
            AnimationUtility.SetAnimationClipSettings(copy,AnimationUtility.GetAnimationClipSettings(source));
            foreach(var binding in AnimationUtility.GetCurveBindings(source))
            {
                if(binding.propertyName.StartsWith("m_LocalScale"))continue;
                copy.SetCurve(binding.path,binding.type,binding.propertyName,AnimationUtility.GetEditorCurve(source,binding));
            }
            foreach(var binding in AnimationUtility.GetObjectReferenceCurveBindings(source))
                AnimationUtility.SetObjectReferenceCurve(copy,binding,AnimationUtility.GetObjectReferenceCurve(source,binding));
            AnimationUtility.SetAnimationEvents(copy,AnimationUtility.GetAnimationEvents(source));
            copy.EnsureQuaternionContinuity();
            if(AssetDatabase.LoadAssetAtPath<AnimationClip>(dest))AssetDatabase.DeleteAsset(dest);
            AssetDatabase.CreateAsset(copy,dest);
            var saved=AssetDatabase.LoadAssetAtPath<AnimationClip>(dest);
            // CreateAsset renames the object after its file; the game looks
            // states up by the original clip names (Idle/Walk/Run/...).
            saved.name=source.name;EditorUtility.SetDirty(saved);AssetDatabase.SaveAssets();
            return saved;
        }

        [MenuItem("LayarTerakhir/Characters/Sanitize Nyonya hunter clips")]
        public static void ApplySanitizedClips()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            // The designer's Gemini claw texture is 2752px wide; force a
            // reimport so the 2048 importer cap actually applies (golden
            // texture budget).
            const string clawPath="Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/CrimsonGoalDecal.png";
            var clawImporter=(TextureImporter)AssetImporter.GetAtPath(clawPath);
            clawImporter.maxTextureSize=1024;clawImporter.SaveAndReimport();
            var clawTex=AssetDatabase.LoadAssetAtPath<Texture2D>(clawPath);
            Debug.Log("CLAW_TEXTURE_SIZE "+(clawTex?clawTex.width+"x"+clawTex.height:"missing"));
            const string path=DesignerVrmBuilder.Root+"Models/NyonyaDesigner.fbx";
            var clips=AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")).ToArray();
            if(clips.Length!=5||new[]{"Idle","Walk","Run","Windup","Stun"}.Any(n=>clips.All(c=>c.name!=n)))throw new InvalidOperationException("The hunter must retain its five existing animation bindings");
            // Phase 1: (re)build the sanitized clip assets and flush them to
            // disk so phase 2 binds persistent assets, never in-memory copies
            // (those get embedded into the scene file on save).
            foreach(var c in clips)Sanitized(c);
            AssetDatabase.SaveAssets();AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            // Phase 2: reload fresh from disk and verify persistence.
            var clean=new System.Collections.Generic.List<KeyValuePair<string,AnimationClip>>();
            foreach(var c in clips)
            {
                var loaded=AssetDatabase.LoadAssetAtPath<AnimationClip>(ClipFolder+"Nyonya"+c.name+".anim");
                if(!loaded||!EditorUtility.IsPersistent(loaded))throw new InvalidOperationException("Sanitized clip not persistent: "+c.name);
                if(AnimationUtility.GetCurveBindings(loaded).Any(b=>b.propertyName.StartsWith("m_LocalScale")))throw new InvalidOperationException("Sanitized clip still has scale curves: "+c.name);
                clean.Add(new KeyValuePair<string,AnimationClip>(c.name,loaded));
            }
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            int rebound=0;
            foreach(var pose in UnityEngine.Object.FindObjectsByType<AtmajaThreatPresentation>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                var motion=pose?pose.authoredMotion:null;if(!motion)continue;
                // Clear via SerializedObject: RemoveClip(name) silently keeps
                // clips whose state names came from FBX takes.
                // Write clip references directly: legacy Animation.AddClip
                // clones clips into the scene file instead of referencing
                // the .anim assets.
                var so=new SerializedObject(motion);
                var list=so.FindProperty("m_Animations");
                list.arraySize=clean.Count;
                for(int i=0;i<clean.Count;i++)list.GetArrayElementAtIndex(i).objectReferenceValue=clean[i].Value;
                so.FindProperty("m_Animation").objectReferenceValue=clean.Single(c=>c.Key=="Idle").Value;
                so.ApplyModifiedPropertiesWithoutUndo();
                motion.playAutomatically=false;motion.cullingType=AnimationCullingType.AlwaysAnimate;
                if(motion.GetClipCount()!=5)throw new InvalidOperationException("Hunter clip count after rebind: "+motion.GetClipCount());
                foreach(AnimationState st in motion)
                    if(!st.clip||!EditorUtility.IsPersistent(st.clip))throw new InvalidOperationException("Hunter state clip not a persistent asset: "+st.name);
                foreach(var skin in motion.GetComponentsInChildren<SkinnedMeshRenderer>(true))skin.updateWhenOffscreen=true;
                EditorUtility.SetDirty(motion);rebound++;
            }
            if(rebound==0)throw new InvalidOperationException("No hunter authoredMotion found to rebind");
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            Debug.Log("NYONYA_CLIPS_SANITIZED rebound="+rebound+" scaleCurvesStripped=true");
        }
    }
}
#endif
