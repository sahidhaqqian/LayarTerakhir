using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Smartphone
{
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
