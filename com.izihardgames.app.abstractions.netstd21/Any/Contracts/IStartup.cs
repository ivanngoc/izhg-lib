using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.NetStd21;
using IziHardGames.DependencyInjection.Promises;

namespace IziHardGames.Apps.Abstractions.Lib
{

    public interface IStartupDelayed
    {

    }

    public interface IStartupAcionAsyncNetStd21
    {
        Task ExecuteAsync();
    }

    public interface IStartupAcion
    {
        void Execute();
    }

    public interface IAppEnterPoint
    {
        public void Run();
    }

    public interface IStartup
    {
        public static IStartup? Startup { get; set; }
        /// <summary>
        /// Do not use that method. invoke <see cref="FinishStartupGlobal"/> Uppon startup is finished
        /// </summary>
        public void FinishStartup(IIziApp app);
        public Task LateStartupAsync();

        public static void BeginStartupGlobal(IStartup startup)
        {
            Startup = startup;
        }
        public static void FinishStartupGlobal()
        {
            IIziApp.IsStartupFinished = true;
        }
    }

    public interface IIziAppFactory
    {
        public Task<IIziApp> CreateAsync(IIziAppBuilder builder);
    }

    public interface IIziApp
    {
        public static bool IsStartupFinished { get; internal set; }
        public static IIziApp? Singleton { get; set; }
        public Task StartAsync();
        public T GetSingleton<T>() where T : class;
        public IziDynamicPromise<T> GetDynamicPromise<T>() where T : class;
    }
    public interface IIziAppBuilder
    {
        public IEnumerable<KeyValuePair<Type, object>> Singletons { get; }
        public IEnumerable<KeyValuePair<Type, IIziService>> Services { get; }
        public static IIziAppBuilder? Singleton { get; set; }
        public void AddSingleton<T>(T item);
        public void AddService<T>(T service) where T : IIziService;
        public object GetSingleton(Type type);
        public T GetSingleton<T>() where T : class;
        public IziDynamicPromise<T> GetDynamicPromise<T>() where T : class;
    }

    public static class ExtensionsForIziAppBuilderAsAppsAbstractions
    {
        public static void EnsureUseIziTypesByDefault(this IIziAppBuilder builder)
        {
            throw new System.NotImplementedException();
        }
        public static void UseIziTypesByDefault(this IIziAppBuilder builder)
        {
            var def = new TypesRegistry();
            IziTypes.Default = def;
            IziTypes.select[typeof(TypesRegistry)] = def;
        }
    }
}
