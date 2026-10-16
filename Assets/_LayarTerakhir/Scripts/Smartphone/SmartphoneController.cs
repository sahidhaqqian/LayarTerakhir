using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Smartphone
{
    public enum ScanVisualMode
    {
        Normal = 0,
        AR = 1,
        UV = 2
    }

    public class SmartphoneController : MonoBehaviour
    {
        public static SmartphoneController Instance { get; private set; }

        [Header("Device State")]
        public bool hasSmartphoneInInventory = false;
        public bool isPhoneRaised = false;
        public bool isScreenOn = true;
        public bool isBroadcastConnected = false;
        public bool isSelfieCamera = false;
        public ScanVisualMode currentScanMode = ScanVisualMode.Normal;

        [Header("Module Unlocks (Gated by Story Progression)")]
        public bool arModuleUnlocked = false;
        public bool uvModuleUnlocked = false;
        public bool selfieModuleUnlocked = false;

        [Header("Battery Parameters (GDD Rancangan.MD)")]
        public float batteryPercent = 100f;
        public float normalDrainPerSecond = 1f / 20f;
        public float arOrUvDrainPerSecond = 1f / 10f;
        public float photoCostPercent = 1f;
        public float blitzCostPercent = 15f;
        public float blitzCooldownSeconds = 6f;
        public float largePowerbankRestore = 80f;
        public float smallPowerbankRestore = 35f;

        [Header("Visual Rig References")]
        public GameObject handPhoneMeshRoot;
        public Light phoneFlashlight;

        private float _blitzCooldownTimer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SyncVisualState();
        }

        private void Update()
        {
            if (_blitzCooldownTimer > 0f)
                _blitzCooldownTimer -= Time.deltaTime;

            if (!hasSmartphoneInInventory || UiFocus.Cinematic)
                return;

            if (HudController.Instance != null && HudController.Instance.IsModalOpen)
                return;

            HandlePhoneInput();
            UpdateBatteryDrain();
        }

        public void AcquireSmartphone()
        {
            hasSmartphoneInInventory = true;
            isPhoneRaised = true;
            isScreenOn = true;
            SyncVisualState();
        }

        private void HandlePhoneInput()
        {
            if (GameInputConfig.Instance == null)
                return;

            if (GameInputConfig.Instance.GetActionDown(GameAction.TogglePhone))
            {
                isPhoneRaised = !isPhoneRaised;
                SyncVisualState();
            }

            if (GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen))
            {
                if (batteryPercent > 0f)
                {
                    isScreenOn = !isScreenOn;
                    SyncVisualState();
                }
            }

            // Recharging must remain possible after battery depletion or while the phone is stored.
            if (GameInputConfig.Instance.GetActionDown(GameAction.UsePowerbank))
            {
                TryUsePowerbank();
            }

            if (!isPhoneRaised || !isScreenOn)
                return;

            if (GameInputConfig.Instance.GetActionDown(GameAction.ModeAR) && arModuleUnlocked)
            {
                currentScanMode = currentScanMode == ScanVisualMode.AR
                    ? ScanVisualMode.Normal
                    : ScanVisualMode.AR;
                SyncVisualState();
            }
            else if (GameInputConfig.Instance.GetActionDown(GameAction.ModeUV) && uvModuleUnlocked)
            {
                currentScanMode = currentScanMode == ScanVisualMode.UV
                    ? ScanVisualMode.Normal
                    : ScanVisualMode.UV;
                SyncVisualState();
            }

            if (GameInputConfig.Instance.GetActionDown(GameAction.ModeSelfie) && selfieModuleUnlocked)
            {
                isSelfieCamera = !isSelfieCamera;
                SyncVisualState();
            }

        }

        private void UpdateBatteryDrain()
        {
            if (!isPhoneRaised || !isScreenOn || batteryPercent <= 0f)
                return;

            float drainRate = (currentScanMode == ScanVisualMode.AR || currentScanMode == ScanVisualMode.UV)
                ? arOrUvDrainPerSecond
                : normalDrainPerSecond;

            ConsumeBattery(drainRate * Time.deltaTime);
        }

        public void ConsumeBattery(float amount)
        {
            batteryPercent = Mathf.Clamp(batteryPercent - amount, 0f, 100f);
            if (CampaignState.Instance != null)
            {
                CampaignState.Instance.Data.battery = batteryPercent;
            }

            if (batteryPercent <= 0f && isScreenOn)
            {
                isScreenOn = false;
                currentScanMode = ScanVisualMode.Normal;
                SyncVisualState();
            }

            if (HudController.Instance != null)
            {
                HudController.Instance.UpdatePhoneStatusUI(this);
            }
        }

        public bool TryUsePowerbank()
        {
            if (CampaignState.Instance == null || batteryPercent >= 100f)
                return false;

            if (CampaignState.Instance.Data.inventory.Remove("POWERBANK_BESAR"))
            {
                batteryPercent = Mathf.Clamp(batteryPercent + largePowerbankRestore, 0f, 100f);
                CampaignState.Instance.Data.battery = batteryPercent;
                SyncVisualState();
                return true;
            }

            if (CampaignState.Instance.Data.inventory.Remove("POWERBANK_KECIL_KOS"))
            {
                batteryPercent = Mathf.Clamp(batteryPercent + smallPowerbankRestore, 0f, 100f);
                CampaignState.Instance.Data.battery = batteryPercent;
                SyncVisualState();
                return true;
            }

            return false;
        }

        public void SyncVisualState()
        {
            if (handPhoneMeshRoot != null)
            {
                handPhoneMeshRoot.SetActive(hasSmartphoneInInventory && isPhoneRaised);
            }

            if (phoneFlashlight != null)
            {
                phoneFlashlight.enabled = hasSmartphoneInInventory && isPhoneRaised && isScreenOn && batteryPercent > 0f;
            }

            if (HudController.Instance != null)
            {
                HudController.Instance.UpdatePhoneStatusUI(this);
            }
        }
    }
}
