using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LayarTerakhir.Core
{
    public enum GameAction
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
        Sprint,
        Crouch,
        Vault,
        Interact,
        TogglePhone,
        ToggleScreen,
        ModeAR,
        ModeUV,
        ModeSelfie,
        ModeThermo,
        ModeEVP,
        OpenJournal,
        UsePowerbank,
        UseFlare,
        UseIncense,
        CaptureOrBlitz
    }

    [Serializable]
    public class GameInputConfig : MonoBehaviour
    {
        public static GameInputConfig Instance { get; private set; }

        [Header("Remappable Keybindings (GDD Standard)")]
        public KeyCode moveForwardKey = KeyCode.W;
        public KeyCode moveBackwardKey = KeyCode.S;
        public KeyCode moveLeftKey = KeyCode.A;
        public KeyCode moveRightKey = KeyCode.D;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;
        public KeyCode vaultKey = KeyCode.Space;
        public KeyCode interactKey = KeyCode.E;
        public KeyCode togglePhoneKey = KeyCode.Tab;
        public KeyCode toggleScreenKey = KeyCode.F;
        public KeyCode modeARKey = KeyCode.Q;
        public KeyCode modeUVKey = KeyCode.R;
        public KeyCode modeSelfieKey = KeyCode.V;
        public KeyCode modeThermoKey = KeyCode.T;
        public KeyCode modeEVPKey = KeyCode.C;
        public KeyCode openJournalKey = KeyCode.J;
        public KeyCode usePowerbankKey = KeyCode.B;
        public KeyCode useFlareKey = KeyCode.G;
        public KeyCode useIncenseKey = KeyCode.H;
        public KeyCode captureOrBlitzKey = KeyCode.Mouse0;

        [Header("Mouse Sensitivity")]
        public float mouseSensitivity = 2.0f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void RemapAction(GameAction action, KeyCode newKey)
        {
            switch (action)
            {
                case GameAction.MoveForward: moveForwardKey = newKey; break;
                case GameAction.MoveBackward: moveBackwardKey = newKey; break;
                case GameAction.MoveLeft: moveLeftKey = newKey; break;
                case GameAction.MoveRight: moveRightKey = newKey; break;
                case GameAction.Sprint: sprintKey = newKey; break;
                case GameAction.Crouch: crouchKey = newKey; break;
                case GameAction.Vault: vaultKey = newKey; break;
                case GameAction.Interact: interactKey = newKey; break;
                case GameAction.TogglePhone: togglePhoneKey = newKey; break;
                case GameAction.ToggleScreen: toggleScreenKey = newKey; break;
                case GameAction.ModeAR: modeARKey = newKey; break;
                case GameAction.ModeUV: modeUVKey = newKey; break;
                case GameAction.ModeSelfie: modeSelfieKey = newKey; break;
                case GameAction.ModeThermo: modeThermoKey = newKey; break;
                case GameAction.ModeEVP: modeEVPKey = newKey; break;
                case GameAction.OpenJournal: openJournalKey = newKey; break;
                case GameAction.UsePowerbank: usePowerbankKey = newKey; break;
                case GameAction.UseFlare: useFlareKey = newKey; break;
                case GameAction.UseIncense: useIncenseKey = newKey; break;
                case GameAction.CaptureOrBlitz: captureOrBlitzKey = newKey; break;
            }
        }

        public KeyCode GetKeyForAction(GameAction action)
        {
            return action switch
            {
                GameAction.MoveForward => moveForwardKey,
                GameAction.MoveBackward => moveBackwardKey,
                GameAction.MoveLeft => moveLeftKey,
                GameAction.MoveRight => moveRightKey,
                GameAction.Sprint => sprintKey,
                GameAction.Crouch => crouchKey,
                GameAction.Vault => vaultKey,
                GameAction.Interact => interactKey,
                GameAction.TogglePhone => togglePhoneKey,
                GameAction.ToggleScreen => toggleScreenKey,
                GameAction.ModeAR => modeARKey,
                GameAction.ModeUV => modeUVKey,
                GameAction.ModeSelfie => modeSelfieKey,
                GameAction.ModeThermo => modeThermoKey,
                GameAction.ModeEVP => modeEVPKey,
                GameAction.OpenJournal => openJournalKey,
                GameAction.UsePowerbank => usePowerbankKey,
                GameAction.UseFlare => useFlareKey,
                GameAction.UseIncense => useIncenseKey,
                GameAction.CaptureOrBlitz => captureOrBlitzKey,
                _ => KeyCode.None
            };
        }

        public bool GetActionDown(GameAction action) => ReadKeyDown(GetKeyForAction(action));
        public bool GetActionHeld(GameAction action) => ReadKeyHeld(GetKeyForAction(action));
        public bool GetActionUp(GameAction action) => ReadKeyUp(GetKeyForAction(action));

        public Vector2 GetMovementInput()
        {
            float x = 0f;
            float y = 0f;
            if (GetActionHeld(GameAction.MoveRight)) x += 1f;
            if (GetActionHeld(GameAction.MoveLeft)) x -= 1f;
            if (GetActionHeld(GameAction.MoveForward)) y += 1f;
            if (GetActionHeld(GameAction.MoveBackward)) y -= 1f;

            Vector2 move = new Vector2(x, y);
            return move.sqrMagnitude > 1f ? move.normalized : move;
        }

        public Vector2 GetLookDelta()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                return delta * (mouseSensitivity * 0.1f);
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return new Vector2(
                Input.GetAxisRaw("Mouse X") * mouseSensitivity,
                Input.GetAxisRaw("Mouse Y") * mouseSensitivity
            );
#else
            return Vector2.zero;
#endif
        }

        private static bool ReadKeyDown(KeyCode keyCode)
        {
#if ENABLE_INPUT_SYSTEM
            if (TryReadInputSystemKey(keyCode, out bool down, out _, out _))
                return down;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(keyCode);
#else
            return false;
#endif
        }

        private static bool ReadKeyHeld(KeyCode keyCode)
        {
#if ENABLE_INPUT_SYSTEM
            if (TryReadInputSystemKey(keyCode, out _, out bool held, out _))
                return held;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKey(keyCode);
#else
            return false;
#endif
        }

        private static bool ReadKeyUp(KeyCode keyCode)
        {
#if ENABLE_INPUT_SYSTEM
            if (TryReadInputSystemKey(keyCode, out _, out _, out bool up))
                return up;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyUp(keyCode);
#else
            return false;
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private static bool TryReadInputSystemKey(KeyCode keyCode, out bool down, out bool held, out bool up)
        {
            down = false;
            held = false;
            up = false;

            if (keyCode >= KeyCode.Mouse0 && keyCode <= KeyCode.Mouse4)
            {
                if (Mouse.current == null) return false;
                var button = keyCode switch
                {
                    KeyCode.Mouse1 => Mouse.current.rightButton,
                    KeyCode.Mouse2 => Mouse.current.middleButton,
                    KeyCode.Mouse3 => Mouse.current.backButton,
                    KeyCode.Mouse4 => Mouse.current.forwardButton,
                    _ => Mouse.current.leftButton
                };
                down = button.wasPressedThisFrame;
                held = button.isPressed;
                up = button.wasReleasedThisFrame;
                return true;
            }

            if (Keyboard.current == null) return false;

            Key mapped = keyCode switch
            {
                KeyCode.W => Key.W,
                KeyCode.A => Key.A,
                KeyCode.S => Key.S,
                KeyCode.D => Key.D,
                KeyCode.E => Key.E,
                KeyCode.F => Key.F,
                KeyCode.Q => Key.Q,
                KeyCode.R => Key.R,
                KeyCode.V => Key.V,
                KeyCode.T => Key.T,
                KeyCode.C => Key.C,
                KeyCode.J => Key.J,
                KeyCode.B => Key.B,
                KeyCode.G => Key.G,
                KeyCode.H => Key.H,
                KeyCode.Tab => Key.Tab,
                KeyCode.Space => Key.Space,
                KeyCode.LeftShift => Key.LeftShift,
                KeyCode.LeftControl => Key.LeftCtrl,
                KeyCode.Escape => Key.Escape,
                KeyCode.RightControl => Key.RightCtrl,
                KeyCode.Return => Key.Enter,
                KeyCode.KeypadEnter => Key.NumpadEnter,
                KeyCode.KeypadPlus => Key.NumpadPlus,
                KeyCode.KeypadMinus => Key.NumpadMinus,
                KeyCode.KeypadMultiply => Key.NumpadMultiply,
                KeyCode.KeypadDivide => Key.NumpadDivide,
                KeyCode.KeypadPeriod => Key.NumpadPeriod,
                KeyCode.KeypadEquals => Key.NumpadEquals,
                KeyCode.LeftCommand => Key.LeftMeta,
                KeyCode.RightCommand => Key.RightMeta,
                KeyCode.LeftWindows => Key.LeftMeta,
                KeyCode.RightWindows => Key.RightMeta,
                _ => MapOtherKey(keyCode)
            };

            if (mapped == Key.None) return false;
            var control = Keyboard.current[mapped];
            down = control.wasPressedThisFrame;
            held = control.isPressed;
            up = control.wasReleasedThisFrame;
            return true;
        }

        private static Key MapOtherKey(KeyCode keyCode)
        {
            if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
                return keyCode == KeyCode.Alpha0 ? Key.Digit0
                    : (Key)((int)Key.Digit1 + (int)keyCode - (int)KeyCode.Alpha1);
            if (keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9)
                return (Key)((int)Key.Numpad0 + (int)keyCode - (int)KeyCode.Keypad0);
            return Enum.TryParse(keyCode.ToString(), true, out Key key) && Enum.IsDefined(typeof(Key), key)
                ? key : Key.None;
        }
#endif
    }
}
