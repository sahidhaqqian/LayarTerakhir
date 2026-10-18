import json
import re
import sys
import unittest
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from opening_presentation_identity import ROOT, opening_structure_sha256, verify_opening_source


class OpeningPresentationIdentityTests(unittest.TestCase):
    def test_actual_opening_matches_historic_structure(self):
        result = verify_opening_source()
        baseline = json.loads((ROOT / 'Audit/opening-presentation-baseline.json').read_text())
        self.assertEqual(result['structureSha256'], baseline['structureSha256'])

    def test_camera_settings_can_change_but_layout_and_gameplay_cannot(self):
        source = (ROOT / 'Assets/_LayarTerakhir/assets/Scenes/Prolog_KamarKos.unity').read_text()
        expected = opening_structure_sha256(source)
        camera = re.sub(r'^  m_Dithering: [01]$', '  m_Dithering: 0', source, flags=re.M)
        self.assertEqual(opening_structure_sha256(camera), expected)
        for field in ('m_LocalPosition', 'm_Size', 'walkSpeed', 'togglePerspectiveKey'):
            pattern = re.compile(r'(^  ' + field + r': (?:\{x: )?)(-?[0-9.]+)', re.M)
            self.assertRegex(source, pattern)
            changed = pattern.sub(lambda match: match[1] + '999.25', source, count=1)
            self.assertNotEqual(opening_structure_sha256(changed), expected, field)


if __name__ == '__main__':
    unittest.main()
