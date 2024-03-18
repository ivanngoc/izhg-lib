using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace IziHardGames.TwoD.TopView.ForUnity.ForEcs
{
    public partial struct SystemMoveByPhysicsPush : ISystem
    {
        public struct ComponentPhysicsPush : IComponentData
        {


        }
        [BurstCompile]
        public partial struct JobMoveByPhysicsPush : IJobEntity
        {
            public void Execute(ref LocalTransform transform)
            {
                
            }
        }

        private void OnCreate(ref SystemState state)
        {

        }

        private void OnUpdate(ref SystemState state)
        {
            JobMoveByPhysicsPush job = new JobMoveByPhysicsPush();
            job.ScheduleParallel();
        }

        private void OnDestroy(ref SystemState state)
        {

        }
    }
}
