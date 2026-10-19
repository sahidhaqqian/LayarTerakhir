using System;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Player
{
    public enum PlayerPerspective { OverShoulder, FirstPerson }
    // Keeps the existing movement/save contract, but renders Raka from a collision-aware shoulder.
    [DefaultExecutionOrder(100)]
    public sealed class ShoulderCameraRig : MonoBehaviour
    {
        public const int AvatarLayer = 26;
        public const uint AvatarRenderingLayer = 1u<<7;
        public Vector3 shoulderOffset = new Vector3(.48f, .10f, -1.85f);
        public float collisionRadius = .16f;
        public float CurrentDistance { get; private set; }
        public Vector3 AimPoint { get; private set; }
        public float VisualGroundOffset { get; private set; }
        public Transform Avatar { get; private set; }
        public bool CinematicVisible { get; set; }
        public bool Riding { get; set; }
        public bool FirstPersonOverride { get; set; }
        public PlayerPerspective SelectedPerspective { get; private set; }
        public bool IsFirstPerson => FirstPersonOverride || SelectedPerspective == PlayerPerspective.FirstPerson;
        public static PlayerPerspective SavedPerspective => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")) && PlayerPrefs.GetInt("LT.Perspective",0)==1 ? PlayerPerspective.FirstPerson : PlayerPerspective.OverShoulder;
        private FirstPersonController player;
        private Camera eye;
        private Transform leftArm, rightArm, leftLeg, rightLeg, head, torso, handset;
        private Renderer[] bodyRenderers;
        private Vector3 lastPosition;
        private float stride;
        private readonly RaycastHit[] hits = new RaycastHit[48];
        private Material skin, hair, jacket, jeans, shoes, shirt;
        private AnimatedRakaAvatar authoredAvatar;
        private Light avatarBounce;
        private MotorcycleRiderAlignment riderAlignment;
        private float cameraDistance = -1, distanceVelocity, phoneBlend, visualSpeed, bodyYaw;
        private int cameraFrame = -1, animationFrame = -1;

        private void Start()
        {
            player = GetComponent<FirstPersonController>();
            eye = GetComponent<InteractionInteractor>().playerCamera;
            eye.nearClipPlane = .055f;
            BuildRaka();
            foreach(var part in Avatar.GetComponentsInChildren<Transform>(true))part.gameObject.layer=AvatarLayer;
            foreach(var renderer in bodyRenderers)renderer.renderingLayerMask|=1u|AvatarRenderingLayer;
            eye.cullingMask|=1<<AvatarLayer;
            var bounce=new GameObject("RakaSoftBounce",typeof(Light));bounce.transform.SetParent(transform,false);
            bounce.transform.localPosition=new Vector3(.55f,1.85f,-.9f);avatarBounce=bounce.GetComponent<Light>();
            avatarBounce.type=LightType.Point;avatarBounce.range=2.8f;avatarBounce.intensity=2.5f;
            avatarBounce.color=new Color(.62f,.68f,.76f);avatarBounce.shadows=LightShadows.None;avatarBounce.cullingMask=1<<AvatarLayer;
            avatarBounce.renderMode=LightRenderMode.ForcePixel;
            avatarBounce.GetUniversalAdditionalLightData().renderingLayers=(RenderingLayerMask)AvatarRenderingLayer;
            SelectedPerspective=SavedPerspective;
            lastPosition = transform.position;
        }

        public void SetPerspective(PlayerPerspective perspective)
        {
            SelectedPerspective=perspective;
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")))
            {PlayerPrefs.SetInt("LT.Perspective",(int)perspective);PlayerPrefs.Save();}
            if(!UiFocus.Cinematic)RefreshCameraPose();
        }

        public void TogglePerspective()=>SetPerspective(SelectedPerspective==PlayerPerspective.OverShoulder?PlayerPerspective.FirstPerson:PlayerPerspective.OverShoulder);

        private void LateUpdate()
        {
            if (!eye || !player.cameraPivot) return;
            bool modal=HudController.Instance&&HudController.Instance.IsModalOpen;
            if(player.enabled&&!UiFocus.Cinematic&&!UiFocus.HardModal&&!modal&&!FirstPersonOverride&&GameInputConfig.Instance&&GameInputConfig.Instance.GetActionDown(GameAction.TogglePerspective))
                TogglePerspective();
            // Wake-up and motorcycle shots temporarily own the camera.
            foreach (var renderer in bodyRenderers) renderer.enabled = !UiFocus.Cinematic || CinematicVisible;
            if(authoredAvatar)authoredAvatar.RefreshOutfit(!UiFocus.Cinematic||CinematicVisible);
            if (UiFocus.Cinematic)
            {
                avatarBounce.enabled=CinematicVisible;
                if(CinematicVisible)
                {
                    foreach(var renderer in bodyRenderers)renderer.shadowCastingMode=ShadowCastingMode.On;
                    AnimateRaka();
                }
                return;
            }
            if (!player.enabled) return;
            UpdateCameraPose(false);
        }

        // Interactions and teleports explicitly request a settled pose in the same frame.
        public void RefreshCameraPose() => UpdateCameraPose(true);

        private void UpdateCameraPose(bool immediate)
        {
            if (!eye || !player || !player.cameraPivot) return;
            var pivot = player.cameraPivot;
            if (IsFirstPerson)
            {
                cameraDistance = -1;
                avatarBounce.enabled=false;
                CurrentDistance=0;
                eye.transform.SetPositionAndRotation(pivot.position, pivot.rotation);
                AnimateRaka();
                foreach (var renderer in bodyRenderers) renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                return;
            }
            Vector3 anchor = pivot.position;
            bool advance = cameraFrame != Time.frameCount;
            if (advance)
            {
                cameraFrame = Time.frameCount;
                bool raised = SmartphoneController.Instance && SmartphoneController.Instance.isPhoneRaised;
                phoneBlend = Mathf.MoveTowards(phoneBlend, raised ? 1 : 0, Time.deltaTime / .22f);
            }
            // Keep the same physical aim line as the phone. The shoulder camera converges
            // on the first visible surface instead of looking along a parallel, offset ray.
            int aimCount = Physics.RaycastNonAlloc(pivot.position, pivot.forward, hits, 40, ~0, QueryTriggerInteraction.Ignore);
            var aimHits = aimCount == hits.Length ? Physics.RaycastAll(pivot.position, pivot.forward, 40, ~0, QueryTriggerInteraction.Ignore) : hits;
            if (aimHits != hits) aimCount = aimHits.Length;
            float focus = 40;
            for (int i = 0; i < aimCount; i++)
                if (aimHits[i].collider && !aimHits[i].transform.IsChildOf(transform)) focus = Mathf.Min(focus, aimHits[i].distance);
            AimPoint = pivot.position + pivot.forward * Mathf.Max(.15f, focus);
            Vector3 offset = Vector3.Lerp(shoulderOffset, new Vector3(.36f, .06f, -1.42f), phoneBlend);
            Vector3 desired = pivot.TransformPoint(offset);
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
            if (immediate || cameraDistance < 0 || Time.deltaTime <= 0 || Vector3.Distance(transform.position, lastPosition) > 3)
            {
                cameraDistance = distance;
                distanceVelocity = 0;
            }
            // Collision retracts immediately; only the unobstructed return is eased.
            if (distance < cameraDistance) { cameraDistance = distance; distanceVelocity = 0; }
            else if (advance)
                cameraDistance = Mathf.SmoothDamp(cameraDistance, distance, ref distanceVelocity, .12f, Mathf.Infinity, Time.deltaTime);
            CurrentDistance = Mathf.Min(distance, cameraDistance);
            avatarBounce.enabled=CurrentDistance>=.9f;
            Vector3 position = anchor + direction.normalized * CurrentDistance;
            eye.transform.SetPositionAndRotation(position, Quaternion.LookRotation(AimPoint - position, pivot.up));
            AnimateRaka();
            foreach (var renderer in bodyRenderers)
                renderer.shadowCastingMode = CurrentDistance < .9f ? ShadowCastingMode.ShadowsOnly : ShadowCastingMode.On;
        }

        private void AnimateRaka()
        {
            // Mount/unmount can happen through explicit refreshes in one frame.
            if (authoredAvatar)
            {
                // Skin clearance varies while settling and stepping. Probe only the
                // nearby support; retain the last offset in air and during traversals.
                var capsule=GetComponent<CharacterController>();
                if(!Riding&&capsule.enabled)
                {
                    float nearest=float.PositiveInfinity;
                    int count=Physics.RaycastNonAlloc(transform.position+Vector3.up*.12f,Vector3.down,hits,.12f+Mathf.Max(.08f,capsule.skinWidth*1.5f),~0,QueryTriggerInteraction.Ignore);
                    for(int i=0;i<count;i++)
                        if(hits[i].collider&&!hits[i].transform.IsChildOf(transform)&&hits[i].normal.y>.5f&&hits[i].distance<nearest)
                        {nearest=hits[i].distance;VisualGroundOffset=Mathf.Clamp(transform.position.y-hits[i].point.y,0,capsule.skinWidth);}
                }
                Avatar.localPosition = Riding ? Vector3.up * .4f : Vector3.down * VisualGroundOffset;
                Avatar.localScale = Vector3.one * (Riding ? 1 : 1 + VisualGroundOffset / player.standingHeight);
            }
            // Explicit refreshes by interactions/fixtures must not advance the gait twice.
            if (animationFrame == Time.frameCount) return;
            animationFrame = Time.frameCount;
            Vector3 delta = transform.position - lastPosition;
            lastPosition = transform.position;
            delta.y = 0;
            float speed = delta.magnitude / Mathf.Max(.001f, Time.deltaTime);
            if (speed > 12 || Time.deltaTime <= 0) { speed = 0; visualSpeed = 0; }
            visualSpeed = Mathf.MoveTowards(visualSpeed, speed, 24 * Time.deltaTime);
            Vector3 localMotion = transform.InverseTransformDirection(delta);
            if (speed < 9) stride += delta.magnitude * 7;
            float swing = Mathf.Sin(stride) * Mathf.Clamp01(speed / 1.5f) * (player.IsCrouching ? 19 : 30);
            bool phone = SmartphoneController.Instance && SmartphoneController.Instance.isPhoneRaised;
            if (authoredAvatar)
            {
                // The seated clip lowers the pelvis by .4 m; the existing motorcycle
                // mount was authored against the standing avatar's pelvis height.
                float yaw = localMotion.sqrMagnitude > .00001f ? Mathf.Atan2(localMotion.x, Mathf.Abs(localMotion.z)) * Mathf.Rad2Deg : 0;
                yaw = Riding || UiFocus.Cinematic ? 0 : Mathf.Clamp(yaw, phone ? -18 : -60, phone ? 18 : 60);
                bodyYaw = Mathf.LerpAngle(bodyYaw, yaw, 1 - Mathf.Exp(-16 * Time.deltaTime));
                Avatar.localRotation = Quaternion.Euler(0, bodyYaw, 0);
                authoredAvatar.Present(visualSpeed,player.IsCrouching,phone,Riding,new Vector2(localMotion.x,localMotion.z),player.crouchHeight<.9f);
                if(Riding&&transform.parent){if(!riderAlignment)riderAlignment=Avatar.gameObject.AddComponent<MotorcycleRiderAlignment>();riderAlignment.Fit(transform.parent);}
                return;
            }
            if(Riding)
            {
                leftLeg.localRotation=rightLeg.localRotation=Quaternion.Euler(-65,0,0);
                leftArm.localRotation=rightArm.localRotation=Quaternion.Euler(-70,0,0);
                torso.localRotation=Quaternion.Euler(12,0,0);handset.gameObject.SetActive(false);return;
            }
            leftLeg.localRotation = Quaternion.Euler(swing, 0, 0);
            rightLeg.localRotation = Quaternion.Euler(-swing, 0, 0);
            leftArm.localRotation = Quaternion.Euler(-swing * .7f, 0, 7);
            rightArm.localRotation = Quaternion.Euler(phone ? -68 : swing * .7f, phone ? -12 : 0, -7);
            handset.gameObject.SetActive(phone);
            Avatar.localScale = new Vector3(1,Mathf.Clamp(GetComponent<CharacterController>().height/player.standingHeight,.35f,1),1);
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
            var authored=Resources.Load<GameObject>("RakaCharacter");
            if(authored)
            {
                Avatar=Instantiate(authored,transform,false).transform;
                Avatar.name="Raka_AnimatedCharacter";
                authoredAvatar=Avatar.GetComponent<AnimatedRakaAvatar>();
                bodyRenderers=Avatar.GetComponentsInChildren<Renderer>();
                return;
            }
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
