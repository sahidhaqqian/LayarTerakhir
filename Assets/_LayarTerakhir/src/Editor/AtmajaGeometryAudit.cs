#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // Mirrors the authored budget exactly, including inactive meshes and every instance.
    public static class AtmajaGeometryAudit
    {
        [Serializable] private sealed class Entry
        {
            public string group, hierarchy, mesh, asset;
            public int triangles, vertices;
            public bool active, rendererEnabled;
            public Vector3 position, scale;
        }

        [Serializable] private sealed class Inventory
        {
            public int totalTriangles, staticTriangles, sceneSkinnedTriangles, runtimeRakaTriangles;
            public Entry[] entries;
            public string[] coincidentInstances;
        }

        public static void Run()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var root = GameObject.Find("Atmaja_Environment_Art");
            var entries = new List<Entry>();
            string Hierarchy(Transform t) => t.parent ? Hierarchy(t.parent) + "/" + t.name : t.name;
            void Add(string group, Component component, Mesh mesh)
            {
                if (!mesh) return;
                int triangles = 0;
                for (int sub = 0; sub < mesh.subMeshCount; sub++)
                    triangles += (int)mesh.GetIndexCount(sub) / 3;
                var renderer = component.GetComponent<Renderer>();
                entries.Add(new Entry
                {
                    group = group, hierarchy = Hierarchy(component.transform), mesh = mesh.name,
                    asset = AssetDatabase.GetAssetPath(mesh), triangles = triangles, vertices = mesh.vertexCount,
                    active = component.gameObject.activeInHierarchy, rendererEnabled = renderer && renderer.enabled,
                    position = component.transform.position, scale = component.transform.lossyScale
                });
            }
            foreach (var filter in root.GetComponentsInChildren<MeshFilter>(true))
                Add("static", filter, filter.sharedMesh);
            foreach (var skin in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                Add("sceneSkinned", skin, skin.sharedMesh);
            var raka = Resources.Load<GameObject>("RakaCharacter");
            foreach (var skin in raka.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                Add("runtimeRaka", skin, skin.sharedMesh);
            var coincident = root.GetComponentsInChildren<MeshFilter>(true)
                .Where(f => f.sharedMesh)
                .GroupBy(f => new { f.sharedMesh, matrix = f.transform.localToWorldMatrix })
                .Where(g => g.Count() > 1)
                .Select(g => string.Join(" | ", g.Select(f => Hierarchy(f.transform)))).ToArray();
            var report = new Inventory
            {
                totalTriangles = entries.Sum(e => e.triangles),
                staticTriangles = entries.Where(e => e.group == "static").Sum(e => e.triangles),
                sceneSkinnedTriangles = entries.Where(e => e.group == "sceneSkinned").Sum(e => e.triangles),
                runtimeRakaTriangles = entries.Where(e => e.group == "runtimeRaka").Sum(e => e.triangles),
                entries = entries.OrderByDescending(e => e.triangles).ToArray(), coincidentInstances = coincident
            };
            var args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, "-geometryReport");
            string path = index >= 0 ? args[index + 1] : "Audit/atmaja-geometry-inventory.json";
            File.WriteAllText(path, JsonUtility.ToJson(report, true) + "\n");
            Debug.Log("ATMAJA_GEOMETRY triangles=" + report.totalTriangles + " entries=" + entries.Count);
        }
    }
}
#endif
