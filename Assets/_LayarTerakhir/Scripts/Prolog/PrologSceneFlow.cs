using System;
using System.Collections;
using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Prolog
{
    public static class PrologSceneFlow
    {
        public const string AtmajaScene="Prolog_RumahAtmaja";
        public const string CorridorScene="Prolog_KoridorKos";
        public static bool AutomaticTransitions=true;
        private static string pendingBindings;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset(){AutomaticTransitions=true;pendingBindings=null;}
        public static bool IsAtmajaCheckpoint(string id)=>id!=null&&(id.StartsWith("PROLOG_ATMAJA_",StringComparison.Ordinal)||id.StartsWith("PROLOG_BASEMENT",StringComparison.Ordinal)||id.StartsWith("PROLOG_CHASE",StringComparison.Ordinal)||id=="PROLOG_MATABATIN_ACQUIRED"||id=="PROLOG_COMPLETE");
        public static bool CanContinue=>AutomaticTransitions&&Application.CanStreamedLevelBeLoaded(AtmajaScene);
        public static bool IsCorridorCheckpoint(string id)=>id=="PROLOG_KOS_CORRIDOR"||id=="PROLOG_KOS_YARD"||id=="PROLOG_TRAVEL_ATMAJA";
        public static IEnumerator LoadCorridor()
        {
            if(!AutomaticTransitions||!Application.CanStreamedLevelBeLoaded(CorridorScene))yield break;
            if(GameInputConfig.Instance)pendingBindings=JsonUtility.ToJson(GameInputConfig.Instance);
            var load=SceneManager.LoadSceneAsync(CorridorScene,LoadSceneMode.Single);while(load!=null&&!load.isDone)yield return null;
        }
        public static IEnumerator LoadAtmaja()
        {
            if(!CanContinue)yield break;
            if(GameInputConfig.Instance)pendingBindings=JsonUtility.ToJson(GameInputConfig.Instance);
            var load=SceneManager.LoadSceneAsync(AtmajaScene,LoadSceneMode.Single);
            while(load!=null&&!load.isDone)yield return null;
        }
        public static void RestoreBindings()
        {if(!string.IsNullOrEmpty(pendingBindings)&&GameInputConfig.Instance)JsonUtility.FromJsonOverwrite(pendingBindings,GameInputConfig.Instance);pendingBindings=null;}
    }
}
