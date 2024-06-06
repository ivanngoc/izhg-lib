using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.DependencyInjection.Promises;
using AsyncFact = IziHardGames.Apps.NetStd21.AsyncFactory<object>;

namespace IziHardGames.Apps.NetStd21
{
    public sealed class IziAppBuilder : IIziAppBuilder
    {
        public IEnumerable<KeyValuePair<Type, object>> Singletons { get => singletons; }
        public IEnumerable<KeyValuePair<Type, IIziService>> Services { get => services; }

        private List<object> items = new List<object>();
        private List<AsyncFact> asyncFactories = new List<AsyncFact>();
        public readonly Dictionary<Type, IIziService> services = new Dictionary<Type, IIziService>();
        internal readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();
        public T Build<T>(Func<IziAppBuilder, T> factory) where T : IziApp
        {
            T app = factory(this);
            return app;
        }
        public async Task<T> BuildAsync<T>(Func<IziAppBuilder, Task<T>> factory) where T : IziApp
        {
            foreach (var item in asyncFactories)
            {
                items.Add(await item());
            }
            var app = await factory(this);
            return app;
        }

        public void AddSingleton<T>() where T : class, new()
        {
            singletons.Add(typeof(T), new T());
        }
        public void AddSingleton<T>(T item)
        {
            singletons.Add(typeof(T), item);
        }
        public void AddService<T>(T service) where T : IIziService
        {
            AddSingleton(service);
            services.Add(typeof(T), service);
        }
        public void AddService<T>() where T : IIziService, new()
        {
            services.Add(typeof(T), new T());
        }
        public void AddModule<T>() where T : class, new()
        {
            items.Add(new T());
        }
        public void AddModule<T>(Func<T> factory)
        {
            items.Add(factory()!);
        }
        public void AddModule(AsyncFact factory)
        {
            asyncFactories.Add(factory);

        }

        public void AddStartup()
        {

        }

        public T GetSingleton<T>() where T : class
        {
            if (singletons.TryGetValue(typeof(T), out var result))
            {
                return (result as T) ?? throw new InvalidCastException();
            }
            throw new System.NullReferenceException($"Singleton with type:{typeof(T).Name} is Not Presented");
        }

        public object GetSingleton(Type type)
        {
            return singletons[type];
        }

        public IziBox<T> GetDynamicPromise<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
