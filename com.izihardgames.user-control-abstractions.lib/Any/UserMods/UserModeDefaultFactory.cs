using IziHardGames.Attributes;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;

namespace IziHardGames.UserControl.Abstractions.NetStd21.UserMods
{
    [IzhgGenericFactory]
    public static class UserModeDefaultFactory
    {
        public static T Create<T>(UserEnvironmentAbstract env) where T : UserMode, new()
        {
            T userMode = new T();
            //userMode.user
            throw new System.NotImplementedException();
        }
    }
}
