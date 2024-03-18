using UnityEngine;

namespace IziHardGames.Apps.Scenes.Unity
{
    public class ReportSceneUnload : MonoBehaviour
    {
        private void OnDestroy()
        {
            ControlForScenes.ReportSceneReadyReverse(gameObject.scene);
        }
    }
}
