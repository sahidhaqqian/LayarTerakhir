#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Phase2;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class Phase2SceneBuilder
    {
        private static void RitualArena()
        {
            Floor(36,48,12,24,4);WallX(12,36,48,4);WallX(24,36,48,4);WallZ(48,12,24,4);
            WallZ(36,12,16,4);WallZ(36,20,24,4);
            Anchor("OR_PREP",new Vector3(39.4f,4.03f,18),90);Room("OPERATING",36.1f,47.8f,12.2f,23.8f,4);
            d.world.ritualTable=Point("Ritual_Table_Aim",new Vector3(40.8f,5,18));
            var table=Medical("OperationTable","AutopsyTable",new Vector3(42,4,18),new Vector3(2.4f,.95f,1));
            Item(table,"RITUAL_TABLE","Letakkan pusaka — ritual Lastri",d.world.ritualTable.position);
            d.world.bossHome=Point("Lastri_Authored_RitualHome",new Vector3(37.8f,4.03f,19.5f));
            d.world.bossRetreat=Point("Lastri_Recovery_Retreat",new Vector3(46,4.03f,22));
            d.world.arenaLock=Barrier("Ritual_Lock_Collision",new Vector3(36,4,17),new Vector3(.2f,2.5f,1.3f));
            d.world.arenaLock.SetActive(false);
            var fogCollider=d.world.operationFog.AddComponent<BoxCollider>();fogCollider.size=new Vector3(.25f,2.4f,1.3f);
            var operation=environment.GetComponentsInChildren<Phase2WorldItem>().First(x=>x.id=="OR_DOOR").transform.parent;
            var fit=operation.GetChild(0);var hinge=Point("Operation_Door_Hinge",new Vector3(36,4,16.43f),operation);
            fit.SetParent(hinge,true);d.world.operationDoor=operation.gameObject.AddComponent<HingedDoor>();d.world.operationDoor.hinge=hinge;d.world.operationDoor.locked=true;
            d.world.tableRelics=Point("RitualRelicsOnTable",Vector3.zero).gameObject;
            var gunting=Prop("RitualScissors","Scissors/Models/Scissors",new Vector3(41.5f,4.96f,18),new Vector3(.25f,.03f,.12f),0,false);
            var gelang=PH("medical_tape",new Vector3(42,4.96f,18),new Vector3(.085f,.025f,.07f),0,false);
            var blood=Medical("RitualBlood","BloodVial",new Vector3(42.4f,4.96f,18),new Vector3(.1f,.22f,.1f),0,false);
            foreach(var go in new[]{gunting,gelang,blood})go.transform.SetParent(d.world.tableRelics.transform,true);
            d.world.tableRelics.SetActive(false);
            d.world.burntCloth=Prop("BurntBabySling","Ritual/Models/BurntCloth",new Vector3(42,4.96f,18),new Vector3(.55f,.025f,.4f),0,false);
            foreach(var r in d.world.burntCloth.GetComponentsInChildren<Renderer>())r.sharedMaterial=Surface("BurntCloth",new Color(.12f,.10f,.085f),materials["Walls_1"].GetTexture("_BaseMap") as Texture2D,null,null,.08f);
            d.world.burntCloth.SetActive(false);
            // The source surgical head/arm is reused separately from its portable stand.
            Prop("LO_SurgicalHead","Ritual/Models/SurgicalHead",new Vector3(42,6.18f,18),new Vector3(1.1f,.45f,.65f),0,false);
            M("Pillar",new Vector3(42,6.6f,18),new Vector3(.07f,.42f,.07f),0,false);
            d.world.beamCentres=new[]{Point("LO_Beam",new Vector3(42,4.03f,18)),Point("UV_N_Beam",new Vector3(42,4.03f,21.4f)),Point("UV_S_Beam",new Vector3(42,4.03f,14.6f))};
            d.world.beamRadii=new[]{d.balance.operationRadius,d.balance.uvRadius,d.balance.uvRadius};d.world.ritualLights=new Light[3];
            M("Pillar",new Vector3(45.85f,4,18),new Vector3(.22f,3,.22f));
            Vector3[] switches={new Vector3(45.6f,5.2f,18),new Vector3(43,5.2f,23.72f),new Vector3(43,5.2f,12.28f)};
            for(int i=0;i<3;i++)
            {
                string label=new[]{"LO • LAMPU OPERASI","UV-N • STERILISASI UTARA","UV-S • STERILISASI SELATAN"}[i];
                var toggle=PH("pull_chain_light_socket",switches[i]-Vector3.up*.1f,new Vector3(.18f,.24f,.13f),0,false);Item(toggle,"LAMP_"+i,"Sakelar "+label,switches[i]);
                Text("RitualLabel_"+i,label,switches[i]+Vector3.up*.3f,i==1?180:0,.065f);
                var light=Point("RitualBeam_"+i,d.world.beamCentres[i].position+Vector3.up*2.6f).gameObject.AddComponent<Light>();
                light.type=LightType.Spot;light.transform.rotation=Quaternion.Euler(90,0,0);light.spotAngle=i==0?150:140;light.innerSpotAngle=i==0?120:100;
                light.color=i==0?new Color(.78f,.88f,1):new Color(.48f,.43f,1);light.intensity=18;light.range=6;light.shadows=LightShadows.Soft;light.enabled=false;d.world.ritualLights[i]=light;
                if(i>0)M("Ceiling_Light",d.world.beamCentres[i].position+Vector3.up*2.7f,new Vector3(.85f,.08f,.22f),0,false);
            }
            for(int i=0;i<2;i++)
            {
                Vector3 p=i==0?new Vector3(46.4f,4,22.7f):new Vector3(37.6f,4,13.4f);
                var model=Prop("ArenaGenerator_"+i,"Additional/Models/Generator",p,new Vector3(1.15f,.88f,.75f));
                Item(model,i==0?"GENERATOR_NE":"GENERATOR_SW","Generator manual — tahan E",p+new Vector3(0,.95f,-.45f));
                Text("Generator_Label_"+i,"DAYA DARURAT\nTAHAN E • +25",p+new Vector3(0,1.3f,-.45f),180,.07f);
            }
            d.world.breakableGlass=new GameObject[4];d.world.glassBursts=new ParticleSystem[4];
            for(int i=0;i<4;i++)
            {
                Vector3 p=new Vector3(37,4,20.3f+i*.8f);
                M("Table",p,new Vector3(.95f,.6f,.6f));
                var unit=Prop("OldIncubator_"+i,"Neonatal/Models/Neonatal",p+Vector3.up*.6f,new Vector3(.9f,.7f,.55f),90,false);
                var glass=Point("BreakableHood_"+i,p,unit.transform).gameObject;
                foreach(var renderer in unit.GetComponentsInChildren<Renderer>())if(renderer.sharedMaterials.Any(m=>m.HasProperty("_Surface")&&m.GetFloat("_Surface")>.5f))renderer.transform.SetParent(glass.transform,true);
                d.world.breakableGlass[i]=glass;
                var burst=Point("GlassBurstPool_"+i,p+Vector3.up*1.05f).gameObject.AddComponent<ParticleSystem>();
                var bm=burst.main;bm.loop=false;bm.playOnAwake=false;bm.duration=.2f;bm.startLifetime=.75f;bm.startSpeed=new ParticleSystem.MinMaxCurve(1.2f,2.5f);bm.startSize=new ParticleSystem.MinMaxCurve(.025f,.06f);bm.startColor=new Color(.6f,.8f,.85f,.8f);bm.maxParticles=24;bm.gravityModifier=.6f;
                var be=burst.emission;be.rateOverTime=0;be.SetBursts(new[]{new ParticleSystem.Burst(0,18)});var bs=burst.shape;bs.shapeType=ParticleSystemShapeType.Sphere;bs.radius=.2f;
                burst.GetComponent<ParticleSystemRenderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(Art+"Materials/OperationMist.mat");burst.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);d.world.glassBursts[i]=burst;
            }
            // Dressing leaves two unobstructed loops around the table and all power points.
            PH("tool_cart",new Vector3(46.8f,4,16),new Vector3(.8f,.85f,.55f),90);
            Medical("RitualPrivacyScreen","PrivacyScreen",new Vector3(46.5f,4,19.8f),new Vector3(1.1f,1.6f,.5f),90);
            M("Iv_bag_holder",new Vector3(44,4,19),new Vector3(.4f,1.9f,.4f));
            var mirror=M("Tile_Wall_Window",new Vector3(47.86f,5,18.6f),new Vector3(1.25f,1.2f,.08f),90,false);
            mirror.name="MedicalMirror_Prop";
            d.world.arenaAmbient=new[]{Fixture("OR_Readable",new Vector3(42,6.85f,18),new Color(.63f,.77f,.73f),4,7,true)};
            foreach(var p in new[]{new Vector3(36.3f,4.35f,17),new Vector3(46.8f,4.35f,23.7f),new Vector3(37.4f,4.35f,12.3f)})
            {var light=Point("OR_Emergency",p).gameObject.AddComponent<Light>();light.color=new Color(.85f,.09f,.06f);light.intensity=.9f;light.range=4.2f;light.shadows=LightShadows.None;}
            d.world.bossMist=Object.Instantiate(d.world.operationFog);d.world.bossMist.name="KabutSengkala";d.world.bossMist.transform.SetParent(d.world.lastri.transform,false);d.world.bossMist.transform.localPosition=Vector3.up;
            Object.DestroyImmediate(d.world.bossMist.GetComponent<Collider>());Layer(d.world.bossMist,Phase2Lastri.VisibilityLayer);
            var mist=d.world.bossMist.GetComponent<ParticleSystem>();var main=mist.main;main.startColor=new Color(.5f,.025f,.015f,.4f);main.maxParticles=24;var shape=mist.shape;shape.scale=new Vector3(.7f,1.6f,.5f);d.world.bossMist.SetActive(false);
            d.world.illusions=new GameObject[2];
            for(int i=0;i<2;i++)
            {
                var copy=Object.Instantiate(d.world.lastri,environment);copy.name="Lastri_Illusion_"+i;Object.DestroyImmediate(copy.GetComponent<UnityEngine.AI.NavMeshAgent>());
                var mistCopy=copy.transform.Find("KabutSengkala");if(mistCopy)Object.DestroyImmediate(mistCopy.gameObject);
                foreach(var r in copy.GetComponentsInChildren<Renderer>())r.enabled=true;
                copy.SetActive(false);d.world.illusions[i]=copy;
            }
            d.world.flareTemplate=Point("FlareSource_CC0_Kenney",Vector3.zero).gameObject;
            var fire=d.world.flareTemplate.AddComponent<ParticleSystem>();main=fire.main;main.startLifetime=.6f;main.startSpeed=.35f;main.startSize=.23f;main.startColor=new Color(1,.12f,.025f,.8f);main.maxParticles=32;main.simulationSpace=ParticleSystemSimulationSpace.World;
            shape=fire.shape;shape.shapeType=ParticleSystemShapeType.Cone;shape.angle=10;shape.radius=.035f;shape.rotation=new Vector3(-90,0,0);
            var emission=fire.emission;emission.rateOverTime=26;
            var rendererFire=fire.GetComponent<ParticleSystemRenderer>();var fireMat=new Material(AssetDatabase.LoadAssetAtPath<Material>(Art+"Materials/OperationMist.mat"));
            string firePath=Art+"Materials/FlareFire.mat";var old=AssetDatabase.LoadAssetAtPath<Material>(firePath);if(old){Object.DestroyImmediate(fireMat);fireMat=old;}else AssetDatabase.CreateAsset(fireMat,firePath);
            fireMat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Imported+"KenneyParticles/Fire.png"));rendererFire.sharedMaterial=fireMat;
            var glow=Point("FlareGlow",Vector3.up*.2f,d.world.flareTemplate.transform).gameObject.AddComponent<Light>();glow.type=LightType.Point;glow.color=new Color(1,.13f,.035f);glow.intensity=5;glow.range=5;glow.shadows=LightShadows.None;
            d.world.flareTemplate.SetActive(false);
            var scooter=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx");var motor=(GameObject)PrefabUtility.InstantiatePrefab(scooter);motor.name="RakaMotor_RSUD_Exit";motor.transform.SetParent(environment,false);motor.transform.position=new Vector3(9,0,-11.8f);motor.transform.rotation=Quaternion.Euler(0,90,0);Item(motor,"EXIT_MOTOR","Tinggalkan RSUD",new Vector3(8.4f,1,-11.8f));
            DamarVideoSet();
        }
        [MenuItem("LayarTerakhir/Phase 2/Build Full Linux Review")]
        public static void BuildLinuxReview(){Build();GoldenLinuxBuild.Build();}
        private static void DamarVideoSet()
        {
            // Non-playable transmission set in the hospital scene, rendered only during the 8s video through the existing feed.
            d.world.videoSet=Point("Damar_Furnace_Transmission_Set",Vector3.zero).gameObject;
            var before=environment.Cast<Transform>().ToArray();
            var furnace=Prop("CoalFurnace_Cold","Ritual/Models/Furnace",new Vector3(83,4,18),new Vector3(4.8f,5.2f,3.5f),0,false);
            for(float x=76;x<90;x+=2)for(float z=12;z<24;z+=2)
            {var slab=M("Floor_tile_1",new Vector3(x+1,3.9f,z+1),new Vector3(2,.1f,2),0,false);foreach(var r in slab.GetComponentsInChildren<Renderer>())r.sharedMaterial=materials["metal_plate_02"];}
            var brick=Object.Instantiate(materials["red_brick_03"]);brick.name="Transmission_Brick";brick.SetTextureScale("_BaseMap",new Vector2(7,2.5f));brick.SetTextureScale("_BumpMap",new Vector2(7,2.5f));
            string brickPath=Art+"Materials/TransmissionBrick.mat";var oldBrick=AssetDatabase.LoadAssetAtPath<Material>(brickPath);if(oldBrick){Object.DestroyImmediate(brick);brick=oldBrick;}else AssetDatabase.CreateAsset(brick,brickPath);
            foreach(var wall in new[]{M("Tile_Wall",new Vector3(83,4,22),new Vector3(14,5.5f,.2f),0,false),M("Tile_Wall",new Vector3(76,4,17),new Vector3(10,5.5f,.2f),90,false),M("Tile_Wall",new Vector3(90,4,17),new Vector3(10,5.5f,.2f),90,false)})foreach(var r in wall.GetComponentsInChildren<Renderer>())r.sharedMaterial=brick;
            var actor=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Prefabs/DamarDesignerCharacter.prefab"));actor.transform.SetParent(environment,false);actor.name="Damar_Captive_Designer";actor.transform.position=new Vector3(80.5f,4,16.5f);actor.transform.rotation=Quaternion.Euler(0,180,0);
            var animation=actor.GetComponent<Animation>();if(animation){animation.playAutomatically=true;animation.clip=animation.GetClip("LowCrouch")??animation.GetClip("Crouch");if(animation.clip){animation.Play(animation.clip.name);animation.Sample();}}
            var rope=Prop("Damar_RopeBinding","Ritual/Models/Rope",new Vector3(80.5f,4.65f,16.5f),new Vector3(.68f,.075f,.52f),0,false);
            foreach(var r in rope.GetComponentsInChildren<Renderer>())r.sharedMaterial.SetColor("_BaseColor",new Color(.3f,.2f,.12f));
            PH("pull_chain_light_socket",new Vector3(80.8f,7.6f,15.5f),new Vector3(.2f,.5f,.2f),0,false);
            var lamp=Point("TransmissionColdLamp",new Vector3(80,6.5f,14)).gameObject.AddComponent<Light>();lamp.type=LightType.Spot;lamp.transform.LookAt(actor.transform.position+Vector3.up);lamp.color=new Color(.82f,.73f,.56f);lamp.intensity=20;lamp.range=10;lamp.spotAngle=65;lamp.shadows=LightShadows.Soft;
            foreach(var node in environment.Cast<Transform>().Where(t=>!before.Contains(t)&&t!=d.world.videoSet.transform).ToArray())node.SetParent(d.world.videoSet.transform,true);
            d.world.videoCamera=Point("Transmission_Lens",new Vector3(79.6f,5.05f,14.2f),d.world.videoSet.transform);d.world.videoCamera.LookAt(new Vector3(81.1f,4.85f,16.9f));
            d.world.videoSet.SetActive(false);
        }
    }
}
#endif
