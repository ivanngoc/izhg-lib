using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using static IziHardGames.Apps.ForEcs.ForUnity.EcsSystemAttackWithProjectile;

namespace IziHardGames.Apps.ForEcs.ForUnity
{

	public struct ComponentAttackWithProjectile : IComponentData
    {
        public Entity projectile;
        public float cooldownLeft;
        public float projectileSpeed;
        public bool isFired;
    }

    public struct ComponentAttackSpeed : IComponentData
    {
        public float atacksPerSecond;
    }

    [DisableAutoCreation]
    public partial struct EcsSystemAttackWithProjectile : ISystem
    {
        private void OnCreate(ref SystemState state)
        {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state)
        {
            JobAttackWithProjectile job = new JobAttackWithProjectile()
            {
                deltaTIme = SystemAPI.Time.DeltaTime,
            };
            job.Schedule();
        }

        private void OnDestroy(ref SystemState state)
        {

        }
        [BurstCompile]
        public partial struct JobAttackWithProjectile : IJobEntity
        {
            public float deltaTIme;
            public void Execute(ref ComponentAttackSpeed speed, ref ComponentAttackWithProjectile attack)
            {
                attack.cooldownLeft -= deltaTIme;
                if (attack.isFired)
                {

                }
            }
        }
    }
}
