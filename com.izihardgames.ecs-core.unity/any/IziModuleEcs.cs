using System.Linq;
using System.Runtime.InteropServices;
using IziHardGames.AppConstructor;
using IziHardGames.Apps.ForEcs.Abstractions.ForUnity;
using Unity.Entities;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;
namespace IziHardGames.Apps.ForUnity.ForEcs
{
    [Guid("7cb56898-20ee-428e-8e4c-ae426727d49b")]
    [CreateAssetMenu(fileName = nameof(IziModuleEcs), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_MODULES + "/" + nameof(IziModuleEcs))]
    public class IziModuleEcs : IziModuleBind
    {
        private UnityEcsService unityEcsService;
        public override void LoadModuleBegin(IziAppModuled app)
        {
            unityEcsService = new UnityEcsService();
            app.PutItem(unityEcsService.GetType().FullName, unityEcsService);
        }

        public override void LoadModuleEnd()
        {
            IziUnityEcs.NotifyDefaultWorldCreated(World.DefaultGameObjectInjectionWorld);
        }

        public override bool IsLoadCompleted() => IziUnityEcs.IsEcsInitilized;

        public override void ItterateLoading()
        {
            if (World.DefaultGameObjectInjectionWorld.IsCreated)
            {
                IziUnityEcs.IsEcsInitilized = true;
            }
        }

        public override void CleanupStaticFields()
        {
            IziUnityEcs.CleanupStatic();
        }

        public override bool ResolveDependecies()
        {
            return true;
        }
    }
}
