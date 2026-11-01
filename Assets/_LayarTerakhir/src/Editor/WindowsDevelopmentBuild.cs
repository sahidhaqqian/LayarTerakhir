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

        // Optional scene subset for partial builds (see skill windows-build-partial).
        // WINDOWS_BUILD_SCENES unset/"full"/"all" = every enabled scene, exactly
        // like before. "prolog" (or "p"/"0") = only the 3 prolog scenes (startup +
        // RumahAtmaja + KoridorKos). Otherwise a comma/space separated list of phase
        // numbers, e.g. "1,2" = prolog + Babak 1 + Babak 2; anything else is rejected.
        // Prolog (startup + RumahAtmaja + KoridorKos) is always included first so the
        // player still boots from the approved startup scene and level indices stay
        // stable (prolog = levels 0-2, phase N = level 2+N).
        private static readonly string[] PhaseScenes =
        {
            "Assets/_LayarTerakhir/assets/Scenes/Babak1_Rusunawa.unity",
            "Assets/_LayarTerakhir/assets/Scenes/Babak2_RSUD.unity",
            "Assets/_LayarTerakhir/assets/Scenes/Babak3_PabrikGula.unity",
            "Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity",
            "Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity",
        };

        private static readonly string[] PrologScenes =
        {
            StartupScene,
            "Assets/_LayarTerakhir/assets/Scenes/Prolog_RumahAtmaja.unity",
            "Assets/_LayarTerakhir/assets/Scenes/Prolog_KoridorKos.unity",
        };

        internal static string SceneSpec => Environment.GetEnvironmentVariable("WINDOWS_BUILD_SCENES") ?? "full";

        internal static string BuildTag => Environment.GetEnvironmentVariable("WINDOWS_BUILD_TAG") ?? string.Empty;

        internal static string[] SelectedScenes()
        {
            string spec = SceneSpec.Trim().ToLowerInvariant();
            if (spec == "" || spec == "full" || spec == "all")
                return null; // null = legacy path: every enabled scene.
            if (spec == "prolog" || spec == "p" || spec == "0")
                return PrologScenes;
            var phases = new System.Collections.Generic.SortedSet<int>();
            foreach (string token in spec.Split(new[] { ',', ' ', ';', '+' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = token.Trim().ToLowerInvariant();
                if (t == "prolog" || t == "p" || t == "0") continue;
                if (t.Length == 1 && t[0] >= '1' && t[0] <= '5') { phases.Add(t[0] - '0'); continue; }
                throw new BuildFailedException("WINDOWS_BUILD_SCENES: unknown token '" + token + "' (use full or phase numbers 1-5, e.g. \"1,2\")");
            }
            var scenes = new System.Collections.Generic.List<string>(PrologScenes);
            foreach (int phase in phases) scenes.Add(PhaseScenes[phase - 1]);
            return scenes.ToArray();
        }

        public static void Build()
        {
            BuildInternal(BuildOptions.Development);
        }

        // Fast path for code-only iterations: rebuilds scripts and patches
        // them into the previous player at the same output path instead of
        // rebuilding scenes and assets. Enabled by setting the environment
        // variable WINDOWS_BUILD_SCRIPTS_ONLY=1 before invoking Build()
        // (see the windows-build-r2 skill, which decides eligibility and
        // falls back to a full build automatically on failure).
        // Requires a previous successful full build at the same location;
        // anything else (scenes, prefabs, assets, settings) needs Build().
        // Partial builds (WINDOWS_BUILD_TAG set, see skill windows-build-partial)
        // always build full assets into their own output directory.
        private static void BuildInternal(BuildOptions options)
        {
            const BuildTarget target = BuildTarget.StandaloneWindows64;
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, target))
                throw new BuildFailedException("Windows x86_64 build support is unavailable.");
            if (EditorUserBuildSettings.activeBuildTarget != target)
                throw new BuildFailedException("Launch Unity with -buildTarget Win64.");
            if (!File.Exists(StartupScene))
                throw new BuildFailedException("Startup scene is missing: " + StartupScene);

            var scenes = SelectedScenes();
            string tag = BuildTag;
            foreach (char c in tag)
                if (!(char.IsLetterOrDigit(c) || c == '-'))
                    throw new BuildFailedException("WINDOWS_BUILD_TAG must match [A-Za-z0-9-] (got '" + tag + "')");
            string outputSubdir = string.IsNullOrEmpty(tag) ? "Windows" : "Windows-" + tag;
            if (scenes == null)
            {
                scenes = new[] { StartupScene }
                    .Concat(EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path))
                    .Distinct().ToArray();
            }
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
            string output = Path.Combine(projectRoot, "Builds", outputSubdir, "LayarTerakhir.exe");
            string reportPath = Path.Combine(projectRoot, "Builds/Logs/windows-development-build-report.json");
            Directory.CreateDirectory(Path.GetDirectoryName(output));
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));
            if (Environment.GetEnvironmentVariable("WINDOWS_BUILD_SCRIPTS_ONLY") == "1" && string.IsNullOrEmpty(tag))
            {
                if (!File.Exists(output))
                    throw new BuildFailedException("Scripts-only build requested but no previous player exists at " + output + "; run a full build first.");
                options |= BuildOptions.BuildScriptsOnly;
            }
            Debug.Log("WINDOWS_BUILD Target=" + target + " Backend=Mono2x Options=" + options);
            Debug.Log("WINDOWS_BUILD Spec=" + SceneSpec + " Tag=" + tag);
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
                options = options
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
                sceneSpec = SceneSpec,
                buildTag = tag,
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
            public string unityVersion, result, target, scriptingBackend, options, outputPath, sceneSpec, buildTag;
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
