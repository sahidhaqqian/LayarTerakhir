#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace LayarTerakhir.Editor
{
    public static partial class Phase4SceneBuilder
    {
        private static readonly Vector3[] sectors = { new Vector3(0,0,29), new Vector3(29,0,0), new Vector3(-29,0,0), new Vector3(0,0,-29) };
        private static void Architecture()
        {
            // The authored floor module supplies all visible surfaces. Empty collider
            // proxies support them; the portal aperture is physically open after CP7.
            Floor("Forest_Ground", new Vector3(0,0,-78), new Vector3(100,0,68), "forest_ground_04");
            Floor("Village_West", new Vector3(-29,0,0), new Vector3(52,0,88), "burned_ground_01");
            Floor("Village_East", new Vector3(29,0,0), new Vector3(52,0,88), "burned_ground_01");
            Floor("Village_CenterSouth", new Vector3(0,0,-20.5f), new Vector3(6,0,47), "mud_cracked_dry_riverbed_002");
            Floor("Village_CenterNorth", new Vector3(0,0,27.5f), new Vector3(6,0,33), "mud_cracked_dry_riverbed_002");
            d.world.portalCover = Floor("Pendopo_Sealed_Floor", new Vector3(0,0,7), new Vector3(6,0,8), "aerial_rocks_04");
            // The collision proxy belongs to the cover and opens with it.
            GameObject.Find("Pendopo_Sealed_Floor_CollisionOnly").transform.SetParent(d.world.portalCover.transform, true);
            var closedPortal=Point("SealedPortal_Navigation",new Vector3(0,-1.8f,7)).gameObject.AddComponent<NavMeshObstacle>();
            closedPortal.shape=NavMeshObstacleShape.Box;closedPortal.size=new Vector3(6.4f,5,8.4f);closedPortal.carving=true;
            closedPortal.transform.SetParent(d.world.portalCover.transform,true);
            d.world.portalStairs = Point("PasarSetan_Stairway", Vector3.zero).gameObject;
            for(int i=0;i<3;i++)
            {
                var stair=Place("ForestStairs","Models/ForestStairs",new Vector3(0,-(i+1)*4f/3,3+(i+.5f)*8f/3),new Vector3(5.8f,4f/3,8f/3),0,true,"Tangga batu berlumut_"+i);
                stair.transform.SetParent(d.world.portalStairs.transform,true);
            }
            Floor("PasarSetan_Vestibule", new Vector3(0,-4,16), new Vector3(10,0,10), "aerial_rocks_04");
            for(int side=-1;side<=1;side+=2) { var wall=Module("JogloWall",new Vector3(side*4.8f,-4,15),new Vector3(.45f,5,12),"Dinding lorong gerbang",0,true); SurfaceOn(wall,"aerial_rocks_04",Vector2.one*4); }
            var gate=Module("JogloGate",new Vector3(0,-4,20),new Vector3(8,5,.8f),"Gerbang kelima",0,true); SurfaceOn(gate,"burned_ground_01",Vector2.one*2);
            Label("Boundary","PASAR SETAN",new Vector3(0,-.8f,19.4f),0,.24f);
            Interaction("Boundary",new Vector3(0,-2.8f,18.5f),"BOUNDARY","Dengarkan riuh dari balik gerbang");
            var pendopo=Module("JogloPendopo",Vector3.up*3.4f,new Vector3(14,2.6f,12),"Pendopo ruwatan",0,true);
            for(int x=-1;x<=1;x+=2)for(int z=-1;z<=1;z+=2)Module("JogloPost",new Vector3(x*5.5f,0,z*4.6f),new Vector3(.35f,3.7f,.35f),"Tiang saka Pendopo");
            Label("Pendopo","KARANG SETRA\nPENDOPO RUWATAN",new Vector3(0,3.2f,-5.8f),0,.2f);
            // Background dwellings and distinct stilt/porch assemblies preserve a
            // rural Javanese silhouette instead of reusing the earlier chapters.
            Vector3[] houses={new Vector3(-16,0,13),new Vector3(16,0,13),new Vector3(-18,0,-13),new Vector3(18,0,-13),new Vector3(-37,0,18),new Vector3(37,0,18),new Vector3(-18,0,36),new Vector3(18,0,36)};
            for(int i=0;i<houses.Length;i++)
            {
                if(i%3==0)Module("JogloHouse",houses[i],new Vector3(8,4.6f,7),"Rumah terbakar_"+i,i%2==0?180:0);
                else
                {
                    Module("JogloRoof",houses[i]+Vector3.up*2.7f,new Vector3(8,2,7),"Atap lumbung_"+i,0,true);
                    for(int x=-1;x<=1;x+=2)for(int z=-1;z<=1;z+=2)Module("JogloPost",houses[i]+new Vector3(x*3.2f,0,z*2.6f),new Vector3(.25f,3.2f,.25f),"Tiang lumbung_"+i,0,true);
                    Module("JogloWall",houses[i]+new Vector3(0,0,2.6f),new Vector3(6.5f,2.8f,.24f),"Anyaman belakang_"+i);
                    if(i!=5)
                    {
                        for(int side=-1;side<=1;side+=2)
                        {
                            var sideWall=Module("JogloWall",houses[i]+new Vector3(side*3.2f,0,0),new Vector3(.22f,2.65f,5.4f),"Dinding samping rumah_"+i);
                            SurfaceOn(sideWall,i%2==0?"bamboo_wall_03":"wood_planks_dirt",new Vector2(2,2));
                            var facade=Module("JogloWall",houses[i]+new Vector3(side*2.2f,0,-2.6f),new Vector3(2.1f,2.65f,.22f),"Daun fasad rumah_"+i);
                            SurfaceOn(facade,i%2==0?"wood_planks_dirt":"bamboo_wall_03",new Vector2(1,2));
                        }
                    }
                }
            }
            // Walkable gubuk panggung: matching visible planks and four-step support.
            Floor("Gubuk_Panggung",new Vector3(29,.65f,4),new Vector3(9,0,10),"wood_planks_dirt");
            Module("JogloRoof",new Vector3(29,3.2f,4),new Vector3(10,2,11),"Atap gubuk Mbah Suro");
            for(int x=-1;x<=1;x+=2)for(int z=-1;z<=1;z+=2)Module("JogloPost",new Vector3(29+x*3.7f,0,4+z*4.2f),new Vector3(.24f,3.8f,.24f),"Tiang gubuk");
            Module("JogloWall",new Vector3(29,.65f,8.8f),new Vector3(9,2.6f,.22f),"Dinding belakang gubuk");
            for(int i=0;i<4;i++){float top=(i+1)*.1625f; var tread=Module("JogloFloor",new Vector3(29,top-.16f,-2.4f+i*.4f),new Vector3(3,.16f,.43f),"Anak tangga gubuk",0,false);SurfaceOn(tread,"wood_planks_dirt",new Vector2(1.5f,.2f));Proxy("Gubuk_Step_Collision",new Vector3(29,top-.1f,-2.4f+i*.4f),new Vector3(3,.2f,.43f));}
            // The global agent can step 75cm, Raka 30cm. Mark only the ground
            // beneath the high ledge so navigation uses the four real steps.
            NavEdge(new Vector3(24.5f,.1f,4),new Vector3(.8f,.5f,10.8f));
            NavEdge(new Vector3(33.5f,.1f,4),new Vector3(.8f,.5f,10.8f));
            NavEdge(new Vector3(29,.1f,9),new Vector3(9.8f,.5f,.8f));
            NavEdge(new Vector3(26,.1f,-1),new Vector3(3,.5f,.8f));
            NavEdge(new Vector3(32,.1f,-1),new Vector3(3,.5f,.8f));
            var motor=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx"));
            motor.name="RakaMotor_CampaignCarryOver";motor.transform.SetParent(env,false);
            var bike=motor.GetComponentsInChildren<Renderer>();var bikeBounds=bike[0].bounds;foreach(var r in bike.Skip(1))bikeBounds.Encapsulate(r.bounds);
            motor.transform.localScale*=1.15f/Mathf.Max(.01f,bikeBounds.size.y);bikeBounds=bike[0].bounds;foreach(var r in bike.Skip(1))bikeBounds.Encapsulate(r.bounds);
            motor.transform.position+=new Vector3(3.6f-bikeBounds.center.x,-bikeBounds.min.y,-101.5f-bikeBounds.center.z);bikeBounds=bike[0].bounds;foreach(var r in bike.Skip(1))bikeBounds.Encapsulate(r.bounds);
            var hull=motor.AddComponent<BoxCollider>();hull.center=motor.transform.InverseTransformPoint(bikeBounds.center);var hullSize=motor.transform.InverseTransformVector(bikeBounds.size);hull.size=new Vector3(Mathf.Abs(hullSize.x),Mathf.Abs(hullSize.y),Mathf.Abs(hullSize.z));
            var motorNav=motor.AddComponent<NavMeshModifier>();motorNav.overrideArea=true;motorNav.area=1;
            Anchor("HUB",new Vector3(0,.06f,-100)); Anchor("ENTRY",new Vector3(0,.06f,-100)); Anchor("J1",new Vector3(0,.06f,-82));
            Anchor("PENDOPO",new Vector3(-3,.06f,-6)); Anchor("NORTH",new Vector3(-6,.06f,21)); Anchor("EAST",new Vector3(25,.06f,-4),90);
            Anchor("WEST",new Vector3(-23,.06f,-4),270); Anchor("SOUTH",new Vector3(-6,.06f,-26),180); Anchor("PORTAL",new Vector3(0,.06f,1)); Anchor("BOUNDARY",new Vector3(0,-3.94f,15));
            Zone("ENTRY",new Vector3(0,1,-100),new Vector3(14,6,10)); Zone("J1",new Vector3(0,1,-82),new Vector3(14,6,12)); Zone("J2",new Vector3(12,1,-64),new Vector3(14,6,12)); Zone("J3",new Vector3(-8,1,-46),new Vector3(16,6,12));
            Zone("NORTH",sectors[0]+Vector3.up,new Vector3(20,7,22)); Zone("EAST",sectors[1]+Vector3.up,new Vector3(22,7,24)); Zone("WEST",sectors[2]+Vector3.up,new Vector3(22,7,24)); Zone("SOUTH",sectors[3]+Vector3.up,new Vector3(20,7,22)); Zone("PENDOPO",new Vector3(0,1,0),new Vector3(19,8,22));
            // Authored bamboo clumps define switchbacks and occlude distant lights.
            for(int row=0;row<20;row++)for(int side=-1;side<=1;side+=2)
            {
                float z=-106+row*3.2f; float center=z<-76?0:z<-58?12:-8;
                var p=new Vector3(center+side*(5.8f+(row%3)*1.2f),0,z);
                var clump=Module("Bamboo"+new[]{"A","B","C","D"}[row%4],p,new Vector3(4+(row%2),9+row%4,4),"Hutan bambu simpang",row*37,false);SolidTrunks(clump);
            }
            // Layer the same newly licensed plants beyond the walkable verge;
            // a single row exposed the flat level edge and read like a test set.
            for(int row=0;row<20;row++)for(int side=-1;side<=1;side+=2)for(int lane=0;lane<2;lane++)
            {
                float z=-108+row*3.4f;float centre=z<-76?0:z<-58?12:-8;
                var p=new Vector3(centre+side*(12+lane*7+row%3),0,z);
                var clump=Module("Bamboo"+new[]{"C","A","D","B"}[(row+lane)%4],p,new Vector3(6,10+(row+lane)%4,6),"Lapisan hutan bambu",row*43+lane*17,false);SolidTrunks(clump);
                foreach(var r in clump.GetComponentsInChildren<Renderer>())r.shadowCastingMode=ShadowCastingMode.Off;
            }
            for(int i=0;i<44;i++)
            {
                float z=-105+i*1.4f;float centre=z<-76?0:z<-58?12:-8;
                Model(i%3==0?"shrub_sorrel_01":i%2==0?"grass_medium_01":"grass_medium_02",new Vector3(centre+(i%2==0?1:-1)*(3.3f+i%3*.3f),0,z),.35f+i%3*.09f,"Tumbuhan bawah jalur hutan",i*67,false);
            }
            for(int i=0;i<52;i++)
            { float a=i*Mathf.PI*2/52; var p=new Vector3(Mathf.Sin(a)*47,0,Mathf.Cos(a)*43); var clump=Module("Bamboo"+new[]{"A","B","C","D"}[i%4],p,new Vector3(5,10+i%4,5),"Batas hutan alami",i*23,false);SolidTrunks(clump); }
            for(int j=0;j<3;j++)
            {
                Vector3 p=new[]{new Vector3(0,.025f,-85),new Vector3(12,.025f,-67),new Vector3(-8,.025f,-49)}[j];
                for(int i=0;i<4;i++)Mark("Footprint",p+new Vector3((j==1?-1:1)*i*.48f,0,i*.72f),.45f,new Color(.35f,.95f,.72f),Quaternion.Euler(90,0,j==1?-35:35));
                Label("Branch_"+j,"JEJAK DAMAR\nPeriksa tanah dengan UV",p+new Vector3(2,1.5f,1),0,.1f);
                Vector3 wrong=p+new Vector3(-7,0,3);
                Model("rock_07",wrong,.7f,"Nisan jalur umpan_"+j,15);
                Mark("AuthenticSkull",wrong+Vector3.up*.02f,1.2f,new Color(.7f,.28f,1),Quaternion.Euler(90,0,0));
                var hazard=Point("FalseBranch_"+j,wrong).gameObject.AddComponent<Phase4ForestHazard>();hazard.director=d;hazard.danger=new Bounds(wrong+Vector3.up,new Vector3(3,3,4));
                hazard.warning=Mark("DecoyCross",wrong+Vector3.up*.04f,2.5f,new Color(1,.19f,.03f,.8f),Quaternion.Euler(90,0,0));hazard.warning.layer=0;hazard.warning.SetActive(false);
                Source("DAMAR_FALSE_"+j,wrong+Vector3.up,Phase3VoiceTag.Mimic,"[Suara Damar dari jalan buntu: Ka, cepat ke sini!]",8);
            }
            SolidTrunks(Model("Banyan",new Vector3(-6,0,36),15,"Beringin makam",0,false));
            Recovery("ENTRY",new Vector3(-3,0,-100)); Recovery("PENDOPO",new Vector3(-6,0,-5));
            Recovery("BOUNDARY",new Vector3(-3,-4,15));
        }
        private static void Recovery(string id, Vector3 at)
        {
            wards.Add(new Bounds(at+new Vector3(0,1,0),new Vector3(6,3,5)));
            var volume=Point("Ward_"+id,at+Vector3.up).gameObject.AddComponent<Unity.AI.Navigation.NavMeshModifierVolume>();volume.size=new Vector3(6,3,5);volume.area=3;
            Model("stone_fire_pit",at,.24f,"Batu ruwatan_"+id,0,true);
            Interaction("Shrine_"+id,at+Vector3.up*.65f,"SHRINE_"+id,"Tahan E: tenangkan diri • +15 Sanity");
            var crank=Model("spinning_wheel_01",at+Vector3.right*1.5f,.9f,"Dinamo manual_"+id);
            Item(crank,"CHARGE_"+id,"Tahan E: putar dinamo • +25 baterai");
            if(id!="ENTRY"&&id!="BOUNDARY")
            {var basin=Model("wooden_bucket_02",at+Vector3.left*1.5f,.5f,"Saluran ruwatan_"+id);Item(basin,"REFILL_"+id,"Tahan E: isi Kendi • air sumur");}
            Label("Recovery_"+id,"BATU RUWATAN / DINAMO\nTahan E • pemulihan gratis",at+new Vector3(0,1.8f,.7f),0,.1f);
            Lamp("Ward_Lantern_"+id,at+Vector3.up*1.8f,new Color(.52f,.7f,.62f),1.3f,6);
        }
        private static void NavEdge(Vector3 at,Vector3 size)
        {var edge=Point("Gubuk_Ledge_Navigation",at+Vector3.up*.5f).gameObject.AddComponent<NavMeshModifierVolume>();edge.size=new Vector3(size.x,2,size.z);edge.area=1;}
        private static void Investigation()
        {
            for(int i=0;i<11;i++)
            {if(i==6)continue;var p=new Vector3(-8+(i%4)*4,0,25+(i/4)*4); Model(i%2==0?"rock_07":"stone_01",p,.7f,"Nisan warga 1997",i*31);}
            var grave=Model("coast_land_rocks_02",new Vector3(0,0,29),.4f,"Makam bara santet",0,true);
            d.world.thermalOcclusionRoot=grave.transform;
            d.world.thermalTarget=Point("Thermal66_Target",new Vector3(0,.55f,29));
            d.world.kendi=Model("jug_01",new Vector3(.8f,.4f,29),.45f,"Kendi pusaka");Item(d.world.kendi,"KENDI","Ambil Kendi Air Sumur Keramat");
            Label("North","UTARA — MAKAM 1997\nThermoKlenik: periksa bara",new Vector3(-3,1.9f,25),0,.13f);
            Recovery("NORTH",new Vector3(-6,0,23));
            d.world.illusionWall=Module("JogloWall",new Vector3(29,.65f,1.5f),new Vector3(8,2.5f,.12f),"Dinding ilusi anyaman",0,true);SurfaceOn(d.world.illusionWall,"bamboo_wall_03",Vector2.one*2);
            foreach(var t in d.world.illusionWall.GetComponentsInChildren<Transform>())t.gameObject.layer=30;
            var obstacle=d.world.illusionWall.AddComponent<NavMeshObstacle>();obstacle.shape=NavMeshObstacleShape.Box;obstacle.center=Vector3.up*1.25f;obstacle.size=new Vector3(8,2.5f,.2f);obstacle.carving=true;
            var altar=Model("WoodenTable_03",new Vector3(29,.65f,5.5f),.9f,"Altar Panca-Sengkala");
            var skull=Part("HumanSkeleton",0,new Vector3(29,1.55f,5.5f),.32f,"Tengkorak altar");
            d.world.arTarget=Point("AR_Altar_Target",new Vector3(29,1.75f,5.4f));d.world.arOcclusionRoot=altar.transform;
            Lamp("Altar_Aura",new Vector3(29,1.9f,5.2f),new Color(.9f,.12f,.05f),1.3f,4);
            d.world.blade=Model("Keris",new Vector3(30,1.55f,5.4f),.48f,"Belati Perak Ruwatan",75);Item(d.world.blade,"BLADE","Ambil Belati Perak Ruwatan");
            var note=Model("decorative_book_set_01",new Vector3(28,1.55f,5.5f),.16f,"Catatan terakhir Damar");Item(note,"DAMAR_NOTE","Baca sobekan catatan terakhir Damar");
            Recovery("EAST",new Vector3(24,0,-5));
            var well=Place("StoneWell","Modules/StoneWellAssembly",new Vector3(-29,0,3),new Vector3(3.6f,3.8f,3.3f),0,true,"Sumur keramat");
            d.world.wellMantra=Point("Well_Mantra",new Vector3(-29,1.1f,1));d.world.secretKnock=Point("Well_SecretKnock",new Vector3(-31,1,3));
            Source("WELL_MANTRA",d.world.wellMantra.position,Phase3VoiceTag.Authentic,"[Mantra terbalik: Utara… Timur… Barat… Selatan…]",6);
            Source("WELL_KNOCK",d.world.secretKnock.position,Phase3VoiceTag.Authentic,"[Tiga ketukan dari relung. Mata tertutup.]",3);
            d.world.soil=Model("compost_bag_02",new Vector3(-27.6f,.3f,2),.35f,"Kantong Tanah Kubur");Item(d.world.soil,"SOIL","Ambil Kantong Tanah Kubur Keramat");
            Interaction("WellRefill",new Vector3(-28.6f,1,0),"REFILL_WELL","Tahan E: isi Kendi dari sumur");
            var wheel=Model("rusted_wheel_rim_01",new Vector3(-32,.65f,3),.6f,"Roda relung terkunci",90);Item(wheel,"WHEEL","Putar roda mata tertutup");
            d.world.secretEye=Mark("ClosedEye",new Vector3(-31.05f,1.25f,3.2f),.4f,new Color(.5f,.72f,1),Quaternion.identity).transform;
            d.world.hiddenDrawer=Point("Hidden_Drawer",new Vector3(-31,.4f,3.3f)).gameObject;
            var drawer=Model("wooden_bowl_02",new Vector3(-31,.4f,3.3f),.14f,"Laci relung cermin");drawer.transform.SetParent(d.world.hiddenDrawer.transform,true);
            // Store the closed transform relative to a stationary mount.
            var mount=Point("DrawerMount",d.world.hiddenDrawer.transform.position);d.world.hiddenDrawer.transform.SetParent(mount,false);d.world.hiddenDrawer.transform.localPosition=Vector3.zero;
            d.world.mirror=Model("standing_picture_frame_01",new Vector3(-31,.55f,3.3f),.2f,"Keping Cermin Saksi");d.world.mirror.transform.SetParent(d.world.hiddenDrawer.transform,true);Item(d.world.mirror,"MIRROR","Ambil Keping Cermin Saksi — mata tertutup");
            Recovery("WEST",new Vector3(-27,0,-.5f)); Recovery("SOUTH",new Vector3(-6,0,-24));
            Hide("NORTH",new Vector3(5,0,24)); Hide("EAST",new Vector3(34,0,-3)); Hide("WEST",new Vector3(-35,0,-3)); Hide("SOUTH",new Vector3(5,0,-24));
            d.world.glasses=Model("round_spectacles",new Vector3(-5.5f,.9f,2.8f),.05f,"Kacamata retak Damar");Item(d.world.glasses,"GLASSES","Periksa kacamata Damar");
            var book=Model("decorative_book_set_01",new Vector3(-3.5f,.9f,2.8f),.23f,"Kitab Rajah Bagian Keempat");Item(book,"BOOK","Baca Kitab Rajah Mbah Suro — IV");
            Model("WoodenTable_01",new Vector3(-4.5f,0,2.8f),.9f,"Meja kacamata dan kitab");
            Interaction("Broadcast",new Vector3(2,1,-8),"START_LIVE","Mulai siaran malam keempat");
        }
        private static void Source(string id, Vector3 at, Phase3VoiceTag tag, string text, float range)
        { var e=Point("EVP_"+id,at).gameObject.AddComponent<Phase3EvpSource>();e.sourceId=id;e.tag=tag;e.subtitle=text;e.range=range;e.priority=1;sources.Add(e); }
        private static void Hide(string id, Vector3 at)
        {
            var g=Point("Hide_"+id,at).gameObject;
            // Actual U-shaped cover blocks rays. Interaction owns only the entrance.
            Module("JogloWall",at+new Vector3(0,0,1),new Vector3(2.5f,1.5f,.2f),"Perlindungan belakang_"+id);
            Module("JogloWall",at+new Vector3(-1.2f,0,0),new Vector3(.2f,1.5f,2),"Perlindungan samping_"+id);
            Module("JogloWall",at+new Vector3(1.2f,0,0),new Vector3(.2f,1.5f,2),"Perlindungan samping_"+id);
            Point("HideInside",at+new Vector3(0,.05f,.3f),g.transform,180);
            var c=g.AddComponent<BoxCollider>();c.center=new Vector3(0,.8f,-.6f);c.size=new Vector3(2,.5f,.3f);
            Item(g,"HIDE_"+id,"Masuk perlindungan • matikan layar (F)");
            Label("Hide_"+id,"PERLINDUNGAN\nE masuk / F layar",at+new Vector3(0,1.8f,1),0,.1f);
        }
        private static void Ritual()
        {
            d.world.offerings=new GameObject[3];
            string[] models={"jug_01","Keris","compost_bag_02"};
            for(int i=0;i<3;i++)
            {
                var p=new Vector3(-2+i*2,0,.2f);Model("stone_01",p,.65f,"Dudukan pusaka_"+i);
                Interaction("AltarSlot_"+i,p+Vector3.up*.95f,"ALTAR_"+i,"Pasang / angkat "+Phase4Content.Label(LayarTerakhir.Core.Phase4Ids.Relics[i]));
                d.world.offerings[i]=Model(models[i],p+Vector3.up*.65f,.3f,"Persembahan_"+i,0,false);
            }
            Model("stone_01",new Vector3(4,0,-1),.8f,"Batu konfirmasi ruwatan");
            Interaction("RitualStart",new Vector3(4,1,-1),"RITUAL","Aktifkan Batu Ruwatan / panggil inti penutup");
            Label("RitualStone","BATU RUWATAN",new Vector3(4,.6f,-1.42f),0,.08f);
            Interaction("Portal",new Vector3(0,1,2.3f),"PORTAL","Periksa tangga menuju gerbang kelima");
            d.world.sectorCore=new Transform[4];d.world.observations=new Transform[4];
            for(int sector=0;sector<4;sector++)
            {
                Vector3 center=sectors[sector];d.world.sectorCore[sector]=Point("SectorCore_"+sector,center+new Vector3(0,0,-5));d.world.observations[sector]=Point("Observation_"+sector,center+new Vector3(-5,.05f,-5));
                for(int candidate=0;candidate<3;candidate++)
                {
                    var pos=center+new Vector3((candidate-1)*3.2f,0,-9);
                    var g=Model("wooden_lantern_01",pos,1.5f,"Obor_"+sector+"_"+candidate);
                    var item=Item(g,"TORCH_"+sector+"_"+candidate,"Siram obor memakai Kendi");
                    var t=g.AddComponent<Phase4Torch>();t.director=d;t.sector=sector;t.candidate=candidate;t.focus=Point("TorchFocus",pos+Vector3.up*1.25f,g.transform);
                    t.authenticMark=Mark("AuthenticSkull",pos+new Vector3(0,.85f,-.28f),.44f,new Color(.4f,.9f,1),Quaternion.identity,g.transform);
                    t.decoyMark=Mark("DecoyCross",pos+new Vector3(0,.85f,-.28f),.44f,new Color(1,.1f,.1f),Quaternion.identity,g.transform);
                    t.flame=Fire(pos+Vector3.up*1.3f,.36f,g.transform);
                    t.practical=Lamp("TorchLight_"+sector+"_"+candidate,pos+Vector3.up*1.5f,new Color(1,.23f,.05f),1.1f,5);t.practical.transform.SetParent(g.transform,true);torches.Add(t);
                }
                Label("Sector_"+sector,new[]{"UTARA — MAKAM","TIMUR — GUBUK","BARAT — SUMUR","SELATAN — BAMBU"}[sector],center+new Vector3(0,2,-8),0,.14f);
            }
            d.world.patrol=new[]{new Vector3(-12,0,-17),new Vector3(-18,0,3),new Vector3(-8,0,19),new Vector3(10,0,20),new Vector3(19,0,5),new Vector3(15,0,-17)}.Select((p,i)=>Point("Patrol_"+i,p)).ToArray();
            d.world.emberWarning=Mark("DecoyCross",new Vector3(0,.03f,-29),3.2f,new Color(1,.23f,.03f,.85f),Quaternion.Euler(90,0,0));
            d.world.emberWarning.layer=0;d.world.emberWarning.SetActive(false);
        }
        private static GameObject Fire(Vector3 at,float radius,Transform parent)
        {
            var g=Point("Licensed_Fire",at,parent).gameObject;var ps=g.AddComponent<ParticleSystem>();var main=ps.main;main.startLifetime=.75f;main.startSpeed=.6f;main.startSize=radius;main.maxParticles=32;main.startColor=new Color(1,.3f,.05f,.75f);main.simulationSpace=ParticleSystemSimulationSpace.Local;
            var emission=ps.emission;emission.rateOverTime=22;var shape=ps.shape;shape.shapeType=ParticleSystemShapeType.Sphere;shape.radius=radius*.4f;
            var sheet=ps.textureSheetAnimation;sheet.enabled=true;sheet.numTilesX=8;sheet.numTilesY=8;sheet.frameOverTime=new ParticleSystem.MinMaxCurve(1,new AnimationCurve(new Keyframe(0,0),new Keyframe(1,.6f)));
            var renderer=ps.GetComponent<ParticleSystemRenderer>();string file=Imported+"Particles/magic1.png";
            var m=NewMaterial("LicensedFire","Universal Render Pipeline/Particles/Unlit");if(file!=null)m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(file));m.SetColor("_BaseColor",Color.white);m.SetFloat("_Surface",1);m.SetFloat("_ZWrite",0);m.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);m.SetFloat("_DstBlend",(float)BlendMode.One);m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");m.renderQueue=3000;renderer.sharedMaterial=m;return g;
        }
        private static void Actors()
        {
            var actor=Point("Banaspati_Navigation",new Vector3(-12,0,-17)).gameObject;d.world.banaspati=actor;d.entity.agent=actor.AddComponent<NavMeshAgent>();d.entity.agent.radius=.4f;d.entity.agent.height=.8f;d.entity.agent.acceleration=10;d.entity.agent.angularSpeed=240;d.entity.agent.stoppingDistance=.45f;
            d.entity.visual=Point("Banaspati_Visual",actor.transform.position+Vector3.up*1.75f,actor.transform);
            d.world.fireVisual=Fire(d.entity.visual.position,1.5f,d.entity.visual);
            d.world.skeletonVisual=Part("HumanSkeleton",0,d.entity.visual.position-Vector3.up*.3f,.7f,"Inti Banaspati");d.world.skeletonVisual.transform.SetParent(d.entity.visual,true);
            d.world.weakPoint=Point("Banaspati_Weakpoint",d.entity.visual.position,d.entity.visual);
            d.world.entityLight=Lamp("Banaspati_WarningLight",d.entity.visual.position,new Color(1,.12f,.025f),5,12,true);d.world.entityLight.transform.SetParent(d.entity.visual,true);
            d.world.suro=Suro(new Vector3(0,5.5f,2));d.world.mergeSuro=Suro(new Vector3(0,0,1));d.world.mergeSuro.SetActive(false);
            d.world.fearEchoes=new[]{Fire(new Vector3(-14,2,15),1.1f,env),Fire(new Vector3(14,2,-15),1.1f,env)};foreach(var echo in d.world.fearEchoes)echo.SetActive(false);
            d.world.southSmoke=Fire(new Vector3(-4,.2f,-39),3,env);
            var smoke=d.world.southSmoke.GetComponent<ParticleSystem>();var sm=smoke.main;sm.startColor=new Color(.25f,.23f,.22f,.12f);sm.startSpeed=.12f;sm.startLifetime=4;sm.maxParticles=12;
            var material=NewMaterial("LicensedSmoke","Universal Render Pipeline/Particles/Unlit");material.CopyPropertiesFromMaterial(d.world.fireVisual.GetComponent<ParticleSystemRenderer>().sharedMaterial);material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Imported+"Particles/smokeTestLow.png"));material.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);smoke.GetComponent<ParticleSystemRenderer>().sharedMaterial=material;d.world.southSmoke.SetActive(false);
        }
        private static GameObject Suro(Vector3 at)
        {
            var root=Point("Arwah_Mbah_Suro",at).gameObject;
            var robe=Part("Robe",0,at,1.7f,"Jubah arwah Mbah Suro");robe.transform.SetParent(root.transform,true);
            var head=Model("ElderHead",at+new Vector3(0,1.45f,0),.43f,"Wajah Mbah Suro",0,false);head.transform.SetParent(root.transform,true);
            var skull=Part("HumanSkeleton",0,at+new Vector3(.09f,1.52f,.03f),.29f,"Separuh wajah tengkorak");skull.transform.SetParent(root.transform,true);
            var staff=Part("Robe",4,at+new Vector3(.5f,0,0),1.8f,"Tongkat Mbah Suro");staff.transform.SetParent(root.transform,true);return root;
        }
        private static void Dressing()
        {
            string[] ids=Directory.GetFiles(Imported+"Models","*.fbx").Select(Path.GetFileNameWithoutExtension).Where(s=>!new[]{"Joglo","BambooSet","Banyan","Keris","StoneWell","HumanSkeleton","Robe","ElderHead","ForestStairs"}.Contains(s)).OrderBy(s=>s).ToArray();
            int nature=0,furniture=0,altar=0,kitchen=0,workshop=0;
            foreach(var id in ids)
            {
                bool natural=new[]{"rock","coast","root","grass","moss","shrub","single_root","dead_tree","dry_branches","pine_roots","anthurium"}.Any(s=>id.StartsWith(s));
                bool food=id.StartsWith("food_")||new[]{"bananas","sweet_potato","yellow_onion","tea_set_01","wooden_bowl_01","wooden_bowl_02","wooden_spoon"}.Contains(id);
                bool ritual=id.StartsWith("brass_")||id.StartsWith("ceramic_")||id.Contains("vase")||id.Contains("elephant");
                if(natural)
                {
                    int i=nature++;float angle=i*2.399963f;var p=new Vector3(Mathf.Sin(angle)*(20+i%3*7),0,Mathf.Cos(angle)*(22+i%4*4));float h=id.StartsWith("grass")?.4f:id.StartsWith("moss")?.08f:id.StartsWith("rock_face")?4:id.Contains("coast")?1.5f:id.Contains("tree")?1: .65f;
                    if(id.StartsWith("coast"))Place(id,"Models/"+id,p,new Vector3(4,h,3.5f),i*31,true,"Batuan batas desa");
                    else if(id.StartsWith("rock_moss_set"))Place(id,"Variants/"+id+"_part_0",p,Vector3.one*h,i*31,true,"Batu lumut makam",true);
                    else Model(id,p,h,"Lereng akar / pinggir makam",i*31,h>.5f);
                }
                else if(food)
                {
                    int i=kitchen++;Vector3 p=new Vector3(-15+(i%4)*.5f,.86f,-12+(i/4)*1.9f);
                    if(i%4==0)Model("WoodenTable_03",new Vector3(-14.2f,0,-12+(i/4)*1.9f),.84f,"Meja jamuan ditinggalkan",0,true);
                    Model(id,p,id.Contains("spoon")?.03f:.13f,"Sisa jamuan warga 1997",i*27,false);
                }
                else if(ritual)
                {
                    int i=altar++;Vector3 p=new Vector3(14+(i/4%2)*2.5f+(i%4)*.45f,.83f,12+(i/8)*2.2f);
                    if(i%4==0)Model("WoodenTable_03",new Vector3(14.65f+(i/4%2)*2.5f,0,12+(i/8)*2.2f),.81f,"Meja persiapan sesaji",0,true);
                    Model(id,p,id.Contains("candle")?.45f:id.Contains("elephant")?.12f:.3f,"Peralatan sesaji desa",i*33,false);
                }
                else if(id.ToLowerInvariant().Contains("chair")||id.ToLowerInvariant().Contains("table")||id.Contains("stool")||id.Contains("cabinet")||id.Contains("shelves"))
                {int i=furniture++;Model(id,new Vector3((i/4%2==0?-16:16)+(i%2==0?-2:2),0,i/8==0?-18:i/8==1?8:31),id.Contains("cabinet")||id.Contains("shelves")?1.5f:.85f,"Perabot beranda warga",i*51,true);}
                else
                {int i=workshop++;Model(id,new Vector3(34+(i%3)*.75f,0,-15+(i/3)*1.4f),id.Contains("ladder")?1.8f:id.Contains("broom")?1.3f:id.Contains("lantern")?.6f:.4f,"Peralatan kerja dan kenangan warga",i*23,true);}
            }
            // Small clumps repeat for density, while the manifest counts each source once.
            for(int i=0;i<100;i++){float a=i*2.399963f;Model(i%2==0?"grass_medium_01":"grass_medium_02",new Vector3(Mathf.Sin(a)*(11+i%10*3),-.03f,Mathf.Cos(a)*(11+i%9*3)),.28f+(i%3)*.09f,"Rumput sela desa",i*37,false);}
        }
        private static void Lighting()
        {
            var presentation=d.gameObject.AddComponent<Phase4Lighting>();presentation.playerCamera=d.phoneView.playerCamera;
            d.phoneView.playerCamera.clearFlags=CameraClearFlags.SolidColor;d.phoneView.playerCamera.backgroundColor=presentation.night;RenderSettings.skybox=null;
            RenderSettings.ambientMode=AmbientMode.Trilight;RenderSettings.ambientSkyColor=new Color(.32f,.38f,.46f);RenderSettings.ambientEquatorColor=new Color(.19f,.23f,.25f);RenderSettings.ambientGroundColor=new Color(.08f,.075f,.065f);
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogColor=new Color(.06f,.085f,.1f);RenderSettings.fogDensity=.012f;
            var moon=Point("Moon",Vector3.up*25).gameObject.AddComponent<Light>();moon.type=LightType.Directional;moon.color=new Color(.66f,.76f,.87f);moon.intensity=.9f;moon.shadows=LightShadows.Soft;moon.transform.rotation=Quaternion.Euler(43,-35,0);
            Lamp("Pendopo_Ember",new Vector3(0,3,-1),new Color(1,.55f,.27f),2.5f,12);Lamp("Well_Moonlight",new Vector3(-29,4,3),new Color(.38f,.56f,.73f),2,10);Lamp("Gate_Glow",new Vector3(0,-1,16),new Color(.4f,.22f,.68f),3,12);
            var profile=ScriptableObject.CreateInstance<VolumeProfile>();string path=Art+"KarangSetraVolume.asset";if(AssetDatabase.LoadAssetAtPath<VolumeProfile>(path))AssetDatabase.DeleteAsset(path);AssetDatabase.CreateAsset(profile,path);
            var tone=profile.Add<Tonemapping>(true);tone.mode.Override(TonemappingMode.ACES);AssetDatabase.AddObjectToAsset(tone,profile);
            var grade=profile.Add<ColorAdjustments>(true);grade.postExposure.Override(.25f);grade.contrast.Override(9);grade.saturation.Override(-14);AssetDatabase.AddObjectToAsset(grade,profile);
            var vignette=profile.Add<Vignette>(true);vignette.intensity.Override(.16f);AssetDatabase.AddObjectToAsset(vignette,profile);
            var bloom=profile.Add<Bloom>(true);bloom.intensity.Override(.12f);bloom.threshold.Override(1.3f);AssetDatabase.AddObjectToAsset(bloom,profile);
            var volume=Point("KarangSetra_Grade",Vector3.zero).gameObject.AddComponent<Volume>();volume.isGlobal=true;volume.priority=10;volume.sharedProfile=profile;
            var fear=d.gameObject.AddComponent<Phase4FearPresentation>();fear.director=d;fear.volume=volume;
        }
    }
}
#endif
