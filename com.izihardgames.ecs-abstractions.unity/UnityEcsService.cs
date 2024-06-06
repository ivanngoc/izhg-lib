using System.Threading.Tasks;
using System;
using IziHardGames.Apps.Abstractions.Lib;
using Unity.Entities;

namespace IziHardGames.Apps.ForEcs.Abstractions.ForUnity
{
    public class UnityEcsService : IIziService
    {
        private World? world;
        private bool isRunning;
        private bool isEcsWorldCreated;

        public UnityEcsService()
        {

        }

        public void Start()
        {
            isRunning = true;
            if (IziUnityEcs.TryGetDefaultWorld(out World world))
            {
                Run(world);
            }
            else
            {
                IziUnityEcs.OnWorldCreated(Run);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }

        private void Run(World world)
        {
            this.world = world;
            isEcsWorldCreated = true;
        }

        public void TurnSystemBaseOn<T>(ComponentSystemGroup group) where T : SystemBase, new()
        {
            var handle = world!.CreateSystem<T>();
            group.AddSystemToUpdateList(handle);
        }
        public SystemHandle TurnSystemBaseOn<T>() where T : SystemBase, new()
        {
            var handle = world!.CreateSystem<T>();
            var groupe = world!.GetOrCreateSystemManaged<Unity.Entities.SimulationSystemGroup>();
            groupe.AddSystemToUpdateList(handle);
            return handle;
        }
        public void TurnSystemBaseOff<T>() where T : SystemBase
        {
            throw new System.NotImplementedException();
        }
        public SystemHandle TurnISystemOn<T>() where T : ISystem
        {
            var handle = world!.GetOrCreateSystem(typeof(T));
            var groupe = world!.GetOrCreateSystemManaged<Unity.Entities.SimulationSystemGroup>();
            groupe.AddSystemToUpdateList(handle);
            return handle;
        }
        public void TurnISystemOff<T>() where T : ISystem
        {
            throw new System.NotImplementedException();
        }

        private void EnsureInitilized()
        {
            throw new System.NotImplementedException();
        }

        public async Task AwaitInitilization()
        {
            if (!isRunning) throw new InvalidOperationException($"Service must running! You must call that method after {nameof(IIziAppVersion1)}.{nameof(IIziAppVersion1.StartAsync)}() is Finished");
            await IziUnityEcs.AwaitDefaultWorldCreation().ConfigureAwait(false);
        }
    }
}
