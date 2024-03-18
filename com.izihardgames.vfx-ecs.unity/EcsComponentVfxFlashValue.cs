using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace IziHardGames.VFX.ForUnity.ForEcs
{
    [DisableAutoCreation]
    public partial struct EcsSystemBlinkByDamageRunVFX : ISystem
    {
        [MaterialProperty("_FlashPerUnit")]
        public struct EcsComponentVfxFlashValue : IComponentData
        {
            public float value01;
        }
        public struct EcsComponentVfxFlashMeta : IComponentData
        {
            public float flashTime;
            public bool isBlinking;
            public bool isTriggered;
        }
        public partial struct JobRunVFX : IJobEntity
        {
            public float deltaTime;
            public void Execute(ref EcsComponentVfxFlashValue flash, ref EcsComponentVfxFlashMeta meta)
            {
                if (meta.isTriggered)
                {
                    meta.isTriggered = false;
                    flash.value01 = 1f;
                    meta.isBlinking = true;
                }
                else if (meta.isBlinking)
                {
                    flash.value01 -= deltaTime / meta.flashTime;
                    if (flash.value01 < math.EPSILON)
                    {
                        meta.isBlinking = false;
                        flash.value01 = 0f;
                    }
                }
            }
        }

        private void OnCreate(ref SystemState state)
        {

        }

        private void OnUpdate(ref SystemState state)
        {
            JobRunVFX job = new JobRunVFX()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.Run();
        }
        private void OnDestroy(ref SystemState state)
        {

        }
    }
}