#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
namespace LayarTerakhir.Editor
{
    public static partial class Phase3SceneBuilder
    {
        [Serializable] private class ImportedMat {public string name,Albedo,Normal,MetalRough;public float[] color;public float roughness,metallic;}
        [Serializable] private class ImportedManifest {public ImportedMat[] materials;}
        private static readonly Dictionary<string,Material> mats=new Dictionary<string,Material>();
        private static ImportedManifest converted;
        private static void PrepareMaterials()
        {
            Directory.CreateDirectory(Art+"Materials");AssetDatabase.Refresh();mats.Clear();
            converted=JsonUtility.FromJson<ImportedManifest>(File.ReadAllText("Audit/phase3-import-manifest.json"));
            foreach(string f in Directory.GetFiles(Imported,"*",SearchOption.AllDirectories).Where(f=>f.EndsWith(".png")||f.EndsWith(".jpg")))
            {
                var i=AssetImporter.GetAtPath(f) as TextureImporter;if(!i)continue;
                bool normal=f.Contains("nor_gl")||f.Contains("Normal.png"),mask=f.Contains("_arm")||f.Contains("MetalRough");
                var type=normal?TextureImporterType.NormalMap:TextureImporterType.Default;
                if(i.maxTextureSize==2048&&i.textureType==type&&i.sRGBTexture==(!normal&&!mask))continue;
                i.maxTextureSize=2048;i.textureType=type;i.sRGBTexture=!normal&&!mask;i.mipmapEnabled=true;
                i.textureCompression=TextureImporterCompression.Compressed;i.anisoLevel=4;i.SaveAndReimport();
            }
            var importer=AssetImporter.GetAtPath(Imported+"Converted/Models/Wewe.fbx") as ModelImporter;
            if(importer&&importer.animationType!=ModelImporterAnimationType.Legacy)
            {
                importer.animationType=ModelImporterAnimationType.Legacy;importer.importAnimation=true;
                var clips=importer.defaultClipAnimations;foreach(var c in clips)c.loopTime=true;importer.clipAnimations=clips;importer.SaveAndReimport();
            }
        }
        private static Material MaterialFor(string name,string path)
        {
            if(path=="Converted/Models/MineCart")
            {
                string cartPath=Art+"Materials/Cart_RustedMetal.mat";var cart=AssetDatabase.LoadAssetAtPath<Material>(cartPath);
                if(!cart){cart=new Material(Surface("metal_grate_rusty"));AssetDatabase.CreateAsset(cart,cartPath);}
                cart.CopyPropertiesFromMaterial(Surface("metal_grate_rusty"));
                cart.SetColor("_BaseColor",new Color(.22f,.16f,.1f));EditorUtility.SetDirty(cart);return cart;
            }
            if(name==null)name="Surface";string id=path.Contains("PolyHaven/")?path.Split('/')[1]:path.StartsWith("Modules/")?path.Split('/')[1]:null;
            string key=id==null?name:id+"_"+name;
            if(mats.TryGetValue(key,out var result))return result;
            string filename=Art+"Materials/"+string.Concat(key.Select(c=>char.IsLetterOrDigit(c)||c=='_'?c:'_'))+".mat";
            result=AssetDatabase.LoadAssetAtPath<Material>(filename);
            if(!result){result=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(result,filename);}
            result.SetFloat("_Cull",0);result.SetColor("_BaseColor",Color.white);result.SetFloat("_Smoothness",.22f);
            if(id!=null)
            {
                string folder=Imported+"PolyHaven/"+id;
                var files=Directory.GetFiles(folder).Where(x=>x.EndsWith(".jpg")||x.EndsWith(".png")).ToArray();
                string group=name.StartsWith(id+"_")?name.Substring(id.Length+1):"";
                string Find(string kind)
                {
                    var choices=files.Where(f=>Path.GetFileName(f).Contains(kind)).ToArray();
                    return choices.FirstOrDefault(f=>group.Length>0&&Path.GetFileName(f).StartsWith(id+"_"+group+"_"))??choices.FirstOrDefault();
                }
                string diff=Find("_diff.")??Find("_Diffuse."),normal=Find("_nor_gl."),mask=Find("_MetalSmooth.");
                Set("_BaseMap",diff);Set("_BumpMap",normal);Set("_MetallicGlossMap",mask);
                if(normal!=null)result.EnableKeyword("_NORMALMAP");if(mask!=null)result.EnableKeyword("_METALLICSPECGLOSSMAP");
                var arm=Find("_arm.");Set("_OcclusionMap",arm);result.SetFloat("_OcclusionStrength",.6f);
                if(arm!=null)result.EnableKeyword("_OCCLUSIONMAP");
            }
            else
            {
                var m=converted.materials.FirstOrDefault(x=>x.name==name);
                if(m!=null)
                {
                    if(m.color!=null&&m.color.Length==4)result.SetColor("_BaseColor",new Color(m.color[0],m.color[1],m.color[2],m.color[3]));
                    Set("_BaseMap",m.Albedo);Set("_BumpMap",m.Normal);result.SetFloat("_Smoothness",Mathf.Clamp(1-m.roughness,.05f,.65f));result.SetFloat("_Metallic",m.metallic);
                    if(!string.IsNullOrEmpty(m.Normal))result.EnableKeyword("_NORMALMAP");
                    if(name.ToLowerInvariant().Contains("hair")){result.SetFloat("_AlphaClip",1);result.SetFloat("_Cutoff",.32f);result.EnableKeyword("_ALPHATEST_ON");}
                    if(name.StartsWith("P3_Wewe_")&&name.ToLowerInvariant().Contains("hair"))result.SetColor("_BaseColor",new Color(.65f,.63f,.59f));
                }
            }
            EditorUtility.SetDirty(result);mats[key]=result;return result;
            void Set(string property,string texture){if(!string.IsNullOrEmpty(texture))result.SetTexture(property,AssetDatabase.LoadAssetAtPath<Texture2D>(texture));}
        }
        private static Material Surface(string id)
        {
            string key="Surface_"+id;if(mats.TryGetValue(key,out var m))return m;
            string path=Art+"Materials/"+key+".mat";m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
            string folder=Imported+"PolyHaven/"+id+"/";
            string Find(string suffix)=>Directory.GetFiles(folder).FirstOrDefault(f=>f.Contains(suffix)&&!f.EndsWith(".meta"));
            string diff=Find("_diff.")??Find("_Diffuse."),normal=Find("_nor_gl."),mask=Find("_MetalSmooth.");
            if(diff!=null)m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(diff));
            if(normal!=null){m.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(normal));m.EnableKeyword("_NORMALMAP");}
            if(mask!=null){m.SetTexture("_MetallicGlossMap",AssetDatabase.LoadAssetAtPath<Texture2D>(mask));m.EnableKeyword("_METALLICSPECGLOSSMAP");}
            m.SetFloat("_Cull",0);m.SetFloat("_Smoothness",.22f);EditorUtility.SetDirty(m);mats[key]=m;return m;
        }
        private static Material ParticleMat(string name,string file,Color colour)
        {
            string path=Art+"Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));AssetDatabase.CreateAsset(m,path);}
            m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Imported+"Particles/"+file));m.SetColor("_BaseColor",colour);
            m.SetFloat("_Surface",1);m.SetFloat("_ZWrite",0);m.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);m.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");m.renderQueue=3000;return m;
        }
    }
}
#endif
