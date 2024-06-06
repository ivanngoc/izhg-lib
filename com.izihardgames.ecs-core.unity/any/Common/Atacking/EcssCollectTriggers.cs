using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace IziHardGames.Apps.ForEcs.ForUnity
{
    public struct EcscCollisionTriggerEvents : IComponentData
    {
        public Entity target;
        public bool isHit;
    }

    public struct EcsComponentDestroyMarker : IComponentData
    {

    }

    public readonly struct Pair : IBufferElementData
    {
        public readonly Entity a;
        public readonly Entity b;

        public Pair(Entity entityA, Entity entityB)
        {
            this.a = entityA;
            this.b = entityB;
        }
    }

    [DisableAutoCreation]
    public partial struct EcssCollectTriggers : ISystem
    {
        //[BurstCompile]
        private struct EcsJobCollectTriggers : ITriggerEventsJob
        {
            public ComponentLookup<EcscCollisionTriggerEvents> lookup;
            public void Execute(TriggerEvent triggerEvent)
            {
                if (lookup.HasComponent(triggerEvent.EntityA) && lookup.HasComponent(triggerEvent.EntityB))
                {
                    var compA = lookup.GetRefRW(triggerEvent.EntityA);
                    compA.ValueRW.target = triggerEvent.EntityB;
                    compA.ValueRW.isHit = true;

                    var compB = lookup.GetRefRW(triggerEvent.EntityB);
                    compB.ValueRW.target = triggerEvent.EntityA;
                    compB.ValueRW.isHit = true;
                }
            }
        }
        private void OnCreate(ref SystemState state)
        {

        }

        private void OnUpdate(ref SystemState state)
        {
            state.Dependency = new EcsJobCollectTriggers()
            {
                lookup = SystemAPI.GetComponentLookup<EcscCollisionTriggerEvents>(false),
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        private void OnDestroy(ref SystemState state)
        {

        }
    }
}
