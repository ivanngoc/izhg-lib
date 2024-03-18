using System;
using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21.Contexts;

namespace IziHardGames.UserControl.Abstractions.NetStd21.UserMods
{
    public abstract class ContextForUserModesAbstract : UserContext
    {
        private readonly Dictionary<Type, UserMode> userModes = new Dictionary<Type, UserMode>();
        public UserMode this[Type type] => userModes[type];

        public void RegistMode<T>() where T : UserMode, new()
        {
            userModes.Add(typeof(T), new T());
        }
        public void RegistMode<T>(T mode) where T : UserMode
        {
            userModes.Add(typeof(T), mode);
        }
    }
}
