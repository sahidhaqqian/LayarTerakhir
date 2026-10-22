#if UNITY_EDITOR
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class PlayerFeelTests
    {
        [Test] public void ImportedPhoneGripsBraceHandsetWithoutSkinPenetration()
        {
            var actor=Object.Instantiate(Resources.Load<GameObject>("RakaCharacter"));
            var firstPerson=Object.Instantiate(Resources.Load<GameObject>("RakaPhoneHand"));
            var phoneMesh=new Mesh();var skinMesh=new Mesh();
            try
            {
                var avatar=actor.GetComponent<AnimatedRakaAvatar>();avatar.handset.SetActive(true);
                var phone=avatar.handset.GetComponent<SkinnedMeshRenderer>();
                var skin=actor.GetComponentsInChildren<SkinnedMeshRenderer>(true).Single(r=>r.name=="VisibleSkin");
                var masks=skin.sharedMesh.boneWeights.Select(weight=>
                {
                    var totals=new float[5];
                    var indices=new[]{weight.boneIndex0,weight.boneIndex1,weight.boneIndex2,weight.boneIndex3};
                    var values=new[]{weight.weight0,weight.weight1,weight.weight2,weight.weight3};
                    for(int j=0;j<4;j++)
                    {
                        string bone=skin.bones[indices[j]].name;
                        if(bone.StartsWith("Finger")&&bone.EndsWith("_R"))totals[bone[6]-'1']+=values[j];
                    }
                    byte mask=0;for(int finger=0;finger<5;finger++)if(totals[finger]>.25f)mask|=(byte)(1<<finger);
                    return mask;
                }).ToArray();
                foreach(string name in new[]{"PhoneIdle","PhoneWalk","PhoneRun","PhoneCrouch","PhoneCrouchWalk","PhoneLowCrouch","PhoneLowCrouchWalk"})
                for(int phase=0;phase<8;phase++)
                {
                    var clip=avatar.motion.GetClip(name);Assert.That(clip,Is.Not.Null);
                    clip.SampleAnimation(actor,clip.length*phase/8);
                    phone.BakeMesh(phoneMesh,true);skin.BakeMesh(skinMesh,true);
                    AssertGrip(phoneMesh.vertices.Select(phone.transform.TransformPoint).ToArray(),skinMesh.vertices.Select(skin.transform.TransformPoint).ToArray(),name+" phase "+phase,masks);
                    phoneMesh.Clear();skinMesh.Clear();
                }
                var staticPhone=firstPerson.GetComponentsInChildren<MeshFilter>(true).Single(m=>m.name=="PhoneHand_HandheldPhone");
                var staticSkin=firstPerson.GetComponentsInChildren<MeshFilter>(true).Single(m=>m.name=="PhoneHand_VisibleSkin");
                AssertGrip(staticPhone.sharedMesh.vertices.Select(staticPhone.transform.TransformPoint).ToArray(),staticSkin.sharedMesh.vertices.Select(staticSkin.transform.TransformPoint).ToArray(),"First-person baked grip");
            }
            finally{Object.DestroyImmediate(phoneMesh);Object.DestroyImmediate(skinMesh);Object.DestroyImmediate(actor);Object.DestroyImmediate(firstPerson);}
        }
        private static void AssertGrip(Vector3[] phone,Vector3[] skin,string label,byte[] masks=null)
        {
            var corners=new List<Vector3>();
            foreach(var point in phone)if(!corners.Any(other=>(point-other).sqrMagnitude<.0000000001f))corners.Add(point);
            Assert.That(corners.Count,Is.EqualTo(8),label+" phone box corners");
            Vector3 centre=corners.Aggregate(Vector3.zero,(a,b)=>a+b)/8;
            var axes=new List<Vector3>();
            foreach(var edge in corners.Skip(1).Select(p=>p-corners[0]).OrderBy(p=>p.sqrMagnitude))
                if(axes.All(axis=>Mathf.Abs(Vector3.Dot(axis,edge.normalized))<.01f))axes.Add(edge.normalized);
            Assert.That(axes.Count,Is.EqualTo(3),label+" orthogonal phone axes");
            var extent=axes.Select(axis=>corners.Max(p=>Mathf.Abs(Vector3.Dot(p-centre,axis)))).ToArray();
            float maximumDepth=0;var contact=Enumerable.Repeat(float.PositiveInfinity,5).ToArray();
            for(int v=0;v<skin.Length;v++)
            {
                Vector3 point=skin[v]-centre;
                var signed=axes.Select((axis,i)=>extent[i]-Mathf.Abs(Vector3.Dot(point,axis))).ToArray();
                maximumDepth=Mathf.Max(maximumDepth,signed.Min());
                if(masks==null||masks[v]==0)continue;
                float gap=Mathf.Sqrt(signed.Sum(d=>Mathf.Min(0,d)*Mathf.Min(0,d)));
                for(int finger=0;finger<5;finger++)if((masks[v]&(1<<finger))!=0)contact[finger]=Mathf.Min(contact[finger],gap);
            }
            Assert.That(maximumDepth,Is.LessThan(.0015f),label+" skin must stay outside the handset (1.5mm import tolerance)");
            if(masks!=null)for(int finger=0;finger<5;finger++)Assert.That(contact[finger],Is.LessThan(.004f),label+" finger "+(finger+1)+" must brace the handset within 4mm");
        }
        [Test] public void ImportedCrouchClipsKeepRenderedGeometryWithinHeadroomAndFloor()
        {
            var actor=Object.Instantiate(Resources.Load<GameObject>("RakaCharacter"));
            var baked=new Mesh();
            try
            {
                actor.transform.position=Vector3.zero;
                var avatar=actor.GetComponent<AnimatedRakaAvatar>();
                foreach(var name in new[]{"Crouch","CrouchWalk","PhoneCrouch","PhoneCrouchWalk","LowCrouch","LowCrouchWalk","PhoneLowCrouch","PhoneLowCrouchWalk"})
                {
                    var clip=avatar.motion.GetClip(name);Assert.That(clip,Is.Not.Null);
                    avatar.handset.SetActive(name.StartsWith("Phone"));
                    for(int phase=0;phase<8;phase++)
                    {
                        clip.SampleAnimation(actor,clip.length*phase/8);
                        float bottom=float.PositiveInfinity,top=float.NegativeInfinity;
                        foreach(var renderer in actor.GetComponentsInChildren<SkinnedMeshRenderer>())
                        {
                            if(!renderer.enabled)continue;
                            renderer.BakeMesh(baked,true);
                            foreach(var vertex in baked.vertices)
                            {float y=renderer.transform.TransformPoint(vertex).y;bottom=Mathf.Min(bottom,y);top=Mathf.Max(top,y);}
                            baked.Clear();
                        }
                        Assert.That(top,Is.InRange(.6f,name.Contains("Low")?.83f:1.02f),name+" phase "+phase+" headroom in world meters");
                        Assert.That(bottom,Is.GreaterThan(-.025f),name+" phase "+phase+" must not fold a leg through the floor");
                    }
                }
            }
            finally{Object.DestroyImmediate(baked);Object.DestroyImmediate(actor);}
        }
        [Test] public void WalkAcceleratesWithoutInstantFullSpeedOrOvershoot()
        {
            Vector3 velocity=FirstPersonController.StepPlanarVelocity(Vector3.zero,Vector3.forward*1.7f,1f/60,9,24,28);
            Assert.That(velocity.z,Is.InRange(.14f,.16f));
            for(int i=0;i<60;i++)velocity=FirstPersonController.StepPlanarVelocity(velocity,Vector3.forward*1.7f,1f/60,9,24,28);
            Assert.That(velocity.z,Is.EqualTo(1.7f).Within(.0001f));
        }
        [Test] public void BrakingStopsRunWithinTwoTenthsWithoutReversing()
        {
            Vector3 v=Vector3.forward*4.2f;
            for(int i=0;i<12;i++)v=FirstPersonController.StepPlanarVelocity(v,Vector3.zero,1f/60,13,24,28);
            Assert.That(v.sqrMagnitude,Is.LessThan(.000001f));
        }
        [TestCase(30)] [TestCase(60)] [TestCase(144)]
        public void AccelerationUsesTimeRatherThanFrameCount(int framesPerSecond)
        {
            Vector3 v=Vector3.zero;
            for(int i=0;i<framesPerSecond;i++)v=FirstPersonController.StepPlanarVelocity(v,Vector3.right*4.2f,1f/framesPerSecond,13,24,28);
            Assert.That(v.x,Is.EqualTo(4.2f).Within(.0001f));Assert.That(v.y,Is.Zero);
        }
        [Test] public void ReversingDirectionHasResponsiveSteering()
        {
            Vector3 v=Vector3.forward*1.7f;
            for(int i=0;i<8;i++)v=FirstPersonController.StepPlanarVelocity(v,Vector3.back*1.7f,1f/60,9,24,28);
            Assert.That(v.z,Is.LessThan(-.65f));
        }
        [Test] public void PausedResponseDoesNotMoveAndDiagonalSpeedIsBounded()
        {
            Vector3 target=new Vector3(1,0,1).normalized*1.7f;
            Assert.That(FirstPersonController.StepPlanarVelocity(Vector3.zero,target,0,9,24,28),Is.EqualTo(Vector3.zero));
            Assert.That(FirstPersonController.StepPlanarVelocity(Vector3.zero,target,1,9,24,28).magnitude,Is.EqualTo(1.7f).Within(.0001f));
        }
        [Test] public void LowCeilingStillPreventsStandingAndTriggersDoNotTrapPlayer()
        {
            var actor=new GameObject("FeelHeadroom",typeof(CharacterController));actor.transform.position=new Vector3(1000,10,1000);
            var control=actor.AddComponent<FirstPersonController>();
            typeof(FirstPersonController).GetField("_controller",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(control,actor.GetComponent<CharacterController>());
            var capsule=actor.GetComponent<CharacterController>();capsule.radius=.32f;capsule.height=1;capsule.center=Vector3.up*.5f;
            var roof=GameObject.CreatePrimitive(PrimitiveType.Cube);roof.transform.position=actor.transform.position+Vector3.up*1.3f;roof.transform.localScale=new Vector3(2,.15f,2);
            try
            {
                Physics.SyncTransforms();Assert.That(control.CanStandUp(),Is.False);
                roof.GetComponent<Collider>().isTrigger=true;Physics.SyncTransforms();Assert.That(control.CanStandUp(),Is.True);
                Assert.That(capsule.height,Is.EqualTo(1));Assert.That(capsule.radius,Is.EqualTo(.32f));
            }
            finally{Object.DestroyImmediate(roof);Object.DestroyImmediate(actor);}
        }
        [Test] public void EveryFootstepSurfaceHasFiveOriginalSurfaceRecordings()
        {
            foreach(string surface in new[]{"Stone","Earth","Wood"})
            {
                var clips=Resources.LoadAll<AudioClip>("PlayerPresentation/Steps/"+surface);
                Assert.That(clips.Length,Is.EqualTo(5),surface+" surface bank");
            }
        }
        [TestCase(false)] [TestCase(true)]
        public void FootstepContactsStayCorrectOverTenWrappedCycles(bool reverse)
        {
            float[] phases=reverse?new[]{.9f,.65f,.4f,.15f,.9f}:new[]{.1f,.3f,.55f,.8f,.1f};
            int count=0;
            for(int cycle=0;cycle<10;cycle++)
                for(int i=1;i<phases.Length;i++)
                    if(PlayerFootsteps.CrossedWrappedContact(phases[i-1],phases[i],reverse?.58f:0,reverse))count++;
            Assert.That(count,Is.EqualTo(20),"Two footfalls per cycle, including every later wrap");
        }
        [Test] public void FootstepContactHandlesForwardReverseAndLoopWrap()
        {
            Assert.That(PlayerFootsteps.CrossedContact(.48f,.52f,0),Is.True,"forward contact");
            Assert.That(PlayerFootsteps.CrossedContact(.52f,.58f,0),Is.False,"forward between contacts");
            Assert.That(PlayerFootsteps.CrossedContact(.60f,.56f,.58f),Is.True,"reverse contact");
            Assert.That(PlayerFootsteps.CrossedContact(.99f,1.01f,0),Is.True,"forward loop wrap");
            Assert.That(PlayerFootsteps.CrossedContact(.10f,.06f,.58f),Is.True,"reverse contact after wrap");
        }
        [Test] public void FootstepSurfaceRecognisesAuthoredFloorNames()
        {
            var names=new[]{"MelatiRoadSection","UpperFloor_West","MelatiNeighborhoodGround","CeramicTileFloor","CellarFloor"};
            var objects=names.Select(name=>{var go=new GameObject(name);go.AddComponent<BoxCollider>();return go;}).ToArray();
            try
            {
                Assert.That(PlayerFootsteps.SurfaceFor(objects[0].GetComponent<Collider>()),Is.EqualTo(FootstepSurface.Stone));
                Assert.That(PlayerFootsteps.SurfaceFor(objects[1].GetComponent<Collider>()),Is.EqualTo(FootstepSurface.Wood));
                Assert.That(PlayerFootsteps.SurfaceFor(objects[2].GetComponent<Collider>()),Is.EqualTo(FootstepSurface.Earth));
                Assert.That(PlayerFootsteps.SurfaceFor(objects[3].GetComponent<Collider>()),Is.EqualTo(FootstepSurface.Tile));
                Assert.That(PlayerFootsteps.SurfaceFor(objects[4].GetComponent<Collider>()),Is.EqualTo(FootstepSurface.Stone));
            }
            finally{foreach(var go in objects)Object.DestroyImmediate(go);}
        }
        [Test] public void PracticalBeamCookieHasSoftEdgeAndOpaqueCore()
        {
            var go=new GameObject("CookieLight",typeof(Light));var light=go.GetComponent<Light>();light.type=LightType.Spot;
            try
            {
                PracticalLightPresentation.ConfigureBeam(light);
                Assert.That(light.cookie,Is.Not.Null,"flashlight must receive a cookie");
                var texture=PracticalLightPresentation.CreateCookie();
                try
                {
                    Assert.That(texture.GetPixel(128,128).a,Is.GreaterThan(.5f),"cookie core stays bright");
                    Assert.That(texture.GetPixel(4,4).a,Is.LessThan(.05f),"cookie edge fades out");
                }
                finally{Object.DestroyImmediate(texture);}
            }
            finally{Object.DestroyImmediate(go);}
        }
        [Test] public void ThreatRimIsLimitedToItsRenderingLayer()
        {
            var subject=new GameObject("ThreatSubject");var model=GameObject.CreatePrimitive(PrimitiveType.Cube);model.transform.SetParent(subject.transform,false);
            try
            {
                PracticalLightPresentation.AddThreatRim(subject);
                var silhouette=subject.transform.Find("ThreatSilhouette");Assert.That(silhouette,Is.Not.Null);
                var light=silhouette.GetComponent<Light>();var data=light.GetUniversalAdditionalLightData();
                Assert.That((uint)data.renderingLayers,Is.EqualTo(PracticalLightPresentation.ThreatRenderingLayer));
                Assert.That(model.GetComponent<Renderer>().renderingLayerMask&PracticalLightPresentation.ThreatRenderingLayer,Is.Not.Zero);
            }
            finally{Object.DestroyImmediate(subject);}
        }
        [Test] public void AvatarDitherShaderCompilesWithoutErrors()
        {
            var shader=Resources.Load<Shader>("PlayerPresentation/AvatarDither");
            Assert.That(shader,Is.Not.Null,"camera fade shader must stay under Resources");
            Assert.That(shader.name,Is.EqualTo("LayarTerakhir/Avatar Dither Lit"));
            Assert.That(UnityEditor.ShaderUtil.ShaderHasError(shader),Is.False,"URP dither shader compiles");
        }
    }
}
#endif
