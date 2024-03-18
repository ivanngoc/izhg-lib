using IziHardGames.Libs.NonEngine.Applications;

namespace IziHardGames.Apps.ForUnity
{
#if DEBUG
    internal class UnityDebugLogger : IziLogger
    {
        public override void Log(string msg, object source)
        {
            UnityEngine.Debug.Log(msg, source as UnityEngine.Object);
        }
    }
#endif
}