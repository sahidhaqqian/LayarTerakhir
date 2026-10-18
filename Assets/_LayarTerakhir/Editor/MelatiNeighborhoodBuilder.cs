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
        private static readonly System.Collections.Generic.Dictionary<string,Mesh> cubes=new System.Collections.Generic.Dictionary<string,Mesh>();
        private static Material Mat(string name)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/Art/Atmaja/Materials/"+name+".mat");
        private static GameObject Box(string name,Vector3 p,Vector3 size,Material material,bool collision=true)
        {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(root,false);g.transform.position=p;
            g.GetComponent<MeshFilter>().sharedMesh=ArchitecturalCube(g.GetComponent<MeshFilter>().sharedMesh,size);
            g.GetComponent<BoxCollider>().size=size;g.GetComponent<Renderer>().sharedMaterial=material;
            if(!collision)Object.DestroyImmediate(g.GetComponent<Collider>());return g;
        }
        private static Mesh ArchitecturalCube(Mesh source,Vector3 size)
        {
            string name="MelatiCube_"+size.x.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+"_"+size.y.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+"_"+size.z.ToString("R",System.Globalization.CultureInfo.InvariantCulture);
            if(cubes.TryGetValue(name,out var existing)&&existing)return existing;
            const string path="Assets/_LayarTerakhir/Art/Review/Models/MelatiArchitecturalMeshes.asset";
            var saved=AssetDatabase.LoadAllAssetsAtPath(path).OfType<Mesh>().FirstOrDefault(m=>m.name==name);if(saved){cubes[name]=saved;return saved;}
            var mesh=Object.Instantiate(source);mesh.name=name;var vertices=mesh.vertices;var normals=mesh.normals;var uv=new Vector2[vertices.Length];
            for(int i=0;i<vertices.Length;i++)
            {
                vertices[i]=Vector3.Scale(vertices[i],size);var v=vertices[i];var n=normals[i];
                uv[i]=Mathf.Abs(n.y)>.5f?new Vector2(v.x,v.z):Mathf.Abs(n.x)>.5f?new Vector2(v.z,v.y):new Vector2(v.x,v.y);
                uv[i]*=.65f;
            }
            mesh.vertices=vertices;mesh.uv=uv;mesh.RecalculateTangents();mesh.RecalculateBounds();
            if(AssetDatabase.LoadMainAssetAtPath(path))AssetDatabase.AddObjectToAsset(mesh,path);else AssetDatabase.CreateAsset(mesh,path);
            cubes[name]=mesh;return mesh;
        }
        private static Transform Anchor(string name,Vector3 p,Vector3 direction)
        {var g=new GameObject(name);g.transform.SetParent(root,false);g.transform.SetPositionAndRotation(p,Quaternion.LookRotation(direction));return g.transform;}
        public static void Apply()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            var old=GameObject.Find("KampungMelati");if(old)Object.DestroyImmediate(old);
            foreach(var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(t=>new[]{"NeighbourBoundary","StreetEndBoundary","YardBoundary","KosGateBoundary"}.Contains(t.name)).ToArray())Object.DestroyImmediate(t.gameObject);
            corridor.gateHinge.GetComponent<DoubleLeafGate>()?.SetImmediate(true);
            root=new GameObject("KampungMelati").transform;
            concrete=Mat("House_MAT_Concrete");wood=Mat("House_MAT_WoodDoor");metal=Mat("Upper_SteelRust");roof=Mat("House_MAT_RoofTiles");
            var street=root.gameObject.AddComponent<MelatiNeighborhood>();street.corridor=corridor;
            street.streetSpawn=Anchor("MelatiStreetSpawn",new Vector3(1.15f,.04f,-18.2f),Vector3.back);
            street.parkedMotor=Anchor("MelatiParkedMotor",new Vector3(0,corridor.motor.position.y,-18.2f),Vector3.back);
            street.departure=Anchor("MelatiAtmajaExit",new Vector3(0,.04f,-94),Vector3.back);corridor.rideExit=street.parkedMotor;
            var roadTemplate=Mat("dirty_concrete");var road=new Material(roadTemplate?roadTemplate:concrete){name="MelatiWetRoad"};
            road.SetColor("_BaseColor",new Color(.24f,.27f,.29f));road.SetFloat("_Smoothness",.58f);
            string roadPath="Assets/_LayarTerakhir/Art/Review/Materials/MelatiWetRoad.mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(roadPath);if(saved){EditorUtility.CopySerialized(road,saved);Object.DestroyImmediate(road);road=saved;}else AssetDatabase.CreateAsset(road,roadPath);
            float kosStreetEnd=GameObject.Find("KosStreet").GetComponent<Renderer>().bounds.min.z;
            for(int i=0;i<24;i++)
            {
                float min=-14-i*4,max=Mathf.Min(min+4,kosStreetEnd);if(min>=max)continue;
                if(max<=-53.6f||min>=-46.4f)RoadRange(min,max,road);
                else {if(min< -53.6f)RoadRange(min,-53.6f,road);if(max> -46.4f)RoadRange(-46.4f,max,road);}
            }
            Box("MelatiCrossStreet",new Vector3(0,-.085f,-50),new Vector3(58,.17f,7.2f),road);
            Box("MelatiNeighborhoodGround",new Vector3(0,-.16f,-54),new Vector3(188,.16f,260),concrete);
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
                foreach(float z in new[]{19f,6f,-11f,-24f,-37f,-64f,-77f,-90f,-103f})
                {
                    if(side==1&&z== -24)continue;
                    BuildHouse(new Vector3(side*11.5f,0,z),side,index++,false);
                }
                for(int i=0;i<9;i++)if(i!=5)BuildHouse(new Vector3(side*23.5f,0,18-i*14),side,i,true);
                for(int i=0;i<4;i++)StreetLamp(new Vector3(side*4.9f,0,-18-i*22));
            }
            ReplaceRakaScooter(corridor);BuildShop();
            Box("MelatiRearLane",new Vector3(0,-.085f,28),new Vector3(62,.17f,6),road);
            foreach(int side in new[]{-1,1})for(int i=0;i<5;i++)BuildHouse(new Vector3(side*(11+i*10),0,37),-1,i,true);
            // No district walls: houses, connected lanes and planted ground define the street.
            var exit=Box("MelatiAtmajaSign",new Vector3(3.1f,1.5f,-95),new Vector3(.15f,1.0f,1.5f),wood);
            var item=exit.AddComponent<MelatiDeparture>();item.neighborhood=street;
            Label("MelatiAtmajaSignText","RUMAH ATMAJA\n200 m  →",new Vector3(3.0f,1.5f,-95),Vector3.left,.17f);
            Box("MelatiSignPole",new Vector3(3.1f,.65f,-95),new Vector3(.10f,1.3f,.10f),metal);
            var weather=new GameObject("MelatiContinuousRain");weather.transform.SetParent(root,false);var rain=weather.AddComponent<NeighborhoodRain>();rain.player=corridor.player;rain.material=Mat("Rain");
            var camera=corridor.player.GetComponent<InteractionInteractor>().playerCamera;camera.farClipPlane=130;
            var moon=new GameObject("MelatiOvercastMoon",typeof(Light));moon.transform.SetParent(root,false);moon.transform.rotation=Quaternion.Euler(58,-30,0);var light=moon.GetComponent<Light>();light.type=LightType.Directional;light.color=new Color(.48f,.58f,.72f);light.intensity=.65f;light.shadows=LightShadows.Soft;
            RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Trilight;RenderSettings.ambientSkyColor=new Color(.22f,.27f,.34f);RenderSettings.ambientEquatorColor=new Color(.13f,.15f,.19f);RenderSettings.ambientGroundColor=new Color(.055f,.06f,.07f);
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogColor=new Color(.035f,.045f,.065f);RenderSettings.fogDensity=.012f;
            foreach(var renderer in root.GetComponentsInChildren<MeshRenderer>())GameObjectUtility.SetStaticEditorFlags(renderer.gameObject,StaticEditorFlags.BatchingStatic);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();Debug.Log("MELATI_NEIGHBORHOOD_AUTHORED 43 detailed houses, open pedestrian lanes, staffed shop, rain");
        }
        private static void RoadRange(float min,float max,Material road)
        {if(max-min>.001f)Box("MelatiRoadSection",new Vector3(0,-.085f,(min+max)*.5f),new Vector3(7.2f,.17f,max-min),road);}
        private static void BuildHouse(Vector3 p,int side,int index,bool rear)
        {
            string prefix="MelatiResidence_"+p.x+"_"+p.z;
            var parent=root;var house=new GameObject(prefix).transform;house.SetParent(root,false);root=house;
            var plaster=Mat(index%3==0?"House_MAT_PlasterDamaged":"House_MAT_Concrete");
            float width=rear?7.4f:7.6f,depth=rear?9:9.6f,height=3.1f+(index%3)*.24f;
            Box("Foundation",p+Vector3.up*.13f,new Vector3(width+.15f,.26f,depth+.15f),concrete);
            Box("RenderedHouseBody",p+Vector3.up*(height*.5f+.2f),new Vector3(width,height,depth),plaster);
            float face=p.x-side*(width*.5f+.015f);
            Box("HouseSkirting",new Vector3(face,.42f,p.z),new Vector3(.08f,.35f,depth),concrete,false);
            Box("TimberFrontDoor",new Vector3(face-side*.035f,1.25f,p.z-.9f),new Vector3(.10f,2.12f,.94f),wood);
            foreach(float z in new[]{-3.0f,1.4f,3.25f})
            {
                Vector3 w=new Vector3(face-side*.065f,1.85f,p.z+z);
                Box("WindowRecess",w,new Vector3(.12f,1.22f,1.1f),metal,false);
                Box("WindowGlass",w-new Vector3(side*.075f,0,0),new Vector3(.025f,1.05f,.93f),AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/Art/Review/Materials/Review_HouseGlass.mat"),false);
                foreach(float y in new[]{-.6f,.6f})Box("WindowLintel",w+new Vector3(-side*.10f,y,0),new Vector3(.15f,.08f,1.22f),wood,false);
                foreach(float dz in new[]{-.56f,0f,.56f})Box("WindowMullion",w+new Vector3(-side*.11f,0,dz),new Vector3(.13f,1.2f,.05f),wood,false);
            }
            foreach(int slope in new[]{-1,1})
            {
                var tile=Box("PitchedTileRoof",p+new Vector3(slope*width*.25f,height+.65f,0),new Vector3(width*.58f,.13f,depth+.8f),roof,false);tile.transform.rotation=Quaternion.Euler(0,0,slope* -19);
            }
            Box("RoofRidge",p+new Vector3(0,height+1.3f,0),new Vector3(.22f,.15f,depth+.9f),roof,false);
            Box("Porch",new Vector3(face-side*.9f,.12f,p.z),new Vector3(1.8f,.24f,depth),concrete);
            Box("PorchCanopy",new Vector3(face-side*1.0f,2.85f,p.z),new Vector3(2.3f,.10f,depth+.1f),roof,false);
            foreach(float z in new[]{-depth*.46f,depth*.46f})Box("PorchSupport",new Vector3(face-side*1.8f,1.5f,p.z+z),new Vector3(.10f,2.8f,.10f),wood);
            if(!rear)
            {
                Label("ResidenceNumber","MELATI  /  "+(index+12),new Vector3(face-side*.1f,2.55f,p.z-.9f),side>0?Vector3.left:Vector3.right,.11f);
                var lamp=new GameObject("PorchPractical",typeof(Light));lamp.transform.SetParent(root,false);lamp.transform.position=new Vector3(face-side*.8f,2.55f,p.z-.8f);var light=lamp.GetComponent<Light>();light.type=LightType.Point;light.color=new Color(1,.77f,.49f);light.intensity=2.8f;light.range=5;
            }
            root=parent;
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
            var light=lamp.GetComponent<Light>();light.type=LightType.Spot;light.range=12;light.spotAngle=105;light.intensity=9f;light.color=new Color(.93f,.79f,.55f);light.shadows=LightShadows.None;
            var wire=new GameObject("MelatiPowerCable",typeof(LineRenderer));wire.transform.SetParent(root,false);var line=wire.GetComponent<LineRenderer>();line.sharedMaterial=metal;line.startWidth=line.endWidth=.018f;line.positionCount=3;line.SetPositions(new[]{p+Vector3.up*5.1f,p+new Vector3(0,4.7f,-11),p+new Vector3(0,5.1f,-22)});
        }
        private static void BuildShop()
        {
            Vector3 p=new Vector3(8.4f,0,-24);Box("WarungFloor",p+new Vector3(0,.1f,0),new Vector3(5,.2f,6.6f),concrete);
            Box("WarungRearWall",p+new Vector3(2.4f,1.55f,0),new Vector3(.15f,2.9f,6.5f),concrete);
            Box("WarungSouthWall",p+new Vector3(0,1.55f,-3.2f),new Vector3(5,2.9f,.13f),wood);
            Box("WarungNorthWall",p+new Vector3(0,1.55f,3.2f),new Vector3(5,2.9f,.13f),wood);
            Box("WarungAwning",p+new Vector3(-.35f,3.12f,0),new Vector3(6.4f,.16f,7),roof);
            var counter=Box("WarungSalesCounter",p+new Vector3(-2.25f,.64f,0),new Vector3(.65f,1.08f,3.8f),wood);var shop=counter.AddComponent<MelatiShop>();BuildVendor(shop,p);
            Box("WarungCounterTop",p+new Vector3(-2.3f,1.2f,0),new Vector3(.78f,.055f,4),concrete);
            for(int i=0;i<3;i++)
            {
                Box("WarungStockShelf",p+new Vector3(1.6f,.7f+i*.55f,0),new Vector3(.8f,.07f,5.5f),wood);
                for(int j=0;j<7;j++)Box("WarungSupplyPacket",p+new Vector3(1.5f,.83f+i*.55f,-2.1f+j*.65f),new Vector3(.27f,.22f,.18f),Mat(j%2==0?"Upper_BookCover":"Upper_BookPaper"),false);
            }
            for(int i=0;i<5;i++){var fruit=DesignerReviewBuilder.ReviewModel("WarungFreshFruit","RottenApple",root,p+new Vector3(-2.2f,1.29f,-1+i*.45f),new Vector3(.10f,.10f,.10f),false);foreach(var renderer in fruit.GetComponentsInChildren<Renderer>())renderer.sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/Art/Review/Materials/Review_FreshApple.mat");}
            Label("WarungSign","WARUNG BU SRI\nSEMBAKO  •  KOPI  •  BEKAL",p+new Vector3(-2.8f,2.65f,0),Vector3.left,.22f);
            var glow=new GameObject("WarungWarmLight",typeof(Light));glow.transform.SetParent(root,false);glow.transform.position=p+new Vector3(-1.2f,2.7f,0);var lamp=glow.GetComponent<Light>();lamp.type=LightType.Point;lamp.range=8;lamp.intensity=8;lamp.color=new Color(1,.81f,.51f);lamp.shadows=LightShadows.Soft;
            Box("WarungVisibleLamp",glow.transform.position,new Vector3(.5f,.07f,.12f),Mat("Upper_ChandelierLamp"),false);
            Box("WarungBenchSeat",p+new Vector3(-2.8f,.52f,2.6f),new Vector3(.45f,.10f,.95f),wood);
            foreach(float z in new[]{2.25f,2.95f})Box("WarungBenchLeg",p+new Vector3(-2.8f,.28f,z),new Vector3(.33f,.46f,.07f),wood);
        }
        private static void BuildVendor(MelatiShop shop,Vector3 p)
        {
            const string path="Assets/_LayarTerakhir/Art/Characters/Models/BuSriVendor.fbx";
            var importer=(ModelImporter)AssetImporter.GetAtPath(path);importer.importAnimation=true;importer.animationType=ModelImporterAnimationType.Legacy;importer.importCameras=false;importer.importLights=false;importer.SaveAndReimport();
            var vendor=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));vendor.name="BuSriShopkeeper";vendor.transform.SetParent(root,false);vendor.transform.SetPositionAndRotation(p+new Vector3(-1.15f,.2f,0),Quaternion.LookRotation(Vector3.left));
            foreach(var renderer in vendor.GetComponentsInChildren<Renderer>())renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>VendorMaterial(m.name)).ToArray();
            var motion=vendor.GetComponent<Animation>();if(!motion)motion=vendor.AddComponent<Animation>();
            foreach(var clip in AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__"))) {clip.wrapMode=WrapMode.Loop;motion.AddClip(clip,"Idle");}
            var person=vendor.AddComponent<MelatiShopkeeper>();person.motion=motion;person.head=vendor.GetComponentsInChildren<Transform>().First(t=>t.name=="Head");person.player=shop.GetComponentInParent<MelatiNeighborhood>().corridor.player.transform;
            var collider=vendor.AddComponent<CapsuleCollider>();collider.center=Vector3.up*.82f;collider.height=1.64f;collider.radius=.22f;shop.shopkeeper=vendor.transform;
        }
        private static Material VendorMaterial(string name)
        {
            string path="Assets/_LayarTerakhir/Art/Review/Materials/"+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}
            string texture=name.Contains("Skin")?"SriSkin":name.Contains("Hair")?"SriHair":name.Contains("Eyes")?"BrownEyes":name.Contains("Batik")?"SriSkirtJacquard":null;
            mat.color=name.Contains("Blouse")?new Color(.38f,.49f,.36f):name.Contains("Batik")?new Color(.22f,.27f,.34f):name.Contains("Hair")?new Color(.20f,.17f,.14f):Color.white;
            if(texture!=null)mat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/Art/Characters/Textures/"+texture+".png"));
            mat.SetFloat("_Smoothness",.2f);if(name.Contains("Hair")){mat.SetFloat("_AlphaClip",1);mat.SetFloat("_Cull",0);mat.SetFloat("_Cutoff",.4f);mat.EnableKeyword("_ALPHATEST_ON");}EditorUtility.SetDirty(mat);return mat;
        }
        private static void Label(string name,string text,Vector3 position,Vector3 normal,float size)
        {
            var go=new GameObject(name,typeof(TextMesh));go.transform.SetParent(root,false);go.transform.SetPositionAndRotation(position,Quaternion.LookRotation(-normal));
            var label=go.GetComponent<TextMesh>();label.font=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/UI/Fonts/Barlow-Regular.ttf");label.text=text;label.fontSize=48;label.characterSize=size/10;label.anchor=TextAnchor.MiddleCenter;label.color=new Color(.87f,.85f,.70f);go.GetComponent<MeshRenderer>().sharedMaterial=label.font.material;
        }
    }
}
#endif
