using System;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.NetStd21;
using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using IziHardGames.UserControl.Lib.Contexts;
using UnityEngine;

namespace IziHardGames.UserControl.ForUnity
{

    /// <summary>
    /// <see cref="ExtensionsForIziAppBuilderAsUserControlUnity.UseUserControlForUnity(IIziAppBuilder)"/>
    /// </summary>
    public class UserControlMonoService : IIziService
    {
        private IziInputSystem? iziInputSystem;
        internal UserEnvironment? environment;
        private IInputCollector? collector;

        private int tokenNormalInput;
        private int tokenFixedInput;
        private int tokenResetInput;
        private int envNormal1;
        private int envNormal2;
        private int envNormal3;
        private int envNormal4;
        private int envNormal5;
        private int envNormal6;
        private int envReset;

        private readonly Action<object> enable;
        private readonly Action<object> disable;
        private readonly Action actionUsersLoop;

        public UserControlMonoService()
        {
            enable = EnableCameraDepended;
            disable = DisableCameraDepended;
            actionUsersLoop = HandleUsersLoop;
        }

        private void HandleUsersLoop()
        {
            var selector = IziEnvironment.Environments;
            if (selector != null)
            {
                Camera camera = IziApp.Singleton!.GetSingleton<Camera>();
                var pos = camera.transform.position;
                foreach (var userEnv in selector.All)
                {
                    userEnv.SetViewerPositionXYZ(pos.x, pos.y, pos.z);
                }
            }
        }

        public void Start()
        {
            Debug.Log($"Trace");
            var app = IziApp.Singleton ?? throw new NullReferenceException();

            if (app.TryGetSingleton<Camera>(out var camera))
            {
                EnableCameraDepended(camera!);
            }
            else
            {
                app.OnSingletonAddSync<Camera>(enable);
                app.OnSingletonRemoveSync<Camera>(disable);
            }
        }

        public void EnableCameraDepended(object camemra)
        {
            ValidateCamera();
            var app = IziApp.Singleton ?? throw new NullReferenceException();

            collector = app.GetSingleton<IInputCollector>();
            iziInputSystem = app.GetSingleton<IziInputSystem>();

            this.tokenNormalInput = IziTicks.Normal!.Enable("TICK_KEY:IInputCollector.CollectAtNormalUpdate()", collector.CollectAtNormalUpdate);

            this.envNormal1 = IziTicks.Normal.Enable("TICK_KEY:environment.CollectInfo()", environment!.CollectInfo);
            this.envNormal2 = IziTicks.Normal.Enable("TICK_KEY:environment.InternalCalculation()", environment.InternalCalculation);
            this.envNormal3 = IziTicks.Normal.Enable("TICK_KEY:environment.ShareInternalCalculation()", environment.ShareInternalCalculation);
            this.envNormal4 = IziTicks.Normal.Enable("TICK_KEY:environment.Filter()", environment.Filter);
            this.envNormal5 = IziTicks.Normal.Enable("TICK_KEY:environment.Execute()", environment.Execute);
            this.envNormal6 = IziTicks.Normal.Enable("TICK_KEY:UserControlMonoService.actionUsersLoop", actionUsersLoop);

            this.tokenFixedInput = IziTicks.Fixed!.Enable("TICK_KEY:collector.CollectAtFixedUpdate()", collector.CollectAtFixedUpdate);
            this.tokenResetInput = IziTicks.Reset!.Enable("TICK_KEY:collector.ResetForNextLoop()", collector.ResetForNextLoop);
            this.envReset = IziTicks.Reset.Enable("TICK_KEY:environment.Clean()", environment.Clean);
        }

        private void ValidateCamera()
        {
#if DEBUG
            Debug.Log($"[{Time.frameCount}] ValidateCamera" + "\r\n" + IziApp.Singleton.GetListOfSingletons());
            Camera camera = IziApp.Singleton.GetSingleton<Camera>();
            if (camera == null) throw new InvalidOperationException("No Camera Founded as Singleton in IziApp");
#endif
        }

        public void Stop()
        {
            var app = IziApp.Singleton ?? throw new NullReferenceException();
            app.OnSingletonAddReverse<Camera>(EnableCameraDepended);
            app.OnSingletonRemoveReverse<Camera>(DisableCameraDepended);
        }
        public void DisableCameraDepended(object camera)
        {
            var normal = IziTicks.Normal ?? throw new NullReferenceException();

            normal.Disable(tokenNormalInput);

            normal.Disable(envNormal1);
            normal.Disable(envNormal2);
            normal.Disable(envNormal3);
            normal.Disable(envNormal4);
            normal.Disable(envNormal5);
            normal.Disable(envNormal6);

            IziTicks.Fixed.Disable(tokenFixedInput);
            IziTicks.Reset.Disable(tokenResetInput);
            IziTicks.Reset.Disable(envReset);
        }
    }
}