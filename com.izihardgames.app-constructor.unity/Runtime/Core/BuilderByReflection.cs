using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IziHardGames.AppConstructor
{
    public class BuilderByReflection
    {
        public void Build()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var guidAtr = type.GetCustomAttribute<GuidAttribute>();
                    if (guidAtr != null)
                    {
                        var guid = Guid.Parse(guidAtr.Value);
                    }
                }
            }
        }
    }
}
