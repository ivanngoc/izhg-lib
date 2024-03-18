using IziHardGames.Apps.ForEcs.ForUnity;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IziHardGames.ForEcs.Avatar2d.ForUnity
{
    [UpdateBefore(typeof(EcsSystemAttackWithProjectile))]
    [WithAll(typeof(ComponentAvatar2dForEcs))]
    public partial struct EcsSystemAvatarFireWithMB : ISystem
    {
        private void OnCreate(ref SystemState state)
        {

        }

        private void OnUpdate(ref SystemState state)
        {
            foreach (var (avatar, attack, asp, e) in SystemAPI.Query<RefRO<ComponentAvatar2dForEcs>, RefRW<ComponentAttackWithProjectile>, RefRO<ComponentAttackSpeed>>().WithEntityAccess())
            {
                var cooldown = attack.ValueRW.cooldownLeft -= SystemAPI.Time.DeltaTime;
                if (Mouse.current.leftButton.ReadValue() != default)
                {
                    if (cooldown < 0)
                    {
                        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                         .CreateCommandBuffer(state.WorldUnmanaged)
                         .AsParallelWriter();
                        /// <see cref="EcsSystemAttackWithProjectile"/>
                        new JobForFire()
                        {
                            ecb = ecb,
                        }.Schedule();
                        attack.ValueRW.cooldownLeft = asp.ValueRO.atacksPerSecond;
                    }
                }
            }
        }

        private void OnDestroy(ref SystemState state)
        {

        }


        [BurstCompile]
        [WithAll(typeof(ComponentAvatar2dForEcs), typeof(ComponentAttackSpeed))]
        public partial struct JobForFire : IJobEntity
        {
            internal EntityCommandBuffer.ParallelWriter ecb;

            [BurstCompile]
            public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref ComponentAttackWithProjectile attack, ref LocalTransform gunLocalTransform, in LocalToWorld gunTransform)
            {
                var projectile = ecb.Instantiate(chunkIndexInQuery, attack.projectile);

                LocalTransform localTransform = LocalTransform.FromPositionRotationScale(
                        gunLocalTransform.Position + gunTransform.Up,
                        gunLocalTransform.Rotation,
                        gunLocalTransform.Scale);

                ecb.SetComponent(chunkIndexInQuery, projectile, localTransform);
                ecb.SetComponent(chunkIndexInQuery, projectile, new EcsComponentProjectile() { timeLife = 5f });
                ecb.SetComponent(chunkIndexInQuery, projectile, new PhysicsVelocity() { Linear = gunTransform.Up * attack.projectileSpeed, Angular = float3.zero });
            }
        }
    }
}
