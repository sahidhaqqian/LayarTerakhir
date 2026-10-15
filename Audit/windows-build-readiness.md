Windows build inspection, 2026-10-15 UTC

A Windows x86_64 standalone player cannot currently be produced by this installation: Windows Build Support (Mono) is missing. No player build was attempted after confirming that the target is unsupported. No modules were downloaded or installed, and no executable or player data directory was generated in `Builds/Windows/`.

| Check | Observed result |
| --- | --- |
| Host | Ubuntu 24.04.5 LTS, x86_64; headless batch mode works |
| Project Editor version | Unity 6.3 LTS, `6000.3.26f1`, revision `de8167e7440c` |
| Installed Editor | `/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity`; exact version match |
| Render pipeline | `com.unity.render-pipelines.universal` version `17.3.0` |
| Installed player support | Linux standalone Mono, with development and nondevelopment player variations |
| Optional modules | Every entry in this installation's `modules.json` has `selected: false`; only Linux standalone support exists under `Editor/Data/PlaybackEngines/` |
| Windows x86_64 support | `BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64)` returned `False` |
| Linux x86_64 support | The corresponding `StandaloneLinux64` check returned `True` |
| Standalone scripting backend | `Mono2x` (Mono), read from a disposable project's copy of the existing Player settings |
| License | Installed Unity CLI reported `active: true`, `signedIn: true`, and an assigned Unity Personal license; the Editor probe completed with exit code 0 |
| Disk space | 142,706,888,704 bytes available, approximately 132.9 GiB; filesystem 11% used, inode usage 3% |
| Existing project / Editor disk use | Approximately 1.8 GiB / 8.2 GiB |
| Windows output | `Builds/Windows/` does not exist; no Windows player was produced |

The exact missing component is **Windows Build Support (Mono)**, module ID `windows-mono`, for **6000.3.26f1**. Its expected installation directory is:

```text
/root/Unity/Hub/Editor/6000.3.26f1/Editor/Data/PlaybackEngines/WindowsStandaloneSupport/
```

The local module manifest lists a download size of **384,604,969 bytes** (384.6 MB / 366.8 MiB) and an installed size estimate of **769,209,938 bytes** (769.2 MB / 733.6 MiB). These are manifest figures, not measurements of an installation. Disk capacity and the current license are sufficient for the environment probe; the missing Windows module is the confirmed build blocker.

The smallest practical next step is to add only that module to the existing Editor. Unity Hub supports adding individual modules through Installs > the matching Editor > Manage > Add modules ([official instructions](https://docs.unity.com/en-us/hub/add-modules)). The installed Unity CLI also documents this equivalent command, shown for later use and **not executed**:

```bash
unity install-modules --editor-version 6000.3.26f1 --module windows-mono
```

After adding it, retry a standalone **Player** build with Windows, Intel 64-bit/x86_64, Mono, and Development Build enabled. Preserve the enabled scene order already stored in `ProjectSettings/EditorBuildSettings.asset`:

1. `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity`
2. `Assets/Scenes/SampleScene.unity`

Write the player to `Builds/Windows/LayarTerakhir.exe`. In a headless build helper, the equivalent settings are `BuildTarget.StandaloneWindows64`, `StandaloneBuildSubtarget.Player`, and `BuildOptions.Development`, with the existing enabled scenes passed to `BuildPipeline.BuildPlayer`. A temporary helper can be used in an isolated project copy to preserve the original project files. No new gameplay code is required.

Mono is the small installation path available for this Linux Editor. Windows IL2CPP additionally needs a Windows build host, the matching Windows IL2CPP support, Visual Studio C++ tools, and a Windows SDK; Unity documents these [IL2CPP host restrictions](https://docs.unity3d.com/6000.3/Documentation/Manual/il2cpp-introduction.html) and [Windows tool requirements](https://docs.unity3d.com/6000.3/Documentation/Manual/windows-requirements-and-compatibility.html). A machine that already has the matching Editor and Windows Mono support is another way to build without adding anything here.

A successful retry must generate the executable, `UnityPlayer.dll`, `LayarTerakhir_Data/` with scene and managed data, and the Mono runtime files, alongside any other files Unity emits. Distribute the complete output directory and verify it on Windows before calling it playable. Project compilation, Windows shader compilation, and Windows runtime behavior remain unverified because the player build could not proceed.

The environment probe used a disposable minimal project under `/tmp/layarterakhir-windows-check-6wqd_jse/probe`, the installed Editor, and copies of the original version and Player settings. It ran with `-batchmode -nographics -quit -executeMethod WindowsEnvironmentProbe.Run`. Its observed output was:

```text
WINDOWS_AUDIT UnityVersion=6000.3.26f1
WINDOWS_AUDIT Platform=LinuxEditor
WINDOWS_AUDIT BatchMode=True
WINDOWS_AUDIT Windows64Supported=False
WINDOWS_AUDIT Linux64Supported=True
WINDOWS_AUDIT StandaloneBackend=Mono2x
WINDOWS_AUDIT ProbeCompleted=True
Exiting batchmode successfully now!
Editor exit code: 0
```

The Editor logged an access-token refresh warning, then resolved its existing license entitlements and completed the probe successfully. No activation or license changes were performed. The raw probe log remains outside the project at `/tmp/layarterakhir-windows-check-6wqd_jse/unity-environment-probe.log`.

All 136 existing non-cache project files were fingerprinted before the probe and verified unchanged afterward. The original project was never opened in Unity during this inspection. This report is the only added project file; no existing files, scenes, settings, packages, or gameplay scripts were edited.
