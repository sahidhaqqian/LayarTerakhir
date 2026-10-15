#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    public static class GoldenLinuxBuild
    {
        public static void Build()
        {
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone,ScriptingImplementation.Mono2x);
            PlayerSettings.defaultScreenWidth=1280;PlayerSettings.defaultScreenHeight=720;PlayerSettings.defaultIsNativeResolution=false;
            PlayerSettings.fullScreenMode=FullScreenMode.Windowed;
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{PrologKosSceneBuilder.ScenePath},
                target=BuildTarget.StandaloneLinux64,locationPathName="Builds/LinuxAudit/LayarTerakhir.x86_64",options=BuildOptions.Development});
            Directory.CreateDirectory("Audit");File.WriteAllText("Audit/golden-linux-build.json",JsonUtility.ToJson(new Record{result=report.summary.result.ToString(),
                utc=DateTime.UtcNow.ToString("o"),errors=report.summary.totalErrors,warnings=report.summary.totalWarnings,bytes=report.summary.totalSize},true)+"\n");
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Linux evidence player build failed");
        }
        [Serializable] private sealed class Record{public string result,utc;public int errors,warnings;public ulong bytes;}
    }
}
#endif
