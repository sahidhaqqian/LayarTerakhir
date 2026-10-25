using LayarTerakhir.Core;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed class Phase4Torch : MonoBehaviour
    {
        public Phase4Director director;
        public int sector, candidate;
        public GameObject flame, authenticMark, decoyMark;
        public Transform focus;
        public Light practical;
        public bool Authentic => candidate == Phase4Ids.AuthenticIndex(director.Campaign.Data.phase4.torch_layout_seed, sector);
        public bool PermanentlyOff => sector < 3 ? Phase4Ids.SectorDone(director.Campaign.Data, sector) : director.Completed("P4_SOUTH_OFF");
        public void Present(bool active, bool temporarilyOut)
        {
            bool lit = active && !PermanentlyOff && !temporarilyOut;
            flame.SetActive(lit); if (practical) practical.enabled = lit;
            authenticMark.SetActive(active && Authentic); decoyMark.SetActive(active && !Authentic);
        }
    }
}
