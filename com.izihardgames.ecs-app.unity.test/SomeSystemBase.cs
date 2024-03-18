using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace IziHardGames.Apps.ECS
{
    [DisableAutoCreation]
    public partial class SomeSystemBase : SystemBase
    {
        protected override void OnUpdate()
        {
            var prefab = new Entity();
            // Must be reference To Unity.Collections.dll
            EntityCommandBuffer buffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
            Entity spawnedEntity = buffer.Instantiate(prefab);


            // на каждый поток по одному итератору
            new SomeJob0() { }.ScheduleParallel();
        }

        private void GetSpecificComponent(Entity entity)
        {
            var b = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<SomeComponentData>(entity);
        }
        private Entity GetRandomEntity()
        {
            EntityQuery entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(SomeComponentData));
            NativeArray<Entity> entityNativeArray = entityQuery.ToEntityArray(global::Unity.Collections.Allocator.Temp);
            return Entity.Null;
        }
    }

    public partial struct SomeJob0 : IJobEntity
    {
        public void Execute()
        {
            //World.Active
        }
    }

    public struct SomeComponentData : IComponentData
    {

    }
}
