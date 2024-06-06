using IziHardGames.Core;

namespace IziHardGames.Libs.NonEngine.Game.Abstractions
{
    public class TickScheduler
    {
        /// <summary>
        /// Run Later On this Frame. Example: schedule [TExe] From Normal Channel To Execute on Late Channel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="exe"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ThisFrame<T>(TickChannel channel, T exe) where T : IExecutable
        {
            throw new System.NotImplementedException();
        }
    }
}
