#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static partial class Phase1SceneBuilder
    {
        public const string ScenePath = "Assets/_LayarTerakhir/assets/Scenes/Babak1_Rusunawa.unity";
        private const string Art = "Assets/_LayarTerakhir/assets/Art/Phase1/";
        private static Phase1Director d;
        private static Transform environment, current;
        private static Material concrete, plaster, soot, tiles, rust, darkMetal, timber, paper, glass, emissive, moss;
        private static readonly List<Phase1Anchor> anchors = new List<Phase1Anchor>();
        private static readonly List<Transform> pathNodes = new List<Transform>();
        private static readonly Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>();

        [MenuItem("LayarTerakhir/Phase 1/Build Rusunawa")]
        public static void Build()
        {
            Directory.CreateDirectory(Art + "Materials"); Directory.CreateDirectory(Art + "Meshes");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            PrepareExistingPhoneGrasp();
            var kos = EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var sourcePlayer = Object.FindFirstObjectByType<FirstPersonController>();
            var sourceSystems = GameObject.Find("GameSystems");
            var events = Object.FindFirstObjectByType<EventSystem>();
            var volumeSource = Object.FindFirstObjectByType<Volume>();
            var sourceProfile = volumeSource ? volumeSource.sharedProfile : null;
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive); SceneManager.SetActiveScene(scene);
            var systems = Object.Instantiate(sourceSystems); systems.name = "RusunawaGameSystems";
            Object.DestroyImmediate(systems.GetComponent<KamarKosPrologDirector>());
            var kosAudio = systems.GetComponentInChildren<KosSceneAudio>(); if (kosAudio) Object.DestroyImmediate(kosAudio.gameObject);
            var player = Object.Instantiate(sourcePlayer.gameObject); player.name = "Player_RakaWijaya";
            if (events && !events.transform.IsChildOf(sourceSystems.transform)) Object.Instantiate(events.gameObject).name = "EventSystem";
            EditorSceneManager.CloseScene(kos, true);
            d = systems.AddComponent<Phase1Director>(); d.player = player.GetComponent<FirstPersonController>();
            d.world = systems.AddComponent<Phase1World>();
            var signs=systems.AddComponent<AtmajaWorldText>();
            signs.includeARText=true;
            signs.materialTemplate=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/CompletionWorldText.mat");
            string balancePath = Art + "Phase1Balance.asset";
            d.balance = AssetDatabase.LoadAssetAtPath<Phase1Balance>(balancePath);
            if(d.balance&&!MonoScript.FromScriptableObject(d.balance))
            {AssetDatabase.DeleteAsset(balancePath);d.balance=null;}
            if (!d.balance) { d.balance = ScriptableObject.CreateInstance<Phase1Balance>(); AssetDatabase.CreateAsset(d.balance, balancePath); }
            d.threats = systems.AddComponent<Phase1Threats>(); d.threats.director = d;
            d.soundscape = systems.AddComponent<Phase1Soundscape>(); d.soundscape.director = d;
            var presentation=systems.AddComponent<Phase1LightBudget>();presentation.player=player.transform;
            d.soundscape.routing = AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");
            d.soundscape.sounds = Sounds();
            d.phoneView = systems.AddComponent<AtmajaPhoneView>(); d.phoneView.playerCamera = player.GetComponent<InteractionInteractor>().playerCamera;
            d.phoneView.feedMaterial = PresentationProfile.Load().phoneFeedMaterial;
            var camera = d.phoneView.playerCamera; camera.farClipPlane = 115; camera.backgroundColor = new Color(.012f, .018f, .026f);camera.clearFlags=CameraClearFlags.SolidColor;
            environment = new GameObject("Rusunawa_GriyaAsri_Environment").transform;
            current = environment; anchors.Clear(); pathNodes.Clear(); meshes.Clear(); Materials(); ImportExternalSurfaces();
            Exterior();
            for (int floor = 0; floor < 4; floor++) Floor(floor);
            StoryObjects(); EnvironmentDressing(); AdditionalDressing(); Threats(); Lighting(sourceProfile);
            d.world.anchors = anchors.ToArray(); d.world.patrolPoints = pathNodes.ToArray();
            if(!MonoScript.FromScriptableObject(d.balance))throw new InvalidOperationException("Phase1Balance has no serializable Unity script");
            foreach(var behaviour in scene.GetRootGameObjects().SelectMany(go=>go.GetComponentsInChildren<MonoBehaviour>(true)))
                if(!behaviour||!MonoScript.FromMonoBehaviour(behaviour))throw new InvalidOperationException("Missing Unity script: "+(behaviour?behaviour.GetType().FullName:"null component"));
            ValidateImportedScene(scene);
            player.transform.SetPositionAndRotation(d.world.Anchor("ARRIVAL").position, d.world.Anchor("ARRIVAL").rotation);
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(s => s.path != ScenePath)
                .Concat(new[] { new EditorBuildSettingsScene(ScenePath, true) }).ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("PHASE1_SCENE_BUILT " + ScenePath + " route=L1/L2/L3/L4/return, original prolog scenes preserved");
        }
        public static void BuildLinux()
        {Build();GoldenLinuxBuild.Build();}
        private static void Materials()
        {
            EnvironmentMaterials();
        }

        private static Material Plain(string name, Color color, float smooth = .18f, float metal = 0)
        {
            string path = Art + "Materials/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!material) { material = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(material, path); }
            material.SetColor("_BaseColor", color); material.SetFloat("_Smoothness", smooth); material.SetFloat("_Metallic", metal); material.enableInstancing = true;
            EditorUtility.SetDirty(material); return material;
        }
        private static Transform Group(string name)
        { var go = new GameObject(name); go.transform.SetParent(environment, false); return go.transform; }
        private static GameObject Box(string name, Vector3 position, Vector3 size, Material mat, bool collider = true, Transform parent = null)
        {
            if(ModularSurface(name,size,mat,parent))return ModularBox(name,position,size,mat,collider,parent?parent:current);
            return MeshInstance(name, ImportedMesh(Imported + "ConcreteInterior/Cube.fbx"), position, size, mat, collider, parent ? parent : current);
        }
        private static Transform Anchor(string id, Vector3 position, Vector3 forward)
        {
            var go = new GameObject("Anchor_" + id); go.transform.SetParent(environment, false); go.transform.position = position;
            go.transform.rotation = Quaternion.LookRotation(forward); anchors.Add(new Phase1Anchor { id = id, point = go.transform }); return go.transform;
        }
        private static void Node(Vector3 p)
        { var go = new GameObject("PatrolNode_" + pathNodes.Count); go.transform.SetParent(environment, false); go.transform.position = p; pathNodes.Add(go.transform); }
        private static Phase1WorldItem Item(GameObject go, string id, string prompt)
        { var item = go.AddComponent<Phase1WorldItem>(); item.director = d; item.id = id; item.prompt = prompt; return item; }
        private static void Zone(string id, Vector3 p, Vector3 size)
        { var go = new GameObject("Zone_" + id, typeof(BoxCollider), typeof(Phase1Zone)); go.transform.SetParent(environment, false); go.transform.position = p; var c = go.GetComponent<BoxCollider>(); c.isTrigger = true; c.size = size; var zone = go.GetComponent<Phase1Zone>(); zone.id = id; zone.director = d; }
        private static void Label(string name, string text, Vector3 p, Quaternion rotation, float size = .12f, Color? color = null, Transform parent = null)
        {
            var go = new GameObject(name, typeof(TextMesh)); go.transform.SetParent(parent ? parent : current, false); go.transform.SetPositionAndRotation(p, rotation*Quaternion.Euler(0,180,0));
            var mesh = go.GetComponent<TextMesh>(); mesh.text = text; mesh.font = PresentationProfile.Load().bodyFont; mesh.fontSize = 64; mesh.characterSize = size*.34f; mesh.anchor = TextAnchor.MiddleCenter; mesh.alignment = TextAlignment.Center;
            mesh.color = color ?? new Color(.72f, .74f, .66f); mesh.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/CompletionWorldText.mat");
        }
        private static void Pipe(string name, Vector3 a, Vector3 b, float radius, Material mat)
        {
            var go = MeshInstance(name, ImportedMesh(Imported + "ConcreteInterior/Pillar.fbx"), (a+b)*.5f,
                new Vector3(radius*2,(b-a).magnitude,radius*2), mat, false, current);
            go.transform.rotation=Quaternion.FromToRotation(Vector3.up,b-a);
        }
        private static void Exterior()
        {
            current = Group("Parking_PumpAndGuard");
            Box("WetParkingConcrete", new Vector3(0, -.2f, -5), new Vector3(82, .4f, 72), parkingSurface);
            for (int side = -1; side <= 1; side += 2)
                Box("NeighbourBoundaryWall", new Vector3(side * 37, 1.6f, -5), new Vector3(.3f, 3.2f, 70), plaster);
            Box("StreetBoundary", new Vector3(0, 1.6f, -40), new Vector3(74, 3.2f, .3f), plaster);
            Box("BackCompoundWall", new Vector3(0, 1.6f, 29), new Vector3(74, 3.2f, .3f), plaster);
            Box("RusunawaNameBoard", new Vector3(4, 2.4f, -22), new Vector3(9, 1.7f, .16f), soot);
            Label("RusunawaName", "RUSUNAWA\nGRIYA ASRI", new Vector3(4, 2.5f, -22.09f), Quaternion.Euler(0, 180, 0), .24f);
            // Display an existing CC0 image on Unity's stock decal plane. No painted/generated artwork.
            var blood=GameObject.CreatePrimitive(PrimitiveType.Quad);blood.name="ExistingCC0SignBlood";blood.transform.SetParent(current,false);
            blood.transform.position=new Vector3(6.7f,2.4f,-22.1f);blood.transform.localScale=new Vector3(2.5f,1.6f,1);
            blood.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(Art+"Materials/External_ExileBlood.mat");Object.DestroyImmediate(blood.GetComponent<Collider>());
            Anchor("SIGNBOARD", new Vector3(4, 2.4f, -22.1f), Vector3.back);
            var echo=new GameObject("FourteenVictims_ExistingSourceSilhouettes");echo.transform.SetParent(current,false);d.world.victimEcho=echo;
            var echoMaterial=Plain("ExistingVictimSilhouette",new Color(.035f,.06f,.055f),.02f);
            for(int i=0;i<14;i++)
            {
                string source=i%2==0?"Assets/_LayarTerakhir/assets/Art/Characters/Models/SriAtmajaEcho.fbx":"Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/NyonyaDesignerEcho.fbx";
                var figure=SourceProp("ExistingEcho","Victim_"+(i+1),new Vector3(-.8f+i%7*1.55f,.85f,-24.5f+i/7*.8f),1.5f+(i%3)*.1f,false,null,echoMaterial,source,echo.transform);
                foreach(var node in figure.GetComponentsInChildren<Transform>(true))node.gameObject.layer=AtmajaPhoneView.ARLayer;
            }
            echo.SetActive(false);
            Anchor("ARRIVAL", new Vector3(1, .06f, -30), Vector3.forward);
            Anchor("PARKING", new Vector3(1, .06f, -30), Vector3.forward);
            var scooter = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx");
            var motor = (GameObject)PrefabUtility.InstantiatePrefab(scooter); motor.name = "RakaMotor_WithDamarHandycam"; motor.transform.SetParent(current, false); motor.transform.position = new Vector3(2, 0, -30);
            AssignImportedMaterials(motor);
            var motorHit = Box("MotorInteraction", new Vector3(2, .85f, -30), new Vector3(1, 1.6f, 2), darkMetal); motorHit.GetComponent<Renderer>().enabled = false; Item(motorHit, "MOTOR", "Periksa motor / tinggalkan Rusunawa");
            // Guard shelter has an actual roof, broken glazing and a lamp above the log.
            Box("GuardRearWall", new Vector3(-8, 1.5f, -21.3f), new Vector3(5, 3, .25f), plaster);
            Box("GuardSideWall", new Vector3(-10.5f, 1.5f, -24), new Vector3(.25f, 3, 5.5f), plaster);
            Box("GuardRoof", new Vector3(-8, 3.2f, -24), new Vector3(5.6f, .22f, 6), concrete);
            var counter = Box("GuardCounter", new Vector3(-8, .85f, -23), new Vector3(2.4f, .12f, .8f), timber);
            var counterBase = Box("GuardCounterBase", new Vector3(-8, .4f, -23.1f), new Vector3(2.2f, .8f, .5f), darkMetal);
            Object.DestroyImmediate(counter);Object.DestroyImmediate(counterBase);
            var log = Box("GuardDutyBook2004", new Vector3(-8, .96f, -23), new Vector3(.45f, .06f, .32f), paper);
            Item(log, "GUARD_LOG", "Baca laporan piket satpam (2004)");
            Fixture("GuardLamp", new Vector3(-8, 2.8f, -22.3f), new Color(1, .68f, .35f), 1.8f, 6, true);
            // The welded lobby is visibly a physical obstruction, not a district forcefield.
            for (int i = -5; i <= 5; i++) Pipe("WeldedLobbyBar", new Vector3(i * .18f, .1f, -21), new Vector3(i * .18f, 2.7f, -21), .022f, rust);
            Box("WeldedLobbyGate", new Vector3(0, 1.4f, -21), new Vector3(2.2f, 2.8f, .14f), darkMetal).GetComponent<Renderer>().enabled = false;
            Label("WeldWarning", "LOBI DILAS\nAKSES SERVIS DI BELAKANG", new Vector3(0, 1.6f, -21.13f), Quaternion.Euler(0, 180, 0), .11f);
            Box("PumpBack", new Vector3(-28, 1.6f, -8), new Vector3(7, 3.2f, .3f), plaster);
            Box("PumpNorth", new Vector3(-31.5f, 1.6f, -11.5f), new Vector3(.3f, 3.2f, 7), plaster);
            Box("PumpRoof", new Vector3(-28, 3.3f, -11.5f), new Vector3(7.5f, .25f, 7.5f), concrete);
            Label("PumpSign", "RUANG POMPA\nAKSES PEMELIHARAAN", new Vector3(-28, 2.5f, -15.2f), Quaternion.Euler(0, 180, 0), .13f);
            for (int i = 0; i < 3; i++)
            {
                Pipe("PumpRiser", new Vector3(-30 + i * .7f, .1f, -9), new Vector3(-30 + i * .7f, 2.8f, -9), .09f, rust);
                Pipe("PumpHeader", new Vector3(-30 + i * .7f, .8f, -9), new Vector3(-26, .8f, -9), .06f, rust);
            }
            Dynamo("DYNAMO_L1", new Vector3(-27, .85f, -8.8f));
            Fixture("PumpBulkhead", new Vector3(-28, 2.8f, -9), new Color(.55f, .78f, .67f), 1.4f, 6, false);
            Zone("PUMP", new Vector3(-25, 1.2f, -11), new Vector3(2, 2.5f, 4));
            Anchor("PUMP", new Vector3(-26, .06f, -11), Vector3.right);
            for (int i = 0; i < 8; i++)
            {
                float x = -21 + i * 5.5f;
                Box("ParkingPaintLine", new Vector3(x, .012f, -32), new Vector3(.07f, .01f, 4), paper, false);
            }
            // Downloaded weathered frames and broken glazing replace the stylized palette kit.
            for(int floor=0;floor<4;floor++)for(int unit=0;unit<8;unit++)
            {
                float x=-21+unit*6;
                SourceProp("DoorsWindows","WeatheredFacadeWindow",new Vector3(x,floor*4+1.8f,-20.32f),1.45f,false,
                    new Vector3(1.8f,1.45f,.24f),null,Imported+"DoorsWindows/"+((unit+floor)%4==0?"Windows_BrokenWindow_1":"Windows_Window_1")+".fbx");
            }
        }

        private static void Floor(int floor)
        {
            float y = floor * 4;
            current = Group("L" + (floor + 1) + "_ConcreteRoomsAndLoop");
            Material wall = floor == 3 ? soot : plaster;
            if (floor == 2)
            {
                // The investigation route is a sealed 40m corridor. The rest of
                // the physical storey still supports L4 and encloses the courtyard.
                Box("L3_NorthStructuralFloor",new Vector3(0,y-.12f,15),new Vector3(48,.24f,10),concrete);
                foreach(int side in new[]{-1,1})
                {
                    Box("L3_SideStructuralFloor",new Vector3(side*20.5f,y-.12f,0),new Vector3(7,.24f,20),concrete);
                    Box("L3_SideEnvelope",new Vector3(side*24.1f,y+1.8f,0),new Vector3(.25f,3.6f,40),wall);
                    Box("L3_SideGalleryParapet",new Vector3(side*13,y+.5f,0),new Vector3(.18f,1,18),wall);
                    for(int pier=0;pier<5;pier++)Box("L3_SidePier",new Vector3(side*13,y+1.8f,-8+pier*4),new Vector3(.26f,3.6f,.28f),concrete);
                }
                Box("L3_NorthEnvelope",new Vector3(0,y+1.8f,20.1f),new Vector3(48,3.6f,.25f),wall);
                Box("L3_NorthRoomFacade",new Vector3(0,y+1.8f,13),new Vector3(34,3.6f,.22f),wall);
                Box("L3_NorthParapet",new Vector3(0,y+.5f,9),new Vector3(26,1,.18f),wall);
                for(int pier=0;pier<7;pier++)Box("L3_NorthPier",new Vector3(-12+pier*4,y+1.8f,9.2f),new Vector3(.26f,3.6f,.28f),concrete);
                Box("L3_SouthApartmentFacade",new Vector3(0,y+1.8f,-20.1f),new Vector3(48,3.6f,.25f),wall);
                Box("L3_40mCorridorFloor", new Vector3(0, y - .12f, -11), new Vector3(42, .24f, 4), tiles);
                Box("L3_NorthWall", new Vector3(-1.5f, y + 1.8f, -9), new Vector3(39, 3.6f, .22f), wall);
                Box("L3_EastEndWall", new Vector3(21, y + 1.7f, -11), new Vector3(.25f, 3.4f, 4), wall);
                Box("L3_SouthWall", new Vector3(0, y + 1.8f, -13), new Vector3(42, 3.6f, .22f), wall);
                Box("L3_Ceiling", new Vector3(0, y + 3.65f, -11), new Vector3(42, .25f, 4.2f), concrete);
                Box("L3_WestShaftWall", new Vector3(-21, y + 1.7f, -11), new Vector3(.25f, 3.4f, 4), concrete);
                for (int i = 0; i < 8; i++) DoorFacade(301 + i, new Vector3(-17 + i * 5, y, -12.85f), false, false);
                for (int i = 0; i < 6; i++) Fixture("L3FailingFluorescent", new Vector3(-17 + i * 6.5f, y + 3.35f, -11), new Color(.6f, .75f, .70f), .8f, 5, i == 3);
                return;
            }
            Box("NorthGalleryFloor", new Vector3(0, y - .12f, 11), new Vector3(34, .24f, 4), tiles);
            Box("SouthGalleryFloor", new Vector3(0, y - .12f, -11), new Vector3(34, .24f, 4), tiles);
            Box("WestGalleryFloor", new Vector3(-15, y - .12f, 0), new Vector3(4, .24f, 18), tiles);
            Box("EastGalleryFloor", new Vector3(15, y - .12f, 0), new Vector3(4, .24f, 18), tiles);
            Box("NorthRoomsFloor", new Vector3(0, y - .12f, 16.5f), new Vector3(48, .24f, 7), concrete);
            // The hidden L1→L2 stair occupies a real slab opening, not a slab you walk through.
            Box("WestRoomsFloorA", new Vector3(-20.5f, y - .12f, -7.5f), new Vector3(7, .24f, 11), concrete);
            Box("WestRoomsFloorB", new Vector3(-20.5f, y - .12f, 10), new Vector3(7, .24f, 6), concrete);
            if (floor != 1) Box("WestRoomsMiddleFloor", new Vector3(-20.5f, y - .12f, 2), new Vector3(7, .24f, 8), concrete);
            if(floor==3)
            {
                Box("EastRoomsFloorNorth", new Vector3(20.5f, y-.12f, 7), new Vector3(7,.24f,12),concrete);
                Box("StairTopLanding",new Vector3(20.5f,y-.12f,-.5f),new Vector3(7,.24f,3),concrete);
                Box("EastRoomsFloorSouth",new Vector3(20.5f,y-.12f,-11.5f),new Vector3(7,.24f,3),concrete);
            }
            else Box("EastRoomsFloor", new Vector3(20.5f, y - .12f, 0), new Vector3(7, .24f, 26), concrete);
            Box("SouthRoomsFloor", new Vector3(0, y - .12f, -16.5f), new Vector3(48, .24f, 7), concrete);
            foreach (int side in new[] { -1, 1 })
            {
                if(floor==0&&side==-1) Box("OuterFacadePumpOpening",new Vector3(-24.1f,y+1.8f,5.5f),new Vector3(.25f,3.6f,29),wall);
                else Box("OuterFacade", new Vector3(side * 24.1f, y + 1.8f, 3.5f), new Vector3(.25f, 3.6f, 33), wall);
                Box("NorthOuterWall", new Vector3(0, y + 1.8f, 20.1f), new Vector3(48, 3.6f, .25f), wall);
                Box("SouthOuterWall", new Vector3(side * 12.5f, y + 1.8f, -20.1f), new Vector3(23, 3.6f, .25f), wall);
            }
            // Gallery ceilings are separate from upper walking slabs, leaving the courtyard open.
            Box("NorthGalleryCeiling", new Vector3(0, y + 3.65f, 15), new Vector3(48, .25f, 10), concrete);
            Box("SouthGalleryCeiling", new Vector3(0, y + 3.65f, -15), new Vector3(48, .25f, 10), concrete);
            if(floor==0)
            {
                Box("WestCeilingSideA",new Vector3(-23.5f,y+3.65f,0),new Vector3(3,.25f,20),concrete);
                Box("WestCeilingSideB",new Vector3(-16.5f,y+3.65f,0),new Vector3(3,.25f,20),concrete);
                Box("WestCeilingSouth",new Vector3(-20,y+3.65f,-5.5f),new Vector3(4,.25f,9),concrete);
                Box("WestCeilingNorth",new Vector3(-20,y+3.65f,8.5f),new Vector3(4,.25f,3),concrete);
            }
            else Box("WestGalleryCeiling", new Vector3(-20, y + 3.65f, 0), new Vector3(10, .25f, 20), concrete);
            Box("EastGalleryCeiling", new Vector3(20, y + 3.65f, 0), new Vector3(10, .25f, 20), concrete);
            // Inner balustrades show the O plan; collision follows the visible concrete.
            foreach (int side in new[] { -1, 1 })
            {
                Box("VoidNorthParapet", new Vector3(0, y + .5f, side * 9), new Vector3(26, 1, .18f), wall);
                Box("VoidSideParapet", new Vector3(side * 13, y + .5f, 0), new Vector3(.18f, 1, 18), wall);
                for (int i = 0; i < 7; i++)
                    Box("GalleryConcretePier", new Vector3(-12 + i * 4, y + 1.8f, side * 9.2f), new Vector3(.26f, 3.6f, .28f), concrete);
            }
            DoorWall(new Vector3(0, y, 13), 34, true, floor == 3 ? new[] { -10f, -2f, 7f } : new[] { -12f, -5f, 3f, 11f }, wall);
            DoorWall(new Vector3(0, y, -13), 34, true, floor == 3 ? new[] { 0f } : new[] { -12f, -5f, 3f, 11f }, wall);
            DoorWall(new Vector3(-17, y, 0), 26, false, new[] { -11f, 2f, 8f }, wall);
            DoorWall(new Vector3(17, y, 0), 26, false, new[] { 4f }, wall);
            int number = floor * 100 + 101;
            foreach (float x in floor == 3 ? new[] { -10f, -2f, 7f } : new[] { -12f, -5f, 3f, 11f })
            {
                bool open = floor == 3;
                DoorFacade(number++, new Vector3(x, y, 13.08f), true, open);
            }
            for (int side = -1; side <= 1; side += 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    float x = -12 + i * 8;
                    Fixture("GalleryBulkhead", new Vector3(x, y + 3.2f, side * 11), side < 0 ? new Color(.55f, .73f, .72f) : new Color(.9f, .66f, .38f), floor == 3 ? 1.1f : .85f, 5.6f, floor == 3 && i == 0);
                }
                Pipe("FireWaterMain", new Vector3(side * 16.7f, y + 2.7f, -12), new Vector3(side * 16.7f, y + 2.7f, 12), .055f, rust);
            }
            Label("FloorLandmark", "LANTAI " + (floor + 1), new Vector3(-15, y + 2.25f, 12.84f), Quaternion.Euler(0, 180, 0), .21f);
            if (floor == 3)
            {
                foreach (var p in new[] { new Vector3(-15, y, -11), new Vector3(0, y, -11), new Vector3(15, y, -11), new Vector3(15, y, 0), new Vector3(15, y, 11), new Vector3(0, y, 11), new Vector3(-15, y, 11), new Vector3(-15, y, 0) }) Node(p);
                foreach (var p in new[] { new Vector3(-10, y, 16), new Vector3(-2, y, 16), new Vector3(7, y, 16), new Vector3(-20, y, 2), new Vector3(20, y, 4), new Vector3(0, y, -17), new Vector3(-7, y, -17), new Vector3(7, y, -17) }) Node(p);
                foreach(float x in new[]{-10f,-2f,7f}){Node(new Vector3(x,y,11));Node(new Vector3(x,y,14));}
                foreach(float x in new[]{-16f,-18f})Node(new Vector3(x,y,2));
                foreach(float x in new[]{16f,18f})Node(new Vector3(x,y,4));
                Node(new Vector3(0,y,-12));Node(new Vector3(0,y,-14));
            }
        }
        private static void DoorWall(Vector3 centre, float span, bool alongX, float[] gaps, Material mat)
        {
            // gaps use world x/z. Keep room shells closed except their actual openings.
            float cursor = -span * .5f;
            foreach (float gap in gaps.OrderBy(v => v))
            {
                float left = gap - 1.05f;
                if (left > cursor)
                {
                    var p = centre + (alongX ? Vector3.right : Vector3.forward) * ((cursor + left) * .5f);
                    Box("ApartmentWallPanel", p + Vector3.up * 1.8f, alongX ? new Vector3(left - cursor, 3.6f, .22f) : new Vector3(.22f, 3.6f, left - cursor), mat);
                }
                var lintel = centre + (alongX ? Vector3.right : Vector3.forward) * gap;
                Box("DoorLintel", lintel + Vector3.up * 3.1f, alongX ? new Vector3(2.1f, 1, .22f) : new Vector3(.22f, 1, 2.1f), mat);
                cursor = gap + 1.05f;
            }
            if (cursor < span * .5f)
            {
                float length = span * .5f - cursor;
                var p = centre + (alongX ? Vector3.right : Vector3.forward) * (cursor + length * .5f);
                Box("ApartmentWallPanel", p + Vector3.up * 1.8f, alongX ? new Vector3(length, 3.6f, .22f) : new Vector3(.22f, 3.6f, length), mat);
            }
        }
        private static void DoorFacade(int number, Vector3 p, bool north, bool open)
        {
            foreach (int side in new[] { -1, 1 }) Box("DoorFrame", p + new Vector3(side * 1.03f, 1.32f, 0), new Vector3(.09f, 2.64f, .14f), darkMetal);
            Box("DoorHeader", p + Vector3.up * 2.65f, new Vector3(2.2f, .12f, .14f), darkMetal);
            if (!open)
            {
                SourceProp("DoorsWindows","RoomDoor_"+number,p+Vector3.up*1.3f,2.6f,true,
                    new Vector3(1.97f,2.6f,.14f),null,Imported+"DoorsWindows/Door_DoorFrame_Double.fbx");
            }
            Label("RoomNumber_" + number, number.ToString(), p + new Vector3(0, 2.9f, north ? -.15f : .15f), Quaternion.Euler(0, north ? 180 : 0, 0), .14f);
        }
        private static void Fixture(string id, Vector3 p, Color color, float intensity, float range, bool shadows)
        {
            SourceProp("mounted_fluorescent_lights",id+"ImportedHousing",p,.16f,false,new Vector3(1.05f,.16f,.24f));
            var go = new GameObject(id + "Light", typeof(Light)); go.transform.SetParent(current, false); go.transform.position = p - Vector3.up * .14f;
            var light = go.GetComponent<Light>(); light.type = LightType.Point; light.color = color; light.intensity = intensity; light.range = range;
            light.shadows = shadows ? LightShadows.Soft : LightShadows.None; light.shadowBias = .025f; light.shadowNormalBias = .12f;
            var lightData=new SerializedObject(light.GetUniversalAdditionalLightData());lightData.FindProperty("m_AdditionalLightsShadowResolutionTier").intValue=0;lightData.ApplyModifiedPropertiesWithoutUndo();
        }
        private static void Dynamo(string id, Vector3 p)
        {
            var body = SourceProp("portable_generator",id,p-.39f*Vector3.up,.8f);
            Label(id + "ServiceLabel", "DINAMO\nTAHAN E", p + new Vector3(0, .35f, -.38f), Quaternion.Euler(0, 180, 0), .065f);
            Item(body, id, "Putar dinamo manual");
        }
        private static void AssignImportedMaterials(GameObject model)
        {
            foreach (var renderer in model.GetComponentsInChildren<Renderer>())
            {
                renderer.sharedMaterials = renderer.sharedMaterials.Select(m =>
                {
                    if (!m) return concrete;
                    string key=renderer.name.Contains("__")?renderer.name:renderer.GetComponent<MeshFilter>()?renderer.GetComponent<MeshFilter>().sharedMesh.name:m.name;
                    if(key.Contains("__"))key=key.Substring(key.IndexOf("__",StringComparison.Ordinal)+2);
                    return AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/" + key + ".mat") ?? darkMetal;
                }).ToArray();
            }
        }
        private static void Lighting(VolumeProfile source)
        {
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.skybox=null;
            RenderSettings.ambientSkyColor = new Color(.12f, .16f, .20f); RenderSettings.ambientEquatorColor = new Color(.065f, .085f, .10f); RenderSettings.ambientGroundColor = new Color(.035f, .04f, .04f);
            RenderSettings.fog = true; RenderSettings.fogMode = FogMode.ExponentialSquared; RenderSettings.fogDensity = .0085f; RenderSettings.fogColor = new Color(.035f, .05f, .06f);
            var moon = new GameObject("OvercastMoon", typeof(Light)); moon.transform.rotation = Quaternion.Euler(55, -25, 0); var light = moon.GetComponent<Light>(); light.type = LightType.Directional; light.intensity = .4f; light.color = new Color(.62f, .72f, .8f); light.shadows = LightShadows.Soft;
            if (source)
            {
                string path = Art + "RusunawaVolume.asset";
                var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                if(!profile){profile=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(profile,path);}
                foreach(var previous in profile.components.ToArray())if(AssetDatabase.GetAssetPath(previous)==path)Object.DestroyImmediate(previous,true);
                profile.components.Clear();
                foreach(var original in source.components)
                {var component=Object.Instantiate(original);component.name=original.name;profile.components.Add(component);AssetDatabase.AddObjectToAsset(component,profile);}
                if (profile.TryGet<ColorAdjustments>(out var grade)) { grade.postExposure.value = .2f; grade.contrast.value = 12; grade.saturation.value = -15; }
                if(profile.TryGet<MotionBlur>(out var blur))blur.active=false;
                if(profile.TryGet<FilmGrain>(out var grain))grain.intensity.value=.12f;
                EditorUtility.SetDirty(profile);
                var volume = new GameObject("RusunawaAtmosphere", typeof(Volume)).GetComponent<Volume>(); volume.isGlobal = true; volume.sharedProfile = profile;
            }
            current = environment;
            // Engine particle shader only: no Prolog environment material dependency.
            string rainPath=Art+"Materials/RusunawaRain.mat";
            var rainMaterial=AssetDatabase.LoadAssetAtPath<Material>(rainPath);
            if(!rainMaterial){rainMaterial=new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));AssetDatabase.CreateAsset(rainMaterial,rainPath);}
            rainMaterial.SetFloat("_Surface",1);rainMaterial.SetFloat("_SrcBlend",(float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            rainMaterial.SetFloat("_DstBlend",(float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);rainMaterial.SetFloat("_ZWrite",0);
            rainMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");rainMaterial.renderQueue=3000;EditorUtility.SetDirty(rainMaterial);
            if (rainMaterial)
            {
                Rain("CourtyardDrizzle", new Vector3(0, 17, 0), new Vector3(23, 15, .1f), 180, rainMaterial);
                Rain("ParkingDrizzle", new Vector3(0, 9, -29), new Vector3(62, 12, .1f), 200, rainMaterial);
            }
        }
        private static void Rain(string id, Vector3 p, Vector3 footprint, float rate, Material material)
        {
            var go = new GameObject(id, typeof(ParticleSystem)); go.transform.SetParent(environment, false); go.transform.position = p; go.transform.rotation = Quaternion.Euler(90, 0, 0);
            var ps = go.GetComponent<ParticleSystem>(); var main = ps.main; main.startLifetime = 1.5f; main.startSpeed = 12; main.startSize = .018f; main.maxParticles = 450; main.startColor = new Color(.6f, .7f, .8f, .18f); main.simulationSpace = ParticleSystemSimulationSpace.World;
            var emission = ps.emission; emission.rateOverTime = rate; var shape = ps.shape; shape.shapeType = ParticleSystemShapeType.Box; shape.scale = footprint;
            var renderer = ps.GetComponent<ParticleSystemRenderer>(); renderer.sharedMaterial = material; renderer.renderMode = ParticleSystemRenderMode.Stretch; renderer.lengthScale = 6; renderer.velocityScale = .02f;
        }
    }
}
#endif
