using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace LayarTerakhir.Phase4
{
    public sealed class Phase4FearPresentation : MonoBehaviour
    {
        public Phase4Director director;
        public Volume volume;
        private Vignette vignette;
        private VolumeProfile instance;
        private void Start()
        { instance = volume.profile; instance.TryGet(out vignette); }
        private void LateUpdate()
        {
            if (!director.Ready || !vignette || !director.ClockRunning) return;
            float fear = director.ClarityActive ? 0 : Mathf.Clamp01((70 - director.Campaign.Data.sanity) / 70);
            float pulse = GameShell.ReducedGlitch ? 0 : (1 + Mathf.Sin(director.Elapsed * 3)) * .025f;
            vignette.intensity.Override(Mathf.Lerp(.16f, GameShell.ReducedGlitch ? .2f : .27f + pulse, fear));
            bool echoes = director.Completed("P4_BANASPATI_REVEAL") && !director.Completed("P4_BOSS_WIN") && !director.ClarityActive && !GameShell.ReducedGlitch && director.Campaign.Data.sanity < 40 && !director.ritual.Exposed;
            foreach(var echo in director.world.fearEchoes)
                echo.SetActive(echoes && Vector3.Distance(echo.transform.position,director.player.transform.position)>8);
            director.world.southSmoke.SetActive(director.ritual.Active && director.CurrentZone=="SOUTH" && LayarTerakhir.Core.Phase4Ids.LocalHits(director.Campaign.Data)==3 && !director.Completed("P4_SOUTH_OFF"));
        }
        private void OnDestroy() { if (instance) Destroy(instance); }
    }
}
