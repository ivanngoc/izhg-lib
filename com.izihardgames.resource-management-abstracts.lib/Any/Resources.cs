using System;
using System.Threading.Tasks;

namespace IziHardGames.ResourceManagement.Abstractions.Lib
{
    public static class Resources
    {
        public static object Ensure { get; set; }
        public static SelectorForResourceLoader Load { get; set; }
#if DEBUG
        internal static void TestAPI()
        {
            ResourceSource source = default;
            var task = Resources.Load[typeof(object)].From(source);
        }
#endif
    }
    public class SelectorForResourceLoader
    {
        public ResourceLoader this[Type type] => throw new System.NotImplementedException();
    }

    public abstract class ResourceSource
    {

    }

    public abstract class ResourceLoader
    {
        internal abstract Task From(ResourceSource source);
    }
}
