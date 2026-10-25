#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        private static AnimationClip humanIdle, humanWalk;
        private static void PrepareHumans()
        {
            var map=new Dictionary<string,string>{{"Hips","Hips"},{"Spine","Spine"},{"Chest","Spine1"},{"UpperChest","Spine2"},{"Neck","Neck"},{"Head","Head"}};
            foreach(string side in new[]{"Left","Right"})
            {
                map[side+"Shoulder"]=side+"Shoulder";map[side+"UpperArm"]=side+"Arm";map[side+"LowerArm"]=side+"ForeArm";map[side+"Hand"]=side+"Hand";
                map[side+"UpperLeg"]=side+"UpLeg";map[side+"LowerLeg"]=side+"Leg";map[side+"Foot"]=side+"Foot";map[side+"Toes"]=side+"ToeBase";
                foreach(string finger in new[]{"Thumb","Index","Middle","Ring","Little"})
                    for(int i=0;i<3;i++)map[side+finger+new[]{"Proximal","Intermediate","Distal"}[i]]=side+"Hand"+(finger=="Little"?"Pinky":finger)+(i+1);
            }
            foreach(string id in new[]{"MemoryGhost","DamarFinale"})
            {
                string path=Imported+"Animations/"+id+".fbx";var importer=AssetImporter.GetAtPath(path) as ModelImporter;
                var model=AssetDatabase.LoadAssetAtPath<GameObject>(path);if(!importer||!model)throw new InvalidOperationException("Missing newly licensed humanoid rig "+id);
                string Canon(string name)=>Regex.Replace(Regex.Replace(name,@"^mixamorig[_:]?",""),@"_\d+$","");
                var transforms=model.GetComponentsInChildren<Transform>();var human=new List<HumanBone>();
                for(int i=0;i<(int)HumanBodyBones.LastBone;i++)
                {
                    string key=((HumanBodyBones)i).ToString();if(!map.TryGetValue(key,out string wanted))continue;
                    var bone=transforms.FirstOrDefault(t=>Canon(t.name)==wanted);if(!bone)continue;
                    human.Add(new HumanBone{humanName=HumanTrait.BoneName[i],boneName=bone.name,limit=new HumanLimit{useDefaultValues=true}});
                }
                if(human.Count<20)throw new InvalidOperationException("Incomplete humanoid source mapping "+id+": "+human.Count);
                var description=importer.humanDescription;description.human=human.ToArray();
                description.skeleton=Array.Empty<SkeletonBone>(); // Let Unity normalize the imported bind pose before humanoid retargeting.
                description.armStretch=description.legStretch=.05f;description.upperArmTwist=description.lowerArmTwist=description.upperLegTwist=description.lowerLegTwist=.5f;
                importer.animationType=ModelImporterAnimationType.Human;importer.avatarSetup=ModelImporterAvatarSetup.CreateFromThisModel;importer.autoGenerateAvatarMappingIfUnspecified=true;importer.humanDescription=description;importer.importAnimation=true;
                var clips=importer.defaultClipAnimations;foreach(var c in clips){c.loopTime=true;c.loopPose=true;c.lockRootRotation=true;c.lockRootHeightY=true;c.lockRootPositionXZ=true;}
                importer.clipAnimations=clips;importer.SaveAndReimport();
                var avatar=AssetDatabase.LoadAllAssetsAtPath(path).OfType<Avatar>().FirstOrDefault();
                if(!avatar||!avatar.isValid||!avatar.isHuman)throw new InvalidOperationException("Human retarget avatar is invalid: "+id);
                Debug.Log("PHASE5_HUMANOID "+id+" mappedBones="+human.Count+" sourceMotionOnly=true");
            }
            PrepareHumanMotion();
        }
        private static void PrepareHumanMotion()
        {
            string path=Imported+"Animations/QuaterniusHumanMotion.fbx";
            var importer=AssetImporter.GetAtPath(path) as ModelImporter;var model=AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(!importer||!model)throw new InvalidOperationException("New CC0 human motion missing; run Tools/fetch_phase5_motion.py");
            var map=new Dictionary<string,string>{{"Hips","DEF-hips"},{"Spine","DEF-spine.001"},{"Chest","DEF-spine.002"},{"UpperChest","DEF-spine.003"},{"Neck","DEF-neck"},{"Head","DEF-head"}};
            foreach(string side in new[]{"Left","Right"})
            {
                string suffix=side=="Left"?".L":".R";
                foreach(var pair in new[]{("Shoulder","shoulder"),("UpperArm","upper_arm"),("LowerArm","forearm"),("Hand","hand"),("UpperLeg","thigh"),("LowerLeg","shin"),("Foot","foot"),("Toes","toe")})map[side+pair.Item1]="DEF-"+pair.Item2+suffix;
                foreach(string finger in new[]{"Thumb","Index","Middle","Ring","Little"})for(int j=0;j<3;j++)
                    map[side+finger+new[]{"Proximal","Intermediate","Distal"}[j]]="DEF-"+(finger=="Thumb"?"thumb":finger=="Little"?"f_pinky":"f_"+finger.ToLowerInvariant())+".0"+(j+1)+suffix;
            }
            var bones=model.GetComponentsInChildren<Transform>();var human=new List<HumanBone>();
            for(int i=0;i<(int)HumanBodyBones.LastBone;i++)
            {
                if(!map.TryGetValue(((HumanBodyBones)i).ToString(),out string wanted))continue;
                var bone=bones.FirstOrDefault(t=>t.name==wanted);if(bone)human.Add(new HumanBone{humanName=HumanTrait.BoneName[i],boneName=bone.name,limit=new HumanLimit{useDefaultValues=true}});
            }
            var desc=importer.humanDescription;desc.human=human.ToArray();desc.skeleton=Array.Empty<SkeletonBone>();
            desc.armStretch=desc.legStretch=.05f;desc.upperArmTwist=desc.lowerArmTwist=desc.upperLegTwist=desc.lowerLegTwist=.5f;
            importer.animationType=ModelImporterAnimationType.Human;importer.avatarSetup=ModelImporterAvatarSetup.CreateFromThisModel;importer.autoGenerateAvatarMappingIfUnspecified=true;importer.humanDescription=desc;importer.importAnimation=true;
            var clips=importer.defaultClipAnimations.Where(c=>new[]{"Idle_Loop","Walk_Loop"}.Contains(c.name.Split('|').Last())).ToArray();
            if(clips.Length!=2)throw new InvalidOperationException("Expected authored human Idle/Walk, got "+string.Join(",",importer.defaultClipAnimations.Select(c=>c.name)));
            foreach(var c in clips){c.loopTime=c.loopPose=c.lockRootRotation=c.lockRootHeightY=c.lockRootPositionXZ=true;}
            importer.clipAnimations=clips;importer.SaveAndReimport();
            var avatar=AssetDatabase.LoadAllAssetsAtPath(path).OfType<Avatar>().FirstOrDefault();if(!avatar||!avatar.isValid||!avatar.isHuman)throw new InvalidOperationException("CC0 human motion avatar invalid");
            var imported=AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")).ToArray();
            humanIdle=imported.Single(c=>c.name.Split('|').Last()=="Idle_Loop");humanWalk=imported.Single(c=>c.name.Split('|').Last()=="Walk_Loop");
            if(!humanIdle.humanMotion||!humanWalk.humanMotion)throw new InvalidOperationException("Source motion must be retargetable humanoid clips");
            Debug.Log("PHASE5_NEW_HUMAN_MOTION authoredIdleWalk=true mapped="+human.Count);
        }
        private static void ValidateDamarIdlePose()
        {
            var clone=UnityEngine.Object.Instantiate(d.world.damar);clone.name="Temporary human idle validation";
            try
            {
                var animator=clone.GetComponentInChildren<Animator>();animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
                animator.Rebind();animator.Update(0);animator.SetFloat("MoveSpeed",0);animator.Update(.5f);
                var head=animator.GetBoneTransform(HumanBodyBones.Head);var left=animator.GetBoneTransform(HumanBodyBones.LeftHand);var right=animator.GetBoneTransform(HumanBodyBones.RightHand);
                bool relaxed=left.position.y<head.position.y-.15f&&right.position.y<head.position.y-.15f;
                string report="{\"handsBelowHead\":"+relaxed.ToString().ToLowerInvariant()+",\"headY\":"+head.position.y.ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"leftHandY\":"+left.position.y.ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"rightHandY\":"+right.position.y.ToString(System.Globalization.CultureInfo.InvariantCulture)+"}";
                System.IO.File.WriteAllText("Audit/phase5-human-idle-validation.json",report+"\n");
                Debug.Log("PHASE5_IDLE_POSE "+report);
                if(!relaxed)throw new InvalidOperationException("Retargeted human idle must not raise Damar's hands above his head");
            }
            finally{UnityEngine.Object.DestroyImmediate(clone);}
        }
    }
}
#endif
