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
            material.SetFloat("_BumpScale",.7f);material.SetFloat("_OcclusionStrength",.7f);
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
            plaster=EnvironmentSurface("DampPlaster",Imported+"ConcreteInterior","Wall",new Color(.72f,.76f,.7f));
            soot=EnvironmentSurface("CharredLimewash",Imported+"Surfaces/worn_cracked_plaster","worn_cracked_plaster",new Color(.27f,.29f,.27f));
            tiles=EnvironmentSurface("WetCorridorTile",Imported+"Surfaces/blue_floor_tiles_01","blue_floor_tiles_01",new Color(.63f,.71f,.65f),.48f);
            rust=EnvironmentSurface("CorrodedPaintedMetal",Imported+"ConcreteInterior","Metal",new Color(.67f,.57f,.46f),.4f);
            darkMetal=EnvironmentSurface("WornSteel",Imported+"Surfaces/rust_coarse_01","rust_coarse_01",new Color(.2f,.24f,.23f),.4f);
            timber=EnvironmentSurface("ScorchedWood",Imported+"DoorsWindows","Door",new Color(.55f,.48f,.37f),.22f);
            moss=EnvironmentSurface("DampMoss",Imported+"Surfaces/worn_mossy_plasterwall","worn_mossy_plasterwall",new Color(.5f,.56f,.43f));
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
            return (material==concrete||material==plaster||material==soot||material==tiles||material==parkingSurface) &&
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
    }
}
#endif
