using System;
using System.Collections.Generic;

namespace IziHardGames.Apps.Abstractions.Lib
{
    public interface IIziAppBuilder
    {
        public IEnumerable<KeyValuePair<Type, object>> Singletons { get; }
        public IEnumerable<KeyValuePair<Type, IIziService>> Services { get; }
        public static IIziAppBuilder? Singleton { get; set; }
        public void AddSingleton<T>(T item);
        public void AddService<T>(T service) where T : IIziService;
        public object GetSingleton(Type type);
        public T GetSingleton<T>() where T : class;
    }
}
