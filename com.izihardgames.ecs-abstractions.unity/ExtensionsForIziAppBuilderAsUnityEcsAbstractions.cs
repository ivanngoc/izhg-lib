using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Attributes;

namespace IziHardGames.Apps.ForEcs.Abstractions.ForUnity
{
    public static class ExtensionsForIziAppBuilderAsUnityEcsAbstractions
    {
        [ExtendApp(EUseType.Independent)]
        public static void UseEcsAbstractions(this IIziAppBuilder iziAppBuilder)
        {
            var service = new UnityEcsService();
            iziAppBuilder.AddService(service);
        }
    }
}
