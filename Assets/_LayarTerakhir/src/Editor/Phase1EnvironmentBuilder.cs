#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static partial class Phase1SceneBuilder
    {
        private static readonly Dictionary<string, Material> surfaceVariants = new Dictionary<string, Material>();
        private static Material parkingSurface;
        [MenuItem("LayarTerakhir/Player/Rebuild Existing Phone Grasp")]
        public static void RebuildPhoneGrasp()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            PrepareExistingPhoneGrasp();AssetDatabase.SaveAssets();
        }
        private static void PrepareExistingPhoneGrasp()
        {
            string source=Art+"Characters/Models/PhoneCameraGrip.fbx";
            var root=new GameObject("RakaCameraGrip");
            var correction=new GameObject("SourceUnits");correction.transform.SetParent(root.transform,false);
            var orientation=new GameObject("SourceBasis");orientation.transform.SetParent(correction.transform,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(source));model.transform.SetParent(orientation.transform,false);
            // Store explicit renderer/material bindings rather than stale nested
            // FBX override IDs after a source mesh has been re-exported.
            PrefabUtility.UnpackPrefabInstance(model,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            Transform Guide(string name)=>model.GetComponentsInChildren<Transform>().Single(t=>t.name==name);
            var origin=Guide("GripOrigin").position;
            var right=Guide("GripRight").position-origin;var up=Guide("GripUp").position-origin;var forward=Guide("GripForward").position-origin;
            model.transform.position-=origin;
            var q=Quaternion.Inverse(Quaternion.LookRotation(forward.normalized,up.normalized));orientation.transform.localRotation=q;
            correction.transform.localScale=new Vector3(Mathf.Sign((q*right).x)*.1f/right.magnitude,.1f/up.magnitude,.1f/forward.magnitude);
            var surface=Plain("SourcedPhoneHand",Color.white,.25f);
            const string materials="Assets/_LayarTerakhir/assets/Art/Characters/Materials/";
            var characterSurface=AssetDatabase.LoadAssetAtPath<Material>(materials+"Raka_Skin.mat");
            surface.SetTexture("_BaseMap",characterSurface.GetTexture("_BaseMap"));
            surface.SetTexture("_BumpMap",characterSurface.GetTexture("_BumpMap"));
            surface.SetFloat("_BumpScale",.45f);if(surface.GetTexture("_BumpMap"))surface.EnableKeyword("_NORMALMAP");else surface.DisableKeyword("_NORMALMAP");surface.SetFloat("_Cull",0);
            foreach(var renderer in model.GetComponentsInChildren<Renderer>(true))
                renderer.sharedMaterial=renderer.name.Contains("Jacket")?AssetDatabase.LoadAssetAtPath<Material>(materials+"Raka_Clothes.mat"):
                    renderer.name.Contains("TShirt")?AssetDatabase.LoadAssetAtPath<Material>(materials+"Raka_TShirt.mat"):surface;
            PrefabUtility.SaveAsPrefabAsset(root,"Assets/Resources/RakaCameraGrip.prefab");Object.DestroyImmediate(root);
        }

        private static Material EnvironmentSurface(string name, string directory, string prefix, Color tint, float smoothness = .32f)
        {
            var material = Plain(name, tint, smoothness);
            foreach(string legacy in new[]{"_MainTex","_SpecGlossMap","_DetailAlbedoMap","_DetailNormalMap","_DetailMask","_ParallaxMap","_EmissionMap"})material.SetTexture(legacy,null);
            foreach(string keyword in new[]{"_NORMALMAP","_METALLICSPECGLOSSMAP","_OCCLUSIONMAP"}) material.DisableKeyword(keyword);
            foreach(var channel in new[]{("Diffuse","_BaseMap"),("Normal","_BumpMap"),("MetalSmooth","_MetallicGlossMap"),("Occlusion","_OcclusionMap")})
            {
                string path = Directory.GetFiles(directory,prefix+channel.Item1+".*").FirstOrDefault(p=>!p.EndsWith(".meta"));
                if(path==null){material.SetTexture(channel.Item2,null);continue;}
                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.textureType=channel.Item1=="Normal"?TextureImporterType.NormalMap:TextureImporterType.Default;
                importer.sRGBTexture=channel.Item1=="Diffuse";importer.maxTextureSize=2048;importer.mipmapEnabled=true;
                importer.wrapMode=TextureWrapMode.Repeat;importer.SaveAndReimport();
                material.SetTexture(channel.Item2,AssetDatabase.LoadAssetAtPath<Texture2D>(path));
            }
            material.SetFloat("_BumpScale",1.05f);material.SetFloat("_OcclusionStrength",.85f);
            if(material.GetTexture("_BumpMap"))material.EnableKeyword("_NORMALMAP");
            if(material.GetTexture("_MetallicGlossMap"))material.EnableKeyword("_METALLICSPECGLOSSMAP");
            if(material.GetTexture("_OcclusionMap"))material.EnableKeyword("_OCCLUSIONMAP");
            material.SetFloat("_Cull",2);EditorUtility.SetDirty(material);return material;
        }
        private static void EnvironmentMaterials()
        {
            surfaceVariants.Clear();
            foreach(string path in Directory.GetFiles(Art+"Materials","*.mat"))
            {
                var parts=Path.GetFileNameWithoutExtension(path).Split('_');
                if(parts.Length>=3&&int.TryParse(parts[parts.Length-1],out _)&&int.TryParse(parts[parts.Length-2],out _))AssetDatabase.DeleteAsset(path);
            }
            concrete=EnvironmentSurface("StructuralConcrete",Imported+"ConcreteInterior","Wall",new Color(.78f,.8f,.77f));
            // Peeling, mottled plaster (licensed worn scan) instead of the clean
            // modular wall print: DreadOut-reference walls are heavily stained.
            plaster=EnvironmentSurface("DampPlaster",Imported+"Surfaces/worn_cracked_plaster","worn_cracked_plaster",new Color(.50f,.55f,.51f));
            soot=EnvironmentSurface("CharredLimewash",Imported+"Surfaces/worn_cracked_plaster","worn_cracked_plaster",new Color(.27f,.29f,.27f));
            tiles=EnvironmentSurface("WetCorridorTile",Imported+"Surfaces/blue_floor_tiles_01","blue_floor_tiles_01",new Color(.63f,.71f,.65f),.48f);
            rust=EnvironmentSurface("CorrodedPaintedMetal",Imported+"ConcreteInterior","Metal",new Color(.67f,.57f,.46f),.4f);
            darkMetal=EnvironmentSurface("WornSteel",Imported+"Surfaces/rust_coarse_01","rust_coarse_01",new Color(.2f,.24f,.23f),.4f);
            timber=EnvironmentSurface("ScorchedWood",Imported+"DoorsWindows","Door",new Color(.55f,.48f,.37f),.22f);
            moss=EnvironmentSurface("DampMoss",Imported+"Surfaces/worn_mossy_plasterwall","worn_mossy_plasterwall",new Color(.5f,.56f,.43f));
            // Per-floor visual identities from the same licensed Poly Haven scans:
            // rain-soaked L1 tile, fire-blackened L4 tile, and two-tone wainscot
            // bands (dark lower wall, as in the rusunawa/DreadOut references).
            tilesWet=EnvironmentSurface("WetDampTile",Imported+"Surfaces/blue_floor_tiles_01","blue_floor_tiles_01",new Color(.45f,.53f,.56f),.62f);
            tilesBurnt=EnvironmentSurface("BurntTile",Imported+"Surfaces/blue_floor_tiles_01","blue_floor_tiles_01",new Color(.20f,.19f,.17f),.30f);
            bandDamp=EnvironmentSurface("DampWainscot",Imported+"Surfaces/worn_mossy_plasterwall","worn_mossy_plasterwall",new Color(.30f,.37f,.28f),.42f);
            bandStd=EnvironmentSurface("WornWainscot",Imported+"ConcreteInterior","Wall",new Color(.36f,.41f,.39f),.38f);
            bandChar=EnvironmentSurface("CharredWainscot",Imported+"Surfaces/worn_cracked_plaster","worn_cracked_plaster",new Color(.11f,.11f,.105f),.26f);
            parkingSurface=EnvironmentSurface("ParkingAsphalt",Imported+"Surfaces/asphalt_02","asphalt_02",new Color(.71f,.75f,.76f),.28f);
            EnvironmentSurface("External_DoorsWindows",Imported+"DoorsWindows","Door",Color.white,.35f);
            paper=Plain("OldPaper",new Color(.64f,.61f,.48f));
            glass=Plain("Rainwater",new Color(.10f,.14f,.14f),.85f,.3f);
            emissive=Plain("FluorescentPhosphor",new Color(.7f,.85f,.78f));emissive.EnableKeyword("_EMISSION");emissive.SetColor("_EmissionColor",new Color(.4f,.7f,.56f)*2);
        }
        private static bool ModularSurface(string name, Vector3 size, Material material, Transform parent)
        {
            if(parent&&parent!=current)return false;
            if(name.StartsWith("K102_")||name=="K401_BreakawayPanel"||name.StartsWith("K404_")||name.Contains("WaterStain"))return false;
            return (material==concrete||material==plaster||material==soot||material==tiles||material==parkingSurface||material==moss||material==tilesWet||material==tilesBurnt||material==bandDamp||material==bandStd||material==bandChar) &&
                   (size.y<.5f&&size.x>=2&&size.z>=2||size.y>=.7f&&(size.x<1&&size.z>=2||size.z<1&&size.x>=2));
        }
        // Repeated publisher panels retain their original topology and UVs.
        // Colliders keep the previous closed shell and stair openings exactly.
        private static GameObject ModularBox(string name, Vector3 p, Vector3 size, Material material, bool collision, Transform parent)
        {
            var root=new GameObject(name);root.transform.SetParent(parent,false);root.transform.position=p;
            if(collision)root.AddComponent<BoxCollider>().size=size;
            if(size.y<.5f)
            {
                PanelGrid(root.transform,p+Vector3.up*size.y*.5f,Vector3.up,Vector3.forward,size.x,size.z,material,true);
                if(p.y>0)PanelGrid(root.transform,p-Vector3.up*size.y*.5f,Vector3.down,Vector3.forward,size.x,size.z,concrete,true);
                foreach(int side in new[]{-1,1})
                {
                    PanelGrid(root.transform,p+Vector3.forward*(side*size.z*.5f),Vector3.forward*side,Vector3.up,size.x,size.y,material,false);
                    PanelGrid(root.transform,p+Vector3.right*(side*size.x*.5f),Vector3.right*side,Vector3.up,size.z,size.y,material,false);
                }
                return root;
            }
            if(size.z<1)
            {
                PanelGrid(root.transform,p+Vector3.forward*size.z*.5f,Vector3.forward,Vector3.up,size.x,size.y,material,false);
                PanelGrid(root.transform,p-Vector3.forward*size.z*.5f,Vector3.back,Vector3.up,size.x,size.y,material,false);
                foreach(int side in new[]{-1,1})PanelGrid(root.transform,p+Vector3.right*(side*size.x*.5f),Vector3.right*side,Vector3.up,size.z,size.y,material,false);
                PanelGrid(root.transform,p+Vector3.up*size.y*.5f,Vector3.up,Vector3.forward,size.x,size.z,material,true);
            }
            else if(size.x<1)
            {
                PanelGrid(root.transform,p+Vector3.right*size.x*.5f,Vector3.right,Vector3.up,size.z,size.y,material,false);
                PanelGrid(root.transform,p-Vector3.right*size.x*.5f,Vector3.left,Vector3.up,size.z,size.y,material,false);
                foreach(int side in new[]{-1,1})PanelGrid(root.transform,p+Vector3.forward*(side*size.z*.5f),Vector3.forward*side,Vector3.up,size.x,size.y,material,false);
                PanelGrid(root.transform,p+Vector3.up*size.y*.5f,Vector3.up,Vector3.forward,size.x,size.z,material,true);
            }
            else throw new InvalidOperationException("Architecture needs a slab or wall: "+name);
            return root;
        }
        private static void PanelGrid(Transform parent,Vector3 centre,Vector3 normal,Vector3 up,float width,float height,Material material,bool floor)
        {
            var mesh=ImportedMesh(Imported+"ConcreteInterior/"+(floor?"Floor":"Wall")+".fbx");
            var n=mesh.normals[0].normalized;
            Vector3 sourceUp=floor?Vector3.forward:Vector3.up;
            if(Mathf.Abs(Vector3.Dot(n,sourceUp))>.9f)sourceUp=Vector3.right;
            var sourceRight=Vector3.Cross(sourceUp,n).normalized;
            sourceUp=Vector3.Cross(n,sourceRight).normalized;
            var sourceSize=mesh.bounds.size;
            float baseW=Vector3.Dot(Abs(sourceRight),sourceSize),baseH=Vector3.Dot(Abs(sourceUp),sourceSize);
            var rotation=Quaternion.LookRotation(normal,up)*Quaternion.Inverse(Quaternion.LookRotation(n,sourceUp));
            var right=Vector3.Cross(up,normal).normalized;
            const float maxWidth=6;
            float maxHeight=floor?6:3.6f;
            for(float x=0;x<width-.001f;x+=maxWidth)for(float y=0;y<height-.001f;y+=maxHeight)
            {
                float w=Mathf.Min(maxWidth,width-x),h=Mathf.Min(maxHeight,height-y);
                var scale=Abs(sourceRight)*(w/baseW)+Abs(sourceUp)*(h/baseH)+Abs(n);
                var go=new GameObject("SourcePanel_"+(floor?"Floor":"Wall"),typeof(MeshFilter),typeof(MeshRenderer));go.transform.SetParent(parent,false);
                go.transform.rotation=rotation;go.transform.localScale=scale;
                go.transform.position=centre+right*(x+w*.5f-width*.5f)+up*(y+h*.5f-height*.5f)-rotation*Vector3.Scale(mesh.bounds.center,scale);
                go.GetComponent<MeshFilter>().sharedMesh=mesh;
                // FBX vertices use centimeters while publisher panel UVs describe
                // 2m width / 3m wall height. Mesh fitting and texture density differ.
                go.GetComponent<MeshRenderer>().sharedMaterial=TiledMaterial(material,w/2,h/(floor?2:3));
                GameObjectUtility.SetStaticEditorFlags(go,StaticEditorFlags.BatchingStatic);
            }
        }
        private static Vector3 Abs(Vector3 v)=>new Vector3(Mathf.Abs(v.x),Mathf.Abs(v.y),Mathf.Abs(v.z));
        private static Material TiledMaterial(Material source,float u,float v)
        {
            string key=source.name+"_"+Mathf.RoundToInt(u*1000)+"_"+Mathf.RoundToInt(v*1000);
            if(surfaceVariants.TryGetValue(key,out var cached))return cached;
            string path=Art+"Materials/"+key+".mat";
            var material=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!material){material=new Material(source);AssetDatabase.CreateAsset(material,path);}
            material.CopyPropertiesFromMaterial(source);material.name=key;
            foreach(string channel in new[]{"_BaseMap","_BumpMap","_MetallicGlossMap","_OcclusionMap"})material.SetTextureScale(channel,new Vector2(u,v));
            EditorUtility.SetDirty(material);surfaceVariants[key]=material;return material;
        }
        private static void EnvironmentDressing()
        {
            current=Group("Downloaded_Rusunawa_Furniture");
            SourceProp("metal_office_desk","GuardDesk",new Vector3(-8,.465f,-23.15f),.93f);
            SourceProp("folding_wooden_stool","GuardStool",new Vector3(-8,.23f,-21.9f),.46f);
            SourceProp("metal_trash_can","GuardBin",new Vector3(-10,.45f,-25.8f),.9f);
            for(int floor=0;floor<4;floor++)
            {
                float y=floor*4;
                if(floor!=2)
                {
                SourceProp("painted_wooden_bench","ResidentsBench_L"+floor,new Vector3(9,y+.42f,12.25f),.84f);
                SourceProp("metal_trash_can","ResidentsBin_L"+floor,new Vector3(12.4f,y+.43f,12.2f),.86f);
                for(int side=-1;side<=1;side+=2)
                {
                    var bags=SourceProp("trashbag","CollectedRefuse_L"+floor,new Vector3(side*16.1f,y+.25f,8),.5f,false);
                    bags.transform.rotation=Quaternion.Euler(0,side*33+floor*67,0);
                    SourceProp("folding_wooden_stool","AbandonedResidentsStool_L"+floor,new Vector3(side*16,y+.23f,-5),.46f);
                }
                }
                // Real slab edges and column rhythm replace the uninterrupted block silhouette.
                Box("FacadeSlabEdge",new Vector3(0,y+3.7f,-20.36f),new Vector3(48.8f,.6f,.5f),concrete);
                Box("RearSlabEdge",new Vector3(0,y+3.7f,20.25f),new Vector3(48.8f,.6f,.5f),concrete);
                foreach(int side in new[]{-1,1})Box("SideSlabEdge",new Vector3(side*24.2f,y+3.7f,0),new Vector3(.5f,.6f,40),concrete);
                for(int bay=0;bay<9;bay++)
                {
                    float x=-24+bay*6;
                    SourceProp("ConcreteInterior","FacadePier",new Vector3(x,y+1.8f,-20.45f),3.6f,false,new Vector3(.34f,3.6f,.36f),concrete,Imported+"ConcreteInterior/Pillar.fbx");
                }
            }
            SourceProp("worn_metal_rack","ArchiveShelves",new Vector3(10,4.9f,19),1.8f);
            SourceProp("worn_metal_rack","PumpMaintenanceRack",new Vector3(-30,1,-13.7f),2);
            SourceProp("metal_office_desk","InvestigationDesk403",new Vector3(9,12.43f,16.5f),.86f);
            SourceProp("folding_wooden_stool","InvestigationStool403",new Vector3(9,12.23f,15.4f),.46f);
            SourceProp("worn_metal_rack","KitchenUtensilRack",new Vector3(23,12.9f,7),1.8f);
            current=environment;
        }
        [Serializable] private sealed class RoomDressingPlacement
        {
            public string id, role;
            public Vector3 supportPoint, centre, size;
        }
        [Serializable] private sealed class RoomDressingReceipt
        {
            public int placements; public string utc; public RoomDressingPlacement[] items;
        }
        private static readonly List<RoomDressingPlacement> roomDressing = new List<RoomDressingPlacement>();
        private static void RoomDress(string id, string role, Vector3 support, float maxSize, bool collision = false, float yaw = 0, Vector3? normal = null, float pitch = 0)
        {
            var n = normal ?? Vector3.up;
            var root = SourceProp(id, "P1Room_" + id + "__" + role, support, 1, collision, maxDimension: maxSize);
            root.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
            var renderers = root.GetComponentsInChildren<Renderer>(); var bounds = renderers[0].bounds;
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            float extent = Vector3.Dot(new Vector3(Mathf.Abs(n.x), Mathf.Abs(n.y), Mathf.Abs(n.z)), bounds.extents);
            root.transform.position += n * (extent + .006f);
            roomDressing.Add(new RoomDressingPlacement { id = id, role = role, supportPoint = support, centre = root.transform.position, size = bounds.size });
        }
        private static void RoomDressing()
        {
            roomDressing.Clear(); current = Group("RusunawaRoomDressing");
            // L1 service lobby corner.
            RoomDress("metal_trash_can", "EntryLobbyBin", new Vector3(-23, 0, -12.4f), .9f, true);
            // Threshold traces outside closed doors: sandals, plants, laundry, crates.
            RoomDress("rubber_boots", "ThresholdSandals105", new Vector3(-12.9f, 0, -12.5f), .32f, false, 20);
            RoomDress("plastic_crate_02", "ThresholdCrate106", new Vector3(-4.1f, 0, -12.5f), .55f, true);
            RoomDress("potted_plant_04", "ThresholdPlant107", new Vector3(2.2f, 0, -12.5f), .58f);
            RoomDress("wicker_basket_01", "ThresholdLaundry108", new Vector3(11.9f, 0, -12.5f), .5f, true);
            RoomDress("rubber_boots", "ThresholdSandals109", new Vector3(-16.4f, 0, 7.2f), .32f, false, -15);
            RoomDress("potted_plant_04", "ThresholdPlant110", new Vector3(16.4f, 0, 3.2f), .58f);
            RoomDress("rubber_boots", "ThresholdSandals205", new Vector3(-12.9f, 4, -12.5f), .32f, false, 35);
            RoomDress("plastic_crate_02", "ThresholdCrate206", new Vector3(-4.1f, 4, -12.5f), .55f, true);
            RoomDress("medical_box", "ThresholdFirstAid207", new Vector3(2.2f, 4, -12.5f), .38f, true);
            RoomDress("potted_plant_04", "ThresholdPlant208", new Vector3(11.9f, 4, -12.5f), .58f);
            RoomDress("rubber_boots", "ThresholdSandals209", new Vector3(-16.4f, 4, -10.2f), .32f, false, -30);
            RoomDress("wicker_basket_01", "ThresholdLaundry210", new Vector3(-16.4f, 4, 2.8f), .5f, true);
            RoomDress("potted_plant_04", "ThresholdPlant211", new Vector3(16.4f, 4, 4.8f), .58f);
            RoomDress("cardboard_box_01", "ArchiveHallBox", new Vector3(4.3f, 4, 12.4f), .6f, true);
            // Archive room interior: the open 203 doorway leads somewhere used.
            RoomDress("folding_wooden_stool", "ArchiveStool", new Vector3(2.2f, 4, 14.5f), .46f);
            RoomDress("cardboard_box_01", "ArchiveBoxA", new Vector3(8, 4, 18), .6f, true);
            RoomDress("trashbag", "ArchiveRefuse", new Vector3(9, 4, 17.5f), .5f, true);
            RoomDress("small_plastic_torch", "ArchiveTorch", new Vector3(5.5f, 5.4f, 18.9f), .22f);
            RoomDress("stationery_supplies", "ArchiveStationery", new Vector3(5, 5.4f, 18.9f), .24f);
            RoomDress("wall_clock", "ArchiveClock", new Vector3(7, 6.2f, 19.97f), .34f, false, 180, Vector3.back);
            // K403 stays a usable stealth room: edges dressed, centre and desk clear.
            RoomDress("worn_metal_rack", "K403Wardrobe", new Vector3(5.8f, 12, 19), 1.8f, true);
            RoomDress("wicker_basket_01", "K403Laundry", new Vector3(8.2f, 12, 18.6f), .5f, true);
            RoomDress("cardboard_box_01", "K403StoredBox", new Vector3(8.3f, 12, 15.5f), .6f, true);
            RoomDress("metal_trash_can", "K403Bin", new Vector3(5.4f, 12, 14), .9f, true);
            RoomDress("wall_clock", "K403Clock", new Vector3(11.36f, 14, 16.7f), .34f, false, -90, Vector3.left);
            // Bathroom: water jars and a crate outside the EVP centre and stall path.
            RoomDress("metal_jug", "BathroomWaterJarA", new Vector3(-21.8f, 12, 3.5f), .3f);
            RoomDress("metal_jug", "BathroomWaterJarB", new Vector3(-21.4f, 12, 3.9f), .3f);
            RoomDress("plastic_crate_02", "BathroomCrate", new Vector3(-18.5f, 12, 4.5f), .55f, true);
            RoomDress("trashbag", "BathroomRefuse", new Vector3(-17.2f, 12, -.5f), .5f, true);
            // Kitchen corners, away from the kiln sightline and approach.
            RoomDress("plastic_crate_02", "KitchenCrate", new Vector3(18, 12, 7.2f), .55f, true);
            RoomDress("cardboard_box_01", "KitchenBox", new Vector3(23, 12, 7.4f), .6f, true);
            RoomDress("metal_trash_can", "KitchenBin", new Vector3(17.6f, 12, 6.8f), .9f, true);
            RoomDress("wall_clock", "KitchenClock", new Vector3(19, 14.2f, 7.88f), .34f, false, 180, Vector3.back);
            // Overhaul dressing: every floor/room gets a functional identity.
            RoomDress("cassette_player", "GuardDeskCassette", new Vector3(-9.35f, .93f, -23.2f), .3f);
            RoomDress("metal_jerrycan_green", "PumpSpareFuel", new Vector3(-31, 0, -13.5f), .48f, true);
            RoomDress("plastic_broom", "L1LobbyBroom", new Vector3(-23.4f, 0, -12.55f), 1.15f, false, 12);
            RoomDress("cardboard_box_01", "ArchiveBoxB", new Vector3(6.8f, 4, 17.2f), .6f, true);
            RoomDress("plastic_broom", "L3CleaningBroom", new Vector3(4.6f, 8, -12.55f), 1.15f, false, 15);
            RoomDress("rubber_boots", "L3CleanerBoots", new Vector3(5.4f, 8, -12.5f), .32f);
            RoomDress("metal_jug", "L3CleaningJug", new Vector3(3.4f, 8, -12.55f), .3f);
            RoomDress("trashbag", "L3CorridorRefuse", new Vector3(-6, 8, -12.5f), .5f, true);
            RoomDress("cement_bag", "L3RepairBag", new Vector3(14, 8, -12.5f), .65f, true);
            RoomDress("trashbag", "K401CharredRefuse", new Vector3(-11.2f, 12, 18.2f), .5f, true);
            RoomDress("cardboard_box_01", "K401StoredBox", new Vector3(-8.4f, 12, 18.6f), .6f, true);
            RoomDress("folding_wooden_stool", "K402Stool", new Vector3(-3.2f, 12, 18.2f), .46f);
            RoomDress("wicker_basket_01", "K402Basket", new Vector3(-.4f, 12, 18.6f), .5f, true);
            RoomDress("ceramic_pot", "BathroomBucketPot", new Vector3(-18, 12, -.9f), .28f);
            RoomDress("carved_wooden_plate", "AltarOfferingPlate", new Vector3(-.95f, 13.1f, -17.5f), .32f);
            RoomDress("ceramic_pot", "AltarClayPot", new Vector3(.95f, 13.1f, -17.9f), .42f);
            RoomDress("brass_pan_01", "AltarBrassPan", new Vector3(.25f, 13.1f, -18.15f), .4f);
            RoomDress("metal_jerrycan_green", "AltarFuelCan", new Vector3(1.9f, 12, -16.1f), .48f, true);
            RoomDress("potted_plant_04", "CourtyardDeadPlant", new Vector3(-12.5f, 0, 8.6f), .58f);
            RoomDress("plastic_crate_02", "CourtyardStorageCrate", new Vector3(12.6f, 4, 8.6f), .55f, true);
            // Guard radio on the desk, pump clock on the back wall, lobby props.
            RoomDress("vintage_radio_transceiver", "GuardRadio", new Vector3(-7.6f, .93f, -23.3f), .33f);
            RoomDress("wall_clock", "PumpClock", new Vector3(-27, 2.2f, -8.17f), .34f, false, 180, Vector3.back);
            RoomDress("metal_trash_can", "PumpBin", new Vector3(-26.5f, 0, -13), .9f, true);
            RoomDress("utility_box_01", "LiftLandingBoxL1", new Vector3(-17.6f, 0, -12.5f), .5f, true);
            RoomDress("cardboard_box_01", "LiftLandingBoxL2", new Vector3(-17.6f, 4, -12.5f), .6f, true);
            // L4 gallery rubble near the claw door, laundry baskets by the lines.
            RoomDress("trashbag", "L4GalleryRefuse", new Vector3(10, 12, 12.3f), .5f, true);
            RoomDress("cement_bag", "L4GalleryRubble", new Vector3(-8.8f, 12, -9.7f), .65f, true);
            RoomDress("wicker_basket_01", "LaundryBasketL1A", new Vector3(-6.5f, 0, 9.6f), .5f, true);
            RoomDress("wicker_basket_01", "LaundryBasketL1B", new Vector3(4.5f, 0, 9.6f), .5f, true);
            RoomDress("wicker_basket_01", "LaundryBasketL2A", new Vector3(-1.5f, 4, 9.6f), .5f, true);
            RoomDress("wicker_basket_01", "LaundryBasketL2B", new Vector3(8.5f, 4, 9.6f), .5f, true);
            File.WriteAllText("Audit/phase1-room-dressing.json", JsonUtility.ToJson(new RoomDressingReceipt
            {
                placements = roomDressing.Count, utc = DateTime.UtcNow.ToString("o"), items = roomDressing.ToArray()
            }, true) + "\n");
            current = environment;
        }
    }
}
#endif
