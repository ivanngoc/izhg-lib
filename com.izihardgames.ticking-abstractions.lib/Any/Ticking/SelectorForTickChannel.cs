using System;

namespace IziHardGames.Libs.NonEngine.Game.Abstractions
{
    public abstract class SelectorForTickChannel
    {
        public TickChannel this[string id] => throw new NotImplementedException();
    }
}
