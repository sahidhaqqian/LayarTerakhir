using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LayarTerakhir.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("References")]
        public Transform cameraPivot;

        [Header("Movement Speeds")]
        public float walkSpeed = 2.8f;
        public float sprintSpeed = 4.6f;
        public float crouchSpeed = 1.6f;
        public float slideSpeed = 5.5f;
        public float slideDuration = 0.65f;

        [Header("Posture & Gravity")]
        public float standingHeight = 1.75f;
        public float crouchHeight = 1.0f;
        public float cameraStandingY = 1.6f;
        public float cameraCrouchY = 0.85f;
        public float postureLerpSpeed = 10f;
        public float gravity = -19.62f;
        public float vaultImpulse = 4.8f;

        [Header("Look Limits")]
        public float minPitch = -80f;
        public float maxPitch = 80f;

        public bool MovementLocked { get; set; }
        public bool IsCrouching { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool IsSliding { get; private set; }

        private CharacterController _controller;
        private float _pitch;
        private Vector3 _verticalVelocity;
        private float _slideTimer;
        private Vector3 _slideDirection;
        private float _standingStepOffset;
        private readonly Collider[] _headroomHits = new Collider[24];
        public static bool PhoneLookHeld => UiFocus.Phone && Mouse.current != null && Mouse.current.rightButton.isPressed;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _standingStepOffset = _controller.stepOffset;
            _controller.height = standingHeight;
            _controller.center = new Vector3(0f, standingHeight * 0.5f, 0f);
            if (!GetComponent<ShoulderCameraRig>()) gameObject.AddComponent<ShoulderCameraRig>();
        }

        private void Start()
        {
            SetCursorLock(!UiFocus.BlocksWorld);
            if (!GetComponent<LayarTerakhir.Prolog.ExplorationCommentary>()) gameObject.AddComponent<LayarTerakhir.Prolog.ExplorationCommentary>();
        }

        private void Update()
        {
            bool modalOpen = HudController.Instance != null && HudController.Instance.IsModalOpen;
            if (modalOpen || MovementLocked || UiFocus.BlocksMovement)
            {
                ApplyGravityOnly();
                return;
            }

            if (!UiFocus.Phone || PhoneLookHeld) HandleMouseLook();
            HandleMovementAndPosture();
        }

        public void SetCursorLock(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }

        private void HandleMouseLook()
        {
            if (GameInputConfig.Instance == null || cameraPivot == null)
                return;

            Vector2 look = GameInputConfig.Instance.GetLookDelta();
            transform.Rotate(Vector3.up * look.x);

            _pitch = Mathf.Clamp(_pitch - look.y, minPitch, maxPitch);
            cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        private void HandleMovementAndPosture()
        {
            if (GameInputConfig.Instance == null)
                return;

            Vector2 input = GameInputConfig.Instance.GetMovementInput();
            Vector3 desiredMove = (transform.right * input.x + transform.forward * input.y);
            if (desiredMove.sqrMagnitude > 1f)
                desiredMove.Normalize();

            bool crouchHeld = GameInputConfig.Instance.GetActionHeld(GameAction.Crouch);
            bool crouchDown = GameInputConfig.Instance.GetActionDown(GameAction.Crouch);
            bool sprintHeld = GameInputConfig.Instance.GetActionHeld(GameAction.Sprint);

            if (IsSliding)
            {
                _slideTimer -= Time.deltaTime;
                if (_slideTimer <= 0f)
                {
                    IsSliding = false;
                }
            }
            else if (crouchDown && sprintHeld && input.y > 0.5f && _controller.isGrounded)
            {
                IsSliding = true;
                _slideTimer = slideDuration;
                _slideDirection = desiredMove.sqrMagnitude > 0.01f ? desiredMove : transform.forward;
            }

            IsCrouching = crouchHeld || IsSliding || !CanStandUp();
            IsSprinting = !IsCrouching && sprintHeld && input.y > 0.1f;

            // Full-height step-up sweeps catch the underside of furniture even when the
            // crouched capsule fits. Crawl with a small step, restore the normal stair step on stand.
            _controller.stepOffset = IsCrouching ? Mathf.Min(_standingStepOffset, .06f) : _standingStepOffset;

            float targetHeight = IsCrouching ? crouchHeight : standingHeight;
            _controller.height = Mathf.Lerp(_controller.height, targetHeight, Time.deltaTime * postureLerpSpeed);
            _controller.center = new Vector3(0f, _controller.height * 0.5f, 0f);

            if (cameraPivot != null)
            {
                float targetCamY = IsCrouching ? cameraCrouchY : cameraStandingY;
                Vector3 localPos = cameraPivot.localPosition;
                localPos.y = Mathf.Lerp(localPos.y, targetCamY, Time.deltaTime * postureLerpSpeed);
                cameraPivot.localPosition = localPos;
            }

            float currentSpeed = walkSpeed;
            if (IsSliding) currentSpeed = slideSpeed;
            else if (IsCrouching) currentSpeed = crouchSpeed;
            else if (IsSprinting) currentSpeed = sprintSpeed;

            Vector3 horizontalVelocity = IsSliding ? _slideDirection * currentSpeed : desiredMove * currentSpeed;

            if (_controller.isGrounded)
            {
                if (_verticalVelocity.y < 0f)
                    _verticalVelocity.y = -2f;

                if (GameInputConfig.Instance.GetActionDown(GameAction.Vault) && !IsCrouching)
                {
                    _verticalVelocity.y = vaultImpulse;
                }
            }

            _verticalVelocity.y += gravity * Time.deltaTime;
            Vector3 finalMotion = horizontalVelocity + _verticalVelocity;
            _controller.Move(finalMotion * Time.deltaTime);
        }

        private void ApplyGravityOnly()
        {
            if (_controller.isGrounded && _verticalVelocity.y < 0f)
                _verticalVelocity.y = -2f;

            _verticalVelocity.y += gravity * Time.deltaTime;
            _controller.Move(_verticalVelocity * Time.deltaTime);
        }

        public bool CanStandUp()
        {
            if (!IsCrouching && _controller.height >= standingHeight - .05f) return true;
            // A cast misses a tabletop already overlapping its start sphere. Test the entire
            // prospective standing capsule, excluding our own hull and harmless triggers.
            float radius = Mathf.Max(.05f, _controller.radius - .025f);
            Vector3 bottom = transform.position + Vector3.up * (radius + .025f);
            Vector3 top = transform.position + Vector3.up * (standingHeight - radius);
            int count = Physics.OverlapCapsuleNonAlloc(bottom, top, radius, _headroomHits, ~0, QueryTriggerInteraction.Ignore);
            var hits = count == _headroomHits.Length
                ? Physics.OverlapCapsule(bottom, top, radius, ~0, QueryTriggerInteraction.Ignore) : _headroomHits;
            if (hits != _headroomHits) count = hits.Length;
            for (int i = 0; i < count; i++)
                if (hits[i] && hits[i] != _controller && !hits[i].transform.IsChildOf(transform)) return false;
            return true;
        }
    }
}
