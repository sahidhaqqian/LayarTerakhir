using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Audio;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public enum KosPrologStage
    {
        WakeUpAndKnocks = 0,
        WaitReadSuratIbuKos = 1,
        WaitPickupSmartphone = 2,
        PreparationAndDrawerPuzzle = 3,
        ReadyToDepart = 4,
        Completed = 5
    }

    public class KamarKosPrologDirector : MonoBehaviour
    {
        public const string DrawerPasscode = "1408";

        [Header("Progression State")]
        public KosPrologStage currentStage = KosPrologStage.WakeUpAndKnocks;
        public bool hasJacket = false;
        public bool hasMotorKey = false;
        public bool hasSpareBattery = false;
        public bool isDrawerUnlocked = false;

        [Header("Scene Object References")]
        public KosInteractableItem pcMonitorItem;
        public KosInteractableItem suratKosItem;
        public KosInteractableItem smartphoneBedItem;
        public KosInteractableItem stickyNoteItem;
        public KosInteractableItem wallCalendarItem;
        public KosInteractableItem lockedDrawerItem;
        public KosInteractableItem jacketItem;
        public KosInteractableItem motorKeyItem;
        public KosInteractableItem kosDoorItem;
        public Light phoneBedRedLed;
        public KosSceneAudio environmentAudio;

        private void Awake()
        {
            KosRoomCorrections.Apply(this);
            if(suratKosItem){suratKosItem.isInteractable=false;suratKosItem.gameObject.SetActive(false);}
        }
        private void Start()
        {StartCoroutine(AwaitMenuChoice());}
        private IEnumerator AwaitMenuChoice()
        {
            while(GameShell.AwaitingStart||GameShell.MenuSceneLoading)yield return null;
            InitializeStory();
        }
        private void InitializeStory()
        {
            if (CampaignState.Instance != null)
            {
                if (CampaignState.Instance.LoadCheckpoint())
                {
                    if(PrologSceneFlow.AutomaticTransitions&&PrologSceneFlow.IsCorridorCheckpoint(CampaignState.Instance.Data.checkpoint_id))
                    {HudController.Instance.SetFadeAlpha(1);StartCoroutine(PrologSceneFlow.LoadCorridor());return;}
                    if (PrologSceneFlow.CanContinue && PrologSceneFlow.IsAtmajaCheckpoint(CampaignState.Instance.Data.checkpoint_id))
                    {
                        HudController.Instance.SetFadeAlpha(1f);
                        StartCoroutine(PrologSceneFlow.LoadAtmaja());
                        return;
                    }
                    if (TryRestoreKosCheckpoint()) return;
                }
                CampaignState.Instance.StartNewGame();
            }

            if (suratKosItem != null){suratKosItem.isInteractable = false;suratKosItem.gameObject.SetActive(false);}
            if (smartphoneBedItem != null) smartphoneBedItem.isInteractable = false;
            if (phoneBedRedLed != null) phoneBedRedLed.enabled = false;

            StartCoroutine(BeginOpeningSequenceRoutine());
        }

        private bool TryRestoreKosCheckpoint()
        {
            var campaign = CampaignState.Instance;
            if (campaign.Data.chapter_id != 0 || campaign.Data.checkpoint_id != "PROLOG_KOS_COMPLETE" ||
                !campaign.HasInventoryItem("SMARTPHONE_RAKA") || !campaign.HasInventoryItem("KUNCI_MOTOR") ||
                !campaign.HasInventoryItem("JAKET_RAKA")) return false;

            hasJacket = hasMotorKey = hasSpareBattery = isDrawerUnlocked = true;
            suratKosItem.gameObject.SetActive(false);
            smartphoneBedItem.gameObject.SetActive(false);
            jacketItem.gameObject.SetActive(false);
            if(motorKeyItem)motorKeyItem.gameObject.SetActive(false);
            lockedDrawerItem.isInteractable = false;
            phoneBedRedLed.enabled = false;
            var phone = SmartphoneController.Instance;
            phone.hasSmartphoneInInventory = true;
            phone.batteryPercent = campaign.Data.battery;
            phone.isPhoneRaised = false;
            phone.isScreenOn = phone.batteryPercent > 0f;
            phone.SyncVisualState();
            currentStage = KosPrologStage.ReadyToDepart;
            HudController.Instance.SetFadeAlpha(0f);
            HudController.Instance.SetObjective("Keluar dari Kamar Kos dan berangkat ke Rumah Keluarga Atmaja.");
            return true;
        }

        private void Update()
        {
            if (phoneBedRedLed != null && phoneBedRedLed.enabled)
            {
                phoneBedRedLed.intensity = 1.5f + Mathf.Sin(Time.time * 12f) * 1.5f;
            }
        }

        private IEnumerator BeginOpeningSequenceRoutine()
        {
            if (HudController.Instance == null)
                yield break;

            HudController.Instance.SetFadeAlpha(1f);
            HudController.Instance.SetObjective(string.Empty);
            yield return WakeFromBed();

            float monologue=HudController.Instance.EnqueueSubtitle(
                "Raka",
                "Tiga puluh dua penonton... Itu pun paling setengahnya akun bot. Kalau malam ini nggak ada konten yang meledak, tamat riwayat gue di Jakarta.",
                5.0f
            );
            // Knock interrupts the end of Raka's muttering; he visibly turns toward its source.
            yield return new WaitForSeconds(Mathf.Max(.5f,monologue-1.2f));

            // Ratna leaves a letter, not a spoken warning. Let the physical sequence finish.
            if(environmentAudio)yield return environmentAudio.PlayOpeningKnocks(
                ()=>StartCoroutine(TurnTowardDoor()),()=>StartCoroutine(SlideWarningLetter()));
            else yield return SlideWarningLetter();
            UiFocus.Cinematic=false;var awakePlayer=FindFirstObjectByType<FirstPersonController>();if(awakePlayer)awakePlayer.MovementLocked=false;

            currentStage = KosPrologStage.WaitReadSuratIbuKos;
            if (suratKosItem != null)
                suratKosItem.isInteractable = true;

            HudController.Instance.SetObjective("Periksa bawah pintu kamar.");
        }
        private IEnumerator TurnTowardDoor()
        {
            var player=FindFirstObjectByType<FirstPersonController>();var from=player.transform.rotation;
            var direction=kosDoorItem.transform.position-player.transform.position;direction.y=0;
            var target=Quaternion.LookRotation(direction);
            for(float t=0;t<1;t+=Time.deltaTime/.65f){player.transform.rotation=Quaternion.Slerp(from,target,Mathf.SmoothStep(0,1,t));yield return null;}
            player.transform.rotation=target;
        }
        private IEnumerator SlideWarningLetter()
        {
            if(!suratKosItem)yield break;
            var end=suratKosItem.transform.position;var inward=end-kosDoorItem.transform.position;inward.y=0;
            if(inward.sqrMagnitude<.001f)inward=Vector3.forward;inward.Normalize();
            var start=end-inward*.32f;suratKosItem.gameObject.SetActive(true);
            for(float t=0;t<1;t+=Time.deltaTime/.7f){suratKosItem.transform.position=Vector3.Lerp(start,end,Mathf.SmoothStep(0,1,t));yield return null;}
            suratKosItem.transform.position=end;
        }
        private IEnumerator WakeFromBed()
        {
            var player=FindFirstObjectByType<FirstPersonController>();var cc=player.GetComponent<CharacterController>();var camera=player.GetComponent<InteractionInteractor>().playerCamera.transform;
            UiFocus.Cinematic=true;player.MovementLocked=true;player.enabled=false;cc.enabled=false;
            var cameraLocal=camera.localPosition;var cameraRotation=camera.localRotation;
            var bed=GameObject.Find("vintage_day_bed");var bounds=new Bounds(new Vector3(-1.89f,.5f,-.65f),Vector3.one);bool first=true;
            if(bed)foreach(var r in bed.GetComponentsInChildren<Renderer>()){if(first){bounds=r.bounds;first=false;}else bounds.Encapsulate(r.bounds);}
            var lying=new Vector3(bounds.center.x,bounds.max.y+.16f,bounds.center.z+.35f);
            camera.position=lying;camera.rotation=Quaternion.Euler(-38,75,-60);
            yield return HudController.Instance.FadeBlack(1,.35f,.55f);yield return HudController.Instance.FadeBlack(.35f,.9f,.16f);yield return HudController.Instance.FadeBlack(.9f,0,.65f);
            var standing=new Vector3(-.8f,.05f,-.98f);var monitor=pcMonitorItem.transform.position;var facing=monitor-standing;facing.y=0;var yaw=Quaternion.LookRotation(facing);
            var end=standing+Vector3.up*player.cameraStandingY;var fromRotation=camera.rotation;
            for(float t=0;t<1;t+=Time.deltaTime/3.2f){camera.position=Vector3.Lerp(lying,end,Mathf.SmoothStep(0,1,t))+Vector3.up*Mathf.Sin(t*Mathf.PI)*.08f;camera.rotation=Quaternion.Slerp(fromRotation,yaw,Mathf.SmoothStep(0,1,t));yield return null;}
            player.transform.SetPositionAndRotation(standing,yaw);player.cameraPivot.localPosition=Vector3.up*player.cameraStandingY;player.cameraPivot.localRotation=Quaternion.identity;camera.localPosition=cameraLocal;camera.localRotation=cameraRotation;cc.enabled=true;player.enabled=true;
        }

        public void OnObjectInteracted(KosInteractableItem item)
        {
            if (item == null || !item.CanInteract() || HudController.Instance == null ||
                HudController.Instance.IsModalOpen || UiFocus.Cinematic || currentStage == KosPrologStage.Completed)
                return;

            switch (item.itemId)
            {
                case "PC_MONITOR":
                    HandleInspectPCMonitor();
                    break;
                case "SURAT_KOS":
                    HandleReadSuratIbuKos(item);
                    break;
                case "SMARTPHONE_BED":
                    HandlePickupSmartphone(item);
                    break;
                case "STICKY_NOTE":
                    HandleInspectStickyNote();
                    break;
                case "KALENDER_DINDING":
                    HandleInspectCalendar();
                    break;
                case "LACI_MEJA":
                    HandleInteractDrawer(item);
                    break;
                case "JAKET_PINTU":
                    HandlePickupJacket(item);
                    break;
                case "KUNCI_MOTOR_MEJA":
                    if(currentStage<KosPrologStage.PreparationAndDrawerPuzzle)return;
                    hasMotorKey=true;item.gameObject.SetActive(false);
                    CampaignState.Instance.AddInventoryItem("KUNCI_MOTOR");
                    if(environmentAudio)environmentAudio.PlayItem("unlock",item.transform.position);
                    CheckPreparationComplete();
                    break;
                case "PINTU_KOS":
                    HandleInteractKosDoor();
                    break;
            }
        }

        private void HandleInspectPCMonitor()
        {
            HudController.Instance.OpenDocumentModal(
                "DASHBOARD KANAL STREAMING: MALAM KERAMAT",
                "Status Kanal: OFFLINE\n" +
                "Penonton Rata-Rata Terakhir: 32 Penonton ( Grafik Anjlok )\n\n" +
                "\"Tiga puluh dua penonton... Itu pun paling setengahnya akun bot. Kalau malam ini nggak ada konten yang meledak, tamat riwayat gue di Jakarta.\""
            );
        }

        private void HandleReadSuratIbuKos(KosInteractableItem suratItem)
        {
            if(environmentAudio)environmentAudio.PlayItem("paper",suratItem.transform.position);
            const string suratBody =
                "\"Raka, ini peringatan terakhir dari saya. Tunggakan kos kamu sudah 3 bulan ditambah uang listrik totalnya Rp3.500.000. " +
                "Jangan alasan ibumu masuk rumah sakit terus. Besok pagi jam 08.00 kalau belum ditransfer, gembok kamar saya ganti dan " +
                "barang-barang komputer kamu saya taruh di luar pagar. — Bu Haji Ratna.\"";

            HudController.Instance.OpenDocumentModal(
                "SURAT PERINGATAN TERAKHIR (ITEM DOKUMEN #1)",
                suratBody,
                () =>
                {
                    if (CampaignState.Instance != null)
                    {
                        CampaignState.Instance.AddInventoryItem("DOC_PROLOG_01_SURAT_KOS");
                    }

                    suratItem.gameObject.SetActive(false);

                    if (currentStage == KosPrologStage.WaitReadSuratIbuKos)
                    {
                        StartCoroutine(AfterSuratReadRoutine());
                    }
                }
            );
        }

        private IEnumerator AfterSuratReadRoutine()
        {
            float reaction=HudController.Instance.PlayPriorityDialogue(
                "Raka",
                "Tiga setengah juta besok pagi... Gue cari ke mana jam segini?!",
                3.5f
            );
            yield return new WaitForSeconds(reaction+.2f);

            if (phoneBedRedLed != null)
                phoneBedRedLed.enabled = true;
            if(environmentAudio)environmentAudio.SetPhoneVibration(true);
            if (smartphoneBedItem != null)
                smartphoneBedItem.isInteractable = true;

            currentStage = KosPrologStage.WaitPickupSmartphone;
            HudController.Instance.SetObjective("Ambil Smartphone di atas kasur.");
        }

        private void HandlePickupSmartphone(KosInteractableItem phoneItem)
        {
            if(environmentAudio)environmentAudio.SetPhoneVibration(false);
            if (phoneBedRedLed != null)
                phoneBedRedLed.enabled = false;

            phoneItem.gameObject.SetActive(false);

            if (SmartphoneController.Instance != null)
            {
                SmartphoneController.Instance.AcquireSmartphone();
            }

            if (CampaignState.Instance != null)
            {
                CampaignState.Instance.AddInventoryItem("SMARTPHONE_RAKA");
            }

            StartCoroutine(AfterVoiceNotePlayedRoutine());
        }

        private IEnumerator AfterVoiceNotePlayedRoutine()
        {
            var view=FindFirstObjectByType<AtmajaPhoneView>();
            while(!view||!view.Apps){yield return null;view=FindFirstObjectByType<AtmajaPhoneView>();}
            view.Apps.BeginDamarCall();
            HudController.Instance.SetObjective("Dengarkan Damar.");
            float voice=HudController.Instance.PlayPriorityDialogue("Damar",KosDialogue.DamarVoiceNote,22);
            yield return new WaitForSeconds(voice+.2f);
            if(environmentAudio)yield return environmentAudio.PlayCallPanic();
            view.Apps.EndDamarCall();
            float response=HudController.Instance.PlayPriorityDialogue(
                "Raka",
                "Dam?! Halo, Damar?! Sial, nomornya nggak aktif!",
                3.2f
            );
            yield return new WaitForSeconds(response+.2f);

            yield return new WaitForSeconds(3.5f);

            if(environmentAudio)environmentAudio.NotifyDonation();
            if (CampaignState.Instance != null)
            {
                CampaignState.Instance.AddBoundDonation(5000000);
            }

            currentStage=KosPrologStage.PreparationAndDrawerPuzzle;
            view.Apps.ReceiveContract();
            HudController.Instance.PlayPriorityDialogue("Raka",KosDialogue.ContractReaction,5);
            preparationStarted=true;CheckPreparationComplete();
        }

        private void HandleInspectStickyNote()
        {
            HudController.Instance.OpenDocumentModal(
                "STICKY NOTE DI MEJA PC",
                "Empat putaran. Yang merawatku lebih dulu.\nHari mendahului bulan."
            );
        }

        private void HandleInspectCalendar()
        {
            HudController.Instance.OpenDocumentModal(
                "KALENDER DINDING KAMAR KOS",
                "AGUSTUS\n\nTanggal 17 ditandai: Merdeka.\nDi bawahnya ada catatan kecil:\n\"Ibu bertambah usia tiga hari sebelumnya. Jangan lupa telepon.\""
            );
        }

        private void HandleInteractDrawer(KosInteractableItem drawerItem)
        {
            if (currentStage < KosPrologStage.PreparationAndDrawerPuzzle)
            {
                HudController.Instance.EnqueueSubtitle(
                    "Raka",
                    "Gue cek dulu apa yang diselipin di bawah pintu tadi.",
                    2.5f
                );
                return;
            }

            if (isDrawerUnlocked)
                return;

            HudController.Instance.OpenPadlockModal(DrawerPasscode, () =>
            {
                isDrawerUnlocked = true;
                if(environmentAudio)environmentAudio.PlayItem("unlock",drawerItem.transform.position);
                hasSpareBattery = true;
                drawerItem.isInteractable = false;

                if (CampaignState.Instance != null)
                {
                    CampaignState.Instance.AddInventoryItem("POWERBANK_KECIL_KOS");
                    CampaignState.Instance.AddInventoryItem("DOMPET_RAKA");
                    CampaignState.Instance.AddInventoryItem("DIARY_RAKA");
                }

                HudController.Instance.EnqueueSubtitle(
                    "Raka",
                    "Kebuka. Powerbank, dompet, sama buku harian gue masih di sini.",
                    3.5f
                );
                CheckPreparationComplete();
            });
        }

        private void HandlePickupJacket(KosInteractableItem jacket)
        {
            if (currentStage < KosPrologStage.PreparationAndDrawerPuzzle)
            {
                HudController.Instance.EnqueueSubtitle(
                    "Raka",
                    "Bentar, gue belum mau keluar sebelum cek bawah pintu.",
                    2.5f
                );
                return;
            }

            hasJacket = true;
            if(environmentAudio)environmentAudio.PlayItem("cloth",jacket.transform.position);
            jacket.gameObject.SetActive(false);

            if (CampaignState.Instance != null)
            {
                CampaignState.Instance.AddInventoryItem("JAKET_RAKA");
            }

            HudController.Instance.EnqueueSubtitle(
                "Raka",
                "Jaket udah dipakai. Hujan di luar makin deres.",
                2.8f
            );
            CheckPreparationComplete();
        }

        private bool preparationStarted;
        private void CheckPreparationComplete()
        {
            if(!preparationStarted)return;
            if (hasJacket && hasMotorKey && hasSpareBattery)
            {
                currentStage = KosPrologStage.ReadyToDepart;
                HudController.Instance.SetObjective("Keluar dari Kamar Kos dan berangkat ke Rumah Keluarga Atmaja.");
            }
            else HudController.Instance.SetObjective("Persiapan berangkat\n"+
                (hasMotorKey?"✓":"○")+" Kunci motor di meja   "+(hasJacket?"✓":"○")+" Jaket di gantungan\n"+
                (hasSpareBattery?"✓":"○")+" Powerbank di laci");
        }

        private void HandleInteractKosDoor()
        {
            if (currentStage < KosPrologStage.ReadyToDepart)
            {
                HudController.Instance.EnqueueSubtitle(
                    "Raka",
                    "Gue belum siap berangkat.",
                    3.2f
                );
                CheckPreparationComplete();
                return;
            }

            if (currentStage == KosPrologStage.Completed)
                return;

            if (CampaignState.Instance == null || !CampaignState.Instance.SaveCheckpointAtomic("PROLOG_KOS_COMPLETE"))
            {
                HudController.Instance.EnqueueSubtitle("Sistem", "Checkpoint gagal disimpan. Coba buka pintu lagi.", 3f);
                return;
            }

            currentStage = KosPrologStage.Completed;
            var player = FindFirstObjectByType<FirstPersonController>();
            if (player != null) player.MovementLocked = true;
            StartCoroutine(DepartKosSequenceRoutine());
        }

        private IEnumerator DepartKosSequenceRoutine()
        {
            if(PrologSceneFlow.AutomaticTransitions&&UnityEngine.SceneManagement.SceneManager.GetSceneByName(PrologSceneFlow.CorridorScene).IsValid()==false&&Application.CanStreamedLevelBeLoaded(PrologSceneFlow.CorridorScene))
            {
                HudController.Instance.SetObjective("Keluar ke lorong kos lantai dua.");
                if(environmentAudio)environmentAudio.PlayItem("unlock",kosDoorItem.transform.position);
                yield return HudController.Instance.FadeBlack(0,1,.4f);yield return PrologSceneFlow.LoadCorridor();yield break;
            }
            if(environmentAudio)environmentAudio.Depart();
            HudController.Instance.SetObjective("Berangkat menuju Rumah Keluarga Atmaja...");
            HudController.Instance.EnqueueSubtitle(
                "SFX",
                "[Pintu kamar kos terbuka — suara deru motor menembus hujan deras menuju Rumah Keluarga Atmaja]",
                4.0f
            );

            yield return HudController.Instance.FadeBlack(0f, 1f, 3.5f);
            yield return PrologSceneFlow.LoadAtmaja();
        }
    }
}
