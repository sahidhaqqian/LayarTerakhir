"""Recording exports must include branching lines, not just direct subtitles."""
import csv
import hashlib
import importlib.util
import json
from pathlib import Path
import unittest

ROOT = Path(__file__).resolve().parents[2]
spec = importlib.util.spec_from_file_location("dialogue_recording", ROOT / "Tools/export_dialogue_recording.py")
recording = importlib.util.module_from_spec(spec)
spec.loader.exec_module(recording)


class DialogueRecordingTests(unittest.TestCase):
    def test_direct_lines_preserve_escaped_quotes_and_newlines(self):
        source = r'Hud.EnqueueSubtitle("Raka","Dia bilang \"pulang\".\nTunggu.",4);'
        self.assertEqual(list(recording.dialogue_literals(source)), [("Raka", 'Dia bilang "pulang".\nTunggu.')])

    def test_both_payment_branches_are_included_once(self):
        source = 'Hud.EnqueueSubtitle("Raka",hospital?"Buat Ibu.":"Buat kos.",4);'
        self.assertEqual(list(recording.dialogue_literals(source)), [("Raka", "Buat Ibu."), ("Raka", "Buat kos.")])

    def test_scare_helper_lines_are_included_without_exporting_signature(self):
        source = 'FlashFigure(figure,.7f,"Raka","Di belakang gue!"); FlashFigure(GameObject figure,float seconds,string speaker,string line)'
        self.assertEqual(list(recording.dialogue_literals(source)), [("Raka", "Di belakang gue!")])

    def test_delivered_csv_json_ids_and_required_lines_match(self):
        rows = json.loads((ROOT / "Audit/Recording/dialogue-recording.json").read_text())
        with (ROOT / "Audit/Recording/dialogue-recording.csv").open(encoding="utf-8-sig", newline="") as handle:
            csv_rows = list(csv.DictReader(handle))
        self.assertEqual(len(rows), len(csv_rows))
        self.assertGreaterEqual(len(rows), 60)
        self.assertEqual(len({row["id"] for row in rows}), len(rows))
        for row, csv_row in zip(rows, csv_rows):
            self.assertEqual(row["id"], hashlib.sha256((row["speaker"] + "\n" + row["text"]).encode()).hexdigest()[:12])
            self.assertEqual(row["text"], csv_row["text"])
            self.assertEqual(row["recording_status"], "WAITING_FOR_USER_RECORDING")
        texts = {row["text"] for row in rows}
        self.assertIn("Di... di belakang gue... Nggak ada siapa-siapa?!", texts)
        self.assertIn("Udah terkirim, Bu. Semoga besok Ibu bisa ditangani.", texts)
        self.assertIn("Lunas, Bu Haji. Setidaknya besok barang gue nggak dikeluarin.", texts)


if __name__ == "__main__":
    unittest.main()
