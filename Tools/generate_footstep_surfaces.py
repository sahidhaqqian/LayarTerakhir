"""Prepare recorded Kenney CC0 surface footsteps without synthesizing surfaces.

Usage: python3 Tools/generate_footstep_surfaces.py --archive /path/impact-sounds.zip
The archive is the verified source already recorded by golden-source-manifest.json.
Stone uses concrete recordings; no claim of recorded wet asphalt is made.
"""
import argparse
import array
import hashlib
import io
import json
from pathlib import Path
import subprocess
import sys
import wave
import zipfile

ROOT = Path(__file__).resolve().parents[1]
TARGET = ROOT / "Assets/Resources/PlayerPresentation/Steps"
ARCHIVE_SHA256 = "029d734af1582474edf3a694d1b0cebc97c1c152f2f39fa34d4c2bafc5de77f8"
RATE = 32000


def prepare(encoded):
    decoded = subprocess.run(
        ["ffmpeg", "-v", "error", "-i", "pipe:0", "-f", "s16le", "-ac", "1", "-ar", str(RATE), "pipe:1"],
        input=encoded, capture_output=True, check=True,
    ).stdout
    pcm = array.array("h")
    pcm.frombytes(decoded)
    if sys.byteorder != "little":
        pcm.byteswap()
    mean = sum(pcm) / max(1, len(pcm))
    samples = [sample - mean for sample in pcm]
    peak = max(abs(sample) for sample in samples) or 1
    gain = 32767 * 10 ** (-13 / 20) / peak
    fade = int(RATE * .005)
    output = array.array("h", (round(sample * gain * min(1, i / fade, (len(samples) - 1 - i) / fade))
                               for i, sample in enumerate(samples)))
    if sys.byteorder != "little":
        output.byteswap()
    result = io.BytesIO()
    with wave.open(result, "wb") as handle:
        handle.setnchannels(1)
        handle.setsampwidth(2)
        handle.setframerate(RATE)
        handle.writeframes(output.tobytes())
    return result.getvalue()


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--archive", type=Path, required=True)
    args = parser.parse_args()
    digest = hashlib.sha256(args.archive.read_bytes()).hexdigest()
    if digest != ARCHIVE_SHA256:
        raise ValueError("Archive does not match the previously verified Kenney source")
    entries = []
    with zipfile.ZipFile(args.archive) as archive:
        for surface, original in (("Stone", "concrete"), ("Earth", "grass"), ("Wood", "wood")):
            for index in range(5):
                member = f"Audio/footstep_{original}_{index:03d}.ogg"
                encoded = archive.read(member)
                output = TARGET / surface / f"Footstep{surface}{index}.wav"
                output.parent.mkdir(parents=True, exist_ok=True)
                data = prepare(encoded)
                output.write_bytes(data)
                entries.append({"path": output.relative_to(ROOT).as_posix(), "sourceMember": member,
                                "sourceSha256": hashlib.sha256(encoded).hexdigest(),
                                "sha256": hashlib.sha256(data).hexdigest()})
        (ROOT / "Audit/kenney-impact-CC0.txt").write_bytes(archive.read("License.txt"))
    manifest = {"source": "Kenney Impact Sounds", "license": "CC0-1.0",
                "sourceUrl": "https://kenney.nl/assets/impact-sounds", "archiveSha256": digest,
                "processing": "Mono PCM16 32kHz, DC removal, peak -13 dBFS, 5ms boundary fades; original surface recordings",
                "limitation": "Concrete represents stone/paved roads. Grass represents earth. No recorded water splashes or gamelan.",
                "entries": entries}
    (ROOT / "Audit/review-footstep-sources.json").write_text(json.dumps(manifest, indent=2) + "\n")
    print(f"Prepared {len(entries)} recorded footsteps")


if __name__ == "__main__":
    main()
