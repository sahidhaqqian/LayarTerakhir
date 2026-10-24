using System;
using UnityEngine;
using LayarTerakhir.Prolog;
namespace LayarTerakhir.Phase2
{
    [Serializable] public sealed class Phase2Anchor { public string id; public Transform point; }
    [Serializable] public sealed class Phase2Room
    {
        public string id; public Bounds bounds;
        public bool Contains(Vector3 p) => bounds.Contains(p + Vector3.up * .4f);
    }
    public sealed class Phase2World : MonoBehaviour
    {
        public Phase2Anchor[] anchors;
        public Phase2Room[] rooms;
        public GameObject basementDoor, stairGrille, inkubatorWall, inkubatorVeil, aura, c3Chain, operationFog;
        public GameObject gunting, gelang, blood, secret, lastri, c3Contents, safePanel;
        public Transform auraTarget, evpTarget, c3Target, wallTarget, safeEchoTarget, lastriHome, lastriHomeL2;
        public Phase2ThermalTarget[] thermalTargets;
        public Phase2Drawer[] drawers;
        public Light[] corridorLights;
        public Transform[] incubatorSources;
        public HingedDoor dutyDoor, safeDoor;
        public Transform Anchor(string id)
        {
            foreach (var a in anchors) if (a.id == id) return a.point;
            return anchors[0].point;
        }
        public string Room(Vector3 p)
        {
            foreach (var room in rooms) if (room.Contains(p)) return room.id;
            return "LANDING";
        }
    }
}
