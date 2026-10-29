#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class DesignerWorldRevision
    {
        public static void Build()
        {
            PlaytestRevisionBuilder.Build();
            // Only input defaults change in the original room; its approved furniture stays intact.
            var room=EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var input=Object.FindFirstObjectByType<GameInputConfig>();
            if(input.vaultKey!=KeyCode.Space){input.vaultKey=KeyCode.Space;EditorSceneManager.MarkSceneDirty(room);EditorSceneManager.SaveScene(room);}
            AssetDatabase.SaveAssets();
            Debug.Log("DESIGNER_WORLD_BUILD complete: paired gates, facilities, inhabited facade, night sky, indoor note and seated doll");
        }
        public static void ProbeTraversal()
        {
            var report=new System.Text.StringBuilder();
            void At(string label,Vector3 feet,float height=1.75f)
            {
                Physics.SyncTransforms();report.AppendLine(label+" feet="+feet);
                foreach(var c in Physics.OverlapCapsule(feet+Vector3.up*.34f,feet+Vector3.up*(height-.32f),.36f,~0,QueryTriggerInteraction.Ignore))
                    report.AppendLine(c.name+" "+c.bounds);
            }
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);var d=Object.FindFirstObjectByType<AtmajaDirector>();
            At("Upper landing corridor approach",d.P(-3.3f,5.6f,3.9f));
            At("Upper corridor eastward",d.P(-2.8f,5.6f,3.9f));
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            At("Kos bathroom foyer front",new Vector3(2.1f,.08f,3.31f));
            System.IO.File.WriteAllText("Audit/designer-traversal-probe.txt",report.ToString());Debug.Log(report);
        }
        private static GameObject Cube(Transform parent,string name,Vector3 position,Vector3 scale,Material material,bool solid=true)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.SetParent(parent,false);
            go.transform.localPosition=position;go.transform.localScale=scale;go.GetComponent<Renderer>().sharedMaterial=material;
            if(!solid)Object.DestroyImmediate(go.GetComponent<Collider>());return go;
        }
        private static Material Mat(string name,Color color)
        {
            string path="Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+name+".mat";
            var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
            m.SetColor("_BaseColor",color);m.SetFloat("_Smoothness",.22f);EditorUtility.SetDirty(m);return m;
        }
        public static Transform PairedGate(Transform parent,Vector3 position,Quaternion rotation,float width,Material iron)
        {
            var root=new GameObject("DoubleLeafIronGate").transform;root.SetParent(parent,false);root.SetPositionAndRotation(position,rotation);
            var pair=root.gameObject.AddComponent<DoubleLeafGate>();
            foreach(int side in new[]{-1,1})
            {
                var hinge=new GameObject(side<0?"GateLeftHinge":"GateRightHinge").transform;hinge.SetParent(root,false);hinge.localPosition=new Vector3(side*width*.5f,0,0);
                float centre=-side*width*.25f;
                foreach(float x in new[]{0f,-side*width*.5f})Cube(hinge,"GateLeafStile",new Vector3(x,1.35f,0),new Vector3(.055f,2.7f,.065f),iron,false);
                foreach(float y in new[]{.18f,.86f,2.55f})Cube(hinge,"GateLeafRail",new Vector3(centre,y,0),new Vector3(width*.5f,.05f,.06f),iron,false);
                for(int i=1;i<10;i++)
                {
                    float x=-side*width*.5f*i/10f;
                    Cube(hinge,"GateVerticalIron",new Vector3(x,1.43f,0),new Vector3(.026f,2.52f,.026f),iron,false);
                    var tip=Cube(hinge,"GateSpear",new Vector3(x,2.74f,0),new Vector3(.048f,.09f,.048f),iron,false);tip.transform.localRotation=Quaternion.Euler(0,0,45);
                }
                Cube(hinge,"GateCentreHandle",new Vector3(-side*(width*.5f-.12f),1.22f,-.065f),new Vector3(.025f,.18f,.06f),iron,false);
                var hull=hinge.gameObject.AddComponent<BoxCollider>();hull.center=new Vector3(centre,1.35f,0);hull.size=new Vector3(width*.5f,2.7f,.09f);
                if(side<0)pair.leftLeaf=hinge;else pair.rightLeaf=hinge;
            }
            return root;
        }
        public static void NightSky(Camera camera)
        {
            const string path="Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/DesignerNightSky.mat";
            var material=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!material){material=new Material(Shader.Find("LayarTerakhir/NightSky"));AssetDatabase.CreateAsset(material,path);}
            RenderSettings.skybox=material;camera.clearFlags=CameraClearFlags.Skybox;
            RenderSettings.ambientLight=new Color(.075f,.085f,.115f);
        }
        public static void Atmaja(AtmajaDirector d,Transform environment)
        {
            var sign=GameObject.Find("AtmajaNameplate");if(sign)Object.DestroyImmediate(sign);
            var scooter=GameObject.Find("DamarParkedScooter");if(scooter&&scooter.GetComponent<AtmajaItem>())Object.DestroyImmediate(scooter.GetComponent<AtmajaItem>());
            var paper=AtmajaSceneBuilder.Box("DamarSurveyInsideDining",d.P(2.30f,7.25f,.56f),new Vector3(.22f,.018f,.30f),Mat("DesignerAgedPaper",new Color(.57f,.53f,.39f)));
            var note=paper.AddComponent<AtmajaItem>();note.director=d;note.action=AtmajaAction.Survey;note.label="Baca catatan terlipat";
            Object.DestroyImmediate(d.gateHinge.gameObject);
            d.gateHinge=PairedGate(environment,d.P(0,-10,0),Quaternion.LookRotation(d.north),2.96f,AtmajaSceneBuilder.Mat("House_MAT_Iron" )??Mat("DesignerRustIron",new Color(.16f,.10f,.07f)));
            foreach(var leaf in new[]{d.gateHinge.GetComponent<DoubleLeafGate>().leftLeaf,d.gateHinge.GetComponent<DoubleLeafGate>().rightLeaf})
            {var item=leaf.gameObject.AddComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.Gate;item.label="Buka kedua daun gerbang";}
            var tree=GameObject.Find("FallenKamboja");
            if(tree)
            {
                tree.transform.rotation=Quaternion.LookRotation(d.north)*Quaternion.Euler(0,0,22);
                var bounds=KosVisualV2.BoundsOf(tree);
                float minimum=d.LocalPosition(bounds.center).x-Mathf.Abs(d.east.x)*bounds.extents.x-Mathf.Abs(d.east.z)*bounds.extents.z;
                if(minimum<6.35f)tree.transform.position+=d.east*(6.35f-minimum);
                AtmajaSceneBuilder.Ground(tree,-.12f);
                var support=GameObject.CreatePrimitive(PrimitiveType.Cylinder);support.name="FallenKambojaSupportedLimb";support.transform.SetParent(environment,false);
                Vector3 a=d.P(6.7f,5.5f,1.12f),b=d.P(9.7f,5.5f,1.62f);
                support.transform.SetPositionAndRotation((a+b)*.5f,Quaternion.FromToRotation(Vector3.up,(b-a).normalized));support.transform.localScale=new Vector3(.36f,(b-a).magnitude*.5f,.36f);
                support.GetComponent<Renderer>().sharedMaterial=AtmajaSceneBuilder.Mat("Tree_FRANG_Bark");Object.DestroyImmediate(support.GetComponent<Collider>());
                var rootPatch=GameObject.CreatePrimitive(PrimitiveType.Sphere);rootPatch.name="UprootedDampSoil";rootPatch.transform.SetParent(environment,false);
                rootPatch.transform.position=d.P(6.5f,5.4f,.08f);rootPatch.transform.localScale=new Vector3(1.55f,.22f,1.4f);rootPatch.GetComponent<Renderer>().sharedMaterial=AtmajaSceneBuilder.Mat("stony_dirt_path");Object.DestroyImmediate(rootPatch.GetComponent<Collider>());
            }
            var doll=AtmajaSceneBuilder.Model("ARSeatedAtmaja","SeatedDollOnEndChair",d.P(3.7f,2.15f,.752f));
            doll.transform.rotation=Quaternion.LookRotation(d.north);doll.transform.localScale=Vector3.one*.60f;
            // A readable, solid porcelain/rag doll in the ordinary world, not a tiny
            // duplicate of the dark AR apparition disappearing below the chair back.
            var dollSkin=Mat("DesignerDollPorcelain",new Color(.68f,.61f,.49f));
            var dollDress=Mat("DesignerDollFadedDress",new Color(.30f,.13f,.12f));
            foreach(var renderer in doll.GetComponentsInChildren<Renderer>())
                renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>m.name=="Detail_ARSkin"?dollSkin:m.name=="Detail_ARCloth"?dollDress:m).ToArray();
            foreach(var col in doll.GetComponentsInChildren<Collider>())Object.DestroyImmediate(col);
            Object.FindFirstObjectByType<GameInputConfig>().vaultKey=KeyCode.Space;
            NightSky(d.player.GetComponent<InteractionInteractor>().playerCamera);
        }
    }

    public static partial class KosCorridorBuilder
    {
        private static void DesignerFacilities()
        {
            // The third left ground-floor room becomes a dedicated communal dining room.
            var old=GameObject.Find("KosGroundDoor_-1.5_2");if(old)Object.DestroyImmediate(old);
            // Ruang makan + dapur dijadikan satu area terbuka tanpa pintu (desainer).
            Box("SharedDiningJambL",new Vector3(-1.45f,1.07f,7.02f),new Vector3(.10f,2.14f,.10f),wood);
            Box("SharedDiningJambR",new Vector3(-1.45f,1.07f,7.98f),new Vector3(.10f,2.14f,.10f),wood);
            Box("SharedDiningHeader",new Vector3(-1.45f,2.20f,7.5f),new Vector3(.10f,.12f,1.06f),wood);
            Label("SharedDiningSign","DAPUR & RUANG MAKAN",new Vector3(-1.36f,2.45f,7.5f),Vector3.right,.13f);
            var table=RealModel("dining_table.fbx","KosCommunalDiningTable",new Vector3(1.12f,.78f,1.65f),new Vector3(-3.8f,.39f,7.5f),0,_=>PhReal("dining_table"));
            foreach(float x in new[]{-2.85f,-4.75f})foreach(float z in new[]{7.0f,8.0f})
            {
                var chair=RealModel("dining_chair_02.fbx","KosDiningWoodChair",new Vector3(.43f,.85f,.44f),new Vector3(x,.425f,z),0,_=>PhReal("dining_chair_02"));chair.transform.rotation=Quaternion.LookRotation(new Vector3(-3.8f-x,0,0));
            }
            Practical(new Vector3(-3.8f,2.7f,7.5f),new Color(.90f,.79f,.58f),1.45f,4.5f);
            var dish=Material("KosEnamelDish",new Color(.68f,.72f,.67f));
            // Ruang makan bersama: enam piring berisi makanan di tiap kursi,
            // bukan tiga piring kosong di tengah meja.
            for(int i=0;i<6;i++)
            {
                float px=i%2==0?-4.02f:-3.58f,pz=6.95f+(i/2)*.55f;
                var plate=GameObject.CreatePrimitive(PrimitiveType.Cylinder);plate.name="SharedDiningPlate";plate.transform.SetParent(root,false);plate.transform.position=new Vector3(px,.80f,pz);plate.transform.localScale=new Vector3(.23f,.008f,.23f);plate.GetComponent<Renderer>().sharedMaterial=dish;Object.DestroyImmediate(plate.GetComponent<Collider>());
                RealModel("veggiesbox.fbx","SharedDiningFood",new Vector3(.17f,.09f,.17f),new Vector3(px,.845f,pz),i*37,null);
            }
            RealModel("pot_enamel_01.fbx","SharedDiningPot",new Vector3(.34f,.20f,.34f),new Vector3(-3.8f,.90f,7.5f),10,_=>PhReal("pot_enamel_01"));
            RealModel("vintage_electric_kettle.fbx","SharedDiningKettle",new Vector3(.20f,.24f,.20f),new Vector3(-3.8f,.92f,7.85f),-15,_=>PhReal("vintage_electric_kettle"));
            // Separate enclosed toilet/shower cubicles, reached through the common washroom foyer.
            foreach(var partition in root.GetComponentsInChildren<Transform>().Where(t=>t.name=="RoomPartition"&&t.position.x>1&&t.position.y<3&&Mathf.Abs(t.position.z-3)<.01f).ToArray())
                Object.DestroyImmediate(partition.gameObject);
            foreach(string name in new[]{"KAMAR MANDIFloor","KAMAR MANDICeiling","KAMAR MANDISide"})
            foreach(var t in root.GetComponentsInChildren<Transform>().Where(t=>t.name==name))
            {var p=t.position;p.x=3.90f;t.position=p;var s=t.localScale;s.x=name.EndsWith("Floor")?4.7f:4.8f;t.localScale=s;}
            var back=GameObject.Find("KAMAR MANDIBack");back.transform.position=new Vector3(6.25f,1.4f,4.25f);
            var outerDoor=GameObject.Find("KAMAR MANDIDoor").GetComponent<HingedDoor>();outerDoor.slides=true;outerDoor.slideOffset=Vector3.forward*1.6f;
            Box("BathPocketDoorTrack",new Vector3(1.40f,2.25f,5.0f),new Vector3(.06f,.055f,3.1f),metal);
            GameObject.Find("BathroomWaterTank").transform.position=new Vector3(5.7f,.55f,5.5f);
            // Kloset, wastafel, dan shower kini aset internet realistis (SharedRoom).
            Box("GenderBathroomPartition",new Vector3(4.575f,1.3f,4.25f),new Vector3(3.35f,2.6f,.10f),wall);
            foreach(float z in new[]{3.30f,5.20f})
            {
                bool women=z>4;string title=women?"WANITA":"PRIA";
                var h=new GameObject("Bathroom"+title+"Hinge").transform;h.SetParent(root,false);h.position=new Vector3(2.82f,0,z-.46f);
                var panel=Box("Bathroom"+title+"Door",new Vector3(2.82f,1.06f,z),new Vector3(.065f,2.12f,.9f),doorMat??wood);panel.transform.SetParent(h,true);
                var hd=panel.AddComponent<HingedDoor>();hd.hinge=h;hd.angle=95;hd.label="Buka kamar mandi "+title.ToLowerInvariant();
                // Plang gender menempel di daun pintu dan ikut bergerak saat
                // pintu dibuka (arahan desainer: kecil, tidak melayang).
                GenderBathroomSign(title,z,women,panel.transform);
                Box("CubicleDoorHeader",new Vector3(2.82f,2.43f,z),new Vector3(.10f,.5f,1.0f),wall);
                // Fixture realistis sudah di SharedRoom; kubikel hanya butuh sekat.
            }
            foreach(var segment in new[]{new Vector2(2.5f,2.84f),new Vector2(3.76f,4.74f),new Vector2(5.66f,6f)})
                Box("BathroomFrontPartition",new Vector3(2.82f,1.3f,(segment.x+segment.y)*.5f),new Vector3(.1f,2.6f,segment.y-segment.x),wall);
            // Properly coloured street-facing finish and visible windows for both floors.
            var paint=Material("KosExteriorWarmIvory",new Color(.66f,.58f,.42f));
            foreach(int side in new[]{-1,1})foreach(float y in new[]{0,3.3f})
            {
                Box("KosPaintedFrontFacade",new Vector3(side*3.65f,y+1.5f,-1.14f),new Vector3(5.15f,2.94f,.035f),paint);
                foreach(float x in new[]{side*2.4f,side*4.8f})
                {
                    Box("KosFrontWindowFrame",new Vector3(x,y+1.65f,-1.19f),new Vector3(1.05f,1.22f,.09f),wood);
                    Box("KosFrontWindowGlass",new Vector3(x,y+1.65f,-1.245f),new Vector3(.90f,1.08f,.018f),glass);
                    Box("KosFrontWindowMullion",new Vector3(x,y+1.65f,-1.27f),new Vector3(.035f,1.1f,.02f),trim);
                    Box("KosWindowSill",new Vector3(x,y+1.03f,-1.25f),new Vector3(1.16f,.06f,.24f),trim);
                }
            }
            Object.DestroyImmediate(d.gateHinge.gameObject);
            d.gateHinge=DesignerWorldRevision.PairedGate(root,new Vector3(0,0,-7),Quaternion.identity,2.8f,metal);
            foreach(var h in new[]{d.gateHinge.GetComponent<DoubleLeafGate>().leftLeaf,d.gateHinge.GetComponent<DoubleLeafGate>().rightLeaf})
            {var travel=h.gameObject.AddComponent<KosTravelItem>();travel.director=d;travel.label="Pilih tujuan di gerbang kos";}
            DesignerWorldRevision.NightSky(d.player.GetComponent<InteractionInteractor>().playerCamera);
        }
        private static void GenderBathroomSign(string title,float z,bool women,Transform doorPanel)
        {
            // Plang kecil (.26 x .34) menempel di muka daun pintu sisi koridor
            // dan di-parent ke daunnya: ikut berayun, tidak melayang di bukaan.
            var plateMat=Material(women?"BathroomSignPink":"BathroomSignBlue",women?new Color(.75f,.35f,.45f):new Color(.25f,.45f,.70f));
            var plaque=Box("Bathroom"+title+"Plaque",new Vector3(2.765f,1.78f,z),new Vector3(.02f,.34f,.26f),plateMat);plaque.transform.SetParent(doorPanel,true);
            var trimPlaque=Box("Bathroom"+title+"PlaqueTrim",new Vector3(2.752f,1.78f,z),new Vector3(.012f,.37f,.29f),trim);trimPlaque.transform.SetParent(doorPanel,true);
            // Ikon sederhana: kepala (silinder) + badan (kotak); rok untuk wanita.
            var iconMat=Material("BathroomIconWhite",new Color(.92f,.92f,.90f));
            var head=GameObject.CreatePrimitive(PrimitiveType.Cylinder);head.name="Bathroom"+title+"IconHead";head.transform.position=new Vector3(2.75f,1.86f,z);head.transform.localScale=new Vector3(.055f,.02f,.055f);head.GetComponent<Renderer>().sharedMaterial=iconMat;Object.DestroyImmediate(head.GetComponent<Collider>());head.transform.SetParent(doorPanel,true);
            if(women)
            {
                var skirt=GameObject.CreatePrimitive(PrimitiveType.Cylinder);skirt.name="Bathroom"+title+"IconSkirt";skirt.transform.position=new Vector3(2.75f,1.72f,z);skirt.transform.localScale=new Vector3(.10f,.085f,.03f);skirt.GetComponent<Renderer>().sharedMaterial=iconMat;Object.DestroyImmediate(skirt.GetComponent<Collider>());skirt.transform.SetParent(doorPanel,true);
            }
            else
            {
                var body=Box("Bathroom"+title+"IconBody",new Vector3(2.75f,1.735f,z),new Vector3(.015f,.11f,.07f),iconMat);body.transform.SetParent(doorPanel,true);
                var legs=Box("Bathroom"+title+"IconLegs",new Vector3(2.75f,1.655f,z),new Vector3(.015f,.055f,.06f),iconMat);legs.transform.SetParent(doorPanel,true);
            }
            foreach(var t in doorPanel.GetComponentsInChildren<Transform>())if(t.name.StartsWith("Bathroom"+title))foreach(var c in t.GetComponents<Collider>())Object.DestroyImmediate(c);
        }
    }
}
#endif
