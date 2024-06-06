using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.UserControl.Abstractions.NetStd21;

namespace IziHardGames.UserControl.InputSystem.ForUnity
{
    public static class ExtensionsForIziAppBuilderAsUserControlInputSystem
    {
        public static void UseNewInputSystem(this IIziAppBuilder builder)
        {          
            IziInputSystemMono iziInputSystemMono = new IziInputSystemMono();
            builder.AddSingleton(iziInputSystemMono);
            builder.AddSingleton<IziInputSystem>(iziInputSystemMono);
            builder.AddSingleton<IInputCollector>(iziInputSystemMono.collector);
            RegistryForInputSystem.inputContainerFactory.Regist<UnityInputKeyContainer>(new FactoryForUnityKeyItem());
            RegistryForInputSystem.inputContainerFactory.Regist<UnityPointerContainer>(new FactoryForUnityPointerContainer());
            RegistryForDevices.Find = new UnityDeviceFinder();
        }
    }
}