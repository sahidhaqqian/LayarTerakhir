using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    // Small pooled emitters; no fullscreen fog, volumetrics or per-frame allocations.
    public sealed class AtmajaEvidenceAtmosphere : MonoBehaviour
    {
        public AtmajaDirector director;
        public ParticleSystem[] sealSmoke = new ParticleSystem[3];
        public Light cupAura;
        public GameObject wardrobeSlats;
        private void LateUpdate()
        {
            var campaign = CampaignState.Instance; var phone = SmartphoneController.Instance;
            if (!campaign || !director) return;
            for (int i = 0; i < sealSmoke.Length; i++)
                if (sealSmoke[i])
                {
                    bool on = !campaign.HasInventoryItem("EVENT_P0_CHAIN" + (i + 1) + "_BROKEN");
                    var emission = sealSmoke[i].emission; emission.enabled = on;
                }
            if (cupAura) cupAura.enabled = phone && phone.isPhoneRaised && phone.isScreenOn && phone.currentScanMode == ScanVisualMode.AR && !campaign.HasInventoryItem("EVENT_P0_CHAIN1_BROKEN");
            if (wardrobeSlats) wardrobeSlats.SetActive(director.basement && director.basement.IsHidden);
        }
    }
}
