using LayarTerakhir.Player;
using UnityEngine;
namespace LayarTerakhir.Phase1
{
    public sealed class Phase1Zone : MonoBehaviour
    {
        public Phase1Director director;
        public string id;
        private void OnTriggerEnter(Collider other)
        { if (other.GetComponent<FirstPersonController>()) director.EnterZone(id); }
    }
}
