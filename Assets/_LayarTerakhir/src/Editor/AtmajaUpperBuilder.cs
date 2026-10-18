#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
using static LayarTerakhir.Editor.AtmajaSceneBuilder;

namespace LayarTerakhir.Editor
{
    // Adegan 4-7: stair pocket, upper floor (child/bath/master/corridor/balcony),
    // cellar U-room, chase modifications and ending vista. Runs inside the same
    // regeneration pass as the ground floor; approved Golden scenes untouched.
    public static class AtmajaUpperBuilder
    {
        private const string MatPath="Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/";
        private const float F2=3.82f, C2=6.83f, BF=-2.8f;
        public static void Build()
        {
            Materials();
            var systems=director.gameObject;
            director.upper=systems.AddComponent<AtmajaUpperDirector>();director.upper.director=director;
            director.basement=systems.AddComponent<AtmajaBasementHorror>();director.basement.director=director;director.basement.upper=director.upper;
            director.chase=systems.AddComponent<AtmajaChaseSequence>();director.chase.director=director;director.chase.basement=director.basement;
            director.basement.chase=director.chase;
            director.stairDoorLeaf=GameObject.Find("LockedCentralStairDoor");
            BuildStairs();BuildUpperShell();DressCorridor();DressChildRoom();DressBathroom();DressBalcony();DressMasterRoom();BuildBasement();BuildChaseMods();WireDirector();
        }
        private static Material Textured(string name,string tex,float rough,float metal=0)
        {
            var m=Plain(name,Color.white,1-rough);m.SetFloat("_Metallic",metal);
            m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Atmaja/Textures/"+tex+"_diff_1k.jpg"));
            var nor=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Atmaja/Textures/"+tex+"_nor_gl_1k.jpg");
            if(nor){m.SetTexture("_BumpMap",nor);m.SetFloat("_BumpScale",.55f);m.EnableKeyword("_NORMALMAP");}
            var arm=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Atmaja/Textures/"+tex+"_arm_1k.jpg");
            if(arm){var mask=AtmajaStairRevisionBuilder.UpperMask(arm);m.SetTexture("_MetallicGlossMap",mask);m.SetTexture("_OcclusionMap",mask);m.SetFloat("_Smoothness",1);m.SetFloat("_OcclusionStrength",.7f);m.EnableKeyword("_METALLICSPECGLOSSMAP");m.EnableKeyword("_OCCLUSIONMAP");}
            EditorUtility.SetDirty(m);return m;
        }
        private static Material Glow(string name,Color color,float intensity)
        {
            var m=Plain(name,color,.9f);m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",color*intensity);EditorUtility.SetDirty(m);return m;
        }
        private static void Materials()
        {
            Textured("Upper_WoodFloor","old_wood_floor",.72f);
            Textured("Upper_Wallpaper","decrepit_wallpaper",.8f);
            Textured("Upper_TileDirty","dirty_tiles",.5f);
            Textured("Upper_CeilingWood","ceiling_interior",.8f);
            Textured("Upper_BedFabric","quatrefoil_jacquard_fabric",.9f);
            Textured("Upper_PineTable","stained_pine",.6f);
            Textured("Upper_ConcretePaint","painted_concrete",.85f);
            var mirror=Plain("Upper_MirrorAged",new Color(.42f,.45f,.42f),.9f);mirror.SetFloat("_Metallic",.95f);EditorUtility.SetDirty(mirror);
            Plain("Upper_PortraitCanvas",new Color(.23f,.20f,.16f),.05f);
            var plaque=Plain("Upper_BrassPlaque",new Color(.45f,.33f,.13f),.5f);plaque.SetFloat("_Metallic",.8f);EditorUtility.SetDirty(plaque);
            Plain("Upper_CurtainDirty",new Color(.16f,.15f,.13f),.02f);
            Plain("Upper_BloodWater",new Color(.28f,.05f,.03f),.6f);
            var brass=Plain("Upper_ChandelierBrass",new Color(.38f,.28f,.12f),.5f);brass.SetFloat("_Metallic",.85f);EditorUtility.SetDirty(brass);
            Glow("Upper_ChandelierLamp",new Color(1,.85f,.6f),1.6f);
            Glow("Upper_TeplokFlame",new Color(1,.62f,.25f),2.2f);
            Glow("Upper_PowerbankLED",new Color(.2f,1,.4f),2f);
            Plain("Upper_BloodSmear",new Color(.35f,.02f,.01f),.1f);
            Plain("Upper_RedCarpet",new Color(.32f,.03f,.03f),.05f);
            Plain("Upper_PowerbankBody",new Color(.08f,.09f,.1f),.4f);
            Plain("Upper_BookCover",new Color(.3f,.08f,.06f),.1f);
            Plain("Upper_BookPaper",new Color(.62f,.56f,.44f),.05f);
            Plain("Upper_SteelRust",new Color(.25f,.12f,.07f),.3f);
            Plain("Upper_HandycamBody",new Color(.05f,.05f,.06f),.4f);
            // AR-only ink for the closed-eye Arsip Saksi foreshadowing; unlit so the dark house cannot swallow it.
            var ink=Plain("ARSecretInk",new Color(.6f,.02f,.01f),.24f);
            ink.shader=Shader.Find("Universal Render Pipeline/Unlit");ink.shaderKeywords=System.Array.Empty<string>();EditorUtility.SetDirty(ink);
        }
        private static void EyeSymbol(Transform parent,Material ink)
        {
            void Segment(string name,float x,float y,float length,float angle)
            {
                var seg=Box(name,parent.position,new Vector3(length,.011f,.018f),ink,false);
                seg.transform.SetParent(parent,true);seg.transform.localPosition=new Vector3(x,y,0f);seg.transform.localRotation=Quaternion.Euler(0,0,angle);
            }
            // A closed eye: arched upper lid, shallow lower lid and three lashes.
            Segment("EyeLidU1",-.031f,.010f,.037f,34);Segment("EyeLidU2",0,.019f,.035f,0);Segment("EyeLidU3",.031f,.010f,.037f,-34);
            Segment("EyeLidL1",-.026f,-.004f,.034f,-16);Segment("EyeLidL2",.026f,-.004f,.034f,16);
            Segment("EyeLash1",-.018f,-.015f,.021f,-58);Segment("EyeLash2",0,-.020f,.022f,-90);Segment("EyeLash3",.018f,-.015f,.021f,58);
        }
        private static GameObject Slab(string name,float x0,float x1,float d0,float d1,float y0,float y1,Material material)
        {
            var go=Box(name,P((x0+x1)/2,(d0+d1)/2,(y0+y1)/2),new Vector3(x1-x0,y1-y0,d1-d0),material,true);
            var local=new Material(material){name=name+"_Tiled"};local.SetTextureScale("_BaseMap",new Vector2((x1-x0)*.5f,(d1-d0)*.5f));
            string path=MatPath+name+"_Tiled.mat";var prior=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(prior){EditorUtility.CopySerialized(local,prior);Object.DestroyImmediate(local);local=prior;}else AssetDatabase.CreateAsset(local,path);
            go.GetComponent<Renderer>().sharedMaterial=local;return go;
        }
        private static GameObject WallX(string name,float x,float d0,float d1,float y0,float y1,Material material)=>Box(name,P(x,(d0+d1)/2,(y0+y1)/2),new Vector3(.16f,y1-y0,d1-d0),material);
        private static GameObject WallD(string name,float d,float x0,float x1,float y0,float y1,Material material)=>Box(name,P((x0+x1)/2,d,(y0+y1)/2),new Vector3(x1-x0,y1-y0,.16f),material);
        private static GameObject Existing(string id,string name,Vector3 center,Vector3 size,string matName=null)
        {
            var root=new GameObject(name);root.transform.SetParent(environment,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/"+id+"/"+id+".fbx"));
            model.transform.SetParent(root.transform,false);
            var mat=Mat(matName??"House_MAT_WoodDoor");
            foreach(var r in root.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(mat,r.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(root,size);KosVisualV2.SetCenter(root,center);FitCollider(root);return root;
        }
        private static GameObject ExistingMapped(string id,string name,Vector3 center,Vector3 size,params Material[] mats)
        {
            var root=new GameObject(name);root.transform.SetParent(environment,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/"+id+"/"+id+".fbx"));
            model.transform.SetParent(root.transform,false);
            foreach(var r in root.GetComponentsInChildren<Renderer>())
            {
                var current=r.sharedMaterials;var arr=new Material[current.Length];
                for(int i=0;i<current.Length;i++)arr[i]=mats[i%mats.Length];
                r.sharedMaterials=arr;
            }
            KosVisualV2.Resize(root,size);KosVisualV2.SetCenter(root,center);FitCollider(root);return root;
        }
        private static GameObject Original(string id,string name,Vector3 pos)
        {
            var go=new GameObject(name);go.transform.SetParent(environment,false);
            var instance=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Original/Models/"+id+".fbx"));instance.transform.SetParent(go.transform,false);
            foreach(var r in go.GetComponentsInChildren<Renderer>())
            {
                string meshName=r.GetComponent<MeshFilter>()?r.GetComponent<MeshFilter>().sharedMesh.name:r.name;
                string key=r.name.Contains("__")?r.name:meshName;
                string materialName=key.Substring(key.IndexOf("__",System.StringComparison.Ordinal)+2);
                var material=Mat(materialName)??Plain(materialName,new Color(.4f,.4f,.4f));
                r.sharedMaterials=Enumerable.Repeat(material,r.sharedMaterials.Length).ToArray();
            }
            go.transform.position=pos;return go;
        }
        private static GameObject Plaque(string name,string words,Vector3 pos,float scale,Color? tint=null)
        {
            var root=new GameObject(name,typeof(TextMesh));root.transform.SetParent(environment,false);root.transform.position=pos;
            var text=root.GetComponent<TextMesh>();text.text=words;text.font=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");text.fontSize=48;text.characterSize=scale/10;text.anchor=TextAnchor.MiddleCenter;text.alignment=TextAlignment.Center;text.color=tint??new Color(.72f,.68f,.52f);
            root.GetComponent<MeshRenderer>().sharedMaterial=text.font.material;return root;
        }
        private static void Layer(GameObject go,int layer){foreach(var t in go.GetComponentsInChildren<Transform>(true))t.gameObject.layer=layer;}
        private static Transform Anchor(string name,Vector3 pos,Vector3 look)
        {var t=new GameObject(name).transform;t.SetParent(director.transform,false);var direction=look-pos;direction.y=0;t.SetPositionAndRotation(pos,Facing(direction));return t;}
        private static Light LightAt(string name,Vector3 pos,LightType type,float power,float range,Color color,Vector3 look)
        {var go=new GameObject(name,typeof(Light));go.transform.SetParent(environment,false);go.transform.SetPositionAndRotation(pos,Facing(look-pos));var light=go.GetComponent<Light>();light.type=type;light.intensity=power;light.range=range;light.color=color;light.shadows=LightShadows.None;light.spotAngle=62;return light;}
        private static HingedDoor Door(string name,Vector3 pivot,Vector3 center,Vector3 size,string label,bool locked,float angle=-95)
        {
            var h=new GameObject(name+"Hinge").transform;h.SetParent(environment,false);h.position=pivot;var leaf=Box(name,center,size,Mat("House_MAT_WoodDoor"));leaf.transform.SetParent(h,true);var d=leaf.AddComponent<HingedDoor>();d.hinge=h;d.label=label;d.locked=locked;d.angle=angle;return d;
        }
        private static GameObject ChainLink(string name,Vector3 pos,float scaleX)
        {
            var mesh=AssetDatabase.LoadAssetAtPath<Mesh>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DoorChain.asset");
            var go=new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer));go.transform.SetParent(environment,false);
            go.transform.SetPositionAndRotation(pos,Facing(north));go.transform.localScale=new Vector3(scaleX,1,1);
            go.GetComponent<MeshFilter>().sharedMesh=mesh;go.GetComponent<MeshRenderer>().sharedMaterial=Mat("House_MAT_RustMetal");return go;
        }
        private static void SmearCluster(Vector3 pos,Vector3 normal,Vector3 right)
        {
            var mat=Mat("Upper_BloodSmear");var up=Vector3.Cross(normal,right).normalized;
            var palm=Box("BloodHandprint",pos,new Vector3(.09f,.11f,.012f),mat,false);
            palm.transform.rotation=Quaternion.LookRotation(normal,up);
            for(int f=0;f<4;f++)
            {
                var finger=Box("BloodFinger",pos+up*(.10f+f*.004f)+right*((f-1.5f)*.028f),new Vector3(.018f,.055f,.012f),mat,false);
                finger.transform.rotation=Quaternion.LookRotation(normal,up);
            }
        }
        // Solid stepped mass. ascendEast=true climbs toward +x, false toward -x.
        private static void StairMass(float x0,float x1,float d0,float d1,float yLow,float yHigh,int steps,Material mat,bool ascendEast=true)
        {
            float w=(x1-x0)/steps,rise=(yHigh-yLow)/steps;
            for(int i=0;i<steps;i++)
            {
                float top=ascendEast?yLow+(i+1)*rise:yHigh-i*rise;
                float yGround=yLow-.05f;
                Box("StairStep",P(x0+(i+.5f)*w,(d0+d1)/2,(yGround+top)/2),new Vector3(w,top-yGround,d1-d0),mat);
            }
            foreach(float d in new[]{d0-.06f,d1+.06f})
                for(int i=0;i<steps;i++)
                {
                    // Leave a real opening from the half landing into the return flight.
                    if(!ascendEast&&i>=steps-2&&d>d1)continue;
                    float top=ascendEast?yLow+(i+1)*rise:yHigh-i*rise;
                    Box("StairParapet",P(x0+(i+.5f)*w,d,top+.45f),new Vector3(w+.02f,.9f,.12f),mat);
                }
        }
        private static void GuardRail(float x0,float x1,float d,float y,float height=.95f)
        {
            var mat=Mat("House_MAT_WoodDoor");
            for(float x=x0+.15f;x<x1;x+=.45f)Box("RailBaluster",P(x,d,y+height/2),new Vector3(.05f,height,.05f),mat);
            Box("RailTop",P((x0+x1)/2,d,y+height),new Vector3(x1-x0,.07f,.09f),mat);
        }
        private static void GuardRailD(float d0,float d1,float x,float y,float height=.95f)
        {
            var mat=Mat("House_MAT_WoodDoor");
            for(float d=d0+.15f;d<d1;d+=.45f)Box("RailBaluster",P(x,d,y+height/2),new Vector3(.05f,height,.05f),mat);
            Box("RailTop",P(x,(d0+d1)/2,y+height),new Vector3(.09f,.07f,d1-d0),mat);
        }
        private static void BuildStairs()
        {
            var wood=Mat("House_MAT_WoodDoor");
            // The old ground-floor storage divider crossed both stair flights.
            // Keep its room-side stub, leaving the authored stairwell genuinely open.
            var storage=GameObject.Find("StorageWall");
            if(storage){storage.transform.position=P(-.15f,5.625f,2.05f);storage.transform.localScale=new Vector3(.16f,3.1f,.35f);}
            // Give both flights full capsule width without stealing the upper corridor floor.
            foreach(string wall in new[]{"StairHallWall_W","StairHallWall_E","StairHallLintel"})
            {var go=GameObject.Find(wall);if(go){var p=director.LocalPosition(go.transform.position);go.transform.position=P(p.x,8.85f,go.transform.position.y-director.origin.y);}}
            Box("StairEntryPlatform",P(-3.55f,8.1f,.53f),new Vector3(1.5f,.1f,1.6f),wood);
            Box("StairEntryWestWall",P(-4.32f,7.85f,2.1f),new Vector3(.16f,3.2f,2.0f),wood);
            Box("StairEntryEastWall",P(-2.8f,7.225f,2.05f),new Vector3(.16f,3.1f,.75f),wood);
            foreach(float x in new[]{-4.23f,-2.87f})Box("StairDoorJamb",P(x,7.7f,2.1f),new Vector3(.18f,3.2f,.16f),wood);
            Box("StairEntryDoorLintel",P(-3.55f,7.7f,3.4f),new Vector3(1.5f,.6f,.16f),wood);
            StairMass(-2.8f,.4f,7.6f,8.65f,.55f,2.2f,8,wood,true);
            Box("StairMidLanding",P(1.0f,7.55f,1.35f),new Vector3(1.2f,1.7f,2.3f),wood);
            StairMass(-2.6f,1.4f,6.4f,7.45f,2.2f,3.82f,8,wood,false);
            Box("StairTopLanding",P(-3.3f,6.925f,3.75f),new Vector3(1.4f,.14f,1.25f),wood);
            GuardRailD(6.4f,8.7f,1.66f,2.2f);GuardRail(.4f,1.6f,8.7f,2.2f);
            GuardRail(-4.0f,-2.6f,7.57f,3.82f);
            for(int i=0;i<7;i++)SmearCluster(P(-2.6f+i*.42f,8.765f,.9f+i*.24f),-north,east);
            for(int i=0;i<5;i++)SmearCluster(P(1.2f-i*.42f,6.405f,2.5f+i*.22f),north,-east);
            var lamp=Existing("Lantern_01","StairMidLantern",P(1.0f,7.55f,5.55f),new Vector3(.3f,.42f,.3f),"Upper_ChandelierBrass");
            Box("StairLanternCord",P(1.0f,7.55f,6.3f),new Vector3(.03f,1.15f,.03f),wood,false);
            LightAt("StairMidLight",P(1.0f,7.55f,5.36f),LightType.Point,1.1f,6,new Color(.9f,.75f,.5f),P(1.0f,7.0f,1.5f));
            LightAt("StairTopLight",P(-3.25f,6.925f,4.6f),LightType.Point,1.1f,6,new Color(.9f,.75f,.5f),P(-3.25f,5.5f,3.8f));
            director.upper.spawnUpper=Anchor("Spawn_UpperLanding",P(-3.25f,6.5f,3.83f),P(0,5.75f,4.5f));
        }
        private static void BuildUpperShell()
        {
            var plaster=Mat("House_MAT_PlasterDamaged");var concrete=Mat("House_MAT_Concrete");
            var woodFloor=Mat("Upper_WoodFloor");var ceiling=Mat("Upper_CeilingWood");
            Slab("UpperSlab_West",-5.85f,-4.0f,.05f,8.95f,3.68f,3.82f,concrete);
            Slab("UpperSlab_South",-4.0f,.55f,.05f,5.8f,3.68f,3.82f,concrete);
            Slab("UpperSlab_CorridorLip",-4.0f,1.7f,5.8f,6.3f,3.68f,3.82f,concrete);
            Slab("UpperSlab_TrapS",.55f,1.7f,.05f,.55f,3.68f,3.82f,concrete);
            Slab("UpperSlab_TrapN",.55f,1.7f,2.25f,5.8f,3.68f,3.82f,concrete);
            Slab("UpperSlab_Mid",1.7f,2.4f,.05f,8.95f,3.68f,3.82f,concrete);
            Slab("UpperSlab_VoidS",2.4f,5.0f,.05f,3.4f,3.68f,3.82f,concrete);
            Slab("UpperSlab_VoidN",2.4f,5.0f,5.2f,8.95f,3.68f,3.82f,concrete);
            Slab("UpperSlab_East",5.0f,5.85f,.05f,8.95f,3.68f,3.82f,concrete);
            Slab("BalconyDeck",2.4f,5.0f,3.4f,5.2f,3.68f,3.82f,concrete);
            Slab("BalconyWalk",2.4f,5.0f,5.2f,6.3f,3.68f,3.82f,concrete);
            Slab("UpperFloor_West",-5.85f,-4.0f,.05f,8.95f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_South",-4.0f,.55f,.05f,5.8f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_CorridorLip",-4.0f,1.7f,5.8f,6.3f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_TrapS",.55f,1.7f,.05f,.55f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_TrapN",.55f,1.7f,2.25f,5.8f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_Mid",1.7f,2.4f,.05f,8.95f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_VoidS",2.4f,5.0f,.05f,3.4f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_VoidN",2.4f,5.0f,5.2f,8.95f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_East",5.0f,5.85f,.05f,8.95f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_Deck",2.4f,5.0f,3.4f,5.2f,3.795f,3.82f,woodFloor);
            Slab("UpperFloor_Walk",2.4f,5.0f,5.2f,6.3f,3.795f,3.82f,woodFloor);
            Box("UpperCeiling",P(0,4.5f,6.9f),new Vector3(11.7f,.14f,8.9f),ceiling);
            WallD("CorrWallN_A",5.2f,-6,-4.9f,F2,C2,plaster);
            WallD("CorrWallN_B",5.2f,-3.9f,-3.45f,F2,C2,plaster);
            WallD("CorrWallN_B_Right",5.2f,-2.55f,-1.55f,F2,C2,plaster);
            WallD("VentOpeningHeader",5.2f,-3.45f,-2.55f,4.9f,C2,plaster);
            WallD("CorrWallN_C",5.2f,-.65f,.75f,F2,C2,plaster);
            WallD("CorrWallN_D",5.2f,1.85f,2.4f,F2,C2,plaster);
            foreach(float c in new[]{4.4f,1.1f})WallD("CorrLintelN",5.2f,c-.5f,c+.5f,5.95f,C2,plaster);
            WallD("CorrLintelMaster",5.2f,.75f,1.85f,5.95f,C2,plaster);
            WallD("CorrWallS_A",6.3f,-6,-4.0f,F2,C2,plaster);
            WallD("CorrWallS_B",6.3f,-2.5f,2.4f,F2,C2,plaster);
            Box("CorridorRunner",P(-1.8f,5.75f,3.825f),new Vector3(8.0f,.012f,.8f),Mat("Upper_RedCarpet"),false);
            WallX("ChildBathWall",-2.4f,0,5.2f,F2,C2,plaster);
            WallX("BathMasterWall",.2f,0,5.2f,F2,C2,plaster);
            WallX("MasterEastWall",2.4f,0,5.2f,F2,C2,plaster);
            WallD("RoomNorthCladding",.08f,-6,2.4f,F2,C2,plaster);
        }
        private static void Portrait(string id,string name,Vector3 center,Vector3 size,string plaqueText,Vector3 plaquePos,GameObject[] bloodSlot,int bloodIndex,string arText=null)
        {
            var frame=ExistingMapped(id,name,center,size,Mat("House_MAT_WoodDoor"),Mat("Upper_PortraitCanvas"));
            frame.transform.rotation=Facing(-north);
            var plaque=Plaque(name+"_Plaque",plaqueText,plaquePos,.30f,new Color(.55f,.42f,.18f));
            plaque.transform.rotation=Facing(-north);
            Box(name+"_PlaqueBack",plaquePos+north*.012f,new Vector3(size.x*.8f,.16f,.02f),Mat("Upper_BrassPlaque"));
            for(int i=0;i<3;i++)
                Box(name+"_Gouge"+i,center-north*.05f+east*((i-1)*.09f)+Vector3.up*.02f,new Vector3(.035f,size.y*.5f,.02f),Mat("Upper_BookPaper"),false);
            var blood=Plaque(name+"_BloodAR",arText??"DIA MELIHAT\nKAU MEMOTRET",center-north*.1f+Vector3.up*-.55f,.22f,new Color(.5f,.02f,.01f));
            blood.transform.rotation=Facing(-north);
            Layer(blood,AtmajaPhoneView.ARLayer);blood.SetActive(false);bloodSlot[bloodIndex]=blood;
            if(arText!=null)
            {
                // GDD foreshadowing: closed-eye Arsip Saksi mark behind the third frame, AR-visible only.
                var ink=Mat("ARSecretInk");
                if(ink)
                {
                    var eye=new GameObject(name+"_ClosedEye");eye.transform.SetParent(blood.transform,false);
                    eye.transform.localPosition=new Vector3(0,.16f,-.004f);EyeSymbol(eye.transform,ink);Layer(eye,AtmajaPhoneView.ARLayer);
                }
            }
        }
        private static void DressCorridor()
        {
            var blood=new GameObject[3];director.upper.portraitBloodAR=blood;
            Portrait("hanging_picture_frame_01","PortraitDosa1",P(-2.5f,5.10f,5.35f),new Vector3(.62f,.78f,.07f),"DOSA PERTAMA — 19:15\nRACUN DI DALAM KASIH",P(-2.5f,5.10f,4.82f),blood,0);
            Portrait("fancy_picture_frame_01","PortraitDosa2",P(.1f,5.10f,5.35f),new Vector3(.62f,.78f,.07f),"DOSA KEDUA — 21:40\nCAKARAN DI BALIK AIR",P(.1f,5.10f,4.82f),blood,1);
            Portrait("hanging_picture_frame_03","PortraitDosa3",P(2.12f,5.10f,5.35f),new Vector3(.5f,.66f,.07f),"DOSA KETIGA — 23:59\nTALI PENUTUP USIA",P(2.12f,5.10f,4.88f),blood,2,"SAKSI MENYIMPAN NAMA\nPENONTON HANYA\nMENYIMPAN WAJAH");
            foreach(float x in new[]{-4.6f,.9f})
            {
                var lamp=Existing("Lantern_01","CorridorLantern",P(x,5.75f,6.1f),new Vector3(.3f,.42f,.3f),"Upper_ChandelierBrass");
                lamp.tag="AtmajaChaseLamp";
                Box("CorridorLampCord",P(x,5.75f,6.5f),new Vector3(.03f,.4f,.03f),Mat("House_MAT_WoodDoor"),false);
                LightAt("CorridorLight",P(x,5.75f,5.95f),LightType.Point,1.2f,7,new Color(.95f,.82f,.6f),P(x,5.75f,3.9f));
            }
            Box("CorridorCobweb",P(-5.6f,5.9f,6.4f),new Vector3(.7f,.02f,.5f),Mat("Upper_CurtainDirty"),false);
        }
        private static void DressChildRoom()
        {
            var wallpaper=Mat("Upper_Wallpaper");var plaster=Mat("House_MAT_PlasterDamaged");
            Box("ChildWallpaperWest",P(-5.9f,2.6f,5.3f),new Vector3(.03f,3.0f,5.2f),wallpaper,false);
            var bed=Existing("old_bed_frame","ChildBed",P(-3.5f,2.8f,4.35f),new Vector3(1.0f,1.9f,1.06f));
            Box("ChildMattress",P(-3.5f,2.8f,4.62f),new Vector3(.92f,1.8f,.22f),Mat("Upper_BedFabric"));
            var pillow=Existing("throw_pillows_01","ChildPillow",P(-3.5f,2.15f,4.78f),new Vector3(.5f,.34f,.16f),"Upper_BedFabric");
            Box("ChildBlanket",P(-3.5f,3.25f,4.78f),new Vector3(.94f,.9f,.1f),Mat("Upper_RedCarpet"),false);
            Box("ChildDeskTop",P(-5.3f,.55f,4.52f),new Vector3(1.2f,.08f,.6f),Mat("Upper_PineTable"));
            foreach(float x in new[]{-5.8f,-4.8f})foreach(float z in new[]{.35f,.75f})Box("ChildDeskLeg",P(x,z,4.17f),new Vector3(.07f,.7f,.07f),Mat("House_MAT_WoodDoor"));
            var chair=Existing("painted_wooden_chair_01","ChildStudyChair",P(-5.3f,1.35f,4.2f),new Vector3(.42f,.85f,.42f));
            chair.transform.rotation=Facing(-north);
            var lampArm=Existing("desk_lamp_arm_01","ChildDeskLamp",P(-5.6f,.5f,4.62f),new Vector3(.22f,.4f,.22f));
            var notebook=Existing("binder_notebook","ChildNotebook",P(-5.15f,.55f,4.6f),new Vector3(.3f,.22f,.04f));
            Box("ChildMilkCup",P(-5.0f,.62f,4.62f),new Vector3(.07f,.09f,.07f),Plain("ChildCupWhite",new Color(.7f,.68f,.62f),.4f));
            Item(GameObject.Find("ChildMilkCup"),AtmajaAction.CupPhoto,"Periksa cangkir susu retak");
            director.upper.childCup=GameObject.Find("ChildMilkCup");
            director.upper.cupAim=Anchor("CupAim",P(-5.0f,.62f,4.66f),P(-5.0f,2,4.4f));
            Box("ChildDiaryBook",P(-5.32f,.62f,4.61f),new Vector3(.22f,.05f,.3f),Mat("Upper_BookCover"));
            Item(GameObject.Find("ChildDiaryBook"),AtmajaAction.Diary,"Baca buku harian Bima");
            var shelf=Existing("painted_wooden_shelves","ChildShelf",P(-3.3f,.30f,5.6f),new Vector3(1.2f,.9f,.28f));
            var books=ExistingMapped("book_encyclopedia_set_01","ChildBooks",P(-3.3f,.30f,5.62f),new Vector3(.8f,.3f,.2f),Mat("Upper_BookCover"),Mat("Upper_BookPaper"));
            var cabinet=Existing("painted_wooden_cabinet_02","ChildWardrobe",P(-2.85f,1.6f,4.45f),new Vector3(.95f,1.25f,.45f));
            Box("ChildToyBlocks",P(-3.9f,3.1f,3.92f),new Vector3(.3f,.12f,.3f),Mat("Upper_BookCover"),false);
            Box("ChildToyBall",P(-4.4f,2.4f,3.95f),new Vector3(.16f,.16f,.16f),Mat("Upper_RedCarpet"),false);
            Box("ChildCarpet",P(-4.3f,2.6f,3.83f),new Vector3(1.6f,.03f,2.2f),Mat("Upper_RedCarpet"),false);
            director.upper.childDoor=Door("ChildBedroomDoor",P(-4.9f,5.2f,F2),P(-4.4f,5.2f,F2+1.05f),new Vector3(1.0f,2.1f,.09f),"Pintu kamar anak (gagang rusak)",true);
            var holeBack=Box("VentHoleDark",P(-3.0f,5.2f,4.5f),new Vector3(.7f,.9f,.1f),Mat("Upper_CurtainDirty"),false);
            var ventAR=Plaque("VentHoleAR","LUBANG VENTILASI\nJEBOL — MERANGKAK\nDENGAN AR MENYALA",P(-3.0f,5.45f,4.5f),.20f,new Color(.5f,.02f,.01f));
            ventAR.transform.rotation=Facing(-north);
            Layer(ventAR,AtmajaPhoneView.ARLayer);ventAR.SetActive(false);director.upper.ventHoleAR=ventAR;
            var gate=Box("VentPassGate",P(-3.0f,5.2f,4.4f),new Vector3(.9f,1.2f,.12f),plaster);gate.GetComponent<Renderer>().enabled=false;
            director.upper.ventGate=gate.GetComponent<Collider>();
            director.upper.ventMouth=Anchor("VentMouth",P(-3.0f,5.6f,4.4f),P(-3.0f,4.5f,4.4f));
            var cabinetCover=Existing("painted_wooden_cabinet_02","VentCabinet",P(-3.0f,4.95f,4.445f),new Vector3(.95f,1.25f,.45f));
            director.upper.ventCabinet=cabinetCover;
            LightAt("ChildRoomLight",P(-4.3f,2.6f,6.2f),LightType.Point,1.1f,7,new Color(.95f,.85f,.65f),P(-4.3f,2.6f,3.9f));
        }
        private static void DressBathroom()
        {
            var tile=Mat("Upper_TileDirty");var plaster=Mat("House_MAT_PlasterDamaged");
            Slab("BathFloor",-2.4f,.2f,.05f,5.15f,3.795f,3.82f,tile);
            Box("BathWallTileW",P(-2.31f,2.6f,4.6f),new Vector3(.03f,1.6f,5.2f),tile,false);
            var tub=Original("UpperBathtub","BathTub",P(-1.1f,.8f,3.83f));Ground(tub,3.83f);FitCollider(tub);
            var curtain=Box("BathCurtain",P(-1.1f,.8f,4.55f),new Vector3(1.6f,.02f,.7f),Mat("Upper_CurtainDirty"));
            director.upper.bathCurtain=curtain;
            Item(curtain,AtmajaAction.BathCurtain,"Buka tirai bak mandi");
            var water=Box("BathBloodyWater",P(-1.1f,.8f,4.15f),new Vector3(1.34f,.56f,.06f),Mat("Upper_BloodWater"),false);
            director.upper.bathWater=water;
            var sink=Original("UpperSinkPedestal","BathSink",P(-.05f,4.5f,3.83f));Ground(sink,3.83f);FitCollider(sink);
            var mirror=Existing("ornate_mirror_01","BathMirror",P(.1f,4.5f,5.0f),new Vector3(.6f,.8f,.08f),"Upper_MirrorAged");
            mirror.transform.rotation=Facing(-east);
            director.upper.mirrorSpot=Anchor("BathMirrorSpot",P(-.7f,4.6f,4.75f),P(-.7f,5.5f,4.4f));
            var figure=Original("UpperStandingNyonya","BathMirrorApparition",P(-.7f,4.6f,3.83f));figure.transform.rotation=Facing(east);figure.SetActive(false);
            director.upper.mirrorFlashFigure=figure;
            var pot=Existing("pot_enamel_01","BathEnamelPot",P(-2.0f,4.6f,4.0f),new Vector3(.4f,.3f,.4f),"Upper_EnamelWhite");
            Box("BathTowelRail",P(.05f,2.6f,5.3f),new Vector3(.03f,.5f,.05f),Mat("Upper_SteelRust"));
            Box("BathTowel",P(.0f,2.6f,5.05f),new Vector3(.06f,.44f,.4f),Mat("Upper_CurtainDirty"),false);
            Box("BathBucket",P(-2.1f,3.4f,4.0f),new Vector3(.3f,.35f,.3f),Mat("House_MAT_WoodDoor"));
            director.upper.bathDoor=Door("BathroomDoor",P(-1.55f,5.2f,F2),P(-1.1f,5.2f,F2+1.0f),new Vector3(.9f,2.0f,.09f),"Buka kamar mandi",false);
            var scratches=Plaque("BathScratchesAR","CAKARAN JARI\nKECIL BERDARAH",P(-1.1f,5.05f,4.7f),.24f,new Color(.5f,.02f,.01f));
            scratches.transform.rotation=Facing(north);
            Layer(scratches,AtmajaPhoneView.ARLayer);scratches.SetActive(false);director.upper.bathScratchesAR=scratches;
            var scratchBox=Box("BathScratchTarget",P(-1.1f,5.28f,4.7f),new Vector3(.5f,.6f,.05f),plaster,true);
            scratchBox.GetComponent<Renderer>().enabled=false;
            Item(scratchBox,AtmajaAction.BathScratches,"Periksa bekas cakaran di pintu");
            director.upper.scratchAim=Anchor("ScratchAim",P(-1.1f,5.05f,4.7f),P(-1.1f,6.2f,4.9f));
            LightAt("BathLight",P(-1.1f,2.6f,6.2f),LightType.Point,.9f,6,new Color(.8f,.9f,.95f),P(-1.1f,2.6f,3.9f));
        }
        private static void DressBalcony()
        {
            var wood=Mat("House_MAT_WoodDoor");
            GuardRail(2.4f,5.0f,3.4f,F2);
            GuardRailD(3.4f,5.2f,5.0f,F2);GuardRailD(3.4f,5.2f,2.4f,F2);
            var railGuardS=Box("BalconyVoidGuardS",P(3.7f,3.4f,5.4f),new Vector3(2.6f,2.4f,.1f),wood);railGuardS.GetComponent<Renderer>().enabled=false;
            var railGuardE=Box("BalconyVoidGuardE",P(5.0f,4.3f,5.4f),new Vector3(.1f,2.4f,1.8f),wood);railGuardE.GetComponent<Renderer>().enabled=false;
            var railGuardW=Box("BalconyVoidGuardW",P(2.4f,4.3f,5.4f),new Vector3(.1f,2.4f,1.8f),wood);railGuardW.GetComponent<Renderer>().enabled=false;
            Box("VoidBeam",P(3.7f,4.9f,6.8f),new Vector3(2.6f,.2f,.18f),wood);
            var chandelier=ExistingMapped("Chandelier_01","VoidChandelier",P(3.7f,4.9f,5.95f),new Vector3(.8f,1.4f,.8f),Mat("Upper_ChandelierBrass"),Mat("Upper_ChandelierLamp"));
            LightAt("ChandelierLight",P(3.7f,4.9f,5.3f),LightType.Point,1.6f,9,new Color(1,.82f,.6f),P(3.7f,4.9f,1.4f));
            var pivot=new GameObject("RopePivot").transform;pivot.SetParent(environment,false);pivot.position=P(2.75f,4.9f,6.68f);
            var rope=GameObject.CreatePrimitive(PrimitiveType.Cylinder);rope.name="HangingRope";rope.transform.SetParent(pivot,false);
            rope.transform.localPosition=new Vector3(0,-1.05f,0);rope.transform.localScale=new Vector3(.035f,2.1f,.035f);
            rope.GetComponent<Renderer>().sharedMaterial=Mat("Upper_OldRope");
            var knot=Box("RopeKnot",pivot.position+new Vector3(0,-2.0f,0),new Vector3(.09f,.07f,.09f),Mat("Upper_OldRope"),false);
            knot.transform.SetParent(pivot,false);knot.transform.localPosition=new Vector3(0,-2.0f,0);
            FitCollider(rope);
            director.upper.ropePivot=pivot;
            director.upper.ropeAim=Anchor("RopeAim",P(2.75f,4.9f,5.0f),P(2.75f,4.9f,4.4f));
            var hanged=Original("UpperHangedNyonya","HangedApparition",P(2.75f,4.9f,4.35f));hanged.transform.rotation=Facing(north);hanged.SetActive(false);
            director.upper.hangedFlashFigure=hanged;
            director.upper.ropeSpot=Anchor("RopeSpot",P(2.75f,4.9f,4.6f),P(2.75f,4.9f,4.4f));
            Box("WillLetterPaper",P(3.4f,3.45f,4.82f),new Vector3(.3f,.02f,.4f),Mat("Upper_BookPaper"));
            Item(GameObject.Find("WillLetterPaper"),AtmajaAction.WillLetter,"Baca surat wasiat Nyonya");
            var rocker=Existing("Rockingchair_01","BalconyRocker",P(4.5f,4.6f,4.32f),new Vector3(.6f,1.0f,.9f));
            rocker.transform.rotation=Facing(-east);
            LightAt("BalconyLight",P(3.7f,4.6f,6.2f),LightType.Point,1.0f,6,new Color(.95f,.85f,.65f),P(3.7f,4.6f,3.9f));
        }
        private static void DressMasterRoom()
        {
            var bed=Existing("old_bed_frame","MasterBed",P(1.35f,3.45f,4.35f),new Vector3(1.9f,1.0f,1.06f));
            Box("MasterMattress",P(1.35f,3.45f,4.62f),new Vector3(1.8f,.9f,.22f),Mat("Upper_BedFabric"));
            var pillow=Existing("throw_pillows_01","MasterPillow",P(1.35f,3.05f,4.78f),new Vector3(.55f,.36f,.16f),"Upper_BedFabric");
            var night=Existing("painted_wooden_nightstand","MasterNightstand",P(.65f,3.0f,4.3f),new Vector3(.45f,.6f,.45f));
            var alarm=Existing("alarm_clock_01","MasterAlarmClock",P(.65f,3.0f,4.68f),new Vector3(.18f,.2f,.1f),"Upper_IronBlack");
            var dresser=Existing("vintage_wooden_drawer_01","MasterDresser",P(2.05f,4.7f,4.45f),new Vector3(.5f,.9f,.5f));
            var candle=Existing("wooden_candlestick","MasterCandle",P(2.05f,4.7f,4.85f),new Vector3(.12f,.3f,.12f));
            var rocker=Existing("Rockingchair_01","MasterRocker",P(2.0f,2.35f,4.32f),new Vector3(.6f,1.0f,.9f));
            rocker.transform.rotation=Facing(east);
            var portrait=ExistingMapped("fancy_picture_frame_02","MasterPortrait",P(2.31f,2.6f,5.4f),new Vector3(.5f,.7f,.06f),Mat("House_MAT_WoodDoor"),Mat("Upper_PortraitCanvas"));
            portrait.transform.rotation=Facing(-east);
            var chest=Existing("metal_tool_chest","MasterToolChest",P(.4f,4.9f,4.05f),new Vector3(.6f,.4f,.35f),"Upper_SteelRust");
            var carpet=Box("MasterRolledCarpet",P(1.0f,1.4f,3.88f),new Vector3(1.2f,.5f,.16f),Mat("Upper_RedCarpet"));
            director.upper.carpetRoll=carpet;
            Item(carpet,AtmajaAction.CarpetLift,"Angkat gulungan karpet merah");
            var lid=Original("UpperTrapdoorSlab","MasterTrapdoorLid",P(1.0f,1.4f,3.83f));
            FitCollider(lid);Item(lid,AtmajaAction.Trapdoor,"Buka pintu jebakan kayu");
            director.upper.trapdoorLid=lid;
            director.upper.trapdoorTop=Anchor("TrapdoorTop",P(1.0f,.95f,3.83f),P(1.0f,1.95f,3.83f));
            var stone=Mat("House_MAT_Concrete");
            for(int i=0;i<8;i++)
            {
                float top=F2-(i+1)*(F2-BF-.02f)/8;
                Box("CellarStep",P(1.02f,2.1f-i*.2f,(top+BF)/2+.02f),new Vector3(.9f,top-BF,.2f),stone);
            }
            WallX("CellarShaftW",.55f,.55f,2.25f,-.35f,F2+.1f,stone);
            WallX("CellarShaftE",1.5f,.55f,2.25f,-.35f,F2+.1f,stone);
            director.upper.masterDoor=Door("MasterBedroomDoor",P(.75f,5.2f,F2),P(1.3f,5.2f,F2+1.1f),new Vector3(1.1f,2.2f,.1f),"Kamar Tidur Utama (disegel)",true,-95);
            // A thin Javanese tembang hum drifts through the sealed door until the seal breaks.
            var song=new GameObject("MasterDoorSong",typeof(AudioSource));song.transform.SetParent(environment,false);song.transform.position=P(1.3f,5.6f,F2+1.2f);
            var songSource=song.GetComponent<AudioSource>();songSource.playOnAwake=true;songSource.loop=true;songSource.spatialBlend=1;songSource.minDistance=1.5f;songSource.maxDistance=10;songSource.rolloffMode=AudioRolloffMode.Linear;songSource.outputAudioMixerGroup=director.audioProfile.effectsBus;
            songSource.clip=AtmajaSynth.Melody(new[]{233.1f,207.7f,196f,174.6f,196f,207.7f},1.35f,.11f);songSource.volume=.32f;
            director.upper.doorSong=songSource;
            for(int i=0;i<3;i++)
            {
                var link=ChainLink("MasterSealChain"+i,P(1.3f,5.06f,4.7f+i*.65f),.92f);
                director.upper.chainLinks[i]=link;
            }
            LightAt("MasterRoomLight",P(1.3f,3.0f,6.2f),LightType.Point,1.1f,7,new Color(.95f,.85f,.65f),P(1.3f,3.0f,3.9f));
        }
        private static void BuildBasement()
        {
            var concrete=Mat("Upper_ConcretePaint");var stone=Mat("House_MAT_Concrete");
            Slab("CellarFloor",.2f,5.6f,.6f,6.9f,BF-.025f,BF,Mat("concrete_floor_damaged_01"));
            WallX("CellarWallW",.2f,.6f,6.9f,BF,-.35f,concrete);
            WallX("CellarWallE",5.6f,.6f,6.9f,BF,-.35f,concrete);
            WallD("CellarWallN",.6f,.2f,5.6f,BF,-.35f,concrete);
            WallD("CellarWallS",6.9f,.2f,5.6f,BF,-.35f,concrete);
            Box("CellarCeiling",P(2.9f,3.75f,0),new Vector3(5.4f,1.0f,6.3f),concrete);
            var rackN=Existing("steel_frame_shelves_02","ArchiveRackN",P(2.9f,2.0f,BF+.9f),new Vector3(1.8f,1.8f,.45f),"Upper_SteelRust");
            var rackS=Existing("steel_frame_shelves_02","ArchiveRackS",P(2.9f,3.2f,BF+.9f),new Vector3(1.8f,1.8f,.45f),"Upper_SteelRust");
            var archBooks=ExistingMapped("book_encyclopedia_set_01","ArchiveBooks",P(2.9f,2.6f,BF+1.1f),new Vector3(1.5f,.5f,.3f),Mat("Upper_BookCover"),Mat("Upper_BookPaper"));
            Existing("wooden_crate_01","CellarCrateA",P(.7f,4.9f,BF+.3f),new Vector3(.55f,.6f,.55f));
            Existing("wooden_crate_02","CellarCrateB",P(4.9f,4.9f,BF+.3f),new Vector3(.55f,.6f,.55f));
            Existing("wooden_crate_01","CellarCrateC",P(.7f,5.6f,BF+.3f),new Vector3(.55f,.6f,.55f));
            Existing("wooden_crate_02","CellarCrateD",P(4.9f,5.6f,BF+.3f),new Vector3(.55f,.6f,.55f));
            Existing("wooden_crate_01","CellarCrateE",P(5.1f,3.4f,BF+.3f),new Vector3(.55f,.6f,.55f));
            var toolbox=Existing("metal_tool_chest","CellarToolbox",P(5.1f,2.2f,BF+.2f),new Vector3(.6f,.4f,.35f),"Upper_SteelRust");
            var ward1=Existing("painted_wooden_cabinet_02","CellarWardrobe1",P(2.6f,1.3f,BF+.95f),new Vector3(.95f,1.9f,.45f));
            var ward2=Existing("painted_wooden_cabinet_02","CellarWardrobe2",P(4.95f,1.05f,BF+.95f),new Vector3(.95f,1.9f,.45f));
            Item(ward1,AtmajaAction.WardrobeHide,"Sembunyi di lemari kayu #1");
            Item(ward2,AtmajaAction.WardrobeHide,"Sembunyi di lemari kayu #2");
            foreach(var w in new[]{ward1,ward2})
            {
                var c=KosVisualV2.BoundsOf(w).center;
                for(int i=0;i<5;i++)Box("WardrobeSlat",P(c.x-.32f,c.z-.5f+i*.16f,BF+1.0f),new Vector3(.03f,.1f,.02f),Mat("House_MAT_WoodDoor"),false);
            }
            director.basement.wardrobeCol1=ward1.GetComponent<BoxCollider>();
            director.basement.wardrobeCol2=ward2.GetComponent<BoxCollider>();
            director.basement.hideAnchor1=Anchor("HideAnchor1",P(2.6f,1.3f,BF+.01f),P(2.6f,3,0));
            director.basement.hideAnchor2=Anchor("HideAnchor2",P(4.95f,1.05f,BF+.01f),P(4.95f,3,0));
            var bench=Original("UpperSewingBench","SewingBench",P(3.7f,6.575f,BF));Ground(bench,BF);FitCollider(bench);
            Object.DestroyImmediate(bench.GetComponent<Collider>());
            foreach(float x in new[]{2.7f,4.7f})foreach(float z in new[]{6.3f,6.85f})
            {var leg=GameObject.CreatePrimitive(PrimitiveType.Cube);leg.name="BenchLeg";leg.transform.SetParent(environment,false);leg.transform.position=P(x,z,BF+.42f);leg.transform.localScale=new Vector3(.09f,.85f,.09f);leg.GetComponent<Renderer>().enabled=false;}
            var benchTop=GameObject.CreatePrimitive(PrimitiveType.Cube);benchTop.name="BenchTopCollision";benchTop.transform.SetParent(environment,false);
            benchTop.transform.position=P(3.7f,6.575f,BF+.845f);benchTop.transform.localScale=new Vector3(2.2f,.055f,.72f);benchTop.GetComponent<Renderer>().enabled=false;
            var machine=Original("UpperSewingMachine","SewingMachine",P(3.6f,6.55f,BF+.875f));FitCollider(machine);
            var packBody=Box("BasementPowerbank",P(2.85f,6.575f,BF+.9f),new Vector3(.16f,.09f,.05f),Mat("Upper_PowerbankBody"));
            Box("BasementPowerbankLED",P(2.85f,6.53f,BF+.93f),new Vector3(.1f,.012f,.012f),Mat("Upper_PowerbankLED"),false);
            Item(packBody,AtmajaAction.Powerbank,"Ambil Powerbank Besar Damar");
            director.basement.powerbankObj=packBody;
            var mirror=Original("UpperStandingMirror","CellarBigMirror",P(.75f,6.2f,BF));mirror.transform.rotation=Facing(north);Ground(mirror,BF);FitCollider(mirror);
            director.chase.bigMirror=mirror;
            var shards=new GameObject("MirrorShards");shards.transform.SetParent(environment,false);
            for(int i=0;i<5;i++)
            {
                var shard=GameObject.CreatePrimitive(PrimitiveType.Cube);shard.name="MirrorShard";shard.transform.SetParent(shards.transform,false);
                shard.transform.position=P(.6f+i*.09f,6.05f,BF+.15f+i*.04f);shard.transform.localScale=new Vector3(.07f,.02f,.1f);
                shard.GetComponent<Renderer>().sharedMaterial=Mat("Upper_MirrorAged");
            }
            shards.SetActive(false);director.chase.mirrorShards=shards;
            var cam=ExistingMapped("vintage_video_camera","DamarHandycam",P(1.05f,5.9f,BF+.1f),new Vector3(.32f,.2f,.22f),Mat("Upper_HandycamBody"),Mat("House_MAT_GlassDark"));
            Item(cam,AtmajaAction.Handycam,"Ambil Handycam Sony milik Damar");
            director.basement.handycamObj=cam;
            var rag=Original("UpperHangingRag","StairExitRag",P(2.6f,5.0f,BF));FitCollider(rag);
            Box("RagRod",P(2.6f,5.0f,BF+2.12f),new Vector3(.95f,.04f,.04f),Mat("House_MAT_WoodDoor"));
            director.basement.ragOriginal=rag;
            Existing("wooden_crate_01","TeplokCrate",P(5.2f,.9f,BF+.25f),new Vector3(.5f,.5f,.5f));
            var teplok=ExistingMapped("vintage_oil_lamp","CellarTeplok",P(5.2f,.9f,BF+.7f),new Vector3(.28f,.4f,.28f),Mat("Upper_ChandelierBrass"),Mat("Upper_TeplokFlame"));
            var flame=Box("TeplokFlame",P(5.2f,.9f,BF+.92f),new Vector3(.05f,.09f,.05f),Mat("Upper_TeplokFlame"),false);
            director.basement.teplokFlame=flame;
            director.basement.teplokLight=LightAt("CellarTeplokLight",P(5.2f,.9f,BF+.9f),LightType.Point,1.35f,8,new Color(1,.7f,.4f),P(4,.9f,BF));
            var nyonya=Original("UpperStandingNyonya","NyonyaSriAtmaja",P(3.3f,5.0f,BF));nyonya.transform.rotation=Facing(-east);FitCollider(nyonya);
            var shears=Original("UpperShearsRust","NyonyaShears",P(0,0,0));shears.transform.SetParent(nyonya.transform,false);
            shears.transform.localPosition=new Vector3(.15f,-.2f,.55f);shears.transform.localRotation=Quaternion.Euler(90,0,20);
            director.basement.nyonya=nyonya;director.basement.shearsProp=shears;director.chase.chaserFigure=nyonya;
            var hum=nyonya.AddComponent<AudioSource>();hum.playOnAwake=false;hum.loop=true;hum.spatialBlend=1;hum.minDistance=1;hum.maxDistance=12;hum.rolloffMode=AudioRolloffMode.Linear;hum.outputAudioMixerGroup=director.audioProfile.effectsBus;
            hum.clip=AtmajaSynth.Melody(new[]{196,174.6f,164.8f,146.8f,130.8f},1.1f,.16f);hum.volume=.4f;hum.Play();
            director.basement.humSource=hum;
            nyonya.SetActive(false);
            var wp=new System.Collections.Generic.List<Transform>();
            foreach(var p in new[]{new[]{1.3f,6.2f},new[]{.6f,3.4f},new[]{1.2f,5.3f},new[]{2.6f,6.3f},new[]{4.6f,6.3f},new[]{4.6f,1.6f},new[]{4.95f,2.0f},new[]{3.6f,1.2f}})
                wp.Add(Anchor("NyonyaWP",P(p[0],p[1],BF+.01f),P(2.9f,3.5f,0)));
            director.basement.waypoints=wp.ToArray();
            director.basement.inspectAnchor1=wp[1];director.basement.inspectAnchor2=wp[6];
            director.basement.spawnBasement=Anchor("Spawn_Basement",P(2.2f,6.3f,BF+.01f),P(2.2f,3,0));
            Box("CellarStain",P(3.9f,4.4f,BF+.005f),new Vector3(1.2f,.012f,.9f),Mat("Upper_BloodSmear"),false);
            LightAt("CellarDimLight",P(2.9f,3.5f,BF+1.9f),LightType.Point,.7f,9,new Color(.6f,.65f,.75f),P(2.9f,3.5f,BF));
        }
        private static void BuildChaseMods()
        {
            var blocker=Box("DiningVaultBlocker",P(3.6f,3.0f,1.2f),new Vector3(2.4f,1.5f,.5f),Mat("House_MAT_WoodDoor"));
            var fallen=Model("LongDiningTable","FallenVaultTable",P(3.6f,3.0f,.52f));fallen.transform.rotation=Quaternion.Euler(0,0,78);
            foreach(float x in new[]{2.8f,4.4f})
            {var chair=Model("WornWoodChair","FallenVaultChair",P(x,3.0f,.52f));chair.transform.rotation=Quaternion.Euler(78,0,15);FitCollider(chair);}
            blocker.GetComponent<Renderer>().enabled=false;blocker.SetActive(false);
            director.chase.diningBlocker=blocker;
            director.chase.windowContextLeaf=GameObject.Find("KitchenVaultContext");
            var gate=Box("TrapSlideGate",P(1.0f,1.4f,5.32f),new Vector3(.9f,1.35f,.9f),Mat("House_MAT_WoodDoor"));
            gate.GetComponent<Renderer>().enabled=false;gate.SetActive(false);
            director.chase.slideGate=gate;
            var railData=new float[][]{
                new[]{2.6f,5.6f,BF+.01f},new[]{2.4f,2.6f,BF+.01f},new[]{1.9f,1.0f,BF+.01f},new[]{1.05f,.78f,BF+.01f},
                new[]{1.02f,1.4f,-.2f},new[]{1.02f,2.15f,3.2f},new[]{1.0f,4.6f,3.83f},new[]{1.0f,5.75f,3.83f},
                new[]{-1f,5.7f,3.83f},new[]{-3.25f,6.3f,3.83f},
                new[]{-1f,6.3f,2.9f},new[]{.8f,7.1f,2.3f},new[]{-1.5f,7.4f,1.5f},new[]{-3.3f,7.5f,.6f},new[]{-3.55f,7.9f,.54f},new[]{-2f,5f,.54f},
                new[]{2f,4.2f,.54f},new[]{3.6f,3.4f,.54f},new[]{4.3f,7.5f,.54f},new[]{4f,9f,.54f},new[]{5f,10.5f,.37f},new[]{6f,10.6f,.37f},
                new[]{7.4f,10.6f,.06f},new[]{7.4f,-8f,.06f},new[]{0f,-9.3f,.06f}
            };
            var rail=new System.Collections.Generic.List<Transform>();
            foreach(var p in railData)rail.Add(Anchor("ChaseRail",P(p[0],p[1],p[2]),P(0,-10,0)));
            director.chase.rail=rail.ToArray();
            director.chase.windowBlockSpot=Anchor("WindowBlockSpot",P(5.6f,10.975f,.37f),P(4,10.5f,1.2f));
            director.chase.windowExitSpot=Anchor("WindowExitSpot",P(7.6f,10.975f,.04f),P(7.6f,8,1));
            director.chase.gateSpot=Anchor("GateSpot",P(0,-9.3f,.06f),P(0,-14,0));
            director.chase.spawnChase=Anchor("Spawn_Chase",P(1.6f,5.4f,BF+.01f),P(2.2f,6.5f,0));
            director.chase.spawnStreet=Anchor("Spawn_Street",P(0,-14,.06f),P(0,2,3));
            director.chase.houseLookSpot=Anchor("HouseLookSpot",P(0,-14,.06f),P(0,2,5));
            var streetFigure=Original("UpperStandingNyonya","EndingWindowSilhouette",P(1.08f,-.55f,3.9f));
            streetFigure.transform.rotation=Facing(-north);streetFigure.SetActive(false);
            director.chase.streetFigure=streetFigure;
            var lightning=new GameObject("EndingLightning",typeof(Light));lightning.transform.SetParent(environment,false);
            var bolt=lightning.GetComponent<Light>();bolt.type=LightType.Directional;bolt.intensity=0;bolt.color=new Color(.75f,.82f,1);bolt.shadows=LightShadows.None;
            director.chase.lightning=bolt;
            var title=Plaque("EndingTitleCard","L A Y A R   T E R A K H I R",P(0,-13.2f,2.4f),3.0f,new Color(.55f,.03f,.02f));
            title.transform.rotation=Facing(-north);title.SetActive(false);
            director.chase.endingTitle=title;
        }
        private static void WireDirector()
        {
            var upper=director.upper;
            if(upper.spawnUpper==null)upper.spawnUpper=Anchor("Spawn_UpperLanding",P(-3.25f,6.1f,3.83f),P(0,6.1f,4.5f));
            if(upper.trapdoorTop==null)upper.trapdoorTop=Anchor("TrapdoorTop",P(1.0f,.95f,3.83f),P(1.0f,1.95f,3.83f));
        }
    }
}
#endif
