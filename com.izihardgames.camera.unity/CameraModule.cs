using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using IziHardGames.AppConstructor;
using IziHardGames.DependencyInjection.Promises;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.IziCamera
{
    [Guid("3ad5cc90-c07b-4917-9de1-7625c0d1da72")]
    [CreateAssetMenu(fileName = nameof(CameraModule), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_MODULES + "/" + nameof(CameraModule))]
    public class CameraModule : IziModuleBind
    {
        private IziBox<Camera> cameraBox;
        public override void LoadModuleBegin(IziAppModuled app)
        {
            cameraBox = new IziBox<Camera>();
            app.PutItem("MainCamera", cameraBox);
            var comp = Object.FindObjectsOfType<Camera>().FirstOrDefault(x => x.tag == "MainCamera");
            if (comp != null) cameraBox.SetValue();
        }

        public override void LoadModuleEnd()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsLoadCompleted()
        {
            throw new System.NotImplementedException();
        }

        public override void ItterateLoading()
        {
            throw new System.NotImplementedException();
        }

        public override void CleanupStaticFields()
        {
            throw new System.NotImplementedException();
        }

        public override bool ResolveDependecies()
        {
            throw new System.NotImplementedException();
        }
    }
}
