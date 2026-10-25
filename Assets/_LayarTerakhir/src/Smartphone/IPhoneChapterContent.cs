using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Smartphone
{
    // Optional chapter simulation clock; existing chapters retain their policy.
    public interface IPhoneChapterClock { bool PhoneClockRunning { get; } }
    // Finale-only power supplied by the ritual; never assigned to stored battery.
    public interface IPhoneRitualPower { bool RitualCameraPower { get; } }
    // A chapter supplies content and encounter restrictions to the single existing handset.
    public interface IPhoneChapterContent
    {
        int Viewers { get; }
        bool CanBeginLive { get; }
        bool CameraLocked { get; }
        string CameraStatus { get; }
        void StartLive();
        void Capture();
        bool PopulateApp(PhoneApp app, Transform page);
    }
}
