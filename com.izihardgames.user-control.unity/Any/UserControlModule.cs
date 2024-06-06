using System;
using System.Runtime.InteropServices;
using IziHardGames.AppConstructor;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.UserControl.ForUnity
{
    [Guid("b945212b-1b0e-4c5a-8f8b-c5adbafac985")]
    [CreateAssetMenu(fileName = nameof(UserControlModule), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_MODULES + "/" + nameof(UserControlModule))]
    public class UserControlModule : IziModuleBind
    {
        private UserControlModuleInitilizer initilizer = new UserControlModuleInitilizerStandart();
        private IziAppModuled app;
        public override void LoadModuleBegin(IziAppModuled app)
        {
            this.app = app;
            initilizer.LoadModule(app);
        }

        public override void LoadModuleEnd()
        {
            initilizer.LoadModuleEnd();            
        }

        public override bool IsLoadCompleted()
        {
            throw new NotImplementedException();
        }

        public override void ItterateLoading()
        {
            throw new NotImplementedException();
        }

        public override void CleanupStaticFields()
        {
            throw new NotImplementedException();
        }

        public override bool ResolveDependecies()
        {
           return initilizer.ResolveDependecies(app);
        }
    }
}