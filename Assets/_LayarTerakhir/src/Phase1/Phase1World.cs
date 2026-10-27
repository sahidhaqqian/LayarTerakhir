using System;
using LayarTerakhir.Player;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    [Serializable] public sealed class Phase1Anchor { public string id; public Transform point; }
    public sealed class Phase1World : MonoBehaviour
    {
        public Phase1Anchor[] anchors;
        public Transform liftCabin, liftPlayerAnchor, kuntilanakSpawn, genderuwoSpawn;
        public Transform[] patrolPoints, halogenTargets;
        public Light[] halogenLights;
        public GameObject passageWall, passageVeil, chain, cutter, altarFog, wall401, wall404North, wall404South;
        public GameObject keris, secret, cloth, cassette, tuyul, kuntilanak, genderuwo;
        public Transform clawAim, auraAim, evpPoint;
        public Phase1WorldItem cutterItem, chainItem;
        public Phase1WorldItem servicePanelItem;
        public Camera transmissionCamera;
        public LayarTerakhir.Prolog.HingedDoor safeRoomDoor;
        public GameObject victimEcho;
        public Transform Anchor(string id)
        {
            foreach(var a in anchors) if(a.id == id) return a.point;
            return anchors[0].point;
        }
    }


}
