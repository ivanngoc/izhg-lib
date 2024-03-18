using System;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Attributes;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
	public static class ExtensionsForIziAppBuilderAsUserControlAbstract
    {
        public static void ValidateUserControlAbstract(this IIziAppBuilder iziAppBuilder)
        {
            if (iziAppBuilder.GetSingleton<UserEnvironmentSelectorAbstract>() == null)
            {
#if DEBUG
                throw new NullReferenceException($"field at [{nameof(IziEnvironment)}.{nameof(IziEnvironment.Environments)}] were not asigned. If you don't have specific method you can call:{typeof(ExtensionsForIziAppBuilderAsUserControlAbstract).GetMethod(nameof(UseUserControlAbstractionsDefaultUserEnvironment)).Name}");
#else
                throw new NullReferenceException();
#endif
            }
        }
        [ExtendApp(EUseType.Independent)]
        public static void UseUserControlAbstractions(this IIziAppBuilder iziAppBuilder)
        {
            var containers = new RegistryForPairContainerAndDevice();
            RegistryForInputSystem.Pairs = containers;
            iziAppBuilder.AddSingleton(containers);
        }

        [ExtendApp(EUseType.Independent)]
        public static void UseUserControlAbstractionsDefaultUserEnvironment(this IIziAppBuilder iziAppBuilder)
        {
            var selector = new MultiUserEnvironmentSelector();
            IziEnvironment.Environments = selector;
            iziAppBuilder.AddSingleton<UserEnvironmentSelectorAbstract>(selector);
        }

        public static void ValidateUserModsAbstract(this IIziAppBuilder iziAppBuilder)
        {
            foreach (var item in IziEnvironment.Environments.All)
            {
                if (item.Modes == null) throw new InvalidOperationException($"UserMode [userid:{item.User.id}] in {item.GetType().FullName} is empty.");
            }
        }
    }
}
