#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    [InitializeOnLoad]
    public static class KosVisualCapture
    {
        [Serializable] public class VisualTelemetryReport
        {
            public string gpuName, graphicsApi, cameraName, pipeline, captureFormat;
            public bool actualEditorPlayMode, postProcessingActive, allCapturesReadable;
            public KosRenderDiagnosis.Metric[] shots;
        }
        private static int shotIndex, readyFrame;
        private static double readyTime;
        private static Camera camera;
        private static readonly List<KosRenderDiagnosis.Metric> metrics = new List<KosRenderDiagnosis.Metric>();
        private static readonly string[] Names = { "01-startup", "02-wide-room", "03-desk", "04-window", "05-bed", "06-door", "07-storage" };
        static KosVisualCapture()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if(state!=PlayModeStateChange.EnteredPlayMode||!SessionState.GetBool("KosV2.Capture",false))return;
                SessionState.SetBool("KosV2.Capture",false);
                camera=Object.FindFirstObjectByType<InteractionInteractor>().playerCamera;
                shotIndex=-1;metrics.Clear();readyFrame=Time.frameCount+8;readyTime=EditorApplication.timeSinceStartup+5;
                EditorApplication.update+=Tick;
            };
        }
        public static void Gameplay()
        {
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",Path.Combine(Path.GetTempPath(),"KosV2-Capture-"+Guid.NewGuid()));
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            KosSceneValidation.Validate();
            SessionState.SetBool("KosV2.Capture",true);
            EditorApplication.EnterPlaymode();
        }
        public static void Before()=>Gameplay();
        public static void After()=>Gameplay();
        private static void Tick()
        {
            if(Time.frameCount<readyFrame||EditorApplication.timeSinceStartup<readyTime)return;
            try
            {
                if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)throw new Exception("No graphics device: run capture under Xvfb without -nographics.");
                if(!camera||!camera.CompareTag("MainCamera"))throw new Exception("Capture must use the gameplay interaction camera.");
                if(shotIndex<0)
                {
                    KosRenderDiagnosis.Capture(camera,"07-startup-first-request",false,"Audit/Visuals/V2-Diagnosis");
                    shotIndex=0;readyFrame=Time.frameCount+3;readyTime=EditorApplication.timeSinceStartup+.3;return;
                }
                metrics.Add(KosRenderDiagnosis.Capture(camera,Names[shotIndex],false,"Audit/Visuals/V2"));
                shotIndex++;
                if(shotIndex==Names.Length)
                {
                    bool readable=metrics.TrueForAll(m=>m.mean>.045f&&m.darkRatio<.97f&&m.contrast>.025f);
                    var report=new VisualTelemetryReport {gpuName=SystemInfo.graphicsDeviceName,graphicsApi=SystemInfo.graphicsDeviceType.ToString(),cameraName=camera.name,
                        pipeline=GraphicsSettings.currentRenderPipeline.name,captureFormat="ARGB32 sRGB -> RGB24 PNG; one StandardRequest; no image adjustments",
                        actualEditorPlayMode=Application.isPlaying,postProcessingActive=camera.GetUniversalAdditionalCameraData().renderPostProcessing,
                        allCapturesReadable=readable,shots=metrics.ToArray()};
                    File.WriteAllText("Audit/Visuals/V2/visual-metrics.json",JsonUtility.ToJson(report,true)+"\n");
                    EditorApplication.update-=Tick;
                    if(!readable)throw new Exception("One or more rendered captures are too dark/invalid; visual verification FAILED.");
                    Debug.Log("KOS_V2_CAPTURE All seven gameplay-camera views passed readability checks; GPU="+SystemInfo.graphicsDeviceName);
                    EditorApplication.Exit(0);return;
                }
                SetView(shotIndex);
                readyFrame=Time.frameCount+3;readyTime=EditorApplication.timeSinceStartup+.3;
            }
            catch(Exception e){EditorApplication.update-=Tick;Debug.LogException(e);EditorApplication.Exit(1);}
        }
        private static void SetView(int index)
        {
            var positions=new[]{Vector3.zero,new Vector3(2.35f,1.65f,-1.52f),new Vector3(-.35f,1.45f,.65f),new Vector3(.85f,1.5f,-.20f),new Vector3(-.85f,1.35f,-.35f),new Vector3(-.10f,1.6f,-.45f),new Vector3(.6f,1.65f,.15f)};
            var targets=new[]{Vector3.zero,new Vector3(-1.15f,1.05f,1.0f),new Vector3(-1.25f,.9f,1.95f),new Vector3(2.95f,1.55f,-.55f),new Vector3(-2.25f,.5f,-.9f),new Vector3(1.55f,1.1f,-2.15f),new Vector3(2.6f,1.0f,1.4f)};
            camera.transform.position=positions[index];camera.transform.LookAt(targets[index]);
        }
    }
}
#endif
