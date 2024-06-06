using UnityEngine;

namespace IziHardGames.Apps.Scenes.Unity
{
    public class ReportSceneLoadByInvoke : MonoBehaviour
    {
        [SerializeField] public bool isReportSended;

        private void Reset()
        {
            isReportSended = false;
        }
        public void Invoke()
        {
            ControlForScenes.ReportSceneReady(gameObject.scene);
            isReportSended = true;
        }
    }
}