#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
namespace LayarTerakhir.Editor
{
    public static partial class Phase2SceneBuilder
    {
        [Serializable] private sealed class ImportedMaterials{public ImportedMaterial[] materials;}
        [Serializable] private sealed class ImportedMaterial
        {public string name,Albedo,Normal,MetalRough;public float[] color;public float roughness,metallic;public bool transparent;}
        private static void PrepareMaterials()
        {
            Directory.CreateDirectory(Art+"Materials");materials.Clear();
            var fields=new[]{
                ("Bed","bed/Bed_"),("Chairs_tables_1","Chairs_table_1/Chairs_tables_1_"),
                ("Ceiling_light","Ceiling_light/Ceiling_light_"),("Ceiling_1","ceiling_1/Ceiling_1_"),
                ("Door_mat","Door_1/Door_1_"),("Doorway_1","Doorway_1/Doorway_1_"),("Exit_sign","exit_sign/Exit_sign_"),("Floors_1","Floors_1/Floors_1_"),
                ("IV_bag","Iv_bag/IV_Bag_IV_pole_bag_"),("IV_Pole","Iv_pole/IV_pole_bag_"),
                ("Magazine","Magazine/Magazine_"),("Walls_1","Walls_1/Walls_1_"),("Wheel_chair","wheel_chair/Wheel_chair_")};
            foreach(var field in fields)
            {
                string prefix=Imported+"Madduck/Textures/"+field.Item2;
                Texture2D Load(string a,string b=null)=>AssetDatabase.LoadAssetAtPath<Texture2D>(prefix+a+".png")??(b==null?null:AssetDatabase.LoadAssetAtPath<Texture2D>(prefix+b+".png"));
                Surface(field.Item1,Color.white,Load("BaseColor","Base_color"),Load("Normal"),Load("MetalSmooth"));
            }
            Surface("Window",new Color(.13f,.22f,.24f,.25f),null,null,null,.95f,.1f,true);
            var input=JsonUtility.FromJson<ImportedMaterials>(File.ReadAllText("Audit/phase2-import-manifest.json"));
            if(File.Exists("Audit/phase2-session2-import-manifest.json")) input.materials=input.materials.Concat(JsonUtility.FromJson<ImportedMaterials>(File.ReadAllText("Audit/phase2-session2-import-manifest.json")).materials).ToArray();
            foreach(var m in input.materials)
            {
                string mask=string.IsNullOrEmpty(m.MetalRough)?null:m.MetalRough.Replace("MetalRough","MetalSmooth");
                Surface(m.name,m.color!=null&&m.color.Length>=3?new Color(m.color[0],m.color[1],m.color[2],m.color.Length>3?m.color[3]:1):Color.white,
                    AssetDatabase.LoadAssetAtPath<Texture2D>(m.Albedo??""),AssetDatabase.LoadAssetAtPath<Texture2D>(m.Normal??""),
                    AssetDatabase.LoadAssetAtPath<Texture2D>(mask??""),1-m.roughness,m.metallic,m.transparent);
            }
            foreach(var folder in Directory.GetDirectories(Imported+"PolyHaven"))
            {
                string id=Path.GetFileName(folder);
                var albedo=Directory.GetFiles(folder,id+"Diffuse.*").First(p=>!p.EndsWith(".meta"));
                var normal=Directory.GetFiles(folder,id+"Normal.*").First(p=>!p.EndsWith(".meta"));
                Surface(id,Color.white,AssetDatabase.LoadAssetAtPath<Texture2D>(albedo),AssetDatabase.LoadAssetAtPath<Texture2D>(normal),AssetDatabase.LoadAssetAtPath<Texture2D>(folder+"/"+id+"MetalSmooth.png"));
            }
            Surface("LastriSurface",Color.white,AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Characters/Textures/LastriAlbedo.png"),
                AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Characters/Textures/LastriNormal.png"),AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Characters/Textures/LastriMetalSmooth.png"));
            Surface("ColdGlass",new Color(.47f,.65f,.67f,.72f),materials["Walls_1"].GetTexture("_BaseMap") as Texture2D,null,null,.87f,.15f,true);
            Surface("RitualAura",new Color(.7f,.025f,.012f),null,null,null,.2f,0);
            var aura=materials["RitualAura"];aura.EnableKeyword("_EMISSION");aura.SetColor("_EmissionColor",new Color(1.8f,.035f,.01f));
        }
        private static Material Surface(string name,Color color,Texture2D albedo,Texture2D normal,Texture2D mask,float smooth=.35f,float metal=0,bool transparent=false)
        {
            string safe=string.Concat(name.Select(c=>char.IsLetterOrDigit(c)||c=='_'?c:'_'));
            string path=Art+"Materials/"+safe+".mat";
            var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
            m.SetColor("_BaseColor",color);m.SetTexture("_BaseMap",albedo);m.SetTexture("_BumpMap",normal);m.SetFloat("_BumpScale",.8f);
            m.SetTexture("_MetallicGlossMap",mask);m.SetFloat("_Smoothness",smooth);m.SetFloat("_Metallic",metal);
            if(normal)m.EnableKeyword("_NORMALMAP");if(mask)m.EnableKeyword("_METALLICSPECGLOSSMAP");m.SetFloat("_Cull",0);m.enableInstancing=true;
            if(transparent)
            {m.SetShaderPassEnabled("ShadowCaster",false);m.SetShaderPassEnabled("DepthOnly",false);m.SetShaderPassEnabled("DepthNormals",false);m.SetFloat("_Surface",1);m.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);m.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);m.SetFloat("_ZWrite",0);m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");m.renderQueue=3000;}
            materials[name]=m;EditorUtility.SetDirty(m);return m;
        }
        private static Material ResolveMaterial(Material source,string path)
        {
            if(path.StartsWith("PolyHaven/"))return materials[path.Split('/')[1]];
            if(path.StartsWith("Kenney/"))return materials["Floors_1"];
            if(source)
            {
                string name=source.name.Replace(" (Instance)","");
                if(materials.TryGetValue(name,out var material))return material;
                string Canon(string value)=>new string(value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
                var alias=materials.FirstOrDefault(pair=>Canon(pair.Key)==Canon(name));
                if(alias.Value)return alias.Value;
                if(name.StartsWith("Material_"))return materials["Walls_1"];
            }
            throw new InvalidOperationException("Missing imported hospital surface: "+(source?source.name:"null")+" at "+path);
        }
    }
}
#endif
