using System;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace IziHardGames.UserControl.ForUnity.ForEcs
{
    public struct EcsPointerRaycastData : IComponentData
    {
        public NativeList<RaycastHit> result;
        public int frames;
    }

    [DisableAutoCreation]
    public partial struct EcsPhysicsRaycastSystem : ISystem
    {
        private Entity dataEntity;
        private void OnCreate(ref SystemState state)
        {
            this.dataEntity = state.EntityManager.CreateSingleton<EcsPointerRaycastData>();
        }

        private void OnUpdate(ref SystemState state)
        {
            var env = IziEnvironment.Environments!.Current ?? throw new NullReferenceException();
            var pos = env.GetViewerPositionXYZ();
            float3 start = new float3((float)pos.Item1, (float)pos.Item2, (float)pos.Item3);
            var distance = (float)env.DetectionDistance;
            if (distance < math.EPSILON)
            {
                throw new ArgumentException($"Raycast dir is less than minimal. Value:{distance}");
            }
            float3 end = ((float3)DependeciesForUserControlEcs.DataInput!.rayMainCameraToPointer.direction * distance) + start;
            Debug.DrawLine(start, end, Color.cyan);
            var results = Raycast(start, end, ConstantsForUserControl.LAYER_RAYCASTS, ConstantsForUserControl.GROUP_HIT_BY_RAY, default);
            SystemAPI.SetSingleton(new EcsPointerRaycastData() { result = results, frames = Time.frameCount });

            foreach (var item in SystemAPI.Query<RefRW<URPMaterialPropertyBaseColor>>())
            {
                item.ValueRW.Value = new float4(Color.white.r, Color.white.g, Color.white.b, Color.white.a);
            }
            foreach (var hit in results)
            {
                var type = ComponentType.ReadWrite(typeof(URPMaterialPropertyBaseColor));
                if (state.EntityManager.HasComponent(hit.Entity, type))
                {
                    var comp = new URPMaterialPropertyBaseColor()
                    {
                        Value = new float4(Color.red.r, Color.red.g, Color.red.b, Color.red.a),
                    };
                    state.EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hit.Entity, comp);
                }
            }
        }

        private void OnDestroy(ref SystemState state)
        {

        }

        public NativeList<RaycastHit> Raycast(float3 RayFrom, float3 RayTo, uint belongsTo, uint collidesWith, int groupIndex)
        {
            PhysicsWorldSingleton collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            RaycastInput input = new RaycastInput()
            {
                Start = RayFrom,
                End = RayTo,
                //Filter = CollisionFilter.Default,

                Filter = new CollisionFilter()
                {
                    BelongsTo = belongsTo, // ~0u,
                    CollidesWith = collidesWith, // ~0u, all 1s, so all layers, collide with everything
                    /// https://docs.unity3d.com/Packages/com.unity.physics@1.2/api/Unity.Physics.CollisionFilter.html#fields
                    GroupIndex = groupIndex,
                }
            };
            NativeList<RaycastHit> hits = new NativeList<RaycastHit>(Allocator.Temp);
            bool haveHit = collisionWorld.CastRay(input, ref hits);
            return hits;
        }
    }
}
