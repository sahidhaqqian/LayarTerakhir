#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Incremental revision: preserve the authored rooms, evidence, actors and chase route.
    public static class AtmajaStairRevisionBuilder
    {
        private static AtmajaDirector d;
        private static Transform root;
        private static Vector3 P(float x,float depth,float y)=>d.P(x,depth,y);
        private static GameObject Find(string name)=>UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).FirstOrDefault(t=>t.name==name)?.gameObject;
        private static Material Mat(string name)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+name+".mat");
        private static GameObject Box(string name,Vector3 at,Vector3 size,Material mat,bool collision=true)
        {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(root,false);
            g.transform.SetPositionAndRotation(at,Quaternion.LookRotation(d.north));g.transform.localScale=size;
            g.GetComponent<Renderer>().sharedMaterial=mat;
            if(!collision)Object.DestroyImmediate(g.GetComponent<Collider>());
            return g;
        }
        private static void Remove(params string[] names)
        {
            foreach(string name in names)
                foreach(var t in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).Where(t=>t.name==name).ToArray())
                    if(t)Object.DestroyImmediate(t.gameObject);
        }
        [MenuItem("LayarTerakhir/Prolog/Repair enclosed Atmaja stairs and upper floor")]
        public static void Apply()
        {
            var kos=File.ReadAllBytes(PrologKosSceneBuilder.ScenePath);var corridor=File.ReadAllBytes(KosCorridorBuilder.ScenePath);
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();
            // Remove complete old assemblies, including colliders and unused door prompts.
            var storage=Find("StorageDoor");if(storage)Object.DestroyImmediate(storage.transform.parent.gameObject.name.Contains("StorageDoor")?storage.transform.parent.gameObject:storage);
            Remove("AtmajaStairRevision","StairStep","StairParapet","StairEntryPlatform","StairEntryWestWall","StairEntryEastWall","StairDoorJamb","StairEntryDoorLintel","StairMidLanding","StairTopLanding",
                "StairHallWall_W","StairHallWall_E","StairHallLintel","StairHallWall","LockedCentralStairDoor","StorageFrontA","StorageFrontB","StorageLintel","StorageWall","CorridorCobweb");
            // Original generic rails belong to this stair assembly; other balcony rails use different names.
            foreach(var t in scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).Where(t=>(t.name=="RailBaluster"||t.name=="RailTop")&&d.LocalPosition(t.position).y>6.3f).ToArray())Object.DestroyImmediate(t.gameObject);
            root=new GameObject("AtmajaStairRevision").transform;root.SetParent(Find("Atmaja_Environment_Art").transform,false);
            RepairUpperMaterials();RepairFacadeBand();
            var plaster=Mat("House_MAT_PlasterDamaged");var concrete=Mat("House_MAT_Concrete");var wood=Mat("Upper_WoodFloor");var trim=Mat("House_MAT_WoodDoor");
            // Full opaque enclosure. The closed door is the only ground-floor sightline into the stairs.
            Box("StairEnclosureFrontWest",P(-5.01f,6.31f,2.10f),new Vector3(1.67f,3.16f,.18f),plaster);
            Box("StairEnclosureFrontEast",P(-.25f,6.31f,2.10f),new Vector3(5.35f,3.16f,.18f),plaster);
            Box("StairEnclosureDoorHeader",P(-3.55f,6.31f,3.225f),new Vector3(1.25f,.91f,.18f),plaster);
            Box("StairEnclosureWest",P(-4.36f,7.56f,2.10f),new Vector3(.18f,3.16f,2.68f),plaster);
            Box("StairEnclosureEast",P(1.79f,7.56f,3.605f),new Vector3(.18f,6.17f,2.68f),plaster);
            Box("StairEnclosureRear",P(-1.245f,8.92f,3.605f),new Vector3(6.25f,6.17f,.18f),plaster);
            Box("StairEntryFloor",P(-3.55f,7.57f,.49f),new Vector3(1.28f,.10f,2.5f),wood);
            Box("StairEntryFoundation",P(-3.55f,7.57f,.25f),new Vector3(1.28f,.38f,2.5f),concrete);
            // Solid closed risers with a continuous sloping soffit, separate tread nosings and rails.
            Flight("StairFlightLower",-2.90f,.40f,7.60f,8.65f,.54f,2.20f,true,wood,trim);
            Flight("StairFlightUpper",-2.60f,1.40f,6.40f,7.45f,2.20f,3.82f,false,wood,trim);
            Box("StairHalfLanding",P(1.0f,7.56f,2.08f),new Vector3(1.20f,.24f,2.34f),wood);
            Box("StairHalfLandingPier",P(1.0f,7.56f,1.01f),new Vector3(1.20f,1.90f,2.34f),plaster);
            Box("StairUpperLanding",P(-3.30f,6.925f,3.70f),new Vector3(1.40f,.24f,1.25f),wood);
            Box("StairUpperLandingWestSupport",P(-3.98f,7.56f,2.10f),new Vector3(.18f,3.16f,2.50f),plaster);
            Rail("HalfLandingRearRail",.4f,1.67f,8.73f,2.2f,2.2f,trim);
            Rail("UpperLandingRearRail",-4f,-2.6f,7.56f,3.82f,3.82f,trim);
            // Keep the complete steps and their support readable without the phone torch.
            var lantern=Find("StairMidLantern");if(lantern)KosVisualV2.SetCenter(lantern,P(1f,7.55f,4.8f));
            var cord=Find("StairLanternCord");if(cord){cord.transform.position=P(1f,7.55f,5.92f);cord.transform.localScale=new Vector3(.03f,1.82f,.03f);}
            var midLight=Find("StairMidLight")?.GetComponent<Light>();if(midLight){midLight.transform.position=P(1f,7.55f,4.59f);midLight.intensity=2.4f;midLight.range=6.5f;}
            var topLight=Find("StairTopLight")?.GetComponent<Light>();if(topLight){topLight.transform.position=P(-3.25f,6.925f,5.95f);topLight.intensity=1.8f;topLight.range=4.5f;}
            // Panelled, hinged door: remains a real leaf beside the wall after opening.
            var frame=new GameObject("StairDoorFrameOrigin");frame.transform.SetParent(root,false);frame.transform.SetPositionAndRotation(P(-4.175f,6.31f,.52f),Quaternion.LookRotation(d.north));
            var hinge=new GameObject("StairDoorHinge");hinge.transform.SetParent(frame.transform,false);
            var leaf=Box("LockedCentralStairDoor",P(-3.55f,6.31f,1.645f),new Vector3(1.25f,2.25f,.10f),trim);
            leaf.transform.SetParent(hinge.transform,true);var item=leaf.AddComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.StairDoor;item.label="Buka pintu kayu terkunci";
            d.stairDoorLeaf=leaf;d.stairDoorMotion=hinge.AddComponent<HingedDoor>();d.stairDoorMotion.hinge=hinge.transform;d.stairDoorMotion.angle=-90;
            foreach(float side in new[]{-.065f,.065f})
            {
                foreach(float y in new[]{1.13f,2.13f})
                {
                    var panel=Box("StairDoorRecessedPanel",P(-3.55f,6.31f+side,y),new Vector3(.96f,.67f,.035f),wood,false);panel.transform.SetParent(leaf.transform,true);
                }
                var handle=Box("StairDoorBrassHandle",P(-3.06f,6.31f+side*.9f,1.55f),new Vector3(.14f,.035f,.055f),Mat("Upper_ChandelierBrass"),false);handle.transform.SetParent(leaf.transform,true);
            }
            foreach(float x in new[]{-4.205f,-2.895f})Box("StairDoorFrame",P(x,6.22f,1.665f),new Vector3(.10f,2.33f,.12f),trim);
            Box("StairDoorFrameHeader",P(-3.55f,6.22f,2.82f),new Vector3(1.41f,.10f,.12f),trim);
            // Replace the purposeless storage room with an open cabinet niche in the lounge.
            var cabinet=Find("LoungeStorage");if(cabinet)KosVisualV2.SetCenter(cabinet,P(.8f,5.85f,1.15f));
            // Close upper landing edges and finish the floor/wall junction without narrowing the route.
            foreach(float x in new[]{-4.14f,1.68f})Box("UpperStairSkirting",P(x,7.58f,3.91f),new Vector3(.045f,.18f,2.46f),trim,false);
            foreach(var span in new[]{new Vector2(-5.8f,-4.05f),new Vector2(-2.5f,2.35f)})
                Box("UpperCorridorSkirting",P((span.x+span.y)*.5f,6.19f,3.91f),new Vector3(span.y-span.x,.18f,.045f),trim,false);
            Physics.SyncTransforms();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            if(!kos.SequenceEqual(File.ReadAllBytes(PrologKosSceneBuilder.ScenePath))||!corridor.SequenceEqual(File.ReadAllBytes(KosCorridorBuilder.ScenePath)))throw new Exception("Stair revision modified approved Kos scenes");
            Debug.Log("ATMAJA_STAIR_REVISION_APPLIED enclosed stairs, twelve closed risers per flight, supported landings, hinged door, removed storage door");
        }
        private struct BandVertex
        {
            public Vector3 position,normal;public Vector2 uv;public Vector4 tangent;
            public static BandVertex Mix(BandVertex a,BandVertex b,float t)=>new BandVertex{position=Vector3.Lerp(a.position,b.position,t),normal=Vector3.Lerp(a.normal,b.normal,t).normalized,uv=Vector2.Lerp(a.uv,b.uv,t),tangent=Vector4.Lerp(a.tangent,b.tangent,t)};
        }
        private static void RepairFacadeBand()
        {
            // The source BAND_mid is a full 12.6 x 9.6m cuboid at y=4.04..4.20,
            // merged into the house's concrete mesh without an interior collider.
            // Its caps hid Raka's legs and visually intersected the entire upper floor.
            var filter=Find("RumahAtmaja_House").GetComponentsInChildren<MeshFilter>(true).Single(f=>f.name=="Static__House_MAT_Concrete");
            var sourceFilter=PrefabUtility.GetCorrespondingObjectFromOriginalSource(filter);var source=sourceFilter?sourceFilter.sharedMesh:filter.sharedMesh;
            var positions=source.vertices;var normals=source.normals;var uvs=source.uv;var tangents=source.tangents;
            var output=new List<BandVertex>();var indices=new List<int>();int caps=0;
            BandVertex Vertex(int index)=>new BandVertex{position=positions[index],normal=normals.Length>index?normals[index]:Vector3.up,uv=uvs.Length>index?uvs[index]:Vector2.zero,tangent=tangents.Length>index?tangents[index]:new Vector4(1,0,0,1)};
            float Coordinate(BandVertex vertex,int axis){var local=d.LocalPosition(filter.transform.TransformPoint(vertex.position));return axis==0?local.x:local.y;}
            List<BandVertex> Clip(List<BandVertex> polygon,int axis,float boundary,bool lesser)
            {
                var result=new List<BandVertex>();if(polygon.Count==0)return result;
                var a=polygon[polygon.Count-1];float av=Coordinate(a,axis);bool insideA=lesser?av<=boundary:av>=boundary;
                foreach(var b in polygon)
                {
                    float bv=Coordinate(b,axis);bool insideB=lesser?bv<=boundary:bv>=boundary;
                    if(insideA!=insideB)result.Add(BandVertex.Mix(a,b,(boundary-av)/(bv-av)));
                    if(insideB)result.Add(b);a=b;av=bv;insideA=insideB;
                }
                return result;
            }
            void Append(List<BandVertex> polygon)
            {
                for(int j=1;j<polygon.Count-1;j++){int at=output.Count;output.Add(polygon[0]);output.Add(polygon[j]);output.Add(polygon[j+1]);indices.AddRange(new[]{at,at+1,at+2});}
            }
            var input=source.triangles;
            foreach(var triangle in Enumerable.Range(0,input.Length/3))
            {
                var polygon=new List<BandVertex>{Vertex(input[triangle*3]),Vertex(input[triangle*3+1]),Vertex(input[triangle*3+2])};
                bool Cap(float height)=>polygon.All(v=>Mathf.Abs(filter.transform.TransformPoint(v.position).y-d.origin.y-height)<.002f);
                if(!Cap(4.04f)&&!Cap(4.20f)){Append(polygon);continue;}
                caps++;
                foreach(var plane in new[]{(0,-5.85f,false),(0,5.85f,true),(1,.05f,false),(1,8.95f,true)})
                {
                    Append(Clip(polygon,plane.Item1,plane.Item2,!plane.Item3));polygon=Clip(polygon,plane.Item1,plane.Item2,plane.Item3);
                }
                // The final polygon lies inside the rooms/stairwell; discard only that cap area.
            }
            if(caps<4)throw new Exception("Could not identify the source facade band's four cap triangles");
            var mesh=new Mesh{name="AtmajaConcreteFacadeCutout",indexFormat=UnityEngine.Rendering.IndexFormat.UInt32};
            mesh.SetVertices(output.Select(v=>v.position).ToList());mesh.SetNormals(output.Select(v=>v.normal).ToList());mesh.SetUVs(0,output.Select(v=>v.uv).ToList());mesh.SetTangents(output.Select(v=>v.tangent).ToList());mesh.SetTriangles(indices,0);mesh.RecalculateBounds();
            const string path="Assets/_LayarTerakhir/assets/Art/Atmaja/Models/AtmajaConcreteFacadeCutout.asset";var prior=AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if(prior){EditorUtility.CopySerialized(mesh,prior);Object.DestroyImmediate(mesh);mesh=prior;}else AssetDatabase.CreateAsset(mesh,path);
            filter.sharedMesh=mesh;PrefabUtility.RecordPrefabInstancePropertyModifications(filter);
            Debug.Log("ATMAJA_FACADE_CAP_REPAIRED sourceCaps="+caps+" exterior retained; interior at y=4.04..4.20 opened");
        }
        internal static Texture2D UpperMask(Texture2D source)
        {
            // ARM stores AO/roughness/metallic in R/G/B. URP Lit expects
            // metallic in R, occlusion in G and smoothness in A. Treat these as
            // linear material data, not colour artwork or an unconverted JPG mask.
            string sourcePath=AssetDatabase.GetAssetPath(source);
            string path="Assets/_LayarTerakhir/assets/Art/Atmaja/Textures/"+source.name.Replace("_arm_1k","")+"_URPMask.asset";
            byte[] data=File.ReadAllBytes(sourcePath);
            using var sha=System.Security.Cryptography.SHA256.Create();string identity="ARM-to-URP-v3:"+Convert.ToBase64String(sha.ComputeHash(data));
            var existing=AssetDatabase.LoadAssetAtPath<Texture2D>(path);var importer=AssetImporter.GetAtPath(path);
            if(existing&&importer&&importer.userData==identity)return existing;
            var raw=new Texture2D(2,2,TextureFormat.RGBA32,false,true);ImageConversion.LoadImage(raw,data);
            var input=raw.GetPixels32();int sourceWidth=raw.width,sourceHeight=raw.height;Object.DestroyImmediate(raw);
            float reduction=Mathf.Max(1,Mathf.Max(sourceWidth,sourceHeight)/1024f);
            int width=Mathf.Min(1024,Mathf.ClosestPowerOfTwo(Mathf.RoundToInt(sourceWidth/reduction))),height=Mathf.Min(1024,Mathf.ClosestPowerOfTwo(Mathf.RoundToInt(sourceHeight/reduction)));
            var pixels=new Color32[width*height];
            // Some downloaded maps have larger source pixels despite a 1k filename.
            // Area-average linear channel data before applying the runtime texture cap.
            for(int y=0;y<height;y++)for(int x=0;x<width;x++)
            {
                int ao=0,rough=0,metal=0,count=0;
                for(int sy=y*sourceHeight/height;sy<(y+1)*sourceHeight/height;sy++)
                for(int sx=x*sourceWidth/width;sx<(x+1)*sourceWidth/width;sx++)
                {var c=input[sy*sourceWidth+sx];ao+=c.r;rough+=c.g;metal+=c.b;count++;}
                pixels[y*width+x]=new Color32((byte)(metal/count),(byte)(ao/count),0,(byte)(255-rough/count));
            }
            var packed=new Texture2D(width,height,TextureFormat.RGBA32,true,true){name=source.name+"_URP",wrapMode=TextureWrapMode.Repeat,anisoLevel=2};
            packed.SetPixels32(pixels);packed.Apply(true,false);EditorUtility.CompressTexture(packed,TextureFormat.DXT5,TextureCompressionQuality.Normal);packed.Apply(false,true);
            if(existing){EditorUtility.CopySerialized(packed,existing);Object.DestroyImmediate(packed);}else{AssetDatabase.CreateAsset(packed,path);existing=packed;}
            importer=AssetImporter.GetAtPath(path);importer.userData=identity;EditorUtility.SetDirty(existing);AssetDatabase.WriteImportSettingsIfDirty(path);
            return existing;
        }
        private static void RepairUpperMaterials()
        {
            var specifications=new[]{("Upper_WoodFloor","old_wood_floor"),("Upper_Wallpaper","decrepit_wallpaper"),("Upper_TileDirty","dirty_tiles"),("Upper_CeilingWood","ceiling_interior"),("Upper_BedFabric","quatrefoil_jacquard_fabric"),("Upper_PineTable","stained_pine"),("Upper_ConcretePaint","painted_concrete")};
            foreach(var specification in specifications)
            {
                var material=Mat(specification.Item1);
                var source=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Atmaja/Textures/"+specification.Item2+"_arm_1k.jpg");
                var mask=UpperMask(source);material.SetTexture("_MetallicGlossMap",mask);material.SetTexture("_OcclusionMap",mask);
                material.SetFloat("_Smoothness",1);material.EnableKeyword("_METALLICSPECGLOSSMAP");material.EnableKeyword("_OCCLUSIONMAP");EditorUtility.SetDirty(material);
            }
            // Tiled floor instances were copied from Upper_WoodFloor by the original builder.
            var wood=Mat("Upper_WoodFloor");
            foreach(string guid in AssetDatabase.FindAssets("t:Material",new[]{"Assets/_LayarTerakhir/assets/Art/Atmaja/Materials"}))
            {
                var material=AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));
                if(!material.name.StartsWith("UpperFloor_")||!material.name.EndsWith("_Tiled"))continue;
                material.SetTexture("_MetallicGlossMap",wood.GetTexture("_MetallicGlossMap"));material.SetTexture("_OcclusionMap",wood.GetTexture("_OcclusionMap"));material.SetFloat("_Smoothness",1);EditorUtility.SetDirty(material);
            }
        }
        private static void Rail(string name,float x0,float x1,float depth,float y0,float y1,Material mat)
        {
            Vector3 a=P(x0,depth,y0+.92f),b=P(x1,depth,y1+.92f);var rail=Box(name,(a+b)*.5f,new Vector3(Vector3.Distance(a,b),.075f,.085f),mat,false);
            rail.transform.rotation=Quaternion.LookRotation(d.north)*Quaternion.Euler(0,0,Mathf.Atan2(y1-y0,x1-x0)*Mathf.Rad2Deg);
            // Spindles attach to a stringer overlapping the tread edge; no hanging ends.
            var stringer=Box(name+"Stringer",(a+b)*.5f-Vector3.up*.97f,new Vector3(Vector3.Distance(a,b),.18f,.19f),mat,false);
            stringer.transform.rotation=rail.transform.rotation;
            int posts=Mathf.CeilToInt((x1-x0)/.27f);
            for(int i=0;i<=posts;i++)
            {
                float t=i/(float)posts;Box(name+"Spindle",P(Mathf.Lerp(x0,x1,t),depth,Mathf.Lerp(y0,y1,t)+.46f),new Vector3(i==0||i==posts?.08f:.035f,.92f,.06f),mat,false);
            }
            // A continuous invisible guard follows the handrail; the camera also collides with it.
            var guard=Box(name+"Guard",(a+b)*.5f-Vector3.up*.44f,new Vector3(Vector3.Distance(a,b),.88f,.08f),mat);
            guard.transform.rotation=rail.transform.rotation;guard.GetComponent<Renderer>().enabled=false;
        }
        private static void Flight(string name,float x0,float x1,float d0,float d1,float low,float high,bool east,Material mat,Material trim)
        {
            var vertices=new List<Vector3>();var triangles=new List<int>();var uv=new List<Vector2>();
            void Face(Vector3 a,Vector3 b,Vector3 c,Vector3 e)
            {
                int n=vertices.Count;vertices.AddRange(new[]{a,b,c,e});uv.AddRange(new[]{Vector2.zero,new Vector2(Vector3.Distance(a,b),0),new Vector2(Vector3.Distance(a,b),Vector3.Distance(b,c)),new Vector2(0,Vector3.Distance(b,c))});
                triangles.AddRange(new[]{n,n+1,n+2,n,n+2,n+3});
            }
            const int steps=12;float width=(x1-x0)/steps,rise=(high-low)/steps;
            for(int i=0;i<steps;i++)
            {
                float a=x0+i*width,b=a+width,top=east?low+(i+1)*rise:high-i*rise;
                float ya=(east?low+i*rise:high-i*rise)-.24f,yb=(east?low+(i+1)*rise:high-(i+1)*rise)-.24f;
                Vector3 A=new Vector3(a,ya,d0),B=new Vector3(b,yb,d0),C=new Vector3(b,top,d0),D=new Vector3(a,top,d0),off=Vector3.forward*(d1-d0);
                Face(D,C,B,A);Face(A+off,B+off,C+off,D+off);Face(A,B,B+off,A+off);Face(D,D+off,C+off,C);Face(A,A+off,D+off,D);Face(B,C,C+off,B+off);
                Box(name+"TreadNosing",P((a+b)*.5f,(d0+d1)*.5f,top+.008f),new Vector3(width+.018f,.016f,d1-d0),trim,false);
            }
            var mesh=new Mesh{name=name};mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.SetUVs(0,uv);mesh.RecalculateNormals();mesh.RecalculateBounds();
            string path="Assets/_LayarTerakhir/assets/Art/Atmaja/Models/"+name+".asset";var prior=AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if(prior){EditorUtility.CopySerialized(mesh,prior);Object.DestroyImmediate(mesh);mesh=prior;}else AssetDatabase.CreateAsset(mesh,path);
            var g=new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));g.transform.SetParent(root,false);g.transform.SetPositionAndRotation(d.origin,Quaternion.LookRotation(d.north));
            g.GetComponent<MeshFilter>().sharedMesh=mesh;g.GetComponent<MeshRenderer>().sharedMaterial=mat;g.GetComponent<MeshCollider>().sharedMesh=mesh;
            Rail(name+"RearRail",x0,east?x1:.35f,d1+.065f,east?low:high,east?high:high-(high-low)*(.35f-x0)/(x1-x0),trim);
            // The upper flight opens onto the half landing at its eastern end.
            Rail(name+"FrontRail",x0,east?x1:.35f,d0-.065f,east?low:high,east?high:high-(high-low)*(.35f-x0)/(x1-x0),trim);
        }
    }
}
#endif
