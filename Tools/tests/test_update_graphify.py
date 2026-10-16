"""Regression checks for the asset-exclusion and freshness gate."""
import importlib.util
import json
from pathlib import Path
import tempfile
import unittest

SPEC = importlib.util.spec_from_file_location("update_graphify", Path(__file__).parents[1] / "update_graphify.py")
graphify = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(graphify)


class GraphifySafetyTests(unittest.TestCase):
    def graph(self, source):
        return {
            "nodes": [{"id": "a", "file_type": "code", "source_file": source}],
            "links": [{"source": "a", "target": "UnityEngine", "source_file": source}],
        }

    def test_first_party_code_allowed(self):
        for path in (
            "Assets/_LayarTerakhir/Scripts/Player/Player.cs",
            "Assets/_LayarTerakhir/Editor/Build.cs",
            "Assets/_LayarTerakhir/Tests/Check.cs",
            "Tools/update_graphify.py", "Tools/tests/test_update_graphify.py", "Tools/run.sh",
        ):
            with self.subTest(path=path):
                self.assertTrue(graphify.allowed_source(path))

    def test_assets_and_vendor_rejected(self):
        for path in (
            "Assets/_LayarTerakhir/Art/mesh.fbx", "Assets/_LayarTerakhir/Scenes/Room.unity",
            "Assets/_LayarTerakhir/Scripts/trap.png", "Tools/trap.wav", "Tools/map.jpg",
            "Tools/prefab.prefab", "Tools/settings.asset", "Tools/script.cs.meta",
            "Assets/Resources/App.asset", "Assets/TutorialInfo/Scripts/Readme.cs",
            "Packages/vendor.cs", "Builds/Game.zip", "Bugs/image.png", "Library/generated.cs",
            "AssetResearch/tools/author.py", "Scene2_Atmaja_Assets/tools/fetch.py",
            "Tools/../secret.py", "/tmp/Tools/secret.py", "Tools\\file.py", "",
        ):
            with self.subTest(path=path):
                self.assertFalse(graphify.allowed_source(path))

    def test_asset_in_graph_rejected(self):
        with self.assertRaises(ValueError):
            graphify.validate_graph(self.graph("Tools/image.png"), {"Tools/image.png": "hash"})

    def test_asset_on_edge_rejected(self):
        path = "Tools/run.py"
        graph = self.graph(path)
        graph["links"][0]["source_file"] = "Assets/tree.fbx"
        with self.assertRaises(ValueError):
            graphify.validate_graph(graph, {path: "hash"})

    def test_semantic_node_rejected(self):
        graph = self.graph("Tools/run.py")
        graph["nodes"][0]["file_type"] = "image"
        with self.assertRaises(ValueError):
            graphify.validate_graph(graph, {"Tools/run.py": "hash"})

    def test_ast_comment_concepts_allowed_but_semantic_concepts_rejected(self):
        graph = self.graph("Tools/run.py")
        node = graph["nodes"][0]
        node.update(file_type="rationale", _origin="ast")
        graphify.validate_graph(graph, {"Tools/run.py": "hash"})
        node["_origin"] = "semantic"
        with self.assertRaises(ValueError):
            graphify.validate_graph(graph, {"Tools/run.py": "hash"})

    def test_deleted_source_rejected(self):
        with self.assertRaises(ValueError):
            graphify.validate_graph(self.graph("Tools/deleted.py"), {})

    def test_semantic_code_edge_rejected(self):
        graph = self.graph("Tools/run.py")
        graph["links"][0]["_origin"] = "semantic"
        with self.assertRaises(ValueError):
            graphify.validate_graph(graph, {"Tools/run.py": "hash"})

    def test_external_symbols_without_file_allowed(self):
        graph = self.graph("Tools/run.py")
        graph["nodes"].append({"id": "UnityEngine", "source_file": ""})
        graph["nodes"].append({"id": "System", "source_file": "", "file_type": "concept", "external": True, "type": "external"})
        self.assertEqual({"Tools/run.py"}, graphify.validate_graph(graph, {"Tools/run.py": "hash"}))

    def test_stale_code_ignore_and_artifacts_rejected(self):
        with tempfile.TemporaryDirectory(prefix="graphify-safety-test-") as directory:
            root = Path(directory)
            (root / "Tools").mkdir()
            source = root / "Tools/run.py"
            source.write_text("print('ok')\n")
            ignore = root / ".graphifyignore"
            ignore.write_text("*\n!Tools/\n!Tools/*.py\n")
            out = root / "graphify-out"
            out.mkdir()
            (out / "graph.json").write_text(json.dumps(self.graph("Tools/run.py")))
            for name in ("GRAPH_REPORT.md", "graph.html"):
                (out / name).write_text("generated")
            receipt = {
                "sources_sha256": graphify.snapshot(root),
                "ignore_sha256": graphify.digest(ignore),
                "artifacts_sha256": {name: graphify.digest(out / name) for name in graphify.ARTIFACTS},
            }
            (out / "source-manifest.json").write_text(json.dumps(receipt))
            graphify.check(root)
            for path in (source, ignore, out / "GRAPH_REPORT.md", out / "graph.html", out / "graph.json"):
                original = path.read_bytes()
                path.write_bytes(original + b"\n# changed\n")
                with self.subTest(path=path.name), self.assertRaises(ValueError):
                    graphify.check(root)
                path.write_bytes(original)

    def test_snapshot_skips_media_and_rejects_symlink(self):
        with tempfile.TemporaryDirectory(prefix="graphify-snapshot-test-") as directory:
            root = Path(directory)
            (root / "Tools").mkdir()
            source = root / "Tools/code.py"
            source.write_text("print('ok')")
            (root / "Tools/image.png").write_bytes(b"not code")
            self.assertEqual(["Tools/code.py"], list(graphify.snapshot(root)))
            (root / "Tools/alias.py").symlink_to(source)
            with self.assertRaises(ValueError):
                graphify.snapshot(root)


if __name__ == "__main__":
    unittest.main()
