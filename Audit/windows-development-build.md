Windows development build result, 2026-10-15 UTC

The real Unity Windows x86_64 standalone development build **succeeded**. Unity exited with code **0** and reported **0 build errors** and **0 build warnings**. This successful build supersedes the earlier readiness inspection: Windows Build Support (Mono) is now installed. No build errors required gameplay code changes.

| Item | Result |
| --- | --- |
| Host | Ubuntu 24.04.5 LTS, x86_64, headless |
| Editor | Unity `6000.3.26f1`, revision `de8167e7440c`, matching the project |
| URP | `17.3.0` |
| License | Active Unity Personal, confirmed by the installed CLI |
| Target | `StandaloneWindows64`, standalone Player |
| Scripting backend | `Mono2x` (Mono), explicitly configured and verified against the emitted runtime |
| Build option | `BuildOptions.Development` |
| Startup scene, index 0 | `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity` |
| Additional scene, index 1 | `Assets/Scenes/SampleScene.unity` |
| Unity build interval | 2026-10-15T02:13:55.9126810Z to 2026-10-15T02:15:36.2084780Z |
| Unity build duration | 100.296 seconds, excluding Editor startup/import |
| Executable | `/root/Projects/LayarTerakhir/Builds/Windows/LayarTerakhir.exe` |
| Complete output directory | `/root/Projects/LayarTerakhir/Builds/Windows/` |
| Complete output size | **166,515,872 bytes (158.8 MiB)** across **289 files** |
| ZIP | `/root/Projects/LayarTerakhir/Builds/LayarTerakhir-Windows-x86_64-Development.zip` |
| ZIP size | **67,508,583 bytes (64.4 MiB)** |

The executable, `UnityPlayer.dll`, and `MonoBleedingEdge/EmbedRuntime/mono-2.0-bdwgc.dll` have valid PE32+ headers with AMD64 machine type `0x8664`. Required scene data (`level0`, `level1`, shared assets, resources and global managers), game assemblies, URP, Input System, native plugins, and Mono runtime/configuration files are present and nonempty. All **288 files** listed by Unity's BuildReport match their reported sizes, and all **112 assemblies** listed in `ScriptingAssemblies.json` exist. The startup scene name is also present in the generated `globalgamemanagers` file.

Unity's reported player size is 166,322,846 bytes. The complete directory is 193,026 bytes larger because it also contains Unity's generated `LayarTerakhir_BurstDebugInformation_DoNotShip/Data/Plugins/x86_64/lib_burst_generated.txt`. The ZIP preserves that file and all other generated development output, including debug symbols. Its file list, uncompressed sizes, and CRC values match the complete build directory; reading every ZIP entry passed the integrity check. SHA-256 checksums were saved for every build file and for the archive.

Download the ZIP with WinSCP and extract the entire archive on Windows. Launch `Windows/LayarTerakhir.exe` from the extracted folder, keeping its accompanying folders and DLLs together.

Archive SHA-256:

```text
ee73058c4587a0bef9934b12ef91993b54ac15cb34719797da5a5d98371e7c98
```

The minimal Editor build entry point is [WindowsDevelopmentBuild.cs](../Assets/_LayarTerakhir/Editor/WindowsDevelopmentBuild.cs). It validates target support and scene files, ensures the existing prolog is first, selects Mono and the Player subtarget, and invokes `BuildPipeline.BuildPlayer` with `BuildOptions.Development`. Its JSON output records Unity's actual summary and output inventory, including the raw summary options string. Mono is separately confirmed through PlayerSettings and the emitted Mono runtime and managed assemblies.

The exact Unity command used was:

```bash
/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity \
  -batchmode -nographics -quit \
  -projectPath /root/Projects/LayarTerakhir \
  -buildTarget Win64 \
  -executeMethod LayarTerakhir.Editor.WindowsDevelopmentBuild.Build \
  -logFile /root/Projects/LayarTerakhir/Builds/Logs/windows-development.log
```

Saved evidence:

- [Unity build log](../Builds/Logs/windows-development.log)
- [Console log](../Builds/Logs/windows-development-console.log)
- [Command, timestamps and process exit code](../Builds/Logs/windows-build-invocation.json)
- [Unity BuildReport summary and file inventory](../Builds/Logs/windows-development-build-report.json)
- [Native Unity build report](../Builds/Logs/windows-development.buildreport)
- [Artifact and ZIP verification, with file hashes](../Builds/Logs/windows-build-verification.json)
- [Build file SHA-256 manifest](../Builds/Logs/windows-build-files.sha256)
- [ZIP SHA-256 checksum](../Builds/LayarTerakhir-Windows-x86_64-Development.zip.sha256)

The only change to an existing project source/configuration file is the explicit `Standalone: 0` Mono backend setting in `ProjectSettings/ProjectSettings.asset`. The startup scene configuration was already correct. Unity's incidental render-asset and default-setting serialization was restored from pre-build copies after the Editor exited; temporary performance-test resources were removed by Unity itself. The new Editor build script and its metadata are retained for repeat builds. No existing scenes, gameplay scripts, GDD, or package versions were changed.

Verification covered the actual Unity build result, generated file structure, native binary architecture, assembly inventory, checksums and ZIP integrity. **The Windows executable was not run, and visual gameplay was not tested.**
