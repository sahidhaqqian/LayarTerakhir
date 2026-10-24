#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace LayarTerakhir.Editor
{
    public static partial class Phase3SceneBuilder
    {
        private static GameObject Gate(string name,Vector3 p,Vector3 size,float yaw=0)
        {
            var go=Module("modular_factory_facade","door_garage_door_01",p,size,yaw,false);go.name=name;
            var leaf=Point("DoorLeaf",p,go.transform,yaw);leaf.localPosition=Vector3.zero;leaf.localRotation=Quaternion.identity;
            go.transform.Find("SourceFit").SetParent(leaf,true);
            var c=leaf.gameObject.AddComponent<BoxCollider>();c.center=Vector3.up*size.y/2;c.size=size;
            var gate=go.AddComponent<Phase3Gate>();gate.director=d;gate.leaf=leaf;gate.openOffset=Vector3.up*(size.y+.25f);foreach(var t in go.GetComponentsInChildren<Transform>())GameObjectUtility.SetStaticEditorFlags(t.gameObject,0);return go;
        }
        private static void Investigation()
        {
            d.world.accessGate=Gate("G_ACC_MagneticGate",new Vector3(24,0,24),new Vector3(4,3,.25f),90);
            var generator=PH("portable_generator",new Vector3(21,0,26),new Vector3(1.2f,.9f,.8f));Item(generator,"G_ACC","Generator akses — engkol tiga kali");
            d.world.accessLight=LightAt("Access_Halogen",new Vector3(23,4,27),new Color(1,.73f,.35f),12,16,true);
            PH("portable_searchlight",new Vector3(23,3.8f,27),new Vector3(.7f,.5f,.5f),0,false);
            d.world.sling=Converted("Cloth",new Vector3(21,1.5f,29),new Vector3(2,.8f,1.2f),180,false);
            foreach(float x in new[]{20.15f,21.85f})Module("modular_industrial_pipes_01","modular_industrial_pipes_01_pipe02",new Vector3(x,2.15f,29),new Vector3(.04f,3.5f,.04f),0,false);
            d.world.slingTarget=Point("SlingPhotoTarget",new Vector3(21,2,29));
            Label("Sling_AR_Echo","JEJAK GENDONGAN TUMBAL",new Vector3(21,2.6f,29),0,.15f,29);
            d.world.doll=Converted("Doll",new Vector3(20.6f,1.7f,28.7f),new Vector3(.2f,.28f,.15f),180,false);Item(d.world.doll,"DOLL","Ambil Boneka Jerami Tebu");
            var note=PH("office_notepads",new Vector3(21.6f,1.75f,28.8f),new Vector3(.32f,.025f,.24f),180,false);Item(note,"MANDOR","Baca Buku Catatan Mandor — 1999");
            Emitter("MILL_MIMIC",new Vector3(7,1,24),Phase3VoiceTag.Mimic,"Suara Damar dari jembatan kiri");
            for(int z=12;z<20;z+=2)Tile(new Vector3(7.5f,0,z+1),new Vector3(5,.12f,2),Phase3FloorKind.FragileFatal,false);
            d.world.sickle=Converted("Sickle",new Vector3(53,5.02f,36),new Vector3(.5f,.08f,.3f),45,false);Item(d.world.sickle,"SICKLE","Ambil Sabit Tebu Berukir Rajah");
            PH("industrial_storage_cart",new Vector3(53,4,36),new Vector3(2,1,1));
            // Maintenance partition gives the recorder a real occlusion pocket; the stairs flank it.
            Wall(new Vector3(49,4,32.5f),10,2.6f);
            PH("steel_frame_shelves_01",new Vector3(44,4,37),new Vector3(1.6f,2.2f,.6f),90);
            PH("vintage_radio_transceiver",new Vector3(47,5,36),new Vector3(.7f,.5f,.4f),180);
            Emitter("INTERCOM",new Vector3(47,5.4f,35.8f),Phase3VoiceTag.Mimic,"Interkom: nina bobo tiruan / rintihan autentik",10,true);
            Label("IntercomInstructions","INTERKOM MANDOR\nREKAM RINTIHAN ASLI\nNINA BOBO = TIRUAN",new Vector3(47,6.3f,36.2f),0,.13f);
            d.world.rajahTarget=Point("RajahPhotoTarget",new Vector3(24.3f,1.8f,35));
            string glyphPath=Imported+"Decals/RajahCircle.png";var glyphImporter=(TextureImporter)AssetImporter.GetAtPath(glyphPath);
            if(glyphImporter.textureType!=TextureImporterType.Sprite){glyphImporter.textureType=TextureImporterType.Sprite;glyphImporter.spritePixelsPerUnit=100;glyphImporter.SaveAndReimport();}
            string glyphMatPath=Art+"Materials/UV_RajahInk.mat";var glyphMat=AssetDatabase.LoadAssetAtPath<Material>(glyphMatPath);
            if(!glyphMat){glyphMat=new Material(Shader.Find("LayarTerakhir/Phase3 UV Mark"));AssetDatabase.CreateAsset(glyphMat,glyphMatPath);}
            glyphMat.SetTexture("_MainTex",AssetDatabase.LoadAssetAtPath<Texture2D>(glyphPath));glyphMat.SetColor("_Color",new Color(.85f,.5f,1.4f,.95f));
            var glyph=Point("Rajah_LicensedCircle_UV",d.world.rajahTarget.position).gameObject;glyph.layer=24;
            var sprite=glyph.AddComponent<SpriteRenderer>();sprite.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(glyphPath);sprite.sharedMaterial=glyphMat;
            glyph.transform.rotation=Quaternion.Euler(0,270,0);glyph.transform.localScale=Vector3.one*.22f;
            Label("Rajah_Children_UV","3 + 6",d.world.rajahTarget.position+Vector3.right*.015f,270,.18f,24);
            Label("Rajah_ChildrenWords_UV","SEMBUNYIKAN ANAK",d.world.rajahTarget.position+Vector3.up*1.3f,270,.14f,24);
            d.world.cloth=Converted("Cloth",new Vector3(25.4f,.3f,35),new Vector3(.8f,.1f,.5f),0,false);Item(d.world.cloth,"CLOTH","Ambil Selendang Batik Terbakar");
            d.world.card=PH("office_notepads",new Vector3(26,.15f,35),new Vector3(.16f,.008f,.24f),0,false);Item(d.world.card,"DAMAR_CARD","Periksa Kartu Pers Damar dan tulisan arang");
            d.world.shoe=Converted("Shoe",new Vector3(26.5f,.05f,35),new Vector3(.12f,.12f,.31f),25,false);
            Item(d.world.shoe,"DAMAR_SHOE","Ambil sepatu kiri Damar");
            Label("CharcoalDamar","KA... KARANG SETRA.\nCHAT LO BUKAN MANUSIA SEMUA.",new Vector3(24.3f,1.3f,37.5f),270,.16f);
            var locker=Converted("Locker",new Vector3(36,0,23),new Vector3(1.2f,2.2f,1));
            Item(locker,"HIDE_LOCKER","Masuk loker pekerja");Point("HideAnchor",new Vector3(36,.02f,23),locker.transform,180);
            var furnace=PH("barrel_stove",new Vector3(31,0,28),new Vector3(1.5f,1.8f,1.4f));
            Item(furnace,"HIDE_FURNACE","Merangkak ke tungku mati");Point("HideAnchor",new Vector3(31,.02f,28),furnace.transform,90);
            foreach(var entry in new[]{("DYNAMO_YARD",new Vector3(3,0,-9)),("DYNAMO_BOILER",new Vector3(27,0,18)),("DYNAMO_OFFICE",new Vector3(58,4,34))})
            {
                var go=PH("vintage_hand_drill",entry.Item2+Vector3.up*.8f,new Vector3(.5f,.5f,.4f));Item(go,entry.Item1,"Dinamo HP — tahan E");
                Label(entry.Item1+"_Label","ENGKOL DAYA HP\nNORMAL SEKALI / DARURAT <10%",entry.Item2+new Vector3(0,1.7f,.4f),0,.12f);
            }
            Emitter("VENT09",new Vector3(46,5.3f,43.4f),Phase3VoiceTag.Authentic,"Tiga ketukan dari ventilasi balkon",20);
            Emitter("BALCONY_MIMIC",new Vector3(61,5,43),Phase3VoiceTag.Mimic,"Damar palsu dari papan lapuk",1);
            d.world.secretUvTarget=Point("SecretUvTarget",new Vector3(51,4.15f,46));
            Label("UV_09_Clue","NOMOR SEMBILAN TIDAK PULANG\n→ → →",new Vector3(51,4.8f,46),0,.19f,24);
            foreach(var n in new[]{1,8,9})
            {
                var valve=Module("modular_industrial_pipes_01","modular_industrial_pipes_01_pipe06",new Vector3(55+n%3*1.5f,4.8f,46),new Vector3(.4f,.4f,.4f));Item(valve,"VALVE_"+n.ToString("00"),"Katup "+n.ToString("00"));
                Label("ValveLabel_"+n,n.ToString("00"),valve.transform.position+Vector3.up*.6f,0,.17f);
            }
            var archive=Converted("Locker",new Vector3(60,4,46),new Vector3(1.2f,2.2f,.8f),180);Item(archive,"LOCKER_09","Loker 09 — ambil Pelat Nomor Buruh");
            d.world.secret=Module("modular_factory_facade","door_garage_door_01",new Vector3(60,5,45.5f),new Vector3(.2f,.28f,.015f),0,false);Item(d.world.secret,"LOCKER_09","Ambil Pelat Nomor Buruh 09");
            Label("ArchiveLockerLabel","09",new Vector3(60,6,45.5f),0,.23f);
            var drop=Tile(new Vector3(58.5f,4,43.5f),new Vector3(3,.12f,3),Phase3FloorKind.FragileDrop,true);drop.lowerLanding=Point("BalconyLowerRestore",new Vector3(60,.04f,38));
            var crack=Tile(new Vector3(39.5f,4,31.5f),new Vector3(3,.12f,3),Phase3FloorKind.FragileDrop,true);crack.lowerLanding=Point("StairLowerRestore",new Vector3(39,.04f,28));
            // A false visual gap over intact collision. The UV feed removes only this illusion layer.
            var illusion=Module("modular_wooden_pier","modular_wooden_pier_planks",new Vector3(59,.025f,27),new Vector3(2.8f,.025f,2.8f),0,false);
            illusion.name="IllusionVoid_IntactSteelBelow";
            string illusionPath=Art+"Materials/IllusionVoid.mat";var illusionMat=AssetDatabase.LoadAssetAtPath<Material>(illusionPath);
            if(!illusionMat){illusionMat=new Material(Surface("metal_grate_rusty"));AssetDatabase.CreateAsset(illusionMat,illusionPath);}
            illusionMat.SetColor("_BaseColor",new Color(.002f,.002f,.004f));illusionMat.SetFloat("_Smoothness",0);
            foreach(var r in illusion.GetComponentsInChildren<Renderer>()){r.gameObject.layer=30;r.sharedMaterial=illusionMat;r.shadowCastingMode=ShadowCastingMode.Off;}
            Label("UV_IntactSteel","PIJAKAN UTUH",new Vector3(59,.08f,27),0,.18f,24);
            var model=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx");
            var motor=(GameObject)PrefabUtility.InstantiatePrefab(model);motor.name="RakaMotor_CampaignCarryOver";motor.transform.SetParent(env,false);
            var bikeRenderers=motor.GetComponentsInChildren<Renderer>();var bikeBounds=bikeRenderers[0].bounds;
            foreach(var renderer in bikeRenderers.Skip(1))bikeBounds.Encapsulate(renderer.bounds);
            motor.transform.localScale*=1.15f/Mathf.Max(.01f,bikeBounds.size.y);
            bikeBounds=bikeRenderers[0].bounds;foreach(var renderer in bikeRenderers.Skip(1))bikeBounds.Encapsulate(renderer.bounds);
            motor.transform.position+=new Vector3(16-bikeBounds.center.x,-bikeBounds.min.y,-14-bikeBounds.center.z);
            bikeBounds=bikeRenderers[0].bounds;foreach(var renderer in bikeRenderers.Skip(1))bikeBounds.Encapsulate(renderer.bounds);
            var bikeHull=motor.AddComponent<BoxCollider>();bikeHull.center=motor.transform.InverseTransformPoint(bikeBounds.center);
            Vector3 localSize=motor.transform.InverseTransformVector(bikeBounds.size);bikeHull.size=new Vector3(Mathf.Abs(localSize.x),Mathf.Abs(localSize.y),Mathf.Abs(localSize.z));
            Item(motor,"EXIT_MOTOR","Tinggalkan Pabrik Gula",new Vector3(15.7f,.85f,-13.8f));
        }
        private static Phase3FloorTile Tile(Vector3 at,Vector3 size,Phase3FloorKind kind,bool dynamic)
        {
            var root=Point("Floor_"+kind+"_"+floors.Count,at).gameObject;var tile=root.AddComponent<Phase3FloorTile>();tile.director=d;tile.kind=kind;tile.dynamicCandidate=dynamic;
            var surface=Module("modular_wooden_pier","modular_wooden_pier_planks",at-Vector3.up*.12f,size,0,false);surface.transform.SetParent(root.transform,true);
            var c=surface.AddComponent<BoxCollider>();c.size=size;c.center=Vector3.up*size.y/2;tile.surface=surface;tile.footBounds=new Bounds(at+Vector3.up*.05f,new Vector3(size.x,.35f,size.z));
            var mark=Point("UV_Cross",at+Vector3.up*.025f,root.transform).gameObject;mark.layer=24;var tm=mark.AddComponent<TextMesh>();tm.text="×";tm.font=PresentationProfile.Load().bodyFont;tm.fontSize=80;tm.characterSize=.18f;tm.anchor=TextAnchor.MiddleCenter;tm.color=new Color(.78f,.45f,1);mark.transform.rotation=Quaternion.Euler(90,0,0);mark.GetComponent<Renderer>().sharedMaterial=tm.font.material;tile.uvMark=mark;
            floors.Add(tile);return tile;
        }
        private static void Arena()
        {
            Floor(68,96,12,20,0,"metal_grate_rusty");Floor(68,96,28,36,0,"metal_grate_rusty");Floor(68,76,20,28,0,"metal_grate_rusty");Floor(88,96,20,28,0,"metal_grate_rusty");
            Floor(76,88,20,28,-4,"factory_brick");
            // A usable second ring, with 4m clearance for the 3m actor below it.
            Floor(68,96,12,15,4,"metal_grate_rusty");Floor(68,96,33,36,4,"metal_grate_rusty");
            Floor(68,71,15,33,4,"metal_grate_rusty");Floor(94.1f,96,15,23,4,"metal_grate_rusty");Floor(93,96,23,33,4,"metal_grate_rusty");
            Stairs(new Vector3(74,0,27),new Vector3(1.8f,4,12),90);
            Stairs(new Vector3(93,0,19),new Vector3(1.8f,4,8),90);
            Rail(new Vector3(82,4,15),22,0);Rail(new Vector3(72,4,33),2,0);
            Rail(new Vector3(82,4,33),14,0);Rail(new Vector3(92,4,33),2,0);
            Rail(new Vector3(71,4,24),18,90);Rail(new Vector3(93,4,24),18,90);
            // The furnace rim is visible and collidable, including after victory.
            Rail(new Vector3(82,0,20),12,0);Rail(new Vector3(82,0,28),12,0);
            Rail(new Vector3(76,0,24),8,90);Rail(new Vector3(88,0,24),8,90);
            Floor(68,96,12,36,10,"corrugated_iron_02");
            Wall(new Vector3(82,0,12),28,10);Wall(new Vector3(82,0,36),28,10,0,true);Wall(new Vector3(96,0,18),12,10,90);Wall(new Vector3(96,0,31.5f),9,10,90);
            Wall(new Vector3(68,0,16),8,10,90);Wall(new Vector3(68,0,32),8,10,90);
            d.world.hallGate=Gate("RitualHallGate",new Vector3(68,0,24),new Vector3(5,3.5f,.3f),90);Item(d.world.hallGate,"HALL_GATE","Gerbang Aula Tungku");
            d.world.arenaLock=Point("ArenaAttemptBarrier",new Vector3(68.4f,0,24),null,90).gameObject;
            var barrier=d.world.arenaLock.AddComponent<BoxCollider>();barrier.center=Vector3.up*1.75f;barrier.size=new Vector3(5,3.5f,.3f);
            d.world.serviceGate=Gate("Welded_ServiceExit",new Vector3(96,0,24),new Vector3(4,3,.3f),90);Item(d.world.serviceGate,"SERVICE_EXIT","Gerbang servis aula — dilas");
            Converted("Boiler",new Vector3(82,-4,24),new Vector3(6,7,6),180);
            d.world.pullCentre=Point("FurnacePullCentre",new Vector3(82,0,24));
            d.world.furnacePit=new Bounds(new Vector3(82,-2,24),new Vector3(12,4,8));
            d.world.bossHome=Point("BossHome",new Vector3(82,.04f,32));d.world.bossRetreat=Point("BossRetreat",new Vector3(93,.04f,33));
            d.world.altarRelics=new GameObject[3];
            for(int i=0;i<3;i++)
            {
                var pedestal=PH("old_military_crate",new Vector3(71,0,21+i*2),new Vector3(.8f,.7f,.8f));Item(pedestal,"ALTAR_"+i,"Dudukan pusaka: "+new[]{"Boneka Jerami","Sabit Rajah","Selendang Batik"}[i]);
                d.world.altarRelics[i]=Converted(new[]{"Doll","Sickle","Cloth"}[i],new Vector3(71,.73f,21+i*2),new Vector3(.35f,.25f,.35f),0,false);
            }
            var decoy=Converted("Sack",new Vector3(73,.7f,24),new Vector3(.7f,1.1f,.5f),180,false);Item(decoy,"DECOY","Damar? Periksa sosok yang terikat");d.world.decoy=Point("DecoyInteraction",new Vector3(72.8f,1.1f,24),decoy.transform);
            PH("industrial_coffee_table",new Vector3(73,0,24),new Vector3(.9f,.7f,.7f),0,true);
            d.world.beams=new Transform[3];d.world.ritualLights=new Light[3];d.world.levers=new Transform[3];
            Vector3[] lever={new Vector3(82,0,35),new Vector3(82,0,13),new Vector3(95,0,24)};
            Vector3[] beam={new Vector3(82,0,31),new Vector3(82,0,17),new Vector3(91,0,24)};
            for(int i=0;i<3;i++)
            {
                var go=PH("portable_generator",lever[i],new Vector3(1.4f,1,.8f));Item(go,"LAMP_"+i,"Generator lampu sorot G-"+new[]{"N / UTARA","S / SELATAN","E / TIMUR"}[i]);
                d.world.levers[i]=go.transform;d.world.beams[i]=Point("BeamCentre_"+i,beam[i]);
                PH("portable_searchlight",lever[i]+Vector3.up*1.2f,new Vector3(.7f,.6f,.6f),i==0?180:i==1?0:270,false);
                Vector3 direction=(beam[i]-lever[i]).normalized;
                var light=LightAt("RitualLight_"+i,lever[i]+Vector3.up*1.6f+direction*.35f,new Color(.86f,.94f,1),25,9,true);light.type=LightType.Spot;light.shadows=LightShadows.Soft;light.spotAngle=70;light.innerSpotAngle=50;light.transform.LookAt(beam[i]+Vector3.up*1.4f);light.enabled=false;d.world.ritualLights[i]=light;
                Label("GeneratorLabel_"+i,"G-"+new[]{"N / UTARA","S / SELATAN","E / TIMUR"}[i],lever[i]+Vector3.up*2,i==0?0:i==1?180:90,.19f);
            }
            Floor(68,73,11,12,0,"metal_grate_rusty");
            d.world.alcove=new Bounds(new Vector3(70.5f,1.5f,13),new Vector3(5,3,4));Wall(new Vector3(68,0,13),4,3,90);Wall(new Vector3(73,0,13),4,3,90);Wall(new Vector3(70.5f,0,11),5,3);
            d.world.serviceDoor=Gate("Alcove_ClosedDoor",new Vector3(70.5f,0,15),new Vector3(4.5f,2.8f,.25f));Item(d.world.serviceDoor,"ALCOVE_DOOR","Buka / tutup pintu alcove");
            var switcher=PH("utility_box_01",new Vector3(69,1,14.3f),new Vector3(.3f,.4f,.2f));Item(switcher,"ALCOVE_DOOR","Tutup / buka pintu alcove dari dalam");
            var reset=PH("vintage_hand_drill",new Vector3(69,.9f,12.3f),new Vector3(.5f,.5f,.4f));Item(reset,"ALCOVE_RESET","Reset jatah rak daya — tahan E 8 detik");
            int power=0;foreach(var at in new[]{new Vector3(71,.8f,12),new Vector3(70,.8f,33)})Item(PH("power_box_01",at,new Vector3(.6f,.5f,.4f)),"POWER_"+power++,"Rak daya manual +25 — tahan E");
            Item(PH("metal_toolbox",new Vector3(70,0,18),new Vector3(.7f,.4f,.5f)),"FLARE_RACK","Ambil satu flare cadangan ritual");
            Label("AlcoveInstructions","ALCOVE SERVIS\nRAK DAYA: TAHAN E 8s\nJATAH HABIS? TUTUP PINTU, ENGKOL RESET\nBUKAN GERBANG KELUAR",new Vector3(70.5f,2,14.5f),0,.13f);
            foreach(var point in arenaHoles)Tile(point,new Vector3(3,.12f,3),Phase3FloorKind.FragileFatal,true);
            d.world.clothKnot=Converted("Cloth",new Vector3(82,1.4f,32),new Vector3(.5f,.3f,.5f),0,false);foreach(var t in d.world.clothKnot.GetComponentsInChildren<Transform>())GameObjectUtility.SetStaticEditorFlags(t.gameObject,0);d.world.clothKnot.SetActive(false);
            var mist=Particles("Wewe_BlackMist",new Vector3(82,1.5f,32),new Color(.05f,.04f,.045f,.35f),1.1f,16);d.world.mist=mist.gameObject;
            var flare=Particles("FlareTemplate",Vector3.zero,new Color(1,.27f,.07f,.8f),.12f,20);var l=flare.gameObject.AddComponent<Light>();l.color=new Color(1,.33f,.12f);l.intensity=9;l.range=6;l.shadows=LightShadows.None;d.world.flareTemplate=flare.gameObject;d.world.flareTemplate.SetActive(false);
        }
        private static void Dressing()
        {
            // Each cluster follows its purpose: weighing/transport, machine maintenance, foreman's controls.
            Converted("MineCart",new Vector3(9,0,-7),new Vector3(1.8f,1.4f,3.1f),90);
            for(int i=0;i<5;i++)Prop("MillGear_"+i,"Converted/Models/Gear",new Vector3(5+i*3.1f,-7.5f,16+i%2*4),new Vector3(4,4,.65f),i*17,true,"PROP",new Vector3(90,0,0));
            PH("industrial_wall_lamp",new Vector3(12,-4,10),new Vector3(.4f,.5f,.4f),0,false);
            Converted("IndustrialBuilding",new Vector3(-24,-.2f,28),new Vector3(24,14,28),90,false);
            Converted("IndustrialBuilding",new Vector3(106,-.2f,55),new Vector3(30,18,24),180,false);
            Converted("Chimneys",new Vector3(-14,-.2f,46),new Vector3(14,34,12),20,false);
            for(int i=0;i<4;i++)Converted("Boiler",new Vector3(i%2==0?33:47,0,i<2?17:26),new Vector3(4,6,4),i*90);
            PH("overhead_crane",new Vector3(12,6.5f,17),new Vector3(20,2,3),0,false);
            PH("hand_truck",new Vector3(5,0,-7),new Vector3(.6f,1.3f,.8f),30);
            PH("wooden_barrels_01",new Vector3(2,0,-3),new Vector3(2,1.2f,1.2f),0);
            PH("wooden_crate_02",new Vector3(19,0,-4),new Vector3(1.5f,1.1f,1.2f),-12);
            PH("industrial_pastic_container",new Vector3(22,0,-7),new Vector3(1.4f,1.2f,1.2f),0);
            PH("old_military_compressor",new Vector3(22,0,15),new Vector3(1.5f,1.2f,1.4f),270);
            PH("drill_press_01",new Vector3(2,0,28),new Vector3(.8f,1.7f,1.1f));
            PH("bench_vice_01",new Vector3(1.8f,.9f,25),new Vector3(.5f,.45f,.5f));
            PH("steel_frame_shelves_01",new Vector3(26,0,39),new Vector3(1.6f,2.2f,.6f),90);
            PH("steel_frame_shelves_02",new Vector3(29,0,40),new Vector3(1.6f,2.2f,.6f),180);
            PH("pipe_wrench",new Vector3(20,.9f,26.5f),new Vector3(.1f,.07f,.45f),40,false);
            PH("water_manhole_cover",new Vector3(40,.02f,14),new Vector3(1,.04f,1),0,false);
            PH("utility_box_01",new Vector3(24.2f,1.2f,20),new Vector3(.5f,.8f,.2f),90);
            PH("oil_tin",new Vector3(30,0,39),new Vector3(.4f,.6f,.4f));
            PH("metal_jerrycan",new Vector3(27,0,37),new Vector3(.35f,.6f,.2f));
            PH("adjustable_wrench",new Vector3(54,5.05f,36),new Vector3(.1f,.06f,.3f),0,false);
            PH("cross_pein_hammer",new Vector3(53.5f,5.05f,36),new Vector3(.15f,.1f,.35f),-20,false);
            PH("pliers",new Vector3(54.3f,5.06f,36),new Vector3(.14f,.05f,.25f),10,false);
            PH("portable_welding_cart",new Vector3(38,0,15),new Vector3(1,1.4f,.7f),90);
            PH("rusted_spade_01",new Vector3(27.8f,0,39),new Vector3(.3f,1.3f,.2f),90);
            for(int i=0;i<9;i++)Converted("Sack",new Vector3(25.4f+i%3*1.25f,0,29.8f+i/3*1.4f),new Vector3(.9f,.8f,.7f),i*17);
            for(int i=0;i<9;i++)Converted("Sack",new Vector3(29+i%5*6,6.8f,14+i/5*12),new Vector3(.6f,1.5f,.55f),i*11,false);
            // Coherent header and drops, rather than the download's laid-out pipe catalogue.
            for(int x=30;x<60;x+=6)Module("modular_industrial_pipes_01","modular_industrial_pipes_01_pipe02",new Vector3(x,6.8f,12),new Vector3(6,.32f,.32f),0,false,new Vector3(0,0,90));
            foreach(var at in new[]{new Vector3(33,5.5f,17),new Vector3(47,5.5f,17),new Vector3(33,5.5f,26),new Vector3(47,5.5f,26)})Module("modular_industrial_pipes_01","modular_industrial_pipes_01_pipe02",at,new Vector3(.32f,1.5f,.32f),0,false);
            // Fixtures physically coincide with their light sources.
            PH("hanging_industrial_lamp",new Vector3(12,8,12),new Vector3(1.1f,1.2f,1.1f),0,false);
            PH("caged_hanging_light",new Vector3(40,7,24),new Vector3(.6f,.8f,.6f),0,false);
            PH("industrial_caged_sconce",new Vector3(47,6.2f,36),new Vector3(.25f,.5f,.25f),180,false);
            PH("industrial_wall_lamp",new Vector3(25,3.5f,34),new Vector3(.4f,.5f,.4f),90,false);
            for(int i=0;i<6;i++)Module("modular_industrial_pipes_01","modular_industrial_pipes_01_pipe02",new Vector3(5+i*3,4.5f,30),new Vector3(.3f,3,.3f),0,false);
            foreach(var at in new[]{new Vector3(43,0,31),new Vector3(59,0,31),new Vector3(43,0,46),new Vector3(61,0,46),new Vector3(69,0,16),new Vector3(69,0,32),new Vector3(95,0,16),new Vector3(95,0,32)})
                Module("modular_industrial_pipes_01","modular_industrial_pipes_01_pipe02",at,new Vector3(.3f,4,.3f),0,true);
            // Continuous downloaded handrail assemblies keep the safe catwalk edges legible.
            for(int z=7;z<28;z+=4)Module("modular_fire_escape","modular_fire_escape_platform_railing",new Vector3(15.1f,0,z),new Vector3(.15f,1,3.8f),90,true);
            for(int x=44;x<60;x+=4)Module("modular_fire_escape","modular_fire_escape_platform_railing",new Vector3(x,4,47.8f),new Vector3(3.8f,1,.15f),0,true);
            Module("modular_fire_escape","modular_fire_escape_platform_railing",new Vector3(62,4,47.8f),new Vector3(3.8f,1.2f,.15f),0,true);
            for(int x=24;x<96;x+=6)Module("overhead_crane","overhead_crane_rails",new Vector3(x,.015f,-2.5f),new Vector3(6,.06f,1.4f),0,false);
            Label("Maintenance1999","CATATAN PERAWATAN\nGILINGAN DIHENTIKAN — 1999\nJANGAN INJAK PAPAN RETAK",new Vector3(20,2,31.5f),0,.18f);
        }
        private static void Actor()
        {
            d.world.wewe=Prop("WeweGombel_ArtistRig","Converted/Models/Wewe",new Vector3(38,6,24),new Vector3(1.7f,2.95f,1.2f),0,false,"CHARACTER");
            var agent=d.world.wewe.AddComponent<NavMeshAgent>();agent.radius=.35f;agent.height=2.9f;agent.speed=d.balance.patrolSpeed;agent.angularSpeed=220;agent.acceleration=8;agent.stoppingDistance=.35f;agent.enabled=false;d.wewe.agent=agent;
            var body=d.world.wewe.AddComponent<CapsuleCollider>();body.height=2.9f;body.radius=.38f;body.center=Vector3.up*1.45f;
            foreach(var t in d.world.wewe.GetComponentsInChildren<Transform>())GameObjectUtility.SetStaticEditorFlags(t.gameObject,0);
            d.world.mist.transform.SetParent(d.world.wewe.transform,true);d.world.mist.transform.localPosition=Vector3.up*1.3f;
            d.world.patrol=new[]{new Vector3(39,.04f,18),new Vector3(55,.04f,18),new Vector3(55,.04f,29),new Vector3(37,.04f,35),new Vector3(27,.04f,25)}.Select((p,i)=>Point("WewePatrol_"+i,p)).ToArray();
            d.world.roof=new[]{new Vector3(38,6,18),new Vector3(53,6,18),new Vector3(53,7,28),new Vector3(38,7,28)}.Select((p,i)=>Point("WeweTruss_"+i,p)).ToArray();
        }
        private static void Rail(Vector3 centre,float length,float yaw)
        {
            for(float x=-length/2;x<length/2-.01f;x+=3)
            {
                float span=Mathf.Min(3,length/2-x);Vector3 at=centre+Quaternion.Euler(0,yaw,0)*Vector3.right*(x+span/2);
                var rail=Module("modular_fire_escape","modular_fire_escape_platform_railing",at,new Vector3(span,1,.12f),yaw,false);
                var c=rail.AddComponent<BoxCollider>();c.size=new Vector3(span,1,.12f);c.center=Vector3.up*.5f;
            }
        }
        private static ParticleSystem Particles(string name,Vector3 at,Color colour,float size,float rate)
        {
            var go=Point(name,at).gameObject;var ps=go.AddComponent<ParticleSystem>();var main=ps.main;main.startLifetime=2.5f;main.startSpeed=.15f;main.startSize=size;main.maxParticles=72;main.startColor=colour;main.simulationSpace=ParticleSystemSimulationSpace.Local;
            var emission=ps.emission;emission.rateOverTime=rate;var shape=ps.shape;shape.shapeType=ParticleSystemShapeType.Sphere;shape.radius=.6f;
            go.GetComponent<ParticleSystemRenderer>().sharedMaterial=ParticleMat(name.Contains("Flare")?"FlareSpark":"SteamMist",name.Contains("Flare")?"Spark.png":"Steam.png",Color.white);return ps;
        }
        private static Light LightAt(string name,Vector3 p,Color colour,float intensity,float range,bool shadow=false)
        {
            var go=Point(name,p).gameObject;var l=go.AddComponent<Light>();l.type=LightType.Point;l.color=colour;l.intensity=intensity;l.range=range;l.shadows=LightShadows.None;l.shadowBias=.03f;l.shadowNormalBias=.2f;return l;
        }
        private static void Lighting()
        {
            RenderSettings.ambientMode=AmbientMode.Trilight;RenderSettings.ambientSkyColor=new Color(.22f,.26f,.32f);RenderSettings.ambientEquatorColor=new Color(.13f,.16f,.19f);RenderSettings.ambientGroundColor=new Color(.075f,.064f,.055f);
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=.009f;RenderSettings.fogColor=new Color(.018f,.025f,.035f);
            var moon=LightAt("Moon",new Vector3(20,22,10),new Color(.47f,.59f,.78f),.65f,100,true);moon.type=LightType.Directional;moon.shadows=LightShadows.Soft;moon.transform.rotation=Quaternion.Euler(48,-38,0);
            LightAt("Weighbridge_Practical",new Vector3(3,4,-8),new Color(1,.7f,.37f),8,18);
            LightAt("Mill_Hanging",new Vector3(12,7.7f,12),new Color(.83f,.71f,.47f),10,19,true);
            LightAt("Mill_Pit_Practical",new Vector3(12,-3.8f,10),new Color(.66f,.55f,.36f),5,12);
            LightAt("Boiler_Hanging",new Vector3(40,6.7f,24),new Color(.64f,.76f,.81f),14,22,true);
            LightAt("Mandor_Practical",new Vector3(47,6,36),new Color(1,.73f,.4f),9,12);
            LightAt("Bagasse_Practical",new Vector3(25,3.4f,34),new Color(.6f,.65f,.42f),6,10);
            var fireLight=LightAt("Furnace_Embers",new Vector3(82,3,24),new Color(1,.29f,.07f),24,22,true);fireLight.type=LightType.Spot;fireLight.transform.rotation=Quaternion.Euler(90,0,0);fireLight.spotAngle=140;fireLight.innerSpotAngle=100;fireLight.shadows=LightShadows.Soft;
            LightAt("Arena_North_Practical",new Vector3(82,5,35),new Color(.5f,.65f,.79f),8,20);
            LightAt("Arena_South_Practical",new Vector3(82,5,13),new Color(.9f,.6f,.3f),8,20);
            LightAt("Service_Door_Practical",new Vector3(96,3,24),new Color(.45f,.67f,.5f),4,10);
            LightAt("Alcove_Service_Practical",new Vector3(70.5f,2.5f,13),new Color(.91f,.72f,.42f),4,6);
            Particles("Steam_BoilerA",new Vector3(33,5,17),new Color(.4f,.45f,.48f,.12f),1.5f,4);
            Particles("Steam_BoilerB",new Vector3(47,5,26),new Color(.4f,.45f,.48f,.12f),1.5f,4);
            PH("barrel_stove",new Vector3(82,-4,20.8f),new Vector3(2.2f,4,2),0,false);
            Particles("Flare_FurnaceEmbers",new Vector3(82,-.4f,20.2f),new Color(1,.23f,.04f,.75f),.14f,12);
            string path=Art+"Phase3Volume.asset";var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if(!profile){profile=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(profile,path);}
            profile.components.RemoveAll(component=>!component);
            if(!profile.TryGet<ColorAdjustments>(out var colour))colour=profile.Add<ColorAdjustments>(true);colour.contrast.Override(8);colour.saturation.Override(-12);colour.postExposure.Override(.55f);
            if(!profile.TryGet<Tonemapping>(out var tonemap))tonemap=profile.Add<Tonemapping>(true);tonemap.mode.Override(TonemappingMode.ACES);
            if(!profile.TryGet<Bloom>(out var bloom))bloom=profile.Add<Bloom>(true);bloom.intensity.Override(.2f);bloom.threshold.Override(1.2f);
            if(!profile.TryGet<Vignette>(out var vignette))vignette=profile.Add<Vignette>(true);vignette.intensity.Override(.22f);vignette.smoothness.Override(.7f);
            foreach(var component in profile.components)
            {if(!AssetDatabase.Contains(component))AssetDatabase.AddObjectToAsset(component,profile);EditorUtility.SetDirty(component);}
            EditorUtility.SetDirty(profile);
            var volume=env.gameObject.AddComponent<Volume>();volume.isGlobal=true;volume.sharedProfile=profile;
            string skyPath=Art+"Materials/FactoryNightSky.mat";var sky=AssetDatabase.LoadAssetAtPath<Material>(skyPath);
            if(!sky){sky=new Material(Shader.Find("Skybox/Panoramic"));AssetDatabase.CreateAsset(sky,skyPath);}
            sky.SetTexture("_MainTex",AssetDatabase.LoadAssetAtPath<Texture>(Imported+"PolyHaven/moonless_golf/moonless_golf.hdr"));sky.SetFloat("_Exposure",.35f);sky.SetFloat("_Rotation",130);
            EditorUtility.SetDirty(sky);
            RenderSettings.skybox=sky;RenderSettings.defaultReflectionMode=DefaultReflectionMode.Skybox;RenderSettings.reflectionIntensity=.35f;
            d.phoneView.playerCamera.backgroundColor=RenderSettings.fogColor;d.phoneView.playerCamera.clearFlags=CameraClearFlags.Skybox;
        }
    }
}
#endif
