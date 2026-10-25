#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Phase5;
using UnityEngine;
namespace LayarTerakhir.Diagnostics
{
    public static class Phase5ReviewFixture
    {
        public static CampaignData Boundary()
        {
            var data = Resources.Load<TextAsset>("Phase5ReviewBoundary");
            if (!data) throw new InvalidOperationException("Missing actual Phase4 boundary review fixture");
            return JsonUtility.FromJson<CampaignData>(data.text);
        }
        // Diagnostic fixtures only; every checkpoint still passes the existing validator.
        public static void Prepare(CampaignState c, bool final)
        {
            Require(Phase5Campaign.Enter(c)); Require(Phase5Campaign.EnterSectors(c));
            Require(Phase5Campaign.ClearSector(c,1)); Require(Phase5Campaign.EnsureVessel(c));
            Require(Phase5Campaign.Refill(c)); Require(Phase5Campaign.Pour(c));
            Require(Phase5Campaign.ClearSector(c,2)); Require(Phase5Campaign.ClearSector(c,3));
            Require(Phase5Campaign.Receipt(c,"P5_CONTRACT_THERMO_OK")); Require(Phase5Campaign.Receipt(c,"P5_CONTRACT_UV_OK"));
            foreach (var id in Phase5Ids.Evidence) Require(Phase5Campaign.Evidence(c,id));
            Require(Phase5Campaign.ConfirmJournal(c)); Require(Phase5Campaign.BeginArena(c));
            if (!final) return;
            c.Data.battery = 100;
            for(int i=1;i<=2;i++) Require(Phase5Campaign.RecordShot(c,c.Data.battery,c.Data.battery-15,i));
            for(int i=1;i<=3;i++) Require(Phase5Campaign.CutCable(c,i));
            Require(Phase5Campaign.RecordShot(c,c.Data.battery,c.Data.battery-15,3)); Require(Phase5Campaign.EnterFinal(c));
        }
        public static void Require(bool ok) { if (!ok) throw new InvalidOperationException("Diagnostic fixture rejected by existing campaign transaction"); }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstallReview()
        {
            if (Array.IndexOf(Environment.GetCommandLineArgs(),"-phase5Review")<0) return;
            string dir=Path.Combine(Application.persistentDataPath,"Phase5Review"); Directory.CreateDirectory(dir);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",dir);
            string path=Path.Combine(dir,"layar_terakhir_save.json");
            if(!File.Exists(path)) File.WriteAllText(path,JsonUtility.ToJson(Boundary(),true));
        }
    }
}
#endif
