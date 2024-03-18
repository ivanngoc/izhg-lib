using IziHardGames.Apps.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.InputSystem.ForUnity;
using IziHardGames.UserControl.Lib.Contexts;
using IziHardGames.Attributes;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Libs.NonEngine.Game.Abstractions;

namespace IziHardGames.UserControl.ForUnity
{
	public static class ExtensionsForIziAppBuilderAsUserControlUnity
    {
        [ExtendApp(EUseType.DependentComplex)]
        public static UserEnvironment UseUserControlForUnity(this IIziAppBuilder builder)
        {
            builder.UseUserControlAbstractionsDefaultUserEnvironment();
            builder.UseUserControlAbstractions();
            builder.UseRegistryForSetOfTriggersDefault();
            DataInput dataInput = new DataInput();
            builder.AddSingleton<DataInput>(dataInput);
            var user = new UserMono(dataInput);
            Users.RegistUser(user);
            Users.SetCurrent(user);
            UserEnvironment env = new UserEnvironment(user);
            var service = new UserControlMonoService();
            service.environment = env;
            builder.AddSingleton(env);
            builder.AddService(service);
            return env;
        }
    }
}