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
        public const string DrawerPasscode = "148";

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
        public KosInteractableItem kosDoorItem;
        public Light phoneBedRedLed;
        public KosSceneAudio environmentAudio;

        private void Start()
        {
            if (CampaignState.Instance != null)
            {
                if (CampaignState.Instance.LoadCheckpoint())
                {
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

            if (suratKosItem != null) suratKosItem.isInteractable = false;
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
            HudController.Instance.EnqueueSubtitle(
                "SFX",
                "[Suara hujan deras menghantam atap seng & dengung kipas PC tua]",
                2.5f
            );

            yield return HudController.Instance.FadeBlack(1f, 0f, 3.0f);

            HudController.Instance.EnqueueSubtitle(
                "Raka",
                "Tiga puluh dua penonton... Itu pun paling setengahnya akun bot. Kalau malam ini nggak ada konten yang meledak, tamat riwayat gue di Jakarta.",
                5.0f
            );
            yield return new WaitForSeconds(5.2f);

            if(environmentAudio)environmentAudio.PlayKnocks();
            HudController.Instance.EnqueueSubtitle(
                "SFX",
                "[TOK! TOK! TOK! Ketukan keras 3 kali di pintu kayu kamar kos, disusul langkah kaki berat menjauh]",
                3.5f
            );
            yield return new WaitForSeconds(3.6f);

            currentStage = KosPrologStage.WaitReadSuratIbuKos;
            if (suratKosItem != null)
                suratKosItem.isInteractable = true;

            HudController.Instance.SetObjective("Periksa bawah pintu kamar.");
        }

        public void OnObjectInteracted(KosInteractableItem item)
        {
            if (item == null || !item.CanInteract() || HudController.Instance == null ||
                HudController.Instance.IsModalOpen || currentStage == KosPrologStage.Completed)
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
            HudController.Instance.EnqueueSubtitle(
                "Raka",
                "Tiga setengah juta besok pagi... Gue cari ke mana jam segini?!",
                3.5f
            );
            yield return new WaitForSeconds(3.6f);

            if (phoneBedRedLed != null)
                phoneBedRedLed.enabled = true;
            if(environmentAudio)environmentAudio.SetPhoneVibration(true);
            if (smartphoneBedItem != null)
                smartphoneBedItem.isInteractable = true;

            currentStage = KosPrologStage.WaitPickupSmartphone;
            HudController.Instance.EnqueueSubtitle(
                "SFX",
                "[HP Raka di atas kasur bergetar keras dengan lampu LED berkedip merah]",
                2.8f
            );
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

            const string phoneScreenContent =
                "14 Panggilan Tak Terjawab dari \"Damar (Kameramen)\" — Pukul 21:15 WIB\n" +
                "1 Pesan Suara (Voice Note) — Durasi 00:22\n\n" +
                "[MEMUTAR VOICE NOTE DAMAR]:\n" +
                "\"Ka... Raka, jangan ke Rumah Atmaja sendirian. Tolong dengerin gue. Aplikasi kamera yang gue oprek dari forum kemarin... " +
                "itu bukan filter AR biasa, Ka. Rumah ini nggak kosong. Di lantai dua ada suara perempuan nyanyi tembang Jawa, tapi pas gue " +
                "sorot pakai kamera HP, dia udah berdiri tepat di depan muka gue—\"\n\n" +
                "(Terdengar suara benda logam diseret, jeritan tertahan Damar, lalu bunyi kaca pecah sebelum rekaman mati total).";

            HudController.Instance.OpenDocumentModal(
                "LAYAR SMARTPHONE RAKA — VOICE NOTE DAMAR",
                phoneScreenContent,
                () => StartCoroutine(AfterVoiceNotePlayedRoutine())
            );
        }

        private IEnumerator AfterVoiceNotePlayedRoutine()
        {
            HudController.Instance.EnqueueSubtitle(
                "Raka",
                "Dam?! Halo, Damar?! Sial, nomornya nggak aktif!",
                3.2f
            );
            yield return new WaitForSeconds(3.3f);

            if(environmentAudio)environmentAudio.NotifyDonation();
            HudController.Instance.ShowBroadcastNotification(
                "[GLITCH LAYAR 1 DETIK — KRING!]\n" +
                "[NOTIFIKASI DONASI MASUK: Rp5.000.000 dari Penonton_Terakhir]\n" +
                "\"Uang muka untuk kosmu dan biaya RS ibumu. Kalau mau temanmu tetap bernapas dan sisa Rp495.000.000 cair, " +
                "nyalakan live di Rumah Keluarga Atmaja sebelum jam 12 malam. Waktu jalan terus.\"",
                12f
            );

            if (CampaignState.Instance != null)
            {
                CampaignState.Instance.AddBoundDonation(5000000);
            }

            currentStage = KosPrologStage.PreparationAndDrawerPuzzle;
            HudController.Instance.SetObjective("Ambil Kunci Motor, Jaket, dan Baterai Cadangan sebelum berangkat.");
        }

        private void HandleInspectStickyNote()
        {
            HudController.Instance.OpenDocumentModal(
                "STICKY NOTE DI MEJA PC",
                "\"Password gembok: Tanggal & bulan ulang tahun Ibu biar nggak lupa.\""
            );
        }

        private void HandleInspectCalendar()
        {
            HudController.Instance.OpenDocumentModal(
                "KALENDER DINDING KAMAR KOS",
                "Bulan: 08 (Agustus)\n" +
                "Tanggal 14 dilingkari spidol merah tebal dengan tulisan tangan:\n" +
                "\"Ultah Ibu (14 - 08)\""
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
                hasMotorKey = true;
                hasSpareBattery = true;
                drawerItem.isInteractable = false;

                if (CampaignState.Instance != null)
                {
                    CampaignState.Instance.AddInventoryItem("KUNCI_MOTOR");
                    CampaignState.Instance.AddInventoryItem("POWERBANK_KECIL_KOS");
                }

                HudController.Instance.EnqueueSubtitle(
                    "Raka",
                    "Kebuka. Kunci motor sama powerbank cadangan udah di tangan.",
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

        private void CheckPreparationComplete()
        {
            if (hasJacket && hasMotorKey && hasSpareBattery)
            {
                currentStage = KosPrologStage.ReadyToDepart;
                HudController.Instance.SetObjective("Keluar dari Kamar Kos dan berangkat ke Rumah Keluarga Atmaja.");
            }
        }

        private void HandleInteractKosDoor()
        {
            if (currentStage < KosPrologStage.ReadyToDepart)
            {
                HudController.Instance.EnqueueSubtitle(
                    "Raka",
                    "Gue harus ambil Smartphone, Jaket, Kunci Motor, sama Baterai Cadangan dulu sebelum berangkat.",
                    3.2f
                );
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
