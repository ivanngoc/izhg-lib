using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.NetStd21;
using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.Ticking.Abstractions.Lib;
using IziHardGames.Ticking.Lib;
using IziHardGames.Ticking.ForUnity;
using UnityEngine.LowLevel;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using System.Collections.ObjectModel;
using System.Linq;

namespace IziHardGames.Libs.Engine.Updating
{
    public static class ExtensionsForIziAppBuilderAsTickingUnity
    {
        public static void UseMonoUpdates(this IIziAppBuilder builder)
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            var list = new List<PlayerLoopSystem>();
            var existed = loop.subSystemList;

            foreach (var item in existed)
            {
                list.Add(item);

                if (item.type == typeof(Initialization))
                {
                    var init = new UpdateChannelForUnity($"IziLoop:{nameof(IziTicks.Initilization)}");
                    IziTicks.Initilization = init;
                    list.Add(new PlayerLoopSystem()
                    {
                        subSystemList = null,
                        type = typeof(UpdateChannelForUnity),
                        updateDelegate = init.ExecuteSync,
                    });
                }
                else if (item.type == typeof(FixedUpdate))
                {
                    var early = new UpdateChannelForUnity($"IziLoop:{nameof(IziTicks.BeforePhysics)}");
                    IziTicks.Initilization = early;

                    list.Add(new PlayerLoopSystem()
                    {
                        subSystemList = null,
                        type = typeof(UpdateChannelForUnity),
                        updateDelegate = early.ExecuteSync,
                    });
                }
            }
            loop.subSystemList = list.ToArray();
            PlayerLoop.SetPlayerLoop(loop);
        }
        public static void UseMonoUpdates(this IIziAppBuilder builder, GeneratorOfUpdates generator)
        {
            var early = new UpdateChannelForUnity("IziEarlyLoop");
            var normal = new UpdateChannelForUnity("IziNormalLoop");

            //IziTicks.Early = ;
            IziTicks.Normal = normal;
            IziTicks.Fixed = new UpdateChannelForUnity("IziFixedLoop");
            IziTicks.Late = new UpdateChannelForUnity("IziLateLoop");
            IziTicks.Reset = new UpdateChannelForUnity("IziResetLoop");

            builder.UseTickingByDefault();
            builder.AddService(generator);

        }
        private static PlayerLoopSystem AddSystem<T>(in PlayerLoopSystem loopSystem, PlayerLoopSystem systemToAdd) where T : struct
        {
            var newPlayerLoop = new PlayerLoopSystem()
            {
                loopConditionFunction = loopSystem.loopConditionFunction,
                type = loopSystem.type,
                updateDelegate = loopSystem.updateDelegate,
                updateFunction = loopSystem.updateFunction
            };

            var newSubSystemList = new List<PlayerLoopSystem>();

            foreach (var subSystem in loopSystem.subSystemList)
            {
                newSubSystemList.Add(subSystem);

                if (subSystem.type == typeof(T))
                    newSubSystemList.Add(systemToAdd);
            }
            newPlayerLoop.subSystemList = newSubSystemList.ToArray();
            return newPlayerLoop;
        }
    }
    public class UpdateChannelForUnity : TickChannelGraphBased
    {
        private string name;
        public UpdateChannelForUnity(string name)
        {
            this.name = name;
        }
        public override void ExecuteSync()
        {
#if UNITY_EDITOR || DEBUG
            UnityEngine.Profiling.Profiler.BeginSample($"UpdateChannelForUnity.{name}");
#endif
            base.ExecuteSync();
#if UNITY_EDITOR || DEBUG
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
    }
}