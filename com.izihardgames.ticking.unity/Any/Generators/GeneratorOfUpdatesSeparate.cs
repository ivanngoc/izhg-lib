using IziHardGames.Libs.NonEngine.Game.Abstractions;
using UnityEngine;

namespace IziHardGames.Ticking.ForUnity
{
    [DefaultExecutionOrder(-1)]
    public class GeneratorOfUpdatesSeparate : GeneratorOfUpdates
    {
        private void Update()
        {
#if UNITY_EDITOR
           UnityEngine.Profiling.Profiler.BeginSample("Izhg Normal Tick",this);
#endif
            IziTicks.Normal?.ExecuteSync();
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
        private void FixedUpdate()
        {
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Izhg FixedUpdate Tick",this);
#endif
            IziTicks.Fixed?.ExecuteSync();
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
        private void LateUpdate()
        {
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Izhg LateUpdate Tick",this);
#endif
            IziTicks.Late?.ExecuteSync();
            IziTicks.Reset?.ExecuteSync();
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
    }
}