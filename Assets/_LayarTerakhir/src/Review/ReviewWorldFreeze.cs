#if UNITY_EDITOR || DEVELOPMENT_BUILD
using LayarTerakhir.Core;
using UnityEngine;

namespace LayarTerakhir.Review
{
    // F11 world freeze: timeScale 0 pauses NPCs, ghosts, animators and physics.
    // The review camera runs on unscaled time and stays responsive. Freeze never
    // survives a scene load, so director boot sequences cannot stall.
    public sealed class ReviewWorldFreeze : MonoBehaviour
    {
        public bool Frozen { get; private set; }
        private float previousScale = 1f;

        public bool Toggle()
        {
            if (Frozen) Unfreeze();
            else Freeze();
            return Frozen;
        }

        public bool Freeze()
        {
            if (Frozen) return true;
            if (UiFocus.Menu || UiFocus.HardModal)
            {
                var mode = GetComponent<DeveloperReviewMode>();
                if (mode) mode.Toast("Tutup menu dulu sebelum freeze (F11).");
                return false;
            }
            previousScale = Time.timeScale;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            Frozen = true;
            return true;
        }

        public void Unfreeze()
        {
            if (!Frozen) return;
            Frozen = false;
            // The pause menu owns audio/time while open; otherwise restore ours.
            if (UiFocus.Menu) return;
            Time.timeScale = previousScale > 0f ? previousScale : 1f;
            AudioListener.pause = false;
        }

        // Scene loads always release the freeze so boot coroutines can run.
        public void ReleaseForSceneLoad(string sceneName)
        {
            if (!Frozen) return;
            Frozen = false;
            Time.timeScale = previousScale > 0f ? previousScale : 1f;
            AudioListener.pause = false;
            var mode = GetComponent<DeveloperReviewMode>();
            if (mode) mode.Toast("Freeze dilepas otomatis saat pindah scene (" + sceneName + ").");
        }

        private void Update()
        {
            // Reassert against drivers that poke timeScale, but never fight the menu.
            if (Frozen && !UiFocus.Menu && Time.timeScale != 0f)
                Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            if (Frozen) Unfreeze();
        }
    }
}
#endif
