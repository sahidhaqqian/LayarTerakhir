#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class MelatiNeighborhoodBuilder
    {
        private static Transform root;private static Material concrete,wood,metal,roof;
        private static Material Mat(string name)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/Art/Atmaja/Materials/"+name+".mat");
        private static GameObject Box(string name,Vector3 p,Vector3 size,Material material,bool collision=true)
        {var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(root,false);g.transform.position=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=material;if(!collision)Object.DestroyImmediate(g.GetComponent<Collider>());return g;}
        private static Transform Anchor(string name,Vector3 p,Vector3 direction)
        {var g=new GameObject(name);g.transform.SetParent(root,false);g.transform.SetPositionAndRotation(p,Quaternion.LookRotation(direction));return g.transform;}
        public static void Apply()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            var old=GameObject.Find("KampungMelati");if(old)Object.DestroyImmediate(old);
            root=new GameObject("KampungMelati").transform;
            concrete=Mat("House_MAT_Concrete");wood=Mat("House_MAT_WoodDoor");metal=Mat("Upper_SteelRust");roof=Mat("House_MAT_RoofTiles");
            var street=root.gameObject.AddComponent<MelatiNeighborhood>();street.corridor=corridor;
            street.streetSpawn=Anchor("MelatiStreetSpawn",new Vector3(1.15f,.04f,-18.2f),Vector3.back);
            street.parkedMotor=Anchor("MelatiParkedMotor",new Vector3(0,corridor.motor.position.y,-18.2f),Vector3.back);
            street.departure=Anchor("MelatiAtmajaExit",new Vector3(0,.04f,-94),Vector3.back);corridor.rideExit=street.parkedMotor;
            var roadTemplate=Mat("dirty_concrete");var road=new Material(roadTemplate?roadTemplate:concrete){name="MelatiWetRoad"};
            road.SetColor("_BaseColor",new Color(.24f,.27f,.29f));road.SetFloat("_Smoothness",.58f);
            string roadPath="Assets/_LayarTerakhir/Art/Review/Materials/MelatiWetRoad.mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(roadPath);if(saved){EditorUtility.CopySerialized(road,saved);Object.DestroyImmediate(road);road=saved;}else AssetDatabase.CreateAsset(road,roadPath);
            for(int i=0;i<24;i++)Box("MelatiRoadSection",new Vector3(0,-.085f,-12-i*4),new Vector3(7.2f,.17f,4),road);
            Box("MelatiCrossStreet",new Vector3(0,-.085f,-50),new Vector3(58,.17f,7.2f),road);
            Box("MelatiNeighborhoodGround",new Vector3(0,-.16f,-54),new Vector3(64,.16f,102),concrete);
            for(int side=-1;side<=1;side+=2)
            {
                for(int i=0;i<22;i++)
                {
                    float z=-12-i*4;if(z< -46&&z> -54)continue;
                    Box("MelatiSidewalk",new Vector3(side*4.55f,.035f,z),new Vector3(1.8f,.07f,3.9f),concrete);
                    Box("MelatiDrain",new Vector3(side*3.72f,-.035f,z),new Vector3(.26f,.07f,3.94f),metal);
                    if(i%2==0)Box("MelatiDrainGrate",new Vector3(side*3.72f,.015f,z),new Vector3(.31f,.025f,.45f),metal,false);
                }
                int index=0;
                foreach(float z in new[]{-24f,-38f,-64f,-79f,-93f})
                {
                    if(side==1&&z== -24)continue;
                    string type=new[]{"a","b","d","f","h","k","m","p"}[(index+(side==1?3:0))%8];
                    var house=DesignerReviewBuilder.ReviewModel("MelatiHome_"+side+"_"+index,"MelatiHouse_"+type,root,new Vector3(side*12,2.5f,z),Vector3.zero,false);
                    house.transform.rotation=Quaternion.Euler(0,side==1?90:-90,0);var bounds=KosVisualV2.BoundsOf(house);house.transform.position+=Vector3.up*(.04f-bounds.min.y);
                    KosVisualV2.FitBox(house.AddComponent<BoxCollider>(),KosVisualV2.BoundsOf(house));
                    Box("MelatiHomePorch",new Vector3(side*7.4f,.10f,z),new Vector3(2.3f,.20f,4.5f),concrete);
                    Box("MelatiPorchAwning",new Vector3(side*7.4f,2.75f,z),new Vector3(2.5f,.13f,4.7f),roof);
                    foreach(float end in new[]{-2.1f,2.1f})
                    {
                        Box("MelatiPorchColumn",new Vector3(side*6.4f,1.43f,z+end),new Vector3(.13f,2.6f,.13f),wood);
                        Box("MelatiGardenWall",new Vector3(side*6.2f,.5f,z+end*1.4f),new Vector3(.15f,1,1.4f),concrete);
                    }
                    Label("MelatiHouseNumber","MELATI / "+(12+index*2+(side==1?1:0)),new Vector3(side*6.25f,1.8f,z-1.75f),side==1?Vector3.left:Vector3.right,.12f);
                    index++;
                }
                for(int i=0;i<4;i++)StreetLamp(new Vector3(side*4.9f,0,-18-i*22));
            }
            for(int side=-1;side<=1;side+=2)
            {
                var house=DesignerReviewBuilder.ReviewModel("MelatiCornerHouse_"+side,"MelatiHouse_p",root,new Vector3(side*24,2,-60),Vector3.zero,false);
                var bounds=KosVisualV2.BoundsOf(house);house.transform.position+=Vector3.up*(.04f-bounds.min.y);KosVisualV2.FitBox(house.AddComponent<BoxCollider>(),KosVisualV2.BoundsOf(house));
                Box("MelatiCrossroadEnd",new Vector3(side*30,1.25f,-50),new Vector3(.25f,2.5f,8),concrete);
            }
            ReplaceRakaScooter(corridor);BuildShop();
            // The playable street has complete borders; there are no walkable gaps into the void.
            foreach(float x in new[]{-31.5f,31.5f})Box("MelatiDistrictBoundary",new Vector3(x,1.35f,-56),new Vector3(.25f,2.7f,98),concrete);
            Box("MelatiStreetEnd",new Vector3(0,1.5f,-108),new Vector3(64,3,.25f),concrete);
            var exit=Box("MelatiAtmajaSign",new Vector3(3.1f,1.5f,-95),new Vector3(.15f,1.0f,1.5f),wood);
            var item=exit.AddComponent<MelatiDeparture>();item.neighborhood=street;
            Label("MelatiAtmajaSignText","RUMAH ATMAJA\n200 m  →",new Vector3(3.0f,1.5f,-95),Vector3.left,.17f);
            Box("MelatiSignPole",new Vector3(3.1f,.65f,-95),new Vector3(.10f,1.3f,.10f),metal);
            var weather=new GameObject("MelatiContinuousRain");weather.transform.SetParent(root,false);var rain=weather.AddComponent<NeighborhoodRain>();rain.player=corridor.player;rain.material=Mat("Rain");
            var camera=corridor.player.GetComponent<InteractionInteractor>().playerCamera;camera.farClipPlane=130;
            var moon=new GameObject("MelatiOvercastMoon",typeof(Light));moon.transform.SetParent(root,false);moon.transform.rotation=Quaternion.Euler(58,-30,0);var light=moon.GetComponent<Light>();light.type=LightType.Directional;light.color=new Color(.48f,.58f,.72f);light.intensity=.18f;light.shadows=LightShadows.None;
            foreach(var renderer in root.GetComponentsInChildren<MeshRenderer>())GameObjectUtility.SetStaticEditorFlags(renderer.gameObject,StaticEditorFlags.BatchingStatic);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);Debug.Log("MELATI_NEIGHBORHOOD_AUTHORED 11 varied CC0 houses, connected streets, shop, rain");
        }
        private static void ReplaceRakaScooter(KosCorridorDirector corridor)
        {
            var old=corridor.motor.Cast<Transform>().FirstOrDefault(t=>t.GetComponentInChildren<MeshRenderer>());
            if(!old)throw new System.Exception("Raka scooter mesh root is missing");
            var materials=old.GetComponentsInChildren<Renderer>().GroupBy(r=>r.name).ToDictionary(g=>g.Key,g=>g.First().sharedMaterials);
            var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/Art/Review/Models/RakaScooter.fbx");
            var model=(GameObject)PrefabUtility.InstantiatePrefab(asset);model.transform.SetParent(corridor.motor,false);
            model.transform.localPosition=old.localPosition;model.transform.localRotation=old.localRotation;model.transform.localScale=old.localScale;
            foreach(var renderer in model.GetComponentsInChildren<Renderer>())
            {
                if(!materials.TryGetValue(renderer.name,out var slots))throw new System.Exception("Closed scooter material missing "+renderer.name);
                renderer.sharedMaterials=slots;
            }
            Object.DestroyImmediate(old.gameObject);var collider=corridor.motor.GetComponent<BoxCollider>();if(collider)KosVisualV2.FitBox(collider,KosVisualV2.BoundsOf(corridor.motor.gameObject));
        }
        private static void StreetLamp(Vector3 p)
        {
            Box("MelatiUtilityPole",p+Vector3.up*2.7f,new Vector3(.12f,5.4f,.12f),wood);
            Box("MelatiLampArm",p+new Vector3(-Mathf.Sign(p.x)*.4f,4.8f,0),new Vector3(.9f,.055f,.06f),metal,false);
            var lamp=new GameObject("MelatiStreetLight",typeof(Light));lamp.transform.SetParent(root,false);lamp.transform.position=p+new Vector3(-Mathf.Sign(p.x)*.85f,4.7f,0);lamp.transform.rotation=Quaternion.Euler(90,0,0);
            var light=lamp.GetComponent<Light>();light.type=LightType.Spot;light.range=12;light.spotAngle=105;light.intensity=3.8f;light.color=new Color(.93f,.79f,.55f);light.shadows=LightShadows.None;
            var wire=new GameObject("MelatiPowerCable",typeof(LineRenderer));wire.transform.SetParent(root,false);var line=wire.GetComponent<LineRenderer>();line.sharedMaterial=metal;line.startWidth=line.endWidth=.018f;line.positionCount=3;line.SetPositions(new[]{p+Vector3.up*5.1f,p+new Vector3(0,4.7f,-11),p+new Vector3(0,5.1f,-22)});
        }
        private static void BuildShop()
        {
            Vector3 p=new Vector3(8.4f,0,-24);Box("WarungFloor",p+new Vector3(0,.1f,0),new Vector3(5,.2f,6.6f),concrete);
            Box("WarungRearWall",p+new Vector3(2.4f,1.55f,0),new Vector3(.15f,2.9f,6.5f),concrete);
            Box("WarungSouthWall",p+new Vector3(0,1.55f,-3.2f),new Vector3(5,2.9f,.13f),wood);
            Box("WarungNorthWall",p+new Vector3(0,1.55f,3.2f),new Vector3(5,2.9f,.13f),wood);
            Box("WarungAwning",p+new Vector3(-.35f,3.12f,0),new Vector3(6.4f,.16f,7),roof);
            var counter=Box("WarungSalesCounter",p+new Vector3(-2.25f,.64f,0),new Vector3(.65f,1.08f,3.8f),wood);counter.AddComponent<MelatiShop>();
            Box("WarungCounterTop",p+new Vector3(-2.3f,1.2f,0),new Vector3(.78f,.055f,4),concrete);
            for(int i=0;i<3;i++)
            {
                Box("WarungStockShelf",p+new Vector3(1.6f,.7f+i*.55f,0),new Vector3(.8f,.07f,5.5f),wood);
                for(int j=0;j<7;j++)Box("WarungSupplyPacket",p+new Vector3(1.5f,.83f+i*.55f,-2.1f+j*.65f),new Vector3(.27f,.22f,.18f),Mat(j%2==0?"Upper_BookCover":"Upper_BookPaper"),false);
            }
            for(int i=0;i<5;i++){var fruit=DesignerReviewBuilder.ReviewModel("WarungFreshFruit","RottenApple",root,p+new Vector3(-2.2f,1.29f,-1+i*.45f),new Vector3(.10f,.10f,.10f),false);foreach(var renderer in fruit.GetComponentsInChildren<Renderer>())renderer.sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/Art/Review/Materials/Review_FreshApple.mat");}
            Label("WarungSign","WARUNG BU SARI\nSEMBAKO  •  KOPI  •  BEKAL",p+new Vector3(-2.8f,2.65f,0),Vector3.left,.22f);
            var glow=new GameObject("WarungWarmLight",typeof(Light));glow.transform.SetParent(root,false);glow.transform.position=p+new Vector3(-1.2f,2.7f,0);var lamp=glow.GetComponent<Light>();lamp.type=LightType.Point;lamp.range=8;lamp.intensity=3;lamp.color=new Color(1,.81f,.51f);lamp.shadows=LightShadows.Soft;
            Box("WarungVisibleLamp",glow.transform.position,new Vector3(.5f,.07f,.12f),Mat("Upper_ChandelierLamp"),false);
            Box("WarungBenchSeat",p+new Vector3(-2.8f,.52f,2.6f),new Vector3(.45f,.10f,.95f),wood);
            foreach(float z in new[]{2.25f,2.95f})Box("WarungBenchLeg",p+new Vector3(-2.8f,.28f,z),new Vector3(.33f,.46f,.07f),wood);
        }
        private static void Label(string name,string text,Vector3 position,Vector3 normal,float size)
        {
            var go=new GameObject(name,typeof(TextMesh));go.transform.SetParent(root,false);go.transform.SetPositionAndRotation(position,Quaternion.LookRotation(-normal));
            var label=go.GetComponent<TextMesh>();label.font=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/UI/Fonts/Barlow-Regular.ttf");label.text=text;label.fontSize=48;label.characterSize=size/10;label.anchor=TextAnchor.MiddleCenter;label.color=new Color(.87f,.85f,.70f);go.GetComponent<MeshRenderer>().sharedMaterial=label.font.material;
        }
    }
}
#endif
