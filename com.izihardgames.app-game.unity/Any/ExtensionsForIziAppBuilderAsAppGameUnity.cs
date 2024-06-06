using IziHardGames.Apps.Abstractions.NetStd21;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;
using IziHardGames.Apps.Games.Lib;
using IziHardGames.Apps.NetStd21;
using IziHardGames.Game.Abstractions.Logics;
using IziHardGames.Libs.Engine.Updating;
using IziHardGames.Libs.NonEngine.Applications;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Ticking.ForUnity;

namespace IziHardGames.Apps.Games.ForUnity
{
    public static class ExtensionsForIziAppBuilderAsAppGameUnity
    {
        public static void UsePresetsForAppGameUnity(this IIziAppBuilder builder, ProjectPresets presets)
        {
            var service = new MonoAppPresetService(presets);
            builder.AddService(service);
            builder.UsePresetsAppAbstractionUnity(presets);
        }

        public static void UseIziGameMono(this IIziAppBuilder builder, GeneratorOfUpdates generator, ControllerForGame controller)
        {
            builder.UseIziGame();
            builder.UseMonoUpdates(generator);

            builder.AddSingleton(controller);
            builder.AddSingleton<IGameConrtoller>(controller);
        }
    }
}