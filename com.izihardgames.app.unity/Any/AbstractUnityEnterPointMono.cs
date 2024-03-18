using System;
using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;

namespace IziHardGames.Apps.ForUnity
{
    /// <summary>
    /// <see cref="IziHardGames.Apps.ForUnity.Presets.ScriptableEnterPoint"/>
    /// </summary>
    public abstract class AbstractUnityEnterPointMono : MonoBehaviour
    {
        [SerializeField] protected UnityEngine.Object[] objects = Array.Empty<UnityEngine.Object>();
        public virtual void Run()
        {
            foreach (var item in objects)
            {
                if (item is IAppEnterPoint enterPoint)
                {
                    enterPoint.Run();
                }
            }
        }
        public virtual Task RunAsync() => Task.CompletedTask;
    }
}

