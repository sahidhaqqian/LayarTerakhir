#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Authors the daytime interlude exterior into Prolog_KoridorKos:
    // Zone A narrow gang, Zone C warteg + gerobak, parked vehicles, wires.
    // Runtime keeps only NPCs, moving traffic, lighting and interactions.
    public static class MelatiInterludeAlleyBuilder
    {
        private static Transform root;
        private static Material concrete, wood, metal, roof;
        private static Material plasterA, plasterB, plasterC, brick, zinc, tarpBlue, tarpOrange, glassCase;
        private static Material kenneyCar, kenneyFood;
        private static readonly List<string> notes = new List<string>();

        private static Material Mat(string name) => AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/" + name + ".mat");

        private static Material Tint(string name, Color color, string source = null)
        {
            // Flat authored colors must NOT inherit the source texture:
            // _BaseColor multiplies it and everything renders mud-brown.
            string path = "Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/" + name + ".mat";
            var fresh = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            fresh.SetColor("_BaseColor", color);
            fresh.SetFloat("_Smoothness", .45f);
            var m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!m) { AssetDatabase.CreateAsset(fresh, path); return fresh; }
            EditorUtility.CopySerialized(fresh, m);
            EditorUtility.SetDirty(m);
            Object.DestroyImmediate(fresh);
            return m;
        }

        private static Material Glass(string name)
        {
            string path = "Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/" + name + ".mat";
            var m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!m) { m = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m, path); }
            m.SetFloat("_Surface", 1);
            m.SetFloat("_Blend", 0);
            m.SetFloat("_AlphaClip", 0);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            m.SetColor("_BaseColor", new Color(.78f, .88f, .86f, .22f));
            m.SetFloat("_Smoothness", .8f);
            m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            EditorUtility.SetDirty(m);
            return m;
        }

        private static void FixKitMaterials()
        {
            kenneyCar = KitMaterial("kenney-car-kit");
            kenneyFood = KitMaterial("kenney-food-kit");
        }

        private static Material KitMaterial(string kit)
        {
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet/{kit}/Textures/colormap.png");
            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = "Interlude" + kit };
            if (tex) mat.SetTexture("_BaseMap", tex);
            mat.SetFloat("_Smoothness", .35f);
            return mat;
        }

        private static GameObject Prefab(string kit, string file)
        {
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet/{kit}/{file}.fbx");
            if (!asset) { notes.Add("MISSING_PREFAB " + kit + "/" + file); return null; }
            return (GameObject)PrefabUtility.InstantiatePrefab(asset);
        }

        private static void PaintKit(GameObject go, Material kit)
        {
            if (!go || !kit) return;
            foreach (var r in go.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(kit, r.sharedMaterials.Length).ToArray();
        }

        private static GameObject Box(string name, Vector3 p, Vector3 size, Material material, bool collision = true)
        {
            var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.name = name; g.transform.SetParent(root, false); g.transform.position = p; g.transform.localScale = size;
            g.GetComponent<Renderer>().sharedMaterial = material;
            if (!collision) Object.DestroyImmediate(g.GetComponent<Collider>());
            return g;
        }

        private static void Label(string name, string text, Vector3 pos, Vector3 facing, float size, Color color)
        {
            var go = new GameObject(name, typeof(TextMesh));
            go.transform.SetParent(root, false); go.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(facing));
            var label = go.GetComponent<TextMesh>();
            label.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");
            label.text = text; label.fontSize = 48; label.characterSize = size / 10;
            label.anchor = TextAnchor.MiddleCenter; label.alignment = TextAlignment.Center; label.color = color;
            go.GetComponent<MeshRenderer>().sharedMaterial = label.font.material;
        }

        private static void Wire(Vector3 a, Vector3 b, float sag = .5f)
        {
            var wire = new GameObject("InterludeWire", typeof(LineRenderer));
            wire.transform.SetParent(root, false);
            var line = wire.GetComponent<LineRenderer>();
            line.sharedMaterial = metal; line.startWidth = line.endWidth = .02f; line.positionCount = 5;
            for (int i = 0; i < 5; i++)
            {
                float t = i / 4f;
                line.SetPosition(i, Vector3.Lerp(a, b, t) + Vector3.down * Mathf.Sin(t * Mathf.PI) * sag);
            }
        }

        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            root = GameObject.Find("KampungMelati").transform;
            // Idempotent: clear previous authored pass before rebuilding.
            var residue = new List<GameObject>();
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                string n; try { n = t.name; } catch { continue; }
                if (!(n.StartsWith("Alley") || n.StartsWith("Warteg") || n.StartsWith("Gerobak") || n.StartsWith("StreetParked") || n.StartsWith("StreetAngkot") || n.StartsWith("StreetDress") || n.StartsWith("StreetMotor") || n == "InterludeWire")) continue;
                Transform p = null; try { p = t.parent; } catch { continue; }
                bool parentOurs = false;
                try { parentOurs = p != null && (p.name.StartsWith("Gerobak") || p.name == "InterludeWire"); } catch { continue; }
                if (!parentOurs) residue.Add(t.gameObject);
            }
            foreach (var go in residue) Object.DestroyImmediate(go);
            concrete = Mat("House_MAT_Concrete"); wood = Mat("House_MAT_WoodDoor"); metal = Mat("Upper_SteelRust"); roof = Mat("House_MAT_RoofTiles");
            plasterA = Tint("InterludePlasterA", new Color(.62f, .64f, .58f));
            plasterB = Tint("InterludePlasterB", new Color(.72f, .68f, .58f));
            plasterC = Tint("InterludePlasterC", new Color(.55f, .62f, .68f));
            brick = Tint("InterludeBrick", new Color(.55f, .32f, .24f));
            zinc = Tint("InterludeZinc", new Color(.42f, .44f, .46f));
            tarpBlue = Tint("InterludeTarpBlue", new Color(.10f, .30f, .62f));
            tarpOrange = Tint("InterludeTarpOrange", new Color(.85f, .42f, .10f));
            glassCase = Glass("InterludeEtalaseGlass");
            FixKitMaterials();
            BuildAlley();
            BuildWarteg();
            BuildGerobak(new Vector3(-4.4f, 0, -20f), 90f, "BATAGOR", new Color(.75f, .25f, .12f));
            BuildGerobak(new Vector3(5.6f, 0, -22.5f), -90f, "SIOMAY", new Color(.12f, .38f, .30f));
            ParkedVehicles();
            StreetWires();
            foreach (var r in root.GetComponentsInChildren<MeshRenderer>())
                if (r.gameObject.name.StartsWith("Alley") || r.gameObject.name.StartsWith("Warteg") || r.gameObject.name.StartsWith("Gerobak") || r.gameObject.name.StartsWith("StreetDress"))
                    GameObjectUtility.SetStaticEditorFlags(r.gameObject, StaticEditorFlags.BatchingStatic);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("INTERLUDE_ALLEY_AUTHORED notes=" + notes.Count + (notes.Count > 0 ? " :: " + string.Join(" | ", notes) : ""));
            if (notes.Count > 0) throw new System.Exception("Missing prefabs: " + string.Join(",", notes));
        }

        // Zone A: narrow gang x[-16.5,-3.6], walkway z[-13.25,-10.25].
        private static void BuildAlley()
        {
            float[] edges = { -16.5f, -13.2f, -9.8f, -6.4f, -3.6f };
            Material[] coats = { plasterA, plasterB, plasterC, plasterA };
            for (int side = 0; side < 2; side++)
            {
                float front = side == 0 ? -10.25f : -13.25f;
                float outward = side == 0 ? 1 : -1;
                for (int i = 0; i < 4; i++)
                {
                    float x0 = edges[i], x1 = edges[i + 1], cx = (x0 + x1) / 2, w = x1 - x0;
                    bool twoFloor = (i + side) % 2 == 0;
                    float h = twoFloor ? 6.4f : 3.6f;
                    var coat = coats[(i + side) % coats.Length];
                    Box("AlleyHouse", new Vector3(cx, h / 2, front + outward * 1.6f), new Vector3(w, h, 3.2f), coat);
                    Box("AlleyRoofSlab", new Vector3(cx, h + .1f, front + outward * 1.6f), new Vector3(w + .3f, .2f, 3.6f), roof, false);
                    // Door + windows facing the gang.
                    Box("AlleyDoor", new Vector3(cx - w * .18f, 1.05f, front + outward * .02f), new Vector3(.95f, 2.1f, .1f), wood);
                    Box("AlleyDoorStep", new Vector3(cx - w * .18f, .08f, front - outward * .25f), new Vector3(1.1f, .16f, .6f), concrete, false);
                    Box("AlleyWindow", new Vector3(cx + w * .22f, 1.7f, front + outward * .02f), new Vector3(1.1f, .9f, .08f), metal);
                    Box("AlleyWindowFrame", new Vector3(cx + w * .22f, 1.7f, front - outward * .02f), new Vector3(1.3f, 1.1f, .05f), wood, false);
                    if (twoFloor)
                    {
                        Box("AlleyUpperWindow", new Vector3(cx + w * .1f, 4.6f, front + outward * .02f), new Vector3(1f, .9f, .08f), metal);
                        Box("AlleyAC", new Vector3(cx - w * .25f, 3.9f, front - outward * .25f), new Vector3(.8f, .55f, .35f), zinc);
                        Box("AlleyCantilever", new Vector3(cx, 3.35f, front - outward * .3f), new Vector3(w, .25f, .9f), coat, false);
                    }
                    // Zinc kanopi shading the walkway.
                    var kanopi = Box("AlleyKanopi", new Vector3(cx, 2.45f, front - outward * .75f), new Vector3(w * .9f, .06f, 1.5f), zinc, false);
                    kanopi.transform.rotation = Quaternion.Euler(outward * 8, 0, 0);
                    // Roster vent strip + house number + sticker.
                    Box("AlleyRoster", new Vector3(cx + w * .3f, 2.75f, front + outward * .02f), new Vector3(.9f, .3f, .06f), concrete, false);
                    Label("AlleyHouseNo" + side + i, ("" + (7 + i * 2 + side)), new Vector3(cx - w * .32f, 1.9f, front - outward * .06f), new Vector3(0, 0, outward), .1f, new Color(.9f, .9f, .88f));
                }
            }
            // Open selokan along the south front + plank bridges at doors.
            Box("AlleyDrain", new Vector3(-10f, -.12f, -13.55f), new Vector3(13f, .12f, .5f), metal);
            Box("AlleyDrainWater", new Vector3(-10f, -.1f, -13.55f), new Vector3(13f, .04f, .34f), Tint("InterludeDrainWater", new Color(.16f, .18f, .15f)), false);
            foreach (float x in new[] { -14.9f, -11.4f, -8.1f, -5f })
                Box("AlleyDrainPlank", new Vector3(x, .02f, -13.55f), new Vector3(1.1f, .06f, .7f), wood, false);
            // Wires across the gang + drops.
            Wire(new Vector3(-15f, 4.6f, -10.3f), new Vector3(-15f, 4.4f, -13.2f), .6f);
            Wire(new Vector3(-9f, 4.8f, -10.3f), new Vector3(-9f, 4.5f, -13.2f), .7f);
            Wire(new Vector3(-15f, 4.6f, -10.3f), new Vector3(-9f, 4.8f, -10.3f), .5f);
            Wire(new Vector3(-15f, 4.4f, -13.2f), new Vector3(-9f, 4.5f, -13.2f), .5f);
            Wire(new Vector3(-9f, 4.8f, -10.3f), new Vector3(-4f, 5.2f, -10.3f), .6f);
            // Kos-kosan board + sandals + pots + broom (never blocking the 3m walkway).
            Box("AlleyKosBoard", new Vector3(-12.5f, 1.9f, -10.2f), new Vector3(2.2f, 1f, .08f), Tint("InterludeBoardYellow", new Color(.85f, .68f, .12f)), false);
            Label("AlleyKosBoardText", "KOS PUTRI • 600RB/BLN\nKM DALAM • WIFI • HUB:0812", new Vector3(-12.5f, 1.9f, -10.14f), Vector3.forward, .11f, new Color(.15f, .1f, .05f));
            foreach (var spot in new[] { new Vector3(-14.6f, .03f, -10.5f), new Vector3(-7.8f, .03f, -13f), new Vector3(-5.2f, .03f, -10.5f) })
                for (int s = -1; s <= 1; s += 2)
                    Box("AlleySandals", spot + new Vector3(s * .09f, 0, 0), new Vector3(.11f, .05f, .28f), Tint("InterludeSandal", new Color(.12f, .1f, .09f)), false);
            Box("AlleyEmber", new Vector3(-6.9f, .25f, -12.9f), new Vector3(.4f, .5f, .4f), Tint("InterludeEmberBlue", new Color(.15f, .35f, .65f)), false);
            Box("AlleyGalon", new Vector3(-6.5f, .4f, -12.9f), new Vector3(.32f, .8f, .32f), Tint("InterludeGalon", new Color(.45f, .7f, .85f)), false);
            // Jemuran line high across the gang (visual only).
            var jemur = new GameObject("AlleyJemuran", typeof(LineRenderer));
            jemur.transform.SetParent(root, false);
            var jl = jemur.GetComponent<LineRenderer>();
            jl.sharedMaterial = metal; jl.startWidth = jl.endWidth = .015f; jl.positionCount = 2;
            jl.SetPositions(new[] { new Vector3(-11f, 2.1f, -10.3f), new Vector3(-11f, 2.1f, -13.2f) });
            float[] jz = { -11f, -11.8f, -12.6f };
            Color[] jc = { new Color(.25f, .45f, .65f), new Color(.75f, .3f, .25f), new Color(.85f, .82f, .72f) };
            for (int ji = 0; ji < 3; ji++)
                Box("AlleyJemuranCloth", new Vector3(-11f, 1.8f - ji * .06f, jz[ji]), new Vector3(.04f, .42f, .3f), Tint("InterludeCloth" + jz[ji], jc[ji]), false);
        }

        // Zone C: warteg storefront with banner facade, etalase, stools.
        // Zone C: warteg storefront facing the road (west, -x).
        private static void BuildWarteg()
        {
            Vector3 p = new Vector3(8f, 0, -30f);
            Box("WartegFloor", p + new Vector3(0, .06f, 0), new Vector3(4.2f, .12f, 3.4f), concrete);
            Box("WartegBackWall", p + new Vector3(2.05f, 1.9f, 0), new Vector3(.15f, 3.8f, 3.4f), plasterB);
            Box("WartegSideNorth", p + new Vector3(0, 1.9f, -1.65f), new Vector3(4.2f, 3.8f, .15f), plasterA);
            Box("WartegSideSouth", p + new Vector3(0, 1.9f, 1.65f), new Vector3(4.2f, 3.8f, .15f), plasterA);
            Box("WartegRoofSlab", p + new Vector3(0, 3.9f, 0), new Vector3(4.5f, .2f, 3.7f), roof, false);
            var zincRoof = Box("WartegZincRoof", p + new Vector3(-.3f, 4.35f, 0), new Vector3(4.2f, .08f, 4.8f), zinc, false);
            zincRoof.transform.rotation = Quaternion.Euler(0, 0, 6);
            // Full-width banner facade (ref: WARUNG TEGAL BAROKAH).
            Box("WartegBanner", p + new Vector3(-2.16f, 3.1f, 0), new Vector3(.08f, 1.2f, 4.1f), Tint("WartegBannerBlue", new Color(.12f, .32f, .66f)), false);
            Label("WartegBannerText", "WARTEG BAHARI\nNASI + LAUK 15RB", p + new Vector3(-2.22f, 3.1f, 0), Vector3.right, .24f, new Color(.98f, .92f, .25f));
            // Big blue tarp angled out over the front.
            var tarp = Box("WartegTarp", p + new Vector3(-3.1f, 2.7f, 0), new Vector3(2.4f, .06f, 4.6f), tarpBlue, false);
            tarp.transform.rotation = Quaternion.Euler(0, 0, -12);
            foreach (float z in new[] { -31.9f, -28.1f })
                Box("WartegTarpPole", new Vector3(4.1f, 1.3f, z), new Vector3(.07f, 2.6f, .07f), wood);
            // Etalase along the front: aluminum frame, 2 glass shelves, food rows.
            Box("WartegCounter", p + new Vector3(-1.5f, .55f, 0), new Vector3(.9f, 1.1f, 3.4f), wood);
            Box("WartegCounterTop", p + new Vector3(-1.5f, 1.13f, 0), new Vector3(1f, .06f, 3.5f), concrete, false);
            Box("WartegCaseBase", p + new Vector3(-1.5f, 1.2f, 0), new Vector3(.8f, .08f, 3.2f), metal, false);
            Box("WartegCaseTop", p + new Vector3(-1.5f, 2.05f, 0), new Vector3(.8f, .08f, 3.2f), metal, false);
            foreach (float z in new[] { -31.56f, -28.44f })
                Box("WartegCaseSide", new Vector3(p.x - 1.5f, 1.62f, z), new Vector3(.8f, .9f, .08f), glassCase, false);
            Box("WartegCaseBack", new Vector3(p.x - 1.12f, 1.62f, -30f), new Vector3(.06f, .9f, 3.2f), glassCase, false);
            Box("WartegShelfMid", p + new Vector3(-1.5f, 1.62f, 0), new Vector3(.8f, .05f, 3.2f), glassCase, false);
            var caseLamp = new GameObject("WartegCaseLight", typeof(Light));
            caseLamp.transform.SetParent(root, false);
            caseLamp.transform.position = p + new Vector3(-1.5f, 2.3f, 0);
            var caseLight = caseLamp.GetComponent<Light>();
            caseLight.type = LightType.Point; caseLight.range = 3.5f; caseLight.intensity = 1.2f;
            caseLight.color = new Color(1f, .9f, .75f); caseLight.shadows = LightShadows.None;
            caseLight.enabled = false;
            string[] foods = { "bowl", "bowl-soup", "rice-ball", "sausage", "meat-sausage", "bowl-broth", "plate", "plate-deep", "bread", "banana", "pineapple", "apple", "plate-dinner" };
            int fi = 0;
            for (int tier = 0; tier < 2; tier++)
                for (int i = 0; i < 8; i++)
                {
                    var f = Prefab("kenney-food-kit", foods[fi % foods.Length]);
                    if (!f) continue;
                    PaintKit(f, kenneyFood);
                    f.name = "WartegFood";
                    f.transform.SetParent(root, false);
                    f.transform.position = p + new Vector3(-1.5f, 1.32f + tier * .42f, -1.32f + i * .38f);
                    f.transform.localScale = Vector3.one * 2.2f;
                    fi++;
                }
            // Rice cooker + dandang behind the counter.
            Box("WartegRiceCooker", p + new Vector3(-.5f, 1.35f, -31.2f), new Vector3(.6f, .5f, .6f), Tint("WartegCooker", new Color(.85f, .85f, .86f)));
            Box("WartegDandang", p + new Vector3(-.5f, 1.4f, -28.8f), new Vector3(.7f, .6f, .7f), metal);
            // Hanging rentengan strips near the front.
            foreach (float z in new[] { -31.2f, -30.8f, -30.4f })
                Box("WartegRenteng", new Vector3(p.x - 1.8f, 2.5f, z), new Vector3(.02f, 1.1f, .16f), Tint("WartegRenteng" + z, new Color(.75f, .2f, .15f)), false);
            // Tables, dingklik stools, bench west of the counter.
            foreach (float z in new[] { -29f, -31f })
            {
                Box("WartegTable", new Vector3(4.3f, .55f, z), new Vector3(.6f, .06f, 1.1f), wood, false);
                foreach (float dz in new[] { -.35f, .35f })
                    Box("WartegStool", new Vector3(4.3f, .15f, z + dz), new Vector3(.3f, .3f, .3f), Tint("WartegStool", new Color(.7f, .15f, .12f)), false);
            }
            Box("WartegBench", new Vector3(4.5f, .3f, -30f), new Vector3(.35f, .08f, 2.4f), wood, false);
            Box("WartegMenuBoard", new Vector3(p.x - 2.18f, 1.6f, -28.6f), new Vector3(.05f, .8f, 1.6f), Tint("WartegMenuBoard", new Color(.08f, .12f, .1f)), false);
            Label("WartegMenu", "NASI 10K • AYAM 12K\nTEMPE 3K • POCI 5K", new Vector3(p.x - 2.24f, 1.6f, -28.6f), Vector3.right, .09f, new Color(.95f, .95f, .9f));
        }

        // Gerobak PKL: wood cart, glass case with painted text, roof, counter clutter.
        private static void BuildGerobak(Vector3 p, float yaw, string sign, Color trim)
        {
            var cart = new GameObject("Gerobak" + sign.Replace(" ", ""));
            cart.transform.SetParent(root, false);
            cart.transform.position = p; cart.transform.rotation = Quaternion.Euler(0, yaw, 0);
            Transform t = cart.transform;
            System.Func<string, Vector3, Vector3, Material, bool, GameObject> cbox = (name, pos, size, mat, collision) =>
            {
                var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.name = name; g.transform.SetParent(t, false); g.transform.localPosition = pos; g.transform.localScale = size;
                g.GetComponent<Renderer>().sharedMaterial = mat;
                if (!collision) Object.DestroyImmediate(g.GetComponent<Collider>());
                return g;
            };
            var woodCart = Tint("GerobakWood", new Color(.55f, .33f, .18f));
            var trimMat = Tint("GerobakTrim" + sign.Replace(" ", ""), trim);
            cbox("GerobakBody", Vector3.up * .8f, new Vector3(1.1f, .65f, 1.9f), woodCart, true);
            cbox("GerobakCounterTop", new Vector3(0, 1.15f, 0), new Vector3(1.15f, .06f, 1.95f), woodCart, false);
            cbox("GerobakGlassFront", new Vector3(0, 1.5f, .5f), new Vector3(.95f, .55f, .04f), glassCase, false);
            cbox("GerobakGlassSide", new Vector3(.5f, 1.5f, 0), new Vector3(.04f, .55f, 1.2f), glassCase, false);
            cbox("GerobakGlassSide", new Vector3(-.5f, 1.5f, 0), new Vector3(.04f, .55f, 1.2f), glassCase, false);
            cbox("GerobakRoof", new Vector3(0, 2.35f, 0), new Vector3(1.5f, .06f, 2.3f), trimMat, false);
            foreach (var off in new[] { new Vector3(-.6f, 0, -.9f), new Vector3(.6f, 0, -.9f), new Vector3(-.6f, 0, .9f), new Vector3(.6f, 0, .9f) })
                cbox("GerobakPole", off + Vector3.up * 1.7f, new Vector3(.06f, 1.3f, .06f), woodCart, false);
            // Wheels + leg.
            foreach (float x in new[] { -.62f, .62f })
            {
                var w = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                w.name = "GerobakWheel"; w.transform.SetParent(t, false);
                w.transform.localPosition = new Vector3(x, .35f, .4f); w.transform.localScale = new Vector3(.6f, .12f, .6f);
                w.transform.localRotation = Quaternion.Euler(0, 0, 90);
                w.GetComponent<Renderer>().sharedMaterial = metal;
            }
            cbox("GerobakLeg", new Vector3(0, .3f, -.85f), new Vector3(.08f, .6f, .08f), woodCart, true);
            // Painted text on the glass (mirrored reading from outside, like real carts).
            var text = new GameObject("GerobakSign", typeof(TextMesh));
            text.transform.SetParent(t, false); text.transform.localPosition = new Vector3(0, 1.55f, .53f); text.transform.localRotation = Quaternion.Euler(0, 180, 0);
            var label = text.GetComponent<TextMesh>();
            label.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");
            label.text = sign; label.fontSize = 48; label.characterSize = .028f;
            label.anchor = TextAnchor.MiddleCenter; label.color = new Color(.95f, .85f, .2f);
            text.GetComponent<MeshRenderer>().sharedMaterial = label.font.material;
            // Counter clutter: sauce bucket, wok, cig pack.
            cbox("GerobakBucket", new Vector3(-.3f, 1.35f, -.5f), new Vector3(.28f, .35f, .28f), Tint("GerobakBucket", new Color(.85f, .85f, .85f)), false);
            cbox("GerobakWok", new Vector3(.25f, 1.3f, -.3f), new Vector3(.45f, .12f, .45f), metal, false);
            cbox("GerobakCigPack", new Vector3(.35f, 1.22f, .3f), new Vector3(.12f, .06f, .08f), Tint("GerobakCig", new Color(.7f, .1f, .1f)), false);
            // Scattered stools.
            cbox("GerobakStool", new Vector3(1.1f, .15f, .6f), new Vector3(.3f, .3f, .3f), trimMat, false);
            cbox("GerobakStool", new Vector3(-1f, .15f, -.4f), new Vector3(.3f, .3f, .3f), trimMat, false);
        }

        // Parked motors (project scooter asset, varied), 1 sedan, 1 angkot van.
        private static void ParkedVehicles()
        {
            var scooter = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Review/Models/RakaScooter.fbx");
            // The FBX ships without usable materials; reuse Raka's closed set.
            var corridorBike = GameObject.Find("RakaMotor");
            var paint = new Dictionary<string, Material[]>();
            if (corridorBike)
                foreach (var r in corridorBike.GetComponentsInChildren<Renderer>())
                    paint[r.name] = r.sharedMaterials;
            Vector3[] spots =
            {
                new Vector3(4.6f, 0, -19.5f), new Vector3(4.75f, 0, -21f), new Vector3(4.55f, 0, -22.5f),
                new Vector3(5.5f, 0, -31.5f), new Vector3(-7.5f, 0, -10.7f), new Vector3(-5.6f, 0, -12.9f)
            };
            float[] yaws = { 168f, 203f, 177f, 160f, 95f, 80f };
            for (int i = 0; i < spots.Length; i++)
            {
                var bike = (GameObject)PrefabUtility.InstantiatePrefab(scooter);
                bike.name = "StreetParkedMotor" + i;
                bike.transform.SetParent(root, false);
                bike.transform.position = spots[i];
                bike.transform.rotation = Quaternion.Euler(0, yaws[i], 0);
                foreach (var r in bike.GetComponentsInChildren<Renderer>())
                    if (paint.TryGetValue(r.name, out var slots)) r.sharedMaterials = slots;
                var col = bike.AddComponent<BoxCollider>();
                col.center = Vector3.up * .7f; col.size = new Vector3(.7f, 1.4f, 1.9f);
            }
            // Covered bike (sarung motor drape).
            Box("StreetMotorCover", new Vector3(4.6f, .55f, -26.6f), new Vector3(.75f, 1.1f, 1.9f), Tint("MotorCover", new Color(.25f, .28f, .32f)));
            // Sedan + angkot van on the road edge.
            var sedan = Prefab("kenney-car-kit", "sedan");
            if (sedan)
            {
                PaintKit(sedan, kenneyCar);
                sedan.name = "StreetParkedSedan";
                sedan.transform.SetParent(root, false);
                sedan.transform.position = new Vector3(2.7f, .1f, -38f);
                sedan.transform.rotation = Quaternion.Euler(0, 180, 0);
                var col = sedan.AddComponent<BoxCollider>(); col.center = Vector3.up * .5f; col.size = new Vector3(1.9f, 1.1f, 4.2f);
            }
            var van = Prefab("kenney-car-kit", "van");
            if (van)
            {
                PaintKit(van, kenneyCar);
                van.name = "StreetAngkot";
                van.transform.SetParent(root, false);
                van.transform.position = new Vector3(2.7f, .1f, -48f);
                van.transform.rotation = Quaternion.Euler(0, 180, 0);
                van.transform.localScale = Vector3.one * 1.15f;
                var col = van.AddComponent<BoxCollider>(); col.center = Vector3.up * .8f; col.size = new Vector3(2f, 1.9f, 4.6f);
                Label("StreetAngkotRoute", "D.01", new Vector3(2.7f, 1.5f, -50.2f), Vector3.back, .16f, Color.white);
            }
        }





        // Extra catenary wires along the main road + drops to houses.
        private static void StreetWires()
        {
            for (int i = 0; i < 3; i++)
            {
                float z0 = -18 - i * 22, z1 = z0 - 22;
                Wire(new Vector3(4.9f, 5.1f, z0), new Vector3(4.9f, 5.1f, z1), .8f);
                Wire(new Vector3(-4.9f, 5.1f, z0), new Vector3(-4.9f, 5.1f, z1), .8f);
                Wire(new Vector3(4.9f, 5.1f, z0 - 11), new Vector3(10.5f, 3.4f, z0 - 11), .5f);
                Wire(new Vector3(-4.9f, 5.1f, z0 - 11), new Vector3(-10.5f, 3.4f, z0 - 11), .5f);
            }
            Wire(new Vector3(-4.9f, 5.1f, -18f), new Vector3(4.9f, 5.1f, -18f), .9f);
        }
    }
}
#endif
