using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace IziHardGames.Apps.ForEcs.ForUnity
{
    public class EcsProjectileAuthoring : MonoBehaviour
    {
        internal class EcsProjectileBaker : Baker<EcsProjectileAuthoring>
        {
            public override void Bake(EcsProjectileAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EcsComponentProjectile() { timeLife = 5 });
                AddComponent(entity, new EcscCollisionTriggerEvents());
            }
        }
    }

    public struct EcsComponentProjectile : IComponentData
    {
        public float timeLife;
        // 
        //public float speed;
    }
    public partial struct EcsSystemProjectileLifeControl : ISystem
    {
        [BurstCompile]
        public partial struct JobLifeWatchdog : IJobEntity
        {
            internal float deltaTime;
            internal EntityCommandBuffer.ParallelWriter ecb;
            public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref EcsComponentProjectile projectile, in Entity entity)
            {
                projectile.timeLife -= deltaTime;
                if (projectile.timeLife < math.EPSILON)
                {
                    ecb.DestroyEntity(chunkIndexInQuery, entity);
                }
            }
        }

        private void OnCreate(ref SystemState state)
        {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
         .CreateCommandBuffer(state.WorldUnmanaged)
         .AsParallelWriter();

            JobLifeWatchdog job = new JobLifeWatchdog()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = ecb,
            };
            job.Schedule();
        }

        private void OnDestroy(ref SystemState state)
        {

        }
    }
    [DisableAutoCreation]
    public partial struct EcsSystemProjectileMoveAndLifeControl : ISystem
    {
        [BurstCompile]
        public partial struct JobMoveProjectilePhysics : IJobEntity
        {
            internal float deltaTime;
            internal EntityCommandBuffer.ParallelWriter ecb;

            public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref PhysicsVelocity v, ref EcsComponentProjectile projectile, in Entity entity)
            {
                projectile.timeLife -= deltaTime;

                if (projectile.timeLife > 0)
                {

                }
                else
                {
                    ecb.DestroyEntity(chunkIndexInQuery, entity);
                }
            }
        }
        [BurstCompile]
        public partial struct JobMoveProjectile : IJobEntity
        {
            internal float deltaTime;
            internal EntityCommandBuffer.ParallelWriter ecb;

            public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref LocalTransform localTransform, ref EcsComponentProjectile projectile, ref EcsComponentMoveByAsignSpeed speed, in Entity entity)
            {
                projectile.timeLife -= deltaTime;
                if (projectile.timeLife > 0)
                {
                    localTransform.Position += localTransform.Up() * (deltaTime * speed.speed);
                }
                else
                {
                    ecb.DestroyEntity(chunkIndexInQuery, entity);
                }
            }
        }

        private void OnCreate(ref SystemState state)
        {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged)
            .AsParallelWriter();

            JobMoveProjectile job = new JobMoveProjectile()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = ecb,
            };
            job.ScheduleParallel();
        }

        private void OnDestroy(ref SystemState state)
        {

        }
    }
}
