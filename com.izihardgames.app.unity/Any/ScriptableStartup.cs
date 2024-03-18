using UnityEngine;
using IziHardGames.Apps.Abstractions.Lib;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;
using System.Threading.Tasks;
using System;
using IziHardGames.Apps.NetStd21;
using System.Collections.Generic;
using IziHardGames.Apps.ForUnity;
using IziHardGames.Apps.ForUnity;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    [CreateAssetMenu(fileName = nameof(ScriptableStartup), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_IZHG_STANDART + "/" + nameof(ScriptableStartup))]
    public class ScriptableStartup : ScriptableObject, IStartup
    {
        [SerializeField] private List<ScriptableObject>? startupActions;

        internal async Task StartAsync(IIziAppBuilder builder, GameObject bootstrapGo, StartupArguments arguments)
        {
#if UNITY_EDITOR
            MonoCleanup monoCleanup = bootstrapGo.GetComponent<MonoCleanup>();
#endif

            IStartup.BeginStartupGlobal(this);

            if (startupActions != null)
            {
                foreach (var item in startupActions)
                {
                    if (item is IStartupAcionAsyncNetStd21 actionNetstd21)
                    {
                        await actionNetstd21.ExecuteAsync().ConfigureAwait(true);
                    }
                    else if (item is IStartupAcion acion)
                    {
                        acion.Execute();
                    }
                }
            }
        }

        public void FinishStartup(IIziApp app)
        {

        }

        public Task LateStartupAsync()
        {
            return Task.CompletedTask;
        }
    }
}