using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.Apps.ForUnity;
using IziHardGames.Apps.Abstractions.NetStd21;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IziHardGames.Apps.Scenes.Unity
{
    public class ReportSceneLoad : MonoBehaviour
    {
        private bool isFinished;
        [SerializeField] public int initialCounter;
        [SerializeField] private List<Component> delayedTasks = new List<Component>();
        [SerializeField] public bool isCoreLoadRequired;
        [SerializeField] public bool isUiLoadRequired;
        [Space]
        [SerializeField] private int frameCount;
        [SerializeField] private int frameInit;
        [Space]
        public List<MonoBehaviour> awaitable = new List<MonoBehaviour>();
        public bool isReported;
        public bool isCoreLoaded;
        public bool isUILoaded;

        private async void Awake()
        {
            isReported = false;
            frameCount = default;

            if (isCoreLoadRequired)
            {
                await ControlForScenes.AwaitCore();
                isCoreLoaded = true;
            }
            if (isUiLoadRequired)
            {
                await ControlForScenes.AwaitUI();
                isUILoaded = true;
            }
            IterateComponents.Foreach<ICollectableMono>(gameObject.scene, Report);
            IterateComponents.Foreach<IAwaitableForSceneReport>(gameObject.scene, AddToAwait);
            enabled = true;
        }

        private void AddToAwait(IAwaitableForSceneReport obj)
        {
            if (obj is MonoBehaviour mono)
            {
                awaitable.Add(mono);
            }
#if UNITY_EDITOR
            else
            {
                throw new InvalidCastException("object must be typeof MonoBehaviour");
            }
#endif
        }

        private void Update()
        {
            frameCount++;
            frameInit = Time.frameCount;
            if (delayedTasks.Any(x => !((x as IDelayedTask)?.IsFinished ?? false))) return;
            if (awaitable.Any(x => !((x as IAwaitableForSceneReport)?.IsFinished ?? false))) return;
            isFinished = initialCounter == 0;
            if (!isFinished) return;
#if UNITY_EDITOR
            Debug.Log($"[{Time.frameCount}] Ready", this);
#endif
            ControlForScenes.ReportSceneReady(gameObject.scene);
            isReported = true;
            this.enabled = false;
        }
#if UNITY_EDITOR
        private void Reset()
        {
            isReported = false;
            isCoreLoadRequired = (gameObject.scene.buildIndex == 0) ? false : true;
            isUiLoadRequired = (gameObject.scene.buildIndex == 1) ? false : true;

            enabled = false;
            delayedTasks.Clear();
            var gos = this.TryGetAllComponentsOnScene<Component>(delayedTasks);
            delayedTasks = delayedTasks.Where(x => x is IDelayedTask).ToList();
            //initialCounter = delayedTasks.Count;
        }
#endif

        private void Report(ICollectableMono mono)
        {
            var hanlder0 = IziHandler.selector.ValueOrNull(typeof(ICollectableMono));
            hanlder0?.Invoke(mono);
            var handler1 = IziHandler.selector.ValueOrNull(mono.GetType());
            handler1?.Invoke(mono);
#if UNITY_EDITOR
            if (hanlder0 == null) Debug.Log($"No Handler is founded for type: {typeof(ICollectableMono).FullName}");
            if (handler1 == null) Debug.Log($"No Handler is founded for type: {mono.GetType().FullName}");
#endif
        }
    }
}