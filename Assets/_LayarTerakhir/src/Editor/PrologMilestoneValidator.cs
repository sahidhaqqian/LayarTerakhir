#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    public static class PrologMilestoneValidator
    {
        public static void RunValidation()
        {
            int failures = 0;
            string previousSaveDirectory = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            string validationDirectory = Path.Combine(Path.GetTempPath(), "LayarTerakhir-Validation-" + Guid.NewGuid());
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", validationDirectory);
            Debug.Log("=== [VALIDATOR] MEMULAI VALIDASI MILESTONE 1 LAYAR TERAKHIR ===");

            try
            {
                if (!File.Exists(PrologKosSceneBuilder.ScenePath))
                {
                    Debug.Log("[VALIDATOR] Scene belum ada, menjalankan PrologKosSceneBuilder.BuildSceneHeadless()...");
                    PrologKosSceneBuilder.BuildSceneHeadless();
                }

                Check(File.Exists(PrologKosSceneBuilder.ScenePath), "File scene Prolog_KamarKos.unity tersedia", ref failures);

                var scene = EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath, OpenSceneMode.Single);
                Check(scene.IsValid() && scene.isLoaded, "Scene Prolog_KamarKos.unity berhasil dibuka", ref failures);

                var inputCfg = UnityEngine.Object.FindFirstObjectByType<GameInputConfig>();
                Check(inputCfg != null, "GameInputConfig ditemukan di scene", ref failures);

                var campaign = UnityEngine.Object.FindFirstObjectByType<CampaignState>();
                Check(campaign != null, "CampaignState ditemukan di scene", ref failures);

                var hud = UnityEngine.Object.FindFirstObjectByType<HudController>();
                Check(hud != null && hud.objectiveText != null && hud.padlockPanel != null, "HudController beserta panel runtime UI terkonfigurasi", ref failures);

                var fpp = UnityEngine.Object.FindFirstObjectByType<FirstPersonController>();
                Check(fpp != null && fpp.cameraPivot != null, "FirstPersonController dan CameraPivot terhubung", ref failures);

                var interactor = UnityEngine.Object.FindFirstObjectByType<InteractionInteractor>();
                Check(interactor != null && interactor.playerCamera != null, "InteractionInteractor dan MainCamera terhubung", ref failures);

                var phone = UnityEngine.Object.FindFirstObjectByType<SmartphoneController>();
                Check(phone != null && phone.handPhoneMeshRoot != null, "SmartphoneController dan rig tangan terhubung", ref failures);

                var director = UnityEngine.Object.FindFirstObjectByType<KamarKosPrologDirector>();
                Check(director != null, "KamarKosPrologDirector ditemukan di scene", ref failures);
                Check(KamarKosPrologDirector.DrawerPasscode == "148", "Kode gembok laci sesuai GDD (148)", ref failures);

                string[] requiredIds =
                {
                    "PC_MONITOR",
                    "SURAT_KOS",
                    "SMARTPHONE_BED",
                    "STICKY_NOTE",
                    "KALENDER_DINDING",
                    "LACI_MEJA",
                    "JAKET_PINTU",
                    "PINTU_KOS"
                };

                var allInteractables = UnityEngine.Object.FindObjectsByType<KosInteractableItem>(FindObjectsSortMode.None);
                var foundIds = new HashSet<string>();
                foreach (var item in allInteractables)
                {
                    if (item != null && item.GetComponent<Collider>() != null && item.director == director)
                    {
                        foundIds.Add(item.itemId);
                    }
                }

                foreach (string reqId in requiredIds)
                {
                    Check(foundIds.Contains(reqId), $"Interactable '{reqId}' memiliki Collider dan terhubung ke Director", ref failures);
                }

                GameObject floor = GameObject.Find("Floor_4x3m");
                Check(
                    floor != null &&
                    Mathf.Abs(floor.transform.localScale.x-CompactKosPolish.Width)<.001f &&
                    Mathf.Abs(floor.transform.localScale.z-CompactKosPolish.Depth)<.001f,
                    "Dimensi lantai Kamar Kos sesuai revisi developer (5.0x3.8 meter)",
                    ref failures
                );

                if (campaign != null)
                {
                    campaign.StartNewGame();
                    campaign.AddInventoryItem("KUNCI_MOTOR");
                    bool saved = campaign.SaveCheckpointAtomic("VALIDATOR_TEST_CHECKPOINT");
                    bool loaded = campaign.LoadCheckpoint();
                    Check(
                        saved && loaded &&
                        campaign.Data.checkpoint_id == "VALIDATOR_TEST_CHECKPOINT" &&
                        campaign.HasInventoryItem("KUNCI_MOTOR"),
                        "Penyimpanan dan pemulihan checkpoint JSON (round trip) berfungsi",
                        ref failures
                    );
                }
            }
            catch (Exception ex)
            {
                failures++;
                Debug.LogError($"[VALIDATOR] [FAIL] Exception saat validasi: {ex}");
            }
            finally
            {
                Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previousSaveDirectory);
                if (Directory.Exists(validationDirectory)) Directory.Delete(validationDirectory, true);
            }

            if (failures == 0)
            {
                Debug.Log("=== [VALIDATOR] SEMUA PEMERIKSAAN LULUS (0 GAGAL) ===");
                EditorApplication.Exit(0);
            }
            else
            {
                Debug.LogError($"=== [VALIDATOR] VALIDASI GAGAL DENGAN {failures} KESALAHAN ===");
                EditorApplication.Exit(1);
            }
        }

        private static void Check(bool condition, string description, ref int failures)
        {
            if (condition)
            {
                Debug.Log($"[VALIDATOR] [PASS] {description}");
            }
            else
            {
                failures++;
                Debug.LogError($"[VALIDATOR] [FAIL] {description}");
            }
        }
    }
}
#endif
