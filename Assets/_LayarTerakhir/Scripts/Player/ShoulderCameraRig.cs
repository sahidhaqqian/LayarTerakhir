using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace LayarTerakhir.Player
{
    // Keeps the existing movement/save contract, but renders Raka from a collision-aware shoulder.
    [DefaultExecutionOrder(100)]
    public sealed class ShoulderCameraRig : MonoBehaviour
    {
        public Vector3 shoulderOffset = new Vector3(.48f, .10f, -1.85f);
        public float collisionRadius = .16f;
        public float CurrentDistance { get; private set; }
        public Transform Avatar { get; private set; }
        private FirstPersonController player;
        private Camera eye;
        private Transform leftArm, rightArm, leftLeg, rightLeg, head, torso, handset;
        private Renderer[] bodyRenderers;
        private Vector3 lastPosition;
        private float stride;
        private readonly RaycastHit[] hits = new RaycastHit[48];
        private Material skin, hair, jacket, jeans, shoes, shirt;

        private void Start()
        {
            player = GetComponent<FirstPersonController>();
            eye = GetComponent<InteractionInteractor>().playerCamera;
            eye.nearClipPlane = .055f;
            BuildRaka();
            lastPosition = transform.position;
        }

        private void LateUpdate()
        {
            if (!eye || !player.cameraPivot) return;
            // Wake-up and motorcycle shots temporarily own the camera.
            if (UiFocus.Cinematic || !player.enabled) return;
            RefreshCameraPose();
        }

        public void RefreshCameraPose()
        {
            if (!eye || !player || !player.cameraPivot) return;
            var pivot = player.cameraPivot;
            Vector3 anchor = pivot.position;
            Vector3 desired = pivot.TransformPoint(shoulderOffset);
            Vector3 direction = desired - anchor;
            float length = direction.magnitude;
            int count = Physics.SphereCastNonAlloc(anchor, collisionRadius, direction.normalized,
                hits, length, ~0, QueryTriggerInteraction.Ignore);
            var results = count == hits.Length ? Physics.SphereCastAll(anchor, collisionRadius,
                direction.normalized, length, ~0, QueryTriggerInteraction.Ignore) : hits;
            if (results != hits) count = results.Length;
            float distance = length;
            for (int i = 0; i < count; i++)
                if (results[i].collider && !results[i].transform.IsChildOf(transform))
                    distance = Mathf.Min(distance, Mathf.Max(.12f, results[i].distance - .06f));
            CurrentDistance = distance;
            eye.transform.SetPositionAndRotation(anchor + direction.normalized * distance, pivot.rotation);
            AnimateRaka();
            foreach (var renderer in bodyRenderers)
                renderer.shadowCastingMode = distance < .42f ? ShadowCastingMode.ShadowsOnly : ShadowCastingMode.On;
        }

        private void AnimateRaka()
        {
            Vector3 delta = transform.position - lastPosition;
            lastPosition = transform.position;
            delta.y = 0;
            float speed = delta.magnitude / Mathf.Max(.001f, Time.deltaTime);
            if (speed < 9) stride += delta.magnitude * 7;
            float swing = Mathf.Sin(stride) * Mathf.Clamp01(speed / 1.5f) * (player.IsCrouching ? 19 : 30);
            bool phone = SmartphoneController.Instance && SmartphoneController.Instance.isPhoneRaised;
            leftLeg.localRotation = Quaternion.Euler(swing, 0, 0);
            rightLeg.localRotation = Quaternion.Euler(-swing, 0, 0);
            leftArm.localRotation = Quaternion.Euler(-swing * .7f, 0, 7);
            rightArm.localRotation = Quaternion.Euler(phone ? -68 : swing * .7f, phone ? -12 : 0, -7);
            handset.gameObject.SetActive(phone);
            Avatar.localScale = new Vector3(1, player.IsCrouching ? .61f : 1, 1);
            torso.localRotation = Quaternion.Euler(player.IsCrouching ? 18 : 0, 0, 0);
        }

        private Material ColorMaterial(string name, Color color)
        {
            var material = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = name };
            material.SetColor("_BaseColor", color);
            material.SetFloat("_Smoothness", .25f);
            return material;
        }

        private Transform Joint(string name, Transform parent, Vector3 position)
        {
            var joint = new GameObject(name).transform;
            joint.SetParent(parent, false); joint.localPosition = position;
            return joint;
        }

        private Transform Part(string name, Transform parent, Vector3 at, Vector3 scale, Material material, bool round = false)
        {
            var part = GameObject.CreatePrimitive(round ? PrimitiveType.Sphere : PrimitiveType.Cube);
            part.name = name; part.transform.SetParent(parent, false);
            part.transform.localPosition = at; part.transform.localScale = scale;
            part.GetComponent<Renderer>().sharedMaterial = material;
            var hull=part.GetComponent<Collider>();hull.enabled=false;Destroy(hull);
            return part.transform;
        }

        private void BuildRaka()
        {
            // Original articulated, clothed young-adult silhouette; never a visible controller capsule.
            skin = ColorMaterial("Raka warm skin", new Color(.48f, .29f, .19f));
            hair = ColorMaterial("Raka black hair", new Color(.025f, .022f, .021f));
            jacket = ColorMaterial("Raka olive rain jacket", new Color(.14f, .19f, .16f));
            jeans = ColorMaterial("Raka charcoal jeans", new Color(.055f, .065f, .08f));
            shoes = ColorMaterial("Raka worn sneakers", new Color(.10f, .095f, .085f));
            shirt = ColorMaterial("Raka dark shirt", new Color(.17f, .16f, .14f));
            Avatar = Joint("Raka_ArticulatedCharacter", transform, Vector3.zero);
            torso = Joint("Chest", Avatar, new Vector3(0, 1.12f, 0));
            Part("JacketBody", torso, new Vector3(0, .12f, 0), new Vector3(.43f, .50f, .26f), jacket, true);
            Part("ShirtHem", torso, new Vector3(0, -.10f, .01f), new Vector3(.32f, .12f, .24f), shirt);
            Part("JacketZip", torso, new Vector3(0, .12f, .132f), new Vector3(.012f, .40f, .015f), shoes);
            Part("HoodFold", torso, new Vector3(0, .34f, -.075f), new Vector3(.29f, .13f, .19f), jacket, true);
            Part("Neck", torso, new Vector3(0, .39f, 0), new Vector3(.12f, .17f, .12f), skin, true);
            head = Joint("Head", torso, new Vector3(0, .54f, .015f));
            Part("Face", head, Vector3.zero, new Vector3(.205f, .25f, .22f), skin, true);
            Part("Hair", head, new Vector3(0, .079f, -.025f), new Vector3(.22f, .14f, .225f), hair, true);
            Part("HairFringe", head, new Vector3(.035f, .065f, .071f), new Vector3(.15f, .075f, .07f), hair, true);
            Part("Nose", head, new Vector3(0, -.01f, .113f), new Vector3(.035f, .045f, .04f), skin, true);
            foreach (int side in new[] { -1, 1 })
            {
                Part("Ear", head, new Vector3(side * .102f, -.005f, 0), new Vector3(.038f, .065f, .04f), skin, true);
                Part("Eye", head, new Vector3(side * .043f, .024f, .103f), new Vector3(.025f, .014f, .012f), hair, true);
                var arm = Joint(side < 0 ? "LeftArm" : "RightArm", torso, new Vector3(side * .225f, .27f, 0));
                Part("Sleeve", arm, new Vector3(0, -.22f, 0), new Vector3(.13f, .44f, .15f), jacket, true);
                Part("Hand", arm, new Vector3(0, -.47f, .015f), new Vector3(.085f, .12f, .07f), skin, true);
                var leg = Joint(side < 0 ? "LeftLeg" : "RightLeg", Avatar, new Vector3(side * .105f, .90f, 0));
                Part("TrouserLeg", leg, new Vector3(0, -.37f, 0), new Vector3(.16f, .76f, .19f), jeans, true);
                Part("Sneaker", leg, new Vector3(0, -.80f, .05f), new Vector3(.17f, .13f, .30f), shoes, true);
                Part("Sole", leg, new Vector3(0, -.855f, .05f), new Vector3(.17f, .025f, .29f), shirt);
                if (side < 0) { leftArm = arm; leftLeg = leg; } else { rightArm = arm; rightLeg = leg; }
            }
            handset = Part("HandheldPhone", rightArm, new Vector3(0, -.48f, .065f), new Vector3(.075f, .145f, .012f), hair);
            Part("PhoneGlass", handset, new Vector3(0, 0, -.6f), new Vector3(.88f, .90f, .08f), shirt);
            bodyRenderers = Avatar.GetComponentsInChildren<Renderer>();
        }

        private void OnDestroy()
        {
            foreach (var material in new[] { skin, hair, jacket, jeans, shoes, shirt }) if (material) Destroy(material);
        }
    }
}
