#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static partial class Phase1SceneBuilder
    {
        private static void StoryObjects()
        {
            current = Group("ServicingRoutes_AuthoredStairs");
            // The 102 doorway hides the entire flight from the front of the building.
            d.world.passageWall = Box("K102_IllusionConcrete", new Vector3(-17, 1.3f, 2), new Vector3(.23f, 2.6f, 2.08f), plaster);
            d.world.passageWall.GetComponent<Renderer>().enabled=false;
            d.world.passageVeil=Box("K102_NormalEyeVeil",new Vector3(-17,1.3f,2),new Vector3(.23f,2.6f,2.08f),plaster,false);
            d.world.passageVeil.layer=30;
            Label("K102_Number", "102", new Vector3(-16.85f, 2.9f, 2), Quaternion.Euler(0, 90, 0), .15f);
            Stair("K102_HiddenConcreteFlight", new Vector3(-20, 0, -1), 4, 8, 2.4f);
            Zone("CROSS_102", new Vector3(-20, 4.8f, 8.3f), new Vector3(3, 2, 2.2f));
            Anchor("MAILBOX", new Vector3(-1, 4.06f, -10.7f), Vector3.back);
            current = Group("L2_MailAndArchive");
            Box("MailNoticeBacking", new Vector3(1, 5.4f, -12.84f), new Vector3(5.6f, 2, .13f), timber);
            for (int i = 0; i < 10; i++)
            {
                int number = 201 + i; Vector3 p = new Vector3(-1.2f + i % 5 * 1.1f, 4.9f + i / 5 * .85f, -12.7f);
                var box = Box("Mailbox_" + number, p, new Vector3(.92f, .68f, .28f), number == 207 ? rust : darkMetal);
                Box("MailboxSlot_" + number, p + new Vector3(0, .17f, .15f), new Vector3(.56f, .035f, .02f), timber, false);
                Label("MailNumber_" + number, number.ToString(), p + new Vector3(0, -.06f, .16f), Quaternion.identity, .09f);
                if (number == 207) Item(box, "MAILBOX", "Buka gembok kotak surat 207");
            }
            var letter = Box("ManagerLetter", new Vector3(5, 5.3f, -12.83f), new Vector3(.7f, 1, .035f), paper);
            Item(letter, "MANAGER", "Baca surat pengelola / petunjuk kotak 207");
            Label("ManagerLetterPrint", "PENGELOLA\nPETUNJUK\nKUNCI LIFT", new Vector3(5, 5.3f, -12.80f), Quaternion.identity, .065f, Color.black);
            Box("RentBoard", new Vector3(9, 5.5f, -12.83f), new Vector3(3.3f, 1.8f, .05f), paper);
            Label("RentStatusSixRooms", "STATUS SEWA\n201   202   204\n206   208   210\nMENUNGGAK", new Vector3(9, 5.5f, -12.79f), Quaternion.identity, .12f, new Color(.52f, .045f, .025f));
            GameObject.Find("RentStatusSixRooms").layer=AtmajaPhoneView.ARLayer;
            Label("RentBoardHeading","CATATAN SEWA MEMUDAR",new Vector3(9,6.15f,-12.79f),Quaternion.identity,.065f,Color.black);
            Dynamo("DYNAMO_L2", new Vector3(-8, 4.85f, -12.6f));
            Zone("TUYUL", new Vector3(-13.5f, 5, 0), new Vector3(4, 2, 2));
            var archiveLabel = Box("SeparateArchiveList_BlackCrossTari", new Vector3(3, 5.5f, 19.84f), new Vector3(1, 1.3f, .035f), paper);
            Item(archiveLabel, "ARCHIVE_LABEL", "Periksa daftar penghuni di arsip");
            Label("ArchiveNames", "ARSIP PENGHUNI\nSUTRISNO\nRATIH\n× TARI ×", new Vector3(3, 5.5f, 19.80f), Quaternion.Euler(0, 180, 0), .08f, Color.black);
            GameObject.Find("ArchiveNames").layer=AtmajaPhoneView.ARLayer;
            Box("ArchiveCabinet", new Vector3(5.5f, 4.7f, 18.9f), new Vector3(1.5f, 1.4f, .7f), timber);
            for (int i = 0; i < 3; i++) Box("ArchiveDrawer", new Vector3(5.5f, 4.3f + i * .4f, 18.52f), new Vector3(1.35f, .34f, .06f), darkMetal, false);
            Label("TariDrawerLabel", "TARI — 404", new Vector3(5.5f, 5.13f, 18.47f), Quaternion.Euler(0, 180, 0), .07f);
            d.world.secret = Box("TariNegativeInDrawer", new Vector3(5.5f, 5.3f, 18.45f), new Vector3(.24f, .17f, .02f), darkMetal);
            Item(d.world.secret, "SECRET", "Buka panel Tari / ambil negatif foto");
            // Permanent plan, legible without chat, explains the last digit.
            Box("CourtyardPlan", new Vector3(-3, 5.4f, 12.84f), new Vector3(1.8f, 1.1f, .04f), paper);
            Label("FourSidesPlan", "DENAH KORIDOR\nUTARA — TIMUR\nSELATAN — BARAT\nVOID / JEMURAN DI TENGAH", new Vector3(-3, 5.4f, 12.80f), Quaternion.Euler(0, 180, 0), .075f, Color.black);
            current = Group("FreightLift_ServiceLandings");
            for (int floor = 0; floor < 3; floor++)
            {
                float y = floor * 4;
                Box("LiftLandingFloor", new Vector3(-19, y - .1f, -11), new Vector3(4, .2f, 3.8f), darkMetal);
                // L1/L2 panels sit clear of the west doorway spans so the
                // interaction ray never lands on a door leaf or jamb first,
                // and proud of the solid wall so the wall itself is not first.
                float pz = floor == 2 ? -10f : -8.5f;
                float px = floor == 2 ? -17.1f : -16.85f;
                var panel = Box("LiftPanel_L" + (floor + 1), new Vector3(px, y + 1.15f, pz), new Vector3(.18f, .6f, .3f), rust);
                Item(panel, "LIFT", "Gunakan panel lift barang");
                Label("LiftPanelLabel", "LIFT BARANG\nKONTAK KUNCI", new Vector3(px + .11f, y + 1.65f, pz), Quaternion.Euler(0, 90, 0), .065f);
            }
            var cabin = new GameObject("FreightLiftCabin"); cabin.transform.SetParent(current, false);
            // The jammed cabin waits at L3, not in the L1 lobby: the pump-side
            // service entry stays walkable, and the shaft reads as a real shaft.
            cabin.transform.position = new Vector3(-20, 8, -11); d.world.liftCabin = cabin.transform;
            // The rear plate is a child of the cabin: position it in cabin space
            // (Box places world coordinates), or it stays behind at L1 forever.
            Box("CabinRearPlate", new Vector3(-20.9f, 9.4f, -11), new Vector3(.15f, 2.8f, 3.2f), rust, true, cabin.transform);
            Anchor("L3", new Vector3(-18.3f, 8.06f, -11), Vector3.right);
            current = Group("L3_KuntilanakChamber");
            d.world.servicePanelItem = Dynamo("DYNAMO_L3_PANEL", new Vector3(-18.5f, 8.85f, -12.5f));
            Label("L3_Notice", "PANEL DAYA SERVIS\nTAHAN E UNTUK MENGISI\nAMBIL ALAT, LALU KE TANGGA", new Vector3(-18, 9.8f, -12.84f), Quaternion.identity, .09f);
            Zone("KUNTI", new Vector3(-1, 9.1f, -11), new Vector3(1, 2.2f, 4));
            var trolley = Box("CleanerServiceTrolley", new Vector3(2, 8.65f, -12.1f), new Vector3(1.1f, .08f, .65f), darkMetal);
            foreach (float x in new[] { 1.55f, 2.45f }) foreach (float z in new[] { -12.35f, -11.85f }) Pipe("TrolleyLeg", new Vector3(x, 8.1f, z), new Vector3(x, 8.6f, z), .025f, rust);
            var cutter=SourceProp("bolt_cutters_01","BoltCutter_HandReach",new Vector3(2,8.75f,-11.85f),.12f,true,new Vector3(.35f,.12f,.75f));
            d.world.cutter = cutter; d.world.cutterItem = Item(cutter, "CUTTER", "Ambil pemotong besi tanpa membalik kamera");
            d.world.chain = Box("L3_ChainGate", new Vector3(20, 9.25f, -8.95f), new Vector3(1.95f, 2.5f, .15f), rust);
            d.world.chain.GetComponent<Renderer>().enabled = false;
            SourceProp("DoorsWindows","DownloadedSafetyChain",new Vector3(20,8.65f,-9.06f),1.3f,false,
                new Vector3(1.95f,1.3f,.3f),darkMetal,Imported+"DoorsWindows/SafetyChain.fbx",d.world.chain.transform);
            d.world.chainItem = Item(d.world.chain, "CHAIN", "Potong rantai tangga — tahan kamera ke hantu");
            Stair("L3ToL4_EnclosedStair", new Vector3(20, 8, -9), 4, 7, 2);
            Box("StairwellEastWall", new Vector3(21.3f, 10.4f, -5.5f), new Vector3(.25f, 5, 8), soot);
            Box("StairwellWestWall", new Vector3(18.7f, 10.4f, -5.5f), new Vector3(.25f, 5, 8), soot);
            Anchor("L4", new Vector3(20, 12.06f, -.5f), Vector3.forward);
            current = Group("L4_Investigation_Ritual");
            foreach (float x in new[] { -6f, 2.5f, 11.5f }) Box("RoomPartition", new Vector3(x, 13.8f, 16.5f), new Vector3(.22f, 3.6f, 7), soot);
            d.world.wall401 = Box("K401_BreakawayPanel", new Vector3(-10, 13.3f, 13.03f), new Vector3(1.95f, 2.6f, .14f), soot);
            Zone("GENDER", new Vector3(14, 13, 4), new Vector3(5, 2.2f, 2));
            d.world.clawAim = Anchor("CLAW", new Vector3(-2, 13.45f, 13.05f), Vector3.back);
            Box("K402_ScratchedIronDoor",new Vector3(-2,13.3f,13.03f),new Vector3(1.95f,2.6f,.12f),darkMetal);
            for (int i = 0; i < 5; i++)
            {
                var slash = Box("K402_FiveFingerGouge", new Vector3(-2.3f + i * .14f, 13.55f - Mathf.Abs(i - 2) * .08f, 12.94f), new Vector3(.04f, .9f, .035f), rust, false);
                slash.transform.rotation = Quaternion.Euler(0, 0, -20);
            }
            var diary = Box("TariDiaryBeside402", new Vector3(-3.5f, 13.2f, 12.8f), new Vector3(.45f, .6f, .035f), paper);
            Item(diary, "TARI_DIARY", "Baca catatan Tari");
            Anchor("INVESTIGATION", new Vector3(7, 12.06f, 16), Vector3.back);
            var hinge=new GameObject("K403_DoorHinge");hinge.transform.SetParent(current,false);hinge.transform.position=new Vector3(5.98f,12,13.08f);
            var safeDoor=SourceProp("DoorsWindows","K403_UsableDoor",new Vector3(7,13.3f,13.08f),2.6f,true,new Vector3(1.97f,2.6f,.12f),null,
                Imported+"DoorsWindows/Door_DoorFrame_Double.fbx",hinge.transform);
            var door=safeDoor.AddComponent<HingedDoor>();door.hinge=hinge.transform;door.angle=-95;door.label="Buka pintu K403";d.world.safeRoomDoor=door;
            Dynamo("DYNAMO_K403", new Vector3(9, 12.85f, 19));
            HideLocker("HIDE_K403", new Vector3(5, 12, 18.6f));
            // A bathroom with individual stall walls and a visible radio gives EVP a place.
            Box("BathroomNorthWall", new Vector3(-20.5f, 13.6f, 5.4f), new Vector3(7, 3.2f, .18f), soot);
            Box("BathroomSouthWall", new Vector3(-20.5f, 13.6f, -1.4f), new Vector3(7, 3.2f, .18f), soot);
            var radio = SourceProp("vintage_radio_transceiver","BathroomDeadRadio",new Vector3(-20,13,4.8f),.33f);
            Label("EVPStaticClue", "STASIUN TIDAK DIKENAL\n[C] EVP", new Vector3(-20, 13.2f, 4.64f), Quaternion.Euler(0, 180, 0), .06f);
            d.world.evpPoint = Anchor("EVP", new Vector3(-20, 12, 2.5f), Vector3.left);
            Anchor("BATH_ENTRY", new Vector3(-16, 12, 2), Vector3.left);
            Zone("EVP_HINT", new Vector3(-15.7f, 13, 2), new Vector3(3, 2.2f, 4));
            HideLocker("HIDE_BATH_STALL", new Vector3(-22.5f, 12, 1.8f));
            for (int i = 0; i < 3; i++)
            {
                Box("WCStallPartition", new Vector3(-22.3f, 13.05f, -1 + i * 2.1f), new Vector3(3, 2.1f, .12f), plaster);
                Box("WCBase", new Vector3(-23.1f, 12.22f, -.1f + i * 2.1f), new Vector3(.48f, .44f, .7f), concrete);
            }
            // Kitchen stove is a fixed masonry kiln; the keris cannot roll under physics clutter.
            Box("KitchenNorthWall", new Vector3(20.5f, 13.6f, 8), new Vector3(7, 3.2f, .18f), soot);
            Box("KitchenKilnBase", new Vector3(21.7f, 12.35f, 5.5f), new Vector3(2.8f, .7f, 1.5f), concrete);
            SourceProp("barrel_stove","ExistingRusunawaStove",new Vector3(21.7f,12.92f,5.8f),.65f);
            d.world.auraAim = Anchor("AURA", new Vector3(21.7f, 12.82f, 4.95f), Vector3.back);
            d.world.keris = Keris(new Vector3(21.7f, 12.76f, 4.95f));
            Item(d.world.keris, "KERIS", "Ambil Pecahan Keris Penumbal");
            var arAura=Object.Instantiate(d.world.keris);arAura.name="KerisAR_Aura";arAura.transform.SetParent(current,false);
            foreach(var hit in arAura.GetComponentsInChildren<Collider>())Object.DestroyImmediate(hit);
            Object.DestroyImmediate(arAura.GetComponent<Phase1WorldItem>());
            arAura.transform.localScale*=1.06f;
            foreach(var node in arAura.GetComponentsInChildren<Transform>())node.gameObject.layer=AtmajaPhoneView.ARLayer;
            foreach(var renderer in arAura.GetComponentsInChildren<Renderer>())renderer.sharedMaterial=emissive;
            Box("K404RearWall", new Vector3(0, 13.8f, -20), new Vector3(10, 3.6f, .22f), soot);
            d.world.wall404North = Box("K404_WestBreakaway", new Vector3(-5, 13.8f, -16.5f), new Vector3(.22f, 3.6f, 7), soot);
            d.world.wall404South = Box("K404_EastBreakaway", new Vector3(5, 13.8f, -16.5f), new Vector3(.22f, 3.6f, 7), soot);
            d.world.altarFog = Box("K404_SealedThreshold", new Vector3(0, 13.3f, -13.05f), new Vector3(2.08f, 2.6f, .22f), darkMetal);
            Label("K404_RoomLabel", "404", new Vector3(0, 14.9f, -12.82f), Quaternion.identity, .2f);
            var altar = Box("AltarArang", new Vector3(0, 12.55f, -17.7f), new Vector3(2.4f, 1.1f, 1.15f), soot);
            Item(altar, "ALTAR", "Tancapkan Pecahan Keris / mulai ritual");
            Anchor("ALTAR", new Vector3(0, 12.06f, -16), Vector3.back); Anchor("AFTER_BOSS", new Vector3(0, 12.06f, -16), Vector3.back);
            d.world.cloth = Box("DamarJacketCloth", new Vector3(-.6f, 13.14f, -17.7f), new Vector3(.5f, .06f, .4f), timber);
            Item(d.world.cloth, "CLOTH", "Ambil kain jaket Damar"); d.world.cloth.SetActive(false);
            d.world.cassette = SourceProp("portable_cassette_player","MbahSuroTapeAndPlayer",new Vector3(.65f,13.22f,-17.7f),.18f);
            Item(d.world.cassette, "TAPE", "Putar kaset pita Mbah Suro"); d.world.cassette.SetActive(false);
            d.world.halogenTargets = new Transform[3]; d.world.halogenLights = new Light[3];
            Halogen(0, "N", new Vector3(0, 12, 11), new Vector3(-3.3f, 13.15f, 12.8f));
            Halogen(1, "E", new Vector3(15, 12, 3), new Vector3(16.7f, 13.15f, 6));
            Halogen(2, "W", new Vector3(-15, 12, -3), new Vector3(-16.7f, 13.15f, -6));
            var power = SourceProp("power_box_01","ArenaEmergencyPower",new Vector3(8,13,-12.65f),.9f);
            Item(power, "ARENA_POWER", "Brancher daya darurat saat boss recovery");
            Label("ArenaPowerLabel", "DAYA DARURAT\nGUNAKAN SAAT RECOVERY", new Vector3(8, 13.65f, -12.36f), Quaternion.identity, .08f);
            var debris = Box("ConcreteDebrisTemplate", new Vector3(0, -100, 0), new Vector3(.32f, .27f, .4f), concrete, false); debris.SetActive(false);
            anchors.Add(new Phase1Anchor { id = "DEBRIS_TEMPLATE", point = debris.transform });
            RoomDoorsAndEntry();
            DressRooms();
            HospitalTransmissionSet();
        }
        private enum RoomDoorState { Closed, CasedOpen, Chained, DebrisBlocked }
        private static void RoomDoor(int number, Vector3 centre, Vector3 facing, RoomDoorState state, string note = null)
        {
            bool alongX = Mathf.Abs(facing.z) > .5f;
            Vector3 side = alongX ? Vector3.right : Vector3.forward;
            foreach (float s in new[] { -1.03f, 1.03f })
                Box("RoomDoorJamb", centre + side * s + Vector3.up * 1.32f, alongX ? new Vector3(.09f, 2.64f, .14f) : new Vector3(.14f, 2.64f, .09f), darkMetal);
            Box("RoomDoorHeader", centre + Vector3.up * 2.62f, alongX ? new Vector3(2.2f, .14f, .14f) : new Vector3(.14f, .14f, 2.2f), darkMetal);
            if (state != RoomDoorState.CasedOpen)
                SourceProp("DoorsWindows", "RoomDoorLeaf_" + number, centre + Vector3.up * 1.3f, 2.6f, true,
                    alongX ? new Vector3(1.97f, 2.6f, .12f) : new Vector3(.12f, 2.6f, 1.97f), null,
                    Imported + "DoorsWindows/Door_DoorFrame_Double.fbx");
            if (state == RoomDoorState.Chained)
                SourceProp("DoorsWindows", "RoomDoorChain_" + number, centre + Vector3.up * 1.45f, 1.3f, false,
                    alongX ? new Vector3(1.95f, 1.3f, .3f) : new Vector3(.3f, 1.3f, 1.95f), darkMetal, Imported + "DoorsWindows/SafetyChain.fbx");
            Quaternion faceRot = facing.z > .5f ? Quaternion.identity : facing.z < -.5f ? Quaternion.Euler(0, 180, 0) :
                facing.x > .5f ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
            if (number > 0)
                Label("RoomPlate_" + number, number.ToString(), centre + Vector3.up * 2.95f + facing * .18f, faceRot, .13f);
            if (!string.IsNullOrEmpty(note))
                Label("RoomNote_" + number, note, centre + Vector3.up * 1.7f + facing * .14f, faceRot, .065f);
        }
        private static void RoomDoorsAndEntry()
        {
            current = Group("L1_ServiceEntryAndRoomDoors");
            // The pump side finally reads as an entrance: a real facade doorway,
            // a short lobby, and a partition so the route never crosses an open shell.
            // DoorWall gaps are local offsets from the centre, not world coordinates.
            DoorWall(new Vector3(-24.1f, 0, -11), 4, false, new[] { 0f }, plaster);
            RoomDoor(0, new Vector3(-24.1f, 0, -11), Vector3.left, RoomDoorState.CasedOpen);
            Label("ServiceEntrySign", "PINTU SERVIS\nRUANG POMPA", new Vector3(-23.85f, 3.0f, -11), Quaternion.Euler(0, -90, 0), .1f);
            Box("EntryCorridorCeiling", new Vector3(-20.5f, 3.65f, -11), new Vector3(7.5f, .25f, 4.5f), concrete);
            DoorWall(new Vector3(-20.5f, 0, -9), 7, true, new[] { 1.5f }, plaster);
            RoomDoor(0, new Vector3(-19, 0, -9), Vector3.back, RoomDoorState.CasedOpen);
            RoomDoor(0, new Vector3(-17, 0, -11), Vector3.right, RoomDoorState.CasedOpen);
            // L1 rooms open off the galleries with real front doors, not rear gaps.
            RoomDoor(105, new Vector3(-12, 0, -13), Vector3.forward, RoomDoorState.Closed);
            RoomDoor(106, new Vector3(-5, 0, -13), Vector3.forward, RoomDoorState.DebrisBlocked);
            RoomDoor(107, new Vector3(3, 0, -13), Vector3.forward, RoomDoorState.Closed);
            RoomDoor(108, new Vector3(11, 0, -13), Vector3.forward, RoomDoorState.Chained, "DIGEMBOK\nWARGA");
            RoomDoor(109, new Vector3(-17, 0, 8), Vector3.right, RoomDoorState.Closed);
            RoomDoor(110, new Vector3(17, 0, 4), Vector3.left, RoomDoorState.Closed);
            // L2 rooms; 207 belongs to the RT head, 203 stays open as the archive room.
            RoomDoor(205, new Vector3(-12, 4, -13), Vector3.forward, RoomDoorState.Closed);
            RoomDoor(206, new Vector3(-5, 4, -13), Vector3.forward, RoomDoorState.DebrisBlocked);
            RoomDoor(207, new Vector3(3, 4, -13), Vector3.forward, RoomDoorState.Chained, "KETUA RT — 207");
            RoomDoor(208, new Vector3(11, 4, -13), Vector3.forward, RoomDoorState.Closed);
            RoomDoor(209, new Vector3(-17, 4, -11), Vector3.right, RoomDoorState.Closed);
            RoomDoor(210, new Vector3(-17, 4, 2), Vector3.right, RoomDoorState.Closed);
            RoomDoor(0, new Vector3(-17, 4, 8), Vector3.right, RoomDoorState.CasedOpen);
            RoomDoor(211, new Vector3(17, 4, 4), Vector3.left, RoomDoorState.Closed);
            Label("ArchiveRoomSign", "ARSIP\nPENGHUNI", new Vector3(3, 6.9f, 12.80f), Quaternion.Euler(0, 180, 0), .08f);
            // L4 stairwell: solid floor around a proper opening, curbs on every exposed edge.
            current = Group("L4_StairwellGuards");
            Box("ShaftFillWest", new Vector3(18, 11.88f, -6), new Vector3(2, .24f, 8), concrete);
            Box("ShaftFillEast", new Vector3(22.5f, 11.88f, -6), new Vector3(3, .24f, 8), concrete);
            Box("ShaftCurbLandingWest", new Vector3(18, 12.55f, -2), new Vector3(2, 1.1f, .25f), concrete);
            Box("ShaftCurbLandingEast", new Vector3(22.5f, 12.55f, -2), new Vector3(3, 1.1f, .25f), concrete);
            Box("ShaftCurbSouthWest", new Vector3(18, 12.55f, -10.2f), new Vector3(2, 1.1f, .25f), concrete);
            Box("ShaftCurbSouthEast", new Vector3(22.5f, 12.55f, -10.2f), new Vector3(3, 1.1f, .25f), concrete);
        }
        private static void HospitalTransmissionSet()
        {
            // A five-second miniature recorded set, not a new playable chapter/scene.
            current=Group("DamarTransmission_NonPlayableSet");
            Vector3 origin=new Vector3(180,0,0);
            Box("HospitalFilmFloor",origin+new Vector3(0,-.1f,0),new Vector3(4,.2f,12),tiles);
            foreach(int side in new[]{-1,1})Box("HospitalFilmWall",origin+new Vector3(side*2,1.7f,0),new Vector3(.2f,3.4f,12),plaster);
            Box("HospitalFilmEnd",origin+new Vector3(0,1.7f,-5.5f),new Vector3(4,3.4f,.2f),soot);
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Prefabs/DamarDesignerCharacter.prefab");
            var damar=(GameObject)PrefabUtility.InstantiatePrefab(prefab);damar.name="DamarBoundInWheelchair";damar.transform.SetParent(current,false);damar.transform.position=origin;
            var motion=damar.GetComponent<Animation>();if(motion&&motion.GetClip("Riding")){motion.clip=motion.GetClip("Riding");motion.Play("Riding");motion.Sample();motion.Stop();}
            SourceProp("wheelchair_01","DamarExistingWheelchair",origin+new Vector3(0,.7f,0),1.4f,false);
            var head=damar.GetComponentsInChildren<Transform>(true).First(t=>t.name=="Head");
            Box("DamarMouthTape",head.position+new Vector3(0,-.08f,.095f),new Vector3(.14f,.045f,.025f),darkMetal,false);
            var lightGo=new GameObject("HospitalFilmFluorescent",typeof(Light));lightGo.transform.SetParent(current,false);lightGo.transform.position=origin+new Vector3(0,2.8f,1);
            var light=lightGo.GetComponent<Light>();light.type=LightType.Point;light.color=new Color(.56f,.75f,.7f);light.range=7;light.intensity=2;light.cullingMask=1<<28;
            var cameraGo=new GameObject("DamarTransmissionCamera",typeof(Camera));cameraGo.transform.SetParent(current,false);cameraGo.transform.position=origin+new Vector3(.25f,1.1f,3.6f);cameraGo.transform.LookAt(origin+new Vector3(0,1.1f,0));
            var camera=cameraGo.GetComponent<Camera>();camera.cullingMask=1<<28;camera.nearClipPlane=.1f;camera.farClipPlane=15;camera.fieldOfView=48;camera.enabled=false;camera.backgroundColor=Color.black;camera.clearFlags=CameraClearFlags.SolidColor;
            d.world.transmissionCamera=camera;
            foreach(var child in current.GetComponentsInChildren<Transform>(true))child.gameObject.layer=28;
        }
        private static void Stair(string id, Vector3 start, float rise, float run, float width)
        {
            var root = new GameObject(id); root.transform.SetParent(current, false);
            for(int flight=0;flight<2;flight++)
                SourceProp("ConcreteInterior",id+"_DownloadedFlight_"+flight,start+new Vector3(0,rise*(flight*.5f+.25f),run*(flight*.5f+.25f)),rise*.5f,false,
                    new Vector3(width,rise*.5f,run*.5f),concrete,Imported+"ConcreteInterior/StairsForward.fbx",root.transform);
            // One inclined collision mesh over the visible closed steps: no snagging,
            // no unsupported ramp floating in space, and no trigger used as a floor.
            var collision = new GameObject(id + "_SolidFlight", typeof(MeshCollider)); collision.transform.SetParent(root.transform, false);
            var mesh = new Mesh { name = id + "Collision" };
            mesh.vertices = new[] { start + new Vector3(-width/2,0,0), start + new Vector3(width/2,0,0), start + new Vector3(-width/2,rise,run), start + new Vector3(width/2,rise,run),start+new Vector3(-width/2,0,run),start+new Vector3(width/2,0,run) };
            mesh.triangles = new[] { 0,2,1,1,2,3, 0,4,2, 1,3,5, 2,4,3,3,4,5, 0,1,4,1,5,4 }; mesh.RecalculateNormals(); mesh.RecalculateBounds();
            string path = Art + "Meshes/" + id + "Collision.asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path); if (existing) { EditorUtility.CopySerialized(mesh, existing); Object.DestroyImmediate(mesh); mesh = existing; } else AssetDatabase.CreateAsset(mesh, path);
            collision.GetComponent<MeshCollider>().sharedMesh = mesh;
            foreach (int side in new[] { -1, 1 })
            {
                Pipe(id + "Handrail", start + new Vector3(side * (width/2-.08f), .95f, 0), start + new Vector3(side * (width/2-.08f), rise+.95f, run), .035f, darkMetal);
                for (int i = 0; i <= 6; i++)
                {
                    var foot = start + new Vector3(side * (width/2-.08f), rise*i/6, run*i/6);
                    Pipe(id + "Baluster", foot, foot + Vector3.up * .95f, .022f, rust);
                }
            }
            // Enclosed service stair: stepped parapet cheeks and newel posts read
            // as solid concrete. The smooth collision flight underneath is untouched.
            Material cheek = start.y > 6 ? soot : concrete;
            foreach (int side in new[] { -1, 1 })
            {
                const int steps = 5;
                for (int i = 0; i < steps; i++)
                {
                    float t = (i + .5f) / steps;
                    Box(id + "_Cheek", start + new Vector3(side * (width / 2 + .11f), rise * t + .35f, run * t),
                        new Vector3(.22f, 1.1f, run / steps + .06f), cheek);
                }
                Box(id + "_NewelBase", start + new Vector3(side * (width / 2 + .11f), .55f, -.1f), new Vector3(.3f, 1.1f, .3f), cheek);
                Box(id + "_NewelTop", start + new Vector3(side * (width / 2 + .11f), rise + .55f, run + .1f), new Vector3(.3f, 1.1f, .3f), cheek);
            }
        }
        private static void HideLocker(string id, Vector3 p)
        {
            var root = new GameObject(id); root.transform.SetParent(current, false); root.transform.position = p + Vector3.up;
            SourceProp("modern_wooden_cabinet",id+"_DownloadedCabinet",p+Vector3.up*1.15f,2.3f,false,new Vector3(1.3f,2.3f,1.1f),parent:root.transform);
            foreach(int side in new[]{-1,1})
            {var hull=new GameObject("CabinetSideCollision",typeof(BoxCollider));hull.transform.SetParent(root.transform,false);hull.transform.position=p+new Vector3(side*.62f,1.15f,0);hull.GetComponent<BoxCollider>().size=new Vector3(.06f,2.3f,1.1f);}
            var trigger = root.AddComponent<BoxCollider>(); trigger.size = new Vector3(1.2f, 2.3f, 1.2f); trigger.isTrigger = true;
            Item(root, id, "Sembunyi di lemari / bilik");
            var hide = new GameObject("HideAnchor"); hide.transform.SetParent(root.transform, false); hide.transform.position = p; hide.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        private static GameObject Keris(Vector3 p)
        {
            return SourceProp("MrLeapWeapons","PecahanKerisPenumbal",p,.08f,true,new Vector3(.18f,.06f,.52f),rust,Imported+"MrLeapWeapons/Kris.fbx");
        }
        private static void Halogen(int index, string compass, Vector3 target, Vector3 switchPosition)
        {
            d.world.halogenTargets[index] = Anchor("HALOGEN_TARGET_" + compass, target, Vector3.forward);
            var switchBox = Box("HalogenSwitch_" + compass, switchPosition, new Vector3(.28f, .5f, .16f), rust);
            Item(switchBox, "HALOGEN_" + compass, "Tarik tuas halogen " + (compass == "N" ? "UTARA" : compass == "E" ? "TIMUR" : "BARAT"));
            Vector3 lampPosition = switchPosition + Vector3.up * 1.4f;
            SourceProp("portable_searchlight","HalogenImportedHousing_"+compass,lampPosition,.45f,false);
            var go = new GameObject("Halogen_" + compass, typeof(Light)); go.transform.SetParent(current, false); go.transform.position = lampPosition;
            go.transform.rotation = Quaternion.LookRotation(target + Vector3.up - lampPosition);
            var light = go.GetComponent<Light>(); light.type = LightType.Spot; light.range = 13; light.spotAngle = 65; light.innerSpotAngle = 38; light.intensity = 75; light.color = new Color(1, .8f, .48f); light.shadows = LightShadows.Soft; light.enabled = false;
            d.world.halogenLights[index] = light;
            Label("HalogenCompass_" + compass, compass == "N" ? "UTARA" : compass == "E" ? "TIMUR" : "BARAT", switchPosition + Vector3.up * .5f, compass == "N" ? Quaternion.Euler(0,180,0) : compass == "E" ? Quaternion.Euler(0,-90,0) : Quaternion.Euler(0,90,0), .1f);
        }
        private static void DressRooms()
        {
            // Everyday traces of displaced tenants, placed outside the clear walking line.
            current = Group("RusunawaDomesticTraces");
            for (int floor = 0; floor < 4; floor++)
            {
                if (floor == 2) continue;
                float y = floor * 4;
                for (int i = 0; i < 5; i++)
                {
                    float x = -12 + i * 5.5f;


                    Pipe("ClotheslineAnchor", new Vector3(x, y + 2.4f, 9), new Vector3(x + 2.4f, y + 2.3f, 8.8f), .008f, darkMetal);
                    if (floor < 2) Box("AbandonedDryingCloth", new Vector3(x + .9f, y + 1.9f, 8.8f), new Vector3(.7f, .8f, .012f), timber, false);
                }
            }
            for(int i=0;i<12;i++)
            {
                float x=-11+i*2;
                Box("CourtyardDrainGrate",new Vector3(x,.04f,8.3f),new Vector3(.7f,.03f,.12f),darkMetal,false);
            }
        }
        private static void Threats()
        {
            current = Group("ChapterActors");
            d.world.genderuwoSpawn = Anchor("GENDER_SPAWN", new Vector3(-10, 12, 16), Vector3.back);
            d.world.kuntilanakSpawn = Anchor("KUNTI_SPAWN", new Vector3(-11, 8, -11), Vector3.right);
            d.world.genderuwo = Character("Genderuwo", d.world.genderuwoSpawn.position, 0);
            d.world.kuntilanak = Character("Putih", d.world.kuntilanakSpawn.position, 0);
            d.world.tuyul = Character("Tuyul", new Vector3(-15, 4, 1), AtmajaPhoneView.ARLayer);
        }
        private static GameObject Character(string role, Vector3 position, int layer)
        {
            string modelPath = Art + "Characters/Models/" + role + ".fbx";
            var importer = AssetImporter.GetAtPath(modelPath) as ModelImporter;
            if (!importer) throw new InvalidOperationException("Run Tools/prepare_phase1_characters.py for " + role + " first");
            importer.importCameras = importer.importLights = importer.addCollider = false;
            importer.importAnimation = true; importer.animationType = ModelImporterAnimationType.Legacy;
            importer.animationCompression = ModelImporterAnimationCompression.Off; importer.materialImportMode = ModelImporterMaterialImportMode.None;
            importer.SaveAndReimport();
            var clips = importer.defaultClipAnimations;
            foreach (var clip in clips) { clip.name = clip.name.Contains("|") ? clip.name.Substring(clip.name.LastIndexOf('|') + 1) : clip.name; clip.loopTime = clip.name != "Windup"; clip.wrapMode = clip.loopTime ? WrapMode.Loop : WrapMode.ClampForever; }
            importer.clipAnimations = clips; importer.SaveAndReimport();
            var material = Plain(role + "Phase1Surface", Color.white, .18f);material.SetFloat("_BumpScale",.45f);
            foreach(var slot in new[] { "Albedo", "Normal", "MetalSmooth" })
            {
                string path = Art + "Characters/Textures/" + role + slot + ".png";
                var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                textureImporter.maxTextureSize = slot == "Albedo" ? 2048 : 1024; textureImporter.textureType = slot == "Normal" ? TextureImporterType.NormalMap : TextureImporterType.Default;
                textureImporter.sRGBTexture = slot == "Albedo"; textureImporter.mipmapEnabled = true; textureImporter.SaveAndReimport();
                material.SetTexture(slot == "Albedo" ? "_BaseMap" : slot == "Normal" ? "_BumpMap" : "_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture2D>(path));
            }
            material.EnableKeyword("_NORMALMAP"); material.EnableKeyword("_METALLICSPECGLOSSMAP"); material.SetFloat("_Cull", 0);
            var go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(modelPath)); go.name = role + "_Phase1"; go.transform.SetParent(current, false); go.transform.position = position;
            foreach (var transform in go.GetComponentsInChildren<Transform>(true)) transform.gameObject.layer = layer;
            foreach (var renderer in go.GetComponentsInChildren<Renderer>()) renderer.sharedMaterial = material;
            var animation = go.GetComponent<Animation>(); if (!animation) animation = go.AddComponent<Animation>(); animation.playAutomatically = false;
            foreach (var clip in AssetDatabase.LoadAllAssetsAtPath(modelPath).OfType<AnimationClip>().Where(c => !c.name.StartsWith("__preview__"))) animation.AddClip(clip, clip.name);
            animation.clip = animation.GetClip("Idle");
            go.SetActive(false); return go;
        }
        private static Phase1Sound[] Sounds()
        {
            var names = new[] { ("Unlock", "DrawerUnlock"), ("Chain", "DoorLatch"), ("Impact", "DoorKnock"), ("Lift", "DepartureEngine"), ("Manifest", "DoorKnock"), ("Foot", "FootstepTile0"), ("Glass", "UIConfirm"), ("Halogen", "PhoneSwitch"), ("Tape", "ClothPickup"), ("Drip", "PhoneSwitch"), ("Echo", "RoomTone"), ("Windup", "DoorKnock") };
            var sounds=names.Select(pair => new Phase1Sound { id = pair.Item1, clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_LayarTerakhir/assets/Audio/Phase1/"+pair.Item1+".wav")??AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_LayarTerakhir/assets/Audio/Prepared/" + pair.Item2 + ".wav"), gain = pair.Item1 == "Foot" ? .8f : .5f }).ToList();
            foreach(string id in new[]{"Kunti","Tuyul","Seal"})sounds.Add(new Phase1Sound{id=id,clip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_LayarTerakhir/assets/Audio/Phase1/"+id+".wav"),gain=.6f});
            return sounds.ToArray();
        }
    }
}
#endif
