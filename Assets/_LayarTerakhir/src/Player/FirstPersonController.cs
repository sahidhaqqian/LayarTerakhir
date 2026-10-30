using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LayarTerakhir.Player
{
    [RequireComponent(typeof(CharacterController))]
    [DefaultExecutionOrder(-40)]
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

        [Header("Locomotion Response")]
        public float walkAcceleration = 9f;
        public float runAcceleration = 13f;
        public float crouchAcceleration = 6f;
        public float braking = 24f;
        public float turnAcceleration = 28f;

        [Header("Posture & Gravity")]
        public float standingHeight = 1.75f;
        public float crouchHeight = 1.0f;
        public float cameraStandingY = 1.6f;
        public float cameraCrouchY = 0.85f;
        public float postureLerpSpeed = 10f;
        public float crouchEyeLift = .08f;
        public float gravity = -19.62f;
        public float vaultImpulse = 4.8f;

        [Header("Look Limits")]
        public float minPitch = -80f;
        public float maxPitch = 80f;

        public bool CursorLockRequested {get;private set;}
        public bool MovementLocked { get; set; }
        public bool IsCrouching { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool IsSliding { get; private set; }
        public Vector3 PlanarVelocity => _horizontalVelocity;
        public float CrouchAmount => Mathf.InverseLerp(standingHeight, crouchHeight, _controller ? _controller.height : standingHeight);

        private CharacterController _controller;
        private float _pitch;
        private Vector3 _verticalVelocity;
        private float _slideTimer;
        private Vector3 _slideDirection;
        private float _standingStepOffset;
        private Vector3 _horizontalVelocity;
        private float _cameraHeightVelocity;
        private readonly Collider[] _headroomHits = new Collider[24];
        private int ignoreLookThroughFrame;
        public static bool PhoneLookHeld => UiFocus.ApplicationFocused && UiFocus.Phone && UiFocus.PhoneAiming;
        private bool phoneDragCandidate, phoneDragLooking;
        private Vector2 phoneDragStart;
        private readonly List<RaycastResult> phonePointerHits = new List<RaycastResult>();

        private void Awake()
        {
            // Match human stride lengths in the authored third-person clips.
            walkSpeed=Mathf.Min(walkSpeed,1.7f);
            sprintSpeed=Mathf.Min(sprintSpeed,4.2f);
            crouchSpeed=Mathf.Min(crouchSpeed,.8f);
            _controller = GetComponent<CharacterController>();
            _standingStepOffset = _controller.stepOffset;
            _controller.height = standingHeight;
            _controller.center = new Vector3(0f, standingHeight * 0.5f, 0f);
            if (!GetComponent<ShoulderCameraRig>()) gameObject.AddComponent<ShoulderCameraRig>();
            AbandonedLocationLighting.Install(gameObject);
        }

        private void Start()
        {
            SetCursorLock(!UiFocus.BlocksWorld);
            if (!GetComponent<LayarTerakhir.Prolog.ExplorationCommentary>()) gameObject.AddComponent<LayarTerakhir.Prolog.ExplorationCommentary>();
        }

        private void Update()
        {
            // Story vaults/climbs temporarily own the transform and disable the capsule.
            // Gravity must not call Move on that inactive controller during a traverse.
            if (_controller == null || !_controller.enabled)
            {
                _horizontalVelocity = Vector3.zero;
                return;
            }
            bool modalOpen = HudController.Instance != null && HudController.Instance.IsModalOpen;
            if (modalOpen || MovementLocked || UiFocus.BlocksMovement)
            {
                phoneDragCandidate=phoneDragLooking=false;
                _horizontalVelocity = Vector3.zero;
                ApplyGravityOnly();
                return;
            }

            UpdatePhoneDrag();
            if (!UiFocus.Phone || PhoneLookHeld || phoneDragLooking) HandleMouseLook();
            HandleMovementAndPosture();
        }

        private void OnDisable() { _horizontalVelocity = Vector3.zero; phoneDragCandidate=phoneDragLooking=false; }

        private void UpdatePhoneDrag()
        {
            var mouse=Mouse.current;
            if(!UiFocus.Phone||PhoneLookHeld||mouse==null||!mouse.leftButton.isPressed)
            {phoneDragCandidate=phoneDragLooking=false;return;}
            if(mouse.leftButton.wasPressedThisFrame)
            {
                phoneDragStart=mouse.position.ReadValue();phoneDragCandidate=true;
                // A press begun on an app button/scrollbar belongs to that control.
                if(EventSystem.current)
                {
                    phonePointerHits.Clear();
                    EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current){position=phoneDragStart},phonePointerHits);
                    foreach(var hit in phonePointerHits)if(hit.gameObject.GetComponentInParent<Selectable>()||hit.gameObject.GetComponentInParent<ScrollRect>())
                    {phoneDragCandidate=false;break;}
                }
            }
            if(phoneDragCandidate&&(mouse.position.ReadValue()-phoneDragStart).sqrMagnitude>=36)phoneDragLooking=true;
        }

        // The capsule retains movement ownership; visual animation never supplies root motion.
        public static Vector3 StepPlanarVelocity(Vector3 current, Vector3 target, float dt,
            float acceleration, float deceleration, float steering)
        {
            current.y = target.y = 0;
            float rate = target.sqrMagnitude < .0001f || target.sqrMagnitude < current.sqrMagnitude
                ? deceleration : acceleration;
            if (current.sqrMagnitude > .01f && target.sqrMagnitude > .01f && Vector3.Dot(current.normalized, target.normalized) < .5f)
                rate = Mathf.Max(rate, steering);
            return Vector3.MoveTowards(current, target, Mathf.Max(0, rate) * Mathf.Max(0, dt));
        }

        private void OnApplicationFocus(bool focused)
        {
            UiFocus.ApplicationFocused=focused;
            phoneDragCandidate=phoneDragLooking=false;
            _horizontalVelocity=Vector3.zero;
            // Discard the window-system pointer delta from focus/cursor recapture.
            ignoreLookThroughFrame=Time.frameCount+1;
            SetCursorLock(focused&&!UiFocus.BlocksWorld);
        }
        private void OnApplicationPause(bool paused)=>OnApplicationFocus(!paused);
        public void SetCursorLock(bool locked)
        {
            locked &= UiFocus.ApplicationFocused&&!UiFocus.HardModal&&!UiFocus.Cinematic;
            CursorLockRequested=locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
        public void SetViewPitch(float pitch)
        {
            _pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            if (cameraPivot) cameraPivot.localRotation = Quaternion.Euler(_pitch, 0, 0);
        }

        private void HandleMouseLook()
        {
            if (GameInputConfig.Instance == null || cameraPivot == null || !UiFocus.ApplicationFocused || Time.frameCount<=ignoreLookThroughFrame)
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
            _controller.height = Mathf.Lerp(_controller.height, targetHeight, 1f - Mathf.Exp(-postureLerpSpeed * Time.deltaTime));
            _controller.center = new Vector3(0f, _controller.height * 0.5f, 0f);

            if (cameraPivot != null)
            {
                float targetCamY = IsCrouching ? Mathf.Min(crouchHeight - .04f, cameraCrouchY + crouchEyeLift) : cameraStandingY;
                Vector3 localPos = cameraPivot.localPosition;
                localPos.y = Mathf.SmoothDamp(localPos.y, targetCamY, ref _cameraHeightVelocity, .105f, Mathf.Infinity, Time.deltaTime);
                cameraPivot.localPosition = localPos;
            }

            float currentSpeed = walkSpeed;
            if (IsSliding) currentSpeed = slideSpeed;
            else if (IsCrouching) currentSpeed = crouchSpeed;
            else if (IsSprinting) currentSpeed = sprintSpeed;

            // Enter low clearance at crouch speed immediately; do not carry sprint momentum under furniture.
            if (IsCrouching && !IsSliding) _horizontalVelocity = Vector3.ClampMagnitude(_horizontalVelocity, crouchSpeed);
            _horizontalVelocity = IsSliding ? _slideDirection * currentSpeed : StepPlanarVelocity(
                _horizontalVelocity, desiredMove * currentSpeed, Time.deltaTime,
                IsCrouching ? crouchAcceleration : IsSprinting ? runAcceleration : walkAcceleration, braking, turnAcceleration);

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
            Vector3 finalMotion = _horizontalVelocity + _verticalVelocity;
            CollisionFlags collision = _controller.Move(finalMotion * Time.deltaTime);
            // Remove velocity into a wall, so releasing it cannot leave stored momentum.
            if (Time.deltaTime > 0 && (collision & CollisionFlags.Sides) != 0)
            {
                Vector3 actual = _controller.velocity; actual.y = 0;
                if (actual.sqrMagnitude < _horizontalVelocity.sqrMagnitude * .04f) _horizontalVelocity = actual;
            }
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
