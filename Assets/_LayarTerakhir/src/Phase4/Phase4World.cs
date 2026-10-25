using System;
using System.Linq;
using UnityEngine;
using LayarTerakhir.Phase3;
namespace LayarTerakhir.Phase4
{
    [Serializable] public sealed class Phase4Anchor { public string id; public Transform point; }
    [Serializable] public sealed class Phase4Zone { public string id; public Bounds bounds; }
    public sealed class Phase4World : MonoBehaviour
    {
        public Phase4Anchor[] anchors;
        public Phase4Zone[] zones;
        public Transform thermalTarget, thermalOcclusionRoot, arTarget, arOcclusionRoot, wellMantra, secretKnock, secretEye, weakPoint;
        public Transform[] patrol, sectorCore, observations;
        public Phase3EvpSource[] sources;
        public Phase4Torch[] torches;
        public GameObject kendi, blade, soil, mirror, illusionWall, hiddenDrawer, glasses, portalCover, portalStairs;
        public GameObject banaspati, fireVisual, skeletonVisual, suro, mergeSuro;
        public GameObject[] offerings;
        public Light entityLight;
        public GameObject emberWarning;
        public GameObject[] fearEchoes;
        public GameObject southSmoke;
        public Bounds[] wards;
        public Transform Anchor(string id) => anchors.First(a => a.id == id).point;
        public string Zone(Vector3 at) => zones.FirstOrDefault(z => z.bounds.Contains(at))?.id ?? "VILLAGE";
        public bool InWard(Vector3 at) => wards != null && wards.Any(b => b.Contains(at + Vector3.up * .5f));
        public Phase4Torch Torch(int sector, int candidate) => torches.First(t => t.sector == sector && t.candidate == candidate);
    }
}
