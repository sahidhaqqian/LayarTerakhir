#if UNITY_EDITOR
using System;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase2;
using LayarTerakhir.Prolog;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static partial class Phase2SceneBuilder
    {
        private static void Floor(float x0,float x1,float z0,float z1,float y,bool ceiling=true)
        {
            for(float x=x0;x<x1-.01f;x+=2)for(float z=z0;z<z1-.01f;z+=2)
            {
                float w=Mathf.Min(2,x1-x),l=Mathf.Min(2,z1-z);
                M(((Mathf.RoundToInt(x+z)/2)&3)==0?"Floor_tile_2":"Floor_tile_1",new Vector3(x+w/2,y-.1f,z+l/2),new Vector3(w,.1f,l));
                if(ceiling)M("Ceiling_Tile",new Vector3(x+w/2,y+3,z+l/2),new Vector3(w,.1f,l));
            }
        }
        private static void WallX(float z,float x0,float x1,float y,params float[] gaps)
        {
            for(float x=x0;x<x1-.01f;x+=2)
            {float span=Mathf.Min(2,x1-x),mid=x+span/2;bool door=gaps.Any(g=>Mathf.Abs(g-mid)<.2f);M(door?"Tile_Wall_Doorway_1":"Tile_Wall",new Vector3(mid,y,z),new Vector3(span,3,.18f));}
        }
        private static void WallZ(float x,float z0,float z1,float y,params float[] gaps)
        {
            for(float z=z0;z<z1-.01f;z+=2)
            {float span=Mathf.Min(2,z1-z),mid=z+span/2;bool door=gaps.Any(g=>Mathf.Abs(g-mid)<.2f);M(door?"Tile_Wall_Doorway_1":"Tile_Wall",new Vector3(x,y,mid),new Vector3(span,3,.18f),90);}
        }
        private static GameObject Barrier(string name,Vector3 p,Vector3 size)
        {var go=Point(name,p).gameObject;var c=go.AddComponent<BoxCollider>();c.center=Vector3.up*size.y/2;c.size=size;return go;}
        private static void Stairs(float x,float z,float y,bool ascend)
        {
            // Existing four-tread module repeated at human step height (~0.20m).
            // Scaling a whole 2m rise into one source module exceeds capsule stepOffset.
            for(int flight=0;flight<5;flight++)
            {
                float baseY=ascend?y+flight*.8f:y-(flight+1)*.8f;
                var stair=Prop("Hospital_StairFlight","Kenney/Stairs",new Vector3(x,baseY,z+1+flight*2),new Vector3(2.5f,.8f,2));
                // Source orientation is inspected from its actual vertices, not guessed.
                float highZ=0,lowZ=0;int highN=0,lowN=0;
                foreach(var f in stair.GetComponentsInChildren<MeshFilter>())foreach(var v in f.sharedMesh.vertices)
                {
                    var p=f.transform.TransformPoint(v);
                    if(p.y>baseY+.72f){highZ+=p.z;highN++;}
                    if(p.y<baseY+.15f){lowZ+=p.z;lowN++;}
                }
                bool risingNorth=highN>0&&lowN>0&&highZ/highN>lowZ/lowN;
                if(risingNorth!=ascend)stair.transform.Rotate(0,180,0);
            }
            // The shaft has a clear 2.5m flight, full side enclosure and top/bottom landings.
            for(float along=z;along<z+10;along+=2)
            {
                float h=y+(ascend?1:-1)*(along-z+1)*.4f;
                M("Tile_Wall",new Vector3(x-1.7f,h-1.2f,along+1),new Vector3(2,5.2f,.18f),90);
                M("Tile_Wall",new Vector3(x+1.7f,h-1.2f,along+1),new Vector3(2,5.2f,.18f),90);
            }
        }
        private static void Architecture()
        {
            Floor(0,12,-14,-8,0,false);
            for(int i=0;i<6;i++)PH("large_iron_gate",new Vector3(1+i*2,0,-14),new Vector3(2,2.2f,.14f));
            foreach(float side in new[]{0f,12f})for(int i=0;i<3;i++)PH("large_iron_gate",new Vector3(side,0,-13+i*2),new Vector3(2,2.2f,.14f),90);
            Floor(0,12,-8,0,0);Floor(0,36,0,4,0);
            WallZ(0,-8,4,0);WallZ(12,-8,0,0);WallX(-8,0,12,0,5);WallX(0,0,12,0,5);
            // Main corridor, four separately dressed clinics visible from their glazed front.
            for(int i=0;i<4;i++)
            {
                float x=14+i*6;
                Floor(x,x+4,-6,0,0);WallZ(x,-6,0,0);WallZ(x+4,-6,0,0);WallX(-6,x,x+4,0);
                M("Tile_Wall_Window",new Vector3(x+1,0,0),new Vector3(2,3,.18f));
                M("Tile_Wall_Doorway_1",new Vector3(x+3,0,0),new Vector3(2,3,.18f));
                M("Door_1",new Vector3(x+3,0,.015f),new Vector3(1.05f,2.4f,.07f));
                Text("Poli_"+(i+1),"POLI 0"+(i+1)+"\n"+new[]{"UMUM","ANAK","KANDUNGAN & KEBIDANAN","GIGI"}[i],new Vector3(x+1,2.55f,.13f),0,.105f);
                Thermal("POLI0"+(i+1),new Vector3(x+1,1.45f,.2f),new[]{22,22,-16,21}[i],3);
                if(i==2)
                {
                    var frozen=M("Tile_Wall_Window",new Vector3(x+1,0,.035f),new Vector3(2,3,.18f),0,false);
                    foreach(var r in frozen.GetComponentsInChildren<Renderer>())r.sharedMaterial=materials["ColdGlass"];
                    Text("LMedicalCard","REKAM MEDIS • L.",new Vector3(x+1,1.05f,.16f),0,.055f);
                }
            }
            WallX(4,0,36,0,1,33);WallZ(36,0,4,0);
            // Physical multi-floor circulation; no scene portals/teleports on these stairs.
            Stairs(1,4,0,true);Floor(0,4,14,16,4);WallX(16,0,4,4,1);
            foreach(float side in new[]{0f,4f})PH("large_iron_gate",new Vector3(side,4,15),new Vector3(2,1.15f,.14f),90);
            PH("large_iron_gate",new Vector3(3.1f,4,14),new Vector3(1.8f,1.15f,.14f));
            Stairs(33,4,0,false);Floor(32,36,14,20,-4);WallZ(36,14,20,-4);
            PH("large_iron_gate",new Vector3(35.1f,-4,14),new Vector3(1.8f,1.15f,.14f));
            // Basement: landing -> morgue loop, archive, duty room and always reachable exit.
            Floor(6,32,14,28,-4);WallZ(6,14,28,-4);WallX(14,6,32,-4);WallX(28,6,32,-4,13);
            WallZ(32,14,28,-4,17,23);
            Floor(8,20,28,34,-4);WallX(34,8,20,-4);WallZ(8,28,34,-4);WallZ(20,28,34,-4);
            Floor(32,36,20,26,-4);WallZ(36,20,26,-4);WallX(26,32,36,-4);WallX(20,32,36,-4,33);
            // L2 corridor, safe rooms and sealed operation-room boundary.
            Floor(0,36,16,20,4);WallZ(0,16,20,4);WallX(16,0,36,4,1,17);WallX(20,0,36,4,13);
            Floor(8,20,20,30,4);WallZ(8,20,30,4);WallZ(20,20,30,4);WallX(30,8,20,4);
            Floor(12,24,8,16,4);WallX(8,12,24,4);WallZ(12,8,16,4);WallZ(24,8,16,4);
            WallZ(36,16,20,4,17);
            Anchor("IGD",new Vector3(5,.03f,-10),0);
            Anchor("BASEMENT",new Vector3(34,-3.97f,17),270);
            Anchor("NURSE",new Vector3(18,4.03f,12),0);
            Room("INKUBATOR",8.3f,19.7f,20.5f,29.7f,4);Room("NURSE",12.3f,23.7f,8.3f,15.5f,4);
            Room("ARCHIVE",8.3f,19.7f,28.3f,33.7f,-4);Room("DUTY",32.3f,35.7f,20.6f,25.7f,-4);
            Room("MORGUE",6.3f,31.5f,14.3f,27.7f,-4);Room("L2",.2f,35.8f,16,20,4);
            Room("POLI",12,36,0,4,0);Room("IGD",.2f,11.8f,-13.8f,3.8f,0);
            d.world.lastriHome=Point("Lastri_MorgueHome",new Vector3(24,-4,18));
            d.world.lastriHomeL2=Point("Lastri_L2Home",new Vector3(30,4,18));
        }
        private static void InvestigationObjects()
        {
            var reception=M("Cabinet_1",new Vector3(8.5f,0,-3),new Vector3(3.5f,1.05f,.8f));
            var log=PH("binder_notebook",new Vector3(8,1.06f,-3.05f),new Vector3(.3f,.045f,.4f),0,false);Item(log,"LOG","Buku Jaga Perawat Malam");
            Text("IGD","IGD\nRSUD LAMA SIDO WARAS",new Vector3(8,2.5f,-5.8f),0,.2f);
            var board=Point("PoliSchedule",new Vector3(10,1.6f,.18f)).gameObject;Text("JadwalPoli","JADWAL POLIKLINIK\n01 UMUM  /  02 ANAK\n03 KANDUNGAN  /  04 GIGI",board.transform.position,0,.09f);Item(board,"POLI_BOARD","Baca jadwal poliklinik");
            Dynamo("DYNAMO_IGD",new Vector3(10,0,-5));
            var door=M("Door_swing",new Vector3(33,0,4),new Vector3(1.2f,2.4f,.09f));d.world.basementDoor=door;
            Item(door,"BASEMENT_PANEL","Panel pintu basement",new Vector3(34,1.25f,3.82f));
            Text("BasementSign","INSTALASI KAMAR MAYAT\nBASEMENT",new Vector3(33,2.65f,3.85f),180,.11f);
            var exit=Point("Basement_Exit_Lever",new Vector3(34,-2.75f,15.9f)).gameObject;Item(exit,"BASEMENT_EXIT","Tuas keluar — kembali ke lantai satu");
            var grille=PH("large_iron_gate",new Vector3(1,0,4),new Vector3(1.2f,2.6f,.13f));d.world.stairGrille=grille;Item(grille,"TERALI","Teralis Bangsal Bersalin");
            Text("L2Sign","BANGSAL BERSALIN\nLANTAI 2",new Vector3(1,2.7f,3.8f),180,.11f);
            // Two real autopsy tables with usable space below and a clear walking loop.
            for(int i=0;i<2;i++)
            {
                var table=Medical("Otopsi_"+i,"AutopsyTable",new Vector3(16,-4,19+i*5),new Vector3(2.5f,.92f,.95f),0);
                var hide=Item(table,i==0?"HIDE_TABLE_A":"HIDE_TABLE_B","Berlindung di kolong meja otopsi",new Vector3(15,-3.4f,18.4f+i*5));
                Point("HideAnchor",new Vector3(15.2f,-3.98f,19+i*5),hide.transform,180);
            }
            var report=PH("binder_notebook",new Vector3(16.6f,-3.07f,19),new Vector3(.3f,.04f,.4f),0,false);Item(report,"AUTOPSY","Laporan Otopsi + Kunci Teralis L2");
            var note=M("Magazine",new Vector3(15.5f,-3.07f,19),new Vector3(.2f,.012f,.28f),0,false);Item(note,"ARCHIVE_NOTE","Catatan perawat");
            Freezers();Archive();DutyRoom();UpperRooms();
        }
        private static void Dynamo(string id,Vector3 p)
        {
            var model=Prop(id,"Additional/Models/Generator",p,new Vector3(.85f,.65f,.55f));Item(model,id,"Tahan untuk memutar dinamo manual",p+new Vector3(0,.8f,-.28f));
            Text(id+"_Label","DINAMO MANUAL\nTAHAN E",p+new Vector3(0,1,-.29f),180,.06f);
        }
        private static void Freezers()
        {
            int[,] temps={{-6,-9,-11,-8},{-13,-18,-15,-7},{-12,-17,-24,-10}};
            for(int column=0;column<4;column++)M("Floor_tile_1",new Vector3(29,-4,17+column*2.5f),new Vector3(2.3f,.2f,1.7f));
            for(int row=0;row<3;row++)for(int col=0;col<4;col++)
            {
                string id=((char)('A'+row)).ToString()+(col+1);
                Vector3 p=new Vector3(29,-3.8f+(2-row)*.78f,17+col*2.5f);
                // Imported healthcare cabinet hardware is fitted into cold-storage bays;
                // no generated mesh is substituted for an asset.
                var unit=PH("metal_tool_chest",p,new Vector3(1.65f,.7f,2.3f),90);
                unit.name="Freezer_"+id;
                foreach(var surface in unit.GetComponentsInChildren<Renderer>())surface.sharedMaterial=materials["metal_plate_02"];
                // The drawer owns its moving colliders, so E can still cancel after the tray slides over its old marker.
                var interaction=Item(unit,id,"Laci pendingin "+id);
                var drawer=interaction.gameObject.AddComponent<Phase2Drawer>();drawer.director=d;drawer.id=id;drawer.temperature=temps[row,col];drawer.tray=unit.transform.GetChild(0);
                drawer.slideOffset=Vector3.back*.65f;drawers.Add(drawer);
                Vector3 face=p+new Vector3(-1.21f,.34f,0);Thermal(id,face,temps[row,col],1.5f);
                Text("DrawerLabel_"+id,id,face+Vector3.up*.23f,270,.085f);
                if(id=="C3")
                {
                    d.world.c3Target=Point("C3_Aim",face);
                    d.world.c3Contents=Point("C3_Contents",p).gameObject;
                    d.world.c3Chain=PH("medical_tape",p+new Vector3(-1.22f,.36f,0),new Vector3(.2f,.2f,.06f),90,false);Layer(d.world.c3Chain,29);
                    d.world.gunting=Prop("Gunting_TaliPusar","Scissors/Models/Scissors",p+new Vector3(-1.65f,.715f,-.22f),new Vector3(.24f,.03f,.12f),0,false);
                    Item(d.world.gunting,"GUNTING","Ambil Gunting Tali Pusar");
                    var jar=PH("chemistry_set",p+new Vector3(-1.55f,.715f,.25f),new Vector3(.24f,.28f,.2f),0,false);
                    jar.transform.SetParent(d.world.c3Contents.transform,true);d.world.gunting.transform.SetParent(d.world.c3Contents.transform,true);
                    d.world.auraTarget=Point("AuraToples",jar.transform.position+Vector3.up*.15f);
                    d.world.aura=Medical("Aura_Toples_C3","BloodVial",jar.transform.position,new Vector3(.2f,.27f,.2f),0,false);
                    d.world.aura.transform.SetParent(d.world.c3Contents.transform,true);
                    foreach(var r in d.world.aura.GetComponentsInChildren<Renderer>())r.sharedMaterial=materials["RitualAura"];Layer(d.world.aura,29);
                }
            }
        }
        private static void Archive()
        {
            var key=M("Cabinet_2",new Vector3(9,-4,30),new Vector3(1,.75f,.5f));Item(key,"ARCHIVE_KEY","Ambil kunci laci arsip",new Vector3(9,-2.8f,29.7f));
            Text("KeyBoard","PAPAN KUNCI\nARSIP",new Vector3(9,-2.2f,29.7f),180,.09f);
            var list=PH("binder_notebook",new Vector3(11,-3.95f,33.4f),new Vector3(.4f,.04f,.55f),0,false);Item(list,"VICTIM_LIST","Daftar jenazah bayi — 2001");
            for(int i=0;i<4;i++)
            {
                var cabinet=M("Cabinet_2",new Vector3(12+i*1.5f,-4,33.2f),new Vector3(1.2f,1.3f,.6f),180);
                Thermal("D"+(i+1),new Vector3(12+i*1.5f,-3.1f,32.8f),i==0?17:12,1.5f);
                Text("DLabel_"+i,"D"+(i+1),new Vector3(12+i*1.5f,-2.9f,32.8f),180,.09f);
                if(i==0)
                {
                    d.world.secret=PH("binder_notebook",new Vector3(12,-2.66f,33),new Vector3(.18f,.01f,.1f),0,false);
                    Item(cabinet,"D1","Buka laci arsip D1",new Vector3(12,-3.1f,32.82f));
                    Text("ARSeventeen","17",new Vector3(12,-2.5f,32.78f),180,.16f,29);
                }
            }
            var service=M("Door_1",new Vector3(18.8f,-4,33.7f),new Vector3(1,2.4f,.1f));Item(service,"SERVICE_TRACE","Periksa gagang pintu terowongan servis");
            Text("DamarTrace","↑",new Vector3(18.8f,-1.7f,33.62f),180,.2f);
        }
        private static void DutyRoom()
        {
            var door=M("Door_swing",new Vector3(33,-4,20),new Vector3(1.12f,2.4f,.07f));
            var leaf=door.transform.GetChild(0);var hinge=Point("DutyDoorHinge",door.transform.position+Vector3.left*.56f,door.transform);
            leaf.SetParent(hinge,true);
            var hinged=door.AddComponent<HingedDoor>();hinged.hinge=hinge;hinged.label="Buka pintu Pos Petugas";d.world.dutyDoor=hinged;
            var locker=M("Cabinet_1",new Vector3(35,-4,24.5f),new Vector3(.9f,2.2f,.9f),180);
            var hiding=Item(locker,"HIDE_LOCKER","Sembunyi di lemari besi",new Vector3(35,-2.9f,23.95f));
            Point("HideAnchor",new Vector3(35,-3.98f,24.3f),hiding.transform,180);
            Dynamo("DYNAMO_DUTY",new Vector3(33,-4,24.3f));Text("DutySign","POS PETUGAS",new Vector3(34,-1.3f,20.13f),0,.12f);
        }
        private static Vector3 MedicalSurface(GameObject station,Vector3 p)
        {
            Physics.SyncTransforms();
            var hits=Physics.RaycastAll(p+Vector3.up*2,Vector3.down,4,~0,QueryTriggerInteraction.Ignore)
                .Where(h=>h.transform.IsChildOf(station.transform)).OrderBy(h=>h.distance).ToArray();
            if(hits.Length==0)throw new InvalidOperationException("Medical prop has no supporting imported surface");
            p.y=hits[0].point.y+.016f;return p;
        }
        private static void UpperRooms()
        {
            Vector3 p=new Vector3(13,4,20);
            d.world.inkubatorWall=Barrier("Inkubator_PhysicalGate",p,new Vector3(1.3f,2.45f,.18f));
            d.world.inkubatorVeil=M("Tile_Wall",p,new Vector3(1.3f,2.45f,.2f),0,false);Layer(d.world.inkubatorVeil,30);
            foreach(var r in d.world.inkubatorVeil.GetComponentsInChildren<Renderer>())r.sharedMaterial=materials["red_brick_03"];
            d.world.wallTarget=Point("AR_Inkubator_Target",p+Vector3.up*1.3f);
            Text("InkubatorSign","RUANG INKUBATOR",p+new Vector3(0,2.7f,-.14f),180,.11f);
            d.world.incubatorSources=new Transform[6];
            for(int i=0;i<6;i++)
            {
                Vector3 at=new Vector3(10+i%3*3.8f,4.6f,23+i/3*4.3f);
                M("Table",at-Vector3.up*.6f,new Vector3(1.4f,.6f,2));
                d.world.incubatorSources[i]=Point("IncubatorCrySource_"+(i+1),at+new Vector3(0,.8f,-1.02f));
                var unit=Prop("Inkubator_0"+(i+1),"Neonatal/Models/Neonatal",at,new Vector3(1.2f,1.05f,1.9f),0);
                Text("IncubatorNumber_"+i,"0"+(i+1),at+new Vector3(0,.4f,-1.02f),180,.095f);
                if(i==3)
                {
                    d.world.evpTarget=Point("EVP_Inkubator04",at+new Vector3(0,.8f,-1.02f));
                    d.world.gelang=PH("medical_tape",MedicalSurface(unit,at+new Vector3(.18f,.45f,-.18f)),new Vector3(.1f,.025f,.08f),0,false);Item(d.world.gelang,"GELANG","Ambil Gelang Identitas Bayi Lastri");
                    var recorder=Medical("DamarRecorder","SampleHolder",MedicalSurface(unit,at+new Vector3(-.3f,.45f,-.7f)),new Vector3(.11f,.04f,.18f),0,false);Item(recorder,"DAMAR","Perekam Suara Damar");
                }
            }
            var desk=M("Table",new Vector3(18,4,10),new Vector3(2.2f,.92f,1.1f));
            var note=M("Magazine",new Vector3(17.4f,4.93f,10),new Vector3(.23f,.01f,.31f),0,false);Item(note,"NURSE_NOTE","Catatan Suster Jaga");
            var board=Point("BirthBoard",new Vector3(20,5.4f,8.15f)).gameObject;
            Text("BirthPlacement","PENEMPATAN BAYI\n01 R.  02 S.  03 N.\n04 L.  05 A.  06 D.",board.transform.position,0,.095f);Item(board,"BIRTH_BOARD","Baca papan penempatan bayi");
            var safe=Prop("BrankasObat","Additional/Models/Safe",new Vector3(22,4,10.5f),new Vector3(1.3f,1.3f,.65f),180);d.world.safePanel=Item(safe,"SAFE","Brankas obat — panel empat digit",new Vector3(22,5.1f,10.1f)).gameObject;
            var doorMesh=safe.GetComponentsInChildren<MeshFilter>().First(f=>f.name.StartsWith("SafeDoor"));
            var doorBounds=doorMesh.GetComponent<Renderer>().bounds;
            var safeHinge=new GameObject("MedicineSafeHinge").transform;safeHinge.SetParent(safe.transform,false);safeHinge.position=new Vector3(doorBounds.min.x,doorBounds.min.y,doorBounds.center.z);
            doorMesh.transform.SetParent(safeHinge,true);var mechanism=d.gameObject.AddComponent<HingedDoor>();mechanism.hinge=safeHinge;mechanism.locked=true;d.world.safeDoor=mechanism;
            d.world.safeEchoTarget=Point("Suster_Echo_Target",new Vector3(22,5.15f,10.07f));
            Text("Safe_Echo_AR","2 — 6 — 0 — 4",new Vector3(22,5.6f,10.06f),180,.08f,29);
            d.world.blood=Medical("BotolDarahSegel","BloodVial",new Vector3(22,4.48f,10.5f),new Vector3(.1f,.21f,.1f),0,false);Item(d.world.blood,"BLOOD","Ambil Botol Darah Segel Mbah Suro");
            Dynamo("DYNAMO_NURSE",new Vector3(13.5f,4,10));
            var operation=M("Door_swing",new Vector3(36,4,17),new Vector3(1.2f,2.4f,.12f),90);Item(operation,"OR_DOOR","Ruang Operasi Bersalin Utama",new Vector3(35.8f,5.2f,17));
            d.world.operationFog=Point("KabutPusaka",new Vector3(35.7f,5.2f,17)).gameObject;
            // Existing CC0 smoke image, configured as bounded world mist; no new art.
            var mist=d.world.operationFog.AddComponent<ParticleSystem>();
            var main=mist.main;main.startLifetime=4;main.startSpeed=.035f;main.startSize=new ParticleSystem.MinMaxCurve(.7f,1.2f);
            main.startColor=new Color(.25f,.32f,.34f,.28f);main.maxParticles=32;main.prewarm=true;
            main.simulationSpace=ParticleSystemSimulationSpace.World;
            var shape=mist.shape;shape.shapeType=ParticleSystemShapeType.Box;shape.scale=new Vector3(.18f,1.9f,1);
            var emission=mist.emission;emission.rateOverTime=6;
            var color=mist.colorOverLifetime;color.enabled=true;var gradient=new Gradient();
            gradient.SetKeys(new[]{new GradientColorKey(Color.white,0),new GradientColorKey(Color.white,1)},
                new[]{new GradientAlphaKey(0,0),new GradientAlphaKey(1,.2f),new GradientAlphaKey(.65f,.7f),new GradientAlphaKey(0,1)});
            color.color=gradient;
            string mistPath=Art+"Materials/OperationMist.mat";
            var mat=UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(mistPath);
            if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));UnityEditor.AssetDatabase.CreateAsset(mat,mistPath);}
            mat.SetTexture("_BaseMap",UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Imported+"KenneyParticles/Smoke.png"));
            mat.SetFloat("_Surface",1);mat.SetFloat("_Blend",0);mat.SetFloat("_ZWrite",0);
            mat.SetFloat("_SrcBlend",(float)UnityEngine.Rendering.BlendMode.SrcAlpha);mat.SetFloat("_DstBlend",(float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");mat.renderQueue=3000;UnityEditor.EditorUtility.SetDirty(mat);
            mist.GetComponent<ParticleSystemRenderer>().sharedMaterial=mat;
            Text("ORSign","RUANG OPERASI\nBERSALIN UTAMA",new Vector3(35.82f,6.65f,17),270,.13f);
        }
        private static void Layer(GameObject root,int layer)
        {foreach(var t in root.GetComponentsInChildren<Transform>(true))t.gameObject.layer=layer;}
    }
}
#endif
