using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LayarTerakhir.Core
{
    public enum EndingType
    {
        None = 0,
        Bad = 1,
        Normal = 2,
        True = 3,
        Secret = 4
    }

    [Serializable]
    public class CampaignData
    {
        public int chapter_id = 0;
        public string checkpoint_id = "PROLOG_KOS_START";
        public List<string> evidence_ids = new List<string>();
        public List<string> relic_ids = new List<string>();
        public List<string> location_seals = new List<string>();
        public List<string> upgrades = new List<string>();
        public List<string> inventory = new List<string>();
        public float battery = 100f;
        public float sanity = 100f;
        public long wallet_legitimate = 0;
        public long wallet_bound = 0;
        public long spent = 0;
        public List<string> secret_items = new List<string>();
        public List<string> secret_notes_read = new List<string>();
        public bool archive_prepared = false;
        public EndingType ending = EndingType.None;
        public int schema_version = 1;
    }

    public class CampaignState : MonoBehaviour
    {
        public static CampaignState Instance { get; private set; }

        public CampaignData Data { get; private set; } = new CampaignData();

        private string SaveDirectory
        {
            get
            {
#if UNITY_EDITOR
                // Headless validation and tests must never overwrite the player's checkpoint.
                string testDirectory = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
                if (!string.IsNullOrEmpty(testDirectory)) return testDirectory;
#endif
                return Application.persistentDataPath;
            }
        }
        private string SaveFilePath => Path.Combine(SaveDirectory, "layar_terakhir_save.json");
        private string TempSaveFilePath => SaveFilePath + ".tmp";
        private string BackupSaveFilePath => SaveFilePath + ".bak";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void StartNewGame()
        {
            Data = new CampaignData();
        }

        public bool HasInventoryItem(string itemId)
        {
            return Data.inventory.Contains(itemId);
        }

        public bool AddInventoryItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || Data.inventory.Contains(itemId))
                return false;

            Data.inventory.Add(itemId);
            return true;
        }

        public void AddBoundDonation(long amount)
        {
            if (amount > 0)
            {
                Data.wallet_bound += amount;
            }
        }

        public bool SaveCheckpointAtomic(string checkpointId)
        {
            string previousCheckpoint = Data.checkpoint_id;
            try
            {
                if (string.IsNullOrWhiteSpace(checkpointId)) return false;
                Data.checkpoint_id = checkpointId;
                if (!IsValid(Data))
                {
                    Data.checkpoint_id = previousCheckpoint;
                    return false;
                }
                string json = JsonUtility.ToJson(Data, true);
                Directory.CreateDirectory(SaveDirectory);
                using (var stream = new FileStream(TempSaveFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush(true);
                }

                if (File.Exists(SaveFilePath))
                {
                    File.Replace(TempSaveFilePath, SaveFilePath, BackupSaveFilePath);
                }
                else File.Move(TempSaveFilePath, SaveFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Data.checkpoint_id = previousCheckpoint;
                Debug.LogError($"[CampaignState] Gagal menyimpan checkpoint secara atomik: {ex.Message}");
                return false;
            }
        }

        public bool LoadCheckpoint()
        {
            try
            {
                if (!File.Exists(SaveFilePath) || !TryReadCheckpoint(SaveFilePath, out CampaignData loaded))
                    return false;
                Data = loaded;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CampaignState] File simpan rusak atau gagal dimuat: {ex.Message}");
                return false;
            }
        }

        // Explicit recovery keeps the caller in control of accepting an older snapshot.
        public bool LoadBackupCheckpoint()
        {
            try
            {
                if (!File.Exists(BackupSaveFilePath) || !TryReadCheckpoint(BackupSaveFilePath, out var loaded))
                    return false;
                Data = loaded;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CampaignState] Snapshot cadangan gagal dimuat: {ex.Message}");
                return false;
            }
        }

        private static bool TryReadCheckpoint(string path, out CampaignData loaded)
        {
            loaded = new CampaignData { schema_version = 0 };
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), loaded);
            return IsValid(loaded);
        }

        private static bool IsValid(CampaignData data)
        {
            return data != null && data.schema_version == 1 && data.chapter_id >= 0 &&
                !string.IsNullOrWhiteSpace(data.checkpoint_id) &&
                data.battery >= 0f && data.battery <= 100f && data.sanity >= 0f && data.sanity <= 100f &&
                data.wallet_legitimate >= 0 && data.wallet_bound >= 0 && data.spent >= 0 &&
                Enum.IsDefined(typeof(EndingType), data.ending) &&
                ValidIds(data.inventory) && ValidIds(data.evidence_ids) && ValidIds(data.relic_ids) &&
                ValidIds(data.location_seals) && ValidIds(data.upgrades) && ValidIds(data.secret_items) &&
                ValidIds(data.secret_notes_read);
        }

        private static bool ValidIds(List<string> ids)
        {
            if (ids == null) return false;
            var unique = new HashSet<string>();
            foreach (string id in ids)
                if (string.IsNullOrWhiteSpace(id) || !unique.Add(id)) return false;
            return true;
        }
    }
}
