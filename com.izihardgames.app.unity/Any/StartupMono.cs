using System;
using System.Threading.Tasks;
using IziHardGames.Apps.NetStd21;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;
using UnityEngine.Events;

namespace IziHardGames.Apps.ForUnity
{
    /// <summary>
    /// Analog of startup in ASP.NET Core app. Create and configurate specific things for given project.
    /// </summary>
    public abstract class StartupMono : MonoBehaviour, IStartup
    {
        [SerializeField] private UnityEvent? unityEvent;
        [SerializeField] private bool isFinished;
        [SerializeField] protected BootstrapMonoAdapter? bootstrapMono;

        private IziAppBuilder? builder;


        #region Unity Message
        protected virtual void OnValidate()
        {
            bootstrapMono = GetComponent<BootstrapMonoAdapter>();
        }
        protected virtual void Reset()
        {

        }
        #endregion
        public virtual void StartSync(IziAppBuilder builder)
        {
            this.builder = builder;
            InitilizePresets();
            IStartup.BeginStartupGlobal(this);
            CallInternalUses(builder as IIziAppBuilder);
        }
        public virtual Task StartAsync(IziAppBuilder builder)
        {
            StartSync(builder);
            return Task.CompletedTask;
        }
        public virtual void FinishStartup(IIziAppVersion1 app)
        {
            unityEvent?.Invoke();
            isFinished = true;
        }

        protected abstract void InitilizePresets();

        /// <summary>
        /// Without using links from another scenes.
        /// </summary>
        public virtual void ConfigureInner()
        {

        }
        public virtual void ConfigureInnerReverse()
        {

        }
        /// <summary>
        /// With using links from another scenes
        /// </summary>
        public virtual void ConfigureOuter()
        {

        }
        public virtual void ConfigureOuterReverse()
        {

        }

        public virtual void UpdatesEnable()
        {

        }
        public virtual void UpdatesDisable()
        {

        }

        public virtual void ConfigureAfterDataLoaded()
        {

        }
        public virtual void ConfigureAfterDataLoadedReverse()
        {

        }

        public virtual void FinishInnerInitilization()
        {

        }

        public virtual Task LateStartupAsync()
        {
            CallUsesWithResolving(builder ?? throw new NullReferenceException());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Подключение независимых пакетов<br/>
        /// использование методов-расширений для <see cref="IIziAppBuilder"/> в которых не требуются внешние по отношению к сборке связи.<br/>
        /// Подключение модуля не имеет связей с другими пакетами/модулями/сборками<br/>
        /// </summary>
        protected virtual void CallInternalUses(IIziAppBuilder builder)
        {
#if UNITY_EDITOR || DEBUG
            builder.UseDebugLogger();
#else
            builder.UseLogging();
#endif
        }

        /// <summary>
        /// использование методов-расширений для <see cref="IIziAppBuilder"/> в которых требуются внешние по отношению к сборке связи
        /// </summary>
        protected virtual void CallUsesWithResolving(IIziAppBuilder builder)
        {

        }
    }
}

