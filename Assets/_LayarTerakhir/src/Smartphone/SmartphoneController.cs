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
        public IPhoneChapterContent ChapterContent { get; set; }
        public bool RitualCameraPower => ChapterContent is IPhoneRitualPower ritual && ritual.RitualCameraPower;
        public bool HasCameraPower => batteryPercent > 0f || RitualCameraPower;
        public float DisplayBatteryPercent => RitualCameraPower ? 5f : batteryPercent;
        public float BlitzCooldownRemaining => Mathf.Max(0, _blitzCooldownTimer);
        public bool CameraLocked => ChapterContent != null && ChapterContent.CameraLocked;
        private Light spillLight;
        private float shutterUntil;

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
            if (ChapterContent is IPhoneChapterClock clock && !clock.PhoneClockRunning) return;
            if (_blitzCooldownTimer > 0f)
                _blitzCooldownTimer -= Time.deltaTime;

            if (!hasSmartphoneInInventory || UiFocus.Cinematic || UiFocus.HardModal)
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

            if (CameraLocked)
            {
                if (GameInputConfig.Instance.GetActionDown(GameAction.TogglePhone) ||
                    GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen))
                    HudController.Instance?.ShowBroadcastNotification("Jaga kamera menghadap Kuntilanak. Mundur menuju rantai.", 2);
                return;
            }

            if (GameInputConfig.Instance.GetActionDown(GameAction.TogglePhone))
            {
                isPhoneRaised = !isPhoneRaised;
                SyncVisualState();
            }

            if (GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen))
            {
                if (HasCameraPower)
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
            if (RitualCameraPower) return;
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

            string instance = CampaignState.Instance.Data.inventory.Find(id =>
                id.StartsWith("P5_POWERBANK_") || id.StartsWith("P4_POWERBANK_") || id.StartsWith("P3_POWERBANK_") || id == "P1_POWERBANK_MAILBOX_207" || id == "P1_POWERBANK_ARRIVAL" ||
                id == "P2_POWERBANK_PASAR_GAIB_A" || id == "P2_POWERBANK_PASAR_GAIB_B" || id == "POWERBANK_BESAR" || id == "POWERBANK_KECIL_KOS");
            if (instance != null)
            {
                var campaign = CampaignState.Instance;
                float restored = Mathf.Min(100, batteryPercent + (instance == "POWERBANK_KECIL_KOS" ? smallPowerbankRestore : largePowerbankRestore));
                if (campaign.WritesBlocked)
                { campaign.Data.inventory.Remove(instance); campaign.Data.battery = restored; }
                else if (!campaign.TryCommit(campaign.Data.checkpoint_id, data =>
                    { data.inventory.Remove(instance); data.battery = restored; })) return false;
                batteryPercent = restored; SyncVisualState(); return true;
            }

            return false;
        }

        public bool TryCaptureEnergy(bool blitz)
        {
            float cost = blitz ? blitzCostPercent : photoCostPercent;
            if (!hasSmartphoneInInventory || !isPhoneRaised || !isScreenOn || batteryPercent < cost ||
                (blitz && _blitzCooldownTimer > 0)) return false;
            if (blitz) _blitzCooldownTimer = blitzCooldownSeconds;
            ConsumeBattery(cost); return true;
        }

        public void RestoreBattery(float value)
        {
            batteryPercent = Mathf.Clamp(value, 0, 100);
            if (CampaignState.Instance) CampaignState.Instance.Data.battery = batteryPercent;
            SyncVisualState();
        }

        public void SyncVisualState()
        {
            // Restores/atomic photo transactions can also reach exactly zero.
            // A dead screen must remain switchable through hiding/recovery rules.
            if (!HasCameraPower)
            {
                isScreenOn = false;
                currentScanMode = ScanVisualMode.Normal;
            }
            if (handPhoneMeshRoot != null)
            {
                handPhoneMeshRoot.SetActive(hasSmartphoneInInventory && isPhoneRaised);
            }

            if (phoneFlashlight != null)
            {
                LayarTerakhir.Player.PracticalLightPresentation.ConfigureBeam(phoneFlashlight);
                phoneFlashlight.enabled = hasSmartphoneInInventory && isPhoneRaised && isScreenOn && HasCameraPower;
                phoneFlashlight.intensity = 22f;
                phoneFlashlight.range = 16f;
                phoneFlashlight.spotAngle = 76f;
                phoneFlashlight.innerSpotAngle = 34f;
                phoneFlashlight.color = new Color(.88f, .94f, 1f);
                phoneFlashlight.shadowBias = .025f;
                phoneFlashlight.shadowNormalBias = .16f;
                phoneFlashlight.shadows = LightShadows.Soft;
                if (!spillLight)
                {
                    var spill = new GameObject("PhoneNearSpill", typeof(Light));
                    spill.transform.SetParent(phoneFlashlight.transform, false);
                    spillLight = spill.GetComponent<Light>(); spillLight.type = LightType.Point;
                    spillLight.range = 3.8f; spillLight.intensity = .38f;
                    spillLight.color = new Color(.76f, .83f, 1f); spillLight.shadows = LightShadows.None;
                }
                spillLight.enabled = phoneFlashlight.enabled;
            }

            if (HudController.Instance != null)
            {
                HudController.Instance.UpdatePhoneStatusUI(this);
            }
        }

        public void PulseShutter()
        {
            if (!isPhoneRaised || !isScreenOn || batteryPercent <= 0) return;
            shutterUntil = Time.unscaledTime + .16f;
        }

        private void LateUpdate()
        {
            if (!phoneFlashlight) return;
            bool pulse = Time.unscaledTime < shutterUntil && isPhoneRaised && isScreenOn;
            phoneFlashlight.intensity = pulse ? 85 : 22;
            if (pulse) phoneFlashlight.enabled = true;
            if (spillLight) spillLight.intensity = pulse ? 3.5f : .38f;
            var player = FindFirstObjectByType<LayarTerakhir.Player.FirstPersonController>();
            if (!player || !player.cameraPivot) return;
            phoneFlashlight.transform.SetPositionAndRotation(
                player.cameraPivot.position + player.transform.right * .24f + player.transform.forward * .28f,
                player.cameraPivot.rotation);
            if (pulse && isSelfieCamera && selfieModuleUnlocked)
                phoneFlashlight.transform.SetPositionAndRotation(player.cameraPivot.position + player.transform.forward * .52f,
                    Quaternion.LookRotation(-player.transform.forward,Vector3.up));
        }
    }
}
