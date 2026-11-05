#if UNITY_EDITOR
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using LayarTerakhir.Prolog;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class NyonyaMotionTests
    {
        [TestCase("Idle",.0005f)]
        [TestCase("Walk",.005f)]
        [TestCase("Run",.005f)]
        public void ShippedMotionDeformsTheMeshAndKeepsItAboveTheFloor(string name,float minimumDeformation)
        {
            const string path=DesignerVrmBuilder.Root+"Models/NyonyaDesigner.fbx";
            var actor=Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            try
            {
                var clip=AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Single(c=>c.name==name);
                var skin=actor.GetComponentInChildren<SkinnedMeshRenderer>();
                Assert.That(skin.bones.Length,Is.EqualTo(64),"Retain the supplied character, skirt and shears rig");
                clip.SampleAnimation(actor,0);var first=SkinPositions(skin,actor.transform);float maximumDeformation=0;
                for(int step=1;step<=24;step++)
                {
                    clip.SampleAnimation(actor,clip.length*step/24);var vertices=SkinPositions(skin,actor.transform);float squared=0,minY=float.PositiveInfinity;
                    for(int i=0;i<vertices.Length;i++)
                    {
                        squared+=(vertices[i]-first[i]).sqrMagnitude;
                        minY=Mathf.Min(minY,vertices[i].y);
                    }
                    Assert.That(minY,Is.GreaterThan(-.015f),name+" must not sink its supplied mesh below the navigation floor");
                    maximumDeformation=Mathf.Max(maximumDeformation,Mathf.Sqrt(squared/vertices.Length));
                    Assert.That(actor.transform.position,Is.EqualTo(Vector3.zero),"Clips must not take ownership of AI root motion");
                }
                Assert.That(maximumDeformation,Is.GreaterThan(minimumDeformation),"A translated rigid statue cannot pass this check");
            }
            finally{Object.DestroyImmediate(actor);}
        }

        // Regression: the FBX takes bake a 100x scale curve onto CharacterRig.
        // The scene rig is pinned to unit scale, so playing a raw FBX clip
        // exploded the basement hunter's body 100x (invisible in game, fine
        // in bind pose). The clips bound in the scene must carry no scale
        // curves and sampling them must keep the mesh human-sized.
        [Test]
        public void HunterBoundClipsKeepRigAtUnitScale()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var pose = d.basement.nyonya.GetComponent<AtmajaThreatPresentation>();
            var model = pose.authoredMotion.gameObject;
            var copy = Object.Instantiate(model);
            try
            {
                copy.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                var skin = copy.GetComponentInChildren<SkinnedMeshRenderer>();
                var rig = copy.GetComponentsInChildren<Transform>(true).Single(x => x.name == "CharacterRig");
                foreach (AnimationState state in pose.authoredMotion)
                {
                    var clip = state.clip;
                    Assert.That(AnimationUtility.GetCurveBindings(clip).Any(b => b.propertyName.StartsWith("m_LocalScale")), Is.False, clip.name + " still carries an m_LocalScale curve");
                    clip.SampleAnimation(copy, clip.length * .5f);
                    Assert.That(rig.localScale, Is.EqualTo(Vector3.one), clip.name + " rescaled CharacterRig");
                    var vertices = SkinPositions(skin, copy.transform);
                    Assert.That(vertices.Max(v => v.magnitude), Is.LessThan(5f), clip.name + " exploded the hunter mesh");
                }
            }
            finally { Object.DestroyImmediate(copy); }
        }

        // Use the same bone/bind-pose matrices as skinning. BakeMesh(false)
        // already includes this FBX's 100x renderer scale; transforming its
        // result through the renderer again would double-apply that scale.
        private static Vector3[] SkinPositions(SkinnedMeshRenderer skin,Transform actor)
        {
            var mesh=skin.sharedMesh;var source=mesh.vertices;var weights=mesh.boneWeights;var binds=mesh.bindposes;
            var matrices=skin.bones.Select((bone,i)=>actor.worldToLocalMatrix*bone.localToWorldMatrix*binds[i]).ToArray();
            var result=new Vector3[source.Length];
            for(int i=0;i<source.Length;i++)
            {
                var w=weights[i];var v=source[i];
                result[i]=matrices[w.boneIndex0].MultiplyPoint3x4(v)*w.weight0+matrices[w.boneIndex1].MultiplyPoint3x4(v)*w.weight1+matrices[w.boneIndex2].MultiplyPoint3x4(v)*w.weight2+matrices[w.boneIndex3].MultiplyPoint3x4(v)*w.weight3;
            }
            return result;
        }
    }
}
#endif
