using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using IziHardGames.Attributes;
using IziHardGames.Libs.Async;
using IziHardGames.Libs.Engine.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IziHardGames.Apps.Scenes.Unity
{
    /// <summary>
    /// Когда сцена загружена контроллер ищет все объекты с этим интерфейсом и вызывает метод.
    /// Необзодим для регистрации <see cref="MonoBehaviour"/> объектов на сцене при запуске
    /// </summary>
    public interface ICollectableMono
    {
        public void ReportCollected(Action<MonoBehaviour> processor)
        {
            processor.Invoke(this as MonoBehaviour ?? throw new InvalidCastException("This interface must be implemented on EnityEngine.MonoBehaviour objects!"));
        }
    }
    public static class ControlForScenes
    {
        private readonly static ConcurrentDictionary<int, ConcurrentBag<int>> awaitTokensPerSceneIndex;
        private readonly static bool[] sceneReports;

        static ControlForScenes()
        {
            int count = SceneManager.sceneCountInBuildSettings;
            sceneReports = new bool[count];
            awaitTokensPerSceneIndex = new ConcurrentDictionary<int, ConcurrentBag<int>>();

            for (int i = 0; i < count; i++)
            {
                awaitTokensPerSceneIndex.TryAdd(i, new ConcurrentBag<int>());
            }
        }

        internal static void ReportSceneReady(Scene scene)
        {
            int index = scene.buildIndex;
            sceneReports[index] = true;

            var tokens = awaitTokensPerSceneIndex[index];
            int awaitingcounts = default;
            foreach (var item in tokens)
            {
                if (item != default)
                {
                    AsyncCenter.ManualComplete(item);
                    awaitingcounts++;
                }
            }
            tokens.Clear();
#if UNITY_EDITOR || DEBUG
            Debug.Log($"[{Time.frameCount}] Scene reported:{scene.name}. Consumers:{awaitingcounts}");
#endif
        }
        internal static void ReportSceneReadyReverse(Scene scene)
        {
            sceneReports[scene.buildIndex] = false;
        }

        public static AsyncCenterTask AwaitCore()
        {
            return AwaitSceneReport(0);
        }
        public static AsyncCenterTask AwaitUI()
        {
            return AwaitSceneReport(1);
        }
        public static AsyncCenterTask AwaitSceneReport(int buildIndex)
        {
#if UNITY_EDITOR||DEBUG
            Debug.Log($"AwaitSceneReport() buildIndex:{buildIndex}. IsReported:{sceneReports[buildIndex]}");
#endif
            if (sceneReports[buildIndex])
            {
                return AsyncCenter.CompletedTask;
            }
            var task = AsyncCenter.CreateAsyncPoint();
            awaitTokensPerSceneIndex[buildIndex].Add(task.id);
            return task;
        }
        public static AsyncCenterTask AwaitSceneReport(Scene scene)
        {
            if (!scene.IsValid()) throw new ArgumentException($"Scene is not Valid");
            int buildIndex = scene.buildIndex;
            if (sceneReports[buildIndex])
            {
                return AsyncCenter.CompletedTask;
            }
            var task = AsyncCenter.CreateAsyncPoint();
            awaitTokensPerSceneIndex[buildIndex].Add(task.id);
            return task;
        }

        public static async ValueTask EnsureSceneLoadedAdditiveOnlyOneAsync(int buildIndex)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var sceneCurrent = SceneManager.GetSceneAt(i);
                if (sceneCurrent.buildIndex == buildIndex) return;
            }
            var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (scene.IsValid())
            {
                if (scene.isLoaded) return;
            }
            var operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            await UnityAwaiting.AwaitIndependently(operation);
        }
        public static async ValueTask EnsureSceneLoadedAdditiveOnlyOneAsync(Scene scene)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var sceneCurrent = SceneManager.GetSceneAt(i);
                if (sceneCurrent == scene) return;
            }
            if (!scene.isLoaded)
            {
                var operation = SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
                await UnityAwaiting.AwaitIndependently(operation);
            }
        }
        [UnityHotReloadEditor]
        public static void CleanupStatic()
        {
            foreach (var item in awaitTokensPerSceneIndex)
            {
                item.Value.Clear();
            }
            for (int i = 0; i < sceneReports.Length; i++)
            {
                sceneReports[i] = default;
            }

            AsyncCenter.CleanupStatics();
        }
    }
}
