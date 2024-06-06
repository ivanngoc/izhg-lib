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
        public static void UseMonoUpdates(this IIziAppBuilder builder, GeneratorOfUpdates generator)
        {
            //var early = new UpdateChannelForUnity("IziEarlyLoop");
            //var normal = new UpdateChannelForUnity("IziNormalLoop");

            ////IziTicks.Early = ;
            //IziTicks.Normal = normal;
            //IziTicks.Fixed = new UpdateChannelForUnity("IziFixedLoop");
            //IziTicks.Late = new UpdateChannelForUnity("IziLateLoop");
            //IziTicks.Reset = new UpdateChannelForUnity("IziResetLoop");

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