"""Integration tests using the installed Graphify discovery/ignore engine."""
from pathlib import Path
import unittest

try:
    from graphify.detect import detect, ignored_predicate
except ImportError:
    detect = ignored_predicate = None

ROOT = Path(__file__).resolve().parents[2]


@unittest.skipIf(detect is None, "Run with the Graphify venv Python for discovery integration tests")
class GraphifyDiscoveryTests(unittest.TestCase):
    def test_real_corpus_is_code_only(self):
        files = detect(ROOT)["files"]
        self.assertGreater(len(files["code"]), 50)
        for kind, paths in files.items():
            if kind != "code":
                self.assertEqual([], paths, kind)
        for source in files["code"]:
            path = Path(source).relative_to(ROOT)
            self.assertIn(path.suffix, (".cs", ".py", ".sh"))
            self.assertTrue(
                path.is_relative_to("Tools") or any(
                    path.is_relative_to("Assets/_LayarTerakhir/" + directory)
                    for directory in ("Scripts", "Editor", "Tests")
                ), str(path)
            )

    def test_real_ignore_rejects_media_even_inside_code_roots(self):
        ignored = ignored_predicate(ROOT)
        for directory in (
            "Tools", "Tools/tests", "Assets/_LayarTerakhir/Scripts/Prolog",
            "Assets/_LayarTerakhir/Editor", "Assets/_LayarTerakhir/Tests",
        ):
            for suffix in (
                ".png", ".jpg", ".svg", ".wav", ".ogg", ".mp4", ".fbx",
                ".glb", ".blend", ".mat", ".unity", ".prefab", ".asset", ".meta", ".zip", ".json",
            ):
                with self.subTest(directory=directory, suffix=suffix):
                    self.assertTrue(ignored(ROOT / directory / ("asset-trap" + suffix)))
        for path in (
            "Assets/TutorialInfo/Scripts/Readme.cs", "Packages/vendor/Code.cs",
            "Library/generated.cs", "Builds/generated.cs", "Bugs/script.py",
            "AssetResearch/tools/author.py", "Scene2_Atmaja_Assets/tools/fetch.py",
        ):
            with self.subTest(path=path):
                self.assertTrue(ignored(ROOT / path))


if __name__ == "__main__":
    unittest.main()
