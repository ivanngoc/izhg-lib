using IziHardGames.Apps.NetStd21;
using IziHardGames.Game.Abstractions.Lib;
using IziHardGames.Apps.Abstractions.Lib;

namespace IziHardGames.Apps.Games.Lib
{
    public static class ExtensionsForIIziAppBuilderAsAppGame
    {
        public static void AddGameElement<T>(this IIziAppBuilder builder, object specificHandler) where T : IGameElement
        {
            throw new System.NotImplementedException();
        }
        public static void AddGameElement<T>(this IIziAppBuilder builder) where T : IGameElement
        {

        }
        public static void UseIziGame(this IIziAppBuilder builder)
        {

        }
    }

    internal enum EAppGameGeneralElements
    {
        None,
        InputSystem,
        SaveSystem,
    }
}
