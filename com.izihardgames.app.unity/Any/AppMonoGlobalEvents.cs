using System;
using UnityEngine;

namespace IziHardGames.Apps.ForUnity
{
	public static class AppMonoGlobalEvents
    {
        public static event Action<Camera>? OnMainCameraChange;
        public static event Action? OnNoCamera;
        
        public static void NotifyCameraChange(Camera camera)
        {
            OnMainCameraChange?.Invoke(camera);
        }
        public static void NotifyNoCamera()
        {
            OnNoCamera?.Invoke();
        }
    }
}

