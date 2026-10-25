using System;
using System.Linq;
using LayarTerakhir.Phase3;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    [Serializable] public sealed class Phase5Anchor { public string id; public Transform point; }
    [Serializable] public sealed class Phase5Zone { public string id; public Bounds bounds; }
    public sealed class Phase5World : MonoBehaviour
    {
        public Phase5Anchor[] anchors;
        public Phase5Zone[] zones;
        public Transform[] doors, ghostWaypoints, sriWaypoints, giantWaypoints, cableTargets, lanternPosts, shadowSpawns;
        public Transform chair, drawerFocus, server, contract, crowd, generator, damarInteraction, bossHead, bossChest, damarFocus, portal, archiveMirror, edge;
        public GameObject tape, drawer, bridgeFlame, bridgeBlocker, arenaBlocker, gateBackMirror, bossRoot, damar, cage, epilogueRoot;
        public GameObject[] lanternVisuals;
        public GameObject[] doorLeaves, sectorBlockers, cableVisuals, clones, uvMarks, arMarks, faceMirrors, collapsePlanks, epilogueSets;
        public Vector3 drawerRest, drawerOffset;
        public Phase3EvpSource[] sources;
        public Bounds[] gazeZones;
        public Transform[] coverCenters;
        public Light[] stageLights;
        public Light generatorLight, droppedLight;
        public Transform Anchor(string id) => anchors.First(a => a.id == id).point;
        public string Zone(Vector3 position) => zones.FirstOrDefault(z => z.bounds.Contains(position))?.id ?? "GATE";
    }
}
