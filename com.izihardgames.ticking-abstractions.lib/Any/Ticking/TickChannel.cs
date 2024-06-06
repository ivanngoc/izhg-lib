using System;

namespace IziHardGames.Libs.NonEngine.Game.Abstractions
{
    public abstract class TickChannel : ITickFlow
    {
        public abstract int Enable(string key, Action handler);
        public abstract void Disable(int token);
        public abstract void ExecuteSync();
    }
}
