#if UNITY_EDITOR || DEBUG
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IziHardGames.Apps.ForUnity
{
    /// <summary>
    /// https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
    /// https://docs.unity3d.com/Manual/ConfigurableEnterPlayModeDetails.html
    /// </summary>
    [UnityEditor.InitializeOnLoad]
    public static class CleanupOnEditorLoad
    {
        static CleanupOnEditorLoad()
        {
            Debug.Log($"{nameof(CleanupOnEditorLoad)} static ctor(). InitializeOnLoad");
            EditorApplication.playModeStateChanged += ChangeHandler;
        }

        private static void ChangeHandler(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                OnEnterPlaymode();
            }
            else if (change == PlayModeStateChange.ExitingPlayMode)
            {
                OnExitPlaymode();
            }
        }

        /// <summary>
        /// Из-за фактической недетерминированности запуска <see cref="EditorApplication.playModeStateChanged"/> код сцены выполняется раньше чем это событие. Поэтому для очитски лучше использовать событие после выхода из PlayMode
        /// </summary>
        internal static void OnExitPlaymode()
        {
            Debug.Log($"{nameof(CleanupOnEditorLoad)}.{nameof(OnExitPlaymode)}() Cleanup");

            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                var roots = scene.GetRootGameObjects();

                foreach (var root in roots)
                {
                    FindCleanup(root);
                }
            }
        }
        internal static void OnEnterPlaymode()
        {

        }

        public static void FindCleanup(GameObject gameObject)
        {
            var comps = gameObject.GetComponents<MonoBehaviour>();

            foreach (var comp in comps)
            {
                if (comp is MonoCleanup monoCleanup)
                {
                    monoCleanup.Cleanup();
                }
                if (comp is ICleanupFastReload cleaner)
                {
                    cleaner.CleanupForFastReload();
                }
            }
            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                FindCleanup(child.gameObject);
            }
        }
    }
}
#endif