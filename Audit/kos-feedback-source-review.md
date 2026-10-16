# Source review — Kos feedback revision, 16 October 2026

The latest user corrections override the older Prolog's three-digit lock and voice-note presentation:
four wheels encode day/month (14 August), keys are on the desk, the phone connects a call automatically,
and the landlady leaves only her signed letter. Other approved room artwork is retained.

## Internet map

- [Jakarta Indonesia street map.svg](https://commons.wikimedia.org/wiki/File:Jakarta_Indonesia_street_map.svg),
  Kirill Shrayber, uploaded 13 June 2024; the file page explicitly dedicates it under
  [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/).
- Imported Wikimedia's 960px PNG rendering as `Assets/Resources/Phone/JakartaStreetMap.png`.
  The phone crops the image non-destructively with RawImage UV coordinates; original pixels are retained.
- Kos Raka / Rumah Atmaja pins and the route are original UI overlays, representing fictional story
  locations, not actual residential addresses or a claimed navigable real-world route. No Google Maps
  screenshots, bulk OSM tile download, geolocation or live network connection is used in the game.

## Dialogue research

- [VoxCPM2 model card](https://huggingface.co/openbmb/VoxCPM2), revision
  `32279effe8c19989596f05d353d1447f51d9e915`, explicitly licenses the released model under Apache-2.0,
  supports Indonesian and voice design. [Official implementation](https://github.com/OpenBMB/VoxCPM).
  Delivery is rendered WAVs only; weights and Python inference dependencies are not bundled.
  Reference samples were generated locally. Final dialogue was rendered through the
  [official public VoxCPM2 demo](https://huggingface.co/spaces/openbmb/VoxCPM-Demo), which does not
  expose its exact backend checkpoint revision. Only original fictional script/synthetic references
  were uploaded; no saves, personal recordings, credentials or paid-service accounts were involved.
  The public API's supported `/generate` endpoint was used sequentially, not a private backend URL.
- Voice-design samples are original synthetic fictional speakers, not samples of named actors,
  public figures, or scraped private people. Each character reuses its own synthetic reference.
- **Rejected:** [OmniVoice model card](https://huggingface.co/k2-fsa/OmniVoice/blob/main/README.md)
  differentiates Apache-2.0 code from **CC-BY-NC pretrained weights**. Trial samples were moved out of
  the project to a temporary rejected-source folder; they must not enter the commercial game/build.
  The model metadata alone was insufficient to establish commercial compatibility.
- The previous eSpeak speech remains historical source material only; the final voice manifest is
  the authoritative list of clips actually referenced by the presentation profile.

ASR transcripts, when generated, are an intelligibility diagnostic, not proof of convincing acting,
emotion, accent, or subjective player approval. Actual Windows listening remains a manual acceptance step.
