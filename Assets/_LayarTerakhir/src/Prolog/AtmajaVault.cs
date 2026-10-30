using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    // Runs before the reused FPS controller, so contextual SPACE does not also apply a jump impulse.
    [DefaultExecutionOrder(-100)]
    public sealed class AtmajaVault:MonoBehaviour
    {
        public AtmajaDirector director;
        private void Update()
        {
            if(!director||!director.player||director.Busy||!GameInputConfig.Instance||HudController.Instance.IsModalOpen||UiFocus.HardModal||UiFocus.Cinematic)return;
            var player=director.player;var eye=player.GetComponent<InteractionInteractor>().playerCamera;
            var target=Vector3.Lerp(director.vaultOutside.position,director.vaultInside.position,.65f)+Vector3.up*1.3f;
            bool close=Vector3.Distance(player.transform.position,director.vaultOutside.position)<1.5f;
            bool aiming=Vector3.Dot(eye.transform.forward,(target-eye.transform.position).normalized)>.6f;
            if(!close||!aiming)return;
            if(GameInputConfig.Instance.GetActionDown(GameAction.Vault)&&director.CanVault())director.StartCoroutine(director.Vault());
        }
    }
}
