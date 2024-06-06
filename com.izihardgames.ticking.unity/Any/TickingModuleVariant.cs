using Mono.Cecil;
using UnityEngine;
using IziHardGames.AppConstructor;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;
using System;
using System.Runtime.InteropServices;
using IziHardGames.Ticking.Lib;
using IziHardGames.Libs.NonEngine.Game.Abstractions;

namespace IziHardGames.Ticking
{
    /*
    Services - singletons 100%
    Singletons
    Transients
    Scoped
    Objects
    */
    [Guid("eb809b3d-b383-4a46-97ac-a80d84b53ab5")]
    [CreateAssetMenu(fileName = nameof(TickingModuleVariant), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_MODULES + "/" + nameof(TickingModuleVariant))]
    public class TickingModuleVariant : IziModuleBind, IModuleBind
    {
        private TickInitilizer tickInitilizer = new TickInitilizerStandart();

        public override void LoadModuleBegin(IziAppModuled app)
        {      
            tickInitilizer.InitilizeBegin(app);
        }
        public override void LoadModuleEnd()
        {
            tickInitilizer.InitilizeEnd();
        }

        public override bool IsLoadCompleted()
        {
           return tickInitilizer.Complete;
        }

        public override void ItterateLoading()
        {

        }
        public override void CleanupStaticFields()
        {
            IziTicks.CleanupStatic();
        }

        public override bool ResolveDependecies()
        {
            return true;
        }
    }
}
