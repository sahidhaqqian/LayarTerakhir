#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class Phase2SceneBuilder
    {
        private static void DressHospital()
        {
            // Deliberate clusters with clear routes, not a scatter over the walkable floor.
            M("Bed",new Vector3(1.8f,0,-5.5f),new Vector3(1,1.1f,2.1f),12);
            M("Bed",new Vector3(10,0,-7),new Vector3(1,1.1f,2.1f),-6);
            M("Wheel_chair",new Vector3(1,0,-1.6f),new Vector3(.68f,1,.95f),110);
            M("Iv_bag_holder",new Vector3(2.6f,0,-6),new Vector3(.45f,1.85f,.45f));
            M("IV_bag",new Vector3(2.5f,1.4f,-6),new Vector3(.18f,.32f,.055f),0,false);
            M("Exit_sign",new Vector3(5,2.55f,-7.85f),new Vector3(.65f,.26f,.07f),0,false);
            M("Chair",new Vector3(8,0,-4.9f),new Vector3(.55f,1,.6f),180);
            PH("tool_cart",new Vector3(3.6f,0,-1.2f),new Vector3(.7f,.85f,.5f),90);
            Medical("EmergencyPrivacyScreen","PrivacyScreen",new Vector3(10,0,-5),new Vector3(1.4f,1.8f,.65f),-25);
            for(int i=0;i<5;i++)
            {
                M("Bench",new Vector3(14+i*4.4f,0,3.35f),new Vector3(1.9f,.9f,.6f),180);
                if(i<4)
                {
                    float x=14+i*6;
                    M("Bed",new Vector3(x+2,0,-3.6f),new Vector3(1,1.05f,2),0);
                    M("Cabinet_2",new Vector3(x+.8f,0,-5.4f),new Vector3(.8f,.75f,.5f),180);
                    M("Chair",new Vector3(x+.6f,0,-2.5f),new Vector3(.5f,.9f,.6f),10);
                    Medical("ClinicMicroscope_"+i,"Microscope",new Vector3(x+.8f,.76f,-5.4f),new Vector3(.18f,.32f,.2f),180,false);
                }
            }
            M("Pillar",new Vector3(11.7f,0,2.8f),new Vector3(.28f,3,.28f));
            // Morgue work surfaces, equipment and privacy establish a medical place.
            for(int i=0;i<2;i++)
            {
                Medical("AutopsyWorkStation_"+i,"AutopsyTable",new Vector3(22,-4,19+i*5),new Vector3(2.5f,.92f,.95f));
                Medical("AutopsyTaskLight_"+i,"SurgicalLamp",new Vector3(24,-4,19+i*5),new Vector3(.7f,1.7f,.7f));
            }
            M("Cabinet_1",new Vector3(8,-4,26),new Vector3(2,.85f,.65f),90);
            Medical("AutopsyLamp","SurgicalLamp",new Vector3(18.3f,-4,19),new Vector3(.9f,1.8f,.9f));
            Medical("AutopsyMicroscope","Microscope",new Vector3(8,-3.14f,26),new Vector3(.25f,.4f,.3f),0,false);
            Medical("AutopsyVialRack","VialRack",new Vector3(8,-3.14f,25.4f),new Vector3(.4f,.16f,.16f),0,false);
            Medical("AutopsyLabTripod","LabTripod",new Vector3(8,-3.14f,26.5f),new Vector3(.2f,.5f,.25f),0,false);
            Medical("AutopsyGlassSlide","GlassSlide",new Vector3(16,-3.07f,24),new Vector3(.12f,.01f,.06f),0,false);
            Medical("AutopsyEmptyVial","EmptyVial",new Vector3(16.8f,-3.07f,24.2f),new Vector3(.04f,.16f,.04f),0,false);
            Medical("MorgueCurtain","PrivacyScreen",new Vector3(10.5f,-4,24),new Vector3(1.1f,1.8f,.8f),60);
            PH("bunsen_burner",new Vector3(8,-3.14f,27),new Vector3(.17f,.25f,.17f),0,false);
            PH("tool_cart",new Vector3(23,-4,26.3f),new Vector3(.8f,.85f,.55f),90);
            M("Ceiling_Light",new Vector3(15,-1.12f,21.5f),new Vector3(1.1f,.1f,.55f),0,false);
            // L2 has a distinct, scarcer warm/red light vocabulary and damaged ward equipment.
            for(int i=0;i<3;i++)
            {
                M("Bed",new Vector3(6+i*10,4,19.1f),new Vector3(.9f,1,1.9f),90+(i%2==0?3:-5));
                M("Iv_bag_holder",new Vector3(7+i*10,4,19.2f),new Vector3(.4f,1.8f,.4f),0);
                Medical("WardCurtain_"+i,"PrivacyScreen",new Vector3(7.6f+i*9,4,16.7f),new Vector3(1,1.8f,.65f),90);
            }
            M("Chair",new Vector3(17.8f,4,8.9f),new Vector3(.5f,1,.6f),0);
            PH("chemistry_set",new Vector3(18.4f,4.93f,10),new Vector3(.4f,.35f,.25f),0,false);
            Medical("NurseVialRack","VialRack",new Vector3(19,4.93f,10),new Vector3(.35f,.13f,.18f),0,false);
            Medical("NurseMicroscope","Microscope",new Vector3(16.9f,4.93f,10),new Vector3(.23f,.35f,.25f),90,false);
            M("Cabinet_3",new Vector3(23,4,14),new Vector3(.65f,.8f,.55f));
            M("Exit_sign",new Vector3(1,6.65f,16.15f),new Vector3(.6f,.24f,.06f),0,false);
            Text("FloorLandmark","BANGSAL BERSALIN • 2001",new Vector3(4,6.6f,19.85f),180,.13f);
            Text("MorgueLandmark","INSTALASI KAMAR MAYAT\nHARAP TENANG",new Vector3(31.8f,-1.3f,20),270,.13f);
            Text("ArchiveLandmark","ADMINISTRASI / ARSIP",new Vector3(13,-1.3f,27.82f),180,.1f);
            // Labels are authored game text, not synthesized posters or photographic assets.
        }
        private static Light Fixture(string id,Vector3 p,Color color,float power=3.2f,float range=5.8f,bool shadows=false)
        {
            M("Ceiling_Light",p,new Vector3(.7f,.06f,.4f),0,false);
            var light=Point(id+"Light",p-Vector3.up*.12f).gameObject.AddComponent<Light>();
            light.type=LightType.Point;light.color=color;light.intensity=power;light.range=range;
            light.shadows=shadows?LightShadows.Soft:LightShadows.None;light.shadowBias=.025f;light.shadowNormalBias=.14f;
            return light;
        }
        private static void Lighting(VolumeProfile unused)
        {
            RenderSettings.skybox=null;RenderSettings.ambientMode=AmbientMode.Trilight;
            RenderSettings.ambientSkyColor=new Color(.12f,.16f,.2f);RenderSettings.ambientEquatorColor=new Color(.065f,.085f,.09f);
            RenderSettings.ambientGroundColor=new Color(.025f,.03f,.033f);
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogColor=new Color(.026f,.035f,.038f);RenderSettings.fogDensity=.006f;
            var moon=Point("RSUD_Moon",Vector3.zero).gameObject.AddComponent<Light>();moon.type=LightType.Directional;moon.intensity=.28f;
            moon.color=new Color(.55f,.69f,.85f);moon.transform.rotation=Quaternion.Euler(45,-25,0);moon.shadows=LightShadows.Soft;
            Fixture("IGD_Reception",new Vector3(7,2.85f,-3),new Color(.74f,.85f,.75f),4.2f,7,true);
            Fixture("IGD_Entrance",new Vector3(3,2.85f,-7),new Color(.73f,.81f,.83f),2,6);
            for(int i=0;i<5;i++)Fixture("Poli_"+i,new Vector3(4+i*7,2.85f,2),i==3?new Color(.52f,.7f,.9f):new Color(.73f,.8f,.7f),3.4f,5.5f,i%2==0);
            Fixture("Poli03_Frozen",new Vector3(27,2.5f,-3),new Color(.32f,.61f,.78f),1.8f,4);
            Fixture("Basement_Landing",new Vector3(34,-1.15f,17),new Color(.64f,.72f,.74f),2.2f,5,true);
            foreach(float x in new[]{10f,18f,26f})Fixture("Morgue_"+x,new Vector3(x,-1.15f,22),new Color(.56f,.72f,.72f),3.8f,6,x==18);
            Fixture("Archive",new Vector3(14,-1.15f,31),new Color(.8f,.65f,.4f),2.5f,5,true);
            Fixture("Duty",new Vector3(34,-1.15f,24),new Color(.86f,.72f,.48f),2.4f,4,true);
            Fixture("InkBankA",new Vector3(11,6.85f,23.5f),new Color(.68f,.76f,.78f),3.2f,5.2f,true);
            Fixture("InkBankB",new Vector3(16,6.85f,27.5f),new Color(.72f,.82f,.77f),3.2f,5.2f);
            Fixture("Nurse",new Vector3(18,6.85f,12),new Color(.86f,.69f,.47f),3.3f,6,true);
            d.world.corridorLights=new Light[3];
            for(int i=0;i<3;i++)
            {
                d.world.corridorLights[i]=Fixture("Ward_S"+i,new Vector3(5+i*12,6.85f,18),new Color(.8f,.57f,.49f),3.3f,6,true);
                // A separate low emergency fixture remains readable after the overhead fails.
                var red=Point("Ward_Emergency_"+i,new Vector3(5+i*12,4.4f,16.25f)).gameObject.AddComponent<Light>();
                red.type=LightType.Point;red.color=new Color(.8f,.12f,.08f);red.intensity=.65f;red.range=4;red.shadows=LightShadows.Soft;
            }
            foreach(int x in new[]{2,34})for(int i=0;i<2;i++)
                Fixture("Stair_"+x+"_"+i,new Vector3(x,(x==2?i*2f:-(i+1)*2f)+2.4f,6.5f+i*5),new Color(.64f,.74f,.73f),2.2f,5,true);
            string path=Art+"RSUDVolume.asset";var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if(!profile){profile=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(profile,path);}
            foreach(var component in profile.components.ToArray())Object.DestroyImmediate(component,true);profile.components.Clear();
            var tone=profile.Add<Tonemapping>(true);tone.mode.value=TonemappingMode.ACES;
            var color=profile.Add<ColorAdjustments>(true);color.postExposure.value=.55f;color.contrast.value=10;color.saturation.value=-10;
            var bloom=profile.Add<Bloom>(true);bloom.intensity.value=.12f;bloom.threshold.value=1.2f;
            var vignette=profile.Add<Vignette>(true);vignette.intensity.value=.19f;vignette.smoothness.value=.5f;
            foreach(var component in profile.components)AssetDatabase.AddObjectToAsset(component,profile);
            EditorUtility.SetDirty(profile);var volume=Point("RSUD_Atmosphere",Vector3.zero).gameObject.AddComponent<Volume>();volume.isGlobal=true;volume.sharedProfile=profile;
            var budget=d.gameObject.AddComponent<Phase1LightBudget>();budget.player=d.player.transform;budget.isolateFloors=true;budget.externallyControlledLights=d.world.corridorLights.Concat(d.world.arenaAmbient).ToArray();
            Probe("IGD",new Vector3(6,1.5f,-3),new Vector3(12,3,10));Probe("Morgue",new Vector3(18,-2.5f,22),new Vector3(26,3,14));Probe("L2",new Vector3(18,5.5f,18),new Vector3(36,3,8));
        }
        private static void Probe(string id,Vector3 p,Vector3 size)
        {
            var probe=Point("RSUD_Reflection_"+id,p).gameObject.AddComponent<ReflectionProbe>();
            probe.mode=ReflectionProbeMode.Realtime;probe.refreshMode=ReflectionProbeRefreshMode.OnAwake;
            probe.timeSlicingMode=ReflectionProbeTimeSlicingMode.IndividualFaces;probe.resolution=128;probe.boxProjection=true;probe.size=size;
            probe.cullingMask=~((1<<25)|(1<<26)|(1<<27)|(1<<28)|(1<<29)|(1<<31));probe.intensity=.65f;
        }
    }
}
#endif
