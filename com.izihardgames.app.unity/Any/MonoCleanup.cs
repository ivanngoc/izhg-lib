using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace IziHardGames.Apps.ForUnity
{
    public class MonoCleanup : MonoBehaviour
    {
        [SerializeField] private Object[] objects = Array.Empty<Object>();

        public void Cleanup()
        {
#if UNITY_EDITOR
            Debug.Log($"{GetType().FullName}.{nameof(Cleanup)}()");
#endif
            foreach (var item in gameObject.GetComponents<MonoBehaviour>())
            {
                if (item is ICleanupFastReload comp)
                {
                    comp.CleanupForFastReload();
                }
            }

            foreach (var item in objects)
            {
                if (item is ICleanupFastReload comp)
                {
                    comp.CleanupForFastReload();
                }
            }
        }
    }
}