using LayarTerakhir.Prolog;
using UnityEngine;

namespace LayarTerakhir.Interlude
{
    // Daytime street dressing for the interlude, built at runtime so the
    // authored night scenes are never re-saved. All coordinates are world
    // space matching MelatiNeighborhoodBuilder; kos-yard dressing uses the
    // building-local DressYard instead. References: warteg etalase + witty
    // names, PKL gerobak dorong, RT/RW gapura, vinyl spanduk, counter pulsa,
    // laundry kiloan, tambal ban, galon isi ulang, pos ronda cues.
    public static class InterludeExterior
    {
        private static Font labelFont;
        private static Material labelMaterial;

        public static void DressStreet(KosCorridorDirector corridor)
        {
            var root = GameObject.Find("KampungMelati");
            Transform parent = root ? root.transform : new GameObject("InterludeStreet").transform;
            CacheLabelFont();
            Gapura(parent);
            Spanduk(parent);
            Warteg(parent);
            GerobakGorengan(parent, new Vector3(-4.4f, 0, -20f), 90f, "GORENGAN ANGET");
            GerobakSiomay(parent, new Vector3(5.2f, 0, -21.5f), -90f);
            MotorRow(corridor, parent);
            ShopSigns(parent);
            StreetClutter(parent);
            PottedPlants(parent);
        }

        private static void CacheLabelFont()
        {
            if (labelFont) return;
            foreach (var label in Object.FindObjectsByType<TextMesh>(FindObjectsSortMode.None))
            {
                if (!label || !label.font) continue;
                labelFont = label.font;
                var renderer = label.GetComponent<MeshRenderer>();
                if (renderer) labelMaterial = renderer.sharedMaterial;
                break;
            }
        }

        private static Material Mat(Color color, float smoothness = .35f)
        {
            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = color;
            mat.SetFloat("_Smoothness", smoothness);
            return mat;
        }

        internal static void DropCollider(GameObject go)
        {
            var collider = go.GetComponent<Collider>();
            if (!collider) return;
            Drop(collider);
        }

        internal static void Drop(Object o)
        {
            if (!o) return;
            if (Application.isPlaying) Object.Destroy(o);
            else Object.DestroyImmediate(o);
        }

        private static GameObject Box(Transform parent, string name, Vector3 pos, Vector3 size, Material mat, bool collision = true)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = size;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            if (!collision) DropCollider(go);
            return go;
        }

        private static GameObject Tube(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat, bool collision = true)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            if (!collision) DropCollider(go);
            return go;
        }

        private static void Label(Transform parent, string name, string text, Vector3 pos, Vector3 facing, float size, Color color)
        {
            if (!labelFont) return;
            var go = new GameObject(name, typeof(TextMesh));
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.rotation = Quaternion.LookRotation(facing);
            var label = go.GetComponent<TextMesh>();
            label.font = labelFont;
            label.text = text;
            label.fontSize = 48;
            label.characterSize = size / 10;
            label.anchor = TextAnchor.MiddleCenter;
            label.color = color;
            if (labelMaterial) go.GetComponent<MeshRenderer>().sharedMaterial = labelMaterial;
        }

        // Neighborhood welcome arch over the kos access lane.
        private static void Gapura(Transform parent)
        {
            var pillar = Mat(new Color(.82f, .25f, .16f));
            var cream = Mat(new Color(.90f, .86f, .74f));
            foreach (float x in new[] { -10.6f, -4.0f })
            {
                Box(parent, "InterludeGapuraPillar", new Vector3(x, 1.1f, -12f), new Vector3(.5f, 2.2f, .5f), pillar);
                Box(parent, "InterludeGapuraCap", new Vector3(x, 2.3f, -12f), new Vector3(.7f, .25f, .7f), cream, false);
            }
            Box(parent, "InterludeGapuraBeam", new Vector3(-7.3f, 2.65f, -12f), new Vector3(7.1f, .55f, .4f), pillar);
            Box(parent, "InterludeGapuraRoof", new Vector3(-7.3f, 3.05f, -12f), new Vector3(7.5f, .18f, 1.1f), Mat(new Color(.16f, .18f, .20f)), false);
            Label(parent, "InterludeGapuraText", "SELAMAT DATANG\nGANG MELATI  •  RT 05 / RW 09",
                new Vector3(-7.3f, 2.65f, -12.25f), Vector3.back, .45f, new Color(.98f, .95f, .85f));
            Label(parent, "InterludeGapuraTextBack", "GANG MELATI\nTAMU WAJIB LAPOR RT",
                new Vector3(-7.3f, 2.65f, -11.75f), Vector3.forward, .45f, new Color(.98f, .95f, .85f));
        }

        // Vinyl banners strung across the street — the RT/RW voice of the kampung.
        private static void Spanduk(Transform parent)
        {
            Banner(parent, "InterludeSpandukUtama", new Vector3(0, 4.4f, -18f), 9.6f, 1.1f,
                new Color(.75f, .12f, .12f),
                "WARGA GANG MELATI  •  JAGA KEBERSIHAN  •  BUANG SAMPAH PADA TEMPATNYA", .025f);
            Banner(parent, "InterludeSpandukWarteg", new Vector3(4.6f, 2.6f, -27.5f), 4.4f, .9f,
                new Color(.95f, .75f, .10f),
                "WARTEG BAHARI  •  NASI + LAUK 15RB  •  TEH POCI", .02f);
        }

        private static void Banner(Transform parent, string name, Vector3 pos, float width, float height, Color bg, string text, float charSize)
        {
            var board = Box(parent, name, pos, new Vector3(width, height, .06f), Mat(bg, .6f), false);
            board.transform.rotation = Quaternion.Euler(0, 0, -1.2f);
            Vector3 facing = board.transform.forward;
            var go = new GameObject(name + "Text", typeof(TextMesh));
            go.transform.SetParent(parent, false);
            go.transform.position = pos + facing * .05f;
            go.transform.rotation = board.transform.rotation;
            if (!labelFont) return;
            var label = go.GetComponent<TextMesh>();
            label.font = labelFont;
            label.text = text;
            label.fontSize = 40;
            // Sized so the longest line fits its board without mesh surgery:
            // the TextMesh glyph mesh only exists after first render.
            label.characterSize = charSize;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.color = bg.grayscale > .5f ? new Color(.6f, .08f, .08f) : Color.white;
            var renderer = go.GetComponent<MeshRenderer>();
            if (labelMaterial) renderer.sharedMaterial = labelMaterial;
        }

        // Humble roadside eatery: etalase counter with food trays, stools, tarp, menu.
        private static void Warteg(Transform parent)
        {
            Vector3 p = new Vector3(4.6f, 0, -28.5f);
            var wood = Mat(new Color(.42f, .30f, .20f));
            var frame = Mat(new Color(.20f, .32f, .30f));
            Box(parent, "InterludeWartegFloor", p + new Vector3(0, .06f, 0), new Vector3(4.2f, .12f, 3f), Mat(new Color(.55f, .54f, .50f)));
            Box(parent, "InterludeWartegCounter", p + new Vector3(0, .55f, -.9f), new Vector3(3.6f, 1.1f, .9f), wood);
            Box(parent, "InterludeWartegCounterTop", p + new Vector3(0, 1.13f, -.9f), new Vector3(3.7f, .06f, 1f), Mat(new Color(.75f, .72f, .65f)), false);
            // Etalase: open-front glass case with food trays.
            Box(parent, "InterludeWartegCaseBase", p + new Vector3(0, 1.35f, -.9f), new Vector3(3.4f, .08f, .8f), frame, false);
            Box(parent, "InterludeWartegCaseTop", p + new Vector3(0, 2.0f, -.9f), new Vector3(3.4f, .08f, .8f), frame, false);
            foreach (float x in new[] { -1.65f, 1.65f })
                Box(parent, "InterludeWartegCaseSide", p + new Vector3(x, 1.67f, -.9f), new Vector3(.08f, .7f, .8f), frame, false);
            Box(parent, "InterludeWartegCaseBack", p + new Vector3(0, 1.67f, -1.28f), new Vector3(3.4f, .7f, .06f), Mat(new Color(.85f, .88f, .86f), .6f), false);
            Color[] foods =
            {
                new Color(.95f, .93f, .88f), new Color(.80f, .45f, .15f), new Color(.72f, .55f, .20f),
                new Color(.30f, .55f, .25f), new Color(.55f, .30f, .15f), new Color(.90f, .60f, .65f)
            };
            for (int i = 0; i < foods.Length; i++)
                Box(parent, "InterludeWartegTray", p + new Vector3(-1.35f + (i % 3) * 1.35f, 1.52f, -.9f + (i / 3) * .02f),
                    new Vector3(1.1f, .12f, .6f), Mat(foods[i], .5f), false);
            // Tarp awning on bamboo poles.
            var bamboo = Mat(new Color(.60f, .50f, .32f));
            foreach (var off in new[] { new Vector3(-1.9f, 0, .4f), new Vector3(1.9f, 0, .4f) })
                Tube(parent, "InterludeWartegPole", p + off + Vector3.up * 1.3f, new Vector3(.07f, 2.6f, .07f), bamboo);
            var tarp = Box(parent, "InterludeWartegTarp", p + new Vector3(0, 2.75f, -.3f), new Vector3(4.6f, .06f, 3.4f), Mat(new Color(.10f, .45f, .30f), .7f), false);
            tarp.transform.rotation = Quaternion.Euler(8, 0, 0);
            // Plastic stools + low table.
            var stoolRed = Mat(new Color(.75f, .15f, .12f), .5f);
            var stoolBlue = Mat(new Color(.12f, .30f, .65f), .5f);
            Vector3[] stools = { new Vector3(-1.2f, 0, .8f), new Vector3(-.4f, 0, .9f), new Vector3(.5f, 0, .8f), new Vector3(1.3f, 0, .9f) };
            for (int i = 0; i < stools.Length; i++)
            {
                Tube(parent, "InterludeWartegStool", p + stools[i] + Vector3.up * .23f,
                    new Vector3(.32f, .45f, .32f), i % 2 == 0 ? stoolRed : stoolBlue, false);
            }
            Box(parent, "InterludeWartegTable", p + new Vector3(.05f, .5f, .85f), new Vector3(1.6f, .06f, .6f), wood, false);
            Label(parent, "InterludeWartegSign", "WARTEG BAHARI\nNASI + LAUK 15RB", p + new Vector3(0, 3.35f, -.6f), Vector3.back, .35f, new Color(.95f, .80f, .15f));
            Label(parent, "InterludeWartegMenu", "NASI SAYUR 10K  •  AYAM GORENG 12K\nTEMPE 3K  •  TEH POCI 5K  •  KOPI 5K",
                p + new Vector3(-1.2f, 1.7f, .62f), Vector3.back, .11f, new Color(.92f, .92f, .88f));
            Box(parent, "InterludeWartegMenuBoard", p + new Vector3(-1.2f, 1.7f, .66f), new Vector3(2.2f, .8f, .05f), Mat(new Color(.08f, .12f, .10f)), false);
        }

        // Fried-snack push cart with glass case and tarp.
        private static void GerobakGorengan(Transform parent, Vector3 p, float yaw, string sign)
        {
            var root = new GameObject("InterludeGerobakGorengan");
            root.transform.SetParent(parent, false);
            root.transform.position = p;
            root.transform.rotation = Quaternion.Euler(0, yaw, 0);
            Transform t = root.transform;
            var body = Mat(new Color(.55f, .38f, .22f));
            var steel = Mat(new Color(.70f, .72f, .74f), .6f);
            Box(t, "CartBody", Vector3.up * .85f, new Vector3(1.1f, .7f, 1.9f), body);
            Box(t, "CartCaseBase", new Vector3(0, 1.25f, 0), new Vector3(1.0f, .06f, 1.7f), steel, false);
            Box(t, "CartCaseTop", new Vector3(0, 1.75f, 0), new Vector3(1.0f, .06f, 1.7f), steel, false);
            foreach (float x in new[] { -.47f, .47f })
                Box(t, "CartCaseSide", new Vector3(x, 1.5f, 0), new Vector3(.06f, .55f, 1.7f), steel, false);
            Color[] snacks = { new Color(.80f, .55f, .20f), new Color(.70f, .42f, .15f), new Color(.85f, .62f, .25f) };
            for (int i = 0; i < 3; i++)
                Box(t, "CartSnack", new Vector3(0, 1.38f, -.55f + i * .55f), new Vector3(.8f, .1f, .45f), Mat(snacks[i], .5f), false);
            var wheel = Mat(new Color(.10f, .10f, .10f), .8f);
            foreach (float x in new[] { -.6f, .6f })
            {
                var w = Tube(t, "CartWheel", new Vector3(x, .3f, .5f), new Vector3(.55f, .12f, .55f), wheel);
                w.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            Box(t, "CartLeg", new Vector3(0, .3f, -.8f), new Vector3(.08f, .6f, .08f), body);
            foreach (float x in new[] { -.3f, .3f })
            {
                var handle = Box(t, "CartHandle", new Vector3(x, .9f, -1.25f), new Vector3(.07f, .07f, .8f), body, false);
                handle.transform.rotation = Quaternion.Euler(-12, 0, 0);
            }
            var tarp = Box(t, "CartTarp", new Vector3(0, 2.25f, 0), new Vector3(1.5f, .05f, 2.3f), Mat(new Color(.85f, .40f, .10f), .7f), false);
            tarp.transform.rotation = Quaternion.Euler(4, 0, 0);
            foreach (var off in new[] { new Vector3(-.6f, 0, -.9f), new Vector3(.6f, 0, -.9f), new Vector3(-.6f, 0, .9f), new Vector3(.6f, 0, .9f) })
                Tube(t, "CartPole", off + Vector3.up * 1.6f, new Vector3(.05f, 1.3f, .05f), body, false);
            Label(t, "CartSign", sign, new Vector3(0, 2.45f, .2f), root.transform.rotation * Vector3.forward, .2f, new Color(.95f, .85f, .20f));
        }

        private static void GerobakSiomay(Transform parent, Vector3 p, float yaw)
        {
            var root = new GameObject("InterludeGerobakSiomay");
            root.transform.SetParent(parent, false);
            root.transform.position = p;
            root.transform.rotation = Quaternion.Euler(0, yaw, 0);
            Transform t = root.transform;
            var body = Mat(new Color(.25f, .45f, .55f));
            Box(t, "CartBody", Vector3.up * .8f, new Vector3(1.2f, .65f, 1.6f), body);
            // Steamer stack.
            var steel = Mat(new Color(.75f, .77f, .78f), .6f);
            for (int i = 0; i < 3; i++)
                Tube(t, "Steamer" + i, new Vector3(0, 1.3f + i * .28f, -.3f), new Vector3(.55f, .26f, .55f), steel, false);
            Tube(t, "SteamerLid", new Vector3(0, 2.15f, -.3f), new Vector3(.55f, .12f, .55f), steel, false);
            var wheel = Mat(new Color(.10f, .10f, .10f), .8f);
            foreach (float x in new[] { -.65f, .65f })
            {
                var w = Tube(t, "CartWheel", new Vector3(x, .3f, .4f), new Vector3(.55f, .12f, .55f), wheel);
                w.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            var tarp = Box(t, "CartTarp", new Vector3(0, 2.5f, 0), new Vector3(1.6f, .05f, 2.1f), Mat(new Color(.12f, .35f, .60f), .7f), false);
            tarp.transform.rotation = Quaternion.Euler(4, 0, 0);
            Label(t, "CartSign", "SIOMAY • BATAGOR\nBANDUNG ASLI", new Vector3(0, 2.7f, .2f), root.transform.rotation * Vector3.forward, .16f, Color.white);
        }

        // A row of parked bikes cloned from the in-scene scooter — instant street life.
        private static void MotorRow(KosCorridorDirector corridor, Transform parent)
        {
            var source = corridor.motor ? corridor.motor.gameObject : GameObject.Find("RakaMotor");
            if (!source) return;
            Vector3[] spots =
            {
                new Vector3(4.6f, 0, -20f), new Vector3(4.6f, 0, -21.6f), new Vector3(4.6f, 0, -23.2f)
            };
            for (int i = 0; i < spots.Length; i++)
            {
                var bike = Object.Instantiate(source);
                bike.name = "InterludeParkedMotor" + i;
                bike.transform.SetParent(parent, false);
                bike.transform.position = spots[i];
                bike.transform.rotation = Quaternion.Euler(0, 180 + (i - 1) * 7, 0);
                foreach (var item in bike.GetComponentsInChildren<Prolog.KosTravelItem>(true)) Drop(item);
                foreach (var item in bike.GetComponentsInChildren<InterludeWorldItem>(true)) Drop(item);
            }
        }

        // Small-business boards on facades — the texture of an Indonesian street.
        private static void ShopSigns(Transform parent)
        {
            Sign(parent, "InterludeSignLaundry", "LAUNDRY KILOAN\n7RB/KG • SETRIKA", new Vector3(-10.3f, 1.9f, -19f), Vector3.forward, new Color(.10f, .40f, .55f));
            Sign(parent, "InterludeSignGalon", "ISI ULANG GALON\nRO 5RB", new Vector3(10.3f, 1.8f, -28f), Vector3.back, new Color(.10f, .45f, .60f));
            Sign(parent, "InterludeSignPulsa", "PULSA • TOKEN • TOP UP", new Vector3(-10.3f, 1.9f, -33f), Vector3.forward, new Color(.70f, .12f, .35f));
            Sign(parent, "InterludeSignTambal", "TAMBAL BAN\nISI ANGIN", new Vector3(10.3f, 1.6f, -14f), Vector3.back, new Color(.15f, .15f, .15f));
        }

        private static void Sign(Transform parent, string name, string text, Vector3 pos, Vector3 facing, Color bg)
        {
            Box(parent, name, pos, new Vector3(2.4f, 1.0f, .08f), Mat(bg, .6f), false);
            Label(parent, name + "Text", text, pos + facing * .06f, facing, .15f, Color.white);
        }

        // Bins, sacks, crates, water jugs — believable roadside clutter.
        private static void StreetClutter(Transform parent)
        {
            var binGreen = Mat(new Color(.12f, .35f, .20f), .5f);
            foreach (var spot in new[] { new Vector3(3.9f, 0, -16f), new Vector3(-3.9f, 0, -26f), new Vector3(3.9f, 0, -34f) })
            {
                Tube(parent, "InterludeBin", spot + Vector3.up * .45f, new Vector3(.6f, .9f, .6f), binGreen);
                Tube(parent, "InterludeBinLid", spot + Vector3.up * .93f, new Vector3(.64f, .08f, .64f), binGreen, false);
            }
            var sack = Mat(new Color(.85f, .82f, .72f));
            Vector3 sackBase = new Vector3(6.2f, 0, -26.5f);
            Box(parent, "InterludeRiceSack0", sackBase + new Vector3(0, .25f, 0), new Vector3(.9f, .5f, .55f), sack);
            Box(parent, "InterludeRiceSack1", sackBase + new Vector3(.1f, .68f, 0), new Vector3(.8f, .4f, .5f), sack, false);
            var crateBlue = Mat(new Color(.15f, .30f, .65f), .5f);
            for (int i = 0; i < 3; i++)
                Box(parent, "InterludeCrate", new Vector3(6.4f + (i % 2) * .45f, .2f + (i / 2) * .42f, -22.8f), new Vector3(.4f, .4f, .4f), crateBlue, i == 0);
            var galonAqua = Mat(new Color(.45f, .70f, .85f));
            for (int i = 0; i < 2; i++)
                Tube(parent, "InterludeGalonStreet", new Vector3(9.6f + i * .4f, .4f, -22.5f), new Vector3(.32f, .8f, .32f), galonAqua, i == 0);
        }

        // Terracotta pots with foliage in front of houses.
        private static void PottedPlants(Transform parent)
        {
            var pot = Mat(new Color(.55f, .28f, .15f));
            var leaf = Mat(new Color(.18f, .42f, .20f), .8f);
            var leaf2 = Mat(new Color(.25f, .52f, .25f), .8f);
            Vector3[] spots =
            {
                new Vector3(-10.2f, 0, -17f), new Vector3(10.2f, 0, -21f),
                new Vector3(-10.2f, 0, -31f), new Vector3(10.2f, 0, -35f)
            };
            for (int i = 0; i < spots.Length; i++)
            {
                Tube(parent, "InterludePot", spots[i] + Vector3.up * .25f, new Vector3(.5f, .5f, .5f), pot);
                var bush = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bush.name = "InterludeBush";
                bush.transform.SetParent(parent, false);
                bush.transform.position = spots[i] + Vector3.up * .85f;
                bush.transform.localScale = new Vector3(.7f, .65f, .7f);
                bush.GetComponent<Renderer>().sharedMaterial = i % 2 == 0 ? leaf : leaf2;
            DropCollider(bush);
            }
        }
    }
}
