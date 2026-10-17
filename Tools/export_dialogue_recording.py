"""Export the implemented Indonesian dialogue for human recording; never synthesizes audio."""
from pathlib import Path
import csv
import hashlib
import json
import re

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / "Audit/Recording"


def export():
    lines = {}
    def add(speaker, text, trigger):
        if speaker in ("SFX", "Sistem") or not text.strip():
            return
        key = hashlib.sha256((speaker + "\n" + text).encode()).hexdigest()[:12]
        direction = {
            "Raka": "Pria muda; bahasa sehari-hari Jakarta; gugup ditahan, jeda napas wajar. Jangan baca rata.",
            "Damar": "Pria muda berbeda warna suara dari Raka; berbisik panik, napas terputus; teriakan di akhir panggilan.",
            "Ibu": "Perempuan paruh baya sedang sakit; lemah tetapi hangat; batuk/napas pendek halus, bukan karikatur.",
            "Bu Haji Ratna": "Perempuan dewasa; tegas, mengantuk, kemudian lega. Bukan suara hantu atau ancaman berteriak.",
        }.get(speaker, "Ikuti konteks adegan; hindari intonasi datar.")
        lines.setdefault(key, dict(id=key, speaker=speaker, text=text, trigger=trigger, direction=direction,
                                   estimated_seconds=round(max(2.5, len(text) / 17), 1), recording_status="WAITING_FOR_USER_RECORDING"))
    def literal(value):
        return json.loads('"' + value + '"')
    for path in sorted((ROOT / "Assets/_LayarTerakhir/Scripts").rglob("*.cs")):
        source = path.read_text()
        for match in re.finditer(r'(?:EnqueueSubtitle|PlayPriorityDialogue)\(\s*"([^"\n]+)"\s*,\s*"((?:\\.|[^"\\])*)"', source):
            add(match[1], literal(match[2]), path.relative_to(ROOT).as_posix())
        if path.name == "PhoneDesktop.cs":
            for match in re.finditer(r'public const string (\w+)\s*=\s*"((?:\\.|[^"\\])*)"', source):
                name = match[1]
                if name in ("Contract", "Diary"):
                    continue
                speaker = "Ibu" if name.startswith("Mother") else "Bu Haji Ratna" if name.startswith("Ratna") else "Damar" if name.startswith("Damar") else "Raka"
                add(speaker, literal(match[2]), "KosDialogue." + name)
        if path.name == "AtmajaDirector.cs":
            match = re.search(r'public const string LiveIntro="((?:\\.|[^"\\])*)"', source)
            if match:
                add("Raka", literal(match[1]), "Mulai live dari aplikasi Kamera")
        if path.name == "ExplorationCommentary.cs":
            for match in re.finditer(r'new Zone\("([^"]+)","((?:\\.|[^"\\])*)"', source):
                add("Raka", literal(match[2]), "Masuk ruangan: " + match[1])
    OUT.mkdir(parents=True, exist_ok=True)
    rows = sorted(lines.values(), key=lambda row: (row["speaker"], row["id"]))
    with (OUT / "dialogue-recording.csv").open("w", newline="", encoding="utf-8-sig") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0]))
        writer.writeheader()
        writer.writerows(rows)
    (OUT / "dialogue-recording.json").write_text(json.dumps(rows, indent=2, ensure_ascii=False) + "\n")
    print(f"Recording script: {len(rows)} lines, {len(set(r['speaker'] for r in rows))} characters; no audio generated")


if __name__ == "__main__":
    export()
