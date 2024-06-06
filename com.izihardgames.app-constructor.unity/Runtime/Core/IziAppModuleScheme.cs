using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;


namespace IziHardGames.AppConstructor
{
    [CreateAssetMenu(fileName = nameof(IziAppModuleScheme), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_MODULES + "/" + nameof(IziAppModuleScheme))]
    public class IziAppModuleScheme : ScriptableObject
    {
        public int orderToStartup;
        public List<IziModuleBind> binds = new List<IziModuleBind>();
        internal void Begin(IziAppModuled app)
        {
            foreach (var bind in binds)
            {
                app.PutItem(bind.GuidAsString, bind);
                bind.LoadModuleBegin(app);
            }
        }
        internal void End()
        {
            foreach (var bind in binds)
            {
                bind.LoadModuleEnd();
                bind.status.SetAsLoaded();
            }
        }
        internal void CleanupStaticFields()
        {
            foreach (var bind in binds)
            {
                bind.CleanupStaticFields();
            }
        }

        internal void ItterateLoading()
        {
            foreach (var bind in binds)
            {
                if (!bind.status.Loaded)
                {
                    bind.ItterateLoading();
                }
            }
        }
        internal bool IsLoadCompleted()
        {
            foreach (var bind in binds)
            {
                if (!bind.IsLoadCompleted()) return false;
            }
            return true;
        }

        internal bool ResolveDependecies()
        {
            foreach (var bind in binds)
            {
                if (!bind.status.Resolved && bind.ResolveDependecies())
                {
                    bind.status.SetAsResolved();
                }
            }
            return binds.All(x => x.status.Resolved);
        }
    }



    /*
    Services - singletons 100%
    Singletons
    Transients
    Scoped
    Objects
    */

    public class IziModuleStatus
    {
        private bool loaded;
        private bool resolved;
        public bool Loaded => loaded;
        public bool Resolved => resolved;
        internal void SetAsLoaded()
        {
            loaded = true;
        }
        internal void SetAsResolved()
        {
            resolved = true;
        }
    }
}
