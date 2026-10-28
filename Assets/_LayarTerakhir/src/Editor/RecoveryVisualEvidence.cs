#if UNITY_EDITOR
using System.Collections.Generic;
using System.Collections;
using System;
using NUnit.Framework;
using Object=UnityEngine.Object;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEditor;

namespace LayarTerakhir.Editor
{
    [DefaultExecutionOrder(10000)]
    public sealed class RecoveryPresentationProbe:MonoBehaviour
    {
        internal Action action;internal Exception failure;internal bool done;
        private void LateUpdate()
        {
            if(done||action==null)return;
            try{action();}catch(Exception error){failure=error;}finally{done=true;}
        }
    }
    internal static class RecoveryRuntimeScenario
    {
        internal static IEnumerator Run(IEnumerator scenario)
        {
            var go=new GameObject("RecoveryRuntimeHost");Object.DontDestroyOnLoad(go);
            var host=go.AddComponent<LayarTerakhir.Editor.Tests.FoundationAuditCoroutineHost>();
            bool done=false;Exception failure=null;
            host.StartCoroutine(Drive(scenario,e=>{failure=e;done=true;}));
            float deadline=Time.realtimeSinceStartup+240;
            while(!done&&Time.realtimeSinceStartup<deadline)yield return null;
            Object.Destroy(go);Assert.That(done,Is.True,"Recovery runtime scenario exceeded its deadline");
            if(failure!=null)throw failure;
        }
        private static IEnumerator Drive(IEnumerator scenario,Action<Exception> complete)
        {
            var stack=new Stack<IEnumerator>();stack.Push(scenario);
            while(stack.Count>0)
            {
                object next=null;Exception failure=null;bool more=false;
                try{more=stack.Peek().MoveNext();if(more)next=stack.Peek().Current;}catch(Exception e){failure=e;}
                if(failure!=null){complete(failure);yield break;}if(!more){stack.Pop();continue;}
                if(next is IEnumerator nested)stack.Push(nested);else yield return next;
            }
            complete(null);
        }
    }
    internal static class RecoveryTestInputSettings
    {
        // Input System's editor domain snapshot must reference a persistent
        // asset. PlayMode-created settings die during exit before its restore
        // callback. Restore the temporary editor config after PlayMode; this
        // fixture asset is excluded from delivery.
        internal const string AssetPath="Assets/RecoveryTestInputSettings.asset";
        internal static void Prepare(string key)
        {
            const string config="com.unity.input.settings";
            EditorBuildSettings.TryGetConfigObject<InputSettings>(config,out var original);
            SessionState.SetString(key+".Config",original?AssetDatabase.GetAssetPath(original):"");
            SessionState.SetString(key,JsonUtility.ToJson(InputSystem.settings));
            var settings=AssetDatabase.LoadAssetAtPath<InputSettings>(AssetPath);
            if(!settings){settings=ScriptableObject.CreateInstance<InputSettings>();AssetDatabase.CreateAsset(settings,AssetPath);}
            JsonUtility.FromJsonOverwrite(SessionState.GetString(key,"{}"),settings);
            InputSystem.settings=settings;
        }
        internal static void RestoreEditorConfiguration(string key)
        {
            const string config="com.unity.input.settings";
            string path=SessionState.GetString(key+".Config","");
            if(string.IsNullOrEmpty(path))EditorBuildSettings.RemoveConfigObject(config);
            else EditorBuildSettings.AddConfigObject(config,AssetDatabase.LoadAssetAtPath<InputSettings>(path),true);
        }
        internal static void Restore(string key)
        {
            var settings=AssetDatabase.LoadAssetAtPath<InputSettings>(AssetPath);
            JsonUtility.FromJsonOverwrite(SessionState.GetString(key,"{}"),settings);
            var old=InputSystem.settings;InputSystem.settings=settings;
            if(old!=settings)Object.Destroy(old);
        }
    }
    // Capture the live PlayMode world and HUD through the actual gameplay camera.
    // Overlay canvases are associated with it only during synchronous readback.
    // This is engine-rendered evidence, not a desktop/physical Alt+Tab recording.
    internal static class RecoveryVisualEvidence
    {
        internal static IEnumerator AfterPresentation(Action action)
        {
            var go=new GameObject("RecoveryPresentationProbe");var probe=go.AddComponent<RecoveryPresentationProbe>();probe.action=action;
            float deadline=Time.realtimeSinceStartup+30;
            while(!probe.done&&Time.realtimeSinceStartup<deadline)yield return null;
            bool done=probe.done;var failure=probe.failure;Object.Destroy(go);
            Assert.That(done,Is.True,"Presentation probe exceeded its deadline");if(failure!=null)throw failure;
        }
        internal static IEnumerator CaptureFrame(Camera camera,string name,Action inspectionPose=null)
        {
            if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)yield break;
            yield return AfterPresentation(()=>
            {
                Vector3 position=camera.transform.position;Quaternion rotation=camera.transform.rotation;float fov=camera.fieldOfView;
                try{inspectionPose?.Invoke();Capture(camera,name);}
                finally{camera.transform.SetPositionAndRotation(position,rotation);camera.fieldOfView=fov;}
            });
        }
        private static void Capture(Camera camera,string name)
        {
            if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)return;
            const string folder="Audit/Visuals/Recovery";
            Directory.CreateDirectory(folder);
            var canvases=new List<Canvas>();
            foreach(var canvas in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                if(canvas.isRootCanvas&&canvas.renderMode==RenderMode.ScreenSpaceOverlay&&canvas.gameObject.activeInHierarchy)
                {canvases.Add(canvas);canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=camera.nearClipPlane+.02f;}
            Canvas.ForceUpdateCanvases();
            try{KosRenderDiagnosis.Capture(camera,name,false,folder);}
            finally
            {
                foreach(var canvas in canvases){canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.worldCamera=null;}
                Canvas.ForceUpdateCanvases();
            }
        }
    }
}
#endif
