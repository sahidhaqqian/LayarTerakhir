#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    public static class WindowsDevelopmentBuild
    {
        private const string StartupScene = "Assets/_LayarTerakhir/assets/Scenes/Prolog_KamarKos.unity";

        public static void Build()
        {
            const BuildTarget target = BuildTarget.StandaloneWindows64;
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, target))
                throw new BuildFailedException("Windows x86_64 build support is unavailable.");
            if (EditorUserBuildSettings.activeBuildTarget != target)
                throw new BuildFailedException("Launch Unity with -buildTarget Win64.");
            if (!File.Exists(StartupScene))
                throw new BuildFailedException("Startup scene is missing: " + StartupScene);

            var scenes = new[] { StartupScene }
                .Concat(EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path))
                .Distinct().ToArray();
            foreach (string scene in scenes)
                if (!File.Exists(scene))
                    throw new BuildFailedException("Enabled scene is missing: " + scene);

            var configuredScenes = EditorBuildSettings.scenes;
            if (configuredScenes.Length == 0 || !configuredScenes[0].enabled || configuredScenes[0].path != StartupScene)
            {
                EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(StartupScene, true) }
                    .Concat(configuredScenes.Where(scene => scene.path != StartupScene)).ToArray();
            }
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            PlayerSettings.defaultIsNativeResolution = false;
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.resizableWindow=true;PlayerSettings.allowFullscreenSwitch=false;
            EditorUserBuildSettings.selectedStandaloneTarget = target;
            EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;
            EditorUserBuildSettings.development = true;
            AssetDatabase.SaveAssets();

            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string output = Path.Combine(projectRoot, "Builds/Windows/LayarTerakhir.exe");
            string reportPath = Path.Combine(projectRoot, "Builds/Logs/windows-development-build-report.json");
            Directory.CreateDirectory(Path.GetDirectoryName(output));
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));
            Debug.Log("WINDOWS_BUILD Target=" + target + " Backend=Mono2x Options=Development");
            Debug.Log("WINDOWS_BUILD Scenes=" + string.Join(",", scenes));
            Debug.Log("WINDOWS_BUILD Output=" + output);

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            EditorUtility.UnloadUnusedAssetsImmediate();GC.Collect();
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = output,
                target = target,
                subtarget = (int)StandaloneBuildSubtarget.Player,
                options = BuildOptions.Development
            });
            var summary = report.summary;
            var record = new BuildRecord
            {
                unityVersion = Application.unityVersion,
                result = summary.result.ToString(),
                target = summary.platform.ToString(),
                scriptingBackend = PlayerSettings.GetScriptingBackend(NamedBuildTarget.Standalone).ToString(),
                options = summary.options.ToString(),
                outputPath = summary.outputPath,
                scenes = scenes,
                totalSizeBytes = summary.totalSize,
                totalErrors = summary.totalErrors,
                totalWarnings = summary.totalWarnings,
                startedUtc = summary.buildStartedAt.ToUniversalTime().ToString("o"),
                endedUtc = summary.buildEndedAt.ToUniversalTime().ToString("o"),
                durationSeconds = summary.totalTime.TotalSeconds,
                files = report.GetFiles().Select(file => new BuildFileRecord
                {
                    path = file.path, role = file.role, sizeBytes = file.size
                }).ToArray()
            };
            File.WriteAllText(reportPath, JsonUtility.ToJson(record, true) + "\n");
            Debug.Log("WINDOWS_BUILD Result=" + record.result + " Errors=" + record.totalErrors
                + " Warnings=" + record.totalWarnings + " Bytes=" + record.totalSizeBytes);
            if (summary.result != BuildResult.Succeeded || !File.Exists(output))
                throw new BuildFailedException("Windows development build failed; inspect the Unity log and " + reportPath);
        }

        [Serializable]
        private sealed class BuildRecord
        {
            public string unityVersion, result, target, scriptingBackend, options, outputPath;
            public string startedUtc, endedUtc;
            public string[] scenes;
            public ulong totalSizeBytes;
            public int totalErrors, totalWarnings;
            public double durationSeconds;
            public BuildFileRecord[] files;
        }

        [Serializable]
        private sealed class BuildFileRecord
        {
            public string path, role;
            public ulong sizeBytes;
        }
    }
}
#endif
