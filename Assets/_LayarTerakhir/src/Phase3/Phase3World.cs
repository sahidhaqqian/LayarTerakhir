using System;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    [Serializable] public sealed class Phase3Anchor { public string id; public Transform point; }
    [Serializable] public sealed class Phase3Zone { public string id; public Bounds bounds; }
    public sealed class Phase3World : MonoBehaviour
    {
        public Phase3Anchor[] anchors;
        public Phase3Zone[] zones;
        public GameObject accessGate,hallGate,arenaLock,serviceGate,sling,doll,sickle,cloth,secret,card,shoe;
        public GameObject wewe,flareTemplate,mist,clothKnot,serviceDoor;
        public GameObject[] altarRelics;
        public Bounds alcove,furnacePit;
        public Transform secretUvTarget;
        public Transform slingTarget,rajahTarget,decoy,bossHome,bossRetreat,pullCentre;
        public Transform[] beams,levers,patrol,roof;
        public Light accessLight;
        public Light[] ritualLights;
        public Phase3FloorTile[] floors;
        public Phase3EvpSource[] sources;
        public Transform Anchor(string id)
        {foreach(var a in anchors)if(a.id==id)return a.point;throw new InvalidOperationException("Unknown P3 safe anchor: "+id);}
        public string Room(Vector3 p)
        {foreach(var z in zones)if(z.bounds.Contains(p+Vector3.up*.4f))return z.id;return "YARD";}
    }
}
