using UnityEngine;
namespace LayarTerakhir.Phase4
{
    // An authored false branch. Warning precedes nonfatal damage; walking out
    // cancels it. No camera seizure, instant kill or invisible navigation wall.
    public sealed class Phase4ForestHazard : MonoBehaviour
    {
        public Phase4Director director;
        public Bounds danger;
        public GameObject warning;
        public float warningSeconds = 1.2f, sanityLoss = 20;
        private float dwell, ready;
        private void Update()
        {
            if (!director.Ready || !director.ClockRunning) return;
            bool inside = danger.Contains(director.player.transform.position + Vector3.up * .5f) && !director.Completed("P4_BOSS_WIN");
            if (!inside || director.Elapsed < ready) { dwell = 0; warning.SetActive(false); return; }
            if (dwell == 0)
            { warning.SetActive(true); director.soundscape.Cue("EmberWarning", transform.position); director.Hud.EnqueueSubtitle("Raka", "[Tanah memerah; bambu berderak. Ada jalan kembali di belakang.]", 3); }
            dwell += Time.deltaTime;
            if (dwell >= warningSeconds)
            {
                director.Campaign.Data.sanity = Mathf.Max(0, director.Campaign.Data.sanity - sanityLoss);
                director.soundscape.Cue("EmberBurst", transform.position); ready = director.Elapsed + 6; dwell = 0; warning.SetActive(false);
            }
        }
    }
}
