#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using LayarTerakhir.Phase5;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        private static void Architecture()
        {
            // A street arrival, not a duplicate of the Phase4 underground vestibule.
            Floor("MarketMud",new Vector3(0,0,28),new Vector2(42,64),"brown_mud_03");
            Anchor("GATE",new Vector3(0,.08f,4)); Zone("GATE",new Vector3(0,1,28),new Vector3(60,15,64));
            Module("FortGate",new Vector3(0,0,53),new Vector3(13,7,2),"Gerbang dalam Pasar Setan");
            Label("MarketGate","PASAR SETAN\nTIGA GERBANG KENANGAN",new Vector3(0,4.3f,51.9f),180,.018f);
            d.world.sectorBlockers=new[]{Blocker("ClosedInnerGate",new Vector3(0,1.5f,54),new Vector3(7,3,1))};
            Module("FacadeDoor",new Vector3(0,0,54),new Vector3(6,3,.2f),"Daun gerbang kenangan",0,false).transform.SetParent(d.world.sectorBlockers[0].transform,true);
            Item("START_SECTORS",new Vector3(0,1.25f,52.4f),"Masuki tiga gerbang kenangan");
            Model("MarketStall",new Vector3(-10,0,13),4.2f,90,"Kios Tumbal berhadapan dengan jalan",true);
            Model("MarketStall",new Vector3(10,0,13),4.2f,-90,"Kios Arsip Bisu di ceruk kanan",true);
            Model("StoreMannequin",new Vector3(-8,0,12),1.75f,90,"Penjaga kios tanpa wajah manusia",false);
            Model("CashRegister_01",new Vector3(-6.8f,.85f,12),.5f,90,"Terminal dana kios");
            Item("SHOP",new Vector3(-6.4f,1.2f,12),"Periksa perbekalan opsional");
            Item("RULES",new Vector3(-6.5f,1.15f,10.9f),"Baca Kertas Aturan Pasar Setan");
            Label("Shop","KIOS TUMBAL\nPERBEKALAN OPSIONAL",new Vector3(-6.8f,2.6f,12),90,.012f);
            Recovery("GATE",new Vector3(-4,0,7),new Vector3(4,0,7));
            d.world.chair=Point("EmptyWitnessChair",new Vector3(6.5f,0,14));
            Model("ArmChair_01",d.world.chair.position,1.1f,-90,"Kursi saksi kosong di ceruk kanan",true);
            Model("StoreMannequin",new Vector3(9,0,16),1.7f,-90,"Manekin Kios Arsip Bisu",false);
            var drawer=Model("drawer_cabinet",new Vector3(7.9f,0,16),1.15f,-90,"Laci Pita Rekaman Pertama",true);
            var drawerMesh=drawer.GetComponentsInChildren<MeshRenderer>().Where(r=>r.name.Contains("drawer_cabinet_drawer_")).OrderByDescending(r=>r.bounds.center.y).First();
            PrefabUtility.UnpackPrefabInstance(PrefabUtility.GetOutermostPrefabInstanceRoot(drawerMesh.gameObject),PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            d.world.drawer=Point("TapeDrawerMove",drawerMesh.bounds.center,drawer.transform).gameObject;
            drawerMesh.transform.SetParent(d.world.drawer.transform,true);
            d.world.drawerRest=d.world.drawer.transform.localPosition;d.world.drawerOffset=drawer.transform.InverseTransformVector(Vector3.back*.28f);
            d.world.drawerFocus=Point("AR_TapeDrawer",new Vector3(7.9f,1.15f,15.5f));
            d.world.tape=Model("boombox",new Vector3(7.9f,1.16f,15.7f),.25f,180,"Rekaman pertama dalam laci saksi");
            d.world.tape.transform.SetParent(d.world.drawer.transform,true);
            Item("TAPE",new Vector3(7.9f,1.2f,15.4f),"Ambil Pita Rekaman Pertama",null,d.world.tape.transform);
            Label("WitnessChair","KIOS ARSIP BISU\nSAKSI TIDAK MEMBELI KORBAN\nDiamkan layar, lalu periksa laci",new Vector3(6.8f,2.55f,15.3f),-90,.01f);
            d.world.gateBackMirror=Model("AntiqueMirror",new Vector3(3,0,48),2.7f,180,"Cermin Jalan Pulang sisi gerbang",false);
            Item("GATE_RETURN",new Vector3(3,1.4f,47.6f),"Kembali ke Ruang Cermin Saksi");
            // Frozen corridor: long, readable approach and three fixed doors.
            Floor("FrozenHall",new Vector3(0,0,77),new Vector2(13,40),"concrete_wall_007");
            for(int side=-1;side<=1;side+=2)
                for(int i=0;i<5;i++)Wall(new Vector3(side*6.4f,0,61+i*7),new Vector2(7.05f,3.8f),"Dinding Koridor Beku",90);
            var ceiling=Module("FacadeFloor",new Vector3(0,3.85f,77),new Vector3(13,.02f,40),"Plafon beton beku",0,false);SurfaceOn(ceiling,"concrete_wall_007",new Vector2(6,18));
            Anchor("S1",new Vector3(0,.08f,62));Zone("S1",new Vector3(0,1,76),new Vector3(16,12,36));
            d.world.doors=new Transform[3];d.world.doorLeaves=new GameObject[3];
            for(int i=0;i<3;i++)
            {
                float x=(i-1)*3.8f;
                Module("FacadeDoorFrame",new Vector3(x,0,91),new Vector3(3.8f,3.5f,.2f),"Rangka Pintu Cermin "+i);
                d.world.doorLeaves[i]=Module("FacadeDoor",new Vector3(x,0,90.85f),new Vector3(2.4f,2.8f,.2f),"Pintu Cermin "+i);
                var solid=d.world.doorLeaves[i].AddComponent<BoxCollider>();solid.center=Vector3.up*1.4f;solid.size=new Vector3(2.4f,2.8f,.2f);
                d.world.doors[i]=Point("ThermoDoor_"+i,new Vector3(x,1.4f,90.55f));
                Item("DOOR_"+i,d.world.doors[i].position,"Periksa Pintu Cermin",new Vector3(1.7f,2.5f,.2f));
                Label("DoorStencil"+i,i==1?"TENGAH\n−22°C":i==0?"KIRI":"KANAN",new Vector3(x,2.8f,90.55f),180,.008f);
            }
            Recovery("S1A",new Vector3(-4.7f,0,64),new Vector3(4.7f,0,64));
            Recovery("S1B",new Vector3(-4.8f,0,86),new Vector3(4.8f,0,86));
            d.world.ghostWaypoints=new[]{Point("Ghost_Start",new Vector3(0,.05f,54)),Point("Ghost_End",new Vector3(0,.05f,89))};
            // Bridge: two physically distinct branches, with space to avoid the
            // UV-marked unstable planks instead of requiring a new jump system.
            Floor("BridgeApron",new Vector3(0,0,99),new Vector2(18,10),"wooden_rough_planks");
            Anchor("S2",new Vector3(0,.08f,99));Zone("S2",new Vector3(0,1,124),new Vector3(30,16,58));
            for(int side=-1;side<=1;side+=2)
            {
                if(side<0)for(int j=0;j<4;j++)Floor("BranchBridge_Left_"+j,new Vector3(-6,0,109+j*10),new Vector2(4.8f,10.05f),"wooden_rough_planks");
                else for(int row=0;row<20;row++)for(int col=0;col<3;col++)
                {
                    float z=105+row*2;if(col==0&&(Mathf.Approximately(z,115)||Mathf.Approximately(z,127)))continue;
                    Floor("BridgeRightCell_"+row+"_"+col,new Vector3(4.4f+col*1.6f,0,z),new Vector2(1.605f,2.005f),"wooden_rough_planks");
                }
                for(int j=0;j<11;j++)
                {
                    Module("ChainlinkPanel",new Vector3(side*6+2.5f,0,104+j*4),new Vector3(4.05f,1.2f,.08f),"Parapet jembatan bambu-besi",90,true);
                    // The author's fence module is fitted only for scale; the
                    // walkway retains a ≥3m capsule route between supports.
                }
            }
            Floor("BridgeExitApron",new Vector3(6,0,147),new Vector2(14,12),"wooden_rough_planks");
            Wall(new Vector3(-6,0,141),new Vector2(5,3),"Cabang suara tiruan berakhir di papan tertutup",0,"wooden_rough_planks");
            d.world.bridgeBlocker=Blocker("SantetBridgeSeal",new Vector3(6,1.4f,151),new Vector3(5.5f,2.8f,.5f));
            d.world.bridgeFlame=Model("StageLantern",new Vector3(6,0,150.5f),1.2f,0,"Satu Obor Santet penghalang jembatan");
            Lamp("SantetLight",new Vector3(6,1,150.5f),new Color(1,.18f,.045f),3,7).transform.SetParent(d.world.bridgeFlame.transform,true);
            Item("POUR",new Vector3(6,.9f,149.8f),"Siram satu Obor Santet dengan Kendi / Cawan");
            Model("modified_thermos",new Vector3(10,.7f,146),.65f,180,"Titik air dan Cawan Air Pasar pengganti Kendi");
            Item("REFILL",new Vector3(10,1.1f,145.5f),"Tahan untuk mengisi Kendi / Cawan");Item("CUP",new Vector3(10.8f,1,145.5f),"Periksa wadah air cadangan");
            Recovery("S2A",new Vector3(6.4f,0,99),new Vector3(-5,0,99));
            Recovery("S2B",new Vector3(11,0,148),new Vector3(1,0,147));
            Label("Water","AIR RUWATAN\nISI ULANG GRATIS",new Vector3(10,1.8f,145.7f),180,.008f);
            d.world.collapsePlanks=new GameObject[2];
            for(int i=0;i<2;i++)
            {
                var plank=Floor("UnstablePlank_"+i,new Vector3(4.4f,0,115+i*12),new Vector2(1.6f,2),"wooden_rough_planks");d.world.collapsePlanks[i]=plank;
                var hazard=plank.AddComponent<Phase5UnstablePlank>();hazard.director=d;
            }
            // U corridor. The central island prevents straight-line shortcuts;
            // closets/workbenches leave two routes for breaking sightlines.
            Floor("SewingWorkshopFloor",new Vector3(0,0,172),new Vector2(25,40),"wooden_rough_planks");
            Floor("SewingWorkshopCeiling",new Vector3(0,3.8f,172),new Vector2(25,40),"wooden_rough_planks");
            Anchor("S3",new Vector3(7,.08f,156));Zone("S3",new Vector3(0,1,171),new Vector3(28,12,40));
            for(int side=-1;side<=1;side+=2)
            {
                Wall(new Vector3(side*12.5f,0,172),new Vector2(40,3.6f),"Perimeter Ruang Jahit",90,"wooden_rough_planks");
                Wall(new Vector3(side*4,0,167),new Vector2(22,3.6f),"Pulau tengah lorong U",90,"wooden_rough_planks");
            }
            Wall(new Vector3(0,0,178),new Vector2(8,3.6f),"Ujung pulau lorong U",0,"wooden_rough_planks");
            Wall(new Vector3(0,0,156),new Vector2(8,3.6f),"Awal pulau lorong U",0,"wooden_rough_planks");
            Wall(new Vector3(0,0,191),new Vector2(25,3.6f),"Dinding belakang ruang jahit",0,"wooden_rough_planks");
            Model("SewingMachine",new Vector3(0,0,183),1.15f,180,"Mesin jahit landmark putaran lorong U",true);
            Model("AntiqueMirror",new Vector3(-8,0,154),3,0,"Cermin Rias portal ke Panggung Lelang");
            Item("SEWING_PORTAL",new Vector3(-8,1.3f,154.4f),"Periksa permukaan Cermin Rias");
            Recovery("S3A",new Vector3(10.7f,0,156),new Vector3(10.7f,0,160));
            Recovery("S3B",new Vector3(-10.7f,0,157),new Vector3(-10.7f,0,161));
            d.world.sriWaypoints=new[]{Point("Sri_A",new Vector3(8,0,168)),Point("Sri_B",new Vector3(8,0,184)),Point("Sri_C",new Vector3(-8,0,184)),Point("Sri_D",new Vector3(-8,0,166))};
            d.world.giantWaypoints=new[]{Point("Giant_A",new Vector3(-8,0,175)),Point("Giant_B",new Vector3(-8,0,185)),Point("Giant_C",new Vector3(8,0,185)),Point("Giant_D",new Vector3(8,0,175))};
            // Stage and antechamber share a physically continuous floor.
            Floor("AuctionStage",new Vector3(0,0,220),new Vector2(40,44),"wooden_rough_planks");
            Floor("MirrorAntechamber",new Vector3(21,0,220),new Vector2(9,14),"concrete_wall_007");
            Anchor("STAGE",new Vector3(0,.08f,201));Anchor("MIRROR",new Vector3(14,.08f,218),90);
            Anchor("ARENA_WEST",new Vector3(-14.5f,.08f,216.4f));Anchor("ARENA_EAST",new Vector3(13.5f,.08f,232.5f),180);
            Anchor("PORTAL",new Vector3(12,.08f,220),90);Anchor("ENDING",new Vector3(12,.08f,220),90);
            Zone("MIRROR",new Vector3(21,1,220),new Vector3(12,12,16));Zone("STAGE",new Vector3(0,1,220),new Vector3(40,16,46));
            d.world.arenaBlocker=Blocker("ArenaEntryLock",new Vector3(0,1.5f,201),new Vector3(11,3,.5f));
            Module("ChainlinkPanel",new Vector3(0,0,201),new Vector3(11,3,.15f),"Gerbang panggung terkunci setelah CP6",0,false).transform.SetParent(d.world.arenaBlocker.transform,true);
            Item("START_ARENA",new Vector3(0,1.1f,208),"Aktifkan Panggung Lelang setelah memeriksa Arsip");
            d.world.generator=Point("GeneratorFocus",new Vector3(-9,1,221));Model("MarketGenerator",new Vector3(-9,0,221),1.4f,90,"Generator Lampu Sorot Panggung",true);
            Item("GENERATOR",new Vector3(-8.5f,1.1f,220),"Nyalakan sakelar saat wujud asli di zona sorot");
            d.world.generatorLight=Lamp("GeneratorSpot",new Vector3(-9,4,221),new Color(.92f,.85f,.62f),12,13,true);d.world.generatorLight.enabled=false;
            Recovery("ARENA_W",new Vector3(-14.5f,0,219),new Vector3(-14.5f,0,223));
            Recovery("ARENA_E",new Vector3(13,0,230),new Vector3(14,0,226));
            Recovery("MIRROR",new Vector3(24,0,216),new Vector3(24,0,224));
            d.world.lanternPosts=new Transform[3];d.world.lanternVisuals=new GameObject[3];Vector3[] lamps={new Vector3(-11,0,211),new Vector3(9,0,232),new Vector3(7,0,211)};
            for(int i=0;i<3;i++){d.world.lanternVisuals[i]=Model("StageLantern",lamps[i]+Vector3.up*.6f,.7f,0,"Lentera Panggung reusable "+i);d.world.lanternPosts[i]=Point("LanternPost_"+i,lamps[i]);Item("LANTERN_"+i,lamps[i]+Vector3.up,"Ambil Lentera Panggung gratis");Lamp("LanternRest_"+i,lamps[i]+Vector3.up,Color.yellow,.6f,5);}
            d.world.droppedLight=Lamp("Dropped_Flare_Or_Lantern",Vector3.zero,new Color(1,.3f,.1f),8,7);d.world.droppedLight.enabled=false;
            var cover=new List<Transform>();Vector3[] corners={new Vector3(-10,0,210),new Vector3(10,0,210),new Vector3(-11,0,231),new Vector3(6,0,234)};
            foreach(var at in corners)
            {
                cover.Add(Point("CoverCenter",at));Model("wooden_military_crate",at,1.5f,90,"Peti kayu cover sapuan api",true);
                Model("wooden_military_crate",at+new Vector3(1.6f,0,1),1.25f,0,"Sayap peti cover dengan dua jalur keluar",true);
            }
            d.world.coverCenters=cover.ToArray();d.boss.waveOrigin=Point("FireWaveOrigin",new Vector3(-19,1,220));
            d.world.portal=Point("FinalPortalStanding",new Vector3(14,0,220));
            Model("AntiqueMirror",new Vector3(17,0,220),3.6f,90,"Cermin Saksi dan portal akhir");
            d.world.archiveMirror=Point("ArchiveMirrorFocus",new Vector3(16.5f,1.6f,220));Item("ARCHIVE",d.world.archiveMirror.position,"Validasi lima Arsip Saksi",new Vector3(.4f,2.4f,2));
            Model("AntiqueMirror",new Vector3(22,0,220),2.9f,90,"Cermin Jalan Pulang ke Kios Arsip Bisu");
            Item("BACK_MIRROR",new Vector3(21.5f,1.5f,220),"Lintasi Cermin Jalan Pulang — kembali ke gerbang");
            d.world.edge=Point("FinalEdge",new Vector3(-17,0,207));Label("FinalEdge","TEPI PANGGUNG\nPUTUS SIARAN",new Vector3(-17,1.5f,206),180,.008f);
        }
        private static void Recovery(string suffix,Vector3 charge,Vector3 prayer)
        {
            Model("MarketGenerator",charge,.85f,180,"Dinamo fisik gratis "+suffix,true);Item("DYNAMO_"+suffix,charge+Vector3.up*.9f,"Tahan interaksi • dinamo gratis +25% /8s");
            Model("GothicCommode_01",prayer,.7f,180,"Altar Doa Pasar gratis "+suffix,true);Item("PRAYER_"+suffix,prayer+Vector3.up*.85f,"Tahan interaksi • Altar Doa +15 Sanity /10s");
            if(suffix.StartsWith("ARENA_"))
            {
                Vector3 center=(charge+prayer)*.5f;
                // A real low canopy excludes the tall NavMesh agent. The player
                // can crawl through either end; there is no invulnerability flag.
                Floor("RecoveryLowCanopy_"+suffix,center+Vector3.up*1.4f,new Vector2(3.6f,8.2f),"wooden_rough_planks");
                Wall(center+new Vector3(-1.8f,0,0),new Vector2(8.2f,1.2f),"Crawl recovery side "+suffix,90,"wooden_rough_planks");
                Wall(center+new Vector3(1.8f,0,0),new Vector2(8.2f,1.2f),"Crawl recovery side "+suffix,90,"wooden_rough_planks");
                Label("CrawlRecovery"+suffix,"JONGKOK UNTUK BERLINDUNG\nDINAMO / DOA GRATIS",center+new Vector3(0,1.7f,-4.1f),180,.007f);
            }
            Label("Recovery"+suffix,"DINAMO / DOA\nGRATIS",(charge+prayer)*.5f+Vector3.up*2,180,.007f);
            Lamp("RecoveryWarm_"+suffix,(charge+prayer)*.5f+Vector3.up*2.8f,new Color(1,.65f,.33f),1.3f,7);
        }
    }
}
#endif
