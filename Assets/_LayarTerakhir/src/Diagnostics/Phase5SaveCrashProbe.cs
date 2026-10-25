#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Phase5;
using UnityEngine;
namespace LayarTerakhir.Diagnostics
{
    // Kills only this explicitly launched isolated diagnostic player, never a user session.
    public sealed class Phase5SaveCrashProbe : MonoBehaviour
    {
        private static string folder, point, action;
        private static bool verify;
        private static string Arg(string name) { var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,name);return i>=0&&i+1<a.Length?a[i+1]:null; }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Install()
        {
            folder=Arg("-phase5Probe"); if(folder==null) return;
            folder=Path.GetFullPath(folder); if(!folder.Contains("/Builds/")&&!folder.Contains("\\Builds\\")) throw new InvalidOperationException("Crash probes require an isolated Builds output");
            point=Arg("-phase5CrashPoint"); action=Arg("-phase5CrashAction"); verify=Arg("-phase5Verify")=="yes";
            if((point!="temp-flushed"&&point!="replaced")||(action!="core"&&action!="ending"))throw new InvalidOperationException("Invalid crash probe");
            Directory.CreateDirectory(folder);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            if(!verify)File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),JsonUtility.ToJson(Phase5ReviewFixture.Boundary(),true));
            var go=new GameObject("ExplicitPhase5SaveCrashProbe");DontDestroyOnLoad(go);go.AddComponent<Phase5SaveCrashProbe>();Application.runInBackground=true;
        }
        private IEnumerator Start()
        {
            while(!CampaignState.Instance)yield return null;
            try { Run(); } catch(Exception e) { File.WriteAllText(Path.Combine(folder,"probe-error.txt"),e.ToString());Application.Quit(1); }
        }
        private void Run()
        {
            var c=CampaignState.Instance;Phase5ReviewFixture.Require(c.LoadCheckpoint());
            if(verify)
            {
                bool after=point=="replaced";var baseline=JsonUtility.FromJson<CampaignData>(File.ReadAllText(Path.Combine(folder,"baseline.json")));
                bool ok=action=="core" ? Phase5Ids.CoreCount(c.Data)==(after?1:0)&&c.Data.battery==baseline.battery-(after?15:0)&&c.Data.checkpoint_id==(after?"PHASE5_CP7":"PHASE5_CP6") :
                    c.Data.ending==(after?EndingId.TRUE:EndingId.NONE)&&c.Data.checkpoint_id=="PHASE5_CP8"&&Phase5Ids.CapacitorPercent(c.Data)==(after?0:5)&&c.Data.battery==baseline.battery;
                File.WriteAllText(Path.Combine(folder,"verified.json"),"{\"passed\":"+ok.ToString().ToLowerInvariant()+",\"coreCount\":"+Phase5Ids.CoreCount(c.Data)+",\"ending\":\""+c.Data.ending+"\",\"checkpoint\":\""+c.Data.checkpoint_id+"\"}\n");Application.Quit(ok?0:1);return;
            }
            Phase5ReviewFixture.Prepare(c,action=="ending");
            if(action=="ending")Phase5ReviewFixture.Require(Phase5Campaign.HelpDamar(c));
            c.Data.battery=60;Phase5ReviewFixture.Require(c.SaveCheckpointAtomic(c.Data.checkpoint_id));
            File.WriteAllText(Path.Combine(folder,"baseline.json"),JsonUtility.ToJson(c.Data,true));
            CampaignState.SaveAtomicFaultProbe=stage=>
            {
                if(stage!=point)return;
                File.WriteAllText(Path.Combine(folder,"kill-reached.txt"),stage+" "+action);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };
            if(action=="core")Phase5Campaign.RecordShot(c,c.Data.battery,c.Data.battery-15,1);else Phase5Campaign.LockEnding(c,EndingId.TRUE);
            throw new InvalidOperationException("Fault probe did not terminate this isolated process");
        }
    }
}
#endif
