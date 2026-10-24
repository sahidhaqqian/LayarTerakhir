using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LayarTerakhir.Core
{
    [Serializable]
    public sealed class PhoneDocument
    {
        public string title, body;
        public PhoneDocument(string title, string body) { this.title = title; this.body = body; }
    }
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
        public List<PhoneDocument> documents = new List<PhoneDocument>();
        public bool archive_prepared = false;
        public EndingType ending = EndingType.None;
        public int schema_version = 4;
        public Phase1Data phase1 = new Phase1Data();
        public Phase2Data phase2 = new Phase2Data();
        public Phase3Data phase3 = new Phase3Data();
    }

    public class CampaignState : MonoBehaviour
    {
        public static CampaignState Instance { get; private set; }

        public CampaignData Data { get; private set; } = new CampaignData();
        private object checkpointAttemptOwner;
        public bool WritesBlocked => checkpointAttemptOwner != null;

        public bool BeginCheckpointAttempt(object owner)
        {
            if (owner == null || checkpointAttemptOwner != null) return false;
            checkpointAttemptOwner = owner; return true;
        }
        public void EndCheckpointAttempt(object owner)
        { if (ReferenceEquals(checkpointAttemptOwner, owner)) checkpointAttemptOwner = null; }

        // Mutate, validate and replace disk once, or restore *all* campaign fields.
        public bool TryCommit(string checkpointId, Action<CampaignData> mutate, object attemptOwner = null)
        {
            if (WritesBlocked && !ReferenceEquals(checkpointAttemptOwner, attemptOwner)) return false;
            string before = JsonUtility.ToJson(Data);
            try
            {
                mutate?.Invoke(Data);
                if (SaveAtomic(checkpointId)) return true;
            }
            catch (Exception ex) { Debug.LogWarning("[CampaignState] Transaksi dibatalkan: " + ex.Message); }
            JsonUtility.FromJsonOverwrite(before, Data);
            return false;
        }

        private string SaveDirectory
        {
            get
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
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
            checkpointAttemptOwner = null;
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

        public bool ArchiveDocument(string title, string body, string documentId = null)
        {
            if (WritesBlocked) return false;
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body)) return false;
            if (Data.documents == null) Data.documents = new List<PhoneDocument>();
            bool archived = Data.documents.Exists(d => d.title == title && d.body == body);
            bool receipt = string.IsNullOrEmpty(documentId) || HasInventoryItem(documentId);
            if (archived && receipt) return true;
            var document = new PhoneDocument(title, body);
            if (!archived) Data.documents.Add(document);
            if (!receipt) AddInventoryItem(documentId);
            if (SaveCheckpointAtomic(Data.checkpoint_id)) return true;
            if (!archived) Data.documents.Remove(document);
            if (!receipt) Data.inventory.Remove(documentId);
            return false;
        }

        public bool TryPayFamilyBill(bool hospital)
        {
            if (WritesBlocked) return false;
            string call = hospital ? "EVENT_CALLED_IBU" : "EVENT_CALLED_RATNA";
            string paid = hospital ? "EVENT_PAID_HOSPITAL" : "EVENT_PAID_RENT";
            long amount = hospital ? 1500000 : 3500000;
            if (!HasInventoryItem(call) || HasInventoryItem(paid) || Data.wallet_bound + Data.wallet_legitimate < amount) return false;
            string before = JsonUtility.ToJson(Data);
            long fromBound = Math.Min(amount, Data.wallet_bound);
            Data.wallet_bound -= fromBound;
            Data.wallet_legitimate -= amount - fromBound;
            Data.spent += amount;
            AddInventoryItem(paid);
            string checkpoint = Data.checkpoint_id;
            if (checkpoint == "PROLOG_KOS_START") checkpoint = "PROLOG_KOS_PREPARATION";
            if (SaveCheckpointAtomic(checkpoint)) return true;
            JsonUtility.FromJsonOverwrite(before, Data);
            return false;
        }

        public bool SaveCheckpointAtomic(string checkpointId)
        {
            if (WritesBlocked) return false;
            return SaveAtomic(checkpointId);
        }

        private bool SaveAtomic(string checkpointId)
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
                checkpointAttemptOwner = null;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CampaignState] File simpan rusak atau gagal dimuat: {ex.Message}");
                return false;
            }
        }

        public bool HasValidCheckpoint{get{try{return File.Exists(SaveFilePath)&&TryReadCheckpoint(SaveFilePath,out _);}catch{return false;}}}
        // New Game is an explicit menu action. Preserve the old campaign, including a corrupt slot.
        public bool BeginNewCampaignPreservingPrevious()
        {
            var previous=Data;
            try
            {
                if(File.Exists(SaveFilePath))
                {
                    var archive=Path.Combine(SaveDirectory,"ArchivedCampaigns");Directory.CreateDirectory(archive);
                    File.Copy(SaveFilePath,Path.Combine(archive,"campaign-"+DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")+"-"+Guid.NewGuid().ToString("N")+".json"));
                }
                StartNewGame();if(SaveCheckpointAtomic("PROLOG_KOS_START"))return true;
            }
            catch(Exception e){Debug.LogWarning("New Game tidak dapat mengarsipkan save: "+e.Message);}
            Data=previous;return false;
        }

        // Explicit recovery keeps the caller in control of accepting an older snapshot.
        public bool LoadBackupCheckpoint()
        {
            try
            {
                if (!File.Exists(BackupSaveFilePath) || !TryReadCheckpoint(BackupSaveFilePath, out var loaded))
                    return false;
                Data = loaded;
                checkpointAttemptOwner = null;
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
            if (loaded.schema_version == 1)
            {
                // Schema 1 never stored a stage. Leave every P0 inventory/event flag intact.
                loaded.phase1 = new Phase1Data();
                loaded.schema_version = 2;
            }
            if (loaded.schema_version == 2)
            {
                loaded.phase2 = new Phase2Data();
                loaded.schema_version = 3;
            }
            if (loaded.schema_version == 3)
            {
                loaded.phase3 = new Phase3Data();
                loaded.schema_version = 4;
            }
            if (loaded.phase3 != null) Phase3Ids.NormalizeAliases(loaded);
            return IsValid(loaded);
        }

        private static bool IsValid(CampaignData data)
        {
            return data != null && data.schema_version == 4 && data.chapter_id >= 0 &&
                !string.IsNullOrWhiteSpace(data.checkpoint_id) &&
                data.battery >= 0f && data.battery <= 100f && data.sanity >= 0f && data.sanity <= 100f &&
                data.wallet_legitimate >= 0 && data.wallet_bound >= 0 && data.spent >= 0 &&
                Enum.IsDefined(typeof(EndingType), data.ending) &&
                ValidIds(data.inventory) && ValidIds(data.evidence_ids) && ValidIds(data.relic_ids) &&
                ValidIds(data.location_seals) && ValidIds(data.upgrades) && ValidIds(data.secret_items) &&
                ValidIds(data.secret_notes_read) && ValidPhase1(data.phase1) && ValidPhase2(data.phase2) && ValidPhase3(data.phase3);
        }

        private static bool ValidPhase2(Phase2Data p)
        {
            if (p == null || !ValidIds(p.completed_event_ids) || !ValidIds(p.dynamo_full_charge_used_ids) ||
                !float.IsFinite(p.elapsed_gameplay_seconds) || p.elapsed_gameplay_seconds < 0 ||
                string.IsNullOrWhiteSpace(p.safe_anchor) || p.dynamo_emergency_last_uses == null || p.money_ledger == null) return false;
            var ids = new HashSet<string>();
            foreach (var e in p.dynamo_emergency_last_uses)
                if (e == null || string.IsNullOrWhiteSpace(e.id) || !ids.Add(e.id) ||
                    !float.IsFinite(e.at_gameplay_seconds) || e.at_gameplay_seconds < 0 || e.at_gameplay_seconds > p.elapsed_gameplay_seconds) return false;
            ids.Clear();
            foreach (var e in p.money_ledger)
                if (e == null || string.IsNullOrWhiteSpace(e.id) || !ids.Add(e.id) || e.amount <= 0) return false;
            return true;
        }

        private static bool ValidPhase3(Phase3Data p)
        {
            if (p == null || !ValidIds(p.completed_event_ids) || !ValidIds(p.dynamo_full_charge_used_ids) ||
                !float.IsFinite(p.elapsed_gameplay_seconds) || p.elapsed_gameplay_seconds < 0 ||
                Array.IndexOf(Phase3Ids.SafeAnchors,p.safe_anchor)<0 || (p.Completed("P3_DEPART_KOS")&&p.safe_anchor=="HUB") || p.dynamo_emergency_last_uses == null || p.money_ledger == null) return false;
            var ids = new HashSet<string>();
            foreach (var e in p.dynamo_emergency_last_uses)
                if (e == null || string.IsNullOrWhiteSpace(e.id) || !ids.Add(e.id) ||
                    !float.IsFinite(e.at_gameplay_seconds) || e.at_gameplay_seconds < 0 || e.at_gameplay_seconds > p.elapsed_gameplay_seconds) return false;
            ids.Clear();
            foreach (var e in p.money_ledger)
                if (e == null || string.IsNullOrWhiteSpace(e.id) || !ids.Add(e.id) || e.amount <= 0) return false;
            return true;
        }

        private static bool ValidPhase1(Phase1Data p)
        {
            if (p == null || !ValidIds(p.completed_event_ids) || !ValidIds(p.dynamo_full_charge_used_ids) ||
                !Enum.IsDefined(typeof(Phase1Challenge), p.signboard_challenge) ||
                !float.IsFinite(p.elapsed_gameplay_seconds) || p.elapsed_gameplay_seconds < 0 ||
                string.IsNullOrWhiteSpace(p.safe_anchor) || p.dynamo_emergency_last_uses == null || p.money_ledger == null) return false;
            var ids = new HashSet<string>();
            foreach (var entry in p.dynamo_emergency_last_uses)
                if (entry == null || string.IsNullOrWhiteSpace(entry.id) || !ids.Add(entry.id) ||
                    !float.IsFinite(entry.at_gameplay_seconds) || entry.at_gameplay_seconds < 0 || entry.at_gameplay_seconds > p.elapsed_gameplay_seconds) return false;
            ids.Clear();
            foreach (var entry in p.money_ledger)
                if (entry == null || string.IsNullOrWhiteSpace(entry.id) || !ids.Add(entry.id) || entry.amount <= 0) return false;
            return true;
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
