using System;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Core;

namespace IziHardGames.Libs.NonEngine.Game.Abstractions
{
	/// <summary>
	/// Или Update System
	/// </summary>
	public interface ITickSystem
    {

    }

    public abstract class TickChannel : ITickChannel
    {
        public abstract void Regist(Action action);
        public abstract int Enable(Action handler);
        public abstract void Disable(int token);
        public abstract void ExecuteSync();
    }

    public abstract class SelectorForTickChannel
    {
        public TickChannel this[string id] => throw new NotImplementedException();
    }

    public interface ITickChannel
    {

    }

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
