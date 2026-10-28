#if UNITY_EDITOR || DEVELOPMENT_BUILD
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LayarTerakhir.Review
{
    // First-person fly camera. Drives the existing eye camera directly; the player
    // transform is never touched, so exiting cannot teleport or break Raka.
    public sealed class ReviewFlyCamera : MonoBehaviour
    {
        public const float MinSpeed = 0.5f;
        public const float MaxSpeed = 60f;
        public const float DefaultSpeed = 6f;
        public const float SprintMultiplier = 4f;

        public bool Active { get; private set; }
        public float FlySpeed = DefaultSpeed;

        private FirstPersonController player;
        private ShoulderCameraRig rig;
        private InteractionInteractor interactor;
        private Camera eye;
        private Vector3 savedEyePos;
        private Quaternion savedEyeRot;
        private Quaternion savedPivotRot;
        private bool savedController, savedRig, savedInteractor;
        private bool writesBlockedByUs;
        private float yaw, pitch;

        public Camera Eye => eye;

        public static float AdjustSpeed(float current, float scrollDelta)
        {
            if (Mathf.Abs(scrollDelta) < 0.001f) return current;
            float next = current * (scrollDelta > 0 ? 1.2f : 1f / 1.2f);
            return Mathf.Clamp(next, MinSpeed, MaxSpeed);
        }

        public static Vector3 StepPosition(Vector3 pos, Vector3 forward, Vector3 right,
            float moveX, float moveY, float vertical, float speed, float sprint, float dt)
        {
            Vector3 dir = right * moveX + forward * moveY + Vector3.up * vertical;
            if (dir.sqrMagnitude > 1f) dir.Normalize();
            return pos + dir * (speed * sprint * Mathf.Max(0f, dt));
        }

        public bool Enable(FirstPersonController target)
        {
            if (Active) return true;
            player = target ? target : FindFirstObjectByType<FirstPersonController>();
            if (player) interactor = player.GetComponent<InteractionInteractor>();
            if (player) rig = player.GetComponent<ShoulderCameraRig>();
            eye = interactor && interactor.playerCamera ? interactor.playerCamera : Camera.main;
            if (!eye) return false;

            savedEyePos = eye.transform.position;
            savedEyeRot = eye.transform.rotation;
            if (player && player.cameraPivot) savedPivotRot = player.cameraPivot.localRotation;
            savedController = player && player.enabled;
            savedRig = rig && rig.enabled;
            savedInteractor = interactor && interactor.enabled;
            if (player) player.enabled = false;
            if (rig) rig.enabled = false;
            if (interactor) interactor.enabled = false;

            // Inspection flights must never mutate the campaign on disk. During a review
            // excursion writes are already redirected to a temp slot, so skip blocking there.
            var nav = GetComponent<ReviewPhaseNavigator>();
            bool excursion = nav && nav.ExcursionActive;
            var campaign = CampaignState.Instance;
            writesBlockedByUs = !excursion && campaign && !campaign.WritesBlocked && campaign.BeginCheckpointAttempt(this);

            Vector3 e = eye.transform.rotation.eulerAngles;
            yaw = e.y;
            pitch = e.x > 180f ? e.x - 360f : e.x;
            if (player) player.SetCursorLock(true);
            else { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
            Active = true;
            return true;
        }

        // Scene unload path: components are going away, only release local state.
        public void ExitForSceneLoad()
        {
            if (!Active) return;
            Active = false;
            eye = null;
            player = null;
            rig = null;
            interactor = null;
            ReleaseWriteBlock();
        }

        public void Disable()
        {
            if (!Active) return;
            Active = false;
            if (eye)
            {
                eye.transform.SetPositionAndRotation(savedEyePos, savedEyeRot);
            }
            if (player && player.cameraPivot) player.cameraPivot.localRotation = savedPivotRot;
            if (player) player.enabled = savedController;
            if (rig)
            {
                rig.enabled = savedRig;
                if (savedRig) rig.RefreshCameraPose();
            }
            if (interactor) interactor.enabled = savedInteractor;
            if (player) player.SetCursorLock(!UiFocus.BlocksWorld);
            else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            ReleaseWriteBlock();
            eye = null;
            player = null;
            rig = null;
            interactor = null;
        }

        private void ReleaseWriteBlock()
        {
            if (!writesBlockedByUs) return;
            writesBlockedByUs = false;
            if (CampaignState.Instance) CampaignState.Instance.EndCheckpointAttempt(this);
        }

        private void OnDisable()
        {
            if (Active) Disable();
        }

        private void Update()
        {
            if (!Active || !eye) return;
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current == null) return;
            if (DeveloperReviewMode.InputFieldFocused) return;
            var keys = Keyboard.current;
            float dt = Time.unscaledDeltaTime;

            if (Mouse.current != null && UiFocus.ApplicationFocused)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                float sens = GameInputConfig.Instance ? GameInputConfig.Instance.mouseSensitivity * 0.1f : 0.2f;
                yaw += delta.x * sens;
                pitch = Mathf.Clamp(pitch - delta.y * sens, -89f, 89f);
                eye.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
                FlySpeed = AdjustSpeed(FlySpeed, Mouse.current.scroll.ReadValue().y / 120f);
            }

            float mx = (keys.dKey.isPressed || keys.rightArrowKey.isPressed ? 1f : 0f)
                - (keys.aKey.isPressed || keys.leftArrowKey.isPressed ? 1f : 0f);
            float my = (keys.wKey.isPressed || keys.upArrowKey.isPressed ? 1f : 0f)
                - (keys.sKey.isPressed || keys.downArrowKey.isPressed ? 1f : 0f);
            float mv = (keys.eKey.isPressed ? 1f : 0f) - (keys.qKey.isPressed ? 1f : 0f);
            float sprint = (keys.leftShiftKey.isPressed || keys.rightShiftKey.isPressed) ? SprintMultiplier : 1f;
            eye.transform.position = StepPosition(eye.transform.position,
                eye.transform.forward, eye.transform.right, mx, my, mv, FlySpeed, sprint, dt);
#else
            return;
#endif
        }
    }
}
#endif
