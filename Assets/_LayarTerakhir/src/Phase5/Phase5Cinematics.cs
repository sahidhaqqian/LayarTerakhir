using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    [Serializable] public sealed class Phase5EndingShot
    {
        public EndingId ending;
        public Transform from, to, look;
        public string speaker, subtitle;
        public float seconds = 7;
        public int epilogueSet = -1;
    }
    public sealed partial class Phase5Director
    {
        public Phase5EndingShot[] endingShots;
        public TextAsset credits;
        private IEnumerator PlayEndingShots(EndingId ending)
        {
            phoneView.GlitchStrength = 0;
            if (ending != EndingId.NORMAL) phoneView.CaptureFlash();
            if (ending == EndingId.BAD) Hud.EnqueueSubtitle("Damar", "Ka...?", 4);
            soundscape.Ending(ending);
            foreach (var shot in endingShots.Where(s => s.ending == ending))
            {
                Hud.SetFadeAlpha(1); yield return new WaitForSeconds(.3f);
                for (int i = 0; i < world.epilogueSets.Length; i++) world.epilogueSets[i].SetActive(i == shot.epilogueSet);
                world.epilogueRoot.SetActive(shot.epilogueSet >= 0);
                var rig = player.GetComponent<LayarTerakhir.Player.ShoulderCameraRig>();
                rig.CinematicVisible = !(ending == EndingId.NORMAL && shot.epilogueSet >= 0);
                if (shot.epilogueSet == 1)
                {
                    GetComponent<Phase5Lighting>()?.Apply(true);
                    RenderSettings.fogColor = new Color(.28f,.31f,.32f); RenderSettings.fogDensity = .006f;
                    RenderSettings.ambientSkyColor = new Color(.48f,.47f,.43f); RenderSettings.ambientEquatorColor = new Color(.3f,.32f,.33f);
                    foreach(var light in FindObjectsByType<Light>(FindObjectsSortMode.None)) if(light.type==LightType.Directional)
                    { light.color=new Color(1,.8f,.62f);light.intensity=1.1f;light.transform.rotation=Quaternion.Euler(18,-70,0); }
                }
                if (ending == EndingId.BAD && shot.epilogueSet < 0)
                    world.damar.transform.position = new Vector3(0,-1.2f,220);
                if (ending == EndingId.NORMAL && shot.epilogueSet < 0)
                    world.damar.transform.position = world.portal.position + Vector3.right * 1.5f;
                if (shot.epilogueSet >= 0)
                {
                    world.bossRoot.SetActive(false); world.cage.SetActive(false);
                    // Epilogue blocking uses new licensed sets. Campaign wallets,
                    // handset inventory and sealed locations are never rewritten.
                    player.transform.position = world.epilogueSets[shot.epilogueSet].transform.position + new Vector3(0, .05f, 1.5f);
                }
                if (shot.from && shot.look) phoneView.playerCamera.transform.SetPositionAndRotation(shot.from.position, Quaternion.LookRotation(shot.look.position - shot.from.position));
                Hud.SetFadeAlpha(0);
                Hud.EnqueueSubtitle(shot.speaker, shot.subtitle, shot.seconds);
                float t = 0;
                while (t < shot.seconds)
                {
                    if (Time.timeScale > 0)
                    {
                        t += Time.deltaTime;
                        if (shot.from && shot.to && shot.look)
                        {
                            Vector3 p = Vector3.Lerp(shot.from.position, shot.to.position, Mathf.SmoothStep(0, 1, t / shot.seconds));
                            phoneView.playerCamera.transform.SetPositionAndRotation(p, Quaternion.LookRotation(shot.look.position - p));
                        }
                    }
                    yield return null;
                }
            }
            Hud.SetFadeAlpha(1); yield return new WaitForSeconds(.6f);
        }
        private void ShowCredits(EndingId ending)
        {
            CloseOverlay(); UiFocus.Cinematic = false; UiFocus.Clock = true; player.MovementLocked = true;
            overlay = UiWidgets.Panel("P5Credits", Hud.GetComponentInChildren<Canvas>().transform, Vector2.zero, Vector2.one, new Color(.008f, .011f, .016f, 1));
            string title = ending == EndingId.BAD ? "TUAN RUMAH BARU" : ending == EndingId.NORMAL ? "SIARAN TERPUTUS" : ending == EndingId.TRUE ? "JEPRETAN TERAKHIR" : "SAKSI TANPA LAYAR";
            UiWidgets.Text("Title", overlay.transform, "L A Y A R   T E R A K H I R\n\n" + title, new Vector2(.1f, .71f), new Vector2(.9f, .96f), 36);
            UiWidgets.ScrollText("Credits", overlay.transform, "Ending final tersimpan • PHASE5_CP9\n\n" + (credits ? credits.text : "Layar Terakhir\nDesain cerita: desainer proyek\nTerima kasih kepada seluruh pembuat aset berlisensi yang dicatat dalam paket build."), new Vector2(.2f, .2f), new Vector2(.8f, .69f), 23);
            UiWidgets.Button("Menu", overlay.transform, "MENU UTAMA", new Vector2(.3f, .07f), new Vector2(.7f, .16f), () => { CloseOverlay(); GameShell.Instance.OpenMain(); });
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
    }
}
