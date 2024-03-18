using System;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.ForUnity.Presets
{
    /// <summary>
    /// <see cref="AbstractUnityEnterPointMono"/>
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ScriptableEnterPoint), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_IZHG_STANDART + "/" + nameof(ScriptableEnterPoint))]
    public class ScriptableEnterPoint : ScriptableObject
    {        
        [SerializeField] public ScriptableObject[] scriptables = Array.Empty<ScriptableObject>();

        public void Run()
        {
            foreach (var item in scriptables)
            {
                if (item is IAppEnterPoint appEnter)
                {
                    appEnter.Run();
                }
            }
        }
    }
}