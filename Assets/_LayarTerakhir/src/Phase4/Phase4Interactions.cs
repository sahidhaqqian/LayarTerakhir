using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed partial class Phase4Director
    {
        public bool ActionBusy { get; private set; }
        public int WheelPosition { get; private set; }
        private float hideHeight;
        private Vector3 hideCentre, hideEye;
        public IInteractable ResolveContextInteraction() => Hidden ? hidingItem : null;
        public bool CanInteract(Phase4WorldItem item) => Ready && !Dead && !ActionBusy && !RecordingEVP &&
            !GameInputConfig.Instance.GetActionHeld(GameAction.ModeEVP) && !GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen) &&
            !UiFocus.HardModal && !UiFocus.Cinematic && !Hud.IsModalOpen && (!Hidden || item == hidingItem);
        public string Prompt(Phase4WorldItem item)
        {
            if (Hidden && item == hidingItem) return "Keluar perlahan dari persembunyian";
            if (item.id.StartsWith("TORCH_")) return State.water_units == 0 ? "Kendi kosong — isi di Sumur / saluran ruwatan" : "Siram dengan Kendi • air " + State.water_units + "/3";
            if (item.id == "WHEEL") return "Putar roda ke tanda mata tertutup • posisi " + (WheelPosition + 1) + "/3";
            var prompt=item.prompt;
            foreach(var binding in new[]{(GameAction.Interact,"E"),(GameAction.ToggleScreen,"F"),(GameAction.UseIncense,"H")})
                prompt=System.Text.RegularExpressions.Regex.Replace(prompt,@"\b"+binding.Item2+@"\b",GameInputConfig.Instance.GetActionLabel(binding.Item1));
            return prompt;
        }
        public void Interact(Phase4WorldItem item)
        {
            if (!CanInteract(item)) return;
            string id = item.id;
            if (id.StartsWith("TORCH_")) { ritual.Pour(item.GetComponent<Phase4Torch>()); return; }
            if (id.StartsWith("ALTAR_"))
            {
                int i = id[id.Length - 1] - '0';
                if (!ritual.Active && i >= 0 && i < 3 && Campaign.Data.inventory.Contains(Phase4Ids.Relics[i])) { Preview[i] = !Preview[i]; RefreshWorld(); }
                else Hud.ShowBroadcastNotification("Pusaka dudukan ini belum dibawa atau telah dipersembahkan.", 3);
                return;
            }
            if (id.StartsWith("CHARGE_"))
            { StartCoroutine(HoldAction(item, balance.chargeSeconds, () => { bool ok = Phase4Campaign.Recover(Campaign, id, SafeAnchor, true, balance.recoveryCooldown); if (ok) Phone.RestoreBattery(Campaign.Data.battery); return ok; }, "ENGKOL DINAMO")); return; }
            if (id.StartsWith("SHRINE_"))
            { StartCoroutine(HoldAction(item, balance.shrineSeconds, () => Phase4Campaign.Recover(Campaign, id, SafeAnchor, false, 30), "TENANGKAN DIRI DI BATU RUWATAN")); return; }
            if (id.StartsWith("REFILL_"))
            { StartCoroutine(HoldAction(item, balance.refillSeconds, () => Phase4Campaign.Refill(Campaign, SafeAnchor), "ISI KENDI DARI SUMUR")); return; }
            if (id.StartsWith("HIDE_")) { ToggleHide(item); return; }
            switch (id)
            {
                case "START_LIVE": StartLive(); break;
                case "KENDI": TakeRelic(Phase4Ids.Kendi); break;
                case "BLADE": TakeRelic(Phase4Ids.Blade); break;
                case "SOIL": TakeRelic(Phase4Ids.Soil); break;
                case "GLASSES": if (Receipt("P4_DAMAR_GLASSES", "PENDOPO")) Hud.EnqueueSubtitle("Raka", "Kacamata Damar... dia beneran lewat sini.", 5); break;
                case "BOOK": Read("P4_DOC_RAJAH_4", "Kitab Rajah Mbah Suro — Bagian Keempat", Phase4Content.Book); break;
                case "DAMAR_NOTE": Read("P4_DOC_DAMAR_LAST", "Sobekan Buku Catatan Terakhir Damar", Phase4Content.DamarNote); break;
                case "RITUAL": BeginRitual(); break;
                case "WHEEL":
                    if (!Completed("P4_SECRET_KNOCK_VERIFIED") || !Completed("P4_SECRET_EYE_VERIFIED")) { Hud.ShowBroadcastNotification("Dengarkan relung dengan EVP, lalu periksa simbolnya memakai UV.", 4); break; }
                    WheelPosition = (WheelPosition + 1) % 3;
                    if (WheelPosition == 2 && Receipt("P4_SECRET_WHEEL_ALIGNED", "WEST")) soundscape.Cue("Drawer", item.transform.position);
                    break;
                case "MIRROR":
                    if (Phase4Campaign.CollectMirror(Campaign, "WEST")) { RefreshWorld(); Hud.ShowBroadcastNotification("KEPING CERMIN SAKSI TERSIMPAN • " + ArchiveStatus(), 6); }
                    else SaveError();
                    break;
                case "PORTAL": OpenExitPanel(); break;
                case "BOUNDARY": if (Completed("P4_COMPLETE")) ShowBoundary(); else OpenExitPanel(); break;
            }
        }
        private void TakeRelic(string id)
        {
            if (Phase4Campaign.Relic(Campaign, id, SafeAnchor)) { RefreshWorld(); Hud.ShowBroadcastNotification(Phase4Content.Label(id) + " — tersimpan", 4); }
            else SaveError();
        }
        private bool Near(Phase4WorldItem item)
        {
            var interactor = player.GetComponent<InteractionInteractor>();
            return object.ReferenceEquals(interactor.CurrentTarget, item) ||
                Vector3.Distance(player.transform.position + Vector3.up, item.transform.position) < interactor.interactionDistance && ClearLine(player.transform.position + Vector3.up, item.transform.position, item.transform);
        }
        private IEnumerator HoldAction(Phase4WorldItem item, float seconds, Func<bool> complete, string label)
        {
            if (ActionBusy) yield break; ActionBusy = true; float progress = 0;
            while (!Dead && Near(item) && GameInputConfig.Instance.GetActionHeld(GameAction.Interact) && progress < seconds)
            {
                if (ClockRunning) progress += Time.deltaTime;
                Hud.SetInteractionPrompt(label + " • " + progress.ToString("0.0") + "/" + seconds.ToString("0") + "s");
                yield return null;
            }
            if (!Dead && progress >= seconds)
            {
                if (complete()) { RefreshWorld(); soundscape.Cue("Water", item.transform.position); }
                else Hud.ShowBroadcastNotification("Belum ada perubahan. Periksa kapasitas, jeda pemulihan, dan coba lagi bila penyimpanan gagal.", 4);
            }
            ActionBusy = false;
        }
        private void Read(string receipt, string title, string text)
        {
            if (!Receipt(receipt, SafeAnchor, "PHASE4_CP4", d =>
            { if (!d.documents.Exists(x => x.title == title)) d.documents.Add(new PhoneDocument(title, text)); if (!d.inventory.Contains(receipt)) d.inventory.Add(receipt); })) return;
            if (Completed("P4_BOSS_WIN") || world.InWard(player.transform.position)) Hud.OpenDocumentModal(title, text);
            else Hud.ShowBroadcastNotification("Catatan tersimpan. Baca di Jurnal sambil berlindung.", 4);
        }
        private void BeginRitual()
        {
            if (ritual.Active) { ritual.RequestCoreWindow(); return; }
            if (!Preview.All(x => x) || !Phase4Campaign.BeginRitual(Campaign, balance.torchPreset))
            { Hud.ShowBroadcastNotification("Lengkapi tiga bukti, konfirmasi Jurnal, pasang tiga pusaka, lalu pastikan checkpoint tersimpan.", 5); return; }
            ritual.Begin(); RefreshWorld(); soundscape.Cue("Ritual", world.Anchor("PENDOPO").position);
            Hud.EnqueueSubtitle("Mbah Suro", "[Mantra dari bubungan atap. Api menyebar ke empat penjuru.]", 7);
        }
        public void UseIncense()
        {
            if (ActionBusy || RecordingEVP || GameInputConfig.Instance.GetActionHeld(GameAction.ModeEVP) || GameInputConfig.Instance.GetActionDown(GameAction.Interact) || GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen)) return;
            StartCoroutine(BurnIncense());
        }
        private IEnumerator BurnIncense()
        {
            if (!Campaign.Data.inventory.Any(Phase4Ids.IsIncense) || Elapsed < State.incense_ready_at || Campaign.Data.sanity >= 100)
            { Hud.ShowBroadcastNotification("Kemenyan belum tersedia atau belum diperlukan. Batu Ruwatan menyediakan pemulihan gratis.", 4); yield break; }
            ActionBusy = true; float t = 0;
            Hud.ShowBroadcastNotification("Menyalakan kemenyan…",2);
            while (!Dead && t < 2) { if (ClockRunning) t += Time.deltaTime; yield return null; }
            if (!Dead && Phase4Campaign.Incense(Campaign, SafeAnchor))
            { clarityUntil = Elapsed + 20; soundscape.Cue("Incense", player.transform.position); Hud.ShowBroadcastNotification("Kemenyan terbakar. Kewarasan pulih.", 4); }
            else if (!Dead) SaveError();
            ActionBusy = false;
        }
        private void ToggleHide(Phase4WorldItem item)
        {
            var capsule = player.GetComponent<CharacterController>();
            if (!Hidden)
            {
                hideEntrance = player.transform.position; hideRotation = player.transform.rotation; hidingItem = item;
                hideHeight = capsule.height; hideCentre = capsule.center; hideEye = player.cameraPivot.localPosition;
                var point = item.transform.Find("HideInside");
                capsule.enabled = false; if (point) player.transform.SetPositionAndRotation(point.position, point.rotation);
                capsule.height = .9f; capsule.center = Vector3.up * .45f; capsule.enabled = true;
                player.cameraPivot.localPosition = Vector3.up * .78f; Hidden = true; player.MovementLocked = true;
                player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = true;
                Hud.ShowBroadcastNotification("Berlindung bukan menghilang. Matikan layar dan putus pandangan api; "+GameInputConfig.Instance.GetActionLabel(GameAction.Interact)+" untuk keluar.", 5);
            }
            else
            {
                capsule.enabled = false; player.transform.SetPositionAndRotation(hideEntrance, hideRotation);
                capsule.height = hideHeight; capsule.center = hideCentre; capsule.enabled = true;
                player.cameraPivot.localPosition = hideEye; Hidden = false; player.MovementLocked = false;
                player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = false; hidingItem = null;
            }
            player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
    }
}
