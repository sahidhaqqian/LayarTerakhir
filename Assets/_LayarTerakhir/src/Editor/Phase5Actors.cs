#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LayarTerakhir.Phase5;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        private static void Actors()
        {
            d.replicas.actors=new GameObject[3];d.replicas.foci=new Transform[3];
            string[] ids={"P5_REPLICA_KUNTILANAK","P5_REPLICA_SRI_ATMAJA","P5_REPLICA_GENDERUWO"};
            Vector3[] positions={new Vector3(0,0,54),new Vector3(8,0,168),new Vector3(-8,0,175)};
            for(int i=0;i<3;i++)
            {
                var root=Point(ids[i],positions[i]).gameObject;
                var visual=i<2?AnimatedHuman("MemoryGhost",positions[i],1.8f,i==0?0:180,"Proyeksi memori beranimasi asli "+ids[i]):Model("JungleMemory",positions[i],2.5f,0,"Proyeksi memori Genderuwo yang tidak hidup kembali");
                visual.transform.SetParent(root.transform,true);d.replicas.actors[i]=root;
                d.replicas.foci[i]=Point("ReplicaFocus_"+i,positions[i]+Vector3.up*(i<2?1.55f:1.9f),root.transform);
                var agent=root.AddComponent<NavMeshAgent>();agent.height=i<2?1.8f:2.5f;agent.radius=.35f;agent.baseOffset=0;agent.speed=i==0?1.3f:i==1?1.4f:1.8f;agent.angularSpeed=180;agent.acceleration=6;
            }
            var boss=Point("Penonton_Terakhir_Sang_Dalang_Layar",new Vector3(0,0,228)).gameObject;
            var robe=Model("ScreenRobe",boss.transform.position,4,0,"Jubah layar Penonton_Terakhir");robe.transform.SetParent(boss.transform,true);
            for(int i=0;i<24;i++)
            {
                float angle=i*Mathf.PI*2/24;float height=.8f+(i%6)*.4f;
                var hand=Module("WitnessHand",boss.transform.position+new Vector3(Mathf.Cos(angle)*.7f,height,Mathf.Sin(angle)*.7f),new Vector3(.3f,.65f,.3f),"Tangan Penonton_Terakhir "+i,i*15,false);
                hand.transform.SetParent(boss.transform,true);hand.transform.Rotate(0,0,(i%2==0?1:-1)*25);
            }
            for(int i=0;i<3;i++)
            {
                var lens=Model("Camera_01",boss.transform.position+new Vector3((i-1)*.23f,3.25f+(i%2)*.25f,.05f),.7f,180+i*18,"Kepala tumpukan lensa "+i);lens.transform.SetParent(boss.transform,true);
            }
            d.world.bossRoot=boss;d.world.bossHead=Point("BossHeadCore",boss.transform.position+new Vector3(0,3.5f,-.25f),boss.transform);
            d.world.bossChest=Point("BossChestCore",boss.transform.position+new Vector3(0,2.1f,-.3f),boss.transform);
            var brain=boss.AddComponent<NavMeshAgent>();brain.height=3.8f;brain.radius=.55f;brain.speed=2.2f;brain.angularSpeed=180;brain.acceleration=5;
            d.world.clones=new GameObject[3];d.world.shadowSpawns=new Transform[3];
            for(int i=0;i<3;i++)
            {
                Vector3 at=new Vector3(i==0?-12:i==1?12:0,0,i==2?208:232);
                var clone=UnityEngine.Object.Instantiate(boss,at,Quaternion.identity,env);clone.name="P5_SHADOW_"+i;
                UnityEngine.Object.DestroyImmediate(clone.GetComponent<NavMeshAgent>());d.world.clones[i]=clone;d.world.shadowSpawns[i]=Point("ShadowSpawn_"+i,at);
            }
            d.world.damar=AnimatedHuman("DamarFinale",new Vector3(0,2.1f,223),1.75f,180,"Damar dalam kurungan dan saat dipapah");
            d.world.damarFocus=Point("DamarEndingFocus",new Vector3(0,3.5f,222.8f),d.world.damar.transform);
            d.world.damarInteraction=Item("DAMAR",new Vector3(0,1.4f,220.8f),"Panggil Damar / periksa kurungan",new Vector3(1.2f,1.6f,.6f)).transform;
            d.world.cage=Point("Damar_Lens_Cage",new Vector3(0,0,223)).gameObject;
            for(int i=0;i<8;i++)
            {
                float a=i*Mathf.PI*2/8;Vector3 at=new Vector3(Mathf.Sin(a)*1.8f,.4f,223+Mathf.Cos(a)*1.8f);
                var bar=Module("ChainlinkPanel",at,new Vector3(1.42f,3.7f,.045f),"Kurungan jaring lensa "+i,i*45);bar.transform.SetParent(d.world.cage.transform,true);
            }
            d.boss.threadMeshes=new Transform[4];
            for(int i=0;i<4;i++)d.boss.threadMeshes[i]=Thread("UV_Thread_"+i,new Vector3(0,3.5f,228),new Vector3(0,3.5f,223),i==0,24).transform;
            // Newly downloaded CC0 flame sheet by para; no authored sprite art.
            var wave=Point("Banaspati_Replica_Wave",new Vector3(-19,1,220)).gameObject;
            var particles=wave.AddComponent<ParticleSystem>();var main=particles.main;main.startLifetime=1.2f;main.startSpeed=.8f;main.startSize=1.2f;main.startColor=new Color(1,.25f,.04f,.6f);main.maxParticles=80;main.simulationSpace=ParticleSystemSimulationSpace.Local;
            var shape=particles.shape;shape.shapeType=ParticleSystemShapeType.Box;shape.scale=new Vector3(.3f,1.4f,26);
            var flame=NewMaterial("ReplicaFire","Universal Render Pipeline/Particles/Unlit");flame.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Imported+"VFX/FireSheet.png"));
            flame.SetColor("_BaseColor",Color.white);flame.SetFloat("_Surface",1);flame.SetFloat("_SrcBlend",5);flame.SetFloat("_DstBlend",10);flame.SetFloat("_ZWrite",0);flame.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");flame.renderQueue=3000;
            var sheet=particles.textureSheetAnimation;sheet.enabled=true;sheet.numTilesX=sheet.numTilesY=8;sheet.frameOverTime=new ParticleSystem.MinMaxCurve(1,AnimationCurve.Linear(0,0,1,1));sheet.cycleCount=1;
            var renderer=particles.GetComponent<ParticleSystemRenderer>();renderer.sharedMaterial=flame;d.boss.waveVisual=wave;wave.SetActive(false);
            var mechanical=env.gameObject.AddComponent<Phase5MechanicalPresentation>();mechanical.director=d;
        }
        private static GameObject AnimatedHuman(string id,Vector3 at,float height,float yaw,string purpose)
        {
            string path=Imported+"Animations/"+id+".fbx";
            var root=Place(id,path,at,Vector3.one*height,yaw,purpose,false,true);
            var animator=root.GetComponentsInChildren<Animator>().FirstOrDefault();if(!animator)return root;
            string source=Imported+"Animations/MemoryGhost.fbx";
            var clip=AssetDatabase.LoadAllAssetsAtPath(source).OfType<AnimationClip>().FirstOrDefault(c=>!c.name.StartsWith("__preview__"));
            if(!clip)throw new InvalidOperationException("New authored ghost animation missing");
            Directory.CreateDirectory(Art+"AnimationControllers");string controllerPath=Art+"AnimationControllers/"+id+".controller";
            var controller=AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if(!controller)controller=AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            var machine=controller.layers[0].stateMachine;var state=machine.states.Length>0?machine.states[0].state:machine.AddState("OriginalSourceMotion");machine.defaultState=state;
            if(id=="DamarFinale")
            {
                controller.parameters=new[]{new AnimatorControllerParameter{name="MoveSpeed",type=AnimatorControllerParameterType.Float}};
                var tree=AssetDatabase.LoadAllAssetsAtPath(controllerPath).OfType<BlendTree>().FirstOrDefault();
                if(!tree){tree=new BlendTree{name="AuthoredHumanIdleWalk"};AssetDatabase.AddObjectToAsset(tree,controller);}
                tree.blendType=BlendTreeType.Simple1D;tree.blendParameter="MoveSpeed";tree.useAutomaticThresholds=false;
                tree.children=new[]{new ChildMotion{motion=humanIdle,threshold=0,timeScale=1},new ChildMotion{motion=humanWalk,threshold=1.1f,timeScale=.85f}};
                state.name="AuthoredHumanLocomotion";state.motion=tree;state.speed=1;
                root.AddComponent<LayarTerakhir.Phase5.Phase5HumanMotion>().animator=animator;
            }
            else {state.motion=clip;state.speed=.7f;}
            animator.runtimeAnimatorController=controller;animator.applyRootMotion=false;return root;
        }
    }
}
#endif
