using System.Collections.Generic;
using LayarTerakhir.Core;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed partial class Phase4Director
    {
        private float clarityUntil, nextWhisper;
        private readonly Dictionary<Material, Material> avatarMaterials = new Dictionary<Material, Material>();
        private void UpdateSanity(float dt)
        {
            if (Completed("P4_BOSS_WIN")) return;
            bool safe = world.InWard(player.transform.position);
            float rate = !Completed("P4_PENDOPO_REACHED") ? balance.forestDrain : Phase4Ids.LocalHits(Campaign.Data) == 3 && CurrentZone == "SOUTH" ? balance.southDrain : balance.villageDrain;
            float floor = !Completed("P4_PENDOPO_REACHED") ? balance.forestFloor : balance.villageFloor;
            if (!safe && Campaign.Data.sanity > floor) Campaign.Data.sanity = Mathf.Max(floor, Campaign.Data.sanity - rate * dt);
            if (safe && CurrentZone == "SOUTH" && Campaign.Data.sanity < balance.shelterCap)
                Campaign.Data.sanity = Mathf.Min(balance.shelterCap, Campaign.Data.sanity + balance.shelterRate * dt);
            if (Campaign.Data.sanity <= 70 && Elapsed >= nextWhisper)
            {
                nextWhisper = Elapsed + 16; soundscape.Cue("Whisper", world.suro.transform.position);
                Hud.EnqueueSubtitle(Campaign.Data.sanity < 40 ? "Suara dari bambu" : "Bisikan warga", Campaign.Data.sanity < 40 ? "[Suara menyerupai Ibu Raka: Pulang, Nak… disusul suara Damar dari arah berbeda.]" : "[Bisikan menyusup di sela gesekan bambu.]", 4);
            }
            // The accessibility option affects presentation, never input, facts or UV.
            soundscape.SetSanity(Campaign.Data.sanity, Elapsed < clarityUntil);
        }
        private void ApplyAvatarMaterials()
        {
            foreach (var renderer in player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                var changed = renderer.sharedMaterials;
                for (int i = 0; i < changed.Length; i++)
                {
                    var original = changed[i]; if (!original) continue;
                    if (!avatarMaterials.TryGetValue(original, out var material))
                    {
                        material = new Material(original) { name = original.name + "_KarangSetraPresentation" };
                        if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", 0);
                        if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", Mathf.Min(.32f, material.GetFloat("_Smoothness")));
                        avatarMaterials[original] = material;
                    }
                    changed[i] = material;
                }
                renderer.sharedMaterials = changed;
            }
        }
        private void DisposeAvatarMaterials() { foreach (var material in avatarMaterials.Values) if (material) Destroy(material); avatarMaterials.Clear(); }
    }
}
