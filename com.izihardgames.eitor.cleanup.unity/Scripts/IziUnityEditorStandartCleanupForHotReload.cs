using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.Abstractions.NetStd21;
using IziHardGames.Apps.ForUnity;
using IziHardGames.Apps.NetStd21;
using IziHardGames.Apps.Scenes.Unity;
using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;
namespace IziHardGames.ForUnityEditor.StaticCleanup
{
    [CreateAssetMenu(fileName = nameof(IziUnityEditorStandartCleanupForHotReload), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_UNITY_EDITOR + "/" + nameof(IziUnityEditorStandartCleanupForHotReload))]
    public class IziUnityEditorStandartCleanupForHotReload : ScriptableObject, ICleanupFastReload
    {
        public void CleanupForFastReload()
        {
#if UNITY_EDITOR||DEBUG
            IziApp.CleanupStatic();
            IIziApp.Singleton = default;
            IIziAppBuilder.Singleton = default;
            IziEnvironment.CleanupStatic();
            RegistryForInputSystem.inputContainerFactory.Cleanup();
            IziTicks.CleanupStatic();
            IziHandler.CleanupStatic();
            ControlForScenes.CleanupStatic();
#endif
        }
    }
}
