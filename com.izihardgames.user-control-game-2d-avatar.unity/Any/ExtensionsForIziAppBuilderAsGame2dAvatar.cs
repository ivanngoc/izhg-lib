using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Game2d.Avatar.Unity.Moving;

namespace IziHardGames.UserControl.Game2d.Avatar.Unity
{
    public static class ExtensionsForIziAppBuilderAsGame2dAvatar
    {
        public static void UserGameAvatar2dSingleUser(this IIziAppBuilder builder)
        {
            builder.ValidateUserModsAbstract();
            var selector = builder.GetSingleton<UserEnvironmentSelectorAbstract>();
            var env = selector.Current;
            env.Modes.RegistMode<UModeAvatar2d>(new UModeAvatar2d(env));
        }
    }
}
