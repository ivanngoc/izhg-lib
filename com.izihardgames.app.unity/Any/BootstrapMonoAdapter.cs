using IziHardGames.Apps.NetStd21;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;
using IziHardGames.Apps.ForUnity.Presets;
using IziHardGames.Apps.ForUnity;
using IziHardGames.Attributes;
using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;

namespace IziHardGames.Apps.ForUnity
{
    [Bootstrap]
    [RequireComponent(typeof(StartupArguments))]
    [RequireComponent(typeof(AbstractAppFactory))]
    public sealed class BootstrapMonoAdapter : MonoBehaviour
    {
        private IziApp? app;
        [Header("Startup")]
        [SerializeField] private StartupMono? startup;
        [SerializeField] private ScriptableStartup? startupScriptable;
        [SerializeField] private StartupArguments? arguments;
        [Header("Presets")]
        [SerializeField] private ProjectPresets? presets;
        [Space]
        [Header("App")]
        [SerializeField] private AbstractAppFactory? factory;
        [Space]
        [Header("Enter Point")]
        [SerializeField] private ScriptableEnterPoint? scriptableEnterPoint;
        [SerializeField] private AbstractUnityEnterPointMono? unityEnterPointMono;
        [Space]
        [SerializeField] private bool isAsyncStartup = true;

        public ProjectPresets Presets => presets!;

        private async void Awake()
        {
#if UNITY_EDITOR 
            //GetComponent<MonoCleanup>().Cleanup(); //static class IziHardGames.Apps.ForUnity./
#endif
            if (isAsyncStartup)
            {
                IziAppBuilder builder = new IziAppBuilder();
                IIziAppBuilder.Singleton = builder;

                Task earlyStartupMono = Task.CompletedTask;
                Task earlyStartupScriptable = Task.CompletedTask;

                if (startup != null)
                {
                    earlyStartupMono = startup.StartAsync(builder);
                }

                if (startupScriptable != null)
                {
                    earlyStartupScriptable = startupScriptable.StartAsync(builder, this.gameObject, arguments!);
                }
                await Task.WhenAll(earlyStartupMono, earlyStartupScriptable).ConfigureAwait(true);

                Task lateStartupMono = Task.CompletedTask;
                Task lateStartupScriptable = Task.CompletedTask;

                if (startup != null)
                {
                    lateStartupMono = startup.LateStartupAsync();
                }
                if (startupScriptable != null)
                {
                    lateStartupScriptable = startupScriptable.LateStartupAsync();
                }
                await Task.WhenAll(lateStartupMono, lateStartupScriptable).ConfigureAwait(true);

                var app = await factory!.CreateAsync(builder).ConfigureAwait(true);

                startup?.FinishStartup(app);
                startupScriptable?.FinishStartup(app);
                IStartup.FinishStartupGlobal();
#if UNITY_EDITOR||DEBUG
                Debug.Log("Startup funished", this);
#endif
                await app.StartAsync().ConfigureAwait(true);
#if UNITY_EDITOR || DEBUG
                Debug.Log("App Started Async", this);
#endif
                if (scriptableEnterPoint != null)
                {
                    scriptableEnterPoint.Run();
                }
                if (unityEnterPointMono != null)
                {
                    await unityEnterPointMono.RunAsync();
                }
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        public void OnValidate()
        {
            startup = GetComponent<StartupMono>();
            if (startup == null && startupScriptable == null)
            {
                Debug.LogError($"Startup is Empty. You must specify Startup!", this);
            }
            factory = factory ?? GetComponent<AbstractAppFactory>();
        }
    }
}