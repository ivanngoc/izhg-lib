using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Libs.NonEngine.Applications;
using IziHardGames.Libs.NonEngine.Game.Abstractions;

namespace IziHardGames.Apps.ForUnity
{
    public static class ExtensionsForIziAppBuilderAsAppLib
    {
        public static void UseLogging(this IIziAppBuilder builder)
        {
            throw new System.NotImplementedException();
        }
#if DEBUG
        public static void UseDebugLogger(this IIziAppBuilder builder)
        {
            var logger = new UnityDebugLogger();
            Logging.Default = logger;
            Logging.Debug = logger;
            builder.AddSingleton(logger);
        }
#endif
        public static void UserMonoInterval(this IIziAppBuilder builder)
        {
            var normal = new MonoIntervalProviderNormal();
            var fix = new MonoIntervalProviderFixed();
            IziInterval.providers[typeof(MonoIntervalProviderNormal)] = normal;
            IziInterval.providers[typeof(MonoIntervalProviderFixed)] = fix;
            IziInterval.SetDefault(normal);
            IziTicks.Fixed!.Enable(normal.Execute);
            IziTicks.Fixed.Enable(fix.Execute);

            builder.AddSingleton(normal);
            builder.AddSingleton(fix);
        }
    }
}