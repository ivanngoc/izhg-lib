using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using IziHardGames.Attributes;
using System.Threading;

namespace IziHardGames.Apps.ForEcs.Abstractions.ForUnity
{
    public static class IziUnityEcs
    {
        public static bool IsWorldCreated { get; set; }
        public static bool IsEcsInitilized { get; set; }
        private static List<Action<World>> actions = new List<Action<World>>();
        private static World? world;
        public static World World => world ?? throw new NullReferenceException();
        private static TaskCompletionSource<World> tcs = new TaskCompletionSource<World>();

        public static void NotifyDefaultWorldCreated(World world)
        {
            IziUnityEcs.world = world;
            foreach (var item in actions)
            {
                item.Invoke(world);
            }
            actions.Clear();
            tcs.SetResult(world);
        }

        public static bool TryGetDefaultWorld(out World? world)
        {
            world = IziUnityEcs.world;
            return world != null;
        }

        internal static void OnWorldCreated(Action<World> action)
        {
            actions.Add(action);
        }

        public static Task<World> AwaitDefaultWorldCreation()
        {
            var result = Interlocked.CompareExchange(ref world, world, world);
            if (result == null)
            {
                return tcs.Task;
            }
            return Task.FromResult(result);
        }

        [UnityHotReloadEditor]
        public static void CleanupStatic()
        {
            world = default;
            tcs = new TaskCompletionSource<World>();
        }
    }
}
