Windows playtest screenshots (development build)

Extract the complete `Windows/` folder from the build ZIP and launch `LayarTerakhir.exe`.
Wait until the opening fade has cleared, then press **F12** during play to save the actual final rendered screen, including post-processing and HUD.

Screenshots are written to:

`%USERPROFILE%\AppData\LocalLow\DefaultCompany\LayarTerakhir\Screenshots\`

For an automatic startup screenshot, launch from PowerShell in the extracted folder:

```powershell
.\LayarTerakhir.exe -kosCapture
```

This captures once after eight seconds; F12 remains available for other views. If the application is still showing its opening fade, take another capture with F12 after it clears. Each PNG has a unique UTC filename. The `Player.log` in the same LayarTerakhir data directory records the output path, graphics device and resolution under `KOS_SCREENSHOT`.

The capture utility is compiled only for the Editor and development players. It uses Unity's end-of-frame ScreenCapture without brightness adjustments, image filters, artificial lighting or changes to game state. It does not hide the HUD or alter the phone/gameplay capture action. Editor evidence in `Audit/Visuals/V2/` uses the existing gameplay camera through one URP StandardRequest into an explicit sRGB render target; those evidence views omit overlay-canvas UI. Windows F12 captures include it.

During Windows testing, check the desk chair facing the computer, phone resting on the quilt, wardrobe access, the door/jacket interactions, both clue interactions and the complete existing departure sequence. Actual Iris Xe frame rate and memory use still need to be measured on the target PC.
