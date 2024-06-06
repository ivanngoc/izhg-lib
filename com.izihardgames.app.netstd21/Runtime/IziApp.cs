using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IziHardGames.Libs.NonEngine.Applications;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Attributes;
using IziHardGames.DependencyInjection.Promises;

namespace IziHardGames.Apps.NetStd21
{
    public class IziApp : IIziAppVersion1
    {
        public Type this[Type key] => throw new System.NotImplementedException();

        public static IziApp? Singleton;
        private static event Action? OnAppRunning;
        private static TaskCompletionSource<IziApp> tcs;

        protected readonly Modules modules = new Modules();
        protected readonly Dictionary<Type, IIziService> services = new Dictionary<Type, IIziService>();

        protected bool isStarted;
        protected readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        private readonly Dictionary<Type, SetOfHandlers> onAddHandlers = new Dictionary<Type, SetOfHandlers>();
        private readonly Dictionary<Type, SetOfHandlers> onSwapHandlers = new Dictionary<Type, SetOfHandlers>();
        private readonly Dictionary<Type, SetOfHandlers> onRemoveHandlers = new Dictionary<Type, SetOfHandlers>();


        static IziApp()
        {
            tcs = new System.Threading.Tasks.TaskCompletionSource<IziApp>();
            OnAppRunning += () => { tcs.SetResult(Singleton); };
        }

        [UnityHotReloadEditor]
        public static void CleanupStatic()
        {
            IziApp.Singleton = default;
            OnAppRunning = default;
            tcs = new TaskCompletionSource<IziApp>();
            OnAppRunning += () => { tcs.SetResult(Singleton); };
        }

        public IziApp(IIziAppBuilder builder)
        {
            foreach (var pair in builder.Services)
            {
                services.Add(pair.Key, pair.Value);
            }
            foreach (var item in builder.Singletons)
            {
                singletons.Add(item.Key, item.Value);
            }
        }
        /// <summary>
        /// App Created and started
        /// </summary>
        /// <param name="iziApp"></param>
        private void SetApp(IziApp iziApp)
        {
            Singleton = iziApp;
            IIziAppVersion1.Singleton = iziApp;
            OnAppRunning?.Invoke();
        }

        public virtual void Start()
        {
            if (isStarted) throw new InvalidOperationException("App is Already started");
            isStarted = true;
            SetApp(this);

            foreach (var service in services.Values)
            {
                service.Start();
            }
        }

        public virtual async Task StartAsync()
        {
            Start();
            await Task.CompletedTask;
        }
        public T GetSingleton<T>() where T : class
        {
            return singletons[typeof(T)] as T;
        }

        public T GetService<T>() where T : class, IIziService
        {
            return services[typeof(T)] as T;
        }

        public bool TryGetSingleton<T>(out T? singleton) where T : class
        {
            if (singletons.TryGetValue(typeof(T), out var existed))
            {
                singleton = existed as T;
                return true;
            }
            singleton = null;
            return false;
        }

        public void AddSingletonTemp<T>(T target)
        {
            lock (singletons)
            {
                singletons.Add(typeof(T), target);
            }
            NotifyAddSync(typeof(T), target);
        }
        public void AddSingletonTemp(Type type, object target)
        {
            lock (singletons)
            {
                singletons.Add(type, target);
            }
            NotifyAddSync(type, target);
        }

        public void RemoveSingletonTemp(Type type)
        {
            var target = ValidateExists(type);
            lock (singletons)
            {
                singletons.Remove(type);
            }
            NotifyRemoveSync(type, target);
        }
        public void RemoveSingletonTemp<T>()
        {
            var target = ValidateExists(typeof(T));
            lock (singletons)
            {
                singletons.Remove(typeof(T));
            }
            NotifyRemoveSync(typeof(T), target);
        }
        public void RemoveSingletonTemp<T>(T target) where T : class
        {
            if (singletons.TryGetValue(typeof(T), out object existed))
            {
                if (!existed.Equals(target)) throw new ArgumentException("Existed and given Singletons Are not the same object");
                lock (singletons)
                {
                    singletons.Remove(typeof(T));
                }
                NotifyRemoveSync(typeof(T), target);
            }
            throw new ArgumentException($"Singleton of type {typeof(T).FullName} is not presented");
        }

        private object ValidateExists(Type type)
        {
            if (!singletons.TryGetValue(type, out var target))
            {
                throw new ArgumentException($"Singleton typeof({type.FullName}) Are not presented in Singletons");
            }
            return target ?? throw new NullReferenceException("Singleton value is null");
        }


        public void OnSingletonAddSync<T>(Action<object> handler) where T : class
        {
            if (!onAddHandlers.TryGetValue(typeof(T), out var setAdd))
            {
                setAdd = new SetOfHandlers();
                onAddHandlers.Add(typeof(T), setAdd);
            }
            setAdd.Add(handler);
        }
        public void OnSingletonAddReverse<T>(Action<object> handler)
        {
            onAddHandlers[typeof(T)].Remove(handler);
        }
        public void OnSingletonSwapSync<T>(Action<object> handler) where T : class
        {
            if (!onSwapHandlers.TryGetValue(typeof(T), out var setSwap))
            {
                setSwap = new SetOfHandlers();
                onSwapHandlers.Add(typeof(T), setSwap);
            }
            setSwap.Add(handler);
        }
        public void OnSingletonRemoveSync<T>(Action<object> handler) where T : class
        {
            if (!onRemoveHandlers.TryGetValue(typeof(T), out var set))
            {
                set = new SetOfHandlers();
                onRemoveHandlers.Add(typeof(T), set);
            }
            set.Add(handler);
        }
        public void OnSingletonRemoveReverse<T>(Action<object> handler) where T : class
        {
            onRemoveHandlers[typeof(T)].Remove(handler);
        }

        public void OnSingletonAddOrSwapSync<T>(Action<object> handler) where T : class
        {
            OnSingletonAddSync<T>(handler);
            OnSingletonSwapSync<T>(handler);
        }

        private void NotifyAddSync(Type type, object target)
        {
            if (onAddHandlers.TryGetValue(type, out var setAdd))
            {
                setAdd.Execute(target);
            }
        }
        private void NotifyRemoveSync(Type type, object target)
        {
            if (onRemoveHandlers.TryGetValue(type, out var set))
            {
                set.Execute(target);
            }
        }
        public static Task<IziApp> AwaitCreated()
        {
            return tcs.Task;
        }

        public string GetListOfSingletons() => singletons.Select(x => $"{x.Key.FullName}; {x.Value.ToString()}").Aggregate((x, y) => x + Environment.NewLine + y);

        public IziBox<T> GetDynamicPromise<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }

    public class Modules
    {
        internal readonly List<object> items = new List<object>();

    }

    public delegate Task<T> AsyncFactory<T>();

    internal class SetOfHandlers
    {
        private readonly List<Action<object>> actions = new List<Action<object>>();

        internal void Add(Action<object> value)
        {
            actions.Add(value);
        }

        internal void Execute(object target)
        {
            foreach (var action in actions)
            {
                action.Invoke(target);
            }
        }

        internal void Remove(Action<object> value)
        {
#if DEBUG
            if (!actions.Contains(value)) throw new ArgumentException($"There is no handler reigstrd. TargetType:{value.Target.GetType().FullName}. method name:{value.Method.Name}");
#endif
            actions.Remove(value);
        }
    }
}