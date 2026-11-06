# Catatan posisi: tangan Meshy + HP (disetujui user, 2026-11-06)

Sumber tangan: `Meshy_AI_Invisible_Frame_20261105_character.glb` (R2 `tangan/`),
pose mentah KECUALI ibu jari (di bawah).

## Tangan
- Scale rig: S = 0.0642 (seragam XYZ).
- Jari telunjuk/tengah: mentah (0 derajat, tidak disentuh).
- Ibu jari (kiri Bone_024/023, kanan Bone_038/037, cermin — sudut sama dua sisi):
  - MCP (024/038): rotasi X = -6.6 deg, order XYZ
  - IP (023/037): rotasi X = -3.3 deg, order XYZ
- Ujung ibu jari terukur (frame GLB×S, mm): R (38.5, -30.0, 12.3), L (-39.5, -30.1, 12.7)
  (turun ~6mm dari mentah 18.4)
- Bake: apply armature, parent clear (keep transform), apply location/rotation/scale.
- Belah kiri/kanan di x=0, geser rigid ke tengah: kiri +2mm X, kanan -2mm X.

## HP nyata (Smartphone.fbx game, bukan proxy)
- Mesh: `Assets/_LayarTerakhir/assets/Art/Original/Models/Smartphone.fbx`
  (case 73x145x12mm + glass display + kamera + earpiece, tanpa Icosphere junk).
- Mapping: handX=phoneY, handY=-phoneZ (kaca ke kamera), handZ=-phoneX.
- Scale: sx=0.18/0.145, sy=0.008/0.012 (tebal 8mm), sz=0.07875/0.073.
- Hasil terukur: span (180.0, 9.7, 78.7)mm, min (-90.0, 3.2, 18.0)mm.
- Posisi: tengah x=0, slab y di belakang jari, tepi bawah z=+18mm (jari di tepi atas).
- Material preview: bodi abu gelap, layar biru muda (basic, bukan tekstur game).
- File: `Audit/Visuals/Feel-Meshy/mentah_hp_nyata_depan.png` = R2 `tangan/mentah_hp_nyata_depan.png`.
- Skrip repo: `Tools/meshy_grasp_redbox.py` (proxy) dan
  `Tools/meshy_grasp_realphone.py` (HP nyata); kamera (0,-0.9,0.060) lens 50.

## Masuk game (final, disetujui user)
- Tangan kanan saja di-export (`Tools/export_meshy_grasp_game.py`):
  pose approved + similarity s=165/180 ke box game 165mm + guides
  (Thumb/Index/Middle snap permukaan + Wrist + Origin/Right/Up/Forward).
- Builder mirror untuk tangan kiri; material `SourcedMeshyHands`
  (diffuse Meshy) via `Phase1EnvironmentBuilder.cs`.
- Runtime `AtmajaPhoneView.cs`: `gripRaiseMetres=.005f` (tangan turun 25mm,
  HP turun 5mm relatif — net sesuai foto R2 `after-meshy-phonedown5`).
- Foto game: R2 `tangan/after-meshy-10a-viewfinder.png` (spike, dibuang),
  `tangan/after-meshy-lowered-10a-viewfinder.png`,
  `tangan/after-meshy-phonedown5-10a-viewfinder.png` (final).
- Jujur: gate `SourcedLandscapeGrasp...` masih merah 4.0mm
  (selaput webbing lewat bodi, nol verteks di dalam) — visual disetujui user.
