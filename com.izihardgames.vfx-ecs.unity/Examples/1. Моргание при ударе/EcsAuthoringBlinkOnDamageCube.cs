using System;
using System.Collections;
using System.Collections.Generic;
using IziHardGames.VFX.ForUnity.ForEcs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using static IziHardGames.VFX.ForEcs.Examples.BlinkOnDamage.EcsSystemBlinkOnDamageRotate;
using static IziHardGames.VFX.ForUnity.ForEcs.EcsSystemBlinkByDamageRunVFX;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using Random = Unity.Mathematics.Random;


namespace IziHardGames.VFX.ForEcs.Examples.BlinkOnDamage
{
    internal class EcsAuthoringBlinkOnDamageCube : MonoBehaviour
    {
        [SerializeField] public float speed = 1;
        [SerializeField] private float cooldownByClick = 0.1f;
        [SerializeField] private float flashTime = 0.5f;

        internal class ExsBakerBlinkOnDamageCube : Baker<EcsAuthoringBlinkOnDamageCube>
        {
            public override void Bake(EcsAuthoringBlinkOnDamageCube authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(e, new EcsComponentVfxFlashValue());
                AddComponent(e, new EcsComponentVfxFlashMeta()
                {
                    flashTime = authoring.flashTime,
                });
                AddComponent(e, new EcsComponentRotateExample()
                {
                    speed = authoring.speed,
                    cooldownByClick = authoring.cooldownByClick,
                    random = new Random((uint)Guid.NewGuid().GetHashCode()),
                });
            }
        }
    }

    [DisableAutoCreation]
    internal partial struct EcsSystemBlinkOnDamageRotate : ISystem
    {
        internal struct EcsComponentRotateExample : IComponentData
        {
            internal float speed;
            internal float timeLeftToBlink;

            internal float cooldownByClick;
            internal float cooldownByClickLeft;
            public const float cooldown = 5f;
            public Random random;
        }

        internal partial struct EcsJobRotate : IJobEntity
        {
            public float time;
            public void Execute(ref LocalTransform localTransform, in EcsComponentRotateExample marker)
            {
                localTransform = localTransform.RotateY(time * marker.speed);
            }
        }

        private void OnCreate(ref SystemState state)
        {

        }

        private void OnUpdate(ref SystemState state)
        {
            EcsJobRotate job = new EcsJobRotate()
            {
                time = (float)SystemAPI.Time.DeltaTime,
            };
            job.Schedule();
        }

        private void OnDestroy(ref SystemState state)
        {

        }
    }
    [DisableAutoCreation]
    internal partial struct EcsSystemBlinkOnDamageSimmulateHit : ISystem
    {
        public partial struct JobHitByClick : IJobEntity
        {
            public float deltaTime;
            internal bool isCLicked;

            public void Execute(ref EcsComponentRotateExample marker, ref EcsComponentVfxFlashValue flash, ref EcsComponentVfxFlashMeta meta)
            {
                marker.cooldownByClickLeft -= deltaTime;
                if (isCLicked && marker.cooldownByClickLeft < math.EPSILON)
                {
                    meta.isTriggered = true;
                    marker.cooldownByClickLeft = marker.cooldownByClick;
                }
            }
        }

        public partial struct JobHitSimmulate : IJobEntity
        {
            internal float time;
            public void Execute(ref EcsComponentRotateExample marker, ref EcsComponentVfxFlashValue flash, ref EcsComponentVfxFlashMeta meta)
            {
                if (!meta.isTriggered)
                {
                    marker.timeLeftToBlink -= time;
                    if (marker.timeLeftToBlink < 0)
                    {
                        meta.isTriggered = true;
                        marker.timeLeftToBlink = EcsComponentRotateExample.cooldown;
                    }
                }
            }
        }

        private void OnCreate(ref SystemState state)
        {

        }

        private void OnUpdate(ref SystemState state)
        {
            state.Dependency = new JobHitSimmulate()
            {
                time = SystemAPI.Time.DeltaTime,
            }.Schedule(state.Dependency);

            state.Dependency = new JobHitByClick()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                isCLicked = Mouse.current.leftButton.ReadValue() != default,
            }.Schedule(state.Dependency);
        }

        private void OnDestroy(ref SystemState state)
        {

        }
    }

}