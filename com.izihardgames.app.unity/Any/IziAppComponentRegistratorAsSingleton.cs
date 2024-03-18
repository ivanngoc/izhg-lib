using System;
using IziHardGames.Apps.NetStd21;
using UnityEngine;

namespace IziHardGames.Apps.ForUnity
{

    public class IziAppComponentRegistratorAsSingleton : MonoBehaviour, IDelayedTask
    {
        public bool IsFinished { get; set; }

        [SerializeField] private Component target;
        private async void Awake()
        {
            Debug.Log($"[{Time.frameCount}] Awaked:{GetType().FullName} Begin");
            var app = await IziApp.AwaitCreated();
            Debug.Log($"[{Time.frameCount}] Awaked:{GetType().FullName} iziApp awaited");
            app.AddSingletonTemp(target.GetType(), target);
            Debug.Log($"[{Time.frameCount}] Awaked:{GetType().FullName} Singleton Added. Type:{target.GetType()}");
            IsFinished = true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (target == null) Debug.LogException(new NullReferenceException("Target for registrate in IziApp as Singleton is empty"), this);
        }

        private void Reset()
        {
            OnValidate();
        }
#endif
    }
}